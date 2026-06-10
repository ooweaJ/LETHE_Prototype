using UnityEngine;

namespace Lethe.Dev
{
    public sealed class DualBladesController : WeaponRuntimeBase
    {
        [SerializeField] private WeaponHitEmitter hitEmitter;
        [SerializeField] private Transform hitOrigin;
        [SerializeField] private GameObject debugTarget;
        [SerializeField] private bool autoAttack = true;
        [SerializeField] private float attackInterval = 0.35f;

        private float nextAttackTime;

        public void Bind(HitResolver resolver, GameObject target)
        {
            debugTarget = target;
            if (hitEmitter == null)
            {
                hitEmitter = GetComponent<WeaponHitEmitter>();
            }

            hitEmitter?.Bind(resolver);
        }

        public void SetHitOrigin(Transform origin)
        {
            hitOrigin = origin;
        }

        public void SetAutoAttack(bool enabled)
        {
            autoAttack = enabled;
        }

        private void Update()
        {
            if (!autoAttack || debugTarget == null || Time.time < nextAttackTime)
            {
                return;
            }

            Attack(debugTarget);
            nextAttackTime = Time.time + attackInterval;
        }

        public void Attack(GameObject target)
        {
            if (target == null || hitEmitter == null)
            {
                return;
            }

            var origin = hitOrigin != null ? (Vector2)hitOrigin.position : (Vector2)transform.position;
            var targetPosition = (Vector2)target.transform.position;
            hitEmitter.Emit(gameObject, target, origin, targetPosition - origin);
        }
    }
}
