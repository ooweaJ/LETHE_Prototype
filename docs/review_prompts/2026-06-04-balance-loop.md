# LETHE v0.12 Balance Loop - 2026-06-04

## 목적

감정 proxy와 Alpha Fun Score를 쓰지 않고 v0.12 telemetry 기반으로 다음 밸런스 조정만 판단한다.

## 현재 판정

- Verdict: `ITERATE_BALANCE`
- Runs: `5`
- First boss clear rate: `0.0%`
- Full clear rate: `0.0%`
- Death rate: `100.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `3`
- Slots filled at median: `20.18s`
- Top DPS share median: `51.2%`

## 실패한 밸런스 체크

- first boss clear rate: value `0`, target `>= 0.7`
- clear rate minimum: value `0`, target `>= 0.35`
- first boss TTK lower bound: value `-`, target `>= 15s`
- level-ups before first boss: value `3`, target `>= 8`
- top DPS share: value `0.5119`, target `<= 0.5`

## Codex 다음 구현 지시

- `docs/BALANCE_TABLE_v0_12.md`와 `docs/LETHE_v0.12_밸런스_개선_제안서.md`를 기준으로 가장 작은 밸런스 조정 1개만 선택한다.
- 감정선, regret, irritation, Alpha Fun Score는 이번 판단에서 제외한다.
- 우선순위는 첫 보스 TTK, 첫 180초 레벨업 수, 3슬롯 완성 시각, top DPS share, clear/death rate 순서다.
- 새 기억, 새 무기, 상점, 메타 진행, 새 지역, 최종 보스 확장은 금지한다.
- 변경 후 `npm run qa:balance` 또는 환경 blocker 기록을 남긴다.

## 원본 summary

```json
{
  "generatedAt": "2026-06-04T08:51:29.208Z",
  "version": "v0.12-balance-loop-1",
  "verdict": "ITERATE_BALANCE",
  "targets": {
    "firstBossClearRateMin": 0.7,
    "clearRateMin": 0.35,
    "clearRateMax": 0.8,
    "firstBossTtkMin": 15,
    "firstBossTtkMax": 30,
    "levelUpsBeforeFirstBossMin": 8,
    "slotsFilledAtMax": 150,
    "topDpsShareMax": 0.5
  },
  "metrics": {
    "runs": 5,
    "clearRate": 0,
    "deathRate": 1,
    "firstBossClearRate": 0,
    "firstBossTtkMean": null,
    "firstBossTtkMedian": null,
    "levelUpsBeforeFirstBossMean": 2.8,
    "levelUpsBeforeFirstBossMedian": 3,
    "slotsFilledAtMean": 20.549999999999997,
    "slotsFilledAtMedian": 20.185,
    "topDpsShareMean": 0.55602,
    "topDpsShareMedian": 0.5119
  },
  "checks": [
    {
      "name": "first boss clear rate",
      "pass": false,
      "value": 0,
      "target": ">= 0.7"
    },
    {
      "name": "clear rate minimum",
      "pass": false,
      "value": 0,
      "target": ">= 0.35"
    },
    {
      "name": "clear rate maximum",
      "pass": true,
      "value": 0,
      "target": "<= 0.8"
    },
    {
      "name": "first boss TTK lower bound",
      "pass": false,
      "value": null,
      "target": ">= 15s"
    },
    {
      "name": "first boss TTK upper bound",
      "pass": true,
      "value": null,
      "target": "<= 30s"
    },
    {
      "name": "level-ups before first boss",
      "pass": false,
      "value": 3,
      "target": ">= 8"
    },
    {
      "name": "slot fill timing",
      "pass": true,
      "value": 20.185,
      "target": "<= 150s"
    },
    {
      "name": "top DPS share",
      "pass": false,
      "value": 0.5119,
      "target": "<= 0.5"
    }
  ],
  "failed": [
    {
      "name": "first boss clear rate",
      "pass": false,
      "value": 0,
      "target": ">= 0.7"
    },
    {
      "name": "clear rate minimum",
      "pass": false,
      "value": 0,
      "target": ">= 0.35"
    },
    {
      "name": "first boss TTK lower bound",
      "pass": false,
      "value": null,
      "target": ">= 15s"
    },
    {
      "name": "level-ups before first boss",
      "pass": false,
      "value": 3,
      "target": ">= 8"
    },
    {
      "name": "top DPS share",
      "pass": false,
      "value": 0.5119,
      "target": "<= 0.5"
    }
  ],
  "runs": [
    {
      "runNumber": 1,
      "status": "complete",
      "runResult": "death",
      "finalClear": false,
      "death": true,
      "firstBossCleared": false,
      "firstBossTtk": null,
      "firstBossFocusedDps": null,
      "level": 3,
      "levelUpsBeforeFirstBoss": 2,
      "slotsFilledAt": 25.03,
      "activeMemoryCount": 3,
      "topDpsSource": "hungry_blades",
      "topDps": 36.51,
      "topDpsShare": 0.6998,
      "dpsBySource": {
        "weapon": 12.14,
        "hungry_blades": 36.51,
        "oblivion_brand": 3.52
      },
      "bossFights": []
    },
    {
      "runNumber": 2,
      "status": "complete",
      "runResult": "death",
      "finalClear": false,
      "death": true,
      "firstBossCleared": false,
      "firstBossTtk": null,
      "firstBossFocusedDps": null,
      "level": 4,
      "levelUpsBeforeFirstBoss": 3,
      "slotsFilledAt": 16.8,
      "activeMemoryCount": 3,
      "topDpsSource": "shatter_ripple",
      "topDps": 36.75,
      "topDpsShare": 0.4134,
      "dpsBySource": {
        "weapon": 13.35,
        "hungry_blades": 31.3,
        "shatter_ripple": 36.75,
        "execution_flash": 7.5
      },
      "bossFights": []
    },
    {
      "runNumber": 3,
      "status": "complete",
      "runResult": "death",
      "finalClear": false,
      "death": true,
      "firstBossCleared": false,
      "firstBossTtk": null,
      "firstBossFocusedDps": null,
      "level": 4,
      "levelUpsBeforeFirstBoss": 3,
      "slotsFilledAt": null,
      "activeMemoryCount": 2,
      "topDpsSource": "hungry_blades",
      "topDps": 42.46,
      "topDpsShare": 0.6601,
      "dpsBySource": {
        "weapon": 18.9,
        "hungry_blades": 42.46,
        "blood_reflection": 2.96
      },
      "bossFights": []
    },
    {
      "runNumber": 4,
      "status": "complete",
      "runResult": "death",
      "finalClear": false,
      "death": true,
      "firstBossCleared": false,
      "firstBossTtk": null,
      "firstBossFocusedDps": null,
      "level": 3,
      "levelUpsBeforeFirstBoss": 2,
      "slotsFilledAt": 18.22,
      "activeMemoryCount": 3,
      "topDpsSource": "hungry_blades",
      "topDps": 34.51,
      "topDpsShare": 0.5119,
      "dpsBySource": {
        "weapon": 16.7,
        "hungry_blades": 34.51,
        "execution_flash": 9.63,
        "stopped_second": 6.58
      },
      "bossFights": []
    },
    {
      "runNumber": 5,
      "status": "complete",
      "runResult": "death",
      "finalClear": false,
      "death": true,
      "firstBossCleared": false,
      "firstBossTtk": null,
      "firstBossFocusedDps": null,
      "level": 5,
      "levelUpsBeforeFirstBoss": 4,
      "slotsFilledAt": 22.15,
      "activeMemoryCount": 3,
      "topDpsSource": "hungry_blades",
      "topDps": 45.83,
      "topDpsShare": 0.4949,
      "dpsBySource": {
        "weapon": 20.55,
        "hungry_blades": 45.83,
        "blood_reflection": 8,
        "ashen_guard": 18.23
      },
      "bossFights": []
    }
  ]
}
```
