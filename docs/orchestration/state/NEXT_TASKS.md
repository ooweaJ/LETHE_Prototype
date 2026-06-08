# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, legacy `docs/NEXT_TASKS.md`, or evidence files.

## 1. Review Balance Loop Failure

- Priority: high
- Why: latest `balance:loop` returned `ITERATE_BALANCE`, full clear `20%`, death `60%`.
- How: read `docs/balance/2026-06-08-v012-balance-qa.md` and `docs/orchestration/review_prompts/2026-06-08-balance-loop.md`.
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

## 5. Wire Project Orchestrator Discord Intake

- Priority: low
- Why: the current shared plugin rule says Discord delivery should go through Project Orchestrator when available, while this repo still has local direct-send scripts as fallback.
- How: add or document the central intake command/API once Project Orchestrator is available, then keep `npm run report:discord:unit` as trusted-local fallback only.
- Verification: run `npm run report`, `npm run report:check`, and a central-intake dry run or documented fallback dry run.
- Blocker: Project Orchestrator intake command/API is not present in this repository yet.
