# Next Tasks

Keep this file short. Detailed history belongs in `docs/TASK.md`, `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.

Current source of truth:

- Product direction: `docs/PRD.md`
- Technical structure: `docs/TECH.md`
- Active work board: `docs/TASK.md`
- Test gate: `docs/TEST.md`
- Detailed design: `docs/design/README.md` and `docs/design/LETHE_DESIGN_00..07`

## 1. Direct v1 visual review

- Priority: highest
- Include: run `Dev_Prototype_v1`, choose both starting weapons, and judge the first 120 seconds.
- Check: player body stability, 4-direction movement frames, terrain readability, HUD density, greatsword silhouette, refreshed Kalmuri/Blood/Blood Blade Storm VFX, and whether the build feels like a real survivor-game shell.
- Done: jaewoo can name the weakest visual/game-feel axis in one sentence.

## 2. Generate weapon arcs and hit sparks

- Priority: high
- Include:
  - `spr_dual_blade_swing_arc_01.png`
  - `spr_dual_blade_swing_arc_02.png`
  - `spr_greatsword_cleave_arc_01.png`
  - `spr_hit_spark_cyan_01.png`
  - `spr_hit_spark_red_01.png`
- Done: assets exist, import as Sprites, and v1 smoke has compile/console error count 0.

## 3. Generate missing six memory/echo VFX

- Priority: high
- Include: Execution, Homing, Shockwave, TimeStop, Ashen, and Brand active-memory sprites plus matching echo sprites from `docs/design/LETHE_SPRITE_PRODUCTION_PROMPTS.md`.
- Done: each missing memory/echo family has a dedicated sprite instead of only procedural runtime shapes.

## 4. Reliable visual evidence path

- Priority: medium
- Include: replace the current camera screenshot path that sometimes captures a solid-color image.
- Done: saved evidence can show player, terrain, weapon, enemies, and relevant UI without manual interpretation.

## 5. Continue M2 loop review

- Priority: medium
- Include: after the visual shell is acceptable, continue checking reward cadence, forgetting UX, echo anticipation, and Blood Blade Storm payoff.
- Done: jaewoo can give `GO`, `ITERATE`, or `NO-GO` for promotion out of `_dev`.
