# LETHE Sprite Production Prompt Sheet

Last updated: 2026-06-21

This is the working source for LETHE runtime sprite generation. It is based on the attached prompt sheet and supersedes ad-hoc image prompts for `_dev` sprite work.

## 0. Base Prompt

Append this block to every sprite prompt unless a row says otherwise.

```text
[STYLE] top-down 2D game asset, stylized dark fantasy roguelite, theme of memory and forgetting (river Lethe), ethereal melancholic mood, crisp glowing edges with soft inner bloom, sharp silhouette, faint drifting particle motes, high contrast on dark void
[TECH] perfectly flat solid #00ff00 chroma-key background, single centered subject, square canvas, sprite-ready, generous transparent-safe padding, no border, no ground, no cast shadow
[NEG] no text, no watermark, no UI, not photorealistic, no busy clutter, no real human hands, do not use #00ff00 in the subject
```

Runtime export rule:

1. Generate source on flat `#00ff00`.
2. Save source under `LETHE/Assets/_dev/Art/Source/`.
3. Remove chroma to alpha.
4. Save final PNG under `LETHE/Assets/_dev/Art/Sprites/`.
5. Apply Unity Sprite import settings.

## 1. Palette

| Family | Use | HEX |
| --- | --- | --- |
| Memory | luminous cyan-blue | `#3FE0E0` |
| Blood | crimson red | `#E03F4F` |
| Ash | grey-white | `#B8BCC8` |
| Execution | white-gold | `#FFF4D0` |
| Oblivion | deep violet-black | `#3A2050` |

## 2. Visual Rules

- Active memory VFX: saturated, alive, current-tense. It should feel like a living memory operating now.
- Echo VFX: same motif, but desaturated, ghosted, doubled, fragmented, or dissolving. It should feel like a forgotten afterimage.
- Ultimate VFX: larger scale, combines two echo motifs, readable as a payoff without hiding combat targets.
- Character sprites must not include weapons.
- Weapon sprites should show only the equipped weapon body. Attack readability belongs mostly to slash/VFX sprites.
- Existing sprites may be replaced when they do not match this sheet, but keep source chroma files for traceability.

## 3. Current Sprite Inventory

| Group | Exists now | Replace with this sheet? | Notes |
| --- | --- | --- | --- |
| Player | `sheet_player_v1_4dir.png` | Later polish | Usable for v1 review. |
| Dual blades | `spr_weapon_dual_blade_left_01.png`, `spr_weapon_dual_blade_right_01.png` | Later polish | Functional, but attack arcs are missing. |
| Greatsword | `spr_weapon_greatsword_01.png` | Later polish | Newly generated, usable. |
| Chaser enemy | `sheet_enemy_chaser_4dir.png` | Later polish | Existing prototype asset. |
| Floor | `tile_dev_floor_dark_01.png` | Add variants | Needs less repetition. |
| Kalmuri VFX | orbit/echo/launch sprites exist | Replace now | Core identity must match final style. |
| Blood VFX | mark/bloom/heal thread exist | Replace now | Core identity must match final style. |
| Blood Blade Storm | ring/blade sprites exist | Replace now | First ultimate payoff. |
| Other 6 memories | mostly procedural | Generate new | Needs dedicated sprites. |
| Other 6 echoes | mostly procedural | Generate new | Needs dedicated sprites. |
| Other 3 ultimates | procedural/minimal | Generate new | Needs dedicated sprites. |
| UI icons | missing | Generate after VFX | Needs card/HUD pass. |

## 4. Weapon / Hit VFX Prompts

| File | Status | Prompt |
| --- | --- | --- |
| `spr_dual_blade_swing_arc_01.png` | Needed | thin fast single crescent slash arc, dual-blade razor edge, cyan-white, slight motion blur |
| `spr_dual_blade_swing_arc_02.png` | Needed | crossing mirrored crescent slash, second hit of a fast combo, sharper cyan-white twin arc |
| `spr_greatsword_cleave_arc_01.png` | Needed | heavy wide crescent cleave arc, thick weighty sweep, cyan-white core with crimson impact tint |
| `spr_hit_spark_cyan_01.png` | Needed | small sharp radial impact spark burst, cyan star shards, crisp |
| `spr_hit_spark_red_01.png` | Needed | jagged radial impact spark, crimson shards, slightly violent burst |

## 5. Enemy / Boss Prompts

For enemies, first generate a strong front-facing reference. Then use that as a reference for the other directions or a 4-direction sheet.

| File | Status | Prompt |
| --- | --- | --- |
| `sheet_enemy_eye_4dir.png` | Needed | floating disembodied eyeball creature, trailing void tendrils, glowing iris, ranged caster, melancholic, front-facing reference |
| `sheet_enemy_splitter_4dir.png` | Needed | gelatinous dividing blob creature, semi-translucent membrane, faint glowing inner core, about to split in two, cyan-tinged |
| `sheet_enemy_voidpriest_4dir.png` | Needed | hooded robed cultist, hollow void where the face should be, casting gesture, tattered ashen robes |
| `spr_boss_gatekeeper_01.png` | Needed | large imposing armored guardian, stone-and-iron body, glowing rune eyes, gate/barrier motif, menacing silhouette, boss scale |

## 6. Active Memory VFX Prompts

| Memory | File | Status | Prompt |
| --- | --- | --- | --- |
| Hungry Blades | `spr_kalmuri_orbit_blade_01.png` | Replace now | spectral cyan memory blade shard, short curved dagger silhouette, orbit-ready, sharp luminous edge, melancholic glass-metal texture |
| Blood Reflection | `spr_blood_mark_01.png` | Replace now | crimson blood reflection mark, small circular blood glyph, liquid mirror glint, sharp red rim, readable as a mark |
| Execution Flash | `spr_execution_flash_01.png` | Needed | blinding white-gold execution flash, vertical light slash with a cross-shaped crack of radiance, holy judgment, sharp luminous shards |
| Hunter Oath | `spr_homing_shot_01.png` | Needed | glowing cyan arrowhead projectile, comet tail, lock-on targeting glow, slight seeking curve trail |
| Shatter Wave | `spr_shockwave_ring_01.png` | Needed | expanding concentric shockwave ring, cracked-glass fracture texture on the edge, cyan impact distortion |
| Stopped Second | `spr_timestop_field_01.png` | Needed | translucent pale-blue dome field, faint clock numerals and a frozen clock-hand, ticking-mark ring, suspended frozen motes |
| Ashen Shield | `spr_ashen_shield_01.png` | Needed | hexagonal runic barrier ring, ash-grey brittle energy, drifting ash particles |
| Oblivion Brand | `spr_brand_mark_01.png` | Needed | dark violet-black circular sigil glyph, smoldering edges, a forgetting rune, ghostly fade at the rim |

## 7. Echo VFX Prompts

| Echo | File | Status | Prompt |
| --- | --- | --- | --- |
| Kalmuri Echo | `spr_kalmuri_echo_slash_01.png` | Replace now | ghosted cyan crescent blade afterimage, fragmented forgotten slash, desaturated double-image edge, dissolving motes |
| Kalmuri Launch | `spr_kalmuri_launch_blade_01.png` | Replace now | launched spectral cyan blade shard, longer dagger projectile, ghosted trailing afterimage, crisp cutting point |
| Blood Echo | `spr_blood_bloom_01.png` | Replace now | crimson blood bloom burst, circular liquid flower explosion, ghosted droplets, dark red inner void |
| Blood Heal Thread | `spr_heal_thread_tip_01.png` | Replace now | tiny crimson healing thread tip, blood filament droplet, glowing red bead, readable at small scale |
| Execution Echo | `spr_execution_burst_01.png` | Needed | white cross-crack burst exploding outward, fractured light shards, desaturated ghosted echo trail |
| Homing Echo | `spr_homing_echo_01.png` | Needed | ghosted duplicate cyan darts with afterimage trails, faded shadow-bullets |
| Shockwave Echo | `spr_shockwave_echo_01.png` | Needed | faint secondary smaller ripple at impact point, translucent desaturated ring |
| TimeStop Echo | `spr_timestop_echo_01.png` | Needed | brief frozen time-fracture crack, pale-blue shards dissolving into motes |
| Ashen Echo | `spr_ashen_echo_01.png` | Needed | grey counter-pulse shockwave of ash, dissolving ash particles |
| Brand Echo | `spr_brand_echo_01.png` | Needed | violet-black sigil shattering into dark fragments and dim sparks |

## 8. Ultimate VFX Prompts

| Ultimate | File | Status | Prompt |
| --- | --- | --- | --- |
| Blood Blade Storm Ring | `spr_blood_blade_storm_ring_01.png` | Replace now | circular vortex ring of crimson blood and cyan spectral blades, storm orbit, strong center opening, dramatic ultimate payoff |
| Blood Blade Storm Blade | `spr_blood_blade_storm_blade_01.png` | Replace now | large blood-stained spectral blade shard, cyan edge with crimson core, ultimate orbit projectile, sharp silhouette |
| Fracture Execution | `spr_fracture_execution_01.png` | Needed | massive screen-cracking white-gold fracture lines fused with a crimson execution burst, huge dramatic radiant shatter |
| Stasis Hunt | `spr_stasis_hunt_01.png` | Needed | frozen pale-blue homing bullets suspended and splitting mid-air, ice shards, stasis field |
| Ashen Oblivion | `spr_ashen_oblivion_01.png` | Needed | grey ashen barrier overlaid with an expanding dark violet oblivion pulse wave, ash and void fused, large scale |

## 9. UI Icon Prompt Base

Use this instead of the runtime VFX base for icons.

```text
[ICON] game UI skill icon, centered emblem on dark rounded square frame, flat with subtle glow, bold readable silhouette at small size, single motif, transparent background, no text
```

Icon groups:

| Group | Count | Motifs |
| --- | --- | --- |
| Memory icons | 8 | orbiting blades, blood drop reflection, cross flash, homing arrow, fracture ring, clock hand, hex shield, violet sigil |
| Echo icons | 8 | Same motifs, ghosted echo variants |
| Ultimate icons | 4 | blood blade storm vortex, cracked execution, frozen homing, ash-void fusion |
| Level badges | 3 | `badge_echo_lv1.png`, `badge_echo_lv3.png`, `badge_echo_lv5.png` |
| Forget warning | 1 | `ico_forget_warning.png` |
| Resonance | 1 | `ico_resonance.png` |
| Run stats | 6-8 | attack speed, damage, area, survival, magnet, echo amp, HP, XP |

## 10. Production Order

1. Replace existing core VFX with this sheet:
   - Kalmuri 3.
   - Blood 3.
   - Blood Blade Storm 2.
2. Generate missing weapon/hit VFX:
   - dual arcs, greatsword arc, cyan/red hit sparks.
3. Generate the missing 6 active memory VFX.
4. Generate the missing 6 echo VFX.
5. Generate the missing 3 ultimate VFX.
6. Generate enemies/boss.
7. Generate UI icon set.

## 11. Verification

Each batch must pass:

- File exists under `Assets/_dev/Art/Sprites/...`.
- Source exists under `Assets/_dev/Art/Source/...`.
- Unity import type is Sprite.
- `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passes.
- Unity compile error count is 0 when MCP is available.
- If used by runtime, short Play Mode smoke has console error count 0.
