문서 갱신 완료했습니다. 코드 기능 변경은 하지 않았습니다.

반영한 내용:
- `feedback-2` double-check synthesis 작성
- [docs/NEXT_TASKS.md](/Users/mr68/jaewoo/LETHE/docs/NEXT_TASKS.md)에 공통점/충돌/선택 범위/다음 작업 갱신
- [docs/CODEX_STATUS.md](/Users/mr68/jaewoo/LETHE/docs/CODEX_STATUS.md)에 최신 gate 상태 반영
- [docs/devlog/2026-06-02.md](/Users/mr68/jaewoo/LETHE/docs/devlog/2026-06-02.md)와 [docs/reports/2026-06-02.md](/Users/mr68/jaewoo/LETHE/docs/reports/2026-06-02.md)에 루프 결과 기록
- `npm run report`로 `docs/reports/2026-06-02.html` 재생성

검증:
- `npm run report`: 통과
- `git diff --check`: 통과

핵심 결론은 그대로 정리했습니다: WP1 자동화 정비는 code-side로 완료됐지만, `docs/loop_runs/2026-06-02-devloop-175642*.md` 산출물 정리, clean-tree `npm run autopilot:preflight:local`, trusted-local `npm run qa:identity` 통과 전까지 WP2 착수는 보류입니다.