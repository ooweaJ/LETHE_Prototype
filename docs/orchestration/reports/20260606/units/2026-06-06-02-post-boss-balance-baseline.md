# 2026-06-06-02 - Post-Boss Balance Baseline

## 1. Current build status

v0.12 now has an automated browser balance baseline: full `qa:balance` returns `GO_BALANCE_BASELINE`.

## 2. What changed today

- Fixed balance QA post-boss automation so the cycle result continue button is clicked before a zero-active-memory guard can stop the loop.
- Added post-boss spawn caps for deficit breath, deficit trial, and later-cycle default pressure.
- Extended the default browser balance QA run window from `608s` to `690s`.
- Changed first boss HP from `2800` to `2500`.

## 3. Test results and evidence

- `node --check src/game.js`: pass.
- `node --check scripts/run_boss_ttk_harness.js`: pass.
- `node --check scripts/run_browser_balance_qa.js`: pass.
- Boss-only HP `2500`: 5/5 accepted, TTK median `15.62s`, verdict `GO_BOSS_TTK_SAMPLE`.
- Browser `first_boss_ttk` HP `2500`: 3/3 accepted, TTK median `21.05s`, verdict `GO_BALANCE_BASELINE`.
- Full browser `qa:balance`: `GO_BALANCE_BASELINE`, first boss clear `100%`, full clear `40%`, death `60%`, first boss TTK median `22.24s`.
- Evidence: `docs/balance/2026-06-06-v012-boss-ttk-hp2500.md`, `docs/balance/2026-06-06-v012-browser-first-boss-ttk-hp2500.md`, `docs/balance/2026-06-06-v012-balance-qa.md`.

## 4. Decisions made

- Treat HP `2500` as the current first boss value.
- Treat post-boss/full-run automation as fixed enough for the current baseline gate.
- Do not call this human-ready without reviewer/GPT interpretation of the remaining death pattern.

## 5. Problems or risks

- Death remains `60%`, concentrated in deficit trial.
- One full clear sample had a first boss TTK outlier, so the median passes but variance remains visible.
- Automated baseline evidence is still not a substitute for human playtest feedback.
- `npm run report:discord:unit` was blocked by the approval reviewer because sending workspace reports/attachments to an external Discord webhook is treated as potential private data exfiltration in this Codex session.

## 6. GPT handoff summary

Emotion proxy is excluded. v0.12 now passes the automated browser balance baseline: first boss clear `100%`, full clear `40%`, first boss TTK median `22.24s`. The remaining question is design interpretation: deficit trial kills many AI runs, which may be acceptable tension or may be too punishing for first human tests.

## 7. Next Codex tasks

- Prepare a reviewer/GPT prompt asking whether deficit-trial death `60%` is acceptable for the current prototype gate.
- If the reviewer asks for another pass, tune deficit trial pressure or duration without changing first boss HP first.
- If accepted, proceed toward the next pre-human-test gate and reporting loop.
- From a trusted local terminal, run `npm run report:discord:unit` if Discord delivery is still required.

## 8. Portfolio notes

- Problem: First-boss TTK was stable, but full-run browser QA still failed from post-boss flow and an impossible final-boss time window.
- Direction: Fix QA progression, give the final scheduled boss enough time, and tune post-boss pressure separately from first boss HP.
- Action: Reordered QA interrupt handling, added post-boss caps, extended the QA run window, then set first boss HP to `2500`.
- Result: Automated browser balance baseline now passes with clear evidence and a narrowed remaining design question.
