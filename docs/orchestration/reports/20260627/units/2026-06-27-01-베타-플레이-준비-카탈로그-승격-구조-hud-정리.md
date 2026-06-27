# 2026-06-27-01 - 베타 플레이 준비: 카탈로그, 승격 구조, HUD 정리

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 계속 `_dev`의 메인 플레이 씬이다. 이번 작업은 실행 파일 빌드가 아니라, 에디터에서 베타처럼 플레이하기 위한 구조 정리다.

## 2. 오늘 바뀐 것

- `V1ContentCatalog`를 추가했다.
- `Assets/_dev/Data/V1_ContentCatalog.asset`에 46개 sprite reference, Pretendard font, 무기 Definition 2개를 묶었다.
- `V1_GameManager`에 catalog와 `Weapon_DualBlades`, `Weapon_Greatsword`를 연결했다.
- `Assets/Lethe` 승격 준비 폴더와 `README.md`를 만들었다.
- `Assets/Lethe/Scenes/Lethe_BetaPreview.unity` 후보 씬을 만들었다.
- HUD에 현재 잔향 요약과 짧은 목표 문장을 추가했다.
- F12 기억/잔향/VFX 테스트 UI는 일부러 유지했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy v0/debug warning 7개, error 0개.
- Unity compile error count: `0`.
- Unity scene missing references: `0`.
- Unity Play Mode 진입 성공.
- `V1GameManager.DebugSnapshot()` 정상 시작 상태 확인.
- Unity console error count after stop: `0`.

## 4. 결정한 것

빌드/배포는 아직 하지 않는다. 대신 runtime asset reference, promotion-prep folder, player-facing HUD를 먼저 정리한다.

## 5. 문제 또는 리스크

`V1GameManager`는 아직 큰 단일 클래스이고, 많은 오브젝트가 런타임 생성 기반이다. `Assets/Lethe`는 후보 구조일 뿐이며, 전체 승격 완료 상태가 아니다.

## 6. GPT/Claude 인계 요약

Codex가 베타 플레이 준비 1차로 카탈로그와 승격 후보 구조를 추가했다. 다음 판단은 HUD/objective line이 플레이 중 방해되지 않는지, 그리고 기억/잔향 VFX 수정 요청을 어떤 id부터 처리할지다.

## 7. 다음 Codex 작업

1. jaewoo가 `Dev_Prototype_v1`을 직접 플레이한다.
2. F12 테스트 UI로 기억/잔향 VFX를 확인한다.
3. 수정 요청이 온 id부터 VFX scale, alpha, lifetime, spawn position을 조정한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 프로토타입은 돌아가지만 베타 플레이용 참조/구조/UI 층이 약했다.
- 방향: 빌드가 아니라 에디터 플레이 안정화와 승격 준비부터 한다.
- 행동: catalog, `Assets/Lethe` 구조, HUD 목표 정보를 추가했다.
- 결과: missing reference 0, compile error 0, Play Mode start state 정상 확인.
