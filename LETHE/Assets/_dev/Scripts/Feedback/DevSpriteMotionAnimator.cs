using UnityEngine;

namespace Lethe.Dev
{
    public sealed class DevSpriteMotionAnimator : MonoBehaviour
    {
        [SerializeField] private Transform visualRoot;
        [SerializeField] private DevPlayerController2D playerController;
        [SerializeField] private DevEnemyChaseController enemyController;
        [SerializeField] private float idleBob = 0.025f;
        [SerializeField] private float moveBob = 0.075f;
        [SerializeField] private float bobSpeed = 8f;
        [SerializeField] private float tiltDegrees = 3f;

        private Vector3 baseLocalPosition;
        private Quaternion baseLocalRotation;

        private void Awake()
        {
            if (visualRoot == null)
            {
                var childVisual = transform.Find("Visual");
                visualRoot = childVisual != null ? childVisual : transform;
            }

            baseLocalPosition = visualRoot.localPosition;
            baseLocalRotation = visualRoot.localRotation;
        }

        private void Update()
        {
            var moving = IsMoving();
            var amount = moving ? moveBob : idleBob;
            var bob = Mathf.Sin(Time.time * bobSpeed) * amount;
            var tilt = moving ? Mathf.Sin(Time.time * bobSpeed * 0.5f) * tiltDegrees : 0f;
            visualRoot.localPosition = baseLocalPosition + Vector3.up * bob;
            visualRoot.localRotation = baseLocalRotation * Quaternion.Euler(0f, 0f, tilt);
        }

        private bool IsMoving()
        {
            if (playerController != null)
            {
                return playerController.IsMoving;
            }

            if (enemyController != null)
            {
                return true;
            }

            return false;
        }

        public void BindPlayer(DevPlayerController2D controller)
        {
            playerController = controller;
        }

        public void BindEnemy(DevEnemyChaseController controller)
        {
            enemyController = controller;
        }
    }
}
