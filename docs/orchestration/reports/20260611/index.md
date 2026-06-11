# LETHE 개발 보고서 - 2026-06-11

Unity `_dev` playable echo slice를 아침 확인 가능한 상태로 만들기 위한 야간 작업을 진행한다.

# 2026-06-11-01 - Unity 핵심 잔향 VFX 생성/import

## 1. 현재 빌드 상태

Unity `LETHE` 프로젝트는 AnkleBreaker MCP port `7890`에 연결되어 있고, `_dev` 기본 전투 씬 위에 붙일 칼무리/혈반/피의 칼폭풍 VFX 리소스가 들어갔다. 아직 debug loop runtime은 붙지 않았지만, Task 4에서 바로 scene에 배치할 수 있는 sprite와 prefab은 준비됐다.

## 2. 오늘 바뀐 것

- Codex imagegen으로 잔향/궁극 VFX 이미지 8개를 생성했다.
- chroma-key 원본을 `LETHE/Assets/_dev/Art/Source`에 보존했다.
- runtime alpha PNG를 `LETHE/Assets/_dev/Art/Sprites/Echoes`와 `LETHE/Assets/_dev/Art/Sprites/Ultimates`에 저장했다.
- Unity에서 8개 texture를 Sprite/Single/100 PPU로 설정했다.
- 칼무리 prefab 3개, 혈반 prefab 3개, 궁극 prefab 1개를 만들었다.
- VFX 확인용 contact sheet를 `docs/orchestration/evidence/2026-06-11-echo-vfx-contact-sheet.png`에 남겼다.

## 3. 테스트 결과와 근거

- Alpha validation: 8개 runtime PNG 모두 corner alpha `0,0,0,0`.
- `unity_asset_list(folder="Assets/_dev/Art/Sprites/Echoes", type="Texture")`: 6개 Texture2D 확인.
- `unity_asset_list(folder="Assets/_dev/Art/Sprites/Ultimates", type="Texture")`: 2개 Texture2D 확인.
- `unity_asset_list(folder="Assets/_dev/Prefabs/Echoes", type="Prefab")`: 6개 prefab 확인.
- `unity_asset_list(folder="Assets/_dev/Prefabs/Ultimates", type="Prefab")`: 1개 prefab 확인.
- `unity_get_compilation_errors(port=7890, severity="all")`: `count: 0`, `isCompiling: false`.
- `npm.cmd run report`: 통과, 1개 unit report 생성.
- `npm.cmd run report:check`: 통과, 1개 unit heading 확인.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패. Project Orchestrator intake endpoint가 현재 응답하지 않는 상태로 판단.

## 4. 결정한 것

- Task 3는 핵심 5개를 넘겨 궁극 표시용 3개까지 같이 포함한다.
- 첫 slice에서는 최종 VFX 완성도보다 상태별 판독성이 우선이다.
- Pillow 미설치로 imagegen helper를 쓰지 못했기 때문에, 이번 단색 chroma 제거는 Windows `System.Drawing`으로 처리했다.

## 5. 문제 또는 리스크

- 생성 이미지는 최종 sprite sheet가 아니라 단일 VFX 파츠다.
- 일부 VFX는 축소 후 Unity Game View에서 실제 크기 재조정이 필요할 수 있다.
- `heal thread`는 tip sprite만 있고, 실제 line/trail 동작은 Task 4에서 구현해야 한다.
- Project Orchestrator API 보고는 endpoint 미응답으로 실제 제출되지 않았다.

## 6. GPT/Claude 인계 요약

칼무리, 혈반, 피의 칼폭풍의 첫 VFX 파츠와 placeholder prefab은 준비됐다. 다음 Codex 작업은 `Dev_EchoSlice`에 debug controller를 붙여 기본 쌍검, 칼무리 +1/+5, 혈반 +5, 피의 칼폭풍 상태를 키보드 또는 UI로 즉시 전환할 수 있게 만드는 것이다.

## 7. 다음 Codex 작업

- `DevEchoSliceDebugController`를 만든다.
- scene에 VFX prefab reference를 연결한다.
- 숫자키 또는 화면 버튼으로 slice 상태를 전환한다.
- Play Mode 또는 MCP 직접 호출로 상태별 오브젝트 활성화를 검증한다.

## 8. 포트폴리오 메모

- 문제: 잔향이 텍스트가 아니라 화면 행동으로 읽히려면 독립적인 VFX 파츠가 필요했다.
- 방향: 칼무리, 혈반, 궁극을 서로 다른 색/형태로 구분한다.
- 행동: imagegen, alpha cleanup, Unity import, prefab 생성, evidence 제작을 한 단위로 처리했다.
- 결과: 다음 구현에서 실제 playable debug loop에 꽂을 수 있는 VFX 리소스 표면이 생겼다.

# 2026-06-11-02 - Unity 플레이 가능한 잔향 디버그 루프

## 1. 현재 빌드 상태

`Dev_EchoSlice.unity`는 이제 Play Mode에서 바로 확인 가능한 상태다. 숫자키 `1~5` 또는 화면의 작은 debug 버튼으로 기본 쌍검, 칼무리 +1, 칼무리 +5, 혈반 +5, 피의 칼폭풍 상태를 전환할 수 있다.

## 2. 오늘 바뀐 것

- `DevEchoSliceDebugController.cs`를 추가했다.
- `Services` 오브젝트에 debug controller를 붙였다.
- player, enemy, weapon, build state, hit resolver, pool service, echo anchor, VFX prefab references를 연결했다.
- 자동 공격과 `Space` 강제 공격을 지원하게 했다.
- 칼무리 +1은 지연 칼선, 칼무리 +5는 고리/발사 칼날, 혈반 +5는 혈반/피꽃/회복 실, 피의 칼폭풍은 고리와 칼날 폭풍을 보여준다.
- Play Mode evidence screenshot을 `docs/orchestration/evidence/2026-06-11-dev-echo-slice-play.png`에 저장했다.

## 3. 테스트 결과와 근거

- 최초 Play Mode 검증에서 Input System-only 프로젝트에서 `UnityEngine.Input.GetKeyDown` 예외가 발생하는 문제를 발견했다.
- 입력 처리를 `UnityEngine.InputSystem.Keyboard` 기준으로 수정하고 legacy input은 define이 있을 때만 쓰게 바꿨다.
- `unity_get_compilation_errors(port=7890, severity="all")`: `count: 0`, `isCompiling: false`.
- Play Mode runtime check: `controller=true`, `stormVisible=true`, `orbitVisible=true`, `healThreadVisible=true`, final forced attack 후 `enemyHealth=6`.
- 최종 Play Mode 후 Unity console error log: `count: 0`.
- Play Mode 정지 후 editor state: `isPlaying=false`, `sceneDirty=false`.
- scene missing reference scan: `totalFound=0`.
- `npm.cmd run report`: 통과, 2개 unit report 생성.
- `npm.cmd run report:check`: 통과, 2개 unit heading 확인.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패. Project Orchestrator intake endpoint가 현재 응답하지 않는 상태로 판단.

## 4. 결정한 것

- 아침 리뷰용 상호작용은 우선 키보드 `1~5`와 최소 OnGUI 버튼으로 충분하다.
- production runtime이 아니라 `_dev` debug controller로 격리한다.
- 승격 전에는 이 debug controller를 그대로 `Assets/Lethe`로 옮기지 않고, 검증된 동작만 production echo runtime으로 정리한다.

## 5. 문제 또는 리스크

- 현재 VFX는 playable placeholder이며 최종 밸런스/타이밍/풀링 구조가 아니다.
- persistent orbit/storm은 debug controller에서 직접 생성한다. production 승격 시 `PoolService`/runtime class 기반으로 바꿔야 한다.
- 소리, 히트스톱, 카메라 impulse는 아직 없다.
- OnGUI debug panel은 리뷰용이며 최종 UI가 아니다.
- Project Orchestrator API 보고는 endpoint 미응답으로 실제 제출되지 않았다.

## 6. GPT/Claude 인계 요약

Unity에서 잔향이 텍스트가 아니라 화면 행동으로 보이는 최소 loop가 생겼다. jaewoo는 아침에 `Dev_EchoSlice.unity`를 열고 Play 후 `1~5`를 눌러 각 상태가 충분히 구분되는지 보면 된다. 핵심 판단은 “피의 칼폭풍을 다시 보고 싶은가”와 “칼무리/혈반이 서로 다른 효과로 읽히는가”다.

## 7. 다음 Codex 작업

- 아침 리뷰 체크리스트와 승격 전 gate 문서를 정리한다.
- jaewoo 확인 후 GO면 `_dev -> Assets/Lethe` 승격 계획을 실행한다.
- ITERATE면 VFX 크기, 패널 위치, hit timing, blood thread readability를 먼저 조정한다.

## 8. 포트폴리오 메모

- 문제: 리소스와 전투 기반이 있어도 플레이어가 직접 눌러볼 수 없으면 체감 검증이 안 된다.
- 방향: 최종 시스템보다 빠른 debug loop를 만들어 감정 루프를 먼저 본다.
- 행동: debug controller, scene wiring, runtime play check, screenshot evidence를 추가했다.
- 결과: 아침에 Unity에서 바로 플레이하며 잔향 slice를 검토할 수 있는 상태가 됐다.

# 2026-06-11-03 - Unity 아침 리뷰 게이트 정리

## 1. 현재 빌드 상태

Unity `_dev` echo slice는 아침 리뷰 대기 상태다. `Dev_EchoSlice.unity`를 열고 Play 후 숫자키 `1~5`와 `Space`로 기본 쌍검, 칼무리 +1, 칼무리 +5, 혈반 +5, 피의 칼폭풍을 확인할 수 있다.

## 2. 오늘 바뀐 것

- jaewoo 아침 리뷰 프롬프트를 만들었다.
- `_dev -> Assets/Lethe` 승격 게이트 문서를 만들었다.
- 디자인 문서 index에 승격 게이트 문서를 추가했다.
- 현재 작업 상태를 추가 구현이 아니라 jaewoo 리뷰 대기로 바꿨다.
- 다음 작업 목록을 리뷰, 첫 ITERATE 수정, production runtime 분리, data asset binding, 승격 순서로 정리했다.

## 3. 테스트 결과와 근거

- 리뷰 대상 scene: `LETHE/Assets/_dev/Scenes/Dev_EchoSlice.unity`.
- 리뷰 프롬프트: `docs/orchestration/review_prompts/2026-06-11-unity-echo-slice-jaewoo-review.md`.
- 승격 게이트: `docs/design/LETHE_UNITY_ECHO_SLICE_PROMOTION_GATE.md`.
- 직전 Play Mode 검증 기준: compile error 0, missing reference 0, console error 0, scene dirty false.
- 직전 runtime check: `controller=true`, `stormVisible=true`, `orbitVisible=true`, `healThreadVisible=true`, `enemyHealth=6`.
- 최종 Unity editor state: active scene `Assets/_dev/Scenes/Dev_EchoSlice.unity`, `isCompiling=false`, `sceneDirty=false`.
- 최종 Unity 검증: compilation errors `count=0`, scene missing references `totalFound=0`, console errors `count=0`.
- `npm.cmd run report`: 통과, 3개 unit report 생성.
- `npm.cmd run report:check`: 통과, 3개 unit heading 확인.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패. Project Orchestrator intake endpoint가 현재 응답하지 않는 상태로 판단.

## 4. 결정한 것

- 지금은 더 자동 구현하지 않고 jaewoo의 체감 리뷰를 받는다.
- GO 전에는 `Assets/Lethe`로 승격하지 않는다.
- GO가 나도 debug controller 그대로 승격하지 않고 production runtime split을 먼저 한다.

## 5. 문제 또는 리스크

- VFX는 placeholder imagegen sprite다.
- sound, hitstop, camera impulse, 2D light는 아직 없다.
- Blood heal thread와 Storm density는 사람 눈으로 보며 조정해야 한다.
- Project Orchestrator API 보고는 이전 Task와 같이 endpoint 미응답 가능성이 남아 있다.

## 6. GPT/Claude 인계 요약

jaewoo가 Unity에서 직접 플레이한 뒤 `GO`, `ITERATE`, `NO-GO`를 정하면 된다. 가장 약한 상태와 첫 수정 항목을 받아 Codex가 다음 구현 단위를 고른다.

## 7. 다음 Codex 작업

- jaewoo 리뷰 결과를 읽는다.
- `ITERATE`면 가장 약한 상태의 VFX scale, sorting, timing, readability를 먼저 고친다.
- `GO`면 promotion gate에 따라 production runtime split과 data asset binding부터 진행한다.
- `NO-GO`면 잔향 기획 문서로 돌아가 형태 변환 규칙을 다시 잡는다.

## 8. 포트폴리오 메모

- 문제: playable slice가 있어도 판단 기준이 없으면 다음 작업이 흔들린다.
- 방향: 구현 완료가 아니라 리뷰 가능한 의사결정 단위로 묶는다.
- 행동: 리뷰 프롬프트, 승격 게이트, task 상태, report를 한 번에 정리했다.
- 결과: jaewoo가 아침에 Unity를 열고 판단하면 바로 다음 Codex 작업으로 이어질 수 있다.

# 2026-06-11-04 - Unity 리뷰 전 타격감 보강

## 1. 현재 빌드 상태

`Dev_EchoSlice.unity`는 기존 `1~5` 리뷰 루프를 유지하면서, 기본 공격과 잔향 타격의 체감 피드백이 더 잘 보이도록 보강됐다. 여전히 `_dev` debug slice이며, `Assets/Lethe` 승격은 하지 않았다.

## 2. 오늘 바뀐 것

- 기본 쌍검 타격마다 절단 arc가 짧게 보이게 했다.
- 칼무리 +1 칼선은 즉시 발생이 아니라 짧은 지연 후 발생하게 바꿨다.
- 타격 순간 2-frame debug hit stop과 짧은 camera shake를 추가했다.
- 혈반 +5 회복 피드백은 1가닥에서 3가닥 heal thread로 바꿨다.
- 칼무리 +5와 피의 칼폭풍은 타격 시 고리가 잠깐 펄스되게 했다.

## 3. 테스트 결과와 근거

- `unity_get_compilation_errors(port=7890, severity="all")`: `count=0`, `isCompiling=false`.
- Play Mode runtime check에서 mode `0~4` 전환과 강제 공격을 직접 호출했다.
- 런타임 생성 확인: `Debug_DualBladeSwingArc`, `Debug_HealThreadLine`, `Echo_Kalmuri_LaunchBlade(Clone)`, `Ultimate_BloodBladeStorm(Clone)`, `Debug_KalmuriOrbit`.
- `unity_console_log(port=7890, type="error")`: `count=0`.
- `unity_search_missing_references(port=7890, scope="scene")`: `totalFound=0`.
- Play Mode 정지 후 editor state: `isPlaying=false`, `isCompiling=false`, active scene `Assets/_dev/Scenes/Dev_EchoSlice.unity`, `sceneDirty=false`.
- `npm.cmd run report`: 통과, 4개 unit report 생성.
- `npm.cmd run report:check`: 통과, 4개 unit heading 확인.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패. Project Orchestrator intake endpoint가 현재 응답하지 않는 상태로 판단.

## 4. 결정한 것

- GO 전에도 리뷰 품질을 높이는 `_dev` polish는 진행한다.
- production runtime split, data asset binding, `Assets/Lethe` 승격은 jaewoo 리뷰 전에는 진행하지 않는다.
- 이번 변경은 새 무기/새 기억/새 콘텐츠가 아니라 기존 slice의 판독성 보강으로 본다.

## 5. 문제 또는 리스크

- hit stop과 camera shake는 `_dev` debug controller 안에 임시로 있다.
- line renderer 기반 arc와 heal thread는 최종 VFX가 아니다.
- camera shake 강도와 blood thread 밀도는 실제 사람 눈으로 과한지 봐야 한다.

## 6. GPT/Claude 인계 요약

기존 리뷰 씬에 첫 타격감 보강이 들어갔다. 리뷰 질문은 그대로 유지하되, 이제 Base/Kalmuri/Blood/Storm이 더 잘 구분되는지 확인하면 된다.

## 7. 다음 Codex 작업

- jaewoo 리뷰 결과를 기다린다.
- `ITERATE`면 가장 약한 상태의 VFX scale, timing, camera shake, heal thread density를 우선 조정한다.
- `GO`면 debug-only feedback을 production feedback/runtime 구조로 분리한다.

## 8. 포트폴리오 메모

- 문제: 기능은 작동하지만 첫 인상에서 공격이 밋밋하면 리뷰 판단이 왜곡된다.
- 방향: 시스템 확장보다 hit feel과 판독성만 좁게 보강한다.
- 행동: 기본 arc, delayed slash, hit stop, camera shake, heal thread 다발, orbit pulse를 한 파일에 제한해 추가했다.
- 결과: 아침 리뷰에서 “잔향이 붙어 있다”보다 “무기와 잔향이 반응한다”를 더 빠르게 판단할 수 있다.

# 2026-06-11-05 - Unity 게임 형식 수리 1차

## 1. 현재 빌드 상태

`Dev_EchoSlice.unity`는 더 이상 정적인 VFX 확인 장치만은 아니다. 이제 플레이어 이동, 카메라 추적, 적 추적, 플레이어 손 위치에 붙은 무기, 공격 스윙, 간단한 sprite bob/tilt, 기존 `1~5` 잔향 상태 전환이 같은 씬 안에서 작동한다.

## 2. 오늘 바뀐 것

- `LETHE_UNITY_GAMEPLAY_SLICE_REPAIR_PLAN.md`를 추가했다.
- `DevPlayerController2D`를 추가해 WASD/방향키 이동을 지원했다.
- `DevEnemyChaseController`를 추가해 적이 플레이어를 추적하게 했다.
- `DevCameraFollow2D`를 추가해 카메라가 플레이어를 따라가게 했다.
- `DevSpriteMotionAnimator`를 추가해 플레이어/적 `Visual` 자식에 bob/tilt 애니메이션을 적용했다.
- `DualBladesController`에 공격 스윙을 추가했다.
- `Weapon_DualBlades_Runtime`를 `Player_EchoShowcase/WeaponAnchor` 밑으로 옮겼다.
- 플레이어/적 SpriteRenderer를 루트에서 `Visual` 자식으로 분리했다.
- debug panel에 WASD 이동, 적 추적, 무기 장착 안내를 추가했다.

## 3. 테스트 결과와 근거

- `unity_get_compilation_errors(port=7890, severity="all")`: `count=0`, `isCompiling=false`.
- `unity_search_missing_references(port=7890, scope="scene")`: `totalFound=0`.
- `unity_console_log(port=7890, type="error")`: `count=0`.
- Scene composition check:
  - `playerController=true`
  - `playerVisual=true`
  - `weaponParent=WeaponAnchor`
  - `dualBlades=true`
  - `enemyChase=true`
  - `enemyVisual=true`
  - `cameraFollow=true`
- Play Mode runtime check:
  - 적이 오른쪽 시작 위치에서 플레이어 쪽으로 이동해 `(-0.03, 0.00, 0.00)` 근처까지 접근했다.
  - 무기 parent는 `WeaponAnchor`로 유지됐다.
  - 공격 시 `Debug_DualBladeSwingArc`와 `Debug_HealThreadLine`이 생성됐다.
  - Play Mode 정지 후 `isPlaying=false`, `sceneDirty=false`.
- `npm.cmd run report`: 통과, 5개 unit report 생성.
- `npm.cmd run report:check`: 통과, 5개 unit heading 확인.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패. Project Orchestrator intake endpoint가 현재 응답하지 않는 상태로 판단.

## 4. 결정한 것

- 이전 리뷰 기준은 너무 낮았다. 이제 “잔향이 보이냐”보다 “게임 상황 위에서 잔향을 판단할 수 있냐”를 기준으로 삼는다.
- `Dev_EchoSlice`는 여전히 `_dev`에 둔다.
- `Assets/Lethe` 승격은 아직 하지 않는다.

## 5. 문제 또는 리스크

- 아직 플레이어 HP와 적 접촉 피해가 없다.
- 적은 1마리라 arena 압박이 약하다.
- 무기는 가까운 적 자동 탐색이 아니라 여전히 debug target 중심이다.
- 잔향 VFX는 실제 echo damage가 아니라 대부분 표시용이다.
- sprite animation은 정식 프레임 애니메이션이 아니라 절차적 bob/tilt다.

## 6. GPT/Claude 인계 요약

정적인 효과 확인 씬을 최소 게임 형태로 수리했다. 다음 판단은 생존 루프, 다수 적, 실제 echo damage 중 무엇을 먼저 넣을지다.

## 7. 다음 Codex 작업

- 플레이어 HP + 적 접촉 피해 + 피격 flash.
- 가장 가까운 적 자동 타겟팅.
- 적 3~8마리 spawn loop.
- Kalmuri/Blood/Storm VFX를 실제 `EchoHit` damage로 연결.
- ScriptableObject data asset binding.

## 8. 포트폴리오 메모

- 문제: VFX가 있어도 캐릭터/몹/무기가 가만히 있으면 게임 slice로 평가할 수 없다.
- 방향: 기획 확장보다 먼저 조작, 추적, 장착, 공격 동작을 갖춘 최소 게임 형태를 만든다.
- 행동: 이동/추적/카메라/무기 장착/스윙/절차 애니메이션/문서 리스트를 한 번에 추가했다.
- 결과: 이제 Unity에서 움직이고 쫓기고 휘두르는 상태 위에서 잔향을 판단할 수 있다.

# 2026-06-11-06 - Unity 완성형 slice 계획과 화면 기준 1차

## 1. 현재 빌드 상태

`Dev_EchoSlice.unity`의 목표를 잔향/VFX 테스트에서 1분짜리 playable combat slice로 재정의했다. Phase 1로 카메라 거리, 캐릭터/몹/무기 크기, arena 경계, debug panel 크기를 정리했다.

## 2. 오늘 바뀐 것

- `LETHE_UNITY_PLAYABLE_GAME_SLICE_PLAN.md`를 추가했다.
- 현재 작업 기준을 Phase 1 카메라/스케일/구도 정리로 바꿨다.
- 다음 작업 목록을 Phase 2 생존 루프, 타겟팅, 다수 적, 실제 echo damage 순서로 재정렬했다.
- Main Camera orthographic size를 `4.15`로 설정했다.
- floor scale을 `13.5 x 8.2`로 조정했다.
- `Arena_Bounds` line renderer를 추가했다.
- player scale `0.92`, enemy scale `0.84`, weapon scale `1.12`로 조정했다.
- weapon anchor, blade local transform, sorting order를 정리했다.
- debug panel을 작게 줄였다.

## 3. 테스트 결과와 근거

- `unity_get_compilation_errors(port=7890, severity="all")`: `count=0`, `isCompiling=false`.
- `unity_search_missing_references(port=7890, scope="scene")`: `totalFound=0`.
- `unity_console_log(port=7890, type="error")`: `count=0`.
- 첫 Play Mode 요청은 MCP queue `fetch failed`가 났지만, `unity_editor_ping(port=7890)`은 정상 연결을 반환했다.
- 두 번째 Play Mode 요청은 성공했다.
- Play Mode composition check:
  - `cameraSize=4.15`
  - `playerScale=(0.92, 0.92, 1.00)`
  - `enemyScale=(0.84, 0.84, 1.00)`
  - `weaponParent=WeaponAnchor`
  - `weaponScale=(1.12, 1.12, 1.00)`
  - `arenaBounds=true`
  - `swingArcPresent=true`
- Play Mode 정지 후 editor state: `isPlaying=false`, `isCompiling=false`, active scene `Assets/_dev/Scenes/Dev_EchoSlice.unity`, `sceneDirty=false`.
- `npm.cmd run report`: 통과, 6개 unit report 생성.
- `npm.cmd run report:check`: 통과, 6개 unit heading 확인.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패. Project Orchestrator intake endpoint가 현재 응답하지 않는 상태로 판단.

## 4. 결정한 것

- 지금부터 기준은 “잔향이 보이냐”가 아니라 “게임 위에서 잔향을 판단할 수 있냐”다.
- Phase 1은 화면 기준과 기본 구도를 먼저 맞춘다.
- Phase 2는 HP/접촉 피해/가까운 적 타겟팅/다수 적으로 간다.

## 5. 문제 또는 리스크

- 아직 플레이어 HP와 적 접촉 피해가 없다.
- 적은 아직 1마리다.
- 실제 echo damage는 아직 연결되지 않았다.
- 시각적으로 최종 art는 아니고, 스케일/구도 baseline만 잡은 상태다.

## 6. GPT/Claude 인계 요약

Unity 방향을 playable game slice로 재정의했고, Phase 1 카메라/스케일/구도 baseline을 적용했다. 다음 판단은 Phase 2에서 생존 루프와 다수 적을 먼저 넣을지, 타겟팅을 먼저 넣을지다.

## 7. 다음 Codex 작업

- player HP와 enemy contact damage.
- nearest enemy targeting.
- enemy 3~8마리 spawn loop.
- hit/recoil polish.
- echo VFX를 실제 damage로 연결.

## 8. 포트폴리오 메모

- 문제: 화면 크기와 카메라 기준이 틀리면 시스템이 있어도 게임처럼 보이지 않는다.
- 방향: 기능 확장 전에 화면 구도와 플레이 기준을 먼저 고정한다.
- 행동: playable game slice 플랜을 만들고 카메라/스케일/arena/weapon anchor/debug panel을 정리했다.
- 결과: 다음 전투 시스템을 올릴 수 있는 화면 baseline이 생겼다.

# 2026-06-11-07 - Unity slice 폐기와 Prototype v0 계획

## 1. 현재 빌드 상태

`Dev_EchoSlice`는 더 이상 메인 작업 대상이 아니다. 현재 결론은 slice를 계속 고치는 것이 아니라 `Dev_Prototype_v0.unity`를 새로 만들어 HTML보다 나은 Unity 프로토타입을 직접 구현하는 것이다.

## 2. 오늘 바뀐 것

- `LETHE_UNITY_PROTOTYPE_V0_PLAN.md`를 추가했다.
- 디자인 문서 index를 갱신했다.
- 현재 작업을 `Dev_Prototype_v0` 구현으로 바꿨다.
- 다음 작업 목록을 새 프로토타입 기준으로 바꿨다.
- `Dev_EchoSlice`는 reference only로 격하했다.

## 3. 테스트 결과와 근거

- 이 유닛은 구현 검증이 아니라 전략/계획 수정이다.
- 기존 `Dev_EchoSlice`는 삭제하지 않았다.
- 새 메인 target은 `Assets/_dev/Scenes/Dev_Prototype_v0.unity`다.
- `npm.cmd run report`: 통과, 7개 unit report 생성.
- `npm.cmd run report:check`: 통과, 7개 unit heading 확인.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패. Project Orchestrator intake endpoint가 현재 응답하지 않는 상태로 판단.

## 4. 결정한 것

- `slice` 접근은 폐기한다.
- `Dev_EchoSlice`를 더 polish하지 않는다.
- 새 프로토타입은 combat loop first로 만든다.
- 잔향은 전투 루프 위에 올라가는 시스템으로 구현한다.

## 5. 문제 또는 리스크

- 새 scene을 만들기 전까지 Unity playable prototype은 아직 없다.
- 기존 slice 에셋을 참고하되, 그 구조를 그대로 복사하면 같은 문제가 반복될 수 있다.
- 첫 구현에서 욕심내서 잔향부터 넣으면 다시 테스트 장치가 될 위험이 있다.

## 6. GPT/Claude 인계 요약

LETHE Unity 작업은 `Dev_EchoSlice` 중심 slice에서 `Dev_Prototype_v0` 직접 구현으로 전환한다. 첫 성공 기준은 이동/공격/적 다수/피격/처치/리스폰/HUD가 있는 30초 combat loop다.

## 7. 다음 Codex 작업

- `Assets/_dev/Scenes/Dev_Prototype_v0.unity` 생성.
- `PrototypeRoot`, `Services`, `Player`, `EnemySpawner`, `Arena`, `RuntimeVFX`, `HUD` 구성.
- player movement, camera follow, 5 enemy spawn/chase, nearest targeting, player HP/contact damage, enemy death/respawn 구현.

## 8. 포트폴리오 메모

- 문제: 작은 slice는 빠르게 만들 수 있었지만 HTML보다 낮은 체감이면 의미가 없다.
- 방향: 시스템 검증 장치가 아니라 실제 게임 프로토타입을 만든다.
- 행동: 기존 slice 경로를 폐기하고 Prototype v0 계획과 작업 순서를 다시 정의했다.
- 결과: 다음 작업 기준이 “잔향 보기”에서 “플레이 가능한 LETHE 프로토타입”으로 바뀌었다.

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

# 2026-06-11-09 - Unity Prototype v0 M1-M5 플레이 루프

## 1. 현재 빌드 상태

`Dev_EchoSlice`가 아니라 `Assets/_dev/Scenes/Dev_Prototype_v0.unity`가 새 메인 프로토타입 씬이다. 이제 이동, 적 추격, 자동 쌍검 공격, 처치, 기억 선택, 망각, 잔향, 공명, 피의 칼폭풍까지 한 씬에서 확인할 수 있다.

## 2. 오늘 바뀐 것

- 플레이어/적 4방향 idle/walk용 스프라이트 시트를 새로 만들고 Unity에 넣었다.
- `Assets/_dev/Scripts/Prototype` 런타임 스크립트를 추가했다.
- `Dev_Prototype_v0.unity` 씬을 만들었다.
- 플레이어, 적, arena, camera, HUD, services 구조를 구성했다.
- 플레이어/적 prototype prefab과 첫 무기/기억/잔향/궁극 데이터 에셋을 만들었다.
- F1~F5 디버그 점프를 추가했다.

## 3. 테스트 결과와 근거

- Unity compile errors: `0`.
- Scene missing references: `0`.
- Play Mode console errors: `0`.
- 런타임 생성 확인: player/manager 존재, enemy `7`.
- 전투 smoke: 적을 플레이어 근처로 강제 배치 후 8초 동안 `kills=7`, `playerHp=26.5`.
- M5 상태 smoke: 활성 기억 `Memory_HungryBlades:3`, `Memory_BloodReflection:2`; 잔향 `Echo_Kalmuri:5`, `Echo_Blood:5`; ultimate `true`.
- 궁극 smoke: 피의 칼폭풍 상태에서 5초 후 `kills=148`, `playerHp=100`, console errors `0`.
- `npm.cmd run report`: 통과, 9개 unit report 생성.
- `npm.cmd run report:check`: 통과, 9개 unit heading 확인.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패. Project Orchestrator intake endpoint가 현재 응답하지 않는 상태로 판단.

## 4. 결정한 것

- `Dev_Prototype_v0`를 jaewoo 리뷰 대상 씬으로 둔다.
- `_dev`에서 계속 검증하고, 아직 `Assets/Lethe`로 승격하지 않는다.
- 피의 칼폭풍은 첫 뽕맛 검증을 위해 강하게 둔 상태이며, 다음 단계에서 밸런스를 잡는다.

## 5. 문제 또는 리스크

- 궁극 처치 속도가 과하다.
- VFX 상당수는 아직 line-renderer placeholder다.
- 생성 스프라이트는 프로토타입 판독용이며, 방향/프레임 가독성이 부족하면 다음 pass에서 재생성해야 한다.

## 6. GPT/Claude 인계 요약

Unity Prototype v0는 M1~M5 루프가 들어간 상태다. 다음 판단은 jaewoo가 직접 Play Mode에서 카메라/스케일/전투 압박/기억 선택/망각/잔향/궁극 체감을 확인하고 GO 또는 ITERATE를 주는 것이다.

## 7. 다음 Codex 작업

- jaewoo 리뷰 결과를 받아 첫 튜닝 지점을 고른다.
- 유력 후보는 camera/framing, contact damage, enemy health, Blood Blade Storm damage/heal, sprite readability, line-renderer VFX 교체다.

## 8. 포트폴리오 메모

- 문제: 작은 echo slice는 빨랐지만 게임 검증에는 부족했다.
- 방향: 먼저 게임 루프가 도는 Unity 프로토타입을 만든다.
- 행동: 스프라이트, 씬, 전투, 기억/망각/잔향/궁극 루프를 한 번에 연결하고 MCP로 검증했다.
- 결과: HTML보다 낮은 테스트 장치가 아니라, 실제 플레이 리뷰 가능한 Unity prototype baseline이 생겼다.
