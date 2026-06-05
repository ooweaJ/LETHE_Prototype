# LETHE v0.12 Balance QA

- Generated: 2026-06-05T15:40:55.348Z
- Verdict: `GO_BALANCE_BASELINE`
- Runs: `3`
- Gameplay runs: `3`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `0.0%`
- Death at median: `-s`
- First boss clear rate: `100.0%`
- First boss TTK median: `25.76s`
- Level-ups before first boss median: `0`
- Slots filled at median: `176s`
- Top DPS share median: `37.3%`
- Max enemies median: `3`
- HP <= 60% median: `-s`
- HP <= 40% median: `-s`
- HP <= 20% median: `-s`
- Death phase counts: `{}`

## Checks

- [x] browser run success rate: `1` target `>= 0.8`
- [x] first boss clear rate: `1` target `>= 0.7`
- [x] first boss TTK lower bound: `25.76` target `>= 15s`
- [x] first boss TTK upper bound: `25.76` target `<= 30s`

## Runs

| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | first_boss_ttk | - | - | 3 | - | yes | 22.93 | 0 | 176 | execution_flash | 37.3% |
| 2 | first_boss_ttk | - | - | 3 | - | yes | 27.89 | 0 | 176 | execution_flash | 35.1% |
| 3 | first_boss_ttk | - | - | 3 | - | yes | 25.76 | 0 | 176 | execution_flash | 38.6% |

