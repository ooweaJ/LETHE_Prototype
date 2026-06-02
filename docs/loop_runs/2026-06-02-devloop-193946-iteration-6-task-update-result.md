문서만 갱신했습니다.

반영한 내용:
- [feedback-6 double-check](/Users/mr68/jaewoo/LETHE/docs/review_responses/2026-06-02-devloop-193946-feedback-6-double-check.md)에 공통점/충돌/선택 범위/필수 테스트를 채웠습니다.
- [NEXT_TASKS.md](/Users/mr68/jaewoo/LETHE/docs/NEXT_TASKS.md)에 feedback-6 결론과 다음 작업 `npm run qa:postloss:trusted`를 추가했습니다.
- [CODEX_STATUS.md](/Users/mr68/jaewoo/LETHE/docs/CODEX_STATUS.md), [devlog](/Users/mr68/jaewoo/LETHE/docs/devlog/2026-06-02.md), [report](/Users/mr68/jaewoo/LETHE/docs/reports/2026-06-02.md)에 루프 결과를 기록했습니다.
- 보고서에는 `# 2026-06-02-57 - Devloop 193946 Feedback-6 태스크 갱신` 형식으로 새 top-level heading을 추가했고, HTML도 Markdown 기준으로 재생성했습니다.

검증:
- `npm run report`: 통과, HTML 재생성
- `npm run report:check`: 통과, 57 units
- 갱신한 파일들의 trailing whitespace 확인: 통과

참고: 전체 `git diff --check`는 기존 dirty 파일 `docs/loop_runs/2026-06-02-devloop-193946.md`의 trailing whitespace 때문에 실패했습니다. 이번 문서 갱신 파일들은 별도로 확인했고 문제 없습니다.