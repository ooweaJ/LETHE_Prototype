# LETHE 개발 보고서 - 2026-06-12

새 통합 설계 문서(`LETHE_DESIGN_00..07`)를 기준으로 Unity 프로토타입을 v1로 다시 시작했다.

# 2026-06-12-01 - Dev_Prototype_v1 새 출발

## 1. 현재 빌드 상태

`Dev_Prototype_v0`는 실패한 참고용으로 내리고, 새 메인 대상은 `Assets/_dev/Scenes/Dev_Prototype_v1.unity`다. v1은 `Scripts/PrototypeV1` 아래 새 코드로 분리했다.

## 2. 오늘 바뀐 것

- `V1GameManager`를 추가했다.
- `Dev_Prototype_v1.unity` 새 씬을 만들었다.
- 플레이어, 카메라, 맵, 쌍검, 적 스폰, HUD, XP/레벨업, 망각/잔향/공명 기초 루프를 새로 구성했다.
- Input System 전용 프로젝트에서 입력 예외가 나지 않도록 고쳤다.
- 4방향 sprite sheet가 통째로 보이는 문제를 런타임 frame crop으로 고쳤다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 0 warning / 0 error.
- Unity compile errors: `count=0`.
- Unity scene open: `Assets/_dev/Scenes/Dev_Prototype_v1.unity` 성공.
- Play Mode smoke: player 생성 `true`, enemy `2`, SpriteRenderer `107`.
- Console: v1 runtime exception 없음.
- Game capture: 플레이어/적이 sprite sheet 전체가 아니라 단일 프레임으로 표시됨.
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패.

## 4. 결정한 것

- v0를 더 고치지 않고 v1을 메인으로 한다.
- 지금은 8기억 전체 확장보다 M1 게임 셸 완성이 우선이다.
- 대검/8기억/8잔향/4궁극은 v1 M1/M2가 납득된 뒤 다시 확장한다.

## 5. 문제 또는 리스크

- v1은 아직 출시형 구조가 아니라 M1 검증용 런타임이다.
- sprite sheet는 import slicing이 아니라 런타임 crop으로 임시 처리했다.
- 아직 자동화 smoke injector와 M2 Gatekeeper 루프가 부족하다.
- Project Orchestrator Discord intake가 현재 응답하지 않아 dry-run 보고가 실패했다.

## 6. GPT/Claude 인계 요약

이전 Unity 프로토타입은 실패 기준으로 보고, 새 문서 기준의 v1을 시작했다. 다음 판단은 v1의 카메라/적/무기/XP/HUD가 게임 셸로 충분한지다.

## 7. 다음 Codex 작업

- v1 카메라와 전투 체감 보강.
- debug smoke injector 추가.
- Gatekeeper -> 망각 결과 -> 결손 생존 -> 공명 루프 구현.

## 8. 포트폴리오 메모

- 문제: 이전 프로토타입은 설계 의도를 반영하지 못해 평가 기준이 흔들렸다.
- 방향: 실패한 v0를 패치하지 않고 새 설계 문서 기준으로 v1을 분리했다.
- 행동: 새 씬/새 namespace/새 런타임을 만들고 Unity MCP로 컴파일·Play Mode 검증했다.
- 결과: 다시 평가 가능한 최소 게임 셸의 출발점이 생겼다.

# 2026-06-12-02 - Dev_Prototype_v1 M1/M2 루프 보강

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 이제 단순 생성 확인을 넘어서, M1 전투 셸과 M2 압축 루프를 Play Mode에서 강제로 확인할 수 있다. 메인 씬은 계속 `Assets/_dev/Scenes/Dev_Prototype_v1.unity`다.

## 2. 오늘 바뀐 것

- `M1 Smoke`, `M2 Loop`, `Continue` 디버그 버튼을 추가했다.
- `F8`로 M2 압축 스모크를 실행할 수 있게 했다.
- 적 처치가 즉시 XP로 바뀌지 않고 XP 오브를 생성하도록 바꿨다.
- 원거리 적 탄환이 플레이어에게 피해를 주도록 연결했다.
- M2 압축 루프에서 최고 레벨 망각, 결과 진행, 공명 재획득, 칼무리/혈반 +5, 피의 칼폭풍이 한 번에 검증되게 했다.
- XP 오브 수집 중 리스트가 바뀌며 터지던 런타임 예외를 고쳤다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 0 warning / 0 error.
- Unity compile errors: `count=0`.
- M2 압축 스모크 120프레임 후 스냅샷: `kills=10`, `level=2`, `echoes=[HungryBlades:5,BloodReflection:5]`, `storm=True`, `death=False`.
- Unity console errors: `count=0`.
- 증거 캡처 저장: `LETHE/Assets/_dev/Evidence/v1_m2_smoke_20260612.png`.
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과, 2개 unit heading 확인.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패.

## 4. 결정한 것

- v1은 계속 메인 경로다.
- 현재 M2는 실제 페이싱이 아니라 압축 주입이다. 상태 전환과 전투 연결을 확인하는 용도다.
- 다음 판단은 jaewoo가 직접 Play Mode에서 `M1 Smoke`와 `M2 Loop`를 눌러보고 한다.

## 5. 문제 또는 리스크

- 아직 실제 플레이로 60~120초 안에 M2에 자연 도달하는 구조는 아니다.
- XP/망각/공명은 동작하지만, 감정 페이싱은 다음 패스에서 봐야 한다.
- 일부 그래픽은 여전히 임시 런타임 도형/크롭 기반이다.
- Project Orchestrator Discord intake가 현재 응답하지 않아 dry-run 보고가 실패했다.

## 6. GPT/Claude 인계 요약

v1은 새 문서 기준으로 다시 만든 런타임이며, M2 압축 스모크가 예외 없이 통과했다. 이제 “이 방향이 맞는지”를 사람 플레이 리뷰로 판단한 뒤, 맞다면 압축 루프를 실제 Gatekeeper 페이싱으로 풀면 된다.

## 7. 다음 Codex 작업

- jaewoo 리뷰 결과를 받아 `GO/ITERATE/NO-GO` 정리.
- `GO/ITERATE`면 M2 실제 페이싱 구현.
- 이후 데이터 자산화와 프리팹 분리를 진행.

## 8. 포트폴리오 메모

- 문제: 이전 Unity 프로토타입은 구현 shortcut 때문에 기획 감정 루프를 평가하기 어려웠다.
- 방향: 먼저 상태 전환이 깨지지 않는 압축 루프를 만들고, 그 뒤 실제 페이싱으로 펼친다.
- 행동: Unity MCP로 Play Mode를 돌려 M2 루프를 직접 검증했다.
- 결과: v1에서 망각→잔향+5→공명→피의 칼폭풍까지 한 번에 재현 가능한 기준점이 생겼다.

# 2026-06-12-03 - Dev_Prototype_v1 M1 전투 손맛 패스

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 카메라/캐릭터 크기 기준을 유지한 채, 기본 쌍검 공격과 레벨업 UI를 한 차례 보강한 상태다. 다음 리뷰는 `M1 Smoke`와 일반 플레이에서 쌍검 손맛, 피격 반응, 칼무리 크기, XP bar, 3카드 선택 UI를 보는 것이다.

## 2. 오늘 바뀐 것

- 쌍검 기본공격을 단일 부채꼴에서 좌/우 교차 2참격 VFX로 바꿨다.
- 무기 스프라이트가 공격 순간 짧게 흔들리도록 했다.
- 무기 피격 시 hitstop, 약한 카메라 흔들림, 피격 섬광, 적 flash, knockback, squash 반응을 추가했다.
- 칼무리 잔향 slash 크기와 alpha를 줄였다.
- 활성 칼무리 궤도 잔상도 조금 더 얇고 짧게 낮췄다.
- HUD에 HP bar와 XP bar를 추가했다.
- 레벨업 선택 UI를 큰 3카드 구조로 바꿨고, 카드마다 태그/이름/설명을 분리했다.
- `F8` 안내와 실제 입력 매핑이 어긋나던 문제를 고쳤다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 7 warning / 0 error. warning은 기존 v0/debug 코드의 Unity deprecated API다.
- Unity compile errors: `count=0`.
- M1 smoke 스냅샷: `kills=4`, `level=2`, `xp=1/9`, `dualSlash=12`, `hitSpark=6`, `xpOrb=4`, `death=False`.
- Unity console errors: `count=0`.
- 증거 렌더 저장: `LETHE/Assets/_dev/Evidence/v1_combat_feel_pass_20260612.png`.
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과, 3개 unit heading 확인.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패.

## 4. 결정한 것

- XP 오브는 뱀서류 장르 문법으로 사용해도 된다. 다만 최종 표현은 LETHE식 기억 조각/망각 파편으로 바꾼다.
- 기본공격은 각 무기의 핵심 재미여야 한다. 잔향은 그 위에 얹히는 빌드 변형이어야 한다.
- 무기별 잔향 시너지는 앞으로 쌍검/대검의 리듬 차이를 반영해 다르게 설계한다.

## 5. 문제 또는 리스크

- 아직 사운드가 없어 hitstop/flash만으로 손맛을 판단해야 한다.
- 직접 렌더 증거는 forced manager-update smoke 뒤라 칼무리 transient가 실제 Play Mode보다 조밀하게 보일 수 있다.
- 쌍검은 첫 손맛 패스일 뿐이고, 기본공격/피격효과 최종 규칙은 jaewoo와 추가 상의가 필요하다.
- Project Orchestrator Discord intake가 현재 응답하지 않아 dry-run 보고가 실패했다.

## 6. GPT/Claude 인계 요약

이번 패스는 M2 확장이 아니라 M1 전투 손맛 보강이다. 카메라/캐릭터 크기는 유지하고, 쌍검 공격과 피격 반응, XP/선택 UI를 먼저 게임답게 만들었다. 다음 판단은 실제 Play Mode에서 쌍검만으로 사냥이 재미있는지다.

## 7. 다음 Codex 작업

- jaewoo가 쌍검 손맛과 3카드 UI를 직접 확인한다.
- 기본공격/피격효과의 무기별 규칙을 확정한다.
- M1이 통과하면 M2 실제 페이싱으로 넘어간다.

## 8. 포트폴리오 메모

- 문제: 시스템은 연결됐지만 기본공격이 재미없어 게임으로 평가하기 어려웠다.
- 방향: 망각/잔향 확장 전에 무기 자체의 전투 감각을 먼저 살린다.
- 행동: 쌍검 slash, hit feedback, XP bar, 3카드 UI를 구현하고 Unity MCP로 검증했다.
- 결과: M1 전투 리뷰 기준이 “시스템이 있나”에서 “쌍검으로 사냥이 재미있나”로 올라갔다.

# 2026-06-12-04 - 무기 리듬과 잔향 변형 법칙 보정

## 1. 현재 빌드 상태

이번 단위는 코드 구현이 아니라 설계 보정이다. Claude가 제안한 `허공 베기 금지`, `쌍검=nearest`, `대검=densest arc`, `잔향/궁극은 무기 리듬을 탄다`는 방향을 수용하되, 단일 `echoScale` 하나로 모든 차이를 처리하지 않도록 문서를 보정했다.

## 2. 오늘 바뀐 것

- `LETHE_DESIGN_02_COMBAT.md`에 기본공격 트리거/조준 모델을 확정했다.
- 근접 무기는 적이 사거리 안에 없으면 허공에 스윙하지 않는다.
- 쌍검은 가장 가까운 적을 향해 빠르게 베고, 대검은 가장 많이 맞는 부채꼴 방향을 고른다.
- `echoScale` 단일값 대신 `echoSizeScale`, `echoDamageScale`, `echoProcStyle`, `ultimatePattern`으로 분리했다.
- `LETHE_DESIGN_03_MEMORY_ECHO.md`에 피의 칼폭풍의 쌍검/대검 표현 차이를 고정 규칙으로 적었다.
- `LETHE_DESIGN_06_BUILD_PLAN.md`의 `WeaponDefinition` 필드를 새 구조로 바꿨고 대상 씬을 `Dev_Prototype_v1`로 맞췄다.
- `DECISION_LOG.md`에 `DEC-2026-06-12-04`를 정리했다.

## 3. 테스트 결과와 근거

- `npm.cmd run report:check`: 통과.
- `npm.cmd run report`: 통과, 4개 unit report 생성.
- `npm.cmd run report:check`: 통과, 4개 unit heading 확인.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패.

## 4. 결정한 것

- 기본공격은 허공에 나가지 않는다.
- 기본공격은 히어로 딜이 아니라 온힛 잔향을 전달하는 장치다.
- 잔향/궁극 정의는 하나만 두고, 무기별 리듬 파라미터로 맛을 바꾼다.
- 피의 칼폭풍 쌍검 버전은 작은 혈검 다수, 빠른 회전, 여러 번 베는 연쇄감으로 간다.
- 피의 칼폭풍 대검 버전은 큰 혈검 소수, 느린 회전 또는 큰 2~3회 참격, 강한 hitstop, 큰 화면 흔들림으로 간다.

## 5. 문제 또는 리스크

- 아직 구현은 문서를 따라가지 않는다. 현재 v1 쌍검은 허공 베기 제거와 nearest targeting이 다음 구현 과제다.
- `echoSizeScale`/`echoDamageScale` 수치는 첫값이므로 실제 플레이에서 조정될 수 있다.
- Project Orchestrator Discord intake가 현재 응답하지 않아 dry-run 보고가 실패했다.

## 6. GPT/Claude 인계 요약

Claude의 방향은 수용했다. 다만 단일 `echoScale`은 너무 거칠어서 크기/피해/형태/궁극 패턴으로 분리했다. 다음 Codex 구현은 이 문서를 기준으로 기본공격 타겟팅과 칼무리 잔향의 무기별 표현을 바꿔야 한다.

## 7. 다음 Codex 작업

- v1 쌍검 허공 베기 제거.
- 쌍검 nearest targeting 구현.
- 칼무리 잔향을 쌍검 `MultiSmall` 방식으로 적용.
- 대검 추가 전 `DensestArc`, `SingleHeavy`, `FewHeavy` 구조 준비.

## 8. 포트폴리오 메모

- 문제: 기본공격이 허공에 나가고, 무기별 잔향 차이가 스케일 하나로 뭉개질 위험이 있었다.
- 방향: 무기 리듬이 잔향과 궁극의 형태를 결정하는 구조로 문서화했다.
- 행동: 전투/기억/구현계획/결정 로그를 같은 용어로 맞췄다.
- 결과: `무기 + 기억 + 잔향` 조합의 설계 축이 더 선명해졌다.

# 2026-06-12-05 - 쌍검 허공 베기 제거와 칼무리 MultiSmall 구현

## 1. 현재 빌드 상태

`Dev_Prototype_v1` 쌍검은 이제 적이 없을 때 허공에 스윙하지 않는다. 적이 사거리 안에 들어오면 가장 가까운 적을 향해 자동 조준하고, 그 방향으로 쌍검 참격과 칼무리 잔향이 발생한다.

## 2. 오늘 바뀐 것

- 쌍검 기본공격에 `engageRadius = range * 1.15`를 적용했다.
- 사거리 안에 적이 없으면 스윙 VFX/피해/잔향 proc이 발생하지 않게 했다.
- 쌍검 타겟팅을 nearest enemy 기준으로 바꿨다.
- 무기 앵커가 타겟 방향으로 회전한 뒤 참격 VFX를 만든다.
- 칼무리 잔향을 쌍검 `MultiSmall` 방식으로 바꿨다.
- `칼무리 잔향 소참`은 적중 적 중심에 작은 slash 여러 개와 작은 범위 피해로 나온다.
- +5 칼무리 발사 칼날이 너무 커서 화면을 덮던 문제를 고쳤다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 7 warning / 0 error. warning은 기존 v0/debug 코드의 Unity deprecated API다.
- Unity compile errors: `count=0`.
- Play Mode targeted smoke: `noAirBefore=0`, `noAirAfter=0`, `slashAfterTarget=3`, `kalmuriSmall=3`, `launch=1`, `hitSpark=2`.
- Unity console errors: `count=0`.
- 증거 렌더 저장: `LETHE/Assets/_dev/Evidence/v1_no_air_swing_kalmuri_multismall_20260612.png`.
- `npm.cmd run report`: 통과, 5개 unit report 생성.
- `npm.cmd run report:check`: 통과, 5개 unit heading 확인.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패.

## 4. 결정한 것

- 근접 무기는 적이 없으면 스윙하지 않는다.
- 쌍검은 nearest target으로 간다.
- 쌍검 칼무리 잔향은 큰 한 방이 아니라 작은 소참 여러 개로 간다.
- 대검은 다음 단계에서 `DensestArc`, `SingleHeavy`, `FewHeavy` 구조로 준비한다.

## 5. 문제 또는 리스크

- 아직 대검은 구현되지 않았다.
- 현재 무기 전략이 `V1GameManager` 안에 남아 있어 다음 단계에서 분리가 필요하다.
- 사운드가 없어 쌍검 손맛은 여전히 VFX/hitstop 중심으로 판단해야 한다.
- Project Orchestrator Discord intake가 현재 응답하지 않아 dry-run 보고가 실패했다.

## 6. GPT/Claude 인계 요약

DEC-2026-06-12-04의 첫 코드 반영이 끝났다. 허공 베기는 제거됐고, 쌍검은 nearest target을 향해 베며, 칼무리 잔향은 `MultiSmall`로 표현된다. 다음은 대검을 위한 targeting/proc 구조 분리다.

## 7. 다음 Codex 작업

- jaewoo가 실제 Play Mode에서 no-air-swing 체감 확인.
- 대검용 `DensestArc` 타겟팅 준비.
- 잔향 proc을 `MultiSmall`/`SingleHeavy` 전략으로 분리.

## 8. 포트폴리오 메모

- 문제: 자동 근접무기가 허공에 스윙해 잔향 중심 게임의 의미가 흐려졌다.
- 방향: 공격은 적이 있을 때만 발생하고, 무기 리듬이 잔향 형태를 결정하게 한다.
- 행동: 쌍검 nearest targeting과 칼무리 `MultiSmall`을 구현하고 Unity MCP로 검증했다.
- 결과: 무기+잔향 조합이 시스템 문서에서 실제 전투 코드로 내려오기 시작했다.
