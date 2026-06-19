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
                new[] { typeof(V1WeaponId) },
                null);

            if (method == null)
            {
                throw new MissingMethodException(nameof(V1GameManager), "BeginRun(V1WeaponId)");
            }

            method.Invoke(manager, new object[] { smoke.WeaponId });
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
            M2Loop
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
                : $"{WeaponId}";
        }
    }
}
