# Current Task

## Goal

Run controlled human sessions for the current v0.12 HTML prototype candidate and summarize the downloaded JSON logs.

## Why Now

The v0.12 balance-loop baseline is accepted for human testing. More blind numeric tuning should wait until human evidence shows what is actually weak.

## Done Criteria

- At least one controlled human session has been run.
- Downloaded JSON logs are placed in `playtest_logs/`.
- Human observation notes are recorded using `docs/PLAYTEST_NOTES.md` or a session-specific note.
- `npm run playtest:summary` has been run after logs are available.
- Resulting summary is used to update orchestration status, devlog, reports, and a planning/review prompt if direction changes are needed.

## Related Files

- `dist\lethe-v0.12-playtest`
- `docs/HUMAN_PLAYTEST_GUIDE.md`
- `docs/PLAYTEST_NOTES.md`
- `docs/playtest/2026-06-07-v012-human.md`
- `playtest_logs/`
- `scripts/summarize_playtests.js`

## Verification Commands

```bash
npm run playtest:package
npm run playtest:summary
npm run report
npm run report:check
npm run doctor
```

## Open Questions

- Do players read the first 2-3 minutes as fun, neutral, boring, or unclear?
- Do level-up choices feel meaningful or random?
- Does first forgetting create regret/acceptance rather than irritation?
- Does the 2-memory deficit segment feel tense but fair?
- Does refill feel like adaptation instead of a full reset?
- Does the prototype feel promising enough for a future Unity version?

## Do Not Touch

Do not add new memories, slots, shop, meta progression, region structure, weapon expansion, final boss expansion, or additional blind balance tuning before human evidence unless the user explicitly changes scope.
