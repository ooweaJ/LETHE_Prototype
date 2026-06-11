# LETHE Design Docs

최종 갱신: 2026-06-11

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
5. `LETHE_WEAPON_MEMORY_ECHO_DETAIL.md`
   - 잔향을 레벨 단위, 동작 단위, 무기별 표현까지 내려 쓴 상세 구현 설계.
6. `LETHE_ECHO_FORM_TRANSFORMATION_SPEC.md`
   - 활성 기억, 망각, 잔향, 각성, 공명, 궁극의 화면 형태 변환 문법.
7. `LETHE_UNITY_ECHO_SYSTEM_PRD.md`
   - Unity 첫 slice의 잔향 시스템 PRD, 클래스 역할, ScriptableObject, 프리팹, 에셋 구조.
8. `LETHE_VISUAL_ASSET_PLAN.md`
   - Unity 첫 slice에 사용할 스프라이트/VFX 콘셉트 이미지, 파츠, import 계획, 프리팹 연결.
9. `LETHE_UNITY_SLICE_ASSET_PRODUCTION_PLAN.md`
   - 첫 Unity slice를 위해 어떤 이미지를 어떤 순서로 만들고, 어떤 MCP/프리팹/테스트 게이트로 검증할지 정의.
10. `LETHE_UNITY_ASSET_BINDING_PLAN.md`
   - 캐릭터, 맵, 무기, 잔향 VFX 이미지 파일을 Unity 프리팹, ScriptableObject, Scene에 연결하는 MCP 실행용 지도.
11. `LETHE_UNITY_ECHO_SLICE_PROMOTION_GATE.md`
   - `_dev` echo slice를 언제 `Assets/Lethe`로 승격할지 정하는 GO/ITERATE/NO-GO 체크리스트.
12. `LETHE_UNITY_GAMEPLAY_SLICE_REPAIR_PLAN.md`
   - 허접한 VFX 확인 장치를 실제로 움직이는 최소 게임 slice로 끌어올리기 위한 수리 계획.
13. `LETHE_UNITY_VERTICAL_SLICE_SPEC.md`
   - Unity 첫 구현 범위와 완료 기준.
14. `LETHE_RUN_STRUCTURE.md`
   - 한 판 흐름, 성장, 망각, 보충.
15. `LETHE_COMBAT_DESIGN.md`
   - 무기, 적 역할, 전투 감각.
16. `LETHE_CONTENT_TABLES.md`
   - 기억, 잔향, 시너지, 적 표.
17. `LETHE_BALANCE_BASELINE.md`
   - v0.12 기준 수치와 Unity 초기 목표.

## 문서 운영 원칙

- `LETHE_GAME_DESIGN_OVERVIEW.md`는 사람이 처음 읽는 입구다.
- 세부 수치와 표는 별도 문서에서 관리한다.
- 오래된 HTML 프로토타입 문서는 근거 자료로 남기고, 최신 판단은 이 폴더와 `docs/orchestration/state/`를 우선한다.
- Unity 첫 slice 전에는 상점, 메타 진행, 다중 지역, 최종보스, 대규모 기억 확장을 시작하지 않는다.
