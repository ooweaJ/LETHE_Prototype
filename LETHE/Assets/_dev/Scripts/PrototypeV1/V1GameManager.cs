using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lethe.Dev;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Lethe.PrototypeV1
{
    public enum V1EnemyKind
    {
        Eroder,
        DriftingEye,
        SplitOne,
        VoidPriest,
        Gatekeeper
    }

    public enum V1MemoryId
    {
        HungryBlades,
        BloodReflection,
        ExecutionFlash,
        HunterOath,
        ShatterWave,
        StoppedSecond,
        AshenShield,
        OblivionBrand
    }

    public enum V1WeaponId
    {
        DualBlades,
        Greatsword
    }

    enum V1WeaponTargetingMode
    {
        Nearest,
        DensestArc
    }

    enum V1EchoProcStyle
    {
        MultiSmall,
        SingleHeavy
    }

    enum V1UltimatePattern
    {
        ManyFast,
        FewHeavy
    }

    enum V1SfxWave
    {
        Sine,
        Triangle,
        Square,
        Noise,
        Blade
    }

    public sealed class V1GameManager : MonoBehaviour
    {
        const float PixelsPerUnit = 40f;
        const float PlayerSpeed = 184f / PixelsPerUnit;
        const float TwinBladeRange = 112f / PixelsPerUnit;
        const float TwinBladeInterval = 0.32f;
        const float TwinBladeDamage = 13.5f;
        const float TwinBladeArcDeg = 132f;
        const float TwinBladeEngageMul = 1.20f;
        const float GreatswordRange = 150f / PixelsPerUnit;
        const float GreatswordInterval = 0.92f;
        const float GreatswordDamage = 40f;
        const float GreatswordArcDeg = 102f;
        const float GreatswordEngageMul = 1.12f;
        const float FirstBossSeconds = 150f;
        const float BossWarningSeconds = 20f;
        const float FastFirstBossSeconds = 62f;
        const float DeficitSurvivalSeconds = 0f;
        const float FastDeficitSeconds = 6f;
        const float HungryBladesRadius = 86f / PixelsPerUnit;
        const float HungryBladesDps = 28f;
        const int KalmuriOrbitBladeCap = 12;
        const int KalmuriBiteTargetCap = 3;
        const int KalmuriBiteBladeCap = 4;
        const int KalmuriMaxPendingFollowups = 10;
        const int DenseDualBladeThrottleEnemyCount = 24;
        const int DenseDualBladeEchoHitLimit = 1;
        const int DenseDualBladeSlashHitLimit = 1;
        const int DenseDualBladeKalmuriSurgeLimit = 2;
        const int MaxMemoryLevel = 5;
        const int MaxEchoLevel = 5;
        const int MaxActiveMemories = 3;
        const float FirstBossHp = 2200f;
        const float FastBossHp = 180f;
        const float DebugReviewBossHp = FirstBossHp;
        const float RunSeconds = 1080f;
        const float ArenaHalfWidth = 24f;
        const float ArenaHalfHeight = 16f;
        const float ArenaTileSpacing = 2.65f;
        const float EnemySeparationPadding = 0.10f;
        const float EnemySeparationMax = 1.15f;
        const float EnemySeparationProbeStep = 0.22f;
        const float PlayerMoveAcceleration = 14f;
        const float PlayerMoveDeceleration = 22f;
        const float DualBladeVisualScale = 0.43f;
        const float GreatswordVisualScale = 0.21f;
        const float DualBladePhantomHeight = 0.82f;
        const float GreatswordPhantomHeight = 1.72f;
        const float DualBladePhantomLifetime = 0.24f;
        const float GreatswordPhantomLifetime = 0.42f;
        const float DualBladeSlashDelay = 0.045f;
        const float DualBladeSecondSlashExtraDelay = 0.040f;
        const float GreatswordSlashDelay = 0.025f;
        const float WeaponSlashLifetimeMultiplier = 1.28f;
        const float CombatVfxVisibilityScale = 1.18f;
        const float DualBladeSlashMinLifetime = 0.28f;
        const float GreatswordSlashMinLifetime = 0.48f;
        const float GreatswordCenterToTipRatio = 0.44f;
        const float GreatswordHandleToTipRatio = 0.86f;
        const float GreatswordHandleToCenterRatio = GreatswordHandleToTipRatio - GreatswordCenterToTipRatio;
        const float GreatswordTipForwardOffset = 0.30f;
        const float GreatswordTipSweepSideOffset = 0.28f;
        const float GreatswordSwingHalfAngle = 45f;
        const float GreatswordSlashFacingCorrection = 180f;
        const float GreatswordPhantomSweepDuration = 0.28f;
        const float GreatswordPhantomAfterimageSweepDuration = 0.32f;
        const string PlayerSheetPath = "Assets/_dev/Art/Sprites/Characters/Player/sheet_player_v1_4dir.png";
        const string EnemyChaserSheetPath = "Assets/_dev/Art/Sprites/Enemies/Chaser/sheet_enemy_chaser_4dir.png";
        const string EnemyEyeSheetPath = "Assets/_dev/Art/Sprites/Enemies/Eye/sheet_enemy_eye_4dir.png";
        const string EnemySplitterSheetPath = "Assets/_dev/Art/Sprites/Enemies/Splitter/sheet_enemy_splitter_4dir.png";
        const string EnemyVoidPriestSheetPath = "Assets/_dev/Art/Sprites/Enemies/VoidPriest/sheet_enemy_voidpriest_4dir.png";
        const string BossGatekeeperPath = "Assets/_dev/Art/Sprites/Enemies/Bosses/spr_boss_gatekeeper_01.png";
        static readonly string[] BossGatekeeperRankPaths =
        {
            BossGatekeeperPath,
            "Assets/_dev/Art/Sprites/Enemies/Bosses/spr_boss_gatekeeper_02.png",
            "Assets/_dev/Art/Sprites/Enemies/Bosses/spr_boss_gatekeeper_03.png",
            "Assets/_dev/Art/Sprites/Enemies/Bosses/spr_boss_gatekeeper_04.png"
        };
        const string ArenaBackdropPath = "Assets/_dev/Art/Sprites/Map/spr_lethe_terrain_backdrop_01.png";
        static readonly string[] ArenaFloorTilePaths =
        {
            "Assets/_dev/Art/Sprites/Map/tile_lethe_terrain_01.png",
            "Assets/_dev/Art/Sprites/Map/tile_lethe_terrain_02.png",
            "Assets/_dev/Art/Sprites/Map/tile_lethe_terrain_03.png",
            "Assets/_dev/Art/Sprites/Map/tile_lethe_terrain_04.png",
            "Assets/_dev/Art/Sprites/Map/tile_lethe_terrain_05.png",
            "Assets/_dev/Art/Sprites/Map/tile_lethe_terrain_06.png",
            "Assets/_dev/Art/Sprites/Map/tile_lethe_terrain_07.png",
            "Assets/_dev/Art/Sprites/Map/tile_lethe_terrain_08.png"
        };
        const string DualBladeSwingArcAPath = "Assets/_dev/Art/Sprites/Weapons/spr_dual_blade_swing_arc_01.png";
        const string DualBladeSwingArcBPath = "Assets/_dev/Art/Sprites/Weapons/spr_dual_blade_swing_arc_02.png";
        const string GreatswordCleaveArcPath = "Assets/_dev/Art/Sprites/Weapons/spr_greatsword_cleave_arc_01.png";
        const string KalmuriOrbitBladePath = "Assets/_dev/Art/Sprites/Echoes/Kalmuri/spr_kalmuri_orbit_blade_01.png";
        const string KalmuriEchoSlashPath = "Assets/_dev/Art/Sprites/Echoes/Kalmuri/spr_kalmuri_echo_slash_01.png";
        const string KalmuriLaunchBladePath = "Assets/_dev/Art/Sprites/Echoes/Kalmuri/spr_kalmuri_launch_blade_01.png";
        const string HitSparkCyanPath = "Assets/_dev/Art/Sprites/Feedback/spr_hit_spark_cyan_01.png";
        const string HitSparkRedPath = "Assets/_dev/Art/Sprites/Feedback/spr_hit_spark_red_01.png";
        const string MemoryExecutionPath = "Assets/_dev/Art/Sprites/Memories/Execution/spr_execution_flash_01.png";
        const string MemoryHunterPath = "Assets/_dev/Art/Sprites/Memories/Homing/spr_homing_shot_01.png";
        const string MemoryShatterPath = "Assets/_dev/Art/Sprites/Memories/Shockwave/spr_shockwave_ring_01.png";
        const string MemoryStoppedPath = "Assets/_dev/Art/Sprites/Memories/TimeStop/spr_timestop_field_01.png";
        const string MemoryAshenPath = "Assets/_dev/Art/Sprites/Memories/Ashen/spr_ashen_shield_01.png";
        const string MemoryBrandPath = "Assets/_dev/Art/Sprites/Memories/Brand/spr_brand_mark_01.png";
        const string EchoExecutionPath = "Assets/_dev/Art/Sprites/Echoes/Execution/spr_execution_burst_01.png";
        const string EchoHunterPath = "Assets/_dev/Art/Sprites/Echoes/Homing/spr_homing_echo_01.png";
        const string EchoShatterPath = "Assets/_dev/Art/Sprites/Echoes/Shockwave/spr_shockwave_echo_01.png";
        const string EchoStoppedPath = "Assets/_dev/Art/Sprites/Echoes/TimeStop/spr_timestop_echo_01.png";
        const string EchoAshenPath = "Assets/_dev/Art/Sprites/Echoes/Ashen/spr_ashen_echo_01.png";
        const string EchoBrandPath = "Assets/_dev/Art/Sprites/Echoes/Brand/spr_brand_echo_01.png";
        const string BloodBladeStormRingPath = "Assets/_dev/Art/Sprites/Ultimates/spr_blood_blade_storm_ring_01.png";
        const string BloodBladeStormBladePath = "Assets/_dev/Art/Sprites/Ultimates/spr_blood_blade_storm_blade_01.png";
        const string UltimateFracturePath = "Assets/_dev/Art/Sprites/Ultimates/spr_fracture_execution_01.png";
        const string UltimateStasisPath = "Assets/_dev/Art/Sprites/Ultimates/spr_stasis_hunt_01.png";
        const string UltimateAshenOblivionPath = "Assets/_dev/Art/Sprites/Ultimates/spr_ashen_oblivion_01.png";
        const string HunterOathSource = "추적자의 맹세";
        const string HunterOathBurstSource = "추적자의 맹세 폭발";
        const string HunterEchoSource = "추적 잔향";
        const string HunterEchoBurstSource = "추적 잔향 폭발";
        static readonly float[] BossScheduleSeconds = { 150f, 300f, 540f, 900f };
        static readonly float[] FastBossScheduleSeconds = { 18f, 38f, 62f, 88f };
        static readonly V1MemoryId[] UtilityMemorySetA = { V1MemoryId.ExecutionFlash, V1MemoryId.HunterOath, V1MemoryId.StoppedSecond };
        static readonly V1MemoryId[] UtilityMemorySetB = { V1MemoryId.ShatterWave, V1MemoryId.AshenShield, V1MemoryId.OblivionBrand };
        static readonly V1MemoryId[] CoreEchoIds =
        {
            V1MemoryId.HungryBlades,
            V1MemoryId.BloodReflection
        };
        static readonly V1MemoryId[][] UltimateEchoPairs =
        {
            new[] { V1MemoryId.HungryBlades, V1MemoryId.BloodReflection },
            new[] { V1MemoryId.ShatterWave, V1MemoryId.ExecutionFlash },
            new[] { V1MemoryId.StoppedSecond, V1MemoryId.HunterOath },
            new[] { V1MemoryId.AshenShield, V1MemoryId.OblivionBrand }
        };
        static readonly V1MemoryId[] UtilityMemoryIds =
        {
            V1MemoryId.ExecutionFlash,
            V1MemoryId.HunterOath,
            V1MemoryId.ShatterWave,
            V1MemoryId.StoppedSecond,
            V1MemoryId.AshenShield,
            V1MemoryId.OblivionBrand
        };
        static readonly V1MemoryId[] AllEchoIds =
        {
            V1MemoryId.HungryBlades,
            V1MemoryId.BloodReflection,
            V1MemoryId.ExecutionFlash,
            V1MemoryId.HunterOath,
            V1MemoryId.ShatterWave,
            V1MemoryId.StoppedSecond,
            V1MemoryId.AshenShield,
            V1MemoryId.OblivionBrand
        };
        static readonly UtilityEchoTuningSpec[] DefaultUtilityEchoTuningSpecTable =
        {
            new(V1MemoryId.ExecutionFlash, false, 0, 1f, 0f, 1f, 0f, 1f, 0f, 1, 1, 0, 0, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.22f, 0.025f),
            new(V1MemoryId.HunterOath, true, 0, 0.36f, 0.09f, 1f, 0f, 1f, 0f, 1, 1, 3, 0, 0.22f, 0.055f, 0.22f, 0.055f, 0f, 0f, 0f, 0f, 0f, 0f),
            new(V1MemoryId.ShatterWave, true, 2, 0.26f, 0.09f, 1.00f, 0.12f, 1.34f, 0.20f, 5, 9, 0, 0, 0.10f, 0.025f, 0.15f, 0.038f, 0f, 0f, 0f, 0f, 0f, 0f),
            new(V1MemoryId.StoppedSecond, true, 0, 0.34f, 0.065f, 0.84f, 0.055f, 1.78f, 0.20f, 3, 8, 0, 0, 0.030f, 0.010f, 0.070f, 0.018f, 0.24f, 0.045f, 0.52f, 0.07f, 0f, 0f),
            new(V1MemoryId.AshenShield, true, 0, 0.30f, 0.055f, 0.42f, 0f, 1.95f, 0.12f, 1, 8, 0, 0, 0.045f, 0.012f, 0.08f, 0.018f, 0f, 0f, 0f, 0f, 0f, 0f),
            new(V1MemoryId.OblivionBrand, false, 0, 0.34f, 0.065f, 0.64f, 0f, 1.18f, 0.12f, 1, 7, 0, 0, 0.16f, 0.042f, 0.13f, 0.036f, 0f, 0f, 0f, 0f, 0f, 0f)
        };

        [Header("Data")]
        [SerializeField] V1ContentCatalog contentCatalog;
        [SerializeField] WeaponDefinition dualBladesDefinition;
        [SerializeField] WeaponDefinition greatswordDefinition;
        [SerializeField] V1UtilityEchoTuningTable utilityEchoTuningTable;
        [SerializeField] UtilityEchoTuningSpec[] utilityEchoTuningSpecs = DefaultUtilityEchoTuningSpecTable.ToArray();

        readonly List<V1Enemy> enemies = new();
        readonly List<V1XpOrb> xpOrbs = new();
        readonly List<string> combatLog = new();
        readonly List<MemoryState> activeMemories = new();
        readonly List<PendingKalmuriFollowup> pendingKalmuriFollowups = new();
        readonly Dictionary<V1MemoryId, int> echoLevels = new();
        readonly Dictionary<string, Sprite> spriteCache = new();
        static readonly Dictionary<string, Sprite> generatedSpriteCache = new();
        readonly Stack<GameObject> transientSpritePool = new();
        readonly Stack<GameObject> floatingTextPool = new();
        readonly Stack<GameObject> damageNumberPool = new();
        readonly Stack<GameObject> xpOrbPool = new();
        readonly List<Choice> currentLevelUpChoices = new();
        readonly System.Random rng = new(120612);

        Camera mainCamera;
        Transform player;
        Transform playerVisual;
        Transform weaponAnchor;
        SpriteRenderer playerSprite;
        SpriteRenderer leftBladeSprite;
        SpriteRenderer rightBladeSprite;
        AudioSource sfxSource;
        Sprite dualLeftWeaponSprite;
        Sprite dualRightWeaponSprite;
        Sprite greatswordWeaponSprite;
        readonly Dictionary<string, AudioClip> sfxClips = new();
        readonly Dictionary<string, float> sfxLastPlayed = new();
        GUIStyle smallStyle;
        GUIStyle titleStyle;
        GUIStyle buttonStyle;
        GUIStyle panelStyle;
        GUIStyle startEyebrowStyle;
        GUIStyle startBodyStyle;
        GUIStyle startCardTitleStyle;
        GUIStyle startKeyStyle;
        GUIStyle startFooterStyle;
        Font koreanFont;

        float playerHp = 210f;
        float playerMaxHp = 210f;
        float elapsed;
        float playerAnimTimer;
        float playerWalkCycle;
        float weaponTimer;
        float weaponAnimTimer;
        bool leftBladeLead = true;
        Vector2 lastAim = Vector2.up;
        Vector2 playerMoveVelocity;
        int playerAnimFrame;
        int playerFacingRow;
        float spawnTimer;
        float bossTimer = FirstBossSeconds;
        float refillTimer;
        float hitstopTimer;
        float cameraShakeTimer;
        float cameraShakeAmount;
        float ultimatePulseTimer;
        float bloodStormBurstTimer;
        float kalmuriAwakenLaunchCooldown;
        float ashenStoredGuardCharge;
        float ashenCounterCooldown;
        int level = 1;
        int xp;
        int nextXp = 8;
        int kills;
        int gatekeeperKills;
        int memoriesForgotten;
        int choicesTaken;
        int bossSpawnIndex;
        int warnedBossIndex = -1;
        int debugEchoIndex;
        int debugSeparationOverlapBefore;
        int debugSeparationOverlapAfter;
        int debugVoidPriestHealAttempts;
        int debugVoidPriestHealAccepted;
        int debugTransientSpriteSpawnCount;
        int debugDenseDualBladeHits;
        int debugDenseDualBladeEchoesSuppressed;
        int debugDenseDualBladeTransient;
        float debugDenseDualBladeMs;
        bool debugForceDenseDualBladeThrottle;
        bool reviewBloodGranted;
        bool reviewHungryBoosted;
        bool reviewBloodBoosted;
        bool reviewThirdMemoryGranted;
        bool reviewEchoTopped;
        bool weaponSelectOverlay = true;
        bool runStarted;
        bool pausedForChoice;
        bool resultOverlay;
        bool refillOverlay;
        bool deathOverlay;
        bool fastDebugRun;
        bool debugReviewBossHp;
        bool echoOnlyDebugMode;
        bool runWon;
        bool showDebugPanel;
        bool bloodStormWasReady;
        string overlayTitle = "";
        string overlayBody = "";
        V1WeaponId currentWeaponId = V1WeaponId.DualBlades;
        V1MemoryId? lastForgotten;

        public static bool GameplayPaused { get; private set; }
        public static bool HitstopActive { get; private set; }
        public bool BloodBladeStormReady => EchoLevel(V1MemoryId.HungryBlades) >= 5 && EchoLevel(V1MemoryId.BloodReflection) >= 5;
        public bool FractureExecutionReady => EchoLevel(V1MemoryId.ShatterWave) >= 5 && EchoLevel(V1MemoryId.ExecutionFlash) >= 5;
        public bool StasisHuntReady => EchoLevel(V1MemoryId.StoppedSecond) >= 5 && EchoLevel(V1MemoryId.HunterOath) >= 5;
        public bool AshenOblivionReady => EchoLevel(V1MemoryId.AshenShield) >= 5 && EchoLevel(V1MemoryId.OblivionBrand) >= 5;
        public bool AnyUltimateReady => BloodBladeStormReady || FractureExecutionReady || StasisHuntReady || AshenOblivionReady;

        void Awake()
        {
            GameplayPaused = true;
            HitstopActive = false;
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                var camGo = new GameObject("Main Camera");
                mainCamera = camGo.AddComponent<Camera>();
                camGo.tag = "MainCamera";
            }

            mainCamera.orthographic = true;
            mainCamera.orthographicSize = 6.8f;
            mainCamera.backgroundColor = new Color(0.035f, 0.045f, 0.055f);

            WeaponStat.AttackSpeed = 0f;
            WeaponStat.DamageMul = 0f;
            ResolveCatalogDefaults();
            LoadFont();
            CreateAudio();
            CreateArena();
            CreatePlayer();
            Log("런 준비: 시작 무기를 선택하세요");
        }

        void Update()
        {
            if (deathOverlay)
            {
                GameplayPaused = true;
                HitstopActive = false;
                if (KeyDown(KeyCode.R))
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                }
                return;
            }

            if (weaponSelectOverlay)
            {
                GameplayPaused = true;
                HitstopActive = false;
                if (KeyDown(KeyCode.Alpha1)) BeginRun(V1WeaponId.DualBlades);
                if (KeyDown(KeyCode.Alpha2)) BeginRun(V1WeaponId.Greatsword);
                if (KeyDown(KeyCode.F6)) DebugJumpToGatekeeper();
                return;
            }

            if (KeyDown(KeyCode.F1)) AddMemory(V1MemoryId.HungryBlades, 5, true);
            if (KeyDown(KeyCode.F2)) AddMemory(V1MemoryId.BloodReflection, 5, true);
            if (KeyDown(KeyCode.F3)) ForgetHighestMemory();
            if (KeyDown(KeyCode.F4)) SetEcho(V1MemoryId.HungryBlades, 5);
            if (KeyDown(KeyCode.F5)) SetEcho(V1MemoryId.BloodReflection, 5);
            if (KeyDown(KeyCode.F6)) DebugJumpToGatekeeper();
            if (KeyDown(KeyCode.F7)) GrantXp(nextXp);
            if (KeyDown(KeyCode.F8)) DebugRunM2Smoke();
            if (KeyDown(KeyCode.F9)) ToggleDebugWeapon();
            if (KeyDown(KeyCode.F10)) DebugSetEchoOnlyLoadout(AllEchoIds, "Debug echo-only all 8 set");
            if (KeyDown(KeyCode.F11)) DebugSetSelectedEchoOnly();
            if (KeyDown(KeyCode.F12)) showDebugPanel = !showDebugPanel;
            if (KeyDown(KeyCode.Space) && resultOverlay)
            {
                ContinueAfterForgetResult();
            }
            if (pausedForChoice || resultOverlay || refillOverlay)
            {
                GameplayPaused = true;
                HitstopActive = false;
                return;
            }

            GameplayPaused = false;
            var dt = Time.deltaTime;
            if (hitstopTimer > 0f)
            {
                hitstopTimer -= dt;
                HitstopActive = hitstopTimer > 0f;
                UpdatePlayer(dt);
                UpdateCamera();
                UpdateWeaponVisuals(dt);
                UpdateHungryBladesVisualDuringHitstop(dt);
                return;
            }
            HitstopActive = false;

            elapsed += dt;
            if (ashenCounterCooldown > 0f) ashenCounterCooldown = Mathf.Max(0f, ashenCounterCooldown - dt);
            if (fastDebugRun)
            {
                UpdateReviewPacing();
            }
            bossTimer -= dt;
            if (bossTimer <= BossWarningSeconds && warnedBossIndex != bossSpawnIndex && !enemies.Any(e => e != null && e.Kind == V1EnemyKind.Gatekeeper))
            {
                warnedBossIndex = bossSpawnIndex;
                SpawnGatekeeperWarning();
            }
            if (bossTimer <= 0f && !enemies.Any(e => e != null && e.Kind == V1EnemyKind.Gatekeeper))
            {
                SpawnGatekeeper();
                bossTimer = 9999f;
            }

            if (refillTimer > 0f)
            {
                refillTimer = 0f;
            }

            UpdatePlayer(dt);
            UpdateCamera();
            UpdateWeaponVisuals(dt);
            UpdateWeapon(dt);
            UpdateActiveMemories(dt);
            kalmuriAwakenLaunchCooldown = Mathf.Max(0f, kalmuriAwakenLaunchCooldown - dt);
            if (!echoOnlyDebugMode)
            {
                UpdateEchoUltimate(dt);
            }
            UpdatePendingKalmuriFollowups(dt);
            UpdateSpawning(dt);
            UpdateXpCollection(dt);
            CleanupLists();

            if (playerHp <= 0f)
            {
                EndRun(false, "침몰");
                return;
            }

            if (elapsed >= RunSeconds && !deathOverlay)
            {
                EndRun(gatekeeperKills >= BossSchedule().Length, gatekeeperKills >= BossSchedule().Length ? "모든 관문 돌파" : "최종 관문 미돌파");
                return;
            }
        }

        public string DebugSnapshot()
        {
            var memoryText = string.Join(",", activeMemories.Select(m => $"{m.Id}:{m.Level}"));
            var echoText = string.Join(",", echoLevels.Where(kv => kv.Value > 0).Select(kv => $"{kv.Key}:{kv.Value}"));
            var liveEnemies = enemies.Count(e => e != null && e.IsAlive);
            return $"scene=v1 weapon={CurrentWeaponSpec().DisplayName} elapsed={elapsed:0.0} hp={playerHp:0.0}/{playerMaxHp:0.0} level={level} xp={xp}/{nextXp} kills={kills} memories=[{memoryText}] echoes=[{echoText}] enemies={liveEnemies} storm={BloodBladeStormReady} result={resultOverlay} refill={refillOverlay} death={deathOverlay}";
        }

        void EndRun(bool victory, string reason)
        {
            if (deathOverlay) return;
            runWon = victory;
            deathOverlay = true;
            pausedForChoice = false;
            resultOverlay = false;
            refillOverlay = false;
            GameplayPaused = true;
            HitstopActive = false;
            overlayTitle = victory ? "프로토타입 클리어" : "런 종료";
            overlayBody = BuildRunResultBody(reason);
            SpawnRunEndCue(victory);
            PlaySfx(victory ? "clear" : "defeat");
            Log(victory ? $"클리어: {reason}" : $"종료: {reason}");
        }

        string BuildRunResultBody(string reason)
        {
            var echoSummary = EchoText();
            if (echoSummary.Length > 42) echoSummary = echoSummary[..42] + "...";
            return
                $"{reason}\n" +
                $"생존 {Mathf.FloorToInt(elapsed)}초 / 처치 {kills} / 관문 {gatekeeperKills}/{BossSchedule().Length}\n" +
                $"{CurrentWeaponSpec().DisplayName} / Lv.{level} / 선택 {choicesTaken}회 / 망각 {memoriesForgotten}회\n" +
                $"잔향: {echoSummary}\n" +
                "R 또는 아래 버튼으로 다시 시작";
        }

        void SpawnRunEndCue(bool victory)
        {
            if (player == null) return;
            var color = victory ? new Color(0.58f, 1f, 0.78f, 0.74f) : new Color(1f, 0.30f, 0.34f, 0.68f);
            SpawnTransientSprite("RunEndOuter", MakeRingSprite("RunEndOuter", Color.white, 180), player.position, Quaternion.identity, victory ? 2.35f : 1.55f, color, 1.20f);
            SpawnTransientSprite("RunEndCore", MakeImpactDiamondSprite("RunEndCore", Color.white), player.position + Vector3.up * 0.18f, Quaternion.Euler(0f, 0f, 45f), victory ? 0.76f : 0.48f, new Color(1f, 1f, 1f, 0.72f), 0.72f);
            cameraShakeTimer = Mathf.Max(cameraShakeTimer, victory ? 0.38f : 0.22f);
            cameraShakeAmount = Mathf.Max(cameraShakeAmount, victory ? 0.11f : 0.07f);
        }

        void SpawnBossClearCue(Vector3 center)
        {
            SpawnTransientSprite("GatekeeperClearOuter", MakeRingSprite("GatekeeperClearOuter", Color.white, 180), center, Quaternion.identity, 2.25f, new Color(1f, 0.72f, 0.38f, 0.58f), 0.92f);
            SpawnTransientSprite("GatekeeperClearInner", MakeRingSprite("GatekeeperClearInner", Color.white, 132), center, Quaternion.Euler(0f, 0f, elapsed * -110f), 1.18f, new Color(0.66f, 1f, 0.92f, 0.48f), 0.78f);
            SpawnTransientSprite("GatekeeperClearCore", MakeImpactDiamondSprite("GatekeeperClearCore", Color.white), center + Vector3.up * 0.18f, Quaternion.Euler(0f, 0f, 45f), 0.56f, new Color(1f, 0.94f, 0.62f, 0.82f), 0.54f);
            SpawnFloatingText(center + Vector3.up * 0.95f, $"관문 {Mathf.Min(gatekeeperKills, BossSchedule().Length)}/{BossSchedule().Length}", new Color(1f, 0.88f, 0.48f));
            cameraShakeTimer = Mathf.Max(cameraShakeTimer, 0.26f);
            cameraShakeAmount = Mathf.Max(cameraShakeAmount, 0.09f);
        }

        void CreateAudio()
        {
            sfxSource = gameObject.GetComponent<AudioSource>();
            if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
            sfxSource.spatialBlend = 0f;
            sfxSource.volume = 0.42f;
            sfxClips["select"] = MakeSfxClip("sfx_select", 560f, 940f, 0.08f, 0.18f, V1SfxWave.Triangle, 0.02f, 0.004f, 1.8f);
            sfxClips["slash_dual"] = MakeSfxClip("sfx_slash_dual", 980f, 280f, 0.09f, 0.25f, V1SfxWave.Blade, 0.10f, 0.002f, 2.2f);
            sfxClips["slash_great"] = MakeSfxClip("sfx_slash_great", 320f, 74f, 0.18f, 0.36f, V1SfxWave.Blade, 0.12f, 0.006f, 1.4f);
            sfxClips["kalmuri_lunge"] = MakeSfxClip("sfx_kalmuri_lunge", 1120f, 520f, 0.105f, 0.24f, V1SfxWave.Blade, 0.16f, 0.001f, 2.5f);
            sfxClips["kalmuri_pierce"] = MakeSfxClip("sfx_kalmuri_pierce", 1380f, 840f, 0.055f, 0.18f, V1SfxWave.Blade, 0.22f, 0.001f, 3.0f);
            sfxClips["kalmuri_echo"] = MakeSfxClip("sfx_kalmuri_echo", 760f, 1180f, 0.13f, 0.24f, V1SfxWave.Blade, 0.10f, 0.004f, 1.7f);
            sfxClips["kalmuri_echo_heavy"] = MakeSfxClip("sfx_kalmuri_echo_heavy", 420f, 980f, 0.22f, 0.34f, V1SfxWave.Blade, 0.14f, 0.006f, 1.5f);
            sfxClips["blood_mark"] = MakeSfxClip("sfx_blood_mark", 190f, 320f, 0.16f, 0.22f, V1SfxWave.Sine, 0.06f, 0.008f, 1.6f);
            sfxClips["blood_heal"] = MakeSfxClip("sfx_blood_heal", 420f, 760f, 0.15f, 0.18f, V1SfxWave.Triangle, 0.04f, 0.006f, 1.8f);
            sfxClips["blood_storm"] = MakeSfxClip("sfx_blood_storm", 260f, 620f, 0.24f, 0.34f, V1SfxWave.Blade, 0.18f, 0.004f, 1.3f);
            sfxClips["execution"] = MakeSfxClip("sfx_execution", 1500f, 2200f, 0.16f, 0.25f, V1SfxWave.Triangle, 0.05f, 0.001f, 1.4f);
            sfxClips["hunter"] = MakeSfxClip("sfx_hunter", 760f, 1040f, 0.09f, 0.18f, V1SfxWave.Square, 0.04f, 0.002f, 2.0f);
            sfxClips["shatter"] = MakeSfxClip("sfx_shatter", 140f, 420f, 0.20f, 0.28f, V1SfxWave.Noise, 0.50f, 0.003f, 1.5f);
            sfxClips["stopped"] = MakeSfxClip("sfx_stopped", 880f, 880f, 0.10f, 0.18f, V1SfxWave.Triangle, 0.01f, 0.001f, 2.8f);
            sfxClips["ashen"] = MakeSfxClip("sfx_ashen", 300f, 520f, 0.22f, 0.18f, V1SfxWave.Triangle, 0.06f, 0.010f, 1.8f);
            sfxClips["brand"] = MakeSfxClip("sfx_brand", 260f, 110f, 0.18f, 0.22f, V1SfxWave.Square, 0.08f, 0.003f, 1.6f);
            sfxClips["xp"] = MakeSfxClip("sfx_xp", 960f, 1320f, 0.045f, 0.12f, V1SfxWave.Triangle, 0.01f, 0.001f, 2.0f);
            sfxClips["hit_player"] = MakeSfxClip("sfx_hit_player", 180f, 90f, 0.12f, 0.24f, V1SfxWave.Square, 0.10f, 0.004f, 1.6f);
            sfxClips["kill"] = MakeSfxClip("sfx_kill", 440f, 220f, 0.09f, 0.14f, V1SfxWave.Triangle, 0.04f, 0.004f, 2.0f);
            sfxClips["levelup"] = MakeSfxClip("sfx_levelup", 420f, 980f, 0.20f, 0.24f, V1SfxWave.Triangle, 0.02f, 0.004f, 1.3f);
            sfxClips["warning"] = MakeSfxClip("sfx_warning", 180f, 260f, 0.30f, 0.22f, V1SfxWave.Sine, 0.05f, 0.012f, 1.2f);
            sfxClips["boss_clear"] = MakeSfxClip("sfx_boss_clear", 260f, 740f, 0.28f, 0.25f, V1SfxWave.Triangle, 0.04f, 0.006f, 1.1f);
            sfxClips["clear"] = MakeSfxClip("sfx_clear", 360f, 1120f, 0.42f, 0.30f, V1SfxWave.Triangle, 0.02f, 0.006f, 1.0f);
            sfxClips["defeat"] = MakeSfxClip("sfx_defeat", 220f, 72f, 0.42f, 0.22f, V1SfxWave.Square, 0.09f, 0.006f, 1.3f);
        }

        void PlaySfx(string id, float volumeMul = 1f, float minInterval = 0f)
        {
            if (sfxSource == null || !sfxClips.TryGetValue(id, out var clip) || clip == null) return;
            if (minInterval > 0f && sfxLastPlayed.TryGetValue(id, out var last) && Time.unscaledTime - last < minInterval) return;
            sfxLastPlayed[id] = Time.unscaledTime;
            sfxSource.PlayOneShot(clip, Mathf.Clamp01(volumeMul));
        }

        static AudioClip MakeSfxClip(string name, float startHz, float endHz, float duration, float volume, V1SfxWave wave, float noiseAmount, float attack, float decayPower)
        {
            const int sampleRate = 22050;
            var sampleCount = Mathf.Max(1, Mathf.CeilToInt(sampleRate * duration));
            var samples = new float[sampleCount];
            var phase = 0f;
            for (int i = 0; i < sampleCount; i++)
            {
                var t = sampleCount <= 1 ? 1f : i / (float)(sampleCount - 1);
                var hz = Mathf.Lerp(startHz, endHz, t);
                phase += hz * Mathf.PI * 2f / sampleRate;
                var env = Mathf.Pow(1f - t, decayPower) * Mathf.Clamp01(t / Mathf.Max(0.001f, attack));
                var osc = wave switch
                {
                    V1SfxWave.Triangle => Mathf.Asin(Mathf.Sin(phase)) * (2f / Mathf.PI),
                    V1SfxWave.Square => Mathf.Sign(Mathf.Sin(phase)),
                    V1SfxWave.Noise => PseudoNoise(i) * 2f - 1f,
                    V1SfxWave.Blade => Mathf.Sin(phase) * 0.58f + Mathf.Sin(phase * 2.01f) * 0.24f + Mathf.Sin(phase * 3.97f) * 0.12f,
                    _ => Mathf.Sin(phase)
                };
                var noisy = (PseudoNoise(i) * 2f - 1f) * noiseAmount;
                samples[i] = Mathf.Clamp((osc * (1f - noiseAmount) + noisy) * env * volume, -1f, 1f);
            }
            var clip = AudioClip.Create(name, sampleCount, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        static float PseudoNoise(int i)
        {
            return Mathf.Repeat(Mathf.Sin(i * 12.9898f + 78.233f) * 43758.5453f, 1f);
        }

        public void DebugRunM1Smoke()
        {
            EnsureRunStarted();
            pausedForChoice = false;
            resultOverlay = false;
            refillOverlay = false;
            deathOverlay = false;
            fastDebugRun = true;
            debugReviewBossHp = false;
            echoOnlyDebugMode = false;
            runWon = false;
            gatekeeperKills = 0;
            memoriesForgotten = 0;
            choicesTaken = 0;
            bossSpawnIndex = 0;
            warnedBossIndex = -1;
            bossTimer = 42f;
            playerHp = Mathf.Max(playerHp, playerMaxHp * 0.65f);
            AddMemory(V1MemoryId.HungryBlades, 3, true);
            AddMemory(V1MemoryId.BloodReflection, 2, true);
            for (int i = 0; i < 8; i++)
            {
                var angle = i * Mathf.PI * 2f / 8f;
                var kind = i % 4 == 0 ? V1EnemyKind.DriftingEye : V1EnemyKind.Eroder;
                SpawnEnemy(kind, player.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * 2.35f);
            }
            Log("디버그 M1: 전투 셸 압축 시작");
        }

        void UpdateReviewPacing()
        {
            if (!reviewBloodGranted && elapsed >= 14f)
            {
                reviewBloodGranted = true;
                if (!HasMemory(V1MemoryId.BloodReflection))
                {
                    AddMemory(V1MemoryId.BloodReflection, 1, true);
                    SpawnFloatingText(player.position + Vector3.up * 0.8f, "피의 반사", new Color(1f, 0.32f, 0.38f));
                }
            }

            if (!reviewHungryBoosted && elapsed >= 28f)
            {
                reviewHungryBoosted = true;
                AddMemory(V1MemoryId.HungryBlades, 3, true);
                SpawnFloatingText(player.position + Vector3.up * 0.95f, "칼무리 성장", new Color(0.62f, 0.96f, 1f));
            }

            if (!reviewBloodBoosted && elapsed >= 42f)
            {
                reviewBloodBoosted = true;
                AddMemory(V1MemoryId.BloodReflection, 3, true);
                SpawnFloatingText(player.position + Vector3.up * 1.1f, "혈반 성장", new Color(1f, 0.26f, 0.36f));
            }

            if (!reviewThirdMemoryGranted && elapsed >= 50f)
            {
                reviewThirdMemoryGranted = true;
                if (activeMemories.Count < MaxActiveMemories && !HasMemory(V1MemoryId.StoppedSecond))
                {
                    AddMemory(V1MemoryId.StoppedSecond, 1, true);
                    SpawnFloatingText(player.position + Vector3.up * 1.15f, "멈춘 초침", new Color(0.82f, 0.86f, 1f));
                }
            }

            var gatekeeper = enemies.FirstOrDefault(e => e != null && e.IsAlive && e.Kind == V1EnemyKind.Gatekeeper);
            if (gatekeeper != null && elapsed >= 74f)
            {
                DealDamage(gatekeeper, 28f * Time.deltaTime, "문지기 균열", false);
            }

            if (!reviewEchoTopped && lastForgotten.HasValue && activeMemories.Count >= MaxActiveMemories && elapsed >= 86f)
            {
                reviewEchoTopped = true;
                SetEcho(V1MemoryId.HungryBlades, 5);
                SetEcho(V1MemoryId.BloodReflection, 5);
                SpawnFloatingText(player.position + Vector3.up * 1.25f, "피의 칼폭풍", new Color(1f, 0.18f, 0.24f));
                Log("리뷰 페이싱: 칼무리/혈반 잔향 +5 각성");
            }
        }

        public void DebugRunM2Smoke()
        {
            EnsureRunStarted();
            pausedForChoice = false;
            resultOverlay = false;
            refillOverlay = false;
            deathOverlay = false;
            fastDebugRun = true;
            debugReviewBossHp = false;
            echoOnlyDebugMode = false;
            bossSpawnIndex = 0;
            warnedBossIndex = -1;
            bossTimer = 18f;
            playerHp = Mathf.Max(playerHp, playerMaxHp * 0.75f);
            AddMemory(V1MemoryId.HungryBlades, 5, true);
            AddMemory(V1MemoryId.BloodReflection, 3, true);
            ForgetHighestMemory();
            ContinueAfterForgetResult();
            SetEcho(V1MemoryId.HungryBlades, 5);
            SetEcho(V1MemoryId.BloodReflection, 5);
            for (int i = 0; i < 10; i++)
            {
                var angle = i * Mathf.PI * 2f / 10f;
                var kind = i % 5 == 0 ? V1EnemyKind.SplitOne : V1EnemyKind.Eroder;
                SpawnEnemy(kind, player.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * 2.45f);
            }
            SpawnEchoTransformVfx(V1MemoryId.HungryBlades);
            overlayTitle = "M2 압축 스모크";
            overlayBody = "망각 -> 잔향 +5 -> 피의 칼폭풍 활성.\nContinue 버튼 또는 Space로 전투 복귀.";
            resultOverlay = true;
            Log("디버그 M2: 망각/잔향/궁극 루프 압축 완료");
        }

        void OnGUI()
        {
            EnsureStyles();
            if (weaponSelectOverlay)
            {
                DrawWeaponSelectOverlay();
                return;
            }

            DrawHud();

            if (pausedForChoice)
            {
                DrawLevelUpOverlay();
            }
            else if (resultOverlay || refillOverlay || deathOverlay)
            {
                DrawCenterOverlay();
            }
        }

        void CreateArena()
        {
            var floorSprites = ArenaFloorTilePaths
                .Select(path => LoadSprite(path))
                .Where(sprite => sprite != null)
                .ToArray();
            if (floorSprites.Length == 0)
            {
                floorSprites = new[] { LoadSprite("Assets/_dev/Art/Sprites/Map/tile_dev_floor_dark_01.png") ?? MakeBoxSprite("floor", new Color(0.08f, 0.09f, 0.105f), 64, 64) };
            }
            var backdrop = LoadSprite(ArenaBackdropPath) ?? MakeBoxSprite("arena_backdrop", Color.white, 16, 16);
            CreateArenaSprite("Terrain_Backdrop", Vector3.forward * 1.8f, new Vector3(76f, 76f, 1f), Quaternion.identity, backdrop, new Color(0.58f, 0.65f, 0.68f, 1f), -130);
            for (int x = -10; x <= 10; x++)
            {
                for (int y = -7; y <= 7; y++)
                {
                    var tile = new GameObject($"Floor_{x}_{y}");
                    tile.transform.position = new Vector3(x * ArenaTileSpacing, y * ArenaTileSpacing, 1f);
                    tile.transform.rotation = Quaternion.Euler(0f, 0f, ((x * 17 + y * 31) & 3) * 90f);
                    var sr = tile.AddComponent<SpriteRenderer>();
                    var terrainNoise = Mathf.PerlinNoise(x * 0.19f + 42.7f, y * 0.21f + 9.3f);
                    var tileIndex = floorSprites.Length <= 1
                        ? 0
                        : terrainNoise > 0.82f
                            ? Mathf.Abs((x * 13 + y * 7) % floorSprites.Length)
                            : Mathf.Abs((x * 3 + y * 5) % Mathf.Min(2, floorSprites.Length));
                    sr.sprite = floorSprites[tileIndex];
                    var v = Mathf.PerlinNoise(x * 0.37f + 12.1f, y * 0.41f + 4.7f);
                    sr.color = Color.Lerp(new Color(0.72f, 0.76f, 0.75f, 1f), new Color(0.90f, 0.94f, 0.90f, 1f), v * 0.24f);
                    sr.sortingOrder = -100;
                    tile.transform.localScale = Vector3.one * (1.08f + v * 0.02f);
                }
            }
            CreateArenaBackdrop();
            CreateLetheRiverAndRuins();
            CreateArenaLandmarks();
        }

        void CreateArenaBackdrop()
        {
            var edgeMist = MakeBoxSprite("terrain_edge_mist", Color.white, 96, 10);
            CreateArenaSprite("North_Marsh_Edge", new Vector3(0f, ArenaHalfHeight + 1.15f, 0.85f), new Vector3(ArenaHalfWidth * 2.35f, 1.6f, 1f), Quaternion.identity, edgeMist, new Color(0.05f, 0.09f, 0.10f, 0.58f), -86);
            CreateArenaSprite("South_Marsh_Edge", new Vector3(0f, -ArenaHalfHeight - 1.15f, 0.85f), new Vector3(ArenaHalfWidth * 2.35f, 1.7f, 1f), Quaternion.identity, edgeMist, new Color(0.04f, 0.07f, 0.08f, 0.66f), -86);
            CreateArenaSprite("West_Marsh_Edge", new Vector3(-ArenaHalfWidth - 1.15f, 0f, 0.85f), new Vector3(ArenaHalfHeight * 2.35f, 1.5f, 1f), Quaternion.Euler(0f, 0f, 90f), edgeMist, new Color(0.04f, 0.08f, 0.09f, 0.58f), -86);
            CreateArenaSprite("East_Marsh_Edge", new Vector3(ArenaHalfWidth + 1.15f, 0f, 0.85f), new Vector3(ArenaHalfHeight * 2.35f, 1.5f, 1f), Quaternion.Euler(0f, 0f, 90f), edgeMist, new Color(0.06f, 0.09f, 0.10f, 0.52f), -86);

            var waterSeam = MakeBoxSprite("lethe_water_seam", Color.white, 128, 9);
            for (int i = 0; i < 12; i++)
            {
                var x = -ArenaHalfWidth + 4.2f + (i % 4) * 11.2f + Mathf.Sin(i * 1.91f) * 1.2f;
                var y = -ArenaHalfHeight + 3.0f + (i / 4) * 8.8f + Mathf.Sin(i * 1.37f) * 1.2f;
                var angle = -18f + Mathf.Sin(i * 0.83f) * 26f;
                var len = 0.58f + (i % 4) * 0.22f;
                var color = new Color(0.12f, 0.48f, 0.52f, 0.085f + (i % 3) * 0.018f);
                CreateArenaSprite($"Lethe_Water_Seam_{i:00}", new Vector3(x, y, 0.82f), new Vector3(len, 1f, 1f), Quaternion.Euler(0f, 0f, angle), waterSeam, color, -84);
            }

            var root = MakeBoxSprite("drowned_root", Color.white, 104, 5);
            for (int i = 0; i < 14; i++)
            {
                var x = -ArenaHalfWidth + 2.8f + (i % 5) * 10.0f + Mathf.Sin(i * 2.1f) * 0.9f;
                var y = -ArenaHalfHeight + 2.6f + (i / 5) * 8.8f + Mathf.Cos(i * 1.6f) * 1.1f;
                var angle = 25f + Mathf.Sin(i * 0.91f) * 82f;
                var len = 0.32f + (i % 5) * 0.12f;
                CreateArenaSprite($"Drowned_Root_{i:00}", new Vector3(x, y, 0.83f), new Vector3(len, 1f, 1f), Quaternion.Euler(0f, 0f, angle), root, new Color(0.34f, 0.32f, 0.29f, 0.18f), -83);
            }

            var gravel = MakeCircleSprite("memory_gravel", Color.white, 72);
            for (int i = 0; i < 18; i++)
            {
                var x = -ArenaHalfWidth + 2.6f + (i % 6) * 8.8f + Mathf.Sin(i * 1.73f) * 0.7f;
                var y = -ArenaHalfHeight + 2.0f + (i / 6) * 9.0f + Mathf.Cos(i * 1.27f) * 0.8f;
                var scale = 0.06f + (i % 4) * 0.018f;
                var color = (i % 5) switch
                {
                    0 => new Color(0.44f, 0.62f, 0.62f, 0.20f),
                    1 => new Color(0.24f, 0.30f, 0.32f, 0.28f),
                    _ => new Color(0.18f, 0.20f, 0.20f, 0.24f)
                };
                CreateArenaSprite($"Memory_Gravel_{i:00}", new Vector3(x, y, 0.84f), Vector3.one * scale, Quaternion.identity, gravel, color, -82);
            }
        }

        void CreateLetheRiverAndRuins()
        {
            var river = MakeBoxSprite("lethe_river_band", Color.white, 192, 18);
            var bank = MakeBoxSprite("lethe_river_bank", Color.white, 192, 8);
            var ruin = MakeBoxSprite("sunken_ruin_slab", Color.white, 96, 14);
            for (int i = 0; i < 9; i++)
            {
                var x = -ArenaHalfWidth + 3.6f + i * 6.0f;
                var y = Mathf.Sin(i * 0.92f) * 2.2f - 1.3f;
                var angle = -8f + Mathf.Sin(i * 1.17f) * 18f;
                var len = 1.15f + (i % 3) * 0.20f;
                CreateArenaSprite($"Lethe_River_Band_{i:00}", new Vector3(x, y, 0.805f), new Vector3(len, 1f, 1f), Quaternion.Euler(0f, 0f, angle), river, new Color(0.06f, 0.30f, 0.34f, 0.18f), -88);
                CreateArenaSprite($"Lethe_River_Bank_A_{i:00}", new Vector3(x, y + 0.42f, 0.815f), new Vector3(len * 0.92f, 1f, 1f), Quaternion.Euler(0f, 0f, angle + 2f), bank, new Color(0.03f, 0.08f, 0.09f, 0.28f), -87);
                CreateArenaSprite($"Lethe_River_Bank_B_{i:00}", new Vector3(x, y - 0.44f, 0.815f), new Vector3(len * 0.88f, 1f, 1f), Quaternion.Euler(0f, 0f, angle - 2f), bank, new Color(0.04f, 0.10f, 0.10f, 0.24f), -87);
            }

            for (int i = 0; i < 10; i++)
            {
                var x = -ArenaHalfWidth + 5.2f + (i % 5) * 9.5f + Mathf.Sin(i * 1.31f) * 0.9f;
                var y = -ArenaHalfHeight + 4.6f + (i / 5) * 19.4f + Mathf.Cos(i * 1.71f) * 1.1f;
                var angle = i * 23f + Mathf.Sin(i * 0.7f) * 25f;
                CreateArenaSprite($"Sunken_Ruin_Slab_{i:00}", new Vector3(x, y, 0.825f), new Vector3(0.55f + (i % 3) * 0.18f, 1f, 1f), Quaternion.Euler(0f, 0f, angle), ruin, new Color(0.18f, 0.22f, 0.23f, 0.30f), -82);
            }
        }

        void CreateArenaLandmarks()
        {
            var ring = MakeRingSprite("terrain_memory_ring", Color.white, 180);
            var shard = MakeImpactDiamondSprite("terrain_memory_shard", Color.white);
            var pillar = MakeBoxSprite("terrain_lethe_pillar", Color.white, 16, 96);
            var positions = new[]
            {
                new Vector3(-15.8f, 9.4f, 0.86f),
                new Vector3(14.6f, 8.2f, 0.86f),
                new Vector3(-17.2f, -8.6f, 0.86f),
                new Vector3(12.0f, -10.4f, 0.86f),
                new Vector3(-2.6f, 4.6f, 0.86f)
            };

            for (int i = 0; i < positions.Length; i++)
            {
                var p = positions[i];
                var color = i % 2 == 0
                    ? new Color(0.20f, 0.58f, 0.62f, 0.28f)
                    : new Color(0.48f, 0.42f, 0.62f, 0.24f);
                CreateArenaSprite($"Memory_Landmark_Ring_{i:00}", p, Vector3.one * (0.92f + i * 0.05f), Quaternion.Euler(0f, 0f, i * 37f), ring, color, -81);
                CreateArenaSprite($"Memory_Landmark_Pillar_{i:00}", p + Vector3.up * 0.03f, new Vector3(0.26f, 0.82f + i * 0.06f, 1f), Quaternion.Euler(0f, 0f, 8f + i * 33f), pillar, new Color(0.20f, 0.28f, 0.30f, 0.32f), -80);
                CreateArenaSprite($"Memory_Landmark_Shard_{i:00}", p + Vector3.up * 0.10f, Vector3.one * (0.17f + i * 0.014f), Quaternion.Euler(0f, 0f, i * 41f), shard, new Color(0.58f, 0.94f, 0.96f, 0.38f), -79);
            }
        }

        void CreateArenaSprite(string name, Vector3 position, Vector3 scale, Quaternion rotation, Sprite sprite, Color color, int sortingOrder)
        {
            var go = new GameObject(name);
            go.transform.position = position;
            go.transform.rotation = rotation;
            go.transform.localScale = scale;
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.color = color;
            sr.sortingOrder = sortingOrder;
        }

        void CreatePlayer()
        {
            var go = new GameObject("Player_V1");
            player = go.transform;
            player.position = Vector3.zero;

            var visual = new GameObject("PlayerVisual").transform;
            visual.SetParent(player);
            visual.localPosition = Vector3.zero;
            visual.localRotation = Quaternion.identity;
            visual.localScale = Vector3.one;
            playerVisual = visual;
            playerSprite = visual.gameObject.AddComponent<SpriteRenderer>();
            playerSprite.sprite = LoadSheetFrame(PlayerSheetPath, 4, 8, 0, 0) ?? MakeCircleSprite("player", new Color(0.78f, 0.88f, 1f), 96);
            playerSprite.sortingOrder = 20;

            weaponAnchor = new GameObject("WeaponAnchor").transform;
            weaponAnchor.SetParent(player);
            weaponAnchor.localPosition = new Vector3(0f, -0.06f, 0f);

            dualLeftWeaponSprite = LoadSprite("Assets/_dev/Art/Sprites/Weapons/spr_weapon_dual_blade_left_01.png") ?? MakeBladeSprite("dual-left", new Color(0.80f, 0.96f, 1f), new Color(0.16f, 0.26f, 0.34f), 34, 112, false);
            dualRightWeaponSprite = LoadSprite("Assets/_dev/Art/Sprites/Weapons/spr_weapon_dual_blade_right_01.png") ?? MakeBladeSprite("dual-right", new Color(0.88f, 0.98f, 1f), new Color(0.18f, 0.30f, 0.38f), 34, 112, false);
            greatswordWeaponSprite = LoadSprite("Assets/_dev/Art/Sprites/Weapons/spr_weapon_greatsword_01.png") ?? MakeBladeSprite("greatsword-visual", new Color(0.82f, 0.96f, 1f), new Color(0.10f, 0.18f, 0.24f), 58, 178, true);
            leftBladeSprite = CreateWeaponSprite("LeftBlade", dualLeftWeaponSprite, new Vector3(-0.19f, -0.04f, 0f));
            rightBladeSprite = CreateWeaponSprite("RightBlade", dualRightWeaponSprite, new Vector3(0.19f, -0.04f, 0f));
        }

        SpriteRenderer CreateWeaponSprite(string name, Sprite sprite, Vector3 localPos)
        {
            var go = new GameObject(name);
            go.transform.SetParent(weaponAnchor);
            go.transform.localPosition = localPos;
            go.transform.localScale = Vector3.one * DualBladeVisualScale;
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.sortingOrder = 30;
            sr.enabled = false;
            sr.transform.localRotation = Quaternion.Euler(0f, 0f, name.Contains("Left") ? 12f : -12f);
            return sr;
        }

        void UpdatePlayer(float dt)
        {
            var move = MoveInput();
            if (move.sqrMagnitude > 1f) move.Normalize();
            var targetVelocity = move * PlayerSpeed;
            var response = move.sqrMagnitude > 0.01f ? PlayerMoveAcceleration : PlayerMoveDeceleration;
            playerMoveVelocity = Vector2.Lerp(playerMoveVelocity, targetVelocity, 1f - Mathf.Exp(-response * dt));
            if (move.sqrMagnitude <= 0.01f && playerMoveVelocity.sqrMagnitude < 0.0005f)
            {
                playerMoveVelocity = Vector2.zero;
            }

            player.position += (Vector3)(playerMoveVelocity * dt);
            player.position = new Vector3(Mathf.Clamp(player.position.x, -ArenaHalfWidth, ArenaHalfWidth), Mathf.Clamp(player.position.y, -ArenaHalfHeight, ArenaHalfHeight), 0f);

            var visualMove = playerMoveVelocity.sqrMagnitude > 0.003f ? playerMoveVelocity.normalized : move;
            if (visualMove.sqrMagnitude > 0.01f)
            {
                var angle = Mathf.Atan2(visualMove.y, visualMove.x) * Mathf.Rad2Deg;
                var current = weaponAnchor.eulerAngles.z;
                weaponAnchor.rotation = Quaternion.Euler(0f, 0f, Mathf.LerpAngle(current, angle - 90f, 1f - Mathf.Exp(-16f * dt)));
                lastAim = visualMove.normalized;
            }
            UpdatePlayerSprite(visualMove, dt);

            foreach (var enemy in enemies.ToList())
            {
                if (enemy == null || !enemy.IsAlive) continue;
                var dist = Vector2.Distance(player.position, enemy.transform.position);
                if (dist < enemy.TouchRadius + 0.35f)
                {
                    var damage = enemy.TouchDamage * EarlyDamageMul() * dt;
                    playerHp -= damage;
                }
            }
        }

        void UpdateCamera()
        {
            var halfViewHeight = mainCamera.orthographicSize;
            var halfViewWidth = halfViewHeight * mainCamera.aspect;
            var targetX = Mathf.Clamp(player.position.x, -ArenaHalfWidth + halfViewWidth, ArenaHalfWidth - halfViewWidth);
            var targetY = Mathf.Clamp(player.position.y, -ArenaHalfHeight + halfViewHeight, ArenaHalfHeight - halfViewHeight);
            var target = new Vector3(targetX, targetY, -10f);
            if (cameraShakeTimer > 0f)
            {
                cameraShakeTimer -= Time.deltaTime;
                target += (Vector3)(UnityEngine.Random.insideUnitCircle * cameraShakeAmount);
                cameraShakeAmount = Mathf.Lerp(cameraShakeAmount, 0f, Time.deltaTime * 12f);
            }
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, target, 0.15f);
        }

        void UpdateWeaponVisuals(float dt)
        {
            if (weaponAnimTimer > 0f)
            {
                weaponAnimTimer -= dt;
            }

            if (leftBladeSprite != null) leftBladeSprite.enabled = false;
            if (rightBladeSprite != null) rightBladeSprite.enabled = false;
        }

        void UpdateWeapon(float dt)
        {
            weaponTimer -= dt;
            if (weaponTimer > 0f) return;

            var weapon = CurrentWeaponSpec();
            var target = FindWeaponTarget(weapon);
            if (target == null)
            {
                weaponTimer = 0f;
                return;
            }

            var forward = (Vector2)(target.transform.position - player.position);
            if (forward.sqrMagnitude <= 0.001f) forward = lastAim;
            forward.Normalize();
            lastAim = forward;
            weaponAnchor.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg - 90f);
            weaponTimer = weapon.Interval * StatAttackIntervalMul();
            weaponAnimTimer = weapon.SwingAnimDuration;
            leftBladeLead = !leftBladeLead;
            var hits = CollectWeaponHits(weapon, forward);
            SpawnWeaponHitVfx(weapon, hits, forward);

            var hitIndex = 0;
            foreach (var hit in hits)
            {
                var damageMul = hitIndex == 0 ? 1f : weapon.SecondaryDamageMul;
                var knock = hitIndex == 0 ? weapon.PrimaryKnock : weapon.SecondaryKnock;
                DealDamage(hit.Enemy, weapon.Damage * damageMul, weapon.DisplayName, true, hit.Dir.normalized, knock);
                if (weapon.EchoProcStyle != V1EchoProcStyle.SingleHeavy || hitIndex == 0)
                {
                    if (ShouldTriggerWeaponEchoForHit(weapon, hitIndex))
                    {
                        TriggerWeaponEchoes(hit.Enemy, forward, hitIndex, weapon);
                    }
                    else
                    {
                        debugDenseDualBladeEchoesSuppressed++;
                    }
                }
                hitIndex++;
            }

            if (hits.Count > 0)
            {
                hitstopTimer = weapon.Hitstop;
                cameraShakeTimer = 0.06f;
                cameraShakeAmount = weapon.ShakeAmount;
            }
        }

        V1Enemy FindWeaponTarget(WeaponRuntimeSpec weapon)
        {
            var range = weapon.Range * (1f + WeaponStat.AreaMul);
            var engageRadius = range * weapon.EngageMul;
            var candidates = enemies
                .Where(e => e != null && e.IsAlive)
                .Select(e => new { Enemy = e, Dir = (Vector2)(e.transform.position - player.position) })
                .Where(x => x.Dir.magnitude <= engageRadius + x.Enemy.TouchRadius)
                .ToList();

            if (weapon.TargetingMode == V1WeaponTargetingMode.Nearest)
            {
                return candidates
                    .OrderBy(x => x.Dir.sqrMagnitude)
                    .Select(x => x.Enemy)
                    .FirstOrDefault();
            }

            return candidates
                .Select(x =>
                {
                    var forward = x.Dir.sqrMagnitude > 0.001f ? x.Dir.normalized : lastAim.normalized;
                    var hits = CollectWeaponHits(weapon, forward);
                    var center = hits.Count == 0 ? x.Enemy.transform.position : (Vector3)hits.Aggregate(Vector2.zero, (sum, h) => sum + (Vector2)h.Enemy.transform.position) / hits.Count;
                    return new { x.Enemy, x.Dir, HitCount = hits.Count, CenterDistance = Vector2.Distance(player.position, center) };
                })
                .OrderByDescending(x => x.HitCount)
                .ThenBy(x => x.CenterDistance)
                .ThenBy(x => x.Dir.sqrMagnitude)
                .Select(x => x.Enemy)
                .FirstOrDefault();
        }

        int CountLiveEnemies() => enemies.Count(e => e != null && e.IsAlive);

        bool DenseDualBladeVfxThrottle(WeaponRuntimeSpec weapon)
        {
            return weapon.Id == V1WeaponId.DualBlades && (debugForceDenseDualBladeThrottle || CountLiveEnemies() >= DenseDualBladeThrottleEnemyCount);
        }

        bool ShouldTriggerWeaponEchoForHit(WeaponRuntimeSpec weapon, int hitIndex)
        {
            return !DenseDualBladeVfxThrottle(weapon) || hitIndex < DenseDualBladeEchoHitLimit;
        }

        bool ShouldSpawnWeaponSlashForHit(WeaponRuntimeSpec weapon, int hitIndex)
        {
            return !DenseDualBladeVfxThrottle(weapon) || hitIndex < DenseDualBladeSlashHitLimit;
        }

        List<WeaponHit> CollectWeaponHits(WeaponRuntimeSpec weapon, Vector2 forward)
        {
            var f = forward.sqrMagnitude > 0.001f ? forward.normalized : lastAim.normalized;
            var range = weapon.Range * (1f + WeaponStat.AreaMul);
            return enemies
                .Where(e => e != null && e.IsAlive)
                .Select(e => new WeaponHit(e, (Vector2)(e.transform.position - player.position)))
                .Where(x => x.Distance <= range + x.Enemy.TouchRadius)
                .Where(x => x.Distance <= 0.001f || Vector2.Angle(f, x.Dir.normalized) <= weapon.ArcDegrees * 0.5f)
                .OrderBy(x => x.Distance)
                .Take(weapon.MaxTargets)
                .ToList();
        }

        void UpdateActiveMemories(float dt)
        {
            var hungry = activeMemories.FirstOrDefault(m => m.Id == V1MemoryId.HungryBlades);
            if (hungry != null)
            {
                UpdateHungryBlades(hungry, dt);
            }

            var blood = activeMemories.FirstOrDefault(m => m.Id == V1MemoryId.BloodReflection);
            if (blood != null)
            {
                UpdateBloodReflectionAction(blood, dt);
                foreach (var enemy in enemies.Where(e => e != null && e.IsAlive && e.BloodMarked).ToList())
                {
                    DealDamage(enemy, (0.8f + blood.Level * 0.25f) * dt, "피의 반사", false);
                }
            }

            var execution = activeMemories.FirstOrDefault(m => m.Id == V1MemoryId.ExecutionFlash);
            if (execution != null) UpdateExecutionFlash(execution, dt);

            var hunter = activeMemories.FirstOrDefault(m => m.Id == V1MemoryId.HunterOath);
            if (hunter != null) UpdateHunterOath(hunter, dt);

            var shatter = activeMemories.FirstOrDefault(m => m.Id == V1MemoryId.ShatterWave);
            if (shatter != null) UpdateShatterWave(shatter, dt);

            var stopped = activeMemories.FirstOrDefault(m => m.Id == V1MemoryId.StoppedSecond);
            if (stopped != null) UpdateStoppedSecond(stopped, dt);

            var ash = activeMemories.FirstOrDefault(m => m.Id == V1MemoryId.AshenShield);
            if (ash != null) UpdateAshenShield(ash, dt);

            var oblivion = activeMemories.FirstOrDefault(m => m.Id == V1MemoryId.OblivionBrand);
            if (oblivion != null) UpdateOblivionBrand(oblivion, dt);
        }

        void UpdateBloodReflectionAction(MemoryState memory, float dt)
        {
            memory.TickTimer -= dt;
            if (memory.TickTimer > 0f) return;
            memory.TickTimer = Mathf.Max(0.62f, 1.52f - memory.Level * 0.14f);

            var anchor = enemies
                .Where(e => e != null && e.IsAlive)
                .OrderByDescending(e => e.BloodMarked ? 1 : 0)
                .ThenBy(e => Vector2.Distance(player.position, e.transform.position))
                .FirstOrDefault();
            var center = anchor != null ? anchor.transform.position : player.position;
            var radius = 1.25f + memory.Level * 0.16f;
            var cap = memory.Level >= 5 ? 7 : memory.Level >= 3 ? 4 : 2;

            PlaySfx("blood_mark", 0.42f, 0.16f);
            SpawnTransientSprite("MemoryBloodReflectionPulse", MakeRingSprite("MemoryBloodReflectionPulse", Color.white, 132), center, Quaternion.identity, 0.74f + memory.Level * 0.065f, new Color(1f, 0.10f, 0.18f, 0.54f), 0.42f);
            SpawnPromptSprite("MemoryBloodReflectionBloom", MemoryVfxSprite(V1MemoryId.BloodReflection), () => MakeRingSprite("MemoryBloodReflectionBloom", Color.white, 128), center + Vector3.up * 0.05f, Quaternion.Euler(0f, 0f, elapsed * -82f), 1.42f + memory.Level * 0.14f, 0.64f, new Color(1f, 0.16f, 0.24f, 0.70f), 0.56f);

            var victims = enemies
                .Where(e => e != null && e.IsAlive && Vector2.Distance(center, e.transform.position) <= radius + e.TouchRadius)
                .OrderBy(e => Vector2.Distance(center, e.transform.position))
                .Take(cap)
                .ToList();
            if (victims.Count == 0)
            {
                HealPlayer(0.35f + memory.Level * 0.08f);
                return;
            }

            foreach (var target in victims)
            {
                target.BloodMarked = true;
                target.MarkTimer = Mathf.Max(target.MarkTimer, 2.0f + memory.Level * 0.18f);
                var dir = (Vector2)(target.transform.position - center);
                DealDamage(target, 4.8f + memory.Level * 2.1f, "BloodReflection active pulse", false, dir.sqrMagnitude > 0.01f ? dir.normalized : Vector2.up, memory.Level >= 3 ? 0.18f : 0.04f);
                SpawnBloodThread(target.transform.position, 0.45f + memory.Level * 0.10f, memory.Level);
                if (memory.Level >= 3)
                {
                    SpawnEchoLink("MemoryBloodReflectionThread", target.transform.position, player.position, new Color(1f, 0.12f, 0.18f, 0.38f), 0.30f, 0.018f + memory.Level * 0.002f);
                }
            }

            HealPlayer(0.55f + victims.Count * (0.12f + memory.Level * 0.035f));
            if (memory.Level >= 5 && anchor != null)
            {
                SpawnRadialSlashLines("MemoryBloodReflectionAwakenLash", anchor.transform.position, (Vector2)(anchor.transform.position - player.position), 7, 1.08f, new Color(1f, 0.12f, 0.18f, 0.68f), 0.46f);
                SpawnEchoLink("MemoryBloodReflectionAwakenDraw", anchor.transform.position, player.position, new Color(1f, 0.08f, 0.16f, 0.48f), 0.42f, 0.028f);
                BloodBloom(anchor, memory.Level);
            }
        }

        void UpdateExecutionFlash(MemoryState memory, float dt)
        {
            memory.TickTimer -= dt;
            if (memory.TickTimer > 0f) return;
            memory.TickTimer = Mathf.Max(0.24f, 0.68f - memory.Level * 0.06f);

            var threshold = 0.28f + memory.Level * 0.028f;
            var target = enemies
                .Where(e => e != null && e.IsAlive && e.HealthRatio <= threshold)
                .OrderBy(e => Vector2.Distance(player.position, e.transform.position))
                .FirstOrDefault();
            if (target == null)
            {
                target = enemies
                    .Where(e => e != null && e.IsAlive && e.HealthRatio <= threshold + 0.18f + memory.Level * 0.012f)
                    .OrderBy(e => e.HealthRatio)
                    .ThenBy(e => Vector2.Distance(player.position, e.transform.position))
                    .FirstOrDefault();
                if (target == null) return;

                SpawnExecutionForecast(target, memory.Level, false);
                MarkEnemyEchoState(target, V1MemoryId.ExecutionFlash, 0.78f, 0.88f);
                DealDamage(target, 7f + memory.Level * 2.2f, "Execution forecast", false);
                return;
            }

            PlaySfx("execution", 0.72f, 0.10f);
            SpawnPromptSprite("ExecutionFlash", MemoryVfxSprite(V1MemoryId.ExecutionFlash), () => MakeImpactDiamondSprite("ExecutionFlash", Color.white), target.transform.position, Quaternion.identity, 2.18f, 0.94f, new Color(1f, 0.95f, 0.62f, 0.98f), 0.46f);
            SpawnExecutionFlashBurst(target.transform.position, 1.14f, 0.42f);
            MarkEnemyEchoState(target, V1MemoryId.ExecutionFlash, 1.10f, 1.08f);
            DealDamage(target, 32f + memory.Level * 8.5f, "처형 섬광", false);
        }

        void UpdateHunterOath(MemoryState memory, float dt)
        {
            memory.TickTimer -= dt;
            if (memory.TickTimer > 0f) return;
            memory.TickTimer = Mathf.Max(0.52f, 1.10f - memory.Level * 0.11f);

            var targets = enemies
                .Where(e => e != null && e.IsAlive)
                .OrderByDescending(EnemyThreatPriority)
                .ThenBy(e => Vector2.Distance(player.position, e.transform.position))
                .Take(Mathf.Min(6, 2 + memory.Level / 2))
                .ToList();
            if (targets.Count == 0) return;

            var projectileCount = Mathf.Min(6, 2 + memory.Level / 2);
            var speed = 10.2f + memory.Level * 0.9f;
            var damage = 16f + memory.Level * 5.2f;
            PlaySfx("hunter", 0.54f, 0.08f);
            SpawnTransientSprite("HunterOathMuzzle", MakeRingSprite("HunterOathMuzzle", Color.white, 104), player.position, Quaternion.identity, 0.58f + projectileCount * 0.06f, new Color(0.80f, 1f, 0.42f, 0.66f), 0.30f);
            for (int i = 0; i < projectileCount; i++)
            {
                SpawnHunterOathShot(targets[i % targets.Count], player.position, i, projectileCount, speed, damage, HunterOathSource, false);
            }
        }

        void SpawnHunterOathShot(V1Enemy target, Vector3 origin, int volleyIndex, int volleyCount, float speed, float damage, string source, bool echo)
        {
            if (target == null || !target.IsAlive) return;
            if (echo) PlaySfx("hunter", 0.30f, 0.06f);
            var toTarget = (Vector2)(target.transform.position - origin);
            var forward = toTarget.sqrMagnitude > 0.01f ? toTarget.normalized : Vector2.up;
            var side = new Vector3(-forward.y, forward.x, 0f);
            var spreadIndex = volleyIndex - (volleyCount - 1) * 0.5f;
            var go = new GameObject(echo ? "HunterEchoShot" : "HunterOathShot");
            go.transform.position = origin + side * spreadIndex * 0.16f + (Vector3)forward * Mathf.Abs(spreadIndex) * 0.04f;
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = echo
                ? EchoVfxSprite(V1MemoryId.HunterOath) ?? MakeCrescentSlashSprite("HunterEchoShot", Color.white, false)
                : MemoryVfxSprite(V1MemoryId.HunterOath) ?? MakeCrescentSlashSprite("HunterOathShot", Color.white, false);
            var targetWidth = echo ? 1.02f : 0.92f;
            go.transform.localScale = Vector3.one * (sr.sprite != null && sr.sprite.bounds.size.x > 2f ? ScaleSpriteToWorldWidth(sr.sprite, targetWidth) : echo ? 0.185f : 0.17f);
            sr.color = echo ? new Color(0.88f, 1f, 0.46f, 0.96f) : new Color(0.84f, 1f, 0.34f, 0.96f);
            sr.sortingOrder = 45;
            SpawnTransientSprite(echo ? "HunterEchoTargetLock" : "HunterOathTargetLock", MakeRingSprite("HunterTargetLock", Color.white, 112), target.transform.position, Quaternion.identity, echo ? 0.62f : 0.54f, echo ? new Color(0.76f, 1f, 0.42f, 0.50f) : new Color(0.80f, 1f, 0.32f, 0.48f), echo ? 0.46f : 0.32f);
            go.AddComponent<V1Projectile>().Configure(this, target, speed, damage, source);
        }

        void UpdateShatterWave(MemoryState memory, float dt)
        {
            memory.TickTimer -= dt;
            if (memory.TickTimer > 0f) return;
            memory.TickTimer = Mathf.Max(0.62f, 1.95f - memory.Level * 0.17f);

            var target = enemies
                .Where(e => e != null && e.IsAlive)
                .Select(e => new { Enemy = e, Count = enemies.Count(o => o != null && o.IsAlive && Vector2.Distance(e.transform.position, o.transform.position) < 1.55f) })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => Vector2.Distance(player.position, x.Enemy.transform.position))
                .Select(x => x.Enemy)
                .FirstOrDefault();
            if (target == null) return;

            var radius = 1.22f + memory.Level * 0.17f;
            SpawnShatterWaveField(target.transform.position, radius, 1.20f, false);
            foreach (var enemy in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(target.transform.position, e.transform.position) <= radius + e.TouchRadius).Take(10).ToList())
            {
                var dir = (Vector2)(enemy.transform.position - target.transform.position);
                var clusterBonus = Mathf.Clamp01((enemies.Count(o => o != null && o.IsAlive && Vector2.Distance(enemy.transform.position, o.transform.position) < 1.35f) - 1) / 5f);
                var bossBonus = enemy.Kind == V1EnemyKind.Gatekeeper ? 1.55f : 1f;
                MarkEnemyEchoState(enemy, V1MemoryId.ShatterWave, 1.15f, 1.02f);
                if (enemy.Kind == V1EnemyKind.Gatekeeper || clusterBonus > 0.35f)
                {
                    SpawnShatterEchoScar(enemy.transform.position, dir.sqrMagnitude > 0.01f ? dir.normalized : Vector2.up, 0.72f + clusterBonus * 0.38f, 0.34f);
                    DealDamage(enemy, (4.0f + memory.Level * 1.6f) * (clusterBonus + bossBonus - 1f), "ShatterWave fracture bonus", false, dir.sqrMagnitude > 0.01f ? dir.normalized : Vector2.up, 0.22f);
                }
                DealDamage(enemy, 10f + memory.Level * 4.0f, "파쇄의 파문", false, dir.sqrMagnitude > 0.01f ? dir.normalized : Vector2.up, 0.62f);
            }
        }

        void SpawnShatterWaveField(Vector3 center, float radius, float lifetime, bool echo)
        {
            PlaySfx("shatter", echo ? 0.34f : 0.58f, echo ? 0.12f : 0.18f);
            var mainColor = echo ? new Color(0.74f, 0.96f, 1f, 0.66f) : new Color(0.86f, 0.98f, 1f, 0.66f);
            var outerColor = echo ? new Color(0.42f, 0.86f, 1f, 0.44f) : new Color(0.42f, 0.82f, 1f, 0.44f);
            var innerColor = echo ? new Color(0.78f, 1f, 1f, 0.36f) : new Color(0.90f, 1f, 1f, 0.38f);
            var sprite = echo ? EchoVfxSprite(V1MemoryId.ShatterWave) : MemoryVfxSprite(V1MemoryId.ShatterWave);
            var name = echo ? "ShatterEchoField" : "ShatterWaveField";
            SpawnPromptSprite(name, sprite, () => MakeRingSprite(name, Color.white, 144), center, Quaternion.Euler(0f, 0f, elapsed * (echo ? -80f : 60f)), radius * (echo ? 1.66f : 1.86f), radius * (echo ? 0.82f : 0.94f), mainColor, lifetime);
            SpawnTransientSprite(echo ? "ShatterEchoOuterHold" : "ShatterWaveOuterHold", MakeRingSprite("ShatterWaveOuterHold", Color.white, 168), center, Quaternion.identity, radius * (echo ? 0.92f : 1.06f), outerColor, lifetime * 0.94f);
            SpawnTransientSprite(echo ? "ShatterEchoInnerHold" : "ShatterWaveInnerHold", MakeRingSprite("ShatterWaveInnerHold", Color.white, 112), center, Quaternion.identity, radius * (echo ? 0.52f : 0.62f), innerColor, lifetime * 0.86f);

            var spoke = MakeBoxSprite("ShatterWaveSpoke", Color.white, 7, 92);
            var spokeCount = echo ? 6 : 8;
            for (int i = 0; i < spokeCount; i++)
            {
                var angle = i * (360f / spokeCount) + elapsed * (echo ? 24f : -18f);
                var dir = Quaternion.Euler(0f, 0f, angle) * Vector3.up;
                var pos = center + dir * radius * (echo ? 0.42f : 0.50f);
                SpawnTransientSpriteScaled(echo ? "ShatterEchoSpoke" : "ShatterWaveSpoke", spoke, pos, Quaternion.Euler(0f, 0f, angle), new Vector3(echo ? 0.028f : 0.032f, radius * (echo ? 0.38f : 0.44f), 1f), new Color(0.78f, 0.98f, 1f, echo ? 0.36f : 0.36f), lifetime * 0.82f);
            }
        }

        void SpawnEchoWoundSlash(string name, Vector3 center, Vector2 forward, Color color, float length, float lifetime)
        {
            var f = forward.sqrMagnitude > 0.01f ? forward.normalized : lastAim.normalized;
            if (f.sqrMagnitude < 0.01f) f = Vector2.up;
            var angle = Mathf.Atan2(f.y, f.x) * Mathf.Rad2Deg - 90f;
            var line = MakeBoxSprite(name, Color.white, 7, 118);
            SpawnTransientSpriteScaled(name, line, center, Quaternion.Euler(0f, 0f, angle), new Vector3(0.026f, length, 1f), color, lifetime);
            SpawnTransientSpriteScaled($"{name}_cross", line, center, Quaternion.Euler(0f, 0f, angle + 72f), new Vector3(0.018f, length * 0.46f, 1f), new Color(color.r, color.g, color.b, color.a * 0.68f), lifetime * 0.82f);
        }

        void SpawnEchoWoundLine(string name, Vector3 center, Vector2 forward, Color color, float length, float lifetime)
        {
            var f = forward.sqrMagnitude > 0.01f ? forward.normalized : lastAim.normalized;
            if (f.sqrMagnitude < 0.01f) f = Vector2.up;
            var angle = Mathf.Atan2(f.y, f.x) * Mathf.Rad2Deg - 90f;
            var line = MakeBoxSprite(name, Color.white, 7, 118);
            SpawnTransientSpriteScaled(name, line, center, Quaternion.Euler(0f, 0f, angle), new Vector3(0.022f, length, 1f), color, lifetime);
        }

        void SpawnShatterEchoScar(Vector3 center, Vector2 forward, float radius, float lifetime)
        {
            var f = forward.sqrMagnitude > 0.01f ? forward.normalized : lastAim.normalized;
            if (f.sqrMagnitude < 0.01f) f = Vector2.up;
            var side = new Vector2(-f.y, f.x);
            var angle = Mathf.Atan2(f.y, f.x) * Mathf.Rad2Deg - 90f;
            var line = MakeBoxSprite("ShatterEchoScar", Color.white, 7, 132);
            var color = new Color(0.74f, 0.98f, 1f, 0.60f);
            SpawnTransientSpriteScaled("ShatterEchoScar_Main", line, center, Quaternion.Euler(0f, 0f, angle), new Vector3(0.030f, radius * 0.72f, 1f), color, lifetime);
            SpawnTransientSpriteScaled("ShatterEchoScar_Left", line, center + (Vector3)(side * radius * 0.18f), Quaternion.Euler(0f, 0f, angle - 26f), new Vector3(0.020f, radius * 0.46f, 1f), new Color(color.r, color.g, color.b, 0.44f), lifetime * 0.86f);
            SpawnTransientSpriteScaled("ShatterEchoScar_Right", line, center - (Vector3)(side * radius * 0.16f), Quaternion.Euler(0f, 0f, angle + 30f), new Vector3(0.020f, radius * 0.42f, 1f), new Color(color.r, color.g, color.b, 0.40f), lifetime * 0.82f);
        }

        void SpawnEchoLink(string name, Vector3 from, Vector3 to, Color color, float lifetime, float thickness)
        {
            var delta = to - from;
            var distance = delta.magnitude;
            if (distance < 0.05f) return;
            var mid = (from + to) * 0.5f;
            var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg - 90f;
            var line = MakeBoxSprite(name, Color.white, 5, 128);
            SpawnTransientSpriteScaled(name, line, mid, Quaternion.Euler(0f, 0f, angle), new Vector3(thickness, distance * 0.50f, 1f), color, lifetime);
        }

        void SpawnStoppedEchoClamp(Vector3 center, float radius, float lifetime)
        {
            var color = TimeStopGold(false);
            SpawnTransientSprite("StoppedEchoClampRing", MakeRingSprite("StoppedEchoClampRing", Color.white, 132), center, Quaternion.Euler(0f, 0f, elapsed * -110f), radius, new Color(color.r, color.g, color.b, 0.72f), lifetime);
            SpawnTransientSprite("StoppedEchoClampCore", MakeImpactDiamondSprite("StoppedEchoClampCore", Color.white), center, Quaternion.Euler(0f, 0f, elapsed * 160f), 0.24f, new Color(1f, 0.86f, 0.28f, 0.72f), lifetime * 0.62f);
            var tick = MakeBoxSprite("StoppedEchoClampTick", Color.white, 7, 48);
            for (int i = 0; i < 8; i++)
            {
                var angle = i * 45f + elapsed * 18f;
                var dir = Quaternion.Euler(0f, 0f, angle) * Vector3.up;
                SpawnTransientSpriteScaled("StoppedEchoClampTick", tick, center + dir * radius * 0.72f, Quaternion.Euler(0f, 0f, angle), new Vector3(0.024f, 0.22f, 1f), new Color(color.r, color.g, color.b, 0.62f), lifetime * 0.88f);
            }
        }

        void SpawnOblivionEchoBrand(Vector3 center, Vector2 forward, float lifetime)
        {
            var f = forward.sqrMagnitude > 0.01f ? forward.normalized : lastAim.normalized;
            if (f.sqrMagnitude < 0.01f) f = Vector2.up;
            var angle = Mathf.Atan2(f.y, f.x) * Mathf.Rad2Deg - 90f;
            var line = MakeBoxSprite("OblivionEchoBrandLine", Color.white, 8, 112);
            var color = new Color(0.86f, 0.48f, 1f, 0.76f);
            SpawnTransientSprite("OblivionEchoBrandSeal", MakeRingSprite("OblivionEchoBrandSeal", Color.white, 128), center, Quaternion.Euler(0f, 0f, elapsed * 135f), 0.46f, new Color(0.72f, 0.34f, 1f, 0.50f), lifetime);
            SpawnTransientSpriteScaled("OblivionEchoBrandLineA", line, center, Quaternion.Euler(0f, 0f, angle), new Vector3(0.024f, 0.62f, 1f), color, lifetime);
            SpawnTransientSpriteScaled("OblivionEchoBrandLineB", line, center, Quaternion.Euler(0f, 0f, angle + 58f), new Vector3(0.020f, 0.46f, 1f), new Color(color.r, color.g, color.b, 0.62f), lifetime * 0.88f);
        }

        void UpdateStoppedSecond(MemoryState memory, float dt)
        {
            memory.TickTimer -= dt;
            if (memory.TickTimer > 0f) return;
            memory.TickTimer = Mathf.Max(1.05f, 3.55f - memory.Level * 0.30f);

            var focus = enemies
                .Where(e => e != null && e.IsAlive)
                .OrderBy(e => Vector2.Distance(player.position, e.transform.position))
                .FirstOrDefault();
            var center = focus != null ? focus.transform.position : player.position;
            var radius = 1.95f + memory.Level * 0.24f;
            var freezeDuration = Mathf.Min(1.1f, 0.58f + memory.Level * 0.10f);
            PlaySfx("stopped", 0.62f, 0.20f);
            SpawnStoppedSecondField(center, radius, TimeStopGold(true), 1.75f, true);
            SpawnTransientSprite("MemoryStoppedSecondBeat", MakeRingSprite("MemoryStoppedSecondBeat", Color.white, 160), center, Quaternion.Euler(0f, 0f, elapsed * -110f), radius * 0.82f, new Color(1f, 0.78f, 0.24f, 0.44f), 0.52f);
            if (memory.Level >= 3)
            {
                SpawnRadialSlashLines("MemoryStoppedSecondAftercut", center, (Vector2)(center - player.position), 6, 0.80f + memory.Level * 0.09f, new Color(1f, 0.82f, 0.32f, 0.62f), 0.42f);
            }
            foreach (var enemy in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(center, e.transform.position) <= radius + e.TouchRadius).Take(10).ToList())
            {
                var dir = (Vector2)(enemy.transform.position - center);
                MarkEnemyEchoState(enemy, V1MemoryId.StoppedSecond, 1.40f, 1.04f);
                var f = dir.sqrMagnitude > 0.01f ? dir.normalized : Vector2.up;
                DealDamage(enemy, 6f + memory.Level * 2.3f, "StoppedSecond pulse", false, f, 0.75f);
                enemy.ApplyBriefFreeze(freezeDuration);
                if (memory.Level >= 3)
                {
                    SpawnStoppedFractureBurst(enemy.transform.position, f, 0.88f + memory.Level * 0.06f, memory.Level >= 5);
                    DealDamage(enemy, 3.0f + memory.Level * 1.8f, "StoppedSecond fracture", false, f, 0.12f);
                }
            }
            if (memory.Level >= 5)
            {
                SpawnTransientSprite("MemoryStoppedSecondAwakenDome", MakeRingSprite("MemoryStoppedSecondAwakenDome", Color.white, 180), center, Quaternion.identity, radius * 1.22f, new Color(1f, 0.72f, 0.18f, 0.40f), 0.76f);
                foreach (var extra in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(center, e.transform.position) <= radius * 1.35f + e.TouchRadius).Take(16).ToList())
                {
                    extra.ApplyBriefFreeze(0.42f);
                }
            }
        }

        void UpdateAshenShield(MemoryState memory, float dt)
        {
            memory.VisualTimer -= dt;
            if (memory.VisualTimer > 0f) return;
            memory.VisualTimer = Mathf.Max(0.82f, 1.28f - memory.Level * 0.075f);
            PlaySfx("ashen", 0.36f, 0.42f);
            SpawnPromptSprite("AshenShield", MemoryVfxSprite(V1MemoryId.AshenShield), () => MakeRingSprite("AshenShield", Color.white, 128), player.position, Quaternion.identity, 1.78f + memory.Level * 0.12f, 0.76f + memory.Level * 0.056f, new Color(0.72f, 0.80f, 0.86f, 0.54f), 0.56f);
            SpawnTransientSprite("AshenShieldCore", MakeRingSprite("AshenShieldCore", Color.white, 96), player.position, Quaternion.Euler(0f, 0f, elapsed * -96f), 0.46f + memory.Level * 0.040f, new Color(0.90f, 0.96f, 1f, 0.36f), 0.34f);
            if (memory.Level >= 3)
            {
                SpawnRadialSlashLines("MemoryAshenShieldCounterLine", player.position, lastAim, 5, 0.72f + memory.Level * 0.065f, new Color(0.84f, 0.92f, 1f, 0.54f), 0.38f);
                var release = ConsumeAshenGuardCharge(0.42f, memory.Level >= 5 ? 5f : 2.5f);
                if (release > 0f)
                {
                    ReleaseAshenGuardWave(player.position, memory.Level, release, memory.Level >= 5, "Ashen stored counter");
                }
                foreach (var enemy in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(player.position, e.transform.position) <= 1.75f + memory.Level * 0.12f).Take(7).ToList())
                {
                    var dir = (Vector2)(enemy.transform.position - player.position);
                    MarkEnemyEchoState(enemy, V1MemoryId.AshenShield, 1.05f, 0.98f);
                    DealDamage(enemy, 4.0f + memory.Level * 1.45f, "AshenShield counter", false, dir.sqrMagnitude > 0.01f ? dir.normalized : Vector2.up, 0.40f);
                }
            }
            if (memory.Level >= 5)
            {
                SpawnTransientSprite("MemoryAshenShieldAwakenWave", MakeRingSprite("MemoryAshenShieldAwakenWave", Color.white, 180), player.position, Quaternion.identity, 1.85f, new Color(0.88f, 0.96f, 1f, 0.48f), 0.66f);
                AddAshenGuardCharge(1.4f, memory.Level, player.position, true);
                HealPlayer(0.75f);
            }
        }

        void UpdateOblivionBrand(MemoryState memory, float dt)
        {
            memory.TickTimer -= dt;
            if (memory.TickTimer > 0f) return;
            memory.TickTimer = Mathf.Max(0.72f, 2.25f - memory.Level * 0.20f);

            var brandCount = memory.Level >= 5 ? 4 : 1 + memory.Level / 2;
            foreach (var enemy in enemies.Where(e => e != null && e.IsAlive).OrderBy(_ => UnityEngine.Random.value).Take(brandCount).ToList())
            {
                PlaySfx("brand", 0.46f, 0.12f);
                MarkEnemyEchoState(enemy, V1MemoryId.OblivionBrand, 1.35f, 1.02f);
                SpawnPromptSprite("OblivionBrand", MemoryVfxSprite(V1MemoryId.OblivionBrand), () => MakeRingSprite("OblivionBrand", Color.white, 96), enemy.transform.position + Vector3.up * 0.10f, Quaternion.identity, 1.34f, 0.60f, new Color(0.70f, 0.42f, 1f, 0.78f), 0.54f);
                SpawnTransientSprite("OblivionBrandSlash", MakeImpactDiamondSprite("OblivionBrandSlash", Color.white), enemy.transform.position + Vector3.up * 0.10f, Quaternion.Euler(0f, 0f, elapsed * 140f), 0.32f, new Color(0.92f, 0.68f, 1f, 0.78f), 0.28f);
                SpawnEchoLink("MemoryOblivionBrandTether", enemy.transform.position, player.position, new Color(0.78f, 0.34f, 1f, 0.48f), 0.36f, 0.022f);
                if (memory.Level >= 3)
                {
                    SpawnRadialSlashLines("MemoryOblivionBrandFork", enemy.transform.position, (Vector2)(enemy.transform.position - player.position), 4, 0.74f + memory.Level * 0.055f, new Color(0.84f, 0.42f, 1f, 0.62f), 0.38f);
                    foreach (var linked in enemies.Where(e => e != null && e.IsAlive && e != enemy && Vector2.Distance(enemy.transform.position, e.transform.position) <= 1.38f + memory.Level * 0.13f).Take(4).ToList())
                    {
                        SpawnEchoLink("MemoryOblivionBrandForkLink", enemy.transform.position, linked.transform.position, new Color(0.76f, 0.30f, 1f, 0.42f), 0.28f, 0.016f);
                        DealDamage(linked, 3.2f + memory.Level * 1.25f, "OblivionBrand fork", false);
                    }
                }
                if (memory.Level >= 5)
                {
                    SpawnTransientSprite("MemoryOblivionBrandAwakenSeal", MakeRingSprite("MemoryOblivionBrandAwakenSeal", Color.white, 180), enemy.transform.position, Quaternion.Euler(0f, 0f, elapsed * 120f), 0.92f, new Color(0.70f, 0.22f, 1f, 0.46f), 0.62f);
                    SpawnRadialSlashLines("MemoryOblivionBrandDetonation", enemy.transform.position, (Vector2)(enemy.transform.position - player.position), 6, 0.86f, new Color(0.92f, 0.46f, 1f, 0.68f), 0.42f);
                    foreach (var linked in enemies.Where(e => e != null && e.IsAlive && e != enemy && Vector2.Distance(enemy.transform.position, e.transform.position) <= 1.72f + memory.Level * 0.12f).Take(5).ToList())
                    {
                        var dir = (Vector2)(linked.transform.position - enemy.transform.position);
                        MarkEnemyEchoState(linked, V1MemoryId.OblivionBrand, 1.25f, 1.02f);
                        DealDamage(linked, 6.0f + memory.Level * 2.0f, "OblivionBrand detonation", false, dir.sqrMagnitude > 0.01f ? dir.normalized : Vector2.up, 0.18f);
                    }
                }
                DealDamage(enemy, 7f + memory.Level * 3.0f, "OblivionBrand mark", false);
            }
        }

        void UpdateHungryBladesVisualDuringHitstop(float dt)
        {
            var hungry = activeMemories.FirstOrDefault(m => m.Id == V1MemoryId.HungryBlades);
            if (hungry != null)
            {
                UpdateHungryBladesOrbitVisual(hungry, dt);
            }
        }

        void UpdateHungryBlades(MemoryState memory, float dt)
        {
            UpdateHungryBladesOrbitVisual(memory, dt);

            memory.TickTimer -= dt;
            if (memory.TickTimer > 0f) return;
            memory.TickTimer = 0.28f;

            var targetLimit = memory.Level >= 5 ? 3 : memory.Level >= 3 ? 2 : 2;
            var lungeRange = HungryBladesRadius * 1.75f + memory.Level * 0.36f;
            var hits = enemies
                .Where(e => e != null && e.IsAlive && Vector2.Distance(player.position, e.transform.position) <= lungeRange)
                .OrderBy(e => Vector2.Distance(player.position, e.transform.position))
                .Take(memory.Level >= 5 ? KalmuriBiteTargetCap : memory.Level >= 3 ? 3 : 2)
                .ToList();
            for (int i = 0; i < hits.Count; i++)
            {
                var mul = i < targetLimit ? 1f : 0.55f;
                var totalDamage = HungryBladesDps * 0.27f * (1f + (memory.Level - 1) * 0.16f) * mul;
                SpawnHungryBladeBite(hits[i], memory.Level, i, totalDamage);
            }
        }

        void UpdateHungryBladesOrbitVisual(MemoryState memory, float dt)
        {
            memory.VisualTimer += dt * (2.15f + memory.Level * 0.26f);
            memory.VisualSpawnTimer -= dt;
            if (memory.VisualSpawnTimer > 0f) return;
            memory.VisualSpawnTimer = Mathf.Max(0.120f, 0.170f - memory.Level * 0.007f);
            var orbitRadius = HungryBladesRadius * 0.78f + memory.Level * 0.080f;

            var focusTargets = enemies
                .Where(e => e != null && e.IsAlive && Vector2.Distance(player.position, e.transform.position) <= HungryBladesRadius * 1.75f + memory.Level * 0.36f)
                .OrderBy(e => Vector2.Distance(player.position, e.transform.position))
                .Take(memory.Level >= 5 ? 2 : 1)
                .ToList();

            var bladeCount = Mathf.Clamp(6 + memory.Level, 7, KalmuriOrbitBladeCap);
            var huntCount = focusTargets.Count > 0 ? Mathf.Clamp(1 + memory.Level / 3, 1, 2) : 0;
            var huntSlotByBlade = new int[bladeCount];
            for (int i = 0; i < huntSlotByBlade.Length; i++) huntSlotByBlade[i] = -1;
            var huntTargets = new Vector3[huntCount];
            var huntLaunches = new Vector3[huntCount];
            var huntEnds = new Vector3[huntCount];
            var huntColors = new Color[huntCount];
            var huntSeeds = new float[huntCount];
            var huntValid = new bool[huntCount];

            for (int hunt = 0; hunt < huntCount; hunt++)
            {
                var target = focusTargets[hunt % focusTargets.Count];
                var targetPos = target.transform.position + Vector3.up * 0.05f;
                var toTargetFromPlayer = (Vector2)(targetPos - player.position);
                if (toTargetFromPlayer.sqrMagnitude < 0.01f) toTargetFromPlayer = lastAim.sqrMagnitude > 0.01f ? lastAim.normalized : Vector2.up;
                var desiredAngle = Mathf.Atan2(toTargetFromPlayer.y, toTargetFromPlayer.x) * Mathf.Rad2Deg;
                var bestSlot = -1;
                var bestDelta = float.MaxValue;
                for (int slot = 0; slot < bladeCount; slot++)
                {
                    if (huntSlotByBlade[slot] >= 0) continue;
                    var lane = slot % 3;
                    var speed = 190f + memory.Level * 8f + lane * 12f;
                    var angle = memory.VisualTimer * speed + slot * 360f / bladeCount + lane * 10f;
                    var arc = 58f + memory.Level * 2f + lane * 4f;
                    var launchAngle = angle + arc;
                    var delta = Mathf.Abs(Mathf.DeltaAngle(launchAngle, desiredAngle));
                    if (delta < bestDelta)
                    {
                        bestDelta = delta;
                        bestSlot = slot;
                    }
                }

                if (bestSlot >= 0)
                {
                    huntSlotByBlade[bestSlot] = hunt;
                    huntTargets[hunt] = targetPos;
                    huntColors[hunt] = new Color(0.84f, 1f, 1f, 0.90f - hunt * 0.10f);
                    huntSeeds[hunt] = bestSlot * 176f + memory.VisualTimer * (190f + memory.Level * 8f);
                }
            }

            SpawnTransientSprite("KalmuriOrbitPath", MakeRingSprite("KalmuriOrbitPath", Color.white, 180), player.position, Quaternion.Euler(0f, 0f, memory.VisualTimer * 34f), orbitRadius, new Color(0.52f, 0.94f, 1f, 0.10f), 0.18f);
            for (int i = 0; i < bladeCount; i++)
            {
                var lane = i % 3;
                var speed = 190f + memory.Level * 8f + lane * 12f;
                var radiusLane = (lane - 1) * 0.035f;
                var angle = memory.VisualTimer * speed + i * 360f / bladeCount + lane * 10f;
                var arc = 58f + memory.Level * 2f + lane * 4f;
                var scale = 0.23f + memory.Level * 0.019f + lane * 0.005f;
                var alpha = 0.86f - lane * 0.055f;
                var huntIndex = huntSlotByBlade[i];
                if (huntIndex >= 0)
                {
                    var launchAngle = angle + arc;
                    var launchRadius = orbitRadius + radiusLane;
                    var launch = player.position + Quaternion.Euler(0f, 0f, launchAngle) * Vector3.right * launchRadius;
                    var targetPos = huntTargets[huntIndex];
                    var toTarget = ((Vector2)(targetPos - launch)).normalized;
                    if (toTarget.sqrMagnitude < 0.01f) toTarget = lastAim.sqrMagnitude > 0.01f ? lastAim.normalized : Vector2.up;
                    var side = new Vector2(-toTarget.y, toTarget.x);
                    huntLaunches[huntIndex] = launch;
                    huntEnds[huntIndex] = targetPos - (Vector3)(toTarget * (0.12f + huntIndex * 0.03f)) + (Vector3)(side * ((huntIndex - (huntCount - 1) * 0.5f) * 0.10f));
                    huntValid[huntIndex] = true;
                    SpawnOrbitingKalmuriBlade("KalmuriLivingOrbitBlade_Hunter", player.position, launchRadius, angle - 10f, launchAngle, scale * 1.10f, new Color(0.86f, 1f, 1f, 0.94f), 0.24f);
                    SpawnTransientSprite("KalmuriHuntReleaseSpark", MakeImpactDiamondSprite("KalmuriHuntReleaseSpark", Color.white), launch, Quaternion.Euler(0f, 0f, launchAngle + 45f), scale * 0.58f, new Color(0.92f, 1f, 1f, 0.62f), 0.11f);
                    continue;
                }

                SpawnOrbitingKalmuriBlade("KalmuriLivingOrbitBlade", player.position, orbitRadius + radiusLane, angle, angle + arc, scale, new Color(0.74f, 0.98f, 1f, alpha), 0.26f);
            }

            for (int i = 0; i < huntCount; i++)
            {
                if (!huntValid[i]) continue;
                var launch = huntLaunches[i];
                var targetPos = huntTargets[i];
                var end = huntEnds[i];
                var color = huntColors[i];
                SpawnEchoLink("KalmuriHuntLockLine", launch, targetPos, new Color(0.62f, 0.96f, 1f, 0.46f), 0.16f, 0.024f);
                SpawnKalmuriDiveBlade("KalmuriHuntingLunge", launch, end, 0.29f + memory.Level * 0.020f, color, 0.22f, 0.14f);

                if (memory.Level >= 4 && i == 0)
                {
                    var recoil = player.position + Quaternion.Euler(0f, 0f, huntSeeds[i] + 78f) * Vector3.right * (orbitRadius * 0.42f);
                    SpawnKalmuriDiveBlade("KalmuriHuntingRecoil", end, recoil, 0.18f + memory.Level * 0.010f, new Color(0.44f, 0.90f, 1f, 0.38f), 0.12f, 0.08f);
                }
            }
        }

        void SpawnHungryBladeBite(V1Enemy target, int levelValue, int targetIndex, float totalDamage)
        {
            if (target == null || !target.IsAlive) return;
            var center = target.transform.position;
            var toTarget = ((Vector2)(center - player.position)).normalized;
            if (toTarget.sqrMagnitude < 0.01f) toTarget = lastAim.sqrMagnitude > 0.01f ? lastAim.normalized : Vector2.up;
            var side = new Vector2(-toTarget.y, toTarget.x);
            var baseAngle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
            var bladeCount = Mathf.Clamp(3 + levelValue / 3, 4, KalmuriBiteBladeCap);
            var damagePerBlade = totalDamage / bladeCount;
            var launchRadius = HungryBladesRadius * 0.78f + levelValue * 0.080f;
            var scale = 0.21f + levelValue * 0.020f;
            PlaySfx("kalmuri_lunge", 0.58f, 0.055f);
            SpawnTransientSprite("KalmuriBiteHalo", MakeRingSprite("KalmuriBiteHalo", Color.white, 136), center, Quaternion.identity, 0.42f + levelValue * 0.040f, new Color(0.56f, 0.96f, 1f, 0.36f), 0.14f);
            SpawnEchoWoundSlash("KalmuriBiteCut", center, toTarget, new Color(0.80f, 1f, 1f, 0.86f), 0.72f + levelValue * 0.055f, 0.12f);
            SpawnEchoWoundSlash("KalmuriBiteCrossCut", center + (Vector3)(side * 0.04f), side, new Color(0.58f, 0.94f, 1f, 0.50f), 0.46f + levelValue * 0.035f, 0.10f);
            for (int i = 0; i < bladeCount; i++)
            {
                var bladeIndex = i - (bladeCount - 1) * 0.5f;
                var spread = bladeIndex * 0.12f;
                var phase = i * 1.37f + targetIndex * 0.41f;
                var launchAngle = baseAngle + bladeIndex * 7.5f + Mathf.Sin(phase) * 5f;
                var launchDir = Quaternion.Euler(0f, 0f, launchAngle) * Vector3.right;
                var start = player.position + launchDir * (launchRadius + (i % 3) * 0.055f) + (Vector3)(side * spread * 0.25f);
                var end = center + (Vector3)(toTarget * (0.26f + (i % 2) * 0.06f)) - (Vector3)(side * spread * 0.16f);
                var sweep = 0.070f + (i % 4) * 0.008f;
                SpawnKalmuriDiveBlade("KalmuriBiteDiveBlade", start, end, scale + (i % 3) * 0.012f, new Color(0.86f, 1f, 1f, 0.92f), 0.18f, sweep);
                StartCoroutine(DealDamageDelayed(target, damagePerBlade, "굶주린 칼무리", false, toTarget, i == 0 ? 0.06f : 0f, 0.035f + i * 0.012f));

                if (i == 0 && levelValue >= 3)
                {
                    var recoil = center + (Vector3)(toTarget * 0.18f) + (Vector3)(side * (spread * 1.30f + (i - 1) * 0.16f));
                    SpawnKalmuriDiveBlade("KalmuriBiteReturnShard", end, recoil, scale * 0.72f, new Color(0.46f, 0.90f, 1f, 0.42f), 0.12f, 0.08f);
                }
            }
        }

        void TriggerWeaponEchoes(V1Enemy enemy, Vector2 forward, int hitIndex, WeaponRuntimeSpec weapon)
        {
            var kalmuriLevel = EchoLevel(V1MemoryId.HungryBlades);
            if (kalmuriLevel > 0)
            {
                var chance = kalmuriLevel >= 2 ? 1f : 0.30f;
                if (UnityEngine.Random.value <= chance)
                {
                    TriggerKalmuriEcho(enemy, forward, kalmuriLevel, hitIndex, weapon);
                    if (kalmuriLevel >= 5 && kalmuriAwakenLaunchCooldown <= 0f && !DenseDualBladeVfxThrottle(weapon))
                    {
                        LaunchKalmuriBlade(enemy, forward, weapon);
                        kalmuriAwakenLaunchCooldown = 0.35f;
                    }
                }
            }

            var bloodLevel = EchoLevel(V1MemoryId.BloodReflection);
            if (bloodLevel > 0)
            {
                enemy.BloodMarked = true;
                enemy.MarkTimer = 2.2f + bloodLevel * 0.25f;
                var denseSecondary = DenseDualBladeVfxThrottle(weapon) && hitIndex > 0;
                if (!denseSecondary)
                {
                PlaySfx("blood_mark", 0.34f, 0.08f);
                SpawnTransientSprite("BloodEchoPulse", MakeRingSprite("BloodEchoPulse", Color.white, 128), enemy.transform.position, Quaternion.identity, 0.38f + bloodLevel * 0.025f, new Color(1f, 0.10f, 0.18f, 0.42f), 0.28f);
                SpawnEchoWoundSlash("BloodEchoWound", enemy.transform.position + Vector3.up * 0.04f, forward, new Color(1f, 0.10f, 0.18f, 0.62f), 0.72f, 0.30f);
                SpawnTransientSprite("혈반", LoadSprite("Assets/_dev/Art/Sprites/Echoes/Blood/spr_blood_mark_01.png"), enemy.transform.position + Vector3.up * 0.2f, Quaternion.identity, 0.33f, new Color(1f, 0.18f, 0.25f, 0.9f), 0.35f);
                if (!DenseDualBladeVfxThrottle(weapon) && bloodLevel >= 5 && UnityEngine.Random.value < 0.42f)
                {
                    BloodBloom(enemy, bloodLevel);
                }
                }
            }

            var denseDualBlade = DenseDualBladeVfxThrottle(weapon);
            if (!denseDualBlade)
            {
                TriggerBloodEchoAccent(enemy, forward, bloodLevel, hitIndex, weapon, false);
                TriggerUtilityEchoes(enemy, forward, hitIndex, weapon);
            }
            else if (hitIndex == 0)
            {
                TriggerUtilityEchoes(enemy, forward, hitIndex, weapon);
            }
        }

        void TriggerUtilityEchoes(V1Enemy enemy, Vector2 forward, int hitIndex, WeaponRuntimeSpec weapon)
        {
            if (enemy == null) return;
            if (DenseDualBladeVfxThrottle(weapon))
            {
                var bucket = Mathf.Abs(Mathf.FloorToInt(elapsed * 11f) + hitIndex) % 3;
                if (bucket == 0)
                {
                    TriggerExecutionEcho(enemy, forward, hitIndex, weapon, false);
                }
                else if (bucket == 1)
                {
                    TriggerShatterEcho(enemy, forward, hitIndex, weapon, false);
                }
                else
                {
                    TriggerAshenEcho(enemy, forward, hitIndex, weapon, false);
                }
                return;
            }
            TriggerShatterEcho(enemy, forward, hitIndex, weapon, false);
            TriggerExecutionEcho(enemy, forward, hitIndex, weapon, false);
            TriggerHunterEcho(enemy, forward, hitIndex, weapon, false);
            TriggerStoppedEcho(enemy, forward, hitIndex, weapon, false);
            TriggerAshenEcho(enemy, forward, hitIndex, weapon, false);
            TriggerOblivionEcho(enemy, forward, hitIndex, weapon, false);
        }

        void MarkEnemyEchoState(V1Enemy enemy, V1MemoryId id, float lifetime, float scaleMul = 1f)
        {
            if (enemy == null || !enemy.IsAlive) return;
            enemy.ApplyEchoStateMark(id, EchoStateMarkSprite(id), EchoStateMarkColor(id, 0.92f), lifetime, scaleMul);
            SpawnEchoIdentityBurst(enemy.transform.position, id, lifetime, scaleMul);
        }

        Sprite EchoStateMarkSprite(V1MemoryId id)
        {
            return id switch
            {
                V1MemoryId.ExecutionFlash => MakeImpactDiamondSprite("EchoStateExecution", Color.white),
                V1MemoryId.HunterOath => MakeRingSprite("EchoStateHunter", Color.white, 96),
                V1MemoryId.ShatterWave => MakeImpactDiamondSprite("EchoStateShatter", Color.white),
                V1MemoryId.StoppedSecond => MakeRingSprite("EchoStateStopped", Color.white, 120),
                V1MemoryId.AshenShield => MakeRingSprite("EchoStateAshen", Color.white, 96),
                V1MemoryId.OblivionBrand => MakeImpactDiamondSprite("EchoStateOblivion", Color.white),
                _ => MakeCircleSprite("EchoStateDefault", Color.white, 48)
            };
        }

        Color EchoStateMarkColor(V1MemoryId id, float alpha)
        {
            return id switch
            {
                V1MemoryId.ExecutionFlash => new Color(1f, 0.92f, 0.42f, alpha),
                V1MemoryId.HunterOath => new Color(0.68f, 1f, 0.32f, alpha),
                V1MemoryId.ShatterWave => new Color(0.46f, 0.94f, 1f, alpha),
                V1MemoryId.StoppedSecond => new Color(1f, 0.74f, 0.24f, alpha),
                V1MemoryId.AshenShield => new Color(0.82f, 0.90f, 1f, alpha),
                V1MemoryId.OblivionBrand => new Color(0.82f, 0.44f, 1f, alpha),
                _ => new Color(1f, 1f, 1f, alpha)
            };
        }

        void SpawnEchoIdentityBurst(Vector3 center, V1MemoryId id, float lifetime, float scaleMul)
        {
            var scale = Mathf.Max(0.25f, scaleMul);
            switch (id)
            {
                case V1MemoryId.ExecutionFlash:
                    SpawnTransientSprite("EchoIdentity_ExecutionVerdict", MakeImpactDiamondSprite("EchoIdentity_ExecutionVerdict", Color.white), center + Vector3.up * 0.10f, Quaternion.Euler(0f, 0f, 45f), 0.34f * scale, new Color(1f, 0.94f, 0.42f, 0.82f), Mathf.Min(0.34f, lifetime));
                    SpawnRadialSlashLines("EchoIdentity_ExecutionCrack", center, Vector2.up, 3, 0.55f * scale, new Color(1f, 0.92f, 0.42f, 0.58f), Mathf.Min(0.32f, lifetime));
                    break;
                case V1MemoryId.HunterOath:
                    SpawnTransientSprite("EchoIdentity_HunterLock", MakeRingSprite("EchoIdentity_HunterLock", Color.white, 96), center, Quaternion.Euler(0f, 0f, elapsed * -120f), 0.48f * scale, new Color(0.68f, 1f, 0.32f, 0.58f), Mathf.Min(0.42f, lifetime));
                    SpawnEchoWoundLine("EchoIdentity_HunterNeedle", center + Vector3.up * 0.03f, lastAim.sqrMagnitude > 0.01f ? lastAim : Vector2.up, new Color(0.78f, 1f, 0.38f, 0.54f), 0.62f * scale, Mathf.Min(0.34f, lifetime));
                    break;
                case V1MemoryId.ShatterWave:
                    SpawnTransientSprite("EchoIdentity_ShatterCore", MakeImpactDiamondSprite("EchoIdentity_ShatterCore", Color.white), center, Quaternion.Euler(0f, 0f, elapsed * 90f), 0.28f * scale, new Color(0.46f, 0.94f, 1f, 0.66f), Mathf.Min(0.34f, lifetime));
                    SpawnRadialSlashLines("EchoIdentity_ShatterFault", center, Vector2.up, 5, 0.58f * scale, new Color(0.54f, 0.96f, 1f, 0.50f), Mathf.Min(0.38f, lifetime));
                    break;
                case V1MemoryId.StoppedSecond:
                    SpawnStoppedEchoClamp(center, 0.46f * scale, Mathf.Min(0.58f, lifetime));
                    break;
                case V1MemoryId.AshenShield:
                    SpawnTransientSprite("EchoIdentity_AshenWard", MakeRingSprite("EchoIdentity_AshenWard", Color.white, 112), center, Quaternion.Euler(0f, 0f, elapsed * -70f), 0.50f * scale, new Color(0.86f, 0.94f, 1f, 0.54f), Mathf.Min(0.46f, lifetime));
                    SpawnRadialSlashLines("EchoIdentity_AshenShard", center, Vector2.up, 4, 0.40f * scale, new Color(0.84f, 0.92f, 1f, 0.42f), Mathf.Min(0.34f, lifetime));
                    break;
                case V1MemoryId.OblivionBrand:
                    SpawnOblivionEchoBrand(center, lastAim.sqrMagnitude > 0.01f ? lastAim : Vector2.up, Mathf.Min(0.56f, lifetime));
                    SpawnTransientSprite("EchoIdentity_OblivionPip", MakeImpactDiamondSprite("EchoIdentity_OblivionPip", Color.white), center + Vector3.up * 0.14f, Quaternion.Euler(0f, 0f, elapsed * 130f), 0.20f * scale, new Color(0.92f, 0.56f, 1f, 0.66f), Mathf.Min(0.36f, lifetime));
                    break;
            }
        }

        bool IsHeavyEchoWeapon(WeaponRuntimeSpec weapon) => weapon.EchoProcStyle == V1EchoProcStyle.SingleHeavy;

        Vector2 EchoForward(Vector2 forward)
        {
            if (forward.sqrMagnitude > 0.01f) return forward.normalized;
            if (lastAim.sqrMagnitude > 0.01f) return lastAim.normalized;
            return Vector2.up;
        }

        float EnemyThreatPriority(V1Enemy enemy)
        {
            if (enemy == null) return 0f;
            return enemy.Kind switch
            {
                V1EnemyKind.Gatekeeper => 6f,
                V1EnemyKind.VoidPriest => 5f,
                V1EnemyKind.DriftingEye => 3.5f,
                V1EnemyKind.SplitOne => 2.8f,
                _ => 1f
            };
        }

        List<V1Enemy> SelectHunterTargets(Vector3 origin, V1Enemy exclude, int cap)
        {
            return enemies
                .Where(e => e != null && e.IsAlive && e != exclude)
                .OrderByDescending(EnemyThreatPriority)
                .ThenBy(e => Vector2.Distance(origin, e.transform.position))
                .Take(Mathf.Max(1, cap))
                .ToList();
        }

        void SpawnExecutionForecast(V1Enemy enemy, int levelValue, bool echo)
        {
            if (enemy == null || !enemy.IsAlive) return;
            var pos = enemy.transform.position;
            var scale = echo ? 0.54f + levelValue * 0.035f : 0.46f + levelValue * 0.030f;
            SpawnTransientSprite(echo ? "ExecutionEchoForecastRing" : "ExecutionForecastRing", MakeRingSprite("ExecutionForecastRing", Color.white, 128), pos, Quaternion.Euler(0f, 0f, elapsed * -96f), scale, new Color(1f, 0.88f, 0.36f, echo ? 0.46f : 0.34f), echo ? 0.42f : 0.34f);
            SpawnRadialSlashLines(echo ? "ExecutionEchoForecastCrack" : "ExecutionForecastCrack", pos, (Vector2)(pos - player.position), 3, 0.42f + levelValue * 0.035f, new Color(1f, 0.92f, 0.42f, echo ? 0.54f : 0.38f), echo ? 0.34f : 0.28f);
        }

        void SpawnStoppedFractureBurst(Vector3 center, Vector2 forward, float scale, bool heavy)
        {
            var color = TimeStopGold(heavy);
            SpawnTransientSprite(heavy ? "StoppedFractureHeavySeal" : "StoppedFractureSeal", MakeImpactDiamondSprite("StoppedFractureSeal", Color.white), center, Quaternion.Euler(0f, 0f, elapsed * 140f), 0.28f * scale, new Color(1f, 0.84f, 0.30f, heavy ? 0.82f : 0.62f), heavy ? 0.34f : 0.26f);
            SpawnRadialSlashLines(heavy ? "StoppedFractureHeavyTick" : "StoppedFractureTick", center, forward, heavy ? 6 : 4, 0.46f * scale, new Color(color.r, color.g, color.b, heavy ? 0.68f : 0.48f), heavy ? 0.34f : 0.26f);
        }

        void AddAshenGuardCharge(float amount, int levelValue, Vector3 origin, bool quiet)
        {
            if (amount <= 0f) return;
            var cap = 18f + levelValue * 11f;
            ashenStoredGuardCharge = Mathf.Min(cap, ashenStoredGuardCharge + amount);
            if (!quiet)
            {
                SpawnTransientSprite("AshenGuardChargePip", MakeRingSprite("AshenGuardChargePip", Color.white, 112), origin, Quaternion.Euler(0f, 0f, elapsed * -120f), 0.46f + Mathf.Clamp01(ashenStoredGuardCharge / cap) * 0.34f, new Color(0.86f, 0.94f, 1f, 0.42f), 0.28f);
            }
        }

        float ConsumeAshenGuardCharge(float fraction, float minimum)
        {
            if (ashenStoredGuardCharge <= 0.01f) return 0f;
            var amount = Mathf.Max(minimum, ashenStoredGuardCharge * Mathf.Clamp01(fraction));
            amount = Mathf.Min(amount, ashenStoredGuardCharge);
            ashenStoredGuardCharge -= amount;
            return amount;
        }

        void ReleaseAshenGuardWave(Vector3 center, int levelValue, float charge, bool heavy, string source)
        {
            if (charge <= 0.01f) return;
            var radius = (heavy ? 2.45f : 1.70f) + levelValue * (heavy ? 0.24f : 0.16f) + Mathf.Clamp(charge * 0.018f, 0f, heavy ? 0.80f : 0.48f);
            var damage = charge * (heavy ? 1.10f : 0.82f) + levelValue * (heavy ? 8.0f : 5.0f);
            PlaySfx("ashen", heavy ? 0.62f : 0.44f, 0.18f);
            SpawnTransientSprite(heavy ? "AshenStoredWaveHeavy" : "AshenStoredWave", MakeRingSprite("AshenStoredWave", Color.white, 180), center, Quaternion.identity, radius, new Color(0.88f, 0.96f, 1f, heavy ? 0.68f : 0.52f), heavy ? 0.48f : 0.34f);
            SpawnTransientSprite(heavy ? "AshenStoredWaveCoreHeavy" : "AshenStoredWaveCore", MakeDiscSprite("AshenStoredWaveCore", Color.white, 128), center, Quaternion.identity, radius * 0.36f, new Color(0.88f, 0.90f, 1f, heavy ? 0.38f : 0.26f), heavy ? 0.30f : 0.22f);
            SpawnRadialSlashLines(heavy ? "AshenStoredWaveShardHeavy" : "AshenStoredWaveShard", center, lastAim, heavy ? 9 : 6, radius * 0.62f, new Color(0.84f, 0.92f, 1f, heavy ? 0.66f : 0.48f), heavy ? 0.42f : 0.30f);
            foreach (var target in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(center, e.transform.position) <= radius + e.TouchRadius).Take(heavy ? 14 : 9).ToList())
            {
                var dir = (Vector2)(target.transform.position - center);
                MarkEnemyEchoState(target, V1MemoryId.AshenShield, heavy ? 1.55f : 1.15f, heavy ? 1.14f : 0.96f);
                DealDamage(target, damage, source, false, dir.sqrMagnitude > 0.01f ? dir.normalized : Vector2.up, heavy ? 1.15f : 0.62f);
            }
            hitstopTimer = Mathf.Max(hitstopTimer, heavy ? 0.045f : 0.025f);
            cameraShakeTimer = Mathf.Max(cameraShakeTimer, heavy ? 0.18f : 0.10f);
            cameraShakeAmount = Mathf.Max(cameraShakeAmount, heavy ? 0.085f : 0.052f);
        }

        bool ShouldTriggerEcho(V1MemoryId id, int levelValue, int hitIndex, bool force)
        {
            if (force) return true;
            var spec = EchoTuning(id);
            if (spec.RequiresFirstHit(levelValue) && hitIndex != 0) return false;
            return UnityEngine.Random.value < spec.ProcChance(levelValue);
        }

        UtilityEchoTuningSpec EchoTuning(V1MemoryId id)
        {
            if (utilityEchoTuningTable != null && utilityEchoTuningTable.TryGetSpec(id, out var assetSpec))
            {
                return assetSpec;
            }

            var specs = utilityEchoTuningSpecs;
            if (specs != null)
            {
                for (int i = 0; i < specs.Length; i++)
                {
                    if (specs[i].Id == id) return specs[i];
                }
            }

            for (int i = 0; i < DefaultUtilityEchoTuningSpecTable.Length; i++)
            {
                if (DefaultUtilityEchoTuningSpecTable[i].Id == id) return DefaultUtilityEchoTuningSpecTable[i];
            }

            return UtilityEchoTuningSpec.Fallback(id);
        }

        float EchoProcChance(V1MemoryId id, int levelValue) => EchoTuning(id).ProcChance(levelValue);

        float EchoRadius(V1MemoryId id, int levelValue, bool heavy) => EchoTuning(id).Radius(levelValue, heavy);

        int EchoTargetLimit(V1MemoryId id, int levelValue, bool heavy) => EchoTuning(id).TargetLimit(levelValue, heavy);

        float EchoDamageMultiplier(V1MemoryId id, int levelValue, bool heavy) => EchoTuning(id).DamageMultiplier(levelValue, heavy);

        float EchoFreezeSeconds(int levelValue, bool heavy) => EchoTuning(V1MemoryId.StoppedSecond).FreezeSeconds(levelValue, heavy);

        float ExecutionHealthThreshold(int levelValue) => EchoTuning(V1MemoryId.ExecutionFlash).ExecutionHealthThreshold(levelValue);

        void TriggerBloodEchoAccent(V1Enemy enemy, Vector2 forward, int levelValue, int hitIndex, WeaponRuntimeSpec weapon, bool force)
        {
            if (enemy == null || !enemy.IsAlive || levelValue <= 0) return;
            var heavy = IsHeavyEchoWeapon(weapon);
            var denseDualBlade = DenseDualBladeVfxThrottle(weapon);
            var f = EchoForward(forward);
            if (heavy)
            {
                var radius = 1.02f + levelValue * 0.12f;
                SpawnTransientSprite("EchoGreat_BloodCleavePool", MakeRingSprite("EchoGreat_BloodCleavePool", Color.white, 144), enemy.transform.position, Quaternion.Euler(0f, 0f, elapsed * -80f), radius, new Color(1f, 0.08f, 0.14f, 0.34f), 0.44f);
                SpawnEchoWoundSlash("EchoGreat_BloodCleaveWound", enemy.transform.position + Vector3.up * 0.04f, f, new Color(1f, 0.10f, 0.18f, 0.78f), 1.22f, 0.42f);
                foreach (var target in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(enemy.transform.position, e.transform.position) <= radius + e.TouchRadius).Take(6).ToList())
                {
                    var dir = (Vector2)(target.transform.position - enemy.transform.position);
                    target.BloodMarked = true;
                    target.MarkTimer = Mathf.Max(target.MarkTimer, 1.9f);
                    DealDamage(target, weapon.Damage * (0.035f + levelValue * 0.010f), "Blood Echo Great", false, dir.sqrMagnitude > 0.01f ? dir.normalized : f, 0.12f);
                }
            }
            else
            {
                var side = new Vector2(-f.y, f.x);
                SpawnTransientSprite("EchoDual_BloodMarkPulse", MakeRingSprite("EchoDual_BloodMarkPulse", Color.white, 96), enemy.transform.position, Quaternion.identity, 0.34f + levelValue * 0.018f, new Color(1f, 0.10f, 0.18f, 0.34f), 0.20f);
                for (int i = 0; i < Mathf.Min(4, 1 + levelValue / 2); i++)
                {
                    var offset = side * ((i - 1.5f) * 0.10f) + f * (0.05f * i);
                    SpawnEchoWoundSlash("EchoDual_BloodNeedleStack", enemy.transform.position + (Vector3)offset, f, new Color(1f, 0.16f, 0.22f, 0.48f), 0.44f + i * 0.08f, 0.20f);
                }
                DealDamage(enemy, weapon.Damage * (0.020f + levelValue * 0.006f), "Blood Echo Dual", false, f, 0.04f);
            }

            if (!denseDualBlade && levelValue >= 5 && (force || UnityEngine.Random.value < 0.12f))
            {
                BloodBloom(enemy, levelValue);
            }
        }

        void TriggerShatterEcho(V1Enemy enemy, Vector2 forward, int hitIndex, WeaponRuntimeSpec weapon, bool force)
        {
            var levelValue = EchoLevel(V1MemoryId.ShatterWave);
            if (levelValue <= 0) return;
            var heavy = IsHeavyEchoWeapon(weapon);
            var f = EchoForward(forward);
            if (!ShouldTriggerEcho(V1MemoryId.ShatterWave, levelValue, hitIndex, force)) return;

            var radius = EchoRadius(V1MemoryId.ShatterWave, levelValue, heavy);
            SpawnTransientSprite(heavy ? "EchoGreat_ShatterCleaveTell" : "EchoDual_ShatterRippleTell", MakeRingSprite("ShatterEchoTell", Color.white, heavy ? 168 : 112), enemy.transform.position, Quaternion.identity, heavy ? radius * 1.02f : radius * 0.72f, new Color(0.54f, 0.92f, 1f, heavy ? 0.48f : 0.34f), heavy ? 0.34f : 0.22f);
            SpawnShatterEchoScar(enemy.transform.position, f, radius, heavy ? 0.72f : 0.54f);
            SpawnShatterWaveField(enemy.transform.position, radius, heavy ? 1.32f : 1.14f, true);
            if (heavy)
            {
                SpawnRadialSlashLines("EchoGreat_ShatterFracture", enemy.transform.position, f, 3, radius * 0.82f, new Color(0.72f, 0.98f, 1f, 0.64f), 0.46f);
                hitstopTimer = Mathf.Max(hitstopTimer, 0.032f);
                cameraShakeTimer = Mathf.Max(cameraShakeTimer, 0.11f);
                cameraShakeAmount = Mathf.Max(cameraShakeAmount, 0.052f);
            }
            foreach (var target in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(enemy.transform.position, e.transform.position) <= radius + e.TouchRadius).Take(EchoTargetLimit(V1MemoryId.ShatterWave, levelValue, heavy)).ToList())
            {
                var dir = (Vector2)(target.transform.position - enemy.transform.position);
                MarkEnemyEchoState(target, V1MemoryId.ShatterWave, heavy ? 1.55f : 1.05f, heavy ? 1.16f : 0.92f);
                DealDamage(target, weapon.Damage * EchoDamageMultiplier(V1MemoryId.ShatterWave, levelValue, heavy), heavy ? "Shatter Echo Great" : "Shatter Echo Dual", false, dir.sqrMagnitude > 0.01f ? dir.normalized : f, heavy ? 0.62f : 0.25f);
            }
        }

        void TriggerExecutionEcho(V1Enemy enemy, Vector2 forward, int hitIndex, WeaponRuntimeSpec weapon, bool force)
        {
            var levelValue = EchoLevel(V1MemoryId.ExecutionFlash);
            if (levelValue <= 0) return;
            var heavy = IsHeavyEchoWeapon(weapon);
            var f = EchoForward(forward);
            var executeThreshold = ExecutionHealthThreshold(levelValue);
            if (!force && enemy.HealthRatio > executeThreshold)
            {
                if (levelValue < 3 || enemy.HealthRatio > executeThreshold + 0.20f)
                {
                    SpawnExecutionForecast(enemy, levelValue, true);
                    return;
                }
                SpawnExecutionForecast(enemy, levelValue, true);
                MarkEnemyEchoState(enemy, V1MemoryId.ExecutionFlash, 0.90f, heavy ? 1.02f : 0.90f);
                DealDamage(enemy, weapon.Damage * (heavy ? 0.20f + levelValue * 0.030f : 0.12f + levelValue * 0.024f), "Execution Echo forecast", false, f, heavy ? 0.24f : 0.08f);
                return;
            }

            PlaySfx("execution", heavy ? 0.68f : 0.48f, heavy ? 0.14f : 0.08f);
            SpawnPromptSprite(heavy ? "EchoGreat_ExecutionStamp" : "EchoDual_ExecutionChain", EchoVfxSprite(V1MemoryId.ExecutionFlash), () => MakeImpactDiamondSprite("ExecutionEcho", Color.white), enemy.transform.position, Quaternion.identity, heavy ? 2.34f : 1.82f, heavy ? 1.02f : 0.78f, new Color(1f, 0.92f, 0.58f, 0.98f), heavy ? 0.66f : 0.52f);
            SpawnTransientSprite(heavy ? "EchoGreat_ExecutionHalo" : "EchoDual_ExecutionHalo", MakeRingSprite("ExecutionEchoHalo", Color.white, heavy ? 168 : 132), enemy.transform.position, Quaternion.identity, heavy ? 1.02f : 0.72f, new Color(1f, 0.90f, 0.44f, heavy ? 0.58f : 0.46f), heavy ? 0.46f : 0.36f);
            if (heavy)
            {
                SpawnRadialSlashLines("EchoGreat_ExecutionCrack", enemy.transform.position, f, 4, 1.18f + levelValue * 0.08f, new Color(1f, 0.96f, 0.50f, 0.76f), 0.44f);
                foreach (var target in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(enemy.transform.position, e.transform.position) <= 1.20f + levelValue * 0.08f).Take(6).ToList())
                {
                    MarkEnemyEchoState(target, V1MemoryId.ExecutionFlash, 1.15f, heavy ? 1.16f : 0.96f);
                    DealDamage(target, weapon.Damage * (target.HealthRatio < 0.36f ? 0.34f + levelValue * 0.065f : 0.15f + levelValue * 0.035f), "Execution Echo Great", false);
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    var rotated = Quaternion.Euler(0f, 0f, i * 38f) * (Vector3)f;
                    SpawnEchoWoundSlash("EchoDual_ExecutionChainCut", enemy.transform.position + (Vector3)(f * (i * 0.05f)), (Vector2)rotated, new Color(1f, 0.96f, 0.50f, 0.78f - i * 0.10f), 0.86f + i * 0.14f, 0.30f);
                }
                SpawnExecutionFlashBurst(enemy.transform.position, 0.92f, 0.44f);
                MarkEnemyEchoState(enemy, V1MemoryId.ExecutionFlash, 1.05f, 0.96f);
                DealDamage(enemy, weapon.Damage * (0.18f + levelValue * 0.05f), "Execution Echo Dual", false);
            }
        }

        void TriggerHunterEcho(V1Enemy enemy, Vector2 forward, int hitIndex, WeaponRuntimeSpec weapon, bool force)
        {
            var levelValue = EchoLevel(V1MemoryId.HunterOath);
            if (levelValue <= 0) return;
            var heavy = IsHeavyEchoWeapon(weapon);
            if (!ShouldTriggerEcho(V1MemoryId.HunterOath, levelValue, hitIndex, force)) return;

            var targets = SelectHunterTargets(enemy.transform.position, enemy, EchoTargetLimit(V1MemoryId.HunterOath, levelValue, heavy));
            if (force && targets.Count == 0) targets.Add(enemy);
            SpawnTransientSprite(heavy ? "EchoGreat_HunterSpearLock" : "EchoDual_HunterFanLock", MakeRingSprite("HunterEchoOriginMark", Color.white, heavy ? 148 : 112), enemy.transform.position, Quaternion.Euler(0f, 0f, elapsed * (heavy ? -55f : 90f)), heavy ? 0.68f : 0.46f, new Color(0.74f, 1f, 0.38f, heavy ? 0.66f : 0.54f), heavy ? 0.58f : 0.42f);
            for (int i = 0; i < targets.Count; i++)
            {
                MarkEnemyEchoState(targets[i], V1MemoryId.HunterOath, heavy ? 1.45f : 1.15f, heavy ? 1.08f : 0.92f);
                SpawnEchoLink(heavy ? "EchoGreat_HunterSpearLine" : "EchoDual_HunterAimLine", enemy.transform.position, targets[i].transform.position, new Color(0.72f, 1f, 0.36f, heavy ? 0.58f : 0.42f), heavy ? 0.58f : 0.40f, heavy ? 0.032f : 0.020f);
                if (heavy) SpawnHunterGreatEchoSpear(targets[i], enemy.transform.position, levelValue, weapon);
                else SpawnHunterOathShot(targets[i], enemy.transform.position, i, targets.Count, 9.2f + levelValue * 0.25f, weapon.Damage * EchoDamageMultiplier(V1MemoryId.HunterOath, levelValue, heavy), HunterEchoSource, true);
            }
        }

        void TriggerStoppedEcho(V1Enemy enemy, Vector2 forward, int hitIndex, WeaponRuntimeSpec weapon, bool force)
        {
            var levelValue = EchoLevel(V1MemoryId.StoppedSecond);
            if (levelValue <= 0) return;
            var heavy = IsHeavyEchoWeapon(weapon);
            var f = EchoForward(forward);
            if (!ShouldTriggerEcho(V1MemoryId.StoppedSecond, levelValue, hitIndex, force)) return;

            PlaySfx("stopped", heavy ? 0.58f : 0.40f, heavy ? 0.18f : 0.12f);
            var radius = EchoRadius(V1MemoryId.StoppedSecond, levelValue, heavy);
            SpawnTransientSprite(heavy ? "EchoGreat_StoppedDome" : "EchoDual_StoppedMicroTick", MakeRingSprite("StoppedEchoWeaponTell", Color.white, heavy ? 180 : 96), enemy.transform.position, Quaternion.identity, radius * (heavy ? 0.95f : 0.72f), new Color(1f, 0.76f, 0.22f, heavy ? 0.48f : 0.34f), heavy ? 0.54f : 0.26f);
            SpawnStoppedEchoClamp(enemy.transform.position, heavy ? 1.00f + levelValue * 0.08f : 0.74f + levelValue * 0.055f, heavy ? 1.36f : 1.12f);
            SpawnStoppedSecondField(enemy.transform.position, heavy ? radius : 1.36f + levelValue * 0.16f, TimeStopGold(heavy), heavy ? 1.70f : 1.48f, heavy);
            foreach (var target in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(enemy.transform.position, e.transform.position) <= radius + e.TouchRadius).Take(EchoTargetLimit(V1MemoryId.StoppedSecond, levelValue, heavy)).ToList())
            {
                var dir = (Vector2)(target.transform.position - enemy.transform.position);
                MarkEnemyEchoState(target, V1MemoryId.StoppedSecond, heavy ? 1.90f : 1.45f, heavy ? 1.08f : 0.94f);
                target.ApplyBriefFreeze(EchoFreezeSeconds(levelValue, heavy));
                DealDamage(target, weapon.Damage * EchoDamageMultiplier(V1MemoryId.StoppedSecond, levelValue, heavy), heavy ? "Stopped Echo Great" : "Stopped Echo Dual", false, dir.sqrMagnitude > 0.01f ? dir.normalized : f, 0.08f);
                if (levelValue >= 3)
                {
                    SpawnStoppedFractureBurst(target.transform.position, dir.sqrMagnitude > 0.01f ? dir.normalized : f, heavy ? 1.18f : 0.82f, heavy);
                    DealDamage(target, weapon.Damage * (heavy ? 0.18f + levelValue * 0.036f : 0.09f + levelValue * 0.024f), heavy ? "Stopped Echo Great fracture" : "Stopped Echo Dual fracture", false, dir.sqrMagnitude > 0.01f ? dir.normalized : f, heavy ? 0.28f : 0.08f);
                }
            }
        }

        void TriggerAshenEcho(V1Enemy enemy, Vector2 forward, int hitIndex, WeaponRuntimeSpec weapon, bool force)
        {
            var levelValue = EchoLevel(V1MemoryId.AshenShield);
            if (levelValue <= 0) return;
            var heavy = IsHeavyEchoWeapon(weapon);
            var f = EchoForward(forward);
            if (!ShouldTriggerEcho(V1MemoryId.AshenShield, levelValue, hitIndex, force)) return;

            HealPlayer((heavy ? 0.48f : 0.28f) + levelValue * (heavy ? 0.16f : 0.12f));
            AddAshenGuardCharge(weapon.Damage * (heavy ? 0.16f : 0.075f) + levelValue * 0.65f, levelValue, enemy.transform.position, DenseDualBladeVfxThrottle(weapon));
            PlaySfx("ashen", heavy ? 0.46f : 0.30f, heavy ? 0.24f : 0.18f);
            SpawnTransientSprite(heavy ? "EchoGreat_AshenGuardSeal" : "EchoDual_AshenParrySeal", MakeRingSprite("AshenEchoHitSeal", Color.white, heavy ? 156 : 112), enemy.transform.position, Quaternion.Euler(0f, 0f, elapsed * (heavy ? 70f : -90f)), heavy ? 0.72f : 0.42f, new Color(0.82f, 0.88f, 0.94f, heavy ? 0.60f : 0.48f), heavy ? 0.56f : 0.42f);
            SpawnEchoLink(heavy ? "EchoGreat_AshenCounterThread" : "EchoDual_AshenReturnThread", enemy.transform.position, player.position, new Color(0.84f, 0.90f, 1f, heavy ? 0.44f : 0.32f), heavy ? 0.48f : 0.34f, heavy ? 0.026f : 0.016f);
            SpawnPromptSprite(heavy ? "EchoGreat_AshenCounterWave" : "EchoDual_AshenGuard", EchoVfxSprite(V1MemoryId.AshenShield), () => MakeRingSprite("AshenEcho", Color.white, 112), player.position, Quaternion.identity, heavy ? 1.86f : 1.26f, heavy ? 0.76f : 0.52f, new Color(0.78f, 0.86f, 0.92f, heavy ? 0.64f : 0.52f), heavy ? 0.70f : 0.54f);
            SpawnTransientSprite(heavy ? "EchoGreat_AshenShieldBreakRing" : "EchoDual_AshenParryGuard", MakeRingSprite("AshenEchoGuard", Color.white, heavy ? 180 : 144), player.position, Quaternion.Euler(0f, 0f, elapsed * -120f), heavy ? 1.12f : 0.58f, new Color(0.86f, 0.92f, 1f, heavy ? 0.42f : 0.32f), heavy ? 0.58f : 0.42f);
            if (heavy)
            {
                var release = ashenStoredGuardCharge >= 18f + levelValue * 1.8f
                    ? ConsumeAshenGuardCharge(0.38f, 4f + levelValue * 0.6f)
                    : 0f;
                if (release > 0f)
                {
                    ReleaseAshenGuardWave(player.position, levelValue, release, true, "Ashen Echo stored wave");
                }
                foreach (var target in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(player.position, e.transform.position) <= EchoRadius(V1MemoryId.AshenShield, levelValue, heavy)).Take(EchoTargetLimit(V1MemoryId.AshenShield, levelValue, heavy)).ToList())
                {
                    var dir = (Vector2)(target.transform.position - player.position);
                    MarkEnemyEchoState(target, V1MemoryId.AshenShield, 1.35f, heavy ? 1.12f : 0.94f);
                    DealDamage(target, weapon.Damage * EchoDamageMultiplier(V1MemoryId.AshenShield, levelValue, heavy), "Ashen Echo Great", false, dir.sqrMagnitude > 0.01f ? dir.normalized : f, 0.42f);
                }
            }
            else
            {
                MarkEnemyEchoState(enemy, V1MemoryId.AshenShield, 1.05f, 0.92f);
                if (levelValue >= 5 && ashenStoredGuardCharge >= 12f)
                {
                    var release = ConsumeAshenGuardCharge(0.22f, 2f);
                    if (release > 0f)
                    {
                        ReleaseAshenGuardWave(enemy.transform.position, levelValue, release, false, "Ashen Echo quick release");
                    }
                }
                DealDamage(enemy, weapon.Damage * EchoDamageMultiplier(V1MemoryId.AshenShield, levelValue, heavy), "Ashen Echo Dual", false, f, 0.10f);
            }
        }

        void TriggerOblivionEcho(V1Enemy enemy, Vector2 forward, int hitIndex, WeaponRuntimeSpec weapon, bool force)
        {
            var levelValue = EchoLevel(V1MemoryId.OblivionBrand);
            if (levelValue <= 0) return;
            var heavy = IsHeavyEchoWeapon(weapon);
            var f = EchoForward(forward);
            if (!ShouldTriggerEcho(V1MemoryId.OblivionBrand, levelValue, hitIndex, force)) return;

            PlaySfx("brand", heavy ? 0.52f : 0.36f, heavy ? 0.16f : 0.10f);
            SpawnPromptSprite(heavy ? "EchoGreat_OblivionDetonation" : "EchoDual_OblivionBrandStack", EchoVfxSprite(V1MemoryId.OblivionBrand), () => MakeRingSprite("OblivionEcho", Color.white, 112), enemy.transform.position, Quaternion.identity, heavy ? 2.05f : 1.52f, heavy ? 1.02f : 0.74f, new Color(0.76f, 0.46f, 1f, heavy ? 0.86f : 0.76f), heavy ? 0.86f : 0.72f);
            SpawnTransientSprite(heavy ? "EchoGreat_OblivionCoreBreak" : "EchoDual_OblivionSlash", MakeImpactDiamondSprite("OblivionEchoSlash", Color.white), enemy.transform.position + Vector3.up * 0.08f, Quaternion.Euler(0f, 0f, elapsed * 140f), heavy ? 0.52f : 0.34f, new Color(0.96f, 0.72f, 1f, heavy ? 0.86f : 0.72f), heavy ? 0.44f : 0.34f);
            SpawnOblivionEchoBrand(enemy.transform.position, f, heavy ? 0.82f : 0.64f);
            if (heavy)
            {
                SpawnRadialSlashLines("EchoGreat_OblivionBrandBurst", enemy.transform.position, f, 5, 0.92f + levelValue * 0.08f, new Color(0.86f, 0.48f, 1f, 0.62f), 0.46f);
                foreach (var target in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(enemy.transform.position, e.transform.position) <= EchoRadius(V1MemoryId.OblivionBrand, levelValue, heavy)).Take(EchoTargetLimit(V1MemoryId.OblivionBrand, levelValue, heavy)).ToList())
                {
                    var dir = (Vector2)(target.transform.position - enemy.transform.position);
                    MarkEnemyEchoState(target, V1MemoryId.OblivionBrand, heavy ? 1.70f : 1.20f, heavy ? 1.12f : 0.96f);
                    DealDamage(target, weapon.Damage * EchoDamageMultiplier(V1MemoryId.OblivionBrand, levelValue, heavy), "Oblivion Echo Great", false, dir.sqrMagnitude > 0.01f ? dir.normalized : f, 0.24f);
                    if (levelValue >= 5)
                    {
                        SpawnOblivionEchoBrand(target.transform.position, dir.sqrMagnitude > 0.01f ? dir.normalized : f, 0.38f);
                        DealDamage(target, weapon.Damage * 0.22f, "Oblivion Echo rupture", false, dir.sqrMagnitude > 0.01f ? dir.normalized : f, 0.08f);
                    }
                }
            }
            else
            {
                for (int i = 0; i < Mathf.Min(4, 1 + levelValue / 2); i++)
                {
                    SpawnTransientSprite("EchoDual_OblivionStackPip", MakeImpactDiamondSprite("EchoDual_OblivionStackPip", Color.white), enemy.transform.position + Quaternion.Euler(0f, 0f, i * 90f + elapsed * 60f) * Vector3.right * 0.22f, Quaternion.Euler(0f, 0f, 45f + i * 30f), 0.16f, new Color(0.92f, 0.64f, 1f, 0.58f), 0.30f);
                }
                MarkEnemyEchoState(enemy, V1MemoryId.OblivionBrand, 1.20f, 0.96f);
                DealDamage(enemy, weapon.Damage * EchoDamageMultiplier(V1MemoryId.OblivionBrand, levelValue, heavy), "Oblivion Echo Dual", false);
                if (levelValue >= 5)
                {
                    foreach (var linked in enemies.Where(e => e != null && e.IsAlive && e != enemy && Vector2.Distance(enemy.transform.position, e.transform.position) <= 1.35f + levelValue * 0.10f).Take(3).ToList())
                    {
                        var dir = (Vector2)(linked.transform.position - enemy.transform.position);
                        MarkEnemyEchoState(linked, V1MemoryId.OblivionBrand, 1.05f, 0.90f);
                        SpawnEchoLink("EchoDual_OblivionSpreadLink", enemy.transform.position, linked.transform.position, new Color(0.82f, 0.46f, 1f, 0.38f), 0.24f, 0.014f);
                        DealDamage(linked, weapon.Damage * 0.16f, "Oblivion Echo spread", false, dir.sqrMagnitude > 0.01f ? dir.normalized : f, 0.06f);
                    }
                }
            }
        }

        void SpawnHunterGreatEchoSpear(V1Enemy target, Vector3 origin, int levelValue, WeaponRuntimeSpec weapon)
        {
            if (target == null || !target.IsAlive) return;
            PlaySfx("hunter", 0.42f, 0.10f);
            var toTarget = (Vector2)(target.transform.position - origin);
            var forward = toTarget.sqrMagnitude > 0.01f ? toTarget.normalized : Vector2.up;
            var angle = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg - 90f;
            var go = new GameObject("EchoGreat_HunterSpear");
            go.transform.position = origin - (Vector3)(forward * 0.22f);
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = EchoVfxSprite(V1MemoryId.HunterOath) ?? MakeCrescentSlashSprite("EchoGreat_HunterSpear", Color.white, false);
            go.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            go.transform.localScale = Vector3.one * (sr.sprite != null && sr.sprite.bounds.size.x > 2f ? ScaleSpriteToWorldWidth(sr.sprite, 1.24f) : 0.24f);
            sr.color = new Color(0.84f, 1f, 0.48f, 0.94f);
            sr.sortingOrder = 45;
            go.AddComponent<V1Projectile>().Configure(this, target, 6.8f + levelValue * 0.20f, weapon.Damage * (0.34f + levelValue * 0.075f), HunterEchoSource);
        }

        void SpawnRadialSlashLines(string name, Vector3 center, Vector2 forward, int count, float length, Color color, float lifetime)
        {
            var f = EchoForward(forward);
            var baseAngle = Mathf.Atan2(f.y, f.x) * Mathf.Rad2Deg - 90f;
            var line = MakeBoxSprite(name, Color.white, 7, 132);
            for (int i = 0; i < count; i++)
            {
                var spread = count <= 1 ? 0f : (i - (count - 1) * 0.5f) * (92f / Mathf.Max(1, count - 1));
                var rotation = Quaternion.Euler(0f, 0f, baseAngle + spread);
                var dir = rotation * Vector3.up;
                var pos = center + dir * (0.04f * Mathf.Abs(i - (count - 1) * 0.5f));
                var alpha = Mathf.Clamp01(color.a * (1f - i * 0.055f));
                SpawnTransientSpriteScaled(name, line, pos, rotation, new Vector3(0.026f + count * 0.002f, length * (0.84f + i * 0.035f), 1f), new Color(color.r, color.g, color.b, alpha), lifetime * (1f - i * 0.035f));
            }
        }

        void TriggerKalmuriEcho(V1Enemy center, Vector2 forward, int level, int hitIndex, WeaponRuntimeSpec weapon)
        {
            if (center == null) return;
            if (pendingKalmuriFollowups.Count >= KalmuriMaxPendingFollowups) return;
            var delay = weapon.FollowupBaseDelay + Mathf.Min(hitIndex, 2) * weapon.FollowupStagger;
            pendingKalmuriFollowups.Add(new PendingKalmuriFollowup(center.transform.position, forward.normalized, level, hitIndex, weapon, delay, DenseDualBladeVfxThrottle(weapon)));
        }

        void UpdatePendingKalmuriFollowups(float dt)
        {
            for (int i = pendingKalmuriFollowups.Count - 1; i >= 0; i--)
            {
                var followup = pendingKalmuriFollowups[i];
                followup.Delay -= dt;
                if (followup.Delay > 0f)
                {
                    pendingKalmuriFollowups[i] = followup;
                    continue;
                }

                pendingKalmuriFollowups.RemoveAt(i);
                ResolveKalmuriFollowup(followup.Origin, followup.Forward, followup.Level, followup.HitIndex, followup.Weapon, followup.DenseDualBlade);
            }
        }

        void ResolveKalmuriFollowup(Vector3 origin, Vector2 forward, int level, int hitIndex, WeaponRuntimeSpec weapon, bool denseDualBlade)
        {
            var isHeavy = weapon.EchoProcStyle == V1EchoProcStyle.SingleHeavy;
            var burstCount = denseDualBlade ? 1 : isHeavy ? 1 : Mathf.Clamp(1 + level / 2, 1, 3);
            var radius = (0.44f + level * 0.045f) * weapon.EchoSizeScale * (1f + WeaponStat.AreaMul * 0.55f);
            var damage = weapon.Damage * (0.22f + level * 0.045f) * weapon.EchoDamageScale * (1f + WeaponStat.EchoAmp);
            var f = forward.sqrMagnitude > 0.01f ? forward.normalized : lastAim.normalized;
            var baseAngle = Mathf.Atan2(f.y, f.x) * Mathf.Rad2Deg;
            var side = new Vector2(-f.y, f.x).normalized;
            if (isHeavy)
            {
                ResolveGreatswordKalmuriFollowup(origin, f, side, baseAngle, level, damage);
                return;
            }

            var entries = weapon.VfxProfile != null ? weapon.VfxProfile.kalmuriFollowupSlashes : Array.Empty<SlashVfxEntry>();
            var ringScale = Mathf.Clamp(radius * 0.88f, 0.42f, isHeavy ? 0.92f : 0.66f);
            var ringColor = isHeavy ? new Color(0.92f, 0.98f, 1f, 0.34f) : new Color(0.58f, 0.96f, 1f, 0.24f);
            PlaySfx(isHeavy ? "kalmuri_echo_heavy" : "kalmuri_echo", isHeavy ? 0.82f : 0.58f, isHeavy ? 0.10f : 0.055f);
            if (!denseDualBlade)
            {
                SpawnTransientSprite("KalmuriEchoRange", MakeRingSprite("KalmuriEchoRange", Color.white, 128), origin, Quaternion.identity, ringScale, ringColor, isHeavy ? 0.18f : 0.12f);
            }
            SpawnKalmuriEchoClampRip(origin, f, side, level, hitIndex, isHeavy, denseDualBlade);
            if (!denseDualBlade)
            {
                SpawnTransientSprite("KalmuriEchoFlash", MakeRingSprite("KalmuriEchoFlash", Color.white, 96), origin, Quaternion.Euler(0f, 0f, baseAngle), Mathf.Clamp(radius * 0.42f, 0.26f, 0.48f), new Color(0.86f, 1f, 1f, 0.26f), isHeavy ? 0.14f : 0.10f);
            }
            if (!denseDualBlade)
            {
                SpawnEchoWoundSlash("KalmuriEchoCutTrace", origin, f, new Color(0.78f, 1f, 1f, isHeavy ? 0.58f : 0.36f), isHeavy ? 1.02f : 0.58f, isHeavy ? 0.18f : 0.10f);
            }
            var surgeCount = isHeavy ? Mathf.Clamp(4 + level / 4, 5, 5) : Mathf.Clamp(3 + level / 3, 4, 4);
            if (denseDualBlade) surgeCount = 0;
            for (int i = 0; i < surgeCount; i++)
            {
                var spread = (i - (surgeCount - 1) * 0.5f) * (isHeavy ? 0.13f : 0.10f);
                var lane = i % 3;
                var start = origin - (Vector3)(f * (isHeavy ? 0.62f : 0.46f) + side * spread * 1.35f);
                var end = origin + (Vector3)(f * (isHeavy ? 0.30f : 0.20f) + side * (spread * 0.22f + Mathf.Sin(i * 1.7f + hitIndex) * 0.05f));
                var scale = (isHeavy ? 0.27f : 0.20f) + level * (isHeavy ? 0.012f : 0.010f) + lane * 0.010f;
                var color = isHeavy
                    ? new Color(0.92f, 1f, 1f, 0.92f - lane * 0.06f)
                    : new Color(0.78f, 1f, 1f, 0.78f - lane * 0.06f);
                SpawnKalmuriDiveBlade(isHeavy ? "KalmuriEchoHeavySurgeBlade" : "KalmuriEchoSurgeBlade", start, end, scale, color, isHeavy ? 0.18f : 0.12f, isHeavy ? 0.12f : 0.08f);
            }
            if (!denseDualBlade)
            {
                SpawnKalmuriEchoBarrage(origin, f, side, baseAngle, level, isHeavy);
            }

            if (!denseDualBlade)
            {
                for (int i = 0; i < burstCount; i++)
                {
                    var offset = isHeavy ? Vector2.zero : side * ((i - (burstCount - 1) * 0.5f) * 0.18f);
                    var pos = origin + (Vector3)(f * (isHeavy ? 0.04f : 0.12f + i * 0.06f) + offset);
                    foreach (var entry in entries)
                    {
                        if (entry == null) continue;
                        var angle = baseAngle + (isHeavy ? 0f : (i - 1) * 18f + hitIndex * 7f);
                        SpawnSlashEntry(entry, pos, pos, f, angle, true, i);
                    }
                }
            }

            if (isHeavy)
            {
                hitstopTimer = Mathf.Max(hitstopTimer, 0.045f);
                cameraShakeTimer = Mathf.Max(cameraShakeTimer, 0.10f);
                cameraShakeAmount = Mathf.Max(cameraShakeAmount, 0.05f);
            }

            var targets = enemies
                .Where(e => e != null && e.IsAlive && Vector2.Distance(origin, e.transform.position) <= radius + e.TouchRadius)
                .OrderBy(e => Vector2.Distance(origin, e.transform.position))
                .Take(isHeavy ? 4 : level >= 5 ? 3 : 2)
                .ToList();

            for (int i = 0; i < targets.Count; i++)
            {
                var toTarget = (Vector2)(targets[i].transform.position - origin);
                var mul = i == 0 ? 1f : 0.55f;
                DealDamage(targets[i], damage * mul, "칼무리 잔향", true, toTarget.sqrMagnitude > 0.01f ? toTarget.normalized : f, 0.28f);
            }
        }

        void ResolveGreatswordKalmuriFollowup(Vector3 origin, Vector2 forward, Vector2 side, float baseAngle, int level, float damage)
        {
            var f = forward.sqrMagnitude > 0.01f ? forward.normalized : lastAim.normalized;
            if (f.sqrMagnitude < 0.01f) f = Vector2.up;
            var s = side.sqrMagnitude > 0.01f ? side.normalized : new Vector2(-f.y, f.x);
            var radius = (0.72f + level * 0.085f) * (1f + WeaponStat.AreaMul * 0.55f);
            var dropStart = origin - (Vector3)(f * (0.78f + level * 0.035f)) + Vector3.up * (0.86f + level * 0.035f);
            var bitePoint = origin + (Vector3)(f * 0.16f);
            var exit = origin + (Vector3)(f * (0.58f + level * 0.055f));
            var bladeScale = 0.42f + level * 0.026f;
            var hot = new Color(0.96f, 1f, 1f, 0.94f);
            var ghost = new Color(0.48f, 0.92f, 1f, 0.36f);

            PlaySfx("kalmuri_echo_heavy", 0.90f, 0.08f);
            SpawnTransientSprite("EchoGreat_KalmuriJudgementTell", MakeRingSprite("EchoGreat_KalmuriJudgementTell", Color.white, 160), bitePoint, Quaternion.Euler(0f, 0f, baseAngle), radius * 0.82f, new Color(0.72f, 0.98f, 1f, 0.30f), 0.28f);
            SpawnEchoLink("EchoGreat_KalmuriDropLine", dropStart, bitePoint, new Color(0.82f, 1f, 1f, 0.42f), 0.26f, 0.034f);
            SpawnKalmuriDiveBlade("EchoGreat_KalmuriFallingGreatBlade", dropStart, bitePoint, bladeScale, hot, 0.36f, 0.18f);
            SpawnKalmuriDiveBlade("EchoGreat_KalmuriGroundRipBlade", bitePoint - (Vector3)(f * 0.04f), exit, bladeScale * 0.82f, ghost, 0.26f, 0.13f);
            SpawnEchoWoundSlash("EchoGreat_KalmuriExecutionRift", bitePoint, f, new Color(0.90f, 1f, 1f, 0.84f), 1.42f + level * 0.10f, 0.28f);
            SpawnEchoWoundSlash("EchoGreat_KalmuriCrossRift", bitePoint + (Vector3)(s * 0.08f), s, new Color(0.56f, 0.94f, 1f, 0.52f), 0.84f + level * 0.055f, 0.22f);
            SpawnRadialSlashLines("EchoGreat_KalmuriGroundTeeth", bitePoint, f, 5, 0.70f + level * 0.06f, new Color(0.74f, 0.98f, 1f, 0.48f), 0.30f);
            SpawnTransientSprite("EchoGreat_KalmuriImpactCore", MakeImpactDiamondSprite("EchoGreat_KalmuriImpactCore", Color.white), bitePoint, Quaternion.Euler(0f, 0f, baseAngle + 45f), 0.44f + level * 0.024f, new Color(0.94f, 1f, 1f, 0.72f), 0.22f);

            var targets = enemies
                .Where(e => e != null && e.IsAlive)
                .Select(e => new { Enemy = e, Dir = (Vector2)(e.transform.position - bitePoint) })
                .Where(x => x.Dir.magnitude <= radius + x.Enemy.TouchRadius)
                .Where(x => x.Dir.sqrMagnitude <= 0.001f || Vector2.Angle(f, x.Dir.normalized) <= 62f)
                .OrderBy(x => Mathf.Abs(Vector2.SignedAngle(f, x.Dir.sqrMagnitude > 0.001f ? x.Dir.normalized : f)))
                .ThenBy(x => x.Dir.sqrMagnitude)
                .Take(5)
                .ToList();

            for (int i = 0; i < targets.Count; i++)
            {
                var dir = targets[i].Dir.sqrMagnitude > 0.01f ? targets[i].Dir.normalized : f;
                var mul = i == 0 ? 1.35f : 0.78f;
                DealDamage(targets[i].Enemy, damage * mul, "Kalmuri Greatsword judgement", true, dir, i == 0 ? 0.82f : 0.44f);
            }

            hitstopTimer = Mathf.Max(hitstopTimer, 0.060f);
            cameraShakeTimer = Mathf.Max(cameraShakeTimer, 0.14f);
            cameraShakeAmount = Mathf.Max(cameraShakeAmount, 0.070f);
        }

        void SpawnKalmuriEchoClampRip(Vector3 origin, Vector2 forward, Vector2 side, int levelValue, int hitIndex, bool isHeavy, bool denseDualBlade)
        {
            var f = forward.sqrMagnitude > 0.01f ? forward.normalized : lastAim.normalized;
            if (f.sqrMagnitude < 0.01f) f = Vector2.up;
            var s = side.sqrMagnitude > 0.01f ? side.normalized : new Vector2(-f.y, f.x);
            var baseAngle = Mathf.Atan2(f.y, f.x) * Mathf.Rad2Deg;
            var lifetime = isHeavy ? 0.26f : 0.18f;
            var width = (isHeavy ? 0.56f : 0.40f) + levelValue * (isHeavy ? 0.018f : 0.012f);
            var depth = isHeavy ? 0.44f : 0.32f;
            var overrun = isHeavy ? 0.18f : 0.12f;
            var scale = (isHeavy ? 0.30f : 0.21f) + levelValue * (isHeavy ? 0.011f : 0.008f);
            var color = isHeavy ? new Color(0.95f, 1f, 1f, 0.94f) : new Color(0.78f, 1f, 1f, 0.82f);
            var pairCount = denseDualBlade ? 1 : isHeavy ? 2 : Mathf.Clamp(1 + levelValue / 4, 1, 2);

            if (denseDualBlade)
            {
                SpawnKalmuriBlade("KalmuriEchoClampBlade", origin + (Vector3)(s * 0.16f + f * 0.02f), baseAngle + 122f, scale * 0.90f, new Color(color.r, color.g, color.b, 0.72f), lifetime * 0.72f);
                SpawnKalmuriBlade("KalmuriEchoClampBlade", origin - (Vector3)(s * 0.16f - f * 0.02f), baseAngle - 122f, scale * 0.90f, new Color(color.r, color.g, color.b, 0.72f), lifetime * 0.72f);
                SpawnEchoWoundLine("KalmuriEchoRipTrace", origin + (Vector3)(f * 0.03f), f, new Color(0.92f, 1f, 1f, 0.52f), 0.66f, 0.10f);
                return;
            }

            if (!denseDualBlade)
            {
                SpawnTransientSprite("KalmuriEchoBiteCore", MakeImpactDiamondSprite("KalmuriEchoBiteCore", Color.white), origin + (Vector3)(f * 0.03f), Quaternion.Euler(0f, 0f, baseAngle + 45f), isHeavy ? 0.38f : 0.28f, new Color(0.92f, 1f, 1f, isHeavy ? 0.58f : 0.44f), lifetime * 0.72f);
            }
            if (!denseDualBlade)
            {
                SpawnTransientSprite("KalmuriEchoBiteJawRing", MakeRingSprite("KalmuriEchoBiteJawRing", Color.white, 112), origin, Quaternion.Euler(0f, 0f, baseAngle), isHeavy ? 0.58f : 0.42f, new Color(0.76f, 1f, 1f, isHeavy ? 0.34f : 0.26f), lifetime);
            }

            for (int i = 0; i < pairCount; i++)
            {
                var lane = (i - (pairCount - 1) * 0.5f) * 0.14f;
                var leftStart = origin + (Vector3)(s * (width + lane) - f * depth);
                var rightStart = origin - (Vector3)(s * (width - lane) + f * depth);
                var leftEnd = origin + (Vector3)(-s * 0.08f + f * overrun + s * lane * 0.30f);
                var rightEnd = origin + (Vector3)(s * 0.08f + f * overrun + s * lane * 0.30f);
                var bladeScale = scale + i * 0.018f;
                var bladeColor = i == 0 ? color : new Color(color.r, color.g, color.b, color.a * 0.72f);
                SpawnKalmuriDiveBlade(isHeavy ? "KalmuriEchoHeavyClampBlade" : "KalmuriEchoClampBlade", leftStart, leftEnd, bladeScale, bladeColor, lifetime, isHeavy ? 0.12f : 0.085f);
                SpawnKalmuriDiveBlade(isHeavy ? "KalmuriEchoHeavyClampBlade" : "KalmuriEchoClampBlade", rightStart, rightEnd, bladeScale, bladeColor, lifetime, isHeavy ? 0.12f : 0.085f);
            }

            SpawnEchoWoundSlash("KalmuriEchoRipTrace", origin + (Vector3)(f * 0.04f), f, new Color(0.92f, 1f, 1f, isHeavy ? 0.82f : 0.62f), isHeavy ? 1.22f : 0.82f, isHeavy ? 0.24f : 0.16f);
            if (isHeavy && !denseDualBlade)
            {
                SpawnEchoWoundSlash("KalmuriEchoHeavyReturnRip", origin - (Vector3)(f * 0.04f), s, new Color(0.62f, 0.96f, 1f, 0.58f), 0.86f, 0.18f);
            }
        }

        void SpawnKalmuriEchoBarrage(Vector3 origin, Vector2 forward, Vector2 side, float baseAngle, int levelValue, bool isHeavy)
        {
            var bladeCount = isHeavy ? Mathf.Clamp(4 + levelValue / 4, 5, 5) : Mathf.Clamp(3 + levelValue / 3, 4, 4);
            var scale = isHeavy ? 0.23f : 0.170f + levelValue * 0.012f;
            var lifetime = isHeavy ? 0.20f : 0.14f;
            var color = isHeavy ? new Color(0.96f, 1f, 1f, 0.90f) : new Color(0.74f, 0.98f, 1f, 0.84f);
            for (int i = 0; i < bladeCount; i++)
            {
                var spread = (i - (bladeCount - 1) * 0.5f) * (isHeavy ? 0.16f : 0.11f);
                var depth = isHeavy ? Mathf.Sin(i * 1.7f) * 0.12f : (i % 3 - 1) * 0.07f;
                var pos = origin + (Vector3)(forward * depth + side * spread);
                SpawnKalmuriBlade("KalmuriEchoBarrage", pos, baseAngle + 92f + spread * 70f, scale, color, lifetime);
            }
        }

        void UpdateEchoUltimate(float dt)
        {
            if (!AnyUltimateReady) return;

            var weapon = CurrentWeaponSpec();
            ultimatePulseTimer -= dt;
            if (!BloodBladeStormReady)
            {
                bloodStormWasReady = false;
                bloodStormBurstTimer = 0f;
                UpdateUtilityUltimate(weapon, dt);
                return;
            }

            UpdateBloodBladeStorm(dt, weapon);
        }

        void UpdateBloodBladeStorm(float dt, WeaponRuntimeSpec weapon)
        {
            var heavy = weapon.UltimatePattern == V1UltimatePattern.FewHeavy;
            if (!bloodStormWasReady)
            {
                bloodStormWasReady = true;
                bloodStormBurstTimer = 0f;
                SpawnBloodStormOpening(heavy);
            }

            bloodStormBurstTimer -= dt;
            if (heavy)
            {
                if (ultimatePulseTimer <= 0f)
                {
                    ultimatePulseTimer = 0.42f;
                    var baseAngle = elapsed * 120f;
                    var ultimateEntries = weapon.VfxProfile != null ? weapon.VfxProfile.ultimateSlashes : Array.Empty<SlashVfxEntry>();
                    PlaySfx("blood_storm", 0.44f, 0.20f);
                    SpawnPromptSprite("BloodBladeStormHeavyRing", LoadSprite(BloodBladeStormRingPath), () => MakeRingSprite("BloodBladeStormHeavyRing", Color.white, 180), player.position, Quaternion.Euler(0f, 0f, baseAngle), 5.45f, 1.70f, new Color(1f, 0.10f, 0.16f, 0.34f), 0.28f);
                    for (int i = 0; i < 4; i++)
                    {
                        var angle = baseAngle + i * 90f;
                        var pos = player.position + Quaternion.Euler(0f, 0f, angle) * Vector3.right * 1.95f;
                        var f = ((Vector2)(pos - player.position)).normalized;
                        foreach (var entry in ultimateEntries)
                        {
                            if (entry == null) continue;
                            SpawnSlashEntry(entry, pos, pos, f, angle + 90f, true, i);
                        }
                        SpawnPromptSprite("BloodBladeStormHeavyBlade", LoadSprite(BloodBladeStormBladePath), () => KalmuriBladeSprite(), pos, Quaternion.Euler(0f, 0f, angle + 90f), 1.10f, 0.42f, new Color(1f, 0.24f, 0.30f, 0.86f), 0.26f);
                    }
                    hitstopTimer = Mathf.Max(hitstopTimer, 0.052f);
                    cameraShakeTimer = Mathf.Max(cameraShakeTimer, 0.20f);
                    cameraShakeAmount = Mathf.Max(cameraShakeAmount, 0.10f);
                }

                ApplyBloodStormPressure(4.25f, 16, 66f * dt, 1.55f, dt);
                if (bloodStormBurstTimer <= 0f)
                {
                    bloodStormBurstTimer = 0.92f;
                    BloodStormBurst(true);
                }
                return;
            }

            if (ultimatePulseTimer <= 0f)
            {
                ultimatePulseTimer = 0.065f;
                PlaySfx("blood_storm", 0.24f, 0.12f);
                var spin = elapsed * 310f;
                for (int i = 0; i < 10; i++)
                {
                    var angle = spin + i * 36f;
                    var radius = 2.05f + (i & 1) * 0.34f;
                    var pos = player.position + Quaternion.Euler(0f, 0f, angle) * Vector3.right * radius;
                    SpawnPromptSprite("BloodBladeStormFastBlade", LoadSprite(BloodBladeStormBladePath), () => KalmuriBladeSprite(), pos, Quaternion.Euler(0f, 0f, angle + 90f), 0.72f, 0.30f, new Color(1f, 0.24f, 0.34f, 0.78f), 0.14f);
                }
            }

            ApplyBloodStormPressure(3.75f, 16, 32f * dt, 1.00f, dt);
            if (bloodStormBurstTimer <= 0f)
            {
                bloodStormBurstTimer = 0.62f;
                BloodStormBurst(false);
            }
        }

        void SpawnBloodStormOpening(bool heavy)
        {
            PlaySfx("blood_storm", heavy ? 0.92f : 0.76f, 0.18f);
            SpawnPromptSprite("BloodBladeStormOpen", LoadSprite(BloodBladeStormRingPath), () => MakeRingSprite("BloodBladeStormOpen", Color.white, 180), player.position, Quaternion.identity, heavy ? 6.20f : 5.20f, heavy ? 1.96f : 1.62f, new Color(1f, 0.08f, 0.16f, 0.54f), 0.56f);
            SpawnTransientSprite("BloodBladeStormWarning", MakeRingSprite("BloodBladeStormWarning", Color.white, 156), player.position, Quaternion.identity, heavy ? 1.92f : 1.56f, new Color(0.72f, 0.98f, 1f, 0.36f), 0.38f);
            SpawnFloatingText(player.position + Vector3.up * 1.05f, "피의 칼폭풍", new Color(1f, 0.18f, 0.24f));
            cameraShakeTimer = Mathf.Max(cameraShakeTimer, 0.24f);
            cameraShakeAmount = Mathf.Max(cameraShakeAmount, heavy ? 0.13f : 0.095f);
        }

        void BloodStormBurst(bool heavy)
        {
            var center = player.position;
            var radius = heavy ? 4.75f : 4.05f;
            var burstDamage = heavy ? 84f : 52f;
            PlaySfx("blood_storm", heavy ? 0.86f : 0.64f, heavy ? 0.28f : 0.18f);
            SpawnPromptSprite("BloodBladeStormBurst", LoadSprite(BloodBladeStormRingPath), () => MakeRingSprite("BloodBladeStormBurst", Color.white, 180), center, Quaternion.Euler(0f, 0f, elapsed * (heavy ? -72f : 160f)), heavy ? 6.35f : 5.40f, heavy ? 2.05f : 1.72f, new Color(1f, 0.08f, 0.13f, heavy ? 0.66f : 0.56f), heavy ? 0.44f : 0.32f);
            var bladeCount = heavy ? 8 : 14;
            for (int i = 0; i < bladeCount; i++)
            {
                var angle = elapsed * (heavy ? 84f : 220f) + i * (360f / bladeCount);
                var pos = center + Quaternion.Euler(0f, 0f, angle) * Vector3.right * (heavy ? 2.48f : 2.22f);
                SpawnPromptSprite("BloodBladeStormBurstBlade", LoadSprite(BloodBladeStormBladePath), () => KalmuriBladeSprite(), pos, Quaternion.Euler(0f, 0f, angle + 90f), heavy ? 1.18f : 0.82f, heavy ? 0.46f : 0.32f, new Color(1f, 0.22f, 0.28f, 0.90f), heavy ? 0.34f : 0.22f);
            }

            var victims = enemies
                .Where(e => e != null && e.IsAlive && Vector2.Distance(center, e.transform.position) <= radius)
                .OrderBy(e => Vector2.Distance(center, e.transform.position))
                .Take(heavy ? 18 : 16)
                .ToList();

            foreach (var enemy in victims)
            {
                var dir = (Vector2)(enemy.transform.position - center);
                if (dir.sqrMagnitude < 0.001f) dir = UnityEngine.Random.insideUnitCircle.normalized;
                enemy.BloodMarked = true;
                enemy.ApplyBriefFreeze(heavy ? 0.055f : 0.035f);
                enemy.ApplyHitFeedback(dir.normalized, heavy ? 3.3f : 2.2f);
                DealDamage(enemy, burstDamage, "BloodBladeStorm", false, dir.normalized, 0f);
            }

            foreach (var enemy in victims.Take(heavy ? 5 : 4))
            {
                if (enemy != null)
                {
                    SpawnBloodThread(enemy.transform.position, heavy ? 2.0f : 1.35f, MaxEchoLevel);
                }
            }

            HealPlayer(heavy ? 5.2f : 3.6f);
            hitstopTimer = Mathf.Max(hitstopTimer, heavy ? 0.065f : 0.040f);
            cameraShakeTimer = Mathf.Max(cameraShakeTimer, heavy ? 0.26f : 0.17f);
            cameraShakeAmount = Mathf.Max(cameraShakeAmount, heavy ? 0.125f : 0.088f);
        }

        void ApplyBloodStormPressure(float radius, int cap, float damage, float pullStrength, float dt)
        {
            var center = player.position;
            foreach (var enemy in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(center, e.transform.position) < radius).Take(cap).ToList())
            {
                var outward = (Vector2)(enemy.transform.position - center);
                if (outward.sqrMagnitude < 0.001f) outward = UnityEngine.Random.insideUnitCircle.normalized;
                enemy.BloodMarked = true;
                enemy.ApplyHitFeedback(-outward.normalized, pullStrength * dt);
                DealDamage(enemy, damage, "BloodBladeStorm", false, outward.normalized, 0f);
            }
        }
        bool RunWeaponPatternUtilityUltimate(WeaponRuntimeSpec weapon, float dt)
        {
            var heavy = weapon.UltimatePattern == V1UltimatePattern.FewHeavy;
            if (FractureExecutionReady)
            {
                if (ultimatePulseTimer <= 0f)
                {
                    SpawnFractureExecutionUltimate(heavy);
                    ultimatePulseTimer = heavy ? 0.86f : 0.28f;
                }
                return true;
            }

            if (StasisHuntReady)
            {
                if (ultimatePulseTimer <= 0f)
                {
                    SpawnStasisHuntUltimate(heavy);
                    ultimatePulseTimer = heavy ? 0.78f : 0.25f;
                }
                return true;
            }

            if (AshenOblivionReady)
            {
                if (ultimatePulseTimer <= 0f)
                {
                    SpawnAshenOblivionUltimate(heavy);
                    ultimatePulseTimer = heavy ? 0.94f : 0.38f;
                }
                return true;
            }

            return false;
        }

        void SpawnFractureExecutionUltimate(bool heavy)
        {
            var target = enemies.Where(e => e != null && e.IsAlive).OrderBy(e => e.HealthRatio).FirstOrDefault();
            if (target == null) return;

            var forward = (Vector2)(target.transform.position - player.position);
            if (forward.sqrMagnitude < 0.01f) forward = lastAim.sqrMagnitude > 0.01f ? lastAim : Vector2.up;
            forward.Normalize();
            PlaySfx("execution", heavy ? 0.96f : 0.70f, 0.16f);

            if (heavy)
            {
                SpawnPromptSprite("UltGreat_FractureExecutionStamp", LoadSprite(UltimateFracturePath), () => MakeRingSprite("UltGreat_FractureExecutionStamp", Color.white, 180), target.transform.position, Quaternion.identity, 3.32f, 1.30f, new Color(1f, 0.90f, 0.48f, 0.78f), 0.62f);
                SpawnEchoWoundSlash("UltGreat_FractureExecutionCleave", target.transform.position, forward, new Color(1f, 0.94f, 0.54f, 0.80f), 1.65f, 0.54f);
                SpawnRadialSlashLines("UltGreat_FractureExecutionVerdict", target.transform.position, forward, 6, 1.18f, new Color(1f, 0.88f, 0.46f, 0.64f), 0.50f);
                foreach (var enemy in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(target.transform.position, e.transform.position) < 2.30f).Take(12).ToList())
                {
                    var dir = (Vector2)(enemy.transform.position - target.transform.position);
                    MarkEnemyEchoState(enemy, V1MemoryId.ExecutionFlash, 1.60f, 1.22f);
                    MarkEnemyEchoState(enemy, V1MemoryId.OblivionBrand, 1.25f, 1.08f);
                    DealDamage(enemy, enemy.HealthRatio < 0.50f ? 158f : 78f, "UltGreat FractureExecution", false, dir.sqrMagnitude > 0.01f ? dir.normalized : forward, 1.42f);
                    if (!enemy.IsAlive)
                    {
                        SpawnRadialSlashLines("UltGreat_FractureExecutionChainBreak", target.transform.position, dir.sqrMagnitude > 0.01f ? dir.normalized : forward, 5, 0.82f, new Color(1f, 0.86f, 0.42f, 0.58f), 0.34f);
                    }
                }
                hitstopTimer = Mathf.Max(hitstopTimer, 0.085f);
                cameraShakeTimer = Mathf.Max(cameraShakeTimer, 0.26f);
                cameraShakeAmount = Mathf.Max(cameraShakeAmount, 0.14f);
                return;
            }

            SpawnPromptSprite("UltDual_FractureExecutionRing", LoadSprite(UltimateFracturePath), () => MakeRingSprite("UltDual_FractureExecutionRing", Color.white, 144), target.transform.position, Quaternion.Euler(0f, 0f, elapsed * 200f), 2.30f, 0.90f, new Color(1f, 0.96f, 0.62f, 0.64f), 0.36f);
            var cutTargets = enemies.Where(e => e != null && e.IsAlive).OrderBy(e => e.HealthRatio).Take(9).ToList();
            for (int i = 0; i < cutTargets.Count; i++)
            {
                var enemy = cutTargets[i];
                var dir = (Vector2)(enemy.transform.position - player.position);
                if (dir.sqrMagnitude < 0.01f) dir = Quaternion.Euler(0f, 0f, i * 60f) * Vector2.up;
                SpawnEchoWoundSlash("UltDual_FractureExecutionCut", enemy.transform.position, dir.normalized, new Color(1f, 0.96f, 0.64f, 0.70f), 0.76f, 0.32f);
                SpawnTransientSprite("UltDual_FractureExecutionMark", MakeImpactDiamondSprite("UltDual_FractureExecutionMark", Color.white), enemy.transform.position, Quaternion.Euler(0f, 0f, elapsed * 230f + i * 37f), 0.30f, new Color(1f, 0.94f, 0.54f, 0.80f), 0.26f);
                MarkEnemyEchoState(enemy, V1MemoryId.ExecutionFlash, 1.20f, 1.02f);
                MarkEnemyEchoState(enemy, V1MemoryId.OblivionBrand, 1.05f, 0.96f);
                DealDamage(enemy, enemy.HealthRatio < 0.44f ? 96f : 46f, "UltDual FractureExecution", false, dir.normalized, 0.56f);
            }
            hitstopTimer = Mathf.Max(hitstopTimer, 0.035f);
        }

        void SpawnStasisHuntUltimate(bool heavy)
        {
            PlaySfx("stopped", heavy ? 0.82f : 0.62f, 0.18f);
            PlaySfx("hunter", heavy ? 0.44f : 0.58f, 0.10f);

            if (heavy)
            {
                var focus = enemies.Where(e => e != null && e.IsAlive).OrderBy(e => Vector2.Distance(player.position, e.transform.position)).FirstOrDefault();
                var center = focus != null ? focus.transform.position : player.position;
                SpawnPromptSprite("UltGreat_StasisHuntDome", LoadSprite(UltimateStasisPath), () => MakeRingSprite("UltGreat_StasisHuntDome", Color.white, 180), center, Quaternion.identity, 4.10f, 1.54f, new Color(1f, 0.74f, 0.28f, 0.66f), 1.08f);
                SpawnStoppedSecondField(center, 2.85f, TimeStopGold(true), 1.95f, true);
                foreach (var enemy in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(center, e.transform.position) <= 3.15f).Take(12).ToList())
                {
                    var dir = (Vector2)(enemy.transform.position - center);
                    enemy.ApplyBriefFreeze(0.92f);
                    MarkEnemyEchoState(enemy, V1MemoryId.StoppedSecond, 1.95f, 1.14f);
                    MarkEnemyEchoState(enemy, V1MemoryId.HunterOath, 1.42f, 1.06f);
                    SpawnEchoWoundSlash("UltGreat_StasisHuntFrozenCleave", enemy.transform.position, dir.sqrMagnitude > 0.01f ? dir.normalized : lastAim, new Color(0.92f, 0.84f, 1f, 0.66f), 1.16f, 0.48f);
                    SpawnStoppedFractureBurst(enemy.transform.position, dir.sqrMagnitude > 0.01f ? dir.normalized : Vector2.up, 1.20f, true);
                    DealDamage(enemy, 78f, "UltGreat StasisHunt", false, dir.sqrMagnitude > 0.01f ? dir.normalized : Vector2.up, 1.00f);
                }
                SpawnTransientSpriteScaled("UltGreat_StasisHuntSpear", MakeBoxSprite("UltGreat_StasisHuntSpear", Color.white, 8, 150), center + Vector3.up * 0.15f, Quaternion.Euler(0f, 0f, elapsed * 60f), new Vector3(0.036f, 1.08f, 1f), new Color(0.86f, 0.78f, 1f, 0.78f), 0.60f);
                hitstopTimer = Mathf.Max(hitstopTimer, 0.060f);
                return;
            }

            SpawnPromptSprite("UltDual_StasisHuntField", LoadSprite(UltimateStasisPath), () => MakeRingSprite("UltDual_StasisHuntField", Color.white, 144), player.position, Quaternion.Euler(0f, 0f, elapsed * 150f), 2.85f, 1.06f, new Color(1f, 0.78f, 0.30f, 0.60f), 0.72f);
            SpawnStoppedSecondField(player.position, 1.92f, TimeStopGold(true), 1.34f, true);
            var shotTargets = SelectHunterTargets(player.position, null, 10);
            for (int i = 0; i < shotTargets.Count; i++)
            {
                var enemy = shotTargets[i];
                enemy.ApplyBriefFreeze(0.40f);
                MarkEnemyEchoState(enemy, V1MemoryId.StoppedSecond, 1.20f, 0.96f);
                MarkEnemyEchoState(enemy, V1MemoryId.HunterOath, 1.10f, 0.92f);
                SpawnTransientSprite("UltDual_StasisHuntMicroClamp", MakeRingSprite("UltDual_StasisHuntMicroClamp", Color.white, 96), enemy.transform.position, Quaternion.identity, 0.52f, new Color(1f, 0.78f, 0.34f, 0.46f), 0.28f);
                SpawnStoppedFractureBurst(enemy.transform.position, (Vector2)(enemy.transform.position - player.position), 0.76f, false);
                SpawnHunterOathShot(enemy, player.position, i, shotTargets.Count, 14.8f, 36f, "UltDual StasisHunt", true);
            }
        }

        void SpawnAshenOblivionUltimate(bool heavy)
        {
            HealPlayer(heavy ? 7.2f : 4.3f);
            PlaySfx("ashen", heavy ? 0.78f : 0.58f, 0.18f);
            PlaySfx("brand", heavy ? 0.56f : 0.50f, 0.18f);
            var storedRelease = ConsumeAshenGuardCharge(heavy ? 0.90f : 0.62f, heavy ? 10f : 5f);

            if (heavy)
            {
                SpawnPromptSprite("UltGreat_AshenOblivionWave", LoadSprite(UltimateAshenOblivionPath), () => MakeRingSprite("UltGreat_AshenOblivionWave", Color.white, 180), player.position, Quaternion.identity, 4.40f, 1.66f, new Color(0.78f, 0.68f, 1f, 0.70f), 0.94f);
                SpawnTransientSprite("UltGreat_AshenOblivionGuardBreak", MakeRingSprite("UltGreat_AshenOblivionGuardBreak", Color.white, 180), player.position, Quaternion.Euler(0f, 0f, elapsed * -80f), 2.22f, new Color(0.88f, 0.92f, 1f, 0.52f), 0.72f);
                SpawnRadialSlashLines("UltGreat_AshenOblivionGuardBreakLine", player.position, lastAim, 6, 1.05f, new Color(0.86f, 0.76f, 1f, 0.58f), 0.48f);
                ReleaseAshenGuardWave(player.position, MaxEchoLevel, storedRelease + 16f, true, "UltGreat AshenOblivion stored wave");
                foreach (var enemy in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(player.position, e.transform.position) < 4.15f).Take(14).ToList())
                {
                    var dir = (Vector2)(enemy.transform.position - player.position);
                    SpawnEchoLink("UltGreat_AshenOblivionBreakBrand", player.position, enemy.transform.position, new Color(0.76f, 0.52f, 1f, 0.38f), 0.42f, 0.020f);
                    MarkEnemyEchoState(enemy, V1MemoryId.AshenShield, 1.40f, 1.10f);
                    MarkEnemyEchoState(enemy, V1MemoryId.OblivionBrand, 1.55f, 1.12f);
                    DealDamage(enemy, 78f, "UltGreat AshenOblivion", false, dir.sqrMagnitude > 0.01f ? dir.normalized : Vector2.up, 1.65f);
                }
                cameraShakeTimer = Mathf.Max(cameraShakeTimer, 0.20f);
                cameraShakeAmount = Mathf.Max(cameraShakeAmount, 0.095f);
                return;
            }

            SpawnPromptSprite("UltDual_AshenOblivionGuard", LoadSprite(UltimateAshenOblivionPath), () => MakeRingSprite("UltDual_AshenOblivionGuard", Color.white, 150), player.position, Quaternion.Euler(0f, 0f, elapsed * 165f), 2.90f, 1.12f, new Color(0.82f, 0.72f, 1f, 0.58f), 0.58f);
            SpawnTransientSprite("UltDual_AshenOblivionReturnRing", MakeRingSprite("UltDual_AshenOblivionReturnRing", Color.white, 128), player.position, Quaternion.Euler(0f, 0f, elapsed * -200f), 1.24f, new Color(0.90f, 0.96f, 1f, 0.44f), 0.44f);
            ReleaseAshenGuardWave(player.position, MaxEchoLevel, storedRelease + 7f, false, "UltDual AshenOblivion stored wave");
            var targets = enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(player.position, e.transform.position) < 3.65f).OrderBy(e => Vector2.Distance(player.position, e.transform.position)).Take(9).ToList();
            for (int i = 0; i < targets.Count; i++)
            {
                var enemy = targets[i];
                var dir = (Vector2)(enemy.transform.position - player.position);
                SpawnRadialSlashLines("UltDual_AshenOblivionParryLine", enemy.transform.position, dir.sqrMagnitude > 0.01f ? dir.normalized : Vector2.up, 4, 0.52f, new Color(0.86f, 0.78f, 1f, 0.52f), 0.30f);
                SpawnEchoLink("UltDual_AshenOblivionReturnBrand", enemy.transform.position, player.position, new Color(0.80f, 0.58f, 1f, 0.38f), 0.34f, 0.016f);
                MarkEnemyEchoState(enemy, V1MemoryId.AshenShield, 1.10f, 0.96f);
                MarkEnemyEchoState(enemy, V1MemoryId.OblivionBrand, 1.25f, 1.00f);
                DealDamage(enemy, 46f, "UltDual AshenOblivion", false, dir.sqrMagnitude > 0.01f ? dir.normalized : Vector2.up, 0.76f);
            }
            hitstopTimer = Mathf.Max(hitstopTimer, 0.035f);
        }

        void UpdateUtilityUltimate(WeaponRuntimeSpec weapon, float dt)
        {
            if (RunWeaponPatternUtilityUltimate(weapon, dt)) return;

            if (FractureExecutionReady)
            {
                if (ultimatePulseTimer <= 0f)
                {
                    ultimatePulseTimer = 0.58f;
                    var target = enemies.Where(e => e != null && e.IsAlive).OrderBy(e => e.HealthRatio).FirstOrDefault();
                    if (target != null)
                    {
                        PlaySfx("execution", 0.82f, 0.18f);
                        SpawnPromptSprite("FractureExecution", LoadSprite(UltimateFracturePath), () => MakeRingSprite("FractureExecution", Color.white, 160), target.transform.position, Quaternion.identity, 2.55f, 1.05f, new Color(1f, 0.94f, 0.62f, 0.66f), 0.46f);
                        SpawnPromptSprite("FractureExecutionCore", EchoVfxSprite(V1MemoryId.ExecutionFlash), () => MakeImpactDiamondSprite("FractureExecutionCore", Color.white), target.transform.position, Quaternion.identity, 1.38f, 0.76f, new Color(1f, 0.92f, 0.55f, 0.92f), 0.26f);
                        foreach (var enemy in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(target.transform.position, e.transform.position) < 1.75f).Take(8).ToList())
                        {
                            DealDamage(enemy, enemy.HealthRatio < 0.36f ? 76f : 32f, "파쇄 처형", false);
                        }
                    }
                }
                return;
            }

            if (StasisHuntReady)
            {
                if (ultimatePulseTimer <= 0f)
                {
                    ultimatePulseTimer = 0.42f;
                    PlaySfx("stopped", 0.68f, 0.18f);
                    PlaySfx("hunter", 0.52f, 0.10f);
                    SpawnPromptSprite("StasisHunt", LoadSprite(UltimateStasisPath), () => MakeRingSprite("StasisHunt", Color.white, 160), player.position, Quaternion.identity, 3.18f, 1.24f, new Color(1f, 0.74f, 0.28f, 0.66f), 1.02f);
                    SpawnStoppedSecondField(player.position, 2.05f, TimeStopGold(true), 1.70f, true);
                    foreach (var enemy in enemies.Where(e => e != null && e.IsAlive).OrderBy(e => Vector2.Distance(player.position, e.transform.position)).Take(6).ToList())
                    {
                        enemy.ApplyBriefFreeze(0.42f);
                        var go = new GameObject("StasisHuntShot");
                        go.transform.position = player.position;
                        var sr = go.AddComponent<SpriteRenderer>();
                        sr.sprite = EchoVfxSprite(V1MemoryId.HunterOath) ?? MakeCrescentSlashSprite("StasisHuntShot", Color.white, false);
                        go.transform.localScale = Vector3.one * (sr.sprite != null && sr.sprite.bounds.size.x > 2f ? ScaleSpriteToWorldWidth(sr.sprite, 0.76f) : 0.14f);
                        sr.color = new Color(0.72f, 0.86f, 1f, 0.86f);
                        sr.sortingOrder = 44;
                        go.AddComponent<V1Projectile>().Configure(this, enemy, 10.2f, 24f, "정지 추적");
                    }
                }
                return;
            }

            if (AshenOblivionReady && ultimatePulseTimer <= 0f)
            {
                ultimatePulseTimer = 0.72f;
                HealPlayer(4.4f);
                PlaySfx("ashen", 0.64f, 0.18f);
                PlaySfx("brand", 0.48f, 0.18f);
                SpawnPromptSprite("AshenOblivion", LoadSprite(UltimateAshenOblivionPath), () => MakeRingSprite("AshenOblivion", Color.white, 180), player.position, Quaternion.identity, 3.24f, 1.30f, new Color(0.78f, 0.68f, 1f, 0.60f), 0.72f);
                SpawnTransientSprite("AshenOblivionGuard", MakeRingSprite("AshenOblivionGuard", Color.white, 144), player.position, Quaternion.Euler(0f, 0f, elapsed * -120f), 1.32f, new Color(0.88f, 0.92f, 1f, 0.42f), 0.56f);
                foreach (var enemy in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(player.position, e.transform.position) < 3.2f).Take(10).ToList())
                {
                    DealDamage(enemy, 30f, "잿빛 망각", false);
                }
            }
        }

        void LaunchKalmuriBlade(V1Enemy first, Vector2 forward, WeaponRuntimeSpec weapon)
        {
            var target = enemies.Where(e => e != null && e.IsAlive && e != first).OrderBy(e => Vector2.Distance(first.transform.position, e.transform.position)).FirstOrDefault();
            if (target == null) return;
            var f = forward.sqrMagnitude > 0.01f ? forward.normalized : lastAim.normalized;
            if (f.sqrMagnitude < 0.01f) f = Vector2.up;
            var side = new Vector2(-f.y, f.x);
            var wound = first.transform.position + (Vector3)(f * 0.10f);
            var heavy = weapon.Id == V1WeaponId.Greatsword;
            var targetDir = (Vector2)(target.transform.position - wound);
            if (targetDir.sqrMagnitude < 0.01f) targetDir = f;
            var angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            PlaySfx("kalmuri_lunge", 0.70f, 0.08f);
            SpawnTransientSprite(heavy ? "EchoGreat_KalmuriAwakenWound" : "EchoDual_KalmuriAwakenWound", MakeImpactDiamondSprite("KalmuriAwakenWound", Color.white), wound, Quaternion.Euler(0f, 0f, angle + 45f), heavy ? 0.34f : 0.24f, new Color(0.86f, 1f, 1f, 0.68f), heavy ? 0.24f : 0.18f);
            SpawnEchoWoundSlash(heavy ? "EchoGreat_KalmuriAwakenScar" : "EchoDual_KalmuriAwakenScar", wound, f, new Color(0.72f, 1f, 1f, heavy ? 0.62f : 0.48f), heavy ? 0.86f : 0.58f, heavy ? 0.18f : 0.12f);
            SpawnEchoLink(heavy ? "EchoGreat_KalmuriAwakenChainLine" : "EchoDual_KalmuriAwakenChainLine", wound, target.transform.position, new Color(0.62f, 0.96f, 1f, heavy ? 0.46f : 0.34f), heavy ? 0.24f : 0.18f, heavy ? 0.026f : 0.018f);
            var go = new GameObject(heavy ? "EchoGreat_KalmuriAwakenBlade" : "EchoDual_KalmuriAwakenBlade");
            go.transform.position = wound + (Vector3)(side * (heavy ? 0.08f : 0.04f));
            go.transform.rotation = Quaternion.Euler(0f, 0f, angle + 62f);
            go.transform.localScale = Vector3.one * (heavy ? 0.32f : 0.22f);
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = LoadSprite(KalmuriLaunchBladePath) ?? MakeBoxSprite("launch", Color.cyan, 16, 64);
            sr.color = heavy ? new Color(0.82f, 1f, 1f, 0.96f) : new Color(0.72f, 1f, 1f, 0.90f);
            sr.sortingOrder = 45;
            SpawnTransientSprite(heavy ? "EchoGreat_KalmuriAwakenWoundBurst" : "EchoDual_KalmuriAwakenWoundBurst", MakeRingSprite("KalmuriAwakenWoundBurst", Color.white, 120), wound, Quaternion.identity, heavy ? 0.42f : 0.32f, new Color(0.68f, 1f, 1f, heavy ? 0.42f : 0.32f), heavy ? 0.18f : 0.14f);
            go.AddComponent<V1Projectile>().Configure(this, target, heavy ? 8.4f : 10.4f, heavy ? 26f : 18f, heavy ? "Kalmuri great wound chain" : "Kalmuri wound chain");
        }

        void BloodBloom(V1Enemy center, int level)
        {
            PlaySfx("blood_mark", 0.58f, 0.12f);
            SpawnTransientSprite("피꽃", LoadSprite("Assets/_dev/Art/Sprites/Echoes/Blood/spr_blood_bloom_01.png"), center.transform.position, Quaternion.identity, 0.92f, new Color(1f, 0.12f, 0.18f, 0.94f), 0.52f);
            SpawnTransientSprite("BloodBloomRing", MakeRingSprite("BloodBloomRing", Color.white, 132), center.transform.position, Quaternion.identity, 0.72f + level * 0.055f, new Color(1f, 0.12f, 0.18f, 0.46f), 0.42f);
            SpawnBloodThread(center.transform.position, 0.8f + level * 0.16f, level);
            foreach (var enemy in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(center.transform.position, e.transform.position) < 1.85f).ToList())
            {
                enemy.BloodMarked = true;
                enemy.MarkTimer = 2.4f;
                DealDamage(enemy, 7f + level * 2f, "혈반 각성", false);
            }
            playerHp = Mathf.Min(playerMaxHp, playerHp + 2f + level);
        }

        void UpdateSpawning(float dt)
        {
            var cap = EnemyCap();
            var liveNonBoss = enemies.Count(e => e != null && e.IsAlive && e.Kind != V1EnemyKind.Gatekeeper);
            if (liveNonBoss >= cap)
            {
                spawnTimer = Mathf.Max(spawnTimer, SpawnProfile().Interval * 0.75f);
                return;
            }

            spawnTimer -= dt;
            if (spawnTimer > 0f) return;
            var profile = SpawnProfile();
            spawnTimer = Mathf.Max(0.34f, profile.Interval);

            for (int i = 0; i < profile.PackSize; i++)
            {
                SpawnEnemy(profile.Pick(rng), RandomSpawnPosition());
            }
        }

        SpawnWaveProfile SpawnProfile()
        {
            if (!fastDebugRun)
            {
                return SteppedSpawnProfile();
            }

            var pressure = CurrentPressure();
            if (pressure.Deficit)
            {
                return pressure.Progress < 0.30f
                    ? new SpawnWaveProfile(0.72f, 2, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne)
                    : new SpawnWaveProfile(pressure.Progress > 0.72f ? 0.44f : 0.50f, pressure.Progress > 0.72f ? 4 : 3, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne, V1EnemyKind.DriftingEye, V1EnemyKind.VoidPriest);
            }

            if (pressure.FirstCycle && elapsed < 120f)
            {
                if (elapsed < 30f)
                {
                    return new SpawnWaveProfile(0.42f, 2, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye);
                }

                return elapsed < 76f
                    ? new SpawnWaveProfile(0.48f, 3, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne)
                    : new SpawnWaveProfile(0.42f, 4, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne, V1EnemyKind.VoidPriest);
            }

            if (pressure.Progress < 0.24f)
            {
                return elapsed < 70f
                    ? new SpawnWaveProfile(0.72f, 2, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye)
                    : new SpawnWaveProfile(0.72f, 3, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne);
            }

            if (pressure.Progress < 0.70f)
            {
                return elapsed >= 126f
                    ? new SpawnWaveProfile(0.54f, 3, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne, V1EnemyKind.VoidPriest)
                    : elapsed > 95f
                        ? new SpawnWaveProfile(0.86f, 2, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne, V1EnemyKind.VoidPriest)
                        : new SpawnWaveProfile(0.92f, 2, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne);
            }

            if (pressure.FirstCycle && pressure.Progress >= 0.94f)
            {
                return new SpawnWaveProfile(1.30f, 1, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye);
            }

            return pressure.FirstCycle
                ? new SpawnWaveProfile(1.08f, 2, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne)
                : new SpawnWaveProfile(0.43f, 4, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne, V1EnemyKind.VoidPriest);
        }

        SpawnWaveProfile SteppedSpawnProfile()
        {
            if (elapsed < 60f)
            {
                var t = Mathf.Clamp01(elapsed / 60f);
                return new SpawnWaveProfile(Mathf.Lerp(1.05f, 0.78f, t), t < 0.55f ? 1 : 2, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye);
            }
            if (elapsed < 150f)
            {
                var t = Mathf.Clamp01((elapsed - 60f) / 90f);
                return new SpawnWaveProfile(Mathf.Lerp(0.76f, 0.58f, t), t < 0.62f ? 2 : 3, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne);
            }
            if (elapsed < 300f)
            {
                var t = Mathf.Clamp01((elapsed - 150f) / 150f);
                return new SpawnWaveProfile(Mathf.Lerp(0.58f, 0.48f, t), t < 0.55f ? 2 : 3, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne, V1EnemyKind.VoidPriest);
            }
            if (elapsed < 540f)
            {
                var t = Mathf.Clamp01((elapsed - 300f) / 240f);
                return new SpawnWaveProfile(Mathf.Lerp(0.48f, 0.40f, t), t < 0.55f ? 3 : 4, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne, V1EnemyKind.VoidPriest);
            }
            if (elapsed < 900f)
            {
                var t = Mathf.Clamp01((elapsed - 540f) / 360f);
                return new SpawnWaveProfile(Mathf.Lerp(0.40f, 0.34f, t), t < 0.55f ? 4 : 5, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne, V1EnemyKind.VoidPriest, V1EnemyKind.VoidPriest);
            }

            var finalT = Mathf.Clamp01((elapsed - 900f) / Mathf.Max(1f, RunSeconds - 900f));
            return new SpawnWaveProfile(Mathf.Lerp(0.34f, 0.30f, finalT), 5, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne, V1EnemyKind.VoidPriest, V1EnemyKind.VoidPriest);
        }

        int EnemyCap()
        {
            if (!fastDebugRun)
            {
                return SteppedEnemyCap();
            }

            var pressure = CurrentPressure();
            if (pressure.Deficit) return pressure.Progress < 0.30f ? 16 : 14;
            if (pressure.FirstCycle && elapsed < 120f) return 36;
            if (pressure.FirstCycle && pressure.Progress >= 0.94f) return 22;
            if (pressure.FirstCycle && pressure.Progress >= 0.70f) return 32;
            if (pressure.FirstCycle) return 34;
            if (pressure.Progress >= 0.70f) return 46;
            return 46;
        }

        int SteppedEnemyCap()
        {
            if (elapsed < 60f) return Mathf.RoundToInt(Mathf.Lerp(14f, 20f, Mathf.Clamp01(elapsed / 60f)));
            if (elapsed < 150f) return Mathf.RoundToInt(Mathf.Lerp(22f, 30f, Mathf.Clamp01((elapsed - 60f) / 90f)));
            if (elapsed < 300f) return Mathf.RoundToInt(Mathf.Lerp(30f, 38f, Mathf.Clamp01((elapsed - 150f) / 150f)));
            if (elapsed < 540f) return Mathf.RoundToInt(Mathf.Lerp(38f, 48f, Mathf.Clamp01((elapsed - 300f) / 240f)));
            if (elapsed < 900f) return Mathf.RoundToInt(Mathf.Lerp(48f, 58f, Mathf.Clamp01((elapsed - 540f) / 360f)));
            return Mathf.RoundToInt(Mathf.Lerp(58f, 64f, Mathf.Clamp01((elapsed - 900f) / Mathf.Max(1f, RunSeconds - 900f))));
        }

        void SpawnEnemy(V1EnemyKind kind, Vector3 pos)
        {
            var go = new GameObject($"Enemy_{kind}");
            go.transform.position = pos;
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = EnemySprite(kind);
            sr.color = EnemyColor(kind);
            sr.sortingOrder = 15;
            var enemy = go.AddComponent<V1Enemy>();
            enemy.Configure(this, kind, player, EnemyHp(kind), EnemySpeed(kind), EnemyDamage(kind), EnemyRadius(kind));
            AddEnemyRoleMarker(go.transform, kind);
            enemies.Add(enemy);
        }

        void SpawnGatekeeper()
        {
            if (enemies.Any(e => e != null && e.Kind == V1EnemyKind.Gatekeeper)) return;
            SpawnEnemy(V1EnemyKind.Gatekeeper, player.position + Vector3.up * 7.2f);
            Log($"문지기 등장 {bossSpawnIndex + 1}/{BossSchedule().Length}: 망각 관문");
        }

        void RemoveExistingGatekeepers()
        {
            var gatekeepers = enemies
                .Where(e => e != null && e.Kind == V1EnemyKind.Gatekeeper)
                .ToList();
            foreach (var gatekeeper in gatekeepers)
            {
                if (gatekeeper != null)
                {
                    Destroy(gatekeeper.gameObject);
                }
            }
            enemies.RemoveAll(e => e == null || gatekeepers.Contains(e));
        }

        void ClearEnemiesForDebug()
        {
            foreach (var enemy in enemies.ToList())
            {
                if (enemy != null)
                {
                    Destroy(enemy.gameObject);
                }
            }
            enemies.Clear();
        }

        void SpawnGatekeeperWarning()
        {
            if (player == null) return;
            PlaySfx("warning");
            var warningPos = player.position + Vector3.up * 5.2f;
            warningPos.x = Mathf.Clamp(warningPos.x, -ArenaHalfWidth + 1.2f, ArenaHalfWidth - 1.2f);
            warningPos.y = Mathf.Clamp(warningPos.y, -ArenaHalfHeight + 1.2f, ArenaHalfHeight - 1.2f);
            SpawnTransientSprite("GatekeeperWarningOuter", MakeRingSprite("GatekeeperWarningOuter", Color.white, 180), warningPos, Quaternion.identity, 1.46f, new Color(1f, 0.20f, 0.16f, 0.58f), 1.55f);
            SpawnTransientSprite("GatekeeperWarningInner", MakeRingSprite("GatekeeperWarningInner", Color.white, 132), warningPos, Quaternion.Euler(0f, 0f, elapsed * -90f), 0.84f, new Color(1f, 0.72f, 0.36f, 0.48f), 1.40f);
            SpawnTransientSprite("GatekeeperWarningCore", MakeImpactDiamondSprite("GatekeeperWarningCore", Color.white), warningPos, Quaternion.Euler(0f, 0f, 45f), 0.42f, new Color(1f, 0.30f, 0.20f, 0.82f), 1.00f);
            SpawnFloatingText(warningPos + Vector3.up * 0.35f, "문지기 접근", new Color(1f, 0.46f, 0.34f));
        }

        public int GatekeeperPatternRank => Mathf.Clamp(gatekeeperKills, 0, 3);

        float GatekeeperRaidWarningSeconds(float baseSeconds) => Mathf.Max(baseSeconds, 0.92f);

        GameObject SpawnGatekeeperRaidFill(string name, Sprite sprite, Vector3 position, Quaternion rotation, Vector3 fullScale, Color color, float warningSeconds, float startScaleFactor)
        {
            var startScale = Vector3.Max(fullScale * Mathf.Clamp(startScaleFactor, 0.04f, 0.40f), Vector3.one * 0.01f);
            var go = SpawnTransientSpriteScaled(name, sprite, position, rotation, startScale, color, warningSeconds);
            var fill = go.GetComponent<V1RaidTelegraphFill>();
            if (fill == null) fill = go.AddComponent<V1RaidTelegraphFill>();
            fill.Configure(fullScale * CombatVfxVisibilityScale, color, warningSeconds);
            return go;
        }

        void SpawnGatekeeperRaidImpact(string name, Vector3 position, Quaternion rotation, Vector3 scale, int rank, bool circular)
        {
            var impactSprite = circular
                ? MakeDiscSprite($"{name}Flash", Color.white, 160)
                : MakeSectorSprite($"{name}Flash", Color.white, 176, rank >= 3 ? 96f : 84f);
            var size = Mathf.Max(scale.x, scale.y);
            SpawnTransientSpriteScaled($"{name}WhiteSnap", impactSprite, position, rotation, scale * 0.74f, new Color(1f, 0.98f, 0.88f, 0.88f), 0.08f);
            SpawnTransientSpriteScaled($"{name}Flash", impactSprite, position, rotation, scale * 1.10f, new Color(1f, 0.92f, 0.78f, 0.82f), 0.14f);
            SpawnTransientSpriteScaled($"{name}RedBang", impactSprite, position, rotation, scale * 1.30f, GatekeeperTelegraphColor(rank, 0.66f), 0.22f);
            SpawnTransientSprite($"{name}OuterShock", MakeRingSprite($"{name}OuterShock", Color.white, 180), position, rotation, size * 1.34f, GatekeeperAccentColor(rank, 0.88f), 0.28f);
            SpawnRadialSlashLines($"{name}GroundRupture", position, Vector2.up, circular ? 8 + rank : 5 + rank, size * 0.92f, GatekeeperAccentColor(rank, 0.66f), 0.24f);
            cameraShakeTimer = Mathf.Max(cameraShakeTimer, 0.16f + rank * 0.03f);
            cameraShakeAmount = Mathf.Max(cameraShakeAmount, 0.070f + rank * 0.012f);
            PlaySfx("warning", 0.42f + rank * 0.04f, 0.08f);
        }

        void SpawnGatekeeperCastBurst(Vector3 center, Vector2 forward, int rank, int patternStep)
        {
            forward = forward.sqrMagnitude > 0.01f ? forward.normalized : Vector2.up;
            var angle = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg - 90f;
            var accent = GatekeeperAccentColor(rank, 0.82f);
            SpawnTransientSprite("GatekeeperCastSigil", GatekeeperSigilSprite(rank), center + Vector3.up * 0.22f, Quaternion.Euler(0f, 0f, 45f + patternStep * 18f), 0.64f + rank * 0.08f, accent, 0.56f);
            SpawnTransientSprite("GatekeeperCastHalo", MakeRingSprite("GatekeeperCastHalo", Color.white, 180), center, Quaternion.Euler(0f, 0f, patternStep * -28f), 0.92f + rank * 0.16f, GatekeeperTelegraphColor(rank, 0.58f), 0.52f);
            SpawnTransientSpriteScaled("GatekeeperCastBladeSpine", MakeBoxSprite("GatekeeperCastBladeSpine", Color.white, 8, 150), center + (Vector3)(forward * (0.46f + rank * 0.04f)), Quaternion.Euler(0f, 0f, angle), new Vector3(0.034f + rank * 0.004f, 0.86f + rank * 0.16f, 1f), accent, 0.44f);
            SpawnRadialSlashLines("GatekeeperCastRupture", center, forward, 5 + rank, 0.72f + rank * 0.14f, GatekeeperTelegraphColor(rank, 0.54f), 0.34f);
            if (player != null)
            {
                SpawnEchoLink("GatekeeperCastTargetLine", center + Vector3.up * 0.14f, player.position, GatekeeperTelegraphColor(rank, 0.24f), 0.34f, 0.018f + rank * 0.003f);
            }
        }

        public void GatekeeperPatternPulse(V1Enemy gatekeeper, int patternStep)
        {
            if (gatekeeper == null || player == null || deathOverlay) return;
            var rank = GatekeeperPatternRank;
            var center = gatekeeper.transform.position;
            var toPlayer = (Vector2)(player.position - center);
            var forward = toPlayer.sqrMagnitude > 0.01f ? toPlayer.normalized : lastAim.normalized;
            SpawnGatekeeperCastBurst(center, forward, rank, patternStep);

            if (rank <= 0)
            {
                GatekeeperMeteorTell(player.position, 1.12f, 0.78f, 13f, patternStep, rank);
            }
            else if (rank == 1)
            {
                GatekeeperConeTell(center, forward, 3.35f, 82f, 0.68f, 16f, rank);
                if (patternStep % 2 == 0)
                {
                    GatekeeperMeteorTell(player.position + (Vector3)(forward * 0.55f), 0.92f, 0.72f, 11f, patternStep, rank);
                }
            }
            else if (rank == 2)
            {
                GatekeeperRingTell(center, 2.15f, 0.74f, 18f, rank);
                GatekeeperMeteorTell(player.position, 0.98f, 0.70f, 13f, patternStep, rank);
            }
            else
            {
                GatekeeperConeTell(center, forward, 3.85f, 96f, 0.62f, 18f, rank);
                var side = new Vector2(-forward.y, forward.x);
                GatekeeperMeteorTell(player.position + (Vector3)(side * 0.72f), 0.98f, 0.68f, 13f, patternStep, rank);
                GatekeeperMeteorTell(player.position - (Vector3)(side * 0.72f), 0.98f, 0.74f, 13f, patternStep + 1, rank);
            }
            cameraShakeTimer = Mathf.Max(cameraShakeTimer, 0.12f);
            cameraShakeAmount = Mathf.Max(cameraShakeAmount, 0.045f + rank * 0.012f);
        }

        void GatekeeperMeteorTell(Vector3 target, float radius, float warningSeconds, float damage, int patternStep, int rank)
        {
            if (player == null) return;
            target.z = 0f;
            var telegraphTime = GatekeeperRaidWarningSeconds(warningSeconds);
            SpawnGatekeeperRaidFill("GatekeeperMeteorTellFill", MakeDiscSprite("GatekeeperMeteorTellFill", Color.white, 160), target, Quaternion.identity, Vector3.one * radius, GatekeeperTelegraphColor(rank, 0.54f), telegraphTime, 0.10f);
            SpawnTransientSprite("GatekeeperMeteorTellRing", MakeRingSprite("GatekeeperMeteorTellRing", Color.white, 180), target, Quaternion.Euler(0f, 0f, patternStep * 19f), radius, GatekeeperTelegraphColor(rank, 0.92f), telegraphTime);
            SpawnTransientSprite("GatekeeperMeteorTellOuterLock", MakeRingSprite("GatekeeperMeteorTellOuterLock", Color.white, 180), target, Quaternion.Euler(0f, 0f, patternStep * -31f), radius * 1.08f, GatekeeperAccentColor(rank, 0.42f), telegraphTime);
            SpawnTransientSprite("GatekeeperMeteorTellCore", GatekeeperMeteorSprite(rank), target + Vector3.up * 0.08f, Quaternion.Euler(0f, 0f, patternStep * 37f), 0.28f, GatekeeperAccentColor(rank, 0.70f), telegraphTime * 0.92f);
            SpawnGatekeeperFallingMeteor(target, radius, telegraphTime, patternStep, rank);
            StartCoroutine(ResolveGatekeeperMeteor(target, radius, telegraphTime, damage, rank));
        }

        void SpawnGatekeeperFallingMeteor(Vector3 target, float radius, float telegraphTime, int patternStep, int rank)
        {
            var start = target + new Vector3(Mathf.Sin(patternStep * 1.7f) * 0.54f, 3.05f + rank * 0.24f, 0f);
            var end = target + Vector3.up * 0.18f;
            var delta = end - start;
            var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg - 90f;
            var sprite = GatekeeperMeteorSprite(rank);
            var color = GatekeeperAccentColor(rank, 0.94f);
            SpawnTransientSprite("GatekeeperMeteorSpawnFlare", MakeRingSprite("GatekeeperMeteorSpawnFlare", Color.white, 132), start, Quaternion.Euler(0f, 0f, patternStep * 27f), 0.32f + rank * 0.04f, GatekeeperAccentColor(rank, 0.62f), telegraphTime * 0.50f);
            SpawnEchoLink("GatekeeperMeteorFallTrail", start, target, GatekeeperAccentColor(rank, 0.52f), telegraphTime * 0.94f, 0.052f + rank * 0.008f);
            SpawnEchoLink("GatekeeperMeteorFallHotLine", start + Vector3.right * 0.10f, target - Vector3.right * 0.06f, new Color(1f, 0.94f, 0.70f, 0.38f), telegraphTime * 0.82f, 0.024f + rank * 0.004f);
            SpawnSweepingTransientSprite("GatekeeperMeteorFallingBody", sprite, start, end, angle, angle + 30f, Mathf.Clamp(radius * 0.42f, 0.38f, 0.58f), Mathf.Clamp(radius * 0.68f, 0.58f, 0.92f), color, telegraphTime, telegraphTime * 0.90f);
            SpawnTransientSprite("GatekeeperMeteorShadow", MakeDiscSprite("GatekeeperMeteorShadow", Color.white, 96), target, Quaternion.identity, Mathf.Clamp(radius * 0.30f, 0.24f, 0.42f), new Color(0.16f, 0.02f, 0.02f, 0.36f), telegraphTime);
        }

        IEnumerator ResolveGatekeeperMeteor(Vector3 center, float radius, float delay, float damage, int rank)
        {
            yield return new WaitForSeconds(delay);
            if (player == null || deathOverlay) yield break;
            SpawnGatekeeperRaidImpact("GatekeeperMeteorImpact", center, Quaternion.identity, Vector3.one * radius, rank, true);
            SpawnTransientSprite("GatekeeperMeteorGroundScorch", MakeDiscSprite("GatekeeperMeteorGroundScorch", Color.white, 128), center, Quaternion.identity, radius * 0.76f, new Color(0.24f, 0.025f, 0.018f, 0.48f), 0.42f);
            SpawnTransientSprite("GatekeeperMeteorImpactShard", GatekeeperMeteorSprite(rank), center, Quaternion.identity, radius * 0.92f, GatekeeperAccentColor(rank, 0.82f), 0.24f);
            SpawnTransientSprite("GatekeeperMeteorImpactRing", MakeRingSprite("GatekeeperMeteorImpactRing", Color.white, 180), center, Quaternion.identity, radius * 1.48f, GatekeeperAccentColor(rank, 0.68f), 0.32f);
            SpawnRadialSlashLines("GatekeeperMeteorDebris", center, Vector2.up, 10 + rank, radius * 1.02f, GatekeeperAccentColor(rank, 0.62f), 0.26f);
            var toPlayer = (Vector2)(player.position - center);
            if (toPlayer.magnitude <= radius)
            {
                var dir = toPlayer.sqrMagnitude > 0.01f ? toPlayer.normalized : Vector2.up;
                playerMoveVelocity += dir * (1.05f + rank * 0.18f);
                DamagePlayer(damage, "Gatekeeper meteor");
            }
        }

        void GatekeeperConeTell(Vector3 center, Vector2 forward, float radius, float arcDegrees, float warningSeconds, float damage, int rank)
        {
            if (player == null) return;
            forward = forward.sqrMagnitude > 0.01f ? forward.normalized : Vector2.up;
            var angle = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg - 90f;
            var telegraphTime = GatekeeperRaidWarningSeconds(warningSeconds);
            var pos = center + (Vector3)(forward * radius * 0.42f);
            SpawnGatekeeperRaidFill("GatekeeperConeTellFill", MakeSectorSprite("GatekeeperConeTellFill", Color.white, 176, arcDegrees), pos, Quaternion.Euler(0f, 0f, angle), Vector3.one * radius, GatekeeperTelegraphColor(rank, 0.50f), telegraphTime, 0.08f);
            SpawnTransientSprite("GatekeeperConeTellEdge", MakeSectorSprite("GatekeeperConeTellEdge", Color.white, 176, arcDegrees), pos, Quaternion.Euler(0f, 0f, angle), radius * 1.03f, GatekeeperTelegraphColor(rank, 0.20f), telegraphTime);
            SpawnTransientSpriteScaled("GatekeeperConeTellCenterLine", MakeBoxSprite("GatekeeperConeTellCenterLine", Color.white, 8, 150), center + (Vector3)(forward * radius * 0.45f), Quaternion.Euler(0f, 0f, angle), new Vector3(0.034f, radius * 0.60f, 1f), GatekeeperTelegraphColor(rank, 0.86f), telegraphTime);
            SpawnGatekeeperConeCharge(center, forward, radius, arcDegrees, telegraphTime, rank);
            StartCoroutine(ResolveGatekeeperCone(center, forward, radius, arcDegrees, telegraphTime, damage, rank));
        }

        void SpawnGatekeeperConeCharge(Vector3 center, Vector2 forward, float radius, float arcDegrees, float telegraphTime, int rank)
        {
            var angle = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg - 90f;
            var side = new Vector2(-forward.y, forward.x).normalized;
            var start = center + (Vector3)(forward * 0.34f);
            var end = center + (Vector3)(forward * (radius * 0.78f));
            var cleave = GatekeeperCleaveSprite(rank);
            SpawnSweepingTransientSprite("GatekeeperConeChargingBlade", cleave, start, end, angle - 5f, angle + 8f, Mathf.Clamp(radius * 0.18f, 0.40f, 0.62f), Mathf.Clamp(radius * 0.40f, 0.76f, 1.08f), GatekeeperAccentColor(rank, 0.48f), telegraphTime, telegraphTime * 0.78f);
            SpawnSweepingTransientSprite("GatekeeperConeChargingAfterblade", cleave, start - (Vector3)(side * radius * 0.08f), end + (Vector3)(side * radius * 0.06f), angle - 22f, angle + 22f, Mathf.Clamp(radius * 0.12f, 0.30f, 0.46f), Mathf.Clamp(radius * 0.58f, 0.86f, 1.28f), GatekeeperAccentColor(rank, 0.28f), telegraphTime, telegraphTime * 0.68f);
            SpawnEchoLink("GatekeeperConeChargeLine", start + (Vector3)(side * radius * 0.14f), end + (Vector3)(side * radius * 0.26f), GatekeeperAccentColor(rank, 0.34f), telegraphTime * 0.86f, 0.032f);
            SpawnEchoLink("GatekeeperConeChargeLine", start - (Vector3)(side * radius * 0.14f), end - (Vector3)(side * radius * 0.26f), GatekeeperAccentColor(rank, 0.34f), telegraphTime * 0.86f, 0.032f);
        }

        IEnumerator ResolveGatekeeperCone(Vector3 center, Vector2 forward, float radius, float arcDegrees, float delay, float damage, int rank)
        {
            yield return new WaitForSeconds(delay);
            if (player == null || deathOverlay) yield break;
            var angle = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg - 90f;
            SpawnGatekeeperRaidImpact("GatekeeperConeImpact", center + (Vector3)(forward * radius * 0.42f), Quaternion.Euler(0f, 0f, angle), Vector3.one * radius, rank, false);
            SpawnTransientSprite("GatekeeperConeSlash", GatekeeperCleaveSprite(rank), center + (Vector3)(forward * radius * 0.48f), Quaternion.Euler(0f, 0f, angle), radius * 0.82f, GatekeeperAccentColor(rank, 0.84f), 0.24f);
            SpawnSweepingTransientSprite("GatekeeperConeSlashWave", GatekeeperCleaveSprite(rank), center + (Vector3)(forward * 0.55f), center + (Vector3)(forward * (radius * 0.78f)), angle - 4f, angle + 18f, radius * 0.28f, radius * 0.74f, GatekeeperAccentColor(rank, 0.92f), 0.28f, 0.14f);
            var side = new Vector2(-forward.y, forward.x).normalized;
            var leftEdge = center + (Vector3)(forward * radius * 0.84f + side * radius * 0.38f);
            var rightEdge = center + (Vector3)(forward * radius * 0.84f - side * radius * 0.38f);
            SpawnEchoLink("GatekeeperConeImpactEdge", center, leftEdge, GatekeeperAccentColor(rank, 0.54f), 0.22f, 0.038f);
            SpawnEchoLink("GatekeeperConeImpactEdge", center, rightEdge, GatekeeperAccentColor(rank, 0.54f), 0.22f, 0.038f);
            SpawnRadialSlashLines("GatekeeperConeGroundScars", center + (Vector3)(forward * radius * 0.38f), forward, 5 + rank, radius * 0.78f, GatekeeperAccentColor(rank, 0.50f), 0.24f);
            var toPlayer = (Vector2)(player.position - center);
            if (toPlayer.magnitude <= radius && Vector2.Angle(forward, toPlayer) <= arcDegrees * 0.5f)
            {
                var dir = toPlayer.sqrMagnitude > 0.01f ? toPlayer.normalized : forward;
                playerMoveVelocity += dir * (1.35f + rank * 0.22f);
                DamagePlayer(damage, "Gatekeeper cone");
            }
        }

        void GatekeeperRingTell(Vector3 center, float radius, float warningSeconds, float damage, int rank)
        {
            var telegraphTime = GatekeeperRaidWarningSeconds(warningSeconds);
            SpawnGatekeeperRaidFill("GatekeeperRingTellFill", MakeDiscSprite("GatekeeperRingTellFill", Color.white, 180), center, Quaternion.identity, Vector3.one * radius, GatekeeperTelegraphColor(rank, 0.40f), telegraphTime, 0.16f);
            SpawnTransientSprite("GatekeeperRingTellOuter", MakeRingSprite("GatekeeperRingTellOuter", Color.white, 180), center, Quaternion.identity, radius, GatekeeperTelegraphColor(rank, 0.92f), telegraphTime);
            SpawnTransientSprite("GatekeeperRingTellInner", MakeRingSprite("GatekeeperRingTellInner", Color.white, 132), center, Quaternion.Euler(0f, 0f, elapsed * -120f), radius * 0.52f, GatekeeperAccentColor(rank, 0.58f), telegraphTime);
            StartCoroutine(ResolveGatekeeperRing(center, radius, telegraphTime, damage, rank));
        }

        IEnumerator ResolveGatekeeperRing(Vector3 center, float radius, float delay, float damage, int rank)
        {
            yield return new WaitForSeconds(delay);
            if (player == null || deathOverlay) yield break;
            SpawnGatekeeperRaidImpact("GatekeeperRingImpact", center, Quaternion.identity, Vector3.one * radius, rank, true);
            SpawnTransientSprite("GatekeeperRingShockwave", MakeRingSprite("GatekeeperRingShockwave", Color.white, 180), center, Quaternion.identity, radius * 1.14f, GatekeeperAccentColor(rank, 0.82f), 0.30f);
            SpawnTransientSprite("GatekeeperRingInnerFlash", MakeDiscSprite("GatekeeperRingInnerFlash", Color.white, 132), center, Quaternion.identity, radius * 0.56f, new Color(1f, 0.88f, 0.70f, 0.44f), 0.16f);
            SpawnRadialSlashLines("GatekeeperRingCrackSpokes", center, Vector2.up, 12 + rank, radius * 0.92f, GatekeeperAccentColor(rank, 0.54f), 0.28f);
            var toPlayer = (Vector2)(player.position - center);
            if (toPlayer.magnitude <= radius)
            {
                var dir = toPlayer.sqrMagnitude > 0.01f ? toPlayer.normalized : Vector2.up;
                playerMoveVelocity += dir * (1.50f + rank * 0.16f);
                DamagePlayer(damage, "Gatekeeper ring");
            }
        }

        void AddEnemyRoleMarker(Transform target, V1EnemyKind kind)
        {
            if (target == null || kind == V1EnemyKind.Eroder) return;
            var marker = new GameObject($"RoleMarker_{kind}");
            marker.transform.SetParent(target, false);
            marker.transform.localPosition = new Vector3(0f, kind == V1EnemyKind.Gatekeeper ? -0.12f : -0.06f, 0.02f);
            marker.transform.localRotation = Quaternion.identity;
            marker.transform.localScale = Vector3.one * (kind == V1EnemyKind.Gatekeeper ? 0.96f : 0.56f);
            var sr = marker.AddComponent<SpriteRenderer>();
            sr.sprite = kind == V1EnemyKind.SplitOne
                ? MakeImpactDiamondSprite("role_split_marker", Color.white)
                : MakeRingSprite($"role_{kind}_marker", Color.white, kind == V1EnemyKind.Gatekeeper ? 180 : 112);
            sr.color = kind switch
            {
                V1EnemyKind.DriftingEye => new Color(0.88f, 0.48f, 1f, 0.72f),
                V1EnemyKind.SplitOne => new Color(1f, 0.78f, 0.30f, 0.66f),
                V1EnemyKind.VoidPriest => new Color(0.36f, 1f, 0.66f, 0.70f),
                V1EnemyKind.Gatekeeper => new Color(1f, 0.22f, 0.16f, 0.76f),
                _ => new Color(1f, 1f, 1f, 0.35f)
            };
            sr.sortingOrder = 14;

            var pulse = marker.AddComponent<V1EnemyRoleMarker>();
            pulse.Configure(kind, sr.color);

            if (kind == V1EnemyKind.VoidPriest)
            {
                var cross = new GameObject("RoleMarker_VoidPriest_Cross");
                cross.transform.SetParent(target, false);
                cross.transform.localPosition = new Vector3(0f, 0.18f, 0.01f);
                cross.transform.localScale = Vector3.one * 0.28f;
                var crossRenderer = cross.AddComponent<SpriteRenderer>();
                crossRenderer.sprite = MakeImpactDiamondSprite("role_voidpriest_cross", Color.white);
                crossRenderer.color = new Color(0.42f, 1f, 0.68f, 0.74f);
                crossRenderer.sortingOrder = 17;
                cross.AddComponent<V1EnemyRoleMarker>().Configure(kind, crossRenderer.color, 1.35f);
            }
            else if (kind == V1EnemyKind.DriftingEye)
            {
                var focus = new GameObject("RoleMarker_DriftingEye_Focus");
                focus.transform.SetParent(target, false);
                focus.transform.localPosition = new Vector3(0f, 0.10f, 0.01f);
                focus.transform.localScale = Vector3.one * 0.26f;
                var focusRenderer = focus.AddComponent<SpriteRenderer>();
                focusRenderer.sprite = MakeRingSprite("role_driftingeye_focus", Color.white, 72);
                focusRenderer.color = new Color(0.94f, 0.54f, 1f, 0.62f);
                focusRenderer.sortingOrder = 17;
                focus.AddComponent<V1EnemyRoleMarker>().Configure(kind, focusRenderer.color, 1.18f);
            }
            else if (kind == V1EnemyKind.SplitOne)
            {
                var split = new GameObject("RoleMarker_SplitOne_Fracture");
                split.transform.SetParent(target, false);
                split.transform.localPosition = new Vector3(0f, 0.08f, 0.01f);
                split.transform.localScale = Vector3.one * 0.24f;
                var splitRenderer = split.AddComponent<SpriteRenderer>();
                splitRenderer.sprite = MakeBoxSprite("role_split_fracture", Color.white, 8, 84);
                splitRenderer.color = new Color(1f, 0.78f, 0.30f, 0.62f);
                splitRenderer.sortingOrder = 17;
                split.AddComponent<V1EnemyRoleMarker>().Configure(kind, splitRenderer.color, 1.22f);
            }
            else if (kind == V1EnemyKind.Gatekeeper)
            {
                var sigil = new GameObject("RoleMarker_Gatekeeper_Sigil");
                sigil.transform.SetParent(target, false);
                sigil.transform.localPosition = new Vector3(0f, 0.34f, 0.01f);
                sigil.transform.localScale = Vector3.one * 0.42f;
                var sigilRenderer = sigil.AddComponent<SpriteRenderer>();
                sigilRenderer.sprite = GatekeeperSigilSprite(Mathf.Clamp(bossSpawnIndex, 0, 3));
                sigilRenderer.color = GatekeeperAccentColor(Mathf.Clamp(bossSpawnIndex, 0, 3), 0.76f);
                sigilRenderer.sortingOrder = 18;
                sigil.AddComponent<V1EnemyRoleMarker>().Configure(kind, sigilRenderer.color, 1.10f);
            }
        }

        float[] BossSchedule() => fastDebugRun ? FastBossScheduleSeconds : BossScheduleSeconds;

        float CurrentDeficitDuration() => fastDebugRun ? FastDeficitSeconds : DeficitSurvivalSeconds;

        PressureState CurrentPressure()
        {
            var schedule = BossSchedule();
            var previousBossTime = bossSpawnIndex <= 0 ? 0f : schedule[Mathf.Clamp(bossSpawnIndex - 1, 0, schedule.Length - 1)];
            var nextBossTime = bossSpawnIndex < schedule.Length ? schedule[bossSpawnIndex] : RunSeconds;
            var span = Mathf.Max(1f, nextBossTime - previousBossTime);
            var progress = Mathf.Clamp01((elapsed - previousBossTime) / span);

            return new PressureState(progress, bossSpawnIndex == 0, false);
        }

        Vector3 RandomSpawnPosition()
        {
            var angle = UnityEngine.Random.value * Mathf.PI * 2f;
            var radius = elapsed < 120f ? 6.2f + UnityEngine.Random.value * 2.0f : 7.2f + UnityEngine.Random.value * 2.8f;
            var pos = player.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius;
            return new Vector3(Mathf.Clamp(pos.x, -ArenaHalfWidth, ArenaHalfWidth), Mathf.Clamp(pos.y, -ArenaHalfHeight, ArenaHalfHeight), 0f);
        }

        float EnemyHp(V1EnemyKind kind)
        {
            if (kind == V1EnemyKind.Gatekeeper) return GatekeeperHp();
            if (!fastDebugRun) return SteppedEnemyHp(kind);
            var baseHp = kind switch
            {
                V1EnemyKind.Eroder => 48f,
                V1EnemyKind.DriftingEye => 36f,
                V1EnemyKind.SplitOne => 58f,
                V1EnemyKind.VoidPriest => 74f,
                _ => 48f
            };
            var minutes = elapsed / 60f;
            return baseHp * (1f + minutes * 0.12f) * (1f + (level - 1) * 0.03f);
        }

        float SteppedEnemyHp(V1EnemyKind kind)
        {
            var avg = SteppedAverageEnemyHp();
            var roleMul = kind switch
            {
                V1EnemyKind.Eroder => 0.90f,
                V1EnemyKind.DriftingEye => 0.78f,
                V1EnemyKind.SplitOne => 1.14f,
                V1EnemyKind.VoidPriest => 1.34f,
                _ => 1f
            };
            return avg * roleMul;
        }

        float SteppedAverageEnemyHp()
        {
            if (elapsed < 60f) return Mathf.Lerp(38f, 48f, Mathf.Clamp01(elapsed / 60f));
            if (elapsed < 150f) return Mathf.Lerp(50f, 66f, Mathf.Clamp01((elapsed - 60f) / 90f));
            if (elapsed < 300f) return Mathf.Lerp(68f, 92f, Mathf.Clamp01((elapsed - 150f) / 150f));
            if (elapsed < 540f) return Mathf.Lerp(94f, 132f, Mathf.Clamp01((elapsed - 300f) / 240f));
            if (elapsed < 900f) return Mathf.Lerp(134f, 190f, Mathf.Clamp01((elapsed - 540f) / 360f));
            return Mathf.Lerp(192f, 245f, Mathf.Clamp01((elapsed - 900f) / Mathf.Max(1f, RunSeconds - 900f)));
        }

        float GatekeeperHp()
        {
            if (fastDebugRun) return debugReviewBossHp ? DebugReviewBossHp : FastBossHp;
            return bossSpawnIndex switch
            {
                <= 0 => FirstBossHp,
                1 => 4200f,
                2 => 7600f,
                _ => 12800f
            };
        }

        float EnemySpeed(V1EnemyKind kind) => (kind switch
        {
            V1EnemyKind.Eroder => 48f,
            V1EnemyKind.DriftingEye => 33f,
            V1EnemyKind.SplitOne => 42f,
            V1EnemyKind.VoidPriest => 28f,
            V1EnemyKind.Gatekeeper => 22f,
            _ => 40f
        }) / PixelsPerUnit;

        float EnemyDamage(V1EnemyKind kind)
        {
            var baseDamage = kind switch
            {
                V1EnemyKind.DriftingEye => 5f,
                V1EnemyKind.VoidPriest => 4f,
                V1EnemyKind.Gatekeeper => 10f,
                _ => 6f
            };
            var minutes = elapsed / 60f;
            var mul = Mathf.Min(2.2f, 1f + minutes * 0.025f + (level - 1) * 0.008f);
            return baseDamage * mul;
        }

        float EnemyRadius(V1EnemyKind kind) => (kind switch
        {
            V1EnemyKind.SplitOne => 15f,
            V1EnemyKind.Gatekeeper => 44f,
            _ => 13f
        }) / PixelsPerUnit;

        public Vector2 EnemySeparationForce(V1Enemy self)
        {
            if (self == null || !self.IsAlive) return Vector2.zero;
            var selfPos = (Vector2)self.transform.position;
            var push = Vector2.zero;
            var count = 0;
            for (int i = 0; i < enemies.Count; i++)
            {
                var other = enemies[i];
                if (other == null || other == self || !other.IsAlive) continue;
                var otherPos = (Vector2)other.transform.position;
                var delta = selfPos - otherPos;
                var minDist = self.TouchRadius + other.TouchRadius + EnemySeparationPadding;
                var sqrDist = delta.sqrMagnitude;
                if (sqrDist >= minDist * minDist) continue;

                var dist = Mathf.Sqrt(Mathf.Max(0.0001f, sqrDist));
                var dir = dist > 0.01f ? delta / dist : DeterministicSeparationDirection(self, other);
                var overlap = 1f - Mathf.Clamp01(dist / minDist);
                var weight = other.Kind == V1EnemyKind.Gatekeeper ? 1.45f : 1f;
                push += dir * overlap * weight;
                count++;
            }

            if (count <= 0) return Vector2.zero;
            return Vector2.ClampMagnitude(push / Mathf.Sqrt(count), EnemySeparationMax);
        }

        Vector2 DeterministicSeparationDirection(V1Enemy self, V1Enemy other)
        {
            var hash = Mathf.Abs((self.GetInstanceID() * 73856093) ^ (other.GetInstanceID() * 19349663));
            var angle = (hash % 360) * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        public void RecordVoidPriestHealAttempt(bool accepted)
        {
            debugVoidPriestHealAttempts++;
            if (accepted) debugVoidPriestHealAccepted++;
        }

        public void SpawnVoidPriestHealVfx(Vector3 from, Vector3 to, float amount)
        {
            var source = from + Vector3.up * 0.18f;
            var target = to + Vector3.up * 0.12f;
            var green = new Color(0.34f, 1f, 0.64f, 0.72f);
            SpawnEchoLink("VoidPriestHealThread", source, target, green, 0.34f, 0.026f);
            SpawnTransientSprite("VoidPriestHealSourcePulse", MakeRingSprite("VoidPriestHealSourcePulse", Color.white, 112), source, Quaternion.Euler(0f, 0f, elapsed * -120f), 0.42f, new Color(0.30f, 1f, 0.62f, 0.48f), 0.34f);
            SpawnTransientSprite("VoidPriestHealTargetPulse", MakeRingSprite("VoidPriestHealTargetPulse", Color.white, 128), target, Quaternion.identity, 0.52f, new Color(0.36f, 1f, 0.66f, 0.58f), 0.38f);
            SpawnTransientSprite("VoidPriestHealCore", MakeImpactDiamondSprite("VoidPriestHealCore", Color.white), target, Quaternion.Euler(0f, 0f, 45f), 0.18f, new Color(0.82f, 1f, 0.70f, 0.82f), 0.26f);
            SpawnFloatingText(target + Vector3.up * 0.32f, $"+{amount:0.#}", new Color(0.58f, 1f, 0.70f));
        }

        void DealDamage(V1Enemy enemy, float amount, string source, bool weaponHit, Vector2 hitDir = default, float knockStrength = 0f)
        {
            if (enemy == null || !enemy.IsAlive) return;
            var before = enemy.Hp;
            var finalAmount = weaponHit ? amount * (1f + WeaponStat.DamageMul) : amount;
            if (enemy.GatekeeperGuardActive)
            {
                finalAmount *= weaponHit ? 0.55f : 0.72f;
                if (weaponHit)
                {
                    SpawnTransientSprite("GatekeeperGuardClash", MakeRingSprite("GatekeeperGuardClash", Color.white, 96), enemy.transform.position, Quaternion.identity, 0.58f, new Color(1f, 0.78f, 0.36f, 0.34f), 0.16f);
                }
            }
            var feedback = CurrentWeaponSpec().VfxProfile;
            if (weaponHit && enemy.BloodMarked)
            {
                var bloodLevel = EchoLevel(V1MemoryId.BloodReflection);
                if (bloodLevel > 0)
                {
                    var heal = 0.55f + bloodLevel * 0.18f;
                    HealPlayer(heal);
                    PlaySfx("blood_heal", 0.34f, 0.09f);
                    SpawnBloodThread(enemy.transform.position, heal, bloodLevel);
                    if (bloodLevel >= 5 && UnityEngine.Random.value < 0.16f)
                    {
                        BloodBloom(enemy, bloodLevel);
                    }
                }
            }
            var flashColor = feedback != null
                ? weaponHit ? feedback.enemyWeaponFlashColor : feedback.enemyNonWeaponFlashColor
                : Color.white;
            var flashDuration = feedback != null
                ? weaponHit ? feedback.enemyWeaponFlashDuration : feedback.enemyNonWeaponFlashDuration
                : weaponHit ? 0.105f : 0.075f;
            enemy.TakeDamage(finalAmount, source, weaponHit, flashColor, flashDuration);
            if (weaponHit)
            {
                var impactDelay = CurrentWeaponSpec().Id == V1WeaponId.Greatsword ? GreatswordSlashDelay : DualBladeSlashDelay;
                StartCoroutine(SpawnWeaponImpactFeedbackDelayed(enemy.transform.position, hitDir, impactDelay));
            }
            else
            {
                SpawnHitSpark(enemy.transform.position, hitDir, false);
            }
            var showDamageNumber = feedback == null || feedback.showDamageNumbers;
            var minNonWeaponDamage = feedback != null ? feedback.damageNumberMinNonWeaponDamage : 5f;
            if (showDamageNumber && (weaponHit || finalAmount >= minNonWeaponDamage))
            {
                SpawnDamageNumber(enemy.transform.position, finalAmount, weaponHit, feedback);
            }
            if (hitDir.sqrMagnitude > 0.01f && knockStrength > 0f)
            {
                enemy.ApplyHitFeedback(hitDir.normalized, knockStrength);
            }
            if (before > 0f && !enemy.IsAlive)
            {
                OnEnemyKilled(enemy);
            }
        }

        void HealPlayer(float amount)
        {
            if (amount <= 0f || deathOverlay) return;
            var before = playerHp;
            playerHp = Mathf.Min(playerMaxHp, playerHp + amount);
            if (playerHp > before + 0.01f)
            {
                SpawnFloatingText(player.position + Vector3.up * 0.55f, $"+{playerHp - before:0}", new Color(1f, 0.24f, 0.32f));
            }
        }

        public void ProjectileHit(V1Enemy enemy, float damage, string source)
        {
            var impactPos = enemy != null ? enemy.transform.position : Vector3.zero;
            var isHunter = source == HunterOathSource;
            var isHunterEcho = source == HunterEchoSource;
            DealDamage(enemy, damage, source, false);
            if (isHunter || isHunterEcho)
            {
                MarkEnemyEchoState(enemy, V1MemoryId.HunterOath, isHunterEcho ? 1.20f : 1.00f, isHunterEcho ? 1.02f : 0.92f);
                SpawnHunterOathImpact(impactPos, damage, isHunterEcho);
            }
        }

        public V1Enemy FindNearestLivingEnemy(Vector3 origin, V1Enemy exclude = null)
        {
            return enemies
                .Where(e => e != null && e.IsAlive && e != exclude)
                .OrderBy(e => Vector2.Distance(origin, e.transform.position))
                .FirstOrDefault();
        }

        public List<V1Enemy> FindVoidPriestHealTargets(V1Enemy priest, float radius, int targetCap)
        {
            if (priest == null) return new List<V1Enemy>();
            var origin = priest.transform.position;
            var radiusSq = radius * radius;
            return enemies
                .Where(e => e != null
                    && e != priest
                    && e.IsAlive
                    && e.Kind != V1EnemyKind.Gatekeeper
                    && e.HealthRatio < 0.999f
                    && ((Vector2)(e.transform.position - origin)).sqrMagnitude < radiusSq)
                .OrderBy(e => e.HealthRatio)
                .ThenBy(e => ((Vector2)(e.transform.position - origin)).sqrMagnitude)
                .Take(targetCap)
                .ToList();
        }

        void SpawnHunterOathImpact(Vector3 center, float primaryDamage, bool echo)
        {
            var radius = echo ? 0.72f : 0.92f;
            var splashDamage = primaryDamage * (echo ? 0.30f : 0.42f);
            SpawnTransientSprite(echo ? "HunterEchoLockBurst" : "HunterOathLockBurst", MakeRingSprite("HunterOathLockBurst", Color.white, 128), center, Quaternion.Euler(0f, 0f, elapsed * 120f), radius * 0.98f, echo ? new Color(0.74f, 1f, 0.42f, 0.54f) : new Color(0.78f, 1f, 0.30f, 0.62f), echo ? 0.42f : 0.36f);
            SpawnTransientSprite(echo ? "HunterEchoCore" : "HunterOathCore", MakeImpactDiamondSprite("HunterOathCore", Color.white), center, Quaternion.identity, echo ? 0.22f : 0.26f, echo ? new Color(0.90f, 1f, 0.50f, 0.76f) : new Color(0.94f, 1f, 0.38f, 0.88f), echo ? 0.34f : 0.30f);
            foreach (var target in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(center, e.transform.position) <= radius + e.TouchRadius).Take(echo ? 3 : 4).ToList())
            {
                var dir = (Vector2)(target.transform.position - center);
                DealDamage(target, splashDamage, echo ? HunterEchoBurstSource : HunterOathBurstSource, false, dir.sqrMagnitude > 0.01f ? dir.normalized : Vector2.up, echo ? 0.12f : 0.22f);
            }
        }

        void OnEnemyKilled(V1Enemy enemy)
        {
            kills++;
            if (enemy.Kind != V1EnemyKind.Gatekeeper) PlaySfx("kill", 0.65f, 0.045f);
            var xpAmount = KillXpAmount(enemy);
            SpawnXpOrb(enemy.transform.position, xpAmount);
            SpawnFloatingText(enemy.transform.position, $"+{xpAmount}", elapsed < 120f ? new Color(0.48f, 1f, 1f) : Color.white);
            if (enemy.Kind == V1EnemyKind.SplitOne)
            {
                for (int i = 0; i < 2; i++) SpawnEnemy(V1EnemyKind.Eroder, enemy.transform.position + (Vector3)(UnityEngine.Random.insideUnitCircle.normalized * 0.45f));
            }
            if (enemy.Kind == V1EnemyKind.Gatekeeper)
            {
                Log("문지기 처치: 망각 발생");
                gatekeeperKills++;
                SpawnBossClearCue(enemy.transform.position);
                PlaySfx("boss_clear");
                bossSpawnIndex = Mathf.Min(bossSpawnIndex + 1, BossSchedule().Length);
                if (!fastDebugRun && bossSpawnIndex >= BossSchedule().Length)
                {
                    EndRun(true, "모든 관문 돌파");
                    return;
                }
                bossTimer = NextBossDelay();
                ForgetHighestMemory();
            }
        }

        void UpdatePlayerSprite(Vector2 move, float dt)
        {
            var moveSpeed = playerMoveVelocity.magnitude;
            var moving = moveSpeed > 0.08f;
            var facing = moving ? move.normalized : lastAim;
            if (Mathf.Abs(facing.x) > Mathf.Abs(facing.y) * 0.85f)
            {
                playerFacingRow = facing.x < 0f ? 2 : 3;
            }
            else
            {
                playerFacingRow = facing.y > 0f ? 1 : 0;
            }

            if (moving)
            {
                var speedFactor = Mathf.Clamp(moveSpeed / PlayerSpeed, 0.35f, 1.15f);
                playerAnimTimer += dt * speedFactor;
                playerWalkCycle += dt * (6.2f + speedFactor * 1.35f);
                if (playerAnimTimer >= 0.16f)
                {
                    playerAnimTimer = 0f;
                    playerAnimFrame = (playerAnimFrame + 1) % 4;
                }
            }
            else
            {
                playerAnimTimer = 0f;
                playerWalkCycle = Mathf.Lerp(playerWalkCycle, 0f, 1f - Mathf.Exp(-10f * dt));
                playerAnimFrame = 0;
            }

            var row = (moving ? 4 : 0) + playerFacingRow;
            playerSprite.flipX = false;
            playerSprite.sprite = LoadSheetFrame(PlayerSheetPath, 4, 8, playerAnimFrame, row)
                ?? LoadSheetFrame(PlayerSheetPath, 4, 8, 0, 0)
                ?? playerSprite.sprite;

            if (playerVisual != null)
            {
                var bob = moving ? Mathf.Abs(Mathf.Sin(playerWalkCycle)) * 0.015f : 0f;
                var tilt = moving ? Mathf.Clamp(-move.x * 1.65f, -1.8f, 1.8f) : 0f;
                playerVisual.localPosition = Vector3.Lerp(playerVisual.localPosition, new Vector3(0f, bob, 0f), 1f - Mathf.Exp(-14f * dt));
                playerVisual.localRotation = Quaternion.Euler(0f, 0f, Mathf.LerpAngle(playerVisual.localEulerAngles.z, tilt, 1f - Mathf.Exp(-12f * dt)));
            }
        }

        int KillXpAmount(V1Enemy enemy)
        {
            if (enemy == null) return 1;
            if (enemy.Kind == V1EnemyKind.Gatekeeper) return 18;
            if (fastDebugRun) return Mathf.Max(1, enemy.Score);
            var roleMul = enemy.Kind switch
            {
                V1EnemyKind.Eroder => 1.00f,
                V1EnemyKind.DriftingEye => 1.28f,
                V1EnemyKind.SplitOne => 1.38f,
                V1EnemyKind.VoidPriest => 1.68f,
                _ => 1f
            };
            return Mathf.Max(1, Mathf.RoundToInt(SteppedBaseXpPerKill() * roleMul));
        }

        float SteppedBaseXpPerKill()
        {
            if (elapsed < 60f) return 1.18f;
            if (elapsed < 150f) return 1.36f;
            if (elapsed < 300f) return 1.72f;
            if (elapsed < 540f) return 2.72f;
            if (elapsed < 900f) return 4.18f;
            return 5.10f;
        }

        float NextBossDelay()
        {
            var schedule = BossSchedule();
            if (bossSpawnIndex >= schedule.Length) return Mathf.Max(20f, RunSeconds - elapsed);
            return Mathf.Max(12f, schedule[bossSpawnIndex] - elapsed);
        }

        void GrantXp(int amount)
        {
            var earlyMultiplier = XpMultiplierForTime();
            xp += Mathf.RoundToInt(amount * earlyMultiplier);
            while (xp >= nextXp)
            {
                xp -= nextXp;
                level++;
                nextXp = NextXpAfterLevelUp();
                currentLevelUpChoices.Clear();
                currentLevelUpChoices.AddRange(BuildChoices());
                SpawnLevelUpCue();
                pausedForChoice = true;
                Log($"레벨업 Lv.{level}");
                break;
            }
        }

        float XpMultiplierForTime()
        {
            if (fastDebugRun) return elapsed < 120f ? 1.0f : (elapsed < 600f ? 1.34f : 1f);
            if (elapsed < 60f) return 1.08f;
            if (elapsed < 150f) return 1.06f;
            if (elapsed < 300f) return 1.08f;
            if (elapsed < 540f) return 1.06f;
            if (elapsed < 900f) return 1.02f;
            return 1f;
        }

        int NextXpAfterLevelUp()
        {
            if (fastDebugRun)
            {
                return Mathf.RoundToInt(nextXp * (level < 10 ? 1.24f : 1.42f) + (level < 10 ? 3f : 4f));
            }
            if (level < 10) return Mathf.RoundToInt(nextXp * 1.32f + 5f);
            if (level < 16) return Mathf.RoundToInt(nextXp * 1.20f + 4f);
            return Mathf.RoundToInt(nextXp * 1.18f + 5f);
        }

        void UpdateXpCollection(float dt)
        {
            foreach (var orb in xpOrbs.ToArray())
            {
                if (orb == null) continue;
                orb.Tick(player, dt);
            }
        }

        void SpawnLevelUpCue()
        {
            if (player == null) return;
            PlaySfx("levelup");
            SpawnTransientSprite("LevelUpOuterRing", MakeRingSprite("LevelUpOuterRing", Color.white, 168), player.position, Quaternion.identity, 1.05f, new Color(0.28f, 0.94f, 1f, 0.58f), 0.70f);
            SpawnTransientSprite("LevelUpInnerRing", MakeRingSprite("LevelUpInnerRing", Color.white, 128), player.position, Quaternion.Euler(0f, 0f, elapsed * -90f), 0.58f, new Color(0.94f, 1f, 0.70f, 0.52f), 0.54f);
            SpawnTransientSprite("LevelUpCoreFlash", MakeImpactDiamondSprite("LevelUpCoreFlash", Color.white), player.position + Vector3.up * 0.16f, Quaternion.Euler(0f, 0f, 45f), 0.40f, new Color(1f, 0.96f, 0.72f, 0.86f), 0.42f);
            SpawnFloatingText(player.position + Vector3.up * 1.18f, $"Lv.{level}", new Color(0.92f, 1f, 0.72f));
        }

        void SpawnXpOrb(Vector3 position, int amount)
        {
            var go = RentPooled(xpOrbPool, "XP_Orb");
            go.transform.position = position;
            var sr = go.GetComponent<SpriteRenderer>();
            if (sr == null) sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = MakeCircleSprite("xp", new Color(0.25f, 0.95f, 1f), 24);
            sr.sortingOrder = 32;
            var orb = go.GetComponent<V1XpOrb>();
            if (orb == null) orb = go.AddComponent<V1XpOrb>();
            orb.Configure(this, amount);
            xpOrbs.Add(orb);
        }

        public void CollectXpOrb(V1XpOrb orb, int amount)
        {
            xpOrbs.Remove(orb);
            PlaySfx("xp", 0.35f, 0.035f);
            GrantXp(amount);
            ReleaseXpOrb(orb);
        }

        void AddMemory(V1MemoryId id, int targetLevel, bool allowUpgrade)
        {
            var existing = activeMemories.FirstOrDefault(m => m.Id == id);
            if (existing != null)
            {
                existing.Level = Mathf.Clamp(allowUpgrade ? Mathf.Max(existing.Level, targetLevel) : existing.Level + 1, 1, MaxMemoryLevel);
                SpawnMemoryGainVfx(id, existing.Level);
                Log($"{MemoryName(id)} 강화 +{existing.Level}");
                return;
            }

            if (activeMemories.Count >= MaxActiveMemories) return;
            var resonance = Mathf.FloorToInt(EchoLevel(id) / 2f);
            var levelValue = Mathf.Clamp(targetLevel + resonance, 1, MaxMemoryLevel);
            activeMemories.Add(new MemoryState(id, levelValue));
            SpawnMemoryGainVfx(id, levelValue);
            Log(resonance > 0 ? $"공명 획득: {MemoryName(id)} +{targetLevel + resonance}" : $"새 기억: {MemoryName(id)} +{targetLevel}");
        }

        void ForgetHighestMemory()
        {
            if (activeMemories.Count == 0) return;
            var forgotten = activeMemories.OrderByDescending(m => m.Level).ThenByDescending(m => m.RecentOrder).First();
            activeMemories.Remove(forgotten);
            memoriesForgotten++;
            var before = EchoLevel(forgotten.Id);
            var raw = before + forgotten.Level;
            var after = Mathf.Min(MaxEchoLevel, raw);
            echoLevels[forgotten.Id] = after;
            lastForgotten = forgotten.Id;

            overlayTitle = "망각 결과";
            overlayBody =
                $"사라진 기억: {MemoryName(forgotten.Id)} +{forgotten.Level}\n" +
                $"남은 잔향: {EchoName(forgotten.Id)} +{after}{(after >= MaxEchoLevel ? " 각성" : "")}\n" +
                (raw > MaxEchoLevel ? $"과부하: +{raw - MaxEchoLevel} 즉시 폭발/궁극 기대\n" : "") +
                "다음: 잃은 기억은 되돌아오지 않고, 남은 잔향으로 다음 구간을 이어갑니다.\n" +
                "Space로 전투 복귀";
            ConfigureForgetOverlay(forgotten, after, raw);
            ConfigureForgetOverlayReadable(forgotten, after, raw);
            resultOverlay = true;
            SpawnEchoTransformVfx(forgotten.Id);
            SpawnForgetResonanceFlowVfx(forgotten.Id, forgotten.Level, after, raw);
            if (after >= MaxEchoLevel && AnyUltimateReady)
            {
                SpawnUltimateReadyCue();
            }
            SpawnFloatingText(player.position + Vector3.up * 1.35f, $"{MemoryName(forgotten.Id)} 상실", new Color(1f, 0.82f, 0.74f));
            SpawnFloatingText(player.position + Vector3.up * 1.05f, $"{EchoName(forgotten.Id)} +{after}", forgotten.Id == V1MemoryId.BloodReflection ? new Color(1f, 0.22f, 0.28f) : new Color(0.62f, 0.96f, 1f));
            Log($"{MemoryName(forgotten.Id)} 망각 -> {EchoName(forgotten.Id)} +{after}");
        }

        void ConfigureForgetOverlay(MemoryState forgotten, int echoLevel, int rawEchoLevel)
        {
            overlayTitle = "망각 결과";
            overlayBody =
                $"사라진 기억: {MemoryName(forgotten.Id)} +{forgotten.Level}\n" +
                $"남은 잔향: {EchoName(forgotten.Id)} +{echoLevel}{(echoLevel >= MaxEchoLevel ? " 각성" : "")}\n" +
                (rawEchoLevel > MaxEchoLevel ? $"초과 잔향 +{rawEchoLevel - MaxEchoLevel}: 궁극 준비 가속\n" : "") +
                "다음: 기억 재획득 없이 잔향만 남기고 바로 전투로 돌아갑니다.\n" +
                "Space로 전투 복귀";
        }

        void ConfigureForgetOverlayReadable(MemoryState forgotten, int echoLevel, int rawEchoLevel)
        {
            var resonanceStart = Mathf.Max(1, 1 + Mathf.FloorToInt(echoLevel / 2f));
            var awaken = echoLevel >= MaxEchoLevel ? " 각성" : string.Empty;
            var overflow = rawEchoLevel > MaxEchoLevel ? $"\nOverflow Echo +{rawEchoLevel - MaxEchoLevel}: ultimate charge" : string.Empty;
            overlayTitle = "망각 -> 잔향";
            overlayBody =
                $"{MemoryName(forgotten.Id)} +{forgotten.Level} 소멸\n" +
                $"{EchoName(forgotten.Id)} +{echoLevel}{awaken}{overflow}\n" +
                $"공명 재획득 시작 +{resonanceStart}\n" +
                "Space: 전투 복귀";
        }

        void SpawnMemoryGainVfx(V1MemoryId id, int levelValue)
        {
            if (id == V1MemoryId.HungryBlades)
            {
                SpawnTransientSprite("MemoryGain_KalmuriRing", MakeRingSprite("MemoryGain_KalmuriRing", Color.white, 160), player.position, Quaternion.identity, 0.82f + levelValue * 0.06f, new Color(0.48f, 0.96f, 1f, 0.42f), 0.42f);
                var spiralCount = 10 + levelValue * 2;
                for (int i = 0; i < spiralCount; i++)
                {
                    var angle = elapsed * 150f + i * 360f / spiralCount;
                    var dir = Quaternion.Euler(0f, 0f, angle) * Vector3.right;
                    var start = player.position + dir * (0.26f + i * 0.015f);
                    var end = player.position + dir * (0.78f + (i % 3) * 0.13f + levelValue * 0.025f);
                    SpawnKalmuriDiveBlade("MemoryGain_KalmuriSpiralBlade", start, end, 0.15f + levelValue * 0.012f + (i % 3) * 0.010f, new Color(0.72f, 1f, 1f, 0.76f), 0.42f, 0.20f);
                }
                for (int i = 0; i < 12; i++)
                {
                    var angle = elapsed * 120f + i * 30f;
                    var radius = 0.62f + (i % 2) * 0.28f + levelValue * 0.045f;
                    SpawnOrbitingKalmuriBlade("MemoryGain_Kalmuri", player.position, radius, angle, angle + 34f, 0.16f + levelValue * 0.014f, new Color(0.70f, 0.98f, 1f, 0.82f), 0.42f);
                }
                return;
            }

            if (id == V1MemoryId.BloodReflection)
            {
                SpawnPromptSprite("MemoryGain_Blood", LoadSprite("Assets/_dev/Art/Sprites/Echoes/Blood/spr_blood_mark_01.png"), () => MakeRingSprite("MemoryGain_Blood", Color.white, 128), player.position, Quaternion.identity, 1.18f, 0.58f, new Color(1f, 0.18f, 0.25f, 0.72f), 0.52f);
                return;
            }

            SpawnOneUtilityPreview(id, player.position + Vector3.up * 0.92f, false, elapsed * 80f);
        }

        void ContinueAfterForgetResult()
        {
            resultOverlay = false;
            refillOverlay = false;
            refillTimer = 0f;
            Log("망각 완료: 잔향 유지, 전투 복귀");
        }

        void SpawnEchoTransformVfx(V1MemoryId id)
        {
            var color = id == V1MemoryId.BloodReflection ? new Color(1f, 0.1f, 0.18f, 0.95f) : new Color(0.58f, 0.95f, 1f, 0.95f);
            SpawnTransientSprite("EchoTransformOuter", MakeRingSprite("EchoTransformOuter", Color.white, 180), player.position, Quaternion.identity, 1.18f, new Color(color.r, color.g, color.b, 0.48f), 0.54f);
            SpawnTransientSprite("EchoTransformCore", MakeImpactDiamondSprite("EchoTransformCore", Color.white), player.position + Vector3.up * 0.12f, Quaternion.Euler(0f, 0f, 45f), 0.48f, new Color(color.r, color.g, color.b, 0.86f), 0.42f);
            var shard = MakeImpactDiamondSprite("EchoTransformShard", Color.white);
            for (int i = 0; i < 12; i++)
            {
                var angle = i * 30f + elapsed * 28f;
                var pos = player.position + Quaternion.Euler(0f, 0f, angle) * Vector3.right * (0.72f + i * 0.035f);
                SpawnTransientSprite("EchoTransformShard", shard, pos, Quaternion.Euler(0f, 0f, angle + 45f), 0.10f + i * 0.008f, new Color(color.r, color.g, color.b, 0.64f), 0.50f);
            }
        }

        void SpawnForgetResonanceFlowVfx(V1MemoryId id, int memoryLevel, int echoLevel, int rawEchoLevel)
        {
            var echoColor = id == V1MemoryId.BloodReflection ? new Color(1f, 0.12f, 0.20f, 0.88f) : new Color(0.62f, 0.96f, 1f, 0.86f);
            var memoryColor = new Color(1f, 0.82f, 0.62f, 0.72f);
            var left = player.position + Vector3.left * 0.58f + Vector3.up * 0.24f;
            var right = player.position + Vector3.right * 0.62f + Vector3.up * 0.24f;
            var resonanceBonus = Mathf.FloorToInt(echoLevel / 2f);

            SpawnPromptSprite("ForgetFlowLostMemory", MemoryVfxSprite(id), () => MakeImpactDiamondSprite("ForgetFlowLostMemory", Color.white), left, Quaternion.Euler(0f, 0f, -24f), 0.78f, 0.48f, memoryColor, 0.54f);
            SpawnTransientSprite("ForgetFlowBreakRing", MakeRingSprite("ForgetFlowBreakRing", Color.white, 128), left, Quaternion.identity, 0.56f + memoryLevel * 0.035f, new Color(1f, 0.62f, 0.42f, 0.44f), 0.46f);
            SpawnPromptSprite("ForgetFlowGainedEcho", EchoVfxSprite(id), () => MakeRingSprite("ForgetFlowGainedEcho", Color.white, 132), right, Quaternion.Euler(0f, 0f, elapsed * 92f), 0.86f, 0.54f, echoColor, 0.60f);
            SpawnEchoLink("ForgetFlowMemoryToEcho", left, right, new Color(echoColor.r, echoColor.g, echoColor.b, 0.64f), 0.42f, 0.034f);
            SpawnEchoLink("ForgetFlowActionThread", left + Vector3.down * 0.12f, right + Vector3.down * 0.12f, new Color(1f, 0.94f, 0.64f, 0.44f), 0.34f, 0.020f);
            SpawnTransientSprite("ForgetFlowEchoLevelRing", MakeRingSprite("ForgetFlowEchoLevelRing", Color.white, 160), right, Quaternion.Euler(0f, 0f, elapsed * -110f), 0.72f + echoLevel * 0.030f, new Color(echoColor.r, echoColor.g, echoColor.b, 0.44f), 0.52f);

            if (resonanceBonus > 0)
            {
                var target = player.position + Vector3.up * 0.92f;
                SpawnTransientSprite("ForgetFlowResonanceTarget", MakeRingSprite("ForgetFlowResonanceTarget", Color.white, 144), target, Quaternion.identity, 0.48f + resonanceBonus * 0.06f, new Color(0.92f, 0.74f, 1f, 0.52f), 0.50f);
                SpawnEchoLink("ForgetFlowResonanceThread", right, target, new Color(0.90f, 0.62f, 1f, 0.44f), 0.42f, 0.016f + resonanceBonus * 0.002f);
                SpawnFloatingText(target + Vector3.up * 0.22f, $"공명 +{resonanceBonus}", new Color(0.92f, 0.74f, 1f));
            }

            if (echoLevel >= MaxEchoLevel)
            {
                SpawnTransientSprite("ForgetFlowAwakenStamp", MakeImpactDiamondSprite("ForgetFlowAwakenStamp", Color.white), right + Vector3.up * 0.10f, Quaternion.Euler(0f, 0f, 45f), 0.48f, new Color(1f, 0.96f, 0.62f, 0.84f), 0.42f);
                SpawnRadialSlashLines("ForgetFlowAwakenBurst", right, (Vector2)(right - player.position), 5, 0.62f, new Color(1f, 0.94f, 0.58f, 0.56f), 0.38f);
            }

            if (rawEchoLevel > MaxEchoLevel || AnyUltimateReady)
            {
                SpawnTransientSprite("ForgetFlowUltimateBridge", MakeRingSprite("ForgetFlowUltimateBridge", Color.white, 180), player.position, Quaternion.Euler(0f, 0f, elapsed * 84f), 1.25f, new Color(1f, 0.30f, 0.42f, 0.30f), 0.56f);
            }
        }

        void SpawnUltimateReadyCue()
        {
            if (player == null) return;
            var color = BloodBladeStormReady
                ? new Color(1f, 0.10f, 0.18f, 0.86f)
                : StasisHuntReady
                    ? new Color(1f, 0.72f, 0.22f, 0.82f)
                    : FractureExecutionReady
                        ? new Color(1f, 0.94f, 0.50f, 0.82f)
                        : new Color(0.80f, 0.66f, 1f, 0.78f);
            SpawnTransientSprite("UltimateReadyOuter", MakeRingSprite("UltimateReadyOuter", Color.white, 180), player.position, Quaternion.identity, 1.48f, new Color(color.r, color.g, color.b, 0.50f), 0.62f);
            SpawnTransientSprite("UltimateReadyInner", MakeRingSprite("UltimateReadyInner", Color.white, 144), player.position, Quaternion.Euler(0f, 0f, elapsed * 140f), 0.80f, new Color(1f, 1f, 1f, 0.32f), 0.46f);
            SpawnTransientSprite("UltimateReadyCore", MakeImpactDiamondSprite("UltimateReadyCore", Color.white), player.position + Vector3.up * 0.16f, Quaternion.Euler(0f, 0f, 45f), 0.50f, color, 0.42f);
            SpawnFloatingText(player.position + Vector3.up * 1.25f, $"궁극 준비: {UltimateReadyName()}", color);
            cameraShakeTimer = Mathf.Max(cameraShakeTimer, 0.18f);
            cameraShakeAmount = Mathf.Max(cameraShakeAmount, 0.075f);
        }

        void SetEcho(V1MemoryId id, int levelValue)
        {
            var before = EchoLevel(id);
            echoLevels[id] = Mathf.Clamp(levelValue, 0, MaxEchoLevel);
            if (before < MaxEchoLevel && EchoLevel(id) >= MaxEchoLevel)
            {
                SpawnFloatingText(player.position + Vector3.up * 1.2f, $"{EchoName(id)} 각성", id == V1MemoryId.BloodReflection ? new Color(1f, 0.22f, 0.28f) : new Color(0.62f, 0.96f, 1f));
                SpawnEchoTransformVfx(id);
                if (AnyUltimateReady)
                {
                    SpawnUltimateReadyCue();
                }
            }
            Log($"{EchoName(id)} 디버그 +{EchoLevel(id)}");
        }

        int EchoLevel(V1MemoryId id) => echoLevels.TryGetValue(id, out var levelValue) ? levelValue : 0;

        WeaponRuntimeSpec CurrentWeaponSpec() => currentWeaponId == V1WeaponId.Greatsword
            ? WeaponRuntimeSpec.FromDefinition(V1WeaponId.Greatsword, greatswordDefinition, WeaponRuntimeSpec.Greatsword())
            : WeaponRuntimeSpec.FromDefinition(V1WeaponId.DualBlades, dualBladesDefinition, WeaponRuntimeSpec.DualBlades());

        void SetDebugWeapon(V1WeaponId weaponId)
        {
            currentWeaponId = weaponId;
            weaponTimer = 0f;
            weaponAnimTimer = 0f;
            var weapon = CurrentWeaponSpec();
            Log($"무기 전환: {weapon.DisplayName}");
        }

        void BeginRun(V1WeaponId weaponId)
        {
            if (runStarted) return;
            if (player == null)
            {
                CreatePlayer();
            }
            currentWeaponId = weaponId;
            weaponSelectOverlay = false;
            runStarted = true;
            fastDebugRun = false;
            debugReviewBossHp = false;
            echoOnlyDebugMode = false;
            bossSpawnIndex = 0;
            warnedBossIndex = -1;
            bossTimer = BossSchedule()[0];
            refillTimer = 0f;
            spawnTimer = 0.35f;
            WeaponStat.Reset();
            GameplayPaused = false;
            HitstopActive = false;
            weaponTimer = 0.18f;
            weaponAnimTimer = 0f;
            var weapon = CurrentWeaponSpec();
            PlaySfx("select");
            SpawnFloatingText(player.position + Vector3.up * 1.15f, weapon.DisplayName, new Color(0.78f, 0.96f, 1f));
            Log($"런 시작: {weapon.DisplayName}");
        }

        void EnsureRunStarted()
        {
            if (!runStarted)
            {
                BeginRun(currentWeaponId);
            }
        }

        void ToggleDebugWeapon()
        {
            SetDebugWeapon(currentWeaponId == V1WeaponId.DualBlades ? V1WeaponId.Greatsword : V1WeaponId.DualBlades);
        }

        void DebugSetUtilityMemoryLoadout(V1MemoryId[] ids)
        {
            EnsureRunStarted();
            EnsureReviewEnemies(10);
            echoOnlyDebugMode = false;
            activeMemories.Clear();
            foreach (var id in ids)
            {
                AddMemory(id, 3, true);
            }
            SpawnUtilityPreview(ids, false);
            Log("Debug memory VFX set");
        }

        void DebugSetUtilityEchoLoadout(V1MemoryId[] ids)
        {
            DebugSetEchoOnlyLoadout(ids, "Debug echo-only VFX set");
        }

        void DebugCycleSelectedEcho(int step)
        {
            debugEchoIndex = (debugEchoIndex + step) % AllEchoIds.Length;
            if (debugEchoIndex < 0) debugEchoIndex += AllEchoIds.Length;
            EnsureRunStarted();
            SpawnFloatingText(player.position + Vector3.up * 1.05f, $"Echo Pick: {DebugSelectedEchoName()}", new Color(0.62f, 0.96f, 1f));
            Log($"Debug echo pick: {DebugSelectedEchoName()}");
        }

        void DebugSetSelectedEchoOnly()
        {
            DebugSetEchoOnlyLoadout(new[] { DebugSelectedEchoId() }, $"Debug echo-only {DebugSelectedEchoName()} set");
        }

        void DebugSetSelectedMemoryOnly()
        {
            EnsureRunStarted();
            EnsureReviewEnemies(10);
            echoOnlyDebugMode = false;
            activeMemories.Clear();
            echoLevels.Clear();
            bloodStormWasReady = false;
            bloodStormBurstTimer = 0f;
            ultimatePulseTimer = 0f;

            var id = DebugSelectedEchoId();
            AddMemory(id, MaxMemoryLevel, true);
            SpawnOneUtilityPreview(id, player.position + Vector3.up * 1.24f, false, elapsed * 80f);
            SpawnFloatingText(player.position + Vector3.up * 1.45f, $"Memory Only: {DebugSelectedEchoName()}", new Color(0.78f, 0.96f, 1f));
            Log($"Debug memory-only {DebugSelectedEchoName()} set");
        }

        V1MemoryId DebugSelectedEchoId()
        {
            if (AllEchoIds.Length == 0) return V1MemoryId.HungryBlades;
            debugEchoIndex = Mathf.Clamp(debugEchoIndex, 0, AllEchoIds.Length - 1);
            return AllEchoIds[debugEchoIndex];
        }

        string DebugSelectedEchoName() => EchoName(DebugSelectedEchoId());

        void DebugSetEchoOnlyLoadout(IEnumerable<V1MemoryId> ids, string label)
        {
            EnsureRunStarted();
            EnsureReviewEnemies(14);
            echoOnlyDebugMode = true;
            activeMemories.Clear();
            echoLevels.Clear();
            bloodStormWasReady = false;
            bloodStormBurstTimer = 0f;
            ultimatePulseTimer = 0f;

            var list = ids.Distinct().ToList();
            foreach (var id in list)
            {
                echoLevels[id] = MaxEchoLevel;
            }
            SpawnUtilityPreview(list, true);
            SpawnFloatingText(player.position + Vector3.up * 1.25f, "Echo Only: ultimate off", new Color(0.62f, 0.96f, 1f));
            Log(label);
        }

        void DebugSetUtilityUltimates()
        {
            EnsureRunStarted();
            EnsureReviewEnemies(14);
            echoOnlyDebugMode = false;
            foreach (var id in UtilityMemoryIds)
            {
                echoLevels[id] = MaxEchoLevel;
            }
            SpawnUtilityUltimatePreview();
            ultimatePulseTimer = 0f;
            Log("Debug utility ultimates ready");
        }

        void DebugPreviewAllUtilityVfx()
        {
            EnsureRunStarted();
            EnsureReviewEnemies(14);
            SpawnUtilityPreview(UtilityMemoryIds, false);
            SpawnUtilityPreview(UtilityMemoryIds, true);
            SpawnUtilityUltimatePreview();
            Log("Debug utility VFX preview");
        }

        void DebugIntegratedReview(V1WeaponId weaponId)
        {
            EnsureRunStarted();
            SetDebugWeapon(weaponId);
            EnsureReviewEnemies(18);
            activeMemories.Clear();
            echoLevels.Clear();
            foreach (var id in AllEchoIds)
            {
                echoLevels[id] = MaxEchoLevel;
            }
            echoOnlyDebugMode = true;
            bossSpawnIndex = 0;
            warnedBossIndex = -1;
            bossTimer = 20f;
            weaponTimer = 0.02f;
            weaponAnimTimer = 0f;
            SpawnFloatingText(player.position + Vector3.up * 1.32f, weaponId == V1WeaponId.Greatsword ? "GS VFX Review" : "DB VFX Review", new Color(0.78f, 0.96f, 1f));
            Log(weaponId == V1WeaponId.Greatsword ? "Debug greatsword integrated review" : "Debug dual-blade integrated review");
        }

        public string DebugRunEchoMatrix(V1WeaponId weaponId)
        {
            EnsureRunStarted();
            SetDebugWeapon(weaponId);
            EnsureReviewEnemies(18);
            activeMemories.Clear();
            echoLevels.Clear();
            foreach (var id in AllEchoIds)
            {
                echoLevels[id] = MaxEchoLevel;
            }
            echoOnlyDebugMode = true;
            bloodStormWasReady = false;
            bloodStormBurstTimer = 0f;
            ultimatePulseTimer = 0f;

            var weapon = CurrentWeaponSpec();
            var live = enemies.Where(e => e != null && e.IsAlive).Take(8).ToList();
            for (int i = 0; i < live.Count; i++)
            {
                var enemy = live[i];
                var dir = (Vector2)(enemy.transform.position - player.position);
                if (dir.sqrMagnitude < 0.01f) dir = (Vector2)(Quaternion.Euler(0f, 0f, i * 45f) * Vector3.up);
                SpawnTransientSprite(weaponId == V1WeaponId.Greatsword ? "EchoGreat_KalmuriMatrixMarker" : "EchoDual_KalmuriMatrixMarker", MakeRingSprite("KalmuriMatrixMarker", Color.white, 112), enemy.transform.position, Quaternion.identity, weaponId == V1WeaponId.Greatsword ? 0.72f : 0.48f, new Color(0.66f, 1f, 1f, weaponId == V1WeaponId.Greatsword ? 0.48f : 0.34f), 0.32f);
                TriggerKalmuriEcho(enemy, dir.normalized, MaxEchoLevel, i, weapon);
                TriggerBloodEchoAccent(enemy, dir.normalized, MaxEchoLevel, i, weapon, true);
                TriggerShatterEcho(enemy, dir.normalized, i, weapon, true);
                TriggerExecutionEcho(enemy, dir.normalized, i, weapon, true);
                TriggerHunterEcho(enemy, dir.normalized, i, weapon, true);
                TriggerStoppedEcho(enemy, dir.normalized, i, weapon, true);
                TriggerAshenEcho(enemy, dir.normalized, i, weapon, true);
                TriggerOblivionEcho(enemy, dir.normalized, i, weapon, true);
            }
            UpdatePendingKalmuriFollowups(0.5f);
            SpawnFloatingText(player.position + Vector3.up * 1.44f, weaponId == V1WeaponId.Greatsword ? "Echo Matrix: Great" : "Echo Matrix: Dual", new Color(0.74f, 0.98f, 1f));
            Log(weaponId == V1WeaponId.Greatsword ? "Debug echo matrix greatsword" : "Debug echo matrix dual blades");
            return DebugSnapshot();
        }

        public string DebugRunPassiveMemoryMatrix()
        {
            EnsureRunStarted();
            SetDebugWeapon(V1WeaponId.DualBlades);
            EnsureReviewEnemies(20);
            activeMemories.Clear();
            echoLevels.Clear();
            echoOnlyDebugMode = false;
            bloodStormWasReady = false;
            bloodStormBurstTimer = 0f;
            ultimatePulseTimer = 0f;

            var blood = new MemoryState(V1MemoryId.BloodReflection, MaxMemoryLevel);
            var ash = new MemoryState(V1MemoryId.AshenShield, MaxMemoryLevel);
            var stopped = new MemoryState(V1MemoryId.StoppedSecond, MaxMemoryLevel);
            var oblivion = new MemoryState(V1MemoryId.OblivionBrand, MaxMemoryLevel);
            activeMemories.Add(blood);
            activeMemories.Add(ash);
            activeMemories.Add(stopped);
            activeMemories.Add(oblivion);

            blood.TickTimer = 0f;
            ash.VisualTimer = 0f;
            stopped.TickTimer = 0f;
            oblivion.TickTimer = 0f;
            UpdateBloodReflectionAction(blood, 2f);
            UpdateAshenShield(ash, 2f);
            UpdateStoppedSecond(stopped, 5f);
            UpdateOblivionBrand(oblivion, 3f);

            SpawnFloatingText(player.position + Vector3.up * 1.48f, "Passive Memory Matrix", new Color(0.86f, 0.94f, 1f));
            Log("Debug passive memory matrix");
            return DebugSnapshot();
        }

        public string DebugRunKalmuriPerfMatrix()
        {
            EnsureRunStarted();
            SetDebugWeapon(V1WeaponId.DualBlades);
            EnsureReviewEnemies(32);
            activeMemories.Clear();
            echoLevels.Clear();
            echoOnlyDebugMode = false;
            pendingKalmuriFollowups.Clear();

            var hungry = new MemoryState(V1MemoryId.HungryBlades, MaxMemoryLevel);
            activeMemories.Add(hungry);
            for (int i = 0; i < 4; i++)
            {
                hungry.TickTimer = 0f;
                hungry.VisualSpawnTimer = 0f;
                UpdateHungryBlades(hungry, 0.22f);
            }

            echoLevels[V1MemoryId.HungryBlades] = MaxEchoLevel;
            var weapon = CurrentWeaponSpec();
            var live = enemies.Where(e => e != null && e.IsAlive).Take(8).ToList();
            for (int i = 0; i < live.Count; i++)
            {
                var dir = (Vector2)(live[i].transform.position - player.position);
                if (dir.sqrMagnitude < 0.01f) dir = Quaternion.Euler(0f, 0f, i * 45f) * Vector2.up;
                TriggerKalmuriEcho(live[i], dir.normalized, MaxEchoLevel, i, weapon);
            }
            UpdatePendingKalmuriFollowups(0.5f);

            SpawnFloatingText(player.position + Vector3.up * 1.48f, "Kalmuri Perf Matrix", new Color(0.74f, 0.98f, 1f));
            Log("Debug Kalmuri perf matrix");
            return DebugSnapshot();
        }

        public string DebugRunDenseDualBladePerfMatrix()
        {
            EnsureRunStarted();
            SetDebugWeapon(V1WeaponId.DualBlades);
            pausedForChoice = false;
            resultOverlay = false;
            refillOverlay = false;
            deathOverlay = false;
            fastDebugRun = true;
            debugReviewBossHp = false;
            echoOnlyDebugMode = false;
            pendingKalmuriFollowups.Clear();
            debugTransientSpriteSpawnCount = 0;
            debugDenseDualBladeHits = 0;
            debugDenseDualBladeEchoesSuppressed = 0;
            debugDenseDualBladeTransient = 0;
            debugDenseDualBladeMs = 0f;
            debugForceDenseDualBladeThrottle = true;

            activeMemories.Clear();
            activeMemories.Add(new MemoryState(V1MemoryId.HungryBlades, MaxMemoryLevel));
            echoLevels.Clear();
            for (int i = 0; i < AllEchoIds.Length; i++)
            {
                echoLevels[AllEchoIds[i]] = MaxEchoLevel;
            }

            ClearEnemiesForDebug();
            var center = player.position;
            for (int i = 0; i < 42; i++)
            {
                var lane = i % 14;
                var row = i / 14;
                var angle = Mathf.Lerp(-48f, 48f, lane / 13f);
                var radius = 0.92f + row * 0.34f + (lane % 3) * 0.045f;
                var pos = center + Quaternion.Euler(0f, 0f, angle) * Vector3.right * radius;
                var kind = i % 13 == 0 ? V1EnemyKind.VoidPriest : i % 7 == 0 ? V1EnemyKind.SplitOne : i % 5 == 0 ? V1EnemyKind.DriftingEye : V1EnemyKind.Eroder;
                SpawnEnemy(kind, pos);
            }

            var weapon = CurrentWeaponSpec();
            var forward = Vector2.right;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                for (int cycle = 0; cycle < 3; cycle++)
                {
                    elapsed += 0.08f;
                    var hits = CollectWeaponHits(weapon, forward);
                    debugDenseDualBladeHits += hits.Count;
                    SpawnWeaponHitVfx(weapon, hits, forward);
                    for (int i = 0; i < hits.Count; i++)
                    {
                        var hit = hits[i];
                        if (hit.Enemy == null || !hit.Enemy.IsAlive) continue;
                        DealDamage(hit.Enemy, weapon.Damage * (i == 0 ? 0.08f : 0.05f), weapon.DisplayName, true, hit.Dir.normalized, i == 0 ? weapon.PrimaryKnock : weapon.SecondaryKnock);
                        if (ShouldTriggerWeaponEchoForHit(weapon, i))
                        {
                            TriggerWeaponEchoes(hit.Enemy, forward, i, weapon);
                        }
                        else
                        {
                            debugDenseDualBladeEchoesSuppressed++;
                        }
                    }
                    UpdatePendingKalmuriFollowups(0.24f);
                }
                UpdatePendingKalmuriFollowups(0.60f);
            }
            finally
            {
                debugForceDenseDualBladeThrottle = false;
            }
            stopwatch.Stop();
            debugDenseDualBladeMs = (float)stopwatch.Elapsed.TotalMilliseconds;
            debugDenseDualBladeTransient = debugTransientSpriteSpawnCount;

            SpawnFloatingText(player.position + Vector3.up * 1.52f, $"Dense Dual {debugDenseDualBladeHits} hits / {debugTransientSpriteSpawnCount} vfx", new Color(0.74f, 0.98f, 1f));
            Log($"Debug dense dual blade perf hits={debugDenseDualBladeHits} suppressed={debugDenseDualBladeEchoesSuppressed} transient={debugTransientSpriteSpawnCount} ms={debugDenseDualBladeMs:0.00}");
            return DebugSnapshot();
        }

        public string DebugRunForgetResonanceFlow()
        {
            EnsureRunStarted();
            SetDebugWeapon(V1WeaponId.DualBlades);
            EnsureReviewEnemies(16);
            activeMemories.Clear();
            echoLevels.Clear();
            echoOnlyDebugMode = false;
            bloodStormWasReady = false;
            bloodStormBurstTimer = 0f;
            ultimatePulseTimer = 0f;

            activeMemories.Add(new MemoryState(V1MemoryId.HungryBlades, MaxMemoryLevel));
            activeMemories.Add(new MemoryState(V1MemoryId.BloodReflection, 3));
            activeMemories.Add(new MemoryState(V1MemoryId.AshenShield, 2));
            echoLevels[V1MemoryId.BloodReflection] = MaxEchoLevel;

            SpawnFloatingText(player.position + Vector3.up * 1.56f, "Forget -> Echo -> Resonance", new Color(0.92f, 0.76f, 1f));
            ForgetHighestMemory();
            Log("Debug forget resonance flow");
            return DebugSnapshot();
        }

        public string DebugRunUtilityUltimateMatrix(V1WeaponId weaponId)
        {
            EnsureRunStarted();
            SetDebugWeapon(weaponId);
            EnsureReviewEnemies(24);
            activeMemories.Clear();
            echoOnlyDebugMode = false;
            bloodStormWasReady = false;
            bloodStormBurstTimer = 0f;
            ultimatePulseTimer = 0f;

            echoLevels.Clear();
            echoLevels[V1MemoryId.ShatterWave] = MaxEchoLevel;
            echoLevels[V1MemoryId.ExecutionFlash] = MaxEchoLevel;
            UpdateEchoUltimate(1f);

            ultimatePulseTimer = 0f;
            echoLevels.Clear();
            echoLevels[V1MemoryId.StoppedSecond] = MaxEchoLevel;
            echoLevels[V1MemoryId.HunterOath] = MaxEchoLevel;
            UpdateEchoUltimate(1f);

            ultimatePulseTimer = 0f;
            echoLevels.Clear();
            echoLevels[V1MemoryId.AshenShield] = MaxEchoLevel;
            echoLevels[V1MemoryId.OblivionBrand] = MaxEchoLevel;
            UpdateEchoUltimate(1f);

            SpawnFloatingText(player.position + Vector3.up * 1.48f, weaponId == V1WeaponId.Greatsword ? "Ultimate Matrix: Great" : "Ultimate Matrix: Dual", new Color(1f, 0.84f, 0.48f));
            Log(weaponId == V1WeaponId.Greatsword ? "Debug utility ultimate matrix greatsword" : "Debug utility ultimate matrix dual blades");
            return DebugSnapshot();
        }

        public string DebugJumpToGatekeeper()
        {
            EnsureRunStarted();
            pausedForChoice = false;
            resultOverlay = false;
            refillOverlay = false;
            deathOverlay = false;
            GameplayPaused = false;
            HitstopActive = false;
            echoOnlyDebugMode = false;
            fastDebugRun = true;
            debugReviewBossHp = true;
            gatekeeperKills = 0;
            bossSpawnIndex = 0;
            warnedBossIndex = 0;
            bossTimer = 9999f;
            elapsed = Mathf.Max(elapsed, BossSchedule()[0]);
            playerHp = Mathf.Max(playerHp, playerMaxHp * 0.82f);

            if (activeMemories.Count == 0 && echoLevels.Count == 0)
            {
                AddMemory(V1MemoryId.HungryBlades, 3, true);
                AddMemory(V1MemoryId.BloodReflection, 2, true);
                AddMemory(V1MemoryId.StoppedSecond, 1, true);
            }

            RemoveExistingGatekeepers();
            EnsureReviewEnemies(14);
            SpawnGatekeeperWarning();
            SpawnGatekeeper();
            SpawnFloatingText(player.position + Vector3.up * 1.55f, $"Boss Debug: Gatekeeper HP {DebugReviewBossHp:0}", new Color(1f, 0.52f, 0.30f));
            Log($"Debug gatekeeper jump review HP {DebugReviewBossHp:0}");
            return DebugSnapshot();
        }

        public string DebugRunGatekeeperPatternMatrix()
        {
            EnsureRunStarted();
            debugReviewBossHp = false;
            echoOnlyDebugMode = false;
            var origin = player.position + Vector3.up * 3.8f;
            for (int rank = 0; rank < 4; rank++)
            {
                gatekeeperKills = rank;
                bossSpawnIndex = rank;
                var offset = new Vector3((rank - 1.5f) * 2.25f, rank % 2 == 0 ? 0f : 0.95f, 0f);
                SpawnEnemy(V1EnemyKind.Gatekeeper, origin + offset);
                var gatekeeper = enemies.LastOrDefault(e => e != null && e.IsAlive && e.Kind == V1EnemyKind.Gatekeeper);
                if (gatekeeper != null)
                {
                    GatekeeperPatternPulse(gatekeeper, rank + 1);
                }
            }
            gatekeeperKills = 0;
            bossSpawnIndex = 0;
            SpawnFloatingText(player.position + Vector3.up * 1.52f, "Gatekeeper Pattern Matrix", new Color(1f, 0.56f, 0.32f));
            Log("Debug gatekeeper pattern matrix");
            return DebugSnapshot();
        }

        public string DebugRunEnemySeparationMatrix()
        {
            EnsureRunStarted();
            echoOnlyDebugMode = false;
            var stack = new List<V1Enemy>();
            var center = player.position + Vector3.right * 2.2f + Vector3.up * 0.35f;
            for (int i = 0; i < 14; i++)
            {
                var kind = i % 5 == 0 ? V1EnemyKind.SplitOne : i % 4 == 0 ? V1EnemyKind.DriftingEye : V1EnemyKind.Eroder;
                var jitter = Quaternion.Euler(0f, 0f, i * 137.5f) * Vector3.right * (0.025f * (i % 3));
                SpawnEnemy(kind, center + jitter);
                var enemy = enemies.LastOrDefault(e => e != null && e.IsAlive);
                if (enemy != null) stack.Add(enemy);
            }

            debugSeparationOverlapBefore = CountOverlapPairs(stack);
            for (int step = 0; step < 24; step++)
            {
                for (int i = 0; i < stack.Count; i++)
                {
                    var enemy = stack[i];
                    if (enemy == null || !enemy.IsAlive) continue;
                    enemy.transform.position += (Vector3)(EnemySeparationForce(enemy) * EnemySeparationProbeStep);
                }
            }

            debugSeparationOverlapAfter = CountOverlapPairs(stack);
            SpawnFloatingText(player.position + Vector3.up * 1.44f, $"Enemy Separation {debugSeparationOverlapBefore}->{debugSeparationOverlapAfter}", new Color(0.76f, 1f, 0.82f));
            Log($"Debug enemy separation matrix {debugSeparationOverlapBefore}->{debugSeparationOverlapAfter}");
            return DebugSnapshot();
        }

        public string DebugRunVoidPriestHealMatrix()
        {
            EnsureRunStarted();
            echoOnlyDebugMode = false;
            debugVoidPriestHealAttempts = 0;
            debugVoidPriestHealAccepted = 0;

            var priests = new List<V1Enemy>();
            var targets = new List<V1Enemy>();
            var center = player.position + Vector3.right * 2.7f + Vector3.down * 0.15f;
            for (int i = 0; i < 3; i++)
            {
                SpawnEnemy(V1EnemyKind.VoidPriest, center + Quaternion.Euler(0f, 0f, i * 120f) * Vector3.right * 0.44f);
                var priest = enemies.LastOrDefault(e => e != null && e.IsAlive && e.Kind == V1EnemyKind.VoidPriest);
                if (priest != null) priests.Add(priest);
            }

            for (int i = 0; i < 4; i++)
            {
                var kind = i % 2 == 0 ? V1EnemyKind.Eroder : V1EnemyKind.SplitOne;
                SpawnEnemy(kind, center + Quaternion.Euler(0f, 0f, i * 90f + 45f) * Vector3.right * 0.72f);
                var target = enemies.LastOrDefault(e => e != null && e.IsAlive && e.Kind == kind);
                if (target == null) continue;
                target.TakeDamage(28f, "VoidPriestHealMatrix setup", false, new Color(0.34f, 1f, 0.64f), 0.10f);
                if (target.IsAlive) targets.Add(target);
            }

            for (int p = 0; p < priests.Count; p++)
            {
                for (int t = 0; t < targets.Count; t++)
                {
                    targets[t]?.TryReceiveVoidPriestHeal(2.4f, priests[p]);
                }
            }

            SpawnFloatingText(player.position + Vector3.up * 1.50f, $"Priest Heal {debugVoidPriestHealAccepted}/{debugVoidPriestHealAttempts}", new Color(0.58f, 1f, 0.70f));
            Log($"Debug void priest heal matrix accepted={debugVoidPriestHealAccepted} attempts={debugVoidPriestHealAttempts}");
            return DebugSnapshot();
        }

        int CountOverlapPairs(IReadOnlyList<V1Enemy> group)
        {
            var overlaps = 0;
            for (int i = 0; i < group.Count; i++)
            {
                var a = group[i];
                if (a == null || !a.IsAlive) continue;
                for (int j = i + 1; j < group.Count; j++)
                {
                    var b = group[j];
                    if (b == null || !b.IsAlive) continue;
                    var minDist = a.TouchRadius + b.TouchRadius + EnemySeparationPadding * 0.5f;
                    if (Vector2.Distance(a.transform.position, b.transform.position) < minDist)
                    {
                        overlaps++;
                    }
                }
            }

            return overlaps;
        }

        void EnsureReviewEnemies(int targetCount)
        {
            var live = enemies.Count(e => e != null && e.IsAlive);
            for (int i = live; i < targetCount; i++)
            {
                var angle = i * 137.5f;
                var radius = 1.8f + (i % 5) * 0.42f;
                var pos = player.position + Quaternion.Euler(0f, 0f, angle) * Vector3.right * radius;
                SpawnEnemy((V1EnemyKind)(i % 4), pos);
            }
        }

        void SpawnUtilityPreview(IEnumerable<V1MemoryId> ids, bool echo)
        {
            var list = ids.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var angle = i * (360f / Mathf.Max(1, list.Count)) + (echo ? 18f : 0f);
                var radius = echo ? 2.65f : 1.65f;
                var pos = player.position + Quaternion.Euler(0f, 0f, angle) * Vector3.right * radius;
                SpawnOneUtilityPreview(list[i], pos, echo, angle);
            }
        }

        void SpawnOneUtilityPreview(V1MemoryId id, Vector3 pos, bool echo, float angle)
        {
            var color = UtilityVfxColor(id, echo);
            var sprite = echo ? EchoVfxSprite(id) : MemoryVfxSprite(id);
            var name = echo ? $"PreviewEcho_{id}" : $"PreviewMemory_{id}";
            var width = id switch
            {
                V1MemoryId.ExecutionFlash => echo ? 1.52f : 1.95f,
                V1MemoryId.ShatterWave => echo ? 1.46f : 1.78f,
                V1MemoryId.StoppedSecond => echo ? 1.82f : 2.28f,
                V1MemoryId.AshenShield => echo ? 1.20f : 1.48f,
                V1MemoryId.OblivionBrand => echo ? 1.16f : 1.22f,
                V1MemoryId.HunterOath => echo ? 0.74f : 0.82f,
                _ => echo ? 1.06f : 1.28f
            };
            var fallbackScale = id == V1MemoryId.ExecutionFlash ? 0.72f : id == V1MemoryId.OblivionBrand ? 0.46f : 0.62f;
            var lifetime = id == V1MemoryId.ShatterWave ? echo ? 0.95f : 1.05f : id == V1MemoryId.StoppedSecond ? echo ? 1.20f : 1.75f : echo ? 0.76f : 0.82f;
            SpawnPromptSprite(name, sprite, () => id == V1MemoryId.ExecutionFlash || id == V1MemoryId.OblivionBrand ? MakeImpactDiamondSprite(name, Color.white) : MakeRingSprite(name, Color.white, 128), pos, Quaternion.Euler(0f, 0f, angle), width, fallbackScale, color, lifetime);
            if (id == V1MemoryId.ExecutionFlash)
            {
                SpawnExecutionFlashBurst(pos, echo ? 0.74f : 1.00f, echo ? 0.42f : 0.56f);
            }
            if (id == V1MemoryId.ShatterWave)
            {
                SpawnShatterWaveField(pos, echo ? 0.86f : 1.02f, echo ? 0.95f : 1.05f, echo);
            }
            if (id == V1MemoryId.StoppedSecond)
            {
                SpawnStoppedSecondField(pos, echo ? 1.18f : 1.48f, TimeStopGold(!echo), echo ? 1.25f : 1.75f, !echo);
            }
        }

        void SpawnUtilityUltimatePreview()
        {
            var center = player.position + Vector3.up * 1.70f;
            var fracture = center + Vector3.left * 2.65f;
            var stasis = center;
            var ashen = center + Vector3.right * 2.65f;
            SpawnPromptSprite("Preview_FractureExecution", LoadSprite(UltimateFracturePath), () => MakeRingSprite("Preview_FractureExecution", Color.white, 160), fracture, Quaternion.identity, 2.85f, 1.16f, new Color(1f, 0.94f, 0.62f, 0.70f), 0.82f);
            SpawnPromptSprite("Preview_StasisHunt", LoadSprite(UltimateStasisPath), () => MakeRingSprite("Preview_StasisHunt", Color.white, 160), stasis, Quaternion.identity, 3.18f, 1.24f, new Color(1f, 0.74f, 0.28f, 0.66f), 1.16f);
            SpawnStoppedSecondField(stasis, 1.65f, TimeStopGold(true), 1.50f, true);
            SpawnPromptSprite("Preview_AshenOblivion", LoadSprite(UltimateAshenOblivionPath), () => MakeRingSprite("Preview_AshenOblivion", Color.white, 180), ashen, Quaternion.identity, 3.24f, 1.30f, new Color(0.78f, 0.68f, 1f, 0.60f), 0.82f);
        }

        static Color UtilityVfxColor(V1MemoryId id, bool echo)
        {
            var alpha = echo ? 0.58f : 0.78f;
            return id switch
            {
                V1MemoryId.ExecutionFlash => new Color(1f, 0.94f, 0.62f, alpha),
                V1MemoryId.HunterOath => new Color(0.86f, 1f, 0.58f, alpha),
                V1MemoryId.ShatterWave => new Color(0.60f, 0.92f, 1f, alpha),
                V1MemoryId.StoppedSecond => new Color(1f, 0.72f, 0.20f, Mathf.Min(0.86f, alpha + 0.10f)),
                V1MemoryId.AshenShield => new Color(0.82f, 0.88f, 0.94f, alpha),
                V1MemoryId.OblivionBrand => new Color(0.78f, 0.48f, 1f, alpha),
                _ => new Color(0.70f, 0.96f, 1f, alpha)
            };
        }

        void DrawWeaponSelectOverlay()
        {
            DrawFilledRect(new Rect(0, 0, Screen.width, Screen.height), new Color(0.015f, 0.022f, 0.026f, 0.96f));
            DrawFilledRect(new Rect(0, Screen.height * 0.54f, Screen.width, Screen.height * 0.46f), new Color(0.02f, 0.12f, 0.13f, 0.22f));
            DrawFilledRect(new Rect(0, 0, Screen.width, 5f), new Color(0.24f, 0.78f, 0.82f, 0.80f));
            DrawFilledRect(new Rect(0, Screen.height - 5f, Screen.width, 5f), new Color(0.42f, 0.12f, 0.15f, 0.82f));

            var compact = Screen.height < 520;
            var width = Mathf.Min(1060f, Screen.width - 72f);
            var height = Mathf.Min(compact ? 374f : 620f, Screen.height - 32f);
            var origin = new Rect(Screen.width * 0.5f - width * 0.5f, Screen.height * 0.5f - height * 0.5f, width, height);
            GUI.Box(origin, "", panelStyle);

            DrawFilledRect(new Rect(origin.x + 24f, origin.y + 20f, origin.width - 48f, 2f), new Color(0.52f, 0.94f, 0.98f, 0.55f));
            GUI.Label(new Rect(origin.x + 34, origin.y + 30, origin.width - 68, 20), "망각의 강으로 들어가기 전", startEyebrowStyle);
            GUI.Label(new Rect(origin.x + 34, origin.y + 50, origin.width - 68, 40), "LETHE", titleStyle);
            GUI.Label(
                new Rect(origin.x + 72, origin.y + 92, origin.width - 144, compact ? 26f : 48f),
                compact
                    ? "무기를 고르면 바로 시작합니다. 기억은 첫 보상 카드에서 선택합니다."
                    : "무기는 첫 손맛을 결정합니다. 기억은 첫 보상 카드에서 고르고, 문지기를 쓰러뜨리면 가장 키운 기억이 잔향으로 남습니다.",
                startBodyStyle);

            var goal = new Rect(origin.x + 54f, origin.y + (compact ? 128f : 156f), origin.width - 108f, compact ? 36f : 48f);
            DrawFilledRect(goal, new Color(0.035f, 0.07f, 0.075f, 0.82f));
            DrawFilledRect(new Rect(goal.x, goal.y, 4f, goal.height), new Color(0.32f, 0.88f, 1f, 0.86f));
            GUI.Label(new Rect(goal.x + 18f, goal.y + 5f, goal.width - 36f, 16f), "첫 목표", startEyebrowStyle);
            GUI.Label(new Rect(goal.x + 18f, goal.y + 20f, goal.width - 36f, 16f), "XP로 기억 3칸을 채우고, 첫 문지기 이후 잃은 기억이 어떤 잔향으로 바뀌는지 확인하세요.", startFooterStyle);

            var gap = 20f;
            var cardWidth = (origin.width - 108f - gap) * 0.5f;
            var footer = new Rect(origin.x + 54f, origin.yMax - 44f, origin.width - 108f, 28f);
            var x0 = origin.x + 54f;
            var x1 = x0 + cardWidth + gap;
            var y0 = goal.yMax + (compact ? 14f : 18f);
            var cardHeight = footer.y - y0 - 14f;
            DrawWeaponCard(
                new Rect(x0, y0, cardWidth, cardHeight),
                V1WeaponId.DualBlades,
                "1",
                "절단쌍검",
                "가까운 적을 자동 조준해 두 번 베고 빠르게 다음 타격으로 넘어갑니다.",
                "빠른 온힛 / 짧은 hitstop / 잔향을 자주 발생",
                "칼무리·혈반을 빠르게 쌓아 피의 칼폭풍 루트를 보기 좋습니다.",
                new Color(0.32f, 0.88f, 1f));
            DrawWeaponCard(
                new Rect(x1, y0, cardWidth, cardHeight),
                V1WeaponId.Greatsword,
                "2",
                "장송대검",
                "가장 많이 맞는 방향을 골라 느리고 넓은 참격으로 무리를 갈라냅니다.",
                "묵직한 강타 / 긴 hitstop / 큰 잔향 한 번",
                "파문·처형·낙인처럼 한 방의 후속 효과를 크게 읽기 좋습니다.",
                new Color(0.92f, 0.86f, 0.70f));

            DrawFilledRect(footer, new Color(0.04f, 0.045f, 0.052f, 0.82f));
            GUI.Label(new Rect(footer.x + 16f, footer.y + 5f, footer.width - 32f, 18f), "WASD 이동 · 기본공격 자동 · F12 디버그 · 첫 보상에서 기억 선택", startFooterStyle);
        }

        void DrawWeaponCard(Rect card, V1WeaponId weaponId, string key, string title, string body, string rhythm, string echoHint, Color accent)
        {
            if (GUI.Button(card, "", buttonStyle))
            {
                BeginRun(weaponId);
            }

            DrawFilledRect(new Rect(card.x + 8f, card.y + 8f, card.width - 16f, card.height - 16f), new Color(0.025f, 0.034f, 0.040f, 0.84f));
            DrawFilledRect(new Rect(card.x + 14f, card.y + 14f, card.width - 28f, 4f), accent);
            DrawFilledRect(new Rect(card.x + 22f, card.y + 26f, 42f, 32f), new Color(accent.r, accent.g, accent.b, 0.92f));
            GUI.Label(new Rect(card.x + 22f, card.y + 26f, 42f, 32f), key, startKeyStyle);
            GUI.Label(new Rect(card.x + 72f, card.y + 25f, card.width - 104f, 36f), title, startCardTitleStyle);

            if (card.height < 210f)
            {
                DrawWeaponGlyph(new Rect(card.xMax - 86f, card.y + 58f, 58f, 42f), weaponId, accent);
                var compactInfo = new Rect(card.x + 28f, card.y + 76f, card.width - 126f, 24f);
                DrawFilledRect(compactInfo, new Color(accent.r, accent.g, accent.b, 0.14f));
                var summary = weaponId == V1WeaponId.Greatsword
                    ? "느린 강타 / 큰 잔향 한 번"
                    : "빠른 2연 베기 / 잔향 자주 발생";
                GUI.Label(new Rect(compactInfo.x + 10f, compactInfo.y + 4f, compactInfo.width - 20f, 16f), summary, startFooterStyle);
                var compactSelect = new Rect(card.x + 28f, card.yMax - 32f, card.width - 56f, 22f);
                DrawFilledRect(compactSelect, new Color(accent.r, accent.g, accent.b, 0.16f));
                GUI.Label(new Rect(compactSelect.x + 12f, compactSelect.y + 3f, compactSelect.width - 24f, 16f), "클릭 또는 숫자키로 선택", startFooterStyle);
                return;
            }

            var bladeRect = new Rect(card.x + 26f, card.y + 74f, card.width - 52f, 52f);
            DrawWeaponGlyph(bladeRect, weaponId, accent);

            GUI.Label(new Rect(card.x + 28f, card.y + 138f, card.width - 56f, 48f), body, startBodyStyle);
            var rowWidth = card.width - 56f;
            var select = new Rect(card.x + 28f, card.yMax - 42f, rowWidth, 28f);
            var row2 = new Rect(card.x + 28f, select.y - 56f, rowWidth, 48f);
            var row1 = new Rect(card.x + 28f, row2.y - 48f, rowWidth, 40f);
            DrawStartInfoRow(row1, "전투 리듬", rhythm, accent);
            DrawStartInfoRow(row2, "잔향 방향", echoHint, accent);

            DrawFilledRect(select, new Color(accent.r, accent.g, accent.b, 0.16f));
            GUI.Label(new Rect(select.x + 12f, select.y + 5f, select.width - 24f, 18f), "클릭 또는 숫자키로 선택", startFooterStyle);
        }

        void DrawStartInfoRow(Rect rect, string label, string text, Color accent)
        {
            DrawFilledRect(rect, new Color(0.06f, 0.075f, 0.085f, 0.80f));
            DrawFilledRect(new Rect(rect.x, rect.y, 3f, rect.height), new Color(accent.r, accent.g, accent.b, 0.72f));
            GUI.Label(new Rect(rect.x + 12f, rect.y + 6f, rect.width - 24f, 17f), label, startEyebrowStyle);
            GUI.Label(new Rect(rect.x + 12f, rect.y + 23f, rect.width - 24f, rect.height - 25f), text, startFooterStyle);
        }

        void DrawWeaponGlyph(Rect rect, V1WeaponId weaponId, Color accent)
        {
            DrawFilledRect(rect, new Color(0.04f, 0.055f, 0.064f, 0.72f));
            var cx = rect.x + rect.width * 0.5f;
            var cy = rect.y + rect.height * 0.5f;
            var oldMatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(weaponId == V1WeaponId.Greatsword ? -18f : -32f, new Vector2(cx, cy));
            DrawFilledRect(new Rect(cx - 4f, cy - 28f, 8f, 56f), new Color(accent.r, accent.g, accent.b, 0.92f));
            DrawFilledRect(new Rect(cx - 13f, cy + 18f, 26f, 5f), new Color(0.75f, 0.85f, 0.86f, 0.72f));
            GUI.matrix = oldMatrix;
            if (weaponId == V1WeaponId.DualBlades)
            {
                GUIUtility.RotateAroundPivot(32f, new Vector2(cx, cy));
                DrawFilledRect(new Rect(cx - 4f, cy - 28f, 8f, 56f), new Color(accent.r, accent.g, accent.b, 0.92f));
                DrawFilledRect(new Rect(cx - 13f, cy + 18f, 26f, 5f), new Color(0.75f, 0.85f, 0.86f, 0.72f));
                GUI.matrix = oldMatrix;
            }
            else
            {
                GUIUtility.RotateAroundPivot(-18f, new Vector2(cx, cy));
                DrawFilledRect(new Rect(cx - 9f, cy - 36f, 18f, 74f), new Color(accent.r, accent.g, accent.b, 0.62f));
                GUI.matrix = oldMatrix;
            }
        }

        void DrawHud()
        {
            var weapon = CurrentWeaponSpec();
            GUI.Box(new Rect(12, 12, 430, 238), "", panelStyle);
            GUI.Label(new Rect(24, 20, 224, 24), $"LETHE  {Mathf.FloorToInt(elapsed)}s  {PhaseName()}", smallStyle);
            GUI.Label(new Rect(260, 20, 166, 24), $"{weapon.DisplayName}  Lv.{level}", smallStyle);
            GUI.Label(new Rect(24, 47, 70, 20), $"HP {Mathf.CeilToInt(playerHp)}/{Mathf.CeilToInt(playerMaxHp)}", smallStyle);
            DrawBar(new Rect(98, 52, 318, 10), Mathf.Clamp01(playerHp / playerMaxHp), new Color(0.18f, 0.95f, 0.62f), new Color(0.08f, 0.12f, 0.13f));
            GUI.Label(new Rect(24, 69, 70, 20), $"XP {xp}/{nextXp}", smallStyle);
            DrawBar(new Rect(98, 74, 318, 10), Mathf.Clamp01(nextXp <= 0 ? 0f : (float)xp / nextXp), new Color(0.32f, 0.88f, 1f), new Color(0.07f, 0.10f, 0.13f));
            GUI.Label(new Rect(312, 92, 104, 22), $"Gate {gatekeeperKills}/{BossSchedule().Length}", smallStyle);
            GUI.Label(new Rect(24, 92, 180, 22), $"처치 {kills}", smallStyle);
            GUI.Label(new Rect(210, 92, 206, 22), $"망각 후보 {ForgetCandidateText()}", smallStyle);
            DrawMemoryStrip(new Rect(24, 119, 392, 30));
            GUI.Label(new Rect(24, 151, 392, 20), $"잔향 {EchoText()}", smallStyle);
            GUI.Label(new Rect(24, 172, 392, 20), BloodBladeStormReady ? $"{UltimateGoalText()} / {UltimatePatternText(weapon)}" : UltimateGoalText(), smallStyle);
            GUI.Label(new Rect(24, 193, 392, 20), M2LoopText(), smallStyle);
            GUI.Label(new Rect(24, 214, 392, 22), PlayerGoalText(), smallStyle);

            if (!showDebugPanel)
            {
                GUI.Label(new Rect(Screen.width - 142, 18, 124, 20), "F12 Debug", smallStyle);
                return;
            }

            GUI.Box(new Rect(Screen.width - 326, 12, 314, 406), "", panelStyle);
            GUI.Label(new Rect(Screen.width - 314, 22, 292, 20), "Debug  F1/F2 기억  F4/F5 잔향  F8 M2", smallStyle);
            if (GUI.Button(new Rect(Screen.width - 314, 48, 92, 28), "M1", buttonStyle)) DebugRunM1Smoke();
            if (GUI.Button(new Rect(Screen.width - 216, 48, 92, 28), "M2", buttonStyle)) DebugRunM2Smoke();
            if (GUI.Button(new Rect(Screen.width - 118, 48, 92, 28), "계속", buttonStyle))
            {
                if (resultOverlay) ContinueAfterForgetResult();
            }
            if (GUI.Button(new Rect(Screen.width - 314, 82, 92, 28), "Mem A", buttonStyle)) DebugSetUtilityMemoryLoadout(UtilityMemorySetA);
            if (GUI.Button(new Rect(Screen.width - 216, 82, 92, 28), "Mem B", buttonStyle)) DebugSetUtilityMemoryLoadout(UtilityMemorySetB);
            if (GUI.Button(new Rect(Screen.width - 118, 82, 92, 28), "Echo A", buttonStyle)) DebugSetUtilityEchoLoadout(UtilityMemorySetA);
            if (GUI.Button(new Rect(Screen.width - 314, 116, 92, 28), "Echo B", buttonStyle)) DebugSetUtilityEchoLoadout(UtilityMemorySetB);
            if (GUI.Button(new Rect(Screen.width - 216, 116, 92, 28), "Echo All", buttonStyle)) DebugSetEchoOnlyLoadout(AllEchoIds, "Debug echo-only all 8 set");
            if (GUI.Button(new Rect(Screen.width - 118, 116, 92, 28), "Ult 3", buttonStyle)) DebugSetUtilityUltimates();
            if (GUI.Button(new Rect(Screen.width - 314, 150, 92, 28), "VFX", buttonStyle)) DebugPreviewAllUtilityVfx();
            GUI.Label(new Rect(Screen.width - 216, 154, 190, 20), echoOnlyDebugMode ? "Echo Only: ultimate off" : "Ultimate: normal", smallStyle);
            GUI.Label(new Rect(Screen.width - 314, 184, 292, 20), $"Pick: {DebugSelectedEchoName()}", smallStyle);
            if (GUI.Button(new Rect(Screen.width - 314, 208, 92, 28), "Prev", buttonStyle)) DebugCycleSelectedEcho(-1);
            if (GUI.Button(new Rect(Screen.width - 216, 208, 92, 28), "Mem One", buttonStyle)) DebugSetSelectedMemoryOnly();
            if (GUI.Button(new Rect(Screen.width - 118, 208, 92, 28), "Next", buttonStyle)) DebugCycleSelectedEcho(1);
            if (GUI.Button(new Rect(Screen.width - 314, 242, 92, 28), "Echo One", buttonStyle)) DebugSetSelectedEchoOnly();
            if (GUI.Button(new Rect(Screen.width - 216, 242, 92, 28), "DB Rev", buttonStyle)) DebugIntegratedReview(V1WeaponId.DualBlades);
            if (GUI.Button(new Rect(Screen.width - 118, 242, 92, 28), "GS Rev", buttonStyle)) DebugIntegratedReview(V1WeaponId.Greatsword);
            if (GUI.Button(new Rect(Screen.width - 314, 276, 92, 28), "Boss", buttonStyle)) DebugJumpToGatekeeper();
            if (GUI.Button(new Rect(Screen.width - 216, 276, 92, 28), "Cont", buttonStyle))
            {
                if (resultOverlay) ContinueAfterForgetResult();
            }
            GUI.Label(new Rect(Screen.width - 118, 280, 92, 20), "Rev: all echo", smallStyle);
            var y = 310;
            foreach (var line in combatLog.TakeLast(3))
            {
                GUI.Label(new Rect(Screen.width - 314, y, 292, 20), line, smallStyle);
                y += 19;
            }
        }

        string PlayerGoalText()
        {
            if (resultOverlay) return "사라진 기억은 잔향으로 남습니다. Space로 전투에 복귀하세요.";
            if (BloodBladeStormReady) return "궁극 잔향 준비 완료. 무기 공격으로 피의 칼폭풍을 터뜨리세요.";
            if (activeMemories.Count < MaxActiveMemories) return "XP를 모아 기억 3칸을 채우고 첫 망각 후보를 만드세요.";
            return "가장 높은 기억은 다음 망각 후보입니다. 잔향을 쌓아 궁극 조합을 노리세요.";
        }

        void DrawMemoryStrip(Rect rect)
        {
            var gap = 8f;
            var width = (rect.width - gap * 2f) / 3f;
            for (int i = 0; i < MaxActiveMemories; i++)
            {
                var slot = new Rect(rect.x + i * (width + gap), rect.y, width, rect.height);
                GUI.color = new Color(0.05f, 0.08f, 0.10f, 0.92f);
                GUI.DrawTexture(slot, Texture2D.whiteTexture);
                GUI.color = activeMemories.Count > i ? new Color(0.32f, 0.88f, 1f, 0.78f) : new Color(0.28f, 0.34f, 0.38f, 0.55f);
                GUI.DrawTexture(new Rect(slot.x, slot.y, slot.width, 3f), Texture2D.whiteTexture);
                GUI.color = Color.white;
                var text = activeMemories.Count > i ? $"{MemoryName(activeMemories[i].Id)} +{activeMemories[i].Level}" : "빈 기억";
                GUI.Label(new Rect(slot.x + 8f, slot.y + 6f, slot.width - 16f, slot.height - 8f), text, smallStyle);
            }
        }

        void DrawLevelUpOverlay()
        {
            var width = Mathf.Min(980f, Screen.width - 80f);
            var height = Mathf.Min(430f, Screen.height - 80f);
            var origin = new Rect(Screen.width * 0.5f - width * 0.5f, Screen.height * 0.5f - height * 0.5f, width, height);
            GUI.Box(origin, "", panelStyle);
            GUI.Label(new Rect(origin.x + 32, origin.y + 24, origin.width - 64, 38), $"레벨업 Lv.{level}", titleStyle);
            GUI.Label(new Rect(origin.x + 42, origin.y + 66, origin.width - 84, 28), "하나의 기억 또는 전투 성향을 선택합니다.", smallStyle);
            if (currentLevelUpChoices.Count == 0)
            {
                currentLevelUpChoices.AddRange(BuildChoices());
            }
            var choices = currentLevelUpChoices;
            var cardGap = 18f;
            var cardWidth = (origin.width - 84f - cardGap * 2f) / 3f;
            var cardHeight = origin.height - 138f;
            for (int i = 0; i < choices.Count; i++)
            {
                var choice = choices[i];
                var card = new Rect(origin.x + 42f + i * (cardWidth + cardGap), origin.y + 106f, cardWidth, cardHeight);
                if (GUI.Button(card, "", buttonStyle))
                {
                    choicesTaken++;
                    choice.Apply();
                    currentLevelUpChoices.Clear();
                    pausedForChoice = false;
                }
                GUI.Label(new Rect(card.x + 18, card.y + 18, card.width - 36, 26), choice.Tag, smallStyle);
                GUI.Label(new Rect(card.x + 18, card.y + 50, card.width - 36, 36), choice.Title, titleStyle);
                GUI.Label(new Rect(card.x + 18, card.y + 104, card.width - 36, card.height - 154), choice.Body, smallStyle);
                GUI.Label(new Rect(card.x + 18, card.yMax - 38, card.width - 36, 24), "선택", smallStyle);
            }
        }

        void DrawCenterOverlay()
        {
            GUI.Box(new Rect(Screen.width * 0.5f - 280, Screen.height * 0.5f - 140, 560, 280), "", panelStyle);
            GUI.Label(new Rect(Screen.width * 0.5f - 230, Screen.height * 0.5f - 108, 460, 40), overlayTitle, titleStyle);
            GUI.Label(new Rect(Screen.width * 0.5f - 230, Screen.height * 0.5f - 52, 460, 140), overlayBody, smallStyle);
            if (resultOverlay && GUI.Button(new Rect(Screen.width * 0.5f - 110, Screen.height * 0.5f + 76, 220, 38), "전투 복귀", buttonStyle))
            {
                ContinueAfterForgetResult();
            }
            if (deathOverlay && GUI.Button(new Rect(Screen.width * 0.5f - 110, Screen.height * 0.5f + 76, 220, 38), runWon ? "다시 시작" : "재도전", buttonStyle))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
        }

        List<Choice> BuildChoices()
        {
            var choices = new List<Choice>();
            var priorityTitles = new List<string>();
            var priorityUpgradeIds = new HashSet<V1MemoryId>();
            var ultimateFocus = UltimateFocusMemory();
            if (ultimateFocus != null)
            {
                var focus = ultimateFocus;
                priorityUpgradeIds.Add(focus.Id);
                var choice = new Choice("궁극 준비", MemoryName(focus.Id), $"현재 Lv.{focus.Level} -> Lv.{focus.Level + 1}\n\n아직 완성되지 않은 궁극 잔향 조합을 먼저 밀어줍니다.", () => AddMemory(focus.Id, focus.Level + 1, true));
                choices.Add(choice);
                priorityTitles.Add(choice.Title);
            }

            if (activeMemories.Count < MaxActiveMemories && !HasMemory(V1MemoryId.HungryBlades) && ShouldOfferUltimateMemory(V1MemoryId.HungryBlades))
            {
                var kalmuriChoice = new Choice("새 기억", "굶주린 칼무리", "주변을 도는 칼날 군집이 적을 물어뜯습니다.\n\n혈반과 함께 키우면 첫 궁극 목표인 피의 칼폭풍으로 이어집니다.", () => AddMemory(V1MemoryId.HungryBlades, 1, true));
                choices.Add(kalmuriChoice);
                priorityTitles.Add(kalmuriChoice.Title);
            }
            if (activeMemories.Count < MaxActiveMemories && !HasMemory(V1MemoryId.BloodReflection) && ShouldOfferUltimateMemory(V1MemoryId.BloodReflection))
            {
                var bloodChoice = new Choice("새 기억", "피의 반사", "타격한 적에게 혈반을 남깁니다.\n\n칼무리와 함께 키우면 회복/출혈 축과 피의 칼폭풍 목표가 열립니다.", () => AddMemory(V1MemoryId.BloodReflection, 1, true));
                choices.Add(bloodChoice);
                priorityTitles.Add(bloodChoice.Title);
            }
            if (activeMemories.Count < MaxActiveMemories && HasMemory(V1MemoryId.BloodReflection) && !HasMemory(V1MemoryId.StoppedSecond))
            {
                choices.Add(new Choice("새 기억", "멈춘 1초", "짧은 정지 잔향 축을 여는 세 번째 기억입니다.\n\n지금은 M2 실제 루프에서 기억 3칸을 채워 첫 망각 후보를 명확히 만드는 역할입니다.", () => AddMemory(V1MemoryId.StoppedSecond, 1, true)));
            }
            var rewardMemory = NextMissingRewardMemory();
            if (rewardMemory.HasValue)
            {
                var id = rewardMemory.Value;
                choices.Add(new Choice("새 기억", MemoryName(id), $"새 전투 축을 여는 기억입니다.\n\n망각되면 {EchoName(id)}으로 형태가 바뀌어 무기 타격에 남습니다.", () => AddMemory(id, 1, true)));
            }
            var lowest = activeMemories.OrderBy(m => m.Level).FirstOrDefault(m => m.Level < MaxMemoryLevel);
            if (lowest != null && !priorityUpgradeIds.Contains(lowest.Id))
            {
                choices.Add(new Choice("기억 강화", MemoryName(lowest.Id), $"현재 Lv.{lowest.Level} -> Lv.{lowest.Level + 1}\n\n효과의 빈도, 개수, 화면 존재감이 함께 올라갑니다.", () => AddMemory(lowest.Id, lowest.Level + 1, true)));
            }
            if (activeMemories.Count >= MaxActiveMemories)
            {
                var secondLowest = activeMemories.Where(m => m.Level < MaxMemoryLevel && !priorityUpgradeIds.Contains(m.Id)).OrderBy(m => m.Level).Skip(1).FirstOrDefault();
                if (secondLowest != null)
                {
                    choices.Add(new Choice("기억 강화", MemoryName(secondLowest.Id), $"현재 Lv.{secondLowest.Level} -> Lv.{secondLowest.Level + 1}\n\n슬롯이 찼을 때는 낮은 레벨 기억을 한 번 더 밀어줍니다.", () => AddMemory(secondLowest.Id, secondLowest.Level + 1, true)));
                }
            }

            var survivalChoice = new Choice("생존", "가라앉지 않는 숨", "최대 HP +16, 즉시 회복 +28, 받는 피해 -5%.\n\n보스 파문과 중후반 압박을 버틸 여유를 만듭니다.", () => { playerMaxHp += 16f; playerHp = Mathf.Min(playerMaxHp, playerHp + 28f); WeaponStat.DamageReduction += 0.05f; Log("스탯: 생존"); });
            choices.Add(new Choice("무기 성향", "칼날 가속", "장착 무기 공격 간격 -11%, 기억 쿨다운 체감 증가.\n\n잔향 발동 기회도 늘어나 무기 리듬이 더 선명해집니다.", () => { WeaponStat.AttackSpeed += 0.11f; Log("스탯: 칼날 가속"); }));
            choices.Add(new Choice("무기 성향", "검은 물의 힘", "기본공격과 무기 기반 잔향 피해 +14%.\n\n한 번의 베기가 더 선명해집니다.", () => { WeaponStat.DamageMul += 0.14f; Log("스탯: 피해 증가"); }));
            choices.Add(new Choice("범위", "파문 확장", "무기 사거리와 주요 잔향 반경 +12%, 넉백 체감 +8%.\n\n무리 처리 안정성이 올라갑니다.", () => { WeaponStat.AreaMul += 0.12f; Log("스탯: 파문 확장"); }));
            choices.Add(survivalChoice);
            choices.Add(new Choice("성장", "기억 흡입", "경험치 흡입 반경 +16%.\n\n전투 중 성장 흐름이 끊기지 않게 합니다.", () => { WeaponStat.MagnetMul += 0.16f; Log("스탯: 기억 흡입"); }));
            choices.Add(new Choice("잔향", "잔향 증폭", "잔향 효과 +20%.\n\n망각 뒤 남은 힘이 더 확실하게 전투를 바꿉니다.", () => { WeaponStat.EchoAmp += 0.20f; Log("스탯: 잔향 증폭"); }));

            var picked = choices.OrderBy(_ => rng.Next()).Take(3).ToList();
            foreach (var title in priorityTitles)
            {
                if (picked.Any(c => c.Title == title)) continue;
                var priority = choices.FirstOrDefault(c => c.Title == title);
                if (string.IsNullOrEmpty(priority.Title)) continue;
                if (picked.Count < 3)
                {
                    picked.Add(priority);
                    continue;
                }

                var replaceIndex = picked.FindIndex(c => !priorityTitles.Contains(c.Title));
                if (replaceIndex >= 0)
                {
                    picked[replaceIndex] = priority;
                }
            }
            if (playerHp / playerMaxHp < 0.72f && !picked.Any(c => c.Title == survivalChoice.Title))
            {
                picked[picked.Count - 1] = survivalChoice;
            }
            return picked;
        }

        bool HasMemory(V1MemoryId id) => activeMemories.Any(m => m.Id == id);

        bool UltimatePairComplete(V1MemoryId[] pair) => pair.All(id => EchoLevel(id) >= MaxEchoLevel);

        bool ShouldOfferUltimateMemory(V1MemoryId id)
        {
            var pair = UltimateEchoPairs.FirstOrDefault(p => p.Contains(id));
            if (pair == null) return true;
            return UltimatePairComplete(pair) || EchoLevel(id) < MaxEchoLevel;
        }

        MemoryState UltimateFocusMemory()
        {
            return activeMemories
                .Where(m => IsIncompleteUltimatePairMember(m.Id) && EchoLevel(m.Id) < MaxEchoLevel && m.Level < MaxMemoryLevel)
                .OrderByDescending(m => m.Level)
                .ThenBy(m => UltimatePairIndex(m.Id))
                .FirstOrDefault();
        }

        bool IsIncompleteUltimatePairMember(V1MemoryId id)
        {
            var pair = UltimateEchoPairs.FirstOrDefault(p => p.Contains(id));
            return pair != null && !UltimatePairComplete(pair);
        }

        int UltimatePairIndex(V1MemoryId id)
        {
            for (int i = 0; i < UltimateEchoPairs.Length; i++)
            {
                if (UltimateEchoPairs[i].Contains(id)) return i;
            }
            return UltimateEchoPairs.Length;
        }

        V1MemoryId? NextMissingRewardMemory()
        {
            if (activeMemories.Count >= MaxActiveMemories) return null;
            var order = new[]
            {
                V1MemoryId.HungryBlades,
                V1MemoryId.BloodReflection,
                V1MemoryId.ExecutionFlash,
                V1MemoryId.HunterOath,
                V1MemoryId.ShatterWave,
                V1MemoryId.StoppedSecond,
                V1MemoryId.AshenShield,
                V1MemoryId.OblivionBrand
            };

            var start = Mathf.Abs(level + kills) % order.Length;
            for (int i = 0; i < order.Length; i++)
            {
                var id = order[(start + i) % order.Length];
                if (id == V1MemoryId.StoppedSecond && HasMemory(V1MemoryId.BloodReflection) && !HasMemory(V1MemoryId.StoppedSecond)) continue;
                if (!ShouldOfferUltimateMemory(id)) continue;
                if (!HasMemory(id)) return id;
            }
            return null;
        }

        float EarlyDamageMul()
        {
            if (elapsed <= 12f) return 0.24f;
            return Mathf.Lerp(0.24f, 1f, Mathf.Clamp01((elapsed - 12f) / (320f - 12f)));
        }

        float StatAttackIntervalMul() => 1f / (1f + WeaponStat.AttackSpeed);

        string M2LoopText()
        {
            if (AnyUltimateReady) return $"M2: 궁극 준비 완료 - {UltimateReadyName()} 보상 확인";
            if (lastForgotten.HasValue) return $"M2: {EchoName(lastForgotten.Value)} 남음 - 잔향을 쌓아 궁극 조합 준비";
            if (activeMemories.Count >= MaxActiveMemories) return $"런: 다음 망각 후보 {ForgetCandidateText()} - 문지기까지 {Mathf.CeilToInt(Mathf.Max(0f, bossTimer))}초";
            return "런: 기억 3개 확보와 최고 레벨 강화가 목표";
        }

        string UltimateReadyName()
        {
            if (BloodBladeStormReady) return "피의 칼폭풍";
            if (FractureExecutionReady) return "파쇄 처형";
            if (StasisHuntReady) return "정지 추적";
            if (AshenOblivionReady) return "잿빛 망각";
            return "없음";
        }

        string UltimateGoalText()
        {
            if (echoOnlyDebugMode) return "Echo Only Debug: 궁극 억제 중";
            if (AnyUltimateReady) return $"궁극: {UltimateReadyName()} 활성";
            return $"궁극 준비: 칼무리 {EchoLevel(V1MemoryId.HungryBlades)}/5 + 혈반 {EchoLevel(V1MemoryId.BloodReflection)}/5";
        }

        string PhaseName()
        {
            var pressure = CurrentPressure();
            if (pressure.Progress < 0.24f) return "숨 고르기";
            if (pressure.Progress < 0.70f) return "압박 상승";
            if (pressure.FirstCycle && pressure.Progress >= 0.94f) return "문지기 호흡";
            return "망각 전조";
        }

        string ForgetCandidateText()
        {
            if (activeMemories.Count == 0) return "없음";
            var c = activeMemories.OrderByDescending(m => m.Level).First();
            return $"{MemoryName(c.Id)} +{c.Level}";
        }

        string EchoText()
        {
            var parts = echoLevels.Where(kv => kv.Value > 0).Select(kv => $"{EchoName(kv.Key)} +{kv.Value}{(kv.Value >= MaxEchoLevel ? " 각성" : "")}");
            var text = string.Join(" / ", parts);
            return string.IsNullOrEmpty(text) ? "없음" : text;
        }

        static string UltimatePatternText(WeaponRuntimeSpec weapon) => weapon.UltimatePattern == V1UltimatePattern.FewHeavy ? "대검 강타형" : "쌍검 연쇄형";

        void SpawnWeaponHitVfx(WeaponRuntimeSpec weapon, List<WeaponHit> hits, Vector2 forward)
        {
            if (hits.Count == 0) return;
            PlaySfx(weapon.Id == V1WeaponId.Greatsword ? "slash_great" : "slash_dual");
            var f = forward.sqrMagnitude > 0.001f ? forward.normalized : lastAim.normalized;
            var baseAngle = Mathf.Atan2(f.y, f.x) * Mathf.Rad2Deg;
            var hitCenter = (Vector3)hits.Aggregate(Vector2.zero, (sum, hit) => sum + (Vector2)hit.Enemy.transform.position) / hits.Count;
            SpawnPhantomWeaponAttack(weapon, hits, f, baseAngle, hitCenter);
            if (weapon.Id == V1WeaponId.DualBlades)
            {
                SpawnDualBladeGuaranteedSlashes(hits, f, baseAngle, hitCenter, DenseDualBladeVfxThrottle(weapon));
            }
            var entries = weapon.VfxProfile != null ? weapon.VfxProfile.weaponHitSlashes : Array.Empty<SlashVfxEntry>();
            var hasProfileSlash = entries != null && entries.Length > 0;
            if (weapon.Id == V1WeaponId.Greatsword)
            {
                SpawnGreatswordGuaranteedSlash(hits[0].Enemy.transform.position, hitCenter, f, hasProfileSlash);
            }

            if (entries == null || entries.Length == 0) return;

            for (int i = 0; i < hits.Count; i++)
            {
                if (!ShouldSpawnWeaponSlashForHit(weapon, i)) continue;
                var primary = i == 0;
                foreach (var entry in entries)
                {
                    if (entry == null || !entry.Matches(primary)) continue;
                    var slashForward = f;
                    var slashBaseAngle = baseAngle;
                    var slashTarget = hits[i].Enemy.transform.position;
                    var slashCenter = hitCenter;
                    if (weapon.Id == V1WeaponId.Greatsword)
                    {
                        var targetSwing = GreatswordSwingForTarget(hits[i].Enemy.transform.position, hitCenter, f, i);
                        var centerSwing = GreatswordSwingForTarget(hitCenter, hitCenter, f, 0);
                        slashForward = targetSwing.EndDirection;
                        slashBaseAngle = GreatswordSlashVfxBaseAngle(slashForward);
                        slashTarget = GreatswordSlashAnchorForTip(entry, GreatswordSlashTipForEntry(entry, targetSwing), slashForward);
                        slashCenter = GreatswordSlashAnchorForTip(entry, GreatswordSlashTipForEntry(entry, centerSwing), centerSwing.EndDirection);
                    }
                    var slashDelay = SlashDelayForEntry(weapon.Id, entry);
                    var minLifetime = weapon.Id == V1WeaponId.Greatsword ? GreatswordSlashMinLifetime : DualBladeSlashMinLifetime;
                    StartCoroutine(SpawnSlashEntryDelayed(entry, slashTarget, slashCenter, slashForward, slashBaseAngle, primary, i, slashDelay, WeaponSlashLifetimeMultiplier, minLifetime));
                }
            }
        }

        void SpawnDualBladeGuaranteedSlashes(List<WeaponHit> hits, Vector2 forward, float baseAngle, Vector3 hitCenter, bool dense)
        {
            if (hits == null || hits.Count == 0) return;
            var f = forward.sqrMagnitude > 0.001f ? forward.normalized : lastAim.normalized;
            if (f.sqrMagnitude < 0.001f) f = Vector2.right;
            var side = new Vector2(-f.y, f.x).normalized;
            var line = MakeBoxSprite("DualBladeGuaranteedCut", Color.white, 9, 142);
            var count = dense ? 1 : Mathf.Min(4, hits.Count + 1);
            var lifetime = dense ? 0.12f : 0.22f;
            for (int i = 0; i < count; i++)
            {
                var primary = i == 0;
                var offset = side * ((i - (count - 1) * 0.5f) * (dense ? 0.10f : 0.15f));
                var pos = hitCenter + (Vector3)(f * (primary ? 0.08f : 0.16f) + offset);
                var angle = baseAngle - 90f + (primary ? 38f : -42f) + i * (dense ? 7f : 11f);
                var color = primary
                    ? new Color(0.86f, 1f, 1f, dense ? 0.76f : 0.88f)
                    : new Color(0.58f, 0.92f, 1f, dense ? 0.46f : 0.60f);
                SpawnTransientSpriteScaled("DualBladeGuaranteedCut", line, pos, Quaternion.Euler(0f, 0f, angle), new Vector3(dense ? 0.018f : 0.022f, dense ? 0.58f : 0.90f, 1f), color, lifetime);
            }
            if (!dense)
            {
                SpawnTransientSprite("DualBladeHitSpark", MakeImpactDiamondSprite("DualBladeHitSpark", Color.white), hitCenter + (Vector3)(f * 0.10f), Quaternion.Euler(0f, 0f, baseAngle + 45f), 0.24f, new Color(0.94f, 1f, 1f, 0.72f), 0.16f);
            }
        }

        float SlashDelayForEntry(V1WeaponId weaponId, SlashVfxEntry entry)
        {
            if (weaponId == V1WeaponId.Greatsword) return GreatswordSlashDelay;
            var id = entry != null ? entry.id ?? "" : "";
            if (id.Contains("Crescent_B", StringComparison.OrdinalIgnoreCase))
            {
                return DualBladeSlashDelay + DualBladeSecondSlashExtraDelay;
            }
            if (id.Contains("CutFlash", StringComparison.OrdinalIgnoreCase))
            {
                return DualBladeSlashDelay + DualBladeSecondSlashExtraDelay * 0.55f;
            }
            return DualBladeSlashDelay;
        }

        void SpawnPhantomWeaponAttack(WeaponRuntimeSpec weapon, List<WeaponHit> hits, Vector2 forward, float baseAngle, Vector3 hitCenter)
        {
            var side = new Vector2(-forward.y, forward.x).normalized;
            if (weapon.Id == V1WeaponId.Greatsword)
            {
                var primary = hits[0].Enemy.transform.position;
                var swing = GreatswordSwingForTarget(primary, hitCenter, forward, 0);
                var scale = ScaleSpriteToWorldHeight(greatswordWeaponSprite, GreatswordPhantomHeight);
                var centerDistance = GreatswordPhantomHeight * GreatswordHandleToCenterRatio;
                SpawnPivotSweepingTransientSprite("GreatswordPhantomStrike", greatswordWeaponSprite, swing.HandlePivot, swing.StartBladeAngle, swing.EndBladeAngle, centerDistance, scale, scale * 1.04f, new Color(0.90f, 0.98f, 1f, 0.90f), GreatswordPhantomLifetime, GreatswordPhantomSweepDuration);
                SpawnPivotSweepingTransientSprite("GreatswordPhantomAfterimage", greatswordWeaponSprite, swing.HandlePivot - (Vector3)(forward * 0.06f), swing.StartBladeAngle - 4f, swing.EndBladeAngle - 4f, centerDistance, scale * 1.10f, scale * 1.14f, new Color(0.55f, 0.82f, 1f, 0.28f), GreatswordPhantomLifetime + 0.08f, GreatswordPhantomAfterimageSweepDuration);
                return;
            }

            var target = hits[0].Enemy.transform.position;
            var lead = leftBladeLead ? -1f : 1f;
            var leftPos = target + (Vector3)(side * 0.18f * lead - forward * 0.04f);
            var rightPos = target + (Vector3)(side * -0.18f * lead + forward * 0.12f);
            var leftScale = ScaleSpriteToWorldHeight(dualLeftWeaponSprite, DualBladePhantomHeight);
            var rightScale = ScaleSpriteToWorldHeight(dualRightWeaponSprite, DualBladePhantomHeight);
            SpawnSweepingTransientSprite("DualBladePhantomLeft", dualLeftWeaponSprite, leftPos - (Vector3)(side * 0.12f * lead), leftPos + (Vector3)(side * 0.12f * lead) + (Vector3)(forward * 0.06f), baseAngle + 30f * lead, baseAngle + 76f * lead, leftScale, leftScale * 1.04f, new Color(0.80f, 0.98f, 1f, 0.92f), DualBladePhantomLifetime, 0.13f);
            SpawnSweepingTransientSprite("DualBladePhantomRight", dualRightWeaponSprite, rightPos + (Vector3)(side * 0.12f * lead), rightPos - (Vector3)(side * 0.12f * lead) + (Vector3)(forward * 0.08f), baseAngle - 32f * lead, baseAngle - 78f * lead, rightScale, rightScale * 1.04f, new Color(0.95f, 1f, 1f, 0.88f), DualBladePhantomLifetime + 0.02f, 0.14f);
        }

        void SpawnGreatswordGuaranteedSlash(Vector3 primaryTarget, Vector3 hitCenter, Vector2 forward, bool profileLayerPresent)
        {
            var f = forward.sqrMagnitude > 0.001f ? forward.normalized : lastAim.normalized;
            var swing = GreatswordSwingForTarget(primaryTarget, hitCenter, f, 0);
            var slashForward = swing.EndDirection;
            var tip = Vector3.Lerp(swing.TipStart, swing.TipEnd, 0.70f);
            var baseAngle = GreatswordSlashVfxBaseAngle(slashForward);
            var arc = LoadSprite(GreatswordCleaveArcPath) ?? MakeWideCrescentSprite("GreatswordGuaranteedCleave", Color.white);
            if (profileLayerPresent)
            {
                SpawnTransientSprite("GreatswordTipAfterglow", arc, tip + (Vector3)(f * 0.04f), Quaternion.Euler(0f, 0f, baseAngle), 0.24f, new Color(0.72f, 0.94f, 1f, 0.26f), GreatswordSlashMinLifetime * 0.62f);
                SpawnEchoWoundSlash("GreatswordTipCutLine", tip - (Vector3)(f * 0.08f), slashForward, new Color(0.92f, 1f, 1f, 0.38f), 1.04f, 0.20f);
                return;
            }

            SpawnTransientSprite("GreatswordGuaranteedCleave_A", arc, tip, Quaternion.Euler(0f, 0f, baseAngle), 0.38f, new Color(0.82f, 0.96f, 1f, 0.58f), GreatswordSlashMinLifetime);
            SpawnEchoWoundSlash("GreatswordGuaranteedCutLine", tip - (Vector3)(f * 0.12f), slashForward, new Color(0.92f, 1f, 1f, 0.58f), 1.24f, 0.26f);
        }

        GreatswordSwingPose GreatswordSwingForTarget(Vector3 targetPosition, Vector3 hitCenter, Vector2 forward, int hitIndex)
        {
            var f = forward.sqrMagnitude > 0.001f ? forward.normalized : (lastAim.sqrMagnitude > 0.001f ? lastAim.normalized : Vector2.right);
            var side = new Vector2(-f.y, f.x).normalized;
            var sideOffset = hitIndex == 0 ? 0f : (hitIndex % 2 == 0 ? -0.16f : 0.16f);
            var tipMid = Vector3.Lerp(targetPosition, hitCenter, 0.35f) + (Vector3)(f * GreatswordTipForwardOffset + side * sideOffset);
            var midDirection = player != null ? (Vector2)(tipMid - player.position) : f;
            if (midDirection.sqrMagnitude < 0.001f) midDirection = f;
            midDirection.Normalize();
            var swingSign = leftBladeLead ? -1f : 1f;
            var startDirection = RotateVector(midDirection, -GreatswordSwingHalfAngle * swingSign);
            var endDirection = RotateVector(midDirection, GreatswordSwingHalfAngle * swingSign);
            var handleToTip = GreatswordPhantomHeight * GreatswordHandleToTipRatio;
            var handlePivot = tipMid - (Vector3)(midDirection * handleToTip);
            var startBladeAngle = Mathf.Atan2(startDirection.y, startDirection.x) * Mathf.Rad2Deg;
            var endBladeAngle = Mathf.Atan2(endDirection.y, endDirection.x) * Mathf.Rad2Deg;
            return new GreatswordSwingPose(handlePivot, startDirection, endDirection, startBladeAngle, endBladeAngle, handlePivot + (Vector3)(startDirection * handleToTip), handlePivot + (Vector3)(endDirection * handleToTip));
        }

        Vector3 GreatswordSlashTipForEntry(SlashVfxEntry entry, GreatswordSwingPose swing)
        {
            var id = entry != null ? entry.id ?? "" : "";
            if (id.Contains("Aoe", StringComparison.OrdinalIgnoreCase))
            {
                return Vector3.Lerp(swing.TipStart, swing.TipEnd, 0.58f);
            }
            if (id.Contains("Primary", StringComparison.OrdinalIgnoreCase))
            {
                return Vector3.Lerp(swing.TipStart, swing.TipEnd, 0.78f);
            }
            if (id.Contains("Assist", StringComparison.OrdinalIgnoreCase))
            {
                return Vector3.Lerp(swing.TipStart, swing.TipEnd, 0.72f);
            }
            return swing.TipEnd;
        }

        Vector3 GreatswordSlashAnchorForTip(SlashVfxEntry entry, Vector3 desiredTipPosition, Vector2 forward)
        {
            var f = forward.sqrMagnitude > 0.001f ? forward.normalized : lastAim.normalized;
            var side = new Vector2(-f.y, f.x).normalized;
            var sideSign = entry != null && entry.mirrorSideByLeadHand ? (leftBladeLead ? -1f : 1f) : 1f;
            var localOffset = entry != null ? entry.localOffset : Vector2.zero;
            return desiredTipPosition - (Vector3)(side * localOffset.x * sideSign + f * localOffset.y);
        }

        float GreatswordSlashVfxBaseAngle(Vector2 bladeDirection)
        {
            var dir = bladeDirection.sqrMagnitude > 0.001f ? bladeDirection.normalized : Vector2.right;
            return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + GreatswordSlashFacingCorrection;
        }

        static Vector2 RotateVector(Vector2 value, float degrees)
        {
            var radians = degrees * Mathf.Deg2Rad;
            var sin = Mathf.Sin(radians);
            var cos = Mathf.Cos(radians);
            return new Vector2(value.x * cos - value.y * sin, value.x * sin + value.y * cos).normalized;
        }

        float SpriteRotationForTipDirection(Vector2 tipDirection)
        {
            var dir = tipDirection.sqrMagnitude > 0.001f ? tipDirection.normalized : Vector2.right;
            return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        }

        IEnumerator SpawnSlashEntryDelayed(SlashVfxEntry entry, Vector3 targetPosition, Vector3 hitCenter, Vector2 forward, float baseAngle, bool primary, int hitIndex, float delay, float lifetimeMultiplier, float minLifetime)
        {
            if (delay > 0f) yield return new WaitForSeconds(delay);
            SpawnSlashEntry(entry, targetPosition, hitCenter, forward, baseAngle, primary, hitIndex, lifetimeMultiplier, minLifetime);
        }

        void SpawnSlashEntry(SlashVfxEntry entry, Vector3 targetPosition, Vector3 hitCenter, Vector2 forward, float baseAngle, bool primary, int hitIndex, float lifetimeMultiplier = 1f, float minLifetime = 0f)
        {
            var side = new Vector2(-forward.y, forward.x).normalized;
            var sideSign = entry.mirrorSideByLeadHand ? (leftBladeLead ? -1f : 1f) : 1f;
            var rotationSign = entry.mirrorRotationByLeadHand ? (leftBladeLead ? -1f : 1f) : 1f;
            var anchor = entry.anchor switch
            {
                SlashAnchor.HitCenter => hitCenter,
                SlashAnchor.CleaveTarget => targetPosition,
                SlashAnchor.PrimaryTarget => targetPosition,
                _ => targetPosition
            };
            var position = anchor + (Vector3)(side * entry.localOffset.x * sideSign + forward * entry.localOffset.y);
            var sprite = MakeSlashSprite(entry, out var spriteScaleFactor);
            var rotation = entry.spriteShape == SlashSpriteShape.ImpactDiamond || entry.spriteShape == SlashSpriteShape.Circle
                ? Quaternion.identity
                : Quaternion.Euler(0f, 0f, baseAngle + entry.rotationOffsetDegrees * rotationSign);
            var lifetime = Mathf.Max(entry.lifetime * lifetimeMultiplier, minLifetime);
            SpawnTransientSprite(entry.id, sprite, position, rotation, entry.scale * spriteScaleFactor, entry.color, lifetime);
        }

        Sprite MakeSlashSprite(SlashVfxEntry entry, out float spriteScaleFactor)
        {
            spriteScaleFactor = 1f;
            var promptSprite = PromptSlashSprite(entry, out var promptScaleFactor);
            if (promptSprite != null)
            {
                spriteScaleFactor = promptScaleFactor;
                return promptSprite;
            }

            return entry.spriteShape switch
            {
                SlashSpriteShape.WideCrescent => MakeWideCrescentSprite(entry.id, Color.white),
                SlashSpriteShape.ImpactDiamond => MakeImpactDiamondSprite(entry.id, Color.white),
                SlashSpriteShape.Circle => null,
                _ => MakeCrescentSlashSprite(entry.id, Color.white, entry.flip)
            };
        }

        Sprite PromptSlashSprite(SlashVfxEntry entry, out float spriteScaleFactor)
        {
            spriteScaleFactor = 1f;
            if (entry == null) return null;

            var id = entry.id ?? "";
            switch (entry.spriteShape)
            {
                case SlashSpriteShape.WideCrescent:
                    spriteScaleFactor = id.Contains("Greatsword", StringComparison.OrdinalIgnoreCase)
                        ? 0.175f
                        : 0.150f;
                    return LoadSprite(GreatswordCleaveArcPath);
                case SlashSpriteShape.ImpactDiamond:
                    spriteScaleFactor = id.Contains("Greatsword", StringComparison.OrdinalIgnoreCase) || id.Contains("Blood", StringComparison.OrdinalIgnoreCase)
                        ? 0.055f
                        : 0.048f;
                    return LoadSprite(spriteScaleFactor > 0.05f ? HitSparkRedPath : HitSparkCyanPath);
                case SlashSpriteShape.Crescent:
                    if (id.Contains("Kalmuri", StringComparison.OrdinalIgnoreCase))
                    {
                        spriteScaleFactor = 0.13f;
                        return LoadSprite("Assets/_dev/Art/Sprites/Echoes/Kalmuri/spr_kalmuri_echo_slash_01.png");
                    }
                    spriteScaleFactor = 0.192f;
                    return LoadSprite(id.Contains("_B", StringComparison.OrdinalIgnoreCase) || id.Contains("Assist", StringComparison.OrdinalIgnoreCase)
                        ? DualBladeSwingArcBPath
                        : DualBladeSwingArcAPath);
                default:
                    return null;
            }
        }

        void SpawnHitSpark(Vector3 pos, Vector2 dir, bool weaponHit)
        {
            if (!weaponHit) return;
            var feedback = CurrentWeaponSpec().VfxProfile;
            if (feedback == null || !feedback.spawnHitSpark || feedback.hitSpark == null) return;
            var forward = dir.sqrMagnitude > 0.01f ? dir.normalized : lastAim.normalized;
            var angle = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
            SpawnSlashEntry(feedback.hitSpark, pos, pos, forward, angle, true, 0);
        }

        IEnumerator SpawnWeaponImpactFeedbackDelayed(Vector3 pos, Vector2 dir, float delay)
        {
            if (delay > 0f) yield return new WaitForSeconds(delay);
            SpawnHitSpark(pos, dir, true);
            SpawnWeaponHitConfirm(pos, dir);
        }

        IEnumerator DealDamageDelayed(V1Enemy enemy, float amount, string source, bool weaponHit, Vector2 hitDir, float knockStrength, float delay)
        {
            if (delay > 0f) yield return new WaitForSeconds(delay);
            var impactPos = enemy != null ? enemy.transform.position : Vector3.zero;
            DealDamage(enemy, amount, source, weaponHit, hitDir, knockStrength);
            if (!weaponHit && enemy != null)
            {
                PlaySfx("kalmuri_pierce", 0.40f, 0.025f);
                var forward = hitDir.sqrMagnitude > 0.01f ? hitDir.normalized : Vector2.up;
                var angle = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
                SpawnTransientSprite("KalmuriBladePierceSpark", MakeImpactDiamondSprite("KalmuriBladePierceSpark", Color.white), impactPos + (Vector3)(forward * 0.06f), Quaternion.Euler(0f, 0f, angle), 0.16f, new Color(0.82f, 1f, 1f, 0.70f), 0.10f);
            }
        }

        void SpawnWeaponHitConfirm(Vector3 pos, Vector2 dir)
        {
            var weapon = CurrentWeaponSpec();
            var heavy = weapon.Id == V1WeaponId.Greatsword;
            var forward = dir.sqrMagnitude > 0.01f ? dir.normalized : lastAim.normalized;
            var angle = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
            var ringColor = heavy ? new Color(0.92f, 0.98f, 1f, 0.36f) : new Color(0.55f, 0.96f, 1f, 0.30f);
            var diamondColor = heavy ? new Color(1f, 0.93f, 0.72f, 0.72f) : new Color(0.96f, 1f, 1f, 0.58f);
            SpawnTransientSprite("WeaponHitConfirmRing", MakeRingSprite("WeaponHitConfirmRing", Color.white, heavy ? 112 : 88), pos, Quaternion.identity, heavy ? 0.38f : 0.26f, ringColor, heavy ? 0.16f : 0.11f);
            SpawnTransientSprite("WeaponHitConfirmCore", MakeImpactDiamondSprite("WeaponHitConfirmCore", Color.white), pos + (Vector3)(forward * 0.05f), Quaternion.Euler(0f, 0f, angle), heavy ? 0.24f : 0.16f, diamondColor, heavy ? 0.12f : 0.08f);
        }

        void DrawBar(Rect rect, float value, Color fill, Color background)
        {
            DrawFilledRect(rect, background);
            DrawFilledRect(new Rect(rect.x, rect.y, rect.width * Mathf.Clamp01(value), rect.height), fill);
        }

        void DrawFilledRect(Rect rect, Color color)
        {
            var previous = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = previous;
        }

        void SpawnFloatingText(Vector3 pos, string text, Color color)
        {
            var go = RentPooled(floatingTextPool, "FloatText");
            go.transform.position = pos + Vector3.up * 0.35f;
            var floating = go.GetComponent<V1FloatingText>();
            if (floating == null) floating = go.AddComponent<V1FloatingText>();
            floating.Configure(this, text, color);
        }

        void SpawnDamageNumber(Vector3 pos, float amount, bool weaponHit, WeaponVfxProfile feedback)
        {
            var go = RentPooled(damageNumberPool, "DamageNumber");
            var jitter = UnityEngine.Random.insideUnitCircle * 0.16f;
            go.transform.position = pos + new Vector3(jitter.x, 0.34f + jitter.y, 0f);
            var color = feedback != null
                ? weaponHit ? feedback.weaponDamageNumberColor : feedback.nonWeaponDamageNumberColor
                : weaponHit ? new Color(1f, 0.96f, 0.72f) : new Color(0.86f, 0.98f, 1f);
            var lifetime = feedback != null
                ? weaponHit ? feedback.weaponDamageNumberLifetime : feedback.nonWeaponDamageNumberLifetime
                : weaponHit ? 0.78f : 0.62f;
            var damageNumber = go.GetComponent<V1DamageNumber>();
            if (damageNumber == null) damageNumber = go.AddComponent<V1DamageNumber>();
            damageNumber.Configure(this, Mathf.CeilToInt(amount).ToString(), color, lifetime);
        }

        void SpawnBloodThread(Vector3 from, float healAmount, int level)
        {
            if (player == null) return;
            var to = player.position + Vector3.up * 0.18f;
            var delta = to - from;
            var length = Mathf.Max(0.18f, delta.magnitude);
            var mid = from + delta * 0.5f;
            var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg - 90f;
            var color = new Color(1f, 0.10f, 0.16f, Mathf.Clamp01(0.42f + level * 0.08f));
            var sprite = MakeBoxSprite("BloodThread", Color.white, 7, 128);
            SpawnTransientSpriteScaled("BloodHealThread", sprite, mid, Quaternion.Euler(0f, 0f, angle), new Vector3(0.030f + level * 0.004f, length * 0.78f, 1f), color, 0.18f);
            if (healAmount >= 1.1f)
            {
                SpawnTransientSprite("BloodHealPulse", MakeRingSprite("BloodHealPulse", Color.white, 96), player.position + Vector3.up * 0.2f, Quaternion.identity, 0.40f, new Color(1f, 0.14f, 0.20f, 0.42f), 0.20f);
            }
        }

        void SpawnPromptSprite(string name, Sprite promptSprite, Func<Sprite> fallbackSprite, Vector3 position, Quaternion rotation, float targetWorldWidth, float fallbackScale, Color color, float lifetime)
        {
            var sprite = promptSprite;
            var scale = fallbackScale;
            if (sprite != null)
            {
                scale = ScaleSpriteToWorldWidth(sprite, targetWorldWidth);
            }
            else if (fallbackSprite != null)
            {
                sprite = fallbackSprite();
            }

            SpawnTransientSprite(name, sprite, position, rotation, scale, color, lifetime);
        }

        float ScaleSpriteToWorldWidth(Sprite sprite, float targetWorldWidth)
        {
            if (sprite == null || sprite.bounds.size.x <= 0.001f) return 1f;
            return targetWorldWidth / sprite.bounds.size.x;
        }

        float ScaleSpriteToWorldHeight(Sprite sprite, float targetWorldHeight)
        {
            if (sprite == null || sprite.bounds.size.y <= 0.001f) return 1f;
            return targetWorldHeight / sprite.bounds.size.y;
        }

        Sprite MemoryVfxSprite(V1MemoryId id) => id switch
        {
            V1MemoryId.ExecutionFlash => LoadSprite(MemoryExecutionPath),
            V1MemoryId.HunterOath => LoadSprite(MemoryHunterPath),
            V1MemoryId.ShatterWave => LoadSprite(MemoryShatterPath),
            V1MemoryId.StoppedSecond => LoadSprite(MemoryStoppedPath),
            V1MemoryId.AshenShield => LoadSprite(MemoryAshenPath),
            V1MemoryId.OblivionBrand => LoadSprite(MemoryBrandPath),
            _ => null
        };

        Sprite EchoVfxSprite(V1MemoryId id) => id switch
        {
            V1MemoryId.ExecutionFlash => LoadSprite(EchoExecutionPath),
            V1MemoryId.HunterOath => LoadSprite(EchoHunterPath),
            V1MemoryId.ShatterWave => LoadSprite(EchoShatterPath),
            V1MemoryId.StoppedSecond => LoadSprite(EchoStoppedPath),
            V1MemoryId.AshenShield => LoadSprite(EchoAshenPath),
            V1MemoryId.OblivionBrand => LoadSprite(EchoBrandPath),
            _ => null
        };

        Sprite EchoTransformSprite(V1MemoryId id) => id switch
        {
            V1MemoryId.HungryBlades => KalmuriBladeSprite(),
            V1MemoryId.BloodReflection => LoadSprite("Assets/_dev/Art/Sprites/Echoes/Blood/spr_blood_bloom_01.png"),
            _ => EchoVfxSprite(id)
        };

        GameObject SpawnTransientSprite(string name, Sprite sprite, Vector3 position, Quaternion rotation, float scale, Color color, float lifetime)
        {
            return SpawnTransientSpriteScaled(name, sprite, position, rotation, Vector3.one * scale, color, lifetime);
        }

        GameObject SpawnSweepingTransientSprite(string name, Sprite sprite, Vector3 startPosition, Vector3 endPosition, float startAngle, float endAngle, float startScale, float endScale, Color color, float lifetime, float sweepDuration)
        {
            var go = SpawnTransientSprite(name, sprite, startPosition, Quaternion.Euler(0f, 0f, startAngle), startScale, color, lifetime);
            var sweep = go.GetComponent<V1WeaponPhantomSweep>();
            if (sweep == null) sweep = go.AddComponent<V1WeaponPhantomSweep>();
            sweep.Configure(startPosition, endPosition, startAngle, endAngle, Vector3.one * (startScale * CombatVfxVisibilityScale), Vector3.one * (endScale * CombatVfxVisibilityScale), sweepDuration);
            return go;
        }

        GameObject SpawnPivotSweepingTransientSprite(string name, Sprite sprite, Vector3 handlePivot, float startBladeAngle, float endBladeAngle, float centerDistance, float startScale, float endScale, Color color, float lifetime, float sweepDuration)
        {
            var startDirection = Quaternion.Euler(0f, 0f, startBladeAngle) * Vector3.right;
            var startPosition = handlePivot + startDirection * centerDistance;
            var go = SpawnTransientSprite(name, sprite, startPosition, Quaternion.Euler(0f, 0f, startBladeAngle - 90f), startScale, color, lifetime);
            var sweep = go.GetComponent<V1WeaponPhantomSweep>();
            if (sweep == null) sweep = go.AddComponent<V1WeaponPhantomSweep>();
            sweep.ConfigurePivot(handlePivot, startBladeAngle, endBladeAngle, centerDistance, Vector3.one * (startScale * CombatVfxVisibilityScale), Vector3.one * (endScale * CombatVfxVisibilityScale), sweepDuration);
            return go;
        }

        void SpawnKalmuriBlade(string name, Vector3 position, float angle, float scale, Color color, float lifetime)
        {
            SpawnTransientSprite(name, KalmuriBladeSprite(), position, Quaternion.Euler(0f, 0f, angle), scale, color, lifetime);
        }

        void SpawnKalmuriDiveBlade(string name, Vector3 start, Vector3 end, float scale, Color color, float lifetime, float sweepDuration)
        {
            var delta = end - start;
            var distance = delta.magnitude;
            if (distance < 0.04f) return;

            var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
            SpawnEchoLink($"{name}_Trail", start, end, new Color(color.r, color.g, color.b, color.a * 0.34f), Mathf.Min(lifetime, sweepDuration + 0.08f), 0.018f + scale * 0.030f);
            SpawnSweepingTransientSprite(
                name,
                KalmuriBladeSprite(),
                start,
                end,
                angle + 58f,
                angle + 74f,
                scale,
                scale * 1.16f,
                color,
                lifetime,
                sweepDuration);
        }

        void SpawnOrbitingKalmuriBlade(string name, Vector3 center, float radius, float startAngle, float endAngle, float scale, Color color, float lifetime)
        {
            var startDir = Quaternion.Euler(0f, 0f, startAngle) * Vector3.right;
            var endDir = Quaternion.Euler(0f, 0f, endAngle) * Vector3.right;
            var start = center + startDir * radius;
            var end = center + endDir * radius;
            SpawnSweepingTransientSprite(name, KalmuriBladeSprite(), start, end, startAngle + 60f, endAngle + 60f, scale, scale * 1.08f, color, lifetime, lifetime * 0.76f);
        }

        void SpawnExecutionFlashBurst(Vector3 center, float scale, float lifetime)
        {
            var line = MakeBoxSprite("ExecutionFlashCrack", Color.white, 8, 128);
            var color = new Color(1f, 0.94f, 0.58f, 0.78f);
            var hot = new Color(1f, 1f, 0.84f, 0.92f);
            SpawnTransientSpriteScaled("ExecutionFlashCrack_V", line, center, Quaternion.identity, new Vector3(0.038f * scale, 1.12f * scale, 1f), color, lifetime);
            SpawnTransientSpriteScaled("ExecutionFlashCrack_H", line, center, Quaternion.Euler(0f, 0f, 90f), new Vector3(0.038f * scale, 1.12f * scale, 1f), color, lifetime);
            SpawnTransientSpriteScaled("ExecutionFlashCrack_D1", line, center, Quaternion.Euler(0f, 0f, 45f), new Vector3(0.026f * scale, 0.82f * scale, 1f), new Color(1f, 0.88f, 0.48f, 0.58f), lifetime * 0.86f);
            SpawnTransientSpriteScaled("ExecutionFlashCrack_D2", line, center, Quaternion.Euler(0f, 0f, 135f), new Vector3(0.026f * scale, 0.82f * scale, 1f), new Color(1f, 0.88f, 0.48f, 0.58f), lifetime * 0.86f);
            SpawnTransientSprite("ExecutionFlashCore", MakeImpactDiamondSprite("ExecutionFlashCore", Color.white), center, Quaternion.Euler(0f, 0f, elapsed * 160f), 0.34f * scale, hot, lifetime * 0.72f);
        }

        void SpawnStoppedSecondField(Vector3 center, float radius, Color color, float lifetime, bool strong)
        {
            var fieldAlpha = strong ? Mathf.Min(0.62f, color.a) : Mathf.Min(0.46f, color.a);
            var ringAlpha = strong ? Mathf.Min(0.86f, color.a + 0.18f) : Mathf.Min(0.70f, color.a + 0.12f);
            SpawnPromptSprite("StoppedSecondClockFace", MemoryVfxSprite(V1MemoryId.StoppedSecond), () => MakeRingSprite("StoppedSecondClockFace", Color.white, 180), center, Quaternion.identity, radius * 2.56f, radius * 1.08f, new Color(color.r, color.g, color.b, fieldAlpha), lifetime);
            SpawnTransientSprite("StoppedSecondClockPulse", MakeRingSprite("StoppedSecondClockPulse", Color.white, 180), center, Quaternion.Euler(0f, 0f, elapsed * -70f), radius * 1.04f, new Color(1f, 0.88f, 0.36f, ringAlpha * 0.68f), lifetime * 0.92f);
            SpawnTransientSprite("StoppedSecondClockOuter", MakeRingSprite("StoppedSecondClockOuter", Color.white, 180), center, Quaternion.identity, radius * 0.96f, new Color(color.r, color.g, color.b, ringAlpha), lifetime);
            SpawnTransientSprite("StoppedSecondClockInner", MakeRingSprite("StoppedSecondClockInner", Color.white, 144), center, Quaternion.identity, radius * 0.56f, new Color(color.r, color.g, color.b, fieldAlpha * 0.82f), lifetime * 0.92f);

            var tick = MakeBoxSprite("StoppedSecondTick", Color.white, 9, 62);
            for (int i = 0; i < 12; i++)
            {
                var angle = i * 30f;
                var dir = Quaternion.Euler(0f, 0f, angle) * Vector3.up;
                var pos = center + dir * radius * 0.80f;
                var tickScale = strong && i % 3 == 0
                    ? new Vector3(0.042f, 0.42f, 1f)
                    : new Vector3(0.030f, 0.28f, 1f);
                SpawnTransientSpriteScaled("StoppedSecondClockTick", tick, pos, Quaternion.Euler(0f, 0f, angle), tickScale, new Color(color.r, color.g, color.b, ringAlpha), lifetime);
            }

            SpawnClockHands(center, radius, new Color(color.r, color.g, color.b, Mathf.Min(0.86f, color.a + 0.18f)), lifetime * 0.88f);
        }

        static Color TimeStopGold(bool strong)
        {
            return strong ? new Color(1f, 0.76f, 0.20f, 0.82f) : new Color(1f, 0.70f, 0.18f, 0.66f);
        }

        void SpawnClockHands(Vector3 center, float radius, Color color, float lifetime)
        {
            var longHand = MakeBoxSprite("clock_hand_long", Color.white, 8, 92);
            var shortHand = MakeBoxSprite("clock_hand_short", Color.white, 8, 64);
            var baseAngle = elapsed * 90f;
            SpawnTransientSpriteScaled("ClockHand_Long", longHand, center, Quaternion.Euler(0f, 0f, baseAngle), new Vector3(0.046f, radius * 0.64f, 1f), color, lifetime);
            SpawnTransientSpriteScaled("ClockHand_Short", shortHand, center, Quaternion.Euler(0f, 0f, baseAngle + 86f), new Vector3(0.040f, radius * 0.46f, 1f), new Color(color.r, color.g, color.b, color.a * 0.86f), lifetime);
            SpawnTransientSprite("ClockHand_Core", MakeImpactDiamondSprite("ClockHand_Core", Color.white), center, Quaternion.Euler(0f, 0f, baseAngle + 45f), 0.22f, new Color(color.r, color.g, color.b, Mathf.Min(0.92f, color.a + 0.12f)), lifetime);
        }

        Sprite KalmuriBladeSprite()
        {
            return LoadSprite(KalmuriOrbitBladePath)
                ?? MakeBladeSprite("kalmuri-fallback", new Color(0.75f, 1f, 1f), new Color(0.10f, 0.22f, 0.28f), 22, 82, false);
        }

        GameObject SpawnTransientSpriteScaled(string name, Sprite sprite, Vector3 position, Quaternion rotation, Vector3 scale, Color color, float lifetime)
        {
            debugTransientSpriteSpawnCount++;
            var go = RentPooled(transientSpritePool, name);
            go.transform.position = new Vector3(position.x, position.y, -0.05f);
            go.transform.rotation = rotation;
            go.transform.localScale = scale * CombatVfxVisibilityScale;
            var sweep = go.GetComponent<V1WeaponPhantomSweep>();
            if (sweep != null) sweep.enabled = false;
            var raidFill = go.GetComponent<V1RaidTelegraphFill>();
            if (raidFill != null) raidFill.enabled = false;
            var sr = go.GetComponent<SpriteRenderer>();
            if (sr == null) sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite ?? MakeCircleSprite(name, Color.white, 48);
            sr.color = color;
            sr.sortingOrder = 40;
            var fading = go.GetComponent<V1FadingSprite>();
            if (fading == null) fading = go.AddComponent<V1FadingSprite>();
            fading.Configure(this, lifetime);
            return go;
        }

        GameObject RentPooled(Stack<GameObject> pool, string name)
        {
            GameObject go = null;
            while (pool.Count > 0 && go == null)
            {
                go = pool.Pop();
            }
            if (go == null)
            {
                go = new GameObject(name);
            }
            go.name = name;
            go.SetActive(true);
            return go;
        }

        public void ReleaseTransientSprite(V1FadingSprite fading)
        {
            if (fading == null) return;
            var go = fading.gameObject;
            go.SetActive(false);
            transientSpritePool.Push(go);
        }

        public void ReleaseFloatingText(V1FloatingText floating)
        {
            if (floating == null) return;
            var go = floating.gameObject;
            go.SetActive(false);
            floatingTextPool.Push(go);
        }

        public void ReleaseDamageNumber(V1DamageNumber damageNumber)
        {
            if (damageNumber == null) return;
            var go = damageNumber.gameObject;
            go.SetActive(false);
            damageNumberPool.Push(go);
        }

        public void ReleaseXpOrb(V1XpOrb orb)
        {
            if (orb == null) return;
            var go = orb.gameObject;
            go.SetActive(false);
            xpOrbPool.Push(go);
        }

        Sprite EnemySprite(V1EnemyKind kind)
        {
            if (kind == V1EnemyKind.Eroder) return LoadSheetFrame(EnemyChaserSheetPath, 4, 8, 0, 0) ?? MakeCircleSprite("eroder", EnemyColor(kind), 80);
            return kind switch
            {
                V1EnemyKind.DriftingEye => LoadSheetFrame(EnemyEyeSheetPath, 4, 8, 0, 0) ?? MakeEyeSprite("drifting_eye", EnemyColor(kind), 88),
                V1EnemyKind.SplitOne => LoadSheetFrame(EnemySplitterSheetPath, 4, 8, 0, 0) ?? MakeSplitterSprite("split_one", EnemyColor(kind), 88),
                V1EnemyKind.VoidPriest => LoadSheetFrame(EnemyVoidPriestSheetPath, 4, 8, 0, 0) ?? MakePriestSprite("void_priest", EnemyColor(kind), 88),
                V1EnemyKind.Gatekeeper => GatekeeperBodySprite(Mathf.Clamp(bossSpawnIndex, 0, 3)),
                _ => MakeCircleSprite(kind.ToString(), EnemyColor(kind), 72)
            };
        }

        Sprite GatekeeperBodySprite(int rank)
        {
            var sprite = LoadSprite(BossGatekeeperRankPaths[Mathf.Clamp(rank, 0, BossGatekeeperRankPaths.Length - 1)]);
            return sprite ?? LoadSprite(BossGatekeeperPath) ?? MakeGatekeeperSprite($"gatekeeper_rank_{rank}", GatekeeperAccentColor(rank, 1f), 144, rank);
        }

        Color EnemyColor(V1EnemyKind kind) => kind switch
        {
            V1EnemyKind.Eroder => new Color(0.75f, 0.82f, 0.88f),
            V1EnemyKind.DriftingEye => new Color(0.80f, 0.45f, 1f),
            V1EnemyKind.SplitOne => new Color(1f, 0.75f, 0.35f),
            V1EnemyKind.VoidPriest => new Color(0.35f, 1f, 0.62f),
            V1EnemyKind.Gatekeeper => new Color(1f, 0.28f, 0.2f),
            _ => Color.white
        };

        static Sprite MakeEyeSprite(string name, Color color, int size)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { name = name };
            var center = new Vector2(size * 0.5f, size * 0.5f);
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                var p = new Vector2(x, y) - center;
                var eye = Mathf.Clamp01(1f - Mathf.Abs(p.x) / (size * 0.43f)) * Mathf.Clamp01(1f - Mathf.Abs(p.y) / (size * 0.22f));
                var iris = Mathf.Clamp01(1f - p.magnitude / (size * 0.16f));
                var pupil = p.magnitude < size * 0.065f ? 1f : 0f;
                var alpha = Mathf.Max(eye * 0.86f, iris);
                var c = alpha <= 0f ? Color.clear : Color.Lerp(new Color(color.r, color.g, color.b, alpha), new Color(0.95f, 1f, 1f, alpha), iris * 0.45f);
                if (pupil > 0f) c = new Color(0.04f, 0.03f, 0.05f, 1f);
                tex.SetPixel(x, y, c);
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        static Sprite MakeSplitterSprite(string name, Color color, int size)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { name = name };
            var center = new Vector2(size * 0.5f, size * 0.5f);
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                var p = new Vector2(x, y) - center;
                var diamond = Mathf.Clamp01(1f - (Mathf.Abs(p.x) + Mathf.Abs(p.y)) / (size * 0.43f));
                var crack = Mathf.Abs(p.x * 0.45f + Mathf.Sin(p.y * 0.13f) * size * 0.045f) < 1.9f && Mathf.Abs(p.y) < size * 0.34f ? 1f : 0f;
                var alpha = diamond;
                var c = alpha <= 0f ? Color.clear : new Color(color.r, color.g, color.b, Mathf.Clamp01(alpha * 1.15f));
                if (crack > 0f && diamond > 0.12f) c = new Color(0.09f, 0.06f, 0.03f, 0.95f);
                tex.SetPixel(x, y, c);
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        static Sprite MakePriestSprite(string name, Color color, int size)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { name = name };
            var center = new Vector2(size * 0.5f, size * 0.48f);
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                var p = new Vector2(x, y) - center;
                var hood = Mathf.Clamp01(1f - p.magnitude / (size * 0.36f));
                var lower = Mathf.Clamp01(1f - (Mathf.Abs(p.x) / (size * 0.28f) + Mathf.Abs(p.y - size * 0.18f) / (size * 0.34f)));
                var core = Mathf.Abs(p.x) < size * 0.045f && p.y > -size * 0.16f && p.y < size * 0.30f ? 1f : 0f;
                var alpha = Mathf.Max(hood, lower);
                var c = alpha <= 0f ? Color.clear : new Color(color.r, color.g, color.b, Mathf.Clamp01(alpha * 0.95f));
                if (core > 0f && alpha > 0.1f) c = new Color(0.88f, 1f, 0.78f, 0.90f);
                tex.SetPixel(x, y, c);
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        static Sprite MakeGatekeeperSprite(string name, Color color, int size, int rank = 0)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { name = name };
            var center = new Vector2(size * 0.5f, size * 0.5f);
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                var p = new Vector2(x, y) - center;
                var door = Mathf.Abs(p.x) < size * 0.30f && p.y > -size * 0.35f && p.y < size * 0.34f ? 1f : 0f;
                var roof = p.y > size * 0.18f && p.y < size * 0.46f && Mathf.Abs(p.x) < Mathf.Lerp(size * 0.06f, size * 0.33f, Mathf.InverseLerp(size * 0.46f, size * 0.18f, p.y)) ? 1f : 0f;
                var lower = p.y < -size * 0.20f && p.y > -size * 0.42f && Mathf.Abs(p.x) < Mathf.Lerp(size * 0.37f, size * 0.18f, Mathf.InverseLerp(-size * 0.42f, -size * 0.20f, p.y)) ? 1f : 0f;
                var outline = Mathf.Max(door, Mathf.Max(roof, lower));
                var faceHole = Mathf.Abs(p.x) < size * 0.18f && p.y > -size * 0.07f && p.y < size * 0.17f ? 1f : 0f;
                var sideHorns = Mathf.Abs(p.y - size * 0.18f) < size * 0.09f && Mathf.Abs(p.x) > size * 0.31f && Mathf.Abs(p.x) < size * 0.46f ? 1f : 0f;
                var crown = rank >= 3 && p.y > size * 0.34f && Mathf.Abs(p.x) < size * 0.24f && Mathf.Abs(Mathf.Sin((p.x + size * 0.2f) * 0.09f)) > 0.55f ? 1f : 0f;
                var ring = rank == 2 && Mathf.Abs(p.magnitude - size * 0.27f) < size * 0.022f ? 1f : 0f;
                var slash = rank >= 1 && Mathf.Abs(p.x + p.y * 0.55f) < size * 0.018f && Mathf.Abs(p.y) < size * 0.27f ? 1f : 0f;
                var alpha = Mathf.Clamp01(outline + sideHorns + crown + ring * 0.85f);
                var hot = Color.Lerp(new Color(0.18f, 0.18f, 0.20f, 1f), color, Mathf.Clamp01(sideHorns + crown + ring + slash));
                var c = alpha <= 0f ? Color.clear : new Color(hot.r, hot.g, hot.b, Mathf.Clamp01(alpha));
                if (outline > 0f && faceHole > 0f) c = new Color(0.03f, 0.035f, 0.045f, 0.98f);
                var eye = faceHole > 0f && Mathf.Abs(Mathf.Abs(p.x) - size * 0.085f) < size * 0.028f && Mathf.Abs(p.y - size * 0.045f) < size * 0.024f;
                var core = Mathf.Abs(p.x) < size * 0.030f && p.y > -size * 0.22f && p.y < -size * 0.02f;
                if (eye || core || (slash > 0f && outline > 0.1f)) c = new Color(color.r, color.g, color.b, 0.98f);
                tex.SetPixel(x, y, c);
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        Color GatekeeperAccentColor(int rank, float alpha)
        {
            var c = rank switch
            {
                1 => new Color(1f, 0.34f, 0.18f, alpha),
                2 => new Color(0.92f, 0.16f, 0.36f, alpha),
                3 => new Color(1f, 0.58f, 0.12f, alpha),
                _ => new Color(1f, 0.24f, 0.16f, alpha)
            };
            return c;
        }

        Color GatekeeperTelegraphColor(int rank, float alpha)
        {
            var c = GatekeeperAccentColor(rank, alpha);
            c.r = Mathf.Max(c.r, 0.95f);
            c.g *= 0.72f;
            c.b *= 0.72f;
            return c;
        }

        Sprite GatekeeperSigilSprite(int rank)
        {
            return rank switch
            {
                1 => MakeSectorSprite("gatekeeper_sigil_fan", Color.white, 96, 86f),
                2 => MakeRingSprite("gatekeeper_sigil_ring", Color.white, 132),
                3 => MakeGatekeeperSprite("gatekeeper_sigil_crown", GatekeeperAccentColor(rank, 1f), 112, rank),
                _ => MakeImpactDiamondSprite("gatekeeper_sigil_meteor", Color.white)
            };
        }

        Sprite GatekeeperMeteorSprite(int rank)
        {
            return MakeGatekeeperMeteorSprite($"gatekeeper_meteor_{rank}", Color.white, 96, rank);
        }

        Sprite GatekeeperCleaveSprite(int rank)
        {
            return MakeSectorSprite($"gatekeeper_cleave_{rank}", Color.white, 176, rank >= 3 ? 106f : 88f);
        }

        Sprite LoadSprite(string path)
        {
            if (spriteCache.TryGetValue(path, out var cached)) return cached;
            var catalogSprite = contentCatalog != null ? contentCatalog.SpriteForPath(path) : null;
            if (catalogSprite != null)
            {
                spriteCache[path] = catalogSprite;
                return catalogSprite;
            }
#if UNITY_EDITOR
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite != null)
            {
                spriteCache[path] = sprite;
                return sprite;
            }
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (texture != null)
            {
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
                spriteCache[path] = sprite;
                return sprite;
            }
#endif
            return null;
        }

        Sprite LoadSheetFrame(string path, int columns, int rows, int column, int rowFromTop)
        {
            var cacheKey = $"{path}:{columns}x{rows}:{column}:{rowFromTop}";
            if (spriteCache.TryGetValue(cacheKey, out var cached)) return cached;
            var texture = contentCatalog != null ? contentCatalog.TextureForPath(path) : null;
#if UNITY_EDITOR
            texture ??= AssetDatabase.LoadAssetAtPath<Texture2D>(path);
#endif
            if (texture == null) return null;

            var width = texture.width / columns;
            var height = texture.height / rows;
            var x = Mathf.Clamp(column, 0, columns - 1) * width;
            var y = texture.height - (Mathf.Clamp(rowFromTop, 0, rows - 1) + 1) * height;
            var sprite = Sprite.Create(texture, new Rect(x, y, width, height), new Vector2(0.5f, 0.5f), 100f);
            spriteCache[cacheKey] = sprite;
            return sprite;
        }

        void LoadFont()
        {
            if (contentCatalog != null && contentCatalog.koreanFont != null)
            {
                koreanFont = contentCatalog.koreanFont;
                return;
            }
#if UNITY_EDITOR
            koreanFont = AssetDatabase.LoadAssetAtPath<Font>("Assets/_dev/Fonts/Pretendard-Regular.otf");
#endif
        }

        void ResolveCatalogDefaults()
        {
            if (contentCatalog == null) return;
            dualBladesDefinition ??= contentCatalog.dualBladesDefinition;
            greatswordDefinition ??= contentCatalog.greatswordDefinition;
            utilityEchoTuningTable ??= contentCatalog.utilityEchoTuningTable;
        }

        void EnsureStyles()
        {
            if (smallStyle != null) return;
            smallStyle = new GUIStyle(GUI.skin.label) { fontSize = 15, normal = { textColor = new Color(0.90f, 0.94f, 0.96f) }, wordWrap = true };
            titleStyle = new GUIStyle(GUI.skin.label) { fontSize = 28, alignment = TextAnchor.MiddleCenter, normal = { textColor = Color.white }, fontStyle = FontStyle.Bold };
            buttonStyle = new GUIStyle(GUI.skin.button) { fontSize = 16, alignment = TextAnchor.MiddleLeft, wordWrap = true };
            panelStyle = new GUIStyle(GUI.skin.box);
            startEyebrowStyle = new GUIStyle(GUI.skin.label) { fontSize = 13, alignment = TextAnchor.MiddleCenter, normal = { textColor = new Color(0.62f, 0.96f, 1f) }, fontStyle = FontStyle.Bold };
            startBodyStyle = new GUIStyle(GUI.skin.label) { fontSize = 16, alignment = TextAnchor.UpperCenter, normal = { textColor = new Color(0.88f, 0.94f, 0.94f) }, wordWrap = true };
            startCardTitleStyle = new GUIStyle(GUI.skin.label) { fontSize = 25, alignment = TextAnchor.MiddleLeft, normal = { textColor = Color.white }, fontStyle = FontStyle.Bold };
            startKeyStyle = new GUIStyle(GUI.skin.label) { fontSize = 24, alignment = TextAnchor.MiddleCenter, normal = { textColor = new Color(0.05f, 0.10f, 0.11f) }, fontStyle = FontStyle.Bold };
            startFooterStyle = new GUIStyle(GUI.skin.label) { fontSize = 14, alignment = TextAnchor.MiddleCenter, normal = { textColor = new Color(0.82f, 0.90f, 0.90f) }, wordWrap = true };
            if (koreanFont != null)
            {
                smallStyle.font = koreanFont;
                titleStyle.font = koreanFont;
                buttonStyle.font = koreanFont;
                startEyebrowStyle.font = koreanFont;
                startBodyStyle.font = koreanFont;
                startCardTitleStyle.font = koreanFont;
                startKeyStyle.font = koreanFont;
                startFooterStyle.font = koreanFont;
            }
        }

        void CleanupLists()
        {
            enemies.RemoveAll(e => e == null);
            xpOrbs.RemoveAll(o => o == null);
        }

        void Log(string text)
        {
            combatLog.Add(text);
            if (combatLog.Count > 8) combatLog.RemoveAt(0);
        }

        public void DamagePlayer(float amount, string source)
        {
            if (deathOverlay || amount <= 0f) return;
            var finalDamage = amount * EarlyDamageMul() * Mathf.Clamp01(1f - WeaponStat.DamageReduction);
            var ash = activeMemories.FirstOrDefault(m => m.Id == V1MemoryId.AshenShield);
            if (ash != null)
            {
                var beforeAsh = finalDamage;
                finalDamage *= Mathf.Clamp01(1f - (0.08f + ash.Level * 0.03f));
                var prevented = Mathf.Max(0f, beforeAsh - finalDamage);
                AddAshenGuardCharge(prevented * (1.55f + ash.Level * 0.12f) + amount * 0.08f, ash.Level, player.position, false);
                SpawnTransientSprite("AshenGuardHit", MakeRingSprite("AshenGuardHit", Color.white, 96), player.position, Quaternion.identity, 0.42f, new Color(0.78f, 0.84f, 0.90f, 0.32f), 0.16f);
                if (ash.Level >= 3 && ashenCounterCooldown <= 0f && ashenStoredGuardCharge >= 7f + ash.Level * 2.2f)
                {
                    var release = ConsumeAshenGuardCharge(ash.Level >= 5 ? 0.68f : 0.46f, 5f);
                    ReleaseAshenGuardWave(player.position, ash.Level, release, ash.Level >= 5, "Ashen guard retribution");
                    ashenCounterCooldown = ash.Level >= 5 ? 0.42f : 0.70f;
                }
            }
            playerHp -= finalDamage;
            PlaySfx("hit_player");
            SpawnPlayerDamageCue(finalDamage, source);
            Log($"{source}: 피해 {finalDamage:0.0}");
        }

        void SpawnPlayerDamageCue(float finalDamage, string source)
        {
            if (player == null) return;
            var bossHit = source != null && source.IndexOf("Gatekeeper", StringComparison.OrdinalIgnoreCase) >= 0;
            var color = bossHit ? new Color(1f, 0.24f, 0.12f, 0.82f) : new Color(1f, 0.45f, 0.55f, 0.72f);
            SpawnTransientSprite("PlayerDamageFlash", MakeDiscSprite("PlayerDamageFlash", Color.white, 96), player.position, Quaternion.identity, bossHit ? 0.54f : 0.42f, color, bossHit ? 0.18f : 0.13f);
            SpawnTransientSprite("PlayerDamageRing", MakeRingSprite("PlayerDamageRing", Color.white, 128), player.position, Quaternion.identity, bossHit ? 0.72f : 0.56f, new Color(color.r, color.g, color.b, bossHit ? 0.92f : 0.68f), bossHit ? 0.26f : 0.18f);
            SpawnFloatingText(player.position + Vector3.up * (bossHit ? 0.70f : 0.52f), $"-{Mathf.CeilToInt(finalDamage)}", bossHit ? new Color(1f, 0.20f, 0.10f) : new Color(1f, 0.45f, 0.55f));
            if (bossHit)
            {
                cameraShakeTimer = Mathf.Max(cameraShakeTimer, 0.16f);
                cameraShakeAmount = Mathf.Max(cameraShakeAmount, 0.085f);
            }
        }

        public float XpMagnetRadius => 2.4f * (1f + WeaponStat.MagnetMul);

        static Vector2 MoveInput()
        {
#if ENABLE_INPUT_SYSTEM
            var keyboard = Keyboard.current;
            if (keyboard != null)
            {
                var x = (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed ? 1f : 0f) - (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed ? 1f : 0f);
                var y = (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed ? 1f : 0f) - (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed ? 1f : 0f);
                return new Vector2(x, y);
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
#else
            return Vector2.zero;
#endif
        }

        static bool KeyDown(KeyCode key)
        {
#if ENABLE_INPUT_SYSTEM
            var keyboard = Keyboard.current;
            if (keyboard != null)
            {
                return key switch
                {
                    KeyCode.R => keyboard.rKey.wasPressedThisFrame,
                    KeyCode.Space => keyboard.spaceKey.wasPressedThisFrame,
                    KeyCode.Alpha1 => keyboard.digit1Key.wasPressedThisFrame,
                    KeyCode.Alpha2 => keyboard.digit2Key.wasPressedThisFrame,
                    KeyCode.Alpha3 => keyboard.digit3Key.wasPressedThisFrame,
                    KeyCode.Alpha4 => keyboard.digit4Key.wasPressedThisFrame,
                    KeyCode.F1 => keyboard.f1Key.wasPressedThisFrame,
                    KeyCode.F2 => keyboard.f2Key.wasPressedThisFrame,
                    KeyCode.F3 => keyboard.f3Key.wasPressedThisFrame,
                    KeyCode.F4 => keyboard.f4Key.wasPressedThisFrame,
                    KeyCode.F5 => keyboard.f5Key.wasPressedThisFrame,
                    KeyCode.F6 => keyboard.f6Key.wasPressedThisFrame,
                    KeyCode.F7 => keyboard.f7Key.wasPressedThisFrame,
                    KeyCode.F8 => keyboard.f8Key.wasPressedThisFrame,
                    KeyCode.F9 => keyboard.f9Key.wasPressedThisFrame,
                    KeyCode.F10 => keyboard.f10Key.wasPressedThisFrame,
                    KeyCode.F11 => keyboard.f11Key.wasPressedThisFrame,
                    KeyCode.F12 => keyboard.f12Key.wasPressedThisFrame,
                    _ => false
                };
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(key);
#else
            return false;
#endif
        }

        static Sprite CachedGeneratedSprite(string key, Func<Sprite> factory)
        {
            if (generatedSpriteCache.TryGetValue(key, out var cached) && cached != null) return cached;
            var sprite = factory();
            generatedSpriteCache[key] = sprite;
            return sprite;
        }

        static string ColorKey(Color color)
        {
            return $"{Mathf.RoundToInt(color.r * 255f)}_{Mathf.RoundToInt(color.g * 255f)}_{Mathf.RoundToInt(color.b * 255f)}_{Mathf.RoundToInt(color.a * 255f)}";
        }

        static Sprite MakeCircleSprite(string name, Color color, int size)
        {
            return CachedGeneratedSprite($"circle:{name}:{ColorKey(color)}:{size}", () => MakeCircleSpriteUncached(name, color, size));
        }

        static Sprite MakeCircleSpriteUncached(string name, Color color, int size)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { name = name };
            var center = new Vector2(size * 0.5f, size * 0.5f);
            var radius = size * 0.45f;
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                var d = Vector2.Distance(new Vector2(x, y), center);
                var a = Mathf.Clamp01((radius - d) / 4f);
                tex.SetPixel(x, y, new Color(color.r, color.g, color.b, a));
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        static Sprite MakeRingSprite(string name, Color color, int size)
        {
            return CachedGeneratedSprite($"ring:{name}:{ColorKey(color)}:{size}", () => MakeRingSpriteUncached(name, color, size));
        }

        static Sprite MakeRingSpriteUncached(string name, Color color, int size)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { name = name };
            var center = new Vector2(size * 0.5f, size * 0.5f);
            var radius = size * 0.39f;
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                var d = Vector2.Distance(new Vector2(x, y), center);
                var ring = Mathf.Clamp01(1f - Mathf.Abs(d - radius) / 3.6f);
                var glow = Mathf.Clamp01(1f - Mathf.Abs(d - radius) / 11f) * 0.20f;
                var alpha = Mathf.Max(ring * 0.78f, glow);
                tex.SetPixel(x, y, alpha <= 0f ? Color.clear : new Color(color.r, color.g, color.b, alpha));
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        static Sprite MakeDiscSprite(string name, Color color, int size)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { name = name };
            var center = new Vector2(size * 0.5f, size * 0.5f);
            var radius = size * 0.39f;
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                var d = Vector2.Distance(new Vector2(x, y), center);
                var fill = Mathf.Clamp01((radius - d) / 5.5f);
                var edge = Mathf.Clamp01(1f - Mathf.Abs(d - radius) / 3.2f);
                var alpha = Mathf.Max(fill * 0.38f, edge * 0.88f);
                tex.SetPixel(x, y, alpha <= 0f ? Color.clear : new Color(color.r, color.g, color.b, alpha));
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        static Sprite MakeSectorSprite(string name, Color color, int size, float arcDegrees)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { name = name };
            var center = new Vector2(size * 0.5f, size * 0.18f);
            var radius = size * 0.76f;
            var halfArc = arcDegrees * 0.5f;
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                var p = new Vector2(x, y) - center;
                if (p.y < 0f)
                {
                    tex.SetPixel(x, y, Color.clear);
                    continue;
                }
                var distance = p.magnitude;
                var angle = Mathf.Abs(Mathf.Atan2(p.x, p.y) * Mathf.Rad2Deg);
                var insideArc = Mathf.Clamp01((halfArc - angle) / 2.8f);
                var insideRadius = Mathf.Clamp01((radius - distance) / 4.8f);
                var edge = Mathf.Max(
                    Mathf.Clamp01(1f - Mathf.Abs(angle - halfArc) / 2.4f),
                    Mathf.Clamp01(1f - Mathf.Abs(distance - radius) / 3.2f));
                var alpha = insideArc * insideRadius * 0.30f + edge * insideRadius * 0.62f;
                tex.SetPixel(x, y, alpha <= 0f ? Color.clear : new Color(color.r, color.g, color.b, Mathf.Clamp01(alpha)));
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.18f), 100f);
        }

        static Sprite MakeGatekeeperMeteorSprite(string name, Color color, int size, int rank)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { name = name };
            var center = new Vector2(size * 0.5f, size * 0.52f);
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                var p = new Vector2(x, y) - center;
                var diamond = Mathf.Clamp01(1f - (Mathf.Abs(p.x) + Mathf.Abs(p.y)) / (size * (0.34f + rank * 0.015f)));
                var inner = Mathf.Clamp01(1f - p.magnitude / (size * 0.20f));
                var tail = p.y > 0f && Mathf.Abs(p.x + Mathf.Sin(p.y * 0.13f) * size * 0.055f) < size * (0.055f + rank * 0.008f) ? Mathf.Clamp01(1f - p.y / (size * 0.46f)) : 0f;
                var crack = rank >= 2 && Mathf.Abs(p.x - p.y * 0.28f) < size * 0.020f && diamond > 0.10f ? 1f : 0f;
                var alpha = Mathf.Clamp01(diamond + inner * 0.45f + tail * 0.65f);
                var c = alpha <= 0f ? Color.clear : new Color(color.r, color.g, color.b, alpha);
                if (crack > 0f) c = new Color(1f, 0.20f, 0.08f, 0.95f);
                tex.SetPixel(x, y, c);
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        static Sprite MakeBoxSprite(string name, Color color, int width, int height)
        {
            return CachedGeneratedSprite($"box:{name}:{ColorKey(color)}:{width}x{height}", () => MakeBoxSpriteUncached(name, color, width, height));
        }

        static Sprite MakeBoxSpriteUncached(string name, Color color, int width, int height)
        {
            var tex = new Texture2D(width, height, TextureFormat.RGBA32, false) { name = name };
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                tex.SetPixel(x, y, color);
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 100f);
        }

        static Sprite MakeBladeSprite(string name, Color blade, Color hilt, int width, int height, bool greatsword)
        {
            var tex = new Texture2D(width, height, TextureFormat.RGBA32, false) { name = name };
            var center = width * 0.5f;
            var guardY = Mathf.RoundToInt(height * 0.22f);
            var pommelY = Mathf.RoundToInt(height * 0.07f);
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                var px = Mathf.Abs(x - center);
                var color = Color.clear;
                if (y >= guardY)
                {
                    var t = Mathf.InverseLerp(guardY, height - 2, y);
                    var half = Mathf.Lerp(greatsword ? width * 0.22f : width * 0.15f, 1.5f, t);
                    if (px <= half)
                    {
                        var edge = Mathf.Clamp01((half - px) / Mathf.Max(1f, half * 0.25f));
                        var shine = x < center ? 1.12f : 0.82f;
                        color = new Color(blade.r * shine, blade.g * shine, blade.b * shine, Mathf.Lerp(0.55f, 1f, edge));
                    }
                }
                else if (y >= guardY - 5 && y <= guardY + 4 && px <= (greatsword ? width * 0.44f : width * 0.36f))
                {
                    color = hilt;
                }
                else if (y < guardY && y >= pommelY && px <= (greatsword ? width * 0.09f : width * 0.08f))
                {
                    color = new Color(hilt.r * 1.25f, hilt.g * 1.25f, hilt.b * 1.25f, 1f);
                }
                else if (y < pommelY && Vector2.Distance(new Vector2(x, y), new Vector2(center, pommelY)) <= width * 0.10f)
                {
                    color = hilt;
                }

                tex.SetPixel(x, y, color);
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, width, height), new Vector2(0.5f, 0.18f), 100f);
        }

        static Sprite MakeCrescentSlashSprite(string name, Color color, bool flip)
        {
            const int width = 192;
            const int height = 128;
            var tex = new Texture2D(width, height, TextureFormat.RGBA32, false) { name = name };
            var center = new Vector2(width * 0.33f, height * 0.50f);
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                var py = flip ? height - 1 - y : y;
                var p = new Vector2(x, py) - center;
                var dist = p.magnitude;
                var angle = Mathf.Atan2(p.y, p.x) * Mathf.Rad2Deg;
                var arc = Mathf.InverseLerp(80f, 18f, Mathf.Abs(angle));
                var outer = Mathf.Clamp01(1f - Mathf.Abs(dist - 74f) / 12f);
                var inner = Mathf.Clamp01(1f - Mathf.Abs(dist - 55f) / 7f) * 0.45f;
                var tipFade = Mathf.Clamp01((x - 12f) / 28f) * Mathf.Clamp01((width - 8f - x) / 22f);
                var alpha = Mathf.Clamp01((outer + inner) * arc * tipFade);
                tex.SetPixel(x, y, alpha <= 0f ? Color.clear : new Color(color.r, color.g, color.b, alpha));
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, width, height), new Vector2(0.34f, 0.5f), 100f);
        }

        static Sprite MakeWideCrescentSprite(string name, Color color)
        {
            const int width = 280;
            const int height = 160;
            var tex = new Texture2D(width, height, TextureFormat.RGBA32, false) { name = name };
            var center = new Vector2(width * 0.25f, height * 0.50f);
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                var p = new Vector2(x, y) - center;
                var dist = p.magnitude;
                var angle = Mathf.Atan2(p.y, p.x) * Mathf.Rad2Deg;
                var absAngle = Mathf.Abs(angle);
                var arc = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(82f, 16f, absAngle));
                var bladeEdge = Mathf.Clamp01(1f - Mathf.Abs(dist - 102f) / 6.5f);
                var innerGlint = Mathf.Clamp01(1f - Mathf.Abs(dist - 92f) / 3.0f) * 0.28f;
                var outerGlow = Mathf.Clamp01(1f - Mathf.Abs(dist - 109f) / 13f) * 0.18f;
                var tipFade = Mathf.Clamp01((x - 16f) / 44f) * Mathf.Clamp01((width - 10f - x) / 34f);
                var alpha = Mathf.Clamp01((bladeEdge + innerGlint + outerGlow) * arc * tipFade);
                tex.SetPixel(x, y, alpha <= 0f ? Color.clear : new Color(color.r, color.g, color.b, alpha));
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, width, height), new Vector2(0.26f, 0.5f), 100f);
        }

        static Sprite MakeIaiSlashSprite(string name, Color color)
        {
            const int width = 192;
            const int height = 64;
            var tex = new Texture2D(width, height, TextureFormat.RGBA32, false) { name = name };
            var a = new Vector2(20f, height * 0.54f);
            var b = new Vector2(width - 18f, height * 0.36f);
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                var p = new Vector2(x, y);
                var dist = DistanceToSegment(p, a, b);
                var along = Mathf.Clamp01(Vector2.Dot(p - a, (b - a).normalized) / (b - a).magnitude);
                var alpha = Mathf.Clamp01((6.5f - dist) / 6.5f) * Mathf.Sin(along * Mathf.PI);
                var core = Mathf.Clamp01((2.2f - dist) / 2.2f);
                tex.SetPixel(x, y, alpha <= 0f ? Color.clear : new Color(color.r, color.g, color.b, Mathf.Clamp01(alpha * 0.72f + core * 0.45f)));
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 100f);
        }

        static Sprite MakeHeavySlashSprite(string name, Color color)
        {
            const int width = 224;
            const int height = 112;
            var tex = new Texture2D(width, height, TextureFormat.RGBA32, false) { name = name };
            var a = new Vector2(28f, height * 0.66f);
            var b = new Vector2(width - 22f, height * 0.30f);
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                var p = new Vector2(x, y);
                var dist = DistanceToSegment(p, a, b);
                var along = Mathf.Clamp01(Vector2.Dot(p - a, (b - a).normalized) / (b - a).magnitude);
                var taper = Mathf.Sin(along * Mathf.PI);
                var alpha = Mathf.Clamp01((16f - dist) / 16f) * taper;
                var inner = Mathf.Clamp01((5f - dist) / 5f);
                tex.SetPixel(x, y, alpha <= 0f ? Color.clear : new Color(color.r, color.g, color.b, Mathf.Clamp01(alpha * 0.55f + inner * 0.48f)));
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 100f);
        }

        static Sprite MakeImpactDiamondSprite(string name, Color color)
        {
            return CachedGeneratedSprite($"impactDiamond:{name}:{ColorKey(color)}", () => MakeImpactDiamondSpriteUncached(name, color));
        }

        static Sprite MakeImpactDiamondSpriteUncached(string name, Color color)
        {
            const int size = 96;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { name = name };
            var center = new Vector2(size * 0.5f, size * 0.5f);
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                var p = new Vector2(x, y) - center;
                var d = Mathf.Abs(p.x) + Mathf.Abs(p.y);
                var ring = Mathf.Clamp01(1f - Mathf.Abs(d - 28f) / 4f);
                var core = Mathf.Clamp01(1f - d / 18f);
                var alpha = Mathf.Max(ring * 0.70f, core * 0.38f);
                tex.SetPixel(x, y, alpha <= 0f ? Color.clear : new Color(color.r, color.g, color.b, alpha));
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        static float DistanceToSegment(Vector2 p, Vector2 a, Vector2 b)
        {
            var ab = b - a;
            var t = Mathf.Clamp01(Vector2.Dot(p - a, ab) / Mathf.Max(0.001f, ab.sqrMagnitude));
            return Vector2.Distance(p, a + ab * t);
        }

        static Sprite MakeArcSprite(string name, Color color)
        {
            const int size = 192;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { name = name };
            var center = new Vector2(size * 0.20f, size * 0.5f);
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                var p = new Vector2(x, y) - center;
                var dist = p.magnitude;
                var angle = Mathf.Abs(Mathf.Atan2(p.y, p.x) * Mathf.Rad2Deg);
                var inside = dist > 32f && dist < 118f && angle < 60f;
                tex.SetPixel(x, y, inside ? color : Color.clear);
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.20f, 0.5f), 100f);
        }

        static string MemoryName(V1MemoryId id) => id switch
        {
            V1MemoryId.HungryBlades => "굶주린 칼무리",
            V1MemoryId.BloodReflection => "피의 반사",
            V1MemoryId.ExecutionFlash => "처형 섬광",
            V1MemoryId.HunterOath => "추적자의 맹세",
            V1MemoryId.ShatterWave => "파쇄의 파문",
            V1MemoryId.StoppedSecond => "멈춘 1초",
            V1MemoryId.AshenShield => "잿빛 방패",
            V1MemoryId.OblivionBrand => "망각의 낙인",
            _ => id.ToString()
        };

        static string EchoName(V1MemoryId id) => id switch
        {
            V1MemoryId.HungryBlades => "칼무리 잔향",
            V1MemoryId.BloodReflection => "혈반 잔향",
            V1MemoryId.ExecutionFlash => "처형 잔향",
            V1MemoryId.HunterOath => "추적 잔향",
            V1MemoryId.ShatterWave => "파문 잔향",
            V1MemoryId.StoppedSecond => "정지 잔향",
            V1MemoryId.AshenShield => "잿빛 잔향",
            V1MemoryId.OblivionBrand => "낙인 잔향",
            _ => id.ToString()
        };

        [Serializable]
        public struct UtilityEchoTuningSpec
        {
            public V1MemoryId id;
            public bool firstHitOnly;
            public int firstHitGateLiftLevel;
            public float procChanceBase;
            public float procChancePerLevel;
            public float lightRadiusBase;
            public float lightRadiusPerLevel;
            public float heavyRadiusBase;
            public float heavyRadiusPerLevel;
            public int lightTargetLimit;
            public int heavyTargetLimit;
            public int lightTargetPerLevelDivisor;
            public int heavyTargetPerLevelDivisor;
            public float lightDamageBase;
            public float lightDamagePerLevel;
            public float heavyDamageBase;
            public float heavyDamagePerLevel;
            public float lightFreezeBase;
            public float lightFreezePerLevel;
            public float heavyFreezeBase;
            public float heavyFreezePerLevel;
            public float executionHealthBase;
            public float executionHealthPerLevel;

            public V1MemoryId Id => id;

            public UtilityEchoTuningSpec(
                V1MemoryId id,
                bool firstHitOnly,
                int firstHitGateLiftLevel,
                float procChanceBase,
                float procChancePerLevel,
                float lightRadiusBase,
                float lightRadiusPerLevel,
                float heavyRadiusBase,
                float heavyRadiusPerLevel,
                int lightTargetLimit,
                int heavyTargetLimit,
                int lightTargetPerLevelDivisor,
                int heavyTargetPerLevelDivisor,
                float lightDamageBase,
                float lightDamagePerLevel,
                float heavyDamageBase,
                float heavyDamagePerLevel,
                float lightFreezeBase,
                float lightFreezePerLevel,
                float heavyFreezeBase,
                float heavyFreezePerLevel,
                float executionHealthBase,
                float executionHealthPerLevel)
            {
                this.id = id;
                this.firstHitOnly = firstHitOnly;
                this.firstHitGateLiftLevel = firstHitGateLiftLevel;
                this.procChanceBase = procChanceBase;
                this.procChancePerLevel = procChancePerLevel;
                this.lightRadiusBase = lightRadiusBase;
                this.lightRadiusPerLevel = lightRadiusPerLevel;
                this.heavyRadiusBase = heavyRadiusBase;
                this.heavyRadiusPerLevel = heavyRadiusPerLevel;
                this.lightTargetLimit = lightTargetLimit;
                this.heavyTargetLimit = heavyTargetLimit;
                this.lightTargetPerLevelDivisor = lightTargetPerLevelDivisor;
                this.heavyTargetPerLevelDivisor = heavyTargetPerLevelDivisor;
                this.lightDamageBase = lightDamageBase;
                this.lightDamagePerLevel = lightDamagePerLevel;
                this.heavyDamageBase = heavyDamageBase;
                this.heavyDamagePerLevel = heavyDamagePerLevel;
                this.lightFreezeBase = lightFreezeBase;
                this.lightFreezePerLevel = lightFreezePerLevel;
                this.heavyFreezeBase = heavyFreezeBase;
                this.heavyFreezePerLevel = heavyFreezePerLevel;
                this.executionHealthBase = executionHealthBase;
                this.executionHealthPerLevel = executionHealthPerLevel;
            }

            public static UtilityEchoTuningSpec Fallback(V1MemoryId id)
            {
                return new UtilityEchoTuningSpec(id, false, 0, 1f, 0f, 1f, 0f, 1f, 0f, 1, 1, 0, 0, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f);
            }

            public bool RequiresFirstHit(int levelValue)
            {
                return firstHitOnly && (firstHitGateLiftLevel <= 0 || levelValue < firstHitGateLiftLevel);
            }

            public float ProcChance(int levelValue)
            {
                return procChanceBase + levelValue * procChancePerLevel;
            }

            public float Radius(int levelValue, bool heavy)
            {
                return heavy ? heavyRadiusBase + levelValue * heavyRadiusPerLevel : lightRadiusBase + levelValue * lightRadiusPerLevel;
            }

            public int TargetLimit(int levelValue, bool heavy)
            {
                var limit = heavy ? heavyTargetLimit : lightTargetLimit;
                var divisor = heavy ? heavyTargetPerLevelDivisor : lightTargetPerLevelDivisor;
                if (divisor > 0) limit += levelValue / divisor;
                return Mathf.Max(1, limit);
            }

            public float DamageMultiplier(int levelValue, bool heavy)
            {
                return heavy ? heavyDamageBase + levelValue * heavyDamagePerLevel : lightDamageBase + levelValue * lightDamagePerLevel;
            }

            public float FreezeSeconds(int levelValue, bool heavy)
            {
                return heavy ? heavyFreezeBase + levelValue * heavyFreezePerLevel : lightFreezeBase + levelValue * lightFreezePerLevel;
            }

            public float ExecutionHealthThreshold(int levelValue)
            {
                return executionHealthBase + levelValue * executionHealthPerLevel;
            }
        }

        sealed class MemoryState
        {
            static int nextOrder;
            public readonly V1MemoryId Id;
            public int Level;
            public int RecentOrder;
            public float TickTimer;
            public float VisualTimer;
            public float VisualSpawnTimer;

            public MemoryState(V1MemoryId id, int level)
            {
                Id = id;
                Level = level;
                RecentOrder = ++nextOrder;
            }
        }

        readonly struct Choice
        {
            public readonly string Tag;
            public readonly string Title;
            public readonly string Body;
            public readonly Action Apply;
            public Choice(string tag, string title, string body, Action apply)
            {
                Tag = tag;
                Title = title;
                Body = body;
                Apply = apply;
            }
        }

        readonly struct WeaponHit
        {
            public readonly V1Enemy Enemy;
            public readonly Vector2 Dir;
            public readonly float Distance;

            public WeaponHit(V1Enemy enemy, Vector2 dir)
            {
                Enemy = enemy;
                Dir = dir;
                Distance = dir.magnitude;
            }
        }

        readonly struct WeaponRuntimeSpec
        {
            public readonly V1WeaponId Id;
            public readonly string DisplayName;
            public readonly float Range;
            public readonly float Damage;
            public readonly float Interval;
            public readonly float ArcDegrees;
            public readonly float EngageMul;
            public readonly int MaxTargets;
            public readonly float SecondaryDamageMul;
            public readonly float PrimaryKnock;
            public readonly float SecondaryKnock;
            public readonly float Hitstop;
            public readonly float ShakeAmount;
            public readonly float SwingAnimDuration;
            public readonly float EchoSizeScale;
            public readonly float EchoDamageScale;
            public readonly V1WeaponTargetingMode TargetingMode;
            public readonly V1EchoProcStyle EchoProcStyle;
            public readonly V1UltimatePattern UltimatePattern;
            public readonly WeaponVfxProfile VfxProfile;
            public readonly float FollowupBaseDelay;
            public readonly float FollowupStagger;

            WeaponRuntimeSpec(
                V1WeaponId id,
                string displayName,
                float range,
                float damage,
                float interval,
                float arcDegrees,
                float engageMul,
                int maxTargets,
                float secondaryDamageMul,
                float primaryKnock,
                float secondaryKnock,
                float hitstop,
                float shakeAmount,
                float swingAnimDuration,
                float echoSizeScale,
                float echoDamageScale,
                V1WeaponTargetingMode targetingMode,
                V1EchoProcStyle echoProcStyle,
                V1UltimatePattern ultimatePattern,
                WeaponVfxProfile vfxProfile,
                float followupBaseDelay,
                float followupStagger)
            {
                Id = id;
                DisplayName = displayName;
                Range = range;
                Damage = damage;
                Interval = interval;
                ArcDegrees = arcDegrees;
                EngageMul = engageMul;
                MaxTargets = maxTargets;
                SecondaryDamageMul = secondaryDamageMul;
                PrimaryKnock = primaryKnock;
                SecondaryKnock = secondaryKnock;
                Hitstop = hitstop;
                ShakeAmount = shakeAmount;
                SwingAnimDuration = swingAnimDuration;
                EchoSizeScale = echoSizeScale;
                EchoDamageScale = echoDamageScale;
                TargetingMode = targetingMode;
                EchoProcStyle = echoProcStyle;
                UltimatePattern = ultimatePattern;
                VfxProfile = vfxProfile;
                FollowupBaseDelay = followupBaseDelay;
                FollowupStagger = followupStagger;
            }

            public static WeaponRuntimeSpec FromDefinition(V1WeaponId id, WeaponDefinition definition, WeaponRuntimeSpec fallback)
            {
                if (definition == null) return fallback;
                return new WeaponRuntimeSpec(
                    id,
                    string.IsNullOrWhiteSpace(definition.displayName) ? fallback.DisplayName : definition.displayName,
                    definition.attackRange,
                    definition.baseDamage,
                    definition.attackCadence,
                    definition.attackArcDegrees,
                    definition.engageMultiplier,
                    definition.maxTargetsPerSwing,
                    definition.secondaryDamageMultiplier,
                    definition.primaryKnockback,
                    definition.secondaryKnockback,
                    definition.hitStopSeconds,
                    definition.cameraShakeAmount,
                    definition.swingAnimDuration,
                    definition.echoSizeScale,
                    definition.echoDamageScale,
                    definition.targetingMode == WeaponTargetingMode.DensestArc ? V1WeaponTargetingMode.DensestArc : V1WeaponTargetingMode.Nearest,
                    definition.echoProcStyle == WeaponEchoProcStyle.SingleHeavy ? V1EchoProcStyle.SingleHeavy : V1EchoProcStyle.MultiSmall,
                    definition.ultimatePattern == Lethe.Dev.UltimatePattern.FewHeavy ? V1UltimatePattern.FewHeavy : V1UltimatePattern.ManyFast,
                    definition.vfxProfile,
                    definition.followupBaseDelay,
                    definition.followupStagger);
            }

            public static WeaponRuntimeSpec DualBlades() => new(
                V1WeaponId.DualBlades,
                "절단쌍검",
                TwinBladeRange,
                TwinBladeDamage,
                TwinBladeInterval,
                TwinBladeArcDeg,
                TwinBladeEngageMul,
                7,
                0.72f,
                1.78f,
                1.05f,
                0.022f,
                0.035f,
                0.16f,
                1.05f,
                0.75f,
                V1WeaponTargetingMode.Nearest,
                V1EchoProcStyle.MultiSmall,
                V1UltimatePattern.ManyFast,
                null,
                0.035f,
                0.012f);

            public static WeaponRuntimeSpec Greatsword() => new(
                V1WeaponId.Greatsword,
                "장송대검",
                GreatswordRange,
                GreatswordDamage,
                GreatswordInterval,
                GreatswordArcDeg,
                GreatswordEngageMul,
                6,
                0.58f,
                3.75f,
                2.10f,
                0.066f,
                0.088f,
                0.34f,
                2.15f,
                1.60f,
                V1WeaponTargetingMode.DensestArc,
                V1EchoProcStyle.SingleHeavy,
                V1UltimatePattern.FewHeavy,
                null,
                0.065f,
                0.000f);
        }

        readonly struct GreatswordSwingPose
        {
            public readonly Vector3 HandlePivot;
            public readonly Vector2 StartDirection;
            public readonly Vector2 EndDirection;
            public readonly float StartBladeAngle;
            public readonly float EndBladeAngle;
            public readonly Vector3 TipStart;
            public readonly Vector3 TipEnd;

            public GreatswordSwingPose(Vector3 handlePivot, Vector2 startDirection, Vector2 endDirection, float startBladeAngle, float endBladeAngle, Vector3 tipStart, Vector3 tipEnd)
            {
                HandlePivot = handlePivot;
                StartDirection = startDirection;
                EndDirection = endDirection;
                StartBladeAngle = startBladeAngle;
                EndBladeAngle = endBladeAngle;
                TipStart = tipStart;
                TipEnd = tipEnd;
            }
        }

        struct PendingKalmuriFollowup
        {
            public readonly Vector3 Origin;
            public readonly Vector2 Forward;
            public readonly int Level;
            public readonly int HitIndex;
            public readonly WeaponRuntimeSpec Weapon;
            public readonly bool DenseDualBlade;
            public float Delay;

            public PendingKalmuriFollowup(Vector3 origin, Vector2 forward, int level, int hitIndex, WeaponRuntimeSpec weapon, float delay, bool denseDualBlade)
            {
                Origin = origin;
                Forward = forward;
                Level = level;
                HitIndex = hitIndex;
                Weapon = weapon;
                Delay = delay;
                DenseDualBlade = denseDualBlade;
            }
        }

        readonly struct SpawnWaveProfile
        {
            readonly V1EnemyKind[] pool;
            public readonly float Interval;
            public readonly int PackSize;

            public SpawnWaveProfile(float interval, int packSize, params V1EnemyKind[] pool)
            {
                Interval = interval;
                PackSize = packSize;
                this.pool = pool;
            }

            public V1EnemyKind Pick(System.Random random) => pool[random.Next(pool.Length)];
        }

        readonly struct PressureState
        {
            public readonly float Progress;
            public readonly bool FirstCycle;
            public readonly bool Deficit;

            public PressureState(float progress, bool firstCycle, bool deficit)
            {
                Progress = progress;
                FirstCycle = firstCycle;
                Deficit = deficit;
            }
        }

        static class WeaponStat
        {
            public static float AttackSpeed;
            public static float DamageMul;
            public static float AreaMul;
            public static float DamageReduction;
            public static float MagnetMul;
            public static float EchoAmp;

            public static void Reset()
            {
                AttackSpeed = 0f;
                DamageMul = 0f;
                AreaMul = 0f;
                DamageReduction = 0f;
                MagnetMul = 0f;
                EchoAmp = 0f;
            }
        }
    }

    public sealed class V1Enemy : MonoBehaviour
    {
        const float VoidPriestHealInterval = 1.05f;
        const float VoidPriestHealAmount = 2.4f;
        const float VoidPriestHealRadius = 2.35f;
        const int VoidPriestHealTargetCap = 3;
        const float VoidPriestHealReceiverLockout = 0.95f;

        V1GameManager manager;
        Transform player;
        Transform healthRoot;
        SpriteRenderer sr;
        SpriteRenderer healthBack;
        SpriteRenderer healthFill;
        Color baseColor = Color.white;
        Vector3 baseScale;
        Vector2 knockVelocity;
        float maxHp;
        float speed;
        float shotTimer;
        float healTimer;
        float hitSquashTimer;
        float freezeTimer;
        float gatekeeperPatternTimer;
        float gatekeeperGuardTimer;
        float voidPriestHealLockout;
        float echoStateTintTimer;
        int gatekeeperPatternStep;
        Color echoStateTint = Color.white;

        public V1EnemyKind Kind { get; private set; }
        public float Hp { get; private set; }
        public float TouchDamage { get; private set; }
        public float TouchRadius { get; private set; }
        public bool IsAlive => Hp > 0f;
        public float HealthRatio => maxHp > 0f ? Mathf.Clamp01(Hp / maxHp) : 0f;
        public bool GatekeeperGuardActive => Kind == V1EnemyKind.Gatekeeper && gatekeeperGuardTimer > 0f;
        public int Score => Kind == V1EnemyKind.VoidPriest ? 3 : Kind == V1EnemyKind.DriftingEye || Kind == V1EnemyKind.SplitOne ? 2 : 1;
        public bool BloodMarked { get; set; }
        public float MarkTimer { get; set; }

        public void Configure(V1GameManager manager, V1EnemyKind kind, Transform player, float hp, float speed, float damage, float radius)
        {
            this.manager = manager;
            this.player = player;
            Kind = kind;
            Hp = hp;
            maxHp = hp;
            this.speed = speed;
            TouchDamage = damage;
            TouchRadius = radius;
            sr = GetComponent<SpriteRenderer>();
            if (sr != null) baseColor = sr.color;
            transform.localScale = Vector3.one * (kind switch
            {
                V1EnemyKind.Gatekeeper => 1.55f,
                V1EnemyKind.VoidPriest => 0.84f,
                V1EnemyKind.SplitOne => 0.82f,
                V1EnemyKind.DriftingEye => 0.76f,
                _ => 0.72f
            });
            baseScale = transform.localScale;
            gatekeeperPatternTimer = kind == V1EnemyKind.Gatekeeper ? 1.8f : 0f;
            gatekeeperGuardTimer = 0f;
            gatekeeperPatternStep = 0;
            CreateHealthBar(kind);
        }

        void Update()
        {
            if (!IsAlive || player == null || V1GameManager.GameplayPaused || V1GameManager.HitstopActive) return;
            var dt = Time.deltaTime;
            if (knockVelocity.sqrMagnitude > 0.001f)
            {
                transform.position += (Vector3)(knockVelocity * dt);
                knockVelocity = Vector2.Lerp(knockVelocity, Vector2.zero, dt * 6.5f);
            }

            if (hitSquashTimer > 0f)
            {
                hitSquashTimer -= dt;
                var pulse = Mathf.Sin(Mathf.Clamp01(hitSquashTimer / 0.08f) * Mathf.PI);
                transform.localScale = new Vector3(baseScale.x * (1f + pulse * 0.10f), baseScale.y * (1f - pulse * 0.06f), baseScale.z);
            }
            else
            {
                transform.localScale = baseScale;
            }

            var toPlayer = (Vector2)(player.position - transform.position);
            var dist = toPlayer.magnitude;
            var dir = dist > 0.01f ? toPlayer / dist : Vector2.zero;
            if (voidPriestHealLockout > 0f)
            {
                voidPriestHealLockout = Mathf.Max(0f, voidPriestHealLockout - dt);
            }

            if (freezeTimer > 0f)
            {
                freezeTimer -= dt;
                return;
            }

            if (Kind == V1EnemyKind.Gatekeeper)
            {
                gatekeeperGuardTimer = Mathf.Max(0f, gatekeeperGuardTimer - dt);
                gatekeeperPatternTimer -= dt;
                if (gatekeeperPatternTimer <= 0f)
                {
                    gatekeeperPatternStep++;
                    manager?.GatekeeperPatternPulse(this, gatekeeperPatternStep);
                    gatekeeperGuardTimer = 1.10f;
                    var rank = manager != null ? manager.GatekeeperPatternRank : 0;
                    gatekeeperPatternTimer = Mathf.Max(2.35f, 4.10f - rank * 0.34f);
                }

                var gateSpeedMul = gatekeeperGuardTimer > 0f ? 0.32f : 1f;
                transform.position += (Vector3)(dir * speed * gateSpeedMul * dt);
                ApplySeparation(dt, gatekeeperGuardTimer > 0f ? 0.10f : 0.22f);
            }
            if (Kind == V1EnemyKind.DriftingEye)
            {
                const float castRange = 4.2f;
                const float stopRange = 3.55f;
                if (dist > stopRange)
                {
                    transform.position += (Vector3)(dir * speed * dt);
                }
                else if (dist <= castRange)
                {
                    shotTimer -= dt;
                    if (shotTimer <= 0f)
                    {
                        shotTimer = 2.2f;
                        var go = new GameObject("EyeShot");
                        go.transform.position = transform.position;
                        go.AddComponent<V1EnemyShot>().Configure(manager, player, 4.8f, TouchDamage * 2.1f);
                    }
                }
                ApplySeparation(dt, dist > stopRange ? 0.55f : 0.42f);
            }
            else if (Kind != V1EnemyKind.Gatekeeper)
            {
                transform.position += (Vector3)(dir * speed * dt);
                ApplySeparation(dt, Kind == V1EnemyKind.VoidPriest ? 0.62f : 0.78f);
            }

            if (Kind == V1EnemyKind.VoidPriest)
            {
                healTimer -= dt;
                if (healTimer <= 0f)
                {
                    healTimer = VoidPriestHealInterval;
                    var healed = 0;
                    var healTargets = manager != null
                        ? manager.FindVoidPriestHealTargets(this, VoidPriestHealRadius, VoidPriestHealTargetCap)
                        : new List<V1Enemy>();
                    foreach (var enemy in healTargets)
                    {
                        if (enemy.TryReceiveVoidPriestHeal(VoidPriestHealAmount, this))
                        {
                            healed++;
                            if (healed >= VoidPriestHealTargetCap) break;
                        }
                    }
                }
            }

            if (BloodMarked)
            {
                MarkTimer -= dt;
                if (MarkTimer <= 0f) BloodMarked = false;
            }
            if (echoStateTintTimer > 0f)
            {
                echoStateTintTimer = Mathf.Max(0f, echoStateTintTimer - dt);
                if (echoStateTintTimer <= 0f) RestoreColor();
            }

            UpdateHealthBar();
        }

        public void TakeDamage(float amount, string source, bool weaponHit, Color flashColor, float flashDuration)
        {
            Hp -= amount;
            hitSquashTimer = weaponHit ? 0.08f : 0.04f;
            if (sr != null)
            {
                sr.color = flashColor;
                CancelInvoke(nameof(RestoreColor));
                Invoke(nameof(RestoreColor), Mathf.Max(0.01f, flashDuration));
            }
            if (Hp <= 0f)
            {
                Destroy(gameObject);
            }
        }

        public void ApplyHitFeedback(Vector2 direction, float strength)
        {
            knockVelocity += direction.normalized * Mathf.Clamp(strength, 0f, 6.2f);
        }

        public void ApplyBriefFreeze(float seconds)
        {
            freezeTimer = Mathf.Max(freezeTimer, seconds);
        }

        public void ApplyEchoStateMark(V1MemoryId id, Sprite sprite, Color color, float lifetime, float scaleMul)
        {
            if (!IsAlive) return;
            echoStateTint = color;
            echoStateTintTimer = Mathf.Max(echoStateTintTimer, lifetime);
            if (sr != null) sr.color = CurrentBodyColor();

            var markerName = $"EchoState_{id}";
            var child = transform.Find(markerName);
            var marker = child != null ? child.gameObject : new GameObject(markerName);
            marker.transform.SetParent(transform, false);
            marker.transform.localPosition = new Vector3(0f, Kind == V1EnemyKind.Gatekeeper ? 0.54f : 0.36f, -0.02f);
            marker.transform.localRotation = Quaternion.identity;
            marker.transform.localScale = Vector3.one * (Kind == V1EnemyKind.Gatekeeper ? 0.34f : 0.22f) * Mathf.Max(0.2f, scaleMul);

            var markerRenderer = marker.GetComponent<SpriteRenderer>();
            if (markerRenderer == null) markerRenderer = marker.AddComponent<SpriteRenderer>();
            markerRenderer.sprite = sprite;
            markerRenderer.color = color;
            markerRenderer.sortingOrder = 52;

            var badge = marker.GetComponent<V1EnemyStateBadge>();
            if (badge == null) badge = marker.AddComponent<V1EnemyStateBadge>();
            badge.Configure(color, lifetime, id);
        }

        public bool TryReceiveVoidPriestHeal(float amount, V1Enemy priest)
        {
            if (!IsAlive || Kind == V1EnemyKind.Gatekeeper || Hp >= maxHp - 0.05f)
            {
                manager?.RecordVoidPriestHealAttempt(false);
                return false;
            }

            if (voidPriestHealLockout > 0f)
            {
                manager?.RecordVoidPriestHealAttempt(false);
                return false;
            }

            Heal(amount);
            voidPriestHealLockout = VoidPriestHealReceiverLockout;
            manager?.RecordVoidPriestHealAttempt(true);
            manager?.SpawnVoidPriestHealVfx(priest != null ? priest.transform.position : transform.position, transform.position, amount);
            return true;
        }

        void ApplySeparation(float dt, float speedMultiplier)
        {
            if (manager == null || speedMultiplier <= 0f) return;
            var separation = manager.EnemySeparationForce(this);
            if (separation.sqrMagnitude <= 0.0001f) return;
            transform.position += (Vector3)(separation * speed * speedMultiplier * dt);
        }

        void Heal(float amount)
        {
            Hp = Mathf.Min(maxHp, Hp + amount);
        }

        void RestoreColor()
        {
            if (sr != null) sr.color = CurrentBodyColor();
        }

        Color CurrentBodyColor()
        {
            var color = BloodMarked ? Color.Lerp(baseColor, new Color(1f, 0.18f, 0.24f), 0.68f) : baseColor;
            if (echoStateTintTimer > 0f)
            {
                color = Color.Lerp(color, new Color(echoStateTint.r, echoStateTint.g, echoStateTint.b, color.a), BloodMarked ? 0.34f : 0.48f);
            }
            return color;
        }

        void CreateHealthBar(V1EnemyKind kind)
        {
            healthRoot = new GameObject("HealthBar").transform;
            healthRoot.SetParent(transform, false);
            healthRoot.localPosition = new Vector3(0f, kind == V1EnemyKind.Gatekeeper ? 0.78f : 0.48f, 0f);
            healthRoot.localScale = Vector3.one;

            healthBack = new GameObject("Back").AddComponent<SpriteRenderer>();
            healthBack.transform.SetParent(healthRoot, false);
            healthBack.sprite = CreateSolidSprite("enemy_hp_back", new Color(0.02f, 0.03f, 0.035f, 0.78f));
            healthBack.color = new Color(0.02f, 0.03f, 0.035f, kind == V1EnemyKind.Gatekeeper ? 0.88f : 0.58f);
            healthBack.sortingOrder = 48;
            healthBack.transform.localScale = new Vector3(kind == V1EnemyKind.Gatekeeper ? 0.92f : 0.48f, 0.055f, 1f);

            healthFill = new GameObject("Fill").AddComponent<SpriteRenderer>();
            healthFill.transform.SetParent(healthRoot, false);
            healthFill.sprite = CreateSolidSprite("enemy_hp_fill", Color.white);
            healthFill.color = kind switch
            {
                V1EnemyKind.Gatekeeper => new Color(1f, 0.20f, 0.16f, 0.86f),
                V1EnemyKind.VoidPriest => new Color(0.38f, 1f, 0.66f, 0.78f),
                V1EnemyKind.DriftingEye => new Color(0.92f, 0.52f, 1f, 0.78f),
                V1EnemyKind.SplitOne => new Color(1f, 0.78f, 0.32f, 0.78f),
                _ => new Color(0.78f, 0.96f, 1f, 0.68f)
            };
            healthFill.sortingOrder = 49;
            healthFill.transform.localPosition = new Vector3(-0.0005f, 0f, 0f);
            healthFill.transform.localScale = healthBack.transform.localScale;
        }

        void UpdateHealthBar()
        {
            if (healthFill == null || healthBack == null) return;
            if (healthRoot != null)
            {
                var sx = Mathf.Abs(transform.localScale.x) > 0.001f ? 1f / transform.localScale.x : 1f;
                var sy = Mathf.Abs(transform.localScale.y) > 0.001f ? 1f / transform.localScale.y : 1f;
                healthRoot.localScale = new Vector3(sx, sy, 1f);
            }
            var ratio = HealthRatio;
            var full = healthBack.transform.localScale;
            healthFill.transform.localScale = new Vector3(full.x * ratio, full.y, full.z);
            healthFill.transform.localPosition = new Vector3(-(full.x - full.x * ratio) * 0.5f, 0f, 0f);
        }

        static Sprite CreateSolidSprite(string name, Color color)
        {
            var tex = new Texture2D(4, 4, TextureFormat.RGBA32, false);
            for (int y = 0; y < 4; y++)
            for (int x = 0; x < 4; x++)
            {
                tex.SetPixel(x, y, color);
            }
            tex.Apply();
            tex.name = name;
            return Sprite.Create(tex, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f), 4f);
        }
    }

    public sealed class V1Projectile : MonoBehaviour
    {
        V1GameManager manager;
        V1Enemy target;
        float speed;
        float damage;
        string source;

        public void Configure(V1GameManager manager, V1Enemy target, float speed, float damage, string source)
        {
            this.manager = manager;
            this.target = target;
            this.speed = speed;
            this.damage = damage;
            this.source = source;
        }

        void Update()
        {
            if (V1GameManager.GameplayPaused || V1GameManager.HitstopActive) return;
            if (target == null || !target.IsAlive)
            {
                target = manager != null ? manager.FindNearestLivingEnemy(transform.position, target) : null;
            }
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }
            var dir = target.transform.position - transform.position;
            transform.position += dir.normalized * speed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f);
            if (dir.magnitude < 0.22f)
            {
                manager.ProjectileHit(target, damage, source);
                Destroy(gameObject);
            }
        }
    }

    public sealed class V1EnemyShot : MonoBehaviour
    {
        V1GameManager manager;
        Transform target;
        Vector3 velocity;
        float damage;
        float life = 3f;

        public void Configure(V1GameManager manager, Transform target, float speed, float damage)
        {
            this.manager = manager;
            this.target = target;
            this.damage = damage;
            velocity = (target.position - transform.position).normalized * speed;
            var sr = gameObject.AddComponent<SpriteRenderer>();
            sr.sprite = CreateShotSprite();
            sr.color = new Color(0.88f, 0.35f, 1f);
            sr.sortingOrder = 35;
        }

        void Update()
        {
            if (V1GameManager.GameplayPaused || V1GameManager.HitstopActive) return;
            life -= Time.deltaTime;
            transform.position += velocity * Time.deltaTime;
            if (target != null && Vector2.Distance(transform.position, target.position) < 0.28f)
            {
                manager?.DamagePlayer(damage, "떠도는 눈");
                Destroy(gameObject);
            }
            if (life <= 0f) Destroy(gameObject);
        }

        static Sprite CreateShotSprite()
        {
            var tex = new Texture2D(24, 24, TextureFormat.RGBA32, false);
            for (int y = 0; y < 24; y++)
            for (int x = 0; x < 24; x++)
            {
                var a = Vector2.Distance(new Vector2(x, y), new Vector2(12, 12)) < 9 ? 1f : 0f;
                tex.SetPixel(x, y, new Color(1f, 1f, 1f, a));
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, 24, 24), new Vector2(0.5f, 0.5f), 100f);
        }
    }

    public sealed class V1XpOrb : MonoBehaviour
    {
        V1GameManager manager;
        int amount;

        public void Configure(V1GameManager manager, int amount)
        {
            this.manager = manager;
            this.amount = amount;
        }

        public void Tick(Transform player, float dt)
        {
            if (player == null) return;
            var dist = Vector2.Distance(transform.position, player.position);
            var magnetRadius = manager != null ? manager.XpMagnetRadius : 2.4f;
            if (dist < magnetRadius)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, 5f * dt);
            }
            if (dist < 0.28f)
            {
                manager?.CollectXpOrb(this, amount);
            }
        }
    }

    public sealed class V1WeaponPhantomSweep : MonoBehaviour
    {
        Vector3 startPosition;
        Vector3 endPosition;
        Vector3 startScale;
        Vector3 endScale;
        float startAngle;
        float endAngle;
        float duration;
        float age;
        bool usePivot;
        Vector3 handlePivot;
        float startBladeAngle;
        float endBladeAngle;
        float centerDistance;

        public void Configure(Vector3 startPosition, Vector3 endPosition, float startAngle, float endAngle, Vector3 startScale, Vector3 endScale, float duration)
        {
            usePivot = false;
            this.startPosition = new Vector3(startPosition.x, startPosition.y, -0.05f);
            this.endPosition = new Vector3(endPosition.x, endPosition.y, -0.05f);
            this.startAngle = startAngle;
            this.endAngle = endAngle;
            this.startScale = startScale;
            this.endScale = endScale;
            this.duration = Mathf.Max(0.02f, duration);
            age = 0f;
            enabled = true;
            Apply(0f);
        }

        public void ConfigurePivot(Vector3 handlePivot, float startBladeAngle, float endBladeAngle, float centerDistance, Vector3 startScale, Vector3 endScale, float duration)
        {
            usePivot = true;
            this.handlePivot = new Vector3(handlePivot.x, handlePivot.y, -0.05f);
            this.startBladeAngle = startBladeAngle;
            this.endBladeAngle = endBladeAngle;
            this.centerDistance = Mathf.Max(0.01f, centerDistance);
            this.startScale = startScale;
            this.endScale = endScale;
            this.duration = Mathf.Max(0.02f, duration);
            age = 0f;
            enabled = true;
            Apply(0f);
        }

        void Update()
        {
            age += Time.deltaTime;
            Apply(Mathf.Clamp01(age / duration));
            if (age >= duration) enabled = false;
        }

        void Apply(float t)
        {
            var eased = 1f - Mathf.Pow(1f - t, 3f);
            if (usePivot)
            {
                var bladeAngle = Mathf.LerpAngle(startBladeAngle, endBladeAngle, eased);
                var dir = Quaternion.Euler(0f, 0f, bladeAngle) * Vector3.right;
                transform.position = handlePivot + dir * centerDistance;
                transform.rotation = Quaternion.Euler(0f, 0f, bladeAngle - 90f);
            }
            else
            {
                transform.position = Vector3.LerpUnclamped(startPosition, endPosition, eased);
                transform.rotation = Quaternion.Euler(0f, 0f, Mathf.LerpAngle(startAngle, endAngle, eased));
            }
            transform.localScale = Vector3.LerpUnclamped(startScale, endScale, eased);
        }
    }

    public sealed class V1FadingSprite : MonoBehaviour
    {
        V1GameManager manager;
        SpriteRenderer sr;
        float lifetime;
        float age;

        public void Configure(V1GameManager manager, float lifetime)
        {
            this.manager = manager;
            this.lifetime = Mathf.Max(0.02f, lifetime);
            age = 0f;
            sr = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            age += Time.deltaTime;
            if (sr != null)
            {
                var c = sr.color;
                c.a = Mathf.Lerp(c.a, 0f, age / lifetime);
                sr.color = c;
            }
            if (age >= lifetime) manager?.ReleaseTransientSprite(this);
        }
    }

    public sealed class V1RaidTelegraphFill : MonoBehaviour
    {
        SpriteRenderer sr;
        Vector3 startScale;
        Vector3 fullScale;
        Color baseColor;
        float lifetime;
        float age;

        public void Configure(Vector3 fullScale, Color baseColor, float lifetime)
        {
            sr = GetComponent<SpriteRenderer>();
            startScale = transform.localScale;
            this.fullScale = fullScale;
            this.baseColor = baseColor;
            this.lifetime = Mathf.Max(0.05f, lifetime);
            age = 0f;
            enabled = true;
        }

        void Update()
        {
            age += Time.deltaTime;
            var t = Mathf.Clamp01(age / lifetime);
            var eased = t * t * (3f - 2f * t);
            transform.localScale = Vector3.LerpUnclamped(startScale, fullScale, eased);
            if (sr != null)
            {
                var pulse = Mathf.Sin(t * Mathf.PI * 5f) * 0.08f;
                var danger = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0.62f, 1f, t));
                sr.color = new Color(
                    Mathf.Lerp(baseColor.r, 1f, danger * 0.35f),
                    Mathf.Lerp(baseColor.g, 0.22f, danger * 0.55f),
                    Mathf.Lerp(baseColor.b, 0.10f, danger * 0.55f),
                    Mathf.Clamp01(baseColor.a + danger * 0.26f + pulse));
            }
        }
    }

    public sealed class V1EnemyStateBadge : MonoBehaviour
    {
        SpriteRenderer sr;
        V1MemoryId id;
        Color color;
        Vector3 baseScale;
        float lifetime;
        float age;
        float spin;

        public void Configure(Color color, float lifetime, V1MemoryId id)
        {
            sr = GetComponent<SpriteRenderer>();
            this.color = color;
            this.id = id;
            this.lifetime = Mathf.Max(0.08f, lifetime);
            age = 0f;
            baseScale = transform.localScale;
            spin = id switch
            {
                V1MemoryId.StoppedSecond => 42f,
                V1MemoryId.HunterOath => -96f,
                V1MemoryId.OblivionBrand => 118f,
                V1MemoryId.ExecutionFlash => 0f,
                _ => 72f
            };
            enabled = true;
        }

        void Update()
        {
            age += Time.deltaTime;
            var t = Mathf.Clamp01(age / lifetime);
            var pulse = 1f + Mathf.Sin(age * 13f) * 0.08f;
            transform.localScale = baseScale * pulse;
            transform.Rotate(0f, 0f, spin * Time.deltaTime);
            if (sr != null)
            {
                var c = color;
                c.a = Mathf.Lerp(color.a, 0f, Mathf.SmoothStep(0.72f, 1f, t));
                sr.color = c;
            }
            if (age >= lifetime)
            {
                Destroy(gameObject);
            }
        }
    }

    public sealed class V1EnemyRoleMarker : MonoBehaviour
    {
        SpriteRenderer sr;
        V1EnemyKind kind;
        Color baseColor;
        Vector3 baseScale;
        float age;
        float scaleMul = 1f;

        public void Configure(V1EnemyKind kind, Color color, float scaleMul = 1f)
        {
            sr = GetComponent<SpriteRenderer>();
            this.kind = kind;
            baseColor = color;
            baseScale = transform.localScale;
            age = 0f;
            this.scaleMul = Mathf.Max(0.2f, scaleMul);
        }

        void Update()
        {
            age += Time.deltaTime;
            var pulseSpeed = kind switch
            {
                V1EnemyKind.VoidPriest => 4.8f,
                V1EnemyKind.DriftingEye => 3.2f,
                V1EnemyKind.SplitOne => 7.8f,
                V1EnemyKind.Gatekeeper => 2.0f,
                _ => 3.6f
            };
            var pulse = Mathf.Sin(age * pulseSpeed) * 0.5f + 0.5f;
            var scalePulse = kind switch
            {
                V1EnemyKind.SplitOne => 1f + pulse * 0.16f,
                V1EnemyKind.VoidPriest => 1f + pulse * 0.10f,
                V1EnemyKind.Gatekeeper => 1f + pulse * 0.06f,
                _ => 1f + pulse * 0.08f
            };
            transform.localScale = baseScale * scaleMul * scalePulse;
            var spin = kind switch
            {
                V1EnemyKind.VoidPriest => -38f,
                V1EnemyKind.DriftingEye => 54f,
                V1EnemyKind.SplitOne => 0f,
                V1EnemyKind.Gatekeeper => -18f,
                _ => 24f
            };
            if (Mathf.Abs(spin) > 0.01f)
            {
                transform.Rotate(0f, 0f, spin * Time.deltaTime);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(age * 10f) * 9f);
            }

            if (sr != null)
            {
                var c = baseColor;
                c.a = Mathf.Clamp01(baseColor.a * (0.62f + pulse * 0.42f));
                sr.color = c;
            }
        }
    }

    public sealed class V1FloatingText : MonoBehaviour
    {
        V1GameManager manager;
        string text;
        Color color;
        float age;

        public void Configure(V1GameManager manager, string text, Color color)
        {
            this.manager = manager;
            this.text = text;
            this.color = color;
            age = 0f;
        }

        void Update()
        {
            age += Time.deltaTime;
            transform.position += Vector3.up * Time.deltaTime * 0.45f;
            if (age > 0.65f) manager?.ReleaseFloatingText(this);
        }

        void OnGUI()
        {
            if (Camera.main == null) return;
            var screen = Camera.main.WorldToScreenPoint(transform.position);
            GUI.color = color;
            GUI.Label(new Rect(screen.x, Screen.height - screen.y, 60, 24), text);
            GUI.color = Color.white;
        }
    }

    public sealed class V1DamageNumber : MonoBehaviour
    {
        V1GameManager manager;
        string text;
        Color color;
        float lifetime;
        float age;
        Vector3 velocity;

        public void Configure(V1GameManager manager, string text, Color color, float lifetime)
        {
            this.manager = manager;
            this.text = text;
            this.color = color;
            this.lifetime = Mathf.Max(0.25f, lifetime);
            age = 0f;
            velocity = new Vector3(UnityEngine.Random.Range(-0.12f, 0.12f), 0.72f, 0f);
        }

        void Update()
        {
            if (V1GameManager.GameplayPaused) return;
            age += Time.deltaTime;
            transform.position += velocity * Time.deltaTime;
            velocity = Vector3.Lerp(velocity, Vector3.up * 0.28f, Time.deltaTime * 5f);
            if (age >= lifetime) manager?.ReleaseDamageNumber(this);
        }

        void OnGUI()
        {
            if (Camera.main == null) return;
            var screen = Camera.main.WorldToScreenPoint(transform.position);
            var alpha = Mathf.Clamp01(1f - age / lifetime);
            GUI.color = new Color(color.r, color.g, color.b, alpha);
            var style = new GUIStyle(GUI.skin.label)
            {
                fontSize = 16,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            style.normal.textColor = GUI.color;
            GUI.Label(new Rect(screen.x - 28f, Screen.height - screen.y - 12f, 56f, 24f), text, style);
            GUI.color = Color.white;
        }
    }

    public sealed class V1BillboardPulse : MonoBehaviour
    {
        float amount;
        float speed;
        Vector3 baseScale;

        public void Configure(float amount, float speed)
        {
            this.amount = amount;
            this.speed = speed;
            baseScale = transform.localScale == Vector3.zero ? Vector3.one : transform.localScale;
        }

        void Update()
        {
            transform.localScale = baseScale * (1f + Mathf.Sin(Time.time * speed) * amount);
        }
    }
}
