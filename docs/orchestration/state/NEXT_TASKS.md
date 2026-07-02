# Next Tasks

## 1. 에코 데이터화와 QA 카운터 정리

- Priority: highest
- Problem: weapon data is in `_dev/Data`, but many echo proc values, ultimate values, and VFX knobs still live inside `V1GameManager`.
- Build:
  - Move echo chance/radius/damage/timing/readability knobs toward data or compact specs.
  - Keep deterministic echo and ultimate matrix smoke for dual blades and greatsword.
  - Capture object counts for each echo/ultimate family so regressions are visible without relying only on hand feel.
- Done:
  - Tuning echo timing/scale no longer requires editing one large manager path.
  - QA can prove every echo family and non-blood ultimate family spawned for both weapons.

## 2. 에코 차별화 후속 정리

- Priority: medium-high
- Problem: the first weapon-specific echo and ultimate passes are playable and testable, but old fallback code and repeated constants still exist in the manager.
- Build:
  - Remove disabled or unreachable legacy utility echo/ultimate branches after the new patterns are stable.
  - Move repeated echo/ultimate color/radius/damage constants into compact per-effect specs.
  - Add screenshots/evidence if Unity capture becomes reliable enough.
- Done:
  - `V1GameManager` echo/ultimate sections are shorter and no disabled compatibility branches remain.

## 3. 패시브 기억 밸런스/체감 튜닝

- Priority: medium
- Problem: `BloodReflection`, `AshenShield`, `StoppedSecond`, and `OblivionBrand` now have stronger action beats, but their cadence/damage/readability still need play-feel tuning after direct review.
- Build:
  - Tune pulse intervals, damage, radius, and opacity one memory at a time.
  - Keep the `LETHE/V1 QA/Passive Memory Matrix` smoke as a regression gate.
- Done:
  - All four memories feel useful before forgetting without overwhelming base weapon readability.

## 4. 망각/공명 UX 체감 튜닝

- Priority: medium
- Problem: the compressed forget/resonance flow is now visible and testable, but direct play still needs to judge whether the overlay, VFX timing, and ultimate bridge are too busy.
- Build:
  - Tune `ForgetFlow_*` scale, lifetime, placement, and text density.
  - Keep `LETHE/V1 QA/Forget Resonance Flow` as the regression gate.
- Done:
  - Forgetting reads first as an action transition, then as text confirmation.

## 5. 4궁극 체감 튜닝

- Priority: medium
- Problem: all four ultimate families now have weapon-specific routes, but the new non-blood ultimates need direct-play judgment for cadence, power, and clutter.
- Build:
  - Tune `FractureExecution`, `StasisHunt`, and `AshenOblivion` per weapon.
  - Keep the two `Utility Ultimate Matrix` QA menus as regression gates.
- Done:
  - Non-blood ultimates feel as distinct as Blood Blade Storm without becoming screen noise.

Completed sequence:

- 2026-07-02: weapon-specific echo pass implemented.
- 2026-07-02: passive-feeling active memory reinforcement implemented.
- 2026-07-02: forgetting / resonance UX flow implemented.
- 2026-07-02: non-blood utility ultimates weapon-pattern pass implemented.
- QA menus passing:
  - `LETHE/V1 QA/Echo Matrix Dual Blades`
  - `LETHE/V1 QA/Echo Matrix Greatsword`
  - `LETHE/V1 QA/Passive Memory Matrix`
  - `LETHE/V1 QA/Forget Resonance Flow`
  - `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`
  - `LETHE/V1 QA/Utility Ultimate Matrix Greatsword`

Keep this file short. Detailed history belongs in `docs/TASK.md`, `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.
