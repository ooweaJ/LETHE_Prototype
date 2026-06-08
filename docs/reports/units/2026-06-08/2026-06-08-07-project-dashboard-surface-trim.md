# 2026-06-08-07 - Project Dashboard Surface Trim

## 1. Current build status

LETHE v0.12 remains the current controlled human-test candidate. This work changed only orchestration HTML and documentation.

## 2. What changed today

- Renamed the dashboard heading from `30초 상태 요약` to `상태 요약`.
- Removed the subtitle explaining where prompts and reports live.
- Removed the bottom cards for next instruction, runbook, and detailed reports.
- Kept the dashboard focused on:
  - current state,
  - latest verification,
  - blocker,
  - next gate,
  - current conclusion,
  - current goal,
  - next judgment,
  - recent completion.

## 3. Test results and evidence

- `npm run report`: pass, generated 7 unit reports.
- `npm run report:check`: pass, 7 report units.
- `npm run report:discord:unit:dry`: pass, latest unit 07 summary generated.
- `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- `Select-String` dashboard check: only `상태 요약` remained; removed target strings were absent.
- `npm run report:discord:unit`: blocked by approval reviewer because it would send workspace-generated report content and attachments to an external Discord webhook, which is untrusted by default and not explicitly approved by the user. Next trusted-local command: `npm run report:discord:unit`.

## 4. Decisions made

- The main dashboard should show state, not explain the interface.
- Human-facing detail belongs in HTML reports.
- AI-facing continuity should stay in Markdown source files and devlogs.

## 5. Problems or risks

- HTML is still manually maintained and can drift from Markdown until a generator is implemented.
- The exact density should be reviewed in-browser after this pass.
- Discord actual send was blocked by approval policy in this Codex session.

## 6. GPT handoff summary

The main dashboard is now a status-only surface. It no longer duplicates the command view, runbook, or report navigation.

## 7. Next Codex tasks

- Run controlled human sessions.
- Put downloaded JSON logs in `playtest_logs/`.
- Run `npm run playtest:summary`.
- Later, build a generator for the orchestration HTML interface.

## 8. Portfolio notes

- Problem: the dashboard still had interface explanation and extra navigation.
- Direction: make the visible surface feel like a concise personal AI plugin dashboard.
- Action: removed explanatory copy and link cards from `index.html`.
- Result: the user gets a cleaner one-screen project state view.
