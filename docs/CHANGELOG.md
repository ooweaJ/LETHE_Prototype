# LETHE CHANGELOG

## 2026-06-17

- Started `Dev_Prototype_v1 Core Prototype Complete` EPIC.
- Added root development-doc entry files:
  - `docs/PRD.md`
  - `docs/TECH.md`
  - `docs/TASK.md`
  - `docs/TEST.md`
  - `docs/CHANGELOG.md`
- Fixed the next work shape around A-I:
  - A data contracts
  - B hit feel / echo readability
  - C real M2 loop
  - D sprite/VFX replacement
  - E 8 memories
  - F 8 echoes
  - G 4 ultimates
  - H whole-play review
  - I first tuning pass
- Added the first B-step hit-feel/readability pass:
  - Kalmuri echo follow-up now spawns a target-local range ring.
  - Blood echo now creates a red heal thread back to the player when marked enemies are hit again.
  - Blood bloom also emits a heal thread.
  - Enemy knockback cap was raised so greatsword feedback is less compressed.
- Added the first C-step real-loop readability pass:
  - HUD now shows the current M2 loop objective/status.
  - Level-up cards can offer `멈춘 1초` as the third memory after Blood Reflection is acquired.
  - This reduces reliance on automatic review injection for filling the three memory slots.
- Added the first E/F/G runtime expansion pass:
  - Active runtime effects now exist for Execution Flash, Hunter Oath, Shatter Wave, Stopped Second, Ashen Shield, and Oblivion Brand.
  - Weapon-hit echo reactions now exist for the non-Kalmuri/Blood echoes.
  - Added minimal runtime ultimates for Fracture Execution, Stasis Hunt, and Ashen Oblivion in addition to Blood Blade Storm.
  - Reward cards can surface a rotating missing memory candidate.
- Added `_dev/Data` asset skeletons for the complete prototype content set:
  - 8 `MemoryDefinition` assets.
  - 8 `EchoDefinition` assets.
  - 4 `UltimateEchoDefinition` assets.

## 2026-06-16

- Weapon and slash VFX tuning moved toward `_dev/Data/Weapons` ScriptableObject assets.
- `WeaponDefinition`, `WeaponVfxProfile`, and `SlashVfxEntry` became the current weapon/VFX data baseline.
- `Dev_Prototype_v1` remained the active Unity prototype target.
