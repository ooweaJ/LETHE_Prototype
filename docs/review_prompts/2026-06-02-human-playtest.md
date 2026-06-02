# LETHE human playtest 기반 다음 방향 결정 - 2026-06-02

## 프로젝트 목표

- HTML 프로토타입으로 핵심 재미와 가능성을 검증한다.
- 충분히 가능성이 있으면 Unity 구현 단계로 넘어갈 근거를 만든다.
- 사람 테스트 결과가 부족하면 HTML v0.6에서 가장 약한 축만 보완한다.

## 사람 테스트 요약

# LETHE Human Playtest Summary - 2026-06-02
## Current Verdict
- Human test sample is not complete yet: 0/5 minimum logs collected.
- Use this summary as evidence for Claude/GPT planning after human sessions.
## Input
- Log directory: `playtest_logs`
- Valid logs: `0`
- Invalid logs: `0`
## Aggregate Metrics
- Average Q1 regret/sadness: `n/a`
- Average Q2 fairness/acceptance: `n/a`
- High regret count (Q1>=3): `0/0`
- Low fairness risk count (Q2<=1): `0/0`
- Memory recall filled: `0/0`
- Unknown prediction count: `0/0`
- Average pre-boss level-ups: `n/a`
## Forgotten Memory Distribution
- No data.
## Growth Choice Distribution
- No data.
## Session Rows
- No human JSON logs found yet.
## Human Notes Excerpt
# Playtest Notes

사람 플레이테스트와 관찰 내용을 기록한다. AI 알파테스트 결과와 섞지 않는다.

테스트 진행 기준은 `docs/HUMAN_PLAYTEST_GUIDE.md`를 따른다.

## Session Template

```text
Date:
Tester ID:
Session ID:
Build: LETHE HTML alpha v0.5
Weapon:
Memories:
JSON log file:

Early loop:
- 첫 2-3분 재미: fun / neutral / boring / unclear
- 적 몰림 체감:
- 레벨업 선택 횟수 체감:
- 3택 선택 태도: 고민함 / 아무거나 누름 / 이해 못함
- 선택한 성장과 이유:

Before forgetting:
- 가장 의존한다고 느낀 기억:
- 가장 잃고 싶지 않은 기억:
- 레테가 가져갈 것 같은 기억:
- 예측 이유:

After forgetting:
- 실제로 잃은 기억:
- Q1 아쉬움 0~4:
- Q2 납득 0~4:
- Q3 기억 이름 회상:
- 즉시 반응:
- 잔향 이해 여부:
- 28% 전투력 딥 체감: 약함 / 적당함 / 과함 / 못 느낌
- 계속 플레이 의향:
- Unity 가능성 발화:

Observations:
- reaction label: regret / irritation / neutral / unclear
- restart intent: yes / maybe / no
- 좋았던 점:
- 짜증/혼란:
- 다음 수정 후보:
```
## Gate Interpretation
- No human logs yet.
- Run 5-8 sessions using `docs/HUMAN_PLAYTEST_GUIDE.md`.
- Put downloaded JSON logs in `playtest_logs/`, then rerun `npm run playtest:summary`.
## Next Planning Question
- Should LETHE proceed toward Unity transition groundwork, iterate HTML v0.6, or fix the core loop first?
- Which observed weakness should Codex address next, if any?

## 판단 질문

1. 현재 사람 테스트 결과는 Unity 전환 근거로 충분한가?
2. 부족하다면 HTML v0.6에서 가장 작게 고칠 작업 1-3개는 무엇인가?
3. 초반 재미, 성장 선택, 기억/망각, 잔향/회복 중 가장 약한 축은 무엇인가?
4. 아직 만들지 말아야 할 것은 무엇인가?

## 답변 형식

## 결론

- `UNITY_GROUNDWORK`, `ITERATE_HTML_V06`, `FIX_CORE`, `NEED_MORE_HUMAN_DATA` 중 하나

## 이유

- 핵심 판단 근거 3-5개

## 앞으로 해야 할 일

- [ ] Codex 작업 1
- [ ] Codex 작업 2
- [ ] Codex 작업 3

## 아직 만들지 말 것

- 제외 범위
