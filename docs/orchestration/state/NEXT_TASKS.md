# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, legacy `docs/NEXT_TASKS.md`, or evidence files.

## 1. Generate Basic Slice Images

- Priority: high
- Why: the Unity slice needs player/enemy/map/weapon readability before echo VFX can be judged.
- Include: player silhouette, walker enemy, dark floor tile, left/right dual blades.
- Source: `docs/design/LETHE_UNITY_SLICE_ASSET_PRODUCTION_PLAN.md`.

## 2. Run MCP Dev Folder Setup

- Priority: high
- How: use AnkleBreaker Unity MCP on port `7890`.
- Start: create `_dev/Art`, `_dev/Prefabs`, `_dev/Scripts`, `_dev/Data`, `_dev/Scenes`.
- Follow: `docs/design/LETHE_UNITY_SLICE_ASSET_PRODUCTION_PLAN.md`.

## 3. Generate Core Echo VFX Sprites

- Priority: high
- Candidates: 칼무리 반달 칼선, 칼무리 고리 칼날, +5 발사 칼날, 혈반 표식, 회복 실 끝점.
- Constraint: one image file per runtime role; no mixed concept sheets for slice prefabs.

## 4. Assemble Dev Echo Scene

- Priority: medium
- Include: `Dev_EchoSlice.unity`, player, test enemy, arena, dual blades, debug echo panel.
- Done: debug state switches can show basic attack, Kalmuri +1/+5, Blood +5, and Blood Blade Storm.

## 5. Convert PRD To Unity Backlog

- Priority: medium
- Include: data SOs, hit event router, echo runtime, pool service, feedback services, debug panel.
- Done: each task has a prefab/class target and visual acceptance criterion.
