# Codex Runbook

This project now uses a normalized Codex workflow: implement the HTML prototype, verify, record, notify, commit, and push when safe.

The project goal is to validate LETHE's core fun and player-facing possibility in HTML first. If the prototype shows enough promise through AI and human tests, the next major phase is Unity implementation.

## Normal Work Loop

1. Read the current files and project notes before editing.
2. Implement the smallest coherent unit.
3. Run the relevant checks.
4. Update `docs/CODEX_STATUS.md`, `docs/NEXT_TASKS.md`, `docs/devlog/YYYY-MM-DD.md`, and `docs/reports/YYYY-MM-DD.md`.
5. Generate the HTML report from the Markdown report.
6. Send a Discord handoff when useful.
7. Commit with a Conventional Commit message.
8. Push only when the working tree is clean and the commit is safe to share.

## Discord Notices

Daily reports use:

```powershell
npm run report:discord
```

Work-unit reports use the latest top-level section in the current daily report:

```powershell
npm run report:discord:unit:dry
npm run report:discord:unit
```

Use a work-unit report after a coherent version-up, automation change, AI test milestone, or other user-facing result. The Markdown source still stays in `docs/reports/YYYY-MM-DD.md`; the Discord summary can target only the relevant section.

Short Codex status notices use:

```powershell
node scripts/send_codex_notice.js --dry-run --type=status --title="v0.2 work" --summary="Working on planning gate"
node scripts/send_codex_notice.js --type=done --title="Workflow normalized" --summary="Runbook and checkpoint rules updated"
```

The npm aliases `npm run codex:notice:dry` and `npm run codex:notice` are available for generic notices. Use the direct `node` command when passing custom fields on Windows.

Notice types:

- `start`: work started.
- `status`: progress update.
- `approval`: permission approval may be needed in the Codex UI.
- `checkpoint`: context or token safety checkpoint.
- `blocked`: work cannot continue without input.
- `done`: coherent unit completed.

Discord can alert the user that Codex may need attention, but approval still happens in the Codex UI.

## Approval Reality

Permission prompts pause execution. They cannot be clicked from Discord.

Reduce interruptions by pre-approving common safe commands when prompted:

- `npm run report`
- `npm run report:discord`
- `npm run report:discord:dry`
- `npm run codex:notice`
- `npm run codex:notice:dry`
- `node --check`
- `git add`
- `git commit`
- `git push`

Risky actions should still require explicit approval:

- destructive file operations
- history rewriting
- dependency installation
- broad network actions
- writes outside the project

## Context And Token Safety

If work may be interrupted, record a checkpoint before stopping:

- completed work
- current incomplete step
- files touched
- checks already run
- checks not yet run
- next exact commands or tasks
- whether Discord/Git/report steps are still pending

Use `docs/checkpoints/YYYY-MM-DD.md` when the interruption is significant.

## GPT And Claude Planning

GPT/Claude are not just generic reviewers. They are planning partners used after AI or human test results are available.

Claude is especially useful for interpreting playtest reactions, revising the design direction, deciding what Codex should implement next, and judging whether the HTML prototype is approaching a Unity-worthy shape.

Do not send every prompt to both by default. Send to them when the decision affects prototype direction, human-test readiness, or the eventual Unity transition.

## Claude Code Automation

Claude Code can be called from the terminal for non-interactive planning iteration after test results.

Dry-run:

```powershell
npm run review:claude:dry
```

Actual review:

```powershell
npm run review:claude
```

The script reads the latest dated file in `docs/review_prompts/` and writes the response to:

```text
docs/review_responses/YYYY-MM-DD-claude.md
```

Claude is called with tools disabled, so it should only answer the planning prompt. Codex remains responsible for file edits, tests, reports, commits, and pushes.

If Claude exits with `401 Invalid authentication credentials`, run `claude` in a local terminal and complete login/authentication, then retry `npm run review:claude`.

## Planning Pipeline

Use this when the current build has AI or human test evidence and the next design direction should be decided before more implementation.

Before using a new local machine, run:

```powershell
npm run doctor
npm run doctor:deep
```

`doctor` checks required tools, npm scripts, and role/rule docs. `doctor:deep` also runs safe dry-runs so missing local setup is visible before a long unattended task.

Dry-run:

```powershell
npm run planning:pipeline:dry
```

Prompt-only, no external model call:

```powershell
npm run planning:pipeline:prompt
```

Actual pipeline:

```powershell
npm run planning:pipeline
```

The pipeline runs a quick AI test by default, writes a fresh prompt to:

```text
docs/review_prompts/YYYY-MM-DD-pipeline.md
```

Then it asks Claude first and falls back to Codex CLI if Claude fails. Responses are saved to:

```text
docs/review_responses/YYYY-MM-DD-pipeline-claude.md
docs/review_responses/YYYY-MM-DD-pipeline-codex.md
```

After the response is saved, Codex should read it, update `docs/NEXT_TASKS.md`, implement the selected work, verify, report, commit, and push when safe.

If the current Codex session cannot export repository prompts to external services, use `planning:pipeline:prompt` and run `npm run planning:pipeline` from the user's trusted local terminal.

## Codex CLI Review Fallback

Use this when the user wants a GPT/Codex terminal answer without manually setting an OpenAI API key.

Dry-run:

```powershell
npm run review:codex:dry
```

Actual review:

```powershell
npm run review:codex
```

The script reads the latest dated file in `docs/review_prompts/` and writes the final Codex message to:

```text
docs/review_responses/YYYY-MM-DD-codex.md
```

It calls `codex exec` in read-only mode with approvals disabled, so it should not edit files. It uses the local Codex CLI login/session and any applicable ChatGPT/Codex plan limits. Set `CODEX_REVIEW_MODEL` to override the model.
