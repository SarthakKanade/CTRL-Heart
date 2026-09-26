using System;
using System.Collections.Generic;
using UnityEngine;

namespace CtrlHeart.Core.Data
{
    /// <summary>
    /// Represents the authored RTS trigger payload attached to a slot or event.
    /// Master Design Bible Part 4 §4.3 & Dev Plan Day 1.
    /// </summary>
    [Serializable]
    public class SocialEventProfile
    {
        [Tooltip("The list of target nodes. The first-listed node is the PRIMARY target that decides answer selection.")]
        public List<InternalNodeType> targetNodes = new List<InternalNodeType>();

        public SocialEffectType effectType = SocialEffectType.StressEvent;

        [Tooltip("Only relevant if effectType == StressEvent")]
        public StressEventSubtype stressEventSubtype = StressEventSubtype.None;

        [Tooltip("Only relevant if effectType == DirectEmotionPush")]
        public CoreEmotion pushTargetEmotion = CoreEmotion.Calm;

        [Range(0f, 100f)]
        public float magnitude = 20f;

        public InternalNodeType PrimaryTargetNode => (targetNodes != null && targetNodes.Count > 0) 
            ? targetNodes[0] 
            : InternalNodeType.Brain;
    }
}
