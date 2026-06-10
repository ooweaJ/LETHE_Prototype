# Scope Guard

Until the user or planning review explicitly changes scope, do not add:

- Meta progression.
- Shop systems.
- Final boss expansion.
- More than the current memory scope.
- More than 3 active memory slots.
- Multi-region run structure.
- Additional bosses.
- Large weapon expansion.
- Memory synthesis, strengthening, or upgrade systems.
- Complex ending branches.
- Narrative cutscene expansion.
- Save/load campaign structure.
- Log analysis dashboard as gameplay scope.
- Difficulty selection.
- Character selection.
- Unlock systems.

## Current Unity Gate

The current gate is the first Unity `_dev` game slice. The slice should prove that `절단쌍검 + 칼무리 잔향 + 혈반 잔향 + 피의 칼폭풍` can be read as combat action, not text labels.

GO requires:

- Basic player/enemy/map/weapon readability in `Dev_EchoSlice`.
- Data-driven `WeaponDefinition`, `MemoryDefinition`, `EchoDefinition`, and `EchoSynergyDefinition` foundations.
- Prefabs wired through definitions rather than hard-coded ids.
- Debug states for basic attack, Kalmuri +1/+5, Blood +5, and Blood Blade Storm.
- jaewoo review evidence recorded in orchestration docs.

## Current Tuning Rule

Do not continue blind numeric tuning before regression evidence and human session evidence unless the user explicitly changes scope. If regression fails, choose exactly one small lever.
