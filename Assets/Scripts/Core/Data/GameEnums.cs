using System;

namespace CtrlHeart.Core.Data
{
    /// <summary>
    /// The five fixed nodes of the internal map.
    /// </summary>
    public enum InternalNodeType
    {
        Brain,
        Lungs,
        Heart,
        Voice,
        Body
    }

    /// <summary>
    /// The four core emotions the player commands.
    /// </summary>
    public enum CoreEmotion
    {
        Calm,
        Anxiety,
        Confidence,
        Attraction
    }

    /// <summary>
    /// The 10 answer-selecting emotion/mixture states + Frozen/Blank state.
    /// Defined per Master Design Bible Part 2 §2.4 & Part 5 §5.4/§5.6.
    /// </summary>
    public enum EmotionState
    {
        // Core 4
        Calm,
        Anxiety,
        Confidence,
        Attraction,

        // 6 Named Mixtures
        CalmAnxiety,
        CalmConfidence,
        CalmAttraction,
        AnxietyConfidence,
        AnxietyAttraction,
        ConfidenceAttraction,

        // Special 11th State
        FrozenBlank
    }

    /// <summary>
    /// The primary type of a Social Event Profile effect.
    /// </summary>
    public enum SocialEffectType
    {
        StressEvent,
        DirectEmotionPush
    }

    /// <summary>
    /// Subtypes of Stress Events (threats).
    /// </summary>
    public enum StressEventSubtype
    {
        None,
        Panic,           // Brain: ↓Focus, ↑Anxiety
        HeartFlutter,    // Heart: ↑Composure pressure, faster heartbeat
        AwkwardSilence,  // Voice: ↓communication quality
        Overthinking,    // Brain: Focus interference, memory mistakes
        SweatSurge       // Body: visible physical embarrassment
    }

    /// <summary>
    /// Connection tiers governing question selection and reactions.
    /// </summary>
    public enum ConnectionTier
    {
        Tier0_Collapse = 0,   // 0 Connection -> Immediate Date Collapse
        Tier1_Awkward = 1,    // 1-39 Connection
        Tier2_Maybe = 2,      // 40-69 Connection
        Tier3_Strong = 3,     // 70-89 Connection
        Tier4_SecondDate = 4  // 90-100 Connection
    }

    /// <summary>
    /// Endings possible at slot 10 resolution or earlier on failure.
    /// </summary>
    public enum EndingType
    {
        DateCollapse_Tier0,
        AwkwardEnding_Tier1,
        Maybe_Tier2,
        SecondDate_Tier3_4,
        Meltdown_ComposureZero
    }
}
