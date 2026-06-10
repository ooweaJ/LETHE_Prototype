# Current Task

## Goal

Commit the Unity 2D project skeleton safely after confirming AnkleBreaker Unity MCP connectivity and removing obsolete Unity MCP settings.

## Why Now

The user created/opened the Unity project. Before implementing gameplay, the repository needs a clean Unity baseline commit: generated folders ignored, obsolete MCP settings removed, only actual project skeleton files tracked, and AnkleBreaker MCP connectivity recorded.

The design must answer:

- Is the active MCP server AnkleBreaker Unity MCP?
- Are obsolete gamelovers/coplaydev MCP registrations removed?
- Are Unity generated folders/files ignored?
- Which Unity skeleton files should be committed?
- Is the next implementation document still `LETHE_UNITY_ASSET_BINDING_PLAN.md`?

## Done Criteria

- AnkleBreaker MCP is registered as `anklebreaker-unity`.
- Unity bridge port `7890` responds.
- Obsolete `mcp-unity` and `unityMCP` registrations are removed.
- `.gitignore` excludes Unity generated files.
- Obsolete `LETHE/ProjectSettings/McpUnitySettings.json` is removed.
- Unity skeleton files under `LETHE/Assets`, `LETHE/Packages`, and `LETHE/ProjectSettings` are committed.
- Report/devlog/state docs record the Unity baseline.

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

- Should the first MCP-driven Unity task create `Assets/Lethe/` folders and import the concept sheet?
- Should transparent runtime sprite generation happen before or after the first placeholder scene?

## Do Not Touch

Do not add new memories, weapons, enemies, shop, meta progression, multi-region structure, or final boss. Unity setup is now allowed only for the first-slice baseline and `LETHE_UNITY_ASSET_BINDING_PLAN.md`.
