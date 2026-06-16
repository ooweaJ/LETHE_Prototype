# 2026-06-16-01 - 카드 선택 정지와 히트스톱 이동감 수정

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 이제 카드 선택 중 적과 투사체가 멈추고, 무기/잔향 히트스톱 중에도 플레이어 쪽 이동·카메라·무기 비주얼 업데이트가 끊기지 않도록 수정됐다.

## 2. 오늘 바뀐 것

- `GameplayPaused`를 추가했다.
  - 시작 무기 선택.
  - 레벨업 카드 선택.
  - 망각 결과 화면.
  - 공명 재획득 화면.
  - 사망 화면.
- 적, 칼무리 투사체, 적 탄환이 pause 상태를 존중하게 바꿨다.
- `HitstopActive`를 추가했다.
  - 전투 히트스톱 때 적/탄은 잠깐 멈춘다.
  - 플레이어, 카메라, 무기 비주얼은 계속 업데이트된다.
- 대검 기본공격이나 칼무리 잔향 때문에 캐릭터 이동이 잠깐 끊기는 느낌을 줄였다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 7 warning / 0 error.
- Unity compile errors: `count=0`.
- Play Mode targeted smoke:
  - `pauseDistance=0.0000`
  - `unpauseDistance=0.0240`
  - `weaponAnimAfterHitstop=0.220`
  - `hitstopAfterUpdate=0.060`
  - `gameplayPaused=False`
  - `hitstopActive=True`
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과, 1 unit heading ok.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패.

## 4. 결정한 것

- 카드 선택은 게임 진행을 멈추는 진짜 pause로 취급한다.
- 히트스톱은 플레이어 입력까지 막는 정지가 아니라, 적/탄/전투 연출 쪽에만 걸리는 충격 표현으로 낮춘다.

## 5. 문제 또는 리스크

- 플레이어 이동감은 개선됐지만, 실제 체감은 jaewoo가 이동하면서 대검/칼무리 잔향을 써봐야 확정된다.
- 너무 부드러워지면 타격감이 약해질 수 있다. 그 경우 카메라 흔들림, 적 피격 플래시, VFX를 더 키우는 쪽이 맞다.
- Project Orchestrator Discord intake는 현재 `fetch failed`를 반환한다.

## 6. GPT/Claude 인계 요약

카드 선택 중 적 이동과 hitstop으로 인한 캐릭터 정지감은 구조상 문제였다. pause와 hitstop을 분리했고, 독립 업데이트되는 적/탄이 이를 따르게 했다.

## 7. 다음 Codex 작업

- jaewoo가 카드 선택 중 적 정지와 이동 중 대검/칼무리 hitstop 체감을 다시 확인한다.
- 여전히 멈춘다고 느껴지면 플레이어 측 hitstop을 완전히 제거하고, 적 freeze + VFX + shake 중심으로 타격감을 만든다.

## 8. 포트폴리오 메모

- 문제: UI 선택과 전투 연출이 플레이어 제어감을 해쳤다.
- 방향: 선택 pause와 전투 hitstop을 분리했다.
- 행동: 전역 pause/freeze 플래그를 추가하고 독립 업데이트 객체들이 따르게 했다.
- 결과: 스모크에서 pause 중 적 이동 0, pause 해제 후 이동, hitstop 중 매니저 업데이트 지속을 확인했다.

# 2026-06-16-02 - 쌍검/대검 반달형 타격 VFX 보정

## 1. 현재 빌드 상태

`Dev_Prototype_v1`의 기본공격 VFX가 얇은 선 중심에서 반달형 베기 중심으로 바뀌었다. 쌍검은 두 개의 작은 반달이 빠르게 겹쳐지고, 대검은 큰 반달이 주변 범위를 베었다고 읽히도록 했다.

## 2. 오늘 바뀐 것

- 쌍검 기본공격:
  - `DualBladeCrescent_A`
  - `DualBladeCrescent_B`
  - 두 개의 반달형 VFX가 타겟 위치에 엇갈려 생성된다.
- 대검 기본공격:
  - `GreatswordCrescent_Aoe`
  - `GreatswordCrescent_Primary`
  - 큰 반달 잔광과 밝은 주 베기가 함께 뜬다.
- 칼무리 잔향:
  - 후속타도 반달형 VFX를 사용하게 바꿨다.
  - 대검 칼무리 후속타는 큰 반달 1회로 읽히게 했다.
- 피의 칼폭풍 대검참도 넓은 반달형 VFX를 사용한다.
- 피격 섬광은 베기선과 겹치지 않도록 impact diamond로 바꿨다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 7 warning / 0 error.
- Unity compile errors: `count=0`.
- Play Mode targeted smoke:
  - `dualCrescent=6`
  - `kalmuriCrescent=10`
  - `greatCrescent=6`
  - `heavyKalmuri=1`
  - `shock=1`
- 증거: `LETHE/Assets/_dev/Evidence/v1_crescent_slash_feedback_20260616.png`.
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과, 2 unit headings ok.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패.

## 4. 결정한 것

- 기본공격의 첫 판독은 데미지 숫자가 아니라 베기 형태로 만든다.
- 쌍검은 작은 반달 2개, 대검은 큰 반달 1개라는 형태 규칙을 유지한다.
- 잔향도 이 무기 형태 언어를 따라가야 한다.

## 5. 문제 또는 리스크

- 아직 절차형 런타임 VFX라 최종 아트는 아니다.
- 타격감이 여전히 약하면 다음은 코드 수치보다 전용 스프라이트/사운드 placeholder가 필요하다.
- Project Orchestrator Discord intake는 현재 `fetch failed`를 반환한다.

## 6. GPT/Claude 인계 요약

타격감 부족 피드백에 따라 쌍검/대검/칼무리 후속타의 VFX 언어를 반달형으로 통일했다. 다음 리뷰에서는 쌍검이 “슥슥”, 대검이 “큰 범위 베기”로 읽히는지 확인해야 한다.

## 7. 다음 Codex 작업

- jaewoo가 쌍검/대검을 각각 플레이하며 반달형 VFX를 확인한다.
- 약하면 전용 sprite atlas 제작 또는 임시 사운드/화면 흔들림 레이어를 붙인다.

## 8. 포트폴리오 메모

- 문제: 공격이 맞아도 화면에서 베었다는 감각이 약했다.
- 방향: 무기별 타격 형태를 시각 규칙으로 고정했다.
- 행동: 쌍검/대검/잔향 VFX를 반달형으로 재구성했다.
- 결과: 스모크에서 쌍검, 대검, 칼무리 후속타 반달 VFX 생성이 확인됐다.

# 2026-06-16-03 - 반달형 참격 크기와 지속시간 보정

## 1. 현재 빌드 상태

쌍검 반달 VFX는 더 커지고 오래 남는다. 대검 반달 VFX는 두께와 채움이 줄어 부채꼴보다 얇은 참격선에 가깝게 보이도록 조정됐다.

## 2. 오늘 바뀐 것

- 쌍검 1타 반달:
  - scale `0.62 -> 0.78`.
  - lifetime `0.13s -> 0.21s`.
- 쌍검 2타 반달:
  - scale `0.54 -> 0.68`.
  - lifetime `0.15s -> 0.23s`.
- 쌍검 보조 타격:
  - scale `0.38 -> 0.50`.
  - lifetime `0.10s -> 0.15s`.
- 대검 AoE 반달:
  - scale `1.02 -> 0.88`.
  - alpha `0.40 -> 0.32`.
  - lifetime `0.28s -> 0.24s`.
- 대검 주 베기:
  - scale `0.78 -> 0.66`.
- `MakeWideCrescentSprite` 내부 채움과 glow를 줄여 부채꼴 면적감을 낮췄다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 7 warning / 0 error.
- Unity compile errors: `count=0`.
- Play Mode targeted smoke:
  - `dualCrescent=6`
  - `dualMaxScale=0.78`
  - `kalmuriCrescent=10`
  - `greatCrescent=6`
  - `greatMaxScale=0.88`
  - `shock=1`
- 증거: `LETHE/Assets/_dev/Evidence/v1_crescent_slash_timing_size_20260616.png`.
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과, 3 unit headings ok.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패.

## 4. 결정한 것

- 쌍검은 작고 빠른 선보다, 조금 더 오래 보이는 2연 반달이 맞다.
- 대검은 큰 면이 아니라 얇고 긴 궤적이어야 한다.
- 절차형 VFX 튜닝은 여기서 더 과하게 끌지 않고, 다음에도 약하면 전용 스프라이트로 넘어간다.

## 5. 문제 또는 리스크

- 아직 런타임 절차형 참격이라 전문 아트 느낌은 제한적이다.
- 지속시간을 늘렸기 때문에 화면에 잔상이 조금 많아질 수 있다.
- Project Orchestrator Discord intake는 현재 `fetch failed`를 반환한다.

## 6. GPT/Claude 인계 요약

쌍검은 보이지 않을 만큼 작고 짧았고, 대검은 너무 두꺼워 부채꼴처럼 보였다. 쌍검은 크기/지속시간을 올리고, 대검은 scale/alpha/내부 채움을 줄여 얇은 참격에 가깝게 보정했다.

## 7. 다음 Codex 작업

- jaewoo가 쌍검 2연 반달이 보이는지 확인한다.
- 대검이 부채꼴이 아니라 큰 참격선으로 읽히는지 확인한다.
- 여전히 약하면 전용 slash sprite 제작으로 넘어간다.

## 8. 포트폴리오 메모

- 문제: 같은 “반달형”이라도 크기와 두께가 다르면 전혀 다른 공격처럼 읽혔다.
- 방향: 쌍검은 가시성, 대검은 날카로움을 각각 보정했다.
- 행동: scale, alpha, lifetime, procedural texture fill을 조정했다.
- 결과: 스모크에서 커진 쌍검 반달과 줄어든 대검 반달 생성이 확인됐다.

# 2026-06-16-04 - 대검 범위 참격과 피격 피드백 보정

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 이제 대검 반달 VFX가 실제 공격 범위만큼 크게 보이고, 적 피격 시 흰색 플래시와 데미지 숫자가 나온다. 원거리몹은 사거리 안에서 뒤로 빠지지 않고 제자리에서 투사체를 던진다.

## 2. 오늘 바뀐 것

- 대검 AoE 반달:
  - scale `0.88 -> 1.24`.
  - lifetime `0.24s -> 0.42s`.
- 대검 주 베기 반달:
  - scale `0.66 -> 1.02`.
  - lifetime `0.20s -> 0.34s`.
- 이전 패스에서 얇게 만든 반달 텍스처는 유지했다.
  - 목표는 “작은 참격”이 아니라 “범위만큼 큰 얇은 참격”이다.
- 적 피격 시 순백 플래시가 더 오래 보인다.
- 데미지 숫자 UI가 떠서 실제로 맞았는지 읽기 쉬워졌다.
- 원거리몹 `DriftingEye`는 사거리 밖이면 접근하고, 사거리 안이면 정지해서 `EyeShot`을 던진다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 7 warning / 0 error.
- Unity compile errors: `count=0`.
- Play Mode targeted smoke:
  - `greatCrescent=6`
  - `greatMaxScale=1.24`
  - `damageNumbers=5`
  - `whiteEnemies=5`
  - `eyeMovedAtRange=0.0000`
  - `eyeShots=1`
- 증거: `LETHE/Assets/_dev/Evidence/v1_damage_feedback_ranged_cast_20260616.png`.
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과, 4 unit heading ok.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패.

## 4. 결정한 것

- 대검은 범위가 큰 무기이므로 VFX도 범위만큼 커야 한다.
- 부채꼴처럼 보이는 문제는 크기를 줄이는 게 아니라, 내부 채움과 두께를 줄이는 쪽으로 해결한다.
- 피격 판독은 VFX만으로 해결하지 않고 흰색 플래시와 데미지 숫자를 같이 쓴다.
- 원거리몹은 카이팅 몹이 아니라 정지 사격 몹으로 둔다.

## 5. 문제 또는 리스크

- 데미지 숫자는 현재 `OnGUI` 기반 프로토타입 구현이라 최종 UI 방식은 아니다.
- 흰색 플래시와 데미지 숫자가 너무 많으면 화면이 지저분할 수 있다.
- Project Orchestrator Discord intake는 최근 dry-run에서 `fetch failed`를 반환하고 있다.

## 6. GPT/Claude 인계 요약

대검 반달은 작게 줄이는 방향이 아니라 범위 판독을 위해 다시 크게 키웠다. 대신 절차형 반달 자체는 얇게 유지해 부채꼴 문제를 피한다. 이번 리뷰에서는 큰 대검 공격, 흰색 피격 플래시, 데미지 숫자, 원거리몹 정지 사격이 한 번에 체감되는지 보면 된다.

## 7. 다음 Codex 작업

- jaewoo가 대검으로 플레이하며 큰 반달이 공격 범위로 읽히는지 확인한다.
- 피격 흰색 플래시와 데미지 숫자가 충분한지 확인한다.
- 원거리몹이 사거리 안에서 후퇴하지 않고 정지 사격하는지 확인한다.
- 여전히 타격감이 약하면 절차형 튜닝을 멈추고 전용 slash sprite와 임시 사운드 레이어로 넘어간다.

## 8. 포트폴리오 메모

- 문제: 대검 VFX를 줄이면 부채꼴 문제는 줄지만 대검 범위감도 같이 사라졌다.
- 방향: 크기는 범위에 맞추고, 형태는 얇은 반달로 정리했다.
- 행동: 대검 반달 scale/lifetime을 키우고, 적 흰색 플래시와 데미지 숫자, 원거리몹 정지 사격을 추가했다.
- 결과: 스모크에서 대검 최대 scale `1.24`, 데미지 숫자 `5`, 흰색 피격 적 `5`, 원거리몹 이동량 `0.0000`, 탄환 `1`을 확인했다.

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
