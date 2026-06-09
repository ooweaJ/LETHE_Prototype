# Current Task

## Goal

Run controlled human sessions against the restored v0.12 balance baseline.

## Why Now

The 2026-06-08 `npm run balance:loop` rerun returned `ITERATE_BALANCE`: full clear `20%`, death `60%`, first boss clear `100%`, first boss TTK median `26.42s`.

The first one-lever candidate, lowering `laterCycleClimax 46 -> 42`, was tested on 2026-06-09 and rejected. It worsened the loop to full clear `20%`, death `80%`, so the code was reverted.

The accepted one-lever recovery was player max HP `180 -> 190`. Two consecutive `npm run balance:loop` runs returned `GO_BALANCE_BASELINE`: full clear `60%`, death `40%`, first boss clear `100%`, first boss TTK median `18.97s` then `20.18s`.

`npm run playtest:package:dry` and `npm run playtest:package` passed, and `dist\lethe-v0.12-playtest` was regenerated. Numeric balance and packaging are no longer the current blockers; human evidence is.

Before the human session, the user reported that the echo effect felt too weak or unclear. Code review showed the mechanics were present, but the player-facing feedback was too subtle. The current build now clarifies weapon echoes in the forget result card, combat log, sidebar, and start-of-deficit visual cue without changing numeric balance.

The report/devlog migration is complete, so new work should now use:

- AI state: `docs/orchestration/state/`
- AI devlog: `docs/orchestration/devlog/YYYY-MM-DD.md`
- Human report: `docs/orchestration/reports/YYYYMMDD/index.md|html`
- Human report archive: `docs/orchestration/reports/index.html`
- Work-unit details: `docs/orchestration/reports/YYYYMMDD/units/`

## Done Criteria

- The restored v0.12 build is packaged or confirmed packageable. Done: `npm run playtest:package:dry` and `npm run playtest:package` passed.
- Echo readability is patched without adding new systems or changing balance values. Done: `npm run qa:postloss` and `npm run qa:identity` passed with local `CHROME_PATH`.
- Human playtest guide and notes are ready for a controlled session.
- At least one human session log or note set is captured under the expected playtest evidence path. Pending.
- Status, devlog, reports, and decision log are updated after the session.

## Related Files

- `docs/balance/2026-06-08-v012-balance-qa.md`
- `docs/balance/2026-06-09-v012-balance-qa.md`
- `docs/orchestration/review_prompts/2026-06-08-balance-loop.md`
- `docs/orchestration/review_prompts/2026-06-09-balance-loop.md`
- `dist\lethe-v0.12-playtest`
- `docs/HUMAN_PLAYTEST_GUIDE.md`
- `docs/PLAYTEST_NOTES.md`
- `docs/BALANCE_TABLE_v0_12.md`
- `docs/LETHE_v0.12_밸런스_개선_제안서.md`
- `scripts/run_balance_loop.js`
- `scripts/run_browser_balance_qa.js`

## Verification Commands

```bash
npm run qa:balance
npm run balance:loop
npm run playtest:package:dry
npm run playtest:package
npm run qa:postloss
npm run qa:identity
npm run report
npm run report:check
npm run report:discord:unit:dry
npm run doctor
```

## Open Questions

- Does the restored numeric baseline feel fair to a human tester?
- Does the forgetting loop feel regrettable rather than irritating?
- Does the clearer echo card/log/sidebar make the remaining weapon echo understandable?
- Does HP `190` make the run too forgiving, or does it only reduce unfair deaths?
- Should the next validation be one-off `qa:balance` first or the full `balance:loop` directly?

## Do Not Touch

Do not add new memories, slots, shop, meta progression, region structure, weapon expansion, or final boss expansion. Do not stack multiple balance changes in one pass; choose one small adjustment and verify.
