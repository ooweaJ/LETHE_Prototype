'use strict';

const fs = require('fs');
const path = require('path');

function pct(v) {
  if (v === null || v === undefined) return '-';
  return `${(v * 100).toFixed(1)}%`;
}

function num(v, digits = 2) {
  if (v === null || v === undefined) return '-';
  return Number(v).toFixed(digits);
}

function passIcon(pass) {
  return pass ? '✅' : '⚠️';
}

function makeMarkdown(summary) {
  const m = summary.headlineMetrics;
  const lines = [];
  lines.push(`# LETHE AI 알파테스트 리포트`);
  lines.push('');
  lines.push(`- 생성 시각: ${summary.generatedAt}`);
  lines.push(`- 실행 조건: runs=${summary.options.runs}, stages=${summary.options.stages}, seed=${summary.options.seed}, echoPower=${summary.options.echoPower}, uiClarity=${summary.options.uiClarity}`);
  lines.push(`- 판정: **${summary.gate.verdict}**`);
  lines.push(`- Alpha Fun Score: **${num(summary.gate.alphaFunScore, 3)} / 1.000**`);
  lines.push(`- 플레이어블 판정: **${summary.playability.label}**`);
  lines.push('');
  lines.push(`> 주의: 이 리포트의 Q1/Q2는 실제 플레이어 감정이 아니라 봇 기반 프록시입니다. 사람 테스트 전에 밸런스 위험을 거르는 용도입니다.`);
  lines.push('');
  lines.push(`## 지금 할만한가?`);
  lines.push('');
  lines.push(`**${summary.playability.label}**`);
  lines.push('');
  lines.push(summary.playability.summary);
  lines.push('');
  lines.push(`- Playable Score: ${num(summary.playability.playableScore, 3)} / 1.000`);
  lines.push(`- Risk Level: ${summary.playability.riskLevel}`);
  lines.push(`- 다음 액션: ${summary.playability.nextStep}`);
  if (summary.playability.hardFails.length) lines.push(`- 하드 실패: ${summary.playability.hardFails.join(', ')}`);
  if (summary.playability.softFails.length) lines.push(`- 소프트 실패: ${summary.playability.softFails.join(', ')}`);
  lines.push('');
  lines.push(`## 핵심 지표`);
  lines.push('');
  lines.push(`| 지표 | 값 | 해석 |`);
  lines.push(`|---|---:|---|`);
  lines.push(`| 클리어율 | ${pct(m.clearRate)} | 난이도 기준선 |`);
  lines.push(`| 아쉬움 분면 | ${pct(m.regretRate)} | 목표: 60% 이상 |`);
  lines.push(`| 짜증 분면 | ${pct(m.irritationRate)} | 목표: 20% 미만 |`);
  lines.push(`| 무덤덤 분면 | ${pct(m.dullRate)} | 목표: 25% 미만 |`);
  lines.push(`| 예측 일치율 | ${pct(m.predictionMatchRate)} | 목표: 60~65% 이상 |`);
  lines.push(`| 삭제 직후 즉시 종료율 | ${pct(m.immediateQuitRate)} | 목표: 15% 미만 |`);
  lines.push(`| 런 재시작 의향 | ${pct(m.restartRate)} | 목표: 60% 이상 |`);
  lines.push(`| 망각 직후 전투력 딥 | ${pct(m.avgPowerDrop)} | 목표: 30~50% |`);
  lines.push(`| 새 기억 후 회복률 | ${pct(m.avgRecovery)} | 목표: 85~110% |`);
  lines.push(`| 첫 망각 전 사용 시간 | ${num(m.firstForgetUseAvgSec / 60, 2)}분 | 목표: 8~10분 |`);
  lines.push('');
  lines.push(`## Go/No-Go 체크`);
  lines.push('');
  lines.push(`| 체크 | 값 | 기준 | 결과 |`);
  lines.push(`|---|---:|---:|:---:|`);
  for (const c of summary.targetChecks) {
    lines.push(`| ${c.name} | ${formatValue(c.value, c.name)} | ${c.op} ${formatValue(c.target, c.name)} | ${passIcon(c.pass)} |`);
  }
  lines.push('');
  lines.push(`## 분포`);
  lines.push('');
  lines.push(`### 4분면`);
  lines.push(histTable(summary.distributions.quadrants, summary.counts.forgetEvents));
  lines.push('');
  lines.push(`### 빌드 분류`);
  lines.push(histTable(summary.distributions.buildClasses, summary.counts.forgetEvents));
  lines.push('');
  lines.push(`### 삭제된 기억`);
  lines.push(histTable(summary.distributions.deletedMemoryNames, summary.counts.forgetEvents));
  lines.push('');
  lines.push(`## 봇별 결과`);
  lines.push('');
  lines.push(`| 봇 | 이벤트 수 | 아쉬움 | 짜증 | 예측 일치 | 즉시 종료 | 평균 딥 | 평균 회복 |`);
  lines.push(`|---|---:|---:|---:|---:|---:|---:|---:|`);
  for (const [bot, row] of Object.entries(summary.byBot)) {
    lines.push(`| ${bot} | ${row.n} | ${pct(row.regretRate)} | ${pct(row.irritationRate)} | ${pct(row.predictionMatchRate)} | ${pct(row.immediateQuitRate)} | ${pct(row.avgPowerDrop)} | ${pct(row.avgRecovery)} |`);
  }
  lines.push('');
  lines.push(`## 경고`);
  lines.push('');
  if (summary.warnings.length) summary.warnings.forEach((w) => lines.push(`- ${w}`));
  else lines.push('- 중대한 자동 경고 없음.');
  lines.push('');
  lines.push(`## 튜닝 제안`);
  lines.push('');
  if (summary.tuningSuggestions.length) summary.tuningSuggestions.forEach((s) => lines.push(`- ${s}`));
  else lines.push('- 현 설정은 AI 알파 기준으로 다음 인간 테스트에 진입 가능.');
  lines.push('');
  return lines.join('\n');
}

function makeHtml(summary) {
  const markdown = makeMarkdown(summary)
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;');
  const data = JSON.stringify(summary).replace(/</g, '\\u003c');
  return `<!doctype html>
<html lang="ko">
<head>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  <title>LETHE AI Alpha Test Report</title>
  <style>
    :root { color-scheme: dark; }
    body { margin: 0; font-family: system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif; background:#080b12; color:#e8edf7; }
    header { padding: 28px 32px; background: linear-gradient(135deg,#111827,#0d3b46); border-bottom:1px solid #234; }
    h1 { margin: 0 0 8px; font-size: 28px; }
    .wrap { max-width: 1160px; margin: 0 auto; padding: 24px; }
    .grid { display:grid; grid-template-columns: repeat(auto-fit, minmax(220px,1fr)); gap:14px; margin: 18px 0; }
    .card { background:#0f172a; border:1px solid #233047; border-radius:14px; padding:16px; box-shadow:0 12px 40px rgba(0,0,0,.22); }
    .card .label { color:#9fb3c8; font-size:13px; }
    .card .value { font-size:26px; margin-top:6px; font-weight:700; }
    .good { color:#7dd3fc; } .warn { color:#fcd34d; } .bad { color:#fca5a5; }
    table { border-collapse: collapse; width:100%; margin: 12px 0 24px; overflow:hidden; border-radius: 10px; }
    th,td { border:1px solid #28364e; padding:8px 10px; text-align:left; }
    th { background:#111827; color:#b7c6d9; }
    td.num { text-align:right; font-variant-numeric: tabular-nums; }
    pre { white-space: pre-wrap; background:#020617; border:1px solid #233047; padding:20px; border-radius:14px; line-height:1.55; }
    .pill { display:inline-block; padding:4px 9px; border-radius:999px; background:#132236; border:1px solid #2f445f; color:#cfe8ff; font-size:12px; }
  </style>
</head>
<body>
  <header>
    <div class="wrap">
      <h1>LETHE AI 알파테스트 리포트</h1>
      <div class="pill">${summary.gate.verdict}</div>
      <div class="pill">${summary.playability.label}</div>
      <div class="pill">Alpha Fun Score ${Number(summary.gate.alphaFunScore).toFixed(3)}</div>
      <div class="pill">${summary.counts.totalRuns} runs / ${summary.counts.forgetEvents} forget events</div>
    </div>
  </header>
  <main class="wrap">
    <div class="grid">
      ${metricCard('아쉬움', summary.headlineMetrics.regretRate, 0.60, true)}
      ${metricCard('짜증', summary.headlineMetrics.irritationRate, 0.20, false)}
      ${metricCard('예측 일치', summary.headlineMetrics.predictionMatchRate, 0.625, true)}
      ${metricCard('즉시 종료', summary.headlineMetrics.immediateQuitRate, 0.15, false)}
      ${metricCard('전투력 딥', summary.headlineMetrics.avgPowerDrop, 0.40, null)}
      ${metricCard('회복률', summary.headlineMetrics.avgRecovery, 0.85, true)}
    </div>
    <h2>요약</h2>
    <pre>${markdown}</pre>
  </main>
  <script id="report-data" type="application/json">${data}</script>
</body>
</html>`;
}

function metricCard(label, value, target, higherIsGood) {
  let cls = 'warn';
  if (higherIsGood === true) cls = value >= target ? 'good' : 'bad';
  else if (higherIsGood === false) cls = value <= target ? 'good' : 'bad';
  else cls = value >= 0.30 && value <= 0.50 ? 'good' : 'warn';
  return `<div class="card"><div class="label">${label}</div><div class="value ${cls}">${pct(value)}</div></div>`;
}

function histTable(hist, total) {
  const rows = Object.entries(hist || {}).sort((a, b) => b[1] - a[1]);
  if (!rows.length) return '_데이터 없음_';
  const lines = ['| 항목 | 개수 | 비율 |', '|---|---:|---:|'];
  for (const [k, v] of rows) lines.push(`| ${k} | ${v} | ${pct(v / total)} |`);
  return lines.join('\n');
}

function formatValue(v, name) {
  if (name.includes('시간')) return `${(v / 60).toFixed(2)}분`;
  if (Math.abs(v) <= 1.5) return pct(v);
  return String(v);
}

function writeReports(outDir, summary) {
  fs.mkdirSync(outDir, { recursive: true });
  fs.writeFileSync(path.join(outDir, 'alpha_summary.md'), makeMarkdown(summary), 'utf8');
  fs.writeFileSync(path.join(outDir, 'alpha_report.html'), makeHtml(summary), 'utf8');
}

module.exports = { makeMarkdown, makeHtml, writeReports };
