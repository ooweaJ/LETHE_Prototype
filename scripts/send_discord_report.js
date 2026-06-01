#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');

const args = process.argv.slice(2);
const dryRun = args.includes('--dry-run');
const input = args.find((arg) => !arg.startsWith('--')) || 'docs/reports/2026-06-01.md';
const markdownPath = path.resolve(input);
const htmlPath = markdownPath.replace(/\.md$/i, '.html');

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
  const message = buildMessage(markdown, markdownPath, htmlPath);

  if (dryRun) {
    console.log(message);
    console.log(`Attachment: ${path.relative(process.cwd(), htmlPath)} (${html.length} bytes)`);
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

function buildMessage(markdown, markdownFile, htmlFile) {
  const title = firstHeading(markdown) || path.basename(markdownFile);
  const reportDate = path.basename(markdownFile, '.md');
  const bullets = summaryBullets(markdown, 4);
  const lines = [
    `LETHE Daily Report - ${reportDate}`,
    title,
    '',
    ...bullets,
    '',
    `Attachment: ${path.basename(htmlFile)}`,
  ];

  return lines.join('\n').slice(0, 1800);
}

function firstHeading(markdown) {
  const match = markdown.match(/^#\s+(.+)$/m);
  return match ? stripMarkdown(match[1]).trim() : '';
}

function summaryBullets(markdown, limit) {
  const bullets = markdown
    .split(/\r?\n/)
    .map((line) => line.match(/^\s*-\s+(.+)$/))
    .filter(Boolean)
    .map((match) => `- ${stripMarkdown(match[1]).trim()}`)
    .filter((line) => line.length > 2);

  if (bullets.length > 0) return bullets.slice(0, limit);
  return ['- Report generated from Markdown source.'];
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
