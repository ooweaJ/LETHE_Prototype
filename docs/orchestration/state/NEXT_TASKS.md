# Next Tasks

## 2026-06-30 Update: Next Priority After Boss Pattern Patch

## 1. Replay updated first 6 minutes

- Priority: highest
- The jaewoo direct-play follow-up is now applied to Unity runtime.
- The intro / starting weapon screen was also refreshed. During the replay, first confirm whether the LETHE title, first-goal strip, and two weapon cards make the build feel like a real game start rather than a debug picker.
- Current values:
  - Gatekeeper schedule: `150 / 300 / 540 / 900s`.
  - Gatekeeper HP: `2200 / 4200 / 7600 / 12800`.
  - Hard cap: `1080s`.
  - Post-forget memory reacquire/refill: removed.
  - Gatekeeper pulse/guard pattern: added.
- Review checklist:
  - Does the refreshed Hungry Blades read as a real `칼무리`: orbiting blades, outer trace, bite impact, and level-up burst?
  - Do +1 / +3 / +5 Hungry Blades stay readable without hiding enemies or the player?
  - Is the first Gatekeeper still good at 2:30 after the HP increase?
  - Is the second Gatekeeper at 5:00 no longer late?
  - Does the pulse/guard pattern make the boss less free-hit without feeling unfair?
  - Are HP bars stable and readable?
  - Is forgetting cleaner now that the lost memory does not come back?
  - Does Hungry Blades keep rotating through base attack hitstop?
  - Does the intro explain weapon-only start clearly, or should starting memory preview return to the start panel later?
- Done: jaewoo identifies at most one next numeric lever: first boss HP, second boss timing, pulse damage/frequency, or late boss HP.

Keep this file short. Detailed history belongs in `docs/TASK.md`, `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.

Current source of truth:

- Product direction: `docs/PRD.md`
- Technical structure: `docs/TECH.md`
- Active work board: `docs/TASK.md`
- Test gate: `docs/TEST.md`
- Detailed design: `docs/design/README.md` and `docs/design/LETHE_DESIGN_00..07`

## 1. 20-minute beta balance playthrough review

- Priority: highest
- 2026-06-29 재확인: 씬 누락 참조를 v1 scene rebuild로 복구했고, compile `0`, scene/assets missing references `0`, console errors `0` 상태에서 dual blades, greatsword, M2, VFX Matrix, Blood Blade Storm QA가 모두 `[V1QA] PASS`했다.
- Review sheet: `docs/orchestration/review_prompts/2026-06-29-jaewoo-beta-run-review.md`
- MCP automated QA on 2026-06-27 passed the technical gate: compile `0`, missing references `0`, console errors `0`, dual/greatsword/M2 smoke routes initialized, and 8 memory/8 echo VFX previews spawned.
- Reliable QA line is now stronger: dual blades, greatsword, M2, VFX Matrix, and Blood Blade Storm all log explicit `[V1QA] PASS` from Unity MCP.
- Include: normal play with debug panel hidden, both starting weapons, first reward timing, first Gatekeeper at 300s, first forgetting around 5m, 1 ultimate around 15~16m, and fourth Gatekeeper clear around 19~20m.
- Also check: the new HUD echo/objective lines are helpful rather than noisy.
- Check: whether the new 20-minute tempo feels satisfying, whether early XP is no longer too fast, and whether 1 ultimate + final Gatekeeper is a clear enough completion rule.
- Done: jaewoo can say GO/ITERATE for the 20-minute beta balance line.

## 2. Weapon final feel review

- Priority: high
- Include: play dual blades and greatsword in the full run, not only debug presets.
- Check: whether greatsword blocks the view, whether its slash appears fast enough, whether dual blades have enough quick-hit identity, and whether greatsword route clears are less stable than dual blades in real play.
- Done: weapon timing/visibility is either locked for prototype or specific values are named for tuning.

## 3. Memory/echo comment pass

- Priority: high
- Include: jaewoo deferred detailed memory/echo comments; wait for direct feedback before changing values again.
- Check: which ids are invisible, too similar, too weak, too strong, or too noisy.
- Done: exact memory/echo ids and desired direction are listed.

## 4. Map identity review

- Priority: high
- Include: judge the current terrain later, after prototype completion is reviewed.
- Check: whether map should stay runtime-generated, move to hand-authored chunks, or grow into a larger arena layout.
- Done: map direction is chosen.

## 5. Release-prep structure decision

- Priority: medium
- Include: decide whether `_dev` should keep getting tuned or whether the new `Assets/Lethe` promotion-prep structure should start receiving stable prefabs/data after full playthrough review.
- Check: remaining prototype-only runtime generation, missing audio, authored prefabs, scene organization, and asset naming.
- Done: GO/ITERATE decision for promotion prep is recorded.
# 2026-06-29 Update: Next Priority After Patch

## 1. Retry V1QA smoke and first-6-minute review

- Priority: highest
- Stepped boss / XP / DPS curve is now applied to Unity runtime.
- Retry V1QA menu line when MCP menu execution stops returning `Error polling queue: fetch failed`:
  - `LETHE/V1 Smoke/Start Dual Blades`
  - `LETHE/V1 Smoke/Start Greatsword`
  - `LETHE/V1 Smoke/M2 Loop`
  - `LETHE/V1 QA/VFX Matrix`
  - `LETHE/V1 QA/Blood Blade Storm`
- Then run jaewoo first-6-minute review:
  - Is first boss at ~2:30 a better hook?
  - Does the first forgetting/echo moment arrive soon enough?
  - Does immediate refill feel cleaner than deficit survival?
  - Is enemy count/HP readable, not empty or oppressive?
- Done: jaewoo can say whether this pacing line is `GO` or needs one numeric adjustment.

# 2026-06-29 Update: Next Priority

## 1. Apply stepped boss / XP / DPS curve

- Priority: highest
- jaewoo direct review found the current first Gatekeeper at `300s` too late and the early run too boring.
- Use the new calculation evidence: `docs/orchestration/evidence/2026-06-29-stepped-boss-xp-dps-plan.md`.
- Candidate values:
  - Gatekeeper schedule: `150 / 360 / 660 / 1020s`.
  - Gatekeeper HP: `1200 / 2250 / 4050 / 8650`.
  - Target boss TTK: `18 / 26 / 36 / 48s`.
  - Hard cap: `1200s`.
  - Remove the separate `54s` deficit survival pocket from normal pacing.
- Implement only this pacing/balance axis first; do not add new weapons, memories, bosses, shop, meta progression, or region structure.
- Done: Unity runtime uses the stepped curve, technical QA passes, and jaewoo can replay the first 6 minutes to judge whether the early run is no longer dull.
