# LETHE Core Systems Unity Plan

Last updated: 2026-06-09

## Purpose

This document defines the core system direction for moving LETHE from the HTML prototype into a Unity vertical slice.

The HTML prototype proved the rough shape of the memory-loss loop, but the next version needs a clearer fantasy: losing a memory should not feel like simple deletion. A lost memory should leave an echo that changes the player's build, creates new synergies, and can become a major power spike when it reaches max level.

## Core Pillar

LETHE is about turning loss into a new build.

- Active memories are the player's current abilities.
- Echoes are the remains of lost memories.
- Forgetting removes a familiar tool, but converts its level into a persistent weapon/body imprint.
- Reacquiring a memory that already has an echo should feel like resonance, not a reset.
- The best moments should come from intentionally letting strong memories become strong echoes.

## Level Rules

Both memories and echoes use the same visible level scale.

- Memory level max: `+5`.
- Echo level max: `+5`.
- A memory can be active while its echo also exists.
- Echoes are not temporary buffs. They are run-level build state.

### Memory To Echo Conversion

When an active memory is forgotten:

1. Remove the active memory from the active slot.
2. Add that memory's current level to the matching echo level.
3. Clamp the echo level at `+5`.
4. If the echo reaches `+5`, unlock its awakened echo behavior.

Examples:

- Forget `Hungry Blades +3` with no existing echo: `Hungry Blades Echo +3`.
- Forget `Hungry Blades +2` while `Hungry Blades Echo +3` exists: `Hungry Blades Echo +5`.
- Forget `Hungry Blades +5` while `Hungry Blades Echo +2` exists: `Hungry Blades Echo +5`, with overflow ignored for now.

### Reacquiring An Echoed Memory

If the player gains a memory whose echo already exists, the memory is strengthened by resonance.

Recommended first rule:

- Reacquired memory starts at `base offered level + floor(echo level / 2)`.
- Clamp active memory level at `+5`.
- The echo is not consumed.
- While the memory and its echo coexist, the memory gains a resonance rider tied to the echo's identity.

Examples:

- `Hungry Blades Echo +3` exists, player picks `Hungry Blades +1`: active memory starts at `+2`.
- `Hungry Blades Echo +5` exists, player picks `Hungry Blades +1`: active memory starts at `+3` and gets its resonance rider.
- If that `Hungry Blades +3` is forgotten again, the echo remains `+5`.

Why not restore the exact lost level immediately:

- It can make memory loss feel reversible and less meaningful.
- It makes reacquisition too obviously optimal.
- Resonance still rewards the player, but keeps build decisions alive.

## Echo Power Bands

Echo level should be readable by effect quality, not only numbers.

| Echo Level | Player Meaning | Design Target |
| --- | --- | --- |
| `+1` | Faint trace | Small stat or low-chance weapon rider |
| `+2` | Noticeable trace | Reliable minor combat identity |
| `+3` | Build-relevant echo | Player starts planning around it |
| `+4` | Strong echo | Clearly changes targeting, AoE, or survival behavior |
| `+5` | Awakened echo | Major power spike with visible effect and combo potential |

## Awakened Echoes

At `+5`, every echo should become a named awakened echo. This is where the player gets the "power fantasy" reward for losing something important.

| Memory | Awakened Echo Direction | Combat Fantasy |
| --- | --- | --- |
| Hungry Blades | Blade Swarm Echo | Blades orbit or sweep around the player and apply on-hit bleed ticks. |
| Execution Flash | Execution Echo | Critical or low-health hits trigger white flash bursts. |
| Stalker Oath | Pursuit Echo | Weapon hits spawn seeking afterimages or homing shards. |
| Shatter Ripple | Ripple Echo | Hits periodically release knockback shockwaves. |
| Blood Reflection | Blood Return Echo | Fast hits create reflected blood strikes and on-hit sustain pressure. |
| Stopped Second | Still Second Echo | Hits briefly slow or pin nearby enemies. |
| Ashen Guard | Ash Guard Echo | Defensive triggers release counter pulses or short guard windows. |
| Oblivion Brand | Brand Echo | Hits mark enemies and amplify follow-up burst damage. |

## Resonance Riders

When a memory and its matching echo coexist, the active memory gets a special rider. This makes "losing and later finding the same memory again" exciting.

Examples:

- Hungry Blades memory + Hungry Blades Echo: active blade hits count as on-hit events for echo blades.
- Execution Flash memory + Execution Echo: flash strikes prioritize marked or low-health targets.
- Stalker Oath memory + Pursuit Echo: active projectiles leave shorter-lived echo shards.
- Shatter Ripple memory + Ripple Echo: active ripples trigger smaller secondary waves.

The first Unity slice only needs two or three resonance riders implemented fully. The rest can be data-defined placeholders until the combat feel is proven.

## Ultimate Echo Synergies

When two awakened echoes reach `+5`, they can unlock an ultimate echo synergy.

Rules:

- Requires two specific echoes at `+5`.
- The synergy should be visibly stronger than either echo alone.
- It should not require an active memory slot.
- It should create a build identity that can carry the rest of the run.

Example synergy candidates:

| Required Echoes | Ultimate Echo | Effect Direction |
| --- | --- | --- |
| Hungry Blades + Blood Reflection | Blood Blade Storm | Orbiting cuts apply bleed and trigger reflected blood hits. |
| Execution Flash + Oblivion Brand | Execution Brand | Marked enemies explode in white flash damage when executed. |
| Stalker Oath + Stopped Second | Frozen Pursuit | Homing shards slow targets and chain toward slowed enemies. |
| Shatter Ripple + Ashen Guard | Bastion Ripple | Defensive pulses create large knockback shockwaves. |

The Unity vertical slice should implement one ultimate echo fully and keep the rest as design targets.

## Build Decision Loop

The player should repeatedly face these questions:

1. Which memory am I relying on right now?
2. If I lose it, what echo will it become?
3. Do I want this memory active, echoed, or both through resonance?
4. Am I building toward a `+5` awakened echo?
5. Can I combine two `+5` echoes into a run-defining ultimate?

This changes forgetting from a pure penalty into a risky build pivot.

## Enemy Role Direction

Enemy behavior should support the memory/echo fantasy. It should not turn the game into a long chase.

### Melee Enemies

- Primary pressure source.
- Move toward the player and create density.
- Let AoE, orbit blades, knockback, and slow effects feel valuable.

### Ranged Enemies

Current HTML behavior lets shooter enemies back away when the player is close. That is acceptable as a prototype signal, but risky for the Unity version.

Unity rule:

- Ranged enemies may short backstep or strafe.
- They should stop to fire.
- They should not kite forever.
- They should not retreat off-screen as the main behavior.
- Their role is positional pressure, not forcing the player into a chase.

Recommended pattern:

1. Approach until in firing band.
2. Stop and telegraph shot.
3. Fire.
4. Short reposition.
5. Rejoin pressure if too far from the player or arena center.

## Unity First Slice Scope

The first Unity slice should not try to rebuild the whole HTML prototype at once.

Required:

- Player movement and basic weapon attack.
- A small enemy set: melee pressure enemy, ranged pressure enemy, one tougher elite or gate enemy.
- Active memory slots.
- Memory level `+1` to `+5`.
- Forgetting event that converts memory level into echo level.
- Echo level `+1` to `+5`.
- Reacquisition resonance rule.
- Two awakened echoes.
- One ultimate echo synergy.
- Simple run report/debug panel showing active memories, echo levels, and synergies.

Out of scope for first Unity slice:

- Shop.
- Meta progression.
- Multi-region structure.
- Final boss.
- Large memory roster expansion.
- Full narrative content.

## Data Model Direction

Recommended Unity data shape:

```text
MemoryDefinition
- id
- displayName
- tags
- activeAbility
- levelScaling
- matchingEchoId

EchoDefinition
- id
- sourceMemoryId
- displayName
- levelScaling
- awakenedEffect
- resonanceRider

EchoSynergyDefinition
- id
- requiredEchoIds
- displayName
- effect
- unlockLevelRequirement

RunBuildState
- activeMemories: memory id -> level
- echoes: echo id -> level
- unlockedEchoSynergies
```

This should be data-driven from the start so Codex/AI can help generate definitions without rewriting core combat code.

## Open Questions

- Should echo overflow above `+5` be ignored, converted into temporary power, or stored as progress toward an ultimate? First recommendation: ignore overflow until the core loop is fun.
- Should a reacquired memory ever consume its echo? First recommendation: no. Consuming the echo weakens the central fantasy.
- Should a `+5` echo always awaken immediately, or require a player choice? First recommendation: awaken immediately for the first slice.
- How many active memory slots should Unity start with? First recommendation: keep the HTML prototype's small-slot pressure before expanding.

## Current Decision

Proceed toward Unity planning with the memory/echo/resonance structure as the core system.

The next step is to turn this document into a Unity vertical-slice backlog: data definitions, combat prefabs, UI/debug panels, first awakened echoes, and one ultimate echo synergy.
