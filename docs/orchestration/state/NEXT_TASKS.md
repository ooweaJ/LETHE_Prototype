# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, legacy `docs/NEXT_TASKS.md`, or evidence files.

## 1. Freeze Unity Core System Rules

- Priority: high
- Why: memory/echo/resonance rules now define the Unity transition.
- How: review `docs/design/LETHE_CORE_SYSTEMS_UNITY_PLAN.md` and confirm the reacquisition formula, echo `+5` awakening rule, and ultimate echo condition.
- Verification: approved rules are reflected in a Unity backlog or implementation prompt.
- Blocker: needs user direction on first awakened echoes and first ultimate synergy.

## 2. Create Unity Vertical-Slice Backlog

- Priority: high
- Why: the next implementation step should be a scoped Unity slice, not more HTML micro-tuning.
- How: break the system plan into data model, combat prefab, UI/debug panel, memory level, echo level, forgetting, reacquisition, awakened echo, and synergy tasks.
- Verification: backlog has ordered tasks with done criteria and validation method.
- Blocker: core rules should be frozen first.

## 3. Choose First Echo Showcase

- Priority: high
- Why: Unity needs one memorable echo power spike to prove the fantasy.
- How: pick two awakened echoes and one ultimate echo synergy from the design doc.
- Verification: selected showcase has clear visual/combat behavior and can be implemented in a small arena.
- Blocker: avoid adding a large memory roster before the showcase works.

## 4. Keep HTML v0.12 As Evidence Build

- Priority: medium
- Why: HTML still contains useful balance and loop evidence.
- How: keep `dist\lethe-v0.12-playtest` as a playable reference and run human testing only if it helps answer Unity planning questions.
- Verification: any new human notes are saved and summarized.
- Blocker: do not let HTML tuning delay Unity system planning.

## 5. Define Ranged Enemy Behavior For Unity

- Priority: medium
- Why: current shooter enemies can back away, but Unity ranged enemies should not create annoying chase behavior.
- How: specify short backstep, stop-and-shoot, reposition, and rejoin-pressure rules.
- Verification: included in Unity enemy AI backlog and test criteria.
- Blocker: none.
