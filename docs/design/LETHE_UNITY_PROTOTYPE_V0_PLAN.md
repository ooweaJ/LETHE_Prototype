# LETHE Unity Prototype v0 Plan

최종 갱신: 2026-06-11

## 0. 결론

`Dev_EchoSlice` 방식은 폐기한다.

이유:

- 잔향/VFX 확인 장치로 시작해서 실제 게임 프로토타입보다 낮은 수준이 됐다.
- HTML 프로토타입보다 게임 루프, 압박, 선택, 망각, 잔향 체감이 약하다.
- 작은 slice를 계속 보강하면 구조가 더 지저분해진다.

이제 목표는 `Unity slice`가 아니라 **Unity Prototype v0.1**이다.

```text
목표:
HTML보다 명확하게 플레이되는 Unity 2D 프로토타입.

핵심:
1분 안에 이동, 공격, 적 압박, 성장 선택, 망각, 잔향, 공명, 궁극 목표를 모두 체험한다.
```

## 1. 기존 slice 처리

`Assets/_dev/Scenes/Dev_EchoSlice.unity`는 더 이상 메인 작업 대상이 아니다.

처리 방침:

- 즉시 삭제하지 않는다.
- 새 prototype scene이 검증될 때까지 참고자료로만 둔다.
- 새 작업은 `Assets/_dev/Scenes/Dev_Prototype_v0.unity`에서 진행한다.
- `Dev_Prototype_v0`이 플레이 가능해지면 `Dev_EchoSlice`는 archive/delete 후보로 분리한다.

이유:

- 지금 삭제하면 이미 만든 sprite/prefab/reference 확인 근거도 같이 잃는다.
- 하지만 더 이상 `Dev_EchoSlice`를 고쳐서 완성하려고 하지 않는다.

## 2. Prototype v0.1의 완료 기준

아래가 모두 되어야 `Prototype v0.1 playable`이라고 부른다.

### 2.1 기본 게임

- 플레이어 이동: WASD/방향키.
- 카메라: 플레이어 중심 추적, 화면 크기 고정.
- 맵: arena 경계와 이동 공간이 명확함.
- 적: 최소 5마리 이상이 추적.
- 적 보충: 처치되면 새 적이 생성됨.
- 플레이어 HP: 피격, 사망, 리셋이 있음.
- 공격: 가까운 적 또는 방향 기준으로 실제 hit 판정.
- 피드백: hit flash, hitstop, knockback 또는 recoil.

### 2.2 성장 선택

- 일정 처치 수 또는 시간마다 기억 선택 UI가 뜬다.
- 선택지는 최소 2개:
  - 굶주린 칼무리
  - 혈반
- 선택하면 active memory가 생기거나 강화된다.
- HUD에 현재 무기, 활성 기억, 잔향, HP가 보인다.

### 2.3 망각

- 일정 조건에서 망각 이벤트가 발생한다.
- 최고 레벨 active memory를 잃는다.
- 잃은 기억은 echo로 전환된다.
- 망각 순간은 화면 행동으로 보여야 한다.
  - 예: 칼무리 고리가 무기로 빨려 들어가고, 이후 공격에 칼선이 남음.

### 2.4 잔향

- 잔향은 약해진 기억이 아니라 형태가 바뀐 효과다.
- Kalmuri Echo:
  - 공격에 지연 칼선/고리/발사 칼날이 붙음.
- Blood Echo:
  - 적 표식/피꽃/회복 실이 붙음.
- 잔향 레벨은 +1~+5.
- +5는 각성 상태로 화면 행동이 달라짐.

### 2.5 공명과 궁극

- 잃었던 기억을 다시 얻으면 공명 표시가 뜬다.
- 공명은 시작 레벨 보너스를 준다.
- Kalmuri Echo +5, Blood Echo +5가 모두 있으면 `피의 칼폭풍` 조건이 열린다.
- 궁극은 debug key가 아니라 조건 만족 후 발동 가능해야 한다.

## 3. 구현 원칙

### 3.1 Debug-first, but game-real

debug key는 허용한다.

하지만 debug key는 상태를 빠르게 만들기 위한 보조 도구일 뿐이다. 핵심 루프는 실제 게임 흐름으로도 작동해야 한다.

허용:

- `F1`: 다음 기억 선택 강제.
- `F2`: 망각 이벤트 강제.
- `F3`: 잔향 +5 세팅.
- `F4`: 공명 강제.

금지:

- `1~5`만 눌러 이펙트 상태를 보여주는 구조.
- 실제 combat event 없이 VFX만 생성하는 구조.

### 3.2 데이터 구조 우선

아래는 v0.1부터 실제 asset/data로 둔다.

- `WeaponDefinition`
- `MemoryDefinition`
- `EchoDefinition`
- `EchoSynergyDefinition`

임시 hard-code는 controller가 아니라 bootstrap/debug layer에만 둔다.

### 3.3 Prototype scene은 하나

메인 작업 씬:

```text
Assets/_dev/Scenes/Dev_Prototype_v0.unity
```

기존 `Dev_EchoSlice.unity`는 참고/폐기 후보.

## 4. 작업 순서

### Phase A. 새 Prototype Scene Skeleton

목표:

- 새 씬에서 기본 게임 구조를 다시 시작한다.

작업:

- `Dev_Prototype_v0.unity` 생성.
- 루트 구조:
  - `PrototypeRoot`
  - `Services`
  - `Player`
  - `EnemySpawner`
  - `Arena`
  - `RuntimeVFX`
  - `HUD`
- 카메라/arena/player/enemy/weapon 배치.

완료 기준:

- Play Mode에서 플레이어가 움직이고 카메라가 따라간다.

### Phase B. Combat Loop

목표:

- 30초 동안 실제 전투가 된다.

작업:

- nearest enemy targeting.
- weapon hit area.
- enemy HP/death.
- enemy spawn/respawn.
- player HP/contact damage.
- hit flash/hitstop/knockback.

완료 기준:

- 플레이어가 적을 죽이고, 적에게 맞고, 죽거나 리셋될 수 있다.

### Phase C. Memory Selection

목표:

- 기억 선택이 실제 run progression이 된다.

작업:

- kill count 또는 timer 기반 선택 UI.
- Kalmuri/Blood memory 선택.
- active memory level up.
- HUD 표시.

완료 기준:

- 플레이 중 기억을 선택하고 강화할 수 있다.

### Phase D. Forgetting/Echo

목표:

- LETHE의 핵심인 망각/잔향 루프가 실제 작동한다.

작업:

- forget event.
- highest-level active memory selection.
- active memory remove.
- echo create/level up.
- Kalmuri/Blood echo combat effect.
- +5 awakening.

완료 기준:

- 기억을 잃고, 그 흔적이 무기 공격 방식으로 남는다.

### Phase E. Resonance/Ultimate

목표:

- 공명과 피의 칼폭풍이 프로토타입 목표가 된다.

작업:

- reacquire memory.
- resonance bonus.
- Kalmuri +5 and Blood +5 condition.
- Blood Blade Storm unlock/activation.

완료 기준:

- 플레이어가 “망각한 것들로 궁극을 만든다”를 체감한다.

## 5. 첫 구현 단위

다음 Codex 작업은 Phase A + Phase B 일부다.

작업:

1. `Dev_Prototype_v0.unity` 생성.
2. scene root 구조 생성.
3. player prefab/runtime 배치.
4. camera follow.
5. arena bounds.
6. enemy spawner 5마리.
7. nearest enemy targeting.
8. player HP/contact damage.
9. HUD 최소 표시.

완료 기준:

- `Dev_Prototype_v0`를 Play하면 30초 동안 이동/공격/피격/처치/리스폰이 된다.

## 6. 폐기 기준

아래는 더 이상 성공 기준이 아니다.

- 잔향 VFX가 보인다.
- 키를 눌러 상태가 바뀐다.
- 적 1마리를 때릴 수 있다.
- debug panel이 상태를 설명한다.

새 성공 기준:

```text
설명 없이 Play를 눌렀을 때,
이게 LETHE의 Unity 프로토타입이라는 것이 게임 루프로 보인다.
```
