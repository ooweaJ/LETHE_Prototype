#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');

const REPORTS_ROOT = path.resolve('docs', 'orchestration', 'reports');
const LEGACY_REPORTS_ROOT = path.resolve('docs', 'reports');
const PROJECT_BRIEF_PATH = path.resolve('docs', 'orchestration', 'state', 'PROJECT_BRIEF.md');
const buildAll = process.argv.includes('--all');

if (buildAll) {
  buildAllReports();
  process.exit(0);
}

const input = process.argv.find((arg, index) => index > 1 && !arg.startsWith('--')) || latestMarkdownReport();
const inputPath = path.resolve(input);
const outputPath = inputPath.replace(/\.md$/i, '.html');

if (!fs.existsSync(inputPath)) {
  console.error(`Report markdown not found: ${input}`);
  process.exit(1);
}

const markdown = fs.readFileSync(inputPath, 'utf8');
const html = isDateReportIndex(inputPath)
  ? renderDateJournalPage(markdown, inputPath)
  : renderPage(markdown, path.basename(inputPath));
fs.writeFileSync(outputPath, html, 'utf8');
console.log(`Wrote ${path.relative(process.cwd(), outputPath)}`);
writeUnitReports(markdown, inputPath);
writeReportsIndex();

function buildAllReports() {
  if (!fs.existsSync(REPORTS_ROOT)) {
    console.error(`Reports root not found: ${path.relative(process.cwd(), REPORTS_ROOT)}`);
    process.exit(1);
  }

  const reports = fs.readdirSync(REPORTS_ROOT)
    .filter((entry) => /^\d{8}$/.test(entry))
    .map((entry) => path.join(REPORTS_ROOT, entry, 'index.md'))
    .filter((filePath) => fs.existsSync(filePath))
    .sort();

  if (!reports.length) {
    console.error('No date report markdown files found.');
    process.exit(1);
  }

  reports.forEach((reportPath) => {
    const md = fs.readFileSync(reportPath, 'utf8');
    const outPath = reportPath.replace(/\.md$/i, '.html');
    const page = renderDateJournalPage(md, reportPath);
    fs.writeFileSync(outPath, page, 'utf8');
    console.log(`Wrote ${path.relative(process.cwd(), outPath)}`);
    writeUnitReports(md, reportPath);
  });

  writeReportsIndex();
}

function writeUnitReports(md, sourcePath) {
  const date = reportDate(sourcePath);
  if (!date) return;

  const unitDir = unitDirectory(sourcePath, date);
  fs.mkdirSync(unitDir, { recursive: true });

  fs.readdirSync(unitDir)
    .filter((file) => (file.startsWith(`${date}-`) && (file.endsWith('.md') || file.endsWith('.html'))) || file === 'latest.json')
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
    fs.writeFileSync(htmlPath, renderPage(unit.markdown, path.basename(markdownPath), {
      backHref: apiReportHref(date.replace(/-/g, '')),
      backLabel: '날짜 리포트로 돌아가기',
    }), 'utf8');
    latest = {
      title: localizeFullReportTitle(unit.title),
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
  const archiveDate = days.length ? compactToDate(days[0]) : new Date().toISOString().slice(0, 10);
  const name = projectName();

  const items = days.map((day) => {
    const date = compactToDate(day);
    const indexPath = apiReportHref(day);
    const dayReport = dailyReportInfo(day, date);
    const latest = latestUnitForDay(day);
    const latestLine = latest
      ? `<small class="unit">최신 작업 단위: <a href="${escapeHtml(apiUnitHref(day, path.basename(latest.htmlPath)))}">${escapeHtml(latest.title)}</a></small>`
      : '<small class="unit">작업 단위 리포트 없음</small>';
    return `<li>
      <article>
        <time datetime="${escapeHtml(date)}">${escapeHtml(date)}</time>
        <h2><a href="${indexPath}">${escapeHtml(dayReport.title)}</a></h2>
        <p>${escapeHtml(dayReport.summary)}</p>
        ${latestLine}
      </article>
    </li>`;
  }).join('\n');

  const html = `<!doctype html>
<html lang="ko">
  <head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>${escapeHtml(name)} 개발 리포트</title>
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
      ul { display: grid; gap: 12px; margin: 0; padding: 0; list-style: none; }
      li { padding: 0; border: 1px solid var(--line); border-radius: 8px; background: var(--panel); }
      article { padding: 16px; }
      time { display: block; margin-bottom: 7px; color: var(--muted); font-size: 13px; font-weight: 900; }
      h2 { margin: 0 0 8px; font-size: 19px; line-height: 1.3; }
      small { display: block; margin-top: 8px; color: var(--muted); line-height: 1.45; }
      .unit a { font-weight: 800; }
      footer { margin-top: 18px; color: var(--muted); font-size: 13px; }
    </style>
  </head>
  <body>
    <main>
      <p class="eyebrow">${escapeHtml(name)} 개발 문서</p>
      <h1>날짜별 리포트</h1>
      <p>사람이 읽는 개발 리포트 아카이브입니다. 날짜별 페이지에서 제목 카드로 작업 단위를 고르고, 각 카드에서 해당 내용만 확인합니다.</p>
      <ul>
        ${items || '<li><article>아직 생성된 리포트가 없습니다.</article></li>'}
      </ul>
      <footer>기준: ${escapeHtml(archiveDate)}</footer>
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

function renderDateJournalPage(md, sourcePath) {
  const date = reportDate(sourcePath);
  const day = date.replace(/-/g, '');
  const heading = firstHeading(md);
  const title = /^\d{4}-\d{2}-\d{2}-\d{2}\s+-\s+/.test(heading)
    ? `${date} 개발 리포트`
    : localizeFullReportTitle(heading || `${date} 개발 리포트`);
  const intro = firstParagraph(md) || '이 날짜의 개발 진행, 검증 결과, 결정, 다음 작업을 모은 리포트입니다.';
  const units = topLevelSections(md)
    .filter((section) => new RegExp(`^${escapeRegExp(date)}-(\\d{2})\\s+-\\s+.+$`).test(section.title))
    .map((section) => {
      const match = section.title.match(new RegExp(`^${escapeRegExp(date)}-(\\d{2})\\s+-\\s+(.+)$`));
      const number = match[1];
      const unitTitle = match[2].trim();
      return {
        number,
        id: `unit-${date}-${number}`,
        shortTitle: localizeReportTitle(unitTitle),
        summary: localizeUnitSummary(unitTitle, firstParagraph(section.markdown) || '작업 내용을 확인합니다.'),
        body: renderMarkdown(section.markdown),
      };
    });

  const cards = units.map((unit) => `<li>
        <article class="unit-card">
          <span class="number">${escapeHtml(`${date}-${unit.number}`)}</span>
          <h2><a href="#${escapeHtml(unit.id)}" data-unit-link="${escapeHtml(unit.id)}">${escapeHtml(unit.shortTitle)}</a></h2>
          <p>${escapeHtml(unit.summary)}</p>
        </article>
      </li>`).join('\n');
  const details = units.map((unit) => `<article class="unit-detail" id="${escapeHtml(unit.id)}" hidden>
      <p class="back-inline"><a href="#" data-panel-close>패널 닫기</a></p>
      ${unit.body}
    </article>`).join('\n');

  return `<!doctype html>
<html lang="ko">
<head>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  <title>${escapeHtml(title)}</title>
  <style>
    :root {
      color-scheme: light;
      --bg: #f6f7f3;
      --paper: #ffffff;
      --ink: #17201d;
      --muted: #64706b;
      --line: #dce2da;
      --accent: #1f6b4d;
      font-family: ui-sans-serif, system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif;
      background: var(--bg);
      color: var(--ink);
    }
    * { box-sizing: border-box; }
    body { margin: 0; background: var(--bg); }
    body.panel-open { overflow: hidden; }
    main { width: min(980px, 100%); margin: 0 auto; padding: 30px 22px 44px; }
    nav { margin-bottom: 18px; font-size: 14px; }
    a { color: var(--accent); font-weight: 900; text-decoration: none; }
    a:hover { text-decoration: underline; }
    .eyebrow { margin: 0 0 8px; color: var(--accent); font-size: 13px; font-weight: 900; }
    h1 { margin: 0 0 10px; font-size: clamp(30px, 5vw, 46px); line-height: 1.08; }
    .intro { margin: 0 0 18px; color: var(--muted); line-height: 1.65; }
    ul { display: grid; gap: 12px; margin: 0; padding: 0; list-style: none; }
    li { margin: 0; }
    .unit-card {
      min-height: 136px;
      padding: 17px;
      border: 1px solid var(--line);
      border-radius: 8px;
      background: var(--paper);
      transition: border-color 160ms ease, box-shadow 160ms ease, transform 160ms ease;
    }
    .unit-card:focus-within,
    .unit-card:hover,
    .unit-card.is-active {
      border-color: rgba(31, 107, 77, 0.55);
      box-shadow: 0 14px 30px rgba(23, 32, 29, 0.08);
      transform: translateY(-1px);
    }
    .unit-panel[hidden] { display: none; }
    .unit-panel {
      position: fixed;
      inset: 0;
      z-index: 30;
      display: flex;
      justify-content: flex-end;
      background: rgba(23, 32, 29, 0.22);
    }
    .unit-panel__scrim {
      position: absolute;
      inset: 0;
      cursor: pointer;
    }
    .unit-panel__shell {
      position: relative;
      width: min(760px, calc(100% - 28px));
      height: calc(100% - 28px);
      margin: 14px;
      overflow: auto;
      border: 1px solid var(--line);
      border-radius: 8px;
      background: var(--paper);
      box-shadow: 0 24px 70px rgba(23, 32, 29, 0.22);
    }
    .unit-panel__bar {
      position: sticky;
      top: 0;
      z-index: 2;
      display: flex;
      align-items: center;
      justify-content: space-between;
      gap: 14px;
      padding: 14px 18px;
      border-bottom: 1px solid var(--line);
      background: rgba(255, 255, 255, 0.96);
      backdrop-filter: blur(10px);
    }
    .unit-panel__title {
      margin: 0;
      color: var(--muted);
      font-size: 13px;
      font-weight: 900;
    }
    .unit-panel__close {
      min-width: 38px;
      height: 38px;
      border: 1px solid var(--line);
      border-radius: 8px;
      background: var(--bg);
      color: var(--ink);
      cursor: pointer;
      font-size: 22px;
      font-weight: 900;
      line-height: 1;
    }
    .unit-panel__close:hover {
      border-color: rgba(31, 107, 77, 0.55);
      color: var(--accent);
    }
    .unit-detail {
      padding: 22px;
    }
    .unit-detail h1 { font-size: 28px; }
    .unit-detail h2 {
      margin-top: 24px;
      padding-top: 16px;
      border-top: 1px solid var(--line);
      font-size: 21px;
    }
    .unit-detail ul { display: block; padding-left: 22px; list-style: disc; }
    .unit-detail li { margin-bottom: 6px; }
    .unit-detail code {
      padding: 2px 5px;
      border-radius: 4px;
      background: #eef2f7;
    }
    .back-inline { margin-bottom: 16px; }
    .number { display: block; margin-bottom: 8px; color: var(--muted); font-size: 13px; font-weight: 900; }
    h2 { margin: 0 0 8px; font-size: 20px; line-height: 1.3; }
    p { margin: 0; color: var(--muted); line-height: 1.6; }
    footer { margin-top: 20px; color: var(--muted); font-size: 13px; }
    @media (max-width: 720px) {
      main { padding: 24px 16px 36px; }
      .unit-panel {
        align-items: flex-end;
      }
      .unit-panel__shell {
        width: 100%;
        height: min(86vh, calc(100% - 18px));
        margin: 9px;
      }
      .unit-detail { padding: 18px; }
      .unit-detail h1 { font-size: 24px; }
    }
  </style>
</head>
<body>
  <main>
    <nav><a href="${escapeHtml(apiReportsIndexHref())}">전체 리포트 목록</a></nav>
    <p class="eyebrow">${escapeHtml(day)} 개발 기록</p>
    <h1>${escapeHtml(title)}</h1>
    <p class="intro">${escapeHtml(intro)}</p>
    <ul id="unit-list">
      ${cards || '<li><article class="unit-card"><p>작업 단위 글이 아직 없습니다.</p></article></li>'}
    </ul>
    <section id="unit-detail-root" class="unit-panel" hidden aria-label="작업 단위 상세 패널">
      <div class="unit-panel__scrim" data-panel-close></div>
      <div class="unit-panel__shell" role="dialog" aria-modal="true" aria-label="작업 단위 상세">
        <div class="unit-panel__bar">
          <p class="unit-panel__title">작업 단위 상세</p>
          <button class="unit-panel__close" type="button" data-panel-close aria-label="패널 닫기">&times;</button>
        </div>
        ${details}
      </div>
    </section>
    <footer>기준: ${escapeHtml(date)}</footer>
  </main>
  <script>
    (() => {
      const panel = document.getElementById('unit-detail-root');
      const panelShell = panel?.querySelector('.unit-panel__shell');
      const closeButton = panel?.querySelector('.unit-panel__close');
      const details = Array.from(document.querySelectorAll('.unit-detail'));
      const links = Array.from(document.querySelectorAll('[data-unit-link]'));
      const setActiveCard = (id) => {
        links.forEach((link) => {
          const isActive = link.getAttribute('data-unit-link') === id;
          link.closest('.unit-card')?.classList.toggle('is-active', isActive);
        });
      };
      const closePanel = () => {
        if (!panel) return;
        panel.hidden = true;
        document.body.classList.remove('panel-open');
        details.forEach((detail) => { detail.hidden = true; });
        setActiveCard('');
        if (location.hash) history.replaceState(null, '', location.pathname + location.search);
      };
      const showUnit = (id) => {
        const target = document.getElementById(id);
        if (!target) return;
        if (!panel) return;
        panel.hidden = false;
        document.body.classList.add('panel-open');
        details.forEach((detail) => { detail.hidden = detail !== target; });
        setActiveCard(id);
        if (panelShell) panelShell.scrollTop = 0;
        if (closeButton) closeButton.focus({ preventScroll: true });
      };
      document.addEventListener('click', (event) => {
        const unitLink = event.target.closest('[data-unit-link]');
        if (unitLink) {
          event.preventDefault();
          const id = unitLink.getAttribute('data-unit-link');
          history.pushState(null, '', '#' + id);
          showUnit(id);
          return;
        }
        if (event.target.closest('[data-panel-close]')) {
          event.preventDefault();
          closePanel();
        }
      });
      document.addEventListener('keydown', (event) => {
        if (event.key === 'Escape' && panel && !panel.hidden) closePanel();
      });
      window.addEventListener('popstate', () => {
        const id = location.hash.replace(/^#/, '');
        if (id) showUnit(id);
        else closePanel();
      });
      const initial = location.hash.replace(/^#/, '');
      if (initial) showUnit(initial);
    })();
  </script>
</body>
</html>`;
}

function dailyReportInfo(day, date) {
  const markdownPath = path.join(REPORTS_ROOT, day, 'index.md');
  const fallback = {
    title: `${date} 개발 리포트`,
    summary: '이 날짜의 개발 진행, 검증 결과, 결정, 다음 작업을 모은 리포트입니다.',
  };

  if (!fs.existsSync(markdownPath)) return fallback;

  const md = fs.readFileSync(markdownPath, 'utf8');
  const heading = firstHeading(md);
  const title = /^\d{4}-\d{2}-\d{2}-\d{2}\s+-\s+/.test(heading)
    ? fallback.title
    : localizeFullReportTitle(heading || fallback.title);
  const summary = firstParagraph(md) || fallback.summary;

  return {
    title,
    summary,
  };
}

function renderPage(md, fileName, options = {}) {
  const title = localizeFullReportTitle(firstHeading(md) || fileName);
  const body = renderMarkdown(md);
  const backLink = options.backHref
    ? `<p class="back-link"><a href="${escapeHtml(options.backHref)}">${escapeHtml(options.backLabel || '돌아가기')}</a></p>`
    : '';
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
    .back-link {
      margin: 0 0 18px;
      font-size: 14px;
    }
    .back-link a {
      color: var(--accent);
      font-weight: 800;
      text-decoration: none;
    }
    .back-link a:hover { text-decoration: underline; }
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
    ${backLink}
    ${body}
  </main>
</body>
</html>`;
}

function firstHeading(md) {
  const match = md.match(/^#\s+(.+)$/m);
  return match ? match[1].trim() : '';
}

function localizeFullReportTitle(text) {
  const value = String(text || '').trim();
  const match = value.match(/^(\d{4}-\d{2}-\d{2}-\d{2})\s+-\s+(.+)$/);
  if (!match) return localizeReportTitle(value);
  return `${match[1]} - ${localizeReportTitle(match[2])}`;
}

function localizeReportTitle(text) {
  const key = normalize(text);
  const translations = new Map([
    ['v0.12 human playtest package', 'v0.12 인간 플레이테스트 패키지'],
    ['balance loop gate fix', '밸런스 루프 게이트 수정'],
    ['browser first boss ttk terminal', '브라우저 첫 보스 TTK 종료 조건 수정'],
    ['post-boss balance baseline', '보스 이후 밸런스 기준선'],
    ['deficit trial survival tuning', '결손 시험 생존 조정'],
    ['deficit trial review follow-up', '결손 시험 리뷰 후속 조정'],
    ['first boss ttk boss-only harness', '첫 보스 TTK 보스 전용 하네스'],
  ]);
  return translations.get(key) || text;
}

function localizeUnitSummary(title, fallback) {
  const key = normalize(title);
  const summaries = new Map([
    ['v0.12 human playtest package', '자동 밸런스 기준선을 사람 테스트용 패키지와 관찰 시트로 연결한 작업입니다.'],
    ['balance loop gate fix', '밸런스 루프를 실제 v0.12 기준선에 맞추고 death gate와 실행 인자 문제를 정리한 작업입니다.'],
    ['browser first boss ttk terminal', '첫 보스 TTK 브라우저 측정이 보스 처치 이후에도 끝나지 않던 문제를 종료 조건으로 분리해 해결했습니다.'],
    ['post-boss balance baseline', '보스 이후 흐름과 전체 런 기준선을 자동 QA로 다시 세운 작업입니다.'],
    ['deficit trial survival tuning', '결손 시험 구간의 사망률을 낮추고 생존/회복 감각을 조정한 작업입니다.'],
    ['deficit trial review follow-up', '결손 시험 리뷰 결과를 반영해 과도하게 쉬운 기준선을 다시 중앙으로 맞춘 작업입니다.'],
    ['first boss ttk boss-only harness', '브라우저/CDP 불안정을 우회하기 위해 첫 보스 TTK를 보스 전용 하네스로 측정한 작업입니다.'],
  ]);
  return summaries.get(key) || fallback;
}

function firstParagraph(md) {
  const lines = String(md || '').split(/\r?\n/);
  let seenHeading = false;
  for (const line of lines) {
    const value = line.trim();
    if (!value) continue;
    if (value.startsWith('# ')) {
      seenHeading = true;
      continue;
    }
    if (!seenHeading) continue;
    if (value.startsWith('#') || value.startsWith('- ') || value.startsWith('```')) continue;
    if (value.endsWith(':')) continue;
    return stripMarkdown(value).slice(0, 180);
  }
  return '';
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
      out.push(`<h${level}>${inline(localizeHeading(heading[2]))}</h${level}>`);
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

function normalize(text) {
  return stripMarkdown(text).trim().toLowerCase().replace(/\s+/g, ' ');
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

function localizeHeading(text) {
  const value = stripMarkdown(text).trim();
  if (/^\d{4}-\d{2}-\d{2}-\d{2}\s+-\s+/.test(value)) {
    return localizeFullReportTitle(value);
  }
  const match = value.match(/^(\d+)\.\s+(.+)$/);
  const prefix = match ? `${match[1]}. ` : '';
  const key = normalize(match ? match[2] : value);
  const translations = new Map([
    ['current build status', '현재 빌드 상태'],
    ['current build', '현재 빌드 상태'],
    ['build status', '현재 빌드 상태'],
    ['what changed today', '오늘 바뀐 것'],
    ['what changed', '오늘 바뀐 것'],
    ['today work', '오늘 바뀐 것'],
    ['test results and evidence', '테스트 결과와 근거'],
    ['test results', '테스트 결과와 근거'],
    ['verification', '검증'],
    ['decisions made', '결정한 것'],
    ['decisions', '결정한 것'],
    ['problems or risks', '문제 또는 리스크'],
    ['problems and risks', '문제 또는 리스크'],
    ['risks', '문제 또는 리스크'],
    ['gpt handoff summary', 'GPT/Claude 인계 요약'],
    ['gpt/claude handoff summary', 'GPT/Claude 인계 요약'],
    ['planning handoff', 'GPT/Claude 인계 요약'],
    ['next codex tasks', '다음 Codex 작업'],
    ['next tasks', '다음 Codex 작업'],
    ['portfolio notes', '포트폴리오 메모'],
  ]);
  const translated = translations.get(key);
  return translated ? `${prefix}${translated}` : text;
}

function isDateReportIndex(filePath) {
  return path.basename(filePath).toLowerCase() === 'index.md'
    && /^\d{8}$/.test(path.basename(path.dirname(filePath)));
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

function projectName() {
  if (!fs.existsSync(PROJECT_BRIEF_PATH)) return '프로젝트';
  const md = fs.readFileSync(PROJECT_BRIEF_PATH, 'utf8');
  const heading = firstHeading(md);
  const headingName = stripMarkdown(heading)
    .replace(/^Project Brief\s*[:|-]?\s*/i, '')
    .replace(/^프로젝트\s*브리프\s*[:|-]?\s*/i, '')
    .trim();
  if (headingName && headingName !== heading) return headingName;

  const summaryMatch = md.match(/##\s+One-Line Summary\s+([\s\S]*?)(?:\n##\s+|$)/i);
  const summary = stripMarkdown(summaryMatch?.[1] || '').trim();
  const leadingName = summary.match(/^([A-Z][A-Z0-9_-]{2,})\b/)?.[1];
  return leadingName || '프로젝트';
}

function projectId() {
  return projectName()
    .trim()
    .toLowerCase()
    .replace(/[^a-z0-9가-힣_-]+/g, '-')
    .replace(/^-+|-+$/g, '') || 'project';
}

function apiReportsIndexHref() {
  return `/api/projects/${projectId()}/reports/index.html`;
}

function apiReportHref(day) {
  return `/api/projects/${projectId()}/reports/${day}/index.html`;
}

function apiUnitHref(day, fileName) {
  return `/api/projects/${projectId()}/reports/${day}/units/${fileName}`;
}
