# Status

Last updated: 2026-06-16

## Current Snapshot

LETHE Unity work has been reset around the new consolidated design source of truth: `docs/design/README.md` + `LETHE_DESIGN_00..07`. The previous `Dev_Prototype_v0` attempt is kept only as historical/reference evidence; it is not the main implementation path anymore.

The current playable target is now:

```text
LETHE/Assets/_dev/Scenes/Dev_Prototype_v1.unity
LETHE/Assets/_dev/Scripts/PrototypeV1/
```

`Dev_Prototype_v1` starts from M1 HTML parity instead of patching the failed v0 prototype. It contains a fresh scene with only `Main Camera` and `V1_GameManager`; runtime objects are spawned by the new `Lethe.PrototypeV1.V1GameManager`.

Implemented in v1 now:

- player movement with separated weapon anchor and dual blade sprites.
- arena floor generation and camera follow.
- dual blades using the documented baseline: range `86px`, damage `15`, interval `0.36s`, arc `119°`.
- enemy spawn pressure profile with four role types represented in the runtime: eroder, drifting eye, split one, void priest, plus debug Gatekeeper.
- early survival damage ramp `0.24 -> 1.0` by `320s`.
- XP/level-up curve and OnGUI level-up overlay.
- HUD with HP/timer/phase/memory slots/echo panel/next forget candidate/ultimate progress.
- active Hungry Blades as real player-orbit blade damage, not generic all-memory orbit.
- Blood Reflection as blood mark/heal identity.
- highest-level forgetting, echo cap +5, resonance reacquire, and Blood Blade Storm debug path.
- Input System and Legacy input compatibility.
- 8x4 player/enemy sheets are cropped to one runtime frame instead of rendering the whole sheet.

This is an explicit scope change from the previous `Dev_Prototype_v0` tuning loop. `Weapon_Greatsword`, all 8 core memories, all 8 matching echoes, and 4 ultimate echoes are now inside the prototype validation scope. Shop, meta progression, multi-region completion, final boss, release UI/audio, Steam/build deployment, and `Assets/Lethe` promotion remain out of scope.

The current design sources are `docs/design/README.md` and `docs/design/LETHE_DESIGN_00..07`. The deleted older `LETHE_UNITY_*`, `LETHE_WEAPON_MEMORY_*`, and scattered balance/combat docs are superseded by that set.

The orchestration HTML interface now exists at `docs/orchestration/interface/index.html`, `docs/orchestration/interface/command.html`, and `docs/orchestration/interface/runbook.html`. AI-facing state lives under `docs/orchestration/state/`; human-facing reports live under `docs/orchestration/reports/YYYYMMDD/`.

The report/devlog/review migration is now applied physically: old `docs/reports/` daily files moved to `docs/orchestration/reports/YYYYMMDD/index.md|html`, old unit reports moved to `docs/orchestration/reports/YYYYMMDD/units/`, old `docs/devlog/` files moved to `docs/orchestration/devlog/YYYYMMDD.md`, and old review prompt/response files moved to `docs/orchestration/review_prompts/` and `docs/orchestration/review_responses/`. New work should not recreate legacy `docs/reports/`, `docs/devlog/`, `docs/review_prompts/`, or `docs/review_responses/` as normal source-of-truth folders.

The current development-docs plugin baseline from `docs/orchestration/MIGRATION_PROMPT.md` has been applied. `AGENTS.md` now uses a `Development Docs Plugin` section, `docs/orchestration/templates/HTML_INTERFACE_TEMPLATE.md` exists, legacy review pointer READMEs are readable, `reports/index.html` is generated as a newest-first date archive, daily report pages are generated as unit-card pages, and Discord delivery is documented as Project Orchestrator first with local direct-send scripts as trusted fallback only.

## Latest Verified Result

- Unity v1 weapon / slash VFX ScriptableObject data pass:
  - User direction: stop hardcoding slash/VFX tuning values; LETHE is now being built as a game, so balance, numbers, VFX, and hit feel should be player-focused and data-driven.
  - Extended `_dev` definition contracts:
    - `WeaponDefinition` now owns runtime combat rhythm values: range, damage, cadence, arc, target count, engage multiplier, knockback, hitstop, camera shake, echo size/damage scale, targeting mode, echo proc style, ultimate pattern, and follow-up timing.
    - Added `WeaponVfxProfile` for slash VFX, Kalmuri follow-up VFX, ultimate slash VFX, damage number colors/lifetimes, enemy flash colors/durations, and hit spark settings.
    - Added `SlashVfxEntry` entries with shape, anchor, flip, local offset, hand-mirror, rotation, scale, color, and lifetime.
  - `V1GameManager` now reads `dualBladesDefinition` and `greatswordDefinition` from scene references instead of relying on inline weapon/VFX literals for normal runtime.
  - Created and linked `_dev/Data/Weapons` assets:
    - `Weapon_DualBlades.asset`
    - `Weapon_Greatsword.asset`
    - `VFX_Weapon_DualBlades.asset`
    - `VFX_Weapon_Greatsword.asset`
  - Data-driven paths now cover:
    - 쌍검 기본 반달 2연 베기.
    - 쌍검 assist slash.
    - 대검 AoE/primary/shock/cutpoint slash.
    - 대검 assist slash.
    - 칼무리 잔향 후속타.
    - 대검 피의 칼폭풍 참격.
    - 피격 섬광.
    - 데미지 숫자 색/수명.
    - 적 흰색 피격 플래시 색/수명.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors.
  - Unity compilation errors: `count=0`.
  - Unity Play Mode targeted smoke:
    - `dualSO=True`
    - `dualVfx=True`
    - `dualEntries=4`
    - `dualSparkProfile=DualBladeHitSpark`
    - `greatSO=True`
    - `greatVfx=True`
    - `greatEntries=5`
    - `greatSparkProfile=GreatswordHitSpark`
    - `dualA=3`
    - `dualB=1`
    - `great=1`
    - `dualSpark=3`
    - `greatSpark=1`
    - `dmg=4`
  - Evidence capture saved: `LETHE/Assets/_dev/Evidence/v1_weapon_vfx_profile_data_20260616.png`.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 5 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `404 Not Found`, `project not found`.

- Unity v1 greatsword range / damage feedback / ranged enemy pass:
  - User review: greatsword crescent should be larger and match its damage range, not smaller; VFX lifetime was too short; hit feedback needed white flash and damage UI; ranged enemies should not kite backward.
  - Greatsword crescent range read was increased again:
    - AoE scale `0.88 -> 1.24`, lifetime `0.24 -> 0.42`.
    - Primary scale `0.66 -> 1.02`, lifetime `0.20 -> 0.34`.
    - The underlying crescent texture stays thin, so it should read as a large slash arc rather than a thick fan.
  - Added damage number UI through `V1DamageNumber`.
  - Enemy hit flash now turns pure white and lasts longer:
    - weapon hit flash `0.105s`.
    - non-weapon hit flash `0.075s`.
  - `DriftingEye` ranged enemy no longer backs away at range:
    - if outside stop range, it approaches.
    - once in range, it stays still and casts `EyeShot`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors.
  - Unity compilation errors: `count=0`.
  - Play Mode targeted smoke:
    - `greatCrescent=6`
    - `greatMaxScale=1.24`
    - `damageNumbers=5`
    - `whiteEnemies=5`
    - `eyeMovedAtRange=0.0000`
    - `eyeShots=1`
  - Evidence capture saved: `LETHE/Assets/_dev/Evidence/v1_damage_feedback_ranged_cast_20260616.png`.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 4 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.

- Unity v1 crescent slash size/timing tune:
  - User review: greatsword crescent was too thick and looked like a fan; dual-blade crescents were too small and disappeared too quickly.
  - Dual-blade primary crescents are now larger and last longer:
    - A scale `0.62 -> 0.78`, lifetime `0.13 -> 0.21`.
    - B scale `0.54 -> 0.68`, lifetime `0.15 -> 0.23`.
    - assist scale `0.38 -> 0.50`, lifetime `0.10 -> 0.15`.
  - Greatsword crescent scale/alpha was reduced:
    - AoE scale `1.02 -> 0.88`, alpha `0.40 -> 0.32`, lifetime `0.28 -> 0.24`.
    - primary scale `0.78 -> 0.66`.
  - `MakeWideCrescentSprite` was reshaped from a filled fan into a thinner game-like slash arc by narrowing the blade edge and reducing inner fill/glow.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors.
  - Unity compilation errors: `count=0`.
  - Play Mode targeted smoke:
    - `dualCrescent=6`
    - `dualMaxScale=0.78`
    - `kalmuriCrescent=10`
    - `greatCrescent=6`
    - `greatMaxScale=0.88`
    - `shock=1`
  - Evidence capture saved: `LETHE/Assets/_dev/Evidence/v1_crescent_slash_timing_size_20260616.png`.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 3 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.

- Unity v1 crescent slash feedback pass:
  - User review: movement/pause fixes worked, but hit feel was still weak.
  - Changed basic attack VFX language from thin iai lines to readable crescent slashes.
  - Dual blades now spawn two crossed half-moon slash sprites on the target:
    - `DualBladeCrescent_A`
    - `DualBladeCrescent_B`
  - Greatsword now spawns a large crescent AoE read plus a brighter primary crescent:
    - `GreatswordCrescent_Aoe`
    - `GreatswordCrescent_Primary`
  - Kalmuri echo follow-ups now use crescent sprites too, so the echo inherits the weapon slash language.
  - Greatsword Blood Blade Storm heavy slashes now use the wide crescent sprite.
  - Hit spark now uses an impact diamond instead of another slash line, reducing visual noise.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors.
  - Unity compilation errors: `count=0`.
  - Play Mode targeted smoke:
    - `dualCrescent=6`
    - `kalmuriCrescent=10`
    - `greatCrescent=6`
    - `heavyKalmuri=1`
    - `shock=1`
  - Evidence capture saved: `LETHE/Assets/_dev/Evidence/v1_crescent_slash_feedback_20260616.png`.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 2 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.

- Unity v1 pause / hitstop movement fix:
  - Fixed the issue where enemies continued moving while choosing weapon/level-up/result cards.
  - Added `V1GameManager.GameplayPaused` for blocking overlays:
    - start weapon selection.
    - level-up card selection.
    - forgetting result.
    - resonance refill.
    - death overlay.
  - `V1Enemy`, `V1Projectile`, and `V1EnemyShot` now respect the pause flag.
  - Split combat hitstop into `V1GameManager.HitstopActive`, so enemies/projectiles can freeze briefly while the player-side manager loop still updates.
  - Hitstop no longer returns before player/camera/weapon visual updates, so weapon/echo impacts should not make the character feel like movement input is interrupted.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors. Warnings are legacy v0/debug deprecated API warnings.
  - Unity compilation errors: `count=0`.
  - Play Mode targeted smoke:
    - `pauseDistance=0.0000`
    - `unpauseDistance=0.0240`
    - `weaponAnimAfterHitstop=0.220`
    - `hitstopAfterUpdate=0.060`
    - `gameplayPaused=False`
    - `hitstopActive=True`
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 1 unit heading ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.

- Unity v1 weapon select / hit feedback pass:
  - Added a run-start weapon selection overlay for `절단쌍검` and `장송대검`.
  - The run now starts only after the player chooses a weapon with a card click or number key `1/2`.
  - `F9` remains as debug/review weapon toggle after the run starts.
  - Replaced the temporary greatsword box visual with a procedural blade silhouette.
  - Twin-blade hit VFX now uses sharper target-local iai slash sprites instead of broad fan-like arcs.
  - Greatsword hit VFX now uses a heavier target-local cleave and impact diamond.
  - Weapon hit feedback was strengthened:
    - dual blades: quicker multi-hit knockback/readability.
    - greatsword: much stronger push, hitstop, shake, and heavy slash identity.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors. Warnings are legacy v0/debug deprecated API warnings.
  - Unity compilation errors: `count=0`.
  - Play Mode targeted smoke:
    - `beforeOverlay=True`
    - `afterOverlay=False`
    - `dualSlash=5`
    - `dualSpark=4`
    - `dualKnock=1.78`
    - `greatSlash=5`
    - `greatShock=1`
    - `greatKnock=5.53`
    - snapshot: `scene=v1 weapon=장송대검 elapsed=0.0 hp=210.0/210.0 level=1 xp=0/5 kills=1 memories=[HungryBlades:1] echoes=[] enemies=7 storm=False result=False refill=False death=False`.
  - Evidence capture saved: `LETHE/Assets/_dev/Evidence/v1_weapon_select_hit_feedback_20260615.png`.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 4 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `404 Not Found`, `project not found`.

- Unity v1 one-pass feedback batch:
  - Implemented the full next-work bundle requested for one-pass review.
  - Twin-blade target-local slashes were made more visible with larger primary/assist marks, stronger alpha, and slightly longer lifetimes.
  - Greatsword debug path now has distinct weapon visual behavior:
    - dual blades show two fast small blades.
    - greatsword hides the left blade and shows one larger cyan blade with slower swing animation.
    - greatsword hit VFX now includes a large primary slash plus shock marker.
  - Review pacing now moves the default run toward an M2 loop without debug buttons:
    - Blood Reflection appears around 14s.
    - Hungry Blades grows around 28s.
    - Blood Reflection grows around 42s.
    - Stopped Second fills the third memory slot around 50s.
    - first Gatekeeper appears at 62s.
    - deficit survival lasts 22s.
    - resonance reacquire has visible resonance VFX.
    - after the loop is restored, Kalmuri/Blood echoes can top to +5 for Blood Blade Storm review.
  - HUD now shows awakened +5 echoes and ultimate readiness as `칼무리 N/5 + 혈반 N/5`.
  - Blood Blade Storm now follows weapon pattern:
    - dual blades: frequent small rotating blood blades.
    - greatsword: slower large heavy blood slashes with stronger hitstop/shake.
  - Enemy spawn timing/radius was tightened for a denser review run.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors. Warnings are legacy v0/debug deprecated API warnings.
  - `unity_get_compilation_errors(port=7890, severity="all")`: `count=0`.
  - Unity Play Mode targeted smoke:
    - `resultAfterGate=True`
    - `activeMemories=3`
    - `dualSlash=3`
    - `greatSlash=4`
    - `multiFollow=6`
    - `heavyFollow=1`
    - `dualStorm=6`
    - `greatStorm=3`
    - `resonance=12`
    - snapshot: `weapon=장송대검`, `elapsed=87.0`, `memories=[HungryBlades:3,StoppedSecond:1,BloodReflection:2]`, `echoes=[BloodReflection:5,HungryBlades:5]`, `storm=True`.
  - Evidence capture saved: `LETHE/Assets/_dev/Evidence/v1_full_feedback_batch_20260615.png`.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 3 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `404 Not Found`, `project not found`.

- Unity v1 weapon spec / greatsword prep implementation:
  - Refactored `V1GameManager` weapon flow around `WeaponRuntimeSpec`, matching the design fields: range, damage, interval, arc, targeting mode, echo size/damage scale, echo proc style, and ultimate pattern.
  - Current default remains `절단쌍검`.
  - Added `장송대검` debug spec with `DensestArc`, `SingleHeavy`, larger range/damage/knockback/hitstop, `echoSizeScale=1.80`, `echoDamageScale=1.60`, and `ultimatePattern=FewHeavy`.
  - Added `F9` debug weapon toggle and HUD weapon display.
  - Weapon targeting and hit collection now use the active spec instead of twin-blade-only methods.
  - Kalmuri echo follow-up now carries the weapon spec through the pending queue:
    - 쌍검 `MultiSmall`: multiple small `KalmuriFollowup` cuts.
    - 대검 `SingleHeavy`: one `KalmuriFollowup_Heavy` per swing from the primary hit path.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors. Warnings are legacy v0/debug deprecated API warnings.
  - `unity_get_compilation_errors(port=7890, severity="all")`: `count=0`.
  - Unity Play Mode targeted smoke: `noAir=0 dualSlash=3 dualOldFan=0 dualFollow=6 greatSlash=4 heavyFollow=1 multiFollowStill=0`.
  - Evidence capture saved: `LETHE/Assets/_dev/Evidence/v1_weapon_spec_greatsword_prep_20260615.png`.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 2 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `404 Not Found`, `project not found`.

- Unity v1 target-local slash / Kalmuri follow-up implementation:
  - Implemented DEC-2026-06-12-05 in `V1GameManager`.
  - Twin-blade basic attack VFX no longer spawns player-origin fan/arc slash objects.
  - Primary weapon hit now spawns two target-local iai/slash marks on the enemy; cleave targets spawn smaller assist slash marks.
  - Kalmuri `MultiSmall` echo is queued as a short delayed follow-up from the weapon hit origin instead of firing in the same character-origin moment.
  - Twin-blade hitstop was reduced from `0.025s` to `0.018s` so echo follow-ups feel less like the character is pausing.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors. Warnings are legacy v0/debug deprecated API warnings.
  - `unity_get_compilation_errors(port=7890, severity="all")`: `count=0`.
  - Unity Play Mode targeted smoke: `noAirAfter=0 targetLocalSlash=3 playerFanArc=0 kalmuriFollowup=6 hitSpark=6`.
  - Evidence captures saved:
    - `LETHE/Assets/_dev/Evidence/v1_target_local_slash_echo_followup_20260615.png`
    - `LETHE/Assets/_dev/Evidence/v1_target_local_slash_echo_followup_20260615_scene.png`
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 1 unit heading ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `404 Not Found`, `project not found`.

- Basic attack VFX direction update:
  - New decision: basic attack VFX should not be a player-origin fan arc.
  - Twin-blade basic attack should read as target-local iai/slash marks on the enemy.
  - On-hit echoes should spawn as follow-up hits from the weapon hit origin.
  - Kalmuri `MultiSmall` should appear after the target-local weapon hit, not as a character-origin pause effect.
  - Next implementation target: remove current player/fan-like slash VFX and replace it with target-local slash + Kalmuri follow-up.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 6 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.

- Unity v1 no-air-swing / Kalmuri MultiSmall implementation:
  - Implemented the first code pass for DEC-2026-06-12-04.
  - Twin blades no longer swing when no enemy is inside `range * 1.15`.
  - Twin blades now acquire the nearest enemy and rotate the weapon anchor toward that target before swinging.
  - Twin blade cleave is collected from the target direction, not from stale movement direction.
  - Kalmuri echo now uses the twin-blade `MultiSmall` style: several small slash arcs around the hit enemy with local AoE damage.
  - Kalmuri +5 launch blade scale was reduced so it no longer covers the screen.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors. Warnings are legacy v0/debug deprecated API warnings.
  - `unity_get_compilation_errors(port=7890, severity="all")`: `count=0`.
  - Unity smoke result: `noAirBefore=0 noAirAfter=0 slashAfterTarget=3 kalmuriSmall=3 launch=1 hitSpark=2`.
  - Unity console errors after smoke: `count=0`.
  - Evidence direct camera render saved: `LETHE/Assets/_dev/Evidence/v1_no_air_swing_kalmuri_multismall_20260612.png`.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 5 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.

- Unity v1 combat feel pass:
  - Jaewoo review: camera/character scale were acceptable, but Kalmuri echo felt too large, basic dual-blade attacks lacked feel, XP needed a bar, and level-up choices should be larger 3-card selections.
  - Dual blades now spawn two offset slash arcs per attack instead of one flat arc.
  - Weapon sprites animate briefly on swing.
  - Weapon hits add short hitstop, light camera shake, enemy flash, hit spark, and enemy knockback/squash feedback.
  - Kalmuri echo slash scale and alpha were reduced; active Kalmuri orbit trail was also softened.
  - HUD now includes HP and XP bars.
  - Level-up UI is now a large 3-card choice layout with tag, title, and description.
  - `F8` input path was fixed in the Input System key switch.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors. Warnings are legacy v0/debug deprecated API warnings, not v1 errors.
  - `unity_get_compilation_errors(port=7890, severity="all")`: `count=0`.
  - Unity Play Mode M1 smoke snapshot: `scene=v1 elapsed=1.2 hp=210.0/210.0 level=2 xp=1/9 kills=4 memories=[HungryBlades:3,BloodReflection:2] echoes=[] enemies=8 storm=False result=False refill=False death=False dualSlash=12 hitSpark=6 xpOrb=4`.
  - Unity console errors after smoke: `count=0`.
  - Evidence direct camera render saved: `LETHE/Assets/_dev/Evidence/v1_combat_feel_pass_20260612.png`.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 3 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.

- Unity v1 M1/M2 loop pass:
  - `V1GameManager` now has debug smoke entry points for M1 and M2 (`DebugRunM1Smoke`, `DebugRunM2Smoke`, `DebugSnapshot`) plus an OnGUI `M2 Loop` button and `F8` shortcut.
  - Enemy kills now spawn collectable XP orbs instead of granting XP directly.
  - Enemy ranged shots can damage the player through `DamagePlayer`.
  - M2 smoke forces highest-level forgetting, result continuation, resonance reacquire, Kalmuri/Blood echoes at +5, and Blood Blade Storm activation.
  - Fixed runtime XP collection mutation by iterating a snapshot of the XP orb list.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 0 warnings, 0 errors.
  - `unity_get_compilation_errors(port=7890, severity="all")`: `count=0`.
  - Unity Play Mode M2 smoke snapshot after 120 forced frames: `scene=v1 elapsed=8.5 hp=155.2/210.0 level=2 xp=7/9 kills=10 memories=[BloodReflection:3,HungryBlades:3] echoes=[HungryBlades:5,BloodReflection:5] enemies=24 storm=True result=False refill=False death=False`.
  - Unity console errors after smoke: `count=0`.
  - Evidence capture saved: `LETHE/Assets/_dev/Evidence/v1_m2_smoke_20260612.png`.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 2 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.

- Unity v1 reset implementation:
  - Added `LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs`.
  - Added `LETHE/Assets/_dev/Scripts/PrototypeV1/Editor/V1SceneBuilder.cs`.
  - Added fresh scene `LETHE/Assets/_dev/Scenes/Dev_Prototype_v1.unity`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 0 warnings, 0 errors after the v1 patch.
  - Unity MCP selected `LETHE` on port `7890`.
  - `unity_get_compilation_errors(port=7890, severity="all")`: `count=0`.
  - `unity_scene_open(path="Assets/_dev/Scenes/Dev_Prototype_v1.unity")`: success.
  - hierarchy check: roots are `V1_GameManager` with `V1GameManager` and `Main Camera`.
  - Play Mode smoke: `player=True`, `enemies=2`, `renderers=107`, `playing=True`, `paused=False`.
  - Console after Input System fix: only AnkleBreaker MCP startup logs, no v1 runtime exception.
  - Game capture confirmed the 4-direction sprite sheet is now cropped to single player/enemy frames.
  - Scene saved with `unity_scene_save(port=7890)`.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.

- Unity basic resource pass: Codex imagegen produced 5 first-slice images; chroma-key cleanup produced transparent runtime sprites for player, enemy, and dual blades; the floor tile remains opaque by design.
- Unity MCP import: `unity_asset_import(port=7890)` imported 5 runtime sprites and 4 chroma source textures under `Assets/_dev/Art`; `unity_texture_set_sprite` configured the 5 runtime sprites as Sprite/Single with 100 pixels per unit.
- Unity MCP verification: `unity_asset_list(folder="Assets/_dev/Art/Sprites", type="Texture")` sees 5 Texture2D assets; `unity_asset_list(folder="Assets/_dev/Art/Source", type="Texture")` sees 4 Texture2D source assets.
- Runtime sprite GUIDs: player `a1ef9603867c4f24a84840ac22180c29`, enemy `bafc84ca59893594abe69b91563746bd`, floor `71bbd8c64392bde4a8ea63e92d2c1a2c`, left blade `691d8a000ca16fc41b69e21c808eb5e9`, right blade `33990fa8e271b0e49bddab9da881e4be`.
- Basic resource reporting: `npm.cmd run report` pass, `npm.cmd run report:check` pass, `node scripts\send_orchestrator_discord_report.js --latest-section --dry-run --print-payload` pass.
- Project Orchestrator notification: `npm.cmd run report:orchestrator:unit:dry` failed with `fetch failed`; escalated retry also failed; `Test-NetConnection 127.0.0.1 -Port 4317` returned `TcpTestSucceeded: False`.
- Unity runtime foundation pass: added `_dev` C# definitions/contracts for `WeaponDefinition`, `MemoryDefinition`, `EchoDefinition`, `EchoSynergyDefinition`, `FeedbackProfile`, runtime interfaces/base classes, `RunBuildState`, hit events, `HitResolver`, `EchoTriggerRouter`, `EchoProcLimiter`, `PoolService`, `FeedbackService`, `Health`, and `WeaponHitEmitter`.
- Unity compile verification: `Assets/Refresh` succeeded and `unity_get_compilation_errors(port=7890, severity="all")` returned `count: 0`, `isCompiling: false`.
- Unity basic combat scene pass: created `Assets/_dev/Scenes/Dev_EchoSlice.unity`, plus prefabs `Player_EchoShowcase`, `Enemy_TestWalker`, `Dev_TestArena`, and `Weapon_DualBlades_Runtime`.
- Unity hit verification: MCP `unity_execute_code` invoked `DualBladesController.Attack(enemy)` and enemy health changed `30 -> 22`, applying `8` damage through `HitResolver`.
- Unity scene verification: `unity_scene_info(port=7890)` returned active scene `Dev_EchoSlice`, path `Assets/_dev/Scenes/Dev_EchoSlice.unity`, dirty `false`, root objects `Dev_EchoSlice_Root` and `Main Camera`; `unity_search_missing_references(scope="scene")` returned `totalFound: 0`.
- Screenshot evidence attempt: first screenshot was discarded after scale correction; retry failed because Unity bridge became unreachable. `unity_editor_ping(port=7890)` returned `connected: false`.
- Basic combat scene reporting: `npm.cmd run report` pass, `npm.cmd run report:check` pass, `node scripts\send_orchestrator_discord_report.js --latest-section --dry-run --print-payload` pass for unit 14.
- Project Orchestrator notification for unit 14: `npm.cmd run report:orchestrator:unit:dry` failed with `fetch failed`.
- Unity MCP recovery: Unity Editor was relaunched directly with Unity `6000.3.10f1` and project path `C:/jaewoo/LETHE_Prototype/LETHE`; AnkleBreaker MCP rediscovered `LETHE` on port `7890`, and `unity_editor_ping(port=7890)` returned `connected: true`.
- Unity core echo VFX pass: generated 8 VFX images for Kalmuri, Blood, and Blood Blade Storm; source chroma images are preserved under `LETHE/Assets/_dev/Art/Source`; runtime alpha PNGs are under `LETHE/Assets/_dev/Art/Sprites/Echoes` and `LETHE/Assets/_dev/Art/Sprites/Ultimates`.
- Unity core echo prefab pass: created 6 echo prefabs under `Assets/_dev/Prefabs/Echoes` and 1 ultimate prefab under `Assets/_dev/Prefabs/Ultimates`.
- VFX evidence: `docs/orchestration/evidence/2026-06-11-echo-vfx-contact-sheet.png`.
- Unity VFX verification: `unity_asset_list(folder="Assets/_dev/Art/Sprites/Echoes", type="Texture")` found 6 textures; `unity_asset_list(folder="Assets/_dev/Art/Sprites/Ultimates", type="Texture")` found 2 textures; `unity_asset_list(folder="Assets/_dev/Prefabs/Echoes", type="Prefab")` found 6 prefabs; `unity_asset_list(folder="Assets/_dev/Prefabs/Ultimates", type="Prefab")` found 1 prefab.
- Unity compile verification after VFX import: `unity_get_compilation_errors(port=7890, severity="all")` returned `count: 0`, `isCompiling: false`.
- Unity core echo VFX reporting: `npm.cmd run report` pass, `npm.cmd run report:check` pass, `npm.cmd run report:orchestrator:unit:dry` failed with `fetch failed`.
- Unity playable debug loop: added `DevEchoSliceDebugController` to `Services` in `Dev_EchoSlice.unity`; Play Mode supports `1` base dual blades, `2` Kalmuri +1, `3` Kalmuri +5, `4` Blood +5, `5` Blood Blade Storm, and `Space` forced attack.
- Unity debug loop runtime verification: first Play Mode run exposed an Input System-only exception from `UnityEngine.Input.GetKeyDown`; fixed with `UnityEngine.InputSystem.Keyboard` under `ENABLE_INPUT_SYSTEM`.
- Final Unity debug loop verification: `unity_get_compilation_errors(port=7890, severity="all")` returned `count: 0`; Play Mode runtime check returned `controller=true`, `stormVisible=true`, `orbitVisible=true`, `healThreadVisible=true`, `enemyHealth=6`; console error log returned `count: 0`; missing reference scan returned `totalFound=0`; editor state after stopping Play Mode returned `sceneDirty=false`.
- Unity debug loop evidence: `docs/orchestration/evidence/2026-06-11-dev-echo-slice-play.png`.
- Unity debug loop reporting: `npm.cmd run report` pass, `npm.cmd run report:check` pass, `npm.cmd run report:orchestrator:unit:dry` failed with `fetch failed`.
- Unity morning review prompt: `docs/orchestration/review_prompts/2026-06-11-unity-echo-slice-jaewoo-review.md`.
- Unity promotion gate: `docs/design/LETHE_UNITY_ECHO_SLICE_PROMOTION_GATE.md`.
- Unity morning review docs status: jaewoo can open `Assets/_dev/Scenes/Dev_EchoSlice.unity`, press Play, use `1~5` and `Space`, then return `GO`, `ITERATE`, or `NO-GO` with weakest state and first fix.
- Unity final morning-readiness verification: `unity_editor_ping(port=7890)` connected to `LETHE`; `unity_editor_state(port=7890)` active scene `Assets/_dev/Scenes/Dev_EchoSlice.unity`, `isCompiling=false`, `sceneDirty=false`; compilation errors `count=0`; scene missing references `totalFound=0`; console errors `count=0`.
- Unity morning review docs reporting: `npm.cmd run report` pass, `npm.cmd run report:check` pass, `npm.cmd run report:orchestrator:unit:dry` failed with `fetch failed`.
- Unity pre-review hit feel polish: `DevEchoSliceDebugController` now adds procedural base swing arc, delayed Kalmuri +1 slash, 2-frame debug hit stop, short camera shake, 3-strand Blood heal thread, and orbit pulse for Kalmuri +5 / Blood Blade Storm.
- Unity pre-review hit feel verification: Unity refresh completed; compilation errors `count=0`; Play Mode mode `0~4` forced attack check observed `Debug_DualBladeSwingArc`, `Debug_HealThreadLine`, `Echo_Kalmuri_LaunchBlade(Clone)`, `Ultimate_BloodBladeStorm(Clone)`, and `Debug_KalmuriOrbit`; console errors `count=0`; missing references `totalFound=0`; editor state after stopping Play Mode `sceneDirty=false`.
- Unity pre-review hit feel reporting: `npm.cmd run report` pass, `npm.cmd run report:check` pass, `npm.cmd run report:orchestrator:unit:dry` failed with `fetch failed`.
- Unity gameplay slice repair plan: `docs/design/LETHE_UNITY_GAMEPLAY_SLICE_REPAIR_PLAN.md` added to define why the previous scene felt insufficient and how to turn it into a minimal game-like slice.
- Unity gameplay repair pass 1: added `DevPlayerController2D`, `DevEnemyChaseController`, `DevCameraFollow2D`, and `DevSpriteMotionAnimator`; updated `DualBladesController`, `TestEnemyController`, and `DevEchoSliceDebugController`.
- Unity gameplay repair scene wiring: `Weapon_DualBlades_Runtime` is now parented under `Player_EchoShowcase/WeaponAnchor`; player/enemy sprites are under `Visual` children; camera follows player; enemy chases player; debug panel now says WASD/arrow movement is available.
- Unity gameplay repair verification: compile errors `count=0`; scene missing references `totalFound=0`; console errors `count=0`; Play Mode check showed enemy moved toward player (`Enemy_TestWalker` near `-0.03` from original `1.40`), weapon parent `WeaponAnchor`, swing arc present, heal thread present; editor stopped with active scene `Assets/_dev/Scenes/Dev_EchoSlice.unity`, `sceneDirty=false`.
- Unity gameplay repair reporting: `npm.cmd run report` pass, `npm.cmd run report:check` pass, `npm.cmd run report:orchestrator:unit:dry` failed with `fetch failed`.
- Unity playable game slice plan: `docs/design/LETHE_UNITY_PLAYABLE_GAME_SLICE_PLAN.md` added to reset the working goal from echo VFX testing to a real 1-minute playable combat slice.
- Unity Phase 1 composition pass: camera orthographic size set to `4.15`; arena floor scale set to `13.5 x 8.2`; arena border line added; player scale set to `0.92`; enemy scale set to `0.84`; weapon scale set to `1.12`; weapon anchor/blade local transforms and sorting orders normalized; debug panel reduced.
- Unity Phase 1 verification: compile errors `count=0`; missing references `totalFound=0`; console errors `count=0`; Play Mode composition check returned `cameraSize=4.15`, `playerScale=(0.92,0.92,1.00)`, `enemyScale=(0.84,0.84,1.00)`, `weaponParent=WeaponAnchor`, `weaponScale=(1.12,1.12,1.00)`, `arenaBounds=true`, `swingArcPresent=true`; editor stopped with `sceneDirty=false`.
- Unity Phase 1 reporting: `npm.cmd run report` pass, `npm.cmd run report:check` pass, `npm.cmd run report:orchestrator:unit:dry` failed with `fetch failed`.
- Unity slice approach decision: `Dev_EchoSlice` is no longer the main path. It remains only as reference until `Dev_Prototype_v0` replaces it.
- Unity Prototype v0 plan: `docs/design/LETHE_UNITY_PROTOTYPE_V0_PLAN.md` added. The new main target is `Assets/_dev/Scenes/Dev_Prototype_v0.unity`, with a real combat loop before memory/forgetting/echo systems.
- Unity Prototype v0 planning reporting: `npm.cmd run report` pass, `npm.cmd run report:check` pass, `npm.cmd run report:orchestrator:unit:dry` failed with `fetch failed`.
- Unity Prototype v0 PRD: `docs/design/LETHE_UNITY_PROTOTYPE_V0_PRD.md` added as the consolidated execution contract across game design, combat, run structure, memory/forgetting/echo, data structure, milestones, and acceptance tests.
- Unity Prototype v0 PRD reporting: `npm.cmd run report` pass, `npm.cmd run report:check` pass, `npm.cmd run report:orchestrator:unit:dry` failed with `fetch failed`.
- New forgetting model `npm run qa:balance`: `GO_BALANCE_BASELINE`, first boss clear `100%`, full clear `60%`, death `40%`, first boss TTK median `20.19s`.
- New forgetting model `npm run balance:loop`: `GO_BALANCE_BASELINE`, first boss clear `80%`, full clear `60%`, death `40%`, first boss TTK median `23.91s`.
- `npm run playtest:package:dry`: pass.
- `npm run playtest:package`: pass, wrote `dist\lethe-v0.12-playtest`.
- `npm run report`: pass, regenerated 2026-06-10 report HTML and report archive.
- `npm run report:check`: pass, one 2026-06-10 unit report verified.
- `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- Forgetting feel spec reporting: `npm run report` pass, regenerated 2026-06-10 report with 2 unit reports.
- Forgetting feel spec reporting: `npm run report:check` pass, 2 unit headings verified.
- Accepted recovery lever for new model: player max HP `190 -> 210`.
- New planning response saved: `docs/orchestration/review_responses/2026-06-10-forgetting-model-gate.md`.
- New feel design spec saved: `docs/design/LETHE_FORGETTING_FEEL_SPEC.md`.
- Accepted recovery change: player max HP `180 -> 190`.
- Latest `npm run balance:loop` pass 1 after HP `190`: `GO_BALANCE_BASELINE`, first boss clear `100%`, full clear `60%`, death `40%`, first boss TTK median `18.97s`.
- Latest `npm run balance:loop` pass 2 after HP `190`: `GO_BALANCE_BASELINE`, first boss clear `100%`, full clear `60%`, death `40%`, first boss TTK median `20.18s`.
- `npm run playtest:package:dry`: pass.
- `npm run playtest:package`: pass, wrote `dist\lethe-v0.12-playtest`.
- Echo clarity patch: pass, no numeric balance changes.
- `npm run qa:postloss` with `CHROME_PATH=C:\Program Files\Google\Chrome\Application\chrome.exe`: pass, failures `[]`.
- `npm run qa:identity` with `CHROME_PATH=C:\Program Files\Google\Chrome\Application\chrome.exe`: pass, failures `[]`.
- `npm run playtest:package:dry`: pass after echo clarity patch.
- `npm run playtest:package`: pass after echo clarity patch, regenerated `dist\lethe-v0.12-playtest`.
- Unity transition system plan: `docs/design/LETHE_CORE_SYSTEMS_UNITY_PLAN.md` created.
- Unity first-slice rule freeze update: highest-level memory forgetting, echo overflow-to-overcharge, and balance target changes were reflected in `docs/design/`.
- Legacy docs current-state sync: `docs/CODEX_STATUS.md` and `docs/NEXT_TASKS.md` now summarize the Unity-transition direction and planned echo system at the top.
- Korean design docs split: `docs/design/README.md`, `LETHE_GAME_DESIGN_OVERVIEW.md`, `LETHE_CORE_SYSTEMS_UNITY_PLAN.md`, `LETHE_RUN_STRUCTURE.md`, `LETHE_COMBAT_DESIGN.md`, `LETHE_CONTENT_TABLES.md`, `LETHE_BALANCE_BASELINE.md`, and `LETHE_UNITY_VERTICAL_SLICE_SPEC.md`.
- Failed checks: none in the latest loop.
- Death phases in the latest loop: `압박 상승` 1, `망각 전조` 1.
- Generated balance report: `docs/balance/2026-06-09-v012-balance-qa.md`.
- Generated review prompt: `docs/orchestration/review_prompts/2026-06-09-balance-loop.md`.
- Rejected prior candidate: `laterCycleClimax 46 -> 42`; it worsened the loop to full clear `20%`, death `80%`, and was reverted.
- Previous failed baseline: `ITERATE_BALANCE`, first boss clear `100%`, full clear `20%`, death `60%`, first boss TTK median `26.42s`, death cluster `망각 전조` 3 runs.
- Previous evidence: `docs/balance/2026-06-08-v012-balance-qa.md`, `docs/orchestration/review_prompts/2026-06-08-balance-loop.md`.
- Prior accepted baseline: `GO_BALANCE_BASELINE`, first boss clear `100%`, full clear `60%`, death `40%`, first boss TTK median `20.73s`.
- `npm run playtest:package:dry`: pass.
- `npm run playtest:package`: pass, regenerated `dist\lethe-v0.12-playtest`.
- `npm run report`: pass.
- `npm run report:check`: pass.
- `npm run report:discord:unit:dry`: pass, latest unit points to `docs/orchestration/reports/20260608/units/2026-06-08-10-오케스트레이션-리포트와-개발로그-실제-마이그레이션.html`.
- `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- `npm run report:discord:unit`: blocked by approval reviewer; next trusted-local command is `npm run report:discord:unit`.
- `npm run report:discord:unit`: blocked by approval reviewer; next trusted-local command is `npm run report:discord:unit`.
- Latest orchestration structure cleanup verification: `npm.cmd run report`, `npm.cmd run report:check`, `npm.cmd run report:discord:unit:dry`, and `npm.cmd run doctor` passed. Actual Discord send succeeded after explicit user request. Latest commit `5949452` was pushed to `main`.
- `docs/orchestration/interface/index.html`: present and updated for the current v0.12 human-test gate.
- `docs/orchestration/interface/command.html`: present as the compact next-instruction block.
- `docs/orchestration/interface/runbook.html`: present as the operating-procedure block.
- `docs/orchestration/reports/index.html`: present as a human-readable date list.
- `docs/orchestration/reports/20260608/index.html`: current daily report page.
- `docs/orchestration/reports/20260608/units/`: current work-unit report collection.
- `docs/orchestration/devlog/index.html`: present as a human-readable devlog list.
- Latest dashboard refresh reporting: `npm run report`, `npm run report:check`, `npm run report:discord:unit:dry`, and `npm run doctor` passed; actual Discord send was blocked by approval reviewer.
- Latest Korean dashboard normalization reporting: `npm run report`, `npm run report:check`, `npm run report:discord:unit:dry`, and `npm run doctor` passed; actual Discord send was blocked by approval reviewer.
- Latest dashboard compaction: `docs/orchestration/interface/index.html` was shortened so it no longer duplicates `interface/command.html` or `reports/`.
- Latest development-docs plugin alignment: updated `AGENTS.md`, `docs/orchestration/README.md`, state runbook/context/tasks/decision log, legacy migration map, `docs/DISCORD_REPORTING.md`, and added `docs/orchestration/templates/HTML_INTERFACE_TEMPLATE.md`. Verification is this work unit's report/check pass.
- Latest template overwrite alignment: updated report generator so `docs/orchestration/reports/index.html` lists date journals newest-first with title/date/summary and `docs/orchestration/reports/YYYYMMDD/index.html` shows that day's unit cards. Unit pages now include a back link. New devlog guidance uses `YYYY-MM-DD.md`.

## Current Blocker

No active Unity blocker. Unity MCP is currently reachable on port `7890`.

Known rough edge: the bundled imagegen chroma-key helper could not run because Pillow is not installed in the active Python environment. The 2026-06-11 VFX alpha cleanup used Windows `System.Drawing` instead.

Discord actual send for the latest orchestration structure cleanup succeeded after explicit user request. Historical approval blocks remain recorded in the relevant devlog/report entries.

Project Orchestrator Discord intake is now connected through `scripts/send_orchestrator_discord_report.js` and the `report:orchestrator:*` npm scripts. The legacy `npm run report:discord:unit:dry` and `npm run report:discord:unit` commands remain documented as trusted-local fallback commands only.

## Current Next Step

Continue from `Dev_Prototype_v1`, not `Dev_Prototype_v0`.

Next implementation step:

1. Let jaewoo run `Dev_Prototype_v1`, choose a start weapon from the new overlay, and give one combined feedback pass.
2. Review checklist:
   - 시작 무기 선택이 런 시작 UX로 자연스럽게 읽히는가?
   - 쌍검 기본공격이 적 위치 발도선으로 읽히는가?
   - 적이 맞을 때 넉백/피격/공간 반응이 충분한가?
   - 칼무리 잔향이 기본공격 뒤 후속타로 읽히는가?
   - 대검이 느린 큰 한 방으로 읽히는가?
   - 60~120초 안에 망각 -> 결손 -> 공명 -> +5 잔향 -> 피의 칼폭풍 흐름이 보이는가?
   - 쌍검/대검 피의 칼폭풍 차이가 보이는가?
3. After feedback, choose a narrow next pass: attack readability, pacing/balance, UI clarity, or art replacement.

Do not promote `_dev` assets to `Assets/Lethe` until `Dev_Prototype_v1` receives explicit `GO`.

For reporting/Discord notification, use `npm run report:orchestrator:unit:dry` before real sends, then `npm run report:orchestrator:unit` when the Project Orchestrator is running and the report is ready.

## Current Source Of Truth

- Top-level rules: `AGENTS.md`
- 기획·개발 기준 세트 (단일 source of truth): `docs/design/` → `README.md` + `LETHE_DESIGN_00..06`
  - 코어/감정/범위: `LETHE_DESIGN_00_OVERVIEW.md`
  - 런 루프/타임라인: `LETHE_DESIGN_01_RUN_LOOP.md`
  - 전투/무기/적/보스: `LETHE_DESIGN_02_COMBAT.md`
  - 기억/망각/잔향/공명/궁극: `LETHE_DESIGN_03_MEMORY_ECHO.md`
  - 밸런스/수치: `LETHE_DESIGN_04_BALANCE.md`
  - UI/UX: `LETHE_DESIGN_05_UI_UX.md`
  - Unity 구현 계획: `LETHE_DESIGN_06_BUILD_PLAN.md`
  - 에셋/VFX/클래스 연결: `LETHE_DESIGN_07_ASSETS_VFX.md`
- Detailed legacy status archive: `docs/CODEX_STATUS.md`
- Current orchestration task: `docs/orchestration/state/CURRENT_TASK.md`

> 2026-06-12: 분산·충돌하던 게임 기획/PRD 문서(A군 18개)와 에셋/아트 가이드(B군 5개)는 위 `LETHE_DESIGN_00..07` 세트로 통합·삭제됨. HTML 코어 수치(`src/game.js`, `v0.12-balance-1`)를 이식 기준으로 삼는다. 결정: DEC-2026-06-12-01/02/03.

## Latest Complete Prototype PRD

- Added `docs/design/LETHE_UNITY_COMPLETE_PROTOTYPE_PRD.md`.
- The new prototype target is no longer `쌍검 + 칼무리 + 혈반` only.
- Complete Prototype scope:
  - `Weapon_DualBlades`
  - `Weapon_Greatsword`
  - 8 active memories
  - 8 matching echoes
  - 4 ultimate echoes
  - 4 enemy role types
  - 60~120 second compressed debug smoke
- Next implementation sequence:
  - C1 Data-Driven Core
  - C2 Weapon Pair
  - C3 Active Memories 8
  - C4 Echoes 8
  - C5 Ultimate Echoes 4
  - C6 Enemies / Encounter
  - C7 Visual / Feedback Pass
  - C8 Complete Prototype Gate
- Reporting verification:
  - `npm.cmd run report`: passed, 16 unit reports generated.
  - `npm.cmd run report:check`: passed, 16 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.

## Latest Complete Prototype Implementation

- Added C1 scaffolding:
  - `EnemyDefinition`
  - `RewardPoolDefinition`
  - `MemoryInventory`
  - `EchoInventory`
  - `ForgetService`
  - `ResonanceService`
  - `UltimateEchoService`
  - `RewardService`
  - `DebugStateInjector`
- `PrototypeWeaponController` now supports:
  - `Weapon_DualBlades`: fast, smaller proc rhythm.
  - `Weapon_Greatsword`: slower, wider, heavier rhythm.
- `PrototypeGameManager` now has a complete prototype catalog:
  - 8 memories.
  - 8 echoes.
  - 4 ultimate echoes.
  - F1-F8/debug buttons for choice, forget, memory add, echo levels, weapon switch, all memories, all ultimates.
- `PrototypeEnemySpawner` now creates four role ids:
  - `Enemy_MeleeChaser`
  - `Enemy_RangedEye`
  - `Enemy_Splitter`
  - `Enemy_EliteGatekeeper`
- Verification:
  - Unity compile errors `0`.
  - scene missing references `0`.
  - Play Mode console errors `0` after snapshot iteration fix.
  - runtime smoke confirmed 대검, 8 active memories, 8 echoes, 4 synergies, and 4 enemy roles.
  - `npm.cmd run report`: passed, 17 unit reports generated.
  - `npm.cmd run report:check`: passed, 17 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.
- Remaining risk:
  - most non-Kalmuri/Blood effects reuse placeholder sprites/colors.
  - data is in code catalog first, not yet `_dev/Data` ScriptableObject assets.
  - balance and final runtime class split still need follow-up.

## Latest Memory / Echo Visibility Patch

- User review: other memories appeared to have no VFX.
- Clarification: only Kalmuri/Blood/Blood Blade Storm had dedicated sprite VFX; the remaining memory/echo families were mostly logic plus reused sprite/color placeholders.
- Added procedural VFX so every memory/echo family has a visible shape even before dedicated art:
  - Execution: white crack cross.
  - Homing: directional arrow/shot lines.
  - Shockwave: double expanding rings.
  - TimeStop: clock ring and clock hands.
  - AshenGuard: shield ring and guard bar.
  - Brand: purple diamond/brand slash.
- `F7` now spawns an immediate 8-memory preview around the player.
- `F4/F5/F8` now spawn immediate echo preview shapes around the player.
- Persistent active memory loops now draw procedural shapes for the six non-Kalmuri/Blood memories.
- Verification:
  - Unity compile errors `0`.
  - scene missing references `0`.
  - Play Mode console errors `0`.
  - runtime smoke after F7/F5: active memories `8`, echoes `8`, line renderers `283`, procedural VFX objects `276`.
  - `npm.cmd run report`: passed, 18 unit reports generated.
  - `npm.cmd run report:check`: passed, 18 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.
- Remaining risk:
  - this pass intentionally makes effects very visible; density may be too high.
  - dedicated sprite/VFX art is still needed for the six non-Kalmuri/Blood families.

## Latest Memory Behavior Correction

- User review: making every memory orbit or preview around the character is wrong and does not reflect the HTML prototype/design.
- Diagnosis: this was an implementation shortcut, not a source-document problem. The design already says:
  - Kalmuri is the player-orbit blade memory.
  - ShatterWave should create shockwaves at weapon/enemy positions.
  - StoppedSecond should create time effects around affected enemies.
  - Active memory builds should operate around 2~3 memories, not all 8 at once.
- Corrected prototype behavior:
  - `F7` now activates only 3 showcase memories: HungryBlades +3, ShatterWave +2, StoppedSecond +1.
  - Active memory slot cap is now `3`.
  - Removed the all-memory player-orbit preview/persistent loop behavior.
  - Kalmuri remains the only player-orbit memory, with blade count and orbit speed scaling by level.
  - ShatterWave now triggers wave VFX/damage around the nearest enemy.
  - StoppedSecond now triggers clock/time VFX around the nearest enemy.
  - AshenGuard only shows player-side shield feedback when defensive conditions matter.
- Verification:
  - Unity compile errors `0`.
  - scene missing references `0`.
  - Play Mode console errors `0`.
  - F7 smoke: active memories `3`, `Memory_HungryBlades:3`, `Memory_ShatterWave:2`, `Memory_StoppedSecond:1`.
  - F7 smoke VFX: Kalmuri orbit `4`, ShatterWave enemy waves `39`, StoppedSecond enemy clocks `29`.
  - Wrong previous VFX categories: `Persistent=0`, `MemoryPreview/EchoPreview=0`.
  - Editor state after stop: `isPlaying=false`, `isCompiling=false`, scene `Dev_Prototype_v0`, `sceneDirty=false`.
  - `npm.cmd run report`: passed, 19 unit reports generated.
  - `npm.cmd run report:check`: passed, 19 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.

## Latest Prototype v0 Result

- `Assets/_dev/Scenes/Dev_Prototype_v0.unity` now exists as the main Unity prototype scene.
- Generated and imported new 4-direction player/enemy sprite sheets:
  - `Assets/_dev/Art/Sprites/Characters/Player/sheet_player_4dir.png`
  - `Assets/_dev/Art/Sprites/Enemies/Chaser/sheet_enemy_chaser_4dir.png`
- Added runtime prototype scripts under `Assets/_dev/Scripts/Prototype/`.
- Added prototype prefabs:
  - `Assets/_dev/Prefabs/Characters/Prefab_Player_Prototype.prefab`
  - `Assets/_dev/Prefabs/Enemies/Prefab_Enemy_MeleeChaser.prefab`
- Added first data assets:
  - `Weapon_DualBlades`
  - `Memory_HungryBlades`
  - `Memory_BloodReflection`
  - `Echo_Kalmuri`
  - `Echo_Blood`
  - `Synergy_BloodBladeStorm`
- M1-M5 prototype loop exists in `_dev`:
  - M1 scene skeleton, camera follow, arena, sprite animation.
  - M2 7 enemy spawn/chase, auto targeting, dual-blade hits, HP/death/respawn.
  - M3 kill-based memory choice UI.
  - M4 highest-level forgetting and Kalmuri/Blood echo combat effects.
  - M5 resonance reacquire bonus and Blood Blade Storm unlock/effect.
- Verification:
  - Unity compilation errors: `0`.
  - Scene missing references: `0`.
  - Console errors during Play Mode smoke: `0`.
  - Combat smoke: enemies forced near player, 8 seconds produced `kills=7`.
  - M5 smoke: forced resonance/echo +5 unlocked ultimate; 5 seconds produced `kills=148`, `playerHp=100`, console errors `0`.
- Known tuning risk: Blood Blade Storm is intentionally overpowered for first hype verification and needs a balance pass after jaewoo review.

## Latest Visual Fix

- Fixed the player-looking-like-a-sword issue.
- Replaced the player sheet with a weaponless 4-direction idle/walk body sheet.
- Kept weapons as separate `WeaponAnchor/BladeVisuals` children.
- Adjusted player PPU to `115`, blade sprite PPU to `800`, and blade scene/prefab scale to `0.25`.
- Evidence:
  - `LETHE/Assets/_dev/Evidence/player_weapon_separated_game.png`
  - `LETHE/Assets/_dev/Evidence/player_weapon_separated_clean.png`
- Verification: Unity compile errors `0`, missing references `0`, Play Mode console errors `0`, active scene `Dev_Prototype_v0`, `sceneDirty=false`.

## Latest Font / VFX Fix

- Added Pretendard OTF files and license under `LETHE/Assets/_dev/Fonts`.
- Connected `Pretendard-Regular.otf` to `PrototypeGameManager.koreanFont`.
- Converted the prototype HUD labels and memory/echo names to Korean.
- Connected sprite VFX references for active Kalmuri, echo Kalmuri, active Blood, echo Blood, and Blood Blade Storm.
- Reduced active memory loop VFX scale/sorting so it supports the character instead of covering it.
- Added `docs/design/LETHE_RELEASE_ART_FONT_VFX_PLAN.md`.
- Evidence: `LETHE/Assets/_dev/Evidence/korean_hud_memory_vfx_game.png`.
- Verification: Unity compile errors `0`, missing references `0`, Play Mode console errors `0`, active scene `Dev_Prototype_v0`, `sceneDirty=false`.

## Latest Art Replacement Pass

- Added `docs/design/LETHE_UNITY_ART_DIRECTION_REPLACEMENT_PLAN.md`.
- Replaced the main `_dev` visual set with LETHE-specific generated art:
  - player 4-direction body sheet.
  - enemy chaser 4-direction sheet.
  - dark river/obsidian arena tile.
  - short cyan memory-glass dual blades.
  - Kalmuri/Blood/Blood Blade Storm VFX sprites.
- Preserved chroma-key source files under `LETHE/Assets/_dev/Art/Source`.
- Converted runtime sprites to alpha PNG for Unity use; the green background is not used at runtime.
- Added `PrototypeSpriteVfx` and wired `PrototypeGameManager` so active memory, echo, awakened echo, and ultimate events spawn sprite VFX in addition to existing damage/log paths.
- Evidence:
  - `LETHE/Assets/_dev/Evidence/lethe_art_replacement_vfx_game.png`.
- Verification:
  - Unity compile errors: `0`.
  - Scene missing references: `0`.
  - Play Mode console errors: `0`.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.
  - Runtime review spawn confirmed 5 VFX sprite references:
    - `spr_kalmuri_orbit_blade_01`
    - `spr_kalmuri_echo_slash_01`
    - `spr_blood_mark_01`
    - `spr_blood_bloom_01`
    - `spr_blood_blade_storm_ring_01`
  - Active scene `Dev_Prototype_v0`, `sceneDirty=false`.
- Excluded from this commit by scope: `LETHE/Assets/_dev/Fonts/` and `.vscode/`. The scene `koreanFont` reference is cleared so the committed scene does not depend on local-only font files.

## Latest Combat Interaction Pass

- Jaewoo review after the first prototype: not bad, but attack range felt too small and weapon/enemy interaction was not physical enough.
- Prototype dual blades now use a real cleave model instead of one nearest-target poke:
  - range `2.35`.
  - arc `108` degrees.
  - max `5` targets per swing.
  - primary/secondary damage split.
  - immediate enemy knockback snap plus decay velocity.
- `PrototypeEnemySpawner` now exposes arc target collection for runtime combat.
- `PrototypeEnemy` now has hit knockback and short contact-damage lockout.
- Scene and player prefab serialized weapon values were synced.
- Verification:
  - Unity compile errors `0`.
  - scene missing references `0`.
  - Play Mode console errors `0`.
  - forced base swing hit 5 enemies: primary `28.0 -> 17.5`, secondary `28.0 -> 20.4`, all targets snapped outward.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, `13` unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.
  - evidence: `LETHE/Assets/_dev/Evidence/prototype_weapon_range_interaction_game.png`.

## Latest Memory Hunting Window Pass

- Jaewoo review after the cleave pass: memory seemed to become echo too quickly, so active memory hunting strength was hard to judge.
- Added an active-memory protection gate before automatic forgetting:
  - first forget `26` kills.
  - forget interval `14` kills.
  - memory protection `14` kills and `18` seconds after choosing/reacquiring a memory.
  - HUD shows remaining protection as `보호 N킬/N초`.
- Verification:
  - Unity compile errors `0`.
  - scene missing references `0`.
  - Play Mode console errors `0`.
  - protected forced state at `kills=31`: active memory stayed active and no echo was created.
  - expired forced state: active memory converted to echo normally.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, `14` unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.

## Latest Memory Identity / Base Balance Pass

- Jaewoo review: Kalmuri/Blood memory identity was not readable; basic attack was carrying too much damage and knockback; blue character-side line VFX was distracting.
- Basic dual blades were nerfed into a weaker base layer:
  - damage `5.8`.
  - range `2.15`.
  - max targets `4`.
  - secondary damage multiplier `0.48`.
  - primary/secondary knockback `1.45/0.85`.
- Active Kalmuri now has independent orbit blade ticks around the player and nearby enemy cuts, instead of relying mostly on basic attack on-hit text/lines.
- Active Blood now has periodic red mark pulses and a heal bloom, separating it from Kalmuri's cyan blade language.
- Removed the old persistent blue `ActiveHungryBladesOrbit` line that was drawing across the character.
- Verification:
  - Unity compile errors `0`.
  - scene missing references `0`.
  - Play Mode console errors `0`.
  - base swing primary `28.0 -> 22.2`, secondary `28.0 -> 25.2`, fifth target not hit.
  - Kalmuri level 1 tick damaged `2` targets, spawned `4` Kalmuri sprites, old blue orbit lines `0`.
  - Blood level 1 tick damaged `2` targets, healed player `72.0 -> 72.7`, spawned `3` blood sprites.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, `15` unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.
  - evidence: `LETHE/Assets/_dev/Evidence/prototype_memory_identity_pass_game.png`.
