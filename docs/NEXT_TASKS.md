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
- Latest devloop feedback-193946 verdict: `ITERATE_BEFORE_TEST`. WP2 Slice B is implementation-complete and scope-valid, but not browser-proven because local Chrome/CDP `qa:postloss` failed at `Target.getTargets`. Before WP3 or people testing, rerun trusted-local `npm run qa:postloss`; if it passes, proceed only to a minimal WP3 Slice A tactical agency hook using the current active memories and current combat loop.
- Latest devloop feedback-193946-feedback-2 verdict: `ITERATE_BEFORE_TEST`. The post-loss QA runner fallback is valid tooling and does not widen gameplay scope, but it still did not produce browser proof in the managed sandbox. Claude and Codex agree the next executable unit remains trusted-local `npm run qa:postloss` only; WP3 Slice A and people testing stay blocked until that proof or a documented environment decision exists.
- Latest post-loss transport update: this loop reran `npm run qa:postloss`, retried `npm run qa:postloss -- --timeout-ms 30000`, and cross-checked `npm run qa:pressure`; all still failed before gameplay evaluation through the same Chrome transport channel. `scripts/run_browser_pressure_qa.js` now emits an explicit `BrowserQaTransportError`, and an environment-blocker decision prompt exists at `docs/review_prompts/2026-06-02-postloss-browser-transport-blocker.md`.
- Latest devloop feedback-193946-feedback-3 verdict: `ITERATE_BEFORE_TEST`. Claude and Codex agree the implementation and diagnostic work are scope-valid, the AI proxy remains a planning pass only, and the next executable unit is still trusted-local `npm run qa:postloss`. There is no material scope conflict in this round; if the same transport failure repeats outside the sandbox, use `docs/review_prompts/2026-06-02-postloss-browser-transport-blocker.md` before starting WP3 or people testing.
- Latest post-loss QA hardening: this loop kept the same foremost v0.9 gate, added a more deterministic remote-debugging-port fallback by using an OS-confirmed free localhost port and shared headless Chrome sandbox flags, then reran `npm run qa:postloss` and `npm run qa:postloss -- --timeout-ms 30000`. Both still failed before gameplay evaluation. The pipe path timed out at `Target.getTargets`; the port fallback now shows the managed sandbox cannot bind `127.0.0.1` (`listen EPERM`). WP3 remains blocked.
- Latest devloop feedback-193946-feedback-4 verdict: `ITERATE_BEFORE_TEST`. Claude and Codex agree the port fallback hardening is scope-valid QA/gate work and the AI proxy remains positive planning evidence only. There is no material next-scope conflict: WP2 Slice B is implementation-complete but not browser-proven, so the next executable unit remains sandbox 밖 trusted-local `npm run qa:postloss`; if the same transport failure repeats, retry once with `--timeout-ms 30000`, then use `docs/review_prompts/2026-06-02-postloss-browser-transport-blocker.md` before WP3 or people testing.
- Latest devloop feedback-193946-feedback-5 verdict: `ITERATE_BEFORE_TEST`. Claude and Codex agree the trusted post-loss wrapper is scope-valid gate tooling and that AI proxy metrics remain a positive planning pass only. There is no material scope conflict: the next executable unit is sandbox 밖 trusted-local `npm run qa:postloss:trusted`; WP3 Slice A, people testing, and any balance/UI/gameplay expansion remain blocked until browser proof passes or `docs/review_prompts/2026-06-02-postloss-browser-transport-blocker.md` produces an explicit environment decision.
- Reporting rule update: work reports now use numbered unit headings like `# 2026-06-02-44 - 보고서 단위 번호 체계`; `npm run report:check` and `doctor` enforce this so Discord latest-section reports are task-readable.
- Reference research: `docs/research/2026-06-02-roguelike-reference.md`.
- New v0.9 prompt: `docs/review_prompts/2026-06-02-v09-release-feel-loop.md`.
- Overnight loop command:
  - `npm run overnight:loop:dry`,
  - `npm run overnight:loop`,
  - `node scripts/run_overnight_loop.js --iterations 3 --sleep-minutes 20`.

## v0.2 Done

- [x] 첫 망각 타이밍을 평균 8-10분 범위로 보정했다.
- [x] 브라우저 빌드의 실제 보스/망각 타이밍을 9분 기준으로 맞췄다.
- [x] `처형자의 섬광` 삭제율을 25-35% 범위로 낮췄다.
- [x] 단일 기억 삭제 쏠림을 AI 기준 허용 범위로 낮췄다.
- [x] JSON 로그에 선택 기억, 예측 기억, 실제 삭제 기억, 기억별 삭제 weight를 추가했다.
- [x] 기본 실험값을 `echo=0.50`, `ui=0.62`로 바꿨다.
- [x] 현재 echo/ui 값을 로그와 AI 결과에 남긴다.
- [x] 망각 결과 화면에 사라진 기억, 예측 결과, 삭제 weight, 잔향 효과, 다음 방향을 표시한다.
- [x] 사람 테스트용 Q3 자유응답 문항을 추가했다.
- [x] 브라우저 QA fast 모드로 결과 화면과 JSON payload를 검증했다.

## v0.3 Started

- [x] 기억 발동명을 전투 중 플로팅 텍스트로 표시한다.
- [x] 타격 숫자, 스파크, 투사체 trail, 보스 등장/페이즈 impact를 추가했다.
- [x] 기억 슬롯에 `레테의 시선` 태그와 의존도 퍼센트를 표시한다.
- [x] 브라우저 `?qa=fast`에서 v0.3 질문/결과/JSON payload 회귀 QA를 통과했다.
- [x] Codex CLI 기획 판단 fallback 명령을 추가했다.

## v0.4 Done

- [x] 브라우저 표기를 v0.4로 올렸다.
- [x] 결과 화면에서 `사라진 행동`과 `잔향 변형`을 분리해 보여준다.
- [x] JSON payload에 `echoTransformation`을 추가했다.
- [x] 기본 AI/UI 선명도 값을 `ui=0.78`로 올렸다.
- [x] 브라우저 `?qa=fast`에서 v0.4 결과 화면과 JSON payload를 검증했다.
- [x] `npm run ai:test:heavy` 5000런/3스테이지에서도 `GO_CANDIDATE`를 확인했다.
- [x] Claude 자동 리뷰 스크립트의 dry-run과 로컬 mock 저장 검증을 통과했다.

## v0.5 Done

- [x] 사용자 피드백을 반영해 기획 프롬프트를 초반 재미/로그라이크 맛 중심으로 갱신했다.
- [x] 실제 Claude 호출은 외부 전송 정책으로 차단되어 Codex가 같은 프롬프트 기준으로 구현을 진행했다.
- [x] 초반 적 스폰 밀도를 올려 1분 안에 몰려오는 압박을 강화했다.
- [x] 처치 XP와 런 중 레벨업 3지선다 스탯 선택을 추가했다.
- [x] 런 성장은 메타 진행/상점/새 기억 추가 없이 해당 런 안에서만 유지되게 했다.
- [x] JSON payload에 `runGrowth`를 추가했다.
- [x] AI test에 `Early Fun Score`, `earlyKillTempo`, `earlyLevelUps` 게이트를 추가했다.
- [x] `npm run ai:test`: `GO_CANDIDATE`, Alpha Fun Score `0.8531`, Early Fun `0.8669`.
- [x] `npm run ai:test:heavy`: `GO_CANDIDATE`, Alpha Fun Score `0.8509`, Early Fun `0.8672`.
- [x] Claude v0.5 구현 후 평가를 저장했다: `docs/review_responses/2026-06-02-claude-v05-eval.md`.
- [x] `?qa=fast,levelup` Chrome headless QA를 추가하고 통과했다.

## Latest AI Criteria

- [x] Verdict: `GO_CANDIDATE`.
- [ ] Alpha Fun Score: `0.89+`. 현재 `0.8531`; 사람 테스트 전 기준으로는 충분하지만 목표치에는 아직 못 미친다.
- [x] Early Fun Score: 목표 `0.72+`, 현재 `0.8669`.
- [x] Early kill tempo: 목표 `0.68+`, 현재 `0.9620`.
- [x] Pre-boss level-ups: 목표 `2+`, 현재 `4.08`.
- [x] 첫 망각 시간: `9.00 min`.
- [ ] Regret proxy: 목표 `85%+`, 현재 `81.6%`; Claude 기준 사람 테스트 전에는 충분하며 사람 반응으로 확인한다.
- [x] Irritation proxy: 목표 `3%` 이하, 현재 `0.3%`.
- [x] Restart intent: 목표 `65%+`, 현재 `76.1%`.
- [ ] Post-forgetting power drop: 목표 `30-40%`, 현재 `28.0%`; Claude 기준 지금 보정하지 말고 사람 테스트에서 체감 확인한다.
- [x] Recovery after replacement: 목표 `90%+`, 현재 `97.5%`.
- [x] Prediction match: 목표 `75-90%`, 현재 `84.8%`.
- [x] Max single memory deletion share: 목표 `35%` 이하, 현재 `28.8%`.

## Next Codex Tasks

- [x] 브라우저에서 v0.2 화면 QA를 한다.
- [x] 결과 화면에서 텍스트가 겹치지 않는지 확인한다.
- [x] JSON 다운로드에 새 필드가 들어가는지 실제 브라우저에서 확인한다.
- [x] 5-8명용 human playtest 가이드를 작성한다.
- [x] 사람 테스트 전 전투 연출을 더 화려하게 만든다.
- [x] 사람 테스트 전 기억 의존도/망각 위험이 플레이 중 더 분명하게 보이게 만든다.
- [x] 사람 테스트 전 잔향 시스템이 결과 이후 더 분명하게 보이게 만든다.
- [x] v0.5 브라우저 QA를 Chrome headless로 확인했다. Browser 플러그인은 Windows sandbox 오류로 열리지 않았지만, 설치된 Chrome으로 실제 HTML/JS를 실행했다.
- [x] 레벨업 3택 화면이 실제 브라우저 DOM에 뜨고, 선택 후 전투가 정상 재개되는지 확인했다.
- [x] JSON payload에 `runGrowth` 선택 내역이 실제 선택과 일치하는지 확인했다.
- [x] 레벨업 일시정지/재개 흐름이 QA 모드에서 정상 완료되는지 확인했다.
- [x] 테스트 결과 기반 Claude/Codex 기획 파이프라인을 추가한다.
- [x] `npm run planning:pipeline:prompt`로 quick AI test와 `docs/review_prompts/2026-06-02-pipeline.md` 생성을 확인했다.
- [x] `npm run planning:pipeline`으로 Claude의 v0.5 이후 판단을 받았다: `GO_TO_HUMAN_TEST`.
- [x] 사람 테스트용 테스터 ID/세션 번호를 JSON 로그에 남긴다.
- [x] v0.5 사람 테스트 가이드와 관찰 기록 템플릿을 갱신한다.
- [x] AI 협업 포트폴리오 문서와 ADR을 추가한다.
- [x] 다른 로컬에서 파이프라인 준비 상태를 확인하는 `npm run doctor` 라인을 추가한다.
- [x] 사람 테스트 JSON 로그를 요약하고 Claude/GPT용 human-test 프롬프트를 생성하는 `npm run playtest:summary` 라인을 추가한다.
- [x] 사람 테스트용 정적 배포 폴더를 만드는 `npm run playtest:package` 라인을 추가한다.
- [x] 실제 1인 플레이 피드백을 바탕으로 v0.5 사람 테스트를 보류하고 런 구조 재설계 프롬프트를 작성한다.
- [x] 자동 루프 시작 전 인증/권한/알림/fallback 상태를 확인하는 `npm run autopilot:preflight` 규약을 추가한다.
- [ ] `docs/review_prompts/2026-06-02-run-structure-redesign.md`를 Claude/GPT에 보내 v0.6 방향을 결정한다.
- [x] GPT/Claude 공통 답변을 읽고 v0.6 Codex 작업 목록으로 변환한다.
- [x] v0.6에서 첫 보스/첫 망각 타이밍, 보스 주기, 기억 상실 후 치환 보상, 기억 보충 타이밍을 구현한다.
- [x] v0.6 구현 후 AI/브라우저 QA를 다시 실행한다.
- [x] v0.6 결과 보고 프롬프트를 작성한다: `docs/review_prompts/2026-06-02-v06-cycle-eval.md`.
- [x] v0.6 1인 피드백을 바탕으로 v0.7 무기 바닥 성능과 잃은 기억의 무기 잔향 시너지를 보강한다.
- [x] v0.7 AI/브라우저 QA를 실행하고 자동 밸런스 결과를 기록한다.
- [x] 실제 Discord work-unit report를 전송했다: `npm run report:discord:unit`.
- [x] 실제 Claude v0.7 balance feedback을 받았다: `docs/review_responses/2026-06-02-v07-balance-claude.md`.
- [x] Claude v0.7 verdict를 다음 gate로 반영한다: `GO_TO_SOLO_PLAYTEST`.
- [x] v0.7 1인 체감 테스트 시트를 만든다: `docs/playtest/2026-06-02-solo.md`.
- [x] v0.7 사용자 직접 피드백을 받았다: 밸런스가 맞지 않는다.
- [x] v0.7 자동 밸런스 판정을 실패 사례로 기록한다.
- [x] v0.7.1 밸런스 현실 검증 프롬프트를 작성한다: `docs/review_prompts/2026-06-02-v071-balance-reality-check.md`.
- [x] 큰 기획 변경은 Claude + Codex CLI 더블 체크를 고정 파이프라인으로 바꾼다.
- [x] v0.8 전투/기억 코어 재기획 프롬프트를 작성한다: `docs/review_prompts/2026-06-02-v08-core-redesign.md`.
- [x] Claude v0.8 답변을 받았다: `docs/review_responses/2026-06-02-v08-core-redesign-claude.md`.
- [x] Codex CLI v0.8 답변을 받았다: `docs/review_responses/2026-06-02-v08-core-redesign-codex.md`.
- [x] v0.8 더블 체크 요약을 작성했다: `docs/review_responses/2026-06-02-v08-core-redesign-double-check.md`.
- [x] v0.8 Gate A를 시작했다: HP 1 고정 제거, 실제 사망 처리, death/danger 로그 추가.
- [x] v0.8 death QA를 추가했다: `?qa=fast,death`.
- [x] v0.8 Gate A 다음 작업: AI proxy와 실제 브라우저 전투 체감의 차이를 줄이는 테스트/지표를 보강한다.
- [x] v0.8 Gate B: 8-10분 런, 90초 미니보스/첫 망각, 기억 예산 평준화, 최소 시너지, 태그 기반 잔향을 구현한다.
- [x] v0.8 Gate B 자동 루프를 2회 이상 돌려 실패 게이트를 다음 수정 입력으로 사용했다.
- [x] `npm run ai:test:quick`, `npm run ai:test`, `npm run ai:test:heavy`가 모두 `GO_CANDIDATE`로 통과했다.
- [x] v0.9 장르 레퍼런스 조사 문서를 작성한다.
- [x] v0.9 release-feel loop 기획 프롬프트를 작성한다.
- [x] 밤샘 루프 실행 스크립트를 추가한다.
- [x] `doctor`와 `doctor:deep`이 밤샘 루프 명령을 확인하게 한다.
- [x] `npm run overnight:loop:dry`와 `npm run doctor:deep`을 검증한다.
- [x] 외부 AI 호출 없는 safe smoke loop를 실행해 `docs/loop_runs/2026-06-02-overnight-163600.md`를 남긴다.
- [x] v0.9 프롬프트를 Claude + Codex CLI 더블 체크에 보내 release-feel 작업 단위를 확정한다.
- [x] v0.9 프롬프트를 Claude + Codex CLI 더블 체크에 보내 release-feel 작업 단위를 확정한다.
- [x] v0.9 더블 체크 요약에 공통점/충돌점/선택 범위를 정리한다.
- [x] 밤샘 루프 Discord 진행 알림을 기본값으로 고정한다.
- [x] 실제 구현까지 반복하는 autonomous dev loop를 추가한다.
- [x] `doctor`와 `doctor:deep`이 autonomous dev loop dry-run을 확인하게 한다.
- [x] v0.9 Work Package 1: 기존 6개 기억 안에서 빌드 정체성과 시너지 체감을 강화한다.
  - [x] 기억 선택 카드와 슬롯에서 기억별 주 역할, 태그, 짧은 전투 설명을 함께 보이게 했다.
  - [x] 현재 빌드 이름, 활성 시너지, 의존 중인 기억을 선택 화면/HUD에 표시한다.
  - [x] JSON payload, 이벤트 로그, AI raw-run payload에 build identity 필드를 추가한다.
  - [x] Claude/Codex 피드백을 받아 WP1 다음 작업을 identity QA runner로 확정한다.
  - [x] `npm run qa:identity` 전용 Chrome/CDP identity QA runner를 추가했다.
  - [x] `?qa=fast,identity`를 실제 브라우저 또는 안정적인 headless runner에서 검증한다.
    - `npm run qa:identity`: `status: complete`, failures `[]`.
  - [x] 기억별 설명 문구를 더 압축해 90초 안에 더 쉽게 읽히게 했다.
    - 6개 기억 선택/보충 카드 요약은 `역할 · 짧은 설명` 형식으로 통일했다.
    - 요약 문구는 20-26자 범위로 줄였다.
    - `npm run ai:test:quick`: `GO_CANDIDATE`, Alpha Fun Score `0.8883`.
    - `npm run ai:test`: `GO_CANDIDATE`, Alpha Fun Score `0.8909`.
    - 이번 Codex 세션의 `npm run qa:identity` 재실행은 Chrome CDP pipe가 `Target.getTargets` 응답을 받지 못해 실패했다. 이전 통과 기록은 유지하되, WP2 착수 전 trusted local 재확인이 안전하다.
- [x] Claude/Codex 피드백 공통점과 충돌을 정리했다.
  - 공통점: WP1 문구 압축은 AI 지표를 해치지 않았고, 새 콘텐츠 추가 없이 다음 검증 축으로 넘어갈 수 있다.
  - 공통점: 미추적 loop-run 파일 때문에 preflight가 막혀 있으며, unattended loop 전에 정리해야 한다.
  - 공통점: trusted local에서 `npm run qa:identity`를 재확인해야 한다.
  - 충돌: Claude는 WP2 압박 고저차를 먼저 검증하고 post-loss challenge를 보류하자고 했고, Codex CLI는 최소 post-loss challenge를 다음 구현으로 제안했다.
  - 선택: preflight 정리와 identity QA를 먼저 처리한 뒤, WP2는 압박 고저차를 첫 slice로 시작한다. post-loss challenge는 WP2 안의 최소 후속 작업으로만 유지한다.
- [x] 자동 개발 루프 구현 프롬프트가 완료된 WP1에 계속 고정되지 않도록 `scripts/run_autonomous_dev_loop.js`의 WP1 하드코딩을 제거했다.
- [x] 기존 `docs/loop_runs/2026-06-02-devloop-173350*.md` 파일은 현재 git 추적 상태임을 확인했다.
- [x] `2026-06-02-devloop-175642` Claude/Codex 피드백 공통점과 충돌을 정리했다.
  - 공통점: WP1은 새 게임플레이 작업으로 재오픈하지 않는다. 이번 구현은 완료된 WP1을 반복 선택하지 않게 하는 자동화 프롬프트 정리로 적절하다.
  - 공통점: AI quick 결과는 `GO_CANDIDATE`, Alpha Fun Score `0.8883`, low irritation, hard fail 없음으로 planning 기준은 안정적이다.
  - 공통점: 현재 `docs/loop_runs/2026-06-02-devloop-175642*` 산출물 정리와 trusted-local `npm run qa:identity` 재확인이 다음 blocker다.
  - 충돌: Claude는 `qa:identity` 통과 후 사람 테스트 체크리스트를 만들고 `GO_TO_HUMAN_TEST`로 가자고 했고, Codex CLI는 WP2 진입 게이트 정리 후 WP2 Slice A 압박 리듬을 먼저 권장했다.
  - 선택: 이번 사이클은 docs-only update로 닫는다. 다음 실행 작업은 loop-run 산출물 기록/정리와 trusted-local identity QA이며, 그 뒤에는 사용자가 바꾸지 않는 한 기존 순서대로 WP2 Slice A 압박 고저차를 시작한다.
- [x] autonomous dev loop가 자기 로그를 만든 뒤 dirty preflight를 `--allow-dirty`로 통과시키는 구조를 정리했다.
  - `scripts/run_autonomous_dev_loop.js` 기본 preflight를 `npm run autopilot:preflight:local`로 바꿨다.
  - dev loop가 Markdown 로그를 만들기 전에 preflight를 먼저 실행하게 했다.
  - preflight가 통과하면 그 stdout/stderr를 생성된 loop log header에 기록한다.
  - `--allow-dirty`를 명시한 dry-run에서만 dirty 허용 preflight 명령으로 바뀌게 했다.
  - 검증: `node --check scripts/run_autonomous_dev_loop.js`, `npm run dev:loop:dry`, `node scripts/run_autonomous_dev_loop.js --dry-run --allow-dirty --no-commit --no-push --discord-dry-run`, `npm run doctor`, `npm run ai:test:quick`, `git diff --check` 통과.
  - 현재 `npm run autopilot:preflight:local`은 기존 dirty loop-run 산출물과 이번 코드 변경 때문에 실패하는 것이 정상이며, unattended loop 전에는 아래 blocker를 계속 처리해야 한다.
- [x] `2026-06-02-devloop-175642-feedback-2` Claude/Codex 피드백 공통점과 충돌을 정리했다.
  - 공통점: preflight-order cleanup은 방향이 맞고 dry-run/doctor/quick AI evidence가 안정적이지만, dirty tree 때문에 WP1 공식 완료와 unattended loop 재개는 아직 보류한다.
  - 공통점: 다음 실행 작업은 `docs/loop_runs/2026-06-02-devloop-175642*.md` 산출물 기록/정리, clean-tree `npm run autopilot:preflight:local`, trusted-local `npm run qa:identity` 재확인이다.
  - 공통점: quick AI test의 `GO_CANDIDATE`, Alpha Fun Score `0.8883`, irritation `0.0104`는 planning pass일 뿐 실제 사람 감정이나 밸런스 판정이 아니다.
  - 공통점: echoPivotScore `0.656`과 earlyChoiceInterest `0.654`는 사람 테스트 또는 WP2 관찰에서 계속 봐야 한다.
  - 충돌: Claude는 다음 1개 작업을 WP1 gate cleanup으로 강하게 제한하고 WP2 착수를 금지했다. Codex CLI도 gate cleanup을 먼저 보되, 통과 후 기존 WP2 Slice A 압박 리듬으로 복귀하는 순서를 명시했다.
  - 선택: 이번 cycle은 docs-only update로 닫는다. 다음 executable scope는 WP1 마무리 gate cleanup 하나이며, 통과 후에만 WP2 Slice A를 시작한다.
- [x] `npm run autopilot:preflight:local`이 loop-run 산출물 때문에 실패할 때 정확한 blocker 조치 문구를 출력하게 했다.
  - `scripts/autopilot_preflight.js`가 dirty 파일 목록을 5개까지 요약하고, `docs/loop_runs/*.md`가 포함되면 loop-run artifact 전용 안내를 출력한다.
  - 출력 안내는 wrapper result 파일 마무리, `git add docs/loop_runs && git commit -m "docs: 자동 개발 루프 산출물 기록"` 또는 abandoned artifact 제거, 그리고 `npm run autopilot:preflight:local` 재실행이다.
  - 검증: `node --check scripts/autopilot_preflight.js` 통과, `npm run doctor` 39 pass, `npm run autopilot:preflight:local`은 현재 dirty tree 때문에 예상대로 실패하면서 새 안내 문구를 출력했다.
- [x] `npm run autopilot:preflight:local`이 dirty loop-run prompt의 누락된 result 파일을 직접 표시하게 했다.
  - dirty `*-prompt.md` 파일에서 예상 `*-result.md` 경로를 계산한다.
  - 현재 확인된 누락 파일은 `docs/loop_runs/2026-06-02-devloop-175642-iteration-4-implement-result.md`다.
  - 검증: `node --check scripts/autopilot_preflight.js` 통과, `npm run autopilot:preflight:local`은 dirty tree 때문에 예상대로 실패하면서 누락 result 경로를 출력했다.
- [x] `2026-06-02-devloop-175642-feedback-3` Claude/Codex 피드백 공통점과 충돌을 정리했다.
  - 공통점: AI proxy는 `GO_CANDIDATE`, Alpha Fun Score `0.8883`, regret `0.8083`, irritation `0.0104`, restart `0.90`로 planning 기준은 긍정적이다.
  - 공통점: 이번 구현은 게임 기능이 아니라 preflight blocker 진단 보강이며, WP1을 새 gameplay 작업으로 재오픈하지 않는다.
  - 공통점: 현재 blocker는 `docs/loop_runs/2026-06-02-devloop-175642*` 산출물 기록/정리, clean-tree `npm run autopilot:preflight:local`, trusted-local `npm run qa:identity`다.
  - 공통점: echoPivotScore `0.656`과 earlyChoiceInterest `0.654`는 AI 수치만으로 수정하지 않고 사람 관찰 또는 WP2 검증에서 본다.
  - 충돌: Claude는 gate cleanup 이후 사람 테스트 체크리스트와 `GO_TO_HUMAN_TEST`를 제안했고, Codex CLI는 cleanup 이후 기존 WP2 Slice A 압박 리듬 순서를 유지하자고 했다.
  - 선택: 이번 cycle은 docs-only update로 닫는다. 다음 executable scope는 gate cleanup 하나이며, 통과 전에는 WP2, human-test checklist, UI/튜토리얼/밸런스 변경을 시작하지 않는다.
- [x] `2026-06-02-devloop-175642-feedback-4` Claude/Codex 피드백 공통점과 충돌을 정리했다.
  - 공통점: AI proxy는 `GO_CANDIDATE`, Alpha Fun Score `0.8883`, regret `0.8083`, irritation `0.0104`, restart `0.90`로 planning 기준은 긍정적이지만 사람 감정/밸런스 증거는 아니다.
  - 공통점: 이번 구현은 missing-result preflight diagnosis이며, 새 gameplay scope를 늘리지 않는다.
  - 공통점: wrapper result 파일은 현재 생성된 상태이므로 다음 blocker는 `docs/loop_runs/2026-06-02-devloop-175642*` 산출물 기록/정리, clean-tree `npm run autopilot:preflight:local`, trusted-local `npm run qa:identity`다.
  - 공통점: `멈춘 초침` 삭제율, earlyChoiceInterest `0.654`, echoPivotScore `0.656`은 AI 수치만으로 튜닝하지 않고 관찰 대상으로 둔다.
  - 충돌: Claude는 사람 테스트 직전 단계로 보고 outlier 관찰을 강조했고, Codex CLI는 아직 Unity 전환 근거가 아니며 다음 최소 작업을 loop-run artifact 정합성 정리로 제한했다.
  - 선택: 이번 cycle은 docs-only update로 닫는다. 다음 executable scope는 artifact 정합성 정리 하나이며, 통과 후에만 기존 순서대로 WP2 Slice A 압박 고저차를 시작한다.
- [x] 현재 루프가 생성한 `docs/loop_runs/2026-06-02-devloop-175642*.md` 파일을 기록/커밋하거나, 다음 unattended loop 전에 abandoned artifact를 정리해 `npm run autopilot:preflight:local` blocker를 제거한다.
  - `f6ee83f feat: 자동 개발 루프 4차 반영`까지 pushed, working tree clean.
- [x] clean tree에서 `npm run autopilot:preflight:local`을 재실행하고 pass를 기록한다.
  - 후속 full check: `npm run autopilot:preflight` passed, 21 pass / 0 warn / 0 fail.
- [x] trusted local에서 `npm run qa:identity`를 재실행하고 `status: complete`, failures `[]`를 확인한다.
  - `npm run qa:identity`: `status: complete`, failures `[]`, `buildIdentitySeenBy90Sec: true`.
- [x] 보고서를 작업 단위별로 읽히게 하기 위해 numbered work-unit heading 규칙을 추가한다.
  - 기존 `docs/reports/2026-06-02.md` top-level 작업 섹션을 `2026-06-02-01`부터 `2026-06-02-44`까지 번호화했다.
  - `scripts/check_report_units.js`, `npm run report:check`, doctor 검사를 추가했다.
  - autonomous dev loop prompt가 새 보고 섹션을 `# 2026-06-02-NN - 작업 제목` 형식으로 쓰게 했다.
- [x] v0.9 Work Package 2 Slice A: 전투 구간별 압박 고저차를 구현한다.
  - 브라우저 전투 스폰이 `숨 고르기 -> 압박 상승 -> 망각 전조`로 움직이게 했다.
  - `runTimeline.pressureSegments`와 `danger.pressure*Time` 로그를 추가했다.
  - AI simulator에 `pressureRhythm`, `pressureContrast` 지표를 추가했다.
  - `npm run qa:pressure`: `status: complete`, failures `[]`, segments `lull/rising/climax`.
  - `npm run ai:test:quick`: `GO_CANDIDATE`, Alpha Fun Score `0.885`, pressureContrast `0.4417`.
- [x] v0.9 Work Package 2 Slice B: 압박 고저차 검증 후 기존 전투 파라미터만 써서 최소 post-loss challenge를 구현한다.
  - 기억 상실 후 2기억 결손 구간이 `결손 정비 -> 결손 압박`으로 움직이게 했다.
  - 새 기억, 새 슬롯, 상점, 메타 진행, 새 지역, 무기 확장 없이 기존 적/스폰 파라미터만 사용했다.
  - `runTimeline.postLossChallenges`와 `danger.deficitBreathTime`, `danger.deficitChallengeTime`, `danger.postLossChallengeCompletions` 로그를 추가했다.
  - AI simulator에 `postLossChallengeScore`, `postLossChallengeContrast` 지표를 추가했다.
  - `npm run qa:postloss` 브라우저 QA entry를 추가했다.
  - 검증: `node --check src/game.js`, `node --check scripts/run_browser_pressure_qa.js`, `node --check alpha_test/src/simulator.js`, `node --check alpha_test/src/metrics.js`, `node --check alpha_test/src/run_alpha.js`, `node --check alpha_test/src/report.js` 통과.
  - 검증: `npm run ai:test:quick`: `GO_CANDIDATE`, Alpha Fun Score `0.8846`, post-loss challenge `0.6687`, contrast `0.3134`, 2-memory survival `79.0%`.
  - 검증: `npm run ai:test`: `GO_CANDIDATE`, Alpha Fun Score `0.8879`, post-loss challenge `0.6692`, contrast `0.3135`, 2-memory survival `78.8%`.
  - 검증: `npm run doctor`: 43 pass, 0 warn, 0 fail.
  - 현재 로컬 Chrome/CDP pipe는 `Target.getTargets` 응답 timeout으로 `npm run qa:postloss -- --timeout-ms 15000`와 `npm run qa:pressure -- --timeout-ms 15000`가 모두 실패했다. trusted local에서 브라우저 QA를 재확인해야 한다.
- [x] `2026-06-02-devloop-193946-feedback-1` Claude/Codex 피드백 공통점과 충돌을 정리했다.
  - 공통점: WP2 Slice B는 새 기억/슬롯/상점/메타/지역/무기 없이 기존 전투 파라미터만 쓴 범위 적합 구현이다.
  - 공통점: AI proxy는 `GO_CANDIDATE`, 낮은 irritation, 높은 restart intent, 2기억 생존율 약 `79%`로 planning 기준은 안정적이다.
  - 공통점: `qa:postloss` 실패는 `qa:pressure`와 같은 Chrome/CDP `Target.getTargets` 지점에서 발생했으므로 Slice B 로직 실패보다 로컬 브라우저 자동화 채널 blocker로 본다.
  - 공통점: `earlyChoiceInterest`와 `postLossChallengeContrast`는 아직 약하므로 사람 테스트/밸런스 근거로 과장하지 않는다.
  - 충돌: Claude는 스테이지 진입 전 2지선다 "기억 집중"을 권장했고, Codex CLI는 HUD/숫자키 기반 전투 중 집중 기억 지정을 권장했다.
  - 선택: trusted-local `npm run qa:postloss`를 먼저 통과시킨 뒤, WP3 Slice A는 기존 활성 기억 1개를 짧게 집중시키는 최소 tactical agency로만 진행한다. UI 표면은 구현 시 기존 구조에 가장 작게 맞는 쪽으로 결정한다.
- [x] `2026-06-02-devloop-193946-feedback-2` Claude/Codex 피드백 공통점과 충돌을 정리했다.
  - 공통점: WP2 Slice B와 QA runner fallback은 범위에 맞고, 새 gameplay content를 늘리지 않았다.
  - 공통점: AI proxy의 `GO_CANDIDATE`, Alpha Fun Score 약 `0.885`, 낮은 irritation, restart intent `0.90`은 planning pass일 뿐 browser/user evidence가 아니다.
  - 공통점: `qa:postloss`는 managed sandbox에서 pipe와 port fallback 모두 막혔으므로 trusted-local에서 다시 실행해야 한다.
  - 공통점: `earlyChoiceInterest`, `echoPivotScore`, `postLossChallengeScore`는 관찰 대상이며 지금 새 시스템으로 보정하지 않는다.
  - 충돌: 이번 feedback-2에는 실질적인 범위 충돌이 없다. Claude와 Codex 모두 WP3 이전 trusted-local post-loss QA를 최우선으로 본다.
  - 선택: 이번 cycle은 docs-only update로 닫는다. 다음 executable scope는 sandbox 밖 trusted-local `npm run qa:postloss` 하나이며, 통과 전에는 WP3, 사람 테스트, UI/튜토리얼/밸런스 변경을 시작하지 않는다.
- [x] `2026-06-02-devloop-193946-feedback-3` Claude/Codex 피드백 공통점과 충돌을 정리했다.
  - 공통점: WP2 Slice B와 `BrowserQaTransportError` 진단은 범위 적합한 QA/gate 작업이며 새 gameplay scope를 늘리지 않았다.
  - 공통점: AI proxy는 `GO_CANDIDATE`, Alpha Fun Score `0.8846`, regret `0.8073`, irritation `0.0104`, restart `0.90`로 긍정적이지만 browser/user evidence가 아니다.
  - 공통점: WP2 Slice B는 implementation-complete이지만 CDP pipe와 port fallback이 모두 실패했으므로 아직 browser-proven이 아니다.
  - 공통점: `earlyChoiceInterest`, `echoPivotScore`, `postLossChallengeScore`, 메모리 삭제 분포는 지금 새 시스템으로 보정하지 않고 browser gate 이후 관찰한다.
  - 충돌: 실질적인 다음 범위 충돌은 없다. Claude와 Codex 모두 WP3와 사람 테스트를 trusted-local post-loss proof 또는 명시적 environment-blocker 결정 전까지 막는다.
  - 선택: 이번 cycle은 docs-only update로 닫는다. 다음 executable scope는 trusted-local `npm run qa:postloss` 하나이며, 같은 transport 실패가 반복되면 `--timeout-ms 30000` 재시도 후 blocker prompt로 판단을 넘긴다.
- [x] `2026-06-02-devloop-193946-feedback-4` Claude/Codex 피드백 공통점과 충돌을 정리했다.
  - 공통점: WP2 Slice B와 post-loss QA port fallback hardening은 범위 적합한 QA/gate 작업이며 새 기억, 슬롯, 상점, 메타 진행, 지역, 무기, WP3 전술 시스템을 추가하지 않았다.
  - 공통점: AI proxy는 `GO_CANDIDATE`, Alpha Fun Score `0.8846`, regret `0.8073`, irritation `0.0104`, restart `0.90`로 사람 테스트 진입 가능성을 지지하지만 browser/user evidence는 아니다.
  - 공통점: WP2 Slice B는 implementation-complete지만 `npm run qa:postloss`가 gameplay evaluation 전에 CDP pipe timeout과 sandbox localhost bind `EPERM`으로 막혔으므로 아직 browser-proven이 아니다.
  - 공통점: `echoPivotScore`, `postLossChallengeScore`, `postLossChallengeContrast`는 관찰 대상으로 두고 지금 새 시스템으로 보정하지 않는다.
  - 충돌: 실질적인 다음 범위 충돌은 없다. Claude는 기술 blocker 해소 뒤 사람 테스트 가능성을 더 강하게 보지만, Codex CLI도 browser proof 또는 environment-blocker decision 전 WP3/사람 테스트 금지에 동의한다.
  - 선택: 이번 cycle은 docs-only update로 닫는다. 다음 executable scope는 trusted-local `npm run qa:postloss` 하나이며, 같은 transport 실패가 반복되면 `--timeout-ms 30000` 1회 재시도 후 blocker prompt로 판단을 넘긴다.
- [ ] trusted local에서 `npm run qa:postloss`를 재실행한다. 같은 CDP timeout이면 `npm run qa:postloss -- --timeout-ms 30000`을 한 번만 재시도하고, 필요하면 `npm run qa:pressure`로 자동화 채널 문제를 대조한다.
  - 이번 Codex 세션 결과: `npm run qa:postloss`와 `npm run qa:postloss -- --timeout-ms 30000`는 모두 Chrome/CDP `Target.getTargets` timeout으로 실패했다.
  - 대조 결과: `npm run qa:pressure`도 같은 지점에서 실패해 Slice B 로직 실패보다 현재 브라우저 자동화 채널 문제로 본다.
  - 보강: `scripts/run_browser_pressure_qa.js`에 CDP pipe 실패 시 remote-debugging-port/WebSocket CDP fallback을 추가했다.
  - 추가 보강: port fallback이 랜덤 포트 대신 OS가 비어 있다고 확인한 `127.0.0.1` 포트를 쓰고, pipe/port 경로가 동일한 headless sandbox flags를 공유하게 했다.
  - 현재 sandbox 한계: 보강 후에도 pipe는 `Target.getTargets` timeout으로 막히고, fallback은 `listen EPERM: operation not permitted 127.0.0.1`로 막혔다. 다음 실행은 sandbox 밖 trusted local에서 `npm run qa:postloss`를 다시 돌린다.
  - feedback-4 결정: sandbox 밖에서도 같은 transport 실패가 반복되면 `npm run qa:postloss -- --timeout-ms 30000`을 한 번만 재시도하고, 그래도 실패하면 새 gameplay 구현 없이 `docs/review_prompts/2026-06-02-postloss-browser-transport-blocker.md`로 진행 여부를 판단한다.
- [x] trusted-local post-loss QA gate 절차를 한 명령으로 묶는다.
  - `scripts/run_trusted_postloss_gate.js`와 `npm run qa:postloss:trusted`를 추가했다.
  - wrapper는 `npm run qa:postloss` 절차를 먼저 실행하고, Chrome/CDP transport 실패일 때만 `--timeout-ms 30000`으로 한 번 재시도한다.
  - 재시도 후에도 transport blocker면 `docs/review_prompts/2026-06-02-postloss-browser-transport-blocker.md`를 안내하고 WP3/사람 테스트/new gameplay scope 진입을 막는다.
  - 검증: `node --check scripts/run_trusted_postloss_gate.js`, `node --check scripts/check_local_pipeline.js`, `npm run doctor` 통과.
  - 검증: `npm run qa:postloss:trusted`는 이 managed sandbox에서 표준 실행과 30000 ms 재시도 모두 gameplay evaluation 전 transport blocker로 실패했다.
  - 다음 실행은 sandbox 밖 trusted local에서 `npm run qa:postloss:trusted`를 실행한다.
- [x] `2026-06-02-devloop-193946-feedback-5` Claude/Codex 피드백 공통점과 충돌을 정리했다.
  - 공통점: WP2 Slice B와 trusted post-loss QA wrapper는 범위 적합한 QA/gate 작업이며 새 gameplay scope를 늘리지 않았다.
  - 공통점: AI proxy는 `GO_CANDIDATE`, Alpha Fun Score `0.8846`, regret `0.8073`, irritation `0.0104`, restart `0.90`로 긍정적이지만 browser/user evidence가 아니다.
  - 공통점: managed sandbox의 실패는 gameplay assertion이 아니라 Chrome/CDP transport blocker이며, next command는 trusted-local `npm run qa:postloss:trusted`다.
  - 공통점: WP3 Slice A와 사람 테스트는 browser proof 또는 environment-blocker decision 전까지 막는다.
  - 충돌: 실질적인 다음 범위 충돌은 없다. Claude는 `멈춘 초침` 노출/삭제 패턴 관찰을 언급하지만 밸런스 변경은 금지했고, Codex CLI는 이번 루프의 다음 작업을 wrapper 실행 하나로 더 엄격히 제한했다.
  - 선택: 이번 cycle은 docs-only update로 닫는다. 다음 executable scope는 sandbox 밖 trusted-local `npm run qa:postloss:trusted` 하나이며, 통과 전에는 WP3, 사람 테스트, UI/튜토리얼/밸런스 변경을 시작하지 않는다.
- [x] 반복 transport 실패를 gameplay 실패와 구분하도록 QA runner 진단과 planning handoff를 보강한다.
  - `scripts/run_browser_pressure_qa.js`가 pipe와 port fallback 모두 실패하면 `BrowserQaTransportError`를 출력하게 했다.
  - 최신 `npm run qa:postloss` 출력은 다음 trusted-local command와 environment blocker 기록 조건을 함께 보여준다.
  - 환경 blocker 판단 프롬프트를 추가했다: `docs/review_prompts/2026-06-02-postloss-browser-transport-blocker.md`.
- [x] post-loss QA runner의 remote-debugging-port fallback을 더 결정적으로 만든다.
  - `findOpenPort()`가 랜덤 포트 추정 대신 OS-confirmed free port를 사용한다.
  - pipe/port Chrome launch args를 공통화하고 `--disable-dev-shm-usage`, `--no-sandbox`를 추가했다.
  - 검증: `node --check scripts/run_browser_pressure_qa.js` 통과.
  - 재검증: `npm run qa:postloss`, `npm run qa:postloss -- --timeout-ms 30000` 모두 gameplay evaluation 전 transport blocker로 실패했다.
- [ ] v0.9 Work Package 3 Slice A: 자동전투 안의 작은 tactical agency를 구현한다. 단, `npm run qa:postloss:trusted`가 trusted local에서 통과하거나 environment-blocker decision이 먼저 기록된 뒤에만 시작한다.
  - 기존 활성 기억 중 1개를 짧게 집중시키는 선택만 허용한다.
  - 새 기억, 새 슬롯, 상점, 메타 성장, 새 지역, 새 적, 새 무기 추가는 금지한다.
  - 목표 지표는 `earlyChoiceInterest > 0.72`, `postLossChallengeContrast >= 0.30`, 낮은 irritation 유지다.
- [ ] v0.9 통과 후에만 실제 브라우저 전투 QA와 사용자 1인 테스트를 요청한다.

## Pre-Human-Test Polish Gate

- 공격 이펙트가 기억별로 충분히 구분되는가?
- 자동 공격과 자동 기억 발동이 “그냥 돌아가는 것”이 아니라 빌드가 작동한다는 느낌을 주는가?
- 의존도가 올라가는 기억이 UI에서 자연스럽게 눈에 띄는가?
- 망각 결과가 전투에서 실제로 의존했던 기억과 연결되어 보이는가?
- 잔향 보상이 “삭제 보상”이 아니라 LETHE만의 상실 후 변형 시스템처럼 느껴지는가?

## Human Test Focus

- 초반 1-3분이 루즈하지 않고 적을 계속 잡고 싶게 만드는가?
- 첫 레벨업 선택이 “로그라이크/뱀서식 성장”으로 읽히는가?
- 3택 성장 선택을 고민하는가, 아니면 아무거나 누르는가?
- 보스 전 평균 2회 이상 성장 선택을 체감하는가?
- “방금 사라진 기억이 아까웠나요, 짜증났나요, 아니면 별 감정이 없었나요?”
- 플레이어가 첫 망각 전에 선택한 기억 3개 중 최소 1개에 애착이나 전략적 기대를 보이는가?
- 플레이어가 삭제될 기억을 예측할 때 자기 나름의 이유를 말하는가?
- 예측이 틀렸을 때도 결과를 납득하는가?
- echo 보상을 보고 “완전히 망했다”가 아니라 “잃었지만 이어진다”고 느끼는가?
- 망각 후 전투력이 약해졌다는 체감이 있는가?
- replacement 이후 회복이 무효화처럼 느껴지는가, 상처를 안고 적응하는 것처럼 느껴지는가?
- 플레이어가 “HTML 프로토타입인데도 더 해보고 싶다”는 반응을 보이는가?
- 이 아이디어를 Unity로 옮기면 더 재미있어질 가능성이 있다고 느끼는가?

## Unity Transition Gate

- 5-8명 사람 테스트에서 초반 1-3분 재미 반응이 대체로 긍정적이어야 한다.
- 레벨업 선택이 단순 버튼이 아니라 런 중 성장 기대감으로 읽혀야 한다.
- 첫 망각까지 도달한 플레이어가 “짜증”보다 “아까움/후회/납득”에 가까운 반응을 보여야 한다.
- 망각 후 잔향과 replacement가 완전한 무효화가 아니라 상실 후 적응으로 읽혀야 한다.
- 최소 일부 플레이어가 다시 플레이하거나 다른 빌드를 시도하고 싶다고 말해야 한다.
- 위 조건이 모이면 Claude/GPT에 테스트 결과를 넘겨 Unity 전환 여부와 다음 설계 방향을 판단한다.
- 조건이 부족하면 Unity 구현으로 넘어가지 않고 HTML v0.6에서 초반 재미, 성장 선택, 망각 감정 중 가장 약한 축을 다시 보완한다.

## Do Not Build Yet

- Meta progression.
- Shop systems.
- Final boss.
- More than the current 6 memories.
- More than 3 active memory slots.
- Multi-region run structure.
- Additional bosses.
- Large weapon expansion.
- Memory synthesis, strengthening, or upgrade systems.
- Complex ending branches.
- Narrative cutscene expansion.
- Save/load campaign structure.
- Log analysis dashboard.
- Difficulty selection.
- Character selection.
- Unlock systems.
