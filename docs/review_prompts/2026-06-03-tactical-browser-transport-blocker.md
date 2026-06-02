# 2026-06-03 Tactical Browser Transport Blocker Review

## Context

LETHE HTML alpha v0.9 WP3 Slice A is implementation-complete and scope-valid as a minimal tactical agency hook. It uses only the current active memory slots and the existing combat loop.

It adds no new memories, slots, shops, meta progression, regions, enemies, weapons, or bosses.

AI proxy checks remain positive enough for planning:

- `npm run ai:test:quick`: `GO_CANDIDATE`.
- Alpha Fun Score: about `0.8846`.
- Early Fun Score: about `0.8316`.
- Post-loss challenge contrast: about `0.3134`.
- Regret proxy: about `80.7%`.
- Irritation proxy: about `1.0%`.

Browser proof is still missing.

## Latest Codex Evidence

This managed Codex sandbox reran the required tactical browser gate:

- `npm run qa:tactical:trusted`: failed before gameplay evaluation.
- The wrapper first ran standard tactical QA.
- It retried once with `--timeout-ms 30000`.
- Both attempts hit the same Chrome/CDP transport class.

The latest command output is:

```text
BrowserQaTransportError: Browser tactical QA could not reach Chrome through either CDP pipe or remote-debugging-port.
Pipe failure: Timed out waiting for Chrome page target: Timed out waiting for CDP response to Target.getTargets
Port fallback failure: listen EPERM: operation not permitted 127.0.0.1
Next trusted-local command: npm run qa:tactical
If the same transport failure repeats outside this managed sandbox, record it as an environment blocker before starting new gameplay scope.
```

The structured wrapper result is written to:

```text
alpha_test/outputs/tactical-trusted-gate/latest.json
```

This points to a browser automation transport blocker, not a tactical-focus gameplay assertion failure.

## Decision Needed

If trusted-local `npm run qa:tactical:trusted` still cannot produce browser proof because of the same transport failure, decide whether Codex should:

1. Keep people testing and new gameplay scope blocked while improving QA transport only.
2. Accept the documented environment blocker and proceed only to the already-deferred one-line echo-pivot hint.
3. Ask the user for manual browser evidence of tactical focus before any hint, balance, UI, or gameplay change.

## Scope Guard

Until this decision is made, do not add:

- New memories.
- New slots.
- Shop systems.
- Meta progression.
- New regions.
- New enemies.
- New weapons or large weapon expansions.
- Additional tactical systems beyond the current `전술 집중` hook.
- Human-test requests.

## Output Requested

Return:

- Verdict: `BLOCK_NEXT_SCOPE`, `PROCEED_WITH_ENVIRONMENT_BLOCKER`, or `REQUEST_MANUAL_BROWSER_EVIDENCE`.
- Reasoning in 3-5 bullets.
- The next single Codex task.
- Tests required before any human playtest.
