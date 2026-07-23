# 2026-07-23-01 - 처형/파문/잿빛/망각 잔향 리디자인

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에서 처형, 파문, 잿빛, 망각 일반 잔향을 기억 VFX 재사용 구조에서 분리했다. 새 잔향 전용 스프라이트 8개를 만들고, 쌍검/대검 판정과 VFX를 각각 다시 연결했다.

## 2. 오늘 바뀐 것

- 처형:
  - 쌍검은 붉은 X 절단과 검은 판결 조각으로 죄목을 난도질하는 느낌.
  - 대검은 대상 주변에 처형문이 내려오는 범위 판정.
- 파문:
  - 쌍검은 적 사이를 튀는 균열/파편 연쇄.
  - 대검은 전방 부채꼴이 아니라 대상 위치의 지면 붕괴 범위.
- 잿빛:
  - 쌍검은 깨진 방패에서 바로 반격선이 나가는 패링 잔향.
  - 대검은 플레이어 앞 성화 방벽이 밀고 나가는 전방 압박 판정.
- 망각:
  - 쌍검은 보라색 삭제 절흔과 void 파편.
  - 대검은 중심으로 무너지는 void 크레이터.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 기존 deprecation warning 7개, 오류 0개.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 통과, 기존 deprecation warning 7개, 오류 0개.
- Unity import settings: `configured=9`.
- Unity compilation errors: `0`.
- `DebugRunEchoMatrix(DualBlades)`: 8개 잔향 +5, `dualReworkObjects=64`.
- `DebugRunEchoMatrix(Greatsword)`: 8개 잔향 +5, `greatReworkObjects=32`.
- `DebugRunDenseDualBladePerfMatrix()`: `hits=18`, `echoesSuppressed=15`, `transient=46`, `ms=18.30`.

## 4. 결정한 것

기억은 상징이고 잔향은 사건이다. 처형/파문/잿빛/망각 일반 잔향은 기억 HQ 이미지를 주연으로 쓰지 않고, 무기별 전용 액션 스프라이트와 판정을 가진다.

## 5. 문제 또는 리스크

디버그 매트릭스는 모든 잔향이 동시에 켜져 여전히 과밀하다. 실제 평상시 전투에서는 새 스프라이트의 크기, 알파, 지속시간을 직접 플레이 기준으로 조정해야 한다.

## 6. GPT/Claude 인계 요약

Execution, Shatter, Ashen, and Oblivion normal Echoes have been redesigned away from memory-VFX reuse. Eight Echo-only sprites were generated/imported and wired into weapon-specific hit logic. Great Shatter is area collapse, Great Execution is an execution gate area, Great Ashen is a forward holy wall, Great Oblivion is void collapse. Dual versions stamp quick chain/parry/erasure sprites across targets.

## 7. 다음 Codex 작업

jaewoo가 직접 플레이로 4개 계열을 확인하고 `keep`, `tune`, `redesign`을 표시한다. 이후 크기/알파/지속시간/발동 빈도를 조정한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 기억 VFX를 잔향에 재사용하면 보상이 복붙처럼 보여 재미가 떨어진다.
- 방향: 기억은 상징, 잔향은 전투 사건으로 분리한다.
- 행동: 8개 잔향 전용 이미지를 만들고 무기별 판정과 연결했다.
- 결과: 자동 검증에서 새 잔향 오브젝트 생성과 Dense 성능 예산을 확인했다.
