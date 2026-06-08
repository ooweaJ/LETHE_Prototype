# LETHE v0.12 Balance Loop - 2026-06-08

## 목적

감정 proxy와 Alpha Fun Score를 쓰지 않고 v0.12 telemetry 기반으로 다음 밸런스 조정만 판단한다.

## 현재 판정

- Verdict: `ITERATE_BALANCE`
- Runs: `5`
- First boss clear rate: `100.0%`
- Full clear rate: `20.0%`
- Death rate: `60.0%`
- First boss TTK median: `26.42s`
- Level-ups before first boss median: `10`
- Slots filled at median: `26.68s`
- Top DPS share median: `43.7%`

## 실패한 밸런스 체크

- clear rate minimum: value `0.2`, target `>= 0.35`
- death rate maximum: value `0.6`, target `<= 0.4`

## Codex 다음 구현 지시

- `docs/BALANCE_TABLE_v0_12.md`와 `docs/LETHE_v0.12_밸런스_개선_제안서.md`를 기준으로 가장 작은 밸런스 조정 1개만 선택한다.
- 감정선, regret, irritation, Alpha Fun Score는 이번 판단에서 제외한다.
- 우선순위는 첫 보스 TTK, 첫 180초 레벨업 수, 3슬롯 완성 시각, top DPS share, clear/death rate 순서다.
- 새 기억, 새 무기, 상점, 메타 진행, 새 지역, 최종 보스 확장은 금지한다.
- 변경 후 `npm run qa:balance` 또는 환경 blocker 기록을 남긴다.

## 원본 summary

```json
{
  "generatedAt": "2026-06-08T09:27:05.062Z",
  "version": "v0.12-balance-loop-1",
  "verdict": "ITERATE_BALANCE",
  "targets": {
    "firstBossClearRateMin": 0.7,
    "browserSuccessRateMin": 0.8,
    "clearRateMin": 0.35,
    "clearRateMax": 0.8,
    "deathRateMax": 0.4,
    "firstBossTtkMin": 15,
    "firstBossTtkMax": 30,
    "levelUpsBeforeFirstBossMin": 8,
    "slotsFilledAtMax": 150,
    "topDpsShareMax": 0.5
  },
  "metrics": {
    "runs": 5,
    "gameplayRuns": 5,
    "clearRate": 0.2,
    "deathRate": 0.6,
    "deathAtMean": 370.2833333333333,
    "deathAtMedian": 322.88,
    "firstBossClearRate": 1,
    "firstBossTtkMean": 27.270000000000003,
    "firstBossTtkMedian": 26.42,
    "levelUpsBeforeFirstBossMean": 9.8,
    "levelUpsBeforeFirstBossMedian": 10,
    "slotsFilledAtMean": 25.458,
    "slotsFilledAtMedian": 26.68,
    "topDpsShareMean": 0.4282,
    "topDpsShareMedian": 0.4372,
    "maxEnemiesMean": 48.8,
    "maxEnemiesMedian": 49,
    "hp60AtMedian": 196.7,
    "hp40AtMedian": 305.65,
    "hp20AtMedian": 314.73,
    "deathPhaseCounts": {
      "망각 전조": 3
    }
  },
  "checks": [
    {
      "name": "browser run success rate",
      "pass": true,
      "value": 1,
      "target": ">= 0.8"
    },
    {
      "name": "first boss clear rate",
      "pass": true,
      "value": 1,
      "target": ">= 0.7"
    },
    {
      "name": "first boss TTK lower bound",
      "pass": true,
      "value": 26.42,
      "target": ">= 15s"
    },
    {
      "name": "first boss TTK upper bound",
      "pass": true,
      "value": 26.42,
      "target": "<= 30s"
    },
    {
      "name": "clear rate minimum",
      "pass": false,
      "value": 0.2,
      "target": ">= 0.35"
    },
    {
      "name": "clear rate maximum",
      "pass": true,
      "value": 0.2,
      "target": "<= 0.8"
    },
    {
      "name": "death rate maximum",
      "pass": false,
      "value": 0.6,
      "target": "<= 0.4"
    },
    {
      "name": "level-ups before first boss",
      "pass": true,
      "value": 10,
      "target": ">= 8"
    },
    {
      "name": "slot fill timing",
      "pass": true,
      "value": 26.68,
      "target": "<= 150"
    },
    {
      "name": "top DPS share",
      "pass": true,
      "value": 0.4372,
      "target": "<= 0.5"
    }
  ],
  "failed": [
    {
      "name": "clear rate minimum",
      "pass": false,
      "value": 0.2,
      "target": ">= 0.35"
    },
    {
      "name": "death rate maximum",
      "pass": false,
      "value": 0.6,
      "target": "<= 0.4"
    }
  ],
  "runs": [
    {
      "runNumber": 1,
      "status": "complete",
      "elapsed": 303.25,
      "runResult": "death",
      "finalClear": false,
      "death": true,
      "deathAt": 303.25,
      "deathPhase": "망각 전조",
      "deathEnemyCount": 46,
      "maxEnemies": 48,
      "hp60At": 163.46,
      "hp40At": 284.45,
      "hp20At": 296.58,
      "firstBossCleared": true,
      "firstBossTtk": 18.82,
      "firstBossFocusedDps": 108.93,
      "level": 12,
      "levelUpsBeforeFirstBoss": 10,
      "slotsFilledAt": 28.48,
      "activeMemoryCount": 3,
      "topDpsSource": "hungry_blades",
      "topDps": 32.18,
      "topDpsShare": 0.3502,
      "dpsBySource": {
        "weapon": 11.71,
        "hungry_blades": 32.18,
        "execution_flash": 25.13,
        "shatter_ripple": 21.88,
        "weapon_echo": 1
      },
      "bossFights": [
        {
          "cycleIndex": 1,
          "bossName": "작은 문지기",
          "spawnedAt": 180,
          "maxHp": 2050,
          "defeatedAt": 198.82,
          "ttk": 18.82,
          "damage": 2050,
          "damageBySource": {
            "hungry_blades": 444.22,
            "weapon": 463.5,
            "execution_flash": 826.64,
            "shatter_ripple": 315.64
          },
          "remainingHp": 0,
          "focusedDps": 108.93
        }
      ],
      "pressureSegments": [
        {
          "id": "deficit_breath",
          "label": "결손 정비",
          "at": 0.02,
          "nextBossIndex": 1,
          "intensity": 0.48
        },
        {
          "id": "lull",
          "label": "숨 고르기",
          "at": 28.52,
          "nextBossIndex": 1,
          "intensity": 0.42
        },
        {
          "id": "rising",
          "label": "압박 상승",
          "at": 43.22,
          "nextBossIndex": 1,
          "intensity": 0.7
        },
        {
          "id": "climax",
          "label": "망각 전조",
          "at": 126.01,
          "nextBossIndex": 1,
          "intensity": 0.78
        },
        {
          "id": "gate_breath",
          "label": "문지기 호흡",
          "at": 169.22,
          "nextBossIndex": 1,
          "intensity": 0.56
        },
        {
          "id": "deficit_breath",
          "label": "결손 정비",
          "at": 198.85,
          "nextBossIndex": 2,
          "intensity": 0.48
        },
        {
          "id": "deficit_trial",
          "label": "결손 압박",
          "at": 215.03,
          "nextBossIndex": 2,
          "intensity": 0.82
        },
        {
          "id": "rising",
          "label": "압박 상승",
          "at": 252.87,
          "nextBossIndex": 2,
          "intensity": 0.7
        },
        {
          "id": "climax",
          "label": "망각 전조",
          "at": 292.01,
          "nextBossIndex": 2,
          "intensity": 0.92
        }
      ],
      "hpSamples": [
        {
          "t": 3.02,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 10,
          "projectilesAlive": 0,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 6.03,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 16,
          "projectilesAlive": 0,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 9.05,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 16,
          "projectilesAlive": 4,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 12.07,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 16,
          "projectilesAlive": 5,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 15.08,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 16,
          "projectilesAlive": 4,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 18.1,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 16,
          "projectilesAlive": 4,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 21.12,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 16,
          "projectilesAlive": 6,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 24.13,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 16,
          "projectilesAlive": 5,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 27.17,
          "hp": 177.2,
          "hpRate": 0.985,
          "enemiesAlive": 16,
          "projectilesAlive": 7,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 30.18,
          "hp": 170.5,
          "hpRate": 0.947,
          "enemiesAlive": 14,
          "projectilesAlive": 11,
          "pressurePhase": "lull",
          "bossActive": false
        },
        {
          "t": 33.2,
          "hp": 170.5,
          "hpRate": 0.947,
          "enemiesAlive": 20,
          "projectilesAlive": 10,
          "pressurePhase": "lull",
          "bossActive": false
        },
        {
          "t": 36.23,
          "hp": 168.7,
          "hpRate": 0.937,
          "enemiesAlive": 25,
          "projectilesAlive": 14,
          "pressurePhase": "lull",
          "bossActive": false
        },
        {
          "t": 39.25,
          "hp": 168.7,
          "hpRate": 0.937,
          "enemiesAlive": 32,
          "projectilesAlive": 8,
          "pressurePhase": "lull",
          "bossActive": false
        },
        {
          "t": 42.28,
          "hp": 168.7,
          "hpRate": 0.937,
          "enemiesAlive": 35,
          "projectilesAlive": 4,
          "pressurePhase": "lull",
          "bossActive": false
        },
        {
          "t": 45.32,
          "hp": 168.7,
          "hpRate": 0.937,
          "enemiesAlive": 35,
          "projectilesAlive": 4,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 48.35,
          "hp": 167,
          "hpRate": 0.928,
          "enemiesAlive": 34,
          "projectilesAlive": 3,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 51.38,
          "hp": 165.3,
          "hpRate": 0.918,
          "enemiesAlive": 32,
          "projectilesAlive": 14,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 54.41,
          "hp": 165.3,
          "hpRate": 0.918,
          "enemiesAlive": 30,
          "projectilesAlive": 13,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 57.43,
          "hp": 165.3,
          "hpRate": 0.918,
          "enemiesAlive": 34,
          "projectilesAlive": 13,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 60.46,
          "hp": 165.3,
          "hpRate": 0.918,
          "enemiesAlive": 34,
          "projectilesAlive": 12,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 63.5,
          "hp": 165.3,
          "hpRate": 0.918,
          "enemiesAlive": 35,
          "projectilesAlive": 6,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 66.53,
          "hp": 163.3,
          "hpRate": 0.907,
          "enemiesAlive": 34,
          "projectilesAlive": 15,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 69.55,
          "hp": 159.4,
          "hpRate": 0.886,
          "enemiesAlive": 32,
          "projectilesAlive": 15,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 72.58,
          "hp": 159.4,
          "hpRate": 0.886,
          "enemiesAlive": 34,
          "projectilesAlive": 32,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 75.6,
          "hp": 159.4,
          "hpRate": 0.886,
          "enemiesAlive": 35,
          "projectilesAlive": 5,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 78.63,
          "hp": 159.4,
          "hpRate": 0.886,
          "enemiesAlive": 35,
          "projectilesAlive": 24,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 81.66,
          "hp": 159.4,
          "hpRate": 0.886,
          "enemiesAlive": 35,
          "projectilesAlive": 29,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 84.7,
          "hp": 159.4,
          "hpRate": 0.886,
          "enemiesAlive": 32,
          "projectilesAlive": 11,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 87.73,
          "hp": 159.4,
          "hpRate": 0.886,
          "enemiesAlive": 34,
          "projectilesAlive": 4,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 90.76,
          "hp": 159.4,
          "hpRate": 0.886,
          "enemiesAlive": 33,
          "projectilesAlive": 12,
          "press
```
