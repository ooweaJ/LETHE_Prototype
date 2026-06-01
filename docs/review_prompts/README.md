# 기획 검토 프롬프트

GPT나 Claude에게 기획 검토를 맡길 때 이 폴더에 프롬프트를 둔다.

권장 파일명:

```text
docs/review_prompts/YYYY-MM-DD.md
```

## 작성 원칙

- 기본은 한국어로 쓴다.
- Codex가 바로 실행할 수 있게 답변 형식을 지정한다.
- GPT는 우선순위, 구현 범위, 테스트 기준을 묻는 데 쓴다.
- Claude는 감정선, 문구, 플레이어 경험을 묻는 데 쓴다.
- 둘 다에게 무조건 보낼 필요는 없다.
- Claude Code가 로그인/키체인 문제로 막히면 `npm run review:openai`로 같은 프롬프트를 OpenAI API에 보낸다.

## 자동 호출

Claude Code:

```bash
npm run review:claude:dry
npm run review:claude
```

OpenAI API:

```bash
npm run review:openai:dry
npm run review:openai
```

OpenAI API 호출에는 `OPENAI_API_KEY`가 필요하다. 로컬 `.env`에 넣어도 되며 `.env`는 Git에 포함하지 않는다.

## 권장 답변 형식

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
