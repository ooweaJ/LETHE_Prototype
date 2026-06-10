# Current Task

## Goal

Concretize weapon, memory, echo, awakened echo, resonance, and ultimate echo design so echoes feel powerful rather than like small text/proc labels.

## Why Now

The user clarified that the real issue is not only the forgetting loop. The current HTML echo feedback feels like "잔향!" attached to basic attacks, which does not create enough impact. LETHE needs a more concrete design for what each weapon does, what each memory becomes when forgotten, and how echoes change the player's combat fantasy.

The design must answer:

- What does each weapon feel like before echoes?
- What new action does each echo add to weapon attacks?
- How does the same echo behave differently on dual blades versus greatsword?
- What changes at echo `+5` awakened state?
- What does reacquisition resonance add beyond level restoration?
- What is the first "this build is crazy" ultimate echo moment?

## Done Criteria

- `docs/design/LETHE_WEAPON_MEMORY_ECHO_SPEC.md` exists.
- The spec defines dual blades and greatsword identity.
- The spec defines concrete active memory, normal echo, awakened echo, resonance, and hype moment for the current 8 memories.
- The spec defines at least `피의 칼폭풍` in enough detail to guide implementation.
- Design README, combat design, content tables, overview, and feel spec link to the new spec.
- Report/devlog/state docs record that this planning pass is about replacing abstract echo text with concrete combat events.
- No gameplay code is added in this planning unit.

## Related Files

- `docs/design/LETHE_WEAPON_MEMORY_ECHO_SPEC.md`
- `docs/design/LETHE_FORGETTING_FEEL_SPEC.md`
- `docs/design/LETHE_COMBAT_DESIGN.md`
- `docs/design/LETHE_CONTENT_TABLES.md`
- `docs/orchestration/state/NEXT_TASKS.md`
- `docs/orchestration/reports/20260610/index.md`

## Verification Commands

```bash
npm run report
npm run report:check
```

## Open Questions

- Is the first implementation target definitely `절단쌍검 + 굶주린 칼무리 + 피의 반사 -> 피의 칼폭풍`?
- Should `장송대검 + 처형자의 섬광 + 망각의 낙인 -> 처형 각인` be the second showcase?
- Should HTML prototype implement only the first showcase, leaving the rest as Unity design?

## Do Not Touch

Do not add new memories, weapons, enemies, shop, meta progression, multi-region structure, final boss, or Unity setup while this design is being clarified.
