#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');

const weapons = {
  twin_blades: { id: 'twin_blades', damage: 15, interval: 0.36 },
  greatsword: { id: 'greatsword', damage: 42, interval: 1.02 },
};

const memories = {
  execution_flash: { id: 'execution_flash', cooldown: 3.15, tags: ['burst'] },
  hungry_blades: { id: 'hungry_blades', cooldown: 0, tags: ['area', 'dot'] },
  shatter_ripple: { id: 'shatter_ripple', cooldown: 4.55, tags: ['area', 'control'] },
  stopped_second: { id: 'stopped_second', cooldown: 7.25, tags: ['control', 'dot'] },
  oblivion_brand: { id: 'oblivion_brand', cooldown: 5.2, tags: ['burst', 'control'] },
};

const balance = {
  version: 'v0.12-balance-1',
  boss: { firstBossHp: 2800 },
  hungryBlades: {
    dps: 28,
    targetSoftCap: 4,
    overflowDamageMul: 0.55,
  },
};

const runGrowth = {
  level: 10,
  damage: 0.28,
  attackSpeed: 0.22,
  cooldownReduction: 0.08,
  range: 0.12,
  knockback: 0.08,
};

const echo = {
  critChance: 0.04,
  critDamage: 0.45,
  attackSpeed: 0,
  dotDamage: 0,
  cooldownReduction: 0,
  range: 0,
  knockback: 0,
  markDuration: 0,
  slowDuration: 0,
};

const options = parseArgs(process.argv.slice(2));

main();

function main() {
  if (options.dryRun) {
    console.log('LETHE boss-only TTK harness dry-run');
    console.log(`- runs: ${options.runs}`);
    console.log(`- boss HP: ${options.bossHp}`);
    console.log(`- target TTK: ${options.targetTtkMin}-${options.targetTtkMax}s`);
    console.log(`- out: ${options.outDir}`);
    return;
  }

  fs.mkdirSync(options.outDir, { recursive: true });
  const runs = [];
  for (let index = 0; index < options.runs; index += 1) {
    const run = runBossTtk(index + 1);
    runs.push(run);
    writeJson(path.join(options.outDir, `run-${String(index + 1).padStart(2, '0')}.json`), run);
    console.log(`[boss-ttk ${index + 1}/${options.runs}] ttk=${fmt(run.firstBossTtk)}s focusedDps=${fmt(run.firstBossFocusedDps)} hp=${run.bossHp}`);
  }

  const summary = summarizeRuns(runs);
  writeJson(path.join(options.outDir, 'summary.json'), summary);
  writeJson(path.join(options.outDir, 'latest.json'), summary);
  fs.mkdirSync(path.dirname(options.reportPath), { recursive: true });
  fs.writeFileSync(options.reportPath, markdownReport(summary), 'utf8');

  console.log('');
  console.log(`Verdict: ${summary.verdict}`);
  console.log(`Accepted samples: ${summary.metrics.acceptedSamples}`);
  console.log(`TTK median: ${fmt(summary.metrics.firstBossTtkMedian)}s`);
  console.log(`Focused DPS median: ${fmt(summary.metrics.focusedDpsMedian)}`);
  console.log(`Recommended first boss HP: ${summary.recommendation.recommendedHp}`);
  console.log(`Report: ${path.relative(process.cwd(), options.reportPath)}`);
}

function runBossTtk(runNumber) {
  const weapon = weapons[options.weapon] || weapons.twin_blades;
  const memoryIds = normalizeMemoryIds(options.memories);
  const state = {
    elapsed: options.spawnedAt,
    bossHp: options.bossHp,
    bossMaxHp: options.bossHp,
    bossSpawnedAt: options.spawnedAt,
    weapon,
    memoryIds,
    activeSynergyIds: activeSynergyIds(memoryIds),
    cooldowns: Object.fromEntries(memoryIds.map((id) => [id, id === 'hungry_blades' ? 0 : 0.8])),
    attackCd: 0,
    focusCooldownLeft: 0,
    focusDurationLeft: 0,
    focusedMemoryId: null,
    tacticalFocusUses: [],
    attackCount: 0,
    memoryTriggerCounts: {},
    marked: 0,
    vulnerable: 0,
    slow: 0,
    damageTotal: 0,
    damageBySource: {},
    bossDamageBySource: {},
    samples: [],
  };

  const maxSteps = Math.ceil(options.maxTtkSec / options.dt);
  for (let step = 0; step < maxSteps && state.bossHp > 0; step += 1) {
    tick(state, options.dt);
  }

  const ttk = state.bossHp <= 0 ? state.elapsed - state.bossSpawnedAt : null;
  const bossFight = {
    cycleIndex: 1,
    bossName: 'first_boss',
    spawnedAt: options.spawnedAt,
    maxHp: options.bossHp,
    defeatedAt: ttk === null ? null : round(state.elapsed),
    ttk: ttk === null ? null : round(ttk),
    damage: round(state.damageTotal),
    focusedDps: ttk === null ? null : round(state.damageTotal / Math.max(0.01, ttk)),
    damageBySource: roundMap(state.bossDamageBySource),
  };
  const dpsBySource = Object.fromEntries(
    Object.entries(state.damageBySource).map(([key, value]) => [key, round(value / Math.max(0.01, ttk || options.maxTtkSec))]),
  );
  const dpsEntries = Object.entries(dpsBySource).sort((a, b) => b[1] - a[1]);
  const totalDps = dpsEntries.reduce((acc, [, value]) => acc + value, 0);

  return {
    runNumber,
    status: ttk === null ? 'timeout' : 'complete',
    runResult: ttk === null ? 'timeout' : 'first_boss_clear',
    scenario: 'in_process_first_boss_ttk',
    deterministic: true,
    version: 'v0.12-boss-ttk-harness-1',
    balanceVersion: balance.version,
    bossHp: options.bossHp,
    weapon: weapon.id,
    memoryIds,
    activeSynergyIds: state.activeSynergyIds,
    firstBossCleared: ttk !== null,
    firstBossTtk: ttk === null ? null : round(ttk),
    firstBossFocusedDps: bossFight.focusedDps,
    elapsed: round(state.elapsed),
    level: runGrowth.level,
    levelUpsBeforeFirstBoss: 9,
    slotsFilledAt: options.spawnedAt,
    activeMemoryCount: memoryIds.length,
    topDpsSource: dpsEntries[0]?.[0] || null,
    topDps: dpsEntries[0]?.[1] || 0,
    topDpsShare: totalDps > 0 ? round(dpsEntries[0][1] / totalDps, 4) : 0,
    dpsBySource,
    damageBySource: roundMap(state.damageBySource),
    bossFights: [bossFight],
    tacticalFocus: {
      useCount: state.tacticalFocusUses.length,
      history: state.tacticalFocusUses,
    },
    debug: {
      attackCount: state.attackCount,
      memoryTriggerCounts: state.memoryTriggerCounts,
      finalAttackCd: round(state.attackCd),
      dt: options.dt,
    },
    telemetry: {
      damageTotal: round(state.damageTotal),
      dpsAverage: round(state.damageTotal / Math.max(0.01, ttk || options.maxTtkSec)),
      damageBySource: roundMap(state.damageBySource),
      dpsBySource,
      bossDamageBySource: roundMap(state.bossDamageBySource),
      bossFights: [bossFight],
      levelUpTimestamps: [],
      slotsFilledAt: options.spawnedAt,
      samples: state.samples,
    },
  };
}

function tick(state, dt) {
  state.elapsed += dt;
  state.attackCd = Math.max(0, state.attackCd - dt);
  state.focusCooldownLeft = Math.max(0, state.focusCooldownLeft - dt);
  state.focusDurationLeft = Math.max(0, state.focusDurationLeft - dt);
  state.marked = Math.max(0, state.marked - dt);
  state.vulnerable = Math.max(0, state.vulnerable - dt);
  state.slow = Math.max(0, state.slow - dt);
  if (state.focusDurationLeft <= 0) state.focusedMemoryId = null;

  if (state.focusCooldownLeft <= 0) requestTacticalFocus(state);
  updateBasicAttack(state);
  updateMemories(state, dt);
  sample(state);
}

function updateBasicAttack(state) {
  const interval = Math.max(0.16, state.weapon.interval / (1 + echo.attackSpeed + runGrowth.attackSpeed));
  if (state.attackCd > 0) return;
  state.attackCd = interval;
  const expectedCritMul = 1 + echo.critChance * echo.critDamage;
  const damage = state.weapon.damage * (1 + runGrowth.damage) * expectedCritMul;
  state.attackCount += 1;
  damageBoss(state, damage, 'weapon');
}

function updateMemories(state, dt) {
  for (const id of state.memoryIds) {
    if (id === 'hungry_blades') {
      const focusMul = state.focusedMemoryId === id ? 1.16 : 1;
      const damage = balance.hungryBlades.dps * dt * focusMul * (1 + echo.dotDamage + runGrowth.damage);
      damageBoss(state, damage, id);
      continue;
    }
    state.cooldowns[id] = Math.max(0, (state.cooldowns[id] || 0) - dt);
    if (state.cooldowns[id] > 0) continue;
    activateMemory(state, id);
    state.cooldowns[id] = memories[id].cooldown * (1 - echo.cooldownReduction - runGrowth.cooldownReduction);
  }
}

function activateMemory(state, id) {
  state.memoryTriggerCounts[id] = (state.memoryTriggerCounts[id] || 0) + 1;
  if (id === 'execution_flash') {
    const damage = 76 * (state.weapon.id === 'greatsword' ? 1.12 : 1) * (1 + runGrowth.damage);
    damageBoss(state, damage, id, { groggyBonus: true });
  }
  if (id === 'shatter_ripple') {
    damageBoss(state, 49 * (1 + runGrowth.damage), id, { pushed: true, controlHit: true });
  }
  if (id === 'stopped_second') {
    state.slow = Math.max(state.slow, 2.4);
    damageBoss(state, 24 * (1 + runGrowth.damage), id, { controlHit: true });
    for (const other of state.memoryIds) {
      if (other !== id && state.cooldowns[other] > 0) state.cooldowns[other] *= 0.88;
    }
  }
  if (id === 'oblivion_brand') {
    state.marked = Math.max(state.marked, 4.2);
    state.vulnerable = Math.max(state.vulnerable, 4.2);
    state.slow = Math.max(state.slow, 0.75);
    damageBoss(state, 32 * (1 + runGrowth.damage), id, { controlHit: true, brandHit: true });
  }
}

function damageBoss(state, amount, source, flags = {}) {
  if (state.marked > 0 || state.vulnerable > 0) amount *= 1.14;
  amount = applySynergyDamage(state, source, amount, flags);
  const before = state.bossHp;
  state.bossHp -= amount;
  const actual = Math.max(0, before - Math.max(0, state.bossHp));
  if (actual <= 0) return;
  state.damageTotal += actual;
  state.damageBySource[source] = (state.damageBySource[source] || 0) + actual;
  state.bossDamageBySource[source] = (state.bossDamageBySource[source] || 0) + actual;
}

function applySynergyDamage(state, source, amount, flags) {
  const tags = memories[source]?.tags || [];
  let next = amount;
  if (state.activeSynergyIds.includes('area_control') && tags.includes('area') && (state.slow > 0 || flags.pushed || flags.controlHit)) {
    next *= 1.22;
  }
  if (state.activeSynergyIds.includes('dot_control') && tags.includes('dot') && state.slow > 0) {
    next *= 1.28;
  }
  if (state.activeSynergyIds.includes('brand_chain') && tags.includes('burst') && (state.marked > 0 || state.vulnerable > 0 || flags.brandHit)) {
    next *= 1.26;
  }
  if (state.focusedMemoryId === source && focusMatchesActiveSynergy(state, tags)) {
    next *= 1.12;
  }
  return next;
}

function requestTacticalFocus(state) {
  const priority = ['execution_flash', 'stalker_oath', 'oblivion_brand', 'hungry_blades', 'shatter_ripple', 'stopped_second'];
  const id = priority.find((candidate) => state.memoryIds.includes(candidate));
  if (!id) return;
  const beforeCooldown = state.cooldowns[id] || 0;
  state.focusCooldownLeft = 9;
  state.focusDurationLeft = 3;
  state.focusedMemoryId = id;
  if (memories[id].cooldown > 0) {
    state.cooldowns[id] = Math.min(beforeCooldown, Math.max(0.12, memories[id].cooldown * 0.24));
  }
  state.tacticalFocusUses.push({
    t: round(state.elapsed),
    memoryId: id,
    beforeCooldown: round(beforeCooldown),
    afterCooldown: round(state.cooldowns[id] || 0),
  });
}

function sample(state) {
  if (state.samples.length && state.elapsed - state.samples[state.samples.length - 1].t < 5) return;
  const elapsed = Math.max(0.01, state.elapsed - state.bossSpawnedAt);
  state.samples.push({
    t: round(state.elapsed),
    bossHp: round(state.bossHp),
    bossHpRate: round(state.bossHp / state.bossMaxHp, 4),
    damageTotal: round(state.damageTotal),
    dpsAverage: round(state.damageTotal / elapsed),
    damageBySource: roundMap(state.damageBySource),
  });
}

function normalizeMemoryIds(value) {
  const ids = String(value || '')
    .split(',')
    .map((item) => item.trim())
    .filter(Boolean);
  const resolved = ids.length ? ids : ['hungry_blades', 'execution_flash', 'shatter_ripple'];
  return resolved.filter((id) => memories[id]).slice(0, 3);
}

function activeSynergyIds(memoryIds) {
  const tagSet = new Set();
  for (const id of memoryIds) {
    for (const tag of memories[id]?.tags || []) tagSet.add(tag);
  }
  const ids = [];
  if (tagSet.has('area') && tagSet.has('control')) ids.push('area_control');
  if (tagSet.has('dot') && tagSet.has('control')) ids.push('dot_control');
  if (tagSet.has('burst') && tagSet.has('survival')) ids.push('burst_survival');
  if (tagSet.has('burst') && tagSet.has('control')) ids.push('brand_chain');
  return ids;
}

function focusMatchesActiveSynergy(state, tags) {
  if (state.activeSynergyIds.includes('area_control') && tags.some((tag) => ['area', 'control'].includes(tag))) return true;
  if (state.activeSynergyIds.includes('dot_control') && tags.some((tag) => ['dot', 'control'].includes(tag))) return true;
  if (state.activeSynergyIds.includes('brand_chain') && tags.some((tag) => ['burst', 'control'].includes(tag))) return true;
  if (state.activeSynergyIds.includes('burst_survival') && tags.some((tag) => ['burst', 'survival'].includes(tag))) return true;
  return false;
}

function summarizeRuns(runs) {
  const accepted = runs.filter((run) => run.status === 'complete' && Number.isFinite(run.firstBossTtk));
  const ttkValues = accepted.map((run) => run.firstBossTtk);
  const dpsValues = accepted.map((run) => run.firstBossFocusedDps);
  const focusedDpsMedian = median(dpsValues);
  const targetTtk = (options.targetTtkMin + options.targetTtkMax) / 2;
  const recommendedHp = focusedDpsMedian ? Math.round(focusedDpsMedian * targetTtk) : null;
  const checks = [
    check('accepted gameplay samples', accepted.length >= options.minAcceptedSamples, accepted.length, `>= ${options.minAcceptedSamples}`),
    check('first boss TTK lower bound', median(ttkValues) >= options.targetTtkMin, median(ttkValues), `>= ${options.targetTtkMin}s`),
    check('first boss TTK upper bound', median(ttkValues) <= options.targetTtkMax, median(ttkValues), `<= ${options.targetTtkMax}s`),
  ];
  const failed = checks.filter((item) => !item.pass);
  return {
    generatedAt: new Date().toISOString(),
    version: 'v0.12-boss-ttk-harness-1',
    verdict: failed.length ? 'ITERATE_BOSS_TTK' : 'GO_BOSS_TTK_SAMPLE',
    metrics: {
      runs: runs.length,
      acceptedSamples: accepted.length,
      firstBossTtkMedian: median(ttkValues),
      firstBossTtkMean: mean(ttkValues),
      focusedDpsMedian,
      focusedDpsMean: mean(dpsValues),
    },
    targets: {
      minAcceptedSamples: options.minAcceptedSamples,
      targetTtkMin: options.targetTtkMin,
      targetTtkMax: options.targetTtkMax,
      targetTtk,
    },
    checks,
    failed,
    recommendation: {
      currentHp: options.bossHp,
      targetTtk,
      recommendedHp,
      delta: recommendedHp === null ? null : recommendedHp - options.bossHp,
      note: 'Use this as boss-only TTK input. Confirm with browser balance QA before treating it as player-facing balance proof.',
    },
    runs,
  };
}

function markdownReport(summary) {
  const lines = [
    '# LETHE v0.12 First Boss TTK Harness',
    '',
    `- Generated: ${summary.generatedAt}`,
    `- Verdict: \`${summary.verdict}\``,
    `- Current first boss HP: \`${summary.recommendation.currentHp}\``,
    `- Recommended first boss HP for ${summary.targets.targetTtk}s target: \`${summary.recommendation.recommendedHp}\``,
    '',
    '## Metrics',
    '',
    `- Accepted samples: \`${summary.metrics.acceptedSamples}/${summary.metrics.runs}\``,
    `- First boss TTK median: \`${fmt(summary.metrics.firstBossTtkMedian)}s\``,
    `- Focused DPS median: \`${fmt(summary.metrics.focusedDpsMedian)}\``,
    '',
    '## Checks',
    '',
    ...summary.checks.map((item) => `- ${item.pass ? '[x]' : '[ ]'} ${item.name}: \`${formatValue(item.value)}\` target \`${item.target}\``),
    '',
    '## Runs',
    '',
    '| run | status | ttk | focused DPS | top DPS | share |',
    '| --- | --- | ---: | ---: | --- | ---: |',
    ...summary.runs.map((run) => `| ${run.runNumber} | ${run.status} | ${fmt(run.firstBossTtk)} | ${fmt(run.firstBossFocusedDps)} | ${run.topDpsSource || '-'} | ${pct(run.topDpsShare)} |`),
    '',
    '## Interpretation',
    '',
    'This is an in-process boss-only harness that mirrors the v0.12 first-boss TTK scenario without Chrome/CDP. It is measurement input for first boss HP tuning, not a replacement for later browser balance QA.',
    '',
  ];
  return `${lines.join('\n')}\n`;
}

function parseArgs(args) {
  const date = todayString();
  return {
    dryRun: args.includes('--dry-run'),
    runs: num(valueAfter(args, '--runs'), 5),
    minAcceptedSamples: num(valueAfter(args, '--min-accepted-samples'), 3),
    dt: num(valueAfter(args, '--dt'), 1 / 60),
    maxTtkSec: num(valueAfter(args, '--max-ttk-sec'), 90),
    spawnedAt: num(valueAfter(args, '--spawned-at'), 180),
    bossHp: num(valueAfter(args, '--boss-hp'), balance.boss.firstBossHp),
    weapon: valueAfter(args, '--weapon') || 'twin_blades',
    memories: valueAfter(args, '--memories') || 'hungry_blades,execution_flash,shatter_ripple',
    targetTtkMin: num(valueAfter(args, '--target-first-boss-ttk-min'), 15),
    targetTtkMax: num(valueAfter(args, '--target-first-boss-ttk-max'), 30),
    outDir: path.resolve(valueAfter(args, '--out') || path.join('alpha_test', 'outputs', 'boss-ttk')),
    reportPath: path.resolve(valueAfter(args, '--report') || path.join('docs', 'balance', `${date}-v012-boss-ttk-harness.md`)),
  };
}

function check(name, pass, value, target) {
  return { name, pass: Boolean(pass), value, target };
}

function valueAfter(args, name) {
  const index = args.indexOf(name);
  if (index !== -1) return args[index + 1] || '';
  const prefix = `${name}=`;
  const match = args.find((arg) => arg.startsWith(prefix));
  return match ? match.slice(prefix.length) : '';
}

function num(value, fallback) {
  if (value === '' || value === undefined || value === null) return fallback;
  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : fallback;
}

function writeJson(file, value) {
  fs.writeFileSync(file, JSON.stringify(value, null, 2), 'utf8');
}

function round(value, digits = 2) {
  return Number(Number(value).toFixed(digits));
}

function roundMap(map) {
  return Object.fromEntries(
    Object.entries(map || {}).map(([key, value]) => [key, round(value)]),
  );
}

function mean(values) {
  return values.length ? values.reduce((acc, value) => acc + value, 0) / values.length : null;
}

function median(values) {
  if (!values.length) return null;
  const sorted = [...values].sort((a, b) => a - b);
  const mid = Math.floor(sorted.length / 2);
  return sorted.length % 2 ? sorted[mid] : (sorted[mid - 1] + sorted[mid]) / 2;
}

function fmt(value) {
  return Number.isFinite(value) ? Number(value.toFixed(2)) : '-';
}

function formatValue(value) {
  if (typeof value === 'number' && Number.isFinite(value)) return String(Number(value.toFixed(4)));
  return String(value ?? '-');
}

function pct(value) {
  return Number.isFinite(value) ? `${(value * 100).toFixed(1)}%` : '-';
}

function todayString() {
  const now = new Date();
  return [
    now.getFullYear(),
    String(now.getMonth() + 1).padStart(2, '0'),
    String(now.getDate()).padStart(2, '0'),
  ].join('-');
}
