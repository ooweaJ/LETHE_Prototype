# 2026-06-08-05 - HTML Interface Contract Alignment

## 1. Current build status

LETHE v0.12 remains the current controlled human-test candidate. This work updated only orchestration documentation and human-facing HTML.

## 2. What changed today

- Updated the working orchestration HTML set to match the revised `EXISTING_PROJECT_MIGRATION_PROMPT.md`.
- Restored `docs/orchestration/index.html` as the main Korean project dashboard.
- Added:
  - `docs/orchestration/command.html`,
  - `docs/orchestration/runbook.html`,
  - `docs/orchestration/reports/2026-06-08-04-korean-human-facing-dashboard-normalization.html`.
- Updated `AGENTS.md` and orchestration Markdown docs to define generated HTML as the human-facing project interface.

## 3. Test results and evidence

- `npm run report`: pass, generated 5 unit reports.
- `npm run report:check`: pass, 5 report units.
- `npm run report:discord:unit:dry`: pass, latest unit 05 summary generated.
- `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- `npm run report:discord:unit`: blocked by approval reviewer because it would send workspace-generated report content and attachments to an external Discord webhook without explicit user approval for that exfiltration. Next trusted-local command: `npm run report:discord:unit`.

## 4. Decisions made

- The required orchestration HTML interface is `index.html`, `command.html`, and `runbook.html`.
- `reports/index.html` and `devlog/index.html` are optional browse pages.
- User-facing reports belong in orchestration `reports/` as progress records, while legacy generated report units stay linked.

## 5. Problems or risks

- HTML is still manually maintained and can drift from Markdown until a generator is implemented.
- Discord actual send was blocked by approval policy in this Codex session.

## 6. GPT handoff summary

The orchestration interface now follows the updated migration contract: Markdown remains source of truth; HTML is the local human-facing dashboard/command/runbook interface; reports are reachable from the orchestration folder.

## 7. Next Codex tasks

- Run controlled human sessions.
- Put downloaded JSON logs in `playtest_logs/`.
- Run `npm run playtest:summary`.
- Later, build a generator for the orchestration HTML interface.

## 8. Portfolio notes

- Problem: Codex conversations alone do not give the user a durable project-management surface.
- Direction: expose project state, next prompt, runbook, and reports as local human-readable HTML.
- Action: aligned the orchestration HTML set with the shared migration prompt.
- Result: the project folder now works more like a visible development dashboard than a hidden chat transcript.
