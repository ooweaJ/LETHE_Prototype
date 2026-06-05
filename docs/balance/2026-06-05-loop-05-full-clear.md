# LETHE v0.12 Balance QA

- Generated: 2026-06-05T01:00:20.983Z
- Verdict: `ITERATE_BALANCE`
- Runs: `5`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `80.0%`
- Death at median: `136.17s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `10`
- Slots filled at median: `14.48s`
- Top DPS share median: `40.9%`

## Checks

- [x] browser run success rate: `0.8` target `>= 0.8`
- [ ] first boss clear rate: `0` target `>= 0.7`
- [ ] clear rate minimum: `0` target `>= 0.35`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [x] level-ups before first boss: `10` target `>= 8`
- [x] slot fill timing: `14.485` target `<= 150s`
- [x] top DPS share: `0.4095` target `<= 0.5`

## Runs

| run | result | death at | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | death | 146.85 | no | - | 10 | 12.22 | hungry_blades | 40.9% |
| 2 | browser_error | - | no | - | 0 | - | - | 0.0% |
| 3 | death | 125.49 | no | - | 10 | 15.42 | shatter_ripple | 38.4% |
| 4 | death | 110.07 | no | - | 10 | 16.02 | hungry_blades | 48.0% |
| 5 | death | 168.72 | no | - | 11 | 13.55 | hungry_blades | 61.7% |

