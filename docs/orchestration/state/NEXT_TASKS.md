# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, legacy `docs/NEXT_TASKS.md`, or evidence files.

## 1. Freeze Unity Core System Rules

- Priority: high
- Why: memory/echo/resonance rules now define the Unity transition.
- How: review the Korean docs under `docs/design/`, especially `LETHE_GAME_DESIGN_OVERVIEW.md`, `LETHE_CORE_SYSTEMS_UNITY_PLAN.md`, and `LETHE_UNITY_VERTICAL_SLICE_SPEC.md`. The current frozen rules are highest-level memory forgetting, echo `+5` cap, overflow-to-overcharge, reacquisition resonance, echo `+5` awakening, and ultimate echo condition.
- Verification: approved rules are reflected in a Unity backlog or implementation prompt.
- Blocker: reacquisition formula and exact overcharge payout still need first-slice tuning.

## 2. Create Unity Vertical-Slice Backlog

- Priority: high
- Why: the next implementation step should be a scoped Unity slice, not more HTML micro-tuning.
- How: break `LETHE_UNITY_VERTICAL_SLICE_SPEC.md` into data model, combat prefab, UI/debug panel, memory level, echo level, forgetting, reacquisition, awakened echo, and synergy tasks.
- Include: debug controls for immediate forgetting, echo `+5`, and ultimate echo demonstration.
- Verification: backlog has ordered tasks with done criteria and validation method.
- Blocker: core rules should be frozen first.

## 3. Choose First Echo Showcase

- Priority: high
- Why: Unity needs one memorable echo power spike to prove the fantasy.
- How: start from the recommended showcase: 칼무리 잔향 + 혈반 잔향 -> 피의 칼폭풍.
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
