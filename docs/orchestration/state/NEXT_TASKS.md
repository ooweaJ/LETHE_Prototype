# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, legacy `docs/NEXT_TASKS.md`, or evidence files.

## 1. Review Balance Loop Failure

- Priority: high
- Why: latest `balance:loop` returned `ITERATE_BALANCE`, full clear `20%`, death `60%`.
- How: read `docs/balance/2026-06-08-v012-balance-qa.md` and `docs/review_prompts/2026-06-08-balance-loop.md`.
- Verification: one small adjustment candidate is selected and documented.
- Blocker: requires direction on whether Codex should tune now or ask for external review first.

## 2. Apply One Small Balance Adjustment

- Priority: high after review
- Why: death rate is above the accepted gate.
- How: change exactly one balance lever from the v0.12 balance docs.
- Verification: changed file is documented and test command is rerun.
- Blocker: should not stack multiple blind tuning changes.

## 3. Rerun Balance Verification

- Priority: high after adjustment
- Why: balance gate must recover before controlled human sessions.
- How: run `npm run qa:balance`, then `npm run balance:loop` if the one-off result is acceptable.
- Verification: target death rate `<= 40%`, clear rate `>= 35%`, first boss TTK within `15-30s`.
- Blocker: local/browser automation environment must pass.

## 4. Run Controlled Human Sessions

- Priority: medium after balance gate recovers
- Why: automated balance alone is not Unity-transition proof.
- How: use `dist\lethe-v0.12-playtest`, `docs/HUMAN_PLAYTEST_GUIDE.md`, and `docs/PLAYTEST_NOTES.md`.
- Verification: downloaded JSON logs exist in `playtest_logs/`.
- Blocker: balance loop currently failed.

## 5. Automate Orchestration HTML Interface Refresh

- Priority: low
- Why: `docs/orchestration/interface/index.html`, `interface/command.html`, `interface/runbook.html`, `reports/index.html`, and `devlog/index.html` exist with clearer roles, but they should eventually be regenerated from Markdown instead of hand-maintained.
- How: add a generator that builds the Korean dashboard, command block, runbook block, and document-list pages from `state/STATUS.md`, `state/CURRENT_TASK.md`, `state/NEXT_TASKS.md`, `state/RUNBOOK.md`, `state/DECISION_LOG.md`, devlog, reports, and generated report units.
- Verification: running the generator updates the HTML pages and preserves Markdown as the source of truth.
- Blocker: should be done after the current dashboard shape feels useful.
