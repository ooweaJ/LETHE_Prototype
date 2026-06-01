# LETHE v0.2 Human Playtest Guide

Purpose: verify whether forgetting feels regrettable, understandable, and recoverable instead of irritating.

## Test Size

- Target testers: 5-8 players.
- Build: LETHE HTML alpha v0.2 tuning candidate.
- Session length: 12-18 minutes per tester.
- Required artifact: downloaded JSON log after the result survey.

## Scope

Test only the current forgetting loop.

Do not evaluate or request:

- meta progression,
- shop systems,
- final boss,
- more memories,
- more regions,
- long-term save/load.

## Setup

1. Open `index.html` in a browser.
2. Ask the tester to choose one weapon and exactly three memories.
3. Do not explain the deletion formula.
4. Tell the tester only this: basic attack and memories trigger automatically, and movement is manual.
5. Let the tester play until the boss is defeated and the forgetting result screen appears.

## Observation Focus

Before forgetting, watch for:

- whether the tester names or points to a favorite memory,
- whether they form a build plan around one memory,
- whether they can explain why a memory feels important,
- whether the boss timing gives enough time to care about the chosen memories.

At the question screen, record:

- the memory they least want to lose,
- the memory they think LETHE will take,
- the reason for each answer.

After forgetting, record:

- first spoken reaction,
- whether the reaction sounds like regret, irritation, confusion, or no emotion,
- whether the tester understands why the memory disappeared,
- whether the echo reward feels like a continuation or a consolation prize,
- whether the remaining build still feels playable.

## Required Questions

Ask these after the in-game Q1/Q2/Q3 survey:

1. What did you feel when that memory disappeared?
2. Did it feel like a fair result, a random punishment, or something else?
3. Did the echo reward make you want to keep playing?
4. Did you feel weaker after forgetting? If yes, was it too much, too little, or about right?
5. Before it disappeared, did you remember that memory's name or only its effect?
6. Would you restart to try protecting or changing that memory choice?

## Scoring Notes

Use these labels in `docs/PLAYTEST_NOTES.md` after each session:

- `regret`: the tester cared about the lost memory and wanted to adapt.
- `irritation`: the tester felt punished, confused, or unfairly blocked.
- `neutral`: the tester noticed the result but did not care.
- `unclear`: the tester could not identify what happened.

The v0.2 loop is promising if most testers land in `regret` or `regret + understandable loss`, and risky if two or more testers land in `irritation` or `unclear`.

## Stop Conditions

Stop and revise before adding content if:

- testers cannot remember any selected memory by name or effect,
- testers do not understand the result screen,
- JSON logs are missing selected, predicted, deleted, or deletion weight fields,
- the forgotten memory feels irrelevant to the build,
- echo makes the loss feel fully canceled.

## After Each Session

1. Save the JSON log.
2. Add notes to `docs/PLAYTEST_NOTES.md`.
3. Keep AI alpha test results separate from human notes.
4. After 5-8 sessions, summarize the pattern before deciding v0.3.
