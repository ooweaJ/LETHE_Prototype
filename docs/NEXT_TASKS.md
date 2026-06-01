# Next Tasks

현재 단계는 v0.2 narrow tuning 구현 후 검증 상태다. 새 콘텐츠를 늘리는 단계가 아니라, 망각 루프가 사람 테스트에서 해석 가능한지 확인하는 단계다.

## Current Verdict

- GPT verdict: `ITERATE_BEFORE_TEST`.
- Claude verdict: `ITERATE_BEFORE_TEST`.
- Codex implementation result: `GO_CANDIDATE` from `npm run ai:test`.

## v0.2 Done

- [x] 첫 망각 타이밍을 평균 8-10분 범위로 보정했다.
- [x] `처형자의 섬광` 삭제율을 25-35% 범위로 낮췄다.
- [x] 단일 기억 삭제 쏠림을 AI 기준 허용 범위로 낮췄다.
- [x] JSON 로그에 선택 기억, 예측 기억, 실제 삭제 기억, 기억별 삭제 weight를 추가했다.
- [x] 기본 실험값을 `echo=0.50`, `ui=0.62`로 바꿨다.
- [x] 현재 echo/ui 값을 로그와 AI 결과에 남긴다.
- [x] 망각 결과 화면에 사라진 기억, 예측 결과, 삭제 weight, 잔향 효과, 다음 방향을 표시한다.
- [x] 사람 테스트용 Q3 자유응답 문항을 추가했다.

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

- [ ] 브라우저에서 v0.2 화면 QA를 한다.
- [ ] 결과 화면에서 텍스트가 겹치지 않는지 확인한다.
- [ ] JSON 다운로드에 새 필드가 들어가는지 실제 브라우저에서 확인한다.
- [ ] 5-8명용 human playtest 가이드를 작성한다.
- [ ] 플레이테스트 후 감정 반응을 기준으로 v0.3 방향을 결정한다.

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
