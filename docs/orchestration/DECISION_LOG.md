# Decision Log

This file is an index of durable decisions. Link to detailed reports, legacy docs, review prompts, review responses, or evidence instead of duplicating full history.

| ID | Date | Decision | Reason | Evidence / Links |
| --- | --- | --- | --- | --- |
| DEC-2026-06-08-01 | 2026-06-08 | Adopt `docs/orchestration/` as the project-management interface. | The user wants a reusable cross-project Codex orchestration layer for status, tasks, logs, reports, evidence, and portfolio material. | `EXISTING_PROJECT_MIGRATION_PROMPT.md`, `AGENTS.md`, `docs/orchestration/README.md` |
| DEC-2026-06-08-02 | 2026-06-08 | Keep root `AGENTS.md` as top-level rules and add only an Orchestration Interface section. | Existing agent rules are project-specific and should not be replaced by orchestration docs. | `AGENTS.md`, `docs/orchestration/PROMPT_CONTEXT.md` |
| DEC-2026-06-08-03 | 2026-06-08 | Treat v0.12 as the current controlled human-test candidate. | The latest balance loop passed and package regeneration succeeded; further direction needs human evidence. | `docs/CODEX_STATUS.md`, `docs/orchestration/STATUS.md`, `docs/reports/2026-06-08.md` |
| DEC-2026-06-08-04 | 2026-06-08 | Make orchestration HTML a Korean human-facing command dashboard. | The user needs to read development docs directly and decide the next prompt from the last completed state. | `docs/orchestration/index.html`, `docs/orchestration/reports/index.html`, `docs/orchestration/devlog/index.html` |
| DEC-2026-06-07-01 | 2026-06-07 | Use `balance:loop` as the required balance source instead of relying on one-off `qa:balance`. | The user flagged loop/one-off mismatch; loop defaults and death gate were fixed. | `docs/reports/2026-06-07.md`, `docs/balance/2026-06-07-v012-balance-loop.md` |
