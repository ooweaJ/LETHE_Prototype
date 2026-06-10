# 2026-06-10 Forgetting Model Gate

Source: user-provided planning conclusion.

## Verdict

`ITERATE_BEFORE_TEST`

Before Unity setup or new content, port only the new forgetting model into the HTML build and use that build for a jaewoo solo feel test.

## Required HTML Changes

1. Forget the highest-level active memory instead of using dependency-weighted random selection. If multiple active memories share the highest level, the human player chooses; QA/debug paths choose the most recently upgraded tied memory.
2. Cap memory and echo levels at `+5`.
3. When a forgotten memory would push its echo above `+5`, keep the echo at `+5` and convert the overflow into one immediate echo overcharge burst. Log `overcharge` and `ultimateGauge`.
4. When reacquiring a memory that has an echo, start it at `min(5, base + floor(echoLevel / 2))`. Do not consume the echo.
5. Make the tradeoff visible: memory slots show levels, echo panel shows echo level and `+5` awakening, and HUD states that the next forgotten memory is the highest-level memory.
6. Add debug demonstration buttons for immediate forgetting, echo `+5`, and, if practical, ultimate echo marking.

## Optional / Supporting

- Sync the AI simulator's forgetting selection to highest-level memory for regression usefulness.
- Do not use AI emotion proxy as the gate for this round.
- One stretch showcase is acceptable only if time remains: `칼무리 잔향 + 혈반 잔향 -> 피의 칼폭풍`.

## Human Test Questions

- When the highest-level memory is lost, does it feel regrettable or merely annoying?
- Does the echo immediately change the fight enough to notice?
- Does reacquiring the lost memory with resonance feel exciting?
- Does the player consciously think, "if I raise this memory, it may be next to disappear"?

## Explicit Non-Goals

- No new memories, weapons, enemies, shops, meta progression, multi-region structure, final boss, or Unity setup in this round.
- No broad automation, dashboard, or reporting pipeline work beyond normal records.
