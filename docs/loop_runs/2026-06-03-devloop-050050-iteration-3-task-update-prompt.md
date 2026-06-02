# LETHE Task Update Cycle

너는 LETHE 프로젝트의 기록/태스크 갱신 담당 Codex다.
코드 기능 변경은 하지 말고 문서만 갱신한다.

## 읽을 파일

- docs/review_prompts/2026-06-03-devloop-050050-feedback-3.md
- docs/review_responses/2026-06-03-devloop-050050-feedback-3-claude.md
- docs/review_responses/2026-06-03-devloop-050050-feedback-3-codex.md
- docs/review_responses/2026-06-03-devloop-050050-feedback-3-double-check.md
- docs/NEXT_TASKS.md
- docs/CODEX_STATUS.md

## 할 일

- Claude/Codex 피드백의 공통점과 충돌을 요약한다.
- `docs/NEXT_TASKS.md`의 완료/다음 작업을 갱신한다.
- `docs/CODEX_STATUS.md`, `docs/devlog/2026-06-03.md`, `docs/reports/2026-06-03.md`에 루프 결과를 기록한다.
- 보고서의 새 top-level heading은 반드시 `# 2026-06-03-NN - 기능/결정 제목` 형식으로 추가한다.
- 보고 단위는 구현 + 검증 + Claude/Codex 피드백 + 다음 태스크 결정을 합친 기능/결정 단위여야 한다.
- `Feedback-N 태스크 갱신`, `자동 개발 루프 N차`, `구현 결과`, `QA 재시도 1회`처럼 작은 절차명은 보고 단위로 만들지 않는다.
- `npm run report`가 최신 단위를 `docs/reports/units/2026-06-03/`에 Markdown/HTML로 분리 생성한다는 전제로, 일일 누적 파일만이 아니라 단위 파일이 Discord 첨부 기준임을 기록한다.
- 새 구현 범위를 늘리지 않는다.
