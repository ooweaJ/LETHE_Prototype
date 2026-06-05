# LETHE v0.12 Balance Loop Matrix

Generated: 2026-06-05

This file summarizes separate balance loops for the five current balance surfaces. The goal is not to declare the build balanced; it is to show which surface fails first.

## Summary

| # | Surface | Command shape | Runs | Verdict | Key result |
| ---: | --- | --- | ---: | --- | --- |
| 1 | First-boss prelude survival/growth | `run_browser_balance_qa --runs 10 --run-sec 190` | 10 | `ITERATE_BALANCE` | death 60%, first boss clear 0%, level-ups median 10 |
| 2 | First boss TTK | `run_browser_balance_qa --runs 5 --run-sec 230` | 5 | `ITERATE_BALANCE` | first boss clear 0%, so TTK not measurable |
| 3A | Weapon/memory parity: twin blades + hungry blades | `--weapon twin_blades --memory hungry_blades --runs 5 --run-sec 190` | 5 | `ITERATE_BALANCE` | first boss clear 20%, TTK 6.4s, death 60% |
| 3B | Weapon/memory parity: greatsword + execution flash | `--weapon greatsword --memory execution_flash --runs 5 --run-sec 190` | 5 | `ITERATE_BALANCE` | first boss clear 0%, death 100% |
| 4 | Post-loss deficit flow | `run_browser_pressure_qa --mode postloss --timeout-ms 90000` | 1 fast QA | `complete` | fast post-loss challenge survived and refill completed |
| 5 | Full-run clear rate | `run_browser_balance_qa --runs 5 --run-sec 608` | 5 | `ITERATE_BALANCE` | full clear 0%, death 80%, one browser error |

## 1. First-Boss Prelude Survival / Growth

- Evidence: `docs/balance/2026-06-05-loop-01-preboss-survival.md`
- Output: `alpha_test/outputs/balance/loop-01-preboss/summary.json`
- Runs: 10
- Death rate: 60%
- Death-at median: 118.38s
- First boss clear rate: 0%
- Pre-boss level-ups median: 10
- Slot-fill median: 15.02s
- Top DPS share median: 44.68%

Conclusion: growth and DPS concentration are acceptable, but the build still fails before or during the first boss prelude.

## 2. First Boss TTK

- Evidence: `docs/balance/2026-06-05-loop-02-first-boss-ttk.md`
- Output: `alpha_test/outputs/balance/loop-02-first-boss-ttk-rerun/summary.json`
- Runs: 5
- Death rate: 80%
- Death-at median: 120.66s
- First boss clear rate: 0%
- First boss TTK median: not measurable
- Pre-boss level-ups median: 10
- Top DPS share median: 46.37%

Conclusion: TTK cannot be tuned honestly yet because most runs fail before first boss defeat.

## 3. Weapon / Memory Parity

### 3A. Twin Blades + Hungry Blades

- Evidence: `docs/balance/2026-06-05-loop-03a-twin-hungry.md`
- Output: `alpha_test/outputs/balance/loop-03a-twin-hungry-190/summary.json`
- Runs: 5
- Death rate: 60%
- First boss clear rate: 20%
- First boss TTK median: 6.4s
- Pre-boss level-ups median: 10
- Top DPS share median: 49.13%

### 3B. Greatsword + Execution Flash

- Evidence: `docs/balance/2026-06-05-loop-03b-great-execution.md`
- Output: `alpha_test/outputs/balance/loop-03b-great-execution-190/summary.json`
- Runs: 5
- Death rate: 100%
- First boss clear rate: 0%
- First boss TTK median: not measurable
- Pre-boss level-ups median: 10
- Top DPS share median: 43.24%

Conclusion: weapon/start-memory parity is not solved. Twin blades + hungry blades can occasionally reach and burst the first boss too quickly, while greatsword + execution flash fails to reach first boss consistently.

## 4. Post-Loss Deficit Flow

- Command: `CHROME_PATH=... node scripts/run_browser_pressure_qa.js --mode postloss --timeout-ms 90000`
- Status: `complete`
- Challenge survived: true
- Refill completed: true
- Remaining HP: 150
- Active memory count during challenge: 2
- Active memory count after refill: 3

Conclusion: the compressed fast QA flow works. This is not yet a real 45/60/75-second deficit balance pass.

## 5. Full-Run Clear Rate

- Evidence: `docs/balance/2026-06-05-loop-05-full-clear.md`
- Output: `alpha_test/outputs/balance/loop-05-full-clear/summary.json`
- Runs: 5
- Browser success rate: 80%
- Death rate: 80%
- Full clear rate: 0%
- First boss clear rate: 0%
- Death-at median: 136.17s
- Pre-boss level-ups median: 10
- Top DPS share median: 40.95%

Conclusion: full-run clear rate cannot be tuned until first-boss prelude survival is stabilized.

## Current Priority Order

1. First-boss prelude survival must reach a stable first boss clear rate before tuning later surfaces.
2. Weapon/start-memory parity must be addressed early because greatsword starts are much less stable than twin-blade starts.
3. First boss TTK should be tuned only after the clear sample is large enough.
4. Post-loss real-duration balance still needs a non-fast loop.
5. Full-run clear-rate tuning is blocked by first-boss prelude failure.

## Follow-Up: Diagnostics + Greatsword Stabilization

Implemented after the split-surface matrix:

- Balance QA now preserves `deathPhase`, `deathEnemyCount`, `maxEnemies`, `pressureSegments`, `hpSamples`, `lowHpSamples`, and `bossPostCycleState`.
- Telemetry samples now include `hpRate`, `pressurePhase`, and `bossActive`.
- Greatsword basic attacks now cleave up to 3 additional enemies in the weapon arc with reduced damage and a small push.
- First-cycle spawn caps were added, then tightened:
  - initial cap pass: lull 34 / rising 44 / climax 52
  - tightened cap pass: lull 34 / rising 36 / climax 42

Follow-up evidence:

| Surface | Evidence | Result |
| --- | --- | --- |
| Greatsword + execution after cleave | `docs/balance/2026-06-05-loop-03b-great-execution-cleave.md` | death 60%, max enemies median 72 |
| Greatsword + execution after cleave + spawn cap | `docs/balance/2026-06-05-loop-03b-great-execution-cleave-cap.md` | death 40%, max enemies median 48, one browser error |
| Basic prelude after spawn cap | `docs/balance/2026-06-05-loop-01-preboss-diagnostics-cap.md` | death 40%, max enemies median 53 |
| First boss 230s after spawn cap | `docs/balance/2026-06-05-loop-02-first-boss-diagnostics-cap.md` | death 80%, max enemies median 47 |
| First boss 230s after tightened cap | `docs/balance/2026-06-05-loop-02-first-boss-diagnostics-cap2.md` | death 60%, max enemies median 42, browser success 60% |

Interpretation:

- Greatsword start stability improved materially: death rate moved from 100% to 40% in the best follow-up sample.
- First-boss clear rate is still 0% in the latest samples, so TTK is still blocked by pre-boss/boss-entry reliability.
- Tightening spawn caps too far risks lowering growth and increasing browser instability. The next pass should use the new `hpSamples` and `pressureSegments` rather than blindly lowering density again.
