# Current Task

## Goal

Turn `Dev_EchoSlice.unity` from a VFX-only check scene into a minimal playable gameplay slice.

## Why Now

Jaewoo's review is correct: the previous scene showed echo VFX, but it did not yet feel like a game. The character and enemy were effectively static, the weapon did not read as held by the player, and there was no movement pressure. That makes echo feel impossible to judge.

The immediate target is not production promotion. The target is a playable `_dev` scene where:

- the player moves,
- the camera follows,
- the enemy chases,
- the weapon is attached to the player,
- the weapon visibly swings,
- player/enemy sprites have simple motion,
- the existing `1~5` echo states still work.

## Done Criteria

- `DevPlayerController2D` moves the player with WASD/arrow keys.
- `DevEnemyChaseController` makes the test enemy chase the player.
- `DevCameraFollow2D` follows the player.
- `Weapon_DualBlades_Runtime` is parented under `Player_EchoShowcase/WeaponAnchor`.
- `DualBladesController` visibly swings on attack.
- `DevSpriteMotionAnimator` gives player/enemy simple bob/tilt animation through `Visual` child objects.
- `DevEchoSliceDebugController` still supports `1~5` and `Space`.
- Unity Play Mode verification confirms enemy movement, weapon parent, attack VFX, compile error 0, console error 0, missing reference 0.
- Report/devlog/status are updated.

## Related Files

- `docs/design/LETHE_UNITY_GAMEPLAY_SLICE_REPAIR_PLAN.md`
- `LETHE/Assets/_dev/Scenes/Dev_EchoSlice.unity`
- `LETHE/Assets/_dev/Scripts/Player/DevPlayerController2D.cs`
- `LETHE/Assets/_dev/Scripts/Combat/Enemies/DevEnemyChaseController.cs`
- `LETHE/Assets/_dev/Scripts/Camera/DevCameraFollow2D.cs`
- `LETHE/Assets/_dev/Scripts/Feedback/DevSpriteMotionAnimator.cs`
- `LETHE/Assets/_dev/Scripts/Combat/Weapons/DualBladesController.cs`
- `LETHE/Assets/_dev/Scripts/Combat/Enemies/TestEnemyController.cs`
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
- Play Mode runtime check for enemy chase and weapon attachment.

## Open Questions

- Should the next repair pass prioritize player HP/contact damage, multi-enemy spawn, or real echo damage?

## Do Not Touch

Do not add shop, meta progression, multi-region structure, final boss, or promote `_dev` to `Assets/Lethe` yet.
