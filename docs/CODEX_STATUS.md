# Codex Status

Last updated: 2026-06-02

## Current Build

- Project: LETHE HTML Alpha v0.4 human-test candidate.
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
- OpenAI API planning-review fallback via `npm run review:openai` and `npm run review:openai:dry`.
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
- AI alpha test tool under `alpha_test/`.
- Codex/GPT/Claude workflow docs.
- Markdown daily reports, generated HTML reports, and Discord report delivery.
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
- Alpha Fun Score: `0.8261`
- Regret proxy: `85.6%`
- Irritation proxy: `0.4%`
- Prediction match: `85.8%`
- Immediate quit: `0.7%`
- Restart intent: `70.9%`
- First forgetting time: `9.00 min`
- Post-forgetting power drop: `29.6%`
- Recovery after replacement: `96.6%`
- Max single memory deletion share: `34.1%`

Heavy check:

- `npm run ai:test:heavy`: `GO_CANDIDATE`, Alpha Fun Score `0.8369`, regret `87.6%`, irritation `0.3%`, prediction `87.7%`.

Remaining note:

- Power drop is still just under the 30% target (`29.4-29.6%`). It is the only soft warning left and should be observed in human testing rather than over-tuned now.

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
- The game is static HTML and can be run by opening `index.html`.
- Default browser boss/forgetting timing now matches the 9-minute v0.2 target; `?qa=fast` is only for QA.
- Generated AI test outputs are ignored by git under `alpha_test/outputs/`.
- Report HTML can be generated from Markdown with `npm run report`.
- Discord report delivery can be previewed with `npm run report:discord:dry`.
- Local `.env` and `.env.*` are ignored by Git.
- No tracked `.env.example` is required.

## Latest Planning Verdict

- GPT verdict: `ITERATE_BEFORE_TEST`.
- Claude verdict: `ITERATE_BEFORE_TEST`.
- v0.2 scope: timing, deletion distribution, echo default, clearer feedback, JSON logs, human-test recall question.
- A v0.3/version-up Claude prompt exists, but actual Claude execution still requires local Claude Code login.
- This session confirmed the local `claude` command is installed, but direct Claude execution from Codex was not run because it would send repository prompt content to an external service.
- `npm run review:claude:dry` still selects `docs/review_prompts/2026-06-02.md` and targets `docs/review_responses/2026-06-02-claude.md`.
- Offline mock verification wrote `alpha_test/outputs/claude-review-mock.md` with `--mock-response`, confirming prompt selection, output directory creation, and response writing without external transmission.
- Codex CLI can write planning responses to `docs/review_responses/YYYY-MM-DD-codex.md` through `npm run review:codex`.
- OpenAI fallback automation exists, but actual GPT review requires `OPENAI_API_KEY` in the environment or local `.env`.
- Human testing is intentionally deferred until combat spectacle and LETHE-system readability feel stronger.

## Next Codex Tasks

- Human playtest is now the next major validation step.
- Use `docs/HUMAN_PLAYTEST_GUIDE.md` for a 5-8 player test focused on regret vs irritation.
- During testing, watch whether the 29.4-29.6% power drop feels slightly too safe.
