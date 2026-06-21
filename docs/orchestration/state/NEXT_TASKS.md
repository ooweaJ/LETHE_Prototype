# Next Tasks

Keep this file short. Detailed history belongs in `docs/TASK.md`, `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.

Current source of truth:

- Product direction: `docs/PRD.md`
- Technical structure: `docs/TECH.md`
- Active work board: `docs/TASK.md`
- Test gate: `docs/TEST.md`
- Detailed design: `docs/design/README.md` and `docs/design/LETHE_DESIGN_00..07`

## 1. Wire generated VFX sprites into runtime

- Priority: highest
- Include: connect the 20 generated weapon/hit, memory, echo, and ultimate sprites to the V1 runtime VFX spawn paths/profiles.
- Check: sprite scale, alpha, sorting order, duration, pooling behavior, and whether generated VFX replaces procedural placeholders where intended.
- Done: Play Mode smoke shows the new sprites during attacks/memories/echoes without console errors.

## 2. Direct v1 visual review

- Priority: high
- Include: run `Dev_Prototype_v1`, choose both starting weapons, and judge the first 120 seconds.
- Check: player body stability, 4-direction movement frames, terrain readability, HUD density, greatsword silhouette, refreshed VFX, and whether the build feels like a real survivor-game shell.
- Done: jaewoo can name the weakest visual/game-feel axis in one sentence.

## 3. Generate enemies and boss sprites

- Priority: high
- Include: `sheet_enemy_eye_4dir.png`, `sheet_enemy_splitter_4dir.png`, `sheet_enemy_voidpriest_4dir.png`, and `spr_boss_gatekeeper_01.png`.
- Done: enemy roles have dedicated sprites instead of only procedural silhouettes.

## 4. Reliable visual evidence path

- Priority: medium
- Include: replace the current camera screenshot path that sometimes captures a solid-color image.
- Done: saved evidence can show player, terrain, weapon, enemies, and relevant UI without manual interpretation.

## 5. Continue M2 loop review

- Priority: medium
- Include: after the visual shell is acceptable, continue checking reward cadence, forgetting UX, echo anticipation, and Blood Blade Storm payoff.
- Done: jaewoo can give `GO`, `ITERATE`, or `NO-GO` for promotion out of `_dev`.
