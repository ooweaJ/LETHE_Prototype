# Current Task

## Goal

Make the Unity `_dev` echo slice playable enough for jaewoo to open it in the morning and review the core loop visually.

## Why Now

The Unity skeleton, basic sprites, runtime foundation, basic combat scene, and core echo VFX prefabs now exist. The remaining useful overnight work is to wire them into a playable debug loop rather than wait for final art or production structure.

This task must answer:

- Can jaewoo press Play in `Dev_EchoSlice` and see the difference between base dual blades, Kalmuri +1, Kalmuri +5, Blood +5, and Blood Blade Storm?
- Are the generated echo sprites connected to actual scene/prefab behavior?
- Is the morning review checklist clear enough to decide GO/ITERATE/NO-GO?

## Done Criteria

- Core echo sprites and placeholder prefabs exist under `LETHE/Assets/_dev`.
- `Dev_EchoSlice.unity` contains a debug controller or panel with immediate state switching.
- The scene can show:
  - base dual blades only,
  - Kalmuri +1 delayed slash,
  - Kalmuri +5 orbit/launch state,
  - Blood +5 mark/bloom/heal thread state,
  - Blood Blade Storm.
- Unity compile errors are zero.
- Missing scene references are zero.
- Report/devlog/state docs record each completed unit.
- Discord notification is attempted through Project Orchestrator after report generation; if the intake endpoint is unavailable, record the failed endpoint check and retry command.

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
- `docs/orchestration/state/NEXT_TASKS.md`
- `docs/orchestration/reports/20260610/index.md`

## Verification Commands

```bash
npm.cmd run report
npm.cmd run report:check
npm.cmd run report:orchestrator:unit:dry
```

## Open Questions

- Whether final debug interaction should be UI buttons or keyboard shortcuts. Overnight default: implement keyboard shortcuts first and add minimal visible labels if feasible.

## Do Not Touch

Do not add new memories, weapons, shop, meta progression, multi-region structure, or final boss. Use `Assets/_dev` for experimental Unity slice work until the first echo slice earns promotion to `Assets/Lethe`.
