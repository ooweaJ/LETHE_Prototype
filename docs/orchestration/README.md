# LETHE Orchestration Interface

This directory is the project-management interface for Codex and the user. It does not replace the existing LETHE documentation; it summarizes and links the current operating state so the project can be resumed quickly.

Markdown files are the source of truth. HTML files, when generated, are readable views only.

## Read Order

Before meaningful work, read these files in order:

1. `PROJECT_BRIEF.md`
2. `STATUS.md`
3. `CURRENT_TASK.md`
4. `NEXT_TASKS.md`
5. `PROMPT_CONTEXT.md`
6. `RUNBOOK.md`
7. `SCOPE_GUARD.md`

## File Roles

- `PROJECT_BRIEF.md`: project identity, goals, tech stack, and portfolio framing.
- `STATUS.md`: current whole-project state, latest verification, blockers, and next major step.
- `CURRENT_TASK.md`: the one active work unit and its completion criteria.
- `NEXT_TASKS.md`: the top five next task candidates.
- `PROMPT_CONTEXT.md`: stable Codex context and project workflow expectations.
- `RUNBOOK.md`: commands for verification, reporting, packaging, and recovery.
- `SCOPE_GUARD.md`: explicit non-goals and forbidden expansions.
- `DECISION_LOG.md`: durable decision index with links to evidence.
- `devlog/`: internal daily work logs.
- `reports/`: user-facing and portfolio-facing work-unit summaries.
- `review_prompts/`: prompts prepared for AI review.
- `review_responses/`: saved AI review responses.
- `evidence/`: test, QA, benchmark, playtest, screenshot, and log evidence or links.
- `templates/`: reusable document templates.

## Legacy Mapping

- Root `AGENTS.md` remains the top-level rulebook.
- `docs/CODEX_STATUS.md` remains the detailed legacy status archive.
- `docs/NEXT_TASKS.md` remains the detailed legacy task archive.
- `docs/devlog/` and `docs/reports/` remain detailed historical logs and reports.
- This orchestration directory keeps the short current interface and links back to those archives when useful.
