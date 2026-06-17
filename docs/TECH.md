# LETHE TECH

## Purpose

이 문서는 LETHE를 Unity에서 어떻게 만들지 정의하는 기술 기준이다. 재미와 감정 목표는 `docs/PRD.md`, 현재 작업 보드는 `docs/TASK.md`, 검증 기준은 `docs/TEST.md`에서 관리한다.

## Current Unity Target

- Scene: `LETHE/Assets/_dev/Scenes/Dev_Prototype_v1.unity`
- Runtime: `LETHE/Assets/_dev/Scripts/PrototypeV1/`
- Shared contracts: `LETHE/Assets/_dev/Scripts/Core/`
- Data root: `LETHE/Assets/_dev/Data/`
- Promotion target after GO: `LETHE/Assets/Lethe/`

`Dev_Prototype_v0` and `Dev_EchoSlice` are reference only.

## Architecture Rule

새 기능은 가능한 한 아래 흐름으로 만든다.

```text
ScriptableObject Definition
-> Runtime controller/service
-> Prefab/VFX/Profile reference
-> Smoke test
-> TASK/TEST/CHANGELOG/report update
```

전투 수치, VFX 지속시간, 발동 확률, enemy stat, pacing 값은 코드 리터럴이 아니라 `_dev/Data` 에셋으로 이동시키는 것을 기본값으로 한다.

## Data Model

Primary definitions:

- `WeaponDefinition`: 무기 리듬, 타겟팅, 데미지, 넉백, hitstop, 잔향 상호작용.
- `WeaponVfxProfile`: 무기별 slash, hit spark, damage number, enemy flash.
- `MemoryDefinition`: 활성 기억의 효과, 카드 설명, 레벨별 성장, 대응 잔향.
- `EchoDefinition`: 망각 후 남는 잔향의 trigger, 형태, +1~+5 성장, 각성.
- `UltimateEchoDefinition`: 잔향 +5 조합 궁극의 조건, 발동 방식, 무기별 표현.
- `EnemyDefinition`: 적 role, 체력, 이동, 공격, XP, spawn cost.
- `EncounterDefinition`: 실제 플레이 M2 루프용 spawn pressure/pacing.
- `RewardPoolDefinition`: 무기, 기억, 잔향, 궁극, 적, encounter 묶음.

## Runtime Direction

- `V1GameManager`는 아직 프로토타입 오케스트레이터 역할을 한다.
- B-I 단계가 진행될수록 하드코딩된 enum/switch/숫자는 definition 데이터로 이동한다.
- 런타임이 id를 기준으로 데이터를 조회하고 효과를 실행하게 만든다.
- 새 기억/잔향/궁극 추가 시 `V1GameManager` 코어 변경이 최소화되어야 한다.

## Asset Rule

- 판독용 procedural VFX는 임시로 허용한다.
- 평가 가능한 플레이 감각을 만들 때부터는 `_dev/Art`, `_dev/Prefabs`, `_dev/Data`의 연결 관계를 문서화한다.
- 스프라이트 교체는 게임 루프가 읽히는 상태에서 진행한다.

## Unity MCP Rule

Unity Editor 조작은 AnkleBreaker Unity MCP를 우선한다. MCP가 도구 목록에 노출되지 않으면 파일/C# 작업과 `dotnet build`까지 진행하고, Editor scene/prefab 검증은 MCP 연결 복구 후 수행한다.
