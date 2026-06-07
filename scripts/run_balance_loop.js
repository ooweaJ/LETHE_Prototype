#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');
const { spawnSync } = require('child_process');

const options = parseArgs(process.argv.slice(2));
const date = todayString();

main();

function main() {
  if (options.dryRun) {
    console.log('LETHE balance loop dry-run');
    console.log(`- node scripts/run_browser_balance_qa.js --runs ${options.runs}`);
    console.log(`- run-sec ${options.runSec}, timeout-ms ${options.timeoutMs}`);
    console.log(`- out ${options.outDir}`);
    console.log(`- report ${options.reportPath}`);
    console.log('- write docs/review_prompts/YYYY-MM-DD-balance-loop.md');
    console.log('- no emotion proxy, no Alpha Fun Score gate');
    return;
  }

  const qaArgs = [
    'scripts/run_browser_balance_qa.js',
    '--runs', String(options.runs),
    '--out', options.outDir,
    '--report', options.reportPath,
  ];
  if (options.timeoutMs) qaArgs.push('--timeout-ms', String(options.timeoutMs));
  if (options.runSec) qaArgs.push('--run-sec', String(options.runSec));

  const qa = spawnSync('node', qaArgs, {
    cwd: process.cwd(),
    encoding: 'utf8',
    maxBuffer: 1024 * 1024 * 30,
  });
  if (qa.stdout) process.stdout.write(qa.stdout);
  if (qa.stderr) process.stderr.write(qa.stderr);

  const summary = readJsonIfExists(path.join(options.outDir, 'summary.json'));
  if (!summary) {
    process.exitCode = qa.status || 1;
    return;
  }

  const promptPath = writeBalancePrompt(summary);
  console.log(`Balance loop prompt: ${promptPath}`);
  if (qa.status !== 0) process.exitCode = qa.status;
}

function writeBalancePrompt(summary) {
  fs.mkdirSync(path.join('docs', 'review_prompts'), { recursive: true });
  const file = path.join('docs', 'review_prompts', `${date}-balance-loop.md`);
  const failed = summary.failed || [];
  const lines = [
    `# LETHE v0.12 Balance Loop - ${date}`,
    '',
    '## 목적',
    '',
    '감정 proxy와 Alpha Fun Score를 쓰지 않고 v0.12 telemetry 기반으로 다음 밸런스 조정만 판단한다.',
    '',
    '## 현재 판정',
    '',
    `- Verdict: \`${summary.verdict || summary.status || 'unknown'}\``,
    `- Runs: \`${summary.metrics?.runs ?? 0}\``,
    `- First boss clear rate: \`${percent(summary.metrics?.firstBossClearRate)}\``,
    `- Full clear rate: \`${percent(summary.metrics?.clearRate)}\``,
    `- Death rate: \`${percent(summary.metrics?.deathRate)}\``,
    `- First boss TTK median: \`${format(summary.metrics?.firstBossTtkMedian)}s\``,
    `- Level-ups before first boss median: \`${format(summary.metrics?.levelUpsBeforeFirstBossMedian)}\``,
    `- Slots filled at median: \`${format(summary.metrics?.slotsFilledAtMedian)}s\``,
    `- Top DPS share median: \`${percent(summary.metrics?.topDpsShareMedian)}\``,
    '',
    '## 실패한 밸런스 체크',
    '',
    ...(failed.length
      ? failed.map((item) => `- ${item.name}: value \`${formatValue(item.value)}\`, target \`${item.target}\``)
      : ['- 없음']),
    '',
    '## Codex 다음 구현 지시',
    '',
    '- `docs/BALANCE_TABLE_v0_12.md`와 `docs/LETHE_v0.12_밸런스_개선_제안서.md`를 기준으로 가장 작은 밸런스 조정 1개만 선택한다.',
    '- 감정선, regret, irritation, Alpha Fun Score는 이번 판단에서 제외한다.',
    '- 우선순위는 첫 보스 TTK, 첫 180초 레벨업 수, 3슬롯 완성 시각, top DPS share, clear/death rate 순서다.',
    '- 새 기억, 새 무기, 상점, 메타 진행, 새 지역, 최종 보스 확장은 금지한다.',
    '- 변경 후 `npm run qa:balance` 또는 환경 blocker 기록을 남긴다.',
    '',
    '## 원본 summary',
    '',
    '```json',
    JSON.stringify(summary, null, 2).slice(0, 12000),
    '```',
    '',
  ];
  fs.writeFileSync(file, lines.join('\n'), 'utf8');
  return file;
}

function readJsonIfExists(file) {
  try {
    return JSON.parse(fs.readFileSync(file, 'utf8'));
  } catch {
    return null;
  }
}

function parseArgs(args) {
  const positional = args.filter((arg) => !String(arg).startsWith('--'));
  return {
    dryRun: args.includes('--dry-run'),
    runs: numberArg(args, '--runs', numberAt(positional, 0, 5)),
    runSec: numberArg(args, '--run-sec', numberAt(positional, 1, 690)),
    timeoutMs: numberArg(args, '--timeout-ms', numberAt(positional, 2, 60000)),
    outDir: valueAfter(args, '--out') || positional[3] || path.join('alpha_test', 'outputs', 'balance'),
    reportPath: valueAfter(args, '--report') || positional[4] || path.join('docs', 'balance', `${todayString()}-v012-balance-qa.md`),
  };
}

function numberAt(values, index, fallback) {
  const parsed = Number(values[index]);
  return Number.isFinite(parsed) ? parsed : fallback;
}

function valueAfter(args, name) {
  const index = args.indexOf(name);
  if (index !== -1) return args[index + 1] || '';
  const prefix = `${name}=`;
  const match = args.find((arg) => arg.startsWith(prefix));
  return match ? match.slice(prefix.length) : '';
}

function numberArg(args, name, fallback) {
  const raw = valueAfter(args, name);
  if (raw === '') return fallback;
  const parsed = Number(raw);
  return Number.isFinite(parsed) ? parsed : fallback;
}

function todayString() {
  const now = new Date();
  return [
    now.getFullYear(),
    String(now.getMonth() + 1).padStart(2, '0'),
    String(now.getDate()).padStart(2, '0'),
  ].join('-');
}

function format(value) {
  return Number.isFinite(value) ? Number(value.toFixed(2)) : '-';
}

function formatValue(value) {
  if (typeof value === 'number' && Number.isFinite(value)) return String(Number(value.toFixed(4)));
  return String(value ?? '-');
}

function percent(value) {
  return Number.isFinite(value) ? `${(value * 100).toFixed(1)}%` : '-';
}
