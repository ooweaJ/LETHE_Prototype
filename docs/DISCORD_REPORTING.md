# Discord Report Delivery

LETHE daily reports are written in Markdown and rendered to HTML. The Discord integration uploads the generated HTML file to a Discord channel through a channel webhook.

## Channel Setup

1. Create a Discord channel, for example `#daily-report`.
2. Open channel settings.
3. Go to Integrations > Webhooks.
4. Create a webhook and copy its URL.
5. Store the URL locally as `DISCORD_WEBHOOK_URL`.

Do not commit the webhook URL. `.env` files are ignored by Git, and the script reads `.env` automatically if it exists.

`.env` example:

```text
DISCORD_WEBHOOK_URL=https://discord.com/api/webhooks/...
```

PowerShell example:

```powershell
$env:DISCORD_WEBHOOK_URL="https://discord.com/api/webhooks/..."
npm run report:discord
```

Dry-run preview without uploading:

```powershell
npm run report:discord:dry
```

## Project Flow

```powershell
npm run report
npm run report:discord
```

`npm run report:discord` rebuilds the HTML report from `docs/reports/2026-06-01.md`, then uploads `docs/reports/2026-06-01.html` as a Discord attachment.

Discord does not render attached HTML inside the chat message. The channel message shows a short report summary and the HTML file as an attachment.
