# Codex Status

Last updated: 2026-06-02

## Current Build

- Project: LETHE HTML Alpha v0.7 weapon/echo balance solo-test candidate.
- Repository: `https://github.com/ooweaJ/LETHE_Prototype.git`
- Branch: `main`
- Current scope: HTML prototype validation. The project is testing whether LETHE's early combat/growth loop and forgetting loop are fun enough to justify moving into Unity implementation. Broad human testing is paused; Claude v0.7 feedback says the automated balance loop should stop and move to user 1-person feel testing.

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
- v0.6 core run structure:
  - 20-minute structure with bosses at 4 / 8 / 12 / 16 / 20 minutes,
  - boss defeat -> dependency-based memory loss,
  - 2-memory deficit survival segment,
  - memory refill from 3 candidates after the deficit segment,
  - `runTimeline` JSON payload with cycles and refill choices,
  - `?qa=fast,v06` browser QA gate.
- v0.7 weapon/echo balance pass:
  - buffed the two existing weapon baselines,
  - added weapon-facing echo effects for lost memories,
  - added side-panel echo labels for weapon residue effects,
  - updated AI simulator weapon baseDps and weapon residue proxy.
- AI alpha test tool under `alpha_test/`.
- Codex/GPT/Claude workflow docs.
- Markdown daily reports, generated HTML reports, and Discord report delivery.
- Work-unit Discord report delivery via `npm run report:discord:unit` and `--section`.
- Short Discord status notices for Codex work.
- Claude Code planning-iteration automation for interpreting AI/human test results and deciding next design direction.
- Test-result planning pipeline via `npm run planning:pipeline`, with Claude first and Codex CLI fallback.
- Local pipeline doctor via `npm run doctor` and `npm run doctor:deep`.
- Autopilot readiness preflight via `npm run autopilot:preflight`, `npm run autopilot:preflight:local`, and `npm run autopilot:preflight:dry`.
- AI collaboration portfolio docs under `docs/ai/`, `docs/adr/`, and `docs/portfolio/`.
- Human playtest summary automation via `npm run playtest:summary`.
- Human playtest package generation via `npm run playtest:package`.

## Latest AI Test Result

Command:

```bash
npm run ai:test
```

v0.7 result:

- Verdict: `GO_CANDIDATE`
- Playability: `AI 기준 사람 테스트 진입 가능`
- Risk Level: `LOW`
- Alpha Fun Score: `0.9131`
- Early Fun Score: `0.8793`
- Early kill tempo: `0.9847`
- Pre-boss level-ups: `4.16`
- First cycle completion: `84.1%`
- Two-memory survival: `81.5%`
- Echo pivot score: `0.7349`
- Regret proxy: `84.0%`
- Irritation proxy: `0.4%`
- Prediction match: `87.6%`
- Immediate quit: `0.7%`
- Restart intent: `76.1%`
- First forgetting time: `4.00 min`
- Post-forgetting power drop: `28.0%`
- Recovery after replacement: `97.5%`
- Max single memory deletion share: `28.8%`

Heavy check:

- `npm run ai:test:heavy`: `GO_CANDIDATE`, Alpha Fun Score `0.9136`, Early Fun Score `0.8787`, first cycle completion `82.7%`, two-memory survival `81.1%`, echo pivot `0.7286`, regret `84.5%`, irritation `0.3%`, prediction `88.1%`.

Remaining note:

- v0.6 fixed the previous 9-minute first-forgetting delay by opening the first cycle at 4 minutes.
- Prediction match is still high and should be watched during 1-person feel testing.
- v0.7 improves weapon baseline and mob-clearing proxy, but echo pivot score dropped slightly; watch whether the player feels "weapon solved it" instead of "lost memory changed the build."
- Actual Claude v0.7 review succeeded and saved `docs/review_responses/2026-06-02-v07-balance-claude.md`.
- Claude verdict: `GO_TO_SOLO_PLAYTEST`, now superseded by user live balance feedback.
- Solo test sheet: `docs/playtest/2026-06-02-solo.md`.

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
- Chrome headless QA passed for v0.5 level-up gate:
  - `file:///C:/jaewoo/LETHE_Prototype/index.html?qa=fast,levelup`,
  - `status: complete`,
  - `levelUpSeen: true`,
  - `resumedAfterUpgrade: true`,
  - `hasRunGrowthPayload: true`,
  - selected upgrade recorded in both `choicesTaken` and payload `runGrowth.choicesTaken`.
- Chrome headless QA passed for v0.5 playtest metadata:
  - `file:///C:/jaewoo/LETHE_Prototype/index.html?qa=fast,levelup&tester=T01&session=S01`,
  - payload `playtest.testerId: T01`,
  - payload `playtest.sessionId: S01`.
- Chrome headless QA passed for v0.6 cycle gate:
  - `file:///C:/jaewoo/LETHE_Prototype/index.html?qa=fast,v06`,
  - version `v0.7`,
  - runTimeline version `v0.7`,
  - `status: complete`,
  - `bossSpawned: true`,
  - `forgotten: true`,
  - `deficitStarted: true`,
  - `refillSeen: true`,
  - `refilled: true`,
  - `hasTimelinePayload: true`,
  - `cycleCount: 1`,
  - `refillCount: 1`.
- Local doctor passed on this machine:
  - `npm run doctor`: 26 pass, 0 warn, 0 fail,
  - `npm run doctor:deep`: 43 pass, 0 warn, 0 fail.
- Latest package check:
  - `npm run playtest:package`: generated `dist\lethe-v0.7-playtest` with `SOLO_PLAYTEST_SHEET.md`.
- Autopilot preflight checks passed:
  - `npm run autopilot:preflight:dry`,
  - `node scripts/autopilot_preflight.js --allow-dirty`: 15 pass, 2 warn, 0 fail,
  - `npm run autopilot:preflight`: 17 pass, 0 warn, 0 fail before v0.7 automation.
- Actual Discord work-unit delivery succeeded in this session:
  - `npm run report:discord:unit`,
  - output: `Uploaded docs\reports\2026-06-02.html to Discord.`
- Full preflight intentionally checks Claude auth before starting unattended implement/test/report loops.
- Human playtest summary preparation passed:
  - `npm run playtest:summary:dry`,
  - `npm run playtest:summary`,
  - generated `docs/playtest_summaries/2026-06-02.md`,
  - generated `docs/review_prompts/2026-06-02-human-playtest.md`.
- Human playtest package preparation passed:
  - `npm run playtest:package:dry`,
  - `npm run playtest:package`,
  - generated `dist/lethe-v0.5-playtest` (ignored by git).
- The game is static HTML and can be run by opening `index.html`.
- Default browser boss/forgetting timing now matches the 9-minute v0.2 target; `?qa=fast` is only for QA.
- Generated AI test outputs are ignored by git under `alpha_test/outputs/`.
- Report HTML can be generated from Markdown with `npm run report`.
- Local setup can be checked with `npm run doctor`.
- Human playtest logs can be summarized from `playtest_logs/` with `npm run playtest:summary`.
- Discord report delivery can be previewed with `npm run report:discord:dry`.
- Work-unit Discord reports can be previewed with `npm run report:discord:unit:dry` or `node scripts/send_discord_report.js docs/reports/YYYY-MM-DD.md --dry-run --section "섹션 제목"`.
- Local `.env` and `.env.*` are ignored by Git.
- No tracked `.env.example` is required.

## Latest Planning Verdict

- GPT verdict: `ITERATE_BEFORE_TEST`.
- Claude v0.5 evaluation: `GO_TO_HUMAN_TEST` after Chrome headless QA confirmed the v0.5 level-up flow and `runGrowth` payload.
- Planning pipeline prompt generated: `docs/review_prompts/2026-06-02-pipeline.md`.
- Planning pipeline Claude response saved: `docs/review_responses/2026-06-02-pipeline-claude.md`.
- Claude pipeline verdict: `GO_TO_HUMAN_TEST`.
- Claude requested no new gameplay feature before human testing. Only minimum data collection was added: tester ID and session number in JSON logs.
- User direct playtest supersedes the previous human-test gate for now: the prototype needs a v0.6 run-structure decision before broader people testing.
- New run-structure planning prompt: `docs/review_prompts/2026-06-02-run-structure-redesign.md`.
- v0.2 scope: timing, deletion distribution, echo default, clearer feedback, JSON logs, human-test recall question.
- A v0.3/version-up Claude prompt exists, but actual Claude execution still requires local Claude Code login.
- This session confirmed the local `claude` command is installed: `claude --version` returned `2.1.153 (Claude Code)`.
- A minimal non-project Claude prompt failed with `401 Invalid authentication credentials`, so actual Claude review is blocked until local Claude authentication is fixed.
- `scripts/ask_claude_review.js` now explains 401 failures by asking the user to run `claude` locally, complete login/authentication, and retry `npm run review:claude`.
- `npm run review:claude:dry` still selects `docs/review_prompts/2026-06-02.md` and targets `docs/review_responses/2026-06-02-claude.md`.
- Offline mock verification wrote `alpha_test/outputs/claude-review-mock.md` with `--mock-response`, confirming prompt selection, output directory creation, and response writing without external transmission.
- Codex CLI can write planning responses to `docs/review_responses/YYYY-MM-DD-codex.md` through `npm run review:codex`.
- OpenAI API fallback has been removed by request. The review order is now Claude Code first, then Codex CLI fallback.
- Human testing is paused until Claude/GPT or the user chooses the v0.6 run structure.
- v0.6 Claude evaluation prompt generated: `docs/review_prompts/2026-06-02-v06-cycle-eval.md`.
- Actual Claude call for v0.6 was blocked by outbound transfer policy in this Codex session. Block note: `docs/review_responses/2026-06-02-v06-cycle-claude-blocked.md`.
- v0.7 balance evaluation prompt generated: `docs/review_prompts/2026-06-02-v07-balance-eval.md`.
- Local Codex v0.7 judgment saved: `docs/review_responses/2026-06-02-v07-balance-codex.md`.
- Actual Claude v0.7 judgment saved: `docs/review_responses/2026-06-02-v07-balance-claude.md`.
- Claude v0.7 verdict: `GO_TO_SOLO_PLAYTEST`.
- User live feedback after v0.7 invalidated the balance judgment: the current balance is not close enough.
- The root problem is that v0.7 Claude feedback was based on AI proxy metrics and browser flow QA, not real balance play.
- New v0.7.1 prompt: `docs/review_prompts/2026-06-02-v071-balance-reality-check.md`.

## Next Codex Tasks

- v0.7 balance reality check is the next product gate.
- On another local machine, run `npm run doctor` first; run `npm run doctor:deep` before leaving Codex to continue unattended.
- Before an unattended implement -> Claude feedback -> implement loop, run `npm run autopilot:preflight`.
- Do not describe AI proxy metrics as real balance feedback.
- Before v0.7.1 tuning, improve the test model or browser balance QA so the automation catches the mismatch the user just found.
- Then run a focused v0.7.1 combat/balance iteration.
