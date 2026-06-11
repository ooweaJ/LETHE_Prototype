using System.Collections;
using UnityEngine;

namespace Lethe.Dev
{
    public sealed class PrototypeWeaponController : MonoBehaviour
    {
        [SerializeField] private float attackInterval = 0.28f;
        [SerializeField] private float attackRange = 2.15f;
        [SerializeField] private float baseDamage = 5.8f;
        [SerializeField] private float swingDuration = 0.13f;
        [SerializeField] private float swingDegrees = 86f;
        [SerializeField] private float attackArcDegrees = 108f;
        [SerializeField] private int maxTargetsPerSwing = 4;
        [SerializeField] private float secondaryDamageMultiplier = 0.48f;
        [SerializeField] private float primaryKnockback = 1.45f;
        [SerializeField] private float secondaryKnockback = 0.85f;
        [SerializeField] private Transform visualRoot;

        private readonly System.Collections.Generic.List<PrototypeEnemy> hitBuffer = new System.Collections.Generic.List<PrototypeEnemy>(8);
        private PrototypeGameManager game;
        private PrototypeEnemySpawner spawner;
        private float nextAttackAt;
        private Coroutine swingRoutine;
        private Quaternion baseRotation;
        private Vector3 baseVisualScale = Vector3.one;

        private void Awake()
        {
            baseRotation = transform.localRotation;
            if (visualRoot != null)
            {
                baseVisualScale = visualRoot.localScale;
            }
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
            var origin = transform.position;
            var direction = enemy.transform.position - origin;
            direction.z = 0f;
            direction = direction.sqrMagnitude > 0.0001f ? direction.normalized : transform.right;
            transform.right = direction;
            spawner.FindTargetsInArc(origin, direction, attackRange, attackArcDegrees, maxTargetsPerSwing, hitBuffer);
            if (hitBuffer.Count == 0)
            {
                hitBuffer.Add(enemy);
            }

            for (var index = 0; index < hitBuffer.Count; index += 1)
            {
                var target = hitBuffer[index];
                if (target == null || target.Health == null || target.Health.IsDead)
                {
                    continue;
                }

                var targetDirection = target.transform.position - origin;
                targetDirection.z = 0f;
                targetDirection = targetDirection.sqrMagnitude > 0.0001f ? targetDirection.normalized : direction;
                var damage = index == 0 ? baseDamage : baseDamage * secondaryDamageMultiplier;
                var killed = target.Health.ApplyDamage(damage, game.gameObject);
                target.ApplyKnockback(targetDirection, index == 0 ? primaryKnockback : secondaryKnockback);
                game.HandleWeaponHit(target, target.transform.position, targetDirection, damage, killed);
                SpawnContactSlash(target.transform.position, targetDirection, index == 0);
            }

            SpawnSwingArc(origin, direction);

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
                    visualRoot.localScale = baseVisualScale * Mathf.Lerp(1.18f, 0.96f, Mathf.Clamp01(elapsed / swingDuration));
                }
                yield return null;
            }

            if (visualRoot != null)
            {
                visualRoot.localRotation = baseRotation;
                visualRoot.localScale = baseVisualScale;
            }
            swingRoutine = null;
        }

        private void SpawnSwingArc(Vector3 origin, Vector3 direction)
        {
            var rangeEnd = origin + direction * attackRange;
            var left = Quaternion.Euler(0f, 0f, attackArcDegrees * 0.5f) * direction;
            var right = Quaternion.Euler(0f, 0f, -attackArcDegrees * 0.5f) * direction;
            var near = origin + direction * 0.35f;
            game.SpawnLineVfx("WeaponCleaveCore", near, rangeEnd, new Color(0.78f, 0.96f, 1f, 0.62f), 0.09f, 0.045f);
            game.SpawnLineVfx("WeaponCleaveLeft", origin + left * 0.72f, rangeEnd, new Color(0.52f, 0.92f, 1f, 0.42f), 0.08f, 0.026f);
            game.SpawnLineVfx("WeaponCleaveRight", origin + right * 0.72f, rangeEnd, new Color(0.52f, 0.92f, 1f, 0.42f), 0.08f, 0.026f);
        }

        private void SpawnContactSlash(Vector3 hitPosition, Vector3 direction, bool primary)
        {
            var cross = Quaternion.Euler(0f, 0f, primary ? 82f : 64f) * direction;
            game.SpawnLineVfx(primary ? "WeaponPrimaryHit" : "WeaponCleaveHit",
                hitPosition - cross * 0.32f,
                hitPosition + cross * 0.38f,
                primary ? new Color(0.95f, 1f, 1f, 0.78f) : new Color(0.6f, 0.92f, 1f, 0.48f),
                primary ? 0.12f : 0.09f,
                primary ? 0.045f : 0.026f);
        }
    }
}
