# Next Tasks

현재 단계는 v0.2 narrow tuning 구현 후 QA 완료 상태다. 다만 사람 테스트는 바로 진행하지 않고, 전투 연출과 LETHE 고유 시스템 체감이 더 분명해진 프로토타입을 만든 뒤 진행한다.

## Current Verdict

- GPT verdict: `ITERATE_BEFORE_TEST`.
- Claude verdict: `ITERATE_BEFORE_TEST`.
- Codex implementation result: `GO_CANDIDATE` from `npm run ai:test`.

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

## Latest AI Criteria

- [x] Verdict: `GO_CANDIDATE`.
- [ ] Alpha Fun Score: `0.89+`. 현재 `0.7737`; 감정 프록시가 보수적으로 낮아졌으므로 사람 테스트에서 실제 반응 확인 필요.
- [x] 첫 망각 시간: `9.00 min`.
- [ ] Regret proxy: 목표 `85%+`, 현재 `76.3%`.
- [x] Irritation proxy: 목표 `3%` 이하, 현재 `1.1%`.
- [x] Restart intent: 목표 `65%+`, 현재 `70.0%`.
- [ ] Post-forgetting power drop: 목표 `30-40%`, 현재 `29.6%`.
- [x] Recovery after replacement: 목표 `90%+`, 현재 `96.6%`.
- [x] Prediction match: 목표 `75-90%`, 현재 `76.3%`.
- [x] `처형자의 섬광` deletion share: 목표 `25-35%`, 현재 약 `28.0%`.

## Next Codex Tasks

- [x] 브라우저에서 v0.2 화면 QA를 한다.
- [x] 결과 화면에서 텍스트가 겹치지 않는지 확인한다.
- [x] JSON 다운로드에 새 필드가 들어가는지 실제 브라우저에서 확인한다.
- [x] 5-8명용 human playtest 가이드를 작성한다.
- [ ] 사람 테스트 전 전투 연출을 더 화려하게 만든다.
- [ ] 사람 테스트 전 기억 의존도/망각/잔향 시스템이 플레이 중 더 분명하게 보이게 만든다.
- [ ] 플레이테스트 후 감정 반응을 기준으로 v0.3 방향을 결정한다.

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
