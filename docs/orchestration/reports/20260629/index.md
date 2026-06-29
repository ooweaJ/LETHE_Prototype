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
# 2026-06-29-02 - 보스 템포 Unity 적용

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에 stepped boss / XP / DPS 곡선을 실제 적용했다. 이번 패치는 새 콘텐츠 추가가 아니라 보스 타이밍, 몹 압력, 체력, XP, 결손 흐름만 조정한 좁은 밸런스 패치다.

## 2. 오늘 바뀐 것

- 문지기 스케줄을 `150 / 360 / 660 / 1020s`로 변경했다.
- 문지기 HP를 `1200 / 2250 / 4050 / 8650`으로 변경했다.
- 런 하드캡을 `1200s`로 변경했다.
- 시작 필요 XP를 `8`로 변경했다.
- 시간대별 스폰 간격, 팩 크기, 적 캡, 적 평균 체력, XP 보상을 코드에 반영했다.
- normal run에서 결손 생존 54초 구간을 제거했다.
- 망각과 잔향 획득은 유지하고, Space 이후 바로 기억 보충/공명으로 넘어가게 했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 기존 v0/debug 경고 7개, 오류 0개.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: 통과, 경고 0개, 오류 0개.
- `node scripts/balance_curve_v1.js`: 통과.
- `node scripts/verify_unity_stepped_balance.js`: 통과.
- Unity MCP compile error count: `0`.
- Unity MCP console error count: `0`.

제한: 기존 V1QA 메뉴 실행은 MCP `fetch failed` 큐 오류로 완료하지 못했다. Unity 에디터는 `Dev_Prototype_v1`에 열려 있고 컴파일/콘솔 오류는 0이다.

## 4. 결정한 것

시뮬레이션상 첫 문지기는 레벨 6 전후, 목표 TTK 18초로 나온다. 후반 최종 문지기는 궁극 활성 이후 목표 TTK 48초라 의도한 상승 곡선에 맞는다. QA 결과만 보면 지금 단계에서 추가 수치 조정은 하지 않는 쪽이 맞다.

## 5. 문제 또는 리스크

MCP 메뉴 QA가 실패했으므로, 다음에는 smoke 메뉴를 다시 돌리거나 직접 첫 6분 플레이로 첫 보스 체감, 첫 망각, 즉시 보충 흐름을 확인해야 한다.

## 6. GPT/Claude 인계 요약

Codex가 보스 템포 재설계안을 Unity 런타임에 적용했다. 핵심 후보값은 `150/360/660/1020s`, HP `1200/2250/4050/8650`, 결손 생존 제거다. 기술 빌드와 정적 검증은 통과했지만 MCP smoke 메뉴는 큐 오류로 미완료다.

## 7. 다음 Codex 작업

MCP 메뉴 실행이 안정화되면 V1QA smoke를 재시도한다. 그 다음 jaewoo가 첫 6분을 플레이해 첫 보스가 지루함을 해소했는지 판단한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 초반 5분이 평평하고 첫 보스가 늦었다.
- 방향: 보스 간격과 난이도를 시간대별 수치 곡선으로 만든다.
- 행동: Unity 런타임의 스케줄, HP, XP, 몹 압력, 결손 흐름을 패치했다.
- 결과: 첫 보스가 2분 30초 목표로 당겨졌고, 다음 리뷰는 첫 6분 체감으로 좁혀졌다.

# 2026-06-29-03 - 보스 템포 / XP / DPS 재설계안

## 1. 현재 빌드 상태

`Dev_Prototype_v1`의 20분 베타 기술 QA는 통과했지만, jaewoo 직접 리뷰에서 첫 문지기까지의 시간이 너무 길어 초반이 지루하다는 문제가 확인됐다.

## 2. 오늘 바뀐 것

- 새 계산 스크립트 `scripts/balance_curve_v1.js`를 추가했다.
- 첫 문지기 시간을 `300s`에서 후보값 `150s`로 당겼다.
- 문지기 간격을 고정 5분형에서 `150 -> 210 -> 300 -> 360s`로 점점 길어지는 구조로 재설계했다.
- 문지기 HP를 평균 보스 DPS와 목표 TTK로 역산했다.
- 결손 생존 `54s` 구간은 다음 후보에서 제거하기로 정리했다.

## 3. 테스트 결과와 근거

`node scripts/balance_curve_v1.js` 실행 결과:

- 문지기 스케줄: `150 / 360 / 660 / 1020s`
- 문지기 HP: `1200 / 2250 / 4050 / 8650`
- 목표 TTK: `18 / 26 / 36 / 48s`
- 예상 보스 시점 레벨: `6 / 8 / 11 / 14`
- 예상 보스 DPS: `68 / 86 / 112 / 180`
- 상세 근거: `docs/orchestration/evidence/2026-06-29-stepped-boss-xp-dps-plan.md`

## 4. 결정한 것

다음 구현 축은 새 콘텐츠 추가가 아니라 시간대별 몹 수, 체력, XP, 보스 타이밍, 보스 HP를 하나의 곡선으로 맞추는 것이다.

## 5. 문제 또는 리스크

이 수치는 아직 Unity 런타임에 적용되지 않은 설계안이다. 실제 체감은 첫 6분 직접 플레이로 다시 확인해야 한다.

## 6. GPT/Claude 인계 요약

첫 보스가 너무 늦다는 jaewoo 피드백을 반영해 `150 / 360 / 660 / 1020s` 문지기 곡선을 설계했다. 결손 생존은 일단 제거 후보로 두고, 망각과 잔향 획득은 유지한다.

## 7. 다음 Codex 작업

Unity `_dev` 런타임에 stepped boss curve를 적용하고, `dotnet build`, Unity QA, report/check를 다시 통과시킨다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 초반 5분이 평평해서 첫 보스 전까지 목표감이 약했다.
- 방향: 보스 간격을 단계적으로 늘리고, HP/XP/DPS를 수치로 묶는다.
- 행동: 계산 스크립트와 근거 문서를 만들었다.
- 결과: 다음 구현에 바로 넣을 수 있는 보스/몹/XP/DPS 후보표가 생겼다.
