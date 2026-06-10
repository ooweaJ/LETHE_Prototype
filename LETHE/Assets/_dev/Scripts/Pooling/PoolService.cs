using System.Collections.Generic;
using UnityEngine;

namespace Lethe.Dev
{
    public sealed class PoolService : MonoBehaviour
    {
        private readonly Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (prefab == null)
            {
                return null;
            }

            if (!pools.TryGetValue(prefab, out var queue))
            {
                queue = new Queue<GameObject>();
                pools.Add(prefab, queue);
            }

            var instance = queue.Count > 0 ? queue.Dequeue() : Instantiate(prefab);
            instance.transform.SetParent(parent, false);
            instance.transform.SetPositionAndRotation(position, rotation);
            instance.SetActive(true);
            foreach (var pooled in instance.GetComponentsInChildren<IPooledObject>(true))
            {
                pooled.OnSpawn();
            }

            return instance;
        }

        public void Despawn(GameObject prefab, GameObject instance)
        {
            if (prefab == null || instance == null)
            {
                return;
            }

            foreach (var pooled in instance.GetComponentsInChildren<IPooledObject>(true))
            {
                pooled.OnDespawn();
            }

            instance.SetActive(false);
            if (!pools.TryGetValue(prefab, out var queue))
            {
                queue = new Queue<GameObject>();
                pools.Add(prefab, queue);
            }

            queue.Enqueue(instance);
        }
    }
}
