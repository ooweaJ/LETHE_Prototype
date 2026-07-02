# Next Tasks

## 1. Echo / Ultimate Runtime Cleanup

- Priority: highest
- Problem: utility echo tuning now lives in `_dev/Data`, but repeated color, radius, damage, timing, and compatibility fallback constants still remain across echo/ultimate runtime paths.
- Build:
  - Move remaining repeated echo/ultimate effect constants into compact specs.
  - Remove any remaining disabled or unreachable legacy utility echo/ultimate branches only after QA proves the data route is stable.
  - Keep Echo Matrix and Utility Ultimate Matrix QA passing for both weapons.
- Done:
  - `V1GameManager` echo/ultimate sections are shorter.
  - Repeated effect constants have one clear spec/data access point.
  - No disabled compatibility branches remain in the cleaned routes.

## 2. Passive Memory Feel Tuning

- Priority: medium-high
- Problem: `BloodReflection`, `AshenShield`, `StoppedSecond`, and `OblivionBrand` now have stronger action beats, but cadence/damage/readability still need play-feel tuning.
- Build:
  - Tune pulse intervals, damage, radius, and opacity one memory at a time.
  - Keep `LETHE/V1 QA/Passive Memory Matrix` as a regression gate.
- Done:
  - All four memories feel useful before forgetting without overwhelming base weapon readability.

## 3. Forget / Resonance UX Tuning

- Priority: medium
- Problem: the compressed forget/resonance flow is visible and testable, but direct play still needs to judge overlay length, VFX timing, and ultimate bridge clutter.
- Build:
  - Tune `ForgetFlow_*` scale, lifetime, placement, and text density.
  - Keep `LETHE/V1 QA/Forget Resonance Flow` as the regression gate.
- Done:
  - Forgetting reads first as an action transition, then as text confirmation.

## 4. Ultimate Feel Tuning

- Priority: medium
- Problem: all four ultimate families have weapon-specific routes, but the non-blood ultimates need direct-play judgment for cadence, power, and clutter.
- Build:
  - Tune `FractureExecution`, `StasisHunt`, and `AshenOblivion` per weapon.
  - Keep both `Utility Ultimate Matrix` QA menus as regression gates.
- Done:
  - Non-blood ultimates feel as distinct as Blood Blade Storm without becoming screen noise.

## 5. Direct Play Review Prep

- Priority: medium
- Problem: automated QA proves object spawning and compilation, but final feel still needs a focused jaewoo direct-play checklist after the cleanup passes.
- Build:
  - Keep the review prompt/checklist current.
  - Preserve one-click QA menus for all major routes.
  - Avoid broad new systems before direct play.
- Done:
  - Jaewoo can review one current `Dev_Prototype_v1` build without extra setup.

Completed sequence:

- 2026-07-02: weapon-specific echo pass implemented.
- 2026-07-02: passive-feeling active memory reinforcement implemented.
- 2026-07-02: forgetting / resonance UX flow implemented.
- 2026-07-02: non-blood utility ultimates weapon-pattern pass implemented.
- 2026-07-02: Kalmuri performance optimization implemented; perf matrix final count `totalKalmuri=374` from first measured fail `690`.
- 2026-07-02: first echo tuning spec / QA counter cleanup pass implemented.
- 2026-07-02: utility echo tuning moved into a serializable manager table with default fallback.
- 2026-07-02: utility echo tuning migrated to `_dev/Data/Echoes/UtilityEcho_Tuning.asset`.
- 2026-07-02: unreachable legacy utility echo fallback branch removed after Echo/Ultimate/Kalmuri QA remained PASS.

QA menus passing:

- `LETHE/V1 QA/Echo Matrix Dual Blades`
- `LETHE/V1 QA/Echo Matrix Greatsword`
- `LETHE/V1 QA/Passive Memory Matrix`
- `LETHE/V1 QA/Forget Resonance Flow`
- `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`
- `LETHE/V1 QA/Utility Ultimate Matrix Greatsword`
- `LETHE/V1 QA/Kalmuri Perf Matrix`

Keep this file short. Detailed history belongs in `docs/TASK.md`, `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.
