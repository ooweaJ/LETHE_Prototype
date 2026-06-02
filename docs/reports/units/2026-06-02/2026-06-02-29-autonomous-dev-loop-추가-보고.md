# 2026-06-02-29 - Autonomous Dev Loop 추가 보고

## 1. 현재 빌드 상태

- 기존 `overnight:loop`는 기획/검증/보고 루프에 가까웠다.
- 사용자가 원하는 실제 루프는 task 확인 후 구현, 검증, 보고, Claude/Codex 피드백, task 갱신, 다음 구현으로 이어지는 개발 루프다.
- 이 실제 개발 루프를 `dev:loop`로 새로 추가했다.

## 2. 오늘 바뀐 것

- `scripts/run_autonomous_dev_loop.js`를 추가했다.
- `package.json`에 `dev:loop`, `dev:loop:dry`를 추가했다.
- `doctor`, `doctor:deep`, `autopilot:preflight`가 새 루프 명령을 확인하게 했다.
- 루프 순서를 다음처럼 고정했다:
  - `docs/NEXT_TASKS.md` 확인,
  - Codex CLI 구현,
  - `npm run doctor`,
  - `npm run ai:test:quick`,
  - Markdown/HTML 보고서 갱신,
  - Discord work-unit report,
  - Claude + Codex CLI 피드백,
  - Codex 문서/task 갱신,
  - commit/push,
  - 다음 iteration.

## 3. 테스트 결과와 근거

- `node --check scripts/run_autonomous_dev_loop.js`: 통과.
- `npm run dev:loop:dry`: 통과.
- `npm run doctor`: 38 pass, 0 warn, 0 fail.
- `npm run doctor:deep`: 54 pass, 0 warn, 0 fail.

## 4. 결정 사항

- 실제 장시간 자동 개발은 `npm run dev:loop`가 담당한다.
- 기본값은 6 iterations / 360 minutes로 둔다.
- 성공한 iteration은 commit/push까지 진행한다.
- 검증 실패 iteration은 commit하지 않고 blocker prompt를 남긴 뒤 멈춘다.

## 5. 문제 또는 리스크

- 실제 장시간 구현 루프는 아직 이번 턴에서 시작하지 않았다.
- `codex exec --sandbox workspace-write`가 구현을 담당하므로, 로컬 Codex CLI 인증과 권한이 안정적이어야 한다.
- 자동 루프는 현재 scope guard를 반드시 따라야 하며, 새 기억/상점/메타 성장 같은 범위 확장은 금지다.

## 6. GPT/Claude 인계 요약

- 피드백은 `node scripts/run_planning_pipeline.js --provider double --test none`으로 Claude와 Codex CLI 양쪽에서 받는다.
- 피드백 이후 별도 Codex docs-only pass가 `docs/NEXT_TASKS.md`, `docs/CODEX_STATUS.md`, devlog/report를 갱신한다.
- 다음 구현 범위는 계속 v0.9 Work Package 1이다.

## 7. 다음 Codex 작업

- 실제 장시간 실행 전 clean tree에서 `npm run doctor:deep`를 확인한다.
- 실행 명령:
  - `npm run dev:loop`
  - 또는 짧게 `node scripts/run_autonomous_dev_loop.js --iterations 2 --duration-minutes 90`.
- v0.9 Work Package 1부터 자동 구현 루프를 시작한다.

## 8. 포트폴리오 메모

- 문제: 기존 자동화는 planning/report loop였고, 사용자가 원하는 “자는 동안 구현이 계속 되는 루프”가 아니었다.
- 방향: 구현 Codex와 피드백 Claude/Codex를 분리해, 구현-검증-보고-피드백-task 갱신을 반복하는 구조로 바꿨다.
- 실행: autonomous dev loop 스크립트, npm 명령, doctor/deep check 연결을 추가했다.
- 결과: `dev:loop`는 실제 구현을 반복할 수 있는 구조를 갖췄고, dry-run과 doctor 기준으로 준비 상태가 검증됐다.

---
