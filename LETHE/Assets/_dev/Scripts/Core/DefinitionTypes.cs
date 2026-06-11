using System;
using UnityEngine;

namespace Lethe.Dev
{
    public enum EchoStyle
    {
        SmallFrequent,
        HeavyConditional
    }

    public enum WeaponRhythm
    {
        FastProc,
        HeavyConditional
    }

    public enum TriggerFamily
    {
        OnWeaponHit,
        OnKill,
        OnDamageTaken,
        OnShieldBreak,
        Periodic
    }

    public enum HitSourceType
    {
        WeaponHit,
        EchoHit,
        UltimateHit
    }

    [Flags]
    public enum EchoBehavior
    {
        None = 0,
        DelayedSlash = 1 << 0,
        LingerSlash = 1 << 1,
        OrbitShard = 1 << 2,
        LaunchBlade = 1 << 3,
        BloodMark = 1 << 4,
        HealThread = 1 << 5,
        BloodBloom = 1 << 6,
        KillAcceleration = 1 << 7,
        ExecuteCrack = 1 << 8,
        HomingShot = 1 << 9,
        Shockwave = 1 << 10,
        TimeFracture = 1 << 11,
        AshenGuard = 1 << 12,
        OblivionBrand = 1 << 13
    }

    [Serializable]
    public sealed class EchoLevelData
    {
        [Range(1, 5)] public int level = 1;
        [Range(0f, 1f)] public float procChance = 1f;
        public int hitInterval = 1;
        public int spawnCount = 1;
        public float damageMultiplier = 1f;
        public float duration = 0.25f;
        public float radius = 1f;
        public float cooldown = 0.1f;
        public EchoBehavior behaviors;
    }

    [CreateAssetMenu(menuName = "LETHE/Weapon Definition")]
    public sealed class WeaponDefinition : ScriptableObject
    {
        public string id = "Weapon_DualBlades";
        public string displayName = "절단쌍검";
        public float attackCadence = 0.22f;
        public GameObject hitboxPrefab;
        public float baseDamage = 8f;
        public FeedbackProfile hitStopProfile;
        public AudioClip[] sfxLayers;
        public EchoStyle echoStyle = EchoStyle.SmallFrequent;
        public WeaponRhythm rhythm = WeaponRhythm.FastProc;
        public float attackRange = 2f;
        public float attackArcDegrees = 100f;
        public int maxTargetsPerSwing = 3;
    }

    [CreateAssetMenu(menuName = "LETHE/Memory Definition")]
    public sealed class MemoryDefinition : ScriptableObject
    {
        public string id;
        public string displayName;
        public string matchingEchoId;
        public GameObject activeAbilityPrefab;
        [Range(1, 5)] public int maxLevel = 5;
        public EchoLevelData[] levelData = Array.Empty<EchoLevelData>();
        public string resonanceRiderId;
    }

    [CreateAssetMenu(menuName = "LETHE/Echo Definition")]
    public sealed class EchoDefinition : ScriptableObject
    {
        public string id;
        public string sourceMemoryId;
        public string displayName;
        public TriggerFamily triggerFamily = TriggerFamily.OnWeaponHit;
        [Range(1, 5)] public int maxLevel = 5;
        public EchoLevelData[] levelData = Array.Empty<EchoLevelData>();
        public string awakenedName;
        public GameObject runtimePrefab;
        public FeedbackProfile feedbackProfile;
        public bool blockRecursiveEchoHits = true;

        public EchoLevelData GetLevelData(int level)
        {
            if (levelData == null || levelData.Length == 0)
            {
                return new EchoLevelData { level = Mathf.Clamp(level, 1, maxLevel) };
            }

            var clamped = Mathf.Clamp(level, 1, maxLevel);
            for (var index = 0; index < levelData.Length; index += 1)
            {
                if (levelData[index] != null && levelData[index].level == clamped)
                {
                    return levelData[index];
                }
            }

            return levelData[Mathf.Clamp(clamped - 1, 0, levelData.Length - 1)];
        }
    }

    [CreateAssetMenu(menuName = "LETHE/Echo Synergy Definition")]
    public sealed class EchoSynergyDefinition : ScriptableObject
    {
        public string id = "Synergy_BloodBladeStorm";
        public string displayName = "피의 칼폭풍";
        public string[] requiredEchoIds = Array.Empty<string>();
        [Range(1, 5)] public int requiredLevel = 5;
        public GameObject runtimePrefab;
        public string hudGoalText;
        public FeedbackProfile feedbackProfile;
    }

    [CreateAssetMenu(menuName = "LETHE/Feedback Profile")]
    public sealed class FeedbackProfile : ScriptableObject
    {
        [Range(0, 12)] public int hitStopFrames;
        [Range(0f, 1f)] public float cameraImpulse;
        public Color enemyFlashColor = Color.white;
        public float enemyFlashDuration = 0.05f;
        public AudioClip[] sfxLayers = Array.Empty<AudioClip>();
        public Color lightColor = Color.cyan;
        public float lightIntensity = 1f;
        public GameObject particlePrefab;
    }

    [CreateAssetMenu(menuName = "LETHE/Enemy Definition")]
    public sealed class EnemyDefinition : ScriptableObject
    {
        public string id = "Enemy_MeleeChaser";
        public string displayName = "침식자";
        public float maxHealth = 28f;
        public float moveSpeed = 1.35f;
        public float contactDamage = 3.5f;
        public float stopDistance = 0.45f;
        public string roleNote = "기본 압박";
    }

    [CreateAssetMenu(menuName = "LETHE/Reward Pool Definition")]
    public sealed class RewardPoolDefinition : ScriptableObject
    {
        public string id = "RewardPool_CompletePrototype";
        public WeaponDefinition[] weapons = Array.Empty<WeaponDefinition>();
        public MemoryDefinition[] memories = Array.Empty<MemoryDefinition>();
        public EchoDefinition[] echoes = Array.Empty<EchoDefinition>();
        public EchoSynergyDefinition[] synergies = Array.Empty<EchoSynergyDefinition>();
        public EnemyDefinition[] enemies = Array.Empty<EnemyDefinition>();
    }
}
