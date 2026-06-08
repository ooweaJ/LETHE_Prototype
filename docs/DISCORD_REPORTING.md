# Discord 보고 규약

LETHE의 Discord 보고는 오케스트레이션 리포트를 외부 알림 채널로 보내는 절차다. Discord는 source of truth가 아니다. source of truth는 `docs/orchestration/` 아래 Markdown과 생성된 HTML이다.

## 기준 경로

```text
docs/orchestration/reports/
  index.html
  YYYYMMDD/
    index.md
    index.html
    units/
      YYYY-MM-DD-NN-slug.md
      YYYY-MM-DD-NN-slug.html
      YYYY-MM-DD-NN-slug.summary.json

docs/orchestration/review_prompts/
docs/orchestration/review_responses/
```

- 사람용 일일 보고 원본: `docs/orchestration/reports/YYYYMMDD/index.md`
- 사람용 일일 보고 HTML: `docs/orchestration/reports/YYYYMMDD/index.html`
- Discord 작업 단위 첨부: `docs/orchestration/reports/YYYYMMDD/units/*.html`
- Discord 구조화 요약 첨부: `docs/orchestration/reports/YYYYMMDD/units/*.summary.json`
- 기획 검토 프롬프트: `docs/orchestration/review_prompts/YYYY-MM-DD-topic.md`
- 기획 검토 응답: `docs/orchestration/review_responses/YYYY-MM-DD-topic-provider.md`

기존 `docs/reports/`, `docs/devlog/`, `docs/review_prompts/`, `docs/review_responses/`는 마이그레이션 전 레거시 기록으로만 본다. 새 작업은 그 경로에 만들지 않는다.

## 채널 설정

1. Discord에 보고용 채널을 만든다.
2. 채널 설정에서 Integrations > Webhooks로 이동한다.
3. Webhook URL을 만든다.
4. 프로젝트 루트의 로컬 `.env`에 URL을 저장한다.

`.env`는 Git에 올리지 않는다.

```text
DISCORD_WEBHOOK_URL=https://discord.com/api/webhooks/...
```

## 사용 명령

일일 보고 HTML과 작업 단위 파일을 재생성한다.

```powershell
npm run report
```

보고서 형식과 생성된 unit 파일을 검사한다.

```powershell
npm run report:check
```

Discord 작업 단위 보고를 미리 본다.

```powershell
npm run report:discord:unit:dry
```

Discord 작업 단위 보고를 실제 전송한다.

```powershell
npm run report:discord:unit
```

특정 날짜 보고서를 지정할 때:

```powershell
node scripts/build_report.js docs/orchestration/reports/20260608/index.md
node scripts/send_discord_report.js docs/orchestration/reports/20260608/index.md --latest-section
```

특정 작업 단위를 지정할 때:

```powershell
node scripts/send_discord_report.js docs/orchestration/reports/20260608/index.md --section "2026-06-08-10 - 오케스트레이션 리포트와 개발로그 실제 마이그레이션"
```

## Discord 메시지 형식

Discord에는 긴 원문을 그대로 붙이지 않는다.

- 첫 메시지: `LETHE Prototype 진행 상태`, 기준일, 어떤 작업, 진행 내용, 결과.
- 첫 첨부: `.summary.json` 구조화 요약.
- 둘째 메시지: `상세 HTML 보고서 파일입니다.`
- 둘째 첨부: 작업 단위 `.html` 보고서와 선택적 review prompt.

정확한 기록은 `docs/orchestration/reports/YYYYMMDD/index.md`와 생성된 HTML에 남긴다.

## 실패 처리

Codex 세션에서는 외부 Discord webhook 전송이 approval reviewer에 의해 차단될 수 있다. 이 경우 우회하지 않는다.

해야 할 일:

- `npm run report:discord:unit:dry` 결과로 본문과 첨부 대상을 확인한다.
- 실제 전송이 차단되면 실패 원인과 다음 trusted-local 명령을 `docs/orchestration/devlog/YYYYMMDD.md`와 `docs/orchestration/reports/YYYYMMDD/index.md`에 기록한다.
- 다음 trusted-local 명령은 보통 `npm run report:discord:unit`이다.

## Review Prompt 연동

기획 검토가 필요하면 HTML 보고서 안에 프롬프트를 묻지 않는다. 별도 Markdown 파일을 둔다.

```text
docs/orchestration/review_prompts/YYYY-MM-DD-topic.md
```

스크립트는 새 경로를 우선 확인하고, 필요한 경우에만 레거시 `docs/review_prompts/`를 fallback으로 읽을 수 있다. 새 프롬프트와 새 응답은 항상 `docs/orchestration/` 아래에 작성한다.

Claude 답변은 Discord가 아니라 아래 경로에 저장한다.

```text
docs/orchestration/review_responses/YYYY-MM-DD-topic-claude.md
```

Discord는 답변 도착, 구현 시작, 완료 같은 상태 알림 또는 작업 단위 보고에만 사용한다.
