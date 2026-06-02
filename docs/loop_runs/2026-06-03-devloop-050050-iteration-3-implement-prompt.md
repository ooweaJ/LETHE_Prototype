# LETHE Autonomous Implementation Cycle

너는 LETHE 프로젝트의 구현 담당 Codex다. 반드시 한국어로 요약한다.

## 이번 루프 목표

- `docs/NEXT_TASKS.md`의 다음 미완료 v0.9 작업 중 가장 앞선 하나만 구현한다.
- 현재 선택 범위는 아래 `NEXT_TASKS` 발췌에서 가장 앞선 미완료 v0.9 항목이다.
- WP1이 완료된 상태라면 새 기능으로 건너뛰기 전에 preflight cleanup과 trusted-local identity QA 재확인을 먼저 처리한다.
- 새 기억, 새 슬롯, 상점, 메타 진행, 새 지역, 대형 무기 확장은 금지한다.

## 반드시 할 일

1. 현재 코드와 문서를 읽는다.
2. 가장 작은 구현 단위를 선택해 코드 변경을 한다.
3. 가능한 검증을 실행한다.
4. `docs/CODEX_STATUS.md`, `docs/NEXT_TASKS.md`, `docs/devlog/2026-06-03.md`를 갱신한다.
5. 루틴 구현 회차에서는 `docs/reports/2026-06-03.md`에 별도 top-level 보고 단위를 만들지 않는다. 보고서는 피드백과 태스크 갱신까지 합친 뒤 task-update 단계에서 한 번만 만든다.
6. 예외: 독립 기능/결정 단위가 완결되어 사용자가 바로 읽어야 하는 경우에만 `# 2026-06-03-NN - 기능/결정 제목` 보고 단위를 추가한다. `Feedback-N 태스크 갱신`, `자동 개발 루프 N차`, `구현 결과`처럼 작은 절차명은 보고 제목으로 쓰지 않는다.

## 현재 상태 발췌

### CODEX_STATUS

```text
# Codex Status

Last updated: 2026-06-03

## Current Build

- Project: LETHE HTML Alpha v0.9 WP3 Slice A minimal tactical agency implemented; browser automation proof is still blocked by the managed-sandbox Chrome transport.
- Repository: `https://github.com/ooweaJ/LETHE_Prototype.git`
- Branch: `main`
- Current scope: HTML prototype validation. Broad human testing is paused. v0.8 AI gates passed, but the user judged that the prototype still needs a stronger release-like roguelike fun loop before people testing. v0.9 now prioritizes reference-driven build identity, pressure, post-loss challenge, and overnight automation.
- Latest task-update status: v0.9 WP3 Slice A is code-complete as a minimal tactical agency hook, and the `2026-06-03-devloop-050050-feedback-2` Claude/Codex synthesis recorded `ITERATE_BEFORE_TEST`. The player can focus one current active memory from the existing slot UI or `Digit1`-`Digit3`; this briefly accelerates that memory's next impact and records `tacticalFocus` in JSON/QA payloads. `npm run qa:tactical` still is not browser-proven in this managed sandbox because Chrome CDP pipe times out at `Target.getTargets` and the port fallback cannot bind `127.0.0.1` (`EPERM`). This loop added `npm run qa:tactical:trusted`, which runs standard tactical QA, retries once at 30000 ms on transport failure, and writes `alpha_test/outputs/tactical-trusted-gate/latest.json`; the current managed-sandbox result is `status: blocked`, `transportFailure: true`. `npm run ai:test:quick` remains `GO_CANDIDATE` with Alpha Fun Score `0.8846`. Claude proposed a one-line echo-pivot hint to improve `echoPivotScore`, while Codex CLI limited the next action to browser proof. The selected scope stays strict: run sandbox-outside trusted-local `npm run qa:tactical:trusted` or record an explicit environment-blocker decision; do not add hint text, balance changes, new UI/gameplay scope, or people testing before that proof/decision.

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
  
... [trimmed]
```

## NEXT_TASKS 발췌

### NEXT_TASKS

```text
# Next Tasks

현재 단계는 v0.8 Gate C 직행에서 v0.9 release-feel loop 준비로 바뀌었다. 사용자는 지금 브라우저 QA만 보강해 사람 테스트로 넘겨도 재미가 부족할 가능성이 높다고 판단했다. 다음 목표는 장르 레퍼런스를 조사해 LETHE에 맞게 번역하고, 목표-검증-피드백-다음 입력이 밤에도 계속 도는 자동 루프를 만드는 것이다.

이 프로젝트의 현재 목표는 HTML 프로토타입으로 LETHE의 핵심 재미와 가능성을 검증하는 것이다. 충분히 재미가 확인되면 그 결과를 근거로 Unity 구현 단계로 넘어간다.

## Current Verdict

- GPT verdict: `ITERATE_BEFORE_TEST`.
- Claude v0.5 evaluation: `GO_TO_HUMAN_TEST` after Chrome headless QA confirmed the level-up flow and `runGrowth` payload.
- Codex implementation result: `GO_CANDIDATE` from `npm run ai:test` and `npm run ai:test:heavy`.
- User direct playtest verdict: pause broad human testing and redesign the core run structure before more testing.
- New planning prompt: `docs/review_prompts/2026-06-02-run-structure-redesign.md`.
- v0.6 implementation verdict: `GO_TO_SOLO_PLAYTEST_CANDIDATE` by local Codex evidence.
- v0.7 implementation verdict: `GO_TO_SOLO_PLAYTEST_CANDIDATE` by local Codex evidence after weapon baseline and lost-memory weapon echo pass.
- Claude v0.7 balance verdict: `GO_TO_SOLO_PLAYTEST`, but this was based on AI proxy metrics rather than live balance play.
- User v0.7 direct feedback invalidated the balance verdict: balance is not close enough and the automatic balance proxy must be fixed before trusting the loop.
- v0.8 double-check planning is now the default for major design changes: Claude plus Codex CLI.
- v0.8 direction: start with Gate A (`death bug + real danger metrics`), then continue into short-run/memory-budget/synergy/tag-echo redesign.
- Actual automation proof for this turn: Discord work-unit report sent successfully, Claude v0.7 prompt sent successfully, Claude response saved successfully.
- Project direction: HTML prototype validation first, Unity implementation later only if AI/human tests show enough promise.
- v0.9 direction: broad human testing remains paused until the HTML prototype has stronger release-like build identity, pressure rhythm, post-loss challenge, and visible tactical agency.
- Latest devloop feedback verdict: the `2026-06-02-devloop-175642` automation prompt cleanup is valid, WP1 should not be reopened for new gameplay work, and the next executable scope remains gate cleanup: record/track current loop-run outputs and rerun trusted-local `npm run qa:identity` before WP2 or unattended automation.
- Latest devloop feedback-2 verdict: `ITERATE_BEFORE_TEST`. The preflight-order cleanup is code-complete, but WP1 is not officially closed until the current loop-run outputs are recorded/cleaned, `npm run autopilot:preflight:local` passes on a clean tree, and trusted-local `npm run qa:identity` passes.
- Latest devloop feedback-3 verdict: AI planning evidence supports `GO_CANDIDATE` / Claude `GO_TO_HUMAN_TEST`, but the selected Codex scope remains gate cleanup only. Do not start WP2 or human-test checklist work until loop-run artifacts are recorded/cleaned, clean-tree `npm run autopilot:preflight:local` passes, and trusted-local `npm run qa:identity` passes.
- Latest devloop feedback-4 verdict: `GO_CANDIDATE` remains an AI planning pass, not human emotion or balance proof. The missing-result diagnosis worked and the wrapper result now exists, so the next executable scope is still artifact 정합성 정리: record/track or remove the `docs/loop_runs/2026-06-02-devloop-175642*` outputs, then pass clean-tree `npm run autopilot:preflight:local` and trusted-local `npm run qa:identity` before WP2.
- Post-loop gate closure: `2026-06-02-devloop-175642*` outputs are committed, `npm run autopilot:preflight` passed with 21 pass / 0 warn / 0 fail, and `npm run qa:identity` passed with `status: complete`, failures `[]`. Next executable scope is v0.9 Work Package 2 Slice A.
- Latest devloop feedback-193946 verdict: `ITERATE_BEFORE_TEST`. WP2 Slice B is implementation-complete and scope-valid, but not browser-proven because local Chrome/CDP `qa:postloss` failed at `Target.getTargets`. Before WP3 or people testing, rerun trusted-local `npm run qa:postloss`; if 
... [trimmed]
```

## v0.9 더블 체크 요약

### DOUBLE_CHECK

```text
# Double Check Summary - 2026-06-02-v09-release-feel-loop

## Prompt

- docs/review_prompts/2026-06-02-v09-release-feel-loop.md

## Responses

- Claude: docs/review_responses/2026-06-02-v09-release-feel-loop-claude.md
- Codex CLI: docs/review_responses/2026-06-02-v09-release-feel-loop-codex.md

## Decision Rules

- User live play feedback outranks AI planning opinions.
- Browser combat evidence outranks aggregate AI proxy metrics.
- If Claude and Codex disagree, Codex should summarize the conflict before implementation.
- Large design changes require both responses to be read before editing game code.
- A `GO` from either AI is only `AI planning pass` until browser combat QA or user play validates it.

## Codex Synthesis Required

- [x] Common recommendations:
  - Verdict is `BUILD_V09_RELEASE_FEEL_LOOP`.
  - Do not proceed directly to v0.8 Gate C or broad human testing.
  - v0.9 must make the player understand their build identity within roughly 90 seconds.
  - Existing 6 memories and 3 active slots are enough; improve role labels, tags, synergies, and combat feedback before adding content.
  - The first implementation unit should be build identity/readability, then post-loss challenge, then Echo Zone or another small tactical agency layer.
  - AI verdicts remain planning/verification signals, not human-fun approval.
- [x] Conflicts:
  - Claude proposes more immediate feel changes in the same batch: post-loss breathing window, deficit passive, boss tell, and Echo Zone recovery pulse.
  - Codex CLI recommends a stricter sequence: build identity first, then post-loss challenge, then Echo Zone, with new metrics such as `buildIdentitySeen`, `synergyActiveTime`, `lossChallengeCompleted`, and `echoZoneUseCount`.
  - Selected resolution: keep Claude's feel ideas as candidates, but implement Codex CLI's order so the first v0.9 loop does not sprawl.
- [x] Selected vNext scope:
  - v0.9 Work Package 1 only: existing 6-memory build identity pass.
  - Add/clarify each memory's role, tag, and short combat description.
  - Show current build name, active synergy, and most-dependent memory in the selection/HUD flow.
  - Add JSON/AI payload hooks for build identity visibility before implementing Echo Zone or post-loss challenge.
- [x] Tests required before reporting balance:
  - `npm run ai:test:quick`.
  - `npm run ai:test`.
  - A browser/headless QA path that confirms 90-second build identity and synergy display.
  - JSON payload must include v0.9 identity fields befor
... [trimmed]
```

## 출력 형식

- 구현한 것
- 검증한 것
- 남은 위험
- 다음 루프 추천 작업
