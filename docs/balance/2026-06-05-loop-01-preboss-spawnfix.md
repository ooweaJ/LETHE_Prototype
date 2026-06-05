# LETHE v0.12 Balance QA

- Generated: 2026-06-05T05:14:54.527Z
- Verdict: `ITERATE_BALANCE`
- Runs: `5`
- Gameplay runs: `4`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `50.0%`
- Death at median: `159.8s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `10`
- Slots filled at median: `19.21s`
- Top DPS share median: `38.7%`
- Max enemies median: `44`
- HP <= 60% median: `80.23s`
- HP <= 40% median: `113.55s`
- HP <= 20% median: `137.76s`
- Death phase counts: `{"망각 전조":2}`

## Checks

- [x] browser run success rate: `0.8` target `>= 0.8`
- [x] first boss clear rate: `0` target `>= 0`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [x] level-ups before first boss: `10` target `>= 8`
- [x] slot fill timing: `19.21` target `<= 150s`
- [x] top DPS share: `0.3871` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | death | 174.93 | 망각 전조 | 49 | 118.19 | no | - | 11 | 20.82 | hungry_blades | 39.3% |
| 2 | running | - | - | 55 | - | no | - | 7 | 24.05 | shatter_ripple | 38.1% |
| 3 | death | 144.67 | 망각 전조 | 39 | 108.91 | no | - | 11 | 13.23 | hungry_blades | 33.7% |
| 4 | browser_error | - | - | - | - | no | - | 0 | - | - | 0.0% |
| 5 | running | - | - | 39 | - | no | - | 9 | 17.6 | hungry_blades | 46.4% |

