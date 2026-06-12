# 07. 에셋 / VFX / 클래스 연결

최종 갱신: 2026-06-12 · 대상 씬: `Assets/_dev/Scenes/Dev_Prototype_v0.unity`

이 문서는 기획(00~05)과 빌드 계획(06)을 **실제 이미지·프리팹·ScriptableObject·클래스에 연결**하는 제작 기준이다. 이전 5개 에셋 문서(SLICE_ASSET_PRODUCTION / ASSET_BINDING / VISUAL_ASSET / ART_DIRECTION_REPLACEMENT / RELEASE_ART_FONT_VFX)를 이 문서로 통합·대체했다. 범위는 1콤보 쇼케이스가 아니라 **무기 2 + 기억 8 + 잔향 8 + 궁극 4 + 적 4 + 보스** 전체다.

## 0. 제작 원칙

- 시선 우선순위: `잔향/궁극 VFX > 활성 기억 VFX > 무기 > 적 > 캐릭터 > 맵`.
- **파일 1개 = 역할 1개**. 한 이미지에 여러 파츠를 섞지 않는다.
- 캐릭터/적/맵은 처음부터 예쁘게 만들지 않는다. VFX 판독을 위한 무대다.
- "스프라이트가 덜 예뻐도 역할이 읽히면 통과, 역할이 안 읽히면 아무리 예뻐도 실패."
- 캐릭터와 무기는 분리한다. 캐릭터는 무기 없는 body, 무기는 `WeaponAnchor` 자식 sprite.
- line renderer는 임시 보조만. 최종은 sprite/trail/particle/pool 기반.
- Unity 런타임에는 alpha PNG만 연결한다(`#00ff00` chroma는 source 보존용).

## 1. 시각 레이어 계약 (Visual Layer Contract)

| 레이어 | 역할 | 금지 |
| --- | --- | --- |
| Character | 몸·방향·이동 상태만 읽게 | 큰 무기를 굽지 않음 |
| Weapon | 현재 무기 종류만 작게 알림 | 공격 판독을 책임지지 않음(VFX가 함) |
| Active Memory VFX | 망각 전 성장 재미. 기억이 몸/전장에 독립적으로 존재 | 약화판처럼 보이면 안 됨 |
| Echo VFX | 망각 후 "무기에 남은 습관". 활성 기억과 형태가 달라야 함 | "약해진 같은 스킬"로 보이면 실패 |
| Ultimate VFX | +5 두 잔향 조합의 후반 목표. 두 잔향이 서로 먹여살리는 루프 | 화면 전체를 가려 전투 판독을 삼키면 실패 |

## 2. 폰트 정책

| 항목 | 값 |
| --- | --- |
| Font | Pretendard (Regular/SemiBold/Bold) |
| License | SIL Open Font License 1.1 (`Pretendard-LICENSE.txt` 동봉) |
| Unity path | `Assets/_dev/Fonts/Pretendard-*.otf` |
| 사용 | `_dev`는 OnGUI/koreanFont로 한글 판독 확인, 정식은 TextMeshPro 승격 |

## 3. imagegen 제작 규칙

투명 PNG 절차:
1. imagegen으로 평평한 `#00ff00` chroma-key 배경 이미지 생성.
2. 로컬 helper로 chroma→alpha 제거.
3. 결과 PNG를 `Assets/_dev/Art/Sprites/...`에 저장, source는 `Assets/_dev/Art/Source/`에 보존.
4. Unity MCP로 Sprite import setting 적용(100 PPU 기준, VFX는 1024 원본).

공통 프롬프트 prefix:
```text
Use case: stylized-concept
Asset type: Unity 2D top-down roguelite runtime sprite
Style: dark fantasy 2D, high-contrast readable silhouette, simplified for small scale
Composition: single isolated asset centered, generous padding, top-down
Background: perfectly flat solid #00ff00 chroma-key
Constraints: no text, no watermark, no cast shadow, no background texture; do not use #00ff00 in subject
```

해상도/PPU:
| 자산 | 원본 | PPU |
| --- | --- | --- |
| 캐릭터/적 | 512² (4방향 시트 별도) | 100~115 |
| 무기 | 512² | 100 (blade PPU는 0.25 scale 기준 조정) |
| 칼선/VFX | 1024² | 100 |
| 타일 | 512² | 100 |
| 궁극 링 | 1024² | 100 |

검수: chroma 제거 후 네 모서리 투명 / 축소 미리보기에서 subject가 한 덩어리로 뭉치면 재생성 / 플레이어·적·맵이 VFX보다 밝으면 채도 낮춤.

## 4. 폴더 구조

작업 루트는 `Assets/_dev/`. GO 후에만 `Assets/Lethe/`로 승격(경로만 바뀌고 data id는 동일 유지).

```text
Assets/_dev/
  Art/
    Source/                      # chroma 원본 보존
    Sprites/
      Characters/Player/
      Enemies/{Chaser,RangedEye,Splitter,VoidPriest}/
      Map/
      Weapons/{DualBlades,Greatsword}/
      Memories/{Kalmuri,Blood,Execution,Homing,Shockwave,TimeStop,Ashen,Brand}/
      Echoes/{...same 8...}/
      Ultimates/{BloodBladeStorm,ExecutionBrand,FrozenHunt,BastionWave}/
    Materials/
    Particles/
  Prefabs/{Characters,Enemies,Map,Weapons,Memories,Echoes,Ultimates,UI,Debug}/
  Scripts/{Prototype,Runtime,Feedback,Debug}/
  Data/{Weapons,Memories,Echoes,EchoSynergies,Enemies,FeedbackProfiles}/
  Scenes/Dev_Prototype_v0.unity
  Fonts/
```

## 5. id 안정성 (절대 규칙)

`Weapon_DualBlades`, `Memory_HungryBlades`, `Echo_Kalmuri`, `Synergy_BloodBladeStorm` 같은 data id는 `_dev`↔`Assets/Lethe`에서 동일하게 유지한다. id를 바꾸면 저장 데이터, debug panel, reward table, report evidence가 끊긴다.

## 6. 무기 에셋 매트릭스

| 무기 id | sprite | swing VFX | prefab | 주 클래스 |
| --- | --- | --- | --- | --- |
| `Weapon_DualBlades` | `spr_weapon_dual_blade_left/right_01` | `spr_dual_blade_swing_arc_01/02` (얇고 짧게) | `Weapon_DualBlades_Runtime` | `DualBladesController`, `WeaponHitEmitter` |
| `Weapon_Greatsword` | `spr_weapon_greatsword_01` (신규) | `spr_greatsword_cleave_arc_01` (느리고 넓게) | `Weapon_Greatsword_Runtime` | `GreatswordController`, `WeaponHitEmitter` |

수치는 [02_COMBAT](LETHE_DESIGN_02_COMBAT.md). 같은 잔향도 무기별로 형태/리듬이 달라야 한다(쌍검=작게 자주, 대검=크게 조건부).

## 7. 캐릭터 / 적 / 맵 에셋

| 요소 | 파일 | 4방향 시트 | prefab | 주 클래스 |
| --- | --- | --- | --- | --- |
| 플레이어 | `sheet_player_4dir` (무기 없음, 검은 망토/흰 가면/청백 기억광) | 8행×4열(idle/walk × 4dir) | `Prefab_Player_Prototype` | `DevPlayerController2D`, `Health`, `MemoryInventory`, `EchoInventory`, `WeaponSlot` |
| 침식자 | `sheet_enemy_chaser_4dir` | 8×4 | `Prefab_Enemy_MeleeChaser` | `DevEnemyChaseController`, `Health`, `HitReceiver` |
| 떠도는 눈 | `sheet_enemy_eye_4dir` (신규) | 원거리, 짧게 물러나 멈춰 쏨 | `Prefab_Enemy_RangedEye` | `EnemyRangedController` |
| 쪼개진 자 | `sheet_enemy_splitter_4dir` (신규) | 처치 시 분열 | `Prefab_Enemy_Splitter` | `EnemySplitterController` |
| 공허 사제 | `sheet_enemy_voidpriest_4dir` (신규) | 아군 회복 | `Prefab_Enemy_VoidPriest` | `EnemyHealerController` |
| 보스 문지기 | `spr_boss_gatekeeper_01` (신규) | HP 페이즈 66/33%, 그로기 | `Prefab_Boss_Gatekeeper` | `BossController` |
| 맵 | `tile_dev_floor_dark_01` (검은 강/석판/기억 균열) | 반복 타일 | `Dev_Arena` | `DevArenaBounds`, `SpawnArea` |

적 스탯은 [02_COMBAT](LETHE_DESIGN_02_COMBAT.md). 적이 모두 근접이면 기억 차이가 안 보이므로 원거리/분열/지원 4종 구현 필수.

## 8. 8기억 / 8잔향 VFX·클래스 매트릭스

각 기억은 활성(독립 객체)과 잔향(무기에 남은 흔적)의 **형태가 달라야** 한다. 칼무리/혈반은 전용 sprite 존재, 나머지 6종은 신규 생성 대상.

| 기억 id | 활성 VFX (sprite) | 잔향 id | 잔향 VFX | 활성 클래스 | 잔향 클래스 | 상태 |
| --- | --- | --- | --- | --- | --- | --- |
| `Memory_HungryBlades` | 청백 칼날 고리 `spr_kalmuri_orbit_blade_01` | `Echo_Kalmuri` | 반달 칼자국 `spr_kalmuri_echo_slash_01`, +5 발사칼날 `spr_kalmuri_launch_blade_01` | `HungryBladesMemoryRuntime` | `KalmuriEchoRuntime` | ✅ 존재 |
| `Memory_BloodReflection` | 피·거울 반사 문양 `spr_blood_mark_01` | `Echo_Blood` | 혈반 표식+회복 실+ +5 피꽃 `spr_blood_bloom_01` | `BloodReflectionRuntime` | `BloodEchoRuntime` | ✅ 존재 |
| `Memory_ExecutionFlash` | 위협 적 위 하얀 섬광 `spr_execution_flash_01` | `Echo_Execution` | 처형 조건 하얀 폭발/균열 `spr_execution_burst_01` | `ExecutionMemoryRuntime` | `ExecutionEchoRuntime` | 🔲 신규 |
| `Memory_HunterOath` | 유도 투사체 `spr_homing_shot_01` | `Echo_Homing` | 유도 잔탄/그림자 추적 `spr_homing_echo_01` | `HunterOathRuntime` | `HomingEchoRuntime` | 🔲 신규 |
| `Memory_ShatterWave` | 충격파+넉백 `spr_shockwave_ring_01` | `Echo_Shockwave` | 타격 위치 파문/안전지대 `spr_shockwave_echo_01` | `ShatterWaveRuntime` | `ShockwaveEchoRuntime` | 🔲 신규 |
| `Memory_StoppedSecond` | 둔화장/시계 고리 `spr_timestop_field_01` | `Echo_TimeStop` | 시간 균열/둔화 `spr_timestop_echo_01` | `StoppedSecondRuntime` | `TimeStopEchoRuntime` | 🔲 신규 |
| `Memory_AshenShield` | 보호막 고리 `spr_ashen_shield_01` | `Echo_AshenGuard` | 피격/보호막 파괴 반격 파동 `spr_ashen_echo_01` | `AshenShieldRuntime` | `AshenGuardEchoRuntime` | 🔲 신규 |
| `Memory_OblivionBrand` | 취약 표식 `spr_brand_mark_01` | `Echo_Brand` | 낙인 폭발/전파 `spr_brand_echo_01` | `OblivionBrandRuntime` | `BrandEchoRuntime` | 🔲 신규 |

공통 보조 컴포넌트: `PooledVfxHandle`, `EchoHitEmitter`, `EchoProcLimiter`(잔향), `PooledProjectile`(발사형). 레벨별(+1/+3/+5) 차이는 [03_MEMORY_ECHO](LETHE_DESIGN_03_MEMORY_ECHO.md) 수락 기준 참조.

## 9. 4궁극 VFX·클래스 매트릭스

| 궁극 id | 필요 잔향(+5) | VFX | prefab | 주 클래스 | 상태 |
| --- | --- | --- | --- | --- | --- |
| `Synergy_BloodBladeStorm` | Kalmuri + Blood | 칼날 고리+붉은 회복 실 `spr_blood_blade_storm_ring/blade_01` | `Ultimate_BloodBladeStorm` | `BloodBladeStormRuntime` | ✅ 존재 |
| `Synergy_ExecutionBrand` | Execution + Brand | 하얀 폭발+검은 파편 연쇄 `spr_execution_brand_01` | `Ultimate_ExecutionBrand` | `ExecutionBrandRuntime` | 🔲 신규 |
| `Synergy_FrozenHunt` | Homing + TimeStop | 둔화장+추적 잔탄 분열 `spr_frozen_hunt_01` | `Ultimate_FrozenHunt` | `FrozenHuntRuntime` | 🔲 신규 |
| `Synergy_BastionWave` | Shockwave + AshenGuard | 큰 파문+안전지대 `spr_bastion_wave_01` | `Ultimate_BastionWave` | `BastionWaveRuntime` | 🔲 신규 |

궁극 공통: 모든 타격은 `UltimateHit`로 발행해 일반 온힛 잔향을 재호출하지 않음. 회복/처치/전파에 cap. 보조: `EchoProcLimiter`, `OrbitShardController`, `SfxLayerSource`.

## 10. ScriptableObject 연결

| SO 종류 | 연결 대상 |
| --- | --- |
| `Weapon_*.asset` | 무기 runtime prefab + swing arc sprite |
| `Memory_*.asset` | 활성 기억 runtime prefab + 활성 VFX |
| `Echo_*.asset` | 잔향 runtime prefab(+1/+3/+5) + 잔향 VFX |
| `Synergy_*.asset` | 궁극 prefab + 궁극 VFX |
| `Enemy_*.asset` | 적 prefab + 4방향 시트 |
| `Feedback_*.asset` | hitstop/flash/shake/sfx 강도 |

## 11. Unity MCP 실행 순서

1. AnkleBreaker MCP 상태 확인: `unity_list_instances`, `unity_editor_ping(port=7890)`.
2. `Assets/_dev/...` 폴더 구조 생성/확인.
3. imagegen으로 미보유 sprite 생성(우선순위 §12) → chroma 제거 → `Art/Sprites`에 배치, source 보존.
4. `unity_asset_list`로 인식 확인 → Sprite import setting 적용.
5. Definition SO 생성/갱신(수치는 02/03/04 표).
6. prefab 생성 + sprite/VFX/SO 연결(§6~10 매트릭스).
7. `Dev_Prototype_v0`에 배치 + debug panel 연결.
8. 검증: compile 0 / console 0 / missing refs 0 / Play Mode 스모크.

## 12. 제작 우선순위

1. **M1 게임 셸용**: player 4dir, 침식자 4dir, 맵 타일, 쌍검 sprite/arc — 이미 존재(placeholder 갱신).
2. **M2 코어용**: 칼무리/혈반 활성·잔향·피의 칼폭풍 — 이미 존재(퀄 polish).
3. **M3**: 대검 sprite + cleave arc.
4. **M4**: 나머지 6기억 활성/잔향 VFX(처형·추적·파문·정지·잿빛·낙인) + 떠도는 눈/쪼개진 자/공허 사제 4dir.
5. **M5**: 3궁극 VFX(처형 각인·정지 추적·성채 파문) + 보스 문지기 sprite.
6. HUD 아이콘 세트(기억/잔향 슬롯, +5 각성 badge, 궁극 progress).

원칙: 효과가 이 게임의 첫 인상이므로 캐릭터/맵보다 VFX 파츠가 먼저다. 단 배경은 어둡고 단순해야 흰 칼선·붉은 혈반이 읽힌다.

## 13. 완료 기준

- 8기억/8잔향/4궁극/2무기/적4+보스가 각각 어떤 sprite·prefab·클래스를 쓰는지 매트릭스로 명확.
- 미보유 파일은 placeholder인지 다음 imagegen 대상(🔲)인지 구분됨.
- line renderer 의존이 sprite/trail/particle로 대체됨.
- debug panel에서 각 상태 VFX를 즉시 켤 수 있음.
- `Dev_Prototype_v0`에서 compile/console/missing refs 0.
