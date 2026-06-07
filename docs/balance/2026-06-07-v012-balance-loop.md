# LETHE v0.12 Balance QA

- Generated: 2026-06-07T10:59:58.954Z
- Verdict: `GO_BALANCE_BASELINE`
- Runs: `5`
- Gameplay runs: `5`

## Metrics

- Full clear rate: `60.0%`
- Death rate: `40.0%`
- Death at median: `266.64s`
- First boss clear rate: `100.0%`
- First boss TTK median: `20.73s`
- Level-ups before first boss median: `9`
- Slots filled at median: `27.85s`
- Top DPS share median: `39.3%`
- Max enemies median: `49`
- HP <= 60% median: `108.87s`
- HP <= 40% median: `208.75s`
- HP <= 20% median: `199.62s`
- Death phase counts: `{"망각 전조":1,"결손 압박":1}`

## Checks

- [x] browser run success rate: `1` target `>= 0.8`
- [x] first boss clear rate: `1` target `>= 0.7`
- [x] first boss TTK lower bound: `20.73` target `>= 15s`
- [x] first boss TTK upper bound: `20.73` target `<= 30s`
- [x] clear rate minimum: `0.6` target `>= 0.35`
- [x] clear rate maximum: `0.6` target `<= 0.8`
- [x] death rate maximum: `0.4` target `<= 0.4`
- [x] level-ups before first boss: `9` target `>= 8`
- [x] slot fill timing: `27.85` target `<= 150`
- [x] top DPS share: `0.3932` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | clear | - | - | 49 | - | yes | 15.75 | 9 | 37.88 | hungry_blades | 41.2% |
| 2 | clear | - | - | 49 | 226.9 | yes | 20.73 | 10 | 28.88 | stalker_oath | 39.3% |
| 3 | death | 293.1 | 망각 전조 | 48 | 190.59 | yes | 12.41 | 11 | 27.85 | stalker_oath | 27.8% |
| 4 | death | 240.19 | 결손 압박 | 35 | 139.14 | yes | 27.85 | 9 | 20.12 | shatter_ripple | 52.8% |
| 5 | clear | - | - | 49 | 250.97 | yes | 34.43 | 8 | 20.3 | hungry_blades | 36.8% |

