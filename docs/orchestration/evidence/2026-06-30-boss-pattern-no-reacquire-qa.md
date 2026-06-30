# 2026-06-30 Boss Pattern / No Reacquire QA

Purpose: jaewoo played the stepped-boss patch and reported that the first boss timing felt good, the second boss felt late, boss TTK was far too short, boss fights were too free-hit because there was no pattern, enemy count/HP felt okay, HP bars looked wrong, the post-forget memory reacquire loop should be removed entirely, and Hungry Blades seemed to pause or stutter during base attacks.

## Applied Runtime Values

| Item | Value |
| --- | --- |
| Gatekeeper schedule | `150 / 300 / 540 / 900s` |
| Gatekeeper intervals | `150 / 150 / 240 / 360s` |
| Gatekeeper HP | `2200 / 4200 / 7600 / 12800` |
| Run hard cap | `1080s` |
| Memory reacquire after forgetting | removed |
| Deficit/refill overlay in normal play | removed |

## Behavior Changes

- Gatekeepers now emit periodic pulse patterns.
- The pulse damages and pushes the player if they stay close, interrupting pure free-hit uptime.
- During the pulse window, Gatekeepers briefly guard and reduce incoming weapon/non-weapon damage.
- Enemy HP bars now counter-scale against enemy squash/local scale so bars do not stretch with hit feedback.
- Hungry Blades orbit visuals now keep updating during weapon hitstop.
- Hungry Blades orbit visual spawning is throttled and pooled so it creates fewer transient objects while staying continuous.

## Verification

- `node scripts/balance_curve_v1.js`: passed.
- `node scripts/verify_unity_stepped_balance.js`: passed.
- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed with 0 warnings and 0 errors after retry. The first parallel attempt failed only because Unity held the output DLL.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed with legacy warnings from shared runtime references and 0 errors.
- Unity MCP:
  - Active instance: `LETHE`, port `7890`.
  - Active scene: `Assets/_dev/Scenes/Dev_Prototype_v1.unity`.
  - Compile error count: `0`.
  - Console error count: `0`.
  - Direct Play Mode entered despite an MCP response parse error, then stopped cleanly.

## MCP Limitation

`LETHE/V1 Smoke/Start Dual Blades` entered Play Mode, but the MCP bridge restarted and only AB-UMCP server logs remained in the console, so a full `[V1QA] PASS` smoke log was not captured in this session. This is recorded as an MCP/queue stability limitation rather than a compile failure.

## Next Human Review

Replay from `Dev_Prototype_v1` and check:

- Is the first boss still a good 2:30 hook after HP increase?
- Does the second boss at 5:00 feel less late?
- Does Gatekeeper pulse/guard make the fight feel less like free DPS?
- Are HP bars stable during hit flashes/squash?
- Does forgetting feel cleaner now that there is no memory reacquire step?
- Does Hungry Blades keep visually rotating through base attack hitstop?
