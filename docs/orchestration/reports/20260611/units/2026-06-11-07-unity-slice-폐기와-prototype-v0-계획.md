# 2026-06-11-07 - Unity slice 폐기와 Prototype v0 계획

## 1. 현재 빌드 상태

`Dev_EchoSlice`는 더 이상 메인 작업 대상이 아니다. 현재 결론은 slice를 계속 고치는 것이 아니라 `Dev_Prototype_v0.unity`를 새로 만들어 HTML보다 나은 Unity 프로토타입을 직접 구현하는 것이다.

## 2. 오늘 바뀐 것

- `LETHE_UNITY_PROTOTYPE_V0_PLAN.md`를 추가했다.
- 디자인 문서 index를 갱신했다.
- 현재 작업을 `Dev_Prototype_v0` 구현으로 바꿨다.
- 다음 작업 목록을 새 프로토타입 기준으로 바꿨다.
- `Dev_EchoSlice`는 reference only로 격하했다.

## 3. 테스트 결과와 근거

- 이 유닛은 구현 검증이 아니라 전략/계획 수정이다.
- 기존 `Dev_EchoSlice`는 삭제하지 않았다.
- 새 메인 target은 `Assets/_dev/Scenes/Dev_Prototype_v0.unity`다.
- `npm.cmd run report`: 통과, 7개 unit report 생성.
- `npm.cmd run report:check`: 통과, 7개 unit heading 확인.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패. Project Orchestrator intake endpoint가 현재 응답하지 않는 상태로 판단.

## 4. 결정한 것

- `slice` 접근은 폐기한다.
- `Dev_EchoSlice`를 더 polish하지 않는다.
- 새 프로토타입은 combat loop first로 만든다.
- 잔향은 전투 루프 위에 올라가는 시스템으로 구현한다.

## 5. 문제 또는 리스크

- 새 scene을 만들기 전까지 Unity playable prototype은 아직 없다.
- 기존 slice 에셋을 참고하되, 그 구조를 그대로 복사하면 같은 문제가 반복될 수 있다.
- 첫 구현에서 욕심내서 잔향부터 넣으면 다시 테스트 장치가 될 위험이 있다.

## 6. GPT/Claude 인계 요약

LETHE Unity 작업은 `Dev_EchoSlice` 중심 slice에서 `Dev_Prototype_v0` 직접 구현으로 전환한다. 첫 성공 기준은 이동/공격/적 다수/피격/처치/리스폰/HUD가 있는 30초 combat loop다.

## 7. 다음 Codex 작업

- `Assets/_dev/Scenes/Dev_Prototype_v0.unity` 생성.
- `PrototypeRoot`, `Services`, `Player`, `EnemySpawner`, `Arena`, `RuntimeVFX`, `HUD` 구성.
- player movement, camera follow, 5 enemy spawn/chase, nearest targeting, player HP/contact damage, enemy death/respawn 구현.

## 8. 포트폴리오 메모

- 문제: 작은 slice는 빠르게 만들 수 있었지만 HTML보다 낮은 체감이면 의미가 없다.
- 방향: 시스템 검증 장치가 아니라 실제 게임 프로토타입을 만든다.
- 행동: 기존 slice 경로를 폐기하고 Prototype v0 계획과 작업 순서를 다시 정의했다.
- 결과: 다음 작업 기준이 “잔향 보기”에서 “플레이 가능한 LETHE 프로토타입”으로 바뀌었다.
