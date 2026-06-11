using UnityEngine;

namespace Lethe.Dev
{
    public sealed class PrototypeEnemy : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 1.35f;
        [SerializeField] private float contactDamage = 3.5f;
        [SerializeField] private float contactCooldown = 0.95f;
        [SerializeField] private float stopDistance = 0.45f;
        [SerializeField] private float knockbackDecay = 11f;
        [SerializeField] private float knockbackContactLock = 0.18f;
        [SerializeField] private float knockbackSnapDistance = 0.035f;
        [SerializeField] private Vector2 arenaHalfExtents = new Vector2(7.6f, 4.6f);
        [SerializeField] private PrototypeSpriteSheetAnimator animator;
        [SerializeField] private PrototypeHealth health;

        private PrototypeGameManager game;
        private Transform target;
        private string roleId = "Enemy_MeleeChaser";
        private string roleName = "침식자";
        private float nextContactAt;
        private Vector3 knockbackVelocity;

        public PrototypeHealth Health => health;
        public string RoleId => roleId;
        public string RoleName => roleName;

        private void Awake()
        {
            animator ??= GetComponentInChildren<PrototypeSpriteSheetAnimator>();
            health ??= GetComponent<PrototypeHealth>();
            if (health != null)
            {
                health.Died += HandleDied;
            }
        }

        private void OnDestroy()
        {
            if (health != null)
            {
                health.Died -= HandleDied;
            }
        }

        private void Update()
        {
            if (target == null || health == null || health.IsDead)
            {
                return;
            }

            var delta = target.position - transform.position;
            delta.z = 0f;
            var distance = delta.magnitude;
            var direction = distance > 0.001f ? (Vector2)(delta / distance) : Vector2.down;

            var recoveringFromHit = knockbackVelocity.sqrMagnitude > 0.25f;
            if (knockbackVelocity.sqrMagnitude > 0.0001f)
            {
                var nextPosition = transform.position + knockbackVelocity * Time.deltaTime;
                transform.position = ClampToArena(nextPosition);
                knockbackVelocity = Vector3.MoveTowards(knockbackVelocity, Vector3.zero, knockbackDecay * Time.deltaTime);
            }

            if (!recoveringFromHit && distance > stopDistance)
            {
                var roleDirection = roleId == "Enemy_RangedEye" && distance < stopDistance + 0.65f ? -direction : direction;
                var nextPosition = transform.position + (Vector3)(roleDirection * moveSpeed * Time.deltaTime);
                transform.position = ClampToArena(nextPosition);
            }

            animator?.SetMotion(direction, distance > stopDistance);

            if (distance <= stopDistance + 0.18f && Time.time >= nextContactAt)
            {
                nextContactAt = Time.time + contactCooldown;
                game?.DamagePlayer(contactDamage);
            }
        }

        public void Initialize(PrototypeGameManager owner, Transform chaseTarget, Texture2D sheet)
        {
            game = owner;
            target = chaseTarget;
            animator ??= GetComponentInChildren<PrototypeSpriteSheetAnimator>();
            animator?.SetSheet(sheet);
            health ??= GetComponent<PrototypeHealth>();
            health?.ResetHealth();
        }

        public void Respawn(Vector3 position)
        {
            transform.position = position;
            knockbackVelocity = Vector3.zero;
            health?.ResetHealth();
        }

        public void ConfigureRole(string id, string displayName, float maxHealth, float speed, float damage, float stoppingDistance)
        {
            roleId = id;
            roleName = displayName;
            moveSpeed = speed;
            contactDamage = damage;
            stopDistance = stoppingDistance;
            health ??= GetComponent<PrototypeHealth>();
            health?.ConfigureMaxHealth(maxHealth);
            name = id;
        }

        public void ApplyKnockback(Vector3 direction, float impulse)
        {
            if (health != null && health.IsDead)
            {
                return;
            }

            direction.z = 0f;
            if (direction.sqrMagnitude <= 0.0001f)
            {
                return;
            }

            knockbackVelocity += direction.normalized * Mathf.Max(0f, impulse);
            knockbackVelocity = Vector3.ClampMagnitude(knockbackVelocity, 2.6f);
            transform.position = ClampToArena(transform.position + direction.normalized * Mathf.Min(0.14f, impulse * knockbackSnapDistance));
            nextContactAt = Mathf.Max(nextContactAt, Time.time + knockbackContactLock);
        }

        private Vector3 ClampToArena(Vector3 position)
        {
            position.x = Mathf.Clamp(position.x, -arenaHalfExtents.x, arenaHalfExtents.x);
            position.y = Mathf.Clamp(position.y, -arenaHalfExtents.y, arenaHalfExtents.y);
            return position;
        }

        private void HandleDied(PrototypeHealth deadHealth, GameObject source)
        {
            game?.RegisterEnemyKilled(this);
        }
    }
}
