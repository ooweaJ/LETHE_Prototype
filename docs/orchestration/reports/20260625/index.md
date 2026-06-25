# 2026-06-25-01 - 대검 VFX 보장 수정

## 1. 현재 빌드 상태

대검 데이터와 VFX 프로필은 남아 있었지만, 실제 플레이에서는 대검 베기 VFX가 사라진 것처럼 보일 수 있었다. 특히 프로필 기반 slash entry가 스킵되거나 다른 시각 요소에 묻히면, 플레이어 입장에서는 무기 잔상만 보이고 베기 이펙트가 없는 것처럼 느껴진다.

## 2. 오늘 바뀐 것

- 대검 타격 성공 시 무조건 뜨는 보장형 cleave VFX를 추가했다.
- `SpawnGreatswordGuaranteedSlash()`를 새로 만들었다.
- 대검이 적을 맞추면 기존 VFX 프로필과 별개로 다음이 항상 생성된다:
  - 큰 대검 cleave arc 1장
  - 밝은 보조 cleave arc 1장
  - 칼끝 방향에 맞춘 cut line 1개
- 위치는 현재 대검 스윙의 손잡이/칼끝 계산을 그대로 사용해서, 무기 움직임과 베기 VFX가 따로 놀지 않게 했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, warning 0개, error 0개.
- Unity `Assets/Refresh`: 성공.
- Unity compile error count: `0`.
- Unity Play Mode 진입: `isPlaying=true` 확인.
- Unity console에는 MCP 서버 정보 로그만 있었고 게임 런타임 오류/경고는 없었다.

## 4. 결정한 것

대검은 공격 속도가 느리고 한 방이 커서, VFX가 조건부로 사라져 보이면 무기 정체성이 바로 무너진다. 그래서 프로필 VFX와 별개로 최소 한 번은 확실한 베기 효과를 보장하는 구조로 바꿨다.

## 5. 문제 또는 리스크

기존 대검 프로필 VFX도 계속 살아 있기 때문에, 직접 플레이에서는 cleave가 너무 많이 겹치는지 봐야 한다. 안 보이는 문제를 막는 대신 과밀 위험이 조금 생겼다.

## 6. GPT/Claude 인계 요약

Codex가 대검 타격 시 보장형 cleave fallback을 추가했다. 다음 판단 기준은 "대검 VFX가 다시 보이는가"와 "기존 프로필 VFX와 겹쳐 과하지 않은가"다.

## 7. 다음 Codex 작업

1. jaewoo가 대검으로 직접 플레이한다.
2. 대검 베기 VFX가 다시 보이는지 확인한다.
3. 너무 겹치면 기존 프로필 VFX 또는 보장형 cleave의 alpha/scale/lifetime만 줄인다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 대검 VFX가 사라진 것처럼 보여 무기 타격감이 무너졌다.
- 방향: 대검은 조건부 VFX보다 항상 보이는 최소 베기 효과를 보장한다.
- 행동: 대검 타격 성공 시 cleave arc와 cut line을 무조건 생성하는 fallback을 추가했다.
- 결과: 기술 검증은 통과했고, 직접 플레이로 시각 밀도만 확인하면 된다.
