# LETHE v0.12 Balance QA

- Generated: 2026-06-05T05:39:22.548Z
- Verdict: `ITERATE_BALANCE`
- Runs: `1`
- Gameplay runs: `1`

## Metrics

- Full clear rate: `0.0%`
- Death rate: `100.0%`
- Death at median: `179.32s`
- First boss clear rate: `0.0%`
- First boss TTK median: `-s`
- Level-ups before first boss median: `9`
- Slots filled at median: `18.95s`
- Top DPS share median: `34.0%`
- Max enemies median: `48`
- HP <= 60% median: `138.18s`
- HP <= 40% median: `148.35s`
- HP <= 20% median: `165.62s`
- Death phase counts: `{"망각 전조":1}`

## Checks

- [x] browser run success rate: `1` target `>= 0.8`
- [x] first boss clear rate: `0` target `>= 0`
- [x] clear rate minimum: `0` target `>= 0`
- [x] clear rate maximum: `0` target `<= 0.8`
- [ ] first boss TTK lower bound: `-` target `>= 15s`
- [x] first boss TTK upper bound: `-` target `<= 30s`
- [x] level-ups before first boss: `9` target `>= 8`
- [x] slot fill timing: `18.95` target `<= 150s`
- [x] top DPS share: `0.34` target `<= 0.5`

## Runs

| run | result | death at | phase | max enemies | hp40 | first boss | ttk | level-ups | slots filled | top DPS | share |
| --- | --- | ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | --- | ---: |
| 1 | death | 179.32 | 망각 전조 | 48 | 148.35 | no | - | 9 | 18.95 | hungry_blades | 34.0% |

