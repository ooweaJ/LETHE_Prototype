**구현한 것**
- WP3 전술 집중 전용 blocker prompt를 추가했습니다: [2026-06-03-tactical-browser-transport-blocker.md](/Users/mr68/jaewoo/LETHE/docs/review_prompts/2026-06-03-tactical-browser-transport-blocker.md)
- `qa:tactical:trusted`가 더 이상 post-loss prompt가 아니라 전술 전용 prompt를 안내하도록 수정했습니다.
- [CODEX_STATUS.md](/Users/mr68/jaewoo/LETHE/docs/CODEX_STATUS.md), [NEXT_TASKS.md](/Users/mr68/jaewoo/LETHE/docs/NEXT_TASKS.md), [devlog](/Users/mr68/jaewoo/LETHE/docs/devlog/2026-06-03.md)에 이번 gate 재실행과 blocker 상태를 기록했습니다.
- 보고서에는 지침대로 새 top-level 단위를 만들지 않았습니다.
- 커밋 완료: `d55ab75 test: 전술 QA blocker 프롬프트 분리`

**검증한 것**
- `node --check scripts/run_trusted_tactical_gate.js`: 통과
- `node --check scripts/check_local_pipeline.js`: 통과
- `npm run doctor`: 46 pass / 0 warn / 0 fail
- `npm run qa:tactical:trusted`: 실패했지만 예상된 managed sandbox transport blocker입니다. 결과 JSON은 `status: blocked`, `transportFailure: true`, `blockerPrompt: docs/review_prompts/2026-06-03-tactical-browser-transport-blocker.md`로 기록됐습니다.

**남은 위험**
- WP3 Slice A는 아직 browser-proven이 아닙니다.
- Chrome/CDP pipe는 `Target.getTargets` timeout, port fallback은 `127.0.0.1 listen EPERM`으로 gameplay evaluation 전에 막힙니다.
- 기존 작업 트리에 제가 건드리지 않은 `docs/loop_runs/...` 변경 2개가 남아 있습니다.

**다음 루프 추천 작업**
- sandbox 밖 trusted-local에서 `npm run qa:tactical:trusted` 실행.
- 통과하면 WP3 Slice A를 browser-proven으로 상태/태스크/devlog에 반영.
- 같은 transport 실패가 trusted-local에서도 반복되면 새로 추가한 tactical blocker prompt로 environment-blocker 결정을 먼저 받기.