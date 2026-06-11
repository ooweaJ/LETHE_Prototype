using System;
using UnityEngine;

namespace Lethe.Dev
{
    public sealed class PrototypeHealth : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 40f;
        [SerializeField] private SpriteRenderer flashRenderer;
        [SerializeField] private Color flashColor = Color.white;
        [SerializeField] private float flashDuration = 0.06f;

        private float currentHealth;
        private Color originalColor = Color.white;
        private float flashUntil;

        public event Action<PrototypeHealth, GameObject> Died;
        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public bool IsDead => currentHealth <= 0f;

        private void Awake()
        {
            flashRenderer ??= GetComponentInChildren<SpriteRenderer>();
            if (flashRenderer != null)
            {
                originalColor = flashRenderer.color;
            }

            ResetHealth();
        }

        private void Update()
        {
            if (flashRenderer != null && Time.time >= flashUntil)
            {
                flashRenderer.color = originalColor;
            }
        }

        public bool ApplyDamage(float amount, GameObject source)
        {
            if (IsDead)
            {
                return false;
            }

            currentHealth = Mathf.Max(0f, currentHealth - Mathf.Max(0f, amount));
            Flash();
            if (!IsDead)
            {
                return false;
            }

            Died?.Invoke(this, source);
            return true;
        }

        public void Heal(float amount)
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + Mathf.Max(0f, amount));
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
            gameObject.SetActive(true);
            if (flashRenderer != null)
            {
                flashRenderer.color = originalColor;
            }
        }

        public void ConfigureMaxHealth(float value)
        {
            maxHealth = Mathf.Max(1f, value);
            ResetHealth();
        }

        private void Flash()
        {
            if (flashRenderer == null)
            {
                return;
            }

            flashRenderer.color = flashColor;
            flashUntil = Time.time + flashDuration;
        }
    }
}
