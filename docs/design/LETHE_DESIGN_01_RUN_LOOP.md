# 01. 런 루프 / 타임라인

최종 갱신: 2026-06-27 · 출처: `src/game.js` (`experiment`, `balance.spawnCaps`, `pressureProfile`, `pressureEnemyPool`) + Unity v1 20분 밸런스 시뮬레이션

한 판은 "강해짐 → 잃음 → 잔향으로 변형 → 다시 강해짐"을 반복한다. 이 문서는 그 1판의 시간 구조와 적 압박 곡선을 정의한다.

## Unity 베타 20분 목표

Unity `Dev_Prototype_v1`의 베타 플레이 기준은 HTML v0.12의 600초 압축 검증을 그대로 늘리는 것이 아니라, 현대 플레이 템포에 맞춘 **20분 목표 런**으로 잡는다.

| 항목 | 값 | 의도 |
| --- | --- | --- |
| 목표 클리어 체감 | 18~22분 | 한 판이 부담스럽지 않으면서 빌드 완성감을 준다 |
| 하드 캡 | 1260초(21분) | 최종 문지기 미돌파 시 실패 처리 |
| 문지기 스케줄 | 300s / 600s / 900s / 1140s | 5분 단위로 빌드 검문, 마지막 19분 전후 결산 |
| 첫 보상 목표 | 24~30초 | 초반 XP가 너무 빠르게 튀지 않게 함 |
| 첫 망각 목표 | 약 5분 20초 | 첫 문지기 처치 직후 상실과 잔향 전환을 경험 |
| 궁극 잔향 목표 | 약 15~16분 | 후반 4~5분 동안 궁극을 실제 전투에서 쓰게 함 |
| 기본 클리어 조건 | 궁극 잔향 1종 완성 + 최종 문지기 처치 | 2궁극은 고급/추가 목표로 남김 |

2026-06-27 `scripts/balance_sim_v1.js` 1차 결과 기준으로 `20m_slow_start` 후보가 채택되었다. 평균 신호는 첫 선택 24~28초, 첫 망각 323~329초, 궁극 완성 936~945초, 클리어 1178~1188초다.

## 런 기본 수치

아래 표는 HTML v0.12 압축 검증의 원본 수치다. Unity 베타 런에서는 위의 20분 목표 수치가 우선한다.

| 항목 | 값 | 출처 |
| --- | --- | --- |
| 한 판 길이 | 600초 | `experiment.runDurationSec` |
| 보스(문지기) 스케줄 | 180s / 340s / 490s / 600s | `experiment.bossScheduleSec` |
| 결손 생존 길이 | 54초 | `experiment.deficitDurationSec` |
| 잔향 파워 계수 | 0.5 | `experiment.echoPower` |

QA 압축 모드(`?qa=fast`): 길이 96초, 보스 18/38/62/88s, 보스HP 180, 결손 5초. Unity 압축 디버그 스모크(60~120초)는 이 fast 프로파일을 기준으로 한다.

## 단계별 흐름

| 단계 | 트리거 | 플레이어 경험 |
| --- | --- | --- |
| 시작 | 무기 1 선택, 첫 보상에서 기억 선택 | 무기 리듬을 먼저 느낀 뒤 빌드 방향 결정 |
| 초반 성장 | 레벨업·기억 획득 | 2~3개 능력이 돌아가기 시작 |
| 첫 문지기 | Unity 베타 300초 / HTML 원본 180초 | 내 빌드가 통하는지 시험 |
| 첫 망각 | 문지기 처치 후 | 최고 레벨 기억 상실, 아쉬움 |
| 결손 생존 | 활성 기억 < 3 | 잃은 능력의 빈자리 + 잔향 첫 등장 |
| 보충 | 결손 종료 후 | 재획득(공명) 또는 새 기억 피벗 |
| 잔향 목표 | 후반 | +5 잔향, 궁극 조합 추구 |

## Unity 베타 클리어 판정

- 1260초 생존만으로는 승리하지 않는다.
- 최종 문지기까지 모두 처치해야 클리어로 판정한다.
- 클리어 전 목표는 궁극 잔향 1종 완성이다. 기본 루트는 `피의 칼폭풍`, 대체 루트는 `파쇄 처형`, `정지 추적`, `잿빛 망각`이다.
- 궁극 2종 완성은 현재 베타의 필수 조건이 아니라 숙련/고점 목표로 둔다.

## 압박 페이즈 (적 밀도 곡선)

페이즈는 "직전 보스 → 다음 보스" 사이 진행도(progress)로 결정된다. 출처: `pressureProfile()`.

### 일반 사이클 (활성 기억 3개일 때)

| progress | 페이즈 | spawnRate(초) | packSize | intensity |
| --- | --- | --- | --- | --- |
| < 0.24 | 숨 고르기(lull) | 0.72 | 2 (≥70s면 3) | 0.42 |
| 0.24~0.70 | 압박 상승(rising) | 1.05 (≥126s면 0.54) | 2 (≥126s면 3) | 0.70 |
| ≥ 0.94 (첫 사이클만) | 문지기 호흡(gate_breath) | 1.30 | 1 | 0.56 |
| 그 외 ≥0.70 | 망각 전조(climax) | 첫사이클 1.08 / 이후 0.43 | 첫사이클 2 / 이후 4 | 첫 0.78 / 이후 0.92 |

`spawnRate`는 스폰 간격(초). 값이 작을수록 자주 스폰. `spawnRate` 하한은 0.34초.

### 결손 사이클 (활성 기억 < 3일 때)

| progress | 페이즈 | spawnRate | packSize | intensity |
| --- | --- | --- | --- | --- |
| < 0.30 | 결손 정비(deficit_breath) | 0.72 | 2 | 0.48 |
| ≥ 0.30 | 결손 압박(deficit_trial) | 0.50 (>0.72면 0.44) | 3 (>0.72면 4) | 0.82 |

## 최대 동시 적 수 (spawnCaps)

출처: `balance.spawnCaps` + `pressureMaxEnemies()`.

| 상황 | 캡 |
| --- | --- |
| 첫 사이클 lull | 34 |
| 첫 사이클 rising | 34 |
| 첫 사이클 climax | 32 |
| 첫 사이클 gate_breath | 22 |
| 결손 정비 | 16 |
| 결손 압박 | 14 |
| 이후 사이클 climax | 46 |
| 기본/그 외 | 46 |

적이 캡에 도달하면 스폰 쿨다운을 `spawnRate * 0.75`로 늘려 과밀을 막는다.

## 적 풀 (페이즈별 스폰 후보)

출처: `pressureEnemyPool()`. base = `[침식자×3, 떠도는눈, 쪼개진자]`.

| 페이즈 | 풀 |
| --- | --- |
| lull (<70s) | 침식자×2, 떠도는눈 |
| lull (≥70s) | 침식자×2, 떠도는눈, 쪼개진자 |
| rising | base (+ >95s면 공허사제) |
| gate_breath | 침식자×2, 떠도는눈 |
| climax (첫 사이클) | base |
| climax (이후) | base + 떠도는눈, 쪼개진자, 공허사제 |
| deficit_breath | 침식자×2, 떠도는눈, 쪼개진자 |
| deficit_trial | base + 떠도는눈, 공허사제 |

## 망각 타이밍 규칙

- 망각은 **문지기(보스) 처치 직후** 발생한다(첫 문지기 = 첫 망각 게이트).
- 대상은 활성 기억 중 **가장 레벨 높은 기억**. 동률이면 플레이어가 선택. 디버그 빌드는 최근 강화 기억 우선.
- 완전 랜덤 삭제처럼 보이면 안 된다. 결과 화면은 "사라진 기억 + 남은 잔향"을 함께 보여준다([05_UI_UX](LETHE_DESIGN_05_UI_UX.md)).
- 상세 변환 규칙은 [03_MEMORY_ECHO](LETHE_DESIGN_03_MEMORY_ECHO.md).

## 결손 생존

활성 기억이 3개 미만이면 결손 사이클로 들어간다.

- 단순 약화 구간이 아니라 **적응 구간**이다.
- 잃은 기억의 빈자리를 느끼게 하고, 잔향 효과가 처음으로 보이게 한다.
- 길면 짜증이 되므로 54초로 제한. 종료 시 보충 오버레이를 연다(`updateRefillGate`).

## 보충 선택 (결손 종료 후)

세 방향을 제공한다.

- **잃었던 기억 재획득** → 공명으로 강화.
- **새 기억 획득** → 새 빌드 방향.
- **기존 기억 강화** → 안정적 성장.

## 첫 사이클 완료 기준

- 첫 망각이 발생한다.
- 잃은 기억 레벨이 잔향으로 전환된다.
- 잔향이 남은 기억을 재획득하면 강화된다.
- 결손 생존 후 보충 선택이 작동한다.
- 플레이어가 기억/잔향 상태를 UI에서 확인할 수 있다.
# 2026-06-29 Addendum: Stepped Boss Curve Candidate

Jaewoo direct review note: the first boss currently arrives too late, the early run feels boring, and boss intervals do not yet create a stepwise rise in tension. The deficit survival pocket also looks like a candidate for removal because it creates downtime after forgetting.

This is the next balance candidate, not yet the applied Unity runtime value:

| Item | Candidate value | Intent |
| --- | --- | --- |
| Run hard cap | 1200s | Keep the run near 20 minutes, but make the final goal arrive before timeout padding. |
| Gatekeeper schedule | 150s / 360s / 660s / 1020s | First boss at 2:30, then intervals grow 150 -> 210 -> 300 -> 360 seconds. |
| Gatekeeper HP | 1200 / 2250 / 4050 / 8650 | Calculated from expected average boss DPS and target TTK. |
| Target boss TTK | 18s / 26s / 36s / 48s | Boss fights become longer and harder as the build matures. |
| First reward target | 20~30s | Keep early choice momentum without returning to the old XP flood. |
| First forgetting target | Immediately after 150s Gatekeeper clear | The first loss/echo moment becomes an early hook instead of a 5-minute wait. |
| Deficit survival | Remove as a separate timer | Forget, grant echo, then return to reward/combat flow without a 54s low-agency pocket. |

Detailed calculation evidence: `docs/orchestration/evidence/2026-06-29-stepped-boss-xp-dps-plan.md`.
