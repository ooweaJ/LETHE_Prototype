# 2026-06-17-06 - BloodBloom 런타임 예외 QA와 수정

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에서 플레이 중 발생한 런타임 예외를 확인하고 수정했다. C# 빌드와 Unity 컴파일은 통과했고, 수정 후 짧은 Play Mode smoke에서는 같은 예외가 다시 나오지 않았다.

## 2. 오늘 바뀐 것

- Unity 콘솔에서 `InvalidOperationException: Collection was modified; enumeration operation may not execute.`를 확인했다.
- stack trace는 `V1GameManager.BloodBloom -> TriggerWeaponEchoes -> UpdateWeapon -> Update`로 이어졌다.
- 원인은 `BloodBloom`이 `enemies.Where(...)`를 직접 순회하는 동안 `DealDamage`가 적을 죽이며 `enemies` 리스트를 수정할 수 있었던 것이다.
- `enemies`를 순회하며 피해를 주는 광역/궁극/잔향 루프를 `.ToList()` snapshot 기반으로 바꿨다.
- null guard가 약한 enemy query도 함께 보강했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과.
- 결과: 기존 v0/debug deprecated API warning 7개 / error 0개.
- Unity `Assets/Refresh`: 성공.
- `unity_get_compilation_errors`: count `0`.
- Unity console clear 후 Play Mode 8초 smoke: runtime exception 없음.
- Play Mode 진입 MCP 호출은 여전히 `fetch failed`를 반환했지만, `unity_editor_state` 기준 실제로는 Play Mode에 들어간 것을 확인했다.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패.

## 4. 결정한 것

- `DealDamage`가 호출되는 enemy area loop는 직접 `enemies`를 열거하지 않고 snapshot을 사용한다.
- 이번 패치는 밸런스 변경이 아니라 런타임 안정성 보정이다.

## 5. 문제 또는 리스크

- 8초 smoke는 시작 구간 안정성 확인이다. 혈반 +5/궁극이 실제 플레이에서 길게 반복되는 상황은 추가 장시간 smoke가 필요하다.
- Play Mode MCP 호출의 `fetch failed`는 게임 예외가 아니라 MCP polling 응답 문제로 보인다.

## 6. GPT/Claude 인계 요약

혈반 각성/광역 피해 계열에서 enemy list를 순회하며 동시에 enemy 제거가 발생할 수 있던 구조를 snapshot 순회로 고쳤다. 다음 QA는 debug 상태 주입이나 장시간 Play Mode로 혈반 +5, 피의 칼폭풍, 파쇄 처형 같은 광역 효과를 강제로 반복시키는 쪽이 좋다.

## 7. 다음 Codex 작업

- 장시간 또는 강제 상태 주입 smoke로 광역/궁극 효과를 반복 검증한다.
- `DealDamage`와 enemy removal 책임을 더 명확히 분리할지 검토한다.
- 이후 VFX/타격감 QA로 돌아간다.

## 8. 포트폴리오 메모

- 문제: 새 잔향/궁극 확장 후 광역 효과가 enemy list를 수정하며 런타임 예외를 만들었다.
- 방향: 기능 추가 후 즉시 콘솔 기반 QA를 돌려 안정성 문제를 먼저 제거한다.
- 행동: stack trace를 따라 원인을 좁히고 snapshot 순회 패턴으로 수정했다.
- 결과: 같은 예외가 짧은 Play Mode smoke에서 재발하지 않았다.
