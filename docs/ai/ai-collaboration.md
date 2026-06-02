# AI Collaboration

## Purpose

LETHE는 HTML 프로토타입으로 핵심 재미를 검증한 뒤, 충분한 가능성이 확인되면 Unity 구현 단계로 넘어갈 근거를 만드는 프로젝트다.

AI는 구현을 대신하는 주체가 아니라, 테스트 결과를 해석하고 선택지를 제안하는 협업 도구다. 최종 구조 결정, 코드 변경, 검증, 커밋은 Codex와 개발자가 책임진다.

## Roles

- User: 목표, 재미 판단, 실제 플레이 피드백, 외부 전송/푸시 승인.
- Codex: HTML prototype implementation, tests, reports, git, task conversion, local automation.
- Claude/GPT: AI/human test result interpretation, planning revision, prioritization, Unity-transition judgment.
- AI alpha simulator: repeatable proxy metrics, early-fun gates, forgetting emotion proxy, regression evidence.

## Workflow

1. User feedback or test result creates a planning question.
2. Codex updates or generates a prompt under `docs/review_prompts/`.
3. `npm run planning:pipeline` runs a quick AI test and sends the result prompt to Claude first, Codex CLI fallback second.
4. The response is saved under `docs/review_responses/`.
5. Codex reads the response and updates `docs/NEXT_TASKS.md`.
6. Codex implements the selected smallest coherent unit.
7. Codex verifies with syntax checks, AI tests, browser QA, report dry-runs, and relevant docs.
8. Codex records the result in devlog/report/status and commits with Conventional Commits.

## Evidence Rules

Record the following in docs when they occur:

- A planning prompt changed the product direction.
- Claude/GPT recommended a gate such as `GO_TO_HUMAN_TEST` or `ITERATE_HTML_V06`.
- Codex rejected, narrowed, or modified an AI recommendation.
- An automation failed because of authentication, browser sandboxing, external transfer policy, or missing local tools.
- A test result caused a design decision.
- A human test result will affect HTML v0.6 or Unity transition judgment.

## Prompt Rules

- Prompts are written in Korean by default.
- Prompts include current build status, latest test evidence, known risks, and explicit scope limits.
- Prompts ask for actionable tasks, not generic opinions.
- Claude/GPT should not edit files during automated planning.
- If external transmission is blocked, use `npm run planning:pipeline:prompt` and run the external step from a trusted local terminal.

## Developer Control Rules

- Do not apply AI output blindly.
- Do not add new scope just because AI suggests it.
- Keep scope guards unless the user explicitly changes them.
- Prefer human test evidence over AI proxy metrics for emotion and fun judgments.
- Keep AI logs, decisions, and verification evidence close to the code changes they influenced.

## Current LETHE Example

### Problem

v0.4 explained forgetting better, but the user reported that the first minutes were too loose and lacked roguelike/Vampire-Survivors-style growth.

### AI Use

Codex generated a planning prompt, implemented a narrow v0.5 core-fun pass, then used Claude pipeline evaluation after AI and browser QA.

### Developer Decision

The team did not add shops, meta progression, new memories, or new regions. The solution stayed inside the current HTML prototype scope: denser enemy packs, kill XP, and run-only 3-choice level-up stats.

### Verification

- `npm run ai:test`: `GO_CANDIDATE`.
- `npm run ai:test:heavy`: `GO_CANDIDATE`.
- Chrome headless `?qa=fast,levelup`: level-up UI, resume, and `runGrowth` payload passed.
- Claude pipeline verdict: `GO_TO_HUMAN_TEST`.

### Result

No further gameplay expansion before human testing. Only tester/session metadata was added so human-test logs can be analyzed cleanly.
