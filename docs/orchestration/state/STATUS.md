# Status

Last updated: 2026-06-10

## Current Snapshot

LETHE HTML Alpha v0.12 implemented the new forgetting model and passed automated regression, but the user feedback is that the play experience still does not feel like a big change. The current task has shifted from raw rule implementation to a Unity-ready echo slice: echoes should not feel like `잔향!` labels on basic attacks, but like weapon-specific combat events supported by concrete images, `_dev` prefabs, debug scene states, and one-person feel tests.

The new design sources are `docs/design/LETHE_FORGETTING_FEEL_SPEC.md`, `docs/design/LETHE_WEAPON_MEMORY_ECHO_SPEC.md`, `docs/design/LETHE_WEAPON_MEMORY_ECHO_DETAIL.md`, `docs/design/LETHE_ECHO_FORM_TRANSFORMATION_SPEC.md`, `docs/design/LETHE_UNITY_ECHO_SYSTEM_PRD.md`, `docs/design/LETHE_VISUAL_ASSET_PLAN.md`, `docs/design/LETHE_UNITY_SLICE_ASSET_PRODUCTION_PLAN.md`, and `docs/design/LETHE_UNITY_ASSET_BINDING_PLAN.md`. Together they define the first showcase, active-to-echo form transformation, loop-safe hit event taxonomy, Unity class roles, ScriptableObjects, prefabs, first-slice acceptance criteria, the first sprite/VFX concept sheet, the imagegen production plan, and the file-to-prefab-to-scene binding map for Unity MCP. No gameplay code should be added until this combat fantasy, image plan, architecture, and visual direction target is accepted or revised.

The orchestration HTML interface now exists at `docs/orchestration/interface/index.html`, `docs/orchestration/interface/command.html`, and `docs/orchestration/interface/runbook.html`. AI-facing state lives under `docs/orchestration/state/`; human-facing reports live under `docs/orchestration/reports/YYYYMMDD/`.

The report/devlog/review migration is now applied physically: old `docs/reports/` daily files moved to `docs/orchestration/reports/YYYYMMDD/index.md|html`, old unit reports moved to `docs/orchestration/reports/YYYYMMDD/units/`, old `docs/devlog/` files moved to `docs/orchestration/devlog/YYYYMMDD.md`, and old review prompt/response files moved to `docs/orchestration/review_prompts/` and `docs/orchestration/review_responses/`. New work should not recreate legacy `docs/reports/`, `docs/devlog/`, `docs/review_prompts/`, or `docs/review_responses/` as normal source-of-truth folders.

The current development-docs plugin baseline from `docs/orchestration/MIGRATION_PROMPT.md` has been applied without committing: `AGENTS.md` now uses a `Development Docs Plugin` section, `docs/orchestration/templates/HTML_INTERFACE_TEMPLATE.md` exists, legacy review pointer READMEs are readable, `reports/index.html` is generated as a newest-first date archive, daily report pages are generated as unit-card pages, and Discord delivery is documented as Project Orchestrator first with local direct-send scripts as trusted fallback only.

## Latest Verified Result

- New forgetting model `npm run qa:balance`: `GO_BALANCE_BASELINE`, first boss clear `100%`, full clear `60%`, death `40%`, first boss TTK median `20.19s`.
- New forgetting model `npm run balance:loop`: `GO_BALANCE_BASELINE`, first boss clear `80%`, full clear `60%`, death `40%`, first boss TTK median `23.91s`.
- `npm run playtest:package:dry`: pass.
- `npm run playtest:package`: pass, wrote `dist\lethe-v0.12-playtest`.
- `npm run report`: pass, regenerated 2026-06-10 report HTML and report archive.
- `npm run report:check`: pass, one 2026-06-10 unit report verified.
- `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- Forgetting feel spec reporting: `npm run report` pass, regenerated 2026-06-10 report with 2 unit reports.
- Forgetting feel spec reporting: `npm run report:check` pass, 2 unit headings verified.
- Accepted recovery lever for new model: player max HP `190 -> 210`.
- New planning response saved: `docs/orchestration/review_responses/2026-06-10-forgetting-model-gate.md`.
- New feel design spec saved: `docs/design/LETHE_FORGETTING_FEEL_SPEC.md`.
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
- Unity first-slice rule freeze update: highest-level memory forgetting, echo overflow-to-overcharge, and balance target changes were reflected in `docs/design/`.
- Legacy docs current-state sync: `docs/CODEX_STATUS.md` and `docs/NEXT_TASKS.md` now summarize the Unity-transition direction and planned echo system at the top.
- Korean design docs split: `docs/design/README.md`, `LETHE_GAME_DESIGN_OVERVIEW.md`, `LETHE_CORE_SYSTEMS_UNITY_PLAN.md`, `LETHE_RUN_STRUCTURE.md`, `LETHE_COMBAT_DESIGN.md`, `LETHE_CONTENT_TABLES.md`, `LETHE_BALANCE_BASELINE.md`, and `LETHE_UNITY_VERTICAL_SLICE_SPEC.md`.
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

The current blocker has moved from setup choice to first-slice Unity development order. The Unity 2D project skeleton now exists under `LETHE/`, AnkleBreaker Unity MCP is registered as `anklebreaker-unity`, and the Unity editor bridge is reachable on port `7890`. `Assets/_dev` and its development subfolders now exist and were confirmed through `unity_asset_list`, so the next work should produce/import the first resources, build runtime foundations, and assemble a debug combat slice.

Discord actual send for the latest orchestration structure cleanup succeeded after explicit user request. Historical approval blocks remain recorded in the relevant devlog/report entries.

Project Orchestrator Discord intake is now connected through `scripts/send_orchestrator_discord_report.js` and the `report:orchestrator:*` npm scripts. The legacy `npm run report:discord:unit:dry` and `npm run report:discord:unit` commands remain documented as trusted-local fallback commands only.

## Current Next Step

Begin the first Unity `_dev` game slice from `docs/design/LETHE_UNITY_SLICE_ASSET_PRODUCTION_PLAN.md` and `docs/design/LETHE_UNITY_ECHO_SYSTEM_PRD.md`: produce/import the basic resources, add runtime foundations, then assemble `Dev_EchoSlice.unity` with a basic combat/debug loop.

For reporting/Discord notification, use `npm run report:orchestrator:unit:dry` before real sends, then `npm run report:orchestrator:unit` when the Project Orchestrator is running and the report is ready.

## Current Source Of Truth

- Top-level rules: `AGENTS.md`
- Detailed legacy status archive: `docs/CODEX_STATUS.md`
- Detailed legacy task archive: `docs/NEXT_TASKS.md`
- Current orchestration task: `docs/orchestration/state/CURRENT_TASK.md`
