# Current Task

## Goal

Finalize the Korean design-doc set and prepare the Unity vertical-slice backlog.

## Why Now

The HTML v0.12 prototype recovered its automated balance gate with one accepted tuning change: player max HP `180 -> 190`. The playtest package was regenerated, and echo readability was patched without changing balance values.

The user has now clarified the stronger target fantasy:

- A lost `+3` memory should become `+3` echo.
- Echo level should cap at `+5`, just like memory level.
- If a `+3` echo exists and a `+2` memory is lost, the echo should become `+5`.
- If the player reacquires a memory whose echo remains, that memory should be strengthened by resonance.
- `+5` echoes should feel powerful and visible.
- Two `+5` echoes should be able to create ultimate echo synergies.
- Ranged enemies should create pressure, but not become annoying permanent kiters.
- Forgetting should target the highest-level active memory, with tied highest memories chosen by the player.
- Echo overflow above `+5` should not be discarded; it becomes echo overcharge or ultimate echo energy.
- First-slice post-forgetting loss should target a meaningful but not futile drop: about `30~40%`, with recovery narrowed to `95~105%`.

This is big enough that further HTML micro-tuning is less important than defining the Unity vertical-slice system contract. The design source has now been split into a Korean multi-document structure under `docs/design/` so the overview, systems, run structure, combat, content tables, balance, and Unity slice scope can evolve separately.

## Done Criteria

- A Korean design-doc set exists for the Unity transition. Done: `docs/design/README.md` plus overview, core systems, run structure, combat, content tables, balance baseline, and Unity slice spec.
- The docs define memory level, echo level, memory-to-echo conversion, reacquisition resonance, awakened echoes, ultimate echo synergies, enemy role direction, and first Unity slice scope.
- The docs define the highest-level memory forgetting rule, echo overflow handling, and updated loss/recovery balance targets.
- Orchestration status, next tasks, decision log, devlog, and report are updated.
- The next task is a Unity vertical-slice backlog, not another blind HTML balance pass.

## Related Files

- `docs/design/LETHE_CORE_SYSTEMS_UNITY_PLAN.md`
- `docs/design/LETHE_GAME_DESIGN_OVERVIEW.md`
- `docs/design/LETHE_RUN_STRUCTURE.md`
- `docs/design/LETHE_COMBAT_DESIGN.md`
- `docs/design/LETHE_CONTENT_TABLES.md`
- `docs/design/LETHE_BALANCE_BASELINE.md`
- `docs/design/LETHE_UNITY_VERTICAL_SLICE_SPEC.md`
- `docs/adr/ADR-001-html-prototype-before-unity.md`
- `docs/orchestration/state/STATUS.md`
- `docs/orchestration/state/NEXT_TASKS.md`
- `docs/orchestration/state/DECISION_LOG.md`
- `docs/orchestration/reports/20260609/index.md`
- `src/game.js`

## Verification Commands

```bash
npm run report
npm run report:check
npm run doctor
```

## Open Questions

- Should reacquired memories start at `base offered level + floor(echo level / 2)`, or should the bonus be tuned differently?
- Should first-slice echo overcharge become immediate burst, ultimate echo gauge, or a fixed hybrid of both?
- Should the recommended first showcase stay `칼무리 잔향 + 혈반 잔향 -> 피의 칼폭풍`, or should the backlog include a second fallback pair?
- Should HTML human testing still happen before Unity, or only as optional supporting evidence?

## Do Not Touch

Do not add shop, meta progression, multi-region structure, final boss, or a large new memory roster before the Unity first-slice system contract is approved.
