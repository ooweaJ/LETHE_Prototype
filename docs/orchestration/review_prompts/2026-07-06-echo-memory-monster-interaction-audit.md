# Echo / Memory / Ultimate Versus Monster Interaction Audit

Date: 2026-07-06

## Context

Jaewoo reported that stacked healer enemies make waves feel impossible to kill unless Blood Reflection is active, and that the differences between memories, echoes, and ultimate echoes are not clearly felt. The current code passes object-count QA, but direct play suggests the combat readability and payoff balance are uneven.

## Immediate Fix Already Applied

- `VoidPriest` no longer heals bosses.
- `VoidPriest` now has visible green heal VFX.
- Priest healing now has:
  - 1.05s cadence,
  - 2.4 heal amount,
  - 3 target cap per priest pulse,
  - 0.95s per-target priest-heal lockout.
- QA: `Void Priest Heal Matrix` passed with `attempts=12`, `accepted=4`, `vfx=16`.

## Observed Balance / Readability Problems

0. New direct-play feedback after the healer fix.
   - VFX identity is still low; many effects exist but are not easy to distinguish during real combat.
   - Hungry Blades / Kalmuri echo still does not visually match the intended concept.
   - Dual blades appear to cause lag when enemy density rises.
   - The next implementation should reduce dense-hit object churn before increasing VFX volume.

1. Blood Reflection feels strongest because it covers too many jobs.
   - It marks on weapon hit.
   - It adds visible wound/pulse VFX frequently.
   - It adds DoT, burst, self-heal, thread VFX, bloom, and Blood Blade Storm progression.
   - It directly counters healer sustain because it keeps damaging marked targets while healing the player.

2. Other memories are functional but less legible as monster interactions.
   - Execution Flash is gated by low HP, so it can feel absent until it suddenly works.
   - Hunter Oath shoots projectiles, but the monster-side state change is less memorable than blood marks.
   - Shatter Wave and Stopped Second have strong utility but low object counts in QA and can read as field VFX rather than enemy impact.
   - Ashen Shield is defensive; its monster impact can feel secondary.
   - Oblivion Brand has high object count but random target selection can make it feel noisy rather than intentional.

3. Echo count/readability is uneven.
   - Echo Matrix Dual result: `K=8`, `B=56`, `Ex=64`, `H=24`, `Sh=8`, `St=8`, `A=32`, `O=40`.
   - Blood and Execution are very present.
   - Kalmuri, Shatter, and Stopped are much lower-count or more conditional, so their effects may be lost in the visual field.

4. Passive Memory Matrix shows similar unevenness.
   - `blood=17`, `ash=6`, `stopped=8`, `oblivion=36`.
   - Blood and Oblivion visibly spam more objects.
   - Ashen and Stopped can be correct mechanically but under-read emotionally.

5. Ultimate pairing is under-communicated.
   - Blood Blade Storm has a clear identity and strong recurring VFX.
   - Utility ultimates pass QA, but the route from memory -> echo -> ultimate is harder to read.
   - The HUD/goal text still primarily teaches the blood pair first, so non-blood ultimate goals may feel optional or opaque.

6. Monster interaction lacks explicit counter-language.
   - Healer now has VFX, but the player still needs clearer "this enemy is sustaining the wave" feedback.
   - Crowd control memories should visibly change enemy state: cracked, frozen, branded, exposed, interrupted, shield-broken.
   - Current feedback often appears as area effects rather than persistent enemy-state marks.

## Recommended Next Codex Passes

1. Dense dual-blade performance / VFX churn pass.
   - Add a QA/perf matrix for dual blades against dense waves.
   - Measure object counts for weapon slashes, hit sparks, Kalmuri, Blood, and utility echoes.
   - Cap/pool the worst repeated transient families before adding more spectacle.
   - Replace whole-scene scans where possible, especially `VoidPriest` healing via `FindObjectsByType`.

2. Kalmuri echo visual redesign.
   - Treat this as concept correction, not only size/alpha tuning.
   - Make the echo read as a blade action: hunt, pierce, clamp, rip, or return.
   - Reduce generic rings and distinguish echo from active memory orbit.

3. Non-blood enemy-state readability pass.
   - Add persistent short-lived state markers for Shatter, Stopped, Ashen, Hunter, Execution, and Oblivion.
   - Keep object counts bounded; make each marker distinctive rather than simply more numerous.

4. Healer counterplay pass.
   - Make VoidPriest heal target obvious.
   - Consider a "heal interrupted" or "heal denied" effect when control/damage thresholds hit priests.
   - Check if priests should expose themselves during healing.

5. Memory versus Echo role split.
   - Memory should feel like the active/current combat identity.
   - Echo should feel like a transformed residual effect after forgetting.
   - Avoid Blood being the only one that clearly does both.

6. Ultimate route communication.
   - Show all four ultimate pair goals in debug/review HUD or level-up language.
   - Give each ultimate pair a different monster-state signature before the ultimate fires.

7. Measurement improvement.
   - Add a QA matrix that records per-family damage/heal/control counts, not only VFX object counts.
   - Use it to detect "looks loud but does little" and "does a lot but is invisible" cases.

## Suggested Review Question

After the VoidPriest fix, direct-play a wave with healer support and compare four builds:

- Blood Reflection + Hungry Blades.
- Shatter Wave + Execution Flash.
- Stopped Second + Hunter Oath.
- Ashen Shield + Oblivion Brand.

For each, answer:

- Can I tell what the memory is doing before reading text?
- Can I tell what the echo is doing after forgetting?
- Does the monster visibly enter a changed state?
- Does the pairing make me want the ultimate?
- Does it solve a combat problem other than raw damage?
