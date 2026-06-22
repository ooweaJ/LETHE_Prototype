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
- Include: run `Dev_Prototype_v1`, choose both starting weapons, and judge the first 120 seconds with the newly wired generated VFX and hit-point phantom weapon sweep timing.
- Check: player body stability, 4-direction movement frames, walking/stop/start naturalness, whether phantom dual blades/greatsword visibly sweep before slash VFX, whether the attacked enemy is readable, terrain readability, HUD density, VFX scale/timing, hit readability, Blood Blade Storm payoff, and whether the build feels like a real survivor-game shell.
- Done: jaewoo can name the weakest visual/game-feel axis in one sentence.

## 2. VFX scale/timing follow-up

- Priority: high
- Include: after direct play, tune generated sprite scale, alpha, sorting order, duration, and spawn frequency for weapon hits, phantom weapons, memories, echoes, and ultimates.
- Check: dual blades stay quick and visible, greatsword stays heavy without being oversized, phantom weapons align with slash VFX, slash/spark timing does not feel late, utility memories do not hide enemies, and Blood Blade Storm feels meaningfully stronger than Kalmuri rather than only visually different.
- Done: direct review no longer calls out VFX size/timing as the weakest axis.

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
