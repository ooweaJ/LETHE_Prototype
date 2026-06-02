'use strict';

const { MEMORIES, DEFAULT_TARGETS } = require('./config');

function round(value, digits = 4) {
  if (value === null || value === undefined || Number.isNaN(value)) return null;
  const p = 10 ** digits;
  return Math.round(value * p) / p;
}

function pct(n, d) {
  return d > 0 ? n / d : 0;
}

function mean(arr) {
  return arr.length ? arr.reduce((a, b) => a + b, 0) / arr.length : 0;
}

function median(arr) {
  if (!arr.length) return 0;
  const s = arr.slice().sort((a, b) => a - b);
  const mid = Math.floor(s.length / 2);
  return s.length % 2 ? s[mid] : (s[mid - 1] + s[mid]) / 2;
}

function histogram(items) {
  const out = {};
  for (const item of items) out[item] = (out[item] || 0) + 1;
  return out;
}

function flattenStages(runs) {
  const rows = [];
  for (const run of runs) {
    for (const stage of run.stageLogs) {
      if (!stage.failedBeforeForget && stage.deletedMemoryId) {
        rows.push({ run, stage });
      }
    }
  }
  return rows;
}

function summarize(batch) {
  const runs = batch.runs;
  const stages = flattenStages(runs);
  const totalRuns = runs.length;
  const completedRuns = runs.filter((r) => !r.failed).length;
  const failureRate = pct(totalRuns - completedRuns, totalRuns);
  const firstStages = runs.map((r) => r.stageLogs.find((s) => !s.failedBeforeForget)).filter(Boolean);

  const quadrants = histogram(stages.map(({ stage }) => stage.emotion.quadrant));
  const predictionMatches = stages.filter(({ stage }) => stage.predictionMatch).length;
  const immediateQuits = stages.filter(({ stage }) => stage.emotion.immediateQuit).length;
  const restartRuns = runs.filter((r) => r.restartIntent).length;
  const buildClasses = histogram(stages.map(({ stage }) => stage.build.buildClass));
  const deletedMemories = histogram(stages.map(({ stage }) => stage.deletedMemoryId));
  const deletedMemoryNames = {};
  Object.keys(deletedMemories).forEach((id) => { deletedMemoryNames[MEMORIES[id]?.name || id] = deletedMemories[id]; });
  const byBot = groupBy(stages, ({ run }) => run.botId);
  const byBuildClass = groupBy(stages, ({ stage }) => stage.build.buildClass);

  const stageCount = stages.length;
  const regretRate = pct(quadrants['아쉬움'] || 0, stageCount);
  const unstableRegretRate = pct(quadrants['불안정한 아쉬움'] || 0, stageCount);
  const irritationRate = pct(quadrants['짜증'] || 0, stageCount);
  const dullRate = pct(quadrants['무덤덤'] || 0, stageCount);
  const confusionRate = pct(quadrants['혼란/노이즈'] || 0, stageCount);
  const predictionMatchRate = pct(predictionMatches, stageCount);
  const immediateQuitRate = pct(immediateQuits, stageCount);
  const restartRate = pct(restartRuns, totalRuns);
  const clearRate = pct(completedRuns, totalRuns);
  const avgPowerDrop = mean(stages.map(({ stage }) => stage.powerDrop));
  const medPowerDrop = median(stages.map(({ stage }) => stage.powerDrop));
  const avgRecovery = mean(stages.map(({ stage }) => stage.recoveryRatio));
  const medRecovery = median(stages.map(({ stage }) => stage.recoveryRatio));
  const firstUseAvg = mean(firstStages.map((s) => s.stageTimeSec));
  const earlyFunScore = mean(stages.map(({ stage }) => stage.earlyLoop?.earlyFunScore || 0));
  const earlyKillTempo = mean(stages.map(({ stage }) => stage.earlyLoop?.killTempo || 0));
  const earlyCrowdPressure = mean(stages.map(({ stage }) => stage.earlyLoop?.crowdPressure || 0));
  const earlyChoiceInterest = mean(stages.map(({ stage }) => stage.earlyLoop?.choiceInterest || 0));
  const pressureContrast = mean(stages.map(({ stage }) => stage.earlyLoop?.pressureContrast || 0));
  const earlyLevelUps = mean(stages.map(({ stage }) => stage.earlyLoop?.levelUpsBeforeBoss || 0));
  const cycleCompletionRate = pct(stages.filter(({ stage }) => stage.cycleCompleted).length, stageCount);
  const firstCycleCompletionRate = pct(firstStages.filter((stage) => stage.cycleCompleted).length, firstStages.length);
  const twoMemorySurvivalRate = mean(stages.map(({ stage }) => stage.deficitSurvivalChance || 0));
  const refillReachedRate = pct(stages.filter(({ stage }) => stage.refillReached).length, stageCount);
  const echoPivotScore = mean(stages.map(({ stage }) => stage.echoPivotScore || 0));
  const postLossChallengeScore = mean(stages.map(({ stage }) => stage.postLossChallenge?.score || 0));
  const postLossChallengeContrast = mean(stages.map(({ stage }) => stage.postLossChallenge?.contrast || 0));

  const buildDiversity = normalizedDiversity(buildClasses, stageCount);
  const memoryDeleteMaxShare = maxShare(deletedMemories, stageCount);
  const memoryDeleteMinShare = minShare(deletedMemories, stageCount);
  const memoryDeleteSpread = Math.max(0, memoryDeleteMaxShare - memoryDeleteMinShare);
  const buildClassMaxShare = maxShare(buildClasses, stageCount);
  const alphaFunScore = clamp01(
    regretRate * 0.23
      + predictionMatchRate * 0.18
      + earlyFunScore * 0.19
      + echoPivotScore * 0.08
      + (1 - immediateQuitRate) * 0.14
      + restartRate * 0.14
      + buildDiversity * 0.05
      + clamp01(1 - Math.abs(avgPowerDrop - 0.40) / 0.40) * 0.04,
  );

  const targetChecks = makeTargetChecks({
    regretRate,
    irritationRate,
    dullRate,
    predictionMatchRate,
    immediateQuitRate,
    restartRate,
    firstUseAvg,
    buildClassMaxShare,
    memoryDeleteMaxShare,
    memoryDeleteSpread,
    avgPowerDrop,
    avgRecovery,
    echoPower: batch.options.echoPower,
    earlyFunScore,
    earlyLevelUps,
    earlyKillTempo,
    firstCycleCompletionRate,
    twoMemorySurvivalRate,
    echoPivotScore,
    refillReachedRate,
    failureRate,
  });
  const verdict = gateVerdict(targetChecks);

  return {
    generatedAt: new Date().toISOString(),
    options: batch.options,
    counts: { totalRuns, completedRuns, failedRuns: totalRuns - completedRuns, forgetEvents: stageCount },
    gate: {
      verdict,
      alphaFunScore: round(alphaFunScore, 4),
      humanEmotionWarning: 'Q1/Q2는 실제 인간 감정이 아니라 봇 기반 감정 프록시입니다. 최종 Go/No-Go는 사람 테스트로 확정해야 합니다.',
    },
    playability: makePlayabilityAssessment({
      verdict,
      alphaFunScore,
      clearRate,
      regretRate,
      irritationRate,
      predictionMatchRate,
      immediateQuitRate,
      restartRate,
      avgPowerDrop,
      avgRecovery,
      earlyFunScore,
      earlyKillTempo,
      earlyCrowdPressure,
      earlyChoiceInterest,
      earlyLevelUps,
      firstCycleCompletionRate,
      cycleCompletionRate,
      twoMemorySurvivalRate,
      refillReachedRate,
      echoPivotScore,
      failureRate,
      targetChecks,
    }),
    headlineMetrics: {
      clearRate: round(clearRate, 4),
      failureRate: round(failureRate, 4),
      regretRate: round(regretRate, 4),
      unstableRegretRate: round(unstableRegretRate, 4),
      irritationRate: round(irritationRate, 4),
      dullRate: round(dullRate, 4),
      confusionRate: round(confusionRate, 4),
      predictionMatchRate: round(predictionMatchRate, 4),
      immediateQuitRate: round(immediateQuitRate, 4),
      restartRate: round(restartRate, 4),
      avgPowerDrop: round(avgPowerDrop, 4),
      medPowerDrop: round(medPowerDrop, 4),
      avgRecovery: round(avgRecovery, 4),
      medRecovery: round(medRecovery, 4),
      earlyFunScore: round(earlyFunScore, 4),
      earlyKillTempo: round(earlyKillTempo, 4),
      earlyCrowdPressure: round(earlyCrowdPressure, 4),
      earlyChoiceInterest: round(earlyChoiceInterest, 4),
      pressureContrast: round(pressureContrast, 4),
      earlyLevelUps: round(earlyLevelUps, 2),
      firstCycleCompletionRate: round(firstCycleCompletionRate, 4),
      cycleCompletionRate: round(cycleCompletionRate, 4),
      twoMemorySurvivalRate: round(twoMemorySurvivalRate, 4),
      refillReachedRate: round(refillReachedRate, 4),
      echoPivotScore: round(echoPivotScore, 4),
      postLossChallengeScore: round(postLossChallengeScore, 4),
      postLossChallengeContrast: round(postLossChallengeContrast, 4),
      firstForgetUseAvgSec: round(firstUseAvg, 1),
      buildDiversity: round(buildDiversity, 4),
      buildClassMaxShare: round(buildClassMaxShare, 4),
      memoryDeleteMaxShare: round(memoryDeleteMaxShare, 4),
      memoryDeleteMinShare: round(memoryDeleteMinShare, 4),
      memoryDeleteSpread: round(memoryDeleteSpread, 4),
    },
    distributions: {
      quadrants,
      buildClasses,
      deletedMemories,
      deletedMemoryNames,
    },
    byBot: summarizeGroups(byBot),
    byBuildClass: summarizeGroups(byBuildClass),
    targetChecks,
    warnings: makeWarnings(targetChecks, { buildClasses, deletedMemoryNames, stageCount }),
    tuningSuggestions: makeTuningSuggestions(targetChecks, { buildClasses, deletedMemoryNames, stageCount }),
  };
}

function makeTargetChecks(m) {
  const t = DEFAULT_TARGETS;
  return [
    check('아쉬움 분면', m.regretRate, '>=', t.regretRateMin, 'Q1>=3 && Q2>=3 비율'),
    check('짜증 분면', m.irritationRate, '<=', t.irritationRateMax, 'Q1>=3 && Q2<=1 비율'),
    check('무덤덤 분면', m.dullRate, '<=', t.dullRateMax, 'Q1<=1 비율'),
    check('예측 일치율', m.predictionMatchRate, '>=', t.predictionMatchMin, '플레이어 예측=실제 삭제'),
    check('예측 일치율 상한', m.predictionMatchRate, '<=', t.predictionMatchMax, '정답 확인처럼 느껴지는 위험 방지'),
    check('삭제 직후 즉시 종료율', m.immediateQuitRate, '<=', t.immediateQuitMax, '무력감/레이지퀏 프록시'),
    check('런 재시작 의향', m.restartRate, '>=', t.restartRateMin, '한 판 더 욕구 프록시'),
    check('사망/실패율 하한', m.failureRate, '>=', t.failureRateMin, '실제 위험이 없는 과안전 상태 방지'),
    check('사망/실패율 상한', m.failureRate, '<=', t.failureRateMax, '즉사/불합리 난이도 방지'),
    check('초반 재미 점수', m.earlyFunScore, '>=', t.earlyFunScoreMin, '초반 압박/처치/성장 선택 프록시'),
    check('초반 레벨업 수', m.earlyLevelUps, '>=', t.earlyLevelUpsMin, '보스 전 런 중 성장 선택 횟수'),
    check('초반 처치 템포', m.earlyKillTempo, '>=', t.earlyKillTempoMin, '적 처치 리듬과 밀도 프록시'),
    check('첫 망각 전 사용 시간 하한', m.firstUseAvg, '>=', t.firstForgetUseMinSec, '첫 사이클 3.5분 이상'),
    check('첫 망각 전 사용 시간 상한', m.firstUseAvg, '<=', t.firstForgetUseMaxSec, '첫 사이클 4.5분 이하'),
    check('첫 사이클 완주율', m.firstCycleCompletionRate, '>=', t.firstCycleCompletionRateMin, '첫 상실->결손 생존->보충 완료율'),
    check('2기억 생존율 하한', m.twoMemorySurvivalRate, '>=', t.twoMemorySurvivalRateMin, '결손 구간이 너무 가혹하지 않은지'),
    check('2기억 생존율 상한', m.twoMemorySurvivalRate, '<=', t.twoMemorySurvivalRateMax, '결손 구간이 너무 안전하지 않은지'),
    check('잔향 피벗 점수', m.echoPivotScore, '>=', t.echoPivotScoreMin, '잃은 기억이 빌드 변형으로 남는지'),
    check('기억 보충 도달율', m.refillReachedRate, '>=', t.refillReachedRateMin, '상실 후 보충까지 도달하는 비율'),
    check('빌드 분류 쏠림', m.buildClassMaxShare, '<=', t.maxBuildClassShare, '몰빵/거미줄/느슨 중 한 분류 80% 초과 금지'),
    check('단일 기억 삭제 쏠림', m.memoryDeleteMaxShare, '<=', t.maxSingleMemoryDeleteShare, '특정 기억 삭제율 과다 금지'),
    check('기억 삭제 분포 편차', m.memoryDeleteSpread, '<=', t.maxMemoryDeleteSpread, '특정 기억이 테스트 전체를 과도하게 대표하지 않게 유지'),
    check('망각 직후 전투력 딥 하한', m.avgPowerDrop, '>=', t.postForgetDropMin, '상실 체감'),
    check('망각 직후 전투력 딥 상한', m.avgPowerDrop, '<=', t.postForgetDropMax, '압수감 방지'),
    check('새 기억 후 회복 하한', m.avgRecovery, '>=', t.recoveryMin, '피벗 가능성'),
    check('새 기억 후 회복 상한', m.avgRecovery, '<=', t.recoveryMax, '망각이 순이익화되는 위험 방지'),
    check('잔향 강도 하한', m.echoPower, '>=', t.echoPowerSweetSpotMin, '잔향 50~70% 스위트스팟'),
    check('잔향 강도 상한', m.echoPower, '<=', t.echoPowerSweetSpotMax, '잔향 50~70% 스위트스팟'),
  ];
}

function check(name, value, op, target, note) {
  let pass = false;
  if (op === '>=') pass = value >= target;
  else if (op === '<=') pass = value <= target;
  return { name, value: round(value, 4), op, target, pass, note };
}

function gateVerdict(checks) {
  const hardNames = new Set(['아쉬움 분면', '짜증 분면', '예측 일치율', '삭제 직후 즉시 종료율', '사망/실패율 하한', '사망/실패율 상한', '초반 재미 점수', '초반 처치 템포', '첫 사이클 완주율', '2기억 생존율 하한', '잔향 피벗 점수', '기억 보충 도달율']);
  const hardFails = checks.filter((c) => hardNames.has(c.name) && !c.pass);
  const softFails = checks.filter((c) => !hardNames.has(c.name) && !c.pass);
  if (hardFails.length === 0 && softFails.length <= 2) return 'GO_CANDIDATE';
  if (hardFails.length <= 1 && softFails.length <= 4) return 'ITERATE';
  return 'NO_GO_FIX_CORE';
}

function makePlayabilityAssessment(m) {
  const failed = m.targetChecks.filter((c) => !c.pass);
  const hardNames = new Set(['아쉬움 분면', '짜증 분면', '예측 일치율', '삭제 직후 즉시 종료율', '사망/실패율 하한', '사망/실패율 상한', '초반 재미 점수', '초반 처치 템포']);
  const hardFails = failed.filter((c) => hardNames.has(c.name)).map((c) => c.name);
  const softFails = failed.filter((c) => !hardNames.has(c.name)).map((c) => c.name);
  let label = '튜닝 후 재검증';
  let summary = '망각 루프의 가능성은 보이지만, 사람 테스트 전에 몇 개의 튜닝 레버를 조정하는 편이 좋습니다.';
  let nextStep = '경고가 뜬 항목을 조정한 뒤 300~1000런을 다시 돌리고, 이후 8~12명 소규모 플레이테스트로 확인하세요.';

  if (m.verdict === 'GO_CANDIDATE') {
    label = 'AI 기준 사람 테스트 진입 가능';
    summary = '봇 프록시 기준으로 망각이 짜증보다 아쉬움에 가깝게 작동합니다. 완성 판정은 아니지만, 사람에게 보여줄 만한 상태입니다.';
    nextStep = '사람 테스트에서는 망각 직전 예측, 망각 직후 감정, 잔향 피벗 이해도를 집중 관찰하세요.';
  } else if (m.verdict === 'NO_GO_FIX_CORE') {
    label = '코어 수정 우선';
    summary = '봇 프록시 기준으로 핵심 게이트가 흔들립니다. 콘텐츠를 늘리기 전에 의존도 공식, 예고 UI, 잔향 강도를 먼저 손봐야 합니다.';
    nextStep = '하드 게이트 실패 항목을 먼저 고친 뒤 quick 테스트로 방향을 확인하세요.';
  }

  return {
    label,
    summary,
    nextStep,
    playableScore: round(m.alphaFunScore, 4),
    riskLevel: m.verdict === 'GO_CANDIDATE' ? 'LOW' : m.verdict === 'ITERATE' ? 'MEDIUM' : 'HIGH',
    hardFails,
    softFails,
  };
}

function makeWarnings(checks, dist) {
  const failed = checks.filter((c) => !c.pass);
  const warnings = failed.map((c) => `${c.name} 미달: ${c.value} ${c.op} ${c.target} 실패 (${c.note})`);
  const maxBuild = topEntry(dist.buildClasses);
  if (maxBuild && maxBuild[1] / dist.stageCount > 0.65) warnings.push(`빌드 분포가 '${maxBuild[0]}'에 다소 쏠림: ${Math.round((maxBuild[1] / dist.stageCount) * 100)}%`);
  const maxDeleted = topEntry(dist.deletedMemoryNames);
  if (maxDeleted && maxDeleted[1] / dist.stageCount > 0.30) warnings.push(`삭제 기억이 '${maxDeleted[0]}'에 쏠림: ${Math.round((maxDeleted[1] / dist.stageCount) * 100)}%`);
  return warnings;
}

function makeTuningSuggestions(checks, dist) {
  const failed = new Set(checks.filter((c) => !c.pass).map((c) => c.name));
  const suggestions = [];
  if (failed.has('아쉬움 분면') || failed.has('무덤덤 분면')) {
    suggestions.push('애착 부족: 첫 망각 전 시간을 늘리거나, 기억 발동 이펙트/시너지 체감을 키우고, 3슬롯 조합 완성 후 보스에 들어가게 한다.');
  }
  if (failed.has('초반 재미 점수') || failed.has('초반 처치 템포') || failed.has('초반 레벨업 수')) {
    suggestions.push('초반 루즈함: 1분 안에 적 밀도를 높이고, 처치 경험치와 3지선다 런 성장 선택을 보장해 보스 전 2회 이상 성장하게 한다.');
  }
  if (failed.has('짜증 분면') || failed.has('예측 일치율')) {
    suggestions.push('납득 부족: 전투 중 선명도 UI를 강화하고, 의존도 공식에서 숨은 대체불가능성 가중치를 낮추거나 보스전 흔적 가중치를 올린다.');
  }
  if (failed.has('삭제 직후 즉시 종료율') || failed.has('망각 직후 전투력 딥 상한')) {
    suggestions.push('압수감 과다: 망각 직후 적응 보너스, 즉시 대체 기억 제시, 다음 방 난이도 완충을 추가한다.');
  }
  if (failed.has('망각 직후 전투력 딥 하한') || failed.has('새 기억 후 회복 상한')) {
    suggestions.push('상실감 약함/순이익 위험: 잔향 강도를 낮추거나 삭제 직후 즉시 보상 수치를 줄여 능동 손맛 상실을 분명히 만든다.');
  }
  if (failed.has('새 기억 후 회복 하한')) {
    suggestions.push('피벗 약함: 잔향과 새 기억/무기 친화도를 더 강하게 연결하고, 삭제 직후 후보 3개 중 잔향 시너지 후보를 최소 1개 보장한다.');
  }
  if (failed.has('빌드 분류 쏠림')) {
    suggestions.push('빌드 다양성 부족: 적 역할군 비율과 기억 시너지 보너스를 조정해 몰빵/분산/거미줄이 모두 등장하게 한다.');
  }
  if (failed.has('단일 기억 삭제 쏠림')) {
    suggestions.push('삭제 쏠림: 가장 많이 삭제된 기억의 baseDps/존재감/보스전 적합도를 낮추거나, 다른 기억의 보스 패턴 대응 구간을 늘린다.');
  }
  const maxDeleted = topEntry(dist.deletedMemoryNames || {});
  if (maxDeleted) suggestions.push(`가장 자주 삭제된 기억 '${maxDeleted[0]}'은 우선 튜닝 후보로 표시한다.`);
  return suggestions;
}

function groupBy(items, keyFn) {
  const m = new Map();
  for (const item of items) {
    const key = keyFn(item);
    if (!m.has(key)) m.set(key, []);
    m.get(key).push(item);
  }
  return m;
}

function summarizeGroups(grouped) {
  const out = {};
  for (const [key, rows] of grouped.entries()) {
    const n = rows.length;
    const quadrants = histogram(rows.map(({ stage }) => stage.emotion.quadrant));
    out[key] = {
      n,
      regretRate: round(pct(quadrants['아쉬움'] || 0, n), 4),
      irritationRate: round(pct(quadrants['짜증'] || 0, n), 4),
      predictionMatchRate: round(pct(rows.filter(({ stage }) => stage.predictionMatch).length, n), 4),
      immediateQuitRate: round(pct(rows.filter(({ stage }) => stage.emotion.immediateQuit).length, n), 4),
      avgPowerDrop: round(mean(rows.map(({ stage }) => stage.powerDrop)), 4),
      avgRecovery: round(mean(rows.map(({ stage }) => stage.recoveryRatio)), 4),
      deleted: histogram(rows.map(({ stage }) => stage.deletedMemoryName)),
      quadrants,
    };
  }
  return out;
}

function maxShare(hist, total) {
  if (!total) return 0;
  return Math.max(0, ...Object.values(hist).map((v) => v / total));
}

function minShare(hist, total) {
  if (!total) return 0;
  const values = Object.values(hist);
  if (!values.length) return 0;
  return Math.min(...values.map((v) => v / total));
}

function topEntry(hist) {
  const entries = Object.entries(hist || {});
  if (!entries.length) return null;
  return entries.sort((a, b) => b[1] - a[1])[0];
}

function normalizedDiversity(hist, total) {
  const values = Object.values(hist);
  if (!values.length || total <= 0) return 0;
  const k = values.length;
  if (k <= 1) return 0;
  const entropy = values.reduce((acc, v) => {
    const p = v / total;
    return p > 0 ? acc - p * Math.log(p) : acc;
  }, 0);
  return entropy / Math.log(k);
}

function clamp01(v) {
  return Math.max(0, Math.min(1, v));
}

function runsToCsv(runs) {
  const headers = [
    'runIndex', 'botId', 'weaponName', 'stageIndex', 'buildClass', 'concentrationIndex', 'synergyConnectivity',
    'deletedMemoryName', 'predictedMemoryName', 'leastWantedName', 'predictionMatch', 'deletedWasLeastWanted',
    'q1Pain', 'q2Understanding', 'quadrant', 'immediateQuit', 'restartIntent', 'powerDrop', 'recoveryRatio',
    'earlyFunScore', 'earlyKillTempo', 'earlyCrowdPressure', 'earlyChoiceInterest', 'earlyLevelUps',
    'cycleCompleted', 'deficitSurvivalChance', 'postLossChallengeScore', 'postLossChallengeContrast', 'refillReached', 'echoPivotScore',
    'preForgetPower', 'postDeletePower', 'postReplacementPower', 'replacementName', 'deletionWeights', 'echoPower', 'uiClarity',
    'activeMemoryNamesBefore', 'activeMemoryNamesAfter'
  ];
  const rows = [];
  for (const run of runs) {
    for (const stage of run.stageLogs) {
      if (stage.failedBeforeForget) continue;
      rows.push(headers.map((h) => csvValue(extractCsvValue(run, stage, h))).join(','));
    }
  }
  return `${headers.join(',')}\n${rows.join('\n')}\n`;
}

function extractCsvValue(run, stage, h) {
  const map = {
    runIndex: run.runIndex,
    botId: run.botId,
    weaponName: run.weaponName,
    stageIndex: stage.stageIndex,
    buildClass: stage.build.buildClass,
    concentrationIndex: stage.build.concentrationIndex,
    synergyConnectivity: stage.build.synergyConnectivity,
    deletedMemoryName: stage.deletedMemoryName,
    predictedMemoryName: stage.predictedMemoryName,
    leastWantedName: stage.leastWantedName,
    predictionMatch: stage.predictionMatch,
    deletedWasLeastWanted: stage.deletedWasLeastWanted,
    q1Pain: stage.emotion.q1Pain,
    q2Understanding: stage.emotion.q2Understanding,
    quadrant: stage.emotion.quadrant,
    immediateQuit: stage.emotion.immediateQuit,
    restartIntent: stage.emotion.restartIntent,
    powerDrop: stage.powerDrop,
    recoveryRatio: stage.recoveryRatio,
    earlyFunScore: stage.earlyLoop?.earlyFunScore,
    earlyKillTempo: stage.earlyLoop?.killTempo,
    earlyCrowdPressure: stage.earlyLoop?.crowdPressure,
    earlyChoiceInterest: stage.earlyLoop?.choiceInterest,
    earlyLevelUps: stage.earlyLoop?.levelUpsBeforeBoss,
    cycleCompleted: stage.cycleCompleted,
    deficitSurvivalChance: stage.deficitSurvivalChance,
    postLossChallengeScore: stage.postLossChallenge?.score,
    postLossChallengeContrast: stage.postLossChallenge?.contrast,
    refillReached: stage.refillReached,
    echoPivotScore: stage.echoPivotScore,
    preForgetPower: stage.preForgetPower,
    postDeletePower: stage.postDeletePower,
    postReplacementPower: stage.postReplacementPower,
    replacementName: stage.replacementName,
    deletionWeights: JSON.stringify(stage.reliance?.deletionWeights || {}),
    echoPower: stage.reviewTargets?.echoPower,
    uiClarity: stage.reviewTargets?.uiClarity,
    activeMemoryNamesBefore: stage.activeMemoryNamesBefore.join(' / '),
    activeMemoryNamesAfter: stage.activeMemoryNamesAfter.join(' / '),
  };
  return map[h];
}

function csvValue(v) {
  if (v === null || v === undefined) return '';
  const s = String(v);
  if (/[",\n]/.test(s)) return `"${s.replace(/"/g, '""')}"`;
  return s;
}

module.exports = { summarize, runsToCsv, flattenStages };
