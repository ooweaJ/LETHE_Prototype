# 2026-07-21-01 - Hunter Echo / Blood Readability Pass

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에 Greatsword Blood Echo 가독성 보강과 Hunter Echo 무기별 리워크가 적용됐다. C# 런타임/에디터 빌드와 Unity QA Matrix는 모두 통과했다.

## 2. 오늘 바뀐 것

- 대검 혈반 잔향:
  - 혈반 이아이도 반월 베기를 더 크게 만들고, 그림자 반월, 이중 반월, 충격 영역, 혈흔 bloom, 방사형 피 조각, 긴 절단선을 추가했다.
  - 피격 정지와 카메라 흔들림을 조금 올려서 작은 붉은 표시가 아니라 대검형 혈반 후속타로 읽히게 했다.
- 추적 잔향:
  - 쌍검은 초록색 쌍검 두 자루를 던져 적 사이를 튕기게 했다.
  - 대검은 초록색 대검 한 자루를 전방으로 던져 관통 광역기로 읽히게 했다.
  - 둘 다 기존 추적 잔향의 초록 계열을 유지해 계보를 알아볼 수 있게 했다.
- 컨셉 메모:
  - 정지 잔향은 기믹 유지, 고급 시계 VFX 방향으로 남겼다.
  - 파문/잿빛/낙인은 다음 패스에서 기억 컨셉 자체가 맞는지부터 재검토하기로 했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 통과.
- Unity compilation errors: `0`.
- Unity console errors: `0`.
- `LETHE/V1 QA/Echo Matrix Dual Blades`: PASS, `total=802`, `H=136`, `state=82`.
- `LETHE/V1 QA/Echo Matrix Greatsword`: PASS, `total=500`, `B=31`, `H=30`, `state=51`.
- `LETHE/V1 QA/Dense Dual Blades Perf Matrix`: PASS, `transient=109`, `activeVfx=82`, `ms=87.70`.

## 4. 결정한 것

이번 패스는 혈반과 추적을 먼저 실제 구현으로 정리했다. 나머지 잔향은 단순 확대/축소로 해결하지 않고, 기억의 판타지가 맞는지부터 확인한 뒤 VFX를 다시 잡는다.

## 5. 문제 또는 리스크

자동 QA는 생성 경로와 성능 예산만 확인한다. 쌍검 추적의 튕김이 진짜로 두 자루처럼 보이는지, 대검 추적이 관통 대검처럼 보이는지, 대검 혈반이 충분히 잘 보이는지는 직접 플레이 판단이 필요하다.

## 6. GPT/Claude 인계 요약

Hunter Echo는 무기별 행동으로 분리됐다. 쌍검은 두 녹색 칼날 ricochet, 대검은 녹색 대검 관통기다. Greatsword Blood는 큰 혈반 이아이도 VFX로 강화됐다. 다음 검토는 Stopped/Shatter/Ashen/Oblivion을 기억 컨셉부터 다시 보는 방향이다.

## 7. 다음 Codex 작업

직접 플레이 피드백을 받은 뒤, 다음 중 하나만 좁게 고른다: 정지 고급 시계 VFX, 파문 기억/잔향 컨셉 재정리, 잿빛 방어/반격 컨셉 재정리, 낙인 망각/확산 컨셉 재정리.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 여러 잔향이 서로 다른 기술처럼 보이지 않고 같은 표시의 크기 차이처럼 보였다.
- 방향: 무기별 행동 차이와 기억 판타지를 먼저 세우고, VFX는 그 행동을 읽히게 하는 수단으로 둔다.
- 행동: 혈반은 대검형 큰 혈반 베기로, 추적은 쌍검 튕김과 대검 관통으로 구현했다.
- 결과: 자동 QA 기준으로 커버리지와 성능은 통과했고, 다음 판단은 직접 플레이 감각 리뷰로 넘어간다.
