# 2026-06-08-02 - Orchestration Interface Adoption

## 1. Current build status

LETHE v0.12 remains the current controlled human-test candidate. This work did not change gameplay.

## 2. What changed today

- Added `docs/orchestration/` as the shared Codex project-management interface.
- Added a root `AGENTS.md` Orchestration Interface section without replacing existing LETHE rules.
- Created current-state, current-task, next-task, scope, runbook, prompt context, decision index, devlog, report, evidence, review, and template seed files.
- Updated `EXISTING_PROJECT_MIGRATION_PROMPT.md` so it describes orchestration adoption rather than blind migration.

## 3. Test results and evidence

- Read `docs/orchestration/README.md`: pass.
- Read `docs/orchestration/STATUS.md`: pass.
- Read `docs/orchestration/CURRENT_TASK.md`: pass.
- Read `docs/orchestration/NEXT_TASKS.md`: pass.
- `npm run report`: pass.
- `npm run report:check`: first parallel run failed because it raced report generation; sequential rerun passed.
- `npm run report:discord:unit:dry`: pass.
- `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- Gameplay and balance tests were not rerun because this is a documentation/interface adoption task.

## 4. Decisions made

- Root `AGENTS.md` remains the top-level rulebook.
- `docs/orchestration/` is now the quick resume and operating interface.
- Legacy docs remain detailed archives.
- Markdown remains the source of truth; HTML dashboard generation is deferred until the Markdown interface is validated.

## 5. Problems or risks

- Legacy `docs/NEXT_TASKS.md` is still long and may drift from the intentionally short orchestration task list.
- The HTML orchestration dashboard is not implemented yet.
- `npm run report:discord:unit` was blocked by the approval reviewer because sending workspace-generated reports/attachments to an external Discord webhook is treated as potential private data exfiltration in this Codex session. Next trusted-local command: `npm run report:discord:unit`.

## 6. GPT handoff summary

The project now has a reusable orchestration interface suitable for testing the cross-project Codex management pattern. Current project direction remains v0.12 human sessions, followed by playtest summary and planning review.

## 7. Next Codex tasks

- Use `docs/orchestration/` as the first read path in the next session.
- Run controlled human sessions and `npm run playtest:summary` when logs exist.
- Consider adding a generated orchestration dashboard after the Markdown interface proves useful.

## 8. Portfolio notes

- Problem: project state was spread across legacy status docs, task docs, devlogs, reports, and review artifacts.
- Direction: introduce a reusable orchestration interface without deleting legacy history.
- Action: created the standard `docs/orchestration/` structure and linked it from `AGENTS.md`.
- Result: future Codex sessions can resume from a small standard document set while detailed history remains available.
