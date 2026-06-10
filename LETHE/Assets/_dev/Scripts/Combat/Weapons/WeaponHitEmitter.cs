using UnityEngine;

namespace Lethe.Dev
{
    public sealed class WeaponHitEmitter : MonoBehaviour
    {
        [SerializeField] private HitResolver hitResolver;
        [SerializeField] private float damage = 8f;

        public void Bind(HitResolver resolver)
        {
            hitResolver = resolver;
        }

        public void Emit(GameObject attacker, GameObject target, Vector2 position, Vector2 direction)
        {
            if (hitResolver == null || target == null)
            {
                return;
            }

            hitResolver.Resolve(HitEvent.Weapon(attacker, target, position, direction, damage));
        }
    }
}
