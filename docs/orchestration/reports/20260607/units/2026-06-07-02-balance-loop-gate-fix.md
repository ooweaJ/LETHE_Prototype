# 2026-06-07-02 - Balance Loop Gate Fix

## 1. Current build status

v0.12 is back to `GO_BALANCE_BASELINE` under the balance loop, not just one-off QA. Final loop result: first boss clear `100%`, full clear `60%`, death `40%`, first boss TTK median `20.73s`.

## 2. What changed today

- Ran local preflight before the loop.
- Fixed `scripts/run_balance_loop.js` so the default run window is `690s`, positional npm/PowerShell args work, and dry-run prints the real loop configuration.
- Added death rate max `<= 40%` to `scripts/run_browser_balance_qa.js`.
- Tuned first boss HP, deficit duration, enemy damage scaling, post-boss caps, and refill safety margin.

## 3. Test results and evidence

- `npm run autopilot:preflight:local`: 20 pass / 1 warn / 0 fail.
- `node --check src/game.js`: pass.
- `node --check scripts/run_browser_balance_qa.js`: pass.
- `node --check scripts/run_balance_loop.js`: pass.
- `npm run balance:loop:dry`: pass, shows `run-sec 690`.
- Browser `first_boss_ttk`: `GO_BALANCE_BASELINE`, 3/3 accepted, TTK median `18.61s`.
- Final `balance:loop`: `GO_BALANCE_BASELINE`, first boss clear `100%`, full clear `60%`, death `40%`, first boss TTK median `20.73s`.
- `npm run playtest:package`: pass, regenerated `dist\lethe-v0.12-playtest` after balance changes.
- `npm run report`: pass.
- `npm run report:check`: pass.
- `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- `npm run report:discord:unit:dry`: pass.
- Evidence: `docs/balance/2026-06-07-v012-browser-first-boss-ttk-loop-hp2050.md`, `docs/balance/2026-06-07-v012-balance-loop.md`, `docs/review_prompts/2026-06-07-balance-loop.md`.

## 4. Decisions made

- Treat `balance:loop` as the required source for balance QA.
- Do not allow death `60%` to pass as GO.
- Keep Discord actual send as required by AGENTS, but document approval-review blocks when this Codex session cannot send externally.

## 5. Problems or risks

- Five-run browser loops are still noisy; use loop evidence first, and consider 10-run confirmation before major claims.
- Death is exactly at the upper bound `40%`.
- Discord actual webhook delivery is blocked by the approval reviewer in this Codex session.

## 6. GPT handoff summary

Emotion proxy is excluded. The balance loop was corrected to use the v0.12 `690s` window and enforce death `<= 40%`. After tuning, the final loop passes with full clear `60%`, death `40%`, and first boss TTK median `20.73s`.

## 7. Next Codex tasks

- From a trusted local terminal, run `npm run report:discord:unit` if Discord delivery is required.

## 8. Portfolio notes

- Problem: One-off QA and loop QA were not aligned, and death `60%` could still pass the automated gate.
- Direction: Make the loop the balance source of truth and encode the death target directly.
- Action: Fixed loop args/defaults, added death gate, and retuned post-boss pressure.
- Result: The balance loop now passes on the intended criteria.
