# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, legacy `docs/NEXT_TASKS.md`, or evidence files.

## 1. Run Controlled Human Sessions

- Priority: high
- Why: HP `180 -> 190` restored the automated balance gate in two consecutive loops, so the next missing evidence is human response.
- How: use `dist\lethe-v0.12-playtest`, `docs/HUMAN_PLAYTEST_GUIDE.md`, and `docs/PLAYTEST_NOTES.md`.
- Verification: downloaded JSON logs or written playtest notes exist in `playtest_logs/` or an agreed evidence path.
- Blocker: needs a human tester/session.

## 2. Summarize Human Playtest Evidence

- Priority: high after session
- Why: Unity-transition judgment needs human evidence, not only automated balance.
- How: run `npm run playtest:summary` after logs exist.
- Verification: summary report records clear moments, deaths, confusion, regret/irritation, and restart intent.
- Blocker: no human logs yet.

## 3. Decide Whether v0.12 Needs Another Tiny Tune

- Priority: medium after human evidence
- Why: HP `190` passes automated gates, but humans may still find the forgetting loop unfair or too soft.
- How: choose at most one small balance lever from the evidence.
- Verification: `npm run balance:loop` remains `GO_BALANCE_BASELINE`.
- Blocker: do not tune blindly before human evidence unless the user explicitly asks.

## 4. Refresh Playtest Package If Needed

- Priority: done unless source changes again
- Why: the playtest package should include HP `190`.
- How: `npm run playtest:package:dry` and `npm run playtest:package` were run.
- Verification: `dist\lethe-v0.12-playtest` was regenerated.
- Blocker: rerun only if source changes before the session.

## 5. Keep Project Orchestrator Discord Intake Ready

- Priority: low
- Why: report delivery should use central intake when available.
- How: use `npm run report:orchestrator:unit:dry` before a real send.
- Verification: Orchestrator dry-run accepts the latest report path.
- Blocker: Project Orchestrator must be running.
