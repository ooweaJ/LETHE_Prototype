# 2026-06-23-01 - 대검 손잡이 방향과 칼끝 VFX 정렬

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 계속 `_dev` 검증 빌드다. 이번 작업은 새 시스템 추가가 아니라 대검 공격 연출의 좌표 기준을 고친 작업이다.

## 2. 오늘 바뀐 것

- 대검 팬텀 무기는 이제 칼끝이 지나갈 위치를 먼저 계산한다.
- 무기 중심은 그 칼끝에서 뒤로 밀어 배치해서, 손잡이가 플레이어 몸쪽을 향하게 했다.
- 대검 베기 VFX는 무기 중심이 아니라 계산된 칼끝 위치에 맞춰 나오도록 보정했다.
- 기존 구조처럼 무기 스윽 모션이 먼저 보이고, 짧은 지연 뒤 slash / spark / hit-confirm VFX가 따라온다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy warning 7개, error 0개.
- Unity `Assets/Refresh`: 성공.
- Unity compile error count: 0.
- Play Mode 강제 대검 공격 체크:
  - `handleCloser=True`
  - 칼끝과 플레이어 거리: `1.67`
  - 손잡이와 플레이어 거리: `0.16`
  - 계산된 칼끝과 `GreatswordCrescent_Primary` 위치 차이: `0.000`
- Unity console error count: 0.

## 4. 결정한 것

대검은 앞으로 “무기 중심”이 아니라 “칼끝”을 기준으로 공격 연출을 맞춘다. 플레이어에게 가까운 쪽은 손잡이, 적/VFX 쪽은 칼끝으로 읽히게 만드는 방향이다.

## 5. 문제 또는 리스크

자동 검증은 좌표와 에러 여부를 확인했다. 실제로 45도 정도 베는 모션이 자연스럽게 보이는지, 칼끝 VFX가 너무 정확해서 오히려 딱딱해 보이지 않는지는 jaewoo 직접 플레이 리뷰가 필요하다.

## 6. GPT/Claude 인계 요약

Codex가 대검 팬텀 무기와 대검 slash VFX를 칼끝 기준으로 재정렬했다. 다음 판단은 밸런스보다 시각 판독성이다. 대검 손잡이 방향, 칼끝 VFX, 공격 대상 판독, 45도 sweep 느낌을 우선 본다.

## 7. 다음 Codex 작업

jaewoo 리뷰 후, 대검 sweep 각도/이동 거리/VFX delay가 어색하면 해당 값만 좁게 조정한다. 문제가 없다면 쌍검과 피의 칼폭풍 쪽 자연스러움 리뷰로 넘어간다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 대검이 어느 쪽이 손잡이고 어느 쪽이 칼끝인지 덜 읽히고, VFX가 무기 타격점과 완전히 맞지 않았다.
- 방향: 현실적인 장착 무기보다 마법처럼 소환되는 자동 칼 연출을 우선하되, 판독 기준은 칼끝으로 통일한다.
- 행동: 칼끝 좌표를 먼저 계산하고, 무기 중심과 slash VFX anchor를 그 좌표에서 역산했다.
- 결과: 기술 검증상 손잡이가 플레이어 쪽에 더 가깝고, VFX 위치는 계산된 칼끝과 일치한다.

# 2026-06-23-02 - 대검 손잡이 pivot 회전과 부채꼴 VFX 방향 보정

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 계속 `_dev` 검증 빌드다. 이번 작업은 앞선 칼끝 정렬을 유지하면서, 대검이 위치를 미는 느낌이 아니라 손잡이를 축으로 회전하는 느낌이 나도록 고친 작업이다.

## 2. 오늘 바뀐 것

- 대검 팬텀 무기는 이제 시작 위치에서 끝 위치로 미끄러지지 않는다.
- 손잡이 pivot을 플레이어 몸쪽에 두고, 칼날 방향을 `-28도 -> +28도`로 회전시킨다.
- 대검 부채꼴 slash VFX는 sweep의 끝 칼날 방향을 기준으로 회전한다.
- 현재 반대로 읽히던 부채꼴 방향은 대검 VFX에 한해 `180도` 보정했다.
- 칼끝 VFX 위치 정렬은 유지해서, 부채꼴이 계산된 칼끝에 붙는다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy warning 7개, error 0개.
- Unity compile error count: 0.
- Play Mode 강제 대검 공격 체크:
  - `usePivot=True`
  - 손잡이 pivot과 플레이어 거리: `0.13`
  - 대검 중심과 플레이어 거리: `0.61`
  - 칼날 sweep: `-28.0 -> 28.0`
- 직접 slash VFX 체크:
  - 끝 칼날 각도: `28.0`
  - VFX 회전값: `208.0`
  - 칼끝 위치 오차: `0.000`
- Unity console error count: 0.

## 4. 결정한 것

대검은 앞으로 “칼끝 좌표를 맞추기 위해 무기 전체를 이동”시키지 않고, “손잡이 pivot을 기준으로 칼날이 회전”하는 연출을 기본으로 한다. 부채꼴 VFX도 이 회전의 끝 칼날 방향에 맞춘다.

## 5. 문제 또는 리스크

자동 검증은 pivot 모드, 회전 각도, VFX 방향값, 칼끝 위치를 확인했다. 실제로 사람이 베는 느낌처럼 자연스러운지는 jaewoo가 직접 플레이하면서 봐야 한다.

## 6. GPT/Claude 인계 요약

대검 연출이 손잡이 pivot 기반으로 바뀌었다. 다음 리뷰에서는 대검이 “밀리는 이미지”가 아니라 “손잡이를 잡고 휘두르는 이미지”로 읽히는지, 부채꼴이 검의 움직임과 같은 방향으로 보이는지를 본다.

## 7. 다음 Codex 작업

직접 플레이에서 대검 sweep이 아직 어색하면 `GreatswordSwingHalfAngle`, 손잡이 pivot 거리, VFX `180도` 보정 여부를 좁게 조정한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 대검이 실제로 베는 느낌보다 위치만 바뀌는 느낌이 있었고, 부채꼴 VFX가 검과 반대로 읽혔다.
- 방향: 손잡이를 축으로 회전하는 공격 연출로 바꿔 물리적 판독성을 높인다.
- 행동: `V1WeaponPhantomSweep`에 handle-pivot 모드를 추가하고, 대검 VFX 회전을 sweep 끝 칼날 방향 기준으로 보정했다.
- 결과: 기술 검증상 대검 팬텀은 pivot 회전으로 움직이고, 부채꼴 VFX는 칼끝 위치와 회전값이 맞는다.

# 2026-06-23-03 - 대검 90도 베기와 화려한 VFX 보강

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 계속 `_dev` 검증 빌드다. 이번 작업은 대검이 너무 얌전하게 보인다는 피드백에 맞춰, 회전각과 베기 VFX 크기를 키운 체감 보강이다.

## 2. 오늘 바뀐 것

- 대검 손잡이 pivot sweep을 `-28도 -> +28도`에서 `-45도 -> +45도`로 늘렸다.
- 결과적으로 대검 베기 각도는 총 `90도`가 됐다.
- 대검 wide crescent PNG 스케일 보정값을 `0.150`에서 `0.175`로 키웠다.
- 대검 AoE crescent scale/lifetime을 `1.24 / 0.42`에서 `1.65 / 0.50`으로 키웠다.
- 대검 Primary crescent scale/lifetime을 `1.02 / 0.34`에서 `1.38 / 0.42`로 키웠다.
- Shock, cut point, assist crescent도 같이 조금 키워서 타격감이 더 화끈하게 보이도록 했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy warning 7개, error 0개.
- Unity compile error count: 0.
- Play Mode 강제 대검 공격 체크:
  - `usePivot=True`
  - 칼날 sweep: `-45.0 -> 45.0`
  - 총 sweep: `90.0`
  - AoE scale: `1.65`
  - Primary scale: `1.38`
- 직접 slash VFX 체크:
  - 끝 칼날 각도: `45.0`
  - VFX 회전값: `225.0`
  - 생성 bounds: `(4.28, 4.28)`
  - 칼끝 위치 오차: `0.000`
- Unity console error count: 0.

## 4. 결정한 것

대검 기본 공격은 더 이상 절제된 작은 참격이 아니라, 한 번 휘두르면 확실히 화면에 남는 큰 베기로 간다. 대신 칼끝 정렬과 손잡이 pivot 기준은 유지한다.

## 5. 문제 또는 리스크

VFX가 커졌기 때문에 화려함은 올라갔지만, 적이나 플레이어를 가릴 위험도 생겼다. jaewoo 직접 플레이에서 “화끈하다”와 “너무 덮는다” 사이를 확인해야 한다.

## 6. GPT/Claude 인계 요약

대검은 90도 pivot sweep과 더 큰 crescent VFX로 보강됐다. 다음 리뷰는 대검이 충분히 화려한지, 타격 대상이 계속 읽히는지, Blood Blade Storm과 비교했을 때 기본 공격이 너무 과해지지 않았는지를 본다.

## 7. 다음 Codex 작업

직접 플레이에서 아직 밋밋하면 VFX alpha/lifetime을 더 올리고, 너무 덮으면 scale만 약간 낮춘다. sweep 각도는 우선 90도를 기준값으로 본다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 대검 베기 연출이 정확해졌지만 화끈하고 화려한 맛이 부족했다.
- 방향: 대검은 느린 무기인 만큼 기본 공격도 넓고 강하게 읽히게 만든다.
- 행동: pivot sweep을 90도로 키우고, 대검 crescent 계열 VFX scale/lifetime을 올렸다.
- 결과: 기술 검증상 대검은 총 90도로 회전하고, Primary slash VFX bounds가 `(4.28, 4.28)`까지 커졌다.

# 2026-06-23-04 - 대검 VFX 타이밍과 휘두른 범위 맞춤

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 계속 `_dev` 검증 빌드다. 이번 작업은 대검 90도 베기 연출을 실제로 캡처해서 보며, VFX가 너무 빨리 덮는 느낌과 휘두른 범위 대비 위치가 애매한 부분을 다듬은 작업이다.

## 2. 오늘 바뀐 것

- 대검 slash VFX delay를 `0.18s`에서 `0.22s`로 늦췄다.
- 대검 sweep은 `0.28s`라서, VFX는 이제 휘두르기 약 `78.6%` 지점부터 나온다.
- 대검 팬텀 무기 lifetime을 `0.42s`로 늘렸다.
- 대검 slash 최소 lifetime을 `0.62s`로 늘렸다.
- 대검 AoE / Primary / Assist VFX 위치를 모두 칼끝 끝점에 몰아두지 않고, 90도 칼끝 호 위에 나눠 배치했다.
  - AoE crescent: 호의 `58%` 지점.
  - Primary crescent: 호의 `78%` 지점.
  - Assist crescent: 호의 `72%` 지점.
  - Shock / cut point: 최종 칼끝.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy warning 7개, error 0개.
- Unity compile error count: 0.
- Unity inline Game View capture 성공:
  - 대검을 약 `85%` 휘두른 지점에 고정했다.
  - 긴 lifetime VFX를 함께 띄워 칼과 VFX 위치 관계를 눈으로 확인했다.
- Runtime 값 확인:
  - slash delay: `0.22s`
  - sweep duration: `0.28s`
  - slash 등장 지점: 약 `78.6%`
  - min slash lifetime: `0.62s`
  - AoE scale/lifetime: `1.65 / 0.62`
  - Primary scale/lifetime: `1.38 / 0.52`
- Unity console error count: 0.

## 4. 결정한 것

대검은 “무기 모션이 먼저 읽히고, VFX가 뒤따라 강하게 남는” 구조로 간다. VFX는 최종 칼끝 하나에만 붙이지 않고, 휘두른 90도 호를 따라 역할별로 나눠 배치한다.

## 5. 문제 또는 리스크

캡처로 위치 관계는 확인했지만, 실제 반복 전투에서 적이 여러 마리일 때 VFX가 너무 오래 남거나 다음 공격과 겹칠 가능성은 직접 플레이로 봐야 한다.

## 6. GPT/Claude 인계 요약

대검 VFX는 이제 sweep 후반부에 나오고 더 오래 남는다. AoE/Primary/Assist는 90도 tip arc 위의 서로 다른 위치를 쓴다. 다음 판단은 “무겁고 화려한데 빠르게 덮지 않는가”다.

## 7. 다음 Codex 작업

쌍검도 같은 원칙을 적용하되, 대검처럼 큰 부채꼴이 아니라 짧은 교차 베기 두 개를 약간 엇갈리게 보여주는 방향으로 별도 pass를 한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 대검 VFX가 칼 모션과 동시에 덮이면서 빠르게 느껴질 수 있었고, VFX 위치가 휘두른 범위 전체와 정교하게 맞지 않았다.
- 방향: 무기 모션을 먼저 읽히게 하고, VFX는 sweep 후반부와 칼끝 호 위에 배치한다.
- 행동: VFX delay/lifetime을 늘리고, VFX별 배치 지점을 90도 tip arc에 나눴다.
- 결과: 기술 검증상 VFX는 sweep의 약 `78.6%` 지점부터 나오며, 캡처로 칼과 VFX의 위치 관계를 확인했다.

# 2026-06-23-05 - 대검 베기 VFX 타이밍 소폭 당김

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 계속 `_dev` 검증 빌드다. 이번 작업은 대검 VFX가 살짝 늦게 느껴진다는 피드백에 맞춘 미세 타이밍 보정이다.

## 2. 오늘 바뀐 것

- 대검 slash VFX delay를 `0.22s`에서 `0.20s`로 당겼다.
- 대검 sweep은 `0.28s`로 유지했다.
- VFX는 이제 휘두르기 약 `71.4%` 지점부터 나온다.
- 이전 pass의 긴 lifetime과 90도 tip arc 위치 분산은 유지했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy warning 7개, error 0개.
- Unity compile error count: 0.
- Runtime 값 확인:
  - slash delay: `0.20s`
  - sweep duration: `0.28s`
  - slash 등장 지점: 약 `71.4%`
- Unity console error count: 0.

## 4. 결정한 것

대검 VFX는 검이 거의 다 지나간 뒤가 아니라, 후반부에 들어가며 바로 터지는 쪽으로 잡는다. 현재 기준값은 `0.20s`다.

## 5. 문제 또는 리스크

너무 빨라지면 다시 무기 모션을 덮을 수 있다. 이번 조정은 `0.18s`와 `0.22s` 사이의 중간값이다.

## 6. GPT/Claude 인계 요약

대검 slash VFX delay가 `0.20s`로 당겨졌다. 다음 리뷰에서는 “살짝 느림”이 해소됐는지, 동시에 너무 빨라진 느낌은 없는지를 본다.

## 7. 다음 Codex 작업

대검이 통과되면 쌍검도 같은 원칙으로 짧은 교차 베기와 엇박 VFX를 다듬는다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 대검 VFX가 살짝 늦게 따라오는 느낌이 있었다.
- 방향: 검 후반부에 더 즉각적으로 VFX가 붙게 한다.
- 행동: slash delay를 `0.22s`에서 `0.20s`로 당겼다.
- 결과: 기술 검증상 VFX는 sweep의 약 `71.4%` 지점부터 나온다.

# 2026-06-23-06 - 쌍검, 피의 칼폭풍, 첫 120초 흐름 보강

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 계속 `_dev` 검증 빌드다. 이번 작업은 다음 후보였던 세 가지, 즉 쌍검 연출 정교화, 피의 칼폭풍 payoff 재점검, 첫 120초 진행감 리뷰를 한 번에 반영한 패스다.

## 2. 오늘 바뀐 것

- 쌍검은 첫 베기, cut flash, 두 번째 베기가 한 번에 겹치지 않도록 VFX 타이밍을 엇박으로 나눴다.
  - 첫 crescent: `0.045s`
  - cut flash: `0.067s`
  - 두 번째 crescent: `0.085s`
- 쌍검 crescent, flash, spark의 scale/lifetime을 조금 키워 무기 종류가 더 읽히게 했다.
- 피의 칼폭풍은 “굶주린 칼무리와 이펙트만 다른 느낌”을 줄이기 위해 opening, pulse, burst를 모두 강화했다.
  - opening ring, burst ring, 회전 blade 수, hitstop, shake, pressure, heal을 상향했다.
  - heavy/fast 상태 모두 반복 타격과 마무리 폭발의 존재감을 키웠다.
- 첫 120초는 더 빨리 전투가 살아나도록 spawn 간격, enemy cap, 초반 XP 배율을 조정했다.
  - 0-35초 spawn interval: `0.52s` -> `0.46s`
  - 35-80초 spawn interval: `0.58s` -> `0.52s`
  - 80-120초 spawn profile: `0.50s / pack 3` -> `0.46s / pack 4`
  - 0-120초 enemy cap: `28` -> `32`
  - 0-120초 XP multiplier: `2.15x`

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy warning 7개, error 0개.
- Unity compile error count: 0.
- Unity runtime 확인:
  - 쌍검 delay: `A=0.045`, `flash=0.067`, `B=0.085`, `assist=0.045`
  - 초반 spawn profile: `interval=0.46`, `pack=2`
  - `GrantXp(1)`: `xp=2/5`
- 피의 칼폭풍 smoke 확인:
  - storm ready: true
  - storm objects: 187
  - burst objects: 45
  - blade objects: 187
  - ring objects: 1
  - kills: 21
- Unity console error count: 0.

## 4. 결정한 것

쌍검도 대검과 같은 큰 원칙을 따른다. 무기가 먼저 베고, VFX가 그 궤적을 읽히게 따라붙는다. 대신 쌍검은 대검처럼 큰 부채꼴 한 방이 아니라 짧은 교차 베기 두 번과 flash 엇박으로 차이를 둔다.

피의 칼폭풍은 이제 기본 잔향이 아니라 궁극급 보상으로 읽혀야 한다. 단순히 칼무리보다 많은 이펙트가 아니라, 화면 압박, 히트스톱, 흔들림, 회복, 폭발 타이밍까지 같이 강하게 묶는다.

## 5. 문제 또는 리스크

피의 칼폭풍 smoke에서 생성 오브젝트가 187개까지 올라간다. 지금은 궁극 payoff를 우선한 값이지만, 직접 플레이에서 화면이 너무 지저분하거나 프레임 부담이 느껴지면 blade 수나 lifetime을 줄이는 쪽으로 조정해야 한다.

첫 120초도 이전보다 빨라졌다. 체감상 초반이 더 재밌어질 수 있지만, 무기/VFX 리뷰를 방해할 만큼 적이 많아지면 spawn cap이나 pack size를 다시 내린다.

## 6. GPT/Claude 인계 요약

이번 패스는 “쌍검도 무기와 VFX가 분리되어 읽히는가”, “피의 칼폭풍이 굶주린 칼무리보다 확실히 궁극답게 느껴지는가”, “첫 120초가 비어 보이지 않는가”를 한 번에 조정했다. 다음 리뷰는 두 무기를 각각 첫 120초 이상 플레이하면서 가장 약한 축 하나를 골라 ITERATE 여부를 판단하면 된다.

## 7. 다음 Codex 작업

직접 플레이 피드백 기준으로 다음 중 하나를 고른다.

- 쌍검이 약하면 교차 베기 각도/간격/VFX 크기를 추가 조정한다.
- 피의 칼폭풍이 과하면 오브젝트 수와 lifetime을 줄이고 hitstop/shake만 남긴다.
- 첫 120초가 과밀하면 spawn profile을 80초 이후만 완화한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 쌍검은 읽힘이 약했고, 피의 칼폭풍은 궁극 보상감이 부족했으며, 초반 진행은 조금 비어 보일 위험이 있었다.
- 방향: 무기별 공격 실루엣을 다르게 만들고, 궁극은 화면/타격/회복까지 한 번에 강하게 느끼게 한다.
- 행동: 쌍검 VFX 엇박, 칼폭풍 opening/pulse/burst 강화, 초반 spawn/XP 템포 상향을 적용했다.
- 결과: 빌드와 Unity runtime smoke가 통과했고, 다음 직접 플레이 리뷰 대상으로 묶을 수 있는 상태가 됐다.

# 2026-06-23-07 - 유틸 기억 VFX, 배경, 이동감 보강

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 계속 `_dev` 검증 빌드다. 이번 작업은 게임 재미가 어느 정도 확인된 뒤, 대검 VFX 타이밍과 보이지 않던 유틸 기억/VFX, 배경, 이동감을 한 번에 정리한 패스다.

## 2. 오늘 바뀐 것

- 대검 slash VFX delay를 `0.20s`에서 `0.18s`로 당겼다.
- 처형, 추적, 파문, 정지, 잿빛, 낙인 기억/잔향 VFX의 크기, alpha, 지속시간을 올렸다.
- `멈춘 1초`는 플레이어 주변 원이 아니라 가장 가까운 적 무리 중심에 정지 초점과 시계바늘 VFX가 생기게 바꿨다.
- 디버그 패널에 `Mem A`, `Mem B`, `Echo A`, `Echo B`, `Ult 3`, `VFX` 버튼을 추가했다.
- 배경 1차로 어두운 backdrop, 경계 띠, 기억 균열, 외곽 표식을 런타임 생성한다.
- 플레이어 걷기는 acceleration/deceleration, frame cadence, bob, tilt를 낮춰 덜 튀게 했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy warning 7개, error 0개.
- Unity compile error count: 0.
- Unity Play Mode utility smoke:
  - `greatDelay=0.18`
  - `sweep=0.28`
  - `activeMemories=3`
  - `bgDecor=30`
  - `utilityVfx=36`
  - `enemies=14`
- Unity Play Mode echo/ultimate smoke:
  - `echoCount=6`
  - `previewUlt=6`
  - `clockHands=21`
- Unity console error count: 0.

## 4. 결정한 것

다른 기억들도 이미 파일과 로직은 있었지만, 체감상 안 보이면 없는 것과 같다. 그래서 이번 패스는 “새 시스템 추가”가 아니라, 기존 6개 기억/잔향/3궁극을 실제 리뷰 가능한 밝기와 크기로 끌어올리는 방향으로 잡았다.

## 5. 문제 또는 리스크

이번 패스는 일부러 잘 보이게 만든 값이다. 직접 플레이에서 화면이 지저분하면 scale/lifetime보다 spawn frequency와 alpha부터 줄이는 게 좋다. 배경도 지금은 VFX 판독용 1차 무대라, 예쁜 최종 배경은 아직 아니다.

## 6. GPT/Claude 인계 요약

대검 VFX는 `0.18s`까지 당겼고, 유틸 기억은 디버그 패널로 즉시 확인 가능하다. 다음 리뷰는 `Mem A/B`, `Echo A/B`, `Ult 3`, `VFX` 버튼으로 각 효과를 먼저 본 뒤, 실제 120초 플레이에서 이 효과들이 과하지 않은지 판단하면 된다.

## 7. 다음 Codex 작업

직접 리뷰 결과에 따라 하나만 고른다.

- 유틸 VFX가 과하면 alpha/lifetime/frequency를 줄인다.
- 배경이 방해되면 균열/외곽 표식 alpha를 낮춘다.
- 걷기가 아직 어색하면 sprite frame 속도와 facing 전환을 더 조정한다.
- 전반이 통과되면 적/보스 전용 sprite 제작으로 넘어간다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 게임 재미는 올라왔지만, 유틸 기억과 멈춘 1초가 보이지 않고 배경이 너무 비어 있었다.
- 방향: 시스템을 더 늘리기보다 이미 있는 기억/잔향을 리뷰 가능한 시각 언어로 끌어올린다.
- 행동: VFX 크기/지속시간/위치, 디버그 버튼, 배경 레이어, 이동 애니메이션을 보강했다.
- 결과: Unity runtime smoke가 통과했고, 이제 6개 유틸 기억과 3개 비혈반 궁극을 즉시 확인할 수 있다.

# 2026-06-23-08 - 처형섬광과 멈춘 1초 판독 보정

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 `_dev` 검증 빌드다. 이번 작업은 직접 피드백에서 나온 두 가지, 처형섬광이 너무 작다는 점과 멈춘 1초가 무엇인지 잘 안 보인다는 점을 바로 보정한 패스다.

## 2. 오늘 바뀐 것

- 처형섬광 active VFX 폭을 `1.30`에서 `1.95`로 키웠다.
- 처형섬광 지속시간을 `0.24s`에서 `0.38s`로 늘렸다.
- 처형섬광에 세로/가로/대각 균열선과 밝은 core를 추가했다.
- 처형 잔향도 폭을 `1.08`에서 `1.48`로 키우고 같은 균열 언어를 쓰게 했다.
- 멈춘 1초는 이제 시계 장판처럼 보이게 바꿨다.
  - clock face
  - outer/inner ring
  - 12개 눈금
  - 긴 바늘/짧은 바늘
  - 중앙 core
- 정지 잔향과 정지 추적 궁극/미리보기에도 같은 시계 장판 언어를 적용했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy warning 7개, error 0개.
- Unity compile error count: 0.
- Unity Play Mode smoke:
  - `executionCracks=16`
  - `executionVfx=24`
  - `clockFaces=5`
  - `clockTicks=60`
  - `clockHands=15`
  - `stoppedVfx=79`
- Unity console error count: 0.

## 4. 결정한 것

멈춘 1초는 작은 파란 이펙트가 아니라 바닥에 깔리는 시계 장판으로 읽히는 쪽이 맞다. 처형섬광은 조건부 처형 기억이므로 작은 hit spark처럼 보이면 안 되고, 짧지만 크게 찢어지는 균열 폭발로 간다.

## 5. 문제 또는 리스크

시계 장판이 너무 잘 보이면 적과 무기 VFX를 덮을 수 있다. 직접 리뷰에서 과하면 눈금 alpha와 lifetime부터 낮춘다.

## 6. GPT/Claude 인계 요약

처형섬광은 큰 십자/대각 균열 폭발로, 멈춘 1초는 clock-field 장판으로 보정됐다. 다음 리뷰는 `Mem A` 또는 `VFX` 디버그 버튼으로 먼저 확인하면 된다.

## 7. 다음 Codex 작업

직접 리뷰 기준으로 처형섬광 크기와 시계 장판 alpha/lifetime을 미세 조정한다. 둘 다 통과하면 다른 유틸 기억 중 가장 약한 것 하나를 같은 방식으로 보강한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 처형섬광은 너무 작고, 멈춘 1초는 무엇이 발동했는지 알기 어려웠다.
- 방향: 처형은 큰 균열 폭발, 정지는 바닥 시계 장판으로 시각 언어를 명확히 한다.
- 행동: 처형 크기/지속시간/균열선을 키우고, 정지 장판에 링/눈금/바늘/core를 추가했다.
- 결과: Unity runtime smoke에서 처형 균열과 정지 장판 오브젝트가 정상 생성됐다.

# 2026-06-23-09 - 멈춘 1초 금색 시계 장판 강화

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 계속 `_dev` 검증 빌드다. 이번 작업은 추적자의 맹세와 멈춘 1초의 색/역할이 겹쳐 보이는 문제를 분리하고, 멈춘 1초를 노란 시계 장판으로 더 시원하게 읽히게 만든 패스다.

## 2. 오늘 바뀐 것

- 추적자의 맹세는 현재 초록/연두 계열 투사체 VFX로 유지했다.
- 멈춘 1초는 파란 정지 느낌에서 금색/노란색 시간 장판 느낌으로 바꿨다.
- 멈춘 1초 active freeze는 최대 `1.0s`까지 적용되게 했다.
- 멈춘 1초 active VFX는 `1.50s` 동안 유지되게 해서 정지 중에도 장판이 보이게 했다.
- 시계 장판의 face, ring, 12 tick, hand, core를 더 크고 밝게 조정했다.
- 회전하는 pulse ring을 추가해서 단순 원형 장판보다 시간장이 살아 있는 느낌을 더했다.
- 정지 잔향과 정지 추적 궁극 미리보기/발동도 같은 금색 시계 장판 언어를 쓰게 맞췄다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy warning 7개, error 0개.
- Unity compile error count: 0.
- Unity Play Mode smoke:
  - `clockFaces=2`
  - `totalClockFaces=3`
  - `clockTicks=24`
  - `totalClockTicks=36`
  - `clockPulses=2`
  - `clockHands=6`
  - `goldFaces=2`
  - `frozenNear1s=5`
- Unity console error count: 0.

## 4. 결정한 것

추적자의 맹세는 사냥/추적 투사체라 초록/연두 계열로 남기고, 멈춘 1초는 시간 정지 기억이므로 금색 시계 장판으로 확실히 분리한다. 보는 재미는 텍스트보다 VFX로 먼저 전달하는 쪽을 우선한다.

## 5. 문제 또는 리스크

금색 장판이 너무 밝으면 적, 무기 베기, 피의 칼폭풍을 덮을 수 있다. 직접 플레이에서 과하면 alpha나 tick 두께부터 낮춘다.

## 6. GPT/Claude 인계 요약

멈춘 1초는 이제 `1.0s` 정지와 `1.50s` 금색 시계 장판으로 읽히게 보강됐다. 다음 리뷰는 `Mem A` 또는 `VFX` 디버그 버튼으로 확인하고, 장판이 충분히 뽕맛 있는지와 전투 가독성을 덮지 않는지를 같이 판단하면 된다.

## 7. 다음 Codex 작업

직접 리뷰 후 멈춘 1초가 아직 약하면 pulse/ring 크기와 시계 바늘 대비를 더 올린다. 반대로 과하면 alpha/lifetime을 줄인다. 그다음 추적자의 맹세 초록 투사체가 기억 정체성으로 충분한지 확인한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 추적자의 맹세와 멈춘 1초의 색감/역할이 헷갈리고, 멈춘 1초가 정지 시간 동안 충분히 보이지 않았다.
- 방향: 시간 정지는 금색 시계 장판으로 강하게 읽히게 하고, 추적 투사체와 색 언어를 분리한다.
- 행동: 멈춘 1초 색, 지속시간, 정지시간, 링/눈금/바늘/core/pulse를 강화했다.
- 결과: Unity smoke에서 금색 clock face, tick, hand, pulse, 1초 근처 freeze가 정상 확인됐다.

# 2026-06-23-10 - 추적자의 맹세 밸류 보강

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 계속 `_dev` 검증 빌드다. 이번 작업은 추적자의 맹세가 기본공격보다 약하게 느껴진다는 직접 피드백을 받아, 기억 선택지로서의 전투 가치를 끌어올린 밸런스 패스다.

## 2. 오늘 바뀐 것

- 추적자의 맹세 active가 한 발짜리 약한 자동탄에서 다중 유도탄 volley로 바뀌었다.
- 레벨 1은 2발, 레벨 3은 3발, 레벨 5는 4발을 발사한다.
- 쿨다운은 `max(0.48, 1.35 - level*0.10)`에서 `max(0.62, 1.25 - level*0.11)`로 조정했다.
- 투사체 속도는 `7.5 + level*0.55`에서 `9.4 + level*0.85`로 올렸다.
- 투사체 피해는 `9 + level*3.2`에서 `13 + level*4.8`로 올렸다.
- 타깃 락온 VFX와 피격 지점의 짧은 락온 폭발을 추가했다.
- 추적 잔향 발동 확률과 피해를 올렸고, +5에서는 잔향 투사체가 2발까지 나간다.
- 유도탄은 목표가 죽으면 가장 가까운 살아있는 적에게 재추적한다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy warning 7개, error 0개.
- Unity compile error count: 0.
- Unity Play Mode entry: `isPlaying=true`.
- Unity console error count: 0.

## 4. 결정한 것

추적자의 맹세는 기본공격과 같은 근접 DPS 경쟁자가 아니라, 원거리 자동 추적과 다중 락온 폭발로 가치를 만든다. 선택했을 때 최소한 “멀리 있는 적을 알아서 지워준다”는 체감이 있어야 한다.

## 5. 문제 또는 리스크

투사체 수와 폭발이 동시에 올라갔기 때문에, 직접 플레이에서 너무 강하면 피해보다 폭발 반경이나 발사 수를 먼저 줄인다. 피의 칼폭풍보다 더 큰 payoff로 느껴지면 안 된다.

## 6. GPT/Claude 인계 요약

추적자의 맹세는 이제 레벨에 따라 2/3/4발 다중 유도탄과 락온 폭발을 가진다. 다음 리뷰에서는 `Mem A` 또는 `VFX` 버튼으로 추적자의 맹세를 먼저 확인하고, 기본공격보다 확실히 선택 가치가 있는지 본다.

## 7. 다음 Codex 작업

직접 리뷰에서 여전히 약하면 발사 수는 유지하고 피해/폭발 반경을 올린다. 너무 강하면 폭발 추가 피해나 잔향 발동 확률부터 낮춘다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 추적자의 맹세가 기본공격보다 약해서 기억 선택지로 설득력이 없었다.
- 방향: 단발 피해가 아니라 다중 유도와 락온 폭발로 고유 가치를 만든다.
- 행동: 발사 수, 속도, 피해, 잔향 확률/피해, 재추적, 피격 폭발을 보강했다.
- 결과: 기술 검증상 빌드/Unity 컴파일/Play Mode 진입이 정상이며, 직접 플레이 밸런스 리뷰 대상이 됐다.

# 2026-06-23-11 - 파문/정지 잔류 VFX와 잔향 판독 보강

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 계속 `_dev` 검증 빌드다. 이번 작업은 파쇄의 파문과 멈춘 1초가 너무 빨리 사라져 보이고, 잔향 VFX가 없는 것처럼 느껴진다는 피드백을 받은 가독성 보강 패스다.

## 2. 오늘 바뀐 것

- 파쇄의 파문 active VFX를 짧은 링에서 잔류 파문장으로 바꿨다.
- 파쇄의 파문 active는 `1.05s` 동안 유지된다.
- 파문 잔향도 같은 파문장 언어를 쓰며 `0.90s` 동안 유지된다.
- 파문장에는 main ring, outer/inner hold ring, 방사형 fracture spoke를 추가했다.
- 멈춘 1초 active 금색 시계 장판은 `1.50s`에서 `1.75s`로 늘렸다.
- 정지 잔향은 더 큰 장판으로 `1.25s` 유지되게 했다.
- 정지 추적 궁극도 금색 시계 장판 언어와 지속시간을 맞췄다.
- 처형/추적/잿빛/낙인 잔향 VFX도 더 길고 밝고 크게 조정했다.
- 첫 보스 `180s`는 이번에는 유지했다. 먼저 3분 구간이 VFX/잔향 가독성 때문에 루즈하게 느껴지는지 확인하기 위함이다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy warning 7개, error 0개.
- Unity compile error count: 0.
- Unity Play Mode entry: `isPlaying=true`.
- Unity console error count: 0.

## 4. 결정한 것

파쇄와 정지는 즉발 타격만 보여주는 기억이 아니라, 발동한 공간이 잠깐 남아야 읽힌다. 잔향은 active보다 작아도 “방금 잔향이 터졌다”는 흔적이 남아야 한다.

## 5. 문제 또는 리스크

잔류 시간이 늘어나면 화면이 지저분해질 수 있다. 직접 플레이에서 과하면 alpha를 낮추고, 부족하면 lifetime보다 중심부 대비를 먼저 올린다.

## 6. GPT/Claude 인계 요약

파쇄의 파문/파문 잔향은 잔류 파문장으로, 멈춘 1초/정지 잔향은 더 오래 남는 금색 시계 장판으로 보강됐다. 다음 리뷰는 `Mem B`, `Echo B`, `VFX` 버튼으로 잔향이 실제로 보이는지 먼저 확인하면 된다.

## 7. 다음 Codex 작업

직접 리뷰에서 잔향이 아직 약하면 각 잔향별 중심 core나 outline을 추가한다. 첫 180초가 여전히 루즈하면 그때 첫 보스 시간을 앞당기거나 중간 이벤트를 넣는다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 파쇄/정지 계열이 빨리 사라지고 잔향 VFX가 없는 것처럼 느껴졌다.
- 방향: 기억 발동 공간이 잠깐 남고, 잔향은 작은 보조 장판/폭발로 읽히게 한다.
- 행동: 파문장 helper, 정지 장판 지속시간, 잔향 VFX 수명/크기/alpha를 보강했다.
- 결과: 기술 검증상 빌드/Unity 컴파일/Play Mode 진입이 정상이며, 직접 플레이 가독성 리뷰 대상이 됐다.

# 2026-06-23-12 - 적/보스 판독 스프라이트 삽입

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 계속 `_dev` 검증 빌드다. 이번 작업은 전투가 재미있어지기 시작한 뒤, 적 역할이 도형 fallback으로만 보이는 문제를 줄이기 위한 첫 적/보스 스프라이트 삽입 패스다.

## 2. 오늘 바뀐 것

- 떠도는 눈 스프라이트 시트 `sheet_enemy_eye_4dir.png`를 추가했다.
- 쪼개진 자 스프라이트 시트 `sheet_enemy_splitter_4dir.png`를 추가했다.
- 공허 사제 스프라이트 시트 `sheet_enemy_voidpriest_4dir.png`를 추가했다.
- 첫 보스 문지기 스프라이트 `spr_boss_gatekeeper_01.png`를 추가했다.
- 같은 이미지의 chroma source PNG를 `_dev/Art/Source`에 남겼다.
- `scripts/generate_enemy_boss_sprites.ps1`를 추가해서 같은 판독용 스프라이트를 재생성할 수 있게 했다.
- `V1GameManager.EnemySprite()`가 새 이미지를 먼저 로드하고, 실패할 때만 기존 도형 fallback을 쓰게 바꿨다.

## 3. 테스트 결과와 근거

- 생성된 눈/분열자/사제/문지기 PNG를 로컬 이미지 뷰어로 확인했다.
- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy warning 7개, error 0개.
- Unity `Assets/Refresh`: success.
- Unity compile error count: 0.
- Unity Play Mode entry: `isPlaying=true`.
- Unity console error count: 0.

## 4. 결정한 것

이번 스프라이트는 최종 아트가 아니라 역할 판독용이다. 지금 목표는 예쁜 완성본보다, 전투 중 원거리 눈/분열자/지원 사제/보스 문지기가 서로 헷갈리지 않는 것이다.

## 5. 문제 또는 리스크

사제와 문지기는 아직 단순한 문양형 실루엣이다. 직접 플레이에서 너무 추상적으로 보이면 다음 패스에서 디테일보다 외곽 실루엣과 색 대비를 먼저 보강한다.

## 6. GPT/Claude 인계 요약

적 3종과 첫 보스가 procedural fallback에서 전용 PNG 기반 판독 이미지로 넘어갔다. 다음 리뷰는 VFX가 많은 상황에서도 역할이 구분되는지 확인하면 된다.

## 7. 다음 Codex 작업

직접 플레이에서 적 역할이 잘 보이면 보스 등장 연출/첫 180초 pacing을 본다. 잘 안 보이면 적 스프라이트 색 대비, 크기, outline을 먼저 조정한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 전투 재미가 올라왔지만 적 역할이 도형 fallback이라 게임처럼 보이는 감각이 부족했다.
- 방향: 최종 아트 전에 역할 판독용 스프라이트를 먼저 넣는다.
- 행동: 적 3종 4방향 시트와 보스 단일 PNG를 만들고 런타임 로드 경로를 연결했다.
- 결과: 기술 검증상 새 PNG import, 빌드, Unity Play Mode 진입이 정상이다.
