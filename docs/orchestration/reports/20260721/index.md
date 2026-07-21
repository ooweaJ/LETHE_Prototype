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

# 2026-07-21-02 - Spatial Hash Unity QA 통과

## 1. 현재 빌드 상태

`Dev_Prototype_v1`는 spatial hash 최적화 이후 Unity QA까지 통과했다. Unity MCP는 LETHE 프로젝트 포트 `7890`에 연결됐고, 씬은 `Dev_Prototype_v1`로 깨끗한 상태였다.

## 2. 오늘 바뀐 것

- 새 코드 변경은 없고, 2026-07-20 최적화 패치의 Unity QA를 완료했다.
- 상태 문서와 다음 작업 큐에서 `Spatial Hash Optimization Unity QA`를 완료 처리했다.

## 3. 테스트 결과와 근거

- Unity compilation errors: `0`.
- Unity console errors: `0`.
- `Dense Dual Blades Perf Matrix`: PASS, `hits=18`, `suppressed=15`, `transient=141`, `activeVfx=87`, `ms=43.11`.
- `Echo Matrix Dual Blades`: PASS, `prefix=EchoDual_`, `total=803`, `state=82`.
- `Echo Matrix Greatsword`: PASS, `prefix=EchoGreat_`, `total=501`, `state=53`.

## 4. 결정한 것

- Spatial hash 최적화는 자동 QA 기준으로 통과 처리한다.
- 다음 판단은 성능보다 직접 플레이 감각이다. 타겟 후보 순서가 바뀐 느낌이 있는지 확인해야 한다.

## 5. 문제 또는 리스크

- MCP polling이 간헐적으로 `fetch failed`를 반환했다.
- 실제 QA 메서드는 실행됐고 PASS 로그도 확인됐지만, MCP queue 안정성은 계속 주의가 필요하다.

## 6. GPT/Claude 인계 요약

공간 해시 최적화 후 Dense Dual Blades와 양쪽 Echo Matrix가 모두 통과했다. 특히 Dense Dual Blades Perf Matrix는 `ms=43.11`로 통과했다. 남은 것은 사람이 직접 밀집 전투를 플레이하며 타겟팅 감각 회귀가 없는지 보는 일이다.

## 7. 다음 Codex 작업

- 직접 플레이 피드백이 들어오면 타겟팅, Kalmuri, Utility Echo 중 하나의 축만 좁게 수정한다.
- 자동 작업 후보는 아직 남아 있는 `Greatsword Start-Smoke QA Fix`다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 서바이버 전투에서 적 수가 늘면 전체 리스트 검색이 성능 리스크가 된다.
- 방향: 전투 규칙은 유지하고 내부 쿼리 구조만 최적화한다.
- 행동: spatial hash 적용 후 Unity QA로 dense combat과 Echo coverage를 확인했다.
- 결과: 자동 QA 기준으로 최적화 패치가 통과했고, 다음 검증은 직접 플레이 감각 리뷰로 넘어갔다.
# 2026-07-21-03 - 칼무리 쌍검 가시성 개선

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에서 쌍검 칼무리 잔향 가시성 패스를 적용했고, C# 빌드와 Unity 자동 QA가 통과했다. 아직 최종 판단은 직접 플레이로 봐야 한다.

## 2. 오늘 바뀐 것

- 쌍검 칼무리 잔향 색을 밝은 청록/흰색에서 짙은 남색, 보라청색, 파란 외곽선 계열로 분리했다.
- 기본 쌍검 공격은 기존처럼 밝은 청록/흰색 베기로 남겼다.
- 칼무리 후속타 타이밍을 `0.035/0.012`에서 `0.085/0.018`로 늦춰, 기본 베기 뒤에 칼무리 물림이 따라오는 리듬으로 조정했다.
- 대검 칼무리 분기는 이번 작업에서 유지했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 최종 재실행 통과.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 통과.
- Unity compilation errors: `0`.
- Unity console errors: `0`.
- Dense Dual Blades Perf Matrix: PASS, `transient=103`, `activeVfx=81`, `ms=87.49`.
- Kalmuri Perf Matrix: PASS, `totalKalmuri=396`.
- Echo Matrix Dual Blades: PASS, `total=803`, `K=8`, `state=82`.
- Echo Matrix Greatsword: PASS, `total=499`, `K=8`, `state=51`.

## 4. 결정한 것

이번 문제는 크기보다 색상군과 타이밍 겹침이 원인이라고 보고, VFX 수를 크게 늘리지 않고 기존 칼무리 쌍검 브랜치의 팔레트와 후속타 지연을 조정했다.

## 5. 문제 또는 리스크

- 자동 QA는 성능과 경로가 깨지지 않았다는 근거일 뿐, 실제 가시성은 직접 플레이 판단이 필요하다.
- 너무 어둡게 느껴지면 다음 패스에서 보라 외곽선이나 파란 엣지만 강화하는 쪽이 좋다.

## 6. GPT/Claude 인계 요약

쌍검 기본 공격은 밝은 청록/흰색으로 유지하고, 칼무리 잔향은 남색/보라청색의 어두운 물림 연출로 분리했다. 직접 플레이에서 “기본 베기 -> 짧은 간격 -> 칼무리 물림” 순서가 읽히는지 봐야 한다.

## 7. 다음 Codex 작업

- jaewoo가 쌍검 + Hungry Blades/Kalmuri를 normal pack과 dense pack에서 직접 확인한다.
- 여전히 약하면 한 번에 하나만 조정한다: 더 어두운 core, 더 밝은 violet edge, 더 긴 delay, 또는 dense mode 기본 slash 억제.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 같은 색 계열의 전투 피드백이 겹치면 추가 효과가 사라진다.
- 방향: 효과의 크기보다 색상군, 실루엣, 타이밍을 분리한다.
- 행동: 쌍검 칼무리 잔향을 어두운 남색/보라청색 물림으로 재정의하고 후속타 타이밍을 늦췄다.
- 결과: 자동 QA는 통과했고, 다음 단계는 인간 눈으로 가시성 개선을 판정하는 것이다.

# 2026-07-21-04 - 정지/추적 마무리와 남은 잔향 컨셉 계획

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에 정지 잔향 clockwork VFX, 추적 잔향 가독성 보강, 대검 추적 부채꼴 제거가 적용됐다. 최종 Unity QA는 쌍검/대검 Echo Matrix와 Dense Dual Blades Perf Matrix 모두 통과했다.

## 2. 오늘 바뀐 것

- 쌍검 정지 잔향:
  - 작은 clock lock, 초침 sweep, clock tick, 강화된 tick cut을 추가했다.
  - dense 상황에서는 무거운 clockwork를 줄여 성능 예산을 지킨다.
- 대검 정지 잔향:
  - 기존 큰 clock field 위에 더 명확한 초침 sweep을 추가했다.
  - judgement hand는 유지했다.
- 쌍검 추적 잔향:
  - ricochet blade 크기를 `0.62 -> 0.82`로 키웠다.
  - 발사 즉시 ricochet preview link/mark를 찍어 단검이 어디로 튕기는지 먼저 읽히게 했다.
  - 빠른 칼날이 목표를 지나쳐 impact를 놓치는 케이스를 보정했다.
- 대검 추적 잔향:
  - 이상하게 보이던 green fan/cone pressure VFX를 제거했다.
  - thrown greatsword line/wake/reticle 중심으로 남겼다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 통과.
- Unity compilation errors: `0`.
- Unity console errors after final QA: `0`.
- `LETHE/V1 QA/Dense Dual Blades Perf Matrix`: PASS, `transient=139`, `activeVfx=82`, `ms=91.05`.
- `LETHE/V1 QA/Echo Matrix Dual Blades`: PASS, `total=946`, `H=175`, `St=160`, `stateH=19`, `stateSt=11`.
- `LETHE/V1 QA/Echo Matrix Greatsword`: PASS, `total=671`, `H=51`, `St=168`, `stateH=3`, `stateSt=19`.

## 4. 결정한 것

칼무리, 혈반, 추적, 정지는 컨셉 방향을 유지한다. 남은 컨셉 재정리 대상은 파문, 잿빛, 낙인이다.

## 5. 문제 또는 리스크

정지 초침은 자동 QA상 충분히 생성되지만, 실제 플레이에서 “멈춘 시간 속에서 움직이는 초침”으로 읽히는지는 직접 확인이 필요하다. 쌍검 추적 칼날도 커졌지만, 너무 커져서 기본 쌍검과 겹치는지는 사람 눈으로 봐야 한다.

## 6. GPT/Claude 인계 요약

Stopped Echo now uses clockwork second-hand VFX. Dual Blades Hunter blades are larger and have immediate ricochet route previews. Greatsword Hunter cone VFX was removed. Remaining concept work should focus on Shatter as world fracture, Ashen as stored guard/counter-pressure, and Oblivion as brand spread/erase.

## 7. 다음 Codex 작업

다음 구현 순서는 파문 -> 잿빛 -> 낙인이다. 파문은 바닥 균열, 잿빛은 저장 방어/반격, 낙인은 낙인 확산/소거로 컨셉을 먼저 고정한 뒤 VFX와 판정을 바꾼다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 일부 잔향이 크기/색 차이만 있는 효과처럼 보였다.
- 방향: 잔향을 먼저 기억 판타지와 무기별 행동으로 분리한다.
- 행동: 정지는 시계 초침, 추적은 쌍검 튕김/대검 관통으로 정리하고, 남은 3종의 컨셉 계획을 세웠다.
- 결과: 자동 QA와 성능 예산을 통과했고, 다음 라운드는 파문/잿빛/낙인의 개성 확립으로 좁혀졌다.
