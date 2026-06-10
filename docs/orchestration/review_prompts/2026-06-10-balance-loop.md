# LETHE v0.12 Balance Loop - 2026-06-10

## 목적

감정 proxy와 Alpha Fun Score를 쓰지 않고 v0.12 telemetry 기반으로 다음 밸런스 조정만 판단한다.

## 현재 판정

- Verdict: `GO_BALANCE_BASELINE`
- Runs: `5`
- First boss clear rate: `80.0%`
- Full clear rate: `60.0%`
- Death rate: `40.0%`
- First boss TTK median: `23.91s`
- Level-ups before first boss median: `10`
- Slots filled at median: `23.12s`
- Top DPS share median: `37.5%`

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
  "generatedAt": "2026-06-10T02:51:55.859Z",
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
    "deathAtMean": 244.055,
    "deathAtMedian": 244.055,
    "firstBossClearRate": 0.8,
    "firstBossTtkMean": 26.0625,
    "firstBossTtkMedian": 23.915,
    "levelUpsBeforeFirstBossMean": 10,
    "levelUpsBeforeFirstBossMedian": 10,
    "slotsFilledAtMean": 29.165999999999997,
    "slotsFilledAtMedian": 23.12,
    "topDpsShareMean": 0.35946000000000006,
    "topDpsShareMedian": 0.3752,
    "maxEnemiesMean": 46.2,
    "maxEnemiesMedian": 49,
    "hp60AtMedian": 154.36,
    "hp40AtMedian": 199.76,
    "hp20AtMedian": 217.9,
    "deathPhaseCounts": {
      "첫 망각 문지기": 1,
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
      "value": 0.8,
      "target": ">= 0.7"
    },
    {
      "name": "first boss TTK lower bound",
      "pass": true,
      "value": 23.915,
      "target": ">= 15s"
    },
    {
      "name": "first boss TTK upper bound",
      "pass": true,
      "value": 23.915,
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
      "value": 23.12,
      "target": "<= 150"
    },
    {
      "name": "top DPS share",
      "pass": true,
      "value": 0.3752,
      "target": "<= 0.5"
    }
  ],
  "failed": [],
  "runs": [
    {
      "runNumber": 1,
      "status": "complete",
      "elapsed": 607.18,
      "runResult": "clear",
      "finalClear": true,
      "death": false,
      "deathAt": null,
      "deathPhase": null,
      "deathEnemyCount": null,
      "maxEnemies": 49,
      "hp60At": 453.92,
      "hp40At": null,
      "hp20At": null,
      "firstBossCleared": true,
      "firstBossTtk": 20.75,
      "firstBossFocusedDps": 98.8,
      "level": 12,
      "levelUpsBeforeFirstBoss": 9,
      "slotsFilledAt": 53.02,
      "activeMemoryCount": 3,
      "topDpsSource": "shatter_ripple",
      "topDps": 35.83,
      "topDpsShare": 0.3752,
      "dpsBySource": {
        "weapon": 12.51,
        "hungry_blades": 8.23,
        "blood_reflection": 5.29,
        "stalker_oath": 20.09,
        "shatter_ripple": 35.83,
        "weapon_echo": 5.5,
        "execution_flash": 8.05
      },
      "bossFights": [
        {
          "cycleIndex": 1,
          "bossName": "작은 문지기",
          "spawnedAt": 180.02,
          "maxHp": 2050,
          "defeatedAt": 200.77,
          "ttk": 20.75,
          "damage": 2050,
          "damageBySource": {
            "stalker_oath": 1002.63,
            "hungry_blades": 341.17,
            "weapon": 330,
            "blood_reflection": 376.2
          },
          "remainingHp": 0,
          "focusedDps": 98.8
        },
        {
          "cycleIndex": 2,
          "bossName": "기억을 씹는 자 2",
          "spawnedAt": 340.03,
          "maxHp": 560,
          "defeatedAt": 351.99,
          "ttk": 11.96,
          "damage": 560,
          "damageBySource": {
            "weapon": 163.91,
            "weapon_echo": 91.84,
            "blood_reflection": 75.24,
            "shatter_ripple": 69.34,
            "stalker_oath": 159.67
          },
          "remainingHp": 0,
          "focusedDps": 46.82
        },
        {
          "cycleIndex": 3,
          "bossName": "기억을 씹는 자 3",
          "spawnedAt": 490.02,
          "maxHp": 661,
          "defeatedAt": 501.93,
          "ttk": 11.91,
          "damage": 661,
          "damageBySource": {
            "execution_flash": 322.24,
            "weapon": 135,
            "weapon_echo": 78.36,
            "blood_reflection": 125.4
          },
          "remainingHp": 0,
          "focusedDps": 55.5
        },
        {
          "cycleIndex": 4,
          "bossName": "끝의 문지기",
          "spawnedAt": 600.01,
          "maxHp": 762,
          "defeatedAt": 607.18,
          "ttk": 7.17,
          "damage": 762,
          "damageBySource": {
            "execution_flash": 237.12,
            "weapon": 180,
            "weapon_echo": 131.68,
            "blood_reflection": 141.63,
            "shatter_ripple": 71.57
          },
          "remainingHp": 0,
          "focusedDps": 106.28
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
          "id": "rising",
          "label": "압박 상승",
          "at": 53.05,
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
          "at": 200.81,
          "nextBossIndex": 2,
          "intensity": 0.48
        },
        {
          "id": "lull",
          "label": "숨 고르기",
          "at": 204.22,
          "nextBossIndex": 2,
          "intensity": 0.42
        },
        {
          "id": "rising",
          "label": "압박 상승",
          "at": 218.4,
          "nextBossIndex": 2,
          "intensity": 0.7
        },
        {
          "id": "climax",
          "label": "망각 전조",
          "at": 292.01,
          "nextBossIndex": 2,
          "intensity": 0.92
        },
        {
          "id": "deficit_breath",
          "label": "결손 정비",
          "at": 352.03,
          "nextBossIndex": 3,
          "intensity": 0.48
        },
        {
          "id": "deficit_trial",
          "label": "결손 압박",
          "at": 368.21,
          "nextBossIndex": 3,
          "intensity": 0.82
        },
        {
          "id": "rising",
          "label": "압박 상승",
          "at": 406.06,
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
          "at": 501.97,
          "nextBossIndex": 4,
          "intensity": 0.48
        },
        {
          "id": "deficit_trial",
          "label": "결손 압박",
          "at": 518.13,
          "nextBossIndex": 4,
          "intensity": 0.82
        },
        {
          "id": "rising",
          "label": "압박 상승",
          "at": 556,
          "nextBossIndex": 4,
          "intensity": 0.7
        },
        {
          "id": "climax",
          "label": "망각 전조",
          "at": 567.03,
          "nextBossIndex": 4,
          "intensity": 0.92
        }
      ],
      "hpSamples": [
        {
          "t": 3.02,
          "hp": 210,
          "hpRate": 1,
          "enemiesAlive": 10,
          "projectilesAlive": 0,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 6.03,
          "hp": 210,
          "hpRate": 1,
          "enemiesAlive": 16,
          "projectilesAlive": 4,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 9.05,
          "hp": 210,
          "hpRate": 1,
          "enemiesAlive": 16,
          "projectilesAlive": 7,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 12.07,
          "hp": 210,
          "hpRate": 1,
          "enemiesAlive": 16,
          "projectilesAlive": 5,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 15.08,
          "hp": 208.6,
          "hpRate": 0.993,
          "enemiesAlive": 17,
          "projectilesAlive": 3,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 18.1,
          "hp": 208.6,
          "hpRate": 0.993,
          "enemiesAlive": 17,
          "projectilesAlive": 2,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 21.12,
          "hp": 207.7,
          "hpRate": 0.989,
          "enemiesAlive": 16,
          "projectilesAlive": 1,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 24.13,
          "hp": 207.7,
          "hpRate": 0.989,
          "enemiesAlive": 16,
          "projectilesAlive": 5,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 27.17,
          "hp": 209.5,
          "hpRate": 0.998,
          "enemiesAlive": 16,
          "projectilesAlive": 4,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 30.18,
          "hp": 210,
          "hpRate": 1,
          "enemiesAlive": 17,
          "projectilesAlive": 4,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 33.2,
          "hp": 210,
          "hpRate": 1,
          "enemiesAlive": 17,
          "projectilesAlive": 5,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 36.23,
          "hp": 210,
          "hpRate": 1,
          "enemiesAlive": 17,
          "projectilesAlive": 5,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 39.25,
          "hp": 210,
          "hpRate": 1,
          "enemiesAlive": 17,
          "projectilesAlive": 12,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 42.27,
          "hp": 210,
          "hpRate": 1,
          "enemiesAlive": 17,
          "projectilesAlive": 4,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 45.3,
          "hp": 210,
          "hpRate": 1,
          "enemiesAlive": 17,
          "projectilesAlive": 7,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 48.32,
          "hp": 210,
          "hpRate": 1,
          "enemiesAlive": 17,
          "projectilesAlive": 7,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 51.33,
          "hp": 210,
          "hpRate": 1,
          "enemiesAlive": 17,
          "projectilesAlive": 14,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 54.35,
          "hp": 210,
          "hpRate": 1,
          "enemiesAlive": 15,
          "projectilesAlive": 9,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 57.37,
          "hp": 210,
          "hpRate": 1,
          "enemiesAlive": 21,
          "projectilesAlive": 11,
          "pressur
```
