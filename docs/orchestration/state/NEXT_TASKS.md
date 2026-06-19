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
- Check: player body stability, 4-direction movement frames, terrain readability, HUD density, greatsword silhouette, and whether the build feels like a real survivor-game shell.
- Done: jaewoo can name the weakest visual/game-feel axis in one sentence.

## 2. One focused follow-up pass

- Priority: high
- Include exactly one axis from review: character animation, start/HUD UI, combat VFX density, terrain brightness, reward card UI, or weapon feel.
- Done: the chosen issue is improved and verified with build + Unity smoke.

## 3. Reliable visual evidence path

- Priority: high
- Include: replace the current camera screenshot path that sometimes captures a solid-color image.
- Done: saved evidence can show player, terrain, weapon, enemies, and relevant UI without manual interpretation.

## 4. Sprite import cleanup

- Priority: medium
- Include: decide whether to keep runtime sheet cropping or move player/enemy sheets to proper Unity slicing/import settings.
- Done: player/enemy sprites are easy to inspect and tune without changing `V1GameManager`.

## 5. Continue M2 loop review

- Priority: medium
- Include: after the visual shell is acceptable, continue checking reward cadence, forgetting UX, echo anticipation, and Blood Blade Storm payoff.
- Done: jaewoo can give `GO`, `ITERATE`, or `NO-GO` for promotion out of `_dev`.
