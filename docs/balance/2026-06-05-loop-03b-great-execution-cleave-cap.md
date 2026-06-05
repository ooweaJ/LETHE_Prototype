# LETHE v0.12 Balance QA

- Generated: 2026-06-05T02:47:54.128Z
- Verdict: `ITERATE_BALANCE`
- Runs: `5`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `40.0%`
- Death at median: `120.56s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `10`
- Slots filled at median: `9.66s`
- Top DPS share median: `34.2%`
- Max enemies median: `48`
- Death phase counts: `{"압박 상승":1,"망각 전조":1}`

## Checks

- [x] browser run success rate: `0.8` target `>= 0.8`
- [ ] first boss clear rate: `0` target `>= 0.7`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [x] level-ups before first boss: `10` target `>= 8`
- [x] slot fill timing: `9.665` target `<= 150s`
- [x] top DPS share: `0.3417` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | death | 97.84 | 압박 상승 | 41 | no | - | 9 | 8.63 | weapon | 34.2% |
| 2 | browser_error | - | - | - | no | - | 0 | - | - | 0.0% |
| 3 | death | 143.29 | 망각 전조 | 43 | no | - | 11 | 17.63 | weapon | 33.3% |
| 4 | running | - | - | 53 | no | - | 11 | 9.55 | shatter_ripple | 41.5% |
| 5 | running | - | - | 53 | no | - | 10 | 9.78 | weapon | 57.0% |

