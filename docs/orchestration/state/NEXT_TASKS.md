# Next Tasks

Keep this file short. Detailed history belongs in `docs/TASK.md`, `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.

Current source of truth:

- Product direction: `docs/PRD.md`
- Technical structure: `docs/TECH.md`
- Active work board: `docs/TASK.md`
- Test gate: `docs/TEST.md`
- Detailed design: `docs/design/README.md` and `docs/design/LETHE_DESIGN_00..07`

## 1. Direct 8-echo VFX readability review

- Priority: highest
- Include: use debug/normal play to confirm Kalmuri, Blood, Execution, Hunter, Shatter, Stopped, Ashen, and Oblivion echoes are visually identifiable without reading text.
- Check: whether each echo has its own color/shape read, whether it lasts long enough, whether proc frequency feels visible but not spammy, and whether the background hides red/gold/green/purple effects.
- Done: jaewoo can name which echoes still feel invisible, too small, too fast, or too similar.

## 2. Direct release-prep map review

- Priority: high
- Include: run `Dev_Prototype_v1` on the enlarged map, choose both starting weapons, and judge whether the arena now feels like a real game space instead of a small prototype box.
- Check: map scale, camera size, player travel distance, enemy approach readability, background contrast under VFX, whether the new Lethe terrain tiles feel like a world instead of an artificial field, and whether the first 180 seconds still feel loose.
- Done: jaewoo can say whether the map should stay this size, grow again, or become denser with terrain landmarks.

## 3. Terrain polish pass

- Priority: high
- Include: after direct play, tune tile brightness, repetition, backdrop visibility, root/water/gravel density, and landmark placement.
- Check: readable floor value range, Lethe identity, enemy silhouette contrast, and no conflict with gold/red/green combat VFX.
- Done: the background reads as LETHE terrain in motion, not only good standalone tiles.

## 4. Enemy / boss sprite readability on the larger map

- Priority: high
- Include: check `sheet_enemy_eye_4dir.png`, `sheet_enemy_splitter_4dir.png`, `sheet_enemy_voidpriest_4dir.png`, and `spr_boss_gatekeeper_01.png` in live combat under VFX.
- Done: jaewoo can recognize ranged eye, splitter, void priest, and Gatekeeper without reading debug labels, even when they enter from farther away.

## 5. VFX scale/timing follow-up after direct echo/map review

- Priority: high
- Include: tune generated sprite scale, alpha, sorting order, duration, and spawn frequency for weapon hits, phantom weapons, memories, echoes, ultimates, and background contrast after the new map is judged.
- Done: direct review no longer calls out VFX size/timing/background contrast as the weakest axis.
