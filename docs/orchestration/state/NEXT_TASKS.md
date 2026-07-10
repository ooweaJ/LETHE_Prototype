# Next Tasks

## 1. Utility Echo Weapon-Identity Direct-Play Review

- Priority: urgent
- Problem: jaewoo reported that the previous weapon-specific pass still looked too similar because shared Echo prompt/ring bodies dominated the read. A follow-up pass now reduces those common bodies and makes the weapon silhouettes primary: Dual Blades use short needle/chain/tick VFX, while Greatsword uses large plate/lance/field/wall/crater reads. Automated checks pass, but direct play is still the real gate.
- Build:
  - Play `Dev_Prototype_v1` with both Greatsword and Dual Blades.
  - Check whether Blood no longer shows the same generic red pulse/mark on both weapons.
  - Compare Blood, Execution, Hunter, Shatter, Stopped, Ashen, and Oblivion Echoes one by one.
  - Judge whether Greatsword reads as fewer heavier actions: double blood crescent, forward fissure, execution plate/guillotine, hunter lance, clock judgement field, ashen wall, collapse crater.
  - Judge whether Dual Blades reads as rapid chained actions: blood stitches, shatter needle spray, execution sentence needles, hunter fan shots, micro-stops, parry needles, brand stack cuts.
  - Watch dense Dual Blades specifically for clutter or frame spikes after the budget-suppression branch.
- Done:
  - jaewoo can name the exact Echo family and weapon pair that should be kept, enlarged, reduced, recolored, sped up, slowed down, or redesigned.

## 2. Kalmuri Convergence Direct-Play Review

- Priority: urgent
- Problem: Greatsword Kalmuri Echo timing and Dual Blades Kalmuri visibility were retuned. Automated QA passes, but only direct play can prove whether the slower ring-edge convergence reads correctly.
- Build:
  - Play `Dev_Prototype_v1` with Greatsword and Hungry Blades +5.
  - Watch whether blades visibly gather from the outer ring before impact.
  - Play Dual Blades with Hungry Blades +5 in normal packs and dense packs.
  - Judge whether the blue Kalmuri Echo remains visible without becoming visual noise.
- Done:
  - jaewoo can say keep, slow further, speed back up, brighten, enlarge, or reduce dense-only effects.

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
