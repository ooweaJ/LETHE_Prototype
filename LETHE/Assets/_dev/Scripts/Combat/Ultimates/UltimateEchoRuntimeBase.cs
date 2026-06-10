using UnityEngine;

namespace Lethe.Dev
{
    public abstract class UltimateEchoRuntimeBase : MonoBehaviour, IUltimateEchoRuntime
    {
        protected EchoSynergyDefinition Definition { get; private set; }
        protected UltimateRuntimeContext Context { get; private set; }

        public virtual void Initialize(EchoSynergyDefinition definition, UltimateRuntimeContext context)
        {
            Definition = definition;
            Context = context;
        }

        public virtual void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}
