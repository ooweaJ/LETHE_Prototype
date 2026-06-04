# 2026-06-04-08 - Discord 보고 Orchestrator 형식 전환

## 1. 현재 빌드 상태

- Discord 작업 단위 보고는 최신 보고 단위 HTML을 첨부하는 구조를 유지한다.
- 이번 변경으로 Discord 본문은 AI Project Orchestrator식 진행 상태 메시지로 표시된다.
- 구조화 요약은 `.summary.json` 첨부에서 확인하고, 상세 기록은 HTML 첨부 파일에서 확인한다.

## 2. 오늘 바뀐 것

- `scripts/send_discord_report.js`의 메시지 본문을 Orchestrator식 진행 상태 요약으로 바꿨다.
- 실제 Discord 전송을 두 메시지로 분리했다.
- 첫 메시지는 진행 상태 요약과 `.summary.json` 파일을 보낸다.
- 둘째 메시지는 `상세 HTML 보고서 파일입니다.` 문구와 HTML 상세 보고서, 선택적 review prompt를 보낸다.
- `docs/DISCORD_REPORTING.md`의 메시지 형식 설명을 현재 전송 형식에 맞게 갱신했다.

## 3. 테스트 결과와 근거

- `node --check scripts/send_discord_report.js`: 통과.
- `npm run report:discord:unit:dry`: Orchestrator식 본문 확인.
- dry-run에서 `.summary.json` 첨부와 HTML 첨부가 분리 표시되는 것을 확인했다.
- `npm run report:discord:unit`: 현재 Codex 승인 검토에서 외부 Discord webhook 업로드 위험으로 차단됐다. 우회 전송은 하지 않았고, trusted local에서 같은 명령을 직접 실행하면 된다.

## 4. 결정한 것

- 앞으로 Discord 본문은 Orchestrator식 진행 상태 요약을 기본 형식으로 쓴다.
- Discord에는 짧은 사람이 읽는 상태 요약만 두고, 구조화 데이터는 JSON 첨부, 긴 내용은 HTML 첨부로 본다.
- review prompt가 있으면 기존처럼 추가 첨부한다.

## 5. 문제 또는 리스크

- Discord content 길이 제한 때문에 본문은 짧게 유지해야 한다.
- 전체 상세 내용은 Discord 본문이 아니라 JSON/HTML 첨부에서 확인해야 한다.
- JSON 첨부는 전송 시점에 생성되는 요약 파일이며 Markdown source of truth를 대체하지 않는다.
- 현재 Codex 세션에서는 외부 Discord webhook 실제 전송이 승인 검토에서 차단될 수 있다.

## 6. GPT/Claude 인계 요약

- 보고 포맷만 바꿨고 게임/밸런스 로직은 바꾸지 않았다.
- 다음 AI 검토는 기존처럼 Markdown/HTML 보고서와 review prompt를 기준으로 하면 된다.
- Discord 본문은 사람이 빠르게 상태를 확인하는 진행 상태 요약으로 사용한다.

## 7. 다음 Codex 작업

- trusted local에서 `npm run report:discord:unit`로 새 Orchestrator식 형식의 실제 Discord 전송을 확인한다.
- 이후 모든 의미 있는 작업 단위는 진행 상태 요약 + JSON 첨부 + HTML 첨부 형식으로 전송한다.
- 필요하면 자동 루프 notice도 같은 진행 상태 형식으로 맞춘다.

## 8. 포트폴리오 노트

- 문제: Discord 줄글 요약은 길고 스캔성이 낮았다.
- 방향: 본문은 Orchestrator식 진행 상태로 정리하고, 구조화 데이터와 상세 HTML을 첨부로 분리한다.
- 행동: Discord 보고 스크립트와 보고 문서를 수정했다.
- 결과: 보고 채널에서 작업 상태를 더 깔끔하게 확인할 수 있다.
