# Status

Last updated: 2026-07-02

## 2026-07-02 Update: Forget / Resonance UX Pass

- Implemented the third production-gap sequence item in `Dev_Prototype_v1`: forgetting and resonance now have a compressed action/VFX transition instead of relying only on text.
- Applied:
  - Added readable Korean forget-result overlay copy after the legacy overlay setup.
  - Added `ForgetFlow_*` VFX for lost memory, memory break, gained echo, memory-to-echo bridge, resonance target/thread, +5 awaken stamp/burst, and ultimate bridge.
  - Added `DebugRunForgetResonanceFlow()`.
  - Added `LETHE/V1 QA/Forget Resonance Flow`.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity console error count: `0`.
  - Unity QA `LETHE/V1 QA/Forget Resonance Flow`: `[V1QA] PASS`, `forgetFlow=15`, `echoTransform=2`, `ultimateReady=3`, `hungryEcho=5`, `bloodEcho=5`.
- Next step: expand the remaining three ultimate families with weapon-specific dual-blade/greatsword patterns.

## 2026-07-02 Update: Passive Memory Reinforcement Pass

- Implemented the second production-gap sequence item in `Dev_Prototype_v1`: active memories that previously felt too passive now have clearer independent action beats.
- Applied:
  - `BloodReflection` now periodically blooms, marks nearby victims, deals active pulse damage, creates +3 tether threads, and adds +5 awakened lash/bloom feedback.
  - `AshenShield` now adds +3 counter lines/counter damage and a +5 awakened shield wave with small recovery.
  - `StoppedSecond` now adds a memory beat ring, +3 aftercut lines, and a +5 wider freeze dome extension.
  - `OblivionBrand` now adds player-to-target tethering, +3 fork links/splash damage, and +5 awakened seal feedback.
  - Added `LETHE/V1 QA/Passive Memory Matrix`.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors.
  - Final editor build after cleanup: passed with 0 warnings and 0 errors.
  - Unity compile error count: `0`.
  - Unity console error count: `0`.
  - Unity QA `LETHE/V1 QA/Passive Memory Matrix`: `[V1QA] PASS`, `blood=13`, `ash=5`, `stopped=6`, `oblivion=24`.
- Next step: build the forgetting/resonance UX production pass so memory loss, echo gain, resonance target, and +5 awakening read as an action transition before text.

## 2026-07-01 Update: Skill SFX Runtime Pass

- Jaewoo requested skill-appropriate sound effects, with reference awareness from Vampire Survivors-like survival auto-combat.
- Applied:
  - Replaced the sine-only prototype sound palette with original runtime-generated clips.
  - Added wave/noise based SFX generation at 22.05 kHz for retro-readable short cues.
  - Added per-sound replay throttling to keep dense auto-attacks from becoming a wall of noise.
  - Wired sounds to weapon attacks, Hungry Blades lunge/pierce/echo, Blood Reflection mark/heal, utility memories, ultimates, XP pickup, kill, hit, warning, level-up, clear, and defeat moments.
  - No copyrighted Vampire Survivors audio was copied; the references informed audio grammar only.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity MCP compile error count: `0`.
  - Unity MCP console error count: `0`.
  - Play Mode direct `DebugRunM2Smoke()` returned `HungryBlades:5`, `BloodReflection:5`, `storm=True`, `enemies=10`, `result=True`, with console error count `0`.
- Remaining gate: jaewoo should direct-play with audio on and judge the mix balance, especially Kalmuri lunge/pierce repetition and Blood Blade Storm loudness.

## 2026-07-01 Update: Kalmuri Lunge Range / Stab Feel

- Jaewoo feedback: Kalmuri blades seemed to launch only at orbit range, and the flying blades did not feel like they were stabbing.
- Applied:
  - Added a larger `lungeRange` separate from the visible orbit.
  - Hungry Blades can now launch toward enemies farther than the rotating blade circle.
  - Bite blades start from the player-side orbit and travel through the target.
  - Per-blade damage is delayed with a short stagger so the hit follows the visual blade arrival.
  - Added a small pierce spark on delayed impact.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity MCP compile error count: `0`.
  - Unity MCP console error count: `0`.
  - Unity `LETHE/V1 Smoke/M2 Loop`: `[V1QA] PASS`.

## 2026-07-01 Update: Kalmuri Outer Orbit Removal / Per-Blade Damage

- Jaewoo screenshot feedback clarified that the visible problem was not only a drawn ring, but two rotating Kalmuri blade rings.
- Applied:
  - Removed the outer orbit layer from active Hungry Blades.
  - Replaced inner/outer orbit radii with one active `orbitRadius`.
  - Flying bite blades now each apply damage.
  - Total tick damage is split across the spawned blades to keep the damage budget stable.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity MCP compile error count: `0`.
  - Unity MCP console error count: `0`.
  - Unity `LETHE/V1 Smoke/M2 Loop`: `[V1QA] PASS`.

## 2026-07-01 Update: Kalmuri Orbit Ring Cleanup

- Jaewoo identified that the extra large ring around the player reads wrong for the current Kalmuri concept.
- Applied:
  - Removed `KalmuriSwarmBreathRing` from active Hungry Blades.
  - Increased the actual orbiting blade radius so the circle is carried by blades instead of a large drawn ring.
  - Increased active orbit blade count to `10..22`.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity MCP compile error count: `0`.
  - Unity `LETHE/V1 Smoke/M2 Loop`: `[V1QA] PASS`.

## 2026-07-01 Update: Kalmuri Living Swarm Motion Pass

- Jaewoo rejected both C and D candidate-image directions as not good enough.
- Implemented a new runtime direction: original Kalmuri blade sprite + dynamic swarm motion.
- Applied in `Dev_Prototype_v1`:
  - Removed C/D candidate runtime references from `V1GameManager.cs`.
  - Active Hungry Blades now has irregular multi-speed orbit blades.
  - Nearby enemies pull blades into hunting lunges from the orbit toward targets.
  - Higher levels add recoil/return shards so the swarm feels like it bites and comes back.
  - Active hit moments converge multiple blades into the target with crossing wound cuts.
  - Kalmuri echo follow-ups now read as blade surge/fan impacts instead of a large image stamp.
  - Memory-gain feedback is now an outward spiral of blades.
- Reference direction:
  - Immediate readable hit strokes like high-polish action roguelites.
  - Dense but clear survival-game swarm motion.
  - LETHE identity through cyan-white ghost blades, recoil, and returning forgotten edges.
- Verification:
  - No `PredatorBite`, `CrescentPack`, `candidate_c`, or `candidate_d` runtime references remain in `V1GameManager.cs`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity MCP compile error count: `0`.
  - Unity MCP console error count: `0`.
  - Unity `LETHE/V1 Smoke/M2 Loop`: `[V1QA] PASS`, `HungryBlades:5`, `BloodReflection:5`, `storm=True`, live enemies `10`.
- Next visual gate: jaewoo should play +1/+3/+5 and judge whether the lunge/recoil swarm now feels like a real Kalmuri identity.

## 2026-07-01 Update: Kalmuri D-Only Runtime Follow-up

- Jaewoo feedback: D / Predator Bite did not feel present enough when C was also used as the active aura.
- Applied a D-only runtime pass in `Dev_Prototype_v1`:
  - Removed C / Crescent Pack from actual Hungry Blades runtime usage.
  - D / Predator Bite is now the main active Hungry Blades player-side orbit read.
  - D remains and is strengthened on enemy-side Hungry Blades bite hits.
  - D remains and is strengthened on Kalmuri echo follow-up impacts.
  - Hungry Blades memory-gain and echo transform feedback now prefer D.
  - Supporting orbit blades remain, but are fewer/dimmer so D is not buried.
- Verification:
  - No `KalmuriCrescentPack` / `candidate_c` runtime references remain in `V1GameManager.cs`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity MCP compile error count: `0`.
  - Unity MCP console error count: `0`.
  - Unity `LETHE/V1 Smoke/M2 Loop`: `[V1QA] PASS`, `HungryBlades:5`, `BloodReflection:5`, `storm=True`, live enemies `10`.
- Next visual gate: jaewoo should play +1/+3/+5 Hungry Blades and judge whether D now reads as a constant bite/lunge identity without hiding enemies.

## 2026-07-01 Update: Kalmuri C/D Runtime Applied

- Hungry Blades / Kalmuri now uses the 2026-06-30 C/D candidate sprites in actual `Dev_Prototype_v1` runtime behavior.
- C / Crescent Pack is wired as the player-side active Hungry Blades aura, including a counter-rotating layer from +3 onward and memory-gain feedback.
- D / Predator Bite is wired as the enemy-side active bite frame and Kalmuri echo follow-up impact frame.
- Existing small orbit blades remain as motion/detail support, but the candidate sprites are now the main visual read.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity MCP compile error count: `0`.
  - Unity `LETHE/V1 Smoke/M2 Loop`: `[V1QA] PASS`, `HungryBlades:5`, `BloodReflection:5`, `storm=True`, live enemies `10`.
- Screenshot note: Unity Game View capture still does not reliably catch the forced Kalmuri frame in this project session, so direct jaewoo play remains the visual judgment gate.
- Dual blades check:
  - No new dual-blade change was made in this unit.
  - The smaller-feeling attack is explained by prior changes: 2026-06-22 moved visible weapons from held sprites to short hit-point phantom strikes, and 2026-06-25 reduced dual-blade slash scale/lifetime (`0.94 -> 0.86`, `0.88 -> 0.82`, min lifetime `0.34 -> 0.28`) plus range/target count (`2.8 -> 2.72`, `7 -> 6`).

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

- Unity-viewable Kalmuri C/D candidate sprite assets, 2026-06-30:
  - Created C / Crescent Pack as a transparent Unity sprite candidate for the always-on surrounding Kalmuri aura.
  - Created D / Predator Bite as a transparent Unity sprite candidate for the enemy-hit lunge/bite frame.
  - Added individual sprite prefabs and a side-by-side preview prefab under `LETHE/Assets/_dev/Prefabs/Echoes/Kalmuri/Candidates/`.
  - Evidence:
    - `LETHE/Assets/_dev/Evidence/v1_kalmuri_candidates_20260630.png`.
    - `LETHE/Assets/_dev/Evidence/v1_kalmuri_candidates_cd_focus_20260630.png`.
    - `LETHE/Assets/_dev/Evidence/v1_kalmuri_cd_sprite_assets_20260630.png`.
  - Verification:
    - Both PNGs import as Unity `Sprite`, `Single`, PPU `256`, alpha transparency enabled.
    - Unity compile error count: `0`.
    - Unity console error count: `0`.
  - Runtime Hungry Blades behavior was not changed in this pass; jaewoo should choose the final C/D mix first.

- Unity v1 Hungry Blades / Kalmuri visual refresh, 2026-06-30:
  - Responded to jaewoo feedback that `굶주린 칼무리` had become visually underwhelming.
  - Reworked active Hungry Blades orbit blades from short static markers into sweeping moving blades.
  - Increased outer ring radius, blade scale, alpha, and lifetime.
  - Added bright lead blades and a stronger cyan outer trace ring around the player.
  - Strengthened enemy-side bite VFX with more blades, larger halo, and target-local cut trace.
  - Strengthened Hungry Blades memory-gain feedback with a ring plus 12 orbiting blades.
  - Evidence:
    - `LETHE/Assets/_dev/Evidence/v1_hungry_blades_kalmuri_refresh_20260630.png`.
    - `LETHE/Assets/_dev/Evidence/v1_hungry_blades_kalmuri_refresh_camera_20260630.png`.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity MCP `Assets/Refresh`: success.
    - Unity compile error count: `0`.
    - Unity console error count during forced Hungry Blades +5 check: `0`.
    - Forced runtime snapshot: `weapon=절단쌍검`, `memories=[HungryBlades:5]`, `enemies=18`.

- Unity v1 intro / weapon-select UI refresh, 2026-06-30:
  - Reworked the `Dev_Prototype_v1` start overlay so it reads more like a game intro rather than a debug card picker.
  - Added LETHE title treatment, dark full-screen backdrop, accent lines, first-goal strip, and two clearer weapon cards.
  - Weapon cards now show number-key badges, simple weapon glyphs, rhythm summaries, and click/number-key selection affordance.
  - Preserved the current start model: the player chooses only the weapon first; memories are still selected from the first reward card.
  - Added compact layout behavior so the intro remains readable in the current short Game View.
  - Evidence: `LETHE/Assets/_dev/Evidence/v1_intro_weapon_select_ui_20260630_v2.png`.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity MCP `Assets/Refresh`: success.
    - Unity compile error count: `0`.
    - Unity console error count during Play Mode intro capture: `0`.

- Unity v1 jaewoo direct-play follow-up, 2026-06-30:
  - Feedback addressed:
    - first Gatekeeper timing felt good.
    - second Gatekeeper felt late.
    - boss TTK was far too short.
    - Gatekeeper had no pattern, so the player could free-hit too much.
    - enemy count/HP felt okay.
    - HP bars looked wrong.
    - memory reacquire after forgetting should be removed entirely.
    - Hungry Blades appeared to pause/stutter during base-attack hitstop.
  - Applied runtime values:
    - Gatekeeper schedule: `150 / 300 / 540 / 900s`.
    - Gatekeeper HP: `2200 / 4200 / 7600 / 12800`.
    - Hard cap: `1080s`.
  - Added Gatekeeper pulse/guard behavior to create a real boss pattern and reduce free-DPS uptime.
  - Removed normal post-forget memory reacquire/refill flow. Forgetting now turns the memory into an echo and returns directly to combat.
  - Fixed enemy/boss HP bars by counter-scaling the bar root against enemy squash/local scale.
  - Kept Hungry Blades orbit visuals updating during hitstop and throttled orbit VFX spawn cadence for optimization.
  - Verification:
    - `node scripts/balance_curve_v1.js`: passed.
    - `node scripts/verify_unity_stepped_balance.js`: passed.
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors after retry.
    - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
    - Unity MCP compile error count: `0`.
    - Unity MCP console error count: `0`.
  - Limitation: Unity MCP Play Mode/menu automation entered Play Mode but did not capture a full `[V1QA] PASS` log because the bridge restarted or returned a response parse error.

- Unity v1 20-minute beta direct-review preparation, 2026-06-29:
  - Rechecked Unity MCP state: active instance `LETHE` on port `7890`, active scene `Assets/_dev/Scenes/Dev_Prototype_v1.unity`.
  - Found and fixed two scene missing references on `V1_GameManager` by running `LETHE/_dev/Rebuild Prototype v1 Scene`.
  - Rechecked technical gate after rebuild:
    - Unity compile error count: `0`.
    - Unity scene missing references: `0`.
    - Unity asset missing references: `0`.
  - Reran the pre-play QA menu line:
    - Dual blades: `[V1QA] PASS`, `elapsed=2.1`, `liveEnemies=8`.
    - Greatsword: `[V1QA] PASS`, `elapsed=2.1`, `liveEnemies=8`.
    - M2 loop: `[V1QA] PASS`, `HungryBlades:5`, `BloodReflection:5`, `storm=True`, `result=True`.
    - VFX Matrix: `[V1QA] PASS`, `previewMemory=8`, `previewEcho=8`, `fracture=1`, `stasis=1`, `ashen=1`.
    - Blood Blade Storm: `[V1QA] PASS`, `stormObjects=77`, `hungryEcho=5`, `bloodEcho=5`.
  - Unity console error count during QA checks: `0`.
  - Created direct-review checklist: `docs/orchestration/review_prompts/2026-06-29-jaewoo-beta-run-review.md`.
  - Added `AudioListener` to `Main Camera` and to `V1SceneBuilder`; follow-up dual-blades smoke still logged `[V1QA] PASS` with console error count `0` and no repeated no-audio-listener log.

- Unity v1 20-minute beta-run balance pass:
  - Adopted a 20-minute target run for `Dev_Prototype_v1`: expected clear band `18~22m`, hard cap `1260s`, and normal clear through all 4 Gatekeepers rather than timer-only survival.
  - Changed Gatekeeper schedule to `300 / 600 / 900 / 1140s`.
  - Changed Gatekeeper HP to `1900 / 2800 / 4000 / 5400`.
  - Changed initial XP requirement to `7`.
  - Changed XP tempo to `0~120s x1.00`, `120~600s x1.34`, `600s+ x1.00`; removed the first-120-second kill XP bonus.
  - Extended reward focus from the Blood Blade Storm pair to all 4 ultimate echo pairs so route testing can cover Blood Storm, Fracture Execution, Stasis Hunt, and Ashen Oblivion.
  - Added `scripts/balance_sim_v1.js` and recorded evidence in `docs/orchestration/evidence/2026-06-27-balance-sim-v1.md`.
  - Simulation result for the selected `20m_slow_start` candidate:
    - first reward: `24~28s`.
    - first forgetting: `323~329s`.
    - ultimate completion: `936~945s`.
    - final clear: `1178~1188s`.
  - Known tuning risk: pure simulation rates greatsword route clears lower than dual blades (`0.63~0.68` vs `1.00`), so the next MCP/hand-play pass should inspect greatsword route consistency first.

- Unity v1 reliable MCP QA line:
  - Reworked `V1SmokeTestMenu` so smoke/QA runs wait for explicit pass conditions and log `[V1QA] PASS/FAIL` instead of relying on a fixed delayed snapshot.
  - Start-weapon QA now requires real runtime progress: `elapsed >= 2.0`, at least 5 live enemies, `timeScale=1`, and no result/refill/death overlay.
  - M2 QA now requires Hungry/Blood echoes at +5, `BloodBladeStormReady`, result overlay, and at least 8 live enemies.
  - Added `LETHE/V1 QA/VFX Matrix` to spawn and verify all 8 memory previews, all 8 echo previews, and 3 non-blood ultimate previews.
  - Added `LETHE/V1 QA/Blood Blade Storm` to force Hungry/Blood +5, tick the ultimate loop, and verify actual `BloodBladeStorm*` objects are generated.
  - Verified through Unity MCP:
    - Dual blades QA: PASS at `elapsed=2.0`, `liveEnemies=8`.
    - Greatsword QA: PASS at `elapsed=2.0`, `liveEnemies=8`.
    - M2 loop QA: PASS with `HungryBlades:5`, `BloodReflection:5`, `storm=True`, `result=True`.
    - VFX Matrix QA: PASS with `previewMemory=8`, `previewEcho=8`, `fracture=1`, `stasis=1`, `ashen=1`.
    - Blood Blade Storm QA: PASS with `stormObjects=77`, `hungryEcho=5`, `bloodEcho=5`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors.
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors.
  - Unity compile error count: `0`; scene missing references: `0`; Unity console error count after QA: `0`.

- Unity v1 MCP automated play/QA pass:
  - Confirmed AnkleBreaker Unity MCP is targeting `LETHE` on port `7890`.
  - `Dev_Prototype_v1` is open, clean, and contains the expected root objects: `V1_GameManager` and `Main Camera`.
  - Unity compile error count: `0`.
  - Unity scene missing references: `0`.
  - `LETHE/V1 Smoke/Start Dual Blades` initialized the dual-blades run without console errors.
  - `LETHE/V1 Smoke/Start Greatsword` initialized the greatsword run; after 2.2 seconds it reported `enemies=10`.
  - `LETHE/V1 Smoke/M2 Loop` injected the compressed Hungry/Blood loop with `HungryBlades:5`, `BloodReflection:5`, and `storm=True`.
  - Direct Play Mode reflection test confirmed a real dual-blades run advances to `elapsed=6.2`, `level=2`, `kills=2`, `enemies=26`, `timeScale=1`.
  - Individual VFX spawn probe confirmed all 8 memory ids and all 8 echo ids spawn one preview object each.
  - Utility VFX probe confirmed 6 utility memory previews, 6 utility echo previews, and the 3 non-blood ultimate previews.
  - Ultimate readiness probe confirmed 8 echoes at +5 and Blood Blade Storm readiness (`storm=True`) with no console errors.
  - Final Unity console error count after stopping Play Mode: `0`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors.
  - QA note: MCP can verify wiring, state, object creation, and runtime errors; it still cannot replace jaewoo's hand-feel/VFX readability judgment.

- Unity v1 beta-play preparation pass:
  - Added `V1ContentCatalog` and created `Assets/_dev/Data/V1_ContentCatalog.asset` with 46 sprite references, Korean font reference, and the two weapon definitions.
  - Wired `Dev_Prototype_v1` `V1_GameManager` to the catalog plus `Weapon_DualBlades` / `Weapon_Greatsword`, removing the two missing scene object references.
  - Added `Assets/Lethe/` promotion-prep structure (`Scenes`, `Prefabs`, `Data`, `Art`, `UI`, `Audio`, `Runtime`) and copied a beta-facing candidate scene to `Assets/Lethe/Scenes/Lethe_BetaPreview.unity`.
  - Updated the player-facing HUD to show current echoes and a short run objective while keeping the F12 debug / memory / echo review panel intact for upcoming VFX review.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity compile error count: `0`.
    - Unity scene missing references: `0`.
    - Unity Play Mode entered and `V1GameManager.DebugSnapshot()` returned a valid v1 start state.
    - Unity console error count after stop: `0`.
  - Scope note: player build/export is intentionally not part of this pass.

- Unity v1 prototype completion loop pass:
  - Added a shared result overlay for death, 600-second survival, and full Gatekeeper clear.
  - Fourth Gatekeeper kill now ends the prototype as `프로토타입 클리어`.
  - Result summary now reports survival time, kills, Gatekeepers cleared, weapon, level, level-up choices, forgotten memories, and echoes.
  - Added Gatekeeper clear burst VFX, minimal procedural SFX, Gatekeeper HUD progress, and an `F12` debug-panel toggle so the default view is closer to a playtest build.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity compile error count: `0`.
    - Unity Play Mode entry reached `isPlaying=true`.
    - Unity console error count: `0`.

- Unity v1 release-feel second integration pass:
  - Added connected Lethe river/bank bands and sunken ruin slabs over the large arena so the background reads less like separated tiles and more like one terrain space.
  - Added enemy/boss health bars and preserved each enemy sprite's base color when blood-mark flash clears.
  - Increased readability/value for Execution Flash, Hunter Oath, Shatter Wave, Stopped Second, Ashen Shield, Oblivion Brand, and utility ultimate echoes through larger VFX, longer fields, stronger proc rates, and higher damage/utility values.
  - Added level-up burst feedback and ultimate-ready burst feedback so growth moments are visible before reading the panel.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity compile error count: `0`.
    - Unity Play Mode entry reached `isPlaying=true`.
    - Unity console error count: `0`.

- Unity v1 integrated feel pass after deferring single memory/echo review:
  - Reframed the greatsword guaranteed slash so it no longer creates a second full cleave when the normal VFX profile is present.
  - When profile VFX exists, the fallback is now only a subdued tip afterglow and cut line; if profile VFX is missing, it still emits a readable cleave.
  - Split weapon feel through data:
    - Greatsword attacks faster (`1.02 -> 0.92`) with slightly larger range/arc, fewer max targets, stronger knockback, hitstop, and shake.
    - Dual blades attack faster (`0.36 -> 0.32`) with lighter individual hits, slightly shorter range, fewer max targets, and shorter hitstop.
  - Improved early pacing and readability:
    - First Gatekeeper timing moved to `135s`; schedule is now `135 / 285 / 435 / 600`.
    - Gatekeeper warning starts at `22s` and uses larger/longer warning rings.
    - First-120s spawn pressure, enemy role marker visibility, terrain darkness, and memory landmark visibility were increased.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity compile error count: `0`.
    - Unity Play Mode entry reached `isPlaying=true`.
    - Unity console showed only MCP server info logs and no gameplay errors/warnings.

- Unity v1 one-by-one memory/echo debug testing:
  - Added `Mem One` to the debug panel so jaewoo can test the selected memory alone at +5.
  - `Prev` / `Next` choose one of the 8 shared memory/echo ids.
  - `Mem One` starts the run if needed, spawns nearby review enemies, clears echo/ultimate state, and enables only the selected memory.
  - `Echo One` remains available for the selected echo alone at +5 with ultimate suppression.
  - `DB Rev` / `GS Rev` remain integrated all-echo review presets.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors after Unity refresh.
    - Unity compile error count: `0`.
    - Unity Play Mode entry reached `isPlaying=true`.
    - Unity console showed only MCP server info logs and no gameplay errors/warnings.

- Unity v1 integrated review presets:
  - Added `DB Rev` and `GS Rev` debug buttons for fast direct review.
  - Each preset starts the run if needed, switches weapon, spawns 18 nearby review enemies, sets all 8 echoes to +5, suppresses ultimates with Echo Only mode, and sets Gatekeeper warning timing to 20 seconds.
  - Purpose: quickly test weapon VFX, echo clutter, enemy role readability, and boss warning timing without waiting through a full run.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors after Unity refresh.
    - Unity compile error count: `0`.
    - Unity Play Mode entry reached `isPlaying=true`.
    - Unity console showed only MCP server info logs and no gameplay errors/warnings.

- Unity v1 greatsword VFX missing-read fix:
  - Responded to jaewoo feedback that the greatsword VFX appeared to be gone.
  - Added a guaranteed greatsword cleave fallback on every successful greatsword hit.
  - The fallback spawns two large cleave arcs and a cut line aligned to the current greatsword swing tip, so the attack does not depend solely on VFX profile entry filtering.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors after Unity refresh.
    - Unity compile error count: `0`.
    - Unity Play Mode entry reached `isPlaying=true`.
    - Unity console showed only MCP server info logs and no gameplay errors/warnings.

- Unity v1 prototype gap pass:
  - Listed the current weak points after the VFX/range scale-up:
    - enlarged VFX still needs human noise/readability review.
    - first 180 seconds could feel loose.
    - enlarged map lacked directional landmarks.
    - enemy/boss sprites still needed stronger in-combat role readability.
    - stacked echoes may now risk clutter after scale-up.
  - Implemented three safe fixes:
    - First Gatekeeper timing moved from `180s` to `150s`; full schedule is now `150 / 300 / 450 / 600`.
    - First Gatekeeper HP reduced from `2050` to `1750`.
    - Added an 18-second Gatekeeper warning VFX.
    - Added five subtle memory landmarks to the arena.
    - Added low-alpha role markers for Drifting Eye, Split One, Void Priest, and Gatekeeper.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors after Unity refresh.
    - Unity compile error count: `0`.
    - Unity Play Mode entry reached `isPlaying=true`.
    - Unity console showed only MCP server info logs and no gameplay errors/warnings.

- Unity v1 combat VFX / attack coverage scale-up:
  - Responded to jaewoo feedback that the effects may be too small and the game should enlarge both VFX and attack range.
  - Added a shared `CombatVfxVisibilityScale = 1.18f` to enlarge transient combat VFX, weapon phantom sweeps, hit sparks, clock hands, slash traces, and prompt-style echo sprites through the common spawn path.
  - Increased dual blades data:
    - range `2.35 -> 2.8`
    - arc `119 -> 132`
    - max targets `6 -> 7`
    - echo size scale `0.8 -> 1.05`
  - Increased greatsword data:
    - range `3.15 -> 3.75`
    - arc `82 -> 96`
    - max targets `5 -> 6`
    - echo size scale `1.8 -> 2.15`
  - Enlarged dual-blade/greatsword VFX profile scales for crescents, cut flashes, hit sparks, Kalmuri followups, and heavy Blood Blade Storm slash.
  - Increased Hungry Blades, Shatter Echo, and Stopped Echo field radii so visual size and functional coverage are closer.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors after Unity refresh.
    - Unity compile error count: `0`.
    - Unity Play Mode entry reached `isPlaying=true`.
    - Unity console showed only MCP server info logs and no gameplay errors/warnings.

- Unity v1 single-echo debug review flow:
  - Added a per-echo isolated test path so jaewoo can identify exactly which echo is unreadable or too similar.
  - Debug panel now shows the selected echo name and includes `Prev`, `Echo One`, and `Next`.
  - Hotkeys:
    - `F10`: all 8 echoes at +5, ultimate suppressed.
    - `F11`: selected single echo at +5, ultimate suppressed.
    - `F12`: cycle to the next echo.
  - Next review order:
    - Test one echo at a time with `Echo One` / `F11`.
    - Mark only echoes that remain invisible, too similar to memories, or too noisy.
    - Then review map/background/enemy contrast after echo identity is judged.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
    - Unity compile error count: `0`.
    - Unity console error count: `0`.

- Unity v1 echo identity / greatsword timing follow-up:
  - Responded to jaewoo feedback that echoes may still be either low-visibility or too similar to memories.
  - Reduced greatsword slash VFX delay from `0.18s` to `0.045s` so the cleave appears almost immediately after the weapon swing.
  - Added hit-site echo identity accents so echoes read as weapon-hit aftereffects rather than standalone memory skills:
    - Kalmuri: added cyan cut trace at the hit origin.
    - Blood: added red wound slash on the marked enemy.
    - Execution: added golden execution cut line over the target.
    - Hunter: added hit-origin target mark and aim-line links toward homing targets.
    - Shatter: added fracture scar lines at the hit point before the field pulse.
    - Stopped: added small gold clock clamp on the struck enemy and slightly tightened the echo field.
    - Ashen: changed read toward hit seal -> return thread -> smaller player guard.
    - Oblivion: added purple brand seal and brand lines on the target.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
    - Unity compile error count: `0`.
    - Unity Play Mode entry reached `isPlaying=true`.
    - Unity console error count: `0`.

- Unity v1 8-echo VFX readability follow-up:
  - Confirmed all 8 implemented echoes already had runtime VFX hooks, but several were too subtle to read during combat.
  - Added Echo Only debug mode so jaewoo can test echoes without triggering ultimate awakenings:
    - `Echo A`: Execution / Hunter / Stopped echoes at +5, ultimate loop suppressed.
    - `Echo B`: Shatter / Ashen / Oblivion echoes at +5, ultimate loop suppressed.
    - `Echo All` or `F10`: all 8 echoes at +5, ultimate loop suppressed.
    - `Ult 3` remains the separate utility ultimate preview path.
  - Strengthened per-echo visual reads in `V1GameManager`:
    - Kalmuri: brighter range ring, added inner flash ring, larger/longer blade barrage, stronger +5 launch blade.
    - Blood: added mark pulse ring, larger/longer Blood Bloom, added Blood Bloom ring, slightly larger bloom hit radius.
    - Execution: larger golden prompt, added execution halo, larger/longer burst.
    - Hunter: larger/brighter echo projectile and longer target-lock ring.
    - Shatter: higher echo proc chance, larger/longer field, brighter rings and fracture spokes.
    - Stopped: higher echo proc chance, larger/longer gold clock field, slightly longer freeze.
    - Ashen: higher proc chance, larger/longer player guard VFX and added guard ring.
    - Oblivion: higher proc chance, larger/longer purple brand and added slash accent.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
    - Unity compile error count: `0`.
    - Unity Play Mode entry reached `isPlaying=true`.
    - Unity console error count: `0`.

- Unity v1 terrain continuity follow-up:
  - Responded to jaewoo feedback that the map did not feel connected and tiles looked like separate pieces.
  - Regenerated `tile_lethe_terrain_01..08.png` from the same wet black stone base instead of eight unrelated terrain categories.
  - Runtime floor selection now mostly uses base terrain tiles, with high-character variants only as rare variation.
  - Water seams, drowned roots, and memory gravel were reduced so they act as dressing rather than separate terrain chunks.
  - Floor tile scale was increased slightly to overlap hard seams.
  - Verification:
    - Local visual check opened regenerated terrain tiles and backdrop.
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
    - Unity `Assets/Refresh`: success.
    - Unity compile error count: `0`.
    - Unity Play Mode entry reached `isPlaying=true`.
    - Unity console error count: `0`.

- Unity v1 Lethe terrain background follow-up:
  - Responded to jaewoo feedback that the new background still felt like an artificial field and should feel more like Vampire Survivors-style terrain.
  - Used built-in image generation to create a natural terrain concept sheet for the Lethe world: wet black stone, mud bank, shallow turquoise water seams, cracked slate, memory-shard gravel, drowned roots, ash soil, and worn marsh path.
  - Saved the generated source sheet into the project at `LETHE/Assets/_dev/Art/Source/spr_lethe_terrain_sheet_01_source.png`.
  - Updated `scripts/generate_world_sprites.ps1` so it crops the sheet into eight Unity-ready terrain tiles and a terrain backdrop.
  - Runtime wiring:
    - `V1GameManager` now loads `tile_lethe_terrain_01..08.png` instead of the previous artificial stone tile set.
    - `ArenaBackdropPath` now points to `spr_lethe_terrain_backdrop_01.png`.
    - Artificial outer marker rings were removed.
    - Runtime dressing now uses marsh edges, Lethe water seams, drowned roots, and memory gravel.
  - Verification:
    - Local visual check opened representative terrain tiles and the new terrain backdrop.
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
    - Unity `Assets/Refresh`: success.
    - Unity compile error count: `0`.
    - Unity Play Mode entry reached `isPlaying=true`.
    - Unity console error count: `0`.
  - Next review:
    - Directly play both weapons and judge whether the new terrain supports LETHE's world identity while staying quiet enough for combat VFX.

- Unity v1 release-prep map / background pass:
  - Responded to jaewoo saying the background should be redone, the map is too small, and LETHE should start moving from prototype feel toward release preparation.
  - Generated a reproducible first map art set:
    - `LETHE/Assets/_dev/Art/Sprites/Map/tile_lethe_stone_01.png`
    - `LETHE/Assets/_dev/Art/Sprites/Map/tile_lethe_stone_02.png`
    - `LETHE/Assets/_dev/Art/Sprites/Map/tile_lethe_stone_03.png`
    - `LETHE/Assets/_dev/Art/Sprites/Map/tile_lethe_stone_04.png`
    - `LETHE/Assets/_dev/Art/Sprites/Map/spr_lethe_arena_backdrop_01.png`
  - Added `scripts/generate_world_sprites.ps1` as the local generator for those map sprites and source PNGs.
  - Runtime changes:
    - Player bounds expanded from prototype clamps `x +/-12`, `y -8.5..8.5` to `x +/-24`, `y +/-16`.
    - Arena floor placement expanded from `11x9` to `21x15` tiles.
    - Camera orthographic size increased from `6.1` to `6.8`.
    - Camera follow now clamps inside the enlarged arena instead of drifting outside the map.
    - Enemy spawn radius increased so the larger map produces more travel and positioning room.
    - Arena dressing now uses the new backdrop and bigger boundary/crack/marker layout.
  - Verification:
    - Local visual check opened the generated floor tile and backdrop PNGs.
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity `Assets/Refresh`: success.
    - Unity compile error count: `0`.
    - Unity Play Mode entry reached `isPlaying=true`.
    - Unity console error count: `0`.
  - Next review:
    - Directly play both weapons on the larger map and judge whether the world now feels like an actual release-facing arena rather than a small test room.

- Unity v1 enemy / boss sprite insertion:
  - Responded to jaewoo asking about the next enemy/boss sprite step.
  - Generated first-pass prototype sprites:
    - `LETHE/Assets/_dev/Art/Sprites/Enemies/Eye/sheet_enemy_eye_4dir.png`
    - `LETHE/Assets/_dev/Art/Sprites/Enemies/Splitter/sheet_enemy_splitter_4dir.png`
    - `LETHE/Assets/_dev/Art/Sprites/Enemies/VoidPriest/sheet_enemy_voidpriest_4dir.png`
    - `LETHE/Assets/_dev/Art/Sprites/Enemies/Bosses/spr_boss_gatekeeper_01.png`
  - Generated matching chroma source files under `LETHE/Assets/_dev/Art/Source/`.
  - Added `scripts/generate_enemy_boss_sprites.ps1` as the reproducible local generator.
  - Runtime wiring:
    - `V1GameManager.EnemySprite()` now loads the new sprites for Drifting Eye, Split One, Void Priest, and Gatekeeper before procedural fallback.
  - Visual role read:
    - Drifting Eye = floating eye with tendrils.
    - Split One = amber dividing blob.
    - Void Priest = hooded caster with green casting cue.
    - Gatekeeper = large armored guardian / door motif.
  - Verification:
    - Opened local PNGs for visual sanity.
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity `Assets/Refresh`: success.
    - Unity compile error count: `0`.
    - Unity Play Mode entry reached `isPlaying=true`.
    - Unity console error count: `0`.
  - Next review:
    - Run both starting weapons and check whether the new enemy roles are visually distinct enough during combat, especially under VFX clutter.

- Unity v1 lingering VFX / echo readability follow-up:
  - Responded to jaewoo review that Shatter Wave and Stopped Second should persist on screen instead of disappearing immediately, and that echo VFX may feel absent.
  - Shatter Wave:
    - Active VFX now uses a lingering field helper with main ring, outer/inner hold rings, and radial fracture spokes.
    - Active field lifetime is now `1.05s`.
    - Shatter Echo now uses the same field language at `0.90s`.
  - Stopped Second:
    - Active clock-field lifetime increased from `1.50s` to `1.75s`.
    - Stopped Echo field increased from `0.92 + level*0.11 / 0.90s` to `1.02 + level*0.13 / 1.25s`.
    - Stasis Hunt pulse was moved fully into the gold clock-field language with longer prompt/field lifetimes.
  - Echo readability:
    - Execution, Hunter, Ashen, and Oblivion echo VFX received longer lifetimes, stronger alpha, and slightly larger scale.
  - Pacing:
    - First boss remains at `180s` for now; the first three minutes should be judged after this readability pass before changing boss timing.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity compile error count: `0`.
    - Unity Play Mode entry reached `isPlaying=true`.
    - Unity console error count: `0`.
  - Next review:
    - Use `Mem B`, `Echo B`, and `VFX` debug buttons to check Shatter Wave / Shatter Echo / Stopped Second persistence before deciding whether boss timing should move earlier.

- Unity v1 Hunter Oath value follow-up:
  - Responded to jaewoo review that Hunter Oath felt weaker than basic attacks and had no meaningful reason to pick.
  - Active Hunter Oath:
    - Now fires a volley instead of a single projectile.
    - Level 1 fires `2` shots, level 3 fires `3`, and level 5 fires `4`.
    - Cooldown is now `max(0.62, 1.25 - level*0.11)`.
    - Projectile speed is now `9.4 + level*0.85`.
    - Projectile damage is now `13 + level*4.8`.
    - Each hit spawns a short lock-on burst that can damage nearby enemies.
  - Hunter Echo:
    - Proc chance is now `0.30 + level*0.08`.
    - Damage is now `weaponDamage * (0.22 + level*0.055)`.
    - +5 can fire two echo shots.
  - Projectile reliability:
    - Homing shots retarget to the nearest living enemy if the original target dies mid-flight.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity compile error count: `0`.
    - Unity Play Mode entry reached `isPlaying=true`.
    - Unity console error count: `0`.
  - Next review:
    - Use `Mem A` / `VFX` and judge whether Hunter Oath now feels like a real ranged tracking memory without overtaking Blood Blade Storm or Kalmuri as the main payoff.

- Unity v1 Stopped Second gold clock-field follow-up:
  - Responded to jaewoo review that Stopped Second should be yellow like a time/clock field, remain visible during the 1-second stop, and deliver more spectacle.
  - External reference check:
    - Eternal Return Henry's Chrono Field describes a time field around Henry that persists for a period, slows enemies/projectiles, then explodes. The LETHE change borrows the readable "persistent time field" idea, not the full skill behavior.
  - Color language:
    - Hunter Oath remains a yellow-green/green projectile VFX.
    - Stopped Second is now the yellow/gold clock-field memory.
  - Stopped Second:
    - Active freeze window now reaches up to `1.0s`.
    - Active clock field lifetime is `1.50s`, so the VFX remains visible through and slightly after the stop.
    - Clock face, rings, 12 ticks, hands, core, and pulse ring were enlarged/brightened for stronger readability.
  - Echo / ultimate:
    - Stopped Echo uses a shorter `0.90s` gold field.
    - Stasis Hunt preview/ultimate uses the same gold field language at `1.20s~1.50s`.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity compile error count: `0`.
    - Unity Play Mode smoke: `clockFaces=2`, `totalClockFaces=3`, `clockTicks=24`, `totalClockTicks=36`, `clockPulses=2`, `clockHands=6`, `goldFaces=2`, `frozenNear1s=5`.
    - Unity console error count: `0`.
  - Next review:
    - Use `Mem A` / `VFX` debug buttons and judge whether Stopped Second now reads as a gold time-stop floor field without hiding enemies or weapon hits.

- Unity v1 Execution Flash / Stopped Second readability follow-up:
  - Responded to jaewoo review that Execution Flash still felt too small and Stopped Second needed an obvious clock-floor telegraph.
  - Execution Flash:
    - Active memory VFX target width increased from `1.30` to `1.95`.
    - Lifetime increased to `0.38s`.
    - Added a crack burst helper that draws vertical, horizontal, and diagonal execution fracture lines plus a bright core.
    - Execution Echo also increased to `1.48` target width and uses the same crack language.
  - Stopped Second:
    - Replaced subtle clock-hand-only feedback with a full clock-field floor telegraph.
    - The field now draws a clock face, outer/inner rings, 12 tick marks, clock hands, and center core.
    - Stopped Echo and Stasis Hunt preview/ultimate paths reuse the same clock-field language.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity compile error count: `0`.
    - Unity Play Mode smoke: `executionCracks=16`, `executionVfx=24`, `clockFaces=5`, `clockTicks=60`, `clockHands=15`, `stoppedVfx=79`.
    - Unity console error count: `0`.
  - Next review:
    - Use `Mem A` / `VFX` debug buttons and judge whether Execution Flash is now big enough and Stopped Second reads as a clock field without hiding enemies.

- Unity v1 utility VFX / background / movement follow-up:
  - Responded to jaewoo review that game feel is now promising, but greatsword slash VFX can appear faster and non-core memories such as Stopped Second feel invisible.
  - Greatsword slash delay reduced from `0.20s` to `0.18s`; with the `0.28s` weapon sweep, slash VFX appears at roughly `64.3%`.
  - Utility active/echo VFX pass:
    - Execution, Hunter, Shatter, Stopped, Ashen, and Brand active memory VFX now use larger target widths, higher alpha, longer lifetimes, and secondary cue sprites.
    - Stopped Second now targets the nearest enemy cluster, draws clock hands, and applies a stronger freeze window.
    - Utility echo VFX was similarly enlarged and lengthened so it no longer reads as absent when it procs.
  - Utility ultimate pass:
    - Fracture Execution, Stasis Hunt, and Ashen Oblivion previews are larger and longer-lived.
    - Stasis Hunt includes clock-hand VFX and stronger freeze readability.
  - Debug review:
    - Right-side debug panel now exposes `Mem A`, `Mem B`, `Echo A`, `Echo B`, `Ult 3`, and `VFX`.
    - These buttons let jaewoo immediately inspect utility memory/echo/ultimate VFX without waiting for a full run.
  - Arena / movement:
    - Runtime arena dressing now adds dark backdrop, boundary bands, memory cracks, and outer markers.
    - Player walk feel was softened with lower acceleration/deceleration, slower walk cadence, less bob, and less tilt.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity compile error count: `0`.
    - Unity Play Mode utility smoke: `greatDelay=0.18`, `sweep=0.28`, `activeMemories=3`, `bgDecor=30`, `utilityVfx=36`, `enemies=14`.
    - Unity Play Mode echo/ultimate smoke: `echoCount=6`, `previewUlt=6`, `clockHands=21`; utility echoes reached +5.
    - Unity console error count: `0`.
  - Next review:
    - Jaewoo should use the new debug buttons first, then play the first 120 seconds to judge whether utility memories are now visible without cluttering the screen.

- Unity v1 dual blades / Blood Blade Storm / first-120 tempo pass:
  - Completed the three next work items after greatsword reached a reviewable state.
  - Dual blades:
    - Kept the greatsword readability principle but scaled it down for a fast weapon.
    - A slash appears at `0.045s`, cut flash at `0.067s`, B slash at `0.085s`.
    - Slash/spark profile scales and lifetimes were increased slightly.
  - Blood Blade Storm:
    - Opening cue, ring/blade scale, burst cadence, pressure damage, heal, hitstop, and camera shake were increased.
    - Heavy storm now has larger slower bursts; dual-blade storm has denser faster rotating blades.
  - First 120 seconds:
    - Opening spawn cadence increased from `0.52 -> 0.46`.
    - 35-80s spawn cadence increased from `0.58 -> 0.52`.
    - 80-120s profile increased from `0.50 x3` to `0.46 x4`.
    - Early enemy cap increased `28 -> 32`.
    - Early XP multiplier increased `1.95 -> 2.15` before 120s.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity compile error count: `0`.
    - Runtime check: dual delays A `0.045`, flash `0.067`, B `0.085`, assist `0.045`; opening spawn interval `0.46`, pack `2`; early `GrantXp(1)` produced `2/5` XP.
    - Blood Blade Storm smoke: `stormReady=True`, `stormObjects=187`, `burstObjects=45`, `bladeObjects=187`, `kills=21`.
    - Unity console error count: `0`.
  - Next review:
    - Jaewoo should directly play both weapons and check whether the first 120 seconds now reaches card choices/combat pressure quickly enough without becoming noisy.

- Unity v1 greatsword slash timing tighten:
  - Responded to jaewoo review that the greatsword slash VFX felt slightly slow.
  - Greatsword slash delay reduced from `0.22s` to `0.20s`.
  - With the `0.28s` weapon sweep, slash VFX now appears at about `71.4%` of the motion instead of `78.6%`.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity compile error count: `0`.
    - Runtime value check: delay `0.20s`, sweep `0.28s`, slash appears at `71.4%`.
    - Unity console error count: `0`.

- Unity v1 greatsword timing / coverage review loop:
  - Responded to jaewoo review that the greatsword VFX may still feel too fast because it appears while the sword is swinging, and that VFX size/position needs finer matching to the swept range.
  - Greatsword slash delay increased from `0.18s` to `0.22s`; with the `0.28s` sweep this makes slash VFX appear at about `78.6%` of the weapon motion.
  - Greatsword phantom lifetime increased to `0.42s`; minimum slash lifetime increased to `0.62s`.
  - Greatsword AoE / Primary / Assist VFX now sample different points along the 90-degree tip arc:
    - AoE: `58%`, to cover the middle-late arc.
    - Primary: `78%`, to sit near the late blade path.
    - Assist: `72%`, to stay between AoE and final tip.
    - Shock/cutpoint remains on the final tip.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity compile error count: `0`.
    - Unity inline Game View capture succeeded for a frozen review frame with sword at about `85%` of the swing and long-lived VFX visible.
    - Runtime value check: delay `0.22s`, sweep `0.28s`, min slash lifetime `0.62s`, AoE scale/lifetime `1.65 / 0.62`, Primary scale/lifetime `1.38 / 0.52`.
    - Unity console error count: `0`.
  - Dual-blade note:
    - Dual blades should use the same readability principle, but as shorter staggered cross-slashes rather than a large fan. This remains a follow-up pass after greatsword review.

- Unity v1 greatsword spectacle pass:
  - Responded to jaewoo review that the cut still lacked heat and spectacle.
  - Greatsword handle-pivot sweep increased from `-28 -> +28` to `-45 -> +45`, giving a full `90` degree cut.
  - Greatsword wide crescent generated sprite scale factor increased from `0.150` to `0.175`.
  - Greatsword weapon-hit VFX profile scales/lifetimes increased:
    - AoE crescent scale `1.24 -> 1.65`, lifetime `0.42 -> 0.50`.
    - Primary crescent scale `1.02 -> 1.38`, lifetime `0.34 -> 0.42`.
    - Shock, cut point, and assist crescent were also enlarged slightly.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity compile error count: `0`.
    - Play Mode forced greatsword attack: `usePivot=True`, blade sweep `-45.0 -> 45.0`, total `90.0`, AoE scale `1.65`, primary scale `1.38`.
    - Direct slash VFX check: end blade `45.0`, VFX rotation `225.0`, generated bounds `(4.28, 4.28)`, tip alignment error `0.000`.
    - Unity console error count: `0`.
  - Limitation:
    - This verifies scale and geometry. Final review should confirm the larger slash feels flashy rather than too screen-covering.

- Unity v1 greatsword handle-pivot / crescent direction pass:
  - Responded to jaewoo review that the greatsword slash VFX was reversed and the sword motion should feel like rotating around a handle gizmo, not shifting the whole sword position.
  - Greatsword phantom attacks now use a pivot sweep mode in `V1WeaponPhantomSweep`.
  - The handle pivot stays on the player-facing side while the blade direction rotates from `-28` to `+28` degrees.
  - Greatsword crescent VFX now uses the sweep end blade direction with a `180` degree facing correction so the fan/crescent opens with the sword motion.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity compile error count: `0`.
    - Play Mode forced greatsword attack: `usePivot=True`, handle distance from player `0.13`, strike center distance `0.61`, start blade `-28.0`, end blade `28.0`.
    - Direct slash VFX check: end blade `28.0`, VFX rotation `208.0`, tip alignment error `0.000`.
    - Unity console error count: `0`.
  - Limitation:
    - This validates runtime geometry and VFX rotation. The final "human-like cut" feel still needs jaewoo direct play review.

- Unity v1 greatsword blade-tip alignment pass:
  - Responded to jaewoo review that the greatsword handle should face the player body and the slash VFX should be calculated from the sword tip.
  - Greatsword phantom attack now calculates the intended blade-tip path first.
  - The weapon sprite center is placed backward from that tip so the local handle side stays closer to the player.
  - Greatsword slash VFX now applies the sprite profile offset in reverse, anchoring `GreatswordCrescent_Primary` to the calculated blade-tip position.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity `Assets/Refresh`: success.
    - Unity compile error count: `0`.
    - Forced greatsword attack Play Mode check: `handleCloser=True`, tip distance from player `1.67`, handle distance from player `0.16`.
    - Slash alignment check: desired tip and `GreatswordCrescent_Primary` position matched with distance `0.000`.
    - Unity console error count: `0`.
  - Limitation:
    - This validates geometry and runtime health. Final motion readability still needs jaewoo direct play review because screenshot capture remains unreliable.

- Unity v1 phantom weapon timing/readability pass:
  - Responded to jaewoo review that hit VFX duration was too short, the attacked target was hard to read, and the weapon type was not visible enough.
  - Phantom weapon sprites now sweep across the hit point before slash / spark / hit-confirm VFX appears.
  - Dual-blade sweep:
    - starts immediately at the hit point.
    - rotates roughly `46` degrees over `0.13~0.14s`.
    - remains visible for `0.24~0.26s`.
    - delays slash / hit feedback by `0.055s`.
  - Greatsword sweep:
    - starts immediately at the cleave center.
    - rotates roughly `48~50` degrees over `0.20~0.22s`.
    - remains visible for `0.34~0.38s`.
    - delays slash / hit feedback by `0.075s`.
  - Weapon slash VFX lifetime now uses `1.45x` runtime extension with minimum lifetimes:
    - dual blades: `0.34s`.
    - greatsword: `0.48s`.
  - Weapon hit spark and hit-confirm ring/core now use the same short delay as slash VFX.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity `Assets/Refresh`: success.
    - Unity compile error count: `0`.
    - Greatsword immediate Play Mode check: phantom `2`, active sweep `2`, slash `0`, spark `0`, confirm `0`.
    - Delayed enumerator check: greatsword slash `1`, spark `1`, confirm `2`, expected slash minimum lifetime `0.48s`.
    - Unity console error count: `0`.
  - Limitation:
    - Unity MCP Play Mode time did not reliably advance coroutine time in this session, so delayed timing was verified through immediate-state checks plus direct enumerator advancement.
    - Final readability of the sweep timing still needs jaewoo direct play review.

- Unity v1 hit-point phantom weapon pass:
  - Responded to jaewoo review that weapons attached to the character body looked like a mistake, and that weapons should instead appear at the slash / hit VFX when attacking.
  - Player-attached `LeftBlade` / `RightBlade` renderers now stay disabled during normal play.
  - Dual blades now spawn two short-lived phantom blade sprites around the target-local slash.
  - Greatsword now spawns a heavy phantom blade strike plus a faint afterimage at the cleave center.
  - Phantom weapon sprites use world-height scaling so generated weapon PNGs do not inherit oversized source image dimensions.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity `Assets/Refresh`: success.
    - Unity compile error count: `0`.
    - Dual-blade Play Mode reflection check: held renderers disabled, `DualBladePhantom*` count `2`, max bounds `(1.151, 1.151)`.
    - Greatsword Play Mode reflection check: held renderers disabled, `GreatswordPhantom*` count `2`, max bounds `(1.586, 1.689)`.
    - Unity console error count: `0`.
  - Limitation:
    - This verifies technical behavior and rough bounds. Final judgment of readability, fantasy, and whether the attack feels natural still needs jaewoo direct play review because screenshot capture remains unreliable.

- Unity v1 direct greatsword cover fix:
  - Direct Play Mode check confirmed the greatsword was covering the player:
    - player bounds: `(2.210, 2.210)`.
    - sword bounds: `(3.121, 4.995)`, ratioY `2.26`.
    - sword sorting order `30`, player sorting order `20`.
  - Greatsword held sprite was reduced again:
    - runtime scale `0.34~0.375 -> 0.21~0.235`.
    - moved behind the player with sorting order `18`.
    - shifted to the side with tighter swing travel.
  - Greatsword cleave PNG scale factor was reduced again:
    - `0.182 -> 0.150`.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity compile error count: `0`.
    - post-fix sword bounds: `(2.327, 2.944)`, ratioY `1.33`.
    - forced greatsword attack VFX count `5`, max VFX bounds `(2.332, 2.332)`.
    - Unity console error count: `0`.

- Unity v1 held weapon silhouette / attack VFX scale tune:
  - Responded to jaewoo review that the greatsword was too large and dual blades were too small.
  - Dual blade held sprites now use runtime scale `0.43~0.475` instead of the previous `0.30~0.33`, and sit closer to the player at `x=±0.19`.
  - Greatsword held sprite now uses runtime scale `0.34~0.375` instead of the previous `0.44~0.51`, with shorter swing travel and a tighter player-relative position.
  - Generated attack VFX scale was rebalanced:
    - dual-blade slash PNG scale factor `0.153 -> 0.192`.
    - greatsword cleave PNG scale factor `0.225 -> 0.182`.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity compile error count: `0`.
    - Play Mode runtime transform check: dual blades scale `0.430`, greatsword scale `0.340`.
    - Unity console error count: `0`.
  - Note:
    - Dedicated weapon attack sprites already exist and are wired. Procedural fallback is acceptable for resilience, but final review should keep weapon-specific attack sprites because they carry most of the hit-readability.

- Unity v1 Blood Blade Storm payoff / player movement pass:
  - Responded to jaewoo review that Kalmuri and Blood Blade Storm were acceptable, but Blood Blade Storm did not yet feel like a superior payoff and player walking still felt unnatural.
  - Player movement now smooths raw input into short acceleration/deceleration instead of direct per-frame movement.
  - `PlayerVisual` now has subtle walk bob/tilt, and movement-facing weapon-anchor rotation is smoothed to reduce snapping.
  - Blood Blade Storm now has:
    - an opening cue.
    - continuous storm pressure that marks and tugs nearby enemies.
    - periodic burst pulses with stronger damage, healing, blood-heal threads, knockback, hitstop, and camera shake.
    - dual-blade fast orbit/burst cadence and greatsword slower heavy slash cadence.
  - Added a defensive `BeginRun` guard for debug/smoke calls before the cached player reference exists.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity compile error count: `0`.
    - Unity console error count after direct M2/ultimate reflection smoke: `0`.
    - Direct M2 state injection reached `storm=True`.
    - Manual reflection ticks through `UpdateEchoUltimate(0.12f)` created `bloodStormObjects=124`, cleared nearby spawned enemies, and reached `kills=14`.
  - Limitation:
    - MCP Play Mode time did not advance normally in this session (`elapsed=0.0` stayed fixed), so the storm loop was verified through direct reflection ticks rather than natural timed gameplay.
    - Final visual/feel judgment still needs jaewoo direct play review.

- Unity v1 generated VFX runtime wiring and scale pass:
  - Wired the generated prompt-sheet PNGs into `V1GameManager` runtime spawn paths:
    - weapon/hit VFX: dual blade arcs, greatsword cleave arc, cyan/red hit sparks.
    - active memory VFX: Execution Flash, Hunter Oath, Shatter Wave, Stopped Second, Ashen Shield, Oblivion Brand.
    - echo VFX: Execution, Homing, Shockwave, TimeStop, Ashen, Brand.
    - ultimate VFX: Fracture Execution, Stasis Hunt, Ashen Oblivion.
  - Added sprite world-size normalization so the 1254px generated sprites do not cover the combat field at Unity default PPU.
  - Kept procedural sprite generation as fallback when an imported PNG is unavailable.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
    - Unity active scene: `Dev_Prototype_v1`.
    - Unity compile error count: `0`.
    - Unity console error count after Play Mode smoke attempts: `0`.
    - Unity `Assets/Refresh`: success.
  - Limitation:
    - Unity Game/Scene capture still returned a solid-color image, so direct visual review remains required for final scale/feel judgment.
    - Manual smoke invocation confirmed no runtime errors, but transient VFX timing was not visually capturable through the current MCP screenshot path.

- Unity v1 remaining VFX prompt-sheet generation:
  - Generated the remaining prompt-sheet sprite set under `LETHE/Assets/_dev/Art/Sprites/`:
    - weapon/hit VFX: `5`.
    - active memory VFX: `6`.
    - echo VFX: `6`.
    - ultimate VFX: `3`.
  - Matching chroma source PNGs were preserved under `LETHE/Assets/_dev/Art/Source/`.
  - Evidence contact sheet:
    - `LETHE/Assets/_dev/Evidence/remaining_vfx_prompt_sheet_20260621.png`.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors.
    - Unity active scene: `Dev_Prototype_v1`.
    - Unity compile error count: `0`.
    - Unity AssetDatabase found generated VFX textures: `20/20`.
    - Unity import settings confirmed Sprite texture type: `20/20`.
  - Remaining work: wire these dedicated sprites into the runtime VFX profile/spawn paths and tune scale, alpha, sort order, and duration during Play Mode.

- Unity v1 sprite prompt sheet and core VFX replacement:
  - Added a clean sprite-generation source document:
    - `docs/design/LETHE_SPRITE_PRODUCTION_PROMPTS.md`.
  - Replaced the existing core Kalmuri/Blood/Blood Blade Storm runtime sprites using the new prompt sheet:
    - `spr_kalmuri_orbit_blade_01.png`
    - `spr_kalmuri_echo_slash_01.png`
    - `spr_kalmuri_launch_blade_01.png`
    - `spr_blood_mark_01.png`
    - `spr_blood_bloom_01.png`
    - `spr_heal_thread_tip_01.png`
    - `spr_blood_blade_storm_ring_01.png`
    - `spr_blood_blade_storm_blade_01.png`
  - Source chroma images were preserved under `LETHE/Assets/_dev/Art/Source/`.
  - Evidence contact sheet:
    - `LETHE/Assets/_dev/Evidence/core_vfx_prompt_sheet_refresh_20260621.png`.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors.
    - Unity active scene: `Dev_Prototype_v1`.
    - Unity compile error count: `0`.
    - Unity AssetDatabase lists Kalmuri `3`, Blood `3`, and Ultimate `2` replacement textures.
  - Remaining work: generate missing weapon arcs/hit sparks, six non-Kalmuri/Blood memory VFX, six matching echo VFX, three remaining ultimate VFX, enemies/boss, and UI icons.

- Unity v1 visual/UI/game-feel refresh:
  - Player body no longer uses `V1BillboardPulse`, so movement should not read as side-to-side body wobble.
  - Player rendering is now on a stable child `PlayerVisual`, with the root reserved for actual movement.
  - Added and imported a new player body sheet:
    - `LETHE/Assets/_dev/Art/Source/sheet_player_v1_4dir_chroma.png`
    - `LETHE/Assets/_dev/Art/Sprites/Characters/Player/sheet_player_v1_4dir.png`
  - The new 8x4 player sheet is now used as 4-direction idle/walk animation instead of a single static frame.
  - Weapon anchor is centered below the player to reduce silhouette drift while moving.
  - Added and imported a dedicated transparent greatsword sprite:
    - `LETHE/Assets/_dev/Art/Source/spr_weapon_greatsword_01_chroma.png`
    - `LETHE/Assets/_dev/Art/Sprites/Weapons/spr_weapon_greatsword_01.png`
  - Arena floor tiles now have rotation, color, and subtle scale variation so the field reads less like a flat repeated grid.
  - HUD was compacted into clearer HP/XP/memory/ultimate panels with smaller debug controls.
  - Verification:
    - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
    - Unity `Assets/Refresh`: success.
    - Unity compile error count: `0`.
    - Unity console error count: `0`.
    - Short Unity Play Mode entry after the new player sheet: success, console errors `0`.
    - Unity missing references: scene `0`, assets `0`.
    - Greatsword sprite asset loaded as `spr_weapon_greatsword_01`.
    - Play Mode Greatsword smoke snapshot: `scene=v1 weapon=장송대검 elapsed=1.8 hp=210.0/210.0 enemies=6 death=False`.
  - Limitation: camera screenshot capture produced a solid-color image and was discarded; direct visual review is still needed.

- Unity v1 start-selection UX correction:
  - The first overlay now selects only the starting weapon: `절단쌍검` or `장송대검`.
  - `굶주린 칼무리` and `피의 반사` are no longer attached to the weapon cards.
  - `BeginRun(V1WeaponId)` no longer grants a starting memory.
  - First level-up after weapon-only start verified choices: `굶주린 칼무리 | 피의 반사 | 칼날 가속`.
  - `LETHE/V1 Smoke/*` start menus now match weapon-only start routes.
  - Verification: `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed with 7 legacy warnings and 0 errors; Unity compile errors `0`; Unity console errors `0`.

- Unity v1 direct Codex smoke-test follow-up:
  - Added `LETHE/V1 Smoke/*` editor menus for four start build routes and the M2 loop.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors.
  - Unity compile error count: `0`.
  - Start-build smoke snapshots confirmed the expected weapon and starting memory for:
    - DualBlades + HungryBlades.
    - DualBlades + BloodReflection.
    - Greatsword + HungryBlades.
    - Greatsword + BloodReflection.
  - M2 loop smoke confirmed Hungry/Blood echoes at +5, enemies `10`, `storm=True`, result overlay active, and death false.
  - Unity console error log count: `0`.
  - Unity missing references: scene `0`, assets `0`.
  - Remaining work: this is technical smoke coverage, not a human feel verdict.

- Unity v1 120-second early fun-loop start pass:
  - Added `J. 120초 초반 재미 루프` to `docs/TASK.md` as the current player-facing implementation checklist.
  - Replaced the weapon-only start overlay with four start build cards:
    - `절단쌍검 + 굶주린 칼무리`.
    - `절단쌍검 + 피의 반사`.
    - `장송대검 + 굶주린 칼무리`.
    - `장송대검 + 피의 반사`.
  - `BeginRun` now accepts a starting memory, while old debug callers still default to Hungry Blades.
  - Level-up rewards now preserve missing Kalmuri/Blood core-memory choices so either start route can quickly reach the Kalmuri/Blood loop.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity MCP:
    - compile error count `0`.
    - Play Mode reached `isPlaying=true`.
    - console error log count `0`.
    - Play Mode stopped successfully.
  - Limitation: camera-based Game View screenshots do not capture OnGUI start cards, so direct human visual review remains required.

- Unity v1 120-second early fun-loop five-pass follow-up:
  - Completed five small implementation passes as separate commits:
    - `695771f feat: 초반 보상 속도 보정`.
    - `fbf0f0f feat: 무기 타격 확인 피드백 추가`.
    - `111cdab feat: 망각 결과 UX 강화`.
    - `b19a1b6 feat: 초반 스폰 압박 보정`.
    - `081e13b feat: 적 역할 실루엣 보정`.
  - Early non-boss kills grant +1 XP during the first 120 seconds.
  - Weapon hits now add a confirm ring/core pulse.
  - Forgetting result copy now names the loss, echo, awakening/overload, deficit survival, and resonance next action.
  - First-cycle early spawn pressure now uses a first-120-second profile with closer spawn radius.
  - Drifting Eye, Split One, Void Priest, and Gatekeeper have distinct procedural silhouettes instead of plain colored circles.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed after each pass with 7 legacy warnings and 0 errors.
  - Final Unity MCP check: compile error count `0`, Play Mode entered, console error count `0`, Play Mode stopped.
  - Remaining work: human review must judge whether the first 120 seconds are actually more fun and readable.

- Unity v1 stage/balance shell and object-pool pass:
  - User direction: pause VFX/art feedback for now; make sure every memory works, implement the stage/balance shell from the design docs, and optimize VFX/objects with pooling.
  - Audit result:
    - All 8 active memories have first-pass runtime behavior in `V1GameManager`.
    - All 8 matching echoes have first-pass weapon-hit or event reactions.
    - All 4 ultimate branches have first-pass runtime paths.
    - Dedicated visual art is not complete for all of them, but visual polish is intentionally deferred.
  - Normal run timing now follows `LETHE_DESIGN_01_RUN_LOOP.md`:
    - 600s run duration.
    - Gatekeeper schedule 180/340/490/600s.
    - first boss HP 2050.
    - deficit survival 54s.
    - pressure phase spawn interval/pack/cap table.
  - Fast/debug paths retain compressed timing for smoke review.
  - Review-only automatic memory/+5 injection now runs only in fast debug mode, not normal play.
  - Level-up choices now include all six documented run stats: attack speed, damage, area, survival, magnet, echo amp.
  - Internal pools now reuse transient procedural VFX, floating text, damage numbers, and XP orbs.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 0 warnings, 0 errors.
  - Unity MCP:
    - `Assets/Refresh`: success.
    - compile error count `0`.
    - short Play Mode entry reached `isPlaying=true`.
    - console error log count `0`.
    - Play Mode stopped successfully.
  - Remaining work: full manual balance review is still required; enemies/projectiles are not yet pooled.

- Unity v1 Hungry Blades / Kalmuri readability follow-up:
  - User review: `굶주린 칼무리` did not read as a blade swarm, and the user asked whether the other memory VFX actually exist.
  - Inventory check:
    - `_dev/Data` has 8 `MemoryDefinition`, 8 `EchoDefinition`, and 4 `UltimateEchoDefinition` assets.
    - Dedicated sprite VFX is still concentrated on Kalmuri, Blood, and Blood Blade Storm.
    - The remaining memory/echo/ultimate families currently use procedural runtime VFX such as rings, diamonds, crescent shots, and generated shapes, not final dedicated sprite art.
  - Reworked active Hungry Blades into a denser two-ring orbit with 6-14 blade sprites.
  - Added target-local bite blades on Hungry Blades damage ticks.
  - Added explicit Kalmuri echo blade barrages before the existing ring/slash follow-up.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 legacy v0/debug deprecation warnings, 0 errors.
  - Unity MCP:
    - active scene `Dev_Prototype_v1`.
    - compile error count `0`.
    - short Play Mode entry reached `isPlaying=true`.
    - console error log count `0`.
  - Human visual review is still needed because the run starts behind the weapon-select overlay.

- Unity v1 30-minute runtime stability pass:
  - Scanned `V1GameManager` for remaining enemy-list enumeration hazards after the Blood Bloom collection-modified fix.
  - Added two defensive null guards:
    - Hungry Blades target selection now ignores null enemy entries.
    - Enemy-cap counting now ignores null enemy entries.
  - Final `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 0 warnings, 0 errors.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed after rerun.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.
  - Pre-patch short Unity Play Mode smoke during this pass showed no runtime exceptions after clearing console.
  - Post-patch Unity MCP recheck was blocked because AnkleBreaker MCP tool calls returned `Transport closed`; Unity Editor itself was still listening on port `7890`.

- Unity v1 E/F/G first content expansion pass:
  - Added first-pass runtime effects for remaining active memories:
    - Execution Flash.
    - Hunter Oath.
    - Shatter Wave.
    - Stopped Second.
    - Ashen Shield.
    - Oblivion Brand.
  - Added first-pass weapon-hit echo reactions for non-Kalmuri/Blood echoes.
  - Added three additional minimal ultimate runtime branches:
    - Shatter Wave + Execution Flash = Fracture Execution.
    - Stopped Second + Hunter Oath = Stasis Hunt.
    - Ashen Shield + Oblivion Brand = Ashen Oblivion.
  - HUD now reports the currently ready ultimate through `UltimateReadyName()`/`UltimateGoalText()`.
  - Reward cards can now surface a rotating missing memory candidate.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 0 warnings, 0 errors.
  - `npm.cmd run report`: passed.
  - `npm.cmd run report:check`: passed, 4 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.
  - Unity MCP tools were not exposed in the current tool list, so visual Play Mode confirmation is still pending.

- Unity v1 data asset expansion pass:
  - Added `_dev/Data` ScriptableObject skeleton assets for the complete prototype content set:
    - 8 `MemoryDefinition` assets.
    - 8 `EchoDefinition` assets.
    - 4 `UltimateEchoDefinition` assets under `_dev/Data/Ultimates`.
  - Asset count check:
    - `Memory_*.asset`: 8.
    - `Echo_*.asset`: 8.
    - `Ultimate_*.asset`: 4.
  - Latest `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 legacy v0/debug deprecation warnings, 0 errors.
  - `npm.cmd run report`: passed, 5 unit reports generated.
  - `npm.cmd run report:check`: passed, 5 unit headings ok.
  - `npm.cmd run report:orchestrator:unit:dry`: failed with `fetch failed`.
  - Unity MCP verification:
    - selected single LETHE instance on port `7890`.
    - `Assets/Refresh`: success.
    - `unity_get_compilation_errors`: count `0`.
    - `unity_asset_list(Assets/_dev/Data)`: confirms 8 `MemoryDefinition`, 8 `EchoDefinition`, 4 `UltimateEchoDefinition`.
    - `Dev_Prototype_v1` short Play Mode smoke: no console errors.
    - evidence: `LETHE/Assets/_dev/Evidence/v1_content_data_asset_play_smoke_20260617.png`.

- Unity v1 runtime exception QA:
  - Found console exception: `InvalidOperationException: Collection was modified; enumeration operation may not execute.`
  - Stack trace pointed to `V1GameManager.BloodBloom` while `DealDamage` could remove enemies during `foreach`.
  - Fixed `enemies` area-effect loops to enumerate snapshot lists with `.ToList()` and added null guards to unsafe enemy queries.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 legacy v0/debug deprecation warnings, 0 errors.
  - Unity MCP `Assets/Refresh`: success.
  - `unity_get_compilation_errors`: count `0`.
  - Short Play Mode smoke after clearing console: no runtime exceptions; only AB-MCP startup logs appeared.

- Unity v1 C-step real M2 loop readability first pass:
  - Added an M2 objective/status line to the HUD.
  - The HUD now exposes whether the player is filling memory slots, approaching the next forget candidate, surviving the deficit window, waiting for resonance, or ready for Blood Blade Storm.
  - Level-up choices can now offer `멈춘 1초` as the third active memory after Blood Reflection is acquired.
  - This reduces reliance on automatic review injection for reaching three active memory slots.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors.
  - Unity MCP tools were not exposed in the current tool list, so visual Play Mode confirmation is still pending.

- Unity v1 B-step hit feel / echo readability first pass:
  - Kalmuri echo follow-up now spawns a target-local `KalmuriEchoRange` ring before damage resolution.
  - Blood echo now rewards hitting already marked enemies with a visible red heal thread back to the player and a small heal.
  - Blood Bloom also emits a heal thread, making the +5 blood effect read less like a plain damage burst.
  - Enemy knockback cap increased from `4.6` to `6.2` so greatsword feedback is not compressed as hard.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors.
  - Unity MCP tools were not exposed in the current tool list, so visual Play Mode confirmation is still pending.

- Unity v1 core prototype EPIC / data contract pass:
  - Started `Dev_Prototype_v1 Core Prototype Complete` as the active A-I work package.
  - Added root development-doc entry files so the project has a clean AI/project-management front door:
    - `docs/PRD.md`
    - `docs/TECH.md`
    - `docs/TASK.md`
    - `docs/TEST.md`
    - `docs/CHANGELOG.md`
  - Updated `AGENTS.md` meaningful-work read order so these root docs are read before orchestration state files.
  - Rewrote `docs/orchestration/state/NEXT_TASKS.md` to remove broken Korean text and align it with A-I work.
  - Expanded `_dev` definition contracts in `LETHE/Assets/_dev/Scripts/Core/DefinitionTypes.cs`:
    - `MemoryEffectKind`
    - `EchoFormKind`
    - `EnemyRole`
    - `EncounterSpawnMode`
    - `UltimateTriggerMode`
    - `EncounterWaveData`
    - `UltimateEchoDefinition`
    - `EncounterDefinition`
    - richer `MemoryDefinition`, `EchoDefinition`, `EchoSynergyDefinition`, `EnemyDefinition`, and `RewardPoolDefinition` fields.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 warnings, 0 errors.
  - Unity MCP tools were not exposed in the current tool list, so Unity Editor compile/play-mode verification still needs to be run once MCP is visible again.

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

1. Use `docs/orchestration/review_prompts/2026-06-29-jaewoo-beta-run-review.md` for jaewoo direct play of the updated 20-minute `Dev_Prototype_v1` balance line.
2. Review checklist:
   - 시작 무기 카드 2개가 명확하게 읽히는가?
   - 첫 보상은 24~30초 전후에 와서 너무 빠르지도 느리지도 않은가?
   - 120초 시점 레벨 3~4가 답답하지 않은가?
   - 첫 문지기 300초와 첫 망각 5분대가 성취/상실 리듬으로 읽히는가?
   - 600초 시점에 빌드가 충분히 달라졌고 레벨 9~10 전후가 적절한가?
   - 15~16분 궁극 완성이 후반 보상처럼 느껴지는가?
   - 19~20분 최종 문지기 처치가 클리어 목표로 명확한가?
   - 쌍검 기본공격이 적 위치 발도선으로 읽히는가?
   - 적이 맞을 때 넉백/피격/공간 반응이 충분한가?
   - 칼무리 잔향이 기본공격 뒤 후속타로 읽히는가?
   - 대검이 느린 큰 한 방으로 읽히는가?
   - 4개 궁극 루트(`피의 칼폭풍`, `파쇄 처형`, `정지 추적`, `잿빛 망각`) 중 특정 루트만 압도적으로 쉽거나 막히지 않는가?
   - 순수 시뮬레이션에서 낮게 나온 대검 루트가 실제 플레이에서도 불안한가?
3. Pay special attention to the newly wired generated VFX:
   - Are dual-blade arcs readable as fast paired cuts instead of screen noise?
   - Is the greatsword cleave large enough to feel heavy without hiding enemies?
   - Do Execution/Hunter/Shatter/Stopped/Ashen/Brand memories and echoes appear at natural target/player positions?
   - Are Fracture Execution, Stasis Hunt, and Ashen Oblivion distinct enough from normal echo bursts?
4. After feedback, choose one narrow next pass: XP cadence, Gatekeeper HP, weapon route balance, reward route steering, VFX scale/timing, or enemy pressure.

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
# 2026-06-29 Update: Stepped Boss Runtime Applied

The stepped boss / XP / DPS candidate is now applied in Unity `Dev_Prototype_v1`.

Applied runtime values:

- Gatekeeper schedule: `150 / 360 / 660 / 1020s`.
- Gatekeeper intervals: `150 / 210 / 300 / 360s`.
- Gatekeeper HP: `1200 / 2250 / 4050 / 8650`.
- Hard cap: `1200s`.
- Initial XP requirement: `8`.
- Normal-run deficit survival: removed as a timed `54s` pocket.

Normal Gatekeeper clear now still forgets the highest memory and grants the echo, but Space moves directly to immediate memory refill/resonance instead of a timed deficit survival phase.

Verification:

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors.
- `node scripts/balance_curve_v1.js`: passed.
- `node scripts/verify_unity_stepped_balance.js`: passed.
- Unity MCP compile error count: `0`.
- Unity MCP console error count: `0`.

Current limitation: Unity MCP `unity_execute_menu_item` repeatedly returned `Error polling queue: fetch failed` during V1 smoke menu attempts, so the explicit V1QA menu line needs a retry before final handoff.

Current next step: retry V1QA smoke when MCP menu execution is stable, then run a focused first-6-minute jaewoo review.

# 2026-06-29 Update: Stepped Boss Balance Candidate

Jaewoo direct review changed the immediate balance direction:

- The first Gatekeeper at `300s` is too late and makes the early run feel boring.
- Boss intervals should grow step by step, and difficulty should rise through enemy count, enemy HP, boss HP, and expected player DPS.
- The separate deficit survival pocket is likely unnecessary for the current prototype.

New calculated candidate:

- Gatekeeper schedule: `150 / 360 / 660 / 1020s`.
- Gatekeeper intervals: `150 / 210 / 300 / 360s`.
- Gatekeeper HP: `1200 / 2250 / 4050 / 8650`.
- Target boss TTK: `18 / 26 / 36 / 48s`.
- Hard cap: `1200s`.
- Deficit survival: remove as a separate `54s` timer; keep forgetting and echo grant, then return to the normal combat/reward flow.

Evidence and formula source: `scripts/balance_curve_v1.js` and `docs/orchestration/evidence/2026-06-29-stepped-boss-xp-dps-plan.md`.

Current next step: implement this single pacing axis in Unity `_dev`, then rerun technical QA and a focused first-6-minute jaewoo review.
