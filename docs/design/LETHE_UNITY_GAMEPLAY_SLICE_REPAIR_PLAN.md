# LETHE Unity Gameplay Slice Repair Plan

최종 갱신: 2026-06-11

## 0. 이 문서의 역할

`Dev_EchoSlice.unity`를 단순 VFX 확인 장치에서 최소 게임 플레이 slice로 끌어올리는 수리 계획이다.

이 문서는 기존 잔향 기획을 버리는 문서가 아니다. 지금 부족한 것은 잔향 아이디어가 아니라, 그 잔향을 판단할 수 있는 게임 형태다.

## 1. 현재 문제

이전 `Dev_EchoSlice`는 아래 기준에서 부족했다.

- 플레이어가 직접 움직이는 게임 느낌이 약했다.
- 적이 플레이어를 추적하지 않아 전투 압박이 없었다.
- 무기가 플레이어 손에 붙어 있지 않고 씬 옆에 떠 있는 것처럼 보였다.
- 캐릭터/몹이 가만히 서 있어 생동감이 없었다.
- 공격은 발생하지만 무기를 휘두르는 시각 피드백이 약했다.
- 잔향 VFX는 보였지만, 기본 게임 루프가 약해서 잔향을 평가할 맥락이 없었다.

## 2. 수리 목표

아침 리뷰 기준을 아래처럼 바꾼다.

```text
VFX가 보이는가? -> 캐릭터를 움직이고, 적이 다가오고, 무기가 휘둘리고, 잔향이 공격 위에서 작동하는가?
```

최소 통과 조건:

- WASD/방향키로 플레이어가 움직인다.
- 카메라가 플레이어를 따라간다.
- 적이 플레이어를 추적한다.
- 무기가 플레이어의 손 위치에 붙어 있다.
- 공격 시 무기가 짧게 스윙한다.
- 플레이어와 적이 idle/move bob 또는 tilt로 살아 움직인다.
- 기존 `1~5` 잔향 상태 전환은 유지된다.
- Play Mode compile/console/missing reference 오류가 0이다.

## 3. 1차 수리 패스

구현 완료:

- `DevPlayerController2D`
  - WASD/방향키 이동.
  - 이동 방향에 따른 facing.
  - `WeaponAnchor` 위치/방향 갱신.
- `DevEnemyChaseController`
  - 플레이어 추적.
  - 적 facing 갱신.
  - 사망 후 spawn 위치 복귀 지원.
- `DevCameraFollow2D`
  - 플레이어 추적 카메라.
- `DevSpriteMotionAnimator`
  - `Visual` 자식에 idle/move bob과 tilt 적용.
- `DualBladesController`
  - 공격 시 무기 transform swing.
- Scene wiring
  - `Weapon_DualBlades_Runtime`를 `Player_EchoShowcase/WeaponAnchor` 자식으로 이동.
  - 플레이어/적 SpriteRenderer를 `Visual` 자식으로 분리.
  - debug panel에 WASD/추적/무기 장착 안내 추가.

## 4. 아직 부족한 것

다음부터는 아래 순서로 진행한다.

### 4.1 플레이어 생존 루프

- 적 접촉 피해.
- 플레이어 HP.
- 피격 flash.
- 사망/리셋 또는 debug revive.

### 4.2 적 다수 루프

- test enemy 1마리에서 3~8마리 spawn으로 확장.
- 일정 반경 밖에서 재스폰.
- 처치 시 새 적 보충.

### 4.3 공격 판정 개선

- 지금은 debug target 중심이다.
- 다음은 범위 내 가장 가까운 적을 자동 타겟팅해야 한다.
- 무기 swing arc와 실제 hit area가 같은 위치로 읽혀야 한다.

### 4.4 잔향 실제 전투화

- Kalmuri/Blood/Storm VFX가 단순 표시가 아니라 실제 echo hit를 만든다.
- `HitSourceType.EchoHit`은 recursive echo를 막는다.
- damage/log/kill event를 `HitResolver` 경로로 보낸다.

### 4.5 데이터 연결

- `WeaponDefinition`
- `MemoryDefinition`
- `EchoDefinition`
- `EchoSynergyDefinition`

Debug controller가 직접 동작을 들고 있는 구조를 줄이고, data asset과 runtime component가 동작을 갖게 한다.

## 5. 다음 Codex 작업 후보

우선순위:

1. 플레이어 HP + 적 접촉 피해 + 피격 flash.
2. 가장 가까운 적 자동 타겟팅.
3. 적 5마리 spawn loop.
4. Kalmuri/Blood VFX에 실제 echo damage 연결.
5. data asset 생성과 reference binding.

## 6. 리뷰 질문

다음 리뷰에서는 아래를 본다.

- “이제 게임처럼 움직이나?”
- “무기를 들고 때리는 느낌이 있나?”
- “적이 다가와서 전투 상황이 생기나?”
- “잔향은 그 전투 위에 올라간 효과로 보이나?”
- “다음에 볼 것은 생존 루프인가, 적 다수 루프인가, 잔향 실제 damage인가?”
