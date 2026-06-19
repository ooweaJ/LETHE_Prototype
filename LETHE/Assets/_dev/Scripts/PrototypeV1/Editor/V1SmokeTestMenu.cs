using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Lethe.PrototypeV1.Editor
{
    public static class V1SmokeTestMenu
    {
        const double ResultDelaySeconds = 2.25;
        const string ActiveKey = "Lethe.V1Smoke.Active";
        const string ModeKey = "Lethe.V1Smoke.Mode";
        const string WeaponKey = "Lethe.V1Smoke.Weapon";
        const string MemoryKey = "Lethe.V1Smoke.Memory";

        static bool invoked;
        static double invokedAt;

        [InitializeOnLoadMethod]
        static void Initialize()
        {
            EditorApplication.update -= Tick;
            EditorApplication.update += Tick;
        }

        [MenuItem("LETHE/V1 Smoke/Start Dual Kalmuri")]
        public static void StartDualKalmuri()
        {
            BeginStartBuildSmoke(V1WeaponId.DualBlades, V1MemoryId.HungryBlades);
        }

        [MenuItem("LETHE/V1 Smoke/Start Dual Blood")]
        public static void StartDualBlood()
        {
            BeginStartBuildSmoke(V1WeaponId.DualBlades, V1MemoryId.BloodReflection);
        }

        [MenuItem("LETHE/V1 Smoke/Start Great Kalmuri")]
        public static void StartGreatKalmuri()
        {
            BeginStartBuildSmoke(V1WeaponId.Greatsword, V1MemoryId.HungryBlades);
        }

        [MenuItem("LETHE/V1 Smoke/Start Great Blood")]
        public static void StartGreatBlood()
        {
            BeginStartBuildSmoke(V1WeaponId.Greatsword, V1MemoryId.BloodReflection);
        }

        [MenuItem("LETHE/V1 Smoke/M2 Loop")]
        public static void StartM2Loop()
        {
            SavePending(new PendingSmoke(SmokeMode.M2Loop, V1WeaponId.DualBlades, V1MemoryId.HungryBlades));
            StartRunner();
        }

        static void BeginStartBuildSmoke(V1WeaponId weaponId, V1MemoryId memoryId)
        {
            SavePending(new PendingSmoke(SmokeMode.StartBuild, weaponId, memoryId));
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
                return;
            }

            if (EditorApplication.timeSinceStartup - invokedAt < ResultDelaySeconds)
            {
                return;
            }

            Debug.Log($"[V1Smoke] result {smoke.Label}: {manager.DebugSnapshot()}");
            Finish();
        }

        static void RunPending(V1GameManager manager, PendingSmoke smoke)
        {
            if (smoke.Mode == SmokeMode.M2Loop)
            {
                manager.DebugRunM2Smoke();
                return;
            }

            var method = typeof(V1GameManager).GetMethod(
                "BeginRun",
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new[] { typeof(V1WeaponId), typeof(V1MemoryId) },
                null);

            if (method == null)
            {
                throw new MissingMethodException(nameof(V1GameManager), "BeginRun(V1WeaponId,V1MemoryId)");
            }

            method.Invoke(manager, new object[] { smoke.WeaponId, smoke.MemoryId });
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
            SessionState.SetInt(MemoryKey, (int)smoke.MemoryId);
        }

        static PendingSmoke LoadPending()
        {
            return new PendingSmoke(
                (SmokeMode)SessionState.GetInt(ModeKey, (int)SmokeMode.StartBuild),
                (V1WeaponId)SessionState.GetInt(WeaponKey, (int)V1WeaponId.DualBlades),
                (V1MemoryId)SessionState.GetInt(MemoryKey, (int)V1MemoryId.HungryBlades));
        }

        enum SmokeMode
        {
            StartBuild,
            M2Loop
        }

        readonly struct PendingSmoke
        {
            public PendingSmoke(SmokeMode mode, V1WeaponId weaponId, V1MemoryId memoryId)
            {
                Mode = mode;
                WeaponId = weaponId;
                MemoryId = memoryId;
            }

            public SmokeMode Mode { get; }
            public V1WeaponId WeaponId { get; }
            public V1MemoryId MemoryId { get; }

            public string Label => Mode == SmokeMode.M2Loop
                ? "M2 Loop"
                : $"{WeaponId} + {MemoryId}";
        }
    }
}
