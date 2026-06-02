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
    throw new Error(`Chrome/Chromium executable not found. Set CHROME_PATH to run browser ${options.mode} QA.`);
  }

  const url = qaUrl();
  try {
    await runPipeQa(chromePath, url);
  } catch (error) {
    if (!isPipeTargetTimeout(error)) throw error;
    console.error(`Chrome CDP pipe target lookup failed; retrying with remote debugging port: ${error.message}`);
    await runPortQa(chromePath, url);
  }
}

async function runPipeQa(chromePath, url) {
  const userDataDir = fs.mkdtempSync(path.join(os.tmpdir(), `lethe-${options.mode}-qa-`));
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
              const raw = document.documentElement.dataset.${datasetName()} || "";
              let qa = null;
              try { qa = raw ? JSON.parse(raw) : null; } catch {}
              if (qa && (qa.status === "complete" || qa.status === "failed")) {
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
      const failures = qaFailures(qa, value);
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

async function runPortQa(chromePath, url) {
  if (typeof WebSocket !== 'function') {
    throw new Error('Node WebSocket client is not available for remote debugging port fallback.');
  }

  const userDataDir = fs.mkdtempSync(path.join(os.tmpdir(), `lethe-${options.mode}-qa-port-`));
  const port = await findOpenPort();
  const chrome = spawn(chromePath, [
    '--headless=new',
    '--disable-gpu',
    '--disable-extensions',
    '--no-first-run',
    '--no-default-browser-check',
    `--remote-debugging-port=${port}`,
    `--user-data-dir=${userDataDir}`,
    url,
  ], {
    stdio: ['ignore', 'ignore', 'pipe'],
  });

  let stderr = '';
  chrome.stderr.on('data', (chunk) => {
    stderr += chunk.toString();
  });

  try {
    try {
      const target = await waitForPortPageTarget(port, options.timeoutMs);
      const cdp = await WebSocketCdpClient.connect(target.webSocketDebuggerUrl);
      try {
        await cdp.send('Runtime.enable');
        await evaluateQa(cdp, '', url, chromePath);
      } finally {
        cdp.close();
      }
    } catch (error) {
      if (stderr.trim()) console.error(stderr.trim().split('\n').slice(-8).join('\n'));
      throw error;
    }
  } finally {
    chrome.kill('SIGKILL');
    fs.rmSync(userDataDir, { recursive: true, force: true });
  }
}

async function evaluateQa(cdp, sessionId, url, chromePath) {
  const result = await cdp.send('Runtime.evaluate', {
    awaitPromise: true,
    returnByValue: true,
    expression: `
      new Promise((resolve) => {
        const deadline = Date.now() + ${Math.max(500, options.timeoutMs - 1000)};
        const read = () => {
          const raw = document.documentElement.dataset.${datasetName()} || "";
          let qa = null;
          try { qa = raw ? JSON.parse(raw) : null; } catch {}
          if (qa && (qa.status === "complete" || qa.status === "failed")) {
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
  const failures = qaFailures(qa, value);
  const output = {
    url,
    chromePath,
    status: failures.length ? 'failed' : 'complete',
    qa,
    failures,
  };
  console.log(JSON.stringify(output, null, 2));
  if (failures.length) process.exitCode = 1;
}

function qaFailures(qa, value) {
  const checks = options.mode === 'postloss' ? postLossChecks(qa) : pressureChecks(qa);
  const failures = checks.filter(([, passed]) => !passed).map(([name]) => name);
  if (value.timedOut) failures.push(`timed out waiting for ${options.mode} QA dataset`);
  return failures;
}

function pressureChecks(qa) {
  return [
    ['status complete', qa.status === 'complete'],
    ['version v0.9', qa.version === 'v0.9'],
    ['browser state exists', qa.hasState],
    ['lull segment exists', qa.hasLull],
    ['rising segment exists', qa.hasRising],
    ['climax segment exists', qa.hasClimax],
    ['pressure segments payload exists', (qa.pressureSegments || []).length >= 3],
  ];
}

function postLossChecks(qa) {
  return [
    ['status complete', qa.status === 'complete'],
    ['version v0.9', qa.version === 'v0.9'],
    ['browser state exists', qa.hasState],
    ['post-loss challenge payload exists', qa.postLossChallengeCount >= 1],
    ['deficit breath segment exists', qa.hasDeficitBreath],
    ['deficit trial segment exists', qa.hasDeficitTrial],
    ['challenge completed', qa.challengeCompleted],
    ['challenge survived', qa.challengeSurvived],
    ['refill restored 3 active memories', qa.activeMemoryCount === 3],
    ['danger post-loss completion counted', (qa.danger?.postLossChallengeCompletions || 0) >= 1],
  ];
}

function qaUrl() {
  const file = pathToFileURL(path.resolve('index.html'));
  const qaName = options.mode === 'postloss' ? 'postloss' : 'pressure';
  const session = options.mode === 'postloss' ? 'POSTLOSS' : 'PRESSURE';
  file.search = `?qa=fast,${qaName}&tester=QA&session=${session}`;
  return file.href;
}

function datasetName() {
  return options.mode === 'postloss' ? 'lethePostLossQa' : 'lethePressureQa';
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

async function waitForPortPageTarget(port, timeoutMs) {
  const deadline = Date.now() + timeoutMs;
  let lastError = null;
  while (Date.now() < deadline) {
    try {
      const targets = await fetchJson(`http://127.0.0.1:${port}/json/list`);
      const page = targets.find((target) => target.type === 'page' && target.url.includes('index.html'));
      if (page?.webSocketDebuggerUrl) return page;
    } catch (error) {
      lastError = error;
    }
    await sleep(100);
  }
  throw new Error(`Timed out waiting for Chrome port page target${lastError ? `: ${lastError.message}` : ''}`);
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

class WebSocketCdpClient {
  constructor(socket) {
    this.socket = socket;
    this.nextId = 1;
    this.pending = new Map();

    socket.addEventListener('message', (event) => this.onMessage(event));
    socket.addEventListener('error', () => {
      this.rejectAll(new Error('Chrome DevTools WebSocket errored before a response was received.'));
    });
    socket.addEventListener('close', () => {
      this.rejectAll(new Error('Chrome DevTools WebSocket closed before a response was received.'));
    });
  }

  static connect(url) {
    return new Promise((resolve, reject) => {
      const socket = new WebSocket(url);
      const timeout = setTimeout(() => {
        socket.close();
        reject(new Error('Timed out opening Chrome DevTools WebSocket.'));
      }, options.timeoutMs);
      socket.addEventListener('open', () => {
        clearTimeout(timeout);
        resolve(new WebSocketCdpClient(socket));
      }, { once: true });
      socket.addEventListener('error', () => {
        clearTimeout(timeout);
        reject(new Error('Failed to open Chrome DevTools WebSocket.'));
      }, { once: true });
    });
  }

  send(method, params = {}, sessionId = '') {
    const id = this.nextId++;
    const message = sessionId ? { id, method, params, sessionId } : { id, method, params };
    this.socket.send(JSON.stringify(message));
    return new Promise((resolve, reject) => {
      const timeout = setTimeout(() => {
        this.pending.delete(id);
        reject(new Error(`Timed out waiting for CDP response to ${method}`));
      }, options.timeoutMs);
      this.pending.set(id, { resolve, reject, timeout });
    });
  }

  close() {
    this.socket.close();
  }

  onMessage(event) {
    const message = JSON.parse(event.data.toString());
    if (!message.id || !this.pending.has(message.id)) return;
    const pending = this.pending.get(message.id);
    this.pending.delete(message.id);
    clearTimeout(pending.timeout);
    if (message.error) pending.reject(new Error(JSON.stringify(message.error)));
    else pending.resolve(message.result);
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

async function findOpenPort() {
  return 40000 + Math.floor(Math.random() * 20000);
}

async function fetchJson(url) {
  const controller = new AbortController();
  const timeout = setTimeout(() => controller.abort(), Math.min(1000, options.timeoutMs));
  try {
    const response = await fetch(url, { signal: controller.signal });
    if (!response.ok) throw new Error(`${response.status} ${response.statusText}`);
    return await response.json();
  } finally {
    clearTimeout(timeout);
  }
}

function isPipeTargetTimeout(error) {
  return error.message.includes('Timed out waiting for Chrome page target')
    || error.message.includes('Timed out waiting for CDP response to Target.getTargets');
}

function parseArgs(args) {
  const timeoutArg = valueAfter(args, '--timeout-ms');
  const modeArg = valueAfter(args, '--mode') || 'pressure';
  if (!['pressure', 'postloss'].includes(modeArg)) {
    throw new Error(`Unknown QA mode: ${modeArg}. Use pressure or postloss.`);
  }
  return {
    timeoutMs: timeoutArg ? Number(timeoutArg) : 8000,
    mode: modeArg,
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
