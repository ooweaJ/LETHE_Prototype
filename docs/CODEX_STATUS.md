# Codex Status

Last updated: 2026-06-02

## Current Build

- Project: LETHE HTML Alpha v0.5 core-fun human-test candidate.
- Repository: `https://github.com/ooweaJ/LETHE_Prototype.git`
- Branch: `main`
- Current scope: pre-human-test polish for the forgetting loop. No broad content expansion yet.

## Implemented

- Static browser prototype: `index.html`, `style.css`, `src/game.js`.
- Weapons: twin blades, greatsword.
- Memories: 6 total, 3 active slots.
- Auto basic attack and auto memory activation.
- Enemy waves and boss encounter.
- Dependency-based forgetting with per-memory deletion bias.
- Echo stat reward after forgetting, default experiment echo power `0.50`.
- Post-boss prediction question UI.
- Forgetting result screen with clearer summary:
  - forgotten memory,
  - prediction result,
  - deletion weight,
  - remaining echo,
  - next build direction.
- Q1/Q2 survey plus Q3 memory-name recall free response.
- JSON log download with selected/predicted/deleted memory names and deletion weights.
- Browser QA fast mode via `?qa=fast` for result-screen and JSON payload verification.
- Codex CLI planning-review fallback via `npm run review:codex` and `npm run review:codex:dry`.
- Claude review local mock mode via `scripts/ask_claude_review.js --mock-response ...` for offline automation checks.
- v0.3 combat-readability polish:
  - floating memory names and damage numbers,
  - hit sparks and projectile trails,
  - boss spawn/phase impact feedback,
  - `레테의 시선` dependency tag and dependency percent in memory slots.
- v0.4 human-test readiness polish:
  - result screen separates lost action from remaining echo transformation,
  - JSON payload includes `echoTransformation`,
  - default UI clarity raised to `0.78` to match the stronger dependency/forgetting UI.
- v0.5 core-fun pass:
  - denser early enemy waves,
  - kill XP and in-run level-up choices,
  - run-only stat growth without meta progression or shops,
  - AI early-fun metrics for pressure, kill tempo, and pre-boss level-ups.
- AI alpha test tool under `alpha_test/`.
- Codex/GPT/Claude workflow docs.
- Markdown daily reports, generated HTML reports, and Discord report delivery.
- Work-unit Discord report delivery via `npm run report:discord:unit` and `--section`.
- Short Discord status notices for Codex work.
- Claude Code planning-review automation.

## Latest AI Test Result

Command:

```bash
npm run ai:test
```

Result:

- Verdict: `GO_CANDIDATE`
- Playability: `AI 기준 사람 테스트 진입 가능`
- Risk Level: `LOW`
- Alpha Fun Score: `0.8531`
- Early Fun Score: `0.8669`
- Early kill tempo: `0.9620`
- Pre-boss level-ups: `4.08`
- Regret proxy: `81.6%`
- Irritation proxy: `0.3%`
- Prediction match: `84.8%`
- Immediate quit: `0.7%`
- Restart intent: `76.1%`
- First forgetting time: `9.00 min`
- Post-forgetting power drop: `28.0%`
- Recovery after replacement: `97.5%`
- Max single memory deletion share: `28.8%`

Heavy check:

- `npm run ai:test:heavy`: `GO_CANDIDATE`, Alpha Fun Score `0.8509`, Early Fun Score `0.8672`, regret `81.4%`, irritation `0.3%`, prediction `84.6%`.

Remaining note:

- Power drop is now below the 30% target (`27.8-28.0%`) because v0.5 prioritizes early fun and smoother growth. It should be observed directly in human testing before over-tuning.

## Latest Sweep Note

Command:

```bash
npm run ai:sweep
```

- Best score in this sweep: `echo=0.40`, `ui=0.78`, score `0.8424`, verdict `ITERATE`.
- Best current human-test candidate: `echo=0.50`, `ui=0.78`, score `0.8245`.
- Current implementation uses `echo=0.50`, `ui=0.78` because v0.4 now has stronger dependency/forgetting UI and passes the main AI gates.

## Open Technical Notes

- Browser visual verification passed in this session:
  - v0.4 labels load correctly,
  - `?qa=fast` reaches the question/result flow,
  - result panel fits without internal scroll at desktop QA viewport,
  - JSON payload includes selected memory names, predicted/protected names, forgotten memory name, deletion weights, survey, echo, experiment, and echo transformation fields,
  - payload experiment version is `v0.4`.
- Browser plugin QA for v0.5 was attempted, but the in-app browser connection failed with a Windows sandbox startup error. Static syntax checks and AI tests passed; visual QA should be rerun locally before or during the first human-test session.
- The game is static HTML and can be run by opening `index.html`.
- Default browser boss/forgetting timing now matches the 9-minute v0.2 target; `?qa=fast` is only for QA.
- Generated AI test outputs are ignored by git under `alpha_test/outputs/`.
- Report HTML can be generated from Markdown with `npm run report`.
- Discord report delivery can be previewed with `npm run report:discord:dry`.
- Work-unit Discord reports can be previewed with `npm run report:discord:unit:dry` or `node scripts/send_discord_report.js docs/reports/YYYY-MM-DD.md --dry-run --section "섹션 제목"`.
- Local `.env` and `.env.*` are ignored by Git.
- No tracked `.env.example` is required.

## Latest Planning Verdict

- GPT verdict: `ITERATE_BEFORE_TEST`.
- Claude v0.5 evaluation: `ITERATE_BEFORE_TEST`, but only due to missing real-browser verification of the v0.5 level-up flow and `runGrowth` payload. Claude says passing that check should move the build to `GO_TO_HUMAN_TEST`.
- v0.2 scope: timing, deletion distribution, echo default, clearer feedback, JSON logs, human-test recall question.
- A v0.3/version-up Claude prompt exists, but actual Claude execution still requires local Claude Code login.
- This session confirmed the local `claude` command is installed: `claude --version` returned `2.1.153 (Claude Code)`.
- A minimal non-project Claude prompt failed with `401 Invalid authentication credentials`, so actual Claude review is blocked until local Claude authentication is fixed.
- `scripts/ask_claude_review.js` now explains 401 failures by asking the user to run `claude` locally, complete login/authentication, and retry `npm run review:claude`.
- `npm run review:claude:dry` still selects `docs/review_prompts/2026-06-02.md` and targets `docs/review_responses/2026-06-02-claude.md`.
- Offline mock verification wrote `alpha_test/outputs/claude-review-mock.md` with `--mock-response`, confirming prompt selection, output directory creation, and response writing without external transmission.
- Codex CLI can write planning responses to `docs/review_responses/YYYY-MM-DD-codex.md` through `npm run review:codex`.
- OpenAI API fallback has been removed by request. The review order is now Claude Code first, then Codex CLI fallback.
- Human testing should proceed after local v0.5 browser QA confirms the level-up flow.

## Next Codex Tasks

- Local v0.5 browser QA is now the one remaining gate before human playtest.
- Confirm level-up 3-choice UI appears, selection resumes combat, and `runGrowth` JSON matches actual choices.
- Use `docs/HUMAN_PLAYTEST_GUIDE.md` for a 5-8 player test focused on regret vs irritation.
- During testing, watch whether the 27.8-28.0% power drop feels too safe or still regretful enough.
