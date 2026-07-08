> 2026-07-08 LETHE 개발 보고서

# 2026-07-08-01 - 기억/잔향 왕귀 VFX와 판정 보강

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에 기억/잔향/궁극 잔향의 왕귀형 보상과 VFX 판정을 1차 구현했다. 아직 `_dev` 검증 단계이며 `Assets/Lethe` 승격은 하지 않았다.

## 2. 오늘 바뀐 것

- 처형은 즉사 직전 예고 VFX와 처형/망각 연계 폭발력을 추가했다.
- 추적은 가까운 적만 보지 않고 보스/힐러/위협 몬스터를 우선 노리게 했다.
- 파쇄는 밀집 몹과 보스에게 균열 보너스가 생겼다.
- 정지는 시간 정지 후 균열 폭발이 붙어 제어만 하는 루트가 아니게 했다.
- 잿빛 방패는 막은 피해와 잔향 타격으로 충전되고, 저장된 방어력을 파동으로 터뜨린다.
- 망각 낙인은 +5에서 전이/폭발하는 방향으로 보강했다.
- 피의 칼폭풍 외 궁극인 처형 균열, 정지 추적, 잿빛 망각의 피해와 VFX를 강화했다.
- 밀집 쌍검 QA는 처치 연쇄가 아니라 실제 밀집 히트/잔향 억제를 측정하도록 정리했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 기존 legacy warning 7개, error 0개.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 기존 legacy warning 7개, error 0개.
- Unity compilation errors: `0`.
- VFX Matrix: PASS, `previewMemory=8`, `previewEcho=8`, `fracture=1`, `stasis=1`, `ashen=1`.
- Echo Matrix Dual Blades: PASS, `total=226`, `state=78`.
- Echo Matrix Greatsword: PASS, `total=207`, `state=57`.
- Passive Memory Matrix: PASS, `blood=17`, `ash=6`, `stopped=8`, `oblivion=62`.
- Utility Ultimate Matrix Dual Blades: PASS, `fracture=28`, `stasis=11`, `ashen=47`.
- Utility Ultimate Matrix Greatsword: PASS, `fracture=49`, `stasis=22`, `ashen=14`.
- Dense Dual Blades Perf Matrix: PASS, `hits=18`, `suppressed=15`, `transient=45`, `activeVfx=26`, `ms=57.58`.

## 4. 결정한 것

혈반/피의 칼폭풍만 강하게 보상하는 구조를 피하고, 약하거나 수동적인 기억도 잔향과 궁극 잔향에서 폭발적으로 변형될 수 있게 잡았다. 특히 잿빛 방패는 방어형 기억이지만 저장 후 방출하는 공격형 보상으로 방향을 틀었다.

## 5. 문제 또는 리스크

자동 QA는 수치와 오브젝트 존재를 확인하지만, 실제 손맛과 가시성은 직접 플레이가 최종 판단이다. 특히 잿빛 파동이 너무 약하거나 반대로 너무 공짜 딜처럼 느껴지는지 확인이 필요하다.

## 6. GPT/Claude 인계 요약

이번 패스는 밸런스 수치만 올린 작업이 아니라, 각 기억/잔향이 몹에게 어떤 판정을 남기고 어떤 VFX 언어로 읽히는지 보강한 작업이다. 다음 리뷰는 텍스트 설명 없이 전투 화면만 보고 루트 차이를 느낄 수 있는지에 집중하면 된다.

## 7. 다음 Codex 작업

jaewoo 직접 플레이 후 가장 약한 루트 하나를 골라 세부 조정한다. 우선 후보는 잿빛 방패 충전량/방출 반경, 정지 균열 타이밍, 망각 전이 수, 처형 예고 가시성, 비혈반 궁극 피해량이다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: Blood Reflection이 효과, 회복, VFX, 궁극 연결을 모두 가져 다른 선택지가 약해 보였다.
- 방향: 약한 기억도 잔향/궁극 단계에서 강하게 변형되는 왕귀 구조를 만든다.
- 행동: 처형, 추적, 파쇄, 정지, 잿빛, 망각의 판정과 VFX를 보강하고 비혈반 궁극을 상향했다.
- 결과: 주요 Unity QA가 모두 통과했고, 이제 직접 플레이로 손맛과 선택지를 평가할 수 있는 상태가 됐다.

# 2026-07-08-02 - 칼무리 잔향 무기별 개성 분리

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에서 칼무리 잔향의 쌍검/대검 분기를 다시 잡았다. 쌍검은 빠른 사냥떼, 대검은 큰 판결검이 내려찍는 방향이다.

## 2. 오늘 바뀐 것

- 대검 칼무리 잔향을 기존 공통 클램프/물어뜯기 패턴에서 분리했다.
- 대검은 낙하하는 큰 칼날, 낙하선, 지면 찢기, 처형 균열, 충격 코어를 사용한다.
- 쌍검 칼무리는 기존처럼 빠른 칼날 군집/물어뜯기 언어를 유지했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 단독 재실행 기준 warning 0개, error 0개.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 기존 legacy warning 7개, error 0개.
- Unity compilation errors: `0`.
- Echo Matrix Greatsword: PASS, `total=335`, `K=136`, `state=58`.
- Echo Matrix Dual Blades: PASS, `total=230`, `K=8`, `state=85`.
- Dense Dual Blades Perf Matrix: PASS, `hits=18`, `suppressed=15`, `transient=94`, `activeVfx=30`, `ms=87.85`.

## 4. 결정한 것

잔향은 무기와 무관한 동일 효과가 아니라, 같은 잔향이라도 무기 성격을 보존해야 한다. 칼무리부터 쌍검은 빠르고 여럿, 대검은 적고 무겁게 갈라냈다.

## 5. 문제 또는 리스크

대검 칼무리 카운트가 크게 늘었다. 직접 플레이에서 통쾌하면 유지하고, 화면을 너무 덮으면 낙하선/지면 이빨 수를 줄이는 쪽이 맞다.

## 6. GPT/Claude 인계 요약

이번 수정은 칼무리 하나만 무기별 액션 문법을 분리한 사례다. 다음에는 처형, 추적, 정지, 잿빛 같은 유틸 잔향도 같은 기준으로 쌍검/대검 개성을 점검해야 한다.

## 7. 다음 Codex 작업

jaewoo가 대검 칼무리의 낙하검 연출을 확인한 뒤, 너무 과하면 VFX 수를 줄이고 약하면 충격/히트스톱을 더 조정한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 같은 잔향이 무기만 바뀌어도 거의 같은 화면 언어로 보였다.
- 방향: 잔향의 정체성은 유지하되 무기별 액션 문법을 다르게 만든다.
- 행동: 대검 칼무리 잔향을 낙하 판결검/지면 균열 패턴으로 분리했다.
- 결과: 대검/쌍검 Echo Matrix와 Dense 쌍검 성능 QA가 모두 통과했다.

# 2026-07-08-03 - 칼무리 잔향 상처 반응 수정

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에서 칼무리 잔향 +5 동작을 다시 수정했다. 이제 칼이 플레이어 몸에서 추가로 발사되는 느낌이 아니라, 무기 공격이 박힌 지점에서 상처/검흔/체인이 반응하는 쪽으로 바뀌었다.

## 2. 오늘 바뀐 것

- 각성 칼무리 체인의 시작점을 플레이어 위치가 아니라 맞은 적의 상처 위치로 옮겼다.
- 상처 폭발, 검흔, 체인 라인, 상처 체인 투사체 이름과 색을 추가했다.
- 밀집 쌍검 상황에서는 고비용 각성 체인을 억제해서 렉 회귀를 막고, 기본 칼무리 타격은 유지했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 기존 legacy warning 7개, error 0개.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: error 0개.
- Unity compilation errors: `0`.
- Echo Matrix Dual Blades: PASS, `total=230`, `K=8`, `state=86`.
- Echo Matrix Greatsword: PASS, `total=335`, `K=136`, `state=58`.
- Dense Dual Blades Perf Matrix: PASS, `hits=18`, `suppressed=15`, `transient=87`, `activeVfx=25`, `ms=80.59`.

## 4. 결정한 것

칼무리 잔향은 “몸에서 추가 칼 발사”가 아니라 “공격 지점에서 굶주린 칼들이 상처를 물고 다음 대상으로 이어지는 반응”이어야 한다. 이후 칼무리 튜닝도 이 기준을 따른다.

## 5. 문제 또는 리스크

자동 QA는 통과했지만 실제 플레이에서 상처 반응의 타이밍, 크기, 체인 라인이 충분히 맛있는지는 직접 확인이 필요하다.

## 6. GPT/Claude 인계 요약

칼무리의 핵심 설계 기준이 바뀌었다. 다음 리뷰에서는 수치보다 “공격 지점에서 상호작용한다”는 느낌이 화면에서 읽히는지를 우선 판단하면 된다.

## 7. 다음 Codex 작업

jaewoo가 직접 플레이하면서 +5 칼무리가 상처 폭발 -> 검흔/체인 -> 다음 대상 연결로 읽히는지 확인한다. 부족하면 스프라이트 추가보다 타이밍, 알파, 체인 지속시간, 생성 위치부터 조정한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 잔향이 무기 공격과 분리된 추가 발사체처럼 보여 설계 의도가 어긋났다.
- 방향: 잔향을 플레이어 몸이 아니라 타격 지점의 상호작용으로 재정의했다.
- 행동: 각성 칼무리 체인의 시작 위치와 VFX 언어를 상처 중심으로 바꿨다.
- 결과: 쌍검/대검 잔향 QA와 밀집 성능 QA가 모두 통과했다.
