# LETHE v0.12 Balance QA

- Generated: 2026-06-05T04:03:46.186Z
- Verdict: `ITERATE_BALANCE`
- Runs: `5`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `40.0%`
- Death at median: `176.07s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `9`
- Slots filled at median: `17.45s`
- Top DPS share median: `39.1%`
- Max enemies median: `43`
- HP <= 60% median: `74.17s`
- HP <= 40% median: `107.5s`
- HP <= 20% median: `142.36s`
- Death phase counts: `{"망각 전조":2}`

## Checks

- [ ] browser run success rate: `0.6` target `>= 0.8`
- [ ] first boss clear rate: `0` target `>= 0.7`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [x] level-ups before first boss: `9` target `>= 8`
- [x] slot fill timing: `17.45` target `<= 150s`
- [x] top DPS share: `0.3905` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | death | 179.3 | 망각 전조 | 39 | 112.07 | no | - | 11 | 13.25 | shatter_ripple | 43.0% |
| 2 | browser_error | - | - | - | - | no | - | 0 | - | - | 0.0% |
| 3 | browser_error | - | - | - | - | no | - | 0 | - | - | 0.0% |
| 4 | running | - | - | 44 | - | no | - | 9 | 17.45 | hungry_blades | 57.4% |
| 5 | death | 172.85 | 망각 전조 | 43 | 102.94 | no | - | 11 | 17.5 | hungry_blades | 39.1% |

