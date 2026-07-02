# Next Tasks

## 1. 4궁극 무기별 패턴 확장

- Priority: highest
- Problem: `BloodBladeStorm` has the clearest dual/greatsword split; the other three ultimates are still closer to large periodic utility effects.
- Build:
  - Extend `FractureExecution`, `StasisHunt`, and `AshenOblivion` with dual-blade and greatsword patterns.
  - Dual blades: repeated execution cuts, many small tracking shots, quick guard/brand returns.
  - Greatsword: execution stamp, slow frozen spear/cleave, large shield-break brand wave.
- Done:
  - Utility ultimate smoke shows all four ultimate families with distinct weapon rhythm.
  - Blood Blade Storm remains the benchmark, not the only polished ultimate.

## 2. 에코 데이터화와 QA 카운터 정리

- Priority: medium-high
- Problem: weapon data is in `_dev/Data`, but many echo proc values and VFX knobs still live inside `V1GameManager`.
- Build:
  - Move echo chance/radius/damage/timing/readability knobs toward data or small specs.
  - Keep deterministic echo matrix smoke for dual blades and greatsword.
  - Capture object counts for each echo family so regressions are visible without relying only on hand feel.
- Done:
  - Tuning echo timing/scale no longer requires editing one large manager path.
  - QA can prove every echo family spawned for both weapons.

## 3. 에코 차별화 후속 정리

- Priority: medium
- Problem: the first weapon-specific echo pass is playable and testable, but some old fallback code still exists in the manager until the next cleanup/data pass.
- Build:
  - Remove disabled legacy utility echo branches after the new patterns are stable.
  - Move repeated echo color/radius/damage constants into compact per-echo specs.
  - Add screenshots/evidence if Unity capture becomes reliable enough.
- Done:
  - `V1GameManager` echo section is shorter and no disabled compatibility branches remain.

## 4. 패시브 기억 밸런스/체감 튜닝

- Priority: medium
- Problem: `BloodReflection`, `AshenShield`, `StoppedSecond`, and `OblivionBrand` now have stronger action beats, but their cadence/damage/readability still need play-feel tuning after direct review.
- Build:
  - Tune pulse intervals, damage, radius, and opacity one memory at a time.
  - Keep the `LETHE/V1 QA/Passive Memory Matrix` smoke as a regression gate.
- Done:
  - All four memories feel useful before forgetting without overwhelming base weapon readability.

## 5. 망각/공명 UX 체감 튜닝

- Priority: medium
- Problem: the compressed forget/resonance flow is now visible and testable, but direct play still needs to judge whether the overlay, VFX timing, and ultimate bridge are too busy.
- Build:
  - Tune `ForgetFlow_*` scale, lifetime, placement, and text density.
  - Keep `LETHE/V1 QA/Forget Resonance Flow` as the regression gate.
- Done:
  - Forgetting reads first as an action transition, then as text confirmation.

Completed sequence:

- 2026-07-02: weapon-specific echo pass implemented.
- 2026-07-02: passive-feeling active memory reinforcement implemented.
- 2026-07-02: forgetting / resonance UX flow implemented.
- QA menus passing:
  - `LETHE/V1 QA/Echo Matrix Dual Blades`
  - `LETHE/V1 QA/Echo Matrix Greatsword`
  - `LETHE/V1 QA/Passive Memory Matrix`
  - `LETHE/V1 QA/Forget Resonance Flow`

Keep this file short. Detailed history belongs in `docs/TASK.md`, `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.
