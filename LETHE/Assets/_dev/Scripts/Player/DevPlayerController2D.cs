using UnityEngine;

namespace Lethe.Dev
{
    public sealed class DevPlayerController2D : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 3.8f;
        [SerializeField] private SpriteRenderer bodyRenderer;
        [SerializeField] private Transform weaponAnchor;

        private Vector2 moveInput;
        private Vector2 facing = Vector2.right;

        public Vector2 MoveInput => moveInput;
        public Vector2 Facing => facing;
        public bool IsMoving => moveInput.sqrMagnitude > 0.01f;

        private void Awake()
        {
            if (bodyRenderer == null)
            {
                bodyRenderer = GetComponentInChildren<SpriteRenderer>();
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

            transform.position += (Vector3)(moveInput * moveSpeed * Time.deltaTime);
            ApplyFacing();
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

        private void ApplyFacing()
        {
            if (bodyRenderer != null && Mathf.Abs(facing.x) > 0.05f)
            {
                bodyRenderer.flipX = facing.x < 0f;
            }

            if (weaponAnchor != null)
            {
                var side = facing.x < -0.05f ? -1f : 1f;
                weaponAnchor.localPosition = new Vector3(0.42f * side, -0.03f, 0f);
                weaponAnchor.localScale = new Vector3(side, 1f, 1f);
            }
        }

        public void SetWeaponAnchor(Transform anchor)
        {
            weaponAnchor = anchor;
        }
    }
}
