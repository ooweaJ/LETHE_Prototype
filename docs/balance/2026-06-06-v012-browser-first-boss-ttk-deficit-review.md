# LETHE v0.12 Balance QA

- Generated: 2026-06-06T08:32:04.310Z
- Verdict: `GO_BALANCE_BASELINE`
- Runs: `3`
- Gameplay runs: `3`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `0.0%`
- Death at median: `-s`
- First boss clear rate: `100.0%`
- First boss TTK median: `20.16s`
- Level-ups before first boss median: `0`
- Slots filled at median: `176s`
- Top DPS share median: `32.7%`
- Max enemies median: `5`
- HP <= 60% median: `-s`
- HP <= 40% median: `-s`
- HP <= 20% median: `-s`
- Death phase counts: `{}`

## Checks

- [x] browser run success rate: `1` target `>= 0.8`
- [x] first boss clear rate: `1` target `>= 0.7`
- [x] first boss TTK lower bound: `20.16` target `>= 15s`
- [x] first boss TTK upper bound: `20.16` target `<= 30s`

## Runs

| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | first_boss_ttk | - | - | 2 | - | yes | 20.16 | 1 | 176 | weapon | 32.2% |
| 2 | first_boss_ttk | - | - | 7 | - | yes | 20.91 | 0 | 176 | weapon | 35.0% |
| 3 | first_boss_ttk | - | - | 5 | - | yes | 18.48 | 0 | 176 | weapon | 32.7% |

