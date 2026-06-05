# LETHE v0.12 Balance QA

- Generated: 2026-06-05T03:44:21.676Z
- Verdict: `ITERATE_BALANCE`
- Runs: `5`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `60.0%`
- Death at median: `143.27s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `9`
- Slots filled at median: `15.75s`
- Top DPS share median: `38.0%`
- Max enemies median: `40`
- HP <= 60% median: `75.71s`
- HP <= 40% median: `102.96s`
- HP <= 20% median: `121.16s`
- Death phase counts: `{"망각 전조":3}`

## Checks

- [x] browser run success rate: `0.8` target `>= 0.8`
- [ ] first boss clear rate: `0` target `>= 0.7`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [x] level-ups before first boss: `9` target `>= 8`
- [x] slot fill timing: `15.745` target `<= 150s`
- [x] top DPS share: `0.3801` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | death | 143.27 | 망각 전조 | 39 | 99.94 | no | - | 9 | 15.12 | shatter_ripple | 38.0% |
| 2 | browser_error | - | - | - | - | no | - | 0 | - | - | 0.0% |
| 3 | death | 135.71 | 망각 전조 | 47 | 102.96 | no | - | 10 | 18.13 | hungry_blades | 44.8% |
| 4 | running | - | - | 39 | - | no | - | 8 | 14.92 | hungry_blades | 65.2% |
| 5 | death | 168.2 | 망각 전조 | 41 | 121.11 | no | - | 10 | 16.37 | hungry_blades | 37.3% |

