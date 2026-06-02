#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');

const options = parseArgs(process.argv.slice(2));
const version = options.version || readVersion();
const outDir = path.resolve(options.out || path.join('dist', `lethe-${version}-playtest`));

main();

function main() {
  const files = [
    ['index.html', 'index.html'],
    ['style.css', 'style.css'],
    ['src/game.js', 'src/game.js'],
    ['docs/HUMAN_PLAYTEST_GUIDE.md', 'HUMAN_PLAYTEST_GUIDE.md'],
    ['docs/PLAYTEST_NOTES.md', 'PLAYTEST_NOTES_TEMPLATE.md'],
    ['docs/playtest_summaries/README.md', 'PLAYTEST_SUMMARY_README.md'],
  ];

  if (options.dryRun) {
    console.log(`Output: ${path.relative(process.cwd(), outDir)}`);
    files.forEach(([from, to]) => console.log(`${from} -> ${to}`));
    console.log('README.md -> README.md');
    return;
  }

  fs.rmSync(outDir, { recursive: true, force: true });
  files.forEach(([from, to]) => copyFile(from, path.join(outDir, to)));
  fs.writeFileSync(path.join(outDir, 'README.md'), buildReadme(), 'utf8');
  console.log(`Wrote ${path.relative(process.cwd(), outDir)}`);
  console.log('Open index.html in a browser to start the playtest.');
}

function copyFile(from, to) {
  const src = path.resolve(from);
  if (!fs.existsSync(src)) throw new Error(`Missing source file: ${from}`);
  fs.mkdirSync(path.dirname(to), { recursive: true });
  fs.copyFileSync(src, to);
}

function buildReadme() {
  return [
    `# LETHE ${version} Playtest Build`,
    '',
    '## Start',
    '',
    'Open `index.html` in Chrome or Edge.',
    '',
    'Before each run, enter:',
    '',
    '- tester ID, for example `T01`',
    '- session number, for example `S01`',
    '',
    'You can also prefill metadata with URL parameters:',
    '',
    '```text',
    'index.html?tester=T01&session=S01',
    '```',
    '',
    '## Required Output',
    '',
    'After the result survey, click `JSON 로그 다운로드` and save the downloaded file.',
    '',
    'After collecting logs, copy them into the repository `playtest_logs/` folder and run:',
    '',
    '```bash',
    'npm run playtest:summary',
    '```',
    '',
    '## Scope',
    '',
    'Do not evaluate Unity implementation quality yet. This build validates early fun, run growth, forgetting emotion, and whether the idea is promising enough to continue.',
    '',
    'See `HUMAN_PLAYTEST_GUIDE.md` for the full test protocol.',
    '',
  ].join('\n');
}

function readVersion() {
  const html = fs.readFileSync(path.resolve('index.html'), 'utf8');
  const match = html.match(/HTML Alpha Prototype\s+(v[0-9.]+)/i) || html.match(/망각의 군주\s+(v[0-9.]+)/i);
  return match ? match[1] : 'v0.5';
}

function parseArgs(args) {
  const parsed = {
    dryRun: false,
    out: '',
    version: '',
  };
  for (let index = 0; index < args.length; index += 1) {
    const arg = args[index];
    if (arg === '--dry-run') {
      parsed.dryRun = true;
    } else if (arg === '--out') {
      parsed.out = args[index + 1] || '';
      index += 1;
    } else if (arg.startsWith('--out=')) {
      parsed.out = arg.slice('--out='.length);
    } else if (arg === '--version') {
      parsed.version = args[index + 1] || '';
      index += 1;
    } else if (arg.startsWith('--version=')) {
      parsed.version = arg.slice('--version='.length);
    }
  }
  return parsed;
}
