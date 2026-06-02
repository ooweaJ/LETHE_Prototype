# Next Tasks

현재 단계는 v0.5 core-fun human-test candidate 상태다. 사용자의 실제 테스트 피드백에 따라 “망각 감정”보다 먼저 초반 재미, 적 몰림, 런 중 성장 선택을 보완했다. AI 기준으로는 다시 사람 테스트 진입 가능하다.

## Current Verdict

- GPT verdict: `ITERATE_BEFORE_TEST`.
- Claude v0.5 evaluation: `ITERATE_BEFORE_TEST`, but only because browser/runtime verification is still missing. If v0.5 level-up flow and `runGrowth` JSON pass in a real browser, proceed to `GO_TO_HUMAN_TEST`.
- Codex implementation result: `GO_CANDIDATE` from `npm run ai:test` and `npm run ai:test:heavy`.

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
- [x] OpenAI 기획 검토 fallback 명령을 추가했다.

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
- [ ] v0.5 브라우저 시각 QA를 로컬에서 다시 확인한다. 이 세션의 Browser 플러그인은 Windows sandbox 오류로 열리지 않았다.
- [ ] 레벨업 3택 화면이 실제 브라우저에서 뜨고, 선택 후 전투가 정상 재개되는지 확인한다.
- [ ] 한 런 완료 후 JSON 다운로드에 `runGrowth` 선택 내역이 실제 선택과 일치하는지 확인한다.
- [ ] 레벨업 일시정지 중 타이머/스폰이 멈추고 재개 후 입력이 정상인지 확인한다.
- [ ] 5-8명 사람 테스트를 진행한다.
- [ ] 플레이테스트 후 감정 반응을 기준으로 다음 방향을 결정한다.
- [ ] 사람 테스트 결과가 모이면 Claude/GPT 검토를 다시 요청한다. 외부 전송 승인이 어려우면 Codex CLI fallback이나 Claude mock 경로로 자동화만 먼저 점검한다.

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
