# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.

Current source of truth: `docs/design/` (LETHE_DESIGN_00..07 + README). Build plan: `docs/design/LETHE_DESIGN_06_BUILD_PLAN.md`.

## 1. Dev_Prototype_v1 무기 선택/타격감 통합 리뷰 — 최우선

- Priority: highest
- Source: `docs/design/LETHE_DESIGN_06_BUILD_PLAN.md` (M1), `LETHE_DESIGN_05_UI_UX.md`, `LETHE_DESIGN_01_RUN_LOOP.md`
- Include: `Dev_Prototype_v1`을 Play Mode로 열고 시작 무기 선택, 카드 선택 중 적 정지, 커진 쌍검 반달 2연 베기, 범위만큼 커진 얇은 대검 반달 베기, hitstop 중 캐릭터 이동감, 적 흰색 피격 플래시, 데미지 숫자, 원거리몹 정지 사격, 적 넉백/피격감, 칼무리 후속 반달타, 망각/공명/+5 잔향/피의 칼폭풍, HUD, 전투 밀도를 한 번에 확인한다. 무기/베기/VFX 값은 `_dev/Data/Weapons` SO에서 조절한다.
- Done: jaewoo가 통합 빌드에 대해 `GO`, `ITERATE`, `NO-GO` 중 하나와 가장 약한 지점 1~3개를 준다.

## 2. 피드백 기반 1순위 보정

- Priority: high
- Include: 통합 리뷰에서 나온 가장 약한 지점 하나를 먼저 고친다. 후보는 무기 선택 UX, pause/hitstop 감각, 반달 slash 타격감, 넉백/피격감, 페이싱/밸런스, UI 가독성, 아트/스프라이트 교체다.
- Done: 선택한 약점 하나가 스모크와 플레이 리뷰에서 개선된다.

## 3. 대검 아트 교체 패스

- Priority: high
- Include: 현재 절차형 대검 visual을 임시 박스가 아니라 LETHE 톤의 대검 sprite/VFX로 교체한다.
- Done: `F9` 대검이 시각적으로도 별도 무기처럼 보인다.

## 4. M2 실제 페이싱화

- Priority: high
- Include: 현재 `M2 Loop` 압축 주입으로 검증한 쌍검 + 굶주린 칼무리 + 피의 반사 -> 첫 보스 -> 첫 망각 -> 결손 생존 -> 공명 -> 칼무리/혈반 +5 -> 피의 칼폭풍을 실제 플레이 페이싱으로 연결한다.
- Done: 디버그 버튼 없이도 60~120초 안에 같은 감정 루프가 도달된다.

## 5. 기억/잔향/적 ScriptableObject 데이터화 + 클래스 연결

- Priority: medium
- Source: `LETHE_DESIGN_06_BUILD_PLAN.md`, `LETHE_DESIGN_02/03/04` 수치표
- Include: 무기/베기/VFX 데이터화는 완료됐으므로, 다음 데이터화는 Memory/Echo/EchoSynergy/Enemy/encounter pacing을 `_dev/Data` Definition 자산으로 분리한다.
- Done: 기억/잔향/적 수치도 매니저 코드 분기 대신 데이터에서 id/레벨/수치를 읽는다.
