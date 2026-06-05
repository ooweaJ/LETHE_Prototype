# LETHE v0.12 Balance QA

- Generated: 2026-06-05T15:21:41.011Z
- Verdict: `ITERATE_BALANCE`
- Runs: `3`
- Gameplay runs: `3`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `0.0%`
- Death at median: `-s`
- First boss clear rate: `33.3%`
- First boss TTK median: `22.59s`
- Level-ups before first boss median: `0`
- Slots filled at median: `176s`
- Top DPS share median: `0.0%`
- Max enemies median: `3`
- HP <= 60% median: `-s`
- HP <= 40% median: `-s`
- HP <= 20% median: `-s`
- Death phase counts: `{}`

## Checks

- [x] browser run success rate: `1` target `>= 0.8`
- [ ] first boss clear rate: `0.3333` target `>= 0.7`
- [ ] clear rate minimum: `0` target `>= 0.35`
- [x] clear rate maximum: `0` target `<= 0.8`
- [x] first boss TTK lower bound: `22.59` target `>= 15s`
- [x] first boss TTK upper bound: `22.59` target `<= 30s`
- [ ] level-ups before first boss: `0` target `>= 8`
- [ ] slot fill timing: `176` target `<= 150s`
- [x] top DPS share: `0` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | incomplete | - | - | - | - | no | - | 0 | - | - | 0.0% |
| 2 | incomplete | - | - | - | - | no | - | 0 | - | - | 0.0% |
| 3 | running | - | - | 3 | - | yes | 22.59 | 0 | 176 | execution_flash | 34.4% |

