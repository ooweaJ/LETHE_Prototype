# LETHE 개발 보고서 - 2026-06-10

새 망각 모델을 HTML에 이식하고 jaewoo 1인 체감 테스트용 빌드를 갱신했다.

# 2026-06-10-01 - 새 망각 모델 HTML 게이트

## 1. 현재 빌드 상태

HTML v0.12는 Unity 전환 직전 상태에서 한 번 더 HTML 체감 게이트로 돌아왔다. 이번 라운드는 신규 콘텐츠나 Unity 셋업 없이, 새 망각 모델만 이식한 빌드로 jaewoo 1인 체감 테스트를 하기 위한 준비이며, 패키지는 `dist\lethe-v0.12-playtest`로 갱신됐다.

## 2. 오늘 바뀐 것

- 망각 대상이 의존도 가중 랜덤에서 최고 레벨 활성 기억으로 바뀌었다.
- 최고 레벨 동률은 일반 플레이에서 플레이어가 선택하고, QA/디버그는 최근 강화 기억을 우선 선택한다.
- 기억과 잔향 레벨 상한을 `+5`로 고정했다.
- 잔향 초과분은 버리지 않고 즉시 과부하 폭발과 `ultimateGauge` 로그로 전환한다.
- 잔향이 있는 기억을 다시 얻으면 `1 + floor(echoLevel / 2)`만큼 공명해 높은 레벨로 돌아온다.
- HUD에 다음 망각 후보, 잔향 레벨, `+5` 각성, 과부하/궁극 게이지를 표시했다.
- 즉시 망각, 잔향 `+5`, 궁극 표식 디버그 버튼을 추가했다.
- 새 망각 모델 적용 후 death rate가 올라가 HP 단일 레버를 `190 -> 210`으로 조정했다.

## 3. 테스트 결과와 근거

- `node --check src/game.js`: 통과.
- `node --check alpha_test/src/simulator.js`: 통과.
- `npm run qa:balance`: `GO_BALANCE_BASELINE`, first boss clear `100%`, full clear `60%`, death `40%`, first boss TTK median `20.19s`.
- `npm run balance:loop`: `GO_BALANCE_BASELINE`, first boss clear `80%`, full clear `60%`, death `40%`, first boss TTK median `23.91s`.
- `npm run playtest:package:dry`: 통과.
- `npm run playtest:package`: 통과, `dist\lethe-v0.12-playtest` 갱신.

이번 게이트에서 AI 테스트는 회귀 확인용이고, 최종 판단은 jaewoo 1인 체감 테스트다.

## 4. 결정한 것

`ITERATE_BEFORE_TEST`: Unity 셋업 전에 HTML에 새 망각 모델만 이식하고 1인 체감 테스트를 진행한다.

## 5. 문제 또는 리스크

- 최고 레벨 망각은 트레이드오프를 선명하게 만들지만, 실제 체감이 "아쉽다"가 아니라 "짜증난다"로 흐를 수 있다.
- HP `210`은 회귀 밴드를 회복했지만, 사람 체감에서는 결손 손실이 약해졌는지 확인해야 한다.
- 공명 재획득이 너무 강하면 망각 손실이 약해질 수 있다.

## 6. GPT/Claude 인계 요약

새 모델의 판단 기준은 자동 지표가 아니라 감정 루프다. 테스트에서 봐야 할 것은 최고 레벨 기억 상실의 아쉬움, 잔향으로 바뀐 전투 체감, 재획득 공명의 설렘, 그리고 플레이어가 강화 선택을 망각 위험과 연결해 생각하는지다.

## 7. 다음 Codex 작업

jaewoo 1인 체감 테스트를 진행하고, 관찰 내용을 `docs/orchestration/evidence/` 또는 `playtest_logs/`에 남긴 뒤 보고서와 다음 작업을 갱신한다.

## 8. 포트폴리오 메모

- 문제: 기존 망각은 의존도 기반 확률이라 플레이어가 성장과 상실의 관계를 읽기 어려웠다.
- 방향: "키운 기억이 다음 망각 후보가 된다"는 규칙으로 감정적 트레이드오프를 전면화했다.
- 행동: 기존 8개 기억/2무기 범위 안에서 망각, 잔향, 재획득 규칙만 바꿨다.
- 결과: Unity로 넘어가기 전에 핵심 감정 루프를 HTML에서 직접 체감 검증할 수 있는 빌드가 준비됐다.

# 2026-06-10-02 - 망각 체감 기획 구체화

## 1. 현재 빌드 상태

새 망각 모델은 HTML에 들어갔고 자동 회귀도 통과했지만, 실제 체감은 아직 크게 달라지지 않았다. 그래서 이번 작업은 코드를 더 넣는 것이 아니라, "무엇이 크게 달라져야 하는지"를 기획 문서로 구체화하는 단계다.

## 2. 오늘 바뀐 것

- `docs/design/LETHE_FORGETTING_FEEL_SPEC.md`를 추가했다.
- 망각 루프를 네 장면으로 재정의했다: 손실, 잔향 전투 변화, 공명 재획득, 궁극 잔향 목표.
- design README와 overview가 새 체감 설계서를 읽는 흐름에 포함하도록 갱신했다.
- 핵심 시스템 문서와 Unity slice 명세가 체감 설계서를 먼저 따르도록 연결했다.
- orchestration state를 "1인 테스트 실행"에서 "기획 구체화"로 바꿨다.

## 3. 테스트 결과와 근거

이번 작업은 문서 기획이다. 기준 피드백은 "큰 변화가 없는 것 같다"였고, 그 원인은 규칙보다 장면/연출/후보 노출이 덜 구체화된 것으로 판단했다.

## 4. 결정한 것

- 지금은 구현을 더 밀기보다 기획 구체화를 먼저 한다.
- 다음 구현은 raw system rule이 아니라 player-facing moment를 따라야 한다.
- 첫 쇼케이스 후보는 `굶주린 칼무리 + 피의 반사 -> 피의 칼폭풍`으로 둔다.

## 5. 문제 또는 리스크

- 체감 설계가 승인되지 않으면 다음 구현이 다시 작은 수치/문구 변경에 머물 수 있다.
- HTML에서 체감 패스를 할지, Unity slice 계약으로 넘길지 아직 선택해야 한다.
- 잔향 후보를 너무 강제로 노출하면 망각 손실이 약해질 수 있다.

## 6. GPT/Claude 인계 요약

새 규칙은 구현됐지만 플레이어에게 큰 장면으로 보이지 않았다. 다음 판단은 `LETHE_FORGETTING_FEEL_SPEC.md`의 네 장면이 맞는지 검토하는 것이다. 승인되면 결과 화면 재설계, 잔향 이펙트 강화, 공명 후보 카드, 궁극 목표 HUD, 한 버튼 debug loop로 작업을 쪼갠다.

## 7. 다음 Codex 작업

- `LETHE_FORGETTING_FEEL_SPEC.md`를 검토한다.
- HTML presentation pass로 갈지 Unity first-slice backlog로 갈지 정한다.
- 선택된 방향에 맞춰 구현 작업 단위를 만든다.

## 8. 포트폴리오 메모

- 문제: 시스템은 바뀌었지만 플레이어 체감은 크게 바뀌지 않았다.
- 방향: 코드 추가보다 "플레이어가 기억하는 장면"을 먼저 정의한다.
- 행동: 손실, 잔향, 공명, 궁극 목표의 화면/전투/선택 기준을 문서화했다.
- 결과: 다음 구현이 감정 장면을 목표로 삼을 수 있는 설계 기준이 생겼다.

# 2026-06-10-03 - 무기·기억·잔향 뽕맛 설계

## 1. 현재 빌드 상태

코드는 추가로 변경하지 않았다. 이번 작업은 잔향이 `잔향!` 텍스트나 작은 proc처럼 느껴지는 문제를 해결하기 위해, 무기와 기억과 잔향이 실제 전투에서 어떤 행동을 만드는지 구체화한 기획 작업이다.

## 2. 오늘 바뀐 것

- `docs/design/LETHE_WEAPON_MEMORY_ECHO_SPEC.md`를 추가했다.
- 절단쌍검과 장송대검의 전투 리듬과 잔향 처리 방식을 구분했다.
- 현재 8개 기억마다 활성 기억, 일반 잔향, 각성 잔향, 공명, 뽕맛 순간을 정의했다.
- `피의 칼폭풍`, `처형 각인`, `정지 추적`, `성채 파문`의 궁극 잔향 방향을 구체화했다.
- design README, overview, combat design, content tables, forgetting feel spec이 새 문서를 참조하도록 갱신했다.
- orchestration state와 dashboard/command를 무기·기억·잔향 설계 검토 중심으로 바꿨다.

## 3. 테스트 결과와 근거

이번 작업은 문서 기획이다. 기준 피드백은 "잔향이 기본공격에 그냥 잔향이라고만 나와서 뽕맛이 없다"였다. 따라서 검증 기준은 코드 테스트가 아니라 다음 구현이 이 문서를 따라 구체적인 전투 사건을 만들 수 있는지다.

## 4. 결정한 것

- 잔향은 기본 공격에 붙는 텍스트가 아니라 무기 공격이 새 행동을 하게 만드는 구조로 설계한다.
- 같은 잔향도 쌍검과 대검에서 다르게 보여야 한다.
- 첫 구현 추천은 `절단쌍검 + 굶주린 칼무리 + 피의 반사 -> 피의 칼폭풍`이다.
- 두 번째 대비 축은 `장송대검 + 처형자의 섬광 + 망각의 낙인 -> 처형 각인`으로 둔다.

## 5. 문제 또는 리스크

- 문서는 구체화됐지만, 실제 HTML/Unity 구현에서 이펙트가 약하면 여전히 뽕맛이 없을 수 있다.
- 첫 구현에서 너무 많은 잔향을 만들면 한 조합의 완성도가 떨어진다.
- 무기별 차이를 만들지 않으면 잔향 시스템이 다시 일반 proc처럼 느껴질 수 있다.

## 6. GPT/Claude 인계 요약

잔향은 이제 스탯 보정이나 텍스트 알림이 아니라 무기별 전투 행동으로 설계해야 한다. 쌍검은 작은 잔향을 자주 연쇄시키고, 대검은 조건부 큰 폭발로 터뜨린다. 첫 쇼케이스는 칼무리/혈반/피의 칼폭풍이며, 다음 구현은 이 조합 하나를 화면에서 확실히 맛있게 만드는 데 집중해야 한다.

## 7. 다음 Codex 작업

- `LETHE_WEAPON_MEMORY_ECHO_SPEC.md`를 검토한다.
- 첫 쇼케이스를 확정한다.
- HTML showcase pass 또는 Unity first-slice backlog 중 하나를 선택한다.
- 선택 후 `피의 칼폭풍` 중심 구현 작업을 작은 단위로 쪼갠다.

## 8. 포트폴리오 메모

- 문제: 잔향이 추상적인 텍스트/proc처럼 느껴져 핵심 판타지가 약했다.
- 방향: 무기, 기억, 잔향의 결합을 구체적인 전투 사건으로 설계한다.
- 행동: 8개 기억의 활성/잔향/각성/공명/궁극 방향과 무기별 차이를 문서화했다.
- 결과: 다음 구현이 "수치가 오른다"가 아니라 "빌드가 화면에서 터진다"를 목표로 삼을 수 있게 됐다.

# 2026-06-10-04 - Unity 잔향 시스템 PRD

## 1. 현재 빌드 상태

코드는 변경하지 않았다. 이번 작업은 `LETHE_WEAPON_MEMORY_ECHO_DETAIL.md`의 상세 기획을 Unity 구현 계약으로 바꾸는 문서화 작업이다. 목표는 Unity를 열기 전에 클래스, ScriptableObject, 프리팹, 이벤트 경계, 수락 기준을 정해 잔향이 다시 일반 proc 코드로 흐르지 않게 하는 것이다.

## 2. 오늘 바뀐 것

- `docs/design/LETHE_ECHO_FORM_TRANSFORMATION_SPEC.md`를 추가했다.
- 활성 기억, 망각 변환, 일반 잔향, 각성 잔향, 공명, 궁극 잔향의 화면 형태 차이를 정의했다.
- `WeaponHit`, `EchoHit`, `UltimateHit`, `Kill`, `DamageTaken`, `ShieldBreak` 이벤트 경계를 정의해 무한 온힛 루프를 막는 기준을 세웠다.
- `docs/design/LETHE_UNITY_ECHO_SYSTEM_PRD.md`를 추가했다.
- Unity 첫 slice의 데이터 SO, 런타임 클래스, 피드백 서비스, 풀링, UI, 디버그 패널, 프리팹 목록을 정리했다.
- `LETHE_WEAPON_MEMORY_ECHO_DETAIL.md`와 design README가 새 문서를 참조하도록 갱신했다.
- orchestration state와 dashboard/command를 Unity echo PRD 리뷰와 구현 표면 결정 중심으로 바꿨다.

## 3. 테스트 결과와 근거

이번 작업은 문서/설계 작업이다. 검증은 보고서 생성과 보고서 구조 검증으로 진행한다.

## 4. 결정한 것

- 잔향은 "약해진 같은 효과"가 아니라 "형태가 바뀐 효과"로 구현한다.
- `+5` 잔향은 활성 기억 복사본이 아니라 무기에 붙은 반각성 형태여야 한다.
- Unity 구현은 `WeaponHit`, `EchoHit`, `UltimateHit`를 분리해 재귀 proc 루프를 막는다.
- 첫 Unity slice는 `절단쌍검 + 굶주린 칼무리 + 피의 반사 -> 피의 칼폭풍`을 기준으로 설계한다.

## 5. 문제 또는 리스크

- 아직 HTML showcase로 한 번 더 볼지, Unity backlog로 바로 넘길지 결정이 필요하다.
- `피의 칼폭풍`이 상시 궁극인지 게이지 발동인지 열어둔 상태다.
- 혈반/궁극 회복량 cap을 정하지 않으면 쌍검 타수와 결합해 생존 밸런스가 터질 수 있다.

## 6. GPT/Claude 인계 요약

새 문서의 핵심은 Unity 구현 전에 형태 변환과 이벤트 경계를 고정하는 것이다. 활성 칼무리는 독립 고리, 잔향 칼무리는 무기에서 튀는 칼선, 공명은 둘이 동시에 존재하는 상태, 피의 칼폭풍은 칼날과 혈반이 서로를 먹여 살리는 루프로 정의한다.

## 7. 다음 Codex 작업

- `LETHE_ECHO_FORM_TRANSFORMATION_SPEC.md`와 `LETHE_UNITY_ECHO_SYSTEM_PRD.md`를 검토한다.
- 다음 표면을 고른다: HTML showcase pass 또는 Unity first-slice backlog.
- Unity를 선택하면 데이터 SO, hit event router, echo runtime, pooling, feedback service, debug panel 순서로 backlog를 쪼갠다.

## 8. 포트폴리오 메모

- 문제: 좋은 잔향 기획도 구현 구조가 없으면 일반 proc 코드로 흐를 수 있다.
- 방향: 재미 기획을 Unity 클래스/데이터/프리팹/이벤트 계약으로 변환한다.
- 행동: 형태 변환 문법과 Unity echo system PRD를 작성했다.
- 결과: 첫 Unity slice가 "무엇을 어떻게 만들지"까지 내려간 구현 준비 상태가 됐다.

# 2026-06-10-05 - 첫 스프라이트/VFX 콘셉트 시트

## 1. 현재 빌드 상태

코드는 변경하지 않았다. Unity 2D 프로젝트를 만들기 전에, 첫 slice의 스프라이트/VFX 방향을 잡는 콘셉트 이미지와 에셋 계획 문서를 추가했다.

## 2. 오늘 바뀐 것

- Codex imagegen으로 첫 echo showcase 콘셉트 시트를 생성했다.
- 생성 이미지를 `docs/design/assets/lethe-first-echo-showcase-concept.png`에 보관했다.
- `docs/design/LETHE_VISUAL_ASSET_PLAN.md`를 추가했다.
- 콘셉트 이미지의 각 영역을 Unity 프리팹 후보와 연결했다: 쌍검, 칼무리 고리, 지연 칼선, 혈반 표식, 회복 실, 피의 칼폭풍.
- Unity MCP가 연결된 뒤 어떤 폴더에 import하고 어떤 prefab에 연결할지 계획을 적었다.
- design README와 orchestration state를 비주얼 에셋 계획까지 포함하도록 갱신했다.

## 3. 테스트 결과와 근거

이번 작업은 이미지/문서 기획이다. 생성 이미지는 직접 확인했고, 런타임 최종 스프라이트가 아니라 첫 Unity slice의 아트 방향 참조로 쓰는 것이 적절하다고 판단했다.

## 4. 결정한 것

- 지금 생성한 이미지는 최종 투명 스프라이트가 아니라 콘셉트 시트로 사용한다.
- 실제 Unity 런타임에는 칼선, 혈반, 회복 실, 발사 칼날, 궁극 칼날을 투명 PNG 파츠로 별도 생성한다.
- Unity MCP는 이미지 생성이 아니라 import, prefab 연결, scene 배치에 사용한다.

## 5. 문제 또는 리스크

- 콘셉트 이미지는 디테일이 많아 축소하면 뭉칠 수 있다.
- 최종 sprite는 더 단순한 실루엣과 높은 대비가 필요하다.
- 투명 PNG 생성/크로마키 제거는 다음 별도 asset pass로 처리해야 한다.

## 6. GPT/Claude 인계 요약

첫 비주얼 방향은 흰 금속 칼날, 어두운 잔향, 붉은 혈반/피실 조합이다. `피의 칼폭풍`은 칼날 고리와 붉은 실이 같이 도는 궁극 형태로 잡혔다. 다음 판단은 이 콘셉트를 기준으로 투명 runtime sprite를 만들지, Unity 프로젝트를 먼저 만들지다.

## 7. 다음 Codex 작업

- `LETHE_VISUAL_ASSET_PLAN.md`를 검토한다.
- 다음 asset pass를 고른다: 투명 runtime sprites, 추가 콘셉트 변형, 또는 Unity 2D 프로젝트 생성.
- 투명 sprite를 만들 경우 우선순위는 칼무리 반달 칼선, 혈반 표식, 회복 실 끝점, +5 발사 칼날, 피의 칼폭풍 칼날이다.

## 8. 포트폴리오 메모

- 문제: Unity 구현 전에 잔향의 시각 방향이 없으면 좋은 구조도 밋밋하게 보일 수 있다.
- 방향: 첫 slice의 핵심 VFX를 한 장의 콘셉트 시트로 고정한다.
- 행동: Codex imagegen으로 콘셉트 이미지를 만들고, Unity prefab/import 계획으로 연결했다.
- 결과: Unity 프로젝트 생성 전에 클래스, 프리팹, 이미지 방향이 함께 맞물린 상태가 됐다.

# 2026-06-10-06 - Unity 에셋-프리팹 연결 지도

## 1. 현재 빌드 상태

코드는 변경하지 않았다. 이번 작업은 기존 콘셉트 이미지가 무기/VFX 중심이라는 점을 반영해, Unity MCP가 실제 프로젝트에서 따라 할 수 있는 파일-프리팹-Scene 연결 지도를 추가한 문서화 작업이다.

## 2. 오늘 바뀐 것

- `docs/design/LETHE_UNITY_ASSET_BINDING_PLAN.md`를 추가했다.
- 현재 사용 가능한 이미지 `docs/design/assets/lethe-first-echo-showcase-concept.png`는 런타임 sprite atlas가 아니라 `Art/Concept/` 참조용으로 정의했다.
- 캐릭터, 맵, 절단쌍검, 칼무리, 혈반, 피의 칼폭풍, 적, UI가 각각 어떤 파일/프리팹/SO에 연결되는지 표로 정리했다.
- 아직 없는 캐릭터/맵/적 이미지는 placeholder로 시작할 수 있게 표시했다.
- Unity MCP 실행 순서를 폴더 생성, 이미지 import, material 생성, prefab 생성, SO 생성, scene 배치, debug 검증 순서로 적었다.
- design README, visual asset plan, Unity echo PRD, orchestration state를 새 연결 문서 기준으로 갱신했다.

## 3. 테스트 결과와 근거

이번 작업은 문서 기획이다. 검증은 보고서 생성과 보고서 구조 검증으로 진행한다.

## 4. 결정한 것

- 방금 만든 콘셉트 이미지는 Unity에 넣을 수 있지만 `Assets/Lethe/Art/Concept/` 참조용으로만 사용한다.
- 실제 게임 런타임에는 개별 투명 PNG 스프라이트와 material을 prefab에 연결한다.
- 첫 Unity slice에서 캐릭터/맵/적은 placeholder로 시작해도 되고, 이펙트 파츠가 우선이다.
- Unity MCP는 이 문서를 기준으로 import, prefab 연결, SO 연결, scene 배치를 수행한다.

## 5. 문제 또는 리스크

- 아직 투명 runtime sprite가 없기 때문에 실제 VFX prefab은 placeholder 또는 다음 imagegen pass가 필요하다.
- 캐릭터/맵을 너무 늦게 만들면 화면 전체 인상이 약할 수 있지만, 첫 검증은 잔향 VFX가 우선이다.
- 콘셉트 시트를 잘라 런타임에 바로 쓰면 축소 시 디테일이 뭉칠 수 있다.

## 6. GPT/Claude 인계 요약

Unity로 넘어갈 때는 `LETHE_UNITY_ASSET_BINDING_PLAN.md`를 먼저 읽으면 된다. 이 문서는 캐릭터는 어떤 파일, 맵은 어떤 파일, 검은 어떤 파일, 칼무리/혈반/피의 칼폭풍은 어떤 VFX 파일과 prefab을 쓰는지 연결한다. 현재 이미지는 참조용이고, 런타임 sprite는 별도 생성 대상이다.

## 7. 다음 Codex 작업

- `LETHE_UNITY_ASSET_BINDING_PLAN.md`를 검토한다.
- 다음 중 하나를 고른다: Unity 2D 프로젝트 생성, 또는 투명 runtime sprite 3~5개 생성.
- Unity를 선택하면 MCP로 `Assets/Lethe/` 구조를 만들고 콘셉트 이미지를 `Art/Concept/`에 import한다.

## 8. 포트폴리오 메모

- 문제: 콘셉트 이미지만 있으면 Unity에서 무엇에 연결할지 모호하다.
- 방향: 이미지 파일을 캐릭터, 맵, 무기, VFX, UI 프리팹과 직접 연결한다.
- 행동: Unity MCP 실행용 asset binding map을 작성했다.
- 결과: Unity 프로젝트 생성 시 문서를 보고 폴더/import/prefab/SO/scene 순서로 실행할 수 있게 됐다.

# 2026-06-10-07 - Unity 2D skeleton 커밋 준비

## 1. 현재 빌드 상태

Unity 2D 프로젝트가 `LETHE/` 아래 생성됐다. 이번 작업은 gameplay 구현이 아니라, Unity baseline을 안전하게 버전관리하기 위한 정리다.

## 2. 오늘 바뀐 것

- AnkleBreaker Unity MCP 서버 경로를 확인했다: `C:\jaewoo\unity-mcp-server\src\index.js`.
- Codex MCP에 `anklebreaker-unity`를 등록했다.
- Unity Editor bridge `127.0.0.1:7890` 연결을 확인했다.
- 이전 gamelovers/HTTP Unity MCP 등록인 `mcp-unity`, `unityMCP`를 제거했다.
- `.gitignore`에 Unity 생성물 제외 규칙을 추가했다.
- 이전 MCP 설정으로 보이는 `LETHE/ProjectSettings/McpUnitySettings.json`을 제거했다.
- Unity skeleton 커밋 대상은 `LETHE/Assets`, `LETHE/Packages`, `LETHE/ProjectSettings`로 좁혔다.

## 3. 테스트 결과와 근거

- `codex mcp list`: `anklebreaker-unity` 등록 확인.
- `Test-NetConnection 127.0.0.1 -Port 7890`: `TcpTestSucceeded: True`.
- Unity instance registry: `LETHE`, Unity `6000.3.10f1`, project path `C:/jaewoo/LETHE_Prototype/LETHE`, port `7890`.
- `git status --short --untracked-files=all`: Unity generated folders are ignored; project skeleton files remain as tracking candidates.

## 4. 결정한 것

- Unity MCP는 AnkleBreaker 서버를 기준으로 사용한다.
- Unity generated cache/build/editor files는 커밋하지 않는다.
- Unity 기본 skeleton은 커밋해 이후 MCP 작업의 기준점으로 삼는다.
- 첫 MCP 작업은 `LETHE_UNITY_ASSET_BINDING_PLAN.md`를 기준으로 `Assets/Lethe/` 구조와 reference art import부터 시작한다.

## 5. 문제 또는 리스크

- 현재 Codex 세션에는 AnkleBreaker MCP 도구가 아직 callable로 노출되지 않았다. 새 MCP 등록 후 세션/tool metadata reload가 필요할 수 있다.
- Unity skeleton은 기본 템플릿 상태라 실제 `Assets/Lethe/` 구조는 아직 없다.
- 플레이어/맵/적/런타임 VFX sprite는 아직 placeholder 또는 다음 imagegen pass가 필요하다.

## 6. GPT/Claude 인계 요약

Unity 프로젝트는 이제 버전관리 baseline으로 들어갈 준비가 됐다. AnkleBreaker MCP 연결은 포트 `7890`에서 확인됐고, 이전 MCP 등록은 제거됐다. 다음 구현은 Unity MCP 도구가 세션에 노출된 뒤 `LETHE_UNITY_ASSET_BINDING_PLAN.md`를 읽고 폴더/import/prefab/SO/scene 순서로 진행하면 된다.

## 7. 다음 Codex 작업

- Unity skeleton을 커밋/푸시한다.
- 새 세션 또는 MCP tool reload 후 AnkleBreaker Unity MCP 도구 노출을 확인한다.
- `Assets/Lethe/` 구조 생성과 콘셉트 이미지 reference import를 시작한다.

## 8. 포트폴리오 메모

- 문제: Unity 프로젝트를 만들었지만 생성물과 실제 프로젝트 파일이 섞이면 repo가 무거워지고 재현성이 떨어진다.
- 방향: Unity generated files를 제외하고, project skeleton만 baseline으로 버전관리한다.
- 행동: MCP 연결과 gitignore를 정리하고 Unity skeleton 커밋 범위를 확정했다.
- 결과: 이후 Unity MCP 작업을 안정적으로 쌓을 기준점이 생겼다.

# 2026-06-10-08 - Unity slice 이미지 제작 플랜

## 1. 현재 빌드 상태

코드와 Unity gameplay는 변경하지 않았다. Unity skeleton과 `Assets/_dev` 작업 루트가 준비된 상태에서, 첫 Unity slice를 imagegen + MCP 루프로 만들기 위한 제작 계획을 구체화했다.

## 2. 오늘 바뀐 것

- `docs/design/LETHE_UNITY_SLICE_ASSET_PRODUCTION_PLAN.md`를 추가했다.
- 첫 slice를 `Assets/_dev`에서 실험하고, 검증된 구조만 `Assets/Lethe`로 승격하는 원칙을 정했다.
- 이미지 제작 대상을 Batch A~E로 나눴다: 기본 판독, 쌍검, 칼무리, 혈반, 피의 칼폭풍.
- 첫 slice 이미지 콘셉트를 고정했다: 어두운 판독용 캐릭터/맵/적 위에 흰 칼날, 청백 잔상, 붉은 혈반/피실이 우선 보이도록 한다.
- asset별 imagegen 프롬프트 팩을 추가했다.
- 이미지가 어느 프리팹과 주 클래스에 연결되는지 매트릭스로 정리했다.
- 먼저 만들 이미지 5개를 확정했다: player silhouette, walker enemy, dark floor tile, left/right dual blades.
- 다음 잔향 VFX 5개를 확정했다: 칼무리 반달 칼선, 고리 칼날, 발사 칼날, 혈반 표식, 회복 실 끝점.
- 이미지 콘셉트 고정표, imagegen 프롬프트 팩, 프리팹/클래스 연결 매트릭스를 추가했다.
- MCP 구현 순서를 `_dev` 폴더 생성, PNG import, asset 인식 확인, import setting, `Dev_EchoSlice.unity`, debug panel 순서로 정리했다.
- design README, visual plan, asset binding plan, orchestration state, dashboard, command page를 새 플랜 기준으로 갱신했다.

## 3. 테스트 결과와 근거

이번 작업은 구현 전 제작 계획이다. 검증은 문서 간 연결과 보고서 생성/검증으로 진행한다.

## 4. 결정한 것

- 첫 Unity slice는 게임 전체가 아니라 잔향 뽕맛 검증 장치로 만든다.
- 이미지 우선순위는 `잔향 VFX > 무기 > 적 > 캐릭터 > 맵`이지만, 첫 scene 판독을 위해 player/enemy/floor/weapon 5개를 먼저 만든다.
- 런타임 파츠는 파일 1개 = 역할 1개로 만든다.
- 실험은 `Assets/_dev`에서 진행하고, 확정 전에는 `Assets/Lethe`로 승격하지 않는다.

## 5. 문제 또는 리스크

- imagegen 결과가 투명 PNG로 바로 나오지 않으면 chroma-key 제거와 alpha 검증이 필요하다.
- 너무 예쁜 캐릭터/맵을 먼저 만들면 잔향 VFX 검증이 흐려질 수 있다.
- 현재 계획은 문서이며, 실제 Unity prefab 조립은 다음 MCP 작업에서 검증해야 한다.

## 6. GPT/Claude 인계 요약

다음 작업은 `LETHE_UNITY_SLICE_ASSET_PRODUCTION_PLAN.md`를 기준으로 시작한다. Codex imagegen으로 문서의 프롬프트 팩을 사용해 기본 판독 이미지 5개를 만들고, AnkleBreaker MCP로 `Assets/_dev/Art/Sprites`에 import한 뒤 Unity asset list로 인식 여부를 확인한다. gameplay code는 아직 만들지 않는다.

## 7. 다음 Codex 작업

- 문서의 `Imagegen 프롬프트 팩`으로 기본 판독 이미지 5개를 만든다.
- 생성 결과를 chroma-key 제거/alpha PNG로 정리한다.
- MCP로 `Assets/_dev/Art/Sprites` 하위에 import하고 인식 확인한다.
- 다음 잔향 VFX 5개 제작으로 넘어간다.

## 8. 포트폴리오 메모

- 문제: Unity로 넘어가도 무엇을 먼저 만들어야 잔향의 재미를 검증할지 흐릿했다.
- 방향: 전체 게임이 아니라 잔향 체감 slice를 이미지 단위, 프리팹 단위, 테스트 단위로 쪼갠다.
- 행동: imagegen 제작 목록과 MCP 조립 순서를 하나의 플랜으로 고정했다.
- 결과: 다음 작업은 "이미지 생성 -> Unity import -> debug slice"로 바로 이어질 수 있다.

# 2026-06-10-09 - Unity 게임개발 규약 전환

## 1. 현재 빌드 상태

Unity 2D skeleton과 `Assets/_dev` 작업 루트는 준비되어 있다. 이번 작업은 코드 구현이 아니라, 앞으로 Codex가 따를 top-level 작업 규약을 HTML 프로토타입 중심에서 Unity 게임개발 중심으로 바꾸는 문서 정리다.

## 2. 오늘 바뀐 것

- `AGENTS.md`의 기본 역할을 Unity 2D 게임개발, 리소스 제작, AnkleBreaker MCP 작업, C# 구현, 프리팹/씬 조립, 검증, 기록 갱신으로 바꿨다.
- imagegen은 전체 목표가 아니라 Unity 게임개발 안의 리소스 파이프라인 단계로 명시했다.
- `Unity Game Development Workflow`를 추가했다: 문서 확인, MCP 확인, `_dev` 구조, 리소스, 런타임 기반, 기본 전투, 기억/망각/잔향, 프리팹/씬, 디버그 루프, 리뷰, 승격 판단.
- HTML 프로토타입 작업은 사용자가 명시적으로 요청할 때만 돌아가는 legacy 경로로 내렸다.
- 보고서, devlog, orchestration, git 규약은 유지했다.
- `NEXT_TASKS`를 Unity 게임개발 순서로 재정렬했다.

## 3. 테스트 결과와 근거

이번 작업은 규약/문서 변경이다. 검증은 보고서 생성과 보고서 구조 검증으로 진행한다.

## 4. 결정한 것

- 앞으로 기본 경로는 Unity `_dev` 게임 slice 개발이다.
- 이미지 생성은 첫 실무 단계지만, 그 자체가 목표는 아니다.
- 첫 개발 흐름은 `_dev` 폴더 세팅 -> 기본 리소스 -> 런타임 기반 -> 기본 전투 씬 -> 잔향 디버그 루프다.

## 5. 문제 또는 리스크

- 규약이 너무 구현 중심으로만 흐르면 리소스와 체감 검증이 뒤로 밀릴 수 있다.
- 반대로 리소스만 만들고 C# 런타임/프리팹 조립이 늦어지면 slice 검증이 지연된다.
- `_dev`와 `Assets/Lethe` 승격 기준을 지키지 않으면 실험 코드와 확정 구조가 섞일 수 있다.

## 6. GPT/Claude 인계 요약

이제 LETHE의 기본 작업 단위는 HTML이 아니라 Unity `_dev` 게임 slice다. 다음 에이전트는 이미지 생성만 하지 말고, 리소스가 어디에 쓰이는지 기준으로 MCP import, C# 런타임 기반, 프리팹/씬 조립, 디버그 전환, 1인 리뷰까지 이어가야 한다.

## 7. 다음 Codex 작업

- AnkleBreaker MCP로 `_dev` 하위 폴더를 만든다.
- 기본 판독 리소스 5개를 만들고 import한다.
- `RunBuildState`, hit event, pool/feedback 기반을 얇게 만든다.
- `Dev_EchoSlice.unity`에서 기본 쌍검 타격이 보이는 상태까지 조립한다.

## 8. 포트폴리오 메모

- 문제: 기존 agent 규약이 HTML 프로토타입 중심이라 Unity 개발 단계와 맞지 않았다.
- 방향: 보고 체계는 유지하고, 작업 규약만 Unity 게임개발 중심으로 전환한다.
- 행동: top-level 규약, prompt context, task list, decision log, report를 정렬했다.
- 결과: 다음 세션의 Codex가 이미지 생성에만 갇히지 않고 Unity slice 개발 전체 흐름을 따라갈 기준이 생겼다.

# 2026-06-10-10 - Unity 구조 계약 점검

## 1. 현재 빌드 상태

Unity gameplay 코드는 아직 시작하지 않았다. 현재 상태는 Unity 2D skeleton, `Assets/_dev`, AnkleBreaker MCP 연결, Unity 게임개발 규약, 첫 slice 리소스/프리팹 계획이 준비된 단계다.

## 2. 오늘 바뀐 것

- `LETHE_UNITY_ECHO_SYSTEM_PRD.md`에 OOP/Data 확장 계약을 추가했다.
- core service가 특정 `echoId`를 직접 비교하지 않는다는 규칙을 명시했다.
- `IWeaponRuntime`, `IActiveMemoryRuntime`, `IEchoRuntime`, `IUltimateEchoRuntime`, `IPooledObject`와 Unity용 abstract `MonoBehaviour` base class 방향을 추가했다.
- 신규 무기/기억/잔향/궁극 잔향 추가 절차를 Definition asset, runtime prefab, trigger family, behavior flag 기준으로 정리했다.
- 첫 slice의 data-prefab-resource 매트릭스를 추가했다.
- `LETHE_UNITY_ASSET_BINDING_PLAN.md`에 `_dev` staging과 `Assets/Lethe` 승격 규칙을 추가했다.
- `SCOPE_GUARD.md`를 Unity `_dev` gate 기준으로 갱신했다.
- `RUNBOOK.md`에 현재 Unity 개발 단계에서는 의미 있는 문서/리소스/MCP/C# 단위마다 Discord 알림을 보낸다는 규칙을 추가했다.

## 3. 테스트 결과와 근거

이번 작업은 기술 문서 감사와 보강이다.

- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패.
- `npm.cmd run report:discord:unit:dry`: 통과, fallback payload 확인.
- 실제 Discord fallback 전송: 정책 차단. 보고서와 추가 review prompt 첨부를 외부 Discord로 내보내는 데이터 export로 판단되어 승인되지 않았다.

## 4. 결정한 것

- 첫 Unity 구현은 data-driven OOP 구조로 시작한다.
- 새 콘텐츠는 `Definition asset -> Runtime prefab -> Runtime component -> Resource refs` 흐름으로 추가한다.
- `HitResolver`, `EchoTriggerRouter`, `ForgetService`, `UltimateEchoService`는 특정 잔향 id를 알면 안 된다.
- 현재 MCP 작업은 `Assets/_dev`에만 한다. `Assets/Lethe` 승격은 GO 판정 후다.

## 5. 문제 또는 리스크

- 첫 구현에서 빠르게 만들겠다고 core service에 칼무리/혈반 전용 분기를 넣으면 이후 확장이 막힌다.
- 반대로 추상화를 과하게 만들면 slice 속도가 떨어진다. 그래서 첫 계약은 인터페이스/abstract base/class 책임 경계까지만 둔다.
- asset binding 문서의 기존 `Assets/Lethe` 경로는 승격 후 기준이므로, 실제 작업 때 `_dev` 경로를 우선해야 한다.
- Discord 실제 전송은 현재 fallback payload에 추가 첨부가 포함되어 정책상 차단됐다. 실제 전송은 사용자가 위험을 이해하고 명시 승인할 때만 재시도한다.

## 6. GPT/Claude 인계 요약

문서 감사 결과, 전체 방향은 맞지만 OOP 확장 계약과 `_dev` staging 규칙이 부족했다. 이제 새 무기/기억/잔향은 Definition asset과 runtime prefab으로 들어가고, core service는 id별 분기를 피하는 기준이 생겼다. 다음 Codex 작업은 MCP로 `_dev` 폴더 구조를 만들고 기본 리소스/런타임 기반 구현을 시작하는 것이다.

## 7. 다음 Codex 작업

- AnkleBreaker MCP로 `_dev/Art`, `_dev/Prefabs`, `_dev/Scripts`, `_dev/Data`, `_dev/Scenes`를 만든다.
- 기본 판독 리소스 5개를 생성/import한다.
- `RunBuildState`, Definition classes, `HitEvent`, `HitResolver`, `EchoTriggerRouter`, `PoolService`를 최소 구현한다.
- `Dev_EchoSlice.unity`에서 기본 쌍검 타격까지 조립한다.

## 8. 포트폴리오 메모

- 문제: Unity 구현 전에 구조가 실제 확장 가능한지 확인하지 않으면 첫 slice 코드가 곧 기술부채가 된다.
- 방향: 재미 기획을 data-driven OOP 계약과 asset/prefab binding으로 변환한다.
- 행동: PRD와 asset binding 문서에 확장 규칙, 금지 분기, 첫 slice 매트릭스, `_dev` staging 규칙을 추가했다.
- 결과: 이제 구현을 시작해도 새 콘텐츠 확장성과 첫 slice 속도 사이의 기준이 생겼다.

# 2026-06-10-11 - Unity _dev 폴더 세팅

## 1. 현재 빌드 상태

Unity 2D 프로젝트는 열려 있고 AnkleBreaker MCP bridge는 `7890`에서 응답한다. 이번 작업은 gameplay 구현이 아니라, 앞으로 리소스, C# 스크립트, 프리팹, 데이터, 씬을 넣을 `_dev` staging 구조를 만드는 단계다.

## 2. 오늘 바뀐 것

- `LETHE/Assets/_dev` 아래 개발 폴더 구조를 만들었다.
- `Art/Source`, `Art/Sprites`, `Art/Materials`, `Art/Particles`를 만들었다.
- `Prefabs`, `Scripts`, `Data`, `Scenes` 하위 구조를 만들었다.
- 각 leaf 폴더에 `.gitkeep`를 추가해 빈 폴더도 Git에서 유지되게 했다.
- AnkleBreaker MCP로 Unity가 주요 `_dev` 폴더를 읽는지 확인했다.
- `NEXT_TASKS`에서 폴더 세팅을 제거하고 다음 작업을 기본 리소스 생성/import로 이동했다.

## 3. 테스트 결과와 근거

- `unity_editor_ping(port=7890)`: connected true, project `LETHE`, Unity `6000.3.10f1`.
- `unity_asset_list(folder="Assets/_dev")`: success true.
- `unity_asset_list(folder="Assets/_dev/Art")`: success true.
- `unity_asset_list(folder="Assets/_dev/Art/Sprites")`: success true.
- `unity_asset_list(folder="Assets/_dev/Prefabs")`: success true.
- `unity_asset_list(folder="Assets/_dev/Scripts")`: success true.
- `unity_asset_list(folder="Assets/_dev/Data")`: success true.
- `unity_asset_list(folder="Assets/_dev/Scenes")`: success true.
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과.
- `npm.cmd run report:discord:unit:dry`: 통과. 단, fallback payload에 추가 review prompt 첨부가 포함되어 실제 전송은 재시도하지 않았다.

## 4. 결정한 것

- 빈 Unity 개발 폴더는 `.gitkeep`로 유지한다.
- 실제 리소스/스크립트가 들어오면 해당 `.gitkeep`는 필요할 때 제거해도 된다.
- 다음 작업은 기본 판독 리소스 5개 생성/import다.

## 5. 문제 또는 리스크

- `.gitkeep`는 Unity Project 창에서 보일 수 있다. `_dev` staging 단계에서는 허용하고, 승격 전 정리한다.
- 아직 runtime C#과 prefab은 없다. 폴더만 준비된 상태다.

## 6. GPT/Claude 인계 요약

Unity `_dev` 구조는 준비됐고 MCP가 읽는다. 다음 Codex는 imagegen으로 player, walker enemy, dark floor tile, left/right dual blades를 만들고, chroma-key/alpha 정리 후 `Assets/_dev/Art/Sprites`에 import하면 된다.

## 7. 다음 Codex 작업

- 기본 판독 리소스 5개 생성.
- Unity import와 sprite setting 확인.
- 이후 `RunBuildState`, Definition classes, hit event 기반 구현으로 이동.

## 8. 포트폴리오 메모

- 문제: Unity 작업을 시작하기 전 리소스/코드/프리팹/데이터가 들어갈 staging 구조가 없었다.
- 방향: 실험은 `_dev`, 승격은 GO 이후 `Assets/Lethe`로 분리한다.
- 행동: `_dev` 하위 폴더와 Git 유지 파일을 만들고 MCP 인식을 확인했다.
- 결과: 이제 리소스 생성과 Unity 런타임 구현을 병렬로 쌓을 수 있는 작업 표면이 생겼다.
