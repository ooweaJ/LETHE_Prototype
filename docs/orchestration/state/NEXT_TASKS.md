# Next Tasks

## 1. Gatekeeper Body Visual Review

- Priority: high
- Problem: jaewoo rejected the previous boss body as visually degraded; four authored Gatekeeper body sprites have now replaced the blob-like procedural body.
- Build:
  - Run `LETHE/V1 QA/Gatekeeper Pattern Matrix` or play to the first Gatekeeper.
  - Judge whether the boss reads as a deliberate gate/mask boss rather than a placeholder.
- Done:
  - Review returns whether to keep the new bodies or do one dedicated boss art pass.

## 2. Dense Wave / Enemy Separation Direct Review

- Priority: high
- Problem: enemies now use soft separation instead of perfect overlap, but the feel needs human judgment.
- Build:
  - Play `Assets/_dev/Scenes/Dev_Prototype_v1.unity`.
  - Let a dense add wave gather around the player and first Gatekeeper.
  - Judge whether enemies still feel threatening while no longer reading as one stacked blob.
- Done:
  - Review returns whether to tune separation padding, normal-enemy multiplier, boss-space multiplier, or DriftingEye standing separation.

## 3. Gatekeeper Pattern Direct Review

- Priority: high
- Problem: the first boss now has telegraphed patterns and no longer receives healer support, but the feel still needs human judgment.
- Build:
  - Play `Assets/_dev/Scenes/Dev_Prototype_v1.unity`.
  - Reach the first Gatekeeper around 150 seconds.
  - Judge red danger-zone readability, fairness, boss HP/TTK, guard feel, and whether the boss sprite/pattern concept reads better.
- Done:
  - Review returns whether to tune telegraph timing, danger size, boss HP/guard uptime, or sprite polish.

## 4. Jaewoo Full Direct Play Review

- Priority: medium
- Problem: automated QA proves object spawning and compilation, but final GO/ITERATE/NO-GO depends on jaewoo feel judgment.
- Build:
  - Play `Assets/_dev/Scenes/Dev_Prototype_v1.unity`.
  - Use `docs/orchestration/review_prompts/2026-07-02-dev-prototype-v1-direct-play-review.md`.
  - Judge base weapons, Kalmuri, passive memories, echoes, forget/resonance, ultimates, audio, performance, and clutter.
- Done:
  - Review returns `GO`, `ITERATE`, or `NO-GO` with top issues.

## 5. Remaining Echo / Ultimate Constant Cleanup

- Priority: lower
- Problem: some repeated colors/timing constants remain in VFX helper routes after the current cleanup/dataization passes.
- Build:
  - Compact only the constants that clearly repeat or block tuning.
  - Avoid broad churn before direct play.
- Done:
  - Code stays readable without delaying feel validation.

Completed sequence:

- 2026-07-06: Gatekeeper body visual repair implemented; four boss PNGs now load by rank, Pattern Matrix and M2 Loop QA passed.
- 2026-07-06: enemy soft separation implemented; Enemy Separation Matrix and M2 Loop QA passed.
- 2026-07-06: Gatekeeper heal exclusion and telegraphed boss-pattern pass implemented; Pattern Matrix and M2 Loop QA passed.
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
- 2026-07-02: direct-play review checklist prepared at `docs/orchestration/review_prompts/2026-07-02-dev-prototype-v1-direct-play-review.md`.

QA menus passing:

- `LETHE/V1 QA/Enemy Separation Matrix`
- `LETHE/V1 QA/Gatekeeper Pattern Matrix`
- `LETHE/V1 QA/Echo Matrix Dual Blades`
- `LETHE/V1 QA/Echo Matrix Greatsword`
- `LETHE/V1 QA/Passive Memory Matrix`
- `LETHE/V1 QA/Forget Resonance Flow`
- `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`
- `LETHE/V1 QA/Utility Ultimate Matrix Greatsword`
- `LETHE/V1 QA/Kalmuri Perf Matrix`

Keep this file short. Detailed history belongs in `docs/TASK.md`, `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.
