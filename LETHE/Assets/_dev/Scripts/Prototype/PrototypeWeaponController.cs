using System.Collections;
using UnityEngine;

namespace Lethe.Dev
{
    public enum PrototypeWeaponMode
    {
        DualBlades,
        Greatsword
    }

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
        [SerializeField] private PrototypeWeaponMode currentMode = PrototypeWeaponMode.DualBlades;

        private readonly System.Collections.Generic.List<PrototypeEnemy> hitBuffer = new System.Collections.Generic.List<PrototypeEnemy>(8);
        private PrototypeGameManager game;
        private PrototypeEnemySpawner spawner;
        private float nextAttackAt;
        private Coroutine swingRoutine;
        private Quaternion baseRotation;
        private Vector3 baseVisualScale = Vector3.one;
        private WeaponStats dualBladesStats;
        private WeaponStats greatswordStats;

        public PrototypeWeaponMode CurrentMode => currentMode;
        public string CurrentWeaponId => currentMode == PrototypeWeaponMode.Greatsword ? "Weapon_Greatsword" : "Weapon_DualBlades";
        public string CurrentWeaponName => currentMode == PrototypeWeaponMode.Greatsword ? "장송대검" : "절단쌍검";

        private void Awake()
        {
            baseRotation = transform.localRotation;
            if (visualRoot != null)
            {
                baseVisualScale = visualRoot.localScale;
            }

            CaptureStats();
            ApplyModeStats(currentMode);
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
            ApplyModeStats(currentMode);
        }

        public void ToggleWeapon()
        {
            SetWeaponMode(currentMode == PrototypeWeaponMode.DualBlades ? PrototypeWeaponMode.Greatsword : PrototypeWeaponMode.DualBlades);
        }

        public void SetWeaponMode(PrototypeWeaponMode mode)
        {
            currentMode = mode;
            ApplyModeStats(mode);
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
                game.HandleWeaponHit(target, target.transform.position, targetDirection, damage, killed, CurrentWeaponId);
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
            var color = currentMode == PrototypeWeaponMode.Greatsword
                ? new Color(0.86f, 0.82f, 1f, 0.72f)
                : new Color(0.78f, 0.96f, 1f, 0.62f);
            var sideColor = currentMode == PrototypeWeaponMode.Greatsword
                ? new Color(0.62f, 0.55f, 1f, 0.48f)
                : new Color(0.52f, 0.92f, 1f, 0.42f);
            game.SpawnLineVfx(currentMode == PrototypeWeaponMode.Greatsword ? "GreatswordCleaveCore" : "WeaponCleaveCore", near, rangeEnd, color, currentMode == PrototypeWeaponMode.Greatsword ? 0.14f : 0.09f, currentMode == PrototypeWeaponMode.Greatsword ? 0.075f : 0.045f);
            game.SpawnLineVfx("WeaponCleaveLeft", origin + left * 0.72f, rangeEnd, sideColor, 0.08f, 0.026f);
            game.SpawnLineVfx("WeaponCleaveRight", origin + right * 0.72f, rangeEnd, sideColor, 0.08f, 0.026f);
        }

        private void SpawnContactSlash(Vector3 hitPosition, Vector3 direction, bool primary)
        {
            var cross = Quaternion.Euler(0f, 0f, primary ? 82f : 64f) * direction;
            var heavy = currentMode == PrototypeWeaponMode.Greatsword;
            game.SpawnLineVfx(primary ? (heavy ? "GreatswordPrimaryHit" : "WeaponPrimaryHit") : "WeaponCleaveHit",
                hitPosition - cross * 0.32f,
                hitPosition + cross * (heavy ? 0.58f : 0.38f),
                primary ? (heavy ? new Color(0.9f, 0.82f, 1f, 0.86f) : new Color(0.95f, 1f, 1f, 0.78f)) : new Color(0.6f, 0.92f, 1f, 0.48f),
                primary ? (heavy ? 0.18f : 0.12f) : 0.09f,
                primary ? (heavy ? 0.07f : 0.045f) : 0.026f);
        }

        private void CaptureStats()
        {
            dualBladesStats = new WeaponStats(0.28f, 2.15f, 5.8f, 0.13f, 86f, 108f, 4, 0.48f, 1.45f, 0.85f);
            greatswordStats = new WeaponStats(0.72f, 2.95f, 16.5f, 0.24f, 124f, 132f, 6, 0.62f, 3.1f, 1.9f);
        }

        private void ApplyModeStats(PrototypeWeaponMode mode)
        {
            var stats = mode == PrototypeWeaponMode.Greatsword ? greatswordStats : dualBladesStats;
            attackInterval = stats.AttackInterval;
            attackRange = stats.AttackRange;
            baseDamage = stats.BaseDamage;
            swingDuration = stats.SwingDuration;
            swingDegrees = stats.SwingDegrees;
            attackArcDegrees = stats.AttackArcDegrees;
            maxTargetsPerSwing = stats.MaxTargetsPerSwing;
            secondaryDamageMultiplier = stats.SecondaryDamageMultiplier;
            primaryKnockback = stats.PrimaryKnockback;
            secondaryKnockback = stats.SecondaryKnockback;
            if (visualRoot != null)
            {
                visualRoot.localScale = baseVisualScale * (mode == PrototypeWeaponMode.Greatsword ? 1.42f : 1f);
            }
        }

        private readonly struct WeaponStats
        {
            public WeaponStats(float attackInterval, float attackRange, float baseDamage, float swingDuration, float swingDegrees, float attackArcDegrees, int maxTargetsPerSwing, float secondaryDamageMultiplier, float primaryKnockback, float secondaryKnockback)
            {
                AttackInterval = attackInterval;
                AttackRange = attackRange;
                BaseDamage = baseDamage;
                SwingDuration = swingDuration;
                SwingDegrees = swingDegrees;
                AttackArcDegrees = attackArcDegrees;
                MaxTargetsPerSwing = maxTargetsPerSwing;
                SecondaryDamageMultiplier = secondaryDamageMultiplier;
                PrimaryKnockback = primaryKnockback;
                SecondaryKnockback = secondaryKnockback;
            }

            public float AttackInterval { get; }
            public float AttackRange { get; }
            public float BaseDamage { get; }
            public float SwingDuration { get; }
            public float SwingDegrees { get; }
            public float AttackArcDegrees { get; }
            public int MaxTargetsPerSwing { get; }
            public float SecondaryDamageMultiplier { get; }
            public float PrimaryKnockback { get; }
            public float SecondaryKnockback { get; }
        }
    }
}
