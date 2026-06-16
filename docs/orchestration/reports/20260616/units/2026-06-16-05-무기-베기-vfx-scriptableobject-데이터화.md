# 2026-06-16-05 - 무기/베기 VFX ScriptableObject 데이터화

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 이제 쌍검/대검 수치와 베기 VFX/피격 피드백 값을 코드가 아니라 `_dev/Data/Weapons`의 ScriptableObject 자산에서 읽는다.

## 2. 오늘 바뀐 것

- `WeaponDefinition`을 확장했다.
  - 공격 범위, 데미지, 공격 간격, 각도, 타겟 수.
  - 넉백, 히트스톱, 카메라 흔들림.
  - 잔향 크기/데미지 배율.
  - 타겟팅 방식, 잔향 proc 스타일, 궁극 패턴.
- `WeaponVfxProfile`을 추가했다.
  - 기본공격 베기 VFX.
  - 칼무리 잔향 후속타 VFX.
  - 대검 피의 칼폭풍 참격 VFX.
  - 피격 섬광.
  - 적 흰색 플래시.
  - 데미지 숫자 색/수명.
- `SlashVfxEntry`를 추가했다.
  - 반달/넓은 반달/impact/circle 형태.
  - 기준 위치, offset, 좌우/회전 mirror, scale, color, lifetime.
- 새 자산을 만들고 씬에 연결했다.
  - `Weapon_DualBlades.asset`
  - `Weapon_Greatsword.asset`
  - `VFX_Weapon_DualBlades.asset`
  - `VFX_Weapon_Greatsword.asset`

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 7 warning / 0 error.
- Unity compile errors: `count=0`.
- Play Mode targeted smoke:
  - `dualSO=True`
  - `dualVfx=True`
  - `dualEntries=4`
  - `dualSparkProfile=DualBladeHitSpark`
  - `greatSO=True`
  - `greatVfx=True`
  - `greatEntries=5`
  - `greatSparkProfile=GreatswordHitSpark`
  - `dualA=3`
  - `dualB=1`
  - `great=1`
  - `dualSpark=3`
  - `greatSpark=1`
  - `dmg=4`
- 증거: `LETHE/Assets/_dev/Evidence/v1_weapon_vfx_profile_data_20260616.png`.
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과, 5 unit heading ok.
- `npm.cmd run report:orchestrator:unit:dry`: `404 Not Found`, `project not found`로 실패.

## 4. 결정한 것

- 앞으로 무기 수치와 베기/VFX 타격감은 `V1GameManager` 코드 숫자가 아니라 ScriptableObject 자산에서 조절한다.
- `V1GameManager`는 임시 프로토타입 매니저여도, 정상 런타임 경로는 데이터 기반으로 둔다.
- 다음 대형 데이터화는 기억/잔향/적/페이싱 쪽이다.

## 5. 문제 또는 리스크

- 아직 절차형 slash sprite를 런타임에서 만들고 있다. 전용 sprite atlas로 교체할 수 있도록 profile 구조는 열어뒀다.
- 현재 데이터화는 무기/VFX 중심이다. 기억/잔향/적 수치는 아직 `V1GameManager` 안에 남아 있다.
- `dotnet build`의 7 warning은 기존 v0/debug deprecated API 경고다.

## 6. GPT/Claude 인계 요약

무기와 베기 VFX는 이제 `_dev/Data/Weapons` ScriptableObject 자산이 source of truth다. 쌍검/대검 베기 scale, lifetime, color, hit spark, enemy flash, damage number 설정은 코드 수정 없이 Unity Inspector에서 조절하는 방향으로 바뀌었다.

## 7. 다음 Codex 작업

- jaewoo가 `Dev_Prototype_v1`을 플레이하며 쌍검/대검 타격감을 다시 확인한다.
- 수치/VFX 보정이 필요하면 `_dev/Data/Weapons` 자산 값을 먼저 조정한다.
- 구조 작업을 이어간다면 Memory/Echo/Enemy 데이터화를 다음 단위로 진행한다.

## 8. 포트폴리오 메모

- 문제: 베기 크기, 지속시간, 색, 피격 피드백을 매번 코드로 바꾸면 플레이 감각 튜닝 속도가 느리다.
- 방향: 게임 완성 단계로 가기 위해 플레이어 체감값을 데이터 자산으로 옮겼다.
- 행동: `WeaponDefinition`, `WeaponVfxProfile`, `SlashVfxEntry`를 추가하고 씬에 연결했다.
- 결과: 스모크에서 쌍검/대검 SO와 VFX profile이 연결됐고, profile 기반 베기/피격섬광/데미지 숫자 생성이 확인됐다.
