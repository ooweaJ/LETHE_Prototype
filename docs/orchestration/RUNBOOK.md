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
npm run report
npm run report:check
npm run report:discord:unit:dry
npm run report:discord:unit
```

If actual Discord send is blocked by policy, webhook, network, or permissions, record the exact reason and next trusted-local command in devlog/report.

## Orchestration HTML

Current human-facing pages:

```text
docs\orchestration\index.html
docs\orchestration\command.html
docs\orchestration\runbook.html
docs\orchestration\reports\index.html
docs\orchestration\devlog\index.html
```

Rules:

- Markdown is the source of truth.
- HTML is the Korean human-facing project interface for the user.
- `index.html` shows the last completed state, recommended next prompt, blockers, evidence, and document links.
- `command.html` shows the compact next instruction.
- `runbook.html` shows repeated commands and operating procedures.
- Until a generator exists, update the HTML manually when orchestration status changes.

## Autopilot

Before unattended implement -> test -> review -> implement loops:

```bash
npm run autopilot:preflight
```

If external Claude transmission is not allowed:

```bash
npm run autopilot:preflight:local
```

Preflight failures are blockers.

## Git

Use Conventional Commits. After a coherent verified unit:

```bash
git status --short
git add <files>
git commit -m "docs: ..."
git push
```

Do not push if the tree is dirty with unrelated or unsafe changes.
