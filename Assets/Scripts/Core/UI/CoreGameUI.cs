using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using CtrlHeart.Core.Data;
using CtrlHeart.Core.Visuals;

namespace CtrlHeart.Core.UI
{
    /// <summary>
    /// Master UI Manager for CTRL+HEART.
    /// Manages the complete layout matching the Bioluminescent Control Room UI:
    /// - Top Half: Date Dialogue, Player Response, Date Reaction, Phase status & Timer
    /// - Left Panel: 4 Emotion Cards (Calm, Anxiety, Confidence, Attraction)
    /// - Center Biome: 5 Organ Nodes (Brain, Voice, Heart, Body, Lungs), Neural Pathways & Traveling Impulses
    /// - Center Event Banner: Real-time threat & effect ticker explaining exactly what is changing
    /// - Right Panel: Internal Resources & States (Oxygen, Focus, Composure, Connection)
    /// - Bottom Bar: Real-time Heart Rate Monitor (ECG wave & dynamic BPM)
    /// </summary>
    public class CoreGameUI : MonoBehaviour
    {
        public static CoreGameUI Instance { get; private set; }
        public static CoreEmotion? SelectedEmotion { get; private set; } = null;

        [Header("Top Half - Date View")]
        [SerializeField] private Text roundCounterText;
        [SerializeField] private Text tierBadgeText;
        [SerializeField] private Text phaseStatusText;
        [SerializeField] private Image phaseStatusBG;
        [SerializeField] private Text dialoguePromptText;
        [SerializeField] private GameObject playerAnswerContainer;
        [SerializeField] private Text spokenAnswerText;
        [SerializeField] private Text playerToneText;
        [SerializeField] private GameObject dateReactionContainer;
        [SerializeField] private Text dateReactionText;
        [SerializeField] private Text connectionDeltaFloatingText;
        [SerializeField] private Image responseTimerBar;
        [SerializeField] private Text timerSecondsText;
        [SerializeField] private DateCharacterVisuals dateVisuals;

        [Header("Unified Horizontal Dialogue Box")]
        [SerializeField] private GameObject unifiedDialogueBox;
        [SerializeField] private Text speakerNameBadge;
        [SerializeField] private Text dialogueBodyText;
        [SerializeField] private Text dialogueMetaSubtext;

        [Header("Autonomic Equilibrium Gauge (RTS Balance Mechanic)")]
        [SerializeField] private GameObject equilibriumGaugeContainer;
        [SerializeField] private Image equilibriumGaugeFill;
        [SerializeField] private Text equilibriumStatusText;

        [Header("Center - Event Ticker & Feedback")]
        [SerializeField] private GameObject eventBannerGO;
        [SerializeField] private Text eventHeadlineText;
        [SerializeField] private Text eventDetailsText;
        [SerializeField] private Text projectedFitText;

        [Header("Bottom Half - Internal World")]
        [SerializeField] private UIResourceBars resourceBars;
        [SerializeField] private UIHeartRateMonitor heartRateMonitor;
        [SerializeField] private UINeuralPathways neuralPathways;
        [SerializeField] private UIFloatingFeedback floatingFeedback;

        private readonly Dictionary<InternalNodeType, UINodeView> nodeViews = new Dictionary<InternalNodeType, UINodeView>();
        private readonly Dictionary<CoreEmotion, UIEmotionDraggable> emotionDraggables = new Dictionary<CoreEmotion, UIEmotionDraggable>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            EnsureInputModule();
            EnsureCameraBinding();
            AutoDiscoverHierarchyComponents();
        }

        public void AutoDiscoverHierarchyComponents()
        {
            if (nodeViews.Count == 0)
            {
                var foundNodes = GetComponentsInChildren<UINodeView>(true);
                foreach (var nv in foundNodes)
                {
                    nodeViews[nv.nodeType] = nv;
                }
            }

            if (emotionDraggables.Count == 0)
            {
                var foundDraggables = GetComponentsInChildren<UIEmotionDraggable>(true);
                foreach (var ed in foundDraggables)
                {
                    emotionDraggables[ed.emotionType] = ed;
                }
            }
        }

        private void EnsureCameraBinding()
        {
            var canvas = GetComponent<Canvas>();
            if (canvas != null)
            {
                var cam = Camera.main ?? FindFirstObjectByType<Camera>();
                if (cam != null)
                {
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvas.worldCamera = cam;
                    canvas.planeDistance = 10f;
                }
            }
        }

        private void EnsureInputModule()
        {
            var eventSystem = FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>();
            if (eventSystem == null)
            {
                var esGO = new GameObject("EventSystem");
                eventSystem = esGO.AddComponent<UnityEngine.EventSystems.EventSystem>();
                esGO.AddComponent<InputSystemUIInputModule>();
            }
            else if (eventSystem.GetComponent<InputSystemUIInputModule>() == null)
            {
                var legacyModule = eventSystem.GetComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                if (legacyModule != null) DestroyImmediate(legacyModule);
                eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
            }
        }

        public static void SelectEmotion(CoreEmotion emotion)
        {
            SelectedEmotion = emotion;
            if (Instance != null)
            {
                foreach (var kvp in Instance.emotionDraggables)
                {
                    kvp.Value.SetSelected(kvp.Key == emotion);
                }
            }
        }

        public void AssignTopPanel(
            Text roundText, Text tierText, Text phaseText, Image phaseBG,
            Text promptText, GameObject answerBox, Text answerText, Text toneText,
            GameObject reactionBox, Text reactText, Text deltaText,
            Image timerBar, Text timerText, DateCharacterVisuals visuals)
        {
            roundCounterText = roundText;
            tierBadgeText = tierText;
            phaseStatusText = phaseText;
            phaseStatusBG = phaseBG;
            dialoguePromptText = promptText;
            playerAnswerContainer = answerBox;
            spokenAnswerText = answerText;
            playerToneText = toneText;
            dateReactionContainer = reactionBox;
            dateReactionText = reactText;
            connectionDeltaFloatingText = deltaText;
            responseTimerBar = timerBar;
            timerSecondsText = timerText;
            dateVisuals = visuals;
        }

        public void AssignUnifiedDialogue(GameObject box, Text speaker, Text body, Text subtext, GameObject eqContainer, Image eqFill, Text eqStatus)
        {
            unifiedDialogueBox = box;
            speakerNameBadge = speaker;
            dialogueBodyText = body;
            dialogueMetaSubtext = subtext;
            equilibriumGaugeContainer = eqContainer;
            equilibriumGaugeFill = eqFill;
            equilibriumStatusText = eqStatus;
        }

        public void AssignEventBanner(GameObject bannerGO, Text headline, Text details, Text projected)
        {
            eventBannerGO = bannerGO;
            eventHeadlineText = headline;
            eventDetailsText = details;
            projectedFitText = projected;
        }

        public void AssignBottomSystems(UIResourceBars bars, UIHeartRateMonitor heart, UINeuralPathways pathways, UIFloatingFeedback feedback)
        {
            resourceBars = bars;
            heartRateMonitor = heart;
            neuralPathways = pathways;
            floatingFeedback = feedback;
        }

        public void RegisterNodeView(InternalNodeType type, UINodeView view)
        {
            nodeViews[type] = view;
        }

        public void RegisterEmotionDraggable(CoreEmotion emotion, UIEmotionDraggable draggable)
        {
            emotionDraggables[emotion] = draggable;
        }

        public UINodeView GetNodeView(InternalNodeType type)
        {
            if (nodeViews.Count == 0) AutoDiscoverHierarchyComponents();
            return nodeViews.TryGetValue(type, out var view) ? view : null;
        }

        // ═══════════════════════════════════════════════════════════════════
        // PHASE DISPLAY METHODS (SEQUENTIAL IN UNIFIED DIALOGUE BOX)
        // ═══════════════════════════════════════════════════════════════════

        public void SetupRtsPhase(int slotIndex, ConnectionTier tier, string prompt, float duration)
        {
            if (roundCounterText != null) roundCounterText.text = $"ROUND {slotIndex} / 10";
            if (tierBadgeText != null) tierBadgeText.text = tier.ToString().Replace("Tier", "TIER ").Replace("_", " - ");
            if (phaseStatusText != null) phaseStatusText.text = "PHASE 1: RTS STABILIZATION & INFLUENCE";
            if (phaseStatusBG != null) phaseStatusBG.color = new Color(0.1f, 0.45f, 0.8f, 0.85f);

            // Phase 1: Her Opening Scenario Beat in Unified Box
            if (speakerNameBadge != null)
            {
                speakerNameBadge.text = "DATE";
                speakerNameBadge.color = new Color(0.92f, 0.40f, 0.55f); // Romantic Rose
            }
            if (dialogueBodyText != null)
            {
                dialogueBodyText.text = prompt;
                dialogueBodyText.color = VisualTheme.ColorDateText; // Dark Walnut Ink
            }
            if (dialogueMetaSubtext != null) dialogueMetaSubtext.text = "";

            if (dialoguePromptText != null) dialoguePromptText.text = prompt;
            if (playerAnswerContainer != null && playerAnswerContainer != unifiedDialogueBox) playerAnswerContainer.SetActive(false);
            if (dateReactionContainer != null && dateReactionContainer != unifiedDialogueBox) dateReactionContainer.SetActive(false);
            if (projectedFitText != null && projectedFitText.transform.parent != null)
                projectedFitText.transform.parent.gameObject.SetActive(false);

            if (unifiedDialogueBox != null) unifiedDialogueBox.SetActive(true);
            if (equilibriumGaugeContainer != null) equilibriumGaugeContainer.SetActive(true);

            if (dateVisuals != null)
            {
                dateVisuals.PlayScenarioIntro(slotIndex, tier, prompt);
            }

            UpdateTimer(duration, duration, VisualTheme.ColorCalm);
        }

        public void SetupPlayerReplyPhase(string spokenAnswer, EmotionState emotionState, float duration)
        {
            if (phaseStatusText != null) phaseStatusText.text = "PHASE 2: YOUR RESPONSE";
            if (phaseStatusBG != null) phaseStatusBG.color = new Color(0.2f, 0.65f, 0.35f, 0.9f);

            // Phase 2: Player's Spoken Answer replaces previous in Unified Box
            if (speakerNameBadge != null)
            {
                speakerNameBadge.text = "YOU";
                speakerNameBadge.color = VisualTheme.ColorGoldAccent; // Radiant Antique Gold
            }
            if (dialogueBodyText != null)
            {
                dialogueBodyText.text = spokenAnswer;
                dialogueBodyText.color = VisualTheme.ColorPlayerAnswerText; // Royal Indigo Ink
            }
            if (dialogueMetaSubtext != null)
            {
                dialogueMetaSubtext.text = $"[Tone: {emotionState}]";
                dialogueMetaSubtext.color = new Color(0.40f, 0.30f, 0.25f, 0.9f);
            }

            if (projectedFitText != null && projectedFitText.transform.parent != null)
                projectedFitText.transform.parent.gameObject.SetActive(false);
            if (playerAnswerContainer != null && playerAnswerContainer != unifiedDialogueBox) playerAnswerContainer.SetActive(false);
            if (dateReactionContainer != null && dateReactionContainer != unifiedDialogueBox) dateReactionContainer.SetActive(false);
            if (unifiedDialogueBox != null) unifiedDialogueBox.SetActive(true);

            if (dateVisuals != null)
            {
                dateVisuals.PlayIdle();
            }

            UpdateTimer(duration, duration, VisualTheme.ColorGoldAccent);
        }

        public void SetupDateReactionPhase(string reactionText, string animationClipTag, float connectionDelta, float duration)
        {
            if (phaseStatusText != null) phaseStatusText.text = "PHASE 3: HER REACTION";
            if (phaseStatusBG != null) phaseStatusBG.color = new Color(0.8f, 0.35f, 0.1f, 0.9f);

            // Phase 3: Her Reaction replaces previous in Unified Box
            if (speakerNameBadge != null)
            {
                speakerNameBadge.text = "DATE";
                speakerNameBadge.color = new Color(0.92f, 0.40f, 0.55f); // Romantic Rose
            }
            if (dialogueBodyText != null)
            {
                dialogueBodyText.text = reactionText;
                dialogueBodyText.color = VisualTheme.ColorDateReactionText; // Forest Emerald Ink
            }

            string deltaString = connectionDelta >= 0 ? $"+{connectionDelta:0} CONNECTION" : $"{connectionDelta:0} CONNECTION";
            Color deltaColor = connectionDelta >= 0 ? new Color(0.12f, 0.52f, 0.22f) : new Color(0.80f, 0.22f, 0.18f);
            if (dialogueMetaSubtext != null)
            {
                dialogueMetaSubtext.text = deltaString;
                dialogueMetaSubtext.color = deltaColor;
            }

            if (projectedFitText != null && projectedFitText.transform.parent != null)
                projectedFitText.transform.parent.gameObject.SetActive(false);
            if (playerAnswerContainer != null && playerAnswerContainer != unifiedDialogueBox) playerAnswerContainer.SetActive(false);
            if (dateReactionContainer != null && dateReactionContainer != unifiedDialogueBox) dateReactionContainer.SetActive(false);
            if (unifiedDialogueBox != null) unifiedDialogueBox.SetActive(true);

            if (dateVisuals != null) dateVisuals.PlayReaction(animationClipTag, connectionDelta, reactionText);

            UpdateTimer(duration, duration, VisualTheme.ColorDateReactionText);
        }

        public void UpdateEquilibriumMeter(float score)
        {
            if (equilibriumGaugeContainer != null) equilibriumGaugeContainer.SetActive(true);

            float fill = Mathf.Clamp01(score / 100f);
            if (equilibriumGaugeFill != null)
            {
                equilibriumGaugeFill.fillAmount = fill;
                if (score >= 70f)
                    equilibriumGaugeFill.color = Color.white; // High Resonance
                else if (score >= 40f)
                    equilibriumGaugeFill.color = new Color(1f, 0.85f, 0.5f); // Active Tension
                else
                    equilibriumGaugeFill.color = new Color(1f, 0.5f, 0.45f); // Critical
            }

            if (equilibriumStatusText != null)
            {
                if (score >= 75f)
                    equilibriumStatusText.text = $"[EQUILIBRIUM: {score:0}% — RESONANT FLOW]";
                else if (score >= 45f)
                    equilibriumStatusText.text = $"[EQUILIBRIUM: {score:0}% — ACTIVE TENSION]";
                else
                    equilibriumStatusText.text = $"[EQUILIBRIUM: {score:0}% — CRITICAL TURBULENCE]";
            }
        }

        public void UpdateTimer(float remaining, float maxDuration, Color fillCol)
        {
            float fill = maxDuration > 0f ? Mathf.Clamp01(remaining / maxDuration) : 0f;
            if (responseTimerBar != null)
            {
                responseTimerBar.fillAmount = fill;
                responseTimerBar.color = fillCol;
            }
            if (timerSecondsText != null)
            {
                timerSecondsText.text = $"{remaining:F1}s";
            }
        }

        // ═══════════════════════════════════════════════════════════════════
        // RTS EVENT & LIVE UNDERSTANDING
        // ═══════════════════════════════════════════════════════════════════

        public void DisplayEventDetails(InternalNodeType primaryNode, SocialEventProfile profile)
        {
            if (nodeViews.Count == 0) AutoDiscoverHierarchyComponents();

            // Highlight primary answer node and reset badges
            foreach (var kvp in nodeViews)
            {
                kvp.Value.SetPrimaryTarget(kvp.Key == primaryNode);
                kvp.Value.SetAlertBadge("Stable", false, "");
            }

            if (profile == null || profile.effects == null || profile.effects.Count == 0)
            {
                if (eventHeadlineText != null) eventHeadlineText.text = "✓ CALM BEAT: No Active Stress Threats";
                if (eventDetailsText != null) eventDetailsText.text = $"Primary Answer Organ: {primaryNode}. Freely guide emotional tone.";
                return;
            }

            var primaryEffect = profile.effects[0];
            string headline = "";
            string details = "";

            if (primaryEffect.effectType == SocialEffectType.StressEvent)
            {
                string subtypeFormatted = FormatSubtypeName(primaryEffect.stressEventSubtype);
                headline = $"⚠ SOCIAL THREAT: {subtypeFormatted.ToUpper()} ON {primaryEffect.targetNode.ToString().ToUpper()}";
                details = $"Impact: -{primaryEffect.magnitude:0} Health | Draining Composure. Answer relies on {primaryNode}!";

                // Update target node alert badge with glowing warning
                var targetView = GetNodeView(primaryEffect.targetNode);
                if (targetView != null)
                {
                    targetView.SetAlertBadge(subtypeFormatted, true, subtypeFormatted);
                }
            }
            else
            {
                string emotionName = primaryEffect.pushTargetEmotion.ToString().ToUpper();
                headline = $"✨ OPPORTUNITY: {emotionName} PUSH ON {primaryEffect.targetNode.ToString().ToUpper()}";
                details = $"Impact: +{primaryEffect.magnitude:0} {primaryEffect.pushTargetEmotion} influence & Composure lift!";

                var targetView = GetNodeView(primaryEffect.targetNode);
                if (targetView != null)
                {
                    targetView.SetAlertBadge($"{primaryEffect.pushTargetEmotion} Push", false, "");
                }
            }

            if (eventHeadlineText != null) eventHeadlineText.text = headline;
            if (eventDetailsText != null) eventDetailsText.text = details;
        }

        private static string FormatSubtypeName(StressEventSubtype subtype)
        {
            return subtype switch
            {
                StressEventSubtype.AwkwardSilence => "Awkward Silence",
                StressEventSubtype.HeartFlutter => "Heart Flutter",
                StressEventSubtype.SweatSurge => "Sweat Surge",
                StressEventSubtype.Overthinking => "Overthinking",
                StressEventSubtype.Panic => "Panic",
                _ => subtype.ToString()
            };
        }

        public void UpdateProjectedAnswerPreview(EmotionState dominantState, string projectedAnswerPreview, float estimatedDelta, bool isCriticalBody = false)
        {
            if (projectedFitText != null)
            {
                string deltaStr = estimatedDelta >= 0 ? $"+{estimatedDelta:0}" : $"{estimatedDelta:0}";
                string stateStr = dominantState == EmotionState.FrozenBlank ? "Frozen / Blank" : dominantState.ToString();
                string quoteStr = !string.IsNullOrEmpty(projectedAnswerPreview) ? $"“{projectedAnswerPreview.Trim('“', '”', '\"')}”" : "...";

                string statusWarning = "";
                if (isCriticalBody)
                {
                    statusWarning = " | <color=#EF4444>⚠ BODY CRITICAL: Tremor Lock!</color>";
                }

                projectedFitText.text = $"<color=#FDE047>{quoteStr}</color>\n<size=13>Tone: <color=#38BDF8>{stateStr}</color> | Est. Delta: <color=#4ADE80>{deltaStr}</color>{statusWarning}</size>";
            }
        }

        public void OnEmotionInjected(InternalNodeType targetNode, CoreEmotion emotion)
        {
            neuralPathways?.LaunchTargetedImpulse(targetNode, emotion);

            var view = GetNodeView(targetNode);
            if (view != null && floatingFeedback != null)
            {
                string tag = emotion switch
                {
                    CoreEmotion.Calm => "+CALM (Healed)",
                    CoreEmotion.Anxiety => "+ANXIETY (Intercept)",
                    CoreEmotion.Confidence => "+CONFIDENCE (+Resolve)",
                    CoreEmotion.Attraction => "+ATTRACTION (Warmth)",
                    _ => $"+{emotion}"
                };
                floatingFeedback.SpawnText(view.RectTransform.position + new Vector3(0, 40, 0), tag, VisualTheme.GetEmotionColor(emotion));
            }
        }

        public void ShowFloatingMessage(Vector3 position, string message, Color color)
        {
            if (floatingFeedback != null)
            {
                floatingFeedback.SpawnText(position + new Vector3(0, 40, 0), message, color);
            }
        }

        public void UpdateResourceBars(ResourceState state)
        {
            if (resourceBars != null) resourceBars.UpdateBars(state);
            if (heartRateMonitor != null) heartRateMonitor.SetComposure(state.composure);
        }

        public void SetDateExpression(string animationClipTag)
        {
            if (dateVisuals != null) dateVisuals.SetExpressionFromTag(animationClipTag);
        }
    }
}
