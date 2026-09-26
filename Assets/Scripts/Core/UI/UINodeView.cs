using UnityEngine;
using UnityEngine.UI;
using CtrlHeart.Core.Data;
using CtrlHeart.Core.Visuals;

namespace CtrlHeart.Core.UI
{
    /// <summary>
    /// UI representation of a single internal node on the mind map.
    /// Displays health, dominant emotion indicator, and active threat/push state.
    /// Master Design Bible Part 2 §2.3.
    /// </summary>
    public class UINodeView : MonoBehaviour
    {
        public InternalNodeType nodeType;

        [Header("Components")]
        [SerializeField] private Image backgroundCircle;
        [SerializeField] private Image healthRingFill;
        [SerializeField] private Image threatPulseGlow;
        [SerializeField] private Text nodeNameText;
        [SerializeField] private Text dominantStateText;

        public RectTransform RectTransform => (RectTransform)transform;

        public void Initialize(InternalNodeType type)
        {
            nodeType = type;
            if (nodeNameText != null) nodeNameText.text = type.ToString().ToUpper();
            if (backgroundCircle != null) backgroundCircle.color = VisualTheme.ColorNodeHealthy;
            if (threatPulseGlow != null) threatPulseGlow.gameObject.SetActive(false);
            UpdateDisplay(100f, EmotionState.Calm, SocialEffectType.StressEvent, false);
        }

        public void UpdateDisplay(float health, EmotionState dominant, SocialEffectType activeEffect, bool isEffectActive)
        {
            if (healthRingFill != null)
            {
                healthRingFill.fillAmount = Mathf.Clamp01(health / 100f);
                healthRingFill.color = health < 30f ? VisualTheme.ColorNodeCritical : VisualTheme.ColorNodeBorder;
            }

            if (dominantStateText != null)
            {
                dominantStateText.text = dominant.ToString();
            }

            if (threatPulseGlow != null)
            {
                threatPulseGlow.gameObject.SetActive(isEffectActive);
                if (isEffectActive)
                {
                    threatPulseGlow.color = activeEffect == SocialEffectType.StressEvent 
                        ? VisualTheme.ColorStressThreat 
                        : VisualTheme.ColorEmotionPushGlow;
                }
            }
        }
    }
}
