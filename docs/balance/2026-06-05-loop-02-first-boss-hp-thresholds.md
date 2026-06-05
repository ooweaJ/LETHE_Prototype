# LETHE v0.12 Balance QA

- Generated: 2026-06-05T03:09:05.697Z
- Verdict: `ITERATE_BALANCE`
- Runs: `5`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `60.0%`
- Death at median: `141.81s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `9`
- Slots filled at median: `18.11s`
- Top DPS share median: `34.0%`
- Max enemies median: `43`
- HP <= 60% median: `65.13s`
- HP <= 40% median: `90.83s`
- HP <= 20% median: `127.17s`
- Death phase counts: `{"압박 상승":1,"망각 전조":2}`

## Checks

- [x] browser run success rate: `0.8` target `>= 0.8`
- [ ] first boss clear rate: `0` target `>= 0.7`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [x] level-ups before first boss: `9` target `>= 8`
- [x] slot fill timing: `18.11` target `<= 150s`
- [x] top DPS share: `0.3398` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | running | - | - | 46 | - | no | - | 9 | 20.22 | hungry_blades | 31.8% |
| 2 | death | 100.11 | 압박 상승 | 40 | 75.81 | no | - | 9 | 17.75 | hungry_blades | 40.6% |
| 3 | browser_error | - | - | - | - | no | - | 0 | - | - | 0.0% |
| 4 | death | 141.81 | 망각 전조 | 43 | 90.83 | no | - | 9 | 15.9 | hungry_blades | 38.9% |
| 5 | death | 155.09 | 망각 전조 | 43 | 96.86 | no | - | 9 | 18.47 | hungry_blades | 34.0% |

