# Status

Last updated: 2026-06-09

## Current Snapshot

LETHE HTML Alpha v0.12 is no longer cleared for controlled human testing after the latest balance-loop rerun. The latest balance-loop gate returned `ITERATE_BALANCE`, so the next step is a small balance adjustment or review before human sessions.

The orchestration HTML interface now exists at `docs/orchestration/interface/index.html`, `docs/orchestration/interface/command.html`, and `docs/orchestration/interface/runbook.html`. AI-facing state lives under `docs/orchestration/state/`; human-facing reports live under `docs/orchestration/reports/YYYYMMDD/`.

The report/devlog/review migration is now applied physically: old `docs/reports/` daily files moved to `docs/orchestration/reports/YYYYMMDD/index.md|html`, old unit reports moved to `docs/orchestration/reports/YYYYMMDD/units/`, old `docs/devlog/` files moved to `docs/orchestration/devlog/YYYYMMDD.md`, and old review prompt/response files moved to `docs/orchestration/review_prompts/` and `docs/orchestration/review_responses/`. New work should not recreate legacy `docs/reports/`, `docs/devlog/`, `docs/review_prompts/`, or `docs/review_responses/` as normal source-of-truth folders.

The current development-docs plugin baseline from `docs/orchestration/MIGRATION_PROMPT.md` has been applied without committing: `AGENTS.md` now uses a `Development Docs Plugin` section, `docs/orchestration/templates/HTML_INTERFACE_TEMPLATE.md` exists, legacy review pointer READMEs are readable, `reports/index.html` is generated as a newest-first date archive, daily report pages are generated as unit-card pages, and Discord delivery is documented as Project Orchestrator first with local direct-send scripts as trusted fallback only.

## Latest Verified Result

- Latest `npm run balance:loop`: `ITERATE_BALANCE`, first boss clear `100%`, full clear `20%`, death `60%`, first boss TTK median `26.42s`.
- Failed checks: clear rate minimum `20% < 35%`; death rate maximum `60% > 40%`.
- Death phase concentration: `망각 전조` deaths in 3 runs.
- Generated balance report: `docs/balance/2026-06-08-v012-balance-qa.md`.
- Generated review prompt: `docs/orchestration/review_prompts/2026-06-08-balance-loop.md`.
- Prior accepted baseline: `GO_BALANCE_BASELINE`, first boss clear `100%`, full clear `60%`, death `40%`, first boss TTK median `20.73s`.
- `npm run playtest:package:dry`: pass.
- `npm run playtest:package`: pass, regenerated `dist\lethe-v0.12-playtest`.
- `npm run report`: pass.
- `npm run report:check`: pass.
- `npm run report:discord:unit:dry`: pass, latest unit points to `docs/orchestration/reports/20260608/units/2026-06-08-10-오케스트레이션-리포트와-개발로그-실제-마이그레이션.html`.
- `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- `npm run report:discord:unit`: blocked by approval reviewer; next trusted-local command is `npm run report:discord:unit`.
- `npm run report:discord:unit`: blocked by approval reviewer; next trusted-local command is `npm run report:discord:unit`.
- Latest orchestration structure cleanup verification: `npm.cmd run report`, `npm.cmd run report:check`, `npm.cmd run report:discord:unit:dry`, and `npm.cmd run doctor` passed. Actual Discord send succeeded after explicit user request. Latest commit `5949452` was pushed to `main`.
- `docs/orchestration/interface/index.html`: present and updated for the current v0.12 human-test gate.
- `docs/orchestration/interface/command.html`: present as the compact next-instruction block.
- `docs/orchestration/interface/runbook.html`: present as the operating-procedure block.
- `docs/orchestration/reports/index.html`: present as a human-readable date list.
- `docs/orchestration/reports/20260608/index.html`: current daily report page.
- `docs/orchestration/reports/20260608/units/`: current work-unit report collection.
- `docs/orchestration/devlog/index.html`: present as a human-readable devlog list.
- Latest dashboard refresh reporting: `npm run report`, `npm run report:check`, `npm run report:discord:unit:dry`, and `npm run doctor` passed; actual Discord send was blocked by approval reviewer.
- Latest Korean dashboard normalization reporting: `npm run report`, `npm run report:check`, `npm run report:discord:unit:dry`, and `npm run doctor` passed; actual Discord send was blocked by approval reviewer.
- Latest dashboard compaction: `docs/orchestration/interface/index.html` was shortened so it no longer duplicates `interface/command.html` or `reports/`.
- Latest development-docs plugin alignment: updated `AGENTS.md`, `docs/orchestration/README.md`, state runbook/context/tasks/decision log, legacy migration map, `docs/DISCORD_REPORTING.md`, and added `docs/orchestration/templates/HTML_INTERFACE_TEMPLATE.md`. Verification is this work unit's report/check pass.
- Latest template overwrite alignment: updated report generator so `docs/orchestration/reports/index.html` lists date journals newest-first with title/date/summary and `docs/orchestration/reports/YYYYMMDD/index.html` shows that day's unit cards. Unit pages now include a back link. New devlog guidance uses `YYYY-MM-DD.md`.

## Current Blocker

The latest balance loop failed. Human reaction evidence is still missing, but human sessions should wait until the balance gate is restored or the user explicitly accepts the risk.

Discord actual send for the latest orchestration structure cleanup succeeded after explicit user request. Historical approval blocks remain recorded in the relevant devlog/report entries.

Project Orchestrator Discord intake is now connected through `scripts/send_orchestrator_discord_report.js` and the `report:orchestrator:*` npm scripts. The legacy `npm run report:discord:unit:dry` and `npm run report:discord:unit` commands remain documented as trusted-local fallback commands only.

## Current Next Step

Review the generated balance prompt, choose the smallest balance adjustment, rerun balance verification, then return to controlled human sessions after the gate is acceptable.

For reporting/Discord notification, use `npm run report:orchestrator:unit:dry` before real sends, then `npm run report:orchestrator:unit` when the Project Orchestrator is running and the report is ready.

## Current Source Of Truth

- Top-level rules: `AGENTS.md`
- Detailed legacy status archive: `docs/CODEX_STATUS.md`
- Detailed legacy task archive: `docs/NEXT_TASKS.md`
- Current orchestration task: `docs/orchestration/state/CURRENT_TASK.md`
