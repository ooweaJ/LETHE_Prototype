# ADR-002: Test-Result Planning Pipeline

## Status

Accepted

## Context

Manual AI review did not match the intended workflow. The project needs a repeatable path:

1. run tests,
2. generate a prompt with evidence,
3. ask Claude/GPT for planning judgment,
4. save the response,
5. let Codex turn the response into tasks and implementation.

The user also needs this to work across local environments, including cases where Claude authentication, Codex CLI login, Discord, or external transmission is unavailable.

## Options

1. Manually write prompts and paste model responses.
2. Use one external model call with no fallback.
3. Use a local pipeline with prompt-only fallback and provider checks.

## Decision

Use `scripts/run_planning_pipeline.js` and npm aliases:

- `npm run planning:pipeline:dry`
- `npm run planning:pipeline:prompt`
- `npm run planning:pipeline`
- `npm run planning:pipeline:claude`
- `npm run planning:pipeline:codex`

The pipeline defaults to quick AI testing, writes `docs/review_prompts/YYYY-MM-DD-pipeline.md`, asks Claude first, and falls back to Codex CLI.

Human test logs use a separate summary line:

- `npm run playtest:summary:dry`
- `npm run playtest:summary`

This reads raw JSON logs from `playtest_logs/`, writes `docs/playtest_summaries/YYYY-MM-DD.md`, and creates `docs/review_prompts/YYYY-MM-DD-human-playtest.md`.

## Reasons

- It keeps AI planning tied to test evidence.
- It preserves prompt and response files for portfolio review.
- It gives a safe no-external-call path.
- It makes local failures diagnosable.

## AI Collaboration

Claude/GPT are planning partners, not code owners. They do not edit project files in automated planning. Codex reads their responses and decides the implementation unit.

## Consequences

- External model calls still require local authentication and permission.
- The project needs a doctor command so other local environments can see what is missing.
- Prompt-only generation is treated as a valid partial success when external transfer is blocked.
- Raw human playtest JSON logs are ignored by git; generated summaries and prompts are tracked.

## Verification

- `npm run planning:pipeline:dry`
- `npm run planning:pipeline:prompt`
- `npm run review:claude:dry`
- `npm run review:codex:dry`
- `npm run playtest:summary:dry`
- `npm run doctor`
