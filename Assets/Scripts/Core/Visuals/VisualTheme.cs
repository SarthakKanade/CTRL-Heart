using UnityEngine;
using CtrlHeart.Core.Data;

namespace CtrlHeart.Core.Visuals
{
    /// <summary>
    /// Master visual palette and style definitions locked for Day 1.
    /// Master Design Bible Part 1 §1.6 & Dev Plan Day 1 Step 3.
    /// </summary>
    public static class VisualTheme
    {
        // ── Emotion Palette ──
        // Calm: soothing cyan/teal
        public static readonly Color ColorCalm = new Color(0.2f, 0.75f, 0.95f, 1f);
        // Anxiety: urgent bright orange-yellow
        public static readonly Color ColorAnxiety = new Color(1f, 0.65f, 0.1f, 1f);
        // Confidence: bold royal purple/indigo
        public static readonly Color ColorConfidence = new Color(0.6f, 0.35f, 0.95f, 1f);
        // Attraction: warm vibrant rose/pink
        public static readonly Color ColorAttraction = new Color(0.95f, 0.25f, 0.55f, 1f);

        // ── Event Visual Distinctives ──
        // Stress Event threat: warm/red pulse
        public static readonly Color ColorStressThreat = new Color(0.95f, 0.2f, 0.2f, 1f);
        // Direct Emotion Push opportunity: cool golden glow
        public static readonly Color ColorEmotionPushGlow = new Color(1f, 0.85f, 0.3f, 1f);

        // ── Node Base Colors ──
        public static readonly Color ColorNodeHealthy = new Color(0.18f, 0.22f, 0.3f, 1f);
        public static readonly Color ColorNodeBorder = new Color(0.35f, 0.42f, 0.55f, 1f);
        public static readonly Color ColorNodeCritical = new Color(0.85f, 0.25f, 0.25f, 1f);

        // ── Resource Bar Colors ──
        public static readonly Color ColorOxygen = new Color(0.25f, 0.8f, 0.7f, 1f);
        public static readonly Color ColorFocus = new Color(0.9f, 0.75f, 0.2f, 1f);
        public static readonly Color ColorComposure = new Color(0.95f, 0.3f, 0.35f, 1f);
        public static readonly Color ColorConnection = new Color(0.4f, 0.85f, 0.45f, 1f);

        // ── Environment Paneling ──
        // Top Half: Warm café ambience
        public static readonly Color ColorDateBackground = new Color(0.93f, 0.88f, 0.82f, 1f);
        public static readonly Color ColorDateBox = new Color(0.98f, 0.96f, 0.94f, 0.95f);
        public static readonly Color ColorDateText = new Color(0.18f, 0.15f, 0.14f, 1f);

        // Bottom Half: Stylized living control room
        public static readonly Color ColorInternalBackground = new Color(0.08f, 0.09f, 0.13f, 1f);
        public static readonly Color ColorDivider = new Color(0.25f, 0.3f, 0.4f, 1f);

        public static Color GetEmotionColor(CoreEmotion emotion)
        {
            return emotion switch
            {
                CoreEmotion.Calm => ColorCalm,
                CoreEmotion.Anxiety => ColorAnxiety,
                CoreEmotion.Confidence => ColorConfidence,
                CoreEmotion.Attraction => ColorAttraction,
                _ => Color.white
            };
        }
    }
}
