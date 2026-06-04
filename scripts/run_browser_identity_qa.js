#!/usr/bin/env node
'use strict';

const fs = require('fs');
const os = require('os');
const path = require('path');
const { pathToFileURL } = require('url');
const { spawn, spawnSync } = require('child_process');

const options = parseArgs(process.argv.slice(2));

async function main() {
  const chromePath = findChrome();
  if (!chromePath) {
    throw new Error('Chrome/Chromium executable not found. Set CHROME_PATH to run browser identity QA.');
  }

  const userDataDir = fs.mkdtempSync(path.join(os.tmpdir(), 'lethe-identity-qa-'));
  const url = identityQaUrl();
  const chrome = spawn(chromePath, [
    '--headless=new',
    '--disable-gpu',
    '--disable-extensions',
    '--no-first-run',
    '--no-default-browser-check',
    '--remote-debugging-pipe',
    `--user-data-dir=${userDataDir}`,
    url,
  ], {
    stdio: ['ignore', 'ignore', 'pipe', 'pipe', 'pipe'],
  });

  let stderr = '';
  chrome.stderr.on('data', (chunk) => {
    stderr += chunk.toString();
  });

  try {
      const cdp = new PipeCdpClient(chrome.stdio[3], chrome.stdio[4]);
    try {
      const target = await waitForPipePageTarget(cdp, options.timeoutMs);
      const attached = await cdp.send('Target.attachToTarget', {
        targetId: target.targetId,
        flatten: true,
      });
      const sessionId = attached.sessionId;
      await cdp.send('Runtime.enable', {}, sessionId);
      const result = await cdp.send('Runtime.evaluate', {
        awaitPromise: true,
        returnByValue: true,
        expression: `
          new Promise((resolve) => {
            const deadline = Date.now() + ${Math.max(500, options.timeoutMs - 1000)};
            const read = () => {
              const raw = document.documentElement.dataset.letheIdentityQa || "";
              let qa = null;
              try { qa = raw ? JSON.parse(raw) : null; } catch {}
              if (qa && (qa.status === "complete" || qa.status === "failed" || (
                qa.buildNameVisible
                && qa.synergyVisible
                && qa.dependencyVisible
                && qa.hasBuildIdentityPayload
                && qa.buildIdentitySeenBy90Sec
              ))) {
                resolve({ qa, raw, text: document.body.innerText });
                return;
              }
              if (Date.now() >= deadline) {
                resolve({ qa, raw, text: document.body.innerText, timedOut: true });
                return;
              }
              setTimeout(read, 100);
            };
            read();
          })
        `,
      }, sessionId);
      const value = result.result?.value || {};
      const qa = value.qa || {};
      const failures = identityFailures(qa, value);
      const output = {
        url,
        chromePath,
        status: failures.length ? 'failed' : 'complete',
        qa,
        failures,
      };
      console.log(JSON.stringify(output, null, 2));
      if (failures.length) process.exitCode = 1;
    } finally {
      cdp.close();
    }
  } catch (error) {
    if (stderr.trim()) console.error(stderr.trim().split('\n').slice(-8).join('\n'));
    throw error;
  } finally {
    chrome.kill('SIGKILL');
    fs.rmSync(userDataDir, { recursive: true, force: true });
  }
}

function identityFailures(qa, value) {
  const coreComplete = Boolean(
    qa.buildNameVisible
    && qa.synergyVisible
    && qa.dependencyVisible
    && qa.hasBuildIdentityPayload
    && qa.buildIdentitySeenBy90Sec
    && qa.buildIdentity?.buildName
    && (qa.buildIdentity?.activeSynergies || []).length > 0
  );
  const checks = [
    ['status complete or core identity visible', qa.status === 'complete' || coreComplete],
    ['version v0.10', qa.version === 'v0.10'],
    ['browser state exists', qa.hasState],
    ['build name visible', qa.buildNameVisible],
    ['synergy visible', qa.synergyVisible],
    ['dependent memory visible', qa.dependencyVisible],
    ['buildIdentity payload exists', qa.hasBuildIdentityPayload],
    ['buildIdentitySeenBy90Sec true', qa.buildIdentitySeenBy90Sec],
    ['build name payload exists', Boolean(qa.buildIdentity?.buildName)],
    ['active synergy payload exists', (qa.buildIdentity?.activeSynergies || []).length > 0],
  ];
  const failures = checks.filter(([, passed]) => !passed).map(([name]) => name);
  if (value.timedOut && !coreComplete) failures.push('timed out waiting for identity QA dataset');
  return failures;
}

function identityQaUrl() {
  const file = pathToFileURL(path.resolve('index.html'));
  file.search = '?qa=fast,identity&tester=QA&session=IDENTITY';
  return file.href;
}

async function waitForPipePageTarget(cdp, timeoutMs) {
  const deadline = Date.now() + timeoutMs;
  let lastError = null;
  while (Date.now() < deadline) {
    try {
      const result = await cdp.send('Target.getTargets');
      const page = (result.targetInfos || []).find((target) => target.type === 'page' && target.url.includes('index.html'));
      if (page?.targetId) return page;
    } catch (error) {
      lastError = error;
    }
    await sleep(100);
  }
  throw new Error(`Timed out waiting for Chrome page target${lastError ? `: ${lastError.message}` : ''}`);
}

class PipeCdpClient {
  constructor(input, output) {
    this.input = input;
    this.output = output;
    this.nextId = 1;
    this.pending = new Map();
    this.buffer = Buffer.alloc(0);

    output.on('data', (chunk) => this.onData(chunk));
    output.on('error', (error) => {
      this.rejectAll(error);
    });
    output.on('close', () => {
      this.rejectAll(new Error('Chrome DevTools pipe closed before a response was received.'));
    });
    input.on('error', (error) => {
      this.rejectAll(error);
    });
  }

  send(method, params = {}, sessionId = '') {
    const id = this.nextId++;
    const message = sessionId ? { id, method, params, sessionId } : { id, method, params };
    this.input.write(`${JSON.stringify(message)}\0`);
    return new Promise((resolve, reject) => {
      const timeout = setTimeout(() => {
        this.pending.delete(id);
        reject(new Error(`Timed out waiting for CDP response to ${method}`));
      }, options.timeoutMs);
      this.pending.set(id, { resolve, reject, timeout });
    });
  }

  close() {
    this.input.destroy();
    this.output.destroy();
  }

  onData(chunk) {
    this.buffer = Buffer.concat([this.buffer, chunk]);
    while (this.buffer.length) {
      const end = this.buffer.indexOf(0);
      if (end === -1) return;
      const raw = this.buffer.slice(0, end).toString('utf8');
      this.buffer = this.buffer.slice(end + 1);
      if (!raw.trim()) continue;
      const message = JSON.parse(raw);
      if (!message.id || !this.pending.has(message.id)) continue;
      const pending = this.pending.get(message.id);
      this.pending.delete(message.id);
      clearTimeout(pending.timeout);
      if (message.error) pending.reject(new Error(JSON.stringify(message.error)));
      else pending.resolve(message.result);
    }
  }

  rejectAll(error) {
    for (const { reject, timeout } of this.pending.values()) {
      clearTimeout(timeout);
      reject(error);
    }
    this.pending.clear();
  }
}

function findChrome() {
  if (process.env.CHROME_PATH && fs.existsSync(process.env.CHROME_PATH)) return process.env.CHROME_PATH;
  const candidates = [
    '/Applications/Google Chrome.app/Contents/MacOS/Google Chrome',
    '/Applications/Chromium.app/Contents/MacOS/Chromium',
    '/Applications/Google Chrome Canary.app/Contents/MacOS/Google Chrome Canary',
    '/Applications/Microsoft Edge.app/Contents/MacOS/Microsoft Edge',
    'google-chrome',
    'google-chrome-stable',
    'chromium',
    'chromium-browser',
    'microsoft-edge',
  ];
  for (const candidate of candidates) {
    if (candidate.includes(path.sep) && fs.existsSync(candidate)) return candidate;
    if (!candidate.includes(path.sep)) {
      const found = spawnSync('command', ['-v', candidate], { shell: true, encoding: 'utf8' });
      if (found.status === 0 && found.stdout.trim()) return found.stdout.trim();
    }
  }
  return '';
}

function parseArgs(args) {
  const timeoutArg = valueAfter(args, '--timeout-ms');
  return {
    timeoutMs: timeoutArg ? Number(timeoutArg) : 8000,
  };
}

function valueAfter(args, name) {
  const index = args.indexOf(name);
  return index === -1 ? '' : args[index + 1];
}

function sleep(ms) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}

main().catch((error) => {
  console.error(error.stack || error.message);
  process.exit(1);
});
