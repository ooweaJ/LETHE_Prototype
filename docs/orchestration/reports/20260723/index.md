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
# 2026-07-23-02 - 잔향 판정 가독성 후속 패치

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에서 새로 리워크한 처형, 파문, 잿빛, 망각 잔향의 피격 가독성을 보강했다. VFX 품질은 유지하고, 실제 데미지를 받은 적마다 짧은 표식과 연결선을 추가했다.

## 2. 오늘 바뀐 것

- 잿빛 기억/저장 반격:
  - 플레이어 몸 주변의 방어 발동은 유지.
  - 실제 맞은 적에게 성흔/반격 표식을 추가.
  - 플레이어 또는 방패 근원에서 적으로 이어지는 반격선을 추가.
- 잿빛 잔향:
  - 대검 성벽 VFX에서 각 피격 대상에게 광선이 연결되도록 보강.
  - 쌍검 반격 체인 대상마다 피격 표식을 추가.
- 처형/파문/망각 잔향:
  - 광역 또는 체인 판정에 맞은 적마다 처형 판결, 균열, 지워짐 표식을 추가.
- 수치 밸런스는 의도적으로 변경하지 않았다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과. 첫 시도는 Unity DLL 파일 잠금으로 실패했으나 단독 재시도에서 경고 0개, 오류 0개.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 통과. 기존 deprecation warning 7개, 오류 0개.
- Unity compilation errors: `0`.
- Play Mode `DebugRunEchoMatrix(DualBlades)`: reflection call 완료.
- Play Mode `DebugRunEchoMatrix(Greatsword)`: reflection call 완료.
- Unity console errors: `0`.

## 4. 결정한 것

이번 문제는 데미지 수치보다 판정 설명 문제로 본다. 대표 VFX 하나만 크게 띄우는 방식은 유지하지 않고, 앞으로 잔향은 `발동 근원 -> 경로 -> 피격 대상`이 최소한 한 번은 화면에 읽혀야 한다.

## 5. 문제 또는 리스크

풀링된 transient VFX는 요청한 이펙트 id가 GameObject 이름으로 그대로 남지 않아서 이름 기반 카운트 검증이 어렵다. 최종 판단은 직접 플레이에서 잿빛과 광역 잔향의 피격 경로가 보이는지로 해야 한다.

## 6. GPT/Claude 인계 요약

The new Echo sprites looked better, but hit readability was still ambiguous. Added a shared per-victim `SpawnEchoHitRead` layer so Execution, Shatter, Ashen, and Oblivion damage loops show target-local hit marks and links. Ashen memory/Echo now explicitly connects guard source to victims.

## 7. 다음 Codex 작업

jaewoo가 직접 플레이하면서 잿빛 기억/잔향을 먼저 확인한다. 특히 `몸에서 발동 -> 적에게 반격선 -> 적 피격 표식` 순서가 읽히는지 보고, 부족하면 잿빛은 판정 자체를 적 중심 반격으로 더 재설계한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 고퀄리티 VFX를 넣어도 판정 경로가 보이지 않으면 전투가 납득되지 않는다.
- 방향: 아름다운 대표 실루엣 위에 피격 가독성 레이어를 별도로 둔다.
- 행동: 네 잔향과 잿빛 기억에 target-local hit read를 추가했다.
- 결과: 빌드와 Unity 매트릭스 스모크 기준으로 런타임 오류 없이 테스트 가능한 상태가 됐다.
# 2026-07-23-03 - 잔향 판정 규칙 아이덴티티 리워크

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에서 잿빛, 망각/낙인, 처형 잔향의 판정 규칙을 다시 갈랐다. 파문은 현재처럼 지면 파괴 규칙이 VFX와 맞기 때문에 큰 방향을 유지했다.

## 2. 오늘 바뀐 것

- 잿빛:
  - 기억이 더 이상 플레이어 몸 주변 적을 주기적으로 때리지 않는다.
  - 평소에는 방패/주시선으로 보이고, 저장된 방어력이 있을 때만 반격 피해를 낸다.
  - 저장 반격은 주변 전체가 아니라 위협도가 높은 소수 대상에게 되돌아간다.
  - 대검 잔향은 넓은 콘이 아니라 성벽 차선 판정으로 바뀌었다.
- 망각/낙인:
  - 기억은 즉시 피해/즉시 전염이 아니라 `새김 -> 지연 삭제 -> 지연 전염`으로 바뀌었다.
  - 잔향도 가벼운 새김 피해 후 짧은 지연 삭제 피해가 본 피해로 들어간다.
  - +5 전염도 즉시 광역 피해가 아니라 예고/지연 중심으로 정리했다.
- 처형:
  - 대검 처형은 반경 전체 피해가 아니라 판결 대상과 제한된 저체력 목격자만 때린다.
  - 쌍검 처형도 일반 플레이에서는 저체력/판결 대상 중심으로 연쇄된다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과. Unity DLL 파일 잠금으로 첫 시도는 실패했지만 단독 재시도에서 경고 0개, 오류 0개.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 통과. 기존 deprecation warning 7개, 오류 0개.
- Unity compilation errors: `0`.
- Play Mode `DebugRunEchoMatrix(DualBlades)`: 완료.
- Play Mode `DebugRunEchoMatrix(Greatsword)`: 완료.
- Play Mode `DebugRunPassiveMemoryMatrix()`: 완료.
- Play Mode `DebugRunDenseDualBladePerfMatrix()`: 완료.
- 지연 낙인 코루틴 대기 후 Unity console errors: `0`.

## 4. 결정한 것

잔향의 개성은 색과 이미지뿐 아니라 판정 규칙에서 먼저 갈라져야 한다. 기준은 `파문 = 지면 파괴`, `잿빛 = 방어 축적/반격`, `망각 = 새김/지연 삭제`, `처형 = 저체력 판결`로 잡는다.

## 5. 문제 또는 리스크

잿빛은 이제 더 이상 몸 주변 주기 피해가 아니므로 직접 플레이에서 너무 조용하게 느껴질 수 있다. 만약 그렇다면 수치 광역을 되돌리기보다, 피격/근접 압박을 받을 때 반격 빈도와 방패 반응을 더 살리는 쪽이 맞다.

## 6. GPT/Claude 인계 요약

Ashen, Oblivion Brand, and Execution were reworked at the hit-rule level. Ashen is now guard storage/counter, Brand is inscription -> delayed erase/spread, and Execution is low-health verdict targeting. Shatter remains the reference because its ground-fracture rule already reads clearly.

## 7. 다음 Codex 작업

jaewoo가 직접 플레이하면서 세 가지 질문에 답하면 된다. 잿빛이 방어/반격으로 읽히는가, 낙인이 지연 삭제로 읽히는가, 처형이 저체력 판결로 읽히는가. 부족한 항목은 VFX가 아니라 판정 규칙부터 다시 조정한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 다른 VFX를 붙여도 같은 광역 판정이면 플레이 감각은 같게 느껴진다.
- 방향: 각 잔향의 전투 규칙을 서로 다른 플레이 언어로 분리한다.
- 행동: 잿빛/낙인/처형의 기억 및 잔향 판정 구조를 바꿨다.
- 결과: 기술 검증 기준으로 런타임 오류 없이 직접 테스트 가능한 상태가 됐다.
