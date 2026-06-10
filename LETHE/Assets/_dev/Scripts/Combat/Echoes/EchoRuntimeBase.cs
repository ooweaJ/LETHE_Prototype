using UnityEngine;

namespace Lethe.Dev
{
    public abstract class EchoRuntimeBase : MonoBehaviour, IEchoRuntime
    {
        protected EchoDefinition Definition { get; private set; }
        protected EchoRuntimeContext Context { get; private set; }
        protected int Level { get; private set; }

        public TriggerFamily TriggerFamily => Definition != null ? Definition.triggerFamily : TriggerFamily.OnWeaponHit;

        public virtual void Initialize(EchoDefinition definition, EchoRuntimeContext context)
        {
            Definition = definition;
            Context = context;
        }

        public virtual void ApplyLevel(int level)
        {
            Level = Mathf.Clamp(level, 1, Definition != null ? Definition.maxLevel : 5);
        }

        public virtual void HandleHit(HitEvent hitEvent)
        {
        }

        public virtual void HandleKill(KillEvent killEvent)
        {
        }

        public virtual void HandleDamageTaken(DamageTakenEvent damageTakenEvent)
        {
        }
    }
}
