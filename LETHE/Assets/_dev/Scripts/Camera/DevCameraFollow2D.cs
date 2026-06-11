using UnityEngine;

namespace Lethe.Dev
{
    public sealed class DevCameraFollow2D : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float followSharpness = 8f;
        [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

        private void LateUpdate()
        {
            if (target == null)
            {
                var player = GameObject.Find("Player_EchoShowcase");
                target = player != null ? player.transform : null;
            }

            if (target == null)
            {
                return;
            }

            var desired = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desired, 1f - Mathf.Exp(-followSharpness * Time.deltaTime));
        }

        public void SetTarget(Transform followTarget)
        {
            target = followTarget;
        }
    }
}
