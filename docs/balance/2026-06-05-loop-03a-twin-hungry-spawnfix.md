# LETHE v0.12 Balance QA

- Generated: 2026-06-05T05:55:27.418Z
- Verdict: `ITERATE_BALANCE`
- Runs: `3`
- Gameplay runs: `3`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `33.3%`
- Death at median: `188.4s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `10`
- Slots filled at median: `18.82s`
- Top DPS share median: `42.1%`
- Max enemies median: `42`
- HP <= 60% median: `60.95s`
- HP <= 40% median: `99.57s`
- HP <= 20% median: `130.05s`
- Death phase counts: `{"첫 망각 문지기":1}`

## Checks

- [x] browser run success rate: `1` target `>= 0.8`
- [x] first boss clear rate: `0` target `>= 0`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [x] level-ups before first boss: `10` target `>= 8`
- [x] slot fill timing: `18.82` target `<= 150s`
- [x] top DPS share: `0.4207` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | death | 188.4 | 첫 망각 문지기 | 49 | 99.57 | no | - | 10 | 20.25 | hungry_blades | 35.9% |
| 2 | running | - | - | 42 | - | no | - | 10 | 16.7 | hungry_blades | 47.1% |
| 3 | running | - | - | 41 | - | no | - | 9 | 18.82 | hungry_blades | 42.1% |

