# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, legacy `docs/NEXT_TASKS.md`, or evidence files.

## 1. Review Weapon/Memory/Echo Spec

- Priority: high
- Why: current echoes feel like labels/procs, not combat fantasy.
- How: read `docs/design/LETHE_WEAPON_MEMORY_ECHO_SPEC.md`.
- Decide: are dual blades, greatsword, the 8 echoes, and `피의 칼폭풍` concrete enough?
- Blocker: do not implement more generic `잔향!` feedback.

## 2. Lock First Showcase

- Priority: high
- Recommendation: `절단쌍검 + 굶주린 칼무리 + 피의 반사 -> 피의 칼폭풍`.
- Done: one weapon rhythm, two active memories, two awakened echoes, one ultimate echo, one debug loop.
- Blocker: if this showcase does not sound exciting on paper, revise before code.

## 3. Choose Next Implementation Surface

- Priority: high
- Options: HTML showcase pass or Unity first-slice backlog.
- Recommendation: implement the first showcase in HTML only if it can prove the moment quickly; otherwise use the spec as Unity contract.
- Include: no new memories, no new weapon roster.

## 4. Convert Spec To Tasks

- Priority: medium
- Include: dual-blade hit rhythm, 칼무리 잔칼, 혈반 피 실, 공명 card, 피의 칼폭풍 HUD/trigger, one-button demo.
- Done: tasks have visible success criteria, not just numeric balance criteria.

## 5. Keep The Second Showcase As Backup

- Priority: medium
- Candidate: `장송대검 + 처형자의 섬광 + 망각의 낙인 -> 처형 각인`.
- Why: it gives a contrasting slow/big-hit fantasy after the fast blood-blade fantasy.
