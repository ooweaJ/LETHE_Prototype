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
