# Current Task

## Goal

Review the latest v0.12 balance-loop failure after the rejected cap-only tuning candidate and choose the next exactly-one small adjustment candidate.

## Why Now

The 2026-06-08 `npm run balance:loop` rerun returned `ITERATE_BALANCE`: full clear `20%`, death `60%`, first boss clear `100%`, first boss TTK median `26.42s`.

The first one-lever candidate, lowering `laterCycleClimax 46 -> 42`, was tested on 2026-06-09 and rejected. `npm run qa:balance` and `npm run balance:loop` both returned full clear `20%`, death `80%`, first boss clear `100%`; the code was reverted to the prior baseline. Human testing should pause until the balance gate is restored or the user explicitly accepts the risk.

The report/devlog migration is complete, so new work should now use:

- AI state: `docs/orchestration/state/`
- AI devlog: `docs/orchestration/devlog/YYYY-MM-DD.md`
- Human report: `docs/orchestration/reports/YYYYMMDD/index.md|html`
- Human report archive: `docs/orchestration/reports/index.html`
- Work-unit details: `docs/orchestration/reports/YYYYMMDD/units/`

## Done Criteria

- `docs/balance/2026-06-09-v012-balance-qa.md` has been reviewed.
- `docs/orchestration/review_prompts/2026-06-09-balance-loop.md` has been used as the planning handoff or summarized for the user.
- The rejected `laterCycleClimax 46 -> 42` candidate is not repeated as the next immediate adjustment.
- Exactly one new small balance adjustment candidate is selected before implementation.
- After any adjustment, `npm run qa:balance` or `npm run balance:loop` is rerun.
- Status, devlog, reports, and decision log are updated under `docs/orchestration/`.

## Related Files

- `docs/balance/2026-06-08-v012-balance-qa.md`
- `docs/balance/2026-06-09-v012-balance-qa.md`
- `docs/orchestration/review_prompts/2026-06-08-balance-loop.md`
- `docs/orchestration/review_prompts/2026-06-09-balance-loop.md`
- `docs/BALANCE_TABLE_v0_12.md`
- `docs/LETHE_v0.12_밸런스_개선_제안서.md`
- `scripts/run_balance_loop.js`
- `scripts/run_browser_balance_qa.js`

## Verification Commands

```bash
npm run qa:balance
npm run balance:loop
npm run report
npm run report:check
npm run report:discord:unit:dry
npm run doctor
```

## Open Questions

- Why did lowering post-cycle climax cap fail to reduce death rate?
- Is the remaining failure driven by refill transition, deficit pressure, early HP erosion before the second cycle, or enemy damage scaling rather than raw climax count?
- Can one small non-density adjustment restore death rate to `<= 40%` without making full clear exceed `80%`?
- Should the next validation be one-off `qa:balance` first or the full `balance:loop` directly?

## Do Not Touch

Do not add new memories, slots, shop, meta progression, region structure, weapon expansion, or final boss expansion. Do not stack multiple balance changes in one pass; choose one small adjustment and verify.
