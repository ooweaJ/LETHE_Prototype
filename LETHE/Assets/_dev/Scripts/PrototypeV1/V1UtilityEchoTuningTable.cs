using System;
using UnityEngine;

namespace Lethe.PrototypeV1
{
    [CreateAssetMenu(menuName = "LETHE/Prototype V1/Utility Echo Tuning Table")]
    public sealed class V1UtilityEchoTuningTable : ScriptableObject
    {
        public V1GameManager.UtilityEchoTuningSpec[] specs = Array.Empty<V1GameManager.UtilityEchoTuningSpec>();

        public bool TryGetSpec(V1MemoryId id, out V1GameManager.UtilityEchoTuningSpec spec)
        {
            if (specs != null)
            {
                for (int i = 0; i < specs.Length; i++)
                {
                    if (specs[i].Id == id)
                    {
                        spec = specs[i];
                        return true;
                    }
                }
            }

            spec = default;
            return false;
        }
    }
}
