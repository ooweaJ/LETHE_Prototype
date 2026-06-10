# Current Task

## Goal

Produce and import the first Unity `_dev` readability resources for the echo slice.

## Why Now

The Unity skeleton and `_dev` folder now exist, AnkleBreaker MCP can see `Assets/_dev`, and the first slice production plan exists. Before runtime C# or prefab assembly, the project needs basic player/enemy/map/weapon sprites so the first combat scene has readable anchors and scale.

This task must answer:

- Can Codex generate usable first-pass images for the slice?
- Are transparent runtime sprites cleaned up enough for Unity use?
- Does AnkleBreaker MCP see the imported assets in `Assets/_dev/Art/Sprites`?
- Are source chroma files and Unity `.meta` files preserved cleanly?
- Is the next implementation step clear after this resource pass?

## Done Criteria

- Player silhouette, walker enemy, dark floor tile, and left/right dual blade sprites exist under `LETHE/Assets/_dev/Art/Sprites`.
- Original chroma source images are preserved under `LETHE/Assets/_dev/Art/Source`.
- Runtime sprites are imported through AnkleBreaker MCP and configured as Unity Sprite assets.
- `unity_asset_list` sees the runtime sprites and source textures.
- Report/devlog/state docs record the resource pass.
- Discord notification is attempted through Project Orchestrator after report generation; if the intake endpoint is unavailable, record the failed endpoint check and retry command.

## Related Files

- `docs/design/LETHE_WEAPON_MEMORY_ECHO_SPEC.md`
- `docs/design/LETHE_WEAPON_MEMORY_ECHO_DETAIL.md`
- `docs/design/LETHE_ECHO_FORM_TRANSFORMATION_SPEC.md`
- `docs/design/LETHE_UNITY_ECHO_SYSTEM_PRD.md`
- `docs/design/LETHE_VISUAL_ASSET_PLAN.md`
- `docs/design/LETHE_UNITY_SLICE_ASSET_PRODUCTION_PLAN.md`
- `docs/design/LETHE_UNITY_ASSET_BINDING_PLAN.md`
- `LETHE/Assets/_dev/Art/Source/*.png`
- `LETHE/Assets/_dev/Art/Sprites/**/*.png`
- `docs/orchestration/state/NEXT_TASKS.md`
- `docs/orchestration/reports/20260610/index.md`

## Verification Commands

```bash
npm run report:orchestrator:unit:dry
npm run report
npm run report:check
```

## Open Questions

- None for this resource pass. The next open question belongs to runtime foundations: exact interface/base-class split in C#.

## Do Not Touch

Do not add new memories, weapons, shop, meta progression, multi-region structure, or final boss. Use `Assets/_dev` for experimental Unity slice work until the first echo slice earns promotion to `Assets/Lethe`.
