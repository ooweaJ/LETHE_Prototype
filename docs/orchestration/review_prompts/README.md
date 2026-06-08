# Review Prompts

이 폴더는 Claude, GPT, Codex CLI 같은 검토 에이전트에게 넘길 프롬프트의 현재 기준 위치다.

레거시 `docs/review_prompts/`는 마이그레이션 전 기록으로만 본다. 새 프롬프트는 이 폴더에 작성한다.

권장 파일명:

```text
docs/orchestration/review_prompts/YYYY-MM-DD-topic.md
```

## 작성 원칙

- 기본은 한국어로 쓴다.
- Codex가 바로 실행할 수 있게 답변 형식을 지정한다.
- 최신 테스트 결과, 사람 반응, 현재 빌드 상태, Unity 전환 판단 질문을 포함한다.
- 작은 문구/후속 확인은 한쪽 모델만 사용해도 된다.
- 큰 기획 변경, 전투 코어 재설계, 밸런스 모델 실패, 사람 테스트 진입, Unity 전환 판단은 Claude와 Codex CLI 더블 체크를 기본으로 쓴다.
- Claude Code가 로그인/키체인 문제로 막히면 더블 체크를 완료할 수 없으므로 preflight에서 막고, 프롬프트만 생성한 뒤 신뢰된 로컬에서 다시 실행한다.

## 테스트 결과 기반 자동 파이프라인

최신 AI 테스트를 돌리고, 그 결과로 `docs/orchestration/review_prompts/YYYY-MM-DD-pipeline.md`를 생성한 뒤, 기본적으로 Claude와 Codex CLI 양쪽 기획 답변을 저장한다.

```bash
npm run planning:pipeline:dry
npm run planning:pipeline:prompt
npm run planning:pipeline
npm run planning:pipeline:double
```

`planning:pipeline:prompt` 또는 `--provider none`은 AI 테스트와 프롬프트 생성까지만 수행하고 외부 모델 호출은 하지 않는다.

응답은 다음 파일에 저장된다.

```text
docs/orchestration/review_responses/YYYY-MM-DD-pipeline-claude.md
docs/orchestration/review_responses/YYYY-MM-DD-pipeline-codex.md
docs/orchestration/review_responses/YYYY-MM-DD-pipeline-double-check.md
```

Codex는 두 답변을 모두 읽고 공통점/충돌점을 정리한 뒤 `docs/orchestration/state/NEXT_TASKS.md`로 작업화하고 구현을 계속한다.

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
