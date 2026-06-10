# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, legacy `docs/NEXT_TASKS.md`, or evidence files.

## 1. Jaewoo Morning Echo Slice Review

- Priority: medium
- Open: `LETHE/Assets/_dev/Scenes/Dev_EchoSlice.unity`.
- Press Play, then check `1` base, `2` Kalmuri +1, `3` Kalmuri +5, `4` Blood +5, `5` Blood Blade Storm, and `Space` forced attack.
- Use: `docs/orchestration/review_prompts/2026-06-11-unity-echo-slice-jaewoo-review.md`.
- Done: jaewoo returns `GO`, `ITERATE`, or `NO-GO`, weakest state, and first fix.

## 2. First ITERATE Fix After Review

- Priority: medium
- When: only after jaewoo review.
- Likely candidates: VFX scale/position/sorting, Blood heal thread readability, Kalmuri +5 orbit timing, Storm density, basic attack arc.
- Done: weakest reviewed state becomes readable enough for a second quick playtest.

## 3. Production Runtime Split

- Priority: medium
- When: after direction is confirmed by `GO` or a successful `ITERATE` pass.
- Include: move validated behavior out of `DevEchoSliceDebugController` into production echo runtime classes using `HitResolver`, `EchoTriggerRouter`, `PoolService`, and data assets.
- Done: debug panel only switches states; actual echo behavior lives in reusable runtime components.

## 4. Data Asset Binding

- Priority: low
- When: after runtime split begins.
- Include: create and bind `WeaponDefinition`, `MemoryDefinition`, `EchoDefinition`, and `EchoSynergyDefinition` assets for dual blades, Kalmuri, Blood, and Blood Blade Storm.
- Done: adding another weapon/memory later does not require hard-coded debug references.

## 5. `_dev -> Assets/Lethe` Promotion

- Priority: low
- When: only after jaewoo `GO`.
- Use: `docs/design/LETHE_UNITY_ECHO_SLICE_PROMOTION_GATE.md`.
- Done: approved resources, scripts, prefabs, data assets, and scene structure are moved into production paths with missing-reference and Play Mode checks passing.
