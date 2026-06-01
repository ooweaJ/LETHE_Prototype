#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');

loadDotEnv();

const options = parseArgs(process.argv.slice(2));
const promptPath = path.resolve(options.prompt || latestMarkdown('docs/review_prompts'));
const outputPath = path.resolve(options.output || defaultOutputPath(promptPath));
const model = options.model || process.env.OPENAI_REVIEW_MODEL || 'gpt-5.2';

main().catch((error) => {
  console.error(error.message || error);
  process.exit(1);
});

async function main() {
  if (!fs.existsSync(promptPath)) {
    fail(`Prompt not found: ${path.relative(process.cwd(), promptPath)}`);
  }

  const prompt = fs.readFileSync(promptPath, 'utf8');
  const fullPrompt = buildPrompt(prompt);

  if (options.dryRun) {
    console.log(`Prompt: ${path.relative(process.cwd(), promptPath)}`);
    console.log(`Output: ${path.relative(process.cwd(), outputPath)}`);
    console.log(`Model: ${model}`);
    console.log('Endpoint: POST https://api.openai.com/v1/responses');
    console.log('');
    console.log(fullPrompt.slice(0, 1200));
    if (fullPrompt.length > 1200) console.log('...');
    return;
  }

  const apiKey = process.env.OPENAI_API_KEY;
  if (!apiKey) {
    fail('OPENAI_API_KEY is missing. Add it to the environment or local .env, then rerun.');
  }

  fs.mkdirSync(path.dirname(outputPath), { recursive: true });

  const response = await fetch('https://api.openai.com/v1/responses', {
    method: 'POST',
    headers: {
      Authorization: `Bearer ${apiKey}`,
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      model,
      input: [
        {
          role: 'system',
          content: [
            {
              type: 'input_text',
              text: [
                '너는 LETHE 프로젝트의 기획 검토자다.',
                '반드시 한국어로 답변한다.',
                '코드 수정은 하지 말고, Codex가 바로 구현할 수 있는 기획 판단과 작업 목록만 작성한다.',
                '답변에는 반드시 "앞으로 해야 할 일" 섹션을 포함한다.',
              ].join('\n'),
            },
          ],
        },
        {
          role: 'user',
          content: [{ type: 'input_text', text: fullPrompt }],
        },
      ],
      reasoning: { effort: options.reasoningEffort || 'medium' },
      max_output_tokens: options.maxOutputTokens,
    }),
  });

  const data = await response.json().catch(() => null);
  if (!response.ok) {
    const message = data?.error?.message || `${response.status} ${response.statusText}`;
    fail(`OpenAI review failed: ${message}`);
  }

  const text = extractText(data).trim();
  if (!text) fail('OpenAI returned an empty response.');

  fs.writeFileSync(outputPath, text.endsWith('\n') ? text : `${text}\n`, 'utf8');
  console.log(`Wrote ${path.relative(process.cwd(), outputPath)}`);
}

function buildPrompt(prompt) {
  return [
    '# LETHE 기획 검토 입력',
    '',
    prompt.trim(),
  ].join('\n');
}

function extractText(data) {
  if (typeof data?.output_text === 'string') return data.output_text;
  const chunks = [];
  for (const item of data?.output || []) {
    for (const content of item.content || []) {
      if (typeof content.text === 'string') chunks.push(content.text);
    }
  }
  return chunks.join('\n');
}

function parseArgs(args) {
  const parsed = {
    dryRun: false,
    prompt: '',
    output: '',
    model: '',
    reasoningEffort: '',
    maxOutputTokens: 4096,
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
    } else if (arg === '--reasoning-effort') {
      parsed.reasoningEffort = args[index + 1] || '';
      index += 1;
    } else if (arg.startsWith('--reasoning-effort=')) {
      parsed.reasoningEffort = arg.slice('--reasoning-effort='.length);
    } else if (arg === '--max-output-tokens') {
      parsed.maxOutputTokens = Number(args[index + 1] || parsed.maxOutputTokens);
      index += 1;
    } else if (arg.startsWith('--max-output-tokens=')) {
      parsed.maxOutputTokens = Number(arg.slice('--max-output-tokens='.length));
    }
  }

  return parsed;
}

function latestMarkdown(dir) {
  const fullDir = path.resolve(dir);
  if (!fs.existsSync(fullDir)) fail(`Directory not found: ${dir}`);

  const files = fs.readdirSync(fullDir)
    .filter((file) => /^\d{4}-\d{2}-\d{2}\.md$/.test(file))
    .sort();

  if (files.length === 0) fail(`No dated Markdown prompts found in ${dir}`);
  return path.join(dir, files[files.length - 1]);
}

function defaultOutputPath(inputPath) {
  const date = path.basename(inputPath, '.md');
  return path.join('docs', 'review_responses', `${date}-openai.md`);
}

function loadDotEnv() {
  const envPath = path.resolve('.env');
  if (!fs.existsSync(envPath)) return;
  const lines = fs.readFileSync(envPath, 'utf8').split(/\r?\n/);
  for (const line of lines) {
    const trimmed = line.trim();
    if (!trimmed || trimmed.startsWith('#')) continue;
    const index = trimmed.indexOf('=');
    if (index <= 0) continue;
    const key = trimmed.slice(0, index).trim();
    let value = trimmed.slice(index + 1).trim();
    if ((value.startsWith('"') && value.endsWith('"')) || (value.startsWith("'") && value.endsWith("'"))) {
      value = value.slice(1, -1);
    }
    if (!process.env[key]) process.env[key] = value;
  }
}

function fail(message) {
  console.error(message);
  process.exit(1);
}
