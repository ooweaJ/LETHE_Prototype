#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');

const REPORTS_ROOT = path.resolve('docs', 'orchestration', 'reports');
const LEGACY_REPORTS_ROOT = path.resolve('docs', 'reports');

const input = process.argv[2] || latestMarkdownReport();
const inputPath = path.resolve(input);
const outputPath = inputPath.replace(/\.md$/i, '.html');

if (!fs.existsSync(inputPath)) {
  console.error(`Report markdown not found: ${input}`);
  process.exit(1);
}

const markdown = fs.readFileSync(inputPath, 'utf8');
const html = renderPage(markdown, path.basename(inputPath));
fs.writeFileSync(outputPath, html, 'utf8');
console.log(`Wrote ${path.relative(process.cwd(), outputPath)}`);
writeUnitReports(markdown, inputPath);
writeReportsIndex();

function writeUnitReports(md, sourcePath) {
  const date = reportDate(sourcePath);
  if (!date) return;

  const unitDir = unitDirectory(sourcePath, date);
  fs.mkdirSync(unitDir, { recursive: true });

  fs.readdirSync(unitDir)
    .filter((file) => file.startsWith(`${date}-`) || file === 'latest.json')
    .forEach((file) => fs.rmSync(path.join(unitDir, file), { force: true }));

  const units = topLevelSections(md)
    .filter((section) => new RegExp(`^${escapeRegExp(date)}-\\d{2}\\s+-\\s+.+$`).test(section.title));

  let latest = null;
  units.forEach((unit) => {
    const match = unit.title.match(new RegExp(`^${escapeRegExp(date)}-(\\d{2})\\s+-\\s+(.+)$`));
    if (!match) return;

    const number = match[1];
    const title = match[2].trim();
    const baseName = `${date}-${number}-${slug(title)}`;
    const markdownPath = path.join(unitDir, `${baseName}.md`);
    const htmlPath = path.join(unitDir, `${baseName}.html`);

    fs.writeFileSync(markdownPath, `${unit.markdown.trim()}\n`, 'utf8');
    fs.writeFileSync(htmlPath, renderPage(unit.markdown, path.basename(markdownPath)), 'utf8');
    latest = {
      title: unit.title,
      number,
      markdownPath: path.relative(process.cwd(), markdownPath),
      htmlPath: path.relative(process.cwd(), htmlPath),
    };
  });

  if (latest) {
    fs.writeFileSync(path.join(unitDir, 'latest.json'), `${JSON.stringify({
      source: path.relative(process.cwd(), sourcePath),
      generatedAt: new Date().toISOString(),
      unitCount: units.length,
      latest,
    }, null, 2)}\n`, 'utf8');
    console.log(`Wrote ${units.length} unit report(s) to ${path.relative(process.cwd(), unitDir)}`);
  }
}

function writeReportsIndex() {
  if (!fs.existsSync(REPORTS_ROOT)) return;

  const days = fs.readdirSync(REPORTS_ROOT)
    .filter((entry) => /^\d{8}$/.test(entry))
    .filter((entry) => fs.existsSync(path.join(REPORTS_ROOT, entry, 'index.html')))
    .sort()
    .reverse();

  const items = days.map((day) => {
    const date = compactToDate(day);
    const indexPath = `${day}/index.html`;
    const latest = latestUnitForDay(day);
    const latestLine = latest
      ? `<small>최신 단위: <a href="${escapeHtml(path.posix.join(day, 'units', path.basename(latest.htmlPath)))}">${escapeHtml(latest.title)}</a></small>`
      : '<small>단위 리포트 없음</small>';
    return `<li>
      <a href="${indexPath}">${escapeHtml(date)} 개발 리포트</a>
      ${latestLine}
    </li>`;
  }).join('\n');

  const html = `<!doctype html>
<html lang="ko">
  <head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>LETHE 개발 리포트</title>
    <style>
      :root {
        --bg: #f6f7f3;
        --panel: #ffffff;
        --text: #17201d;
        --muted: #64706b;
        --line: #dce2da;
        --green: #1f6b4d;
        font-family: Inter, ui-sans-serif, system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif;
        background: var(--bg);
        color: var(--text);
      }
      * { box-sizing: border-box; }
      body { margin: 0; }
      main { width: min(980px, 100%); margin: 0 auto; padding: 30px 22px 42px; }
      a { color: var(--green); font-weight: 900; text-decoration: none; }
      a:hover { text-decoration: underline; }
      .eyebrow { margin: 0 0 8px; color: var(--green); font-size: 13px; font-weight: 900; }
      h1 { margin: 0 0 10px; font-size: clamp(30px, 5vw, 46px); line-height: 1.08; }
      p { margin: 0 0 16px; color: var(--muted); line-height: 1.65; }
      ul { display: grid; gap: 10px; margin: 0; padding: 0; list-style: none; }
      li { padding: 16px; border: 1px solid var(--line); border-radius: 8px; background: var(--panel); }
      small { display: block; margin-top: 6px; color: var(--muted); line-height: 1.45; }
      footer { margin-top: 18px; color: var(--muted); font-size: 13px; }
    </style>
  </head>
  <body>
    <main>
      <p class="eyebrow">LETHE 개발 문서</p>
      <h1>날짜별 리포트</h1>
      <p>사람이 읽는 개발 리포트입니다. 날짜별 index가 그날의 작업을 모아 보여주고, 세부 단위는 각 날짜의 units 폴더에 보관됩니다.</p>
      <ul>
        ${items || '<li>아직 생성된 리포트가 없습니다.</li>'}
      </ul>
      <footer>기준: ${escapeHtml(new Date().toISOString().slice(0, 10))}</footer>
    </main>
  </body>
</html>`;

  fs.writeFileSync(path.join(REPORTS_ROOT, 'index.html'), html, 'utf8');
  console.log(`Wrote ${path.relative(process.cwd(), path.join(REPORTS_ROOT, 'index.html'))}`);
}

function latestUnitForDay(day) {
  const latestPath = path.join(REPORTS_ROOT, day, 'units', 'latest.json');
  if (!fs.existsSync(latestPath)) return null;
  try {
    const data = JSON.parse(fs.readFileSync(latestPath, 'utf8'));
    return data.latest || null;
  } catch {
    return null;
  }
}

function renderPage(md, fileName) {
  const title = firstHeading(md) || fileName;
  const body = renderMarkdown(md);
  return `<!doctype html>
<html lang="ko">
<head>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  <title>${escapeHtml(title)}</title>
  <style>
    :root {
      color-scheme: light;
      --bg: #f5f7fb;
      --paper: #ffffff;
      --ink: #172033;
      --muted: #667085;
      --line: #d8dee9;
      --accent: #2563eb;
      --good: #087443;
      --warn: #a15c00;
      --bad: #b42318;
      --code: #111827;
    }
    * { box-sizing: border-box; }
    body {
      margin: 0;
      background: var(--bg);
      color: var(--ink);
      font-family: ui-sans-serif, system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif;
      line-height: 1.62;
    }
    header {
      background: #101828;
      color: #fff;
      padding: 28px 20px;
      border-bottom: 4px solid var(--accent);
    }
    main {
      width: min(1080px, calc(100% - 32px));
      margin: 24px auto 48px;
      background: var(--paper);
      border: 1px solid var(--line);
      border-radius: 8px;
      padding: 28px;
      box-shadow: 0 16px 40px rgba(16, 24, 40, 0.08);
    }
    .header-inner {
      width: min(1080px, calc(100% - 32px));
      margin: 0 auto;
    }
    h1, h2, h3, h4 { line-height: 1.25; margin: 1.5em 0 0.55em; }
    h1 { margin-top: 0; font-size: 30px; }
    h2 {
      padding-top: 18px;
      border-top: 1px solid var(--line);
      font-size: 22px;
    }
    h3 { font-size: 17px; color: #243047; }
    p, ul, ol, table, pre { margin: 0 0 16px; }
    a { color: var(--accent); }
    ul, ol { padding-left: 24px; }
    code {
      background: #eef2f7;
      color: #111827;
      padding: 2px 5px;
      border-radius: 4px;
      font-size: 0.92em;
    }
    pre {
      background: var(--code);
      color: #e5e7eb;
      padding: 16px;
      border-radius: 8px;
      overflow: auto;
    }
    pre code {
      background: transparent;
      color: inherit;
      padding: 0;
    }
    blockquote {
      margin: 0 0 18px;
      padding: 12px 16px;
      border-left: 4px solid var(--accent);
      background: #eef4ff;
      color: #243047;
    }
    table {
      width: 100%;
      border-collapse: collapse;
      font-size: 14px;
    }
    th, td {
      border: 1px solid var(--line);
      padding: 9px 10px;
      vertical-align: top;
    }
    th {
      background: #f2f4f7;
      text-align: left;
    }
    .meta {
      color: #cbd5e1;
      margin: 8px 0 0;
    }
    .callout {
      border: 1px solid #bfd7ff;
      background: #eef4ff;
      border-radius: 8px;
      padding: 14px 16px;
      margin: 16px 0;
    }
    @media (max-width: 720px) {
      main { padding: 18px; }
      table { display: block; overflow-x: auto; }
    }
  </style>
</head>
<body>
  <header>
    <div class="header-inner">
      <h1>${escapeHtml(title)}</h1>
      <p class="meta">Generated from ${escapeHtml(fileName)}</p>
    </div>
  </header>
  <main>
    ${body}
  </main>
</body>
</html>`;
}

function firstHeading(md) {
  const match = md.match(/^#\s+(.+)$/m);
  return match ? match[1].trim() : '';
}

function topLevelSections(md) {
  const lines = md.split(/\r?\n/);
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

function renderMarkdown(md) {
  const lines = md.split(/\r?\n/);
  const out = [];
  let inCode = false;
  let code = [];
  let inList = false;
  let inTable = false;

  for (const rawLine of lines) {
    const line = rawLine;

    if (line.startsWith('```')) {
      if (!inCode) {
        closeList();
        closeTable();
        inCode = true;
        code = [];
      } else {
        out.push(`<pre><code>${escapeHtml(code.join('\n'))}</code></pre>`);
        inCode = false;
      }
      continue;
    }

    if (inCode) {
      code.push(line);
      continue;
    }

    if (isTableLine(line)) {
      closeList();
      if (!inTable) {
        inTable = true;
        out.push('<table>');
      }
      if (/^\s*\|?\s*:?-{3,}:?\s*\|/.test(line)) continue;
      const cells = line.trim().replace(/^\|/, '').replace(/\|$/, '').split('|').map((cell) => cell.trim());
      const tag = out[out.length - 1] === '<table>' ? 'th' : 'td';
      out.push(`<tr>${cells.map((cell) => `<${tag}>${inline(cell)}</${tag}>`).join('')}</tr>`);
      continue;
    }

    closeTable();

    if (!line.trim()) {
      closeList();
      continue;
    }

    const heading = line.match(/^(#{1,4})\s+(.+)$/);
    if (heading) {
      closeList();
      const level = heading[1].length;
      out.push(`<h${level}>${inline(heading[2])}</h${level}>`);
      continue;
    }

    const bullet = line.match(/^\s*-\s+(.+)$/);
    if (bullet) {
      if (!inList) {
        inList = true;
        out.push('<ul>');
      }
      out.push(`<li>${inline(bullet[1])}</li>`);
      continue;
    }

    const quote = line.match(/^>\s?(.+)$/);
    if (quote) {
      closeList();
      out.push(`<blockquote>${inline(quote[1])}</blockquote>`);
      continue;
    }

    closeList();
    out.push(`<p>${inline(line)}</p>`);
  }

  closeList();
  closeTable();
  return out.join('\n');

  function closeList() {
    if (inList) {
      out.push('</ul>');
      inList = false;
    }
  }

  function closeTable() {
    if (inTable) {
      out.push('</table>');
      inTable = false;
    }
  }
}

function isTableLine(line) {
  return line.includes('|') && line.trim().startsWith('|');
}

function inline(text) {
  return escapeHtml(text)
    .replace(/`([^`]+)`/g, '<code>$1</code>')
    .replace(/\*\*([^*]+)\*\*/g, '<strong>$1</strong>')
    .replace(/\[([^\]]+)\]\(([^)]+)\)/g, '<a href="$2">$1</a>');
}

function stripMarkdown(text) {
  return String(text || '')
    .replace(/`([^`]+)`/g, '$1')
    .replace(/\*\*([^*]+)\*\*/g, '$1')
    .replace(/\[([^\]]+)\]\([^)]+\)/g, '$1');
}

function slug(text) {
  return String(text || '')
    .trim()
    .toLowerCase()
    .replace(/[`"'“”‘’]/g, '')
    .replace(/[^a-z0-9가-힣]+/g, '-')
    .replace(/^-+|-+$/g, '')
    .slice(0, 80) || 'unit';
}

function escapeRegExp(text) {
  return String(text).replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
}

function escapeHtml(text) {
  return String(text)
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')
    .replace(/"/g, '&quot;');
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

  if (reports.length === 0) return 'docs/orchestration/reports/20260601/index.md';
  return path.join('docs', 'reports', reports[reports.length - 1]);
}

function reportDate(filePath) {
  const base = path.basename(filePath);
  const direct = base.match(/^(\d{4}-\d{2}-\d{2})\.md$/);
  if (direct) return direct[1];

  if (base.toLowerCase() === 'index.md') {
    const compact = path.basename(path.dirname(filePath));
    if (/^\d{8}$/.test(compact)) return compactToDate(compact);
  }

  return '';
}

function unitDirectory(sourcePath, date) {
  if (path.basename(sourcePath).toLowerCase() === 'index.md' && /^\d{8}$/.test(path.basename(path.dirname(sourcePath)))) {
    return path.join(path.dirname(sourcePath), 'units');
  }

  return path.join(path.dirname(sourcePath), 'units', date);
}

function compactToDate(value) {
  return `${value.slice(0, 4)}-${value.slice(4, 6)}-${value.slice(6, 8)}`;
}
