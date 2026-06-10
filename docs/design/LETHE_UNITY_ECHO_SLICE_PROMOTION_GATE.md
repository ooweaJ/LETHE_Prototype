# LETHE Unity Echo Slice Promotion Gate

최종 갱신: 2026-06-11

## 0. 이 문서의 역할

이 문서는 `Assets/_dev`에서 만든 Unity echo slice를 언제 `Assets/Lethe`로 승격할지 정하는 체크리스트다.

중요:

- 이 문서는 승격을 실행하는 문서가 아니다.
- jaewoo 리뷰 전에는 `Assets/Lethe`로 옮기지 않는다.
- GO 판정 전에는 `_dev` 구조를 유지한다.

## 1. 현재 리뷰 대상

Scene:

```text
Assets/_dev/Scenes/Dev_EchoSlice.unity
```

현재 slice:

```text
절단쌍검 + 칼무리 + 혈반 + 피의 칼폭풍
```

현재 controls:

```text
1 = 기본 쌍검
2 = 칼무리 +1
3 = 칼무리 +5
4 = 혈반 +5
5 = 피의 칼폭풍
Space = 강제 공격
```

## 2. GO 조건

아래가 모두 참이면 GO다.

- `Dev_EchoSlice.unity`가 Play Mode에서 에러 없이 실행된다.
- `1~5` 전환이 10초 안에 모두 확인된다.
- 칼무리 +1과 칼무리 +5가 서로 다른 행동으로 보인다.
- 혈반 +5가 단순 붉은 damage VFX가 아니라 적 표식/회복 피드백으로 읽힌다.
- 피의 칼폭풍이 칼무리와 혈반이 합쳐진 상태로 보인다.
- jaewoo가 “이 방향으로 더 다듬을 가치가 있다”고 판단한다.

## 3. ITERATE 조건

아래 중 하나면 ITERATE다.

- 방향은 맞지만 VFX 크기/색/위치 때문에 잘 안 보인다.
- debug loop는 작동하지만 한 상태가 지나치게 지저분하다.
- 피의 칼폭풍은 보이지만 궁극처럼 강하지 않다.
- 회복 실이나 혈반 표식이 플레이어에게 의미 있게 읽히지 않는다.

ITERATE 우선순위:

1. VFX scale/position/sorting 조정.
2. hit timing 조정.
3. heal thread readability 조정.
4. 기본 베기 arc 추가.
5. hitstop/sound/camera impulse 추가.

## 4. NO-GO 조건

아래 중 하나면 NO-GO다.

- 잔향이 여전히 기본 공격 위에 붙은 장식처럼 느껴진다.
- 칼무리와 혈반이 서로 다른 시스템으로 읽히지 않는다.
- 피의 칼폭풍이 궁극 목표로 매력적이지 않다.
- Unity에서 더 진행하기 전에 기획 자체를 다시 잡아야 한다.

## 5. 승격 전 기술 체크

GO가 나더라도 아래 작업 전에는 `Assets/Lethe`로 옮기지 않는다.

- Debug-only `DevEchoSliceDebugController`에서 검증된 동작을 production runtime class로 분리한다.
- Persistent orbit/storm 생성 경로를 `PoolService` 또는 runtime prefab lifecycle로 정리한다.
- `WeaponDefinition`, `MemoryDefinition`, `EchoDefinition`, `EchoSynergyDefinition` asset을 실제로 생성하고 prefab reference를 연결한다.
- `EchoTriggerRouter`와 `HitResolver`에 특정 echo id 분기가 없는지 재검토한다.
- missing reference scan을 scene/assets 양쪽에서 통과한다.
- Play Mode console error가 0인지 확인한다.
- 보고서와 devlog에 GO 사유와 승격 범위를 남긴다.

## 6. 승격 대상 후보

GO 후 승격 후보:

- `Assets/_dev/Art/Sprites/Echoes/Kalmuri/*`
- `Assets/_dev/Art/Sprites/Echoes/Blood/*`
- `Assets/_dev/Art/Sprites/Ultimates/*`
- `Assets/_dev/Prefabs/Echoes/*`
- `Assets/_dev/Prefabs/Ultimates/*`
- production으로 정리된 echo runtime scripts
- production으로 정리된 data assets

GO 전 승격 금지:

- `Assets/_dev/Scripts/Debug/DevEchoSliceDebugController.cs`
- `Assets/_dev/Scenes/Dev_EchoSlice.unity` 원본 그대로
- 임시 OnGUI debug panel

## 7. 다음 Codex 입력

jaewoo 리뷰 후 Codex에게 아래 형식으로 전달한다.

```text
Unity Echo Slice Review:
Decision: GO | ITERATE | NO-GO
Weakest state:
Strongest state:
Blood Blade Storm feel:
First fix:
Promotion allowed: yes | no
```
