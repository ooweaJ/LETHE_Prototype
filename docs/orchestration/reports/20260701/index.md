# 2026-07-01-01 - 칼무리 C/D 런타임 적용

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에서 굶주린 칼무리 C/D 후보 스프라이트가 실제 전투 VFX에 연결됐다. 아직 `_dev` 단계이며 `Assets/Lethe` 승격은 하지 않았다.

## 2. 오늘 바뀐 것

- C / Crescent Pack을 칼무리 상시 오라로 적용했다.
- 칼무리 +3 이상에서는 C 오라가 반대 방향 레이어로 한 번 더 돈다.
- D / Predator Bite를 적중 지점의 물어뜯기 프레임으로 적용했다.
- 칼무리 잔향 후속타에도 D를 붙였다.
- 기억 획득 순간에는 C 후보가 크게 보이도록 했다.
- 기존 작은 칼날 파편은 삭제하지 않고 움직임 보조 레이어로 낮췄다.
- 쌍검은 오늘 바꾸지 않았다. 작아 보이는 원인은 6/22 팬텀 무기 전환과 6/25 쌍검 VFX 축소 튜닝 이력으로 확인했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy 경고 7개 / 오류 0개.
- Unity MCP compile error count: `0`.
- Unity `LETHE/V1 Smoke/M2 Loop`: `[V1QA] PASS`.
- M2 로그 상태: `HungryBlades:5`, `BloodReflection:5`, `storm=True`, live enemies `10`.

## 4. 결정한 것

칼무리 후보는 우선 `상시=C`, `적중=D` 조합으로 플레이 가능하게 만들었다. 후보 비교 단계에서 멈추지 않고 실제 손맛 판단으로 넘긴다.

## 5. 문제 또는 리스크

Unity Game View 캡처가 강제 칼무리 전투 프레임을 안정적으로 잡지 못했다. 최종 판단은 jaewoo 직접 플레이가 필요하다. C 오라가 너무 자주 겹치면 +5에서 시야를 가릴 수 있다.

## 6. GPT/Claude 인계 요약

굶주린 칼무리는 이제 C 후보가 플레이어 주변 군집 오라, D 후보가 적에게 몰려드는 bite 프레임으로 실제 런타임에 연결됐다. 쌍검은 오늘 변경 없음. 작아 보이는 체감은 기존 팬텀 무기화와 6/25 scale/lifetime 축소 이력 때문으로 보는 것이 맞다.

## 7. 다음 Codex 작업

jaewoo가 직접 플레이한 뒤 칼무리 C 오라의 알파/빈도, D bite 크기, 쌍검 phantom 크기 중 하나만 좁게 조정한다.

## 8. 포트폴리오 메모

- 문제: 후보 VFX가 Unity에서 보기만 가능하고 실제 손맛 판단으로 이어지지 않았다.
- 방향: 후보를 바로 런타임에 꽂아 플레이 판단 가능한 상태로 만든다.
- 행동: C는 오라, D는 bite로 연결하고 기술 QA를 통과시켰다.
- 결과: 칼무리 VFX는 이제 후보 이미지 기준으로 실제 플레이 리뷰가 가능해졌다.
# 2026-07-01-02 - 칼무리 D 단독 런타임 전환

## 1. 현재 빌드 상태

`Dev_Prototype_v1`의 굶주린 칼무리 VFX를 C/D 혼합에서 D 단독 중심으로 바꿨다. 아직 `_dev` 검증 단계이며 `Assets/Lethe` 승격은 하지 않았다.

## 2. 오늘 바뀐 것

- C / Crescent Pack의 런타임 사용을 제거했다.
- D / Predator Bite가 상시 칼무리 궤도, 적중 bite, 칼무리 잔향 후속타, 기억 획득, 잔향 변환 피드백에 모두 쓰이게 했다.
- 작은 보조 칼날 수와 알파를 줄여 D 실루엣이 묻히지 않게 했다.
- 적중/잔향 D bite는 더 크고 밝게 조정했다.
- 잔향 +3 이상에서는 보조 side bite가 한 번 더 붙는다.

## 3. 테스트 결과와 근거

- `rg` 확인: `V1GameManager.cs`에 C 런타임 참조 없음.
- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy 경고 7개 / 오류 0개.
- Unity MCP compile error count: `0`.
- Unity MCP console error count: `0`.
- Unity `LETHE/V1 Smoke/M2 Loop`: `[V1QA] PASS`.

## 4. 결정한 것

칼무리는 지금 단계에서 C/D 조합이 아니라 D 단독으로 체감 검증한다. 목표는 "원형 오라"보다 "계속 물어뜯고 달려드는 칼무리"가 먼저 읽히게 하는 것이다.

## 5. 문제 또는 리스크

D만 쓰면 정체성은 강해질 수 있지만, 반복 프레임처럼 보이거나 주변 적을 가릴 위험이 있다. 다음 조정은 새 후보 추가가 아니라 D 크기/알파/빈도 중 하나만 좁게 건드리는 쪽이 안전하다.

## 6. GPT/Claude 인계 요약

jaewoo가 "D가 안 느껴진다"고 피드백해서 C 오라를 제거하고 D를 상시/적중/잔향/획득 피드백의 주연으로 바꿨다. 기술 검증은 통과했고, 남은 판단은 직접 플레이에서 +1/+3/+5가 충분히 굶주린 칼무리처럼 읽히는지다.

## 7. 다음 Codex 작업

jaewoo 직접 플레이 결과에 따라 D size, alpha, spawn frequency 중 하나만 좁게 조정한다. 쌍검 phantom 크기 문제는 별도 피드백이 다시 나오면 분리해서 처리한다.

## 8. 포트폴리오 메모

- 문제: 후보 D를 붙였지만 C 오라에 묻혀 핵심 체감이 약했다.
- 방향: 후보를 섞지 않고 D 하나로 플레이 감각을 검증한다.
- 행동: D를 모든 주요 칼무리 순간에 연결하고 보조 VFX를 낮췄다.
- 결과: 기술 QA는 통과했고, 이제 직접 플레이로 D 단독 정체성을 판단할 수 있다.
# 2026-07-01-03 - 칼무리 살아있는 칼날 군집 전환

## 1. 현재 빌드 상태

`Dev_Prototype_v1`의 굶주린 칼무리를 C/D 후보 이미지가 아니라 원래 칼날 스프라이트 기반의 동적 군집 VFX로 다시 만들었다. 아직 `_dev` 검증 단계이며 `Assets/Lethe` 승격은 하지 않았다.

## 2. 오늘 바뀐 것

- C/D 후보 이미지는 런타임에서 제거했다.
- 상시 칼무리는 서로 다른 속도, 반경, 투명도, arc를 가진 칼날들이 불규칙하게 돈다.
- 가까운 적이 있으면 일부 칼날이 궤도에서 빠져나와 적에게 짧게 돌진한다.
- 레벨이 오르면 돌진 후 되돌아가는 recoil shard가 붙는다.
- 실제 피해 순간에는 여러 칼날이 한 점으로 모여 물고, 교차 상처선이 남는다.
- 칼무리 잔향 후속타는 큰 문양 대신 칼날 fan/surge로 터진다.
- 기억 획득 피드백은 칼날이 바깥으로 소용돌이치는 형태로 바뀌었다.

## 3. 테스트 결과와 근거

- `rg` 확인: `V1GameManager.cs`에 C/D 후보 런타임 참조 없음.
- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy 경고 7개 / 오류 0개.
- Unity MCP compile error count: `0`.
- Unity MCP console error count: `0`.
- Unity `LETHE/V1 Smoke/M2 Loop`: `[V1QA] PASS`.

## 4. 결정한 것

칼무리의 핵심은 새 한 장짜리 이미지가 아니라 움직임이다. 지금 방향은 "원형 오라"나 "큰 bite 문양"이 아니라, 작은 칼날들이 돌고, 적에게 쏠리고, 베고, 다시 돌아오는 살아있는 군집이다.

## 5. 문제 또는 리스크

동작은 강해졌지만 직접 플레이에서 과하면 적/플레이어를 가릴 수 있다. 다음 조정은 lunge frequency, trail alpha, blade count, hit convergence scale 중 하나만 좁게 건드리는 게 좋다.

## 6. GPT/Claude 인계 요약

jaewoo가 C/D 둘 다 별로라고 판단했다. 이에 따라 후보 이미지를 버리고 원래 칼날 기반 motion grammar로 칼무리를 재구성했다. 자동 QA는 통과했고, 남은 판단은 +1/+3/+5 직접 플레이에서 군집이 진짜 칼무리처럼 읽히는지다.

## 7. 다음 Codex 작업

jaewoo 직접 플레이 결과에 따라 lunge 빈도, trail 알파, 칼날 수, hit convergence 크기 중 하나만 조정한다. 쌍검 phantom 크기는 별도 피드백이 다시 나오면 분리해서 다룬다.

## 8. 포트폴리오 메모

- 문제: 후보 이미지는 정적인 마크처럼 보였고, 굶주린 칼무리의 행동성이 부족했다.
- 방향: 스틸 이미지가 아니라 모션 문법으로 정체성을 만든다.
- 행동: orbit, lunge, bite convergence, recoil, echo surge, memory spiral을 구현했다.
- 결과: 기술 QA는 통과했고, 이제 직접 플레이로 출시감에 가까운 VFX인지 판단할 수 있다.

# 2026-07-01-04 - 칼무리 큰 원 제거와 궤도 확장

## 1. 현재 빌드 상태

`Dev_Prototype_v1`의 굶주린 칼무리에서 큰 보조 원을 제거하고, 실제 칼날들이 도는 궤도를 더 크게 만들었다. 아직 `_dev` 검증 단계이며 `Assets/Lethe` 승격은 하지 않았다.

## 2. 오늘 바뀐 것

- `KalmuriSwarmBreathRing` 큰 원 연출을 제거했다.
- 칼날 orbit 반경을 키웠다.
- 상시 orbit 칼날 개수를 `10..22`개로 늘렸다.
- 범위감은 큰 원이 아니라 실제 칼날 궤도로 읽히게 했다.

## 3. 테스트 결과와 근거

- `rg` 확인: `V1GameManager.cs`에 `KalmuriSwarmBreathRing` 참조 없음.
- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy 경고 7개 / 오류 0개.
- Unity MCP compile error count: `0`.
- Unity `LETHE/V1 Smoke/M2 Loop`: `[V1QA] PASS`.

## 4. 결정한 것

칼무리에는 큰 장판 원을 쓰지 않는다. 궤도와 범위는 실제 칼날 수, 칼날 반경, 칼날 trail로만 읽히게 한다.

## 5. 문제 또는 리스크

칼날 수가 늘어났기 때문에 +5에서 화면이 복잡해질 수 있다. 다음 직접 플레이에서 과하면 blade count나 trail alpha 중 하나만 줄인다.

## 6. GPT/Claude 인계 요약

jaewoo가 큰 원이 오류처럼 보인다고 지적했다. 해당 원은 제거했고, 대신 실제 orbit 칼날 반경과 개수를 늘렸다. 자동 QA는 통과했다.

## 7. 다음 Codex 작업

jaewoo 직접 플레이 결과에 따라 칼날 수, orbit 반경, trail alpha 중 하나만 좁게 조정한다.

## 8. 포트폴리오 메모

- 문제: 큰 원이 칼무리보다 장판/오라처럼 읽혔다.
- 방향: 보조 원을 빼고 실제 칼날 움직임으로 범위감을 만든다.
- 행동: 큰 원 제거, orbit 반경 확대, 칼날 수 증가.
- 결과: 기술 QA는 통과했고, 직접 플레이로 가독성을 판단할 수 있다.

# 2026-07-01-05 - 칼무리 외곽 궤도 제거와 칼날별 데미지

## 1. 현재 빌드 상태

`Dev_Prototype_v1`의 굶주린 칼무리에서 바깥쪽 회전 칼무리 레이어를 제거하고, 날아가는 칼날마다 실제 데미지를 갖게 했다. 아직 `_dev` 검증 단계이며 `Assets/Lethe` 승격은 하지 않았다.

## 2. 오늘 바뀐 것

- `innerRadius` / `outerRadius` 2겹 구조를 제거했다.
- 하나의 `orbitRadius`만 남겨 플레이어 주변 칼무리가 한 겹으로 돌게 했다.
- `lane != 1`로 바깥 겹을 만드는 분기를 제거했다.
- 대상당 한 번 들어가던 칼무리 데미지를 날아가는 칼날 수만큼 나눴다.
- 각 `KalmuriBiteDiveBlade`가 개별 `DealDamage`를 호출한다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy 경고 7개 / 오류 0개.
- Unity MCP compile error count: `0`.
- Unity MCP console error count: `0`.
- Unity `LETHE/V1 Smoke/M2 Loop`: `[V1QA] PASS`.

## 4. 결정한 것

칼무리는 한 겹의 회전 칼날에서 적에게 튀어나가는 구조로 간다. 데미지도 이제 칼날 연출과 분리하지 않고, 실제로 날아간 칼날마다 나눠서 적용한다.

## 5. 문제 또는 리스크

칼날마다 데미지를 주면 피격 피드백이 더 잦아진다. 현재는 전체 데미지를 분할해 DPS는 유지했지만, 직접 플레이에서 숫자/피격 반짝임이 많으면 표시 빈도만 줄여야 한다.

## 6. GPT/Claude 인계 요약

jaewoo가 스크린샷으로 외곽 칼무리 레이어를 지적했다. 외곽 레이어를 제거하고 단일 orbit으로 바꿨으며, flying blade마다 damage event가 발생하도록 변경했다. 자동 QA는 통과했다.

## 7. 다음 Codex 작업

jaewoo 직접 플레이에서 한 겹 궤도가 맞는지, 칼날별 데미지 피드백이 과하지 않은지 확인한다. 필요하면 damage number 표시만 줄이고 실제 타격 구조는 유지한다.

## 8. 포트폴리오 메모

- 문제: 칼무리가 2겹 원으로 보여 의도보다 산만했다.
- 방향: 한 겹 orbit과 칼날별 실제 타격으로 연출과 판정을 일치시킨다.
- 행동: 외곽 orbit 제거, per-blade damage 적용.
- 결과: 기술 QA는 통과했고, 직접 플레이로 체감 확인 가능하다.

# 2026-07-01-06 - 칼무리 발사 범위와 찌르는 감각 보정

## 1. 현재 빌드 상태

`Dev_Prototype_v1`의 굶주린 칼무리가 보이는 회전 궤도보다 더 먼 적에게도 날아가고, 칼날이 적을 관통해 찌르는 느낌이 나도록 조정했다. 아직 `_dev` 검증 단계이며 `Assets/Lethe` 승격은 하지 않았다.

## 2. 오늘 바뀐 것

- 보이는 orbit과 별도의 더 큰 `lungeRange`를 추가했다.
- 칼무리 발사 대상 탐색은 `lungeRange`를 사용한다.
- bite 칼날 시작점을 적 근처가 아니라 플레이어 주변 궤도 쪽으로 옮겼다.
- 칼날 도착점을 적 뒤쪽으로 넘겨 찌르고 지나가게 했다.
- 칼날별 데미지를 짧은 시간차로 지연시켜 시각적 도착 타이밍과 맞췄다.
- 지연 데미지 순간 작은 `KalmuriBladePierceSpark`를 추가했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy 경고 7개 / 오류 0개.
- Unity MCP compile error count: `0`.
- Unity MCP console error count: `0`.
- Unity `LETHE/V1 Smoke/M2 Loop`: `[V1QA] PASS`.

## 4. 결정한 것

칼무리의 시각 궤도와 발사 판정 범위는 분리한다. 보이는 칼날은 몸 주변에 남아 있어도, 적이 조금 더 먼 거리에서 이미 칼날에 물리는 구조가 더 쾌감에 맞다.

## 5. 문제 또는 리스크

발사 범위가 커져 체감 공격력이 강해질 수 있다. 현재 데미지 총량은 유지했지만, 직접 플레이에서 너무 안정적이면 발사 범위만 낮추는 쪽이 좋다.

## 6. GPT/Claude 인계 요약

jaewoo가 칼무리가 회전 범위에 닿아야만 날아가는 것 같고, 찌르는 감각이 약하다고 피드백했다. 발사 범위를 키우고, 칼날 시작/도착/데미지 타이밍을 조정해 실제로 꽂히는 리듬을 만들었다. 자동 QA는 통과했다.

## 7. 다음 Codex 작업

jaewoo 직접 플레이에서 발사 시작 거리, 칼날 속도, pierce spark, 데미지 피드백이 과하거나 약한지 확인한다.

## 8. 포트폴리오 메모

- 문제: 칼날이 적 근처에서 생겨서 찌르는 느낌이 약했다.
- 방향: 플레이어 궤도에서 적을 향해 관통하는 투사체처럼 보이게 한다.
- 행동: lunge range 확대, start/end 재배치, staggered per-blade damage, pierce spark 추가.
- 결과: 기술 QA는 통과했고, 직접 플레이로 쾌감 판단 가능하다.

