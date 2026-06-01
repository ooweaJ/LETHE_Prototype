#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');

const options = parseArgs(process.argv.slice(2));

loadDotEnv(path.resolve('.env'));

main().catch((error) => {
  console.error(error.message);
  process.exit(1);
});

async function main() {
  const message = buildMessage(options);

  if (options.dryRun) {
    console.log(message);
    return;
  }

  const webhookUrl = process.env.CODEX_STATUS_WEBHOOK_URL || process.env.DISCORD_WEBHOOK_URL;
  if (!webhookUrl) {
    throw new Error('Set CODEX_STATUS_WEBHOOK_URL or DISCORD_WEBHOOK_URL before sending Codex notices.');
  }

  const response = await fetch(webhookUrl, {
    method: 'POST',
    headers: { 'content-type': 'application/json' },
    body: JSON.stringify({
      content: message,
      allowed_mentions: { parse: [] },
    }),
  });

  if (!response.ok) {
    const body = await response.text();
    throw new Error(`Discord notice failed: ${response.status} ${response.statusText}\n${body}`);
  }

  console.log('Sent Codex notice to Discord.');
}

function buildMessage(opts) {
  const type = normalizeType(opts.type);
  const title = opts.title || defaultTitle(type);
  const lines = [
    `LETHE Codex Notice - ${type.toUpperCase()}`,
    `제목: ${fit(title, 160)}`,
  ];

  if (opts.summary) lines.push(`상태: ${fit(opts.summary, 320)}`);
  if (opts.blocker) lines.push(`막힘: ${fit(opts.blocker, 320)}`);
  if (opts.next) lines.push(`다음: ${fit(opts.next, 320)}`);
  if (opts.file) lines.push(`기록: ${opts.file}`);

  if (type === 'approval') {
    lines.push('확인: Codex UI에서 권한 승인이 필요할 수 있음');
  }

  return lines.join('\n').slice(0, 1800);
}

function parseArgs(args) {
  const parsed = {
    type: 'status',
    title: '',
    summary: '',
    blocker: '',
    next: '',
    file: '',
    dryRun: false,
  };

  for (let index = 0; index < args.length; index += 1) {
    const arg = args[index];
    if (arg === '--dry-run') {
      parsed.dryRun = true;
    } else if (arg.startsWith('--')) {
      const equalIndex = arg.indexOf('=');
      const rawKey = equalIndex === -1 ? arg.slice(2) : arg.slice(2, equalIndex);
      const key = rawKey.replace(/-([a-z])/g, (_, letter) => letter.toUpperCase());
      if (Object.prototype.hasOwnProperty.call(parsed, key)) {
        if (equalIndex === -1) {
          parsed[key] = args[index + 1] || '';
          index += 1;
        } else {
          parsed[key] = arg.slice(equalIndex + 1);
        }
      }
    } else if (parsed.type === 'status' && isNoticeType(arg)) {
      parsed.type = arg;
    }
  }

  return parsed;
}

function normalizeType(type) {
  const value = String(type || '').toLowerCase();
  return isNoticeType(value) ? value : 'status';
}

function isNoticeType(type) {
  const allowed = new Set(['start', 'status', 'approval', 'checkpoint', 'blocked', 'done']);
  return allowed.has(String(type || '').toLowerCase());
}

function defaultTitle(type) {
  return {
    start: '작업 시작',
    status: '작업 상태 갱신',
    approval: '권한 승인 대기 가능',
    checkpoint: '중간 체크포인트 기록',
    blocked: '작업 중단',
    done: '작업 완료',
  }[type] || '작업 상태 갱신';
}

function fit(text, maxLength) {
  const value = String(text || '').replace(/\s+/g, ' ').trim();
  if (value.length <= maxLength) return value;
  return `${value.slice(0, maxLength - 3).trim()}...`;
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
