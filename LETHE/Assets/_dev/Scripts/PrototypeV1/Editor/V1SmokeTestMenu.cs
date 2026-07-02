using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Lethe.PrototypeV1.Editor
{
    public static class V1SmokeTestMenu
    {
        const double DefaultTimeoutSeconds = 12.0;
        const string ActiveKey = "Lethe.V1Smoke.Active";
        const string ModeKey = "Lethe.V1Smoke.Mode";
        const string WeaponKey = "Lethe.V1Smoke.Weapon";

        static bool invoked;
        static double invokedAt;

        [InitializeOnLoadMethod]
        static void Initialize()
        {
            EditorApplication.update -= Tick;
            EditorApplication.update += Tick;
        }

        [MenuItem("LETHE/V1 Smoke/Start Dual Blades")]
        public static void StartDualBlades()
        {
            BeginStartWeaponSmoke(V1WeaponId.DualBlades);
        }

        [MenuItem("LETHE/V1 Smoke/Start Greatsword")]
        public static void StartGreatsword()
        {
            BeginStartWeaponSmoke(V1WeaponId.Greatsword);
        }

        [MenuItem("LETHE/V1 Smoke/M2 Loop")]
        public static void StartM2Loop()
        {
            SavePending(new PendingSmoke(SmokeMode.M2Loop, V1WeaponId.DualBlades));
            StartRunner();
        }

        [MenuItem("LETHE/V1 QA/Start Dual Blades")]
        public static void QaStartDualBlades()
        {
            BeginStartWeaponSmoke(V1WeaponId.DualBlades);
        }

        [MenuItem("LETHE/V1 QA/Start Greatsword")]
        public static void QaStartGreatsword()
        {
            BeginStartWeaponSmoke(V1WeaponId.Greatsword);
        }

        [MenuItem("LETHE/V1 QA/M2 Loop")]
        public static void QaM2Loop()
        {
            StartM2Loop();
        }

        [MenuItem("LETHE/V1 QA/VFX Matrix")]
        public static void QaVfxMatrix()
        {
            SavePending(new PendingSmoke(SmokeMode.VfxMatrix, V1WeaponId.DualBlades));
            StartRunner();
        }

        [MenuItem("LETHE/V1 QA/Echo Matrix Dual Blades")]
        public static void QaEchoMatrixDualBlades()
        {
            SavePending(new PendingSmoke(SmokeMode.EchoMatrix, V1WeaponId.DualBlades));
            StartRunner();
        }

        [MenuItem("LETHE/V1 QA/Echo Matrix Greatsword")]
        public static void QaEchoMatrixGreatsword()
        {
            SavePending(new PendingSmoke(SmokeMode.EchoMatrix, V1WeaponId.Greatsword));
            StartRunner();
        }

        [MenuItem("LETHE/V1 QA/Passive Memory Matrix")]
        public static void QaPassiveMemoryMatrix()
        {
            SavePending(new PendingSmoke(SmokeMode.PassiveMemoryMatrix, V1WeaponId.DualBlades));
            StartRunner();
        }

        [MenuItem("LETHE/V1 QA/Forget Resonance Flow")]
        public static void QaForgetResonanceFlow()
        {
            SavePending(new PendingSmoke(SmokeMode.ForgetResonanceFlow, V1WeaponId.DualBlades));
            StartRunner();
        }

        [MenuItem("LETHE/V1 QA/Utility Ultimate Matrix Dual Blades")]
        public static void QaUtilityUltimateMatrixDualBlades()
        {
            SavePending(new PendingSmoke(SmokeMode.UtilityUltimateMatrix, V1WeaponId.DualBlades));
            StartRunner();
        }

        [MenuItem("LETHE/V1 QA/Utility Ultimate Matrix Greatsword")]
        public static void QaUtilityUltimateMatrixGreatsword()
        {
            SavePending(new PendingSmoke(SmokeMode.UtilityUltimateMatrix, V1WeaponId.Greatsword));
            StartRunner();
        }

        [MenuItem("LETHE/V1 QA/Blood Blade Storm")]
        public static void QaBloodBladeStorm()
        {
            SavePending(new PendingSmoke(SmokeMode.BloodBladeStorm, V1WeaponId.DualBlades));
            StartRunner();
        }

        static void BeginStartWeaponSmoke(V1WeaponId weaponId)
        {
            SavePending(new PendingSmoke(SmokeMode.StartWeapon, weaponId));
            StartRunner();
        }

        static void StartRunner()
        {
            invoked = false;
            invokedAt = 0;
            EditorApplication.update -= Tick;
            EditorApplication.update += Tick;

            if (!EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = true;
            }
        }

        static void Tick()
        {
            if (!SessionState.GetBool(ActiveKey, false))
            {
                return;
            }

            if (!EditorApplication.isPlaying || EditorApplication.isPaused)
            {
                return;
            }

            var manager = UnityEngine.Object.FindFirstObjectByType<V1GameManager>();
            if (manager == null)
            {
                return;
            }

            var smoke = LoadPending();
            if (!invoked)
            {
                invoked = true;
                invokedAt = EditorApplication.timeSinceStartup;
                RunPending(manager, smoke);
                Debug.Log($"[V1Smoke] invoked {smoke.Label}: {manager.DebugSnapshot()}");
            }

            var age = EditorApplication.timeSinceStartup - invokedAt;
            var result = EvaluatePending(manager, smoke, age, out var details);
            if (result == SmokeResult.Pending && age < DefaultTimeoutSeconds)
            {
                return;
            }

            if (result == SmokeResult.Pass)
            {
                Debug.Log($"[V1QA] PASS {smoke.Label}: {details}");
            }
            else
            {
                Debug.LogError($"[V1QA] FAIL {smoke.Label}: {details}");
            }

            Finish();
        }

        static void RunPending(V1GameManager manager, PendingSmoke smoke)
        {
            if (smoke.Mode == SmokeMode.M2Loop)
            {
                manager.DebugRunM2Smoke();
                return;
            }

            if (smoke.Mode == SmokeMode.VfxMatrix)
            {
                BeginRun(manager, V1WeaponId.DualBlades);
                SpawnAllMemoryEchoPreviews(manager);
                Invoke(manager, "SpawnUtilityUltimatePreview");
                return;
            }

            if (smoke.Mode == SmokeMode.BloodBladeStorm)
            {
                BeginRun(manager, smoke.WeaponId);
                Invoke(manager, "EnsureReviewEnemies", new[] { typeof(int) }, 18);
                Invoke(manager, "SetEcho", new[] { typeof(V1MemoryId), typeof(int) }, V1MemoryId.HungryBlades, 5);
                Invoke(manager, "SetEcho", new[] { typeof(V1MemoryId), typeof(int) }, V1MemoryId.BloodReflection, 5);
                for (int i = 0; i < 6; i++)
                {
                    Invoke(manager, "UpdateEchoUltimate", new[] { typeof(float) }, 0.12f);
                }
                return;
            }

            if (smoke.Mode == SmokeMode.EchoMatrix)
            {
                manager.DebugRunEchoMatrix(smoke.WeaponId);
                return;
            }

            if (smoke.Mode == SmokeMode.PassiveMemoryMatrix)
            {
                manager.DebugRunPassiveMemoryMatrix();
                return;
            }

            if (smoke.Mode == SmokeMode.ForgetResonanceFlow)
            {
                manager.DebugRunForgetResonanceFlow();
                return;
            }

            if (smoke.Mode == SmokeMode.UtilityUltimateMatrix)
            {
                manager.DebugRunUtilityUltimateMatrix(smoke.WeaponId);
                return;
            }

            BeginRun(manager, smoke.WeaponId);
            AdvanceStartSmoke(manager, 2.1f);
        }

        static void BeginRun(V1GameManager manager, V1WeaponId weaponId)
        {
            var method = typeof(V1GameManager).GetMethod(
                "BeginRun",
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new[] { typeof(V1WeaponId) },
                null);
            if (method == null)
            {
                throw new MissingMethodException(nameof(V1GameManager), "BeginRun(V1WeaponId)");
            }

            method.Invoke(manager, new object[] { weaponId });
        }

        static SmokeResult EvaluatePending(V1GameManager manager, PendingSmoke smoke, double age, out string details)
        {
            var snapshot = manager.DebugSnapshot();
            var liveEnemies = LiveEnemyCount(manager);
            var elapsed = Field<float>(manager, "elapsed");
            var resultOverlay = Field<bool>(manager, "resultOverlay");
            var refillOverlay = Field<bool>(manager, "refillOverlay");
            var deathOverlay = Field<bool>(manager, "deathOverlay");

            details = $"{snapshot} | age={age:0.0}s liveEnemies={liveEnemies} timeScale={Time.timeScale:0.00}";

            switch (smoke.Mode)
            {
                case SmokeMode.StartWeapon:
                    if (elapsed >= 2f && liveEnemies >= 5 && Time.timeScale > 0.99f && !resultOverlay && !refillOverlay && !deathOverlay)
                    {
                        return SmokeResult.Pass;
                    }
                    return age >= DefaultTimeoutSeconds ? SmokeResult.Fail : SmokeResult.Pending;

                case SmokeMode.M2Loop:
                    details += $" | hungryEcho={EchoLevel(manager, V1MemoryId.HungryBlades)} bloodEcho={EchoLevel(manager, V1MemoryId.BloodReflection)}";
                    if (EchoLevel(manager, V1MemoryId.HungryBlades) >= 5 && EchoLevel(manager, V1MemoryId.BloodReflection) >= 5 && manager.BloodBladeStormReady && resultOverlay && liveEnemies >= 8)
                    {
                        return SmokeResult.Pass;
                    }
                    return age >= DefaultTimeoutSeconds ? SmokeResult.Fail : SmokeResult.Pending;

                case SmokeMode.VfxMatrix:
                    var missing = MissingPreviewIds();
                    details += $" | previewMemory={CountObjects("PreviewMemory_")} previewEcho={CountObjects("PreviewEcho_")} fracture={CountObjects("Preview_FractureExecution")} stasis={CountObjects("Preview_StasisHunt")} ashen={CountObjects("Preview_AshenOblivion")} missing={missing}";
                    if (missing.Length == 0 && CountObjects("Preview_FractureExecution") > 0 && CountObjects("Preview_StasisHunt") > 0 && CountObjects("Preview_AshenOblivion") > 0)
                    {
                        return SmokeResult.Pass;
                    }
                    return age >= DefaultTimeoutSeconds ? SmokeResult.Fail : SmokeResult.Pending;

                case SmokeMode.BloodBladeStorm:
                    var stormObjects = CountObjects("BloodBladeStorm");
                    details += $" | stormObjects={stormObjects} hungryEcho={EchoLevel(manager, V1MemoryId.HungryBlades)} bloodEcho={EchoLevel(manager, V1MemoryId.BloodReflection)}";
                    if (manager.BloodBladeStormReady && stormObjects >= 8 && liveEnemies >= 1)
                    {
                        return SmokeResult.Pass;
                    }
                    return age >= DefaultTimeoutSeconds ? SmokeResult.Fail : SmokeResult.Pending;

                case SmokeMode.EchoMatrix:
                    var prefix = smoke.WeaponId == V1WeaponId.Greatsword ? "EchoGreat_" : "EchoDual_";
                    var prefixObjects = CountObjects(prefix);
                    var kalmuri = CountObjects($"{prefix}Kalmuri");
                    var blood = CountObjects($"{prefix}Blood");
                    var execution = CountObjects($"{prefix}Execution");
                    var hunter = CountObjects($"{prefix}Hunter");
                    var shatter = CountObjects($"{prefix}Shatter");
                    var stopped = CountObjects($"{prefix}Stopped");
                    var ashen = CountObjects($"{prefix}Ashen");
                    var oblivion = CountObjects($"{prefix}Oblivion");
                    details += $" | prefix={prefix} total={prefixObjects} K={kalmuri} B={blood} Ex={execution} H={hunter} Sh={shatter} St={stopped} A={ashen} O={oblivion}";
                    if (prefixObjects >= 12 && kalmuri > 0 && blood > 0 && execution > 0 && hunter > 0 && shatter > 0 && stopped > 0 && ashen > 0 && oblivion > 0)
                    {
                        return SmokeResult.Pass;
                    }
                    return age >= DefaultTimeoutSeconds ? SmokeResult.Fail : SmokeResult.Pending;

                case SmokeMode.PassiveMemoryMatrix:
                    var bloodMemory = CountObjects("MemoryBloodReflection");
                    var ashMemory = CountObjects("MemoryAshenShield");
                    var stoppedMemory = CountObjects("MemoryStoppedSecond");
                    var oblivionMemory = CountObjects("MemoryOblivionBrand");
                    details += $" | passiveMemory blood={bloodMemory} ash={ashMemory} stopped={stoppedMemory} oblivion={oblivionMemory}";
                    if (bloodMemory > 0 && ashMemory > 0 && stoppedMemory > 0 && oblivionMemory > 0)
                    {
                        return SmokeResult.Pass;
                    }
                    return age >= DefaultTimeoutSeconds ? SmokeResult.Fail : SmokeResult.Pending;

                case SmokeMode.ForgetResonanceFlow:
                    var forgetFlow = CountObjects("ForgetFlow");
                    var echoTransform = CountObjects("EchoTransform");
                    var ultimateReady = CountObjects("UltimateReady");
                    details += $" | forgetFlow={forgetFlow} echoTransform={echoTransform} ultimateReady={ultimateReady} hungryEcho={EchoLevel(manager, V1MemoryId.HungryBlades)} bloodEcho={EchoLevel(manager, V1MemoryId.BloodReflection)}";
                    if (resultOverlay && manager.BloodBladeStormReady && forgetFlow >= 8 && echoTransform > 0 && ultimateReady > 0)
                    {
                        return SmokeResult.Pass;
                    }
                    return age >= DefaultTimeoutSeconds ? SmokeResult.Fail : SmokeResult.Pending;

                case SmokeMode.UtilityUltimateMatrix:
                    var ultPrefix = smoke.WeaponId == V1WeaponId.Greatsword ? "UltGreat_" : "UltDual_";
                    var fracture = CountObjects($"{ultPrefix}FractureExecution");
                    var stasis = CountObjects($"{ultPrefix}StasisHunt");
                    var ashenUlt = CountObjects($"{ultPrefix}AshenOblivion");
                    details += $" | ultPrefix={ultPrefix} fracture={fracture} stasis={stasis} ashen={ashenUlt}";
                    if (fracture > 0 && stasis > 0 && ashenUlt > 0)
                    {
                        return SmokeResult.Pass;
                    }
                    return age >= DefaultTimeoutSeconds ? SmokeResult.Fail : SmokeResult.Pending;

                default:
                    return SmokeResult.Fail;
            }
        }

        static void Finish()
        {
            SessionState.SetBool(ActiveKey, false);
            invoked = false;
            EditorApplication.update -= Tick;
            EditorApplication.isPlaying = false;
        }

        static void SavePending(PendingSmoke smoke)
        {
            SessionState.SetBool(ActiveKey, true);
            SessionState.SetInt(ModeKey, (int)smoke.Mode);
            SessionState.SetInt(WeaponKey, (int)smoke.WeaponId);
        }

        static PendingSmoke LoadPending()
        {
            return new PendingSmoke(
                (SmokeMode)SessionState.GetInt(ModeKey, (int)SmokeMode.StartWeapon),
                (V1WeaponId)SessionState.GetInt(WeaponKey, (int)V1WeaponId.DualBlades));
        }

        enum SmokeMode
        {
            StartWeapon,
            M2Loop,
            VfxMatrix,
            EchoMatrix,
            PassiveMemoryMatrix,
            ForgetResonanceFlow,
            UtilityUltimateMatrix,
            BloodBladeStorm
        }

        enum SmokeResult
        {
            Pending,
            Pass,
            Fail
        }

        readonly struct PendingSmoke
        {
            public PendingSmoke(SmokeMode mode, V1WeaponId weaponId)
            {
                Mode = mode;
                WeaponId = weaponId;
            }

            public SmokeMode Mode { get; }
            public V1WeaponId WeaponId { get; }

            public string Label => Mode == SmokeMode.M2Loop
                ? "M2 Loop"
                : Mode == SmokeMode.VfxMatrix
                    ? "VFX Matrix"
                    : Mode == SmokeMode.EchoMatrix
                        ? $"Echo Matrix {WeaponId}"
                        : Mode == SmokeMode.PassiveMemoryMatrix
                            ? "Passive Memory Matrix"
                            : Mode == SmokeMode.ForgetResonanceFlow
                                ? "Forget Resonance Flow"
                                : Mode == SmokeMode.UtilityUltimateMatrix
                                    ? $"Utility Ultimate Matrix {WeaponId}"
                                    : Mode == SmokeMode.BloodBladeStorm
                                        ? "Blood Blade Storm"
                                        : $"{WeaponId}";
        }

        static void SpawnAllMemoryEchoPreviews(V1GameManager manager)
        {
            var ids = (V1MemoryId[])Enum.GetValues(typeof(V1MemoryId));
            var player = Field<Transform>(manager, "player");
            var center = player != null ? player.position : Vector3.zero;
            for (int i = 0; i < ids.Length; i++)
            {
                var angle = i * (360f / ids.Length);
                var pos = center + Quaternion.Euler(0f, 0f, angle) * Vector3.right * 2.8f;
                Invoke(manager, "SpawnOneUtilityPreview", new[] { typeof(V1MemoryId), typeof(Vector3), typeof(bool), typeof(float) }, ids[i], pos, false, angle);
                Invoke(manager, "SpawnOneUtilityPreview", new[] { typeof(V1MemoryId), typeof(Vector3), typeof(bool), typeof(float) }, ids[i], pos + Vector3.up * 0.7f, true, angle + 18f);
            }
        }

        static string MissingPreviewIds()
        {
            var ids = (V1MemoryId[])Enum.GetValues(typeof(V1MemoryId));
            var missing = new System.Text.StringBuilder();
            foreach (var id in ids)
            {
                if (CountObjects($"PreviewMemory_{id}") <= 0)
                {
                    if (missing.Length > 0) missing.Append(",");
                    missing.Append("M:").Append(id);
                }
                if (CountObjects($"PreviewEcho_{id}") <= 0)
                {
                    if (missing.Length > 0) missing.Append(",");
                    missing.Append("E:").Append(id);
                }
            }
            return missing.ToString();
        }

        static int EchoLevel(V1GameManager manager, V1MemoryId id)
        {
            var levels = Field<System.Collections.Generic.Dictionary<V1MemoryId, int>>(manager, "echoLevels");
            return levels != null && levels.TryGetValue(id, out var value) ? value : 0;
        }

        static int LiveEnemyCount(V1GameManager manager)
        {
            var enemies = Field<System.Collections.Generic.List<V1Enemy>>(manager, "enemies");
            return enemies == null ? 0 : enemies.FindAll(enemy => enemy != null && enemy.IsAlive).Count;
        }

        static int CountObjects(string namePart)
        {
            var count = 0;
            foreach (var transform in UnityEngine.Object.FindObjectsByType<Transform>(FindObjectsSortMode.None))
            {
                if (transform != null && transform.gameObject.activeInHierarchy && transform.name.Contains(namePart, StringComparison.Ordinal))
                {
                    count++;
                }
            }
            return count;
        }

        static void AdvanceStartSmoke(V1GameManager manager, float seconds)
        {
            const float dt = 0.1f;
            var steps = Mathf.CeilToInt(seconds / dt);
            for (int i = 0; i < steps; i++)
            {
                SetField(manager, "elapsed", Field<float>(manager, "elapsed") + dt);
                Invoke(manager, "UpdateSpawning", new[] { typeof(float) }, dt);
                Invoke(manager, "CleanupLists");
            }
        }

        static T Field<T>(V1GameManager manager, string name)
        {
            var field = typeof(V1GameManager).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
            return field != null && field.GetValue(manager) is T value ? value : default;
        }

        static void SetField<T>(V1GameManager manager, string name, T value)
        {
            var field = typeof(V1GameManager).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null)
            {
                throw new MissingFieldException(nameof(V1GameManager), name);
            }

            field.SetValue(manager, value);
        }

        static object Invoke(V1GameManager manager, string name)
        {
            return Invoke(manager, name, Type.EmptyTypes);
        }

        static object Invoke(V1GameManager manager, string name, Type[] parameterTypes, params object[] args)
        {
            var method = typeof(V1GameManager).GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic, null, parameterTypes, null);
            if (method == null)
            {
                throw new MissingMethodException(nameof(V1GameManager), name);
            }

            return method.Invoke(manager, args);
        }
    }
}
