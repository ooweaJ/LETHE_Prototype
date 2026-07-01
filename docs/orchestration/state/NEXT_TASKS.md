# Next Tasks

## 1. Replay updated first 6 minutes

- Priority: highest
- Review the current `Dev_Prototype_v1` build after the boss/no-reacquire patch and the new Kalmuri living swarm pass.
- Kalmuri review focus:
  - Does the original-blade living swarm read as real Kalmuri motion?
  - Confirm the outer rotating Kalmuri layer is gone and only one orbit remains.
  - Confirm each flying blade feels like it owns a damage hit.
  - Confirm the larger lunge range makes blades launch before enemies touch the visible orbit.
  - Confirm the new stagger/pierce spark makes the blades feel like they stab, not drift in.
  - Check irregular orbit, target lunge, bite convergence, recoil shards, echo surge, and level-up spiral.
  - Judge +1 / +3 / +5 readability without hiding enemies or the player.
  - If it still misses, name only one next lever: lunge frequency, trail alpha, blade count, or hit convergence scale.
- Run-loop review focus:
  - First Gatekeeper at `150s`.
  - Second Gatekeeper at `300s`.
  - Gatekeeper HP `2200 / 4200 / 7600 / 12800`.
  - Hard cap `1080s`.
  - Post-forget memory reacquire remains removed.
  - Gatekeeper pulse/guard pattern should feel less free-hit without becoming unfair.
- Done: jaewoo gives GO/ITERATE and at most one next numeric or VFX lever.

## 2. Weapon final feel review

- Priority: high
- Play both dual blades and greatsword in the same current runtime.
- Check whether dual-blade phantom strikes still feel too small after the Kalmuri motion pass.
- Done: weapon timing/visibility is either locked for prototype or one exact tuning target is named.

## 3. Memory/echo comment pass

- Priority: high
- Wait for direct feedback before changing memory/echo values again.
- Done: exact memory/echo ids and desired direction are listed.

## 4. Map identity review

- Priority: high
- Judge the current runtime-generated arena after the prototype combat read is acceptable.
- Done: choose runtime-generated, authored chunks, or larger arena layout.

## 5. Release-prep structure decision

- Priority: medium
- Decide whether `_dev` should continue receiving tuning or whether stable pieces should begin promotion prep.
- Done: GO/ITERATE decision for `Assets/Lethe` promotion prep is recorded.

Keep this file short. Detailed history belongs in `docs/TASK.md`, `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.
