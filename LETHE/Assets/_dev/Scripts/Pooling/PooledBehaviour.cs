using UnityEngine;

namespace Lethe.Dev
{
    public class PooledBehaviour : MonoBehaviour, IPooledObject
    {
        public virtual void OnSpawn()
        {
        }

        public virtual void OnDespawn()
        {
        }
    }
}
