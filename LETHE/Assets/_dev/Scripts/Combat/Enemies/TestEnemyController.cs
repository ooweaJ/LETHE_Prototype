using UnityEngine;

namespace Lethe.Dev
{
    public sealed class TestEnemyController : MonoBehaviour
    {
        [SerializeField] private float resetDelay = 0.5f;

        private Health health;
        private DevEnemyChaseController chaseController;
        private Vector3 spawnPosition;
        private float resetAt = -1f;

        private void Awake()
        {
            health = GetComponent<Health>();
            chaseController = GetComponent<DevEnemyChaseController>();
            spawnPosition = transform.position;
            if (health != null)
            {
                health.Died += HandleDied;
            }
        }

        private void Update()
        {
            if (resetAt > 0f && Time.time >= resetAt)
            {
                resetAt = -1f;
                if (chaseController != null)
                {
                    chaseController.ResetToSpawn();
                }
                else
                {
                    transform.position = spawnPosition;
                }

                health?.ResetHealth();
            }
        }

        private void HandleDied(Health target, GameObject source)
        {
            resetAt = Time.time + resetDelay;
        }
    }
}
