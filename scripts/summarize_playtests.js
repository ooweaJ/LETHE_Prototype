#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');

const options = parseArgs(process.argv.slice(2));
const date = options.date || todayString();
const inputDir = path.resolve(options.input || 'playtest_logs');
const summaryPath = path.resolve(options.output || path.join('docs', 'playtest_summaries', `${date}.md`));
const promptPath = path.resolve(options.prompt || path.join('docs', 'review_prompts', `${date}-human-playtest.md`));

main();

function main() {
  const logs = readLogs(inputDir);
  const notes = readTextIfExists('docs/PLAYTEST_NOTES.md');
  const summary = buildSummary(logs, notes);
  const prompt = buildPrompt(summary);

  if (options.dryRun) {
    console.log(`Input: ${path.relative(process.cwd(), inputDir)}`);
    console.log(`Logs: ${logs.length}`);
    console.log(`Summary: ${path.relative(process.cwd(), summaryPath)}`);
    console.log(`Prompt: ${path.relative(process.cwd(), promptPath)}`);
    console.log('');
    console.log(summary.slice(0, 1800));
    if (summary.length > 1800) console.log('...');
    return;
  }

  fs.mkdirSync(path.dirname(summaryPath), { recursive: true });
  fs.mkdirSync(path.dirname(promptPath), { recursive: true });
  fs.writeFileSync(summaryPath, summary, 'utf8');
  fs.writeFileSync(promptPath, prompt, 'utf8');
  console.log(`Read ${logs.length} playtest log(s).`);
  console.log(`Wrote ${path.relative(process.cwd(), summaryPath)}`);
  console.log(`Wrote ${path.relative(process.cwd(), promptPath)}`);
  if (logs.length < 5) {
    console.log('Note: fewer than 5 logs. Treat this as preparation output, not a final human-test verdict.');
  }
}

function readLogs(dir) {
  if (!fs.existsSync(dir)) return [];
  return listJsonFiles(dir)
    .map((file) => {
      try {
        const payload = JSON.parse(fs.readFileSync(file, 'utf8'));
        return normalizeLog(file, payload);
      } catch (error) {
        return {
          file,
          invalid: true,
          error: error.message,
        };
      }
    })
    .sort((left, right) => String(left.testerId).localeCompare(String(right.testerId)) || String(left.sessionId).localeCompare(String(right.sessionId)));
}

function listJsonFiles(dir) {
  const out = [];
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      out.push(...listJsonFiles(full));
    } else if (/\.json$/i.test(entry.name)) {
      out.push(full);
    }
  }
  return out;
}

function normalizeLog(file, payload) {
  const playtest = payload.playtest || {};
  const runGrowth = payload.runGrowth || {};
  const choices = Array.isArray(runGrowth.choicesTaken) ? runGrowth.choicesTaken : [];
  const survey = payload.survey || {};
  return {
    file,
    raw: payload,
    testerId: playtest.testerId || '',
    sessionId: playtest.sessionId || '',
    version: payload.version || '',
    elapsed: numberOrNull(payload.elapsed),
    weapon: payload.weapon || '',
    memories: payload.memoryNames || payload.memories || [],
    protect: payload.questionNames?.protect || '',
    predict: payload.questionNames?.predict || '',
    forgotten: payload.forgottenName || payload.forgotten || '',
    sadness: numberOrNull(survey.sadness),
    fairness: numberOrNull(survey.fairness),
    memoryRecall: survey.memoryRecall || '',
    runGrowthLevel: numberOrNull(runGrowth.level),
    levelUpsBeforeBoss: numberOrNull(runGrowth.levelUpsBeforeBoss),
    growthChoices: choices.map((choice) => choice.name || choice.id).filter(Boolean),
  };
}

function buildSummary(logs, notes) {
  const valid = logs.filter((log) => !log.invalid);
  const invalid = logs.filter((log) => log.invalid);
  const enough = valid.length >= 5;
  const sadnessValues = valid.map((log) => log.sadness).filter(isNumber);
  const fairnessValues = valid.map((log) => log.fairness).filter(isNumber);
  const levelUps = valid.map((log) => log.levelUpsBeforeBoss).filter(isNumber);
  const recallCount = valid.filter((log) => log.memoryRecall.trim()).length;
  const highRegret = valid.filter((log) => isNumber(log.sadness) && log.sadness >= 3).length;
  const lowFairness = valid.filter((log) => isNumber(log.fairness) && log.fairness <= 1).length;
  const unknownPredict = valid.filter((log) => log.predict === '모르겠다').length;
  const forgottenCounts = countBy(valid.map((log) => log.forgotten).filter(Boolean));
  const growthCounts = countBy(valid.flatMap((log) => log.growthChoices));

  return [
    `# LETHE Human Playtest Summary - ${date}`,
    '',
    '## Current Verdict',
    '',
    enough
      ? '- Human test sample size reached the minimum 5-session gate.'
      : `- Human test sample is not complete yet: ${valid.length}/5 minimum logs collected.`,
    '- Use this summary as evidence for Claude/GPT planning after human sessions.',
    '',
    '## Input',
    '',
    `- Log directory: \`${path.relative(process.cwd(), inputDir)}\``,
    `- Valid logs: \`${valid.length}\``,
    `- Invalid logs: \`${invalid.length}\``,
    '',
    '## Aggregate Metrics',
    '',
    `- Average Q1 regret/sadness: \`${format(avg(sadnessValues))}\``,
    `- Average Q2 fairness/acceptance: \`${format(avg(fairnessValues))}\``,
    `- High regret count (Q1>=3): \`${highRegret}/${valid.length}\``,
    `- Low fairness risk count (Q2<=1): \`${lowFairness}/${valid.length}\``,
    `- Memory recall filled: \`${recallCount}/${valid.length}\``,
    `- Unknown prediction count: \`${unknownPredict}/${valid.length}\``,
    `- Average pre-boss level-ups: \`${format(avg(levelUps))}\``,
    '',
    '## Forgotten Memory Distribution',
    '',
    tableFromCounts(forgottenCounts),
    '',
    '## Growth Choice Distribution',
    '',
    tableFromCounts(growthCounts),
    '',
    '## Session Rows',
    '',
    valid.length ? sessionTable(valid) : '- No human JSON logs found yet.',
    '',
    invalid.length ? '## Invalid Logs' : '',
    invalid.length ? invalid.map((log) => `- \`${path.relative(process.cwd(), log.file)}\`: ${log.error}`).join('\n') : '',
    invalid.length ? '' : '',
    '## Human Notes Excerpt',
    '',
    notesExcerpt(notes),
    '',
    '## Gate Interpretation',
    '',
    gateInterpretation(valid, { highRegret, lowFairness, recallCount }),
    '',
    '## Next Planning Question',
    '',
    '- Should LETHE proceed toward Unity transition groundwork, iterate HTML v0.6, or fix the core loop first?',
    '- Which observed weakness should Codex address next, if any?',
    '',
  ].filter((line) => line !== '').join('\n');
}

function buildPrompt(summary) {
  return [
    `# LETHE human playtest 기반 다음 방향 결정 - ${date}`,
    '',
    '## 프로젝트 목표',
    '',
    '- HTML 프로토타입으로 핵심 재미와 가능성을 검증한다.',
    '- 충분히 가능성이 있으면 Unity 구현 단계로 넘어갈 근거를 만든다.',
    '- 사람 테스트 결과가 부족하면 HTML v0.6에서 가장 약한 축만 보완한다.',
    '',
    '## 사람 테스트 요약',
    '',
    summary,
    '',
    '## 판단 질문',
    '',
    '1. 현재 사람 테스트 결과는 Unity 전환 근거로 충분한가?',
    '2. 부족하다면 HTML v0.6에서 가장 작게 고칠 작업 1-3개는 무엇인가?',
    '3. 초반 재미, 성장 선택, 기억/망각, 잔향/회복 중 가장 약한 축은 무엇인가?',
    '4. 아직 만들지 말아야 할 것은 무엇인가?',
    '',
    '## 답변 형식',
    '',
    '## 결론',
    '',
    '- `UNITY_GROUNDWORK`, `ITERATE_HTML_V06`, `FIX_CORE`, `NEED_MORE_HUMAN_DATA` 중 하나',
    '',
    '## 이유',
    '',
    '- 핵심 판단 근거 3-5개',
    '',
    '## 앞으로 해야 할 일',
    '',
    '- [ ] Codex 작업 1',
    '- [ ] Codex 작업 2',
    '- [ ] Codex 작업 3',
    '',
    '## 아직 만들지 말 것',
    '',
    '- 제외 범위',
    '',
  ].join('\n');
}

function sessionTable(logs) {
  const rows = [
    '| tester | session | Q1 | Q2 | recall | level-ups | forgotten | growth choices | file |',
    '|---|---|---:|---:|---|---:|---|---|---|',
  ];
  logs.forEach((log) => {
    rows.push([
      log.testerId || '-',
      log.sessionId || '-',
      format(log.sadness),
      format(log.fairness),
      log.memoryRecall ? 'yes' : 'no',
      format(log.levelUpsBeforeBoss),
      escapeCell(log.forgotten || '-'),
      escapeCell(log.growthChoices.join(', ') || '-'),
      `\`${path.relative(process.cwd(), log.file)}\``,
    ].join(' | ').replace(/^/, '| ').replace(/$/, ' |'));
  });
  return rows.join('\n');
}

function tableFromCounts(counts) {
  const entries = Object.entries(counts).sort((a, b) => b[1] - a[1]);
  if (!entries.length) return '- No data.';
  return [
    '| item | count |',
    '|---|---:|',
    ...entries.map(([name, count]) => `| ${escapeCell(name)} | ${count} |`),
  ].join('\n');
}

function gateInterpretation(logs, aggregate) {
  if (logs.length === 0) {
    return [
      '- No human logs yet.',
      '- Run 5-8 sessions using `docs/HUMAN_PLAYTEST_GUIDE.md`.',
      '- Put downloaded JSON logs in `playtest_logs/`, then rerun `npm run playtest:summary`.',
    ].join('\n');
  }

  const lines = [];
  if (logs.length < 5) lines.push(`- Need more human data: ${logs.length}/5 minimum sessions.`);
  if (aggregate.lowFairness >= 2) lines.push('- Risk: 2 or more low-fairness reactions. Investigate irritation/unfairness before Unity groundwork.');
  if (aggregate.highRegret >= Math.ceil(logs.length * 0.6)) lines.push('- Positive: high-regret reactions are the majority.');
  if (aggregate.recallCount < Math.ceil(logs.length * 0.5)) lines.push('- Risk: fewer than half recalled a memory name. Memory identity may need stronger reinforcement.');
  if (!lines.length) lines.push('- Human data is mixed. Read notes before deciding Unity groundwork or HTML v0.6.');
  return lines.join('\n');
}

function notesExcerpt(notes) {
  if (!notes.trim()) return '- No playtest notes found.';
  const content = notes.trim();
  return content.length > 2500 ? `${content.slice(0, 2500).trim()}\n...` : content;
}

function countBy(items) {
  return items.reduce((acc, item) => {
    acc[item] = (acc[item] || 0) + 1;
    return acc;
  }, {});
}

function avg(values) {
  if (!values.length) return null;
  return values.reduce((acc, value) => acc + value, 0) / values.length;
}

function numberOrNull(value) {
  const num = Number(value);
  return Number.isFinite(num) ? num : null;
}

function isNumber(value) {
  return typeof value === 'number' && Number.isFinite(value);
}

function format(value) {
  if (!isNumber(value)) return 'n/a';
  return value.toFixed(2).replace(/0+$/, '').replace(/\.$/, '');
}

function escapeCell(value) {
  return String(value).replace(/\|/g, '/');
}

function readTextIfExists(filePath) {
  const full = path.resolve(filePath);
  return fs.existsSync(full) ? fs.readFileSync(full, 'utf8') : '';
}

function parseArgs(args) {
  const parsed = {
    date: '',
    dryRun: false,
    input: '',
    output: '',
    prompt: '',
  };

  for (let index = 0; index < args.length; index += 1) {
    const arg = args[index];
    if (arg === '--dry-run') {
      parsed.dryRun = true;
    } else if (arg === '--date') {
      parsed.date = args[index + 1] || '';
      index += 1;
    } else if (arg.startsWith('--date=')) {
      parsed.date = arg.slice('--date='.length);
    } else if (arg === '--input') {
      parsed.input = args[index + 1] || '';
      index += 1;
    } else if (arg.startsWith('--input=')) {
      parsed.input = arg.slice('--input='.length);
    } else if (arg === '--output') {
      parsed.output = args[index + 1] || '';
      index += 1;
    } else if (arg.startsWith('--output=')) {
      parsed.output = arg.slice('--output='.length);
    } else if (arg === '--prompt') {
      parsed.prompt = args[index + 1] || '';
      index += 1;
    } else if (arg.startsWith('--prompt=')) {
      parsed.prompt = arg.slice('--prompt='.length);
    }
  }

  return parsed;
}

function todayString() {
  const now = new Date();
  const yyyy = String(now.getFullYear());
  const mm = String(now.getMonth() + 1).padStart(2, '0');
  const dd = String(now.getDate()).padStart(2, '0');
  return `${yyyy}-${mm}-${dd}`;
}
