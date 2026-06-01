# Codex Runbook

This project now uses a normalized Codex workflow: implement, verify, record, notify, commit, and push when safe.

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

## GPT And Claude Review

GPT is the default reviewer for system direction, implementation priority, and test criteria.

Claude is useful when the question is emotional feel, wording, narrative tone, or whether forgetting feels regrettable instead of irritating.

Do not send every prompt to both by default. Send to both only when the decision affects the prototype direction or human-test readiness.

## Claude Code Automation

Claude Code can be called from the terminal for non-interactive planning review.

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

## OpenAI Review Fallback

If Claude Code is installed but blocked by login, OAuth, or keychain state, use the OpenAI API fallback.

Dry-run:

```powershell
npm run review:openai:dry
```

Actual review:

```powershell
npm run review:openai
```

The script reads the latest dated file in `docs/review_prompts/` and writes the response to:

```text
docs/review_responses/YYYY-MM-DD-openai.md
```

It requires `OPENAI_API_KEY` in the environment or local `.env`. The default model is `gpt-5.2`; set `OPENAI_REVIEW_MODEL` to override it.
