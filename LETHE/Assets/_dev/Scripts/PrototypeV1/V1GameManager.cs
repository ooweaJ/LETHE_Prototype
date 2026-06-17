using System;
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
        const float TwinBladeRange = 94f / PixelsPerUnit;
        const float TwinBladeInterval = 0.36f;
        const float TwinBladeDamage = 15f;
        const float TwinBladeArcDeg = 119f;
        const float TwinBladeEngageMul = 1.15f;
        const float GreatswordRange = 126f / PixelsPerUnit;
        const float GreatswordInterval = 1.02f;
        const float GreatswordDamage = 42f;
        const float GreatswordArcDeg = 82f;
        const float GreatswordEngageMul = 1.08f;
        const float ReviewFirstBossSeconds = 62f;
        const float ReviewDeficitSeconds = 22f;
        const float HungryBladesRadius = 72f / PixelsPerUnit;
        const float HungryBladesDps = 28f;
        const int MaxMemoryLevel = 5;
        const int MaxEchoLevel = 5;
        const int MaxActiveMemories = 3;
        const float FirstBossHp = 760f;
        const float RunSeconds = 600f;

        [Header("Data")]
        [SerializeField] WeaponDefinition dualBladesDefinition;
        [SerializeField] WeaponDefinition greatswordDefinition;

        readonly List<V1Enemy> enemies = new();
        readonly List<V1XpOrb> xpOrbs = new();
        readonly List<string> combatLog = new();
        readonly List<MemoryState> activeMemories = new();
        readonly List<PendingKalmuriFollowup> pendingKalmuriFollowups = new();
        readonly Dictionary<V1MemoryId, int> echoLevels = new();
        readonly Dictionary<string, Sprite> spriteCache = new();
        readonly System.Random rng = new(120612);

        Camera mainCamera;
        Transform player;
        Transform weaponAnchor;
        SpriteRenderer playerSprite;
        SpriteRenderer leftBladeSprite;
        SpriteRenderer rightBladeSprite;
        Sprite dualLeftWeaponSprite;
        Sprite dualRightWeaponSprite;
        Sprite greatswordWeaponSprite;
        GUIStyle smallStyle;
        GUIStyle titleStyle;
        GUIStyle buttonStyle;
        GUIStyle panelStyle;
        Font koreanFont;

        float playerHp = 210f;
        float playerMaxHp = 210f;
        float elapsed;
        float weaponTimer;
        float weaponAnimTimer;
        bool leftBladeLead = true;
        Vector2 lastAim = Vector2.up;
        float spawnTimer;
        float bossTimer = ReviewFirstBossSeconds;
        float refillTimer;
        float hitstopTimer;
        float cameraShakeTimer;
        float cameraShakeAmount;
        float ultimatePulseTimer;
        int level = 1;
        int xp;
        int nextXp = 5;
        int kills;
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
        string overlayTitle = "";
        string overlayBody = "";
        V1WeaponId currentWeaponId = V1WeaponId.DualBlades;
        V1MemoryId? lastForgotten;

        public static bool GameplayPaused { get; private set; }
        public static bool HitstopActive { get; private set; }
        public bool BloodBladeStormReady => EchoLevel(V1MemoryId.HungryBlades) >= 5 && EchoLevel(V1MemoryId.BloodReflection) >= 5;

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
            mainCamera.orthographicSize = 6.1f;
            mainCamera.backgroundColor = new Color(0.035f, 0.045f, 0.055f);

            WeaponStat.AttackSpeed = 0f;
            WeaponStat.DamageMul = 0f;
            LoadFont();
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
            if (KeyDown(KeyCode.Space) && resultOverlay)
            {
                ContinueAfterForgetResult();
            }
            if (KeyDown(KeyCode.Space) && refillOverlay)
            {
                ReacquireLastForgotten();
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
                return;
            }
            HitstopActive = false;

            elapsed += dt;
            UpdateReviewPacing();
            bossTimer -= dt;
            if (bossTimer <= 0f && !enemies.Any(e => e.Kind == V1EnemyKind.Gatekeeper))
            {
                SpawnGatekeeper();
                bossTimer = 9999f;
            }

            if (refillTimer > 0f)
            {
                refillTimer -= dt;
                if (refillTimer <= 0f && activeMemories.Count < MaxActiveMemories)
                {
                    refillOverlay = true;
                    overlayTitle = "기억 보충";
                    overlayBody = "결손 생존 종료.\nSpace로 잃었던 기억을 공명 재획득합니다.\n잔향은 사라지지 않고 무기에 남습니다.";
                }
            }

            UpdatePlayer(dt);
            UpdateCamera();
            UpdateWeaponVisuals(dt);
            UpdateWeapon(dt);
            UpdateActiveMemories(dt);
            UpdateEchoUltimate(dt);
            UpdatePendingKalmuriFollowups(dt);
            UpdateSpawning(dt);
            UpdateXpCollection(dt);
            CleanupLists();

            if (playerHp <= 0f)
            {
                deathOverlay = true;
                overlayTitle = "사망";
                overlayBody = $"생존 {Mathf.FloorToInt(elapsed)}초 / 처치 {kills} / Lv.{level}\nR 키로 재시작";
            }
        }

        public string DebugSnapshot()
        {
            var memoryText = string.Join(",", activeMemories.Select(m => $"{m.Id}:{m.Level}"));
            var echoText = string.Join(",", echoLevels.Where(kv => kv.Value > 0).Select(kv => $"{kv.Key}:{kv.Value}"));
            var liveEnemies = enemies.Count(e => e != null && e.IsAlive);
            return $"scene=v1 weapon={CurrentWeaponSpec().DisplayName} elapsed={elapsed:0.0} hp={playerHp:0.0}/{playerMaxHp:0.0} level={level} xp={xp}/{nextXp} kills={kills} memories=[{memoryText}] echoes=[{echoText}] enemies={liveEnemies} storm={BloodBladeStormReady} result={resultOverlay} refill={refillOverlay} death={deathOverlay}";
        }

        public void DebugRunM1Smoke()
        {
            EnsureRunStarted();
            pausedForChoice = false;
            resultOverlay = false;
            refillOverlay = false;
            deathOverlay = false;
            fastDebugRun = true;
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
            playerHp = Mathf.Max(playerHp, playerMaxHp * 0.75f);
            AddMemory(V1MemoryId.HungryBlades, 5, true);
            AddMemory(V1MemoryId.BloodReflection, 3, true);
            ForgetHighestMemory();
            ContinueAfterForgetResult();
            ReacquireLastForgotten();
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
            overlayBody = "망각 -> 잔향 +5 -> 공명 재획득 -> 피의 칼폭풍 활성.\nContinue 버튼 또는 Space로 전투 복귀.";
            resultOverlay = true;
            Log("디버그 M2: 망각/공명/궁극 루프 압축 완료");
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
            var floorSprite = LoadSprite("Assets/_dev/Art/Sprites/Map/tile_dev_floor_dark_01.png") ?? MakeBoxSprite("floor", new Color(0.08f, 0.09f, 0.105f), 64, 64);
            for (int x = -5; x <= 5; x++)
            {
                for (int y = -4; y <= 4; y++)
                {
                    var tile = new GameObject($"Floor_{x}_{y}");
                    tile.transform.position = new Vector3(x * 2.5f, y * 2.5f, 1f);
                    var sr = tile.AddComponent<SpriteRenderer>();
                    sr.sprite = floorSprite;
                    sr.color = new Color(0.18f, 0.20f, 0.24f, 1f);
                    sr.sortingOrder = -100;
                    tile.transform.localScale = Vector3.one * 2.6f;
                }
            }
        }

        void CreatePlayer()
        {
            var go = new GameObject("Player_V1");
            player = go.transform;
            player.position = Vector3.zero;
            playerSprite = go.AddComponent<SpriteRenderer>();
            playerSprite.sprite = LoadSheetFrame("Assets/_dev/Art/Sprites/Characters/Player/sheet_player_4dir.png", 4, 8, 0, 0) ?? MakeCircleSprite("player", new Color(0.78f, 0.88f, 1f), 96);
            playerSprite.sortingOrder = 20;
            go.AddComponent<V1BillboardPulse>().Configure(0.03f, 2.2f);

            weaponAnchor = new GameObject("WeaponAnchor").transform;
            weaponAnchor.SetParent(player);
            weaponAnchor.localPosition = new Vector3(0.25f, -0.05f, 0f);

            dualLeftWeaponSprite = LoadSprite("Assets/_dev/Art/Sprites/Weapons/spr_weapon_dual_blade_left_01.png") ?? MakeBladeSprite("dual-left", new Color(0.80f, 0.96f, 1f), new Color(0.16f, 0.26f, 0.34f), 34, 112, false);
            dualRightWeaponSprite = LoadSprite("Assets/_dev/Art/Sprites/Weapons/spr_weapon_dual_blade_right_01.png") ?? MakeBladeSprite("dual-right", new Color(0.88f, 0.98f, 1f), new Color(0.18f, 0.30f, 0.38f), 34, 112, false);
            greatswordWeaponSprite = MakeBladeSprite("greatsword-visual", new Color(0.82f, 0.96f, 1f), new Color(0.10f, 0.18f, 0.24f), 58, 178, true);
            leftBladeSprite = CreateWeaponSprite("LeftBlade", dualLeftWeaponSprite, new Vector3(-0.22f, -0.05f, 0f));
            rightBladeSprite = CreateWeaponSprite("RightBlade", dualRightWeaponSprite, new Vector3(0.22f, -0.05f, 0f));
        }

        SpriteRenderer CreateWeaponSprite(string name, Sprite sprite, Vector3 localPos)
        {
            var go = new GameObject(name);
            go.transform.SetParent(weaponAnchor);
            go.transform.localPosition = localPos;
            go.transform.localScale = Vector3.one * 0.36f;
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.sortingOrder = 30;
            sr.transform.localRotation = Quaternion.Euler(0f, 0f, name.Contains("Left") ? 12f : -12f);
            return sr;
        }

        void UpdatePlayer(float dt)
        {
            var move = MoveInput();
            if (move.sqrMagnitude > 1f) move.Normalize();
            player.position += (Vector3)(move * PlayerSpeed * dt);
            player.position = new Vector3(Mathf.Clamp(player.position.x, -12f, 12f), Mathf.Clamp(player.position.y, -8.5f, 8.5f), 0f);

            if (move.sqrMagnitude > 0.01f)
            {
                var angle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;
                weaponAnchor.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
                lastAim = move.normalized;
                playerSprite.flipX = move.x < -0.1f;
            }

            foreach (var enemy in enemies)
            {
                if (!enemy.IsAlive) continue;
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
            var target = player.position + new Vector3(0f, 0f, -10f);
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

            var weapon = CurrentWeaponSpec();
            var t = Mathf.Clamp01(weaponAnimTimer / weapon.SwingAnimDuration);
            var swing = Mathf.Sin(t * Mathf.PI);
            if (weapon.Id == V1WeaponId.Greatsword)
            {
                leftBladeSprite.enabled = false;
                rightBladeSprite.enabled = true;
                rightBladeSprite.sprite = greatswordWeaponSprite;
                rightBladeSprite.color = new Color(0.88f, 0.98f, 1f, 0.98f);
                rightBladeSprite.transform.localScale = Vector3.one * (0.44f + 0.07f * swing);
                rightBladeSprite.transform.localPosition = new Vector3(0.02f + 0.21f * swing, -0.14f + 0.10f * swing, 0f);
                rightBladeSprite.transform.localRotation = Quaternion.Euler(0f, 0f, -28f + 128f * swing);
                return;
            }

            leftBladeSprite.enabled = true;
            rightBladeSprite.enabled = true;
            leftBladeSprite.sprite = dualLeftWeaponSprite;
            rightBladeSprite.sprite = dualRightWeaponSprite;
            leftBladeSprite.color = Color.white;
            rightBladeSprite.color = Color.white;
            leftBladeSprite.transform.localScale = Vector3.one * (0.30f + 0.03f * swing);
            rightBladeSprite.transform.localScale = Vector3.one * (0.30f + 0.03f * swing);
            var leadMul = leftBladeLead ? 1f : -1f;
            leftBladeSprite.transform.localPosition = new Vector3(-0.22f - 0.08f * swing * leadMul, -0.05f + 0.04f * swing, 0f);
            rightBladeSprite.transform.localPosition = new Vector3(0.22f + 0.08f * swing * leadMul, -0.05f + 0.04f * swing, 0f);
            leftBladeSprite.transform.localRotation = Quaternion.Euler(0f, 0f, 12f - 48f * swing * leadMul);
            rightBladeSprite.transform.localRotation = Quaternion.Euler(0f, 0f, -12f - 48f * swing * leadMul);
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
            var engageRadius = weapon.Range * weapon.EngageMul;
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
            return enemies
                .Where(e => e != null && e.IsAlive)
                .Select(e => new WeaponHit(e, (Vector2)(e.transform.position - player.position)))
                .Where(x => x.Distance <= weapon.Range + x.Enemy.TouchRadius)
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
                foreach (var enemy in enemies.Where(e => e.IsAlive && e.BloodMarked))
                {
                    DealDamage(enemy, (0.8f + blood.Level * 0.25f) * dt, "피의 반사", false);
                }
            }
        }

        void UpdateHungryBlades(MemoryState memory, float dt)
        {
            memory.VisualTimer += dt * (1.3f + memory.Level * 0.18f);
            var bladeCount = Mathf.Clamp(1 + memory.Level, 2, 6);
            for (int i = 0; i < bladeCount; i++)
            {
                var angle = memory.VisualTimer * 160f + i * 360f / bladeCount;
                var pos = player.position + Quaternion.Euler(0f, 0f, angle) * Vector3.right * (HungryBladesRadius + memory.Level * 0.12f);
                SpawnTransientSprite("칼무리", LoadSprite("Assets/_dev/Art/Sprites/Echoes/Kalmuri/spr_kalmuri_orbit_blade_01.png"), pos, Quaternion.Euler(0f, 0f, angle + 30f), 0.16f, new Color(0.65f, 0.95f, 1f, 0.44f), 0.045f);
            }

            memory.TickTimer -= dt;
            if (memory.TickTimer > 0f) return;
            memory.TickTimer = 0.18f;

            var targetLimit = 4;
            var hits = enemies
                .Where(e => e.IsAlive && Vector2.Distance(player.position, e.transform.position) <= HungryBladesRadius + memory.Level * 0.22f)
                .OrderBy(e => Vector2.Distance(player.position, e.transform.position))
                .Take(8)
                .ToList();
            for (int i = 0; i < hits.Count; i++)
            {
                var mul = i < targetLimit ? 1f : 0.55f;
                DealDamage(hits[i], HungryBladesDps * 0.18f * (1f + (memory.Level - 1) * 0.16f) * mul, "굶주린 칼무리", false);
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
                SpawnTransientSprite("혈반", LoadSprite("Assets/_dev/Art/Sprites/Echoes/Blood/spr_blood_mark_01.png"), enemy.transform.position + Vector3.up * 0.2f, Quaternion.identity, 0.33f, new Color(1f, 0.18f, 0.25f, 0.9f), 0.35f);
                if (bloodLevel >= 5 && UnityEngine.Random.value < 0.42f)
                {
                    BloodBloom(enemy, bloodLevel);
                }
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
            var radius = (0.44f + level * 0.045f) * weapon.EchoSizeScale;
            var damage = weapon.Damage * (0.22f + level * 0.045f) * weapon.EchoDamageScale;
            var f = forward.sqrMagnitude > 0.01f ? forward.normalized : lastAim.normalized;
            var baseAngle = Mathf.Atan2(f.y, f.x) * Mathf.Rad2Deg;
            var side = new Vector2(-f.y, f.x).normalized;
            var entries = weapon.VfxProfile != null ? weapon.VfxProfile.kalmuriFollowupSlashes : Array.Empty<SlashVfxEntry>();
            var ringScale = Mathf.Clamp(radius * 0.92f, 0.38f, isHeavy ? 0.92f : 0.72f);
            var ringColor = isHeavy ? new Color(0.92f, 0.98f, 1f, 0.50f) : new Color(0.58f, 0.96f, 1f, 0.40f);
            SpawnTransientSprite("KalmuriEchoRange", MakeRingSprite("KalmuriEchoRange", Color.white, 112), origin, Quaternion.identity, ringScale, ringColor, isHeavy ? 0.22f : 0.16f);

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

        void UpdateEchoUltimate(float dt)
        {
            if (!BloodBladeStormReady) return;

            var weapon = CurrentWeaponSpec();
            ultimatePulseTimer -= dt;
            if (weapon.UltimatePattern == V1UltimatePattern.FewHeavy)
            {
                if (ultimatePulseTimer <= 0f)
                {
                    ultimatePulseTimer = 0.72f;
                    var baseAngle = elapsed * 120f;
                    var ultimateEntries = weapon.VfxProfile != null ? weapon.VfxProfile.ultimateSlashes : Array.Empty<SlashVfxEntry>();
                    for (int i = 0; i < 3; i++)
                    {
                        var angle = baseAngle + i * 120f;
                        var pos = player.position + Quaternion.Euler(0f, 0f, angle) * Vector3.right * 1.75f;
                        var f = ((Vector2)(pos - player.position)).normalized;
                        foreach (var entry in ultimateEntries)
                        {
                            if (entry == null) continue;
                            SpawnSlashEntry(entry, pos, pos, f, angle + 90f, true, i);
                        }
                    }
                    hitstopTimer = Mathf.Max(hitstopTimer, 0.035f);
                    cameraShakeTimer = Mathf.Max(cameraShakeTimer, 0.14f);
                    cameraShakeAmount = Mathf.Max(cameraShakeAmount, 0.065f);
                }

                foreach (var enemy in enemies.Where(e => e.IsAlive && Vector2.Distance(player.position, e.transform.position) < 3.75f).Take(10))
                {
                    enemy.BloodMarked = true;
                    DealDamage(enemy, 42f * dt, "피의 칼폭풍", false);
                }
                return;
            }

            if (ultimatePulseTimer <= 0f)
            {
                ultimatePulseTimer = 0.09f;
                var spin = elapsed * 240f;
                for (int i = 0; i < 6; i++)
                {
                    var angle = spin + i * 60f;
                    var pos = player.position + Quaternion.Euler(0f, 0f, angle) * Vector3.right * 2.35f;
                    SpawnTransientSprite("피의 칼폭풍", LoadSprite("Assets/_dev/Art/Sprites/Ultimates/spr_blood_blade_storm_blade_01.png"), pos, Quaternion.Euler(0f, 0f, angle + 90f), 0.26f, new Color(1f, 0.28f, 0.34f, 0.60f), 0.10f);
                }
            }

            foreach (var enemy in enemies.Where(e => e.IsAlive && Vector2.Distance(player.position, e.transform.position) < 3.20f).Take(12))
            {
                enemy.BloodMarked = true;
                DealDamage(enemy, 18f * dt, "피의 칼폭풍", false);
            }
        }

        void LaunchKalmuriBlade(V1Enemy first)
        {
            var target = enemies.Where(e => e.IsAlive && e != first).OrderBy(e => Vector2.Distance(first.transform.position, e.transform.position)).FirstOrDefault();
            if (target == null) return;
            var go = new GameObject("KalmuriLaunchBlade");
            go.transform.position = player.position;
            go.transform.localScale = Vector3.one * 0.18f;
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = LoadSprite("Assets/_dev/Art/Sprites/Echoes/Kalmuri/spr_kalmuri_launch_blade_01.png") ?? MakeBoxSprite("launch", Color.cyan, 16, 64);
            sr.sortingOrder = 45;
            go.AddComponent<V1Projectile>().Configure(this, target, 8.5f, 16f, "칼무리 각성");
        }

        void BloodBloom(V1Enemy center, int level)
        {
            SpawnTransientSprite("피꽃", LoadSprite("Assets/_dev/Art/Sprites/Echoes/Blood/spr_blood_bloom_01.png"), center.transform.position, Quaternion.identity, 0.72f, new Color(1f, 0.12f, 0.18f, 0.85f), 0.36f);
            SpawnBloodThread(center.transform.position, 0.8f + level * 0.16f, level);
            foreach (var enemy in enemies.Where(e => e.IsAlive && Vector2.Distance(center.transform.position, e.transform.position) < 1.65f))
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
            if (enemies.Count(e => e.IsAlive && e.Kind != V1EnemyKind.Gatekeeper) >= cap) return;

            spawnTimer -= dt;
            if (spawnTimer > 0f) return;
            var profile = SpawnProfile();
            spawnTimer = profile.Interval;

            for (int i = 0; i < profile.PackSize; i++)
            {
                SpawnEnemy(profile.Pick(rng), RandomSpawnPosition());
            }
        }

        SpawnWaveProfile SpawnProfile()
        {
            var firstCycleProgress = Mathf.Clamp01(elapsed / ReviewFirstBossSeconds);
            if (activeMemories.Count < MaxActiveMemories && elapsed > 30f)
            {
                return firstCycleProgress < 0.3f
                    ? new SpawnWaveProfile(0.58f, 2, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne)
                    : new SpawnWaveProfile(0.42f, 3, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.VoidPriest);
            }

            if (firstCycleProgress < 0.24f) return new SpawnWaveProfile(0.58f, elapsed > 38f ? 3 : 2, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye);
            if (firstCycleProgress < 0.70f) return new SpawnWaveProfile(elapsed >= 42f ? 0.48f : 0.82f, elapsed >= 42f ? 3 : 2, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne, V1EnemyKind.VoidPriest);
            return new SpawnWaveProfile(firstCycleProgress >= 0.94f ? 0.86f : 0.72f, firstCycleProgress >= 0.94f ? 1 : 3, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne);
        }

        int EnemyCap()
        {
            if (activeMemories.Count < MaxActiveMemories && elapsed > 30f) return elapsed < 80f ? 18 : 16;
            var progress = Mathf.Clamp01(elapsed / ReviewFirstBossSeconds);
            if (progress >= 0.94f) return 22;
            if (progress >= 0.70f) return 30;
            return 32;
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
            enemies.Add(enemy);
        }

        void SpawnGatekeeper()
        {
            if (enemies.Any(e => e.Kind == V1EnemyKind.Gatekeeper)) return;
            SpawnEnemy(V1EnemyKind.Gatekeeper, player.position + Vector3.up * 4.8f);
            Log("문지기 등장: 첫 망각 관문");
        }

        Vector3 RandomSpawnPosition()
        {
            var angle = UnityEngine.Random.value * Mathf.PI * 2f;
            var radius = 5.9f + UnityEngine.Random.value * 1.7f;
            return player.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius;
        }

        float EnemyHp(V1EnemyKind kind)
        {
            var baseHp = kind switch
            {
                V1EnemyKind.Eroder => 48f,
                V1EnemyKind.DriftingEye => 36f,
                V1EnemyKind.SplitOne => 58f,
                V1EnemyKind.VoidPriest => 74f,
                V1EnemyKind.Gatekeeper => FirstBossHp,
                _ => 48f
            };
            var minutes = elapsed / 60f;
            return baseHp * (1f + minutes * 0.12f) * (1f + (level - 1) * 0.03f);
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
            SpawnHitSpark(enemy.transform.position, hitDir, weaponHit);
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
            DealDamage(enemy, damage, source, false);
        }

        void OnEnemyKilled(V1Enemy enemy)
        {
            kills++;
            SpawnXpOrb(enemy.transform.position, enemy.Kind == V1EnemyKind.Gatekeeper ? 18 : enemy.Score);
            SpawnFloatingText(enemy.transform.position, $"+{enemy.Score}", Color.white);
            if (enemy.Kind == V1EnemyKind.SplitOne)
            {
                for (int i = 0; i < 2; i++) SpawnEnemy(V1EnemyKind.Eroder, enemy.transform.position + (Vector3)(UnityEngine.Random.insideUnitCircle.normalized * 0.45f));
            }
            if (enemy.Kind == V1EnemyKind.Gatekeeper)
            {
                Log("문지기 처치: 망각 발생");
                ForgetHighestMemory();
            }
        }

        void GrantXp(int amount)
        {
            xp += Mathf.RoundToInt(amount * (elapsed < 180f ? 1.95f : 1f));
            while (xp >= nextXp)
            {
                xp -= nextXp;
                level++;
                nextXp = Mathf.RoundToInt(nextXp * (level < 10 ? 1.24f : 1.42f) + (level < 10 ? 3f : 4f));
                pausedForChoice = true;
                Log($"레벨업 Lv.{level}");
                break;
            }
        }

        void UpdateXpCollection(float dt)
        {
            foreach (var orb in xpOrbs.ToArray())
            {
                if (orb == null) continue;
                orb.Tick(player, dt);
            }
        }

        void SpawnXpOrb(Vector3 position, int amount)
        {
            var go = new GameObject("XP_Orb");
            go.transform.position = position;
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = MakeCircleSprite("xp", new Color(0.25f, 0.95f, 1f), 24);
            sr.sortingOrder = 32;
            var orb = go.AddComponent<V1XpOrb>();
            orb.Configure(this, amount);
            xpOrbs.Add(orb);
        }

        public void CollectXpOrb(V1XpOrb orb, int amount)
        {
            xpOrbs.Remove(orb);
            GrantXp(amount);
        }

        void AddMemory(V1MemoryId id, int targetLevel, bool allowUpgrade)
        {
            var existing = activeMemories.FirstOrDefault(m => m.Id == id);
            if (existing != null)
            {
                existing.Level = Mathf.Clamp(allowUpgrade ? Mathf.Max(existing.Level, targetLevel) : existing.Level + 1, 1, MaxMemoryLevel);
                Log($"{MemoryName(id)} 강화 +{existing.Level}");
                return;
            }

            if (activeMemories.Count >= MaxActiveMemories) return;
            var resonance = Mathf.FloorToInt(EchoLevel(id) / 2f);
            activeMemories.Add(new MemoryState(id, Mathf.Clamp(targetLevel + resonance, 1, MaxMemoryLevel)));
            Log(resonance > 0 ? $"공명 획득: {MemoryName(id)} +{targetLevel + resonance}" : $"새 기억: {MemoryName(id)} +{targetLevel}");
        }

        void ForgetHighestMemory()
        {
            if (activeMemories.Count == 0) return;
            var forgotten = activeMemories.OrderByDescending(m => m.Level).ThenByDescending(m => m.RecentOrder).First();
            activeMemories.Remove(forgotten);
            var before = EchoLevel(forgotten.Id);
            var raw = before + forgotten.Level;
            var after = Mathf.Min(MaxEchoLevel, raw);
            echoLevels[forgotten.Id] = after;
            lastForgotten = forgotten.Id;

            overlayTitle = "망각 결과";
            overlayBody = $"사라진 기억: {MemoryName(forgotten.Id)} +{forgotten.Level}\n남은 잔향: {EchoName(forgotten.Id)} +{after}" + (raw > MaxEchoLevel ? $"\n과부하: +{raw - MaxEchoLevel} 폭발" : "") + "\nSpace로 결손 생존에 진입";
            resultOverlay = true;
            SpawnEchoTransformVfx(forgotten.Id);
            Log($"{MemoryName(forgotten.Id)} 망각 -> {EchoName(forgotten.Id)} +{after}");
        }

        void ContinueAfterForgetResult()
        {
            resultOverlay = false;
            refillTimer = fastDebugRun ? 6f : ReviewDeficitSeconds;
            Log($"결손 생존 시작: {Mathf.CeilToInt(refillTimer)}초");
        }

        void SpawnEchoTransformVfx(V1MemoryId id)
        {
            var color = id == V1MemoryId.BloodReflection ? new Color(1f, 0.1f, 0.18f, 0.95f) : new Color(0.58f, 0.95f, 1f, 0.95f);
            for (int i = 0; i < 10; i++)
            {
                var angle = i * 36f;
                var pos = player.position + Quaternion.Euler(0f, 0f, angle) * Vector3.right * (1.2f + i * 0.03f);
                SpawnTransientSprite("망각 변환", null, pos, Quaternion.Euler(0f, 0f, angle), 0.15f + i * 0.02f, color, 0.55f);
            }
        }

        void ReacquireLastForgotten()
        {
            refillOverlay = false;
            resultOverlay = false;
            if (lastForgotten.HasValue)
            {
                AddMemory(lastForgotten.Value, 1, true);
                SpawnResonanceVfx(lastForgotten.Value);
            }
            else
            {
                AddMemory(V1MemoryId.BloodReflection, 1, true);
                SpawnResonanceVfx(V1MemoryId.BloodReflection);
            }
            Log("공명 재획득 완료");
        }

        void SpawnResonanceVfx(V1MemoryId id)
        {
            var color = id == V1MemoryId.BloodReflection ? new Color(1f, 0.16f, 0.24f, 0.92f) : new Color(0.62f, 0.96f, 1f, 0.92f);
            SpawnFloatingText(player.position + Vector3.up * 1.35f, "공명", color);
            for (int i = 0; i < 12; i++)
            {
                var angle = elapsed * 90f + i * 30f;
                var pos = player.position + Quaternion.Euler(0f, 0f, angle) * Vector3.right * (0.85f + i * 0.025f);
                SpawnTransientSprite("공명 표식", null, pos, Quaternion.Euler(0f, 0f, angle), 0.10f + i * 0.01f, color, 0.48f);
            }
        }

        void SetEcho(V1MemoryId id, int levelValue)
        {
            var before = EchoLevel(id);
            echoLevels[id] = Mathf.Clamp(levelValue, 0, MaxEchoLevel);
            if (before < MaxEchoLevel && EchoLevel(id) >= MaxEchoLevel)
            {
                SpawnFloatingText(player.position + Vector3.up * 1.2f, $"{EchoName(id)} 각성", id == V1MemoryId.BloodReflection ? new Color(1f, 0.22f, 0.28f) : new Color(0.62f, 0.96f, 1f));
                SpawnEchoTransformVfx(id);
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
            currentWeaponId = weaponId;
            weaponSelectOverlay = false;
            runStarted = true;
            GameplayPaused = false;
            HitstopActive = false;
            weaponTimer = 0.18f;
            weaponAnimTimer = 0f;
            AddMemory(V1MemoryId.HungryBlades, 1, true);
            var weapon = CurrentWeaponSpec();
            SpawnFloatingText(player.position + Vector3.up * 1.15f, weapon.DisplayName, new Color(0.78f, 0.96f, 1f));
            Log($"런 시작: {weapon.DisplayName} + 굶주린 칼무리 Lv.1");
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

        void DrawWeaponSelectOverlay()
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", panelStyle);
            var width = Mathf.Min(1040f, Screen.width - 80f);
            var height = Mathf.Min(520f, Screen.height - 70f);
            var origin = new Rect(Screen.width * 0.5f - width * 0.5f, Screen.height * 0.5f - height * 0.5f, width, height);
            GUI.Box(origin, "", panelStyle);
            GUI.Label(new Rect(origin.x + 30, origin.y + 28, origin.width - 60, 42), "시작 무기 선택", titleStyle);
            GUI.Label(new Rect(origin.x + 54, origin.y + 78, origin.width - 108, 42), "무기는 자동으로 발동합니다. 잔향은 이 무기 타격 지점에 붙어 후속타가 됩니다.", smallStyle);

            var gap = 28f;
            var cardWidth = (origin.width - 108f - gap) * 0.5f;
            var cardHeight = origin.height - 158f;
            DrawWeaponCard(new Rect(origin.x + 54f, origin.y + 126f, cardWidth, cardHeight), V1WeaponId.DualBlades, "1", "빠른 2연 발도", "작은 칼자국이 여러 번 적 몸에 찍힙니다.\n\n칼무리 잔향: 작고 빠른 혈검 다수\n궁극: 연쇄형 피의 칼폭풍", new Color(0.32f, 0.88f, 1f));
            DrawWeaponCard(new Rect(origin.x + 54f + cardWidth + gap, origin.y + 126f, cardWidth, cardHeight), V1WeaponId.Greatsword, "2", "느린 큰 참격", "적 무리 중심에 큰 검흔과 밀어내기가 생깁니다.\n\n칼무리 잔향: 큰 혈검 소수\n궁극: 강타형 피의 칼폭풍", new Color(0.86f, 0.96f, 1f));
        }

        void DrawWeaponCard(Rect card, V1WeaponId weaponId, string key, string subtitle, string body, Color accent)
        {
            if (GUI.Button(card, "", buttonStyle))
            {
                BeginRun(weaponId);
            }

            var name = weaponId == V1WeaponId.Greatsword ? "장송대검" : "절단쌍검";
            GUI.color = accent;
            GUI.DrawTexture(new Rect(card.x + 18, card.y + 18, card.width - 36, 4), Texture2D.whiteTexture);
            GUI.color = Color.white;
            GUI.Label(new Rect(card.x + 22, card.y + 38, card.width - 44, 36), $"{key}. {name}", titleStyle);
            GUI.Label(new Rect(card.x + 28, card.y + 88, card.width - 56, 28), subtitle, smallStyle);
            GUI.Label(new Rect(card.x + 28, card.y + 132, card.width - 56, 120), body, smallStyle);
            GUI.Label(new Rect(card.x + 28, card.yMax - 46, card.width - 56, 26), "클릭 또는 숫자키로 선택", smallStyle);
        }

        void DrawHud()
        {
            var weapon = CurrentWeaponSpec();
            GUI.Box(new Rect(12, 12, 432, 190), "", panelStyle);
            GUI.Label(new Rect(24, 20, 380, 28), $"LETHE v1 / {PhaseName()} / {Mathf.FloorToInt(elapsed)}s / HP {Mathf.CeilToInt(playerHp)}/{Mathf.CeilToInt(playerMaxHp)}", smallStyle);
            DrawBar(new Rect(24, 47, 396, 12), Mathf.Clamp01(playerHp / playerMaxHp), new Color(0.18f, 0.95f, 0.62f), new Color(0.08f, 0.12f, 0.13f));
            GUI.Label(new Rect(24, 64, 380, 24), $"Lv.{level} XP {xp}/{nextXp} / 처치 {kills} / 무기 {weapon.DisplayName}", smallStyle);
            DrawBar(new Rect(24, 91, 396, 14), Mathf.Clamp01(nextXp <= 0 ? 0f : (float)xp / nextXp), new Color(0.32f, 0.88f, 1f), new Color(0.07f, 0.10f, 0.13f));
            GUI.Label(new Rect(24, 112, 380, 24), $"다음 망각 후보: {ForgetCandidateText()}", smallStyle);
            GUI.Label(new Rect(24, 140, 380, 24), $"잔향: {EchoText()}", smallStyle);
            GUI.Label(new Rect(24, 166, 390, 24), BloodBladeStormReady ? $"궁극: 피의 칼폭풍 활성 / {UltimatePatternText(weapon)}" : $"궁극 준비: 칼무리 {EchoLevel(V1MemoryId.HungryBlades)}/5 + 혈반 {EchoLevel(V1MemoryId.BloodReflection)}/5", smallStyle);

            GUI.Box(new Rect(Screen.width - 372, 12, 360, 196), "", panelStyle);
            GUI.Label(new Rect(Screen.width - 358, 22, 340, 22), "F1 칼무리+5  F2 혈반+5  F3 망각", smallStyle);
            GUI.Label(new Rect(Screen.width - 358, 46, 340, 22), "F4 칼무리잔향+5  F5 혈반잔향+5", smallStyle);
            GUI.Label(new Rect(Screen.width - 358, 70, 340, 22), "F6 문지기  F7 레벨업  F8 M2압축  F9 무기", smallStyle);
            if (GUI.Button(new Rect(Screen.width - 358, 96, 106, 28), "M1 Smoke", buttonStyle)) DebugRunM1Smoke();
            if (GUI.Button(new Rect(Screen.width - 246, 96, 106, 28), "M2 Loop", buttonStyle)) DebugRunM2Smoke();
            if (GUI.Button(new Rect(Screen.width - 134, 96, 106, 28), "Continue", buttonStyle))
            {
                if (resultOverlay) ContinueAfterForgetResult();
                else if (refillOverlay) ReacquireLastForgotten();
            }
            var y = 130;
            foreach (var line in combatLog.TakeLast(3))
            {
                GUI.Label(new Rect(Screen.width - 358, y, 340, 22), line, smallStyle);
                y += 22;
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
            var choices = BuildChoices();
            var cardGap = 18f;
            var cardWidth = (origin.width - 84f - cardGap * 2f) / 3f;
            var cardHeight = origin.height - 138f;
            for (int i = 0; i < choices.Count; i++)
            {
                var choice = choices[i];
                var card = new Rect(origin.x + 42f + i * (cardWidth + cardGap), origin.y + 106f, cardWidth, cardHeight);
                if (GUI.Button(card, "", buttonStyle))
                {
                    choice.Apply();
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
            if (resultOverlay && GUI.Button(new Rect(Screen.width * 0.5f - 110, Screen.height * 0.5f + 76, 220, 38), "결손 생존 시작", buttonStyle))
            {
                ContinueAfterForgetResult();
            }
            if (refillOverlay && GUI.Button(new Rect(Screen.width * 0.5f - 110, Screen.height * 0.5f + 76, 220, 38), "공명 재획득", buttonStyle))
            {
                ReacquireLastForgotten();
            }
        }

        List<Choice> BuildChoices()
        {
            var choices = new List<Choice>();
            if (activeMemories.Count < MaxActiveMemories && !HasMemory(V1MemoryId.BloodReflection))
            {
                choices.Add(new Choice("새 기억", "피의 반사", "타격한 적에게 혈반을 남깁니다.\n\n후반에 혈반 잔향과 피의 칼폭풍으로 이어지는 회복/출혈 축입니다.", () => AddMemory(V1MemoryId.BloodReflection, 1, true)));
            }
            var lowest = activeMemories.OrderBy(m => m.Level).FirstOrDefault(m => m.Level < MaxMemoryLevel);
            if (lowest != null)
            {
                choices.Add(new Choice("기억 강화", MemoryName(lowest.Id), $"현재 Lv.{lowest.Level} -> Lv.{lowest.Level + 1}\n\n효과의 빈도, 개수, 화면 존재감이 함께 올라갑니다.", () => AddMemory(lowest.Id, lowest.Level + 1, true)));
            }
            choices.Add(new Choice("무기 성향", "칼날 가속", "장착 무기 공격 간격 -11%.\n\n잔향 발동 기회도 늘어나 무기 리듬이 더 선명해집니다.", () => { WeaponStat.AttackSpeed += 0.11f; Log("스탯: 칼날 가속"); }));
            choices.Add(new Choice("무기 성향", "검은 물의 힘", "기본공격과 무기 기반 잔향 피해 +14%.\n\n한 번의 베기가 더 선명해집니다.", () => { WeaponStat.DamageMul += 0.14f; Log("스탯: 피해 증가"); }));
            choices.Add(new Choice("생존", "가라앉지 않는 숨", "최대 HP +16, 즉시 회복 +28.\n\n망각 뒤 결손 생존 구간을 버틸 여유를 만듭니다.", () => { playerMaxHp += 16f; playerHp = Mathf.Min(playerMaxHp, playerHp + 28f); Log("스탯: 생존"); }));
            return choices.Take(3).ToList();
        }

        bool HasMemory(V1MemoryId id) => activeMemories.Any(m => m.Id == id);

        float EarlyDamageMul()
        {
            if (elapsed <= 12f) return 0.24f;
            return Mathf.Lerp(0.24f, 1f, Mathf.Clamp01((elapsed - 12f) / (320f - 12f)));
        }

        float StatAttackIntervalMul() => 1f / (1f + WeaponStat.AttackSpeed);

        string PhaseName()
        {
            if (activeMemories.Count < MaxActiveMemories && elapsed > 30f) return "결손 생존";
            var p = Mathf.Clamp01(elapsed / ReviewFirstBossSeconds);
            if (p < 0.24f) return "숨 고르기";
            if (p < 0.70f) return "압박 상승";
            if (p > 0.94f) return "문지기 호흡";
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
            var entries = weapon.VfxProfile != null ? weapon.VfxProfile.weaponHitSlashes : Array.Empty<SlashVfxEntry>();
            if (entries == null || entries.Length == 0) return;

            var f = forward.sqrMagnitude > 0.001f ? forward.normalized : lastAim.normalized;
            var baseAngle = Mathf.Atan2(f.y, f.x) * Mathf.Rad2Deg;
            var hitCenter = (Vector3)hits.Aggregate(Vector2.zero, (sum, hit) => sum + (Vector2)hit.Enemy.transform.position) / hits.Count;
            for (int i = 0; i < hits.Count; i++)
            {
                var primary = i == 0;
                foreach (var entry in entries)
                {
                    if (entry == null || !entry.Matches(primary)) continue;
                    SpawnSlashEntry(entry, hits[i].Enemy.transform.position, hitCenter, f, baseAngle, primary, i);
                }
            }
        }

        void SpawnSlashEntry(SlashVfxEntry entry, Vector3 targetPosition, Vector3 hitCenter, Vector2 forward, float baseAngle, bool primary, int hitIndex)
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
            var sprite = MakeSlashSprite(entry);
            var rotation = entry.spriteShape == SlashSpriteShape.ImpactDiamond || entry.spriteShape == SlashSpriteShape.Circle
                ? Quaternion.identity
                : Quaternion.Euler(0f, 0f, baseAngle + entry.rotationOffsetDegrees * rotationSign);
            SpawnTransientSprite(entry.id, sprite, position, rotation, entry.scale, entry.color, entry.lifetime);
        }

        Sprite MakeSlashSprite(SlashVfxEntry entry)
        {
            return entry.spriteShape switch
            {
                SlashSpriteShape.WideCrescent => MakeWideCrescentSprite(entry.id, Color.white),
                SlashSpriteShape.ImpactDiamond => MakeImpactDiamondSprite(entry.id, Color.white),
                SlashSpriteShape.Circle => null,
                _ => MakeCrescentSlashSprite(entry.id, Color.white, entry.flip)
            };
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

        void DrawBar(Rect rect, float value, Color fill, Color background)
        {
            GUI.color = background;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = fill;
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width * Mathf.Clamp01(value), rect.height), Texture2D.whiteTexture);
            GUI.color = Color.white;
        }

        void SpawnFloatingText(Vector3 pos, string text, Color color)
        {
            var go = new GameObject("FloatText");
            go.transform.position = pos + Vector3.up * 0.35f;
            go.AddComponent<V1FloatingText>().Configure(text, color);
        }

        void SpawnDamageNumber(Vector3 pos, float amount, bool weaponHit, WeaponVfxProfile feedback)
        {
            var go = new GameObject("DamageNumber");
            var jitter = UnityEngine.Random.insideUnitCircle * 0.16f;
            go.transform.position = pos + new Vector3(jitter.x, 0.34f + jitter.y, 0f);
            var color = feedback != null
                ? weaponHit ? feedback.weaponDamageNumberColor : feedback.nonWeaponDamageNumberColor
                : weaponHit ? new Color(1f, 0.96f, 0.72f) : new Color(0.86f, 0.98f, 1f);
            var lifetime = feedback != null
                ? weaponHit ? feedback.weaponDamageNumberLifetime : feedback.nonWeaponDamageNumberLifetime
                : weaponHit ? 0.78f : 0.62f;
            go.AddComponent<V1DamageNumber>().Configure(Mathf.CeilToInt(amount).ToString(), color, lifetime);
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

        void SpawnTransientSprite(string name, Sprite sprite, Vector3 position, Quaternion rotation, float scale, Color color, float lifetime)
        {
            SpawnTransientSpriteScaled(name, sprite, position, rotation, Vector3.one * scale, color, lifetime);
        }

        void SpawnTransientSpriteScaled(string name, Sprite sprite, Vector3 position, Quaternion rotation, Vector3 scale, Color color, float lifetime)
        {
            var go = new GameObject(name);
            go.transform.position = new Vector3(position.x, position.y, -0.05f);
            go.transform.rotation = rotation;
            go.transform.localScale = scale;
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite ?? MakeCircleSprite(name, Color.white, 48);
            sr.color = color;
            sr.sortingOrder = 40;
            go.AddComponent<V1FadingSprite>().Configure(lifetime);
        }

        Sprite EnemySprite(V1EnemyKind kind)
        {
            if (kind == V1EnemyKind.Eroder) return LoadSheetFrame("Assets/_dev/Art/Sprites/Enemies/Chaser/sheet_enemy_chaser_4dir.png", 4, 8, 0, 0) ?? MakeCircleSprite("eroder", EnemyColor(kind), 80);
            return MakeCircleSprite(kind.ToString(), EnemyColor(kind), kind == V1EnemyKind.Gatekeeper ? 128 : 72);
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

        Sprite LoadSprite(string path)
        {
            if (spriteCache.TryGetValue(path, out var cached)) return cached;
#if UNITY_EDITOR
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite != null)
            {
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
#if UNITY_EDITOR
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (texture == null) return null;

            var width = texture.width / columns;
            var height = texture.height / rows;
            var x = Mathf.Clamp(column, 0, columns - 1) * width;
            var y = texture.height - (Mathf.Clamp(rowFromTop, 0, rows - 1) + 1) * height;
            var sprite = Sprite.Create(texture, new Rect(x, y, width, height), new Vector2(0.5f, 0.5f), 100f);
            spriteCache[cacheKey] = sprite;
            return sprite;
#else
            return null;
#endif
        }

        void LoadFont()
        {
#if UNITY_EDITOR
            koreanFont = AssetDatabase.LoadAssetAtPath<Font>("Assets/_dev/Fonts/Pretendard-Regular.otf");
#endif
        }

        void EnsureStyles()
        {
            if (smallStyle != null) return;
            smallStyle = new GUIStyle(GUI.skin.label) { fontSize = 15, normal = { textColor = new Color(0.90f, 0.94f, 0.96f) }, wordWrap = true };
            titleStyle = new GUIStyle(GUI.skin.label) { fontSize = 28, alignment = TextAnchor.MiddleCenter, normal = { textColor = Color.white }, fontStyle = FontStyle.Bold };
            buttonStyle = new GUIStyle(GUI.skin.button) { fontSize = 16, alignment = TextAnchor.MiddleLeft, wordWrap = true };
            panelStyle = new GUIStyle(GUI.skin.box);
            if (koreanFont != null)
            {
                smallStyle.font = koreanFont;
                titleStyle.font = koreanFont;
                buttonStyle.font = koreanFont;
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
            var finalDamage = amount * EarlyDamageMul();
            playerHp -= finalDamage;
            SpawnFloatingText(player.position + Vector3.up * 0.45f, $"-{Mathf.CeilToInt(finalDamage)}", new Color(1f, 0.45f, 0.55f));
            Log($"{source}: 피해 {finalDamage:0.0}");
        }

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
                    KeyCode.F1 => keyboard.f1Key.wasPressedThisFrame,
                    KeyCode.F2 => keyboard.f2Key.wasPressedThisFrame,
                    KeyCode.F3 => keyboard.f3Key.wasPressedThisFrame,
                    KeyCode.F4 => keyboard.f4Key.wasPressedThisFrame,
                    KeyCode.F5 => keyboard.f5Key.wasPressedThisFrame,
                    KeyCode.F6 => keyboard.f6Key.wasPressedThisFrame,
                    KeyCode.F7 => keyboard.f7Key.wasPressedThisFrame,
                    KeyCode.F8 => keyboard.f8Key.wasPressedThisFrame,
                    KeyCode.F9 => keyboard.f9Key.wasPressedThisFrame,
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
                6,
                0.72f,
                1.78f,
                1.05f,
                0.022f,
                0.035f,
                0.16f,
                0.80f,
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
                5,
                0.58f,
                3.75f,
                2.10f,
                0.066f,
                0.088f,
                0.34f,
                1.80f,
                1.60f,
                V1WeaponTargetingMode.DensestArc,
                V1EchoProcStyle.SingleHeavy,
                V1UltimatePattern.FewHeavy,
                null,
                0.065f,
                0.000f);
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

        static class WeaponStat
        {
            public static float AttackSpeed;
            public static float DamageMul;
        }
    }

    public sealed class V1Enemy : MonoBehaviour
    {
        V1GameManager manager;
        Transform player;
        SpriteRenderer sr;
        Vector3 baseScale;
        Vector2 knockVelocity;
        float maxHp;
        float speed;
        float shotTimer;
        float healTimer;
        float hitSquashTimer;

        public V1EnemyKind Kind { get; private set; }
        public float Hp { get; private set; }
        public float TouchDamage { get; private set; }
        public float TouchRadius { get; private set; }
        public bool IsAlive => Hp > 0f;
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
            transform.localScale = Vector3.one * (kind == V1EnemyKind.Gatekeeper ? 1.35f : 0.72f);
            baseScale = transform.localScale;
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
            else
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

        void Heal(float amount)
        {
            Hp = Mathf.Min(maxHp, Hp + amount);
        }

        void RestoreColor()
        {
            if (sr != null) sr.color = BloodMarked ? new Color(1f, 0.28f, 0.32f) : Color.white;
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
            if (dist < 2.4f)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, 5f * dt);
            }
            if (dist < 0.28f)
            {
                manager?.CollectXpOrb(this, amount);
                Destroy(gameObject);
            }
        }
    }

    public sealed class V1FadingSprite : MonoBehaviour
    {
        SpriteRenderer sr;
        float lifetime;
        float age;

        public void Configure(float lifetime)
        {
            this.lifetime = Mathf.Max(0.02f, lifetime);
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
            if (age >= lifetime) Destroy(gameObject);
        }
    }

    public sealed class V1FloatingText : MonoBehaviour
    {
        string text;
        Color color;
        float age;

        public void Configure(string text, Color color)
        {
            this.text = text;
            this.color = color;
        }

        void Update()
        {
            age += Time.deltaTime;
            transform.position += Vector3.up * Time.deltaTime * 0.45f;
            if (age > 0.65f) Destroy(gameObject);
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
        string text;
        Color color;
        float lifetime;
        float age;
        Vector3 velocity;

        public void Configure(string text, Color color, float lifetime)
        {
            this.text = text;
            this.color = color;
            this.lifetime = Mathf.Max(0.25f, lifetime);
            velocity = new Vector3(UnityEngine.Random.Range(-0.12f, 0.12f), 0.72f, 0f);
        }

        void Update()
        {
            if (V1GameManager.GameplayPaused) return;
            age += Time.deltaTime;
            transform.position += velocity * Time.deltaTime;
            velocity = Vector3.Lerp(velocity, Vector3.up * 0.28f, Time.deltaTime * 5f);
            if (age >= lifetime) Destroy(gameObject);
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
