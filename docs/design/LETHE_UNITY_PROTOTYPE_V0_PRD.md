# LETHE Unity Prototype v0 PRD

최종 갱신: 2026-06-11

## 0. 문서 역할

이 문서는 LETHE Unity Prototype v0.1을 구현하기 위한 상위 PRD다.

참고 문서:

- 게임 개요: `LETHE_GAME_DESIGN_OVERVIEW.md`
- 핵심 시스템: `LETHE_CORE_SYSTEMS_UNITY_PLAN.md`
- 런 구조: `LETHE_RUN_STRUCTURE.md`
- 전투 방향: `LETHE_COMBAT_DESIGN.md`
- 기억/잔향 상세: `LETHE_WEAPON_MEMORY_ECHO_SPEC.md`, `LETHE_WEAPON_MEMORY_ECHO_DETAIL.md`
- Unity 잔향 구조: `LETHE_UNITY_ECHO_SYSTEM_PRD.md`
- 콘텐츠 표: `LETHE_CONTENT_TABLES.md`
- 밸런스 기준: `LETHE_BALANCE_BASELINE.md`
- Prototype 전환 계획: `LETHE_UNITY_PROTOTYPE_V0_PLAN.md`

이 PRD가 최신 실행 기준이다. 세부 설계는 위 문서를 참고하되, 구현 순서와 완료 기준은 이 문서를 우선한다.

## 1. 배경

HTML v0.12는 LETHE의 망각/잔향 규칙과 자동 밸런스 근거를 만들었다. 하지만 Unity에서 만든 `Dev_EchoSlice`는 잔향 VFX 상태 확인에 치우쳤고, HTML보다 게임 루프 체감이 낮았다.

따라서 Unity 작업의 목표를 바꾼다.

```text
기존 목표:
잔향이 화면에 보이는지 확인한다.

새 목표:
HTML보다 명확하게 플레이되는 Unity 2D 프로토타입에서
이동, 전투, 성장, 망각, 잔향, 공명, 궁극 목표를 체험한다.
```

## 2. 제품 목표

Prototype v0.1은 아래 질문에 답해야 한다.

1. LETHE가 Unity에서 기본 액션 게임으로 성립하는가?
2. 적 압박, 이동, 공격, 피격, 처치가 30~60초 안에 읽히는가?
3. 기억 선택과 강화가 실제 플레이 흐름으로 작동하는가?
4. 최고 레벨 기억을 잃는 망각이 아쉽지만 납득되는가?
5. 잔향이 약화판이 아니라 무기 공격 형태 변화로 보이는가?
6. 잃었던 기억을 다시 얻는 공명이 보상처럼 느껴지는가?
7. 칼무리 + 혈반 각성 잔향이 피의 칼폭풍이라는 목표로 이어지는가?

## 3. 대상 플레이 경험

플레이어는 첫 1~3분 안에 아래 흐름을 경험한다.

1. 쌍검을 들고 arena에서 시작한다.
2. 적 다수가 플레이어를 압박한다.
3. 플레이어는 이동하며 가까운 적을 자동/반자동 공격한다.
4. 처치 또는 시간으로 기억 선택을 받는다.
5. `굶주린 칼무리` 또는 `피의 반사`를 얻고 강화한다.
6. 일정 조건에서 최고 레벨 기억이 망각된다.
7. 잃은 기억은 같은 계열 잔향으로 남는다.
8. 잔향이 무기 공격에 다른 형태로 붙는다.
9. 잃었던 기억을 다시 얻으면 공명이 발생한다.
10. 칼무리 잔향 +5와 혈반 잔향 +5가 피의 칼폭풍 목표를 연다.

## 4. MVP 범위

### 4.1 Scene

메인 씬:

```text
Assets/_dev/Scenes/Dev_Prototype_v0.unity
```

기존 씬:

```text
Assets/_dev/Scenes/Dev_EchoSlice.unity
```

`Dev_EchoSlice`는 reference only다. 더 이상 메인 구현 대상으로 삼지 않는다.

### 4.2 Core Gameplay

필수:

- 플레이어 이동: WASD/방향키.
- 카메라 추적.
- arena 경계.
- 적 5마리 이상 spawn/chase.
- 적 처치 후 respawn/replacement.
- 플레이어 HP.
- 적 접촉 피해.
- 플레이어 사망/리셋.
- 가까운 적 자동 타겟팅.
- 쌍검 공격 판정.
- hit flash, hitstop, recoil/knockback 중 최소 2개.
- 최소 HUD: HP, kills, time, active memory, echo, next forget candidate.

### 4.2-A Prototype Sprite / Animation

Prototype v0.1은 정식 아트 완성이 아니어도, 캐릭터와 적이 정지 이미지처럼 보이면 실패로 본다.

필수:

- Player 4방향 idle animation.
  - down, up, left, right.
  - 각 방향 최소 2프레임, 권장 4프레임.
- Player 4방향 walk animation.
  - down, up, left, right.
  - 각 방향 최소 4프레임.
- Enemy 4방향 idle animation.
  - down, up, left, right.
  - 각 방향 최소 2프레임.
- Enemy 4방향 walk/chase animation.
  - down, up, left, right.
  - 각 방향 최소 4프레임.
- Sprite sheet 기준:
  - 한 frame은 64x64 또는 96x96 기준.
  - player와 enemy는 별도 sheet.
  - chroma-key source 보존 후 alpha PNG로 변환.
  - Unity import는 Multiple Sprite 기준으로 slice한다.

첫 구현 파일명:

```text
Assets/_dev/Art/Source/sheet_player_4dir_chroma.png
Assets/_dev/Art/Source/sheet_enemy_chaser_4dir_chroma.png
Assets/_dev/Art/Sprites/Characters/Player/sheet_player_4dir.png
Assets/_dev/Art/Sprites/Enemies/Chaser/sheet_enemy_chaser_4dir.png
```

Animator 요구사항:

- `PrototypeSpriteAnimator` 또는 동등한 runtime이 이동 방향과 속도에 따라 idle/walk frame을 바꾼다.
- 첫 버전은 Unity Animator Controller를 쓰지 않아도 된다. C# frame swap runtime 허용.
- 방향 판정은 마지막 이동 방향을 기준으로 한다.

### 4.3 First Content

무기:

- `Weapon_DualBlades` / 절단쌍검.

적:

- `Enemy_MeleeChaser` / 기본 근접 적.

활성 기억:

- `Memory_HungryBlades` / 굶주린 칼무리.
- `Memory_BloodReflection` / 피의 반사.

잔향:

- `Echo_Kalmuri` / 칼무리 잔향.
- `Echo_Blood` / 혈반 잔향.

궁극:

- `Synergy_BloodBladeStorm` / 피의 칼폭풍.

### 4.4 Progression

Prototype v0.1에서는 완전한 로그라이크 progression이 아니라 압축 루프를 사용한다.

- 첫 기억 선택: 30~50초 안 또는 kill count 5 전후.
- 첫 망각: 90~150초 안 또는 debug trigger.
- 결손 생존: 30~60초.
- 보충/재획득 선택: 망각 후 짧은 전투 뒤.
- 첫 각성 잔향: 한 번의 테스트 세션 안에서 가능.

## 5. 비범위

Prototype v0.1에서 하지 않는다.

- 상점.
- 메타 성장.
- 다중 지역.
- 최종 보스.
- 장송대검.
- 8종 기억 전체 구현.
- 4종 궁극 전체 구현.
- 정식 아트 완성.
- 정식 사운드 전체 제작.
- `Assets/Lethe` 승격.

## 6. 시스템 요구사항

### 6.1 Runtime State

`RunBuildState`는 아래 상태를 제공해야 한다.

- current weapon id.
- active memories: id -> level.
- echoes: id -> level.
- unlocked synergies.
- next forget candidate.
- kill count / run time.
- player HP snapshot.

### 6.2 Combat

전투 이벤트는 아래 흐름을 따른다.

```text
WeaponRuntime
-> HitEvent(sourceType=WeaponHit)
-> HitResolver
-> Health damage/death
-> FeedbackService
-> EchoTriggerRouter
-> EchoRuntime
```

규칙:

- `EchoHit`는 다시 일반 온힛 echo를 무한 발동시키면 안 된다.
- `HitResolver`는 특정 echo id를 몰라야 한다.
- `EchoTriggerRouter`는 trigger family를 보고 라우팅한다.
- VFX만 있고 damage/log가 없는 잔향은 PRD 완료로 보지 않는다.

### 6.3 Memory / Forgetting / Echo

망각 규칙:

- 활성 기억 중 최고 레벨 기억을 잃는다.
- 동률이면 선택 UI가 이상적이지만 v0.1에서는 최근 강화 기억 우선 fallback 허용.
- 잃은 기억 레벨만큼 matching echo에 더한다.
- echo max는 +5.
- 초과분은 overcharge event로 기록한다.

재획득 공명:

```text
reacquired level = min(5, base level + floor(echo level / 2))
```

잔향은 소비하지 않는다.

### 6.4 Data

v0.1부터 아래 데이터 asset을 만든다.

- `Assets/_dev/Data/Weapons/Weapon_DualBlades.asset`
- `Assets/_dev/Data/Memories/Memory_HungryBlades.asset`
- `Assets/_dev/Data/Memories/Memory_BloodReflection.asset`
- `Assets/_dev/Data/Echoes/Echo_Kalmuri.asset`
- `Assets/_dev/Data/Echoes/Echo_Blood.asset`
- `Assets/_dev/Data/Synergies/Synergy_BloodBladeStorm.asset`

원칙:

- Definition은 데이터와 prefab reference를 가진다.
- Runtime component가 실제 동작을 수행한다.
- debug controller는 상태를 빠르게 만드는 도구일 뿐, 시스템의 핵심 로직을 들고 있지 않는다.

## 7. UX / HUD 요구사항

HUD는 최소한 아래를 보여준다.

- HP bar/text.
- kill count.
- run time.
- active memory slots and levels.
- echo slots and levels.
- next forget candidate.
- Blood Blade Storm requirement progress:
  - Kalmuri Echo +5.
  - Blood Echo +5.

UI 원칙:

- 설명문보다 상태 표시가 우선이다.
- 잔향/공명/궁극은 텍스트만이 아니라 아이콘, 색, VFX 상태와 함께 읽혀야 한다.
- debug panel은 개발용이며 화면 중심을 가리면 안 된다.

## 8. Milestones

### M1. Prototype Scene Skeleton

목표:

- 새 씬과 root 구조를 만든다.

완료 기준:

- `Dev_Prototype_v0.unity` 존재.
- `PrototypeRoot`, `Services`, `Player`, `EnemySpawner`, `Arena`, `RuntimeVFX`, `HUD` 존재.
- 플레이어 이동과 카메라 follow 작동.
- player/enemy 4방향 idle/walk sprite sheet가 import되어 runtime frame swap이 가능하다.
- compile/console/missing reference 0.

### M2. Combat Loop

목표:

- 30초 동안 게임처럼 싸울 수 있다.

완료 기준:

- 적 5마리 이상 spawn/chase.
- 가까운 적 자동 타겟팅.
- 쌍검 hit 판정.
- enemy HP/death/respawn.
- player HP/contact damage/death/reset.
- HUD에 HP/kills/time 표시.

### M3. Memory Selection

목표:

- 플레이 중 기억을 얻고 강화한다.

완료 기준:

- kill/time 조건으로 선택 UI 발생.
- Kalmuri/Blood 중 선택 가능.
- 선택 결과가 active memory level에 반영.
- HUD에 active memory 표시.

### M4. Forgetting / Echo

목표:

- 망각과 잔향이 실제 게임 시스템으로 작동한다.

완료 기준:

- 최고 레벨 기억 망각.
- matching echo 생성/강화.
- Kalmuri/Blood echo가 실제 combat effect와 damage를 만든다.
- echo +1~+5 구분.
- next forget candidate HUD 표시.

### M5. Resonance / Ultimate

목표:

- 공명과 피의 칼폭풍이 목표로 작동한다.

완료 기준:

- 잔향이 있는 기억 재획득 시 공명 보너스.
- Kalmuri Echo +5와 Blood Echo +5 조건 체크.
- Blood Blade Storm unlock/activation.
- 궁극이 damage, VFX, recovery loop를 만든다.

## 9. Acceptance Tests

### Human Playtest

jaewoo가 직접 Play Mode에서 확인한다.

필수 질문:

- 30초 안에 게임처럼 움직이고 싸우는가?
- 적 압박이 보이는가?
- 쌍검을 들고 공격한다는 것이 보이는가?
- 기억 선택이 성장으로 느껴지는가?
- 망각이 손실로 느껴지는가?
- 잔향이 공격 형태 변화로 보이는가?
- 공명/궁극이 다음 목표로 보이는가?

### Automated / MCP Checks

각 milestone마다:

- Unity compile errors: 0.
- Unity console errors: 0.
- scene missing references: 0.
- Play Mode runtime smoke check.
- report generation pass.
- report check pass.

## 10. Implementation Order

다음 Codex 작업은 아래 순서로 진행한다.

1. M1 scene skeleton.
2. M2 combat loop.
3. M3 memory selection.
4. M4 forgetting/echo.
5. M5 resonance/ultimate.

각 milestone은 별도 commit으로 남긴다.

## 11. Risk

| Risk | 대응 |
| --- | --- |
| 또 VFX 테스트 장치로 흐름 | `Dev_Prototype_v0` 성공 기준을 combat loop로 고정 |
| debug controller가 시스템을 삼킴 | debug는 state injection만 허용 |
| 데이터화 전에 hard-code 증가 | v0.1부터 Definition asset 생성 |
| 카메라/스케일 문제 반복 | M1에서 scene composition을 acceptance에 포함 |
| 잔향이 damage 없는 장식이 됨 | EchoHit과 HitResolver 경로 필수 |

## 12. Decision Log

- `Dev_EchoSlice`는 메인 경로에서 제외한다.
- 새 메인 씬은 `Dev_Prototype_v0.unity`다.
- Prototype v0.1은 HTML보다 낮은 수준의 VFX test가 아니라, 실제 30~60초 combat loop를 목표로 한다.
- 잔향 구현은 combat loop 이후에 올린다.
