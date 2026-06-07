# LETHE v0.12 Balance QA

- Generated: 2026-06-07T10:58:21.037Z
- Verdict: `ITERATE_BALANCE`
- Runs: `10`
- Gameplay runs: `10`

## Metrics

- Full clear rate: `80.0%`
- Death rate: `20.0%`
- Death at median: `332.2s`
- First boss clear rate: `100.0%`
- First boss TTK median: `35.39s`
- Level-ups before first boss median: `9`
- Slots filled at median: `22.23s`
- Top DPS share median: `37.4%`
- Max enemies median: `49`
- HP <= 60% median: `157.44s`
- HP <= 40% median: `239.2s`
- HP <= 20% median: `322.41s`
- Death phase counts: `{"압박 상승":2}`

## Checks

- [x] browser run success rate: `1` target `>= 0.8`
- [x] first boss clear rate: `1` target `>= 0.7`
- [x] first boss TTK lower bound: `35.39` target `>= 15s`
- [ ] first boss TTK upper bound: `35.39` target `<= 30s`
- [x] clear rate minimum: `0.8` target `>= 0.35`
- [x] clear rate maximum: `0.8` target `<= 0.8`
- [x] death rate maximum: `0.2` target `<= 0.4`
- [x] level-ups before first boss: `9` target `>= 8`
- [x] slot fill timing: `22.225` target `<= 150`
- [x] top DPS share: `0.3742` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | clear | - | - | 49 | - | yes | 31.55 | 9 | 12.17 | hungry_blades | 32.5% |
| 2 | clear | - | - | 49 | 239.2 | yes | 39.23 | 9 | 17.27 | weapon_evolution | 24.0% |
| 3 | clear | - | - | 49 | - | yes | 54.94 | 9 | 26.42 | hungry_blades | 35.9% |
| 4 | clear | - | - | 49 | - | yes | 25.25 | 10 | 31.7 | hungry_blades | 44.0% |
| 5 | clear | - | - | 49 | - | yes | 65.41 | 7 | 21.13 | hungry_blades | 48.5% |
| 6 | death | 253.56 | 압박 상승 | 48 | 233.18 | yes | 20.68 | 10 | 22.58 | shatter_ripple | 52.5% |
| 7 | clear | - | - | 49 | 260.32 | yes | 51.22 | 8 | 18.97 | stopped_second | 25.8% |
| 8 | death | 410.84 | 압박 상승 | 49 | 248.13 | yes | 16.53 | 11 | 37.05 | hungry_blades | 36.5% |
| 9 | clear | - | - | 49 | 236.05 | yes | 15.68 | 10 | 21.87 | hungry_blades | 38.5% |
| 10 | clear | - | - | 49 | - | yes | 39.83 | 9 | 49.9 | hungry_blades | 38.4% |

