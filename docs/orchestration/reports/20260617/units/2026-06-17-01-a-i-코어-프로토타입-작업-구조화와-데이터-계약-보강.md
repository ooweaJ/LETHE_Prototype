# 2026-06-17-01 - A-I 코어 프로토타입 작업 구조화와 데이터 계약 보강

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 계속 현재 Unity 메인 프로토타입이다. 오늘은 바로 새 콘텐츠를 덧붙이기보다, A-I 전체 작업을 이어서 만들 수 있도록 루트 문서와 ScriptableObject 데이터 계약을 먼저 정리했다.

## 2. 오늘 바뀐 것

- 새 루트 문서 입구를 만들었다.
  - `docs/PRD.md`
  - `docs/TECH.md`
  - `docs/TASK.md`
  - `docs/TEST.md`
  - `docs/CHANGELOG.md`
- `AGENTS.md`의 작업 전 읽는 순서를 새 문서 구조에 맞게 바꿨다.
- 깨져 있던 `docs/orchestration/state/NEXT_TASKS.md`를 A-I 작업 기준으로 다시 작성했다.
- `LETHE/Assets/_dev/Scripts/Core/DefinitionTypes.cs`를 확장했다.
  - 기억 효과 종류.
  - 잔향 형태 종류.
  - 적 역할.
  - encounter spawn mode.
  - 궁극 trigger mode.
  - M2 encounter wave data.
  - `UltimateEchoDefinition`.
  - `EncounterDefinition`.
  - 확장된 `MemoryDefinition`, `EchoDefinition`, `EchoSynergyDefinition`, `EnemyDefinition`, `RewardPoolDefinition`.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과.
- 결과: 7 warning / 0 error.
- warning 7개는 기존 v0/debug 코드의 `Object.FindObjectOfType<T>()` deprecation warning이다.
- 이번 세션의 도구 목록에는 AnkleBreaker Unity MCP가 노출되지 않고 UE-MCP만 노출되어 Unity Editor compile/play-mode 검증은 아직 실행하지 못했다.

## 4. 결정한 것

- A-I 작업을 하나의 EPIC으로 보고 진행한다.
- 새 기능은 가능한 한 `Definition asset -> Runtime -> VFX/Prefab -> TEST -> report` 흐름으로 만든다.
- 기존 `EchoSynergyDefinition`은 Blood Blade Storm 호환성을 위해 유지하고, 4궁극 확장용으로 `UltimateEchoDefinition`을 추가한다.

## 5. 문제 또는 리스크

- Unity MCP가 현재 도구 목록에 보이지 않아 Editor 검증은 아직 남아 있다.
- 아직 런타임 전체가 새 데이터 계약을 읽는 것은 아니다. 오늘은 A단계의 계약을 만든 것이고, B-I에서 점진적으로 실제 런타임 연결이 필요하다.
- 기존 소스 일부 한글 기본 문자열은 터미널에서 깨져 보인다. 컴파일은 통과하지만 장기적으로는 문자열/폰트/인코딩 정리가 필요하다.

## 6. GPT/Claude 인계 요약

LETHE는 이제 A-I EPIC을 기준으로 진행한다. 첫 단위로 루트 문서 구조와 데이터 계약을 만들었다. 다음 판단 지점은 B단계, 즉 쌍검/대검 기본공격 타격감과 칼무리/혈반 잔향 가독성을 먼저 끌어올리는 것이다.

## 7. 다음 Codex 작업

- B단계 공격 타격감 / 잔향 가독성 보정 시작.
- 쌍검 2연 반달, 대검 큰 반달, 피격 플래시, 데미지 숫자, 넉백, hitstop을 새 데이터 구조 기준으로 점검한다.
- 칼무리 잔향은 적 위치 후속타, 혈반 잔향은 표식 -> 회복 실 -> 피꽃 흐름으로 정리한다.

## 8. 포트폴리오 메모

- 문제: Unity 프로토타입이 기능별로 덧붙으며 구조와 작업 기준이 흐려질 위험이 있었다.
- 방향: PRD/TECH/TASK/TEST/CHANGELOG와 orchestration을 분리해 개인 AI 게임개발 규약으로 정리했다.
- 행동: A-I EPIC을 문서화하고, Unity 콘텐츠 확장을 받을 ScriptableObject 데이터 계약을 보강했다.
- 결과: 다음 작업부터는 즉흥 구현이 아니라 데이터 계약과 작업 보드를 기준으로 진행할 수 있다.
