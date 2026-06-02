#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');
const { spawnSync } = require('child_process');

const options = parseArgs(process.argv.slice(2));
const results = [];

main();

function main() {
  checkRuntime();
  checkPackageScripts();
  checkRequiredDocs();
  checkRequiredCommands();
  checkEnvironment();

  if (options.deep) {
    runDeepChecks();
  }

  printResults();
  const hasFail = results.some((result) => result.level === 'fail');
  process.exit(hasFail ? 1 : 0);
}

function checkRuntime() {
  const major = Number(process.versions.node.split('.')[0]);
  add(major >= 18 ? 'pass' : 'fail', 'Node.js >= 18', `current ${process.version}`, 'Install Node.js 18 or newer.');
}

function checkPackageScripts() {
  const pkg = readJson('package.json');
  const scripts = pkg?.scripts || {};
  [
    'ai:test:quick',
    'report',
    'report:discord:unit:dry',
    'review:claude:dry',
    'review:codex:dry',
    'planning:pipeline:dry',
    'planning:pipeline:prompt',
    'planning:pipeline',
    'playtest:summary',
    'playtest:summary:dry',
    'playtest:package',
    'playtest:package:dry',
  ].forEach((name) => {
    add(scripts[name] ? 'pass' : 'fail', `npm script ${name}`, scripts[name] || 'missing', `Add ${name} to package.json scripts.`);
  });
}

function checkRequiredDocs() {
  const docs = [
    {
      file: 'docs/ai/ai-collaboration.md',
      tokens: ['## Purpose', '## Roles', '## Workflow', '## Evidence Rules', '## Developer Control Rules'],
    },
    {
      file: 'docs/ai/ai-failure-cases.md',
      tokens: ['## Case 01', '### AI/Automation Attempt', '### Developer Fix', '### Verification', '## Template'],
    },
    {
      file: 'docs/adr/ADR-001-html-prototype-before-unity.md',
      tokens: ['## Status', '## Context', '## Decision', '## AI Collaboration', '## Consequences'],
    },
    {
      file: 'docs/adr/ADR-002-test-result-planning-pipeline.md',
      tokens: ['## Status', '## Context', '## Decision', '## Verification'],
    },
    {
      file: 'docs/portfolio/LETHE-case-study.md',
      tokens: ['## Problem', '## Constraints', '## AI Usage', '## Developer Judgment', '## Verification Evidence'],
    },
    {
      file: 'docs/CODEX_RUNBOOK.md',
      tokens: ['## Planning Pipeline', '## GPT And Claude Planning', '## Approval Reality'],
    },
    {
      file: 'docs/review_prompts/README.md',
      tokens: ['## 테스트 결과 기반 자동 파이프라인', 'planning:pipeline:prompt'],
    },
  ];

  docs.forEach((doc) => {
    const full = path.resolve(doc.file);
    if (!fs.existsSync(full)) {
      add('fail', `doc ${doc.file}`, 'missing', `Create ${doc.file}.`);
      return;
    }

    const content = fs.readFileSync(full, 'utf8');
    const missing = doc.tokens.filter((token) => !content.includes(token));
    add(
      missing.length ? 'fail' : 'pass',
      `doc sections ${doc.file}`,
      missing.length ? `missing: ${missing.join(', ')}` : 'ok',
      missing.length ? `Add the missing role/rule sections to ${doc.file}.` : ''
    );
  });
}

function checkRequiredCommands() {
  [
    { command: 'git --version', required: true },
    { command: 'npm --version', required: true },
    { command: 'node --version', required: true },
    { command: 'claude --version', required: false, fix: 'Install/login Claude Code or use Codex fallback.' },
    { command: 'codex --version', required: false, fix: 'Install/login Codex CLI or use Claude only.' },
  ].forEach((entry) => {
    const result = run(entry.command);
    const level = result.status === 0 ? 'pass' : entry.required ? 'fail' : 'warn';
    add(level, `command ${entry.command}`, result.status === 0 ? firstLine(result.stdout || result.stderr) : 'not available', entry.fix || `Install or expose ${entry.command.split(' ')[0]} on PATH.`);
  });
}

function checkEnvironment() {
  const hasEnv = fs.existsSync(path.resolve('.env'));
  add(hasEnv ? 'pass' : 'warn', '.env file', hasEnv ? 'present' : 'missing', 'Only needed for Discord webhook delivery.');

  if (hasEnv) {
    const env = fs.readFileSync(path.resolve('.env'), 'utf8');
    add(env.includes('DISCORD_WEBHOOK_URL=') ? 'pass' : 'warn', 'DISCORD_WEBHOOK_URL', env.includes('DISCORD_WEBHOOK_URL=') ? 'configured' : 'missing', 'Set DISCORD_WEBHOOK_URL only on trusted locals that should send Discord notices.');
  }

  const prompt = latestMatchingFile('docs/review_prompts', /^\d{4}-\d{2}-\d{2}(?:-[a-z0-9-]+)?\.md$/i);
  add(prompt ? 'pass' : 'fail', 'latest review prompt', prompt || 'missing', 'Run npm run planning:pipeline:prompt to generate one.');
}

function runDeepChecks() {
  [
    'node --check scripts/run_planning_pipeline.js',
    'node --check scripts/ask_claude_review.js',
    'node --check scripts/ask_codex_review.js',
    'node --check scripts/send_discord_report.js',
    'node --check scripts/summarize_playtests.js',
    'node --check scripts/prepare_playtest_build.js',
    'npm run planning:pipeline:dry',
    'npm run playtest:summary:dry',
    'npm run playtest:package:dry',
    'npm run report:discord:unit:dry',
  ].forEach((command) => {
    const result = run(command, { maxBuffer: 1024 * 1024 * 8 });
    add(result.status === 0 ? 'pass' : 'fail', `deep ${command}`, result.status === 0 ? 'ok' : firstLine(result.stderr || result.stdout), `Run ${command} directly for full output.`);
  });
}

function run(command, opts = {}) {
  return spawnSync(command, {
    cwd: process.cwd(),
    encoding: 'utf8',
    shell: true,
    maxBuffer: opts.maxBuffer || 1024 * 1024,
  });
}

function latestMatchingFile(dir, pattern) {
  const fullDir = path.resolve(dir);
  if (!fs.existsSync(fullDir)) return '';
  const files = fs.readdirSync(fullDir)
    .filter((file) => pattern.test(file))
    .map((file) => ({
      file,
      mtimeMs: fs.statSync(path.join(fullDir, file)).mtimeMs,
    }))
    .sort((a, b) => a.mtimeMs - b.mtimeMs || a.file.localeCompare(b.file));
  return files.length ? path.join(dir, files[files.length - 1].file) : '';
}

function readJson(file) {
  try {
    return JSON.parse(fs.readFileSync(path.resolve(file), 'utf8'));
  } catch {
    add('fail', `read ${file}`, 'invalid or missing', `Fix ${file}.`);
    return null;
  }
}

function parseArgs(args) {
  return {
    deep: args.includes('--deep'),
  };
}

function add(level, name, detail, fix = '') {
  results.push({ level, name, detail, fix });
}

function printResults() {
  const icons = { pass: 'PASS', warn: 'WARN', fail: 'FAIL' };
  console.log('LETHE local pipeline doctor');
  console.log('');
  results.forEach((result) => {
    console.log(`[${icons[result.level]}] ${result.name}: ${result.detail}`);
    if (result.level !== 'pass' && result.fix) console.log(`       fix: ${result.fix}`);
  });
  console.log('');
  const counts = results.reduce((acc, result) => {
    acc[result.level] += 1;
    return acc;
  }, { pass: 0, warn: 0, fail: 0 });
  console.log(`Summary: ${counts.pass} pass, ${counts.warn} warn, ${counts.fail} fail`);
}

function firstLine(text) {
  return String(text || '').trim().split(/\r?\n/)[0] || 'no output';
}
