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
    /// Master UI framework coordinating both top (Date) and bottom (Internal World) halves.
    /// Master Design Bible Part 2 §2.1 & Dev Plan Day 1 Track B.
    /// Uses modern Unity UI with InputSystemUIInputModule.
    /// </summary>
    public class CoreGameUI : MonoBehaviour
    {
        [Header("Root Layout")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform datePanel;
        [SerializeField] private RectTransform internalPanel;
        [SerializeField] private Image dividerLine;

        [Header("Top Half - Date View")]
        [SerializeField] private Text slotCounterText;
        [SerializeField] private Text dialoguePromptText;
        [SerializeField] private Text spokenAnswerText;
        [SerializeField] private Text dateReactionText;
        [SerializeField] private Image responseTimerBar;

        [Header("Bottom Half - Internal World View")]
        [SerializeField] private RectTransform nodesContainer;
        [SerializeField] private RectTransform emotionTrayContainer;
        [SerializeField] private UIResourceBars resourceBars;

        private readonly Dictionary<InternalNodeType, UINodeView> nodeViews = new Dictionary<InternalNodeType, UINodeView>();
        private readonly List<UIEmotionDraggable> emotionDraggables = new List<UIEmotionDraggable>();

        [SerializeField] private DateCharacterVisuals dateVisuals;

        public void AssignReferences(Text slot, Text prompt, Text answer, Text reaction, Image timer, UIResourceBars bars, DateCharacterVisuals visuals)
        {
            slotCounterText = slot;
            dialoguePromptText = prompt;
            spokenAnswerText = answer;
            dateReactionText = reaction;
            responseTimerBar = timer;
            resourceBars = bars;
            dateVisuals = visuals;
        }

        public void SetDateExpression(string animationClipTag)
        {
            if (dateVisuals != null) dateVisuals.SetExpressionFromTag(animationClipTag);
        }

        private void Awake()
        {
            EnsureInputModule();
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
                // Ensure InputSystemUIInputModule is present
                var legacyModule = eventSystem.GetComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                if (legacyModule != null) DestroyImmediate(legacyModule);
                eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
            }
        }

        public void SetupDateDisplay(int slotIndex, string prompt, float timerFill)
        {
            if (slotCounterText != null) slotCounterText.text = $"ROUND {slotIndex} / 10";
            if (dialoguePromptText != null) dialoguePromptText.text = prompt;
            if (responseTimerBar != null) responseTimerBar.fillAmount = Mathf.Clamp01(timerFill);
        }

        public void DisplaySpokenAnswer(string answerText)
        {
            if (spokenAnswerText != null) spokenAnswerText.text = answerText;
        }

        public void DisplayDateReaction(string reactionText)
        {
            if (dateReactionText != null) dateReactionText.text = reactionText;
        }

        public void UpdateResourceBars(ResourceState state)
        {
            if (resourceBars != null) resourceBars.UpdateBars(state);
        }

        public void RegisterNodeView(InternalNodeType type, UINodeView view)
        {
            nodeViews[type] = view;
        }

        public UINodeView GetNodeView(InternalNodeType type)
        {
            return nodeViews.TryGetValue(type, out var view) ? view : null;
        }
    }
}
