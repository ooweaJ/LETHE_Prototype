# LETHE v0.12 Balance QA

- Generated: 2026-06-05T02:48:23.130Z
- Verdict: `ITERATE_BALANCE`
- Runs: `5`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `40.0%`
- Death at median: `155.44s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `10`
- Slots filled at median: `18.05s`
- Top DPS share median: `43.4%`
- Max enemies median: `53`
- Death phase counts: `{"망각 전조":2}`

## Checks

- [x] browser run success rate: `1` target `>= 0.8`
- [ ] first boss clear rate: `0` target `>= 0.7`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [x] level-ups before first boss: `10` target `>= 8`
- [x] slot fill timing: `18.05` target `<= 150s`
- [x] top DPS share: `0.4336` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | death | 134.09 | 망각 전조 | 52 | no | - | 10 | 18.72 | shatter_ripple | 41.5% |
| 2 | death | 176.78 | 망각 전조 | 53 | no | - | 10 | 18.7 | shatter_ripple | 44.0% |
| 3 | running | - | - | 53 | no | - | 11 | 13.25 | stalker_oath | 43.1% |
| 4 | running | - | - | 53 | no | - | 10 | 18.05 | hungry_blades | 51.8% |
| 5 | running | - | - | 53 | no | - | 10 | 17.28 | hungry_blades | 43.4% |

