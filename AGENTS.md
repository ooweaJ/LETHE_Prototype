# LETHE Project Agent Rules

이 저장소에서 Codex는 구현, 검증, 기록 갱신을 담당한다.

## Core Workflow

1. 구현 전 현재 파일과 문서를 확인한다.
2. 코드 변경 후 가능한 검증을 실행한다.
3. 결과를 `docs/CODEX_STATUS.md`에 반영한다.
4. 의미 있는 작업 단위가 끝나면 `docs/devlog/YYYY-MM-DD.md`와 `docs/reports/YYYY-MM-DD.md`를 갱신한다.
5. 보고서 HTML은 Markdown 원본을 기준으로 생성한다.
6. GPT 기획 검토가 필요한 질문은 `docs/GPT_REVIEW_PROMPT.md` 또는 `docs/NEXT_TASKS.md`에 남긴다.

## Role Split

- Codex: code, tests, git, report generation, status updates.
- GPT: planning review, design direction, prioritization, playtest interpretation.

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

The current prototype goal is to verify whether forgetting feels regrettable rather than irritating.

## Claude Code Automation

- Claude Code may be used for planning review through `scripts/ask_claude_review.js`.
- Claude answers should be saved under `docs/review_responses/YYYY-MM-DD-claude.md`.
- Claude should not edit project files during automated review; it is called with tools disabled.
- Codex reads the saved Claude response, updates `docs/NEXT_TASKS.md`, then implements the selected work.
- Discord is only a status/attention channel. Markdown files are the source of truth.
