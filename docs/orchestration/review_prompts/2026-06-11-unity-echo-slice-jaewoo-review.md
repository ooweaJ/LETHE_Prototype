# Unity Echo Slice Jaewoo Review - 2026-06-11

## Purpose

Review whether the first Unity `_dev` slice makes LETHE's echo loop readable and exciting enough to continue iterating in Unity.

This is not a balance test. It is a feel/readability test.

## Open

1. Open Unity project:
   - `C:\jaewoo\LETHE_Prototype\LETHE`
2. Open scene:
   - `Assets/_dev/Scenes/Dev_EchoSlice.unity`
3. Press Play.

## Controls

- `1`: Base dual blades only.
- `2`: Kalmuri +1. Weapon hit creates delayed slash.
- `3`: Kalmuri +5. Orbit blades plus launch blade on hit.
- `4`: Blood +5. Blood mark, blood bloom, and heal thread on hit.
- `5`: Blood Blade Storm. Orbit/storm ring plus blood feedback.
- `Space`: Force one attack.

The scene also has a small debug panel in the top-left with the same state buttons.

## What To Look For

### 1. Base Dual Blades

Question:

- Can you read player, enemy, and attack direction without echo VFX?

Pass:

- Basic setup is readable enough to judge later effects.

Fail:

- Player/enemy/weapon scale is confusing before echoes even start.

### 2. Kalmuri +1

Question:

- Does the delayed slash feel like a weapon habit left behind by a lost memory?

Pass:

- It reads as a separate white-blue slash after the weapon hit.

Fail:

- It looks like the same basic attack, or it is too late/too faint to notice.

### 3. Kalmuri +5

Question:

- Does +5 feel like a new action, not just a bigger number?

Pass:

- Orbit blades and launch blade are visually distinct from +1.

Fail:

- The extra blade is hard to see, or the orbit reads as decoration only.

### 4. Blood +5

Question:

- Does Blood echo read as survival feedback rather than generic red damage?

Pass:

- Blood mark/bloom and heal thread feel connected: enemy gets marked, something returns to player.

Fail:

- Red effects merge into noise, or the heal thread is not readable.

### 5. Blood Blade Storm

Question:

- Do Kalmuri and Blood feel fused into one ultimate loop?

Pass:

- White blades and red blood feedback are both visible, and the state feels more exciting than either echo alone.

Fail:

- It is just visual clutter, or it does not feel meaningfully stronger/different.

## Decision

Choose one:

- `GO`: Keep this Unity direction. Next work should improve timing, pooling, hitstop, sound, and production runtime.
- `ITERATE`: The direction works, but one or two readability problems must be fixed before promotion.
- `NO-GO`: The echo loop still feels like labels/effects pasted onto attacks rather than a strong combat identity.

## Known Rough Edges

- VFX sprites are placeholder imagegen assets, not final art.
- Debug controller uses OnGUI and direct debug state switching.
- Persistent orbit/storm behavior is `_dev` debug implementation; it is not production runtime yet.
- No sound, hitstop, camera impulse, 2D light, or final UI.
- Only one test enemy is wired as the main hit target.

## Send Back To Codex

After review, give Codex:

1. Decision: `GO`, `ITERATE`, or `NO-GO`.
2. The weakest visual state: `Base`, `Kalmuri +1`, `Kalmuri +5`, `Blood +5`, or `Storm`.
3. One sentence on whether Blood Blade Storm has enough “뽕맛”.
4. One concrete fix to do first.
