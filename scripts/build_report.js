#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');

const input = process.argv[2] || 'docs/reports/2026-06-01.md';
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

function escapeHtml(text) {
  return String(text)
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')
    .replace(/"/g, '&quot;');
}
