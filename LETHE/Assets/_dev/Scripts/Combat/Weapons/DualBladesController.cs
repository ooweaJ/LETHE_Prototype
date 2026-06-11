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
        [SerializeField] private float swingDegrees = 42f;
        [SerializeField] private float swingDuration = 0.12f;

        private float nextAttackTime;
        private Coroutine swingRoutine;
        private Quaternion baseLocalRotation;

        private void Awake()
        {
            baseLocalRotation = transform.localRotation;
        }

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
            PlaySwing(targetPosition - origin);
        }

        private void PlaySwing(Vector2 direction)
        {
            if (swingRoutine != null)
            {
                StopCoroutine(swingRoutine);
            }

            swingRoutine = StartCoroutine(SwingRoutine(direction));
        }

        private System.Collections.IEnumerator SwingRoutine(Vector2 direction)
        {
            var facingSign = direction.x < -0.05f ? -1f : 1f;
            var start = baseLocalRotation * Quaternion.Euler(0f, 0f, -swingDegrees * facingSign);
            var end = baseLocalRotation * Quaternion.Euler(0f, 0f, swingDegrees * facingSign);
            var elapsed = 0f;
            while (elapsed < swingDuration)
            {
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / swingDuration);
                transform.localRotation = Quaternion.Slerp(start, end, Mathf.SmoothStep(0f, 1f, t));
                yield return null;
            }

            transform.localRotation = baseLocalRotation;
            swingRoutine = null;
        }
    }
}
