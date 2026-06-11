# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, legacy `docs/NEXT_TASKS.md`, or evidence files.

## 1. Complete Prototype Data Contract

- Priority: highest
- Source: `docs/design/LETHE_UNITY_COMPLETE_PROTOTYPE_PRD.md`
- Include: fix ids and ScriptableObject contracts for 2 weapons, 8 memories, 8 echoes, 4 ultimate echoes, enemy roles, reward pools, feedback profiles.
- Done: data assets and runtime contracts exist so adding a memory/echo does not require new branches inside `PrototypeGameManager`.

## 2. Weapon Pair Implementation

- Priority: highest
- Source: `docs/design/LETHE_UNITY_COMPLETE_PROTOTYPE_PRD.md`
- Include: `Weapon_DualBlades` and `Weapon_Greatsword` runtime, weapon switching debug button, separate weapon sprites and hit profiles.
- Done: 쌍검은 빠른 proc형, 대검은 느린 강타형으로 같은 적 무리에서 리듬 차이가 보인다.

## 3. Active Memories 8 L1

- Priority: high
- Include: implement level 1 behavior for HungryBlades, BloodReflection, ExecutionFlash, HunterOath, ShatterWave, StoppedSecond, AshenShield, OblivionBrand.
- Done: 8개 기억이 모두 전투 중 역할이 다르게 보이고 debug panel에서 즉시 부여 가능하다.

## 4. Echoes 8 +1 Prototype

- Priority: high
- Include: implement +1 form for Echo_Kalmuri, Echo_Blood, Echo_Execution, Echo_Homing, Echo_Shockwave, Echo_TimeStop, Echo_AshenGuard, Echo_Brand.
- Done: 잔향이 활성 기억의 약화판이 아니라 무기/몸에 남은 다른 형태로 읽힌다.

## 5. Complete Prototype Debug / Smoke Loop

- Priority: high
- Include: debug panel for weapon switch, memory add/level, forced forget, echo +1/+3/+5, 4 ultimate conditions, 60 second smoke metrics.
- Done: jaewoo가 60~120초 안에 대검, 8기억, 8잔향, 4궁극의 기준을 모두 확인할 수 있다.
