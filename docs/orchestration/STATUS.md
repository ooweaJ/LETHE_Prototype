# Status

Last updated: 2026-06-08

## Current Snapshot

LETHE HTML Alpha v0.12 is the current controlled human-test candidate. The latest balance-loop gate passed, and the v0.12 playtest package has been regenerated.

The orchestration HTML interface now exists at `docs/orchestration/index.html`, `docs/orchestration/command.html`, and `docs/orchestration/runbook.html`; Markdown remains the source of truth. The dashboard is a Korean human-facing report screen for deciding the next prompt.

## Latest Verified Result

- Final browser `first_boss_ttk`: `GO_BALANCE_BASELINE`, 3/3 accepted samples, first boss TTK median `18.61s`.
- Final `npm run balance:loop`: `GO_BALANCE_BASELINE`, first boss clear `100%`, full clear `60%`, death `40%`, first boss TTK median `20.73s`.
- `npm run playtest:package:dry`: pass.
- `npm run playtest:package`: pass, regenerated `dist\lethe-v0.12-playtest`.
- `npm run report`: pass.
- `npm run report:check`: pass.
- `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- `npm run report:discord:unit:dry`: pass.
- `npm run report:discord:unit`: blocked by approval reviewer; next trusted-local command is `npm run report:discord:unit`.
- `docs/orchestration/index.html`: present and updated for the current v0.12 human-test gate.
- `docs/orchestration/command.html`: present as the compact next-instruction block.
- `docs/orchestration/runbook.html`: present as the operating-procedure block.
- `docs/orchestration/reports/index.html`: present as a human-readable report list.
- `docs/orchestration/devlog/index.html`: present as a human-readable devlog list.
- Latest dashboard refresh reporting: `npm run report`, `npm run report:check`, `npm run report:discord:unit:dry`, and `npm run doctor` passed; actual Discord send was blocked by approval reviewer.
- Latest Korean dashboard normalization reporting: `npm run report`, `npm run report:check`, `npm run report:discord:unit:dry`, and `npm run doctor` passed; actual Discord send was blocked by approval reviewer.

## Current Blocker

Human reaction evidence is still missing. Automated balance evidence is not Unity-transition proof.

Discord actual send from this Codex session was blocked by the approval reviewer because sending workspace-generated reports/attachments to an external webhook is treated as potential private data exfiltration. Next trusted-local command remains `npm run report:discord:unit`.

## Current Next Step

Run controlled human sessions, place downloaded JSON logs in `playtest_logs/`, then run `npm run playtest:summary`.

## Current Source Of Truth

- Top-level rules: `AGENTS.md`
- Detailed legacy status: `docs/CODEX_STATUS.md`
- Detailed legacy task archive: `docs/NEXT_TASKS.md`
- Current orchestration task: `docs/orchestration/CURRENT_TASK.md`
