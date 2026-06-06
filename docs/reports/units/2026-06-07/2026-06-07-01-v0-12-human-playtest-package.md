# 2026-06-07-01 - v0.12 Human Playtest Package

## 1. Current build status

v0.12 is the current controlled human-test candidate. The latest automated balance baseline remains `GO_BALANCE_BASELINE`: first boss clear `80%`, full clear `60%`, death `40%`, first boss TTK median `25.79s`.

## 2. What changed today

- Updated `docs/HUMAN_PLAYTEST_GUIDE.md` for v0.12 controlled 5-8 player sessions.
- Updated `docs/PLAYTEST_NOTES.md` with v0.12 observation fields.
- Added `docs/playtest/2026-06-07-v012-human.md`.
- Updated `scripts/prepare_playtest_build.js` so the package includes `V012_HUMAN_PLAYTEST_SHEET.md`.

## 3. Test results and evidence

- `node --check scripts/prepare_playtest_build.js`: pass.
- `npm run playtest:package:dry`: pass, output target `dist\lethe-v0.12-playtest`.
- `npm run playtest:package`: pass, generated `dist\lethe-v0.12-playtest`.
- Package file check confirmed `HUMAN_PLAYTEST_GUIDE.md`, `PLAYTEST_NOTES_TEMPLATE.md`, `V012_HUMAN_PLAYTEST_SHEET.md`, `PLAYTEST_SUMMARY_README.md`, `README.md`, `index.html`, `style.css`, and `src/`.
- `npm run report`: pass.
- `npm run report:check`: pass.
- `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- `npm run report:discord:unit:dry`: pass.

## 4. Decisions made

- Accept the latest automated balance baseline as the current pre-human-test candidate.
- Stop blind numeric tuning until human evidence exists.
- Move the next work toward controlled human sessions and log summarization.

## 5. Problems or risks

- Human players may still find the `40%` death / post-loss pressure too sharp.
- AI balance QA is not human emotion evidence.
- The generated `dist/` package is ignored by git, so it must be regenerated with `npm run playtest:package` when needed.
- `npm run report:discord:unit` was blocked by the approval reviewer because sending workspace reports/attachments to an external Discord webhook is treated as potential private data exfiltration in this Codex session.

## 6. GPT handoff summary

Emotion proxy is excluded. v0.12 has an automated balance baseline and a packaged human-test candidate. Next judgment should come from 5-8 controlled human sessions focused on early fun, meaningful level-up choices, first forgetting reaction, deficit survival feel, refill motivation, and restart intent.

## 7. Next Codex tasks

- After sessions, place downloaded JSON logs in `playtest_logs/`.
- Run `npm run playtest:summary`.
- Send the generated human-test prompt through the planning pipeline before major tuning or Unity-transition decisions.

## 8. Portfolio notes

- Problem: v0.12 passed automated balance, but human-test materials still referenced the old v0.7 solo gate.
- Direction: Convert the automated baseline into a practical controlled playtest package.
- Action: Updated guide, notes, session sheet, and package generation.
- Result: `dist\lethe-v0.12-playtest` is generated and ready to open for controlled sessions.
