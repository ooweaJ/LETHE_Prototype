# 2026-07-02 Dev_Prototype_v1 Direct Play Review

Purpose: jaewoo direct-play review after the July 2 echo, Kalmuri, passive memory, forget/resonance, and utility ultimate cleanup passes.

## Build To Review

- Unity scene: `Assets/_dev/Scenes/Dev_Prototype_v1.unity`
- Scope: `_dev` prototype only. Do not promote to `Assets/Lethe` until this review returns GO.
- Review target: decide `GO`, `ITERATE`, or `NO-GO` for the current slice feel.

## Fast Setup

1. Open `Dev_Prototype_v1`.
2. Press Play.
3. Review both starting weapons:
   - Dual Blades.
   - Greatsword.
4. Use debug/review controls only after at least one normal short run impression.
5. Keep audio on, because July 1 SFX routing is part of the current feel.

## One-Click QA State Before Review

Codex already verified:

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: pass, 7 legacy warnings, 0 errors.
- `dotnet build LETHE/Assembly-CSharp-Editor.csproj --nologo`: pass, 7 legacy warnings, 0 errors.
- Unity compile errors: `0`.
- Unity console errors after latest QA: `0`.
- `LETHE/V1 QA/Echo Matrix Dual Blades`: PASS, `total=240`.
- `LETHE/V1 QA/Echo Matrix Greatsword`: PASS, `total=221`.
- `LETHE/V1 QA/Passive Memory Matrix`: PASS, `blood=17`, `ash=6`, `stopped=8`, `oblivion=36`.
- `LETHE/V1 QA/Forget Resonance Flow`: PASS, `forgetFlow=15`, `echoTransform=14`, `ultimateReady=3`.
- `LETHE/V1 QA/Utility Ultimate Matrix Dual Blades`: PASS, `fracture=22`, `stasis=9`, `ashen=47`.
- `LETHE/V1 QA/Utility Ultimate Matrix Greatsword`: PASS, `fracture=9`, `stasis=20`, `ashen=21`.
- `LETHE/V1 QA/Blood Blade Storm`: PASS, `stormObjects=77`.
- `LETHE/V1 QA/Kalmuri Perf Matrix`: PASS, `totalKalmuri=374`.

## Review Questions

Answer each as `GO`, `ITERATE`, or `NO-GO`, with one short reason.

1. Base weapon feel:
   - Do dual blades and greatsword feel meaningfully different?
   - Does either attack feel too small, too weak, or too visually quiet?

2. Hungry Blades / Kalmuri:
   - Does +5 still feel like a living blade swarm after the performance optimization?
   - Does `totalKalmuri=374` feel rich enough, or did optimization make it too sparse?
   - Are flying blades clearly stabbing through targets rather than just drifting toward them?

3. Passive memories:
   - Do `BloodReflection`, `StoppedSecond`, `AshenShield`, and `OblivionBrand` feel useful before forgetting?
   - Are the new passive beats readable without making the screen tiring?

4. Echo identity:
   - Can you tell which echo family is active by motion/VFX before reading text?
   - Are any echoes still too similar to generic rings, seals, or flashes?

5. Forget / resonance flow:
   - Does forgetting read as an action transition first and text confirmation second?
   - Is the compressed overlay too short, too long, or just right?

6. Ultimates:
   - Does Blood Blade Storm still feel like the strongest benchmark?
   - Do `FractureExecution`, `StasisHunt`, and `AshenOblivion` feel distinct per weapon?
   - Is dual `AshenOblivion` too busy after rising to `ashen=47`?

7. Audio:
   - Are repeated Kalmuri, echo, and ultimate sounds satisfying rather than tiring?
   - Which sound is too loud, too quiet, or too frequent?

8. Performance/readability:
   - Any visible frame hitch when many enemies and Kalmuri are active?
   - Any VFX hiding enemies, player position, health bars, or pickups?

## Decision Output

Use this format:

```text
Decision: GO / ITERATE / NO-GO

Must fix before promotion:
- ...

Nice to improve:
- ...

Strongest current feel:
- ...

Weakest current feel:
- ...
```

## Codex Next Action After Review

- `GO`: prepare `_dev` to `Assets/Lethe` promotion plan.
- `ITERATE`: fix only the top 1-2 feel issues from this checklist.
- `NO-GO`: write a focused rework plan before adding more content.
