# Status

Last updated: 2026-06-11

## Current Snapshot

LETHE HTML Alpha v0.12 implemented the new forgetting model and passed automated regression, but the user feedback is that the play experience still does not feel like a big change. The current task has shifted from raw rule implementation to a Unity-ready echo slice: echoes should not feel like `잔향!` labels on basic attacks, but like weapon-specific combat events supported by concrete images, `_dev` prefabs, debug scene states, and one-person feel tests. The first Unity `_dev` readability resource pass is now imported: player silhouette, walker enemy, dark floor tile, and left/right dual blade sprites are in `LETHE/Assets/_dev/Art/Sprites`, with chroma source images preserved in `LETHE/Assets/_dev/Art/Source`.

The new design sources are `docs/design/LETHE_FORGETTING_FEEL_SPEC.md`, `docs/design/LETHE_WEAPON_MEMORY_ECHO_SPEC.md`, `docs/design/LETHE_WEAPON_MEMORY_ECHO_DETAIL.md`, `docs/design/LETHE_ECHO_FORM_TRANSFORMATION_SPEC.md`, `docs/design/LETHE_UNITY_ECHO_SYSTEM_PRD.md`, `docs/design/LETHE_VISUAL_ASSET_PLAN.md`, `docs/design/LETHE_UNITY_SLICE_ASSET_PRODUCTION_PLAN.md`, and `docs/design/LETHE_UNITY_ASSET_BINDING_PLAN.md`. Together they define the first showcase, active-to-echo form transformation, loop-safe hit event taxonomy, Unity class roles, ScriptableObjects, prefabs, first-slice acceptance criteria, the first sprite/VFX concept sheet, the imagegen production plan, and the file-to-prefab-to-scene binding map for Unity MCP. The current implementation target is now a playable `_dev` debug slice for morning review.

The orchestration HTML interface now exists at `docs/orchestration/interface/index.html`, `docs/orchestration/interface/command.html`, and `docs/orchestration/interface/runbook.html`. AI-facing state lives under `docs/orchestration/state/`; human-facing reports live under `docs/orchestration/reports/YYYYMMDD/`.

The report/devlog/review migration is now applied physically: old `docs/reports/` daily files moved to `docs/orchestration/reports/YYYYMMDD/index.md|html`, old unit reports moved to `docs/orchestration/reports/YYYYMMDD/units/`, old `docs/devlog/` files moved to `docs/orchestration/devlog/YYYYMMDD.md`, and old review prompt/response files moved to `docs/orchestration/review_prompts/` and `docs/orchestration/review_responses/`. New work should not recreate legacy `docs/reports/`, `docs/devlog/`, `docs/review_prompts/`, or `docs/review_responses/` as normal source-of-truth folders.

The current development-docs plugin baseline from `docs/orchestration/MIGRATION_PROMPT.md` has been applied without committing: `AGENTS.md` now uses a `Development Docs Plugin` section, `docs/orchestration/templates/HTML_INTERFACE_TEMPLATE.md` exists, legacy review pointer READMEs are readable, `reports/index.html` is generated as a newest-first date archive, daily report pages are generated as unit-card pages, and Discord delivery is documented as Project Orchestrator first with local direct-send scripts as trusted fallback only.

## Latest Verified Result

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

Run a jaewoo hands-on review of `Assets/_dev/Scenes/Dev_Prototype_v0.unity`.

Focus the review on:

- 4-direction player/enemy animation readability.
- Camera scale and arena framing.
- Whether the auto dual-blade combat loop feels like a game rather than a debug toy.
- Whether memory choice, highest-level forgetting, echo +5, resonance, and Blood Blade Storm are understandable enough for the next tuning pass.

Do not promote `_dev` assets to `Assets/Lethe` until jaewoo explicitly returns `GO`.

Do not promote `_dev` assets to `Assets/Lethe` until jaewoo explicitly returns `GO`.

For reporting/Discord notification, use `npm run report:orchestrator:unit:dry` before real sends, then `npm run report:orchestrator:unit` when the Project Orchestrator is running and the report is ready.

## Current Source Of Truth

- Top-level rules: `AGENTS.md`
- Detailed legacy status archive: `docs/CODEX_STATUS.md`
- Detailed legacy task archive: `docs/NEXT_TASKS.md`
- Current orchestration task: `docs/orchestration/state/CURRENT_TASK.md`

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
