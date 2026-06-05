# LETHE v0.12 Balance QA

- Generated: 2026-06-05T09:54:17.609Z
- Verdict: `ITERATE_BALANCE`
- Runs: `1`
- Gameplay runs: `0`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `0.0%`
- Death at median: `-s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `-`
- Slots filled at median: `-s`
- Top DPS share median: `-`
- Max enemies median: `-`
- HP <= 60% median: `-s`
- HP <= 40% median: `-s`
- HP <= 20% median: `-s`
- Death phase counts: `{}`

## Checks

- [ ] browser run success rate: `0` target `>= 0.8`
- [x] first boss clear rate: `0` target `>= 0`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [ ] level-ups before first boss: `-` target `>= 8`
- [x] slot fill timing: `-` target `<= 150s`
- [x] top DPS share: `-` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | browser_error | - | - | - | - | no | - | 0 | - | - | 0.0% |

