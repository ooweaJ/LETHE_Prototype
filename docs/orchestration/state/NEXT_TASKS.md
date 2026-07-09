# Next Tasks

## 1. Kalmuri Convergence Direct-Play Review

- Priority: urgent
- Problem: Greatsword Kalmuri Echo timing and Dual Blades Kalmuri visibility were just retuned. Automated QA passes, but only direct play can prove whether the slower ring-edge convergence reads correctly.
- Build:
  - Play `Dev_Prototype_v1` with Greatsword and Hungry Blades +5.
  - Watch whether blades visibly gather from the outer ring before impact.
  - Play Dual Blades with Hungry Blades +5 in normal packs and dense packs.
  - Judge whether the blue Kalmuri Echo remains visible without becoming visual noise.
- Done:
  - jaewoo can say keep, slow further, speed back up, brighten, enlarge, or reduce dense-only effects.

## 2. Weapon-Specific Echo Direct-Play Review

- Priority: urgent
- Problem: Greatsword and Dual Blades Echo VFX were just rescaled and separated. Automated QA passes, but jaewoo needs to judge whether the feel is actually better in motion.
- Build:
  - Play `Dev_Prototype_v1` with both weapons.
  - Check Kalmuri +5 with Dual Blades in dense packs.
  - Check Kalmuri +5 with Greatsword against small packs and boss-review targets.
  - Compare Blood, Execution, Hunter, Shatter, Stopped, Ashen, and Oblivion Echoes by weapon.
  - Judge whether Greatsword feels heavier and whether Dual Blades effects remain readable under rapid slashes.
- Done:
  - jaewoo can name the exact Echo family that still feels too small, too hidden, or too noisy.

## 3. Intro Weapon Selection Direct-Play Review

- Priority: high
- Problem: the new first screen now has LETHE atmosphere and weapon cards, but MCP state checks cannot prove whether it feels polished in the actual Game view.
- Build:
  - Enter `Dev_Prototype_v1`.
  - Confirm the first screen shows before combat starts.
  - Compare whether `절단쌍검` and `장송대검` cards read clearly as the first real choice.
  - Start with click and with number keys `1` / `2`.
  - Note whether the intro needs stronger art, less text, a start-only flow, or a more ceremonial weapon pick.
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

## 5. Blue Kalmuri Echo Direct-Play Review

- Priority: high
- Problem: the default Kalmuri Echo now uses blue spectral blade pulls instead of red wound circles, but automated QA can only prove budget and object coverage. jaewoo still needs to judge whether it finally feels like the original Hungry Blades memory becoming an Echo.
- Build:
  - Play `Dev_Prototype_v1`.
  - Keep F12 Kalmuri prototype mode off so the UI shows `K real echo prototype: default`.
  - Test Hungry Blades +5 with Dual Blades in normal and dense packs.
  - Test Hungry Blades +5 with Greatsword against small packs and boss-review targets.
  - Judge whether the effect reads as blue spectral blades being pulled into the wound, not red circles or detached projectiles.
  - Tune intensity, timing, color, object count, or damage only after direct-play notes.
- Done:
  - jaewoo can say keep/tune/redesign for the blue default Kalmuri Echo and name the exact weak read if it still misses.

Completed sequence:

- 2026-07-09: Greatsword Kalmuri convergence slowed so blades gather from the ring edge; Dual Blades normal-pack Kalmuri visibility improved while dense branch stayed budget-safe.
- 2026-07-09: weapon-specific Echo VFX readability pass enlarged Greatsword Echoes, clarified Dual Blades Echoes, raised Kalmuri/Echo VFX sorting, and passed Kalmuri/Dense/Echo Matrix QA.
- 2026-07-09: LETHE-style intro weapon selection screen added; initial overlay state and selection state pass, Dual Blades start QA invokes successfully, Greatsword start QA mismatch is tracked separately.
- 2026-07-09: default Kalmuri Echo converted to the selected hunger hybrid; default route now uses weapon-trail scent pull, wound-devouring bite VFX, and +5 wound-side devour bloom. Kalmuri, Dense Dual, Echo Dual, and Echo Great QA passed.
- 2026-07-09: default Kalmuri Echo recolored and detailed as blue Hungry Blades memory-lineage VFX; bite pieces now use Kalmuri blade sprites and QA passed again.
- 2026-07-08: Kalmuri candidates rebuilt around hunger-fit imagery; K1 wound feast, K2 blood-scent hunt, K3 feast table, and K4 chewed trail replaced the previous ribbon/cross/curse directions.
- 2026-07-08: Kalmuri K1-K4 corrected from cosmetic previews into playable Echo prototype modes.
- 2026-07-08: Kalmuri old awakened projectile suppressed during prototype review.
- 2026-07-07: direct feedback VFX action pass implemented; dual-blade guaranteed slashes, Gatekeeper falling meteor/charge cleave, player damage cue, Kalmuri orbit-to-lunge, and Dense QA snapshot passed.

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
- `LETHE/V1 QA/Void Priest Heal Matrix`
- `LETHE/V1 QA/M2 Loop`

Keep this file short. Detailed history belongs in `docs/TASK.md`, `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.
