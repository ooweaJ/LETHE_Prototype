#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');

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

  const unitDir = path.join(path.dirname(reportPath), 'units', date);
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
  const match = path.basename(filePath).match(/^(\d{4}-\d{2}-\d{2})\.md$/);
  if (!match) {
    console.error(`Report file must be named YYYY-MM-DD.md: ${path.relative(process.cwd(), filePath)}`);
    process.exit(1);
  }
  return match[1];
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
  const reportsDir = path.resolve('docs', 'reports');
  if (!fs.existsSync(reportsDir)) return 'docs/reports/2026-06-01.md';

  const reports = fs.readdirSync(reportsDir)
    .filter((file) => /^\d{4}-\d{2}-\d{2}\.md$/.test(file))
    .sort();

  if (!reports.length) return 'docs/reports/2026-06-01.md';
  return path.join('docs', 'reports', reports[reports.length - 1]);
}

function escapeRegExp(text) {
  return String(text).replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
}
