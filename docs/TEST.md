# LETHE TEST

# 2026-07-23 Echo Rule Identity Rework

- Purpose:
  - Respond to jaewoo's review that Ashen, Brand, and Execution still feel like similar area attacks even after VFX improvements.
  - Preserve Shatter's current direction because its ground-break action already matches the hit rule.
- Applied target:
  - `LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs`.
- Changes:
  - Ashen memory:
    - removed periodic nearby enemy damage from the player body;
    - kept guard VFX / threat watch;
    - only releases damage when guard charge is stored.
  - Ashen stored release:
    - targets a small number of high-threat enemies instead of all enemies in a radius.
  - Great Ashen Echo:
    - changed from broad cone to holy-wall lane targeting.
  - Oblivion Brand memory:
    - changed from immediate mark damage / fork damage to inscription -> delayed erase -> delayed spread.
  - Oblivion Echo:
    - changed Great/Dual paths to light inscription damage followed by delayed erasure/collapse damage.
  - Execution Echo:
    - reduced broad area damage;
    - Greatsword now focuses the condemned target and a few low-health verdict witnesses;
    - Dual Blades chains only through condemned/low-health targets in normal play.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`.
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`.
  - Unity compilation error check.
  - Play Mode `DebugRunEchoMatrix(DualBlades)`.
  - Play Mode `DebugRunEchoMatrix(Greatsword)`.
  - Play Mode `DebugRunPassiveMemoryMatrix()`.
  - Play Mode `DebugRunDenseDualBladePerfMatrix()`.
  - Unity console error check after a delayed coroutine wait.
- Results:
  - Runtime C# build passed with 0 warnings and 0 errors after retrying a transient `Assembly-CSharp.dll` file lock.
  - Editor C# build passed with 7 existing deprecation warnings and 0 errors.
  - Unity compilation errors: `0`.
  - Dual / Great / Passive / Dense matrix reflection calls completed.
  - Unity console errors after delayed Brand coroutine wait: `0`.
- Notes:
  - Direct play should judge whether Ashen now feels like defense/counter, Brand like delayed deletion, and Execution like conditional verdict instead of three differently colored area attacks.

# 2026-07-23 Echo Hit Readability Follow-up

- Purpose:
  - Respond to jaewoo's review that the new sprites look better but the damage 판정 path is still ambiguous.
  - Especially make Ashen memory/Echo explain how a player-side guard effect turns into enemy damage.
- Applied target:
  - `LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs`.
- Changes:
  - Added `SpawnEchoHitRead` as a shared per-victim hit-confirm layer.
  - Ashen memory counter and stored guard wave now draw visible counter-return links and target-local consecration marks.
  - Great Ashen holy wall now links from the wall source to each damaged target.
  - Execution / Shatter / Oblivion Echo damage loops now stamp target-local verdict, fault, or erase marks on every affected enemy.
  - Dense Dual Blades still skips non-essential extra hit-read layers to avoid VFX spam.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`.
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`.
  - Unity compilation error check.
  - Play Mode `DebugRunEchoMatrix(DualBlades)` through MCP reflection.
  - Play Mode `DebugRunEchoMatrix(Greatsword)` through MCP reflection.
  - Unity console error check.
- Results:
  - Runtime C# build passed with 0 warnings and 0 errors after a retry. First retry hit a transient `Assembly-CSharp.dll` file lock while Unity/editor build was active.
  - Editor C# build passed with 7 existing deprecation warnings and 0 errors.
  - Unity compilation errors: `0`.
  - Dual Blades Echo Matrix reflection call completed.
  - Greatsword Echo Matrix reflection call completed.
  - Unity console errors: `0`.
- Notes:
  - Name-based runtime object counting is not reliable for these transient sprites because the runtime sprite pool does not preserve every requested effect id as the GameObject name.
  - Direct play should judge whether Ashen now reads as guard -> return/counter -> enemy hit instead of body-only VFX.

# 2026-07-23 Execution / Shatter / Ashen / Oblivion Echo Redesign Pass

- Purpose:
  - Stop normal Echoes from reusing the remade memory VFX for Execution, Shatter, Ashen, and Oblivion.
  - Confirm new weapon-specific Echo sprites and hit reads are wired, visible, and budget-safe.
- Applied target:
  - `LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs`.
  - `LETHE/Assets/_dev/Art/Sprites/Echoes/{Execution,Shockwave,Ashen,Brand}/spr_*_echo_*_01.png`.
- Changes:
  - Generated and imported 8 Echo-only sprites from a selected LETHE-style atlas.
  - Rewired four normal Echo families to use dedicated Dual/Great sprites.
  - Changed Great Shatter, Execution, Ashen, and Oblivion hit reads to match the new concepts.
  - Kept Dense Dual Blades from spawning non-essential ornament stacks.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`.
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`.
  - Unity import setting automation for 9 PNGs.
  - Unity compilation error check.
  - Play Mode `DebugRunEchoMatrix(DualBlades)` with camera capture.
  - Play Mode `DebugRunEchoMatrix(Greatsword)` with camera capture.
  - Play Mode `DebugRunDenseDualBladePerfMatrix()` reflection read.
- Results:
  - Runtime C# build passed with 7 existing deprecation warnings and 0 errors.
  - Editor C# build passed with 7 existing deprecation warnings and 0 errors.
  - Unity import settings configured: `configured=9`.
  - Unity compilation errors: `0`.
  - Dual Blades Echo Matrix: all 8 Echoes at `+5`, `dualReworkObjects=64`.
  - Greatsword Echo Matrix: all 8 Echoes at `+5`, `greatReworkObjects=32`.
  - Dense Dual Blades Perf Matrix: `hits=18`, `echoesSuppressed=15`, `transient=46`, `ms=18.30`.
- Notes:
  - All-on matrix screenshots are still intentionally noisy. The contact sheet is the cleanest evidence of the new sprite quality; direct play should judge combat scale and timing.

# 2026-07-22 Weapon-Specific Echo Mutation VFX Pass

- Purpose:
  - Make Echo VFX use the new HQ family motifs as source language but mutate by weapon.
  - Confirm Dual Blades and Greatsword Echoes produce different supporting silhouettes instead of the same HQ image at different scale.
- Applied target:
  - `LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs`.
- Changes:
  - Added Dual Blades mutation layers for Blood, Shatter, Execution, Stopped, Ashen, and Oblivion.
  - Added Greatsword mutation layers for Blood, Shatter, Execution, Stopped, Ashen, and Oblivion.
  - Preserved dense-branch throttling for non-essential Dual Blades ornaments.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`.
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`.
  - Unity compilation error check.
  - Play Mode `DebugRunEchoMatrix(DualBlades)` with synchronous camera capture.
  - Play Mode `DebugRunEchoMatrix(Greatsword)` with synchronous camera capture.
  - Play Mode `DebugRunDenseDualBladePerfMatrix()` reflection read.
- Results:
  - Runtime C# build passed with 7 existing deprecation warnings and 0 errors.
  - Editor C# build passed with 7 existing deprecation warnings and 0 errors.
  - Unity compilation errors: `0`.
  - Dual Blades Echo Matrix: all 8 Echoes at `+5`, `dualMutationObjects=320`, `spriteRenderers=4124`.
  - Greatsword Echo Matrix: all 8 Echoes at `+5`, `greatMutationObjects=240`, `spriteRenderers=3952`.
  - Dense Dual Blades Perf Matrix: `hits=18`, `echoesSuppressed=15`, `transient=109`, `ms=27.16`.
  - Evidence screenshots saved under `docs/orchestration/evidence/`.
- Notes:
  - The all-on captures intentionally overpack the screen. Direct play should decide normal-combat scale, alpha, and lifetime.

# 2026-07-22 HQ Bitmap VFX Texture Pass

- Purpose:
  - Replace the latest low-detail procedural VFX read with high-quality bitmap sprites close to jaewoo's Blood vortex reference direction.
  - Confirm the HQ assets are imported, wired into runtime motif paths, visible in Play Mode, and free of compile/console errors.
- Applied target:
  - `LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs`.
  - `LETHE/Assets/_dev/Art/Source/spr_hq_*_chroma.png`.
  - `LETHE/Assets/_dev/Art/Sprites/**/spr_*_hq_01.png`.
- Changes:
  - Generated Blood Vortex, Stopped Clock, Execution Judgement, Shatter Slam, Oblivion Brand, and Ashen Holy Fire source images.
  - Chroma-keyed them into transparent Unity sprites.
  - Set imported sprites to Sprite/Single, alpha transparency, no mipmaps, bilinear, uncompressed, PPU 100.
  - Rewired memory, Echo, and Ultimate motif calls to use the HQ assets first.
  - Tuned repeated Stopped clock scale/alpha for Dual Blades after Play Mode capture review.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`.
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`.
  - Unity `AssetDatabase.Refresh()`.
  - Unity compilation error check.
  - Unity console error check.
  - Play Mode `DebugPreviewAllUtilityVfx()`.
  - Play Mode `DebugRunEchoMatrix(DualBlades)` with synchronous camera capture.
  - Play Mode `DebugRunEchoMatrix(Greatsword)` with synchronous camera capture.
  - Play Mode `DebugRunDenseDualBladePerfMatrix()` reflection read.
- Results:
  - Runtime C# build passed with 7 existing legacy warnings and 0 errors.
  - Editor C# build passed with 0 warnings and 0 errors.
  - Unity compilation errors: `0`.
  - Unity console errors after Play Mode HQ QA: `0`.
  - Utility preview: `activeSprites=768`, `hqLike=20`.
  - Dual Blades Echo Matrix: all 8 Echoes at `+5`, `hqLike=161`.
  - Greatsword Echo Matrix: all 8 Echoes at `+5`, `hqLike=155`.
  - Dense Dual Blades Perf Matrix: `hits=18`, `suppressed=15`, `transient=113`, `ms=25.07`.
  - Evidence screenshots saved under `docs/orchestration/evidence/`.
- Notes:
  - The HQ texture sheet is now strong, but all-on Echo Matrix captures are intentionally overpacked. Direct play should tune normal-combat scale/noise before the next VFX expansion.

# 2026-07-22 Procedural Motif VFX Rework

- Purpose:
  - Implement the approved silhouette-board direction in the actual Unity VFX path.
  - Make Stopped, Execution, Shatter, Oblivion, and Ashen readable by distinct action motifs instead of circle/ring variants.
- Applied target:
  - `LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs`.
  - `docs/orchestration/evidence/2026-07-22-memory-echo-weapon-vfx-silhouette-board.png`.
- Changes:
  - Added procedural generated motif sprites for ornate clock seals, judgement stamps, shatter slams, torn void brands, and holy ash fire.
  - Rewired memory previews, normal Echoes, weapon-specific Echo layers, and utility Ultimate preview/support layers to use the motif sprites as primary silhouettes.
  - Kept rings/lines as supporting timing or field-read layers only.
- Commands / checks:
  - `dotnet build` from repo root: not applicable, returned `MSB1003` because no root solution/project file exists.
  - Unity editor state check.
  - Unity compilation error check.
  - Unity console error check.
  - Play Mode `DebugPreviewAllUtilityVfx()`.
  - Play Mode `DebugRunEchoMatrix(DualBlades)`.
  - Play Mode `DebugRunEchoMatrix(Greatsword)`.
- Results:
  - Unity editor state after QA: `Dev_Prototype_v1`, not playing, not compiling.
  - Unity compilation errors: `0`.
  - Unity console errors after Play Mode QA: `0`.
  - Utility VFX preview invoked successfully.
  - Dual Blades Echo Matrix direct run completed with all 8 Echoes at `+5`, `kills=31`, `storm=True`.
  - Greatsword Echo Matrix direct run completed with all 8 Echoes at `+5`, `kills=57`, `storm=True`.
  - Evidence screenshots saved under `docs/orchestration/evidence/`.
- Notes:
  - The all-on debug screenshots are intentionally overpacked; direct play should judge final scale/noise/timing.

# 2026-07-22 Memory / Echo / Ultimate Dopamine Rework

- Purpose:
  - Rework memories, normal Echoes, and Ultimate Echoes so the next review checks dopamine/payoff, not only baseline readability.
  - Preserve Dense Dual Blades performance after adding richer VFX layers.
- Applied target:
  - `LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs`.
- Changes:
  - Ashen memory gained a cracked guard plate/halo.
  - Ashen Echo gained counter-burst layers for normal density.
  - Oblivion memory +5 and Echo gained void erase burst layers.
  - Blood Blade Storm opening/climax gained shock ring, white-hot core, and orbit shard bursts.
  - Fracture Execution gained a ground sentence/verdict mark.
  - Stasis Hunt gained a larger clock burst and second-hand snap.
  - Ashen Oblivion gained shield plate, ash wall, void break, and guard-collapse layers.
  - Dense Dual Blades skips the newest non-essential Ashen/Oblivion ornament layer.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`.
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`.
  - Unity compilation error check.
  - Unity console error check.
  - `LETHE/V1 QA/Echo Matrix Dual Blades`.
  - `LETHE/V1 QA/Echo Matrix Greatsword`.
  - `LETHE/V1 QA/Passive Memory Matrix`.
  - `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`.
  - `LETHE/V1 QA/Utility Ultimate Matrix Greatsword`.
  - `LETHE/V1 QA/Dense Dual Blades Perf Matrix`.
- Results:
  - Runtime C# build passed with 7 existing legacy warnings and 0 errors.
  - Editor C# build passed with 0 warnings and 0 errors.
  - Unity compilation errors: `0`.
  - Unity console errors after final QA: `0`.
  - Echo Matrix Dual Blades: PASS, `total=1028`, `A=110`, `O=146`, `state=88`.
  - Echo Matrix Greatsword: PASS, `total=991`, `A=120`, `O=103`, `state=45`.
  - Passive Memory Matrix: PASS, `blood=17`, `ash=6`, `stopped=8`, `oblivion=60`.
  - Utility Ultimate Matrix Dual Blades: PASS, `fracture=28`, `stasis=11`, `ashen=47`.
  - Utility Ultimate Matrix Greatsword: PASS, `fracture=49`, `stasis=26`, `ashen=12`.
  - Dense Dual Blades Perf Matrix: first run failed at `transient=170`, `activeVfx=96`, `ms=146.61`; final run passed at `transient=51`, `activeVfx=38`, `ms=98.03`.
- Notes:
  - MCP polling intermittently returned `fetch failed`, but each QA result was confirmed through Unity console logs.
  - Direct play remains required to judge whether the new spectacle actually feels satisfying.

# 2026-07-22 Blood Repeat Fix / Remaining Echo VFX Plan

- Purpose:
  - Fix the player-facing issue where Blood Echo appeared once and then seemed to stop.
  - Preserve Echo Matrix and Dense Dual performance after restoring repeated Blood visibility.
  - Record remaining Echo / Ultimate VFX direction with a visual example board.
- Applied target:
  - `LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs`.
  - `docs/orchestration/evidence/2026-07-22-remaining-echo-vfx-plan.md`.
  - `docs/orchestration/evidence/2026-07-22-remaining-echo-vfx-concept-board.png`.
- Changes:
  - Greatsword Blood accent no longer skips VFX when the base hit kills the target first.
  - Dead-target Greatsword Blood is VFX-only.
  - Dense Dual Blades now spawns a lightweight repeated Blood pulse/suture read on the first allowed dense hit.
  - Removed dead fallback code guarded by `bloodLevel < 0`.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`.
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`.
  - Unity `AssetDatabase.Refresh()`.
  - Unity compilation error check.
  - Unity console error check.
  - `LETHE/V1 QA/Echo Matrix Greatsword`.
  - `LETHE/V1 QA/Echo Matrix Dual Blades`.
  - `LETHE/V1 QA/Dense Dual Blades Perf Matrix`.
- Results:
  - Runtime C# build passed with 7 existing legacy warnings and 0 errors.
  - Editor C# build passed with 7 existing legacy warnings and 0 errors.
  - Unity compilation errors: `0`.
  - Unity console errors after final QA: `0`.
  - Echo Matrix Greatsword: PASS, `total=991`, `B=303`, `stateH=1`, `stateSt=20`.
  - Echo Matrix Dual Blades: PASS, `total=1027`, `B=83`, `state=86`.
  - Dense Dual Blades Perf Matrix: PASS, `hits=18`, `suppressed=15`, `transient=46`, `activeVfx=33`, `ms=91.56`.
- Notes:
  - The Greatsword Blood count is higher because kill-hit Blood VFX is now intentionally visible.
  - MCP polling intermittently returned `fetch failed`, but PASS results were confirmed through Unity console logs.

# 2026-07-21 Blood / Stopped Dopamine VFX Pass

- Purpose:
  - Increase Greatsword Blood Echo's emotional payoff after jaewoo asked for a white/red circular slash-ring style.
  - Make Stopped Echo remain visible for the full frozen second and show a rotating second hand during the stop.
  - Preserve Dense Dual Blades performance after adding more visible Echo VFX.
- Applied target:
  - `LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs`.
- Changes:
  - Greatsword Blood Echo now spawns a procedural broken blood-vortex ring with separate white blade and red blood layers.
  - Greatsword Blood Echo hitstop/camera shake were increased.
  - Stopped Echo freeze is at least `1.0s`; clock field elements use held fade so they do not vanish during the stop.
  - Stopped Echo second hand is driven by `V1ClockHandSweep` and completes one full turn over the freeze window.
  - Dense Dual Blades suppresses extra dense-only Kalmuri/Blood decorations and the dense perf QA path now avoids replaying secondary-hit damage overhead.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`.
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`.
  - Unity `AssetDatabase.Refresh()`.
  - Unity compilation error check.
  - Unity console error check.
  - `LETHE/V1 QA/Dense Dual Blades Perf Matrix`.
  - `LETHE/V1 QA/Echo Matrix Dual Blades`.
  - `LETHE/V1 QA/Echo Matrix Greatsword`.
- Results:
  - Runtime C# build passed with 7 existing legacy warnings and 0 errors.
  - Editor C# build passed with 7 existing legacy warnings and 0 errors.
  - Unity compilation errors: `0`.
  - Unity console errors after final QA: `0`.
  - Dense Dual Blades Perf Matrix: PASS, `hits=18`, `suppressed=15`, `transient=36`, `activeVfx=15`, `ms=93.06`.
  - Echo Matrix Dual Blades: PASS, `total=1027`, `St=160`, `stateSt=11`.
  - Echo Matrix Greatsword: PASS, `total=779`, `B=87`, `St=168`, `stateSt=20`.
- Notes:
  - One MCP menu poll returned `fetch failed`, but the QA still executed and PASS was confirmed through Unity console logs.
  - Direct play remains required for taste: automated QA proves coverage/perf, not whether the vortex and clock actually feel dopamine-rich.

# 2026-07-21 Shatter Echo Ground Fracture Rework

- Purpose:
  - Move Shatter Echo away from generic ring/needle VFX and into a clear terrain/world fracture fantasy.
  - Preserve Dense Dual Blades performance after adding richer Shatter visuals.
- Applied target:
  - `LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs`.
- Changes:
  - Dual Blades Shatter now spawns chained ground cracks under targets outside dense throttle.
  - Greatsword Shatter now spawns a forward rupture spine, glow, core crack, branch cracks, shards, and ground breaks.
  - Dense Dual Blades suppresses extra Shatter/Ashen identity burst/link VFX while keeping state application and damage.
  - Dense perf matrix clears transient debug VFX before setup.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`.
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`.
  - Unity `AssetDatabase.Refresh()`.
  - Unity compilation error check.
  - `LETHE/V1 QA/Dense Dual Blades Perf Matrix`.
  - `LETHE/V1 QA/Echo Matrix Dual Blades`.
  - `LETHE/V1 QA/Echo Matrix Greatsword`.
- Results:
  - Runtime C# build passed with 7 existing legacy warnings and 0 errors.
  - Editor C# build passed with 0 warnings and 0 errors.
  - Unity compilation errors: `0`.
  - Dense Dual Blades Perf Matrix: PASS, `hits=18`, `suppressed=15`, `transient=97`, `activeVfx=81`, `ms=93.74`.
  - Echo Matrix Dual Blades: PASS, `total=1027`, `Sh=175`, `stateSh=12`.
  - Echo Matrix Greatsword: PASS on rerun, `total=742`, `Sh=144`, `stateSh=3`.
- Notes:
  - The first Dense Dual run failed after the richer Shatter pass; dense-only burst/link suppression restored the matrix to `ms=93.74`.
  - The first Greatsword Echo Matrix run failed from a zero-live-enemy setup snapshot; rerun passed with the expected Shatter counts.
  - MCP menu polling still intermittently returned `fetch failed`, but PASS/FAIL was confirmed through Unity console logs.

# 2026-07-21 Stopped / Hunter Readability Follow-up

- Purpose:
  - Make Dual Blades Stopped Echo visible.
  - Add the desired clock second-hand motion to Stopped Echo.
  - Remove the unwanted Greatsword Hunter fan/cone shape.
  - Increase Dual Blades Hunter blade readability.
- Applied target:
  - `LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs`.
- Changes:
  - Dual Blades Hunter blade visual height increased from `0.62` to `0.82`.
  - Dual Blades Hunter now spawns ricochet preview links/marks immediately, while the moving blades still fly afterward.
  - Greatsword Hunter no longer spawns `EchoGreat_HunterPiercePressureCone`.
  - Dual Blades Stopped Echo now spawns a small clock field, second-hand sweep, clock ticks, and stronger tick cut outside dense throttle.
  - Greatsword Stopped Echo now layers a larger second-hand sweep over the clock field.
  - Dense Dual Blades reduces the heaviest clockwork and ricochet preview extras.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`.
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`.
  - Unity `AssetDatabase.Refresh()`.
  - Unity compilation error check.
  - Unity console error check.
  - `LETHE/V1 QA/Dense Dual Blades Perf Matrix`.
  - `LETHE/V1 QA/Echo Matrix Dual Blades`.
  - `LETHE/V1 QA/Echo Matrix Greatsword`.
- Results:
  - Runtime C# build passed with 7 existing legacy warnings and 0 errors.
  - Editor C# build passed with 7 existing legacy warnings and 0 errors.
  - Unity compilation errors: `0`.
  - Unity console errors after final QA: `0`.
  - Dense Dual Blades Perf Matrix: PASS, `hits=18`, `suppressed=15`, `transient=139`, `activeVfx=82`, `ms=91.05`.
  - Echo Matrix Dual Blades: PASS, `total=946`, `H=175`, `St=160`, `stateH=19`, `stateSt=11`.
  - Echo Matrix Greatsword: PASS, `total=671`, `H=51`, `St=168`, `stateH=3`, `stateSt=19`.
- Notes:
  - The first post-change Dual Blades Matrix failed before Unity reloaded the latest script and before ricochet preview stabilization.
  - One Dense Dual run failed at `ms=113.26`; dense-only VFX throttling restored the final run to `ms=91.05`.

# 2026-07-21 Hunter Echo / Greatsword Blood Readability Pass

- Purpose:
  - Improve Greatsword Blood Echo readability after jaewoo reported it was hard to see.
  - Rework Hunter Echo into a weapon-specific action instead of another generic tracking VFX.
- Applied target:
  - `LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs`.
- Changes:
  - Greatsword Blood Echo now uses a larger blood-iaido crescent stack, impact zone, bloom, radial blood petals, longer wound cut, larger radius, and stronger hit feedback.
  - Dual Blades Hunter Echo now throws two green ricochet blades that bounce between enemies.
  - Greatsword Hunter Echo now throws one large green greatsword forward as a piercing area attack.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`.
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`.
  - Unity compilation error check.
  - Unity console error check.
  - `LETHE/V1 QA/Echo Matrix Dual Blades`.
  - `LETHE/V1 QA/Echo Matrix Greatsword`.
  - `LETHE/V1 QA/Dense Dual Blades Perf Matrix`.
- Results:
  - Runtime C# build passed with 0 warnings and 0 errors.
  - Editor C# build passed with 0 warnings and 0 errors.
  - Unity compilation errors: `0`.
  - Unity console errors: `0`.
  - Echo Matrix Dual Blades: PASS, `prefix=EchoDual_`, `total=802`, `K=8`, `H=136`, `state=82`.
  - Echo Matrix Greatsword: PASS, `prefix=EchoGreat_`, `total=500`, `K=8`, `B=31`, `H=30`, `state=51`.
  - Dense Dual Blades Perf Matrix: PASS, `hits=18`, `suppressed=15`, `transient=109`, `activeVfx=82`, `ms=87.70`.
- Notes:
  - MCP menu polling intermittently returned `fetch failed`, but each QA menu produced PASS lines in Unity console logs.
  - Direct play remains required for visual taste: the automated matrix proves coverage, not whether the new Echo identity feels good.

# 2026-07-21 Dual Blades Kalmuri Visibility Pass

- Purpose:
  - Improve readability after jaewoo reported that Dual Blades Kalmuri Echo looked too close to the basic Dual Blades attack color.
- Applied target:
  - `LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs`.
  - `LETHE/Assets/_dev/Data/Weapons/Weapon_DualBlades.asset`.
- Changes:
  - Dual Blades Kalmuri Hunger Echo now uses dark indigo, violet-blue, and blue-edge colors instead of mostly cyan/white.
  - Follow-up timing changed from `0.035/0.012` to `0.085/0.018` so the Kalmuri bite lands after the basic paired slash.
  - Greatsword Kalmuri colors/timing were kept stable.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`.
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`.
  - Unity `AssetDatabase.Refresh()`.
  - Unity compilation error check.
  - Unity console error check.
  - `LETHE/V1 QA/Dense Dual Blades Perf Matrix`.
  - `LETHE/V1 QA/Kalmuri Perf Matrix`.
  - `LETHE/V1 QA/Echo Matrix Dual Blades`.
  - `LETHE/V1 QA/Echo Matrix Greatsword`.
- Results:
  - Runtime C# build passed with 0 warnings and 0 errors on the final rerun.
  - Editor C# build passed with 7 existing legacy warnings and 0 errors.
  - Unity compilation errors: `0`.
  - Unity console errors: `0`.
  - Dense Dual Blades Perf Matrix: PASS, `hits=18`, `suppressed=15`, `transient=103`, `activeVfx=81`, `ms=87.49`.
  - Kalmuri Perf Matrix: PASS, `totalKalmuri=396`.
  - Echo Matrix Dual Blades: PASS, `prefix=EchoDual_`, `total=803`, `K=8`, `state=82`.
  - Echo Matrix Greatsword: PASS, `prefix=EchoGreat_`, `total=499`, `K=8`, `state=51`.
- Notes:
  - The first parallel C# build attempt hit a transient DLL write lock between runtime/editor builds; rerunning runtime build alone passed cleanly.
  - MCP polling still intermittently returned `fetch failed`, but QA results were confirmed through Unity console logs.
  - Direct play remains the real visual gate.

# 2026-07-21 Spatial Hash Unity QA Follow-up

- Purpose:
  - Complete the Unity QA that was pending after the spatial hash targeting optimization.
- Unity MCP state:
  - Project: `D:/LETHE_Prototype/LETHE`.
  - Port: `7890`.
  - Scene: `Dev_Prototype_v1`.
  - Unity: `6000.3.10f1`.
  - `isPlaying=false`, `isCompiling=false`, `sceneDirty=false`.
- Commands / checks:
  - Unity compilation error check.
  - Unity console error check.
  - `Lethe.PrototypeV1.Editor.V1SmokeTestMenu.QaDenseDualBladesPerfMatrix()`.
  - `Lethe.PrototypeV1.Editor.V1SmokeTestMenu.QaEchoMatrixDualBlades()`.
  - `Lethe.PrototypeV1.Editor.V1SmokeTestMenu.QaEchoMatrixGreatsword()`.
- Results:
  - Unity compilation errors: `0`.
  - Unity console errors: `0`.
  - Dense Dual Blades Perf Matrix: PASS, `hits=18`, `suppressed=15`, `transient=141`, `activeVfx=87`, `ms=43.11`.
  - Echo Matrix Dual Blades: PASS, `prefix=EchoDual_`, `total=803`, `state=82`.
  - Echo Matrix Greatsword: PASS, `prefix=EchoGreat_`, `total=501`, `state=53`.
- Notes:
  - MCP menu/code polling intermittently returned `fetch failed`, but the QA methods executed and PASS lines were confirmed in Unity console logs.
  - Direct play is still needed to judge whether target selection feels unchanged after the optimization.

# 2026-07-20 Spatial Hash Targeting Optimization

- Purpose:
  - Reduce dense-combat CPU/GC pressure from repeated full-list enemy scans and LINQ sorting in core targeting helpers.
- Applied target:
  - `LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs`.
  - Added per-frame enemy spatial hash grid and reusable target buffers.
  - Routed weapon targeting, weapon hit collection, Echo radius/cone/chain helpers, Void Priest healing, enemy separation, and live enemy counting through lower-allocation helpers.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`.
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`.
  - `npm run report`.
  - `npm.cmd run report:check`.
  - Unity MCP `unity_editor_state` attempted.
- Results:
  - Runtime build passed with 7 existing legacy deprecation warnings and 0 errors.
  - Editor build passed with 0 warnings and 0 errors.
  - Report generation passed.
  - Report unit heading check passed.
  - Unity MCP reported no detected Unity Editor instance, so Play Mode QA is pending.
- Notes:
  - Next Unity checks should run Dense Dual Blades Perf Matrix and both Echo Matrix QA menus, then direct-play dense packs for targeting feel regression.

# 2026-07-10 Project Thumbnail and Intro Key-Art Pass

- Purpose:
  - Add LETHE-feeling project thumbnail art and replace the in-game intro's flat procedural background with a proper key-art first screen.
- Applied target:
  - Imported `LETHE/Assets/_dev/Art/Sprites/UI/spr_lethe_project_thumbnail_01.png`.
  - Imported `LETHE/Assets/_dev/Art/Sprites/UI/spr_lethe_intro_background_01.png`.
  - Added both UI assets to `V1_ContentCatalog.asset`.
  - Updated `DrawLetheIntroOverlay()` to draw the generated intro background behind the weapon cards and use lighter glass panels.
- Commands / checks:
  - Unity `Assets/Refresh` through MCP.
  - Unity compilation error check on LETHE port `7890`.
  - Unity Play Mode entry and Game View screenshot capture.
  - Unity console error check after capture.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`.
- Results:
  - Unity compilation errors: `0`.
  - Unity console errors after Play Mode capture: `0`.
  - Runtime screenshot evidence: `LETHE/Assets/_dev/Evidence/lethe_intro_keyart_screen_20260710.png`.
  - Runtime build passed with 7 existing legacy deprecation warnings and 0 errors.
- Notes:
  - Direct play should now judge whether the first screen feels ceremonial enough and whether weapon cards are readable over the darker key art.

# 2026-07-09 Greatsword Blood Echo Double Crescent Scale Pass

- Purpose:
  - Respond to jaewoo feedback that the Greatsword Blood Echo crescent was too small and should appear at the end of the Greatsword range as two thin half-moons.
- Applied target:
  - Moved the blood iaido center to the Greatsword swing tip/end range.
  - Replaced the single crescent with two offset thin crescents: outer and inner.
  - Enlarged the damage radius around the crescent endpoint.
- Commands / checks:
  - Unity compilation error check on LETHE port `7890`.
  - Unity method `Lethe.PrototypeV1.Editor.V1SmokeTestMenu.QaEchoMatrixGreatsword()`.
  - Unity method `Lethe.PrototypeV1.Editor.V1SmokeTestMenu.QaDenseDualBladesPerfMatrix()`.
- Results:
  - Unity compilation errors: `0`.
  - Echo Matrix Greatsword: PASS, `total=368`, `K=8`, `B=27`, `Ex=64`, `H=22`, `Sh=56`, `St=16`, `A=80`, `O=95`, `state=51`.
  - Dense Dual Blades Perf Matrix: PASS, `hits=18`, `suppressed=15`, `transient=139`, `activeVfx=75`, `ms=104.66`.
- Notes:
  - Direct play should check whether the two crescents now sit far enough out on the Greatsword range and read as a thin `((` slash rather than a small hit-center mark.

# 2026-07-09 Greatsword Blood Echo Crescent Follow-up

- Purpose:
  - Respond to jaewoo feedback that Greatsword Blood Echo still felt too close to dagger/dual-blade behavior.
  - Make Greatsword Blood Echo read as another Greatsword-style red half-moon slash after the weapon hit.
- Applied target:
  - Removed the Greatsword blood-thread harvest visual from the Echo accent branch.
  - Added `EchoGreat_BloodIaidoCrescent`, `EchoGreat_BloodIaidoAfterimage`, `EchoGreat_BloodIaidoEdge`, and a red impact-zone ring.
  - Greatsword Blood Echo now deals area damage around the crescent impact zone, applies Blood Mark, and adds hitstop/camera shake.
  - Dense Dual Blades utility routing now skips the heaviest execution branch in dense mode to restore frame budget.
- Commands / checks:
  - Unity compilation error check on LETHE port `7890`.
  - Unity method `Lethe.PrototypeV1.Editor.V1SmokeTestMenu.QaEchoMatrixGreatsword()`.
  - Unity method `Lethe.PrototypeV1.Editor.V1SmokeTestMenu.QaDenseDualBladesPerfMatrix()`.
- Results:
  - Unity compilation errors: `0`.
  - Echo Matrix Greatsword: PASS, `total=374`, `K=8`, `B=33`, `Ex=64`, `H=22`, `Sh=56`, `St=16`, `A=80`, `O=95`, `state=49`, `stateEx=2`, `stateH=5`, `stateSh=4`, `stateSt=19`, `stateA=12`, `stateO=7`.
  - Dense Dual Blades Perf Matrix: one regression run failed at `ms=111.50`; after dense routing reduction it passed with `hits=18`, `suppressed=15`, `transient=98`, `activeVfx=73`, `ms=74.56`.
- Notes:
  - Direct play should now judge whether Greatsword Blood Echo reads as a blood iaido/cleave follow-up rather than a stitch, thread, or dagger-like effect.

# 2026-07-09 Utility Echo Weapon-Identity Mechanics Correction

- Purpose:
  - Respond to jaewoo feedback that non-Kalmuri Echoes still felt like the same VFX and same effect on both weapons.
  - Raise the bar from weapon-specific accents to weapon-specific hit logic.
- Applied target:
  - Blood Greatsword: forward harvest arc, multi-target mark, blood-thread healing.
  - Blood Dual Blades: short stitch chains through nearby targets.
  - Shatter Greatsword: forward fissure/cone target selection.
  - Shatter Dual Blades: needle fracture chain.
  - Execution Greatsword: forward verdict cleave with stronger low-HP payoff.
  - Execution Dual Blades: repeated sentence cuts across a short chain.
  - Hunter Greatsword: one heavy piercing spear line.
  - Hunter Dual Blades: fast multi-target fan shots plus mark bites.
  - Stopped Greatsword: large clock-field freeze.
  - Stopped Dual Blades: short micro-stop chain.
  - Ashen Greatsword: player-centered bulwark/wave.
  - Ashen Dual Blades: enemy-side parry chain.
  - Oblivion Greatsword: collapse well.
  - Oblivion Dual Blades: brand stacks and hops.
  - Dense Dual Blades reduces Shatter/Execution/Ashen chains to one-target budget branches.
- Commands / checks:
  - Unity compilation error check on LETHE port `7890`.
  - Unity method `Lethe.PrototypeV1.Editor.V1SmokeTestMenu.QaEchoMatrixDualBlades()`.
  - Unity method `Lethe.PrototypeV1.Editor.V1SmokeTestMenu.QaEchoMatrixGreatsword()`.
  - Unity method `Lethe.PrototypeV1.Editor.V1SmokeTestMenu.QaDenseDualBladesPerfMatrix()`.
- Results:
  - Unity compilation errors: `0`.
  - Echo Matrix Dual Blades: PASS, `total=404`, `K=8`, `B=53`, `Ex=99`, `H=64`, `Sh=45`, `St=16`, `A=54`, `O=65`, `state=82`, `stateEx=11`, `stateH=13`, `stateSh=12`, `stateSt=11`, `stateA=20`, `stateO=15`.
  - Echo Matrix Greatsword: PASS, `total=376`, `K=8`, `B=34`, `Ex=64`, `H=22`, `Sh=56`, `St=16`, `A=80`, `O=96`, `state=52`, `stateEx=3`, `stateH=4`, `stateSh=5`, `stateSt=19`, `stateA=13`, `stateO=8`.
  - Dense Dual Blades Perf Matrix: first run failed at `ms=118.12`; after dense chain suppression it passed with `hits=18`, `suppressed=15`, `transient=109`, `activeVfx=75`, `ms=91.01`.
- Notes:
  - This is the correction pass for the prior utility Echo identity work. Direct play should now compare whether each Echo truly changes by weapon, not just whether it has a larger sprite.

# 2026-07-09 Utility Echo Weapon-Identity VFX / Judgment Pass

- Purpose:
  - Apply the earlier memory/Echo identity table to the non-Kalmuri utility Echoes.
  - Make Greatsword and Dual Blades Echoes use different VFX grammar and payoff rather than identical effects with different attack cadence.
  - Keep dense Dual Blades under the current perf budget while making normal combat reads stronger.
- Applied target:
  - Blood Echo: Greatsword adds a heavier drain-axis read; Dual Blades adds quick suture cuts.
  - Execution Echo: Greatsword adds guillotine-style judgment cuts; Dual Blades adds repeated chain/pip execution marks.
  - Hunter Echo: Greatsword adds spear-shadow pursuit; Dual Blades adds fan needles and target pips.
  - Shatter Echo: Greatsword adds fault-line fracture; Dual Blades adds needle ripples outside the dense branch.
  - Stopped Second Echo: Greatsword adds clock-hand cleaves; Dual Blades adds tick-cut micro pauses.
  - Ashen Shield Echo: Greatsword adds cracked bulwark impact; Dual Blades adds parry sparks outside the dense branch.
  - Oblivion Brand Echo: Greatsword adds collapse rings; Dual Blades adds stack rings.
  - Hunter, Shatter, Stopped, Ashen, and Oblivion tuning received proc/radius/damage/control adjustments so weaker/passive memories have stronger Echo payoff.
- Commands / checks:
  - Unity compilation error check on LETHE port `7890`.
  - Unity method `Lethe.PrototypeV1.Editor.V1SmokeTestMenu.QaEchoMatrixDualBlades()`.
  - Unity method `Lethe.PrototypeV1.Editor.V1SmokeTestMenu.QaEchoMatrixGreatsword()`.
  - Unity method `Lethe.PrototypeV1.Editor.V1SmokeTestMenu.QaDenseDualBladesPerfMatrix()`.
  - Unity method `Lethe.PrototypeV1.Editor.V1SmokeTestMenu.QaPassiveMemoryMatrix()`.
  - Unity method `Lethe.PrototypeV1.Editor.V1SmokeTestMenu.QaUtilityUltimateMatrixDualBlades()`.
  - Unity method `Lethe.PrototypeV1.Editor.V1SmokeTestMenu.QaUtilityUltimateMatrixGreatsword()`.
- Results:
  - Unity compilation errors: `0`.
  - Echo Matrix Dual Blades: PASS, `total=363`, `K=8`, `B=40`, `Ex=88`, `H=64`, `Sh=32`, `St=16`, `A=56`, `O=59`, `state=91`, `stateEx=5`, `stateH=13`, `stateSh=23`, `stateSt=18`, `stateA=20`, `stateO=12`.
  - Echo Matrix Greatsword: PASS, `total=316`, `K=8`, `B=8`, `Ex=64`, `H=20`, `Sh=48`, `St=16`, `A=80`, `O=72`, `state=56`, `stateEx=6`, `stateH=2`, `stateSh=22`, `stateSt=12`, `stateA=11`, `stateO=3`.
  - Dense Dual Blades Perf Matrix: PASS, `hits=18`, `suppressed=15`, `transient=104`, `activeVfx=76`, `ms=94.36`.
  - Passive Memory Matrix: PASS, `blood=17`, `ash=6`, `stopped=8`, `oblivion=64`.
  - Utility Ultimate Matrix Dual Blades: PASS, `ultPrefix=UltDual_`, `fracture=28`, `stasis=11`, `ashen=47`.
  - Utility Ultimate Matrix Greatsword: PASS, `ultPrefix=UltGreat_`, `fracture=49`, `stasis=22`, `ashen=14`.
- Notes:
  - One intermediate Dense Dual Blades QA run failed after the first VFX expansion (`transient=185`, `ms=130.84`). The dense branch now suppresses Shatter needle ripple, Execution pip extras, and Ashen parry-spark extras, bringing the final run back under budget.
  - Direct play should now judge each Echo family one by one for taste: Greatsword should feel like fewer heavy verdicts, Dual Blades like rapid layered marks.

# 2026-07-09 Kalmuri Convergence Timing and Dual Blades Visibility Pass

- Purpose:
  - Respond to jaewoo feedback that Greatsword Kalmuri Echo is too fast and should visibly gather blades from the ring edge.
  - Make Dual Blades Kalmuri Echo more visible without reintroducing dense-combat frame spikes.
- Applied target:
  - Greatsword Kalmuri Echo ring-edge blade pulls, upper/lower jaws, and blue-edge trails now travel inward more slowly.
  - Greatsword supporting pool/ring/rift/jaw afterimages live longer so the convergence has a readable wind-up.
  - Dual Blades non-dense Kalmuri Echo has larger blue rift/core/pulse and two foreground blade glints.
  - Dense Dual Blades uses a lighter rift/core/pulse branch after a rerun failed at `ms=113.48`.
- Commands / checks:
  - Unity compilation error check on LETHE port `7890`.
  - Unity menu/method `LETHE/V1 QA/Dense Dual Blades Perf Matrix`.
  - Unity menu/method `LETHE/V1 QA/Echo Matrix Dual Blades`.
  - Unity menu/method `LETHE/V1 QA/Echo Matrix Greatsword`.
  - Unity menu/method `LETHE/V1 QA/Kalmuri Perf Matrix`.
- Results:
  - Unity compilation errors: `0`.
  - Dense Dual Blades Perf Matrix: PASS, `hits=18`, `suppressed=12`, `transient=118`, `activeVfx=73`, `ms=87.43`.
  - Echo Matrix Dual Blades: PASS, `total=229`, `K=8`, `B=35`, `Ex=64`, `H=24`, `Sh=8`, `St=8`, `A=32`, `O=50`, `state=86`.
  - Echo Matrix Greatsword: PASS, `total=231`, `K=8`, `B=9`, `Ex=56`, `H=14`, `Sh=40`, `St=8`, `A=32`, `O=64`, `state=58`.
  - Kalmuri Perf Matrix: PASS. The meaningful visual-count run before the dense-only safety branch logged `totalKalmuri=396`; final runner snapshots after later QA logged `totalKalmuri=0` at the 2s check.
- Notes:
  - Direct play should now focus on whether Greatsword reads as blades gathering from the perimeter and whether Dual Blades Kalmuri is visible in normal packs.

# 2026-07-09 Weapon-Specific Echo VFX Readability Pass

- Purpose:
  - Respond to jaewoo feedback that Greatsword Echo VFX should feel bigger, Dual Blades Echo VFX should not be hidden by weapon slashes, and Kalmuri should be more visible.
  - Give the remaining Echo families clearer Greatsword vs Dual Blades personality.
- Applied target:
  - Kalmuri / Echo / Ultimate transient sprites now sort above ordinary weapon slash VFX.
  - Kalmuri Echo uses larger blade pulls, blue rifts, bite scars, and stronger trail alpha.
  - Dense Dual Blades Kalmuri keeps a reduced object count while retaining a visible blue pulse/scar read.
  - Greatsword utility Echoes use larger stamps, cleaves, domes, seals, fracture bursts, and heavy ring reads.
  - Dual Blades utility Echoes use brighter stacked cuts, smaller repeated marks, and clearer short links.
- Commands / checks:
  - Unity compilation error check on LETHE port `7890`.
  - Unity menu `LETHE/V1 QA/Kalmuri Perf Matrix`.
  - Unity menu `LETHE/V1 QA/Dense Dual Blades Perf Matrix`.
  - Unity menu `LETHE/V1 QA/Echo Matrix Dual Blades`.
  - Unity menu `LETHE/V1 QA/Echo Matrix Greatsword`.
- Results:
  - Unity compilation errors: `0`.
  - Kalmuri Perf Matrix: PASS, `orbit=44`, `bite=72`, `return=24`, `hunting=16`, `echoSurge=0`, `echoBarrage=0`, `totalKalmuri=420`.
  - Dense Dual Blades Perf Matrix: PASS, `hits=18`, `suppressed=15`, `transient=156`, `activeVfx=73`, `ms=99.50`.
  - Echo Matrix Dual Blades: PASS, `total=232`, `K=8`, `B=35`, `Ex=64`, `H=24`, `Sh=8`, `St=8`, `A=32`, `O=53`, `state=87`.
  - Echo Matrix Greatsword: PASS, `total=231`, `K=8`, `B=9`, `Ex=56`, `H=14`, `Sh=40`, `St=8`, `A=32`, `O=64`, `state=59`.
- Notes:
  - Automated QA confirms coverage and budget. Direct play still needs to judge whether Greatsword now feels heavy enough and whether Dual Blades Kalmuri remains visible inside dense weapon slashes.

# 2026-07-09 Intro Weapon Selection Screen

- Purpose:
  - Add a LETHE-style first screen that starts the run through weapon selection.
  - Preserve the existing start route while making the opening feel more atmospheric and intentional.
- Applied target:
  - First screen draws a dark river/memory-shard background.
  - Two cards present `절단쌍검` and `장송대검` with their rhythm and echo direction.
  - Card click and number keys `1` / `2` both start the run.
- Commands / checks:
  - Unity compilation error check on LETHE port `7890`.
  - Unity Play Mode initial state reflection check.
  - Direct `BeginRun(V1WeaponId.DualBlades)` reflection check.
  - Unity menu `LETHE/V1 QA/Start Dual Blades`.
  - Unity menu `LETHE/V1 QA/Start Greatsword`.
- Results:
  - Unity compilation errors: `0`.
  - Initial Play Mode state: `weaponSelectOverlay=True`, `runStarted=False`, `GameplayPaused=True`.
  - After selection call: `weaponSelectOverlay=False`, `runStarted=True`, `GameplayPaused=False`.
  - Start Dual Blades QA invoked successfully and produced a valid start snapshot.
  - Start Greatsword QA invoked but failed with `liveEnemies=2` against the current start-smoke expectation; this is tracked as a pre-existing QA/balance-smoke mismatch, not a compile or intro-start blocker.
- Notes:
  - Direct visual review is still needed because MCP reflection proves state flow, not whether the intro composition feels polished on the Game view.

# 2026-07-09 Kalmuri Blue Memory-Lineage VFX Pass

- Purpose:
  - Respond to jaewoo feedback that default Kalmuri Echo should preserve the original blue Hungry Blades memory tone.
  - Make the Echo feel more like detailed spectral blades rather than red wound circles.
- Applied target:
  - Default Kalmuri Echo palette moved to cyan/blue/white.
  - Dual Blades no longer spawns the large red/orange circle-style wound pool in default Kalmuri.
  - Inward bite pieces now use the Kalmuri blade sprite instead of generic diamond teeth.
  - Greatsword keeps a larger blue blade-wake ring and blade-jaw closure for heavy impact.
  - +5 awakened Kalmuri now uses blue spectral blade devour pulls.
- Commands / checks:
  - Unity compilation error check on LETHE port `7890`.
  - Unity menu `LETHE/V1 QA/Kalmuri Perf Matrix`.
  - Unity menu `LETHE/V1 QA/Dense Dual Blades Perf Matrix`.
  - Unity menu `LETHE/V1 QA/Echo Matrix Dual Blades`.
  - Unity menu `LETHE/V1 QA/Echo Matrix Greatsword`.
- Results:
  - Unity compilation errors: `0`.
  - Kalmuri Perf Matrix: PASS, `orbit=44`, `bite=72`, `return=24`, `hunting=16`, `echoSurge=0`, `echoBarrage=0`, `totalKalmuri=356`.
  - Dense Dual Blades Perf Matrix: PASS, `hits=18`, `suppressed=15`, `transient=120`, `activeVfx=58`, `ms=86.60`.
  - Echo Matrix Dual Blades: PASS, `total=232`, `K=8`, `state=86`.
  - Echo Matrix Greatsword: PASS, `total=207`, `K=8`, `state=58`.
- Notes:
  - Direct play should judge whether the new blue blade-pull read now clearly feels like the original Hungry Blades memory becoming an Echo.

# 2026-07-09 Kalmuri Default Hunger Echo Runtime

- Purpose:
  - Replace the default Kalmuri Echo feel after jaewoo chose the hunger-fit direction.
  - Stop default +5 Kalmuri from reading as an old detached flying blade.
- Applied target:
  - Default Kalmuri Echo now uses a weapon-trail scent pull into wound-devouring bites.
  - Dual Blades use fast inward teeth, short gnaw scars, and quick pack-bite VFX.
  - Greatsword uses a larger wound pool, jaw closure, drawn-to-wound trails, and heavy splinter scars.
  - +5 awakened Kalmuri now creates a wound-side devour bloom instead of calling the old awakened projectile.
- Commands / checks:
  - Unity compilation error check on LETHE port `7890`.
  - Unity menu `LETHE/V1 QA/Kalmuri Perf Matrix`.
  - Unity menu `LETHE/V1 QA/Dense Dual Blades Perf Matrix`.
  - Unity menu `LETHE/V1 QA/Echo Matrix Dual Blades`.
  - Unity menu `LETHE/V1 QA/Echo Matrix Greatsword`.
- Results:
  - Unity compilation errors: `0`.
  - Kalmuri Perf Matrix: PASS, `orbit=44`, `bite=72`, `return=24`, `hunting=16`, `echoSurge=0`, `echoBarrage=0`, `totalKalmuri=340`.
  - Dense Dual Blades Perf Matrix: PASS, `hits=18`, `suppressed=15`, `transient=134`, `activeVfx=52`, `ms=8.56`.
  - Echo Matrix Dual Blades: PASS, `total=231`, `K=8`, `state=87`.
  - Echo Matrix Greatsword: PASS, `total=207`, `K=8`, `state=58`.
- Notes:
  - `unity_execute_menu_item` intermittently returned MCP queue `fetch failed`; bridge ping stayed healthy and retry succeeded.
  - Direct play is still needed to judge whether the wound/scent/teeth language feels satisfying, not only budget-safe.

# 2026-07-08 Dual-Blade Kalmuri Red-Circle Read Fix

- Purpose:
  - Stop Dual Blades Kalmuri candidates from reading as red circles.
- Applied target:
  - Dual Blades Kalmuri candidates now use short wound slashes, tooth snaps, and small bite/shard marks instead of large red rings/discs.
  - Greatsword retains larger impact silhouettes.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - Unity compilation error check.
  - Unity console error check.
- Results:
  - Runtime build passed with existing legacy warnings and 0 errors.
  - Editor build passed with 0 warnings and 0 errors on sequential rerun.
  - Unity compilation errors: `0`.
  - Unity console errors: `0`.

# 2026-07-08 Kalmuri Hunger-Fit Candidate Rebuild

- Purpose:
  - Rebuild K1-K4 around `굶주린 칼무리` imagery instead of generic different-looking VFX.
- Applied target:
  - `K1`: wound feast / bite swarm.
  - `K2`: blood-scent hunt.
  - `K3`: feast table.
  - `K4`: chewed trail.
  - Dual Blades use faster pack-bite reads; Greatsword uses heavier wound/furrow reads.
  - Prototype hit rules now match the four candidate concepts.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - Unity compilation error check.
  - Unity console error check.
  - Unity menu `LETHE/V1 QA/Kalmuri Perf Matrix`.
- Results:
  - Runtime build passed with existing legacy warnings and 0 errors.
  - Editor build passed with 0 warnings and 0 errors on sequential rerun after an initial shared-DLL lock.
  - Unity compilation errors: `0`.
  - Unity console errors: `0`.
  - Kalmuri Perf Matrix: PASS, `totalKalmuri=268`.

# 2026-07-08 Kalmuri Prototype Legacy Projectile Suppression

- Purpose:
  - Remove the old flying awakened Kalmuri VFX from K1-K4 prototype review.
- Applied target:
  - Suppress `LaunchKalmuriBlade` while an F12 Kalmuri prototype mode is active.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - Unity compilation error check.
  - Unity console error check.
- Results:
  - Runtime build passed with existing legacy warnings and 0 errors.
  - Editor build passed with 0 warnings and 0 errors on sequential rerun after an initial shared-DLL lock.
  - Unity compilation errors: `0`.
  - Unity console errors: `0`.

# 2026-07-08 Kalmuri Echo Playable Prototypes

- Purpose:
  - Correct K1-K4 from cosmetic Kalmuri previews into playable Hungry Blades Echo prototype modes.
  - Let jaewoo judge concept, VFX, hit rules, and weapon fit in actual combat.
- Applied target:
  - F12 `K1` to `K4` now select a real Kalmuri Echo prototype.
  - Real weapon-hit Echo follow-ups route through the selected prototype.
  - `K1`: maw/wound bite hit area.
  - `K2`: ribbon/trail strip hit area.
  - `K3`: X/cross burst hit area.
  - `K4`: curse-mark network hit area.
  - Debug UI displays `K real echo prototype: K#`.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - Unity compilation error check.
  - Unity console error check.
  - Unity menu `LETHE/V1 QA/Kalmuri Perf Matrix`.
- Results:
  - Runtime build passed with 7 existing legacy warnings and 0 errors.
  - Editor build passed with 0 warnings and 0 errors on sequential rerun after an initial shared-DLL lock.
  - Unity compilation errors: `0`.
  - Unity console errors: `0`.
  - Kalmuri Perf Matrix: PASS, `totalKalmuri=268`.
- Debug instruction:
  - Start/play `Dev_Prototype_v1`, press `F12`, choose `K1` to `K4`, then attack enemies with Dual Blades and Greatsword to judge the actual Echo prototypes.

# 2026-07-08 Kalmuri VFX Hard Reset Preview

- Purpose:
  - Implement the requested Kalmuri hard-reset preview candidates and make `K1` to `K4` visually different for both Dual Blades and Greatsword.
- Applied target:
  - `K1`: wound mouth / saw-tooth scar.
  - `K2`: ribbon trail / burial banner.
  - `K3`: geometric X/cross burst.
  - `K4`: curse-mark seal plus chain/fork network.
  - Dual Blades and Greatsword now use different preview variants.
  - Preview label changed to `K Preview HARD RESET / HP 9999`.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - Unity compilation error check.
  - Unity console error check.
  - Unity menu `LETHE/V1 QA/Kalmuri Perf Matrix`.
- Results:
  - Runtime build passed with 7 existing legacy warnings and 0 errors.
  - Editor build passed with 0 warnings and 0 errors on sequential rerun after an initial shared-DLL lock.
  - Unity compilation errors: `0`.
  - Unity console errors: `0`.
  - Kalmuri Perf Matrix: PASS, `totalKalmuri=268`.
- Debug instruction:
  - Start/play `Dev_Prototype_v1`, press `F12`, and compare `K1` to `K4` with both Dual Blades and Greatsword.

# 2026-07-08 Kalmuri Preview High-HP Dummies

- Purpose:
  - Make Kalmuri concept previews easier to inspect after jaewoo reported that enemies died too quickly and the update might not be visible.
- Applied target:
  - K-preview enemies now spawn with HP `9999`.
  - K-preview damage is capped to `1`.
  - Added an on-screen `K Preview v2 / high HP dummies` label when pressing `K1` to `K4`.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - Unity `Assets/Refresh`
  - Unity compilation error check.
  - Unity console error check.
- Results:
  - Runtime build passed with 7 existing legacy warnings and 0 errors.
  - Editor build passed with 7 existing legacy warnings and 0 errors.
  - Unity compilation errors: `0`.
  - Unity console errors: `0`.
- Debug instruction:
  - If `K Preview v2 / high HP dummies` does not appear after pressing a K button, exit and re-enter Play Mode so Unity loads the latest script assembly.

# 2026-07-08 Kalmuri Concept Preview Readability Split

- Purpose:
  - Fix the first Kalmuri concept preview set after jaewoo noted that all four buttons looked almost identical.
  - Make each debug candidate visually separable at a glance before any final Kalmuri Echo implementation.
- Applied target:
  - `K1` wound-feast now uses red/orange bite ring, blood disc, inward teeth, and scar cuts.
  - `K2` trail-bloom now uses blue attack-ribbon afterimages and long delayed rip lines instead of the same blade swarm.
  - `K3` cross-swarm now uses purple/white radial cuts plus a clear X impact.
  - `K4` mark-frenzy now uses violet seal/ring/fork links and removes the extra bite blade from the fork read.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - Unity `Assets/Refresh`
  - Unity compilation error check.
  - Unity console error check.
- Results:
  - Runtime build passed with 7 existing legacy warnings and 0 errors; a parallel build attempt produced only a shared DLL lock, then sequential rerun confirmed no code errors.
  - Editor build passed with 0 warnings and 0 errors on sequential rerun.
  - Unity compilation errors: `0`.
  - Unity console errors: `0`.
- Debug instructions:
  - Start/play `Dev_Prototype_v1`.
  - Press `F12`.
  - Compare `K1` red wound-feast, `K2` blue trail-bloom, `K3` purple cross-swarm, and `K4` violet mark-frenzy.

# 2026-07-08 Kalmuri Concept Preview Debugger

- Purpose:
  - Let jaewoo compare four Kalmuri Echo concept candidates directly in the same build before choosing the final direction.
  - Avoid prematurely committing one design after the previous "single blade flies out" read still felt wrong.
- Applied target:
  - F12 debug panel now includes `K1`, `K2`, `K3`, and `K4`.
  - `K1`: wound-feast, multiple blades collapse into the hit wound.
  - `K2`: trail-bloom, attack trail afterimages multiply into delayed cuts.
  - `K3`: cross-swarm, blades spawn around the target and cross-cut inward.
  - `K4`: mark-frenzy, a hungry scar mark forks into nearby enemies.
  - Each button clears/rebuilds the same small enemy pack in front of the player for consistent visual comparison.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - Unity `Assets/Refresh`
  - Unity console error check.
- Results:
  - Runtime build passed with 0 warnings and 0 errors on sequential rerun.
  - Editor build passed with 7 existing legacy warnings and 0 errors.
  - Unity console errors: `0`.
  - Unity `EditorApplication.isCompiling` stayed `true` longer than expected after refresh, so direct in-editor button click testing is still pending.
- Debug instructions:
  - Start/play `Dev_Prototype_v1`.
  - Press `F12`.
  - Use `K1`/`K2`/`K3`/`K4` in the debug panel.
  - Compare shape, readability, and Kalmuri identity before choosing which concept becomes real gameplay.

# 2026-07-08 Kalmuri Wound-Reaction Correction

- Purpose:
  - Respond to jaewoo's design correction that Kalmuri Echo should not look like a new blade leaving the player body after an attack.
  - Reframe awakened Kalmuri as an echo reaction at the weapon impact/wound point, closer to Blood Reflection's target-side interaction.
- Applied target:
  - +5 Kalmuri wound-chain now starts from the struck enemy/wound position.
  - Added wound burst, wound scar, chain line, and wound-chain projectile naming.
  - Dense Dual Blades suppresses the expensive awakened wound-chain follow-up while preserving core Kalmuri hit behavior.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - Unity `Assets/Refresh`
  - Unity compilation error check.
  - Unity `LETHE/V1 QA/Echo Matrix Dual Blades`
  - Unity `LETHE/V1 QA/Echo Matrix Greatsword`
  - Unity `LETHE/V1 QA/Dense Dual Blades Perf Matrix`
- Results:
  - Runtime build passed with 7 existing legacy warnings and 0 errors.
  - Editor build passed with 0 warnings and 0 errors after sequential rerun; a previous parallel build attempt briefly locked the shared DLL.
  - Unity compilation errors: `0`.
  - Echo Matrix Dual Blades: `[V1QA] PASS`, `total=230`, `K=8`, `state=86`.
  - Echo Matrix Greatsword: `[V1QA] PASS`, `total=335`, `K=136`, `state=58`.
  - Dense Dual Blades Perf Matrix: `[V1QA] PASS`, `hits=18`, `suppressed=15`, `transient=87`, `activeVfx=25`, `ms=80.59`.
- Limitation:
  - Automated QA confirms object counts, coverage, and dense budget. Direct play should judge whether the wound-side Kalmuri chain now feels like a satisfying echo interaction rather than a detached projectile.

# 2026-07-08 Memory/Echo Kingmaker VFX and Judgment Pass

- Purpose:
  - Convert the approved memory/echo/ultimate reward design into playable VFX and hitbox behavior.
  - Reduce the gap where Blood Reflection felt like the only route with strong feedback and payoff.
  - Follow up jaewoo's concern that echoes should preserve weapon identity instead of feeling identical across Dual Blades and Greatsword.
- Applied target:
  - ExecutionFlash now has near-threshold forecast VFX and stronger Fracture Execution payoff.
  - HunterOath prioritizes higher-threat targets before distance.
  - ShatterWave gains cluster/boss fracture payoff.
  - StoppedSecond gains fracture-burst follow-up damage and VFX.
  - AshenShield stores prevented/echo guard charge and releases stored radial guard waves.
  - OblivionBrand gains +5 detonation/spread and stronger echo rupture/spread.
  - Fracture Execution, Stasis Hunt, and Ashen Oblivion were buffed so non-blood ultimates have stronger payoff.
  - Dense Dual Blades benchmark damage was lowered so the perf matrix measures hit/echo suppression instead of kill-chain aftermath.
  - Greatsword Kalmuri Echo was split from the shared clamp/bite follow-up into a heavy falling judgement blade and ground-rip pattern.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - Unity `Assets/Refresh`
  - Unity compilation error check.
  - Unity `LETHE/V1 QA/VFX Matrix`
  - Unity `LETHE/V1 QA/Echo Matrix Dual Blades`
  - Unity `LETHE/V1 QA/Echo Matrix Greatsword`
  - Unity `LETHE/V1 QA/Passive Memory Matrix`
  - Unity `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`
  - Unity `LETHE/V1 QA/Utility Ultimate Matrix Greatsword`
  - Unity `LETHE/V1 QA/Dense Dual Blades Perf Matrix`
- Results:
  - Runtime build passed with 7 existing legacy warnings and 0 errors.
  - Editor build passed with 7 existing legacy warnings and 0 errors.
  - Unity compilation errors: `0`.
  - VFX Matrix: `[V1QA] PASS`, `previewMemory=8`, `previewEcho=8`, `fracture=1`, `stasis=1`, `ashen=1`, `missing=`.
  - Echo Matrix Dual Blades: `[V1QA] PASS`, `total=226`, `state=78`.
  - Echo Matrix Greatsword: `[V1QA] PASS`, `total=207`, `state=57`.
  - Passive Memory Matrix: `[V1QA] PASS`, `blood=17`, `ash=6`, `stopped=8`, `oblivion=62`.
  - Utility Ultimate Matrix Dual Blades: `[V1QA] PASS`, `fracture=28`, `stasis=11`, `ashen=47`.
  - Utility Ultimate Matrix Greatsword: `[V1QA] PASS`, `fracture=49`, `stasis=22`, `ashen=14`.
  - Dense Dual Blades Perf Matrix: `[V1QA] PASS`, `hits=18`, `suppressed=15`, `transient=45`, `activeVfx=26`, `ms=57.58`.
  - Weapon identity follow-up:
    - Echo Matrix Greatsword: `[V1QA] PASS`, `total=335`, `K=136`, `state=58`.
    - Echo Matrix Dual Blades: `[V1QA] PASS`, `total=230`, `K=8`, `state=85`.
    - Dense Dual Blades Perf Matrix: `[V1QA] PASS`, `hits=18`, `suppressed=15`, `transient=94`, `activeVfx=30`, `ms=87.85`.
- Limitation:
  - MCP menu calls intermittently returned `Error polling queue: fetch failed`, but console logs confirmed the QA results after wait/retry.
  - Automated QA does not replace direct feel review for payoff, readability, and satisfaction.

# 2026-07-07 Gatekeeper Review HP / Impact VFX Pass

- Purpose:
  - Clarify jaewoo's feedback that the boss felt too weak: the real run boss is not HP `180`; the F6/F12 Boss debug path was using the compressed QA boss HP.
  - Make Gatekeeper attacks feel stronger through visible cast, falling/charging attack bodies, impact shock, cracks, and camera shake.
- Applied target:
  - Added a separate review-boss HP path for F6/F12 Boss: compressed QA still uses `FastBossHp = 180`, while Boss review jump now uses `DebugReviewBossHp = FirstBossHp = 2200`.
  - Kept normal run Gatekeeper HP unchanged at `2200 / 4200 / 7600 / 12800`.
  - Added Gatekeeper cast burst around the boss body: sigil, halo, blade spine, rupture lines, and target line.
  - Strengthened meteor fall and impact: higher falling body, hot trail, spawn flare, scorch, impact ring, debris, and stronger camera shake.
  - Strengthened cone and ring resolve: extra afterblade, edge snap lines, ground scars, inner flash, crack spokes, and shared stronger raid impact burst.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - Unity `Assets/Refresh`
  - Unity compilation error check.
  - Unity `LETHE/V1 QA/Gatekeeper Pattern Matrix`
  - Unity `LETHE/V1 QA/Gatekeeper Jump`
- Results:
  - Runtime build passed with 7 existing legacy warnings and 0 errors.
  - Editor build passed with 0 warnings and 0 errors.
  - Unity compilation errors: `0`.
  - Gatekeeper Pattern Matrix: `[V1QA] PASS`, `boss=4`, `meteor=20`, `cone=6`, `ring=3`.
  - Gatekeeper Jump: `[V1QA] PASS`, `boss=1`, `liveEnemies=15`.
- Limitation:
  - MCP menu calls intermittently returned `Error polling queue: fetch failed`, but console logs confirmed the QA menu results after retry/wait.

# 2026-07-07 Memory/Echo/Enemy Identity Pass

- Purpose:
  - Respond to jaewoo's feedback that memories, echoes, monsters, and bosses still lack distinctive identity and VFX strength.
  - Identify the immediate gap: many effects function, but the monster-facing state read is too similar across echo families, and enemy role markers are mostly static.
- Applied target:
  - Added `SpawnEchoIdentityBurst()` behind `MarkEnemyEchoState()` so each utility echo leaves a different short monster-state VFX:
    - ExecutionFlash: gold verdict diamond and crack lines.
    - HunterOath: green lock ring and needle line.
    - ShatterWave: cyan fracture core and fault lines.
    - StoppedSecond: gold clock clamp/ticks.
    - AshenShield: pale ward ring and shield shards.
    - OblivionBrand: purple brand seal and pip.
  - Passive memory hits now also apply matching enemy state marks for ExecutionFlash, ShatterWave, StoppedSecond, AshenShield, and OblivionBrand.
  - Hunter projectiles now mark targets on hit.
  - Enemy role markers now animate via `V1EnemyRoleMarker`.
  - VoidPriest, DriftingEye, SplitOne, and Gatekeeper now get extra role symbols so their role reads faster in dense fights.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - Unity `Assets/Refresh`
  - Unity compilation error check.
  - Unity `LETHE/V1 QA/Echo Matrix Dual Blades`
  - Unity `LETHE/V1 QA/Echo Matrix Greatsword`
  - Unity `LETHE/V1 QA/Passive Memory Matrix`
  - Unity `LETHE/V1 QA/Dense Dual Blades Perf Matrix`
  - Unity `LETHE/V1 QA/Gatekeeper Pattern Matrix`
- Results:
  - Runtime build standalone rerun passed with 0 warnings and 0 errors after a temporary parallel DLL lock.
  - Editor build passed with 7 existing legacy warnings and 0 errors.
  - Unity compilation errors: `0`.
  - Echo Matrix Dual Blades: `[V1QA] PASS`, `total=240`, `state=72`, all six utility state families present.
  - Echo Matrix Greatsword: `[V1QA] PASS`, `total=223`, `state=70`, all six utility state families present.
  - Passive Memory Matrix: `[V1QA] PASS`, `blood=17`, `ash=6`, `stopped=8`, `oblivion=37`.
  - Dense Dual Blades Perf Matrix: `[V1QA] PASS`, `hits=18`, `suppressed=15`, `transient=114`, `activeVfx=27`, `ms=104.35`.
  - Gatekeeper Pattern Matrix: `[V1QA] PASS`, `boss=4`, `meteor=20`, `cone=6`, `ring=3`.
- Limitation:
  - Dense Dual Blades remains within budget but closer to the `110ms` QA threshold after identity bursts. Direct play should verify whether the added identity VFX is worth the extra cost.

# 2026-07-07 Direct Feedback VFX Action Pass

- Purpose:
  - Respond to jaewoo's direct-play feedback that dual-blade VFX was not visible enough, Gatekeeper meteor/cone patterns looked like static red zones, player damage was hard to read, and Hungry Blades / Kalmuri orbit felt like wobble instead of circle-to-target hunting.
  - Follow-up: fix the remaining disconnect where orbiting Kalmuri blades and flying attack blades looked like unrelated VFX.
- Applied target:
  - Dual blades now spawn guaranteed lightweight slash cuts and a hit spark in normal density.
  - Dense dual-blade mode keeps only one cheap guaranteed cut and skips the extra spark, preserving responsiveness under crowd pressure.
  - Gatekeeper meteor tells now include a falling meteor body, fall trail, target shadow, debris burst, and impact cue.
  - Gatekeeper cone tells now include charging blade/edge lines before the cleave and a sweeping slash wave on resolve.
  - Boss hits on the player now add a red flash/ring, damage number, SFX, and small camera shake.
  - Hungry Blades / Kalmuri orbit now uses coherent same-direction circular lanes, an orbit guide ring, orbit-exit cue, lock line, and lunge toward the target.
  - Kalmuri now reserves a real orbit blade slot nearest the target direction, highlights that blade as the hunter, and launches the lunge from the same orbit endpoint.
  - Dense Dual Blades Perf Matrix now snapshots dense-specific transient counts so later gameplay does not contaminate the QA result.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - Unity `Assets/Refresh`
  - Unity compilation error check.
  - Unity `LETHE/V1 QA/Dense Dual Blades Perf Matrix`
  - Unity `LETHE/V1 QA/Gatekeeper Pattern Matrix`
  - Unity `LETHE/V1 QA/Kalmuri Perf Matrix`
- Results:
  - Runtime build passed with 7 existing legacy warnings and 0 errors.
  - Editor build passed with 7 existing legacy warnings and 0 errors.
  - Unity compilation errors: `0`.
  - Dense Dual Blades Perf Matrix: `[V1QA] PASS`, `hits=18`, `suppressed=15`, `transient=62`, `activeVfx=27`, `ms=85.10`.
  - Gatekeeper Pattern Matrix: `[V1QA] PASS`, `boss=4`, `meteor=20`, `cone=6`, `ring=3`.
  - Kalmuri Perf Matrix: `[V1QA] PASS`, `orbit=44`, `bite=72`, `return=24`, `hunting=16`, `totalKalmuri=268`.
  - Kalmuri orbit-to-lunge follow-up check: `[V1QA] PASS`, `orbit=44`, `bite=72`, `return=24`, `hunting=16`, `totalKalmuri=270`.
- Limitation:
  - Automated QA confirms object creation, perf budget, and compile health. Direct play still needs to judge whether the new meteor fall, cone slash, damage cue, and orbit-to-lunge motion feel good enough.

# 2026-07-06 MCP QA Recovery / Dense Dual-Blade Final Pass

- Purpose:
  - Resume Unity QA after AnkleBreaker Unity MCP recovered on `LETHE` port `7890`.
  - Finish pending Echo, Gatekeeper, Dense Dual-Blade, Kalmuri, VoidPriest, and M2 verification.
  - Fix the remaining Dense Dual-Blade Perf Matrix failure.
- Applied target:
  - Dense dual-blade weapon slash VFX now keeps only the primary slash in high-density fights.
  - Dense Kalmuri echo follow-ups now skip the support range ring, moving dive trails, and extra slash entries.
  - Dense Kalmuri echo uses a minimal clamp/rip read: two clamp blades plus one rip line.
  - Added generated sprite caching for repeated circle/ring/box/impact-diamond procedural sprites.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - Unity `Assets/Refresh`
  - Unity compilation error check.
  - Unity `LETHE/V1 QA/Echo Matrix Dual Blades`
  - Unity `LETHE/V1 QA/Echo Matrix Greatsword`
  - Unity `LETHE/V1 QA/Gatekeeper Pattern Matrix`
  - Unity `LETHE/V1 QA/Gatekeeper Jump`
  - Unity `LETHE/V1 QA/Dense Dual Blades Perf Matrix`
  - Unity `LETHE/V1 QA/Kalmuri Perf Matrix`
  - Unity `LETHE/V1 QA/Void Priest Heal Matrix`
  - Unity `LETHE/V1 QA/M2 Loop`
- Results:
  - Runtime build passed with 0 warnings and 0 errors.
  - Editor build passed with 0 warnings and 0 errors.
  - Unity compilation errors: `0`.
  - Unity console errors after final pass: `0`.
  - Echo Matrix Dual Blades: `[V1QA] PASS`, `total=240`, `state=73`.
  - Echo Matrix Greatsword: `[V1QA] PASS`, `total=223`, `state=69`.
  - Gatekeeper Pattern Matrix: `[V1QA] PASS`, `boss=4`, `meteor=20`, `cone=6`, `ring=3`.
  - Gatekeeper Jump: `[V1QA] PASS`, `boss=1`, `liveEnemies=15`.
  - Dense Dual Blades Perf Matrix:
    - before final fix: `[V1QA] FAIL`, `hits=24`, `suppressed=10`, `transient=711`, `activeVfx=0`, `ms=570.01`.
    - after final fix: `[V1QA] PASS`, `hits=30`, `suppressed=25`, `transient=64`, `activeVfx=42`, `ms=55.73`.
  - Kalmuri Perf Matrix: `[V1QA] PASS`, `totalKalmuri=0` at the 2s limit check.
  - Void Priest Heal Matrix: `[V1QA] PASS`, `attempts=12`, `accepted=4`, `vfx=16`.
  - M2 Loop: `[V1QA] PASS`, `hungryEcho=5`, `bloodEcho=5`.
- Limitation:
  - Automated QA is green. Final visual quality still needs direct play judgment for Kalmuri clamp/rip, Gatekeeper raid telegraph readability, and healer/separation feel.

# 2026-07-06 Kalmuri Echo Clamp/Rip Readability Pass

- Purpose:
  - Respond to jaewoo feedback that Hungry Blades / Kalmuri echo still does not visually match its concept.
  - Move the echo read away from generic cyan rings and toward a recognizable blade action.
- Applied target:
  - Added `SpawnKalmuriEchoClampRip()`.
  - Kalmuri echo now opens with two opposing clamp blades converging on the target point, then leaves a rip slash.
  - Reduced the old generic Kalmuri range/flash/cut alpha so it becomes support, not the main read.
  - Dense dual-blade Kalmuri follow-ups now skip the old surge blade loop and use the lighter clamp/rip shape to avoid adding object churn.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - AnkleBreaker Unity MCP reconnect attempt through `unity_execute_code`.
- Results:
  - Runtime build passed with 7 existing legacy warnings and 0 errors.
  - Editor build passed with 7 existing legacy warnings and 0 errors.
- Limitation:
  - Unity Kalmuri visual/perf QA rerun is pending because AnkleBreaker Unity MCP still returns `Transport closed`.

# 2026-07-06 Echo State Mark Readability Pass

- Purpose:
  - Respond to jaewoo feedback that non-blood memories/echoes do not clearly feel different in combat.
  - Make utility echoes leave readable monster-state marks instead of relying only on one-shot VFX bursts.
- Applied target:
  - Added `V1Enemy.ApplyEchoStateMark()`.
  - Added `V1EnemyStateBadge`, a short-lived rotating/pulsing marker attached to the affected enemy.
  - Added echo state tinting on enemies, blended with existing BloodMarked tint when both are present.
  - Added state marks for:
    - `ExecutionFlash`
    - `HunterOath`
    - `ShatterWave`
    - `StoppedSecond`
    - `AshenShield`
    - `OblivionBrand`
  - Updated Echo Matrix QA detail/pass criteria to count `EchoState_*` markers as well as existing echo VFX.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
- Results:
  - Runtime build passed with 0 warnings and 0 errors.
  - Editor build passed with 0 warnings and 0 errors.
- Limitation:
  - Unity Echo Matrix rerun is pending because AnkleBreaker Unity MCP still returns `Transport closed`.

# 2026-07-06 Gatekeeper Raid Telegraph Pass

- Purpose:
  - Respond to jaewoo feedback that Gatekeeper cone/circle patterns still do not feel like raid-game danger zones.
  - Make simple boss patterns read as `tell -> fill -> bang` instead of static red shapes.
- Applied target:
  - Added `V1RaidTelegraphFill`, a lightweight transient component that scales the danger-zone fill from small to full size over the warning window.
  - Gatekeeper meteor, cone, and ring tells now show:
    - full danger-zone boundary from the start,
    - growing internal red fill,
    - stronger outer lock/ring read,
    - short bright impact flash when damage resolves.
  - Gatekeeper warning duration now clamps to at least `0.92s`, so the fill has time to read.
  - Transient sprite pooling now disables stale `V1RaidTelegraphFill` components when reusing objects for non-raid VFX.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
- Results:
  - Runtime build passed with 7 existing legacy warnings and 0 errors.
  - Editor build first hit a parallel DLL lock, then standalone rerun passed with 0 warnings and 0 errors.
- Limitation:
  - Unity `LETHE/V1 QA/Gatekeeper Pattern Matrix` rerun is pending because AnkleBreaker Unity MCP still returns `Transport closed`.

# 2026-07-06 Dense Dual-Blade VFX Churn Pass

- Purpose:
  - Respond to jaewoo's report that dual blades feel laggy when many enemies are on screen.
  - Add a focused QA matrix that measures dense dual-blade hit count, suppressed echo hits, transient VFX count, active VFX count, and synchronous stress time.
  - Reduce dense-hit VFX churn without adding more spectacle.
- Applied target:
  - Added dense dual-blade throttles:
    - secondary dual-blade hits stop spawning full echo chains once live enemy count is high,
    - secondary dual-blade slash VFX is capped in dense waves,
    - dense Kalmuri follow-ups remember the dense state when scheduled and use a lighter follow-up shape,
    - dense Blood Reflection keeps the mark read but skips extra blood bloom/accent spam,
    - dense utility echoes rotate one utility family per primary hit instead of firing several families together.
  - Replaced `VoidPriest` heal target lookup with `FindVoidPriestHealTargets()` on the manager enemy list instead of whole-scene enemy scans.
  - Added `DebugRunDenseDualBladePerfMatrix()`.
  - Added Unity QA menu `LETHE/V1 QA/Dense Dual Blades Perf Matrix`.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - Unity `Assets/Refresh`
  - Unity compile error check before MCP transport closed.
  - Unity `LETHE/V1 QA/Dense Dual Blades Perf Matrix` baseline attempts before the final throttle pass.
- Results:
  - Runtime build passed with 7 existing legacy warnings and 0 errors.
  - Editor build passed with 7 existing legacy warnings and 0 errors.
  - Unity compile error count before the final throttle pass: `0`.
  - Dense matrix failing baseline before final throttle:
    - first measured fail: `hits=24`, `suppressed=4`, `transient=954`, `activeVfx=506`, `ms=947.64`.
    - first optimization fail: `hits=24`, `suppressed=10`, `transient=550`, `activeVfx=307`, `ms=561.77`.
    - second optimization fail: `hits=30`, `suppressed=15`, `transient=627`, `activeVfx=361`, `ms=644.39`.
  - After the final dense Kalmuri/Blood/utility throttle changes, local runtime/editor builds pass, but Unity QA rerun is blocked because the AnkleBreaker Unity MCP transport is closed while the `LETHE` Unity process remains alive.
- Limitation:
  - This pass has build verification and a failing baseline/progress trail, but it does not yet have a final Unity `[V1QA] PASS` line after the last throttle. Retry the Dense Dual Blades Perf Matrix when MCP routing is stable.

# 2026-07-06 Gatekeeper Jump Debug / Future Improvement Triage

- Purpose:
  - Let jaewoo skip directly to the first Gatekeeper from debug without waiting for the 150s timer.
  - Record the next concrete defects from direct play: low VFX identity, Kalmuri echo concept mismatch, and dual-blade dense-wave lag risk.
- Applied target:
  - `F6` now calls `DebugJumpToGatekeeper()` instead of only spawning a boss in the current state.
  - Start overlay also accepts `F6`, so the boss review can begin from the first screen.
  - F12 debug panel includes a `Boss` button.
  - `DebugJumpToGatekeeper()`:
    - ensures the run is started,
    - exits blocking overlays,
    - switches to fast debug boss values,
    - removes any existing Gatekeeper,
    - adds a small default review loadout if the run is empty,
    - spawns review enemies and one Gatekeeper.
  - Added `LETHE/V1 QA/Gatekeeper Jump`.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - Unity `Assets/Refresh`
  - Unity compile error check.
  - Unity `LETHE/V1 QA/Gatekeeper Jump`.
  - Unity console error check.
- Results:
  - Runtime build passed with 7 existing legacy warnings and 0 errors.
  - Editor build passed with 0 warnings and 0 errors.
  - Unity compile error count: `0`.
  - Gatekeeper Jump QA: `[V1QA] PASS`, `boss=1`, `liveEnemies=15`, `memories=[HungryBlades:3,BloodReflection:2,StoppedSecond:1]`.
  - Unity console error count after QA: `0`.
- Limitation:
  - This verifies the debug jump path and runtime safety. It does not solve the larger VFX identity or dense-wave performance issues; those are now the next implementation targets.

# 2026-07-06 Void Priest Heal / Interaction Audit Pass

- Purpose:
  - Respond to jaewoo feedback that stacked healers can make enemies feel impossible to kill without Blood Reflection.
  - Make healer behavior visible through VFX.
  - Begin a broader audit of memory/echo/ultimate versus monster interaction readability.
- Applied target:
  - `VoidPriest` heal cadence changed to `1.05s`.
  - Heal amount changed to `2.4`.
  - Heal target cap per priest pulse set to `3`.
  - Non-boss targets now have a `0.95s` priest-heal receiver lockout, preventing multiple priests from stacking full healing into the same target on the same beat.
  - Healers prioritize wounded nearby non-boss targets.
  - Added green heal VFX:
    - source pulse,
    - target pulse/core,
    - source-to-target heal thread,
    - floating heal amount.
  - Added `LETHE/V1 QA/Void Priest Heal Matrix`.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - Unity compile error check.
  - Unity `LETHE/V1 QA/Void Priest Heal Matrix`.
  - Unity `LETHE/V1 QA/M2 Loop`.
  - Unity `LETHE/V1 QA/Echo Matrix Dual Blades`.
  - Unity `LETHE/V1 QA/Passive Memory Matrix`.
  - Unity `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`.
- Results:
  - Runtime build passed with 7 existing legacy warnings and 0 errors.
  - Editor build passed with 0 warnings and 0 errors on standalone rerun.
  - Unity compile error count: `0`.
  - Void Priest Heal Matrix: `[V1QA] PASS`, `attempts=12`, `accepted=4`, `vfx=16`.
  - M2 Loop: `[V1QA] PASS`, `hungryEcho=5`, `bloodEcho=5`, `storm=True`.
  - Echo Matrix Dual Blades: `[V1QA] PASS`, `total=240`, `K=8`, `B=56`, `Ex=64`, `H=24`, `Sh=8`, `St=8`, `A=32`, `O=40`.
  - Passive Memory Matrix: `[V1QA] PASS`, `blood=17`, `ash=6`, `stopped=8`, `oblivion=36`.
  - Utility Ultimate Matrix Dual Blades: `[V1QA] PASS`, `fracture=22`, `stasis=9`, `ashen=47`.
- Limitation:
  - QA confirms safety and object presence. It also exposes count/readability imbalance, but it does not prove final feel. Direct play should now check whether non-blood memories/echoes feel meaningful against healer-supported waves.

# 2026-07-06 Gatekeeper Sprite Repair Pass

- Purpose:
  - Respond to jaewoo feedback that the boss visual had degraded into a low-effort blob/slime-like silhouette.
  - Restore Gatekeeper as an angular gate/mask boss with rank-specific body sprites.
- Applied target:
  - Replaced the previous generated first boss PNG with a sharper gate/mask sprite.
  - Added rank-specific boss body sprites:
    - `Assets/_dev/Art/Sprites/Enemies/Bosses/spr_boss_gatekeeper_01.png`
    - `Assets/_dev/Art/Sprites/Enemies/Bosses/spr_boss_gatekeeper_02.png`
    - `Assets/_dev/Art/Sprites/Enemies/Bosses/spr_boss_gatekeeper_03.png`
    - `Assets/_dev/Art/Sprites/Enemies/Bosses/spr_boss_gatekeeper_04.png`
  - Updated `V1GameManager.EnemySprite(Gatekeeper)` to load these authored PNGs before using procedural fallback.
  - Tightened the fallback procedural Gatekeeper body to stay angular and mask-like if asset loading fails.
- Commands / checks:
  - Unity `AssetDatabase.Refresh()`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - Unity compile error check.
  - Unity `LETHE/V1 QA/Gatekeeper Pattern Matrix`.
  - Unity `LETHE/V1 QA/M2 Loop`.
- Results:
  - Runtime build passed with 7 existing legacy warnings and 0 errors.
  - Editor build passed with 0 warnings and 0 errors on standalone rerun.
  - Unity compile error count: `0`.
  - Gatekeeper Pattern Matrix: `[V1QA] PASS`, `boss=4`, `meteor=15`, `cone=4`, `ring=3`.
  - M2 Loop: `[V1QA] PASS`, `hungryEcho=5`, `bloodEcho=5`, `storm=True`.
- Limitation:
  - Automated QA confirms asset import, code safety, and pattern runtime. Final boss visual quality still needs direct visual judgment in scene.

# 2026-07-06 Enemy Soft Separation Pass

- Purpose:
  - Respond to jaewoo's question about whether normal enemies should overlap into a single stacked mass.
  - Keep survivor-style crowd density while preventing enemies from visually collapsing into one unreadable blob.
- Applied target:
  - Added `V1GameManager.EnemySeparationForce(V1Enemy self)`.
  - Added a small movement-scaled separation nudge to `V1Enemy.Update()`.
  - Gatekeepers receive reduced separation, while normal enemies are pushed more strongly away from the boss.
  - DriftingEye keeps its ranged behavior but separates slightly while standing at casting range.
  - Added `LETHE/V1 QA/Enemy Separation Matrix`.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - Unity compile error check.
  - Unity `LETHE/V1 QA/Enemy Separation Matrix`.
  - Unity `LETHE/V1 QA/M2 Loop`.
- Results:
  - Runtime build passed with 0 warnings and 0 errors on standalone rerun.
  - Editor build passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: `0`.
  - Enemy Separation Matrix: `[V1QA] PASS`, overlap pairs `91 -> 4`.
  - M2 Loop: `[V1QA] PASS`, `hungryEcho=5`, `bloodEcho=5`, `storm=True`.
- Limitation:
  - Automated QA confirms separation pressure and runtime safety. Direct play still needs to judge whether the swarm feels dense enough without becoming visually muddy.

# 2026-07-06 Gatekeeper Heal Fix / Telegraph Pattern Pass

- Purpose:
  - Fix jaewoo's first-boss observation that the boss felt unkillable after add spawns.
  - Replace the previous vague boss pulse with readable red danger-zone attacks.
- Applied target:
  - `VoidPriest` healing now skips `Gatekeeper`.
  - Gatekeeper pattern rank now branches into meteor circle, cone slash, ring burst, and combined late-gate pressure.
  - Each pattern shows a red telegraph before delayed damage.
  - Gatekeeper fallback sprites now vary by rank with different color/sigil/crack shapes.
  - Added `LETHE/V1 QA/Gatekeeper Pattern Matrix`.
- Commands / checks:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`
  - `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`
  - Unity compile error check.
  - Unity `LETHE/V1 QA/Gatekeeper Pattern Matrix`.
  - Unity `LETHE/V1 QA/M2 Loop`.
  - Unity console error check.
- Results:
  - Runtime build passed with 0 warnings and 0 errors on standalone rerun.
  - Editor build passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: `0`.
  - Gatekeeper Pattern Matrix: `[V1QA] PASS`, `boss=4`, `meteor=15`, `cone=4`, `ring=3`.
  - M2 Loop: `[V1QA] PASS`, `hungryEcho=5`, `bloodEcho=5`, `storm=True`.
  - Unity console error count after QA: `0`.
- Limitation:
  - Automated QA confirms object creation and runtime safety. Jaewoo direct play still needs to judge whether the telegraphs are fair, readable, and exciting.

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
