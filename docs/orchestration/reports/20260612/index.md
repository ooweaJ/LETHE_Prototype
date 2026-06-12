# LETHE 개발 보고서 - 2026-06-12

새 통합 설계 문서(`LETHE_DESIGN_00..07`)를 기준으로 Unity 프로토타입을 v1로 다시 시작했다.

# 2026-06-12-01 - Dev_Prototype_v1 새 출발

## 1. 현재 빌드 상태

`Dev_Prototype_v0`는 실패한 참고용으로 내리고, 새 메인 대상은 `Assets/_dev/Scenes/Dev_Prototype_v1.unity`다. v1은 `Scripts/PrototypeV1` 아래 새 코드로 분리했다.

## 2. 오늘 바뀐 것

- `V1GameManager`를 추가했다.
- `Dev_Prototype_v1.unity` 새 씬을 만들었다.
- 플레이어, 카메라, 맵, 쌍검, 적 스폰, HUD, XP/레벨업, 망각/잔향/공명 기초 루프를 새로 구성했다.
- Input System 전용 프로젝트에서 입력 예외가 나지 않도록 고쳤다.
- 4방향 sprite sheet가 통째로 보이는 문제를 런타임 frame crop으로 고쳤다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 0 warning / 0 error.
- Unity compile errors: `count=0`.
- Unity scene open: `Assets/_dev/Scenes/Dev_Prototype_v1.unity` 성공.
- Play Mode smoke: player 생성 `true`, enemy `2`, SpriteRenderer `107`.
- Console: v1 runtime exception 없음.
- Game capture: 플레이어/적이 sprite sheet 전체가 아니라 단일 프레임으로 표시됨.
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패.

## 4. 결정한 것

- v0를 더 고치지 않고 v1을 메인으로 한다.
- 지금은 8기억 전체 확장보다 M1 게임 셸 완성이 우선이다.
- 대검/8기억/8잔향/4궁극은 v1 M1/M2가 납득된 뒤 다시 확장한다.

## 5. 문제 또는 리스크

- v1은 아직 출시형 구조가 아니라 M1 검증용 런타임이다.
- sprite sheet는 import slicing이 아니라 런타임 crop으로 임시 처리했다.
- 아직 자동화 smoke injector와 M2 Gatekeeper 루프가 부족하다.
- Project Orchestrator Discord intake가 현재 응답하지 않아 dry-run 보고가 실패했다.

## 6. GPT/Claude 인계 요약

이전 Unity 프로토타입은 실패 기준으로 보고, 새 문서 기준의 v1을 시작했다. 다음 판단은 v1의 카메라/적/무기/XP/HUD가 게임 셸로 충분한지다.

## 7. 다음 Codex 작업

- v1 카메라와 전투 체감 보강.
- debug smoke injector 추가.
- Gatekeeper -> 망각 결과 -> 결손 생존 -> 공명 루프 구현.

## 8. 포트폴리오 메모

- 문제: 이전 프로토타입은 설계 의도를 반영하지 못해 평가 기준이 흔들렸다.
- 방향: 실패한 v0를 패치하지 않고 새 설계 문서 기준으로 v1을 분리했다.
- 행동: 새 씬/새 namespace/새 런타임을 만들고 Unity MCP로 컴파일·Play Mode 검증했다.
- 결과: 다시 평가 가능한 최소 게임 셸의 출발점이 생겼다.
