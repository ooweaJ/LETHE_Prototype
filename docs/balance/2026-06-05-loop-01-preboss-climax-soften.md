# LETHE v0.12 Balance QA

- Generated: 2026-06-05T08:17:05.127Z
- Verdict: `ITERATE_BALANCE`
- Runs: `5`
- Gameplay runs: `5`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `20.0%`
- Death at median: `180.42s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `9`
- Slots filled at median: `21.03s`
- Top DPS share median: `44.9%`
- Max enemies median: `42`
- HP <= 60% median: `129.57s`
- HP <= 40% median: `160.58s`
- HP <= 20% median: `169.73s`
- Death phase counts: `{"첫 망각 문지기":1}`

## Checks

- [x] browser run success rate: `1` target `>= 0.8`
- [x] first boss clear rate: `0` target `>= 0`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [x] level-ups before first boss: `9` target `>= 8`
- [x] slot fill timing: `21.03` target `<= 150s`
- [x] top DPS share: `0.4491` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | running | - | - | 42 | - | no | - | 8 | 16.53 | hungry_blades | 63.8% |
| 2 | death | 180.42 | 첫 망각 문지기 | 55 | 160.58 | no | - | 11 | 23.05 | hungry_blades | 44.9% |
| 3 | running | - | - | 37 | - | no | - | 9 | 12.88 | hungry_blades | 42.3% |
| 4 | running | - | - | 42 | - | no | - | 9 | 22.13 | hungry_blades | 36.6% |
| 5 | running | - | - | 49 | - | no | - | 9 | 21.03 | shatter_ripple | 60.5% |

