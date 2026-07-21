# 2026-07-21 Blood / Stopped Dopamine VFX Pass

## Problem

jaewoo judged the current Echo VFX as readable but still low on dopamine. Greatsword Blood Echo in particular needed a stronger white/red circular slash-ring payoff, and Stopped Echo needed to keep the clock visible during the full frozen second with the second hand rotating.

## Implementation Evidence

- `LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs`
  - Greatsword Blood Echo now calls `SpawnGreatswordBloodVortexRing()` before the existing blood-iaido crescent stack.
  - `MakeBloodVortexRingSprite()` generates a broken white/red circular ring sprite at runtime.
  - Greatsword Blood hitstop and camera shake were increased.
  - Stopped Echo clamps freeze duration to at least `1.0s`.
  - `SpawnStoppedEchoClockwork()` now holds its clock elements through the freeze window and spawns a rotating second hand.
  - `V1ClockHandSweep` rotates the hand over the freeze duration.
  - Dense Dual Blades reduces dense-only Kalmuri/Blood decorations so the richer normal Echoes stay within budget.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: PASS, 7 existing legacy warnings, 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: PASS, 7 existing legacy warnings, 0 errors.
- Unity compilation errors: `0`.
- Unity console errors after final QA: `0`.
- `LETHE/V1 QA/Dense Dual Blades Perf Matrix`: PASS, `hits=18`, `suppressed=15`, `transient=36`, `activeVfx=15`, `ms=93.06`.
- `LETHE/V1 QA/Echo Matrix Dual Blades`: PASS, `total=1027`, `St=160`, `stateSt=11`.
- `LETHE/V1 QA/Echo Matrix Greatsword`: PASS, `total=779`, `B=87`, `St=168`, `stateSt=20`.

## Remaining Human Gate

Automated QA verifies that VFX spawn, states apply, and dense performance remains under budget. It does not prove dopamine. Direct play should judge whether Greatsword Blood feels like a blood-blade vortex and whether Stopped Echo reads as time frozen while the second hand moves.
