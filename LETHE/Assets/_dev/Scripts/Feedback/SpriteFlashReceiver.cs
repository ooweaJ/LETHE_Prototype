using System.Collections;
using UnityEngine;

namespace Lethe.Dev
{
    public sealed class SpriteFlashReceiver : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer targetRenderer;

        private Color originalColor = Color.white;
        private Coroutine flashRoutine;

        private void Awake()
        {
            if (targetRenderer == null)
            {
                targetRenderer = GetComponentInChildren<SpriteRenderer>();
            }

            if (targetRenderer != null)
            {
                originalColor = targetRenderer.color;
            }
        }

        public void Flash(Color color, float duration)
        {
            if (targetRenderer == null)
            {
                return;
            }

            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
            }

            flashRoutine = StartCoroutine(FlashRoutine(color, duration));
        }

        private IEnumerator FlashRoutine(Color color, float duration)
        {
            targetRenderer.color = color;
            yield return new WaitForSeconds(duration);
            targetRenderer.color = originalColor;
            flashRoutine = null;
        }
    }
}
