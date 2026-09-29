using UnityEngine;
using CtrlHeart.Core.Data;

namespace CtrlHeart.Core.Visuals
{
    /// <summary>
    /// Master visual palette and style definitions locked for CTRL+HEART.
    /// Perfectly calibrated to the Bioluminescent Neurological Control Room aesthetic.
    /// </summary>
    public static class VisualTheme
    {
        // ── Emotion Palette (Warm Modern Fantasy Jewel Tones) ──
        public static readonly Color ColorCalm = new Color(0.18f, 0.72f, 0.82f, 1f);          // Serene Sea Glass
        public static readonly Color ColorAnxiety = new Color(0.92f, 0.25f, 0.20f, 1f);       // Ruby Flame
        public static readonly Color ColorConfidence = new Color(0.95f, 0.68f, 0.15f, 1f);     // Antique Gold
        public static readonly Color ColorAttraction = new Color(0.92f, 0.32f, 0.58f, 1f);   // Romantic Rose Petal

        // ── Event Visual Distinctives ──
        public static readonly Color ColorStressThreat = new Color(0.88f, 0.22f, 0.18f, 1f);   // Crimson Warning
        public static readonly Color ColorStressAmber = new Color(0.92f, 0.58f, 0.12f, 1f);    // Warm Amber
        public static readonly Color ColorEmotionPushGlow = new Color(0.98f, 0.82f, 0.35f, 1f);// Golden Bloom

        // ── Organ Node Identity Colors (Warm Biological Constellation) ──
        public static readonly Color ColorNodeBrain = new Color(0.72f, 0.52f, 0.88f, 1f);     // Amethyst Cognition
        public static readonly Color ColorNodeVoice = new Color(0.88f, 0.45f, 0.72f, 1f);     // Rose Resonance
        public static readonly Color ColorNodeHeart = new Color(0.95f, 0.30f, 0.42f, 1f);     // Crimson Pulse
        public static readonly Color ColorNodeBody  = new Color(0.30f, 0.78f, 0.65f, 1f);     // Jade Vitality
        public static readonly Color ColorNodeLungs = new Color(0.25f, 0.68f, 0.88f, 1f);     // Azure Breath

        public static readonly Color ColorNodeHealthy = new Color(0.24f, 0.16f, 0.13f, 0.95f); // Deep Walnut
        public static readonly Color ColorNodeBorder = new Color(0.78f, 0.62f, 0.38f, 1f);     // Warm Brass
        public static readonly Color ColorNodeCritical = new Color(0.88f, 0.18f, 0.18f, 1f);

        // ── Resource Bar Colors ──
        public static readonly Color ColorOxygen = new Color(0.18f, 0.75f, 0.60f, 1f);      // Emerald Flow
        public static readonly Color ColorFocus = new Color(0.70f, 0.45f, 0.90f, 1f);       // Lavender
        public static readonly Color ColorComposure = new Color(0.92f, 0.28f, 0.38f, 1f);   // Heart Crimson
        public static readonly Color ColorConnection = new Color(0.95f, 0.68f, 0.18f, 1f);  // Radiant Gold Connection

        // ── Environment Paneling & Warm RPG Wood / Light Parchment ──
        public static readonly Color ColorGoldAccent = new Color(1.0f, 0.95f, 0.65f, 1f);       // Radiant Crisp Bright Gold
        public static readonly Color ColorParchmentText = new Color(1.0f, 1.0f, 1.0f, 1f);     // Pure Crisp White
        public static readonly Color ColorBackgroundDeep = new Color(0.13f, 0.08f, 0.07f, 1f);    // Deep Walnut Espresso #211512
        public static readonly Color ColorPanelGlass = new Color(0.24f, 0.16f, 0.13f, 0.95f);     // Warm Mahogany RPG Wood
        public static readonly Color ColorPanelBorder = new Color(0.78f, 0.64f, 0.38f, 0.90f);    // Antique Gold / Brass Filigree
        public static readonly Color ColorCardInner = new Color(0.28f, 0.18f, 0.14f, 0.95f);      // Warm Leather Wood
        public static readonly Color ColorParchment = new Color(0.97f, 0.94f, 0.88f, 1f);        // Warm Cream Light Parchment

        // Top Half: Romantic Café Twilight
        public static readonly Color ColorDateBackground = new Color(0.15f, 0.10f, 0.12f, 1f);   // Warm Rosewood Ambiance
        public static readonly Color ColorDateBox = new Color(0.97f, 0.94f, 0.88f, 0.98f);       // Light Parchment Dialogue Box
        public static readonly Color ColorDateText = new Color(0.16f, 0.10f, 0.08f, 1f);         // Crisp Dark Walnut Ink
        public static readonly Color ColorPlayerAnswerText = new Color(0.12f, 0.28f, 0.65f, 1f); // Royal Indigo Ink
        public static readonly Color ColorDateReactionText = new Color(0.12f, 0.45f, 0.20f, 1f); // Forest Emerald Ink

        // Bottom Half: Living Alchemical Hearth
        public static readonly Color ColorInternalBackground = new Color(0.13f, 0.08f, 0.07f, 1f); // Deep Warm Mahogany
        public static readonly Color ColorDivider = new Color(0.85f, 0.70f, 0.35f, 0.95f);        // Antique Gold Filigree Bar
        public static readonly Color ColorPathwayGlow = new Color(0.90f, 0.72f, 0.42f, 0.75f);     // Golden Ley-Lines

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

        public static Color GetNodeColor(InternalNodeType node)
        {
            return node switch
            {
                InternalNodeType.Brain => ColorNodeBrain,
                InternalNodeType.Voice => ColorNodeVoice,
                InternalNodeType.Heart => ColorNodeHeart,
                InternalNodeType.Body => ColorNodeBody,
                InternalNodeType.Lungs => ColorNodeLungs,
                _ => Color.white
            };
        }

        private static Font cachedPixelFont;

        public static Font GetPixelFont()
        {
            if (cachedPixelFont != null) return cachedPixelFont;

#if UNITY_EDITOR
            cachedPixelFont = UnityEditor.AssetDatabase.LoadAssetAtPath<Font>("Assets/Sprout Lands - UI Pack - Basic pack/fonts/pixelFont-7-8x14-sproutLands.ttf");
#endif
            if (cachedPixelFont == null)
            {
                cachedPixelFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf") ?? Resources.GetBuiltinResource<Font>("Arial.ttf");
            }
            return cachedPixelFont;
        }

        public static string SanitizeText(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input
                .Replace('“', '"')
                .Replace('”', '"')
                .Replace('‘', '\'')
                .Replace('’', '\'')
                .Replace('—', '-')
                .Replace('–', '-')
                .Replace('•', '-')
                .Replace('★', '*')
                .Replace('⚠', '!');
        }

        /// <summary>
        /// Formats dialogue strings into rich text distinguishing verbal spoken dialogue (bold ink in quotes)
        /// from physical actions, gestures, and narrative descriptions (warm sepia italics).
        /// </summary>
        public static string FormatDialogue(string input, bool isPlayer = false)
        {
            if (string.IsNullOrEmpty(input)) return input;
            string clean = SanitizeText(input).Trim();

            // Rich text palette on parchment paper:
            // Verbal: Dark Walnut (#1E130D) for Date, Royal Indigo (#1D2B53) for Player
            // Action/Narrative: Warm Terracotta Sepia (#7C4D32)
            string spokenColor = isPlayer ? "#1D2B53" : "#1E130D";
            string actionColor = "#7C4D32";

            // If text contains neither quotes nor asterisks, treat the entire string as spoken dialogue
            if (!clean.Contains("\"") && !clean.Contains("*"))
            {
                return $"<color={spokenColor}><b>\"{clean}\"</b></color>";
            }

            // Regex matches either quoted speech ("...") or asterisked actions (*...*)
            var regex = new System.Text.RegularExpressions.Regex("(\"[^\"]*\")|(\\*[^*]*\\*)");
            var matches = regex.Matches(clean);

            var sb = new System.Text.StringBuilder();
            int lastEnd = 0;

            foreach (System.Text.RegularExpressions.Match m in matches)
            {
                if (m.Index > lastEnd)
                {
                    string narrative = clean.Substring(lastEnd, m.Index - lastEnd).Trim();
                    if (!string.IsNullOrEmpty(narrative))
                    {
                        if (sb.Length > 0) sb.Append(" ");
                        sb.Append($"<color={actionColor}><i>{narrative}</i></color>");
                    }
                }

                if (m.Groups[1].Success) // Quoted speech
                {
                    if (sb.Length > 0) sb.Append(" ");
                    sb.Append($"<color={spokenColor}><b>{m.Groups[1].Value}</b></color>");
                }
                else if (m.Groups[2].Success) // Asterisked action
                {
                    if (sb.Length > 0) sb.Append(" ");
                    sb.Append($"<color={actionColor}><i>{m.Groups[2].Value}</i></color>");
                }

                lastEnd = m.Index + m.Length;
            }

            if (lastEnd < clean.Length)
            {
                string narrative = clean.Substring(lastEnd).Trim();
                if (!string.IsNullOrEmpty(narrative))
                {
                    if (sb.Length > 0) sb.Append(" ");
                    sb.Append($"<color={actionColor}><i>{narrative}</i></color>");
                }
            }

            return sb.ToString();
        }
    }
}
