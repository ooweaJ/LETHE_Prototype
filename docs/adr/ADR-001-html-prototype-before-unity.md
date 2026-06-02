# ADR-001: Validate In HTML Before Unity Implementation

## Status

Accepted

## Context

LETHE's core risk is not technical rendering fidelity. The core risk is whether the loop is fun and emotionally legible:

- early combat must avoid feeling loose,
- run growth must feel like a roguelike choice,
- forgetting must feel regrettable rather than irritating,
- echo/replacement must feel like adaptation rather than full cancellation.

Unity implementation would take more time and increase production cost. Before that, the team needs evidence that the design is worth expanding.

## Options

1. Move to Unity immediately.
2. Keep iterating only on written design.
3. Build and test a static HTML prototype first.

## Decision

Use the HTML prototype as the validation layer. Move toward Unity only after AI tests and 5-8 human playtests show enough promise.

## Reasons

- HTML iteration is faster for core loop experiments.
- AI simulation can repeatedly test proxy metrics such as early fun, regret, irritation, restart intent, and power dip.
- Human tests can verify emotion and fun before Unity production begins.
- Scope guards prevent the prototype from becoming a large unfinished game.

## AI Collaboration

Claude/GPT are used after test evidence exists. They interpret whether the current data supports human testing, HTML v0.6 iteration, or future Unity transition groundwork.

Codex remains responsible for implementation, verification, report generation, and converting AI answers into tasks.

## Consequences

- Unity-specific implementation is intentionally delayed.
- The portfolio story becomes evidence-driven: problem, constraints, AI planning, developer correction, verification, result.
- Human test logs and AI planning responses become first-class project artifacts.

## Current Evidence

- v0.5 AI quick/default/heavy tests reached `GO_CANDIDATE`.
- Chrome headless verified level-up UI, run resume, `runGrowth`, and playtest metadata payload.
- Claude pipeline verdict: `GO_TO_HUMAN_TEST`.
- Next gate: 5-8 human playtests.
