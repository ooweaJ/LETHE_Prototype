# LETHE v0.12 First Boss TTK Harness

Generated: 2026-06-05

## Goal

After first-cycle `망각 전조` survival reached the target band, the next task was to get stable first-boss TTK samples.

Required samples:

- at least 3 accepted gameplay samples past 180s,
- `bossFights[0].damage`,
- `firstBossTtk`,
- focused DPS.

## Changes

### Balance Runner Polling

Changed `scripts/run_browser_balance_qa.js` so the runner no longer waits on one long in-page `Runtime.evaluate` Promise.

Before:

- one `Runtime.evaluate` call waited until the run completed or timed out.

After:

- `pollBalanceQaResult()` sends short `Runtime.evaluate` reads every 500ms,
- each read only returns `document.documentElement.dataset.letheBalanceQa`,
- intended to reduce long CDP call instability.

### First Boss TTK Scenario

Added `--scenario first_boss_ttk`.

The scenario passes `balanceScenario=first_boss_ttk` into the browser QA URL and prepares a representative first-boss state:

- elapsed: 176s,
- level: 10,
- three active memories,
- full HP,
- no active enemies/projectiles,
- first boss still scheduled for 180s.

This is a targeted boss HP/TTK measurement harness. It is not a replacement for the full prelude survival loop.

## Evidence

| Attempt | Evidence | Result |
| --- | --- | --- |
| Polling smoke | `docs/balance/2026-06-05-loop-02-first-boss-polling-smoke.md` | browser success 0%, no gameplay sample |
| TTK scenario smoke | `docs/balance/2026-06-05-loop-02-first-boss-ttk-scenario-smoke-clean.md` | command timed out before report during one run; no gameplay sample |

## Conclusion

The code path for a TTK-specific harness exists, but the current local Chrome/CDP execution path still fails before accepted gameplay samples are produced.

Do not tune first boss HP yet.

## Next Step

Use one of these before boss HP tuning:

- run the `first_boss_ttk` scenario in a trusted/local browser environment with stable CDP,
- or replace the browser/CDP path with an in-process deterministic simulation harness for boss-only TTK.
