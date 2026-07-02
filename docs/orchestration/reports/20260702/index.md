# 2026-07-02-01 - 다음 제작 과제 재정렬

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 쌍검/대검, 기억 8종, 잔향 8종, 궁극 4종, 기술별 프로토타입 사운드까지 들어간 상태다. 아직 `_dev` 검증 단계이며 `Assets/Lethe` 승격은 하지 않는다.

## 2. 오늘 바뀐 것

- 다음 작업 목록을 jaewoo 직접 검증 중심에서 Codex 제작 중심으로 바꿨다.
- 최우선 과제를 `잔향 무기별 차별화 1차 패스`로 정했다.
- `CURRENT_TASK.md`에 현재 부족분 진단과 다음 구현 완료 기준을 추가했다.
- 오늘 devlog에 분석 근거를 기록했다.

## 3. 테스트 결과와 근거

- 코드 변경은 없어서 Unity 빌드 테스트는 실행하지 않았다.
- 정적 확인 근거:
  - `V1GameManager.cs`의 `TriggerUtilityEchoes`가 여러 유틸리티 잔향을 한 무기 타격 경로에서 처리한다.
  - `WeaponRuntimeSpec`에는 이미 `MultiSmall` / `SingleHeavy` 구분이 있어 무기별 잔향 행동 분기가 가능하다.
  - `UpdateUtilityUltimate`의 비혈풍 궁극 3종은 `BloodBladeStorm`보다 무기별 패턴 차이가 약하다.
  - VFX Matrix QA는 존재하지만 쌍검/대검별 잔향 차이까지 증명하는 구조는 아직 부족하다.

## 4. 결정한 것

다음 제작 우선순위는 잔향 무기별 차별화다. 쌍검은 작고 빠른 연쇄/스택 감각, 대검은 느리고 큰 범위/폭발 감각으로 같은 잔향도 다른 전투 행동처럼 읽히게 만든다.

## 5. 문제 또는 리스크

- 잔향이 많아질수록 `V1GameManager` 한 곳에 확률, 반경, 데미지, VFX 값이 쌓이는 구조가 유지보수 리스크다.
- `BloodReflection`, `AshenShield`, `StoppedSecond`, `OblivionBrand`는 기능은 있지만 플레이어가 "내가 뭔가 발동했다"고 느끼기 약할 수 있다.
- 비혈풍 궁극 3종은 큰 효과는 있지만 무기별 리듬 차이가 아직 약하다.

## 6. GPT/Claude 인계 요약

다음 리뷰나 기획 판단은 "잔향이 무기별로 정말 다른 플레이를 만드는가"에 집중하면 된다. 새 잔향을 늘리는 방향이 아니라, 기존 8종을 쌍검/대검 문법으로 다르게 표현하는 것이 우선이다.

## 7. 다음 Codex 작업

1. `TriggerUtilityEchoes`를 무기별 패턴으로 분리한다.
2. 쌍검/대검 각각 8잔향이 다른 오브젝트 이름과 VFX 리듬을 만들게 한다.
3. 필요하면 Echo Matrix smoke를 쌍검/대검 두 경로로 확장한다.
4. `dotnet build`, Unity compile error check, console error check를 통과시킨다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 잔향 시스템은 수량상 구현됐지만, 일부는 같은 온힛 효과의 색/크기 변형처럼 느껴질 위험이 있다.
- 방향: 무기별 전투 문법을 잔향에도 적용해 쌍검과 대검의 빌드 경험을 분리한다.
- 행동: 다음 작업 목록과 현재 작업 문서를 제작 가능한 과제 중심으로 재정렬했다.
- 결과: 다음 세션은 검증 요청 없이 바로 잔향 무기별 차별화 구현으로 들어갈 수 있다.

# 2026-07-02-02 - 잔향 무기별 차별화 1차 구현

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에서 잔향 8종이 쌍검/대검에 따라 다른 런타임 패턴을 만들도록 1차 구현했다. `_dev` 검증 단계이며 `Assets/Lethe` 승격은 하지 않는다.

## 2. 오늘 바뀐 것

- 잔향 런타임 오브젝트 이름을 `EchoDual_*` / `EchoGreat_*` 계열로 분리했다.
- 쌍검은 빠른 연쇄, 작은 스택, 다중 타격 중심으로 만들었다.
- 대검은 큰 범위, 느린 강타, 폭발/장판 중심으로 만들었다.
- `BloodReflection` 잔향도 무기별로 갈랐다.
- 6개 유틸리티 잔향도 무기별로 갈랐다.
- Unity QA 메뉴를 추가했다:
  - `LETHE/V1 QA/Echo Matrix Dual Blades`
  - `LETHE/V1 QA/Echo Matrix Greatsword`

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy 경고 7개 / 오류 0개.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 통과, 경고 0개 / 오류 0개.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- Unity QA:
  - 쌍검 Echo Matrix: `[V1QA] PASS`, `EchoDual_ total=240`, 8잔향 모두 카운트.
  - 대검 Echo Matrix: `[V1QA] PASS`, `EchoGreat_ total=221`, 8잔향 모두 카운트.

## 4. 결정한 것

잔향은 별도 정의 16개로 늘리지 않는다. `EchoDefinition`은 8개를 유지하고, 무기별 표현은 런타임 행동과 VFX 리듬으로 갈라낸다.

## 5. 문제 또는 리스크

이번 패스는 체감 차이를 먼저 만든 1차 구현이다. 값과 반복 코드는 아직 `V1GameManager`에 남아 있으므로, 뒤쪽 데이터화/정리 패스에서 더 줄여야 한다.

## 6. GPT/Claude 인계 요약

잔향 무기별 차별화는 기술적으로 들어갔다. 다음 리뷰 포인트는 "쌍검은 잔향이 빠르게 몰아치는가", "대검은 잔향이 한 번에 크게 터지는가", "각 잔향의 정체성이 색만 다른 효과가 아니라 행동으로 구분되는가"다.

## 7. 다음 Codex 작업

다음 차례는 `패시브화 기억 보강`이다. 우선 대상은 `BloodReflection`, `AshenShield`, `StoppedSecond`, `OblivionBrand`이며, 각 기억이 +1/+3/+5에서 독립적인 행동을 보이게 만든다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 잔향 8종은 있었지만 무기별 빌드 감각이 충분히 갈라지지 않았다.
- 방향: 쌍검은 연쇄/스택, 대검은 강타/범위/폭발로 같은 잔향을 다르게 표현한다.
- 행동: 런타임 분기와 Echo Matrix QA를 추가했다.
- 결과: 8잔향 모두 쌍검/대검 경로에서 스폰과 카운트가 검증됐다.
