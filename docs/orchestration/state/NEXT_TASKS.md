# Next Tasks

## 1. 패시브화 기억 보강

- Priority: highest
- Problem: several active memories are useful but still feel passive because they pulse, mark, or heal without a strong independent action read.
- Build:
  - Focus first on `BloodReflection`, `AshenShield`, `StoppedSecond`, and `OblivionBrand`.
  - Give each active memory a clear +1/+3/+5 action beat independent of basic weapon hits.
  - Make +5 feel like a behavior change, not only larger numbers.
- Done:
  - Memory-only debug for the four memories creates obvious action VFX and damage/defense/heal events.
  - The active memory loop can be understood before it is forgotten into an echo.

## 2. 망각/공명 UX 제작 패스

- Priority: high
- Problem: forgetting and resonance are mechanically wired, but the player-facing meaning can still feel like a status update instead of a dramatic build shift.
- Build:
  - Strengthen the result overlay and HUD for lost memory, gained echo, current resonance target, and +5 awakening.
  - Add short transition cues that show "memory became echo" through VFX/action before text.
  - Add a debug path that forces memory -> forget -> echo -> resonance target without needing a long run.
- Done:
  - A compressed flow shows the loss, echo grant, resonance next target, and +5 goal clearly.
  - Console errors remain 0.

## 3. 4궁극 무기별 패턴 확장

- Priority: medium-high
- Problem: `BloodBladeStorm` has the clearest dual/greatsword split; the other three ultimates are still closer to large periodic utility effects.
- Build:
  - Extend `FractureExecution`, `StasisHunt`, and `AshenOblivion` with dual-blade and greatsword patterns.
  - Dual blades: repeated execution cuts, many small tracking shots, quick guard/brand returns.
  - Greatsword: execution stamp, slow frozen spear/cleave, large shield-break brand wave.
- Done:
  - Utility ultimate smoke shows all four ultimate families with distinct weapon rhythm.
  - Blood Blade Storm remains the benchmark, not the only polished ultimate.

## 4. 잔향 데이터화와 QA 하네스 정리

- Priority: medium
- Problem: weapon data is in `_dev/Data`, but many echo proc values and VFX knobs still live inside `V1GameManager`.
- Build:
  - Move echo chance/radius/damage/timing/readability knobs toward data or small specs.
  - Keep deterministic echo matrix smoke for dual blades and greatsword.
  - Capture object counts for each echo family so regressions are visible without relying only on hand feel.
- Done:
  - Tuning echo timing/scale no longer requires editing one large manager path.
  - QA can prove every echo family spawned for both weapons.

## 5. 잔향 차별화 후속 정리

- Priority: medium
- Problem: the first weapon-specific echo pass is playable and testable, but some old fallback code still exists in the manager until the next cleanup/data pass.
- Build:
  - Remove disabled legacy utility echo branches after the new patterns are stable.
  - Move repeated echo color/radius/damage constants into compact per-echo specs.
  - Add screenshots/evidence if Unity capture becomes reliable enough.
- Done:
  - `V1GameManager` echo section is shorter and no disabled compatibility branches remain.

Completed first sequence:

- 2026-07-02: 잔향 무기별 차별화 1차 패스 implemented.
- Dual blades and greatsword now spawn separate `EchoDual_` / `EchoGreat_` behavior families for all 8 echoes.
- `LETHE/V1 QA/Echo Matrix Dual Blades` and `LETHE/V1 QA/Echo Matrix Greatsword` pass.

Keep this file short. Detailed history belongs in `docs/TASK.md`, `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.
