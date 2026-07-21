# 2026-07-21 Shatter Echo Ground Fracture Rework

## Problem

- Shatter Echo still risked reading as another generic ring/needle/size variant.
- The remaining Echo concept plan required Shatter to become a distinct terrain/world fracture fantasy before moving on to Ashen and Oblivion.

## Direction

- Shatter is now treated as the world or ground breaking under the hit, not as another projectile or circular pulse.
- Dual Blades and Greatsword share cyan fracture color language, but differ by action silhouette:
  - Dual Blades: chained ground cracks under individual targets.
  - Greatsword: one heavy forward rupture line through the combat space.

## Implementation

- `V1GameManager.TriggerShatterEcho`
  - Dual Blades now uses `SpawnDualShatterGroundChain`.
  - Greatsword now uses `SpawnGreatswordShatterGroundRupture`.
  - Greatsword cone was narrowed to support fissure-line readability.
- New VFX helpers:
  - `SpawnGreatswordShatterGroundRupture`
  - `SpawnDualShatterGroundChain`
  - `SpawnShatterGroundBreakAt`
- Dense Dual Blades:
  - keeps state application and damage;
  - suppresses extra Shatter/Ashen identity burst/link VFX;
  - clears transient debug VFX before perf matrix setup.

## Verification

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: PASS, 7 existing legacy warnings, 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: PASS, 0 warnings, 0 errors.
- Unity compilation errors: `0`.
- `LETHE/V1 QA/Dense Dual Blades Perf Matrix`: PASS, `hits=18`, `suppressed=15`, `transient=97`, `activeVfx=81`, `ms=93.74`.
- `LETHE/V1 QA/Echo Matrix Dual Blades`: PASS, `total=1027`, `Sh=175`, `stateSh=12`.
- `LETHE/V1 QA/Echo Matrix Greatsword`: PASS on rerun, `total=742`, `Sh=144`, `stateSh=3`.

## Next

- Ashen: stored guard / counter-pressure.
- Oblivion: brand spread / erase.
