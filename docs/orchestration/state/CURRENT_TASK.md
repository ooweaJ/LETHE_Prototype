# Current Task

## Goal

Prepare the Unity first-slice asset binding map: link character, map, weapon, echo VFX, concept references, prefabs, ScriptableObjects, and scene placement so Unity MCP can execute from the document.

## Why Now

The first VFX concept sheet exists, but it is mostly weapon/effect direction. Before creating the Unity 2D project, the project needs a clearer binding map that says character uses which file, map uses which file, sword uses which file, and echo effects use which generated image or future transparent sprite. The goal is to make the document executable by Unity MCP.

The design must answer:

- Which current image can be imported into Unity now, and is it runtime or reference?
- Which character, map, weapon, echo, enemy, and UI prefabs need which image files?
- Which missing images can be placeholder assets, and which need imagegen next?
- Which ScriptableObjects link to which prefabs?
- What exact Unity MCP sequence should create folders, import assets, create prefabs, and place the scene?

## Done Criteria

- `docs/design/LETHE_UNITY_ASSET_BINDING_PLAN.md` exists.
- It maps current concept art to Unity `Art/Concept/`.
- It maps character, map, weapon, Kalmuri, Blood, Blood Blade Storm, enemy, and UI assets to prefabs.
- It marks missing files as placeholder or next imagegen target.
- It defines the Unity MCP execution sequence.
- Report/devlog/state docs record that Unity asset binding is ready before Unity setup.
- No gameplay code is added in this planning unit.

## Related Files

- `docs/design/LETHE_WEAPON_MEMORY_ECHO_SPEC.md`
- `docs/design/LETHE_WEAPON_MEMORY_ECHO_DETAIL.md`
- `docs/design/LETHE_ECHO_FORM_TRANSFORMATION_SPEC.md`
- `docs/design/LETHE_UNITY_ECHO_SYSTEM_PRD.md`
- `docs/design/LETHE_VISUAL_ASSET_PLAN.md`
- `docs/design/LETHE_UNITY_ASSET_BINDING_PLAN.md`
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

- Should the next step be Unity 2D project creation or transparent runtime sprite generation?
- Should player/map/enemy stay placeholder for first slice?
- Should the concept sheet be imported only as reference art in Unity?

## Do Not Touch

Do not add new memories, weapons, enemies, shop, meta progression, multi-region structure, final boss, or Unity setup while this design is being clarified.
