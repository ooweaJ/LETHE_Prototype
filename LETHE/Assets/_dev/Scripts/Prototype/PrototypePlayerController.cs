using UnityEngine;

namespace Lethe.Dev
{
    public sealed class PrototypePlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 3.65f;
        [SerializeField] private Vector2 arenaHalfExtents = new Vector2(7.2f, 4.2f);
        [SerializeField] private PrototypeSpriteSheetAnimator animator;
        [SerializeField] private Transform weaponAnchor;

        private Vector2 moveInput;
        private Vector2 facing = Vector2.down;

        public Vector2 Facing => facing;
        public Transform WeaponAnchor => weaponAnchor;

        private void Awake()
        {
            animator ??= GetComponentInChildren<PrototypeSpriteSheetAnimator>();
            if (weaponAnchor == null)
            {
                var anchor = transform.Find("WeaponAnchor");
                weaponAnchor = anchor != null ? anchor : transform;
            }
        }

        private void Update()
        {
            moveInput = ReadMoveInput();
            if (moveInput.sqrMagnitude > 1f)
            {
                moveInput.Normalize();
            }

            if (moveInput.sqrMagnitude > 0.01f)
            {
                facing = moveInput.normalized;
            }

            var nextPosition = transform.position + (Vector3)(moveInput * moveSpeed * Time.deltaTime);
            nextPosition.x = Mathf.Clamp(nextPosition.x, -arenaHalfExtents.x, arenaHalfExtents.x);
            nextPosition.y = Mathf.Clamp(nextPosition.y, -arenaHalfExtents.y, arenaHalfExtents.y);
            transform.position = nextPosition;
            animator?.SetMotion(facing, moveInput.sqrMagnitude > 0.01f);
            UpdateWeaponAnchor();
        }

        private static Vector2 ReadMoveInput()
        {
            var input = Vector2.zero;
#if ENABLE_INPUT_SYSTEM
            var keyboard = UnityEngine.InputSystem.Keyboard.current;
            if (keyboard != null)
            {
                if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) input.x -= 1f;
                if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) input.x += 1f;
                if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) input.y -= 1f;
                if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) input.y += 1f;
                return input;
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
#endif
            return input;
        }

        private void UpdateWeaponAnchor()
        {
            if (weaponAnchor == null)
            {
                return;
            }

            var offset = new Vector3(facing.x * 0.38f, facing.y * 0.18f - 0.05f, 0f);
            weaponAnchor.localPosition = offset;
            var angle = Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg;
            weaponAnchor.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
