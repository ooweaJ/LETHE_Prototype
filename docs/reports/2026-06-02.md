# LETHE 개발 보고서 - 2026-06-02

# 2026-06-02-01 - v0.2 망각 이해도 튜닝과 브라우저 QA

## 1. Current build status

- LETHE HTML alpha moved from v0.1 into v0.2 narrow tuning.
- The goal was not more content, but making the first forgetting event understandable and testable.
- v0.2 reached AI `GO_CANDIDATE` and browser QA candidate status.

## 2. What changed today

- Converted GPT/Claude v0.2 planning into Korean task docs.
- Tuned first forgetting timing toward the 8-10 minute target.
- Tuned boss/forgetting timing around a 9-minute browser run.
- Reduced single-memory deletion skew, especially `처형자의 섬광`.
- Added clearer result screen fields: forgotten memory, prediction result, deletion weight, remaining echo, next build direction.
- Added Q3 free-response recall and JSON fields for selected/predicted/deleted memories and deletion weights.

## 3. Test results and evidence

- `npm run ai:test:quick`: `GO_CANDIDATE`, Alpha Fun Score around `0.7798`.
- `npm run ai:test`: `GO_CANDIDATE`, Alpha Fun Score around `0.7737`.
- Browser `?qa=fast` verified result screen and JSON payload.
- Report/Discord dry-runs confirmed Korean summary format.

## 4. Decisions made

- v0.2 should validate whether forgetting feels like understandable loss.
- Do not expand content before result-screen and JSON evidence are readable.
- Keep Discord as a concise status channel, not the source of truth.

## 5. Problems or risks

- v0.2 was still early and not yet a release-feel prototype.
- AI metrics were enough for a candidate signal, not proof of human emotion.
- Browser QA had to remain part of the gate before people testing.

## 6. GPT handoff summary

- v0.2 asks whether the first forgetting is predictable, legible, and regrettable rather than irritating.
- Human feedback should focus on regret, irritation, neutral reactions, unclear reactions, and memory-name recall.
- GPT/Claude should use test evidence, not vibes, to choose the next version.

## 7. Next Codex tasks

- Continue pre-human-test polish only where readability or evidence quality is weak.
- Keep browser QA and JSON payload verification mandatory.
- Prepare v0.3 only if the result-screen/forgetting loop needs more clarity before people testing.

## 8. Portfolio notes

- Problem: v0.1 had a promising emotional loop but the first loss could be too early and hard to parse.
- Direction: narrow the prototype around a testable first forgetting moment.
- Action: tuned timing, deletion skew, result UI, survey, JSON logging, and Discord/report flow.
- Result: v0.2 became a better evidence-gathering build rather than a content expansion.

# 2026-06-02-02 - v0.3-v0.4 전투 가시성과 사람 테스트 후보화

## 1. Current build status

- LETHE moved through v0.3 combat readability and v0.4 human-test candidate polish.
- v0.4 reached stronger AI scores and clearer browser evidence.
- The build was closer to people-test readiness but still below the long-term Alpha Fun Score target.

## 2. What changed today

- Added Codex CLI planning fallback so Claude auth issues would not stop planning.
- Added combat readability polish:
  - floating memory names,
  - damage numbers,
  - hit sparks,
  - projectile trails,
  - boss spawn/phase impact feedback,
  - dependency tag and dependency percent in memory slots.
- Updated result screen to separate lost action from echo transformation.
- Added `echoTransformation` to JSON.
- Raised default UI clarity to `0.78`.

## 3. Test results and evidence

- `npm run review:codex:dry`: planning fallback path confirmed.
- `npm run ai:test:quick`: `GO_CANDIDATE`, Alpha Fun Score around `0.8190` after v0.4 polish.
- `npm run ai:test`: `GO_CANDIDATE`, Alpha Fun Score around `0.8261`, regret around `85.6%`, irritation around `0.4%`.
- `npm run ai:test:heavy`: `GO_CANDIDATE`, Alpha Fun Score around `0.8369`, regret around `87.6%`, irritation around `0.3%`.
- Browser QA confirmed v0.3/v0.4 labels and payload fields.

## 4. Decisions made

- v0.3 focused on combat legibility rather than new systems.
- v0.4 focused on making loss and echo readable enough for people to answer test questions.
- External planning fallback is required because Claude auth can be unavailable in some sessions.

## 5. Problems or risks

- Alpha Fun Score improved but stayed below the aspirational `0.89+`.
- Combat readability improved, but release-feel fun was still not proven.
- AI `GO_CANDIDATE` was still not a human-test result.

## 6. GPT handoff summary

- v0.3/v0.4 improved how players see memory use, dependency risk, lost action, and echo transformation.
- The next planning question was whether this was enough for people testing or whether early fun needed improvement first.
- GPT/Claude should treat v0.4 as a candidate, not a finished prototype.

## 7. Next Codex tasks

- Verify level-up/run-fun if human testing is delayed.
- Keep browser QA in the loop for every UI payload change.
- Keep the human playtest guide aligned with the current version.

## 8. Portfolio notes

- Problem: the prototype could pass simulation while still feeling visually flat or unclear.
- Direction: make memory activations and forgetting risk visible during play.
- Action: added combat feedback, dependency labels, result-screen separation, and payload fields.
- Result: v0.4 became a stronger human-test candidate, but not yet a release-feel game loop.

# 2026-06-02-03 - v0.5 초반 재미와 런 중 성장 보강

## 1. Current build status

- LETHE moved into v0.5 core-fun candidate status.
- The user feedback was that the game needed to be fun enough for players to reach forgetting.
- v0.5 added early combat pressure and in-run growth without adding meta progression or shop systems.

## 2. What changed today

- Increased early enemy density and combat pressure.
- Added kill XP and run-only level-up choices.
- Added `runGrowth` payload data.
- Added AI early-fun metrics:
  - Early Fun Score,
  - early kill tempo,
  - early level-ups,
  - early crowd pressure.
- Added level-up browser QA mode and verified level-up pause/resume flow.
- Updated Claude v0.5 evaluation and human playtest guide.

## 3. Test results and evidence

- `npm run ai:test`: `GO_CANDIDATE`, Alpha Fun Score around `0.8531`, Early Fun around `0.8669`, kill tempo around `0.9620`, level-ups around `4.08`.
- `npm run ai:test:heavy`: `GO_CANDIDATE`, Alpha Fun Score around `0.8509`, Early Fun around `0.8672`.
- `?qa=fast,levelup` Chrome headless QA verified level-up DOM, selection, resume, and `runGrowth` payload.
- Claude v0.5 evaluation was recorded and interpreted as conditional on browser validation.

## 4. Decisions made

- Improve the early combat/growth loop before broad people testing.
- Keep growth run-only; no meta progression, shop, unlocks, or permanent growth.
- Do not add more memories while the current six memories are not fully proven.

## 5. Problems or risks

- Alpha Fun improved but still did not prove human enjoyment.
- Simulation proxies could miss feel issues in combat rhythm and choice interest.
- Human testing remained useful but should not happen if the prototype still felt too thin.

## 6. GPT handoff summary

- v0.5 asks whether players have enough moment-to-moment fun to keep playing until forgetting.
- Evidence says early fun improved, but the user later judged that release-feel depth still needed work.
- GPT/Claude should separate "AI can clear it" from "a human wants to replay it."

## 7. Next Codex tasks

- Use test-result planning pipeline for future design decisions.
- Watch earlyChoiceInterest and echoPivotScore as weak-but-not-yet-actionable signals.
- Keep human-test packaging ready but pause it if the core loop is not yet compelling.

## 8. Portfolio notes

- Problem: good forgetting metrics do not matter if players quit before the first meaningful loss.
- Direction: strengthen early combat tempo and in-run choices.
- Action: added denser waves, XP, level-ups, and early-fun metrics.
- Result: v0.5 improved early playability but exposed the need for a deeper run structure.

# 2026-06-02-04 - 테스트 결과 기반 기획 파이프라인과 운영 문서화

## 1. Current build status

- The project shifted from ad hoc AI opinions to test-result-driven planning.
- HTML prototype validation remained the current project goal.
- Unity implementation stayed conditional on stronger evidence.

## 2. What changed today

- Added planning pipeline scripts and dry-run modes.
- Added Claude/Codex planning handoff docs.
- Added local doctor and deep doctor.
- Added portfolio/ADR/AI collaboration docs.
- Added human playtest summary automation.
- Added human playtest package generation.
- Improved Discord status/report delivery and dry-runs.

## 3. Test results and evidence

- `npm run planning:pipeline:prompt`: generated prompt with quick AI evidence.
- `npm run doctor`: checked runtime, scripts, docs, CLI availability, env, and latest prompt.
- `npm run doctor:deep`: added syntax checks and dry-runs.
- `npm run playtest:summary:dry`: verified summary automation path.
- `npm run playtest:package:dry`: verified package generation path.

## 4. Decisions made

- Markdown files are the source of truth.
- Discord is a status/attention channel only.
- Claude/GPT should interpret results and revise planning, but Codex owns HTML prototype implementation and verification.
- Before unattended loops, run preflight.

## 5. Problems or risks

- CLI auth and Discord webhook availability vary by environment.
- Automation can create a false sense of progress if it does not preserve test evidence.
- AI planning passes must not be treated as human fun proof.

## 6. GPT handoff summary

- Future planning prompts should include concrete test outputs, browser QA evidence, risks, and the current scope guard.
- GPT/Claude should return verdict, next tasks, test criteria, and do-not-build list.
- Codex should then implement only the selected narrow scope.

## 7. Next Codex tasks

- Keep `npm run doctor` and `npm run doctor:deep` green.
- Keep report/Discord dry-runs available.
- Use the planning pipeline after AI or human tests before major design changes.

## 8. Portfolio notes

- Problem: AI collaboration can become noisy without a repeatable evidence loop.
- Direction: build a test-result planning pipeline with clear role split.
- Action: added scripts, docs, doctor checks, playtest summary, and package automation.
- Result: future iteration can run as a loop instead of a one-off prompt exchange.

# 2026-06-02-05 - v0.6 코어 런 구조 재설계

## 1. Current build status

- LETHE moved from a single first-loss prototype toward a 20-minute run structure.
- The goal was to test whether repeated loss/recovery cycles could sustain a roguelike loop.
- The scope remained HTML prototype validation only.

## 2. What changed today

- Added a 20-minute structure with bosses at 4, 8, 12, 16, and 20 minutes.
- Boss defeat now leads to dependency-based memory loss.
- Added a 2-memory deficit survival segment after loss.
- Added memory refill from 3 candidates after the deficit segment.
- Added `runTimeline` JSON payload with cycles and refill choices.
- Added `?qa=fast,v06` browser QA gate.

## 3. Test results and evidence

- AI metrics stayed in `GO_CANDIDATE` range after the run-structure change.
- Browser QA validated the v0.6 flow and payload hooks.
- Timeline payload recorded cycles and refill choices for later interpretation.

## 4. Decisions made

- The core loop should be tested as repeated loss, deficit survival, and recovery.
- Keep the current six memories and three active slots.
- Do not add a shop, meta progression, or multi-region structure.

## 5. Problems or risks

- A longer structure could expose balance issues not visible in short-run metrics.
- Deficit survival needs to feel challenging, not irritating.
- Refill choices must feel like recovery/pivot, not a full undo of loss.

## 6. GPT handoff summary

- v0.6 created the first real run-loop scaffold.
- GPT/Claude should evaluate whether loss/recovery cadence supports regret and replay desire.
- If the loop is too flat, the next pass should strengthen build identity, pressure rhythm, and post-loss challenge before adding content.

## 7. Next Codex tasks

- Run balance/proxy checks on the new structure.
- Inspect whether two-memory survival and refill are understandable.
- Prepare v0.7/v0.8 only if evidence says the loop needs clearer agency or balance.

## 8. Portfolio notes

- Problem: one forgetting event was not enough to prove LETHE as a roguelike loop.
- Direction: add a repeatable run cycle without broadening scope.
- Action: implemented boss cycles, deficit survival, refill choices, and run timeline logging.
- Result: LETHE became a run-structure prototype rather than a one-event test.

# 2026-06-02-06 - v0.7 무기/잔향 밸런스와 판정 정정

## 1. Current build status

- v0.7 improved weapon baselines and lost-memory echo effects.
- AI and Claude planning signals initially suggested solo/human test candidacy.
- User direct feedback later invalidated the balance confidence.

## 2. What changed today

- Buffed the two existing weapon baselines.
- Added weapon-facing echo effects for lost memories.
- Added side-panel echo labels for weapon residue effects.
- Updated AI simulator weapon base DPS and residue proxy.
- Recorded Claude v0.7 feedback and the later correction that AI balance confidence was too optimistic.

## 3. Test results and evidence

- `npm run ai:test` and related checks stayed in `GO_CANDIDATE` range.
- Claude v0.7 balance verdict leaned toward solo/human testing.
- User direct feedback judged the balance verdict unreliable and the game not yet ready.

## 4. Decisions made

- User live play feedback outranks AI planning/proxy metrics.
- Do not proceed to people testing just because Claude or AI proxies say `GO`.
- Fix the automatic balance proxy before trusting it for major decisions.

## 5. Problems or risks

- AI proxies can miss subjective balance and fun issues.
- Weapon/echo improvements can look good numerically while still feeling off.
- The project needed a stronger release-feel plan before more people testing.

## 6. GPT handoff summary

- v0.7 improved the prototype but also exposed that AI verdicts were overconfident.
- GPT/Claude should treat future `GO` outputs as planning passes until browser/user evidence supports them.
- The next plan should be stricter and more reference-driven.

## 7. Next Codex tasks

- Start v0.8 double-check planning for major design changes.
- Improve danger/death metrics and balance proxy reliability.
- Keep scope guard active: no new memories, shop, meta progression, final boss, or multi-region run.

## 8. Portfolio notes

- Problem: simulated balance confidence diverged from user play feel.
- Direction: make AI planning subordinate to direct play evidence.
- Action: recorded the verdict correction and changed the planning default to double-check.
- Result: future work became more cautious, evidence-based, and user-feedback-led.

# 2026-06-02-07 - v0.8 더블체크와 Gate A/B 자동 밸런스 정비

## 1. Current build status

- v0.8 became the stricter evidence/planning phase after v0.7 feedback.
- The project started Gate A and Gate B work instead of rushing to people testing.
- Broad human testing was paused.

## 2. What changed today

- Added v0.8 double-check planning as the default for major direction changes.
- Started Gate A around death bug and real danger metrics.
- Removed HP 1 death-prevention behavior.
- Added real death/run-end handling.
- Added `death` and `danger` JSON payload fields.
- Added `?qa=fast,death` death QA mode.
- Continued Gate B automatic balance-loop reporting.

## 3. Test results and evidence

- Doctor/deep doctor checks expanded as automation scripts grew.
- AI quick/default/heavy checks remained available but were treated as proxy evidence.
- Death QA and danger payloads improved the ability to distinguish real danger from simulated survivability.

## 4. Decisions made

- Major design changes require Claude plus Codex CLI double-check.
- Browser combat evidence outranks aggregate AI proxy metrics.
- A `GO` from either AI is only an AI planning pass until browser/user play validates it.

## 5. Problems or risks

- Current metrics still did not prove release-feel fun.
- Danger/death evidence can improve reliability but not replace human response.
- Automation can loop on stale tasks if the prompt does not identify the foremost unfinished scope.

## 6. GPT handoff summary

- v0.8 was a trust-repair phase for the testing and planning loop.
- GPT/Claude should help decide next implementation slices only after checking conflicts and evidence quality.
- The user had already signaled that the prototype needed a stronger game loop before broad human testing.

## 7. Next Codex tasks

- Prepare v0.9 release-feel planning.
- Research comparable roguelike loops and adapt only patterns that fit LETHE's current scope.
- Build overnight/autonomous loop infrastructure carefully.

## 8. Portfolio notes

- Problem: the previous loop could produce overconfident AI verdicts.
- Direction: add double-check planning and better danger/death gates.
- Action: implemented death/danger payloads, double-check process, and stricter evidence rules.
- Result: the project had a safer basis for v0.9 release-feel work.

# 2026-06-02-08 - v0.9 출시작 감각 계획과 자동 루프 준비

## 1. Current build status

- v0.8 Gate C direct progression stopped.
- v0.9 release-feel loop became the new target.
- Human testing stayed paused until the core loop felt stronger.

## 2. What changed today

- Added roguelike reference research under `docs/research/2026-06-02-roguelike-reference.md`.
- Added v0.9 release-feel prompt.
- Added overnight loop and autonomous dev loop scripts/dry-runs.
- Added Discord progress notice flow for longer unattended work.
- Updated doctor/preflight to check new loop commands.
- Clarified that Markdown files are source of truth and Discord is status only.

## 3. Test results and evidence

- `npm run doctor`: expanded pass set as scripts were added.
- `npm run doctor:deep`: verified dry-run and syntax paths.
- `npm run overnight:loop:dry`: confirmed overnight loop command shape.
- `npm run dev:loop:dry`: confirmed autonomous loop command shape.
- Actual automation proof included Discord work-unit report and Claude prompt/response paths where available.

## 4. Decisions made

- v0.9 should improve build identity, pressure rhythm, post-loss challenge, and visible tactical agency.
- Use existing six memories and three active slots.
- Do not add meta progression, shops, final boss, new region, or more memory content.
- Let the loop continue during sleep, but make preflight failures blockers.

## 5. Problems or risks

- Automation can keep working but still work on the wrong unit if prompts are too small or stale.
- External Claude/Discord availability depends on trusted local auth/env.
- Reports must stay readable even when the loop creates many logs.

## 6. GPT handoff summary

- GPT/Claude should judge v0.9 planning using reference-driven roguelike criteria and the user's direct feedback.
- Selected initial v0.9 order: build identity first, then pressure/post-loss challenge, then minimal tactical agency.
- AI metrics remain planning evidence, not human fun proof.

## 7. Next Codex tasks

- Implement v0.9 WP1 build identity/readability.
- Verify 90-second identity visibility.
- Keep automation preflight and report delivery clean.

## 8. Portfolio notes

- Problem: the prototype was testable but not yet convincing as a release-feel roguelike slice.
- Direction: use references and automation to improve the core loop systematically.
- Action: added research, v0.9 prompts, loop scripts, Discord notices, and preflight checks.
- Result: the project could run longer implementation/test/feedback cycles without losing source-of-truth records.

# 2026-06-02-09 - v0.9 WP1 빌드 정체성과 Identity QA

## 1. Current build status

- v0.9 WP1 implemented build identity readability using existing memories.
- WP1 became automation-complete after cleanup and trusted-local identity QA.
- WP2 could start only after the identity/preflight gate closed.

## 2. What changed today

- Browser label and experiment version moved to `v0.9`.
- Memory selection cards now show role, short combat description, and tag chips.
- Setup side panel and combat HUD show:
  - current build name,
  - active synergy,
  - most-dependent memory.
- JSON/event payloads include `buildIdentity` and `buildIdentitySeenBy90Sec`.
- AI raw-run payload includes build name, active synergy details, and most-dependent memory.
- Added `?qa=fast,identity` and `npm run qa:identity`.
- Compressed memory descriptions to fit UI cards better.

## 3. Test results and evidence

- `npm run qa:identity`: later passed with `status: complete`, failures `[]`, `buildIdentitySeenBy90Sec: true`.
- `npm run autopilot:preflight`: later passed with 21 pass / 0 warn / 0 fail.
- `npm run ai:test:quick`: remained `GO_CANDIDATE` around the v0.9 loop.
- Browser identity QA verified that build identity appeared within the required early window.

## 4. Decisions made

- WP1 should not be reopened for more gameplay work after identity QA passes.
- Build identity improvement should stay within existing six memories.
- Do not start WP2 until loop-run artifacts, clean preflight, and identity QA are closed.

## 5. Problems or risks

- Several early devloop runs selected gate cleanup rather than new gameplay because dirty loop artifacts blocked preflight.
- Identity readability is a necessary condition, not proof of fun.
- Some weak metrics such as earlyChoiceInterest and echoPivotScore remained observation items.

## 6. GPT handoff summary

- WP1 gave LETHE a clearer build identity surface without adding content.
- Claude/Codex synthesis repeatedly said the next executable work should stay gate-cleanup-only until preflight and identity QA closed.
- After closure, WP2 Slice A pressure rhythm became the next scope.

## 7. Next Codex tasks

- Keep WP1 closed unless a regression appears.
- Start WP2 Slice A pressure rhythm only after clean-tree preflight and identity proof.
- Preserve scope guard for existing memories/slots.

## 8. Portfolio notes

- Problem: players needed to understand their build identity before loss could feel meaningful.
- Direction: make the existing memory set legible instead of adding content.
- Action: added role/tag/synergy UI, identity payloads, and identity QA.
- Result: WP1 became a proven gate for v0.9 release-feel work.

# 2026-06-02-10 - 자동 개발 루프 preflight와 산출물 정합성 정리

## 1. Current build status

- Autonomous loop infrastructure was usable but needed cleanup to avoid self-blocking.
- The project was still in v0.9 gate/automation work before WP2.
- Clean-tree preflight became mandatory before unattended loops.

## 2. What changed today

- Updated autonomous dev loop prompt selection so it targeted the foremost unfinished v0.9 item rather than hard-coded WP1.
- Changed default dev-loop preflight to run before creating loop logs.
- Changed preflight order so the loop does not dirty the tree before checking cleanliness.
- Added diagnosis for dirty `docs/loop_runs/*.md` artifacts.
- Added missing-result diagnosis for wrapper-owned prompt/result pairs.
- Recorded and committed prior loop-run artifacts so preflight could pass.

## 3. Test results and evidence

- `npm run autopilot:preflight:local`: initially failed on dirty loop-run artifacts, then later passed after cleanup.
- Missing result diagnosis identified expected wrapper-owned result files.
- Post-loop gate closure recorded:
  - `npm run autopilot:preflight`: 21 pass / 0 warn / 0 fail,
  - `npm run qa:identity`: `status: complete`, failures `[]`.

## 4. Decisions made

- Preflight failures are blockers, not footnotes.
- Loop logs and prompt/result files must either be committed as records or removed if abandoned.
- Do not let a loop start new gameplay work while wrapper artifacts make the tree dirty.

## 5. Problems or risks

- Long unattended loops can create many artifacts.
- If artifact cleanup is unclear, the loop may repeat gate work instead of implementing gameplay.
- Reports can become noisy if each internal feedback step becomes a top-level report.

## 6. GPT handoff summary

- The loop infrastructure became safer but exposed reporting-size problems.
- GPT/Claude should distinguish AI planning pass, browser proof, and user evidence.
- After preflight/identity closure, WP2 Slice A was allowed.

## 7. Next Codex tasks

- Start v0.9 WP2 Slice A pressure rhythm.
- Keep loop artifacts tracked and clean.
- Keep report units at feature/gate size, not internal step size.

## 8. Portfolio notes

- Problem: automation can fail because of its own logs and stale wrapper outputs.
- Direction: make preflight and artifact diagnosis explicit.
- Action: changed loop preflight order, prompt targeting, dirty-tree diagnostics, and result-file checks.
- Result: unattended loops became safer to restart and less likely to mask blockers.

# 2026-06-02-11 - v0.9 WP2 압박 리듬과 결손 압박 구현

## 1. Current build status

- v0.9 WP2 Slice A pressure rhythm is implemented.
- v0.9 WP2 Slice B minimal post-loss challenge is implemented.
- AI proxy metrics stayed positive, but browser proof for post-loss remained missing.

## 2. What changed today

- Added pressure rhythm phases:
  - `숨 고르기`,
  - `압박 상승`,
  - `망각 전조`.
- Added pressure timeline/payload fields:
  - `runTimeline.pressureSegments`,
  - `danger.pressureLullTime`,
  - `danger.pressureRiseTime`,
  - `danger.pressureClimaxTime`.
- Added AI pressure metrics and `pressureContrast`.
- Added post-loss challenge phases:
  - `결손 정비`,
  - `결손 압박`.
- Added `runTimeline.postLossChallenges` and post-loss danger fields.
- Added `?qa=fast,postloss`, `npm run qa:pressure`, and `npm run qa:postloss`.

## 3. Test results and evidence

- `npm run qa:pressure`: passed earlier with `status: complete`, failures `[]`, segments `lull/rising/climax`.
- `npm run ai:test:quick`: `GO_CANDIDATE`, Alpha Fun Score around `0.8846`, pressure contrast around `0.4416`, post-loss challenge around `0.6687`, post-loss contrast around `0.3134`.
- `npm run ai:test`: `GO_CANDIDATE`, Alpha Fun Score around `0.8879`, post-loss challenge around `0.6692`.
- `npm run doctor`: 43 pass / 0 warn / 0 fail during WP2 Slice B.
- `npm run qa:postloss`: failed before gameplay evaluation at Chrome/CDP transport.

## 4. Decisions made

- WP2 Slice A and Slice B are implementation-complete and scope-valid.
- Treat post-loss browser proof as missing until trusted-local `qa:postloss` or `qa:postloss:trusted` passes.
- Do not start WP3 tactical agency until the post-loss browser gate is resolved.

## 5. Problems or risks

- Post-loss browser QA failed at transport rather than gameplay assertions.
- The managed sandbox later blocked both CDP pipe and remote-debugging-port paths.
- PostLossChallengeScore around `0.669` is a planning signal, not human feel proof.

## 6. GPT handoff summary

- WP2 added pressure rhythm before loss and a minimal challenge after loss.
- The implementation uses existing enemies, memories, and combat parameters.
- GPT/Claude should judge whether the missing browser proof blocks WP3 or can be treated as environment-specific only after trusted-local evidence.

## 7. Next Codex tasks

- Run trusted-local `npm run qa:postloss` or `npm run qa:postloss:trusted`.
- If browser proof passes, record WP2 Slice B as browser-proven.
- If gameplay assertion fails, fix only the post-loss QA/flow issue.
- If transport fails again outside the sandbox, use the environment-blocker prompt before WP3.

## 8. Portfolio notes

- Problem: LETHE needed pressure highs/lows and a meaningful post-loss challenge before tactical agency.
- Direction: strengthen pacing and loss recovery using existing systems.
- Action: implemented pressure phases, post-loss phases, simulator metrics, and QA hooks.
- Result: WP2 was implemented, but its browser evidence gate remained open.

# 2026-06-02-12 - Post-loss 브라우저 게이트와 환경 blocker 정리

## 1. Current build status

- WP2 Slice B remains implementation-complete and scope-valid.
- Latest status is `ITERATE_BEFORE_TEST`.
- Browser proof is still missing because the managed sandbox cannot complete Chrome/CDP post-loss QA.
- WP3 Slice A, people testing, balance changes, and UI/gameplay expansion remain blocked.

## 2. What changed today

- Hardened `scripts/run_browser_pressure_qa.js`:
  - CDP pipe remains the first path,
  - pipe target timeout falls back to remote-debugging-port,
  - fallback uses OS-confirmed `127.0.0.1` port,
  - pipe/port share stable headless Chrome flags,
  - repeated transport failures are reported as `BrowserQaTransportError`.
- Added `docs/review_prompts/2026-06-02-postloss-browser-transport-blocker.md`.
- Added `scripts/run_trusted_postloss_gate.js` and `npm run qa:postloss:trusted`.
- The trusted gate runs standard post-loss QA, retries once with 30000 ms only for transport failures, then points to the blocker prompt.
- Added `alpha_test/outputs/postloss-trusted-gate/latest.json` result logging with `status`, `transportFailure`, run summaries, `nextCommand`, and `blockerPrompt`.

## 3. Test results and evidence

- `node --check scripts/run_browser_pressure_qa.js`: passed.
- `node --check scripts/run_trusted_postloss_gate.js`: passed.
- `npm run doctor`: 44 pass / 0 warn / 0 fail after adding trusted gate.
- `npm run doctor:deep`: 64 pass / 0 warn / 0 fail.
- `npm run qa:postloss`: failed before gameplay evaluation.
- `npm run qa:postloss -- --timeout-ms 30000`: failed before gameplay evaluation.
- `npm run qa:pressure`: failed at the same transport stage during later sandbox runs.
- `npm run qa:postloss:trusted`: failed before gameplay evaluation after standard run plus 30000 ms retry.
- Latest gate JSON recorded `status: blocked`, `transportFailure: true`.
- Failure shape:
  - pipe: `Timed out waiting for CDP response to Target.getTargets`,
  - port fallback: `listen EPERM: operation not permitted 127.0.0.1`.

## 4. Decisions made

- Treat this as a browser automation transport blocker in the managed sandbox, not proof that post-loss gameplay failed.
- Keep WP3 blocked until trusted-local browser proof passes or an explicit environment-blocker decision exists.
- Use `npm run qa:postloss:trusted` as the next single gate command.
- Keep AI proxy metrics as planning evidence only.

## 5. Problems or risks

- Trusted-local behavior outside this sandbox is still unknown.
- Repeated QA tooling fixes can consume loop time without advancing gameplay.
- AI metrics remain positive but cannot replace browser/user proof.
- If the same transport failure repeats outside this sandbox, the project needs a decision rather than more blind QA retries.

## 6. GPT handoff summary

- WP2 Slice B is code-complete and scope-valid.
- The next executable unit is sandbox-outside trusted-local `npm run qa:postloss:trusted`.
- If it passes, record WP2 Slice B as browser-proven before starting only minimal WP3 Slice A.
- If it fails on gameplay assertions, fix only the post-loss flow.
- If it fails on the same transport path, use `docs/review_prompts/2026-06-02-postloss-browser-transport-blocker.md`.

## 7. Next Codex tasks

- Run `npm run qa:postloss:trusted` on a trusted local machine outside this managed sandbox.
- Inspect `alpha_test/outputs/postloss-trusted-gate/latest.json`.
- Update `docs/CODEX_STATUS.md`, `docs/NEXT_TASKS.md`, devlog, and report based on that result.
- Keep WP3, people testing, balance, and UI/gameplay expansion closed until the gate is resolved.

## 8. Portfolio notes

- Problem: WP2 had promising AI proxy metrics but lacked browser proof because automation failed before gameplay evaluation.
- Direction: distinguish gameplay failure from environment transport failure.
- Action: added transport diagnostics, fallback hardening, trusted gate wrapper, blocker prompt, and machine-readable gate result JSON.
- Result: the blocker is now explicit and repeatable; the next decision depends on trusted-local browser evidence.

# 2026-06-02-13 - 보고서/Discord 단위 체계 1차 정리

## 1. Current build status

- The project had many loop logs and report sections after v0.9 automation work.
- Daily reports were readable as source records but too noisy for Discord/latest-section consumption.
- A first report-unit rule was added on 2026-06-02, later refined on 2026-06-03.

## 2. What changed today

- Converted previous top-level work sections into numbered headings.
- Added `scripts/check_report_units.js`.
- Added `npm run report:check`.
- Added report-unit validation to doctor.
- Changed Discord `--latest-section` behavior so it can send a clearly named work-unit section.
- The first rule still allowed too-small loop-step units such as feedback-only task updates.

## 3. Test results and evidence

- `npm run report:check`: validated numbered headings.
- `npm run doctor`: included report heading validation.
- Discord latest-section delivery used the latest top-level report section.
- Later 2026-06-03 work generated per-unit Markdown/HTML files and tightened unit size.

## 4. Decisions made

- Use numbered work-unit titles to make Discord and reports less ambiguous.
- Keep the daily Markdown report as the source document.
- Keep refining report units if they become too small for humans to read.

## 5. Problems or risks

- The 2026-06-02 first pass still produced 57 units, which was too many.
- Internal loop mechanics were overrepresented in the report.
- The 2026-06-03 rule now treats report units as feature/decision units, not commit/log units.

## 6. GPT handoff summary

- The first report-unit system made reports addressable but not yet well-sized.
- Future reports should group implementation, verification, feedback, and next decision together.
- Detailed loop artifacts should stay in `docs/loop_runs/`, not become separate report units.

## 7. Next Codex tasks

- Apply the 2026-06-03 larger report-unit rule to future reports.
- Keep generated unit files synced with `npm run report`.
- Use Discord latest-section for feature/gate/decision units, not internal loop steps.

## 8. Portfolio notes

- Problem: automated loops create lots of small records that are bad as human-facing reports.
- Direction: separate detailed logs from decision-readable report units.
- Action: added heading rules, report checks, and Discord latest-section support.
- Result: reporting became addressable, and later work made the unit size more humane.
