# Current Task

## Goal

Finalize the Unity-transition core system plan for LETHE.

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

This is big enough that further HTML micro-tuning is less important than defining the Unity vertical-slice system contract.

## Done Criteria

- A core system design document exists for the Unity transition. Done: `docs/design/LETHE_CORE_SYSTEMS_UNITY_PLAN.md`.
- The document defines memory level, echo level, memory-to-echo conversion, reacquisition resonance, awakened echoes, ultimate echo synergies, and enemy role direction.
- Orchestration status, next tasks, decision log, devlog, and report are updated.
- The next task is a Unity vertical-slice backlog, not another blind HTML balance pass.

## Related Files

- `docs/design/LETHE_CORE_SYSTEMS_UNITY_PLAN.md`
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

- Which two awakened echoes should be implemented first in Unity?
- Which one ultimate echo synergy should be the first vertical-slice showcase?
- Should reacquired memories start at `base offered level + floor(echo level / 2)`, or should the bonus be tuned differently?
- Should echo overflow above `+5` be ignored in the first slice, or converted into a temporary burst?
- Should HTML human testing still happen before Unity, or only as optional supporting evidence?

## Do Not Touch

Do not add shop, meta progression, multi-region structure, final boss, or a large new memory roster before the Unity first-slice system contract is approved.
