using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
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

        [Header("Pause System")]
        [SerializeField] private Button pauseButton;
        [SerializeField] private GameObject pauseOverlayPanel;
        [SerializeField] private Button pauseResumeButton;
        [SerializeField] private Button pauseMainMenuButton;

        [Header("End Game Results Overlay")]
        [SerializeField] private GameObject resultsOverlayPanel;
        [SerializeField] private Text resultsVerdictTitleText;
        [SerializeField] private Text resultsVerdictBodyText;
        [SerializeField] private Text resultsQuoteText;
        [SerializeField] private Text resultsConnectionScoreText;
        [SerializeField] private Image resultsConnectionBarFill;
        [SerializeField] private Text resultsComposureScoreText;
        [SerializeField] private Image resultsComposureBarFill;
        [SerializeField] private Text resultsTierBadgeText;
        [SerializeField] private Button resultsMainMenuButton;
        [SerializeField] private Button resultsPlayAgainButton;

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
            Time.timeScale = 1f;
            EnsureInputModule();
            EnsureCameraBinding();
            AutoDiscoverHierarchyComponents();
            BindOverlayButtons();
        }

        private void BindOverlayButtons()
        {
            if (pauseButton != null)
            {
                pauseButton.onClick.RemoveAllListeners();
                pauseButton.onClick.AddListener(PauseGame);
            }
            if (pauseResumeButton != null)
            {
                pauseResumeButton.onClick.RemoveAllListeners();
                pauseResumeButton.onClick.AddListener(ResumeGame);
            }
            if (pauseMainMenuButton != null)
            {
                pauseMainMenuButton.onClick.RemoveAllListeners();
                pauseMainMenuButton.onClick.AddListener(GoToMainMenu);
            }
            if (resultsMainMenuButton != null)
            {
                resultsMainMenuButton.onClick.RemoveAllListeners();
                resultsMainMenuButton.onClick.AddListener(GoToMainMenu);
            }
            if (resultsPlayAgainButton != null)
            {
                resultsPlayAgainButton.onClick.RemoveAllListeners();
                resultsPlayAgainButton.onClick.AddListener(RestartGame);
            }
        }

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (resultsOverlayPanel == null || !resultsOverlayPanel.activeSelf)
                {
                    TogglePause();
                }
            }
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
            bool promptIsAction = !string.IsNullOrEmpty(prompt) && prompt.Trim().StartsWith("*") && !prompt.Contains("\"") && !prompt.Contains("“");
            if (speakerNameBadge != null)
            {
                speakerNameBadge.text = promptIsAction ? "MAYA [ACTION]" : "MAYA";
                speakerNameBadge.color = new Color(0.92f, 0.40f, 0.55f); // Romantic Rose
            }
            if (dialogueBodyText != null)
            {
                dialogueBodyText.text = VisualTheme.FormatDialogue(prompt, isPlayer: false);
                dialogueBodyText.color = Color.white;
            }
            if (dialogueMetaSubtext != null) dialogueMetaSubtext.text = "";

            if (dialoguePromptText != null && dialoguePromptText != dialogueBodyText) dialoguePromptText.text = VisualTheme.SanitizeText(prompt);
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
            bool answerIsAction = !string.IsNullOrEmpty(spokenAnswer) && spokenAnswer.Trim().StartsWith("*") && !spokenAnswer.Contains("\"") && !spokenAnswer.Contains("“");
            if (speakerNameBadge != null)
            {
                speakerNameBadge.text = answerIsAction ? "YOU [ACTION]" : "YOU";
                speakerNameBadge.color = VisualTheme.ColorGoldAccent; // Radiant Antique Gold
            }
            if (dialogueBodyText != null)
            {
                dialogueBodyText.text = VisualTheme.FormatDialogue(spokenAnswer, isPlayer: true);
                dialogueBodyText.color = Color.white;
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
            bool reactionIsAction = !string.IsNullOrEmpty(reactionText) && !reactionText.Contains("\"") && !reactionText.Contains("“");
            if (speakerNameBadge != null)
            {
                speakerNameBadge.text = reactionIsAction ? "MAYA [REACTION]" : "MAYA";
                speakerNameBadge.color = new Color(0.92f, 0.40f, 0.55f); // Romantic Rose
            }
            if (dialogueBodyText != null)
            {
                dialogueBodyText.text = VisualTheme.FormatDialogue(reactionText, isPlayer: false);
                dialogueBodyText.color = Color.white;
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

        public void WireOverlays(
            Button pauseBtn,
            GameObject pausePanel,
            Button resumeBtn,
            Button pauseMenuBtn,
            GameObject resultsPanel,
            Text verdictTitle,
            Text verdictBody,
            Text quoteTxt,
            Text connScoreTxt,
            Image connBarFill,
            Text compScoreTxt,
            Image compBarFill,
            Text tierBadge,
            Button resultsMenuBtn,
            Button resultsRetryBtn)
        {
            pauseButton = pauseBtn;
            pauseOverlayPanel = pausePanel;
            pauseResumeButton = resumeBtn;
            pauseMainMenuButton = pauseMenuBtn;

            resultsOverlayPanel = resultsPanel;
            resultsVerdictTitleText = verdictTitle;
            resultsVerdictBodyText = verdictBody;
            resultsQuoteText = quoteTxt;
            resultsConnectionScoreText = connScoreTxt;
            resultsConnectionBarFill = connBarFill;
            resultsComposureScoreText = compScoreTxt;
            resultsComposureBarFill = compBarFill;
            resultsTierBadgeText = tierBadge;
            resultsMainMenuButton = resultsMenuBtn;
            resultsPlayAgainButton = resultsRetryBtn;

            BindOverlayButtons();
        }

        public void TogglePause()
        {
            if (Time.timeScale == 0f)
                ResumeGame();
            else
                PauseGame();
        }

        public void PauseGame()
        {
            Time.timeScale = 0f;
            if (pauseOverlayPanel != null)
                pauseOverlayPanel.SetActive(true);
        }

        public void ResumeGame()
        {
            Time.timeScale = 1f;
            if (pauseOverlayPanel != null)
                pauseOverlayPanel.SetActive(false);
        }

        public void GoToMainMenu()
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainDateScene");
        }

        public void ShowEndResults(EndingType ending, ResourceState finalResources)
        {
            if (resultsOverlayPanel == null) return;

            resultsOverlayPanel.SetActive(true);

            var endingData = EndingsData.GetEnding(ending);
            var tier = finalResources.GetConnectionTier();
            float conn = Mathf.Clamp(finalResources.connection, 0f, 100f);
            float comp = Mathf.Clamp(finalResources.composure, 0f, 100f);

            // Determine verdict narrative
            string verdictTitle;
            string verdictBody;

            if (ending == EndingType.SecondDate_Tier3_4)
            {
                if (conn >= 80f)
                {
                    verdictTitle = "SHE ASKED YOU TO COME OVER!";
                    verdictBody = "Maya leaned in with a warm, genuine smile: <i>\"I really don't want this evening to end yet... Want to come over to my place? I have that vinyl record we were talking about.\"</i>\n\n<color=#4ADE80><b>Verdict:</b> Exceptional romantic chemistry! You unlocked the deepest connection.</color>";
                }
                else
                {
                    verdictTitle = "SECOND DATE SECURED!";
                    verdictBody = "Maya smiled warmly, holding your gaze: <i>\"I had such a wonderful time tonight. Can we do this again this Saturday?\"</i>\n\n<color=#4ADE80><b>Verdict:</b> Mutual attraction & comfort! She wants to see you again.</color>";
                }
            }
            else if (ending == EndingType.Maybe_Tier2)
            {
                verdictTitle = "THE 'MAYBE' ZONE";
                verdictBody = "A sweet, lingering smile and a gentle hug goodbye at the station. She said she'd text you later - the door is gently open, but left uncertain.\n\n<color=#FBBF24><b>Verdict:</b> Pleasant rapport, but needs more boldness next time.</color>";
            }
            else if (ending == EndingType.AwkwardEnding_Tier1)
            {
                verdictTitle = "POLITE FAREWELL";
                verdictBody = "The conversation stayed polite, but the romantic spark never ignited. Maya waved goodbye with a soft smile and slipped into the evening crowd.\n\n<color=#F87171><b>Verdict:</b> Awkward friction hung between your words.</color>";
            }
            else if (ending == EndingType.Meltdown_ComposureZero)
            {
                verdictTitle = "AUTONOMIC MELTDOWN!";
                verdictBody = "Composure plummeted to zero! Heart rate spiked frantically, hands shook, and your internal control system tripped the emergency brake.\n\n<color=#EF4444><b>Verdict:</b> Overstimulated panic spiral caused an abrupt end to the date.</color>";
            }
            else // DateCollapse_Tier0
            {
                verdictTitle = "DATE COLLAPSED";
                verdictBody = "Connection dropped into icy silence. Maya found an early excuse to catch an early train home.\n\n<color=#EF4444><b>Verdict:</b> Emotional detachment severed all chemistry.</color>";
            }

            if (resultsVerdictTitleText != null)
            {
                resultsVerdictTitleText.text = VisualTheme.SanitizeText(verdictTitle);
                resultsVerdictTitleText.color = endingData.bannerColor;
            }

            if (resultsVerdictBodyText != null)
            {
                resultsVerdictBodyText.text = $"{VisualTheme.SanitizeText(verdictBody)}\n\n<color=#E2E8F0>{VisualTheme.SanitizeText(endingData.summary)}</color>";
            }

            if (resultsQuoteText != null)
            {
                resultsQuoteText.text = $"Maya: {VisualTheme.SanitizeText(endingData.dateFinalQuote)}";
            }

            if (resultsConnectionScoreText != null)
            {
                resultsConnectionScoreText.text = $"CONNECTION: {conn:F0} / 100";
            }

            if (resultsConnectionBarFill != null)
            {
                resultsConnectionBarFill.fillAmount = conn / 100f;
            }

            if (resultsComposureScoreText != null)
            {
                resultsComposureScoreText.text = $"COMPOSURE: {comp:F0} / 100";
            }

            if (resultsComposureBarFill != null)
            {
                resultsComposureBarFill.fillAmount = comp / 100f;
            }

            if (resultsTierBadgeText != null)
            {
                string tierName = tier switch
                {
                    ConnectionTier.Tier4_SecondDate => "Tier 4: Passionate Chemistry",
                    ConnectionTier.Tier3_Strong => "Tier 3: Mutual Spark",
                    ConnectionTier.Tier2_Maybe => "Tier 2: Polite Rapport",
                    ConnectionTier.Tier1_Awkward => "Tier 1: Awkward Friction",
                    _ => "Tier 0: Detached"
                };
                resultsTierBadgeText.text = $"OUTCOME: {tierName.ToUpper()}";
            }
        }
    }
}
