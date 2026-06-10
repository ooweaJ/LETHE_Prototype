# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, legacy `docs/NEXT_TASKS.md`, or evidence files.

## 1. Commit Unity Skeleton

- Priority: high
- Why: Unity project exists and should be versioned before MCP-driven edits.
- Include: `LETHE/Assets`, `LETHE/Packages`, `LETHE/ProjectSettings`.
- Exclude: `Library`, `Temp`, `Logs`, `UserSettings`, `.sln`, `.csproj`, `.vsconfig`.
- Blocker: do not commit generated Unity cache files.

## 2. Run First MCP Setup Pass

- Priority: high
- How: use AnkleBreaker Unity MCP after the tool surface reloads.
- Start: create `Assets/Lethe/` folders and import the concept sheet as reference art.
- Follow: `docs/design/LETHE_UNITY_ASSET_BINDING_PLAN.md`.

## 3. Generate Runtime VFX Sprites

- Priority: high
- Candidates: 칼무리 반달 칼선, 혈반 표식, 회복 실 끝점, +5 발사 칼날, 피의 칼폭풍 칼날.
- Constraint: concept sheet is reference only, not runtime sprite atlas.

## 4. Convert PRD To Unity Backlog

- Priority: medium
- Include: data SOs, hit event router, echo runtime, pool service, feedback services, debug panel.
- Done: each task has a prefab/class target and visual acceptance criterion.

## 5. Keep The Second Showcase As Backup

- Priority: medium
- Candidate: `장송대검 + 처형자의 섬광 + 망각의 낙인 -> 처형 각인`.
- Why: it gives a contrasting slow/big-hit fantasy after the fast blood-blade fantasy.
