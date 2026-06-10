# Current Task

## Goal

Await jaewoo's morning review of the Unity `_dev` echo slice.

## Why Now

The Unity skeleton, basic sprites, runtime foundation, basic combat scene, core echo VFX prefabs, and playable debug loop now exist. The next decision is not more implementation by default; it is jaewoo review.

This task must answer:

- Does jaewoo choose `GO`, `ITERATE`, or `NO-GO` after playing the scene?
- Which state is weakest: Base, Kalmuri +1, Kalmuri +5, Blood +5, or Storm?
- Is `_dev -> Assets/Lethe` promotion allowed or blocked?

## Done Criteria

- Morning review prompt exists at `docs/orchestration/review_prompts/2026-06-11-unity-echo-slice-jaewoo-review.md`.
- Promotion gate exists at `docs/design/LETHE_UNITY_ECHO_SLICE_PROMOTION_GATE.md`.
- Latest report/devlog/status mention controls, known rough edges, and next decision.
- No actual `Assets/Lethe` promotion happens before jaewoo GO.

## Related Files

- `docs/design/LETHE_WEAPON_MEMORY_ECHO_SPEC.md`
- `docs/design/LETHE_WEAPON_MEMORY_ECHO_DETAIL.md`
- `docs/design/LETHE_ECHO_FORM_TRANSFORMATION_SPEC.md`
- `docs/design/LETHE_UNITY_ECHO_SYSTEM_PRD.md`
- `docs/design/LETHE_VISUAL_ASSET_PLAN.md`
- `docs/design/LETHE_UNITY_SLICE_ASSET_PRODUCTION_PLAN.md`
- `docs/design/LETHE_UNITY_ASSET_BINDING_PLAN.md`
- `LETHE/Assets/_dev/Art/Source/*.png`
- `LETHE/Assets/_dev/Art/Sprites/**/*.png`
- `LETHE/Assets/_dev/Prefabs/Echoes/**/*.prefab`
- `LETHE/Assets/_dev/Prefabs/Ultimates/**/*.prefab`
- `LETHE/Assets/_dev/Scripts/Debug/**/*.cs`
- `LETHE/Assets/_dev/Scenes/Dev_EchoSlice.unity`
- `docs/design/LETHE_UNITY_ECHO_SLICE_PROMOTION_GATE.md`
- `docs/orchestration/review_prompts/2026-06-11-unity-echo-slice-jaewoo-review.md`
- `docs/orchestration/state/NEXT_TASKS.md`
- `docs/orchestration/reports/20260611/index.md`

## Verification Commands

```bash
npm.cmd run report
npm.cmd run report:check
npm.cmd run report:orchestrator:unit:dry
```

## Open Questions

- Waiting for jaewoo review result.

## Do Not Touch

Do not add new memories, weapons, shop, meta progression, multi-region structure, or final boss. Use `Assets/_dev` for experimental Unity slice work until the first echo slice earns promotion to `Assets/Lethe`.
