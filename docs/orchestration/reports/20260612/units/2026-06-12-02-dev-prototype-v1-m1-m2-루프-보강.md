# 2026-06-12-02 - Dev_Prototype_v1 M1/M2 루프 보강

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 이제 단순 생성 확인을 넘어서, M1 전투 셸과 M2 압축 루프를 Play Mode에서 강제로 확인할 수 있다. 메인 씬은 계속 `Assets/_dev/Scenes/Dev_Prototype_v1.unity`다.

## 2. 오늘 바뀐 것

- `M1 Smoke`, `M2 Loop`, `Continue` 디버그 버튼을 추가했다.
- `F8`로 M2 압축 스모크를 실행할 수 있게 했다.
- 적 처치가 즉시 XP로 바뀌지 않고 XP 오브를 생성하도록 바꿨다.
- 원거리 적 탄환이 플레이어에게 피해를 주도록 연결했다.
- M2 압축 루프에서 최고 레벨 망각, 결과 진행, 공명 재획득, 칼무리/혈반 +5, 피의 칼폭풍이 한 번에 검증되게 했다.
- XP 오브 수집 중 리스트가 바뀌며 터지던 런타임 예외를 고쳤다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 0 warning / 0 error.
- Unity compile errors: `count=0`.
- M2 압축 스모크 120프레임 후 스냅샷: `kills=10`, `level=2`, `echoes=[HungryBlades:5,BloodReflection:5]`, `storm=True`, `death=False`.
- Unity console errors: `count=0`.
- 증거 캡처 저장: `LETHE/Assets/_dev/Evidence/v1_m2_smoke_20260612.png`.
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과, 2개 unit heading 확인.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패.

## 4. 결정한 것

- v1은 계속 메인 경로다.
- 현재 M2는 실제 페이싱이 아니라 압축 주입이다. 상태 전환과 전투 연결을 확인하는 용도다.
- 다음 판단은 jaewoo가 직접 Play Mode에서 `M1 Smoke`와 `M2 Loop`를 눌러보고 한다.

## 5. 문제 또는 리스크

- 아직 실제 플레이로 60~120초 안에 M2에 자연 도달하는 구조는 아니다.
- XP/망각/공명은 동작하지만, 감정 페이싱은 다음 패스에서 봐야 한다.
- 일부 그래픽은 여전히 임시 런타임 도형/크롭 기반이다.
- Project Orchestrator Discord intake가 현재 응답하지 않아 dry-run 보고가 실패했다.

## 6. GPT/Claude 인계 요약

v1은 새 문서 기준으로 다시 만든 런타임이며, M2 압축 스모크가 예외 없이 통과했다. 이제 “이 방향이 맞는지”를 사람 플레이 리뷰로 판단한 뒤, 맞다면 압축 루프를 실제 Gatekeeper 페이싱으로 풀면 된다.

## 7. 다음 Codex 작업

- jaewoo 리뷰 결과를 받아 `GO/ITERATE/NO-GO` 정리.
- `GO/ITERATE`면 M2 실제 페이싱 구현.
- 이후 데이터 자산화와 프리팹 분리를 진행.

## 8. 포트폴리오 메모

- 문제: 이전 Unity 프로토타입은 구현 shortcut 때문에 기획 감정 루프를 평가하기 어려웠다.
- 방향: 먼저 상태 전환이 깨지지 않는 압축 루프를 만들고, 그 뒤 실제 페이싱으로 펼친다.
- 행동: Unity MCP로 Play Mode를 돌려 M2 루프를 직접 검증했다.
- 결과: v1에서 망각→잔향+5→공명→피의 칼폭풍까지 한 번에 재현 가능한 기준점이 생겼다.
