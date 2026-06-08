# 2026-06-08-06 - Project Dashboard Compaction

## 1. Current build status

LETHE v0.12 remains the current controlled human-test candidate. This work changed only orchestration documentation and HTML.

## 2. What changed today

- Rebuilt `docs/orchestration/index.html` as a shorter 30-second project dashboard.
- Removed duplicated next-instruction content that belongs in `command.html`.
- Removed broad document browsing and Codex-comparison copy from the main dashboard.
- Kept the dashboard focused on:
  - current state,
  - latest verification,
  - blocker,
  - next gate,
  - recent completion,
  - links to command/runbook/reports.
- Updated orchestration Markdown docs to define the role split.

## 3. Test results and evidence

- `npm run report`: pass, generated 6 unit reports.
- `npm run report:check`: pass, 6 report units.
- `npm run report:discord:unit:dry`: pass, latest unit 06 summary generated.
- `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- `npm run report:discord:unit`: blocked by approval reviewer because it would send workspace-generated report content and attachments to an external Discord webhook without explicit user approval for that exfiltration. Next trusted-local command: `npm run report:discord:unit`.

## 4. Decisions made

- `index.html` should be readable in about 30 seconds.
- `command.html` owns the next prompt and done criteria.
- `runbook.html` owns commands and procedures.
- `reports/` owns detailed explanation.

## 5. Problems or risks

- HTML is still manually maintained and can drift from Markdown until a generator is implemented.
- The dashboard may need one more shape pass after the user views it in the browser.
- Discord actual send was blocked by approval policy in this Codex session.

## 6. GPT handoff summary

The dashboard has been simplified into a top-level status surface. Detailed instruction, runbook, and report content now live in their dedicated pages.

## 7. Next Codex tasks

- Run controlled human sessions.
- Put downloaded JSON logs in `playtest_logs/`.
- Run `npm run playtest:summary`.
- Later, build a generator for the orchestration HTML interface.

## 8. Portfolio notes

- Problem: the dashboard was mixing status, instruction, documentation browsing, and explanation.
- Direction: make the interface modular and scannable.
- Action: compacted the dashboard around core project status and primary next links.
- Result: the project now has a cleaner AI-plugin-like control surface for resuming work.
