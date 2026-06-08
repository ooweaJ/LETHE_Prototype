#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');

const REPORTS_ROOT = path.resolve('docs', 'orchestration', 'reports');
const LEGACY_REPORTS_ROOT = path.resolve('docs', 'reports');

const input = process.argv[2] || latestMarkdownReport();
const reportPath = path.resolve(input);

if (!fs.existsSync(reportPath)) {
  console.error(`Report markdown not found: ${input}`);
  process.exit(1);
}

const markdown = fs.readFileSync(reportPath, 'utf8');
const date = reportDate(reportPath);
const headings = markdown
  .split(/\r?\n/)
  .map((line, index) => ({ line, lineNumber: index + 1 }))
  .filter((entry) => /^#\s+/.test(entry.line));

const errors = [];
let previousNumber = 0;
const units = [];

headings.forEach((heading, index) => {
  const text = heading.line.replace(/^#\s+/, '').trim();

  if (index === 0 && text === `LETHE 개발 보고서 - ${date}`) return;

  const match = text.match(new RegExp(`^${escapeRegExp(date)}-(\\d{2})\\s+-\\s+(.+)$`));
  if (!match) {
    errors.push(`${heading.lineNumber}: expected "# ${date}-NN - 제목", got "${heading.line}"`);
    return;
  }

  const title = match[2].trim();
  const titleIssue = enforceTitlePolicy(date) ? reportTitleIssue(title) : '';
  if (titleIssue) {
    errors.push(`${heading.lineNumber}: report unit title is too small/procedural: "${title}" (${titleIssue})`);
  }

  const number = Number(match[1]);
  if (number !== previousNumber + 1) {
    errors.push(`${heading.lineNumber}: expected unit ${String(previousNumber + 1).padStart(2, '0')}, got ${match[1]}`);
  }
  previousNumber = number;
  units.push({
    number: match[1],
    title,
  });
});

verifyUnitFiles(units);

if (errors.length) {
  console.error(`Report unit heading check failed: ${path.relative(process.cwd(), reportPath)}`);
  errors.slice(0, 10).forEach((error) => console.error(`- ${error}`));
  if (errors.length > 10) console.error(`- ... +${errors.length - 10} more`);
  process.exit(1);
}

console.log(`Report unit headings ok: ${path.relative(process.cwd(), reportPath)} (${previousNumber} units)`);

function verifyUnitFiles(unitList) {
  if (!unitList.length) return;

  const unitDir = unitDirectory(reportPath, date);
  if (!fs.existsSync(unitDir)) {
    errors.push(`missing unit report directory: ${path.relative(process.cwd(), unitDir)}`);
    return;
  }

  unitList.forEach((unit) => {
    const prefix = `${date}-${unit.number}-`;
    const files = fs.readdirSync(unitDir).filter((file) => file.startsWith(prefix));
    if (!files.some((file) => file.endsWith('.md'))) {
      errors.push(`missing unit markdown file for ${date}-${unit.number}`);
    }
    if (!files.some((file) => file.endsWith('.html'))) {
      errors.push(`missing unit html file for ${date}-${unit.number}`);
    }
  });

  const latestPath = path.join(unitDir, 'latest.json');
  if (!fs.existsSync(latestPath)) {
    errors.push(`missing unit latest metadata: ${path.relative(process.cwd(), latestPath)}`);
  }
}

function reportDate(filePath) {
  const base = path.basename(filePath);
  const match = base.match(/^(\d{4}-\d{2}-\d{2})\.md$/);
  if (match) return match[1];

  if (base.toLowerCase() === 'index.md') {
    const compact = path.basename(path.dirname(filePath));
    if (/^\d{8}$/.test(compact)) return compactToDate(compact);
  }

  {
    console.error(`Report file must be named YYYY-MM-DD.md: ${path.relative(process.cwd(), filePath)}`);
    process.exit(1);
  }
}

function reportTitleIssue(title) {
  const value = String(title || '').trim();
  const bannedPatterns = [
    { pattern: /feedback-\d+\s*태스크\s*갱신/i, reason: 'feedback update should be folded into the feature/decision report unit' },
    { pattern: /devloop\s+\d+\s+feedback-\d+/i, reason: 'loop feedback is an internal step, not a report unit' },
    { pattern: /자동\s*개발\s*루프\s*\d+차/i, reason: 'loop iteration is a commit/log unit, not a report unit' },
    { pattern: /구현\s*결과$/i, reason: 'implementation result should include verification and feedback before reporting' },
    { pattern: /태스크\s*갱신$/i, reason: 'task update should be folded into the feature/decision report unit' },
    { pattern: /qa\s*재시도\s*\d*회?/i, reason: 'single QA retry is too small for a report unit' },
  ];

  const match = bannedPatterns.find((entry) => entry.pattern.test(value));
  return match ? match.reason : '';
}

function enforceTitlePolicy(reportDay) {
  return reportDay >= '2026-06-03';
}

function latestMarkdownReport() {
  const reportsDir = REPORTS_ROOT;
  if (fs.existsSync(reportsDir)) {
    const reports = fs.readdirSync(reportsDir)
      .filter((entry) => /^\d{8}$/.test(entry))
      .filter((entry) => fs.existsSync(path.join(reportsDir, entry, 'index.md')))
      .sort();

    if (reports.length > 0) {
      return path.join('docs', 'orchestration', 'reports', reports[reports.length - 1], 'index.md');
    }
  }

  if (!fs.existsSync(LEGACY_REPORTS_ROOT)) return 'docs/orchestration/reports/20260601/index.md';

  const reports = fs.readdirSync(LEGACY_REPORTS_ROOT)
    .filter((file) => /^\d{4}-\d{2}-\d{2}\.md$/.test(file))
    .sort();

  if (!reports.length) return 'docs/orchestration/reports/20260601/index.md';
  return path.join('docs', 'reports', reports[reports.length - 1]);
}

function unitDirectory(sourcePath, reportDay) {
  if (path.basename(sourcePath).toLowerCase() === 'index.md' && /^\d{8}$/.test(path.basename(path.dirname(sourcePath)))) {
    return path.join(path.dirname(sourcePath), 'units');
  }

  return path.join(path.dirname(sourcePath), 'units', reportDay);
}

function compactToDate(value) {
  return `${value.slice(0, 4)}-${value.slice(4, 6)}-${value.slice(6, 8)}`;
}

function escapeRegExp(text) {
  return String(text).replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
}
