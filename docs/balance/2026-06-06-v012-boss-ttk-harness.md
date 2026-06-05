# LETHE v0.12 First Boss TTK Harness

- Generated: 2026-06-05T15:04:34.780Z
- Verdict: `GO_BOSS_TTK_SAMPLE`
- Current first boss HP: `3500`
- Recommended first boss HP for 22.5s target: `3593`

## Metrics

- Accepted samples: `5/5`
- First boss TTK median: `21.92s`
- Focused DPS median: `159.7`

## Checks

- [x] accepted gameplay samples: `5` target `>= 3`
- [x] first boss TTK lower bound: `21.92` target `>= 15s`
- [x] first boss TTK upper bound: `21.92` target `<= 30s`

## Runs

| run | status | ttk | focused DPS | top DPS | share |
| --- | --- | ---: | ---: | --- | ---: |
| 1 | complete | 21.92 | 159.7 | weapon | 41.2% |
| 2 | complete | 21.92 | 159.7 | weapon | 41.2% |
| 3 | complete | 21.92 | 159.7 | weapon | 41.2% |
| 4 | complete | 21.92 | 159.7 | weapon | 41.2% |
| 5 | complete | 21.92 | 159.7 | weapon | 41.2% |

## Interpretation

This is an in-process boss-only harness that mirrors the v0.12 first-boss TTK scenario without Chrome/CDP. It is measurement input for first boss HP tuning, not a replacement for later browser balance QA.

