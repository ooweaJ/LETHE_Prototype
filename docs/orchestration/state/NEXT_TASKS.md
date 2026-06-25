# Next Tasks

Keep this file short. Detailed history belongs in `docs/TASK.md`, `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.

Current source of truth:

- Product direction: `docs/PRD.md`
- Technical structure: `docs/TECH.md`
- Active work board: `docs/TASK.md`
- Test gate: `docs/TEST.md`
- Detailed design: `docs/design/README.md` and `docs/design/LETHE_DESIGN_00..07`

## 1. Direct release-feel review

- Priority: highest
- Include: normal play and `DB Rev` / `GS Rev` after the second integration pass.
- Check: greatsword slash timing, dual-blade speed, larger memory/echo VFX, level-up burst, ultimate-ready burst, enemy HP bars, and whether the first Gatekeeper at `135s` feels exciting instead of rushed.
- Done: jaewoo can name the weakest axis among weapon feel, VFX clutter, early pacing, enemy readability, growth feedback, and map identity.

## 2. Map identity review

- Priority: high
- Include: judge the new connected Lethe river/bank bands and sunken ruin slabs while moving through the enlarged arena.
- Check: whether the map now feels like one terrain space instead of separated decorations, and whether enemy/VFX contrast remains readable.
- Done: jaewoo can decide whether the next map pass should add hand-authored chunks, more terrain density, or a new larger arena layout.

## 3. Enemy / boss readability review

- Priority: high
- Include: live combat with Drifting Eye, Split One, Void Priest, and Gatekeeper.
- Check: sprite size, role marker, new HP bar, boss silhouette, and whether health bars help without making the screen too UI-heavy.
- Done: jaewoo can identify which enemy/boss sprite needs repainting or scale/color changes.

## 4. Single memory/echo identity review

- Priority: high
- Include: use `Mem One` / `Echo One` for all 8 ids after broad feel review.
- Check: whether memory and matching echo are visually distinct, useful, and not too similar after the value/VFX buffs.
- Done: jaewoo can name exact ids that need tuning.

## 5. Release-prep structure decision

- Priority: medium
- Include: decide whether `_dev` should keep getting tuned or whether a limited `Assets/Lethe` promotion prep branch should begin.
- Check: remaining prototype-only runtime generation, missing audio, authored prefabs, scene organization, and asset naming.
- Done: GO/ITERATE decision for promotion prep is recorded.
