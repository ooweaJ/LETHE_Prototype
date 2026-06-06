# 2026-06-06 Balance Baseline Review - Codex

External Claude review was requested, but the approval reviewer blocked `npm run review:claude` because the prompt is workspace-derived content sent to an external Claude service. This local Codex review is the safer fallback for this session.

## Decision

`ITERATE_DEFICIT_TRIAL`

## Reasoning

- The latest automated baseline passes, but full clear is exactly at the upper bound: `80%`.
- Death fell from `60%` to `20%`, which meets the target but may have removed too much post-boss tension.
- First boss HP should remain `2500`, because boss-only TTK and browser first-boss TTK both remain inside the current target band.
- The next pass should reintroduce a small amount of late/post-loss pressure without changing first-boss tuning or adding new systems.

## Next Codex Tasks

- [ ] Increase deficit trial pressure slightly, preferably through the existing spawn cap.
- [ ] Reduce the refill safety margin slightly while keeping the memory replacement loop survivable.
- [ ] Rerun boss-only TTK, browser first-boss TTK, and full `qa:balance`; accept only if first boss TTK remains `15-30s`, full clear remains `35-80%`, and death remains `<= 40%`.
