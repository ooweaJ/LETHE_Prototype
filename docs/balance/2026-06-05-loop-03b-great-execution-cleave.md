# LETHE v0.12 Balance QA

- Generated: 2026-06-05T02:44:53.348Z
- Verdict: `ITERATE_BALANCE`
- Runs: `5`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `60.0%`
- Death at median: `138.17s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `11`
- Slots filled at median: `9.45s`
- Top DPS share median: `38.1%`
- Max enemies median: `72`
- Death phase counts: `{"압박 상승":1,"망각 전조":2}`

## Checks

- [x] browser run success rate: `1` target `>= 0.8`
- [ ] first boss clear rate: `0` target `>= 0.7`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [x] level-ups before first boss: `11` target `>= 8`
- [x] slot fill timing: `9.45` target `<= 150s`
- [x] top DPS share: `0.3811` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | running | - | - | 72 | no | - | 11 | 13.53 | shatter_ripple | 49.8% |
| 2 | death | 109.82 | 압박 상승 | 74 | no | - | 10 | 9.03 | weapon | 45.2% |
| 3 | running | - | - | 84 | no | - | 11 | 15.28 | hungry_blades | 38.1% |
| 4 | death | 156.23 | 망각 전조 | 63 | no | - | 11 | 9.45 | weapon | 28.1% |
| 5 | death | 138.17 | 망각 전조 | 56 | no | - | 11 | 8.88 | weapon | 30.3% |

