using UnityEngine;

namespace Lethe.Dev
{
    public interface IWeaponRuntime
    {
        void Initialize(WeaponDefinition definition, WeaponContext context);
        void SetEnabled(bool enabled);
    }

    public interface IActiveMemoryRuntime
    {
        void Initialize(MemoryDefinition definition, MemoryRuntimeContext context);
        void ApplyLevel(int level);
        void AttachResonance(EchoDefinition echoDefinition, int echoLevel);
    }

    public interface IEchoRuntime
    {
        TriggerFamily TriggerFamily { get; }
        void Initialize(EchoDefinition definition, EchoRuntimeContext context);
        void ApplyLevel(int level);
        void HandleHit(HitEvent hitEvent);
        void HandleKill(KillEvent killEvent);
        void HandleDamageTaken(DamageTakenEvent damageTakenEvent);
    }

    public interface IUltimateEchoRuntime
    {
        void Initialize(EchoSynergyDefinition definition, UltimateRuntimeContext context);
        void SetActive(bool active);
    }

    public interface IPooledObject
    {
        void OnSpawn();
        void OnDespawn();
    }

    public sealed class WeaponContext
    {
        public GameObject Owner { get; }
        public RunBuildState BuildState { get; }
        public HitResolver HitResolver { get; }

        public WeaponContext(GameObject owner, RunBuildState buildState, HitResolver hitResolver)
        {
            Owner = owner;
            BuildState = buildState;
            HitResolver = hitResolver;
        }
    }

    public sealed class MemoryRuntimeContext
    {
        public GameObject Owner { get; }
        public RunBuildState BuildState { get; }

        public MemoryRuntimeContext(GameObject owner, RunBuildState buildState)
        {
            Owner = owner;
            BuildState = buildState;
        }
    }

    public sealed class EchoRuntimeContext
    {
        public GameObject Owner { get; }
        public RunBuildState BuildState { get; }
        public HitResolver HitResolver { get; }
        public PoolService Pool { get; }
        public FeedbackService Feedback { get; }

        public EchoRuntimeContext(GameObject owner, RunBuildState buildState, HitResolver hitResolver, PoolService pool, FeedbackService feedback)
        {
            Owner = owner;
            BuildState = buildState;
            HitResolver = hitResolver;
            Pool = pool;
            Feedback = feedback;
        }
    }

    public sealed class UltimateRuntimeContext
    {
        public GameObject Owner { get; }
        public RunBuildState BuildState { get; }
        public HitResolver HitResolver { get; }

        public UltimateRuntimeContext(GameObject owner, RunBuildState buildState, HitResolver hitResolver)
        {
            Owner = owner;
            BuildState = buildState;
            HitResolver = hitResolver;
        }
    }
}
