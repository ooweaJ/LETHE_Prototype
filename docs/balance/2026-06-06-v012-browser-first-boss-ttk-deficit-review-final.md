# LETHE v0.12 Balance QA

- Generated: 2026-06-06T08:34:16.247Z
- Verdict: `GO_BALANCE_BASELINE`
- Runs: `3`
- Gameplay runs: `3`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `0.0%`
- Death at median: `-s`
- First boss clear rate: `100.0%`
- First boss TTK median: `19.82s`
- Level-ups before first boss median: `0`
- Slots filled at median: `176s`
- Top DPS share median: `34.8%`
- Max enemies median: `3`
- HP <= 60% median: `-s`
- HP <= 40% median: `-s`
- HP <= 20% median: `-s`
- Death phase counts: `{}`

## Checks

- [x] browser run success rate: `1` target `>= 0.8`
- [x] first boss clear rate: `1` target `>= 0.7`
- [x] first boss TTK lower bound: `19.82` target `>= 15s`
- [x] first boss TTK upper bound: `19.82` target `<= 30s`

## Runs

| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | first_boss_ttk | - | - | 3 | - | yes | 19.82 | 0 | 176 | weapon | 34.8% |
| 2 | first_boss_ttk | - | - | 3 | - | yes | 18.86 | 0 | 176 | weapon | 30.9% |
| 3 | first_boss_ttk | - | - | 6 | - | yes | 26.96 | 0 | 176 | execution_flash | 35.0% |

