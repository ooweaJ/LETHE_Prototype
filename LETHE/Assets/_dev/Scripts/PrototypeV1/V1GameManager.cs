using System;
using System.Collections.Generic;
using System.Linq;
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

    public sealed class V1GameManager : MonoBehaviour
    {
        const float PixelsPerUnit = 40f;
        const float PlayerSpeed = 184f / PixelsPerUnit;
        const float TwinBladeRange = 86f / PixelsPerUnit;
        const float TwinBladeInterval = 0.36f;
        const float TwinBladeDamage = 15f;
        const float TwinBladeArcDeg = 119f;
        const float HungryBladesRadius = 72f / PixelsPerUnit;
        const float HungryBladesDps = 28f;
        const int MaxMemoryLevel = 5;
        const int MaxEchoLevel = 5;
        const int MaxActiveMemories = 3;
        const float FirstBossHp = 2050f;
        const float RunSeconds = 600f;

        readonly List<V1Enemy> enemies = new();
        readonly List<V1XpOrb> xpOrbs = new();
        readonly List<string> combatLog = new();
        readonly List<MemoryState> activeMemories = new();
        readonly Dictionary<V1MemoryId, int> echoLevels = new();
        readonly Dictionary<string, Sprite> spriteCache = new();
        readonly System.Random rng = new(120612);

        Camera mainCamera;
        Transform player;
        Transform weaponAnchor;
        SpriteRenderer playerSprite;
        SpriteRenderer leftBladeSprite;
        SpriteRenderer rightBladeSprite;
        GUIStyle smallStyle;
        GUIStyle titleStyle;
        GUIStyle buttonStyle;
        GUIStyle panelStyle;
        Font koreanFont;

        float playerHp = 210f;
        float playerMaxHp = 210f;
        float elapsed;
        float weaponTimer;
        float spawnTimer;
        float bossTimer = 180f;
        float refillTimer;
        float hitstopTimer;
        int level = 1;
        int xp;
        int nextXp = 5;
        int kills;
        bool pausedForChoice;
        bool resultOverlay;
        bool refillOverlay;
        bool deathOverlay;
        string overlayTitle = "";
        string overlayBody = "";
        V1MemoryId? lastForgotten;

        public bool BloodBladeStormReady => EchoLevel(V1MemoryId.HungryBlades) >= 5 && EchoLevel(V1MemoryId.BloodReflection) >= 5;

        void Awake()
        {
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

            LoadFont();
            CreateArena();
            CreatePlayer();
            AddMemory(V1MemoryId.HungryBlades, 1, true);
            Log("런 시작: 절단쌍검 + 굶주린 칼무리 Lv.1");
        }

        void Update()
        {
            if (deathOverlay)
            {
            if (KeyDown(KeyCode.R))
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                }
                return;
            }

            if (KeyDown(KeyCode.F1)) AddMemory(V1MemoryId.HungryBlades, 5, true);
            if (KeyDown(KeyCode.F2)) AddMemory(V1MemoryId.BloodReflection, 5, true);
            if (KeyDown(KeyCode.F3)) ForgetHighestMemory();
            if (KeyDown(KeyCode.F4)) SetEcho(V1MemoryId.HungryBlades, 5);
            if (KeyDown(KeyCode.F5)) SetEcho(V1MemoryId.BloodReflection, 5);
            if (KeyDown(KeyCode.F6)) SpawnGatekeeper();
            if (KeyDown(KeyCode.F7)) GrantXp(nextXp);
            if (KeyDown(KeyCode.Space) && resultOverlay)
            {
                resultOverlay = false;
                refillTimer = 54f;
            }
            if (KeyDown(KeyCode.Space) && refillOverlay)
            {
                ReacquireLastForgotten();
            }

            if (pausedForChoice || resultOverlay || refillOverlay)
            {
                return;
            }

            var dt = Time.deltaTime;
            if (hitstopTimer > 0f)
            {
                hitstopTimer -= dt;
                return;
            }

            elapsed += dt;
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
                    overlayBody = "결손 생존 종료. Space로 잃었던 기억을 공명 재획득합니다.";
                }
            }

            UpdatePlayer(dt);
            UpdateCamera();
            UpdateWeapon(dt);
            UpdateActiveMemories(dt);
            UpdateEchoUltimate(dt);
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

        void OnGUI()
        {
            EnsureStyles();
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

            leftBladeSprite = CreateWeaponSprite("LeftBlade", "Assets/_dev/Art/Sprites/Weapons/spr_weapon_dual_blade_left_01.png", new Vector3(-0.22f, -0.05f, 0f));
            rightBladeSprite = CreateWeaponSprite("RightBlade", "Assets/_dev/Art/Sprites/Weapons/spr_weapon_dual_blade_right_01.png", new Vector3(0.22f, -0.05f, 0f));
        }

        SpriteRenderer CreateWeaponSprite(string name, string path, Vector3 localPos)
        {
            var go = new GameObject(name);
            go.transform.SetParent(weaponAnchor);
            go.transform.localPosition = localPos;
            go.transform.localScale = Vector3.one * 0.36f;
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = LoadSprite(path) ?? MakeBoxSprite(name, new Color(0.86f, 0.93f, 1f), 18, 72);
            sr.sortingOrder = 30;
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
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, target, 0.15f);
        }

        void UpdateWeapon(float dt)
        {
            weaponTimer -= dt;
            if (weaponTimer > 0f) return;
            weaponTimer = TwinBladeInterval * StatAttackIntervalMul();

            var forward = weaponAnchor.up;
            SpawnSwingArc(forward);
            var hits = enemies
                .Where(e => e.IsAlive)
                .Select(e => new { Enemy = e, Dir = (Vector2)(e.transform.position - player.position) })
                .Where(x => x.Dir.magnitude <= TwinBladeRange + x.Enemy.TouchRadius)
                .Where(x => Vector2.Angle(forward, x.Dir.normalized) <= TwinBladeArcDeg * 0.5f)
                .OrderBy(x => x.Dir.magnitude)
                .Take(7)
                .ToList();

            foreach (var hit in hits)
            {
                DealDamage(hit.Enemy, TwinBladeDamage, "절단쌍검", true);
                TriggerWeaponEchoes(hit.Enemy, forward);
            }
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
                    enemy.TakeDamage((0.8f + blood.Level * 0.25f) * dt, "피의 반사", false);
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
                SpawnTransientSprite("칼무리", LoadSprite("Assets/_dev/Art/Sprites/Echoes/Kalmuri/spr_kalmuri_orbit_blade_01.png"), pos, Quaternion.Euler(0f, 0f, angle + 30f), 0.22f, new Color(0.65f, 0.95f, 1f, 0.65f), 0.08f);
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

        void TriggerWeaponEchoes(V1Enemy enemy, Vector2 forward)
        {
            var kalmuriLevel = EchoLevel(V1MemoryId.HungryBlades);
            if (kalmuriLevel > 0)
            {
                var chance = kalmuriLevel >= 2 ? 1f : 0.30f;
                if (UnityEngine.Random.value <= chance)
                {
                    var damage = TwinBladeDamage * (0.40f + kalmuriLevel * 0.08f);
                    var pos = enemy.transform.position + (Vector3)(forward.normalized * 0.18f);
                    SpawnTransientSprite("칼자국 잔향", LoadSprite("Assets/_dev/Art/Sprites/Echoes/Kalmuri/spr_kalmuri_echo_slash_01.png"), pos, Quaternion.Euler(0f, 0f, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg), 0.62f + kalmuriLevel * 0.08f, new Color(0.75f, 0.98f, 1f, 0.9f), 0.24f);
                    DealDamage(enemy, damage, "칼무리 잔향", true);
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

        void UpdateEchoUltimate(float dt)
        {
            if (!BloodBladeStormReady) return;
            var spin = elapsed * 180f;
            for (int i = 0; i < 5; i++)
            {
                var angle = spin + i * 72f;
                var pos = player.position + Quaternion.Euler(0f, 0f, angle) * Vector3.right * 2.6f;
                SpawnTransientSprite("피의 칼폭풍", LoadSprite("Assets/_dev/Art/Sprites/Ultimates/spr_blood_blade_storm_blade_01.png"), pos, Quaternion.Euler(0f, 0f, angle + 90f), 0.28f, new Color(1f, 0.28f, 0.34f, 0.55f), 0.08f);
            }

            foreach (var enemy in enemies.Where(e => e.IsAlive && Vector2.Distance(player.position, e.transform.position) < 3f))
            {
                enemy.BloodMarked = true;
                DealDamage(enemy, 9f * dt, "피의 칼폭풍", false);
            }
        }

        void LaunchKalmuriBlade(V1Enemy first)
        {
            var target = enemies.Where(e => e.IsAlive && e != first).OrderBy(e => Vector2.Distance(first.transform.position, e.transform.position)).FirstOrDefault();
            if (target == null) return;
            var go = new GameObject("KalmuriLaunchBlade");
            go.transform.position = player.position;
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = LoadSprite("Assets/_dev/Art/Sprites/Echoes/Kalmuri/spr_kalmuri_launch_blade_01.png") ?? MakeBoxSprite("launch", Color.cyan, 16, 64);
            sr.sortingOrder = 45;
            go.AddComponent<V1Projectile>().Configure(this, target, 8.5f, 16f, "칼무리 각성");
        }

        void BloodBloom(V1Enemy center, int level)
        {
            SpawnTransientSprite("피꽃", LoadSprite("Assets/_dev/Art/Sprites/Echoes/Blood/spr_blood_bloom_01.png"), center.transform.position, Quaternion.identity, 0.72f, new Color(1f, 0.12f, 0.18f, 0.85f), 0.36f);
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
            var firstCycleProgress = Mathf.Clamp01(elapsed / 180f);
            if (activeMemories.Count < MaxActiveMemories && elapsed > 30f)
            {
                return firstCycleProgress < 0.3f
                    ? new SpawnWaveProfile(0.72f, 2, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne)
                    : new SpawnWaveProfile(0.50f, 3, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.VoidPriest);
            }

            if (firstCycleProgress < 0.24f) return new SpawnWaveProfile(0.72f, elapsed > 70f ? 3 : 2, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye);
            if (firstCycleProgress < 0.70f) return new SpawnWaveProfile(elapsed >= 126f ? 0.54f : 1.05f, elapsed >= 126f ? 3 : 2, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne, V1EnemyKind.VoidPriest);
            return new SpawnWaveProfile(firstCycleProgress >= 0.94f ? 1.30f : 1.08f, firstCycleProgress >= 0.94f ? 1 : 2, V1EnemyKind.Eroder, V1EnemyKind.Eroder, V1EnemyKind.DriftingEye, V1EnemyKind.SplitOne);
        }

        int EnemyCap()
        {
            if (activeMemories.Count < MaxActiveMemories && elapsed > 30f) return elapsed < 80f ? 16 : 14;
            var progress = Mathf.Clamp01(elapsed / 180f);
            if (progress >= 0.94f) return 22;
            if (progress >= 0.70f) return 32;
            return 34;
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
            var radius = 7.8f + UnityEngine.Random.value * 2.2f;
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

        void DealDamage(V1Enemy enemy, float amount, string source, bool weaponHit)
        {
            if (enemy == null || !enemy.IsAlive) return;
            var before = enemy.Hp;
            enemy.TakeDamage(amount, source, weaponHit);
            if (before > 0f && !enemy.IsAlive)
            {
                OnEnemyKilled(enemy);
            }
        }

        public void ProjectileHit(V1Enemy enemy, float damage, string source)
        {
            DealDamage(enemy, damage, source, false);
        }

        void OnEnemyKilled(V1Enemy enemy)
        {
            kills++;
            GrantXp(enemy.Kind == V1EnemyKind.Gatekeeper ? 18 : enemy.Score);
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
            foreach (var orb in xpOrbs)
            {
                if (orb == null) continue;
                orb.Tick(player, dt);
            }
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
            if (lastForgotten.HasValue)
            {
                AddMemory(lastForgotten.Value, 1, true);
            }
            else
            {
                AddMemory(V1MemoryId.BloodReflection, 1, true);
            }
        }

        void SetEcho(V1MemoryId id, int levelValue)
        {
            echoLevels[id] = Mathf.Clamp(levelValue, 0, MaxEchoLevel);
            Log($"{EchoName(id)} 디버그 +{EchoLevel(id)}");
        }

        int EchoLevel(V1MemoryId id) => echoLevels.TryGetValue(id, out var levelValue) ? levelValue : 0;

        void DrawHud()
        {
            GUI.Box(new Rect(12, 12, 420, 158), "", panelStyle);
            GUI.Label(new Rect(24, 20, 380, 28), $"LETHE v1 / {PhaseName()} / {Mathf.FloorToInt(elapsed)}s / HP {Mathf.CeilToInt(playerHp)}/{Mathf.CeilToInt(playerMaxHp)}", smallStyle);
            GUI.Label(new Rect(24, 48, 380, 24), $"Lv.{level} XP {xp}/{nextXp} / 처치 {kills} / 활성 기억 {activeMemories.Count}/{MaxActiveMemories}", smallStyle);
            GUI.Label(new Rect(24, 76, 380, 24), $"다음 망각 후보: {ForgetCandidateText()}", smallStyle);
            GUI.Label(new Rect(24, 104, 380, 24), $"잔향: {EchoText()}", smallStyle);
            GUI.Label(new Rect(24, 132, 380, 24), BloodBladeStormReady ? "궁극: 피의 칼폭풍 활성" : "궁극: 칼무리+5 / 혈반+5 필요", smallStyle);

            GUI.Box(new Rect(Screen.width - 372, 12, 360, 168), "", panelStyle);
            GUI.Label(new Rect(Screen.width - 358, 22, 340, 22), "F1 칼무리+5  F2 혈반+5  F3 망각", smallStyle);
            GUI.Label(new Rect(Screen.width - 358, 46, 340, 22), "F4 칼무리잔향+5  F5 혈반잔향+5", smallStyle);
            GUI.Label(new Rect(Screen.width - 358, 70, 340, 22), "F6 문지기  F7 레벨업  Space 진행", smallStyle);
            var y = 98;
            foreach (var line in combatLog.TakeLast(3))
            {
                GUI.Label(new Rect(Screen.width - 358, y, 340, 22), line, smallStyle);
                y += 22;
            }
        }

        void DrawLevelUpOverlay()
        {
            GUI.Box(new Rect(Screen.width * 0.5f - 260, Screen.height * 0.5f - 150, 520, 300), "", panelStyle);
            GUI.Label(new Rect(Screen.width * 0.5f - 220, Screen.height * 0.5f - 126, 440, 36), "레벨업", titleStyle);
            var choices = BuildChoices();
            for (int i = 0; i < choices.Count; i++)
            {
                var choice = choices[i];
                if (GUI.Button(new Rect(Screen.width * 0.5f - 220, Screen.height * 0.5f - 78 + i * 74, 440, 58), choice.Label, buttonStyle))
                {
                    choice.Apply();
                    pausedForChoice = false;
                }
            }
        }

        void DrawCenterOverlay()
        {
            GUI.Box(new Rect(Screen.width * 0.5f - 280, Screen.height * 0.5f - 140, 560, 280), "", panelStyle);
            GUI.Label(new Rect(Screen.width * 0.5f - 230, Screen.height * 0.5f - 108, 460, 40), overlayTitle, titleStyle);
            GUI.Label(new Rect(Screen.width * 0.5f - 230, Screen.height * 0.5f - 52, 460, 140), overlayBody, smallStyle);
        }

        List<Choice> BuildChoices()
        {
            var choices = new List<Choice>();
            if (activeMemories.Count < MaxActiveMemories && !HasMemory(V1MemoryId.BloodReflection))
            {
                choices.Add(new Choice("새 기억: 피의 반사\n타격한 적에게 혈반을 남기고, 잔향/궁극의 혈류 축을 엽니다.", () => AddMemory(V1MemoryId.BloodReflection, 1, true)));
            }
            var lowest = activeMemories.OrderBy(m => m.Level).FirstOrDefault(m => m.Level < MaxMemoryLevel);
            if (lowest != null)
            {
                choices.Add(new Choice($"기억 강화: {MemoryName(lowest.Id)}\nLv.{lowest.Level} -> Lv.{lowest.Level + 1}", () => AddMemory(lowest.Id, lowest.Level + 1, true)));
            }
            choices.Add(new Choice("칼날 가속\n공격 간격 -11%, 기억 쿨감 -4%", () => { TwinBladeStat.AttackSpeed += 0.11f; Log("스탯: 칼날 가속"); }));
            choices.Add(new Choice("검은 물의 힘\n피해 +14%", () => { TwinBladeStat.DamageMul += 0.14f; Log("스탯: 피해 증가"); }));
            choices.Add(new Choice("가라앉지 않는 숨\n최대 HP +16, 즉시 회복 +28", () => { playerMaxHp += 16f; playerHp = Mathf.Min(playerMaxHp, playerHp + 28f); Log("스탯: 생존"); }));
            return choices.Take(3).ToList();
        }

        bool HasMemory(V1MemoryId id) => activeMemories.Any(m => m.Id == id);

        float EarlyDamageMul()
        {
            if (elapsed <= 12f) return 0.24f;
            return Mathf.Lerp(0.24f, 1f, Mathf.Clamp01((elapsed - 12f) / (320f - 12f)));
        }

        float StatAttackIntervalMul() => 1f / (1f + TwinBladeStat.AttackSpeed);

        string PhaseName()
        {
            if (activeMemories.Count < MaxActiveMemories && elapsed > 30f) return "결손 생존";
            var p = Mathf.Clamp01(elapsed / 180f);
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
            var parts = echoLevels.Where(kv => kv.Value > 0).Select(kv => $"{EchoName(kv.Key)} +{kv.Value}");
            var text = string.Join(" / ", parts);
            return string.IsNullOrEmpty(text) ? "없음" : text;
        }

        void SpawnSwingArc(Vector2 forward)
        {
            var pos = player.position + (Vector3)(forward.normalized * 0.8f);
            var rot = Quaternion.Euler(0f, 0f, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
            SpawnTransientSprite("쌍검 공격", MakeArcSprite("arc", new Color(0.68f, 0.94f, 1f, 0.9f)), pos, rot, 1.35f, new Color(0.72f, 0.95f, 1f, 0.9f), 0.12f);
        }

        void SpawnFloatingText(Vector3 pos, string text, Color color)
        {
            var go = new GameObject("FloatText");
            go.transform.position = pos + Vector3.up * 0.35f;
            go.AddComponent<V1FloatingText>().Configure(text, color);
        }

        void SpawnTransientSprite(string name, Sprite sprite, Vector3 position, Quaternion rotation, float scale, Color color, float lifetime)
        {
            var go = new GameObject(name);
            go.transform.position = new Vector3(position.x, position.y, -0.05f);
            go.transform.rotation = rotation;
            go.transform.localScale = Vector3.one * scale;
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
                    KeyCode.F1 => keyboard.f1Key.wasPressedThisFrame,
                    KeyCode.F2 => keyboard.f2Key.wasPressedThisFrame,
                    KeyCode.F3 => keyboard.f3Key.wasPressedThisFrame,
                    KeyCode.F4 => keyboard.f4Key.wasPressedThisFrame,
                    KeyCode.F5 => keyboard.f5Key.wasPressedThisFrame,
                    KeyCode.F6 => keyboard.f6Key.wasPressedThisFrame,
                    KeyCode.F7 => keyboard.f7Key.wasPressedThisFrame,
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
            public readonly string Label;
            public readonly Action Apply;
            public Choice(string label, Action apply)
            {
                Label = label;
                Apply = apply;
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

        static class TwinBladeStat
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
        float maxHp;
        float speed;
        float shotTimer;
        float healTimer;

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
        }

        void Update()
        {
            if (!IsAlive || player == null) return;
            var dt = Time.deltaTime;
            var toPlayer = (Vector2)(player.position - transform.position);
            var dist = toPlayer.magnitude;
            var dir = dist > 0.01f ? toPlayer / dist : Vector2.zero;

            if (Kind == V1EnemyKind.DriftingEye && dist < 4f)
            {
                transform.position -= (Vector3)(dir * speed * 0.45f * dt);
                shotTimer -= dt;
                if (shotTimer <= 0f)
                {
                    shotTimer = 2.2f;
                    var go = new GameObject("EyeShot");
                    go.transform.position = transform.position;
                    go.AddComponent<V1EnemyShot>().Configure(player, 4.8f, TouchDamage * 2.1f);
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

        public void TakeDamage(float amount, string source, bool weaponHit)
        {
            Hp -= amount;
            if (sr != null)
            {
                sr.color = BloodMarked ? new Color(1f, 0.35f, 0.38f) : Color.white;
                CancelInvoke(nameof(RestoreColor));
                Invoke(nameof(RestoreColor), 0.045f);
            }
            if (Hp <= 0f)
            {
                Destroy(gameObject);
            }
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
        Transform target;
        Vector3 velocity;
        float damage;
        float life = 3f;

        public void Configure(Transform target, float speed, float damage)
        {
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
            life -= Time.deltaTime;
            transform.position += velocity * Time.deltaTime;
            if (target != null && Vector2.Distance(transform.position, target.position) < 0.28f)
            {
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
        public void Tick(Transform player, float dt)
        {
            if (player == null) return;
            var dist = Vector2.Distance(transform.position, player.position);
            if (dist < 2.4f)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, 5f * dt);
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
