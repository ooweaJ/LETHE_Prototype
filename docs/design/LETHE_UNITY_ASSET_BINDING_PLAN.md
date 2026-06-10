# LETHE Unity 에셋-프리팹 연결 계획

최종 갱신: 2026-06-10

## 0. 이 문서의 위치

이 문서는 Unity MCP가 실제 Unity 2D 프로젝트를 열었을 때, 어떤 이미지 파일을 어디에 import하고 어떤 프리팹/ScriptableObject에 연결해야 하는지 정의한다.

관련 문서:

- `LETHE_VISUAL_ASSET_PLAN.md`: 첫 스프라이트/VFX 콘셉트와 파츠 계획.
- `LETHE_UNITY_SLICE_ASSET_PRODUCTION_PLAN.md`: 어떤 이미지를 어떤 순서로 만들고 `_dev`에서 검증할지 정하는 제작/테스트 플랜.
- `LETHE_UNITY_ECHO_SYSTEM_PRD.md`: Unity 클래스, SO, prefab 구조.
- `LETHE_ECHO_FORM_TRANSFORMATION_SPEC.md`: 잔향 형태 변환 문법.

핵심:

```text
이미지 파일 -> Unity Art 폴더 -> Sprite/Material/Particle -> Prefab -> ScriptableObject -> Scene
```

MCP가 이 문서를 보면 "이 파일을 이 경로로 import하고, 이 prefab의 이 역할에 연결한다"까지 따라 할 수 있어야 한다. 단, 첫 실험은 `LETHE_UNITY_SLICE_ASSET_PRODUCTION_PLAN.md`에 따라 `Assets/_dev/`에서 먼저 진행하고, 검증된 구조만 `Assets/Lethe/`로 승격한다.

## 0-A. `_dev` staging과 `Assets/Lethe` 승격 규칙

현재 작업의 실제 대상은 `Assets/_dev`다. 이 문서의 `Assets/Lethe` 경로는 첫 slice가 GO 판정을 받은 뒤 승격할 최종 경로다.

| 단계 | 경로 | 용도 | 규칙 |
| --- | --- | --- | --- |
| 실험/staging | `Assets/_dev/...` | 이미지 생성, import, C# 런타임, prefab, debug scene 검증 | 현재 모든 MCP 작업은 여기서 시작 |
| 승격/main | `Assets/Lethe/...` | GO 판정 후 유지할 실제 게임 구조 | jaewoo 리뷰 전에는 직접 생성하지 않음 |

경로 변환 규칙:

```text
Assets/_dev/Art/Sprites/...      -> Assets/Lethe/Art/Sprites/...
Assets/_dev/Prefabs/...          -> Assets/Lethe/Prefabs/...
Assets/_dev/Scripts/...          -> Assets/Lethe/Scripts/...
Assets/_dev/Data/...             -> Assets/Lethe/Data/...
Assets/_dev/Scenes/Dev_EchoSlice -> Assets/Lethe/Scenes/Slice_EchoShowcase
```

id 안정성:

- `Weapon_DualBlades`, `Memory_HungryBlades`, `Echo_Kalmuri` 같은 data id는 `_dev`와 `Assets/Lethe`에서 동일하게 유지한다.
- 승격 때 바뀌는 것은 파일 경로와 prefab 위치뿐이다.
- id를 바꾸면 저장 데이터, debug panel, reward table, report evidence가 끊기므로 금지한다.

현재 slice에서 MCP가 생성해야 하는 경로는 아래 `_dev` 기준이다.

```text
Assets/_dev/
  Art/
  Prefabs/
  Scripts/
  Data/
  Scenes/
```

## 1. 현재 사용 가능한 이미지

| 구분 | 현재 파일 | Unity 용도 | 바로 런타임 사용 여부 |
| --- | --- | --- | --- |
| 첫 잔향 콘셉트 시트 | `docs/design/assets/lethe-first-echo-showcase-concept.png` | 칼날, 혈반, 회복 실, 피의 칼폭풍 시각 기준 | 아니오. `Art/Concept/` 참조용 |

주의:

- 이 이미지는 한 장짜리 콘셉트 시트라서 Unity에 넣을 수는 있지만, 런타임 sprite atlas로 바로 쓰기엔 부적합하다.
- Unity에는 먼저 `Assets/Lethe/Art/Concept/lethe-first-echo-showcase-concept.png`로 import한다.
- 실제 프리팹에는 별도의 투명 PNG 파츠를 연결한다.

## 2. 첫 Unity slice 에셋 큰 지도

| 게임 요소 | Unity 프리팹 | 필요한 이미지 파일 | 현재 상태 | 처리 |
| --- | --- | --- | --- | --- |
| 플레이어 캐릭터 | `Player_EchoShowcase` | `spr_player_echo_showcase_placeholder.png` | 없음 | 첫 Unity slice는 단색 실루엣 placeholder 또는 다음 imagegen 대상 |
| 테스트 맵 | `Scene_Slice_EchoShowcase` / `Map_TestArena` | `tile_test_floor_01.png`, `spr_arena_boundary_01.png` | 없음 | 처음은 Unity Tile/Shape placeholder, 이후 imagegen/타일 제작 |
| 절단쌍검 | `Weapon_DualBlades_Runtime` | `spr_weapon_dual_blade_left.png`, `spr_weapon_dual_blade_right.png` | 콘셉트 시트에 방향 있음 | 별도 투명 PNG 생성 필요 |
| 기본 베기 | `Hitbox_DualBladeArc_L/R` | `spr_dual_blade_swing_arc_01.png`, `spr_dual_blade_swing_arc_02.png` | 콘셉트 시트에 방향 있음 | 별도 투명 PNG 생성 필요 |
| 활성 칼무리 | `Memory_HungryBlades_Ring` | `spr_kalmuri_orbit_blade_01.png` | 콘셉트 시트에 방향 있음 | 별도 투명 PNG 생성 필요 |
| 칼무리 잔향 | `Echo_Kalmuri_DelayedSlash`, `Echo_Kalmuri_LingerSlash` | `spr_kalmuri_echo_slash_01.png` | 콘셉트 시트에 방향 있음 | 첫 투명 VFX 생성 우선순위 1 |
| 칼무리 +5 | `Echo_Kalmuri_WeaponShard`, `Echo_Kalmuri_LaunchBlade` | `spr_kalmuri_echo_shard_01.png`, `spr_kalmuri_launch_blade_01.png` | 콘셉트 시트에 방향 있음 | 별도 투명 PNG 생성 필요 |
| 혈반 표식 | `Echo_Blood_Mark` | `spr_blood_mark_01.png`, `spr_blood_mark_02.png` | 콘셉트 시트에 방향 있음 | 첫 투명 VFX 생성 우선순위 2 |
| 회복 실 | `Echo_Blood_HealThread` | `spr_heal_thread_tip_01.png`, `mat_heal_thread_line.mat` | 콘셉트 시트에 방향 있음 | tip sprite + LineRenderer material |
| 혈반 피꽃 | `Echo_Blood_Bloom` | `spr_blood_bloom_01.png` 또는 particle texture | 콘셉트 시트에 방향 있음 | particle/sprite burst 제작 |
| 피의 칼폭풍 | `Ultimate_BloodBladeStorm` | `spr_blood_blade_storm_blade_01.png`, `spr_blood_blade_storm_ring_01.png`, `mat_blood_thread.mat` | 콘셉트 시트에 방향 있음 | 궁극 전용 투명 PNG/Material 제작 |
| 테스트 적 | `Enemy_TestWalker`, `Enemy_TestShooter` | `spr_enemy_test_walker_placeholder.png`, `spr_enemy_test_shooter_placeholder.png` | 없음 | 첫 slice는 단순 실루엣 placeholder 가능 |
| UI | `UI_EchoHud`, `UI_DebugEchoPanel` | icon sprites optional | 없음 | TextMeshPro + 단색 UI로 시작 |

## 3. Unity 폴더와 파일 배치

Unity 프로젝트가 생기면 MCP는 먼저 `_dev`에 같은 구조를 만든다. 아래 `Assets/Lethe` 구조는 GO 판정 뒤 승격 형태다.

```text
Assets/Lethe/
  Art/
    Concept/
      lethe-first-echo-showcase-concept.png
    Sprites/
      Characters/
        spr_player_echo_showcase_placeholder.png
      Maps/
        tile_test_floor_01.png
        spr_arena_boundary_01.png
      Weapons/
        spr_weapon_dual_blade_left.png
        spr_weapon_dual_blade_right.png
        spr_dual_blade_swing_arc_01.png
        spr_dual_blade_swing_arc_02.png
      Echoes/
        Kalmuri/
          spr_kalmuri_orbit_blade_01.png
          spr_kalmuri_echo_slash_01.png
          spr_kalmuri_echo_shard_01.png
          spr_kalmuri_launch_blade_01.png
        Blood/
          spr_blood_mark_01.png
          spr_blood_mark_02.png
          spr_blood_bloom_01.png
          spr_heal_thread_tip_01.png
      Ultimates/
        spr_blood_blade_storm_blade_01.png
        spr_blood_blade_storm_ring_01.png
    Materials/
      mat_heal_thread_line.mat
      mat_blood_thread.mat
      mat_echo_additive.mat
    Particles/
      ps_blood_bloom.prefab
      ps_kalmuri_spark.prefab
  Prefabs/
    Player/
    Weapons/
    Hitboxes/
    Memories/
    Echoes/
    Ultimates/
    Enemies/
    UI/
  Data/
    Weapons/
    Memories/
    Echoes/
    EchoSynergies/
    FeedbackProfiles/
  Scenes/
    Slice_EchoShowcase.unity
```

## 4. 프리팹별 연결 상세

### 4.1 캐릭터

| 항목 | 값 |
| --- | --- |
| Prefab | `Assets/Lethe/Prefabs/Player/Player_EchoShowcase.prefab` |
| Sprite | `Assets/Lethe/Art/Sprites/Characters/spr_player_echo_showcase_placeholder.png` |
| 필수 컴포넌트 | `Rigidbody2D`, `Collider2D`, `PlayerController`, `Health`, `MemoryInventory`, `EchoInventory`, `WeaponSlot` |
| 자식 오브젝트 | `WeaponAnchor`, `EchoAnchor`, `VfxAnchor`, `HudAnchor` |
| 첫 slice 역할 | 잔향/VFX가 붙는 중심점. 캐릭터 아트 완성도보다 anchor 정확도가 중요 |

처리:

- 캐릭터 이미지는 아직 없다.
- Unity 첫 세팅에서는 원형/실루엣 placeholder로 시작해도 된다.
- 캐릭터 최종 이미지보다 `WeaponAnchor`와 `EchoAnchor` 위치가 우선이다.

### 4.2 맵

| 항목 | 값 |
| --- | --- |
| Scene | `Assets/Lethe/Scenes/Slice_EchoShowcase.unity` |
| Prefab | `Assets/Lethe/Prefabs/Map/Map_TestArena.prefab` |
| Sprite/Tile | `Assets/Lethe/Art/Sprites/Maps/tile_test_floor_01.png` |
| 필수 오브젝트 | `SpawnArea`, `ArenaBoundary`, `EnemySpawnPoints`, `CameraBounds` |
| 첫 slice 역할 | VFX 가독성을 확인하는 어두운 중립 배경 |

처리:

- 맵 이미지는 아직 없다.
- 첫 세팅에서는 Unity 2D Sprite Shape/Tilemap placeholder로 충분하다.
- 배경은 어둡고 단순해야 붉은 혈반/흰 칼선이 읽힌다.

### 4.3 절단쌍검

| 항목 | 값 |
| --- | --- |
| Prefab | `Assets/Lethe/Prefabs/Weapons/Weapon_DualBlades_Runtime.prefab` |
| Definition | `Assets/Lethe/Data/Weapons/Weapon_DualBlades.asset` |
| Sprite | `spr_weapon_dual_blade_left.png`, `spr_weapon_dual_blade_right.png` |
| Swing VFX | `spr_dual_blade_swing_arc_01.png`, `spr_dual_blade_swing_arc_02.png` |
| 필수 컴포넌트 | `DualBladesController`, `WeaponHitEmitter`, `SfxLayerSource` |
| 연결 대상 | `Player_EchoShowcase/WeaponAnchor` |

처리:

- 현재 생성된 콘셉트 시트의 좌상단 쌍검을 방향 참조로 사용한다.
- 런타임 쌍검은 투명 PNG를 별도로 만든다.
- 기본 베기 arc는 칼무리 잔향과 헷갈리지 않게 더 얇고 짧게 만든다.

### 4.4 칼무리 계열

| Prefab | Sprite | Runtime 클래스 | 역할 |
| --- | --- | --- | --- |
| `Memory_HungryBlades_Ring.prefab` | `spr_kalmuri_orbit_blade_01.png` | `HungryBladesMemoryRuntime` | 활성 기억. 독립 칼날 고리 |
| `Echo_Kalmuri_DelayedSlash.prefab` | `spr_kalmuri_echo_slash_01.png` | `KalmuriEchoRuntime` | 잔향 +1~+3. 무기 공격 뒤 칼선 |
| `Echo_Kalmuri_LingerSlash.prefab` | `spr_kalmuri_echo_slash_01.png` | `KalmuriEchoRuntime` | 잔향 +3. 짧게 남는 칼자국 |
| `Echo_Kalmuri_WeaponShard.prefab` | `spr_kalmuri_echo_shard_01.png` | `KalmuriEchoRuntime` | 잔향 +4. 무기 주변 파편 |
| `Echo_Kalmuri_LaunchBlade.prefab` | `spr_kalmuri_launch_blade_01.png` | `PooledProjectile`, `EchoHitEmitter` | 잔향 +5. 타격 반응 발사 칼날 |

구분 규칙:

- 활성 칼무리: 플레이어 주변 독립 고리.
- 잔향 칼무리: 무기 공격에서 튀어나오는 칼선/파편.
- 공명: 활성 고리와 무기 칼선이 동시에 존재.

### 4.5 혈반 계열

| Prefab | Sprite/Material | Runtime 클래스 | 역할 |
| --- | --- | --- | --- |
| `Echo_Blood_Mark.prefab` | `spr_blood_mark_01.png` | `BloodEchoRuntime` | 적 몸에 붙는 혈반 |
| `Echo_Blood_HealThread.prefab` | `spr_heal_thread_tip_01.png`, `mat_heal_thread_line.mat` | `HealThreadVfx` | 적에서 플레이어로 돌아오는 회복 실 |
| `Echo_Blood_Bloom.prefab` | `spr_blood_bloom_01.png` or `ps_blood_bloom.prefab` | `BloodBloomVfx` | 혈반 +5 피꽃 폭발 |

구분 규칙:

- 혈반은 공간 고리가 아니라 적 몸에 남는 표식이다.
- 회복 실은 타격 피드백보다 생존 피드백이다.
- 피꽃은 +5 각성 순간에만 선명해야 한다.

### 4.6 피의 칼폭풍

| 항목 | 값 |
| --- | --- |
| Prefab | `Assets/Lethe/Prefabs/Ultimates/Ultimate_BloodBladeStorm.prefab` |
| Definition | `Assets/Lethe/Data/EchoSynergies/Synergy_BloodBladeStorm.asset` |
| Sprites | `spr_blood_blade_storm_blade_01.png`, `spr_blood_blade_storm_ring_01.png` |
| Materials | `mat_blood_thread.mat`, `mat_echo_additive.mat` |
| 필수 컴포넌트 | `BloodBladeStormRuntime`, `OrbitShardController`, `EchoProcLimiter`, `SfxLayerSource` |
| 연결 대상 | `Player_EchoShowcase/EchoAnchor` |

동작:

- 폭풍 칼날이 적에게 혈반을 묻힌다.
- 혈반이 터지면 회복 실이 플레이어에게 돌아온다.
- 처치가 이어지면 고리 속도가 2초간 증가한다.
- 모든 타격은 `UltimateHit`로 발행하고 일반 온힛 잔향을 다시 부르지 않는다.

## 5. ScriptableObject 연결

| SO | 연결 prefab/asset | 역할 |
| --- | --- | --- |
| `Weapon_DualBlades.asset` | `Weapon_DualBlades_Runtime.prefab`, swing arc sprites | 쌍검 공격 리듬 |
| `Memory_HungryBlades.asset` | `Memory_HungryBlades_Ring.prefab` | 활성 칼무리 |
| `Memory_BloodReflection.asset` | `Memory_BloodReflection_Strike.prefab` | 활성 피의 반사 |
| `Echo_Kalmuri.asset` | 칼무리 echo prefabs | 칼무리 잔향 레벨별 runtime |
| `Echo_Blood.asset` | 혈반 echo prefabs | 혈반 잔향 레벨별 runtime |
| `Synergy_BloodBladeStorm.asset` | `Ultimate_BloodBladeStorm.prefab` | 피의 칼폭풍 궁극 |
| `Feedback_EchoLight.asset` | slash/blood/storm prefabs | hitstop, flash, sound, light |

## 6. Unity MCP 실행 순서

Unity 프로젝트가 열린 뒤 MCP는 아래 순서로 실행한다. 현재는 `Assets/_dev` 경로를 사용하고, GO 판정 후에만 `Assets/Lethe`로 승격한다.

1. 프로젝트 상태 확인: AnkleBreaker MCP `unity_list_instances`, `unity_editor_ping(port=7890)`.
2. 폴더 생성: `Assets/_dev/...` 구조 생성.
3. 콘셉트 이미지 import:
   - Source: `docs/design/assets/lethe-first-echo-showcase-concept.png`
   - Target: `Assets/_dev/Art/Source/lethe-first-echo-showcase-concept.png`
4. placeholder sprite 생성 또는 import:
   - player, map, enemy는 placeholder로 시작 가능.
5. runtime sprite import:
   - 투명 PNG가 준비된 것부터 `Assets/_dev/Art/Sprites/...`에 넣는다.
6. material 생성:
   - `mat_heal_thread_line`, `mat_blood_thread`, `mat_echo_additive`.
7. prefab 생성:
   - `Player_EchoShowcase`, `Weapon_DualBlades_Runtime`, echo prefabs, ultimate prefab.
8. ScriptableObject 생성:
   - Weapon, Memory, Echo, Synergy, FeedbackProfile.
9. prefab과 SO 연결:
   - SO가 runtime prefab을 참조하고, prefab은 sprite/material/profile을 참조한다.
10. scene 생성:
   - `Slice_EchoShowcase.unity`에 player, map, test enemy spawner, debug panel 배치.
11. 검증:
   - debug button으로 칼무리/혈반/+5/피의 칼폭풍 상태를 즉시 켠다.
12. 승격:
   - GO 판정 후 `_dev` 구조를 `Assets/Lethe`로 옮긴다.

## 7. 아직 없는 이미지와 다음 생성 대상

현재 없는 이미지:

- 플레이어 캐릭터.
- 테스트 맵 바닥/경계.
- 테스트 적.
- 투명 배경 쌍검.
- 투명 배경 개별 칼선/혈반/피실/폭풍 파츠.

다음 생성 우선순위:

1. `spr_kalmuri_echo_slash_01.png`
2. `spr_blood_mark_01.png`
3. `spr_kalmuri_launch_blade_01.png`
4. `spr_blood_blade_storm_blade_01.png`
5. `spr_weapon_dual_blade_left/right.png`
6. `spr_player_echo_showcase_placeholder.png`
7. `tile_test_floor_01.png`

판단:

- 이펙트가 이 게임의 첫 인상이므로, 캐릭터/맵보다 칼선/혈반/폭풍 파츠가 먼저다.
- 캐릭터와 맵은 처음에 placeholder여도 된다.
- 단, 배경은 어둡고 단순해야 흰 칼선과 붉은 혈반이 잘 보인다.

## 8. 완료 기준

이 문서 기준으로 Unity MCP 작업이 성공한 상태:

- 콘셉트 이미지가 `Assets/Lethe/Art/Concept/`에 들어가 있다.
- `Player_EchoShowcase`가 scene에 있고 `WeaponAnchor`, `EchoAnchor`를 가진다.
- `Weapon_DualBlades_Runtime`가 player에 연결되어 있다.
- 칼무리/혈반/피의 칼폭풍 prefab이 각각 어떤 sprite/material을 써야 하는지 명확하다.
- 아직 없는 파일은 placeholder인지 다음 imagegen 대상인지 구분된다.
- debug panel에서 특정 prefab/VFX를 바로 켜서 확인할 수 있다.
