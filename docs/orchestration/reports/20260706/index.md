> 2026-07-06 LETHE 개발 보고서

# 2026-07-06-01 - 문지기 회복 차단과 보스 패턴 예고 구현

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에서 첫 문지기 보스가 힐러 잡몹에게 회복받던 문제를 차단했고, 보스 공격은 빨간 범위 예고 후 피해가 들어가는 구조로 바뀌었다. 아직 `_dev` 검증 단계이며 `Assets/Lethe` 승격은 하지 않았다.

## 2. 오늘 바뀐 것

- `VoidPriest`가 `Gatekeeper`를 회복하지 못하게 했다.
- 문지기 패턴을 즉발 파문에서 빨간 예고 범위 기반 공격으로 바꿨다.
- 보스 순번별 패턴을 나눴다:
  - 1번째: 플레이어 발밑 메테오 원형 범위.
  - 2번째: 부채꼴 절단 + 메테오.
  - 3번째: 보스 중심 링 폭발 + 메테오.
  - 4번째: 부채꼴 + 양쪽 메테오 복합 압박.
- 보스 fallback 스프라이트도 순번별 색, 문양, 균열 형태가 다르게 보이도록 절차형으로 분기했다.
- `LETHE/V1 QA/Gatekeeper Pattern Matrix` QA 메뉴를 추가했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 단독 재실행 기준 경고 0, 오류 0.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 기존 레거시 경고 7, 오류 0.
- Unity compile error count: `0`.
- Unity QA `Gatekeeper Pattern Matrix`: `[V1QA] PASS`, `boss=4`, `meteor=15`, `cone=4`, `ring=3`.
- Unity QA `M2 Loop`: `[V1QA] PASS`, `hungryEcho=5`, `bloodEcho=5`, `storm=True`.
- Unity console error count after QA: `0`.

## 4. 결정한 것

보스 회복은 의도 기믹이 아니라 힐러 타겟 필터 누락으로 보고 차단했다. 보스 난이도는 숨겨진 회복이 아니라, 빨간 예고 범위를 보고 피하는 패턴으로 만든다.

## 5. 문제 또는 리스크

자동 QA는 패턴 오브젝트 생성과 런타임 안정성만 확인한다. 실제로 빨간 원/부채꼴이 충분히 빨리 읽히는지, 피할 시간이 적당한지, 첫 보스 TTK가 납득되는지는 직접 플레이로 다시 봐야 한다.

## 6. GPT/Claude 인계 요약

문지기 보스는 이제 힐러에게 회복받지 않는다. 보스 공격은 메테오, 부채꼴, 링 폭발의 빨간 예고 후 지연 피해 방식으로 바뀌었다. 다음 리뷰는 수치보다 "읽고 피할 수 있는가"와 "보스별 컨셉이 구분되는가"를 봐야 한다.

## 7. 다음 Codex 작업

jaewoo가 첫 문지기를 다시 플레이한 뒤, 한 가지 축만 골라 조정한다: 예고 시간, 범위 크기, 보스 HP/가드 지속, 또는 보스 스프라이트 폴리시.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 첫 보스가 잡몹과 함께 나오면 죽지 않는 것처럼 느껴졌다.
- 방향: 숨겨진 회복을 제거하고, 보스 난이도를 명확한 예고 패턴으로 만든다.
- 행동: 힐러의 보스 회복을 막고, 빨간 범위 예고형 보스 패턴과 순번별 스프라이트 차이를 구현했다.
- 결과: 빌드와 Unity QA가 통과했고, 다음 판단은 직접 플레이 감각 검증으로 넘어간다.

# 2026-07-06-02 - 잡몹 겹침 완화 소프트 분리 구현

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에 잡몹 소프트 분리가 들어갔다. 적들이 완전히 서로 막히는 충돌체가 된 것은 아니고, 뱀파이어 서바이벌류처럼 촘촘히 몰려오되 한 점에 겹쳐 보이지 않도록 서로 살짝 밀어내는 방식이다. 아직 `_dev` 검증 단계이며 `Assets/Lethe` 승격은 하지 않았다.

## 2. 오늘 바뀐 것

- `V1GameManager.EnemySeparationForce()`를 추가했다.
- `V1Enemy.Update()`에서 기존 추적/사격 이동 후 작은 분리 보정을 적용했다.
- 보스는 덜 밀리고, 잡몹은 보스 주변에서 조금 더 비켜나가도록 했다.
- 원거리 적 `DriftingEye`도 사거리에서 멈춰 쏠 때 살짝 분리되도록 했다.
- QA 메뉴 `LETHE/V1 QA/Enemy Separation Matrix`를 추가했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 단독 재실행 기준 경고 0, 오류 0.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 기존 레거시 경고 7, 오류 0.
- Unity compile error count: `0`.
- Unity QA `Enemy Separation Matrix`: `[V1QA] PASS`, overlap pair `91 -> 4`.
- Unity QA `M2 Loop`: `[V1QA] PASS`, `hungryEcho=5`, `bloodEcho=5`, `storm=True`.

## 4. 결정한 것

잡몹은 완전 충돌로 막지 않고 소프트 분리로 처리한다. 생존류 게임의 압박감은 유지하되, 여러 마리가 하나의 덩어리처럼 겹쳐 읽히는 문제만 줄이는 방향이다.

## 5. 문제 또는 리스크

자동 QA는 겹침 감소와 런타임 안정성만 확인한다. 실제 플레이에서 몹 무리가 너무 헐거워졌는지, 반대로 아직 뭉쳐 보이는지는 직접 플레이 감각으로 다시 봐야 한다.

## 6. GPT/Claude 인계 요약

잡몹 겹침은 하드 충돌 대신 소프트 분리로 완화했다. 다음 리뷰에서는 보스 패턴보다도 “무리가 여전히 위협적인가”, “한 점으로 뭉개져 보이지 않는가”, “보스 주변 잡몹 간격이 읽히는가”를 확인하면 된다.

## 7. 다음 Codex 작업

jaewoo가 밀집 웨이브를 플레이한 뒤 너무 느슨하거나 여전히 뭉친다고 느끼는 쪽에 맞춰 분리 padding, 잡몹 multiplier, 보스 주변 가중치, 원거리 적 분리량 중 하나만 좁게 조정한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 잡몹이 겹쳐 오면 전투 위협은 커져도 시각적으로 한 덩어리처럼 읽힌다.
- 방향: 생존류 압박감은 유지하고, 읽기 어려운 겹침만 줄인다.
- 행동: 적 리스트 기반 소프트 분리와 전용 QA 매트릭스를 추가했다.
- 결과: QA에서 의도적으로 겹친 14마리의 overlap pair가 `91 -> 4`로 줄었고, 기존 M2 루프도 유지됐다.

# 2026-07-06-03 - 문지기 보스 외형 퇴화 복구

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에서 문지기 보스 본체 스프라이트를 다시 손봤다. 직전 패스의 절차형 보스 본체가 너무 둥글고 단순해서 슬라임처럼 읽히는 문제가 있었고, 이번에는 각진 철문/가면 실루엣의 PNG 본체 4종을 우선 로드하도록 바꿨다.

## 2. 오늘 바뀐 것

- `spr_boss_gatekeeper_01.png`를 더 각진 문지기 본체로 교체했다.
- `spr_boss_gatekeeper_02.png`, `03.png`, `04.png`를 추가했다.
- `V1GameManager`에 `BossGatekeeperRankPaths`를 추가했다.
- 보스 스프라이트는 순번별 PNG를 먼저 로드하고, 실패할 때만 fallback을 쓰도록 바꿨다.
- fallback 절차형 스프라이트도 둥근 덩어리가 아니라 각진 관문/가면 형태가 되도록 고쳤다.

## 3. 테스트 결과와 근거

- Unity `AssetDatabase.Refresh()`로 새 PNG와 `.meta` import 확인.
- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 기존 레거시 경고 7, 오류 0.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 단독 재실행 기준 경고 0, 오류 0.
- Unity compile error count: `0`.
- Unity QA `Gatekeeper Pattern Matrix`: `[V1QA] PASS`, `boss=4`, `meteor=15`, `cone=4`, `ring=3`.
- Unity QA `M2 Loop`: `[V1QA] PASS`, `hungryEcho=5`, `bloodEcho=5`, `storm=True`.

## 4. 결정한 것

보스 본체는 패턴 구분용 절차형 fallback으로 때우지 않는다. 본체는 최소한 별도 PNG 에셋을 우선하고, fallback은 로드 실패 대비용으로만 둔다.

## 5. 문제 또는 리스크

자동 QA는 스프라이트 import와 런타임 안전성만 확인한다. 최종적으로 이 보스가 충분히 성의 있고 위협적으로 보이는지는 직접 눈으로 다시 판단해야 한다.

## 6. GPT/Claude 인계 요약

문지기 보스 외형 퇴화는 인정하고 수리했다. 이제 네 순번 보스는 별도 PNG 본체를 로드한다. 다음 리뷰는 전투 수치보다 보스 실루엣, 위압감, 패턴과 이미지의 일치감을 봐야 한다.

## 7. 다음 Codex 작업

jaewoo가 새 문지기 본체 4종을 보고도 부족하다고 판단하면, 다음은 전투 코드가 아니라 보스 아트 전용 패스로 진행한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 보스가 성의 없는 둥근 placeholder처럼 보였다.
- 방향: 문지기라는 컨셉이 보이는 각진 가면/관문 실루엣을 되살린다.
- 행동: 순번별 PNG 보스 본체 4종을 만들고 코드 로드 순서를 에셋 우선으로 바꿨다.
- 결과: 빌드와 Unity QA는 통과했고, 다음 판단은 직접 시각 리뷰로 넘어간다.

# 2026-07-06-04 - 힐러 중첩 완화와 잔향/기억 상호작용 감사

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에서 힐러 잡몹의 회복이 보이도록 바뀌었고, 여러 힐러가 같은 잡몹을 동시에 과하게 살리는 문제를 줄였다. 동시에 혈반 잔향이 왜 제일 강하게 느껴지는지, 기억/잔향/궁극잔향과 몹의 상호작용에서 무엇이 부족한지 감사 문서로 정리했다.

## 2. 오늘 바뀐 것

- `VoidPriest` 힐에 초록색 회복 이펙트를 추가했다.
- 힐러가 가장 많이 다친 근처 비보스 잡몹을 우선 회복한다.
- 힐 주기는 `1.05초`, 힐량은 `2.4`, 한 번에 최대 3명까지로 정리했다.
- 같은 대상은 `0.95초` 동안 힐러 회복을 다시 받지 못하게 해서 힐러 여러 명이 겹쳤을 때 중첩 회복이 폭주하지 않게 했다.
- `LETHE/V1 QA/Void Priest Heal Matrix` QA를 추가했다.
- `docs/orchestration/review_prompts/2026-07-06-echo-memory-monster-interaction-audit.md`에 잔향/기억/궁극잔향 상호작용 문제를 정리했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 기존 레거시 경고 7, 오류 0.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 단독 재실행 기준 경고 0, 오류 0.
- Unity compile error count: `0`.
- Unity QA `Void Priest Heal Matrix`: `[V1QA] PASS`, `attempts=12`, `accepted=4`, `vfx=16`.
- Unity QA `M2 Loop`: `[V1QA] PASS`, `hungryEcho=5`, `bloodEcho=5`, `storm=True`.
- Unity QA `Echo Matrix Dual Blades`: `[V1QA] PASS`, `K=8`, `B=56`, `Ex=64`, `H=24`, `Sh=8`, `St=8`, `A=32`, `O=40`.
- Unity QA `Passive Memory Matrix`: `[V1QA] PASS`, `blood=17`, `ash=6`, `stopped=8`, `oblivion=36`.
- Unity QA `Utility Ultimate Matrix Dual Blades`: `[V1QA] PASS`, `fracture=22`, `stasis=9`, `ashen=47`.

## 4. 결정한 것

힐러는 숨겨진 수치가 아니라 보이는 전장 역할로 둔다. 혈반만 숫자로 깎는 식이 아니라, 먼저 다른 기억/잔향이 몹에게 어떤 상태 변화를 남기는지 더 잘 보이게 만든다.

## 5. 문제 또는 리스크

혈반은 표식, 지속 피해, 회복, bloom, 궁극 루트까지 한 번에 묶여 있어 체감이 강하다. 반대로 정지, 잿빛, 파문 같은 계열은 기능은 있어도 몹 상태 변화가 약하게 읽혀 “뭔가 되는지 모르겠다”가 생긴다.

## 6. GPT/Claude 인계 요약

힐러 중첩은 대상별 lockout으로 완화했다. 다음 밸런스 패스는 raw damage buff가 아니라 비혈반 계열의 몹 상태 표식과 상호작용 감각을 먼저 강화해야 한다.

## 7. 다음 Codex 작업

비혈반 기억/잔향에 몹 상태 마크를 붙인다: 파쇄는 균열, 정지는 동결/정지 표식, 사냥은 조준/관통, 잿빛은 방패 파쇄/반격, 망각은 낙인 누적처럼 보이게 한다. 이후 피해/제어/회복 기여도를 수치로 찍는 QA를 추가한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 힐러가 겹치면 잡몹이 죽지 않고, 혈반만 답처럼 느껴졌다.
- 방향: 힐러 역할을 보이게 만들고, 혈반 외 계열도 몹과의 상호작용을 읽히게 만든다.
- 행동: 힐러 회복 VFX, 회복 lockout, 힐러 QA, 상호작용 감사 문서를 추가했다.
- 결과: 힐러 QA와 기존 주요 QA가 통과했고, 다음 핵심 작업은 비혈반 계열 가독성 패스로 정리됐다.
> 2026-07-06 LETHE 개발 보고서

# 2026-07-06-05 - 보스 즉시 점프 디버그와 다음 개선 정리

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에 첫 보스로 바로 넘어가는 디버그 경로가 들어갔다. 이제 시작 화면에서도 `F6`을 누르면 런을 시작하고, F12 디버그 패널에서는 `Boss` 버튼으로 바로 문지기 보스 상태를 만들 수 있다.

## 2. 오늘 바뀐 것

- `F6`이 단순 보스 소환이 아니라 `DebugJumpToGatekeeper()`를 실행하도록 바뀌었다.
- 시작 화면에서도 `F6`이 동작한다.
- F12 디버그 패널에 `Boss` 버튼을 추가했다.
- 보스 점프는 기존 보스를 지우고, 빈 런이면 기본 리뷰 기억 `칼무리 3 / 혈반 2 / 정지초침 1`을 넣은 뒤, 주변 잡몹과 첫 문지기 보스를 즉시 만든다.
- Unity QA 메뉴 `LETHE/V1 QA/Gatekeeper Jump`를 추가했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 기존 legacy 경고 7개, 오류 0.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 경고 0, 오류 0.
- Unity compile error count: `0`.
- Unity QA `Gatekeeper Jump`: `[V1QA] PASS`, `boss=1`, `liveEnemies=15`.
- Unity console error count after QA: `0`.

## 4. 결정한 것

다음 작업은 단순 이펙트 확대가 아니라 밀집전 성능과 VFX 식별성부터 잡는다. 지금 상태에서 화면을 더 화려하게 만들면 쌍검 밀집전 렉과 효과 구분 문제가 더 커질 가능성이 높다.

## 5. 문제 또는 리스크

- VFX 식별성이 낮아서 잔향과 기억의 차이가 플레이 중에 덜 보인다.
- 칼무리 잔향은 아직 칼무리라는 컨셉과 잘 맞지 않는다.
- 쌍검이 많은 적을 때릴 때 렉이 느껴진다.
- 의심 지점은 밀집 히트 VFX 오브젝트 churn, 칼무리/잔향 반복 생성량, LINQ 기반 적 검색, `VoidPriest`의 전체 씬 힐 대상 검색이다.

## 6. GPT/Claude 인계 요약

보스 점프 디버그는 구현 완료됐다. 다음 판단은 콘텐츠 추가보다 성능과 가독성이다. 특히 쌍검 밀집전에서 어느 VFX 계열이 많이 생성되는지 측정하고, 칼무리 잔향은 별도 컨셉 재설계가 필요하다.

## 7. 다음 Codex 작업

1. 쌍검 밀집전 성능/오브젝트 카운트 QA를 추가한다.
2. 최악의 반복 VFX 계열을 pool/cap/쿨다운으로 줄인다.
3. `VoidPriest` 힐 탐색을 매번 `FindObjectsByType`로 도는 방식에서 매니저 적 리스트 기반으로 바꾼다.
4. 칼무리 잔향을 “더 많은 파란 칼”이 아니라 사냥, 관통, 물어뜯기, 회수 같은 명확한 칼 동작으로 다시 잡는다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 보스를 확인하려면 시간을 기다려야 했고, 최신 직접 플레이에서 VFX 식별성/칼무리 컨셉/쌍검 렉 문제가 다시 드러났다.
- 방향: 테스트 접근성을 먼저 열고, 다음 작업을 성능과 가독성 중심으로 좁힌다.
- 행동: `F6`, F12 `Boss`, `Gatekeeper Jump` QA를 추가하고 다음 개선 목록을 재정렬했다.
- 결과: 보스는 즉시 확인 가능해졌고, 다음 패스는 밀집전 성능과 칼무리 잔향 재설계로 명확해졌다.
