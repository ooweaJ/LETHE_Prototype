# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, legacy `docs/NEXT_TASKS.md`, or evidence files.

## 1. Assemble Basic Combat Scene

- Priority: medium
- Include: `Dev_EchoSlice.unity`, player anchors, test enemy, dark arena, dual blades, basic hitbox, damage/flash feedback.
- Done: basic 쌍검 타격이 enemy에 맞고 읽힌다.

## 2. Generate Core Echo VFX Sprites

- Priority: medium
- Include: Kalmuri slash, Kalmuri orbit/launch blade, blood mark, heal thread tip, and one Blood Blade Storm blade.
- Method: Codex imagegen, alpha cleanup, MCP import, then prefab binding.
- Done: each sprite has a concrete prefab/runtime target in `LETHE_UNITY_ASSET_BINDING_PLAN.md`.

## 3. Implement First Echo Debug Loop

- Priority: medium
- Include: Kalmuri +1/+5, Blood +5, Blood Blade Storm, and `UI_DebugEchoPanel`.
- Done: debug state switches can show basic attack, Kalmuri +1/+5, Blood +5, and Blood Blade Storm.

## 4. Prepare Promotion Gate

- Priority: low
- When: only after jaewoo GO review.
- Include: checklist for promoting approved resources, scripts, data, prefabs, and scene from `Assets/_dev` to `Assets/Lethe`.
- Done: ids remain stable while paths move to production structure; no promotion before review.
