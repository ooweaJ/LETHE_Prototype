# LETHE Release-Grade Art / Font / VFX Plan

최종 갱신: 2026-06-11

## 0. 문서 역할

이 문서는 `Dev_Prototype_v0`를 단순 프로토타입 이미지에서 출시 후보 감각으로 올리기 위한 리소스 기준이다.

핵심 결론:

```text
초반 재미 = 활성 기억 VFX를 보며 성장 체감
중반 변화 = 망각 후 잔향 VFX가 무기 행동으로 변형
후반 뽕맛 = 잔향 +5 / 잔향 조합 / 피의 칼폭풍
```

## 1. Font Policy

### 기본 한글 폰트

- Font: Pretendard
- Source: official GitHub release
- License: SIL Open Font License 1.1
- Unity path:
  - `Assets/_dev/Fonts/Pretendard-Regular.otf`
  - `Assets/_dev/Fonts/Pretendard-SemiBold.otf`
  - `Assets/_dev/Fonts/Pretendard-Bold.otf`
  - `Assets/_dev/Fonts/Pretendard-LICENSE.txt`

### 사용 원칙

- 폰트는 게임에 번들 가능하지만, 라이선스 파일을 같이 보관한다.
- 폰트 단독 판매는 하지 않는다.
- `_dev`에서는 `OnGUI`에 연결해 한글 판독을 먼저 확인한다.
- 정식 UI 전환 시 TextMeshPro 또는 UGUI font asset으로 승격한다.

## 2. Visual Layer Contract

### Character

역할:

- 몸, 방향, 이동 상태를 읽게 한다.
- 무기와 공격력 표현을 책임지지 않는다.

규칙:

- 캐릭터 sheet에는 큰 무기를 굽지 않는다.
- 손 주변의 작은 장비 실루엣은 `WeaponAnchor` 자식이 담당한다.

### Weapon

역할:

- 현재 무기 종류를 작게 알려준다.
- 공격 판독은 slash arc / hit VFX가 담당한다.

현재 기준:

- `WeaponAnchor/BladeVisuals`
- dual blade sprite PPU: `800`
- blade local scale: `0.25`

### Active Memory VFX

역할:

- 망각 전 초반/중반 성장 재미.
- 기억이 몸에 붙어 새로운 행동을 준다는 느낌.

예:

- 굶주린 칼무리 active:
  - 플레이어 주변 희미한 칼날 orbit.
  - 공격 시 적 위치에 푸른 칼날 흔적.
  - 레벨이 오를수록 orbit 밀도/밝기 증가.
- 피의 반사 active:
  - 적 타격 시 붉은 mark.
  - 플레이어 쪽으로 짧은 회복 실.
  - 레벨이 오를수록 mark 선명도/회복 실 수 증가.

### Echo VFX

역할:

- 망각 후 형태가 바뀐 효과.
- "약해진 같은 스킬"이 아니라 "무기에 남은 습관"이어야 한다.

예:

- 칼무리 잔향:
  - 무기 공격에서 칼자국이 튀어나옴.
  - +5에서 orbit/발사/광역으로 각성.
- 혈반 잔향:
  - 무기 공격이 혈반 mark를 남김.
  - +5에서 피꽃 폭발과 회복 실 다발.

### Ultimate VFX

역할:

- 잔향 +5 조합의 후반 목표.
- 두 잔향이 서로 먹여살리는 루프를 보여준다.

예:

- 피의 칼폭풍:
  - 칼날 고리 + 붉은 회복 실.
  - 광역 피해 + 회복.
  - UI 목표는 `칼무리 잔향 +5 / 혈반 잔향 +5`.

## 3. Current Implementation Mapping

| Layer | Current Asset | Current Script Path |
| --- | --- | --- |
| Korean HUD font | `Assets/_dev/Fonts/Pretendard-Regular.otf` | `PrototypeGameManager.koreanFont` |
| Active Kalmuri | `spr_kalmuri_orbit_blade_01.png` | `hungryBladesActiveSprite` |
| Echo Kalmuri | `spr_kalmuri_echo_slash_01.png` | `kalmuriSlashSprite` |
| Active Blood | `spr_blood_mark_01.png` | `bloodReflectionSprite` |
| Echo Blood | `spr_blood_bloom_01.png` | `bloodBloomSprite` |
| Ultimate Storm | `spr_blood_blade_storm_ring_01.png` | `bloodBladeStormSprite` |

## 4. Next Art Production

우선순위:

1. Player 정식 4방향 idle/walk sheet.
2. Enemy_MeleeChaser 정식 4방향 idle/walk sheet.
3. Active memory VFX sheet:
   - Hungry Blades orbit / hit trace.
   - Blood Reflection mark / heal thread.
4. Echo VFX sheet:
   - Kalmuri slash / awakened orbit.
   - Blood bloom / awakened heal threads.
5. Blood Blade Storm ultimate sheet.
6. HUD icon set:
   - 기억 slot.
   - 잔향 slot.
   - +5 각성 badge.
   - 피의 칼폭풍 progress badge.

## 5. Review Gate

jaewoo 리뷰 질문:

- 한글 HUD가 깨지지 않고 읽히는가?
- 초반에 활성 기억을 얻었을 때 화면이 달라졌다고 느끼는가?
- 망각 후 잔향이 active memory와 다른 형태로 보이는가?
- +5 잔향과 피의 칼폭풍은 후반 목표처럼 보이는가?
- 캐릭터/무기/공격/잔향이 서로 뭉개지지 않는가?
