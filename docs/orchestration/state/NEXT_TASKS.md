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
- Include: run `Dev_Prototype_v1`, use the new `Mem A`, `Mem B`, `Echo A`, `Echo B`, `Ult 3`, and `VFX` debug buttons first, then choose both starting weapons and judge the first 120 seconds.
- Check: player body stability, 4-direction movement frames, walking/stop/start naturalness after the softer movement pass, whether phantom dual blades/greatsword visibly sweep before slash VFX, whether greatsword slash VFX now appears fast enough at `0.18s`, whether 처형섬광 is large enough as a crack burst, whether `멈춘 1초` reads as a clock-field floor telegraph, terrain readability after arena dressing, HUD density, hit readability, Blood Blade Storm payoff, and whether the first 120 seconds reaches choices/combat pressure quickly enough.
- Done: jaewoo can name the weakest visual/game-feel axis in one sentence.

## 2. VFX scale/timing follow-up

- Priority: high
- Include: after direct play, tune generated sprite scale, alpha, sorting order, duration, and spawn frequency for weapon hits, phantom weapons, memories, echoes, ultimates, and background contrast.
- Check: dual blades stay quick and visible after the new stagger; greatsword stays heavy without being oversized; greatsword handle/tip orientation stays readable; phantom weapons align with slash VFX; slash/spark timing does not feel late; utility memories are visible but do not hide enemies; arena dressing supports VFX readability; and Blood Blade Storm feels meaningfully stronger than Kalmuri rather than only visually different.
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
