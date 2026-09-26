using System;
using UnityEngine;

namespace CtrlHeart.Core.Data.ScriptableObjects
{
    /// <summary>
    /// Represents an authored question for a specific slot and Connection tier.
    /// Master Design Bible Part 5 §5.3.
    /// </summary>
    [CreateAssetMenu(fileName = "Question_Slot_Tier", menuName = "CTRL-Heart/Question Data")]
    public class QuestionData : ScriptableObject
    {
        [Range(1, 10)]
        [Tooltip("Which slot (1-10) this question belongs to")]
        public int slotIndex = 1;

        [Tooltip("The Connection tier this question variant belongs to (Tier 1-4)")]
        public ConnectionTier connectionTier = ConnectionTier.Tier2_Maybe;

        [TextArea(2, 4)]
        public string questionText;

        [Header("Social Event Profile")]
        [Tooltip("The authored RTS disruption and target profile linked to this question")]
        public SocialEventProfile socialEventProfile = new SocialEventProfile();

        [Header("Response Window")]
        [Tooltip("Seconds the player has to respond before timer expiry")]
        public float responseTimeWindow = 5f;
    }
}
