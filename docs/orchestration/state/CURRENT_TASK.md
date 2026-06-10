# Current Task

## Goal

Concretize the forgetting, echo, and resonance feel design before doing more implementation.

## Why Now

The new HTML forgetting model was implemented and regression-checked, but the user felt that the play experience did not change much. That means the next blocker is not another small code patch; it is a design clarity problem.

The current rules are understandable on paper:

- highest-level active memory is forgotten;
- forgotten level becomes echo level;
- echo caps at `+5`;
- overflow becomes overcharge;
- reacquiring an echoed memory creates resonance.

But the player-facing moments are not concrete enough yet. The project now needs to define exactly what the player should see, feel, and decide during:

- the loss moment;
- the echo combat shift;
- the resonance reacquisition;
- the long-term ultimate echo goal.

## Done Criteria

- `docs/design/LETHE_FORGETTING_FEEL_SPEC.md` exists and defines the desired player-facing moments.
- Design docs point to the feel spec before Unity or additional implementation work.
- `STATUS.md` and `NEXT_TASKS.md` describe planning concretization as the next step.
- Daily devlog/report record that the latest user feedback is "the change does not feel big enough."
- No new gameplay code is added in this planning unit.

## Related Files

- `docs/design/LETHE_FORGETTING_FEEL_SPEC.md`
- `docs/design/README.md`
- `docs/design/LETHE_GAME_DESIGN_OVERVIEW.md`
- `docs/design/LETHE_CORE_SYSTEMS_UNITY_PLAN.md`
- `docs/design/LETHE_UNITY_VERTICAL_SLICE_SPEC.md`
- `docs/orchestration/state/STATUS.md`
- `docs/orchestration/state/NEXT_TASKS.md`
- `docs/orchestration/reports/20260610/index.md`

## Verification Commands

```bash
npm run report
npm run report:check
```

## Open Questions

- Should the next implementation happen in HTML as a feel-presentation pass, or should the feel spec become the Unity first-slice contract?
- Should `굶주린 칼무리 + 피의 반사 -> 피의 칼폭풍` be the only first showcase?
- Should the next HTML patch force the resonance candidate to appear, or should that be reserved for a debug demonstration loop?

## Do Not Touch

Do not add new memories, weapons, enemies, shop, meta progression, multi-region structure, final boss, or Unity setup while this design is being clarified.
