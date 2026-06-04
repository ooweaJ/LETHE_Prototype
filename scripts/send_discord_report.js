#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');

const options = parseArgs(process.argv.slice(2));
const dryRun = options.dryRun;
const input = options.input || latestMarkdownReport();
const markdownPath = path.resolve(input);
const dailyHtmlPath = markdownPath.replace(/\.md$/i, '.html');
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

  if (!fs.existsSync(dailyHtmlPath)) {
    throw new Error(`Report HTML not found. Run the report builder first: ${path.relative(process.cwd(), dailyHtmlPath)}`);
  }

  const fullMarkdown = fs.readFileSync(markdownPath, 'utf8');
  const reportScope = selectReportScope(fullMarkdown, options);
  const attachmentHtmlPath = resolveAttachmentHtmlPath(markdownPath, reportScope, options);
  const markdown = reportScope.markdown;
  const html = fs.readFileSync(attachmentHtmlPath);
  const prompt = promptPath ? fs.readFileSync(promptPath) : null;
  const jsonAttachmentName = jsonSummaryFileName(markdownPath, reportScope.title);
  const messagePayload = buildMessagePayload(markdown, markdownPath, attachmentHtmlPath, promptPath, reportScope.title);
  messagePayload.attachments.unshift(jsonAttachmentName);
  const message = formatMessagePayload(messagePayload);
  const jsonAttachment = Buffer.from(JSON.stringify(messagePayload, null, 2), 'utf8');

  if (dryRun) {
    console.log(message);
    console.log(`Attachment: ${jsonAttachmentName} (${jsonAttachment.length} bytes)`);
    console.log(`Attachment: ${path.relative(process.cwd(), attachmentHtmlPath)} (${html.length} bytes)`);
    if (promptPath) {
      console.log(`Attachment: ${path.relative(process.cwd(), promptPath)} (${prompt.length} bytes)`);
    }
    return;
  }

  const webhookUrl = process.env.DISCORD_WEBHOOK_URL;
  if (!webhookUrl) {
    console.log('DISCORD_WEBHOOK_URL is not set. Skipped Discord upload; the HTML report was still generated.');
    console.log(`Report: ${path.relative(process.cwd(), htmlPath)}`);
    return;
  }

  const form = new FormData();
  form.append('payload_json', JSON.stringify({
    content: message,
    allowed_mentions: { parse: [] },
  }));
  form.append('files[0]', new Blob([jsonAttachment], { type: 'application/json' }), jsonAttachmentName);
  form.append('files[1]', new Blob([html], { type: 'text/html' }), path.basename(attachmentHtmlPath));
  if (promptPath) {
    form.append('files[2]', new Blob([prompt], { type: 'text/markdown' }), path.basename(promptPath));
  }

  const response = await fetch(webhookUrl, {
    method: 'POST',
    body: form,
  });

  if (!response.ok) {
    const body = await response.text();
    throw new Error(`Discord upload failed: ${response.status} ${response.statusText}\n${body}`);
  }

  console.log(`Uploaded ${path.relative(process.cwd(), attachmentHtmlPath)} to Discord.`);
}

function buildMessagePayload(markdown, markdownFile, htmlFile, reviewPromptPath, sectionTitle = '') {
  const reportDate = path.basename(markdownFile, '.md');
  const work = bulletsFromSections(markdown, [
    '2. 오늘 바뀐 것',
    '오늘 바뀐 것',
    '오늘 한 일',
    '작업 내용',
    'what changed today',
    'what changed',
    'today work',
  ], 2);
  const status = bulletsFromSections(markdown, [
    '1. 현재 빌드 상태',
    '현재 빌드 상태',
    '현재 상태',
    '완료 상태',
    'current build status',
    'current build',
    'build status',
  ], 2);
  const problems = bulletsFromSections(markdown, [
    '5. 문제 또는 리스크',
    '문제 또는 리스크',
    '문제와 리스크',
    '문제',
    'problems or risks',
    'problems and risks',
    'risks',
  ], 2);
  const handoff = bulletsFromSections(markdown, [
    '6. GPT/Claude 인계 요약',
    'GPT/Claude 인계 요약',
    '기획 검토 요약',
    'gpt handoff summary',
    'planning handoff',
    'gpt 전달',
  ], 2);
  const planningLine = reviewPromptPath
    ? `있음 - ${path.relative(process.cwd(), reviewPromptPath)}`
    : handoff.length > 0
      ? fit(joinBullets(handoff), 220)
      : '없음';
  return {
    title: sectionTitle ? 'LETHE 작업 보고' : 'LETHE 일일 보고서',
    date: reportDate,
    section: sectionTitle || null,
    status: fit(joinBullets(status) || 'HTML 보고서 생성 완료', 180),
    work: fit(joinBullets(work) || '보고서가 갱신되었습니다.', 240),
    risks: fit(joinBullets(problems) || '새로 기록된 문제 없음', 180),
    planning: planningLine,
    attachments: [
      path.basename(htmlFile),
      ...(reviewPromptPath ? [path.basename(reviewPromptPath)] : []),
    ],
  };
}

function formatMessagePayload(payload) {
  const json = JSON.stringify(payload, null, 2);
  if (json.length <= 1800) return `\`\`\`json\n${json}\n\`\`\``;

  payload.status = fit(payload.status, 120);
  payload.work = fit(payload.work, 160);
  payload.risks = fit(payload.risks, 120);
  payload.planning = fit(payload.planning, 140);
  return `\`\`\`json\n${JSON.stringify(payload, null, 2)}\n\`\`\``.slice(0, 1900);
}

function jsonSummaryFileName(markdownFile, sectionTitle = '') {
  const reportDate = path.basename(markdownFile, '.md');
  if (!sectionTitle) return `${reportDate}-discord-summary.json`;
  const unit = unitReportForTitle(markdownFile, sectionTitle);
  if (unit) return path.basename(unit.htmlPath).replace(/\.html$/i, '.json');
  return `${reportDate}-discord-summary.json`;
}

function parseArgs(args) {
  const parsed = {
    dryRun: false,
    input: '',
    prompt: '',
    section: '',
    latestSection: false,
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
    } else if (arg === '--section') {
      parsed.section = args[index + 1] || '';
      index += 1;
    } else if (arg.startsWith('--section=')) {
      parsed.section = arg.slice('--section='.length);
    } else if (arg === '--latest-section') {
      parsed.latestSection = true;
    } else if (!arg.startsWith('--') && !parsed.input) {
      parsed.input = arg;
    }
  }

  return parsed;
}

function selectReportScope(markdown, opts) {
  if (opts.section) {
    const section = sectionByTitle(markdown, opts.section);
    if (!section) {
      throw new Error(`Report section not found: ${opts.section}`);
    }
    return section;
  }

  if (opts.latestSection) {
    const section = latestTopLevelSection(markdown);
    if (!section) {
      throw new Error('No top-level report section found.');
    }
    return section;
  }

  return {
    title: '',
    markdown,
  };
}

function resolveAttachmentHtmlPath(reportPath, reportScope, opts) {
  if (!opts.latestSection && !opts.section) return reportPath.replace(/\.md$/i, '.html');

  const unit = unitReportForTitle(reportPath, reportScope.title);
  if (unit && fs.existsSync(unit.htmlPath)) return unit.htmlPath;

  return reportPath.replace(/\.md$/i, '.html');
}

function unitReportForTitle(reportPath, title) {
  const date = path.basename(reportPath, '.md');
  const match = String(title || '').match(new RegExp(`^${escapeRegExp(date)}-(\\d{2})\\s+-\\s+(.+)$`));
  if (!match) return null;

  const unitDir = path.join(path.dirname(reportPath), 'units', date);
  const prefix = `${date}-${match[1]}-`;
  if (!fs.existsSync(unitDir)) return null;

  const htmlFile = fs.readdirSync(unitDir)
    .filter((file) => file.startsWith(prefix) && file.endsWith('.html'))
    .sort()[0];
  if (!htmlFile) return null;

  const htmlPath = path.join(unitDir, htmlFile);
  return {
    htmlPath,
    markdownPath: htmlPath.replace(/\.html$/i, '.md'),
  };
}

function sectionByTitle(markdown, title) {
  const target = normalize(title);
  return topLevelSections(markdown).find((section) => normalize(section.title) === target) || null;
}

function latestTopLevelSection(markdown) {
  const sections = topLevelSections(markdown).filter((section) => section.title);
  return sections[sections.length - 1] || null;
}

function topLevelSections(markdown) {
  const lines = markdown.split(/\r?\n/);
  const headings = [];
  for (let index = 0; index < lines.length; index += 1) {
    const match = lines[index].match(/^#\s+(.+)$/);
    if (match) headings.push({ index, title: stripMarkdown(match[1]).trim() });
  }

  return headings.map((heading, idx) => {
    const end = headings[idx + 1]?.index ?? lines.length;
    return {
      title: heading.title,
      markdown: lines.slice(heading.index, end).join('\n'),
    };
  });
}

function resolvePromptPath(explicitPrompt, reportPath) {
  if (explicitPrompt) {
    const explicitPath = path.resolve(explicitPrompt);
    return fs.existsSync(explicitPath) ? explicitPath : '';
  }

  const date = path.basename(reportPath, '.md');
  const promptsDir = path.resolve('docs', 'review_prompts');
  if (!fs.existsSync(promptsDir)) return '';

  const prompts = fs.readdirSync(promptsDir)
    .filter((file) => new RegExp(`^${escapeRegExp(date)}(?:-[a-z0-9-]+)?\\.md$`, 'i').test(file))
    .map((file) => ({
      file,
      mtimeMs: fs.statSync(path.join(promptsDir, file)).mtimeMs,
    }))
    .sort((a, b) => a.mtimeMs - b.mtimeMs || a.file.localeCompare(b.file));

  return prompts.length ? path.join(promptsDir, prompts[prompts.length - 1].file) : '';
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
  const target = normalizeSectionHeading(headingName);

  for (let index = 0; index < lines.length; index += 1) {
    const heading = lines[index].match(/^(#{1,6})\s+(.+)$/);
    if (!heading || normalizeSectionHeading(heading[2]) !== target) continue;

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

function normalizeSectionHeading(text) {
  return normalize(text).replace(/^\d+\.\s+/, '');
}

function escapeRegExp(text) {
  return String(text).replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
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

function latestMarkdownReport() {
  const reportsDir = path.resolve('docs', 'reports');
  if (!fs.existsSync(reportsDir)) return 'docs/reports/2026-06-01.md';

  const reports = fs.readdirSync(reportsDir)
    .filter((file) => /^\d{4}-\d{2}-\d{2}\.md$/.test(file))
    .sort();

  if (reports.length === 0) return 'docs/reports/2026-06-01.md';
  return path.join('docs', 'reports', reports[reports.length - 1]);
}
