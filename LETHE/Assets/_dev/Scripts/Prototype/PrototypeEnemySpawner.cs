using System.Collections.Generic;
using UnityEngine;

namespace Lethe.Dev
{
    public sealed class PrototypeEnemySpawner : MonoBehaviour
    {
        [SerializeField] private int targetCount = 5;
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
            var root = new GameObject($"Enemy_MeleeChaser_{index + 1:00}");
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
            body.radius = 0.34f;

            enemy.Initialize(game, player, enemySheet);
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
