# LETHE v0.5 Human Playtest Guide

Purpose: verify whether v0.5 is fun enough in HTML prototype form to justify continued iteration toward a possible Unity implementation.

## Test Size

- Target testers: 5-8 players.
- Build: LETHE HTML alpha v0.5 core-fun human-test ready.
- Session length: 12-18 minutes per tester.
- Required artifact: downloaded JSON log after the result survey.
- Required metadata: tester ID and session number on the start screen before each run.

## Scope

Test only the current HTML prototype loop:

- weapon + 3 memories,
- early enemy pressure,
- kill XP and run-only level-up choices,
- first boss,
- prediction question,
- dependency-based forgetting,
- echo/replacement recovery,
- result survey and JSON log.

Do not evaluate or request:

- meta progression,
- shop systems,
- final boss,
- more memories,
- more regions,
- long-term save/load,
- Unity implementation quality.

## Setup

1. Optional: run `npm run playtest:package` to create a shareable static playtest folder under `dist/`.
2. Open `index.html` in a browser.
3. Enter tester ID and session number, for example `T01` and `S01`.
4. Ask the tester to choose one weapon and exactly three memories.
5. Do not explain the deletion formula.
6. Tell the tester only this: movement is manual, basic attack and memories trigger automatically, and level-up choices last only for this run.
7. Let the tester play until the boss is defeated and the forgetting result screen appears.
8. Ask the tester to complete Q1/Q2/Q3 and download the JSON log.

## Observation Focus

During the first 2-3 minutes, watch for:

- whether enemies feel like they are arriving fast enough,
- whether the tester says they feel stronger after level-up choices,
- whether level-up choices are considered or clicked at random,
- whether the tester seems bored before the first boss.

Before forgetting, watch for:

- whether the tester names or points to a favorite memory,
- whether they form a build plan around one memory,
- whether they can explain why a memory feels important,
- whether the 9 minute first-forgetting window gives enough time to care about the chosen memories.

At the question screen, record:

- the memory they least want to lose,
- the memory they think LETHE will take,
- the reason for each answer.

After forgetting, record:

- first spoken reaction,
- whether the reaction sounds like regret, irritation, confusion, or no emotion,
- whether the tester understands why the memory disappeared,
- whether the 28% power dip feels too weak, too harsh, or about right,
- whether the echo/replacement recovery feels like adaptation rather than full cancellation,
- whether the tester wants to restart or try a different build.

## Required Questions

Ask these after the in-game Q1/Q2/Q3 survey:

1. Was the first 2-3 minutes fun, boring, or unclear?
2. Did the level-up choices make you feel like you were shaping a run?
3. Did you think about the 3 choices, or did you click anything?
4. What did you feel when that memory disappeared?
5. Did it feel like a fair result, a random punishment, or something else?
6. Did you feel weaker after forgetting? If yes, was it too much, too little, or about right?
7. Did the echo/replacement make you want to keep playing?
8. Would you restart to try another build?
9. Does this feel promising enough that a Unity version could be worth making?

## Scoring Notes

Use these labels in `docs/PLAYTEST_NOTES.md` after each session:

- `regret`: the tester cared about the lost memory and wanted to adapt.
- `irritation`: the tester felt punished, confused, or unfairly blocked.
- `neutral`: the tester noticed the result but did not care.
- `unclear`: the tester could not identify what happened.

The v0.5 loop is promising if:

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
- testers do not feel the post-forgetting power dip at all,
- echo/replacement makes the loss feel fully canceled.

## After Each Session

1. Save the JSON log. The filename should include tester/session if entered.
2. Add notes to `docs/PLAYTEST_NOTES.md`.
3. Keep AI alpha test results separate from human notes.
4. After 5-8 sessions, summarize the pattern.
5. Put downloaded JSON logs in `playtest_logs/`.
6. Run `npm run playtest:summary`.
7. Send the generated human-test prompt through the planning pipeline before deciding HTML v0.6 or Unity transition groundwork.
