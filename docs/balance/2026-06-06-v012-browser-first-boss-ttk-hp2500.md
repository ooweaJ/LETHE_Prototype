# LETHE v0.12 Balance QA

- Generated: 2026-06-05T17:55:26.759Z
- Verdict: `GO_BALANCE_BASELINE`
- Runs: `3`
- Gameplay runs: `3`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `0.0%`
- Death at median: `-s`
- First boss clear rate: `100.0%`
- First boss TTK median: `21.05s`
- Level-ups before first boss median: `0`
- Slots filled at median: `176s`
- Top DPS share median: `35.4%`
- Max enemies median: `4`
- HP <= 60% median: `-s`
- HP <= 40% median: `-s`
- HP <= 20% median: `-s`
- Death phase counts: `{}`

## Checks

- [x] browser run success rate: `1` target `>= 0.8`
- [x] first boss clear rate: `1` target `>= 0.7`
- [x] first boss TTK lower bound: `21.05` target `>= 15s`
- [x] first boss TTK upper bound: `21.05` target `<= 30s`

## Runs

| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | first_boss_ttk | - | - | 3 | - | yes | 19.7 | 0 | 176 | execution_flash | 35.4% |
| 2 | first_boss_ttk | - | - | 4 | - | yes | 21.45 | 0 | 176 | execution_flash | 31.4% |
| 3 | first_boss_ttk | - | - | 5 | - | yes | 21.05 | 0 | 176 | execution_flash | 39.1% |

