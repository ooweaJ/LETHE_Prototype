# 2026-07-06-09 - Kalmuri Echo Clamp/Rip Readability Pass

## 1. Current Build State

`Dev_Prototype_v1` now has a redesigned Kalmuri echo visual phrase in code. Local C# runtime/editor builds pass. Unity visual/perf QA is pending because AnkleBreaker Unity MCP still returns `Transport closed`.

## 2. Changed Today

- Added `SpawnKalmuriEchoClampRip()`.
- Kalmuri echo now uses opposing clamp blades that close into the target point, followed by a rip slash.
- Reduced the old generic range/flash/cut visuals so they support the action instead of dominating it.
- Dense dual-blade Kalmuri follow-ups now skip the previous surge blade loop and keep the lighter clamp/rip shape.

## 3. Test Result / Evidence

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 existing legacy warnings, 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed, 7 existing legacy warnings, 0 errors.
- Unity MCP reconnect attempt: failed with `Transport closed`.

## 4. Decision

Kalmuri should read as a blade action first, not as another cyan circular effect. The new baseline is clamp into target, then rip.

## 5. Risk

The implementation is build-safe, but it still needs Unity visual judgment. Dense combat also needs a Kalmuri Perf Matrix rerun to confirm the new read did not raise object churn.

## 6. GPT/Claude Handoff

Review Kalmuri by concept read and performance together. If the clamp/rip is readable but too small, tune scale/alpha first. If object counts rise, reduce support visuals before touching damage.

## 7. Next Codex Work

Retry `Kalmuri Perf Matrix`, direct-play +1/+3/+5 Kalmuri, and recheck dense dual blades once Unity MCP routing is stable.

## 8. Portfolio Note

- Problem: Kalmuri echo looked like generic cyan blade clutter rather than a distinct Hungry Blades concept.
- Direction: make the VFX communicate a recognizable action.
- Action: implemented clamp blades plus rip slash and reduced support rings/flashes.
- Result: local builds pass; Unity visual/perf QA remains pending due MCP transport.
