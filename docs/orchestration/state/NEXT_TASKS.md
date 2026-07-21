# Next Tasks

## 1. Echo / Memory Concept Direct-Play Review

- Priority: urgent
- Problem: Greatsword Blood and Hunter have now been reworked, but jaewoo correctly flagged that the remaining Echo families can still feel like size/color variants. The next review should judge the active memory fantasy and Echo fantasy together, not only VFX scale.
- Build:
  - Play `Dev_Prototype_v1` with both Greatsword and Dual Blades.
  - Confirm Greatsword Blood reads as a large red blood-iaido slash.
  - Confirm Hunter reads as two green bouncing Dual Blades versus one green thrown Greatsword pierce.
  - Keep Stopped Second mechanics for now, but judge whether it needs a premium clockwork VFX pass.
  - Re-evaluate Shatter, Ashen, and Oblivion from the memory concept upward:
    - Shatter should feel like fracture/terrain rupture, not just another needle or ring.
    - Ashen should feel like guard, sacrifice, or counter-pressure, not just a gray burst.
    - Oblivion should feel like erasure/brand spreading, not just purple size scaling.
  - Watch dense Dual Blades for clutter after ricochet blades enter the effect budget.
- Done:
  - jaewoo can name which Echo family is conceptually correct, which only needs VFX polish, and which needs a memory/Echo redesign.

## 2. Kalmuri / Dual Blades Visibility Direct-Play Review

- Priority: urgent
- Problem: Greatsword Kalmuri Echo timing was previously retuned, and Dual Blades Kalmuri has now been recolored/timed because it looked too close to the basic Dual Blades slash. Automated QA passes, but only direct play can prove whether the Echo now reads as a separate event.
- Build:
  - Play `Dev_Prototype_v1` with Greatsword and Hungry Blades +5.
  - Watch whether blades visibly gather from the outer ring before impact.
  - Play Dual Blades with Hungry Blades +5 in normal packs and dense packs.
  - Judge whether the new dark indigo/violet Dual Blades Kalmuri bite separates from the bright white/cyan basic slash.
  - Watch whether the slightly delayed follow-up feels readable rather than sluggish.
- Done:
  - jaewoo can say keep, slow further, speed back up, darken, brighten, enlarge, or reduce dense-only effects.

## 3. Intro Weapon Selection Direct-Play Review

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

## 4. Greatsword Start-Smoke QA Fix

- Priority: high
- Problem: `LETHE/V1 QA/Start Greatsword` invokes the run but currently fails with `liveEnemies=2` against the current start-smoke expectation.
- Build:
  - Inspect `V1SmokeTestMenu.AdvanceStartSmoke` and the Greatsword start route.
  - Decide whether the QA expectation should change or the Greatsword start smoke should spawn/advance to the same threshold as Dual Blades.
  - Keep this separate from intro visuals.
- Done:
  - `LETHE/V1 QA/Start Greatsword` passes from a clean Play Mode session.

Completed sequence:

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
