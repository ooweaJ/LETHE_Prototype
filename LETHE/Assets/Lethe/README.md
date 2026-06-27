# LETHE Runtime Promotion Area

This folder is the candidate home for beta-facing runtime assets promoted from
`Assets/_dev` after play review.

Current rule for this pass:

- Keep `Dev_Prototype_v1` and echo/memory debug review tools in `_dev`.
- Use this folder to prepare the product-facing structure only.
- Promote scene, prefab, data, UI, art, and audio assets here once a specific
  item is stable enough to stop being experimental.

Suggested structure:

- `Scenes/`: player-facing preview and later beta scenes.
- `Prefabs/`: player, enemy, weapon, VFX, UI, and arena prefabs.
- `Data/`: catalogs, definitions, balance tables, and run config.
- `Art/`: runtime sprites and visual source exports.
- `UI/`: reusable UI prefabs, fonts, and style assets.
- `Audio/`: SFX and music assets.
- `Runtime/`: non-prototype runtime scripts after class split.
