# 04. 성장 / 밸런스 / 전 상수표

최종 갱신: 2026-06-12 · 출처: `src/game.js` (`balance`, `levelUpChoices`, `queueLevelUp`, `chooseLevelUpChoices`) · `balance.version = "v0.12-balance-1"`

이 문서는 Unity 이식 시 그대로 옮길 검증된 수치 모음이다. 임의로 재유도하지 말 것.

## 경험치 / 레벨업 곡선

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
| 첫 보스 HP | 2050 | `balance.boss.firstBossHp` |
| 이후 보스 HP | round(560×(1+0.18×(idx-2))) | `experiment.bossHp` |
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
| `boss.firstBossHp` | 2050 |
| `hungryBlades.dps` | 28 |
| `hungryBlades.radius` | 72 |
| `hungryBlades.targetSoftCap` | 4 |
| `hungryBlades.overflowDamageMul` | 0.55 |
| `enemyScaling.hpTimePerMinute` | 0.12 |
| `enemyScaling.hpLevelPerLevel` | 0.03 |
| `enemyScaling.damageTimePerMinute` | 0.025 |
| `enemyScaling.damageLevelPerLevel` | 0.008 |
| `enemyScaling.damageCap` | 2.2 |
| `runGrowth.initialNextXp` | 5 |
| `runGrowth.preBossXpMul` | 1.95 |
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
