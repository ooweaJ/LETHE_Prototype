using System;
using UnityEngine;

namespace Lethe.Dev
{
    public sealed class Health : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 30f;
        [SerializeField] private bool destroyOnDeath;

        private float currentHealth;

        public event Action<Health, GameObject> Died;
        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public bool IsDead => currentHealth <= 0f;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public bool ApplyDamage(float amount, GameObject source)
        {
            if (IsDead)
            {
                return false;
            }

            currentHealth = Mathf.Max(0f, currentHealth - Mathf.Max(0f, amount));
            if (!IsDead)
            {
                return false;
            }

            Died?.Invoke(this, source);
            if (destroyOnDeath)
            {
                gameObject.SetActive(false);
            }

            return true;
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
            gameObject.SetActive(true);
        }
    }
}
