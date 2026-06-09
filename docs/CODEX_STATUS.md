# Codex Status

Last updated: 2026-06-09

## Current Build

- Project: LETHE HTML Alpha v0.12 is now treated as an evidence build for Unity transition planning.
- Repository: `https://github.com/ooweaJ/LETHE_Prototype.git`
- Branch: `main`
- Current scope: freeze the Unity vertical-slice system contract before more HTML tuning. The current core is memory -> echo -> resonance -> awakened echo -> ultimate echo synergy.
- Latest task-update status: `docs/design/LETHE_CORE_SYSTEMS_UNITY_PLAN.md` is the current system-planning source. Orchestration state now points to Unity-transition planning and the next task is a Unity vertical-slice backlog.
- Latest verification status: v0.12 balance evidence remains valid. The accepted recovery is player max HP `180 -> 190`; two consecutive `npm run balance:loop` runs returned `GO_BALANCE_BASELINE` with full clear `60%`, death `40%`, and first boss clear `100%`. After echo readability work, `npm run qa:postloss`, `npm run qa:identity`, `npm run playtest:package:dry`, `npm run playtest:package`, `npm run report`, `npm run report:check`, and `npm run doctor` passed.
- Latest reporting format: Discord work-unit reports now send an AI Project Orchestrator style status message, attach a generated `.summary.json` file, then send a second message with the generated HTML report.
- Planning document status: `docs/design/` is now a Korean Unity-transition design-doc set. Start with `docs/design/LETHE_GAME_DESIGN_OVERVIEW.md`, then use `LETHE_CORE_SYSTEMS_UNITY_PLAN.md`, `LETHE_RUN_STRUCTURE.md`, `LETHE_COMBAT_DESIGN.md`, `LETHE_CONTENT_TABLES.md`, `LETHE_BALANCE_BASELINE.md`, and `LETHE_UNITY_VERTICAL_SLICE_SPEC.md`. `docs/LETHE_망각의_군주_프로토타입_기획서_v0_11.md`, `docs/BALANCE_TABLE_v0_12.md`, and `docs/LETHE_v0.12_밸런스_개선_제안서.md` remain HTML prototype/balance references.
- Echo system to add: memory and echo levels both cap at `+5`; forgotten memory levels accumulate into matching echoes; reacquired memories with existing echoes gain resonance; `+5` echoes awaken into strong visible powers; two `+5` echoes can unlock an ultimate echo synergy.
- Balance status: v0.12 is no longer the main iteration target unless needed as evidence. Do not continue blind numeric HTML tuning before the Unity system backlog is defined.

## Implemented

- Unity-transition Korean design-doc set:
  - `docs/design/README.md` defines the reading order.
  - `docs/design/LETHE_GAME_DESIGN_OVERVIEW.md` is the first human-facing overview.
  - `docs/design/LETHE_CORE_SYSTEMS_UNITY_PLAN.md` defines the memory/echo/resonance system contract.
  - `docs/design/LETHE_RUN_STRUCTURE.md` defines run flow, growth, forgetting, deficit survival, and refill.
  - `docs/design/LETHE_COMBAT_DESIGN.md` defines combat feel, weapons, enemy roles, and ranged enemy behavior.
  - `docs/design/LETHE_CONTENT_TABLES.md` manages memories, echoes, ultimate echo candidates, and enemies.
  - `docs/design/LETHE_BALANCE_BASELINE.md` summarizes HTML v0.12 evidence and Unity initial targets.
  - `docs/design/LETHE_UNITY_VERTICAL_SLICE_SPEC.md` defines the first Unity implementation scope.
  - Memories and echoes both use a visible `+1` to `+5` level scale.
  - Forgetting converts the active memory level into the matching echo level and clamps at `+5`.
  - If a matching echo already exists, the forgotten level is added to it, e.g. `+3` echo plus forgotten `+2` memory becomes `+5` echo.
  - Reacquiring a memory with an existing echo strengthens the memory through resonance instead of consuming the echo.
  - `+5` echoes become awakened echoes with strong visible combat behavior.
  - Two `+5` echoes can unlock an ultimate echo synergy.
  - Unity first-slice scope should implement a small data-driven version: memory levels, echo levels, forgetting, reacquisition resonance, two awakened echoes, one ultimate echo, and ranged enemy pressure rules.
- Static browser prototype: `index.html`, `style.css`, `src/game.js`.
- Current planning source: `docs/LETHE_망각의_군주_프로토타입_기획서_v0_11.md`, mirrored into the existing Word 기획서 DOCX.
- Current balance source: `docs/BALANCE_TABLE_v0_12.md` plus `docs/LETHE_v0.12_밸런스_개선_제안서.md`.
- v0.12 first balance pass:
  - browser label and experiment version moved to `v0.12`,
  - `굶주린 칼무리` now deals DPS with `dt` and uses nearest-target soft cap,
  - normal enemies receive hybrid time+level HP and damage scaling with a damage cap,
  - JSON logs include `balance` and `telemetry` with damage by source, average DPS, boss TTK/focused DPS, level-up timestamps, slot-fill timing, and scaling samples,
  - tactical-focus dependency in forgetting score uses the v0.12 3x weight,
  - `피의 늪` uses lower proc rates, lower tick strength, shorter duration, and max 5 active pools.
- v0.12 balance automation:
  - `npm run qa:balance` runs browser telemetry balance QA without emotion proxy,
  - `npm run balance:loop` runs balance QA and writes `docs/review_prompts/YYYY-MM-DD-balance-loop.md`,
  - outputs live under `alpha_test/outputs/balance/` and `docs/balance/`,
  - default targets track first-boss clear rate, full clear rate, first-boss TTK median, pre-boss level-ups, slot-fill timing, and top DPS share.
- v0.11 target slice:
  - browser label and experiment version moved to `v0.11`,
  - setup starts with weapon 1 and starting memory 1,
  - level-up choices mix new memory acquisition, active memory upgrade, run stat, and `잔향 증폭`,
  - active memory slots remain capped at 3,
  - boss forgetting is weighted-random after bosses 1-3 and final boss ends the run without a new forgetting,
  - UI shows forget probability during omen/question phases,
  - forgotten memories engrave as leveled weapon echoes,
  - echo tag pairs unlock `피의 늪` and `파쇄 각인`,
  - logs include `memoryAcquisition`, `forgetProbability`, `forgetResult`, `echoState`, `weaponEvolution`, and `predictionAccuracy`.
- v0.10 target slice:
  - browser label and experiment version moved to `v0.10`,
  - default run schedule moved to 600 seconds with bosses at 180 / 340 / 490 / 600 seconds,
  - memory pool expanded from 6 to 8 with `잿빛 보호막` and `망각의 낙인`,
  - synergy pool expanded with `각인 연쇄`,
  - dependency scoring now uses reliance/focus as the dominant signal and removes memory-specific deletion bias,
  - boss defeat opens a top-2 `망각 갈림길` where the player chooses the released memory,
  - releasing the rank-1 memory creates a stronger echo unlock route; releasing rank-2 preserves more familiarity with a weaker route,
  - logs include `forkChoice`, `predictionAccuracy`, and `echoUnlocks`,
  - tactical focus now adds overheat wording, dependency pressure, and synergy boost context.
- Weapons: twin blades, greatsword.
- Memories: 8 total, 3 active slots.
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
- v0.8 gate A:
  - browser label and experiment version moved to `v0.8`,
  - removed HP 1 death-prevention behavior,
  - added real death/run-end handling,
  - added `death` and `danger` JSON payload fields,
  - added `?qa=fast,death` death QA mode.
- v0.9 Work Package 1 first implementation unit:
  - browser label and experiment version moved to `v0.9`,
  - memory selection cards show role, short combat description, and tag chips,
  - setup side panel and combat HUD show current build name, active synergy, and most-dependent memory,
  - JSON/event payloads include `buildIdentity` and `buildIdentitySeenBy90Sec`,
  - AI raw-run payload `stage.build` includes `buildName`, `activeSynergyDetails`, and `mostDependentMemory`,
  - `?qa=fast,identity` writes `data-lethe-identity-qa` for build identity QA.
- v0.9 Work Package 1 QA runner:
  - added `scripts/run_browser_identity_qa.js`,
  - added `npm run qa:identity`,
  - local doctor now checks the identity QA script entry and syntax.
- v0.9 Work Package 1 text-compression pass:
  - compressed the six existing memory combat descriptions,
  - setup/refill memory cards now use one shared `role · short description` summary,
  - choice summaries are visually capped at two lines,
  - no new memories, slots, shop, meta progression, region, or weapon expansion was added.
- v0.9 Work Package 2 Slice A pressure rhythm:
  - browser spawn pacing moves through `숨 고르기`, `압박 상승`, and `망각 전조`,
  - `runTimeline.pressureSegments` and `danger.pressure*Time` fields record the pressure rhythm,
  - AI simulator reports `pressureRhythm` and headline `pressureContrast`,
  - `npm run qa:pressure` script exists for Chrome/CDP pressure QA.
- v0.9 Work Package 2 Slice B minimal post-loss challenge:
  - after memory loss, the 2-memory deficit segment uses existing enemies/spawn parameters to move through `결손 정비` and `결손 압박`,
  - `runTimeline.postLossChallenges` records started/completed/survived state, active memories, HP, and segment ids,
  - `danger` records deficit breath/challenge time and post-loss challenge completions,
  - AI simulator reports `postLossChallengeScore` and `postLossChallengeContrast`,
  - `npm run qa:postloss` script exists for Chrome/CDP post-loss QA.
- v0.9 Work Package 3 Slice A minimal tactical agency:
  - existing active memory slots can be clicked during combat to trigger `전술 집중`,
  - `Digit1`-`Digit3` can focus the corresponding current active memory while preserving the 3-slot limit,
  - focused cooldown memories have their next trigger pulled forward, while `굶주린 칼무리` and `피의 반사` receive a short existing-effect boost,
  - `tacticalFocus` is recorded in JSON logs and `runTimeline`,
  - `?qa=fast,tactical` writes `data-lethe-tactical-qa`,
  - `npm run qa:tactical` exists for Chrome/CDP tactical QA.
- v0.9 browser QA runner fallback:
  - `scripts/run_browser_pressure_qa.js` keeps the existing Chrome CDP pipe path,
  - if pipe target lookup times out, it retries once through Chrome remote-debugging-port and a WebSocket CDP client,
  - if both transport paths fail, it now reports `BrowserQaTransportError` with the pipe failure, port failure, and next trusted-local command,
  - port fallback now asks the OS for a confirmed free `127.0.0.1` port and shares stable headless Chrome flags with the pipe path,
  - `scripts/run_trusted_postloss_gate.js` and `npm run qa:postloss:trusted` run the selected trusted-local post-loss gate as one command: standard post-loss QA, one 30000 ms retry for transport failures, then the blocker prompt if transport still fails,
  - the trusted wrapper writes `alpha_test/outputs/postloss-trusted-gate/latest.json` with `status`, `transportFailure`, run summaries, `nextCommand`, and `blockerPrompt` for loop/report handoff,
  - `scripts/run_trusted_tactical_gate.js` and `npm run qa:tactical:trusted` mirror that gate for WP3 Slice A tactical browser proof and write `alpha_test/outputs/tactical-trusted-gate/latest.json`,
  - tactical transport blockers now hand off to `docs/review_prompts/2026-06-03-tactical-browser-transport-blocker.md`,
  - this is a QA tooling change only and does not alter gameplay scope.
- AI alpha test tool under `alpha_test/`.
- Codex/GPT/Claude workflow docs.
- Markdown daily reports, generated HTML reports, and Discord report delivery.
- Work-unit Discord report delivery via `npm run report:discord:unit` and `--section`.
- Generated per-unit report files:
  - `npm run report` splits each top-level `# YYYY-MM-DD-NN - 작업 제목` section into `docs/reports/units/YYYY-MM-DD/*.md` and `*.html`,
  - `docs/reports/units/YYYY-MM-DD/latest.json` records the latest unit,
  - `node scripts/send_discord_report.js --latest-section` now attaches the latest unit HTML rather than the full daily HTML,
  - the Discord attachment source of truth is the generated unit HTML under `docs/reports/units/YYYY-MM-DD/`, while the daily report remains the Markdown source,
  - `npm run report:check` verifies headings and generated unit files.
- Larger report-unit policy:
  - report units should describe a feature, gate, or decision,
  - one report unit should include implementation, verification, feedback, and next-task decision,
  - commit/log units may stay small, but Discord/report units should not be `Feedback-N 태스크 갱신` or loop-step titles.
  - `docs/reports/2026-06-02.md` is now consolidated to 13 units instead of 57; detailed loop mechanics remain in `docs/loop_runs/` and git history.
- Short Discord status notices for Codex work.
- Claude Code planning-iteration automation for interpreting AI/human test results and deciding next design direction.
- Test-result planning pipeline via `npm run planning:pipeline`, with Claude first and Codex CLI fallback.
- Local pipeline doctor via `npm run doctor` and `npm run doctor:deep`.
- Autopilot readiness preflight via `npm run autopilot:preflight`, `npm run autopilot:preflight:local`, and `npm run autopilot:preflight:dry`.
- Autonomous dev loop prompt cleanup: `scripts/run_autonomous_dev_loop.js` now points nested Codex at the foremost unfinished v0.9 item in `docs/NEXT_TASKS.md` instead of hard-coding v0.9 WP1.
- Autonomous dev loop preflight-order cleanup:
  - default dev-loop preflight changed from `node scripts/autopilot_preflight.js --allow-dirty` to `npm run autopilot:preflight:local`,
  - preflight now runs before the dev loop creates its Markdown log,
  - successful preflight output is copied into the loop log header after the log is created,
  - this prevents a clean tree from becoming dirty only because the loop wrote its own log before preflight.
- Autopilot preflight loop-run blocker diagnosis:
  - `scripts/autopilot_preflight.js` now summarizes dirty trees with a bounded file list,
  - when dirty files include `docs/loop_runs/*.md`, the fix text points to finishing wrapper result files, recording/removing abandoned artifacts, and rerunning `npm run autopilot:preflight:local`,
  - this is a diagnostic gate-cleanup change only; it does not commit or delete existing loop-run outputs.
- Autopilot preflight loop-run missing-result diagnosis:
  - dirty loop-run prompt files are paired with their expected `*-result.md` file,
  - missing result paths are printed in the preflight fix text,
  - current verification identified `docs/loop_runs/2026-06-02-devloop-175642-iteration-4-implement-result.md` as the wrapper-owned missing result.
- AI collaboration portfolio docs under `docs/ai/`, `docs/adr/`, and `docs/portfolio/`.
- Human playtest summary automation via `npm run playtest:summary`.
- Human playtest package generation via `npm run playtest:package`.

## Latest AI Test Result

Command:

```bash
npm run ai:test
```

v0.9 WP2 Slice B result:

- Verdict: `GO_CANDIDATE`
- Playability: `AI 기준 사람 테스트 진입 가능`
- Risk Level: `LOW`
- Alpha Fun Score: `0.8879`
- Early Fun Score: `0.8339`
- Early kill tempo: `0.9624`
- Pre-boss level-ups: `3.85`
- First cycle completion: `80.9%`
- Two-memory survival: `78.8%`
- Post-loss challenge score: `0.6692`
- Post-loss challenge contrast: `0.3135`
- Echo pivot score: `0.6527`
- Regret proxy: `81.6%`
- Irritation proxy: `0.6%`
- Prediction match: `85.2%`
- Death/fail: `47.0%`
- Post-forgetting power drop: `22.9%`
- Recovery after replacement: `96.8%`

WP3 Slice A quick check:

- `npm run ai:test:quick`: `GO_CANDIDATE`
- Alpha Fun Score: `0.8846`
- Early Fun Score: `0.8316`
- Pressure contrast: `0.4416`
- Post-loss challenge score: `0.6687`
- Post-loss challenge contrast: `0.3134`
- Echo pivot score: `0.6554`
- Regret proxy: `80.7%`
- Irritation proxy: `1.0%`

Heavy check:

- Not rerun in the WP3 Slice A loop. Use `npm run ai:test` and `npm run ai:test:quick` results above for this work unit.

Remaining note:

- v0.9 WP2 now has both pre-loss pressure rhythm and a minimal post-loss challenge, and WP3 Slice A has a first minimal tactical agency hook.
- `npm run qa:postloss:trusted` passed in this local run after making Chrome temp-profile cleanup retryable. The browser QA reached `status: complete`, `failures: []`, confirmed `deficit_breath` and `deficit_trial`, completed the post-loss challenge, and restored 3 active memories after refill.
- `npm run qa:tactical` failed before gameplay evaluation in this managed sandbox with the same Chrome transport class: CDP pipe `Target.getTargets` timeout and remote-debugging-port `listen EPERM` on `127.0.0.1`. Treat this as missing browser proof, not a gameplay assertion failure.
- `npm run qa:tactical:trusted` now records that blocker as `alpha_test/outputs/tactical-trusted-gate/latest.json`; the latest managed-sandbox run is `status: blocked`, `transportFailure: true` after the standard run and one 30000 ms retry.
- The tactical trusted gate now points repeated outside-sandbox transport failure to `docs/review_prompts/2026-06-03-tactical-browser-transport-blocker.md` for a WP3-specific environment-blocker decision.
- AI proxy evidence remains a planning pass only, not human emotion or Unity-transition proof.
- People testing still waits until tactical browser proof or an explicit environment-blocker decision is recorded.

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

- v0.9 direction: do not proceed directly to v0.8 Gate C as the next product step.
- New target: release-feel HTML prototype before broad human testing.
- Reference research added: `docs/research/2026-06-02-roguelike-reference.md`.
- New planning prompt: `docs/review_prompts/2026-06-02-v09-release-feel-loop.md`.
- New overnight loop runner:
  - `npm run overnight:loop:dry`,
  - `npm run overnight:loop`,
  - logs to `docs/loop_runs/`,
  - blocker prompts to `docs/review_prompts/YYYY-MM-DD-overnight-loop-blocker-N.md`.
- Overnight loop verification:
  - `node --check scripts/run_overnight_loop.js`: passed,
  - `npm run overnight:loop:dry`: passed without writing a log file,
  - `npm run doctor`: 36 pass, 0 warn, 0 fail,
  - `npm run doctor:deep`: 50 pass, 0 warn, 0 fail,
  - safe smoke run passed with `--provider none --test none`,
  - smoke log: `docs/loop_runs/2026-06-02-overnight-163600.md`.
- Actual v0.9 loop iteration:
  - command used `--provider double --test quick`,
  - preflight result: 17 pass, 2 warn, 0 fail with dirty tree allowed,
  - quick AI test: `GO_CANDIDATE`, Alpha Fun Score `0.8883`, death/fail `40.0%`, echo pivot `0.656`,
  - Claude response: `docs/review_responses/2026-06-02-v09-release-feel-loop-claude.md`,
  - Codex CLI response: `docs/review_responses/2026-06-02-v09-release-feel-loop-codex.md`,
  - synthesis: `docs/review_responses/2026-06-02-v09-release-feel-loop-double-check.md`,
  - loop log: `docs/loop_runs/2026-06-02-overnight-163806.md`.
- Discord progress tracking fixed for overnight loops:
  - real `overnight:loop` sends start/status/checkpoint/blocked/done Codex notices by default,
  - real `overnight:loop` uploads the latest work-unit report by default,
  - `--discord-dry-run` previews messages without sending,
  - `--no-discord` disables notices only for deliberate local debugging,
  - notification smoke log: `docs/loop_runs/2026-06-02-overnight-164700.md`.
- Autonomous development loop added:
  - `npm run dev:loop:dry`,
  - `npm run dev:loop`,
  - loop shape is task/NEXT_TASKS -> Codex implementation -> verification -> report/Discord -> Claude + Codex feedback -> task update -> commit/push -> next task,
  - default budget is 6 iterations / 360 minutes,
  - implementation uses `codex exec --sandbox workspace-write`,
  - verification uses `npm run doctor` and `npm run ai:test:quick`,
  - feedback uses `node scripts/run_planning_pipeline.js --provider double --test none`,
  - task update uses a second Codex docs-only pass,
  - successful iterations commit and push by default,
  - blocked iterations write `docs/review_prompts/YYYY-MM-DD-autodev-blocker-*.md`.
- Autonomous dev loop verification:
  - `node --check scripts/run_autonomous_dev_loop.js`: passed,
  - `npm run dev:loop:dry`: passed,
  - `npm run doctor`: 38 pass, 0 warn, 0 fail,
  - `npm run doctor:deep`: 54 pass, 0 warn, 0 fail.
- Autonomous dev loop first implementation attempt:
  - started `node scripts/run_autonomous_dev_loop.js --iterations 1 --duration-minutes 90`,
  - loop log: `docs/loop_runs/2026-06-02-devloop-170139.md`,
  - implementation result: `docs/loop_runs/2026-06-02-devloop-170139-iteration-1-implement-result.md`,
  - implemented v0.9 WP1 identity hook and UI/payload fields,
  - nested Codex process did not return cleanly, so the loop was manually recovered,
  - added nested Codex timeout option: `--codex-timeout-minutes` default `20`.
- v0.9 WP1 identity hook verification:
  - `node --check src/game.js`: passed,
  - `node --check alpha_test/src/simulator.js`: passed,
  - `npm run ai:test:quick`: `GO_CANDIDATE`, Alpha Fun Score `0.8883`,
  - `npm run ai:test`: `GO_CANDIDATE`, Alpha Fun Score `0.8909`,
  - feedback prompt: `docs/review_prompts/2026-06-02-autodev-feedback-1.md`,
  - Claude feedback: `docs/review_responses/2026-06-02-autodev-feedback-1-claude.md`,
  - Codex feedback: `docs/review_responses/2026-06-02-autodev-feedback-1-codex.md`,
  - synthesis: `docs/review_responses/2026-06-02-autodev-feedback-1-double-check.md`.
- Selected next implementation scope:
  - keep v0.9 Work Package 1 open,
  - next smallest task is a stable `?qa=fast,identity` browser/headless smoke test,
  - do not move to WP2 post-loss challenge until identity UI/payload visibility is verified.
- v0.9 adaptation focus:
  - visible build identity inside 90 seconds,
  - pressure highs and lows,
  - post-forgetting loss challenge,
  - small tactical agency inside auto combat,
  - no content expansion beyond current scope guards.
- v0.8 Gate B status: `AI_GO_CANDIDATE`, but not a final human-fun verdict.
- v0.8 Gate B implemented:
  - 9-minute prototype run,
  - first mini-boss/first forgetting at 90 seconds,
  - later bosses at 210 / 360 / 510 seconds,
  - memory tag badges,
  - three minimal synergies: area+control, dot+control, burst+survival,
  - tag echo display after forgetting,
  - reduced generic stat refund from echoes,
  - actual death/failure rate as an AI gate.
- v0.8 automation loop evidence:
  - first Gate B quick run failed after probabilistic deletion made forgetting feel random: `NO_GO_FIX_CORE`,
  - deletion was restored to deterministic highest-dependence memory loss,
  - post-forget drop gate was corrected from `30-40%` to `20-35%` because the user's real play feedback was already "too weak after loss",
  - heavy 5000-run test initially failed on death/failure rate `70.7% > 70%`,
  - boss difficulty/late boss HP scaling was softened,
  - final quick/default/heavy runs all returned `GO_CANDIDATE`.
- Latest AI test evidence:
  - v0.9 identity hook verification:
    - `npm run autopilot:preflight:local`: failed because existing untracked `docs/loop_runs/2026-06-02-devloop-170139*.md` files made the working tree dirty,
    - `node --check src/game.js`: passed,
    - `node --check alpha_test/src/simulator.js`: passed,
    - quick raw-run payload confirmed `stage.build.buildName`, `stage.build.mostDependentMemory`, and `stage.build.activeSynergyDetails`,
    - static hook check confirmed `buildIdentity`, `letheIdentityQa`, v0.9 labels, and build-card CSS are present,
    - browser visual QA remains incomplete because the in-app Browser reported `iab` unavailable and Chrome headless `--dump-dom` returned empty output.
  - `npm run ai:test:quick`: `GO_CANDIDATE`, Alpha Fun Score `0.8883`, regret `80.8%`, irritation `1.0%`, prediction `85.5%`, death/fail `40.0%`.
  - `npm run ai:test`: `GO_CANDIDATE`, Alpha Fun Score `0.8909`, regret `81.6%`, irritation `0.7%`, prediction `85.1%`, death/fail `45.1%`.
  - `npm run ai:test:heavy`: `GO_CANDIDATE`, Alpha Fun Score `0.8893`, regret `81.8%`, irritation `0.8%`, prediction `84.8%`, death/fail `67.7%`.
- Latest Discord work-unit delivery succeeded with `npm run report:discord:unit`.
- Browser plugin QA was not available in this session (`iab` unavailable). Chrome CLI also returned no dump output because it handed off to an existing user browser session, so the next gate must add a stable browser QA runner rather than treating flow QA as complete.
- Latest v0.9 identity QA runner attempt:
  - `npm run qa:identity` exists and syntax checks pass,
  - fixed Chrome pipe direction and core-visible completion criteria,
  - `npm run qa:identity`: passed,
  - result: `status: complete`, failures `[]`,
  - confirmed visible build name, active synergy, dependent memory, buildIdentity payload, and `buildIdentitySeenBy90Sec`.
- Latest v0.9 WP1 text-compression verification:
  - `npm run autopilot:preflight:local`: failed because existing untracked files keep the working tree dirty:
    - `docs/loop_runs/2026-06-02-devloop-173350-iteration-1-implement-prompt.md`,
    - `docs/loop_runs/2026-06-02-devloop-173350.md`,
  - `node --check src/game.js`: passed,
  - `node --check scripts/run_browser_identity_qa.js`: passed,
  - static summary-length check: six memory summaries are 20-26 characters,
  - `npm run ai:test:quick`: `GO_CANDIDATE`, Alpha Fun Score `0.8883`, regret `80.8%`, irritation `1.0%`, prediction `85.5%`, death/fail `40.0%`,
  - `npm run ai:test`: `GO_CANDIDATE`, Alpha Fun Score `0.8909`, regret `81.6%`, irritation `0.7%`, prediction `85.1%`, death/fail `45.1%`,
  - `npm run qa:identity`: failed in this Codex session because Chrome CDP pipe timed out waiting for `Target.getTargets`,
  - in-app Browser fallback was unavailable because `iab` was not provided in this session.
- Latest devloop feedback synthesis:
  - prompt: `docs/review_prompts/2026-06-02-devloop-173350-feedback-1.md`,
  - Claude response: `docs/review_responses/2026-06-02-devloop-173350-feedback-1-claude.md`,
  - Codex CLI response: `docs/review_responses/2026-06-02-devloop-173350-feedback-1-codex.md`,
  - synthesis: `docs/review_responses/2026-06-02-devloop-173350-feedback-1-double-check.md`,
  - common conclusion: WP1 copy compression is implemented and AI-stable, but unattended automation is blocked by untracked loop-run files and identity QA should be rerun from trusted local,
  - conflict: Claude wants WP2 pressure high/low before post-loss challenge, while Codex CLI proposed a minimal post-loss challenge first,
  - selected order: preflight cleanup, trusted-local `npm run qa:identity`, then WP2 Slice A pressure rhythm; post-loss challenge remains a minimal WP2 follow-up.
- Latest devloop feedback synthesis for prompt cleanup:
  - prompt: `docs/review_prompts/2026-06-02-devloop-175642-feedback-1.md`,
  - Claude response: `docs/review_responses/2026-06-02-devloop-175642-feedback-1-claude.md`,
  - Codex CLI response: `docs/review_responses/2026-06-02-devloop-175642-feedback-1-codex.md`,
  - synthesis: `docs/review_responses/2026-06-02-devloop-175642-feedback-1-double-check.md`,
  - common conclusion: prompt cleanup was appropriate, WP1 should not be reopened for new gameplay work, AI proxy evidence remains planning-stable, and dirty loop-run outputs plus trusted-local identity QA are the next blockers,
  - conflict: Claude recommends `GO_TO_HUMAN_TEST` after identity QA with a human-test checklist, while Codex CLI recommends gate cleanup followed by WP2 Slice A pressure rhythm before people testing,
  - selected order: docs-only update in this pass, then record/track `2026-06-02-devloop-175642*` outputs, rerun trusted-local `npm run qa:identity`, and keep the existing `NEXT_TASKS.md` WP2 Slice A order unless the user overrides it.
- Latest autonomous dev-loop preflight-order cleanup verification:
  - `node --check scripts/run_autonomous_dev_loop.js`: passed,
  - `npm run dev:loop:dry`: passed and now shows `npm run autopilot:preflight:local` as the preflight command,
  - `node scripts/run_autonomous_dev_loop.js --dry-run --allow-dirty --no-commit --no-push --discord-dry-run`: passed and shows `node scripts/autopilot_preflight.js --allow-dirty` only for explicit dirty smoke checks,
  - `npm run doctor`: 39 pass, 0 warn, 0 fail,
  - `npm run ai:test:quick`: `GO_CANDIDATE`, Alpha Fun Score `0.8883`, regret `80.8%`, irritation `1.0%`, prediction `85.5%`, death/fail `40.0%`,
  - `git diff --check`: passed,
  - `npm run autopilot:preflight:local`: failed as expected in this dirty session with `M docs/loop_runs/2026-06-02-devloop-175642.md`, `M scripts/run_autonomous_dev_loop.js`, and `?? docs/loop_runs/2026-06-02-devloop-175642-iteration-2-implement-prompt.md`,
  - failure confirms the existing wrapper outputs still need commit/cleanup before unattended automation; it is not evidence that the new ordering is broken.
- Latest devloop feedback synthesis for preflight-order cleanup:
  - prompt: `docs/review_prompts/2026-06-02-devloop-175642-feedback-2.md`,
  - Claude response: `docs/review_responses/2026-06-02-devloop-175642-feedback-2-claude.md`,
  - Codex CLI response: `docs/review_responses/2026-06-02-devloop-175642-feedback-2-codex.md`,
  - synthesis: `docs/review_responses/2026-06-02-devloop-175642-feedback-2-double-check.md`,
  - common conclusion: WP1 pipeline cleanup is functionally correct, but dirty loop-run outputs keep official WP1 completion and unattended loop restart blocked,
  - common conclusion: `GO_CANDIDATE`, Alpha Fun Score `0.8883`, and low irritation are planning evidence only; they are not real human emotion or balance proof,
  - watch points: echoPivotScore `0.656` and earlyChoiceInterest `0.654` remain weak enough to observe in WP2 or human testing,
  - conflict: Claude limits the next executable unit to WP1 gate cleanup and forbids WP2 before clean preflight plus identity QA, while Codex CLI points back to WP2 Slice A after that gate passes,
  - selected order: docs-only update in this pass, then loop-run output recording/cleanup, clean-tree `npm run autopilot:preflight:local`, trusted-local `npm run qa:identity`, and only then WP2 Slice A pressure rhythm.
- Latest loop-run blocker diagnosis verification:
  - `node --check scripts/autopilot_preflight.js`: passed,
  - `npm run doctor`: 39 pass, 0 warn, 0 fail,
  - `npm run autopilot:preflight:local`: failed as expected because the tree is still dirty, but now reports the loop-run-artifact-specific fix:
    - finish missing wrapper result files,
    - run `git add docs/loop_runs && git commit -m "docs: 자동 개발 루프 산출물 기록"` or remove abandoned artifacts,
    - rerun `npm run autopilot:preflight:local`,
  - latest missing-result check: `npm run autopilot:preflight:local` reports `docs/loop_runs/2026-06-02-devloop-175642-iteration-4-implement-result.md` as the expected wrapper-owned result file.
- Latest devloop feedback synthesis for blocker diagnosis:
  - prompt: `docs/review_prompts/2026-06-02-devloop-175642-feedback-3.md`,
  - Claude response: `docs/review_responses/2026-06-02-devloop-175642-feedback-3-claude.md`,
  - Codex CLI response: `docs/review_responses/2026-06-02-devloop-175642-feedback-3-codex.md`,
  - synthesis: `docs/review_responses/2026-06-02-devloop-175642-feedback-3-double-check.md`,
  - common conclusion: AI proxy data is positive enough for a planning pass, but it is not human emotion or balance proof,
  - common conclusion: the latest implementation is infrastructure/gate cleanup, so WP1 should not be reopened for new gameplay work,
  - common conclusion: the next executable blocker remains loop-run artifact recording/cleanup, clean-tree `npm run autopilot:preflight:local`, and trusted-local `npm run qa:identity`,
  - watch points: echoPivotScore `0.656` and earlyChoiceInterest `0.654` should be observed rather than patched from AI numbers alone,
  - conflict: Claude recommends human-test checklist and `GO_TO_HUMAN_TEST` after gate cleanup, while Codex CLI keeps the existing WP2 Slice A pressure-rhythm order after gate cleanup,
  - selected order: docs-only update in this pass, then gate cleanup only; do not start WP2, human-test checklist, tutorial/UI changes, or balance changes until the cleanup checks pass.
- Latest devloop feedback synthesis for missing-result diagnosis:
  - prompt: `docs/review_prompts/2026-06-02-devloop-175642-feedback-4.md`,
  - Claude response: `docs/review_responses/2026-06-02-devloop-175642-feedback-4-claude.md`,
  - Codex CLI response: `docs/review_responses/2026-06-02-devloop-175642-feedback-4-codex.md`,
  - synthesis: `docs/review_responses/2026-06-02-devloop-175642-feedback-4-double-check.md`,
  - common conclusion: AI proxy evidence remains positive enough for planning (`GO_CANDIDATE`, Alpha Fun Score `0.8883`, regret `0.8083`, irritation `0.0104`, restart `0.90`), but it is not human emotion or balance proof,
  - common conclusion: the latest implementation is infrastructure/gate cleanup only and should not expand WP1 gameplay scope,
  - current blocker: the wrapper result file now exists, so the next step is recording/cleaning `docs/loop_runs/2026-06-02-devloop-175642*`, then clean-tree `npm run autopilot:preflight:local`, then trusted-local `npm run qa:identity`,
  - watch points: `멈춘 초침` deletion-rate outlier, earlyChoiceInterest `0.654`, and echoPivotScore `0.656` should be observed rather than patched from AI proxy data alone,
  - conflict: Claude frames the build as a people-test preparation candidate after cleanup, while Codex CLI limits the next executable unit to loop-run artifact 정합성 정리 and warns it is not Unity-transition evidence,
  - selected order: docs-only update in this pass, artifact cleanup next, then clean preflight and identity QA; WP2 Slice A pressure rhythm starts only after those pass.
- Latest devloop feedback synthesis for WP2 Slice B:
  - prompt: `docs/review_prompts/2026-06-02-devloop-193946-feedback-1.md`,
  - Claude response: `docs/review_responses/2026-06-02-devloop-193946-feedback-1-claude.md`,
  - Codex CLI response: `docs/review_responses/2026-06-02-devloop-193946-feedback-1-codex.md`,
  - synthesis: `docs/review_responses/2026-06-02-devloop-193946-feedback-1-double-check.md`,
  - common conclusion: WP2 Slice B is within the selected scope and should not be expanded with new memories, slots, shops, meta progression, regions, enemies, or weapons,
  - common conclusion: AI proxy evidence is stable enough for planning but remains weaker than browser combat evidence or user play evidence,
  - browser blocker: `npm run qa:postloss` and `npm run qa:pressure` both failed at Chrome/CDP `Target.getTargets`, so trusted-local browser QA is required before treating Slice B as browser-proven,
  - watch points: `earlyChoiceInterest` remains the weakest current fun metric, while `postLossChallengeScore` around `0.67` and contrast around `0.313` mean the new post-loss beat is present but not human-proven,
  - conflict: Claude proposes a stage-entry two-choice "기억 집중" moment, while Codex CLI proposes HUD/number-key active-memory focus during combat,
  - selected order: rerun trusted-local `npm run qa:postloss`; if it passes, start WP3 Slice A as one minimal existing-memory tactical focus hook and decide the UI surface from the smallest stable implementation path.
- Latest devloop feedback-2 synthesis for post-loss QA fallback:
  - prompt: `docs/review_prompts/2026-06-02-devloop-193946-feedback-2.md`,
  - Claude response: `docs/review_responses/2026-06-02-devloop-193946-feedback-2-claude.md`,
  - Codex CLI response: `docs/review_responses/2026-06-02-devloop-193946-feedback-2-codex.md`,
  - synthesis: `docs/review_responses/2026-06-02-devloop-193946-feedback-2-double-check.md`,
  - common conclusion: the QA runner fallback is valid tooling and keeps scope intact, but WP2 Slice B remains not browser-proven,
  - common conclusion: AI proxy data remains a planning pass only; `earlyChoiceInterest`, `echoPivotScore`, and `postLossChallengeScore` are observation targets, not reasons to add new systems,
  - conflict: no material scope conflict in this feedback round; both Claude and Codex block WP3 until trusted-local post-loss browser proof or a documented environment decision,
  - selected order: run trusted-local `npm run qa:postloss` outside the managed sandbox; if gameplay assertions fail, fix only that post-loss flow; if transport still fails, document the environment blocker before asking whether to proceed.
- Latest devloop feedback-3 synthesis for post-loss transport blocker:
  - prompt: `docs/review_prompts/2026-06-02-devloop-193946-feedback-3.md`,
  - Claude response: `docs/review_responses/2026-06-02-devloop-193946-feedback-3-claude.md`,
  - Codex CLI response: `docs/review_responses/2026-06-02-devloop-193946-feedback-3-codex.md`,
  - synthesis: `docs/review_responses/2026-06-02-devloop-193946-feedback-3-double-check.md`,
  - common conclusion: WP2 Slice B and `BrowserQaTransportError` diagnostic work are scope-valid, but the slice is still not browser-proven,
  - common conclusion: AI proxy data is positive enough for planning (`GO_CANDIDATE`, Alpha Fun Score `0.8846`, regret `0.8073`, irritation `0.0104`, restart `0.90`) but is not browser/user evidence,
  - conflict: no material next-scope conflict; Claude and Codex both require trusted-local post-loss proof or an explicit environment-blocker decision before WP3 or people testing,
  - selected order: run trusted-local `npm run qa:postloss`; if the same transport failure repeats, retry once with `--timeout-ms 30000`, then use `docs/review_prompts/2026-06-02-postloss-browser-transport-blocker.md` before any new gameplay scope.
- Latest devloop feedback synthesis for WP3 Slice A tactical focus:
  - prompt: `docs/review_prompts/2026-06-03-devloop-050050-feedback-1.md`,
  - Claude response: `docs/review_responses/2026-06-03-devloop-050050-feedback-1-claude.md`,
  - Codex CLI response: `docs/review_responses/2026-06-03-devloop-050050-feedback-1-codex.md`,
  - synthesis: `docs/review_responses/2026-06-03-devloop-050050-feedback-1-double-check.md`,
  - common conclusion: WP3 Slice A `전술 집중` is scope-valid because it uses only existing memories, active slots, and combat systems,
  - common conclusion: `npm run ai:test:quick` is positive planning evidence (`GO_CANDIDATE`, Alpha Fun Score `0.8846`, low irritation), but it does not replace tactical browser QA or human play evidence,
  - browser blocker: `npm run qa:tactical` failed before gameplay evaluation through Chrome/CDP `Target.getTargets` timeout and remote-debugging-port `127.0.0.1 listen EPERM`,
  - watch point: `멈춘 초침` deletion frequency is low versus other memories and should be observed after browser proof rather than tuned from AI proxy alone,
  - conflict: no material next-scope conflict; Claude and Codex both block people testing and additional gameplay/UI/balance expansion until tactical browser proof or an explicit environment-blocker decision,
  - selected order: run trusted-local `npm run qa:tactical`; if it passes, record WP3 Slice A as browser-proven, and if transport still fails outside the sandbox, create an environment-blocker decision prompt before changing scope.
- Latest devloop feedback-2 synthesis for tactical trusted gate logging:
  - prompt: `docs/review_prompts/2026-06-03-devloop-050050-feedback-2.md`,
  - Claude response: `docs/review_responses/2026-06-03-devloop-050050-feedback-2-claude.md`,
  - Codex CLI response: `docs/review_responses/2026-06-03-devloop-050050-feedback-2-codex.md`,
  - synthesis: `docs/review_responses/2026-06-03-devloop-050050-feedback-2-double-check.md`,
  - common conclusion: WP3 Slice A and the trusted tactical gate wrapper are scope-valid, and the latest managed-sandbox `status: blocked`, `transportFailure: true` result is transport evidence rather than gameplay evidence,
  - common conclusion: `GO_CANDIDATE`, Alpha Fun Score `0.8846`, regret `0.8073`, and irritation `0.0104` remain planning evidence only,
  - conflict: Claude proposed adding one line of echo-pivot hint text before people testing, while Codex CLI constrained the next action to trusted-local tactical browser proof only,
  - selected order: keep this cycle docs-only and do not widen implementation scope; run sandbox-outside trusted-local `npm run qa:tactical:trusted`, then either mark WP3 Slice A browser-proven or record an environment-blocker decision.
- Latest devloop feedback-3 synthesis for tactical blocker handoff:
  - prompt: `docs/review_prompts/2026-06-03-devloop-050050-feedback-3.md`,
  - Claude response: `docs/review_responses/2026-06-03-devloop-050050-feedback-3-claude.md`,
  - Codex CLI response: `docs/review_responses/2026-06-03-devloop-050050-feedback-3-codex.md`,
  - synthesis: `docs/review_responses/2026-06-03-devloop-050050-feedback-3-double-check.md`,
  - common conclusion: WP3 Slice A `전술 집중` and the WP3-specific blocker prompt split are scope-valid gate/handoff work, not browser proof or gameplay expansion,
  - common conclusion: `GO_CANDIDATE`, Alpha Fun Score `0.8846`, regret `0.8073`, and irritation `0.0104` remain planning evidence only,
  - blocker: `npm run qa:tactical:trusted` latest managed-sandbox result is still `status: blocked`, `transportFailure: true`, so tactical focus remains not browser-proven,
  - conflict: no material next-scope conflict; Claude flags `멈춘 초침` deletion outlier and echo-pivot/post-loss scores as observation risks, while Codex CLI limits the executable next step to trusted-local tactical browser proof,
  - selected order: run sandbox-outside trusted-local `npm run qa:tactical:trusted`; if it passes, mark WP3 Slice A browser-proven, and if the same transport failure repeats, use `docs/review_prompts/2026-06-03-tactical-browser-transport-blocker.md` before people testing or new gameplay/UI/balance scope.
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

- v0.9 Work Package 1 implementation is complete: identity hook, identity QA runner, and compressed existing-memory card copy are all in place.
- Docs-only loop update completed for the latest `2026-06-02-devloop-175642-feedback-1` Claude/Codex synthesis: `docs/NEXT_TASKS.md`, `docs/CODEX_STATUS.md`, devlog, report, and the double-check summary now reflect the selected order.
- Docs-only loop update completed for `2026-06-02-devloop-175642-feedback-2`: Claude/Codex common points, conflict, selected vNext scope, and required tests are recorded in the double-check summary, `NEXT_TASKS`, status, devlog, and report.
- Docs-only loop update completed for `2026-06-02-devloop-175642-feedback-3`: AI planning pass evidence, Claude/Codex conflict, selected gate-cleanup-only scope, and required verification are recorded in the double-check summary, `NEXT_TASKS`, status, devlog, and report.
- Docs-only loop update completed for `2026-06-02-devloop-175642-feedback-4`: missing-result diagnosis feedback, wrapper-result-created status, remaining artifact-cleanup blocker, and required verification are recorded in the double-check summary, `NEXT_TASKS`, status, devlog, and report.
- Docs-only loop update completed for `2026-06-02-devloop-193946-feedback-1`: WP2 Slice B feedback, common/ conflict synthesis, selected trusted-local QA gate, and WP3 Slice A scope guard are recorded in the double-check summary, `NEXT_TASKS`, status, devlog, and report.
- Docs-only loop update completed for `2026-06-02-devloop-193946-feedback-2`: post-loss QA runner fallback feedback, common recommendations, lack of material conflict, selected trusted-local QA-only scope, and tests required before balance reporting are recorded in the double-check summary, `NEXT_TASKS`, status, devlog, and report.
- Docs-only loop update completed for `2026-06-02-devloop-193946-feedback-3`: post-loss transport blocker feedback, common recommendations, lack of material next-scope conflict, selected trusted-local QA-only scope, and tests required before balance reporting are recorded in the double-check summary, `NEXT_TASKS`, status, devlog, and report.
- Docs-only loop update completed for `2026-06-03-devloop-050050-feedback-1`: WP3 Slice A feedback, common recommendations, lack of material next-scope conflict, selected trusted-local tactical QA-only scope, and required browser proof before balance/human reporting are recorded in the double-check summary, `NEXT_TASKS`, status, devlog, and report.
- Docs-only loop update completed for `2026-06-03-devloop-050050-feedback-2`: tactical trusted gate logging feedback, common recommendations, the Claude/Codex next-scope conflict, selected trusted-local tactical QA-only scope, and required browser proof before hint/balance/human reporting are recorded in the double-check summary, `NEXT_TASKS`, status, devlog, and report.
- Docs-only loop update completed for `2026-06-03-devloop-050050-feedback-3`: tactical blocker handoff feedback, common recommendations, lack of material next-scope conflict, selected trusted-local tactical QA-only scope, and required browser proof before balance/human reporting are recorded in the double-check summary, `NEXT_TASKS`, status, devlog, and report.
- Post-loss QA rerun loop completed with tooling fallback:
  - `npm run qa:postloss`: failed at Chrome/CDP `Target.getTargets`,
  - `npm run qa:postloss -- --timeout-ms 30000`: failed at the same point,
  - `npm run qa:pressure`: failed at the same point,
  - after adding the remote-debugging-port fallback, `npm run qa:postloss` still could not produce proof in this managed sandbox because the fallback HTTP fetch path failed,
  - trusted-local `npm run qa:postloss` remains the next gate before WP3.
- Post-loss transport blocker documentation loop completed:
  - `node --check scripts/run_browser_pressure_qa.js`: passed,
  - latest `npm run qa:postloss` now fails with explicit `BrowserQaTransportError`,
  - environment blocker prompt added: `docs/review_prompts/2026-06-02-postloss-browser-transport-blocker.md`,
  - WP3 remains blocked until trusted-local post-loss browser proof or a reviewed environment-blocker decision.
- Trusted-local post-loss gate wrapper completed:
  - `scripts/run_trusted_postloss_gate.js` and `npm run qa:postloss:trusted` added,
  - `node --check scripts/run_trusted_postloss_gate.js`: passed,
  - `npm run doctor`: 44 pass, 0 warn, 0 fail,
  - `npm run doctor:deep`: 64 pass, 0 warn, 0 fail,
  - `npm run qa:postloss:trusted`: failed before gameplay evaluation in this sandbox after standard run plus 30000 ms retry,
  - next execution remains sandbox outside trusted-local `npm run qa:postloss:trusted` or equivalent manual `qa:postloss` plus one timeout retry.
- Trusted-local tactical gate wrapper completed:
  - `scripts/run_trusted_tactical_gate.js` and `npm run qa:tactical:trusted` added,
  - tactical blocker handoff prompt added: `docs/review_prompts/2026-06-03-tactical-browser-transport-blocker.md`,
  - `node --check scripts/run_trusted_tactical_gate.js`: passed,
  - `node --check scripts/check_local_pipeline.js`: passed,
  - `npm run qa:tactical`: failed before gameplay evaluation in this sandbox with CDP `Target.getTargets` timeout and `127.0.0.1 listen EPERM`,
  - `npm run qa:tactical:trusted`: wrote `alpha_test/outputs/tactical-trusted-gate/latest.json` with `status: blocked`, `transportFailure: true` after standard run plus 30000 ms retry,
  - latest managed-sandbox rerun of `npm run qa:tactical:trusted`: still `status: blocked`, `transportFailure: true`; both the standard run and 30000 ms retry stopped before gameplay evaluation,
  - next execution remains sandbox-outside trusted-local `npm run qa:tactical:trusted`.
- Current dev-loop prompt cleanup is implemented: future nested implementation prompts should not keep re-selecting WP1 after WP1 is complete.
- Current dev-loop preflight-order cleanup is implemented: future clean-tree dev loops should run preflight before creating their own loop log, and should not mask dirty-tree state with `--allow-dirty` by default.
- Current autopilot preflight diagnosis now gives exact loop-run artifact cleanup guidance when `docs/loop_runs/*.md` blocks a clean unattended loop.
- Report work-unit heading rule:
  - daily reports still live in `docs/reports/YYYY-MM-DD.md`,
  - each top-level work section after the daily title must be `# YYYY-MM-DD-NN - 작업 제목`,
  - `npm run report:check` and `doctor` enforce the latest report,
  - Discord `--latest-section` should now send a clearly named task/unit instead of an ambiguous daily tail.
- Post-loop gate closure:
  - working tree clean after `f6ee83f feat: 자동 개발 루프 4차 반영`,
  - `npm run autopilot:preflight`: 21 pass, 0 warn, 0 fail,
  - `npm run qa:identity`: `status: complete`, failures `[]`, `buildIdentitySeenBy90Sec: true`.
- WP1 gate is officially complete for automation purposes. WP2 Slice A pressure rhythm/high-low pacing and WP2 Slice B minimal post-loss challenge are implemented, and WP2 Slice B has trusted-local browser proof. WP3 Slice A minimal tactical focus is code-complete but not browser-proven. The next executable gate is sandbox-outside trusted-local `npm run qa:tactical:trusted`.
- On another local machine, run `npm run doctor` first; run `npm run doctor:deep` before leaving Codex to continue unattended.
- Before an unattended implement -> Claude feedback -> implement loop, run `npm run autopilot:preflight`.
- Do not describe AI proxy metrics as real balance feedback.
- Use Claude + Codex CLI double check for major planning changes.
- Before reporting balance, separate AI simulator evidence, browser flow QA evidence, and user play evidence.
- Do not request user 1-person playtest until v0.9 has browser-visible build identity, pressure rhythm, post-loss challenge evidence, and tactical-focus browser proof or a documented environment-blocker decision.

## Latest Override - 2026-06-05 Boss TTK

- `npm run qa:boss-ttk` now runs an in-process boss-only deterministic TTK harness for the first boss.
- Latest default result: 5/5 accepted samples, first boss HP `3500`, TTK median `21.92s`, focused DPS median `159.7`.
- Evidence: `docs/balance/2026-06-05-v012-boss-ttk-harness-final.md`.
- Overall balance is still `ITERATE_BALANCE` until browser `qa:balance` rechecks first-boss reach, clear, death, and post-boss flow with HP `3500`.

## Latest Override - 2026-06-05 HP 2800 Follow-Up

- First boss HP is now `2800`.
- Boss-only HP `2800` result: 5/5 accepted samples, TTK median `17.8s`, focused DPS median `157.3`, verdict `GO_BOSS_TTK_SAMPLE`.
- Browser `first_boss_ttk` HP `2800` result: 1/3 accepted sample, accepted TTK `22.59s`, 2/3 incomplete.
- Browser full `qa:balance` HP `2800` result: first boss clear `60%`, death `0%`, TTK median `53.21s`, but 2/5 incomplete.
- Current blocker is browser accepted-sample stability, not another immediate boss HP guess.

## Latest Override - 2026-06-06 Browser First Boss TTK Terminal

- Browser `first_boss_ttk` now terminates as `complete` when the first boss TTK sample is recorded.
- The browser QA summarizer now uses first-boss-only checks for `--scenario first_boss_ttk` instead of full-run clear, level-up, and slot-fill checks.
- HP remains `2800`.
- Browser `first_boss_ttk` result: 3/3 accepted samples, TTK median `25.76s`, first boss clear `100%`, verdict `GO_BALANCE_BASELINE`.
- Full browser `qa:balance` after the fix: first boss clear `80%`, death `20%`, first boss TTK median `27.79s`, level-ups before first boss median `11`, verdict `ITERATE_BALANCE`.
- Current blocker moved from first-boss TTK sample stability to post-boss/full-run flow: full clear is still `0%`, and one run died during the forget-warning phase.

## Latest Override - 2026-06-06 Balance Baseline

- First boss HP is now `2500`.
- Browser `first_boss_ttk` HP `2500`: 3/3 accepted samples, TTK median `21.05s`, first boss clear `100%`, verdict `GO_BALANCE_BASELINE`.
- Full browser `qa:balance`: verdict `GO_BALANCE_BASELINE`, first boss clear `100%`, full clear `40%`, death `60%`, first boss TTK median `22.24s`, level-ups before first boss median `10`.
- Full-run QA window is now `690s` so the final scheduled boss has time to resolve after spawning at `600s`.
- Post-boss spawn caps are now lower: deficit breath `16`, deficit trial `22`, later-cycle default `58`.
- Current next task is interpretation/design direction: death remains concentrated in deficit trial, but the automated baseline gates pass.

## Latest Override - 2026-06-06 Deficit Trial Survival Tuning

- Balance goal: reduce death from `60%` to `<= 40%` while keeping first boss TTK in `15-30s` and full clear within the automated `35-80%` band.
- Deficit duration is now `60s`.
- Pre-boss XP multiplier is now `1.95`.
- Deficit trial cap is now `16`.
- Refill now restores the player to at least `85%` HP and grants shield `18`.
- Low-HP level-up rolls now force-offer survival once, and QA chooses survival below `78%` HP.
- Full browser `qa:balance`: verdict `GO_BALANCE_BASELINE`, first boss clear `100%`, full clear `80%`, death `20%`, first boss TTK median `27.84s`.
- Risk: full clear is exactly at the automated upper bound, so the next reviewer should judge whether the prototype became too forgiving after refill.

## Latest Override - 2026-06-06 Deficit Trial Review Follow-Up

- External Claude review was attempted but blocked by the approval reviewer because sending workspace-derived prompt content to an external Claude service is treated as potential private data exfiltration in this Codex session.
- Local fallback review saved: `docs/review_responses/2026-06-06-balance-baseline-deficit-trial-codex.md`.
- Review decision: `ITERATE_DEFICIT_TRIAL`; full clear `80%` was treated as too close to the automated upper bound.
- First boss HP remains `2500`.
- Balance QA selection now avoids over-prioritizing survival before the first boss, but still uses the stronger survival threshold after the first boss.
- Final browser `qa:balance`: verdict `GO_BALANCE_BASELINE`, first boss clear `80%`, full clear `60%`, death `40%`, first boss TTK median `25.79s`, level-ups before first boss median `11`.
- Final boss-only HP `2500`: 5/5 accepted, TTK median `15.62s`.
- Final browser `first_boss_ttk`: 3/3 accepted, TTK median `19.82s`.

## Latest Override - 2026-06-07 v0.12 Human Playtest Package

- Accepted the latest automated balance baseline as the current pre-human-test candidate instead of continuing blind numeric tuning.
- Human playtest guide is now v0.12-specific: `docs/HUMAN_PLAYTEST_GUIDE.md`.
- Playtest notes template is now v0.12-specific: `docs/PLAYTEST_NOTES.md`.
- New session sheet: `docs/playtest/2026-06-07-v012-human.md`.
- `npm run playtest:package` now includes `V012_HUMAN_PLAYTEST_SHEET.md` instead of the old v0.7 solo sheet.
- Verification:
  - `node --check scripts/prepare_playtest_build.js`: pass.
  - `npm run playtest:package:dry`: pass, output target `dist\lethe-v0.12-playtest`.
  - `npm run playtest:package`: pass, generated `dist\lethe-v0.12-playtest`.

## Latest Override - 2026-06-07 Balance Loop Gate Fix

- User flagged that balance QA should run through the loop, not only one-off `qa:balance`.
- `npm run autopilot:preflight:local`: 20 pass / 1 warn / 0 fail; warning is the expected skipped live Claude auth check in local mode.
- `scripts/run_balance_loop.js` now defaults to `690s`, shows run-sec/timeout/out/report in dry-run, and accepts PowerShell/npm positional arguments.
- `scripts/run_browser_balance_qa.js` now enforces death rate max `<= 0.4`; death `60%` no longer passes as `GO_BALANCE_BASELINE`.
- Balance changes:
  - first boss HP `2500 -> 2050`,
  - deficit duration `60s -> 54s`,
  - enemy damage scaling `0.03/0.01 -> 0.025/0.008`,
  - first-cycle rising cap `36 -> 34`,
  - deficit trial cap `16 -> 14`,
  - later-cycle/default caps `58 -> 46`,
  - refill HP floor `85% -> 95%`, shield `18 -> 24`.
- Final browser `first_boss_ttk`: `GO_BALANCE_BASELINE`, 3/3 accepted, first boss TTK median `18.61s`.
- Final `npm run balance:loop -- 5 690 60000 ...`: `GO_BALANCE_BASELINE`, first boss clear `100%`, full clear `60%`, death `40%`, first boss TTK median `20.73s`.

## Latest Override - 2026-06-08 Playtest Package Rerun

- Reran the v0.12 playtest package after the latest balance loop gate fix.
- Verification:
  - `npm run playtest:package:dry`: pass, output target `dist\lethe-v0.12-playtest`.
  - `npm run playtest:package`: pass, regenerated `dist\lethe-v0.12-playtest`.
  - `npm run report`: pass.
  - `npm run report:check`: pass.
  - `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
  - `npm run report:discord:unit:dry`: pass.
- Discord actual send:
  - `npm run report:discord:unit` was blocked by the approval reviewer because sending workspace-generated reports/attachments to an external Discord webhook is treated as potential private data exfiltration in this Codex session.
  - Next trusted-local command: `npm run report:discord:unit`.
- Current next task: run controlled human sessions, place downloaded JSON logs in `playtest_logs/`, then run `npm run playtest:summary`.

## Latest Override - 2026-06-08 Orchestration Adoption

- Added `docs/orchestration/` as the shared Codex project-management interface for LETHE.
- Added a root `AGENTS.md` Orchestration Interface section without replacing existing project-specific rules.
- Created orchestration core files:
  - `README.md`,
  - `PROJECT_BRIEF.md`,
  - `STATUS.md`,
  - `CURRENT_TASK.md`,
  - `NEXT_TASKS.md`,
  - `PROMPT_CONTEXT.md`,
  - `RUNBOOK.md`,
  - `SCOPE_GUARD.md`,
  - `DECISION_LOG.md`.
- Created extension folders and seed files for `devlog/`, `reports/`, `review_prompts/`, `review_responses/`, `evidence/`, and `templates/`.
- Verified by reading `docs/orchestration/README.md`, `STATUS.md`, `CURRENT_TASK.md`, and `NEXT_TASKS.md`.
- Verification:
  - `npm run report`: pass.
  - `npm run report:check`: pass after rerun; the first parallel check raced report generation.
  - `npm run report:discord:unit:dry`: pass.
  - `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- Discord actual send:
  - `npm run report:discord:unit` was blocked by the approval reviewer because sending workspace-generated reports/attachments to an external Discord webhook is treated as potential private data exfiltration in this Codex session.
  - Next trusted-local command: `npm run report:discord:unit`.
- Legacy docs remain in place and are treated as detailed archives; orchestration files are the quick resume interface.

## Latest Override - 2026-06-08 Orchestration Dashboard Refresh

- Updated `docs/orchestration/index.html` so the top summary reflects the current project state.
- Current stage now reads as v0.12 controlled human-test gate.
- Latest verification now highlights `balance:loop GO`, full clear `60%`, and death `40%`.
- Current blocker now highlights missing human reaction evidence.
- Updated `docs/orchestration/STATUS.md` and `docs/orchestration/NEXT_TASKS.md` to match the dashboard state.
- Verification:
  - `npm run report`: pass.
  - `npm run report:check`: pass.
  - `npm run report:discord:unit:dry`: pass.
  - `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- Discord actual send:
  - `npm run report:discord:unit` was blocked by the approval reviewer because sending workspace-generated reports/attachments to an external Discord webhook is treated as potential private data exfiltration in this Codex session.
  - Next trusted-local command: `npm run report:discord:unit`.
- Current next task remains controlled human sessions, JSON logs in `playtest_logs/`, then `npm run playtest:summary`.

## Latest Override - 2026-06-08 Korean Dashboard Normalization

- Rebuilt `docs/orchestration/index.html` as a Korean human-facing command dashboard.
- Added a copyable recommended prompt for the next Codex action.
- Added `docs/orchestration/reports/index.html` and `docs/orchestration/devlog/index.html`.
- Updated orchestration docs to state:
  - Markdown is the source of truth,
  - HTML is a readable user view,
  - the dashboard shows the last completed state, not live in-progress Codex thoughts,
  - the dashboard should help the user decide the next prompt.
- Verification:
  - `npm run report`: pass.
  - `npm run report:check`: pass.
  - `npm run report:discord:unit:dry`: pass.
  - `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- Discord actual send:
  - `npm run report:discord:unit` was blocked by the approval reviewer because sending workspace-generated reports/attachments to an external Discord webhook is treated as potential private data exfiltration in this Codex session.
  - Next trusted-local command: `npm run report:discord:unit`.
- Current next task remains controlled human sessions, JSON logs in `playtest_logs/`, then `npm run playtest:summary`.

## Latest Override - 2026-06-08 HTML Interface Contract Alignment

- Updated the current orchestration HTML changes to match the revised `EXISTING_PROJECT_MIGRATION_PROMPT.md`.
- Required human-facing HTML interface:
  - `docs/orchestration/index.html`: Korean project dashboard.
  - `docs/orchestration/command.html`: compact next-instruction block.
  - `docs/orchestration/runbook.html`: operating-procedure block.
- Optional browse pages remain:
  - `docs/orchestration/reports/index.html`
  - `docs/orchestration/devlog/index.html`
- Added orchestration-local report HTML files under `docs/orchestration/reports/` so user-facing reports can be opened from the orchestration folder.
- Updated `AGENTS.md`, orchestration README/RUNBOOK/STATUS/NEXT_TASKS, and `DECISION_LOG.md` so Markdown remains the source of truth and generated HTML is the human-facing project interface.
- Verification:
  - `npm run report`: pass, generated 5 unit reports.
  - `npm run report:check`: pass, 5 report units.
  - `npm run report:discord:unit:dry`: pass, latest unit 05 summary generated.
  - `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- Discord actual send:
  - `npm run report:discord:unit` was rejected by the approval reviewer because it would send workspace-generated report content and attachments to an external Discord webhook without explicit user approval for that exfiltration.
  - Next trusted-local command: `npm run report:discord:unit`.
- Current next task remains controlled human sessions, JSON logs in `playtest_logs/`, then `npm run playtest:summary`.

## Latest Override - 2026-06-08 Project Dashboard Compaction

- Rebuilt `docs/orchestration/index.html` as a shorter 30-second project dashboard.
- Removed next-instruction duplication from the dashboard; `command.html` remains the compact prompt/done-criteria surface.
- Removed broad document browsing and Codex-comparison copy from the main dashboard.
- Dashboard now focuses on current stage, latest verification, blocker, next gate, current conclusion, recent completion, and primary links.
- Updated orchestration README/RUNBOOK/STATUS/NEXT_TASKS and decision log to preserve the role split.
- Verification:
  - `npm run report`: pass, generated 6 unit reports.
  - `npm run report:check`: pass, 6 report units.
  - `npm run report:discord:unit:dry`: pass, latest unit 06 summary generated.
  - `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- Discord actual send:
  - `npm run report:discord:unit` was rejected by the approval reviewer because it would send workspace-generated report content and attachments to an external Discord webhook without explicit user approval for that exfiltration.
  - Next trusted-local command: `npm run report:discord:unit`.
- Current next task remains controlled human sessions, JSON logs in `playtest_logs/`, then `npm run playtest:summary`.

## Latest Override - 2026-06-08 Project Dashboard Surface Trim

- Trimmed `docs/orchestration/index.html` further into a status-only dashboard.
- Changed heading from `30초 상태 요약` to `상태 요약`.
- Removed the explanatory subtitle and bottom cards for command/runbook/reports.
- Kept current state, latest verification, blocker, next gate, current conclusion, current goal, next judgment, recent completion, and date.
- Verification:
  - `npm run report`: pass, generated 7 unit reports.
  - `npm run report:check`: pass, 7 report units.
  - `npm run report:discord:unit:dry`: pass, latest unit 07 summary generated.
  - `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
  - `Select-String` dashboard check: only `상태 요약` remained; removed target strings were absent.
- Discord actual send:
  - `npm run report:discord:unit` was rejected by the approval reviewer because it would send workspace-generated report content and attachments to an external Discord webhook, which is untrusted by default and not explicitly approved by the user.
  - Next trusted-local command: `npm run report:discord:unit`.
- Current next task remains controlled human sessions, JSON logs in `playtest_logs/`, then `npm run playtest:summary`.

## Latest Override - 2026-06-08 Plugin-Oriented Migration Prompt Update

- Updated `EXISTING_PROJECT_MIGRATION_PROMPT.md` for a reusable personal AI plugin-style orchestration structure.
- New target convention:
  - `docs/orchestration/interface/`: human-facing HTML (`index.html`, `command.html`, `runbook.html`).
  - `docs/orchestration/state/`: AI-facing Markdown source of truth.
  - `docs/orchestration/reports/`: people-facing HTML work-unit reports.
  - `docs/orchestration/devlog/`: AI/internal Markdown continuity.
  - `docs/orchestration/legacy/`: migration maps, archived docs, and pointers.
- Added rules for migrating existing docs so old project-management docs outside orchestration stop being required for normal resume.
- Current repo note: the prompt is updated, but LETHE still needs a follow-up physical migration to the new folder layout.
- Verification:
  - `npm run report`: pass, generated 8 unit reports.
  - `npm run report:check`: pass, 8 report units.
  - `npm run report:discord:unit:dry`: pass, latest unit 08 summary generated.
  - `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- Discord actual send:
  - `npm run report:discord:unit` was rejected by the approval reviewer because it would send workspace-generated report content and attachments to an external Discord webhook, which is untrusted by default and not explicitly approved by the user.
  - Next trusted-local command: `npm run report:discord:unit`.
- Current next product task remains controlled human sessions, JSON logs in `playtest_logs/`, then `npm run playtest:summary`.

## Latest Override - 2026-06-08 v0.12 Balance Loop Rerun

- User requested the next balance test.
- Command:
  - `npm run balance:loop`
- Result:
  - verdict: `ITERATE_BALANCE`,
  - first boss clear: `100%`,
  - full clear: `20%`,
  - death: `60%`,
  - first boss TTK median: `26.42s`.
- Failed checks:
  - clear rate minimum: `20%`, target `>= 35%`,
  - death rate maximum: `60%`, target `<= 40%`.
- Death phase cluster: `망각 전조` in 3 death runs.
- Generated:
  - `docs/balance/2026-06-08-v012-balance-qa.md`,
  - `docs/review_prompts/2026-06-08-balance-loop.md`.
- Updated orchestration status/current task/next tasks/dashboard/command view to pause human sessions until balance gate is restored or the user explicitly accepts the risk.
- Verification:
  - `npm run report`: pass, generated 9 unit reports.
  - `npm run report:check`: pass, 9 report units.
  - `npm run report:discord:unit:dry`: pass, latest unit 09 summary generated and attached the balance review prompt.
  - `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- Discord actual send:
  - `npm run report:discord:unit` was rejected by the approval reviewer because it would send workspace-generated report content and attachments to an external Discord webhook, which is untrusted by default and not explicitly approved by the user.
  - Next trusted-local command: `npm run report:discord:unit`.

## Latest Override - 2026-06-08 Orchestration Report/Devlog Migration

- Legacy `docs/reports/` and `docs/devlog/` were physically migrated into `docs/orchestration/`.
- New report source of truth:
  - `docs/orchestration/reports/YYYYMMDD/index.md`
  - `docs/orchestration/reports/YYYYMMDD/index.html`
  - `docs/orchestration/reports/YYYYMMDD/units/`
- New devlog source of truth:
  - `docs/orchestration/devlog/YYYYMMDD.md`
- Human-facing HTML interface moved to `docs/orchestration/interface/`.
- AI-facing state Markdown moved to `docs/orchestration/state/`.
- `docs/orchestration/reports/20260608/index.md` was rewritten as a Korean date-based report with 10 work units.
- Verification:
  - `node --check scripts/build_report.js`: pass.
  - `node --check scripts/check_report_units.js`: pass.
  - `node --check scripts/send_discord_report.js`: pass.
  - `node --check scripts/run_autonomous_dev_loop.js`: pass.
  - `npm run report`: pass, generated `docs/orchestration/reports/20260608/index.html` and 10 unit reports.
  - `npm run report:check`: pass, 10 report units.
  - `npm run report:discord:unit:dry`: pass, latest unit 10 points to the new orchestration report path.
  - `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- Discord actual send:
  - `npm run report:discord:unit` was rejected by the approval reviewer because it would send workspace-generated report content and attachments to an external Discord webhook, which is untrusted by default and not explicitly approved by the user.
  - Next trusted-local command: `npm run report:discord:unit`.
