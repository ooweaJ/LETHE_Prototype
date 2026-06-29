# 2026-06-29-01 - 20분 베타 직접 리뷰 준비

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 20분 베타런 후보로 다시 기술 QA를 통과했다. 현재 다음 게이트는 자동 테스트가 아니라 jaewoo 직접 플레이 리뷰다.

## 2. 오늘 바뀐 것

- Unity MCP로 `LETHE` 인스턴스와 `Dev_Prototype_v1` 상태를 확인했다.
- `V1_GameManager`의 무기 definition 누락 참조 2개를 발견했다.
- `LETHE/_dev/Rebuild Prototype v1 Scene` 메뉴로 씬을 재생성해 누락 참조를 복구했다.
- 5개 pre-play QA 메뉴를 다시 실행했다.
- 재생성된 씬에서 `Main Camera`에 `AudioListener`가 빠져 있어 반복 log가 났고, 이를 씬과 `V1SceneBuilder`에 반영해 수정했다.
- jaewoo 직접 리뷰 기록지를 추가했다:
  - `docs/orchestration/review_prompts/2026-06-29-jaewoo-beta-run-review.md`

## 3. 테스트 결과와 근거

- Unity compile error count: `0`.
- Unity scene missing references: `0`.
- Unity asset missing references: `0`.
- Unity console error count during QA checks: `0`.
- 쌍검 QA: `[V1QA] PASS`, `elapsed=2.1`, `liveEnemies=8`.
- 대검 QA: `[V1QA] PASS`, `elapsed=2.1`, `liveEnemies=8`.
- M2 루프 QA: `[V1QA] PASS`, `HungryBlades:5`, `BloodReflection:5`, `storm=True`, `result=True`.
- VFX Matrix QA: `[V1QA] PASS`, `previewMemory=8`, `previewEcho=8`, `fracture=1`, `stasis=1`, `ashen=1`.
- 피의 칼폭풍 QA: `[V1QA] PASS`, `stormObjects=77`, `hungryEcho=5`, `bloodEcho=5`.
- AudioListener 수정 후 쌍검 smoke 재확인: `[V1QA] PASS`, console error `0`, no-audio-listener log 없음.

## 4. 결정한 것

현재 상태는 직접 플레이 리뷰로 넘긴다. 새 기능을 추가하지 않고, 플레이 결과에 따라 다음 수정 축을 하나만 고른다.

## 5. 문제 또는 리스크

MCP 메뉴 실행은 가끔 `fetch failed`를 반환하지만 Unity 안에서는 실행되는 경우가 있다. 이번에도 최종 기준은 Unity console의 `[V1QA] PASS` 로그로 삼았다.

## 6. GPT/Claude 인계 요약

Codex가 `Dev_Prototype_v1`의 pre-play QA를 2026-06-29 기준으로 다시 통과시켰다. 다음 판단은 자동 QA가 아니라 jaewoo 직접 플레이에서 20분 템포, 쌍검/대검 손맛, 기억/잔향 구분감, HUD 피로도를 확인하는 것이다.

## 7. 다음 Codex 작업

1. jaewoo가 리뷰 기록지 기준으로 직접 플레이한다.
2. 결과가 오면 `GO`, `ITERATE`, `NO-GO` 중 하나로 정리한다.
3. `ITERATE`라면 XP cadence, Gatekeeper HP, weapon route balance, reward route steering, VFX scale/timing, enemy pressure, forgetting UX, HUD readability 중 하나만 고친다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 직접 리뷰 전 기술 검증선과 리뷰 기록 양식이 필요했다.
- 방향: 자동 QA는 안정성만 확인하고, 재미 판단은 사람 리뷰로 넘긴다.
- 행동: Unity MCP QA를 재실행하고, 누락 참조를 복구하고, 리뷰 기록지를 만들었다.
- 결과: 기술 차단 요소는 사라졌고, 다음 작업은 직접 플레이 피드백 수집으로 좁혀졌다.
