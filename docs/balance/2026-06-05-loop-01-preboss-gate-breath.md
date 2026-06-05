# LETHE v0.12 Balance QA

- Generated: 2026-06-05T08:10:35.689Z
- Verdict: `ITERATE_BALANCE`
- Runs: `5`
- Gameplay runs: `4`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `75.0%`
- Death at median: `163.05s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `10`
- Slots filled at median: `15.66s`
- Top DPS share median: `39.8%`
- Max enemies median: `39.5`
- HP <= 60% median: `90.42s`
- HP <= 40% median: `127s`
- HP <= 20% median: `147.32s`
- Death phase counts: `{"망각 전조":2,"문지기 호흡":1}`

## Checks

- [x] browser run success rate: `0.8` target `>= 0.8`
- [x] first boss clear rate: `0` target `>= 0`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [x] level-ups before first boss: `10` target `>= 8`
- [x] slot fill timing: `15.655` target `<= 150s`
- [x] top DPS share: `0.3981` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | browser_error | - | - | - | - | no | - | 0 | - | - | 0.0% |
| 2 | death | 163.05 | 망각 전조 | 40 | 127 | no | - | 10 | 16.18 | hungry_blades | 37.5% |
| 3 | running | - | - | 39 | - | no | - | 8 | 14.83 | hungry_blades | 42.1% |
| 4 | death | 174.3 | 문지기 호흡 | 39 | 142.25 | no | - | 11 | 15.13 | shatter_ripple | 43.2% |
| 5 | death | 136.43 | 망각 전조 | 45 | 86.38 | no | - | 10 | 17.28 | hungry_blades | 37.0% |

