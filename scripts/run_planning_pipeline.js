#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');
const { spawnSync } = require('child_process');

const options = parseArgs(process.argv.slice(2));
const today = options.date || todayString();
const promptPath = path.resolve(options.prompt || path.join('docs', 'review_prompts', `${today}-pipeline.md`));

main();

function main() {
  const steps = [];

  if (!options.dryRun && !options.skipTests && options.test !== 'none') {
    steps.push(runNpmScript(testScript(options.test)));
  }

  const prompt = buildPlanningPrompt({
    date: today,
    aiSummary: readJsonIfExists(summaryPathFor(options.test)),
    status: readTextIfExists('docs/CODEX_STATUS.md'),
    nextTasks: readTextIfExists('docs/NEXT_TASKS.md'),
    latestClaude: readTextIfExists('docs/review_responses/2026-06-02-claude-v05-eval.md'),
  });

  if (options.dryRun) {
    console.log(`Prompt: ${path.relative(process.cwd(), promptPath)}`);
    console.log(`Provider: ${options.provider}`);
    console.log(`Test: ${options.skipTests ? 'skipped' : options.test}`);
    console.log('');
    console.log(prompt.slice(0, 1600));
    if (prompt.length > 1600) console.log('...');
    console.log('');
    console.log('Next command preview:');
    console.log(reviewCommandPreview(options.provider, promptPath));
    return;
  }

  fs.mkdirSync(path.dirname(promptPath), { recursive: true });
  fs.writeFileSync(promptPath, prompt, 'utf8');
  console.log(`Wrote ${path.relative(process.cwd(), promptPath)}`);

  const response = runPlanningProvider(options.provider, promptPath, today);
  if (response) {
    console.log(`Planning response: ${path.relative(process.cwd(), response.outputPath)}`);
  } else {
    console.log('Planning provider skipped. Prompt is ready for a local external call.');
  }

  if (steps.some((step) => step.status !== 0)) {
    process.exitCode = 1;
  }
}

function runNpmScript(script) {
  console.log(`Running npm run ${script}`);
  const result = spawnSync(`npm run ${script}`, {
    cwd: process.cwd(),
    encoding: 'utf8',
    shell: true,
    stdio: 'inherit',
  });
  if (result.error || result.status !== 0) {
    const error = result.error ? ` (${result.error.message})` : '';
    fail(`npm run ${script} failed with status ${result.status}${error}`);
  }
  return result;
}

function runPlanningProvider(provider, prompt, date) {
  if (provider === 'none') return null;
  if (provider === 'claude') return runReviewScript('claude', prompt, responsePath(date, 'claude'));
  if (provider === 'codex') return runReviewScript('codex', prompt, responsePath(date, 'codex'));

  const claude = runReviewScript('claude', prompt, responsePath(date, 'claude'), { allowFailure: true });
  if (claude.ok) return claude;

  console.warn('Claude planning failed. Falling back to Codex CLI.');
  console.warn(claude.errorMessage);
  return runReviewScript('codex', prompt, responsePath(date, 'codex'));
}

function runReviewScript(provider, prompt, output, opts = {}) {
  const script = provider === 'claude' ? 'scripts/ask_claude_review.js' : 'scripts/ask_codex_review.js';
  const args = [script, '--prompt', prompt, '--output', output];
  const result = spawnSync(process.execPath, args, {
    cwd: process.cwd(),
    encoding: 'utf8',
    stdio: ['ignore', 'pipe', 'pipe'],
    maxBuffer: 1024 * 1024 * 12,
  });

  if (result.stdout) process.stdout.write(result.stdout);
  if (result.stderr) process.stderr.write(result.stderr);

  if (result.error || result.status !== 0) {
    const errorMessage = result.error ? result.error.message : `status ${result.status}`;
    if (opts.allowFailure) {
      return {
        ok: false,
        outputPath: output,
        errorMessage,
      };
    }
    fail(`${provider} planning failed: ${errorMessage}`);
  }

  return {
    ok: true,
    outputPath: output,
  };
}

function buildPlanningPrompt(context) {
  const ai = summarizeAi(context.aiSummary);
  const statusExcerpt = excerptSection(context.status, '## Current Build', '## Implemented');
  const nextExcerpt = excerptSection(context.nextTasks, '## Current Verdict', '## v0.2 Done');
  const claudeExcerpt = context.latestClaude ? context.latestClaude.trim().slice(0, 2200) : '없음';

  return [
    `# LETHE v0.5 테스트 결과 기반 다음 방향 결정 - ${context.date}`,
    '',
    '## 프로젝트 목표',
    '',
    '- 현재 목표는 HTML 프로토타입으로 LETHE의 핵심 재미와 가능성을 검증하는 것이다.',
    '- 충분한 재미와 가능성이 확인되면 Unity 구현 단계로 넘어갈 근거를 만든다.',
    '- 지금은 최종 콘텐츠 확장이 아니라 가능성 검증과 기획 수정 단계다.',
    '',
    '## 현재 빌드',
    '',
    statusExcerpt || '- LETHE HTML alpha v0.5 core-fun human-test ready.',
    '',
    '## 현재 판단',
    '',
    nextExcerpt || '- v0.5는 Chrome headless level-up/runGrowth gate를 통과했고 human-test ready 상태다.',
    '',
    '## 최신 AI 테스트 요약',
    '',
    ai,
    '',
    '## 브라우저 QA 근거',
    '',
    '- Chrome headless `file:///C:/jaewoo/LETHE_Prototype/index.html?qa=fast,levelup` 통과.',
    '- `levelUpSeen: true`, `resumedAfterUpgrade: true`, `hasRunGrowthPayload: true`.',
    '- 실제 선택 내역이 runtime `choicesTaken`과 JSON payload `runGrowth.choicesTaken`에 동시에 기록됐다.',
    '',
    '## 직전 Claude 판단 요약',
    '',
    claudeExcerpt,
    '',
    '## 이번에 판단해줘야 할 것',
    '',
    '1. 지금은 바로 5-8명 사람 테스트로 가야 하는가, 아니면 HTML v0.6을 먼저 한 번 더 만들어야 하는가?',
    '2. 만약 v0.6이 필요하다면, Codex가 구현할 가장 작은 작업 단위 1-3개는 무엇인가?',
    '3. 사람 테스트를 먼저 한다면, 테스트에서 반드시 확인해야 할 관찰 항목과 질문은 무엇인가?',
    '4. Unity 전환 가능성을 판단하려면 어떤 사람 테스트 신호가 필요하며, 어느 정도면 긍정으로 볼 수 있는가?',
    '5. 이번 라운드에서 만들지 말아야 할 범위는 무엇인가?',
    '',
    '## 답변 형식',
    '',
    '## 결론',
    '',
    '- `GO_TO_HUMAN_TEST`, `ITERATE_HTML_V06`, `FIX_CORE` 중 하나를 고른다.',
    '',
    '## 이유',
    '',
    '- 핵심 판단 근거 3-5개',
    '',
    '## 앞으로 해야 할 일',
    '',
    '- [ ] Codex가 구현하거나 준비할 작업 1',
    '- [ ] Codex가 구현하거나 준비할 작업 2',
    '- [ ] Codex가 구현하거나 준비할 작업 3',
    '',
    '## 사람 테스트 기준',
    '',
    '- 초반 재미:',
    '- 성장 선택:',
    '- 기억/망각:',
    '- Unity 전환 가능성:',
    '',
    '## 아직 만들지 말 것',
    '',
    '- 이번 라운드에서 제외할 범위',
    '',
  ].join('\n');
}

function summarizeAi(summary) {
  if (!summary) return '- AI summary file not found. Use docs/CODEX_STATUS.md instead.';
  const metrics = summary.headlineMetrics || {};
  const checks = Array.isArray(summary.targetChecks) ? summary.targetChecks : [];
  const failed = checks.filter((check) => check.pass === false).map((check) => check.name);
  return [
    `- Verdict: \`${summary.gate?.verdict || 'unknown'}\``,
    `- Alpha Fun Score: \`${format(metrics.alphaFunScore || summary.gate?.alphaFunScore)}\``,
    `- Early Fun Score: \`${format(metrics.earlyFunScore)}\``,
    `- Early kill tempo: \`${format(metrics.earlyKillTempo)}\``,
    `- Pre-boss level-ups: \`${format(metrics.earlyLevelUps)}\``,
    `- Regret proxy: \`${percent(metrics.regretRate)}\``,
    `- Irritation proxy: \`${percent(metrics.irritationRate)}\``,
    `- Restart intent: \`${percent(metrics.restartRate)}\``,
    `- Prediction match: \`${percent(metrics.predictionMatchRate)}\``,
    `- First forgetting time: \`${format((metrics.firstForgetUseAvgSec || 0) / 60)} min\``,
    `- Post-forgetting power drop: \`${percent(metrics.avgPowerDrop)}\``,
    `- Recovery after replacement: \`${percent(metrics.avgRecovery)}\``,
    `- Failed/soft checks: ${failed.length ? failed.map((name) => `\`${name}\``).join(', ') : 'none'}`,
    `- Warnings: ${(summary.warnings || []).length ? summary.warnings.join(' / ') : 'none'}`,
  ].join('\n');
}

function excerptSection(markdown, startHeading, endHeading) {
  if (!markdown) return '';
  const start = markdown.indexOf(startHeading);
  if (start < 0) return '';
  const end = markdown.indexOf(endHeading, start + startHeading.length);
  return markdown.slice(start, end > start ? end : undefined).trim();
}

function readTextIfExists(filePath) {
  const full = path.resolve(filePath);
  return fs.existsSync(full) ? fs.readFileSync(full, 'utf8') : '';
}

function readJsonIfExists(filePath) {
  const full = path.resolve(filePath);
  if (!fs.existsSync(full)) return null;
  return JSON.parse(fs.readFileSync(full, 'utf8'));
}

function summaryPathFor(test) {
  if (test === 'quick') return path.join('alpha_test', 'outputs', 'quick', 'summary.json');
  if (test === 'heavy') return path.join('alpha_test', 'outputs', 'heavy', 'summary.json');
  return path.join('alpha_test', 'outputs', 'default', 'summary.json');
}

function responsePath(date, provider) {
  return path.resolve(path.join('docs', 'review_responses', `${date}-pipeline-${provider}.md`));
}

function testScript(test) {
  if (test === 'quick') return 'ai:test:quick';
  if (test === 'heavy') return 'ai:test:heavy';
  return 'ai:test';
}

function reviewCommandPreview(provider, prompt) {
  if (provider === 'codex') return `node scripts/ask_codex_review.js --prompt ${path.relative(process.cwd(), prompt)}`;
  if (provider === 'claude') return `node scripts/ask_claude_review.js --prompt ${path.relative(process.cwd(), prompt)}`;
  return `node scripts/ask_claude_review.js --prompt ${path.relative(process.cwd(), prompt)} || node scripts/ask_codex_review.js --prompt ${path.relative(process.cwd(), prompt)}`;
}

function parseArgs(args) {
  const parsed = {
    date: '',
    dryRun: false,
    prompt: '',
    provider: 'auto',
    skipTests: false,
    test: 'quick',
  };

  for (let index = 0; index < args.length; index += 1) {
    const arg = args[index];
    if (arg === '--dry-run') {
      parsed.dryRun = true;
    } else if (arg === '--skip-tests') {
      parsed.skipTests = true;
    } else if (arg === '--date') {
      parsed.date = args[index + 1] || '';
      index += 1;
    } else if (arg.startsWith('--date=')) {
      parsed.date = arg.slice('--date='.length);
    } else if (arg === '--prompt') {
      parsed.prompt = args[index + 1] || '';
      index += 1;
    } else if (arg.startsWith('--prompt=')) {
      parsed.prompt = arg.slice('--prompt='.length);
    } else if (arg === '--provider') {
      parsed.provider = normalizeProvider(args[index + 1] || '');
      index += 1;
    } else if (arg.startsWith('--provider=')) {
      parsed.provider = normalizeProvider(arg.slice('--provider='.length));
    } else if (arg === '--test') {
      parsed.test = normalizeTest(args[index + 1] || '');
      index += 1;
    } else if (arg.startsWith('--test=')) {
      parsed.test = normalizeTest(arg.slice('--test='.length));
    }
  }

  return parsed;
}

function normalizeProvider(value) {
  if (['auto', 'claude', 'codex', 'none'].includes(value)) return value;
  fail(`Unknown provider: ${value}. Use auto, claude, codex, or none.`);
}

function normalizeTest(value) {
  if (['quick', 'default', 'heavy', 'none'].includes(value)) return value;
  fail(`Unknown test: ${value}. Use quick, default, heavy, or none.`);
}

function todayString() {
  const now = new Date();
  const yyyy = String(now.getFullYear());
  const mm = String(now.getMonth() + 1).padStart(2, '0');
  const dd = String(now.getDate()).padStart(2, '0');
  return `${yyyy}-${mm}-${dd}`;
}

function format(value) {
  if (typeof value !== 'number' || Number.isNaN(value)) return 'n/a';
  return value.toFixed(4).replace(/0+$/, '').replace(/\.$/, '');
}

function percent(value) {
  if (typeof value !== 'number' || Number.isNaN(value)) return 'n/a';
  return `${(value * 100).toFixed(1)}%`;
}

function fail(message) {
  console.error(message);
  process.exit(1);
}
