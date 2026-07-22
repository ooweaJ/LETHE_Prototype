# LETHE TASK

## 2026-07-22 Weapon-Specific Echo Mutation VFX Pass

- [x] Echo weapon mutation pass completed:
  - [x] Blood Echo now has separate Dual Blades twin-pip/needle and Greatsword ritual-spine/crescent-teeth layers.
  - [x] Shatter Echo now has separate Dual Blades skip-chip cuts and Greatsword anvil/down-slam rupture layers.
  - [x] Execution Echo now has separate Dual Blades sentence/barcode marks and Greatsword execution-gate layers.
  - [x] Stopped Echo now has separate Dual Blades broken-clock shards and Greatsword clock-cage layers.
  - [x] Ashen Echo now has separate Dual Blades parry-return streaks and Greatsword cathedral-wall pressure layers.
  - [x] Oblivion Echo now has separate Dual Blades shredded-void marks and Greatsword crater-teeth/collapse-line layers.
- [x] Performance guard completed:
  - [x] Dense Dual Blades skips the newest non-essential mutation ornaments.
  - [x] Dense Dual Blades perf reflection run stayed within the current VFX budget.
- [x] Verification completed:
  - [x] Runtime C# build passed.
  - [x] Editor C# build passed.
  - [x] Unity compilation errors `0`.
  - [x] Dual Blades and Greatsword mutation matrix captures saved.
  - [x] Dense Dual Blades perf reflection run completed.

Current next:

1. Direct-play weapon-specific Echo mutation effects in normal combat.
2. Mark each family/weapon pair `keep`, `tune`, or `redesign`.
3. Tune alpha/scale/lifetime for the all-on overpacked cases before adding additional gameplay logic.

## 2026-07-22 HQ Bitmap VFX Texture Pass

- [x] Reference-quality bitmap VFX pass completed:
  - [x] Blood Vortex generated close to the red/white circular slash reference.
  - [x] Stopped Clock generated with ornate clock, Roman numerals, gold frame, and frozen-glass accents.
  - [x] Execution Judgement generated as guillotine/verdict stamp art.
  - [x] Shatter Slam generated as a down-slam ground rupture.
  - [x] Oblivion Brand generated as a torn void/rune mark.
  - [x] Ashen Holy Fire generated as sacred ash flame/ward art.
- [x] Runtime integration completed:
  - [x] transparent sprites imported under `_dev/Art/Sprites`;
  - [x] source chroma images preserved under `_dev/Art/Source`;
  - [x] memory/Echo/Ultimate motif paths now prefer HQ bitmap sprites;
  - [x] procedural motif sprites remain as fallback/support layers;
  - [x] repeated Dual Blades Stopped clock scale/alpha tuned down after first capture review.
- [x] Verification completed:
  - [x] Runtime C# build passed.
  - [x] Editor C# build passed.
  - [x] Unity compilation errors `0`.
  - [x] Unity console errors `0`.
  - [x] Play Mode utility preview showed HQ sprites active.
  - [x] Dual Blades and Greatsword Echo Matrix captures saved.
  - [x] Dense Dual Blades perf reflection run completed without console errors.

Current next:

1. Direct-play individual HQ effects in normal combat.
2. Tune each family scale/alpha/lifetime from the player camera, especially all-on overpacked cases.
3. Keep the HQ bitmap motifs as the visual baseline unless jaewoo marks a family `redesign`.

## 2026-07-22 Procedural Motif VFX Rework

- [x] Approved silhouette-board direction implemented:
  - [x] Stopped = ornate clock seal / frozen second.
  - [x] Execution = judgement / guillotine / sentence stamp.
  - [x] Shatter = down-slam / ground crack / lifted debris.
  - [x] Oblivion = torn void brand / erased rune fragments.
  - [x] Ashen = holy ash fire / ward / guard-counter.
- [x] Runtime wiring completed:
  - [x] memory previews use motif call paths;
  - [x] normal memory and Echo paths use motif sprites as primary silhouettes;
  - [x] weapon-specific Greatsword and Dual Blades utility Echoes use the same family motifs at different scale/action density;
  - [x] utility Ultimate previews/support layers use motif language instead of only prompt rings.
- [x] Verification completed:
  - [x] Unity compilation errors `0`.
  - [x] Unity console errors `0`.
  - [x] `DebugPreviewAllUtilityVfx()` invoked successfully.
  - [x] `DebugRunEchoMatrix(DualBlades)` completed.
  - [x] `DebugRunEchoMatrix(Greatsword)` completed.
  - [x] Evidence screenshots copied to orchestration evidence.

Current next:

1. Direct-play the motif pass in normal combat, not only all-on debug matrices.
2. Mark Stopped / Execution / Shatter / Oblivion / Ashen as `keep`, `tune`, or `redesign`.
3. If noisy, tune scale, alpha, lifetime, and spawn count before adding new mechanics.

## 2026-07-22 Memory / Echo / Ultimate Dopamine Rework

- [x] Memory/Echo dopamine pass completed:
  - [x] Ashen memory now shows a cracked guard plate/halo.
  - [x] Ashen Echo now adds parry/counter burst layers for normal density.
  - [x] Oblivion memory +5 and Echo now add void-core, brand ring, cracks, and erase fragments.
  - [x] Blood Blade Storm opening/climax now add shock-ring, white-hot core, and blade-shard burst layers.
  - [x] Fracture Execution now adds a sentence/verdict ground mark.
  - [x] Stasis Hunt now adds a larger ultimate clock burst and second-hand snap.
  - [x] Ashen Oblivion now layers guard plate, ash wall, void break, and guard-collapse burst.
- [x] Dense performance recovery completed:
  - [x] First dense run failed after the dopamine additions.
  - [x] Dense Dual Blades now skips non-essential Ashen/Oblivion ornament layers.
  - [x] Dense Dual Blades Perf Matrix returned to PASS.
- [x] Verification completed:
  - [x] Runtime C# build passed.
  - [x] Editor C# build passed.
  - [x] Unity compilation errors `0`, console errors `0`.
  - [x] Echo Matrix Dual Blades PASS, `total=1028`.
  - [x] Echo Matrix Greatsword PASS, `total=991`.
  - [x] Passive Memory Matrix PASS.
  - [x] Utility Ultimate Matrix Dual Blades PASS.
  - [x] Utility Ultimate Matrix Greatsword PASS.
  - [x] Dense Dual Blades Perf Matrix PASS, `ms=98.03`.

Current next:

1. Direct-play the new Ashen, Oblivion, Blood Blade Storm, Fracture Execution, Stasis Hunt, and Ashen Oblivion reads.
2. Mark each effect `keep`, `tune`, or `redesign`.
3. If the screen feels too noisy, tune alpha/lifetime/counts before adding any new mechanics.

## 2026-07-22 Blood Repeat Fix / Remaining Echo VFX Plan

- [x] Blood visibility regression fixed:
  - [x] Greatsword Blood VFX appears on kill hits.
  - [x] Dead-target Greatsword Blood is VFX-only.
  - [x] Dense Dual Blades now shows a lightweight repeated Blood mark read.
  - [x] Removed the impossible `bloodLevel < 0` fallback branch.
- [x] Remaining VFX direction organized:
  - [x] Ashen = stored guard / cracked shield / counter wave.
  - [x] Oblivion = brand stamp / void spread / erase burst.
  - [x] Ultimate Echoes = later dopamine pass above normal Echo baseline.
  - [x] Concept board saved under orchestration evidence.
- [x] Verification completed:
  - [x] Runtime C# build passed.
  - [x] Editor C# build passed.
  - [x] Unity compilation errors `0`, console errors `0`.
  - [x] Echo Matrix Greatsword PASS, `B=303`.
  - [x] Echo Matrix Dual Blades PASS, `B=83`.
  - [x] Dense Dual Blades Perf Matrix PASS, `ms=91.56`.

Current next:

1. Direct-play Blood Echo and confirm it no longer feels one-shot.
2. Rework Ashen Echo as cracked shield/counter wave.
3. Rework Oblivion Echo as brand spread/erase.
4. Start Ultimate Echo dopamine pass after normal Echoes stabilize.

## 2026-07-21 Blood / Stopped Dopamine VFX Pass

- [x] Greatsword Blood Echo dopamine pass completed:
  - [x] Added white/red broken blood-vortex ring inspired by jaewoo's reference.
  - [x] Kept the blood-iaido slash stack but made the first read a circular blade/blood payoff.
  - [x] Increased Blood Echo hitstop and camera shake.
- [x] Stopped Echo 1-second clock pass completed:
  - [x] Freeze duration clamps to at least `1.0s`.
  - [x] Clock field, lock rings, ticks, and pin hold through the freeze window.
  - [x] Second hand rotates one full turn during the stop.
  - [x] Dual Blades Stopped now also shows the clock field.
- [x] Dense performance recovery completed:
  - [x] Reduced dense-only Kalmuri/Blood decorative extras.
  - [x] Dense perf QA now counts secondary hits as suppressed instead of replaying the damage path for every hit.
- [x] Verification completed:
  - [x] Runtime C# build passed.
  - [x] Editor C# build passed.
  - [x] Unity compilation errors `0`, console errors `0`.
  - [x] Dense Dual Blades Perf Matrix PASS, `ms=93.06`.
  - [x] Echo Matrix Dual Blades PASS, `St=160`.
  - [x] Echo Matrix Greatsword PASS, `B=87`, `St=168`.

Current next:

1. Direct-play Greatsword Blood and Stopped Echo to judge actual dopamine/readability.
2. Rework Ashen and Oblivion so remaining normal Echoes stop feeling like size/color variants.
3. After normal Echo baseline improves, design Ultimate Echoes with a higher dopamine ceiling than these normal Echoes.

## 2026-07-21 Shatter Echo Ground Fracture Rework

- [x] Shatter concept rework completed:
  - [x] Reframed Shatter as terrain/world fracture.
  - [x] Dual Blades Shatter now uses chained ground cracks under targets.
  - [x] Greatsword Shatter now uses a forward ground rupture with branch cracks and shards.
  - [x] Removed the old Greatsword Shatter ring/wedge/fan-like read.
  - [x] Added dense-only suppression for Shatter/Ashen identity burst/link extras.
  - [x] Dense perf matrix now clears transient debug VFX before setup.
- [x] Verification completed:
  - [x] Runtime C# build passed.
  - [x] Editor C# build passed.
  - [x] Unity compilation errors `0`.
  - [x] Dense Dual Blades Perf Matrix PASS, `ms=93.74`.
  - [x] Echo Matrix Dual Blades PASS, `Sh=175`.
  - [x] Echo Matrix Greatsword PASS, `Sh=144`.

Current next:

1. Direct-play Shatter with both weapons and judge whether it reads as ground fracture.
2. Start Ashen concept rework as stored guard/counter-pressure.
3. Rework Oblivion after Ashen as brand spread/erase.

## 2026-07-21 Stopped / Hunter Readability Finish

- [x] Dual Blades Stopped Echo visibility improved:
  - [x] Added clock field/lock outside dense throttle.
  - [x] Added second-hand sweep and clock ticks.
  - [x] Strengthened tick cut.
- [x] Greatsword Stopped Echo improved:
  - [x] Added larger second-hand sweep over the existing field.
  - [x] Kept judgement-hand read.
- [x] Hunter follow-up completed:
  - [x] Removed the Greatsword Hunter fan/cone sector.
  - [x] Increased Dual Blades Hunter blade size from `0.62` to `0.82`.
  - [x] Added immediate ricochet preview links/marks for readability and QA stability.
  - [x] Added dense-only preview/clockwork throttling after a perf regression.
- [x] Verification completed:
  - [x] Runtime C# build passed.
  - [x] Editor C# build passed.
  - [x] Unity compilation errors `0`, console errors `0`.
  - [x] Dense Dual Blades Perf Matrix PASS, `ms=91.05`.
  - [x] Echo Matrix Dual Blades PASS, `H=175`, `St=160`.
  - [x] Echo Matrix Greatsword PASS, `H=51`, `St=168`.

Current next:

1. Direct-play Stopped Echo with both weapons and judge whether the second-hand motion reads in the frozen-time window.
2. Direct-play Hunter Echo and judge whether the larger Dual Blades are readable enough.
3. Start the remaining concept rework in this order: Shatter, Ashen, Oblivion.

## 2026-07-21 Hunter Echo / Blood Readability Update

- [x] Greatsword Blood Echo readability pass completed:
  - [x] Increased Greatsword Blood Echo radius and target cap.
  - [x] Added stronger crescent stack, shadow crescent, impact zone, blood bloom, radial petals, and longer cut line.
  - [x] Increased Blood Echo hitstop/camera shake slightly.
- [x] Hunter Echo weapon mechanic rework completed:
  - [x] Dual Blades now throws two green ricochet blades.
  - [x] Ricochet bounce count scales by Echo level, with +5 reaching the highest bounce budget.
  - [x] Greatsword now throws a large green piercing greatsword line/area.
  - [x] Both variants preserve green tracking/Hunter lineage.
- [x] Verification completed:
  - [x] Runtime C# build passed.
  - [x] Editor C# build passed.
  - [x] Unity compilation errors `0`, console errors `0`.
  - [x] Echo Matrix Dual Blades PASS, `total=802`, `H=136`.
  - [x] Echo Matrix Greatsword PASS, `total=500`, `B=31`, `H=30`.
  - [x] Dense Dual Blades Perf Matrix PASS, `ms=87.70`.

Current next:

1. Direct-play Greatsword Blood, Dual Blades Hunter, and Greatsword Hunter.
2. Keep Stopped Second mechanics but plan a premium clockwork VFX pass.
3. Recheck Shatter, Ashen, and Oblivion from the memory concept upward before adding more size/color polish.

## 2026-07-21 Kalmuri Dual Blades Visibility Update

- [x] Dual Blades Kalmuri Echo visibility pass completed:
  - [x] Recolored the Dual Blades Kalmuri Hunger Echo branch from bright cyan/white into dark indigo, violet-blue, and blue-edge colors.
  - [x] Preserved the Greatsword Kalmuri branch.
  - [x] Increased Dual Blades Kalmuri core/pulse/bite lifetimes slightly so it reads as a separate memory-bite event.
  - [x] Adjusted `Weapon_DualBlades.asset` Kalmuri follow-up timing from `0.035/0.012` to `0.085/0.018`.
  - [x] `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed on the final rerun.
  - [x] `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo` passed.
  - [x] Unity compilation errors `0`, console errors `0`.
  - [x] Dense Dual Blades Perf Matrix PASS, `ms=87.49`.
  - [x] Kalmuri Perf Matrix PASS, `totalKalmuri=396`.
  - [x] Echo Matrix Dual Blades PASS, `total=803`.
  - [x] Echo Matrix Greatsword PASS, `total=499`.

Current next:

1. Direct-play Dual Blades + Hungry Blades/Kalmuri in normal and dense packs.
2. Judge whether the bright basic slash and darker Kalmuri bite now separate clearly.
3. If still weak, tune only one lever next: darker core, stronger violet edge, longer delay, or fewer dense-mode basic slash overlays.

## 2026-07-20 Update

- [x] `Dev_Prototype_v1` runtime targeting optimization added:
  - [x] per-frame living-enemy spatial hash grid in `V1GameManager`.
  - [x] reusable query buffers for weapon targeting, weapon hit collection, Echo radius/cone/chain helpers, Void Priest healing, enemy separation, and live enemy counting.
  - [x] cache invalidation on enemy spawn, kill, debug clear, gatekeeper removal, and cleanup.
  - [x] `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed with 7 existing warnings and 0 errors.
  - [x] `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo` passed with 0 warnings and 0 errors.
  - [x] `npm run report` and `npm.cmd run report:check` passed.
  - [x] Unity follow-up QA passed on 2026-07-21:
    - [x] Dense Dual Blades Perf Matrix PASS, `ms=43.11`.
    - [x] Echo Matrix Dual Blades PASS, `total=803`.
    - [x] Echo Matrix Greatsword PASS, `total=501`.

Current next:

1. Direct-play dense combat and check target-selection/separation feel after the spatial hash pass.
2. Continue the existing Echo/Kalmuri direct-play review gate.

## 2026-07-10 Update

- [x] LETHE project thumbnail/key visual added under `_dev/Art/Sprites/UI`.
- [x] In-game intro background key art added and wired into `Dev_Prototype_v1` weapon-selection intro.
- [x] Intro overlay panels were softened so the key art remains visible behind the title and weapon cards.
- [x] Game View evidence captured at `LETHE/Assets/_dev/Evidence/lethe_intro_keyart_screen_20260710.png`.
- [x] Unity compile, Unity console, and runtime C# build checks passed.

## Current Epic

`Dev_Prototype_v1 Core Prototype Complete`

목표:

```text
무기 2종
+ 기억 8종
+ 잔향 8종
+ 궁극 4종
+ 실제 플레이 M2 루프
+ 타격감/VFX/스프라이트 판독성
= HTML보다 명확한 Unity 코어 프로토타입
```

## Progress

- 2026-06-27 Update:
  - [x] Unity v1 베타 런 목표를 20분으로 정하고 1차 수치(`1260s`, 문지기 `300/600/900/1140s`, HP `1900/2800/4000/5400`, 시작 XP `7`)를 적용했다.
  - [x] 초반 XP 과속을 줄였다: 0~120초 `x1.00`, 120~600초 `x1.34`, 600초 이후 `x1.00`, 첫 120초 처치 XP 보너스 제거.
  - [x] 타이머 생존 클리어를 제거하고, 4번째 문지기 처치를 일반 클리어 조건으로 고정했다.
  - [x] `scripts/balance_sim_v1.js`로 4개 궁극 루트와 2개 무기 조합을 비교했다.
  - [x] 레벨업 보상 우선순위를 피의 칼폭풍 전용에서 4개 궁극 조합 공통으로 확장했다.
  - [x] `V1SmokeTestMenu`를 고정 지연 스냅샷에서 조건 기반 `[V1QA] PASS/FAIL` 검증으로 보강했다.
  - [x] 쌍검/대검 시작 QA는 실제 `elapsed >= 2.0`, 적 생성, 정상 `timeScale`, overlay 없음 조건을 확인한다.
  - [x] M2 QA는 칼무리/혈반 잔향 +5, 피의 칼폭풍 준비, result overlay, 적 생성을 확인한다.
  - [x] `LETHE/V1 QA/VFX Matrix`로 8기억/8잔향/3유틸궁극 프리뷰 생성을 확인한다.
  - [x] `LETHE/V1 QA/Blood Blade Storm`으로 피의 칼폭풍 준비뿐 아니라 실제 storm 오브젝트 생성을 확인한다.
  - [x] Unity MCP QA 결과: 쌍검, 대검, M2, VFX Matrix, Blood Blade Storm 모두 `[V1QA] PASS`.
  - [ ] jaewoo 직접 플레이에서 손맛, VFX 과밀도, 잔향 구분감, HUD 피로도를 판단한다.

- 2026-06-24 Update:
  - [x] Artificial arena-field read replaced with Lethe natural terrain direction: wet stone, mud, shallow water seams, roots, shard gravel, ash soil.
  - [x] Runtime outer marker rings removed in favor of marsh edges, water seams, drowned roots, and memory gravel.
  - [x] Release-prep map/background direction started after jaewoo said the map felt too small and the project should move beyond prototype feel.
  - [x] New Lethe stone floor tile set and arena backdrop generated under `_dev/Art/Sprites/Map`.
  - [x] Runtime arena expanded from prototype bounds `x +/-12`, `y -8.5..8.5` to `x +/-24`, `y +/-16`.
  - [x] Camera size and clamped follow updated for the larger arena.
  - [x] Enemy spawn radius expanded so combat has more room to breathe.
  - [ ] Direct jaewoo play review still needs to judge whether the larger map feels like a real game space rather than a small test box.

- 2026-06-23 Update:
  - [x] 적 3종과 첫 보스 판독용 스프라이트를 생성하고 런타임에 연결했다.
  - [x] 대검 slash VFX delay를 `0.18s`로 당겨 더 빠르게 읽히게 했다.
  - [x] 6개 유틸리티 기억/잔향 VFX의 scale, alpha, lifetime, secondary cue를 보강했다.
  - [x] `멈춘 1초`는 적 무리 중심의 시간 정지 초점과 시계바늘 VFX로 변경했다.
  - [x] `Mem A/B`, `Echo A/B`, `Ult 3`, `VFX` 디버그 버튼을 추가했다.
  - [x] 배경 1차 드레싱과 걷기 애니메이션 완화가 들어갔다.
- A. 데이터 구조 정리: partially done
- B. 공격/잔향 타격감 보정: working
- C. M2 실제 플레이 루프 연결: working
- D. 주요 스프라이트/VFX 교체: pending
- E. 기억 8종 확장: first runtime + data asset pass
- F. 잔향 8종 확장: first runtime + data asset pass
- G. 궁극 4종 구현: first runtime + data asset pass
- H. 전체 플레이 리뷰: pending
- I. 밸런스/가독성 1차 튜닝: pending
- J. 120초 초반 재미 루프: newly scoped

## A. 데이터 구조 정리

Done criteria:

- [x] `MemoryDefinition`이 카드 설명, 효과 종류, trigger, 수치, VFX/feedback 연결을 담는다.
- [x] `EchoDefinition`이 원본 기억과 다른 잔향 형태, 무기별 반응, +5 각성을 담는다.
- [x] `UltimateEchoDefinition` 또는 호환 구조가 4궁극 조건과 무기별 표현을 담는다.
- [x] `EnemyDefinition`이 role, stat, attack, XP, spawn cost를 담는다.
- [x] `EncounterDefinition`이 M2 실제 플레이 pacing을 담는다.
- [x] `RewardPoolDefinition`이 weapons/memories/echoes/ultimates/enemies/encounters를 묶는다.
- [x] `dotnet build LETHE/Assembly-CSharp.csproj --nologo`가 통과한다.
- [ ] Unity Editor에서 새 Definition 타입 import/compile 상태를 MCP로 확인한다.

## B. 공격/잔향 타격감 보정

Done criteria:

- [ ] 쌍검은 빠른 2연 반달 베기로 읽힌다.
- [ ] 대검은 느린 큰 반달 범위 참격으로 읽힌다.
- [ ] 적 피격 흰색 플래시, 데미지 숫자, 넉백, hitstop이 충분히 보인다.
- [x] 칼무리 잔향은 적 위치 후속타 링/반달로 읽히는 1차 보정이 들어갔다.
- [x] 굶주린 칼무리 활성 기억은 이중 궤도 칼날 군집과 타깃 물어뜯기 VFX로 1차 재보정했다.
- [x] 혈반 잔향은 표식 -> 회복 실 -> 피꽃 흐름의 1차 보정이 들어갔다.
- [x] transient VFX, floating text, damage number, XP orb는 풀링으로 재사용한다.
- [ ] Unity Play Mode에서 칼무리 칼날 군집, 칼무리 잔향, 혈반 실이 실제로 방해 없이 보이는지 사람이 확인한다.

## C. M2 실제 플레이 루프 연결

Done criteria:

- [ ] 디버그 없이 XP -> 카드 -> 기억 강화 -> 망각 -> 잔향 -> 공명 -> +5 -> 궁극 흐름에 도달한다.
- [x] 일반 런은 Unity 베타 기준 1260초 하드 캡, 300/600/900/1140초 문지기 스케줄, 54초 결손 생존을 사용한다.
- [x] 압축 smoke/debug 루프는 fast timing으로 분리한다.
- [x] 압박 페이즈 spawn interval/pack/cap이 `LETHE_DESIGN_01_RUN_LOOP.md` 기준으로 1차 연결됐다.
- [x] 레벨업 선택지는 문서 기준 6런스탯을 모두 포함한다.
- [ ] 일반 플레이에서 XP -> 카드 -> 기억 강화 -> 첫 문지기 -> 망각 -> 결손 -> 보충/공명까지 밸런스가 맞는다.
- [ ] 60~120초 압축 smoke 또는 6~10분 일반 런에서 핵심 감정 루프가 보인다.
- [x] 최고 레벨 기억이 다음 망각 후보라는 점이 HUD에서 보인다.
- [x] HUD에 M2 현재 목표/결손 생존/공명 대기/궁극 준비 상태가 보인다.
- [x] 레벨업 카드로 세 번째 기억 `멈춘 1초`를 선택해 기억 3칸을 채울 수 있다.
- [x] 자동 리뷰 보정 없이도 +5/궁극까지 닿도록 20분 베타 pacing 1차 수치를 적용했다.

## D. 주요 스프라이트/VFX 교체

Done criteria:

- [ ] 플레이어 4방향 idle/move 판독 이미지가 들어간다.
- [ ] 쌍검/대검 무기 이미지가 캐릭터와 분리되어 보인다.
- [x] 기본 적 2~3종이 역할별로 구분된다.
- [ ] 칼무리/혈반 기억과 잔향 VFX가 서로 다른 형태로 보인다.
- [ ] 피의 칼폭풍 VFX가 무기별로 다르게 보인다.

## E. 기억 8종 확장

Done criteria:

- [x] 8개 `MemoryDefinition` asset이 존재한다.
- [x] 8개 활성 기억이 서로 다른 최소 플레이 감각을 가진다.
- [ ] 카드 설명과 실제 효과가 일치한다.

## F. 잔향 8종 확장

Done criteria:

- [x] 8개 `EchoDefinition` asset이 존재한다.
- [x] 8개 잔향의 최소 무기 타격 반응이 런타임에 존재한다.
- [ ] 잔향은 약한 복사본이 아니라 형태가 바뀐 효과로 보인다.
- [ ] +1~+5 성장과 +5 각성 표시가 있다.
- [ ] 무기별 잔향 표현 차이가 있다.

## G. 궁극 4종 구현

Done criteria:

- [x] 4개 궁극 definition이 존재한다.
- [x] 피의 칼폭풍은 칼무리 +5 + 혈반 +5로 발동한다.
- [x] 파쇄 처형, 정지 추적, 잿빛 망각의 최소 궁극 런타임이 존재한다.
- [ ] 궁극은 무기별로 다른 패턴을 가진다.
- [x] HUD에 궁극 준비/발동 상태가 보인다.

## H. 전체 플레이 리뷰

Done criteria:

- [ ] jaewoo가 `GO`, `ITERATE`, `NO-GO` 중 하나를 줄 수 있다.
- [ ] 피드백은 1~3개 핵심 문제로 압축된다.

## I. 밸런스/가독성 1차 튜닝

Done criteria:

- [x] XP curve, 문지기 스케줄/HP, 클리어 조건, 궁극 루트 보상 우선순위가 1차 튜닝된다.
- [x] `scripts/balance_sim_v1.js`와 evidence 문서로 4개 루트/2개 무기 수치 근거가 남는다.
- [ ] MCP QA와 직접 플레이에서 대검 루트, 4궁극 루트 편차, 20분 체감 템포를 확인한다.
- [ ] `docs/TEST.md`, `docs/CHANGELOG.md`, orchestration report가 갱신된다.

## J. 120초 초반 재미 루프

목표: 뱀서류의 즉각적 재미를 먼저 확보한 뒤 LETHE의 망각/잔향 차별점을 얹는다. 이 단계는 콘텐츠 추가가 아니라 `초반 손맛 -> 빌드 선택 -> 첫 망각 예고 -> 잔향 기대`가 2분 안에 읽히는지 확인하는 패스다.

구현 리스트:

- [x] 시작 화면에서는 무기만 고른다: `절단쌍검`, `장송대검`.
- [x] 시작 기억은 무기 카드에 붙이지 않고 첫 레벨업 보상에서 고른다.
- [x] 첫 레벨업 카드에서 빠르게 코어 조합(`굶주린 칼무리`, `피의 반사`)을 채울 수 있다.
- [x] 첫 20~30초 안에 첫 레벨업 또는 강한 보상 선택이 온다.
- [x] 60~90초 안에 활성 기억 2~3개가 화면에서 돌아가고, 최고 레벨 기억이 다음 망각 후보로 보인다.
- [ ] 첫 망각 전에는 선택한 시작 기억을 충분히 써볼 시간이 있다.
- [x] 망각 결과 화면은 손실과 보상을 동시에 말한다: `사라진 기억`, `남은 잔향`, `다음 행동`.
- [ ] 120초 압축 리뷰에서는 디버그 자동 완성 없이도 `기억 강화 -> 망각 예고 -> 잔향 기대`가 보인다.

Done criteria:

- [ ] jaewoo가 시작 무기 카드만 보고 “이번 판 기본 공격 방향”을 이해한다.
- [ ] 첫 120초에서 지루한 공백 없이 XP, 카드, 전투 피드백, 다음 망각 후보가 순환한다.
- [ ] 망각 전 기억 상실이 최소한 “뭘 잃는지 알겠다” 수준까지 도달한다.
- [ ] 120초 리뷰 결과에 따라 다음 패스를 `공격 손맛`, `보상 카드`, `망각 UX`, `스폰 압박` 중 하나로 좁힌다.

## K. Visual/UI/game-feel refresh

Goal: make `Dev_Prototype_v1` read more like an actual survivor game shell before deeper balance review.

Implementation list:

- [x] Stop player body wobble caused by runtime scale pulse.
- [x] Keep player root movement stable and move animation onto a child `PlayerVisual`.
- [x] Generate/import a new `sheet_player_v1_4dir.png` player body sheet.
- [x] Use the new 8x4 player sheet as real idle/walk 4-direction animation instead of a single static frame.
- [x] Center the weapon anchor so movement does not make the body feel offset by the weapon.
- [x] Add a dedicated transparent greatsword sprite asset and load it before procedural fallback.
- [x] Add per-tile rotation, color variation, and tiny scale variation to the arena floor.
- [x] Compact the HUD into a cleaner survivor-style status panel with HP, XP, memory slots, ultimate status, and smaller debug controls.
- [x] Add a v1 content catalog for runtime sprites/font/weapon definitions and wire it to `Dev_Prototype_v1`.
- [x] Create `Assets/Lethe` promotion-prep folders plus a `Lethe_BetaPreview` scene copy.
- [x] Add current echo summary and a short player objective line to the HUD while keeping F12 debug review UI.
- [ ] Replace or slice/import the full player sheet through Unity Sprite Editor instead of runtime cropping.
- [ ] Capture a reliable visual screenshot path that includes camera objects and relevant UI overlays.

Verification:

- Beta-play prep verification: `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed with 7 legacy warnings and 0 errors; Unity compile error count `0`; scene missing references `0`; Play Mode start snapshot valid; console error count `0`.
- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
- Unity MCP `Assets/Refresh`: success.
- Unity compile error count: 0.
- Short Unity Play Mode entry after the new player sheet: success.
- Unity console error count after smoke: 0.
- Unity missing references: scene 0, assets 0.
- Greatsword sprite asset loaded as `spr_weapon_greatsword_01`.
- Play Mode Greatsword smoke snapshot reached `scene=v1`, `weapon=장송대검`, `elapsed=1.8`, `hp=210/210`, `enemies=6`, `death=False`.
- Camera screenshot attempt saved a solid-color image and was discarded; it is not visual evidence.

## Current Next

1. jaewoo가 `Dev_Prototype_v1`에서 2개 시작 무기를 각각 짧게 확인한다.
2. 첫 120초 기준으로 가장 약한 문제를 하나만 고른다: 보상 속도, 공격 손맛, 망각 UX, 스폰 압박, 적/기억 판독성.
3. 피드백 후 해당 한 축만 추가 튜닝한다.

## 2026-06-22 Update

- [x] Generated weapon/hit, six utility memory, six utility echo, and three utility ultimate PNG sprites are wired into `V1GameManager` runtime spawn paths with procedural fallback.
- [x] 1254px generated VFX sprites are normalized to existing combat world-size targets so they should not cover the whole field.
- [x] Blood Blade Storm has been separated from Kalmuri feel with opening cue, continuous storm pressure, periodic burst pulses, heal threads, stronger knockback, and weapon-specific dual/greatsword cadence.
- [x] Player walking now uses short acceleration/deceleration smoothing, smoothed movement-facing weapon rotation, and subtle `PlayerVisual` bob/tilt.
- [x] Held weapon size pass: dual blades are larger/closer to the body, greatsword is smaller/less screen-dominant, and generated attack slash scale was rebalanced.
- [x] Direct greatsword play check found the sword covering the player; greatsword is now smaller, behind the player, side-shifted, and its cleave VFX is reduced.
- [x] Player-attached weapon sprites are now hidden during normal play; dual blades and greatsword appear as short hit-point phantom strikes aligned with slash VFX.
- [x] Phantom weapon timing pass: weapon sweep appears before slash/spark/confirm VFX, and weapon slash VFX now lasts longer for readability.
- [ ] Direct Play Mode review confirms generated VFX scale, alpha, duration, and spawn frequency feel natural during the first 120 seconds.

## 2026-06-23 Update

- [x] Greatsword phantom attack now uses a blade-tip-first calculation: the tip travels through the hit/VFX point, and the handle stays closer to the player body.
- [x] Greatsword slash VFX now uses a tip-aligned anchor so the crescent appears at the sword tip rather than the weapon center.
- [x] Unity Play Mode geometry check confirmed `handleCloser=True` and slash-tip alignment distance `0.000`.
- [x] Greatsword phantom attack now rotates around a handle pivot instead of sliding between two positions.
- [x] Greatsword crescent/fan VFX now uses the sweep end blade direction with a `180` degree correction so it faces with the sword.
- [x] Unity Play Mode pivot check confirmed `usePivot=True`, blade sweep `-28.0 -> 28.0`, and slash tip error `0.000`.
- [x] Greatsword spectacle pass: sweep widened to `90` degrees and weapon-hit VFX scale/lifetime increased.
- [x] Unity Play Mode spectacle check confirmed blade sweep `-45.0 -> 45.0`, Primary slash bounds `(4.28, 4.28)`, and tip error `0.000`.
- [x] Greatsword timing/coverage pass: slash VFX now appears at `78.6%` of the sweep, lasts longer, and places AoE/Primary/Assist along different points of the 90-degree tip arc.
- [x] Greatsword slash timing tighten: VFX delay pulled from `0.22s` to `0.20s`, so it appears around `71.4%` of the sweep.
- [x] Dual-blade follow-up uses the same principle in a smaller form: A slash, cut flash, and B slash are staggered at `0.045s / 0.067s / 0.085s`.
- [x] Blood Blade Storm payoff follow-up: stronger opening cue, larger/faster bursts, more pressure damage, heal, hitstop, and camera shake.
- [x] First-120 flow follow-up: faster opening spawn cadence, higher early cap, and early XP multiplier `2.15`.
- [ ] Jaewoo direct review confirms the greatsword sweep angle, tip VFX placement, and hit readability feel natural in real play.

Current next:

1. jaewoo reviews both starting weapons in `Dev_Prototype_v1`.
2. Check whether Blood Blade Storm now feels like a true +5/+5 payoff instead of only a Kalmuri color swap.
3. Check whether hit-point phantom weapons now visibly sweep before slash VFX and clearly show which enemy was attacked.
4. Check whether player walking now feels less stiff during diagonal movement, stop/start, and direction changes.
5. Pick one weak axis for the next pass: reward cadence, attack feel, forgetting UX, spawn pressure, enemy/memory readability, or VFX scale/timing.
