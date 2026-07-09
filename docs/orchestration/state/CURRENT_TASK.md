# Current Task

# 2026-07-09 Greatsword Blood Echo Crescent Follow-up

## Status

- Implemented and verified.

## Applied Changes

- Replaced the Greatsword Blood Echo harvest/thread feel with a red crescent follow-up slash.
- Added `TriggerGreatswordBloodIaido`:
  - uses Greatsword cleave arc geometry,
  - spawns red crescent, afterimage, edge, impact-zone, and cut-line VFX,
  - damages enemies around the crescent impact zone,
  - applies Blood Mark and adds hitstop/camera shake.
- Reduced Dense Dual Blades utility routing by skipping the heaviest execution branch in dense mode after one frame-budget regression.

## Verification

- Unity compilation errors: `0`.
- `LETHE/V1 QA/Echo Matrix Greatsword`: PASS, `total=374`, `K=8`, `B=33`, `Ex=64`, `H=22`, `Sh=56`, `St=16`, `A=80`, `O=95`, `state=49`.
- `LETHE/V1 QA/Dense Dual Blades Perf Matrix`: PASS after dense routing reduction, `hits=18`, `suppressed=15`, `transient=98`, `activeVfx=73`, `ms=74.56`.

## Remaining Gate

- Direct play should confirm whether Greatsword Blood Echo now feels like a blood iaido / red half-moon Greatsword follow-up.

# 2026-07-09 Utility Echo Weapon Mechanics Correction

## Status

- Implemented and verified.
- This corrects the previous utility Echo identity pass, which still felt too similar between weapons.

## Applied Changes

- Added shared utility target helpers for radius, cone, and short chain targeting.
- Reworked utility Echo weapon behavior:
  - Blood: Greatsword forward harvest arc and healing threads; Dual Blades stitch chain.
  - Shatter: Greatsword forward fissure/cone; Dual Blades needle bounce chain.
  - Execution: Greatsword verdict cleave; Dual Blades sentence chain.
  - Hunter: Greatsword piercing spear line; Dual Blades fan shots and mark bites.
  - Stopped: Greatsword clock-field freeze; Dual Blades micro-stop chain.
  - Ashen: Greatsword bulwark/wave; Dual Blades enemy-side parry chain.
  - Oblivion: Greatsword collapse well; Dual Blades brand stack hops.
- Dense Dual Blades now suppresses Shatter/Execution/Ashen chains into one-target branches after an intermediate performance fail.

## Verification

- Unity compilation errors: `0`.
- `LETHE/V1 QA/Echo Matrix Dual Blades`: PASS, `total=404`, `K=8`, `B=53`, `Ex=99`, `H=64`, `Sh=45`, `St=16`, `A=54`, `O=65`, `state=82`.
- `LETHE/V1 QA/Echo Matrix Greatsword`: PASS, `total=376`, `K=8`, `B=34`, `Ex=64`, `H=22`, `Sh=56`, `St=16`, `A=80`, `O=96`, `state=52`.
- `LETHE/V1 QA/Dense Dual Blades Perf Matrix`: PASS after dense correction, `hits=18`, `suppressed=15`, `transient=109`, `activeVfx=75`, `ms=91.01`.

## Remaining Gate

- Direct play should judge whether each utility Echo now feels meaningfully different by weapon. If not, the next move is deeper concept redesign per Echo family.

# 2026-07-09 Utility Echo Weapon-Identity VFX / Judgment Pass

## Status

- Implemented and verified.
- Scope stayed in `Assets/_dev`.

## Applied Changes

- Expanded non-Kalmuri utility Echoes so Greatsword and Dual Blades no longer share the same read:
  - Blood Echo: Greatsword heavy drain-axis, Dual Blades quick suture cuts.
  - Execution Echo: Greatsword guillotine verdict, Dual Blades repeated chain/pip execution marks.
  - Hunter Echo: Greatsword spear-shadow pursuit, Dual Blades fan needles and target pips.
  - Shatter Echo: Greatsword fault-line fracture, Dual Blades needle ripples outside dense branch.
  - Stopped Second Echo: Greatsword clock-hand cleave, Dual Blades tick-cut micro-pause.
  - Ashen Shield Echo: Greatsword cracked bulwark impact, Dual Blades parry sparks outside dense branch.
  - Oblivion Brand Echo: Greatsword collapse ring, Dual Blades stack ring.
- Adjusted utility Echo tuning so weaker/passive memories have stronger Echo payoff:
  - HunterOath proc and heavy-target payoff increased.
  - ShatterWave radius/heavy damage/control space improved.
  - StoppedSecond freeze window and heavy payoff improved.
  - AshenShield proc/radius/damage improved.
  - OblivionBrand proc/radius/heavy payoff improved.
- Kept Dense Dual Blades budget safe by suppressing the most expensive secondary extras in dense branches.

## Verification

- Unity compilation errors: `0`.
- `LETHE/V1 QA/Echo Matrix Dual Blades`: PASS, `total=363`, `K=8`, `B=40`, `Ex=88`, `H=64`, `Sh=32`, `St=16`, `A=56`, `O=59`, `state=91`.
- `LETHE/V1 QA/Echo Matrix Greatsword`: PASS, `total=316`, `K=8`, `B=8`, `Ex=64`, `H=20`, `Sh=48`, `St=16`, `A=80`, `O=72`, `state=56`.
- `LETHE/V1 QA/Dense Dual Blades Perf Matrix`: PASS, `hits=18`, `suppressed=15`, `transient=104`, `activeVfx=76`, `ms=94.36`.
- `LETHE/V1 QA/Passive Memory Matrix`: PASS, `blood=17`, `ash=6`, `stopped=8`, `oblivion=64`.
- `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`: PASS, `ultPrefix=UltDual_`, `fracture=28`, `stasis=11`, `ashen=47`.
- `LETHE/V1 QA/Utility Ultimate Matrix Greatsword`: PASS, `ultPrefix=UltGreat_`, `fracture=49`, `stasis=22`, `ashen=14`.

## Remaining Gate

- Direct-play review is still needed. Automated QA proves coverage, compilation, and budget; jaewoo should judge whether each Echo family now has enough personality and whether passive memory growth paths feel worth choosing.

# 2026-07-09 Kalmuri Convergence Timing and Dual Blades Visibility Pass

## Status

Implemented, Unity-compiled, and dense/Echo QA passed. Awaiting jaewoo direct-play feel review.

## Applied Changes

- Greatsword Kalmuri Echo now shows a slower ring-edge convergence:
  - blade pulls start farther out from the circle,
  - blue edge trails sweep inward more slowly,
  - upper/lower blade jaws close more slowly,
  - wake/rift/jaw afterimages live longer.
- Dual Blades Kalmuri Echo gained clearer normal-pack visibility:
  - larger non-dense blue rift/core/pulse,
  - two short foreground blade glints,
  - stronger blue read without changing the dense-object budget.
- Dense Dual Blades uses lighter rift/core/pulse values after an intermediate run exceeded the `110ms` budget.

## Verification

- Unity compilation errors: `0`.
- `LETHE/V1 QA/Dense Dual Blades Perf Matrix`: PASS, `hits=18`, `suppressed=12`, `transient=118`, `activeVfx=73`, `ms=87.43`.
- `LETHE/V1 QA/Echo Matrix Dual Blades`: PASS, `total=229`, `K=8`, `state=86`.
- `LETHE/V1 QA/Echo Matrix Greatsword`: PASS, `total=231`, `K=8`, `state=58`.
- `LETHE/V1 QA/Kalmuri Perf Matrix`: PASS; meaningful same-pass visual-count run logged `totalKalmuri=396`, final post-QA runner snapshots logged `totalKalmuri=0`.

## Remaining Gate

- Direct-play Greatsword Hungry Blades +5:
  - Do the blades visibly gather from the perimeter?
  - Is it slower enough without feeling sluggish?
- Direct-play Dual Blades Hungry Blades +5:
  - Is the blue Echo now visible in normal packs?
  - Does dense combat stay readable rather than noisy?

# 2026-07-09 Weapon-Specific Echo VFX Readability Pass

## Status

Implemented, Unity-compiled, and core Echo/Kalmuri QA passed. Awaiting jaewoo direct-play feel review.

## Applied Changes

- Kalmuri, Echo, and Ultimate transient VFX now sort above ordinary weapon slash VFX.
- Kalmuri Echo visibility was increased:
  - larger blue rifts,
  - brighter blade-pull trails,
  - larger Kalmuri blade cores,
  - visible blue pulse/scar cues even in Dense Dual Blades.
- Greatsword Echoes now read bigger and heavier:
  - Blood: wider cleave pool and wound slash.
  - Execution: larger stamp/halo and more verdict cracks.
  - Hunter: larger spear lock and heavier spear projectile.
  - Shatter: larger fracture tell, field, and crack lines.
  - Stopped: larger dome/clamp/clock field.
  - Ashen: larger guard seal and stored counter wave.
  - Oblivion: larger detonation seal, core break, and burst.
- Dual Blades Echoes now read slightly larger without turning into Greatsword:
  - brighter stacked cuts,
  - clearer short links,
  - slightly bigger repeated marks,
  - Kalmuri pulse/scar cues that survive over weapon slash VFX.

## Verification

- Unity compilation errors: `0`.
- `LETHE/V1 QA/Kalmuri Perf Matrix`: PASS, `totalKalmuri=420`.
- `LETHE/V1 QA/Dense Dual Blades Perf Matrix`: PASS, `hits=18`, `suppressed=15`, `transient=156`, `activeVfx=73`, `ms=99.50`.
- `LETHE/V1 QA/Echo Matrix Dual Blades`: PASS, `total=232`, `K=8`, `state=87`.
- `LETHE/V1 QA/Echo Matrix Greatsword`: PASS, `total=231`, `K=8`, `state=59`.

## Remaining Gate

Direct-play with both weapons:

- Does Greatsword Echo now feel large and heavy enough?
- Does Dual Blades Echo remain readable without drowning the screen?
- Does Kalmuri stay visible through Dual Blades slash VFX?
- If one family still feels weak, tune that family only rather than globally scaling every Echo again.

# 2026-07-09 LETHE Intro Weapon Selection Screen

## Status

Implemented, Unity-compiled, and state-flow checked. Awaiting jaewoo direct visual review.

## Applied Changes

- Added a new LETHE-style first screen in `V1GameManager`.
- The intro uses weapon selection rather than a generic click-to-start:
  - `절단쌍검`: fast paired cuts, frequent echo rhythm.
  - `장송대검`: slower heavy swings, larger echo rhythm.
- The screen now has:
  - dark river / lower-water background bands,
  - cyan memory drift lines,
  - small memory-shard glints,
  - title/goal copy,
  - two large clickable weapon cards.
- Existing start controls remain:
  - `1` starts Dual Blades,
  - `2` starts Greatsword,
  - clicking the matching card also starts the run.

## Verification

- Unity compilation errors: `0`.
- Play Mode initial state:
  - `weaponSelectOverlay=True`,
  - `runStarted=False`,
  - `GameplayPaused=True`.
- Direct selection call:
  - `weaponSelectOverlay=False`,
  - `runStarted=True`,
  - `GameplayPaused=False`.
- `LETHE/V1 QA/Start Dual Blades`: invoked successfully.
- `LETHE/V1 QA/Start Greatsword`: invoked but currently fails with `liveEnemies=2`; treat this as a start-smoke expectation/balance mismatch to fix separately.

## Remaining Gate

Direct-play inspect the intro in Game view:

- Does the first screen feel like LETHE before the run begins?
- Are the two weapon cards readable without feeling like a menu placeholder?
- Does weapon selection feel better than a separate click-to-start page?
- After that, fix the Greatsword start QA mismatch as its own small task.

# 2026-07-09 Kalmuri Blue Memory-Lineage VFX Pass

## Status

Implemented, Unity-compiled, and core QA passed. Awaiting jaewoo direct-play feel review.

## Applied Changes

- Reworked the default Kalmuri Echo palette after jaewoo clarified the desired emotion:
  - the original memory was blue Hungry Blades,
  - so the Echo should feel like that blue blade memory remaining and becoming more detailed.
- Removed the red/orange wound-pool read from Dual Blades default Kalmuri.
- Replaced generic inward tooth/bite sprites with actual Kalmuri blade sprites.
- Dual Blades now emphasizes:
  - quick blue rifts,
  - spectral blade pulls,
  - small blue blade-bite marks.
- Greatsword now emphasizes:
  - blue memory pool,
  - blue blade-wake ring,
  - heavy spectral blade-jaw closure,
  - blue splinter scars.
- +5 awakened Kalmuri now uses blue spectral blade devour pulls.

## Verification

- Unity compilation errors: `0`.
- `LETHE/V1 QA/Kalmuri Perf Matrix`: PASS, `orbit=44`, `bite=72`, `return=24`, `hunting=16`, `echoSurge=0`, `echoBarrage=0`, `totalKalmuri=356`.
- `LETHE/V1 QA/Dense Dual Blades Perf Matrix`: PASS, `hits=18`, `suppressed=15`, `transient=120`, `activeVfx=58`, `ms=86.60`.
- `LETHE/V1 QA/Echo Matrix Dual Blades`: PASS, `total=232`, `K=8`, `state=86`.
- `LETHE/V1 QA/Echo Matrix Greatsword`: PASS, `total=207`, `K=8`, `state=58`.

## Remaining Gate

Direct-play with F12 Kalmuri prototype mode off. Check whether the default Kalmuri Echo now reads as a polished blue Hungry Blades Echo, not a red blood/wound effect.

# 2026-07-09 Default Kalmuri Hunger Echo Runtime

## Status

Implemented, Unity-compiled, and core QA passed. Awaiting jaewoo direct-play feel review.

## Applied Changes

- Converted the default Kalmuri Echo runtime away from the old detached flying-blade read.
- Default Kalmuri now uses a K2/K1 hybrid:
  - weapon-trail scent pull,
  - wound opening,
  - inward teeth,
  - devouring bite/scar closure.
- Dual Blades variant:
  - many fast small inward teeth,
  - short gnaw scars,
  - quick pack-bite secondary marks.
- Greatsword variant:
  - larger wound pool and scent ring,
  - heavy jaw closure,
  - drawn-to-wound trails,
  - splinter scars.
- +5 awakened Kalmuri in default mode now spawns a wound-side devour bloom instead of calling the old `LaunchKalmuriBlade` projectile.
- F12 K1-K4 prototype review modes remain available, but default mode is now the new hunger-fit runtime.

## Verification

- Unity compilation errors: `0`.
- `LETHE/V1 QA/Kalmuri Perf Matrix`: PASS, `orbit=44`, `bite=72`, `return=24`, `hunting=16`, `echoSurge=0`, `echoBarrage=0`, `totalKalmuri=340`.
- `LETHE/V1 QA/Dense Dual Blades Perf Matrix`: PASS, `hits=18`, `suppressed=15`, `transient=134`, `activeVfx=52`, `ms=8.56`.
- `LETHE/V1 QA/Echo Matrix Dual Blades`: PASS, `total=231`, `K=8`, `state=87`.
- `LETHE/V1 QA/Echo Matrix Greatsword`: PASS, `total=207`, `K=8`, `state=58`.

## Remaining Gate

Direct-play with F12 Kalmuri prototype mode off. Check whether the new default Kalmuri reads as hungry blades eating the wound, whether Dual Blades feels fast but readable, and whether Greatsword feels heavy enough.

# 2026-07-08 Dual-Blade Kalmuri Red-Circle Read Fix

## Status

Implemented and build/Unity-error checked.

## Applied Changes

- Removed or suppressed large red circle/ring reads from Dual Blades Kalmuri candidates.
- Dual Blades now favors short bite marks, wound slashes, tooth snaps, and small shard marks instead of readable red circular markers.
- Greatsword keeps larger wound/table/furrow shapes where big impact silhouettes make sense.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with existing legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors on sequential rerun.
- Unity compilation errors: `0`.
- Unity console errors: `0`.

## Remaining Gate

Direct-play Dual Blades K1-K4 again and check whether the effect now reads as fast bite marks rather than red circles.

# 2026-07-08 Kalmuri Hunger-Fit Candidate Rebuild

## Status

Implemented, build-verified, and Unity console/QA checked.

## Applied Changes

- Rebuilt the Kalmuri K1-K4 candidates around the actual `굶주린 칼무리` image instead of generic different-looking VFX.
- New candidate meanings:
  - `K1`: wound feast / bite swarm. The wound opens like a mouth and teeth close inward.
  - `K2`: blood-scent hunt. The first wound becomes a scent beacon and nearby blade echoes are pulled toward it.
  - `K3`: feast table. Blades/teeth set a circular table around the wound and fold inward.
  - `K4`: chewed trail. The weapon path leaves repeated bite marks and a chewed furrow.
- Removed the previous ribbon/cross/curse conceptual reads from the candidate labels and main VFX bodies.
- Kept weapon personality:
  - Dual Blades: more small bite marks, pack behavior, fast closures.
  - Greatsword: larger wound, heavier bite/furrow, fewer bigger closures.
- Updated hit rules:
  - `K2`: radial scent pull.
  - `K3`: circular feast area.
  - `K4`: forward chewed path.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with existing legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors on sequential rerun after an initial shared-DLL lock.
- Unity compilation errors: `0`.
- Unity console errors: `0`.
- `LETHE/V1 QA/Kalmuri Perf Matrix`: PASS, `totalKalmuri=268` from Unity console.

## Remaining Gate

jaewoo should direct-play `K1` to `K4` again with Dual Blades and Greatsword. This review should judge whether any candidate finally reads as `굶주린 칼무리의 잔향`; if not, discard all four and design the next set from the wound/scent/feast/trail foundation.

# 2026-07-08 Kalmuri Prototype Legacy VFX Suppression

## Status

Implemented and build/Unity-error checked.

## Applied Changes

- Suppressed the old +5 `LaunchKalmuriBlade` awakened projectile while an F12 Kalmuri prototype is active.
- This prevents a selected K1-K4 candidate from being polluted by the previous flying-blade VFX.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with existing legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors on sequential rerun.
- Unity compilation errors: `0`.
- Unity console errors: `0`.

## Remaining Gate

Direct-play K1-K4 again and confirm no old awakened flying blade appears on top of the selected prototype.

# 2026-07-08 Kalmuri Echo Playable Prototype Correction

## Status

Implemented after jaewoo clarified that the goal is not cosmetic preview VFX, but four playable Hungry Blades Echo concept candidates with VFX, hit rules, and weapon fit.

## Correction

- Previous K1-K4 work was too shallow: it changed the F12 preview visuals but did not make the real Hungry Blades Echo feel different in play.
- The actual need is:
  - design the forgotten `굶주린 칼무리` memory as a fun Echo,
  - make Dual Blades and Greatsword both feel appropriate,
  - provide four playable examples so jaewoo can choose a direction,
  - include VFX, hit area, damage application, hitstop/camera feel, and readable concept identity.

## Applied Changes

- F12 `K1` to `K4` now select a real Kalmuri Echo prototype mode, not only a preview animation.
- After selecting a K candidate, weapon hits route `TriggerKalmuriEcho` into that selected prototype.
- Candidate hit rules now differ:
  - `K1`: wound-mouth / maw bite area.
  - `K2`: weapon-trail ribbon strip.
  - `K3`: geometric X/cross burst area.
  - `K4`: curse-mark chain/network transfer area.
- The debug panel now shows the active prototype: `K real echo prototype: K#`.
- Preview-only 1 damage was separated from real Echo damage so playable prototypes use weapon/echo scaling.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 existing legacy warnings and 0 errors; one shared-DLL retry warning.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors on sequential rerun after an initial shared-DLL lock.
- Unity compilation errors: `0`.
- Unity console errors: `0`.
- `LETHE/V1 QA/Kalmuri Perf Matrix`: PASS, `totalKalmuri=268`.

## Remaining Gate

Direct-play review should now press `F12`, choose `K1` to `K4`, then attack enemies with Dual Blades and Greatsword. The choice should be based on whether the memory-to-echo transformation feels hungry, readable, and fun in actual combat, not only whether the still-frame VFX looks different.

# 2026-07-08 Kalmuri VFX Hard Reset Result

## Status

Implemented, locally build-verified, and Unity QA checked.

## Applied Changes

- Rebuilt the F12 `K1` to `K4` Kalmuri preview candidates as hard-reset silhouettes.
- `K1` now reads as wound mouth / saw-tooth scar.
- `K2` now reads as ribbon trail / burial banner.
- `K3` now reads as geometric cross/X burst.
- `K4` now reads as curse-mark seal plus chain/fork network.
- Dual Blades and Greatsword now show different variants for all four candidates:
  - Dual Blades: faster, thinner, multi-hit shapes.
  - Greatsword: heavier, broader, more singular impact shapes.
- The preview pack label is now `K Preview HARD RESET / HP 9999`.
- The hard-reset preview helpers no longer use the old `KalmuriConcept_*` naming and avoid the old flying-blade preview silhouette.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 existing legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors on sequential rerun after an initial shared-DLL lock.
- Unity compilation errors: `0`.
- Unity console errors: `0`.
- `LETHE/V1 QA/Kalmuri Perf Matrix`: PASS, `totalKalmuri=268`.

## Remaining Gate

jaewoo should now direct-play `Dev_Prototype_v1`, press `F12`, compare `K1` to `K4` with both Dual Blades and Greatsword, and pick one winner or hybrid. After that Codex should convert the chosen visual grammar into the actual Kalmuri Echo behavior.

# 2026-07-08 Kalmuri VFX Hard Reset Handoff

## Status

Planning / next-session handoff. Do this at home before more Kalmuri tuning.

## Finding

- jaewoo reported that Kalmuri preview VFX still appears unchanged.
- Current problem is not dummy HP anymore.
- Current problem is that the preview still reuses too much of the existing Kalmuri visual language:
  - existing blade sprite,
  - existing cyan/echo slash habits,
  - target-centered short bursts,
  - old Kalmuri helper paths.
- Therefore the next step should not be another small color/lifetime tweak.

## Required Direction

Hard reset Kalmuri VFX preview and then the real Kalmuri Echo.

- Remove the current `K1` to `K4` preview implementation as a selection tool.
- Do not use `KalmuriBladeSprite()` for the new preview candidates.
- Do not reuse the existing `KalmuriEchoClampBlade`, `KalmuriEchoBarrage`, `KalmuriEchoSurgeBlade`, `KalmuriAwakenBlade`, or wound-chain projectile look for the new candidates.
- Create new silhouettes that cannot be mistaken for the current Kalmuri:
  - `K1`: wound mouth / saw-tooth scar, not flying blades.
  - `K2`: long ribbon/afterimage trail, not separate blade sprites.
  - `K3`: large geometric cross-cut burst, not orbit blades.
  - `K4`: curse-mark network / chained seals, not more knives.
- Keep high-HP preview dummies and v2 label or replace label with `K Preview HARD RESET`.

## Next Implementation Checklist

1. In `V1GameManager.cs`, isolate or delete the current Kalmuri preview helpers:
   - `DebugPreviewKalmuriWoundFeast`
   - `DebugPreviewKalmuriTrailBloom`
   - `DebugPreviewKalmuriCrossSwarm`
   - `DebugPreviewKalmuriMarkFrenzy`
2. Build new preview helpers from generated primitive sprites or brand-new assets:
   - discs, jagged lines, sectors, ribbons, seals, cracks, chains;
   - avoid the existing Kalmuri blade sprite entirely.
3. Make each candidate last at least `0.8s` for visual inspection.
4. Make the hit area visibly readable:
   - K1: circular wound area.
   - K2: long weapon trail strip.
   - K3: cross/X area.
   - K4: marked target plus linked secondary targets.
5. After a candidate is approved, only then convert it into real Kalmuri Echo behavior.

## Remaining Gate

At home, start from this task. Do not spend more time trying to polish the current K1-K4 if it still looks the same; replace the visual system.

# 2026-07-08 Kalmuri Preview High-HP Dummies

## Status

Implemented, locally build-verified, and Unity compile/console checked.

## Finding

- jaewoo could not tell whether the preview update was applied.
- K-preview enemies also died too quickly, making the VFX hard to inspect.

## Applied Changes

- K-preview dummy enemies now spawn with HP `9999`.
- K-preview damage is capped to `1`.
- Pressing `K1` to `K4` shows `K Preview v2 / high HP dummies` above the pack.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 existing legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 7 existing legacy warnings and 0 errors.
- Unity compilation errors: `0`.
- Unity console errors: `0`.

## Remaining Gate

Direct play should press `F12`, then `K1` to `K4`. If the `K Preview v2 / high HP dummies` label does not appear, exit/re-enter Play Mode.

# 2026-07-08 Kalmuri Concept Preview Readability Split

## Status

Implemented, locally build-verified, and Unity compile/console checked.

## Finding

- The first `K1` to `K4` debug preview set still looked too similar because all four used similar cyan Kalmuri blade sprites, short lifetimes, and nearby target-centered motion.
- As a selection tool, that was not useful enough: jaewoo could not tell what was actually different.

## Applied Changes

- `K1` wound-feast:
  - red/orange wound disc,
  - bite ring,
  - inward teeth,
  - scar cuts.
- `K2` trail-bloom:
  - blue attack-ribbon afterimages,
  - longer trail scars,
  - delayed rip line.
- `K3` cross-swarm:
  - purple/white radial inward cuts,
  - large X impact,
  - brighter center flash.
- `K4` mark-frenzy:
  - violet seal/ring,
  - fork links to nearby enemies,
  - secondary seals instead of extra flying blades.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 existing legacy warnings and 0 errors; a parallel run briefly hit a shared DLL lock, then sequential rerun confirmed no code errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors on sequential rerun.
- Unity compilation errors: `0`.
- Unity console errors: `0`.

## Remaining Gate

Direct play should press `F12`, compare the revised `K1` through `K4`, and choose a winner or hybrid.

# 2026-07-08 Kalmuri Concept Preview Debugger

## Status

Implemented and locally build-verified. Unity console shows no errors, but the editor compilation flag stayed `true` longer than expected after refresh, so direct button click verification is pending.

## Goal

Give jaewoo a fast way to compare multiple Kalmuri Echo concepts before choosing the final implementation.

## Applied Changes

- Added F12 debug panel buttons:
  - `K1`: wound-feast, multiple blades collapse into the hit wound.
  - `K2`: trail-bloom, attack trail afterimages multiply into delayed cuts.
  - `K3`: cross-swarm, blades spawn around the target and cross-cut inward.
  - `K4`: mark-frenzy, a hungry scar mark forks into nearby enemies.
- Each preview clears/rebuilds the same small enemy pack in front of the player so the concepts can be compared under the same layout.
- The previews are intentionally debug samples, not final balance.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors on sequential rerun.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 7 existing legacy warnings and 0 errors.
- Unity console errors: `0`.
- Unity `EditorApplication.isCompiling`: remained `true` after refresh; direct click test pending.

## Remaining Gate

Direct play should press `F12`, compare `K1` through `K4`, and choose which visual grammar should become the real Kalmuri Echo.

# 2026-07-08 Kalmuri Wound-Reaction Correction

## Status

Implemented, locally build-verified, and Unity-QA-verified through AnkleBreaker Unity MCP on `LETHE` port `7890`.

## Finding

- jaewoo correctly identified that the awakened Kalmuri Echo concept was wrong when it read as "the attack already went out, then one more blade leaves the player body."
- Kalmuri is an echo, so the satisfying interaction should happen where the weapon hit lands: wound, scar, chain, bite, or rip at the enemy/attack point.

## Applied Changes

- Awakened Kalmuri wound-chain now starts from the struck enemy/wound point.
- Added wound burst, wound scar, chain line, and wound-chain projectile behavior so the echo reads as attack-point interaction.
- Dense Dual Blades suppresses the expensive awakened wound-chain follow-up while preserving normal Kalmuri echo hits, preventing a dense perf regression.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 existing legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors after sequential rerun.
- Unity compilation errors: `0`.
- `LETHE/V1 QA/Echo Matrix Dual Blades`: PASS, `total=230`, `K=8`, `state=86`.
- `LETHE/V1 QA/Echo Matrix Greatsword`: PASS, `total=335`, `K=136`, `state=58`.
- `LETHE/V1 QA/Dense Dual Blades Perf Matrix`: PASS, `hits=18`, `suppressed=15`, `transient=87`, `activeVfx=25`, `ms=80.59`.

## Remaining Gate

Direct-play review should judge whether +5 Kalmuri now feels like a wound-side echo reaction rather than an extra detached projectile.

# 2026-07-08 Memory/Echo Kingmaker VFX and Judgment Pass

## Status

Implemented, locally build-verified, and Unity-QA-verified through AnkleBreaker Unity MCP on `LETHE` port `7890`.

## Goal

Turn the approved memory/echo/ultimate reward design into playable VFX and hitbox behavior, with special attention to routes that previously felt weaker than Blood Reflection.

## Applied Changes

- Weapon identity follow-up:
  - Split Greatsword Kalmuri Echo out of the shared Kalmuri clamp/bite follow-up.
  - Dual Blades Kalmuri Echo remains a fast swarm/bite pattern.
  - Greatsword Kalmuri Echo now uses a heavy falling judgement blade, drop line, ground rip, execution rift, and impact core.
- ExecutionFlash:
  - Added near-threshold execution forecast VFX before the actual kill window.
  - Fracture Execution now marks both Execution and Oblivion states and deals stronger low-HP payoff damage.
- HunterOath:
  - Added threat-priority targeting so VoidPriests, Gatekeepers, DriftingEyes, and SplitOnes are selected before low-value bodies when possible.
- ShatterWave:
  - Added cluster/boss fracture bonus logic and scar VFX so it rewards dense packs and larger threats.
- StoppedSecond:
  - Added stopped-fracture bursts to memory, echo, and Stasis Hunt ultimate beats.
- AshenShield:
  - Added stored guard charge from prevented damage and echo hits.
  - Added stored guard release waves with radial lines, hitstop, camera shake, and AoE damage.
- OblivionBrand:
  - Added +5 detonation/spread behavior and echo rupture/spread follow-ups.
- Utility ultimates:
  - Buffed Fracture Execution, Stasis Hunt, and Ashen Oblivion damage/VFX so non-blood ultimates have visible payoff.
- Dense QA:
  - Lowered Dense Dual Blades benchmark-only damage so the perf matrix measures dense hit/echo suppression, not kill-chain aftermath.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 existing legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 7 existing legacy warnings and 0 errors.
- Unity compilation errors: `0`.
- `LETHE/V1 QA/VFX Matrix`: PASS, `previewMemory=8`, `previewEcho=8`, `fracture=1`, `stasis=1`, `ashen=1`, `missing=`.
- `LETHE/V1 QA/Echo Matrix Dual Blades`: PASS, `total=226`, `K=8`, `B=35`, `Ex=64`, `H=24`, `Sh=8`, `St=8`, `A=32`, `O=47`, `state=78`.
- `LETHE/V1 QA/Echo Matrix Greatsword`: PASS, `total=207`, `K=8`, `B=9`, `Ex=48`, `H=14`, `Sh=32`, `St=8`, `A=32`, `O=56`, `state=57`.
- `LETHE/V1 QA/Passive Memory Matrix`: PASS, `blood=17`, `ash=6`, `stopped=8`, `oblivion=62`.
- `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`: PASS, `fracture=28`, `stasis=11`, `ashen=47`.
- `LETHE/V1 QA/Utility Ultimate Matrix Greatsword`: PASS, `fracture=49`, `stasis=22`, `ashen=14`.
- `LETHE/V1 QA/Dense Dual Blades Perf Matrix`: PASS, `hits=18`, `suppressed=15`, `transient=45`, `activeVfx=26`, `ms=57.58`.
- Weapon identity follow-up:
  - `LETHE/V1 QA/Echo Matrix Greatsword`: PASS, `total=335`, `K=136`, `state=58`.
  - `LETHE/V1 QA/Echo Matrix Dual Blades`: PASS, `total=230`, `K=8`, `state=85`.
  - `LETHE/V1 QA/Dense Dual Blades Perf Matrix`: PASS, `hits=18`, `suppressed=15`, `transient=94`, `activeVfx=30`, `ms=87.85`.

## Remaining Gate

Direct-play review should judge:

- Whether AshenShield now feels like a defensive route that can become explosive later.
- Whether Execution/Oblivion and Ashen/Oblivion pairings have enough "kingmaker" payoff.
- Whether StoppedSecond and ShatterWave feel like damage-control hybrids instead of only passive utility.
- Whether non-blood ultimates are worth choosing beside Blood Blade Storm.
- Whether dense dual blades still feel responsive after the added VFX.
- Whether Greatsword Kalmuri now feels like a heavy judgement blade instead of the same swarm effect as Dual Blades.

# 2026-07-07 Gatekeeper Review HP / Impact VFX Pass

## Status

Implemented, locally build-verified, and Unity-QA-verified through AnkleBreaker Unity MCP on `LETHE` port `7890`.

## Finding

- The first Gatekeeper felt too weak in direct review because the F6/F12 Boss path was using compressed QA boss HP `180`.
- The real normal-run Gatekeeper HP path is not `180`; it is currently `2200 / 4200 / 7600 / 12800`.
- The boss pattern issue was mainly output strength: red zones existed, but the boss cast body and impact moment were not loud enough.

## Applied Changes

- Added `DebugReviewBossHp = FirstBossHp`.
- Added `debugReviewBossHp` so fast QA can keep HP `180` while F6/F12 Boss review uses HP `2200`.
- Added Gatekeeper cast burst VFX:
  - boss sigil,
  - halo,
  - blade spine,
  - rupture lines,
  - target line.
- Strengthened Gatekeeper meteor:
  - higher falling body,
  - hot fall trail,
  - spawn flare,
  - scorch,
  - impact ring,
  - more debris.
- Strengthened cone/ring resolve:
  - cone afterblade,
  - edge snap lines,
  - ground scars,
  - ring inner flash,
  - crack spokes,
  - stronger shared impact flash/shock/camera shake.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 existing legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors.
- Unity compilation errors: `0`.
- `LETHE/V1 QA/Gatekeeper Pattern Matrix`: `[V1QA] PASS`, `boss=4`, `meteor=20`, `cone=6`, `ring=3`.
- `LETHE/V1 QA/Gatekeeper Jump`: `[V1QA] PASS`, `boss=1`, `liveEnemies=15`.

## Remaining Gate

Direct-play review should judge:

- Whether F6/F12 Boss now survives long enough to inspect the pattern loop.
- Whether meteor feels like something falls and hits, not just a red circle.
- Whether cone/ring impacts feel like a real boss attack with clear damage feedback.

# 2026-07-07 Memory/Echo/Enemy Identity Pass

## Status

Implemented, locally build-verified, and Unity-QA-verified through AnkleBreaker Unity MCP on `LETHE` port `7890`.

## Finding

The current weakness is less about missing mechanics and more about readability:

- Blood Reflection still reads strongest because it has color, sustain, mark, DoT, and ultimate progression in one package.
- Utility memories/echoes work, but several monster-state reads are too similar or too brief.
- Enemy roles exist, but the role markers are static and can disappear into dense combat.
- Gatekeeper patterns now read better, but the boss body still benefits from stronger persistent identity markers.

## Applied Changes

- Added `SpawnEchoIdentityBurst()`.
- `MarkEnemyEchoState()` now also spawns family-specific monster-state bursts.
- Passive memories now leave matching marks when they directly affect enemies:
  - `ExecutionFlash`
  - `ShatterWave`
  - `StoppedSecond`
  - `AshenShield`
  - `OblivionBrand`
- Hunter projectiles now mark targets on impact.
- Added `V1EnemyRoleMarker`.
- Added animated role symbols for:
  - `VoidPriest`
  - `DriftingEye`
  - `SplitOne`
  - `Gatekeeper`

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: standalone rerun passed with 0 warnings and 0 errors after a temporary parallel DLL lock.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 7 existing legacy warnings and 0 errors.
- Unity compilation errors: `0`.
- `LETHE/V1 QA/Echo Matrix Dual Blades`: `[V1QA] PASS`, `total=240`, `state=72`.
- `LETHE/V1 QA/Echo Matrix Greatsword`: `[V1QA] PASS`, `total=223`, `state=70`.
- `LETHE/V1 QA/Passive Memory Matrix`: `[V1QA] PASS`, `blood=17`, `ash=6`, `stopped=8`, `oblivion=37`.
- `LETHE/V1 QA/Dense Dual Blades Perf Matrix`: `[V1QA] PASS`, `hits=18`, `suppressed=15`, `transient=114`, `activeVfx=27`, `ms=104.35`.
- `LETHE/V1 QA/Gatekeeper Pattern Matrix`: `[V1QA] PASS`, `boss=4`, `meteor=20`, `cone=6`, `ring=3`.

## Remaining Gate

Direct-play review should judge:

- Whether each utility echo can now be recognized by monster-state VFX before reading text.
- Whether role markers make VoidPriest, DriftingEye, SplitOne, and Gatekeeper easier to identify.
- Whether Dense Dual Blades still feels responsive with the extra identity bursts.

# 2026-07-07 Direct Feedback VFX Action Pass

## Status

Implemented, locally build-verified, and Unity-QA-verified through AnkleBreaker Unity MCP on `LETHE` port `7890`.

## Applied Changes

- Dual blades now spawn guaranteed lightweight slash cuts even when profile VFX is throttled.
- Dense dual-blade mode keeps one cheap guaranteed cut and skips the extra spark so dense waves remain responsive.
- Gatekeeper meteor now reads as:
  - red danger tell,
  - falling meteor body/trail/shadow,
  - impact/debris burst.
- Gatekeeper cone now reads as:
  - red danger tell,
  - charge blade/edge lines,
  - sweeping slash wave on hit.
- Player damage from Gatekeeper patterns now has a red flash/ring, damage text, hit SFX, and small camera shake.
- Hungry Blades / Kalmuri orbit now uses same-direction circular lanes, an orbit path guide, orbit-exit cue, lock line, and target lunge.
- Follow-up: Kalmuri hunt now chooses a real orbit blade slot nearest the target, highlights it, and launches the lunge from that same orbit endpoint.
- Dense Dual Blades Perf Matrix now uses a dense-specific transient snapshot instead of the global transient counter.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 existing legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 7 existing legacy warnings and 0 errors.
- Unity compilation errors: `0`.
- `LETHE/V1 QA/Dense Dual Blades Perf Matrix`: `[V1QA] PASS`, `hits=18`, `suppressed=15`, `transient=62`, `activeVfx=27`, `ms=85.10`.
- `LETHE/V1 QA/Gatekeeper Pattern Matrix`: `[V1QA] PASS`, `boss=4`, `meteor=20`, `cone=6`, `ring=3`.
- `LETHE/V1 QA/Kalmuri Perf Matrix`: `[V1QA] PASS`, `orbit=44`, `bite=72`, `return=24`, `hunting=16`, `totalKalmuri=268`.
- `LETHE/V1 QA/Kalmuri Perf Matrix` after orbit-slot linking: `[V1QA] PASS`, `orbit=44`, `bite=72`, `return=24`, `hunting=16`, `totalKalmuri=270`.

## Remaining Gate

Direct-play review should judge:

- Whether dual blades are now visible enough without feeling noisy.
- Whether Gatekeeper meteor feels like something falls before the hit.
- Whether Gatekeeper cone feels like a real cleave instead of a static wedge.
- Whether player damage from boss patterns is unmistakable.
- Whether Kalmuri now reads as orbit -> lock -> lunge rather than wobble.

# 2026-07-06 MCP QA Recovery / Dense Dual-Blade Final Pass

## Status

Implemented, locally build-verified, and Unity-QA-verified through AnkleBreaker Unity MCP on `LETHE` port `7890`.

## Applied Changes

- Dense dual-blade weapon slash VFX now keeps only the primary slash in high-density fights.
- Dense Kalmuri follow-ups now skip:
  - support range ring,
  - moving dive trails,
  - extra slash entries.
- Dense Kalmuri keeps a minimal clamp/rip read:
  - two clamp blades,
  - one rip line.
- Added generated sprite caching for repeated circle/ring/box/impact-diamond procedural sprites.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors.
- Unity compilation errors: `0`.
- Unity console errors after final QA: `0`.
- `LETHE/V1 QA/Echo Matrix Dual Blades`: `[V1QA] PASS`, `total=240`, `state=73`.
- `LETHE/V1 QA/Echo Matrix Greatsword`: `[V1QA] PASS`, `total=223`, `state=69`.
- `LETHE/V1 QA/Gatekeeper Pattern Matrix`: `[V1QA] PASS`, `boss=4`, `meteor=20`, `cone=6`, `ring=3`.
- `LETHE/V1 QA/Gatekeeper Jump`: `[V1QA] PASS`, `boss=1`, `liveEnemies=15`.
- `LETHE/V1 QA/Dense Dual Blades Perf Matrix`: `[V1QA] PASS`, `hits=30`, `suppressed=25`, `transient=64`, `activeVfx=42`, `ms=55.73`.
- `LETHE/V1 QA/Kalmuri Perf Matrix`: `[V1QA] PASS`, `totalKalmuri=0`.
- `LETHE/V1 QA/Void Priest Heal Matrix`: `[V1QA] PASS`, `attempts=12`, `accepted=4`, `vfx=16`.
- `LETHE/V1 QA/M2 Loop`: `[V1QA] PASS`, `hungryEcho=5`, `bloodEcho=5`.

## Remaining Gate

Automated QA is now green. Direct-play review should judge:

- Kalmuri clamp/rip visual quality at +1/+3/+5.
- Gatekeeper raid telegraph readability.
- Dense dual-blade feel after the aggressive VFX throttle.
- VoidPriest heal readability and enemy separation feel in a real dense wave.

# 2026-07-06 Kalmuri Echo Clamp/Rip Readability Pass

## Status

Implemented in code and locally build-verified. Unity Kalmuri visual/perf QA rerun is pending because AnkleBreaker Unity MCP still returns `Transport closed`.

## Applied Changes

- Added `SpawnKalmuriEchoClampRip()`.
- Kalmuri echo follow-ups now show:
  - two opposing clamp blades closing into the target point,
  - a central bite/rip cue,
  - a short wound slash after the clamp.
- Reduced the old generic Kalmuri echo range/flash/cut visuals so they act as supporting read instead of the main concept.
- Dense dual-blade Kalmuri follow-ups now skip the old surge blade loop and keep the lighter clamp/rip shape to avoid undoing the dense VFX churn pass.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 existing legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 7 existing legacy warnings and 0 errors.
- AnkleBreaker Unity MCP reconnect attempt still failed with `Transport closed`.

## Remaining Gate

Retry Unity QA when MCP routing is stable:

- `LETHE/V1 QA/Kalmuri Perf Matrix`
- direct-play check +1/+3/+5 Kalmuri echo visual scale
- dense dual-blade check to confirm the lighter clamp/rip does not reintroduce VFX churn

# 2026-07-06 Echo State Mark Readability Pass

## Status

Implemented in code and locally build-verified. Unity Echo Matrix rerun is pending because AnkleBreaker Unity MCP still returns `Transport closed`.

## Applied Changes

- Added `V1Enemy.ApplyEchoStateMark()`.
- Added `V1EnemyStateBadge`, a short-lived pulsing/rotating marker attached to affected enemies.
- Utility echoes now mark affected monsters:
  - `ExecutionFlash`
  - `HunterOath`
  - `ShatterWave`
  - `StoppedSecond`
  - `AshenShield`
  - `OblivionBrand`
- Enemy body color now receives a temporary utility echo tint, blended with BloodMarked tint if both are active.
- Echo Matrix QA now counts and requires `EchoState_*` objects.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors.

## Remaining Gate

Retry Unity QA when MCP routing is stable:

- `LETHE/V1 QA/Echo Matrix Dual Blades`
- `LETHE/V1 QA/Echo Matrix Greatsword`

Direct play should check whether non-blood echoes can now be identified from monster marks before reading text.

# 2026-07-06 Gatekeeper Raid Telegraph Pass

## Status

Implemented in code and locally build-verified. Unity QA rerun is pending because AnkleBreaker Unity MCP still returns `Transport closed`.

## Applied Changes

- Added `V1RaidTelegraphFill`.
- Gatekeeper meteor, cone, and ring patterns now read as:
  - danger boundary appears immediately,
  - internal red fill grows over the warning time,
  - impact flash/bang plays when damage resolves.
- Gatekeeper warning windows now clamp to at least `0.92s`.
- Transient sprite pooling now disables stale `V1RaidTelegraphFill` components when reusing objects for unrelated VFX.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 existing legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors on standalone rerun after a parallel DLL lock.

## Remaining Gate

Retry Unity QA when MCP routing is stable:

- `LETHE/V1 QA/Gatekeeper Pattern Matrix`
- `LETHE/V1 QA/Gatekeeper Jump`

Visually judge whether the pattern now reads like a simple raid mechanic: red zone appears, fills, then pops.

# 2026-07-06 Dense Dual-Blade VFX Churn Pass

## Status

Implemented in code and locally build-verified. Final Unity QA rerun is pending because the AnkleBreaker Unity MCP transport closed after `Assets/Refresh`, while the `LETHE` Unity editor process remained alive.

## Applied Changes

- Added dense dual-blade throttles:
  - secondary echo chains are suppressed in high live-enemy counts,
  - secondary slash VFX is capped,
  - dense Kalmuri follow-ups carry a scheduled dense flag,
  - dense Kalmuri follow-ups use fewer surge blades and skip extra flash/barrage,
  - dense Blood Reflection suppresses bloom/accent spam while keeping mark readability,
  - dense utility echoes rotate one utility family per primary hit.
- Added debug counters:
  - `debugTransientSpriteSpawnCount`
  - `debugDenseDualBladeHits`
  - `debugDenseDualBladeEchoesSuppressed`
  - `debugDenseDualBladeMs`
- Added `FindVoidPriestHealTargets()` so VoidPriest healing uses the manager enemy list instead of repeated whole-scene scans.
- Added `ClearEnemiesForDebug()`.
- Added `DebugRunDenseDualBladePerfMatrix()`.
- Added Unity QA menu:
  - `LETHE/V1 QA/Dense Dual Blades Perf Matrix`

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 existing legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 7 existing legacy warnings and 0 errors.
- Unity compile error count before MCP transport closure: `0`.
- Dense QA failing baseline/progress before the final throttle:
  - `hits=24`, `suppressed=4`, `transient=954`, `activeVfx=506`, `ms=947.64`.
  - `hits=24`, `suppressed=10`, `transient=550`, `activeVfx=307`, `ms=561.77`.
  - `hits=30`, `suppressed=15`, `transient=627`, `activeVfx=361`, `ms=644.39`.

## Remaining Gate

Retry Unity QA when MCP routing is stable:

- `LETHE/V1 QA/Dense Dual Blades Perf Matrix`
- `LETHE/V1 QA/Void Priest Heal Matrix`
- `LETHE/V1 QA/M2 Loop`

If Dense QA still fails, keep tuning object churn before adding any more VFX volume.

# 2026-07-06 Gatekeeper Jump Debug / VFX-Performance Triage Result

## Status

The immediate debug request is implemented in `Dev_Prototype_v1`. Jaewoo can now jump directly to the first Gatekeeper from the start overlay or from the F12 debug panel, without waiting for the boss timer.

## Applied Changes

- Changed `F6` to call `DebugJumpToGatekeeper()`.
- Made `F6` work from the weapon-select overlay.
- Added a `Boss` button to the F12 debug panel.
- Added `RemoveExistingGatekeepers()` so repeated debug jumps do not stack bosses.
- Added `DebugJumpToGatekeeper()`:
  - starts the run if needed,
  - closes choice/result/refill/death overlays,
  - uses fast debug boss values,
  - resets Gatekeeper rank/index to the first boss,
  - seeds an empty run with `HungryBlades:3`, `BloodReflection:2`, `StoppedSecond:1`,
  - spawns review enemies,
  - shows the Gatekeeper warning cue,
  - spawns one Gatekeeper.
- Added Unity QA menu:
  - `LETHE/V1 QA/Gatekeeper Jump`

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 existing legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors.
- Unity compile error count: `0`.
- Unity QA:
  - `LETHE/V1 QA/Gatekeeper Jump`: `[V1QA] PASS`, `boss=1`, `liveEnemies=15`, `memories=[HungryBlades:3,BloodReflection:2,StoppedSecond:1]`.
- Unity console error count after QA: `0`.

## Triage Finding

The next work should not be another broad content pass. Jaewoo's current blockers are:

- VFX identity is too low; effects work but do not read as different enough in play.
- Hungry Blades / Kalmuri echo still feels visually mismatched to the intended concept.
- Dual blades appear to lag when enemy count rises, likely from dense-hit VFX/object churn plus repeated enemy queries.

## Next Implementation

Make a focused dense-wave VFX/performance pass:

- Profile/measure transient object counts during dual-blade dense hits.
- Cap or pool the worst repeated VFX families before increasing spectacle.
- Replace `VoidPriest` whole-scene heal scans with manager-side nearby enemy queries.
- Redesign Kalmuri echo visual language around readable blade action, not more generic cyan rings.
- Add a QA matrix that records both object count and rough frame/perf risk under dense dual-blade combat.

# 2026-07-06 VoidPriest Heal / Echo-Memory Interaction Audit Result

## Status

The immediate healer stacking problem has been addressed in `Dev_Prototype_v1`. VoidPriest healing is now visible and capped per receiver beat. A broader memory/echo/ultimate versus monster interaction audit has also been recorded for the next pass.

## Applied Changes

- Added `VoidPriest` heal VFX:
  - source pulse,
  - target pulse/core,
  - heal thread,
  - floating heal amount.
- Changed VoidPriest healing:
  - interval: `1.05s`,
  - amount: `2.4`,
  - per-priest target cap: `3`,
  - per-target receiver lockout: `0.95s`,
  - target priority: lowest health ratio nearby non-boss enemies.
- Added debug counters:
  - `debugVoidPriestHealAttempts`
  - `debugVoidPriestHealAccepted`
- Added `DebugRunVoidPriestHealMatrix()`.
- Added Unity QA menu:
  - `LETHE/V1 QA/Void Priest Heal Matrix`
- Added audit handoff:
  - `docs/orchestration/review_prompts/2026-07-06-echo-memory-monster-interaction-audit.md`

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 existing legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors on standalone rerun.
- Unity compile error count: `0`.
- Unity QA:
  - `LETHE/V1 QA/Void Priest Heal Matrix`: `[V1QA] PASS`, `attempts=12`, `accepted=4`, `vfx=16`.
  - `LETHE/V1 QA/M2 Loop`: `[V1QA] PASS`, `hungryEcho=5`, `bloodEcho=5`, `storm=True`.
  - `LETHE/V1 QA/Echo Matrix Dual Blades`: `[V1QA] PASS`, `total=240`.
  - `LETHE/V1 QA/Passive Memory Matrix`: `[V1QA] PASS`, `blood=17`, `ash=6`, `stopped=8`, `oblivion=36`.
  - `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`: `[V1QA] PASS`, `fracture=22`, `stasis=9`, `ashen=47`.

## Audit Finding

Blood Reflection currently feels strongest because it is frequent, visible, damaging, healing, and tied to the clearest ultimate route. Non-blood memories/echoes need clearer enemy-state changes and interaction payoff before broad numeric buffs.

## Next Implementation

Make one non-blood readability pass: add clearer monster-state marks for Shatter, Stopped, Ashen, Hunter, Execution, and Oblivion, then add a QA matrix that tracks damage/heal/control contributions rather than only object counts.

# 2026-07-06 Gatekeeper Sprite Repair Result

## Status

The Gatekeeper visual regression has been repaired in `Dev_Prototype_v1`. The previous rank-based procedural body read as a soft blob/slime, so the boss now uses authored angular gate/mask PNG sprites first.

## Applied Changes

- Replaced `spr_boss_gatekeeper_01.png` with a sharper gate/mask body.
- Added:
  - `spr_boss_gatekeeper_02.png`
  - `spr_boss_gatekeeper_03.png`
  - `spr_boss_gatekeeper_04.png`
- Added `BossGatekeeperRankPaths`.
- Changed `EnemySprite(Gatekeeper)` to call `GatekeeperBodySprite(rank)`.
- `GatekeeperBodySprite(rank)` now loads rank-specific PNGs before falling back.
- Reworked the procedural `MakeGatekeeperSprite()` fallback so it keeps an angular gate/mask silhouette instead of a rounded body.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 existing legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors on standalone rerun.
- Unity compile error count: `0`.
- Unity QA:
  - `LETHE/V1 QA/Gatekeeper Pattern Matrix`: `[V1QA] PASS`, `boss=4`, `meteor=15`, `cone=4`, `ring=3`.
  - `LETHE/V1 QA/M2 Loop`: `[V1QA] PASS`, `hungryEcho=5`, `bloodEcho=5`, `storm=True`.

## Next Implementation

Directly inspect the four boss bodies in scene. If the shape still feels too cheap, the next pass should be dedicated art direction only, not combat tuning.

# 2026-07-06 Enemy Soft Separation Result

## Status

The normal-enemy stacking issue has been addressed in `Dev_Prototype_v1` with soft separation. Enemies can still pack densely, but they now nudge away from nearby enemies instead of collapsing into a single point.

## Applied Changes

- Added `V1GameManager.EnemySeparationForce(V1Enemy self)`.
- Added deterministic fallback direction for exact-position overlap cases.
- Applied separation in `V1Enemy.Update()`:
  - lower multiplier for `Gatekeeper`,
  - normal multiplier for chasers/splitters,
  - smaller but persistent separation for `VoidPriest`,
  - ranged-position separation for `DriftingEye`.
- Added debug counters:
  - `debugSeparationOverlapBefore`
  - `debugSeparationOverlapAfter`
- Added `DebugRunEnemySeparationMatrix()`.
- Added Unity QA menu:
  - `LETHE/V1 QA/Enemy Separation Matrix`

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors on standalone rerun.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- Unity compile error count: `0`.
- Unity QA:
  - `LETHE/V1 QA/Enemy Separation Matrix`: `[V1QA] PASS`, overlap pairs `91 -> 4`.
  - `LETHE/V1 QA/M2 Loop`: `[V1QA] PASS`, `hungryEcho=5`, `bloodEcho=5`, `storm=True`.

## Next Implementation

Direct-play a dense wave around/after the first Gatekeeper. If the swarm now feels too loose or too sticky, tune only the separation padding/multipliers before adding new enemy behavior.

# 2026-07-06 Gatekeeper Pattern Feedback Result

## Status

The user-reported first-boss issue has been addressed in `Dev_Prototype_v1`. The boss no longer receives `VoidPriest` healing, and Gatekeepers now use visible red danger telegraphs before pattern damage.

## Applied Changes

- Excluded `Gatekeeper` from `VoidPriest` healing targets.
- Replaced the old immediate Gatekeeper pulse with delayed red telegraph patterns:
  - meteor circle,
  - cone slash,
  - ring burst,
  - late-gate combined cone/meteor pressure.
- Added rank-based procedural Gatekeeper sprite variants and pattern sigils so the four boss appearances no longer read as the exact same fallback concept.
- Added `DebugRunGatekeeperPatternMatrix()`.
- Added Unity QA menu:
  - `LETHE/V1 QA/Gatekeeper Pattern Matrix`

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors on standalone rerun.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- Unity compile error count: `0`.
- Unity QA:
  - `LETHE/V1 QA/Gatekeeper Pattern Matrix`: `[V1QA] PASS`, `boss=4`, `meteor=15`, `cone=4`, `ring=3`.
  - `LETHE/V1 QA/M2 Loop`: `[V1QA] PASS`, `hungryEcho=5`, `bloodEcho=5`, `storm=True`.
- Unity console error count after QA: `0`.

## Next Implementation

Direct-play the first Gatekeeper again. If it still feels unfair or visually weak, make exactly one narrow pass: telegraph timing, telegraph size, boss HP/guard uptime, or boss sprite polish.

# 2026-07-02 Direct Play Review Prep Result

## Status

The current `_dev` slice is ready for jaewoo direct-play review. Automated QA is green, but promotion should wait for human feel judgment.

## Added Handoff

- `docs/orchestration/review_prompts/2026-07-02-dev-prototype-v1-direct-play-review.md`

## Review Scope

- Base weapon feel for dual blades and greatsword.
- Kalmuri +5 density after optimization.
- Passive memory readability before forgetting.
- Echo identity by VFX/action, not text.
- Forget/resonance transition timing.
- Blood Blade Storm versus the three utility ultimates.
- Audio fatigue.
- Performance and VFX clutter.

## Decision Gate

Use the prompt's output format:

- `GO`: prepare `_dev` to `Assets/Lethe` promotion plan.
- `ITERATE`: fix only the top 1-2 feel issues.
- `NO-GO`: write a focused rework plan before adding more content.

# 2026-07-02 Utility Ultimate Feel Tuning Result

## Status

Non-blood utility ultimates now have sharper cadence and impact without adding a new system. `BloodBladeStorm` remains the benchmark and still passes after the changes.

## Applied Changes

- Reduced utility ultimate pulse intervals.
- `FractureExecution`: stronger greatsword stamp/cleave/verdict, faster dual execution cuts, higher hitstop and damage.
- `StasisHunt`: stronger greatsword freeze dome/spear, faster dual hunter shots, longer freeze windows.
- `AshenOblivion`: stronger heal, heavier greatsword guard-break line, faster dual return/parry rhythm.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- Unity QA:
  - `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`: `[V1QA] PASS`, `fracture=22`, `stasis=9`, `ashen=47`.
  - `LETHE/V1 QA/Utility Ultimate Matrix Greatsword`: `[V1QA] PASS`, `fracture=9`, `stasis=20`, `ashen=21`.
  - `LETHE/V1 QA/Blood Blade Storm`: `[V1QA] PASS`, `stormObjects=77`.

## Next Implementation

Prepare the direct-play review checklist and current-build handoff so jaewoo can judge whether `_dev` is close to promotion quality.

# 2026-07-02 Forget / Resonance UX Compression Result

## Status

Forget/resonance now reads more like a quick action transition. The result overlay is shorter, the transform VFX is tighter, and the ultimate-ready cue is less screen-dominating.

## Applied Changes

- Shortened readable forget result overlay text.
- Rebuilt `EchoTransform` into a smaller core burst with named shards.
- Pulled `ForgetFlow` memory and echo symbols closer to the player.
- Shortened memory-to-echo, resonance, awaken, and ultimate bridge lifetimes.
- Reduced `UltimateReady` ring size/lifetime and lowered the floating text.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- Unity QA:
  - `LETHE/V1 QA/Forget Resonance Flow`: `[V1QA] PASS`, `forgetFlow=15`, `echoTransform=14`, `ultimateReady=3`.
  - `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`: `[V1QA] PASS`, `fracture=19`, `stasis=9`, `ashen=34`.
  - `LETHE/V1 QA/Kalmuri Perf Matrix`: `[V1QA] PASS`, `totalKalmuri=374`.

## Next Implementation

Move to ultimate feel tuning: sharpen `FractureExecution`, `StasisHunt`, and `AshenOblivion` cadence/power without adding screen clutter.

# 2026-07-02 Passive Memory Feel Tuning Result

## Status

Passive-feeling memories now have stronger readable combat beats before they are forgotten into echoes. The work stayed inside `Dev_Prototype_v1` and did not add new systems or promote content out of `_dev`.

## Applied Changes

- `BloodReflection`: faster pulse cadence, wider radius, +5 cap increased to seven targets, stronger bloom, and awakened draw thread.
- `StoppedSecond`: faster field cadence, wider freeze radius, stronger aftercut, and longer +5 freeze reach.
- `AshenShield`: faster guard pulse, larger counter radius, stronger counter damage, and clearer +5 guard wave.
- `OblivionBrand`: faster mark cadence, four +5 brands, stronger fork links, and awakened seals centered on branded targets.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- Unity QA:
  - `LETHE/V1 QA/Passive Memory Matrix`: `[V1QA] PASS`, `blood=17`, `ash=6`, `stopped=8`, `oblivion=36`.
  - `LETHE/V1 QA/Echo Matrix Dual Blades`: `[V1QA] PASS`, `total=240`.
  - `LETHE/V1 QA/Forget Resonance Flow`: `[V1QA] PASS`, `forgetFlow=15`, `echoTransform=2`, `ultimateReady=3`.
  - `LETHE/V1 QA/Kalmuri Perf Matrix`: `[V1QA] PASS`, `totalKalmuri=374`.

## Next Implementation

Move to forget/resonance UX tuning: shorten or clarify the flow so forgetting reads as an action transition first and a text confirmation second.

# 2026-07-02 Utility Echo Legacy Fallback Removal Result

## Status

The current utility echo trigger path is now cleaner and safer. `TriggerUtilityEchoes` no longer carries the old inline fallback implementation that was unreachable after the newer per-echo handlers were introduced.

## Applied Changes

- Removed the legacy inline utility echo fallback branch from `V1GameManager.TriggerUtilityEchoes`.
- Added an explicit null-enemy guard.
- Kept current behavior routed through the existing per-echo handlers:
  - `TriggerShatterEcho`
  - `TriggerExecutionEcho`
  - `TriggerHunterEcho`
  - `TriggerStoppedEcho`
  - `TriggerAshenEcho`
  - `TriggerOblivionEcho`

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- Unity compile error count: `0`.
- Unity QA:
  - `LETHE/V1 QA/Echo Matrix Dual Blades`: `[V1QA] PASS`, `total=240`.
  - `LETHE/V1 QA/Echo Matrix Greatsword`: `[V1QA] PASS`, `total=221`.
  - `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`: `[V1QA] PASS`, `fracture=19`, `stasis=9`, `ashen=34`.
  - `LETHE/V1 QA/Utility Ultimate Matrix Greatsword`: `[V1QA] PASS`, `fracture=8`, `stasis=22`, `ashen=16`.
  - `LETHE/V1 QA/Kalmuri Perf Matrix`: `[V1QA] PASS`, `totalKalmuri=374`.

## Next Implementation

Continue the remaining highest-priority cleanup: compact repeated echo/ultimate effect constants, then move into passive memory feel tuning once the runtime routes are short enough.

# 2026-07-02 Utility Echo Tuning Data Asset Migration Result

## Status

Utility echo tuning is now backed by `_dev/Data` in `Dev_Prototype_v1`. The manager still keeps safe serialized/static fallback values, but the primary runtime route is a ScriptableObject asset.

## Applied Changes

- Added `V1UtilityEchoTuningTable` ScriptableObject.
- Added data asset:
  - `Assets/_dev/Data/Echoes/UtilityEcho_Tuning.asset`
- Moved the six utility echo tuning specs into that asset:
  - `ExecutionFlash`
  - `HunterOath`
  - `ShatterWave`
  - `StoppedSecond`
  - `AshenShield`
  - `OblivionBrand`
- Connected the asset through:
  - `V1_ContentCatalog.asset`
  - `V1ContentCatalog`
  - `V1SceneBuilder`
  - `V1GameManager`
- Runtime lookup order is now:
  - asset table,
  - serialized manager table,
  - static default table,
  - safe fallback spec.

## Verification

- Unity compile error count: `0`.
- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors after retrying a transient DLL lock from a parallel build.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- Unity QA:
  - `LETHE/V1 QA/Echo Matrix Dual Blades`: `[V1QA] PASS`, `total=240`.
  - `LETHE/V1 QA/Echo Matrix Greatsword`: `[V1QA] PASS`, `total=221`.
  - `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`: `[V1QA] PASS`, `fracture=19`, `stasis=9`, `ashen=34`.
  - `LETHE/V1 QA/Utility Ultimate Matrix Greatsword`: `[V1QA] PASS`, `fracture=8`, `stasis=22`, `ashen=16`.

## Next Implementation

Continue with echo/ultimate runtime cleanup: move remaining repeated effect constants into compact specs and remove unreachable compatibility/fallback branches only after each QA matrix proves stable.

# 2026-07-02 Utility Echo Serializable Tuning Table Result

## Status

The next echo dataization step is implemented in `Dev_Prototype_v1`. Utility echo tuning still lives inside `V1GameManager`, but the values now flow through a serializable tuning table instead of scattered helper formulas.

## Applied Changes

- Added `UtilityEchoTuningSpec[] utilityEchoTuningSpecs` as a serialized field on `V1GameManager`.
- Added a static default tuning table for:
  - `ExecutionFlash`
  - `HunterOath`
  - `ShatterWave`
  - `StoppedSecond`
  - `AshenShield`
  - `OblivionBrand`
- Added safe fallback lookup so an empty or partially missing serialized table still preserves previous behavior.
- Moved utility echo tuning access through table methods:
  - proc chance,
  - first-hit gating,
  - light/heavy radius,
  - light/heavy target limits,
  - light/heavy damage multiplier,
  - light/heavy freeze duration,
  - execution health threshold.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- Unity compile error count: `0`.
- Unity QA:
  - `LETHE/V1 QA/Echo Matrix Dual Blades`: `[V1QA] PASS`, `total=240`.
  - `LETHE/V1 QA/Echo Matrix Greatsword`: `[V1QA] PASS`, `total=219`.
  - `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`: `[V1QA] PASS`, `fracture=19`, `stasis=9`, `ashen=34`.
  - `LETHE/V1 QA/Utility Ultimate Matrix Greatsword`: `[V1QA] PASS`, `fracture=8`, `stasis=26`, `ashen=16`.
  - `LETHE/V1 QA/Kalmuri Perf Matrix`: `[V1QA] PASS`, `totalKalmuri=0` in the current post-optimization smoke run.

## Next Implementation

Move the serializable utility echo table into `_dev/Data` as a dedicated ScriptableObject/data contract, then apply the same data path to remaining ultimate and VFX timing constants.

# 2026-07-02 Echo Tuning Spec / QA Counter Cleanup Result

## Status

The first pass of echo dataization cleanup is implemented in `Dev_Prototype_v1`. Values are not yet moved to ScriptableObject assets, but repeated tuning formulas are now centralized behind compact helper/spec functions.

## Applied Changes

- Added utility echo tuning helpers in `V1GameManager`:
  - `ShouldTriggerEcho`
  - `EchoProcChance`
  - `EchoRadius`
  - `EchoTargetLimit`
  - `EchoDamageMultiplier`
  - `EchoFreezeSeconds`
  - `ExecutionHealthThreshold`
- Rewired weapon-specific utility echoes to use the helpers for proc checks, radius, damage, freeze duration, and target limits.
- Added reusable QA count structures/helpers in `V1SmokeTestMenu`:
  - count metrics,
  - count limits,
  - positive-count checks,
  - max-limit checks,
  - shared count formatting.
- Refactored Echo Matrix, Passive Memory Matrix, Kalmuri Perf Matrix, and Utility Ultimate Matrix pass checks to use the shared helpers.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors after retrying a transient DLL lock.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- Unity QA:
  - `LETHE/V1 QA/Echo Matrix Dual Blades`: `[V1QA] PASS`, `total=240`.
  - `LETHE/V1 QA/Echo Matrix Greatsword`: `[V1QA] PASS`, `total=223`.
  - `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`: `[V1QA] PASS`, `fracture=19`, `stasis=9`, `ashen=34`.
  - `LETHE/V1 QA/Utility Ultimate Matrix Greatsword`: `[V1QA] PASS`, `fracture=8`, `stasis=26`, `ashen=16`.
  - `LETHE/V1 QA/Kalmuri Perf Matrix`: `[V1QA] PASS`, `totalKalmuri=374`.

## Next Implementation

Continue the dataization sequence by moving these compact helper/spec values toward a small serializable spec table or `_dev/Data` assets, then remove old fallback echo branches once the data route is stable.

# 2026-07-02 Kalmuri Performance Optimization Result

## Status

The user-reported multi-enemy lag risk for `Hungry Blades / Kalmuri` has been optimized and a dedicated QA gate now exists.

## Applied Changes

- Lowered the +5 visual swarm from many small blades to fewer, larger, clearer blades.
- Reduced Kalmuri bite tick rate, +5 target fan-out, bite blade count, return shard count, echo surge count, and echo barrage count.
- Added a short cooldown to +5 awakened Kalmuri launch projectiles so dense hit chains no longer create a projectile/ring burst every hit.
- Shortened several helper ring/flash/cut lifetimes.
- Added `DebugRunKalmuriPerfMatrix()`.
- Added Unity QA menu:
  - `LETHE/V1 QA/Kalmuri Perf Matrix`

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- Unity QA:
  - `LETHE/V1 QA/Kalmuri Perf Matrix`: `[V1QA] PASS`
  - Object counts: `orbit=44`, `bite=72`, `return=24`, `hunting=14`, `echoSurge=64`, `echoBarrage=32`, `totalKalmuri=374`.
- Baseline comparison:
  - First measured fail: `totalKalmuri=690`.
  - Intermediate fails: `450`, `434`.
  - Final pass: `374`.

## Next Implementation

Direct-play the +5 Kalmuri feel to confirm the lower count still feels like a strong blade swarm, then continue `docs/orchestration/state/NEXT_TASKS.md` item 1: echo dataization and QA counter cleanup.

# 2026-07-02 Utility Ultimate Weapon Pattern Result

## Status

The fourth sequence item, weapon-specific utility ultimate expansion, is implemented in `Dev_Prototype_v1`.

## Applied Changes

- Added weapon-specific utility ultimate runtime paths while preserving `BloodBladeStorm` as the benchmark.
- Added `UltDual_*` and `UltGreat_*` object families for the three non-blood ultimates.
- `FractureExecution`:
  - Dual blades: rapid low-HP execution cuts and marks.
  - Greatsword: large execution stamp, cleave, verdict burst, and heavier damage/knockback.
- `StasisHunt`:
  - Dual blades: small stasis field, micro clamps, and many fast hunter shots.
  - Greatsword: target-centered stasis dome, frozen cleaves, spear read, longer freeze.
- `AshenOblivion`:
  - Dual blades: guard return ring, parry lines, and returning brand links.
  - Greatsword: large ash/brand wave, guard-break ring, and heavy brand links.
- Added Unity QA menu items:
  - `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`
  - `LETHE/V1 QA/Utility Ultimate Matrix Greatsword`

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors after a retry. First attempt hit a transient DLL file lock from Unity/dotnet concurrency.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- Unity QA:
  - `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`: `[V1QA] PASS`, `fracture=19`, `stasis=9`, `ashen=34`.
  - `LETHE/V1 QA/Utility Ultimate Matrix Greatsword`: `[V1QA] PASS`, `fracture=8`, `stasis=22`, `ashen=16`.

## Next Implementation

Continue with `docs/orchestration/state/NEXT_TASKS.md` item 1: echo dataization and QA counter cleanup.

# 2026-07-02 Forget / Resonance UX Pass Result

## Status

The third sequence item, forgetting / resonance UX production pass, is implemented in `Dev_Prototype_v1`.

## Applied Changes

- Added readable Korean forget-result overlay copy after the legacy overlay setup.
- Added `ForgetFlow_*` transition VFX:
  - lost memory marker,
  - memory break ring,
  - gained echo marker,
  - memory-to-echo bridge line,
  - echo level ring,
  - resonance target/thread,
  - +5 awaken stamp/burst,
  - ultimate bridge when a completed pair is ready.
- Added debug method `DebugRunForgetResonanceFlow()`.
- Added Unity QA menu:
  - `LETHE/V1 QA/Forget Resonance Flow`

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- Unity QA:
  - `LETHE/V1 QA/Forget Resonance Flow`: `[V1QA] PASS`
  - Object counts/state: `forgetFlow=15`, `echoTransform=2`, `ultimateReady=3`, `hungryEcho=5`, `bloodEcho=5`, `storm=True`, `result=True`.

## Next Implementation

Continue with `docs/orchestration/state/NEXT_TASKS.md` item 1: four ultimate weapon-pattern expansion.

# 2026-07-02 Passive Memory Reinforcement Result

## Status

The second sequence item, passive-feeling active memory reinforcement, is implemented in `Dev_Prototype_v1`.

## Applied Changes

- Strengthened `BloodReflection` as an active pulse instead of only a mark/heal passive:
  - periodic blood bloom action,
  - nearby victim marking and damage,
  - +3 player tether threads,
  - +5 awakened lash and blood bloom burst.
- Strengthened `AshenShield`:
  - existing shield pulse remains,
  - +3 counter slash lines and nearby counter damage,
  - +5 awakened shield wave and small recovery.
- Strengthened `StoppedSecond`:
  - added a visible memory beat ring,
  - +3 aftercut lines,
  - +5 wider freeze dome extension.
- Strengthened `OblivionBrand`:
  - added player-to-target brand tether,
  - +3 fork links and splash damage to nearby enemies,
  - +5 awakened seal feedback.
- Added Unity QA menu:
  - `LETHE/V1 QA/Passive Memory Matrix`

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors.
- Final editor build after cleanup: passed with 0 warnings and 0 errors.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- Unity QA:
  - `LETHE/V1 QA/Passive Memory Matrix`: `[V1QA] PASS`
  - Object counts: `blood=13`, `ash=5`, `stopped=6`, `oblivion=24`.

## Next Implementation

Continue with `docs/orchestration/state/NEXT_TASKS.md` item 1: forgetting / resonance UX production pass.

# 2026-07-02 Weapon-Specific Echo Pass Result

## Status

The first sequence item, weapon-specific echo differentiation, is implemented in `Dev_Prototype_v1`.

## Applied Changes

- Added dual-blade and greatsword echo behavior branches while preserving one echo definition per memory.
- Added `EchoDual_` and `EchoGreat_` runtime object families so QA can inspect the actual weapon-specific echo route.
- Blood echo now branches into fast bleed-stack wounds for dual blades and heavier cleave/pool spread for greatsword.
- Utility echoes now branch by weapon style:
  - Shatter: dual ripple vs greatsword fracture cleave.
  - Execution: dual chain cuts vs greatsword execution stamp.
  - Hunter: dual fan shots vs greatsword spear-like tracking hit.
  - Stopped: dual micro clamp vs greatsword stasis dome.
  - Ashen: dual parry return vs greatsword counter-wave.
  - Oblivion: dual brand stacks vs greatsword brand detonation.
- Added Unity QA menu items:
  - `LETHE/V1 QA/Echo Matrix Dual Blades`
  - `LETHE/V1 QA/Echo Matrix Greatsword`

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with 0 warnings and 0 errors.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- Unity QA:
  - `LETHE/V1 QA/Echo Matrix Dual Blades`: `[V1QA] PASS`, `EchoDual_ total=240`, all 8 echo families counted.
  - `LETHE/V1 QA/Echo Matrix Greatsword`: `[V1QA] PASS`, `EchoGreat_ total=221`, all 8 echo families counted.

## Next Implementation

Continue with `docs/orchestration/state/NEXT_TASKS.md` item 1: 패시브화 기억 보강.

# 2026-07-02 Production Gap Audit

## Status

Next work should move away from "jaewoo direct validation" as the main blocker and into Codex-buildable production gaps.

## Findings

- The highest-value gameplay gap is echo identity, not another Kalmuri-only tuning pass.
- `TriggerUtilityEchoes` currently handles many echo families in one weapon-hit path, so several echoes differ mostly by chance, radius, color, or target count.
- `WeaponRuntimeSpec` already exposes `MultiSmall` vs `SingleHeavy`, so the next pass can branch behavior by weapon style without creating duplicate echo definitions.
- `BloodBladeStorm` is the strongest weapon-specific ultimate reference; `FractureExecution`, `StasisHunt`, and `AshenOblivion` still need the same level of dual-blade/greatsword pattern split.
- Active memories that most risk feeling passive are `BloodReflection`, `AshenShield`, `StoppedSecond`, and `OblivionBrand`.

## Next Implementation

Start with `docs/orchestration/state/NEXT_TASKS.md` item 1: 잔향 무기별 차별화 1차 패스.

Done criteria for that first pass:

- Dual blades and greatsword produce visibly different echo behavior for all 8 echo families.
- One `EchoDefinition` per memory remains the rule; weapon-specific expression is runtime behavior, not separate content duplication.
- VFX Matrix or a new echo matrix smoke can prove both weapon routes spawn the intended echo families.
- `dotnet build`, Unity compile error check, and console error check pass.

# 2026-07-01 Skill SFX Runtime Pass Result

## Status

`Dev_Prototype_v1` now has original generated SFX assigned to the main weapon, memory, echo, ultimate, pickup, and UI combat moments.

## Applied Audio Changes

- Added a procedural SFX palette using sine, triangle, square, noise, and layered blade waves.
- Added `PlaySfx(id, volumeMul, minInterval)` so dense repeated events can be throttled.
- Replaced the old sine-only placeholder clips.
- Added Hungry Blades lunge, pierce, and echo sounds.
- Added Blood Reflection mark/heal and Blood Blade Storm pulse sounds.
- Added utility memory sounds for Execution Flash, Hunter Oath, Shatter Wave, Stopped Second, Ashen Shield, and Oblivion Brand.
- Added or refreshed XP pickup, kill, warning, level-up, player hit, clear, and defeat cues.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- Play Mode direct `DebugRunM2Smoke()` snapshot: `HungryBlades:5`, `BloodReflection:5`, `storm=True`, `enemies=10`, `result=True`.

## Remaining Gate

Jaewoo should direct-play with audio on and judge whether repeated Kalmuri pierce/lunge sounds are satisfying rather than tiring, and whether Blood Blade Storm feels powerful without overpowering the mix.

# 2026-07-01 Kalmuri Lunge Range / Stab Feel Result

## Status

Hungry Blades now uses a larger launch acquisition range than the visible orbit, and flying blades read more like real stab projectiles.

## Applied VFX / Combat Changes

- Added `lungeRange = HungryBladesRadius * 1.75 + level * 0.36`.
- Active Kalmuri target acquisition now uses `lungeRange`.
- Bite blades now start from the player-side orbit.
- Bite blades now pass slightly through the enemy.
- Per-blade damage is delayed by short staggered timings.
- Added `KalmuriBladePierceSpark` at delayed impact.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- Unity `LETHE/V1 Smoke/M2 Loop`: `[V1QA] PASS`.

## Remaining Gate

Jaewoo should check whether blades now launch early enough and feel like stabbing rather than drifting in.

# 2026-07-01 Kalmuri Outer Orbit Removal / Per-Blade Damage Result

## Status

The outer rotating Kalmuri layer is removed. Hungry Blades now keeps one active orbit around the player, and each flying bite blade applies damage.

## Applied VFX / Combat Changes

- Removed the inner/outer orbit split.
- Replaced `innerRadius` / `outerRadius` with one `orbitRadius`.
- Removed the `lane != 1` outer-ring branch.
- Changed active bite damage from one target-level hit into per-flying-blade split damage.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- Unity `LETHE/V1 Smoke/M2 Loop`: `[V1QA] PASS`.

## Remaining Gate

Jaewoo should confirm that only one rotating Kalmuri ring remains, and that flying blades now feel like real damaging blades.

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
