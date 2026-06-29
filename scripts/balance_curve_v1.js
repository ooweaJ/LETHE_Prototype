const config = {
  id: 'v2_stepped_boss_no_deficit',
  hardCap: 1200,
  initialNextXp: 8,
  bossSchedule: [150, 360, 660, 1020],
  bossTargetTtk: [18, 26, 36, 48],
  damage: {
    baseFieldDps: 37,
    baseBossDps: 46,
    levelDps: 0.072,
    echoDps: 0.155,
    ultimateDps: 0.28,
    bossEfficiency: 1.08,
  },
  spawnBands: [
    {
      name: '0: 판독',
      from: 0,
      to: 60,
      spawnInterval: [1.05, 0.78],
      pack: [1, 2],
      cap: [14, 20],
      averageEnemyHp: [38, 48],
      xpPerKill: 1.18,
      xpMul: 1.08,
    },
    {
      name: '1: 첫 문지기 예열',
      from: 60,
      to: 150,
      spawnInterval: [0.76, 0.58],
      pack: [2, 3],
      cap: [22, 30],
      averageEnemyHp: [50, 66],
      xpPerKill: 1.36,
      xpMul: 1.06,
    },
    {
      name: '2: 선택 검증',
      from: 150,
      to: 360,
      spawnInterval: [0.58, 0.48],
      pack: [2, 3],
      cap: [30, 38],
      averageEnemyHp: [68, 92],
      xpPerKill: 1.72,
      xpMul: 1.08,
    },
    {
      name: '3: 잔향 압력',
      from: 360,
      to: 660,
      spawnInterval: [0.48, 0.40],
      pack: [3, 4],
      cap: [38, 48],
      averageEnemyHp: [94, 132],
      xpPerKill: 2.72,
      xpMul: 1.06,
    },
    {
      name: '4: 궁극 준비',
      from: 660,
      to: 1020,
      spawnInterval: [0.40, 0.34],
      pack: [4, 5],
      cap: [48, 58],
      averageEnemyHp: [134, 190],
      xpPerKill: 4.18,
      xpMul: 1.02,
    },
    {
      name: '5: 최종 압축',
      from: 1020,
      to: 1200,
      spawnInterval: [0.34, 0.30],
      pack: [5, 5],
      cap: [58, 64],
      averageEnemyHp: [192, 245],
      xpPerKill: 5.1,
      xpMul: 1,
    },
  ],
};

function lerp(a, b, t) {
  return a + (b - a) * t;
}

function roundTo(value, unit) {
  return Math.round(value / unit) * unit;
}

function findBand(t) {
  return config.spawnBands.find((band) => t >= band.from && t < band.to) ?? config.spawnBands.at(-1);
}

function bandProgress(band, t) {
  if (band.to <= band.from) return 1;
  return Math.max(0, Math.min(1, (t - band.from) / (band.to - band.from)));
}

function bandValue(band, key, t) {
  const value = band[key];
  if (!Array.isArray(value)) return value;
  return lerp(value[0], value[1], bandProgress(band, t));
}

function nextXpAfter(level, current) {
  if (level < 10) return Math.round(current * 1.32 + 5);
  if (level < 16) return Math.round(current * 1.2 + 4);
  return Math.round(current * 1.18 + 5);
}

function powerMultiplier(level, echoes, hasUltimate) {
  const levelMul = 1 + Math.max(0, level - 1) * config.damage.levelDps;
  const echoMul = 1 + echoes * config.damage.echoDps;
  const ultimateMul = hasUltimate ? 1 + config.damage.ultimateDps : 1;
  return levelMul * echoMul * ultimateMul;
}

function fieldDps(level, echoes, hasUltimate) {
  return config.damage.baseFieldDps * powerMultiplier(level, echoes, hasUltimate);
}

function bossDps(level, echoes, hasUltimate) {
  return (
    config.damage.baseBossDps *
    powerMultiplier(level, echoes, hasUltimate) *
    config.damage.bossEfficiency
  );
}

function simulate() {
  let level = 1;
  let xp = 0;
  let nextXp = config.initialNextXp;
  let echoes = 0;
  let hasUltimate = false;
  let kills = 0;
  const seconds = [];
  const bossRows = [];

  for (let t = 0; t <= config.hardCap; t += 1) {
    const band = findBand(t);
    const interval = bandValue(band, 'spawnInterval', t);
    const pack = bandValue(band, 'pack', t);
    const cap = bandValue(band, 'cap', t);
    const enemyHp = bandValue(band, 'averageEnemyHp', t);
    const spawnDemand = pack / interval;
    const killByDps = fieldDps(level, echoes, hasUltimate) / enemyHp;
    const capBrake = Math.min(1, cap / 52);
    const killRate = Math.min(spawnDemand, killByDps) * lerp(0.88, 1.04, capBrake);
    const gainedXp = killRate * band.xpPerKill * band.xpMul;

    kills += killRate;
    xp += gainedXp;

    while (xp >= nextXp) {
      xp -= nextXp;
      level += 1;
      nextXp = nextXpAfter(level, nextXp);
      if (level >= 14 && echoes >= 3) hasUltimate = true;
    }

    const bossIndex = config.bossSchedule.indexOf(t);
    if (bossIndex >= 0) {
      const dps = bossDps(level, echoes, hasUltimate);
      const targetTtk = config.bossTargetTtk[bossIndex];
      const hp = roundTo(dps * targetTtk, 50);
      bossRows.push({
        gate: bossIndex + 1,
        time: t,
        interval:
          bossIndex === 0 ? t : t - config.bossSchedule[bossIndex - 1],
        level,
        echoes,
        ultimate: hasUltimate ? 'Y' : '-',
        avgBossDps: Math.round(dps),
        targetTtk,
        bossHp: hp,
      });
      echoes += 1;
      if (level >= 14 && echoes >= 3) hasUltimate = true;
    }

    if (t % 60 === 0 || t === config.hardCap) {
      seconds.push({
        time: t,
        band: band.name,
        level,
        echoes,
        ultimate: hasUltimate ? 'Y' : '-',
        kills: Math.round(kills),
        xpToNext: Math.round(nextXp - xp),
        avgFieldDps: Math.round(fieldDps(level, echoes, hasUltimate)),
        spawnPerSec: Number(spawnDemand.toFixed(2)),
        killPerSec: Number(killRate.toFixed(2)),
        cap: Math.round(cap),
        averageEnemyHp: Math.round(enemyHp),
      });
    }
  }

  return { timeline: seconds, bosses: bossRows };
}

function printTable(title, rows, headers) {
  console.log(`\n${title}`);
  console.log(headers.join('\t'));
  for (const row of rows) {
    console.log(headers.map((header) => row[header]).join('\t'));
  }
}

const result = simulate();

console.log(`# ${config.id}`);
printTable('BOSS_PLAN', result.bosses, [
  'gate',
  'time',
  'interval',
  'level',
  'echoes',
  'ultimate',
  'avgBossDps',
  'targetTtk',
  'bossHp',
]);
printTable('MINUTE_CURVE', result.timeline, [
  'time',
  'band',
  'level',
  'echoes',
  'ultimate',
  'kills',
  'xpToNext',
  'avgFieldDps',
  'spawnPerSec',
  'killPerSec',
  'cap',
  'averageEnemyHp',
]);
