using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lethe.Dev
{
    public sealed class PrototypeGameManager : MonoBehaviour
    {
        private const string WeaponDualBlades = "Weapon_DualBlades";
        private const string WeaponGreatsword = "Weapon_Greatsword";
        private const string SynergyBloodBladeStorm = "Synergy_BloodBladeStorm";
        private const string SynergyExecutionBrand = "Synergy_ExecutionBrand";
        private const string SynergyFrozenHunt = "Synergy_FrozenHunt";
        private const string SynergyBastionWave = "Synergy_BastionWave";

        private static readonly List<PrototypeMemorySpec> MemorySpecs = new List<PrototypeMemorySpec>
        {
            new PrototypeMemorySpec("Memory_HungryBlades", "굶주린 칼무리", "Echo_Kalmuri", PrototypeEffectKind.Kalmuri, new Color(0.58f, 0.96f, 1f, 0.9f), "주변 독립 칼날 고리", "무기 공격 칼자국"),
            new PrototypeMemorySpec("Memory_BloodReflection", "피의 반사", "Echo_Blood", PrototypeEffectKind.Blood, new Color(1f, 0.12f, 0.18f, 0.9f), "붉은 표식과 회복", "혈반과 회복 실"),
            new PrototypeMemorySpec("Memory_ExecutionFlash", "처형자의 섬광", "Echo_Execution", PrototypeEffectKind.Execution, new Color(1f, 0.96f, 0.82f, 0.92f), "약한 적 처형 보조", "하얀 균열 처형"),
            new PrototypeMemorySpec("Memory_HunterOath", "추적자의 맹세", "Echo_Homing", PrototypeEffectKind.Homing, new Color(0.72f, 0.84f, 1f, 0.9f), "원거리/약한 적 추적", "유도 잔탄"),
            new PrototypeMemorySpec("Memory_ShatterWave", "파쇄의 파문", "Echo_Shockwave", PrototypeEffectKind.Shockwave, new Color(0.88f, 0.9f, 1f, 0.88f), "포위 해제 충격파", "타격 위치 파문"),
            new PrototypeMemorySpec("Memory_StoppedSecond", "멈춘 초침", "Echo_TimeStop", PrototypeEffectKind.TimeStop, new Color(0.62f, 0.78f, 1f, 0.86f), "둔화와 시간 제어", "시간 균열"),
            new PrototypeMemorySpec("Memory_AshenShield", "잿빛 보호막", "Echo_AshenGuard", PrototypeEffectKind.AshenGuard, new Color(0.72f, 0.76f, 0.82f, 0.86f), "방어 후 반격", "잿빛 반응"),
            new PrototypeMemorySpec("Memory_OblivionBrand", "망각의 낙인", "Echo_Brand", PrototypeEffectKind.Brand, new Color(0.72f, 0.55f, 1f, 0.86f), "취약 표식", "낙인 증폭")
        };

        private static readonly List<PrototypeEchoSpec> EchoSpecs = new List<PrototypeEchoSpec>
        {
            new PrototypeEchoSpec("Echo_Kalmuri", "칼무리 잔향", "Memory_HungryBlades", PrototypeEffectKind.Kalmuri, new Color(0.62f, 1f, 1f, 0.9f)),
            new PrototypeEchoSpec("Echo_Blood", "혈반 잔향", "Memory_BloodReflection", PrototypeEffectKind.Blood, new Color(1f, 0.1f, 0.16f, 0.92f)),
            new PrototypeEchoSpec("Echo_Execution", "처형 잔향", "Memory_ExecutionFlash", PrototypeEffectKind.Execution, new Color(1f, 0.96f, 0.78f, 0.9f)),
            new PrototypeEchoSpec("Echo_Homing", "추적 잔향", "Memory_HunterOath", PrototypeEffectKind.Homing, new Color(0.66f, 0.78f, 1f, 0.9f)),
            new PrototypeEchoSpec("Echo_Shockwave", "파문 잔향", "Memory_ShatterWave", PrototypeEffectKind.Shockwave, new Color(0.86f, 0.9f, 1f, 0.9f)),
            new PrototypeEchoSpec("Echo_TimeStop", "정지 잔향", "Memory_StoppedSecond", PrototypeEffectKind.TimeStop, new Color(0.58f, 0.74f, 1f, 0.9f)),
            new PrototypeEchoSpec("Echo_AshenGuard", "잿빛 잔향", "Memory_AshenShield", PrototypeEffectKind.AshenGuard, new Color(0.68f, 0.7f, 0.76f, 0.9f)),
            new PrototypeEchoSpec("Echo_Brand", "낙인 잔향", "Memory_OblivionBrand", PrototypeEffectKind.Brand, new Color(0.74f, 0.48f, 1f, 0.9f))
        };

        private static readonly List<PrototypeSynergySpec> SynergySpecs = new List<PrototypeSynergySpec>
        {
            new PrototypeSynergySpec(SynergyBloodBladeStorm, "피의 칼폭풍", new[] { "Echo_Kalmuri", "Echo_Blood" }, new Color(1f, 0.12f, 0.18f, 0.95f), "칼날이 혈반을 묻히고 회복 실을 돌려준다"),
            new PrototypeSynergySpec(SynergyExecutionBrand, "처형 각인", new[] { "Echo_Execution", "Echo_Brand" }, new Color(1f, 0.86f, 0.62f, 0.95f), "낙인 적 처형이 하얀 파편으로 연쇄된다"),
            new PrototypeSynergySpec(SynergyFrozenHunt, "정지 추적", new[] { "Echo_Homing", "Echo_TimeStop" }, new Color(0.55f, 0.75f, 1f, 0.95f), "둔화 적을 추적 잔탄이 우선 공격한다"),
            new PrototypeSynergySpec(SynergyBastionWave, "성채 파문", new[] { "Echo_Shockwave", "Echo_AshenGuard" }, new Color(0.76f, 0.78f, 0.9f, 0.95f), "방어 반응이 큰 파문과 안전지대를 만든다")
        };

        [Header("Scene")]
        [SerializeField] private PrototypePlayerController player;
        [SerializeField] private PrototypeWeaponController weapon;
        [SerializeField] private PrototypeEnemySpawner spawner;
        [SerializeField] private Camera mainCamera;

        [Header("Sprites")]
        [SerializeField] private Texture2D playerSheet;
        [SerializeField] private Texture2D enemySheet;
        [SerializeField] private Sprite hungryBladesActiveSprite;
        [SerializeField] private Sprite kalmuriSlashSprite;
        [SerializeField] private Sprite bloodReflectionSprite;
        [SerializeField] private Sprite bloodBloomSprite;
        [SerializeField] private Sprite bloodBladeStormSprite;
        [SerializeField] private Font koreanFont;

        [Header("Player")]
        [SerializeField] private float playerMaxHealth = 100f;
        [SerializeField] private int firstChoiceKills = 4;
        [SerializeField] private int choiceKillInterval = 5;
        [SerializeField] private int firstForgetKills = 26;
        [SerializeField] private int forgetKillInterval = 14;
        [SerializeField] private int minActiveMemoryKillsBeforeForget = 14;
        [SerializeField] private float minActiveMemorySecondsBeforeForget = 18f;
        [SerializeField] private bool autoPrototypeLoop = true;

        [Header("Memory Feel")]
        [SerializeField] private float activeKalmuriRadius = 1.75f;
        [SerializeField] private float activeKalmuriInterval = 0.72f;
        [SerializeField] private float activeBloodRadius = 2.05f;
        [SerializeField] private float activeBloodInterval = 1.05f;

        private readonly MemoryInventory activeMemories = new MemoryInventory();
        private readonly EchoInventory echoes = new EchoInventory();
        private readonly ForgetService forgetService = new ForgetService();
        private readonly ResonanceService resonanceService = new ResonanceService();
        private readonly UltimateEchoService ultimateService = new UltimateEchoService();
        private readonly RewardService rewardService = new RewardService();
        private readonly DebugStateInjector debugStateInjector = new DebugStateInjector();
        private readonly Dictionary<string, float> nextMemoryTickAt = new Dictionary<string, float>();
        private readonly HashSet<string> resonance = new HashSet<string>();

        private float playerHealth;
        private int kills;
        private int nextChoiceKills;
        private int nextForgetKills;
        private int earliestForgetKills;
        private float runTime;
        private float earliestForgetTime;
        private bool offeringChoice;
        private string notice = "Complete Prototype";
        private float noticeUntil;
        private GUIStyle panelStyle;
        private GUIStyle labelStyle;
        private GUIStyle noticeStyle;
        private GUIStyle buttonStyle;

        public float PlayerHealth => playerHealth;
        public float PlayerMaxHealth => playerMaxHealth;
        public int Kills => kills;
        public IReadOnlyDictionary<string, int> ActiveMemoryLevels => activeMemories.Levels;
        public IReadOnlyDictionary<string, int> EchoLevels => echoes.Levels;
        public IReadOnlyCollection<string> UnlockedSynergies => ultimateService.Unlocked;
        public string CurrentWeaponId => weapon != null ? weapon.CurrentWeaponId : WeaponDualBlades;

        private void Awake()
        {
            playerHealth = playerMaxHealth;
            nextChoiceKills = firstChoiceKills;
            nextForgetKills = firstForgetKills;
            earliestForgetKills = firstForgetKills;
            earliestForgetTime = 0f;
        }

        private void Start()
        {
            WireScene();
            ShowNotice("Complete Prototype: 2무기 8기억 8잔향 4궁극");
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

            TickActiveMemoryEffects();
            DrawPersistentLoops();
        }

        private void OnGUI()
        {
            EnsureGuiStyles();
            GUI.Box(new Rect(8f, 8f, 462f, 214f), "LETHE Complete Prototype", panelStyle);
            GUI.Label(new Rect(18f, 34f, 430f, 20f), $"무기 {CurrentWeaponName()}   체력 {Mathf.CeilToInt(playerHealth)} / {Mathf.CeilToInt(playerMaxHealth)}   처치 {kills}   시간 {runTime:0}s", labelStyle);
            GUI.Label(new Rect(18f, 58f, 430f, 34f), $"기억: {FormatState(activeMemories.Levels, true)}", labelStyle);
            GUI.Label(new Rect(18f, 92f, 430f, 34f), $"잔향: {FormatState(echoes.Levels, false)}", labelStyle);
            GUI.Label(new Rect(18f, 126f, 430f, 20f), $"다음 망각: {NextForgetCandidate()}   궁극: {FormatSynergies()}", labelStyle);
            GUI.Label(new Rect(18f, 150f, 430f, 20f), "F1 선택  F2 망각  F3 다음기억  F4 잔향+1  F5 잔향+5  F6 무기  F7 전체기억  F8 전체궁극", labelStyle);

            var y = 176f;
            if (GUI.Button(new Rect(18f, y, 62f, 28f), "무기", buttonStyle)) ToggleWeapon();
            if (GUI.Button(new Rect(86f, y, 72f, 28f), "기억+1", buttonStyle)) AddNextMemory();
            if (GUI.Button(new Rect(164f, y, 72f, 28f), "망각", buttonStyle)) TriggerForget();
            if (GUI.Button(new Rect(242f, y, 82f, 28f), "잔향+1", buttonStyle)) ForceAllEchoes(1);
            if (GUI.Button(new Rect(330f, y, 82f, 28f), "궁극", buttonStyle)) ForceAllUltimate();

            if (Time.time < noticeUntil)
            {
                GUI.Box(new Rect(Screen.width * 0.5f - 220f, 38f, 440f, 44f), notice, noticeStyle);
            }

            if (offeringChoice)
            {
                DrawMemoryChoiceOverlay();
            }
        }

        public void RegisterEnemyKilled(PrototypeEnemy enemy)
        {
            kills += 1;
            ApplyKillEchoes(enemy);
            StartCoroutine(RespawnEnemy(enemy));
            if (autoPrototypeLoop && activeMemories.Count > 0 && kills >= nextForgetKills && CanAutoForget())
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
            if (activeMemories.GetLevel("Memory_AshenShield") > 0 || echoes.GetLevel("Echo_AshenGuard") > 0)
            {
                var level = Mathf.Max(activeMemories.GetLevel("Memory_AshenShield"), echoes.GetLevel("Echo_AshenGuard"));
                HealPlayer(0.4f * level);
                var center = player != null ? player.transform.position : transform.position;
                DamageEnemiesInRadius(center, 1.1f + level * 0.08f, 1.4f + level * 0.55f, "AshenGuardRetaliate", FindEcho("Echo_AshenGuard").Color);
            }

            ShowNotice($"피격 -{amount:0}");
            if (playerHealth <= 0f)
            {
                ResetRun();
            }
        }

        public void HandleWeaponHit(PrototypeEnemy enemy, Vector3 hitPosition, Vector3 direction, float baseDamage, bool killed, string weaponId = WeaponDualBlades)
        {
            ApplyActiveMemoryOnHit(enemy, hitPosition, direction, baseDamage, weaponId);
            ApplyEchoes(enemy, hitPosition, direction, baseDamage, weaponId);
            ApplyUnlockedSynergies(enemy, hitPosition, direction, baseDamage, weaponId);
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

        public void SpawnSpriteVfx(string name, Sprite sprite, Vector3 position, float scale, float lifetime, Color color, float spin = 0f, int sortingOrder = 45)
        {
            if (sprite == null)
            {
                return;
            }

            var vfxObject = new GameObject($"VFX_{name}");
            vfxObject.transform.SetParent(transform, true);
            vfxObject.transform.position = position;
            vfxObject.transform.localScale = Vector3.one * scale;
            vfxObject.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

            var renderer = vfxObject.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.color = color;
            renderer.sortingOrder = sortingOrder;

            var vfx = vfxObject.AddComponent<PrototypeSpriteVfx>();
            vfx.Configure(lifetime, scale, scale * 1.35f, spin);
        }

        private void WireScene()
        {
            player ??= FindObjectOfType<PrototypePlayerController>();
            weapon ??= FindObjectOfType<PrototypeWeaponController>();
            spawner ??= FindObjectOfType<PrototypeEnemySpawner>();
            mainCamera ??= Camera.main;

            var animator = player != null ? player.GetComponentInChildren<PrototypeSpriteSheetAnimator>() : null;
            animator?.SetSheet(playerSheet);
            if (spawner != null && player != null)
            {
                spawner.Initialize(this, player.transform, enemySheet);
            }

            weapon?.Initialize(this, spawner);
        }

        private void ApplyActiveMemoryOnHit(PrototypeEnemy enemy, Vector3 hitPosition, Vector3 direction, float baseDamage, string weaponId)
        {
            if (enemy == null || enemy.Health == null)
            {
                return;
            }

            var memorySnapshot = new List<KeyValuePair<string, int>>(activeMemories.Levels);
            foreach (var pair in memorySnapshot)
            {
                var spec = FindMemory(pair.Key);
                var level = pair.Value;
                var heavy = weaponId == WeaponGreatsword;
                switch (spec.Kind)
                {
                    case PrototypeEffectKind.Kalmuri:
                        SpawnSpriteVfx("ActiveKalmuriCut", hungryBladesActiveSprite, enemy.transform.position, 0.3f + level * 0.03f, 0.25f, spec.Color, 220f, 38);
                        enemy.Health.ApplyDamage((heavy ? 1.05f : 0.7f) * level, gameObject);
                        break;
                    case PrototypeEffectKind.Blood:
                        enemy.Health.ApplyDamage((heavy ? 0.75f : 0.5f) * level, gameObject);
                        HealPlayer((heavy ? 0.36f : 0.25f) * level);
                        SpawnLineVfx("ActiveBloodThread", hitPosition, player.transform.position, spec.Color, 0.16f, 0.022f);
                        break;
                    case PrototypeEffectKind.Execution:
                        if (enemy.Health.CurrentHealth <= enemy.Health.MaxHealth * (0.32f + level * 0.02f) || heavy)
                        {
                            enemy.Health.ApplyDamage(baseDamage * (0.18f + level * 0.08f), gameObject);
                            SpawnLineVfx("ActiveExecutionFlash", hitPosition + Vector3.left * 0.38f, hitPosition + Vector3.right * 0.38f, spec.Color, 0.12f, 0.04f);
                        }
                        break;
                    case PrototypeEffectKind.Homing:
                        HitNearestOther(enemy, hitPosition, 2.8f + level * 0.25f, 1.2f + level * 0.45f, "ActiveHunterOath", spec.Color, kalmuriSlashSprite);
                        break;
                    case PrototypeEffectKind.Shockwave:
                        DamageEnemiesInRadius(hitPosition, 0.72f + level * 0.12f + (heavy ? 0.35f : 0f), 0.9f + level * 0.4f, "ActiveShatterWave", spec.Color);
                        break;
                    case PrototypeEffectKind.TimeStop:
                        SpawnLineVfx("ActiveStoppedSecond", hitPosition + Vector3.down * 0.32f, hitPosition + Vector3.up * 0.32f, spec.Color, 0.22f, 0.035f);
                        enemy.ApplyKnockback(-direction, 0.35f + level * 0.08f);
                        break;
                    case PrototypeEffectKind.AshenGuard:
                        if (playerHealth < playerMaxHealth * 0.65f)
                        {
                            HealPlayer(0.22f * level);
                            SpawnSpriteVfx("ActiveAshenGuard", bloodBloomSprite, player.transform.position, 0.22f + level * 0.02f, 0.26f, spec.Color, 40f, 17);
                        }
                        break;
                    case PrototypeEffectKind.Brand:
                        enemy.Health.ApplyDamage(baseDamage * (0.06f + level * 0.035f), gameObject);
                        SpawnSpriteVfx("ActiveOblivionBrand", bloodReflectionSprite, hitPosition, 0.24f + level * 0.025f, 0.32f, spec.Color, 80f, 39);
                        break;
                }
            }
        }

        private void ApplyEchoes(PrototypeEnemy enemy, Vector3 hitPosition, Vector3 direction, float baseDamage, string weaponId)
        {
            if (enemy == null || enemy.Health == null)
            {
                return;
            }

            var echoSnapshot = new List<KeyValuePair<string, int>>(echoes.Levels);
            foreach (var pair in echoSnapshot)
            {
                var level = pair.Value;
                if (level <= 0)
                {
                    continue;
                }

                var spec = FindEcho(pair.Key);
                var heavy = weaponId == WeaponGreatsword;
                var levelScale = 0.08f + level * 0.035f;
                var radiusBonus = level >= 3 ? 0.38f : 0f;
                switch (spec.Kind)
                {
                    case PrototypeEffectKind.Kalmuri:
                        SpawnCross(hitPosition, direction, "EchoKalmuriSlash", spec.Color, level >= 5 ? 0.58f : 0.38f, level >= 5 ? 0.05f : 0.032f);
                        SpawnSpriteVfx("EchoKalmuriSprite", kalmuriSlashSprite, hitPosition, 0.28f + level * 0.035f, 0.24f, spec.Color, 140f);
                        enemy.Health.ApplyDamage(baseDamage * levelScale, gameObject);
                        if (level >= 3) DamageEnemiesInRadius(hitPosition, 0.72f + radiusBonus, baseDamage * 0.1f * level, "EchoKalmuriLinger", spec.Color);
                        break;
                    case PrototypeEffectKind.Blood:
                        SpawnLineVfx("EchoBloodThread", hitPosition, player.transform.position, spec.Color, 0.2f, 0.028f);
                        SpawnSpriteVfx("EchoBloodBloom", bloodBloomSprite, hitPosition, 0.24f + level * 0.035f, 0.3f, spec.Color, 90f);
                        enemy.Health.ApplyDamage(baseDamage * (0.06f + level * 0.032f), gameObject);
                        HealPlayer(0.34f + level * 0.25f);
                        if (level >= 5) DamageEnemiesInRadius(hitPosition, 0.95f, baseDamage * 0.22f, "EchoBloodFlower", spec.Color);
                        break;
                    case PrototypeEffectKind.Execution:
                        if (enemy.Health.CurrentHealth <= enemy.Health.MaxHealth * 0.36f || level >= 5)
                        {
                            enemy.Health.ApplyDamage(baseDamage * (0.2f + level * 0.08f), gameObject);
                            DamageEnemiesInRadius(hitPosition, 0.55f + level * 0.08f, baseDamage * 0.08f * level, "EchoExecutionCrack", spec.Color);
                        }
                        break;
                    case PrototypeEffectKind.Homing:
                        HitNearestOther(enemy, hitPosition, 3.2f + level * 0.25f, baseDamage * (0.12f + level * 0.04f), "EchoHomingShot", spec.Color, kalmuriSlashSprite);
                        if (level >= 5) HitNearestOther(enemy, hitPosition + Vector3.up * 0.18f, 3.6f, baseDamage * 0.18f, "EchoHomingSplit", spec.Color, kalmuriSlashSprite);
                        break;
                    case PrototypeEffectKind.Shockwave:
                        DamageEnemiesInRadius(hitPosition, 0.86f + level * 0.1f + (heavy ? 0.3f : 0f), baseDamage * (0.08f + level * 0.035f), "EchoShockwave", spec.Color);
                        break;
                    case PrototypeEffectKind.TimeStop:
                        SpawnCross(hitPosition, direction, "EchoTimeFracture", spec.Color, 0.34f + level * 0.04f, 0.034f);
                        DamageEnemiesInRadius(hitPosition, 0.55f + level * 0.08f, baseDamage * 0.06f * level, "EchoTimeSlow", spec.Color);
                        break;
                    case PrototypeEffectKind.AshenGuard:
                        if (playerHealth < playerMaxHealth || level >= 5)
                        {
                            HealPlayer(0.18f + level * 0.18f);
                            SpawnSpriteVfx("EchoAshenGuard", bloodBloomSprite, player.transform.position + Vector3.down * 0.08f, 0.22f + level * 0.025f, 0.3f, spec.Color, 50f, 18);
                        }
                        break;
                    case PrototypeEffectKind.Brand:
                        SpawnSpriteVfx("EchoOblivionBrand", bloodReflectionSprite, hitPosition, 0.26f + level * 0.03f, 0.34f, spec.Color, 120f);
                        enemy.Health.ApplyDamage(baseDamage * (0.08f + level * 0.04f), gameObject);
                        if (level >= 3) DamageEnemiesInRadius(hitPosition, 0.58f + level * 0.06f, baseDamage * 0.08f * level, "EchoBrandSpread", spec.Color);
                        break;
                }
            }
        }

        private void ApplyUnlockedSynergies(PrototypeEnemy enemy, Vector3 hitPosition, Vector3 direction, float baseDamage, string weaponId)
        {
            if (ultimateService.Unlocked.Count == 0)
            {
                return;
            }

            foreach (var synergyId in ultimateService.Unlocked)
            {
                var spec = FindSynergy(synergyId);
                var center = player != null ? player.transform.position : hitPosition;
                switch (synergyId)
                {
                    case SynergyBloodBladeStorm:
                        SpawnSpriteVfx("SynergyBloodBladeStorm", bloodBladeStormSprite, center, 0.58f, 0.42f, spec.Color, 260f, 43);
                        DamageEnemiesInRadius(center, 1.65f, baseDamage * 0.34f, "SynergyBloodBladeStormPulse", spec.Color);
                        HealPlayer(0.9f);
                        break;
                    case SynergyExecutionBrand:
                        SpawnCross(hitPosition, direction, "SynergyExecutionBrand", spec.Color, 0.74f, 0.07f);
                        DamageEnemiesInRadius(hitPosition, 1.15f, baseDamage * 0.36f, "SynergyExecutionBrandBurst", spec.Color);
                        break;
                    case SynergyFrozenHunt:
                        HitNearestOther(enemy, hitPosition, 4.4f, baseDamage * 0.42f, "SynergyFrozenHuntShot", spec.Color, kalmuriSlashSprite);
                        SpawnLineVfx("SynergyFrozenHuntClock", center + Vector3.left * 0.58f, center + Vector3.right * 0.58f, spec.Color, 0.18f, 0.042f);
                        break;
                    case SynergyBastionWave:
                        DamageEnemiesInRadius(center, 1.95f, baseDamage * 0.3f, "SynergyBastionWave", spec.Color);
                        HealPlayer(0.65f);
                        SpawnSpriteVfx("SynergyBastionSafeZone", bloodBloomSprite, center, 0.62f, 0.35f, spec.Color, 80f, 16);
                        break;
                }
            }
        }

        private void ApplyKillEchoes(PrototypeEnemy enemy)
        {
            if (enemy == null)
            {
                return;
            }

            if (echoes.GetLevel("Echo_Execution") >= 3 || echoes.GetLevel("Echo_Brand") >= 3)
            {
                var level = Mathf.Max(echoes.GetLevel("Echo_Execution"), echoes.GetLevel("Echo_Brand"));
                var color = level >= 5 ? FindSynergy(SynergyExecutionBrand).Color : FindEcho("Echo_Execution").Color;
                DamageEnemiesInRadius(enemy.transform.position, 0.75f + level * 0.08f, 1.8f + level * 0.65f, "KillExecutionBrand", color);
            }
        }

        private void TickActiveMemoryEffects()
        {
            if (player == null || spawner == null)
            {
                return;
            }

            var memorySnapshot = new List<KeyValuePair<string, int>>(activeMemories.Levels);
            foreach (var pair in memorySnapshot)
            {
                var memoryId = pair.Key;
                var level = pair.Value;
                var spec = FindMemory(memoryId);
                var interval = ActiveInterval(spec.Kind, level);
                if (nextMemoryTickAt.TryGetValue(memoryId, out var nextAt) && Time.time < nextAt)
                {
                    continue;
                }

                nextMemoryTickAt[memoryId] = Time.time + interval;
                TickOneActiveMemory(spec, level);
            }
        }

        private void TickOneActiveMemory(PrototypeMemorySpec spec, int level)
        {
            var center = player.transform.position;
            switch (spec.Kind)
            {
                case PrototypeEffectKind.Kalmuri:
                    SpawnKalmuriOrbit(center, level, spec.Color);
                    DamageClosestEnemiesInRadius(center, activeKalmuriRadius + level * 0.08f, Mathf.Clamp(2 + level / 2, 2, 4), 2.4f + level, "ActiveKalmuriOrbitCut", spec.Color, 0.3f, hungryBladesActiveSprite);
                    break;
                case PrototypeEffectKind.Blood:
                    var hitCount = DamageClosestEnemiesInRadius(center, activeBloodRadius + level * 0.06f, 1 + Mathf.CeilToInt(level * 0.4f), 1.4f + level * 0.7f, "ActiveBloodPulse", spec.Color, 0.12f, bloodReflectionSprite);
                    if (hitCount > 0)
                    {
                        HealPlayer(0.45f + level * 0.28f);
                        SpawnSpriteVfx("ActiveBloodHeal", bloodBloomSprite, center + Vector3.up * 0.12f, 0.24f + level * 0.025f, 0.3f, spec.Color, 90f, 39);
                    }
                    break;
                case PrototypeEffectKind.Execution:
                    SpawnProceduralMemoryVfx(spec.Kind, center, spec.Color, level, "ActiveExecutionAura");
                    ExecuteWeakestEnemy(center, 2.4f + level * 0.15f, 1.8f + level * 0.8f, spec.Color);
                    break;
                case PrototypeEffectKind.Homing:
                    SpawnProceduralMemoryVfx(spec.Kind, center, spec.Color, level, "ActiveHunterOathAura");
                    HitNearestEnemy(center, 3.4f + level * 0.2f, 1.6f + level * 0.55f, "ActiveHunterOathPeriodic", spec.Color, kalmuriSlashSprite);
                    break;
                case PrototypeEffectKind.Shockwave:
                    SpawnProceduralMemoryVfx(spec.Kind, center, spec.Color, level, "ActiveShatterWaveAura");
                    DamageEnemiesInRadius(center, 0.95f + level * 0.1f, 1.1f + level * 0.35f, "ActiveShatterWavePeriodic", spec.Color);
                    break;
                case PrototypeEffectKind.TimeStop:
                    SpawnProceduralMemoryVfx(spec.Kind, center, spec.Color, level, "ActiveStoppedSecondAura");
                    DamageEnemiesInRadius(center, 1.2f + level * 0.08f, 0.7f + level * 0.28f, "ActiveStoppedSecondField", spec.Color);
                    break;
                case PrototypeEffectKind.AshenGuard:
                    SpawnProceduralMemoryVfx(spec.Kind, center, spec.Color, level, "ActiveAshenShieldAura");
                    if (playerHealth < playerMaxHealth)
                    {
                        HealPlayer(0.28f + level * 0.12f);
                        SpawnSpriteVfx("ActiveAshenShieldLoop", bloodBloomSprite, center, 0.2f + level * 0.02f, 0.24f, spec.Color, 35f, 14);
                    }
                    break;
                case PrototypeEffectKind.Brand:
                    SpawnProceduralMemoryVfx(spec.Kind, center, spec.Color, level, "ActiveOblivionBrandAura");
                    HitNearestEnemy(center, 2.6f + level * 0.12f, 1.2f + level * 0.42f, "ActiveOblivionBrandPulse", spec.Color, bloodReflectionSprite);
                    break;
            }
        }

        private float ActiveInterval(PrototypeEffectKind kind, int level)
        {
            return kind switch
            {
                PrototypeEffectKind.Kalmuri => Mathf.Max(0.34f, activeKalmuriInterval - level * 0.045f),
                PrototypeEffectKind.Blood => Mathf.Max(0.55f, activeBloodInterval - level * 0.035f),
                PrototypeEffectKind.Execution => 1.15f,
                PrototypeEffectKind.Homing => 0.82f,
                PrototypeEffectKind.Shockwave => 1.05f,
                PrototypeEffectKind.TimeStop => 1.25f,
                PrototypeEffectKind.AshenGuard => 1.3f,
                PrototypeEffectKind.Brand => 0.95f,
                _ => 1f
            };
        }

        private void ChooseMemory(string memoryId)
        {
            offeringChoice = false;
            var spec = FindMemory(memoryId);
            var echoLevel = echoes.GetLevel(spec.EchoId);
            var baseLevel = echoLevel > 0 ? resonanceService.GetReacquiredLevel(1, echoLevel) : 1;
            if (echoLevel > 0)
            {
                resonance.Add(memoryId);
                ShowNotice($"공명: {spec.DisplayName} +{baseLevel}");
            }

            activeMemories.SetLevel(memoryId, Mathf.Max(baseLevel, activeMemories.GetLevel(memoryId) + 1));
            ArmMemoryHuntingWindow();
        }

        private void TriggerForget()
        {
            if (!forgetService.ForgetHighest(activeMemories, echoes, MatchingEcho, out var memoryId, out var echoId, out var lostLevel, out var echoLevel, out var overflow))
            {
                ShowNotice("망각할 기억이 없음");
                return;
            }

            RefreshUltimates();
            ShowNotice($"{DisplayName(memoryId)} 망각 -> {DisplayName(echoId)} +{echoLevel}" + (overflow > 0 ? $" 과부하 {overflow}" : ""));
        }

        private void AddNextMemory()
        {
            ChooseMemory(rewardService.NextMemory(MemorySpecs));
        }

        private void ForceAllEchoes(int level)
        {
            debugStateInjector.SetAllEchoes(echoes, EchoSpecs, level);
            RefreshUltimates();
            SpawnAllEchoPreview();
            ShowNotice($"8잔향 +{level}");
        }

        private void ForceAllUltimate()
        {
            debugStateInjector.SetAllEchoes(echoes, EchoSpecs, 5);
            RefreshUltimates();
            ShowNotice("4궁극 조건 충족");
        }

        private void ForceAllMemories()
        {
            debugStateInjector.SetAllMemories(activeMemories, MemorySpecs, 1);
            ArmMemoryHuntingWindow();
            SpawnAllMemoryPreview();
            ShowNotice("8기억 활성화");
        }

        private void ToggleWeapon()
        {
            weapon?.ToggleWeapon();
            ShowNotice($"무기 전환: {CurrentWeaponName()}");
        }

        private void RefreshUltimates()
        {
            if (ultimateService.Refresh(echoes, SynergySpecs))
            {
                ShowNotice($"궁극 준비: {FormatSynergies()}");
            }
        }

        private void HealPlayer(float amount)
        {
            playerHealth = Mathf.Min(playerMaxHealth, playerHealth + Mathf.Max(0f, amount));
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
                enemy.ApplyKnockback((enemy.transform.position - center).normalized, 1.25f + radius * 0.25f);
                SpawnLineVfx(effectName, center, enemy.transform.position, color, 0.16f, 0.032f);
            }
        }

        private void SpawnAllMemoryPreview()
        {
            if (player == null)
            {
                return;
            }

            var center = player.transform.position;
            for (var index = 0; index < MemorySpecs.Count; index += 1)
            {
                var spec = MemorySpecs[index];
                var offset = Quaternion.Euler(0f, 0f, index * 45f) * Vector3.right * 1.0f;
                SpawnProceduralMemoryVfx(spec.Kind, center + offset, spec.Color, 3, "MemoryPreview" + spec.Kind);
            }
        }

        private void SpawnAllEchoPreview()
        {
            if (player == null)
            {
                return;
            }

            var center = player.transform.position;
            for (var index = 0; index < EchoSpecs.Count; index += 1)
            {
                var spec = EchoSpecs[index];
                var offset = Quaternion.Euler(0f, 0f, index * 45f + 22.5f) * Vector3.right * 1.35f;
                SpawnProceduralMemoryVfx(spec.Kind, center + offset, spec.Color, 5, "EchoPreview" + spec.Kind);
            }
        }

        private void SpawnProceduralMemoryVfx(PrototypeEffectKind kind, Vector3 center, Color color, int level, string name)
        {
            var scale = 0.42f + level * 0.045f;
            switch (kind)
            {
                case PrototypeEffectKind.Execution:
                    SpawnCross(center, Vector3.right, name + "CrackA", color, scale, 0.052f);
                    SpawnCross(center, Vector3.up, name + "CrackB", color, scale * 0.72f, 0.036f);
                    break;
                case PrototypeEffectKind.Homing:
                    SpawnLineVfx(name + "ArrowShaft", center + Vector3.left * scale, center + Vector3.right * scale, color, 0.22f, 0.038f);
                    SpawnLineVfx(name + "ArrowHeadA", center + Vector3.right * scale, center + new Vector3(scale * 0.55f, scale * 0.26f, 0f), color, 0.22f, 0.03f);
                    SpawnLineVfx(name + "ArrowHeadB", center + Vector3.right * scale, center + new Vector3(scale * 0.55f, -scale * 0.26f, 0f), color, 0.22f, 0.03f);
                    break;
                case PrototypeEffectKind.Shockwave:
                    SpawnRingVfx(name + "RingOuter", center, scale * 1.25f, color, 0.24f, 0.035f, 20);
                    SpawnRingVfx(name + "RingInner", center, scale * 0.72f, new Color(color.r, color.g, color.b, color.a * 0.65f), 0.22f, 0.024f, 16);
                    break;
                case PrototypeEffectKind.TimeStop:
                    SpawnRingVfx(name + "Clock", center, scale, color, 0.28f, 0.026f, 24);
                    SpawnLineVfx(name + "HandA", center, center + Vector3.up * scale, color, 0.28f, 0.032f);
                    SpawnLineVfx(name + "HandB", center, center + Vector3.right * (scale * 0.68f), color, 0.28f, 0.026f);
                    break;
                case PrototypeEffectKind.AshenGuard:
                    SpawnRingVfx(name + "Shield", center, scale * 1.08f, color, 0.32f, 0.044f, 18);
                    SpawnLineVfx(name + "ShieldBar", center + Vector3.left * scale * 0.5f, center + Vector3.right * scale * 0.5f, color, 0.28f, 0.035f);
                    break;
                case PrototypeEffectKind.Brand:
                    SpawnDiamondVfx(name + "Diamond", center, scale, color, 0.28f, 0.04f);
                    SpawnCross(center, Vector3.right, name + "BrandSlash", color, scale * 0.42f, 0.03f);
                    break;
                case PrototypeEffectKind.Kalmuri:
                    SpawnKalmuriOrbit(center, level, color);
                    break;
                case PrototypeEffectKind.Blood:
                    SpawnRingVfx(name + "BloodHalo", center, scale * 0.8f, color, 0.24f, 0.035f, 14);
                    SpawnSpriteVfx(name + "BloodSprite", bloodReflectionSprite, center, 0.22f + level * 0.02f, 0.26f, color, 80f, 39);
                    break;
            }
        }

        private void SpawnRingVfx(string name, Vector3 center, float radius, Color color, float lifetime, float width, int segments)
        {
            var previous = center + Vector3.right * radius;
            for (var index = 1; index <= segments; index += 1)
            {
                var angle = index * (360f / segments);
                var next = center + Quaternion.Euler(0f, 0f, angle) * Vector3.right * radius;
                SpawnLineVfx(name, previous, next, color, lifetime, width);
                previous = next;
            }
        }

        private void SpawnDiamondVfx(string name, Vector3 center, float radius, Color color, float lifetime, float width)
        {
            var up = center + Vector3.up * radius;
            var right = center + Vector3.right * radius * 0.72f;
            var down = center + Vector3.down * radius;
            var left = center + Vector3.left * radius * 0.72f;
            SpawnLineVfx(name, up, right, color, lifetime, width);
            SpawnLineVfx(name, right, down, color, lifetime, width);
            SpawnLineVfx(name, down, left, color, lifetime, width);
            SpawnLineVfx(name, left, up, color, lifetime, width);
        }

        private int DamageClosestEnemiesInRadius(Vector3 center, float radius, int maxTargets, float damage, string effectName, Color color, float knockback, Sprite sprite)
        {
            var candidates = CollectEnemiesInRadius(center, radius);
            var hits = Mathf.Min(maxTargets, candidates.Count);
            for (var index = 0; index < hits; index += 1)
            {
                var enemy = candidates[index];
                var direction = enemy.transform.position - center;
                direction.z = 0f;
                direction = direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector3.right;
                enemy.Health.ApplyDamage(damage, gameObject);
                enemy.ApplyKnockback(direction, knockback);
                SpawnCross(enemy.transform.position, direction, effectName, color, 0.34f, 0.03f);
                SpawnSpriteVfx(effectName + "Sprite", sprite, enemy.transform.position, 0.3f, 0.28f, color, 160f, 41);
            }

            return hits;
        }

        private List<PrototypeEnemy> CollectEnemiesInRadius(Vector3 center, float radius)
        {
            var candidates = new List<PrototypeEnemy>();
            if (spawner == null)
            {
                return candidates;
            }

            var radiusSqr = radius * radius;
            var enemies = spawner.Enemies;
            for (var index = 0; index < enemies.Count; index += 1)
            {
                var enemy = enemies[index];
                if (enemy == null || enemy.Health == null || enemy.Health.IsDead)
                {
                    continue;
                }

                if ((enemy.transform.position - center).sqrMagnitude <= radiusSqr)
                {
                    candidates.Add(enemy);
                }
            }

            candidates.Sort((left, right) =>
                (left.transform.position - center).sqrMagnitude.CompareTo((right.transform.position - center).sqrMagnitude));
            return candidates;
        }

        private void ExecuteWeakestEnemy(Vector3 center, float radius, float damage, Color color)
        {
            var candidates = CollectEnemiesInRadius(center, radius);
            PrototypeEnemy weakest = null;
            var lowestRatio = 2f;
            for (var index = 0; index < candidates.Count; index += 1)
            {
                var enemy = candidates[index];
                var ratio = enemy.Health.CurrentHealth / Mathf.Max(1f, enemy.Health.MaxHealth);
                if (ratio < lowestRatio)
                {
                    lowestRatio = ratio;
                    weakest = enemy;
                }
            }

            if (weakest == null)
            {
                return;
            }

            weakest.Health.ApplyDamage(damage * (lowestRatio < 0.42f ? 2f : 1f), gameObject);
            SpawnCross(weakest.transform.position, Vector3.right, "ActiveExecutionPeriodic", color, 0.54f, 0.045f);
        }

        private void HitNearestEnemy(Vector3 origin, float range, float damage, string effectName, Color color, Sprite sprite)
        {
            if (spawner == null)
            {
                return;
            }

            var enemy = spawner.FindNearest(origin, range);
            if (enemy == null)
            {
                return;
            }

            enemy.Health.ApplyDamage(damage, gameObject);
            SpawnLineVfx(effectName, origin, enemy.transform.position, color, 0.18f, 0.032f);
            SpawnSpriteVfx(effectName + "Sprite", sprite, enemy.transform.position, 0.24f, 0.28f, color, 180f, 41);
        }

        private void HitNearestOther(PrototypeEnemy excluded, Vector3 origin, float range, float damage, string effectName, Color color, Sprite sprite)
        {
            var candidates = CollectEnemiesInRadius(origin, range);
            for (var index = 0; index < candidates.Count; index += 1)
            {
                var enemy = candidates[index];
                if (enemy == excluded)
                {
                    continue;
                }

                enemy.Health.ApplyDamage(damage, gameObject);
                SpawnLineVfx(effectName, origin, enemy.transform.position, color, 0.18f, 0.032f);
                SpawnSpriteVfx(effectName + "Sprite", sprite, enemy.transform.position, 0.24f, 0.28f, color, 180f, 41);
                return;
            }
        }

        private void SpawnCross(Vector3 center, Vector3 direction, string name, Color color, float length, float width)
        {
            direction.z = 0f;
            direction = direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector3.right;
            var cross = Quaternion.Euler(0f, 0f, 86f) * direction;
            SpawnLineVfx(name, center - cross * length, center + cross * length, color, 0.16f, width);
        }

        private void SpawnKalmuriOrbit(Vector3 center, int level, Color color)
        {
            var count = Mathf.Clamp(2 + level / 2, 2, 4);
            var orbitRadius = 0.72f + level * 0.035f;
            for (var index = 0; index < count; index += 1)
            {
                var angle = Time.time * 210f + index * (360f / count);
                var offset = Quaternion.Euler(0f, 0f, angle) * Vector3.right * orbitRadius;
                SpawnSpriteVfx("ActiveKalmuriOrbitBlade", hungryBladesActiveSprite, center + offset, 0.18f + level * 0.018f, 0.32f, color, 260f, 18);
            }
        }

        private void DrawPersistentLoops()
        {
            if (player == null || Time.frameCount % 12 != 0)
            {
                return;
            }

            var center = player.transform.position;
            var memorySnapshot = new List<KeyValuePair<string, int>>(activeMemories.Levels);
            foreach (var pair in memorySnapshot)
            {
                var spec = FindMemory(pair.Key);
                var offset = Quaternion.Euler(0f, 0f, Time.time * 110f + (int)spec.Kind * 31f) * Vector3.right * (0.52f + (int)spec.Kind * 0.015f);
                SpawnSpriteVfx("ActiveMemoryLoop", SpriteForKind(spec.Kind), center + offset, 0.12f + pair.Value * 0.012f, 0.18f, new Color(spec.Color.r, spec.Color.g, spec.Color.b, 0.26f), 120f, 12);
                if (spec.Kind != PrototypeEffectKind.Kalmuri && spec.Kind != PrototypeEffectKind.Blood)
                {
                    SpawnProceduralMemoryVfx(spec.Kind, center + offset * 1.15f, new Color(spec.Color.r, spec.Color.g, spec.Color.b, 0.42f), Mathf.Max(1, pair.Value), "Persistent" + spec.Kind);
                }
            }

            var synergySnapshot = new List<string>(ultimateService.Unlocked);
            foreach (var synergyId in synergySnapshot)
            {
                var spec = FindSynergy(synergyId);
                SpawnSpriteVfx("UltimateGoalLoop", bloodBladeStormSprite, center, 0.48f, 0.18f, new Color(spec.Color.r, spec.Color.g, spec.Color.b, 0.24f), 220f, 9);
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
            runTime = 0f;
            nextChoiceKills = firstChoiceKills;
            nextForgetKills = firstForgetKills;
            earliestForgetKills = firstForgetKills;
            earliestForgetTime = 0f;
            activeMemories.Clear();
            echoes.Clear();
            ultimateService.Clear();
            rewardService.Reset();
            resonance.Clear();
            ShowNotice("사망 -> Complete Prototype 리셋");
        }

        private void HandleDebugKeys()
        {
#if ENABLE_INPUT_SYSTEM
            var keyboard = UnityEngine.InputSystem.Keyboard.current;
            if (keyboard != null)
            {
                if (keyboard.f1Key.wasPressedThisFrame) offeringChoice = true;
                if (keyboard.f2Key.wasPressedThisFrame) TriggerForget();
                if (keyboard.f3Key.wasPressedThisFrame) AddNextMemory();
                if (keyboard.f4Key.wasPressedThisFrame) ForceAllEchoes(1);
                if (keyboard.f5Key.wasPressedThisFrame) ForceAllEchoes(5);
                if (keyboard.f6Key.wasPressedThisFrame) ToggleWeapon();
                if (keyboard.f7Key.wasPressedThisFrame) ForceAllMemories();
                if (keyboard.f8Key.wasPressedThisFrame) ForceAllUltimate();
                return;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.F1)) offeringChoice = true;
            if (Input.GetKeyDown(KeyCode.F2)) TriggerForget();
            if (Input.GetKeyDown(KeyCode.F3)) AddNextMemory();
            if (Input.GetKeyDown(KeyCode.F4)) ForceAllEchoes(1);
            if (Input.GetKeyDown(KeyCode.F5)) ForceAllEchoes(5);
            if (Input.GetKeyDown(KeyCode.F6)) ToggleWeapon();
            if (Input.GetKeyDown(KeyCode.F7)) ForceAllMemories();
            if (Input.GetKeyDown(KeyCode.F8)) ForceAllUltimate();
#endif
        }

        private void DrawMemoryChoiceOverlay()
        {
            var width = 560f;
            var height = 250f;
            var x = Screen.width * 0.5f - width * 0.5f;
            var y = Screen.height * 0.5f - height * 0.5f;
            GUI.Box(new Rect(x, y, width, height), "기억 선택", panelStyle);
            GUI.Label(new Rect(x + 22f, y + 36f, width - 44f, 22f), "8기억 중 하나를 활성화한다. 잔향이 있으면 재획득 공명으로 시작 레벨이 오른다.", labelStyle);

            for (var index = 0; index < MemorySpecs.Count; index += 1)
            {
                var spec = MemorySpecs[index];
                var row = index / 4;
                var col = index % 4;
                var rect = new Rect(x + 22f + col * 130f, y + 72f + row * 72f, 118f, 58f);
                if (GUI.Button(rect, $"{spec.DisplayName}\n{spec.ActiveNote}", buttonStyle))
                {
                    ChooseMemory(spec.Id);
                }
            }

            if (GUI.Button(new Rect(x + width - 112f, y + height - 42f, 90f, 28f), "닫기", buttonStyle))
            {
                offeringChoice = false;
            }
        }

        private string NextForgetCandidate()
        {
            var id = activeMemories.FindHighestLevelMemory();
            if (string.IsNullOrEmpty(id))
            {
                return "-";
            }

            var protection = ForgetProtectionLabel();
            return string.IsNullOrEmpty(protection) ? $"{DisplayName(id)} +{activeMemories.GetLevel(id)}" : $"{DisplayName(id)} +{activeMemories.GetLevel(id)} ({protection})";
        }

        private void ArmMemoryHuntingWindow()
        {
            earliestForgetKills = Mathf.Max(earliestForgetKills, kills + minActiveMemoryKillsBeforeForget);
            earliestForgetTime = Mathf.Max(earliestForgetTime, runTime + minActiveMemorySecondsBeforeForget);
            nextForgetKills = Mathf.Max(nextForgetKills, earliestForgetKills);
        }

        private bool CanAutoForget()
        {
            return kills >= earliestForgetKills && runTime >= earliestForgetTime;
        }

        private string ForgetProtectionLabel()
        {
            if (CanAutoForget())
            {
                return string.Empty;
            }

            var remainingKills = Mathf.Max(0, earliestForgetKills - kills);
            var remainingTime = Mathf.Max(0f, earliestForgetTime - runTime);
            if (remainingKills > 0 && remainingTime > 0.1f)
            {
                return $"보호 {remainingKills}킬/{Mathf.CeilToInt(remainingTime)}초";
            }

            if (remainingKills > 0)
            {
                return $"보호 {remainingKills}킬";
            }

            return $"보호 {Mathf.CeilToInt(remainingTime)}초";
        }

        private string CurrentWeaponName()
        {
            return weapon != null ? weapon.CurrentWeaponName : "절단쌍검";
        }

        private static string MatchingEcho(string memoryId)
        {
            return FindMemory(memoryId).EchoId;
        }

        private static PrototypeMemorySpec FindMemory(string id)
        {
            for (var index = 0; index < MemorySpecs.Count; index += 1)
            {
                if (MemorySpecs[index].Id == id)
                {
                    return MemorySpecs[index];
                }
            }

            return MemorySpecs[0];
        }

        private static PrototypeEchoSpec FindEcho(string id)
        {
            for (var index = 0; index < EchoSpecs.Count; index += 1)
            {
                if (EchoSpecs[index].Id == id)
                {
                    return EchoSpecs[index];
                }
            }

            return EchoSpecs[0];
        }

        private static PrototypeSynergySpec FindSynergy(string id)
        {
            for (var index = 0; index < SynergySpecs.Count; index += 1)
            {
                if (SynergySpecs[index].Id == id)
                {
                    return SynergySpecs[index];
                }
            }

            return SynergySpecs[0];
        }

        private Sprite SpriteForKind(PrototypeEffectKind kind)
        {
            return kind switch
            {
                PrototypeEffectKind.Blood => bloodReflectionSprite,
                PrototypeEffectKind.AshenGuard => bloodBloomSprite,
                PrototypeEffectKind.Brand => bloodReflectionSprite,
                PrototypeEffectKind.Kalmuri => hungryBladesActiveSprite,
                _ => kalmuriSlashSprite != null ? kalmuriSlashSprite : hungryBladesActiveSprite
            };
        }

        private static string DisplayName(string id)
        {
            for (var index = 0; index < MemorySpecs.Count; index += 1)
            {
                if (MemorySpecs[index].Id == id)
                {
                    return MemorySpecs[index].DisplayName;
                }
            }

            for (var index = 0; index < EchoSpecs.Count; index += 1)
            {
                if (EchoSpecs[index].Id == id)
                {
                    return EchoSpecs[index].DisplayName;
                }
            }

            for (var index = 0; index < SynergySpecs.Count; index += 1)
            {
                if (SynergySpecs[index].Id == id)
                {
                    return SynergySpecs[index].DisplayName;
                }
            }

            return id;
        }

        private static string FormatState(IReadOnlyDictionary<string, int> state, bool memories)
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

        private string FormatSynergies()
        {
            if (ultimateService.Unlocked.Count == 0)
            {
                return "대기";
            }

            var parts = new List<string>();
            foreach (var id in ultimateService.Unlocked)
            {
                parts.Add(DisplayName(id));
            }

            return string.Join(", ", parts);
        }

        private void ShowNotice(string message)
        {
            notice = message;
            noticeUntil = Time.time + 2f;
        }

        private void EnsureGuiStyles()
        {
            if (labelStyle != null)
            {
                return;
            }

            var baseFont = koreanFont != null ? koreanFont : GUI.skin.font;
            panelStyle = new GUIStyle(GUI.skin.box)
            {
                font = baseFont,
                fontSize = 15,
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = new Color(0.88f, 0.92f, 0.96f, 1f) },
                padding = new RectOffset(10, 10, 8, 8)
            };
            labelStyle = new GUIStyle(GUI.skin.label)
            {
                font = baseFont,
                fontSize = 13,
                normal = { textColor = new Color(0.9f, 0.94f, 0.98f, 1f) },
                wordWrap = true
            };
            noticeStyle = new GUIStyle(panelStyle)
            {
                fontSize = 15,
                alignment = TextAnchor.MiddleCenter
            };
            buttonStyle = new GUIStyle(GUI.skin.button)
            {
                font = baseFont,
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true
            };
        }
    }
}
