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
    /// Builder script to instantiate and assemble the complete CoreGameUI hierarchy in editor/runtime.
    /// Eliminates manual UI prefab wiring errors.
    /// </summary>
    public static class UIHierarchyBuilder
    {
        public static CoreGameUI BuildUIHierarchy(GameObject rootGO)
        {
            // Canvas setup
            var canvas = rootGO.GetComponent<Canvas>();
            if (canvas == null) canvas = rootGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = rootGO.GetComponent<CanvasScaler>() ?? rootGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            var raycaster = rootGO.GetComponent<GraphicRaycaster>() ?? rootGO.AddComponent<GraphicRaycaster>();

            // Ensure EventSystem
            var eventSystem = UnityEngine.Object.FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>();
            if (eventSystem == null)
            {
                var esGO = new GameObject("EventSystem");
                eventSystem = esGO.AddComponent<UnityEngine.EventSystems.EventSystem>();
                esGO.AddComponent<InputSystemUIInputModule>();
            }

            var coreUI = rootGO.GetComponent<CoreGameUI>() ?? rootGO.AddComponent<CoreGameUI>();

            // 1. Top Panel (Date Half ~48% height)
            var topPanelGO = CreateUIObject("DatePanel", rootGO.transform);
            var topRect = topPanelGO.GetComponent<RectTransform>();
            SetAnchor(topRect, new Vector2(0f, 0.52f), new Vector2(1f, 1f), Vector2.zero, Vector2.zero);
            var topImg = topPanelGO.AddComponent<Image>();
            topImg.color = VisualTheme.ColorDateBackground;

            // Date Character Portrait & Expression
            var portraitGO = CreateUIObject("DatePortrait", topPanelGO.transform);
            var portraitRect = portraitGO.GetComponent<RectTransform>();
            SetAnchor(portraitRect, new Vector2(0.04f, 0.25f), new Vector2(0.22f, 0.85f), Vector2.zero, Vector2.zero);
            var portraitImg = portraitGO.AddComponent<Image>();
            portraitImg.color = new Color(0.9f, 0.82f, 0.75f, 1f);

            var badgeGO = CreateUIObject("ExpressionBadge", portraitGO.transform);
            var badgeRect = badgeGO.GetComponent<RectTransform>();
            SetAnchor(badgeRect, new Vector2(0f, -0.2f), new Vector2(1f, 0f), Vector2.zero, Vector2.zero);
            var badgeText = badgeGO.AddComponent<Text>();
            badgeText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            badgeText.fontSize = 18;
            badgeText.alignment = TextAnchor.MiddleCenter;
            badgeText.text = "[Date: Neutral]";
            badgeText.color = new Color(0.3f, 0.3f, 0.3f, 1f);

            var dateVisuals = portraitGO.AddComponent<DateCharacterVisuals>();

            // Date Prompt Text
            var promptGO = CreateUIObject("PromptText", topPanelGO.transform);
            var promptRect = promptGO.GetComponent<RectTransform>();
            SetAnchor(promptRect, new Vector2(0.25f, 0.55f), new Vector2(0.92f, 0.95f), Vector2.zero, Vector2.zero);
            var promptText = promptGO.AddComponent<Text>();
            promptText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            promptText.fontSize = 32;
            promptText.alignment = TextAnchor.MiddleCenter;
            promptText.color = VisualTheme.ColorDateText;

            // Spoken Answer Text
            var answerGO = CreateUIObject("AnswerText", topPanelGO.transform);
            var answerRect = answerGO.GetComponent<RectTransform>();
            SetAnchor(answerRect, new Vector2(0.15f, 0.3f), new Vector2(0.85f, 0.52f), Vector2.zero, Vector2.zero);
            var answerText = answerGO.AddComponent<Text>();
            answerText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            answerText.fontSize = 24;
            answerText.fontStyle = FontStyle.Italic;
            answerText.alignment = TextAnchor.MiddleCenter;
            answerText.color = new Color(0.2f, 0.4f, 0.7f, 1f);

            // Date Reaction Text
            var reactGO = CreateUIObject("ReactionText", topPanelGO.transform);
            var reactRect = reactGO.GetComponent<RectTransform>();
            SetAnchor(reactRect, new Vector2(0.15f, 0.08f), new Vector2(0.85f, 0.28f), Vector2.zero, Vector2.zero);
            var reactText = reactGO.AddComponent<Text>();
            reactText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            reactText.fontSize = 26;
            reactText.alignment = TextAnchor.MiddleCenter;
            reactText.color = new Color(0.1f, 0.5f, 0.2f, 1f);

            // Timer Bar
            var timerBgGO = CreateUIObject("TimerBG", topPanelGO.transform);
            var timerBgRect = timerBgGO.GetComponent<RectTransform>();
            SetAnchor(timerBgRect, new Vector2(0.2f, 0.02f), new Vector2(0.8f, 0.06f), Vector2.zero, Vector2.zero);
            var timerBgImg = timerBgGO.AddComponent<Image>();
            timerBgImg.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);

            var timerFillGO = CreateUIObject("TimerFill", timerBgGO.transform);
            var timerFillRect = timerFillGO.GetComponent<RectTransform>();
            SetAnchor(timerFillRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var timerFillImg = timerFillGO.AddComponent<Image>();
            timerFillImg.type = Image.Type.Filled;
            timerFillImg.fillMethod = Image.FillMethod.Horizontal;
            timerFillImg.color = new Color(0.95f, 0.6f, 0.2f, 1f);

            // 2. Middle Divider
            var divGO = CreateUIObject("Divider", rootGO.transform);
            var divRect = divGO.GetComponent<RectTransform>();
            SetAnchor(divRect, new Vector2(0f, 0.505f), new Vector2(1f, 0.52f), Vector2.zero, Vector2.zero);
            var divImg = divGO.AddComponent<Image>();
            divImg.color = VisualTheme.ColorDivider;

            // 3. Bottom Panel (Internal World ~50% height)
            var bottomPanelGO = CreateUIObject("InternalPanel", rootGO.transform);
            var bottomRect = bottomPanelGO.GetComponent<RectTransform>();
            SetAnchor(bottomRect, Vector2.zero, new Vector2(1f, 0.505f), Vector2.zero, Vector2.zero);
            var bottomImg = bottomPanelGO.AddComponent<Image>();
            bottomImg.color = VisualTheme.ColorInternalBackground;

            // Resource Bars Container
            var resBarsGO = CreateUIObject("ResourceBars", bottomPanelGO.transform);
            var resBarsRect = resBarsGO.GetComponent<RectTransform>();
            SetAnchor(resBarsRect, new Vector2(0.05f, 0.88f), new Vector2(0.95f, 0.98f), Vector2.zero, Vector2.zero);
            var resourceBarsComp = resBarsGO.AddComponent<UIResourceBars>();
            resourceBarsComp.Initialize();

            // Mind Map Nodes (5 Nodes positioned geometrically)
            var nodesContainer = CreateUIObject("MindMapNodes", bottomPanelGO.transform);
            var nodesRect = nodesContainer.GetComponent<RectTransform>();
            SetAnchor(nodesRect, new Vector2(0.1f, 0.22f), new Vector2(0.9f, 0.85f), Vector2.zero, Vector2.zero);

            var nodePositions = new Dictionary<InternalNodeType, Vector2>
            {
                { InternalNodeType.Brain, new Vector2(0.5f, 0.75f) },
                { InternalNodeType.Voice, new Vector2(0.3f, 0.45f) },
                { InternalNodeType.Heart, new Vector2(0.7f, 0.45f) },
                { InternalNodeType.Body,  new Vector2(0.3f, 0.15f) },
                { InternalNodeType.Lungs, new Vector2(0.7f, 0.15f) }
            };

            foreach (var kvp in nodePositions)
            {
                var nodeGO = CreateUIObject($"Node_{kvp.Key}", nodesContainer.transform);
                var nRect = nodeGO.GetComponent<RectTransform>();
                nRect.anchorMin = kvp.Value;
                nRect.anchorMax = kvp.Value;
                nRect.sizeDelta = new Vector2(120, 120);

                var nodeView = nodeGO.AddComponent<UINodeView>();
                var bgImg = nodeGO.AddComponent<Image>();
                bgImg.color = VisualTheme.ColorNodeHealthy;

                var textGO = CreateUIObject("Name", nodeGO.transform);
                var tRect = textGO.GetComponent<RectTransform>();
                SetAnchor(tRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
                var txt = textGO.AddComponent<Text>();
                txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                txt.fontSize = 18;
                txt.alignment = TextAnchor.MiddleCenter;
                txt.color = Color.white;
                txt.text = kvp.Key.ToString();

                nodeView.Initialize(kvp.Key);
                coreUI.RegisterNodeView(kvp.Key, nodeView);
            }

            // Emotion Unit Drag Tray (Bottom)
            var trayGO = CreateUIObject("EmotionTray", bottomPanelGO.transform);
            var trayRect = trayGO.GetComponent<RectTransform>();
            SetAnchor(trayRect, new Vector2(0.15f, 0.02f), new Vector2(0.85f, 0.18f), Vector2.zero, Vector2.zero);
            var hlg = trayGO.AddComponent<HorizontalLayoutGroup>();
            hlg.childControlWidth = true;
            hlg.childControlHeight = true;
            hlg.spacing = 20;

            foreach (CoreEmotion emotion in Enum.GetValues(typeof(CoreEmotion)))
            {
                var emotionUnitGO = CreateUIObject($"Unit_{emotion}", trayGO.transform);
                var dragger = emotionUnitGO.AddComponent<UIEmotionDraggable>();
                var eImg = emotionUnitGO.AddComponent<Image>();
                eImg.color = VisualTheme.GetEmotionColor(emotion);

                var labelGO = CreateUIObject("Label", emotionUnitGO.transform);
                var lRect = labelGO.GetComponent<RectTransform>();
                SetAnchor(lRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
                var lTxt = labelGO.AddComponent<Text>();
                lTxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                lTxt.fontSize = 20;
                lTxt.fontStyle = FontStyle.Bold;
                lTxt.alignment = TextAnchor.MiddleCenter;
                lTxt.color = Color.black;
                lTxt.text = emotion.ToString();

                dragger.Initialize(emotion);
            }

            coreUI.AssignReferences(null, promptText, answerText, reactText, timerFillImg, resourceBarsComp, dateVisuals);

            return coreUI;
        }

        private static GameObject CreateUIObject(string name, Transform parent)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.AddComponent<RectTransform>();
            return go;
        }

        private static void SetAnchor(RectTransform rect, Vector2 min, Vector2 max, Vector2 offsetMin, Vector2 offsetMax)
        {
            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;
        }
    }
}
