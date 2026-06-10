# Current Task

## Goal

Register Unity game development as the active agent workflow.

## Why Now

The Unity skeleton and `_dev` folder now exist, AnkleBreaker MCP can see `Assets/_dev`, and the first slice production plan exists. The root agent rules still referenced the old HTML prototype workflow, so the top-level rules need to point future Codex sessions toward Unity game development while preserving report/devlog/git requirements. Image generation is only the first resource step, not the overall workflow.

The design must answer:

- What does Codex do next after the planning phase?
- Which Unity game-development workflow should be treated as mandatory?
- Which HTML rules are now legacy?
- Which report and orchestration rules must remain active?

## Done Criteria

- `AGENTS.md` makes Unity `_dev` game slice development the default agent workflow.
- `AGENTS.md` treats imagegen as a resource pipeline inside Unity development, not the whole goal.
- HTML autopilot/balance-loop work is marked legacy unless explicitly requested.
- Report/devlog/orchestration/git rules remain active.
- `PROMPT_CONTEXT`, decision log, devlog, and report record the rule transition.
- `NEXT_TASKS` remains the concrete list of upcoming Unity slice tasks.

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
- `AGENTS.md`
- `docs/orchestration/state/NEXT_TASKS.md`
- `docs/orchestration/reports/20260610/index.md`

## Verification Commands

```bash
npm run report
npm run report:check
```

## Open Questions

- None for the rule transition. Next execution starts with basic slice image generation and `_dev` MCP folder setup.

## Do Not Touch

Do not add new memories, weapons, shop, meta progression, multi-region structure, or final boss. Use `Assets/_dev` for experimental Unity slice work until the first echo slice earns promotion to `Assets/Lethe`.
