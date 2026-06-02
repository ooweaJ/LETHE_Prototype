# Next Tasks

현재 단계는 v0.5 core-fun human-test ready 상태다. 사용자의 실제 테스트 피드백에 따라 “망각 감정”보다 먼저 초반 재미, 적 몰림, 런 중 성장 선택을 보완했고, Claude가 요구한 레벨업 UI/`runGrowth` 브라우저 gate도 Chrome headless로 통과했다.

이 프로젝트의 현재 목표는 HTML 프로토타입으로 LETHE의 핵심 재미와 가능성을 검증하는 것이다. 충분히 재미가 확인되면 그 결과를 근거로 Unity 구현 단계로 넘어간다.

## Current Verdict

- GPT verdict: `ITERATE_BEFORE_TEST`.
- Claude v0.5 evaluation: `GO_TO_HUMAN_TEST` after Chrome headless QA confirmed the level-up flow and `runGrowth` payload.
- Codex implementation result: `GO_CANDIDATE` from `npm run ai:test` and `npm run ai:test:heavy`.
- Project direction: HTML prototype validation first, Unity implementation later only if AI/human tests show enough promise.

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
- [ ] 5-8명 사람 테스트를 진행한다.
- [ ] 플레이테스트 후 감정 반응을 기준으로 다음 방향을 결정한다.
- [ ] 사람 테스트 결과가 모이면 Claude/GPT에 결과를 보고하고 기획 수정 방향을 받는다. 외부 전송 승인이 어려우면 Codex CLI fallback이나 Claude mock 경로로 자동화만 먼저 점검한다.

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
