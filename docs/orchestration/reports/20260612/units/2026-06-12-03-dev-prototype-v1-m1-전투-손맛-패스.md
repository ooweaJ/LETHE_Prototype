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
