문서 갱신 완료했습니다.

반영한 내용:
- feedback-4 double-check 요약 작성
- Claude/Codex 공통점과 충돌을 `docs/NEXT_TASKS.md`에 기록
- `docs/CODEX_STATUS.md`, `docs/devlog/2026-06-02.md` 갱신
- `docs/reports/2026-06-02.md`에 `# 2026-06-02-53 - Devloop 193946 Feedback-4 태스크 갱신` 추가
- Markdown 기준으로 `docs/reports/2026-06-02.html` 재생성
- 기존 dirty loop-run 문서의 trailing whitespace 1종 정리

검증:
- `npm run report:check`: 통과, 53 units
- `npm run report`: HTML 재생성 완료
- `git diff --check`: 통과

코드 기능 변경은 하지 않았습니다. 현재 다음 실행 범위는 그대로 trusted-local `npm run qa:postloss`이며, WP3/사람 테스트는 browser proof 또는 environment-blocker 결정 전까지 blocked로 기록했습니다.