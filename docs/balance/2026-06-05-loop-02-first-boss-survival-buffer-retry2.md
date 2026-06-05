# LETHE v0.12 Balance QA

- Generated: 2026-06-05T03:33:45.978Z
- Verdict: `ITERATE_BALANCE`
- Runs: `5`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `80.0%`
- Death at median: `153.41s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `10`
- Slots filled at median: `15.75s`
- Top DPS share median: `43.5%`
- Max enemies median: `43`
- HP <= 60% median: `81.73s`
- HP <= 40% median: `108.97s`
- HP <= 20% median: `136.23s`
- Death phase counts: `{"망각 전조":4}`

## Checks

- [x] browser run success rate: `1` target `>= 0.8`
- [ ] first boss clear rate: `0` target `>= 0.7`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [x] level-ups before first boss: `10` target `>= 8`
- [x] slot fill timing: `15.75` target `<= 150s`
- [x] top DPS share: `0.4349` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | death | 127.94 | 망각 전조 | 40 | 81.71 | no | - | 10 | 14.83 | hungry_blades | 50.9% |
| 2 | running | - | - | 43 | - | no | - | 10 | 15.75 | hungry_blades | 42.8% |
| 3 | death | 173.42 | 망각 전조 | 45 | 111.99 | no | - | 11 | 17.73 | shatter_ripple | 43.5% |
| 4 | death | 175.5 | 망각 전조 | 43 | 142.26 | no | - | 11 | 14.08 | shatter_ripple | 43.7% |
| 5 | death | 133.41 | 망각 전조 | 42 | 105.94 | no | - | 10 | 15.92 | hungry_blades | 40.5% |

