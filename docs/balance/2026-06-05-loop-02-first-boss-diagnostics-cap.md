# LETHE v0.12 Balance QA

- Generated: 2026-06-05T02:48:49.491Z
- Verdict: `ITERATE_BALANCE`
- Runs: `5`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `80.0%`
- Death at median: `131.7s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `10`
- Slots filled at median: `17.58s`
- Top DPS share median: `41.3%`
- Max enemies median: `47`
- Death phase counts: `{"망각 전조":2,"압박 상승":2}`

## Checks

- [x] browser run success rate: `1` target `>= 0.8`
- [ ] first boss clear rate: `0` target `>= 0.7`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [x] level-ups before first boss: `10` target `>= 8`
- [x] slot fill timing: `17.58` target `<= 150s`
- [x] top DPS share: `0.4127` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | death | 144.15 | 망각 전조 | 52 | no | - | 10 | 13.88 | shatter_ripple | 33.4% |
| 2 | running | - | - | 53 | no | - | 10 | 18 | hungry_blades | 46.8% |
| 3 | death | 121.46 | 압박 상승 | 47 | no | - | 9 | 22.85 | hungry_blades | 41.3% |
| 4 | death | 91.64 | 압박 상승 | 43 | no | - | 9 | 17.58 | hungry_blades | 32.2% |
| 5 | death | 141.94 | 망각 전조 | 47 | no | - | 11 | 14.83 | hungry_blades | 42.4% |

