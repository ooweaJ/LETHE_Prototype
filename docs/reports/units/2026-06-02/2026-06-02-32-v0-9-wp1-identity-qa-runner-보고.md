# 2026-06-02-32 - v0.9 WP1 Identity QA Runner 보고

## 1. 현재 빌드 상태

- v0.9 Work Package 1은 빌드 정체성 UI/payload hook이 구현된 상태다.
- 이번 작업으로 `?qa=fast,identity`를 실행하는 전용 runner가 생겼다.
- 실제 identity browser QA 통과 판정은 아직 보류다.

## 2. 오늘 바뀐 것

- `scripts/run_browser_identity_qa.js`를 추가했다.
- `package.json`에 `qa:identity`를 추가했다.
- `doctor`와 `doctor:deep`가 identity QA runner를 확인하게 했다.
- `docs/NEXT_TASKS.md`에는 runner 추가를 완료로 기록하고, 실제 identity QA 통과 항목은 미완료로 유지했다.

## 3. 테스트 결과와 근거

- `node --check scripts/run_browser_identity_qa.js`: 통과.
- `node --check scripts/check_local_pipeline.js`: 통과.
- `node --check src/game.js`: 통과.
- `npm run doctor`: 39 pass, 0 warn, 0 fail.
- `npm run ai:test:quick`: `GO_CANDIDATE`, Alpha Fun Score `0.8883`.
- `npm run qa:identity`: 현재 sandbox에서는 실패/차단.
  - in-app Browser: `iab` unavailable.
  - Chrome CDP port: `connect EPERM`.
  - Chrome pipe: `Target.getTargets` 응답 timeout.
- `npm run autopilot:preflight:local`: dirty tree와 기존 미추적 loop log 때문에 실패. 무인 루프 시작 전 정리 필요.

## 4. 결정 사항

- WP1은 아직 완료 처리하지 않는다.
- 이번 단위는 identity QA runner 추가까지만 완료로 본다.
- WP2 post-loss challenge는 `npm run qa:identity`가 trusted local에서 통과한 뒤 시작한다.

## 5. 문제 또는 리스크

- 현재 Codex sandbox는 실제 브라우저 접근을 막고 있어 browser QA 증거를 만들 수 없다.
- runner가 trusted local에서도 실패하면 Playwright/Puppeteer 같은 명시적 브라우저 자동화 의존성을 도입할지 판단해야 한다.

## 6. GPT handoff summary

- v0.9 WP1의 다음 병목은 구현이 아니라 검증 환경이다.
- `npm run qa:identity`가 `data-lethe-identity-qa.status: complete`를 출력하면 WP1 identity QA를 완료로 바꿀 수 있다.
- AI 점수는 유지되지만 UI 읽기성 통과 증거로 쓰지 않는다.

## 7. Next Codex tasks

- trusted local에서 `npm run qa:identity`를 재실행한다.
- 통과하면 `docs/NEXT_TASKS.md`의 identity QA 항목을 완료 처리한다.
- 그 다음에만 기억별 설명 문구 압축 또는 WP2 post-loss challenge로 이동한다.

## 8. Portfolio notes

- 문제: 기존 Chrome `--dump-dom` 검증은 사용 중인 브라우저 세션에 handoff되어 자동 증거를 만들지 못했다.
- 방향: QA URL의 dataset을 읽는 명령형 runner로 브라우저 검증을 재현 가능하게 만든다.
- 실행: CDP 기반 identity QA runner와 npm/doctor 연결을 추가했다.
- 결과: 검증 명령은 생겼지만, 현재 sandbox 제약 때문에 실제 통과는 trusted local 실행으로 넘긴다.

---
