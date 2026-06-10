# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, legacy `docs/NEXT_TASKS.md`, or evidence files.

## 1. Review Visual Asset Plan

- Priority: high
- Why: Unity setup should know which sprites/VFX concept and import targets to use.
- How: read `docs/design/LETHE_VISUAL_ASSET_PLAN.md`.
- Decide: is the first concept sheet good enough as the art direction anchor?
- Blocker: do not import concept art as final runtime sprite atlas.

## 2. Choose Next Asset Pass

- Priority: high
- Options: transparent runtime sprites, more concept variations, or Unity project setup.
- Recommendation: generate 3~5 transparent runtime VFX sprites only after the concept direction is accepted.
- First candidates: 칼무리 반달 칼선, 혈반 표식, 회복 실 끝점, +5 발사 칼날, 피의 칼폭풍 칼날.

## 3. Choose Next Implementation Surface

- Priority: high
- Options: HTML showcase pass or Unity first-slice backlog.
- Recommendation: use HTML only for cheap rule visualization; use Unity when testing hitstop/sound/VFX/pooling matters.
- Include: no new memories, no new weapon roster, no meta systems.

## 4. Convert PRD To Unity Backlog

- Priority: medium
- Include: data SOs, hit event router, echo runtime, pool service, feedback services, debug panel.
- Done: each task has a prefab/class target and visual acceptance criterion.

## 5. Keep The Second Showcase As Backup

- Priority: medium
- Candidate: `장송대검 + 처형자의 섬광 + 망각의 낙인 -> 처형 각인`.
- Why: it gives a contrasting slow/big-hit fantasy after the fast blood-blade fantasy.
