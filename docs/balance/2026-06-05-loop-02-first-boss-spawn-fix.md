# LETHE v0.12 Balance QA

- Generated: 2026-06-05T04:25:23.626Z
- Verdict: `ITERATE_BALANCE`
- Runs: `5`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `40.0%`
- Death at median: `125.09s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `0`
- Slots filled at median: `17.98s`
- Top DPS share median: `0.0%`
- Max enemies median: `45`
- HP <= 60% median: `68.09s`
- HP <= 40% median: `84.73s`
- HP <= 20% median: `110.47s`
- Death phase counts: `{"압박 상승":1,"망각 전조":1}`

## Checks

- [ ] browser run success rate: `0.4` target `>= 0.8`
- [ ] first boss clear rate: `0` target `>= 0.7`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [ ] level-ups before first boss: `0` target `>= 8`
- [x] slot fill timing: `17.985` target `<= 150s`
- [x] top DPS share: `0` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | browser_error | - | - | - | - | no | - | 0 | - | - | 0.0% |
| 2 | browser_error | - | - | - | - | no | - | 0 | - | - | 0.0% |
| 3 | death | 120.26 | 압박 상승 | 45 | 87.76 | no | - | 8 | 18.67 | shatter_ripple | 39.7% |
| 4 | death | 129.91 | 망각 전조 | 45 | 81.71 | no | - | 10 | 17.3 | hungry_blades | 36.3% |
| 5 | browser_error | - | - | - | - | no | - | 0 | - | - | 0.0% |

