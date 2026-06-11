# LETHE Unity Complete Prototype PRD

최종 갱신: 2026-06-11

## 0. 문서 역할

이 문서는 `Dev_Prototype_v0` 이후 LETHE Unity 프로토타입의 상위 실행 PRD다.

이전 `LETHE_UNITY_PROTOTYPE_V0_PRD.md`는 `절단쌍검 + 칼무리 + 피의 반사` 중심의 첫 playable prototype 기준이었다. 하지만 현재 판단은 다르다.

```text
현재 결론:
쌍검 1종과 기억 2종만으로는 LETHE의 기준을 잡을 수 없다.
대검, 8개 기억, 8개 잔향, 4개 궁극 잔향을 모두 테스트 가능한 구조로 올려야
기억/잔향 시스템의 진짜 방향을 판단할 수 있다.
```

따라서 이 문서가 이후 Unity 구현의 최신 기준이다.

참고 문서:

- `LETHE_WEAPON_MEMORY_ECHO_DETAIL.md`
- `LETHE_ECHO_FORM_TRANSFORMATION_SPEC.md`
- `LETHE_UNITY_ECHO_SYSTEM_PRD.md`
- `LETHE_CONTENT_TABLES.md`
- `LETHE_RUN_STRUCTURE.md`
- `LETHE_COMBAT_DESIGN.md`
- `LETHE_BALANCE_BASELINE.md`

## 1. 제품 비전

LETHE는 단순히 적을 많이 죽이는 뱀서류가 아니다.

핵심 재미는 아래 순서에서 나온다.

```text
기억으로 강해진다
-> 가장 키운 기억을 잃는다
-> 잃은 기억이 잔향으로 형태를 바꾼다
-> 잔향 조합이 새 빌드 목표가 된다
-> 잃었던 기억을 다시 얻으면 공명한다
```

Unity Complete Prototype은 이 루프를 모든 핵심 콘텐츠 축에서 검증해야 한다.

## 2. 현재 문제 정의

최근 플레이 피드백:

- 쌍검만으로는 기준이 잡히지 않는다.
- 칼무리와 피의 반사가 무엇을 하는지 화면에서 잘 안 보인다.
- 기본공격이 강하면 기억/잔향의 정체성이 묻힌다.
- 잔향이 캐릭터 주변 선 몇 개로 보이면 거슬리고 최종 게임처럼 느껴지지 않는다.
- 시스템 자체는 재미 방향이 보인다.
- 그래서 더 좁은 slice가 아니라 전체 기획 기준을 확고히 해야 한다.

이 PRD의 목적은 "작게 만든 후 판단"이 아니라, **전체 시스템의 기준선을 충분히 구현한 뒤 무엇이 재미있는지 판단**하는 것이다.

## 3. 대상 빌드

메인 씬:

```text
Assets/_dev/Scenes/Dev_Prototype_v0.unity
```

작업 루트:

```text
Assets/_dev/
```

승격 조건:

```text
Assets/_dev에서 Complete Prototype이 GO 판정을 받은 뒤에만 Assets/Lethe로 승격한다.
```

## 4. Complete Prototype 범위

### 4.1 무기

Complete Prototype은 무기 2종을 포함한다.

| ID | 이름 | 역할 | 전투 리듬 |
| --- | --- | --- | --- |
| `Weapon_DualBlades` | 절단쌍검 | 빠른 연타, 잔향 proc, 주변 압박 | 작게 자주 |
| `Weapon_Greatsword` | 장송대검 | 느린 강타, 처형, 범위 제어 | 크게 조건부 |

무기 원칙:

- 기본공격은 빌드 정체성을 빼앗으면 안 된다.
- 쌍검은 온힛과 잔향 빈도 검증용이다.
- 대검은 조건부 폭발, 강타, 처형, 파문, 낙인 검증용이다.
- 같은 기억/잔향도 무기에 따라 화면 형태와 수치 리듬이 달라야 한다.

### 4.2 기억 / 잔향

Complete Prototype은 8개 기억과 matching echo 8개를 모두 가진다.

| 기억 ID | 기억 이름 | 활성 기억 역할 | 잔향 ID | 잔향 역할 |
| --- | --- | --- | --- | --- |
| `Memory_HungryBlades` | 굶주린 칼무리 | 주변 독립 칼날 고리 | `Echo_Kalmuri` | 무기 공격에 남는 칼자국/각성 칼날 |
| `Memory_BloodReflection` | 피의 반사 | 붉은 표식, 회복, 피의 추가타 | `Echo_Blood` | 혈반, 회복 실, 피꽃 |
| `Memory_ExecutionFlash` | 처형자의 섬광 | 저체력/위협 적 처형 보조 | `Echo_Execution` | 처형 조건 폭발, 하얀 균열 |
| `Memory_HunterOath` | 추적자의 맹세 | 원거리/약한 적 추적 | `Echo_Homing` | 유도 잔탄, 그림자 추적 |
| `Memory_ShatterWave` | 파쇄의 파문 | 충격파, 포위 해제 | `Echo_Shockwave` | 타격 위치 파문, 넉백, 안전지대 |
| `Memory_StoppedSecond` | 멈춘 초침 | 둔화, 시간 제어 | `Echo_TimeStop` | 둔화, 시간 균열 |
| `Memory_AshenShield` | 잿빛 보호막 | 방어, 피격 후 반격 | `Echo_AshenGuard` | 피격/보호막 파괴 반응 |
| `Memory_OblivionBrand` | 망각의 낙인 | 취약 표식, 후속타 증폭 | `Echo_Brand` | 낙인, 증폭, 표식 전파 |

기억 원칙:

- 활성 기억은 플레이어 주변/전장에 독립적으로 존재한다.
- 잔향은 활성 기억의 약화판이 아니라, 잃은 기억이 무기/몸에 남긴 다른 형태다.
- 모든 기억은 최소 레벨 1에서도 화면에서 정체성이 읽혀야 한다.
- 모든 잔향은 +1, +3, +5가 행동 차이로 구분되어야 한다.

### 4.3 궁극 잔향

Complete Prototype은 4개 궁극 잔향을 가진다.

| 필요 조건 | 궁극 ID | 이름 | 핵심 루프 |
| --- | --- | --- | --- |
| `Echo_Kalmuri +5` + `Echo_Blood +5` | `Synergy_BloodBladeStorm` | 피의 칼폭풍 | 칼날이 혈반을 묻히고 혈반이 회복 실을 돌려준다 |
| `Echo_Execution +5` + `Echo_Brand +5` | `Synergy_ExecutionBrand` | 처형 각인 | 낙인 적 처형 시 하얀 폭발과 검은 파편이 연쇄된다 |
| `Echo_Homing +5` + `Echo_TimeStop +5` | `Synergy_FrozenHunt` | 정지 추적 | 둔화 적을 추적 잔탄이 우선 공격하고 분열한다 |
| `Echo_Shockwave +5` + `Echo_AshenGuard +5` | `Synergy_BastionWave` | 성채 파문 | 방어 반응이 큰 파문과 안전지대를 만든다 |

궁극 원칙:

- 단순 1+1 강화가 아니다.
- 두 잔향이 서로를 먹여 살리는 루프여야 한다.
- 텍스트 없이 화면/SFX/피드백으로 "궁극이 켜졌다"가 보여야 한다.

## 5. 무기별 표현 규칙

### 5.1 절단쌍검

쌍검은 빠르고 잦은 발동을 담당한다.

공통:

- 기본 공격은 약한 베이스.
- 기억/잔향이 damage와 화면 장악력을 가져간다.
- 온힛 계열 잔향의 빈도와 연쇄를 검증한다.

기억/잔향 표현:

- 칼무리: 작고 빠른 칼날, 주변 압박.
- 혈반: 잦은 붉은 표식과 짧은 회복 실.
- 추적: 작은 잔탄 다수.
- 파문: 작은 파문이 자주 포위를 흐트러뜨림.
- 정지: 얇은 둔화를 여러 적에게 묻힘.
- 처형: 작은 섬광이 자주 발생.
- 잿빛: 피격 후 짧은 반격/공속 리듬.
- 낙인: 빠르게 표식을 터뜨림.

### 5.2 장송대검

대검은 느리고 큰 조건부 효과를 담당한다.

공통:

- 기본 공격은 느리지만 넓고 무겁다.
- 강타/처형/파문/낙인 계열을 검증한다.
- 잔향은 "자주"보다 "크게, 조건부로" 보여야 한다.

기억/잔향 표현:

- 칼무리: 큰 관통 칼날, 느린 회전.
- 혈반: 한 번의 피꽃과 회복량이 큼.
- 추적: 큰 잔탄이 관통하거나 오래 추적.
- 파문: 강타 지점 큰 파문.
- 정지: 맞은 적 주변 시간 장판.
- 처형: 한 방 처형 폭발.
- 잿빛: 피격 후 다음 강타에 잿빛 파문.
- 낙인: 표식 폭발이 큼.

## 6. 게임 루프

Complete Prototype의 한 세션 목표 길이:

```text
6~8분
```

압축 테스트 모드:

```text
60~120초 안에 무기 변경, 기억 선택, 망각, 잔향 +5, 궁극을 강제로 검증 가능
```

기본 흐름:

1. 무기 선택: 쌍검 또는 대검.
2. 첫 기억 선택: 8개 중 2~3개 후보.
3. 초반 사냥: 활성 기억으로 사냥 정체성 확인.
4. 강화: 기억 레벨 증가.
5. 첫 망각: 최고 레벨 기억 상실.
6. 잔향 획득: matching echo 생성.
7. 결손 생존: 잃은 기억의 빈자리와 잔향의 새 형태 체감.
8. 보충 선택: 재획득/새 기억/기존 강화.
9. 공명: 잔향이 있는 기억 재획득 시 강화.
10. 잔향 +5 목표: 각성 잔향 확보.
11. 궁극 조합 목표: 4개 궁극 중 하나 확인.

## 7. 시스템 요구사항

### 7.1 데이터 구조

필수 Definition:

```text
WeaponDefinition
MemoryDefinition
EchoDefinition
EchoLevelData
EchoSynergyDefinition
FeedbackProfile
EnemyDefinition
RewardPoolDefinition
```

필수 Runtime:

```text
WeaponRuntimeBase
ActiveMemoryRuntimeBase
EchoRuntimeBase
UltimateEchoRuntimeBase
EnemyRuntimeBase
PooledBehaviour
```

필수 Service:

```text
RunBuildState
MemoryInventory
EchoInventory
ForgetService
ResonanceService
UltimateEchoService
HitResolver
EchoTriggerRouter
EchoProcLimiter
PoolService
FeedbackService
RewardService
DebugStateInjector
```

규칙:

- `PrototypeGameManager`가 모든 기억/잔향을 직접 분기하는 구조는 폐기 대상이다.
- 신규 기억/잔향 추가 시 `HitResolver`, `ForgetService`, `EchoInventory`를 수정하지 않아야 한다.
- 새 트리거 패밀리가 필요할 때만 core event layer를 확장한다.

### 7.2 트리거 패밀리

| TriggerFamily | 용도 | 대표 잔향 |
| --- | --- | --- |
| `OnWeaponHit` | 무기 타격 후 발동 | 칼무리, 혈반, 추적, 파문, 정지, 낙인 |
| `OnKill` | 처치/처형 후 발동 | 처형, 낙인 전파, 칼무리 가속 |
| `OnDamageTaken` | 피격 후 발동 | 잿빛 |
| `OnShieldBreak` | 보호막 파괴 후 발동 | 잿빛 각성, 성채 파문 |
| `Periodic` | 주기적 독립 효과 | 활성 칼무리, 활성 혈반, 정지장 |

무한 루프 제한:

- `WeaponHit`은 일반 잔향을 발동할 수 있다.
- `EchoHit`은 일반 온힛 잔향을 다시 발동할 수 없다.
- `UltimateHit`은 궁극 전용 전파/회복만 허용한다.
- 한 프레임 내 전파/처형/회복은 target cap과 recursion cap을 가진다.

## 8. 콘텐츠 상세 요구사항

### 8.1 8기억 수락 기준

각 기억은 아래를 만족해야 한다.

- 레벨 1에서 역할이 보인다.
- 레벨 3에서 범위/대상/지속 중 하나가 확장된다.
- 레벨 5에서 행동 자체가 바뀐다.
- 쌍검과 대검에서 최소 하나 이상의 표현 차이가 있다.
- 잔향으로 바뀌었을 때 활성 기억과 화면 형태가 다르다.

### 8.2 8잔향 수락 기준

각 잔향은 아래를 만족해야 한다.

- +1: 작은 형태 변화.
- +3: 다중 대상, linger, 전파, 조건 강화 중 하나.
- +5: 각성 행동.
- damage/log가 실제 전투에 반영된다.
- HUD에 level과 awakened 상태가 보인다.
- debug panel에서 +1/+3/+5 상태로 즉시 전환할 수 있다.

### 8.3 4궁극 수락 기준

각 궁극은 아래를 만족해야 한다.

- 두 required echo가 +5일 때 unlock된다.
- 단순 damage aura가 아니라 두 잔향의 상호작용 루프를 가진다.
- 10초 debug showcase에서 화면 변화가 명확하다.
- 회복량/처치/전파는 cap을 가진다.

## 9. 적 / 전장 요구사항

Complete Prototype은 최소 4종 적을 가진다.

| Enemy ID | 이름 | 역할 | 검증 대상 |
| --- | --- | --- | --- |
| `Enemy_MeleeChaser` | 침식자 | 기본 압박 | 일반 사냥, 칼무리 |
| `Enemy_RangedEye` | 떠도는 눈 | 원거리 압박 | 추적, 정지 |
| `Enemy_Splitter` | 쪼개진 자 | 분열/군집 | 파문, 피의 칼폭풍 |
| `Enemy_EliteGatekeeper` | 문지기 | 빌드 시험 | 처형, 낙인, 대검 |

적 원칙:

- 적이 모두 같은 근접 추격이면 기억 차이가 안 보인다.
- 원거리 적은 계속 도망치면 안 된다.
- 엘리트는 체력만 높은 적이 아니라, 표식/처형/대검의 가치를 보여야 한다.

## 10. UI / Debug 요구사항

### 10.1 HUD

필수:

- HP bar.
- 현재 무기.
- active memory slots.
- echo slots.
- next forget candidate.
- memory protection.
- current ultimate goal.
- debug state badge.

### 10.2 Debug Panel

필수 버튼:

- 무기 변경: 쌍검 / 대검.
- 기억 추가: 8종.
- 기억 강화: 선택 기억 +1.
- 자동 망각 실행.
- 잔향 level set: +1 / +3 / +5.
- 궁극 조건 만들기: 4종.
- 60초 balance smoke 시작.

Debug Panel은 시스템 로직을 직접 들고 있지 않고, `DebugStateInjector`로 상태만 주입한다.

## 11. 아트 / VFX 기준

현재 생성 스프라이트는 prototype art다. 하지만 Complete Prototype에서는 최소한 아래 판독 기준을 만족해야 한다.

필수:

- player 4방향 idle/walk.
- enemy 4방향 idle/walk.
- 쌍검/대검 weapon sprite 분리.
- 8기억 active VFX icon/sprite.
- 8잔향 VFX sprite.
- 4궁극 VFX sprite.

우선순위:

1. 기억/잔향 판독 sprite.
2. weapon sprite.
3. enemy role sprite.
4. player sheet polish.
5. 궁극 VFX polish.

원칙:

- 스프라이트가 덜 예뻐도 역할이 읽히면 통과 가능.
- 역할이 안 읽히면 아무리 예뻐도 실패다.
- line renderer는 임시 보조만 허용한다.
- 최종 형태는 sprite/trail/particle/pool 기반으로 간다.

## 12. Milestones

### C0. PRD / 데이터 계약 고정

완료 기준:

- 이 PRD가 source of truth로 등록된다.
- 무기 2종, 기억 8종, 잔향 8종, 궁극 4종 id가 고정된다.
- `_dev` 구현 범위가 쌍검-only에서 complete prototype으로 변경된다.

### C1. Data-Driven Core

목표:

- `PrototypeGameManager`에 몰린 분기를 분리한다.

완료 기준:

- `RunBuildState`, `MemoryInventory`, `EchoInventory`, `ForgetService`, `ResonanceService`, `UltimateEchoService` 존재.
- Definition asset으로 무기/기억/잔향/궁극을 등록한다.
- debug panel은 state injection만 한다.

### C2. Weapon Pair

목표:

- 쌍검과 대검을 모두 플레이 가능하게 만든다.

완료 기준:

- 쌍검은 빠른 proc형.
- 대검은 느린 강타형.
- 같은 적 무리에서 두 무기의 리듬 차이가 보인다.
- 무기 변경 debug button.

### C3. Active Memories 8

목표:

- 8개 활성 기억이 모두 사냥 정체성을 가진다.

완료 기준:

- 각 기억 level 1 동작 구현.
- 각 기억 level 3 확장 구현.
- 각 기억 level 5 행동 변화 구현.
- 쌍검/대검 중 최소 하나에서 차별 표현 확인.

### C4. Echoes 8

목표:

- 8개 잔향이 활성 기억과 다른 형태로 작동한다.

완료 기준:

- +1/+3/+5 단계 구분.
- trigger family별 runtime 동작.
- echo hit loop 제한.
- HUD level 표시.

### C5. Ultimate Echoes 4

목표:

- 4개 궁극 잔향 조합이 모두 debug showcase 가능하다.

완료 기준:

- 피의 칼폭풍.
- 처형 각인.
- 정지 추적.
- 성채 파문.
- 각 궁극의 조건/발동/VFX/cap 확인.

### C6. Enemies / Encounter

목표:

- 기억/잔향 차이가 보이는 적 구성을 만든다.

완료 기준:

- 근접, 원거리, 분열, 엘리트 적.
- 60초 smoke에서 각 기억군의 가치가 최소 한 번 드러난다.

### C7. Visual / Feedback Pass

목표:

- 시스템이 텍스트가 아니라 화면으로 읽힌다.

완료 기준:

- 기억/잔향 line renderer 의존 감소.
- sprite/trail/particle/pool 기반 VFX.
- hitstop, flash, shake, SFX hook 준비.

### C8. Complete Prototype Gate

목표:

- 6~8분 플레이 또는 debug 압축 루프로 전체 시스템을 판단한다.

완료 기준:

- 두 무기 모두 플레이 가능.
- 8기억/8잔향 모두 debug test 가능.
- 4궁극 모두 showcase 가능.
- Unity compile/console/missing refs 0.
- jaewoo hands-on review에서 `GO / ITERATE / NO-GO` 결정.

## 13. Acceptance Tests

### 13.1 Human Review Questions

jaewoo가 직접 확인한다.

- 쌍검과 대검의 리듬 차이가 느껴지는가?
- 8기억이 서로 다른 빌드 방향으로 보이는가?
- 잔향이 약화판이 아니라 형태 변화로 보이는가?
- 가장 키운 기억을 잃는 것이 아쉬운가?
- 공명은 다시 얻는 설렘이 있는가?
- 4궁극 중 최소 하나가 목표로 삼고 싶을 만큼 강렬한가?
- 기본공격이 기억/잔향 정체성을 묻지 않는가?
- 스프라이트가 부족해도 역할 판독은 되는가?

### 13.2 Automated / MCP Checks

각 milestone마다:

- Unity compilation errors: `0`.
- Unity console errors: `0`.
- scene missing references: `0`.
- Play Mode smoke.
- report generation pass.
- report check pass.

### 13.3 Balance Smoke

60초 smoke 최소 측정:

- kill count.
- player HP.
- weapon id.
- active memory ids/levels.
- echo ids/levels.
- ultimate unlocked.
- death 여부.
- 가장 많이 damage를 낸 memory/echo.

목표:

- 기억 없는 구간은 너무 길지 않다.
- 기억 획득 후 사냥 방식이 달라진다.
- 망각 후 잔향이 새 방식으로 남는다.
- 궁극은 강하지만 화면 삭제만 하지 않는다.

## 14. 비범위

Complete Prototype에서도 아직 하지 않는다.

- 상점.
- 메타 성장.
- 다중 지역 완성.
- 최종 보스.
- 출시급 UI 전체.
- 출시급 사운드 전체.
- Steam/빌드 배포.
- `Assets/Lethe` 승격.

단, 아래는 이제 비범위가 아니다.

- 장송대검.
- 8종 기억.
- 8종 잔향.
- 4종 궁극 잔향.

## 15. 구현 우선순위

바로 다음 작업 순서:

1. `NEXT_TASKS`를 Complete Prototype 기준으로 갱신한다.
2. `PrototypeGameManager` hard-code를 data-driven core로 분리한다.
3. 쌍검/대검 weapon runtime을 병렬로 둔다.
4. 8기억의 level 1 동작부터 구현한다.
5. 8잔향의 +1 동작을 구현한다.
6. 그 다음 +3, +5, 궁극 순으로 확장한다.

중요:

```text
스프라이트 최종 퀄리티보다 먼저 "각 효과가 무엇인지"가 보여야 한다.
역할이 보이면 polish한다.
역할이 안 보이면 sprite를 아무리 고쳐도 기준이 안 잡힌다.
```

## 16. 결정

- 쌍검-only slice는 기준 확정용으로 부족하다.
- Complete Prototype은 `쌍검 + 대검 + 8기억 + 8잔향 + 4궁극`을 목표로 한다.
- 기존 첫 slice 문서들은 근거로 남기되, 이후 구현 우선순위는 이 PRD를 따른다.
- 첫 구현은 여전히 `_dev`에서 진행한다.
