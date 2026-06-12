# LETHE 기획·개발 기준 문서 (단일 source of truth)

최종 갱신: 2026-06-12

이 폴더는 LETHE의 **유일한 기획·개발 기준 문서 세트**다. HTML v0.12 프로토타입에서 검증된 코어(전투·런 루프·밸런스·UI)를 기준선으로 삼고, 그 위에 잔향·공명·궁극 잔향 확장 개념을 얹었다. **이 세트만 보고 Unity 구현이 가능하도록** 모든 수치를 포함한다.

이전의 분산된 기획/PRD 문서(A군: `LETHE_GAME_DESIGN_OVERVIEW`, `LETHE_CORE_SYSTEMS_UNITY_PLAN`, `LETHE_UNITY_*_PRD`, `LETHE_RUN_STRUCTURE`, `LETHE_COMBAT_DESIGN`, `LETHE_CONTENT_TABLES`, `LETHE_BALANCE_BASELINE` 등)와 에셋/아트 가이드(B군: `LETHE_UNITY_SLICE_ASSET_PRODUCTION_PLAN`, `LETHE_UNITY_ASSET_BINDING_PLAN`, `LETHE_VISUAL_ASSET_PLAN`, `LETHE_UNITY_ART_DIRECTION_REPLACEMENT_PLAN`, `LETHE_RELEASE_ART_FONT_VFX_PLAN`)는 이 세트(특히 07)로 통합·대체되었다.

## 핵심 원칙

- HTML 코어는 "참고 자료"가 아니라 **이식(port) 대상**이다. 검증된 수치는 재유도하지 말고 그대로 옮긴다.
- 새 개념(잔향·공명·궁극)은 HTML 코어를 대체하지 않고 **그 위에 얹는다**.
- "역할이 화면에서 읽히는가"가 스프라이트 퀄리티보다 우선이다. 단, 기본 게임 셸(HUD·레벨업·망각 결과 화면)은 HTML 수준을 먼저 재현한다.

## 문서 인덱스

| 문서 | 역할 |
| --- | --- |
| [00_OVERVIEW](LETHE_DESIGN_00_OVERVIEW.md) | 제품 비전, 코어 루프, 플레이어 감정, 범위 |
| [01_RUN_LOOP](LETHE_DESIGN_01_RUN_LOOP.md) | 런 타임라인, 압박 페이즈, 망각 타이밍, 결손 생존, 보충 |
| [02_COMBAT](LETHE_DESIGN_02_COMBAT.md) | 플레이어, 무기 2종, 적 4종, 보스, 전투 수치 |
| [03_MEMORY_ECHO](LETHE_DESIGN_03_MEMORY_ECHO.md) | 8기억, 망각→잔향 변환, 과부하, 공명, 각성, 4궁극 |
| [04_BALANCE](LETHE_DESIGN_04_BALANCE.md) | XP 곡선, 레벨업 선택, 스탯, 적 스케일링, 전 상수표 |
| [05_UI_UX](LETHE_DESIGN_05_UI_UX.md) | HUD, 시작/레벨업/망각결과/사망 화면, 패널, 디버그 |
| [06_BUILD_PLAN](LETHE_DESIGN_06_BUILD_PLAN.md) | Unity 구현 순서, 마일스톤, 데이터 구조, 수락 기준 |
| [07_ASSETS_VFX](LETHE_DESIGN_07_ASSETS_VFX.md) | 에셋·VFX·프리팹·클래스 연결 매트릭스(8/8/4+대검), imagegen·폰트·MCP |

## 수치 출처

모든 코어 수치는 `src/game.js`(HTML v0.12, `balance.version = "v0.12-balance-1"`)에서 추출했다. 각 표에 원본 식별자를 병기한다. 수치를 바꿀 때는 이 문서 세트와 `src/game.js`를 함께 갱신한다.

## 범위 (이 세트가 다루는 것 / 안 다루는 것)

- 다룬다: 무기 2종, 기억 8종, 잔향 8종, 궁극 4종, 적 4종 + 보스, 런 1판 루프, UI 전체, 밸런스 수치.
- 안 다룬다(보류): 상점, 메타 성장, 다중 지역, 최종 보스 확장, 출시급 사운드, 빌드 배포, `Assets/Lethe` 승격.
