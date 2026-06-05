# LETHE v0.12 Balance QA

- Generated: 2026-06-05T06:08:23.712Z
- Verdict: `ITERATE_BALANCE`
- Runs: `3`
- Gameplay runs: `2`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `50.0%`
- Death at median: `174.58s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `10.5`
- Slots filled at median: `8.9s`
- Top DPS share median: `35.5%`
- Max enemies median: `39`
- HP <= 60% median: `121.92s`
- HP <= 40% median: `137.17s`
- HP <= 20% median: `160.53s`
- Death phase counts: `{"망각 전조":1}`

## Checks

- [ ] browser run success rate: `0.6667` target `>= 0.8`
- [x] first boss clear rate: `0` target `>= 0`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [x] level-ups before first boss: `10.5` target `>= 8`
- [x] slot fill timing: `8.9` target `<= 150s`
- [x] top DPS share: `0.3549` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | browser_error | - | - | - | - | no | - | 0 | - | - | 0.0% |
| 2 | running | - | - | 39 | - | no | - | 10 | 9.93 | weapon | 41.5% |
| 3 | death | 174.58 | 망각 전조 | 39 | 137.17 | no | - | 11 | 7.87 | weapon | 29.5% |

