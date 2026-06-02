# AI Collaboration

## Purpose

LETHE는 HTML 프로토타입으로 핵심 재미를 검증한 뒤, 충분한 가능성이 확인되면 Unity 구현 단계로 넘어갈 근거를 만드는 프로젝트다.

AI는 구현을 대신하는 주체가 아니라, 테스트 결과를 해석하고 선택지를 제안하는 협업 도구다. 최종 구조 결정, 코드 변경, 검증, 커밋은 Codex와 개발자가 책임진다.

## Roles

- User: 목표, 재미 판단, 실제 플레이 피드백, 외부 전송/푸시 승인.
- Codex: HTML prototype implementation, tests, reports, git, task conversion, local automation.
- Claude: player emotion, pacing, regret/irritation, memory-loss feel, Unity-transition feel judgment.
- Codex CLI/GPT: systems design, balance model risk, implementation order, testability, Unity-transition feasibility.
- AI alpha simulator: repeatable proxy metrics, early-fun gates, forgetting emotion proxy, regression evidence.

## Workflow

1. User feedback or test result creates a planning question.
2. Codex updates or generates a prompt under `docs/review_prompts/`.
3. Before unattended automation, Codex runs `npm run autopilot:preflight` or `npm run autopilot:preflight:local`.
4. `npm run planning:pipeline` runs a quick AI test and sends the result prompt to both Claude and Codex CLI for major planning decisions.
5. The responses and double-check summary are saved under `docs/review_responses/`.
6. Codex reads both responses, records common points/conflicts, and updates `docs/NEXT_TASKS.md`.
7. Codex implements the selected smallest coherent unit.
8. Codex verifies with syntax checks, AI tests, browser QA, report dry-runs, and relevant docs.
9. Codex records the result in devlog/report/status and commits with Conventional Commits.

## Human Test Result Loop

After human sessions:

1. Put downloaded JSON logs in `playtest_logs/`.
2. Add qualitative notes to `docs/PLAYTEST_NOTES.md`.
3. Run `npm run playtest:summary`.
4. Review `docs/playtest_summaries/YYYY-MM-DD.md`.
5. Send `docs/review_prompts/YYYY-MM-DD-human-playtest.md` through Claude/GPT planning.
6. Codex updates `docs/NEXT_TASKS.md` and implements only the selected next step.

## Evidence Rules

Record the following in docs when they occur:

- A planning prompt changed the product direction.
- Claude/GPT recommended a gate such as `GO_TO_HUMAN_TEST` or `ITERATE_HTML_V06`.
- Codex rejected, narrowed, or modified an AI recommendation.
- An automation failed because of authentication, browser sandboxing, external transfer policy, or missing local tools.
- A test result caused a design decision.
- A human test result will affect HTML v0.6 or Unity transition judgment.
- Human test JSON logs are summarized into Markdown before being sent back to Claude/GPT.
- Large design changes, combat-core redesigns, balance-model failures, human-test readiness decisions, and Unity-transition decisions use Claude plus Codex CLI double check.

## Prompt Rules

- Prompts are written in Korean by default.
- Prompts include current build status, latest test evidence, known risks, and explicit scope limits.
- Prompts ask for actionable tasks, not generic opinions.
- Claude/Codex CLI should not edit files during automated planning.
- If external transmission is blocked, use `npm run planning:pipeline:prompt` and run the external step from a trusted local terminal.

## Developer Control Rules

- Do not apply AI output blindly.
- Do not add new scope just because AI suggests it.
- Keep scope guards unless the user explicitly changes them.
- Prefer human test evidence over AI proxy metrics for emotion and fun judgments.
- Prefer user live play feedback and browser combat evidence over both Claude and Codex CLI opinions.
- Treat a `GO` from any AI as `AI planning pass`, not balance or human-fun proof.
- Keep AI logs, decisions, and verification evidence close to the code changes they influenced.
- Treat autopilot preflight failures as blockers. Known Claude auth, dirty git state, missing fallback, or notification failures must be handled before a long automatic loop begins.

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
