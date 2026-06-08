# 2026-06-08-01 - v0.12 Playtest Package Rerun

## 1. Current build status

v0.12 is the current controlled human-test candidate. The latest balance loop baseline is `GO_BALANCE_BASELINE`: first boss clear `100%`, full clear `60%`, death `40%`, first boss TTK median `20.73s`.

## 2. What changed today

- Reran the v0.12 playtest package step after the latest balance loop gate fix.
- Updated status/task docs so the package rerun is no longer the next unfinished task.

## 3. Test results and evidence

- `npm run playtest:package:dry`: pass, output target `dist\lethe-v0.12-playtest`.
- `npm run playtest:package`: pass, regenerated `dist\lethe-v0.12-playtest`.
- `npm run report`: pass, generated `docs\reports\2026-06-08.html` and one unit report under `docs\reports\units\2026-06-08`.
- `npm run report:check`: pass.
- `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- `npm run report:discord:unit:dry`: pass.

## 4. Decisions made

- Treat the package as current for controlled human sessions.
- Do not add new gameplay, memories, slots, shop, meta progression, region, weapon, or balance changes before human evidence unless the user explicitly changes scope.

## 5. Problems or risks

- The generated `dist/` package is ignored by git and must be regenerated when needed.
- Human reaction evidence is still missing; automated balance is not Unity-transition proof.
- `npm run report:discord:unit` was blocked by the approval reviewer because sending workspace-generated reports/attachments to an external Discord webhook is treated as potential private data exfiltration in this Codex session. Next trusted-local command: `npm run report:discord:unit`.

## 6. GPT handoff summary

Emotion proxy remains excluded. v0.12 has a passing balance loop baseline and a freshly regenerated human-test package. Next judgment should come from controlled human sessions and JSON log summaries.

## 7. Next Codex tasks

- Place downloaded JSON logs in `playtest_logs/` after sessions.
- Run `npm run playtest:summary`.
- Use the generated summary for the next planning/review handoff before major tuning or Unity-transition decisions.

## 8. Portfolio notes

- Problem: the latest balance loop pass needed a fresh shareable playtest package.
- Direction: close packaging before asking for human evidence.
- Action: ran dry-run and actual package generation.
- Result: `dist\lethe-v0.12-playtest` is regenerated for controlled sessions.
