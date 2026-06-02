# LETHE v0.7 자동 밸런스/잔향 시너지 결과 판단 요청 - 2026-06-02

## 목적

v0.6 1인 체감 피드백에서 “기억을 잃은 뒤 너무 약해져 쫄몹을 거의 못 잡는다”는 문제가 나왔다.

v0.7은 이 피드백을 자동 루프의 입력으로 삼아, 사람 테스트 전에 무기 바닥 성능과 잃은 기억의 무기 잔향 시너지를 보강한 버전이다.

Claude/GPT는 다음을 판단한다:

- v0.7을 사용자 1인 체감 테스트로 보내도 되는가?
- 아니면 v0.7.1 자동 밸런스 보정을 한 번 더 해야 하는가?
- 보정한다면 어떤 수치를 우선 봐야 하는가?

## 사용자 피드백

- v0.6은 구조는 좋아졌지만, 기억을 잃은 뒤 갑자기 너무 약해진다.
- 쫄몹을 제대로 못 잡는 느낌이 있다.
- 무기 성능, 무기 종류, 무기 특수기, 잃은 기억과 무기의 시너지 같은 전투/빌드 기획 고도화가 필요해 보인다.
- 밸런스는 한 번에 잡힐 수 없으므로 자동화된 반복 버전업으로 조정하고 싶다.

## v0.7에서 구현한 것

- 버전을 `v0.7`로 올렸다.
- 신규 무기 대량 추가는 보류하고, 기존 2개 무기의 기본 성능을 먼저 보강했다.
- `절단쌍검`
  - damage `12 -> 15`
  - range `74 -> 86`
  - interval `0.42 -> 0.36`
  - 역할을 “기억 결손 구간에서도 쫄몹을 정리하는 안정형 무기”로 조정했다.
- `장송대검`
  - damage `34 -> 42`
  - range `112 -> 128`
  - interval `1.18 -> 1.02`
  - 역할을 “기억 결손 구간에서 한 줄을 뚫는 돌파형 무기”로 조정했다.
- 잃은 기억이 무기에 남는 잔향 효과를 추가했다:
  - `처형자의 섬광` 상실 -> 무기 섬광 연쇄 chance.
  - `굶주린 칼무리` 상실 -> 무기 출혈 잔향.
  - `추적자의 맹세` 상실 -> 무기 타격 후 보조 유도탄 chance.
  - `파쇄의 파문` 상실 -> 무기 타격 중 작은 파문 chance.
  - `피의 반사` 상실 -> 무기 출혈/온힛 잔향 보강.
  - `멈춘 초침` 상실 -> 무기 타격 둔화 chance.
- 사이드 패널 잔향 표시에도 무기 잔향 줄을 추가했다.
- AI 시뮬레이터도 무기 baseDps와 weapon residue echo proxy를 반영하도록 갱신했다.

## v0.7 테스트 결과

### Quick 120런

- Command: `npm run ai:test:quick`
- Verdict: `GO_CANDIDATE`
- Alpha Fun Score: `0.9339`
- Regret: `88.9%`
- Irritation: `0.0%`
- Prediction: `92.6%`
- Early Fun: `0.8772`
- Kill Tempo: `0.9815`
- Level-ups before boss: `4.2`
- First cycle completion: `82.1%`
- Two-memory survival: `81.6%`
- Echo pivot score: `0.7363`

### Default 1000런

- Command: `npm run ai:test`
- Verdict: `GO_CANDIDATE`
- Alpha Fun Score: `0.9131`
- Regret: `84.0%`
- Irritation: `0.4%`
- Prediction: `87.6%`
- Early Fun: `0.8793`
- Kill Tempo: `0.9847`
- Level-ups before boss: `4.16`
- First cycle completion: `84.1%`
- Two-memory survival: `81.5%`
- Echo pivot score: `0.7349`

### Heavy 5000런 / 3 stages

- Command: `npm run ai:test:heavy`
- Verdict: `GO_CANDIDATE`
- Alpha Fun Score: `0.9136`
- Regret: `84.5%`
- Irritation: `0.3%`
- Prediction: `88.1%`
- Early Fun: `0.8787`
- Kill Tempo: `0.9854`
- Level-ups before boss: `4.15`
- First cycle completion: `82.7%`
- Two-memory survival: `81.1%`
- Echo pivot score: `0.7286`

## 브라우저 QA 결과

### cycle QA

- URL: `file:///C:/jaewoo/LETHE_Prototype/index.html?qa=fast,v06`
- Version: `v0.7`
- runTimeline version: `v0.7`
- Status: `complete`
- bossSpawned: `true`
- forgotten: `true`
- deficitStarted: `true`
- refilled: `true`
- hasTimelinePayload: `true`

### level-up regression QA

- URL: `file:///C:/jaewoo/LETHE_Prototype/index.html?qa=fast,levelup&tester=T01&session=S01`
- Version: `v0.7`
- Status: `complete`
- levelUpSeen: `true`
- resumedAfterUpgrade: `true`
- hasRunGrowthPayload: `true`

## 알려진 리스크

- AI 지표상 초반 처치 템포는 좋아졌지만, 실제 체감상 “너무 쉬워졌는지”는 아직 모른다.
- Echo pivot score는 v0.6보다 살짝 낮아졌다. 무기 바닥 성능이 올라가면서 “상실 후 변형”보다 “무기 자체로 해결”처럼 느껴질 위험이 있다.
- 예측율이 여전히 높다. 특히 quick 기준 92%대라 정답 맞히기처럼 보일 수 있다.
- 신규 무기/무기 특수기는 아직 넣지 않았다.

## 판단 요청

1. v0.7은 사용자 1인 체감 테스트로 넘겨도 되는가?
2. 아니면 v0.7.1 자동 보정을 한 번 더 해야 하는가?
3. v0.7.1이 필요하다면 우선순위는 무엇인가?
   - 무기 바닥 성능 조정,
   - 잔향 무기 시너지 가시성 강화,
   - 결손 구간 적 밀도 조정,
   - 예측율 조정,
   - 무기 특수기 추가,
   - 또는 다른 것.
4. 신규 무기나 무기 특수기는 지금 넣어야 하는가, 아니면 기존 2무기와 잔향 시너지 안정화 후에 넣어야 하는가?
5. 다음 자동 루프에서 절대 만들지 말아야 할 것은 무엇인가?

## 답변 형식

## 결론

- `GO_TO_SOLO_PLAYTEST`, `ITERATE_V071_BALANCE`, `FIX_COMBAT_CORE` 중 하나를 고른다.

## 이유

- 핵심 판단 근거 3-5개.

## 다음 Codex 작업

- [ ] 작업 1
- [ ] 작업 2
- [ ] 작업 3

## 1인 테스트에서 볼 것

- 기억 상실 후 쫄몹 처리:
- 무기 바닥 성능:
- 잔향 무기 시너지:
- 결손 구간 난이도:
- 다시 하고 싶은지:

## 아직 만들지 말 것

- 이번 자동 루프에서 제외할 범위.
