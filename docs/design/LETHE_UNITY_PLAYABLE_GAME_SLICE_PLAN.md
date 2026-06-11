# LETHE Unity Playable Game Slice Plan

최종 갱신: 2026-06-11

## 0. 이 문서의 역할

이 문서는 LETHE Unity 작업을 `잔향 VFX 테스트`에서 `실제로 게임처럼 플레이되는 slice`로 전환하기 위한 기준 문서다.

이전 `_dev` 작업의 문제는 기능이 하나도 없어서가 아니라, 작업 순서가 잘못 보였다는 점이다. 잔향을 판단하려면 먼저 화면 크기, 카메라, 캐릭터 크기, 조작감, 적 압박, 무기 판정, 피격/사망 루프가 최소한 게임처럼 작동해야 한다.

## 1. 현재 판단

현재 `Dev_EchoSlice.unity`는 아직 완성형 slice가 아니다.

문제:

- 카메라 시점과 캐릭터/몹 크기 기준이 정리되지 않았다.
- 플레이어와 적이 화면에서 어느 정도 크기로 보여야 하는지 기준이 없다.
- 무기/캐릭터/적/이펙트의 sorting, scale, anchor가 아직 임시다.
- 이동은 붙었지만 조작감과 카메라 follow가 게임용으로 튜닝되지 않았다.
- 적이 다가오지만 생존 압박, 피격, 사망, 리셋 루프가 없다.
- 잔향은 여전히 상태 강제 전환 중심이고, 실제 게임 진행에서 획득/망각/잔향/공명으로 이어지지 않는다.

결론:

```text
이제 목표는 "잔향이 보이는가"가 아니다.
목표는 "LETHE의 1분짜리 전투 루프가 게임처럼 성립하는가"다.
```

## 2. 최종 slice 목표

첫 playable game slice는 아래를 포함해야 한다.

### 2.1 화면/카메라/스케일

- 16:9 Game View 기준으로 플레이어, 적, 무기가 한눈에 읽힌다.
- 플레이어는 화면 높이의 약 8~11% 정도로 보인다.
- 일반 적은 플레이어보다 약간 작거나 비슷하다.
- 무기는 플레이어 손 위치에 붙고, 공격할 때 arc가 명확하다.
- 카메라는 플레이어를 따라가되 너무 흔들리지 않는다.
- 전투 영역은 단색 타일 하나가 아니라 arena 경계와 이동 공간이 보인다.

### 2.2 기본 조작

- WASD/방향키 이동.
- 마우스 또는 가장 가까운 적 기준으로 공격 방향 결정.
- 자동 공격은 가능하지만, 공격 방향이 읽혀야 한다.
- debug key는 유지하되, 실제 플레이 흐름을 깨지 않는 위치에 있어야 한다.

### 2.3 적/전투 루프

- 적 3~8마리가 플레이어를 추적한다.
- 적 접촉 시 플레이어가 피해를 받는다.
- 플레이어 HP가 보인다.
- 적을 처치하면 새 적이 보충된다.
- 공격은 가까운 적을 자동 타겟팅하거나 범위 안의 적을 맞힌다.
- hit flash, hit stop, knockback 또는 recoil이 최소한 들어간다.

### 2.4 기억/망각/잔향 루프

최소 버튼 기반이어도 아래 순서를 밟아야 한다.

1. 기억 보유: 칼무리 또는 혈반 기억이 활성 상태로 보인다.
2. 망각: 활성 기억이 사라지고 잔향으로 바뀐다.
3. 잔향: 무기 공격에 다른 형태로 붙는다.
4. 잔향 강화: +1~+5가 화면 행동 변화로 보인다.
5. 재획득 공명: 잃었던 기억을 다시 얻으면 공명 상태가 보인다.
6. 궁극: 칼무리 +5와 혈반 +5가 피의 칼폭풍으로 연결된다.

### 2.5 데이터 구조

- 무기/기억/잔향/궁극은 hard-coded debug mode가 아니라 data asset과 runtime component로 연결한다.
- `DevEchoSliceDebugController`는 상태를 세팅하는 도구일 뿐, 실제 전투 동작을 전부 들고 있으면 안 된다.

## 3. 진행 순서

### Phase 1. 화면 기준과 기본 게임감

목표:

- “일단 보기 좋고 조작 가능한 화면”을 만든다.

작업:

- 카메라 orthographic size, follow sharpness, UI panel 위치 정리.
- player/enemy/weapon scale 기준 재조정.
- arena floor 크기와 경계 표시.
- weapon anchor와 sprite sorting 정리.
- player/enemy bob/tilt 과하지 않게 조정.

완료 기준:

- Play Mode를 켰을 때 캐릭터/적/무기/카메라 구도가 어색하지 않다.
- “캐릭터가 무기를 들고 있다”가 즉시 읽힌다.

### Phase 2. 전투 기본 루프

목표:

- 적이 다가오고, 플레이어가 때리고, 맞으면 위험한 구조를 만든다.

작업:

- player HP.
- enemy contact damage.
- 피격 flash/recoil.
- enemy death/respawn.
- nearest enemy targeting.
- 3~8 enemy spawn loop.

완료 기준:

- 30초 동안 움직이며 적을 피하고 죽일 수 있다.
- 피격과 처치가 화면에서 읽힌다.

### Phase 3. 잔향 시스템 루프

목표:

- 잔향이 이펙트가 아니라 게임 시스템으로 작동한다.

작업:

- active memory state.
- forget action.
- echo creation.
- echo level +1~+5.
- overcharge/cap.
- reacquire resonance.
- ultimate unlock.
- echo damage through `HitResolver`.

완료 기준:

- `기억 -> 망각 -> 잔향 -> 강화 -> 공명 -> 궁극`을 한 씬에서 순서대로 확인할 수 있다.

### Phase 4. 데이터화

목표:

- 새 무기/기억/잔향이 들어오기 쉬운 구조로 바꾼다.

작업:

- `WeaponDefinition` asset.
- `MemoryDefinition` asset.
- `EchoDefinition` asset.
- `EchoSynergyDefinition` asset.
- prefab references binding.
- debug controller hard-code 축소.

완료 기준:

- 칼무리/혈반 외 새 기억을 넣을 때 C# 분기 추가를 최소화한다.

### Phase 5. 감각 polish

목표:

- 최소 slice를 “보이는 게임”에서 “계속 치고 싶은 게임”으로 올린다.

작업:

- sprite animation 또는 frame swap.
- sound layering.
- hitstop 튜닝.
- camera impulse.
- VFX scale/timing/sorting.
- HUD polish.

완료 기준:

- 1분 플레이 후 잔향/궁극을 다시 보고 싶다.

## 4. 지금 바로 할 일

현재는 Phase 1부터 다시 잡는다.

우선순위:

1. 카메라 orthographic size와 player/enemy/weapon scale 기준 정리.
2. weapon anchor와 sprite sorting 재정렬.
3. arena floor/경계 표시.
4. debug panel 축소 및 화면 방해 제거.
5. Play Mode screenshot/evidence로 구도 확인.

## 5. 보류

아래는 Phase 1 전에는 하지 않는다.

- 신규 기억/무기 추가.
- 복잡한 잔향 밸런스.
- 상점/메타 진행.
- 지역/보스 구조.
- `Assets/Lethe` 승격.

## 6. 리뷰 기준

Phase 1 리뷰 질문:

- 카메라 거리와 캐릭터 크기가 게임처럼 보이는가?
- 플레이어가 무기를 들고 있다는 게 바로 보이는가?
- 적이 다가오는 압박이 화면에서 읽히는가?
- VFX가 너무 크거나 작아서 기본 전투를 가리지 않는가?
- 다음 단계로 HP/접촉 피해/다수 적을 넣어도 되는 화면 기준인가?
