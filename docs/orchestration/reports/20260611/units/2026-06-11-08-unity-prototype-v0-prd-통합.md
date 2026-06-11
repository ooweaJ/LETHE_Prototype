# 2026-06-11-08 - Unity Prototype v0 PRD 통합

## 1. 현재 빌드 상태

구현을 더 진행하기 전에 기존 기획/개발 문서를 하나의 실행 PRD로 통합했다. 이제 Unity 작업의 최신 기준은 `LETHE_UNITY_PROTOTYPE_V0_PRD.md`다.

## 2. 오늘 바뀐 것

- `LETHE_UNITY_PROTOTYPE_V0_PRD.md`를 추가했다.
- 디자인 문서 index를 갱신했다.
- `CURRENT_TASK.md`를 PRD 기준 구현으로 바꿨다.
- `NEXT_TASKS.md`를 PRD milestone 순서로 바꿨다.
- `STATUS.md`의 다음 작업을 M1 Prototype Scene Skeleton으로 정리했다.

## 3. 테스트 결과와 근거

- 이 유닛은 구현 검증이 아니라 PRD/작업 기준 정리다.
- PRD는 기존 문서를 참고한다:
  - 게임 개요
  - 핵심 시스템
  - 런 구조
  - 전투 기획
  - 기억/잔향 상세
  - Unity 잔향 시스템 PRD
  - 콘텐츠 표
  - 밸런스 기준
  - Prototype v0 계획
- `npm.cmd run report`: 통과, 8개 unit report 생성.
- `npm.cmd run report:check`: 통과, 8개 unit heading 확인.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패. Project Orchestrator intake endpoint가 현재 응답하지 않는 상태로 판단.

## 4. 결정한 것

- PRD가 구현 순서와 수락 기준의 최신 기준이다.
- 다음 구현은 M1부터 시작한다.
- `Dev_EchoSlice`는 더 이상 메인 구현 대상이 아니다.

## 5. 문제 또는 리스크

- PRD만으로는 아직 playable prototype이 생기지 않는다.
- 다음 작업에서 M1을 바로 구현하지 않으면 다시 문서만 늘어날 위험이 있다.

## 6. GPT/Claude 인계 요약

Unity Prototype v0.1은 PRD milestone 기반으로 진행한다. 다음 Codex 작업은 `Dev_Prototype_v0.unity` 생성과 root scene skeleton 구축이다.

## 7. 다음 Codex 작업

- M1 Prototype Scene Skeleton 구현.
- `Dev_Prototype_v0.unity` 생성.
- `PrototypeRoot`, `Services`, `Player`, `EnemySpawner`, `Arena`, `RuntimeVFX`, `HUD` 구성.
- Play Mode에서 새 prototype scene이 열리는지 검증.

## 8. 포트폴리오 메모

- 문제: 기획 문서는 많았지만 실행 기준이 흩어져 있었다.
- 방향: 상위 PRD를 만들어 구현 순서와 수락 기준을 고정한다.
- 행동: 기존 문서들을 하나의 Unity Prototype v0 PRD로 통합했다.
- 결과: 다음 구현이 M1부터 M5까지 추적 가능한 milestone 구조로 정리됐다.
