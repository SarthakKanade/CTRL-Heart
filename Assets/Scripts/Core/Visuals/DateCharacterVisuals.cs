using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CtrlHeart.Core.Visuals
{
    /// <summary>
    /// Archetype categories for the ~20 shared reaction animations.
    /// Master Design Bible Part 5 §5.5 & Dev Plan Day 5.
    /// </summary>
    public enum DateExpressionArchetype
    {
        Neutral,
        Smiling,
        Laughing,
        Confused,
        Surprised,
        Nervous,
        Concerned,
        Awkward,
        Flirty
    }

    /// <summary>
    /// Controls the Date character visual representation on the top panel.
    /// Manages expression swapping mapped to reaction animation tags,
    /// and subtle procedural idle breathing animation.
    /// Master Design Bible Part 1 §1.6 & Dev Plan Day 5 Priority 1 & 3.
    /// </summary>
    public class DateCharacterVisuals : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Image portraitImage;
        [SerializeField] private Image expressionOverlay;
        [SerializeField] private Text expressionBadgeText;

        [Header("Procedural Breathing")]
        [SerializeField] private bool enableBreathing = true;
        [SerializeField] private float breathSpeed = 1.8f;
        [SerializeField] private float breathScaleMagnitude = 0.025f;

        private RectTransform portraitRect;
        private Vector3 initialScale;
        private DateExpressionArchetype currentArchetype = DateExpressionArchetype.Neutral;

        private void Awake()
        {
            if (portraitImage != null)
            {
                portraitRect = portraitImage.GetComponent<RectTransform>();
                initialScale = portraitRect != null ? portraitRect.localScale : Vector3.one;
            }
        }

        private void Update()
        {
            if (enableBreathing && portraitRect != null)
            {
                float breath = Mathf.Sin(Time.time * breathSpeed) * breathScaleMagnitude;
                portraitRect.localScale = new Vector3(initialScale.x + breath, initialScale.y + breath, initialScale.z);
            }
        }

        public void SetExpressionFromTag(string animationClipTag)
        {
            var archetype = MapClipTagToArchetype(animationClipTag);
            SetExpression(archetype, animationClipTag);
        }

        public void SetExpression(DateExpressionArchetype archetype, string tagLabel = "")
        {
            currentArchetype = archetype;

            // Update procedural expression styling & badge
            if (expressionBadgeText != null)
            {
                expressionBadgeText.text = $"[Date: {archetype}]";
                expressionBadgeText.color = GetArchetypeColor(archetype);
            }

            if (portraitImage != null)
            {
                portraitImage.color = GetArchetypeTint(archetype);
            }

            Debug.Log($"[DateCharacter] Expression changed to: {archetype} (Tag: '{tagLabel}')");
        }

        public static DateExpressionArchetype MapClipTagToArchetype(string tag)
        {
            if (string.IsNullOrEmpty(tag)) return DateExpressionArchetype.Neutral;

            tag = tag.ToLowerInvariant();

            if (tag.Contains("laugh") || tag.Contains("chuckle") || tag.Contains("giggle"))
                return DateExpressionArchetype.Laughing;

            if (tag.Contains("flirt") || tag.Contains("blush") || tag.Contains("wink") || tag.Contains("hug"))
                return DateExpressionArchetype.Flirty;

            if (tag.Contains("smile") || tag.Contains("nod") || tag.Contains("sweet") || tag.Contains("polite"))
                return DateExpressionArchetype.Smiling;

            if (tag.Contains("surprised") || tag.Contains("startled") || tag.Contains("eyes"))
                return DateExpressionArchetype.Surprised;

            if (tag.Contains("confused") || tag.Contains("doubt") || tag.Contains("question"))
                return DateExpressionArchetype.Confused;

            if (tag.Contains("nervous") || tag.Contains("tense") || tag.Contains("worried"))
                return DateExpressionArchetype.Nervous;

            if (tag.Contains("concerned") || tag.Contains("soft"))
                return DateExpressionArchetype.Concerned;

            if (tag.Contains("awkward") || tag.Contains("flat") || tag.Contains("grin"))
                return DateExpressionArchetype.Awkward;

            return DateExpressionArchetype.Neutral;
        }

        private static Color GetArchetypeColor(DateExpressionArchetype archetype)
        {
            return archetype switch
            {
                DateExpressionArchetype.Smiling => new Color(0.3f, 0.8f, 0.4f, 1f),
                DateExpressionArchetype.Laughing => new Color(0.95f, 0.75f, 0.2f, 1f),
                DateExpressionArchetype.Flirty => new Color(0.95f, 0.35f, 0.6f, 1f),
                DateExpressionArchetype.Nervous => new Color(1f, 0.6f, 0.2f, 1f),
                DateExpressionArchetype.Concerned => new Color(0.35f, 0.65f, 0.95f, 1f),
                DateExpressionArchetype.Awkward => new Color(0.85f, 0.45f, 0.3f, 1f),
                DateExpressionArchetype.Surprised => new Color(0.9f, 0.85f, 0.25f, 1f),
                DateExpressionArchetype.Confused => new Color(0.7f, 0.5f, 0.85f, 1f),
                _ => new Color(0.7f, 0.7f, 0.7f, 1f)
            };
        }

        private static Color GetArchetypeTint(DateExpressionArchetype archetype)
        {
            // Subtle warm tint shifts reflecting emotional response
            return archetype switch
            {
                DateExpressionArchetype.Flirty => new Color(1f, 0.92f, 0.95f, 1f),
                DateExpressionArchetype.Laughing => new Color(1f, 0.98f, 0.9f, 1f),
                DateExpressionArchetype.Nervous => new Color(0.98f, 0.94f, 0.9f, 1f),
                DateExpressionArchetype.Concerned => new Color(0.94f, 0.96f, 1f, 1f),
                DateExpressionArchetype.Awkward => new Color(0.96f, 0.94f, 0.93f, 1f),
                _ => Color.white
            };
        }
    }
}
