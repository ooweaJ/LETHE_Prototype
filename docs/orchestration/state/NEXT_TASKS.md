# Next Tasks

Keep this file short. Detailed history belongs in `docs/TASK.md`, `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.

Current source of truth:

- Product direction: `docs/PRD.md`
- Technical structure: `docs/TECH.md`
- Active work board: `docs/TASK.md`
- Test gate: `docs/TEST.md`
- Detailed design: `docs/design/README.md` and `docs/design/LETHE_DESIGN_00..07`

## 1. Full prototype playthrough review

- Priority: highest
- MCP automated QA on 2026-06-27 passed the technical gate: compile `0`, missing references `0`, console errors `0`, dual/greatsword/M2 smoke routes initialized, and 8 memory/8 echo VFX previews spawned.
- Reliable QA line is now stronger: dual blades, greatsword, M2, VFX Matrix, and Blood Blade Storm all log explicit `[V1QA] PASS` from Unity MCP.
- Include: normal play with debug panel hidden, both starting weapons, death case, 600-second survival if reachable, and fourth Gatekeeper clear if reachable.
- Also check: the new HUD echo/objective lines are helpful rather than noisy.
- Check: whether the run now feels like a complete prototype with a clear ending, readable boss progress, acceptable SFX, and useful result summary.
- Done: jaewoo can say GO/ITERATE for prototype completion loop.

## 2. Weapon final feel review

- Priority: high
- Include: play dual blades and greatsword in the full run, not only debug presets.
- Check: whether greatsword blocks the view, whether its slash appears fast enough, and whether dual blades have enough quick-hit identity.
- Done: weapon timing/visibility is either locked for prototype or specific values are named for tuning.

## 3. Memory/echo comment pass

- Priority: high
- Include: jaewoo deferred detailed memory/echo comments; wait for direct feedback before changing values again.
- Check: which ids are invisible, too similar, too weak, too strong, or too noisy.
- Done: exact memory/echo ids and desired direction are listed.

## 4. Map identity review

- Priority: high
- Include: judge the current terrain later, after prototype completion is reviewed.
- Check: whether map should stay runtime-generated, move to hand-authored chunks, or grow into a larger arena layout.
- Done: map direction is chosen.

## 5. Release-prep structure decision

- Priority: medium
- Include: decide whether `_dev` should keep getting tuned or whether the new `Assets/Lethe` promotion-prep structure should start receiving stable prefabs/data after full playthrough review.
- Check: remaining prototype-only runtime generation, missing audio, authored prefabs, scene organization, and asset naming.
- Done: GO/ITERATE decision for promotion prep is recorded.
