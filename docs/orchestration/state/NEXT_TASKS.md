# Next Tasks

## 1. Dense Dual-Blade Performance / VFX Churn Pass

- Priority: high
- Problem: jaewoo felt lag when dual blades attack into many enemies. The likely risk is dense hit-triggered transient VFX plus repeated enemy queries.
- Build:
  - Add a dense dual-blade QA/perf matrix with object-count and timing-risk metrics.
  - Reduce or pool the worst repeated transient VFX families.
  - Replace expensive whole-scene enemy scans where practical, starting with VoidPriest healing.
  - Keep effect readability while lowering object churn.
- Done:
  - Dense dual-blade waves no longer hitch noticeably and QA stays under object-count limits.

## 2. VFX Identity / Echo Readability Pass

- Priority: high
- Problem: VFX identity is still low. Memories/echoes pass QA but do not read as different enough in actual combat.
- Build:
  - Add clearer monster-state marks for Shatter, Stopped, Ashen, Hunter, Execution, and Oblivion.
  - Make each state visually distinct before adding raw damage or screen-filling effects.
  - Add contribution metrics for damage/heal/control, not only object counts.
- Done:
  - Direct play can identify what each memory/echo is doing before reading text.

## 3. Hungry Blades / Kalmuri Echo Visual Redesign

- Priority: high
- Problem: jaewoo still feels the Kalmuri echo visual does not fit the concept. More cyan blade clutter is not enough.
- Build:
  - Reframe Kalmuri echo around a recognizable blade action: hunt, pierce, clamp, rip, or return.
  - Reduce generic rings and make the echo different from the active memory orbit.
  - Verify +1/+3/+5 scale without overloading dense fights.
- Done:
  - Jaewoo can look at the effect and say it matches "칼무리" without relying on labels.

## 4. Gatekeeper Jump / Boss Pattern Direct Review

- Priority: high
- Problem: the new `F6`/`Boss` debug jump works technically, but the boss pattern and body still need direct feel judgment.
- Build:
  - Press `F6` or use the F12 `Boss` button.
  - Judge red danger-zone readability, TTK, guard feel, add pressure, and boss body quality.
- Done:
  - Review returns one narrow boss adjustment: telegraph timing, danger size, HP/guard, add pressure, or art polish.

## 5. Healer / Enemy Separation Direct Review

- Priority: high
- Problem: VoidPriest healing and enemy separation are technically fixed, but they need dense-wave direct play judgment.
- Build:
  - Test dense waves with multiple priests and mixed enemies.
  - Check whether heal source/readability, killability, and soft separation all feel fair.
- Done:
  - Review returns whether to tune heal amount, priest frequency, receiver lockout, separation padding, or enemy cap.

Completed sequence:

- 2026-07-06: Gatekeeper jump debug implemented; `F6`, F12 `Boss`, and `LETHE/V1 QA/Gatekeeper Jump` now work.
- 2026-07-06: VoidPriest heal stacking/readability pass implemented; heal matrix, M2, Echo Matrix Dual, Passive Memory Matrix, and Utility Ultimate Dual QA passed.
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

- `LETHE/V1 QA/Gatekeeper Jump`
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
