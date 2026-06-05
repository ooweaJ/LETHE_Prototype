#!/usr/bin/env node
'use strict';

const fs = require('fs');
const os = require('os');
const path = require('path');
const net = require('net');
const { pathToFileURL } = require('url');
const { spawn } = require('child_process');

const options = parseArgs(process.argv.slice(2));

main().catch((error) => {
  console.error(error.stack || error.message);
  process.exit(1);
});

async function main() {
  if (options.dryRun) {
    console.log('LETHE browser balance QA dry-run');
    console.log(`- runs: ${options.runs}`);
    console.log(`- out: ${options.outDir}`);
    console.log(`- target first boss TTK: ${options.firstBossTtkMin}-${options.firstBossTtkMax}s`);
    console.log(`- target clear rate: ${options.clearRateMin}-${options.clearRateMax}`);
    console.log(`- target level-ups before first boss: >= ${options.levelUpsBeforeFirstBossMin}`);
    return;
  }

  fs.mkdirSync(options.outDir, { recursive: true });
  const chromePath = findChrome();
  if (!chromePath) {
    const blocked = {
      status: 'blocked',
      reason: 'Chrome/Chromium executable not found. Set CHROME_PATH to run browser balance QA.',
      nextCommand: 'Set CHROME_PATH, then run npm run qa:balance.',
      generatedAt: new Date().toISOString(),
    };
    writeJson(path.join(options.outDir, 'summary.json'), blocked);
    writeJson(path.join(options.outDir, 'latest.json'), blocked);
    throw new Error(blocked.reason);
  }

  const runs = [];
  for (let index = 0; index < options.runs; index += 1) {
    let qa = null;
    try {
      qa = await runSingleBalanceQaWithRetries(chromePath, index + 1);
    } catch (error) {
      qa = {
        status: 'browser_error',
        runResult: 'browser_error',
        error: error.message,
      };
      console.error(`[balance ${index + 1}/${options.runs}] browser_error: ${firstLine(error.message)}`);
    }
    const normalized = normalizeRun(qa, index + 1);
    runs.push(normalized);
    writeJson(path.join(options.outDir, `run-${String(index + 1).padStart(2, '0')}.json`), normalized);
    console.log(`[balance ${index + 1}/${options.runs}] ${normalized.runResult} firstBoss=${normalized.firstBossTtk ?? '-'}s clear=${normalized.finalClear}`);
  }

  const summary = summarizeRuns(runs);
  writeJson(path.join(options.outDir, 'summary.json'), summary);
  writeJson(path.join(options.outDir, 'latest.json'), summary);
  fs.mkdirSync(path.dirname(options.reportPath), { recursive: true });
  fs.writeFileSync(options.reportPath, markdownReport(summary), 'utf8');

  console.log('');
  console.log(`Verdict: ${summary.verdict}`);
  console.log(`First boss clear: ${pct(summary.metrics.firstBossClearRate)} / full clear: ${pct(summary.metrics.clearRate)} / death: ${pct(summary.metrics.deathRate)}`);
  console.log(`First boss TTK median: ${fmt(summary.metrics.firstBossTtkMedian)}s`);
  console.log(`Level-ups before first boss median: ${fmt(summary.metrics.levelUpsBeforeFirstBossMedian)}`);
  console.log(`Top DPS share median: ${pct(summary.metrics.topDpsShareMedian)}`);
  console.log(`Report: ${path.relative(process.cwd(), options.reportPath)}`);
}

async function runSingleBalanceQaWithRetries(chromePath, runNumber) {
  let lastError = null;
  for (let attempt = 0; attempt <= options.retries; attempt += 1) {
    try {
      const qa = await runSingleBalanceQa(chromePath, runNumber);
      if (attempt > 0) qa.retryAttempt = attempt;
      return qa;
    } catch (error) {
      lastError = error;
      if (attempt >= options.retries) break;
      console.error(`[balance ${runNumber}/${options.runs}] retry ${attempt + 1}/${options.retries}: ${firstLine(error.message)}`);
      await sleep(750);
    }
  }
  throw lastError;
}

async function runSingleBalanceQa(chromePath, runNumber) {
  const userDataDir = fs.mkdtempSync(path.join(os.tmpdir(), `lethe-balance-qa-${runNumber}-`));
  const port = await findOpenPort();
  const url = qaUrl(runNumber);
  const chrome = spawn(chromePath, [
    '--headless=new',
    '--disable-gpu',
    '--disable-extensions',
    '--disable-dev-shm-usage',
    '--disable-background-timer-throttling',
    '--disable-backgrounding-occluded-windows',
    '--disable-renderer-backgrounding',
    '--disable-features=CalculateNativeWinOcclusion',
    '--no-sandbox',
    '--no-first-run',
    '--no-default-browser-check',
    `--user-data-dir=${userDataDir}`,
    `--remote-debugging-port=${port}`,
    url,
  ], {
    stdio: ['ignore', 'ignore', 'pipe'],
  });

  let stderr = '';
  chrome.stderr.on('data', (chunk) => {
    stderr += chunk.toString();
  });

  try {
    const target = await waitForPageTarget(port, options.timeoutMs);
    const cdp = await WebSocketCdpClient.connect(target.webSocketDebuggerUrl, options.timeoutMs);
    try {
      await cdp.send('Runtime.enable');
      const result = await cdp.send('Runtime.evaluate', {
        awaitPromise: true,
        returnByValue: true,
        expression: `
          new Promise((resolve) => {
            const deadline = Date.now() + ${Math.max(500, options.timeoutMs - 1000)};
            const read = () => {
              const raw = document.documentElement.dataset.letheBalanceQa || "";
              let qa = null;
              try { qa = raw ? JSON.parse(raw) : null; } catch {}
              if (qa && ["complete", "failed", "timeout"].includes(qa.status)) {
                resolve({ qa, raw });
                return;
              }
              if (Date.now() >= deadline) {
                resolve({ qa, raw, timedOut: true });
                return;
              }
              setTimeout(read, 100);
            };
            read();
          })
        `,
      });
      const value = result.result?.value || {};
      if (value.timedOut) {
        return { status: 'timeout', timedOut: true, raw: value.raw || '', qa: value.qa || null };
      }
      return value.qa || { status: 'failed', reason: 'Missing letheBalanceQa dataset.' };
    } finally {
      cdp.close();
    }
  } catch (error) {
    if (stderr.trim()) console.error(stderr.trim().split('\n').slice(-8).join('\n'));
    throw error;
  } finally {
    await stopChrome(chrome);
    await cleanupTempDir(userDataDir);
  }
}

function qaUrl(runNumber) {
  const file = pathToFileURL(path.resolve('index.html'));
  const params = new URLSearchParams();
  params.set('qa', 'balance');
  params.set('tester', 'BALANCE');
  params.set('session', `RUN${String(runNumber).padStart(2, '0')}`);
  params.set('balanceRunSec', String(options.runSec));
  params.set('balanceTimeoutMs', String(options.timeoutMs - 1500));
  params.set('balanceStepsPerTick', String(options.stepsPerTick));
  if (options.weapon) params.set('weapon', options.weapon);
  if (options.memory) params.set('memory', options.memory);
  file.search = `?${params.toString()}`;
  return file.href;
}

function normalizeRun(qa, runNumber) {
  const runResult = qa.runResult || (qa.finalClear ? 'clear' : qa.death ? 'death' : 'incomplete');
  const diagnostics = qa.balanceDiagnostics || {};
  const hp60At = firstHpThresholdAt(diagnostics.hpSamples, 0.6);
  const hp40At = firstHpThresholdAt(diagnostics.hpSamples, 0.4);
  const hp20At = firstHpThresholdAt(diagnostics.hpSamples, 0.2);
  return {
    runNumber,
    status: qa.status || 'unknown',
    elapsed: numberOrNull(qa.elapsed),
    runResult,
    finalClear: Boolean(qa.finalClear),
    death: Boolean(qa.death),
    deathAt: numberOrNull(qa.deathAt || qa.death?.at),
    deathPhase: diagnostics.deathPhase || qa.death?.phase || null,
    deathEnemyCount: numberOrNull(diagnostics.deathEnemyCount ?? qa.death?.enemyCount),
    maxEnemies: numberOrNull(diagnostics.maxEnemies ?? qa.danger?.maxEnemies),
    hp60At,
    hp40At,
    hp20At,
    firstBossCleared: Boolean(qa.firstBossCleared),
    firstBossTtk: numberOrNull(qa.firstBossTtk),
    firstBossFocusedDps: numberOrNull(qa.firstBossFocusedDps),
    level: Number(qa.level || 0),
    levelUpsBeforeFirstBoss: Number(qa.levelUpsBeforeFirstBoss || 0),
    slotsFilledAt: numberOrNull(qa.slotsFilledAt),
    activeMemoryCount: Number(qa.activeMemoryCount || 0),
    topDpsSource: qa.topDpsSource || null,
    topDps: Number(qa.topDps || 0),
    topDpsShare: Number(qa.topDpsShare || 0),
    dpsBySource: qa.dpsBySource || {},
    bossFights: qa.bossFights || [],
    pressureSegments: diagnostics.pressureSegments || [],
    hpSamples: diagnostics.hpSamples || [],
    lowHpSamples: diagnostics.lowHpSamples || [],
    bossPostCycleState: diagnostics.bossPostCycleState || null,
    balanceDiagnostics: diagnostics,
    retryAttempt: Number(qa.retryAttempt || 0),
    error: qa.error || null,
  };
}

function summarizeRuns(runs) {
  const gameplayRuns = runs.filter((run) => run.runResult !== 'browser_error' && run.status !== 'browser_error');
  const firstBossTtks = gameplayRuns.map((run) => run.firstBossTtk).filter(Number.isFinite);
  const slotsFilled = gameplayRuns.map((run) => run.slotsFilledAt).filter(Number.isFinite);
  const deathTimes = gameplayRuns.map((run) => run.deathAt).filter(Number.isFinite);
  const levelUps = gameplayRuns.map((run) => run.levelUpsBeforeFirstBoss).filter(Number.isFinite);
  const topShares = gameplayRuns.map((run) => run.topDpsShare).filter(Number.isFinite);
  const maxEnemies = gameplayRuns.map((run) => run.maxEnemies).filter(Number.isFinite);
  const hp60Times = gameplayRuns.map((run) => run.hp60At).filter(Number.isFinite);
  const hp40Times = gameplayRuns.map((run) => run.hp40At).filter(Number.isFinite);
  const hp20Times = gameplayRuns.map((run) => run.hp20At).filter(Number.isFinite);
  const metrics = {
    runs: runs.length,
    gameplayRuns: gameplayRuns.length,
    clearRate: rate(gameplayRuns, (run) => run.finalClear),
    deathRate: rate(gameplayRuns, (run) => run.death),
    deathAtMean: mean(deathTimes),
    deathAtMedian: median(deathTimes),
    firstBossClearRate: rate(gameplayRuns, (run) => run.firstBossCleared),
    firstBossTtkMean: mean(firstBossTtks),
    firstBossTtkMedian: median(firstBossTtks),
    levelUpsBeforeFirstBossMean: mean(levelUps),
    levelUpsBeforeFirstBossMedian: median(levelUps),
    slotsFilledAtMean: mean(slotsFilled),
    slotsFilledAtMedian: median(slotsFilled),
    topDpsShareMean: mean(topShares),
    topDpsShareMedian: median(topShares),
    maxEnemiesMean: mean(maxEnemies),
    maxEnemiesMedian: median(maxEnemies),
    hp60AtMedian: median(hp60Times),
    hp40AtMedian: median(hp40Times),
    hp20AtMedian: median(hp20Times),
    deathPhaseCounts: countBy(gameplayRuns.filter((run) => run.death), (run) => run.deathPhase || 'unknown'),
  };
  const checks = [
    check('browser run success rate', rate(runs, (run) => !run.error) >= options.browserSuccessRateMin, rate(runs, (run) => !run.error), `>= ${options.browserSuccessRateMin}`),
    check('first boss clear rate', metrics.firstBossClearRate >= options.firstBossClearRateMin, metrics.firstBossClearRate, `>= ${options.firstBossClearRateMin}`),
    check('clear rate minimum', metrics.clearRate >= options.clearRateMin, metrics.clearRate, `>= ${options.clearRateMin}`),
    check('clear rate maximum', metrics.clearRate <= options.clearRateMax, metrics.clearRate, `<= ${options.clearRateMax}`),
    check('first boss TTK lower bound', metrics.firstBossTtkMedian >= options.firstBossTtkMin, metrics.firstBossTtkMedian, `>= ${options.firstBossTtkMin}s`),
    check('first boss TTK upper bound', metrics.firstBossTtkMedian <= options.firstBossTtkMax, metrics.firstBossTtkMedian, `<= ${options.firstBossTtkMax}s`),
    check('level-ups before first boss', metrics.levelUpsBeforeFirstBossMedian >= options.levelUpsBeforeFirstBossMin, metrics.levelUpsBeforeFirstBossMedian, `>= ${options.levelUpsBeforeFirstBossMin}`),
    check('slot fill timing', metrics.slotsFilledAtMedian <= options.slotsFilledAtMax, metrics.slotsFilledAtMedian, `<= ${options.slotsFilledAtMax}s`),
    check('top DPS share', metrics.topDpsShareMedian <= options.topDpsShareMax, metrics.topDpsShareMedian, `<= ${options.topDpsShareMax}`),
  ];
  const failed = checks.filter((item) => !item.pass);
  return {
    generatedAt: new Date().toISOString(),
    version: 'v0.12-balance-loop-1',
    verdict: failed.length ? 'ITERATE_BALANCE' : 'GO_BALANCE_BASELINE',
    targets: targetSnapshot(),
    metrics,
    checks,
    failed,
    runs,
  };
}

function markdownReport(summary) {
  const lines = [
    '# LETHE v0.12 Balance QA',
    '',
    `- Generated: ${summary.generatedAt}`,
    `- Verdict: \`${summary.verdict}\``,
    `- Runs: \`${summary.metrics.runs}\``,
    `- Gameplay runs: \`${summary.metrics.gameplayRuns}\``,
    '',
    '## Metrics',
    '',
    `- Full clear rate: \`${pct(summary.metrics.clearRate)}\``,
    `- Death rate: \`${pct(summary.metrics.deathRate)}\``,
    `- Death at median: \`${fmt(summary.metrics.deathAtMedian)}s\``,
    `- First boss clear rate: \`${pct(summary.metrics.firstBossClearRate)}\``,
    `- First boss TTK median: \`${fmt(summary.metrics.firstBossTtkMedian)}s\``,
    `- Level-ups before first boss median: \`${fmt(summary.metrics.levelUpsBeforeFirstBossMedian)}\``,
    `- Slots filled at median: \`${fmt(summary.metrics.slotsFilledAtMedian)}s\``,
    `- Top DPS share median: \`${pct(summary.metrics.topDpsShareMedian)}\``,
    `- Max enemies median: \`${fmt(summary.metrics.maxEnemiesMedian)}\``,
    `- HP <= 60% median: \`${fmt(summary.metrics.hp60AtMedian)}s\``,
    `- HP <= 40% median: \`${fmt(summary.metrics.hp40AtMedian)}s\``,
    `- HP <= 20% median: \`${fmt(summary.metrics.hp20AtMedian)}s\``,
    `- Death phase counts: \`${JSON.stringify(summary.metrics.deathPhaseCounts)}\``,
    '',
    '## Checks',
    '',
    ...summary.checks.map((item) => `- ${item.pass ? '[x]' : '[ ]'} ${item.name}: \`${formatValue(item.value)}\` target \`${item.target}\``),
    '',
    '## Runs',
    '',
    '| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |',
    '| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |',
    ...summary.runs.map((run) => `| ${run.runNumber} | ${run.runResult} | ${fmt(run.deathAt)} | ${run.deathPhase || '-'} | ${fmt(run.maxEnemies)} | ${fmt(run.hp40At)} | ${run.firstBossCleared ? 'yes' : 'no'} | ${fmt(run.firstBossTtk)} | ${run.levelUpsBeforeFirstBoss} | ${fmt(run.slotsFilledAt)} | ${run.topDpsSource || '-'} | ${pct(run.topDpsShare)} |`),
    '',
  ];
  return `${lines.join('\n')}\n`;
}

function targetSnapshot() {
  return {
    firstBossClearRateMin: options.firstBossClearRateMin,
    browserSuccessRateMin: options.browserSuccessRateMin,
    clearRateMin: options.clearRateMin,
    clearRateMax: options.clearRateMax,
    firstBossTtkMin: options.firstBossTtkMin,
    firstBossTtkMax: options.firstBossTtkMax,
    levelUpsBeforeFirstBossMin: options.levelUpsBeforeFirstBossMin,
    slotsFilledAtMax: options.slotsFilledAtMax,
    topDpsShareMax: options.topDpsShareMax,
  };
}

class WebSocketCdpClient {
  constructor(socket, timeoutMs) {
    this.socket = socket;
    this.timeoutMs = timeoutMs;
    this.nextId = 1;
    this.pending = new Map();
    socket.addEventListener('message', (event) => this.onMessage(event));
    socket.addEventListener('error', () => this.rejectAll(new Error('Chrome DevTools WebSocket errored before a response was received.')));
    socket.addEventListener('close', () => this.rejectAll(new Error('Chrome DevTools WebSocket closed before a response was received.')));
  }

  static connect(url, timeoutMs) {
    return new Promise((resolve, reject) => {
      if (typeof WebSocket !== 'function') {
        reject(new Error('Node WebSocket client is not available.'));
        return;
      }
      const socket = new WebSocket(url);
      const timeout = setTimeout(() => {
        socket.close();
        reject(new Error('Timed out opening Chrome DevTools WebSocket.'));
      }, timeoutMs);
      socket.addEventListener('open', () => {
        clearTimeout(timeout);
        resolve(new WebSocketCdpClient(socket, timeoutMs));
      }, { once: true });
      socket.addEventListener('error', () => {
        clearTimeout(timeout);
        reject(new Error('Failed to open Chrome DevTools WebSocket.'));
      }, { once: true });
    });
  }

  send(method, params = {}) {
    const id = this.nextId++;
    this.socket.send(JSON.stringify({ id, method, params }));
    return new Promise((resolve, reject) => {
      const timeout = setTimeout(() => {
        this.pending.delete(id);
        reject(new Error(`Timed out waiting for CDP response to ${method}`));
      }, this.timeoutMs);
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

async function waitForPageTarget(port, timeoutMs) {
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
  throw new Error(`Timed out waiting for Chrome page target${lastError ? `: ${lastError.message}` : ''}`);
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

async function findOpenPort() {
  return new Promise((resolve, reject) => {
    const server = net.createServer();
    server.unref();
    server.on('error', reject);
    server.listen(0, '127.0.0.1', () => {
      const address = server.address();
      const port = typeof address === 'object' && address ? address.port : 0;
      server.close((error) => {
        if (error) reject(error);
        else resolve(port);
      });
    });
  });
}

async function stopChrome(chrome) {
  if (chrome.exitCode !== null || chrome.signalCode !== null) return;
  chrome.kill('SIGKILL');
  await new Promise((resolve) => {
    const timer = setTimeout(resolve, 500);
    chrome.once('exit', () => {
      clearTimeout(timer);
      resolve();
    });
  });
}

async function cleanupTempDir(dir) {
  let lastError = null;
  for (let attempt = 0; attempt < 5; attempt += 1) {
    try {
      fs.rmSync(dir, { recursive: true, force: true });
      return;
    } catch (error) {
      lastError = error;
      await sleep(100 * (attempt + 1));
    }
  }
  throw lastError;
}

function findChrome() {
  if (process.env.CHROME_PATH && fs.existsSync(process.env.CHROME_PATH)) return process.env.CHROME_PATH;
  const envCandidates = [
    process.env.PROGRAMFILES,
    process.env['PROGRAMFILES(X86)'],
    process.env.LOCALAPPDATA,
  ].filter(Boolean);
  const winCandidates = envCandidates.flatMap((root) => [
    path.join(root, 'Google', 'Chrome', 'Application', 'chrome.exe'),
    path.join(root, 'Microsoft', 'Edge', 'Application', 'msedge.exe'),
  ]);
  const candidates = [
    ...winCandidates,
    '/Applications/Google Chrome.app/Contents/MacOS/Google Chrome',
    '/Applications/Chromium.app/Contents/MacOS/Chromium',
    '/Applications/Microsoft Edge.app/Contents/MacOS/Microsoft Edge',
    'google-chrome',
    'google-chrome-stable',
    'chromium',
    'chromium-browser',
    'microsoft-edge',
  ];
  for (const candidate of candidates) {
    if (candidate.includes(path.sep) && fs.existsSync(candidate)) return candidate;
  }
  return '';
}

function parseArgs(args) {
  const date = todayString();
  const outDir = path.resolve(valueAfter(args, '--out') || path.join('alpha_test', 'outputs', 'balance'));
  return {
    dryRun: args.includes('--dry-run'),
    runs: num(valueAfter(args, '--runs'), 5),
    timeoutMs: num(valueAfter(args, '--timeout-ms'), 45000),
    retries: num(valueAfter(args, '--retries'), 2),
    runSec: num(valueAfter(args, '--run-sec'), 608),
    stepsPerTick: num(valueAfter(args, '--steps-per-tick'), 90),
    outDir,
    reportPath: path.resolve(valueAfter(args, '--report') || path.join('docs', 'balance', `${date}-v012-balance-qa.md`)),
    weapon: valueAfter(args, '--weapon') || 'twin_blades',
    memory: valueAfter(args, '--memory') || 'hungry_blades',
    firstBossClearRateMin: num(valueAfter(args, '--target-first-boss-clear-rate'), 0.7),
    browserSuccessRateMin: num(valueAfter(args, '--target-browser-success-rate'), 0.8),
    clearRateMin: num(valueAfter(args, '--target-clear-rate-min'), 0.35),
    clearRateMax: num(valueAfter(args, '--target-clear-rate-max'), 0.8),
    firstBossTtkMin: num(valueAfter(args, '--target-first-boss-ttk-min'), 15),
    firstBossTtkMax: num(valueAfter(args, '--target-first-boss-ttk-max'), 30),
    levelUpsBeforeFirstBossMin: num(valueAfter(args, '--target-level-ups-before-first-boss'), 8),
    slotsFilledAtMax: num(valueAfter(args, '--target-slots-filled-at-max'), 150),
    topDpsShareMax: num(valueAfter(args, '--target-top-dps-share-max'), 0.5),
  };
}

function valueAfter(args, name) {
  const index = args.indexOf(name);
  if (index !== -1) return args[index + 1] || '';
  const prefix = `${name}=`;
  const match = args.find((arg) => arg.startsWith(prefix));
  return match ? match.slice(prefix.length) : '';
}

function num(value, fallback) {
  if (value === '' || value === undefined || value === null) return fallback;
  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : fallback;
}

function writeJson(file, value) {
  fs.writeFileSync(file, JSON.stringify(value, null, 2), 'utf8');
}

function numberOrNull(value) {
  if (value === null || value === undefined || value === '') return null;
  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : null;
}

function check(name, pass, value, target) {
  return { name, pass: Boolean(pass), value, target };
}

function rate(items, predicate) {
  return items.length ? items.filter(predicate).length / items.length : 0;
}

function countBy(items, keyFn) {
  return items.reduce((acc, item) => {
    const key = keyFn(item);
    acc[key] = (acc[key] || 0) + 1;
    return acc;
  }, {});
}

function firstHpThresholdAt(samples, threshold) {
  const sample = (samples || []).find((item) => Number.isFinite(item.hpRate) && item.hpRate <= threshold);
  return numberOrNull(sample?.t);
}

function mean(values) {
  return values.length ? values.reduce((acc, value) => acc + value, 0) / values.length : null;
}

function median(values) {
  if (!values.length) return null;
  const sorted = [...values].sort((a, b) => a - b);
  const mid = Math.floor(sorted.length / 2);
  return sorted.length % 2 ? sorted[mid] : (sorted[mid - 1] + sorted[mid]) / 2;
}

function fmt(value) {
  return Number.isFinite(value) ? Number(value.toFixed(2)) : '-';
}

function formatValue(value) {
  if (typeof value === 'number' && Number.isFinite(value)) return String(Number(value.toFixed(4)));
  return String(value ?? '-');
}

function pct(value) {
  return Number.isFinite(value) ? `${(value * 100).toFixed(1)}%` : '-';
}

function sleep(ms) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}

function firstLine(text) {
  return String(text || '').split(/\r?\n/).find(Boolean) || '';
}

function todayString() {
  const now = new Date();
  return [
    now.getFullYear(),
    String(now.getMonth() + 1).padStart(2, '0'),
    String(now.getDate()).padStart(2, '0'),
  ].join('-');
}
