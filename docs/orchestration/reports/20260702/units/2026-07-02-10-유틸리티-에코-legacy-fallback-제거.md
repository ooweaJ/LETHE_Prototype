# 2026-07-02-10 - 유틸리티 에코 legacy fallback 제거

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 `_dev` 검증 단계에 있으며, 유틸리티 에코 튜닝은 `_dev/Data/Echoes/UtilityEcho_Tuning.asset`을 1차 경로로 사용한다. 이번 작업은 새 기능 추가가 아니라, 이미 쓰이지 않는 오래된 fallback 코드를 제거한 런타임 정리 작업이다.

## 2. 오늘 바뀐 것

- `V1GameManager.TriggerUtilityEchoes` 안에 남아 있던 오래된 inline fallback 분기를 제거했다.
- `enemy == null`일 때 즉시 반환하도록 안전 가드를 명확히 했다.
- 현재 경로는 `TriggerShatterEcho`, `TriggerExecutionEcho`, `TriggerHunterEcho`, `TriggerStoppedEcho`, `TriggerAshenEcho`, `TriggerOblivionEcho`만 호출한다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 기존 경고 7개, 오류 0개.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 통과, 기존 경고 7개, 오류 0개.
- Unity compile error count: `0`.
- Echo Matrix Dual Blades: `[V1QA] PASS`, `total=240`.
- Echo Matrix Greatsword: `[V1QA] PASS`, `total=221`.
- Utility Ultimate Matrix Dual Blades: `[V1QA] PASS`, `fracture=19`, `stasis=9`, `ashen=34`.
- Utility Ultimate Matrix Greatsword: `[V1QA] PASS`, `fracture=8`, `stasis=22`, `ashen=16`.
- Kalmuri Perf Matrix: `[V1QA] PASS`, `totalKalmuri=374`.

## 4. 결정한 것

유틸리티 에코는 이제 asset table과 개별 handler 경로가 기준이다. 예전처럼 `TriggerUtilityEchoes` 안에서 모든 에코를 직접 다시 구현하던 fallback은 유지 가치보다 혼동과 null 위험이 커서 제거했다.

## 5. 문제 또는 리스크

MCP polling이 일부 QA 실행에서 `fetch failed`를 냈지만, Unity 콘솔에는 각 QA의 PASS 로그가 정상적으로 남았다. 기능 리스크는 낮고, 남은 리스크는 반복되는 궁극/VFX 상수들이 아직 manager 안에 많다는 점이다.

## 6. GPT/Claude 인계 요약

검증 포인트는 "legacy fallback 제거 후 에코/궁극 발동량이 유지되는가"였다. 쌍검/대검 에코 매트릭스, 쌍검/대검 유틸 궁극 매트릭스, 칼무리 perf가 모두 PASS라서 동작 보존은 확인됐다.

## 7. 다음 Codex 작업

남은 echo/ultimate cleanup에서 반복 색상, 반경, 타이밍, 데미지 상수를 compact spec으로 더 모은다. 그 다음에는 패시브 기억 체감 튜닝으로 넘어간다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 과거 fallback 코드가 새 handler 경로와 함께 남아 있어 런타임 코드가 길고 위험해졌다.
- 방향: 동작이 검증된 현재 경로만 남겨 유지보수 표면을 줄인다.
- 행동: legacy branch를 제거하고 null guard와 handler dispatch만 남겼다.
- 결과: 주요 QA가 모두 PASS했고, 다음 데이터화/튜닝 작업을 진행하기 쉬운 구조가 됐다.
