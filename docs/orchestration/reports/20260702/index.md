> 2026-07-02 LETHE 개발 보고서

# 2026-07-02-01 - 다음 시작 과제 재정렬

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 쌍검/대검, 기억 8종, 에코 8종, 궁극 4종이 들어간 `_dev` 검증 단계다. 아직 `Assets/Lethe` 승격은 하지 않았다.

## 2. 오늘 바뀐 것

- jaewoo 직접 검증만 기다리는 흐름에서 Codex가 만들 수 있는 생산 격차를 다시 골랐다.
- 우선순위를 `에코 무기별 차별화`, `패시브화 기억 보강`, `망각/공명 UX`, `4궁극 무기별 패턴`, `에코 데이터화/정리`로 재정렬했다.
- `CURRENT_TASK.md`와 `NEXT_TASKS.md`를 다음 구현 중심으로 갱신했다.

## 3. 테스트 결과와 근거

- 코드 변경 전 분석 작업이라 Unity 빌드 테스트는 실행하지 않았다.
- 근거:
  - `TriggerUtilityEchoes`가 여러 유틸리티 에코를 한 경로에서 처리하고 있었다.
  - `WeaponRuntimeSpec`에는 이미 `MultiSmall` / `SingleHeavy` 구분이 있어 무기별 에코 분기가 가능했다.
  - `BloodReflection`, `AshenShield`, `StoppedSecond`, `OblivionBrand`는 작동하지만 플레이어에게는 패시브처럼 느껴질 위험이 컸다.

## 4. 결정한 것

다음 구현은 에코를 새로 늘리는 것이 아니라, 같은 에코가 쌍검과 대검에서 다른 전투 문법으로 보이게 만드는 쪽부터 진행한다.

## 5. 문제 또는 리스크

`V1GameManager`에 전투 수치와 VFX 값이 많이 모여 있어, 차별화가 진행될수록 후속 데이터화와 정리가 필요하다.

## 6. GPT/Claude 인계 요약

기획 리뷰는 "에코가 무기별로 정말 다른 플레이를 만드는가"와 "기억이 망각되기 전에 충분히 행동으로 읽히는가"를 봐야 한다.

## 7. 다음 Codex 작업

무기별 에코 차별화 1차 구현을 시작한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 기능은 많지만 일부 효과가 같은 색/반경/확률 차이처럼 느껴질 수 있었다.
- 방향: 무기별 전투 문법을 에코 표현에도 적용한다.
- 행동: 다음 구현 목록과 완료 기준을 재정렬했다.
- 결과: 검증 요청 없이 바로 만들 수 있는 순차 구현 계획이 생겼다.

# 2026-07-02-02 - 에코 무기별 차별화 1차 구현

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에서 에코 8종이 쌍검/대검에 따라 다른 효과와 패턴을 만들도록 1차 구현했다. `_dev` 검증 단계이며 `Assets/Lethe` 승격은 하지 않았다.

## 2. 오늘 바뀐 것

- 에코 오브젝트 계열을 `EchoDual_*` / `EchoGreat_*`로 분리했다.
- 쌍검은 빠른 연쇄, 작은 중첩, 다중 타격 중심으로 만들었다.
- 대검은 큰 범위, 무거운 각인, 폭발/장판 중심으로 만들었다.
- `BloodReflection` 에코와 6개 유틸리티 에코를 무기별로 갈랐다.
- Unity QA 메뉴를 추가했다:
  - `LETHE/V1 QA/Echo Matrix Dual Blades`
  - `LETHE/V1 QA/Echo Matrix Greatsword`

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 기존 경고 7개, 오류 0개.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 통과, 경고 0개, 오류 0개.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- Unity QA:
  - 쌍검 Echo Matrix: `[V1QA] PASS`, `EchoDual_ total=240`, 8개 에코 모두 카운트.
  - 대검 Echo Matrix: `[V1QA] PASS`, `EchoGreat_ total=221`, 8개 에코 모두 카운트.

## 4. 결정한 것

에코 정의를 16개로 늘리지 않고, `EchoDefinition`은 8개를 유지한다. 무기별 차이는 런타임 행동과 VFX 리듬으로 만든다.

## 5. 문제 또는 리스크

이번 패스는 체감 차이를 먼저 만든 1차 구현이다. 수치와 반복 코드는 아직 `V1GameManager` 안에 남아 있으므로 후속 데이터화가 필요하다.

## 6. GPT/Claude 인계 요약

검토 포인트는 "쌍검 에코는 빠르게 몰아치고, 대검 에코는 한 번에 크게 찍는가"이다. 색만 다른 효과가 아니라 행동 차이로 구분되는지 봐야 한다.

## 7. 다음 Codex 작업

다음은 `패시브화 기억 보강`이다. 우선 대상은 `BloodReflection`, `AshenShield`, `StoppedSecond`, `OblivionBrand`다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 에코 8종이 있었지만 무기별 빌드 감각이 충분히 갈라지지 않았다.
- 방향: 쌍검은 연쇄/중첩, 대검은 강타/범위/폭발로 표현한다.
- 행동: 무기별 런타임 분기와 Echo Matrix QA를 추가했다.
- 결과: 8에코 모두 쌍검/대검 경로에서 스폰과 카운트가 검증됐다.

# 2026-07-02-03 - 패시브 기억 보강 구현

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에서 `피의 반사`, `재의 방패`, `멈춘 1초`, `망각의 낙인`이 더 이상 단순 패시브처럼만 보이지 않도록 1차 보강했다. 아직 `_dev` 검증 단계이며 `Assets/Lethe` 승격은 하지 않았다.

## 2. 오늘 바뀐 것

- `BloodReflection`: 주기적 혈반 폭발, 대상 표식, 피해, 회복, +3 연결선, +5 각성 베기/피꽃 피드백을 추가했다.
- `AshenShield`: +3 반격선과 주변 반격 피해, +5 각성 방패 파동과 소량 회복을 추가했다.
- `StoppedSecond`: 시간 박동 링, +3 잔상 베기, +5 확장 정지 돔을 추가했다.
- `OblivionBrand`: 대상 연결선, +3 분기 낙인 링크와 확산 피해, +5 각성 봉인을 추가했다.
- `LETHE/V1 QA/Passive Memory Matrix` 메뉴를 추가했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 기존 경고 7개, 오류 0개.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 통과, 경고 0개, 오류 0개.
- 최종 에디터 빌드 재확인: 통과, 경고 0개, 오류 0개.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- Unity QA `LETHE/V1 QA/Passive Memory Matrix`: `[V1QA] PASS`, `blood=13`, `ash=5`, `stopped=6`, `oblivion=24`.

## 4. 결정한 것

기억 4종은 수치만 키우지 않고, +1 기본 행동, +3 연결/반격/잔상/분기, +5 각성 피드백으로 읽히게 한다. 플레이어가 망각 전에 기억의 정체를 한 번은 체감해야 한다.

## 5. 문제 또는 리스크

이번 패스는 자동 QA로 스폰과 에러를 확인한 단계다. 실제 플레이에서는 빈도, 피해량, 화면 밀도, 사운드 반복감이 과할 수 있어 다음 직접 리뷰 뒤 튜닝이 필요하다.

## 6. GPT/Claude 인계 요약

검토 포인트는 "기억이 패시브 설명문이 아니라 전투 행동으로 느껴지는가"이다. 특히 `망각의 낙인`은 보라색 선/분기 링크가 충분히 낙인답게 읽히는지, `재의 방패`는 방어 기억인지 공격 기억인지 혼란스럽지 않은지 봐야 한다.

## 7. 다음 Codex 작업

다음은 `망각/공명 UX 제작 패스`다. 기억 상실, 에코 획득, 공명 목표, +5 각성을 텍스트보다 먼저 VFX/행동 전환으로 보여주는 압축 디버그 플로우를 만든다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 일부 기억이 유용해도 플레이어 눈에는 자동 상태효과처럼 느껴질 위험이 있었다.
- 방향: 각 기억에 독립 발동 타이밍과 단계별 행동 언어를 준다.
- 행동: 4종 기억의 +1/+3/+5 피드백과 전용 QA 매트릭스를 추가했다.
- 결과: 네 기억 모두 자동 QA에서 오브젝트 생성과 콘솔 무오류가 검증됐다.

# 2026-07-02-04 - 망각/공명 UX 제작 패스

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에서 망각 결과가 텍스트 확인만이 아니라 `기억 파괴 -> 잔향 획득 -> 공명 목표 -> +5 각성/궁극 준비` 흐름으로 보이게 1차 구현했다. 아직 `_dev` 검증 단계이며 `Assets/Lethe` 승격은 하지 않았다.

## 2. 오늘 바뀐 것

- 망각 결과 오버레이를 정상 한국어 문구로 다시 덮어쓰게 했다.
- `ForgetFlow_*` 전환 VFX를 추가했다:
  - 사라진 기억 마커,
  - 기억 파괴 링,
  - 획득 잔향 마커,
  - 기억에서 잔향으로 이어지는 브릿지,
  - 잔향 레벨 링,
  - 공명 목표와 연결선,
  - +5 각성 스탬프/폭발,
  - 궁극 준비 브릿지.
- `DebugRunForgetResonanceFlow()`를 추가했다.
- `LETHE/V1 QA/Forget Resonance Flow` 메뉴를 추가했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 기존 경고 7개, 오류 0개.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 통과, 기존 경고 7개, 오류 0개.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- Unity QA `LETHE/V1 QA/Forget Resonance Flow`: `[V1QA] PASS`, `forgetFlow=15`, `echoTransform=2`, `ultimateReady=3`, `hungryEcho=5`, `bloodEcho=5`, `storm=True`, `result=True`.

## 4. 결정한 것

망각은 손실 안내 텍스트만으로 처리하지 않는다. 화면에서는 먼저 기억이 깨지고 잔향으로 이어지며, 공명 목표와 궁극 준비까지 짧은 전환 액션으로 보이게 한다.

## 5. 문제 또는 리스크

이번 패스는 압축 QA로 흐름을 증명한 단계다. 실제 플레이에서는 오버레이 문구 길이, VFX 밀도, 궁극 준비 브릿지의 과장 정도를 직접 보고 줄여야 할 수 있다.

## 6. GPT/Claude 인계 요약

검토 포인트는 "망각이 손실과 성장의 전환으로 동시에 읽히는가"이다. 특히 +5 각성과 궁극 준비가 한 화면에 들어올 때 정보가 과하지 않은지 봐야 한다.

## 7. 다음 Codex 작업

다음은 `4궁극 무기별 패턴 확장`이다. `BloodBladeStorm` 수준으로 `FractureExecution`, `StasisHunt`, `AshenOblivion`도 쌍검/대검 리듬이 다르게 보이게 만든다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 망각/공명이 작동해도 플레이어에게는 상태 변화 텍스트처럼 느껴질 수 있었다.
- 방향: 손실, 잔향, 공명, 각성, 궁극 준비를 짧은 전환 액션으로 묶는다.
- 행동: `ForgetFlow_*` VFX와 전용 QA 플로우를 추가했다.
- 결과: 자동 QA에서 결과 오버레이, +5 잔향, 궁극 준비, 전환 VFX가 모두 검증됐다.

# 2026-07-02-05 - 4궁극 무기별 패턴 확장

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에서 `피의 칼폭풍`을 제외한 3개 궁극도 쌍검/대검에 따라 다른 리듬과 VFX를 만들도록 1차 구현했다. 아직 `_dev` 검증 단계이며 `Assets/Lethe` 승격은 하지 않았다.

## 2. 오늘 바뀐 것

- `UltDual_*` / `UltGreat_*` 궁극 오브젝트 계열을 추가했다.
- `FractureExecution`:
  - 쌍검: 낮은 체력 대상에게 빠른 처형 컷과 표식을 연쇄로 넣는다.
  - 대검: 큰 처형 낙인, 단일 대검 베기, verdict burst로 찍는다.
- `StasisHunt`:
  - 쌍검: 작은 정지장, 마이크로 클램프, 빠른 추적탄을 다수 발사한다.
  - 대검: 대상 중심 정지 돔, 얼어붙은 대검 베기, 창 형태 읽기를 만든다.
- `AshenOblivion`:
  - 쌍검: 방패 반환, 패링선, 적에서 플레이어로 돌아오는 낙인 링크를 만든다.
  - 대검: 큰 잿빛 파쇄 파동과 묵직한 낙인 연결을 만든다.
- Unity QA 메뉴를 추가했다:
  - `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`
  - `LETHE/V1 QA/Utility Ultimate Matrix Greatsword`

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 재시도 후 통과, 경고 0개, 오류 0개. 첫 시도는 Unity/dotnet 동시 접근으로 DLL 파일 잠금이 발생했다.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 통과, 기존 경고 7개, 오류 0개.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- Unity QA:
  - 쌍검 Utility Ultimate Matrix: `[V1QA] PASS`, `fracture=19`, `stasis=9`, `ashen=34`.
  - 대검 Utility Ultimate Matrix: `[V1QA] PASS`, `fracture=8`, `stasis=22`, `ashen=16`.

## 4. 결정한 것

궁극도 에코처럼 새 정의를 늘리지 않고, 같은 궁극 조합을 무기 리듬으로 다르게 표현한다. 쌍검은 연쇄/다중/반환, 대검은 낙인/돔/파쇄파동이 기준이다.

## 5. 문제 또는 리스크

이번 패스는 자동 QA로 스폰과 에러를 확인한 단계다. 실제 플레이에서는 궁극 3종의 피해량, 화면 점유율, 발동 빈도, `피의 칼폭풍`과의 상대적 임팩트를 다시 봐야 한다.

## 6. GPT/Claude 인계 요약

검토 포인트는 "비혈액 궁극 3종이 피의 칼폭풍만큼 무기별로 다른가"이다. 특히 대검 궁극은 묵직해야 하지만 느려서 밋밋하면 안 되고, 쌍검 궁극은 빠르지만 화면 잡음처럼 보이면 안 된다.

## 7. 다음 Codex 작업

다음은 `에코 데이터화와 QA 카운터 정리`다. `V1GameManager` 안에 쌓인 에코/궁극 수치와 색, 반경, 타이밍을 작은 spec이나 데이터 쪽으로 덜어내고 QA 카운터를 유지한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 피의 칼폭풍은 강했지만 나머지 궁극 3종은 아직 큰 유틸 효과처럼 보일 위험이 있었다.
- 방향: 궁극도 무기별 전투 문법을 따른다.
- 행동: 3궁극에 쌍검/대검 분기와 전용 QA 매트릭스를 추가했다.
- 결과: 쌍검/대검 모두 자동 QA에서 3궁극 계열이 스폰되고 콘솔 무오류가 검증됐다.
# 2026-07-02-06 - 굶주린 칼무리 성능 최적화

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에서 굶주린 칼무리의 다수 적 상황 렉 위험을 줄이는 최적화가 들어갔다. 아직 `_dev` 검증 단계이며 `Assets/Lethe` 승격은 하지 않았다.

## 2. 오늘 바뀐 것

- +5 칼무리를 "작은 칼 다수"에서 "더 큰 칼 소수 + 명확한 물어뜯기/잔향" 방향으로 조정했다.
- 회전 칼 수, 물어뜯기 대상 수, 대상당 칼 수, 리턴 조각, 잔향 surge/barrage 수를 줄였다.
- +5 각성 투사체에 짧은 쿨다운을 넣어 밀집 타격 때 투사체와 링이 매번 폭증하지 않게 했다.
- 보조 링/플래시/컷 잔상 수명을 줄였다.
- `LETHE/V1 QA/Kalmuri Perf Matrix` 자동 QA를 추가했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 기존 경고 7개, 오류 0개.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 통과, 기존 경고 7개, 오류 0개.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- Unity QA `LETHE/V1 QA/Kalmuri Perf Matrix`: `[V1QA] PASS`.
- 최종 카운트: `orbit=44`, `bite=72`, `return=24`, `hunting=14`, `echoSurge=64`, `echoBarrage=32`, `totalKalmuri=374`.
- 최초 실패 기준 `totalKalmuri=690`에서 최종 `374`로 약 46% 줄었다.

## 4. 결정한 것

칼무리의 강함은 칼 개수만으로 만들지 않는다. +5에서는 칼을 무작정 늘리기보다 칼 크기, 발사 리듬, 타격 분배, 잔향 타이밍으로 쾌감을 유지한다.

## 5. 문제 또는 리스크

직접 플레이에서 +5 칼무리가 이전보다 덜 풍성하게 느껴질 수 있다. 성능 수치는 좋아졌지만, 실제 손맛은 다시 확인해야 한다.

## 6. GPT/Claude 인계 요약

검증 포인트는 "성능 최적화 뒤에도 굶주린 칼무리가 여전히 칼무리처럼 느껴지는가"이다. 특히 +5에서 대상 수가 줄어든 대신 칼 크기와 데미지 보정이 충분한지 봐야 한다.

## 7. 다음 Codex 작업

다음은 `에코 데이터화와 QA 카운터 정리`다. 다만 그 전에 짧은 직접 플레이로 +5 칼무리의 체감 밀도를 확인하면 좋다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 자동 전투 VFX가 다수 적 상황에서 너무 많은 오브젝트를 만들며 렉 위험이 커졌다.
- 방향: 판타지는 유지하되 수량 중심 연출을 크기, 리듬, 타격감 중심으로 바꿨다.
- 행동: 칼무리 스폰 수, 수명, 후속 잔향, +5 발사 쿨다운을 조정하고 전용 QA를 추가했다.
- 결과: 스트레스 QA에서 활성 칼무리 계열 오브젝트가 `690 -> 374`로 감소했고 자동 QA가 통과했다.
