# Next Tasks

## 1. Blood Direct-Play Regression Check

- Priority: urgent
- Problem: Blood Echo was reported as appearing once and then disappearing. The code now guarantees kill-hit Greatsword Blood VFX and dense Dual Blades lightweight repeated Blood marks, but this needs a human feel check.
- Build:
  - Play Greatsword + Blood Echo in normal packs and confirm the white/red vortex appears on repeated attacks, including kill hits.
  - Play Dual Blades + Blood Echo in dense packs and confirm the smaller blood pulse/suture read repeats without becoming noisy.
- Done:
  - jaewoo can say whether Blood now feels repeated, too frequent, too large, or still missing.

## 2. Ashen Echo Rework

- Priority: urgent
- Problem: Ashen still risks feeling like generic pale rings. It needs a guard/counter action fantasy.
- Build:
  - Use `docs/orchestration/evidence/2026-07-22-remaining-echo-vfx-concept-board.png` top-left as the visual target.
  - Dual Blades: enemy-side parry spark into two quick counter cuts.
  - Greatsword: ash shield wall cracks into a heavy counter wave.
- Done:
  - Ashen has a visible guard charge, break, and counter payoff for both weapons.

## 3. Oblivion Echo Rework

- Priority: high
- Problem: Oblivion still needs a concrete erase fantasy instead of another dark mark/ring.
- Build:
  - Use `docs/orchestration/evidence/2026-07-22-remaining-echo-vfx-concept-board.png` top-right as the visual target.
  - Dual Blades: brand hops between targets and leaves tiny void scars.
  - Greatsword: large brand stamp collapses into a dark erased crater.
- Done:
  - Oblivion reads as stamp -> spread -> erase, with separate Dual Blades and Greatsword silhouettes.

## 4. Ultimate Echo Dopamine Pass

- Priority: high
- Problem: jaewoo explicitly expects Ultimate Echoes to feel stronger than normal Echoes. Current normal Echoes are being raised first; Ultimate Echoes need a separate payoff layer once the baseline is set.
- Build:
  - Review the 4 utility Ultimate Echo routes.
  - Define each Ultimate's one-screen spectacle: screen-space tell, sound/stop moment, main hit, aftermath, and weapon-specific silhouette.
  - Keep gameplay scope unchanged; this is a VFX/action payoff pass, not a new ultimate system.
- Done:
  - Ultimate Echoes clearly exceed normal Echoes in ceremony, hit feedback, and aftermath while staying within dense performance budget.

## 5. Intro Weapon Selection Direct-Play Review

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

## 6. Greatsword Start-Smoke QA Fix

- Priority: high
- Problem: `LETHE/V1 QA/Start Greatsword` invokes the run but currently fails with `liveEnemies=2` against the current start-smoke expectation.
- Build:
  - Inspect `V1SmokeTestMenu.AdvanceStartSmoke` and the Greatsword start route.
  - Decide whether the QA expectation should change or the Greatsword start smoke should spawn/advance to the same threshold as Dual Blades.
  - Keep this separate from intro visuals.
- Done:
  - `LETHE/V1 QA/Start Greatsword` passes from a clean Play Mode session.

Completed sequence:

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
