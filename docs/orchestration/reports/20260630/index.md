# 2026-06-30-01 - 보스 패턴 / 망각 재획득 제거 패치

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에 jaewoo 플레이 피드백을 반영한 보스/망각/체력바/칼무리 패치를 적용했다. Unity 컴파일 에러와 콘솔 에러는 `0`이다.

## 2. 오늘 바뀐 것

- 2번째 문지기 타이밍을 `360s`에서 `300s`로 당겼다.
- 문지기 스케줄을 `150 / 300 / 540 / 900s`로 바꿨다.
- 문지기 HP를 `2200 / 4200 / 7600 / 12800`으로 올렸다.
- 문지기에게 주기적인 파문/방어 패턴을 추가했다.
- 망각 후 기억 재획득/보충 메커니즘을 제거했다.
- 체력바가 적 스케일/스쿼시에 같이 찌그러지지 않게 보정했다.
- 칼무리 궤도 VFX가 hitstop 중에도 계속 돌도록 바꿨고, 생성 빈도를 줄여 최적화했다.

## 3. 테스트 결과와 근거

- `node scripts/balance_curve_v1.js`: 통과.
- `node scripts/verify_unity_stepped_balance.js`: 통과.
- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 경고 0 / 오류 0.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 통과, legacy 경고 7 / 오류 0.
- Unity MCP compile error count: `0`.
- Unity MCP console error count: `0`.
- 근거 파일: `docs/orchestration/evidence/2026-06-30-boss-pattern-no-reacquire-qa.md`.

## 4. 결정한 것

현재 프로토타입에서는 망각 후 다시 같은 기억을 얻는 메커니즘을 제거한다. 재미 판단은 “잃은 기억이 잔향으로 남아 빌드를 바꾸는가”에 집중한다.

## 5. 문제 또는 리스크

MCP 메뉴 스모크는 Play Mode에 들어갔지만 브리지 재시작/응답 파싱 문제 때문에 `[V1QA] PASS` 로그를 확보하지 못했다. 다음 직접 플레이에서 체감 QA가 필요하다.

## 6. GPT/Claude 인계 요약

첫 보스 타이밍은 유지하고, 두 번째 보스만 빠르게 당겼다. 보스 TTK는 HP 증가와 패턴으로 보정했다. 망각 후 재획득은 완전히 제거했으며, 체력바와 칼무리 hitstop 시각 문제를 함께 고쳤다.

## 7. 다음 Codex 작업

jaewoo가 다시 플레이한 뒤 첫 보스 HP, 2보스 타이밍, 문지기 파문 피해/빈도, 4보스 HP 중 하나만 좁게 조정한다.

## 8. 포트폴리오 메모

- 문제: 보스가 너무 빨리 죽고 패턴이 없어 프리딜이 됐으며, 망각 뒤 재획득 루프가 불필요하게 느껴졌다.
- 방향: 새 콘텐츠 추가 대신 기존 보스/망각 루프를 플레이 피드백 기준으로 좁게 수정했다.
- 행동: 스케줄, HP, 보스 패턴, 망각 UX, 체력바, 칼무리 VFX/최적화를 패치했다.
- 결과: 기술 검증은 통과했고, 다음 판단은 직접 플레이 감각으로 좁혀졌다.

# 2026-06-30-02 - 인트로 / 무기 선택 UI 게임 셸 강화

## 1. 현재 빌드 상태

`Dev_Prototype_v1`의 시작 화면이 기존의 단순 무기 카드에서 LETHE 타이틀, 첫 목표 안내, 무기별 리듬 요약, 조작 안내가 있는 게임형 인트로 화면으로 바뀌었다.

## 2. 오늘 바뀐 것

- 시작 화면에 `LETHE` 타이틀과 어두운 풀스크린 배경, 상하단 컬러 라인을 추가했다.
- “첫 목표” 안내 줄을 넣어 XP, 기억 3칸, 첫 문지기, 잔향 변환 흐름을 바로 읽게 했다.
- 절단쌍검/장송대검 카드에 숫자 키 배지, 무기 glyph, 리듬 요약, 선택 안내를 추가했다.
- 현재 기획대로 시작은 무기만 고르고, 기억은 첫 보상 카드에서 선택하는 흐름을 유지했다.
- 낮은 Game View 높이에서도 겹치지 않도록 compact 레이아웃을 추가했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy 경고 7 / 오류 0.
- Unity MCP `Assets/Refresh`: 통과.
- Unity compile error count: `0`.
- Unity Play Mode intro capture 중 console error count: `0`.
- 시각 근거: `LETHE/Assets/_dev/Evidence/v1_intro_weapon_select_ui_20260630_v2.png`.

## 4. 결정한 것

이번 작업은 새 시스템 추가가 아니라 “게임처럼 보이는 첫 화면”을 만드는 범위로 제한했다. 시작 기억 선택은 아직 첫 보상 카드 흐름에 둔다.

## 5. 문제 또는 리스크

OnGUI 기반 UI라 출시급 UI는 아니다. 다만 현재 `_dev` 직접 플레이용으로는 제목, 선택 카드, 첫 목표, 조작 안내가 한 화면에서 읽힌다.

## 6. GPT/Claude 인계 요약

인트로는 이제 무기 2종의 차이를 첫 화면에서 보여준다. 다음 리뷰에서는 실제 시작 화면을 보고 “무기만 고르는 모델이 충분히 명확한지”와 “첫 목표 안내가 과하지 않은지”를 확인하면 된다.

## 7. 다음 Codex 작업

jaewoo 직접 플레이에서 시작 화면을 확인한 뒤, 필요한 경우 시작 기억 후보까지 보여줄지 또는 현재처럼 첫 보상 카드에 남길지 결정한다.

## 8. 포트폴리오 메모

- 문제: 프로토타입 시작 화면이 기능은 있지만 게임 첫 화면처럼 보이지 않았다.
- 방향: 전투 로직을 건드리지 않고 시작 경험의 정보 구조와 분위기를 보강했다.
- 행동: 타이틀, 목표 안내, 무기 카드, responsive compact 레이아웃, 캡처 검증을 추가했다.
- 결과: 낮은 Game View에서도 겹치지 않는 인트로 화면을 확보했다.

# 2026-06-30-03 - 굶주린 칼무리 VFX 재강화

## 1. 현재 빌드 상태

`Dev_Prototype_v1`의 굶주린 칼무리가 다시 칼날 군집처럼 읽히도록 활성 기억 VFX와 적중 지점 bite VFX를 강화했다.

## 2. 오늘 바뀐 것

- 칼무리 궤도 칼날을 정적인 짧은 표시가 아니라 이동하는 스윕 칼날로 바꿨다.
- 바깥 칼날 고리의 반경, 스케일, 알파, 수명을 키웠다.
- 바깥 고리에 밝은 lead blade를 추가해 “칼날 무리” 실루엣을 더 선명하게 했다.
- 플레이어 주변 outer trace ring을 강화했다.
- 적중 지점 bite 효과에 더 많은 칼날, 큰 halo, target-local cut trace를 추가했다.
- 칼무리 기억 획득 순간도 ring + 12개 orbiting blade로 더 강하게 보이게 했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy 경고 7 / 오류 0.
- Unity MCP `Assets/Refresh`: 통과.
- Unity compile error count: `0`.
- Unity forced Hungry Blades +5 Play Mode 확인 중 console error count: `0`.
- 강제 상태: `절단쌍검`, `HungryBlades:5`, 근처 적 18마리.
- 시각 근거:
  - `LETHE/Assets/_dev/Evidence/v1_hungry_blades_kalmuri_refresh_20260630.png`
  - `LETHE/Assets/_dev/Evidence/v1_hungry_blades_kalmuri_refresh_camera_20260630.png`

## 4. 결정한 것

칼무리는 최적화 때문에 너무 얇게 보이면 안 된다. 활성 기억 칼무리는 “계속 도는 칼날 군집”을 우선하고, 잔향 칼무리는 무기 타격 지점의 후속 칼날로 구분한다.

## 5. 문제 또는 리스크

칼날 수와 수명이 늘어났으므로 후반 대량 전투에서 VFX 밀도/성능을 다시 볼 필요가 있다. 현재는 pooled transient sprite 경로를 쓰므로 기술 리스크는 제한적이다.

## 6. GPT/Claude 인계 요약

jaewoo가 칼무리가 멋없어졌다고 피드백했다. 이번 패치는 수치/피해를 바꾸지 않고, 칼무리의 시각 정체성을 더 강한 cyan blade swarm으로 되돌렸다.

## 7. 다음 Codex 작업

직접 플레이에서 칼무리 +1/+3/+5 단계가 모두 충분히 구분되는지 확인하고, 필요하면 +1은 얇게, +5는 더 폭풍처럼 계층 차이를 만든다.

## 8. 포트폴리오 메모

- 문제: 최적화/축약 이후 핵심 기억인 굶주린 칼무리가 밋밋하게 보였다.
- 방향: 새 기능 추가 대신 핵심 판타지인 “칼날 군집”의 시각 언어를 복구했다.
- 행동: 궤도 스윕 칼날, lead blade, bite 칼날, 획득 VFX를 강화하고 Unity에서 강제 상태 캡처로 확인했다.
- 결과: 플레이어 주변에 더 명확한 칼날 고리가 생겨 칼무리 정체성이 다시 살아났다.

# 2026-06-30-04 - 칼무리 C/D 후보 스프라이트 에셋

## 1. 현재 빌드 상태

`Dev_Prototype_v1`의 칼무리 후보 C/D를 Unity Project 창에서 직접 볼 수 있는 투명 PNG Sprite와 프리팹으로 만들었다. 런타임 전투 VFX는 아직 바꾸지 않았고, jaewoo 선택용 후보 에셋 단계다.

## 2. 오늘 바뀐 것

- C / Crescent Pack 후보를 상시 칼무리 오라용 투명 Sprite로 생성했다.
- D / Predator Bite 후보를 타격 순간 돌진/물어뜯기 프레임용 투명 Sprite로 생성했다.
- 각 후보를 바로 씬에 끌어놓을 수 있는 개별 프리팹으로 만들었다.
- C/D를 나란히 볼 수 있는 `Preview_Kalmuri_C_D_SpriteCandidates.prefab`을 만들었다.
- 기존 후보 보드와 C/D 확대 보드, 실제 Sprite 프리팹 프리뷰 캡처를 증거 이미지로 남겼다.

## 3. 테스트 결과와 근거

- Unity importer 확인:
  - 두 PNG 모두 `Sprite`, `Single`, PPU `256`, alpha transparency enabled.
- Unity compile error count: `0`.
- Unity console error count: `0`.
- 시각 근거:
  - `LETHE/Assets/_dev/Evidence/v1_kalmuri_candidates_20260630.png`
  - `LETHE/Assets/_dev/Evidence/v1_kalmuri_candidates_cd_focus_20260630.png`
  - `LETHE/Assets/_dev/Evidence/v1_kalmuri_cd_sprite_assets_20260630.png`

## 4. 결정한 것

C는 상시로 플레이어 주변을 감싸는 칼무리 오라 후보로, D는 적에게 칼무리가 몰려가 물어뜯는 타격 순간 후보로 분리해 둔다. 최종 런타임 적용은 jaewoo가 Unity에서 후보를 보고 선택한 뒤 진행한다.

## 5. 문제 또는 리스크

현재 후보는 정적 합성 Sprite라 애니메이션/회전/타격 타이밍이 실제 게임에서 어떻게 보일지는 다음 런타임 적용 후 다시 봐야 한다.

## 6. GPT/Claude 인계 요약

칼무리 방향은 C/D로 좁혀졌다. C는 덩어리감 있는 초승달 오라, D는 타격 순간 한 방향 돌진이다. 다음 판단은 `상시=C, 타격=D` 조합이 맞는지, 혹은 C만/D만 쓸지에 대한 jaewoo 선택이다.

## 7. 다음 Codex 작업

jaewoo가 후보를 선택하면 `V1GameManager`의 Hungry Blades 상시 orbit VFX와 enemy-side bite VFX에 선택된 Sprite/프리팹 방향을 반영한다.

## 8. 포트폴리오 메모

- 문제: 칼무리가 코드 VFX로만 튜닝되면서 정체성이 어색하게 느껴졌다.
- 방향: 런타임 수치보다 먼저 시각 후보를 실제 Unity 에셋으로 만들어 선택 가능하게 했다.
- 행동: C/D 후보 Sprite, 개별 프리팹, 비교 프리팹, 증거 이미지를 생성했다.
- 결과: 기획자/플레이어가 Unity 안에서 후보를 직접 보고 다음 적용 방향을 고를 수 있게 됐다.

