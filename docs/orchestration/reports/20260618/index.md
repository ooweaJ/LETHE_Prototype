# 2026-06-18-01 - Dev_Prototype_v1 런타임 안정성 30분 점검

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 계속 Unity 메인 프로토타입입니다. 이번 작업은 새 기능 추가가 아니라, 최근 `Collection was modified` 예외 수정 이후 남은 런타임 위험을 줄이는 안정화 패스였습니다.

## 2. 오늘 바뀐 것

- `V1GameManager`의 적 리스트 조회 코드를 추가 점검했습니다.
- 굶주린 칼무리 타겟 선택에서 null 적을 건너뛰도록 보강했습니다.
- 적 cap 계산에서도 null 적을 건너뛰도록 보강했습니다.

## 3. 테스트 결과와 근거

- 작업 초반 Unity MCP Play Mode smoke에서는 콘솔 런타임 예외가 없었습니다.
- 최종 `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과.
- 결과: 0 warning / 0 error.
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과. 최초 병렬 실행 때는 report 생성과 race가 나서 실패했고, 단독 재실행으로 통과했습니다.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패.
- 패치 이후 Unity MCP 재확인은 `Transport closed`로 막혔습니다. Unity Editor 자체는 7890 포트에서 실행 중이었습니다.

## 4. 결정한 것

- 이번 단위에서는 게임 감각 수정을 넣지 않고, 리스트 순회 안정성과 검증 기록만 남겼습니다.
- Unity MCP가 끊긴 상태에서는 Play Mode 결과를 추측하지 않고, 빌드 검증과 막힘 기록까지만 완료로 봅니다.

## 5. 문제 또는 리스크

- AnkleBreaker MCP tool transport가 닫혀 post-patch Play Mode 재확인을 못 했습니다.
- 다음 세션 첫 작업은 MCP 재연결 후 `Dev_Prototype_v1` Play Mode smoke를 다시 실행하는 것입니다.

## 6. GPT/Claude 인계 요약

Blood Bloom 예외 수정 이후 `V1GameManager`의 남은 적 리스트 순회 위험을 점검했고, null guard 2곳을 추가했습니다. C# 빌드는 통과했지만 MCP transport가 끊겨 Unity post-patch smoke는 다음 세션으로 넘깁니다.

## 7. 다음 Codex 작업

- AnkleBreaker MCP 재연결 확인.
- Unity compile error count 0 확인.
- `Dev_Prototype_v1` Play Mode 10~20초 smoke.
- 콘솔이 깨끗하면 hit feedback / weapon identity / M2 pacing 개선으로 복귀.

## 8. 포트폴리오 메모

- 문제: 프로토타입 확장 중 전투 루프에서 컬렉션 변경 예외가 발생할 수 있었다.
- 방향: 게임 감각 작업 전에 런타임 안정성을 먼저 확보한다.
- 행동: 적 리스트 순회 패턴을 점검하고 null guard를 추가했다.
- 결과: C# 빌드 기준으로 0 error 상태를 유지했고, 남은 검증 막힘은 MCP transport 문제로 명확히 분리했다.

# 2026-06-18-02 - 굶주린 칼무리 판독성 1차 재작업

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 계속 `_dev` 평가 빌드입니다. 이번 작업은 `굶주린 칼무리`가 이름처럼 칼무리로 읽히지 않는다는 플레이 피드백을 받아, 활성 기억과 칼무리 잔향의 시각 언어를 칼날 군집 쪽으로 보정한 작은 패스입니다.

## 2. 오늘 바뀐 것

- 활성 `굶주린 칼무리`의 주변 VFX를 2~6개의 희미한 짧은 궤도에서 6~14개의 이중 궤도 칼날 군집으로 바꿨습니다.
- 칼무리 데미지 틱마다 적 위치에 작은 칼날 다발과 물어뜯기 halo가 생기게 했습니다.
- 칼무리 잔향 후속타에도 기존 링/참격 위에 칼날 barrage를 추가했습니다.
- 칼무리 칼날 sprite 로딩을 `SpawnKalmuriBlade`, `KalmuriBladeSprite` helper로 모았습니다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과.
- 결과: legacy v0/debug deprecated API warning 7개, error 0개.
- Unity MCP 확인:
  - active scene: `Dev_Prototype_v1`.
  - Unity compile error count: 0.
  - 짧은 Play Mode 진입 성공.
  - Unity console error log count: 0.
  - Play Mode stop 성공.
- 제한: 시작 무기 선택 overlay 때문에 자동 확인은 런타임 예외 확인 수준입니다. 실제 칼무리 판독성은 jaewoo가 무기 선택 후 직접 봐야 합니다.

## 4. 결정한 것

- 다른 기억들의 전용 sprite VFX가 모두 존재한다고 보지 않습니다.
- 현재 `_dev/Data`에는 8기억, 8잔향, 4궁극 definition이 있지만, 전용 PNG VFX는 칼무리/혈반/피의 칼폭풍에 집중되어 있습니다.
- 나머지 기억/잔향/궁극은 일단 절차형 링, 다이아, 초승달, 투사체 형태로 보이게 해둔 상태입니다.

## 5. 문제 또는 리스크

- 칼무리 VFX 밀도가 올라갔기 때문에 실제 전투에서 너무 바쁘거나 캐릭터/적을 가릴 수 있습니다.
- 전용 sprite art가 없는 기억들은 여전히 최종 게임 아트라기보다 판독용 임시 VFX입니다.
- 칼무리 자체도 아직 “최종 아트”가 아니라 기존 칼날 PNG를 더 명확하게 쓰는 1차 보정입니다.

## 6. GPT/Claude 인계 요약

사용자 피드백은 `굶주린 칼무리`가 칼무리처럼 보이지 않는다는 것이었다. 전용 VFX 재고를 확인한 결과 모든 기억에 dedicated sprite VFX가 있는 상태는 아니고, 일부는 procedural placeholder다. Codex는 칼무리 활성 기억을 이중 궤도 칼날 군집과 적 위치 bite blades로 바꾸고, 칼무리 잔향 후속타에 칼날 barrage를 추가했다.

## 7. 다음 Codex 작업

- jaewoo가 `Dev_Prototype_v1`에서 무기 선택 후 칼무리 활성 기억과 칼무리 잔향을 직접 확인한다.
- 칼무리가 여전히 약하면 다음 패스는 새 칼무리 전용 sprite/VFX asset 제작으로 간다.
- 다른 6개 기억/잔향은 “절차형 placeholder 유지”인지 “전용 sprite 제작 착수”인지 우선순위를 정한다.

## 8. 포트폴리오 메모

- 문제: 이름과 화면 효과가 맞지 않아 핵심 기억의 정체성이 전달되지 않았다.
- 방향: 수치보다 먼저 전투 중 읽히는 형태 언어를 고친다.
- 행동: 칼무리의 오라/틱/잔향 후속타를 모두 칼날 군집 표현으로 재구성했다.
- 결과: 기술 검증은 통과했고, 이제 사람 플레이에서 “칼무리처럼 보이는가”를 다시 판단할 수 있다.

# 2026-06-18-03 - 스테이지/밸런스 셸과 오브젝트 풀링 1차 적용

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 비주얼 피드백이 아니라 게임 구조 검증 단계로 전환했습니다. 이번 작업은 모든 기억이 최소 동작하는지 확인하고, 일반 런이 문서의 스테이지/밸런스 규칙을 따르도록 맞춘 1차 패스입니다.

## 2. 오늘 바뀐 것

- 8개 활성 기억, 8개 잔향, 4개 궁극이 모두 런타임 최소 동작을 갖고 있음을 코드 기준으로 확인했습니다.
- 일반 런을 600초 기준으로 바꿨습니다.
- 문지기 스케줄을 180초 / 340초 / 490초 / 600초로 맞췄습니다.
- 첫 문지기 HP를 문서 기준 2050으로 맞췄습니다.
- 결손 생존 시간을 54초로 맞췄습니다.
- 적 압박 페이즈의 spawn interval, pack size, cap을 문서 표 기준으로 1차 연결했습니다.
- 리뷰용 자동 기억/+5 주입은 fast debug 모드에서만 작동하게 분리했습니다.
- 레벨업 선택지에 문서 기준 6개 런스탯을 모두 넣었습니다.
- 절차형 VFX, floating text, damage number, XP orb를 내부 오브젝트 풀로 재사용하게 했습니다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과.
- 결과: 0 warning / 0 error.
- Unity MCP:
  - `Assets/Refresh`: 성공.
  - Unity compile error count: 0.
  - 짧은 Play Mode 진입 성공.
  - Unity console error log count: 0.
  - Play Mode stop 성공.

## 4. 결정한 것

- VFX/art 품질 평가는 뒤로 미룹니다.
- 현재 기준의 “동작한다”는 전용 아트가 완성됐다는 뜻이 아니라, 런타임 효과/데미지/상태 변화가 있다는 뜻입니다.
- 일반 런은 더 이상 리뷰 보정으로 기억과 잔향을 자동 완성하지 않습니다. 실제 XP와 카드 선택으로 성장해야 합니다.
- fast debug는 계속 압축 검증용으로 남깁니다.

## 5. 문제 또는 리스크

- 적, 추적 투사체, 적 탄환은 아직 create/destroy를 사용합니다.
- 2050 HP 첫 보스와 180초 타이밍은 문서 기준이지만, 현재 Unity 감각에서 TTK가 맞는지는 사람 플레이 또는 압축 스모크가 필요합니다.
- 모든 기억은 최소 동작하지만, 각 기억의 밸런스와 재미는 아직 최종 확정이 아닙니다.

## 6. GPT/Claude 인계 요약

사용자는 VFX 피드백을 뒤로 미루고 게임 완성 방향을 요구했다. Codex는 `Dev_Prototype_v1`의 8기억/8잔향/4궁극 최소 동작을 확인하고, 일반 런을 문서의 600초/문지기/결손/압박 페이즈 규칙에 맞게 연결했다. 리뷰 자동 주입은 fast debug 전용으로 분리했고, transient VFX/text/damage/XP 오브젝트는 내부 풀로 재사용하게 바꿨다.

## 7. 다음 Codex 작업

- 일반 런 또는 압축 스모크로 첫 문지기 TTK와 망각 도달성을 확인한다.
- 적/투사체/적 탄환 풀링을 추가한다.
- 8기억 각각의 수치와 카드 설명이 실제 효과와 맞는지 조정한다.
- 보스 처치 후 망각/결손/보충이 일반 플레이에서 끊기지 않는지 확인한다.

## 8. 포트폴리오 메모

- 문제: 프로토타입이 리뷰 압축 보정에 기대고 있어 실제 런 구조와 문서 밸런스가 분리되어 있었다.
- 방향: 비주얼보다 먼저 런 타임라인, 성장, 망각, 스폰 압박을 source of truth에 맞춘다.
- 행동: 600초 런, 문지기 스케줄, 결손 생존, 스폰 페이즈, 런스탯, 오브젝트 풀링을 연결했다.
- 결과: 기술 검증은 통과했고, 이제 사람 플레이에서 밸런스와 루프 감정을 검증할 수 있는 상태에 가까워졌다.
