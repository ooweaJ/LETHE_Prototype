using System.Collections.Generic;
using UnityEngine;

namespace Lethe.Dev
{
    public sealed class EchoProcLimiter : MonoBehaviour
    {
        [SerializeField] private int maxPerFrame = 12;
        [SerializeField] private float perRuntimeCooldown = 0.02f;

        private readonly Dictionary<object, float> nextAllowedTime = new Dictionary<object, float>();
        private int frame;
        private int frameCount;

        public bool CanProc(IEchoRuntime runtime, HitEvent hitEvent)
        {
            if (runtime == null || hitEvent.sourceType != HitSourceType.WeaponHit)
            {
                return false;
            }

            if (Time.frameCount != frame)
            {
                frame = Time.frameCount;
                frameCount = 0;
            }

            if (frameCount >= maxPerFrame)
            {
                return false;
            }

            return !nextAllowedTime.TryGetValue(runtime, out var until) || Time.time >= until;
        }

        public void RecordProc(IEchoRuntime runtime, HitEvent hitEvent)
        {
            if (runtime == null)
            {
                return;
            }

            frameCount += 1;
            nextAllowedTime[runtime] = Time.time + perRuntimeCooldown;
        }
    }
}
