using System;
using UnityEngine;

namespace CtrlHeart.Core.Data
{
    /// <summary>
    /// Holds the 4 continuous resource meters.
    /// Oxygen, Focus, Composure, Connection.
    /// Master Design Bible Part 2 §2.5.
    /// </summary>
    [Serializable]
    public class ResourceState
    {
        // ── Oxygen (Operational - Lungs) ──
        [Range(0f, 100f)] public float oxygen = 100f;
        public float oxygenBaseRegenRate = 5f; // per second at 100% Lungs health

        // ── Focus (Cognitive - Brain) ──
        [Range(0f, 100f)] public float focus = 70f;
        public float focusBaseRegenRate = 8f; // per second at 100% Brain health

        // ── Composure (Global stability, 0-100 - Heart) ──
        [Range(0f, 100f)] public float composure = 80f;

        // ── Connection (Relationship score, 0-100) ──
        [Range(0f, 100f)] public float connection = 50f;

        public const float MAX_VALUE = 100f;
        public const float MIN_VALUE = 0f;

        public ConnectionTier GetConnectionTier()
        {
            if (connection <= 0f) return ConnectionTier.Tier0_Collapse;
            if (connection < 40f) return ConnectionTier.Tier1_Awkward;
            if (connection < 70f) return ConnectionTier.Tier2_Maybe;
            if (connection < 90f) return ConnectionTier.Tier3_Strong;
            return ConnectionTier.Tier4_SecondDate;
        }

        public bool IsMeltdown() => composure <= 0f;
        public bool IsDateCollapsed() => connection <= 0f;
    }
}
