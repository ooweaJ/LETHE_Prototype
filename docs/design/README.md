# LETHE Design Docs

최종 갱신: 2026-06-10

이 폴더는 LETHE의 기억/망각/잔향/공명 루프를 구체화하기 위한 한글 기획 문서 묶음이다. 현재는 Unity 셋업보다 "망각이 실제로 크게 느껴지는 장면"을 먼저 확정한다.

## 읽는 순서

1. `LETHE_GAME_DESIGN_OVERVIEW.md`
   - 프로젝트 전체 비전, 핵심 재미, 현재 단계.
2. `LETHE_CORE_SYSTEMS_UNITY_PLAN.md`
   - 기억, 잔향, 공명, 각성 잔향, 궁극 잔향.
3. `LETHE_FORGETTING_FEEL_SPEC.md`
   - 망각, 잔향, 공명, 궁극 목표가 플레이 중 어떻게 보여야 하는지.
4. `LETHE_WEAPON_MEMORY_ECHO_SPEC.md`
   - 무기, 기억, 잔향, 각성, 공명, 궁극 잔향의 구체 전투 행동.
5. `LETHE_UNITY_VERTICAL_SLICE_SPEC.md`
   - Unity 첫 구현 범위와 완료 기준.
6. `LETHE_RUN_STRUCTURE.md`
   - 한 판 흐름, 성장, 망각, 보충.
7. `LETHE_COMBAT_DESIGN.md`
   - 무기, 적 역할, 전투 감각.
8. `LETHE_CONTENT_TABLES.md`
   - 기억, 잔향, 시너지, 적 표.
9. `LETHE_BALANCE_BASELINE.md`
   - v0.12 기준 수치와 Unity 초기 목표.

## 문서 운영 원칙

- `LETHE_GAME_DESIGN_OVERVIEW.md`는 사람이 처음 읽는 입구다.
- 세부 수치와 표는 별도 문서에서 관리한다.
- 오래된 HTML 프로토타입 문서는 근거 자료로 남기고, 최신 판단은 이 폴더와 `docs/orchestration/state/`를 우선한다.
- Unity 첫 slice 전에는 상점, 메타 진행, 다중 지역, 최종보스, 대규모 기억 확장을 시작하지 않는다.
