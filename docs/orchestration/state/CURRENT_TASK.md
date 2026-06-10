# Current Task

## Goal

Implement the HTML-only new forgetting model and prepare a jaewoo solo feel-test build.

## Why Now

The user changed the immediate direction from Unity backlog work to `ITERATE_BEFORE_TEST`.

The current HTML forgetting model used dependency-weighted randomness, so the player could not clearly see the tradeoff between "I strengthen this memory" and "this memory is now at risk." The new testable fantasy is:

- the highest-level active memory is the next forgetting target;
- tied highest memories are chosen by the player;
- forgotten memory level becomes echo level up to `+5`;
- overflow above `+5` becomes immediate overcharge;
- reacquiring an echoed memory creates resonance and returns at a stronger level;
- the whole loop should be tested in HTML before Unity setup.

## Done Criteria

- HTML forgetting selects the highest-level active memory.
- Highest-level ties show a player choice UI in normal play.
- QA/debug automated paths choose the most recently upgraded tied memory.
- Memory and echo levels are capped at `+5`.
- Echo overflow triggers one immediate overcharge burst and logs `overcharge` plus `ultimateGauge`.
- Reacquired echoed memories start at `min(5, base + floor(echoLevel / 2))`; echo is not consumed.
- HUD clearly shows memory levels, next forgetting candidate, echo level, `+5` awakening, overcharge, and resonance.
- Debug controls exist for immediate forgetting, echo `+5`, and ultimate marker setup.
- AI simulator forgetting selection is synchronized enough for balance regression checks.
- `npm run qa:balance`, `npm run balance:loop`, `npm run playtest:package:dry`, and `npm run playtest:package` have passed.
- `npm run report` and `npm run report:check` have passed.

## Related Files

- `src/game.js`
- `index.html`
- `style.css`
- `alpha_test/src/simulator.js`
- `docs/orchestration/review_responses/2026-06-10-forgetting-model-gate.md`
- `docs/orchestration/state/STATUS.md`
- `docs/orchestration/state/NEXT_TASKS.md`
- `docs/orchestration/reports/20260610/index.md`

## Verification Commands

```bash
npm run qa:balance
npm run balance:loop
npm run report
npm run report:check
```

## Human Test Gate

The jaewoo solo feel test should answer:

- Is losing the highest-level memory regrettable rather than irritating?
- Does the echo immediately change combat enough to notice?
- Does reacquiring the echoed memory with resonance feel exciting?
- Does the player consciously think about the tradeoff of leveling a memory that may be forgotten next?

## Do Not Touch

Do not add new memories, weapons, enemies, shop, meta progression, multi-region structure, final boss, or Unity setup in this round. Do not build a broader dashboard or automation system beyond normal records.
