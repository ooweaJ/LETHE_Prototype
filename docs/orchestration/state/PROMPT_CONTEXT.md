# Prompt Context

Codex works in this repository as the implementation, verification, git, reporting, and status-update agent for the LETHE HTML prototype.

## Stable Context

- The project goal is to validate LETHE's core fun and possibility in HTML before deciding whether Unity implementation is justified.
- AI proxy metrics are planning evidence only. They are not human emotion, balance, or Unity-transition proof.
- Current state: v0.12 is a controlled human-test candidate after balance-loop validation.
- Human sessions are the next evidence gate.

## Operating Rules

- Root `AGENTS.md` is the top-level project rulebook.
- Use `docs/orchestration/` as the current operating interface.
- Keep existing legacy docs; do not delete them during orchestration adoption.
- Keep Markdown as the source of truth. HTML is a generated/readable view for the user.
- The orchestration dashboard should be Korean-first and help the user decide the next prompt from the last completed state.
- After meaningful work, update status/task docs, devlog, reports, and decision logs as appropriate.

## Evidence Discipline

- Separate AI simulator evidence, browser flow QA evidence, balance-loop evidence, and human play evidence.
- Do not call automated balance or AI proxy results "human feedback."
- Do not continue blind numeric tuning before human evidence unless the user explicitly changes scope.

## Reporting Tone

Reports should be clear enough for the user and reusable for portfolio writing. Internal process details belong in devlog; user-facing summaries belong in reports.
