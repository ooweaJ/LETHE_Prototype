# 2026-06-09-03 - Project Orchestrator Discord 연결

## 1. 현재 빌드 상태

- 게임 빌드나 밸런스 코드는 변경하지 않았다.
- Discord 보고 경로를 기존 직접 webhook fallback에서 Project Orchestrator 중앙 intake 우선 흐름으로 연결했다.
- Project Orchestrator endpoint는 `http://127.0.0.1:4317/api/orchestration/discord-report`다.

## 2. 오늘 바뀐 것

- `scripts/send_orchestrator_discord_report.js`를 추가했다.
- `package.json`에 `report:orchestrator`, `report:orchestrator:dry`, `report:orchestrator:unit`, `report:orchestrator:unit:dry`를 추가했다.
- `docs/DISCORD_REPORTING.md`에 실제 Orchestrator 명령과 payload 형식을 반영했다.
- `docs/orchestration/state/RUNBOOK.md`와 `docs/orchestration/interface/runbook.html`에 Orchestrator 전송 명령을 추가했다.
- `docs/orchestration/MIGRATION_PROMPT.md`와 `HTML_INTERFACE_TEMPLATE.md`에는 다른 프로젝트가 따라야 할 Orchestrator script 명령, `projectId`, `reportPath`, `dryRun`, hash drill-down 규칙을 보강했다.

## 3. 테스트 결과와 근거

- `node --check scripts/send_orchestrator_discord_report.js`: 통과.
- `node --check scripts/build_report.js`: 통과.
- `node scripts\send_orchestrator_discord_report.js --latest-section --dry-run --print-payload`: 통과, `projectId: lethe`, `reportPath: 20260609/units/...html` 확인.
- `npm run report:orchestrator:unit:dry`: 통과, Project Orchestrator가 Discord embed payload와 HTML attachment 경로를 생성했다.
- `npm run report:orchestrator:unit`: 통과, Orchestrator 응답 `accepted: true`, `sent: true`, attachment `sent: true`.

## 4. 결정한 것

- 정상 Discord 보고는 Project Orchestrator 중앙 intake를 기본 경로로 사용한다.
- 기존 `npm run report:discord:unit`은 trusted-local fallback으로 유지한다.
- 다른 프로젝트는 `report:orchestrator:*` 명령을 복사하거나 동등한 script를 만들어야 한다.

## 5. 문제 또는 리스크

- Project Orchestrator가 실행 중이어야 중앙 intake가 동작한다.
- `projectId`는 Orchestrator의 등록 id와 일치해야 한다.
- 직접 webhook fallback은 여전히 민감 정보 전송 경로이므로 명시 요청이 있을 때만 사용한다.

## 6. GPT/Claude 인계 요약

LETHE는 이제 Project Orchestrator Discord intake를 실제로 호출한다. dry-run과 실제 전송 모두 통과했고, Discord에는 Orchestrator가 만든 embed와 HTML attachment가 전송됐다.

## 7. 다음 Codex 작업

- 다른 프로젝트에 마이그레이션할 때 `report:orchestrator:*` 명령과 `docs/orchestration/reports/YYYYMMDD/index.html` 구조를 같이 만든다.
- Project Orchestrator가 꺼져 있으면 먼저 Orchestrator를 실행한 뒤 dry-run을 확인한다.

## 8. 포트폴리오 메모

- 문제: Discord 규약은 있었지만 프로젝트가 실제로 Orchestrator API를 호출하지 않았다.
- 방향: per-project webhook 전송 대신 중앙 Orchestrator intake를 기본 보고 경로로 만든다.
- 행동: 전송 스크립트, npm 명령, 문서 규약을 연결하고 dry-run/실제 전송을 검증했다.
- 결과: LETHE의 작업 단위 보고가 Project Orchestrator를 통해 Discord로 실제 전송됐다.
