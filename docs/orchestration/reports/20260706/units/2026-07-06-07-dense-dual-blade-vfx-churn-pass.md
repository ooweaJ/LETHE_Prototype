# 2026-07-06-07 - Dense Dual-Blade VFX Churn Pass

## 1. Current Build State

`Dev_Prototype_v1` now has a focused dense dual-blade performance pass in code. Local C# runtime/editor builds pass. Final Unity QA rerun is pending because AnkleBreaker Unity MCP transport closed after `Assets/Refresh`.

## 2. Changed Today

- Added dense dual-blade echo/slash throttles.
- Added lighter dense Kalmuri follow-ups that remember dense state when scheduled.
- Suppressed dense Blood Reflection bloom/accent spam while keeping mark readability.
- Changed dense utility echoes to rotate one utility family per primary hit.
- Replaced VoidPriest heal target lookup with manager-list targeting.
- Added `LETHE/V1 QA/Dense Dual Blades Perf Matrix`.

## 3. Test Result / Evidence

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: passed, 7 existing legacy warnings, 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: passed, 7 existing legacy warnings, 0 errors.
- Unity compile error count before MCP closure: `0`.
- Dense QA reproduced the problem before final throttle:
  - `954 transient / 506 activeVfx / 947.64ms`
  - `550 transient / 307 activeVfx / 561.77ms`
  - `627 transient / 361 activeVfx / 644.39ms`

## 4. Decision

Do not add more VFX volume to dual blades until the dense matrix passes. The next move is measurement and churn reduction, not spectacle.

## 5. Risk

The latest code is build-verified but not yet Unity-QA-verified after the final throttle because MCP transport is closed.

## 6. GPT/Claude Handoff

Dense dual-blade lag is now an explicit QA target. If final QA still fails, inspect Kalmuri follow-up object count, Blood mark/accent count, and utility echo fan-out first.

## 7. Next Codex Work

Retry Dense Dual Blades Perf Matrix, then rerun Void Priest Heal Matrix and M2 Loop.

## 8. Portfolio Note

- Problem: dense dual-blade combat produced too much transient VFX and felt laggy.
- Direction: measure dense combat as a QA matrix and throttle the repeated VFX families.
- Action: added dense echo/slash/Kalmuri/Blood/utility throttles and manager-side healer targeting.
- Result: local builds pass and QA baseline is documented; final Unity QA rerun remains pending due MCP transport.
