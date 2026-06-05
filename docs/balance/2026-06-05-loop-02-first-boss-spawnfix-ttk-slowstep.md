# LETHE v0.12 Balance QA

- Generated: 2026-06-05T05:52:28.354Z
- Verdict: `ITERATE_BALANCE`
- Runs: `3`
- Gameplay runs: `2`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `100.0%`
- Death at median: `165.51s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `11`
- Slots filled at median: `18.34s`
- Top DPS share median: `40.6%`
- Max enemies median: `42`
- HP <= 60% median: `104.15s`
- HP <= 40% median: `130.06s`
- HP <= 20% median: `151.91s`
- Death phase counts: `{"망각 전조":2}`

## Checks

- [ ] browser run success rate: `0.6667` target `>= 0.8`
- [x] first boss clear rate: `0` target `>= 0`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [x] level-ups before first boss: `11` target `>= 8`
- [x] slot fill timing: `18.34` target `<= 150s`
- [x] top DPS share: `0.4055` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | death | 167.57 | 망각 전조 | 39 | 127 | no | - | 11 | 18.4 | shatter_ripple | 38.9% |
| 2 | death | 163.45 | 망각 전조 | 45 | 133.13 | no | - | 11 | 18.28 | hungry_blades | 42.2% |
| 3 | browser_error | - | - | - | - | no | - | 0 | - | - | 0.0% |

