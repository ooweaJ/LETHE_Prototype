# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, legacy `docs/NEXT_TASKS.md`, or evidence files.

## 1. Run Jaewoo Solo Feel Test

- Priority: high
- Why: the real gate is human feel, not simulator emotion proxy.
- How: open `dist\lethe-v0.12-playtest\index.html` and play the new highest-level forgetting loop.
- Observe: highest-level loss regret vs irritation, visible echo change, resonance excitement, and conscious leveling tradeoff.
- Evidence: save notes under `playtest_logs/` or `docs/orchestration/evidence/`.

## 2. Record Human Test Notes

- Priority: high
- Why: Unity transition should depend on concrete feel evidence.
- How: save jaewoo notes under `playtest_logs/` or `docs/orchestration/evidence/`, then summarize in report/devlog.
- Include: exact moment of loss, echo visibility, reacquisition reaction, and whether debug buttons were used.

## 3. Interpret Feel Gate

- Priority: medium
- Why: the next decision is either another narrow HTML iteration or Unity backlog.
- How: compare notes against the four human-test questions in `CURRENT_TASK.md`.
- Blocker: do not use AI proxy metrics as the GO decision.

## 4. Tune One Lever Only If Human Test Fails Narrowly

- Priority: medium
- Why: broad tuning would blur whether the new forgetting model is working.
- How: choose one small lever, document why, rerun the relevant gate.
- Blocker: do not tune from AI emotion proxy.

## 5. Resume Unity Backlog Only After Feel Gate

- Priority: medium
- Why: Unity setup is intentionally excluded from this round.
- How: if the solo test supports the loop, convert the frozen rules into the Unity vertical-slice backlog.
- Blocker: do not start Unity before the HTML feel gate is recorded.
