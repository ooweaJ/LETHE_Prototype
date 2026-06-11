# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, legacy `docs/NEXT_TASKS.md`, or evidence files.

## 1. Player Survival Loop

- Priority: high
- Include: player HP, enemy contact damage, player hit flash, debug revive/reset.
- Done: enemy chasing creates actual danger instead of only visual pressure.

## 2. Nearest Enemy Targeting

- Priority: high
- Include: dual blades pick nearest living enemy within range instead of one hard-coded target.
- Done: combat still works after adding multiple enemies.

## 3. Multi-Enemy Spawn Loop

- Priority: high
- Include: 3-8 test enemies, respawn around player, death replacement.
- Done: the scene feels like a small arena test rather than a duel.

## 4. Real Echo Damage

- Priority: medium
- Include: Kalmuri/Blood/Storm VFX create `EchoHit` events through `HitResolver`, with recursive echo blocked.
- Done: echoes affect combat, not only visuals.

## 5. Data Asset Binding

- Priority: medium
- Include: create and bind `WeaponDefinition`, `MemoryDefinition`, `EchoDefinition`, and `EchoSynergyDefinition` assets for dual blades, Kalmuri, Blood, and Blood Blade Storm.
- Done: adding another weapon/memory later does not require hard-coded debug references.
