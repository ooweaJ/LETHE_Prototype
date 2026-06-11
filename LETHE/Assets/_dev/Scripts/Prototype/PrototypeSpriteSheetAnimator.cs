using UnityEngine;

namespace Lethe.Dev
{
    public enum PrototypeFacing
    {
        Down = 0,
        Up = 1,
        Left = 2,
        Right = 3
    }

    public sealed class PrototypeSpriteSheetAnimator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer targetRenderer;
        [SerializeField] private Texture2D spriteSheet;
        [SerializeField] private int columns = 4;
        [SerializeField] private int rows = 8;
        [SerializeField] private float pixelsPerUnit = 128f;
        [SerializeField] private float idleFps = 4f;
        [SerializeField] private float walkFps = 8f;

        private Sprite[,] frames;
        private PrototypeFacing facing = PrototypeFacing.Down;
        private bool moving;
        private float frameClock;
        private int frameIndex;

        private void Awake()
        {
            if (targetRenderer == null)
            {
                targetRenderer = GetComponentInChildren<SpriteRenderer>();
            }

            BuildFrames();
        }

        private void Update()
        {
            if (frames == null || targetRenderer == null)
            {
                return;
            }

            var fps = moving ? walkFps : idleFps;
            frameClock += Time.deltaTime * fps;
            if (frameClock >= 1f)
            {
                frameClock -= 1f;
                frameIndex = (frameIndex + 1) % columns;
            }

            var row = (moving ? 4 : 0) + (int)facing;
            targetRenderer.sprite = frames[row, frameIndex];
        }

        public void SetMotion(Vector2 direction, bool isMoving)
        {
            moving = isMoving;
            if (direction.sqrMagnitude <= 0.01f)
            {
                return;
            }

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                facing = direction.x < 0f ? PrototypeFacing.Left : PrototypeFacing.Right;
            }
            else
            {
                facing = direction.y < 0f ? PrototypeFacing.Down : PrototypeFacing.Up;
            }
        }

        public void SetSheet(Texture2D sheet)
        {
            spriteSheet = sheet;
            BuildFrames();
        }

        private void BuildFrames()
        {
            if (spriteSheet == null || columns <= 0 || rows <= 0)
            {
                return;
            }

            frames = new Sprite[rows, columns];
            var cellWidth = spriteSheet.width / (float)columns;
            var cellHeight = spriteSheet.height / (float)rows;
            for (var row = 0; row < rows; row += 1)
            {
                for (var column = 0; column < columns; column += 1)
                {
                    var x = column * cellWidth;
                    var y = spriteSheet.height - ((row + 1) * cellHeight);
                    var rect = new Rect(x, y, cellWidth, cellHeight);
                    frames[row, column] = Sprite.Create(spriteSheet, rect, new Vector2(0.5f, 0.35f), pixelsPerUnit);
                }
            }
        }
    }
}
