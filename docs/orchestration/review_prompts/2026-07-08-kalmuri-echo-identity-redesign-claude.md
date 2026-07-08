# Claude Review Prompt: Kalmuri Memory / Echo Identity Redesign

Date: 2026-07-08
Project: LETHE Unity `Dev_Prototype_v1`
Authoring intent: ask Claude for design recommendation only. Claude should not edit files.

## Role

You are reviewing one focused design problem for LETHE: how `굶주린 칼무리` should work as an active memory, and how it should transform after forgetting into `칼무리 잔향`.

Do not answer as a general VFX brainstorm. Treat this as a combat-design and readability problem: concept, VFX language, hit rules, weapon identity, and player fun must agree.

## Project Context

LETHE is a 2D action roguelite about memories that grow, are forgotten, and return as echoes. The important emotional loop is:

- active memory = the thing the player currently owns and grows,
- forgetting = a real loss,
- echo = not a weaker copy, but a transformed remnant that changes combat,
- resonance / +5 echo = late reward that makes the loss feel meaningful.

Current Unity scope is `_dev` only:

- Scene: `LETHE/Assets/_dev/Scenes/Dev_Prototype_v1.unity`
- Main runtime file: `LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs`
- Weapons:
  - `절단쌍검`: fast, frequent, small multi-hit rhythm.
  - `장송대검`: slow, heavy, wider single-hit rhythm.
- Same echo definition should survive across weapons, but the weapon must change rhythm and visual grammar.

Important design rule:

- Active memory can be independent of weapon.
- On-hit echo must originate from the weapon hit / enemy wound / target-side interaction.
- Echo should not look like another unrelated projectile leaving the player after the attack.

## Current Problem

Jaewoo is not satisfied with the current `칼무리 잔향` candidates.

The complaint is not simply "make four VFX look different." The issue is that the player should feel:

> "아, 굶주린 칼무리라는 기억이 망각된 뒤 이렇게 잔향으로 남았구나. 재밌다."

Current attempts have repeatedly missed that feeling. Blood Reflection is currently the best benchmark because it has a clear memory-to-echo identity:

- blood mark,
- wound/bloom,
- target-side reaction,
- heal thread,
- +5 payoff.

Kalmuri has not reached that level of identity yet.

## Current / Recent Kalmuri Implementation History

These ideas were tried or partially implemented:

1. Old awakened projectile
   - A Kalmuri blade launched after the weapon attack.
   - Rejected because it read like "one more blade flies out from the player," not like an echo at the wound.

2. Wound-side correction
   - The awakened chain now starts from the struck enemy / wound position.
   - Better direction, but still not enough as a full identity.

3. Four playable prototype concepts
   - `K1`: wound feast / bite swarm.
   - `K2`: blood-scent hunt.
   - `K3`: feast table.
   - `K4`: chewed trail.
   - These tried to use wound, scent, teeth, inward bite, and chewed scars.
   - Jaewoo still does not like the four results.

4. Dual Blades red-circle issue
   - Dual Blades Kalmuri recently read as a red circle.
   - That is considered wrong.
   - Dual Blades should read as fast bite marks, wound slashes, tooth snaps, small shards, repeated nicks.
   - Greatsword may keep broader wound/table/furrow silhouettes if they fit heavy impact.

5. Legacy VFX suppression
   - The old +5 flying blade is now suppressed while K1-K4 prototype mode is active.
   - The next recommendation should not rely on the old flying-blade language.

## Design Baseline To Preserve

`굶주린 칼무리` as active memory currently implies:

- a hungry group of blades,
- orbit / presence around the player,
- independent hunting or chewing pressure,
- repeated cuts against nearby enemies,
- "many blades are alive around me" before forgetting.

The echo should not simply repeat that orbit. It should feel like the memory has become a residual combat habit attached to weapon hits.

Possible interpretation:

- active memory = the whole hungry pack is present around the player,
- echo = only the pack's appetite remains at the wound after each weapon hit.

Please challenge or improve this interpretation if you think it is wrong.

## What We Need From You

Please answer in Korean.

The core question:

> `굶주린 칼무리` 하면 어떤 이미지가 떠오르는가? 그 이미지가 활성 기억일 때와 잔향일 때 어떻게 달라져야 하는가?

Give a recommendation that helps Codex build the next playable candidates.

## Required Output

### 1. Core Diagnosis

Explain why the current approaches are probably failing. Be specific:

- concept mismatch,
- VFX silhouette,
- hit origin,
- weapon rhythm,
- color/readability,
- active memory vs echo confusion.

### 2. Identity Definition

Define `굶주린 칼무리` in one strong sentence.

Then split it:

| State | Player should read | Gameplay role | VFX grammar |
| --- | --- | --- | --- |
| Active Memory | ... | ... | ... |
| Echo | ... | ... | ... |
| +5 / Awakened Echo | ... | ... | ... |

### 3. Recommended Direction

Pick one primary direction you think is strongest. Do not hedge too much.

For that direction, specify:

- fantasy phrase,
- why it fits `굶주린 칼무리`,
- why it works better than the current K1-K4,
- how it differs from Blood Reflection,
- how it avoids looking like a generic red circle or generic slash.

### 4. Four Playable Candidate Designs

Give four candidate designs Codex can implement and jaewoo can test. They should all be Kalmuri-appropriate, not random styles.

For each candidate, provide:

| Candidate | Concept | Dual Blades behavior | Greatsword behavior | VFX shape | Hit rule | Fun point | Risk |
| --- | --- | --- | --- | --- | --- | --- | --- |

Constraints:

- Dual Blades: small, fast, multi-bite, no big red circle.
- Greatsword: heavy, fewer, bigger, but do not cover the whole screen.
- Echo should happen at hit / wound / path, not from the player body.
- Avoid old flying projectile language.
- Avoid plain rings unless the ring is clearly teeth, wound, table, or trap.
- Avoid making Kalmuri look like curse/brand, time-stop, shatter, or blood echo.

### 5. Implementation Notes For Codex

Give concrete instructions in terms Codex can translate into Unity:

- transient VFX objects to spawn,
- timing,
- hitbox shape,
- camera shake / hitstop feel,
- dense Dual Blades suppression rule,
- what to remove from the current implementation.

### 6. Final Recommendation

End with:

- best candidate,
- fallback candidate,
- one sentence Jaewoo should use while testing to decide if it works.

## Known Good Benchmark

Use Blood Reflection only as a quality benchmark, not as a style to copy:

- Blood Reflection feels better because it marks targets, blooms on them, heals back to the player, and builds into Blood Blade Storm.
- Kalmuri should find an equally clear loop, but its theme should be hunger / pack / wound / chewing / blades, not blood sustain.

## Do Not Recommend

Do not recommend:

- "make it bigger/brighter" as the main answer,
- only color changes,
- generic circular AoE,
- another blade projectile launched from the player,
- a purely decorative preview with no hit rule,
- a design that makes Dual Blades and Greatsword look like the same effect at different scale.

