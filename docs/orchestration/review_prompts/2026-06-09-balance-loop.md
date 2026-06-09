# LETHE v0.12 Balance Loop - 2026-06-09

## 목적

감정 proxy와 Alpha Fun Score를 쓰지 않고 v0.12 telemetry 기반으로 다음 밸런스 조정만 판단한다.

## 현재 판정

- Verdict: `ITERATE_BALANCE`
- Runs: `5`
- First boss clear rate: `100.0%`
- Full clear rate: `20.0%`
- Death rate: `80.0%`
- First boss TTK median: `23.1s`
- Level-ups before first boss median: `10`
- Slots filled at median: `32s`
- Top DPS share median: `37.0%`

## 실패한 밸런스 체크

- clear rate minimum: value `0.2`, target `>= 0.35`
- death rate maximum: value `0.8`, target `<= 0.4`

## Codex 다음 구현 지시

- `docs/BALANCE_TABLE_v0_12.md`와 `docs/LETHE_v0.12_밸런스_개선_제안서.md`를 기준으로 가장 작은 밸런스 조정 1개만 선택한다.
- 감정선, regret, irritation, Alpha Fun Score는 이번 판단에서 제외한다.
- 우선순위는 첫 보스 TTK, 첫 180초 레벨업 수, 3슬롯 완성 시각, top DPS share, clear/death rate 순서다.
- 새 기억, 새 무기, 상점, 메타 진행, 새 지역, 최종 보스 확장은 금지한다.
- 변경 후 `npm run qa:balance` 또는 환경 blocker 기록을 남긴다.

## 원본 summary

```json
{
  "generatedAt": "2026-06-09T01:01:08.093Z",
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
    "deathRate": 0.8,
    "deathAtMean": 266.045,
    "deathAtMedian": 262.48,
    "firstBossClearRate": 1,
    "firstBossTtkMean": 28.224,
    "firstBossTtkMedian": 23.1,
    "levelUpsBeforeFirstBossMean": 10,
    "levelUpsBeforeFirstBossMedian": 10,
    "slotsFilledAtMean": 31.636000000000003,
    "slotsFilledAtMedian": 32,
    "topDpsShareMean": 0.4024,
    "topDpsShareMedian": 0.37,
    "maxEnemiesMean": 42.8,
    "maxEnemiesMedian": 48,
    "hp60AtMedian": 113.475,
    "hp40AtMedian": 166.49,
    "hp20AtMedian": 234.55,
    "deathPhaseCounts": {
      "망각 전조": 2,
      "결손 압박": 1,
      "압박 상승": 1
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
      "value": 23.1,
      "target": ">= 15s"
    },
    {
      "name": "first boss TTK upper bound",
      "pass": true,
      "value": 23.1,
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
      "value": 0.8,
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
      "value": 32,
      "target": "<= 150"
    },
    {
      "name": "top DPS share",
      "pass": true,
      "value": 0.37,
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
      "value": 0.8,
      "target": "<= 0.4"
    }
  ],
  "runs": [
    {
      "runNumber": 1,
      "status": "complete",
      "elapsed": 299.36,
      "runResult": "death",
      "finalClear": false,
      "death": true,
      "deathAt": 299.36,
      "deathPhase": "망각 전조",
      "deathEnemyCount": 44,
      "maxEnemies": 48,
      "hp60At": 121.01,
      "hp40At": 175.51,
      "hp20At": 293.5,
      "firstBossCleared": true,
      "firstBossTtk": 15.02,
      "firstBossFocusedDps": 136.48,
      "level": 12,
      "levelUpsBeforeFirstBoss": 10,
      "slotsFilledAt": 32,
      "activeMemoryCount": 3,
      "topDpsSource": "hungry_blades",
      "topDps": 30.73,
      "topDpsShare": 0.37,
      "dpsBySource": {
        "hungry_blades": 30.73,
        "weapon": 12.07,
        "execution_flash": 13.97,
        "shatter_ripple": 23.79,
        "stalker_oath": 2.05,
        "weapon_echo": 0.45
      },
      "bossFights": [
        {
          "cycleIndex": 1,
          "bossName": "작은 문지기",
          "spawnedAt": 180.02,
          "maxHp": 2050,
          "defeatedAt": 195.04,
          "ttk": 15.02,
          "damage": 2050,
          "damageBySource": {
            "hungry_blades": 487.57,
            "weapon": 501.75,
            "shatter_ripple": 277.38,
            "execution_flash": 783.3
          },
          "remainingHp": 0,
          "focusedDps": 136.48
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
          "at": 32.03,
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
          "at": 126.03,
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
          "at": 195.07,
          "nextBossIndex": 2,
          "intensity": 0.48
        },
        {
          "id": "deficit_trial",
          "label": "결손 압박",
          "at": 211.25,
          "nextBossIndex": 2,
          "intensity": 0.82
        },
        {
          "id": "rising",
          "label": "압박 상승",
          "at": 249.09,
          "nextBossIndex": 2,
          "intensity": 0.7
        },
        {
          "id": "climax",
          "label": "망각 전조",
          "at": 292.03,
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
          "projectilesAlive": 3,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 12.07,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 17,
          "projectilesAlive": 3,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 15.08,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 17,
          "projectilesAlive": 3,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 18.1,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 16,
          "projectilesAlive": 2,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 21.12,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 16,
          "projectilesAlive": 2,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 24.13,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 16,
          "projectilesAlive": 3,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 27.18,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 16,
          "projectilesAlive": 0,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 30.2,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 15,
          "projectilesAlive": 2,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 33.2,
          "hp": 176.8,
          "hpRate": 0.982,
          "enemiesAlive": 12,
          "projectilesAlive": 5,
          "pressurePhase": "lull",
          "bossActive": false
        },
        {
          "t": 36.22,
          "hp": 175.4,
          "hpRate": 0.974,
          "enemiesAlive": 15,
          "projectilesAlive": 7,
          "pressurePhase": "lull",
          "bossActive": false
        },
        {
          "t": 39.25,
          "hp": 175.4,
          "hpRate": 0.974,
          "enemiesAlive": 23,
          "projectilesAlive": 8,
          "pressurePhase": "lull",
          "bossActive": false
        },
        {
          "t": 42.28,
          "hp": 175.4,
          "hpRate": 0.974,
          "enemiesAlive": 30,
          "projectilesAlive": 11,
          "pressurePhase": "lull",
          "bossActive": false
        },
        {
          "t": 45.32,
          "hp": 173.8,
          "hpRate": 0.965,
          "enemiesAlive": 31,
          "projectilesAlive": 15,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 48.33,
          "hp": 170,
          "hpRate": 0.945,
          "enemiesAlive": 31,
          "projectilesAlive": 13,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 51.37,
          "hp": 164.8,
          "hpRate": 0.916,
          "enemiesAlive": 33,
          "projectilesAlive": 13,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 54.4,
          "hp": 159.5,
          "hpRate": 0.886,
          "enemiesAlive": 33,
          "projectilesAlive": 13,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 57.41,
          "hp": 154.1,
          "hpRate": 0.856,
          "enemiesAlive": 31,
          "projectilesAlive": 11,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 60.45,
          "hp": 146.6,
          "hpRate": 0.815,
          "enemiesAlive": 33,
          "projectilesAlive": 13,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 63.48,
          "hp": 140.9,
          "hpRate": 0.783,
          "enemiesAlive": 31,
          "projectilesAlive": 20,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 66.51,
          "hp": 164.7,
          "hpRate": 0.84,
          "enemiesAlive": 31,
          "projectilesAlive": 14,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 69.53,
          "hp": 162.8,
          "hpRate": 0.831,
          "enemiesAlive": 34,
          "projectilesAlive": 6,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 72.56,
          "hp": 160.4,
          "hpRate": 0.819,
          "enemiesAlive": 31,
          "projectilesAlive": 1,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 75.6,
          "hp": 160.4,
          "hpRate": 0.819,
          "enemiesAlive": 33,
          "projectilesAlive": 0,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 78.63,
          "hp": 160.4,
          "hpRate": 0.819,
          "enemiesAlive": 32,
          "projectilesAlive": 1,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 81.65,
          "hp": 160.4,
          "hpRate": 0.819,
          "enemiesAlive": 33,
          "projectilesAlive": 5,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 84.68,
          "hp": 158.3,
          "hpRate": 0.808,
          "enemiesAlive": 30,
          "projectilesAlive": 14,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 87.71,
          "hp": 158.3,
          "hpRate": 0.808,
          "enemiesAlive": 31,
          "projectilesAlive": 24,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 90.73,
          "hp": 158.3,
          "hpRate": 0.808,
          "enemiesAlive": 35,
          "
```
