# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, legacy `docs/NEXT_TASKS.md`, or evidence files.

## 1. Review Unity Echo System PRD

- Priority: high
- Why: Unity work should start from a class/event/prefab contract, not improvised proc code.
- How: read `docs/design/LETHE_ECHO_FORM_TRANSFORMATION_SPEC.md` and `docs/design/LETHE_UNITY_ECHO_SYSTEM_PRD.md`.
- Decide: are the class roles, ScriptableObjects, prefabs, and event boundaries enough for the first slice?
- Blocker: do not implement Unity echoes without `WeaponHit/EchoHit/UltimateHit` loop rules.

## 2. Lock First Showcase And Trigger Rules

- Priority: high
- Recommendation: `절단쌍검 + 굶주린 칼무리 + 피의 반사 -> 피의 칼폭풍`.
- Include: 망각 변환 연출, 칼무리 +1~+5, 혈반 +1~+5, 공명, 피의 칼폭풍.
- Decide: 궁극은 unlock 후 상시인지, 게이지 발동인지.
- Blocker: if +5 looks like active memory copy, revise form language.

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
