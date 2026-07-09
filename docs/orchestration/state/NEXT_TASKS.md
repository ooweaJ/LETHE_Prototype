# Next Tasks

## 1. Default Kalmuri Hunger Echo Direct-Play Review

- Priority: urgent
- Problem: the chosen K2/K1 hunger hybrid is now the default Kalmuri Echo runtime, but automated QA can only prove budget and object coverage. jaewoo still needs to judge combat feel.
- Build:
  - Play `Dev_Prototype_v1`.
  - Keep F12 Kalmuri prototype mode off so the UI shows `K real echo prototype: default`.
  - Test Hungry Blades +5 with Dual Blades in normal and dense packs.
  - Test Hungry Blades +5 with Greatsword against small packs and boss-review targets.
  - Judge whether the effect reads as weapon-trail scent pull into wound-devouring blades, not a detached projectile.
  - Tune intensity, timing, color, object count, or damage only after direct-play notes.
- Done:
  - jaewoo can say keep/tune/redesign for the default Kalmuri Echo and name the exact weak read if it still misses.

## 2. Direct-Play Kingmaker Route Review

- Priority: high
- Problem: automated QA is green after the memory/echo kingmaker pass, but jaewoo still needs to judge whether non-blood routes feel rewarding in actual play.
- Build:
  - Play `Dev_Prototype_v1` with Dual Blades and Greatsword.
  - Use `F6` / F12 `Boss` to inspect the first Gatekeeper quickly; this path uses review HP `2200`, while compressed QA keeps HP `180`.
  - Judge Execution forecast, Hunter threat targeting, Shatter cluster/boss fracture, Stopped fracture burst, Ashen stored guard release, Oblivion spread/detonation, and the three non-blood utility ultimates.
- Done:
  - Review returns concrete tune targets: keep, brighten, shrink, slow down, rebalance, or redesign.

## 3. Utility Echo Identity Tuning

- Priority: high
- Problem: most non-Kalmuri utility echoes still need the same weapon-personality audit now applied to Kalmuri.
- Build:
  - Compare ExecutionFlash, HunterOath, ShatterWave, StoppedSecond, AshenShield, and OblivionBrand in normal and dense fights.
  - For each echo, check whether Dual Blades reads as fast/multi-hit and Greatsword reads as heavy/few-hit.
  - If too noisy, reduce only the dense subset first.
  - If too subtle, tune alpha/lifetime for one weak family at a time.
- Done:
  - jaewoo can name or visually separate each utility echo family without reading text.

## 4. Gatekeeper Raid Telegraph Feel Tune

- Priority: high
- Problem: Gatekeeper pattern QA passes, but visual timing/fairness still needs player judgment.
- Build:
  - Review meteor, cone, and ring as `red zone -> fill/charge -> visible attack body -> bang`.
  - Tune only warning duration, fill alpha, impact flash, or danger-zone size if needed.
- Done:
  - The first boss feels readable, fair, and more like a simple raid encounter.

## 5. Dense Dual-Blade Feel Check

- Priority: high
- Problem: Dense Dual Blades Perf Matrix passes after the new Kalmuri VFX, but real play still needs feel judgment because automated QA cannot prove input/visual smoothness.
- Build:
  - Play dense waves with Dual Blades, Kalmuri, Blood, and utility echoes.
  - If hitchy, reduce dense utility identity bursts before touching normal-density readability.
- Done:
  - Dense dual-blade combat feels responsive and still readable.

Completed sequence:

- 2026-07-09: default Kalmuri Echo converted to the selected hunger hybrid; default route now uses weapon-trail scent pull, wound-devouring bite VFX, and +5 wound-side devour bloom. Kalmuri, Dense Dual, Echo Dual, and Echo Great QA passed.
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
