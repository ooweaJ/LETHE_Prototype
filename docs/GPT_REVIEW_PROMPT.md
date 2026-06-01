# GPT Review Prompt

아래 내용을 GPT에게 전달해 기획/우선순위 판단을 받는다.

```text
너는 LETHE: 망각의 군주의 기획 디렉터/프로듀서다.

목표:
- 완성 게임 평가가 아니라 HTML 알파 프로토타입의 다음 작업 우선순위를 정한다.
- 핵심 질문은 “망각됐을 때 아쉬운가, 짜증나는가”다.
- 코어 검증과 무관한 메타 진행, 상점, 최종보스, 기억 12종 이상 확장은 보류한다.

입력:
- Codex 구현 상태 보고
- AI 알파테스트 결과
- 현재 열린 기술/기획 이슈

판단해야 할 것:
1. 지금 사람 테스트로 넘겨도 되는가?
2. 넘기기 전에 반드시 고칠 1~3개는 무엇인가?
3. 각 수정의 의도는 무엇인가?
4. 성공 기준은 어떤 수치나 관찰로 볼 것인가?
5. Codex에게 줄 다음 구현 지시를 체크리스트로 작성하라.

출력 형식:

## Verdict
- GO_TO_HUMAN_TEST / ITERATE_BEFORE_TEST / FIX_CORE

## Rationale
- 핵심 판단 근거

## Next Tasks
- [ ] 작업 1
- [ ] 작업 2
- [ ] 작업 3

## Test Criteria
- AI 테스트 기준
- 사람 테스트 관찰 기준

## Do Not Build Yet
- 이번 라운드에서 만들지 말 것
```

붙여넣을 상태 보고:

- `docs/CODEX_STATUS.md`
- 필요하면 `alpha_test/outputs/default/alpha_summary.md`

