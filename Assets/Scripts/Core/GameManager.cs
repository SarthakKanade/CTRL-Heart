using System;
using System.Collections.Generic;
using UnityEngine;
using CtrlHeart.Core.Data;
using CtrlHeart.Core.Data.ScriptableObjects;
using CtrlHeart.Core.Systems;
using CtrlHeart.Core.UI;
using CtrlHeart.Core.Visuals;
using CtrlHeart.Core.Testing;

namespace CtrlHeart.Core
{
    public enum SlotPhase
    {
        None,
        RtsIntervention, // 20 seconds: Scenario prompt + RTS effects live + player dragging/routing emotions
        PlayerReply,     // 5 seconds: RTS freezes, Player's spoken reply is highlighted
        DateReaction     // 5 seconds: Date character reacts with expression and quote, connection delta applies
    }

    /// <summary>
    /// Master Game Controller for CTRL+HEART.
    /// Orchestrates the disciplined 30-second slot loop:
    /// - Phase 1: RTS Scenario & Effects (20.0s)
    /// - Phase 2: Player Reply (5.0s)
    /// - Phase 3: Date Reaction (5.0s)
    /// Total = 30.0s per slot. Nothing is simultaneous; functions as a coherent, legible system.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Core Systems")]
        [SerializeField] private MindMapManager mindMap;
        [SerializeField] private ResourceManager resources;
        [SerializeField] private SocialEventEngine eventEngine;
        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private CoreGameUI ui;

        [Header("Phase Timing (Total 30s per slot)")]
        [SerializeField] private float rtsPhaseDuration = 20.0f;
        [SerializeField] private float playerReplyDuration = 5.0f;
        [SerializeField] private float dateReactionDuration = 5.0f;

        [Header("Current State")]
        [SerializeField] private int currentSlotIndex = 1;
        [SerializeField] private SlotPhase currentPhase = SlotPhase.None;
        [SerializeField] private float phaseTimer = 0f;
        [SerializeField] private QuestionData currentQuestion;
        [SerializeField] private AnswerData resolvedAnswer;
        [SerializeField] private ReactionData resolvedReaction;

        public SlotPhase CurrentPhase => currentPhase;
        public int CurrentSlotIndex => currentSlotIndex;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            Application.runInBackground = true;
        }

        private void Start()
        {
            InitializeGame();
        }

        public void InitializeGame()
        {
            if (mindMap == null) mindMap = GetComponent<MindMapManager>() ?? gameObject.AddComponent<MindMapManager>();
            if (resources == null) resources = GetComponent<ResourceManager>() ?? gameObject.AddComponent<ResourceManager>();
            if (eventEngine == null) eventEngine = GetComponent<SocialEventEngine>() ?? gameObject.AddComponent<SocialEventEngine>();
            if (dialogueManager == null) dialogueManager = GetComponent<DialogueManager>() ?? gameObject.AddComponent<DialogueManager>();
            if (ui == null) ui = FindFirstObjectByType<CoreGameUI>();

            mindMap.InitializeNodes();
            eventEngine.Initialize(mindMap, resources);

            // Register authored content (Full 10 Slots)
            var (questions, answers, reactions) = FullDateContent.CreateFull10SlotsContent();
            dialogueManager.RegisterContent(questions, answers, reactions);

            // Hook UI node updates
            mindMap.OnNodeChanged += OnNodeStateChanged;
            resources.OnResourceChanged += OnResourceStateChanged;

            // Start Slot 1
            StartSlot(1);
        }

        public void StartSlot(int slotIndex)
        {
            currentSlotIndex = slotIndex;
            resolvedAnswer = null;
            resolvedReaction = null;

            // Select question based on live Connection tier
            var currentTier = resources.CurrentState.GetConnectionTier();
            if (currentTier == ConnectionTier.Tier0_Collapse)
            {
                TriggerEnding(EndingType.DateCollapse_Tier0);
                return;
            }

            currentQuestion = dialogueManager.SelectQuestion(slotIndex, currentTier);
            if (currentQuestion == null)
            {
                Debug.LogWarning($"[GameManager] No question found for Slot {slotIndex} at {currentTier}");
                return;
            }

            // Start Phase 1: RTS Scenario & Effects (30.0s)
            currentPhase = SlotPhase.RtsIntervention;
            phaseTimer = rtsPhaseDuration;

            if (ui != null)
            {
                ui.SetupRtsPhase(slotIndex, currentTier, currentQuestion.questionText, rtsPhaseDuration);
                ui.DisplayEventDetails(currentQuestion.socialEventProfile.PrimaryTargetNode, currentQuestion.socialEventProfile);
            }

            // Trigger Question's Social Event Profile across target nodes
            eventEngine.TriggerSocialEvent(currentQuestion.socialEventProfile);
            RefreshProjectedAnswerPreview();

            Debug.Log($"<color=yellow>[GameManager] Slot {slotIndex} Phase 1 (RTS - {rtsPhaseDuration}s) Started: \"{currentQuestion.questionText}\"</color>");
        }

        private void Update()
        {
            if (currentPhase == SlotPhase.None) return;

            // Check game-over fail states
            if (resources.IsMeltdown())
            {
                currentPhase = SlotPhase.None;
                TriggerEnding(EndingType.Meltdown_ComposureZero);
                return;
            }
            if (resources.IsDateCollapsed())
            {
                currentPhase = SlotPhase.None;
                TriggerEnding(EndingType.DateCollapse_Tier0);
                return;
            }

            switch (currentPhase)
            {
                case SlotPhase.RtsIntervention:
                    UpdateRtsPhase();
                    break;

                case SlotPhase.PlayerReply:
                    UpdatePlayerReplyPhase();
                    break;

                case SlotPhase.DateReaction:
                    UpdateDateReactionPhase();
                    break;
            }
        }

        private void UpdateRtsPhase()
        {
            // Passive regen tick
            float lungsHealth = mindMap.GetNode(InternalNodeType.Lungs)?.currentHealth ?? 100f;
            float brainHealth = mindMap.GetNode(InternalNodeType.Brain)?.currentHealth ?? 100f;
            resources.TickPassiveRegen(Time.deltaTime, lungsHealth, brainHealth);

            // Real-time RTS crisis pressure & threat ticking
            eventEngine.TickActiveSocialEvent(Time.deltaTime);

            // Timer countdown
            phaseTimer -= Time.deltaTime;
            if (ui != null)
            {
                ui.UpdateTimer(Mathf.Max(0f, phaseTimer), rtsPhaseDuration, VisualTheme.ColorCalm);
                RefreshProjectedAnswerPreview();
            }

            // Expiry -> Transition to Phase 2: Player Reply
            if (phaseTimer <= 0f)
            {
                TransitionToPlayerReplyPhase();
            }
        }

        public void RefreshProjectedAnswerPreview()
        {
            UpdateRtsBalancingMeter();
        }

        public void UpdateRtsBalancingMeter()
        {
            if (ui == null || mindMap == null) return;
            float eq = mindMap.CalculateEquilibriumScore();
            ui.UpdateEquilibriumMeter(eq);
        }

        private void TransitionToPlayerReplyPhase()
        {
            currentPhase = SlotPhase.PlayerReply;
            phaseTimer = playerReplyDuration;

            // Stop RTS stress effects during conversational delivery
            eventEngine.StopSocialEvent();

            var primaryNode = currentQuestion.socialEventProfile.PrimaryTargetNode;
            var dominantState = mindMap.GetDominantState(primaryNode);
            bool isCriticalBody = mindMap.IsBodyCriticallyLow();

            resolvedAnswer = dialogueManager.ResolveAnswer(currentQuestion, dominantState, isCriticalBody);

            if (resolvedAnswer != null)
            {
                // Dynamic Composure connection: Delivery tone impacts player's internal composure
                float replyComposureDelta = resolvedAnswer.emotionState switch
                {
                    EmotionState.Confidence => 12f,
                    EmotionState.Calm => 10f,
                    EmotionState.Attraction => 8f,
                    EmotionState.Anxiety => -6f,
                    EmotionState.FrozenBlank => -12f,
                    _ => 4f
                };
                resources.ModifyComposure(replyComposureDelta);
            }

            if (ui != null && resolvedAnswer != null)
            {
                ui.SetupPlayerReplyPhase(resolvedAnswer.spokenText, resolvedAnswer.emotionState, playerReplyDuration);
            }

            Debug.Log($"<color=cyan>[GameManager] Slot {currentSlotIndex} Phase 2 (Player Reply - {playerReplyDuration}s): \"{resolvedAnswer?.spokenText}\" [Tone: {resolvedAnswer?.emotionState}]</color>");
        }

        private void UpdatePlayerReplyPhase()
        {
            phaseTimer -= Time.deltaTime;
            if (ui != null)
            {
                ui.UpdateTimer(Mathf.Max(0f, phaseTimer), playerReplyDuration, VisualTheme.ColorPlayerAnswerText);
            }

            // Expiry -> Transition to Phase 3: Date Reaction
            if (phaseTimer <= 0f)
            {
                TransitionToDateReactionPhase();
            }
        }

        private void TransitionToDateReactionPhase()
        {
            currentPhase = SlotPhase.DateReaction;
            phaseTimer = dateReactionDuration;

            // Apply Connection delta and date feedback to Composure
            if (resolvedAnswer != null)
            {
                resources.ModifyConnection(resolvedAnswer.connectionDelta);

                // Date Reaction feedback: Positive response provides huge relief/confidence surge; negative response causes social tension/cringe
                float reactionComposureDelta = resolvedAnswer.connectionDelta switch
                {
                    > 0f => Mathf.Clamp(resolvedAnswer.connectionDelta * 1.5f, 6f, 15f),
                    < 0f => Mathf.Clamp(resolvedAnswer.connectionDelta * 1.2f, -12f, -4f),
                    _ => 3f
                };
                resources.ModifyComposure(reactionComposureDelta);
            }

            // Select reaction based on answered emotion state
            resolvedReaction = dialogueManager.SelectReaction(currentQuestion, resolvedAnswer != null ? resolvedAnswer.emotionState : EmotionState.FrozenBlank);

            if (ui != null && resolvedReaction != null)
            {
                ui.SetupDateReactionPhase(resolvedReaction.reactionText, resolvedReaction.animationClipTag, resolvedAnswer?.connectionDelta ?? 0f, dateReactionDuration);
            }

            Debug.Log($"<color=green>[GameManager] Slot {currentSlotIndex} Phase 3 (Date Reaction - {dateReactionDuration}s): \"{resolvedReaction?.reactionText}\" [Clip: {resolvedReaction?.animationClipTag}]</color>");
        }

        private void UpdateDateReactionPhase()
        {
            phaseTimer -= Time.deltaTime;
            if (ui != null)
            {
                ui.UpdateTimer(Mathf.Max(0f, phaseTimer), dateReactionDuration, VisualTheme.ColorDateReactionText);
            }

            // Expiry -> Slot Complete! Advance to next slot or ending
            if (phaseTimer <= 0f)
            {
                AdvanceToNextSlot();
            }
        }

        private void AdvanceToNextSlot()
        {
            if (currentSlotIndex >= 10)
            {
                var finalTier = resources.CurrentState.GetConnectionTier();
                var ending = finalTier switch
                {
                    ConnectionTier.Tier4_SecondDate => EndingType.SecondDate_Tier3_4,
                    ConnectionTier.Tier3_Strong => EndingType.SecondDate_Tier3_4,
                    ConnectionTier.Tier2_Maybe => EndingType.Maybe_Tier2,
                    _ => EndingType.AwkwardEnding_Tier1
                };
                TriggerEnding(ending);
            }
            else
            {
                // Inter-slot natural emotional settling (Bible §4.4 natural recovery / continuous carry)
                // Settles high saturation by 50% so each new social beat challenges the player dynamically
                mindMap?.SettleInfluencesBetweenSlots(0.50f);

                StartSlot(currentSlotIndex + 1);
            }
        }

        public void HandleEmotionDrop(InternalNodeType targetNode, CoreEmotion emotion)
        {
            if (currentPhase != SlotPhase.RtsIntervention)
            {
                Debug.Log("[GameManager] Emotions can only be deployed during Phase 1 (RTS).");
                return;
            }

            // Oxygen Operational Cost (Master Bible Part 2 §2.5)
            if (resources.CurrentState.oxygen < 15f)
            {
                Debug.LogWarning("[GameManager] Oxygen depleted! Lungs must generate oxygen before deploying units.");
                if (ui != null)
                {
                    var nodePos = ui.GetNodeView(targetNode)?.RectTransform.position ?? Vector3.zero;
                    ui.ShowFloatingMessage(nodePos, "LOW OXYGEN!", VisualTheme.ColorOxygen);
                }
                return;
            }

            // Deduct oxygen operational cost
            resources.ModifyOxygen(-15f);

            // Execute tactical RTS intervention on the node
            eventEngine.OnPlayerIntervene(targetNode, emotion);

            if (ui != null)
            {
                ui.OnEmotionInjected(targetNode, emotion);
            }

            RefreshProjectedAnswerPreview();

            Debug.Log($"<color=cyan>[GameManager] Player deployed {emotion} onto {targetNode} (-15 Oxygen)</color>");
        }

        private void OnNodeStateChanged(InternalNodeType type, NodeState state)
        {
            if (ui != null)
            {
                var view = ui.GetNodeView(type);
                if (view != null)
                {
                    var dominant = mindMap.GetDominantState(type);
                    view.UpdateDisplay(state.currentHealth, dominant, SocialEffectType.StressEvent, false);
                }
            }
        }

        private void OnResourceStateChanged(ResourceState state)
        {
            if (ui != null)
            {
                ui.UpdateResourceBars(state);
            }
        }

        private void TriggerEnding(EndingType ending)
        {
            currentPhase = SlotPhase.None;
            var data = EndingsData.GetEnding(ending);
            Debug.Log($"<color=magenta>=== DATE ENDED: {data.title} ===</color>\n{data.summary}\nDate: {data.dateFinalQuote}");

            if (ui != null)
            {
                ui.SetupRtsPhase(currentSlotIndex, ConnectionTier.Tier0_Collapse, $"[DATE COMPLETE: {data.title.ToUpper()}]", 0f);
                ui.SetupPlayerReplyPhase(data.summary, EmotionState.Calm, 0f);
                ui.SetupDateReactionPhase(data.dateFinalQuote, "polite_smile", 0f, 0f);
            }
        }
    }
}
