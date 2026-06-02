# AI Failure Cases

이 문서는 AI 또는 자동화가 틀렸거나 막힌 지점을 기록한다. 목적은 “AI를 사용했다”가 아니라 “AI와 자동화의 한계를 개발자가 어떻게 통제했는지”를 보여주는 것이다.

## Case 01. External Claude Prompt Transfer Blocked

### AI/Automation Attempt

Codex attempted to run the planning pipeline and send the generated project prompt to Claude automatically.

### Problem

The current Codex session blocked exporting repository prompt content to an external service.

### Cause

The prompt contained project status and test summaries. External model calls are not always allowed from the active Codex environment.

### Developer Fix

- Added `npm run planning:pipeline:prompt`.
- This safe path runs the AI quick test and generates `docs/review_prompts/YYYY-MM-DD-pipeline.md` without external transmission.
- The user can run `npm run planning:pipeline` from a trusted local terminal where external calls and Claude authentication are available.

### Verification

- `npm run planning:pipeline:dry`: previewed prompt and provider route.
- `npm run planning:pipeline:prompt`: generated `docs/review_prompts/2026-06-02-pipeline.md`.
- User later ran the actual Claude step and saved `docs/review_responses/2026-06-02-pipeline-claude.md`.

### Lesson

External AI calls must be treated as an explicit boundary. The local automation should always provide a prompt-only fallback.

## Case 02. Browser Plugin QA Failed, Chrome Headless Replaced It

### AI/Automation Attempt

Codex attempted to use the in-app Browser plugin to verify v0.5 level-up UI and JSON payload behavior.

### Problem

The Browser plugin failed with a Windows sandbox startup error.

### Cause

The issue was tooling/runtime startup, not game code.

### Developer Fix

- Added `?qa=fast,levelup` automated QA mode in `src/game.js`.
- Ran installed Chrome headless directly.
- Read page state through Chrome DevTools Protocol when dataset verification was needed.

### Verification

- `levelUpSeen: true`.
- `resumedAfterUpgrade: true`.
- `hasRunGrowthPayload: true`.
- `choicesTaken` matched `runGrowth.choicesTaken`.
- Later metadata QA also confirmed `playtest.testerId: T01` and `playtest.sessionId: S01`.

### Lesson

When one AI/browser tool fails, keep the test goal fixed and replace only the execution path.

## Case 03. Claude Role Was Too Close To Generic Review

### AI/Automation Attempt

Early automation described Claude as a review helper.

### Problem

The user clarified that Claude should not merely review. Claude should receive AI/human test results, interpret them, revise planning, and decide what Codex should implement next.

### Cause

The workflow language made the system sound like “ask AI for feedback” rather than “use AI as a controlled planning partner after evidence is collected.”

### Developer Fix

- Updated `AGENTS.md`, `CLAUDE.md`, `docs/CODEX_RUNBOOK.md`, `docs/CODEX_STATUS.md`, and `docs/NEXT_TASKS.md`.
- Reframed the goal as HTML prototype validation before possible Unity implementation.
- Added `planning:pipeline` to connect test evidence to planning response files.

### Verification

- `npm run planning:pipeline:dry`.
- `npm run review:claude:dry`.
- `npm run review:codex:dry`.
- Reports now record test evidence, Claude verdicts, implementation decisions, and next gates.

### Lesson

AI collaboration value comes from evidence-driven planning and developer judgment, not from generic AI review.

## Case 04. OpenAI API Fallback Was Over-Expanded

### AI/Automation Attempt

The automation briefly had Claude, Codex CLI, and OpenAI API fallback paths.

### Problem

The third fallback added operational complexity and did not match the user's intended workflow.

### Cause

The automation optimized for availability instead of the user's actual decision process.

### Developer Fix

- Removed the OpenAI API fallback.
- Kept Claude first, Codex CLI second.
- Updated docs and package scripts.

### Verification

- OpenAI fallback script and npm aliases removed.
- Runbook and status docs now describe the two-step review order.

### Lesson

More automation paths are not automatically better. The best AI workflow is the smallest one the project can operate reliably.

## Case 05. AI Balance Proxy Was Mistaken For Real Balance Feedback

### AI/Automation Attempt

Codex implemented v0.7 from the user's v0.6 feedback that memory loss made the player too weak to kill normal mobs. Codex then sent Claude an evaluation prompt containing AI simulator results, browser functional QA, and a summary of the weapon/echo changes.

### Problem

The user played v0.7 and reported that the balance was not close. The earlier report made it sound like the project had received meaningful balance feedback, but the feedback was only a proxy-based planning judgment.

### Cause

- Claude did not play the browser build.
- The AI simulator used aggregate DPS/survival proxies rather than actual browser combat pressure.
- Functional browser QA verified boss/forget/refill flow, not whether the combat felt balanced.
- The v0.7 prompt over-weighted passing AI metrics and under-weighted the known risk that human feel could invalidate the numbers.

### Developer Fix

- Treat the user's live play feedback as the highest-priority balance signal.
- Mark v0.7 balance as invalidated by human evidence.
- Create a v0.7.1 reality-check prompt that asks planning AI to judge the mismatch, not rubber-stamp AI metrics.
- Future balance reports must distinguish `AI proxy passed`, `browser flow passed`, and `human balance passed`.

### Verification

- User direct feedback: "밸런스가 1도 안 맞는다."
- Code inspection showed that actual browser deficit pressure increases spawn pressure when active memories drop below 3, while the AI simulator only estimates survival from abstract power ratios.

### Lesson

AI balance metrics are only scouting tools. They cannot be reported as real balance feedback until live play or a browser-play balance bot validates the actual combat loop.

## Template

Use this template for future cases:

```markdown
## Case NN. Title

### AI/Automation Attempt

What did AI or automation propose/do?

### Problem

What failed, was wrong, or was risky?

### Cause

Why did it happen?

### Developer Fix

What did the developer/Codex change?

### Verification

What command, test, or human evidence proved the fix?

### Lesson

What should future work remember?
```
