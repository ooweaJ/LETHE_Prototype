문서 갱신 완료했습니다. 코드 기능 변경은 하지 않았습니다.

반영한 내용:
- feedback-3 공통점/충돌/선택 범위를 `docs/review_responses/...feedback-3-double-check.md`에 정리
- `docs/NEXT_TASKS.md`, `docs/CODEX_STATUS.md`, `docs/devlog/2026-06-03.md` 갱신
- `docs/reports/2026-06-03.md`의 기존 `# 2026-06-03-05 - 전술 집중 구현과 브라우저 검증 보류 결정` 단위에 feedback-3까지 접어 넣음
- `npm run report`로 `docs/reports/units/2026-06-03/`의 최신 Markdown/HTML 단위 파일 재생성
- Discord 첨부 기준이 일일 누적 HTML이 아니라 최신 unit HTML임을 기록

검증:
- `npm run report`: 통과, 5개 unit report 생성
- `npm run report:check`: 통과
- `node scripts/send_discord_report.js --dry-run --latest-section`: 최신 unit HTML 첨부 확인
- 이번에 수정한 문서/생성 보고서 대상 `git diff --check`: 통과

참고: 전체 `git diff --check`는 기존 dirty 상태인 `docs/loop_runs/2026-06-03-devloop-050050.md`의 trailing whitespace 때문에 실패합니다. 이번 작업 범위 파일에서는 whitespace 문제 없습니다.