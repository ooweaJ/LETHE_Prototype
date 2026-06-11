# 2026-06-11-11 - LETHE 아트 전면 교체와 sprite VFX 적용

## 1. 현재 빌드 상태

`Dev_Prototype_v0`의 player, enemy, map, weapon, memory/echo VFX를 LETHE 컨셉 기준으로 교체했다. 초록 배경은 source chroma-key용이고, Unity runtime은 alpha PNG를 사용한다.

## 2. 오늘 바뀐 것

- `LETHE_UNITY_ART_DIRECTION_REPLACEMENT_PLAN.md`를 추가했다.
- imagegen으로 LETHE용 리소스를 새로 만들었다:
  - weaponless player 4방향 sheet.
  - erased memory husk enemy 4방향 sheet.
  - dark river/obsidian floor tile.
  - cyan memory-glass short dual blades.
  - Kalmuri/Blood/Blood Blade Storm VFX atlas.
- source 이미지는 `Assets/_dev/Art/Source`에 보존했다.
- Unity runtime 이미지는 alpha PNG로 변환했다.
- `PrototypeSpriteVfx`를 추가했다.
- `PrototypeGameManager`가 기억/잔향/궁극 상태에서 sprite VFX를 spawn하도록 연결했다.
- evidence screenshot을 추가했다:
  - `LETHE/Assets/_dev/Evidence/lethe_art_replacement_vfx_game.png`

## 3. 테스트 결과와 근거

- Unity compile errors: `0`.
- Scene missing references: `0`.
- Play Mode console errors: `0`.
- Runtime sprite VFX reference 확인:
  - `spr_kalmuri_orbit_blade_01`
  - `spr_kalmuri_echo_slash_01`
  - `spr_blood_mark_01`
  - `spr_blood_bloom_01`
  - `spr_blood_blade_storm_ring_01`
- Direct review spawn으로 sprite VFX 오브젝트 생성 확인.
- editor state after stop: `Dev_Prototype_v0`, `sceneDirty=false`.
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패. 중앙 Discord intake 연결이 현재 환경에서 닿지 않는다.

## 4. 결정한 것

- 초록 배경은 최종 리소스가 아니라 chroma-key source로만 사용한다.
- Unity에는 alpha PNG를 연결한다.
- line-renderer VFX는 당장 삭제하지 않고, damage/log 안정성을 유지하면서 sprite VFX를 추가한다.
- 폰트 작업은 다음 단계로 분리한다.

## 5. 문제 또는 리스크

- OnGUI 선택 UI가 아직 중앙 VFX를 가린다.
- VFX는 화려해졌지만, 최종 게임용으로는 pooling/animation/sorting 정리가 필요하다.
- `Assets/_dev/Fonts/`에 로컬 Pretendard 파일이 있으나 이번 범위가 아니므로 커밋하지 않는다. 씬의 `koreanFont` 참조도 비워둬서 커밋된 빌드가 untracked 폰트에 의존하지 않게 했다.
- Discord 보고는 중앙 intake 연결 실패로 미전송이다.

## 6. GPT/Claude 인계 요약

LETHE prototype의 시각 기준을 placeholder에서 dark fantasy memory/forgetting 방향으로 바꿨다. 다음 평가는 sprite 자체가 LETHE처럼 보이는지, VFX가 기억/잔향/궁극 차이를 충분히 보여주는지다.

## 7. 다음 Codex 작업

- OnGUI HUD를 제거하고 한글 폰트/TextMeshPro UI로 교체.
- sprite VFX를 pooling/animation 기반으로 정리.
- memory choice overlay가 전투 화면을 가리지 않도록 UI 배치 재설계.

## 8. 포트폴리오 메모

- 문제: prototype sprite는 시스템 평가를 방해할 만큼 임시 느낌이 강했다.
- 방향: LETHE의 망각/기억/혈색/칼무리 컨셉이 화면에서 먼저 읽히게 만든다.
- 행동: player/enemy/map/weapon/VFX를 전면 교체하고 Unity runtime VFX spawn 경로를 추가했다.
- 결과: 폰트/UI를 제외한 핵심 gameplay 화면이 LETHE 컨셉 평가 가능한 수준으로 이동했다.
