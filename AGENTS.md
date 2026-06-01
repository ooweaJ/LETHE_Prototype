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

## Scope Guard

Until GPT or the user explicitly changes scope, do not add:

- Meta progression.
- Shop systems.
- Final boss.
- More than the current 6 memories.
- Multi-region run structure.

The current prototype goal is to verify whether forgetting feels regrettable rather than irritating.

