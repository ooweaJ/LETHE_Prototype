# 2026-06-06-03 - Deficit Trial Survival Tuning

## 1. Current build status

v0.12 remains `GO_BALANCE_BASELINE`. Death is now below the target, but full clear is exactly at the automated upper bound.

## 2. What changed today

- Set the next balance target: death `<= 40%`, full clear `35-80%`, first boss TTK `15-30s`.
- Changed deficit duration from `75s` to `60s`.
- Changed deficit trial cap from `22` to `16`.
- Changed pre-boss XP multiplier from `1.75` to `1.95`.
- Added refill recovery: HP floor `85%`, shield `18`.
- Strengthened survival stat and made low-HP survival choices more reliable.

## 3. Test results and evidence

- `node --check src/game.js`: pass.
- `node --check scripts/run_boss_ttk_harness.js`: pass.
- `node --check scripts/run_browser_balance_qa.js`: pass.
- Boss-only TTK: 5/5 accepted, TTK median `15.62s`, verdict `GO_BOSS_TTK_SAMPLE`.
- Browser `first_boss_ttk`: 3/3 accepted, TTK median `21.04s`, verdict `GO_BALANCE_BASELINE`.
- Full browser `qa:balance`: `GO_BALANCE_BASELINE`, first boss clear `100%`, full clear `80%`, death `20%`, first boss TTK median `27.84s`.
- Evidence: `docs/balance/2026-06-06-v012-boss-ttk-survival-choice.md`, `docs/balance/2026-06-06-v012-browser-first-boss-ttk-survival-choice.md`, `docs/balance/2026-06-06-v012-balance-qa.md`.

## 4. Decisions made

- Keep first boss HP at `2500`.
- Treat death `20%` as meeting the survival target.
- Do not do another blind balance pass until reviewer/GPT judges whether full clear `80%` is too forgiving.

## 5. Problems or risks

- Full clear is exactly at the automated upper bound `80%`.
- One run still had a slow first boss outlier, though the median is in target.
- AI proxy metrics still are not human emotion feedback.
- `npm run report:discord:unit` was blocked by the approval reviewer because sending workspace reports/attachments to an external Discord webhook is treated as potential private data exfiltration in this Codex session.

## 6. GPT handoff summary

Emotion proxy is excluded. The new target was death `<= 40%` while preserving full clear `35-80%` and first boss TTK `15-30s`. Latest full `qa:balance` passes with death `20%`, full clear `80%`, first boss TTK median `27.84s`. Reviewer should decide whether to accept this as pre-human-test baseline or reintroduce slight late pressure.

## 7. Next Codex tasks

- If reviewer accepts, proceed to the next pre-human-test/reporting gate.
- If reviewer says too forgiving, reintroduce a small amount of late pressure without changing first boss HP.
- Do not add new systems or expand scope.
- From a trusted local terminal, run `npm run report:discord:unit` if Discord delivery is still required.

## 8. Portfolio notes

- Problem: The previous automated baseline passed but killed `60%` of AI runs, mostly around post-boss pressure.
- Direction: Improve survival recovery and low-HP choice reliability instead of weakening the first boss.
- Action: Tuned deficit duration/cap, refill recovery, survival stat, and low-HP survival choices.
- Result: Death dropped to `20%` and full browser balance still passes, leaving a clear design judgment about difficulty ceiling.
