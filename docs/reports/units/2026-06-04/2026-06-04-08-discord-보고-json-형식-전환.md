# 2026-06-04-08 - Discord 보고 JSON 형식 전환

## 1. 현재 빌드 상태

- Discord 작업 단위 보고는 최신 보고 단위 HTML을 첨부하는 구조를 유지한다.
- 이번 변경으로 Discord 본문은 줄글 요약이 아니라 JSON code block으로 표시된다.
- 상세 기록은 계속 HTML 첨부 파일에서 확인한다.

## 2. 오늘 바뀐 것

- `scripts/send_discord_report.js`의 메시지 본문을 JSON 요약으로 바꿨다.
- Discord 첨부에 JSON 요약 파일을 추가했다.
- 첨부 순서는 JSON 요약, HTML 상세 보고서, 선택적 review prompt다.
- `docs/DISCORD_REPORTING.md`의 메시지 형식 설명을 JSON 기준으로 갱신했다.

## 3. 테스트 결과와 근거

- `node --check scripts/send_discord_report.js`: 통과.
- `npm run report:discord:unit:dry`: JSON 본문 확인.
- dry-run에서 JSON 첨부 파일과 HTML 첨부 파일이 모두 표시되는 것을 확인했다.

## 4. 결정한 것

- 앞으로 Discord 본문은 JSON code block을 기본 형식으로 쓴다.
- Discord에는 짧은 구조화 요약만 두고, 긴 내용은 HTML 첨부로 본다.
- review prompt가 있으면 기존처럼 추가 첨부한다.

## 5. 문제 또는 리스크

- Discord content 길이 제한 때문에 JSON 본문은 짧게 잘라야 한다.
- 전체 상세 내용은 JSON 본문이 아니라 HTML 보고서에서 확인해야 한다.
- JSON 첨부는 전송 시점에 생성되는 요약 파일이며 Markdown source of truth를 대체하지 않는다.

## 6. GPT/Claude 인계 요약

- 보고 포맷만 바꿨고 게임/밸런스 로직은 바꾸지 않았다.
- 다음 AI 검토는 기존처럼 Markdown/HTML 보고서와 review prompt를 기준으로 하면 된다.
- Discord 본문은 사람이 빠르게 상태를 확인하는 구조화 요약으로 사용한다.

## 7. 다음 Codex 작업

- 새 JSON 형식으로 실제 Discord 전송을 확인한다.
- 이후 모든 의미 있는 작업 단위는 JSON 요약 + HTML 첨부 형식으로 전송한다.
- 필요하면 자동 루프 notice도 같은 JSON 형식으로 맞춘다.

## 8. 포트폴리오 노트

- 문제: Discord 줄글 요약은 길고 스캔성이 낮았다.
- 방향: 본문을 JSON 요약으로 구조화하고 상세는 HTML 첨부로 분리한다.
- 행동: Discord 보고 스크립트와 보고 문서를 수정했다.
- 결과: 보고 채널에서 작업 상태를 더 깔끔하게 확인할 수 있다.
