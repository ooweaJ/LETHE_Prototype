# Current Task

## Goal

Use the new Unity Prototype v0 PRD as the execution contract, then start implementation from M1.

## Why Now

Jaewoo's feedback is that the existing planning/design docs should be consolidated into a proper PRD before implementation continues. That is correct: the project needs one execution contract that ties game design, run structure, combat, memory/forgetting/echo, data structure, milestones, and acceptance tests together.

Current source of truth:

```text
docs/design/LETHE_UNITY_PROTOTYPE_V0_PRD.md
```

The correct next move is to implement from PRD milestone M1, not continue patching `Dev_EchoSlice`.

Main target:

```text
Assets/_dev/Scenes/Dev_Prototype_v0.unity
```

Reference only:

```text
Assets/_dev/Scenes/Dev_EchoSlice.unity
```

## Done Criteria

Phase A/B first implementation should produce:

- `Dev_Prototype_v0.unity` exists.
- Scene root structure exists:
  - `PrototypeRoot`
  - `Services`
  - `Player`
  - `EnemySpawner`
  - `Arena`
  - `RuntimeVFX`
  - `HUD`
- Player moves with WASD/arrow keys.
- Camera follows player.
- Arena bounds are readable.
- At least 5 enemies spawn/chase.
- Player can attack nearest enemies.
- Enemies have HP/death/respawn.
- Player has HP/contact damage.
- Minimal HUD shows HP, kills, active memory/echo placeholders.
- Unity compile error 0, console error 0, missing reference 0.

## Related Files

- `docs/design/LETHE_UNITY_PROTOTYPE_V0_PRD.md`
- `docs/design/LETHE_UNITY_PROTOTYPE_V0_PLAN.md`
- `LETHE/Assets/_dev/Scenes/Dev_Prototype_v0.unity`
- `LETHE/Assets/_dev/Scripts/**`
- `LETHE/Assets/_dev/Art/Sprites/**`
- `LETHE/Assets/_dev/Prefabs/**`

## Verification Commands

```bash
npm.cmd run report
npm.cmd run report:check
npm.cmd run report:orchestrator:unit:dry
```

Unity MCP verification:

- `unity_get_compilation_errors(port=7890, severity="all")`
- `unity_search_missing_references(port=7890, scope="scene")`
- `unity_console_log(port=7890, type="error")`
- Play Mode runtime check for player movement, enemy spawn/chase, attack, player HP, enemy death/respawn.

## Open Questions

- None blocking. Use existing placeholder sprites first; art polish comes after the prototype loop exists.

## Do Not Touch

Do not continue polishing `Dev_EchoSlice` as the main path. Do not add shop, meta progression, multi-region structure, or final boss.
