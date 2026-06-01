#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');

const options = parseArgs(process.argv.slice(2));
const dryRun = options.dryRun;
const input = options.input || 'docs/reports/2026-06-01.md';
const markdownPath = path.resolve(input);
const htmlPath = markdownPath.replace(/\.md$/i, '.html');
const promptPath = resolvePromptPath(options.prompt, markdownPath);

loadDotEnv(path.resolve('.env'));

main().catch((error) => {
  console.error(error.message);
  process.exit(1);
});

async function main() {
  if (!fs.existsSync(markdownPath)) {
    throw new Error(`Report markdown not found: ${path.relative(process.cwd(), markdownPath)}`);
  }

  if (!fs.existsSync(htmlPath)) {
    throw new Error(`Report HTML not found. Run npm run report first: ${path.relative(process.cwd(), htmlPath)}`);
  }

  const markdown = fs.readFileSync(markdownPath, 'utf8');
  const html = fs.readFileSync(htmlPath);
  const prompt = promptPath ? fs.readFileSync(promptPath) : null;
  const message = buildMessage(markdown, markdownPath, htmlPath, promptPath);

  if (dryRun) {
    console.log(message);
    console.log(`Attachment: ${path.relative(process.cwd(), htmlPath)} (${html.length} bytes)`);
    if (promptPath) {
      console.log(`Attachment: ${path.relative(process.cwd(), promptPath)} (${prompt.length} bytes)`);
    }
    return;
  }

  const webhookUrl = process.env.DISCORD_WEBHOOK_URL;
  if (!webhookUrl) {
    throw new Error('DISCORD_WEBHOOK_URL is not set. Create a Discord channel webhook and set it before running this script.');
  }

  const form = new FormData();
  form.append('payload_json', JSON.stringify({
    content: message,
    allowed_mentions: { parse: [] },
  }));
  form.append('files[0]', new Blob([html], { type: 'text/html' }), path.basename(htmlPath));
  if (promptPath) {
    form.append('files[1]', new Blob([prompt], { type: 'text/markdown' }), path.basename(promptPath));
  }

  const response = await fetch(webhookUrl, {
    method: 'POST',
    body: form,
  });

  if (!response.ok) {
    const body = await response.text();
    throw new Error(`Discord upload failed: ${response.status} ${response.statusText}\n${body}`);
  }

  console.log(`Uploaded ${path.relative(process.cwd(), htmlPath)} to Discord.`);
}

function buildMessage(markdown, markdownFile, htmlFile, reviewPromptPath) {
  const reportDate = path.basename(markdownFile, '.md');
  const work = bulletsFromSections(markdown, [
    'what changed today',
    'what changed',
    'today work',
    '오늘 한 일',
    '작업 내용',
  ], 2);
  const status = bulletsFromSections(markdown, [
    'current build status',
    'current build',
    'build status',
    '현재 상태',
    '완료 상태',
  ], 2);
  const problems = bulletsFromSections(markdown, [
    'problems or risks',
    'problems and risks',
    'risks',
    '문제와 리스크',
    '문제',
  ], 2);
  const handoff = bulletsFromSections(markdown, [
    'gpt handoff summary',
    'planning handoff',
    '기획 핸드오프',
    'gpt 전달',
  ], 1);
  const planningLine = reviewPromptPath
    ? `있음 - ${path.relative(process.cwd(), reviewPromptPath)}`
    : handoff.length > 0
      ? fit(handoff[0], 220)
      : '없음';
  const lines = [
    `LETHE Report - ${reportDate}`,
    `작업: ${fit(joinBullets(work) || '보고서가 갱신되었습니다.', 260)}`,
    `완료: ${fit(joinBullets(status) || 'HTML 보고서 생성 완료', 220)}`,
    `문제: ${fit(joinBullets(problems) || '새로 기록된 문제 없음', 220)}`,
    `기획질문: ${planningLine}`,
    `첨부: ${path.basename(htmlFile)}${reviewPromptPath ? `, ${path.basename(reviewPromptPath)}` : ''}`,
  ];

  return lines.join('\n').slice(0, 1800);
}

function parseArgs(args) {
  const parsed = {
    dryRun: false,
    input: '',
    prompt: '',
  };

  for (let index = 0; index < args.length; index += 1) {
    const arg = args[index];
    if (arg === '--dry-run') {
      parsed.dryRun = true;
    } else if (arg === '--prompt') {
      parsed.prompt = args[index + 1] || '';
      index += 1;
    } else if (!arg.startsWith('--') && !parsed.input) {
      parsed.input = arg;
    }
  }

  return parsed;
}

function resolvePromptPath(explicitPrompt, reportPath) {
  if (explicitPrompt) {
    const explicitPath = path.resolve(explicitPrompt);
    return fs.existsSync(explicitPath) ? explicitPath : '';
  }

  const date = path.basename(reportPath, '.md');
  const defaultPath = path.resolve('docs', 'review_prompts', `${date}.md`);
  return fs.existsSync(defaultPath) ? defaultPath : '';
}

function bulletsFromSections(markdown, headingNames, limit) {
  const sections = headingNames
    .flatMap((heading) => findSections(markdown, heading))
    .sort((left, right) => left.index - right.index);
  const latestSection = sections[sections.length - 1]?.content || '';
  const bullets = (latestSection.match(/^\s*-\s+(.+)$/gm) || [])
    .map((line) => stripMarkdown(line.replace(/^\s*-\s+/, '')).trim())
    .filter(Boolean);

  return bullets.slice(0, limit);
}

function findSections(markdown, headingName) {
  const lines = markdown.split(/\r?\n/);
  const sections = [];
  const target = normalize(headingName);

  for (let index = 0; index < lines.length; index += 1) {
    const heading = lines[index].match(/^(#{1,6})\s+(.+)$/);
    if (!heading || normalize(heading[2]) !== target) continue;

    const level = heading[1].length;
    const content = [];
    for (let next = index + 1; next < lines.length; next += 1) {
      const nextHeading = lines[next].match(/^(#{1,6})\s+(.+)$/);
      if (nextHeading && nextHeading[1].length <= level) break;
      content.push(lines[next]);
    }
    sections.push({
      index,
      content: content.join('\n'),
    });
  }

  return sections;
}

function joinBullets(bullets) {
  return bullets.join(' / ');
}

function fit(text, maxLength) {
  if (text.length <= maxLength) return text;
  return `${text.slice(0, maxLength - 3).trim()}...`;
}

function normalize(text) {
  return stripMarkdown(text).trim().toLowerCase().replace(/\s+/g, ' ');
}

function stripMarkdown(text) {
  return String(text)
    .replace(/`([^`]+)`/g, '$1')
    .replace(/\*\*([^*]+)\*\*/g, '$1')
    .replace(/\[([^\]]+)\]\([^)]+\)/g, '$1');
}

function loadDotEnv(filePath) {
  if (!fs.existsSync(filePath)) return;

  const lines = fs.readFileSync(filePath, 'utf8').split(/\r?\n/);
  for (const line of lines) {
    const trimmed = line.trim();
    if (!trimmed || trimmed.startsWith('#')) continue;

    const separator = trimmed.indexOf('=');
    if (separator === -1) continue;

    const key = trimmed.slice(0, separator).trim();
    const value = unquote(trimmed.slice(separator + 1).trim());
    if (key && process.env[key] === undefined) {
      process.env[key] = value;
    }
  }
}

function unquote(value) {
  const quote = value[0];
  if ((quote === '"' || quote === "'") && value[value.length - 1] === quote) {
    return value.slice(1, -1);
  }
  return value;
}
