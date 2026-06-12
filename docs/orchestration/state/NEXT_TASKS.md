# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.

Current source of truth: `docs/design/` (LETHE_DESIGN_00..06 + README). Build plan: `docs/design/LETHE_DESIGN_06_BUILD_PLAN.md`.

## 1. Dev_Prototype_v1 M1 체감 보강 — 최우선

- Priority: highest
- Source: `docs/design/LETHE_DESIGN_06_BUILD_PLAN.md` (M1), `LETHE_DESIGN_05_UI_UX.md`, `LETHE_DESIGN_01_RUN_LOOP.md`
- Include: `Dev_Prototype_v1` 기준으로 카메라/프레이밍, 적 추격 체감, 쌍검 판정, XP/레벨업 선택, HUD 읽힘을 다듬는다. v0는 더 이상 main path가 아니다.
- Done: jaewoo가 Play Mode에서 "게임 셸은 이제 됐다"고 판단할 수 있다.

## 2. M2 1코어 수직 슬라이스

- Priority: highest
- Include: 쌍검 + 굶주린 칼무리 + 피의 반사 → 첫 보스(2050) → 첫 망각 → 망각 결과 화면 → 결손 생존 → 공명 → 칼무리/혈반 +5 → 피의 칼폭풍.
- Done: "Unity도 HTML만큼 된다"가 증명됨.

## 3. Debug smoke injector

- Priority: high
- Include: v1에서 keyboard 없이 MCP/OnGUI로 level-up, forced forget, echo +5, Blood Blade Storm, Gatekeeper를 즉시 만들 수 있는 상태 주입 API.
- Done: 자동 스모크가 player/enemy/memory/echo/ultimate 상태를 읽어 회귀 확인 가능.

## 4. ScriptableObject 데이터화 + 클래스 연결

- Priority: medium
- Source: `LETHE_DESIGN_06_BUILD_PLAN.md`, `LETHE_DESIGN_02/03/04` 수치표
- Include: v1 manager 하드코딩을 `_dev/Data` Definition 자산으로 분리(Weapon/Memory/Echo/EchoSynergy/Enemy).
- Done: 매니저가 코드 분기 대신 데이터에서 id/레벨/수치를 읽는다.

## 5. 에셋·클래스 연결 (07 기준)

- Priority: low (M2 GO 이후)
- Include: 대검 → 6기억/6잔향 VFX → 3궁극 → 적 4dir/보스 순(07 §12 우선순위).
- Done: 8/8/4+대검+적4가 각각 sprite·prefab·클래스에 연결되고 line renderer 의존 제거.
