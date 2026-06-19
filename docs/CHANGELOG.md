# LETHE CHANGELOG

## 2026-06-19

- Added the `J. 120초 초반 재미 루프` implementation checklist to `docs/TASK.md`.
- Changed the `Dev_Prototype_v1` start overlay from weapon-only selection to four start build cards:
  - `절단쌍검 + 굶주린 칼무리`
  - `절단쌍검 + 피의 반사`
  - `장송대검 + 굶주린 칼무리`
  - `장송대검 + 피의 반사`
- `BeginRun` now accepts a starting memory while preserving the old weapon-only debug path as Hungry Blades default.
- Level-up choices now prioritize missing core memories (`굶주린 칼무리`, `피의 반사`) so either start route can reach the Kalmuri/Blood core loop quickly.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity Play Mode entry reached `isPlaying=true`.
  - Unity console error log count: 0.
  - Note: camera-based Game View screenshots do not capture the OnGUI start overlay, so UI review still needs direct play inspection.
- Continued the 120-second loop through five separate implementation commits:
  - Early kills now grant +1 XP during the first 120 seconds, reducing the wait to the first reward.
  - Weapon hits now add a small confirm ring/core pulse in addition to hit spark and damage numbers.
  - Forgetting result copy now states the lost memory, remaining echo, awakening/overload status, deficit survival, and resonance next action.
  - First-cycle spawn pressure now has a tighter first-120-second profile with closer spawn radius and a capped enemy count.
  - Non-eroder enemy roles now use distinct procedural silhouettes for eye, splitter, priest, and gatekeeper instead of plain colored circles.
  - Verification after each pass: `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed with 7 legacy warnings and 0 errors.
  - Final Unity MCP check: compile error count 0, Play Mode entered, console error count 0, Play Mode stopped.

## 2026-06-18

- Hardened `Dev_Prototype_v1` runtime enemy-list queries:
  - Hungry Blades target selection now skips null enemy entries.
  - Enemy-cap counting now skips null enemy entries.
- Recorded a 30-minute runtime stability pass:
  - Final `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed with 0 warnings and 0 errors.
  - Unity MCP post-patch recheck was blocked by `Transport closed`.
- Reworked first-pass Hungry Blades/Kalmuri readability:
  - Active Hungry Blades now shows a denser two-ring blade swarm instead of a faint short orbit.
  - Each Hungry Blades damage tick now spawns target-local bite blades.
  - Kalmuri echo follow-ups now add an explicit blade barrage on top of the existing ring/slash read.
  - Verification: `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed; Unity compile error count was 0; short Play Mode entry produced 0 console errors.
- Moved `Dev_Prototype_v1` toward the documented full-run game loop:
  - Normal runs now use the documented 600s run, 180/340/490/600s Gatekeeper schedule, 54s deficit survival window, first boss HP 2050, and pressure phase spawn table.
  - Fast/debug paths keep compressed timing for smoke review.
  - Review-only automatic memory/+5 injection now runs only in fast debug mode.
  - Level-up choices now include the six documented run stats: attack speed, damage, area, survival, magnet, and echo amp.
  - Procedural transient VFX, floating text, damage numbers, and XP orbs now use internal object pools instead of constant create/destroy.
  - Verification: `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed; Unity compile error count was 0 after `Assets/Refresh`; short Play Mode entry produced 0 console errors.
- Fixed a pooled VFX regression:
  - `KalmuriSwarmOrbit` could throw `MissingComponentException` because pooled Unity components were fetched with `?? AddComponent`.
  - Replaced Unity component fallback checks with explicit `if (component == null)` checks for SpriteRenderer, fading sprites, floating text, damage numbers, and XP orbs.
  - Verification: `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed; Unity compile error count was 0; short Play Mode entry produced 0 console errors.
- Fixed level-up reward cards rapidly changing every frame:
  - Reward choices are now generated once when level-up starts and cached until the player selects a card.

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
- Fixed a `Dev_Prototype_v1` runtime exception where Blood Bloom and other area effects could modify the `enemies` list while it was being enumerated.

## 2026-06-16

- Weapon and slash VFX tuning moved toward `_dev/Data/Weapons` ScriptableObject assets.
- `WeaponDefinition`, `WeaponVfxProfile`, and `SlashVfxEntry` became the current weapon/VFX data baseline.
- `Dev_Prototype_v1` remained the active Unity prototype target.
