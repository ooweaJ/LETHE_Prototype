# Next Tasks

Keep this file short. Detailed history belongs in `docs/TASK.md`, `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.

Current source of truth:

- Product direction: `docs/PRD.md`
- Technical structure: `docs/TECH.md`
- Active work board: `docs/TASK.md`
- Test gate: `docs/TEST.md`
- Detailed design: `docs/design/README.md` and `docs/design/LETHE_DESIGN_00..07`

## 1. Direct release-prep map review

- Priority: highest
- Include: run `Dev_Prototype_v1` on the enlarged map, choose both starting weapons, and judge whether the arena now feels like a real game space instead of a small prototype box.
- Check: map scale, camera size, player travel distance, enemy approach readability, background contrast under VFX, whether the new backdrop/tile pattern supports combat without becoming visual noise, and whether the first 180 seconds still feel loose.
- Done: jaewoo can say whether the map should stay this size, grow again, or become denser with scenery/landmarks.

## 2. Release-facing background art pass

- Priority: high
- Include: replace the generated first-pass map art with stronger production-direction sprites after scale is approved.
- Check: readable floor value range, Lethe identity, clear boundary language, environmental landmarks, enemy silhouette contrast, and no conflict with gold/red/green combat VFX.
- Done: the background reads as LETHE's world identity, not only a dark test floor.

## 3. Enemy / boss sprite readability on the larger map

- Priority: high
- Include: check `sheet_enemy_eye_4dir.png`, `sheet_enemy_splitter_4dir.png`, `sheet_enemy_voidpriest_4dir.png`, and `spr_boss_gatekeeper_01.png` in live combat under VFX.
- Done: jaewoo can recognize ranged eye, splitter, void priest, and Gatekeeper without reading debug labels, even when they enter from farther away.

## 4. VFX scale/timing follow-up after map review

- Priority: high
- Include: tune generated sprite scale, alpha, sorting order, duration, and spawn frequency for weapon hits, phantom weapons, memories, echoes, ultimates, and background contrast after the new map is judged.
- Done: direct review no longer calls out VFX size/timing/background contrast as the weakest axis.

## 5. Continue release-prep shell work

- Priority: medium
- Include: after the world baseline is acceptable, continue checking HUD polish, reward cadence, forgetting UX, echo anticipation, Blood Blade Storm payoff, and evidence capture.
- Done: jaewoo can give `GO`, `ITERATE`, or `NO-GO` for promotion out of `_dev`.
