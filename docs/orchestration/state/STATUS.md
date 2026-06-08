# Status

Last updated: 2026-06-08

## Current Snapshot

LETHE HTML Alpha v0.12 is no longer cleared for controlled human testing after the latest balance-loop rerun. The latest balance-loop gate returned `ITERATE_BALANCE`, so the next step is a small balance adjustment or review before human sessions.

The orchestration HTML interface now exists at `docs/orchestration/interface/index.html`, `docs/orchestration/interface/command.html`, and `docs/orchestration/interface/runbook.html`. AI-facing state lives under `docs/orchestration/state/`; human-facing reports live under `docs/orchestration/reports/YYYYMMDD/`.

The report/devlog migration is now applied physically: old `docs/reports/` daily files moved to `docs/orchestration/reports/YYYYMMDD/index.md|html`, old unit reports moved to `docs/orchestration/reports/YYYYMMDD/units/`, and old `docs/devlog/` files moved to `docs/orchestration/devlog/YYYYMMDD.md`. New work should not recreate legacy `docs/reports/` or `docs/devlog/` as normal source-of-truth folders.

## Latest Verified Result

- Latest `npm run balance:loop`: `ITERATE_BALANCE`, first boss clear `100%`, full clear `20%`, death `60%`, first boss TTK median `26.42s`.
- Failed checks: clear rate minimum `20% < 35%`; death rate maximum `60% > 40%`.
- Death phase concentration: `망각 전조` deaths in 3 runs.
- Generated balance report: `docs/balance/2026-06-08-v012-balance-qa.md`.
- Generated review prompt: `docs/review_prompts/2026-06-08-balance-loop.md`.
- Prior accepted baseline: `GO_BALANCE_BASELINE`, first boss clear `100%`, full clear `60%`, death `40%`, first boss TTK median `20.73s`.
- `npm run playtest:package:dry`: pass.
- `npm run playtest:package`: pass, regenerated `dist\lethe-v0.12-playtest`.
- `npm run report`: pass.
- `npm run report:check`: pass.
- `npm run report:discord:unit:dry`: pass, latest unit points to `docs/orchestration/reports/20260608/units/2026-06-08-10-오케스트레이션-리포트와-개발로그-실제-마이그레이션.html`.
- `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- `npm run report:discord:unit`: blocked by approval reviewer; next trusted-local command is `npm run report:discord:unit`.
- `npm run report:discord:unit`: blocked by approval reviewer; next trusted-local command is `npm run report:discord:unit`.
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

## Current Blocker

The latest balance loop failed. Human reaction evidence is still missing, but human sessions should wait until the balance gate is restored or the user explicitly accepts the risk.

Discord actual send from this Codex session was blocked by the approval reviewer because sending workspace-generated reports/attachments to an external webhook is treated as potential private data exfiltration. Next trusted-local command remains `npm run report:discord:unit`.

## Current Next Step

Review the generated balance prompt, choose the smallest balance adjustment, rerun balance verification, then return to controlled human sessions after the gate is acceptable.

## Current Source Of Truth

- Top-level rules: `AGENTS.md`
- Detailed legacy status: `docs/CODEX_STATUS.md`
- Detailed legacy task archive: `docs/NEXT_TASKS.md`
- Current orchestration task: `docs/orchestration/state/CURRENT_TASK.md`
