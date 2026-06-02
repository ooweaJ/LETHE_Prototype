#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');
const { spawnSync } = require('child_process');

const options = parseArgs(process.argv.slice(2));
const startedAt = new Date();
const date = options.date || todayString();
const logDir = path.resolve(options.logDir || path.join('docs', 'loop_runs'));
const promptPath = path.resolve(options.prompt || path.join('docs', 'review_prompts', '2026-06-02-v09-release-feel-loop.md'));
const runId = `${date}-overnight-${compactTime(startedAt)}`;
const runLogPath = path.join(logDir, `${runId}.md`);

main();

function main() {
  if (options.dryRun) {
    printDryRun();
    return;
  }

  fs.mkdirSync(logDir, { recursive: true });
  writeRunHeader();

  for (let iteration = 1; iteration <= options.iterations; iteration += 1) {
    log(`\n## Iteration ${iteration}\n`);

    const steps = [
      {
        name: 'git status',
        command: 'git status --short',
        required: !options.allowDirty,
      },
      {
        name: 'autopilot preflight',
        command: options.preflight,
        required: true,
        skip: options.skipPreflight,
      },
      {
        name: 'planning pipeline',
        command: planningCommand(iteration),
        required: true,
      },
      {
        name: 'implementation command',
        command: options.implementCmd,
        required: true,
        skip: !options.implementCmd,
      },
      {
        name: 'post-loop doctor',
        command: 'npm run doctor',
        required: true,
      },
      {
        name: 'post-loop ai quick',
        command: 'npm run ai:test:quick',
        required: true,
        skip: options.skipPostAi,
      },
      {
        name: 'report build',
        command: 'npm run report',
        required: true,
        skip: options.skipReport,
      },
      {
        name: 'discord unit dry-run',
        command: 'npm run report:discord:unit:dry',
        required: false,
        skip: options.skipDiscordDry,
      },
    ];

    let failed = false;
    for (const step of steps) {
      if (step.skip) {
        log(`- SKIP ${step.name}\n`);
        continue;
      }

      const result = runStep(step.name, step.command);
      const blockedByDirtyTree = step.name === 'git status' && result.stdout.trim() && !options.allowDirty;
      const stepFailed = result.status !== 0 || blockedByDirtyTree;
      if (stepFailed) {
        failed = true;
        const detail = blockedByDirtyTree ? result.stdout.trim() : firstLine(result.stderr || result.stdout);
        log(`- FAIL ${step.name}: ${detail || 'no output'}\n`);
        writeBlockerPrompt(iteration, step.name, detail || `exit ${result.status}`);
        if (step.required) break;
      } else {
        log(`- PASS ${step.name}\n`);
      }
    }

    if (failed) {
      log('\nLoop stopped because a required step failed. A blocker prompt was written for the next planning pass.\n');
      process.exitCode = 1;
      break;
    }

    if (iteration < options.iterations) {
      log(`\nSleeping ${options.sleepMinutes} minute(s) before next iteration.\n`);
      sleep(options.sleepMinutes * 60 * 1000);
    }
  }

  log('\n## Result\n\nLoop finished. Review this log, the latest planning response, and docs/CODEX_STATUS.md before editing scope.\n');
  console.log(`Overnight loop log: ${path.relative(process.cwd(), runLogPath)}`);
}

function writeRunHeader() {
  const lines = [
    `# Overnight Loop Run - ${runId}`,
    '',
    `- Started: ${startedAt.toISOString()}`,
    `- Prompt: ${path.relative(process.cwd(), promptPath)}`,
    `- Provider: ${options.provider}`,
    `- Test: ${options.test}`,
    `- Iterations: ${options.iterations}`,
    `- Implement command: ${options.implementCmd ? options.implementCmd : 'none'}`,
    '',
  ];
  fs.writeFileSync(runLogPath, lines.join('\n'), 'utf8');
}

function printDryRun() {
  const commands = [
    options.allowDirty ? 'git status --short (dirty tree allowed)' : 'git status --short (dirty tree blocks loop)',
    options.skipPreflight ? 'skip preflight' : options.preflight,
    planningCommand(1),
    options.implementCmd ? options.implementCmd : 'skip implementation command',
    'npm run doctor',
    options.skipPostAi ? 'skip post-loop ai quick' : 'npm run ai:test:quick',
    options.skipReport ? 'skip report build' : 'npm run report',
    options.skipDiscordDry ? 'skip Discord dry-run' : 'npm run report:discord:unit:dry',
  ];

  console.log('LETHE overnight loop dry-run');
  console.log('');
  commands.forEach((command) => console.log(`- ${command}`));
  console.log('');
  console.log(`Log: ${path.relative(process.cwd(), runLogPath)}`);
}

function planningCommand(iteration) {
  const args = [
    'node scripts/run_planning_pipeline.js',
    `--prompt ${quote(promptPath)}`,
    `--provider ${options.provider}`,
    `--test ${options.test}`,
    `--date ${date}`,
  ];
  if (iteration > 1) args.push('--skip-tests');
  return args.join(' ');
}

function runStep(name, command) {
  log(`\n### ${name}\n\n\`${command}\`\n`);
  const result = spawnSync(command, {
    cwd: process.cwd(),
    encoding: 'utf8',
    shell: true,
    maxBuffer: 1024 * 1024 * 20,
  });

  if (result.stdout) log(fence('stdout', trimOutput(result.stdout)));
  if (result.stderr) log(fence('stderr', trimOutput(result.stderr)));
  return result;
}

function writeBlockerPrompt(iteration, stepName, detail) {
  const blockerPath = path.join('docs', 'review_prompts', `${date}-overnight-loop-blocker-${iteration}.md`);
  const content = [
    `# LETHE Overnight Loop Blocker - ${date} Iteration ${iteration}`,
    '',
    '## Blocked Step',
    '',
    `- ${stepName}`,
    '',
    '## Failure Detail',
    '',
    '```text',
    detail,
    '```',
    '',
    '## Context',
    '',
    `- Loop log: ${path.relative(process.cwd(), runLogPath)}`,
    `- Original prompt: ${path.relative(process.cwd(), promptPath)}`,
    '- User priority: make the development cycle continue during sleep, but keep Markdown files as source of truth.',
    '',
    '## Ask',
    '',
    '- Explain why the loop stopped.',
    '- Propose the smallest fix that lets the next loop continue.',
    '- Do not propose scope expansion beyond the current 6 memories, 3 active memory slots, and HTML prototype validation.',
    '',
  ].join('\n');
  fs.writeFileSync(path.resolve(blockerPath), content, 'utf8');
  log(`\nBlocker prompt written: ${blockerPath}\n`);
}

function parseArgs(args) {
  const parsed = {
    allowDirty: false,
    date: '',
    dryRun: false,
    implementCmd: '',
    iterations: 1,
    logDir: '',
    preflight: 'npm run autopilot:preflight:local',
    prompt: '',
    provider: 'double',
    skipDiscordDry: false,
    skipPostAi: false,
    skipPreflight: false,
    skipReport: false,
    sleepMinutes: 0,
    test: 'quick',
  };

  for (let index = 0; index < args.length; index += 1) {
    const arg = args[index];
    if (arg === '--allow-dirty') parsed.allowDirty = true;
    else if (arg === '--dry-run') parsed.dryRun = true;
    else if (arg === '--skip-discord-dry') parsed.skipDiscordDry = true;
    else if (arg === '--skip-post-ai') parsed.skipPostAi = true;
    else if (arg === '--skip-preflight') parsed.skipPreflight = true;
    else if (arg === '--skip-report') parsed.skipReport = true;
    else if (arg === '--date') parsed.date = args[++index] || '';
    else if (arg.startsWith('--date=')) parsed.date = arg.slice('--date='.length);
    else if (arg === '--implement-cmd') parsed.implementCmd = args[++index] || '';
    else if (arg.startsWith('--implement-cmd=')) parsed.implementCmd = arg.slice('--implement-cmd='.length);
    else if (arg === '--iterations') parsed.iterations = positiveInteger(args[++index], 'iterations');
    else if (arg.startsWith('--iterations=')) parsed.iterations = positiveInteger(arg.slice('--iterations='.length), 'iterations');
    else if (arg === '--log-dir') parsed.logDir = args[++index] || '';
    else if (arg.startsWith('--log-dir=')) parsed.logDir = arg.slice('--log-dir='.length);
    else if (arg === '--preflight') parsed.preflight = args[++index] || '';
    else if (arg.startsWith('--preflight=')) parsed.preflight = arg.slice('--preflight='.length);
    else if (arg === '--prompt') parsed.prompt = args[++index] || '';
    else if (arg.startsWith('--prompt=')) parsed.prompt = arg.slice('--prompt='.length);
    else if (arg === '--provider') parsed.provider = normalizeProvider(args[++index] || '');
    else if (arg.startsWith('--provider=')) parsed.provider = normalizeProvider(arg.slice('--provider='.length));
    else if (arg === '--sleep-minutes') parsed.sleepMinutes = nonNegativeNumber(args[++index], 'sleep-minutes');
    else if (arg.startsWith('--sleep-minutes=')) parsed.sleepMinutes = nonNegativeNumber(arg.slice('--sleep-minutes='.length), 'sleep-minutes');
    else if (arg === '--test') parsed.test = normalizeTest(args[++index] || '');
    else if (arg.startsWith('--test=')) parsed.test = normalizeTest(arg.slice('--test='.length));
    else fail(`Unknown option: ${arg}`);
  }

  return parsed;
}

function normalizeProvider(value) {
  if (['auto', 'double', 'claude', 'codex', 'none'].includes(value)) return value;
  fail(`Unknown provider: ${value}. Use auto, double, claude, codex, or none.`);
}

function normalizeTest(value) {
  if (['quick', 'default', 'heavy', 'none'].includes(value)) return value;
  fail(`Unknown test: ${value}. Use quick, default, heavy, or none.`);
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

function sleep(ms) {
  if (ms <= 0) return;
  Atomics.wait(new Int32Array(new SharedArrayBuffer(4)), 0, 0, ms);
}

function log(text) {
  fs.appendFileSync(runLogPath, text, 'utf8');
}

function fence(label, text) {
  return `\n\`\`\`${label}\n${text}\n\`\`\`\n`;
}

function trimOutput(text) {
  const max = 8000;
  const value = String(text || '').trim();
  return value.length > max ? `${value.slice(0, max)}\n... [trimmed]` : value;
}

function firstLine(text) {
  return String(text || '').trim().split(/\r?\n/)[0] || 'no output';
}

function quote(value) {
  const stringValue = String(value);
  if (process.platform === 'win32') return `"${stringValue.replace(/"/g, '\\"')}"`;
  return `'${stringValue.replace(/'/g, "'\\''")}'`;
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

function fail(message) {
  console.error(message);
  process.exit(1);
}
