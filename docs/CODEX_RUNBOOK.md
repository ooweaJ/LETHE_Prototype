# Codex Runbook

This project now uses a normalized Codex workflow: implement the HTML prototype, verify, record, notify, commit, and push when safe.

The project goal is to validate LETHE's core fun and player-facing possibility in HTML first. If the prototype shows enough promise through AI and human tests, the next major phase is Unity implementation.

## Normal Work Loop

1. Run autopilot preflight before any unattended version-up loop.
2. Read the current files and project notes before editing.
3. Implement the smallest coherent unit.
4. Run the relevant checks.
5. Update `docs/CODEX_STATUS.md`, `docs/NEXT_TASKS.md`, `docs/devlog/YYYY-MM-DD.md`, and `docs/reports/YYYY-MM-DD.md`.
6. Generate the HTML report from the Markdown report.
7. Send a Discord handoff when useful.
8. Commit with a Conventional Commit message.
9. Push only when the working tree is clean and the commit is safe to share.

## Autopilot Preflight

Use this before the implement -> test -> Claude report -> implement loop, especially when the user is stepping away.

Dry-run:

```powershell
npm run autopilot:preflight:dry
```

Local checks without live Claude transmission:

```powershell
npm run autopilot:preflight:local
```

Full preflight with a minimal Claude authentication check:

```powershell
npm run autopilot:preflight
```

The full preflight checks:

- clean git working tree,
- required npm scripts,
- `npm run doctor:deep`,
- Claude/Codex dry-runs,
- Discord work-unit report dry-run,
- `claude --version`,
- `codex --version` fallback readiness,
- `.env` Discord webhook presence,
- a minimal non-project Claude prompt to catch login/auth failures early.

Preflight failures are blockers. Do not start a long automation loop if Claude authentication, fallback readiness, dirty git state, or report notification setup is already known to be broken. Fix the failed item, or generate the prompt/report and ask the user to run the blocked external command from a trusted local terminal.

## Overnight Loop

Use this when the user wants Codex to keep the planning and verification cycle moving while they are away. The loop is intentionally evidence-first: it runs preflight, AI/planning checks, reports, and optional implementation commands, then writes a Markdown log for the next handoff.

Dry-run:

```powershell
npm run overnight:loop:dry
```

One planning/verification loop:

```powershell
npm run overnight:loop
```

Longer loop:

```powershell
node scripts/run_overnight_loop.js --iterations 3 --sleep-minutes 20
```

The default loop uses:

```text
docs/review_prompts/2026-06-02-v09-release-feel-loop.md
```

and writes logs to:

```text
docs/loop_runs/
```

Default behavior does not let an external model edit project files. If a trusted local terminal should run an implementation command during the loop, pass it explicitly:

```powershell
node scripts/run_overnight_loop.js --implement-cmd "your safe implementation command"
```

If any required step fails, the loop writes a blocker prompt under `docs/review_prompts/YYYY-MM-DD-overnight-loop-blocker-N.md` and stops. The next Codex session should read the loop log, blocker prompt, latest planning response, and `docs/CODEX_STATUS.md`.

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

For small follow-up checks, a single provider can be enough. For major prototype direction changes, combat/core redesign, human-test readiness, or Unity transition decisions, use the fixed double-check pipeline: Claude plus Codex CLI.

Role split for double checks:

- Claude: player emotion, pacing, regret/irritation, whether the loop sounds fun.
- Codex CLI: systems design, balance model risk, implementation order, testability.
- Current Codex: read both, summarize common points and conflicts, then implement only the selected scope.

Decision priority:

1. User live play feedback.
2. Browser combat or human-test evidence.
3. Automated proxy metrics.
4. Claude/Codex planning opinions.

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
npm run autopilot:preflight:local
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

Actual double-check pipeline:

```powershell
npm run planning:pipeline
```

Explicit double-check pipeline:

```powershell
npm run planning:pipeline:double
```

Legacy Claude-first fallback pipeline:

```powershell
npm run planning:pipeline:auto
```

The pipeline runs a quick AI test by default, writes a fresh prompt to:

```text
docs/review_prompts/YYYY-MM-DD-pipeline.md
```

By default, it asks both Claude and Codex CLI. Responses and the double-check handoff are saved to:

```text
docs/review_responses/YYYY-MM-DD-pipeline-claude.md
docs/review_responses/YYYY-MM-DD-pipeline-codex.md
docs/review_responses/YYYY-MM-DD-pipeline-double-check.md
```

After the responses are saved, Codex should read both, update the double-check summary with common points and conflicts, update `docs/NEXT_TASKS.md`, implement the selected work, verify, report, commit, and push when safe.

If the current Codex session cannot export repository prompts to external services, use `planning:pipeline:prompt` and run `npm run planning:pipeline` from the user's trusted local terminal.

## Human Playtest Summary

Before human sessions, prepare a static playtest folder:

```powershell
npm run playtest:package:dry
npm run playtest:package
```

After human sessions, put downloaded JSON logs in:

```text
playtest_logs/
```

Then run:

```powershell
npm run playtest:summary:dry
npm run playtest:summary
```

This writes:

```text
docs/playtest_summaries/YYYY-MM-DD.md
docs/review_prompts/YYYY-MM-DD-human-playtest.md
```

Use the generated human-test prompt as the next Claude/GPT planning input before HTML v0.6 or Unity transition decisions.

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
