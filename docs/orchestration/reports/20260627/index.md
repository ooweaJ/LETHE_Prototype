# 2026-06-27-01 - 베타 플레이 준비: 카탈로그, 승격 구조, HUD 정리

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 계속 `_dev`의 메인 플레이 씬이다. 이번 작업은 실행 파일 빌드가 아니라, 에디터에서 베타처럼 플레이하기 위한 구조 정리다.

## 2. 오늘 바뀐 것

- `V1ContentCatalog`를 추가했다.
- `Assets/_dev/Data/V1_ContentCatalog.asset`에 46개 sprite reference, Pretendard font, 무기 Definition 2개를 묶었다.
- `V1_GameManager`에 catalog와 `Weapon_DualBlades`, `Weapon_Greatsword`를 연결했다.
- `Assets/Lethe` 승격 준비 폴더와 `README.md`를 만들었다.
- `Assets/Lethe/Scenes/Lethe_BetaPreview.unity` 후보 씬을 만들었다.
- HUD에 현재 잔향 요약과 짧은 목표 문장을 추가했다.
- F12 기억/잔향/VFX 테스트 UI는 일부러 유지했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy v0/debug warning 7개, error 0개.
- Unity compile error count: `0`.
- Unity scene missing references: `0`.
- Unity Play Mode 진입 성공.
- `V1GameManager.DebugSnapshot()` 정상 시작 상태 확인.
- Unity console error count after stop: `0`.

## 4. 결정한 것

빌드/배포는 아직 하지 않는다. 대신 runtime asset reference, promotion-prep folder, player-facing HUD를 먼저 정리한다.

## 5. 문제 또는 리스크

`V1GameManager`는 아직 큰 단일 클래스이고, 많은 오브젝트가 런타임 생성 기반이다. `Assets/Lethe`는 후보 구조일 뿐이며, 전체 승격 완료 상태가 아니다.

## 6. GPT/Claude 인계 요약

Codex가 베타 플레이 준비 1차로 카탈로그와 승격 후보 구조를 추가했다. 다음 판단은 HUD/objective line이 플레이 중 방해되지 않는지, 그리고 기억/잔향 VFX 수정 요청을 어떤 id부터 처리할지다.

## 7. 다음 Codex 작업

1. jaewoo가 `Dev_Prototype_v1`을 직접 플레이한다.
2. F12 테스트 UI로 기억/잔향 VFX를 확인한다.
3. 수정 요청이 온 id부터 VFX scale, alpha, lifetime, spawn position을 조정한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 프로토타입은 돌아가지만 베타 플레이용 참조/구조/UI 층이 약했다.
- 방향: 빌드가 아니라 에디터 플레이 안정화와 승격 준비부터 한다.
- 행동: catalog, `Assets/Lethe` 구조, HUD 목표 정보를 추가했다.
- 결과: missing reference 0, compile error 0, Play Mode start state 정상 확인.

# 2026-06-27-02 - MCP 자동 플레이 QA

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 Unity Editor에서 열리고, AnkleBreaker Unity MCP는 `LETHE` 인스턴스 `7890` 포트에 연결되어 있다. 이번 확인은 빌드/export가 아니라 에디터 플레이 가능성, 런타임 주입, VFX 생성, 오류 여부를 보는 자동 QA다.

## 2. 오늘 바뀐 것

코드나 에셋 구현은 바꾸지 않았다. 대신 MCP로 다음 항목을 직접 실행했다.

- 쌍검 시작 스모크.
- 대검 시작 스모크.
- M2 압축 루프 스모크.
- 직접 Play Mode 진입 후 쌍검 런 시작.
- 8개 기억/8개 잔향 개별 VFX 프리뷰 생성.
- 6개 유틸 기억/6개 유틸 잔향/3개 비혈폭풍 궁극 프리뷰 생성.
- 8개 잔향 +5 및 피의 칼폭풍 준비 상태 확인.

## 3. 테스트 결과와 근거

- Unity compile error count: `0`.
- Unity scene missing references: `0`.
- 최종 Unity console error count: `0`.
- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: warning `0`, error `0`.
- 쌍검 직접 런: `elapsed=6.2`, `level=2`, `kills=2`, `enemies=26`, `timeScale=1`.
- 대검 스모크: `elapsed=2.2`, `enemies=10`.
- M2 스모크: `HungryBlades:5`, `BloodReflection:5`, `storm=True`.
- 8개 기억/잔향 개별 프리뷰: 모든 id가 `M1/E1`로 생성 확인.

## 4. 결정한 것

현재 상태는 “MCP 기준 기술적으로 플레이 진입 가능”으로 본다. 다만 이것은 손맛, VFX 과밀도, HUD 피로도, 잔향 구분감까지 통과했다는 뜻은 아니다.

## 5. 문제 또는 리스크

MCP는 클릭/키 입력을 사람처럼 오래 운용하는 플레이어가 아니다. 자동 QA는 참조/컴파일/상태/VFX 오브젝트 생성을 잡아낼 수 있지만, 재미와 가독성은 jaewoo 직접 플레이가 최종이다.

## 6. GPT/Claude 인계 요약

Codex 자동 QA에서 `Dev_Prototype_v1`은 쌍검/대검 시작, M2 압축 루프, 8기억/8잔향 VFX 프리뷰, 궁극 준비 상태까지 오류 없이 확인됐다. 다음 판단은 VFX 수정 요청 전에 jaewoo가 직접 보고 “어떤 기억/잔향이 안 보이는지, 너무 비슷한지, 너무 시끄러운지”를 id 단위로 적는 것이다.

## 7. 다음 Codex 작업

1. jaewoo 직접 플레이 피드백을 받는다.
2. 피드백이 VFX라면 기억/잔향 id 단위로 scale, alpha, lifetime, spawn position을 조정한다.
3. 피드백이 게임 완성도라면 reward cadence, attack readability, forgetting UX, spawn pressure 중 하나만 좁게 고친다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 사람이 플레이하기 전 자동으로 확인할 수 있는 범위와 없는 범위가 섞여 있었다.
- 방향: MCP 자동 QA는 기술 안정성과 런타임 연결을 검증하고, 감각 판단은 직접 플레이로 넘긴다.
- 행동: Unity 스모크 메뉴, Play Mode reflection, VFX preview probe, compile/missing-reference/console 검사를 실행했다.
- 결과: 기술적 막힘은 발견되지 않았고, 남은 핵심은 VFX/손맛 직접 리뷰로 좁혀졌다.

# 2026-06-27-03 - 플레이 전 검증선 보강

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 플레이 전 자동 QA 기준을 통과했다. 이제 단순히 “에러가 없다”가 아니라 쌍검, 대검, M2 루프, 8기억/8잔향 VFX, 피의 칼폭풍 실제 생성까지 조건 기반으로 확인한다.

## 2. 오늘 바뀐 것

- `V1SmokeTestMenu`를 고정 지연 스냅샷 방식에서 `[V1QA] PASS/FAIL` 방식으로 바꿨다.
- 쌍검/대검 시작 QA는 실제 시간이 흐르고 적이 생성되어야 PASS가 된다.
- M2 QA는 칼무리/혈반 잔향 +5, 피의 칼폭풍 준비, 결과 overlay, 적 생성을 확인한다.
- `LETHE/V1 QA/VFX Matrix`를 추가했다.
- `LETHE/V1 QA/Blood Blade Storm`을 추가했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: warning `0`, error `0`.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: warning `0`, error `0`.
- Unity compile error count: `0`.
- Unity scene missing references: `0`.
- 쌍검 QA: `[V1QA] PASS`, `elapsed=2.0`, `liveEnemies=8`.
- 대검 QA: `[V1QA] PASS`, `elapsed=2.0`, `liveEnemies=8`.
- M2 QA: `[V1QA] PASS`, `HungryBlades:5`, `BloodReflection:5`, `storm=True`, `result=True`.
- VFX Matrix QA: `[V1QA] PASS`, `previewMemory=8`, `previewEcho=8`, `fracture=1`, `stasis=1`, `ashen=1`.
- 피의 칼폭풍 QA: `[V1QA] PASS`, `stormObjects=77`.
- 최종 Unity console error count: `0`.

## 4. 결정한 것

플레이 전 기술 검증선은 이제 충분히 강한 상태로 본다. 이 다음부터는 자동 테스트보다 jaewoo 직접 플레이에서 보이는 감각 피드백이 우선이다.

## 5. 문제 또는 리스크

MCP 메뉴 실행은 가끔 `fetch failed`를 반환하지만 Unity 안에서는 명령이 실행된다. 따라서 이 작업의 기준은 MCP 호출 반환값이 아니라 Unity console의 `[V1QA] PASS/FAIL` 로그다.

## 6. GPT/Claude 인계 요약

Codex가 플레이 전 QA 메뉴를 보강했고, 쌍검/대검/M2/VFX Matrix/피의 칼폭풍이 모두 PASS했다. 남은 판단은 실제 플레이에서 공격 손맛, VFX 과밀도, 기억/잔향 구분감, HUD 피로도를 보는 것이다.

## 7. 다음 Codex 작업

1. jaewoo가 직접 플레이한다.
2. VFX 피드백이 오면 기억/잔향 id 단위로 수정한다.
3. 게임 완성도 피드백이 오면 보상 템포, 공격 판독성, 망각 UX, 스폰 압력 중 하나만 좁게 고친다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 이전 스모크 테스트는 고정 시간 스냅샷이라 실제 통과 조건이 약했다.
- 방향: 플레이 전 자동 QA를 조건 기반 PASS/FAIL 라인으로 바꾼다.
- 행동: 시작 무기, M2, VFX Matrix, 피의 칼폭풍 검증 메뉴를 강화/추가했다.
- 결과: 기술 검증선은 통과했고, 남은 리스크는 사람 눈과 손으로 판단할 감각 영역으로 좁혀졌다.

# 2026-06-27-04 - 20분 베타 런 밸런스 1차

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 이제 600초 압축 프로토타입이 아니라 20분 베타 런 후보로 조정됐다. 기본 목표는 1개 궁극 잔향을 완성하고 최종 문지기까지 처치하는 것이다.

## 2. 오늘 바뀐 것

- 런 하드 캡을 `1260초`로 바꿨다.
- 문지기 스케줄을 `300 / 600 / 900 / 1140초`로 바꿨다.
- 문지기 HP를 `1900 / 2800 / 4000 / 5400`으로 바꿨다.
- 시작 필요 XP를 `5`에서 `7`로 올렸다.
- 초반 XP 과속을 줄였다:
  - 0~120초 `x1.00`.
  - 120~600초 `x1.34`.
  - 600초 이후 `x1.00`.
  - 첫 120초 처치 XP 보너스 제거.
- 시간 생존만으로 클리어되는 조건을 제거하고, 4번째 문지기 처치를 클리어 조건으로 고정했다.
- 레벨업 보상 우선순위를 피의 칼폭풍 전용에서 4개 궁극 조합 공통으로 확장했다.
- `scripts/balance_sim_v1.js`를 추가했다.

## 3. 테스트 결과와 근거

`node scripts\balance_sim_v1.js` 기준 `20m_slow_start`가 1차 채택 후보가 됐다.

- 첫 보상: `24~28초`.
- 첫 망각: `323~329초`.
- 궁극 완성: `936~945초`.
- 최종 클리어: `1178~1188초`.
- 근거 문서: `docs/orchestration/evidence/2026-06-27-balance-sim-v1.md`.
- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy warning 7개, error 0개.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: warning 0개, error 0개.
- Unity compile error count: `0`.
- Unity console error count after QA: `0`.
- Unity MCP QA: 쌍검, 대검, M2, VFX Matrix, 피의 칼폭풍 모두 `[V1QA] PASS`.

## 4. 결정한 것

기본 클리어 목표는 **20분 런 + 궁극 잔향 1개 + 최종 문지기 처치**로 잡는다. 궁극 2개 완성은 기본 클리어 조건이 아니라 숙련/고점 목표로 둔다.

## 5. 문제 또는 리스크

순수 시뮬레이션은 위치, 피격, 회피, VFX 가림, 실제 손맛을 반영하지 않는다. 특히 대검 루트 클리어율이 쌍검보다 낮게 나와서 실제 플레이와 MCP 장기 검증에서 먼저 봐야 한다.

## 6. GPT/Claude 인계 요약

Codex가 20분 베타 런 1차 수치를 코드와 문서에 반영했다. 다음 판단은 `20m_slow_start`가 직접 플레이에서도 적절한지, 대검 루트가 너무 불안정한지, 4개 궁극 루트가 각자 다른 맛으로 클리어 가능한지다.

## 7. 다음 Codex 작업

1. MCP로 가능한 장기 상태 검증을 추가한다.
2. jaewoo 직접 플레이 후 XP, 문지기 HP, 보상 루트 유도, 대검 안정성 중 하나를 좁게 조정한다.
3. VFX 수정 요청이 오면 이번 밸런스 라인을 유지한 채 잔향/기억 id 단위로 조정한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 기존 초반 XP와 600초 구조는 베타 플레이로 보기엔 너무 빠르고 압축적이었다.
- 방향: 현대 플레이 템포에 맞춰 20분 목표, 1궁극, 최종 문지기 클리어 구조로 전환한다.
- 행동: 시뮬레이션을 만들고 XP/문지기/클리어 조건/보상 루트 우선순위를 조정했다.
- 결과: 첫 보상 24~28초, 궁극 15~16분, 클리어 19~20분대의 1차 수치 라인이 생겼다.
