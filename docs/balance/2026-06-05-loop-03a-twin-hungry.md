# LETHE v0.12 Balance QA

- Generated: 2026-06-05T00:56:37.767Z
- Verdict: `ITERATE_BALANCE`
- Runs: `5`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `60.0%`
- Death at median: `103.59s`
- First boss clear rate: `20.0%`
- First boss TTK median: `6.4s`
- Level-ups before first boss median: `10`
- Slots filled at median: `16.88s`
- Top DPS share median: `49.1%`

## Checks

- [x] browser run success rate: `1` target `>= 0.8`
- [ ] first boss clear rate: `0.2` target `>= 0.7`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `6.4` target `>= 15s`
- [x] first boss TTK upper bound: `6.4` target `<= 30s`
- [x] level-ups before first boss: `10` target `>= 8`
- [x] slot fill timing: `16.88` target `<= 150s`
- [x] top DPS share: `0.4913` target `<= 0.5`

## Runs

| run | result | death at | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | death | 103.59 | no | - | 9 | 15.97 | shatter_ripple | 48.1% |
| 2 | running | - | no | - | 11 | 15.15 | hungry_blades | 50.1% |
| 3 | running | - | yes | 6.4 | 12 | 16.88 | hungry_blades | 49.1% |
| 4 | death | 128.4 | no | - | 10 | 17.32 | shatter_ripple | 55.2% |
| 5 | death | 101.82 | no | - | 10 | 17.43 | shatter_ripple | 39.0% |

