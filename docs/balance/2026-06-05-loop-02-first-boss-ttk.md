# LETHE v0.12 Balance QA

- Generated: 2026-06-05T00:48:28.198Z
- Verdict: `ITERATE_BALANCE`
- Runs: `5`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `80.0%`
- Death at median: `120.66s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `10`
- Slots filled at median: `14.78s`
- Top DPS share median: `46.4%`

## Checks

- [x] browser run success rate: `1` target `>= 0.8`
- [ ] first boss clear rate: `0` target `>= 0.7`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [x] level-ups before first boss: `10` target `>= 8`
- [x] slot fill timing: `14.78` target `<= 150s`
- [x] top DPS share: `0.4637` target `<= 0.5`

## Runs

| run | result | death at | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | running | - | no | - | 11 | 21.55 | hungry_blades | 64.8% |
| 2 | death | 118.07 | no | - | 10 | 22.35 | hungry_blades | 42.7% |
| 3 | death | 123.24 | no | - | 10 | 14.22 | shatter_ripple | 44.0% |
| 4 | death | 84.29 | no | - | 9 | 13.88 | hungry_blades | 46.4% |
| 5 | death | 209.26 | no | - | 11 | 14.78 | shatter_ripple | 56.2% |

