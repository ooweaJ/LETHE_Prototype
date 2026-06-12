# Current Task

## Goal

Rebuild the Unity prototype from the consolidated `LETHE_DESIGN_00..07` docs as a fresh v1, treating the previous `Dev_Prototype_v0` as failed reference only.

## Why Now

Jaewoo rejected the previous Unity prototype as a trustworthy evaluation target. The issues were not just tuning: sprite framing, memory behavior, orbit/echo interpretation, and overall game-shell feel had drifted away from the design. Patching v0 risks carrying those assumptions forward.

The current source of truth is:

```text
docs/design/README.md
docs/design/LETHE_DESIGN_00_OVERVIEW.md
docs/design/LETHE_DESIGN_01_RUN_LOOP.md
docs/design/LETHE_DESIGN_02_COMBAT.md
docs/design/LETHE_DESIGN_03_MEMORY_ECHO.md
docs/design/LETHE_DESIGN_04_BALANCE.md
docs/design/LETHE_DESIGN_05_UI_UX.md
docs/design/LETHE_DESIGN_06_BUILD_PLAN.md
docs/design/LETHE_DESIGN_07_ASSETS_VFX.md
```

## Main Target

```text
LETHE/Assets/_dev/Scenes/Dev_Prototype_v1.unity
LETHE/Assets/_dev/Scripts/PrototypeV1/
```

Reference only:

```text
LETHE/Assets/_dev/Scenes/Dev_Prototype_v0.unity
LETHE/Assets/_dev/Scenes/Dev_EchoSlice.unity
```

## Done Criteria For This Work Unit

- Fresh v1 scene exists under `_dev`.
- Fresh v1 runtime code is isolated under `Scripts/PrototypeV1`.
- Player, camera, arena, dual blades, enemy spawn, XP/level-up, HUD, highest-level forgetting, echo cap, resonance, and Blood Blade Storm debug path exist.
- M1 and M2 debug smoke paths can be triggered without keyboard-only review.
- M2 smoke reaches forgetting, result continuation, resonance, +5 echoes, and Blood Blade Storm.
- Combat feel pass responds to jaewoo feedback: smaller Kalmuri echo, clearer dual-blade swing, hit feedback, XP bar, larger 3-card level-up UI.
- Unity compile error 0.
- Play Mode smoke creates player and enemies with no v1 runtime console exceptions.
- v1 screenshot/capture confirms player/enemy sheets are not rendered as whole sheets.

## Latest v1 Reset

- Added `V1GameManager` and `V1SceneBuilder`.
- Added `Assets/_dev/Scenes/Dev_Prototype_v1.unity`.
- v1 starts with only `Main Camera` and `V1_GameManager`; runtime creates arena/player/weapon/enemies.
- Fixed Unity Input System-only exception by supporting `Keyboard.current` with legacy fallback.
- Fixed whole-sheet rendering by cropping the generated 8x4 player/enemy sheets at runtime.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 0 warnings, 0 errors.
- latest `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 0 warnings, 0 errors.
- `unity_get_compilation_errors(port=7890, severity="all")`: `count=0`.
- `unity_scene_open(path="Assets/_dev/Scenes/Dev_Prototype_v1.unity")`: success.
- hierarchy: `V1_GameManager` has `V1GameManager`, `Main Camera` has `Camera` + `AudioListener`.
- Play Mode smoke: `player=True`, `enemies=2`, `renderers=107`, `playing=True`, `paused=False`.
- Console after Input System fix: no v1 runtime exception.
- Game capture: sprite sheets are cropped to single character/enemy frames.
- Scene saved: `sceneDirty=false`.
- M2 compressed smoke after 120 forced frames: `scene=v1 elapsed=8.5 hp=155.2/210.0 level=2 xp=7/9 kills=10 memories=[BloodReflection:3,HungryBlades:3] echoes=[HungryBlades:5,BloodReflection:5] enemies=24 storm=True result=False refill=False death=False`.
- Unity console errors after M2 smoke: `count=0`.
- Evidence capture saved: `LETHE/Assets/_dev/Evidence/v1_m2_smoke_20260612.png`.
- Combat feel pass:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors. Warnings are old v0/debug deprecated API usage, not v1 errors.
  - `unity_get_compilation_errors(port=7890, severity="all")`: `count=0`.
  - Play Mode M1 smoke snapshot: `scene=v1 elapsed=1.2 hp=210.0/210.0 level=2 xp=1/9 kills=4 memories=[HungryBlades:3,BloodReflection:2] echoes=[] enemies=8 storm=False result=False refill=False death=False dualSlash=12 hitSpark=6 xpOrb=4`.
  - Unity console errors: `count=0`.
  - Evidence direct camera render: `LETHE/Assets/_dev/Evidence/v1_combat_feel_pass_20260612.png`.

## Next Implementation

1. Jaewoo Play Mode review of the combat feel pass: dual-blade attack, hit feedback, Kalmuri size, XP bar, and 3-card level-up UI.
2. Discuss/decide the final basic-attack and hit-feedback model per weapon.
3. If M1 feel is accepted, replace the compressed M2 shortcut with real pacing: Gatekeeper kill -> result screen -> deficit survival -> resonance.

## Open Questions

- Is the new dual-blade attack readable enough, or does it need stronger animation/sound/VFX timing?
- Is the softened Kalmuri size acceptable in actual Play Mode?
- What should be the shared rule for weapon-specific echo synergy between dual blades and greatsword?
- Does the compressed M2 loop prove the direction, or does the real pacing need to be built before judgment?
- Should generated sheets be sliced/imported properly next, instead of runtime-cropped in the manager?

## Do Not Touch

Do not continue polishing `Dev_EchoSlice` or `Dev_Prototype_v0` as the main path. Do not add shop, meta progression, multi-region structure, or final boss.
