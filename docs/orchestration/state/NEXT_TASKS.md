# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.

Current source of truth: `docs/design/` (LETHE_DESIGN_00..07 + README). Build plan: `docs/design/LETHE_DESIGN_06_BUILD_PLAN.md`.

## 1. Dev_Prototype_v1 플레이 리뷰 — 최우선

- Priority: highest
- Source: `docs/design/LETHE_DESIGN_06_BUILD_PLAN.md` (M1), `LETHE_DESIGN_05_UI_UX.md`, `LETHE_DESIGN_01_RUN_LOOP.md`
- Include: `Dev_Prototype_v1`을 Play Mode로 열고 쌍검 공격, 피격 피드백, 줄어든 칼무리, XP bar, 큰 3카드 선택 UI를 직접 확인한다. `M1 Smoke`, `M2 Loop`, `F8`도 같이 본다.
- Done: jaewoo가 M1 전투 손맛에 대해 `GO`, `ITERATE`, `NO-GO` 중 하나를 준다.

## 2. 기본공격/피격효과 설계 확정

- Priority: highest
- Include: 쌍검/대검 각각의 기본공격 리듬, 범위, hitstop, knockback, 피격 flash, 잔향 발동 방식 차이를 문서와 구현에 맞춘다.
- Done: 무기별 기본공격이 "각 무기의 재미"로 기능한다.

## 3. M2 실제 페이싱화

- Priority: high
- Include: 현재 `M2 Loop` 압축 주입으로 검증한 쌍검 + 굶주린 칼무리 + 피의 반사 -> 첫 보스 -> 첫 망각 -> 결손 생존 -> 공명 -> 칼무리/혈반 +5 -> 피의 칼폭풍을 실제 플레이 페이싱으로 연결한다.
- Done: 디버그 버튼 없이도 60~120초 안에 같은 감정 루프가 도달된다.

## 4. ScriptableObject 데이터화 + 클래스 연결

- Priority: medium
- Source: `LETHE_DESIGN_06_BUILD_PLAN.md`, `LETHE_DESIGN_02/03/04` 수치표
- Include: v1 manager 하드코딩을 `_dev/Data` Definition 자산으로 분리(Weapon/Memory/Echo/EchoSynergy/Enemy).
- Done: 매니저가 코드 분기 대신 데이터에서 id/레벨/수치를 읽는다.

## 5. 에셋·클래스 연결 (07 기준)

- Priority: low (M2 GO 이후)
- Include: 대검 → 6기억/6잔향 VFX → 3궁극 → 적 4dir/보스 순(07 §12 우선순위).
- Done: 8/8/4+대검+적4가 각각 sprite·prefab·클래스에 연결되고 line renderer 의존 제거.
