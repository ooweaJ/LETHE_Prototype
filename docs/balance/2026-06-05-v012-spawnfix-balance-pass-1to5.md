# LETHE v0.12 Spawn-Fix Balance Pass 1-5

Generated: 2026-06-05

This pass reran the five balance surfaces after the first-boss spawn-order fix. The goal was to identify the next tuning target, not to declare the build balanced.

## Summary

| # | Surface | Sample | Verdict | Key result |
| ---: | --- | ---: | --- | --- |
| 1 | First-boss prelude survival/growth | 5 runs / 4 gameplay | `ITERATE_BALANCE` | death 50%, death-at median 159.8s, HP <= 20% median 137.76s |
| 2 | First boss TTK | 2 gameplay partial + 1 smoke | `BLOCKED_BY_PRELUDE` | deaths at 163-179s before boss damage/TTK sample |
| 3A | Twin blades + hungry blades | 3 gameplay | `ITERATE_BALANCE` | death 33.3%, no first-boss clear sample |
| 3B | Greatsword + execution flash | 1 gameplay partial | `PARTIAL_SIGNAL` | survived to 190s; first boss spawned at 180.02s and took 647.87 damage in 10s |
| 4 | Post-loss fast gate | 1 browser gate | `PASS_FAST_GATE` | challenge survived and refill completed |
| 5 | Full-run clear rate | 1 gameplay partial | `BLOCKED_BY_PRELUDE` | death 178.35s in `망각 전조`; full clear not measurable |

## 1. First-Boss Prelude Survival / Growth

- Evidence: `docs/balance/2026-06-05-loop-01-preboss-spawnfix.md`
- Command shape: `run_browser_balance_qa --runs 5 --run-sec 190 --steps-per-tick default`
- Gameplay runs: 4
- Browser success rate: 80%
- Death rate: 50%
- Death-at median: 159.8s
- Level-ups before first boss median: 10
- HP <= 40% median: 113.55s
- HP <= 20% median: 137.76s
- Death phase counts: `{"망각 전조":2}`

Conclusion: growth pace passes, but HP collapses too early during `망각 전조`.

## 2. First Boss TTK

- Smoke evidence: `docs/balance/2026-06-05-loop-02-first-boss-spawnfix-ttk-smoke-slowstep.md`
- Partial output: `alpha_test/outputs/balance/loop-02-first-boss-spawnfix-ttk-slowstep/`
- Stable command shape used for partial sample: `--run-sec 230 --steps-per-tick 30`
- Gameplay partials:
  - run 01: death 167.57s, `망각 전조`, HP <= 20% at 151.38s, no boss fight
  - run 02: death 163.45s, `망각 전조`, HP <= 20% at 152.43s, no boss fight
- Smoke sample:
  - death 179.32s, `망각 전조`, HP <= 20% at 165.62s, no boss fight

Conclusion: first boss TTK is still blocked because most runs die just before the 180s boss gate.

## 3. Weapon / Starting Memory Parity

### 3A. Twin Blades + Hungry Blades

- Evidence: `docs/balance/2026-06-05-loop-03a-twin-hungry-spawnfix.md`
- Runs: 3
- Browser success rate: 100%
- Death rate: 33.3%
- First boss clear rate: 0%
- Level-ups before first boss median: 10
- Top DPS share median: 42.1%

### 3B. Greatsword + Execution Flash

- Partial output: `alpha_test/outputs/balance/loop-03b-great-execution-spawnfix/`
- Gameplay sample: 1
- Browser errors: 1
- Result: running at 190.03s, no death
- First boss spawned at: 180.02s
- First boss damage by 190.03s: 647.87 / 780
- Main boss damage sources:
  - `execution_flash`: 476.4
  - `weapon`: 126
  - `stopped_second`: 45.47

Conclusion: greatsword + execution can reach and heavily damage the first boss if it survives to the gate. The next parity concern is less boss DPS and more prelude survival consistency.

## 4. Post-Loss Deficit Flow

- Command: `CHROME_PATH="C:\Program Files\Google\Chrome\Application\chrome.exe" npm run qa:postloss`
- Status: `complete`
- Challenge survived: true
- Refill completed: true
- Remaining HP: 180
- Active memory count during challenge: 2
- Active memory count after refill: 3

Conclusion: compressed post-loss gate still passes. This does not replace a real-duration 45/60/75s deficit balance pass.

## 5. Full-Run Clear Rate

- Partial output: `alpha_test/outputs/balance/loop-05-full-clear-spawnfix/`
- Gameplay sample: 1
- Result: death at 178.35s in `망각 전조`
- HP <= 40% at 125.03s
- HP <= 20% at 146.37s
- First boss clear: no
- Full clear: no

Conclusion: full-run clear rate is not yet a useful tuning target. The run fails before the first boss gate.

## Next Tuning Target

The next numeric balance target should be first-cycle `망각 전조` survival:

- target HP <= 40% median: move from 113-133s to 140s+
- target HP <= 20% median: move from 137-152s to 170s+
- target first boss reach rate: at least 70%
- then retune first boss HP/TTK to 15-30s after first-boss damage samples are stable

Do not tune full-run clear rate yet.
