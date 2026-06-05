# LETHE v0.12 Balance QA

- Generated: 2026-06-05T02:53:55.374Z
- Verdict: `ITERATE_BALANCE`
- Runs: `5`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `60.0%`
- Death at median: `120.32s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `8`
- Slots filled at median: `14.73s`
- Top DPS share median: `36.0%`
- Max enemies median: `42`
- Death phase counts: `{"압박 상승":2,"망각 전조":1}`

## Checks

- [ ] browser run success rate: `0.6` target `>= 0.8`
- [ ] first boss clear rate: `0` target `>= 0.7`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [x] level-ups before first boss: `8` target `>= 8`
- [x] slot fill timing: `14.73` target `<= 150s`
- [x] top DPS share: `0.3605` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | death | 119.87 | 압박 상승 | 48 | no | - | 9 | 19.47 | hungry_blades | 36.9% |
| 2 | browser_error | - | - | - | no | - | 0 | - | - | 0.0% |
| 3 | browser_error | - | - | - | no | - | 0 | - | - | 0.0% |
| 4 | death | 120.32 | 압박 상승 | 37 | no | - | 8 | 14.12 | hungry_blades | 36.0% |
| 5 | death | 146.59 | 망각 전조 | 42 | no | - | 11 | 14.73 | shatter_ripple | 37.1% |

