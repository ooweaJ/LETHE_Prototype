using UnityEngine;

namespace Lethe.Dev
{
    public abstract class ActiveMemoryRuntimeBase : MonoBehaviour, IActiveMemoryRuntime
    {
        protected MemoryDefinition Definition { get; private set; }
        protected MemoryRuntimeContext Context { get; private set; }
        protected int Level { get; private set; }

        public virtual void Initialize(MemoryDefinition definition, MemoryRuntimeContext context)
        {
            Definition = definition;
            Context = context;
        }

        public virtual void ApplyLevel(int level)
        {
            Level = Mathf.Clamp(level, 1, Definition != null ? Definition.maxLevel : 5);
        }

        public virtual void AttachResonance(EchoDefinition echoDefinition, int echoLevel)
        {
        }
    }
}
