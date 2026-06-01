"use strict";

const canvas = document.getElementById("gameCanvas");
const ctx = canvas.getContext("2d");
const overlay = document.getElementById("overlay");

const ui = {
  phaseLabel: document.getElementById("phaseLabel"),
  timerLabel: document.getElementById("timerLabel"),
  hpLabel: document.getElementById("hpLabel"),
  weaponChoices: document.getElementById("weaponChoices"),
  memoryChoices: document.getElementById("memoryChoices"),
  slotCount: document.getElementById("slotCount"),
  startRunButton: document.getElementById("startRunButton"),
  weaponCard: document.getElementById("weaponCard"),
  memorySlots: document.getElementById("memorySlots"),
  echoList: document.getElementById("echoList"),
  combatLog: document.getElementById("combatLog"),
};

const weapons = {
  twin_blades: {
    id: "twin_blades",
    name: "절단쌍검",
    role: "빠른 근접 / 온힛",
    desc: "짧은 사거리, 빠른 타격. 붙어서 긁고 빠질수록 피의 반사와 칼무리가 선명해진다.",
    range: 74,
    damage: 12,
    interval: 0.42,
    arc: Math.PI * 0.66,
  },
  greatsword: {
    id: "greatsword",
    name: "장송대검",
    role: "느린 강타 / 폭딜",
    desc: "긴 사거리, 무거운 단타. 보스 딜타임과 처형자의 섬광, 파쇄의 파문에 잘 맞는다.",
    range: 112,
    damage: 34,
    interval: 1.18,
    arc: Math.PI * 0.84,
  },
};

const memories = {
  execution_flash: {
    id: "execution_flash",
    name: "처형자의 섬광",
    role: "버스트",
    desc: "주기적으로 가장 위협적인 적에게 백색 섬광 강타.",
    cooldown: 3.15,
    echo: "치명타 확률 +12%, 치명타 피해 +35%",
    direction: "대검, 단발 공격, 폭딜 기억이 강해진다.",
  },
  hungry_blades: {
    id: "hungry_blades",
    name: "굶주린 칼무리",
    role: "근접 도트",
    desc: "주변을 도는 칼무리가 가까운 적을 지속적으로 벤다.",
    cooldown: 0.34,
    echo: "공격속도 +18%, 지속 피해 +35%",
    direction: "쌍검, 도트, 근접 유지 빌드가 강해진다.",
  },
  stalker_oath: {
    id: "stalker_oath",
    name: "추적자의 맹세",
    role: "추적 다중",
    desc: "멀거나 위협적인 적을 따라가는 기억 투사체 발사.",
    cooldown: 2.2,
    echo: "투사체 수 +1, 투사체 속도 +25%",
    direction: "중거리 투사체 기억과 카이팅 운용이 강해진다.",
  },
  shatter_ripple: {
    id: "shatter_ripple",
    name: "파쇄의 파문",
    role: "광역 / 넉백",
    desc: "충격파를 일으켜 주변 적을 밀치고 충돌 피해를 준다.",
    cooldown: 4.55,
    echo: "범위 +18%, 넉백 +25%, 피해 감소 +6%",
    direction: "포위 대응, 광역 제어, 대검 접근전이 강해진다.",
  },
  blood_reflection: {
    id: "blood_reflection",
    name: "피의 반사",
    role: "온힛 증폭",
    desc: "기본 공격 명중 시 붉은 추가타가 자동으로 반사된다.",
    cooldown: 0,
    echo: "추가타 확률 +12%, 온힛 피해 +22%",
    direction: "평타 기반 무기와 빠른 타격 운용이 강해진다.",
  },
  stopped_second: {
    id: "stopped_second",
    name: "멈춘 초침",
    role: "제어",
    desc: "주변 시간을 늦추고 기억 발동 주기를 조금 당긴다.",
    cooldown: 7.25,
    echo: "쿨다운 감소 +12%, 둔화 지속 +35%",
    direction: "쿨다운형 기억, 생존형 제어 빌드가 강해진다.",
  },
};

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

const experiment = {
  version: "v0.3",
  echoPower: 0.5,
  uiClarity: 0.62,
  bossSpawnTimeSec: 540,
  bossHp: 1750,
  qaFastMode: new URLSearchParams(window.location.search).get("qa") === "fast",
};

if (experiment.qaFastMode) {
  experiment.bossSpawnTimeSec = 10;
  experiment.bossHp = 180;
}

const forgetBias = {
  execution_flash: 0.72,
  hungry_blades: 0.90,
  stalker_oath: 0.98,
};

const keys = new Set();
let selectedWeapon = weapons.twin_blades.id;
let selectedMemories = [];
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
};

function createRunState() {
  const activeMemories = selectedMemories.map((id) => ({
    ...memories[id],
    cooldownLeft: id === "blood_reflection" ? 0 : 0.8 + Math.random() * 1.3,
    clarity: 0,
    forgotten: false,
  }));

  const metricSeed = {};
  activeMemories.forEach((memory) => {
    metricSeed[memory.id] = {
      damage: 0,
      kills: 0,
      assists: 0,
      status: 0,
      bossDamage: 0,
      groggyDamage: 0,
      bossControls: 0,
      activeCount: 0,
      presenceTime: 0,
      score: 0,
      components: {},
    };
  });

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
    },
    enemies: [],
    projectiles: [],
    effects: [],
    floaters: [],
    boss: null,
    shake: 0,
    spawnCd: 0,
    metrics: metricSeed,
    questions: {
      protect: null,
      predict: null,
    },
    forgotten: null,
    survey: {
      sadness: null,
      fairness: null,
      memoryRecall: "",
    },
    logs: {
      version: experiment.version,
      experiment: { ...experiment },
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
    button.innerHTML = `<strong>${memory.name}</strong><span>${memory.role}<br>${memory.desc}</span>`;
    button.addEventListener("click", () => toggleMemory(memory.id));
    ui.memoryChoices.appendChild(button);
  });

  ui.startRunButton.addEventListener("click", startRun);
  renderSetup();
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

function startRun() {
  state = createRunState();
  overlay.classList.remove("show");
  addLog(experiment.qaFastMode ? "QA fast mode: 검은 물이 빠르게 차오른다." : "검은 물 위로 기억이 떠올랐다.");
  logEvent("run_start");
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
  updateSpawning(dt);
  updateBoss(dt);
  updateEnemies(dt);
  updateProjectiles(dt);
  updateMemories(dt);
  updateEffects(dt);
  updateClarity();
  updateUi();

  if (state.player.hp <= 0) {
    state.player.hp = 1;
    addLog("프로토타입 보호: 쓰러지기 직전 강물이 밀어냈다.");
    logEvent("debug_death_prevented");
  }
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
  if (state.bossSpawned) return;
  state.spawnCd -= dt;
  const spawnRate = state.elapsed < 22 ? 1.45 : 0.92;
  if (state.spawnCd <= 0) {
    state.spawnCd = spawnRate;
    const pool = ["eroder", "eroder", "drifting_eye", "split_one"];
    if (state.elapsed > 16) pool.push("void_priest");
    spawnEnemy(pool[Math.floor(Math.random() * pool.length)]);
  }

  if (state.elapsed >= experiment.bossSpawnTimeSec) {
    spawnBoss();
  }
}

function spawnEnemy(typeId, x = null, y = null, child = false) {
  const type = enemyTypes[typeId];
  const pos = x === null ? edgePosition() : { x, y };
  state.enemies.push({
    ...type,
    x: pos.x,
    y: pos.y,
    hp: child ? Math.round(type.hp * 0.45) : type.hp,
    maxHp: child ? Math.round(type.hp * 0.45) : type.hp,
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
  state.enemies.length = Math.min(state.enemies.length, 8);
  state.shake = Math.max(state.shake, 12);
  state.boss = {
    id: "boss",
    name: "기억을 씹는 자",
    x: canvas.width / 2,
    y: 96,
    hp: experiment.bossHp,
    maxHp: experiment.bossHp,
    r: 32,
    phase: 1,
    phaseTimer: 0,
    actionCd: 1.2,
    groggy: false,
    groggyTimer: 0,
    aoeCd: 4,
  };
  addLog("기억을 씹는 자가 검은 물을 갈랐다.");
  addFloater("문지기 출현", canvas.width / 2, 84, "#ff5d6c");
  addBurst(canvas.width / 2, 96, "#ff5d6c", 28, 6.8);
  logEvent("boss_spawn");
}

function updateBoss(dt) {
  const boss = state.boss;
  if (!boss) return;

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
      const reduced = 1 - state.echo.damageReduction;
      p.hp -= enemy.damage * reduced;
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
        state.player.hp -= projectile.damage * (1 - state.echo.damageReduction);
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

function updateMemories(dt) {
  const p = state.player;
  basicAttack(dt);

  for (const memory of state.memories) {
    if (memory.forgotten) continue;
    const cdMul = 1 - state.echo.cooldownReduction;
    memory.cooldownLeft = Math.max(0, memory.cooldownLeft - dt);
    if (memory.id === "hungry_blades") {
      memory.metricsTime = (memory.metricsTime || 0) + dt;
      memory.visualCd = Math.max(0, (memory.visualCd || 0) - dt);
      const radius = 74 * (1 + state.echo.range);
      let hit = false;
      for (const target of hostiles()) {
        if (distance(p, target) < radius + target.r) {
          damageHostile(target, 5.8 * (1 + state.echo.dotDamage), memory.id);
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
  const interval = Math.max(0.18, weapon.interval / (1 + state.echo.attackSpeed));
  if (p.attackCd > 0) return;

  const target = nearestHostile(p.x, p.y, weapon.range * (1 + state.echo.range));
  if (!target) return;
  p.attackCd = interval;
  p.facing = angleTo(p, target);

  let damage = weapon.damage;
  if (Math.random() < state.echo.critChance) damage *= 1 + state.echo.critDamage;
  damageHostile(target, damage, "weapon");
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
    const chance = (weapon.id === "twin_blades" ? 0.4 : 0.28) + state.echo.extraHitChance;
    if (Math.random() < chance) {
      const extra = 16 * (1 + state.echo.onHitDamage);
      damageHostile(target, extra, blood.id, { bossTrace: true });
      recordPresence(blood.id, 0.8);
      state.metrics[blood.id].activeCount += 1;
      state.effects.push({ type: "blood", x: target.x, y: target.y, r: 10, maxR: 34, life: 0.28, maxLife: 0.28 });
    }
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
    const damage = 74 * (state.weapon.id === "greatsword" ? 1.12 : 1);
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
        damage: 31,
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
    const radius = 132 * (1 + state.echo.range);
    addFloater(memory.name, p.x, p.y - 28, "#6ddfd2");
    state.shake = Math.max(state.shake, 4);
    for (const target of hostiles()) {
      const dist = distance(p, target);
      if (dist < radius + target.r) {
        const push = (48 + state.echo.knockback * 48) * (1 - dist / (radius + target.r));
        const angle = angleTo(p, target);
        target.x += Math.cos(angle) * push;
        target.y += Math.sin(angle) * push;
        damageHostile(target, 38, memory.id, { bossTrace: true });
        state.metrics[memory.id].status += push;
      }
    }
    addBurst(p.x, p.y, "#6ddfd2", 16, 3.6);
    state.effects.push({ type: "ripple", x: p.x, y: p.y, r: 14, maxR: radius, life: 0.48, maxLife: 0.48 });
  }

  if (memory.id === "stopped_second") {
    const radius = 150 * (1 + state.echo.range);
    const duration = 2.4 * (1 + state.echo.slowDuration);
    addFloater(memory.name, p.x, p.y - 28, "#a98cff");
    for (const target of hostiles()) {
      if (distance(p, target) < radius + target.r) {
        target.slow = Math.max(target.slow || 0, duration);
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
}

function damageHostile(target, amount, source, options = {}) {
  const before = target.hp;
  target.hp -= amount;
  const actual = Math.max(0, before - Math.max(0, target.hp));
  if (actual > 0) {
    const color = source === "weapon" ? "#f0f3f8" : source === "execution_flash" ? "#eef8ff" : source === "stalker_oath" ? "#a98cff" : source === "shatter_ripple" ? "#6ddfd2" : source === "blood_reflection" ? "#ff5d6c" : "#e8c15d";
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
}

function recordPresence(memoryId, amount) {
  if (state.metrics[memoryId]) state.metrics[memoryId].presenceTime += amount;
}

function defeatBoss() {
  state.mode = "questions";
  state.running = false;
  calculateDependency();
  addLog("문지기가 쓰러지고, 소리가 강 아래로 빨려 들어갔다.");
  logEvent("boss_defeated", { elapsed: Number(state.elapsed.toFixed(2)) });
  showQuestionOverlay();
}

function calculateDependency() {
  const ids = state.memories.filter((m) => !m.forgotten).map((m) => m.id);
  const combatValues = ids.map((id) => {
    const m = state.metrics[id];
    return m.damage + m.kills * 34 + m.assists * 10 + m.status * 0.28;
  });
  const bossValues = ids.map((id) => {
    const m = state.metrics[id];
    return m.bossDamage + m.groggyDamage * 0.75 + m.bossControls * 34;
  });
  const presenceValues = ids.map((id) => {
    const m = state.metrics[id];
    return m.activeCount * 18 + m.presenceTime * 10;
  });
  const totalDamage = sum(ids.map((id) => state.metrics[id].damage)) || 1;
  const totalKills = sum(ids.map((id) => state.metrics[id].kills)) || 1;

  ids.forEach((id, index) => {
    const metric = state.metrics[id];
    const combat = normalize(combatValues[index], combatValues);
    const boss = normalize(bossValues[index], bossValues);
    const irreplace = clamp(((metric.damage / totalDamage) * 0.72 + (metric.kills / totalKills) * 0.28) * 100, 0, 100);
    const presence = normalize(presenceValues[index], presenceValues);
    const score = combat * 0.4 + boss * 0.25 + irreplace * 0.2 + presence * 0.15;
    const deletionScore = score * (forgetBias[id] ?? 1);
    metric.components = {
      combat: Math.round(combat),
      boss: Math.round(boss),
      irreplaceability: Math.round(irreplace),
      presence: Math.round(presence),
    };
    metric.score = Math.round(score);
    metric.deletionScore = Math.round(deletionScore);
  });

  const deletionTotal = sum(ids.map((id) => state.metrics[id].deletionScore)) || 1;
  ids.forEach((id) => {
    state.metrics[id].deletionWeight = Number((state.metrics[id].deletionScore / deletionTotal).toFixed(4));
  });
}

function forgetMostDependent() {
  calculateDependency();
  const ranked = state.memories
    .filter((memory) => !memory.forgotten)
    .sort((a, b) => state.metrics[b.id].deletionScore - state.metrics[a.id].deletionScore);
  const forgotten = ranked[0];
  forgotten.forgotten = true;
  state.forgotten = forgotten.id;
  applyEcho(forgotten.id);
  logEvent("memory_forgotten", {
    forgotten: forgotten.id,
    forgottenName: forgotten.name,
    score: state.metrics[forgotten.id].score,
    deletionScore: state.metrics[forgotten.id].deletionScore,
    deletionWeight: state.metrics[forgotten.id].deletionWeight,
    deletionWeights: dependencyWeights(),
    questions: state.questions,
    questionNames: questionNames(),
    echo: state.echo,
  });
}

function applyEcho(memoryId) {
  if (memoryId === "execution_flash") {
    state.echo.critChance += 0.12 * experiment.echoPower;
    state.echo.critDamage += 0.35 * experiment.echoPower;
  }
  if (memoryId === "hungry_blades") {
    state.echo.attackSpeed += 0.18 * experiment.echoPower;
    state.echo.dotDamage += 0.35 * experiment.echoPower;
  }
  if (memoryId === "stalker_oath") {
    state.echo.projectileCount += 1 * experiment.echoPower;
    state.echo.projectileSpeed += 0.25 * experiment.echoPower;
  }
  if (memoryId === "shatter_ripple") {
    state.echo.range += 0.18 * experiment.echoPower;
    state.echo.knockback += 0.25 * experiment.echoPower;
    state.echo.damageReduction += 0.06 * experiment.echoPower;
  }
  if (memoryId === "blood_reflection") {
    state.echo.extraHitChance += 0.12 * experiment.echoPower;
    state.echo.onHitDamage += 0.22 * experiment.echoPower;
  }
  if (memoryId === "stopped_second") {
    state.echo.cooldownReduction += 0.12 * experiment.echoPower;
    state.echo.slowDuration += 0.35 * experiment.echoPower;
  }
}

function showQuestionOverlay() {
  const template = document.getElementById("questionTemplate");
  overlay.innerHTML = "";
  overlay.appendChild(template.content.cloneNode(true));
  overlay.classList.add("show");
  const protect = overlay.querySelector("#protectChoices");
  const predict = overlay.querySelector("#predictChoices");
  const submit = overlay.querySelector("#submitQuestionsButton");

  state.memories.forEach((memory) => {
    protect.appendChild(questionButton(memory.name, () => {
      state.questions.protect = memory.id;
      selectPill(protect, memory.name);
      submit.disabled = !state.questions.protect || !state.questions.predict;
    }));
    predict.appendChild(questionButton(memory.name, () => {
      state.questions.predict = memory.id;
      selectPill(predict, memory.name);
      submit.disabled = !state.questions.protect || !state.questions.predict;
    }));
  });
  predict.appendChild(questionButton("모르겠다", () => {
    state.questions.predict = "unknown";
    selectPill(predict, "모르겠다");
    submit.disabled = !state.questions.protect || !state.questions.predict;
  }));

  submit.addEventListener("click", () => {
    forgetMostDependent();
    showResultOverlay();
  });
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
    <div class="result-card"><strong>사라진 기억</strong><br>${forgotten.name}: ${forgotten.desc}</div>
    <div class="result-card"><strong>예측 결과</strong><br>${predictionText}</div>
    <div class="result-card"><strong>삭제 weight</strong><br>${deletionWeightText()}</div>
    <div class="result-card"><strong>남은 잔향</strong><br>${forgotten.echo}<br><small>이번 실험 echo 배율: ${Math.round(experiment.echoPower * 100)}%</small></div>
    <div class="result-card"><strong>이어지는 방향</strong><br>${forgotten.direction}</div>
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
  const header = `<div class="detail-row"><span>기억</span><span>전투</span><span>보스</span><span>대체</span><span>존재</span><span>삭제</span></div>`;
  const rows = state.memories
    .map((memory) => {
      const metric = state.metrics[memory.id];
      const c = metric.components;
      return `<div class="detail-row"><span>${memory.name}</span><span>${c.combat}</span><span>${c.boss}</span><span>${c.irreplaceability}</span><span>${c.presence}</span><span>${metric.deletionScore}</span></div>`;
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
  state.logs.completedAt = new Date().toISOString();
  state.logs.elapsed = Number(state.elapsed.toFixed(2));
  state.logs.questions = state.questions;
  state.logs.questionNames = questionNames();
  state.logs.forgotten = state.forgotten;
  state.logs.forgottenName = memories[state.forgotten]?.name || state.forgotten;
  state.logs.survey = state.survey;
  state.logs.metrics = state.metrics;
  state.logs.deletionWeights = dependencyWeights();
  state.logs.echo = state.echo;
  state.logs.echoPower = experiment.echoPower;
  return state.logs;
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
  a.download = `lethe-${experiment.version}-log-${Date.now()}.json`;
  a.click();
  setTimeout(() => URL.revokeObjectURL(a.href), 1000);
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
        state.player.hp -= effect.damage * (1 - state.echo.damageReduction);
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
}

function updateUi() {
  ui.phaseLabel.textContent = state.boss ? `보스 ${state.boss.phase}페이즈` : state.phase;
  ui.timerLabel.textContent = formatTime(state.elapsed);
  ui.hpLabel.textContent = `HP ${Math.ceil(state.player.hp)} / ${state.player.maxHp}`;
  ui.weaponCard.innerHTML = weaponCardHtml(state.weapon);
  renderMemorySlots();
  renderEchoes();
}

function renderMemorySlots() {
  const source = state?.memories || selectedMemories.map((id) => ({ ...memories[id], cooldownLeft: 0, clarity: 0 }));
  ui.memorySlots.innerHTML = "";
  if (!source.length) {
    ui.memorySlots.innerHTML = `<div class="info-card empty">기억 3개를 선택하세요.</div>`;
    return;
  }
  const maxClarity = Math.max(0, ...source.map((memory) => memory.clarity || 0));
  for (const memory of source) {
    const maxCd = memory.cooldown || 1;
    const cdPercent = memory.id === "blood_reflection" ? 100 : (1 - (memory.cooldownLeft || 0) / maxCd) * 100;
    const clarityPercent = (memory.clarity || 0) * 100;
    const watched = state && clarityPercent > 35 && (memory.clarity || 0) >= maxClarity - 0.01;
    const slot = document.createElement("div");
    slot.className = `slot ${watched ? "watched" : ""} ${clarityPercent > 68 ? "cracked" : ""} ${clarityPercent > 88 ? "gazed" : ""}`;
    slot.innerHTML = `
      <div class="slot-header"><strong>${memory.name}</strong><small>${memory.role}</small></div>
      ${watched ? `<div class="risk-tag">레테의 시선</div>` : ""}
      <div class="cooldown-track"><div class="cooldown-fill" style="width:${clamp(cdPercent, 0, 100)}%"></div></div>
      <small>${memory.forgotten ? "망각됨" : memory.desc}</small>
      <div class="clarity-row"><span>의존도</span><span>${Math.round(clarityPercent)}%</span></div>
      <div class="clarity-track"><div class="clarity-fill" style="width:${clarityPercent}%"></div></div>
    `;
    ui.memorySlots.appendChild(slot);
  }
}

function renderEchoes() {
  if (!state) {
    ui.echoList.textContent = "아직 남은 잔향이 없습니다.";
    return;
  }
  const lines = [];
  if (state.echo.critChance > baseEcho.critChance) lines.push(`치명 +${percent(state.echo.critChance - baseEcho.critChance)}`);
  if (state.echo.attackSpeed) lines.push(`공격속도 +${percent(state.echo.attackSpeed)}`);
  if (state.echo.projectileCount) lines.push(`투사체 +${state.echo.projectileCount}`);
  if (state.echo.range) lines.push(`범위 +${percent(state.echo.range)}`);
  if (state.echo.extraHitChance) lines.push(`추가타 +${percent(state.echo.extraHitChance)}`);
  if (state.echo.cooldownReduction) lines.push(`쿨다운 -${percent(state.echo.cooldownReduction)}`);
  ui.echoList.innerHTML = lines.length ? lines.map((line) => `<div class="echo-line">${line}</div>`).join("") : "아직 남은 잔향이 없습니다.";
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

function nearestHostile(x, y, maxDistance = Infinity) {
  let best = null;
  let bestDist = maxDistance;
  for (const target of hostiles()) {
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
});

window.addEventListener("keyup", (event) => {
  keys.delete(event.code);
});

if (experiment.qaFastMode) {
  window.__letheQaLog = () => (state ? JSON.parse(JSON.stringify(collectLogPayload())) : null);
}

initSetup();
requestAnimationFrame(frame);
