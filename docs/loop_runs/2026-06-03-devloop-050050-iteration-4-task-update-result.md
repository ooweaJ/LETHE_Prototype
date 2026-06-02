문서만 갱신했습니다.

반영한 내용:
- Claude/Codex feedback-4 공통점/충돌/선택 범위를 정리했습니다.
- [docs/NEXT_TASKS.md](/Users/mr68/jaewoo/LETHE/docs/NEXT_TASKS.md)에 feedback-4 verdict와 다음 실행 범위를 반영했습니다.
- [docs/CODEX_STATUS.md](/Users/mr68/jaewoo/LETHE/docs/CODEX_STATUS.md)를 feedback-4 기준으로 갱신했습니다.
- [docs/devlog/2026-06-03.md](/Users/mr68/jaewoo/LETHE/docs/devlog/2026-06-03.md)에 tactical trusted rerun feedback synthesis를 추가했습니다.
- [docs/reports/2026-06-03.md](/Users/mr68/jaewoo/LETHE/docs/reports/2026-06-03.md)는 새 절차 단위를 만들지 않고 기존 `2026-06-03-05 - 전술 집중 구현과 브라우저 검증 보류 결정` 단위에 feedback-4를 접었습니다.
- `npm run report`로 `docs/reports/units/2026-06-03/` 최신 단위 Markdown/HTML도 재생성했습니다.

검증:
- `npm run report`: 통과, 5 unit 생성
- `npm run report:check`: 통과
- `node scripts/send_discord_report.js --dry-run --latest-section`: 최신 unit HTML 첨부 확인
- `git diff --check`: 통과

다음 실행 범위는 그대로 `sandbox 밖 trusted-local npm run qa:tactical:trusted` 하나입니다. 새 기능/힌트/밸런스/UI 범위는 늘리지 않았습니다.