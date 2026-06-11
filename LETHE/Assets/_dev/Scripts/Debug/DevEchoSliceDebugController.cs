using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lethe.Dev
{
    public enum DevEchoSliceMode
    {
        BaseDualBlades = 0,
        KalmuriOne = 1,
        KalmuriFive = 2,
        BloodFive = 3,
        BloodBladeStorm = 4
    }

    public sealed class DevEchoSliceDebugController : MonoBehaviour
    {
        private const string KalmuriEchoId = "Echo_Kalmuri";
        private const string BloodEchoId = "Echo_Blood";
        private const string HungryBladesMemoryId = "Memory_HungryBlades";
        private const string BloodBladeStormId = "Synergy_BloodBladeStorm";

        [Header("Scene")]
        [SerializeField] private RunBuildState buildState;
        [SerializeField] private HitResolver hitResolver;
        [SerializeField] private PoolService poolService;
        [SerializeField] private DualBladesController dualBlades;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject targetEnemy;
        [SerializeField] private Transform echoAnchor;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private bool autoAttack = true;

        [Header("Kalmuri")]
        [SerializeField] private GameObject kalmuriSlashPrefab;
        [SerializeField] private GameObject kalmuriOrbitBladePrefab;
        [SerializeField] private GameObject kalmuriLaunchBladePrefab;

        [Header("Blood")]
        [SerializeField] private GameObject bloodMarkPrefab;
        [SerializeField] private GameObject healThreadTipPrefab;
        [SerializeField] private GameObject bloodBloomPrefab;

        [Header("Ultimate")]
        [SerializeField] private GameObject bloodBladeStormPrefab;

        [Header("Tuning")]
        [SerializeField] private DevEchoSliceMode currentMode;
        [SerializeField] private float slashLifetime = 0.45f;
        [SerializeField] private float launchLifetime = 0.55f;
        [SerializeField] private float bloodMarkLifetime = 0.8f;
        [SerializeField] private float bloomLifetime = 0.5f;
        [SerializeField] private float healThreadLifetime = 0.35f;
        [SerializeField] private float persistentOrbitRadius = 0.85f;
        [SerializeField] private float orbitSpeed = 140f;
        [SerializeField] private float delayedSlashDelay = 0.12f;
        [SerializeField] private int bloodThreadCount = 3;
        [SerializeField] private int debugHitStopFrames = 2;
        [SerializeField] private float cameraShakeDuration = 0.08f;
        [SerializeField] private float cameraShakeMagnitude = 0.045f;

        private readonly List<GameObject> persistentObjects = new List<GameObject>();
        private Material healThreadMaterial;
        private Material swingArcMaterial;
        private Coroutine cameraShakeRoutine;
        private Coroutine hitStopRoutine;
        private float orbitPulseRemaining;

        private void Awake()
        {
            FindFallbackReferences();
        }

        private void OnEnable()
        {
            if (hitResolver != null)
            {
                hitResolver.HitResolved += HandleHitResolved;
            }
        }

        private void OnDisable()
        {
            if (hitResolver != null)
            {
                hitResolver.HitResolved -= HandleHitResolved;
            }
        }

        private void Start()
        {
            if (dualBlades != null)
            {
                dualBlades.Bind(hitResolver, targetEnemy);
                dualBlades.SetAutoAttack(autoAttack);
            }

            SetMode(currentMode);
        }

        private void Update()
        {
            if (WasPressed(DevDebugKey.Digit1))
            {
                SetMode(DevEchoSliceMode.BaseDualBlades);
            }
            else if (WasPressed(DevDebugKey.Digit2))
            {
                SetMode(DevEchoSliceMode.KalmuriOne);
            }
            else if (WasPressed(DevDebugKey.Digit3))
            {
                SetMode(DevEchoSliceMode.KalmuriFive);
            }
            else if (WasPressed(DevDebugKey.Digit4))
            {
                SetMode(DevEchoSliceMode.BloodFive);
            }
            else if (WasPressed(DevDebugKey.Digit5))
            {
                SetMode(DevEchoSliceMode.BloodBladeStorm);
            }
            else if (WasPressed(DevDebugKey.Space))
            {
                dualBlades?.Attack(targetEnemy);
            }

            RotatePersistentObjects();
        }

        private static bool WasPressed(DevDebugKey key)
        {
#if ENABLE_INPUT_SYSTEM
            var keyboard = UnityEngine.InputSystem.Keyboard.current;
            if (keyboard != null)
            {
                return key switch
                {
                    DevDebugKey.Digit1 => keyboard.digit1Key.wasPressedThisFrame,
                    DevDebugKey.Digit2 => keyboard.digit2Key.wasPressedThisFrame,
                    DevDebugKey.Digit3 => keyboard.digit3Key.wasPressedThisFrame,
                    DevDebugKey.Digit4 => keyboard.digit4Key.wasPressedThisFrame,
                    DevDebugKey.Digit5 => keyboard.digit5Key.wasPressedThisFrame,
                    DevDebugKey.Space => keyboard.spaceKey.wasPressedThisFrame,
                    _ => false
                };
            }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            return key switch
            {
                DevDebugKey.Digit1 => Input.GetKeyDown(KeyCode.Alpha1),
                DevDebugKey.Digit2 => Input.GetKeyDown(KeyCode.Alpha2),
                DevDebugKey.Digit3 => Input.GetKeyDown(KeyCode.Alpha3),
                DevDebugKey.Digit4 => Input.GetKeyDown(KeyCode.Alpha4),
                DevDebugKey.Digit5 => Input.GetKeyDown(KeyCode.Alpha5),
                DevDebugKey.Space => Input.GetKeyDown(KeyCode.Space),
                _ => false
            };
#else
            return false;
#endif
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(8f, 8f, 214f, 104f), GUI.skin.box);
            GUILayout.Label($"Mode: {currentMode}");
            GUILayout.Label("Move WASD | Hit Space");
            GUILayout.Label("1 Base 2 K+1 3 K+5");
            GUILayout.Label("4 Blood+5 5 Storm");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("1")) SetMode(DevEchoSliceMode.BaseDualBlades);
            if (GUILayout.Button("2")) SetMode(DevEchoSliceMode.KalmuriOne);
            if (GUILayout.Button("3")) SetMode(DevEchoSliceMode.KalmuriFive);
            if (GUILayout.Button("4")) SetMode(DevEchoSliceMode.BloodFive);
            if (GUILayout.Button("5")) SetMode(DevEchoSliceMode.BloodBladeStorm);
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        public void SetMode(int mode)
        {
            SetMode((DevEchoSliceMode)Mathf.Clamp(mode, 0, 4));
        }

        public void SetMode(DevEchoSliceMode mode)
        {
            currentMode = mode;
            ClearPersistentObjects();
            ApplyBuildState(mode);

            if (mode == DevEchoSliceMode.KalmuriFive)
            {
                CreateKalmuriOrbit(3, persistentOrbitRadius);
            }
            else if (mode == DevEchoSliceMode.BloodBladeStorm)
            {
                CreateKalmuriOrbit(3, persistentOrbitRadius * 0.9f);
                CreatePersistentPrefab(bloodBladeStormPrefab, echoAnchor != null ? echoAnchor.position : PlayerPosition(), Quaternion.identity);
            }
        }

        public void ForceAttack()
        {
            dualBlades?.Attack(targetEnemy);
        }

        private void HandleHitResolved(HitEvent hitEvent)
        {
            if (hitEvent.sourceType != HitSourceType.WeaponHit)
            {
                return;
            }

            var direction = hitEvent.direction.sqrMagnitude > 0f ? hitEvent.direction.normalized : Vector2.right;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var hitPosition = hitEvent.target != null ? hitEvent.target.transform.position : (Vector3)hitEvent.position;

            SpawnSwingArc(hitPosition, direction);
            PulseHitFeedback();

            if (currentMode == DevEchoSliceMode.KalmuriOne)
            {
                StartCoroutine(SpawnDelayed(kalmuriSlashPrefab, hitPosition + (Vector3)(direction * -0.2f), Quaternion.Euler(0f, 0f, angle), slashLifetime, delayedSlashDelay));
            }
            else if (currentMode == DevEchoSliceMode.KalmuriFive)
            {
                SpawnTimed(kalmuriSlashPrefab, hitPosition, Quaternion.Euler(0f, 0f, angle), slashLifetime);
                SpawnTimed(kalmuriLaunchBladePrefab, hitPosition + (Vector3)(direction * 0.35f), Quaternion.Euler(0f, 0f, angle), launchLifetime);
                orbitPulseRemaining = 0.18f;
            }
            else if (currentMode == DevEchoSliceMode.BloodFive)
            {
                SpawnBloodHit(hitPosition, hitEvent.target);
            }
            else if (currentMode == DevEchoSliceMode.BloodBladeStorm)
            {
                SpawnTimed(kalmuriLaunchBladePrefab, hitPosition + (Vector3)(direction * 0.4f), Quaternion.Euler(0f, 0f, angle), launchLifetime);
                SpawnBloodHit(hitPosition, hitEvent.target);
                orbitPulseRemaining = 0.22f;
            }
        }

        private void SpawnBloodHit(Vector3 hitPosition, GameObject target)
        {
            SpawnTimed(bloodMarkPrefab, hitPosition + Vector3.up * 0.05f, Quaternion.identity, bloodMarkLifetime, target != null ? target.transform : null);
            SpawnTimed(bloodBloomPrefab, hitPosition, Quaternion.identity, bloomLifetime);
            var threads = Mathf.Max(1, bloodThreadCount);
            for (var index = 0; index < threads; index += 1)
            {
                var offset = threads == 1 ? 0f : (index - (threads - 1) * 0.5f) * 0.12f;
                var from = hitPosition + new Vector3(offset, Mathf.Abs(offset) * 0.35f, 0f);
                var to = PlayerPosition() + new Vector3(-offset * 0.4f, 0.08f * index, 0f);
                StartCoroutine(HealThread(from, to, healThreadLifetime + index * 0.04f));
            }
        }

        private void ApplyBuildState(DevEchoSliceMode mode)
        {
            if (buildState == null)
            {
                return;
            }

            buildState.SetEchoLevel(KalmuriEchoId, 0);
            buildState.SetEchoLevel(BloodEchoId, 0);
            buildState.RemoveActiveMemory(HungryBladesMemoryId);

            if (mode == DevEchoSliceMode.KalmuriOne)
            {
                buildState.SetEchoLevel(KalmuriEchoId, 1);
            }
            else if (mode == DevEchoSliceMode.KalmuriFive)
            {
                buildState.SetEchoLevel(KalmuriEchoId, 5);
                buildState.SetActiveMemoryLevel(HungryBladesMemoryId, 5);
            }
            else if (mode == DevEchoSliceMode.BloodFive)
            {
                buildState.SetEchoLevel(BloodEchoId, 5);
            }
            else if (mode == DevEchoSliceMode.BloodBladeStorm)
            {
                buildState.SetEchoLevel(KalmuriEchoId, 5);
                buildState.SetEchoLevel(BloodEchoId, 5);
                buildState.SetActiveMemoryLevel(HungryBladesMemoryId, 5);
                buildState.UnlockSynergy(BloodBladeStormId);
            }
        }

        private GameObject SpawnTimed(GameObject prefab, Vector3 position, Quaternion rotation, float lifetime, Transform parent = null)
        {
            if (prefab == null)
            {
                return null;
            }

            var instance = poolService != null
                ? poolService.Spawn(prefab, position, rotation, parent)
                : Instantiate(prefab, position, rotation, parent);

            if (instance != null)
            {
                StartCoroutine(DespawnAfter(prefab, instance, lifetime));
            }

            return instance;
        }

        private IEnumerator SpawnDelayed(GameObject prefab, Vector3 position, Quaternion rotation, float lifetime, float delay)
        {
            yield return new WaitForSeconds(delay);
            SpawnTimed(prefab, position, rotation, lifetime);
        }

        private IEnumerator DespawnAfter(GameObject prefab, GameObject instance, float lifetime)
        {
            yield return new WaitForSeconds(lifetime);
            if (poolService != null)
            {
                poolService.Despawn(prefab, instance);
            }
            else if (instance != null)
            {
                Destroy(instance);
            }
        }

        private void SpawnSwingArc(Vector3 hitPosition, Vector2 direction)
        {
            var normal = new Vector3(-direction.y, direction.x, 0f);
            var forward = new Vector3(direction.x, direction.y, 0f);
            var center = hitPosition - forward * 0.18f;
            var lineObject = new GameObject("Debug_DualBladeSwingArc");
            var line = lineObject.AddComponent<LineRenderer>();
            line.positionCount = 5;
            line.startWidth = 0.055f;
            line.endWidth = 0.012f;
            line.startColor = new Color(0.85f, 0.96f, 1f, 0.9f);
            line.endColor = new Color(0.55f, 0.8f, 1f, 0.2f);
            line.sortingOrder = 18;
            line.material = GetSwingArcMaterial();

            for (var index = 0; index < line.positionCount; index += 1)
            {
                var t = index / (float)(line.positionCount - 1);
                var side = Mathf.Lerp(-0.45f, 0.45f, t);
                var bow = Mathf.Sin(t * Mathf.PI) * 0.22f;
                line.SetPosition(index, center + normal * side + forward * bow);
            }

            Destroy(lineObject, 0.09f);
        }

        private IEnumerator HealThread(Vector3 from, Vector3 to, float lifetime)
        {
            var lineObject = new GameObject("Debug_HealThreadLine");
            var line = lineObject.AddComponent<LineRenderer>();
            line.positionCount = 2;
            line.startWidth = 0.035f;
            line.endWidth = 0.012f;
            line.startColor = new Color(1f, 0.08f, 0.12f, 0.9f);
            line.endColor = new Color(1f, 0.35f, 0.35f, 0.25f);
            line.sortingOrder = 24;
            line.material = GetHealThreadMaterial();

            var tip = SpawnTimed(healThreadTipPrefab, from, Quaternion.identity, lifetime);
            var elapsed = 0f;
            while (elapsed < lifetime)
            {
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / lifetime);
                var current = Vector3.Lerp(from, to, t);
                line.SetPosition(0, current);
                line.SetPosition(1, to);
                if (tip != null)
                {
                    tip.transform.position = current;
                }

                yield return null;
            }

            Destroy(lineObject);
        }

        private Material GetHealThreadMaterial()
        {
            if (healThreadMaterial == null)
            {
                var shader = Shader.Find("Sprites/Default");
                healThreadMaterial = new Material(shader);
            }

            return healThreadMaterial;
        }

        private Material GetSwingArcMaterial()
        {
            if (swingArcMaterial == null)
            {
                var shader = Shader.Find("Sprites/Default");
                swingArcMaterial = new Material(shader);
            }

            return swingArcMaterial;
        }

        private void PulseHitFeedback()
        {
            if (hitStopRoutine == null && debugHitStopFrames > 0)
            {
                hitStopRoutine = StartCoroutine(DebugHitStop(debugHitStopFrames));
            }

            if (cameraTransform != null && cameraShakeDuration > 0f && cameraShakeMagnitude > 0f)
            {
                if (cameraShakeRoutine != null)
                {
                    StopCoroutine(cameraShakeRoutine);
                }

                cameraShakeRoutine = StartCoroutine(CameraShake(cameraShakeDuration, cameraShakeMagnitude));
            }
        }

        private IEnumerator DebugHitStop(int frames)
        {
            var previousScale = Time.timeScale;
            Time.timeScale = 0f;
            for (var index = 0; index < frames; index += 1)
            {
                yield return null;
            }

            Time.timeScale = previousScale;
            hitStopRoutine = null;
        }

        private IEnumerator CameraShake(float duration, float magnitude)
        {
            var startPosition = cameraTransform.localPosition;
            var elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                var falloff = 1f - Mathf.Clamp01(elapsed / duration);
                var shake = Random.insideUnitCircle * magnitude * falloff;
                cameraTransform.localPosition = startPosition + new Vector3(shake.x, shake.y, 0f);
                yield return null;
            }

            cameraTransform.localPosition = startPosition;
            cameraShakeRoutine = null;
        }

        private void CreateKalmuriOrbit(int count, float radius)
        {
            if (kalmuriOrbitBladePrefab == null)
            {
                return;
            }

            var root = new GameObject("Debug_KalmuriOrbit");
            root.transform.position = echoAnchor != null ? echoAnchor.position : PlayerPosition();
            persistentObjects.Add(root);

            for (var index = 0; index < count; index += 1)
            {
                var angle = (360f / count) * index;
                var rad = angle * Mathf.Deg2Rad;
                var blade = Instantiate(kalmuriOrbitBladePrefab, root.transform);
                blade.name = $"OrbitBlade_{index + 1}";
                blade.transform.localPosition = new Vector3(Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius, 0f);
                blade.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
            }
        }

        private void CreatePersistentPrefab(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (prefab == null)
            {
                return;
            }

            var instance = Instantiate(prefab, position, rotation);
            persistentObjects.Add(instance);
        }

        private void RotatePersistentObjects()
        {
            var center = echoAnchor != null ? echoAnchor.position : PlayerPosition();
            if (orbitPulseRemaining > 0f)
            {
                orbitPulseRemaining = Mathf.Max(0f, orbitPulseRemaining - Time.unscaledDeltaTime);
            }

            var pulse = orbitPulseRemaining > 0f ? 1f + Mathf.Sin((orbitPulseRemaining / 0.22f) * Mathf.PI) * 0.16f : 1f;
            for (var index = 0; index < persistentObjects.Count; index += 1)
            {
                var item = persistentObjects[index];
                if (item == null)
                {
                    continue;
                }

                item.transform.position = center;
                item.transform.localScale = Vector3.one * pulse;
                item.transform.Rotate(0f, 0f, orbitSpeed * pulse * Time.deltaTime);
            }
        }

        private void ClearPersistentObjects()
        {
            for (var index = 0; index < persistentObjects.Count; index += 1)
            {
                if (persistentObjects[index] != null)
                {
                    Destroy(persistentObjects[index]);
                }
            }

            persistentObjects.Clear();
        }

        private Vector3 PlayerPosition()
        {
            return player != null ? player.transform.position : transform.position;
        }

        private void FindFallbackReferences()
        {
            buildState ??= FindObjectOfType<RunBuildState>();
            hitResolver ??= FindObjectOfType<HitResolver>();
            poolService ??= FindObjectOfType<PoolService>();
            dualBlades ??= FindObjectOfType<DualBladesController>();

            if (player == null)
            {
                var playerObject = GameObject.Find("Player_EchoShowcase");
                player = playerObject;
            }

            if (targetEnemy == null)
            {
                var enemyObject = GameObject.Find("Enemy_TestWalker");
                targetEnemy = enemyObject;
            }

            if (echoAnchor == null && player != null)
            {
                var anchor = player.transform.Find("EchoAnchor");
                echoAnchor = anchor != null ? anchor : player.transform;
            }

            if (cameraTransform == null && Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
        }
    }

    internal enum DevDebugKey
    {
        Digit1,
        Digit2,
        Digit3,
        Digit4,
        Digit5,
        Space
    }
}
