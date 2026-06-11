# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, legacy `docs/NEXT_TASKS.md`, or evidence files.

## 1. Complete Prototype Hands-On Review

- Priority: highest
- Source: `docs/design/LETHE_UNITY_COMPLETE_PROTOTYPE_PRD.md`
- Include: open `Assets/_dev/Scenes/Dev_Prototype_v0.unity`, press Play, use F1-F8/debug buttons, compare 쌍검 vs 대검, force 8 memories, 8 echoes, 4 ultimates, and judge the new procedural VFX readability.
- Done: record whether the corrected F7 three-memory showcase reads as Kalmuri orbit + ShatterWave enemy wave + StoppedSecond enemy clock, which weapon feels better, and which memory still violates its design role.

## 2. ScriptableObject Asset Binding

- Priority: highest
- Include: turn the in-code complete prototype catalog into `_dev/Data` `WeaponDefinition`, `MemoryDefinition`, `EchoDefinition`, `EchoSynergyDefinition`, `EnemyDefinition`, `RewardPoolDefinition` assets.
- Done: `PrototypeGameManager` can read ids/display names/levels from data assets rather than only static code tables.

## 3. Dedicated VFX For Remaining Memories

- Priority: high
- Include: generate/import readable sprite VFX for 처형, 추적, 파문, 정지, 잿빛, 낙인 active/echo states.
- Done: no remaining memory/echo depends on procedural line placeholder shapes as its main read.

## 4. Complete Prototype Balance Smoke

- Priority: high
- Include: run 60~120 second smoke for 쌍검 and 대검 with 8 memories/echoes, measuring kills, HP, synergies, strongest effect.
- Done: complete prototype is tense enough to review without instant collapse or full-screen deletion.

## 5. Runtime Service Split

- Priority: high
- Include: move effect routing from `PrototypeGameManager` into dedicated runtime classes using the new C1 services.
- Done: adding or disabling a memory/echo no longer requires editing the manager's switch statements.
