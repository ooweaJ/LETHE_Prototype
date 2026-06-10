using System.Collections.Generic;
using UnityEngine;

namespace Lethe.Dev
{
    public sealed class EchoTriggerRouter : MonoBehaviour
    {
        [SerializeField] private EchoProcLimiter procLimiter;

        private readonly List<IEchoRuntime> runtimes = new List<IEchoRuntime>();

        public void Register(IEchoRuntime runtime)
        {
            if (runtime != null && !runtimes.Contains(runtime))
            {
                runtimes.Add(runtime);
            }
        }

        public void Unregister(IEchoRuntime runtime)
        {
            runtimes.Remove(runtime);
        }

        public void RouteHit(HitEvent hitEvent)
        {
            if (hitEvent.sourceType != HitSourceType.WeaponHit || !hitEvent.canTriggerEcho)
            {
                return;
            }

            for (var index = 0; index < runtimes.Count; index += 1)
            {
                var runtime = runtimes[index];
                if (runtime == null || runtime.TriggerFamily != TriggerFamily.OnWeaponHit)
                {
                    continue;
                }

                if (procLimiter != null && !procLimiter.CanProc(runtime, hitEvent))
                {
                    continue;
                }

                runtime.HandleHit(hitEvent);
                procLimiter?.RecordProc(runtime, hitEvent);
            }
        }

        public void RouteKill(KillEvent killEvent)
        {
            for (var index = 0; index < runtimes.Count; index += 1)
            {
                var runtime = runtimes[index];
                if (runtime != null && runtime.TriggerFamily == TriggerFamily.OnKill)
                {
                    runtime.HandleKill(killEvent);
                }
            }
        }

        public void RouteDamageTaken(DamageTakenEvent damageTakenEvent)
        {
            for (var index = 0; index < runtimes.Count; index += 1)
            {
                var runtime = runtimes[index];
                if (runtime != null && runtime.TriggerFamily == TriggerFamily.OnDamageTaken)
                {
                    runtime.HandleDamageTaken(damageTakenEvent);
                }
            }
        }
    }
}
