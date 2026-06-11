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
