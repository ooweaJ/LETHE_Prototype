# LETHE Orchestration Interface

This directory is the project-management interface for Codex and the user. It does not replace the existing LETHE documentation; it summarizes and links the current operating state so the project can be resumed quickly.

Markdown state/devlog files are the AI source of truth. Generated HTML report/interface files are the human-facing project interface.

## Human Dashboard

- `interface/index.html` is the Korean 30-second project dashboard.
- `interface/command.html` is the compact next-instruction block for the command area.
- `interface/runbook.html` is the operating-procedure block for repeated commands.
- The dashboard reports only the current state, blocker, latest completed work, and the main links.
- The dashboard should stay short; detailed explanation belongs in `reports/`.
- `reports/index.html` lists date-based user-facing and portfolio-ready development reports.
- `reports/YYYYMMDD/index.html` is the human-readable daily report page.
- `reports/YYYYMMDD/units/` contains generated work-unit pages for commit-sized or decision-sized entries.
- `devlog/index.html` lists internal work logs.
- Markdown remains the source of truth; these HTML pages should eventually be generated from Markdown.

## Read Order

Before meaningful work, read these files in order:

1. `state/PROJECT_BRIEF.md`
2. `state/STATUS.md`
3. `state/CURRENT_TASK.md`
4. `state/NEXT_TASKS.md`
5. `state/PROMPT_CONTEXT.md`
6. `state/RUNBOOK.md`
7. `state/SCOPE_GUARD.md`

## File Roles

- `state/PROJECT_BRIEF.md`: project identity, goals, tech stack, and portfolio framing.
- `state/STATUS.md`: current whole-project state, latest verification, blockers, and next major step.
- `state/CURRENT_TASK.md`: the one active work unit and its completion criteria.
- `state/NEXT_TASKS.md`: the top five next task candidates.
- `state/PROMPT_CONTEXT.md`: stable Codex context and project workflow expectations.
- `state/RUNBOOK.md`: commands for verification, reporting, packaging, and recovery.
- `state/SCOPE_GUARD.md`: explicit non-goals and forbidden expansions.
- `state/DECISION_LOG.md`: durable decision index with links to evidence.
- `devlog/YYYYMMDD.md`: AI/internal daily work logs.
- `reports/`: Korean user-facing and portfolio-facing daily report pages.
- `reports/index.html`: human-readable date list.
- `reports/YYYYMMDD/index.md`: daily report Markdown source.
- `reports/YYYYMMDD/index.html`: daily report HTML page.
- `reports/YYYYMMDD/units/`: generated work-unit pages and JSON summaries.
- `interface/index.html`: generated 30-second project dashboard for people.
- `interface/command.html`: generated next-instruction block.
- `interface/runbook.html`: generated operating-procedure block.
- `review_prompts/`: prompts prepared for AI review.
- `review_responses/`: saved AI review responses.
- `evidence/`: test, QA, benchmark, playtest, screenshot, and log evidence or links.
- `templates/`: reusable document templates.
- `devlog/index.html`: human-readable devlog list.

## Legacy Mapping

- Root `AGENTS.md` remains the top-level rulebook.
- `docs/CODEX_STATUS.md` remains the detailed legacy status archive.
- `docs/NEXT_TASKS.md` remains the detailed legacy task archive.
- Former `docs/devlog/` files were migrated into `docs/orchestration/devlog/`.
- Former `docs/reports/` files were migrated into `docs/orchestration/reports/YYYYMMDD/`.
- New work should not recreate `docs/devlog/` or `docs/reports/` as normal source-of-truth folders.
