"use strict";

const canvas = document.getElementById("gameCanvas");
const ctx = canvas.getContext("2d");
const overlay = document.getElementById("overlay");

const ui = {
  phaseLabel: document.getElementById("phaseLabel"),
  timerLabel: document.getElementById("timerLabel"),
  hpLabel: document.getElementById("hpLabel"),
  testerIdInput: document.getElementById("testerIdInput"),
  sessionIdInput: document.getElementById("sessionIdInput"),
  weaponChoices: document.getElementById("weaponChoices"),
  memoryChoices: document.getElementById("memoryChoices"),
  slotCount: document.getElementById("slotCount"),
  startRunButton: document.getElementById("startRunButton"),
  weaponCard: document.getElementById("weaponCard"),
  memorySlots: document.getElementById("memorySlots"),
  echoList: document.getElementById("echoList"),
  combatLog: document.getElementById("combatLog"),
};

const levelUpChoices = {
  attack_speed: {
    id: "attack_speed",
    name: "칼날 가속",
    desc: "기본 공격과 기억 발동 호흡이 빨라집니다.",
    apply() {
      state.runGrowth.attackSpeed += 0.11;
      state.runGrowth.cooldownReduction += 0.04;
    },
    log: "공격 속도 +11%, 기억 호흡 +4%",
  },
  damage: {
    id: "damage",
    name: "검은 물의 힘",
    desc: "기본 공격과 기억 피해가 강해집니다.",
    apply() {
      state.runGrowth.damage += 0.14;
    },
    log: "피해 +14%",
  },
  area: {
    id: "area",
    name: "파문 확장",
    desc: "공격 범위가 넓어지고 포위 대응이 쉬워집니다.",
    apply() {
      state.runGrowth.range += 0.12;
      state.runGrowth.knockback += 0.08;
    },
    log: "범위 +12%, 넉백 +8%",
  },
  survival: {
    id: "survival",
    name: "가라앉지 않는 숨",
    desc: "최대 체력과 회복력이 올라갑니다.",
    apply() {
      state.player.maxHp += 12;
      state.player.hp = Math.min(state.player.maxHp, state.player.hp + 18);
      state.runGrowth.damageReduction += 0.04;
    },
    log: "최대 HP +12, 즉시 회복, 피해 감소 +4%",
  },
  magnet: {
    id: "magnet",
    name: "기억 흡입",
    desc: "처치 경험치가 더 빠르게 모입니다.",
    apply() {
      state.runGrowth.xpGain += 0.16;
    },
    log: "경험치 획득 +16%",
  },
};

const weapons = {
  twin_blades: {
    id: "twin_blades",
    name: "절단쌍검",
    role: "빠른 근접 / 온힛",
    desc: "빠른 타격과 넓어진 기본 베기. 기억을 잃어도 쫄몹을 정리할 수 있는 안정형 무기.",
    range: 86,
    damage: 15,
    interval: 0.36,
    arc: Math.PI * 0.66,
  },
  greatsword: {
    id: "greatsword",
    name: "장송대검",
    role: "느린 강타 / 폭딜",
    desc: "긴 사거리와 강한 전방 정리. 기억 결손 구간에서 한 줄을 뚫어내는 돌파형 무기.",
    range: 128,
    damage: 42,
    interval: 1.02,
    arc: Math.PI * 0.84,
  },
};

const memories = {
  execution_flash: {
    id: "execution_flash",
    name: "처형자의 섬광",
    role: "버스트",
    desc: "위협 적 하나에 주기적 강타.",
    cooldown: 3.15,
    tags: ["burst"],
    echo: "치명타 확률 +12%, 치명타 피해 +35%",
    direction: "대검, 단발 공격, 폭딜 기억이 강해진다.",
  },
  hungry_blades: {
    id: "hungry_blades",
    name: "굶주린 칼무리",
    role: "근접 도트",
    desc: "근접 적을 계속 베는 칼날.",
    cooldown: 0.34,
    tags: ["area", "dot"],
    echo: "공격속도 +18%, 지속 피해 +35%",
    direction: "쌍검, 도트, 근접 유지 빌드가 강해진다.",
  },
  stalker_oath: {
    id: "stalker_oath",
    name: "추적자의 맹세",
    role: "추적 다중",
    desc: "위협 적을 쫓는 투사체.",
    cooldown: 2.2,
    tags: ["burst"],
    echo: "투사체 수 +1, 투사체 속도 +25%",
    direction: "중거리 투사체 기억과 카이팅 운용이 강해진다.",
  },
  shatter_ripple: {
    id: "shatter_ripple",
    name: "파쇄의 파문",
    role: "광역 / 넉백",
    desc: "주변 적을 밀치고 충돌 피해.",
    cooldown: 4.55,
    tags: ["area", "control"],
    echo: "범위 +18%, 넉백 +25%, 피해 감소 +6%",
    direction: "포위 대응, 광역 제어, 대검 접근전이 강해진다.",
  },
  blood_reflection: {
    id: "blood_reflection",
    name: "피의 반사",
    role: "온힛 증폭",
    desc: "기본 공격마다 붉은 추가타.",
    cooldown: 0,
    tags: ["survival", "dot"],
    echo: "추가타 확률 +12%, 온힛 피해 +22%",
    direction: "평타 기반 무기와 빠른 타격 운용이 강해진다.",
  },
  stopped_second: {
    id: "stopped_second",
    name: "멈춘 초침",
    role: "제어",
    desc: "주변 둔화, 기억 쿨 단축.",
    cooldown: 7.25,
    tags: ["control", "dot"],
    echo: "쿨다운 감소 +12%, 둔화 지속 +35%",
    direction: "쿨다운형 기억, 생존형 제어 빌드가 강해진다.",
  },
  ashen_guard: {
    id: "ashen_guard",
    name: "잿빛 보호막",
    role: "보호 / 반격",
    desc: "주기적 보호막, 파열 시 주변 피해.",
    cooldown: 8.4,
    tags: ["survival", "area"],
    echo: "보호막 지속 +20%, 무기 반격 +10%",
    direction: "결손 생존, 광역 반격, 무기 방어 피벗이 강해진다.",
  },
  oblivion_brand: {
    id: "oblivion_brand",
    name: "망각의 낙인",
    role: "표식 / 증폭",
    desc: "위협 적에 낙인, 받는 피해 증가.",
    cooldown: 5.2,
    tags: ["burst", "control"],
    echo: "낙인 지속 +20%, 무기 표식 +10%",
    direction: "폭딜 제어, 단일 표식, 보스 딜타임 피벗이 강해진다.",
  },
};

const synergyRules = [
  {
    id: "area_control",
    name: "압축 파문",
    tags: ["area", "control"],
    desc: "둔화되거나 밀린 적에게 광역 피해가 증가합니다.",
  },
  {
    id: "dot_control",
    name: "느린 출혈",
    tags: ["dot", "control"],
    desc: "둔화된 적에게 지속 피해가 강해집니다.",
  },
  {
    id: "burst_survival",
    name: "피 묻은 결의",
    tags: ["burst", "survival"],
    desc: "큰 피해를 넣으면 짧게 회복합니다.",
  },
  {
    id: "brand_chain",
    name: "각인 연쇄",
    tags: ["burst", "control"],
    desc: "약화되거나 낙인된 적에게 단발 폭딜이 증가합니다.",
  },
];

const enemyTypes = {
  eroder: {
    id: "eroder",
    name: "침식자",
    hp: 48,
    speed: 48,
    damage: 9,
    radius: 13,
    color: "#61d5b8",
    score: 1,
  },
  drifting_eye: {
    id: "drifting_eye",
    name: "떠도는 눈",
    hp: 36,
    speed: 33,
    damage: 7,
    radius: 12,
    color: "#a98cff",
    score: 2,
    shooter: true,
  },
  split_one: {
    id: "split_one",
    name: "쪼개진 자",
    hp: 58,
    speed: 42,
    damage: 8,
    radius: 15,
    color: "#e8c15d",
    score: 2,
    splitter: true,
  },
  void_priest: {
    id: "void_priest",
    name: "공허 사제",
    hp: 74,
    speed: 28,
    damage: 5,
    radius: 14,
    color: "#ff7c90",
    score: 3,
    healer: true,
  },
};

const qaMode = new URLSearchParams(window.location.search).get("qa") || "";

const experiment = {
  version: "v0.10",
  echoPower: 0.5,
  uiClarity: 0.78,
  runDurationSec: 600,
  bossScheduleSec: [180, 340, 490, 600],
  bossHp: 560,
  deficitDurationSec: 75,
  qaFastMode: qaMode.includes("fast"),
  qaLevelupMode: qaMode.includes("levelup"),
  qaV06Mode: qaMode.includes("v06"),
  qaDeathMode: qaMode.includes("death"),
  qaIdentityMode: qaMode.includes("identity"),
  qaPressureMode: qaMode.includes("pressure"),
  qaPostLossMode: qaMode.includes("postloss"),
  qaTacticalMode: qaMode.includes("tactical"),
};

if (experiment.qaFastMode) {
  experiment.runDurationSec = 96;
  experiment.bossScheduleSec = [18, 38, 62, 88];
  experiment.bossHp = 180;
  experiment.deficitDurationSec = 5;
}

const keys = new Set();
let selectedWeapon = weapons.twin_blades.id;
let selectedMemories = [];
let playtestMeta = readPlaytestMetaFromUrl();
let state = null;
let lastFrame = performance.now();

const baseEcho = {
  critChance: 0.04,
  critDamage: 0.45,
  attackSpeed: 0,
  dotDamage: 0,
  projectileCount: 0,
  projectileSpeed: 0,
  range: 0,
  knockback: 0,
  damageReduction: 0,
  extraHitChance: 0,
  onHitDamage: 0,
  cooldownReduction: 0,
  slowDuration: 0,
  shieldDuration: 0,
  markDuration: 0,
  weaponFlashChance: 0,
  weaponBleedDamage: 0,
  weaponHomingChance: 0,
  weaponShockwaveChance: 0,
  weaponSlowChance: 0,
  weaponCounterChance: 0,
  weaponMarkChance: 0,
};

function createMemoryInstance(id) {
  return {
    ...memories[id],
    cooldownLeft: id === "blood_reflection" ? 0 : 0.8 + Math.random() * 1.3,
    clarity: 0,
    forgotten: false,
    joinedAt: state ? Number(state.elapsed.toFixed(2)) : 0,
  };
}

function createMetricSeed(memoryIds) {
  const metricSeed = {};
  memoryIds.forEach((id) => {
    metricSeed[id] = {
      damage: 0,
      kills: 0,
      assists: 0,
      status: 0,
      bossDamage: 0,
      groggyDamage: 0,
      bossControls: 0,
      activeCount: 0,
      presenceTime: 0,
      focusDependency: 0,
      score: 0,
      components: {},
    };
  });
  return metricSeed;
}

function createRunState() {
  const activeMemories = selectedMemories.map((id) => createMemoryInstance(id));

  return {
    running: true,
    mode: "combat",
    elapsed: 0,
    waveElapsed: 0,
    bossSpawned: false,
    phase: "전투",
    weapon: weapons[selectedWeapon],
    memories: activeMemories,
    echo: { ...baseEcho },
    tagEchoes: [],
    activeSynergies: [],
    runGrowth: {
      level: 1,
      xp: 0,
      nextXp: 8,
      damage: 0,
      attackSpeed: 0,
      cooldownReduction: 0,
      range: 0,
      knockback: 0,
      damageReduction: 0,
      xpGain: 0,
      choicesTaken: [],
      pendingChoices: [],
      earlyKills: 0,
      maxEnemies: 0,
      levelUpsBeforeBoss: 0,
    },
    danger: {
      deaths: 0,
      deathAt: null,
      deathPhase: null,
      deathActiveMemoryCount: null,
      lowHpTime: 0,
      enemyPressureTime: 0,
      maxEnemies: 0,
      lastKillAt: 0,
      maxKillGap: 0,
      currentKillGap: 0,
      deficitTime: 0,
      deficitLowHpTime: 0,
      deficitDeath: false,
      deficitBreathTime: 0,
      deficitChallengeTime: 0,
      deficitChallengeLowHpTime: 0,
      postLossChallengeCompletions: 0,
      pressureLullTime: 0,
      pressureRiseTime: 0,
      pressureClimaxTime: 0,
    },
    player: {
      x: canvas.width / 2,
      y: canvas.height / 2,
      r: 14,
      hp: 100,
      maxHp: 100,
      speed: 176,
      attackCd: 0,
      invuln: 0,
      facing: 0,
      shield: 0,
      maxShield: 0,
    },
    enemies: [],
    projectiles: [],
    effects: [],
    floaters: [],
    boss: null,
    shake: 0,
    spawnCd: 0,
    metrics: createMetricSeed(selectedMemories),
    runTimeline: {
      version: experiment.version,
      totalRunSec: experiment.runDurationSec,
      bossScheduleSec: [...experiment.bossScheduleSec],
      nextBossIndex: 0,
      currentBossIndex: 0,
      deficitDurationSec: experiment.deficitDurationSec,
      deficitStartedAt: null,
      refillAvailableAt: null,
      cycles: [],
      refillChoices: [],
      pressurePhaseId: null,
      pressureSegments: [],
      postLossChallenges: [],
    },
    tacticalFocus: {
      cooldownSec: 9,
      cooldownLeft: 0,
      durationLeft: 0,
      memoryId: null,
      overheatSec: 9,
      dependencyBonus: {},
      useCount: 0,
      successfulCount: 0,
      lastUsedAt: null,
      history: [],
    },
    questions: {
      protect: null,
      predict: null,
      release: null,
    },
    forkChoice: null,
    echoUnlocks: [],
    forgotten: null,
    forgottenHistory: [],
    death: null,
    survey: {
      sadness: null,
      fairness: null,
      memoryRecall: "",
    },
    logs: {
      version: experiment.version,
      experiment: { ...experiment },
      playtest: currentPlaytestMeta(),
      runGrowth: null,
      runTimeline: null,
      startedAt: new Date().toISOString(),
      weapon: selectedWeapon,
      memories: [...selectedMemories],
      memoryNames: selectedMemories.map((id) => memories[id].name),
      events: [],
    },
  };
}

function logEvent(type, payload = {}) {
  if (!state) return;
  state.logs.events.push({
    t: Number(state.elapsed.toFixed(2)),
    type,
    ...payload,
  });
}

function initSetup() {
  ui.weaponChoices.innerHTML = "";
  Object.values(weapons).forEach((weapon) => {
    const button = document.createElement("button");
    button.className = `choice ${weapon.id === selectedWeapon ? "selected" : ""}`;
    button.type = "button";
    button.innerHTML = `<strong>${weapon.name}</strong><span>${weapon.role}<br>${weapon.desc}</span>`;
    button.addEventListener("click", () => {
      selectedWeapon = weapon.id;
      renderSetup();
    });
    ui.weaponChoices.appendChild(button);
  });

  ui.memoryChoices.innerHTML = "";
  Object.values(memories).forEach((memory) => {
    const button = document.createElement("button");
    button.className = `choice ${selectedMemories.includes(memory.id) ? "selected" : ""}`;
    button.type = "button";
    button.innerHTML = memoryChoiceHtml(memory);
    button.addEventListener("click", () => toggleMemory(memory.id));
    ui.memoryChoices.appendChild(button);
  });

  ui.startRunButton.addEventListener("click", startRun);
  bindPlaytestMetaInputs();
  renderSetup();
}

function memoryChoiceHtml(memory) {
  return `<strong>${memory.name}</strong><span class="choice-summary">${memory.role} · ${memory.desc}</span><div class="tag-row">${tagBadges(memory.tags)}</div>`;
}

function bindPlaytestMetaInputs() {
  if (ui.testerIdInput) {
    ui.testerIdInput.value = playtestMeta.testerId;
    ui.testerIdInput.addEventListener("input", () => {
      playtestMeta.testerId = ui.testerIdInput.value.trim();
    });
  }
  if (ui.sessionIdInput) {
    ui.sessionIdInput.value = playtestMeta.sessionId;
    ui.sessionIdInput.addEventListener("input", () => {
      playtestMeta.sessionId = ui.sessionIdInput.value.trim();
    });
  }
}

function toggleMemory(id) {
  if (selectedMemories.includes(id)) {
    selectedMemories = selectedMemories.filter((memoryId) => memoryId !== id);
  } else if (selectedMemories.length < 3) {
    selectedMemories.push(id);
  }
  renderSetup();
}

function renderSetup() {
  initChoiceSelection(ui.weaponChoices, selectedWeapon);
  initChoiceSelection(ui.memoryChoices, selectedMemories);
  ui.slotCount.textContent = `${selectedMemories.length} / 3`;
  ui.startRunButton.disabled = selectedMemories.length !== 3;
  ui.weaponCard.innerHTML = weaponCardHtml(weapons[selectedWeapon]);
  renderMemorySlots();
  renderSetupSynergies();
}

function initChoiceSelection(container, selection) {
  [...container.children].forEach((child) => {
    const name = child.querySelector("strong").textContent;
    const weaponHit = weapons[selection]?.name === name;
    const memoryHit = Array.isArray(selection) && selection.some((id) => memories[id].name === name);
    child.classList.toggle("selected", weaponHit || memoryHit);
  });
}

function weaponCardHtml(weapon) {
  return `<strong>${weapon.name}</strong><br>${weapon.role}<br>${weapon.desc}`;
}

function activeSynergiesFor(memoryIds) {
  const tagSet = new Set();
  memoryIds.forEach((id) => (memories[id]?.tags || []).forEach((tag) => tagSet.add(tag)));
  return synergyRules.filter((rule) => rule.tags.every((tag) => tagSet.has(tag)));
}

function buildIdentityFor(memoryIds, sourceState = state) {
  const ids = memoryIds.filter((id) => memories[id]);
  const activeSynergies = activeSynergiesFor(ids);
  const tagCounts = ids.reduce((counts, id) => {
    (memories[id].tags || []).forEach((tag) => {
      counts[tag] = (counts[tag] || 0) + 1;
    });
    return counts;
  }, {});
  const primaryTag = Object.entries(tagCounts).sort((a, b) => b[1] - a[1] || tagLabel(a[0]).localeCompare(tagLabel(b[0]), "ko"))[0]?.[0] || null;
  const mostDependentMemory = mostDependentMemoryFor(ids, sourceState);
  return {
    buildName: buildNameFor(tagCounts, activeSynergies),
    primaryTag,
    primaryTagLabel: primaryTag ? tagLabel(primaryTag) : null,
    tags: Object.keys(tagCounts),
    tagLabels: Object.keys(tagCounts).map(tagLabel),
    activeSynergies: activeSynergies.map((rule) => ({
      id: rule.id,
      name: rule.name,
      desc: rule.desc,
    })),
    mostDependentMemory,
    memoryIds: ids,
    memoryNames: ids.map((id) => memories[id].name),
    visibleBy90Sec: sourceState ? sourceState.elapsed <= 180 || sourceState.logs?.buildIdentitySeenBy90Sec : ids.length === 3,
  };
}

function buildNameFor(tagCounts, activeSynergies) {
  if (activeSynergies.some((rule) => rule.id === "area_control")) return "압축 제어 빌드";
  if (activeSynergies.some((rule) => rule.id === "dot_control")) return "느린 출혈 빌드";
  if (activeSynergies.some((rule) => rule.id === "burst_survival")) return "피 묻은 결의 빌드";
  if (activeSynergies.some((rule) => rule.id === "brand_chain")) return "각인 처형 빌드";
  if ((tagCounts.burst || 0) >= 2) return "집중 처형 빌드";
  if ((tagCounts.dot || 0) >= 2) return "지속 절삭 빌드";
  if ((tagCounts.control || 0) >= 2) return "시간 제어 빌드";
  if ((tagCounts.area || 0) >= 2) return "광역 압박 빌드";
  return "느슨한 기억 조합";
}

function mostDependentMemoryFor(memoryIds, sourceState = state) {
  if (!sourceState) {
    return {
      id: null,
      name: "전투 중 계산",
      score: 0,
      deletionWeight: 0,
    };
  }
  const ranked = memoryIds
    .filter((id) => sourceState.metrics[id])
    .map((id) => ({
      id,
      name: memories[id].name,
      score: sourceState.metrics[id].score || 0,
      deletionWeight: sourceState.metrics[id].deletionWeight || 0,
    }))
    .sort((a, b) => b.score - a.score || b.deletionWeight - a.deletionWeight);
  return ranked[0] || {
    id: null,
    name: "계산 중",
    score: 0,
    deletionWeight: 0,
  };
}

function buildIdentityHtml(identity) {
  const synergyLine = identity.activeSynergies.length
    ? identity.activeSynergies.map((rule) => rule.name).join(" / ")
    : "활성 시너지 없음";
  const dependent = identity.mostDependentMemory;
  const dependentScore = dependent?.score ? ` ${Math.round(dependent.score)}점` : "";
  return `
    <div class="build-card">
      <div class="build-card-row">
        <span>현재 빌드</span>
        <strong>${identity.buildName}</strong>
      </div>
      <div class="build-card-row">
        <span>활성 시너지</span>
        <strong>${synergyLine}</strong>
      </div>
      <div class="build-card-row">
        <span>의존 중인 기억</span>
        <strong>${dependent?.name || "계산 중"}${dependentScore}</strong>
      </div>
    </div>
  `;
}

function refreshActiveSynergies() {
  if (!state) return [];
  state.activeSynergies = activeSynergiesFor(activeMemoryIds());
  return state.activeSynergies;
}

function hasSynergy(id) {
  return Boolean(state?.activeSynergies?.some((rule) => rule.id === id));
}

function tagBadges(tags = []) {
  return tags.map((tag) => `<span class="tag-chip">${tagLabel(tag)}</span>`).join("");
}

function tagLabel(tag) {
  const labels = {
    burst: "폭딜",
    area: "광역",
    dot: "지속",
    control: "제어",
    survival: "생존",
  };
  return labels[tag] || tag;
}

function renderSetupSynergies() {
  if (state) return;
  if (!selectedMemories.length) {
    ui.echoList.textContent = "기억 태그를 조합하면 시너지가 열립니다.";
    return;
  }
  const identity = buildIdentityFor(selectedMemories, null);
  const active = activeSynergiesFor(selectedMemories);
  const tags = selectedMemories.flatMap((id) => memories[id]?.tags || []);
  const tagText = [...new Set(tags)].map((tag) => `<span class="tag-chip">${tagLabel(tag)}</span>`).join("");
  const synergyText = active.length
    ? active.map((rule) => `<div class="echo-line"><strong>${rule.name}</strong><br>${rule.desc}</div>`).join("")
    : `<div class="echo-line">활성 시너지 없음</div>`;
  ui.echoList.innerHTML = `${buildIdentityHtml(identity)}<div class="tag-row">${tagText}</div>${synergyText}`;
}

function startRun() {
  playtestMeta = currentPlaytestMeta();
  state = createRunState();
  refreshActiveSynergies();
  state.logs.buildIdentity = buildIdentityFor(activeMemoryIds(), state);
  state.logs.buildIdentitySeenBy90Sec = true;
  overlay.classList.remove("show");
  addLog(experiment.qaFastMode ? "QA fast mode: 검은 물이 빠르게 차오른다." : "검은 물 위로 기억이 떠올랐다.");
  logEvent("run_start", {
    playtest: state.logs.playtest,
    buildIdentity: state.logs.buildIdentity,
  });
  writeIdentityQaResult({ status: "started" });
}

function addLog(text) {
  const row = document.createElement("div");
  row.className = "log-entry";
  row.textContent = text;
  ui.combatLog.prepend(row);
  while (ui.combatLog.children.length > 10) ui.combatLog.lastChild.remove();
}

function update(dt) {
  if (!state || state.mode !== "combat") return;

  state.elapsed += dt;
  state.waveElapsed += dt;
  state.player.invuln = Math.max(0, state.player.invuln - dt);
  state.player.attackCd = Math.max(0, state.player.attackCd - dt);

  movePlayer(dt);
  updateTacticalFocus(dt);
  updateRefillGate();
  updateSpawning(dt);
  updateBoss(dt);
  updateEnemies(dt);
  state.runGrowth.maxEnemies = Math.max(state.runGrowth.maxEnemies, state.enemies.length);
  updateDangerMetrics(dt);
  updateProjectiles(dt);
  updateMemories(dt);
  updateEffects(dt);
  if (checkPlayerDeath()) return;
  updateClarity();
  updateUi();
}

function updateTacticalFocus(dt) {
  const focus = state.tacticalFocus;
  focus.cooldownLeft = Math.max(0, focus.cooldownLeft - dt);
  focus.durationLeft = Math.max(0, focus.durationLeft - dt);
  if (focus.durationLeft <= 0) focus.memoryId = null;
}

function requestTacticalFocus(memoryId) {
  if (!state || state.mode !== "combat") return false;
  const focus = state.tacticalFocus;
  if (focus.cooldownLeft > 0) return false;
  const memory = activeMemories().find((candidate) => candidate.id === memoryId);
  if (!memory) return false;

  const beforeCooldown = memory.cooldownLeft || 0;
  focus.cooldownLeft = focus.cooldownSec;
  focus.durationLeft = 3;
  focus.memoryId = memory.id;
  focus.dependencyBonus[memory.id] = (focus.dependencyBonus[memory.id] || 0) + 18;
  state.metrics[memory.id].focusDependency += 18;
  focus.useCount += 1;
  focus.lastUsedAt = Number(state.elapsed.toFixed(2));

  if (memory.cooldown > 0) {
    memory.cooldownLeft = Math.min(beforeCooldown, Math.max(0.12, memory.cooldown * 0.24));
  }
  const afterCooldown = memory.cooldownLeft || 0;
  const synergyBoosted = activeSynergiesFor(activeMemoryIds()).some((rule) => (memory.tags || []).some((tag) => rule.tags.includes(tag)));
  const successful = memory.cooldown === 0 || beforeCooldown - afterCooldown >= 0.05 || synergyBoosted;
  if (successful) focus.successfulCount += 1;

  const entry = {
    t: Number(state.elapsed.toFixed(2)),
    memoryId: memory.id,
    memoryName: memory.name,
    beforeCooldown: Number(beforeCooldown.toFixed(2)),
    afterCooldown: Number(afterCooldown.toFixed(2)),
    dependencyAdded: 18,
    synergyBoosted,
    successful,
  };
  focus.history.push(entry);
  addLog(`전술 집중: ${memory.name}`);
  addFloater("전술 집중", state.player.x, state.player.y - 42, "#e8c15d");
  addBurst(state.player.x, state.player.y, "#e8c15d", 10, 2.4);
  logEvent("tactical_focus", entry);
  renderMemorySlots();
  renderEchoes();
  writeTacticalQaResult({ status: "used" });
  return true;
}

function activeTacticalFocus(memoryId) {
  return state?.tacticalFocus?.memoryId === memoryId && state.tacticalFocus.durationLeft > 0;
}

function movePlayer(dt) {
  const p = state.player;
  let dx = 0;
  let dy = 0;
  if (keys.has("ArrowLeft") || keys.has("KeyA")) dx -= 1;
  if (keys.has("ArrowRight") || keys.has("KeyD")) dx += 1;
  if (keys.has("ArrowUp") || keys.has("KeyW")) dy -= 1;
  if (keys.has("ArrowDown") || keys.has("KeyS")) dy += 1;
  const len = Math.hypot(dx, dy) || 1;
  p.x = clamp(p.x + (dx / len) * p.speed * dt, 24, canvas.width - 24);
  p.y = clamp(p.y + (dy / len) * p.speed * dt, 24, canvas.height - 24);
  if (dx || dy) p.facing = Math.atan2(dy, dx);
}

function updateSpawning(dt) {
  if (state.boss) return;
  state.spawnCd -= dt;
  const profile = pressureProfile();
  updatePressurePhase(profile);
  const spawnRate = Math.max(0.34, profile.spawnRate);
  if (state.spawnCd <= 0) {
    state.spawnCd = spawnRate;
    const packSize = profile.packSize;
    const pool = pressureEnemyPool(profile);
    for (let i = 0; i < packSize; i += 1) {
      spawnEnemy(pool[Math.floor(Math.random() * pool.length)]);
    }
  }

  const nextBossAt = state.runTimeline.bossScheduleSec[state.runTimeline.nextBossIndex];
  if (nextBossAt && state.elapsed >= nextBossAt) {
    spawnBoss();
  }
}

function pressureProfile() {
  if (activeMemoryCount() < 3) {
    const timeline = state.runTimeline;
    const start = timeline.deficitStartedAt ?? state.elapsed;
    const end = timeline.refillAvailableAt ?? start + experiment.deficitDurationSec;
    const progress = clamp((state.elapsed - start) / Math.max(1, end - start), 0, 1);
    if (progress < 0.30) {
      return {
        id: "deficit_breath",
        label: "결손 정비",
        note: "잃은 기억의 빈자리를 확인하는 짧은 숨 고르기",
        intensity: 0.48,
        spawnRate: 0.72,
        packSize: 2,
        postLossChallenge: true,
      };
    }
    return {
      id: "deficit_trial",
      label: "결손 압박",
      note: "기억 2개와 잔향만으로 버티는 짧은 테스트",
      intensity: 0.82,
      spawnRate: progress > 0.72 ? 0.44 : 0.50,
      packSize: progress > 0.72 ? 4 : 3,
      postLossChallenge: true,
    };
  }

  const timeline = state.runTimeline;
  const nextBossAt = timeline.bossScheduleSec[timeline.nextBossIndex] || experiment.runDurationSec;
  const prevBossAt = timeline.nextBossIndex > 0 ? timeline.bossScheduleSec[timeline.nextBossIndex - 1] : 0;
  const progress = clamp((state.elapsed - prevBossAt) / Math.max(1, nextBossAt - prevBossAt), 0, 1);

  if (progress < 0.24) {
    return {
      id: "lull",
      label: "숨 고르기",
      note: "레벨업과 빌드 확인을 위한 낮은 압박",
      intensity: 0.42,
      spawnRate: 0.72,
      packSize: state.elapsed < 24 ? 2 : 3,
    };
  }

  if (progress < 0.70) {
    return {
      id: "rising",
      label: "압박 상승",
      note: "적 밀도가 올라가며 선택한 빌드를 시험",
      intensity: 0.70,
      spawnRate: 0.54,
      packSize: 3,
    };
  }

  return {
    id: "climax",
    label: "망각 전조",
    note: "문지기 직전 최고 압박",
    intensity: 0.92,
    spawnRate: 0.43,
    packSize: 4,
  };
}

function pressureEnemyPool(profile) {
  const base = ["eroder", "eroder", "eroder", "drifting_eye", "split_one"];
  if (profile.id === "lull") return ["eroder", "eroder", "drifting_eye", "split_one"];
  if (profile.id === "rising") return base.concat(state.elapsed > 28 ? ["void_priest"] : []);
  if (profile.id === "climax") return base.concat(["drifting_eye", "split_one", "void_priest"]);
  if (profile.id === "deficit_breath") return ["eroder", "eroder", "drifting_eye", "split_one"];
  if (profile.id === "deficit_trial") return base.concat(["drifting_eye", "void_priest"]);
  return ["eroder", "eroder", "split_one", "drifting_eye"];
}

function updatePressurePhase(profile) {
  if (state.runTimeline.pressurePhaseId === profile.id) return;
  state.runTimeline.pressurePhaseId = profile.id;
  state.phase = profile.label;
  state.runTimeline.pressureSegments.push({
    id: profile.id,
    label: profile.label,
    at: Number(state.elapsed.toFixed(2)),
    nextBossIndex: state.runTimeline.nextBossIndex + 1,
    intensity: profile.intensity,
  });
  addLog(`${profile.label}: ${profile.note}`);
  logEvent("pressure_phase", {
    id: profile.id,
    label: profile.label,
    intensity: profile.intensity,
    nextBossAt: state.runTimeline.bossScheduleSec[state.runTimeline.nextBossIndex] || null,
  });
  if (profile.postLossChallenge) recordPostLossChallengePhase(profile);
}

function currentPostLossChallenge() {
  const challenges = state.runTimeline.postLossChallenges;
  return challenges[challenges.length - 1] || null;
}

function recordPostLossChallengePhase(profile) {
  const challenge = currentPostLossChallenge();
  if (!challenge || challenge.completedAt) return;
  if (!challenge.segments.some((segment) => segment.id === profile.id)) {
    challenge.segments.push({
      id: profile.id,
      label: profile.label,
      at: Number(state.elapsed.toFixed(2)),
      intensity: profile.intensity,
    });
  }
  if (profile.id === "deficit_trial" && challenge.challengeStartedAt === null) {
    challenge.challengeStartedAt = Number(state.elapsed.toFixed(2));
  }
  logEvent("post_loss_challenge_phase", {
    cycleIndex: challenge.cycleIndex,
    id: profile.id,
    label: profile.label,
    intensity: profile.intensity,
  });
}

function completePostLossChallenge() {
  const challenge = currentPostLossChallenge();
  if (!challenge || challenge.completedAt) return;
  challenge.completedAt = Number(state.elapsed.toFixed(2));
  challenge.survived = !state.death && activeMemoryCount() < 3;
  challenge.remainingHp = Number(state.player.hp.toFixed(1));
  challenge.activeMemoryCount = activeMemoryCount();
  challenge.durationSec = Number((challenge.completedAt - challenge.startedAt).toFixed(2));
  state.danger.postLossChallengeCompletions += challenge.survived ? 1 : 0;
  logEvent("post_loss_challenge_completed", {
    cycleIndex: challenge.cycleIndex,
    survived: challenge.survived,
    remainingHp: challenge.remainingHp,
    durationSec: challenge.durationSec,
    activeMemoryIds: activeMemoryIds(),
  });
}

function updateRefillGate() {
  const timeline = state.runTimeline;
  if (!timeline.refillAvailableAt || state.elapsed < timeline.refillAvailableAt) return;
  if (state.boss || activeMemoryCount() >= 3) return;
  completePostLossChallenge();
  timeline.refillAvailableAt = null;
  showRefillOverlay();
}

function spawnEnemy(typeId, x = null, y = null, child = false) {
  const type = enemyTypes[typeId];
  const pos = x === null ? edgePosition() : { x, y };
  state.enemies.push({
    ...type,
    x: pos.x,
    y: pos.y,
    hp: child ? Math.round(type.hp * 0.38) : type.hp,
    maxHp: child ? Math.round(type.hp * 0.38) : type.hp,
    r: child ? Math.max(8, type.radius * 0.68) : type.radius,
    shotCd: 1.4 + Math.random(),
    healCd: 1.8,
    slow: 0,
    hitBy: new Set(),
    child,
  });
}

function spawnBoss() {
  state.bossSpawned = true;
  const bossIndex = state.runTimeline.nextBossIndex + 1;
  const isFinal = bossIndex >= state.runTimeline.bossScheduleSec.length;
  const miniBoss = bossIndex === 1;
  state.runTimeline.currentBossIndex = bossIndex;
  state.runTimeline.nextBossIndex += 1;
  state.phase = miniBoss ? "첫 망각 문지기" : isFinal ? "최종 문지기" : `${bossIndex}차 문지기`;
  state.enemies.length = Math.min(state.enemies.length, 8);
  state.shake = Math.max(state.shake, 12);
  state.boss = {
    id: "boss",
    name: miniBoss ? "작은 문지기" : isFinal ? "끝의 문지기" : `기억을 씹는 자 ${bossIndex}`,
    x: canvas.width / 2,
    y: 96,
    hp: Math.round((miniBoss ? experiment.bossHp * 0.68 : experiment.bossHp) * (1 + Math.max(0, bossIndex - 2) * 0.18)),
    maxHp: Math.round((miniBoss ? experiment.bossHp * 0.68 : experiment.bossHp) * (1 + Math.max(0, bossIndex - 2) * 0.18)),
    r: 32,
    phase: 1,
    phaseTimer: 0,
    actionCd: 1.2,
    groggy: false,
    groggyTimer: 0,
    aoeCd: 4,
    cycleIndex: bossIndex,
    final: isFinal,
    miniBoss,
  };
  addLog(`${state.boss.name}가 검은 물을 갈랐다.`);
  addFloater(miniBoss ? "첫 망각 문지기" : isFinal ? "최종 문지기" : `${bossIndex}차 문지기`, canvas.width / 2, 84, "#ff5d6c");
  addBurst(canvas.width / 2, 96, "#ff5d6c", 28, 6.8);
  logEvent("boss_spawn", {
    cycleIndex: bossIndex,
    bossName: state.boss.name,
    final: isFinal,
    miniBoss,
    activeMemoryCount: activeMemoryCount(),
  });
}

function updateBoss(dt) {
  const boss = state.boss;
  if (!boss) return;
  boss.marked = Math.max(0, (boss.marked || 0) - dt);
  boss.vulnerable = Math.max(0, (boss.vulnerable || 0) - dt);

  boss.phaseTimer += dt;
  boss.actionCd -= dt;
  boss.aoeCd -= dt;

  const hpRate = boss.hp / boss.maxHp;
  const nextPhase = hpRate > 0.66 ? 1 : hpRate > 0.33 ? 2 : 3;
  if (nextPhase !== boss.phase) {
    boss.phase = nextPhase;
    boss.phaseTimer = 0;
    boss.groggy = boss.phase === 3;
    boss.groggyTimer = boss.groggy ? 3.2 : 0;
    addLog(`${boss.name} ${boss.phase}페이즈.`);
    addFloater(`${boss.phase}페이즈`, boss.x, boss.y - 38, boss.groggy ? "#e8c15d" : "#ff5d6c");
    addBurst(boss.x, boss.y, boss.groggy ? "#e8c15d" : "#ff5d6c", 18, 4.6);
    state.shake = Math.max(state.shake, boss.phase === 3 ? 9 : 6);
    logEvent("boss_phase", { phase: boss.phase });
  }

  if (boss.groggy) {
    boss.groggyTimer -= dt;
    if (boss.groggyTimer <= 0) boss.groggy = false;
  }

  if (boss.phase === 1 && boss.actionCd <= 0) {
    boss.actionCd = 3.2;
    spawnEnemy(Math.random() > 0.45 ? "eroder" : "split_one", boss.x + rand(-50, 50), boss.y + rand(20, 60));
  }

  if (boss.phase === 2 && boss.actionCd <= 0) {
    boss.actionCd = 2.1;
    boss.x = rand(110, canvas.width - 110);
    boss.y = rand(80, 240);
    for (let i = 0; i < 8; i += 1) {
      const angle = (Math.PI * 2 * i) / 8 + rand(-0.08, 0.08);
      addProjectile({
        x: boss.x,
        y: boss.y,
        vx: Math.cos(angle) * 150,
        vy: Math.sin(angle) * 150,
        r: 5,
        damage: 8,
        hostile: true,
        life: 4,
        color: "#ff5d6c",
      });
    }
  }

  if (boss.phase === 3 && boss.aoeCd <= 0) {
    boss.aoeCd = boss.groggy ? 4.4 : 2.7;
    state.effects.push({
      type: "hostile_aoe",
      x: state.player.x + rand(-90, 90),
      y: state.player.y + rand(-70, 70),
      r: 28,
      maxR: 92,
      life: 1.35,
      maxLife: 1.35,
      damage: 15,
      hit: false,
    });
  }

  const toPlayer = angleTo(boss, state.player);
  const desired = boss.phase === 2 ? 280 : 160;
  const distToPlayer = distance(boss, state.player);
  const speed = boss.groggy ? 0 : boss.phase === 3 ? 56 : 34;
  if (distToPlayer > desired + 30) {
    boss.x += Math.cos(toPlayer) * speed * dt;
    boss.y += Math.sin(toPlayer) * speed * dt;
  } else if (distToPlayer < desired - 40) {
    boss.x -= Math.cos(toPlayer) * speed * dt;
    boss.y -= Math.sin(toPlayer) * speed * dt;
  }
  boss.x = clamp(boss.x, 48, canvas.width - 48);
  boss.y = clamp(boss.y, 48, canvas.height - 48);

  if (boss.hp <= 0) {
    defeatBoss();
  }
}

function updateEnemies(dt) {
  const p = state.player;
  for (const enemy of state.enemies) {
    enemy.slow = Math.max(0, enemy.slow - dt);
    enemy.marked = Math.max(0, (enemy.marked || 0) - dt);
    enemy.vulnerable = Math.max(0, (enemy.vulnerable || 0) - dt);
    enemy.shotCd -= dt;
    enemy.healCd -= dt;
    const slowMul = enemy.slow > 0 ? 0.45 : 1;
    const angle = angleTo(enemy, p);
    const dist = distance(enemy, p);

    if (enemy.shooter && dist < 320) {
      if (enemy.shotCd <= 0) {
        enemy.shotCd = 2.6;
        addProjectile({
          x: enemy.x,
          y: enemy.y,
          vx: Math.cos(angle) * 135,
          vy: Math.sin(angle) * 135,
          r: 4,
          damage: enemy.damage,
          hostile: true,
          life: 4,
          color: enemy.color,
        });
      }
      enemy.x -= Math.cos(angle) * enemy.speed * 0.4 * slowMul * dt;
      enemy.y -= Math.sin(angle) * enemy.speed * 0.4 * slowMul * dt;
    } else {
      enemy.x += Math.cos(angle) * enemy.speed * slowMul * dt;
      enemy.y += Math.sin(angle) * enemy.speed * slowMul * dt;
    }

    if (enemy.healer && enemy.healCd <= 0) {
      enemy.healCd = 2.4;
      let healed = false;
      for (const other of state.enemies) {
        if (other !== enemy && distance(enemy, other) < 95 && other.hp < other.maxHp) {
          other.hp = Math.min(other.maxHp, other.hp + 12);
          healed = true;
        }
      }
      if (healed) {
        state.effects.push({ type: "heal_pulse", x: enemy.x, y: enemy.y, r: 12, maxR: 96, life: 0.45, maxLife: 0.45 });
      }
    }

    if (dist < enemy.r + p.r && p.invuln <= 0) {
      const reduced = 1 - state.echo.damageReduction - state.runGrowth.damageReduction;
      damagePlayer(enemy.damage * reduced, enemy);
      p.invuln = 0.52;
    }
  }

  state.enemies = state.enemies.filter((enemy) => {
    if (enemy.hp > 0) return true;
    for (const memoryId of enemy.hitBy) {
      if (state.metrics[memoryId]) {
        state.metrics[memoryId].assists += 1;
      }
    }
    const killer = enemy.killedBy;
    if (killer && state.metrics[killer]) state.metrics[killer].kills += 1;
    recordKillPressure();
    grantXp(enemy);
    if (enemy.splitter && !enemy.child) {
      spawnEnemy("eroder", enemy.x - 12, enemy.y + 4, true);
      spawnEnemy("eroder", enemy.x + 12, enemy.y - 4, true);
    }
    return false;
  });
}

function updateProjectiles(dt) {
  for (const projectile of state.projectiles) {
    projectile.life -= dt;
    if (projectile.homing && projectile.target && projectile.target.hp > 0) {
      const angle = angleTo(projectile, projectile.target);
      const speed = Math.hypot(projectile.vx, projectile.vy);
      projectile.vx = lerp(projectile.vx, Math.cos(angle) * speed, 0.08);
      projectile.vy = lerp(projectile.vy, Math.sin(angle) * speed, 0.08);
    }
    projectile.x += projectile.vx * dt;
    projectile.y += projectile.vy * dt;

    if (projectile.hostile) {
      if (distance(projectile, state.player) < projectile.r + state.player.r && state.player.invuln <= 0) {
        damagePlayer(projectile.damage * (1 - state.echo.damageReduction - state.runGrowth.damageReduction), projectile);
        state.player.invuln = 0.44;
        projectile.life = 0;
      }
    } else {
      const target = projectile.target && projectile.target.hp > 0 ? projectile.target : nearestHostile(projectile.x, projectile.y);
      if (target && distance(projectile, target) < projectile.r + target.r) {
        damageHostile(target, projectile.damage, projectile.source, { bossTrace: true });
        projectile.life = 0;
      }
    }
  }
  state.projectiles = state.projectiles.filter((projectile) => projectile.life > 0 && insideBounds(projectile, 80));
}

function damagePlayer(amount, source = null) {
  const p = state.player;
  const shieldAbsorb = Math.min(p.shield || 0, amount);
  if (shieldAbsorb > 0) {
    p.shield -= shieldAbsorb;
    amount -= shieldAbsorb;
    addFloater("보호", p.x, p.y - 32, "#c8c8c8");
    if (p.shield <= 0) burstAshenGuard(source);
  }
  if (amount > 0) p.hp -= amount;
}

function burstAshenGuard(source = null) {
  const guard = state.memories.find((memory) => memory.id === "ashen_guard" && !memory.forgotten);
  if (!guard) return;
  const radius = 118 * (1 + state.echo.range + state.runGrowth.range);
  for (const target of hostiles()) {
    if (distance(state.player, target) < radius + target.r) {
      damageHostile(target, 30 * (1 + state.runGrowth.damage), guard.id, { pushed: true });
    }
  }
  state.metrics[guard.id].status += 30;
  addBurst(state.player.x, state.player.y, "#c8c8c8", 18, 3.4);
  addFloater("잿빛 파열", state.player.x, state.player.y - 42, "#c8c8c8");
  logEvent("ashen_guard_burst", { source: source?.id || source?.type || null });
}

function updateMemories(dt) {
  const p = state.player;
  basicAttack(dt);

  for (const memory of state.memories) {
    if (memory.forgotten) continue;
    const cdMul = 1 - state.echo.cooldownReduction - state.runGrowth.cooldownReduction;
    memory.cooldownLeft = Math.max(0, memory.cooldownLeft - dt);
    if (memory.id === "hungry_blades") {
      memory.metricsTime = (memory.metricsTime || 0) + dt;
      memory.visualCd = Math.max(0, (memory.visualCd || 0) - dt);
      const focusMul = activeTacticalFocus(memory.id) ? 1.16 : 1;
      const radius = 74 * focusMul * (1 + state.echo.range + state.runGrowth.range);
      let hit = false;
      for (const target of hostiles()) {
        if (distance(p, target) < radius + target.r) {
          damageHostile(target, 7.4 * focusMul * (1 + state.echo.dotDamage + state.runGrowth.damage), memory.id);
          hit = true;
        }
      }
      if (hit) recordPresence(memory.id, dt * 2.2);
      if (hit && memory.visualCd <= 0) {
        memory.visualCd = 0.22;
        state.effects.push({ type: "ripple", x: p.x, y: p.y, r: 24, maxR: radius, life: 0.22, maxLife: 0.22 });
        addBurst(p.x, p.y, "#6ddfd2", 4, 1.5);
      }
      continue;
    }
    if (memory.id === "blood_reflection") continue;

    if (memory.cooldownLeft <= 0) {
      activateMemory(memory);
      memory.cooldownLeft = memory.cooldown * cdMul;
    }
  }
}

function basicAttack() {
  const p = state.player;
  const weapon = state.weapon;
  const interval = Math.max(0.16, weapon.interval / (1 + state.echo.attackSpeed + state.runGrowth.attackSpeed));
  if (p.attackCd > 0) return;

  const target = nearestHostile(p.x, p.y, weapon.range * (1 + state.echo.range + state.runGrowth.range));
  if (!target) return;
  p.attackCd = interval;
  p.facing = angleTo(p, target);

  let damage = weapon.damage * (1 + state.runGrowth.damage);
  if (Math.random() < state.echo.critChance) damage *= 1 + state.echo.critDamage;
  damageHostile(target, damage, "weapon");
  applyWeaponEchoEffects(target, damage);
  state.effects.push({
    type: weapon.id === "greatsword" ? "slash_heavy" : "slash_fast",
    x: p.x,
    y: p.y,
    angle: p.facing,
    r: weapon.range,
    life: 0.16,
    maxLife: 0.16,
  });

  const blood = state.memories.find((memory) => memory.id === "blood_reflection" && !memory.forgotten);
  if (blood) {
    const focusMul = activeTacticalFocus(blood.id) ? 1.2 : 1;
    const chance = (weapon.id === "twin_blades" ? 0.4 : 0.28) + state.echo.extraHitChance + (activeTacticalFocus(blood.id) ? 0.18 : 0);
    if (Math.random() < chance) {
      const extra = 19 * focusMul * (1 + state.echo.onHitDamage + state.runGrowth.damage);
      damageHostile(target, extra, blood.id, { bossTrace: true });
      state.player.hp = Math.min(state.player.maxHp, state.player.hp + 1.8);
      recordPresence(blood.id, 0.8);
      state.metrics[blood.id].activeCount += 1;
      state.effects.push({ type: "blood", x: target.x, y: target.y, r: 10, maxR: 34, life: 0.28, maxLife: 0.28 });
    }
  }
}

function updateDangerMetrics(dt) {
  const danger = state.danger;
  const hpRate = state.player.hp / state.player.maxHp;
  danger.maxEnemies = Math.max(danger.maxEnemies, state.enemies.length);
  danger.currentKillGap = Math.max(0, state.elapsed - danger.lastKillAt);
  danger.maxKillGap = Math.max(danger.maxKillGap, danger.currentKillGap);
  if (hpRate <= 0.4) danger.lowHpTime += dt;
  if (state.enemies.length >= 12) danger.enemyPressureTime += dt;
  if (state.runTimeline.pressurePhaseId === "lull") danger.pressureLullTime += dt;
  if (state.runTimeline.pressurePhaseId === "rising") danger.pressureRiseTime += dt;
  if (state.runTimeline.pressurePhaseId === "climax") danger.pressureClimaxTime += dt;
  if (activeMemoryCount() < 3) {
    danger.deficitTime += dt;
    if (hpRate <= 0.4) danger.deficitLowHpTime += dt;
    if (state.runTimeline.pressurePhaseId === "deficit_breath") danger.deficitBreathTime += dt;
    if (state.runTimeline.pressurePhaseId === "deficit_trial") {
      danger.deficitChallengeTime += dt;
      if (hpRate <= 0.4) danger.deficitChallengeLowHpTime += dt;
    }
  }
}

function recordKillPressure() {
  if (!state?.danger) return;
  const gap = Math.max(0, state.elapsed - state.danger.lastKillAt);
  state.danger.maxKillGap = Math.max(state.danger.maxKillGap, gap);
  state.danger.lastKillAt = state.elapsed;
  state.danger.currentKillGap = 0;
}

function checkPlayerDeath() {
  if (!state || state.player.hp > 0 || state.death) return false;
  state.player.hp = 0;
  state.running = false;
  state.mode = "dead";
  state.death = {
    at: Number(state.elapsed.toFixed(2)),
    phase: state.phase,
    activeMemoryCount: activeMemoryCount(),
    enemyCount: state.enemies.length,
    bossActive: Boolean(state.boss),
    deficit: activeMemoryCount() < 3,
  };
  state.danger.deaths += 1;
  state.danger.deathAt = state.death.at;
  state.danger.deathPhase = state.death.phase;
  state.danger.deathActiveMemoryCount = state.death.activeMemoryCount;
  state.danger.deficitDeath = state.death.deficit;
  addLog("검은 물이 플레이어를 삼켰다.");
  logEvent("player_death", {
    death: state.death,
    danger: dangerLog(),
  });
  updateClarity();
  updateUi();
  showDeathOverlay();
  return true;
}

function applyWeaponEchoEffects(target, baseDamage) {
  if (!target) return;
  const p = state.player;

  if (state.echo.weaponFlashChance && Math.random() < state.echo.weaponFlashChance) {
    const flashTarget = nearestHostile(target.x, target.y, 180, [target]) || target;
    damageHostile(flashTarget, 20 + baseDamage * 0.48, "weapon_echo");
    addFloater("섬광 잔향", flashTarget.x, flashTarget.y - 18, "#eef8ff");
    addBurst(flashTarget.x, flashTarget.y, "#eef8ff", 8, 2.2);
  }

  if (state.echo.weaponBleedDamage) {
    damageHostile(target, 4 + state.echo.weaponBleedDamage * 18, "weapon_echo");
    addFloater("칼무리 잔향", target.x, target.y - 10, "#e8c15d");
  }

  if (state.echo.weaponHomingChance && Math.random() < state.echo.weaponHomingChance) {
    const homingTarget = nearestHostile(p.x, p.y, 420, [target]);
    if (homingTarget) {
      const angle = angleTo(p, homingTarget);
      addProjectile({
        x: p.x,
        y: p.y,
        vx: Math.cos(angle) * 280,
        vy: Math.sin(angle) * 280,
        r: 5,
        damage: 18 + baseDamage * 0.38,
        hostile: false,
        life: 2.5,
        color: "#a98cff",
        source: "weapon_echo",
        trail: true,
      });
      addFloater("추적 잔향", p.x, p.y - 20, "#a98cff");
    }
  }

  if (state.echo.weaponShockwaveChance && Math.random() < state.echo.weaponShockwaveChance) {
    const radius = 82 * (1 + state.echo.range + state.runGrowth.range);
    for (const enemy of state.enemies) {
      const dist = distance(p, enemy);
      if (dist < radius + enemy.r) {
        damageHostile(enemy, 12 + baseDamage * 0.28, "weapon_echo");
        const push = 34 * (1 - dist / (radius + enemy.r));
        const angle = angleTo(p, enemy);
        enemy.x += Math.cos(angle) * push;
        enemy.y += Math.sin(angle) * push;
      }
    }
    addBurst(p.x, p.y, "#6ddfd2", 18, 3.0);
    addFloater("파문 잔향", p.x, p.y - 28, "#6ddfd2");
  }

  if (state.echo.weaponSlowChance && Math.random() < state.echo.weaponSlowChance) {
    target.slow = Math.max(target.slow || 0, 1.2 + state.echo.slowDuration);
    addFloater("초침 잔향", target.x, target.y - 18, "#a98cff");
  }

  if (state.echo.weaponCounterChance && Math.random() < state.echo.weaponCounterChance) {
    const radius = 74 * (1 + state.echo.range + state.runGrowth.range);
    for (const enemy of state.enemies) {
      if (distance(p, enemy) < radius + enemy.r) damageHostile(enemy, 10 + baseDamage * 0.22, "weapon_echo");
    }
    state.player.shield = Math.max(state.player.shield || 0, 8);
    addFloater("보호 잔향", p.x, p.y - 32, "#c8c8c8");
  }

  if (state.echo.weaponMarkChance && Math.random() < state.echo.weaponMarkChance) {
    target.marked = Math.max(target.marked || 0, 1.9 + state.echo.markDuration);
    target.vulnerable = Math.max(target.vulnerable || 0, 1.9 + state.echo.markDuration);
    addFloater("낙인 잔향", target.x, target.y - 20, "#ff7c90");
  }
}

function activateMemory(memory) {
  const p = state.player;
  state.metrics[memory.id].activeCount += 1;
  recordPresence(memory.id, 1);
  logEvent("memory_trigger", { memory: memory.id });

  if (memory.id === "execution_flash") {
    const target = highestThreat();
    if (!target) return;
    const damage = 76 * (state.weapon.id === "greatsword" ? 1.12 : 1) * (1 + state.runGrowth.damage);
    damageHostile(target, damage, memory.id, { bossTrace: true, groggyBonus: true });
    addFloater(memory.name, target.x, target.y - 26, "#eef8ff");
    addBurst(target.x, target.y, "#eef8ff", 18, 4.8);
    state.shake = Math.max(state.shake, 5);
    state.effects.push({ type: "flash", x: target.x, y: target.y, r: 10, maxR: 86, life: 0.42, maxLife: 0.42 });
  }

  if (memory.id === "stalker_oath") {
    const count = 2 + state.echo.projectileCount;
    addFloater(memory.name, p.x, p.y - 24, "#a98cff");
    for (let i = 0; i < count; i += 1) {
      const target = farHostile() || nearestHostile(p.x, p.y);
      const angle = target ? angleTo(p, target) + rand(-0.28, 0.28) : p.facing + rand(-0.35, 0.35);
      const speed = 240 * (1 + state.echo.projectileSpeed);
      addProjectile({
        x: p.x,
        y: p.y,
        vx: Math.cos(angle) * speed,
        vy: Math.sin(angle) * speed,
        r: 6,
        damage: 36 * (1 + state.runGrowth.damage),
        hostile: false,
        source: memory.id,
        target,
        homing: true,
        life: 3.2,
        color: "#a98cff",
      });
    }
  }

  if (memory.id === "shatter_ripple") {
    const radius = 132 * (1 + state.echo.range + state.runGrowth.range);
    addFloater(memory.name, p.x, p.y - 28, "#6ddfd2");
    state.shake = Math.max(state.shake, 4);
    for (const target of hostiles()) {
      const dist = distance(p, target);
      if (dist < radius + target.r) {
        const push = (48 + (state.echo.knockback + state.runGrowth.knockback) * 48) * (1 - dist / (radius + target.r));
        const angle = angleTo(p, target);
        target.x += Math.cos(angle) * push;
        target.y += Math.sin(angle) * push;
        damageHostile(target, 49 * (1 + state.runGrowth.damage), memory.id, { bossTrace: true, pushed: true });
        state.metrics[memory.id].status += push;
      }
    }
    addBurst(p.x, p.y, "#6ddfd2", 16, 3.6);
    state.effects.push({ type: "ripple", x: p.x, y: p.y, r: 14, maxR: radius, life: 0.48, maxLife: 0.48 });
  }

  if (memory.id === "stopped_second") {
    const radius = 150 * (1 + state.echo.range + state.runGrowth.range);
    const duration = 2.4 * (1 + state.echo.slowDuration);
    addFloater(memory.name, p.x, p.y - 28, "#a98cff");
    for (const target of hostiles()) {
      if (distance(p, target) < radius + target.r) {
        target.slow = Math.max(target.slow || 0, duration);
        damageHostile(target, 24 * (1 + state.runGrowth.damage), memory.id, { bossTrace: true, controlHit: true });
        if (target.id === "boss") state.metrics[memory.id].bossControls += 1;
        state.metrics[memory.id].status += 24;
      }
    }
    for (const other of state.memories) {
      if (other.id !== memory.id && !other.forgotten && other.cooldownLeft > 0) {
        other.cooldownLeft *= 0.88;
      }
    }
    addBurst(p.x, p.y, "#a98cff", 12, 2.7);
    state.effects.push({ type: "clock", x: p.x, y: p.y, r: 18, maxR: radius, life: 0.72, maxLife: 0.72 });
  }

  if (memory.id === "ashen_guard") {
    const focusMul = activeTacticalFocus(memory.id) ? 1.25 : 1;
    const shield = Math.round(22 * focusMul * (1 + state.echo.shieldDuration));
    p.maxShield = Math.max(p.maxShield || 0, shield);
    p.shield = Math.max(p.shield || 0, shield);
    state.metrics[memory.id].status += shield;
    addFloater(memory.name, p.x, p.y - 30, "#c8c8c8");
    addBurst(p.x, p.y, "#c8c8c8", 12, 2.4);
  }

  if (memory.id === "oblivion_brand") {
    const target = highestThreat();
    if (!target) return;
    const duration = 4.2 * (1 + state.echo.markDuration);
    target.marked = Math.max(target.marked || 0, duration);
    target.vulnerable = Math.max(target.vulnerable || 0, duration);
    target.slow = Math.max(target.slow || 0, 0.75);
    damageHostile(target, 32 * (1 + state.runGrowth.damage), memory.id, { bossTrace: true, controlHit: true, brandHit: true });
    if (target.id === "boss") state.metrics[memory.id].bossControls += 1;
    state.metrics[memory.id].status += 38;
    addFloater(memory.name, target.x, target.y - 28, "#ff7c90");
    addBurst(target.x, target.y, "#ff7c90", 13, 3.0);
  }
}

function damageHostile(target, amount, source, options = {}) {
  if (target.marked > 0 || target.vulnerable > 0) amount *= 1.14;
  amount = applySynergyDamage(source, target, amount, options);
  const before = target.hp;
  target.hp -= amount;
  const actual = Math.max(0, before - Math.max(0, target.hp));
  if (actual > 0) {
    const color = source === "weapon" ? "#f0f3f8" : source === "execution_flash" ? "#eef8ff" : source === "stalker_oath" ? "#a98cff" : source === "shatter_ripple" ? "#6ddfd2" : source === "blood_reflection" ? "#ff5d6c" : source === "oblivion_brand" ? "#ff7c90" : source === "ashen_guard" ? "#c8c8c8" : "#e8c15d";
    if (actual >= 12 || Math.random() < 0.12) addFloater(String(Math.round(actual)), target.x, target.y - target.r - 6, color);
    if (actual >= 20) addBurst(target.x, target.y, color, source === "weapon" ? 5 : 8, source === "weapon" ? 1.4 : 2.1);
  }
  if (source && state.metrics[source]) {
    state.metrics[source].damage += actual;
    target.hitBy?.add(source);
    if (target.hp <= 0) target.killedBy = source;
    if (target.id === "boss" || target.name === "기억을 씹는 자") {
      state.metrics[source].bossDamage += actual;
      if (state.boss?.groggy || options.groggyBonus) state.metrics[source].groggyDamage += actual;
    }
  }
  applySynergyAfterDamage(source, target, actual);
}

function applySynergyDamage(source, target, amount, options = {}) {
  if (!source || !state?.metrics[source]) return amount;
  const tags = memories[source]?.tags || [];
  let nextAmount = amount;
  if (hasSynergy("area_control") && tags.includes("area") && (target.slow > 0 || options.pushed || options.controlHit)) {
    nextAmount *= 1.22;
  }
  if (hasSynergy("dot_control") && tags.includes("dot") && target.slow > 0) {
    nextAmount *= 1.28;
  }
  if (hasSynergy("brand_chain") && tags.includes("burst") && (target.marked > 0 || target.vulnerable > 0 || options.brandHit)) {
    nextAmount *= 1.26;
  }
  if (activeTacticalFocus(source) && activeSynergiesFor(activeMemoryIds()).some((rule) => rule.tags.some((tag) => tags.includes(tag)))) {
    nextAmount *= 1.12;
  }
  return nextAmount;
}

function applySynergyAfterDamage(source, target, actual) {
  if (!source || !state?.metrics[source] || actual <= 0) return;
  const tags = memories[source]?.tags || [];
  if (hasSynergy("burst_survival") && tags.includes("burst") && actual >= 42) {
    const before = state.player.hp;
    state.player.hp = Math.min(state.player.maxHp, state.player.hp + 5);
    if (state.player.hp > before) addFloater("결의", state.player.x, state.player.y - 34, "#ff5d6c");
  }
}

function grantXp(enemy) {
  if (!state || state.mode !== "combat") return;
  const growth = state.runGrowth;
  const gained = Math.max(1, Math.round(enemy.score * (enemy.child ? 0.55 : 1) * (1 + growth.xpGain)));
  growth.xp += gained;
  if (state.elapsed <= 180) growth.earlyKills += 1;
  addFloater(`+${gained}`, enemy.x, enemy.y - enemy.r - 16, "#72e49b");
  logEvent("enemy_killed", {
    enemy: enemy.id,
    enemyName: enemy.name,
    xp: gained,
    totalXp: growth.xp,
    level: growth.level,
  });
  if (growth.xp >= growth.nextXp) queueLevelUp();
}

function queueLevelUp() {
  const growth = state.runGrowth;
  growth.xp -= growth.nextXp;
  growth.level += 1;
  growth.nextXp = Math.round(growth.nextXp * 1.42 + 4);
  if (!state.bossSpawned) growth.levelUpsBeforeBoss += 1;
  growth.pendingChoices = chooseLevelUpChoices();
  state.mode = "upgrade";
  state.running = false;
  addLog(`${growth.level}레벨. 검은 물이 새 힘을 건넨다.`);
  logEvent("level_up_available", {
    level: growth.level,
    choices: growth.pendingChoices,
    nextXp: growth.nextXp,
  });
  showLevelUpOverlay();
}

function chooseLevelUpChoices() {
  const taken = new Set(state.runGrowth.choicesTaken.map((choice) => choice.id));
  const pool = Object.values(levelUpChoices)
    .sort(() => Math.random() - 0.5)
    .sort((a, b) => Number(taken.has(a.id)) - Number(taken.has(b.id)));
  return pool.slice(0, 3).map((choice) => choice.id);
}

function showLevelUpOverlay() {
  overlay.innerHTML = `
    <div class="panel upgrade-panel">
      <p class="eyebrow">런 성장</p>
      <h2>기억이 피를 먹고 선명해졌다.</h2>
      <p class="panel-copy">이번 런 동안만 유지되는 힘을 하나 고르세요.</p>
      <div id="levelUpChoices" class="choice-list upgrade-list"></div>
    </div>
  `;
  overlay.classList.add("show");
  const container = overlay.querySelector("#levelUpChoices");
  state.runGrowth.pendingChoices.forEach((id) => {
    const choice = levelUpChoices[id];
    const button = document.createElement("button");
    button.className = "choice";
    button.type = "button";
    button.innerHTML = `<strong>${choice.name}</strong><span>${choice.desc}<br>${choice.log}</span>`;
    button.addEventListener("click", () => applyLevelUpChoice(id));
    container.appendChild(button);
  });
}

function applyLevelUpChoice(id) {
  const choice = levelUpChoices[id];
  if (!choice || state.mode !== "upgrade") return;
  choice.apply();
  state.runGrowth.choicesTaken.push({ id, name: choice.name, level: state.runGrowth.level });
  state.runGrowth.pendingChoices = [];
  overlay.classList.remove("show");
  overlay.innerHTML = "";
  state.mode = "combat";
  state.running = true;
  addLog(`${choice.name}: ${choice.log}`);
  addFloater(choice.name, state.player.x, state.player.y - 36, "#72e49b");
  logEvent("level_up_choice", {
    level: state.runGrowth.level,
    choice: id,
    choiceName: choice.name,
    runGrowth: runGrowthLog(),
  });
  renderEchoes();
}

function recordPresence(memoryId, amount) {
  if (state.metrics[memoryId]) state.metrics[memoryId].presenceTime += amount;
}

function defeatBoss() {
  state.mode = "questions";
  state.running = false;
  calculateDependency();
  const defeated = state.boss;
  state.boss = null;
  state.bossSpawned = false;
  addLog("문지기가 쓰러지고, 소리가 강 아래로 빨려 들어갔다.");
  logEvent("boss_defeated", {
    elapsed: Number(state.elapsed.toFixed(2)),
    cycleIndex: defeated?.cycleIndex || state.runTimeline.currentBossIndex,
    final: Boolean(defeated?.final),
  });
  showQuestionOverlay(Boolean(defeated?.final));
}

function calculateDependency() {
  const ids = state.memories.filter((m) => !m.forgotten).map((m) => m.id);
  const relianceValues = ids.map((id) => {
    const m = state.metrics[id];
    return m.damage + m.kills * 30 + m.assists * 8 + m.focusDependency * 12;
  });
  const bossValues = ids.map((id) => {
    const m = state.metrics[id];
    return m.bossDamage + m.groggyDamage * 0.5 + m.bossControls * 18;
  });
  const presenceValues = ids.map((id) => {
    const m = state.metrics[id];
    return m.activeCount * 10 + m.presenceTime * 6;
  });

  ids.forEach((id, index) => {
    const metric = state.metrics[id];
    const reliance = normalize(relianceValues[index], relianceValues);
    const boss = normalize(bossValues[index], bossValues);
    const presence = normalize(presenceValues[index], presenceValues);
    const score = reliance * 0.72 + boss * 0.16 + presence * 0.12;
    metric.components = {
      combat: Math.round(reliance),
      boss: Math.round(boss),
      irreplaceability: Math.round(reliance),
      presence: Math.round(presence),
      focus: Math.round(metric.focusDependency || 0),
    };
    metric.score = Math.round(score);
    metric.deletionScore = Math.round(score);
  });

  const deletionTotal = sum(ids.map((id) => state.metrics[id].deletionScore)) || 1;
  ids.forEach((id) => {
    state.metrics[id].deletionWeight = Number((state.metrics[id].deletionScore / deletionTotal).toFixed(4));
  });
}

function topDependencyCandidates(limit = 2) {
  calculateDependency();
  return state.memories
    .filter((memory) => !memory.forgotten)
    .sort((a, b) => state.metrics[b.id].deletionScore - state.metrics[a.id].deletionScore)
    .slice(0, limit);
}

function forgetMemory(memoryId, forkMeta = null) {
  calculateDependency();
  const ranked = topDependencyCandidates(activeMemoryCount());
  const forgotten = state.memories.find((memory) => memory.id === memoryId && !memory.forgotten) || ranked[0];
  forgotten.forgotten = true;
  state.forgotten = forgotten.id;
  state.forgottenHistory.push({
    id: forgotten.id,
    name: forgotten.name,
    t: Number(state.elapsed.toFixed(2)),
    cycleIndex: state.runTimeline.currentBossIndex,
    activeBefore: ranked.map((memory) => memory.id),
  });
  const echoPowerTier = forkMeta?.rank === 1 ? "strong" : "safe";
  applyEcho(forgotten.id, echoPowerTier);
  refreshActiveSynergies();
  const buildIdentity = buildIdentityFor(activeMemoryIds(), state);
  state.logs.buildIdentity = buildIdentity;
  renderEchoes();
  const cycle = {
    cycleIndex: state.runTimeline.currentBossIndex,
    bossDefeatedAt: Number(state.elapsed.toFixed(2)),
    forgotten: forgotten.id,
    forgottenName: forgotten.name,
    activeBefore: ranked.map((memory) => memory.id),
    activeAfterForget: activeMemoryIds(),
    forkChoice: forkMeta,
    echoUnlock: latestEchoUnlock(),
    refillAvailableAt: null,
    refillChoice: null,
    final: state.runTimeline.currentBossIndex >= state.runTimeline.bossScheduleSec.length,
  };
  state.runTimeline.cycles.push(cycle);
  logEvent("memory_forgotten", {
    cycleIndex: state.runTimeline.currentBossIndex,
    forgotten: forgotten.id,
    forgottenName: forgotten.name,
    score: state.metrics[forgotten.id].score,
    deletionScore: state.metrics[forgotten.id].deletionScore,
    deletionWeight: state.metrics[forgotten.id].deletionWeight,
    deletionWeights: dependencyWeights(),
    questions: state.questions,
    questionNames: questionNames(),
    forkChoice: forkMeta,
    echoUnlock: latestEchoUnlock(),
    predictionAccuracy: predictionAccuracyLog(),
    echo: state.echo,
    echoTransformation: echoTransformationLog(forgotten.id),
    activeMemoryCount: activeMemoryCount(),
    buildIdentity,
  });
  return forgotten;
}

function forgetMostDependent() {
  const chosen = topDependencyCandidates(1)[0];
  const forkChoice = {
    cycleIndex: state.runTimeline.currentBossIndex,
    t: Number(state.elapsed.toFixed(2)),
    chosenMemoryId: chosen?.id || null,
    chosenMemoryName: chosen?.name || null,
    rank: 1,
    candidateIds: chosen ? [chosen.id] : [],
    candidateNames: chosen ? [chosen.name] : [],
    predictedMemoryId: state.questions.predict,
    predictedMemoryName: state.questions.predict === "unknown" ? "모르겠다" : memories[state.questions.predict]?.name || null,
    strongPivot: true,
    automated: true,
  };
  state.questions.release = chosen?.id || null;
  state.forkChoice = forkChoice;
  return forgetMemory(chosen?.id, forkChoice);
}

function applyEcho(memoryId, tier = "safe") {
  const tags = memories[memoryId]?.tags || [];
  const powerMul = tier === "strong" ? 1.55 : 0.82;
  state.tagEchoes.push({
    memoryId,
    memoryName: memories[memoryId]?.name || memoryId,
    tags: tags.slice(0, 1),
    power: tier === "strong" ? 0.55 : 0.28,
    tier,
  });
  if (memoryId === "execution_flash") {
    state.echo.critChance += 0.05 * experiment.echoPower * powerMul;
    state.echo.critDamage += 0.12 * experiment.echoPower * powerMul;
    state.echo.weaponFlashChance += 0.09 * experiment.echoPower * powerMul;
  }
  if (memoryId === "hungry_blades") {
    state.echo.attackSpeed += 0.07 * experiment.echoPower * powerMul;
    state.echo.dotDamage += 0.13 * experiment.echoPower * powerMul;
    state.echo.weaponBleedDamage += 0.36 * experiment.echoPower * powerMul;
  }
  if (memoryId === "stalker_oath") {
    state.echo.projectileCount += 0.42 * experiment.echoPower * powerMul;
    state.echo.projectileSpeed += 0.11 * experiment.echoPower * powerMul;
    state.echo.weaponHomingChance += 0.10 * experiment.echoPower * powerMul;
  }
  if (memoryId === "shatter_ripple") {
    state.echo.range += 0.07 * experiment.echoPower * powerMul;
    state.echo.knockback += 0.11 * experiment.echoPower * powerMul;
    state.echo.damageReduction += 0.025 * experiment.echoPower * powerMul;
    state.echo.weaponShockwaveChance += 0.09 * experiment.echoPower * powerMul;
  }
  if (memoryId === "blood_reflection") {
    state.echo.extraHitChance += 0.05 * experiment.echoPower * powerMul;
    state.echo.onHitDamage += 0.10 * experiment.echoPower * powerMul;
    state.echo.weaponBleedDamage += 0.14 * experiment.echoPower * powerMul;
  }
  if (memoryId === "stopped_second") {
    state.echo.cooldownReduction += 0.05 * experiment.echoPower * powerMul;
    state.echo.slowDuration += 0.14 * experiment.echoPower * powerMul;
    state.echo.weaponSlowChance += 0.10 * experiment.echoPower * powerMul;
  }
  if (memoryId === "ashen_guard") {
    state.echo.shieldDuration += 0.12 * experiment.echoPower * powerMul;
    state.echo.range += 0.035 * experiment.echoPower * powerMul;
    state.echo.weaponCounterChance += 0.10 * experiment.echoPower * powerMul;
  }
  if (memoryId === "oblivion_brand") {
    state.echo.markDuration += 0.12 * experiment.echoPower * powerMul;
    state.echo.critDamage += 0.08 * experiment.echoPower * powerMul;
    state.echo.weaponMarkChance += 0.10 * experiment.echoPower * powerMul;
  }
  state.echoUnlocks.push(echoUnlockFor(memoryId, tier));
}

function echoUnlockFor(memoryId, tier = "safe") {
  const strong = tier === "strong";
  const names = {
    execution_flash: ["섬광 처형 경로", "무기 치명/섬광이 새 폭딜 축을 엽니다."],
    hungry_blades: ["출혈 난무 경로", "빠른 타격과 무기 출혈로 지속 절삭 축을 엽니다."],
    stalker_oath: ["잔탄 추적 경로", "무기 유도탄과 투사체 수로 카이팅 축을 엽니다."],
    shatter_ripple: ["반향 파문 경로", "충격파와 넉백으로 광역 제어 축을 엽니다."],
    blood_reflection: ["피의 온힛 경로", "추가타와 무기 출혈로 평타 피벗을 엽니다."],
    stopped_second: ["정지 둔화 경로", "무기 둔화와 쿨다운으로 제어 피벗을 엽니다."],
    ashen_guard: ["잿빛 반격 경로", "보호막과 무기 반격으로 결손 생존 피벗을 엽니다."],
    oblivion_brand: ["각인 처형 경로", "낙인과 단발 증폭으로 보스 딜타임 피벗을 엽니다."],
  };
  const [name, desc] = names[memoryId] || ["잔향 경로", "사라진 기억의 전투 성향이 새 길을 엽니다."];
  return {
    memoryId,
    memoryName: memories[memoryId]?.name || memoryId,
    tier,
    name,
    desc: strong ? `${desc} 강한 잔향으로 열린 경로입니다.` : `${desc} 안전한 잔향이라 경로는 좁게 열립니다.`,
    power: strong ? "strong" : "safe",
  };
}

function latestEchoUnlock() {
  return state.echoUnlocks[state.echoUnlocks.length - 1] || null;
}

function showQuestionOverlay(finalCycle = false) {
  state.questions = { protect: null, predict: null, release: null };
  const candidates = topDependencyCandidates(2);
  overlay.innerHTML = `
    <div class="panel question-panel fork-panel">
      <p class="eyebrow">망각 갈림길</p>
      <h2>레테가 가장 선명한 두 기억을 내밀었다.</h2>
      <p class="panel-copy">먼저 무엇이 떠날지 예측한 뒤, 둘 중 하나를 직접 떠나보내세요. 가장 의존한 기억을 놓아주면 강한 잔향 경로가 열리고, 2순위를 놓아주면 익숙함은 남지만 새 길은 좁게 열립니다.</p>
      <div class="question-block">
        <h3>레테가 가져갈 것 같았던 기억은?</h3>
        <div id="predictChoices" class="pill-row"></div>
      </div>
      <div class="fork-choice-grid" id="forkChoices"></div>
      <button id="submitQuestionsButton" class="primary-btn" type="button" disabled>이 기억을 떠나보내기</button>
    </div>
  `;
  overlay.classList.add("show");
  const predict = overlay.querySelector("#predictChoices");
  const forkChoices = overlay.querySelector("#forkChoices");
  const submit = overlay.querySelector("#submitQuestionsButton");

  activeMemories().forEach((memory) => {
    predict.appendChild(questionButton(memory.name, () => {
      state.questions.predict = memory.id;
      selectPill(predict, memory.name);
      submit.disabled = !state.questions.predict || !state.questions.release;
    }));
  });
  predict.appendChild(questionButton("모르겠다", () => {
    state.questions.predict = "unknown";
    selectPill(predict, "모르겠다");
    submit.disabled = !state.questions.predict || !state.questions.release;
  }));

  candidates.forEach((memory, index) => {
    const rank = index + 1;
    const metric = state.metrics[memory.id];
    const unlock = echoUnlockFor(memory.id, rank === 1 ? "strong" : "safe");
    const button = document.createElement("button");
    button.className = `choice fork-choice ${rank === 1 ? "strong-fork" : "safe-fork"}`;
    button.type = "button";
    button.innerHTML = `
      <strong>${rank}위: ${memory.name}</strong>
      <span>의존도 ${metric.deletionScore} · ${rank === 1 ? "강한 잔향 / 새 경로 크게 열림" : "약한 잔향 / 익숙함 유지"}</span>
      <div class="fork-preview">${unlock.name}<br>${unlock.desc}</div>
    `;
    button.addEventListener("click", () => {
      state.questions.release = memory.id;
      [...forkChoices.children].forEach((child) => child.classList.toggle("selected", child === button));
      submit.disabled = !state.questions.predict || !state.questions.release;
    });
    forkChoices.appendChild(button);
  });

  submit.addEventListener("click", () => {
    const ranked = topDependencyCandidates(activeMemoryCount());
    const release = state.questions.release || ranked[0].id;
    const rank = ranked.findIndex((memory) => memory.id === release) + 1 || 1;
    const forkChoice = {
      cycleIndex: state.runTimeline.currentBossIndex,
      t: Number(state.elapsed.toFixed(2)),
      chosenMemoryId: release,
      chosenMemoryName: memories[release].name,
      rank,
      candidateIds: candidates.map((memory) => memory.id),
      candidateNames: candidates.map((memory) => memory.name),
      predictedMemoryId: state.questions.predict,
      predictedMemoryName: state.questions.predict === "unknown" ? "모르겠다" : memories[state.questions.predict]?.name || null,
      strongPivot: rank === 1,
    };
    state.forkChoice = forkChoice;
    const forgotten = forgetMemory(release, forkChoice);
    if (finalCycle) {
      showResultOverlay();
    } else {
      showCycleResultOverlay(forgotten);
    }
  });
}

function showCycleResultOverlay(forgotten) {
  const cycle = state.runTimeline.cycles[state.runTimeline.cycles.length - 1];
  const refillAt = state.elapsed + experiment.deficitDurationSec;
  state.runTimeline.deficitStartedAt = Number(state.elapsed.toFixed(2));
  state.runTimeline.refillAvailableAt = refillAt;
  if (cycle) cycle.refillAvailableAt = Number(refillAt.toFixed(2));

  overlay.innerHTML = `
    <div class="panel result-panel">
      <p class="eyebrow">망각 사이클 ${state.runTimeline.currentBossIndex}</p>
      <h2>${forgotten.name} 기억이 가라앉았다.</h2>
      <div class="result-summary">
        <div class="result-card loss-card"><strong>사라진 행동</strong><br>${forgotten.name} 자동 발동이 멈춥니다.</div>
        ${echoTransformationHtml(forgotten)}
        ${echoUnlockHtml(latestEchoUnlock())}
        <div class="result-card"><strong>결손 생존</strong><br>${Math.round(experiment.deficitDurationSec)}초 동안 기억 ${activeMemoryCount()}개로 버틴 뒤 새 기억을 고릅니다.</div>
      </div>
      <button id="continueCycleButton" class="primary-btn" type="button">결손 구간 시작</button>
    </div>
  `;
  overlay.classList.add("show");
  overlay.querySelector("#continueCycleButton").addEventListener("click", () => {
    overlay.classList.remove("show");
    overlay.innerHTML = "";
    state.mode = "combat";
    state.running = true;
    state.phase = "결손 생존";
    const challenge = {
      cycleIndex: state.runTimeline.currentBossIndex,
      startedAt: Number(state.elapsed.toFixed(2)),
      challengeStartedAt: null,
      refillAvailableAt: Number(refillAt.toFixed(2)),
      completedAt: null,
      survived: false,
      forgotten: forgotten.id,
      forgottenName: forgotten.name,
      activeMemoryIds: activeMemoryIds(),
      activeMemoryNames: activeMemories().map((memory) => memory.name),
      segments: [],
    };
    state.runTimeline.postLossChallenges.push(challenge);
    if (cycle) cycle.postLossChallenge = challenge;
    addLog(`${forgotten.name}의 잔향만 남았다. 기억 ${activeMemoryCount()}개로 버텨라.`);
    logEvent("deficit_started", {
      cycleIndex: state.runTimeline.currentBossIndex,
      refillAvailableAt: Number(refillAt.toFixed(2)),
      activeMemoryIds: activeMemoryIds(),
      postLossChallenge: challenge,
    });
  });
}

function showRefillOverlay() {
  const candidates = refillCandidates();
  state.mode = "refill";
  state.running = false;
  overlay.innerHTML = `
    <div class="panel question-panel">
      <p class="eyebrow">기억 보충</p>
      <h2>망가진 빌드가 새 기억을 붙잡았다.</h2>
      <p class="panel-copy">새 기억 하나를 골라 다시 3개 슬롯으로 돌아갑니다. 이전 빌드는 복구되지 않고, 잔향 위에 새 방향이 붙습니다.</p>
      <div id="refillChoices" class="choice-list memory-list"></div>
    </div>
  `;
  overlay.classList.add("show");
  const container = overlay.querySelector("#refillChoices");
  candidates.forEach((id) => {
    const memory = memories[id];
    const button = document.createElement("button");
    button.className = "choice";
    button.type = "button";
    button.innerHTML = memoryChoiceHtml(memory);
    button.addEventListener("click", () => applyMemoryRefill(id));
    container.appendChild(button);
  });
  logEvent("memory_refill_available", {
    cycleIndex: state.runTimeline.currentBossIndex,
    candidates,
    candidateNames: candidates.map((id) => memories[id].name),
  });
}

function applyMemoryRefill(id) {
  if (activeMemoryIds().includes(id)) return;
  const memory = createMemoryInstance(id);
  memory.joinedAt = Number(state.elapsed.toFixed(2));
  state.memories.push(memory);
  if (!state.metrics[id]) state.metrics[id] = createMetricSeed([id])[id];
  refreshActiveSynergies();
  const buildIdentity = buildIdentityFor(activeMemoryIds(), state);
  state.logs.buildIdentity = buildIdentity;
  const choice = {
    cycleIndex: state.runTimeline.currentBossIndex,
    t: Number(state.elapsed.toFixed(2)),
    memoryId: id,
    memoryName: memories[id].name,
    activeAfter: activeMemoryIds(),
    buildIdentity,
  };
  state.runTimeline.refillChoices.push(choice);
  const cycle = state.runTimeline.cycles[state.runTimeline.cycles.length - 1];
  if (cycle) {
    cycle.refillChoice = choice;
    cycle.activeAfterRefill = activeMemoryIds();
  }
  overlay.classList.remove("show");
  overlay.innerHTML = "";
  state.mode = "combat";
  state.running = true;
  state.phase = "전투";
  addLog(`${memories[id].name} 기억이 빈 슬롯을 채웠다.`);
  addFloater("기억 보충", state.player.x, state.player.y - 34, "#6ddfd2");
  logEvent("memory_refilled", choice);
  logEvent("build_identity_updated", { buildIdentity });
  updateUi();
  renderMemorySlots();
}

function questionButton(label, onClick) {
  const button = document.createElement("button");
  button.className = "pill";
  button.type = "button";
  button.textContent = label;
  button.addEventListener("click", onClick);
  return button;
}

function selectPill(container, text) {
  [...container.children].forEach((button) => button.classList.toggle("selected", button.textContent === text));
}

function activeMemories() {
  return state.memories.filter((memory) => !memory.forgotten);
}

function activeMemoryIds() {
  return activeMemories().map((memory) => memory.id);
}

function activeMemoryCount() {
  return activeMemoryIds().length;
}

function refillCandidates() {
  const active = new Set(activeMemoryIds());
  const used = new Set(state.memories.map((memory) => memory.id));
  const fresh = Object.keys(memories).filter((id) => !active.has(id) && !used.has(id));
  const recycled = Object.keys(memories).filter((id) => !active.has(id) && used.has(id));
  const latestUnlock = latestEchoUnlock();
  const desiredTags = latestUnlock ? memories[latestUnlock.memoryId]?.tags || [] : [];
  return fresh
    .concat(recycled)
    .sort((a, b) => synergyFitScore(b, desiredTags) - synergyFitScore(a, desiredTags))
    .slice(0, 3);
}

function synergyFitScore(memoryId, desiredTags = []) {
  const ids = activeMemoryIds().concat(memoryId);
  const synergies = activeSynergiesFor(ids).length * 20;
  const tagFit = (memories[memoryId]?.tags || []).filter((tag) => desiredTags.includes(tag)).length * 6;
  return synergies + tagFit;
}

function showResultOverlay() {
  const template = document.getElementById("resultTemplate");
  overlay.innerHTML = "";
  overlay.appendChild(template.content.cloneNode(true));
  overlay.classList.add("show");

  const forgotten = memories[state.forgotten];
  const predicted = state.questions.predict === "unknown" ? null : memories[state.questions.predict];
  const predictionText = predicted
    ? predicted.id === forgotten.id
      ? `예측 성공: ${predicted.name} 기억`
      : `예측 실패: ${predicted.name}을 예상했지만 실제로는 ${forgotten.name} 기억이 사라졌습니다.`
    : `예측 보류: ${forgotten.name} 기억이 사라졌습니다.`;
  overlay.querySelector("#forgottenTitle").textContent = `${forgotten.name} 기억이 망각되었습니다.`;
  overlay.querySelector("#resultSummary").innerHTML = `
    <div class="result-card loss-card"><strong>사라진 행동</strong><br>${forgotten.name} 자동 발동이 멈춥니다.<br><small>${forgotten.desc}</small></div>
    <div class="result-card"><strong>예측 결과</strong><br>${predictionText}</div>
    <div class="result-card"><strong>갈림길 선택</strong><br>${forkChoiceText()}</div>
    <div class="result-card"><strong>삭제 weight</strong><br>${deletionWeightText()}</div>
    ${echoTransformationHtml(forgotten)}
    ${echoUnlockHtml(latestEchoUnlock())}
    <div class="result-card"><strong>이어지는 방향</strong><br>${forgotten.direction}</div>
    ${runConclusionHtml()}
  `;
  overlay.querySelector("#detailTable").innerHTML = dependencyTableHtml();
  renderScale("sadnessScale", "sadness");
  renderScale("fairnessScale", "fairness");
  const recallInput = overlay.querySelector("#memoryRecallInput");
  if (recallInput) {
    recallInput.addEventListener("input", () => {
      state.survey.memoryRecall = recallInput.value.trim();
      logEvent("memory_recall_update", { memoryRecall: state.survey.memoryRecall });
    });
  }

  const downloadButton = overlay.querySelector("#downloadLogButton");
  downloadButton.disabled = true;
  downloadButton.addEventListener("click", downloadLog);
  overlay.querySelector("#restartButton").addEventListener("click", () => {
    window.location.reload();
  });
}

function dependencyTableHtml() {
  const header = `<div class="detail-row"><span>기억</span><span>의존</span><span>보스</span><span>집중</span><span>존재</span><span>점수</span></div>`;
  const rows = state.memories
    .map((memory) => {
      const metric = state.metrics[memory.id];
      const c = metric.components;
      return `<div class="detail-row"><span>${memory.name}</span><span>${c.combat}</span><span>${c.boss}</span><span>${c.focus || 0}</span><span>${c.presence}</span><span>${metric.deletionScore}</span></div>`;
    })
    .join("");
  return header + rows;
}

function dependencyWeights() {
  const weights = {};
  state.memories.forEach((memory) => {
    const metric = state.metrics[memory.id];
    weights[memory.id] = {
      name: memory.name,
      score: metric.score,
      deletionScore: metric.deletionScore,
      deletionWeight: metric.deletionWeight,
      components: metric.components,
    };
  });
  return weights;
}

function echoTransformationHtml(memory) {
  const transform = echoTransformationLog(memory.id);
  const badges = transform.stats.map((stat) => `<span>${stat}</span>`).join("");
  return `
    <div class="result-card echo-transform-card">
      <strong>잔향 변형</strong>
      <p>${transform.summary}</p>
      <div class="echo-badges">${badges}</div>
      <small>이번 실험 echo 배율: ${Math.round(experiment.echoPower * 100)}%</small>
    </div>
  `;
}

function echoUnlockHtml(unlock) {
  if (!unlock) return "";
  return `
    <div class="result-card echo-unlock-card">
      <strong>열린 새 경로</strong>
      <p>${unlock.name}: ${unlock.desc}</p>
      <small>${unlock.power === "strong" ? "가장 의존한 기억을 놓아준 강한 피벗입니다." : "익숙함을 남긴 안전한 피벗입니다."}</small>
    </div>
  `;
}

function echoTransformationLog(memoryId) {
  const map = {
    execution_flash: {
      lost: "가장 위협적인 적을 찍어 누르던 백색 강타",
      summary: "큰 한 방은 사라지고, 무기 공격에 작은 섬광 연쇄가 붙어 결손 구간의 마무리 힘을 보탭니다.",
      stats: ["치명률", "치명 피해", "무기 섬광"],
    },
    hungry_blades: {
      lost: "가까운 적을 계속 긁던 칼무리 오라",
      summary: "상시 오라는 사라지고, 무기 타격마다 칼무리 잔흔이 남아 쫄몹을 계속 갉아먹습니다.",
      stats: ["공격 속도", "무기 출혈", "근접 유지"],
    },
    stalker_oath: {
      lost: "멀리 있는 표적을 쫓던 유도 투사체",
      summary: "추적 발사는 사라지고, 무기 타격 중 일부가 보라색 잔탄을 불러 다른 적을 쫓습니다.",
      stats: ["투사체 수", "무기 유도탄", "카이팅"],
    },
    shatter_ripple: {
      lost: "주변을 밀어내던 충격파",
      summary: "즉발 파문은 사라지고, 무기 타격 중 일부가 작은 충격파로 번져 포위를 밀어냅니다.",
      stats: ["범위", "무기 파문", "피해 감소"],
    },
    blood_reflection: {
      lost: "기본 공격마다 되돌아오던 붉은 추가타",
      summary: "반사 추가타는 사라지고, 평타 빌드 전체에 피의 반동이 얇게 남습니다.",
      stats: ["추가타 확률", "온힛 피해", "빠른 타격"],
    },
    stopped_second: {
      lost: "주변 시간을 늦추던 시간 균열",
      summary: "즉시 둔화 장은 사라지고, 무기 타격에 짧은 정지감이 남아 추격을 늦춥니다.",
      stats: ["쿨다운", "무기 둔화", "제어 운용"],
    },
    ashen_guard: {
      lost: "공격을 받아내던 잿빛 보호막",
      summary: "보호막 발동은 사라지고, 무기 타격과 결손 생존에 반격 잔향이 남습니다.",
      stats: ["보호막", "무기 반격", "광역 생존"],
    },
    oblivion_brand: {
      lost: "위협 적을 약화하던 망각의 낙인",
      summary: "능동 낙인은 사라지고, 무기 타격에 짧은 표식과 폭딜 증폭의 길이 남습니다.",
      stats: ["낙인", "무기 표식", "폭딜 제어"],
    },
  };
  return map[memoryId] || {
    lost: "사라진 기억",
    summary: memories[memoryId]?.echo || "잔향이 남았습니다.",
    stats: [memories[memoryId]?.role || "잔향"],
  };
}

function forkChoiceText() {
  const choice = state.forkChoice;
  if (!choice) return "갈림길 기록 없음";
  const rankText = choice.rank === 1 ? "1순위 기억을 놓아 강한 경로를 열었습니다." : "2순위 기억을 놓아 익숙함을 남겼습니다.";
  return `${choice.chosenMemoryName} 선택. ${rankText}`;
}

function predictionAccuracyLog() {
  const predicted = state.questions.predict;
  const released = state.questions.release || state.forgotten;
  return {
    predictedMemoryId: predicted,
    predictedMemoryName: predicted === "unknown" ? "모르겠다" : memories[predicted]?.name || null,
    releasedMemoryId: released,
    releasedMemoryName: memories[released]?.name || null,
    matched: predicted !== "unknown" && predicted === released,
    unknown: predicted === "unknown",
  };
}

function deletionWeightText() {
  return state.memories
    .slice()
    .sort((a, b) => state.metrics[b.id].deletionScore - state.metrics[a.id].deletionScore)
    .map((memory) => `${memory.name} ${Math.round((state.metrics[memory.id].deletionWeight || 0) * 100)}%`)
    .join(" / ");
}

function questionNames() {
  return {
    protect: memories[state.questions.protect]?.name || null,
    predict: state.questions.predict === "unknown" ? "모르겠다" : memories[state.questions.predict]?.name || null,
  };
}

function renderScale(containerId, key) {
  const container = overlay.querySelector(`#${containerId}`);
  for (let value = 0; value <= 4; value += 1) {
    const button = document.createElement("button");
    button.className = "scale-btn";
    button.type = "button";
    button.textContent = String(value);
    button.addEventListener("click", () => {
      state.survey[key] = value;
      [...container.children].forEach((child) => child.classList.toggle("selected", child === button));
      logEvent("survey_update", { survey: state.survey });
      updateDownloadEnabled();
    });
    container.appendChild(button);
  }
}

function updateDownloadEnabled() {
  const button = overlay.querySelector("#downloadLogButton");
  if (!button) return;
  button.disabled = state.survey.sadness === null || state.survey.fairness === null;
}

function collectLogPayload() {
  calculateDependency();
  state.logs.playtest = currentPlaytestMeta();
  state.logs.completedAt = new Date().toISOString();
  state.logs.elapsed = Number(state.elapsed.toFixed(2));
  state.logs.questions = state.questions;
  state.logs.questionNames = questionNames();
  state.logs.forkChoice = state.forkChoice;
  state.logs.predictionAccuracy = predictionAccuracyLog();
  state.logs.forgotten = state.forgotten;
  state.logs.forgottenName = memories[state.forgotten]?.name || state.forgotten;
  state.logs.survey = state.survey;
  state.logs.metrics = state.metrics;
  state.logs.deletionWeights = dependencyWeights();
  state.logs.echo = state.echo;
  state.logs.tagEchoes = state.tagEchoes;
  state.logs.echoUnlocks = state.echoUnlocks;
  state.logs.activeSynergies = state.activeSynergies;
  state.logs.echoPower = experiment.echoPower;
  state.logs.runGrowth = runGrowthLog();
  state.logs.runTimeline = runTimelineLog();
  state.logs.forgottenHistory = state.forgottenHistory;
  state.logs.echoTransformation = state.forgotten ? echoTransformationLog(state.forgotten) : null;
  state.logs.death = state.death;
  state.logs.danger = dangerLog();
  state.logs.buildIdentity = buildIdentityFor(activeMemoryIds(), state);
  state.logs.buildIdentitySeenBy90Sec = Boolean(state.logs.buildIdentitySeenBy90Sec);
  state.logs.tacticalFocus = tacticalFocusLog();
  return state.logs;
}

function runConclusionHtml() {
  const active = activeMemories();
  const longest = state.memories
    .slice()
    .sort((a, b) => (a.joinedAt || 0) - (b.joinedAt || 0))[0];
  const mostDependent = state.memories
    .slice()
    .sort((a, b) => (state.metrics[b.id]?.score || 0) - (state.metrics[a.id]?.score || 0))[0];
  const strongestEcho = state.forgottenHistory[state.forgottenHistory.length - 1];
  return `
    <div class="result-card"><strong>런 결산</strong><br>
      오래 붙잡은 기억: ${longest?.name || "-"}<br>
      가장 의존한 기억: ${mostDependent?.name || "-"}<br>
      마지막 잔향: ${strongestEcho?.name || "-"}<br>
      마지막 슬롯: ${active.map((memory) => memory.name).join(" / ") || "-"}
    </div>
  `;
}

function runGrowthLog() {
  const growth = state.runGrowth;
  return {
    level: growth.level,
    xp: growth.xp,
    nextXp: growth.nextXp,
    damage: growth.damage,
    attackSpeed: growth.attackSpeed,
    cooldownReduction: growth.cooldownReduction,
    range: growth.range,
    knockback: growth.knockback,
    damageReduction: growth.damageReduction,
    xpGain: growth.xpGain,
    choicesTaken: growth.choicesTaken,
    earlyKills: growth.earlyKills,
    maxEnemies: growth.maxEnemies,
    levelUpsBeforeBoss: growth.levelUpsBeforeBoss,
  };
}

function runTimelineLog() {
  const timeline = state.runTimeline;
  return {
    version: timeline.version,
    totalRunSec: timeline.totalRunSec,
    bossScheduleSec: timeline.bossScheduleSec,
    nextBossIndex: timeline.nextBossIndex,
    currentBossIndex: timeline.currentBossIndex,
    deficitDurationSec: timeline.deficitDurationSec,
    deficitStartedAt: timeline.deficitStartedAt,
    refillAvailableAt: timeline.refillAvailableAt,
    cycles: timeline.cycles,
    refillChoices: timeline.refillChoices,
    pressureSegments: timeline.pressureSegments,
    postLossChallenges: timeline.postLossChallenges,
    tacticalFocus: tacticalFocusLog(),
    activeMemoryIds: activeMemoryIds(),
    activeMemoryNames: activeMemories().map((memory) => memory.name),
  };
}

function tacticalFocusLog() {
  const focus = state.tacticalFocus;
  return {
    cooldownSec: focus.cooldownSec,
    cooldownLeft: Number(focus.cooldownLeft.toFixed(2)),
    durationLeft: Number(focus.durationLeft.toFixed(2)),
    memoryId: focus.memoryId,
    memoryName: memories[focus.memoryId]?.name || null,
    dependencyBonus: focus.dependencyBonus,
    useCount: focus.useCount,
    successfulCount: focus.successfulCount,
    lastUsedAt: focus.lastUsedAt,
    history: focus.history,
  };
}

function dangerLog() {
  const danger = state.danger;
  return {
    deaths: danger.deaths,
    deathAt: danger.deathAt,
    deathPhase: danger.deathPhase,
    deathActiveMemoryCount: danger.deathActiveMemoryCount,
    lowHpTime: Number(danger.lowHpTime.toFixed(2)),
    enemyPressureTime: Number(danger.enemyPressureTime.toFixed(2)),
    maxEnemies: danger.maxEnemies,
    maxKillGap: Number(danger.maxKillGap.toFixed(2)),
    currentKillGap: Number(danger.currentKillGap.toFixed(2)),
    deficitTime: Number(danger.deficitTime.toFixed(2)),
    deficitLowHpTime: Number(danger.deficitLowHpTime.toFixed(2)),
    deficitDeath: danger.deficitDeath,
    deficitBreathTime: Number(danger.deficitBreathTime.toFixed(2)),
    deficitChallengeTime: Number(danger.deficitChallengeTime.toFixed(2)),
    deficitChallengeLowHpTime: Number(danger.deficitChallengeLowHpTime.toFixed(2)),
    postLossChallengeCompletions: danger.postLossChallengeCompletions,
    pressureLullTime: Number(danger.pressureLullTime.toFixed(2)),
    pressureRiseTime: Number(danger.pressureRiseTime.toFixed(2)),
    pressureClimaxTime: Number(danger.pressureClimaxTime.toFixed(2)),
  };
}

function showDeathOverlay() {
  overlay.innerHTML = `
    <div class="panel result-panel">
      <p class="eyebrow">런 종료</p>
      <h2>검은 물이 플레이어를 삼켰다.</h2>
      <div class="result-summary">
        <div class="result-card loss-card"><strong>사망 시각</strong><br>${formatTime(state.death.at)}</div>
        <div class="result-card"><strong>구간</strong><br>${state.death.phase || "-"}</div>
        <div class="result-card"><strong>위험도</strong><br>최대 적 ${state.danger.maxEnemies} / 최대 처치 공백 ${state.danger.maxKillGap.toFixed(1)}초</div>
        <div class="result-card"><strong>기억 수</strong><br>${state.death.activeMemoryCount}개 상태에서 사망</div>
      </div>
      <div class="button-row">
        <button id="downloadLogButton" class="secondary-btn" type="button">JSON 로그 다운로드</button>
        <button id="restartButton" class="primary-btn" type="button">다시 테스트</button>
      </div>
    </div>
  `;
  overlay.classList.add("show");
  overlay.querySelector("#downloadLogButton").addEventListener("click", downloadLog);
  overlay.querySelector("#restartButton").addEventListener("click", () => {
    window.location.reload();
  });
}

function downloadLog() {
  const payload = collectLogPayload();
  if (experiment.qaFastMode) {
    window.__letheLastDownloadedLog = JSON.parse(JSON.stringify(payload));
    document.documentElement.dataset.letheQaLog = JSON.stringify(payload);
  }
  const blob = new Blob([JSON.stringify(payload, null, 2)], { type: "application/json" });
  const a = document.createElement("a");
  a.href = URL.createObjectURL(blob);
  a.download = logFileName(payload);
  a.click();
  setTimeout(() => URL.revokeObjectURL(a.href), 1000);
}

function currentPlaytestMeta() {
  return {
    testerId: sanitizeMetaValue(ui.testerIdInput?.value || playtestMeta.testerId),
    sessionId: sanitizeMetaValue(ui.sessionIdInput?.value || playtestMeta.sessionId),
  };
}

function readPlaytestMetaFromUrl() {
  const params = new URLSearchParams(window.location.search);
  return {
    testerId: sanitizeMetaValue(params.get("tester") || params.get("testerId") || ""),
    sessionId: sanitizeMetaValue(params.get("session") || params.get("sessionId") || ""),
  };
}

function sanitizeMetaValue(value) {
  return String(value || "").trim().slice(0, 40);
}

function safeFilePart(value) {
  return sanitizeMetaValue(value).replace(/[^a-z0-9가-힣_-]+/gi, "-").replace(/^-+|-+$/g, "") || "";
}

function logFileName(payload) {
  const tester = safeFilePart(payload.playtest?.testerId);
  const session = safeFilePart(payload.playtest?.sessionId);
  const parts = ["lethe", experiment.version, tester, session, "log", String(Date.now())].filter(Boolean);
  return `${parts.join("-")}.json`;
}

function renderScaleState() {
  return null;
}

function updateEffects(dt) {
  for (const effect of state.effects) {
    effect.life -= dt;
    if (effect.type === "hostile_aoe") {
      effect.r = lerp(effect.r, effect.maxR, 0.08);
      if (!effect.hit && effect.life < 0.28 && distance(effect, state.player) < effect.r + state.player.r) {
        effect.hit = true;
        damagePlayer(effect.damage * (1 - state.echo.damageReduction), effect);
      }
    }
  }
  state.effects = state.effects.filter((effect) => effect.life > 0);
  for (const floater of state.floaters) {
    floater.life -= dt;
    floater.y += floater.vy * dt;
    floater.x += floater.vx * dt;
  }
  state.floaters = state.floaters.filter((floater) => floater.life > 0);
  state.shake = Math.max(0, state.shake - dt * 18);
}

function updateClarity() {
  calculateDependency();
  const maxScore = Math.max(1, ...state.memories.map((memory) => state.metrics[memory.id].score));
  state.memories.forEach((memory) => {
    memory.clarity = clamp(state.metrics[memory.id].score / maxScore, 0, 1);
  });
  state.logs.buildIdentity = buildIdentityFor(activeMemoryIds(), state);
  if (state.elapsed <= 180) state.logs.buildIdentitySeenBy90Sec = true;
}

function updateUi() {
  const growth = state.runGrowth;
  const xpText = `Lv.${growth.level} ${growth.xp}/${growth.nextXp}`;
  ui.phaseLabel.textContent = state.boss ? `보스 ${state.boss.phase}페이즈 · ${xpText}` : `${state.phase} · ${xpText}`;
  ui.timerLabel.textContent = formatTime(state.elapsed);
  const shieldText = state.player.shield > 0 ? ` + 보호 ${Math.ceil(state.player.shield)}` : "";
  ui.hpLabel.textContent = `HP ${Math.ceil(state.player.hp)} / ${state.player.maxHp}${shieldText}`;
  ui.weaponCard.innerHTML = weaponCardHtml(state.weapon);
  renderMemorySlots();
  renderEchoes();
}

function renderMemorySlots() {
  const source = state ? activeMemories() : selectedMemories.map((id) => ({ ...memories[id], cooldownLeft: 0, clarity: 0 }));
  ui.memorySlots.innerHTML = "";
  if (!source.length) {
    ui.memorySlots.innerHTML = `<div class="info-card empty">기억 3개를 선택하세요.</div>`;
    return;
  }
  const maxClarity = Math.max(0, ...source.map((memory) => memory.clarity || 0));
  const omenActive = state?.runTimeline?.pressurePhaseId === "climax" || state?.mode === "questions";
  source.forEach((memory, index) => {
    const maxCd = memory.cooldown || 1;
    const cdPercent = memory.id === "blood_reflection" ? 100 : (1 - (memory.cooldownLeft || 0) / maxCd) * 100;
    const clarityPercent = (memory.clarity || 0) * 100;
    const watched = state && clarityPercent > 35 && (memory.clarity || 0) >= maxClarity - 0.01;
    const focused = state && activeTacticalFocus(memory.id);
    const tacticalStatus = state ? tacticalFocusStatus(memory) : "";
    const slot = document.createElement("div");
    slot.className = `slot ${watched ? "watched" : ""} ${focused ? "focused" : ""} ${clarityPercent > 68 ? "cracked" : ""} ${clarityPercent > 88 ? "gazed" : ""}`;
    if (state) {
      slot.setAttribute("role", "button");
      slot.tabIndex = 0;
    }
    slot.innerHTML = `
      <div class="slot-header"><strong>${memory.name}</strong><small>${memory.role}</small></div>
      <div class="tag-row">${tagBadges(memory.tags)}</div>
      ${watched ? `<div class="risk-tag">레테의 시선</div>` : ""}
      <div class="cooldown-track"><div class="cooldown-fill" style="width:${clamp(cdPercent, 0, 100)}%"></div></div>
      <small>${memory.forgotten ? "망각됨" : memory.desc}</small>
      ${state ? `<div class="tactical-row"><span>전술 집중</span><span>${tacticalStatus}</span></div>` : ""}
      <div class="clarity-row"><span>${omenActive ? "망각 전조" : "의존도"}</span><span>${Math.round(clarityPercent)}%</span></div>
      <div class="clarity-track ${omenActive ? "omen-track" : ""}"><div class="clarity-fill" style="width:${clarityPercent}%"></div></div>
    `;
    if (state) {
      slot.addEventListener("click", () => requestTacticalFocus(memory.id));
      slot.addEventListener("keydown", (event) => {
        if (event.code === "Enter" || event.code === "Space") {
          event.preventDefault();
          requestTacticalFocus(memory.id);
        }
      });
      slot.dataset.tacticalSlot = String(index + 1);
    }
    ui.memorySlots.appendChild(slot);
  });
}

function tacticalFocusStatus(memory) {
  const focus = state.tacticalFocus;
  if (activeTacticalFocus(memory.id)) return `${focus.durationLeft.toFixed(1)}초`;
  if (focus.cooldownLeft > 0) return `과열 ${focus.cooldownLeft.toFixed(1)}초`;
  if (memory.cooldown > 0 && (memory.cooldownLeft || 0) <= 0.15) return "즉시";
  return "준비";
}

function renderEchoes() {
  if (!state) {
    ui.echoList.textContent = "아직 남은 잔향이 없습니다.";
    return;
  }
  const identity = buildIdentityFor(activeMemoryIds(), state);
  const lines = [];
  if (state.activeSynergies?.length) {
    state.activeSynergies.forEach((rule) => lines.push(`<strong>시너지: ${rule.name}</strong><br>${rule.desc}`));
  }
  if (state.tagEchoes?.length) {
    state.tagEchoes.forEach((echo) => lines.push(`태그 잔향: ${echo.memoryName} -> ${echo.tags.map(tagLabel).join("/")}`));
  }
  if (state.echoUnlocks?.length) {
    const unlock = latestEchoUnlock();
    lines.push(`열린 경로: ${unlock.name}`);
  }
  if (state.echo.critChance > baseEcho.critChance) lines.push(`치명 +${percent(state.echo.critChance - baseEcho.critChance)}`);
  if (state.echo.attackSpeed) lines.push(`공격속도 +${percent(state.echo.attackSpeed)}`);
  if (state.echo.projectileCount) lines.push(`투사체 +${state.echo.projectileCount}`);
  if (state.echo.range) lines.push(`범위 +${percent(state.echo.range)}`);
  if (state.echo.extraHitChance) lines.push(`추가타 +${percent(state.echo.extraHitChance)}`);
  if (state.echo.cooldownReduction) lines.push(`쿨다운 -${percent(state.echo.cooldownReduction)}`);
  if (state.echo.weaponFlashChance) lines.push(`무기 섬광 +${percent(state.echo.weaponFlashChance)}`);
  if (state.echo.weaponBleedDamage) lines.push(`무기 출혈 +${percent(state.echo.weaponBleedDamage)}`);
  if (state.echo.weaponHomingChance) lines.push(`무기 유도탄 +${percent(state.echo.weaponHomingChance)}`);
  if (state.echo.weaponShockwaveChance) lines.push(`무기 파문 +${percent(state.echo.weaponShockwaveChance)}`);
  if (state.echo.weaponSlowChance) lines.push(`무기 둔화 +${percent(state.echo.weaponSlowChance)}`);
  if (state.echo.weaponCounterChance) lines.push(`무기 반격 +${percent(state.echo.weaponCounterChance)}`);
  if (state.echo.weaponMarkChance) lines.push(`무기 표식 +${percent(state.echo.weaponMarkChance)}`);
  const growth = state.runGrowth;
  const growthLines = [];
  if (growth.damage) growthLines.push(`런 피해 +${percent(growth.damage)}`);
  if (growth.attackSpeed) growthLines.push(`런 공격속도 +${percent(growth.attackSpeed)}`);
  if (growth.cooldownReduction) growthLines.push(`런 쿨다운 -${percent(growth.cooldownReduction)}`);
  if (growth.range) growthLines.push(`런 범위 +${percent(growth.range)}`);
  if (growth.damageReduction) growthLines.push(`런 피해감소 +${percent(growth.damageReduction)}`);
  if (growth.xpGain) growthLines.push(`런 경험치 +${percent(growth.xpGain)}`);
  const allLines = [...growthLines, ...lines];
  if (state.tacticalFocus.useCount > 0) {
    const focus = state.tacticalFocus;
    const name = memories[focus.memoryId]?.name || focus.history[focus.history.length - 1]?.memoryName || "기억";
    allLines.unshift(`전술 집중: ${name} ${focus.durationLeft > 0 ? `${focus.durationLeft.toFixed(1)}초` : "재정비"}`);
  } else if (state.tacticalFocus.cooldownLeft <= 0) {
    allLines.unshift("전술 집중 준비");
  }
  ui.echoList.innerHTML = `${buildIdentityHtml(identity)}${allLines.map((line) => `<div class="echo-line">${line}</div>`).join("")}`;
  writeIdentityQaResult({ status: "visible" });
  writeTacticalQaResult({ status: state.tacticalFocus.useCount > 0 ? "used" : "visible" });
}

function draw() {
  ctx.clearRect(0, 0, canvas.width, canvas.height);
  drawArena();
  if (!state) {
    drawTitleWater();
    return;
  }
  ctx.save();
  if (state.shake > 0) {
    ctx.translate(rand(-state.shake, state.shake), rand(-state.shake, state.shake));
  }
  drawEffects("under");
  drawProjectiles(false);
  drawPlayer();
  drawEnemies();
  drawBoss();
  drawProjectiles(true);
  drawEffects("over");
  drawFloaters();
  ctx.restore();
}

function drawArena() {
  const grd = ctx.createLinearGradient(0, 0, canvas.width, canvas.height);
  grd.addColorStop(0, "#071017");
  grd.addColorStop(0.52, "#0d1822");
  grd.addColorStop(1, "#080a0f");
  ctx.fillStyle = grd;
  ctx.fillRect(0, 0, canvas.width, canvas.height);
  ctx.strokeStyle = "rgba(109, 223, 210, 0.09)";
  ctx.lineWidth = 1;
  for (let y = 42; y < canvas.height; y += 52) {
    ctx.beginPath();
    ctx.moveTo(0, y);
    for (let x = 0; x <= canvas.width; x += 80) {
      ctx.lineTo(x, y + Math.sin(x * 0.017 + y) * 5);
    }
    ctx.stroke();
  }
}

function drawTitleWater() {
  ctx.fillStyle = "rgba(109, 223, 210, 0.12)";
  ctx.fillRect(120, 286, canvas.width - 240, 2);
}

function drawPlayer() {
  const p = state.player;
  ctx.save();
  ctx.translate(p.x, p.y);
  ctx.rotate(p.facing);
  ctx.fillStyle = p.invuln > 0 ? "#e8c15d" : "#eef8ff";
  ctx.beginPath();
  ctx.moveTo(18, 0);
  ctx.lineTo(-11, -12);
  ctx.lineTo(-6, 0);
  ctx.lineTo(-11, 12);
  ctx.closePath();
  ctx.fill();
  ctx.strokeStyle = "#6ddfd2";
  ctx.stroke();
  ctx.restore();
}

function drawEnemies() {
  for (const enemy of state.enemies) {
    drawUnit(enemy, enemy.color, enemy.name);
  }
}

function drawBoss() {
  if (!state.boss) return;
  const boss = state.boss;
  drawUnit(boss, boss.groggy ? "#e8c15d" : "#ff5d6c", boss.name);
  const w = 320;
  const x = canvas.width / 2 - w / 2;
  const y = 22;
  ctx.fillStyle = "#111720";
  ctx.fillRect(x, y, w, 10);
  ctx.fillStyle = boss.groggy ? "#e8c15d" : "#ff5d6c";
  ctx.fillRect(x, y, w * clamp(boss.hp / boss.maxHp, 0, 1), 10);
  ctx.fillStyle = "#f0f3f8";
  ctx.font = "12px system-ui";
  ctx.textAlign = "center";
  ctx.fillText(`${boss.name} ${boss.phase}페이즈`, canvas.width / 2, y + 27);
}

function drawUnit(unit, color, label) {
  ctx.fillStyle = color;
  ctx.beginPath();
  ctx.arc(unit.x, unit.y, unit.r, 0, Math.PI * 2);
  ctx.fill();
  ctx.strokeStyle = "rgba(255,255,255,0.45)";
  ctx.stroke();
  if (unit.hp < unit.maxHp) {
    ctx.fillStyle = "#111720";
    ctx.fillRect(unit.x - unit.r, unit.y - unit.r - 9, unit.r * 2, 4);
    ctx.fillStyle = "#72e49b";
    ctx.fillRect(unit.x - unit.r, unit.y - unit.r - 9, unit.r * 2 * clamp(unit.hp / unit.maxHp, 0, 1), 4);
  }
  if (unit.slow > 0) {
    ctx.strokeStyle = "#6ddfd2";
    ctx.beginPath();
    ctx.arc(unit.x, unit.y, unit.r + 5, 0, Math.PI * 2);
    ctx.stroke();
  }
}

function drawProjectiles(hostile) {
  for (const projectile of state.projectiles.filter((p) => p.hostile === hostile)) {
    ctx.strokeStyle = projectile.hostile ? "rgba(255, 93, 108, 0.26)" : `${projectile.color}66`;
    ctx.lineWidth = projectile.hostile ? 2 : 3;
    ctx.beginPath();
    ctx.moveTo(projectile.x, projectile.y);
    ctx.lineTo(projectile.x - projectile.vx * 0.06, projectile.y - projectile.vy * 0.06);
    ctx.stroke();
    ctx.fillStyle = projectile.color;
    ctx.beginPath();
    ctx.arc(projectile.x, projectile.y, projectile.r, 0, Math.PI * 2);
    ctx.fill();
  }
}

function drawEffects(layer) {
  for (const effect of state.effects) {
    const over = ["flash", "blood", "slash_fast", "slash_heavy"].includes(effect.type);
    if ((layer === "over") !== over) continue;
    const t = 1 - effect.life / effect.maxLife;
    ctx.save();
    if (effect.type === "ripple" || effect.type === "clock" || effect.type === "heal_pulse" || effect.type === "hostile_aoe") {
      const color = effect.type === "hostile_aoe" ? "255, 93, 108" : effect.type === "clock" ? "169, 140, 255" : "109, 223, 210";
      ctx.strokeStyle = `rgba(${color}, ${1 - t})`;
      ctx.lineWidth = effect.type === "hostile_aoe" ? 3 : 2;
      ctx.beginPath();
      ctx.arc(effect.x, effect.y, lerp(effect.r, effect.maxR, t), 0, Math.PI * 2);
      ctx.stroke();
    }
    if (effect.type === "flash") {
      ctx.strokeStyle = `rgba(240, 248, 255, ${1 - t})`;
      ctx.lineWidth = 5;
      ctx.beginPath();
      ctx.arc(effect.x, effect.y, lerp(effect.r, effect.maxR, t), 0, Math.PI * 2);
      ctx.stroke();
      ctx.beginPath();
      ctx.moveTo(effect.x - 36, effect.y);
      ctx.lineTo(effect.x + 36, effect.y);
      ctx.moveTo(effect.x, effect.y - 36);
      ctx.lineTo(effect.x, effect.y + 36);
      ctx.stroke();
    }
    if (effect.type === "blood") {
      ctx.strokeStyle = `rgba(255, 93, 108, ${1 - t})`;
      ctx.lineWidth = 3;
      ctx.beginPath();
      ctx.arc(effect.x, effect.y, lerp(effect.r, effect.maxR, t), 0, Math.PI * 2);
      ctx.stroke();
    }
    if (effect.type === "slash_fast" || effect.type === "slash_heavy") {
      ctx.translate(effect.x, effect.y);
      ctx.rotate(effect.angle);
      ctx.strokeStyle = effect.type === "slash_heavy" ? `rgba(232, 193, 93, ${1 - t})` : `rgba(109, 223, 210, ${1 - t})`;
      ctx.lineWidth = effect.type === "slash_heavy" ? 8 : 4;
      ctx.beginPath();
      ctx.arc(0, 0, effect.r, -0.35, 0.35);
      ctx.stroke();
    }
    if (effect.type === "spark") {
      ctx.globalAlpha = Math.max(0, 1 - t);
      ctx.fillStyle = effect.color;
      ctx.beginPath();
      ctx.arc(effect.x + effect.vx * t, effect.y + effect.vy * t, effect.size * (1 - t), 0, Math.PI * 2);
      ctx.fill();
    }
    ctx.restore();
  }
}

function drawFloaters() {
  ctx.save();
  ctx.textAlign = "center";
  ctx.font = "700 13px system-ui";
  for (const floater of state.floaters) {
    const alpha = clamp(floater.life / floater.maxLife, 0, 1);
    ctx.globalAlpha = alpha;
    ctx.fillStyle = floater.color;
    ctx.strokeStyle = "rgba(3, 6, 10, 0.82)";
    ctx.lineWidth = 4;
    ctx.strokeText(floater.text, floater.x, floater.y);
    ctx.fillText(floater.text, floater.x, floater.y);
  }
  ctx.restore();
}

function addFloater(text, x, y, color = "#f0f3f8") {
  if (!state) return;
  state.floaters.push({
    text,
    x,
    y,
    vx: rand(-8, 8),
    vy: -28,
    color,
    life: 1.05,
    maxLife: 1.05,
  });
}

function addBurst(x, y, color, count = 10, power = 3) {
  if (!state) return;
  for (let i = 0; i < count; i += 1) {
    const angle = rand(0, Math.PI * 2);
    const speed = rand(14, 32) * power;
    state.effects.push({
      type: "spark",
      x,
      y,
      vx: Math.cos(angle) * speed,
      vy: Math.sin(angle) * speed,
      size: rand(2, 4.5),
      color,
      life: rand(0.22, 0.48),
      maxLife: 0.48,
    });
  }
}

function hostiles() {
  const list = [...state.enemies];
  if (state.boss) list.push(state.boss);
  return list.filter((target) => target.hp > 0);
}

function highestThreat() {
  return hostiles().sort((a, b) => threat(b) - threat(a))[0];
}

function threat(target) {
  if (target.id === "boss") return 999;
  return target.score * 100 + target.hp;
}

function farHostile() {
  const p = state.player;
  return hostiles().sort((a, b) => distance(b, p) - distance(a, p))[0];
}

function nearestHostile(x, y, maxDistance = Infinity, excluded = []) {
  const excludedSet = new Set(excluded);
  let best = null;
  let bestDist = maxDistance;
  for (const target of hostiles()) {
    if (excludedSet.has(target)) continue;
    const dist = Math.hypot(target.x - x, target.y - y);
    if (dist < bestDist) {
      best = target;
      bestDist = dist;
    }
  }
  return best;
}

function addProjectile(projectile) {
  state.projectiles.push(projectile);
}

function edgePosition() {
  const side = Math.floor(Math.random() * 4);
  if (side === 0) return { x: rand(0, canvas.width), y: -20 };
  if (side === 1) return { x: canvas.width + 20, y: rand(0, canvas.height) };
  if (side === 2) return { x: rand(0, canvas.width), y: canvas.height + 20 };
  return { x: -20, y: rand(0, canvas.height) };
}

function frame(now) {
  const dt = Math.min(0.033, (now - lastFrame) / 1000);
  lastFrame = now;
  update(dt);
  draw();
  requestAnimationFrame(frame);
}

function clamp(value, min, max) {
  return Math.max(min, Math.min(max, value));
}

function rand(min, max) {
  return min + Math.random() * (max - min);
}

function distance(a, b) {
  return Math.hypot(a.x - b.x, a.y - b.y);
}

function angleTo(a, b) {
  return Math.atan2(b.y - a.y, b.x - a.x);
}

function lerp(a, b, t) {
  return a + (b - a) * t;
}

function sum(values) {
  return values.reduce((acc, value) => acc + value, 0);
}

function normalize(value, values) {
  const max = Math.max(...values, 1);
  return clamp((value / max) * 100, 0, 100);
}

function formatTime(seconds) {
  const m = Math.floor(seconds / 60);
  const s = Math.floor(seconds % 60);
  return `${String(m).padStart(2, "0")}:${String(s).padStart(2, "0")}`;
}

function percent(value) {
  return `${Math.round(value * 100)}%`;
}

function insideBounds(point, pad) {
  return point.x > -pad && point.x < canvas.width + pad && point.y > -pad && point.y < canvas.height + pad;
}

window.addEventListener("keydown", (event) => {
  keys.add(event.code);
  if (event.target?.matches?.("input, textarea, button")) return;
  const slotIndex = ["Digit1", "Digit2", "Digit3"].indexOf(event.code);
  if (slotIndex >= 0 && state?.mode === "combat") {
    const memory = activeMemories()[slotIndex];
    if (memory && requestTacticalFocus(memory.id)) event.preventDefault();
  }
});

window.addEventListener("keyup", (event) => {
  keys.delete(event.code);
});

function writeLevelupQaResult(extra = {}) {
  if (!experiment.qaLevelupMode) return;
  const payload = {
    version: experiment.version,
    hasState: Boolean(state),
    mode: state?.mode || null,
    elapsed: state ? Number(state.elapsed.toFixed(2)) : 0,
    level: state?.runGrowth?.level || 0,
    choicesTaken: state?.runGrowth?.choicesTaken || [],
    levelUpsBeforeBoss: state?.runGrowth?.levelUpsBeforeBoss || 0,
    runGrowth: state ? runGrowthLog() : null,
    overlayHasUpgradeChoices: Boolean(document.querySelector("#levelUpChoices .choice")),
    ...extra,
  };
  document.documentElement.dataset.letheLevelupQa = JSON.stringify(payload);
}

function startLevelupQa() {
  selectedWeapon = weapons.twin_blades.id;
  selectedMemories = ["hungry_blades", "stalker_oath", "blood_reflection"];
  renderSetup();
  setTimeout(() => {
    if (!state) {
      startRun();
      setTimeout(() => {
        if (state?.mode === "combat" && state.runGrowth.level === 1) {
          state.runGrowth.xp = state.runGrowth.nextXp;
          queueLevelUp();
        }
      }, 300);
    }
  }, 50);

  const qa = {
    levelUpSeen: false,
    resumedAfterUpgrade: false,
    resultLogged: false,
    answered: false,
    startedAt: performance.now(),
  };

  const timer = setInterval(() => {
    if (!state) {
      writeLevelupQaResult({ status: "waiting_for_run" });
      return;
    }

    if (state.mode === "upgrade" && state.runGrowth.pendingChoices.length) {
      qa.levelUpSeen = document.querySelectorAll("#levelUpChoices .choice").length === 3;
      applyLevelUpChoice(state.runGrowth.pendingChoices[0]);
      qa.resumedAfterUpgrade = state.mode === "combat" && state.running;
    }

    if (qa.resumedAfterUpgrade && !qa.answered) {
      qa.answered = true;
      state.questions.protect = state.memories[0].id;
      state.questions.predict = state.memories[0].id;
      forgetMostDependent();
      showResultOverlay();
    }

    if (state.forgotten && !qa.resultLogged) {
      qa.resultLogged = true;
      state.survey.sadness = 3;
      state.survey.fairness = 3;
      state.survey.memoryRecall = memories[state.forgotten]?.name || "";
      const logPayload = collectLogPayload();
      writeLevelupQaResult({
        status: "complete",
        levelUpSeen: qa.levelUpSeen,
        resumedAfterUpgrade: qa.resumedAfterUpgrade,
        hasRunGrowthPayload: Boolean(logPayload.runGrowth),
        payloadChoicesTaken: logPayload.runGrowth?.choicesTaken || [],
        playtest: logPayload.playtest,
        forgotten: logPayload.forgotten,
      });
      clearInterval(timer);
      return;
    }

    const timedOut = performance.now() - qa.startedAt > 45000;
    writeLevelupQaResult({
      status: timedOut ? "timeout" : "running",
      levelUpSeen: qa.levelUpSeen,
      resumedAfterUpgrade: qa.resumedAfterUpgrade,
    });
    if (timedOut) clearInterval(timer);
  }, 120);
}

function writeV06QaResult(extra = {}) {
  if (!experiment.qaV06Mode) return;
  const payload = {
    version: experiment.version,
    hasState: Boolean(state),
    mode: state?.mode || null,
    elapsed: state ? Number(state.elapsed.toFixed(2)) : 0,
    activeMemoryCount: state ? activeMemoryCount() : 0,
    runTimeline: state ? runTimelineLog() : null,
    ...extra,
  };
  document.documentElement.dataset.letheV06Qa = JSON.stringify(payload);
}

function writeDeathQaResult(extra = {}) {
  const payload = {
    version: experiment.version,
    hasState: Boolean(state),
    mode: state?.mode || null,
    elapsed: state ? Number(state.elapsed.toFixed(2)) : 0,
    death: state?.death || null,
    danger: state ? dangerLog() : null,
    ...extra,
  };
  document.documentElement.dataset.letheDeathQa = JSON.stringify(payload);
}

function startV06CycleQa() {
  selectedWeapon = weapons.twin_blades.id;
  selectedMemories = ["hungry_blades", "stalker_oath", "blood_reflection"];
  renderSetup();
  setTimeout(() => {
    if (!state) startRun();
  }, 50);

  const qa = {
    bossSpawned: false,
    forgotten: false,
    deficitStarted: false,
    refillSeen: false,
    refilled: false,
    startedAt: performance.now(),
  };

  const timer = setInterval(() => {
    if (!state) {
      writeV06QaResult({ status: "waiting_for_run" });
      return;
    }

    if (!qa.bossSpawned && !state.boss && state.mode === "combat") {
      const nextBossAt = state.runTimeline.bossScheduleSec[state.runTimeline.nextBossIndex] || 0;
      state.elapsed = Math.max(state.elapsed, nextBossAt);
      updateSpawning(0);
    }

    if (state.boss) {
      qa.bossSpawned = true;
      state.boss.hp = 0;
      updateBoss(0);
    }

    if (state.mode === "questions") {
      const active = activeMemories();
      if (active.length) {
        state.questions.protect = active[0].id;
        state.questions.predict = active[0].id;
        const forgotten = forgetMostDependent();
        qa.forgotten = Boolean(forgotten);
        showCycleResultOverlay(forgotten);
      }
    }

    const continueButton = document.querySelector("#continueCycleButton");
    if (continueButton) {
      continueButton.click();
      qa.deficitStarted = true;
    }

    if (state.runTimeline.refillAvailableAt && state.mode === "combat") {
      state.elapsed = Math.max(state.elapsed, state.runTimeline.refillAvailableAt);
      updateRefillGate();
    }

    if (state.mode === "refill") {
      qa.refillSeen = document.querySelectorAll("#refillChoices .choice").length > 0;
      const candidates = refillCandidates();
      if (candidates.length) {
        applyMemoryRefill(candidates[0]);
        qa.refilled = true;
      }
    }

    const complete = qa.bossSpawned && qa.forgotten && qa.deficitStarted && qa.refillSeen && qa.refilled && activeMemoryCount() === 3;
    if (complete) {
      const payload = collectLogPayload();
      writeV06QaResult({
        status: "complete",
        bossSpawned: qa.bossSpawned,
        forgotten: qa.forgotten,
        deficitStarted: qa.deficitStarted,
        refillSeen: qa.refillSeen,
        refilled: qa.refilled,
        hasTimelinePayload: Boolean(payload.runTimeline),
        cycleCount: payload.runTimeline?.cycles?.length || 0,
        refillCount: payload.runTimeline?.refillChoices?.length || 0,
      });
      clearInterval(timer);
      return;
    }

    const timedOut = performance.now() - qa.startedAt > 45000;
    writeV06QaResult({
      status: timedOut ? "timeout" : "running",
      bossSpawned: qa.bossSpawned,
      forgotten: qa.forgotten,
      deficitStarted: qa.deficitStarted,
      refillSeen: qa.refillSeen,
      refilled: qa.refilled,
    });
    if (timedOut) clearInterval(timer);
  }, 120);
}

function startDeathQa() {
  selectedWeapon = weapons.twin_blades.id;
  selectedMemories = ["hungry_blades", "stalker_oath", "blood_reflection"];
  renderSetup();
  setTimeout(() => {
    if (!state) startRun();
    state.player.hp = 0;
    checkPlayerDeath();
    const payload = collectLogPayload();
    writeDeathQaResult({
      status: state.mode === "dead" && payload.death && payload.danger?.deaths === 1 ? "complete" : "failed",
      hasDeathPayload: Boolean(payload.death),
      hasDangerPayload: Boolean(payload.danger),
      deathAt: payload.death?.at,
      deaths: payload.danger?.deaths,
    });
  }, 50);
}

function writeIdentityQaResult(extra = {}) {
  if (!experiment.qaIdentityMode) return;
  const payload = state ? collectLogPayload() : null;
  const identity = payload?.buildIdentity || buildIdentityFor(selectedMemories, state);
  const visibleText = ui.echoList?.textContent || "";
  document.documentElement.dataset.letheIdentityQa = JSON.stringify({
    version: experiment.version,
    hasState: Boolean(state),
    elapsed: state ? Number(state.elapsed.toFixed(2)) : 0,
    buildNameVisible: Boolean(identity?.buildName && visibleText.includes(identity.buildName)),
    synergyVisible: identity?.activeSynergies?.length
      ? identity.activeSynergies.some((rule) => visibleText.includes(rule.name))
      : visibleText.includes("활성 시너지 없음"),
    dependencyVisible: visibleText.includes(identity?.mostDependentMemory?.name || "전투 중 계산"),
    hasBuildIdentityPayload: Boolean(payload?.buildIdentity?.buildName),
    buildIdentitySeenBy90Sec: Boolean(payload?.buildIdentitySeenBy90Sec),
    buildIdentity: identity,
    visibleText,
    ...extra,
  });
}

function startIdentityQa() {
  selectedWeapon = weapons.twin_blades.id;
  selectedMemories = ["hungry_blades", "shatter_ripple", "stopped_second"];
  renderSetup();
  writeIdentityQaResult({ status: "setup_visible" });
  setTimeout(() => {
    if (!state) startRun();
  }, 50);
  setTimeout(() => {
    const qa = JSON.parse(document.documentElement.dataset.letheIdentityQa || "{}");
    const complete = qa.buildNameVisible
      && qa.synergyVisible
      && qa.dependencyVisible
      && qa.hasBuildIdentityPayload
      && qa.buildIdentitySeenBy90Sec;
    writeIdentityQaResult({ status: complete ? "complete" : "failed" });
  }, 650);
}

function writePressureQaResult(extra = {}) {
  if (!experiment.qaPressureMode) return;
  const payload = state ? collectLogPayload() : null;
  const segments = payload?.runTimeline?.pressureSegments || [];
  const ids = segments.map((segment) => segment.id);
  document.documentElement.dataset.lethePressureQa = JSON.stringify({
    version: experiment.version,
    hasState: Boolean(state),
    elapsed: state ? Number(state.elapsed.toFixed(2)) : 0,
    pressureSegments: segments,
    pressureSegmentIds: ids,
    hasLull: ids.includes("lull"),
    hasRising: ids.includes("rising"),
    hasClimax: ids.includes("climax"),
    danger: payload?.danger || null,
    ...extra,
  });
}

function startPressureQa() {
  selectedWeapon = weapons.twin_blades.id;
  selectedMemories = ["hungry_blades", "shatter_ripple", "stopped_second"];
  renderSetup();
  setTimeout(() => {
    if (!state) startRun();
  }, 50);

  let step = 0;
  const timer = setInterval(() => {
    if (!state) {
      writePressureQaResult({ status: "waiting_for_run" });
      return;
    }

    const nextBossAt = state.runTimeline.bossScheduleSec[state.runTimeline.nextBossIndex] || 10;
    const targets = [0.02, 0.36, 0.78];
    if (step < targets.length) {
      state.elapsed = Math.max(state.elapsed, nextBossAt * targets[step]);
      updateSpawning(0);
      updateDangerMetrics(0.1);
      step += 1;
      writePressureQaResult({ status: "running", step });
      return;
    }

    const qa = JSON.parse(document.documentElement.dataset.lethePressureQa || "{}");
    const complete = qa.hasLull && qa.hasRising && qa.hasClimax;
    writePressureQaResult({ status: complete ? "complete" : "failed", step });
    clearInterval(timer);
  }, 120);
}

function writePostLossQaResult(extra = {}) {
  if (!experiment.qaPostLossMode) return;
  const payload = state ? collectLogPayload() : null;
  const challenges = payload?.runTimeline?.postLossChallenges || [];
  const challenge = challenges[challenges.length - 1] || null;
  const segmentIds = (challenge?.segments || []).map((segment) => segment.id);
  document.documentElement.dataset.lethePostLossQa = JSON.stringify({
    version: experiment.version,
    hasState: Boolean(state),
    elapsed: state ? Number(state.elapsed.toFixed(2)) : 0,
    activeMemoryCount: state ? activeMemoryCount() : 0,
    postLossChallenge: challenge,
    postLossChallengeCount: challenges.length,
    postLossSegmentIds: segmentIds,
    hasDeficitBreath: segmentIds.includes("deficit_breath"),
    hasDeficitTrial: segmentIds.includes("deficit_trial"),
    challengeCompleted: Boolean(challenge?.completedAt),
    challengeSurvived: Boolean(challenge?.survived),
    refillChoices: payload?.runTimeline?.refillChoices || [],
    danger: payload?.danger || null,
    ...extra,
  });
}

function startPostLossQa() {
  selectedWeapon = weapons.twin_blades.id;
  selectedMemories = ["hungry_blades", "shatter_ripple", "stopped_second"];
  renderSetup();
  setTimeout(() => {
    if (!state) startRun();
  }, 50);

  let phase = 0;
  const startedAt = performance.now();
  const timer = setInterval(() => {
    if (!state) {
      writePostLossQaResult({ status: "waiting_for_run" });
      return;
    }

    if (phase === 0 && !state.boss && state.mode === "combat") {
      const nextBossAt = state.runTimeline.bossScheduleSec[state.runTimeline.nextBossIndex] || 0;
      state.elapsed = Math.max(state.elapsed, nextBossAt);
      updateSpawning(0);
      phase = 1;
      writePostLossQaResult({ status: "running", phase });
      return;
    }

    if (phase === 1 && state.boss) {
      state.boss.hp = 0;
      updateBoss(0);
      phase = 2;
      writePostLossQaResult({ status: "running", phase });
      return;
    }

    if (phase === 2 && state.mode === "questions") {
      const active = activeMemories();
      state.questions.protect = active[0].id;
      state.questions.predict = active[0].id;
      const forgotten = forgetMostDependent();
      showCycleResultOverlay(forgotten);
      phase = 3;
      writePostLossQaResult({ status: "running", phase });
      return;
    }

    const continueButton = document.querySelector("#continueCycleButton");
    if (phase === 3 && continueButton) {
      continueButton.click();
      updateSpawning(0);
      updateDangerMetrics(0.5);
      phase = 4;
      writePostLossQaResult({ status: "running", phase });
      return;
    }

    if (phase === 4 && state.runTimeline.refillAvailableAt) {
      const start = state.runTimeline.deficitStartedAt || state.elapsed;
      state.elapsed = Math.max(state.elapsed, start + experiment.deficitDurationSec * 0.68);
      updateSpawning(0);
      updateDangerMetrics(1.5);
      phase = 5;
      writePostLossQaResult({ status: "running", phase });
      return;
    }

    if (phase === 5 && state.runTimeline.refillAvailableAt) {
      state.elapsed = Math.max(state.elapsed, state.runTimeline.refillAvailableAt);
      updateRefillGate();
      phase = 6;
      writePostLossQaResult({ status: "running", phase });
      return;
    }

    if (phase === 6 && state.mode === "refill") {
      const candidates = refillCandidates();
      if (candidates.length) applyMemoryRefill(candidates[0]);
      phase = 7;
      writePostLossQaResult({ status: "running", phase });
      return;
    }

    const qa = JSON.parse(document.documentElement.dataset.lethePostLossQa || "{}");
    const complete = qa.hasDeficitBreath
      && qa.hasDeficitTrial
      && qa.challengeCompleted
      && qa.challengeSurvived
      && activeMemoryCount() === 3;
    if (phase >= 7 || complete) {
      writePostLossQaResult({ status: complete ? "complete" : "failed", phase });
      clearInterval(timer);
      return;
    }

    const timedOut = performance.now() - startedAt > 45000;
    writePostLossQaResult({ status: timedOut ? "timeout" : "running", phase });
    if (timedOut) clearInterval(timer);
  }, 120);
}

function writeTacticalQaResult(extra = {}) {
  if (!experiment.qaTacticalMode) return;
  let previous = null;
  try {
    previous = JSON.parse(document.documentElement.dataset.letheTacticalQa || "null");
  } catch {}
  const previousTerminal = previous && ["complete", "failed"].includes(previous.status);
  const nextTerminal = ["complete", "failed"].includes(extra.status);
  if (previousTerminal && !nextTerminal) return;

  const payload = state ? collectLogPayload() : null;
  const focus = payload?.tacticalFocus || null;
  const visibleText = `${ui.echoList?.textContent || ""} ${ui.memorySlots?.textContent || ""}`;
  document.documentElement.dataset.letheTacticalQa = JSON.stringify({
    version: experiment.version,
    hasState: Boolean(state),
    mode: state?.mode || null,
    elapsed: state ? Number(state.elapsed.toFixed(2)) : 0,
    tacticalFocus: focus,
    useCount: focus?.useCount || 0,
    successfulCount: focus?.successfulCount || 0,
    historyCount: focus?.history?.length || 0,
    visibleTextHasTacticalFocus: visibleText.includes("전술 집중"),
    activeMemoryCount: state ? activeMemoryCount() : 0,
    ...extra,
  });
}

function startTacticalQa() {
  selectedWeapon = weapons.twin_blades.id;
  selectedMemories = ["hungry_blades", "shatter_ripple", "stopped_second"];
  renderSetup();
  writeTacticalQaResult({ status: "setup_visible" });
  setTimeout(() => {
    if (!state) startRun();
  }, 50);

  let phase = 0;
  const startedAt = performance.now();
  const timer = setInterval(() => {
    if (!state) {
      writeTacticalQaResult({ status: "waiting_for_run" });
      return;
    }

    if (phase === 0 && state.mode === "combat") {
      updateSpawning(0);
      const memory = activeMemories()[0];
      if (memory) requestTacticalFocus(memory.id);
      phase = 1;
      writeTacticalQaResult({ status: "running", phase });
      return;
    }

    if (phase === 1) {
      updateMemories(0.2);
      renderMemorySlots();
      renderEchoes();
      const qa = JSON.parse(document.documentElement.dataset.letheTacticalQa || "{}");
      const complete = qa.useCount >= 1
        && qa.successfulCount >= 1
        && qa.historyCount >= 1
        && qa.visibleTextHasTacticalFocus
        && qa.tacticalFocus?.history?.[0]?.memoryId;
      writeTacticalQaResult({ status: complete ? "complete" : "failed", phase });
      clearInterval(timer);
      return;
    }

    const timedOut = performance.now() - startedAt > 12000;
    writeTacticalQaResult({ status: timedOut ? "timeout" : "running", phase });
    if (timedOut) clearInterval(timer);
  }, 120);
}

if (experiment.qaFastMode || experiment.qaLevelupMode || experiment.qaV06Mode || experiment.qaDeathMode || experiment.qaIdentityMode || experiment.qaPressureMode || experiment.qaPostLossMode || experiment.qaTacticalMode) {
  window.__letheQaLog = () => (state ? JSON.parse(JSON.stringify(collectLogPayload())) : null);
}

initSetup();
if (experiment.qaIdentityMode) startIdentityQa();
if (experiment.qaLevelupMode) startLevelupQa();
if (experiment.qaV06Mode) startV06CycleQa();
if (experiment.qaDeathMode) startDeathQa();
if (experiment.qaPressureMode) startPressureQa();
if (experiment.qaPostLossMode) startPostLossQa();
if (experiment.qaTacticalMode) startTacticalQa();
requestAnimationFrame(frame);
