# LETHE v0.12 Balance QA

- Generated: 2026-06-05T00:57:02.837Z
- Verdict: `ITERATE_BALANCE`
- Runs: `5`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `100.0%`
- Death at median: `119.24s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `10`
- Slots filled at median: `13.6s`
- Top DPS share median: `43.2%`

## Checks

- [x] browser run success rate: `1` target `>= 0.8`
- [ ] first boss clear rate: `0` target `>= 0.7`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [x] level-ups before first boss: `10` target `>= 8`
- [x] slot fill timing: `13.6` target `<= 150s`
- [x] top DPS share: `0.4324` target `<= 0.5`

## Runs

| run | result | death at | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | death | 100.32 | no | - | 10 | 10.28 | hungry_blades | 52.5% |
| 2 | death | 90.11 | no | - | 10 | 19.23 | hungry_blades | 43.2% |
| 3 | death | 178.37 | no | - | 12 | 10.12 | shatter_ripple | 48.0% |
| 4 | death | 157.17 | no | - | 11 | 13.6 | shatter_ripple | 37.7% |
| 5 | death | 119.24 | no | - | 10 | 14.63 | hungry_blades | 42.9% |

