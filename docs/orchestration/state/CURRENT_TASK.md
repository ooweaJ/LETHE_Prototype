# Current Task

## Goal

Plan the first Unity echo slice as an image-first, MCP-driven production loop.

## Why Now

The Unity skeleton and `_dev` folder now exist, and AnkleBreaker MCP can see `Assets/_dev`. Before creating prefabs or gameplay code, the project needs a concrete slice plan that says which images Codex imagegen will make, where Unity MCP will import them, and which tests prove the echo fantasy works.

The design must answer:

- Which player/map/enemy/weapon/echo/ultimate images are required first?
- Which images are placeholders and which are slice-critical?
- How does each image connect to `_dev` prefabs and scene tests?
- What order should imagegen, MCP import, prefab assembly, and review follow?
- What makes the first slice a GO, ITERATE, or NO-GO?

## Done Criteria

- `LETHE_UNITY_SLICE_ASSET_PRODUCTION_PLAN.md` defines the image batches, MCP sequence, `_dev` folder structure, and slice tests.
- The plan fixes the slice visual concept, imagegen prompt pack, and image-to-prefab-to-class matrix.
- Existing Unity design docs reference the new plan in the right reading order.
- `STATUS`, `NEXT_TASKS`, and `PROMPT_CONTEXT` point to imagegen + MCP slice assembly as the next step.
- Report/devlog/state docs record the planning change.

## Related Files

- `docs/design/LETHE_WEAPON_MEMORY_ECHO_SPEC.md`
- `docs/design/LETHE_WEAPON_MEMORY_ECHO_DETAIL.md`
- `docs/design/LETHE_ECHO_FORM_TRANSFORMATION_SPEC.md`
- `docs/design/LETHE_UNITY_ECHO_SYSTEM_PRD.md`
- `docs/design/LETHE_VISUAL_ASSET_PLAN.md`
- `docs/design/LETHE_UNITY_SLICE_ASSET_PRODUCTION_PLAN.md`
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

- Should imagegen create the basic readability set first, or should MCP create a placeholder-only scene before any generated art?
- Should the first generated art be separate transparent sprites immediately, or larger contact sheets that we split later?

## Do Not Touch

Do not add new memories, weapons, shop, meta progression, multi-region structure, or final boss. Use `Assets/_dev` for experimental Unity slice work until the first echo slice earns promotion to `Assets/Lethe`.
