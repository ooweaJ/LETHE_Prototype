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
        const int MaxMemoryLevel = 5;
        const int MaxEchoLevel = 5;
        const int MaxActiveMemories = 3;
        const float FirstBossHp = 2200f;
        const float FastBossHp = 180f;
        const float RunSeconds = 1080f;
        const float ArenaHalfWidth = 24f;
        const float ArenaHalfHeight = 16f;
        const float ArenaTileSpacing = 2.65f;
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

        [Header("Data")]
        [SerializeField] V1ContentCatalog contentCatalog;
        [SerializeField] WeaponDefinition dualBladesDefinition;
        [SerializeField] WeaponDefinition greatswordDefinition;

        readonly List<V1Enemy> enemies = new();
        readonly List<V1XpOrb> xpOrbs = new();
        readonly List<string> combatLog = new();
        readonly List<MemoryState> activeMemories = new();
        readonly List<PendingKalmuriFollowup> pendingKalmuriFollowups = new();
        readonly Dictionary<V1MemoryId, int> echoLevels = new();
        readonly Dictionary<string, Sprite> spriteCache = new();
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
                return;
            }

            if (KeyDown(KeyCode.F1)) AddMemory(V1MemoryId.HungryBlades, 5, true);
            if (KeyDown(KeyCode.F2)) AddMemory(V1MemoryId.BloodReflection, 5, true);
            if (KeyDown(KeyCode.F3)) ForgetHighestMemory();
            if (KeyDown(KeyCode.F4)) SetEcho(V1MemoryId.HungryBlades, 5);
            if (KeyDown(KeyCode.F5)) SetEcho(V1MemoryId.BloodReflection, 5);
            if (KeyDown(KeyCode.F6)) SpawnGatekeeper();
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
            sfxSource.volume = 0.42f;
            sfxClips["select"] = MakeToneClip("sfx_select", 520f, 780f, 0.10f, 0.16f);
            sfxClips["slash_dual"] = MakeToneClip("sfx_slash_dual", 620f, 260f, 0.075f, 0.18f);
            sfxClips["slash_great"] = MakeToneClip("sfx_slash_great", 220f, 92f, 0.16f, 0.28f);
            sfxClips["hit_player"] = MakeToneClip("sfx_hit_player", 180f, 90f, 0.12f, 0.24f);
            sfxClips["kill"] = MakeToneClip("sfx_kill", 440f, 220f, 0.09f, 0.14f);
            sfxClips["levelup"] = MakeToneClip("sfx_levelup", 420f, 980f, 0.20f, 0.24f);
            sfxClips["warning"] = MakeToneClip("sfx_warning", 180f, 260f, 0.30f, 0.22f);
            sfxClips["boss_clear"] = MakeToneClip("sfx_boss_clear", 260f, 740f, 0.28f, 0.25f);
            sfxClips["clear"] = MakeToneClip("sfx_clear", 360f, 1120f, 0.42f, 0.30f);
            sfxClips["defeat"] = MakeToneClip("sfx_defeat", 220f, 72f, 0.42f, 0.22f);
        }

        void PlaySfx(string id)
        {
            if (sfxSource == null || !sfxClips.TryGetValue(id, out var clip) || clip == null) return;
            sfxSource.PlayOneShot(clip);
        }

        static AudioClip MakeToneClip(string name, float startHz, float endHz, float duration, float volume)
        {
            const int sampleRate = 22050;
            var sampleCount = Mathf.Max(1, Mathf.CeilToInt(sampleRate * duration));
            var samples = new float[sampleCount];
            var phase = 0f;
            for (int i = 0; i < sampleCount; i++)
            {
                var t = i / (float)(sampleCount - 1);
                var hz = Mathf.Lerp(startHz, endHz, t);
                phase += hz * Mathf.PI * 2f / sampleRate;
                var env = Mathf.Sin(Mathf.Clamp01(t) * Mathf.PI);
                samples[i] = Mathf.Sin(phase) * env * volume;
            }
            var clip = AudioClip.Create(name, sampleCount, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        public void DebugRunM1Smoke()
        {
            EnsureRunStarted();
            pausedForChoice = false;
            resultOverlay = false;
            refillOverlay = false;
            deathOverlay = false;
            fastDebugRun = true;
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
                    TriggerWeaponEchoes(hit.Enemy, forward, hitIndex, weapon);
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
            if (target == null) return;

            SpawnPromptSprite("ExecutionFlash", MemoryVfxSprite(V1MemoryId.ExecutionFlash), () => MakeImpactDiamondSprite("ExecutionFlash", Color.white), target.transform.position, Quaternion.identity, 2.18f, 0.94f, new Color(1f, 0.95f, 0.62f, 0.98f), 0.46f);
            SpawnExecutionFlashBurst(target.transform.position, 1.14f, 0.42f);
            DealDamage(target, 32f + memory.Level * 8.5f, "처형 섬광", false);
        }

        void UpdateHunterOath(MemoryState memory, float dt)
        {
            memory.TickTimer -= dt;
            if (memory.TickTimer > 0f) return;
            memory.TickTimer = Mathf.Max(0.52f, 1.10f - memory.Level * 0.11f);

            var targets = enemies
                .Where(e => e != null && e.IsAlive)
                .OrderBy(e => Vector2.Distance(player.position, e.transform.position))
                .Take(Mathf.Min(6, 2 + memory.Level / 2))
                .ToList();
            if (targets.Count == 0) return;

            var projectileCount = Mathf.Min(6, 2 + memory.Level / 2);
            var speed = 10.2f + memory.Level * 0.9f;
            var damage = 16f + memory.Level * 5.2f;
            SpawnTransientSprite("HunterOathMuzzle", MakeRingSprite("HunterOathMuzzle", Color.white, 104), player.position, Quaternion.identity, 0.58f + projectileCount * 0.06f, new Color(0.80f, 1f, 0.42f, 0.66f), 0.30f);
            for (int i = 0; i < projectileCount; i++)
            {
                SpawnHunterOathShot(targets[i % targets.Count], player.position, i, projectileCount, speed, damage, HunterOathSource, false);
            }
        }

        void SpawnHunterOathShot(V1Enemy target, Vector3 origin, int volleyIndex, int volleyCount, float speed, float damage, string source, bool echo)
        {
            if (target == null || !target.IsAlive) return;
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
                DealDamage(enemy, 10f + memory.Level * 4.0f, "파쇄의 파문", false, dir.sqrMagnitude > 0.01f ? dir.normalized : Vector2.up, 0.62f);
            }
        }

        void SpawnShatterWaveField(Vector3 center, float radius, float lifetime, bool echo)
        {
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
            memory.TickTimer = Mathf.Max(1.40f, 4.20f - memory.Level * 0.28f);

            var focus = enemies
                .Where(e => e != null && e.IsAlive)
                .OrderBy(e => Vector2.Distance(player.position, e.transform.position))
                .FirstOrDefault();
            var center = focus != null ? focus.transform.position : player.position;
            var radius = 1.85f + memory.Level * 0.22f;
            var freezeDuration = Mathf.Min(1.0f, 0.55f + memory.Level * 0.10f);
            SpawnStoppedSecondField(center, radius, TimeStopGold(true), 1.75f, true);
            foreach (var enemy in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(center, e.transform.position) <= radius + e.TouchRadius).Take(10).ToList())
            {
                var dir = (Vector2)(enemy.transform.position - center);
                DealDamage(enemy, 5f + memory.Level * 2.1f, "멈춘 1초", false, dir.sqrMagnitude > 0.01f ? dir.normalized : Vector2.up, 0.75f);
                enemy.ApplyBriefFreeze(freezeDuration);
            }
        }

        void UpdateAshenShield(MemoryState memory, float dt)
        {
            memory.VisualTimer -= dt;
            if (memory.VisualTimer > 0f) return;
            memory.VisualTimer = 1.15f;
            SpawnPromptSprite("AshenShield", MemoryVfxSprite(V1MemoryId.AshenShield), () => MakeRingSprite("AshenShield", Color.white, 128), player.position, Quaternion.identity, 1.66f + memory.Level * 0.11f, 0.70f + memory.Level * 0.052f, new Color(0.72f, 0.80f, 0.86f, 0.46f), 0.52f);
            SpawnTransientSprite("AshenShieldCore", MakeRingSprite("AshenShieldCore", Color.white, 96), player.position, Quaternion.Euler(0f, 0f, elapsed * -80f), 0.42f + memory.Level * 0.035f, new Color(0.90f, 0.96f, 1f, 0.30f), 0.30f);
        }

        void UpdateOblivionBrand(MemoryState memory, float dt)
        {
            memory.TickTimer -= dt;
            if (memory.TickTimer > 0f) return;
            memory.TickTimer = Mathf.Max(0.85f, 2.60f - memory.Level * 0.18f);

            foreach (var enemy in enemies.Where(e => e != null && e.IsAlive).OrderBy(_ => UnityEngine.Random.value).Take(1 + memory.Level / 2).ToList())
            {
                SpawnPromptSprite("OblivionBrand", MemoryVfxSprite(V1MemoryId.OblivionBrand), () => MakeRingSprite("OblivionBrand", Color.white, 96), enemy.transform.position + Vector3.up * 0.10f, Quaternion.identity, 1.18f, 0.54f, new Color(0.70f, 0.42f, 1f, 0.68f), 0.48f);
                SpawnTransientSprite("OblivionBrandSlash", MakeImpactDiamondSprite("OblivionBrandSlash", Color.white), enemy.transform.position + Vector3.up * 0.10f, Quaternion.Euler(0f, 0f, elapsed * 120f), 0.26f, new Color(0.92f, 0.68f, 1f, 0.70f), 0.22f);
                DealDamage(enemy, 6f + memory.Level * 2.8f, "망각의 낙인", false);
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
            memory.TickTimer = 0.16f;

            var targetLimit = 4;
            var hits = enemies
                .Where(e => e != null && e.IsAlive && Vector2.Distance(player.position, e.transform.position) <= HungryBladesRadius + memory.Level * 0.22f)
                .OrderBy(e => Vector2.Distance(player.position, e.transform.position))
                .Take(8)
                .ToList();
            for (int i = 0; i < hits.Count; i++)
            {
                var mul = i < targetLimit ? 1f : 0.55f;
                var totalDamage = HungryBladesDps * 0.18f * (1f + (memory.Level - 1) * 0.16f) * mul;
                SpawnHungryBladeBite(hits[i], memory.Level, i, totalDamage);
            }
        }

        void UpdateHungryBladesOrbitVisual(MemoryState memory, float dt)
        {
            memory.VisualTimer += dt * (2.15f + memory.Level * 0.26f);
            memory.VisualSpawnTimer -= dt;
            if (memory.VisualSpawnTimer > 0f) return;
            memory.VisualSpawnTimer = Mathf.Max(0.056f, 0.100f - memory.Level * 0.007f);
            var orbitRadius = HungryBladesRadius * 0.78f + memory.Level * 0.080f;

            var focusTargets = enemies
                .Where(e => e != null && e.IsAlive && Vector2.Distance(player.position, e.transform.position) <= HungryBladesRadius + memory.Level * 0.22f)
                .OrderBy(e => Vector2.Distance(player.position, e.transform.position))
                .Take(memory.Level >= 5 ? 3 : memory.Level >= 3 ? 2 : 1)
                .ToList();

            var bladeCount = Mathf.Clamp(7 + memory.Level * 3, 10, 22);
            for (int i = 0; i < bladeCount; i++)
            {
                var lane = i % 4;
                var speed = lane % 2 == 0 ? 210f + lane * 20f : -172f - lane * 18f;
                var wobble = Mathf.Sin(memory.VisualTimer * (2.9f + lane * 0.37f) + i * 1.73f) * 0.11f;
                var angle = memory.VisualTimer * speed + i * 360f / bladeCount + lane * 23f;
                var startRadius = orbitRadius + wobble;
                var arc = (lane % 2 == 0 ? 40f : -34f) + Mathf.Sin(memory.VisualTimer + i) * 8f;
                var scale = 0.19f + memory.Level * 0.013f + lane * 0.006f;
                var alpha = 0.74f - lane * 0.045f;
                SpawnOrbitingKalmuriBlade("KalmuriLivingOrbitBlade", player.position, startRadius, angle, angle + arc, scale, new Color(0.74f, 0.98f, 1f, alpha), 0.28f);
            }

            if (focusTargets.Count > 0)
            {
                var lungeCount = Mathf.Clamp(1 + memory.Level / 2, 1, 3);
                for (int i = 0; i < lungeCount; i++)
                {
                    var target = focusTargets[i % focusTargets.Count];
                    var targetPos = target.transform.position + Vector3.up * 0.05f;
                    var toTarget = ((Vector2)(targetPos - player.position)).normalized;
                    if (toTarget.sqrMagnitude < 0.01f) toTarget = lastAim.sqrMagnitude > 0.01f ? lastAim.normalized : Vector2.up;
                    var side = new Vector2(-toTarget.y, toTarget.x);
                    var seed = memory.VisualTimer * 260f + i * 137f;
                    var orbitDir = Quaternion.Euler(0f, 0f, seed) * Vector3.right;
                    var start = player.position + orbitDir * (orbitRadius * (0.72f + i * 0.06f)) + (Vector3)(side * Mathf.Sin(seed * 0.05f) * 0.12f);
                    var end = targetPos - (Vector3)(toTarget * (0.14f + i * 0.03f)) + (Vector3)(side * ((i - (lungeCount - 1) * 0.5f) * 0.10f));
                    var color = new Color(0.84f, 1f, 1f, 0.82f - i * 0.10f);
                    SpawnKalmuriDiveBlade("KalmuriHuntingLunge", start, end, 0.25f + memory.Level * 0.018f, color, 0.24f, 0.13f);

                    if (memory.Level >= 4)
                    {
                        var recoil = player.position + Quaternion.Euler(0f, 0f, seed + 78f) * Vector3.right * (orbitRadius * 0.42f);
                        SpawnKalmuriDiveBlade("KalmuriHuntingRecoil", end, recoil, 0.17f + memory.Level * 0.010f, new Color(0.44f, 0.90f, 1f, 0.38f), 0.18f, 0.11f);
                    }
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
            var bladeCount = Mathf.Clamp(5 + levelValue, 6, 10);
            var damagePerBlade = totalDamage / bladeCount;
            var scale = 0.15f + levelValue * 0.016f;
            SpawnTransientSprite("KalmuriBiteHalo", MakeRingSprite("KalmuriBiteHalo", Color.white, 136), center, Quaternion.identity, 0.42f + levelValue * 0.040f, new Color(0.56f, 0.96f, 1f, 0.36f), 0.20f);
            SpawnEchoWoundSlash("KalmuriBiteCut", center, toTarget, new Color(0.80f, 1f, 1f, 0.86f), 0.72f + levelValue * 0.055f, 0.18f);
            SpawnEchoWoundSlash("KalmuriBiteCrossCut", center + (Vector3)(side * 0.04f), side, new Color(0.58f, 0.94f, 1f, 0.50f), 0.46f + levelValue * 0.035f, 0.14f);
            for (int i = 0; i < bladeCount; i++)
            {
                var spread = (i - (bladeCount - 1) * 0.5f) * 0.12f;
                var phase = i * 1.37f + targetIndex * 0.41f;
                var start = center - (Vector3)(toTarget * (0.70f + Mathf.Abs(spread) * 0.60f + Mathf.Sin(phase) * 0.08f)) + (Vector3)(side * spread);
                var end = center + (Vector3)(toTarget * (0.10f + (i % 2) * 0.04f)) - (Vector3)(side * spread * 0.18f);
                SpawnKalmuriDiveBlade("KalmuriBiteDiveBlade", start, end, scale + (i % 3) * 0.012f, new Color(0.82f, 1f, 1f, 0.88f), 0.22f, 0.105f + (i % 3) * 0.012f);
                DealDamage(target, damagePerBlade, "굶주린 칼무리", false, toTarget, i == 0 ? 0.06f : 0f);

                if (i < 3 && levelValue >= 3)
                {
                    var recoil = center + (Vector3)(toTarget * 0.18f) + (Vector3)(side * (spread * 1.30f + (i - 1) * 0.16f));
                    SpawnKalmuriDiveBlade("KalmuriBiteReturnShard", end, recoil, scale * 0.78f, new Color(0.46f, 0.90f, 1f, 0.42f), 0.16f, 0.09f);
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
                    if (kalmuriLevel >= 5)
                    {
                        LaunchKalmuriBlade(enemy);
                    }
                }
            }

            var bloodLevel = EchoLevel(V1MemoryId.BloodReflection);
            if (bloodLevel > 0)
            {
                enemy.BloodMarked = true;
                enemy.MarkTimer = 2.2f + bloodLevel * 0.25f;
                SpawnTransientSprite("BloodEchoPulse", MakeRingSprite("BloodEchoPulse", Color.white, 128), enemy.transform.position, Quaternion.identity, 0.38f + bloodLevel * 0.025f, new Color(1f, 0.10f, 0.18f, 0.42f), 0.28f);
                SpawnEchoWoundSlash("BloodEchoWound", enemy.transform.position + Vector3.up * 0.04f, forward, new Color(1f, 0.10f, 0.18f, 0.62f), 0.72f, 0.30f);
                SpawnTransientSprite("혈반", LoadSprite("Assets/_dev/Art/Sprites/Echoes/Blood/spr_blood_mark_01.png"), enemy.transform.position + Vector3.up * 0.2f, Quaternion.identity, 0.33f, new Color(1f, 0.18f, 0.25f, 0.9f), 0.35f);
                if (bloodLevel >= 5 && UnityEngine.Random.value < 0.42f)
                {
                    BloodBloom(enemy, bloodLevel);
                }
            }

            TriggerUtilityEchoes(enemy, forward, hitIndex, weapon);
        }

        void TriggerUtilityEchoes(V1Enemy enemy, Vector2 forward, int hitIndex, WeaponRuntimeSpec weapon)
        {
            var shatterLevel = EchoLevel(V1MemoryId.ShatterWave);
            if (shatterLevel > 0 && (shatterLevel >= 2 || hitIndex == 0) && UnityEngine.Random.value < 0.26f + shatterLevel * 0.09f)
            {
                var radius = 1.00f + shatterLevel * 0.12f;
                SpawnShatterEchoScar(enemy.transform.position, forward, radius, 0.54f);
                SpawnShatterWaveField(enemy.transform.position, radius, 1.14f, true);
                foreach (var target in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(enemy.transform.position, e.transform.position) <= radius + e.TouchRadius).Take(5).ToList())
                {
                    var dir = (Vector2)(target.transform.position - enemy.transform.position);
                    DealDamage(target, weapon.Damage * (0.10f + shatterLevel * 0.025f), "파문 잔향", false, dir.sqrMagnitude > 0.01f ? dir.normalized : forward, 0.25f);
                }
            }

            var executionLevel = EchoLevel(V1MemoryId.ExecutionFlash);
            if (executionLevel > 0 && enemy.HealthRatio <= 0.22f + executionLevel * 0.025f)
            {
                SpawnPromptSprite("ExecutionEcho", EchoVfxSprite(V1MemoryId.ExecutionFlash), () => MakeImpactDiamondSprite("ExecutionEcho", Color.white), enemy.transform.position, Quaternion.identity, 1.82f, 0.78f, new Color(1f, 0.92f, 0.58f, 0.98f), 0.52f);
                SpawnTransientSprite("ExecutionEchoHalo", MakeRingSprite("ExecutionEchoHalo", Color.white, 132), enemy.transform.position, Quaternion.identity, 0.72f, new Color(1f, 0.90f, 0.44f, 0.46f), 0.36f);
                SpawnEchoWoundSlash("ExecutionEchoCutLine", enemy.transform.position, forward, new Color(1f, 0.96f, 0.50f, 0.82f), 1.18f, 0.38f);
                SpawnExecutionFlashBurst(enemy.transform.position, 0.92f, 0.44f);
                DealDamage(enemy, weapon.Damage * (0.18f + executionLevel * 0.05f), "처형 잔향", false);
            }

            var hunterLevel = EchoLevel(V1MemoryId.HunterOath);
            if (hunterLevel > 0 && hitIndex == 0 && UnityEngine.Random.value < 0.36f + hunterLevel * 0.09f)
            {
                var targets = enemies
                    .Where(e => e != null && e.IsAlive && e != enemy)
                    .OrderBy(e => Vector2.Distance(enemy.transform.position, e.transform.position))
                    .Take(1 + hunterLevel / 4)
                    .ToList();
                SpawnTransientSprite("HunterEchoOriginMark", MakeRingSprite("HunterEchoOriginMark", Color.white, 112), enemy.transform.position, Quaternion.Euler(0f, 0f, elapsed * 90f), 0.46f, new Color(0.74f, 1f, 0.38f, 0.54f), 0.42f);
                for (int i = 0; i < targets.Count; i++)
                {
                    SpawnEchoLink("HunterEchoAimLine", enemy.transform.position, targets[i].transform.position, new Color(0.72f, 1f, 0.36f, 0.42f), 0.40f, 0.020f);
                    SpawnHunterOathShot(targets[i], enemy.transform.position, i, targets.Count, 9.2f + hunterLevel * 0.25f, weapon.Damage * (0.22f + hunterLevel * 0.055f), HunterEchoSource, true);
                }
            }

            var stoppedLevel = EchoLevel(V1MemoryId.StoppedSecond);
            if (stoppedLevel > 0 && hitIndex == 0 && UnityEngine.Random.value < 0.34f + stoppedLevel * 0.065f)
            {
                enemy.ApplyBriefFreeze(0.28f + stoppedLevel * 0.060f);
                SpawnStoppedEchoClamp(enemy.transform.position, 0.74f + stoppedLevel * 0.055f, 1.12f);
                SpawnStoppedSecondField(enemy.transform.position, 1.36f + stoppedLevel * 0.16f, TimeStopGold(false), 1.48f, false);
            }

            var ashLevel = EchoLevel(V1MemoryId.AshenShield);
            if (ashLevel > 0 && hitIndex == 0 && UnityEngine.Random.value < 0.30f + ashLevel * 0.055f)
            {
                HealPlayer(0.28f + ashLevel * 0.12f);
                SpawnTransientSprite("AshenEchoHitSeal", MakeRingSprite("AshenEchoHitSeal", Color.white, 112), enemy.transform.position, Quaternion.Euler(0f, 0f, elapsed * -90f), 0.42f, new Color(0.82f, 0.88f, 0.94f, 0.48f), 0.42f);
                SpawnEchoLink("AshenEchoReturnThread", enemy.transform.position, player.position, new Color(0.84f, 0.90f, 1f, 0.32f), 0.34f, 0.016f);
                SpawnPromptSprite("AshenEcho", EchoVfxSprite(V1MemoryId.AshenShield), () => MakeRingSprite("AshenEcho", Color.white, 112), player.position, Quaternion.identity, 1.26f, 0.52f, new Color(0.78f, 0.86f, 0.92f, 0.52f), 0.54f);
                SpawnTransientSprite("AshenEchoGuard", MakeRingSprite("AshenEchoGuard", Color.white, 144), player.position, Quaternion.Euler(0f, 0f, elapsed * -120f), 0.58f, new Color(0.86f, 0.92f, 1f, 0.32f), 0.42f);
            }

            var oblivionLevel = EchoLevel(V1MemoryId.OblivionBrand);
            if (oblivionLevel > 0 && UnityEngine.Random.value < 0.34f + oblivionLevel * 0.065f)
            {
                SpawnPromptSprite("OblivionEcho", EchoVfxSprite(V1MemoryId.OblivionBrand), () => MakeRingSprite("OblivionEcho", Color.white, 112), enemy.transform.position, Quaternion.identity, 1.52f, 0.74f, new Color(0.76f, 0.46f, 1f, 0.76f), 0.72f);
                SpawnTransientSprite("OblivionEchoSlash", MakeImpactDiamondSprite("OblivionEchoSlash", Color.white), enemy.transform.position + Vector3.up * 0.08f, Quaternion.Euler(0f, 0f, elapsed * 140f), 0.34f, new Color(0.96f, 0.72f, 1f, 0.72f), 0.34f);
                SpawnOblivionEchoBrand(enemy.transform.position, forward, 0.64f);
                DealDamage(enemy, weapon.Damage * (0.16f + oblivionLevel * 0.042f), "낙인 잔향", false);
            }
        }

        void TriggerKalmuriEcho(V1Enemy center, Vector2 forward, int level, int hitIndex, WeaponRuntimeSpec weapon)
        {
            if (center == null) return;
            var delay = weapon.FollowupBaseDelay + Mathf.Min(hitIndex, 2) * weapon.FollowupStagger;
            pendingKalmuriFollowups.Add(new PendingKalmuriFollowup(center.transform.position, forward.normalized, level, hitIndex, weapon, delay));
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
                ResolveKalmuriFollowup(followup.Origin, followup.Forward, followup.Level, followup.HitIndex, followup.Weapon);
            }
        }

        void ResolveKalmuriFollowup(Vector3 origin, Vector2 forward, int level, int hitIndex, WeaponRuntimeSpec weapon)
        {
            var isHeavy = weapon.EchoProcStyle == V1EchoProcStyle.SingleHeavy;
            var burstCount = isHeavy ? 1 : Mathf.Clamp(1 + level / 2, 1, 3);
            var radius = (0.44f + level * 0.045f) * weapon.EchoSizeScale * (1f + WeaponStat.AreaMul * 0.55f);
            var damage = weapon.Damage * (0.22f + level * 0.045f) * weapon.EchoDamageScale * (1f + WeaponStat.EchoAmp);
            var f = forward.sqrMagnitude > 0.01f ? forward.normalized : lastAim.normalized;
            var baseAngle = Mathf.Atan2(f.y, f.x) * Mathf.Rad2Deg;
            var side = new Vector2(-f.y, f.x).normalized;
            var entries = weapon.VfxProfile != null ? weapon.VfxProfile.kalmuriFollowupSlashes : Array.Empty<SlashVfxEntry>();
            var ringScale = Mathf.Clamp(radius * 1.08f, 0.48f, isHeavy ? 1.08f : 0.82f);
            var ringColor = isHeavy ? new Color(0.92f, 0.98f, 1f, 0.58f) : new Color(0.58f, 0.96f, 1f, 0.50f);
            SpawnTransientSprite("KalmuriEchoRange", MakeRingSprite("KalmuriEchoRange", Color.white, 128), origin, Quaternion.identity, ringScale, ringColor, isHeavy ? 0.30f : 0.22f);
            SpawnTransientSprite("KalmuriEchoFlash", MakeRingSprite("KalmuriEchoFlash", Color.white, 96), origin, Quaternion.Euler(0f, 0f, baseAngle), Mathf.Clamp(radius * 0.55f, 0.30f, 0.60f), new Color(0.86f, 1f, 1f, 0.42f), isHeavy ? 0.22f : 0.16f);
            SpawnEchoWoundSlash("KalmuriEchoCutTrace", origin, f, new Color(0.78f, 1f, 1f, isHeavy ? 0.82f : 0.64f), isHeavy ? 1.18f : 0.82f, isHeavy ? 0.30f : 0.22f);
            var surgeCount = isHeavy ? Mathf.Clamp(7 + level, 8, 12) : Mathf.Clamp(4 + level, 5, 10);
            for (int i = 0; i < surgeCount; i++)
            {
                var spread = (i - (surgeCount - 1) * 0.5f) * (isHeavy ? 0.13f : 0.10f);
                var lane = i % 3;
                var start = origin - (Vector3)(f * (isHeavy ? 0.62f : 0.46f) + side * spread * 1.35f);
                var end = origin + (Vector3)(f * (isHeavy ? 0.30f : 0.20f) + side * (spread * 0.22f + Mathf.Sin(i * 1.7f + hitIndex) * 0.05f));
                var scale = (isHeavy ? 0.24f : 0.17f) + level * (isHeavy ? 0.012f : 0.010f) + lane * 0.010f;
                var color = isHeavy
                    ? new Color(0.92f, 1f, 1f, 0.92f - lane * 0.06f)
                    : new Color(0.78f, 1f, 1f, 0.78f - lane * 0.06f);
                SpawnKalmuriDiveBlade(isHeavy ? "KalmuriEchoHeavySurgeBlade" : "KalmuriEchoSurgeBlade", start, end, scale, color, isHeavy ? 0.30f : 0.22f, isHeavy ? 0.15f : 0.11f);
            }
            SpawnKalmuriEchoBarrage(origin, f, side, baseAngle, level, isHeavy);

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

            if (isHeavy)
            {
                hitstopTimer = Mathf.Max(hitstopTimer, 0.045f);
                cameraShakeTimer = Mathf.Max(cameraShakeTimer, 0.10f);
                cameraShakeAmount = Mathf.Max(cameraShakeAmount, 0.05f);
            }

            var targets = enemies
                .Where(e => e != null && e.IsAlive && Vector2.Distance(origin, e.transform.position) <= radius + e.TouchRadius)
                .OrderBy(e => Vector2.Distance(origin, e.transform.position))
                .Take(isHeavy ? 6 : level >= 5 ? 5 : 3)
                .ToList();

            for (int i = 0; i < targets.Count; i++)
            {
                var toTarget = (Vector2)(targets[i].transform.position - origin);
                var mul = i == 0 ? 1f : 0.55f;
                DealDamage(targets[i], damage * mul, "칼무리 잔향", true, toTarget.sqrMagnitude > 0.01f ? toTarget.normalized : f, 0.28f);
            }
        }

        void SpawnKalmuriEchoBarrage(Vector3 origin, Vector2 forward, Vector2 side, float baseAngle, int levelValue, bool isHeavy)
        {
            var bladeCount = isHeavy ? Mathf.Clamp(5 + levelValue, 6, 10) : Mathf.Clamp(3 + levelValue, 4, 8);
            var scale = isHeavy ? 0.21f : 0.155f + levelValue * 0.012f;
            var lifetime = isHeavy ? 0.32f : 0.24f;
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
                UpdateUtilityUltimate(dt);
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
        void UpdateUtilityUltimate(float dt)
        {
            if (FractureExecutionReady)
            {
                if (ultimatePulseTimer <= 0f)
                {
                    ultimatePulseTimer = 0.58f;
                    var target = enemies.Where(e => e != null && e.IsAlive).OrderBy(e => e.HealthRatio).FirstOrDefault();
                    if (target != null)
                    {
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
                SpawnPromptSprite("AshenOblivion", LoadSprite(UltimateAshenOblivionPath), () => MakeRingSprite("AshenOblivion", Color.white, 180), player.position, Quaternion.identity, 3.24f, 1.30f, new Color(0.78f, 0.68f, 1f, 0.60f), 0.72f);
                SpawnTransientSprite("AshenOblivionGuard", MakeRingSprite("AshenOblivionGuard", Color.white, 144), player.position, Quaternion.Euler(0f, 0f, elapsed * -120f), 1.32f, new Color(0.88f, 0.92f, 1f, 0.42f), 0.56f);
                foreach (var enemy in enemies.Where(e => e != null && e.IsAlive && Vector2.Distance(player.position, e.transform.position) < 3.2f).Take(10).ToList())
                {
                    DealDamage(enemy, 30f, "잿빛 망각", false);
                }
            }
        }

        void LaunchKalmuriBlade(V1Enemy first)
        {
            var target = enemies.Where(e => e != null && e.IsAlive && e != first).OrderBy(e => Vector2.Distance(first.transform.position, e.transform.position)).FirstOrDefault();
            if (target == null) return;
            var go = new GameObject("KalmuriLaunchBlade");
            go.transform.position = player.position;
            go.transform.localScale = Vector3.one * 0.24f;
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = LoadSprite(KalmuriLaunchBladePath) ?? MakeBoxSprite("launch", Color.cyan, 16, 64);
            sr.sortingOrder = 45;
            SpawnTransientSprite("KalmuriAwakenLaunch", MakeRingSprite("KalmuriAwakenLaunch", Color.white, 120), first.transform.position, Quaternion.identity, 0.52f, new Color(0.68f, 1f, 1f, 0.48f), 0.32f);
            go.AddComponent<V1Projectile>().Configure(this, target, 9.2f, 20f, "칼무리 각성");
        }

        void BloodBloom(V1Enemy center, int level)
        {
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

        public void GatekeeperPatternPulse(V1Enemy gatekeeper, int patternStep)
        {
            if (gatekeeper == null || player == null || deathOverlay) return;
            var rank = GatekeeperPatternRank;
            var center = gatekeeper.transform.position;
            var pulseRadius = 1.65f + rank * 0.22f;
            var lineCount = 8 + rank * 2;
            var color = new Color(1f, 0.24f, 0.16f, 0.62f);
            SpawnTransientSprite("GatekeeperPulseOuter", MakeRingSprite("GatekeeperPulseOuter", Color.white, 180), center, Quaternion.identity, pulseRadius, color, 0.54f);
            SpawnTransientSprite("GatekeeperPulseInner", MakeRingSprite("GatekeeperPulseInner", Color.white, 132), center, Quaternion.Euler(0f, 0f, elapsed * -120f), pulseRadius * 0.58f, new Color(1f, 0.70f, 0.36f, 0.46f), 0.46f);
            SpawnTransientSprite("GatekeeperGuardCore", MakeImpactDiamondSprite("GatekeeperGuardCore", Color.white), center + Vector3.up * 0.18f, Quaternion.Euler(0f, 0f, 45f), 0.42f, new Color(1f, 0.92f, 0.52f, 0.76f), 0.40f);

            var line = MakeBoxSprite("GatekeeperPulseLine", Color.white, 7, 128);
            for (int i = 0; i < lineCount; i++)
            {
                var angle = patternStep * 17f + i * 360f / lineCount;
                var dir = Quaternion.Euler(0f, 0f, angle) * Vector3.right;
                var pos = center + dir * (pulseRadius * 0.34f);
                SpawnTransientSpriteScaled("GatekeeperPulseSpoke", line, pos, Quaternion.Euler(0f, 0f, angle - 90f), new Vector3(0.026f, pulseRadius * 0.56f, 1f), new Color(1f, 0.52f, 0.24f, 0.42f), 0.42f);
            }

            var toPlayer = (Vector2)(player.position - center);
            if (toPlayer.magnitude <= pulseRadius + 0.34f)
            {
                var dir = toPlayer.sqrMagnitude > 0.01f ? toPlayer.normalized : lastAim.normalized;
                playerMoveVelocity += dir * (1.25f + rank * 0.22f);
                DamagePlayer(8.5f + rank * 2.2f, "문지기 파문");
            }

            cameraShakeTimer = Mathf.Max(cameraShakeTimer, 0.12f);
            cameraShakeAmount = Mathf.Max(cameraShakeAmount, 0.045f + rank * 0.012f);
        }

        void AddEnemyRoleMarker(Transform target, V1EnemyKind kind)
        {
            if (target == null || kind == V1EnemyKind.Eroder) return;
            var marker = new GameObject("RoleMarker");
            marker.transform.SetParent(target, false);
            marker.transform.localPosition = new Vector3(0f, -0.06f, 0.02f);
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
            if (fastDebugRun) return FastBossHp;
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
            if (enemy.Kind != V1EnemyKind.Gatekeeper) PlaySfx("kill");
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
            resultOverlay = true;
            SpawnEchoTransformVfx(forgotten.Id);
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
            SpawnTransientSprite("EchoTransformOuter", MakeRingSprite("EchoTransformOuter", Color.white, 180), player.position, Quaternion.identity, 1.32f, new Color(color.r, color.g, color.b, 0.52f), 0.70f);
            SpawnTransientSprite("EchoTransformCore", MakeImpactDiamondSprite("EchoTransformCore", Color.white), player.position + Vector3.up * 0.14f, Quaternion.Euler(0f, 0f, 45f), 0.42f, new Color(color.r, color.g, color.b, 0.78f), 0.46f);
            for (int i = 0; i < 16; i++)
            {
                var angle = i * 22.5f;
                var pos = player.position + Quaternion.Euler(0f, 0f, angle) * Vector3.right * (1.05f + i * 0.035f);
                SpawnTransientSprite("망각 변환", null, pos, Quaternion.Euler(0f, 0f, angle), 0.16f + i * 0.014f, color, 0.70f);
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
            SpawnTransientSprite("UltimateReadyOuter", MakeRingSprite("UltimateReadyOuter", Color.white, 180), player.position, Quaternion.identity, 1.65f, new Color(color.r, color.g, color.b, 0.54f), 0.86f);
            SpawnTransientSprite("UltimateReadyInner", MakeRingSprite("UltimateReadyInner", Color.white, 144), player.position, Quaternion.Euler(0f, 0f, elapsed * 120f), 0.92f, new Color(1f, 1f, 1f, 0.34f), 0.62f);
            SpawnTransientSprite("UltimateReadyCore", MakeImpactDiamondSprite("UltimateReadyCore", Color.white), player.position + Vector3.up * 0.20f, Quaternion.Euler(0f, 0f, 45f), 0.54f, color, 0.48f);
            SpawnFloatingText(player.position + Vector3.up * 1.50f, UltimateReadyName(), color);
            cameraShakeTimer = Mathf.Max(cameraShakeTimer, 0.22f);
            cameraShakeAmount = Mathf.Max(cameraShakeAmount, 0.085f);
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
            GUI.Label(new Rect(Screen.width - 314, 278, 292, 20), "One: selected memory/echo   Rev: all echo", smallStyle);
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
            var entries = weapon.VfxProfile != null ? weapon.VfxProfile.weaponHitSlashes : Array.Empty<SlashVfxEntry>();
            var hasProfileSlash = entries != null && entries.Length > 0;
            if (weapon.Id == V1WeaponId.Greatsword)
            {
                SpawnGreatswordGuaranteedSlash(hits[0].Enemy.transform.position, hitCenter, f, hasProfileSlash);
            }

            if (entries == null || entries.Length == 0) return;

            for (int i = 0; i < hits.Count; i++)
            {
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
            var go = RentPooled(transientSpritePool, name);
            go.transform.position = new Vector3(position.x, position.y, -0.05f);
            go.transform.rotation = rotation;
            go.transform.localScale = scale * CombatVfxVisibilityScale;
            var sweep = go.GetComponent<V1WeaponPhantomSweep>();
            if (sweep != null) sweep.enabled = false;
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
                V1EnemyKind.Gatekeeper => LoadSheetFrame(BossGatekeeperPath, 1, 1, 0, 0) ?? MakeGatekeeperSprite("gatekeeper", EnemyColor(kind), 144),
                _ => MakeCircleSprite(kind.ToString(), EnemyColor(kind), 72)
            };
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

        static Sprite MakeGatekeeperSprite(string name, Color color, int size)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { name = name };
            var center = new Vector2(size * 0.5f, size * 0.5f);
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                var p = new Vector2(x, y) - center;
                var arch = Mathf.Clamp01(1f - Mathf.Abs(p.x) / (size * 0.34f)) * Mathf.Clamp01(1f - Mathf.Abs(p.y) / (size * 0.42f));
                var hollow = Mathf.Clamp01(1f - Mathf.Abs(p.x) / (size * 0.18f)) * Mathf.Clamp01(1f - Mathf.Abs(p.y + size * 0.02f) / (size * 0.28f));
                var horns = Mathf.Abs(p.y - size * 0.24f) < size * 0.05f && Mathf.Abs(p.x) > size * 0.22f && Mathf.Abs(p.x) < size * 0.42f ? 1f : 0f;
                var alpha = Mathf.Clamp01(arch - hollow * 0.75f + horns);
                var c = alpha <= 0f ? Color.clear : new Color(color.r, color.g, color.b, Mathf.Clamp01(alpha));
                tex.SetPixel(x, y, c);
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
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
                finalDamage *= Mathf.Clamp01(1f - (0.08f + ash.Level * 0.03f));
                SpawnTransientSprite("AshenGuardHit", MakeRingSprite("AshenGuardHit", Color.white, 96), player.position, Quaternion.identity, 0.42f, new Color(0.78f, 0.84f, 0.90f, 0.32f), 0.16f);
            }
            playerHp -= finalDamage;
            PlaySfx("hit_player");
            SpawnFloatingText(player.position + Vector3.up * 0.45f, $"-{Mathf.CeilToInt(finalDamage)}", new Color(1f, 0.45f, 0.55f));
            Log($"{source}: 피해 {finalDamage:0.0}");
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

        static Sprite MakeCircleSprite(string name, Color color, int size)
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

        static Sprite MakeBoxSprite(string name, Color color, int width, int height)
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
            public float Delay;

            public PendingKalmuriFollowup(Vector3 origin, Vector2 forward, int level, int hitIndex, WeaponRuntimeSpec weapon, float delay)
            {
                Origin = origin;
                Forward = forward;
                Level = level;
                HitIndex = hitIndex;
                Weapon = weapon;
                Delay = delay;
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
        int gatekeeperPatternStep;

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
            }
            else if (Kind != V1EnemyKind.Gatekeeper)
            {
                transform.position += (Vector3)(dir * speed * dt);
            }

            if (Kind == V1EnemyKind.VoidPriest)
            {
                healTimer -= dt;
                if (healTimer <= 0f)
                {
                    healTimer = 1.4f;
                    foreach (var enemy in FindObjectsByType<V1Enemy>(FindObjectsSortMode.None))
                    {
                        if (enemy != this && enemy.IsAlive && Vector2.Distance(transform.position, enemy.transform.position) < 2.2f)
                        {
                            enemy.Heal(3.5f);
                        }
                    }
                }
            }

            if (BloodMarked)
            {
                MarkTimer -= dt;
                if (MarkTimer <= 0f) BloodMarked = false;
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

        void Heal(float amount)
        {
            Hp = Mathf.Min(maxHp, Hp + amount);
        }

        void RestoreColor()
        {
            if (sr != null) sr.color = BloodMarked ? Color.Lerp(baseColor, new Color(1f, 0.18f, 0.24f), 0.68f) : baseColor;
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
