# Double Check Summary - 2026-06-02-v09-release-feel-loop

## Prompt

- docs/review_prompts/2026-06-02-v09-release-feel-loop.md

## Responses

- Claude: docs/review_responses/2026-06-02-v09-release-feel-loop-claude.md
- Codex CLI: docs/review_responses/2026-06-02-v09-release-feel-loop-codex.md

## Decision Rules

- User live play feedback outranks AI planning opinions.
- Browser combat evidence outranks aggregate AI proxy metrics.
- If Claude and Codex disagree, Codex should summarize the conflict before implementation.
- Large design changes require both responses to be read before editing game code.
- A `GO` from either AI is only `AI planning pass` until browser combat QA or user play validates it.

## Codex Synthesis Required

- [x] Common recommendations:
  - Verdict is `BUILD_V09_RELEASE_FEEL_LOOP`.
  - Do not proceed directly to v0.8 Gate C or broad human testing.
  - v0.9 must make the player understand their build identity within roughly 90 seconds.
  - Existing 6 memories and 3 active slots are enough; improve role labels, tags, synergies, and combat feedback before adding content.
  - The first implementation unit should be build identity/readability, then post-loss challenge, then Echo Zone or another small tactical agency layer.
  - AI verdicts remain planning/verification signals, not human-fun approval.
- [x] Conflicts:
  - Claude proposes more immediate feel changes in the same batch: post-loss breathing window, deficit passive, boss tell, and Echo Zone recovery pulse.
  - Codex CLI recommends a stricter sequence: build identity first, then post-loss challenge, then Echo Zone, with new metrics such as `buildIdentitySeen`, `synergyActiveTime`, `lossChallengeCompleted`, and `echoZoneUseCount`.
  - Selected resolution: keep Claude's feel ideas as candidates, but implement Codex CLI's order so the first v0.9 loop does not sprawl.
- [x] Selected vNext scope:
  - v0.9 Work Package 1 only: existing 6-memory build identity pass.
  - Add/clarify each memory's role, tag, and short combat description.
  - Show current build name, active synergy, and most-dependent memory in the selection/HUD flow.
  - Add JSON/AI payload hooks for build identity visibility before implementing Echo Zone or post-loss challenge.
- [x] Tests required before reporting balance:
  - `npm run ai:test:quick`.
  - `npm run ai:test`.
  - A browser/headless QA path that confirms 90-second build identity and synergy display.
  - JSON payload must include v0.9 identity fields before any future "balance pass" is reported.
  - Manual 1-person testing remains blocked until Work Package 1-3 have at least one browser-verified build.
