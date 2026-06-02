#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');
const { spawnSync } = require('child_process');

const options = parseArgs(process.argv.slice(2));
const promptPath = path.resolve(options.prompt || latestMarkdown('docs/review_prompts'));
const outputPath = path.resolve(options.output || defaultOutputPath(promptPath));
const model = options.model || process.env.CODEX_REVIEW_MODEL || '';

main();

function main() {
  if (!fs.existsSync(promptPath)) {
    fail(`Prompt not found: ${path.relative(process.cwd(), promptPath)}`);
  }

  const prompt = fs.readFileSync(promptPath, 'utf8');
  const fullPrompt = buildPrompt(prompt);
  const args = buildCodexArgs(outputPath);

  if (options.dryRun) {
    console.log(`Prompt: ${path.relative(process.cwd(), promptPath)}`);
    console.log(`Output: ${path.relative(process.cwd(), outputPath)}`);
    console.log(`Command: codex ${args.join(' ')} < prompt`);
    console.log('');
    console.log(fullPrompt.slice(0, 1200));
    if (fullPrompt.length > 1200) console.log('...');
    return;
  }

  fs.mkdirSync(path.dirname(outputPath), { recursive: true });

  const result = spawnSync('codex', args, {
    cwd: process.cwd(),
    encoding: 'utf8',
    input: fullPrompt,
    shell: process.platform === 'win32',
    maxBuffer: 1024 * 1024 * 8,
  });

  if (result.error) fail(result.error.message);
  if (result.status !== 0) {
    const stderr = result.stderr ? `\n${result.stderr.trim()}` : '';
    fail(`Codex exited with status ${result.status}.${stderr}`);
  }

  if (!fs.existsSync(outputPath)) fail('Codex did not write an output file.');
  const response = fs.readFileSync(outputPath, 'utf8').trim();
  if (!response) fail('Codex returned an empty response.');

  fs.writeFileSync(outputPath, `${response}\n`, 'utf8');
  console.log(`Wrote ${path.relative(process.cwd(), outputPath)}`);
}

function buildCodexArgs(outputPathValue) {
  const args = [
    'exec',
    '--cd',
    process.cwd(),
    '--sandbox',
    'read-only',
    '--output-last-message',
    outputPathValue,
  ];
  if (model) args.push('--model', model);
  args.push('-');
  return args;
}

function buildPrompt(prompt) {
  return [
    '너는 LETHE 프로젝트의 테스트 결과 기반 기획 파트너다.',
    '반드시 한국어로 답변한다.',
    '코드 수정은 하지 말고, AI/사람 테스트 결과를 해석해 Codex가 바로 구현할 수 있는 기획 판단과 작업 목록만 작성한다.',
    '현재 목표는 HTML 프로토타입으로 핵심 재미와 가능성을 검증하고, 충분히 가능성이 보이면 Unity 구현 단계로 넘어갈 근거를 만드는 것이다.',
    '답변에는 반드시 "앞으로 해야 할 일" 섹션을 포함한다.',
    '',
    prompt.trim(),
  ].join('\n');
}

function parseArgs(args) {
  const parsed = {
    dryRun: false,
    prompt: '',
    output: '',
    model: '',
  };

  for (let index = 0; index < args.length; index += 1) {
    const arg = args[index];
    if (arg === '--dry-run') {
      parsed.dryRun = true;
    } else if (arg === '--prompt') {
      parsed.prompt = args[index + 1] || '';
      index += 1;
    } else if (arg.startsWith('--prompt=')) {
      parsed.prompt = arg.slice('--prompt='.length);
    } else if (arg === '--output') {
      parsed.output = args[index + 1] || '';
      index += 1;
    } else if (arg.startsWith('--output=')) {
      parsed.output = arg.slice('--output='.length);
    } else if (arg === '--model') {
      parsed.model = args[index + 1] || '';
      index += 1;
    } else if (arg.startsWith('--model=')) {
      parsed.model = arg.slice('--model='.length);
    }
  }

  return parsed;
}

function latestMarkdown(dir) {
  const fullDir = path.resolve(dir);
  if (!fs.existsSync(fullDir)) fail(`Directory not found: ${dir}`);

  const files = fs.readdirSync(fullDir)
    .filter((file) => /^\d{4}-\d{2}-\d{2}(?:-[a-z0-9-]+)?\.md$/i.test(file))
    .map((file) => ({
      file,
      mtimeMs: fs.statSync(path.join(fullDir, file)).mtimeMs,
    }))
    .sort((a, b) => a.mtimeMs - b.mtimeMs || a.file.localeCompare(b.file));

  if (files.length === 0) fail(`No dated Markdown prompts found in ${dir}`);
  return path.join(dir, files[files.length - 1].file);
}

function defaultOutputPath(inputPath) {
  const date = path.basename(inputPath, '.md');
  return path.join('docs', 'review_responses', `${date}-codex.md`);
}

function fail(message) {
  console.error(message);
  process.exit(1);
}
