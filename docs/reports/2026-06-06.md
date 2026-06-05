# 2026-06-06-01 - Browser First Boss TTK Terminal

## 1. Current build status

v0.12 remains `ITERATE_BALANCE`, but the first-boss browser TTK sample blocker is resolved. First boss HP remains `2800`.

## 2. What changed today

- `balanceScenario=first_boss_ttk` now completes as soon as the first boss TTK sample is recorded.
- Browser balance QA now preserves the latest page QA payload when the outer poll times out.
- Browser balance QA now applies first-boss-only checks for the `first_boss_ttk` scenario.

## 3. Test results and evidence

- `node --check src/game.js`: pass.
- `node --check scripts/run_browser_balance_qa.js`: pass.
- Browser `first_boss_ttk`: 3/3 accepted samples, first boss clear `100%`, TTK median `25.76s`, verdict `GO_BALANCE_BASELINE`.
- Full browser `qa:balance`: first boss clear `80%`, death `20%`, first boss TTK median `27.79s`, level-ups before first boss median `11`, full clear `0%`, verdict `ITERATE_BALANCE`.
- Evidence: `docs/balance/2026-06-06-v012-browser-first-boss-ttk-terminal.md`, `docs/balance/2026-06-06-v012-balance-qa.md`.

## 4. Decisions made

- Keep first boss HP at `2800`.
- Stop treating first-boss TTK sample stability as the current blocker.
- Move the next balance task to post-boss/full-run flow.

## 5. Problems or risks

- Full clear remains `0%`.
- One full `qa:balance` run died at `156.09s` during the forget-warning phase.
- Four full runs cleared or reached the first boss but still ended incomplete.
- The browser TTK scenario is a measurement path, not proof of full-run player-facing balance.
- `npm run report:discord:unit` was blocked by the approval reviewer because sending workspace reports/attachments to an external Discord webhook is treated as potential private data exfiltration in this Codex session.

## 6. GPT handoff summary

Emotion proxy is excluded. Browser first-boss TTK collection is now stable at HP `2800`: 3/3 accepted and median `25.76s`. Full browser QA now passes first-boss clear and TTK gates, but still fails full-run flow. The next reviewer should inspect post-boss pressure, forget-warning survival, and later-cycle completion rather than asking Codex to guess another first boss HP.

## 7. Next Codex tasks

- Inspect the full QA death at `156.09s` during the forget-warning phase.
- Inspect why post-first-boss runs remain incomplete instead of reaching full clear.
- Tune post-boss pressure, forget-warning survival, or later-cycle pacing separately from first boss TTK.
- Rerun `npm run qa:balance` after the post-boss/full-run adjustment.
- From a trusted local terminal, run `npm run report:discord:unit` if Discord delivery is still required.

## 8. Portfolio notes

- Problem: Browser TTK samples were timing out even after the first boss was defeated.
- Direction: Make the first-boss TTK scenario terminate on the measured boss sample instead of waiting for full-run completion.
- Action: Added scenario-specific terminal and summary checks, then reran browser TTK and full balance QA.
- Result: First-boss TTK evidence is stable, and the next balance problem is now correctly isolated to post-boss/full-run flow.
