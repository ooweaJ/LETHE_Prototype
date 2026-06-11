using System.Collections;
using UnityEngine;

namespace Lethe.Dev
{
    public sealed class PrototypeWeaponController : MonoBehaviour
    {
        [SerializeField] private float attackInterval = 0.24f;
        [SerializeField] private float attackRange = 1.65f;
        [SerializeField] private float baseDamage = 12f;
        [SerializeField] private float swingDuration = 0.1f;
        [SerializeField] private float swingDegrees = 60f;
        [SerializeField] private Transform visualRoot;

        private PrototypeGameManager game;
        private PrototypeEnemySpawner spawner;
        private float nextAttackAt;
        private Coroutine swingRoutine;
        private Quaternion baseRotation;

        private void Awake()
        {
            baseRotation = transform.localRotation;
        }

        private void Update()
        {
            if (game == null || spawner == null || Time.time < nextAttackAt)
            {
                return;
            }

            var enemy = spawner.FindNearest(transform.position, attackRange);
            if (enemy == null)
            {
                return;
            }

            Attack(enemy);
            nextAttackAt = Time.time + attackInterval;
        }

        public void Initialize(PrototypeGameManager owner, PrototypeEnemySpawner enemySpawner)
        {
            game = owner;
            spawner = enemySpawner;
        }

        private void Attack(PrototypeEnemy enemy)
        {
            var direction = (enemy.transform.position - transform.position).normalized;
            transform.right = direction;
            var killed = enemy.Health.ApplyDamage(baseDamage, game.gameObject);
            game.HandleWeaponHit(enemy, enemy.transform.position, direction, baseDamage, killed);
            SpawnSwingArc(enemy.transform.position, direction);

            if (swingRoutine != null)
            {
                StopCoroutine(swingRoutine);
            }

            swingRoutine = StartCoroutine(Swing(direction));
        }

        private IEnumerator Swing(Vector3 direction)
        {
            var sign = direction.x < -0.05f ? -1f : 1f;
            var start = baseRotation * Quaternion.Euler(0f, 0f, -swingDegrees * sign);
            var end = baseRotation * Quaternion.Euler(0f, 0f, swingDegrees * sign);
            var elapsed = 0f;
            while (elapsed < swingDuration)
            {
                elapsed += Time.deltaTime;
                if (visualRoot != null)
                {
                    visualRoot.localRotation = Quaternion.Slerp(start, end, Mathf.Clamp01(elapsed / swingDuration));
                }
                yield return null;
            }

            if (visualRoot != null)
            {
                visualRoot.localRotation = baseRotation;
            }
            swingRoutine = null;
        }

        private void SpawnSwingArc(Vector3 hitPosition, Vector3 direction)
        {
            game.SpawnLineVfx("WeaponArc", hitPosition - direction * 0.2f, hitPosition + direction * 0.35f, new Color(0.8f, 0.95f, 1f, 0.9f), 0.08f, 0.045f);
        }
    }
}
