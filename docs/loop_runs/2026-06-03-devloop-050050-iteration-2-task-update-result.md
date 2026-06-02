문서 갱신 완료했습니다. 코드 기능 변경은 하지 않았습니다.

반영 내용:
- Claude/Codex feedback-2 공통점과 충돌 요약 작성
- 선택 범위 확정: 새 힌트/UI/밸런스 구현은 보류, 다음 실행은 `trusted-local npm run qa:tactical:trusted`
- [docs/NEXT_TASKS.md](/Users/mr68/jaewoo/LETHE/docs/NEXT_TASKS.md), [docs/CODEX_STATUS.md](/Users/mr68/jaewoo/LETHE/docs/CODEX_STATUS.md), [docs/devlog/2026-06-03.md](/Users/mr68/jaewoo/LETHE/docs/devlog/2026-06-03.md), [docs/reports/2026-06-03.md](/Users/mr68/jaewoo/LETHE/docs/reports/2026-06-03.md) 갱신
- `npm run report`로 `docs/reports/units/2026-06-03/` 최신 단위 Markdown/HTML 재생성
- feedback-2 double-check summary 채움

검증:
- `npm run report`: 통과, 5 unit 생성
- `npm run report:check`: 통과
- `node scripts/send_discord_report.js --dry-run --latest-section`: 최신 unit HTML 첨부 확인
- `git diff --check`: 통과

커밋:
- `a444731 docs: 전술 피드백 태스크 갱신`

작업트리는 clean 상태입니다.