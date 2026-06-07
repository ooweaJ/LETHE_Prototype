# LETHE v0.12 Balance Loop - 2026-06-07

## 목적

감정 proxy와 Alpha Fun Score를 쓰지 않고 v0.12 telemetry 기반으로 다음 밸런스 조정만 판단한다.

## 현재 판정

- Verdict: `GO_BALANCE_BASELINE`
- Runs: `5`
- First boss clear rate: `100.0%`
- Full clear rate: `60.0%`
- Death rate: `40.0%`
- First boss TTK median: `20.73s`
- Level-ups before first boss median: `9`
- Slots filled at median: `27.85s`
- Top DPS share median: `39.3%`

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
  "generatedAt": "2026-06-07T10:59:58.954Z",
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
    "deathAtMean": 266.645,
    "deathAtMedian": 266.645,
    "firstBossClearRate": 1,
    "firstBossTtkMean": 22.234,
    "firstBossTtkMedian": 20.73,
    "levelUpsBeforeFirstBossMean": 9.4,
    "levelUpsBeforeFirstBossMedian": 9,
    "slotsFilledAtMean": 27.006000000000007,
    "slotsFilledAtMedian": 27.85,
    "topDpsShareMean": 0.39557999999999993,
    "topDpsShareMedian": 0.3932,
    "maxEnemiesMean": 46,
    "maxEnemiesMedian": 49,
    "hp60AtMedian": 108.86500000000001,
    "hp40AtMedian": 208.745,
    "hp20AtMedian": 199.615,
    "deathPhaseCounts": {
      "망각 전조": 1,
      "결손 압박": 1
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
      "value": 20.73,
      "target": ">= 15s"
    },
    {
      "name": "first boss TTK upper bound",
      "pass": true,
      "value": 20.73,
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
      "value": 9,
      "target": ">= 8"
    },
    {
      "name": "slot fill timing",
      "pass": true,
      "value": 27.85,
      "target": "<= 150"
    },
    {
      "name": "top DPS share",
      "pass": true,
      "value": 0.3932,
      "target": "<= 0.5"
    }
  ],
  "failed": [],
  "runs": [
    {
      "runNumber": 1,
      "status": "complete",
      "elapsed": 628.73,
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
      "firstBossTtk": 15.75,
      "firstBossFocusedDps": 130.16,
      "level": 12,
      "levelUpsBeforeFirstBoss": 9,
      "slotsFilledAt": 37.88,
      "activeMemoryCount": 3,
      "topDpsSource": "hungry_blades",
      "topDps": 27.24,
      "topDpsShare": 0.4117,
      "dpsBySource": {
        "weapon": 12.97,
        "hungry_blades": 27.24,
        "stalker_oath": 7.02,
        "blood_reflection": 7.32,
        "weapon_echo": 6.08,
        "oblivion_brand": 5.54
      },
      "bossFights": [
        {
          "cycleIndex": 1,
          "bossName": "작은 문지기",
          "spawnedAt": 180.02,
          "maxHp": 2050,
          "defeatedAt": 195.77,
          "ttk": 15.75,
          "damage": 2050,
          "damageBySource": {
            "hungry_blades": 406.55,
            "weapon": 463.5,
            "blood_reflection": 300.96,
            "stalker_oath": 878.99
          },
          "remainingHp": 0,
          "focusedDps": 130.16
        },
        {
          "cycleIndex": 2,
          "bossName": "기억을 씹는 자 2",
          "spawnedAt": 340.03,
          "maxHp": 560,
          "defeatedAt": 344.1,
          "ttk": 4.07,
          "damage": 560,
          "damageBySource": {
            "oblivion_brand": 51.48,
            "hungry_blades": 225.33,
            "weapon": 136.8,
            "blood_reflection": 146.39
          },
          "remainingHp": 0,
          "focusedDps": 137.59
        },
        {
          "cycleIndex": 3,
          "bossName": "기억을 씹는 자 3",
          "spawnedAt": 490,
          "maxHp": 661,
          "defeatedAt": 503.47,
          "ttk": 13.47,
          "damage": 661,
          "damageBySource": {
            "oblivion_brand": 148.93,
            "hungry_blades": 147.64,
            "weapon": 161.43,
            "weapon_echo": 93.21,
            "blood_reflection": 109.79
          },
          "remainingHp": 0,
          "focusedDps": 49.07
        },
        {
          "cycleIndex": 4,
          "bossName": "끝의 문지기",
          "spawnedAt": 600.02,
          "maxHp": 762,
          "defeatedAt": 628.73,
          "ttk": 28.71,
          "damage": 762,
          "damageBySource": {
            "oblivion_brand": 343.82,
            "weapon": 102.6,
            "weapon_echo": 117.25,
            "hungry_blades": 88.55,
            "blood_reflection": 109.79
          },
          "remainingHp": 0,
          "focusedDps": 26.54
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
          "at": 37.92,
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
          "at": 169.21,
          "nextBossIndex": 1,
          "intensity": 0.56
        },
        {
          "id": "deficit_breath",
          "label": "결손 정비",
          "at": 195.81,
          "nextBossIndex": 2,
          "intensity": 0.48
        },
        {
          "id": "deficit_trial",
          "label": "결손 압박",
          "at": 211.97,
          "nextBossIndex": 2,
          "intensity": 0.82
        },
        {
          "id": "rising",
          "label": "압박 상승",
          "at": 249.82,
          "nextBossIndex": 2,
          "intensity": 0.7
        },
        {
          "id": "climax",
          "label": "망각 전조",
          "at": 292,
          "nextBossIndex": 2,
          "intensity": 0.92
        },
        {
          "id": "deficit_breath",
          "label": "결손 정비",
          "at": 344.13,
          "nextBossIndex": 3,
          "intensity": 0.48
        },
        {
          "id": "deficit_trial",
          "label": "결손 압박",
          "at": 360.31,
          "nextBossIndex": 3,
          "intensity": 0.82
        },
        {
          "id": "rising",
          "label": "압박 상승",
          "at": 398.14,
          "nextBossIndex": 3,
          "intensity": 0.7
        },
        {
          "id": "climax",
          "label": "망각 전조",
          "at": 445.01,
          "nextBossIndex": 3,
          "intensity": 0.92
        },
        {
          "id": "deficit_breath",
          "label": "결손 정비",
          "at": 503.5,
          "nextBossIndex": 4,
          "intensity": 0.48
        },
        {
          "id": "deficit_trial",
          "label": "결손 압박",
          "at": 519.7,
          "nextBossIndex": 4,
          "intensity": 0.82
        },
        {
          "id": "rising",
          "label": "압박 상승",
          "at": 557.52,
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
          "projectilesAlive": 3,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 9.05,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 16,
          "projectilesAlive": 8,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 12.07,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 17,
          "projectilesAlive": 6,
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
          "enemiesAlive": 17,
          "projectilesAlive": 2,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 21.12,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 17,
          "projectilesAlive": 2,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 24.13,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 17,
          "projectilesAlive": 5,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 27.18,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 17,
          "projectilesAlive": 6,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 30.2,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 17,
          "projectilesAlive": 0,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 33.22,
          "hp": 180,
          "hpRate": 1,
          "enemiesAlive": 17,
          "projectilesAlive": 4,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 36.23,
          "hp": 173.4,
          "hpRate": 0.964,
          "enemiesAlive": 15,
          "projectilesAlive": 10,
          "pressurePhase": "deficit_breath",
          "bossActive": false
        },
        {
          "t": 39.25,
          "hp": 167.1,
          "hpRate": 0.928,
          "enemiesAlive": 17,
          "projectilesAlive": 11,
          "pressurePhase": "lull",
          "bossActive": false
        },
        {
          "t": 42.27,
          "hp": 168.9,
          "hpRate": 0.938,
          "enemiesAlive": 23,
          "projectilesAlive": 10,
          "pressurePhase": "lull",
          "bossActive": false
        },
        {
          "t": 45.28,
          "hp": 172.5,
          "hpRate": 0.958,
          "enemiesAlive": 26,
          "projectilesAlive": 12,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 48.32,
          "hp": 172.5,
          "hpRate": 0.958,
          "enemiesAlive": 30,
          "projectilesAlive": 11,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 51.35,
          "hp": 176.2,
          "hpRate": 0.979,
          "enemiesAlive": 34,
          "projectilesAlive": 19,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 54.38,
          "hp": 174.4,
          "hpRate": 0.969,
          "enemiesAlive": 33,
          "projectilesAlive": 23,
          "pressurePhase": "rising",
          "bossActive": false
        },
        {
          "t": 57.41,
          "hp":
```
