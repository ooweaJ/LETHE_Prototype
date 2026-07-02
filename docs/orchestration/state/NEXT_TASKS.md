# Next Tasks

## 1. 잔향 무기별 차별화 1차 패스

- Priority: highest
- Problem: current echoes are technically present, but many utility echoes still read like the same on-hit burst with different color/scale.
- Build:
  - Split `TriggerUtilityEchoes` into explicit dual-blade and greatsword echo patterns.
  - Dual blades should feel fast, small, chained, and stack-based.
  - Greatsword should feel delayed, heavy, fewer, wider, and burst-based.
  - Preserve one `EchoDefinition` per memory; branch by weapon runtime style, not by making 16 separate echoes.
- Done:
  - Dual blades and greatsword produce visibly different behavior for all 8 echoes in debug/smoke.
  - VFX object names/counts make the difference inspectable.
  - `dotnet build`, Unity compile errors, and VFX Matrix smoke pass.

## 2. 패시브화 기억 보강

- Priority: high
- Problem: several active memories are useful but feel passive because they pulse, mark, or heal without a strong action read.
- Build:
  - Focus first on `BloodReflection`, `AshenShield`, `StoppedSecond`, and `OblivionBrand`.
  - Give each active memory a clear +1/+3/+5 action beat independent of basic weapon hits.
  - Make +5 feel like a behavior change, not only larger numbers.
- Done:
  - Memory-only debug for the four memories creates obvious action VFX and damage/defense/heal events.
  - The active memory loop can be understood before it is forgotten into an echo.

## 3. 망각/공명 UX 제작 패스

- Priority: high
- Problem: forgetting and resonance are mechanically wired, but the player-facing meaning can still feel like a status update instead of a dramatic build shift.
- Build:
  - Strengthen the result overlay and HUD for lost memory, gained echo, current resonance target, and +5 awakening.
  - Add short transition cues that show "memory became echo" through VFX/action before text.
  - Add a debug path that forces memory -> forget -> echo -> resonance target without needing a long run.
- Done:
  - A compressed flow shows the loss, echo grant, resonance next target, and +5 goal clearly.
  - Console errors remain 0.

## 4. 4궁극 무기별 패턴 확장

- Priority: medium-high
- Problem: `BloodBladeStorm` has the clearest dual/greatsword split; the other three ultimates are still closer to large periodic utility effects.
- Build:
  - Extend `FractureExecution`, `StasisHunt`, and `AshenOblivion` with dual-blade and greatsword patterns.
  - Dual blades: repeated execution cuts, many small tracking shots, quick guard/brand returns.
  - Greatsword: execution stamp, slow frozen spear/cleave, large shield-break brand wave.
- Done:
  - Utility ultimate smoke shows all four ultimate families with distinct weapon rhythm.
  - Blood Blade Storm remains the benchmark, not the only polished ultimate.

## 5. 잔향 데이터화와 QA 하네스 정리

- Priority: medium
- Problem: weapon data is in `_dev/Data`, but many echo proc values and VFX knobs still live inside `V1GameManager`.
- Build:
  - Move echo chance/radius/damage/timing/readability knobs toward data or small specs.
  - Add deterministic echo matrix smoke for dual blades and greatsword separately.
  - Capture object counts for each echo family so regressions are visible without relying only on hand feel.
- Done:
  - Tuning echo timing/scale no longer requires editing one large manager switch.
  - QA can prove every echo family spawned for both weapons.

Keep this file short. Detailed history belongs in `docs/TASK.md`, `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.
