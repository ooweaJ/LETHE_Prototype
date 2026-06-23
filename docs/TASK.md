# LETHE TASK

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
- [x] 일반 런은 문서 기준 600초/180-340-490-600초 문지기 스케줄과 54초 결손 생존을 사용한다.
- [x] 압축 smoke/debug 루프는 fast timing으로 분리한다.
- [x] 압박 페이즈 spawn interval/pack/cap이 `LETHE_DESIGN_01_RUN_LOOP.md` 기준으로 1차 연결됐다.
- [x] 레벨업 선택지는 문서 기준 6런스탯을 모두 포함한다.
- [ ] 일반 플레이에서 XP -> 카드 -> 기억 강화 -> 첫 문지기 -> 망각 -> 결손 -> 보충/공명까지 밸런스가 맞는다.
- [ ] 60~120초 압축 smoke 또는 6~10분 일반 런에서 핵심 감정 루프가 보인다.
- [x] 최고 레벨 기억이 다음 망각 후보라는 점이 HUD에서 보인다.
- [x] HUD에 M2 현재 목표/결손 생존/공명 대기/궁극 준비 상태가 보인다.
- [x] 레벨업 카드로 세 번째 기억 `멈춘 1초`를 선택해 기억 3칸을 채울 수 있다.
- [ ] 자동 리뷰 보정 없이도 +5/궁극까지 닿는 실제 pacing을 완성한다.

## D. 주요 스프라이트/VFX 교체

Done criteria:

- [ ] 플레이어 4방향 idle/move 판독 이미지가 들어간다.
- [ ] 쌍검/대검 무기 이미지가 캐릭터와 분리되어 보인다.
- [ ] 기본 적 2~3종이 역할별로 구분된다.
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

- [ ] 무기 DPS, 적 체력/속도/spawn, XP curve, 망각 타이밍, 잔향 proc, 궁극 조건이 1차 튜닝된다.
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
- [ ] Replace or slice/import the full player sheet through Unity Sprite Editor instead of runtime cropping.
- [ ] Capture a reliable visual screenshot path that includes camera objects and relevant UI overlays.

Verification:

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
