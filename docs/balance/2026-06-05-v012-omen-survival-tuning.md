# LETHE v0.12 Omen Survival Tuning

Generated: 2026-06-05

## Goal

Improve first-cycle `망각 전조` survival so runs can reach the 180s first boss gate often enough to measure boss TTK.

Targets:

- HP <= 40% median: 140s+
- HP <= 20% median: 170s+
- first-boss reach rate: 70%+

## Change

Added a first-cycle boss-entry relief phase:

- New pressure phase: `문지기 호흡`
- Starts at first-cycle progress `>= 0.94` (about 169.2s before the 180s boss)
- Spawn rate: 1.30s
- Pack size: 1
- Enemy pool: `eroder`, `eroder`, `drifting_eye`
- Spawn cap: 22

Softened first-cycle `망각 전조`:

- first-cycle climax cap: 38 -> 32
- first-cycle climax spawn rate: 0.90 -> 1.08
- first-cycle climax enemy pool no longer adds extra `drifting_eye` and `split_one` beyond the base pool

## Evidence

### Gate Breath Only

- Evidence: `docs/balance/2026-06-05-loop-01-preboss-gate-breath.md`
- Gameplay runs: 4 / 5
- Death rate: 75%
- HP <= 40% median: 127s
- HP <= 20% median: 147.32s

Interpretation: HP thresholds improved, but the relief started too late and did not reduce death rate enough.

### Climax Soften + Gate Breath

- Evidence: `docs/balance/2026-06-05-loop-01-preboss-climax-soften.md`
- Gameplay runs: 5 / 5
- Death rate: 20%
- Death-at median: 180.42s
- HP <= 40% median: 160.58s
- HP <= 20% median: 169.73s
- Level-ups before first boss median: 9
- Top DPS share median: 44.9%

Interpretation: first-cycle `망각 전조` survival reached the target band. Growth pace and top-DPS concentration still pass.

## TTK Follow-Up

Attempted first-boss TTK reruns after the survival tuning:

- `docs/balance/2026-06-05-loop-02-first-boss-climax-soften-ttk.md`: no accepted gameplay sample; first run was browser_error and the command timed out.
- `docs/balance/2026-06-05-loop-02-first-boss-climax-soften-ttk-smoke.md`: command timed out before writing a report.

Conclusion: first-boss TTK remains unmeasured because the long 230s browser/CDP run is currently the tooling bottleneck. The survival tuning should be kept, then TTK should be measured with a more stable long-run QA path.

## Next Target

Stabilize first-boss TTK measurement:

- get at least 3 gameplay samples past 180s,
- capture `bossFights[0].damage`, `firstBossTtk`, and focused DPS,
- tune first boss HP only after those samples exist.
