# LETHE Unity 잔향 시스템 PRD / 구조 설계

최종 갱신: 2026-06-10

## 0. 이 문서의 위치

이 문서는 `LETHE_ECHO_FORM_TRANSFORMATION_SPEC.md`, `LETHE_WEAPON_MEMORY_ECHO_DETAIL.md`, `LETHE_WEAPON_MEMORY_ECHO_SPEC.md`를 Unity vertical slice로 옮기기 위한 제품 요구사항 및 기술 구조 설계서다.

목표는 바로 모든 기억을 구현하는 것이 아니라, 첫 slice에서 LETHE의 핵심 감정이 실제 전투로 보이는지 검증하는 것이다.

첫 slice 문장:

```text
절단쌍검으로 칼무리와 혈반을 잃고, 그 잔향이 무기에 남아 피의 칼폭풍으로 터진다.
```

## 1. 제품 목표

플레이어는 첫 Unity slice에서 아래 네 문장을 체감해야 한다.

1. 가장 키운 기억을 잃었다.
2. 잃은 기억이 약해진 복사본이 아니라 무기에 남은 다른 형태가 되었다.
3. 같은 기억을 다시 얻으면 활성 기억과 잔향이 공명한다.
4. 두 각성 잔향이 모이면 궁극 잔향 빌드가 열린다.

## 2. 첫 구현 범위

필수:

- 무기 1종: `절단쌍검`
- 활성 기억 2종: `굶주린 칼무리`, `피의 반사`
- 잔향 2종: `칼무리 잔향`, `혈반 잔향`
- 궁극 잔향 1종: `피의 칼폭풍`
- 망각 장면: 칼무리 고리 또는 피의 효과가 무기로 빨려 들어가는 변환 연출
- 공명 장면: 잔향이 있는 기억 재획득 카드와 공명 발동
- 디버그 루프: 강화 -> 망각 -> 잔향 +5 -> 공명 후보 -> 궁극 발동

보류:

- 장송대검
- 처형자의 섬광, 망각의 낙인, 처형 각인
- 나머지 4개 기억/잔향
- 상점, 메타 성장, 다중 지역, 최종 보스

## 3. Unity 권장 패키지와 렌더링 기준

권장:

- Unity 2D URP: 2D Light, Sprite Renderer, Shader Graph 사용.
- Cinemachine: 궁극/처형/강타 임펄스만 담당.
- Input System: 이동/디버그 입력 분리.
- TextMeshPro: HUD, 카드, 디버그 패널.

VFX 기준:

- 첫 slice는 VFX Graph가 필수는 아니다. 2D 파티클, Sprite Renderer, Trail Renderer, Line Renderer를 우선 사용한다.
- 칼선은 Sprite Renderer 또는 Trail Renderer 기반 프리팹으로 시작한다.
- 혈반/피꽃은 Sprite Renderer + Particle System으로 시작한다.
- 회복 실은 Line Renderer 또는 얇은 Trail Renderer로 시작한다.
- 궁극 칼폭풍은 여러 `PooledProjectile`/`OrbitShard`를 조합한다.

성능 기준:

- 칼날, 칼선, 혈반, 피실, 피꽃, 플로터 텍스트는 모두 풀링 대상이다.
- 런타임 `Instantiate/Destroy`는 디버그나 로딩 외 전투 루프에서 금지한다.
- 첫 slice 목표는 200개 이하의 동시 이펙트에서 GC spike 없이 유지되는 것이다.

## 4. 폴더 구조

```text
Assets/Lethe/
  Scenes/
    Slice_EchoShowcase.unity
  Scripts/
    Core/
    Combat/
    Combat/HitEvents/
    Combat/Weapons/
    Combat/Echoes/
    Combat/Echoes/Runtime/
    Combat/Memories/
    Combat/Enemies/
    Feedback/
    Pooling/
    UI/
    Debug/
  Data/
    Weapons/
    Memories/
    Echoes/
    EchoSynergies/
    FeedbackProfiles/
  Prefabs/
    Player/
    Weapons/
    Hitboxes/
    Echoes/
    Echoes/Kalmuri/
    Echoes/Blood/
    Ultimates/
    Enemies/
    UI/
    Feedback/
  Art/
    Sprites/
    Materials/
    Particles/
    Shaders/
  Audio/
    SFX/
    Mixers/
```

## 5. 데이터 ScriptableObject

### `WeaponDefinition`

역할: 무기의 공격 리듬과 잔향 변환 방식을 정의한다.

필드:

```text
id
displayName
attackCadence
hitboxPrefab
baseDamage
hitStopProfile
sfxProfile
echoStyle: SmallFrequent | HeavyConditional
```

첫 데이터:

- `Weapon_DualBlades`

### `MemoryDefinition`

역할: 활성 기억의 데이터와 연결 잔향을 정의한다.

필드:

```text
id
displayName
matchingEchoId
activeAbilityPrefab
maxLevel = 5
levelData[]
resonanceRiderId
```

첫 데이터:

- `Memory_HungryBlades`
- `Memory_BloodReflection`

### `EchoDefinition`

역할: 잔향의 트리거 패밀리, 레벨별 행동, 각성 효과를 정의한다.

필드:

```text
id
sourceMemoryId
displayName
triggerFamily: OnWeaponHit | OnKill | OnDamageTaken | OnShieldBreak | Periodic
maxLevel = 5
levelData: EchoLevelData[]
awakenedName
runtimePrefab
feedbackProfile
loopPolicy
```

첫 데이터:

- `Echo_Kalmuri`
- `Echo_Blood`

### `EchoLevelData`

역할: 레벨별 빈도, 개수, 행동 플래그를 정의한다.

필드:

```text
level
procChance
hitInterval
spawnCount
damageMultiplier
duration
radius
cooldown
behaviors: EchoBehavior flags
```

### `EchoSynergyDefinition`

역할: 두 각성 잔향이 모였을 때 열리는 궁극 잔향을 정의한다.

필드:

```text
id
displayName
requiredEchoIds[]
requiredLevel = 5
runtimePrefab
hudGoalText
feedbackProfile
```

첫 데이터:

- `Synergy_BloodBladeStorm`

### `FeedbackProfile`

역할: 히트스톱, 셰이크, 플래시, 사운드, 라이트 강도를 데이터로 제어한다.

필드:

```text
hitStopFrames
cameraImpulse
enemyFlashColor
enemyFlashDuration
sfxLayers[]
lightColor
lightIntensity
particlePrefab
```

## 6. 런타임 핵심 클래스

### 빌드 상태

`RunBuildState`

- 활성 기억 id/level을 보관한다.
- 잔향 id/level을 보관한다.
- 열린 궁극 잔향을 보관한다.
- UI와 디버그 패널에 읽기 전용 snapshot을 제공한다.

`MemoryInventory`

- 활성 기억 추가, 강화, 제거를 담당한다.
- 활성 기억 슬롯 수 제한을 관리한다.
- 최고 레벨 기억 후보를 `ForgetService`에 제공한다.

`EchoInventory`

- 잔향 레벨 추가, +5 cap, overcharge 계산을 담당한다.
- 각성 상태와 궁극 조건을 계산한다.

### 망각/공명

`ForgetService`

- 최고 레벨 활성 기억을 망각 후보로 고른다.
- 동률이면 UI 선택 요청을 보낸다.
- 기억 제거 후 `EchoInventory.AddEchoLevel()`을 호출한다.
- 망각 변환 연출 이벤트를 발행한다.

`ResonanceService`

- 잔향이 있는 기억이 보상 후보에 등장하면 공명 카드로 표시한다.
- 재획득 레벨 `base + floor(echoLevel / 2)`를 계산한다.
- 공명 rider를 활성 기억 runtime에 붙인다.

`UltimateEchoService`

- `EchoSynergyDefinition` 조건을 감시한다.
- 두 잔향이 모두 +5면 궁극 잔향 runtime을 생성/활성화한다.
- HUD 목표 상태를 갱신한다.

### 전투 이벤트

`WeaponHitEmitter`

- 무기 공격이 적을 맞힐 때 `HitEvent`를 생성한다.
- 모든 기본 잔향은 `WeaponHit`에서 시작한다.

`HitEvent`

```text
sourceType: WeaponHit | EchoHit | UltimateHit
attacker
target
position
direction
damage
tags
canTriggerEcho
canTriggerResonance
```

`HitResolver`

- 피해 적용, 처치 판정, 플래시 요청을 처리한다.
- `KillEvent`를 발행한다.

`EchoTriggerRouter`

- `HitEvent`, `KillEvent`, `DamageTakenEvent`, `ShieldBreakEvent`를 받아 트리거 패밀리별 processor로 전달한다.
- `sourceType`을 보고 무한 proc 루프를 차단한다.

`EchoProcLimiter`

- 한 프레임/한 타격/초당 발동 cap을 관리한다.
- `EchoHit`가 다시 일반 온힛 잔향을 부르지 못하게 막는다.
- 궁극 회복량, 전파 횟수, 타겟 수 cap을 관리한다.

### 잔향 runtime

`EchoRuntimeController`

- 현재 잔향 레벨과 definition을 읽는다.
- 레벨별 behavior flag에 따라 processor를 활성화한다.

`KalmuriEchoRuntime`

- 지연 칼선, 잔류 칼자국, 무기 주변 파편, +5 발사 칼날을 관리한다.
- 활성 칼무리와 공명할 때 shared on-hit rider를 제공한다.

`BloodEchoRuntime`

- 혈반 표식, 회복 실, 피꽃 폭발, 전파를 관리한다.
- 초당 회복량 cap을 적용한다.

`BloodBladeStormRuntime`

- 칼날 고리, 혈반 부착, 회복 실, 처치 가속을 통합한다.
- 궁극 전용 hit source를 사용한다.

### 피드백

`HitStopService`

- 각성/궁극/처형 순간만 짧게 시간을 멈춘다.

`CameraImpulseService`

- Cinemachine Impulse를 감싸고, 궁극/강타만 허용한다.

`SpriteFlashService`

- 피격 적의 white flash를 처리한다.

`SfxLayerService`

- 기본 타격, 잔향, 각성, 궁극 레이어를 동시에 재생한다.

`FloatingTextService`

- 디버그/QA용 텍스트만 담당한다.
- 최종 체감은 텍스트가 아니라 VFX/SFX가 우선이다.

## 7. 첫 slice 프리팹 목록

### Player / Weapon

| 프리팹 | 역할 |
| --- | --- |
| `Player_EchoShowcase` | 이동, 체력, 무기 장착, build state 소유 |
| `Weapon_DualBlades_Runtime` | 쌍검 공격 cadence와 hit emitter |
| `Hitbox_DualBladeArc_L` | 왼손 베기 판정 |
| `Hitbox_DualBladeArc_R` | 오른손 베기 판정 |

### Active Memory

| 프리팹 | 역할 |
| --- | --- |
| `Memory_HungryBlades_Ring` | 활성 칼무리 독립 칼날 고리 |
| `Memory_BloodReflection_Strike` | 활성 피 추가타/작은 회복 |

### Echo

| 프리팹 | 역할 |
| --- | --- |
| `Echo_Kalmuri_DelayedSlash` | +1~+3 반달 지연 칼선 |
| `Echo_Kalmuri_LingerSlash` | +3 잔류 궤도 칼자국 |
| `Echo_Kalmuri_WeaponShard` | +4 무기 주변 희미한 칼날 파편 |
| `Echo_Kalmuri_LaunchBlade` | +5 타격 반응 발사 칼날 |
| `Echo_Blood_Mark` | 혈반 표식 |
| `Echo_Blood_HealThread` | 플레이어에게 돌아오는 회복 실 |
| `Echo_Blood_Bloom` | +5 피꽃 폭발 |
| `Ultimate_BloodBladeStorm` | 칼날 고리 + 혈반 + 회복 실 통합 |

### Enemy / UI / Debug

| 프리팹 | 역할 |
| --- | --- |
| `Enemy_TestWalker` | 느린 근접 테스트 적 |
| `Enemy_TestShooter` | 멀리서 남는 적, 추후 추적 잔향용 |
| `UI_EchoHud` | 잔향 레벨, 궁극 목표, 다음 망각 후보 |
| `UI_ForgetResultPanel` | 잃은 기억, 생긴 잔향, overcharge 표시 |
| `UI_ResonanceChoiceCard` | 공명 재획득 카드 |
| `UI_DebugEchoPanel` | 강화/망각/+5/궁극 발동 버튼 |

## 8. 첫 slice 이벤트 흐름

### 망각

```text
BossOrDebug triggers forget
-> ForgetService selects highest-level memory
-> MemoryInventory removes memory
-> ForgetTransformationVfx plays ring-to-weapon absorption
-> EchoInventory adds matching echo level
-> EchoRuntimeController refreshes active echo runtime
-> UI_ForgetResultPanel opens
```

### 잔향 타격

```text
DualBlades hit enemy
-> WeaponHitEmitter emits HitEvent(sourceType=WeaponHit)
-> HitResolver applies damage
-> EchoTriggerRouter routes to OnWeaponHit processors
-> KalmuriEchoRuntime spawns slash/blade based on level
-> BloodEchoRuntime marks target or returns heal thread
-> EchoProcLimiter blocks recursive EchoHit loops
```

### 궁극

```text
EchoInventory has Kalmuri +5 and Blood +5
-> UltimateEchoService unlocks BloodBladeStorm
-> UI_EchoHud shows active ultimate goal complete
-> BloodBladeStormRuntime starts
-> Blade hits apply blood marks
-> Blood marks return heal threads with healing cap
-> Kills accelerate blade ring briefly
```

## 9. 수락 기준

기능 기준:

- 디버그 버튼으로 `칼무리 +5`, `혈반 +5`, `피의 칼폭풍` 상태를 10초 안에 만들 수 있다.
- 망각 시 활성 칼무리 고리가 사라지고 무기 잔향으로 변하는 장면이 보인다.
- 잔향 +1, +3, +5가 화면 행동으로 구분된다.
- 공명 재획득 시 활성 기억과 잔향이 동시에 존재한다.
- `EchoHit`가 무한 온힛 루프를 만들지 않는다.

체감 기준:

- 플레이어가 "잔향은 약해진 복사본이 아니라 형태가 바뀐 효과"라고 설명할 수 있다.
- 피의 칼폭풍은 칼날, 혈반, 회복 실이 서로 이어지는 루프로 보인다.
- 궁극 발동은 텍스트 없이도 화면과 소리로 구분된다.

성능 기준:

- 전투 중 주요 이펙트는 풀에서 재사용된다.
- 60초 디버그 전투에서 GC spike로 인한 명확한 멈춤이 없어야 한다.
- 궁극 발동 중에도 카메라 셰이크와 파티클이 과도하게 누적되지 않아야 한다.

## 10. 구현 순서

1. 데이터 SO: `WeaponDefinition`, `MemoryDefinition`, `EchoDefinition`, `EchoSynergyDefinition`.
2. 전투 이벤트: `HitEvent`, `WeaponHitEmitter`, `HitResolver`, `EchoTriggerRouter`.
3. 풀링/피드백: `PoolService`, `HitStopService`, `SpriteFlashService`, `SfxLayerService`.
4. 쌍검 기본 공격과 테스트 적.
5. 활성 칼무리와 활성 피의 반사.
6. 망각 서비스와 잔향 레벨 전환.
7. 칼무리 잔향 +1~+5.
8. 혈반 잔향 +1~+5.
9. 공명 재획득 카드와 공명 rider.
10. 피의 칼폭풍.
11. 디버그 패널과 slice 수락 테스트.

## 11. 열어둘 질문

- 첫 Unity slice에서 자동 전투를 유지할지, 수동 공격 입력을 넣을지.
- 피의 칼폭풍이 항상 켜지는 궁극인지, 게이지로 발동하는 궁극인지.
- 혈반 회복 cap을 초당 고정으로 둘지, 최대 체력 비례로 둘지.
- 망각 변환 연출을 실제 보스 후 결과 화면에서만 보여줄지, 전투 중 즉시 보여줄지.
