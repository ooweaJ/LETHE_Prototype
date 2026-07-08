# Next Tasks

## 1. Direct-Play Kingmaker Route Review

- Priority: high
- Problem: automated QA is green after the 2026-07-08 memory/echo kingmaker pass, but jaewoo still needs to judge whether non-blood routes feel rewarding in actual play.
- Build:
  - Play `Dev_Prototype_v1` with dual blades and greatsword.
  - Use `F6` / F12 `Boss` to inspect the first Gatekeeper quickly; this path now uses HP `2200`, while compressed QA remains HP `180`.
  - Judge Execution forecast, Hunter threat targeting, Shatter cluster/boss fracture, Stopped fracture burst, Ashen stored guard release, Oblivion spread/detonation, and the three non-blood utility ultimates.
- Done:
  - Review returns concrete tune targets: keep, brighten, shrink, slow down, rebalance, or redesign.

## 2. Utility Echo Identity Tuning

- Priority: high
- Problem: Echo Matrix passes and Kalmuri now has a stronger Greatsword/Dual split, but most non-Kalmuri utility echoes still need the same weapon-personality audit.
- Build:
  - Compare ExecutionFlash, HunterOath, ShatterWave, StoppedSecond, AshenShield, and OblivionBrand in normal and dense fights.
  - For each echo, check whether Dual Blades reads as fast/multi-hit and Greatsword reads as heavy/few-hit, not just bigger numbers.
  - If too noisy, reduce only the dense subset first.
  - If too subtle, tune alpha/lifetime for one weak family at a time.
- Done:
  - jaewoo can name or at least visually separate each utility echo family without reading text.

## 3. Kalmuri Concept Pick Review

- Priority: high
- Problem: The latest wound-chain still risks reading as "one blade flies out." Four alternate Kalmuri concept previews now exist and need direct selection before final implementation.
- Build:
  - Press `F12` in Play Mode and click `K1`, `K2`, `K3`, `K4`.
  - `K1`: red/orange wound-feast.
  - `K2`: blue trail-bloom.
  - `K3`: purple/white cross-swarm.
  - `K4`: violet mark-frenzy.
  - Pick one winner or a hybrid, then Codex converts it into the actual Kalmuri Echo behavior.
- Done:
  - Jaewoo can name the chosen Kalmuri direction and why it reads best.

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
- Problem: Dense Dual Blades Perf Matrix passes at `57.58ms`, but real play still needs feel judgment because automated QA cannot prove input/visual smoothness.
- Build:
  - Play dense waves with dual blades, Kalmuri, Blood, and utility echoes.
  - If hitchy, reduce dense utility identity bursts before touching normal-density readability.
- Done:
  - Dense dual-blade combat feels responsive and still readable.

Completed sequence:

- 2026-07-08: memory/echo kingmaker VFX and judgment pass implemented; Execution, Hunter, Shatter, Stopped, Ashen, Oblivion, and non-blood utility ultimates now have stronger payoff behavior and QA coverage.
- 2026-07-08: Kalmuri concept preview readability split implemented; K1-K4 now use distinct colors/shapes instead of similar cyan blade swarms.
- 2026-07-08: Kalmuri concept preview debugger added; F12 now offers K1 wound-feast, K2 trail-bloom, K3 cross-swarm, and K4 mark-frenzy samples.
- 2026-07-08: awakened Kalmuri Echo corrected from player-body launch to wound-side echo reaction; Echo Matrix Dual/Great and Dense Dual QA passed.
- 2026-07-08: Kalmuri Echo weapon-identity split implemented; Greatsword Kalmuri now uses a heavy falling judgement blade while Dual Blades keeps the swarm/bite language.
- 2026-07-07: Gatekeeper review HP and impact VFX pass implemented; F6/F12 Boss now uses review HP `2200`, fast QA keeps `180`, and Gatekeeper Pattern/Jump QA passed.
- 2026-07-07: memory/echo/enemy identity pass implemented; utility echo identity bursts, passive memory state marks, animated enemy role symbols, and Gatekeeper sigil marker pass build/Unity QA.
- 2026-07-07: direct feedback VFX action pass implemented; dual-blade guaranteed slashes, Gatekeeper falling meteor/charge cleave, player damage cue, Kalmuri orbit-to-lunge, and Dense QA snapshot all pass build/Unity QA.
- 2026-07-07: Kalmuri orbit-to-lunge link tightened; the hunt lunge now starts from a reserved orbit blade endpoint and Kalmuri Perf Matrix still passes.
- 2026-07-06: MCP QA recovered on LETHE port `7890`; Echo, Gatekeeper, Dense Dual, Kalmuri, VoidPriest, and M2 QA passed after final dense optimization.
- 2026-07-06: Kalmuri echo clamp/rip visual redesign implemented and local builds passed.
- 2026-07-06: utility echo monster-state marks implemented and Echo Matrix QA passed for dual blades and greatsword.
- 2026-07-06: Gatekeeper raid telegraph fill/bang pass implemented and Pattern/Jump QA passed.
- 2026-07-06: dense dual-blade VFX churn throttles implemented; final matrix now passes at `hits=30`, `suppressed=25`, `transient=64`, `activeVfx=42`, `ms=55.73`.
- 2026-07-06: Gatekeeper jump debug implemented; `F6`, F12 `Boss`, and `LETHE/V1 QA/Gatekeeper Jump` work.
- 2026-07-06: VoidPriest heal stacking/readability pass implemented; heal matrix, M2, Echo Matrix Dual, Passive Memory Matrix, and Utility Ultimate Dual QA passed.
- 2026-07-06: Gatekeeper body visual repair implemented; four boss PNGs now load by rank, Pattern Matrix and M2 Loop QA passed.
- 2026-07-06: enemy soft separation implemented; Enemy Separation Matrix and M2 Loop QA passed.
- 2026-07-06: Gatekeeper heal exclusion and telegraphed boss-pattern pass implemented; Pattern Matrix and M2 Loop QA passed.
- 2026-07-02: direct-play review checklist prepared at `docs/orchestration/review_prompts/2026-07-02-dev-prototype-v1-direct-play-review.md`.

QA menus passing:

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
