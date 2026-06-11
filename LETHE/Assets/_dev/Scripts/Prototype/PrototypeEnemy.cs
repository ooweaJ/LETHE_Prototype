using UnityEngine;

namespace Lethe.Dev
{
    public sealed class PrototypeEnemy : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 1.35f;
        [SerializeField] private float contactDamage = 3.5f;
        [SerializeField] private float contactCooldown = 0.95f;
        [SerializeField] private float stopDistance = 0.45f;
        [SerializeField] private Vector2 arenaHalfExtents = new Vector2(7.6f, 4.6f);
        [SerializeField] private PrototypeSpriteSheetAnimator animator;
        [SerializeField] private PrototypeHealth health;

        private PrototypeGameManager game;
        private Transform target;
        private float nextContactAt;

        public PrototypeHealth Health => health;

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

            if (distance > stopDistance)
            {
                var nextPosition = transform.position + (Vector3)(direction * moveSpeed * Time.deltaTime);
                nextPosition.x = Mathf.Clamp(nextPosition.x, -arenaHalfExtents.x, arenaHalfExtents.x);
                nextPosition.y = Mathf.Clamp(nextPosition.y, -arenaHalfExtents.y, arenaHalfExtents.y);
                transform.position = nextPosition;
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
            health?.ResetHealth();
        }

        private void HandleDied(PrototypeHealth deadHealth, GameObject source)
        {
            game?.RegisterEnemyKilled(this);
        }
    }
}
