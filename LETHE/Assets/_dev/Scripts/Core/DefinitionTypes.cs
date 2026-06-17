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

    public enum WeaponTargetingMode
    {
        Nearest,
        DensestArc
    }

    public enum WeaponEchoProcStyle
    {
        MultiSmall,
        SingleHeavy
    }

    public enum UltimatePattern
    {
        ManyFast,
        FewHeavy
    }

    public enum SlashSpriteShape
    {
        Crescent,
        WideCrescent,
        ImpactDiamond,
        Circle
    }

    public enum SlashAnchor
    {
        PrimaryTarget,
        CleaveTarget,
        HitCenter,
        FollowupOrigin,
        PlayerOrbit
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

    public enum MemoryEffectKind
    {
        HungryBlades,
        BloodReflection,
        ExecutionFlash,
        HunterOath,
        ShatterWave,
        StoppedSecond,
        AshenShield,
        OblivionBrand
    }

    public enum EchoFormKind
    {
        WeaponHabitSlash,
        BloodMarkThread,
        ExecutionCrack,
        PursuitAfterimage,
        ImpactRipple,
        TimeAfterimage,
        AshenGuard,
        OblivionBrandPulse
    }

    public enum EnemyRole
    {
        MeleeChaser,
        RangedCaster,
        Splitter,
        SupportPriest,
        BossGatekeeper
    }

    public enum EncounterSpawnMode
    {
        TimedPressure,
        KillMilestone,
        BossGate,
        DebugOnly
    }

    public enum UltimateTriggerMode
    {
        PassiveWhileReady,
        OnWeaponHit,
        OnKillChain,
        TimedBurst,
        ManualDebug
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

    [Serializable]
    public sealed class EncounterWaveData
    {
        public string id = "Wave_Opening";
        public EncounterSpawnMode spawnMode = EncounterSpawnMode.TimedPressure;
        public EnemyDefinition enemy;
        public float startTimeSeconds;
        public float endTimeSeconds = 30f;
        public float spawnInterval = 1.2f;
        public int spawnCount = 1;
        public int maxAlive = 24;
        public float spawnRadius = 8f;
        public string note;
    }

    [Serializable]
    public sealed class SlashVfxEntry
    {
        public string id = "Slash";
        public SlashSpriteShape spriteShape = SlashSpriteShape.Crescent;
        public SlashAnchor anchor = SlashAnchor.PrimaryTarget;
        public bool flip;
        public bool primaryOnly;
        public bool assistOnly;
        public Vector2 localOffset;
        public bool mirrorSideByLeadHand;
        public float rotationOffsetDegrees;
        public bool mirrorRotationByLeadHand;
        public float scale = 1f;
        public Color color = Color.white;
        public float lifetime = 0.18f;

        public bool Matches(bool primary)
        {
            if (primaryOnly && !primary) return false;
            if (assistOnly && primary) return false;
            return true;
        }
    }

    [CreateAssetMenu(menuName = "LETHE/Weapon VFX Profile")]
    public sealed class WeaponVfxProfile : ScriptableObject
    {
        public string id = "VFX_Weapon_DualBlades";
        public SlashVfxEntry[] weaponHitSlashes = Array.Empty<SlashVfxEntry>();
        public SlashVfxEntry[] kalmuriFollowupSlashes = Array.Empty<SlashVfxEntry>();
        public SlashVfxEntry[] ultimateSlashes = Array.Empty<SlashVfxEntry>();
        public Color weaponDamageNumberColor = new(1f, 0.96f, 0.72f);
        public Color nonWeaponDamageNumberColor = new(0.86f, 0.98f, 1f);
        public float weaponDamageNumberLifetime = 0.78f;
        public float nonWeaponDamageNumberLifetime = 0.62f;
        public Color enemyWeaponFlashColor = Color.white;
        public Color enemyNonWeaponFlashColor = Color.white;
        public float enemyWeaponFlashDuration = 0.105f;
        public float enemyNonWeaponFlashDuration = 0.075f;
        public bool showDamageNumbers = true;
        public float damageNumberMinNonWeaponDamage = 5f;
        public bool spawnHitSpark = true;
        public SlashVfxEntry hitSpark = new()
        {
            id = "HitSpark",
            spriteShape = SlashSpriteShape.ImpactDiamond,
            anchor = SlashAnchor.PrimaryTarget,
            localOffset = new Vector2(0f, 0.16f),
            scale = 0.16f,
            color = new Color(1f, 1f, 1f, 0.86f),
            lifetime = 0.06f
        };
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
        public float engageMultiplier = 1.1f;
        public float secondaryDamageMultiplier = 0.7f;
        public float primaryKnockback = 1.5f;
        public float secondaryKnockback = 0.9f;
        public float hitStopSeconds = 0.02f;
        public float cameraShakeAmount = 0.03f;
        public float swingAnimDuration = 0.16f;
        public float echoSizeScale = 1f;
        public float echoDamageScale = 1f;
        public WeaponTargetingMode targetingMode = WeaponTargetingMode.Nearest;
        public WeaponEchoProcStyle echoProcStyle = WeaponEchoProcStyle.MultiSmall;
        public UltimatePattern ultimatePattern = UltimatePattern.ManyFast;
        public float followupBaseDelay = 0.035f;
        public float followupStagger = 0.012f;
        public WeaponVfxProfile vfxProfile;
    }

    [CreateAssetMenu(menuName = "LETHE/Memory Definition")]
    public sealed class MemoryDefinition : ScriptableObject
    {
        public string id;
        public string displayName;
        [TextArea] public string shortDescription;
        [TextArea] public string cardDescription;
        public string matchingEchoId;
        public MemoryEffectKind effectKind = MemoryEffectKind.HungryBlades;
        public TriggerFamily triggerFamily = TriggerFamily.Periodic;
        public GameObject activeAbilityPrefab;
        [Range(1, 5)] public int maxLevel = 5;
        public EchoLevelData[] levelData = Array.Empty<EchoLevelData>();
        public float baseCooldown = 1f;
        public float baseDamage = 10f;
        public float targetingRadius = 2.5f;
        public FeedbackProfile feedbackProfile;
        [TextArea] public string activeFormNote;
        public string resonanceRiderId;
    }

    [CreateAssetMenu(menuName = "LETHE/Echo Definition")]
    public sealed class EchoDefinition : ScriptableObject
    {
        public string id;
        public string sourceMemoryId;
        public string displayName;
        [TextArea] public string shortDescription;
        [TextArea] public string cardDescription;
        public EchoFormKind formKind = EchoFormKind.WeaponHabitSlash;
        public TriggerFamily triggerFamily = TriggerFamily.OnWeaponHit;
        [Range(1, 5)] public int maxLevel = 5;
        public EchoLevelData[] levelData = Array.Empty<EchoLevelData>();
        public string awakenedName;
        [TextArea] public string awakenedDescription;
        public GameObject runtimePrefab;
        public FeedbackProfile feedbackProfile;
        public WeaponEchoProcStyle defaultWeaponProcStyle = WeaponEchoProcStyle.MultiSmall;
        public string[] preferredWeaponIds = Array.Empty<string>();
        [TextArea] public string weaponInteractionNote;
        [TextArea] public string overflowNote = "+5 cap overflow triggers one immediate echo burst.";
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

    [CreateAssetMenu(menuName = "LETHE/Ultimate Echo Definition")]
    public sealed class UltimateEchoDefinition : ScriptableObject
    {
        public string id = "Ultimate_BloodBladeStorm";
        public string displayName = "Blood Blade Storm";
        [TextArea] public string shortDescription;
        public string[] requiredEchoIds = Array.Empty<string>();
        [Range(1, 5)] public int requiredLevel = 5;
        public UltimateTriggerMode triggerMode = UltimateTriggerMode.OnWeaponHit;
        public UltimatePattern dualBladesPattern = UltimatePattern.ManyFast;
        public UltimatePattern greatswordPattern = UltimatePattern.FewHeavy;
        public float cooldown = 0.2f;
        public float duration = 3f;
        public float damageMultiplier = 1f;
        public GameObject runtimePrefab;
        public string hudGoalText;
        public FeedbackProfile feedbackProfile;
        public SlashVfxEntry[] fallbackSlashes = Array.Empty<SlashVfxEntry>();
        [TextArea] public string dualBladesNote = "Many small blood blades, fast rotation, chain-hit feel.";
        [TextArea] public string greatswordNote = "Few large blood blades, slower heavy slashes, stronger hitstop.";
    }

    [CreateAssetMenu(menuName = "LETHE/Echo Synergy Definition")]
    public sealed class EchoSynergyDefinition : ScriptableObject
    {
        public string id = "Synergy_BloodBladeStorm";
        public string displayName = "피의 칼폭풍";
        public string[] requiredEchoIds = Array.Empty<string>();
        [Range(1, 5)] public int requiredLevel = 5;
        public UltimateTriggerMode triggerMode = UltimateTriggerMode.OnWeaponHit;
        public UltimatePattern dualBladesPattern = UltimatePattern.ManyFast;
        public UltimatePattern greatswordPattern = UltimatePattern.FewHeavy;
        public float cooldown = 0.2f;
        public float duration = 3f;
        public float damageMultiplier = 1f;
        public GameObject runtimePrefab;
        public string hudGoalText;
        public FeedbackProfile feedbackProfile;
        public SlashVfxEntry[] fallbackSlashes = Array.Empty<SlashVfxEntry>();
        [TextArea] public string dualBladesNote = "Many small blood blades, fast rotation, chain-hit feel.";
        [TextArea] public string greatswordNote = "Few large blood blades, slower heavy slashes, stronger hitstop.";
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
        public EnemyRole role = EnemyRole.MeleeChaser;
        [TextArea] public string shortDescription;
        public string displayName = "침식자";
        public float maxHealth = 28f;
        public float moveSpeed = 1.35f;
        public float contactDamage = 3.5f;
        public float stopDistance = 0.45f;
        public float attackRange = 0.45f;
        public float attackCooldown = 1.2f;
        public float projectileSpeed = 4f;
        public float projectileDamage = 4f;
        public int splitCount;
        public int xpValue = 1;
        public float spawnCost = 1f;
        public GameObject runtimePrefab;
        public FeedbackProfile feedbackProfile;
        public string roleNote = "기본 압박";
    }

    [CreateAssetMenu(menuName = "LETHE/Encounter Definition")]
    public sealed class EncounterDefinition : ScriptableObject
    {
        public string id = "Encounter_M2_PrototypeLoop";
        public string displayName = "M2 Prototype Loop";
        public float targetLoopSeconds = 120f;
        public float firstBossSeconds = 62f;
        public float deficitSurvivalSeconds = 22f;
        public EncounterWaveData[] waves = Array.Empty<EncounterWaveData>();
        [TextArea] public string pacingNote = "Reach forget -> echo -> resonance -> +5 -> first ultimate inside the review window.";
    }

    [CreateAssetMenu(menuName = "LETHE/Reward Pool Definition")]
    public sealed class RewardPoolDefinition : ScriptableObject
    {
        public string id = "RewardPool_CompletePrototype";
        public WeaponDefinition[] weapons = Array.Empty<WeaponDefinition>();
        public MemoryDefinition[] memories = Array.Empty<MemoryDefinition>();
        public EchoDefinition[] echoes = Array.Empty<EchoDefinition>();
        public EchoSynergyDefinition[] synergies = Array.Empty<EchoSynergyDefinition>();
        public UltimateEchoDefinition[] ultimates = Array.Empty<UltimateEchoDefinition>();
        public EnemyDefinition[] enemies = Array.Empty<EnemyDefinition>();
        public EncounterDefinition[] encounters = Array.Empty<EncounterDefinition>();
    }
}
