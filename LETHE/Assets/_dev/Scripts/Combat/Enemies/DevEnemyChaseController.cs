using UnityEngine;

namespace Lethe.Dev
{
    public sealed class DevEnemyChaseController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float moveSpeed = 1.35f;
        [SerializeField] private float stopDistance = 0.55f;
        [SerializeField] private SpriteRenderer bodyRenderer;

        private Vector3 spawnPosition;

        private void Awake()
        {
            spawnPosition = transform.position;
            if (bodyRenderer == null)
            {
                bodyRenderer = GetComponentInChildren<SpriteRenderer>();
            }
        }

        private void Update()
        {
            if (target == null)
            {
                var player = GameObject.Find("Player_EchoShowcase");
                target = player != null ? player.transform : null;
            }

            if (target == null)
            {
                return;
            }

            var delta = target.position - transform.position;
            delta.z = 0f;
            if (delta.sqrMagnitude <= stopDistance * stopDistance)
            {
                return;
            }

            var direction = delta.normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            if (bodyRenderer != null && Mathf.Abs(direction.x) > 0.05f)
            {
                bodyRenderer.flipX = direction.x < 0f;
            }
        }

        public void ResetToSpawn()
        {
            transform.position = spawnPosition;
        }

        public void SetTarget(Transform chaseTarget)
        {
            target = chaseTarget;
        }
    }
}
