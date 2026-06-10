# LETHE Unity Slice 이미지 제작 / MCP 구현 플랜

최종 갱신: 2026-06-10

## 0. 이 문서의 역할

이 문서는 첫 Unity slice를 만들기 전에 필요한 이미지, Unity 에셋, 프리팹, 테스트 게이트를 Superpowers식으로 세분화한 작업 계획서다.

기존 문서와의 역할 분리:

| 문서 | 역할 |
| --- | --- |
| `LETHE_WEAPON_MEMORY_ECHO_DETAIL.md` | 무기, 기억, 잔향이 레벨별로 무엇을 하는지 정의 |
| `LETHE_UNITY_ECHO_SYSTEM_PRD.md` | Unity 클래스, ScriptableObject, 런타임 서비스, 이벤트 구조 정의 |
| `LETHE_VISUAL_ASSET_PLAN.md` | 첫 잔향 비주얼 방향과 필요한 이미지 파츠 목록 정의 |
| `LETHE_UNITY_ASSET_BINDING_PLAN.md` | 이미지 파일을 Unity 프리팹/SO/씬에 어떻게 연결할지 정의 |
| 이 문서 | 어떤 이미지를 어떤 순서로 만들고, 그걸로 어떤 slice 테스트를 통과할지 정의 |

핵심 목표:

```text
이미지 생성 -> Unity _dev import -> 프리팹 연결 -> Debug slice -> 1인 체감 테스트
```

## 1. 첫 Unity slice 한 문장

```text
어두운 테스트 아레나에서 절단쌍검으로 적을 때리면, 잃은 칼무리와 혈반이 무기에 남아 칼선, 혈반, 회복 실, 피의 칼폭풍으로 변한다.
```

이 slice는 전체 게임이 아니라 아래 감정만 검증한다.

- 잔향이 텍스트가 아니라 화면 행동으로 보이는가.
- 활성 기억과 잔향의 형태가 다르게 읽히는가.
- +5 각성은 단순 수치 상승이 아니라 새 행동처럼 보이는가.
- 칼무리 + 혈반이 합쳐진 피의 칼폭풍이 "궁극"처럼 보이는가.

## 2. 작업 루프

Superpowers식으로 이번 작업은 아래 순서를 따른다.

1. **Brainstorm lock**: 첫 slice의 판타지를 하나로 고정한다.
2. **Asset spec**: 필요한 이미지를 파일 단위로 쪼갠다.
3. **Unity plan**: 각 이미지가 어느 프리팹/씬/테스트에 쓰이는지 연결한다.
4. **Imagegen**: Codex imagegen으로 project-bound PNG를 만든다.
5. **MCP import**: AnkleBreaker Unity MCP로 `Assets/_dev`에 import하고 import setting을 맞춘다.
6. **Prefab assembly**: player, weapon, enemy, echo VFX, debug scene을 만든다.
7. **Slice test**: debug 버튼으로 잔향 레벨과 궁극을 빠르게 켠다.
8. **Review**: jaewoo 1인 테스트로 "뽕맛/가독성/짜증"을 판단한다.

## 3. Unity 작업 루트

사용자가 지정한 작업 루트:

```text
Assets/_dev/
```

첫 slice에서는 `Assets/Lethe/`가 아니라 `_dev` 아래에서 실험한다. 검증 후 살아남은 구조만 `Assets/Lethe/`로 승격한다.

권장 구조:

```text
Assets/_dev/
  Art/
    Source/
    Sprites/
      Characters/
      Enemies/
      Map/
      Weapons/
      Echoes/Kalmuri/
      Echoes/Blood/
      Ultimates/
    Materials/
    Particles/
  Prefabs/
    Player/
    Enemies/
    Map/
    Weapons/
    Echoes/
    Ultimates/
    Debug/
  Scripts/
    Slice/
    Runtime/
    Feedback/
    Debug/
  Data/
    Weapons/
    Memories/
    Echoes/
    Synergies/
    Feedback/
  Scenes/
    Dev_EchoSlice.unity
```

## 4. 이미지 제작 단위

### Batch A: 화면 판독용 기본 세트

목표: 칼선/혈반이 잘 보이는 화면 환경을 먼저 만든다.

| 파일 | 용도 | 프리팹/씬 | 우선순위 |
| --- | --- | --- | --- |
| `spr_player_echo_silhouette_01.png` | 플레이어 기준점 | `Player_EchoShowcase` | 필수 |
| `spr_enemy_walker_01.png` | 맞는 대상 | `Enemy_TestWalker` | 필수 |
| `spr_enemy_shooter_01.png` | 멀리 남는 대상 | `Enemy_TestShooter` | 보조 |
| `tile_dev_floor_dark_01.png` | VFX 가독성 배경 | `Dev_TestArena` | 필수 |
| `spr_dev_arena_boundary_01.png` | 아레나 경계 | `Dev_TestArena` | 보조 |

이미지 방향:

- 플레이어는 완성 캐릭터가 아니라 검은/남색 실루엣 + 작은 흰 머리/가슴 포인트.
- 적은 회색/탁한 보라 실루엣. 붉은 혈반이 붙었을 때 보여야 한다.
- 맵은 어둡고 단순해야 한다. 붉은/흰 VFX보다 튀면 실패.

테스트:

- 혈반 표식이 적 몸에 붙었을 때 1초 안에 보인다.
- 칼선이 바닥과 적에게 묻히지 않는다.
- 플레이어 anchor 방향이 눈으로 읽힌다.

### Batch B: 절단쌍검 / 기본 타격

목표: 기본 공격과 잔향 공격이 서로 구분되게 한다.

| 파일 | 용도 | 프리팹 | 우선순위 |
| --- | --- | --- | --- |
| `spr_weapon_dual_blade_left_01.png` | 왼손 검 | `Weapon_DualBlades_Runtime` | 필수 |
| `spr_weapon_dual_blade_right_01.png` | 오른손 검 | `Weapon_DualBlades_Runtime` | 필수 |
| `spr_dual_blade_swing_arc_01.png` | 기본 베기 | `Hitbox_DualBladeArc_L` | 필수 |
| `spr_dual_blade_swing_arc_02.png` | 두 번째 베기 | `Hitbox_DualBladeArc_R` | 보조 |

이미지 방향:

- 쌍검은 작고 명확한 실루엣. 장식보다 방향성이 중요하다.
- 기본 베기 arc는 얇고 짧다.
- 칼무리 잔향 arc는 기본 베기보다 더 늦게, 더 길게, 더 밝게 보여야 한다.

테스트:

- 기본 타격만 켰을 때 잔향처럼 보이면 실패.
- 잔향을 켰을 때 "무기에 남은 버릇"처럼 추가 칼선이 보이면 성공.

### Batch C: 칼무리 잔향

목표: 잃은 칼무리가 무기 쪽으로 형태를 바꿨다는 것을 보여준다.

| 파일 | 용도 | 프리팹 | 레벨 |
| --- | --- | --- | --- |
| `spr_kalmuri_orbit_blade_01.png` | 활성 칼무리 고리 | `Memory_HungryBlades_Ring` | 활성 |
| `spr_kalmuri_echo_slash_01.png` | 지연 반달 칼선 | `Echo_Kalmuri_DelayedSlash` | +1~+3 |
| `spr_kalmuri_echo_linger_01.png` | 남는 궤도 칼자국 | `Echo_Kalmuri_LingerSlash` | +3 |
| `spr_kalmuri_echo_shard_01.png` | 무기 주변 파편 | `Echo_Kalmuri_WeaponShard` | +4 |
| `spr_kalmuri_launch_blade_01.png` | 발사 칼날 | `Echo_Kalmuri_LaunchBlade` | +5 |

이미지 방향:

- 활성: 플레이어 주변 독립 고리. 무기와 떨어져 보여야 한다.
- 잔향: 무기 공격에서 튀어나오는 칼자국. 고리가 아니라 공격의 흔적.
- +5: 타격 순간 고리에서 칼날 하나가 발사된다.

테스트:

- 활성 칼무리만 켜면 "주변을 도는 칼"로 보인다.
- 망각 후에는 고리가 사라지고, 무기 타격 뒤 칼선이 남는다.
- +5에서는 다중 타격/온힛 발사가 텍스트 없이 보인다.

### Batch D: 혈반 잔향

목표: 혈반은 "회복 숫자"가 아니라 적 몸에 남고 플레이어에게 돌아오는 생존 피드백이어야 한다.

| 파일 | 용도 | 프리팹 | 레벨 |
| --- | --- | --- | --- |
| `spr_blood_mark_01.png` | 기본 혈반 | `Echo_Blood_Mark` | +1~+2 |
| `spr_blood_mark_02.png` | 강화 혈반 | `Echo_Blood_Mark` | +3~+4 |
| `spr_blood_bloom_01.png` | 피꽃 폭발 | `Echo_Blood_Bloom` | +5 |
| `spr_heal_thread_tip_01.png` | 회복 실 끝점 | `Echo_Blood_HealThread` | +1~+5 |
| `tex_heal_thread_line_01.png` | 회복 실 텍스처 | `mat_heal_thread_line` | +1~+5 |

이미지 방향:

- 혈반은 적 몸 위에 붙는 작은 붉은 문양이다.
- 피꽃은 적 몸에서 짧게 터지고 사라진다.
- 회복 실은 적에서 플레이어로 돌아오는 얇은 붉은 선이다.

테스트:

- 적에게 표식이 붙었는지 0.5초 안에 보인다.
- 회복 실은 "내가 회복됐다"를 텍스트 없이 알려준다.
- +5 피꽃은 혈반 전파의 시작점으로 보인다.

### Batch E: 피의 칼폭풍

목표: 칼무리와 혈반이 합쳐진 궁극 잔향을 만든다.

| 파일 | 용도 | 프리팹 | 우선순위 |
| --- | --- | --- | --- |
| `spr_blood_blade_storm_ring_01.png` | 궁극 바닥/고리 | `Ultimate_BloodBladeStorm` | 필수 |
| `spr_blood_blade_storm_blade_01.png` | 회전 칼날 | `Ultimate_BloodBladeStorm` | 필수 |
| `spr_blood_blade_storm_thread_01.png` | 폭풍 붉은 실 | `Ultimate_BloodBladeStorm` | 보조 |
| `spr_blood_blade_storm_burst_01.png` | 발동 순간 폭발 | `Ultimate_BloodBladeStorm` | 보조 |

이미지 방향:

- 흰 칼날 고리 + 붉은 혈실이 한 화면에서 같이 돌아야 한다.
- 궁극은 평소 잔향보다 밝지만, 화면 전체를 가려서는 안 된다.
- 칼날은 혈반을 묻히고, 혈반은 회복 실을 돌려보내는 루프로 보여야 한다.

테스트:

- `칼무리 +5`와 `혈반 +5`를 켰을 때 궁극이 열린 이유가 화면에서 읽힌다.
- 5초 동안 봤을 때 "칼날이 혈반을 만들고 피실이 돌아온다"가 느껴진다.

## 5. Imagegen 제작 규칙

첫 제작은 built-in imagegen을 사용한다.

투명 PNG는 아래 과정을 기본으로 한다.

1. imagegen으로 평평한 chroma-key 배경 이미지를 만든다.
2. 로컬 helper로 chroma-key를 alpha로 제거한다.
3. 결과 PNG를 `Assets/_dev/Art/Sprites/...`에 저장한다.
4. Unity MCP로 import setting을 Sprite 기준으로 맞춘다.

공통 프롬프트 규칙:

```text
2D top-down roguelite sprite/VFX asset, dark fantasy, readable at small scale,
high contrast silhouette, no text, no watermark, no UI frame,
flat solid #00ff00 chroma-key background,
no shadows on the background, no gradient, generous padding.
```

공통 금지:

- 한 이미지 안에 여러 파츠를 섞지 않는다. 런타임 파츠는 파일 1개 = 역할 1개.
- 과한 디테일로 축소 시 뭉치지 않게 한다.
- 배경/그림자/텍스트/프레임을 넣지 않는다.
- 플레이어/적/맵은 잔향 VFX보다 눈에 띄면 안 된다.

권장 해상도:

| 자산 | 원본 크기 | Unity PPU 기준 |
| --- | --- | --- |
| 캐릭터/적 | 512x512 | 100 |
| 무기 | 512x512 | 100 |
| 칼선/VFX | 1024x1024 | 100 |
| 타일 | 512x512 | 100 |
| 궁극 링 | 1024x1024 | 100 |

## 6. MCP 구현 순서

AnkleBreaker Unity MCP 확인 기준:

- `unity_list_instances`에서 `LETHE`가 보인다.
- `unity_editor_ping(port=7890)`이 `connected: true`다.
- `unity_asset_list(folder="Assets/_dev")`가 성공한다.

구현 순서:

1. `_dev` 하위 폴더 생성.
2. 생성된 PNG를 `Assets/_dev/Art/Sprites/...`로 복사.
3. Unity MCP로 asset list를 확인해 Unity가 파일을 인식했는지 검증.
4. Sprite import setting 적용.
5. `Dev_EchoSlice.unity` 생성.
6. `Player_EchoShowcase`, `Enemy_TestWalker`, `Dev_TestArena` placeholder 프리팹 생성.
7. `Weapon_DualBlades_Runtime` 프리팹 생성.
8. 칼무리/혈반/궁극 VFX 프리팹 생성.
9. `UI_DebugEchoPanel` 생성.
10. debug 버튼으로 아래 상태를 즉시 전환:
    - 기본 쌍검만
    - 칼무리 잔향 +1
    - 칼무리 잔향 +5
    - 혈반 잔향 +5
    - 피의 칼폭풍

## 7. Slice 테스트 시나리오

### Test 1: 기본 화면 판독

상태:

- 어두운 맵.
- 플레이어 1명.
- 적 5명.
- 쌍검 기본 공격만 켬.

성공:

- 플레이어, 적, 공격 방향이 헷갈리지 않는다.
- 잔향이 없어도 기본 타격 리듬이 보인다.

### Test 2: 칼무리 망각 형태 변환

상태:

- 활성 칼무리 고리 켬.
- debug 버튼으로 망각 실행.

성공:

- 고리가 무기 쪽으로 빨려 들어가는 것처럼 보인다.
- 이후 기본 공격 뒤에 칼무리 칼선이 남는다.

### Test 3: 혈반 생존 피드백

상태:

- 혈반 잔향 +3 또는 +5.
- 적 10명.

성공:

- 표식이 붙은 적과 아닌 적이 구분된다.
- 회복 실이 플레이어에게 돌아오는 경로가 보인다.

### Test 4: +5 각성 차이

상태:

- 칼무리 +1 -> +3 -> +5 순서로 버튼 전환.

성공:

- +1은 작은 지연 칼선.
- +3은 남는 궤도.
- +5는 발사 칼날/고리로 동작이 바뀐다.

### Test 5: 피의 칼폭풍

상태:

- 칼무리 +5.
- 혈반 +5.
- 궁극 켬.

성공:

- 칼날, 혈반, 회복 실이 하나의 루프로 보인다.
- 화면이 지저분해지지 않는다.
- 10초 뒤에도 어떤 효과가 무엇인지 구분된다.

## 8. 작업 세분화

### Phase 0: 기준 고정

- 이 문서를 source of truth로 등록.
- `_dev` 루트 사용 확정.
- 기존 `Assets/Lethe` 승격은 보류.

완료 기준:

- `CURRENT_TASK`가 Unity slice asset production으로 갱신된다.

### Phase 1: 폴더/MCP 준비

- MCP로 `Assets/_dev` 인식 확인.
- `_dev/Art`, `_dev/Prefabs`, `_dev/Scripts`, `_dev/Data`, `_dev/Scenes` 생성.

완료 기준:

- Unity asset list가 `_dev` 하위 폴더를 읽는다.

### Phase 2: 기본 이미지 5개 생성

먼저 만들 이미지:

1. `spr_player_echo_silhouette_01.png`
2. `spr_enemy_walker_01.png`
3. `tile_dev_floor_dark_01.png`
4. `spr_weapon_dual_blade_left_01.png`
5. `spr_weapon_dual_blade_right_01.png`

완료 기준:

- Unity scene에 player/enemy/map/weapon이 보인다.
- 아직 잔향 없이도 기본 화면 판독이 된다.

### Phase 3: 잔향 핵심 이미지 5개 생성

먼저 만들 이미지:

1. `spr_kalmuri_echo_slash_01.png`
2. `spr_kalmuri_orbit_blade_01.png`
3. `spr_kalmuri_launch_blade_01.png`
4. `spr_blood_mark_01.png`
5. `spr_heal_thread_tip_01.png`

완료 기준:

- debug scene에서 칼무리 +1/+5와 혈반 표식/회복 실을 볼 수 있다.

### Phase 4: 궁극 이미지 3개 생성

먼저 만들 이미지:

1. `spr_blood_blade_storm_ring_01.png`
2. `spr_blood_blade_storm_blade_01.png`
3. `spr_blood_bloom_01.png`

완료 기준:

- 피의 칼폭풍 debug 버튼을 눌렀을 때 궁극처럼 보인다.

### Phase 5: MCP 프리팹/씬 조립

- `Dev_EchoSlice.unity` 생성.
- player, enemy, map, weapon, echo, ultimate prefab 연결.
- debug panel 버튼 연결.

완료 기준:

- Unity Editor에서 scene을 열고 10초 안에 모든 slice 상태를 켤 수 있다.

### Phase 6: jaewoo 1인 리뷰

질문:

- 칼무리 고리를 잃고 무기 칼선으로 바뀐 게 보였나?
- 혈반/회복 실이 생존 피드백으로 보였나?
- +5는 "숫자 상승"이 아니라 "새 행동"처럼 보였나?
- 피의 칼폭풍은 기대감이 있었나?
- 너무 지저분하거나 피곤한 효과는 무엇이었나?

GO:

- 잔향이 텍스트 없이 전투 행동으로 보인다.
- 피의 칼폭풍을 다시 보고 싶다.
- 이미지/프리팹 구조를 `Assets/Lethe`로 승격할 가치가 있다.

ITERATE:

- 효과는 보이지만 가독성/타이밍/색이 약하다.

NO-GO:

- 잔향이 여전히 기본 공격 위에 붙은 라벨처럼 느껴진다.

## 9. 지금 만들지 않을 것

- 신규 무기/기억/적 확장.
- 장송대검 라인.
- 상점, 메타 진행, 다중 지역.
- 최종 캐릭터 일러스트.
- 완성형 타일셋.
- 자동화 대시보드.

이번 slice의 적/맵/캐릭터는 "잔향을 검증하기 위한 무대"다. 완성도 우선순위는 `잔향 VFX > 무기 > 적 > 캐릭터 > 맵`이다.
