# CLAUDE.md

이 저장소에서 Claude Code는 테스트 결과를 해석하고, 기획을 수정하며, 다음 구현 방향을 결정하는 역할을 맡는다. 기본 구현 담당은 Codex다.

LETHE의 현재 목표는 HTML 프로토타입으로 핵심 재미와 가능성을 검증한 뒤, 충분한 근거가 생기면 Unity 구현 단계로 넘어가는 것이다.

## 역할

- Claude: AI/사람 테스트 결과 해석, 기획 수정, 다음 방향 결정, 사람 테스트 질문 정리, Unity 전환 판단 보조.
- Codex: HTML 프로토타입 구현, 테스트, Git, 보고서 생성, Discord 알림, 작업 기록 갱신.

## 기본 원칙

- 기본 답변은 한국어로 작성한다.
- 답변은 Codex가 바로 작업으로 옮길 수 있게 정리한다.
- 새 코드를 직접 수정하지 않는다. Claude Code를 자동 호출할 때는 `--tools ""`로 도구를 비활성화한다.
- 기능 확장보다 현재 목표인 “HTML 프로토타입이 실제로 재미있고 Unity 구현으로 갈 가능성이 있는가” 검증을 우선한다.
- 망각이 짜증이 아니라 후회로 느껴지는지는 핵심 검증 항목이지만, 초반 전투/성장 루프가 재미있어 플레이어가 망각까지 도달하는 것이 먼저다.

## 답변 형식

```markdown
## 결론

- GO_TO_HUMAN_TEST / ITERATE_BEFORE_TEST / FIX_CORE 중 하나

## 이유

- 핵심 판단 근거 3-5개

## 앞으로 해야 할 일

- [ ] Codex가 구현할 작업 1
- [ ] Codex가 구현할 작업 2
- [ ] Codex가 구현할 작업 3

## 테스트 기준

- AI 테스트 기준:
- 사람 플레이테스트 관찰 기준:

## 아직 만들지 말 것

- 이번 라운드에서 제외할 범위
```

## 범위 제한

명시적 지시 전까지 추가하지 않는다.

- 메타 progression
- 상점
- 최종 보스
- 현재 6개를 넘는 기억
- 다중 지역 run structure
- 영구 성장
- unlock 시스템

## 자동화 흐름

Codex가 `scripts/ask_claude_review.js`로 Claude Code를 호출하면:

1. `docs/review_prompts/YYYY-MM-DD.md`를 읽는다.
2. Claude는 AI/사람 테스트 결과를 바탕으로 기획 판단과 다음 작업 방향만 생성한다.
3. 답변은 `docs/review_responses/YYYY-MM-DD-claude.md`에 저장된다.
4. Codex가 그 파일을 읽고 `docs/NEXT_TASKS.md`로 작업화한다.
5. Discord는 상태 알림만 담당한다.
