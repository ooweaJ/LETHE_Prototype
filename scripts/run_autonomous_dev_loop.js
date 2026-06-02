#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');
const { spawnSync } = require('child_process');

const DEFAULT_PREFLIGHT = 'npm run autopilot:preflight:local';
const DIRTY_PREFLIGHT = 'node scripts/autopilot_preflight.js --allow-dirty';

const options = parseArgs(process.argv.slice(2));
const startedAt = new Date();
const date = options.date || todayString();
const runId = `${date}-devloop-${compactTime(startedAt)}`;
const logDir = path.resolve(options.logDir || path.join('docs', 'loop_runs'));
const runLogPath = path.join(logDir, `${runId}.md`);
const deadline = Date.now() + options.durationMinutes * 60 * 1000;

main();

function main() {
  if (options.dryRun) {
    printDryRun();
    return;
  }

  checkInitialGitState();
  const preflight = runPreflightBeforeLog();
  fs.mkdirSync(logDir, { recursive: true });
  writeLogHeader(preflight);
  sendNotice('start', '자동 개발 루프 시작', `iterations=${options.iterations}, duration=${options.durationMinutes}m`);

  for (let iteration = 1; iteration <= options.iterations; iteration += 1) {
    if (Date.now() >= deadline) {
      log(`\nTime budget reached before iteration ${iteration}.\n`);
      break;
    }

    log(`\n## Iteration ${iteration}\n`);
    sendNotice('status', `자동 개발 루프 ${iteration}/${options.iterations}`, '구현 사이클을 시작합니다.');

    const context = readLoopContext(iteration);
    const implementation = runCodexImplementation(iteration, context);
    const verified = runVerification(iteration);

    if (!verified) {
      writeBlockerPrompt(iteration, 'verification failed', 'See loop log for failed verification step.');
      sendNotice('blocked', `자동 개발 루프 중단 ${iteration}`, '검증 실패. blocker prompt를 남겼습니다.');
      process.exitCode = 1;
      break;
    }

    runRequired('report build', 'npm run report');
    sendWorkUnitReport();

    const feedbackPrompt = writeFeedbackPrompt(iteration, implementation);
    runRequired(
      'Claude/Codex feedback',
      `node scripts/run_planning_pipeline.js --prompt ${quote(feedbackPrompt)} --provider ${options.provider} --test none`
    );

    runCodexTaskUpdate(iteration, feedbackPrompt);
    runRequired('report build after feedback', 'npm run report');
    sendWorkUnitReport();

    sendNotice('checkpoint', `자동 개발 루프 ${iteration}/${options.iterations} 완료`, '구현-검증-보고-피드백-태스크 갱신까지 완료했습니다.');

    if (options.commit) {
      commitAndMaybePush(iteration);
    }

    if (iteration < options.iterations && options.sleepMinutes > 0) {
      log(`\nSleeping ${options.sleepMinutes} minute(s).\n`);
      sleep(options.sleepMinutes * 60 * 1000);
    }
  }

  sendNotice('done', '자동 개발 루프 완료', '루프가 종료되었습니다.', path.relative(process.cwd(), runLogPath), { record: false });
  console.log(`Autonomous dev loop log: ${path.relative(process.cwd(), runLogPath)}`);
}

function writeLogHeader(preflight) {
  fs.writeFileSync(runLogPath, [
    `# Autonomous Dev Loop - ${runId}`,
    '',
    `- Started: ${startedAt.toISOString()}`,
    `- Iterations: ${options.iterations}`,
    `- Duration minutes: ${options.durationMinutes}`,
    `- Codex timeout minutes: ${options.codexTimeoutMinutes}`,
    `- Provider: ${options.provider}`,
    `- Codex sandbox: ${options.codexSandbox}`,
    `- Commit: ${options.commit}`,
    `- Push: ${options.push}`,
    '',
    '### preflight',
    '',
    `\`${options.preflight}\``,
    '',
    fence('stdout', trimOutput(preflight.stdout || '')),
    preflight.stderr ? fence('stderr', trimOutput(preflight.stderr)) : '',
  ].join('\n'), 'utf8');
}

function printDryRun() {
  const commands = [
    options.allowDirty ? 'git status --short (dirty tree allowed)' : 'git status --short (dirty tree blocks loop)',
    options.preflight,
    'codex exec --sandbox workspace-write < implementation prompt>',
    verificationCommands().join(' && '),
    'npm run report',
    options.noDiscord ? 'skip Discord work-unit report' : 'node scripts/send_discord_report.js --latest-section',
    `node scripts/run_planning_pipeline.js --provider ${options.provider} --test none --prompt <feedback prompt>`,
    'codex exec --sandbox workspace-write < task-update prompt>',
    options.commit ? 'git add + git commit' : 'skip commit',
    options.push ? 'git push' : 'skip push',
  ];

  console.log('LETHE autonomous dev loop dry-run');
  console.log('');
  commands.forEach((command) => console.log(`- ${command}`));
  console.log('');
  console.log(`Log: ${path.relative(process.cwd(), runLogPath)}`);
}

function checkInitialGitState() {
  const status = runCapture('git status --short');
  if (!status.trim()) return;
  if (options.allowDirty) return;

  console.error('Autonomous dev loop requires a clean working tree before it creates loop logs.');
  console.error(status);
  process.exit(1);
}

function runPreflightBeforeLog() {
  const result = spawnSync(options.preflight, {
    cwd: process.cwd(),
    encoding: 'utf8',
    shell: true,
    maxBuffer: 1024 * 1024 * 30,
  });
  if (result.status !== 0 || result.error) {
    const detail = result.error ? result.error.message : firstLine(result.stderr || result.stdout);
    console.error(`Autonomous dev loop preflight failed before log creation: ${detail}`);
    if (result.stdout) console.error(result.stdout.trim());
    if (result.stderr) console.error(result.stderr.trim());
    process.exit(1);
  }
  return result;
}

function readLoopContext(iteration) {
  return {
    iteration,
    status: readText('docs/CODEX_STATUS.md'),
    nextTasks: readText('docs/NEXT_TASKS.md'),
    doubleCheck: readText('docs/review_responses/2026-06-02-v09-release-feel-loop-double-check.md'),
    latestSummary: readJsonIfExists('alpha_test/outputs/quick/summary.json'),
  };
}

function runCodexImplementation(iteration, context) {
  const promptPath = path.join(logDir, `${runId}-iteration-${iteration}-implement-prompt.md`);
  const outputPath = path.join(logDir, `${runId}-iteration-${iteration}-implement-result.md`);
  const prompt = buildImplementationPrompt(context);
  fs.writeFileSync(promptPath, prompt, 'utf8');

  const command = [
    'codex exec',
    '--cd', quote(process.cwd()),
    '--sandbox', quote(options.codexSandbox),
    '--output-last-message', quote(outputPath),
    '-',
  ].join(' ');

  log(`\n### Codex implementation\n\nPrompt: ${path.relative(process.cwd(), promptPath)}\nOutput: ${path.relative(process.cwd(), outputPath)}\n`);
  const result = spawnSync(command, {
    cwd: process.cwd(),
    encoding: 'utf8',
    input: prompt,
    shell: true,
    maxBuffer: 1024 * 1024 * 30,
    timeout: options.codexTimeoutMinutes * 60 * 1000,
  });
  recordResult('codex implementation', result);
  if (result.status !== 0) failLoop('codex implementation', firstLine(result.stderr || result.stdout));
  return {
    promptPath,
    outputPath,
    output: readText(outputPath),
  };
}

function buildImplementationPrompt(context) {
  return [
    '# LETHE Autonomous Implementation Cycle',
    '',
    '너는 LETHE 프로젝트의 구현 담당 Codex다. 반드시 한국어로 요약한다.',
    '',
    '## 이번 루프 목표',
    '',
    '- `docs/NEXT_TASKS.md`의 다음 미완료 v0.9 작업 중 가장 앞선 하나만 구현한다.',
    '- 현재 선택 범위는 아래 `NEXT_TASKS` 발췌에서 가장 앞선 미완료 v0.9 항목이다.',
    '- WP1이 완료된 상태라면 새 기능으로 건너뛰기 전에 preflight cleanup과 trusted-local identity QA 재확인을 먼저 처리한다.',
    '- 새 기억, 새 슬롯, 상점, 메타 진행, 새 지역, 대형 무기 확장은 금지한다.',
    '',
    '## 반드시 할 일',
    '',
    '1. 현재 코드와 문서를 읽는다.',
    '2. 가장 작은 구현 단위를 선택해 코드 변경을 한다.',
    '3. 가능한 검증을 실행한다.',
    '4. `docs/CODEX_STATUS.md`, `docs/NEXT_TASKS.md`, `docs/devlog/2026-06-02.md`, `docs/reports/2026-06-02.md`를 갱신한다.',
    '5. 보고서의 새 top-level heading은 반드시 `# 2026-06-02-NN - 작업 제목` 형식으로 추가한다. 하루 전체 요약 제목을 새로 만들지 않는다.',
    '6. 보고서 HTML은 wrapper가 생성하므로 직접 커밋/푸시는 하지 않는다.',
    '',
    '## 현재 상태 발췌',
    '',
    section('CODEX_STATUS', context.status, 3500),
    '',
    '## NEXT_TASKS 발췌',
    '',
    section('NEXT_TASKS', context.nextTasks, 4000),
    '',
    '## v0.9 더블 체크 요약',
    '',
    section('DOUBLE_CHECK', context.doubleCheck, 2500),
    '',
    '## 출력 형식',
    '',
    '- 구현한 것',
    '- 검증한 것',
    '- 남은 위험',
    '- 다음 루프 추천 작업',
    '',
  ].join('\n');
}

function runVerification(iteration) {
  for (const command of verificationCommands()) {
    const result = runStep(`verify ${command}`, command);
    if (result.status !== 0) {
      log(`- FAIL verification ${iteration}: ${command}\n`);
      return false;
    }
    log(`- PASS verification: ${command}\n`);
  }
  return true;
}

function verificationCommands() {
  return [
    'npm run doctor',
    'npm run ai:test:quick',
  ];
}

function writeFeedbackPrompt(iteration, implementation) {
  const promptPath = path.join('docs', 'review_prompts', `${runId}-feedback-${iteration}.md`);
  const summary = readText('alpha_test/outputs/quick/summary.json');
  const diffStat = runCapture('git diff --stat');
  const prompt = [
    `# LETHE 자동 개발 루프 피드백 - ${date} iteration ${iteration}`,
    '',
    '## 목적',
    '',
    'Codex가 한 구현 결과를 보고 다음 루프에서 무엇을 고쳐야 할지 판단한다.',
    '',
    '## 구현 결과',
    '',
    section('IMPLEMENTATION_RESULT', implementation.output, 4000),
    '',
    '## quick AI test summary',
    '',
    '```json',
    summary.slice(0, 5000),
    '```',
    '',
    '## diff stat',
    '',
    '```text',
    diffStat,
    '```',
    '',
    '## 답변 요청',
    '',
    '- 이번 구현이 `docs/NEXT_TASKS.md`의 현재 v0.9 최우선 미완료 항목과 범위 제한에 맞는지 판단한다.',
    '- 다음 루프에서 구현할 가장 작은 작업 1개를 제안한다.',
    '- 실패/리스크가 있으면 명확히 적는다.',
    '- 범위 확장은 금지한다.',
    '',
  ].join('\n');
  fs.writeFileSync(promptPath, prompt, 'utf8');
  return promptPath;
}

function runCodexTaskUpdate(iteration, feedbackPrompt) {
  const promptPath = path.join(logDir, `${runId}-iteration-${iteration}-task-update-prompt.md`);
  const outputPath = path.join(logDir, `${runId}-iteration-${iteration}-task-update-result.md`);
  const stem = path.basename(feedbackPrompt, '.md');
  const prompt = [
    '# LETHE Task Update Cycle',
    '',
    '너는 LETHE 프로젝트의 기록/태스크 갱신 담당 Codex다.',
    '코드 기능 변경은 하지 말고 문서만 갱신한다.',
    '',
    '## 읽을 파일',
    '',
    `- ${feedbackPrompt}`,
    `- docs/review_responses/${stem}-claude.md`,
    `- docs/review_responses/${stem}-codex.md`,
    `- docs/review_responses/${stem}-double-check.md`,
    '- docs/NEXT_TASKS.md',
    '- docs/CODEX_STATUS.md',
    '',
    '## 할 일',
    '',
    '- Claude/Codex 피드백의 공통점과 충돌을 요약한다.',
    '- `docs/NEXT_TASKS.md`의 완료/다음 작업을 갱신한다.',
    '- `docs/CODEX_STATUS.md`, `docs/devlog/2026-06-02.md`, `docs/reports/2026-06-02.md`에 루프 결과를 기록한다.',
    '- 보고서의 새 top-level heading은 반드시 `# 2026-06-02-NN - 작업 제목` 형식으로 추가한다.',
    '- 새 구현 범위를 늘리지 않는다.',
    '',
  ].join('\n');
  fs.writeFileSync(promptPath, prompt, 'utf8');

  const command = [
    'codex exec',
    '--cd', quote(process.cwd()),
    '--sandbox', quote(options.codexSandbox),
    '--output-last-message', quote(outputPath),
    '-',
  ].join(' ');
  const result = spawnSync(command, {
    cwd: process.cwd(),
    encoding: 'utf8',
    input: prompt,
    shell: true,
    maxBuffer: 1024 * 1024 * 30,
    timeout: options.codexTimeoutMinutes * 60 * 1000,
  });
  recordResult('codex task update', result);
  if (result.status !== 0) failLoop('codex task update', firstLine(result.stderr || result.stdout));
}

function commitAndMaybePush(iteration) {
  const status = runCapture('git status --short');
  if (!status.trim()) {
    log('\nNo changes to commit.\n');
    return;
  }

  log('\n### git checkpoint\n\nGit add/commit/push is executed after loop logging so the next automation starts clean.\n');

  runSilentRequired('git add', 'git add -A');
  runSilentRequired('git commit', `git commit -m ${quote(`feat: 자동 개발 루프 ${iteration}차 반영`)}`);

  if (options.push) {
    runSilentRequired('git push', 'git push');
  }
}

function sendWorkUnitReport() {
  if (options.noDiscord) return;
  const command = options.discordDryRun
    ? 'node scripts/send_discord_report.js --dry-run --latest-section'
    : 'node scripts/send_discord_report.js --latest-section';
  const result = runStep(options.discordDryRun ? 'discord work-unit dry-run' : 'discord work-unit report', command);
  if (result.status !== 0) {
    log(`- WARN discord work-unit report: ${firstLine(result.stderr || result.stdout)}\n`);
  }
}

function sendNotice(type, title, summary, file = '', opts = {}) {
  if (options.noDiscord) return;
  const args = [
    'node scripts/send_codex_notice.js',
    `--type=${quoteArg(type)}`,
    `--title=${quoteArg(title)}`,
    `--summary=${quoteArg(summary)}`,
  ];
  if (file) args.push(`--file=${quoteArg(file)}`);
  if (options.discordDryRun) args.push('--dry-run');
  const result = opts.record === false
    ? runSilentStep(`discord notice ${type}`, args.join(' '))
    : runStep(`discord notice ${type}`, args.join(' '));
  if (result.status !== 0) {
    log(`- WARN discord notice ${type}: ${firstLine(result.stderr || result.stdout)}\n`);
  }
}

function runRequired(name, command, opts = {}) {
  const result = runStep(name, command);
  const failedByStdout = opts.failOnStdout && result.stdout.trim();
  if (result.status !== 0 || failedByStdout) {
    const detail = failedByStdout ? result.stdout.trim() : firstLine(result.stderr || result.stdout);
    writeBlockerPrompt(0, name, detail);
    sendNotice('blocked', `자동 개발 루프 중단: ${name}`, detail);
    failLoop(name, detail);
  }
  return result;
}

function runSilentRequired(name, command) {
  const result = runSilentStep(name, command);
  if (result.status !== 0) {
    const detail = firstLine(result.stderr || result.stdout);
    writeBlockerPrompt(0, name, detail);
    sendNotice('blocked', `자동 개발 루프 중단: ${name}`, detail);
    failLoop(name, detail);
  }
  return result;
}

function runStep(name, command) {
  log(`\n### ${name}\n\n\`${command}\`\n`);
  const result = spawnSync(command, {
    cwd: process.cwd(),
    encoding: 'utf8',
    shell: true,
    maxBuffer: 1024 * 1024 * 30,
  });
  recordResult(name, result);
  return result;
}

function runSilentStep(name, command) {
  const result = spawnSync(command, {
    cwd: process.cwd(),
    encoding: 'utf8',
    shell: true,
    maxBuffer: 1024 * 1024 * 30,
  });
  if (result.status !== 0 || result.error) {
    recordResult(name, result);
  }
  return result;
}

function recordResult(name, result) {
  if (result.stdout) log(fence('stdout', trimOutput(result.stdout)));
  if (result.stderr) log(fence('stderr', trimOutput(result.stderr)));
  if (result.error) log(`\nError in ${name}: ${result.error.message}\n`);
}

function writeBlockerPrompt(iteration, stepName, detail) {
  const blockerPath = path.join('docs', 'review_prompts', `${date}-autodev-blocker-${iteration || 'preflight'}.md`);
  fs.writeFileSync(blockerPath, [
    `# LETHE 자동 개발 루프 Blocker - ${date}`,
    '',
    `- Step: ${stepName}`,
    `- Loop log: ${path.relative(process.cwd(), runLogPath)}`,
    '',
    '## Detail',
    '',
    '```text',
    detail || 'no detail',
    '```',
    '',
    '## Ask',
    '',
    '- 막힌 이유를 분석한다.',
    '- 다음 루프에서 재개하기 위한 가장 작은 수정만 제안한다.',
    '',
  ].join('\n'), 'utf8');
  log(`\nBlocker prompt written: ${blockerPath}\n`);
}

function parseArgs(args) {
  const parsed = {
    allowDirty: false,
    codexTimeoutMinutes: 20,
    codexSandbox: 'workspace-write',
    commit: true,
    date: '',
    discordDryRun: false,
    dryRun: false,
    durationMinutes: 360,
    iterations: 6,
    logDir: '',
    noDiscord: false,
    preflight: DEFAULT_PREFLIGHT,
    provider: 'double',
    push: true,
    sleepMinutes: 0,
  };

  for (let index = 0; index < args.length; index += 1) {
    const arg = args[index];
    if (arg === '--allow-dirty') parsed.allowDirty = true;
    else if (arg === '--codex-timeout-minutes') parsed.codexTimeoutMinutes = positiveInteger(args[++index], 'codex-timeout-minutes');
    else if (arg.startsWith('--codex-timeout-minutes=')) parsed.codexTimeoutMinutes = positiveInteger(arg.slice('--codex-timeout-minutes='.length), 'codex-timeout-minutes');
    else if (arg === '--discord-dry-run') parsed.discordDryRun = true;
    else if (arg === '--dry-run') parsed.dryRun = true;
    else if (arg === '--no-commit') parsed.commit = false;
    else if (arg === '--no-discord') parsed.noDiscord = true;
    else if (arg === '--no-push') parsed.push = false;
    else if (arg === '--date') parsed.date = args[++index] || '';
    else if (arg.startsWith('--date=')) parsed.date = arg.slice('--date='.length);
    else if (arg === '--duration-minutes') parsed.durationMinutes = positiveInteger(args[++index], 'duration-minutes');
    else if (arg.startsWith('--duration-minutes=')) parsed.durationMinutes = positiveInteger(arg.slice('--duration-minutes='.length), 'duration-minutes');
    else if (arg === '--iterations') parsed.iterations = positiveInteger(args[++index], 'iterations');
    else if (arg.startsWith('--iterations=')) parsed.iterations = positiveInteger(arg.slice('--iterations='.length), 'iterations');
    else if (arg === '--log-dir') parsed.logDir = args[++index] || '';
    else if (arg.startsWith('--log-dir=')) parsed.logDir = arg.slice('--log-dir='.length);
    else if (arg === '--preflight') parsed.preflight = args[++index] || '';
    else if (arg.startsWith('--preflight=')) parsed.preflight = arg.slice('--preflight='.length);
    else if (arg === '--provider') parsed.provider = normalizeProvider(args[++index] || '');
    else if (arg.startsWith('--provider=')) parsed.provider = normalizeProvider(arg.slice('--provider='.length));
    else if (arg === '--sandbox') parsed.codexSandbox = normalizeSandbox(args[++index] || '');
    else if (arg.startsWith('--sandbox=')) parsed.codexSandbox = normalizeSandbox(arg.slice('--sandbox='.length));
    else if (arg === '--sleep-minutes') parsed.sleepMinutes = nonNegativeNumber(args[++index], 'sleep-minutes');
    else if (arg.startsWith('--sleep-minutes=')) parsed.sleepMinutes = nonNegativeNumber(arg.slice('--sleep-minutes='.length), 'sleep-minutes');
    else fail(`Unknown option: ${arg}`);
  }

  if (parsed.allowDirty && parsed.preflight === DEFAULT_PREFLIGHT) {
    parsed.preflight = DIRTY_PREFLIGHT;
  }

  return parsed;
}

function normalizeProvider(value) {
  if (['auto', 'double', 'claude', 'codex', 'none'].includes(value)) return value;
  fail(`Unknown provider: ${value}. Use auto, double, claude, codex, or none.`);
}

function normalizeSandbox(value) {
  if (['workspace-write', 'danger-full-access'].includes(value)) return value;
  fail(`Unknown sandbox: ${value}. Use workspace-write or danger-full-access.`);
}

function positiveInteger(value, name) {
  const number = Number(value);
  if (Number.isInteger(number) && number > 0) return number;
  fail(`--${name} must be a positive integer.`);
}

function nonNegativeNumber(value, name) {
  const number = Number(value);
  if (Number.isFinite(number) && number >= 0) return number;
  fail(`--${name} must be a non-negative number.`);
}

function runCapture(command) {
  const result = spawnSync(command, {
    cwd: process.cwd(),
    encoding: 'utf8',
    shell: true,
    maxBuffer: 1024 * 1024 * 10,
  });
  return [result.stdout, result.stderr].filter(Boolean).join('\n').trim();
}

function readText(filePath) {
  const full = path.resolve(filePath);
  return fs.existsSync(full) ? fs.readFileSync(full, 'utf8') : '';
}

function readJsonIfExists(filePath) {
  const text = readText(filePath);
  if (!text) return null;
  try {
    return JSON.parse(text);
  } catch {
    return null;
  }
}

function section(title, text, max) {
  const value = typeof text === 'string' ? text : JSON.stringify(text, null, 2);
  return [`### ${title}`, '', '```text', trimTo(value || '없음', max), '```'].join('\n');
}

function trimTo(text, max) {
  const value = String(text || '').trim();
  return value.length > max ? `${value.slice(0, max)}\n... [trimmed]` : value;
}

function trimOutput(text) {
  return trimTo(text, 9000);
}

function fence(label, text) {
  return `\n\`\`\`${label}\n${text}\n\`\`\`\n`;
}

function quote(value) {
  const stringValue = String(value);
  if (process.platform === 'win32') return `"${stringValue.replace(/"/g, '\\"')}"`;
  return `'${stringValue.replace(/'/g, "'\\''")}'`;
}

function quoteArg(value) {
  return quote(String(value).replace(/\r?\n/g, ' '));
}

function firstLine(text) {
  return String(text || '').trim().split(/\r?\n/)[0] || 'no output';
}

function sleep(ms) {
  if (ms <= 0) return;
  Atomics.wait(new Int32Array(new SharedArrayBuffer(4)), 0, 0, ms);
}

function log(text) {
  fs.appendFileSync(runLogPath, text, 'utf8');
}

function todayString() {
  const now = new Date();
  return `${now.getFullYear()}-${String(now.getMonth() + 1).padStart(2, '0')}-${String(now.getDate()).padStart(2, '0')}`;
}

function compactTime(dateValue) {
  return [
    String(dateValue.getHours()).padStart(2, '0'),
    String(dateValue.getMinutes()).padStart(2, '0'),
    String(dateValue.getSeconds()).padStart(2, '0'),
  ].join('');
}

function failLoop(step, detail) {
  console.error(`Autonomous dev loop failed at ${step}: ${detail}`);
  process.exit(1);
}

function fail(message) {
  console.error(message);
  process.exit(1);
}
