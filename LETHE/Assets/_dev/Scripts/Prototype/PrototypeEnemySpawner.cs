using System.Collections.Generic;
using UnityEngine;

namespace Lethe.Dev
{
    public sealed class PrototypeEnemySpawner : MonoBehaviour
    {
        [SerializeField] private int targetCount = 8;
        [SerializeField] private float spawnRadius = 4.2f;
        [SerializeField] private Vector2 arenaHalfExtents = new Vector2(7.4f, 4.4f);
        [SerializeField] private Texture2D enemySheet;

        private readonly List<PrototypeEnemy> enemies = new List<PrototypeEnemy>();
        private PrototypeGameManager game;
        private Transform player;

        public IReadOnlyList<PrototypeEnemy> Enemies => enemies;

        public void Initialize(PrototypeGameManager owner, Transform playerTransform, Texture2D sheet)
        {
            game = owner;
            player = playerTransform;
            enemySheet = sheet;
            EnsureEnemies();
        }

        public PrototypeEnemy FindNearest(Vector3 position, float range)
        {
            PrototypeEnemy best = null;
            var bestSqr = range * range;
            for (var index = 0; index < enemies.Count; index += 1)
            {
                var enemy = enemies[index];
                if (enemy == null || enemy.Health == null || enemy.Health.IsDead || !enemy.gameObject.activeSelf)
                {
                    continue;
                }

                var sqr = (enemy.transform.position - position).sqrMagnitude;
                if (sqr < bestSqr)
                {
                    best = enemy;
                    bestSqr = sqr;
                }
            }

            return best;
        }

        public void FindTargetsInArc(Vector3 origin, Vector3 forward, float range, float arcDegrees, int maxTargets, List<PrototypeEnemy> results)
        {
            results.Clear();
            forward.z = 0f;
            if (forward.sqrMagnitude <= 0.0001f)
            {
                return;
            }

            forward.Normalize();
            var rangeSqr = range * range;
            var minDot = Mathf.Cos(Mathf.Clamp(arcDegrees, 1f, 360f) * 0.5f * Mathf.Deg2Rad);
            for (var index = 0; index < enemies.Count; index += 1)
            {
                var enemy = enemies[index];
                if (enemy == null || enemy.Health == null || enemy.Health.IsDead || !enemy.gameObject.activeSelf)
                {
                    continue;
                }

                var toEnemy = enemy.transform.position - origin;
                toEnemy.z = 0f;
                var sqr = toEnemy.sqrMagnitude;
                if (sqr > rangeSqr || sqr <= 0.0001f)
                {
                    continue;
                }

                if (Vector3.Dot(forward, toEnemy.normalized) < minDot)
                {
                    continue;
                }

                results.Add(enemy);
            }

            results.Sort((left, right) =>
                (left.transform.position - origin).sqrMagnitude.CompareTo((right.transform.position - origin).sqrMagnitude));

            if (maxTargets > 0 && results.Count > maxTargets)
            {
                results.RemoveRange(maxTargets, results.Count - maxTargets);
            }
        }

        public void Respawn(PrototypeEnemy enemy)
        {
            if (enemy == null || player == null)
            {
                return;
            }

            enemy.Respawn(RandomSpawnPosition());
        }

        private void EnsureEnemies()
        {
            for (var index = enemies.Count; index < targetCount; index += 1)
            {
                var enemy = CreateEnemy(index);
                enemies.Add(enemy);
            }
        }

        private PrototypeEnemy CreateEnemy(int index)
        {
            var role = index % 4;
            var roleId = role switch
            {
                1 => "Enemy_RangedEye",
                2 => "Enemy_Splitter",
                3 => "Enemy_EliteGatekeeper",
                _ => "Enemy_MeleeChaser"
            };
            var root = new GameObject($"{roleId}_{index + 1:00}");
            root.transform.SetParent(transform, false);
            root.transform.position = RandomSpawnPosition();

            var visual = new GameObject("Visual");
            visual.transform.SetParent(root.transform, false);
            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sortingOrder = 9;
            var animator = visual.AddComponent<PrototypeSpriteSheetAnimator>();
            animator.SetSheet(enemySheet);

            var health = root.AddComponent<PrototypeHealth>();
            health.ConfigureMaxHealth(28f);

            var enemy = root.AddComponent<PrototypeEnemy>();
            var body = root.AddComponent<CircleCollider2D>();
            body.isTrigger = true;
            body.radius = role == 3 ? 0.46f : 0.34f;

            enemy.Initialize(game, player, enemySheet);
            if (role == 1)
            {
                enemy.ConfigureRole(roleId, "떠도는 눈", 20f, 1.15f, 2.4f, 1.55f);
                renderer.color = new Color(0.72f, 0.86f, 1f, 1f);
            }
            else if (role == 2)
            {
                enemy.ConfigureRole(roleId, "쪼개진 자", 18f, 1.55f, 2.8f, 0.38f);
                renderer.color = new Color(1f, 0.72f, 0.78f, 1f);
            }
            else if (role == 3)
            {
                enemy.ConfigureRole(roleId, "문지기", 74f, 0.82f, 5.6f, 0.58f);
                root.transform.localScale = Vector3.one * 1.24f;
                renderer.color = new Color(0.92f, 0.82f, 1f, 1f);
            }
            else
            {
                enemy.ConfigureRole(roleId, "침식자", 28f, 1.35f, 3.5f, 0.45f);
            }
            return enemy;
        }

        private Vector3 RandomSpawnPosition()
        {
            var center = player != null ? player.position : Vector3.zero;
            var angle = Random.Range(0f, Mathf.PI * 2f);
            var distance = Random.Range(spawnRadius * 0.75f, spawnRadius);
            var position = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * distance;
            position.x = Mathf.Clamp(position.x, -arenaHalfExtents.x, arenaHalfExtents.x);
            position.y = Mathf.Clamp(position.y, -arenaHalfExtents.y, arenaHalfExtents.y);
            return position;
        }
    }
}
