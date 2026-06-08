# 2026-06-07-01 - v0.12 Human Playtest Package

## 1. Current build status

v0.12 is the current controlled human-test candidate. The latest automated balance baseline remains `GO_BALANCE_BASELINE`: first boss clear `80%`, full clear `60%`, death `40%`, first boss TTK median `25.79s`.

## 2. What changed today

- Updated `docs/HUMAN_PLAYTEST_GUIDE.md` for v0.12 controlled 5-8 player sessions.
- Updated `docs/PLAYTEST_NOTES.md` with v0.12 observation fields.
- Added `docs/playtest/2026-06-07-v012-human.md`.
- Updated `scripts/prepare_playtest_build.js` so the package includes `V012_HUMAN_PLAYTEST_SHEET.md`.

## 3. Test results and evidence

- `node --check scripts/prepare_playtest_build.js`: pass.
- `npm run playtest:package:dry`: pass, output target `dist\lethe-v0.12-playtest`.
- `npm run playtest:package`: pass, generated `dist\lethe-v0.12-playtest`.
- Package file check confirmed `HUMAN_PLAYTEST_GUIDE.md`, `PLAYTEST_NOTES_TEMPLATE.md`, `V012_HUMAN_PLAYTEST_SHEET.md`, `PLAYTEST_SUMMARY_README.md`, `README.md`, `index.html`, `style.css`, and `src/`.
- `npm run report`: pass.
- `npm run report:check`: pass.
- `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- `npm run report:discord:unit:dry`: pass.

## 4. Decisions made

- Accept the latest automated balance baseline as the current pre-human-test candidate.
- Stop blind numeric tuning until human evidence exists.
- Move the next work toward controlled human sessions and log summarization.

## 5. Problems or risks

- Human players may still find the `40%` death / post-loss pressure too sharp.
- AI balance QA is not human emotion evidence.
- The generated `dist/` package is ignored by git, so it must be regenerated with `npm run playtest:package` when needed.
- `npm run report:discord:unit` was blocked by the approval reviewer because sending workspace reports/attachments to an external Discord webhook is treated as potential private data exfiltration in this Codex session.

## 6. GPT handoff summary

Emotion proxy is excluded. v0.12 has an automated balance baseline and a packaged human-test candidate. Next judgment should come from 5-8 controlled human sessions focused on early fun, meaningful level-up choices, first forgetting reaction, deficit survival feel, refill motivation, and restart intent.

## 7. Next Codex tasks

- After sessions, place downloaded JSON logs in `playtest_logs/`.
- Run `npm run playtest:summary`.
- Send the generated human-test prompt through the planning pipeline before major tuning or Unity-transition decisions.

## 8. Portfolio notes

- Problem: v0.12 passed automated balance, but human-test materials still referenced the old v0.7 solo gate.
- Direction: Convert the automated baseline into a practical controlled playtest package.
- Action: Updated guide, notes, session sheet, and package generation.
- Result: `dist\lethe-v0.12-playtest` is generated and ready to open for controlled sessions.

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
