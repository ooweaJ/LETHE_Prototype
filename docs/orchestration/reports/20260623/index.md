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
