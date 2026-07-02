# Next Tasks

## 1. Echo Tuning ScriptableObject Migration

- Priority: highest
- Problem: utility echo values now flow through a serializable table, but that table still lives inside `V1GameManager` instead of `_dev/Data`.
- Build:
  - Create a dedicated `_dev/Data` ScriptableObject/data contract for utility echo tuning specs.
  - Migrate the current `UtilityEchoTuningSpec[]` defaults into project data while keeping safe runtime fallback.
  - Repeat the same data-path pattern for remaining ultimate and VFX timing/readability knobs.
  - Keep Echo Matrix and Utility Ultimate Matrix QA passing for both weapons.
- Done:
  - Tuning utility echo timing/scale no longer requires editing the large manager path.
  - Scene/runtime references use `_dev/Data` assets for utility echo tuning.
  - QA proves every echo family and non-blood ultimate family still spawns for both weapons.

## 2. Echo / Ultimate Runtime Cleanup

- Priority: medium-high
- Problem: weapon-specific echo and ultimate passes are playable and testable, but repeated constants and compatibility fallback code still exist in the manager.
- Build:
  - Remove disabled or unreachable legacy utility echo/ultimate branches after the data route is stable.
  - Move repeated color, radius, damage, and timing constants into compact per-effect specs.
  - Add screenshots/evidence if Unity capture becomes reliable enough.
- Done:
  - `V1GameManager` echo/ultimate sections are shorter and no disabled compatibility branches remain.

## 3. Passive Memory Feel Tuning

- Priority: medium
- Problem: `BloodReflection`, `AshenShield`, `StoppedSecond`, and `OblivionBrand` now have stronger action beats, but cadence/damage/readability still need play-feel tuning.
- Build:
  - Tune pulse intervals, damage, radius, and opacity one memory at a time.
  - Keep `LETHE/V1 QA/Passive Memory Matrix` as a regression gate.
- Done:
  - All four memories feel useful before forgetting without overwhelming base weapon readability.

## 4. Forget / Resonance UX Tuning

- Priority: medium
- Problem: the compressed forget/resonance flow is visible and testable, but direct play still needs to judge overlay length, VFX timing, and ultimate bridge clutter.
- Build:
  - Tune `ForgetFlow_*` scale, lifetime, placement, and text density.
  - Keep `LETHE/V1 QA/Forget Resonance Flow` as the regression gate.
- Done:
  - Forgetting reads first as an action transition, then as text confirmation.

## 5. Ultimate Feel Tuning

- Priority: medium
- Problem: all four ultimate families have weapon-specific routes, but the non-blood ultimates need direct-play judgment for cadence, power, and clutter.
- Build:
  - Tune `FractureExecution`, `StasisHunt`, and `AshenOblivion` per weapon.
  - Keep both `Utility Ultimate Matrix` QA menus as regression gates.
- Done:
  - Non-blood ultimates feel as distinct as Blood Blade Storm without becoming screen noise.

Completed sequence:

- 2026-07-02: weapon-specific echo pass implemented.
- 2026-07-02: passive-feeling active memory reinforcement implemented.
- 2026-07-02: forgetting / resonance UX flow implemented.
- 2026-07-02: non-blood utility ultimates weapon-pattern pass implemented.
- 2026-07-02: Kalmuri performance optimization implemented; perf matrix final count `totalKalmuri=374` from first measured fail `690`.
- 2026-07-02: first echo tuning spec / QA counter cleanup pass implemented.
- 2026-07-02: utility echo tuning moved into a serializable manager table with default fallback.

QA menus passing:

- `LETHE/V1 QA/Echo Matrix Dual Blades`
- `LETHE/V1 QA/Echo Matrix Greatsword`
- `LETHE/V1 QA/Passive Memory Matrix`
- `LETHE/V1 QA/Forget Resonance Flow`
- `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`
- `LETHE/V1 QA/Utility Ultimate Matrix Greatsword`
- `LETHE/V1 QA/Kalmuri Perf Matrix`

Keep this file short. Detailed history belongs in `docs/TASK.md`, `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.
