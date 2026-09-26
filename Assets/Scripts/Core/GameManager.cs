using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CtrlHeart.Core.Data;
using CtrlHeart.Core.Data.ScriptableObjects;
using CtrlHeart.Core.Systems;
using CtrlHeart.Core.UI;
using CtrlHeart.Core.Visuals;
using CtrlHeart.Core.Testing;

namespace CtrlHeart.Core
{
    /// <summary>
    /// Master Game Controller for Phase 1 / Day 3 Integration.
    /// Orchestrates the 10-slot loop, connects MindMapManager, ResourceManager,
    /// SocialEventEngine, and DialogueManager to the CoreGameUI in real time.
    /// Master Design Bible Part 3 §3.2, Part 5 §5.2 & Dev Plan Day 3.
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

        [Header("State")]
        [SerializeField] private int currentSlotIndex = 1;
        [SerializeField] private QuestionData currentQuestion;
        [SerializeField] private float slotTimer;
        [SerializeField] private float slotTimerDuration = 6f;
        [SerializeField] private bool isResponding = false;
        [SerializeField] private bool isShowingReaction = false;
        private float reactionTimer = 0f;
        private const float REACTION_DURATION = 3f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
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
            isShowingReaction = false;

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

            slotTimerDuration = currentQuestion.responseTimeWindow;
            slotTimer = slotTimerDuration;
            isResponding = true;

            // Update UI with Question prompt
            if (ui != null)
            {
                ui.SetupDateDisplay(slotIndex, currentQuestion.questionText, 1f);
                ui.DisplaySpokenAnswer("...");
                ui.DisplayDateReaction("");
            }

            // Trigger Question's Social Event Profile (threats / pushes)
            eventEngine.TriggerSocialEvent(currentQuestion.socialEventProfile);

            Debug.Log($"<color=yellow>[GameManager] Slot {slotIndex} Started: \"{currentQuestion.questionText}\"</color>");
        }

        private void Update()
        {
            if (isResponding)
            {
                // Passive regen tick
                float lungsHealth = mindMap.GetNode(InternalNodeType.Lungs)?.currentHealth ?? 100f;
                float brainHealth = mindMap.GetNode(InternalNodeType.Brain)?.currentHealth ?? 100f;
                resources.TickPassiveRegen(Time.deltaTime, lungsHealth, brainHealth);

                // Check fail states
                if (resources.IsMeltdown())
                {
                    isResponding = false;
                    TriggerEnding(EndingType.Meltdown_ComposureZero);
                    return;
                }
                if (resources.IsDateCollapsed())
                {
                    isResponding = false;
                    TriggerEnding(EndingType.DateCollapse_Tier0);
                    return;
                }

                // Response timer countdown
                slotTimer -= Time.deltaTime;
                float fill = Mathf.Clamp01(slotTimer / slotTimerDuration);
                if (ui != null) ui.SetupDateDisplay(currentSlotIndex, currentQuestion.questionText, fill);

                // Strictly resolve on timer expiry (Bible Part 5 §5.2 Step 4)
                if (slotTimer <= 0f)
                {
                    isResponding = false;
                    ResolveSlot();
                }
            }
            else if (isShowingReaction)
            {
                reactionTimer -= Time.deltaTime;
                if (reactionTimer <= 0f)
                {
                    isShowingReaction = false;
                    AdvanceToNextSlot();
                }
            }
        }

        private void ResolveSlot()
        {
            var primaryNode = currentQuestion.socialEventProfile.PrimaryTargetNode;
            var dominantState = mindMap.GetDominantState(primaryNode);
            bool isLowFocus = resources.IsFocusLow();
            bool isCriticalBody = mindMap.IsBodyCriticallyLow();

            var answer = dialogueManager.ResolveAnswer(currentQuestion, dominantState, isLowFocus, isCriticalBody);
            if (answer != null)
            {
                // Display player's spoken answer
                if (ui != null) ui.DisplaySpokenAnswer($"\"{answer.spokenText}\"");

                // Apply Connection delta
                resources.ModifyConnection(answer.connectionDelta);

                // Select and display reaction based on post-answer tier
                var postTier = resources.CurrentState.GetConnectionTier();
                var reaction = dialogueManager.SelectReaction(currentQuestion, postTier);
                if (reaction != null && ui != null)
                {
                    ui.DisplayDateReaction($"\"{reaction.reactionText}\"");
                    ui.SetDateExpression(reaction.animationClipTag);
                }

                Debug.Log($"<color=cyan>[GameManager] Slot {currentSlotIndex} Resolved -> Answer: \"{answer.spokenText}\" | Delta: {answer.connectionDelta:+#;-#;0} | Date Expression: [{reaction?.animationClipTag}]</color>");
            }

            isShowingReaction = true;
            reactionTimer = REACTION_DURATION;
        }

        private void AdvanceToNextSlot()
        {
            if (currentSlotIndex >= 10)
            {
                // Date Complete -> check final Connection ending
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
                // Continuous carry-forward: no reset! Advance to next slot
                StartSlot(currentSlotIndex + 1);
            }
        }

        public void HandleEmotionDrop(InternalNodeType targetNode, CoreEmotion emotion)
        {
            if (!isResponding) return;

            // Apply influence to node
            mindMap.ApplyInfluence(targetNode, emotion, 25f);

            // Emotion impact on resources
            if (emotion == CoreEmotion.Calm)
            {
                resources.ModifyComposure(+3f);
            }
            else if (emotion == CoreEmotion.Confidence)
            {
                resources.ModifyFocus(+2f);
            }

            Debug.Log($"[GameManager] Applied {emotion} to {targetNode}");
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
            var data = EndingsData.GetEnding(ending);
            Debug.Log($"<color=magenta>=== DATE ENDED: {data.title} ===</color>\n{data.summary}\nDate: {data.dateFinalQuote}");
            if (ui != null)
            {
                ui.SetupDateDisplay(currentSlotIndex, $"[{data.title}]", 0f);
                ui.DisplaySpokenAnswer(data.summary);
                ui.DisplayDateReaction(data.dateFinalQuote);
            }
        }
    }
}
