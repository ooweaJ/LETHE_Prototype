#!/usr/bin/env node

const routes = [
  { id: "blood_storm", label: "피의 칼폭풍", pair: ["Hungry", "Blood"], support: ["Stopped", "Ashen"] },
  { id: "fracture", label: "파쇄 처형", pair: ["Shatter", "Execution"], support: ["Hunter", "Ashen"] },
  { id: "stasis", label: "정지 추적", pair: ["Stopped", "Hunter"], support: ["Shatter", "Blood"] },
  { id: "ashen", label: "잿빛 망각", pair: ["Ashen", "Oblivion"], support: ["Stopped", "Blood"] },
];

const weapons = [
  { id: "dual", label: "절단쌍검", killMul: 1.06, bossMul: 0.96 },
  { id: "greatsword", label: "장송대검", killMul: 0.92, bossMul: 1.18 },
];

const candidates = [
  {
    id: "current_10m_fast",
    runHardCap: 600,
    bossSchedule: [135, 285, 435, 600],
    xpMulA: 2.15,
    xpMulATo: 120,
    xpMulB: 1.95,
    xpMulBTo: 180,
    earlyKillBonus: 1,
    initialNextXp: 5,
    firstBossHp: 1750,
    bossHp: [1750, 560, 661, 762],
  },
  {
    id: "20m_slow_start",
    runHardCap: 1260,
    bossSchedule: [300, 600, 900, 1140],
    xpMulA: 1.00,
    xpMulATo: 120,
    xpMulB: 1.34,
    xpMulBTo: 600,
    earlyKillBonus: 0,
    initialNextXp: 7,
    firstBossHp: 1900,
    bossHp: [1900, 2800, 4000, 5400],
  },
  {
    id: "20m_target",
    runHardCap: 1260,
    bossSchedule: [300, 600, 900, 1140],
    xpMulA: 1.06,
    xpMulATo: 120,
    xpMulB: 1.42,
    xpMulBTo: 600,
    earlyKillBonus: 0,
    initialNextXp: 7,
    firstBossHp: 2050,
    bossHp: [2050, 3000, 4300, 5800],
  },
  {
    id: "20m_generous",
    runHardCap: 1260,
    bossSchedule: [300, 600, 900, 1140],
    xpMulA: 1.12,
    xpMulATo: 120,
    xpMulB: 1.55,
    xpMulBTo: 600,
    earlyKillBonus: 0,
    initialNextXp: 7,
    firstBossHp: 2150,
    bossHp: [2150, 3300, 4700, 6400],
  },
];

function rng(seed) {
  let s = seed >>> 0;
  return () => {
    s = (s * 1664525 + 1013904223) >>> 0;
    return s / 0x100000000;
  };
}

function nextXpAfter(level, current) {
  return Math.round(current * (level < 10 ? 1.24 : 1.42) + (level < 10 ? 3 : 4));
}

function xpMultiplier(config, t) {
  if (t < config.xpMulATo) return config.xpMulA;
  if (t < config.xpMulBTo) return config.xpMulB;
  return 1;
}

function pressureAt(t, config) {
  const idx = config.bossSchedule.findIndex((b) => t < b);
  const bossIndex = idx === -1 ? config.bossSchedule.length - 1 : idx;
  const prev = bossIndex <= 0 ? 0 : config.bossSchedule[bossIndex - 1];
  const next = config.bossSchedule[bossIndex] ?? config.runHardCap;
  const p = Math.max(0, Math.min(1, (t - prev) / Math.max(1, next - prev)));
  if (p < 0.24) return 0.78;
  if (p < 0.7) return 1.0;
  if (bossIndex === 0 && p > 0.94) return 0.68;
  return bossIndex === 0 ? 0.92 : 1.26;
}

function chooseMemory(state, route, random) {
  state.choices += 1;
  const allRoute = [...route.pair, ...route.support];
  const unfinishedPair = route.pair.filter((id) => (state.echo[id] ?? 0) < 5);
  const pairFocus = unfinishedPair.length > 0 ? unfinishedPair : route.pair;
  for (const id of pairFocus) {
    if (!state.mem[id]) {
      state.mem[id] = 1;
      return;
    }
  }

  const routeFocusChance = state.choices < 11 ? 0.88 : 0.72;
  const focusPool = random() < routeFocusChance ? pairFocus : allRoute;
  const upgrade = focusPool
    .filter((id) => (state.mem[id] ?? 0) > 0 && state.mem[id] < 5)
    .sort((a, b) => {
      const levelDiff = state.mem[b] - state.mem[a];
      if (levelDiff !== 0) return levelDiff;
      return route.pair.indexOf(a) - route.pair.indexOf(b);
    })[0];
  if (upgrade) {
    state.mem[upgrade] += 1;
    return;
  }

  const missing = allRoute.find((id) => !state.mem[id]);
  if (missing) {
    state.mem[missing] = 1;
    return;
  }

  const anyUpgrade = Object.keys(state.mem)
    .filter((id) => state.mem[id] > 0 && state.mem[id] < 5)
    .sort((a, b) => state.mem[b] - state.mem[a])[0];
  if (anyUpgrade) state.mem[anyUpgrade] += 1;
}

function forgetHighest(state, route) {
  const ids = Object.keys(state.mem).filter((id) => state.mem[id] > 0);
  ids.sort((a, b) => {
    const diff = state.mem[b] - state.mem[a];
    if (diff !== 0) return diff;
    return route.pair.indexOf(a) - route.pair.indexOf(b);
  });
  const id = ids[0];
  if (!id) return null;
  state.echo[id] = Math.min(5, (state.echo[id] ?? 0) + state.mem[id]);
  state.mem[id] = 0;
  return id;
}

function hasUltimate(state, route) {
  return route.pair.every((id) => (state.echo[id] ?? 0) >= 5);
}

function totalLevels(obj) {
  return Object.values(obj).reduce((sum, value) => sum + value, 0);
}

function simulate(config, route, weapon, seed) {
  const random = rng(seed);
  const state = {
    t: 0,
    xp: 0,
    nextXp: config.initialNextXp ?? 7,
    level: 1,
    choices: 0,
    kills: 0,
    mem: {},
    echo: {},
    ultimateAt: null,
    firstChoiceAt: null,
    firstForgetAt: null,
    clearAt: null,
    deathAt: null,
    gateKills: 0,
    samples: {},
  };

  const baseKillRate = 0.27 * weapon.killMul * (0.92 + random() * 0.20);
  const baseBossDps = 46 * weapon.bossMul * (0.90 + random() * 0.22);
  let bossRemaining = null;
  let bossIndex = 0;

  for (let t = 1; t <= config.runHardCap; t++) {
    state.t = t;
    const buildPower = 1 + totalLevels(state.mem) * 0.075 + totalLevels(state.echo) * 0.085 + (state.ultimateAt ? 0.34 : 0);
    const killRate = baseKillRate * pressureAt(t, config) * buildPower;
    const averageXp = 1.28 + (t > 300 ? 0.18 : 0) + (t > 720 ? 0.16 : 0) + (t < 120 ? config.earlyKillBonus : 0);
    state.kills += killRate;
    state.xp += killRate * averageXp * xpMultiplier(config, t);

    while (state.xp >= state.nextXp) {
      state.xp -= state.nextXp;
      state.level += 1;
      state.nextXp = nextXpAfter(state.level, state.nextXp);
      chooseMemory(state, route, random);
      if (!state.firstChoiceAt) state.firstChoiceAt = t;
    }

    if (bossIndex < config.bossSchedule.length && t === config.bossSchedule[bossIndex]) {
      bossRemaining = config.bossHp[bossIndex] ?? config.bossHp[config.bossHp.length - 1];
    }

    if (bossRemaining != null) {
      const bossPower = 1 + totalLevels(state.mem) * 0.08 + totalLevels(state.echo) * 0.10 + (state.ultimateAt ? 0.46 : 0);
      bossRemaining -= baseBossDps * bossPower;
      if (bossRemaining <= 0) {
        state.gateKills += 1;
        bossIndex += 1;
        if (!state.firstForgetAt) state.firstForgetAt = t;
        forgetHighest(state, route);
        if (!state.ultimateAt && hasUltimate(state, route)) state.ultimateAt = t;
        if (state.gateKills >= config.bossSchedule.length && state.ultimateAt) {
          state.clearAt = t;
          break;
        }
        bossRemaining = null;
      }
    }

    if ([60, 120, 180, 300, 600, 900, 1140, 1200].includes(t)) {
      state.samples[t] = {
        level: state.level,
        choices: state.choices,
        mem: { ...state.mem },
        echo: { ...state.echo },
        ultimateAt: state.ultimateAt,
        gateKills: state.gateKills,
      };
    }
  }

  if (!state.clearAt) {
    state.deathAt = config.runHardCap;
  }
  return state;
}

function summarize(config, route, weapon) {
  const runs = [];
  for (let i = 0; i < 40; i++) runs.push(simulate(config, route, weapon, 1000 + i * 97 + route.id.length * 17 + weapon.id.length));
  const cleared = runs.filter((run) => run.clearAt);
  const avg = (values) => values.length ? values.reduce((a, b) => a + b, 0) / values.length : null;
  return {
    config: config.id,
    route: route.id,
    weapon: weapon.id,
    clearRate: cleared.length / runs.length,
    avgClear: avg(cleared.map((run) => run.clearAt)),
    avgUltimate: avg(runs.filter((run) => run.ultimateAt).map((run) => run.ultimateAt)),
    avgFirstChoice: avg(runs.map((run) => run.firstChoiceAt).filter(Boolean)),
    avgFirstForget: avg(runs.map((run) => run.firstForgetAt).filter(Boolean)),
    avgLevel120: avg(runs.map((run) => run.samples[120]?.level ?? run.level)),
    avgLevel300: avg(runs.map((run) => run.samples[300]?.level ?? run.level)),
    avgLevel600: avg(runs.map((run) => run.samples[600]?.level ?? run.level)),
  };
}

function score(row) {
  const clearTarget = row.avgClear == null ? 999 : Math.abs(row.avgClear - 1200);
  const ultTarget = row.avgUltimate == null ? 999 : Math.abs(row.avgUltimate - 840);
  const choiceTarget = row.avgFirstChoice == null ? 999 : Math.abs(row.avgFirstChoice - 28);
  const firstForgetTarget = row.avgFirstForget == null ? 999 : Math.abs(row.avgFirstForget - 330);
  const clearRatePenalty = Math.abs(row.clearRate - 0.68) * 260;
  return clearTarget * 0.55 + ultTarget * 0.20 + choiceTarget * 1.0 + firstForgetTarget * 0.18 + clearRatePenalty;
}

const rows = [];
for (const config of candidates) {
  for (const route of routes) {
    for (const weapon of weapons) rows.push(summarize(config, route, weapon));
  }
}

const byConfig = candidates.map((config) => {
  const subset = rows.filter((row) => row.config === config.id);
  return {
    config: config.id,
    avgScore: scoreAverage(subset),
    avgClearRate: average(subset.map((row) => row.clearRate)),
    avgClear: average(subset.map((row) => row.avgClear).filter(Boolean)),
    avgUltimate: average(subset.map((row) => row.avgUltimate).filter(Boolean)),
    avgLevel120: average(subset.map((row) => row.avgLevel120)),
    avgLevel600: average(subset.map((row) => row.avgLevel600)),
  };
});

function average(values) {
  return values.length ? values.reduce((a, b) => a + b, 0) / values.length : null;
}

function scoreAverage(values) {
  return average(values.map(score));
}

console.log("# Config Summary");
console.table(byConfig.map((row) => ({
  config: row.config,
  avgScore: row.avgScore?.toFixed(1),
  avgClearRate: row.avgClearRate?.toFixed(2),
  avgClear: row.avgClear?.toFixed(0),
  avgUltimate: row.avgUltimate?.toFixed(0),
  avgLevel120: row.avgLevel120?.toFixed(1),
  avgLevel600: row.avgLevel600?.toFixed(1),
})));

const bestConfig = [...byConfig].sort((a, b) => a.avgScore - b.avgScore)[0].config;
console.log(`\\n# Best Candidate: ${bestConfig}`);
console.table(rows
  .filter((row) => row.config === bestConfig)
  .map((row) => ({
    route: row.route,
    weapon: row.weapon,
    clearRate: row.clearRate.toFixed(2),
    avgClear: row.avgClear?.toFixed(0) ?? "-",
    avgUltimate: row.avgUltimate?.toFixed(0) ?? "-",
    firstChoice: row.avgFirstChoice?.toFixed(0) ?? "-",
    firstForget: row.avgFirstForget?.toFixed(0) ?? "-",
    level120: row.avgLevel120?.toFixed(1),
    level300: row.avgLevel300?.toFixed(1),
    level600: row.avgLevel600?.toFixed(1),
  })));
