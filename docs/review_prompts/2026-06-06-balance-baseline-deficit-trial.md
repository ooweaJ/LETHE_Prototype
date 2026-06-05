# 2026-06-06 v0.12 Balance Baseline Review Prompt

## Context

LETHE HTML prototype v0.12 now passes the automated browser balance baseline.

Codex changes in the latest unit:

- Fixed browser balance QA post-boss automation so the cycle result continue button is clicked before the zero-active-memory guard can stop the loop.
- Added post-boss spawn caps:
  - deficit breath: `16`,
  - deficit trial: `22`,
  - later-cycle default: `58`.
- Extended default `qa:balance` run window from `608s` to `690s`, because the scheduled final boss spawns at `600s`.
- Changed first boss HP from `2800` to `2500`.

## Evidence

- Boss-only HP `2500`:
  - 5/5 accepted,
  - TTK median `15.62s`,
  - verdict `GO_BOSS_TTK_SAMPLE`,
  - evidence: `docs/balance/2026-06-06-v012-boss-ttk-hp2500.md`.
- Browser `first_boss_ttk` HP `2500`:
  - 3/3 accepted,
  - TTK median `21.05s`,
  - verdict `GO_BALANCE_BASELINE`,
  - evidence: `docs/balance/2026-06-06-v012-browser-first-boss-ttk-hp2500.md`.
- Full browser `qa:balance`:
  - verdict `GO_BALANCE_BASELINE`,
  - first boss clear `100%`,
  - full clear `40%`,
  - death `60%`,
  - first boss TTK median `22.24s`,
  - level-ups before first boss median `10`,
  - evidence: `docs/balance/2026-06-06-v012-balance-qa.md`.

## Review Questions

1. Is a `60%` AI death rate concentrated in deficit trial acceptable as tension for the current HTML prototype gate, given the full clear target now passes at `40%`?
2. Should Codex do another balance pass before human playtest, or should the next step be pre-human-test polish/reporting?
3. If another pass is needed, should it tune deficit trial pressure/duration/recovery rather than changing first boss HP?

## Constraints

- Do not ask Codex to add meta progression, shop systems, final boss expansion, more memories, more active slots, multi-region structure, or new major systems.
- Do not treat AI proxy metrics as human emotion feedback.
- Keep first boss HP `2500` unless the review specifically argues that the first boss TTK target should change.

## Desired Output

- Decision: `ACCEPT_BASELINE_FOR_PRE_HUMAN_TEST` or `ITERATE_DEFICIT_TRIAL`.
- Short reasoning.
- If iterating, give 1-3 concrete Codex tasks scoped to existing HTML prototype systems.
