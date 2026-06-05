# LETHE v0.12 Balance QA

- Generated: 2026-06-05T03:25:11.888Z
- Verdict: `ITERATE_BALANCE`
- Runs: `5`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `40.0%`
- Death at median: `144.29s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `9`
- Slots filled at median: `15.67s`
- Top DPS share median: `30.3%`
- Max enemies median: `43`
- HP <= 60% median: `80.22s`
- HP <= 40% median: `99.9s`
- HP <= 20% median: `122.63s`
- Death phase counts: `{"압박 상승":1,"망각 전조":1}`

## Checks

- [ ] browser run success rate: `0.6` target `>= 0.8`
- [ ] first boss clear rate: `0` target `>= 0.7`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [x] level-ups before first boss: `9` target `>= 8`
- [x] slot fill timing: `15.67` target `<= 150s`
- [x] top DPS share: `0.3031` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | browser_error | - | - | - | - | no | - | 0 | - | - | 0.0% |
| 2 | running | - | - | 43 | - | no | - | 9 | 15.92 | hungry_blades | 50.7% |
| 3 | death | 121.86 | 압박 상승 | 39 | 93.86 | no | - | 10 | 15.25 | hungry_blades | 31.4% |
| 4 | browser_error | - | - | - | - | no | - | 0 | - | - | 0.0% |
| 5 | death | 166.72 | 망각 전조 | 43 | 105.94 | no | - | 10 | 15.67 | hungry_blades | 30.3% |

