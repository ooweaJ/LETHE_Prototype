# Next Tasks

Keep this file short. Detailed history belongs in `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, legacy `docs/NEXT_TASKS.md`, or evidence files.

## 1. Jaewoo Prototype v0 Hands-On Review

- Priority: highest
- Source: `docs/design/LETHE_UNITY_PROTOTYPE_V0_PRD.md`
- Include: open `Assets/_dev/Scenes/Dev_Prototype_v0.unity`, press Play, move, fight, use F1-F5 debug jumps, judge camera/combat/memory/echo/ultimate feel.
- Done: record GO / ITERATE / NO-GO and the first weakest point.

## 2. Prototype Balance Pass

- Priority: highest
- Include: player contact damage, enemy health, auto-attack cadence, Blood Blade Storm damage/heal.
- Done: 60 seconds is tense without instant collapse or instant full-screen deletion.

## 3. Sprite Sheet Cleanup

- Priority: high
- Include: regenerate or clean player/enemy 4-direction sheets if jaewoo says direction frames are unclear.
- Done: idle/walk direction reads at current camera size.

## 4. Echo VFX Upgrade

- Priority: high
- Include: replace line-renderer placeholder effects with sprite/pool based Kalmuri slash, blood bloom, heal thread, orbit ring.
- Done: echo form transformation reads before text.

## 5. Data-Driven Runtime Refactor

- Priority: high
- Include: move prototype hard-coded branches toward `RunBuildState`, Definition assets, runtime prefabs, and trigger routing.
- Done: adding another memory/echo does not require editing `PrototypeGameManager` core branches.
