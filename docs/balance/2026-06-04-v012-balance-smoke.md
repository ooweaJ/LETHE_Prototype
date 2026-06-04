# LETHE v0.12 Balance QA

- Generated: 2026-06-04T08:47:08.465Z
- Verdict: `ITERATE_BALANCE`
- Runs: `1`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `100.0%`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `3`
- Slots filled at median: `30.95s`
- Top DPS share median: `70.4%`

## Checks

- [ ] first boss clear rate: `0` target `>= 0.7`
- [ ] clear rate minimum: `0` target `>= 0.35`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [ ] level-ups before first boss: `3` target `>= 8`
- [x] slot fill timing: `30.95` target `<= 150s`
- [ ] top DPS share: `0.7043` target `<= 0.5`

## Runs

| run | result | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | --- | ---: | ---: | ---: | --- | ---: |
| 1 | death | no | - | 3 | 30.95 | hungry_blades | 70.4% |

