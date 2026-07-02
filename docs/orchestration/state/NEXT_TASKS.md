# Next Tasks

## 1. Direct Play Review Prep

- Priority: medium
- Problem: automated QA proves object spawning and compilation, but final feel still needs a focused jaewoo direct-play checklist after the cleanup passes.
- Build:
  - Keep the review prompt/checklist current.
  - Preserve one-click QA menus for all major routes.
  - Avoid broad new systems before direct play.
- Done:
  - Jaewoo can review one current `Dev_Prototype_v1` build without extra setup.

## 2. Remaining Echo / Ultimate Constant Cleanup

- Priority: lower
- Problem: some repeated colors/timing constants remain in VFX helper routes after the current cleanup/dataization passes.
- Build:
  - Compact only the constants that clearly repeat or block tuning.
  - Avoid broad churn before direct play.
- Done:
  - Code stays readable without delaying feel validation.

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
- 2026-07-02: passive memory feel tuning implemented for BloodReflection, StoppedSecond, AshenShield, and OblivionBrand; passive/echo/forget/Kalmuri QA remained PASS.
- 2026-07-02: forget/resonance UX compressed; Forget Resonance, Utility Ultimate Dual, and Kalmuri Perf QA remained PASS.
- 2026-07-02: utility ultimate feel tuning implemented; Utility Ultimate Dual/Great and Blood Blade Storm QA remained PASS.

QA menus passing:

- `LETHE/V1 QA/Echo Matrix Dual Blades`
- `LETHE/V1 QA/Echo Matrix Greatsword`
- `LETHE/V1 QA/Passive Memory Matrix`
- `LETHE/V1 QA/Forget Resonance Flow`
- `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`
- `LETHE/V1 QA/Utility Ultimate Matrix Greatsword`
- `LETHE/V1 QA/Kalmuri Perf Matrix`

Keep this file short. Detailed history belongs in `docs/TASK.md`, `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.
