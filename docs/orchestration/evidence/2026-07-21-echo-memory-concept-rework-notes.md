# 2026-07-21 Echo / Memory Concept Rework Notes

## Context

jaewoo's latest review separates two problems:

- Some Echoes have readability problems in the current build.
- Some Echoes may not yet have a strong enough memory concept underneath them, so making the VFX bigger would still feel like "same effect, different size."

This note records the next concept direction before more VFX-only edits.

## Implemented In This Pass

### Blood Reflection / Greatsword

- Current problem: Greatsword Blood Echo was hard to see.
- Direction taken: keep the Blood memory concept, but make the Greatsword Echo read as a heavy blood-iaido follow-up.
- Runtime change:
  - larger red crescent stack;
  - shadow crescent and second crescent layer;
  - larger impact zone;
  - blood bloom and radial petals;
  - longer wound cut and stronger hit feedback.
- Rationale:
  - Blood Reflection still works conceptually because it is visible, visceral, and mechanically tied to mark/heal payoff. It needed readability, not a new concept.

### Hunter Oath / Weapon Split

- Current problem: Hunter Echo felt too close to generic tracking/projectile VFX.
- New direction:
  - Dual Blades: two separate green blades bounce between enemies.
  - Greatsword: one large green greatsword pierces through the field.
- Rationale:
  - The memory concept remains "tracking/hunting," but now the action reflects each weapon's body language:
    - Dual Blades hunt as fast paired blades.
    - Greatsword hunts as one committed execution line.

## Next Concept Review Candidates

### Stopped Second

- Keep the core concept.
- Needed pass: premium clockwork/watch VFX.
- Desired read:
  - thin clock ring;
  - clean hand sweep;
  - tiny tick marks;
  - restrained bright edge;
  - less generic dome/ring noise.

### Shatter Wave

- Risk:
  - If it remains needles/rings, it can overlap with Hunter and Execution.
- Concept question:
  - Is this memory "fracture in the world" or "impact shockwave from the weapon"?
- Proposed direction:
  - Active memory should feel like cracks opening under pressure.
  - Dual Blades Echo can chain small fracture cuts through the ground or through marked enemies, but should avoid becoming another ricochet projectile.
  - Greatsword Echo should become a clear ground-fissure rupture with terrain break and dust, not just a larger cone.

### Ashen Shield

- Risk:
  - The current gray/ashen effects can read like a neutral burst instead of a defensive memory.
- Concept question:
  - Is this memory about guarding, sacrifice, counterattack, or survival after being hit?
- Proposed direction:
  - Active memory should be player-side protection with visible stored pressure.
  - Dual Blades Echo can read as enemy-side parry sparks or a quick counter-cut.
  - Greatsword Echo can read as an ash wall / shield face that releases one heavy counter wave.

### Oblivion Brand

- Risk:
  - Purple marks and rings can feel abstract unless the brand behavior is legible.
- Concept question:
  - Is this memory about marking, erasing, spreading, or consuming forgotten enemies?
- Proposed direction:
  - Active memory should show clear brand stacking/spreading.
  - Dual Blades Echo can hop brands quickly between targets.
  - Greatsword Echo can stamp one large brand and collapse the area.
  - VFX should emphasize erasure and disappearance, not only purple scale.

## Next Review Rule

For the next Echo pass, do not start by scaling VFX.

1. Confirm the active memory fantasy.
2. Confirm the Echo action fantasy.
3. Confirm the weapon-specific expression.
4. Then tune VFX size, color, timing, and density.
