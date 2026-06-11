# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, legacy `docs/NEXT_TASKS.md`, or evidence files.

## 1. Phase 1 Camera/Scale/Composition

- Priority: high
- Include: camera orthographic size, follow settings, player/enemy/weapon scale, weapon anchor, sorting, arena bounds, compact debug panel.
- Done: opening Play Mode immediately looks like a small playable arena, not a debug board.

## 2. Player Survival Loop

- Priority: high
- Include: player HP, enemy contact damage, player hit flash, debug revive/reset.
- Done: enemy chasing creates actual danger instead of only visual pressure.

## 3. Nearest Enemy Targeting

- Priority: high
- Include: dual blades pick nearest living enemy within range instead of one hard-coded target.
- Done: combat still works after adding multiple enemies.

## 4. Multi-Enemy Spawn Loop

- Priority: high
- Include: 3-8 test enemies, respawn around player, death replacement.
- Done: the scene feels like a small arena test rather than a duel.

## 5. Real Echo Damage

- Priority: medium
- Include: Kalmuri/Blood/Storm VFX create `EchoHit` events through `HitResolver`, with recursive echo blocked.
- Done: echoes affect combat, not only visuals.
