# Double Check Summary - 2026-06-02-v08-core-redesign

## Prompt

- `docs/review_prompts/2026-06-02-v08-core-redesign.md`

## Responses

- Claude: `docs/review_responses/2026-06-02-v08-core-redesign-claude.md`
- Codex CLI: `docs/review_responses/2026-06-02-v08-core-redesign-codex.md`

## Common Recommendations

- v0.7 is not a tuning problem; it needs v0.8 core redesign.
- HP 1 death-prevention must be removed first because it invalidates survival and balance evidence.
- The HTML validation run should be shortened to roughly 8-10 minutes.
- First mini-boss/first forgetting should arrive around 90-120 seconds, not 4 minutes.
- Memory power budgets must be flattened so utility memories still have a damage floor.
- Synergy should be added in v0.8, but kept small.
- Echo should become tag-based build transformation, not a generic stat refund.
- AI proxy metrics must be treated as scouting, not balance proof.

## Conflicts

- Claude label: `FIX_TEST_MODEL_FIRST`.
- Codex CLI label: `REDESIGN_V08_CORE`.
- Resolution: implement v0.8 in two gates:
  1. v0.8 gate A: death bug and real danger metrics.
  2. v0.8 gate B: short run, memory budget, minimal synergies, tag echo.

## Selected v0.8 Scope

- Start now with v0.8 gate A.
- Do not touch meta progression, shop, final boss, more than 6 memories, multi-region runs, unlocks, or third weapon.
- Do not report balance as passed until human play or browser combat QA validates it.

## Tests Required Before Reporting Balance

- Syntax checks.
- AI test with real death metrics.
- Browser combat QA that can fail from death.
- User solo play feedback before any broad human test.
