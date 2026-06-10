using UnityEngine;

namespace Lethe.Dev
{
    public struct HitEvent
    {
        public HitSourceType sourceType;
        public GameObject attacker;
        public GameObject target;
        public Vector2 position;
        public Vector2 direction;
        public float damage;
        public string[] tags;
        public bool canTriggerEcho;
        public bool canTriggerResonance;

        public static HitEvent Weapon(GameObject attacker, GameObject target, Vector2 position, Vector2 direction, float damage)
        {
            return new HitEvent
            {
                sourceType = HitSourceType.WeaponHit,
                attacker = attacker,
                target = target,
                position = position,
                direction = direction.sqrMagnitude > 0f ? direction.normalized : Vector2.right,
                damage = damage,
                tags = System.Array.Empty<string>(),
                canTriggerEcho = true,
                canTriggerResonance = true
            };
        }
    }

    public struct KillEvent
    {
        public GameObject attacker;
        public GameObject target;
        public Vector2 position;
        public HitSourceType sourceType;
    }

    public struct DamageTakenEvent
    {
        public GameObject source;
        public GameObject target;
        public float amount;
        public Vector2 position;
    }
}
