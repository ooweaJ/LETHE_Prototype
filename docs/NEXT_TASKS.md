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
- [ ] v0.9 프롬프트를 Claude + Codex CLI 더블 체크에 보내 release-feel 작업 단위를 확정한다.
- [x] v0.9 프롬프트를 Claude + Codex CLI 더블 체크에 보내 release-feel 작업 단위를 확정한다.
- [x] v0.9 더블 체크 요약에 공통점/충돌점/선택 범위를 정리한다.
- [x] 밤샘 루프 Discord 진행 알림을 기본값으로 고정한다.
- [x] 실제 구현까지 반복하는 autonomous dev loop를 추가한다.
- [x] `doctor`와 `doctor:deep`이 autonomous dev loop dry-run을 확인하게 한다.
- [ ] v0.9 Work Package 1: 기존 6개 기억 안에서 빌드 정체성과 시너지 체감을 강화한다.
  - [x] 기억 선택 카드와 슬롯에서 기억별 주 역할, 태그, 짧은 전투 설명을 함께 보이게 했다.
  - [x] 현재 빌드 이름, 활성 시너지, 의존 중인 기억을 선택 화면/HUD에 표시한다.
  - [x] JSON payload, 이벤트 로그, AI raw-run payload에 build identity 필드를 추가한다.
  - [x] Claude/Codex 피드백을 받아 WP1 다음 작업을 identity QA runner로 확정한다.
  - [x] `npm run qa:identity` 전용 Chrome/CDP identity QA runner를 추가했다.
  - [x] `?qa=fast,identity`를 실제 브라우저 또는 안정적인 headless runner에서 검증한다.
    - `npm run qa:identity`: `status: complete`, failures `[]`.
  - [ ] 필요하면 기억별 설명 문구를 더 압축해 90초 안에 더 쉽게 읽히게 한다.
- [ ] v0.9 Work Package 2: 압박 고저차와 post-loss challenge를 구현한다.
- [ ] v0.9 Work Package 3: 자동전투 안의 작은 tactical agency를 구현한다.
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
