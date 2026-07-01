# Current Task

# 2026-07-01 Kalmuri Orbit Ring Cleanup Result

## Status

Removed the large surrounding ring from Hungry Blades and shifted the readability budget into a larger, denser orbiting blade circle.

## Applied VFX Changes

- Removed `KalmuriSwarmBreathRing`.
- Increased inner/outer orbit radius multipliers to `0.62` / `1.22`.
- Increased active orbit blade count to `10..22`.

## Verification

- `rg` found no `KalmuriSwarmBreathRing` references.
- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- Unity compile error count: `0`.
- Unity `LETHE/V1 Smoke/M2 Loop`: `[V1QA] PASS`.

## Remaining Gate

Jaewoo should confirm that the larger blade orbit now reads as Kalmuri motion without the unwanted large ring.

# 2026-07-01 Kalmuri Living Swarm Motion Result

## Status

Jaewoo rejected both C and D candidate-image directions, so Hungry Blades / Kalmuri has been rebuilt around original blade sprites and dynamic motion instead of static candidate emblems.

## Applied VFX Changes

- Removed C/D candidate runtime references from `V1GameManager.cs`.
- Active Hungry Blades now uses irregular orbit blades with varied speed, radius, alpha, and arc length.
- Nearby enemies pull blades into short hunting lunges from the player orbit toward the target.
- Higher levels add recoil/return shards after lunges.
- Active bite hits converge multiple blades into the enemy, add crossing wound cuts, and throw return shards from +3 onward.
- Kalmuri echo follow-ups now use blade surge/fan strokes instead of a large candidate image.
- Hungry Blades memory-gain feedback now spirals blade strokes outward.

## Verification

- `rg` found no C/D candidate runtime references in `V1GameManager.cs`.
- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- Unity `LETHE/V1 Smoke/M2 Loop`: `[V1QA] PASS`, Hungry/Blood echoes at +5, storm ready, live enemies `10`.

## Remaining Gate

Jaewoo should play `Dev_Prototype_v1` and judge the new motion at +1/+3/+5. If it still misses, tune lunge frequency, trail alpha, or blade count one at a time.

# 2026-07-01 Kalmuri D-Only Runtime Result

## Status

Jaewoo feedback said D was not noticeable enough, so Hungry Blades / Kalmuri runtime VFX has been changed from the C/D mix to D-only.

## Applied VFX Changes

- Removed C / Crescent Pack from active Hungry Blades runtime usage.
- D / Predator Bite now appears around the player as the main active Hungry Blades orbit silhouette.
- D / Predator Bite remains on enemy-side Hungry Blades bite hits, now larger/brighter.
- D / Predator Bite remains on Kalmuri echo follow-up impacts, now larger/brighter with an extra side bite from +3 onward.
- Hungry Blades memory-gain feedback now uses three D bite frames instead of the C aura.
- Hungry Blades echo transform sprite now uses D.
- Small orbit blades are kept as support, but their count/alpha is reduced so D is the main read.

## Verification

- `rg` found no remaining C runtime references in `V1GameManager.cs`.
- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- Unity `LETHE/V1 Smoke/M2 Loop`: `[V1QA] PASS`, Hungry/Blood echoes at +5, storm ready, live enemies `10`.

## Remaining Gate

Jaewoo should play `Dev_Prototype_v1` and judge D-only at +1/+3/+5. If D is still weak, the next narrow lever should be D size/alpha/frequency only, not a new candidate mix.

# 2026-07-01 Kalmuri C/D Runtime Result

## Status

The previously created Kalmuri C/D candidate sprites are now connected to actual `Dev_Prototype_v1` runtime VFX.

## Applied VFX Changes

- C / Crescent Pack is used for the active Hungry Blades player aura.
- +3 and higher adds a counter-rotating C aura layer.
- D / Predator Bite is used for enemy-side active Hungry Blades bite hits.
- D / Predator Bite is also used for Kalmuri echo follow-up impacts.
- Hungry Blades memory-gain feedback now includes the C candidate sprite.
- Existing procedural/orbiting small blades remain as support detail instead of being the main read.

## Dual-Blade VFX Check

- No dual-blade VFX or attack-motion code was changed in this 2026-07-01 unit.
- Prior causes for the smaller feel:
  - 2026-06-22: held weapon renderers were disabled during normal play; dual blades became short hit-point phantom strikes.
  - 2026-06-25: dual-blade slash profile was slightly reduced:
    - `DualBladeCrescent_A` scale `0.94 -> 0.86`, lifetime `0.23 -> 0.20`.
    - `DualBladeCrescent_B` scale `0.88 -> 0.82`, lifetime `0.25 -> 0.21`.
    - minimum slash lifetime `0.34 -> 0.28`.
    - attack range `2.8 -> 2.72`, max targets `7 -> 6`.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
- Unity compile error count: `0`.
- Unity `LETHE/V1 Smoke/M2 Loop`: `[V1QA] PASS`, Hungry/Blood echoes at +5, storm ready, live enemies `10`.

## Remaining Gate

Jaewoo should play `Dev_Prototype_v1` and judge whether C as the aura plus D as the bite frame feels like a real hungry blade swarm without hiding enemies or making +5 too noisy.

## 2026-06-30 Kalmuri C/D Candidate Asset Result

## Status

The C and D Kalmuri visual candidates are now available as Unity-viewable sprite assets and prefabs.

## Created Assets

- `LETHE/Assets/_dev/Art/Sprites/Echoes/Kalmuri/Candidates/spr_kalmuri_candidate_c_crescent_pack_01.png`
- `LETHE/Assets/_dev/Art/Sprites/Echoes/Kalmuri/Candidates/spr_kalmuri_candidate_d_predator_bite_01.png`
- `LETHE/Assets/_dev/Prefabs/Echoes/Kalmuri/Candidates/VFX_Kalmuri_Candidate_C_CrescentPack.prefab`
- `LETHE/Assets/_dev/Prefabs/Echoes/Kalmuri/Candidates/VFX_Kalmuri_Candidate_D_PredatorBite.prefab`
- `LETHE/Assets/_dev/Prefabs/Echoes/Kalmuri/Candidates/Preview_Kalmuri_C_D_SpriteCandidates.prefab`

## Intended Read

- C / Crescent Pack: always-on surrounding Kalmuri aura.
- D / Predator Bite: enemy-hit lunge/bite frame.

## Verification

- Both PNGs import as Unity `Sprite`, `Single`, PPU `256`, alpha transparency enabled.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- Visual evidence: `LETHE/Assets/_dev/Evidence/v1_kalmuri_cd_sprite_assets_20260630.png`.

## Remaining Gate

Jaewoo should open the preview prefab or sprites in Unity and choose whether runtime Hungry Blades should use C as the idle aura, D as the hit frame, or another mix.

## 2026-06-30 Hungry Blades Visual Result

## Status

The Unity `Dev_Prototype_v1` Hungry Blades / Kalmuri active-memory VFX has been strengthened after jaewoo feedback that it no longer felt like a proper blade swarm.

## Applied VFX Changes

- Active Hungry Blades orbit blades now sweep around the player instead of blinking as short static markers.
- Outer blade ring radius, blade scale, alpha, and lifetime were increased.
- Bright lead blades and a stronger cyan outer trace ring were added.
- Enemy-side bite VFX now uses more blades, a larger halo, and target-local cut trace.
- Hungry Blades memory-gain VFX now uses a ring plus 12 orbiting blades.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
- Unity MCP `Assets/Refresh`: success.
- Unity compile error count: `0`.
- Unity console error count during forced Hungry Blades +5 Play Mode check: `0`.
- Forced runtime snapshot: `weapon=절단쌍검`, `memories=[HungryBlades:5]`, `enemies=18`.
- Visual evidence:
  - `LETHE/Assets/_dev/Evidence/v1_hungry_blades_kalmuri_refresh_20260630.png`
  - `LETHE/Assets/_dev/Evidence/v1_hungry_blades_kalmuri_refresh_camera_20260630.png`

## Remaining Gate

Jaewoo should directly review whether +1/+3/+5 Hungry Blades progression is readable without becoming late-run clutter.

## 2026-06-30 Intro UI Result

## Status

The Unity `Dev_Prototype_v1` intro / starting weapon screen has been refreshed so it reads more like a playable game shell.

## Applied UI Changes

- Added LETHE title treatment, dark intro backdrop, and top/bottom accent lines.
- Added a first-goal strip: XP -> memory slots -> first Gatekeeper -> echo transformation.
- Upgraded the two starting weapon cards with number-key badges, simple glyphs, rhythm summaries, and clearer click/number-key selection text.
- Kept the current weapon-only start flow. Starting memories still come from the first reward card.
- Added compact layout behavior for the current short Game View height.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
- Unity MCP `Assets/Refresh`: success.
- Unity compile error count: `0`.
- Unity console error count during Play Mode intro capture: `0`.
- Visual evidence: `LETHE/Assets/_dev/Evidence/v1_intro_weapon_select_ui_20260630_v2.png`.

## Remaining Gate

Jaewoo should confirm whether the first-goal strip helps or feels too explanatory, and whether the weapon-only start model is clear enough before deciding whether to add starting memory preview to the intro.

## 2026-06-30 Active Task Result

## Status

Jaewoo direct-play feedback follow-up has been applied to Unity `Dev_Prototype_v1`.

## Applied Runtime Values

- Gatekeeper schedule: `150 / 300 / 540 / 900s`.
- Gatekeeper HP: `2200 / 4200 / 7600 / 12800`.
- Hard cap: `1080s`.
- Post-forget memory reacquire/refill: removed.
- Gatekeeper pulse/guard pattern: added.
- Enemy/boss HP bar inverse scale: added.
- Hungry Blades orbit visual continuity during hitstop: added.

## Verification

- `node scripts/balance_curve_v1.js`: passed.
- `node scripts/verify_unity_stepped_balance.js`: passed.
- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors after retry.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- Unity MCP compile error count: `0`.
- Unity MCP console error count: `0`.

## Remaining Gate

MCP menu/play automation did not capture a full `[V1QA] PASS` log because the bridge restarted or returned a response parse error. Jaewoo should replay the updated run and judge:

- first boss at 2:30 after HP increase.
- second boss at 5:00.
- Gatekeeper pulse/guard readability and damage.
- HP bar stability.
- no memory reacquire after forgetting.
- Hungry Blades visual continuity during base attacks.

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

- 20-minute beta balance pass, 2026-06-27:
  - Normal run target is now `18~22m`, with hard cap `1260s`.
  - Gatekeeper schedule is now `300 / 600 / 900 / 1140s`.
  - Gatekeeper HP is now `1900 / 2800 / 4000 / 5400`.
  - Initial required XP is now `7`.
  - XP tempo is now `0~120s x1.00`, `120~600s x1.34`, `600s+ x1.00`; first-120 kill XP bonus removed.
  - Timer-only victory removed; clear requires all 4 Gatekeepers.
  - Reward focus now supports all 4 ultimate echo pairs, not only Blood Blade Storm.
  - `node scripts\balance_sim_v1.js`: selected `20m_slow_start`; first reward `24~28s`, first forget `323~329s`, ultimate `936~945s`, clear `1178~1188s`.
  - Evidence: `docs/orchestration/evidence/2026-06-27-balance-sim-v1.md`.
- Reliable MCP QA line, 2026-06-27:
  - `V1SmokeTestMenu` now logs explicit `[V1QA] PASS/FAIL` results based on runtime conditions.
  - Start-weapon QA requires elapsed runtime, live enemies, normal time scale, and no overlay state.
  - M2 QA requires Hungry/Blood echoes at +5, Blood Blade Storm readiness, result overlay, and live enemies.
  - VFX Matrix QA verifies all 8 memory previews, all 8 echo previews, and 3 non-blood ultimate previews.
  - Blood Blade Storm QA ticks `UpdateEchoUltimate` and verifies actual `BloodBladeStorm*` objects.
  - Unity MCP results:
    - Dual blades: PASS, `elapsed=2.0`, `liveEnemies=8`.
    - Greatsword: PASS, `elapsed=2.0`, `liveEnemies=8`.
    - M2 loop: PASS, `HungryBlades:5`, `BloodReflection:5`, `storm=True`.
    - VFX Matrix: PASS, `previewMemory=8`, `previewEcho=8`, `fracture=1`, `stasis=1`, `ashen=1`.
    - Blood Blade Storm: PASS, `stormObjects=77`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors.
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors.
  - Unity compile error count: `0`; scene missing references: `0`; console error count: `0`.
- MCP automated play/QA pass, 2026-06-27:
  - AnkleBreaker Unity MCP targeted `LETHE` on port `7890`.
  - Unity compile error count: `0`; scene missing references: `0`.
  - Smoke menu results:
    - `Start Dual Blades`: initialized `weapon=절단쌍검` with no console errors.
    - `Start Greatsword`: initialized `weapon=장송대검`; 2.2-second result showed `enemies=10`.
    - `M2 Loop`: injected Hungry/Blood echoes at +5 and `storm=True`.
  - Direct Play Mode reflection result: dual-blades run advanced to `elapsed=6.2`, `level=2`, `kills=2`, `enemies=26`, `timeScale=1`.
  - VFX probe: all 8 memory ids and all 8 echo ids spawned preview objects.
  - Utility probe: 6 utility memory previews, 6 utility echo previews, and 3 non-blood ultimate previews spawned.
  - Ultimate readiness probe: all 8 echoes at +5, Blood Blade Storm readiness `storm=True`, console errors `0`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors.
  - Limit: this confirms technical playability and VFX wiring, not human readability or hand-feel.
- Beta-play preparation pass:
  - Added `V1ContentCatalog` for prototype v1 runtime asset references.
  - Created and wired `Assets/_dev/Data/V1_ContentCatalog.asset`.
  - Created `Assets/Lethe/` promotion-prep folders and `Assets/Lethe/Scenes/Lethe_BetaPreview.unity`.
  - HUD now surfaces current echoes and the current player objective.
  - F12 debug and memory/echo review UI remain available for the next VFX iteration.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity scene missing references: `0`.
  - Unity Play Mode start snapshot: `scene=v1`, weapon `절단쌍검`, HP `210/210`, no death/result/refill state.
  - Unity console error count after stop: `0`.
- Prototype completion loop pass:
  - Death, 600-second survival, and full Gatekeeper clear now route into a shared result overlay.
  - Fourth Gatekeeper kill now completes the prototype run.
  - Result summary records survival time, kills, Gatekeeper progress, weapon, level, choices, forgotten memories, and echoes.
  - Added boss-clear VFX, minimal procedural SFX, Gatekeeper HUD progress, and `F12` debug panel toggle.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`; Unity console error count: `0`.
- Release-feel second integration pass:
  - Added connected Lethe river/bank terrain bands and sunken ruin slabs over the enlarged arena.
  - Added enemy/boss HP bars and fixed blood-mark color restore to preserve enemy base colors.
  - Buffed under-readable memories/echoes and utility ultimates through scale, lifetime, proc chance, radius, damage, freeze, heal, and damage reduction values.
  - Added level-up and ultimate-ready burst feedback.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`; Unity console error count: `0`.
- Prototype gap pass:
  - First Gatekeeper moved from `180s` to `150s`; later schedule is `300 / 450 / 600`.
  - First Gatekeeper HP reduced from `2050` to `1750`.
  - Added 18-second Gatekeeper warning VFX.
  - Added five subtle arena memory landmarks.
  - Added role markers for Drifting Eye, Split One, Void Priest, and Gatekeeper.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors after Unity refresh.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`; Unity console showed no gameplay errors/warnings.
- Combat VFX / attack coverage scale-up:
  - Added shared combat VFX visibility multiplier `1.18x`.
  - Increased dual blades range/arc/max targets/echo size scale to `2.8` / `132` / `7` / `1.05`.
  - Increased greatsword range/arc/max targets/echo size scale to `3.75` / `96` / `6` / `2.15`.
  - Enlarged weapon VFX profile scales and key echo field radii.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors after Unity refresh.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`; Unity console showed no gameplay errors/warnings.
- 8-echo VFX readability follow-up:
  - All 8 echoes already had runtime VFX hooks, but jaewoo feedback was that several were hard to notice in live combat.
  - Strengthened Kalmuri, Blood, Execution, Hunter, Shatter, Stopped, Ashen, and Oblivion echo VFX through scale, alpha, lifetime, proc chance, and added ring/halo/slash accent sprites.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`; Unity console error count: `0`.
- Terrain continuity follow-up:
  - Regenerated the terrain tiles from one shared wet black stone base.
  - Runtime floor tile selection now favors connected base tiles and keeps high-character variants rare.
  - Reduced water seam, drowned root, and memory gravel dressing density.
  - Increased floor tile overlap slightly to soften grid boundaries.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity `Assets/Refresh`: success.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`; Unity console error count: `0`.
- Lethe terrain background follow-up:
  - Replaced artificial arena-field direction with Vampire Survivors-style natural terrain readability.
  - Added source sheet `LETHE/Assets/_dev/Art/Source/spr_lethe_terrain_sheet_01_source.png`.
  - Generated eight terrain tiles and `spr_lethe_terrain_backdrop_01.png`.
  - Updated `V1GameManager` to load `tile_lethe_terrain_01..08.png`.
  - Replaced outer marker rings with marsh edges, Lethe water seams, drowned roots, and memory gravel runtime dressing.
  - Local visual sanity check opened representative generated terrain PNGs.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity `Assets/Refresh`: success.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`; Unity console error count: `0`.
- Release-prep map / background pass:
  - Generated four new Lethe stone floor tiles and one large arena backdrop.
  - Added `scripts/generate_world_sprites.ps1` for reproducible map sprite generation.
  - Expanded player clamp from `x +/-12`, `y -8.5..8.5` to `x +/-24`, `y +/-16`.
  - Expanded floor coverage from `11x9` to `21x15` tile placements.
  - Increased camera orthographic size from `6.1` to `6.8` and clamped camera follow inside the enlarged arena.
  - Increased enemy spawn radius for the larger space.
  - Local visual sanity check opened representative generated map PNGs.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity `Assets/Refresh`: success.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`; Unity console error count: `0`.
- Enemy / boss sprite insertion:
  - Generated first-pass sprites for Drifting Eye, Split One, Void Priest, and Gatekeeper.
  - Added matching chroma source PNGs and `scripts/generate_enemy_boss_sprites.ps1`.
  - `V1GameManager.EnemySprite()` now loads these assets before procedural fallback.
  - Local visual sanity check opened the generated PNGs.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity `Assets/Refresh`: success.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`; Unity console error count: `0`.
- Lingering VFX / echo readability follow-up:
  - Shatter Wave active VFX now persists for `1.05s` as a field with hold rings and fracture spokes.
  - Shatter Echo now persists for `0.90s` with the same field language.
  - Stopped Second active clock field now lasts `1.75s`.
  - Stopped Echo now uses a larger field for `1.25s`.
  - Execution, Hunter, Ashen, and Oblivion echo VFX were lengthened/brightened/slightly enlarged.
  - First boss timing remains `180s` until direct play confirms whether first-three-minute pacing still feels loose.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`; Unity console error count: `0`.
- Hunter Oath value follow-up:
  - Active Hunter Oath now fires 2/3/4 projectiles at levels 1/3/5.
  - Active cooldown, projectile speed, projectile damage, target-lock VFX, and hit burst were strengthened.
  - Hunter Echo proc chance and damage were increased, and +5 can fire two echo shots.
  - Homing projectiles retarget if their original target dies mid-flight.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`; Unity console error count: `0`.
- Stopped Second gold clock-field follow-up:
  - Hunter Oath remains yellow-green/green, while Stopped Second is now the yellow/gold time-stop VFX language.
  - Active Stopped Second now freezes enemies for up to `1.0s` and keeps the clock-field VFX visible for `1.50s`.
  - The field now has a larger/brighter clock face, stronger rings, larger ticks, thicker hands, brighter core, and a rotating pulse ring.
  - Stopped Echo and Stasis Hunt reuse the gold field language with shorter support lifetimes.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity Play Mode smoke: `clockFaces=2`, `totalClockFaces=3`, `clockTicks=24`, `totalClockTicks=36`, `clockPulses=2`, `clockHands=6`, `goldFaces=2`, `frozenNear1s=5`.
  - Unity console error count: `0`.
- Execution Flash / Stopped Second readability follow-up:
  - Execution Flash active VFX target width increased to `1.95`, lifetime to `0.38s`, and now adds cross/diagonal crack lines.
  - Execution Echo target width increased to `1.48` and uses the same crack helper.
  - Stopped Second now draws a clock-field floor telegraph with face, rings, 12 ticks, hands, and core.
  - Stopped Echo and Stasis Hunt paths reuse the clock-field language.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity Play Mode smoke: `executionCracks=16`, `executionVfx=24`, `clockFaces=5`, `clockTicks=60`, `clockHands=15`, `stoppedVfx=79`.
  - Unity console error count: `0`.
- Utility VFX / background / movement follow-up:
  - Greatsword slash delay reduced to `0.18s`, so VFX appears at roughly `64.3%` of the `0.28s` sweep.
  - Six utility memories/echoes now have stronger runtime visibility.
  - `StoppedSecond` now creates an enemy-cluster time-stop focus with clock hands.
  - Debug panel now exposes `Mem A`, `Mem B`, `Echo A`, `Echo B`, `Ult 3`, and `VFX`.
  - Runtime arena dressing and softer player walking were added.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity Play Mode smoke: `greatDelay=0.18`, `sweep=0.28`, `activeMemories=3`, `bgDecor=30`, `utilityVfx=36`, `enemies=14`.
  - Unity Play Mode echo/ultimate smoke: `echoCount=6`, `previewUlt=6`, `clockHands=21`.
  - Unity console error count: `0`.
- Dual blades / Blood Blade Storm / first-120 tempo pass:
  - Dual-blade VFX stagger: A `0.045s`, cut flash `0.067s`, B `0.085s`, assist `0.045s`.
  - Blood Blade Storm opening, pressure, burst cadence, burst damage, heal, hitstop, and camera shake increased.
  - First-120 opening cadence and XP tempo increased.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: `0`.
  - Runtime check: opening spawn interval `0.46`, pack `2`; early `GrantXp(1)` produced `2/5` XP.
  - Blood Blade Storm smoke: `stormReady=True`, `stormObjects=187`, `burstObjects=45`, `bladeObjects=187`, `kills=21`.
  - Unity console error count: `0`.
- Greatsword slash timing tighten:
  - Greatsword slash delay reduced from `0.22s` to `0.20s`, so VFX appears at roughly `71.4%` of the `0.28s` weapon sweep.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: `0`.
  - Runtime value check: delay `0.20s`, sweep `0.28s`, slash appears at `71.4%`.
  - Unity console error count: `0`.
- Greatsword timing / coverage review loop:
  - Greatsword slash delay increased to `0.22s`, so VFX appears at roughly `78.6%` of the `0.28s` weapon sweep.
  - Greatsword minimum slash lifetime increased to `0.62s`.
  - Greatsword AoE / Primary / Assist positions now sample different points along the 90-degree tip arc (`58%`, `78%`, `72%`).
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity inline Game View capture succeeded on a frozen review frame with sword around `85%` of the swing and VFX visible.
  - Runtime value check: delay `0.22s`, sweep `0.28s`, min slash lifetime `0.62s`, AoE scale/lifetime `1.65 / 0.62`, Primary scale/lifetime `1.38 / 0.52`.
  - Unity console error count: `0`.
- Greatsword spectacle pass:
  - Greatsword handle-pivot sweep increased to `-45.0 -> 45.0`, a full `90` degree cut.
  - Greatsword weapon-hit VFX profile scales/lifetimes increased for a flashier hit.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: `0`.
  - Play Mode forced greatsword attack: `usePivot=True`, blade sweep `-45.0 -> 45.0`, total `90.0`, AoE scale `1.65`, primary scale `1.38`.
  - Direct slash VFX check: end blade `45.0`, VFX rotation `225.0`, generated bounds `(4.28, 4.28)`, tip alignment error `0.000`.
  - Unity console error count: `0`.
- Greatsword handle-pivot / crescent direction pass:
  - Greatsword phantom weapon now rotates around a handle pivot instead of sliding between start/end positions.
  - Greatsword crescent VFX now uses the sweep end blade direction with a `180` degree facing correction.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: `0`.
  - Play Mode forced greatsword attack: `usePivot=True`, handle distance from player `0.13`, strike center distance `0.61`, blade sweep `-28.0 -> 28.0`.
  - Direct slash VFX check: end blade `28.0`, VFX rotation `208.0`, tip alignment error `0.000`.
  - Unity console error count: `0`.
- Greatsword blade-tip alignment pass:
  - Greatsword phantom weapon now uses blade-tip-first placement so the handle faces back toward the player body.
  - Greatsword slash VFX now anchors to the calculated blade tip by compensating for the slash profile offset.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: `0`.
  - Forced greatsword attack Play Mode check: `handleCloser=True`, tip distance from player `1.67`, handle distance from player `0.16`.
  - Slash alignment check: desired tip and `GreatswordCrescent_Primary` position matched with distance `0.000`.
  - Unity console error count: `0`.
- Remaining VFX prompt-sheet generation:
  - Generated/imported 20 VFX sprites for weapon hits, six active memories, six echoes, and three ultimates.
  - Evidence contact sheet: `LETHE/Assets/_dev/Evidence/remaining_vfx_prompt_sheet_20260621.png`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 0 warnings, 0 errors.
  - `unity_get_compilation_errors(port=7890, severity="all")`: `count=0`.
  - Unity AssetDatabase found generated VFX textures: `20/20`.
  - Unity Sprite import setting confirmed: `20/20`.
- Generated VFX runtime wiring and scale pass:
  - Connected generated weapon/hit VFX, six active memory VFX, six echo VFX, and three utility ultimate VFX to `V1GameManager` spawn paths.
  - `WeaponVfxProfile` slash entries now resolve to generated dual-blade, greatsword, Kalmuri, and hit-spark PNGs before falling back to procedural sprites.
  - Added world-size normalization for 1254px generated sprites so they fit existing combat scale instead of covering the play field.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity `Assets/Refresh`: success.
  - Unity compile error count: `0`.
  - Unity Play Mode smoke attempts produced console error count `0`.
  - Limitation: Unity Game/Scene capture still returned solid-color images, so final VFX size/timing needs jaewoo direct visual review.
- Blood Blade Storm payoff / movement pass:
  - Blood Blade Storm now uses an opening cue, continuous storm pressure, and periodic burst pulses so it should no longer read as only Kalmuri with different coloring.
  - Dual-blade storm uses fast orbit/burst cadence; greatsword storm uses slower heavy rings/slashes, stronger damage, hitstop, camera shake, knockback, healing, and blood-heal threads.
  - Player movement now smooths raw input through short acceleration/deceleration, smooths movement-facing weapon rotation, and gives `PlayerVisual` subtle bob/tilt.
  - `BeginRun` now defensively creates the player if a debug/smoke call reaches it before the cached player reference exists.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity console error count after direct M2/ultimate reflection smoke: `0`.
  - Manual reflection ticks through `UpdateEchoUltimate(0.12f)` created `bloodStormObjects=124`, cleared nearby spawned enemies, and reached `kills=14`.
  - Limitation: MCP Play Mode time did not advance normally, so this is a runtime method smoke rather than full natural-timing visual proof.
- Held weapon silhouette / attack VFX scale tune:
  - Dual blade hand sprites were increased to runtime scale `0.43~0.475` and pulled closer to the player body.
  - Greatsword hand sprite was reduced to runtime scale `0.34~0.375`, with less swing travel and tighter player-relative position.
  - Generated dual-blade slash PNG scale factor increased from `0.153` to `0.192`.
  - Generated greatsword cleave PNG scale factor reduced from `0.225` to `0.182`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: `0`.
  - Play Mode transform check confirmed dual blade scale `0.430`, greatsword scale `0.340`, console error count `0`.
- Direct greatsword cover fix:
  - Direct Play Mode check showed the greatsword was still too dominant: sword bounds `(3.121, 4.995)`, player bounds `(2.210, 2.210)`, ratioY `2.26`, and sword sorting order `30` in front of player sorting order `20`.
  - Greatsword held sprite was reduced to runtime scale `0.21~0.235`, shifted to the side, and moved behind the player at sorting order `18`.
  - Greatsword cleave PNG scale factor was reduced from `0.182` to `0.150`.
  - Post-fix Play Mode check showed sword bounds `(2.327, 2.944)`, ratioY `1.33`, and forced attack VFX max bounds `(2.332, 2.332)`.
  - Unity console error count: `0`.
- Hit-point phantom weapon pass:
  - Player-attached held weapon renderers now stay disabled during normal play.
  - Dual blades appear as two short hit-point phantom blade sprites aligned with the target-local slash VFX.
  - Greatsword appears as a heavy hit-point phantom strike plus a faint afterimage at the cleave center.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity `Assets/Refresh`: success.
  - Unity compile error count: `0`.
  - Dual-blade Play Mode reflection check: held renderers disabled, phantom count `2`, max bounds `(1.151, 1.151)`.
  - Greatsword Play Mode reflection check: held renderers disabled, phantom count `2`, max bounds `(1.586, 1.689)`.
  - Unity console error count: `0`.
- Phantom weapon timing/readability pass:
  - Phantom weapons now sweep before slash / spark / hit-confirm VFX appears.
  - Dual-blade slash / hit feedback is delayed by `0.055s`; greatsword by `0.075s`.
  - Weapon slash VFX lifetime is extended by `1.45x`, with minimum lifetimes of `0.34s` for dual blades and `0.48s` for greatsword.
  - Greatsword immediate Play Mode check: phantom `2`, active sweep `2`, slash `0`, spark `0`, confirm `0`.
  - Delayed enumerator check: greatsword slash `1`, spark `1`, confirm `2`, expected slash minimum lifetime `0.48s`.
  - Unity console error count: `0`.
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

1. Review `Dev_Prototype_v1` as a 20-minute beta-run candidate, not as the old 600-second prototype loop. Use `docs/orchestration/review_prompts/2026-06-29-jaewoo-beta-run-review.md` as the direct-play checklist.
2. Collect one combined feedback pass for:
   - 시작 무기 카드 2개가 명확하게 읽히는지.
   - 첫 보상이 24~30초 전후에 자연스럽게 오는지.
   - 120초 레벨 3~4가 너무 느리거나 너무 빠르지 않은지.
   - 첫 문지기 300초와 첫 망각 5분대가 적절한 성취/상실 리듬인지.
   - 600초 시점 레벨 9~10 전후와 빌드 다양성이 적절한지.
   - 15~16분 궁극 1종 완성이 후반 보상처럼 느껴지는지.
   - 19~20분대 최종 문지기 처치가 명확한 클리어 목표인지.
   - 카드 선택 중 적/탄이 완전히 멈추는지.
   - 쌍검 기본공격.
   - 쌍검 반달 2연 베기가 슥슥 하는 느낌인지.
   - 적 피격 넉백/피격감.
   - 칼무리 후속타.
   - 대검 큰 반달이 주변 범위 피해로 읽히는지.
   - 적 피격 시 흰색 플래시와 데미지 숫자가 충분한지.
   - 원거리몹이 사거리 안에서 후퇴하지 않고 정지 사격하는지.
   - 대검/칼무리 hitstop 중 캐릭터 이동감이 끊기지 않는지.
   - 대검 루트가 순수 시뮬레이션처럼 실제로도 낮은 클리어 안정성을 보이는지.
   - 피의 칼폭풍, 파쇄 처형, 정지 추적, 잿빛 망각 루트 중 한쪽만 과도하게 쉽거나 막히지 않는지.
   - 망각 예고/공명/+5 잔향/궁극 목표가 너무 늦거나 복잡하지 않은지.
   - HUD readability and combat density.
3. During review, specifically judge the newly wired generated VFX scale/timing for weapon hits, six utility memories, six utility echoes, and three non-blood ultimates.
4. After feedback, pick exactly one next pass: XP cadence, Gatekeeper HP, weapon route balance, reward route steering, VFX scale/timing, or enemy pressure.

## Open Questions

- Does no-air-swing make the twin blades feel more intentional, or does it make idle combat feel too quiet?
- Does target-local slash make twin blades feel cleaner than player fan arcs?
- Does Kalmuri `MultiSmall` feel like a follow-up hit from the enemy, or still like the character is stopping?
- What should be the shared rule for weapon-specific echo synergy between dual blades and greatsword?
- Does the compressed M2 loop prove the direction, or does the real pacing need to be built before judgment?
- Should generated sheets be sliced/imported properly next, instead of runtime-cropped in the manager?

## Do Not Touch

Do not continue polishing `Dev_EchoSlice` or `Dev_Prototype_v0` as the main path. Do not add shop, meta progression, multi-region structure, or final boss.
# 2026-06-29 Active Task Result

## Status

Stepped boss / XP / DPS curve has been applied to Unity `Dev_Prototype_v1`.

## Applied Runtime Values

- Gatekeeper schedule: `150 / 360 / 660 / 1020s`.
- Gatekeeper HP: `1200 / 2250 / 4050 / 8650`.
- Hard cap: `1200s`.
- Initial XP requirement: `8`.
- Normal-run deficit survival: removed as a timed pocket.
- Fast/debug deficit timing: preserved for compressed smoke paths.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors.
- `node scripts/balance_curve_v1.js`: passed.
- `node scripts/verify_unity_stepped_balance.js`: passed.
- Unity MCP compile error count: `0`.
- Unity MCP console error count: `0`.

## Remaining Gate

V1QA smoke menus need a retry because MCP `unity_execute_menu_item` returned `Error polling queue: fetch failed` in this session. After that, jaewoo should replay the first 6 minutes and judge whether the early run is no longer dull.

# 2026-06-29 Active Task Update

## Goal

Apply the jaewoo review finding that the first boss is too late by converting the current flat 20-minute beta pacing into a stepped boss / XP / DPS curve.

## Planned Runtime Candidate

- Gatekeeper schedule: `150 / 360 / 660 / 1020s`.
- Gatekeeper HP: `1200 / 2250 / 4050 / 8650`.
- Target TTK: `18 / 26 / 36 / 48s`.
- Hard cap: `1200s`.
- XP model evidence: `scripts/balance_curve_v1.js`.
- Evidence report: `docs/orchestration/evidence/2026-06-29-stepped-boss-xp-dps-plan.md`.
- Remove the separate normal-run deficit survival timer unless later play review proves that downtime is valuable.

## Done Criteria For This Update

- Unity runtime constants/data follow the stepped schedule.
- The first Gatekeeper appears around 2:30.
- Later Gatekeeper intervals grow instead of staying flat.
- Enemy cap/HP/XP phases follow the calculated table.
- Deficit survival no longer inserts a 54-second post-forgetting pocket in normal play.
- `dotnet build`, Unity QA, report generation, and report check pass.
- jaewoo can replay the first 6 minutes and judge whether the early run is no longer dull.
