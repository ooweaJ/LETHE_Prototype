# 2026-06-08-04 - Korean Human-Facing Dashboard Normalization

## 1. Current build status

LETHE v0.12 remains the current controlled human-test candidate. This work updated only orchestration documentation and human-facing HTML.

## 2. What changed today

- Rebuilt `docs/orchestration/index.html` as a Korean report dashboard.
- Added a copyable recommended prompt.
- Added report/devlog list pages:
  - `docs/orchestration/reports/index.html`,
  - `docs/orchestration/devlog/index.html`.
- Updated orchestration docs to define the dashboard convention.

## 3. Test results and evidence

- HTML files were inspected through file reads.
- `npm run report`: pass.
- `npm run report:check`: pass.
- `npm run report:discord:unit:dry`: pass.
- `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- Gameplay and balance tests were not rerun because this is documentation/dashboard work.

## 4. Decisions made

- Orchestration HTML is Korean-first and user-facing.
- The dashboard reports the last completed state rather than live Codex thoughts.
- Markdown remains the source of truth.
- HTML should eventually be generated from Markdown.

## 5. Problems or risks

- The dashboard is currently hand-maintained and can drift from Markdown.
- A generator is needed before this pattern scales comfortably across many projects.
- `npm run report:discord:unit` was blocked by the approval reviewer because sending workspace-generated reports/attachments to an external Discord webhook is treated as potential private data exfiltration in this Codex session. Next trusted-local command: `npm run report:discord:unit`.

## 6. GPT handoff summary

The orchestration dashboard now acts like a Korean project report and command surface. It highlights the current LETHE state, what the user should do next, and which prompt can be copied for the next Codex action.

## 7. Next Codex tasks

- Use this dashboard to guide the next prompt.
- Run controlled human sessions and `npm run playtest:summary` when logs exist.
- Later, implement automatic dashboard/list generation from Markdown.

## 8. Portfolio notes

- Problem: project progress inside Codex is hard to inspect outside the chat.
- Direction: expose project management documents as local human-readable HTML.
- Action: normalized dashboard UX and added document indexes.
- Result: the repository now contains a user-facing view for current state, reports, devlogs, and next prompt selection.
