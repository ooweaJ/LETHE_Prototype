# Current Task

## Goal

Implement the first complete prototype pass from the new PRD.

## Why Now

Jaewoo's latest feedback is that a dual-blades-only prototype with two memories cannot establish LETHE's real standard. The project needs a complete execution contract that includes greatsword, all planned core memories, all echoes, and the first four ultimate echoes before the next implementation push.

Current source of truth:

```text
docs/design/LETHE_UNITY_COMPLETE_PROTOTYPE_PRD.md
```

Legacy reference:

```text
docs/design/LETHE_UNITY_PROTOTYPE_V0_PRD.md
```

The correct next move is to make the full scope testable in Unity before more slice-level tuning. This pass should get the complete prototype into a compressed playable/debuggable state, then jaewoo can judge which parts deserve polish or redesign.

Main target:

```text
Assets/_dev/Scenes/Dev_Prototype_v0.unity
```

Reference only:

```text
Assets/_dev/Scenes/Dev_EchoSlice.unity
```

## Done Criteria For This Work Unit

- C1 service/data scaffolding exists:
  - `EnemyDefinition`
  - `RewardPoolDefinition`
  - `MemoryInventory`
  - `EchoInventory`
  - `ForgetService`
  - `ResonanceService`
  - `UltimateEchoService`
  - `RewardService`
  - `DebugStateInjector`
- C2 weapon pair is testable:
  - `Weapon_DualBlades`
  - `Weapon_Greatsword`
  - debug weapon switch.
- C3/C4/C5 compressed prototype is testable:
  - 8 active memories.
  - 8 echoes.
  - 4 ultimate echoes.
- 4 enemy role ids appear in the scene runtime.
- Unity compile error 0, console error 0, missing reference 0.

## Complete Prototype Done Criteria

Implemented in the broader complete prototype sequence:

- Data-driven core replaces `PrototypeGameManager` memory/echo hard-code.
- 쌍검 and 대검 are both playable.
- 8 active memories have at least level 1 behavior.
- 8 echoes have at least +1 behavior.
- Debug can force +3/+5 and four ultimate echo conditions.
- Complete prototype smoke can run in 60~120 seconds.
- Dedicated sprite VFX, balance, data asset binding, and runtime class split still need follow-up.

## Latest Visibility Patch

- Added procedural VFX for the six memory/echo families that did not have dedicated sprites:
  - `Memory_ExecutionFlash` / `Echo_Execution`
  - `Memory_HunterOath` / `Echo_Homing`
  - `Memory_ShatterWave` / `Echo_Shockwave`
  - `Memory_StoppedSecond` / `Echo_TimeStop`
  - `Memory_AshenShield` / `Echo_AshenGuard`
  - `Memory_OblivionBrand` / `Echo_Brand`
- `F7` and echo debug actions now spawn immediate preview shapes so the user can tell whether the state changed.
- Verification:
  - Unity compile errors `0`.
  - scene missing references `0`.
  - Play Mode console errors `0`.
  - F7/F5 smoke: active memories `8`, echoes `8`, procedural VFX objects `276`.

Next question for hands-on review:

- Are the new shapes readable, or are they too noisy?

## Latest Behavior Correction

- The previous VFX visibility patch overcorrected and made memories look like character-centered clutter.
- Corrected implementation:
  - active memory cap: `3`.
  - F7 showcase: Kalmuri + ShatterWave + StoppedSecond only.
  - Kalmuri is the only player-orbit memory.
  - ShatterWave and StoppedSecond now trigger around target/enemy positions.
  - removed all-memory persistent orbit/preview behavior.
- Verification:
  - Unity compile errors `0`.
  - scene missing references `0`.
  - Play Mode console errors `0`.
  - F7 smoke confirms active memory count `3` and no old Persistent/Preview VFX.

Next hands-on review question:

- Does F7 now feel like a real three-memory build instead of an all-effects debug mess?

## Related Files

- `docs/design/LETHE_UNITY_COMPLETE_PROTOTYPE_PRD.md`
- `docs/design/LETHE_UNITY_PROTOTYPE_V0_PRD.md`
- `docs/design/LETHE_WEAPON_MEMORY_ECHO_DETAIL.md`
- `docs/design/LETHE_CONTENT_TABLES.md`
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

- Complete Prototype implementation pass:
  - Unity compile errors: `0`.
  - Scene missing references: `0`.
  - Play Mode console errors after retry: `0`.
  - Runtime smoke injection:
    - manager present: `true`.
    - weapon switched to `Weapon_Greatsword`.
    - active memories: `8`.
    - echoes: `8`.
    - unlocked synergies: `4`.
    - enemy roles present: `Enemy_MeleeChaser`, `Enemy_RangedEye`, `Enemy_Splitter`, `Enemy_EliteGatekeeper`.
  - Fixed a Play Mode exception where active memory iteration could be modified by kill/forget side effects; memory/echo/synergy loops now use snapshots.
  - Editor state after stop: active scene `Assets/_dev/Scenes/Dev_Prototype_v0.unity`, `isPlaying=false`, `isCompiling=false`, `sceneDirty=false`.

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

## Latest Combat Feel Fix

- User review: prototype is not bad, but attack range is too small and weapon/enemy interaction does not yet feel physical enough.
- Changed base dual blades from nearest-single-target hit to wide cleave:
  - actual range `2.35`.
  - attack arc `108` degrees.
  - up to `5` enemies per swing.
  - primary damage `10.5`, secondary damage `72%`.
  - primary/secondary knockback with immediate hit snap.
- Added runtime arc targeting through `PrototypeEnemySpawner.FindTargetsInArc`.
- Added enemy hit reaction through `PrototypeEnemy.ApplyKnockback`.
- Synced scene and player prefab serialized weapon values.
- Evidence:
  - `LETHE/Assets/_dev/Evidence/prototype_weapon_range_interaction_game.png`
- Verification:
  - Unity compile errors: `0`.
  - Scene missing references: `0`.
  - Play Mode console errors: `0`.
  - Forced base dual-blade swing against five enemies:
    - primary target `28.0 -> 17.5`.
    - secondary targets `28.0 -> 20.4`.
    - all five targets moved outward immediately after hit.
    - weapon cleave/primary hit line VFX count observed: `12`.
  - Editor state after stop: active scene `Dev_Prototype_v0`, `sceneDirty=false`.

## Latest Memory Hunting Window Fix

- User review: memory seemed to turn into echo too quickly, making it hard to judge whether active memories are good for hunting.
- Diagnosis: after the cleave range pass, kill speed increased, but auto-forget still used the old early threshold.
- Changed automatic forgetting so every newly chosen/reacquired memory gets a protected active hunting window:
  - first auto-forget target moved to `26` kills.
  - subsequent auto-forget interval moved to `14` kills.
  - new active memory protection: at least `14` kills and `18` seconds before auto-forget can fire.
  - HUD now shows `보호 N킬/N초` beside the next forget candidate while protection is active.
- Verification:
  - Unity compile errors: `0`.
  - Scene missing references: `0`.
  - Play Mode console errors: `0`.
  - Forced state with `kills=31` during protection: `activeCount=1`, `echoCount=0`.
  - Forced state after protection expired: `activeCount=0`, `echoCount=1`.
  - Editor state after stop: active scene `Dev_Prototype_v0`, `sceneDirty=false`.

## Latest Memory Identity / Balance Fix

- User review: Kalmuri and Blood memory effects are not readable; base attack is too strong with too much knockback; the blue line near the character is distracting.
- Changed basic dual blades into a weaker base layer:
  - damage `10.5 -> 5.8`.
  - range `2.35 -> 2.15`.
  - max targets `5 -> 4`.
  - secondary damage `72% -> 48%`.
  - primary knockback `3.8 -> 1.45`.
  - secondary knockback `2.6 -> 0.85`.
  - enemy knockback snap reduced and velocity clamp lowered.
- Changed active Kalmuri into an independent orbit blade memory:
  - periodic orbit blade sprites around player.
  - periodic nearby enemy cuts independent of weapon swing.
  - removed old persistent blue `ActiveHungryBladesOrbit` line.
- Changed active Blood into a red mark/heal identity:
  - periodic red mark pulse on nearby enemies.
  - visible heal bloom near player.
  - reduced on-hit thread strength so it supports, not dominates.
- Verification:
  - Unity compile errors: `0`.
  - Scene missing references: `0`.
  - Play Mode console errors: `0`.
  - Basic swing smoke: primary `28.0 -> 22.2`, secondary `28.0 -> 25.2`, fifth target not hit.
  - Kalmuri smoke: level 1 tick damaged `2` nearby enemies and spawned `4` Kalmuri sprites; old blue orbit lines `0`.
  - Blood smoke: level 1 tick damaged `2` nearby enemies, healed player `72.0 -> 72.7`, spawned `3` blood sprites.
  - Evidence: `LETHE/Assets/_dev/Evidence/prototype_memory_identity_pass_game.png`.
