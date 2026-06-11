# LETHE Unity Art Direction Replacement Plan

최종 갱신: 2026-06-11

## 0. 문서 역할

이 문서는 `Dev_Prototype_v0`의 placeholder sprite를 LETHE 컨셉에 맞는 평가용 아트로 전면 교체하기 위한 기준이다.

폰트 작업은 이번 범위에서 제외한다. 한글 UI는 추후 출시 가능 폰트와 TextMeshPro 전환 단계에서 다룬다.

## 1. 핵심 방향

LETHE의 화면 키워드:

- 망각.
- 검은 강.
- 기억 파편.
- 청백색 칼무리.
- 혈색 잔향.
- 어두운 폐허 위의 강한 이펙트.

기본 원칙:

- 캐릭터와 무기는 분리한다.
- 캐릭터는 무기 없는 body animation만 가진다.
- 무기는 `WeaponAnchor` 자식 sprite로 표현한다.
- 공격의 강함은 무기 크기보다 slash, orbit, bloom, thread, storm VFX로 표현한다.
- Unity 최종 사용 이미지는 alpha PNG여야 한다.
- imagegen source는 `#00ff00` chroma-key로 만들 수 있지만 Unity runtime에는 투명 PNG만 연결한다.

## 2. 이번 교체 대상

### Character

파일:

```text
LETHE/Assets/_dev/Art/Source/sheet_player_4dir_chroma.png
LETHE/Assets/_dev/Art/Sprites/Characters/Player/sheet_player_4dir.png
```

요구:

- 무기 없음.
- 검은 망토, 흰 가면, 청백색 기억광.
- 8 rows x 4 columns.
- row order:
  - idle_down
  - idle_up
  - idle_left
  - idle_right
  - walk_down
  - walk_up
  - walk_left
  - walk_right

### Enemy

파일:

```text
LETHE/Assets/_dev/Art/Source/sheet_enemy_chaser_4dir_chroma.png
LETHE/Assets/_dev/Art/Sprites/Enemies/Chaser/sheet_enemy_chaser_4dir.png
```

요구:

- 망각에 침식된 인간형 추격자.
- 플레이어보다 낮고 넓은 실루엣.
- 혈색 균열, 녹슨 살, 기억 파편.
- 8 rows x 4 columns.

### Map

파일:

```text
LETHE/Assets/_dev/Art/Sprites/Map/tile_dev_floor_dark_01.png
```

요구:

- 검은 강/젖은 석판/희미한 기억 균열.
- 반복 타일로 쓸 수 있어야 한다.
- 캐릭터와 VFX보다 낮은 명도.

### Weapon

파일:

```text
LETHE/Assets/_dev/Art/Source/spr_weapon_dual_blade_left_01_chroma.png
LETHE/Assets/_dev/Art/Source/spr_weapon_dual_blade_right_01_chroma.png
LETHE/Assets/_dev/Art/Sprites/Weapons/spr_weapon_dual_blade_left_01.png
LETHE/Assets/_dev/Art/Sprites/Weapons/spr_weapon_dual_blade_right_01.png
```

요구:

- 짧은 쌍검.
- 캐릭터 실루엣을 먹지 않는 크기.
- 청백색 날, 검은 손잡이.
- 공격 판독은 VFX가 담당하므로 weapon sprite는 장비로만 읽히면 된다.

### Memory / Echo VFX

이번 최소 VFX 4종:

```text
Memory_HungryBlades_Active
Echo_Kalmuri_Slash
Memory_BloodReflection_Active
Echo_Blood_Bloom
```

추가 ultimate VFX:

```text
Synergy_BloodBladeStorm_Ring
```

요구:

- `Memory_HungryBlades_Active`: 플레이어 주위를 도는 청백색 칼날 고리.
- `Echo_Kalmuri_Slash`: 무기 공격에서 튀어나오는 반달 칼자국.
- `Memory_BloodReflection_Active`: 피와 거울 파편이 섞인 반사 문양.
- `Echo_Blood_Bloom`: 적 몸에서 터지는 피꽃과 회복 실.
- `Synergy_BloodBladeStorm_Ring`: 칼날 원과 붉은 실이 섞인 궁극 고리.

## 3. Unity 연결 기준

- `PrototypeGameManager`는 기존 line renderer를 유지하되, 핵심 상태마다 sprite VFX를 추가로 spawn한다.
- sprite VFX는 짧은 lifetime, fade, scale animation을 가진다.
- VFX가 damage 없이 장식만 되면 안 된다. 기존 damage 경로는 유지한다.
- `Dev_Prototype_v0.unity`는 계속 `_dev` 검증 씬이다.
- `Assets/Lethe` 승격은 jaewoo GO 이후에만 한다.

## 4. 평가 기준

jaewoo review에서 확인할 질문:

- 플레이어가 LETHE 주인공처럼 보이는가.
- 적이 임시 좀비가 아니라 망각에 침식된 적으로 보이는가.
- 맵이 어두운 배경 역할을 하면서 VFX를 살리는가.
- 기본 공격, 활성 기억, 잔향, 궁극이 화면에서 서로 다른 형태로 읽히는가.
- VFX가 충분히 화려하지만 플레이어/적 판독을 삼키지 않는가.
