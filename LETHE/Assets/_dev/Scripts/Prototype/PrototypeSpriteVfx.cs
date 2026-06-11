using UnityEngine;

namespace Lethe.Dev
{
    public sealed class PrototypeSpriteVfx : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float lifetime = 0.35f;
        [SerializeField] private float startScale = 1f;
        [SerializeField] private float endScale = 1.35f;
        [SerializeField] private float spinDegreesPerSecond;

        private float age;
        private Color startColor = Color.white;

        private void Awake()
        {
            spriteRenderer ??= GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                startColor = spriteRenderer.color;
            }
        }

        private void Update()
        {
            age += Time.deltaTime;
            var t = lifetime <= 0f ? 1f : Mathf.Clamp01(age / lifetime);
            var scale = Mathf.Lerp(startScale, endScale, t);
            transform.localScale = new Vector3(scale, scale, scale);
            transform.Rotate(0f, 0f, spinDegreesPerSecond * Time.deltaTime);

            if (spriteRenderer != null)
            {
                var color = startColor;
                color.a *= 1f - (t * t);
                spriteRenderer.color = color;
            }

            if (age >= lifetime)
            {
                Destroy(gameObject);
            }
        }

        public void Configure(float duration, float fromScale, float toScale, float spin)
        {
            lifetime = duration;
            startScale = fromScale;
            endScale = toScale;
            spinDegreesPerSecond = spin;
        }
    }
}
