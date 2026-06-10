using UnityEngine;

namespace Lethe.Dev
{
    public abstract class WeaponRuntimeBase : MonoBehaviour, IWeaponRuntime
    {
        protected WeaponDefinition Definition { get; private set; }
        protected WeaponContext Context { get; private set; }

        public virtual void Initialize(WeaponDefinition definition, WeaponContext context)
        {
            Definition = definition;
            Context = context;
        }

        public virtual void SetEnabled(bool enabled)
        {
            gameObject.SetActive(enabled);
        }
    }
}
