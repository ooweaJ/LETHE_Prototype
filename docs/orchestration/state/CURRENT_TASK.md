# Current Task

## Goal

Turn the concrete weapon/memory/echo design into a Unity-ready system PRD: form transformation grammar, class roles, ScriptableObjects, prefabs, event boundaries, and first-slice acceptance criteria.

## Why Now

The weapon/memory/echo detail is now concrete enough to expose the next risk: if Unity implementation starts without a class/event/prefab contract, echoes can collapse back into generic proc text or tangled on-hit loops. The current task is to define how Unity should build the first showcase without implementing the project yet.

The design must answer:

- How are active memories, echoes, awakened echoes, resonance, and ultimate echoes visually different?
- Which Unity classes own build state, forgetting, resonance, hit events, echo routing, and feedback?
- Which ScriptableObjects hold weapon, memory, echo, synergy, and feedback data?
- Which prefabs are required for the first `절단쌍검 + 칼무리 + 혈반 -> 피의 칼폭풍` slice?
- How do `WeaponHit`, `EchoHit`, `UltimateHit`, `Kill`, and `ShieldBreak` avoid recursive proc loops?
- What acceptance criteria prove that echoes became transformed combat events rather than weak copies?

## Done Criteria

- `docs/design/LETHE_ECHO_FORM_TRANSFORMATION_SPEC.md` exists.
- `docs/design/LETHE_UNITY_ECHO_SYSTEM_PRD.md` exists.
- The form spec defines active memory, forgetting transition, echo, awakened echo, resonance, and ultimate shape differences.
- The PRD defines Unity data SOs, runtime classes, event boundaries, prefab list, folder layout, and first-slice acceptance criteria.
- `docs/design/README.md` links the new docs in the reading order.
- Report/devlog/state docs record that the design is now implementation-architecture ready.
- No gameplay code is added in this planning unit.

## Related Files

- `docs/design/LETHE_WEAPON_MEMORY_ECHO_SPEC.md`
- `docs/design/LETHE_WEAPON_MEMORY_ECHO_DETAIL.md`
- `docs/design/LETHE_ECHO_FORM_TRANSFORMATION_SPEC.md`
- `docs/design/LETHE_UNITY_ECHO_SYSTEM_PRD.md`
- `docs/design/LETHE_FORGETTING_FEEL_SPEC.md`
- `docs/design/LETHE_COMBAT_DESIGN.md`
- `docs/design/LETHE_CONTENT_TABLES.md`
- `docs/orchestration/state/NEXT_TASKS.md`
- `docs/orchestration/reports/20260610/index.md`

## Verification Commands

```bash
npm run report
npm run report:check
```

## Open Questions

- Should the next work be HTML showcase implementation or Unity project setup/backlog conversion?
- Should `피의 칼폭풍` be always-on once unlocked, or triggered by a gauge?
- Should the first Unity slice use automatic attacks like HTML or manual attack input?
- What healing cap should `혈반` and `피의 칼폭풍` use?

## Do Not Touch

Do not add new memories, weapons, enemies, shop, meta progression, multi-region structure, final boss, or Unity setup while this design is being clarified.
