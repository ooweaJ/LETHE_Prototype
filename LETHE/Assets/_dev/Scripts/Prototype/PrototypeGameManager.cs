using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lethe.Dev
{
    public sealed class PrototypeGameManager : MonoBehaviour
    {
        private const string KalmuriMemory = "Memory_HungryBlades";
        private const string BloodMemory = "Memory_BloodReflection";
        private const string KalmuriEcho = "Echo_Kalmuri";
        private const string BloodEcho = "Echo_Blood";

        [Header("Scene")]
        [SerializeField] private PrototypePlayerController player;
        [SerializeField] private PrototypeWeaponController weapon;
        [SerializeField] private PrototypeEnemySpawner spawner;
        [SerializeField] private Camera mainCamera;

        [Header("Sprites")]
        [SerializeField] private Texture2D playerSheet;
        [SerializeField] private Texture2D enemySheet;

        [Header("Player")]
        [SerializeField] private float playerMaxHealth = 100f;
        [SerializeField] private int firstChoiceKills = 4;
        [SerializeField] private int choiceKillInterval = 5;
        [SerializeField] private int firstForgetKills = 12;
        [SerializeField] private int forgetKillInterval = 9;
        [SerializeField] private bool autoPrototypeLoop = true;

        private readonly Dictionary<string, int> activeMemories = new Dictionary<string, int>();
        private readonly Dictionary<string, int> echoes = new Dictionary<string, int>();
        private readonly HashSet<string> resonance = new HashSet<string>();

        private float playerHealth;
        private int kills;
        private int nextChoiceKills;
        private int nextForgetKills;
        private float runTime;
        private bool offeringChoice;
        private bool ultimateUnlocked;
        private string notice = "Prototype v0";
        private float noticeUntil;

        public float PlayerHealth => playerHealth;
        public float PlayerMaxHealth => playerMaxHealth;
        public int Kills => kills;

        private void Awake()
        {
            playerHealth = playerMaxHealth;
            nextChoiceKills = firstChoiceKills;
            nextForgetKills = firstForgetKills;
        }

        private void Start()
        {
            WireScene();
            ShowNotice("M1-M5 Prototype Ready");
        }

        private void Update()
        {
            runTime += Time.deltaTime;
            HandleDebugKeys();

            if (autoPrototypeLoop && !offeringChoice && kills >= nextChoiceKills)
            {
                offeringChoice = true;
                nextChoiceKills += choiceKillInterval;
            }

            DrawPersistentLoops();
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(8f, 8f, 320f, 154f), "LETHE Prototype v0");
            GUI.Label(new Rect(18f, 34f, 290f, 20f), $"HP {Mathf.CeilToInt(playerHealth)} / {Mathf.CeilToInt(playerMaxHealth)}   Kills {kills}   Time {runTime:0}s");
            GUI.Label(new Rect(18f, 56f, 290f, 20f), $"Active: {FormatState(activeMemories)}");
            GUI.Label(new Rect(18f, 78f, 290f, 20f), $"Echo: {FormatState(echoes)}");
            GUI.Label(new Rect(18f, 100f, 290f, 20f), $"Forget: {NextForgetCandidate()}   Storm: {(ultimateUnlocked ? "READY" : StormProgress())}");
            GUI.Label(new Rect(18f, 122f, 290f, 20f), "F1 Choice  F2 Forget  F3 Echo+5  F4 Resonance  F5 Both Memories");

            if (Time.time < noticeUntil)
            {
                GUI.Box(new Rect(Screen.width * 0.5f - 160f, 38f, 320f, 42f), notice);
            }

            if (offeringChoice)
            {
                GUI.Box(new Rect(Screen.width * 0.5f - 170f, Screen.height * 0.5f - 80f, 340f, 160f), "Choose Memory");
                if (GUI.Button(new Rect(Screen.width * 0.5f - 150f, Screen.height * 0.5f - 32f, 140f, 44f), "Hungry Blades"))
                {
                    ChooseMemory(KalmuriMemory);
                }
                if (GUI.Button(new Rect(Screen.width * 0.5f + 10f, Screen.height * 0.5f - 32f, 140f, 44f), "Blood Reflection"))
                {
                    ChooseMemory(BloodMemory);
                }
            }
        }

        public void RegisterEnemyKilled(PrototypeEnemy enemy)
        {
            kills += 1;
            StartCoroutine(RespawnEnemy(enemy));
            if (autoPrototypeLoop && activeMemories.Count > 0 && kills >= nextForgetKills)
            {
                nextForgetKills += forgetKillInterval;
                TriggerForget();
            }

            if (kills >= nextChoiceKills)
            {
                offeringChoice = true;
                nextChoiceKills += choiceKillInterval;
            }
        }

        public void DamagePlayer(float amount)
        {
            playerHealth = Mathf.Max(0f, playerHealth - amount);
            ShowNotice($"Hit -{amount:0}");
            if (playerHealth <= 0f)
            {
                ResetRun();
            }
        }

        public void HandleWeaponHit(PrototypeEnemy enemy, Vector3 hitPosition, Vector3 direction, float baseDamage, bool killed)
        {
            ApplyActiveMemories(enemy, hitPosition, direction, baseDamage);
            ApplyEchoes(enemy, hitPosition, direction, baseDamage);
        }

        public void SpawnLineVfx(string name, Vector3 from, Vector3 to, Color color, float lifetime, float width)
        {
            var lineObject = new GameObject($"VFX_{name}");
            lineObject.transform.SetParent(transform, true);
            var line = lineObject.AddComponent<LineRenderer>();
            line.positionCount = 2;
            line.SetPosition(0, from);
            line.SetPosition(1, to);
            line.startWidth = width;
            line.endWidth = width * 0.25f;
            line.startColor = color;
            line.endColor = new Color(color.r, color.g, color.b, 0f);
            line.sortingOrder = 40;
            line.material = new Material(Shader.Find("Sprites/Default"));
            Destroy(lineObject, lifetime);
        }

        private void WireScene()
        {
            player ??= FindObjectOfType<PrototypePlayerController>();
            weapon ??= FindObjectOfType<PrototypeWeaponController>();
            spawner ??= FindObjectOfType<PrototypeEnemySpawner>();
            mainCamera ??= Camera.main;

            var animator = player != null ? player.GetComponentInChildren<PrototypeSpriteSheetAnimator>() : null;
            animator?.SetSheet(playerSheet);
            spawner?.Initialize(this, player.transform, enemySheet);
            weapon?.Initialize(this, spawner);
        }

        private void ApplyActiveMemories(PrototypeEnemy enemy, Vector3 hitPosition, Vector3 direction, float baseDamage)
        {
            if (enemy == null || enemy.Health == null)
            {
                return;
            }

            if (activeMemories.TryGetValue(KalmuriMemory, out var kalmuriLevel))
            {
                SpawnLineVfx("HungryBlades", hitPosition + Vector3.up * 0.18f, hitPosition - Vector3.up * 0.18f, new Color(0.45f, 0.85f, 1f, 0.9f), 0.12f, 0.035f);
                enemy.Health.ApplyDamage(1.4f * kalmuriLevel, gameObject);
            }

            if (activeMemories.TryGetValue(BloodMemory, out var bloodLevel))
            {
                enemy.Health.ApplyDamage(0.9f * bloodLevel, gameObject);
                HealPlayer(0.35f * bloodLevel);
                SpawnLineVfx("BloodReflection", hitPosition, player.transform.position, new Color(1f, 0.05f, 0.12f, 0.85f), 0.16f, 0.028f);
            }
        }

        private void ApplyEchoes(PrototypeEnemy enemy, Vector3 hitPosition, Vector3 direction, float baseDamage)
        {
            if (enemy == null || enemy.Health == null)
            {
                return;
            }

            if (echoes.TryGetValue(KalmuriEcho, out var kalmuriEchoLevel))
            {
                var delayOffset = Quaternion.Euler(0f, 0f, 90f) * direction * 0.35f;
                SpawnLineVfx("KalmuriEcho", hitPosition - delayOffset, hitPosition + delayOffset, new Color(0.7f, 0.95f, 1f, 0.95f), 0.18f, 0.04f);
                enemy.Health.ApplyDamage(baseDamage * (0.12f + 0.05f * kalmuriEchoLevel), gameObject);
                if (kalmuriEchoLevel >= 5)
                {
                    DamageEnemiesInRadius(hitPosition, 1.05f, baseDamage * 0.38f, "KalmuriAwakened", new Color(0.55f, 0.95f, 1f, 0.9f));
                }
            }

            if (echoes.TryGetValue(BloodEcho, out var bloodEchoLevel))
            {
                SpawnLineVfx("BloodEcho", hitPosition + Vector3.left * 0.12f, player.transform.position, new Color(1f, 0.02f, 0.08f, 0.9f), 0.2f, 0.035f);
                enemy.Health.ApplyDamage(baseDamage * (0.08f + 0.04f * bloodEchoLevel), gameObject);
                HealPlayer(0.5f + bloodEchoLevel * 0.35f);
                if (bloodEchoLevel >= 5)
                {
                    DamageEnemiesInRadius(hitPosition, 0.95f, baseDamage * 0.28f, "BloodBloom", new Color(1f, 0.04f, 0.08f, 0.9f));
                    HealPlayer(1.2f);
                }
            }

            if (ultimateUnlocked)
            {
                var center = player != null ? player.transform.position : hitPosition;
                SpawnLineVfx("BloodBladeStorm", center + Vector3.left * 0.85f, center + Vector3.right * 0.85f, new Color(1f, 0.12f, 0.12f, 1f), 0.22f, 0.06f);
                SpawnLineVfx("BloodBladeStormThread", center + Vector3.down * 0.72f, center + Vector3.up * 0.72f, new Color(1f, 0.05f, 0.18f, 0.88f), 0.22f, 0.05f);
                DamageEnemiesInRadius(center, 1.75f, baseDamage * 0.48f, "BloodBladeStormPulse", new Color(1f, 0.1f, 0.16f, 1f));
                HealPlayer(1.4f);
            }
        }

        private void ChooseMemory(string memoryId)
        {
            offeringChoice = false;
            var baseLevel = 1;
            var echoId = MatchingEcho(memoryId);
            if (echoes.TryGetValue(echoId, out var echoLevel))
            {
                baseLevel = Mathf.Min(5, baseLevel + Mathf.FloorToInt(echoLevel / 2f));
                resonance.Add(memoryId);
                ShowNotice($"Resonance: {DisplayName(memoryId)} +{baseLevel}");
            }

            if (activeMemories.TryGetValue(memoryId, out var current))
            {
                activeMemories[memoryId] = Mathf.Min(5, current + 1);
            }
            else
            {
                activeMemories[memoryId] = baseLevel;
            }
        }

        private void TriggerForget()
        {
            var candidate = NextForgetCandidateId();
            if (string.IsNullOrEmpty(candidate))
            {
                ShowNotice("No memory to forget");
                return;
            }

            var level = activeMemories[candidate];
            activeMemories.Remove(candidate);
            var echoId = MatchingEcho(candidate);
            var before = echoes.TryGetValue(echoId, out var echoLevel) ? echoLevel : 0;
            var next = Mathf.Min(5, before + level);
            echoes[echoId] = next;
            var overflow = Mathf.Max(0, before + level - 5);
            ShowNotice($"Forgot {DisplayName(candidate)} -> {DisplayName(echoId)} +{next}" + (overflow > 0 ? $" Overload {overflow}" : ""));
            CheckUltimate();
        }

        private void ForceEchoFive()
        {
            echoes[KalmuriEcho] = 5;
            echoes[BloodEcho] = 5;
            CheckUltimate();
            ShowNotice("Echoes awakened: Kalmuri + Blood");
        }

        private void ForceResonance()
        {
            ChooseMemory(KalmuriMemory);
            ChooseMemory(BloodMemory);
            ShowNotice("Resonance forced");
        }

        private void ForceBothMemories()
        {
            activeMemories[KalmuriMemory] = Mathf.Min(5, GetActiveLevel(KalmuriMemory) + 1);
            activeMemories[BloodMemory] = Mathf.Min(5, GetActiveLevel(BloodMemory) + 1);
            ShowNotice("Both memories reinforced");
        }

        private void CheckUltimate()
        {
            ultimateUnlocked = echoes.TryGetValue(KalmuriEcho, out var k) && k >= 5 &&
                               echoes.TryGetValue(BloodEcho, out var b) && b >= 5;
            if (ultimateUnlocked)
            {
                ShowNotice("Blood Blade Storm READY");
            }
        }

        private void HealPlayer(float amount)
        {
            playerHealth = Mathf.Min(playerMaxHealth, playerHealth + amount);
        }

        private int GetActiveLevel(string id)
        {
            return activeMemories.TryGetValue(id, out var level) ? level : 0;
        }

        private void DamageEnemiesInRadius(Vector3 center, float radius, float damage, string effectName, Color color)
        {
            if (spawner == null)
            {
                return;
            }

            var enemies = spawner.Enemies;
            for (var index = 0; index < enemies.Count; index += 1)
            {
                var enemy = enemies[index];
                if (enemy == null || enemy.Health == null || enemy.Health.IsDead)
                {
                    continue;
                }

                if ((enemy.transform.position - center).sqrMagnitude > radius * radius)
                {
                    continue;
                }

                enemy.Health.ApplyDamage(damage, gameObject);
                SpawnLineVfx(effectName, center, enemy.transform.position, color, 0.16f, 0.032f);
            }
        }

        private void DrawPersistentLoops()
        {
            if (player == null || Time.frameCount % 8 != 0)
            {
                return;
            }

            var center = player.transform.position;
            if (activeMemories.ContainsKey(KalmuriMemory))
            {
                SpawnLineVfx("ActiveHungryBladesOrbit", center + Vector3.left * 0.62f, center + Vector3.right * 0.62f, new Color(0.35f, 0.75f, 1f, 0.45f), 0.12f, 0.018f);
            }

            if (echoes.TryGetValue(KalmuriEcho, out var kalmuri) && kalmuri >= 5)
            {
                SpawnLineVfx("AwakenedKalmuriRing", center + Vector3.down * 0.82f, center + Vector3.up * 0.82f, new Color(0.62f, 1f, 1f, 0.5f), 0.14f, 0.026f);
            }

            if (ultimateUnlocked)
            {
                SpawnLineVfx("StormGoalLoop", center + Vector3.left * 1.05f, center + Vector3.right * 1.05f, new Color(1f, 0.04f, 0.14f, 0.55f), 0.14f, 0.03f);
            }
        }

        private IEnumerator RespawnEnemy(PrototypeEnemy enemy)
        {
            yield return new WaitForSeconds(0.45f);
            spawner.Respawn(enemy);
        }

        private void ResetRun()
        {
            playerHealth = playerMaxHealth;
            kills = 0;
            activeMemories.Clear();
            echoes.Clear();
            resonance.Clear();
            ultimateUnlocked = false;
            ShowNotice("Death -> prototype reset");
        }

        private void HandleDebugKeys()
        {
#if ENABLE_INPUT_SYSTEM
            var keyboard = UnityEngine.InputSystem.Keyboard.current;
            if (keyboard != null)
            {
                if (keyboard.f1Key.wasPressedThisFrame) offeringChoice = true;
                if (keyboard.f2Key.wasPressedThisFrame) TriggerForget();
                if (keyboard.f3Key.wasPressedThisFrame) ForceEchoFive();
                if (keyboard.f4Key.wasPressedThisFrame) ForceResonance();
                if (keyboard.f5Key.wasPressedThisFrame) ForceBothMemories();
                return;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.F1)) offeringChoice = true;
            if (Input.GetKeyDown(KeyCode.F2)) TriggerForget();
            if (Input.GetKeyDown(KeyCode.F3)) ForceEchoFive();
            if (Input.GetKeyDown(KeyCode.F4)) ForceResonance();
            if (Input.GetKeyDown(KeyCode.F5)) ForceBothMemories();
#endif
        }

        private string NextForgetCandidate()
        {
            var id = NextForgetCandidateId();
            return string.IsNullOrEmpty(id) ? "-" : $"{DisplayName(id)} +{activeMemories[id]}";
        }

        private string NextForgetCandidateId()
        {
            string best = null;
            var bestLevel = -1;
            foreach (var pair in activeMemories)
            {
                if (pair.Value > bestLevel)
                {
                    best = pair.Key;
                    bestLevel = pair.Value;
                }
            }

            return best;
        }

        private string StormProgress()
        {
            echoes.TryGetValue(KalmuriEcho, out var k);
            echoes.TryGetValue(BloodEcho, out var b);
            return $"K {k}/5 B {b}/5";
        }

        private static string MatchingEcho(string memoryId)
        {
            return memoryId == BloodMemory ? BloodEcho : KalmuriEcho;
        }

        private static string DisplayName(string id)
        {
            return id switch
            {
                KalmuriMemory => "Hungry Blades",
                BloodMemory => "Blood Reflection",
                KalmuriEcho => "Kalmuri Echo",
                BloodEcho => "Blood Echo",
                _ => id
            };
        }

        private static string FormatState(Dictionary<string, int> state)
        {
            if (state.Count == 0)
            {
                return "-";
            }

            var parts = new List<string>();
            foreach (var pair in state)
            {
                parts.Add($"{DisplayName(pair.Key)} +{pair.Value}");
            }

            return string.Join(", ", parts);
        }

        private void ShowNotice(string message)
        {
            notice = message;
            noticeUntil = Time.time + 2f;
        }
    }
}
