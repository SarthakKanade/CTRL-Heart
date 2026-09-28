using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CtrlHeart.Core.Data;
using CtrlHeart.Core.Visuals;

namespace CtrlHeart.Core.UI
{
    /// <summary>
    /// UI representation of an internal organ node (Brain, Voice, Heart, Body, Lungs).
    /// Matches the Bioluminescent Control Room reference:
    /// - Glowing circular node ring with organ icon
    /// - Mini sub-meter (e.g. Focus Generation / health)
    /// - Status Pill Badge (e.g. ⚠️ Panic, ⚠️ Voice Pressure, ⚠️ Heart Flutter, ⚠️ Sweat Surge, ⚠️ Strained)
    /// - Primary Target Node glow ring indicating the answer-driving organ
    /// - Dominant emotion readout
    /// </summary>
    public class UINodeView : MonoBehaviour, IPointerClickHandler, IDropHandler
    {
        public InternalNodeType nodeType;

        [Header("Components")]
        [SerializeField] private Image outerRingImage;
        [SerializeField] private Image innerDiscImage;
        [SerializeField] private Image organIconImage;
        [SerializeField] private Text nodeNameText;
        [SerializeField] private Image healthBarFill;
        [SerializeField] private Text subMeterLabel;
        [SerializeField] private Image alertBadgeImage;
        [SerializeField] private Text alertBadgeText;
        [SerializeField] private Image targetCrownGlow;
        [SerializeField] private GameObject targetBadgeGO;
        [SerializeField] private Text targetBadgeText;
        [SerializeField] private Text dominantEmotionBadge;

        private float currentHealth = 100f;
        private bool isPrimaryTarget = false;
        private bool isThreatActive = false;

        public RectTransform RectTransform => (RectTransform)transform;

        public void Initialize(InternalNodeType type)
        {
            nodeType = type;

            if (nodeNameText != null)
            {
                nodeNameText.text = type.ToString().ToUpper();
                nodeNameText.color = VisualTheme.ColorGoldAccent;
            }

            Color themeColor = VisualTheme.GetNodeColor(type);

            if (outerRingImage != null)
            {
                outerRingImage.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_square_brown");
                outerRingImage.type = Image.Type.Sliced;
                outerRingImage.color = Color.white;
            }

            if (innerDiscImage != null)
            {
                innerDiscImage.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_inset_brown");
                innerDiscImage.type = Image.Type.Sliced;
                innerDiscImage.color = Color.white;
            }

            if (organIconImage != null)
            {
                string iconKey = type switch
                {
                    InternalNodeType.Brain => "icon_brain",
                    InternalNodeType.Voice => "icon_voice",
                    InternalNodeType.Heart => "icon_heart",
                    InternalNodeType.Body => "icon_body",
                    InternalNodeType.Lungs => "icon_lungs",
                    _ => "circle_glow"
                };
                organIconImage.sprite = UIProceduralTextureGenerator.GetSprite(iconKey);
                organIconImage.color = Color.white;
            }

            if (subMeterLabel != null)
            {
                subMeterLabel.text = type switch
                {
                    InternalNodeType.Brain => "• Cognition",
                    InternalNodeType.Voice => "• Articulation",
                    InternalNodeType.Heart => "• Rhythm",
                    InternalNodeType.Body => "• Stability",
                    InternalNodeType.Lungs => "• Oxygen Flow",
                    _ => "• Status"
                };
            }

            SetAlertBadge("Stable", false, "");
            SetPrimaryTarget(false);
            UpdateDisplay(100f, EmotionState.FrozenBlank, SocialEffectType.StressEvent, false);
        }

        public void AssignComponents(Image ring, Image disc, Image icon, Text name, Image hpFill, Image statusBg, Text statusTxt, Image crown, GameObject badgeGO = null, Text badgeTxt = null)
        {
            outerRingImage = ring;
            innerDiscImage = disc;
            organIconImage = icon;
            nodeNameText = name;
            healthBarFill = hpFill;
            alertBadgeImage = statusBg;
            alertBadgeText = statusTxt;
            targetCrownGlow = crown;
            targetBadgeGO = badgeGO;
            targetBadgeText = badgeTxt;
        }

        public void UpdateDisplay(float health, EmotionState dominant, SocialEffectType activeEffect, bool isEffectActive)
        {
            currentHealth = health;

            if (healthBarFill != null)
            {
                healthBarFill.fillAmount = Mathf.Clamp01(health / 100f);
                healthBarFill.color = health < 30f ? VisualTheme.ColorNodeCritical : VisualTheme.GetNodeColor(nodeType);
            }

            if (!isThreatActive && alertBadgeText != null)
            {
                string domString = dominant == EmotionState.FrozenBlank ? "Stable" : dominant.ToString();
                alertBadgeText.text = domString;
                alertBadgeText.color = VisualTheme.ColorParchmentText;

                if (alertBadgeImage != null)
                {
                    alertBadgeImage.color = Color.white;
                }
            }

            if (innerDiscImage != null)
            {
                if (isEffectActive)
                {
                    Color pulseCol = activeEffect == SocialEffectType.StressEvent
                        ? VisualTheme.ColorStressThreat
                        : VisualTheme.ColorEmotionPushGlow;
                    innerDiscImage.color = new Color(pulseCol.r, pulseCol.g, pulseCol.b, 0.75f);
                }
                else
                {
                    innerDiscImage.color = Color.white;
                }
            }
        }

        public void SetPrimaryTarget(bool isTarget)
        {
            isPrimaryTarget = isTarget;
            if (targetCrownGlow != null)
            {
                targetCrownGlow.gameObject.SetActive(isTarget);
                if (isTarget)
                {
                    targetCrownGlow.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_square_beige");
                    targetCrownGlow.type = Image.Type.Sliced;
                    targetCrownGlow.color = new Color(1.0f, 0.85f, 0.2f, 0.95f);
                }
            }

            if (targetBadgeGO != null)
            {
                targetBadgeGO.SetActive(isTarget);
            }
        }

        public void SetAlertBadge(string alertText, bool isThreat, string subtypeTag)
        {
            isThreatActive = isThreat;

            if (alertBadgeText != null)
            {
                alertBadgeText.text = isThreat ? $"⚠ {alertText}" : alertText;
                alertBadgeText.color = isThreat ? Color.white : VisualTheme.ColorParchmentText;
            }

            if (alertBadgeImage != null)
            {
                alertBadgeImage.sprite = UIProceduralTextureGenerator.GetSprite(isThreat ? "rpg_button_long_brown" : "rpg_panel_inset_brown");
                alertBadgeImage.type = Image.Type.Sliced;
                alertBadgeImage.color = isThreat
                    ? new Color(0.85f, 0.25f, 0.15f, 1f) // Crimson alert
                    : Color.white;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // Direct click interaction to inject selected emotion
            if (CoreGameUI.SelectedEmotion.HasValue)
            {
                GameManager.Instance?.HandleEmotionDrop(nodeType, CoreGameUI.SelectedEmotion.Value);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            var draggable = eventData.pointerDrag?.GetComponent<UIEmotionDraggable>();
            if (draggable != null)
            {
                GameManager.Instance?.HandleEmotionDrop(nodeType, draggable.emotionType);
            }
        }

        private void Update()
        {
            // Subtle idle breathing pulse for the organ
            if (organIconImage != null)
            {
                float pulseSpeed = isThreatActive ? 6f : 2f;
                float scale = 1.0f + Mathf.Sin(Time.time * pulseSpeed) * (isThreatActive ? 0.08f : 0.03f);
                organIconImage.transform.localScale = Vector3.one * scale;
            }

            // Target ring rotation/pulse if this is the primary answer organ
            if (isPrimaryTarget)
            {
                if (targetCrownGlow != null)
                {
                    targetCrownGlow.transform.Rotate(Vector3.forward, -25f * Time.deltaTime);
                    float glowAlpha = 0.5f + Mathf.Sin(Time.time * 4f) * 0.3f;
                    targetCrownGlow.color = new Color(1.0f, 0.85f, 0.2f, glowAlpha);
                }

                if (targetBadgeGO != null)
                {
                    float badgeScale = 1.0f + Mathf.Sin(Time.time * 3.5f) * 0.05f;
                    targetBadgeGO.transform.localScale = new Vector3(badgeScale, badgeScale, 1f);
                }
            }
        }
    }
}
