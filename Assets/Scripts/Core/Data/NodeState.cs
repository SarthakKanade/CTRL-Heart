using System;
using System.Collections.Generic;
using UnityEngine;

namespace CtrlHeart.Core.Data
{
    /// <summary>
    /// Represents a single node on the 5-node internal map.
    /// Tracks node health and influence percentage for each core emotion.
    /// </summary>
    [Serializable]
    public class NodeState
    {
        public InternalNodeType nodeType;
        [Range(0f, 100f)] public float currentHealth = 100f;
        public const float MAX_HEALTH = 100f;

        // Influence percentages (0 - 100) for each core emotion
        public float calmInfluence;
        public float anxietyInfluence;
        public float confidenceInfluence;
        public float attractionInfluence;

        public NodeState(InternalNodeType type)
        {
            nodeType = type;
            currentHealth = MAX_HEALTH;
            calmInfluence = 0f;
            anxietyInfluence = 0f;
            confidenceInfluence = 0f;
            attractionInfluence = 0f;
        }

        public void DecayInfluences(float rate)
        {
            float factor = Mathf.Clamp01(1.0f - rate);
            calmInfluence *= factor;
            anxietyInfluence *= factor;
            confidenceInfluence *= factor;
            attractionInfluence *= factor;
        }

        public float GetInfluence(CoreEmotion emotion)
        {
            return emotion switch
            {
                CoreEmotion.Calm => calmInfluence,
                CoreEmotion.Anxiety => anxietyInfluence,
                CoreEmotion.Confidence => confidenceInfluence,
                CoreEmotion.Attraction => attractionInfluence,
                _ => 0f
            };
        }

        public void SetInfluence(CoreEmotion emotion, float value)
        {
            switch (emotion)
            {
                case CoreEmotion.Calm: calmInfluence = Mathf.Clamp(value, 0f, 100f); break;
                case CoreEmotion.Anxiety: anxietyInfluence = Mathf.Clamp(value, 0f, 100f); break;
                case CoreEmotion.Confidence: confidenceInfluence = Mathf.Clamp(value, 0f, 100f); break;
                case CoreEmotion.Attraction: attractionInfluence = Mathf.Clamp(value, 0f, 100f); break;
            }
        }
    }
}
