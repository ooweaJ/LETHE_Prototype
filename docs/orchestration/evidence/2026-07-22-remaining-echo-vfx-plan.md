# 2026-07-22 Remaining Echo / Ultimate VFX Direction

![Remaining Echo VFX concept board](2026-07-22-remaining-echo-vfx-concept-board.png)

## Current Read

- Kalmuri, Blood, Hunter, Shatter, and Stopped now have usable directions.
- Blood needed a bug fix because the visible echo read could disappear after the first obvious trigger:
  - dead Greatsword targets skipped Blood accent VFX;
  - dense Dual Blades marked enemies but suppressed almost all visible Blood read.
- Ashen and Oblivion still need concept-up rework before more scale/color tuning.
- Ultimate Echoes should be treated as a later dopamine pass, with stronger ceremony than normal Echoes.

## Normal Echoes

### Ashen Shield / 잿빛

- Fantasy: stored defense turns into counter-pressure.
- Visual example: top-left concept-board panel.
- Shared VFX:
  - pale ash-white shield plate appears for a short beat;
  - plate cracks from the center;
  - shards burst outward;
  - a counter wave follows the break.
- Dual Blades:
  - enemy-side parry sparks;
  - two short counter cuts snap outward from the target;
  - small ash shards mark blocked/countered enemies.
- Greatsword:
  - large shield wall/face appears in front of the player or target;
  - wall fractures into a heavy horizontal counter wave;
  - stronger hitstop than Dual Blades.
- Dopamine lever:
  - charge flash before release;
  - crack sound/flash;
  - visible outward burst after the guard breaks.

### Oblivion Brand / 낙인

- Fantasy: a memory is stamped, spreads, then gets erased.
- Visual example: top-right concept-board panel.
- Shared VFX:
  - black-violet sigil stamp appears under the target;
  - cracks spread from the brand;
  - the center briefly becomes a dark void;
  - fragments lift upward and disappear.
- Dual Blades:
  - brand hops between targets like a fast curse;
  - each hop leaves a tiny void scar;
  - the final stack pops in a small erase burst.
- Greatsword:
  - one large brand stamp lands with a heavy impact;
  - delayed collapse pulls nearby branded targets inward;
  - aftermath leaves a dark erased crater.
- Dopamine lever:
  - delayed detonation after a visible brand stack;
  - target blink/desaturation during erase;
  - bigger final collapse for Greatsword.

## Ultimate Echoes

### Blood Blade Storm / 피의 칼폭풍

- Visual example: bottom-left concept-board panel.
- Current role: benchmark ultimate.
- Next pass:
  - opening: screen hitstop + crimson/white vortex ring;
  - loop: weapon-shaped blades orbit and slash;
  - climax: central blood-star detonation;
  - aftermath: healing threads return to player.

### Fracture Execution / 균열 처형

- Pair direction: Shatter + Execution.
- VFX:
  - execution line draws across cracked ground;
  - ground fissure opens like a sentence line;
  - a gold-white verdict blade falls;
  - condemned targets burst through cracks.
- Dual Blades:
  - repeated verdict ticks across branded targets.
- Greatsword:
  - one huge guillotine cleave along the fissure.

### Stasis Hunt / 정지 사냥

- Visual example: bottom-right concept-board panel.
- Pair direction: Stopped + Hunter.
- VFX:
  - clock field freezes targets;
  - green hunter blades hang in place for a beat;
  - second hand snaps;
  - all blades release at once.
- Dual Blades:
  - many frozen ricochet daggers release in sequence.
- Greatsword:
  - a huge greatsword hand pierces the clock face.

### Ashen Oblivion / 잿빛 망각

- Pair direction: Ashen + Oblivion.
- VFX:
  - ash shield forms;
  - shield cracks open into black-violet void;
  - stored guard detonates as erasure;
  - enemies leave ash silhouettes before disappearing.
- Dual Blades:
  - parry sparks become void cuts.
- Greatsword:
  - ash wall collapses forward into a void stamp.

## Implementation Order

1. Fix Blood visibility regression.
2. Rework Ashen as stored guard/counter-pressure.
3. Rework Oblivion as brand spread/erase.
4. Direct-play normal Echo dopamine review.
5. Run Ultimate Echo dopamine pass after normal Echoes stop feeling generic.
