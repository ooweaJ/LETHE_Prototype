# Next Tasks

Keep this file short. Detailed history belongs in `docs/TASK.md`, `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.

Current source of truth:

- Product direction: `docs/PRD.md`
- Technical structure: `docs/TECH.md`
- Active work board: `docs/TASK.md`
- Test gate: `docs/TEST.md`
- Detailed design: `docs/design/README.md` and `docs/design/LETHE_DESIGN_00..07`

## 1. Direct integrated feel review

- Priority: highest
- Include: normal play and `DB Rev` / `GS Rev` with the new greatsword duplicate-VFX correction, weapon feel split, 135s first Gatekeeper, larger 22s warning, darker terrain, stronger landmarks, and larger enemy role markers.
- Check: whether greatsword now reads as one strong slash instead of two stacked cleaves, whether dual blades feel faster/lighter, whether the first boss is exciting rather than too early, and whether terrain/markers improve readability.
- Done: jaewoo can name which of weapon VFX, weapon balance, first boss timing, enemy markers, or map contrast is now the weakest remaining axis.

## 2. Single memory/echo identity review

- Priority: high
- Include: use `Mem One` / `Echo One` after the broader combat pass is judged.
- Check: whether each memory and matching echo are visually distinct, useful, and not too similar.
- Done: jaewoo can name which memory/echo ids need tuning.

## 3. Direct release-prep map review

- Priority: high
- Include: run `Dev_Prototype_v1` on the enlarged and darkened map, choose both starting weapons, and judge whether the arena now feels like a real game space instead of a small prototype box.
- Check: map scale, camera size, player travel distance, enemy approach readability, background contrast under VFX, and whether the new Lethe terrain tiles feel like a world instead of an artificial field.
- Done: jaewoo can say whether the map should stay this size, grow again, or become denser with terrain landmarks.

## 4. Terrain polish pass

- Priority: high
- Include: after direct play, tune tile brightness, repetition, backdrop visibility, root/water/gravel density, and the new memory landmark placement.
- Check: readable floor value range, Lethe identity, enemy silhouette contrast, and no conflict with gold/red/green combat VFX.
- Done: the background reads as LETHE terrain in motion, not only good standalone tiles.

## 5. Enemy / boss sprite readability on the larger map

- Priority: high
- Include: check `sheet_enemy_eye_4dir.png`, `sheet_enemy_splitter_4dir.png`, `sheet_enemy_voidpriest_4dir.png`, and `spr_boss_gatekeeper_01.png` in live combat under VFX.
- Done: jaewoo can recognize ranged eye, splitter, void priest, and Gatekeeper without reading debug labels, even when they enter from farther away.
