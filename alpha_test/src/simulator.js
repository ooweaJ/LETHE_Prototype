'use strict';

const { WEAPONS, MEMORIES, MEMORY_FORGET_BIAS, SYNERGIES, ENCOUNTERS, SIM_DEFAULTS } = require('./config');
const { listBots } = require('./bots');
const { RNG } = require('./rng');

function clamp(value, min, max) {
  return Math.max(min, Math.min(max, value));
}

function round(value, digits = 4) {
  const p = 10 ** digits;
  return Math.round(value * p) / p;
}

function sum(obj) {
  return Object.values(obj).reduce((a, b) => a + b, 0);
}

function normalizeMap(obj) {
  const total = sum(obj);
  const out = {};
  Object.keys(obj).forEach((key) => {
    out[key] = total > 0 ? obj[key] / total : 0;
  });
  return out;
}

function pairKey(a, b) {
  return [a, b].sort().join('__');
}

const SYNERGY_MAP = new Map(SYNERGIES.map((s) => [pairKey(s.a, s.b), s]));

function getActiveSynergies(memoryIds) {
  const out = [];
  for (let i = 0; i < memoryIds.length; i += 1) {
    for (let j = i + 1; j < memoryIds.length; j += 1) {
      const syn = SYNERGY_MAP.get(pairKey(memoryIds[i], memoryIds[j]));
      if (syn) out.push(syn);
    }
  }
  return out;
}

function emptyStats() {
  return {
    critChance: 0,
    critDamage: 0,
    attackSpeed: 0,
    dotPower: 0,
    projectileCount: 0,
    homing: 0,
    area: 0,
    knockback: 0,
    survival: 0,
    extraHitChance: 0,
    onHitDamage: 0,
    cooldownReduction: 0,
    slowDuration: 0,
  };
}

function addStats(a, b, scale = 1) {
  const out = { ...a };
  Object.keys(b || {}).forEach((key) => {
    out[key] = (out[key] || 0) + b[key] * scale;
  });
  return out;
}

function totalEchoStats(echoes, echoPower = 0.6) {
  let stats = emptyStats();
  for (const echo of echoes) {
    const memory = MEMORIES[echo.memoryId];
    if (memory) stats = addStats(stats, memory.echo, echoPower * (echo.stacks || 1));
  }
  return stats;
}

function weaponScalar(weapon, stats) {
  const critTerm = 1 + (weapon.critBias + stats.critChance) * (0.8 + stats.critDamage);
  const speedTerm = 1 + stats.attackSpeed * weapon.affinities.attackSpeed;
  const extraHitTerm = 1 + (stats.extraHitChance + stats.onHitDamage * 0.45) * weapon.affinities.extraHit;
  const areaTerm = 1 + (weapon.areaBias + stats.area * 0.75) * 0.32;
  const cooldownTerm = 1 + stats.cooldownReduction * 0.38;
  return Math.max(0.45, critTerm * speedTerm * extraHitTerm * areaTerm * cooldownTerm);
}

function memoryEchoScalar(memory, weapon, stats) {
  const aff = weapon.affinities;
  let scalar = 1;
  scalar += memory.burst * ((stats.critChance * 1.10 + stats.critDamage * 0.52) * aff.burst);
  scalar += memory.dot * ((stats.dotPower * 0.90 + stats.attackSpeed * 0.30) * aff.dot);
  scalar += memory.projectile * ((stats.projectileCount * 1.00 + stats.homing * 0.56) * aff.projectile);
  scalar += memory.area * ((stats.area * 0.85 + stats.knockback * 0.30) * aff.area);
  scalar += memory.control * ((stats.cooldownReduction * 0.70 + stats.slowDuration * 0.44) * aff.control);
  scalar += memory.onhit * ((stats.extraHitChance * 1.05 + stats.onHitDamage * 0.54 + stats.attackSpeed * 0.22) * aff.extraHit);
  return Math.max(0.35, scalar);
}

function memoryEncounterScalar(memory, encounter) {
  if (encounter.phases) {
    return encounter.phases.reduce((acc, phase) => acc + phase.weight * rolePreference(memory, phase.preferred), 0);
  }
  return rolePreference(memory, encounter.preferred);
}

function rolePreference(memory, preferred) {
  return 0.65
    + memory.burst * (preferred.burst || 0)
    + memory.dot * (preferred.dot || 0)
    + memory.projectile * (preferred.projectile || 0)
    + memory.area * (preferred.area || 0)
    + memory.control * (preferred.control || 0)
    + memory.onhit * (preferred.onhit || 0);
}

function activeSynergyBonus(memoryId, activeMemoryIds) {
  return getActiveSynergies(activeMemoryIds)
    .filter((s) => s.a === memoryId || s.b === memoryId)
    .reduce((acc, s) => acc + s.bonus, 0);
}

function calculateContributions(state, encounterName) {
  const encounter = ENCOUNTERS[encounterName];
  const weapon = WEAPONS[state.weaponId];
  const stats = totalEchoStats(state.echoes, state.echoPower);
  const active = state.activeMemories;
  const contributions = {};
  let totalActive = 0;

  for (const id of active) {
    const memory = MEMORIES[id];
    const echoScalar = memoryEchoScalar(memory, weapon, stats);
    const encounterScalar = memoryEncounterScalar(memory, encounter);
    const bossScalar = encounter.boss ? memory.bossBias : 1;
    const crowdScalar = 1 + (encounter.crowd - 0.5) * (memory.crowdBias - 1);
    const synergy = 1 + activeSynergyBonus(id, active);
    const botUse = state.bot.concentrationPreference > 0
      ? 1 + state.bot.concentrationPreference * 0.11 * (memory.baseDps / 18)
      : 1 + state.bot.concentrationPreference * 0.06 * (memory.baseDps / 18);
    const val = memory.baseDps * echoScalar * encounterScalar * bossScalar * crowdScalar * synergy * botUse;
    contributions[id] = Math.max(0, val);
    totalActive += contributions[id];
  }

  const weaponDps = weapon.baseDps * weaponScalar(weapon, stats);
  const synergyCount = getActiveSynergies(active).length;
  const survival = clamp(weapon.survival + stats.survival + stats.slowDuration * 0.08 + stats.knockback * 0.04 - encounter.danger * 0.10 + state.bot.skill * 0.10, 0.40, 1.25);
  const effectivePower = (weaponDps + totalActive) * (0.82 + survival * 0.20) * (1 + synergyCount * 0.018);

  return {
    encounter: encounterName,
    weaponDps: round(weaponDps, 3),
    activeDps: round(totalActive, 3),
    effectivePower: round(effectivePower, 3),
    survival: round(survival, 3),
    synergyCount,
    contributions,
    shares: normalizeMap(contributions),
  };
}

function calculateStageSnapshot(state) {
  const room = calculateContributions(state, 'room');
  const elite = calculateContributions(state, 'elite');
  const boss = calculateContributions(state, 'boss');
  const total = {};
  const duration = ENCOUNTERS.room.durationSec + ENCOUNTERS.elite.durationSec + ENCOUNTERS.boss.durationSec;
  for (const id of state.activeMemories) {
    total[id] = (room.contributions[id] || 0) * ENCOUNTERS.room.durationSec
      + (elite.contributions[id] || 0) * ENCOUNTERS.elite.durationSec
      + (boss.contributions[id] || 0) * ENCOUNTERS.boss.durationSec;
  }
  return {
    room,
    elite,
    boss,
    total,
    totalShares: normalizeMap(total),
    durationSec: duration,
    preForgetPower: boss.effectivePower,
  };
}

function calculatePower(state) {
  const room = calculateContributions(state, 'room');
  const boss = calculateContributions(state, 'boss');
  return round(room.effectivePower * 0.42 + boss.effectivePower * 0.58, 3);
}

function estimateIrreplaceability(state, snapshot) {
  const base = calculatePower(state);
  const out = {};
  for (const id of state.activeMemories) {
    const without = { ...state, activeMemories: state.activeMemories.filter((m) => m !== id) };
    const powerWithout = calculatePower(without);
    const drop = base > 0 ? clamp((base - powerWithout) / base, 0, 1) : 0;
    const synergyCentrality = activeSynergyBonus(id, state.activeMemories);
    const share = snapshot.totalShares[id] || 0;
    out[id] = clamp(drop * 0.70 + share * 0.20 + synergyCentrality * 0.40, 0, 1);
  }
  return normalizeMap(out);
}

function computeReliance(state, snapshot) {
  const combat = snapshot.totalShares;
  const boss = snapshot.boss.shares;
  const irreplaceability = estimateIrreplaceability(state, snapshot);
  const presenceRaw = {};
  for (const id of state.activeMemories) {
    const memory = MEMORIES[id];
    const focus = state.bot.concentrationPreference > 0 ? (combat[id] || 0) * state.bot.concentrationPreference * 0.22 : 0;
    presenceRaw[id] = memory.presence + focus + activeSynergyBonus(id, state.activeMemories) * 0.35;
  }
  const presence = normalizeMap(presenceRaw);

  const score = {};
  for (const id of state.activeMemories) {
    score[id] = 0.40 * (combat[id] || 0)
      + 0.25 * (boss[id] || 0)
      + 0.20 * (irreplaceability[id] || 0)
      + 0.15 * (presence[id] || 0);
  }
  const normalized = normalizeMap(score);
  const deletionRaw = {};
  for (const id of state.activeMemories) {
    deletionRaw[id] = (normalized[id] || 0) * (MEMORY_FORGET_BIAS[id] ?? 1);
  }
  const deletionWeights = normalizeMap(deletionRaw);
  const deletedMemoryId = Object.entries(deletionWeights).sort((a, b) => b[1] - a[1])[0][0];
  const sorted = Object.entries(normalized).sort((a, b) => b[1] - a[1]);
  return {
    combat,
    boss,
    irreplaceability,
    presence,
    score: normalized,
    deletionRaw,
    deletionWeights,
    deletedMemoryId,
    clarityGap: sorted.length > 1 ? round(sorted[0][1] - sorted[1][1], 4) : 1,
  };
}

function classifyBuild(state, snapshot) {
  const shares = Object.values(snapshot.totalShares).sort((a, b) => b - a);
  const concentrationIndex = shares[0] || 0;
  const activeSynergies = getActiveSynergies(state.activeMemories);
  const maxPairs = Math.max(1, (state.activeMemories.length * (state.activeMemories.length - 1)) / 2);
  const synergyConnectivity = activeSynergies.length / maxPairs;
  let buildClass = '분산-느슨';
  if (concentrationIndex >= 0.50) buildClass = '몰빵';
  else if (synergyConnectivity >= 0.45) buildClass = '분산-거미줄';
  return {
    buildClass,
    concentrationIndex: round(concentrationIndex, 4),
    synergyConnectivity: round(synergyConnectivity, 4),
    activeSynergies: activeSynergies.map((s) => s.name),
  };
}

function chooseWeapon(rng, bot) {
  const ids = Object.keys(WEAPONS);
  return rng.weighted(ids, (id) => bot.weaponWeights[id] || 1);
}

function expectedMemoryScore(memoryId, state, candidateSet = null) {
  const memory = MEMORIES[memoryId];
  const weapon = WEAPONS[state.weaponId];
  const stats = totalEchoStats(state.echoes, state.echoPower);
  const active = candidateSet || state.activeMemories;
  const base = memory.baseDps * memoryEchoScalar(memory, weapon, stats) * (1 + activeSynergyBonus(memoryId, active) * 1.2);
  const botWeight = state.bot.memoryWeights[memoryId] || 1;
  const echoAffinity = memory.echoTags.reduce((acc, tag) => {
    if (tag === 'crit') return acc + (stats.critChance + stats.critDamage * 0.45) * 1.6;
    if (tag === 'attackSpeed') return acc + stats.attackSpeed * 1.4;
    if (tag === 'dot') return acc + stats.dotPower * 1.2;
    if (tag === 'projectile') return acc + (stats.projectileCount + stats.homing * 0.4) * 1.2;
    if (tag === 'area') return acc + (stats.area + stats.knockback * 0.25) * 1.1;
    if (tag === 'survival') return acc + stats.survival * 1.8;
    if (tag === 'control') return acc + (stats.cooldownReduction + stats.slowDuration * 0.35) * 1.1;
    if (tag === 'extraHit') return acc + (stats.extraHitChance + stats.onHitDamage * 0.35) * 1.3;
    return acc;
  }, 0);
  const optimizer = state.bot.id === 'optimizer' ? 1.20 : 1.0;
  return base * botWeight * optimizer * (1 + echoAffinity);
}

function chooseInitialMemories(rng, bot, weaponId) {
  const state = { weaponId, activeMemories: [], echoes: [], echoPower: SIM_DEFAULTS.echoPower, bot };
  const pool = Object.keys(MEMORIES);
  while (state.activeMemories.length < 3) {
    const candidates = pool.filter((id) => !state.activeMemories.includes(id));
    const chosen = rng.weighted(candidates, (id) => {
      const candidateSet = state.activeMemories.concat(id);
      const score = expectedMemoryScore(id, { ...state, activeMemories: candidateSet }, candidateSet);
      const randomizer = bot.id === 'random' ? rng.float(0.75, 1.25) : rng.float(0.90, 1.10);
      const diversityPenalty = bot.concentrationPreference < 0 ? (1 - activeSynergyBonus(id, candidateSet) * 0.10) : 1;
      return score * randomizer * diversityPenalty;
    });
    state.activeMemories.push(chosen);
  }
  return state.activeMemories;
}

function chooseReplacement(rng, state, forgottenIds) {
  const candidates = Object.keys(MEMORIES).filter((id) => !state.activeMemories.includes(id) && !forgottenIds.includes(id));
  if (!candidates.length) return null;
  return rng.weighted(candidates, (id) => {
    const candidateSet = state.activeMemories.concat(id);
    const score = expectedMemoryScore(id, { ...state, activeMemories: candidateSet }, candidateSet);
    const novelty = 1 + state.bot.novelty * rng.float(-0.05, 0.18);
    const pivotBonus = state.bot.id === 'echo_pivot' ? 1.15 : 1.0;
    return score * novelty * pivotBonus;
  });
}

function predictDeletion(rng, state, reliance, options) {
  const visible = {};
  for (const id of state.activeMemories) {
    visible[id] = 0.50 * (reliance.boss[id] || 0)
      + 0.34 * (reliance.combat[id] || 0)
      + 0.16 * (reliance.presence[id] || 0);
    // UI clarity exposes part of the deletion weight without making prediction a pure answer key.
    const visibleTruth = reliance.deletionWeights[id] || reliance.score[id] || 0;
    const clarity = Math.min(0.88, options.uiClarity * 1.16);
    visible[id] = visible[id] * (1 - clarity) + visibleTruth * clarity;
    visible[id] += rng.noise((1 - state.bot.perception) * 0.045 + 0.006);
  }
  const sortedVisible = Object.entries(visible).sort((a, b) => b[1] - a[1]);
  return sortedVisible[0][0];
}

function chooseLeastWantedToLose(rng, state, snapshot, reliance) {
  const score = {};
  for (const id of state.activeMemories) {
    const memory = MEMORIES[id];
    const botTaste = state.bot.memoryWeights[id] || 1;
    const strength = (snapshot.totalShares[id] || 0) * 0.52 + (reliance.score[id] || 0) * 0.28 + activeSynergyBonus(id, state.activeMemories) * 0.18;
    score[id] = strength * (0.80 + state.bot.attachmentBias * 0.45) * botTaste + memory.presence * 0.05 + rng.noise(0.025);
  }
  return Object.entries(score).sort((a, b) => b[1] - a[1])[0][0];
}

function simulateEmotionProxy(rng, state, snapshot, reliance, deletedMemoryId, predictedMemoryId, leastWantedId, powerDrop, recoveryRatio, options) {
  if (!options.enableHumanEmotionProxy) {
    return {
      q1Pain: null,
      q2Understanding: null,
      quadrant: 'not_measured',
      immediateQuit: false,
      restartIntent: false,
      note: 'Human emotion proxy disabled.',
    };
  }
  const share = snapshot.totalShares[deletedMemoryId] || 0;
  const isLeastWanted = leastWantedId === deletedMemoryId;
  const predicted = predictedMemoryId === deletedMemoryId;
  const painRaw = 0.70 + share * 5.10 + (isLeastWanted ? 0.95 : 0) + state.bot.attachmentBias * 0.48 + powerDrop * 0.95 + rng.noise(0.34);
  const q1Pain = clamp(Math.round(painRaw), 0, 4);

  const hiddenSurprise = Math.max(0, (reliance.irreplaceability[deletedMemoryId] || 0) - ((reliance.combat[deletedMemoryId] || 0) + (reliance.boss[deletedMemoryId] || 0)) / 2);
  const understandingRaw = 0.82
    + (predicted ? 1.95 : 0.15)
    + reliance.clarityGap * 3.20
    + options.uiClarity * 0.55
    + state.bot.perception * 0.52
    - hiddenSurprise * 0.95
    + rng.noise(0.38);
  const q2Understanding = clamp(Math.round(understandingRaw), 0, 4);

  let quadrant = '혼란/노이즈';
  if (q1Pain >= 3 && q2Understanding >= 3) quadrant = '아쉬움';
  else if (q1Pain >= 3 && q2Understanding <= 1) quadrant = '짜증';
  else if (q1Pain <= 1) quadrant = '무덤덤';
  else if (q1Pain >= 3 && q2Understanding === 2) quadrant = '불안정한 아쉬움';
  else if (q1Pain === 2 && q2Understanding >= 3) quadrant = '납득했지만 약함';

  const quitP = clamp(
    0.035
      + (quadrant === '짜증' ? 0.31 : 0)
      + (quadrant === '혼란/노이즈' ? 0.16 : 0)
      + Math.max(0, powerDrop - 0.48) * 0.52
      - Math.max(0, recoveryRatio - 0.85) * 0.16
      - state.bot.persistence * 0.05,
    0.01,
    0.82,
  );
  const immediateQuit = rng.bool(quitP);
  const restartP = clamp(
    0.42
      + (quadrant === '아쉬움' ? 0.22 : 0)
      + (quadrant === '불안정한 아쉬움' ? 0.11 : 0)
      + state.bot.persistence * 0.22
      + state.bot.novelty * 0.08
      + (recoveryRatio >= 0.85 && recoveryRatio <= 1.12 ? 0.10 : 0)
      - (immediateQuit ? 0.32 : 0)
      - (quadrant === '짜증' ? 0.18 : 0)
      - (quadrant === '무덤덤' ? 0.12 : 0),
    0.02,
    0.95,
  );
  const restartIntent = rng.bool(restartP);

  return { q1Pain, q2Understanding, quadrant, immediateQuit, restartIntent, quitP: round(quitP, 3), restartP: round(restartP, 3) };
}

function bossClearCheck(rng, state, stageIndex, power) {
  const difficulty = 108 + stageIndex * 18;
  const margin = (power - difficulty) / difficulty;
  const p = clamp(0.66 + margin * 0.95 + state.bot.skill * 0.18 - state.bot.risk * 0.04, 0.08, 0.98);
  return { clear: rng.bool(p), clearChance: round(p, 3), difficulty };
}

function applyEcho(state, memoryId) {
  const existing = state.echoes.find((e) => e.memoryId === memoryId);
  if (existing) existing.stacks += 1;
  else state.echoes.push({ memoryId, stacks: 1 });
}

function memoryNames(ids) {
  return ids.map((id) => MEMORIES[id]?.name || id);
}

function simulateOneRun(rng, bot, options, runIndex = 0) {
  const weaponId = chooseWeapon(rng, bot);
  const state = {
    runIndex,
    bot,
    weaponId,
    activeMemories: chooseInitialMemories(rng, bot, weaponId),
    echoes: [],
    echoPower: options.echoPower,
  };
  const forgottenIds = [];
  const stageLogs = [];
  let totalTimeSec = 0;
  let failed = false;
  let failureStage = null;

  for (let stageIndex = 1; stageIndex <= options.stages; stageIndex += 1) {
    const snapshot = calculateStageSnapshot(state);
    totalTimeSec += snapshot.durationSec;
    const bossPower = snapshot.boss.effectivePower;
    const clearCheck = bossClearCheck(rng, state, stageIndex, bossPower);
    const build = classifyBuild(state, snapshot);
    const reliance = computeReliance(state, snapshot);

    if (!clearCheck.clear) {
      failed = true;
      failureStage = stageIndex;
      stageLogs.push({
        stageIndex,
        failedBeforeForget: true,
        clearChance: clearCheck.clearChance,
        difficulty: clearCheck.difficulty,
        weaponId: state.weaponId,
        weaponName: WEAPONS[state.weaponId].name,
        activeMemories: state.activeMemories.slice(),
        activeMemoryNames: memoryNames(state.activeMemories),
        build,
        snapshot: minimalSnapshot(snapshot),
      });
      break;
    }

    const deletedMemoryId = reliance.deletedMemoryId;
    const predictedMemoryId = predictDeletion(rng, state, reliance, options);
    const leastWantedId = chooseLeastWantedToLose(rng, state, snapshot, reliance);

    const preForgetPower = calculatePower(state);
    const preActive = state.activeMemories.slice();
    const preEchoes = state.echoes.map((e) => ({ ...e }));

    // Delete active memory and add echo.
    state.activeMemories = state.activeMemories.filter((id) => id !== deletedMemoryId);
    applyEcho(state, deletedMemoryId);
    forgottenIds.push(deletedMemoryId);
    const postDeletePower = calculatePower(state);
    const powerDrop = preForgetPower > 0 ? clamp((preForgetPower - postDeletePower) / preForgetPower, -1, 1) : 0;

    let replacementId = null;
    if (options.replacementOffer && state.activeMemories.length < 3) {
      replacementId = chooseReplacement(rng, state, forgottenIds);
      if (replacementId) state.activeMemories.push(replacementId);
    }
    const postReplacementPower = calculatePower(state);
    const recoveryRatio = preForgetPower > 0 ? postReplacementPower / preForgetPower : 1;
    const emotion = simulateEmotionProxy(rng, { ...state, activeMemories: preActive, echoes: preEchoes }, snapshot, reliance, deletedMemoryId, predictedMemoryId, leastWantedId, powerDrop, recoveryRatio, options);

    stageLogs.push({
      stageIndex,
      failedBeforeForget: false,
      clearChance: clearCheck.clearChance,
      difficulty: clearCheck.difficulty,
      weaponId: state.weaponId,
      weaponName: WEAPONS[state.weaponId].name,
      activeMemoriesBefore: preActive,
      activeMemoryNamesBefore: memoryNames(preActive),
      forgottenBefore: forgottenIds.slice(0, -1),
      echoesBefore: preEchoes,
      build,
      snapshot: minimalSnapshot(snapshot),
      reliance: minimalReliance(reliance),
      reviewTargets: {
        echoPower: options.echoPower,
        uiClarity: options.uiClarity,
        memoryNameRecallCheck: true,
      },
      leastWantedId,
      leastWantedName: MEMORIES[leastWantedId].name,
      predictedMemoryId,
      predictedMemoryName: MEMORIES[predictedMemoryId].name,
      deletedMemoryId,
      deletedMemoryName: MEMORIES[deletedMemoryId].name,
      predictionMatch: predictedMemoryId === deletedMemoryId,
      deletedWasLeastWanted: leastWantedId === deletedMemoryId,
      preForgetPower: round(preForgetPower, 3),
      postDeletePower: round(postDeletePower, 3),
      postReplacementPower: round(postReplacementPower, 3),
      powerDrop: round(powerDrop, 4),
      recoveryRatio: round(recoveryRatio, 4),
      echoAdded: MEMORIES[deletedMemoryId].echo,
      replacementId,
      replacementName: replacementId ? MEMORIES[replacementId].name : null,
      activeMemoriesAfter: state.activeMemories.slice(),
      activeMemoryNamesAfter: memoryNames(state.activeMemories),
      echoesAfter: state.echoes.map((e) => ({ ...e })),
      emotion,
      stageTimeSec: snapshot.durationSec,
    });
  }

  const restartIntent = stageLogs.some((s) => s.emotion?.restartIntent) || (!failed && rng.bool(0.48 + bot.persistence * 0.24));
  return {
    runIndex,
    botId: bot.id,
    botName: bot.name,
    weaponId,
    weaponName: WEAPONS[weaponId].name,
    initialMemories: stageLogs[0]?.activeMemoriesBefore || stageLogs[0]?.activeMemories || state.activeMemories,
    initialMemoryNames: memoryNames(stageLogs[0]?.activeMemoriesBefore || stageLogs[0]?.activeMemories || state.activeMemories),
    stagesCompleted: stageLogs.filter((s) => !s.failedBeforeForget).length,
    failed,
    failureStage,
    totalTimeSec,
    forgottenIds,
    forgottenNames: memoryNames(forgottenIds),
    finalActiveMemories: state.activeMemories.slice(),
    finalActiveMemoryNames: memoryNames(state.activeMemories),
    finalEchoes: state.echoes.map((e) => ({ ...e })),
    restartIntent,
    stageLogs,
  };
}

function minimalSnapshot(snapshot) {
  return {
    durationSec: snapshot.durationSec,
    preForgetPower: snapshot.preForgetPower,
    totalShares: mapRound(snapshot.totalShares),
    bossShares: mapRound(snapshot.boss.shares),
    roomPower: snapshot.room.effectivePower,
    bossPower: snapshot.boss.effectivePower,
    survival: snapshot.boss.survival,
  };
}

function minimalReliance(reliance) {
  return {
    score: mapRound(reliance.score),
    deletionWeights: mapRound(reliance.deletionWeights),
    combat: mapRound(reliance.combat),
    boss: mapRound(reliance.boss),
    irreplaceability: mapRound(reliance.irreplaceability),
    presence: mapRound(reliance.presence),
    clarityGap: reliance.clarityGap,
  };
}

function mapRound(obj) {
  const out = {};
  Object.keys(obj || {}).forEach((k) => {
    out[k] = round(obj[k], 4);
  });
  return out;
}

function simulateBatch(options = {}) {
  const opt = { ...SIM_DEFAULTS, ...options };
  const rng = new RNG(opt.seed);
  const selectedBots = opt.botIds?.length
    ? listBots().filter((b) => opt.botIds.includes(b.id))
    : listBots();
  if (!selectedBots.length) throw new Error('No bot profiles selected.');
  const runs = [];
  for (let i = 0; i < opt.runs; i += 1) {
    const bot = selectedBots[i % selectedBots.length];
    const runRng = new RNG(`${opt.seed}:${i}:${bot.id}`);
    runs.push(simulateOneRun(runRng, bot, opt, i));
  }
  return { options: opt, runs };
}

module.exports = {
  simulateBatch,
  simulateOneRun,
  calculateContributions,
  calculatePower,
  computeReliance,
  classifyBuild,
  totalEchoStats,
  clamp,
  round,
};
