# Runbook

## Health Checks

```bash
npm run doctor
npm run doctor:deep
```

## Playtest Package

```bash
npm run playtest:package:dry
npm run playtest:package
```

Generated package:

```text
dist\lethe-v0.12-playtest
```

## Human Test Summary

```bash
npm run playtest:summary
```

Expected input:

```text
playtest_logs/
```

## Balance QA

```bash
npm run qa:balance
npm run balance:loop
npm run qa:boss-ttk
```

Current balance source of truth is the loop result, not a one-off run.

## Reports

```bash
npm.cmd run report
npm.cmd run report:check
```

`npm run report` regenerates the current date-folder report HTML and unit pages from Markdown.

It also regenerates `docs/orchestration/reports/index.html` as a newest-first date archive. The archive should link primarily to `reports/YYYYMMDD/index.html`, not flatten every unit page.

For generated daily reports, `docs/orchestration/reports/YYYYMMDD/index.html` should show cards for that day's units. Open a card to read one unit, then use the back link to return to the date page.

## External Notification

Preferred rule:

```text
Submit a short Korean summary, the finished report path, optional attachment path, and source file list to Project Orchestrator's central Discord intake.
```

Project Orchestrator owns the shared Discord webhook and sends the notification/attachment. This LETHE repository should not store or depend on project-local Discord secrets as the normal path.

Project Orchestrator dry-run:

```bash
npm run report:orchestrator:dry
npm run report:orchestrator:unit:dry
```

Project Orchestrator real submit:

```bash
npm run report:orchestrator
npm run report:orchestrator:unit
```

Trusted-local fallback, only when explicitly requested or when the central intake is unavailable:

```bash
npm run report:discord:unit:dry
npm run report:discord:unit
```

If actual Discord send is blocked by policy, webhook, network, permissions, or missing Project Orchestrator intake, record the exact reason and next trusted-local command in devlog/report.

For the current Unity development phase, send a Discord notification after meaningful document, resource, Unity MCP, or C# implementation units. Do not wait until multiple units have accumulated if the work changes the next step or the active rules.

## Orchestration HTML

Current human-facing pages:

```text
docs\orchestration\interface\index.html
docs\orchestration\interface\command.html
docs\orchestration\interface\runbook.html
docs\orchestration\reports\index.html
docs\orchestration\devlog\index.html
```

Rules:

- Markdown is the source of truth.
- HTML is the Korean human-facing project interface for the user.
- `interface/index.html` shows the 30-second project state: current stage, verification, blocker, next gate, recent completion, and key links.
- `interface/command.html` shows the compact next instruction.
- `interface/runbook.html` shows repeated commands and operating procedures.
- `reports/YYYYMMDD/index.html` is the user-facing daily report page.
- `reports/YYYYMMDD/units/` contains generated work-unit details and JSON summaries.
- Until a generator exists, update the HTML manually when orchestration status changes.

## Legacy HTML Autopilot

HTML autopilot is legacy. Do not start it unless the user explicitly asks to return to HTML prototype automation.

Historical command:

```bash
npm run autopilot:preflight
```

If external Claude transmission is not allowed:

```bash
npm run autopilot:preflight:local
```

Preflight failures are blockers.

## Git

Use Conventional Commits unless the user explicitly says not to commit. After a coherent verified unit:

```bash
git status --short
git add <files>
git commit -m "docs: ..."
git push
```

Do not push if the tree is dirty with unrelated or unsafe changes.
