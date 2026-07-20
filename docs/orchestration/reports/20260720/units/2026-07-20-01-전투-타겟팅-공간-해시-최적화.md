# 2026-07-20-01 - 전투 타겟팅 공간 해시 최적화

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에 전투 타겟팅 최적화가 들어갔다. Unity 에디터가 꺼져 있어 Play Mode QA는 아직 못 했지만, 런타임/에디터 C# 빌드는 통과했다.

## 2. 오늘 바뀐 것

- `V1GameManager`에 살아있는 적 전용 spatial hash grid를 추가했다.
- 무기 타겟 선택, 무기 히트 수집, 잔향 radius/cone/chain 타겟 헬퍼, Void Priest 힐 타겟, 적 분리력, live enemy count를 새 쿼리 구조로 옮겼다.
- 적 생성, 처치, 디버그 클리어, Gatekeeper 제거, null cleanup 때 공간 캐시를 무효화하도록 했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 기존 legacy warning 7개, error 0개.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 통과, warning 0개, error 0개.
- `npm run report`: 통과, 20260720 HTML 보고서 생성.
- `npm.cmd run report:check`: 통과.
- Unity MCP는 에디터 인스턴스를 찾지 못해 Play Mode QA를 실행하지 못했다.

## 4. 결정한 것

- `enemies` 리스트는 계속 source of truth로 둔다.
- 공간 해시는 매 프레임 살아있는 적 위치를 재구성하는 보조 인덱스로만 쓴다.
- 최적화는 게임 감각을 바꾸는 밸런스 패치가 아니라, 같은 타겟팅 규칙을 더 적은 검색 비용으로 실행하는 방향으로 제한한다.

## 5. 문제 또는 리스크

- Unity Play Mode에서 아직 Dense Dual Blades / Echo Matrix QA를 확인하지 못했다.
- 공간 셀 후보 순서가 일부 동률 상황에서 이전 LINQ 정렬과 완전히 같지 않을 수 있으므로 직접 플레이에서 타겟 선택 감각을 확인해야 한다.

## 6. GPT/Claude 인계 요약

전투 성능 최적화를 위해 living enemy spatial hash grid를 추가했다. 핵심 타겟 검색은 주변 셀 후보만 검사하고, 최종 판정은 기존과 같은 거리/각도/TouchRadius 조건을 사용한다. 다음 리뷰는 성능 수치보다 타겟팅 회귀 여부를 먼저 봐야 한다.

## 7. 다음 Codex 작업

- Unity 에디터를 켠 뒤 `Dense Dual Blades Perf Matrix`, `Echo Matrix Dual Blades`, `Echo Matrix Greatsword`를 실행한다.
- QA가 통과하면 직접 플레이로 밀집 전투의 타겟 선택, Kalmuri chain, Void Priest heal, enemy separation을 확인한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 적과 VFX가 많아지는 서바이버 전투에서 전체 적 리스트를 반복 정렬/검색하면 성능과 GC가 불안정해질 수 있다.
- 방향: 기획/전투 감각은 유지하고, 내부 쿼리만 공간 분할과 재사용 버퍼로 바꾼다.
- 행동: spatial hash grid와 Top-K식 삽입 정렬 헬퍼를 추가하고 핵심 타겟 경로를 교체했다.
- 결과: C# 빌드는 통과했으며, Unity Play Mode 성능 QA만 남았다.
