# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, legacy `docs/NEXT_TASKS.md`, or evidence files.

## 1. Create `Dev_Prototype_v0`

- Priority: highest
- Include: new scene, root hierarchy, camera, arena, player, enemy spawner, services, HUD root.
- Done: Play Mode opens into a real prototype scene, not the old echo slice.

## 2. Combat Loop First

- Priority: highest
- Include: player movement, nearest enemy targeting, weapon hit area, enemy HP/death/respawn, player HP/contact damage.
- Done: 30 seconds of movement/attack/pain/death/restart can be played.

## 3. Minimal HUD

- Priority: high
- Include: HP, kills, wave/time, active memory placeholder, echo placeholder.
- Done: player state is readable without reading debug panel text.

## 4. Memory Selection

- Priority: high
- Include: kill/time based selection, Kalmuri/Blood memory choices, level up, HUD update.
- Done: growth happens through gameplay, not debug keys.

## 5. Forgetting/Echo Loop

- Priority: high
- Include: highest-level memory forget, echo creation, echo level +1~+5, Kalmuri/Blood echo combat effects, resonance, Blood Blade Storm unlock.
- Done: LETHE's core loop works in Unity prototype form.
