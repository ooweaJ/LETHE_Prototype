# LETHE CHANGELOG

## 2026-06-23

- Strengthened Stopped Second as a gold clock-field VFX:
  - Kept Hunter Oath in the yellow-green/green projectile family, and separated Stopped Second into a clear yellow/gold time-field language.
  - Stopped Second active VFX now uses gold clock colors instead of the prior blue/cyan read.
  - Active Stopped Second freeze now lasts up to `1.0s`, while the field VFX stays visible for `1.50s`.
  - Stopped Echo field lifetime increased to `0.90s`; Stasis Hunt field lifetime increased to `1.20s~1.50s`.
  - Clock-field readability was amplified with a larger face, stronger alpha, thicker hands, brighter core, larger ticks, and a rotating pulse ring.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity Play Mode smoke: `clockFaces=2`, `totalClockFaces=3`, `clockTicks=24`, `totalClockTicks=36`, `clockPulses=2`, `clockHands=6`, `goldFaces=2`, `frozenNear1s=5`.
  - Unity console error count: 0.

- Refined Execution Flash and Stopped Second readability:
  - Execution Flash active VFX target width increased from `1.30` to `1.95`, with a longer `0.38s` lifetime.
  - Execution Flash now spawns explicit vertical/horizontal/diagonal crack lines around the target so it reads as a large execution burst instead of a tiny diamond.
  - Execution Echo target width increased from `1.08` to `1.48` and now also uses the crack burst helper.
  - Stopped Second now uses a clock-field floor telegraph: clock face, outer/inner rings, 12 tick marks, clock hands, and core.
  - Stopped Echo and Stasis Hunt preview/ultimate paths also use the clock-field language.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity Play Mode smoke: `executionCracks=16`, `executionVfx=24`, `clockFaces=5`, `clockTicks=60`, `clockHands=15`, `stoppedVfx=79`.
  - Unity console error count: 0.

- Completed the utility VFX / background / movement follow-up:
  - Greatsword slash VFX delay reduced again from `0.20s` to `0.18s`, so the slash appears at roughly `64.3%` of the `0.28s` weapon sweep.
  - Six non-core active memories and echoes were made more visible through larger generated sprite scaling, stronger alpha, longer lifetimes, and clearer secondary cues.
  - `StoppedSecond` now places its time-stop focus on the nearest enemy cluster instead of only around the player, with clock-hand VFX and a stronger freeze window.
  - Added debug review buttons for `Mem A`, `Mem B`, `Echo A`, `Echo B`, `Ult 3`, and `VFX` so utility memory/echo/ultimate VFX can be checked immediately.
  - Added runtime arena dressing: dark backdrop, boundary bands, memory cracks, and outer markers that keep the arena readable without competing with combat VFX.
  - Player movement animation was softened with lower acceleration/deceleration, slower walk cadence, reduced bob, and reduced tilt.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity Play Mode utility smoke: `greatDelay=0.18`, `sweep=0.28`, `activeMemories=3`, `bgDecor=30`, `utilityVfx=36`, `enemies=14`.
  - Unity Play Mode echo/ultimate smoke: `echoCount=6`, `previewUlt=6`, `clockHands=21`, and utility echoes all reached +5.
  - Unity console error count: 0.

- Completed a three-part weapon/combat flow follow-up:
  - Dual blades now use staggered slash VFX timing: A slash `0.045s`, cut flash `0.067s`, B slash `0.085s`.
  - Dual-blade slash/spark profile scales and lifetimes were increased slightly so the quick cross-cut reads without becoming a greatsword fan.
  - Blood Blade Storm payoff increased: stronger opening cue, heavier/faster burst cadence, larger rings/blades, more pressure damage, stronger heal, hitstop, and camera shake.
  - First-120-second tempo increased: opening spawn interval `0.52 -> 0.46`, mid opening `0.58 -> 0.52`, late opening `0.50 x3 -> 0.46 x4`, early cap `28 -> 32`, early XP multiplier `1.95 -> 2.15`.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: 0.
  - Runtime check: dual delays A `0.045`, flash `0.067`, B `0.085`, assist `0.045`; opening spawn interval `0.46`, pack `2`; early `GrantXp(1)` produced `2/5` XP.
  - Blood Blade Storm smoke: `stormReady=True`, `stormObjects=187`, `burstObjects=45`, `bladeObjects=187`, `kills=21`.
  - Unity console error count: 0.

- Tightened greatsword slash VFX timing:
  - greatsword slash delay reduced from `0.22s` to `0.20s` after jaewoo review that the slash VFX felt slightly late.
  - with the `0.28s` weapon sweep, slash VFX now appears at roughly `71.4%` of the motion instead of `78.6%`.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: 0.
  - Runtime value check: delay `0.20s`, sweep `0.28s`, slash appears at `71.4%`.
  - Unity console error count: 0.

- Tuned greatsword VFX timing and sweep coverage after visual review:
  - greatsword slash delay increased from `0.18s` to `0.22s`; with the `0.28s` sweep, slash VFX now appears at roughly `78.6%` of the weapon swing.
  - greatsword phantom lifetime increased to `0.42s`; slash minimum lifetime increased to `0.62s`.
  - AoE / Primary / Assist crescent placement now samples different points along the 90-degree tip arc instead of placing every VFX at the final blade tip.
  - AoE crescent stays around the middle-late arc (`58%`), Primary around the late arc (`78%`), and shock/cutpoint remains at the final tip.
  - Greatsword VFX profile lifetimes increased: AoE `0.62s`, Primary `0.52s`, shock `0.26s`, assist `0.22s`.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity inline Game View capture succeeded for a frozen review frame with sword at about `85%` of the swing and long-lived VFX visible.
  - Runtime check: slash appears at `78.6%` of the sweep; AoE scale/lifetime `1.65 / 0.62`, Primary scale/lifetime `1.38 / 0.52`.
  - Unity console error count: 0.

- Increased greatsword swing spectacle:
  - greatsword handle-pivot sweep increased from `-28 -> +28` degrees to `-45 -> +45` degrees, for a full `90` degree cut.
  - greatsword wide crescent prompt-sprite scale factor increased from `0.150` to `0.175`.
  - greatsword weapon-hit VFX profile scales/lifetimes increased for AoE crescent, primary crescent, shock, cut point, and assist crescent.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: 0.
  - Play Mode forced greatsword attack: `usePivot=True`, blade sweep `-45.0 -> 45.0`, total `90.0`, AoE scale `1.65`, primary scale `1.38`.
  - Direct slash VFX check: end blade `45.0`, VFX rotation `225.0`, generated bounds `(4.28, 4.28)`, tip alignment error `0.000`.
  - Unity console error count: 0.

- Reworked greatsword phantom attack into a handle-pivot sweep:
  - greatsword phantom no longer slides from one position to another; it rotates around a handle pivot placed near the player-facing side.
  - blade direction now sweeps from `-28` to `+28` degrees around the handle, making the motion read more like a real cut.
  - greatsword crescent VFX now uses the sweep end blade direction with a `180` degree facing correction so the fan/crescent opens with the sword instead of backwards.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: 0.
  - Play Mode forced greatsword attack: `usePivot=True`, handle distance from player `0.13`, strike center distance `0.61`, start blade `-28.0`, end blade `28.0`.
  - Direct slash VFX check: end blade `28.0`, VFX rotation `208.0`, tip alignment error `0.000`.
  - Unity console error count: 0.

- Aligned greatsword phantom weapon and slash VFX around the blade tip:
  - greatsword phantom now calculates the blade tip first, then derives the sprite center and rotation from player-to-tip direction.
  - the handle side now points back toward the player body while the blade tip points through the target.
  - greatsword slash VFX anchors are corrected so final slash position lands at the calculated blade tip, including compensation for each `SlashVfxEntry.localOffset`.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity `Assets/Refresh`: success.
  - Unity compile error count: 0.
  - Greatsword Play Mode check: `handleCloser=True`, tip distance from player `1.67`, handle distance `0.16`.
  - Greatsword slash alignment check: desired tip and `GreatswordCrescent_Primary` position matched with distance `0.000`.
  - Unity console error count: 0.

## 2026-06-22

- Improved hit readability timing for phantom weapon attacks:
  - phantom weapon sprites now sweep across the hit point before the slash / hit VFX appears.
  - dual-blade phantom weapons sweep roughly `46` degrees over `0.13~0.14s` and remain visible for `0.24~0.26s`.
  - greatsword phantom weapons sweep roughly `48~50` degrees over `0.20~0.22s` and remain visible for `0.34~0.38s`.
  - weapon slash VFX now spawns after a short delay: dual blades `0.055s`, greatsword `0.075s`.
  - weapon slash VFX lifetime is multiplied by `1.45`, with minimum lifetimes of `0.34s` for dual blades and `0.48s` for greatsword.
  - weapon hit spark and hit-confirm ring/core now use the same short delay as the slash VFX, so the weapon motion reads first.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity `Assets/Refresh`: success.
  - Unity compile error count: 0.
  - Greatsword immediate Play Mode check: phantom `2`, active sweep `2`, slash `0`, spark `0`, confirm `0`.
  - Delayed enumerator check: greatsword slash `1`, spark `1`, confirm `2`, expected slash minimum lifetime `0.48s`.
  - Unity console error count: 0.

- Changed weapon visuals from player-attached silhouettes to hit-point phantom strikes:
  - player-held `LeftBlade` / `RightBlade` renderers now stay disabled during normal play.
  - dual blades now briefly spawn two blade sprites at the hit target, crossing around the slash VFX.
  - greatsword now briefly spawns a heavy blade strike plus a faint afterimage at the cleave center.
  - phantom weapon sprites are scaled by target world height so the generated weapon PNGs do not cover the player.
- Verification:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity `Assets/Refresh`: success.
  - Unity compile error count: 0.
  - Dual-blade Play Mode reflection check: held renderers disabled, `DualBladePhantom*` count `2`, max bounds `(1.151, 1.151)`.
  - Greatsword Play Mode reflection check: held renderers disabled, `GreatswordPhantom*` count `2`, max bounds `(1.586, 1.689)`.
  - Unity console error count: 0.

- Re-tuned greatsword after direct Play Mode check:
  - greatsword held sprite reduced again from `0.34~0.375` to `0.21~0.235`.
  - greatsword moved behind the player (`sortingOrder 18` vs player `20`) so it no longer covers the character body.
  - greatsword held position shifted to the side at `x=0.18`, `y=-0.08` with shorter swing travel.
  - greatsword cleave PNG scale factor reduced again from `0.182` to `0.150`.
- Verification:
  - Before fix: greatsword bounds `4.995` high, `2.26x` player height, in front of player.
  - After fix: greatsword bounds `2.944` high, `1.33x` player height, behind player.
  - Forced greatsword attack VFX max bounds: `2.332 x 2.332`.
  - Unity console error count: 0.

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
