# LETHE 개발 보고서 - 2026-06-10

새 망각 모델을 HTML에 이식하고 jaewoo 1인 체감 테스트용 빌드를 갱신했다.

# 2026-06-10-01 - 새 망각 모델 HTML 게이트

## 1. 현재 빌드 상태

HTML v0.12는 Unity 전환 직전 상태에서 한 번 더 HTML 체감 게이트로 돌아왔다. 이번 라운드는 신규 콘텐츠나 Unity 셋업 없이, 새 망각 모델만 이식한 빌드로 jaewoo 1인 체감 테스트를 하기 위한 준비이며, 패키지는 `dist\lethe-v0.12-playtest`로 갱신됐다.

## 2. 오늘 바뀐 것

- 망각 대상이 의존도 가중 랜덤에서 최고 레벨 활성 기억으로 바뀌었다.
- 최고 레벨 동률은 일반 플레이에서 플레이어가 선택하고, QA/디버그는 최근 강화 기억을 우선 선택한다.
- 기억과 잔향 레벨 상한을 `+5`로 고정했다.
- 잔향 초과분은 버리지 않고 즉시 과부하 폭발과 `ultimateGauge` 로그로 전환한다.
- 잔향이 있는 기억을 다시 얻으면 `1 + floor(echoLevel / 2)`만큼 공명해 높은 레벨로 돌아온다.
- HUD에 다음 망각 후보, 잔향 레벨, `+5` 각성, 과부하/궁극 게이지를 표시했다.
- 즉시 망각, 잔향 `+5`, 궁극 표식 디버그 버튼을 추가했다.
- 새 망각 모델 적용 후 death rate가 올라가 HP 단일 레버를 `190 -> 210`으로 조정했다.

## 3. 테스트 결과와 근거

- `node --check src/game.js`: 통과.
- `node --check alpha_test/src/simulator.js`: 통과.
- `npm run qa:balance`: `GO_BALANCE_BASELINE`, first boss clear `100%`, full clear `60%`, death `40%`, first boss TTK median `20.19s`.
- `npm run balance:loop`: `GO_BALANCE_BASELINE`, first boss clear `80%`, full clear `60%`, death `40%`, first boss TTK median `23.91s`.
- `npm run playtest:package:dry`: 통과.
- `npm run playtest:package`: 통과, `dist\lethe-v0.12-playtest` 갱신.

이번 게이트에서 AI 테스트는 회귀 확인용이고, 최종 판단은 jaewoo 1인 체감 테스트다.

## 4. 결정한 것

`ITERATE_BEFORE_TEST`: Unity 셋업 전에 HTML에 새 망각 모델만 이식하고 1인 체감 테스트를 진행한다.

## 5. 문제 또는 리스크

- 최고 레벨 망각은 트레이드오프를 선명하게 만들지만, 실제 체감이 "아쉽다"가 아니라 "짜증난다"로 흐를 수 있다.
- HP `210`은 회귀 밴드를 회복했지만, 사람 체감에서는 결손 손실이 약해졌는지 확인해야 한다.
- 공명 재획득이 너무 강하면 망각 손실이 약해질 수 있다.

## 6. GPT/Claude 인계 요약

새 모델의 판단 기준은 자동 지표가 아니라 감정 루프다. 테스트에서 봐야 할 것은 최고 레벨 기억 상실의 아쉬움, 잔향으로 바뀐 전투 체감, 재획득 공명의 설렘, 그리고 플레이어가 강화 선택을 망각 위험과 연결해 생각하는지다.

## 7. 다음 Codex 작업

jaewoo 1인 체감 테스트를 진행하고, 관찰 내용을 `docs/orchestration/evidence/` 또는 `playtest_logs/`에 남긴 뒤 보고서와 다음 작업을 갱신한다.

## 8. 포트폴리오 메모

- 문제: 기존 망각은 의존도 기반 확률이라 플레이어가 성장과 상실의 관계를 읽기 어려웠다.
- 방향: "키운 기억이 다음 망각 후보가 된다"는 규칙으로 감정적 트레이드오프를 전면화했다.
- 행동: 기존 8개 기억/2무기 범위 안에서 망각, 잔향, 재획득 규칙만 바꿨다.
- 결과: Unity로 넘어가기 전에 핵심 감정 루프를 HTML에서 직접 체감 검증할 수 있는 빌드가 준비됐다.
