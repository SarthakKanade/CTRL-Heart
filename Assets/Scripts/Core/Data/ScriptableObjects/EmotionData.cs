using System;
using UnityEngine;

namespace CtrlHeart.Core.Data.ScriptableObjects
{
    /// <summary>
    /// Configuration data for one of the 4 core emotions.
    /// </summary>
    [CreateAssetMenu(fileName = "EmotionData_", menuName = "CTRL-Heart/Emotion Data")]
    public class EmotionData : ScriptableObject
    {
        public CoreEmotion emotionType;
        public string displayName;
        public Color emotionColor = Color.white;
        public Sprite unitSprite;

        [Header("Movement & Cooldown")]
        public float moveSpeed = 5f;
        public float actionCooldown = 1.5f;

        [Header("Impact")]
        [Tooltip("Influence percent applied per second or per application")]
        public float influenceStrength = 20f;
    }
}
