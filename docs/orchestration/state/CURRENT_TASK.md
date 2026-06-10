# Current Task

## Goal

Audit and strengthen the Unity game-development architecture contract before starting implementation.

## Why Now

The Unity skeleton and `_dev` folder now exist, AnkleBreaker MCP can see `Assets/_dev`, and the first slice production plan exists. Before creating assets or scripts, the technical docs need to prove that the Unity structure is OOP-friendly and data-driven enough for future weapons, memories, echoes, prefabs, and images to be added without rewriting core services.

The design must answer:

- Do `WeaponDefinition`, `MemoryDefinition`, `EchoDefinition`, and `EchoSynergyDefinition` support adding new content as data?
- Are runtime classes separated from core services enough to avoid `if echoId == ...` growth?
- Are image, prefab, class, and ScriptableObject paths concrete enough for MCP implementation?
- Is `_dev` staging clearly separated from `Assets/Lethe` promotion?
- Is Discord notification part of the document/development workflow?

## Done Criteria

- `LETHE_UNITY_ECHO_SYSTEM_PRD.md` includes OOP/data extension rules.
- `LETHE_UNITY_ASSET_BINDING_PLAN.md` clearly separates `_dev` staging from `Assets/Lethe` promotion.
- `SCOPE_GUARD` and `RUNBOOK` reflect the Unity `_dev` gate and Discord notification rule.
- Report/devlog/state docs record the architecture audit and next implementation sequence.
- Discord notification is sent for this document unit.

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
- `docs/orchestration/state/RUNBOOK.md`
- `docs/orchestration/state/SCOPE_GUARD.md`
- `docs/orchestration/state/NEXT_TASKS.md`
- `docs/orchestration/reports/20260610/index.md`

## Verification Commands

```bash
npm run report
npm run report:check
```

## Open Questions

- Should the first Unity implementation use abstract base classes only, or interfaces plus abstract base classes? Current decision: both, with abstract `MonoBehaviour` base classes for Unity serialization.

## Do Not Touch

Do not add new memories, weapons, shop, meta progression, multi-region structure, or final boss. Use `Assets/_dev` for experimental Unity slice work until the first echo slice earns promotion to `Assets/Lethe`.
