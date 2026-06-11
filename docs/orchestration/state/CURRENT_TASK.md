# Current Task

## Goal

Prepare `Dev_Prototype_v0` for jaewoo hands-on review after implementing PRD milestones M1-M5.

## Why Now

Jaewoo's feedback is that the existing planning/design docs should be consolidated into a proper PRD before implementation continues. That is correct: the project needs one execution contract that ties game design, run structure, combat, memory/forgetting/echo, data structure, milestones, and acceptance tests together.

Current source of truth:

```text
docs/design/LETHE_UNITY_PROTOTYPE_V0_PRD.md
```

The correct next move is to play the new prototype scene and decide the first tuning target, not continue patching `Dev_EchoSlice`.

Main target:

```text
Assets/_dev/Scenes/Dev_Prototype_v0.unity
```

Reference only:

```text
Assets/_dev/Scenes/Dev_EchoSlice.unity
```

## Done Criteria

Implemented in this work unit:

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
- Memory choice can happen from kills/debug.
- Highest-level forgetting creates matching echo.
- Kalmuri/Blood echo effects affect combat.
- Echo +5 unlock path exists.
- Reacquiring echoed memories applies resonance.
- Kalmuri +5 and Blood +5 unlock Blood Blade Storm.
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

## Latest Verification

- Unity compile errors: `0`.
- Scene missing references: `0`.
- Play Mode console errors: `0`.
- LETHE art replacement pass:
  - player/enemy/map/weapon sprites replaced.
  - Kalmuri/Blood/Blood Blade Storm sprite VFX wired into `PrototypeGameManager`.
  - chroma source images preserved under `Assets/_dev/Art/Source`.
  - runtime Unity sprites use alpha PNG, not green backgrounds.
  - scene `koreanFont` reference cleared; local font files remain out of scope.
- Enemy spawn runtime check: `7` enemies, player and manager present.
- Combat smoke: enemies forced near player; after 8 seconds, `kills=7`, `playerHp=26.5`.
- M5 state smoke: active memories `Memory_HungryBlades:3`, `Memory_BloodReflection:2`; echoes `Echo_Kalmuri:5`, `Echo_Blood:5`; ultimate `true`.
- Ultimate smoke: after 5 seconds, `kills=148`, `playerHp=100`, console errors `0`.

## Open Questions

- Is the camera scale/framing now acceptable?
- Is Blood Blade Storm too strong even for hype validation?
- Are the generated 4-direction sprites readable enough, or should the next pass regenerate cleaner sheets?

## Do Not Touch

Do not continue polishing `Dev_EchoSlice` as the main path. Do not add shop, meta progression, multi-region structure, or final boss.
