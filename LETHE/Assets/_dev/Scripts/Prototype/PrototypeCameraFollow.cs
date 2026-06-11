using UnityEngine;

namespace Lethe.Dev
{
    public sealed class PrototypeCameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float followSharpness = 7f;
        [SerializeField] private Vector3 offset = new Vector3(0f, 0.2f, -10f);

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            transform.position = Vector3.Lerp(transform.position, target.position + offset, 1f - Mathf.Exp(-followSharpness * Time.deltaTime));
        }

        public void SetTarget(Transform followTarget)
        {
            target = followTarget;
        }
    }
}
