# Current Task

## Goal

Rebuild the Unity `_dev` scene foundation so `Dev_EchoSlice.unity` first reads as a playable game, not a VFX debug board.

## Why Now

Jaewoo's review is that the current result is not enough: camera distance, character scale, monster presence, weapon readability, and overall game feel are not yet acceptable. That means continuing echo-system work first would be premature.

Current priority is Phase 1 from `docs/design/LETHE_UNITY_PLAYABLE_GAME_SLICE_PLAN.md`:

- camera size and follow feel,
- player/enemy/weapon scale,
- weapon anchor and sorting,
- arena floor and boundary readability,
- debug panel not blocking the view,
- Play Mode screenshot/evidence.

## Done Criteria

- `Main Camera` orthographic size and follow settings are tuned for readable 16:9 gameplay.
- Player/enemy/weapon scales are consistent.
- `Weapon_DualBlades_Runtime` clearly reads as held by the player.
- Arena floor and bounds give spatial context.
- Debug panel is compact and does not dominate the screen.
- Play Mode verification confirms scene has compile error 0, console error 0, missing reference 0.
- A screenshot/evidence file is captured or documented if MCP screenshot is available.
- Report/devlog/status are updated.

## Related Files

- `docs/design/LETHE_UNITY_PLAYABLE_GAME_SLICE_PLAN.md`
- `LETHE/Assets/_dev/Scenes/Dev_EchoSlice.unity`
- `LETHE/Assets/_dev/Scripts/Camera/DevCameraFollow2D.cs`
- `LETHE/Assets/_dev/Scripts/Player/DevPlayerController2D.cs`
- `LETHE/Assets/_dev/Scripts/Combat/Enemies/DevEnemyChaseController.cs`
- `LETHE/Assets/_dev/Scripts/Combat/Weapons/DualBladesController.cs`
- `LETHE/Assets/_dev/Scripts/Debug/DevEchoSliceDebugController.cs`

## Verification Commands

```bash
npm.cmd run report
npm.cmd run report:check
npm.cmd run report:orchestrator:unit:dry
```

Unity MCP verification:

- `unity_get_compilation_errors(port=7890, severity="all")`
- `unity_search_missing_references(port=7890, scope="scene")`
- `unity_console_log(port=7890, type="error")`
- Play Mode composition check for camera/player/enemy/weapon.

## Open Questions

- After Phase 1, should Phase 2 start with player HP/contact damage or multi-enemy spawn?

## Do Not Touch

Do not add new content, shop, meta progression, multi-region structure, final boss, or promote `_dev` to `Assets/Lethe` yet.
