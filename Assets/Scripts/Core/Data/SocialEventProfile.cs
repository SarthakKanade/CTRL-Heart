using System;
using System.Collections.Generic;
using UnityEngine;

namespace CtrlHeart.Core.Data
{
    /// <summary>
    /// An authored effect targeting a specific node in a Social Event Profile.
    /// Supports compound multi-node and dual-target events.
    /// </summary>
    [Serializable]
    public class TargetedEffect
    {
        [Tooltip("The specific node targeted by this effect")]
        public InternalNodeType targetNode = InternalNodeType.Brain;

        public SocialEffectType effectType = SocialEffectType.StressEvent;

        [Tooltip("Only relevant if effectType == StressEvent")]
        public StressEventSubtype stressEventSubtype = StressEventSubtype.None;

        [Tooltip("Only relevant if effectType == DirectEmotionPush")]
        public CoreEmotion pushTargetEmotion = CoreEmotion.Calm;

        [Range(0f, 100f)]
        public float magnitude = 20f;
    }

    /// <summary>
    /// Represents the authored RTS trigger payload attached to a slot or event.
    /// Master Design Bible Part 4 §4.3 & Dev Plan Day 1.
    /// Supports compound multi-node and dual-target events.
    /// </summary>
    [Serializable]
    public class SocialEventProfile
    {
        [Tooltip("List of targeted effects. The first entry's targetNode is the PRIMARY target that decides answer selection.")]
        public List<TargetedEffect> effects = new List<TargetedEffect>();

        [Tooltip("Gated on player initiation rather than auto-resolving on timer expiry alone (Slot 4 T2)")]
        public bool requiresPlayerInitiation = false;

        [Tooltip("Explicit primary target node override when effects list is empty (e.g., Slot 6 T2 natural Oxygen recovery beat on Lungs)")]
        public InternalNodeType primaryNodeOverride = InternalNodeType.Brain;

        public InternalNodeType PrimaryTargetNode
        {
            get
            {
                if (effects != null && effects.Count > 0)
                {
                    return effects[0].targetNode;
                }
                return primaryNodeOverride;
            }
        }
    }
}
