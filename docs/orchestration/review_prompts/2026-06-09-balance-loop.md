# LETHE v0.12 Balance Loop - 2026-06-09

## 목적

감정 proxy와 Alpha Fun Score를 쓰지 않고 v0.12 telemetry 기반으로 다음 밸런스 조정만 판단한다.

## 현재 판정

- Verdict: `GO_BALANCE_BASELINE`
- Runs: `5`
- First boss clear rate: `100.0%`
- Full clear rate: `60.0%`
- Death rate: `40.0%`
- First boss TTK median: `20.18s`
- Level-ups before first boss median: `10`
- Slots filled at median: `21.43s`
- Top DPS share median: `38.7%`

## 실패한 밸런스 체크

- 없음

## Codex 다음 구현 지시

- `docs/BALANCE_TABLE_v0_12.md`와 `docs/LETHE_v0.12_밸런스_개선_제안서.md`를 기준으로 가장 작은 밸런스 조정 1개만 선택한다.
- 감정선, regret, irritation, Alpha Fun Score는 이번 판단에서 제외한다.
- 우선순위는 첫 보스 TTK, 첫 180초 레벨업 수, 3슬롯 완성 시각, top DPS share, clear/death rate 순서다.
- 새 기억, 새 무기, 상점, 메타 진행, 새 지역, 최종 보스 확장은 금지한다.
- 변경 후 `npm run qa:balance` 또는 환경 blocker 기록을 남긴다.

## 원본 summary

```json
{
  "generatedAt": "2026-06-09T01:24:34.296Z",
  "version": "v0.12-balance-loop-1",
  "verdict": "GO_BALANCE_BASELINE",
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
    "clearRate": 0.6,
    "deathRate": 0.4,
    "deathAtMean": 279.795,
    "deathAtMedian": 279.795,
    "firstBossClearRate": 1,
    "firstBossTtkMean": 27.972,
    "firstBossTtkMedian": 20.18,
    "levelUpsBeforeFirstBossMean": 9.8,
    "levelUpsBeforeFirstBossMedian": 10,
    "slotsFilledAtMean": 27.008,
    "slotsFilledAtMedian": 21.43,
    "topDpsShareMean": 0.36824,
    "topDpsShareMedian": 0.3869,
    "maxEnemiesMean": 48.6,
    "maxEnemiesMedian": 49,
    "hp60AtMedian": 127.17,
    "hp40AtMedian": 166.49,
    "hp20AtMedian": 287.53,
    "deathPhaseCounts": {
      "압박 상승": 1,
      "망각 전조": 1
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
      "value": 20.18,
      "target": ">= 15s"
    },
    {
      "name": "first boss TTK upper bound",
      "pass": true,
      "value": 20.18,
      "target": "<= 30s"
    },
    {
      "name": "clear rate minimum",
      "pass": true,
      "value": 0.6,
      "target": ">= 0.35"
    },
    {
      "name": "clear rate maximum",
      "pass": true,
      "value": 0.6,
      "target": "<= 0.8"
    },
    {
      "name": "death rate maximum",
      "pass": true,
      "value": 0.4,
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
      "value": 21.43,
      "target": "<= 150"
    },
    {
      "name": "top DPS share",
      "pass": true,
      "value": 0.3869,
      "target": "<= 0.5"
    }
  ],
  "failed": [],
  "runs": [
    {
      "runNumber": 1,
      "status": "complete",
      "elapsed": 624.5,
      "runResult": "clear",
      "finalClear": true,
      "death": false,
      "deathAt": null,
      "deathPhase": null,
      "deathEnemyCount": null,
      "maxEnemies": 49,
      "hp60At": null,
      "hp40At": null,
      "hp20At": null,
      "firstBossCleared": true,
      "firstBossTtk": 16.36,
      "firstBossFocusedDps": 125.31,
      "level": 12,
      "levelUpsBeforeFirstBoss": 10,
      "slotsFilledAt": 20.87,
      "activeMemoryCount": 3,
      "topDpsSource": "stalker_oath",
      "topDps": 21.64,
      "topDpsShare": 0.2924,
      "dpsBySource": {
        "weapon": 12.19,
        "hungry_blades": 10.75,
        "stalker_oath": 21.64,
        "weapon_echo": 4.94,
        "stopped_second": 9.44,
        "ashen_guard": 9.65,
        "execution_flash": 4.42,
        "oblivion_brand": 0.98
      },
      "bossFights": [
        {
          "cycleIndex": 1,
          "bossName": "작은 문지기",
          "spawnedAt": 180.02,
          "maxHp": 2050,
          "defeatedAt": 196.38,
          "ttk": 16.36,
          "damage": 2050,
          "damageBySource": {
            "hungry_blades": 535.85,
            "weapon": 516.75,
            "stalker_oath": 997.4
          },
          "remainingHp": 0,
          "focusedDps": 125.31
        },
        {
          "cycleIndex": 2,
          "bossName": "기억을 씹는 자 2",
          "spawnedAt": 340.01,
          "maxHp": 560,
          "defeatedAt": 348.81,
          "ttk": 8.8,
          "damage": 560,
          "damageBySource": {
            "weapon": 120,
            "weapon_echo": 73.47,
            "stalker_oath": 366.53
          },
          "remainingHp": 0,
          "focusedDps": 63.64
        },
        {
          "cycleIndex": 3,
          "bossName": "기억을 씹는 자 3",
          "spawnedAt": 490.01,
          "maxHp": 661,
          "defeatedAt": 511.99,
          "ttk": 21.98,
          "damage": 661,
          "damageBySource": {
            "execution_flash": 559.36,
            "weapon": 64.9,
            "weapon_echo": 36.74
          },
          "remainingHp": 0,
          "focusedDps": 30.07
        },
        {
          "cycleIndex": 4,
          "bossName": "끝의 문지기",
          "spawnedAt": 600.01,
          "maxHp": 762,
          "defeatedAt": 624.5,
          "ttk": 24.49,
          "damage": 762,
          "damageBySource": {
            "oblivion_brand": 292.34,
            "weapon": 280.17,
            "weapon_echo": 154.47,
            "stopped_second": 35.02
          },
          "remainingHp": 0,
          "focusedDps": 31.11
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
          "at": 20.9,
          "nextBossIndex": 1,
          "intensity": 0.42
        },
        {
          "id": "rising",
          "label": "압박 상승",
          "at": 43.23,
          "nextBossIndex": 1,
          "intensity": 0.7
        },
        {
          "id": "climax",
          "label": "망각 전조",
          "at": 126.02,
          "nextBossIndex": 1,
          "intensity": 0.78
        },
        {
          "id": "gate_breath",
          "label": "문지기 호흡",
          "at": 169.2,
          "nextBossIndex": 1,
          "intensity": 0.56
        },
        {
          "id": "deficit_breath",
          "label": "결손 정비",
          "at": 196.42,
          "nextBossIndex": 2,
          "intensity": 0.48
        },
        {
          "id": "deficit_trial",
          "label": "결손 압박",
          "at": 212.6,
          "nextBossIndex": 2,
          "intensity": 0.82
        },
        {
          "id": "rising",
          "label": "압박 상승",
          "at": 250.43,
          "nextBossIndex": 2,
          "intensity": 0.7
        },
        {
          "id": "climax",
          "label": "망각 전조",
          "at": 292.03,
          "nextBossIndex": 2,
          "intensity": 0.92
        },
        {
          "id": "deficit_breath",
          "label": "결손 정비",
          "at": 348.84,
          "nextBossIndex": 3,
          "intensity": 0.48
        },
        {
          "id": "deficit_trial",
          "label": "결손 압박",
          "at": 365.04,
          "nextBossIndex": 3,
          "intensity": 0.82
        },
        {
          "id": "rising",
          "label": "압박 상승",
          "at": 402.85,
          "nextBossIndex": 3,
          "intensity": 0.7
        },
        {
          "id": "climax",
          "label": "망각 전조",
          "at": 445.02,
          "nextBossIndex": 3,
          "intensity": 0.92
        },
        {
          "id": "deficit_breath",
          "label": "결손 정비",
          "at": 512.03,
          "nextBossIndex": 4,
          "intensity": 0.48
        },
        {
          "id": "deficit_trial",
          "label": "결손 압박",
          "at": 528.21,
          "nextBossIndex": 4,
          "intensity": 0.82
        },
        {
          "id": "rising",
          "label": "압박 상승",
          "at": 566.04,
          "nextBossIndex": 4,
          "intensity": 0.7
        },
        {
          "id": "climax",
          "label": "망각 전조",
          "at": 567.02,
          "nextBossIndex": 4,
          "intensity": 0.92
        }
      ],
      "hpSamples": [
        {
          "t": 3.02,
          "hp": 190,
          "hpRate": 1,
          "enemiesAlive": 10,
          "projectilesAlive": 2,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 6.03,
          "hp": 190,
          "hpRate": 1,
          "enemiesAlive": 16,
          "projectilesAlive": 2,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 9.05,
          "hp": 190,
          "hpRate": 1,
          "enemiesAlive": 16,
          "projectilesAlive": 4,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 12.07,
          "hp": 190,
          "hpRate": 1,
          "enemiesAlive": 17,
          "projectilesAlive": 7,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 15.08,
          "hp": 190,
          "hpRate": 1,
          "enemiesAlive": 16,
          "projectilesAlive": 3,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 18.1,
          "hp": 190,
          "hpRate": 1,
          "enemiesAlive": 16,
          "projectilesAlive": 0,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 21.13,
          "hp": 190,
          "hpRate": 1,
          "enemiesAlive": 16,
          "projectilesAlive": 0,
          "pressurePhase": "lull",
          "bossActive": false
        },
        {
          "t": 24.17,
          "hp": 190,
          "hpRate": 1,
          "enemiesAlive": 18,
          "projectilesAlive": 3,
          "pressurePhase": "lull",
          "bossActive": false
        },
        {
          "t": 27.2,
          "hp": 190,
          "hpRate": 1,
          "enemiesAlive": 21,
          "projectilesAlive": 9,
          "pressurePhase": "lull",
          "bossActive": false
        },
        {
          "t": 30.23,
          "hp": 190,
          "hpRate": 1,
          "enemiesAlive": 30,
          "projectilesAlive": 6,
          "pressurePhase": "lull",
          "bossActive": false
        },
        {
          "t": 33.25,
          "hp": 190,
          "hpRate": 1,
          "enemiesAlive": 33,
          "projectilesAlive": 7,
          "pressurePhase": "lull",
          "bossActive": false
        },
        {
          "t": 36.28,
          "hp": 190,
          "hpRate": 1,
          "enemiesAlive": 33,
          "projectilesAlive": 10,
          "pressurePhase": "lull",
          "bossActive": false
        },
        {
          "t": 39.31,
          "hp": 190,
          "hpRate": 1,
          "enemiesAlive": 34,
          "projectilesAlive": 10,
          "pressurePhase": "lull",
          "bossActive": false
        },
        {
          "t": 42.33,
          "hp": 190,
          "hpRate": 1,
          "enemiesAlive": 34,
          "projectilesAlive": 15,
          "pressurePhase": "lull",
          "bossActive": false
        },
        {
          "t": 45.36,
          "hp": 190,
          "hpRate": 1,
          "enemiesAlive": 30,
          "projectilesAlive": 15,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 48.4,
          "hp": 190,
          "hpRate": 1,
          "enemiesAlive": 32,
          "projectilesAlive": 9,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 51.43,
          "hp": 190,
          "hpRate": 1,
          "enemiesAlive": 33,
          "projectilesAlive": 10,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 54.46,
          "hp": 190,
          "hpRate": 1,
          "enemiesAlive": 34,
          "projectilesAlive": 16,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 57.5,
          "hp": 190,
          "hpRate": 1,
          "enemiesAlive": 33,
          "projectilesAlive": 6,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 60.53,
          "hp": 190,
          "hpRate": 1,
          "enemiesAlive": 33,
  
```
