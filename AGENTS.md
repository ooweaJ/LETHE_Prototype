# LETHE Project Agent Rules

이 저장소에서 Codex는 HTML 프로토타입 구현, 검증, 기록 갱신을 담당한다.

현재 목표는 LETHE의 핵심 재미와 가능성을 HTML 프로토타입으로 빠르게 검증한 뒤, 충분히 재미가 확인되면 Unity 구현 단계로 넘어갈 근거를 만드는 것이다.

## Core Workflow

1. 자동 버전업/무인 루프를 시작하기 전 `npm run autopilot:preflight` 또는 최소 `npm run autopilot:preflight:local`을 실행한다.
2. 구현 전 현재 파일과 문서를 확인한다.
3. 코드 변경 후 가능한 검증을 실행한다.
4. 결과를 `docs/CODEX_STATUS.md`에 반영한다.
5. 의미 있는 작업 단위가 끝나면 `docs/devlog/YYYY-MM-DD.md`와 `docs/reports/YYYY-MM-DD.md`를 갱신한다.
6. 보고서 HTML은 Markdown 원본을 기준으로 생성한다.
7. 의미 있는 구현/밸런스/문서화 단위가 끝나면 `npm run report`, `npm run report:check`, `npm run report:discord:unit`까지 실행해 Discord 작업 단위 보고를 실제 전송한다. `dry-run`은 본문/첨부 확인용이지 완료 상태가 아니다.
8. Discord 실제 전송 예외는 사용자가 명시적으로 전송하지 말라고 했거나 webhook/네트워크/권한 문제로 실패한 경우뿐이다. 실패하면 실패 원인과 다음 실행 명령을 devlog/report에 남긴다.
9. 검증과 보고까지 끝난 의미 있는 작업 단위는 Conventional Commit으로 커밋하고, 작업 트리가 clean이며 공유해도 안전하면 `git push`까지 완료한다. push 실패 시 원인과 다음 명령을 사용자에게 보고한다.
10. 테스트 결과를 바탕으로 기획 수정 또는 방향 결정이 필요하면 `docs/review_prompts/`에 Claude/GPT 전달 프롬프트를 남긴다.

## Orchestration Interface

This project uses `docs/orchestration/` as the shared Codex project-management interface. Existing project-specific rules in this `AGENTS.md` remain the top-level rules; orchestration files organize state, tasks, decisions, devlogs, reports, and evidence so Codex and the user can resume work quickly.

Before meaningful work, read:

1. `docs/orchestration/README.md`
2. `docs/orchestration/STATUS.md`
3. `docs/orchestration/CURRENT_TASK.md`
4. `docs/orchestration/NEXT_TASKS.md`
5. `docs/orchestration/PROMPT_CONTEXT.md`
6. `docs/orchestration/RUNBOOK.md`
7. `docs/orchestration/SCOPE_GUARD.md`

Use orchestration files as follows:

- `PROJECT_BRIEF.md`: project identity, goals, tech stack, and portfolio framing.
- `STATUS.md`: whole-project current state, latest verification, blockers, and next major step.
- `CURRENT_TASK.md`: one active work unit, done criteria, related files, open questions, and verification commands.
- `NEXT_TASKS.md`: top five next work candidates only.
- `PROMPT_CONTEXT.md`: stable context Codex should keep in mind each session.
- `RUNBOOK.md`: commands and procedures for test, build, report, package, recovery, and external handoff.
- `SCOPE_GUARD.md`: explicit non-goals and things not to build yet.
- `DECISION_LOG.md`: index of important technical, product, and AI-direction decisions.
- `devlog/`: internal daily work log.
- `reports/`: user-facing and portfolio-facing work-unit reports.
- `review_prompts/`: prompts prepared for Claude/GPT/Codex review.
- `review_responses/`: saved AI review responses.
- `evidence/`: useful test outputs, screenshots, logs, benchmark summaries, or links.
- `templates/`: reusable document, report, review, and task templates.
- `index.html`: generated project dashboard for people.
- `command.html`: generated next-instruction block for the command area.
- `runbook.html`: generated operating-procedure block.

After meaningful work:

- Update `STATUS.md`, `CURRENT_TASK.md`, or `NEXT_TASKS.md` as needed.
- Record internal process details in `devlog/YYYY-MM-DD.md`.
- Record user-facing summaries in `reports/YYYY-MM-DD.md`.
- Record durable decisions in `DECISION_LOG.md`.
- Keep `NEXT_TASKS.md` short, usually no more than five active candidates.
- Keep Markdown as the source of truth; generated HTML is the human-facing project interface generated from Markdown.
- Do not delete legacy docs during adoption; summarize or link them from orchestration files.

## Role Split

- Codex: HTML prototype code, tests, git, report generation, status updates.
- Claude/GPT: test-result interpretation, planning revision, design direction, prioritization, Unity-transition judgment.

## Report Requirements

Every daily report should preserve these eight items:

1. Current build status.
2. What changed today.
3. Test results and evidence.
4. Decisions made.
5. Problems or risks.
6. GPT handoff summary.
7. Next Codex tasks.
8. Portfolio notes: problem, direction, action, result.

## Git Convention

Use Conventional Commits.

Examples:

- `feat: 기억 자동 발동 구현`
- `fix: 보스 처치 후 질문 UI 오류 수정`
- `docs: 오늘 개발 보고서 갱신`
- `test: AI 알파테스트 시나리오 추가`
- `chore: gitignore 정리`

Commit after a coherent unit is implemented and verified. Push after the working tree is clean and the commit is safe to share.

## Scope Guard

Until GPT or the user explicitly changes scope, do not add:

- Meta progression.
- Shop systems.
- Final boss.
- More than the current 6 memories.
- Multi-region run structure.

The current prototype goal is to verify whether the core loop is fun enough to justify a Unity implementation. The forgetting loop should feel regrettable rather than irritating, but only after the early combat/growth loop is fun enough for players to reach it.

## Claude Code Automation

- Claude Code may be used after AI or human tests to interpret results, revise planning, and decide what Codex should implement next.
- Claude answers should be saved under `docs/review_responses/YYYY-MM-DD-claude.md`.
- Claude should not edit project files during automated planning; it is called with tools disabled.
- Codex reads the saved Claude response, updates `docs/NEXT_TASKS.md`, then implements the selected work.
- Discord is only a status/attention channel. Markdown files are the source of truth.

## Autopilot Preflight

- Before any unattended implement -> test -> Claude report -> implement loop, run `npm run autopilot:preflight`.
- If external Claude transmission is not allowed in the current session, run `npm run autopilot:preflight:local` and generate the Claude prompt for the user to execute from a trusted terminal.
- Preflight failures are blockers, not footnotes. Fix authentication, dirty git state, missing scripts, missing Discord config, or fallback readiness before starting the loop.
- If preflight cannot pass, Codex must document the exact failed check and next command instead of starting a long automation run.
