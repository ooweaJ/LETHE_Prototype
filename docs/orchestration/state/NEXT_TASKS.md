# Next Tasks

## 1. Echo Rule Identity Direct-Play Review

- Priority: urgent
- Problem: Shatter reads acceptably as ground fracture. Brand now has monster-head marks and Ashen memory now creates a scorched field, but only direct play can judge whether the identity finally lands.
- Build:
  - Check Brand: overhead mark appears above the monster before delayed erase/spread.
  - Check Ashen memory: scorched holy-ash field appears near threatened enemies and burns as a field, not a body-centered automatic hit.
  - Check Ashen field balance: frequency, radius, DoT, and visual noise.
  - Check Great Ashen: holy-wall lane, not broad cone or circle.
  - Check Dual Ashen: parry-return chain.
  - Check Oblivion Brand: inscription -> delayed erase -> delayed spread.
  - Check Execution: low-health verdict target and limited witnesses, not broad area damage.
  - Keep Shatter as the current reference if ground fracture still feels good.
- Done:
  - jaewoo can say whether Ashen, Brand, and Execution now feel mechanically distinct, and mark each `keep`, `tune`, or `redesign`.

## 2. Blood Direct-Play Regression Check

- Priority: urgent
- Problem: Blood Echo was reported as appearing once and then disappearing. The code now guarantees repeated kill-hit / dense reads, but direct play still needs to confirm the feel.
- Build:
  - Play Greatsword + Blood Echo in normal packs and confirm the white/red vortex appears on repeated attacks, including kill hits.
  - Play Dual Blades + Blood Echo in dense packs and confirm the smaller blood pulse/suture read repeats without becoming noisy.
- Done:
  - jaewoo can say whether Blood now feels repeated, too frequent, too large, or still missing.

## 3. VFX Budget / Noise Tune

- Priority: high
- Problem: The first dopamine pass temporarily failed Dense Dual Blades perf (`transient=170`, `activeVfx=96`, `ms=146.61`) before dense ornament throttling restored PASS.
- Build:
  - If direct play feels too noisy, tune alpha/lifetime/spawn counts rather than adding new mechanics.
  - Keep dense branches on core readable markers, not full ornament stacks.
- Done:
  - Dense Dual remains under budget and normal packs retain the stronger payoff.

## 4. Intro Weapon Selection Direct-Play Review

- Priority: high
- Problem: the first screen now uses generated LETHE key art behind the weapon cards, and Game View capture confirms it renders. Direct play still needs to judge whether the ceremony, contrast, and card readability feel good.
- Build:
  - Enter `Dev_Prototype_v1`.
  - Confirm the first screen shows the new dark river / memory shard / cyan-blue left and crimson right key art before combat starts.
  - Compare whether `절단쌍검` and `장송대검` cards read clearly as the first real choice.
  - Start with click and with number keys `1` / `2`.
  - Note whether the intro needs less text, stronger title treatment, a start-only flow, a weapon-pick animation, or more contrast behind the cards.
- Done:
  - jaewoo can say keep/tune/redesign for the intro and identify the exact weak read if it misses.

## 5. Greatsword Start-Smoke QA Fix

- Priority: high
- Problem: `LETHE/V1 QA/Start Greatsword` invokes the run but currently fails with `liveEnemies=2` against the current start-smoke expectation.
- Build:
  - Inspect `V1SmokeTestMenu.AdvanceStartSmoke` and the Greatsword start route.
  - Decide whether the QA expectation should change or the Greatsword start smoke should spawn/advance to the same threshold as Dual Blades.
  - Keep this separate from intro visuals.
- Done:
  - `LETHE/V1 QA/Start Greatsword` passes from a clean Play Mode session.

Completed sequence:

- 2026-07-23: Brand overhead / Ashen field identity follow-up implemented. Brand now spawns dedicated overhead markers on affected monsters, and Ashen memory creates a scorched holy-ash field with DoT ticks and guard-charge feed. Runtime/editor builds, Unity compile, Passive + Dual Echo Matrix smoke, delayed coroutine console check, and Dense Dual matrix call passed.
- 2026-07-23: Echo rule identity rework implemented. Ashen memory no longer body-AoEs and now stores guard/counter charge, Ashen releases target high-threat enemies, Great Ashen uses a holy-wall lane, Brand uses inscription -> delayed erase/spread, and Execution targets condemned low-health verdicts instead of broad area damage. Runtime/editor builds, Unity compile, Play Mode Dual/Great/Passive/Dense matrix calls, and delayed Brand console check passed.
- 2026-07-23: Echo hit readability follow-up implemented. Added per-victim hit-confirm VFX so source/path/victim reads are clearer, especially Ashen guard/counter paths. Runtime/editor builds, Unity compile, Play Mode Dual/Great matrix calls, and console error checks passed.
- 2026-07-23: Execution/Shatter/Ashen/Oblivion normal Echo redesign implemented. Generated a new LETHE-style Echo-only sprite atlas, cut 8 transparent weapon-specific Echo sprites, removed memory-motif reuse from these Echo paths, changed Greatsword Shatter/Execution/Ashen/Oblivion hit reads, and verified C# builds, Unity compile, Dual/Great matrix counts, and Dense Dual budget.
- 2026-07-22: Weapon-specific Echo mutation VFX pass implemented. HQ memory motifs now get separate Dual Blades and Greatsword Echo ornament layers for Blood, Shatter, Execution, Stopped, Ashen, and Oblivion. C# builds passed, Unity compilation errors were 0, Dual/Great matrix captures were saved, and Dense Dual stayed within budget.
- 2026-07-22: HQ bitmap VFX texture pass implemented. Blood Vortex, Stopped Clock, Execution Judgement, Shatter Slam, Oblivion Brand, and Ashen Holy Fire sprites were generated, chroma-keyed, imported as Unity sprites, wired into memory/Echo/Ultimate motif paths, and verified with C# builds plus Unity Play Mode captures. Console and compilation errors were 0.
- 2026-07-22: Procedural motif VFX rework implemented from the approved silhouette board. Stopped/Execution/Shatter/Oblivion/Ashen now have primary generated motif sprites, previews and weapon-specific Echoes use those motifs, Unity compile/console errors are 0, and preview/dual/great evidence screenshots were saved.
- 2026-07-22: Stopped field now freezes enemies entering the active space, stopped field/clock/dome VFX render above map and behind monsters, and Execution/Shatter/Oblivion/Ashen were reworked away from circle/ring reads into judgement stamps, ground slams, torn void brands, and holy ash fire. C# build and Unity compile passed; Passive Memory, Echo Dual, Utility Ultimate Dual/Great QA passed; Greatsword Echo was directly verified by manager object counts because MCP menu polling was unstable.
- 2026-07-22: Blood visibility regression fixed. Greatsword Blood now appears on kill hits as VFX-only, Dense Dual Blades gets repeated lightweight Blood marks, remaining Ashen/Oblivion/Ultimate VFX direction and concept board were documented, and C#/Unity QA passed.
- 2026-07-21: Blood / Stopped dopamine pass added a white/red Greatsword Blood vortex ring, stronger Blood hit feedback, 1-second held Stopped clock VFX, rotating second hand, and Dense Dual perf recovery. Runtime/editor builds, Unity compile, console error check, Dense Dual, Echo Dual, and Echo Great QA passed.
- 2026-07-21: Shatter Echo was reworked into terrain/world fracture. Dual Blades now uses chained ground cracks, Greatsword now uses a large forward rupture, and Dense Dual perf was restored by suppressing extra dense identity/link VFX.
- 2026-07-21: Stopped/Hunter follow-up made Dual Blades Stopped visible with clockwork/second-hand VFX, enlarged Dual Blades Hunter blades, removed the Greatsword Hunter cone sector, added ricochet preview marks, and restored Dense Dual perf to PASS.
- 2026-07-21: Greatsword Blood Echo readability increased with larger blood-iaido crescents, bloom, petals, and impact feedback. Hunter Echo was reworked so Dual Blades throw two green ricochet blades and Greatsword throws one green piercing greatsword. Runtime/editor builds and Dual/Great Echo Matrix plus Dense Dual QA passed.
- 2026-07-21: Dual Blades Kalmuri visibility pass recolored the Kalmuri Hunger Echo into dark indigo/violet-blue, delayed Dual Blades Kalmuri follow-up timing to `0.085/0.018`, and passed Dense Dual, Kalmuri, Echo Dual, and Echo Great QA.
- 2026-07-21: Unity MCP connected on port `7890`; spatial hash follow-up QA passed Dense Dual Blades Perf Matrix, Echo Matrix Dual Blades, and Echo Matrix Greatsword. Console and compilation errors were 0.
- 2026-07-20: spatial hash targeting optimization added for living-enemy range queries, weapon hit collection, Echo target helpers, Void Priest healing, enemy separation, and live enemy counting. Runtime and Editor C# builds passed; Unity Play Mode QA is pending because the editor was not detected.
- 2026-07-10: common Echo prompt/ring bodies were reduced so weapon-specific VFX become the primary read. Dual Blades now lean into needles/ticks/chains, while Greatsword leans into plates/lances/fields/walls/craters. C#/Unity checks pass and `Echo Matrix Dual Blades` menu execution succeeded.
- 2026-07-10: weapon-specific Echo runtime pass added stronger Dual Blades stitch/mark/fan/rune reads and Greatsword crescent/fissure/guillotine/clock/bulwark/crater reads. C# and Unity compilation pass.
- 2026-07-10: project thumbnail and in-game intro key art added. Wide intro background is wired into `DrawLetheIntroOverlay`, UI sprites are cataloged, Game View evidence captured, Unity/C# checks passed.
- 2026-07-09: Greatsword Blood Echo crescent moved to the swing range edge and split into two thin `((` crescents. Greatsword Echo Matrix and Dense Dual QA passed.
- 2026-07-09: Greatsword Blood Echo changed from thread/harvest behavior into red crescent blood-iaido follow-up damage. Greatsword Echo Matrix and Dense Dual QA passed.
- 2026-07-09: utility Echo weapon mechanics correction split Blood/Execution/Hunter/Shatter/Stopped/Ashen/Oblivion into actual Greatsword vs Dual Blades target logic. Echo Matrix and Dense Dual QA passed.
- 2026-07-09: utility Echo weapon-identity pass added distinct Greatsword/Dual Blades VFX and tuning for Blood, Execution, Hunter, Shatter, Stopped, Ashen, and Oblivion. Echo Matrix, Dense Dual, Passive Memory, and Utility Ultimate QA passed.
- 2026-07-09: Greatsword Kalmuri convergence slowed so blades gather from the ring edge; Dual Blades normal-pack Kalmuri visibility improved while dense branch stayed budget-safe.
- 2026-07-09: weapon-specific Echo VFX readability pass enlarged Greatsword Echoes, clarified Dual Blades Echoes, raised Kalmuri/Echo VFX sorting, and passed Kalmuri/Dense/Echo Matrix QA.
- 2026-07-09: LETHE-style intro weapon selection screen added; initial overlay state and selection state pass, Dual Blades start QA invokes successfully, Greatsword start QA mismatch is tracked separately.
- 2026-07-09: default Kalmuri Echo converted to the selected hunger hybrid; default route now uses weapon-trail scent pull, wound-devouring bite VFX, and +5 wound-side devour bloom. Kalmuri, Dense Dual, Echo Dual, and Echo Great QA passed.

QA menus currently passing:

- `LETHE/V1 QA/Gatekeeper Jump`
- `LETHE/V1 QA/Enemy Separation Matrix`
- `LETHE/V1 QA/Gatekeeper Pattern Matrix`
- `LETHE/V1 QA/Echo Matrix Dual Blades`
- `LETHE/V1 QA/Echo Matrix Greatsword`
- `LETHE/V1 QA/Passive Memory Matrix`
- `LETHE/V1 QA/Forget Resonance Flow`
- `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`
- `LETHE/V1 QA/Utility Ultimate Matrix Greatsword`
- `LETHE/V1 QA/Kalmuri Perf Matrix`
- `LETHE/V1 QA/Dense Dual Blades Perf Matrix`

Keep this file short. Detailed history belongs in `docs/TASK.md`, `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.
