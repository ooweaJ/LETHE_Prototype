# ADR-002: Test-Result Planning Pipeline

## Status

Accepted

## Context

Manual AI review did not match the intended workflow. The project needs a repeatable path:

1. run tests,
2. generate a prompt with evidence,
3. ask Claude and Codex CLI for planning judgment when the decision is large,
4. save the response,
5. let Codex turn the response into tasks and implementation.

The user also needs this to work across local environments, including cases where Claude authentication, Codex CLI login, Discord, or external transmission is unavailable.

## Options

1. Manually write prompts and paste model responses.
2. Use one external model call with no fallback.
3. Use a local pipeline with prompt-only fallback and provider checks.
4. Use fixed double-check planning for major design decisions.

## Decision

Use `scripts/run_planning_pipeline.js` and npm aliases:

- `npm run planning:pipeline:dry`
- `npm run planning:pipeline:prompt`
- `npm run planning:pipeline`
- `npm run planning:pipeline:double`
- `npm run planning:pipeline:auto`
- `npm run planning:pipeline:claude`
- `npm run planning:pipeline:codex`

The pipeline defaults to quick AI testing, writes `docs/review_prompts/YYYY-MM-DD-pipeline.md`, and asks both Claude and Codex CLI for major planning decisions. `planning:pipeline:auto` preserves the older Claude-first fallback behavior for smaller checks.

Human test logs use a separate summary line:

- `npm run playtest:summary:dry`
- `npm run playtest:summary`

This reads raw JSON logs from `playtest_logs/`, writes `docs/playtest_summaries/YYYY-MM-DD.md`, and creates `docs/review_prompts/YYYY-MM-DD-human-playtest.md`.

## Reasons

- It keeps AI planning tied to test evidence.
- It preserves prompt and response files for portfolio review.
- It gives a safe no-external-call path.
- It makes local failures diagnosable.
- It prevents a single AI planning answer from being mistaken for product truth.
- It separates emotion/pacing review from systems/balance/testability review.

## AI Collaboration

Claude and Codex CLI are planning partners, not code owners. They do not edit project files in automated planning. Codex reads both responses, summarizes common points and conflicts, then decides the implementation unit.

## Consequences

- External model calls still require local authentication and permission.
- The project needs a doctor command so other local environments can see what is missing.
- Prompt-only generation is treated as a valid partial success when external transfer is blocked.
- Raw human playtest JSON logs are ignored by git; generated summaries and prompts are tracked.
- Major direction changes require both responses before implementation unless the user explicitly overrides the gate.

## Verification

- `npm run planning:pipeline:dry`
- `npm run planning:pipeline:prompt`
- `npm run planning:pipeline:double`
- `npm run review:claude:dry`
- `npm run review:codex:dry`
- `npm run playtest:summary:dry`
- `npm run doctor`
