using System;
using UnityEngine;

namespace CtrlHeart.Core.Data.ScriptableObjects
{
    /// <summary>
    /// Represents an authored date reaction line.
    /// Master Design Bible Part 5 §5.5.
    /// Selected by parent Question + resulting Connection tier (1-4).
    /// </summary>
    [CreateAssetMenu(fileName = "Reaction_Q_Tier", menuName = "CTRL-Heart/Reaction Data")]
    public class ReactionData : ScriptableObject
    {
        [Tooltip("Reference to the parent Question")]
        public QuestionData parentQuestion;

        [Tooltip("The EmotionState matching the player answer that triggers this reaction (1:1 Option A)")]
        public EmotionState emotionState = EmotionState.Calm;

        [Tooltip("The resulting post-answer Connection tier (Tier 1-4)")]
        public ConnectionTier resultingTier = ConnectionTier.Tier2_Maybe;

        [TextArea(2, 4)]
        public string reactionText;

        [Tooltip("The player answer delta this reaction corresponds to (cross-check value)")]
        public float linkedAnswerDelta = 0f;

        [Tooltip("Tag of the animation clip (~20 shared pool) to play with this line")]
        public string animationClipTag = "neutral_nod";
    }
}
