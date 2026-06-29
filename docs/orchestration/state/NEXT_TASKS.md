# Next Tasks

Keep this file short. Detailed history belongs in `docs/TASK.md`, `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.

Current source of truth:

- Product direction: `docs/PRD.md`
- Technical structure: `docs/TECH.md`
- Active work board: `docs/TASK.md`
- Test gate: `docs/TEST.md`
- Detailed design: `docs/design/README.md` and `docs/design/LETHE_DESIGN_00..07`

## 1. 20-minute beta balance playthrough review

- Priority: highest
- 2026-06-29 재확인: 씬 누락 참조를 v1 scene rebuild로 복구했고, compile `0`, scene/assets missing references `0`, console errors `0` 상태에서 dual blades, greatsword, M2, VFX Matrix, Blood Blade Storm QA가 모두 `[V1QA] PASS`했다.
- Review sheet: `docs/orchestration/review_prompts/2026-06-29-jaewoo-beta-run-review.md`
- MCP automated QA on 2026-06-27 passed the technical gate: compile `0`, missing references `0`, console errors `0`, dual/greatsword/M2 smoke routes initialized, and 8 memory/8 echo VFX previews spawned.
- Reliable QA line is now stronger: dual blades, greatsword, M2, VFX Matrix, and Blood Blade Storm all log explicit `[V1QA] PASS` from Unity MCP.
- Include: normal play with debug panel hidden, both starting weapons, first reward timing, first Gatekeeper at 300s, first forgetting around 5m, 1 ultimate around 15~16m, and fourth Gatekeeper clear around 19~20m.
- Also check: the new HUD echo/objective lines are helpful rather than noisy.
- Check: whether the new 20-minute tempo feels satisfying, whether early XP is no longer too fast, and whether 1 ultimate + final Gatekeeper is a clear enough completion rule.
- Done: jaewoo can say GO/ITERATE for the 20-minute beta balance line.

## 2. Weapon final feel review

- Priority: high
- Include: play dual blades and greatsword in the full run, not only debug presets.
- Check: whether greatsword blocks the view, whether its slash appears fast enough, whether dual blades have enough quick-hit identity, and whether greatsword route clears are less stable than dual blades in real play.
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
