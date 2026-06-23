# LETHE TEST

## Purpose

이 문서는 LETHE 작업이 성공했는지 판단하는 기준이다. 자동 검증은 회귀 확인용이고, 최종 GO/ITERATE/NO-GO는 jaewoo 플레이 체감 리뷰가 우선한다.

## Required Technical Checks

Unity/C# 작업 후 가능한 경우 아래를 실행한다.

```powershell
dotnet build LETHE/Assembly-CSharp.csproj --nologo
npm.cmd run report
npm.cmd run report:check
```

Unity MCP가 연결되어 있으면 추가로 확인한다.

- Unity compile error count = 0.
- `Dev_Prototype_v1.unity` open success.
- Play Mode smoke에서 player/enemy/weapon runtime exception 없음.
- 필요한 경우 evidence screenshot 저장.

## Current Epic Checks

### A. Data Contract

- Definition 타입이 컴파일된다.
- 새 memory/echo/enemy/ultimate/encounter 데이터를 추가해도 기존 weapon data asset이 깨지지 않는다.
- 기존 `Weapon_DualBlades`, `Weapon_Greatsword`, `VFX_Weapon_DualBlades`, `VFX_Weapon_Greatsword` 경로가 유지된다.

### B. Hit Feel / Echo Readability

- 쌍검 기본공격이 빠른 2연 반달 베기로 보인다.
- 대검 기본공격이 범위만큼 큰 반달 참격으로 보인다.
- 적은 피격 시 흰색 플래시와 데미지 숫자를 보여준다.
- 굶주린 칼무리는 작은 장식 오라가 아니라 여러 칼이 주변을 점유하고 타깃을 물어뜯는 군집으로 보인다.
- 칼무리 잔향은 캐릭터 주변 잡선이 아니라 타격 지점 후속타로 보인다.
- 혈반 잔향은 표식/실/피꽃으로 보인다.
- Current B-step technical check: `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed after Kalmuri range ring, Blood heal thread, Blood bloom thread, and knockback cap changes.
- Kalmuri readability follow-up, 2026-06-18: active Hungry Blades now uses a denser two-ring blade swarm and target-local bite blades. Kalmuri echo follow-ups now add blade barrage sprites. `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed with 7 legacy warnings and 0 errors. Unity compile error count was 0, and short Play Mode entry produced 0 console errors; human visual review is still required because the run starts behind the weapon-select overlay.
- Core VFX prompt-sheet replacement, 2026-06-21:
  - Added `docs/design/LETHE_SPRITE_PRODUCTION_PROMPTS.md`.
  - Replaced Kalmuri 3, Blood 3, and Blood Blade Storm 2 sprites with prompt-sheet generated PNGs.
  - Evidence: `LETHE/Assets/_dev/Evidence/core_vfx_prompt_sheet_refresh_20260621.png`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity AssetDatabase lists Kalmuri 3, Blood 3, and Ultimate 2 replacement textures.
  - Remaining risk: this verifies asset import and visual direction, not final in-run scale/timing.
- Remaining VFX prompt-sheet generation, 2026-06-21:
  - Generated weapon/hit VFX 5, active memory VFX 6, echo VFX 6, and ultimate VFX 3.
  - Evidence: `LETHE/Assets/_dev/Evidence/remaining_vfx_prompt_sheet_20260621.png`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity AssetDatabase found 20/20 generated VFX textures.
  - Unity import settings confirmed 20/20 final PNGs as Sprite textures.
  - Remaining risk: these assets are generated/imported, but runtime VFX profiles still need sprite wiring, scale, alpha, and timing review.
- Greatsword direct Play Mode cover fix, 2026-06-22:
  - Direct Greatsword run showed player bounds `(2.210, 2.210)` and sword bounds `(3.121, 4.995)`, ratioY `2.26`, with sword sorting order `30` in front of player sorting order `20`.
  - Greatsword held sprite was reduced to runtime scale `0.21~0.235`, shifted to the side, and moved behind the player at sorting order `18`.
  - Post-fix Play Mode check showed sword bounds `(2.327, 2.944)`, ratioY `1.33`, sword sorting order `18`, player sorting order `20`.
  - Forced greatsword attack produced `greatswordVfx=5` with max VFX bounds `(2.332, 2.332)` and Unity console error count 0.
- Weapon silhouette / attack VFX scale tune, 2026-06-22:
  - Dual blade held-weapon runtime scale increased to `0.43~0.475` and pulled closer to the body.
  - Greatsword held-weapon runtime scale reduced to `0.34~0.375` with shorter swing travel.
  - Generated dual-blade slash PNG scale factor increased from `0.153` to `0.192`.
  - Generated greatsword cleave PNG scale factor reduced from `0.225` to `0.182`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Play Mode runtime transform check confirmed dual blade scale `0.430`, greatsword scale `0.340`, console error count 0.
- Generated VFX runtime wiring, 2026-06-22:
  - Connected generated weapon/hit, six active memory, six echo, and three utility ultimate sprites to `V1GameManager`.
  - Weapon slash profile entries now prefer generated dual-blade arcs, greatsword cleave, Kalmuri slash, and cyan/red hit sparks before procedural fallback.
  - Added generated-sprite world-size scaling because the prompt-sheet PNGs are 1254px square and would otherwise render too large.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity `Assets/Refresh`: success.
  - Unity compile error count: 0.
  - Unity Play Mode smoke attempts produced console error count 0.
  - Remaining risk: Game/Scene capture still returned solid-color images, so jaewoo direct visual review must judge final scale, alpha, timing, and natural combat readability.
- Blood Blade Storm payoff / movement pass, 2026-06-22:
  - Blood Blade Storm now has opening cue, continuous pressure, and periodic burst pulses instead of only Kalmuri-like rotating blades.
  - Dual-blade storm uses faster blade orbit and more frequent bursts; greatsword storm uses slower/heavier slashes and burst impact.
  - Player movement now uses acceleration/deceleration smoothing, smoothed movement-facing weapon rotation, and subtle `PlayerVisual` walk bob/tilt.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity console error count after direct M2/ultimate reflection smoke: 0.
  - Direct M2 state injection confirmed `storm=True`; manual `UpdateEchoUltimate(0.12f)` reflection ticks created `bloodStormObjects=124` and cleared nearby spawned enemies with `kills=14`.
  - Remaining risk: MCP Play Mode time did not advance normally in this session, so the storm loop was verified by reflection ticks rather than natural timed play.
- Hit-point phantom weapon pass, 2026-06-22:
  - Player-attached `LeftBlade` / `RightBlade` renderers now stay disabled during normal play.
  - Dual-blade weapon identity is shown by two short-lived blade sprites spawned at the hit target.
  - Greatsword identity is shown by a heavy strike sprite plus a faint afterimage spawned at the cleave center.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy v0/debug warnings and 0 errors.
  - Unity `Assets/Refresh`: success.
  - Unity compile error count: 0.
  - Dual-blade Play Mode reflection check: held renderers disabled, phantom count `2`, max bounds `(1.151, 1.151)`.
  - Greatsword Play Mode reflection check: held renderers disabled, phantom count `2`, max bounds `(1.586, 1.689)`.
  - Unity console error count: 0.
  - Remaining risk: this verifies technical behavior and rough bounds. Jaewoo direct play review still needs to judge whether the magical auto-blade look feels better than body-held weapons.
- Phantom weapon timing/readability pass, 2026-06-22:
  - Phantom weapons now sweep before weapon slash VFX appears.
  - Dual blades use a short `0.055s` slash / hit feedback delay after the weapon sweep starts.
  - Greatsword uses a slightly heavier `0.075s` slash / hit feedback delay after the weapon sweep starts.
  - Weapon slash lifetime is extended by `1.45x`, with minimum lifetimes of `0.34s` for dual blades and `0.48s` for greatsword.
  - Weapon hit spark and hit-confirm ring/core now use the same delayed timing as slash VFX.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Greatsword immediate Play Mode check: phantom `2`, active sweep `2`, slash `0`, spark `0`, confirm `0`.
  - Delayed enumerator check: greatsword slash `1`, spark `1`, confirm `2`, expected slash minimum lifetime `0.48s`.
  - Unity console error count: 0.
  - Remaining risk: Unity MCP time did not reliably advance coroutine time in this session, so delayed timing was verified through immediate state plus direct enumerator advancement. Final motion readability still requires jaewoo direct play review.
- Greatsword blade-tip alignment pass, 2026-06-23:
  - Greatsword phantom weapon now calculates the intended blade tip first, then places the weapon center so the handle points back toward the player body.
  - Greatsword slash VFX now anchors from the calculated blade-tip position, so `GreatswordCrescent_Primary` appears on the sword tip instead of drifting around the sword center.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Play Mode forced greatsword attack check: `handleCloser=True`, tip distance from player `1.67`, handle distance from player `0.16`.
  - Slash alignment check: desired tip and `GreatswordCrescent_Primary` position matched with distance `0.000`.
  - Unity console error count: 0.
  - Remaining risk: this confirms geometry and runtime errors, not final feel. Jaewoo direct play review should confirm the 45-degree sweep reads naturally.
- Greatsword handle-pivot / crescent direction pass, 2026-06-23:
  - Greatsword phantom sweep now rotates around a handle pivot instead of moving the whole weapon from start position to end position.
  - The handle pivot is placed on the player-facing side; the blade direction sweeps `-28` to `+28` degrees.
  - Greatsword crescent VFX now uses the sweep end blade direction plus a `180` degree facing correction so the fan/crescent faces with the sword.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Play Mode forced greatsword attack: `usePivot=True`, handle distance from player `0.13`, strike center distance `0.61`, start blade `-28.0`, end blade `28.0`.
  - Direct slash VFX check: end blade `28.0`, VFX rotation `208.0`, tip alignment error `0.000`.
  - Unity console error count: 0.
  - Remaining risk: automated verification confirms pivot geometry and VFX rotation, but final visual feel still needs jaewoo direct play review.
- Greatsword spectacle pass, 2026-06-23:
  - Greatsword handle-pivot sweep increased to a full `90` degree cut: `-45.0 -> 45.0`.
  - Greatsword wide crescent generated sprite scale factor increased from `0.150` to `0.175`.
  - Greatsword weapon-hit VFX profile scales/lifetimes increased so the slash reads more hot and flashy.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Play Mode forced greatsword attack: `usePivot=True`, blade sweep `-45.0 -> 45.0`, total `90.0`, AoE scale `1.65`, primary scale `1.38`.
  - Direct slash VFX check: end blade `45.0`, VFX rotation `225.0`, generated bounds `(4.28, 4.28)`, tip alignment error `0.000`.
  - Unity console error count: 0.
  - Remaining risk: this is intentionally more explosive; jaewoo direct review should confirm it is flashy, not screen-covering.
- Greatsword timing / coverage review loop, 2026-06-23:
  - Greatsword slash delay increased from `0.18s` to `0.22s`, so slash VFX appears after roughly `78.6%` of the `0.28s` sweep.
  - Greatsword minimum slash lifetime increased to `0.62s`.
  - Greatsword AoE / Primary / Assist crescent positions now use different points along the 90-degree blade-tip arc: `58%`, `78%`, and `72%`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity inline Game View capture succeeded on a frozen review frame with the sword held around `85%` of the swing and long-lived VFX visible.
  - Runtime value check: delay `0.22s`, sweep `0.28s`, min slash lifetime `0.62s`, AoE scale/lifetime `1.65 / 0.62`, Primary scale/lifetime `1.38 / 0.52`.
  - Unity console error count: 0.
  - Remaining risk: Game View capture was inline rather than saved to evidence. Jaewoo direct play review is still the final feel gate.
- Greatsword slash timing tighten, 2026-06-23:
  - Greatsword slash delay reduced from `0.22s` to `0.20s` because the VFX felt slightly late.
  - With the `0.28s` weapon sweep, slash VFX now appears after roughly `71.4%` of the swing.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Runtime value check: delay `0.20s`, sweep `0.28s`, slash appears at `71.4%`.
  - Unity console error count: 0.

### C. Real M2 Loop

- 디버그 버튼 없이 60~120초 안에 망각/잔향/공명/+5/궁극 중 핵심 흐름이 보인다.
- 플레이어가 "이 기억을 키우면 다음에 잃는다"를 의식할 수 있다.
- Current C-step technical check: `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed after adding the M2 HUD objective text and the third-memory level-up card path.
- Stage/balance shell follow-up, 2026-06-18:
  - Normal runs now use 600s run duration, Gatekeepers at 180/340/490/600s, first boss HP 2050, 54s deficit survival, documented pressure phase spawn profiles, documented spawn caps, and all six run stat choices.
  - Fast/debug paths retain compressed timing for smoke tests.
  - Review-only automatic memory/+5 injection is no longer part of normal runs.
  - Transient VFX, floating text, damage numbers, and XP orbs now use internal object pools.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 0 warnings, 0 errors.
  - Unity `Assets/Refresh` then compile error count: 0.
  - Short Unity Play Mode entry: console error count 0.
  - Remaining risk: this confirms technical wiring, not final player-facing balance. A full manual run or compressed smoke still needs jaewoo review.

### J. 120초 초반 재미 루프

- 시작 화면은 무기+기억 조합 4개를 보여준다.
- 1~4 숫자키 또는 카드 클릭으로 아래 빌드가 시작된다:
  - 절단쌍검 + 굶주린 칼무리.
  - 절단쌍검 + 피의 반사.
  - 장송대검 + 굶주린 칼무리.
  - 장송대검 + 피의 반사.
- 혈반으로 시작해도 레벨업 카드에서 굶주린 칼무리가 우선 후보로 나온다.
- 칼무리로 시작해도 레벨업 카드에서 피의 반사가 우선 후보로 나온다.
- Current technical check, 2026-06-19:
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity Play Mode entry reached `isPlaying=true`.
  - Unity console error log count: 0.
  - Camera-based Game View screenshot did not capture OnGUI start cards; direct human visual review is still required.
- Five-pass follow-up, 2026-06-19:
  - First-120-second kills grant +1 extra XP before the normal pre-boss multiplier.
  - Weapon hits spawn a confirm ring/core pulse for clearer hit reading.
  - Forgetting result text now includes loss, echo, overcharge/awakening, deficit survival, and resonance next action.
  - First-cycle spawn pressure has an early 120-second profile and closer spawn radius.
  - Drifting Eye, Split One, Void Priest, and Gatekeeper now have distinct procedural silhouettes.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed after each pass with 7 legacy warnings and 0 errors.
  - Final Unity MCP check: compile error count 0, Play Mode entered, console error count 0, Play Mode stopped.
  - Human review still needs to judge whether the first 120 seconds now feel busy, readable, and worth replaying.
- Direct Codex smoke follow-up, 2026-06-19:
  - Added `LETHE/V1 Smoke/*` editor menu items to run start routes and the M2 loop without manual keyboard/click input.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors for the smoke menu code.
  - Unity compile error count: 0.
  - Start build smoke snapshots:
    - `DualBlades + HungryBlades`: weapon `절단쌍검`, memories `[HungryBlades:1]`, result/refill/death all false.
    - `DualBlades + BloodReflection`: weapon `절단쌍검`, memories `[BloodReflection:1]`, result/refill/death all false.
    - `Greatsword + HungryBlades`: weapon `장송대검`, memories `[HungryBlades:1]`, result/refill/death all false.
    - `Greatsword + BloodReflection`: weapon `장송대검`, memories `[BloodReflection:1]`, result/refill/death all false.
  - M2 loop smoke snapshot: memories `[BloodReflection:3,HungryBlades:3]`, echoes `[HungryBlades:5,BloodReflection:5]`, enemies `10`, storm `True`, result overlay `True`, death `False`.
  - Unity console error log count after M2 smoke: 0.
  - Unity missing references: scene 0, assets 0.
  - Remaining risk: this is a technical smoke test, not a feel review. It proves the routes can initialize and the debug M2 loop wires up, but jaewoo still needs to play the first 120 seconds.
- Start-selection UX correction, 2026-06-19:
  - Start overlay now presents only two weapon choices: `절단쌍검` and `장송대검`.
  - `BeginRun(V1WeaponId)` no longer grants a starting memory.
  - `LETHE/V1 Smoke/Start Dual Blades` snapshot: weapon `절단쌍검`, memories `[]`, result/refill/death all false.
  - Forced first level-up after weapon start produced choices: `굶주린 칼무리 | 피의 반사 | 칼날 가속`.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity compile error count: 0.
  - Unity console error count: 0.

- Visual/UI/game-feel refresh, 2026-06-19:
  - Player root movement is stable; the visual sprite is now on `PlayerVisual`.
  - Player body scale pulse was removed to avoid side-to-side wobble perception.
  - New player body sheet `sheet_player_v1_4dir.png` was generated/imported.
  - The new 8x4 player sheet now drives 4-direction idle/walk animation.
  - Greatsword uses the imported `spr_weapon_greatsword_01.png` sprite.
  - Arena floor has tile rotation, color variation, and small scale variation.
  - HUD was compacted into HP/XP/memory/ultimate/debug panels.
  - `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 7 legacy warnings and 0 errors.
  - Unity `Assets/Refresh`: success.
  - Unity compile error count: 0.
  - Unity console error count: 0.
  - Short Unity Play Mode entry after the new player sheet: success, console error count 0.
  - Unity missing references: scene 0, assets 0.
  - Greatsword Play Mode smoke snapshot: `scene=v1 weapon=장송대검 elapsed=1.8 hp=210.0/210.0 enemies=6 death=False`.
  - Remaining risk: automated screenshot capture was discarded as solid color, so direct visual review is still required.

### H. Human Review Gate

jaewoo 리뷰 질문:

- 기본 공격이 무기별로 재미있나?
- 망각이 아깝나, 짜증나나?
- 잔향이 실제 전투를 바꾼다고 느껴지나?
- 재획득 공명이 설레나?
- +5/궁극이 후반 보상처럼 느껴지나?
- 다음에 고칠 가장 큰 문제 1~3개는 무엇인가?

### E/F/G. Content Expansion

- Current technical check: `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed with 0 warnings and 0 errors after adding first-pass active effects for the remaining memories, utility echo reactions, and three additional ultimate runtime branches.
- Current data asset check: `_dev/Data` now contains 8 `MemoryDefinition` assets, 8 `EchoDefinition` assets, and 4 `UltimateEchoDefinition` assets. Latest `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed with 7 legacy v0/debug deprecation warnings and 0 errors.
- Unity MCP check: after `Assets/Refresh`, AssetDatabase lists 8 `MemoryDefinition`, 8 `EchoDefinition`, and 4 `UltimateEchoDefinition` assets. `Dev_Prototype_v1` entered Play Mode with no console errors in the short smoke, and evidence was saved to `LETHE/Assets/_dev/Evidence/v1_content_data_asset_play_smoke_20260617.png`.
- Runtime exception QA: Unity console showed `InvalidOperationException: Collection was modified` in `V1GameManager.BloodBloom`. Area-effect loops over `enemies` now use snapshot lists and null guards. Follow-up Play Mode smoke showed no runtime exceptions.
- Runtime exception QA follow-up, 2026-06-18: `V1GameManager` enemy-list queries were scanned for remaining direct `enemies` enumeration/mutation risks. Added null guards to Hungry Blades target selection and enemy-cap counting. Final `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed with 0 warnings and 0 errors. A short Unity Play Mode smoke had passed before the patch with no runtime exceptions, but post-patch MCP recheck was blocked by `Transport closed`.
- Human review still needs to confirm whether these effects read as distinct enough, because Unity MCP visual verification was not available in this session.

## Known Non-Blocking Warnings

현재 `dotnet build`에는 legacy v0/debug 코드의 `Object.FindObjectOfType<T>()` deprecation warning 7개가 남아 있다. `Dev_Prototype_v1` compile error가 아니므로 현재 EPIC 진행을 막지 않는다.
