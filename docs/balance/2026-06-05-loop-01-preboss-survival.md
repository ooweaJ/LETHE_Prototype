# LETHE v0.12 Balance QA

- Generated: 2026-06-05T00:41:44.487Z
- Verdict: `ITERATE_BALANCE`
- Runs: `10`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `60.0%`
- Death at median: `118.38s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `10`
- Slots filled at median: `15.02s`
- Top DPS share median: `44.7%`

## Checks

- [x] browser run success rate: `1` target `>= 0.8`
- [ ] first boss clear rate: `0` target `>= 0.7`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [x] level-ups before first boss: `10` target `>= 8`
- [x] slot fill timing: `15.015` target `<= 150s`
- [x] top DPS share: `0.4468` target `<= 0.5`

## Runs

| run | result | death at | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | running | - | no | - | 11 | 15.2 | hungry_blades | 55.0% |
| 2 | death | 106.94 | no | - | 9 | 12.88 | shatter_ripple | 38.5% |
| 3 | death | 126.07 | no | - | 10 | 14.8 | hungry_blades | 52.4% |
| 4 | running | - | no | - | 11 | 14.83 | shatter_ripple | 63.0% |
| 5 | death | 111.76 | no | - | 10 | 14.82 | hungry_blades | 31.9% |
| 6 | death | 73.61 | no | - | 8 | 13.92 | hungry_blades | 41.0% |
| 7 | running | - | no | - | 11 | 17.32 | hungry_blades | 52.3% |
| 8 | death | 131.75 | no | - | 10 | 22.2 | stopped_second | 32.4% |
| 9 | death | 125 | no | - | 10 | 15.42 | stopped_second | 38.8% |
| 10 | running | - | no | - | 11 | 19.52 | hungry_blades | 48.4% |

