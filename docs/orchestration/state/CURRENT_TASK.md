# Current Task

## Goal

Prepare the Unity first-slice visual asset direction: generate a concept sheet, define sprite/VFX parts, map them to prefabs, and record how Unity MCP should import/connect them later.

## Why Now

The Unity echo PRD now defines classes, ScriptableObjects, prefabs, and event boundaries. Before creating the Unity 2D project, the first slice also needs a visual asset direction so the echo system has concrete sprites/VFX to aim at. The goal is not to create final production art yet; it is to establish the first concept sheet and import plan.

The design must answer:

- What concept image anchors the first `피의 칼폭풍` visual direction?
- Which sprite/VFX parts should be generated first?
- Which Unity prefabs consume each sprite/VFX part?
- What should Unity MCP do after the Unity project exists?
- Which assets are concept-only and which need transparent sprite generation?

## Done Criteria

- A first echo showcase concept image exists under `docs/design/assets/`.
- `docs/design/LETHE_VISUAL_ASSET_PLAN.md` exists.
- The asset plan maps concept image regions to Unity prefabs.
- The asset plan lists the first transparent sprite/VFX generation priorities.
- The asset plan explains how Unity MCP should import/connect assets later.
- Report/devlog/state docs record that the visual asset direction is ready before Unity setup.
- No gameplay code is added in this planning unit.

## Related Files

- `docs/design/LETHE_WEAPON_MEMORY_ECHO_SPEC.md`
- `docs/design/LETHE_WEAPON_MEMORY_ECHO_DETAIL.md`
- `docs/design/LETHE_ECHO_FORM_TRANSFORMATION_SPEC.md`
- `docs/design/LETHE_UNITY_ECHO_SYSTEM_PRD.md`
- `docs/design/LETHE_VISUAL_ASSET_PLAN.md`
- `docs/design/assets/lethe-first-echo-showcase-concept.png`
- `docs/design/LETHE_FORGETTING_FEEL_SPEC.md`
- `docs/design/LETHE_COMBAT_DESIGN.md`
- `docs/design/LETHE_CONTENT_TABLES.md`
- `docs/orchestration/state/NEXT_TASKS.md`
- `docs/orchestration/reports/20260610/index.md`

## Verification Commands

```bash
npm run report
npm run report:check
```

## Open Questions

- Should the next image generation produce transparent runtime sprites or more concept variations?
- Should the Unity 2D project be created now?
- Should first transparent assets be chroma-key processed locally or created later inside a full art pass?

## Do Not Touch

Do not add new memories, weapons, enemies, shop, meta progression, multi-region structure, final boss, or Unity setup while this design is being clarified.
