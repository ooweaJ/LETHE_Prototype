# Discord 보고서 전송

LETHE 보고서는 Markdown으로 작성하고 HTML로 생성한다. Discord에는 긴 원문을 그대로 붙이지 않고, 짧은 한국어 요약과 HTML 첨부 파일을 보낸다.

## 채널 설정

1. Discord에 `#daily-report` 같은 채널을 만든다.
2. 채널 설정을 연다.
3. Integrations > Webhooks로 이동한다.
4. Webhook을 만들고 URL을 복사한다.
5. 프로젝트 루트의 로컬 `.env`에 URL을 저장한다.

`.env`는 Git에 올라가지 않는다.

```text
DISCORD_WEBHOOK_URL=https://discord.com/api/webhooks/...
```

## 사용 방법

미리보기:

```powershell
npm run report:discord:dry
```

실제 전송:

```powershell
npm run report:discord
```

기본 명령은 `docs/reports/` 안의 최신 날짜 Markdown 보고서를 자동으로 선택한다.

특정 보고서를 보내고 싶을 때:

```powershell
node scripts/build_report.js docs/reports/2026-06-02.md
node scripts/send_discord_report.js docs/reports/2026-06-02.md
```

## Discord 메시지 형식

Discord 본문은 아래 네 줄 중심으로 짧게 유지한다.

- `작업`: 오늘 무엇을 했는지.
- `완료`: 어디까지 끝났는지.
- `문제`: 막힌 점이나 리스크.
- `기획질문`: GPT/Claude 검토가 필요한지.

전체 기록은 HTML 첨부 파일에서 확인한다.

## GPT/Claude 프롬프트

기획 검토가 필요하면 HTML 보고서 안에 프롬프트를 묻어두지 말고 별도 Markdown 파일로 둔다.

```text
docs/review_prompts/YYYY-MM-DD.md
```

이 파일이 있으면 Discord 보고서 전송 시 자동으로 함께 첨부된다.

프롬프트는 기본적으로 한국어로 작성하고, 답변 형식에는 `앞으로 해야 할 일`을 포함한다.
