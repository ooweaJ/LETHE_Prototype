# Codex Status

Last updated: 2026-06-02

## Current Build

- Project: LETHE HTML Alpha v0.2 tuning candidate.
- Repository: `https://github.com/ooweaJ/LETHE_Prototype.git`
- Branch: `main`
- Current scope: forgetting loop emotional validation. No new content expansion yet.

## Implemented

- Static browser prototype: `index.html`, `style.css`, `src/game.js`.
- Weapons: twin blades, greatsword.
- Memories: 6 total, 3 active slots.
- Auto basic attack and auto memory activation.
- Enemy waves and boss encounter.
- Dependency-based forgetting with per-memory deletion bias.
- Echo stat reward after forgetting, default experiment echo power `0.50`.
- Post-boss prediction question UI.
- Forgetting result screen with clearer summary:
  - forgotten memory,
  - prediction result,
  - deletion weight,
  - remaining echo,
  - next build direction.
- Q1/Q2 survey plus Q3 memory-name recall free response.
- JSON log download with selected/predicted/deleted memory names and deletion weights.
- AI alpha test tool under `alpha_test/`.
- Codex/GPT/Claude workflow docs.
- Markdown daily reports, generated HTML reports, and Discord report delivery.
- Short Discord status notices for Codex work.
- Claude Code planning-review automation.

## Latest AI Test Result

Command:

```bash
npm run ai:test
```

Result:

- Verdict: `GO_CANDIDATE`
- Playability: `AI 기준 사람 테스트 진입 가능`
- Risk Level: `LOW`
- Alpha Fun Score: `0.7737`
- Regret proxy: `76.3%`
- Irritation proxy: `1.1%`
- Prediction match: `76.3%`
- Immediate quit: `0.8%`
- Restart intent: `70.0%`
- First forgetting time: `9.00 min`
- Post-forgetting power drop: `29.6%`
- Recovery after replacement: `96.6%`
- Max single memory deletion share: `34.1%`

Remaining note:

- Power drop is just under the 30% target. It is close enough for human-test candidate status, but v0.3 should watch whether the loss feels too safe.

## Latest Sweep Note

Command:

```bash
npm run ai:sweep
```

- Best score in this sweep: `echo=0.40`, `ui=0.78`, score `0.8424`, verdict `ITERATE`.
- Best `GO_CANDIDATE` around the current preset: `echo=0.50`, `ui=0.78`, score `0.8245`.
- Current implementation stays at `echo=0.50`, `ui=0.62` because that matches the GPT/Claude v0.2 instruction and keeps the tuning narrow.

## Open Technical Notes

- Browser visual verification has started in this session:
  - start screen and run start flow were checked,
  - result screen and JSON download still need full browser confirmation.
- The game is static HTML and can be run by opening `index.html`.
- Generated AI test outputs are ignored by git under `alpha_test/outputs/`.
- Report HTML can be generated from Markdown with `npm run report`.
- Discord report delivery can be previewed with `npm run report:discord:dry`.
- Local `.env` and `.env.*` are ignored by Git.
- No tracked `.env.example` is required.

## Latest Planning Verdict

- GPT verdict: `ITERATE_BEFORE_TEST`.
- Claude verdict: `ITERATE_BEFORE_TEST`.
- v0.2 scope: timing, deletion distribution, echo default, clearer feedback, JSON logs, human-test recall question.
- No additional GPT/Claude question is required before the next human-test preparation pass.

## Next Codex Tasks

- Review the v0.2 result screen visually in browser.
- Confirm the downloaded JSON log fields in browser.
- Use `docs/HUMAN_PLAYTEST_GUIDE.md` for the 5-8 player human test.
- During human test, focus on whether the forgotten memory feels regrettable, understandable, and recoverable.
