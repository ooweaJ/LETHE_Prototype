# LETHE TEST

# 2026-07-01 Skill SFX Runtime Pass

- Purpose:
  - Add skill-appropriate audio feedback for the current `Dev_Prototype_v1` combat slice.
  - Use Vampire Survivors-like lessons as direction: short readable retro/gameplay sounds, clear pickup/level/cast cues, and throttled projectile impacts so dense auto-combat does not become noisy.
- Applied target:
  - Replaced the simple sine-only prototype clips with a procedural SFX palette in `V1GameManager`.
  - Added `PlaySfx(id, volumeMul, minInterval)` to throttle dense repeated events.
  - Added original generated clips for:
    - basic weapon slashes.
    - Hungry Blades lunge, pierce, and echo.
    - Blood Reflection mark/heal and Blood Blade Storm pulses.
    - Execution Flash, Hunter Oath, Shatter Wave, Stopped Second, Ashen Shield, Oblivion Brand.
    - XP pickup, kill, warning, level-up, clear, defeat, and player hit.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - Unity MCP compile error check.
  - Unity MCP console error check.
  - Unity Play Mode direct call: `V1GameManager.DebugRunM2Smoke()`.
- Results:
  - Runtime build passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity console error count: `0`.
  - Direct M2 smoke snapshot: `HungryBlades:5`, `BloodReflection:5`, `storm=True`, `enemies=10`, `result=True`.
- Limitation:
  - Automated checks confirm runtime safety. Final mix judgment still needs jaewoo direct play with speakers/headphones.

# 2026-07-01 Kalmuri Lunge Range / Stab Feel

- Purpose:
  - Respond to jaewoo feedback that Kalmuri blades seemed to launch only when enemies reached the rotating orbit and did not feel like they were stabbing.
- Applied target:
  - Added a larger `lungeRange` for Hungry Blades target acquisition.
  - Kept the visible rotating orbit as a smaller one-ring visual.
  - Bite blades now launch from the player-side orbit toward the target.
  - Bite blade endpoints now pass slightly through the enemy to read as a stab.
  - Per-blade damage is delayed by a short stagger to match the incoming blade rhythm.
  - Added `KalmuriBladePierceSpark` on delayed impact.
- Results:
  - Runtime build passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity console error count: `0`.
  - M2 Loop QA logged `[V1QA] PASS`.

# 2026-07-01 Kalmuri Outer Orbit Removal / Per-Blade Damage

- Purpose:
  - Respond to jaewoo screenshot feedback: the rotating Kalmuri still had two rings, and the outermost Kalmuri layer should be removed.
  - Make each flying blade own damage instead of applying one damage event per target.
- Applied target:
  - Removed the inner/outer orbit split from active Hungry Blades.
  - Replaced `innerRadius` / `outerRadius` with one `orbitRadius`.
  - Removed `lane != 1` outer-ring branching.
  - Changed `SpawnHungryBladeBite` to receive the target enemy and total damage.
  - Split total Hungry Blades tick damage by the number of flying bite blades.
  - Each spawned `KalmuriBiteDiveBlade` now calls `DealDamage`.
- Results:
  - Runtime build passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity console error count: `0`.
  - M2 Loop QA logged `[V1QA] PASS`.

# 2026-07-01 Kalmuri Orbit Ring Cleanup

- Purpose:
  - Remove the unintended-feeling large ring around Hungry Blades and make the real orbiting blade circle larger/denser.
- Applied target:
  - Removed `KalmuriSwarmBreathRing` from active Hungry Blades visuals.
  - Increased active Kalmuri orbit radius:
    - inner base multiplier `0.54 -> 0.62`
    - outer base multiplier `1.06 -> 1.22`
  - Increased active orbit blade count from `Mathf.Clamp(5 + level * 2, 7, 14)` to `Mathf.Clamp(7 + level * 3, 10, 22)`.
- Commands / checks:
  - `rg -n 'KalmuriSwarmBreathRing' LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs`
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - Unity MCP compile error check.
  - Unity `LETHE/V1 Smoke/M2 Loop`.
- Results:
  - `KalmuriSwarmBreathRing` no longer exists in `V1GameManager.cs`.
  - Runtime build passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: `0`.
  - M2 Loop QA logged `[V1QA] PASS`.

# 2026-07-01 Kalmuri Living Swarm Motion Pass

- Purpose:
  - Respond to jaewoo feedback that both C and D candidate-image directions felt weak.
  - Make Hungry Blades feel like a dynamic swarm of blades rather than a static aura or emblem.
- Applied target:
  - Removed C/D candidate runtime references from `V1GameManager.cs`.
  - Returned the original Kalmuri blade sprite to the center of the effect language.
  - Active Hungry Blades uses irregular orbit blades with different speeds, radii, alpha, and arcs.
  - Nearby enemies trigger short hunting lunges from orbit toward the target, with cyan motion trails and higher-level recoil shards.
  - Active bite hits converge blades into the target, add crossing wound cuts, and throw return shards from +3 onward.
  - Kalmuri echo follow-ups use blade surges/fans instead of a large C/D candidate silhouette.
  - Hungry Blades memory-gain feedback uses outward blade spirals.
- Commands / checks:
  - `rg -n "PredatorBite|CrescentPack|candidate_c|candidate_d" LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs`
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - Unity MCP compile error check.
  - Unity MCP console error check.
  - Unity `LETHE/V1 Smoke/M2 Loop`.
  - Editor log check for `[V1QA] PASS`.
- Results:
  - No C/D candidate runtime references remain in `V1GameManager.cs`.
  - Runtime build passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity console error count: `0`.
  - M2 Loop QA logged `[V1QA] PASS`, with `HungryBlades:5`, `BloodReflection:5`, `storm=True`, and live enemies `10`.
- Limitation:
  - Automated QA confirms runtime safety. Direct jaewoo play is still needed to judge whether the new motion reads as release-quality enough.

# 2026-07-01 Kalmuri D-Only Runtime Follow-up

- Purpose:
  - Respond to jaewoo feedback: "D is not felt; do D only."
- Applied target:
  - Removed C / Crescent Pack from actual Hungry Blades runtime usage.
  - D / Predator Bite is now used for the active player-side Hungry Blades orbit, enemy-side bite hit, Kalmuri echo follow-up, echo transform sprite, and Hungry Blades memory-gain feedback.
  - Supporting orbit blades remain but are fewer and dimmer so D is the main visual read.
  - Active and echo bite frames were enlarged/brightened for stronger bite/lunge readability.
- Commands / checks:
  - `rg -n "KalmuriCrescentPack|CrescentPack|candidate_c" LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs`
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - Unity MCP compile error check.
  - Unity MCP console error check.
  - Unity `LETHE/V1 Smoke/M2 Loop`.
  - Editor log check for `[V1QA] PASS`.
- Results:
  - No C runtime references remain in `V1GameManager.cs`.
  - Runtime build passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity console error count before smoke: `0`.
  - M2 Loop QA logged `[V1QA] PASS`, with `HungryBlades:5`, `BloodReflection:5`, `storm=True`, and live enemies `10`.
- Limitation:
  - This is a D-only runtime feel pass, not a balance/stat change. Direct jaewoo play remains the visual readability gate.

# 2026-07-01 Kalmuri C/D Runtime Wiring

- Purpose:
  - Move the 2026-06-30 C/D Kalmuri candidate sprites from preview-only assets into actual `Dev_Prototype_v1` Hungry Blades runtime VFX.
- Applied target:
  - C / Crescent Pack: active Hungry Blades aura around the player, plus memory-gain feedback.
  - D / Predator Bite: enemy-side active bite frame and Kalmuri echo follow-up impact.
  - Existing small orbit blades kept as motion/detail support.
- Dual-blade check:
  - No new dual-blade VFX change was made in this unit.
  - Smaller-feeling attack likely comes from prior hit-point phantom weapon change and 2026-06-25 scale/lifetime/range reductions.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - Unity MCP compile error check.
  - Unity `LETHE/V1 Smoke/M2 Loop`.
  - Editor log check for `[V1QA] PASS`.
- Results:
  - Runtime build passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: `0`.
  - M2 Loop QA logged `[V1QA] PASS`, with `HungryBlades:5`, `BloodReflection:5`, `storm=True`, and live enemies `10`.
- Limitation:
  - Unity Game View screenshot did not reliably capture the forced Kalmuri combat frame in this session. Direct jaewoo play remains the final visual readability gate.

## 2026-06-30 Kalmuri C/D Sprite Candidate Assets

- Purpose:
  - Make the selected C/D Kalmuri candidate concepts visible as real Unity sprite assets before changing runtime VFX behavior.
- Created assets:
  - `LETHE/Assets/_dev/Art/Sprites/Echoes/Kalmuri/Candidates/spr_kalmuri_candidate_c_crescent_pack_01.png`
  - `LETHE/Assets/_dev/Art/Sprites/Echoes/Kalmuri/Candidates/spr_kalmuri_candidate_d_predator_bite_01.png`
  - `LETHE/Assets/_dev/Prefabs/Echoes/Kalmuri/Candidates/VFX_Kalmuri_Candidate_C_CrescentPack.prefab`
  - `LETHE/Assets/_dev/Prefabs/Echoes/Kalmuri/Candidates/VFX_Kalmuri_Candidate_D_PredatorBite.prefab`
  - `LETHE/Assets/_dev/Prefabs/Echoes/Kalmuri/Candidates/Preview_Kalmuri_C_D_SpriteCandidates.prefab`
- Commands / checks:
  - Unity MCP asset generation/import.
  - Unity MCP import settings query.
  - Unity compile error check.
  - Unity console error check.
- Results:
  - Both PNGs imported as `Sprite`, `Single`, PPU `256`, alpha transparency enabled.
  - Unity compile error count: `0`.
  - Unity console error count: `0`.
  - Visual evidence: `LETHE/Assets/_dev/Evidence/v1_kalmuri_cd_sprite_assets_20260630.png`.
- Limitation:
  - These are candidate sprites/prefabs only. Runtime Hungry Blades behavior is not switched to C/D until jaewoo chooses the final direction.

## 2026-06-30 Hungry Blades / Kalmuri Visual Refresh

- Purpose:
  - Verify jaewoo feedback that `굶주린 칼무리` should read more like a real blade swarm instead of a thin/underwhelming effect.
- Applied target:
  - Active Hungry Blades orbit uses sweeping moving blades instead of mostly static short-lived markers.
  - Outer blade ring is larger, brighter, and longer-lived.
  - Bright lead blades and stronger cyan outer trace ring are added.
  - Enemy-side bite VFX has more blades, a larger halo, and target-local cut trace.
  - Memory gain VFX uses a ring plus 12 orbiting blades.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - Unity MCP `Assets/Refresh`
  - Unity compile error check
  - Forced Play Mode state: `절단쌍검`, `HungryBlades:5`, 18 nearby enemies
  - Unity console error check
- Results:
  - Runtime build passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity console error count during forced Hungry Blades +5 check: `0`.
  - Visual evidence:
    - `LETHE/Assets/_dev/Evidence/v1_hungry_blades_kalmuri_refresh_20260630.png`
    - `LETHE/Assets/_dev/Evidence/v1_hungry_blades_kalmuri_refresh_camera_20260630.png`
- Limitation:
  - This verifies readability in a forced +5 setup. Direct play should still judge +1/+3/+5 progression and late-run clutter.

## 2026-06-30 Intro / Weapon Select UI Refresh

- Purpose:
  - Verify that the Unity `Dev_Prototype_v1` start screen reads more like a playable game shell while preserving the current weapon-only start flow.
- Applied target:
  - LETHE title treatment, dark full-screen intro backdrop, accent lines, and first-goal strip.
  - Two weapon cards with key badges, simple glyphs, rhythm summaries, and click/number-key affordance.
  - Compact layout for the current short Game View height.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - Unity MCP `Assets/Refresh`
  - Unity compile error check
  - Unity Play Mode intro capture and console error check
- Results:
  - Runtime build passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity console error count during intro capture: `0`.
  - Visual evidence saved at `LETHE/Assets/_dev/Evidence/v1_intro_weapon_select_ui_20260630_v2.png`.
- Limitation:
  - This is still OnGUI prototype UI, not final production UI. Direct play should judge whether the first-goal copy is helpful or too explanatory.

## 2026-06-30 Boss Pattern / No Reacquire Follow-up

- Purpose:
  - Verify jaewoo direct-play feedback fixes for second boss timing, short boss TTK, no boss pattern, HP-bar instability, unwanted memory reacquire, and Hungry Blades hitstop stutter.
- Applied target:
  - Gatekeeper schedule: `150 / 300 / 540 / 900s`.
  - Gatekeeper HP: `2200 / 4200 / 7600 / 12800`.
  - Hard cap: `1080s`.
  - Post-forget memory reacquire/refill: removed.
  - Gatekeeper pulse/guard pattern: added.
  - Hungry Blades orbit visual update during hitstop: added.
  - HP bar inverse scale: added.
- Commands:
  - `node scripts/balance_curve_v1.js`
  - `node scripts/verify_unity_stepped_balance.js`
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
- Results:
  - Balance curve script passed.
  - Static Unity balance verification passed.
  - Runtime build passed with 0 warnings and 0 errors after retry.
  - Editor build passed with 7 legacy warnings and 0 errors.
  - Unity MCP compile error count: `0`.
  - Unity MCP console error count: `0`.
- Limitation:
  - Unity MCP menu/play automation entered Play Mode, but a full `[V1QA] PASS` log was not captured because the MCP bridge restarted or returned a response parse error.

## Purpose

이 문서는 LETHE 작업이 성공했는지 판단하는 기준이다. 자동 검증은 회귀 확인용이고, 최종 GO/ITERATE/NO-GO는 jaewoo 플레이 체감 리뷰가 우선한다.

## Required Technical Checks

Unity/C# 작업 후 가능한 경우 아래를 실행한다.

```powershell
dotnet build LETHE/Assembly-CSharp.csproj --nologo
npm.cmd run report
npm.cmd run report:check
```

Unity MCP가 연결되어 있으면 추가로 확인한다.

- Unity compile error count = 0.
- `Dev_Prototype_v1.unity` open success.
- Play Mode smoke에서 player/enemy/weapon runtime exception 없음.
- 필요한 경우 evidence screenshot 저장.

## Current Epic Checks

### 20-Minute Beta Balance Pass, 2026-06-27

- Purpose:
  - Set a playable beta-run target before jaewoo direct play: 1 ultimate echo plus final Gatekeeper clear around 20 minutes.
- Applied target:
  - Run hard cap: `1260s`.
  - Gatekeeper schedule: `300 / 600 / 900 / 1140s`.
  - Gatekeeper HP: `1900 / 2800 / 4000 / 5400`.
  - Initial required XP: `7`.
  - XP multiplier: `0~120s x1.00`, `120~600s x1.34`, `600s+ x1.00`.
  - Timer-only survival win removed; all 4 Gatekeepers must be cleared.
- Simulation command:
  - `node scripts\balance_sim_v1.js`
- Simulation result for selected `20m_slow_start`:
  - First choice: `24~28s`.
  - First forgetting: `323~329s`.
  - Ultimate completion: `936~945s`.
  - Final clear: `1178~1188s`.
  - Evidence: `docs/orchestration/evidence/2026-06-27-balance-sim-v1.md`.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity console error count after QA: `0`.
  - Unity MCP QA rerun:
    - Dual blades: `[V1QA] PASS`, `elapsed=2.1`, `xp=0/7`, `enemies=8`.
    - Greatsword: `[V1QA] PASS`, `elapsed=2.1`, `xp=0/7`, `enemies=8`.
    - M2 loop: `[V1QA] PASS`, `HungryBlades:5`, `BloodReflection:5`, `storm=True`, `result=True`.
    - VFX Matrix: `[V1QA] PASS`, `previewMemory=8`, `previewEcho=8`, `fracture=1`, `stasis=1`, `ashen=1`.
    - Blood Blade Storm: `[V1QA] PASS`, `stormObjects=77`, `hungryEcho=5`, `bloodEcho=5`.
- Remaining risk:
  - Pure simulation does not model player dodging, death pressure, camera readability, or VFX clutter.
  - Greatsword route clear rate was lower than dual blades in pure sim, so this is the first route to inspect in MCP/hand play.

### Reliable MCP QA Line, 2026-06-27

- Purpose:
  - Replace ambiguous delayed smoke snapshots with explicit Unity Editor QA pass/fail checks before jaewoo direct play.
- QA menu paths:
  - `LETHE/V1 Smoke/Start Dual Blades`
  - `LETHE/V1 Smoke/Start Greatsword`
  - `LETHE/V1 Smoke/M2 Loop`
  - `LETHE/V1 QA/VFX Matrix`
  - `LETHE/V1 QA/Blood Blade Storm`
- Pass conditions:
  - Start weapon: runtime `elapsed >= 2.0`, at least 5 live enemies, `timeScale=1`, and no result/refill/death overlay.
  - M2 loop: Hungry/Blood echoes at +5, `BloodBladeStormReady`, result overlay, and at least 8 live enemies.
  - VFX Matrix: all 8 memory preview objects, all 8 echo preview objects, and `Preview_FractureExecution`, `Preview_StasisHunt`, `Preview_AshenOblivion`.
  - Blood Blade Storm: Hungry/Blood echoes at +5 plus actual `BloodBladeStorm*` runtime objects after `UpdateEchoUltimate` ticks.
- Unity MCP results:
  - Dual blades: `[V1QA] PASS`, `elapsed=2.0`, `liveEnemies=8`.
  - Greatsword: `[V1QA] PASS`, `elapsed=2.0`, `liveEnemies=8`.
  - M2 loop: `[V1QA] PASS`, `HungryBlades:5`, `BloodReflection:5`, `storm=True`, `result=True`.
  - VFX Matrix: `[V1QA] PASS`, `previewMemory=8`, `previewEcho=8`, `fracture=1`, `stasis=1`, `ashen=1`.
  - Blood Blade Storm: `[V1QA] PASS`, `stormObjects=77`, `hungryEcho=5`, `bloodEcho=5`.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors.
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity scene missing references: `0`.
  - Unity console error count after QA: `0`.
- Remaining risk:
  - The line is now strong enough for technical pre-play QA, but it still cannot judge hand-feel, final VFX readability, or whether the combat screen is emotionally exciting.

### MCP Automated Play/QA, 2026-06-27

- Connection:
  - AnkleBreaker Unity MCP targeted `LETHE` on port `7890`.
  - Active scene: `Assets/_dev/Scenes/Dev_Prototype_v1.unity`.
- Stability:
  - Unity compile error count: `0`.
  - Unity scene missing references: `0`.
  - Final Unity console error count after Play Mode stop: `0`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors.
- Smoke routes:
  - `LETHE/V1 Smoke/Start Dual Blades`: initialized `weapon=절단쌍검`.
  - `LETHE/V1 Smoke/Start Greatsword`: initialized `weapon=장송대검`; result snapshot showed `elapsed=2.2`, `enemies=10`.
  - `LETHE/V1 Smoke/M2 Loop`: injected `HungryBlades:5`, `BloodReflection:5`, and `storm=True`.
- Direct runtime probe:
  - Dual-blades run advanced to `elapsed=6.2`, `level=2`, `kills=2`, `enemies=26`, `timeScale=1`.
  - All 8 memory ids and all 8 echo ids spawned preview objects by reflection.
  - Utility preview path spawned 6 utility memory previews, 6 utility echo previews, and 3 non-blood ultimate previews.
  - All 8 echoes at +5 produced Blood Blade Storm readiness (`storm=True`).
- Remaining risk:
  - This is an automated technical QA pass. It cannot judge whether combat movement, VFX scale, echo identity, and HUD density feel good to jaewoo in direct play.

### Beta-Play Preparation, 2026-06-27

- Runtime asset catalog:
  - `V1ContentCatalog` exists under `Assets/_dev/Scripts/PrototypeV1/`.
  - `Assets/_dev/Data/V1_ContentCatalog.asset` contains 46 sprite references plus Korean font and weapon definitions.
  - `Dev_Prototype_v1` `V1_GameManager` references the catalog and both weapon definitions.
- Promotion prep:
  - `Assets/Lethe/` exists with `Scenes`, `Prefabs`, `Data`, `Art`, `UI`, `Audio`, and `Runtime`.
  - `Assets/Lethe/Scenes/Lethe_BetaPreview.unity` exists as a beta-facing candidate scene copy.
- UI:
  - HUD shows current echo summary and a short player objective line.
  - F12 debug / memory / echo review UI remains available by design for upcoming VFX review.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity scene missing references: `0`.
  - Unity Play Mode entered; `V1GameManager.DebugSnapshot()` returned a valid start state.
  - Unity console error count after stop: `0`.
- Not included:
  - Player build/export and debug UI removal were intentionally deferred.

### Prototype Completion Loop Pass, 2026-06-25

- Run completion:
  - Defeat, 600-second survival, and full Gatekeeper clear now use one result overlay.
  - The fourth Gatekeeper kill ends the prototype as a clear.
  - Result summary includes survival time, kills, Gatekeeper progress, weapon, level, choice count, forgotten memory count, and echo summary.
- Boss/progression feedback:
  - Gatekeeper kills now emit a clear burst and camera shake.
  - HUD shows Gatekeeper progress.
- Playtest presentation:
  - Debug panel is hidden by default and toggled with `F12`.
  - Minimal procedural SFX exist for select, weapon slashes, player hit, enemy kill, level-up, warning, boss clear, clear, and defeat.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`.
  - Unity console error count: `0`.
- Remaining risk:
  - Human playtest should confirm the clear screen, SFX volume, and hidden debug panel feel right in an actual run.

### Release-Feel Second Integration Pass, 2026-06-25

- Map/background:
  - Runtime now overlays connected Lethe river bands, banks, and sunken ruin slabs on the enlarged terrain.
  - Purpose is to reduce the separated-tile feeling and give the arena a larger world read before authored map chunks exist.
- Enemy/boss readability:
  - All runtime enemies now get HP bars.
  - Blood-mark color restore now returns to each enemy's base sprite color instead of plain white.
- Memory/echo value and VFX:
  - Execution Flash threshold, tick speed, VFX size/lifetime, and damage increased.
  - Hunter Oath fires faster, reaches up to 6 shots, and hits harder.
  - Shatter Wave has larger/longer fields, higher target cap, damage, and knockback.
  - Stopped Second echo has higher proc/freeze values and longer gold clock fields.
  - Ashen Shield and Oblivion Brand echoes have stronger proc/value; Ashen damage reduction increased.
  - Utility ultimates received larger VFX and stronger damage/freeze/heal values.
- Growth feedback:
  - Level-up now emits a cyan/gold burst around the player.
  - Ultimate-ready state now emits a colored burst, floating text, and camera shake.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`.
  - Unity console error count: `0`.
- Remaining risk:
  - Direct play is required to judge whether larger VFX and HP bars improve readability without making the screen too busy.

### Integrated Feel Pass, 2026-06-25

- Greatsword duplicate-VFX correction:
  - Profile-driven greatsword slash remains the main visible slash.
  - Guaranteed fallback now becomes a subdued tip afterglow/cut line when profile slashes exist.
  - If profile slashes are missing, fallback still emits a readable cleave.
- Weapon feel split:
  - Greatsword cadence `1.02 -> 0.92`, range `3.75 -> 3.85`, arc `96 -> 102`, max targets `6 -> 5`, hitstop/shake/knockback increased.
  - Dual blades cadence `0.36 -> 0.32`, damage `15 -> 13.5`, range `2.8 -> 2.72`, max targets `7 -> 6`, hitstop/shake shortened.
- Early run/readability pass:
  - First Gatekeeper timing `150s -> 135s`; schedule now `135 / 285 / 435 / 600`.
  - Gatekeeper warning lead `18s -> 22s`; warning field scale/lifetime increased.
  - First 120 seconds spawn pressure increased.
  - Enemy role markers and terrain memory landmarks are more readable.
  - Terrain base tint is darker to help enemies/VFX sit above the floor.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`.
  - Unity console showed only MCP server info logs and no gameplay errors/warnings.
- Remaining risk:
  - Direct play must judge whether the greatsword now reads as one strong slash, whether 135s first boss feels too early, and whether the darker terrain improves contrast without losing Lethe mood.

### One-by-One Memory / Echo Debug, 2026-06-25

- Added debug panel flow:
  - `Prev` / `Next`: cycle the selected memory/echo id.
  - `Mem One`: selected memory only at +5, with echoes and ultimate state cleared.
  - `Echo One`: selected echo only at +5, with ultimate updates suppressed.
  - `DB Rev` / `GS Rev`: integrated all-echo weapon review remains available.
- Intended manual review loop:
  - Pick one id.
  - Test `Mem One` to judge the memory's own readability and value.
  - Test `Echo One` to judge whether the matching echo reads as a distinct aftereffect.
  - Record ids that feel invisible, too similar, too weak, too noisy, or too strong.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors after Unity refresh.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`.
  - Unity console showed only MCP server info logs and no gameplay errors/warnings.
- Remaining risk:
  - Direct play is required to judge feel and visual distinction; this patch only creates the isolated test harness.

### Integrated Review Presets, 2026-06-25

- Added debug panel buttons:
  - `DB Rev`: dual-blade integrated review.
  - `GS Rev`: greatsword integrated review.
- Preset behavior:
  - Starts the run if needed.
  - Switches to the chosen weapon.
  - Spawns 18 nearby review enemies.
  - Sets all 8 echoes to +5.
  - Enables Echo Only mode to suppress ultimates.
  - Sets Gatekeeper warning timing to 20 seconds.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors after Unity refresh.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`.
  - Unity console showed only MCP server info logs and no gameplay errors/warnings.
- Remaining risk:
  - Direct play is required to judge whether the review presets expose the right amount of VFX clutter without creating an unrealistic worst-case screen.

### Greatsword VFX Guarantee, 2026-06-25

- Issue:
  - Jaewoo reported that the greatsword VFX appeared to be missing.
- Fix:
  - Greatsword hits now spawn a guaranteed cleave fallback in addition to profile-driven slash entries.
  - The fallback emits two large cleave arcs and one cut line at the calculated swing tip.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors after Unity refresh.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`.
  - Unity console showed only MCP server info logs and no gameplay errors/warnings.
- Remaining risk:
  - Human direct play should confirm whether the guaranteed cleave is now visible enough without becoming too noisy on top of the existing profile VFX.

### Prototype Gap Pass, 2026-06-24

- Weak points listed for direct review:
  - enlarged VFX may still need human scale/noise judgment.
  - first 180 seconds felt potentially loose.
  - enlarged map needed directional landmarks.
  - enemy/boss sprites needed stronger in-combat role readability.
  - scaled-up echoes may now risk clutter when stacked.
- Implemented technical pass:
  - First Gatekeeper timing moved from `180s` to `150s`.
  - Boss schedule changed to `150 / 300 / 450 / 600`.
  - First Gatekeeper HP changed from `2050` to `1750`.
  - Gatekeeper warning VFX appears 18 seconds before spawn.
  - Five subtle memory landmarks are generated on the arena floor.
  - Role markers are generated for Drifting Eye, Split One, Void Priest, and Gatekeeper.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors after Unity refresh.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`.
  - Unity console showed only MCP server info logs and no gameplay errors/warnings.
- Remaining risk:
  - Human direct play must confirm whether the 150-second first boss feels exciting rather than abrupt, and whether role markers/landmarks are visible without adding clutter.

### 8 Echo VFX Readability, 2026-06-24

- Combat VFX / attack coverage follow-up:
  - Global transient combat VFX scale is now `1.18x`.
  - Dual blades coverage changed to range `2.8`, arc `132`, max targets `7`, echo size scale `1.05`.
  - Greatsword coverage changed to range `3.75`, arc `96`, max targets `6`, echo size scale `2.15`.
  - Weapon VFX profiles were enlarged for crescents, cut flashes, Kalmuri followups, heavy ultimate slash, and hit sparks.
  - Hungry Blades, Shatter Echo, and Stopped Echo radii were increased so the readable field better matches the useful area.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors after Unity refresh.
    - Unity compile error count: `0`.
    - Unity Play Mode entry reached `isPlaying=true`.
    - Unity console showed only MCP server info logs and no gameplay errors/warnings.
  - Remaining risk:
    - Human direct play must judge whether the larger VFX now feel juicy or too noisy on the current terrain.

- Runtime hooks checked:
  - Kalmuri Echo
  - Blood Echo
  - Execution Echo
  - Hunter Echo
  - Shatter Echo
  - Stopped Echo
  - Ashen Echo
  - Oblivion Echo
- Technical readability pass:
  - Increased scale, alpha, lifetime, proc chance, or added support ring/halo/slash accents for the subtle echo families.
  - Kalmuri, Blood, Execution, Hunter, Shatter, Stopped, Ashen, and Oblivion now have clearer color/shape separation.
  - Echo identity follow-up:
    - Greatsword slash VFX delay is now `0.045s`, nearly immediate instead of `0.18s`.
    - Echoes now add hit-site accents: cut traces, wound marks, aim lines, fracture scars, clock clamps, return threads, and brand lines.
- Echo-only debug path:
  - `Echo A`: tests Execution / Hunter / Stopped echoes at +5 with ultimate updates suppressed.
  - `Echo B`: tests Shatter / Ashen / Oblivion echoes at +5 with ultimate updates suppressed.
  - `Echo All` or `F10`: tests all 8 echoes at +5 with ultimate updates suppressed.
  - `Prev` / `Next`: cycles the selected single echo in the debug panel.
  - `Echo One` or `F11`: tests only the selected echo at +5 with ultimate updates suppressed.
  - `F12`: cycles to the next selected echo.
  - `Ult 3`: remains separate for utility ultimate preview.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`.
  - Unity console error count: `0`.
- Remaining risk:
  - Human direct play is still required to judge whether each echo is distinguishable during real combat speed and under the new terrain background.

### Release-Prep Map / Background, 2026-06-24

- Terrain continuity follow-up:
  - Regenerated the eight terrain tiles from one shared wet black stone base to avoid unrelated tile chunks.
  - Runtime floor selection now mostly uses connected base tiles and keeps high-character variants rare.
  - Runtime water/root/gravel dressing density was reduced.
  - Floor tile scale increased slightly to overlap seams.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity `Assets/Refresh`: success.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`.
  - Unity console error count: `0`.
- Terrain redo follow-up:
  - Added image-generated source sheet `LETHE/Assets/_dev/Art/Source/spr_lethe_terrain_sheet_01_source.png`.
  - Generated 8 terrain tiles `tile_lethe_terrain_01.png` through `tile_lethe_terrain_08.png`.
  - Replaced runtime references from `tile_lethe_stone_*.png` / `spr_lethe_arena_backdrop_01.png` to `tile_lethe_terrain_*.png` / `spr_lethe_terrain_backdrop_01.png`.
  - Replaced artificial outer marker rings with natural marsh edge, water seam, drowned root, and memory gravel dressing.
  - Local visual check opened representative terrain tiles and backdrop.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity `Assets/Refresh`: success.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`.
  - Unity console error count: `0`.
- Generated map sprites:
  - `LETHE/Assets/_dev/Art/Sprites/Map/tile_lethe_stone_01.png`
  - `LETHE/Assets/_dev/Art/Sprites/Map/tile_lethe_stone_02.png`
  - `LETHE/Assets/_dev/Art/Sprites/Map/tile_lethe_stone_03.png`
  - `LETHE/Assets/_dev/Art/Sprites/Map/tile_lethe_stone_04.png`
  - `LETHE/Assets/_dev/Art/Sprites/Map/spr_lethe_arena_backdrop_01.png`
- Runtime arena checks:
  - Player clamp expanded from `x +/-12`, `y -8.5..8.5` to `x +/-24`, `y +/-16`.
  - Runtime floor coverage expanded from `11x9` to `21x15` tile placements.
  - Camera orthographic size changed from `6.1` to `6.8` and camera follow is clamped inside the enlarged arena.
  - Enemy spawn radius increased for the larger play space.
- Verification:
  - Local visual check opened representative generated tile/backdrop PNGs.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity `Assets/Refresh`: success.
  - Unity compile error count: `0`.
  - Unity Play Mode entry reached `isPlaying=true`.
  - Unity console error count: `0`.

### A. Data Contract

- Definition 타입이 컴파일된다.
- 새 memory/echo/enemy/ultimate/encounter 데이터를 추가해도 기존 weapon data asset이 깨지지 않는다.
- 기존 `Weapon_DualBlades`, `Weapon_Greatsword`, `VFX_Weapon_DualBlades`, `VFX_Weapon_Greatsword` 경로가 유지된다.

### B. Hit Feel / Echo Readability

- 쌍검 기본공격이 빠른 2연 반달 베기로 보인다.
- 대검 기본공격이 범위만큼 큰 반달 참격으로 보인다.
- 적은 피격 시 흰색 플래시와 데미지 숫자를 보여준다.
- 굶주린 칼무리는 작은 장식 오라가 아니라 여러 칼이 주변을 점유하고 타깃을 물어뜯는 군집으로 보인다.
- 칼무리 잔향은 캐릭터 주변 잡선이 아니라 타격 지점 후속타로 보인다.
- 혈반 잔향은 표식/실/피꽃으로 보인다.
- Current B-step technical check: `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed after Kalmuri range ring, Blood heal thread, Blood bloom thread, and knockback cap changes.
- Kalmuri readability follow-up, 2026-06-18: active Hungry Blades now uses a denser two-ring blade swarm and target-local bite blades. Kalmuri echo follow-ups now add blade barrage sprites. `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed with 7 legacy warnings and 0 errors. Unity compile error count was 0, and short Play Mode entry produced 0 console errors; human visual review is still required because the run starts behind the weapon-select overlay.
- Core VFX prompt-sheet replacement, 2026-06-21:
  - Added `docs/design/LETHE_SPRITE_PRODUCTION_PROMPTS.md`.
  - Replaced Kalmuri 3, Blood 3, and Blood Blade Storm 2 sprites with prompt-sheet generated PNGs.
  - Evidence: `LETHE/Assets/_dev/Evidence/core_vfx_prompt_sheet_refresh_20260621.png`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity AssetDatabase lists Kalmuri 3, Blood 3, and Ultimate 2 replacement textures.
  - Remaining risk: this verifies asset import and visual direction, not final in-run scale/timing.
- Remaining VFX prompt-sheet generation, 2026-06-21:
  - Generated weapon/hit VFX 5, active memory VFX 6, echo VFX 6, and ultimate VFX 3.
  - Evidence: `LETHE/Assets/_dev/Evidence/remaining_vfx_prompt_sheet_20260621.png`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity AssetDatabase found 20/20 generated VFX textures.
  - Unity import settings confirmed 20/20 final PNGs as Sprite textures.
  - Remaining risk: these assets are generated/imported, but runtime VFX profiles still need sprite wiring, scale, alpha, and timing review.
- Greatsword direct Play Mode cover fix, 2026-06-22:
  - Direct Greatsword run showed player bounds `(2.210, 2.210)` and sword bounds `(3.121, 4.995)`, ratioY `2.26`, with sword sorting order `30` in front of player sorting order `20`.
  - Greatsword held sprite was reduced to runtime scale `0.21~0.235`, shifted to the side, and moved behind the player at sorting order `18`.
  - Post-fix Play Mode check showed sword bounds `(2.327, 2.944)`, ratioY `1.33`, sword sorting order `18`, player sorting order `20`.
  - Forced greatsword attack produced `greatswordVfx=5` with max VFX bounds `(2.332, 2.332)` and Unity console error count 0.
- Weapon silhouette / attack VFX scale tune, 2026-06-22:
  - Dual blade held-weapon runtime scale increased to `0.43~0.475` and pulled closer to the body.
  - Greatsword held-weapon runtime scale reduced to `0.34~0.375` with shorter swing travel.
  - Generated dual-blade slash PNG scale factor increased from `0.153` to `0.192`.
  - Generated greatsword cleave PNG scale factor reduced from `0.225` to `0.182`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Play Mode runtime transform check confirmed dual blade scale `0.430`, greatsword scale `0.340`, console error count 0.
- Generated VFX runtime wiring, 2026-06-22:
  - Connected generated weapon/hit, six active memory, six echo, and three utility ultimate sprites to `V1GameManager`.
  - Weapon slash profile entries now prefer generated dual-blade arcs, greatsword cleave, Kalmuri slash, and cyan/red hit sparks before procedural fallback.
  - Added generated-sprite world-size scaling because the prompt-sheet PNGs are 1254px square and would otherwise render too large.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity `Assets/Refresh`: success.
  - Unity compile error count: 0.
  - Unity Play Mode smoke attempts produced console error count 0.
  - Remaining risk: Game/Scene capture still returned solid-color images, so jaewoo direct visual review must judge final scale, alpha, timing, and natural combat readability.
- Utility VFX / background / movement follow-up, 2026-06-23:
  - Greatsword slash VFX delay is now `0.18s`, about `64.3%` of the `0.28s` weapon sweep.
  - Execution, Hunter, Shatter, Stopped, Ashen, and Brand active/echo VFX were enlarged and made longer-lived.
  - Stopped Second now focuses on the nearest enemy cluster and draws clock-hand VFX.
  - Right-side debug panel now exposes `Mem A`, `Mem B`, `Echo A`, `Echo B`, `Ult 3`, and `VFX` review buttons.
  - Runtime arena dressing now creates 30 decorative boundary/crack/marker objects.
  - Player walk animation is softer: lower acceleration/deceleration, slower frame cadence, lower bob, and lower tilt.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity Play Mode smoke: `greatDelay=0.18`, `sweep=0.28`, `activeMemories=3`, `bgDecor=30`, `utilityVfx=36`, `enemies=14`.
  - Unity Play Mode echo/ultimate smoke: `echoCount=6`, `previewUlt=6`, `clockHands=21`.
  - Unity console error count: 0.
- Execution / Stopped Second readability follow-up, 2026-06-23:
  - Execution Flash active target width increased to `1.95`, lifetime to `0.38s`, and now adds vertical/horizontal/diagonal crack lines.
  - Execution Echo target width increased to `1.48` and uses the same crack burst language.
  - Stopped Second now draws a floor-readable clock field: clock face, outer/inner rings, 12 tick marks, hands, and core.
  - Stopped Echo and Stasis Hunt preview/ultimate VFX reuse the same clock-field language.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity Play Mode smoke: `executionCracks=16`, `executionVfx=24`, `clockFaces=5`, `clockTicks=60`, `clockHands=15`, `stoppedVfx=79`.
  - Unity console error count: 0.
- Stopped Second gold clock-field follow-up, 2026-06-23:
  - Hunter Oath remains in the yellow-green/green projectile family.
  - Stopped Second now uses yellow/gold time-stop colors, a larger clock face, brighter rings, thicker hands, larger ticks, a stronger core, and a rotating pulse ring.
  - Active Stopped Second freezes nearby enemies for up to `1.0s` while keeping the field visible for `1.50s`.
  - Stopped Echo and Stasis Hunt reuse the gold clock-field language with shorter supporting lifetimes.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity Play Mode smoke: `clockFaces=2`, `totalClockFaces=3`, `clockTicks=24`, `totalClockTicks=36`, `clockPulses=2`, `clockHands=6`, `goldFaces=2`, `frozenNear1s=5`.
  - Unity console error count: 0.
- Hunter Oath value follow-up, 2026-06-23:
  - Active Hunter Oath now fires 2/3/4 projectiles at levels 1/3/5 instead of a single weak projectile.
  - Projectile speed and damage were increased, and hits now create a short lock-on burst.
  - Hunter Echo proc chance and damage were increased; +5 can fire two echo shots.
  - Homing projectiles now retarget if their original target dies.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity Play Mode entry reached `isPlaying=true`; Unity console error count: 0.
- Lingering VFX / echo readability follow-up, 2026-06-23:
  - Shatter Wave active field now lasts `1.05s` and includes lingering rings/spokes.
  - Shatter Echo now lasts `0.90s` with the same field language.
  - Stopped Second active clock field now lasts `1.75s`.
  - Stopped Echo now uses a larger `1.02 + level*0.13` field for `1.25s`.
  - Execution, Hunter, Ashen, and Oblivion echo VFX were made longer/brighter/larger enough to read as echoes.
  - First boss timing remains `180s` until direct play confirms whether the first three minutes are still loose after VFX/readability improvements.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity Play Mode entry reached `isPlaying=true`; Unity console error count: 0.
- Enemy / boss sprite insertion, 2026-06-23:
  - Generated/imported `sheet_enemy_eye_4dir.png`, `sheet_enemy_splitter_4dir.png`, `sheet_enemy_voidpriest_4dir.png`, and `spr_boss_gatekeeper_01.png`.
  - Generated matching chroma source files under `_dev/Art/Source`.
  - Runtime `EnemySprite()` now loads these assets before procedural fallback.
  - Local visual check opened the eye, splitter, void priest, and gatekeeper images.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity `Assets/Refresh`: success.
  - Unity compile error count: 0.
  - Unity Play Mode entry reached `isPlaying=true`; Unity console error count: 0.
- Blood Blade Storm payoff / movement pass, 2026-06-22:
  - Blood Blade Storm now has opening cue, continuous pressure, and periodic burst pulses instead of only Kalmuri-like rotating blades.
  - Dual-blade storm uses faster blade orbit and more frequent bursts; greatsword storm uses slower/heavier slashes and burst impact.
  - Player movement now uses acceleration/deceleration smoothing, smoothed movement-facing weapon rotation, and subtle `PlayerVisual` walk bob/tilt.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity console error count after direct M2/ultimate reflection smoke: 0.
  - Direct M2 state injection confirmed `storm=True`; manual `UpdateEchoUltimate(0.12f)` reflection ticks created `bloodStormObjects=124` and cleared nearby spawned enemies with `kills=14`.
  - Remaining risk: MCP Play Mode time did not advance normally in this session, so the storm loop was verified by reflection ticks rather than natural timed play.
- Hit-point phantom weapon pass, 2026-06-22:
  - Player-attached `LeftBlade` / `RightBlade` renderers now stay disabled during normal play.
  - Dual-blade weapon identity is shown by two short-lived blade sprites spawned at the hit target.
  - Greatsword identity is shown by a heavy strike sprite plus a faint afterimage spawned at the cleave center.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity `Assets/Refresh`: success.
  - Unity compile error count: 0.
  - Dual-blade Play Mode reflection check: held renderers disabled, phantom count `2`, max bounds `(1.151, 1.151)`.
  - Greatsword Play Mode reflection check: held renderers disabled, phantom count `2`, max bounds `(1.586, 1.689)`.
  - Unity console error count: 0.
  - Remaining risk: this verifies technical behavior and rough bounds. Jaewoo direct play review still needs to judge whether the magical auto-blade look feels better than body-held weapons.
- Phantom weapon timing/readability pass, 2026-06-22:
  - Phantom weapons now sweep before weapon slash VFX appears.
  - Dual blades use a short `0.055s` slash / hit feedback delay after the weapon sweep starts.
  - Greatsword uses a slightly heavier `0.075s` slash / hit feedback delay after the weapon sweep starts.
  - Weapon slash lifetime is extended by `1.45x`, with minimum lifetimes of `0.34s` for dual blades and `0.48s` for greatsword.
  - Weapon hit spark and hit-confirm ring/core now use the same delayed timing as slash VFX.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Greatsword immediate Play Mode check: phantom `2`, active sweep `2`, slash `0`, spark `0`, confirm `0`.
  - Delayed enumerator check: greatsword slash `1`, spark `1`, confirm `2`, expected slash minimum lifetime `0.48s`.
  - Unity console error count: 0.
  - Remaining risk: Unity MCP time did not reliably advance coroutine time in this session, so delayed timing was verified through immediate state plus direct enumerator advancement. Final motion readability still requires jaewoo direct play review.
- Greatsword blade-tip alignment pass, 2026-06-23:
  - Greatsword phantom weapon now calculates the intended blade tip first, then places the weapon center so the handle points back toward the player body.
  - Greatsword slash VFX now anchors from the calculated blade-tip position, so `GreatswordCrescent_Primary` appears on the sword tip instead of drifting around the sword center.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Play Mode forced greatsword attack check: `handleCloser=True`, tip distance from player `1.67`, handle distance from player `0.16`.
  - Slash alignment check: desired tip and `GreatswordCrescent_Primary` position matched with distance `0.000`.
  - Unity console error count: 0.
  - Remaining risk: this confirms geometry and runtime errors, not final feel. Jaewoo direct play review should confirm the 45-degree sweep reads naturally.
- Greatsword handle-pivot / crescent direction pass, 2026-06-23:
  - Greatsword phantom sweep now rotates around a handle pivot instead of moving the whole weapon from start position to end position.
  - The handle pivot is placed on the player-facing side; the blade direction sweeps `-28` to `+28` degrees.
  - Greatsword crescent VFX now uses the sweep end blade direction plus a `180` degree facing correction so the fan/crescent faces with the sword.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Play Mode forced greatsword attack: `usePivot=True`, handle distance from player `0.13`, strike center distance `0.61`, start blade `-28.0`, end blade `28.0`.
  - Direct slash VFX check: end blade `28.0`, VFX rotation `208.0`, tip alignment error `0.000`.
  - Unity console error count: 0.
  - Remaining risk: automated verification confirms pivot geometry and VFX rotation, but final visual feel still needs jaewoo direct play review.
- Greatsword spectacle pass, 2026-06-23:
  - Greatsword handle-pivot sweep increased to a full `90` degree cut: `-45.0 -> 45.0`.
  - Greatsword wide crescent generated sprite scale factor increased from `0.150` to `0.175`.
  - Greatsword weapon-hit VFX profile scales/lifetimes increased so the slash reads more hot and flashy.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Play Mode forced greatsword attack: `usePivot=True`, blade sweep `-45.0 -> 45.0`, total `90.0`, AoE scale `1.65`, primary scale `1.38`.
  - Direct slash VFX check: end blade `45.0`, VFX rotation `225.0`, generated bounds `(4.28, 4.28)`, tip alignment error `0.000`.
  - Unity console error count: 0.
  - Remaining risk: this is intentionally more explosive; jaewoo direct review should confirm it is flashy, not screen-covering.
- Greatsword timing / coverage review loop, 2026-06-23:
  - Greatsword slash delay increased from `0.18s` to `0.22s`, so slash VFX appears after roughly `78.6%` of the `0.28s` sweep.
  - Greatsword minimum slash lifetime increased to `0.62s`.
  - Greatsword AoE / Primary / Assist crescent positions now use different points along the 90-degree blade-tip arc: `58%`, `78%`, and `72%`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity inline Game View capture succeeded on a frozen review frame with the sword held around `85%` of the swing and long-lived VFX visible.
  - Runtime value check: delay `0.22s`, sweep `0.28s`, min slash lifetime `0.62s`, AoE scale/lifetime `1.65 / 0.62`, Primary scale/lifetime `1.38 / 0.52`.
  - Unity console error count: 0.
  - Remaining risk: Game View capture was inline rather than saved to evidence. Jaewoo direct play review is still the final feel gate.
- Greatsword slash timing tighten, 2026-06-23:
  - Greatsword slash delay reduced from `0.22s` to `0.20s` because the VFX felt slightly late.
  - With the `0.28s` weapon sweep, slash VFX now appears after roughly `71.4%` of the swing.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Runtime value check: delay `0.20s`, sweep `0.28s`, slash appears at `71.4%`.
  - Unity console error count: 0.
- Dual blades / Blood Blade Storm / first-120 tempo pass, 2026-06-23:
  - Dual blades now use staggered VFX timing: A slash `0.045s`, cut flash `0.067s`, B slash `0.085s`, assist `0.045s`.
  - Dual-blade slash/spark profile scales and lifetimes were increased slightly.
  - Blood Blade Storm opening, continuous pressure, burst cadence, burst damage, heal, hitstop, and camera shake were increased.
  - First 120 seconds now have faster opening spawn intervals, a higher early enemy cap, and XP multiplier `2.15` before 120s.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Runtime check: opening spawn interval `0.46`, pack `2`; early `GrantXp(1)` produced `2/5` XP.
  - Blood Blade Storm smoke: `stormReady=True`, `stormObjects=187`, `burstObjects=45`, `bladeObjects=187`, `kills=21`.
  - Unity console error count: 0.

### C. Real M2 Loop

- 디버그 버튼 없이 60~120초 안에 망각/잔향/공명/+5/궁극 중 핵심 흐름이 보인다.
- 플레이어가 "이 기억을 키우면 다음에 잃는다"를 의식할 수 있다.
- Current C-step technical check: `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed after adding the M2 HUD objective text and the third-memory level-up card path.
- Stage/balance shell follow-up, 2026-06-18:
  - Normal runs now use 600s run duration, Gatekeepers at 180/340/490/600s, first boss HP 2050, 54s deficit survival, documented pressure phase spawn profiles, documented spawn caps, and all six run stat choices.
  - Fast/debug paths retain compressed timing for smoke tests.
  - Review-only automatic memory/+5 injection is no longer part of normal runs.
  - Transient VFX, floating text, damage numbers, and XP orbs now use internal object pools.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 0 warnings, 0 errors.
  - Unity `Assets/Refresh` then compile error count: 0.
  - Short Unity Play Mode entry: console error count 0.
  - Remaining risk: this confirms technical wiring, not final player-facing balance. A full manual run or compressed smoke still needs jaewoo review.

### J. 120초 초반 재미 루프

- 시작 화면은 무기+기억 조합 4개를 보여준다.
- 1~4 숫자키 또는 카드 클릭으로 아래 빌드가 시작된다:
  - 절단쌍검 + 굶주린 칼무리.
  - 절단쌍검 + 피의 반사.
  - 장송대검 + 굶주린 칼무리.
  - 장송대검 + 피의 반사.
- 혈반으로 시작해도 레벨업 카드에서 굶주린 칼무리가 우선 후보로 나온다.
- 칼무리로 시작해도 레벨업 카드에서 피의 반사가 우선 후보로 나온다.
- Current technical check, 2026-06-19:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity Play Mode entry reached `isPlaying=true`.
  - Unity console error log count: 0.
  - Camera-based Game View screenshot did not capture OnGUI start cards; direct human visual review is still required.
- Five-pass follow-up, 2026-06-19:
  - First-120-second kills grant +1 extra XP before the normal pre-boss multiplier.
  - Weapon hits spawn a confirm ring/core pulse for clearer hit reading.
  - Forgetting result text now includes loss, echo, overcharge/awakening, deficit survival, and resonance next action.
  - First-cycle spawn pressure has an early 120-second profile and closer spawn radius.
  - Drifting Eye, Split One, Void Priest, and Gatekeeper now have distinct procedural silhouettes.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed after each pass with 7 legacy warnings and 0 errors.
  - Final Unity MCP check: compile error count 0, Play Mode entered, console error count 0, Play Mode stopped.
  - Human review still needs to judge whether the first 120 seconds now feel busy, readable, and worth replaying.
- Direct Codex smoke follow-up, 2026-06-19:
  - Added `LETHE/V1 Smoke/*` editor menu items to run start routes and the M2 loop without manual keyboard/click input.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors for the smoke menu code.
  - Unity compile error count: 0.
  - Start build smoke snapshots:
    - `DualBlades + HungryBlades`: weapon `절단쌍검`, memories `[HungryBlades:1]`, result/refill/death all false.
    - `DualBlades + BloodReflection`: weapon `절단쌍검`, memories `[BloodReflection:1]`, result/refill/death all false.
    - `Greatsword + HungryBlades`: weapon `장송대검`, memories `[HungryBlades:1]`, result/refill/death all false.
    - `Greatsword + BloodReflection`: weapon `장송대검`, memories `[BloodReflection:1]`, result/refill/death all false.
  - M2 loop smoke snapshot: memories `[BloodReflection:3,HungryBlades:3]`, echoes `[HungryBlades:5,BloodReflection:5]`, enemies `10`, storm `True`, result overlay `True`, death `False`.
  - Unity console error log count after M2 smoke: 0.
  - Unity missing references: scene 0, assets 0.
  - Remaining risk: this is a technical smoke test, not a feel review. It proves the routes can initialize and the debug M2 loop wires up, but jaewoo still needs to play the first 120 seconds.
- Start-selection UX correction, 2026-06-19:
  - Start overlay now presents only two weapon choices: `절단쌍검` and `장송대검`.
  - `BeginRun(V1WeaponId)` no longer grants a starting memory.
  - `LETHE/V1 Smoke/Start Dual Blades` snapshot: weapon `절단쌍검`, memories `[]`, result/refill/death all false.
  - Forced first level-up after weapon start produced choices: `굶주린 칼무리 | 피의 반사 | 칼날 가속`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity console error count: 0.

- Visual/UI/game-feel refresh, 2026-06-19:
  - Player root movement is stable; the visual sprite is now on `PlayerVisual`.
  - Player body scale pulse was removed to avoid side-to-side wobble perception.
  - New player body sheet `sheet_player_v1_4dir.png` was generated/imported.
  - The new 8x4 player sheet now drives 4-direction idle/walk animation.
  - Greatsword uses the imported `spr_weapon_greatsword_01.png` sprite.
  - Arena floor has tile rotation, color variation, and small scale variation.
  - HUD was compacted into HP/XP/memory/ultimate/debug panels.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity `Assets/Refresh`: success.
  - Unity compile error count: 0.
  - Unity console error count: 0.
  - Short Unity Play Mode entry after the new player sheet: success, console error count 0.
  - Unity missing references: scene 0, assets 0.
  - Greatsword Play Mode smoke snapshot: `scene=v1 weapon=장송대검 elapsed=1.8 hp=210.0/210.0 enemies=6 death=False`.
  - Remaining risk: automated screenshot capture was discarded as solid color, so direct visual review is still required.

### H. Human Review Gate

jaewoo 리뷰 질문:

- 기본 공격이 무기별로 재미있나?
- 망각이 아깝나, 짜증나나?
- 잔향이 실제 전투를 바꾼다고 느껴지나?
- 재획득 공명이 설레나?
- +5/궁극이 후반 보상처럼 느껴지나?
- 다음에 고칠 가장 큰 문제 1~3개는 무엇인가?

### E/F/G. Content Expansion

- Current technical check: `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed with 0 warnings and 0 errors after adding first-pass active effects for the remaining memories, utility echo reactions, and three additional ultimate runtime branches.
- Current data asset check: `_dev/Data` now contains 8 `MemoryDefinition` assets, 8 `EchoDefinition` assets, and 4 `UltimateEchoDefinition` assets. Latest `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed with 7 legacy v0/debug deprecation warnings and 0 errors.
- Unity MCP check: after `Assets/Refresh`, AssetDatabase lists 8 `MemoryDefinition`, 8 `EchoDefinition`, and 4 `UltimateEchoDefinition` assets. `Dev_Prototype_v1` entered Play Mode with no console errors in the short smoke, and evidence was saved to `LETHE/Assets/_dev/Evidence/v1_content_data_asset_play_smoke_20260617.png`.
- Runtime exception QA: Unity console showed `InvalidOperationException: Collection was modified` in `V1GameManager.BloodBloom`. Area-effect loops over `enemies` now use snapshot lists and null guards. Follow-up Play Mode smoke showed no runtime exceptions.
- Runtime exception QA follow-up, 2026-06-18: `V1GameManager` enemy-list queries were scanned for remaining direct `enemies` enumeration/mutation risks. Added null guards to Hungry Blades target selection and enemy-cap counting. Final `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed with 0 warnings and 0 errors. A short Unity Play Mode smoke had passed before the patch with no runtime exceptions, but post-patch MCP recheck was blocked by `Transport closed`.
- Human review still needs to confirm whether these effects read as distinct enough, because Unity MCP visual verification was not available in this session.

## Known Non-Blocking Warnings

현재 `dotnet build`에는 legacy v0/debug 코드의 `Object.FindObjectOfType<T>()` deprecation warning 7개가 남아 있다. `Dev_Prototype_v1` compile error가 아니므로 현재 EPIC 진행을 막지 않는다.
# 2026-06-29 Stepped Boss Unity Runtime Patch Check

- Purpose:
  - Verify that the stepped boss / XP / DPS candidate is now applied to `Dev_Prototype_v1`.
- Commands:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - `node scripts/balance_curve_v1.js`
  - `node scripts/verify_unity_stepped_balance.js`
- Results:
  - Runtime build passed with 7 legacy v0/debug deprecation warnings and 0 errors.
  - Editor build passed with 0 warnings and 0 errors.
  - Unity MCP compile error count: `0`.
  - Unity MCP console error count: `0`.
  - Static balance verification passed for schedule, HP, XP curve, immediate refill, stepped spawn, cap, and HP helpers.
- Limitation:
  - `LETHE/V1 Smoke/*` menu execution could not be completed through MCP because `unity_execute_menu_item` repeatedly returned `Error polling queue: fetch failed` and restarted the MCP bridge. This should be retried before treating the patch as fully handoff-ready.

# 2026-06-29 Stepped Boss / XP / DPS Planning Check

- Purpose:
  - Convert jaewoo's "first boss is too late and boring" feedback into a numeric candidate before Unity runtime edits.
- Command:
  - `node scripts/balance_curve_v1.js`
- Result:
  - Gatekeeper schedule: `150 / 360 / 660 / 1020s`.
  - Gatekeeper HP: `1200 / 2250 / 4050 / 8650`.
  - Target TTK: `18 / 26 / 36 / 48s`.
  - Expected boss levels: `6 / 8 / 11 / 14`.
  - Expected boss DPS: `68 / 86 / 112 / 180`.
  - Evidence: `docs/orchestration/evidence/2026-06-29-stepped-boss-xp-dps-plan.md`.
- Remaining risk:
  - This is a planning/simulation check only. Unity runtime implementation, technical QA, and jaewoo first-6-minute review remain required.
