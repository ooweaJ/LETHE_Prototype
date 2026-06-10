# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, legacy `docs/NEXT_TASKS.md`, or evidence files.

## 1. Produce Basic Slice Resources

- Priority: high
- Why: the Unity slice needs player/enemy/map/weapon readability before echo VFX can be judged.
- Include: player silhouette, walker enemy, dark floor tile, left/right dual blades.
- Method: Codex imagegen, chroma-key/alpha cleanup, then MCP import.
- Source: `docs/design/LETHE_UNITY_SLICE_ASSET_PRODUCTION_PLAN.md`.

## 2. Build Unity Runtime Foundations

- Priority: high
- Include: `RunBuildState`, definition data classes/SOs, `HitEvent`, `HitResolver`, `EchoTriggerRouter`, `PoolService`, feedback service stubs.
- Constraint: keep the first implementation `_dev` scoped and minimal.
- Done: scripts compile and Unity console has no errors.

## 3. Assemble Basic Combat Scene

- Priority: medium
- Include: `Dev_EchoSlice.unity`, player anchors, test enemy, dark arena, dual blades, basic hitbox, damage/flash feedback.
- Done: basic 쌍검 타격이 enemy에 맞고 읽힌다.

## 4. Implement First Echo Debug Loop

- Priority: medium
- Include: Kalmuri +1/+5, Blood +5, Blood Blade Storm, and `UI_DebugEchoPanel`.
- Done: debug state switches can show basic attack, Kalmuri +1/+5, Blood +5, and Blood Blade Storm.

## 5. Promote Surviving Dev Structure

- Priority: low
- When: only after jaewoo GO review.
- Include: promote approved resources, scripts, data, prefabs, and scene from `Assets/_dev` to `Assets/Lethe`.
- Done: ids remain stable while paths move to production structure.
