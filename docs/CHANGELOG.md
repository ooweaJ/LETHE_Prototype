# LETHE CHANGELOG

## 2026-06-22

- Tuned player-held weapon silhouettes and attack VFX scale:
  - dual blade hand sprites increased from the too-small `0.30~0.33` runtime scale to `0.43~0.475`.
  - dual blade local positions moved closer to the player body at `x=±0.19`, `y=-0.035~-0.04`.
  - greatsword hand sprite reduced from `0.44~0.51` to `0.34~0.375`.
  - greatsword local position/swing travel tightened so it no longer dominates the character silhouette.
  - dual-blade attack PNG scale factor increased; greatsword cleave PNG scale factor reduced.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: 0.
  - Play Mode runtime transform check: dual blades `0.43`, greatsword `0.34`.
  - Unity console error count: 0.

- Strengthened Blood Blade Storm payoff in `Dev_Prototype_v1`:
  - added an opening cue, continuous storm pressure, and periodic burst pulses.
  - dual-blade storm now uses faster 8-blade orbit ticks and 12-blade bursts.
  - greatsword storm now uses heavier rings/slashes, stronger burst damage, hitstop, camera shake, knockback, healing, and blood-heal threads.
  - storm damage now marks enemies and uses ASCII `BloodBladeStorm` source strings to avoid mojibake-string fragility.
- Improved player walking feel:
  - raw input is smoothed into short acceleration/deceleration.
  - `PlayerVisual` now gets subtle walk bob/tilt.
  - movement-driven weapon-anchor rotation is smoothed to reduce direction snapping.
- Added a defensive `BeginRun` guard so debug/smoke calls can create the player if invoked before the cached player reference exists.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity console error count: 0 after direct M2/ultimate reflection smoke.
  - Direct `UpdateEchoUltimate(0.12f)` reflection ticks created `bloodStormObjects=124` and cleared nearby enemies with `kills=14`.

- Wired generated VFX sprites into the `Dev_Prototype_v1` runtime:
  - weapon/hit VFX: dual blade arcs, greatsword cleave arc, cyan/red hit sparks.
  - active memory VFX: Execution Flash, Hunter Oath, Shatter Wave, Stopped Second, Ashen Shield, Oblivion Brand.
  - echo VFX: Execution, Homing, Shockwave, TimeStop, Ashen, Brand.
  - ultimate VFX: Fracture Execution, Stasis Hunt, Ashen Oblivion.
- Updated weapon slash profile resolution so `SlashVfxEntry` values use generated PNG sprites first and procedural sprites only as fallback.
- Added generated-sprite scale normalization for the 1254px prompt-sheet images so VFX fit the existing combat field.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity `Assets/Refresh`: success.
  - Unity compile error count: 0.
  - Unity Play Mode smoke attempts produced console error count 0.
- Remaining review need:
  - Unity screenshot/capture still produced solid-color output, so direct play review must judge final VFX size, timing, brightness, and hit-position readability.

## 2026-06-21

- Generated the remaining prompt-sheet VFX sprites:
  - weapon/hit VFX: dual blade arcs, greatsword cleave arc, cyan/red hit sparks.
  - active memory VFX: Execution Flash, Hunter Oath, Shatter Wave, Stopped Second, Ashen Shield, Oblivion Brand.
  - echo VFX: Execution, Homing, Shockwave, TimeStop, Ashen, Brand.
  - ultimate VFX: Fracture Execution, Stasis Hunt, Ashen Oblivion.
- Preserved matching chroma-key sources under `LETHE/Assets/_dev/Art/Source/`.
- Added visual evidence contact sheet:
  - `LETHE/Assets/_dev/Evidence/remaining_vfx_prompt_sheet_20260621.png`.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity AssetDatabase found 20/20 generated VFX textures.
  - Unity import settings confirmed 20/20 final PNGs as Sprite textures.
- Added `docs/design/LETHE_SPRITE_PRODUCTION_PROMPTS.md` as the clean sprite-generation source sheet.
- Replaced the existing core Kalmuri/Blood/Blood Blade Storm VFX sprites using that prompt sheet:
  - `spr_kalmuri_orbit_blade_01.png`
  - `spr_kalmuri_echo_slash_01.png`
  - `spr_kalmuri_launch_blade_01.png`
  - `spr_blood_mark_01.png`
  - `spr_blood_bloom_01.png`
  - `spr_heal_thread_tip_01.png`
  - `spr_blood_blade_storm_ring_01.png`
  - `spr_blood_blade_storm_blade_01.png`
- Preserved matching chroma-key source PNGs under `LETHE/Assets/_dev/Art/Source/`.
- Added visual evidence contact sheet:
  - `LETHE/Assets/_dev/Evidence/core_vfx_prompt_sheet_refresh_20260621.png`.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity AssetDatabase lists Kalmuri 3, Blood 3, and Ultimate 2 replacement textures.

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
- Added direct Codex smoke menus under `LETHE/V1 Smoke/*`:
  - Four start build routes can be initialized from Unity Editor menus and logged through `DebugSnapshot()`.
  - The M2 loop can be initialized from an editor menu and verifies Hungry/Blood echoes at +5 with Blood Blade Storm ready.
  - Verification: `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed with 0 warnings and 0 errors; Unity compile errors 0; Unity console errors 0; scene/assets missing references 0.
- Corrected the start-selection UX after review:
  - The first overlay now selects only the starting weapon: `절단쌍검` or `장송대검`.
  - `굶주린 칼무리` and `피의 반사` are no longer attached to the weapon card.
  - First level-up rewards now carry the memory choice; verified first choices as `굶주린 칼무리 | 피의 반사 | 칼날 가속`.
  - `LETHE/V1 Smoke/*` start menus now match the weapon-only model: `Start Dual Blades`, `Start Greatsword`, and `M2 Loop`.
  - Verification: `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed with 7 legacy warnings and 0 errors; Unity compile errors 0; Unity console errors 0.

- Refreshed v1 visual/UI/game-feel basics:
  - Removed player body pulse wobble and moved sprite rendering under a stable `PlayerVisual` child.
  - Added/imported a new player body sheet:
    - `LETHE/Assets/_dev/Art/Source/sheet_player_v1_4dir_chroma.png`
    - `LETHE/Assets/_dev/Art/Sprites/Characters/Player/sheet_player_v1_4dir.png`
  - Reused the new 8x4 player sheet as actual idle/walk 4-direction animation.
  - Centered the weapon anchor to reduce perceived body drift while moving.
  - Added imported greatsword sprite assets:
    - `LETHE/Assets/_dev/Art/Source/spr_weapon_greatsword_01_chroma.png`
    - `LETHE/Assets/_dev/Art/Sprites/Weapons/spr_weapon_greatsword_01.png`
  - Added arena floor tile rotation/color/scale variation.
  - Compact HUD now emphasizes HP, XP, memory slots, ultimate/M2 status, and smaller debug controls.
  - Verification: `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed with 7 legacy warnings and 0 errors; Unity compile errors 0; Unity console errors 0; missing references scene 0/assets 0.

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
