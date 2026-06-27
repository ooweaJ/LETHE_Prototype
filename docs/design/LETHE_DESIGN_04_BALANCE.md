# 04. 성장 / 밸런스 / 전 상수표

최종 갱신: 2026-06-27 · 출처: `src/game.js` (`balance`, `levelUpChoices`, `queueLevelUp`, `chooseLevelUpChoices`) + Unity v1 20분 밸런스 시뮬레이션

이 문서는 HTML v0.12에서 검증된 원본 수치와 Unity v1 베타 플레이용 20분 목표 수치를 함께 기록한다. Unity `Dev_Prototype_v1`에서는 아래의 "Unity 베타 20분 1차 적용값"이 우선한다.

## Unity 베타 20분 1차 적용값

2026-06-27 기준 1차 목표는 "초반 레벨업 과속을 낮추고, 궁극 1종을 15~16분에 완성한 뒤, 19~20분대 최종 문지기 처치로 끝나는 런"이다.

| 항목 | 값 | 의도 |
| --- | --- | --- |
| 시작 필요 XP | 7 | 첫 보상을 24~30초 전후로 늦춤 |
| 0~120초 XP 배수 | ×1.00 | 초반 레벨 폭주 방지 |
| 120~600초 XP 배수 | ×1.34 | 중반 빌드 형성은 유지 |
| 600초 이후 XP 배수 | ×1.00 | 후반은 잔향/궁극 완성으로 템포 유지 |
| 초반 처치 XP 보너스 | 없음 | 레벨 120초 3.5~4.0 목표 |
| 문지기 스케줄 | 300 / 600 / 900 / 1140초 | 20분 목표 런의 4개 검문 |
| 하드 캡 | 1260초 | 최종 문지기 미돌파 시 실패 |
| 문지기 HP | 1900 / 2800 / 4000 / 5400 | 1궁극 완성 후 최종 처치 목표 |
| 클리어 조건 | 문지기 4체 처치 | 단순 생존 승리 제거 |
| 궁극 목표 | 1개 궁극 잔향 완성 | 2궁극은 숙련 목표 |

`scripts/balance_sim_v1.js`의 `20m_slow_start` 후보가 현재 코드 반영 기준이다. 4개 궁극 루트와 2개 무기 조합의 40회 반복 시뮬레이션에서 평균 첫 선택 24~28초, 첫 망각 323~329초, 궁극 완성 936~945초, 클리어 1178~1188초가 나왔다. 대검 루트의 순수 시뮬레이션 클리어율은 0.63~0.68로 쌍검보다 낮아 다음 MCP/실플레이에서 우선 확인한다.

## 경험치 / 레벨업 곡선

아래 표는 HTML v0.12 원본 기준이다. Unity 베타 20분 런에서는 위의 1차 적용값이 우선한다.

출처: `balance.runGrowth` + `queueLevelUp()`.

| 항목 | 값 |
| --- | --- |
| 시작 필요 XP | 5 |
| 보스 전 XP 획득 배수 | ×1.95 |
| 초반 곡선 적용 레벨 | < 10레벨 |
| 초반 nextXp 배수 / 가산 | ×1.24 / +3 |
| 후반 nextXp 배수 / 가산 | ×1.42 / +4 |

레벨업 시 다음 필요 XP:
```text
nextXp = round(prevNextXp * mul + add)
  level < 10 : mul=1.24, add=3
  level ≥ 10 : mul=1.42, add=4
```
예: 5 → 9 → 14 → 20 → 28 → 38 → ... (초반), 이후 1.42 곡선으로 가팔라짐.

## 레벨업 선택 생성 규칙

출처: `chooseLevelUpChoices()`. 매 레벨업마다 3개 선택지 제시.

- 활성 슬롯이 비었으면(<3) **새 기억 후보 2개** 추가(미보유 우선).
- **기억 강화 후보** 추가(슬롯 꽉 차면 2개, 아니면 1개, 낮은 레벨 우선).
- **런 스탯 후보 3개** 추가(미획득 우선).
- 위를 섞어 3개로 자른다.
- HP가 72% 미만이고 생존 스탯 미획득이면 생존 선택지를 강제 1개 보장.

## 런 스탯 6종

출처: `levelUpChoices`.

| ID | 이름 | 효과 |
| --- | --- | --- |
| `attack_speed` | 칼날 가속 | 공속 +11%, 기억 쿨감 +4% |
| `damage` | 검은 물의 힘 | 피해 +14% |
| `area` | 파문 확장 | 범위 +12%, 넉백 +8% |
| `survival` | 가라앉지 않는 숨 | 최대 HP +16, 즉시 회복 +28, 피해 감소 +5% |
| `magnet` | 기억 흡입 | 경험치 획득 +16% |
| `echo_amp` | 잔향 증폭 | 잔향 효과 +20% |

기억 강화 선택: 해당 활성 기억 레벨 +1(최대 5). 새 기억 선택: 레벨 1로 슬롯 추가.

## 플레이어 / 적 / 보스 핵심 수치

| 항목 | 값 | 출처 |
| --- | --- | --- |
| 플레이어 최대 HP | 210 | `balance.player.maxHp` |
| 플레이어 속도 | 184 | `balance.player.speed` |
| 첫 보스 HP | HTML 원본 2050 / Unity 베타 1900 | `balance.boss.firstBossHp`, `V1GameManager.FirstBossHp` |
| 이후 보스 HP | HTML 원본 round(560×(1+0.18×(idx-2))) / Unity 베타 2800, 4000, 5400 | `experiment.bossHp`, `V1GameManager.GatekeeperHp()` |
| 첫 보스 TTK 목표(중앙값) | 약 18~24초 | 밸런스 루프 근거 |

적 스탯·스케일링은 [02_COMBAT](LETHE_DESIGN_02_COMBAT.md) 참조.

## 초반 생존 보정

| 항목 | 값 |
| --- | --- |
| 시작 받는 피해 배수 | ×0.24 |
| 완전 grace 구간 | 0~12초 |
| 정상 피해 도달 | 320초 |

## 굶주린 칼무리 (활성 기억 대표 수치)

출처: `balance.hungryBlades`.

| 항목 | 값 |
| --- | --- |
| DPS | 28 |
| 반경 | 72 |
| 대상 소프트캡 | 4 |
| 초과 대상 피해 배수 | 0.55 |

## 피의 늪 (무기 진화)

출처: `balance.bloodMarsh`.

| 항목 | 값 |
| --- | --- |
| 쌍검 발동 확률 | 0.30 |
| 대검 발동 확률 | 0.20 |
| 최대 웅덩이 | 5 |
| 지속 | 2.1초 |
| 틱 간격 | 0.48초 |
| 기본 피해 | 5 + 무기피해×0.08 |
| 둔화 | 0.5초 |

## 전 상수표 (balance 객체 전체)

| 경로 | 값 |
| --- | --- |
| `player.maxHp` | 210 |
| `player.speed` | 184 |
| `boss.firstBossHp` | HTML 원본 2050 / Unity 베타 1900 |
| `hungryBlades.dps` | 28 |
| `hungryBlades.radius` | 72 |
| `hungryBlades.targetSoftCap` | 4 |
| `hungryBlades.overflowDamageMul` | 0.55 |
| `enemyScaling.hpTimePerMinute` | 0.12 |
| `enemyScaling.hpLevelPerLevel` | 0.03 |
| `enemyScaling.damageTimePerMinute` | 0.025 |
| `enemyScaling.damageLevelPerLevel` | 0.008 |
| `enemyScaling.damageCap` | 2.2 |
| `runGrowth.initialNextXp` | HTML 원본 5 / Unity 베타 7 |
| `runGrowth.preBossXpMul` | HTML 원본 1.95 / Unity 베타 0~120s ×1.00, 120~600s ×1.34 |
| `runGrowth.earlyCurveUntilLevel` | 10 |
| `runGrowth.earlyNextXpMul` | 1.24 |
| `runGrowth.earlyNextXpAdd` | 3 |
| `runGrowth.lateNextXpMul` | 1.42 |
| `runGrowth.lateNextXpAdd` | 4 |
| `earlySurvival.initialDamageMul` | 0.24 |
| `earlySurvival.fullGraceSec` | 12 |
| `earlySurvival.rampEndSec` | 320 |
| `spawnCaps.firstCycleLull` | 34 |
| `spawnCaps.firstCycleRising` | 34 |
| `spawnCaps.firstCycleClimax` | 32 |
| `spawnCaps.firstCycleGateBreath` | 22 |
| `spawnCaps.deficitBreath` | 16 |
| `spawnCaps.deficitTrial` | 14 |
| `spawnCaps.laterCycleClimax` | 46 |
| `spawnCaps.default` | 46 |
| `bloodMarsh.twinBladesProc` | 0.30 |
| `bloodMarsh.greatswordProc` | 0.20 |
| `bloodMarsh.maxPools` | 5 |
| `bloodMarsh.durationSec` | 2.1 |
| `bloodMarsh.tickSec` | 0.48 |
| `bloodMarsh.baseDamage` | 5 |
| `bloodMarsh.weaponDamageMul` | 0.08 |
| `bloodMarsh.slowSec` | 0.5 |
| `telemetrySampleSec` | 5 |
| `tacticalFocusForgetWeight` | 3 |

## 밸런스 검증 목표 (스모크)

60~120초 스모크에서 측정: kill count, player HP, weapon id, 활성 기억 id/레벨, 잔향 id/레벨, 궁극 unlock, death 여부, 최다 피해 memory/echo. 목표: 기억 없는 구간이 너무 길지 않음 / 기억 획득 후 사냥 방식이 달라짐 / 망각 후 잔향이 새 방식으로 남음 / 궁극이 강하지만 화면 삭제만 하지 않음.

HTML 기준 합격 예: 첫 보스 클리어 100%, 풀 클리어 60%, 사망 40%, 첫 보스 TTK 중앙값 ≈ 20초.
