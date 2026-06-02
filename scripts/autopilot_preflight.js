#!/usr/bin/env node
'use strict';

const fs = require('fs');
const { spawnSync } = require('child_process');

const options = parseArgs(process.argv.slice(2));
const results = [];

main();

function main() {
  if (options.dryRun) {
    printDryRun();
    return;
  }

  checkGitState();
  checkNpmScripts();
  runRequired('npm run doctor:deep', 'local doctor deep');
  runRequired('npm run review:claude:dry', 'Claude prompt dry-run');
  runRequired('npm run review:codex:dry', 'Codex fallback dry-run');
  runRequired('npm run report:discord:unit:dry', 'Discord work-unit dry-run');
  checkClaudeCommand();
  checkCodexCommand();
  checkDiscordEnv();

  if (options.external) {
    checkClaudeAuth();
  } else {
    add(
      'warn',
      'Claude auth live check',
      'skipped',
      'Run `npm run autopilot:preflight` from a trusted local terminal before unattended automation.'
    );
  }

  printResults();
  process.exit(results.some((result) => result.level === 'fail') ? 1 : 0);
}

function printDryRun() {
  console.log('LETHE autopilot preflight dry-run');
  console.log('');
  console.log('Checks that would run:');
  [
    'git working tree cleanliness',
    'required npm scripts',
    'npm run doctor:deep',
    'npm run review:claude:dry',
    'npm run review:codex:dry',
    'npm run report:discord:unit:dry',
    'claude --version',
    'codex --version',
    '.env DISCORD_WEBHOOK_URL presence',
    'minimal Claude auth prompt when --external is enabled',
  ].forEach((line) => console.log(`- ${line}`));
  console.log('');
  console.log('Use `npm run autopilot:preflight:local` for no external call.');
  console.log('Use `npm run autopilot:preflight` for the full Claude auth check.');
}

function checkGitState() {
  const result = run('git status --porcelain');
  if (result.status !== 0) {
    add('fail', 'git status', firstLine(result.stderr || result.stdout), 'Fix Git availability before autopilot.');
    return;
  }

  const dirty = result.stdout.trim();
  if (!dirty) {
    add('pass', 'git working tree', 'clean');
    return;
  }

  add(
    options.allowDirty ? 'warn' : 'fail',
    'git working tree',
    dirty.split(/\r?\n/).slice(0, 5).join(' / '),
    'Commit, stash, ignore, or move unrelated files before unattended automation. Use --allow-dirty only for deliberate local checks.'
  );
}

function checkNpmScripts() {
  let scripts = {};
  try {
    scripts = JSON.parse(fs.readFileSync('package.json', 'utf8')).scripts || {};
  } catch {
    add('fail', 'package.json scripts', 'unreadable', 'Fix package.json.');
    return;
  }

  [
    'doctor:deep',
    'review:claude:dry',
    'review:codex:dry',
    'planning:pipeline:prompt',
    'report:discord:unit:dry',
    'autopilot:preflight',
    'autopilot:preflight:local',
    'autopilot:preflight:dry',
    'overnight:loop',
    'overnight:loop:dry',
  ].forEach((name) => {
    add(scripts[name] ? 'pass' : 'fail', `npm script ${name}`, scripts[name] || 'missing', `Add ${name} to package.json.`);
  });
}

function runRequired(command, name) {
  const result = run(command, { maxBuffer: 1024 * 1024 * 8 });
  add(
    result.status === 0 ? 'pass' : 'fail',
    name,
    result.status === 0 ? 'ok' : firstLine(result.stderr || result.stdout),
    `Run \`${command}\` directly for full output.`
  );
}

function checkClaudeCommand() {
  const result = run('claude --version');
  add(
    result.status === 0 ? 'pass' : 'fail',
    'claude command',
    result.status === 0 ? firstLine(result.stdout || result.stderr) : 'not available',
    'Install Claude Code or expose `claude` on PATH.'
  );
}

function checkCodexCommand() {
  const result = run('codex --version');
  add(
    result.status === 0 ? 'pass' : 'warn',
    'codex fallback command',
    result.status === 0 ? firstLine(result.stdout || result.stderr) : 'not available',
    'Install/login Codex CLI if Claude cannot be used.'
  );
}

function checkDiscordEnv() {
  if (!fs.existsSync('.env')) {
    add('warn', 'Discord webhook env', '.env missing', 'Only trusted locals that send Discord notices need DISCORD_WEBHOOK_URL.');
    return;
  }

  const env = fs.readFileSync('.env', 'utf8');
  add(
    env.includes('DISCORD_WEBHOOK_URL=') ? 'pass' : 'warn',
    'Discord webhook env',
    env.includes('DISCORD_WEBHOOK_URL=') ? 'configured' : 'missing',
    'Set DISCORD_WEBHOOK_URL if this local should send completion/attention notices.'
  );
}

function checkClaudeAuth() {
  const result = spawnSync('claude', [
    '-p',
    '--tools',
    '',
    '--output-format',
    'text',
  ], {
    cwd: process.cwd(),
    encoding: 'utf8',
    input: '인증 확인용입니다. 프로젝트 정보 없이 OK만 답하세요.',
    shell: process.platform === 'win32',
    timeout: 30000,
    maxBuffer: 1024 * 1024,
  });

  if (result.error) {
    add('fail', 'Claude auth live check', result.error.message, 'Run `claude` locally, complete login/authentication, then retry.');
    return;
  }

  if (result.status !== 0) {
    const detail = [result.stderr, result.stdout].filter(Boolean).join('\n').trim();
    const fix = /401|invalid authentication credentials|authenticate/i.test(detail)
      ? 'Run `claude` locally, complete login/authentication, then retry `npm run autopilot:preflight`.'
      : 'Run the same minimal Claude command directly for full output.';
    add('fail', 'Claude auth live check', firstLine(detail) || `exit ${result.status}`, fix);
    return;
  }

  add('pass', 'Claude auth live check', firstLine(result.stdout) || 'ok');
}

function run(command, opts = {}) {
  return spawnSync(command, {
    cwd: process.cwd(),
    encoding: 'utf8',
    shell: true,
    maxBuffer: opts.maxBuffer || 1024 * 1024,
  });
}

function parseArgs(args) {
  return {
    allowDirty: args.includes('--allow-dirty'),
    dryRun: args.includes('--dry-run'),
    external: args.includes('--external'),
  };
}

function add(level, name, detail, fix = '') {
  results.push({ level, name, detail, fix });
}

function printResults() {
  console.log('LETHE autopilot preflight');
  console.log('');
  results.forEach((result) => {
    console.log(`[${result.level.toUpperCase()}] ${result.name}: ${result.detail}`);
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
