# 2026-06-02-38 - Autonomous Dev Loop Preflight Ordering Cleanup 보고

## 1. 현재 빌드 상태

- LETHE HTML alpha v0.9 WP1은 구현 완료 상태로 유지한다.
- 이번 작업은 새 게임 기능이 아니라 unattended loop 진입 blocker를 줄이는 자동화 cleanup이다.
- 기존 `docs/loop_runs/2026-06-02-devloop-175642*` dirty 산출물은 아직 남아 있으므로 preflight blocker 자체는 완전히 해소되지 않았다.

## 2. 오늘 바뀐 것

- `scripts/run_autonomous_dev_loop.js`의 기본 preflight를 `node scripts/autopilot_preflight.js --allow-dirty`에서 `npm run autopilot:preflight:local`로 바꿨다.
- autonomous dev loop가 Markdown loop log를 만들기 전에 preflight를 먼저 실행하게 했다.
- preflight가 통과하면 stdout/stderr를 생성된 loop log header에 기록한다.
- `--allow-dirty`는 기본값이 아니라 명시적 local smoke-test 옵션으로만 남기고, 그때만 dirty 허용 preflight 명령으로 전환한다.

## 3. 테스트 결과와 근거

- `node --check scripts/run_autonomous_dev_loop.js`: 통과.
- `npm run dev:loop:dry`: 통과. dry-run 출력에서 preflight가 `npm run autopilot:preflight:local`로 표시됨을 확인했다.
- `node scripts/run_autonomous_dev_loop.js --dry-run --allow-dirty --no-commit --no-push --discord-dry-run`: 통과. 명시적 dirty smoke path에서만 `node scripts/autopilot_preflight.js --allow-dirty`로 표시됨을 확인했다.
- `npm run doctor`: 통과, 39 pass, 0 warn, 0 fail.
- `npm run ai:test:quick`: `GO_CANDIDATE`, Alpha Fun Score `0.8883`, regret `80.8%`, irritation `1.0%`, prediction `85.5%`, death/fail `40.0%`.
- `git diff --check`: 통과.
- `npm run autopilot:preflight:local`: 실패. 현재 dirty 파일이 남아 있으므로 기대한 실패다:
  - `M docs/loop_runs/2026-06-02-devloop-175642.md`,
  - `M scripts/run_autonomous_dev_loop.js`,
  - `?? docs/loop_runs/2026-06-02-devloop-175642-iteration-2-implement-prompt.md`.

## 4. 결정 사항

- WP1은 새 게임플레이 작업으로 재오픈하지 않는다.
- 이번 단위는 loop-run 산출물 자체를 정리한 것이 아니라, 다음 clean-tree dev loop가 자기 로그 때문에 preflight를 더럽히지 않게 만든 code-side cleanup으로 본다.
- 기존 dirty 산출물 기록/정리와 trusted-local `npm run qa:identity`는 계속 WP2 전 blocker다.

## 5. 문제 또는 리스크

- 현재 세션은 dirty 상태라 `npm run autopilot:preflight:local`이 아직 실패한다.
- 커밋/푸시는 이번 요청 범위에서 하지 않았으므로 wrapper 또는 사용자가 산출물 기록을 완료해야 한다.
- 이 변경은 자동화 진입 순서 개선이며, 브라우저 identity 가독성 재검증은 아니다.

## 6. GPT handoff summary

- autonomous dev loop가 자기 log를 만든 뒤 preflight dirty warning을 `--allow-dirty`로 숨기던 구조를 제거했다.
- 다음 unattended loop는 clean tree에서 preflight를 먼저 통과한 뒤 log를 만들게 된다.
- 남은 실행 순서는 loop-run dirty 정리, trusted-local `npm run qa:identity`, WP2 Slice A 압박 고저차다.

## 7. Next Codex tasks

- `docs/loop_runs/2026-06-02-devloop-175642*.md` 산출물을 wrapper가 기록하거나 다음 unattended loop 전에 정리한다.
- clean tree에서 `npm run autopilot:preflight:local`을 다시 실행한다.
- trusted local에서 `npm run qa:identity`를 재실행하고 통과하면 v0.9 WP2 Slice A로 넘어간다.

## 8. Portfolio notes

- 문제: 자동 루프가 자기 로그 때문에 preflight를 더럽히고, 기본 `--allow-dirty`가 그 문제를 숨길 수 있었다.
- 방향: preflight를 실제 루프 산출물 생성보다 앞에 두어 unattended automation의 진입 조건을 엄격하게 만든다.
- 실행: preflight 실행 순서와 기본 명령을 수정하고, 통과 결과를 log header에 남기게 했다.
- 결과: 다음 clean-tree 자동 루프는 dirty 상태를 숨기지 않고 시작 조건을 더 명확히 검증한다.

---
