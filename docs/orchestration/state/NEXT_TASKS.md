# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.

Current source of truth: `docs/design/` (LETHE_DESIGN_00..07 + README). Build plan: `docs/design/LETHE_DESIGN_06_BUILD_PLAN.md`.

## 1. Dev_Prototype_v1 플레이 리뷰 — 최우선

- Priority: highest
- Source: `docs/design/LETHE_DESIGN_06_BUILD_PLAN.md` (M1), `LETHE_DESIGN_05_UI_UX.md`, `LETHE_DESIGN_01_RUN_LOOP.md`
- Include: `Dev_Prototype_v1`을 Play Mode로 열고 쌍검이 허공에 베지 않는지, 기본공격 VFX가 적 위치 발도/절단선으로 보이는지, 칼무리 `MultiSmall`이 적중 위치 후속타로 읽히는지 확인한다. XP bar/3카드 UI도 같이 본다.
- Done: jaewoo가 target-local basic attack + echo follow-up에 대해 `GO`, `ITERATE`, `NO-GO` 중 하나를 준다.

## 2. 쌍검 target-local VFX 체감 보정

- Priority: high
- Source: `DEC-2026-06-12-05`, `LETHE_DESIGN_02_COMBAT.md`, `LETHE_DESIGN_03_MEMORY_ECHO.md`
- Include: 플레이 리뷰에서 공격감이 약하면 발도선 크기/위치, 보조 slash 수, 칼무리 지연 시간, hitstop 값을 조정한다. 코드 구현은 완료됐으므로 수치와 화면 리듬 보정에 한정한다.
- Done: jaewoo가 쌍검 기본공격과 칼무리 잔향 후속타를 구분해서 읽을 수 있다고 판단한다.

## 3. 대검 공격 구조 준비

- Priority: high
- Include: `DensestArc`, `SingleHeavy`, `FewHeavy`를 구현할 수 있게 weapon targeting/echo proc 구조를 분리한다.
- Done: 대검을 추가할 때 쌍검 코드를 복사하지 않고 무기 정의/전략으로 갈아낄 수 있다.

## 4. M2 실제 페이싱화

- Priority: high
- Include: 현재 `M2 Loop` 압축 주입으로 검증한 쌍검 + 굶주린 칼무리 + 피의 반사 -> 첫 보스 -> 첫 망각 -> 결손 생존 -> 공명 -> 칼무리/혈반 +5 -> 피의 칼폭풍을 실제 플레이 페이싱으로 연결한다.
- Done: 디버그 버튼 없이도 60~120초 안에 같은 감정 루프가 도달된다.

## 5. ScriptableObject 데이터화 + 클래스 연결

- Priority: medium
- Source: `LETHE_DESIGN_06_BUILD_PLAN.md`, `LETHE_DESIGN_02/03/04` 수치표
- Include: v1 manager 하드코딩을 `_dev/Data` Definition 자산으로 분리(Weapon/Memory/Echo/EchoSynergy/Enemy).
- Done: 매니저가 코드 분기 대신 데이터에서 id/레벨/수치를 읽는다.
