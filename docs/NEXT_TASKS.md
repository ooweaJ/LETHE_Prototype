# Next Tasks

현재 단계는 v0.4 human-test candidate 상태다. AI 기준으로는 사람 테스트 진입 가능하며, 다음 큰 판단은 실제 플레이어 5-8명 반응이다.

## Current Verdict

- GPT verdict: `ITERATE_BEFORE_TEST`.
- Claude verdict: `ITERATE_BEFORE_TEST`.
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

## Latest AI Criteria

- [x] Verdict: `GO_CANDIDATE`.
- [ ] Alpha Fun Score: `0.89+`. 현재 `0.8261`; 사람 테스트 전 기준으로는 충분하지만 목표치에는 아직 못 미친다.
- [x] 첫 망각 시간: `9.00 min`.
- [x] Regret proxy: 목표 `85%+`, 현재 `85.6%`.
- [x] Irritation proxy: 목표 `3%` 이하, 현재 `0.4%`.
- [x] Restart intent: 목표 `65%+`, 현재 `70.9%`.
- [ ] Post-forgetting power drop: 목표 `30-40%`, 현재 `29.6%`.
- [x] Recovery after replacement: 목표 `90%+`, 현재 `96.6%`.
- [x] Prediction match: 목표 `75-90%`, 현재 `85.8%`.
- [x] `처형자의 섬광` deletion share: 목표 `25-35%`, 현재 약 `28.0%`.

## Next Codex Tasks

- [x] 브라우저에서 v0.2 화면 QA를 한다.
- [x] 결과 화면에서 텍스트가 겹치지 않는지 확인한다.
- [x] JSON 다운로드에 새 필드가 들어가는지 실제 브라우저에서 확인한다.
- [x] 5-8명용 human playtest 가이드를 작성한다.
- [x] 사람 테스트 전 전투 연출을 더 화려하게 만든다.
- [x] 사람 테스트 전 기억 의존도/망각 위험이 플레이 중 더 분명하게 보이게 만든다.
- [x] 사람 테스트 전 잔향 시스템이 결과 이후 더 분명하게 보이게 만든다.
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
