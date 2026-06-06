# LETHE v0.12 Human Playtest Guide

Purpose: verify whether the current HTML prototype is fun enough to keep iterating toward a possible Unity implementation.

Current gate: v0.12 is a controlled 5-8 player human-test candidate after the automated balance baseline. Use `docs/playtest/2026-06-07-v012-human.md` for each session.

## Test Size

- Target testers: 5-8 players.
- Build: LETHE HTML alpha v0.12 human-test candidate.
- Session length: 12-18 minutes per tester, or one complete run if shorter.
- Required artifact: downloaded JSON log after the result survey.
- Required metadata: tester ID and session number on the start screen before each run.

## Scope

Test only the current HTML prototype loop:

- weapon plus 3 memories,
- early enemy pressure,
- kill XP and run-only level-up choices,
- first boss,
- prediction question,
- dependency-based forgetting,
- 2-memory deficit survival,
- memory refill and continued run pressure,
- result survey and JSON log.

Do not evaluate or request:

- meta progression,
- shop systems,
- final boss expansion,
- more memories,
- more regions,
- long-term save/load,
- Unity implementation quality.

## Setup

1. Optional: run `npm run playtest:package` to create a shareable static playtest folder under `dist/`.
2. Open `index.html` in Chrome or Edge.
3. Enter tester ID and session number, for example `T01` and `S01`.
4. Ask the tester to choose one weapon and one starting memory.
5. Do not explain the deletion formula.
6. Tell the tester only this: movement is manual, basic attack and memories trigger automatically, and level-up choices last only for this run.
7. Let the tester play until the boss is defeated, the forgetting result appears, the 2-memory deficit segment resolves, and at least one refill choice is made.
8. Ask the tester to complete Q1/Q2/Q3 and download the JSON log.

## Observation Focus

During the first 2-3 minutes, watch for:

- whether enemies feel active enough without overwhelming the tester,
- whether the tester says they feel stronger after level-up choices,
- whether level-up choices are considered or clicked at random,
- whether the tester seems bored before the first boss.

Before forgetting, watch for:

- whether the tester names or points to a favorite memory,
- whether they form a build plan around one memory,
- whether they can explain why a memory feels important,
- whether the first boss gives enough time to care about the chosen memories.

At the question screen, record:

- the memory they least want to lose,
- the memory they think LETHE will take,
- the reason for each answer.

After forgetting, record:

- first spoken reaction,
- whether the reaction sounds like regret, irritation, confusion, or no emotion,
- whether the tester understands why the memory disappeared,
- whether the deficit segment feels tense, unfair, too easy, or unclear,
- whether refill recovery feels like adaptation rather than full cancellation,
- whether the tester wants to continue or restart with a different build.

## Required Questions

Ask these after the in-game Q1/Q2/Q3 survey:

1. Was the first 2-3 minutes fun, boring, or unclear?
2. Did the level-up choices make you feel like you were shaping a run?
3. Did you think about the 3 choices, or did you click anything?
4. What did you feel when that memory disappeared?
5. Did it feel like a fair result, a random punishment, or something else?
6. Did the 2-memory deficit segment feel tense in a good way, too hard, too easy, or confusing?
7. Did the refill choice make you want to keep playing?
8. Would you restart to try another build?
9. Does this feel promising enough that a Unity version could be worth making?

## Scoring Notes

Use these labels in `docs/PLAYTEST_NOTES.md` after each session:

- `regret`: the tester cared about the lost memory and wanted to adapt.
- `irritation`: the tester felt punished, confused, or unfairly blocked.
- `neutral`: the tester noticed the result but did not care.
- `unclear`: the tester could not identify what happened.

The v0.12 loop is promising if:

- most testers reach the first forgetting without boredom,
- most testers treat level-up choices as meaningful,
- regret beats irritation by at least 2:1,
- 60% or more show restart intent or say they want to try another build,
- at least some testers say the idea feels worth expanding beyond HTML.

## Stop Conditions

Stop and revise before adding content if:

- half or more testers are bored before the first boss,
- most testers click level-up choices randomly,
- testers cannot remember any selected memory by name or effect,
- two or more testers call the forgetting result irritating or unfair,
- two or more testers call the deficit segment unfair or impossible,
- refill makes the loss feel fully canceled.

## After Each Session

1. Save the JSON log. The filename should include tester/session if entered.
2. Add notes to `docs/PLAYTEST_NOTES.md`.
3. Keep AI balance QA results separate from human notes.
4. After 5-8 sessions, put downloaded JSON logs in `playtest_logs/`.
5. Run `npm run playtest:summary`.
6. Send the generated human-test prompt through the planning pipeline before deciding major HTML or Unity-transition direction.
