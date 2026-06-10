using System.Collections;
using UnityEngine;

namespace Lethe.Dev
{
    public sealed class FeedbackService : MonoBehaviour
    {
        [SerializeField] private FeedbackProfile defaultProfile;

        public void PlayHitFeedback(HitEvent hitEvent)
        {
            var receiver = hitEvent.target != null ? hitEvent.target.GetComponentInChildren<SpriteFlashReceiver>() : null;
            if (receiver != null)
            {
                var color = defaultProfile != null ? defaultProfile.enemyFlashColor : Color.white;
                var duration = defaultProfile != null ? defaultProfile.enemyFlashDuration : 0.05f;
                receiver.Flash(color, duration);
            }

            if (defaultProfile != null && defaultProfile.hitStopFrames > 0)
            {
                StartCoroutine(HitStop(defaultProfile.hitStopFrames));
            }
        }

        private static IEnumerator HitStop(int frames)
        {
            var previousScale = Time.timeScale;
            Time.timeScale = 0f;
            for (var index = 0; index < frames; index += 1)
            {
                yield return null;
            }

            Time.timeScale = previousScale;
        }
    }
}
