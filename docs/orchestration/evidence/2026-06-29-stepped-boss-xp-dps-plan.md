# 2026-06-29 Stepped Boss / XP / DPS Plan

Purpose: jaewoo direct review said the first boss arrives too late, the early run feels boring, boss intervals do not create stepwise tension, and the deficit survival system may be unnecessary. This evidence file records a concrete next balance candidate before Unity implementation.

Source command:

```powershell
node scripts/balance_curve_v1.js
```

## Design Decision Candidate

- Remove the separate deficit survival timer from the normal pacing loop.
- Keep forgetting on Gatekeeper clear, grant the echo immediately, then return to normal combat/reward flow without a 54-second low-agency survival pocket.
- Pull the first Gatekeeper from `300s` to `150s`.
- Make later Gatekeeper intervals longer: `150 -> 210 -> 300 -> 360` seconds.
- Compute Gatekeeper HP from expected average boss DPS at that time multiplied by target TTK.

## Boss Plan

| Gate | Time | Interval | Expected level | Echoes before fight | Ultimate | Avg boss DPS | Target TTK | HP |
| --- | ---: | ---: | ---: | ---: | --- | ---: | ---: | ---: |
| 1 | 150s | 150s | 6 | 0 | No | 68 | 18s | 1200 |
| 2 | 360s | 210s | 8 | 1 | No | 86 | 26s | 2250 |
| 3 | 660s | 300s | 11 | 2 | No | 112 | 36s | 4050 |
| 4 | 1020s | 360s | 14 | 3 | Yes | 180 | 48s | 8650 |

## Minute Curve

| Time | Phase | Level | Echoes | Ultimate | Kills | XP to next | Avg field DPS | Spawn/sec | Kill/sec | Cap | Avg enemy HP |
| ---: | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| 0s | 0: 판독 | 1 | 0 | No | 1 | 7 | 37 | 0.95 | 0.88 | 14 | 38 |
| 60s | 1: 첫 문지기 예열 | 4 | 0 | No | 55 | 18 | 45 | 2.63 | 0.85 | 22 | 50 |
| 120s | 1: 첫 문지기 예열 | 5 | 0 | No | 104 | 4 | 48 | 4.17 | 0.76 | 27 | 61 |
| 180s | 2: 선택 검증 | 6 | 1 | No | 151 | 5 | 58 | 3.79 | 0.79 | 31 | 71 |
| 240s | 2: 선택 검증 | 7 | 1 | No | 199 | 25 | 61 | 4.52 | 0.77 | 33 | 78 |
| 300s | 2: 선택 검증 | 8 | 1 | No | 245 | 89 | 64 | 5.34 | 0.75 | 36 | 85 |
| 360s | 3: 잔향 압력 | 8 | 2 | No | 288 | 8 | 73 | 6.25 | 0.68 | 38 | 94 |
| 480s | 3: 잔향 압력 | 10 | 2 | No | 379 | 193 | 80 | 7.59 | 0.74 | 42 | 109 |
| 660s | 4: 궁극 준비 | 11 | 3 | No | 503 | 133 | 93 | 10.00 | 0.64 | 48 | 134 |
| 840s | 4: 궁극 준비 | 13 | 3 | No | 625 | 418 | 101 | 12.16 | 0.65 | 53 | 162 |
| 1020s | 5: 최종 압축 | 14 | 4 | Yes | 736 | 477 | 149 | 14.71 | 0.73 | 58 | 192 |
| 1200s | 5: 최종 압축 | 15 | 4 | Yes | 865 | 461 | 154 | 16.67 | 0.65 | 64 | 245 |

## XP Formula Candidate

```text
initialNextXp = 8

nextXp = round(prevNextXp * 1.32 + 5), level < 10
nextXp = round(prevNextXp * 1.20 + 4), 10 <= level < 16
nextXp = round(prevNextXp * 1.18 + 5), level >= 16
```

Enemy XP value should rise by phase so the first boss is reached around level 6, while later high-HP enemies still move the bar:

| Time band | XP per kill | XP multiplier |
| --- | ---: | ---: |
| 0~60s | 1.18 | 1.08 |
| 60~150s | 1.36 | 1.06 |
| 150~360s | 1.72 | 1.08 |
| 360~660s | 2.72 | 1.06 |
| 660~1020s | 4.18 | 1.02 |
| 1020~1200s | 5.10 | 1.00 |

## Spawn / HP Phase Table

| Time band | Role | Spawn interval | Pack | Cap | Avg enemy HP |
| --- | --- | --- | --- | --- | --- |
| 0~60s | 판독 | 1.05 -> 0.78s | 1 -> 2 | 14 -> 20 | 38 -> 48 |
| 60~150s | 첫 문지기 예열 | 0.76 -> 0.58s | 2 -> 3 | 22 -> 30 | 50 -> 66 |
| 150~360s | 선택 검증 | 0.58 -> 0.48s | 2 -> 3 | 30 -> 38 | 68 -> 92 |
| 360~660s | 잔향 압력 | 0.48 -> 0.40s | 3 -> 4 | 38 -> 48 | 94 -> 132 |
| 660~1020s | 궁극 준비 | 0.40 -> 0.34s | 4 -> 5 | 48 -> 58 | 134 -> 190 |
| 1020~1200s | 최종 압축 | 0.34 -> 0.30s | 5 | 58 -> 64 | 192 -> 245 |

## Expected Effect

- The first meaningful boss goal appears at 2:30 instead of 5:00.
- The run still gets longer between bosses, so the player has more time to assemble and feel the build after each Gatekeeper.
- Difficulty increases through enemy cap, enemy HP, pack size, boss HP, and target TTK instead of only waiting longer.
- Removing deficit survival should reduce downtime. The regret should come from losing the highest memory and seeing the echo remain, not from a separate empty survival phase.
