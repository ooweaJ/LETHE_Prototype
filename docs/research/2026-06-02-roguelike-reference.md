# Roguelike Reference Notes - 2026-06-02

## Purpose

LETHE v0.8 passed the current AI balance gates, but the user expects the prototype to feel closer to a small release candidate before broad human testing. This note reframes the next stage from browser QA Gate C to a reference-driven upgrade loop.

The goal is not to clone successful roguelikes. The goal is to translate their proven fun structures into LETHE's core promise: a build that becomes precious because it can be lost.

## References Checked

- Vampire Survivors / survivor-like design:
  - Android Police interview notes that three to four level-up options felt like the right balance for Vampire Survivors, and that Magic Survival was an important auto-attacking reference.
  - Source: https://www.androidpolice.com/vampire-survivors-developer-interview/
- Hades:
  - GameSpot interview coverage emphasizes that Hades lets players keep discovering dialogue, systems, weapons, and story even when they fail.
  - It also highlights the appeal of managing randomness instead of treating difficulty as the whole point.
  - Source: https://www.gamespot.com/articles/hades-changes-what-it-means-to-be-a-roguelike/1100-6483420/
- Deep Rock Galactic: Survivor:
  - PC Gamer review highlights automatic weapons, level-up additions, overclocks, relentless pressure, tactical terrain shaping, and boss fights that feel more like action-RPG drama than passive screen clearing.
  - Source: https://www.pcgamer.com/games/roguelike/deep-rock-galactic-survivor-review/
- Balatro:
  - Game Informer interview highlights familiar iconography, simple readable descriptions, and a design target of "I want to play this myself."
  - Source: https://gameinformer.com/interview/2024/03/21/balatro-was-almost-called-joker-poker-and-other-details-from-its-creator
- Risk of Rain:
  - Risk of Rain's time-based difficulty and spawn economy create alternating highs and lows. It also accepts that random item combinations can sometimes break the game if those moments come from the run's random generator.
  - Source: https://en.wikipedia.org/wiki/Risk_of_Rain

## Patterns To Adapt

### 1. Immediate Build Direction

Successful roguelikes show the player a direction early. Vampire Survivors does it through frequent 3-4 option choices. Balatro does it through readable jokers and familiar card language. LETHE should do it through memory tags, visible pair bonuses, and combat feedback that makes a player say, "This run is becoming an area-control run" or "This run is a burst-survival run."

LETHE adaptation:

- Keep 3 active memory slots.
- Do not add more than the current 6 memories yet.
- Make each memory's combat role obvious in under 20 words.
- Make the first 90 seconds produce a visible build identity before the first forgetting.
- Add run-level goals that ask the player to preserve or sacrifice a role, not just survive.

### 2. Pressure With Highs And Lows

Risk of Rain and DRG:S keep pressure high without making the player feel constantly doomed. The fun comes from a wave that nearly crushes the player, then a build spike, then a new threat that asks for adaptation.

LETHE adaptation:

- Replace flat swarm pressure with named pressure beats.
- Track and display danger spikes, near-death recovery, boss windows, and post-loss stabilization.
- Tune the first loop around one clear high, one clear loss, and one clear recovery.
- Avoid AI gates that only reward survival; reward "almost lost but recovered" moments.

### 3. Loss Must Create A New Challenge

Hades makes failure feel like forward motion. LETHE's forgetting should work similarly inside a run: losing a memory should not only reduce DPS, it should create a fresh tactical question.

LETHE adaptation:

- After forgetting, show one "loss challenge" objective for 60-90 seconds.
- Examples: survive with no area control, kill an elite without burst, protect a fragile echo field.
- Reward completion with a tag echo that changes the remaining build's behavior.
- The player should feel, "I lost something, but now I have a new way to prove this run."

### 4. Tactical Agency In Auto Combat

DRG:S makes auto-combat tactical through terrain and mission constraints. LETHE currently risks feeling too automatic. The prototype needs at least one low-cost agency layer that does not become a full action game.

LETHE adaptation candidates:

- Memory anchor zones: standing near a memory trace amplifies one tag but raises dependency.
- Boss tells that ask the player to move toward or away from a forgotten-memory echo.
- Enemy types that punish one tag and reward another, making memory composition matter.

### 5. Readability Before Content Expansion

Balatro's lesson is not "make cards"; it is that each object should be instantly readable. LETHE should not solve weak fun by adding more memories, regions, shops, or bosses. It should make the existing 6 memories feel sharper.

LETHE adaptation:

- One short role line per memory.
- One visible tag per memory, plus one secondary tag only if needed.
- One clear synergy label when two memories combine.
- One loss consequence line after forgetting.
- No lore-heavy explanation during combat.

## New Prototype Target

The next target is not "v0.8 Gate C human-test ready." It is:

> LETHE v0.9 release-feel HTML prototype: a 9-minute survivor-like run where the player understands their build direction, feels pressure spikes, loses a memory they relied on, receives a tactical loss challenge, and sees the remaining build mutate clearly enough that a 1-person test is meaningful.

## Candidate v0.9 Work Packages

1. Build Identity Pass
   - Memory role labels.
   - Stronger tag/synergy display.
   - Combat feedback for active synergy.
   - AI metric: build identity clarity proxy.

2. Pressure And Challenge Pass
   - Named pressure beats.
   - Elite or boss pattern that asks for movement.
   - Danger recovery metric.
   - AI metric: high-low pressure score.

3. Forgetting Challenge Pass
   - A short post-loss objective.
   - Tag echo reward tied to the lost role.
   - Result payload tracks loss challenge outcome.
   - AI metric: loss challenge completion and recovery.

4. Overnight Automation Pass
   - A loop command that runs preflight, AI tests, planning double-check, reporting, and optional implementation command.
   - Each loop writes logs and leaves a next prompt.
   - Discord is used as status only; Markdown remains source of truth.

## Guardrails

- Do not add meta progression.
- Do not add shops.
- Do not add final boss.
- Do not add more than the current 6 memories.
- Do not add more than 3 active memory slots.
- Do not start multi-region structure.
- Do not use AI metrics as a human-fun verdict.
- Do not request broad human testing until the release-feel loop has at least one visually verified playable build.

