#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');

const REPORTS_ROOT = path.resolve('docs', 'orchestration', 'reports');
const PROJECT_BRIEF_PATH = path.resolve('docs', 'orchestration', 'state', 'PROJECT_BRIEF.md');
const DEFAULT_ENDPOINT = 'http://127.0.0.1:4317/api/orchestration/discord-report';

const options = parseArgs(process.argv.slice(2));

main().catch((error) => {
  console.error(error.message);
  process.exit(1);
});

async function main() {
  const endpoint = options.url || process.env.PROJECT_ORCHESTRATOR_DISCORD_URL || DEFAULT_ENDPOINT;
  const projectId = options.projectId || projectIdFromBrief();
  const reportMarkdownPath = path.resolve(options.input || latestMarkdownReport());
  const reportPath = resolveReportPath(reportMarkdownPath, options);
  const sourceFiles = [path.relative(process.cwd(), reportMarkdownPath)];
  const summaryKo = options.summary || summaryFromMarkdown(reportMarkdownPath, options);

  const payload = {
    projectId,
    reportPath,
    attachHtml: options.attachHtml,
    dryRun: options.dryRun,
    summaryKo,
    sourceFiles,
    requestedBy: 'codex',
  };

  if (options.printPayload) {
    console.log(JSON.stringify(payload, null, 2));
    return;
  }

  const response = await fetch(endpoint, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json; charset=utf-8',
    },
    body: JSON.stringify(payload),
  });

  const text = await response.text();
  if (!response.ok) {
    throw new Error(`Project Orchestrator Discord intake failed: ${response.status} ${response.statusText}\n${text}`);
  }

  console.log(`Submitted Discord report to Project Orchestrator: ${endpoint}`);
  console.log(`projectId: ${projectId}`);
  console.log(`reportPath: ${reportPath}`);
  if (text.trim()) console.log(text.trim());
}

function parseArgs(args) {
  const parsed = {
    attachHtml: true,
    dryRun: false,
    input: '',
    latestSection: false,
    printPayload: false,
    projectId: '',
    section: '',
    summary: '',
    url: '',
  };

  for (let index = 0; index < args.length; index += 1) {
    const arg = args[index];
    if (arg === '--dry-run') {
      parsed.dryRun = true;
    } else if (arg === '--no-attach-html') {
      parsed.attachHtml = false;
    } else if (arg === '--latest-section') {
      parsed.latestSection = true;
    } else if (arg === '--print-payload') {
      parsed.printPayload = true;
    } else if (arg === '--project-id') {
      parsed.projectId = args[index + 1] || '';
      index += 1;
    } else if (arg.startsWith('--project-id=')) {
      parsed.projectId = arg.slice('--project-id='.length);
    } else if (arg === '--section') {
      parsed.section = args[index + 1] || '';
      index += 1;
    } else if (arg.startsWith('--section=')) {
      parsed.section = arg.slice('--section='.length);
    } else if (arg === '--summary') {
      parsed.summary = args[index + 1] || '';
      index += 1;
    } else if (arg.startsWith('--summary=')) {
      parsed.summary = arg.slice('--summary='.length);
    } else if (arg === '--url') {
      parsed.url = args[index + 1] || '';
      index += 1;
    } else if (arg.startsWith('--url=')) {
      parsed.url = arg.slice('--url='.length);
    } else if (!arg.startsWith('--') && !parsed.input) {
      parsed.input = arg;
    }
  }

  return parsed;
}

function latestMarkdownReport() {
  if (!fs.existsSync(REPORTS_ROOT)) {
    throw new Error(`Reports root not found: ${path.relative(process.cwd(), REPORTS_ROOT)}`);
  }

  const reports = fs.readdirSync(REPORTS_ROOT)
    .filter((entry) => /^\d{8}$/.test(entry))
    .filter((entry) => fs.existsSync(path.join(REPORTS_ROOT, entry, 'index.md')))
    .sort();

  if (!reports.length) throw new Error('No orchestration report index.md files found.');
  return path.join(REPORTS_ROOT, reports[reports.length - 1], 'index.md');
}

function resolveReportPath(markdownPath, opts) {
  const day = reportDayFromMarkdownPath(markdownPath);
  if (!day) {
    throw new Error(`Cannot infer report day from: ${path.relative(process.cwd(), markdownPath)}`);
  }

  if (opts.latestSection || opts.section) {
    const unitPath = resolveUnitHtml(markdownPath, opts);
    if (unitPath) return toReportsRelativePath(unitPath);
  }

  return `${day}/index.html`;
}

function resolveUnitHtml(markdownPath, opts) {
  const markdown = fs.readFileSync(markdownPath, 'utf8');
  const section = opts.section
    ? sectionByTitle(markdown, opts.section)
    : latestTopLevelSection(markdown);
  if (!section) return '';

  const day = reportDayFromMarkdownPath(markdownPath);
  const date = compactToDate(day);
  const match = section.title.match(new RegExp(`^${escapeRegExp(date)}-(\\d{2})\\s+-\\s+(.+)$`));
  if (!match) return '';

  const prefix = `${date}-${match[1]}-`;
  const unitDir = path.join(path.dirname(markdownPath), 'units');
  if (!fs.existsSync(unitDir)) return '';

  const htmlFile = fs.readdirSync(unitDir)
    .filter((file) => file.startsWith(prefix) && file.endsWith('.html'))
    .sort()[0];
  return htmlFile ? path.join(unitDir, htmlFile) : '';
}

function summaryFromMarkdown(markdownPath, opts) {
  const markdown = fs.readFileSync(markdownPath, 'utf8');
  const section = opts.latestSection || opts.section
    ? (opts.section ? sectionByTitle(markdown, opts.section) : latestTopLevelSection(markdown))
    : null;
  const source = section?.markdown || markdown;
  const heading = section?.title || firstHeading(markdown) || '개발 보고서';
  const changed = bulletsFromSection(source, '오늘 바뀐 것', 2);
  const tests = bulletsFromSection(source, '테스트 결과와 근거', 1);
  const parts = [
    stripMarkdown(heading),
    ...changed,
    ...tests,
  ].filter(Boolean);
  return parts.join(' / ').slice(0, 360) || '개발 보고서가 갱신되었습니다.';
}

function projectIdFromBrief() {
  if (!fs.existsSync(PROJECT_BRIEF_PATH)) return 'project';
  const brief = fs.readFileSync(PROJECT_BRIEF_PATH, 'utf8');
  const summaryMatch = brief.match(/##\s+One-Line Summary\s+([\s\S]*?)(?:\n##\s+|$)/i);
  const summary = stripMarkdown(summaryMatch?.[1] || '').trim();
  const leadingName = summary.match(/^([A-Z][A-Z0-9_-]{2,})\b/)?.[1];
  return slug(leadingName || firstHeading(brief) || 'project');
}

function toReportsRelativePath(filePath) {
  return path.relative(REPORTS_ROOT, filePath).replace(/\\/g, '/');
}

function reportDayFromMarkdownPath(filePath) {
  const dir = path.basename(path.dirname(filePath));
  return /^\d{8}$/.test(dir) ? dir : '';
}

function firstHeading(markdown) {
  const match = markdown.match(/^#\s+(.+)$/m);
  return match ? match[1].trim() : '';
}

function latestTopLevelSection(markdown) {
  const sections = topLevelSections(markdown).filter((section) => section.title);
  return sections[sections.length - 1] || null;
}

function sectionByTitle(markdown, title) {
  const target = normalize(title);
  return topLevelSections(markdown).find((section) => normalize(section.title) === target) || null;
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

function bulletsFromSection(markdown, headingName, limit) {
  const target = normalize(headingName);
  const lines = markdown.split(/\r?\n/);
  const bullets = [];
  let inTarget = false;
  let targetLevel = 0;

  for (const line of lines) {
    const heading = line.match(/^(#{1,6})\s+(.+)$/);
    if (heading) {
      const level = heading[1].length;
      if (inTarget && level <= targetLevel) break;
      inTarget = normalize(heading[2]).replace(/^\d+\.\s+/, '') === target;
      targetLevel = inTarget ? level : targetLevel;
      continue;
    }

    if (!inTarget) continue;
    const bullet = line.match(/^\s*-\s+(.+)$/);
    if (bullet) bullets.push(stripMarkdown(bullet[1]).trim());
    if (bullets.length >= limit) break;
  }

  return bullets;
}

function stripMarkdown(text) {
  return String(text || '')
    .replace(/`([^`]+)`/g, '$1')
    .replace(/\*\*([^*]+)\*\*/g, '$1')
    .replace(/\[([^\]]+)\]\([^)]+\)/g, '$1');
}

function normalize(text) {
  return stripMarkdown(text).trim().toLowerCase().replace(/\s+/g, ' ');
}

function slug(text) {
  return String(text || '')
    .trim()
    .toLowerCase()
    .replace(/[^a-z0-9가-힣_-]+/g, '-')
    .replace(/^-+|-+$/g, '') || 'project';
}

function escapeRegExp(text) {
  return String(text).replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
}

function compactToDate(value) {
  return `${value.slice(0, 4)}-${value.slice(4, 6)}-${value.slice(6, 8)}`;
}
