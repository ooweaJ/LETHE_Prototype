# Next Tasks

Keep this file short. Detailed history belongs in `devlog/`, `DECISION_LOG.md`, legacy `docs/NEXT_TASKS.md`, or evidence files.

## 1. Run Controlled Human Sessions

- Priority: high
- Why: automated balance passed, but human reaction evidence is missing.
- How: use `dist\lethe-v0.12-playtest`, `docs/HUMAN_PLAYTEST_GUIDE.md`, and `docs/PLAYTEST_NOTES.md`.
- Verification: downloaded JSON logs exist in `playtest_logs/`.
- Blocker: needs actual human session time.

## 2. Summarize Playtest Logs

- Priority: high after sessions
- Why: summary is needed before planning or Unity-transition judgment.
- How: place logs in `playtest_logs/`, then run `npm run playtest:summary`.
- Verification: summary output exists and is referenced from status/report.
- Blocker: requires playtest logs.

## 3. Create Planning Review Prompt From Human Evidence

- Priority: medium
- Why: Claude/GPT/Codex review should interpret human evidence before major scope changes.
- How: create a prompt under `docs/orchestration/review_prompts/` or legacy `docs/review_prompts/`.
- Verification: prompt references human logs, notes, and summary.
- Blocker: requires summarized human evidence.

## 4. Decide Whether To Tune HTML Or Prepare Unity Direction

- Priority: medium
- Why: the next implementation direction should follow human evidence.
- How: update `DECISION_LOG.md`, `STATUS.md`, and `NEXT_TASKS.md` after review.
- Verification: decision has evidence links and next task changes.
- Blocker: requires human summary and review response.

## 5. Automate Orchestration HTML Dashboard Refresh

- Priority: low
- Why: `docs/orchestration/index.html` exists, but it should eventually be regenerated from Markdown instead of hand-maintained.
- How: add a generator that builds `docs/orchestration/index.html` from `STATUS.md`, `CURRENT_TASK.md`, `NEXT_TASKS.md`, `DECISION_LOG.md`, devlog, and reports.
- Verification: running the generator updates the dashboard and preserves Markdown as the source of truth.
- Blocker: should be done after the current dashboard shape feels useful.
