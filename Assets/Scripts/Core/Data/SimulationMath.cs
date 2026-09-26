using System;
using System.Collections.Generic;
using UnityEngine;

namespace CtrlHeart.Core.Data
{
    /// <summary>
    /// Pure mathematical models for internal simulation.
    /// Master Design Bible Part 2 §2.4, §2.5 & Dev Plan Day 1.
    /// </summary>
    public static class SimulationMath
    {
        public const float MIXTURE_PERCENT_DIFF_THRESHOLD = 15f;
        public const float MIXTURE_MIN_PERCENT_THRESHOLD = 25f;

        /// <summary>
        /// Calculates Oxygen regen rate based on current Lungs health (0 - 100).
        /// </summary>
        public static float CalculateOxygenRegen(float lungsHealth, float baseRegenRate)
        {
            float healthFactor = Mathf.Clamp01(lungsHealth / 100f);
            return baseRegenRate * healthFactor;
        }

        /// <summary>
        /// Calculates Focus regen rate based on current Brain health (0 - 100).
        /// </summary>
        public static float CalculateFocusRegen(float brainHealth, float baseRegenRate)
        {
            float healthFactor = Mathf.Clamp01(brainHealth / 100f);
            return baseRegenRate * healthFactor;
        }

        /// <summary>
        /// Detects the dominant EmotionState according to the locked mixture rule:
        /// Two emotions count as a named mixture state when both are within 15 percentage points
        /// of each other's influence AND both exceed 25% influence individually.
        /// Otherwise, the single highest-influence emotion is the pure-dominant state.
        /// Master Design Bible Part 2 §2.4.
        /// </summary>
        public static EmotionState EvaluateDominantState(NodeState node)
        {
            if (node == null) return EmotionState.FrozenBlank;

            var list = new List<(CoreEmotion emotion, float influence)>
            {
                (CoreEmotion.Calm, node.calmInfluence),
                (CoreEmotion.Anxiety, node.anxietyInfluence),
                (CoreEmotion.Confidence, node.confidenceInfluence),
                (CoreEmotion.Attraction, node.attractionInfluence)
            };

            // Sort descending by influence
            list.Sort((a, b) => b.influence.CompareTo(a.influence));

            var first = list[0];
            var second = list[1];

            // If nothing dominant / all zero
            if (first.influence <= 0.01f)
            {
                return EmotionState.FrozenBlank;
            }

            // Check mixture rule:
            // diff <= 15% AND both > 25%
            bool isMixture = (first.influence - second.influence <= MIXTURE_PERCENT_DIFF_THRESHOLD)
                             && (first.influence > MIXTURE_MIN_PERCENT_THRESHOLD)
                             && (second.influence > MIXTURE_MIN_PERCENT_THRESHOLD);

            if (isMixture)
            {
                return ResolveMixtureState(first.emotion, second.emotion);
            }

            return ResolvePureState(first.emotion);
        }

        public static EmotionState ResolvePureState(CoreEmotion emotion)
        {
            return emotion switch
            {
                CoreEmotion.Calm => EmotionState.Calm,
                CoreEmotion.Anxiety => EmotionState.Anxiety,
                CoreEmotion.Confidence => EmotionState.Confidence,
                CoreEmotion.Attraction => EmotionState.Attraction,
                _ => EmotionState.FrozenBlank
            };
        }

        public static EmotionState ResolveMixtureState(CoreEmotion e1, CoreEmotion e2)
        {
            // Normalize pair order to map 6 combinations:
            // (Calm, Anxiety), (Calm, Confidence), (Calm, Attraction),
            // (Anxiety, Confidence), (Anxiety, Attraction),
            // (Confidence, Attraction)
            if (e1 > e2)
            {
                var temp = e1;
                e1 = e2;
                e2 = temp;
            }

            if (e1 == CoreEmotion.Calm && e2 == CoreEmotion.Anxiety) return EmotionState.Mixture_1;
            if (e1 == CoreEmotion.Calm && e2 == CoreEmotion.Confidence) return EmotionState.Mixture_2;
            if (e1 == CoreEmotion.Calm && e2 == CoreEmotion.Attraction) return EmotionState.Mixture_3;
            if (e1 == CoreEmotion.Anxiety && e2 == CoreEmotion.Confidence) return EmotionState.Mixture_4;
            if (e1 == CoreEmotion.Anxiety && e2 == CoreEmotion.Attraction) return EmotionState.Mixture_5;
            if (e1 == CoreEmotion.Confidence && e2 == CoreEmotion.Attraction) return EmotionState.Mixture_6;

            return EmotionState.FrozenBlank;
        }
    }
}
