# Next Tasks

현재 단계는 v0.2 narrow tuning이다. 새 콘텐츠를 늘리지 말고, 망각 루프를 사람 테스트에서 해석 가능한 상태로 보정한다.

## Current Verdict

- GPT verdict: `ITERATE_BEFORE_TEST`.
- Claude verdict: `ITERATE_BEFORE_TEST`.
- 근거 문서:
  - `docs/review_responses/2026-06-02-gpt-v02.md`
  - `docs/review_responses/2026-06-01-claude.md`

## v0.2 작업

- [ ] 첫 망각 타이밍을 평균 또는 중앙값 8-10분, 가능하면 8.0-9.5분 범위로 보정한다.
- [ ] `처형자의 섬광` 삭제율을 25-35% 범위로 낮춘다.
- [ ] 기억별 삭제 분포에서 한 기억이 전체 테스트를 과도하게 대표하지 않도록 확인한다.
- [ ] JSON 로그에 선택 기억, 예측 기억, 실제 삭제 기억, 각 기억의 삭제 weight 또는 score를 추가한다.
- [ ] 실험 빌드에서 `echo=0.50`, `ui=0.62`를 사용할 수 있게 한다.
- [ ] 기존 `echo=0.60`과 비교 가능하도록 현재 echo preset 또는 값을 로그에 남긴다.
- [ ] 망각 후 피드백 화면에서 사라진 기억, 예측 결과, 관계, 잔향 효과를 쉽게 읽히게 한다.
- [ ] 사람 테스트용 설문에 “방금 사라진 기억의 이름을 말할 수 있나요?” 또는 같은 의미의 자유응답 문항을 추가한다.

## v0.2 AI 테스트 기준

- [ ] Verdict: `GO_CANDIDATE` 이상.
- [ ] Alpha Fun Score: `0.89+`.
- [ ] 첫 망각 시간: 평균 또는 중앙값 8-10분.
- [ ] 후회 proxy: `85%+`.
- [ ] 짜증 proxy: `3-5%` 이하.
- [ ] 재시작 의향: `65%+`.
- [ ] 망각 직후 전투력 하락: `30-40%`.
- [ ] 대체 기억 후 회복률: `90%+`.
- [ ] 예측 일치율: `75-90%`.
- [ ] `처형자의 섬광` 삭제율: `25-35%`.
- [ ] 단일 기억 삭제율 최대-최소 격차가 과도하지 않은지 확인한다.
- [ ] `echo=0.50`과 `echo=0.60` 조건을 비교할 수 있게 결과를 분리한다.

## 사람 테스트 준비

- [ ] 5-8명 소규모 테스트로 준비한다.
- [ ] 핵심 질문: “방금 사라진 기억이 아까웠나요, 짜증났나요, 아니면 별 감정이 없었나요? 왜 그렇게 느꼈나요?”
- [ ] 플레이어가 사라질 기억을 예측할 때 자기 이유를 말하는지 관찰한다.
- [ ] 예측이 틀렸을 때도 결과를 납득하는지 관찰한다.
- [ ] echo 보상이 상실을 무효화하는지, 상처를 안고 적응하게 하는지 관찰한다.
- [ ] 망각 직후 5초 안에 아쉬움, 당황, 납득, 억울함 중 어떤 반응이 나오는지 기록한다.
- [ ] 세션 종료 후 사라진 기억 이름을 자유응답으로 적을 수 있는지 확인한다.

## Do Not Build Yet

- [ ] 메타 progression.
- [ ] 상점.
- [ ] 영구 성장.
- [ ] 최종 보스.
- [ ] 7개 이상의 기억.
- [ ] 3개 초과 active memory slot.
- [ ] 다중 지역 run structure.
- [ ] 추가 보스.
- [ ] 추가 weapon 대량 확장.
- [ ] 기억 합성, 강화, 업그레이드 시스템.
- [ ] 복잡한 엔딩 분기.
- [ ] narrative cutscene 확대.
- [ ] save/load 캠페인 구조.
- [ ] 로그 분석 대시보드.
- [ ] 난이도 선택.
- [ ] 캐릭터 선택.
- [ ] unlock 시스템.

## Recently Done

- [x] HTML alpha v0.1 구현.
- [x] AI alpha test tool 통합.
- [x] GitHub repository push.
- [x] Codex-GPT 기록 워크플로우 문서화.
- [x] Discord report delivery workflow.
- [x] GPT/Claude planning prompt file prepared.
- [x] Codex runbook and checkpoint workflow documented.
- [x] Short Discord status notice script added.
- [x] GPT v0.2 planning answer converted into Codex tasks.
- [x] Claude Code automatic review script tested and response saved.
