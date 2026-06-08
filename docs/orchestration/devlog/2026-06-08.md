# 2026-06-08 Devlog

## Orchestration Adoption

- Goal: install `docs/orchestration/` as the ongoing Codex project-management interface for LETHE.
- Existing sources inspected:
  - `AGENTS.md`
  - `docs/CODEX_STATUS.md`
  - `docs/NEXT_TASKS.md`
  - existing `docs/devlog/`, `docs/reports/`, `docs/review_prompts/`, `docs/review_responses/`, `docs/balance/`
- Mapping:
  - `AGENTS.md` remains the top-level rulebook and received an Orchestration Interface section.
  - `docs/CODEX_STATUS.md` was summarized into `STATUS.md`.
  - `docs/NEXT_TASKS.md` was reduced into the top-five `NEXT_TASKS.md`.
  - Scope boundaries were split into `SCOPE_GUARD.md`.
  - Commands were split into `RUNBOOK.md`.
  - Durable decisions were indexed in `DECISION_LOG.md`.
- Verification:
  - Read `docs/orchestration/README.md`: pass.
  - Read `docs/orchestration/STATUS.md`: pass.
  - Read `docs/orchestration/CURRENT_TASK.md`: pass.
  - Read `docs/orchestration/NEXT_TASKS.md`: pass.
  - `npm run report`: pass.
  - `npm run report:check`: pass after rerun; the first parallel check raced report generation.
  - `npm run report:discord:unit:dry`: pass.
  - `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- Discord:
  - `npm run report:discord:unit` was blocked by the approval reviewer because sending workspace-generated reports/attachments to an external Discord webhook is treated as potential private data exfiltration in this Codex session.
  - Next trusted-local command: `npm run report:discord:unit`.
- Decision: keep legacy docs as detailed archives and use orchestration files as the quick resume interface.
- Not migrated: full historical docs, large QA outputs, and all old reports were not copied. They remain in their legacy locations and are linked or summarized.
- Next: test whether this interface is enough for new sessions, then optionally add a generated HTML dashboard from Markdown.

## Orchestration Dashboard Current-State Refresh

- Goal: update `docs/orchestration/index.html` so the user can read the current LETHE state and next tasks at a glance.
- Changed:
  - top summary cards now show the v0.12 controlled human-test gate,
  - latest verification now highlights `balance:loop GO`, full clear `60%`, and death `40%`,
  - blocker now shows missing human reaction evidence,
  - `STATUS.md` records that the dashboard exists,
  - `NEXT_TASKS.md` now treats dashboard automation as the future task instead of initial dashboard creation.
- Decision: keep Markdown as the source of truth; `index.html` is a readable view until a generator is added.
- Verification:
  - `npm run report`: pass.
  - `npm run report:check`: pass.
  - `npm run report:discord:unit:dry`: pass.
  - `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- Discord:
  - `npm run report:discord:unit` was blocked by the approval reviewer because sending workspace-generated reports/attachments to an external Discord webhook is treated as potential private data exfiltration in this Codex session.
  - Next trusted-local command: `npm run report:discord:unit`.
- Next: run controlled human sessions, then summarize playtest logs.

## Korean Human-Facing Dashboard Normalization

- Goal: make the orchestration HTML useful as a Korean report screen where the user can decide the next prompt.
- Changed:
  - rebuilt `docs/orchestration/index.html` around current status, next user action, last completed report, evidence, blockers, scope guard, and copyable prompt,
  - added `docs/orchestration/reports/index.html` as a human-readable report list,
  - added `docs/orchestration/devlog/index.html` as a human-readable devlog list,
  - updated `README.md`, `STATUS.md`, `NEXT_TASKS.md`, `PROMPT_CONTEXT.md`, `RUNBOOK.md`, and `DECISION_LOG.md` with the dashboard rule.
- Decision: orchestration HTML should be Korean-first and should report the last completed state, not live in-progress Codex thoughts.
- Rationale: the key difference from the Codex app is that the user can directly inspect development documents, report history, devlogs, and evidence from the project folder.
- Verification:
  - `npm run report`: pass.
  - `npm run report:check`: pass.
  - `npm run report:discord:unit:dry`: pass.
  - `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- Discord:
  - `npm run report:discord:unit` was blocked by the approval reviewer because sending workspace-generated reports/attachments to an external Discord webhook is treated as potential private data exfiltration in this Codex session.
  - Next trusted-local command: `npm run report:discord:unit`.
- Next: if the dashboard shape feels useful, add a generator so the HTML pages are rebuilt from Markdown.

## HTML Interface Contract Alignment

- Goal: align the current HTML changes with the updated `EXISTING_PROJECT_MIGRATION_PROMPT.md`.
- Changed:
  - restored `docs/orchestration/index.html` as the Korean human-facing dashboard instead of a thin generated placeholder,
  - added `docs/orchestration/command.html` as the compact next-instruction block,
  - added `docs/orchestration/runbook.html` as the operating-procedure block,
  - updated `AGENTS.md`, `README.md`, `STATUS.md`, `NEXT_TASKS.md`, and `RUNBOOK.md` so generated HTML is described as the human-facing project interface,
  - added an orchestration-local HTML report under `docs/orchestration/reports/` and linked it from `reports/index.html`.
- Decision: the required HTML interface is the 3-file set `index.html`, `command.html`, and `runbook.html`; `reports/index.html` and `devlog/index.html` remain optional browse pages.
- Rationale: the user should be able to inspect development documents directly from the project folder and use them to decide the next prompt.
- Verification:
  - `npm run report`: pass.
  - `npm run report:check`: pass, 5 report units.
  - `npm run report:discord:unit:dry`: pass, latest unit 05 summary generated.
  - `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- Discord:
  - `npm run report:discord:unit` was rejected by the approval reviewer because it would send workspace-generated report content and attachments to an external Discord webhook without explicit user approval for that exfiltration.
  - Next trusted-local command: `npm run report:discord:unit`.

## Project Dashboard Compaction

- Goal: make `docs/orchestration/index.html` a true 30-second project dashboard instead of overlapping with `command.html`.
- Changed:
  - removed the copyable long prompt from the project dashboard,
  - removed detailed completion criteria, large document browsing blocks, and Codex comparison copy,
  - kept only current stage, latest verification, blocker, next gate, current conclusion, recent completion, and three primary links,
  - updated `README.md`, `RUNBOOK.md`, `STATUS.md`, and `NEXT_TASKS.md` to describe the new role split.
- Decision: `index.html` should summarize; `command.html` should instruct; `runbook.html` should operate; `reports/` should explain.
- Verification:
  - `npm run report`: pass, generated 6 unit reports.
  - `npm run report:check`: pass, 6 report units.
  - `npm run report:discord:unit:dry`: pass, latest unit 06 summary generated.
  - `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- Discord:
  - `npm run report:discord:unit` was rejected by the approval reviewer because it would send workspace-generated report content and attachments to an external Discord webhook without explicit user approval for that exfiltration.
  - Next trusted-local command: `npm run report:discord:unit`.

## Project Dashboard Surface Trim

- Goal: remove remaining explanatory/chrome elements from `docs/orchestration/index.html` so it reads purely as a status dashboard.
- Changed:
  - renamed the heading from `30초 상태 요약` to `상태 요약`,
  - removed the explanatory subtitle about `command.html` and `reports/`,
  - removed the bottom link cards for next instruction, runbook, and detailed reports,
  - changed latest completion text to `프로젝트 대시보드 요약화`,
  - kept only the status cards, current conclusion, goal, next judgment, recent completion, and date.
- Decision: people-facing dashboard HTML should not explain the interface; it should show the current project state.
- Verification:
  - `npm run report`: pass, generated 7 unit reports.
  - `npm run report:check`: pass, 7 report units.
  - `npm run report:discord:unit:dry`: pass, latest unit 07 summary generated.
  - `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
  - `Select-String` dashboard check: only `상태 요약` remained; removed target strings were absent.
- Discord:
  - `npm run report:discord:unit` was rejected by the approval reviewer because it would send workspace-generated report content and attachments to an external Discord webhook, which is untrusted by default and not explicitly approved by the user.
  - Next trusted-local command: `npm run report:discord:unit`.

## Plugin-Oriented Migration Prompt Update

- Goal: update `EXISTING_PROJECT_MIGRATION_PROMPT.md` so future projects adopt the orchestration interface as a reusable personal AI plugin structure.
- Changed:
  - defined `docs/orchestration/interface/` for the three human-facing HTML files,
  - defined `docs/orchestration/state/` for AI-facing Markdown source-of-truth files,
  - defined `docs/orchestration/reports/` for user-facing HTML work-unit reports,
  - defined `docs/orchestration/devlog/` for AI/internal chronological Markdown logs,
  - added legacy migration rules so old docs become migrated, archived, linked, or pointer-only instead of remaining normal source files.
- Decision: people should normally read HTML; AI should resume from concise Markdown state/devlog files.
- Current repo note: this changed the shared migration prompt only. The LETHE folder structure itself still needs a follow-up migration from the older flat `docs/orchestration/` layout to `interface/` and `state/`.
- Verification:
  - `npm run report`: pass, generated 8 unit reports.
  - `npm run report:check`: pass, 8 report units.
  - `npm run report:discord:unit:dry`: pass, latest unit 08 summary generated.
  - `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- Discord:
  - `npm run report:discord:unit` was rejected by the approval reviewer because it would send workspace-generated report content and attachments to an external Discord webhook, which is untrusted by default and not explicitly approved by the user.
  - Next trusted-local command: `npm run report:discord:unit`.

## v0.12 Balance Loop Rerun

- Goal: run the requested balance test against the current v0.12 prototype state.
- Command:
  - `npm run balance:loop`
- Result:
  - verdict: `ITERATE_BALANCE`,
  - first boss clear: `100%`,
  - full clear: `20%`,
  - death: `60%`,
  - first boss TTK median: `26.42s`,
  - failed checks: clear rate minimum and death rate maximum,
  - death phase cluster: `망각 전조` in 3 death runs.
- Generated:
  - `docs/balance/2026-06-08-v012-balance-qa.md`,
  - `docs/review_prompts/2026-06-08-balance-loop.md`.
- Changed:
  - updated `STATUS.md`, `CURRENT_TASK.md`, `NEXT_TASKS.md`, `index.html`, and `command.html` so human sessions are no longer the immediate next gate,
  - next work is review/select one small balance adjustment, then rerun balance verification.
- Decision: pause controlled human sessions until balance gate is restored or the user explicitly accepts the risk.
- Verification:
  - `npm run report`: pass, generated 9 unit reports.
  - `npm run report:check`: pass, 9 report units.
  - `npm run report:discord:unit:dry`: pass, latest unit 09 summary generated and attached the balance review prompt.
  - `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- Discord:
  - `npm run report:discord:unit` was rejected by the approval reviewer because it would send workspace-generated report content and attachments to an external Discord webhook, which is untrusted by default and not explicitly approved by the user.
  - Next trusted-local command: `npm run report:discord:unit`.
