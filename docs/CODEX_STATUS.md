# Codex Status

Last updated: 2026-06-01

## Current Build

- Project: LETHE: 망각의 군주 HTML Alpha v0.1
- Repository: `https://github.com/ooweaJ/LETHE_Prototype.git`
- Branch: `main`
- Latest pushed commit: `810c567 Initial LETHE HTML alpha prototype`

## Implemented

- Static browser prototype: `index.html`, `style.css`, `src/game.js`
- Weapons: 절단쌍검, 장송대검
- Memories: 6 total, 3 active slots
- Auto basic attack
- Auto memory activation
- Enemy types: 침식자, 떠도는 눈, 쪼개진 자, 공허 사제
- Boss: 기억을 씹는 자, 3 phases
- Dependency-based forgetting
- Echo stat reward after forgetting
- Post-boss question UI
- Forgetting result screen
- Q1/Q2 survey
- JSON log download
- AI alpha test tool under `alpha_test/`
- Codex-GPT workflow docs
- Daily devlog and human-readable report system
- Root `AGENTS.md` for project agent rules

## Latest AI Test Result

Command:

```bash
npm run ai:test
```

Result:

- Verdict: `ITERATE`
- Playability: `튜닝 후 재검증`
- Risk Level: `MEDIUM`
- Alpha Fun Score: `0.8647`
- Regret: `92.7%`
- Irritation: `0.6%`
- Prediction match: `92.7%`
- Immediate quit: `0.9%`
- Restart intent: `70.7%`
- Power drop after forgetting: `29.9%`
- Recovery after replacement: `95.9%`
- First forgetting time: `6.83 min`

Main warnings:

- First forgetting is too early. Target is 8-10 min, current average is 6.83 min.
- Deletion is biased toward `처형자의 섬광` at 46.4%.
- Post-forgetting power drop is slightly low at 29.9%, just below the 30% lower bound.

Sweep note:

- Best tested setting: `echo=0.5`, `ui=0.62`
- Sweep result at that setting: `GO_CANDIDATE`, Alpha Fun Score `0.9053`

## Open Technical Notes

- Browser plugin was unavailable during visual verification, so verification used static checks and simulated runtime checks.
- The game is static HTML and can be run by opening `index.html` or with `python3 -m http.server 8000`.
- Generated AI test outputs are ignored by git under `alpha_test/outputs/`.
- Report HTML can be generated from Markdown with `npm run report`.

## Recommended Next Questions For GPT

- Should first forgetting be delayed in the HTML prototype, or only in the simulation model?
- Is `처형자의 섬광` deletion bias desirable because it creates a clear emotional anchor, or should it be reduced?
- Should echo power default move from `0.60` to `0.50` based on sweep results?
- What should v0.2 validate: stronger loss pain, better predictability, or post-loss pivot?
