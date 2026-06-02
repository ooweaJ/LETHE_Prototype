# 기획 검토 프롬프트

GPT나 Claude에게 기획 검토를 맡길 때 이 폴더에 프롬프트를 둔다.

권장 파일명:

```text
docs/review_prompts/YYYY-MM-DD.md
docs/review_prompts/YYYY-MM-DD-pipeline.md
```

## 작성 원칙

- 기본은 한국어로 쓴다.
- Codex가 바로 실행할 수 있게 답변 형식을 지정한다.
- GPT/Claude는 단순 리뷰어가 아니라 AI/사람 테스트 결과를 해석하고 다음 기획 방향을 정하는 파트너로 쓴다.
- 프롬프트에는 최신 테스트 결과, 사람 반응, 현재 빌드 상태, Unity 전환 판단 질문을 포함한다.
- 둘 다에게 무조건 보낼 필요는 없다.
- Claude Code가 로그인/키체인 문제로 막히면 우선 `npm run review:codex`로 Codex CLI에 같은 프롬프트를 보낸다.

## 자동 호출

Claude Code:

```bash
npm run review:claude:dry
npm run review:claude
```

Codex CLI:

```bash
npm run review:codex:dry
npm run review:codex
```

Codex CLI는 ChatGPT/Codex 로그인 세션을 사용한다. 답변은 `docs/review_responses/YYYY-MM-DD-codex.md`에 저장된다.

## 테스트 결과 기반 자동 파이프라인

최신 AI 테스트를 돌리고, 그 결과로 `docs/review_prompts/YYYY-MM-DD-pipeline.md`를 생성한 뒤, Claude 우선/Codex fallback으로 기획 답변을 저장한다.

```bash
npm run planning:pipeline:dry
npm run planning:pipeline:prompt
npm run planning:pipeline
```

기본은 quick AI test다. 필요하면 직접 인자를 넘긴다.

```bash
node scripts/run_planning_pipeline.js --test default
node scripts/run_planning_pipeline.js --test heavy
node scripts/run_planning_pipeline.js --skip-tests
node scripts/run_planning_pipeline.js --provider none
node scripts/run_planning_pipeline.js --provider codex
```

`planning:pipeline:prompt` 또는 `--provider none`은 AI 테스트와 프롬프트 생성까지만 수행하고 외부 모델 호출은 하지 않는다.

응답은 `docs/review_responses/YYYY-MM-DD-pipeline-claude.md` 또는 `docs/review_responses/YYYY-MM-DD-pipeline-codex.md`에 저장된다. Codex는 이 답변을 읽고 `docs/NEXT_TASKS.md`로 작업화한 뒤 구현을 계속한다.

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
