using System;
using UnityEngine;

namespace Lethe.Dev
{
    public sealed class HitResolver : MonoBehaviour
    {
        [SerializeField] private FeedbackService feedbackService;
        [SerializeField] private EchoTriggerRouter echoTriggerRouter;

        public event Action<HitEvent> HitResolved;
        public event Action<KillEvent> TargetKilled;

        public void Resolve(HitEvent hitEvent)
        {
            if (hitEvent.target == null)
            {
                return;
            }

            var health = hitEvent.target.GetComponentInParent<Health>();
            var killed = false;
            if (health != null)
            {
                killed = health.ApplyDamage(hitEvent.damage, hitEvent.attacker);
            }

            feedbackService?.PlayHitFeedback(hitEvent);
            HitResolved?.Invoke(hitEvent);

            if (hitEvent.canTriggerEcho)
            {
                echoTriggerRouter?.RouteHit(hitEvent);
            }

            if (!killed)
            {
                return;
            }

            var killEvent = new KillEvent
            {
                attacker = hitEvent.attacker,
                target = hitEvent.target,
                position = hitEvent.position,
                sourceType = hitEvent.sourceType
            };
            TargetKilled?.Invoke(killEvent);
            echoTriggerRouter?.RouteKill(killEvent);
        }

        public void Bind(EchoTriggerRouter router, FeedbackService feedback)
        {
            echoTriggerRouter = router;
            feedbackService = feedback;
        }
    }
}
