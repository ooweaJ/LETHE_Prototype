# 2026-06-05-08 - First Boss TTK Boss-Only Harness

## 1. Current build status

v0.12 remains `ITERATE_BALANCE`, but the first-boss TTK measurement blocker now has a non-CDP fallback. First boss HP is now `2800`: boss-only TTK is inside target, and the latest accepted browser first-boss TTK sample is also inside target.

## 2. What changed today

- Added `scripts/run_boss_ttk_harness.js`.
- Added `npm run qa:boss-ttk` and `npm run qa:boss-ttk:dry`.
- Fixed the harness `echo.attackSpeed` baseline so weapon attack interval is not `NaN`.
- Changed first boss HP from `780` to `3500`, then to `2800` after browser QA showed HP `3500` was too slow.
- Updated balance automation/status/task docs with the new harness and next gate.

## 3. Test results and evidence

- `node --check src/game.js`: pass.
- `node --check scripts/run_boss_ttk_harness.js`: pass.
- `npm run qa:boss-ttk` at HP `3500`: `GO_BOSS_TTK_SAMPLE`, 5/5 accepted samples, TTK median `21.92s`, focused DPS median `159.7`.
- `node scripts\run_boss_ttk_harness.js --boss-hp 2800`: `GO_BOSS_TTK_SAMPLE`, 5/5 accepted samples, TTK median `17.8s`, focused DPS median `157.3`.
- `npm run qa:balance` at HP `3500`: first boss clear `100%`, death `0%`, TTK median `35.65s`.
- `npm run qa:balance` at HP `2800`: first boss clear `60%`, death `0%`, TTK median `53.21s`, but 2/5 runs were incomplete.
- `node scripts\run_browser_balance_qa.js --scenario first_boss_ttk --runs 3 --run-sec 230 --timeout-ms 60000`: 1/3 accepted TTK sample, TTK `22.59s`; 2/3 runs were incomplete.
- Evidence: `docs/balance/2026-06-05-v012-boss-ttk-hp2800.md`, `docs/balance/2026-06-05-v012-browser-first-boss-ttk-hp2800.md`.

## 4. Decisions made

- Use boss-only deterministic TTK as HP tuning input when Chrome/CDP cannot produce accepted TTK samples.
- Treat HP `2800` as the current first-boss value before the next browser balance QA.
- Do not treat this as human emotion proof or full browser balance proof.

## 5. Problems or risks

- The new harness removes browser/CDP instability, but it also removes movement, enemy clutter, boss attacks, and post-boss flow.
- Browser `qa:balance` still has to prove first-boss reach/clear/death with HP `2800`.
- The previous default run generated a host-date `2026-06-06` balance report as well; the canonical work-unit evidence for this report is the explicit `2026-06-05` file.
- `npm run report:discord:unit` was blocked by the approval reviewer because sending workspace reports/attachments to an external Discord webhook is treated as potential private data exfiltration in this Codex session. Next trusted-local command: `npm run report:discord:unit`.

## 6. GPT handoff summary

Emotion proxy is excluded. First boss HP is now `2800`. Boss-only TTK is `17.8s`; the latest accepted browser first-boss TTK scenario sample is `22.59s`, but browser accepted sample rate is still poor. Next reviewer should judge browser first-boss reach/clear/death separately, because boss-only TTK is not full run balance evidence.

## 7. Next Codex tasks

- Stabilize browser `first_boss_ttk` accepted sample rate with first boss HP `2800`.
- Then rerun browser `npm run qa:balance` with first boss HP `2800`.
- Check first-boss reach rate, clear rate, death phase, and whether the post-boss loop still proceeds.
- If browser flow is too punishing, adjust entry pressure or boss damage separately instead of undoing TTK without evidence.
- From a trusted local terminal, run `npm run report:discord:unit` if Discord delivery is still required.

## 8. Portfolio notes

- Problem: Chrome/CDP could not reliably collect first-boss TTK samples after survival tuning.
- Direction: split boss HP tuning from browser transport by creating a deterministic boss-only measurement path.
- Action: implemented the harness, captured accepted TTK samples, tested HP `3500`, then applied HP `2800`.
- Result: first-boss HP now has a measured target-band basis, while the next gate remains browser balance validation.
