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
- DEC-2026-06-12-04 first implementation exists: no air swings, twin-blade nearest targeting, Kalmuri `MultiSmall` echo style.
- DEC-2026-06-12-05 first implementation exists: target-local twin-blade slash VFX and delayed Kalmuri follow-up from hit origin.
- Weapon rhythm structure prep exists: `WeaponRuntimeSpec` supports current dual blades and debug greatsword paths without copying the weapon/echo loop.
- One-pass review batch exists: twin-blade visibility, greatsword visual behavior, review M2 pacing, resonance VFX, awakened echo HUD, weapon-patterned Blood Blade Storm, and denser combat pressure.
- Weapon selection / hit feedback pass exists: run-start weapon card selection, sharper target-local weapon VFX, stronger enemy knockback, and a non-box greatsword silhouette.
- Pause / hitstop movement fix exists: enemies/projectiles stop during card overlays, while hitstop no longer blocks player-side movement/visual updates.
- Crescent slash feedback pass exists: dual blades use two target-local half-moon slashes, greatsword uses a large crescent AoE read, and Kalmuri follow-ups reuse crescent language.
- Crescent size/timing tune exists: dual-blade crescents are bigger and last longer, while greatsword crescent is thinner and less fan-like.
- Damage feedback / ranged enemy pass exists: greatsword slash is range-sized again, enemies flash white longer, damage numbers appear, and ranged enemies stand still to cast once in range.
- Weapon / slash VFX data pass exists: weapon rhythm, slash VFX, Kalmuri follow-up VFX, heavy ultimate slash VFX, hit spark, damage number, and enemy flash tuning for dual blades/greatsword are controlled by `_dev/Data/Weapons` ScriptableObject assets.
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

- A-I EPIC / data contract pass:
  - Added root entry docs: `docs/PRD.md`, `docs/TECH.md`, `docs/TASK.md`, `docs/TEST.md`, `docs/CHANGELOG.md`.
  - Updated `AGENTS.md` read order to use these docs before orchestration state files.
  - Cleaned `docs/orchestration/state/NEXT_TASKS.md` around A-I work.
  - Expanded `DefinitionTypes.cs` with memory/echo/ultimate/enemy/encounter data contracts.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors.
  - Unity MCP editor verification not run because Unity MCP tools were not exposed in the current tool list.
- B-step hit feel / echo readability first pass:
  - Kalmuri follow-up now has a target-local range ring.
  - Blood-marked weapon hits now produce a red heal thread and small heal.
  - Blood Bloom now emits a heal thread.
  - Enemy knockback cap increased to `6.2`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors.
- C-step real M2 loop readability first pass:
  - HUD now shows the current M2 loop objective/status.
  - Level-up cards can offer `멈춘 1초` as the third active memory after Blood Reflection is acquired.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors.
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
- No-air-swing / Kalmuri MultiSmall pass:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors. Warnings are old v0/debug deprecated API usage.
  - `unity_get_compilation_errors(port=7890, severity="all")`: `count=0`.
  - Play Mode targeted smoke: `noAirBefore=0 noAirAfter=0 slashAfterTarget=3 kalmuriSmall=3 launch=1 hitSpark=2`.
  - Unity console errors: `count=0`.
  - Evidence direct camera render: `LETHE/Assets/_dev/Evidence/v1_no_air_swing_kalmuri_multismall_20260612.png`.
- Target-local slash / Kalmuri follow-up pass:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors. Warnings are old v0/debug deprecated API usage.
  - `unity_get_compilation_errors(port=7890, severity="all")`: `count=0`.
  - Play Mode targeted smoke: `noAirAfter=0 targetLocalSlash=3 playerFanArc=0 kalmuriFollowup=6 hitSpark=6`.
  - Evidence captures:
    - `LETHE/Assets/_dev/Evidence/v1_target_local_slash_echo_followup_20260615.png`.
    - `LETHE/Assets/_dev/Evidence/v1_target_local_slash_echo_followup_20260615_scene.png`.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 1 unit heading ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `404 Not Found`, `project not found`.
- Weapon spec / greatsword prep pass:
  - `V1GameManager` now uses `WeaponRuntimeSpec` for targeting, hit collection, base damage, hitstop/shake, echo size/damage scale, echo proc style, and ultimate pattern.
  - Default weapon remains dual blades.
  - `F9` toggles the debug greatsword spec.
  - Greatsword path uses `DensestArc` targeting and `SingleHeavy` Kalmuri follow-up.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors. Warnings are old v0/debug deprecated API usage.
  - `unity_get_compilation_errors(port=7890, severity="all")`: `count=0`.
  - Play Mode targeted smoke: `noAir=0 dualSlash=3 dualOldFan=0 dualFollow=6 greatSlash=4 heavyFollow=1 multiFollowStill=0`.
  - Evidence capture: `LETHE/Assets/_dev/Evidence/v1_weapon_spec_greatsword_prep_20260615.png`.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 2 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `404 Not Found`, `project not found`.
- One-pass feedback batch:
  - Twin-blade target-local slash VFX was made larger and more readable.
  - Greatsword debug path now shows a single larger weapon visual, slow swing, larger slash, and shock marker.
  - Default review pacing now drives the run toward Blood Reflection, boosted Hungry/Blood memories, a third memory slot, first Gatekeeper at 62s, shorter 22s deficit survival, resonance VFX, +5 echoes, and Blood Blade Storm.
  - HUD now shows echo awakening and ultimate readiness counts.
  - Blood Blade Storm now branches by weapon pattern: dual-blade small fast storm vs greatsword heavy slashes.
  - Enemy spawn radius/timing was tightened for denser review combat.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors. Warnings are old v0/debug deprecated API usage.
  - `unity_get_compilation_errors(port=7890, severity="all")`: `count=0`.
  - Play Mode targeted smoke: `resultAfterGate=True activeMemories=3 dualSlash=3 greatSlash=4 multiFollow=6 heavyFollow=1 dualStorm=6 greatStorm=3 resonance=12`.
  - Evidence capture: `LETHE/Assets/_dev/Evidence/v1_full_feedback_batch_20260615.png`.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 3 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `404 Not Found`, `project not found`.
- Weapon selection / hit feedback pass:
  - Start weapon selection overlay now appears before the run begins.
  - `1` chooses `절단쌍검`, `2` chooses `장송대검`; card click also starts the run.
  - `F9` remains as a debug/review toggle after start.
  - Greatsword visual is now a procedural blade silhouette instead of a rectangular block.
  - Twin-blade hit VFX uses sharper target-local iai slash sprites.
  - Greatsword hit VFX uses a larger target-local heavy slash plus impact diamond.
  - Weapon hit knockback was increased, with greatsword tuned much heavier than dual blades.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors. Warnings are old v0/debug deprecated API usage.
  - Unity compile errors: `count=0`.
  - Play Mode targeted smoke: `beforeOverlay=True afterOverlay=False dualSlash=5 dualSpark=4 dualKnock=1.78 greatSlash=5 greatShock=1 greatKnock=5.53`.
  - Evidence capture: `LETHE/Assets/_dev/Evidence/v1_weapon_select_hit_feedback_20260615.png`.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 4 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `404 Not Found`, `project not found`.
- Pause / hitstop movement fix:
  - Added `GameplayPaused` for blocking overlays.
  - Added `HitstopActive` for combat impact freeze.
  - `V1Enemy`, `V1Projectile`, and `V1EnemyShot` now respect pause/freeze flags.
  - Hitstop now updates player, camera, and weapon visuals before returning, so character movement should not feel interrupted by weapon/greatsword/Kalmuri impact pauses.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors.
  - Unity compile errors: `count=0`.
  - Play Mode targeted smoke: `pauseDistance=0.0000 unpauseDistance=0.0240 weaponAnimAfterHitstop=0.220 hitstopAfterUpdate=0.060 gameplayPaused=False hitstopActive=True`.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 1 unit heading ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.
- Crescent slash feedback pass:
  - Replaced thin target-local slash lines with crescent-shaped runtime sprites.
  - Dual blades now create two quick half-moon slashes per primary hit.
  - Greatsword now creates a large crescent AoE read plus a bright primary crescent.
  - Kalmuri follow-up and greatsword storm heavy slashes now use crescent sprites.
  - Hit spark uses impact diamond feedback instead of another slash line.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors.
  - Unity compile errors: `count=0`.
  - Play Mode targeted smoke: `dualCrescent=6 kalmuriCrescent=10 greatCrescent=6 heavyKalmuri=1 shock=1`.
  - Evidence capture: `LETHE/Assets/_dev/Evidence/v1_crescent_slash_feedback_20260616.png`.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 2 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.
- Crescent size/timing tune:
  - Dual blade primary crescents were scaled up and made longer-lived.
  - Dual blade assist crescents were also enlarged.
  - Greatsword crescent scale/alpha was reduced.
  - `MakeWideCrescentSprite` now draws a thinner slash arc with less inner fill, so it should read less like a fan.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors.
  - Unity compile errors: `count=0`.
  - Play Mode targeted smoke: `dualCrescent=6 dualMaxScale=0.78 kalmuriCrescent=10 greatCrescent=6 greatMaxScale=0.88 shock=1`.
  - Evidence capture: `LETHE/Assets/_dev/Evidence/v1_crescent_slash_timing_size_20260616.png`.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 3 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.
- Damage feedback / ranged enemy pass:
  - Greatsword crescent AoE scale/lifetime increased to `1.24 / 0.42s`.
  - Greatsword primary crescent scale/lifetime increased to `1.02 / 0.34s`.
  - Added `V1DamageNumber` floating damage UI.
  - Enemy hit flash now turns pure white for longer.
  - DriftingEye ranged enemy no longer retreats; it approaches until in range, then stands still and casts.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors.
  - Unity compile errors: `count=0`.
  - Play Mode targeted smoke: `greatCrescent=6 greatMaxScale=1.24 damageNumbers=5 whiteEnemies=5 eyeMovedAtRange=0.0000 eyeShots=1`.
  - Evidence capture: `LETHE/Assets/_dev/Evidence/v1_damage_feedback_ranged_cast_20260616.png`.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 4 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.
- Weapon / slash VFX ScriptableObject data pass:
  - Added `WeaponVfxProfile` and `SlashVfxEntry` to `_dev` definition contracts.
  - Expanded `WeaponDefinition` so runtime weapon rhythm and feedback values can live in assets.
  - Created/updated:
    - `LETHE/Assets/_dev/Data/Weapons/Weapon_DualBlades.asset`
    - `LETHE/Assets/_dev/Data/Weapons/Weapon_Greatsword.asset`
    - `LETHE/Assets/_dev/Data/Weapons/VFX_Weapon_DualBlades.asset`
    - `LETHE/Assets/_dev/Data/Weapons/VFX_Weapon_Greatsword.asset`
  - `V1GameManager` now reads the weapon SO references from `Dev_Prototype_v1`.
  - Data-driven runtime now covers basic slash VFX, Kalmuri follow-up slash VFX, greatsword Blood Blade Storm slash VFX, hit spark, enemy flash, and damage-number settings.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors.
  - Unity compile errors: `count=0`.
  - Play Mode targeted smoke: `dualSO=True dualVfx=True dualEntries=4 dualSparkProfile=DualBladeHitSpark greatSO=True greatVfx=True greatEntries=5 greatSparkProfile=GreatswordHitSpark dualA=3 dualB=1 great=1 dualSpark=3 greatSpark=1 dmg=4`.
  - Evidence capture: `LETHE/Assets/_dev/Evidence/v1_weapon_vfx_profile_data_20260616.png`.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 5 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `404 Not Found`, `project not found`.
- Hungry Blades / Kalmuri readability follow-up:
  - Confirmed the current asset inventory: all 8 memory/echo data assets exist, but dedicated sprite VFX does not exist for every memory family yet.
  - Dedicated PNG VFX is currently concentrated on Kalmuri, Blood, and Blood Blade Storm; the other memory/echo families use procedural runtime shapes.
  - Reworked active Hungry Blades from a faint short orbit into a denser 6-14 blade two-ring swarm.
  - Hungry Blades damage ticks now spawn target-local bite blades so the damage reads as blades chewing through enemies.
  - Kalmuri echo follow-ups now add an explicit blade barrage on top of the existing range ring and weapon slash.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 legacy warnings, 0 errors.
  - Unity compile errors: `count=0`.
  - Short Play Mode entry reached `isPlaying=true`; Unity console error log count `0`.
  - Human visual review is still needed because the weapon-select overlay prevents a full no-input Kalmuri visual smoke.
- Stage/balance shell and object-pool pass:
  - Confirmed the current runtime has first-pass behavior for all 8 active memories, all 8 echoes, and all 4 ultimate branches.
  - Normal runs now use the documented 600s duration, 180/340/490/600s Gatekeeper schedule, first boss HP 2050, and 54s deficit survival.
  - Spawn pressure now follows the documented phase table for lull/rising/gate breath/climax and deficit breath/trial, including caps and pack sizes.
  - Review-only automatic memory/+5 injection now runs only during `fastDebugRun`; normal play must reach growth through XP/cards.
  - Level-up choices now include the six documented run stats: attack speed, damage, area, survival, magnet, echo amp.
  - Transient procedural VFX, floating text, damage numbers, and XP orbs are pooled/reused.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 0 warnings, 0 errors.
  - Unity `Assets/Refresh`: success; compile error count `0`; short Play Mode entry console error count `0`.
  - Remaining technical debt: enemies, player/enemy projectiles, and enemy shots still use create/destroy and should be pooled in a later optimization pass.
- 120-second early fun-loop start pass:
  - Added `J. 120초 초반 재미 루프` to `docs/TASK.md` as the active implementation checklist for the next player-facing gate.
  - Start overlay now presents four build cards instead of weapon-only selection:
    - `절단쌍검 + 굶주린 칼무리`.
    - `절단쌍검 + 피의 반사`.
    - `장송대검 + 굶주린 칼무리`.
    - `장송대검 + 피의 반사`.
  - `BeginRun` now accepts a starting memory while preserving the old weapon-only debug path as Hungry Blades default.
  - Level-up choices prioritize missing core memories so either Kalmuri or Blood starts can quickly fill the Kalmuri/Blood loop.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings, 0 errors.
  - Unity compile error count: `0`.
  - Play Mode entry reached `isPlaying=true`; console error log count `0`.
  - Limitation: camera-based Game View screenshots do not capture OnGUI start cards, so direct play inspection is still required.
- Five-pass 120-second loop follow-up:
  - `695771f feat: 초반 보상 속도 보정`: first-120-second non-boss kills grant +1 XP.
  - `fbf0f0f feat: 무기 타격 확인 피드백 추가`: weapon hits spawn confirm ring/core pulses.
  - `111cdab feat: 망각 결과 UX 강화`: forgetting result now explains loss, echo, overcharge/awakening, deficit survival, and resonance next action.
  - `b19a1b6 feat: 초반 스폰 압박 보정`: first-cycle first-120-second spawn profile and closer spawn radius.
  - `081e13b feat: 적 역할 실루엣 보정`: Drifting Eye, Split One, Void Priest, and Gatekeeper use distinct procedural silhouettes.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed after each pass with 7 legacy warnings and 0 errors.
  - Final Unity MCP check: compile error count `0`, Play Mode entered, console error count `0`, Play Mode stopped.

- Direct Codex smoke-test follow-up:
  - Added `LETHE/V1 Smoke/*` editor menu tests for the four start builds and the M2 loop.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings, 0 errors.
  - Unity compile error count: `0`.
  - Four start-build smoke snapshots initialized the expected weapon and starting memory.
  - M2 smoke reached Hungry/Blood echoes at +5 and `storm=True`.
  - Unity console error count: `0`; scene/assets missing references: `0`.
- Start-selection UX correction:
  - Start overlay now presents only `절단쌍검` and `장송대검`.
  - Starting memories are no longer attached to weapon selection.
  - First forced level-up after weapon start produced `굶주린 칼무리 | 피의 반사 | 칼날 가속`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed with 7 legacy warnings, 0 errors.
  - Unity compile error count `0`; Unity console error count `0`.

- Visual/UI/game-feel refresh:
  - Player body scale pulse was removed, and the sprite now lives under a stable `PlayerVisual` child.
  - Added/imported `sheet_player_v1_4dir.png` as the new player body sheet.
  - The new 8x4 player sheet now drives idle/walk 4-direction animation.
  - Weapon anchor was centered to reduce perceived body drift while moving.
  - Added/imported `spr_weapon_greatsword_01.png` as the dedicated greatsword runtime sprite.
  - Arena floor tiles now have rotation/color/scale variation.
  - HUD was compacted around HP, XP, memory slots, ultimate state, and smaller debug controls.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed with 7 legacy warnings, 0 errors.
  - Unity `Assets/Refresh` succeeded.
  - Unity compile error count `0`; Unity console error count `0`.
  - Short Unity Play Mode entry after the new player sheet succeeded with console error count `0`.
  - Unity missing references: scene `0`, assets `0`.
  - Play Mode Greatsword smoke snapshot: `scene=v1 weapon=장송대검 elapsed=1.8 hp=210.0/210.0 enemies=6 death=False`.
  - Screenshot capture produced a solid-color image and was discarded, so direct visual review is still required.

## Next Implementation

1. Ask jaewoo to review `Dev_Prototype_v1` as a 120-second early fun loop first, not as a full 600-second balance run.
2. Collect one combined feedback pass for:
   - 시작 무기 카드 2개가 명확하게 읽히는지.
   - 첫 보상에서 칼무리/혈반 기억 선택이 자연스럽게 보이는지.
   - 첫 20~30초 안에 첫 레벨업/보상 리듬이 오는지.
   - 60~90초 안에 활성 기억 2~3개와 다음 망각 후보가 보이는지.
   - 카드 선택 중 적/탄이 완전히 멈추는지.
   - 쌍검 기본공격.
   - 쌍검 반달 2연 베기가 슥슥 하는 느낌인지.
   - 적 피격 넉백/피격감.
   - 칼무리 후속타.
   - 대검 큰 반달이 주변 범위 피해로 읽히는지.
   - 적 피격 시 흰색 플래시와 데미지 숫자가 충분한지.
   - 원거리몹이 사거리 안에서 후퇴하지 않고 정지 사격하는지.
   - 대검/칼무리 hitstop 중 캐릭터 이동감이 끊기지 않는지.
   - 대검 시작 선택과 `F9` 비교.
   - 망각 예고/공명/+5 잔향/피의 칼폭풍 목표가 너무 늦거나 복잡하지 않은지.
   - HUD readability and combat density.
3. After feedback, pick exactly one next pass: reward cadence, attack readability, forgetting UX, spawn pressure, or art replacement.

## Open Questions

- Does no-air-swing make the twin blades feel more intentional, or does it make idle combat feel too quiet?
- Does target-local slash make twin blades feel cleaner than player fan arcs?
- Does Kalmuri `MultiSmall` feel like a follow-up hit from the enemy, or still like the character is stopping?
- What should be the shared rule for weapon-specific echo synergy between dual blades and greatsword?
- Does the compressed M2 loop prove the direction, or does the real pacing need to be built before judgment?
- Should generated sheets be sliced/imported properly next, instead of runtime-cropped in the manager?

## Do Not Touch

Do not continue polishing `Dev_EchoSlice` or `Dev_Prototype_v0` as the main path. Do not add shop, meta progression, multi-region structure, or final boss.
