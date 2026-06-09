# Status

Last updated: 2026-06-09

## Current Snapshot

LETHE HTML Alpha v0.12 is cleared again as a controlled human-test candidate after the latest one-lever balance recovery, but the project direction has now shifted from more HTML tuning toward Unity transition planning. The accepted tuning change remains player max HP `180 -> 190`; two consecutive `npm run balance:loop` runs returned `GO_BALANCE_BASELINE`. After the user clarified the desired echo fantasy, the current core-system direction is documented in `docs/design/LETHE_CORE_SYSTEMS_UNITY_PLAN.md`: memory and echo levels both cap at `+5`, forgotten memory levels accumulate into matching echoes, reacquired echoed memories gain resonance, and two `+5` echoes can unlock ultimate echo synergies.

The orchestration HTML interface now exists at `docs/orchestration/interface/index.html`, `docs/orchestration/interface/command.html`, and `docs/orchestration/interface/runbook.html`. AI-facing state lives under `docs/orchestration/state/`; human-facing reports live under `docs/orchestration/reports/YYYYMMDD/`.

The report/devlog/review migration is now applied physically: old `docs/reports/` daily files moved to `docs/orchestration/reports/YYYYMMDD/index.md|html`, old unit reports moved to `docs/orchestration/reports/YYYYMMDD/units/`, old `docs/devlog/` files moved to `docs/orchestration/devlog/YYYYMMDD.md`, and old review prompt/response files moved to `docs/orchestration/review_prompts/` and `docs/orchestration/review_responses/`. New work should not recreate legacy `docs/reports/`, `docs/devlog/`, `docs/review_prompts/`, or `docs/review_responses/` as normal source-of-truth folders.

The current development-docs plugin baseline from `docs/orchestration/MIGRATION_PROMPT.md` has been applied without committing: `AGENTS.md` now uses a `Development Docs Plugin` section, `docs/orchestration/templates/HTML_INTERFACE_TEMPLATE.md` exists, legacy review pointer READMEs are readable, `reports/index.html` is generated as a newest-first date archive, daily report pages are generated as unit-card pages, and Discord delivery is documented as Project Orchestrator first with local direct-send scripts as trusted fallback only.

## Latest Verified Result

- Accepted recovery change: player max HP `180 -> 190`.
- Latest `npm run balance:loop` pass 1 after HP `190`: `GO_BALANCE_BASELINE`, first boss clear `100%`, full clear `60%`, death `40%`, first boss TTK median `18.97s`.
- Latest `npm run balance:loop` pass 2 after HP `190`: `GO_BALANCE_BASELINE`, first boss clear `100%`, full clear `60%`, death `40%`, first boss TTK median `20.18s`.
- `npm run playtest:package:dry`: pass.
- `npm run playtest:package`: pass, wrote `dist\lethe-v0.12-playtest`.
- Echo clarity patch: pass, no numeric balance changes.
- `npm run qa:postloss` with `CHROME_PATH=C:\Program Files\Google\Chrome\Application\chrome.exe`: pass, failures `[]`.
- `npm run qa:identity` with `CHROME_PATH=C:\Program Files\Google\Chrome\Application\chrome.exe`: pass, failures `[]`.
- `npm run playtest:package:dry`: pass after echo clarity patch.
- `npm run playtest:package`: pass after echo clarity patch, regenerated `dist\lethe-v0.12-playtest`.
- Unity transition system plan: `docs/design/LETHE_CORE_SYSTEMS_UNITY_PLAN.md` created.
- Legacy docs current-state sync: `docs/CODEX_STATUS.md` and `docs/NEXT_TASKS.md` now summarize the Unity-transition direction and planned echo system at the top.
- Failed checks: none in the latest loop.
- Death phases in the latest loop: `압박 상승` 1, `망각 전조` 1.
- Generated balance report: `docs/balance/2026-06-09-v012-balance-qa.md`.
- Generated review prompt: `docs/orchestration/review_prompts/2026-06-09-balance-loop.md`.
- Rejected prior candidate: `laterCycleClimax 46 -> 42`; it worsened the loop to full clear `20%`, death `80%`, and was reverted.
- Previous failed baseline: `ITERATE_BALANCE`, first boss clear `100%`, full clear `20%`, death `60%`, first boss TTK median `26.42s`, death cluster `망각 전조` 3 runs.
- Previous evidence: `docs/balance/2026-06-08-v012-balance-qa.md`, `docs/orchestration/review_prompts/2026-06-08-balance-loop.md`.
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

The main blocker is no longer a single HTML balance gate. The next needed work is turning the newly defined memory/echo/resonance rules into a Unity vertical-slice backlog and confirming which first awakened echoes and ultimate echo synergy should be implemented.

Discord actual send for the latest orchestration structure cleanup succeeded after explicit user request. Historical approval blocks remain recorded in the relevant devlog/report entries.

Project Orchestrator Discord intake is now connected through `scripts/send_orchestrator_discord_report.js` and the `report:orchestrator:*` npm scripts. The legacy `npm run report:discord:unit:dry` and `npm run report:discord:unit` commands remain documented as trusted-local fallback commands only.

## Current Next Step

Review `docs/design/LETHE_CORE_SYSTEMS_UNITY_PLAN.md`, freeze the first-slice rules, then create the Unity vertical-slice backlog. Human testing of the HTML build remains useful as supporting evidence, but the next project step is system planning for Unity rather than another blind HTML tuning pass.

For reporting/Discord notification, use `npm run report:orchestrator:unit:dry` before real sends, then `npm run report:orchestrator:unit` when the Project Orchestrator is running and the report is ready.

## Current Source Of Truth

- Top-level rules: `AGENTS.md`
- Detailed legacy status archive: `docs/CODEX_STATUS.md`
- Detailed legacy task archive: `docs/NEXT_TASKS.md`
- Current orchestration task: `docs/orchestration/state/CURRENT_TASK.md`
