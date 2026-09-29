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
    /// Assembles the unified Bioluminescent Neurological Control Room UI:
    /// - Strict 50-50 Split Screen:
    ///   * Top 50%: The Date (Protagonist, Date, Scenarios, Responses, Reactions, Master Timer)
    ///   * Bottom 50%: The Internal World (5-Node Map, 4 Affect Units, 4 Resource Bars, ECG Monitor)
    /// - Streamlined, non-repetitive UI matching Master Design Bible Part 2 §2.1–§2.7.
    /// </summary>
    public static class UIHierarchyBuilder
    {
        public static CoreGameUI BuildUIHierarchy(GameObject rootGO)
        {
            // ── Root Canvas Setup ──
            var canvas = rootGO.GetComponent<Canvas>() ?? rootGO.AddComponent<Canvas>();
            var cam = Camera.main;
            if (cam != null)
            {
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = cam;
                canvas.planeDistance = 10f;
            }
            else
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }

            var scaler = rootGO.GetComponent<CanvasScaler>() ?? rootGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1440, 1080);
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

            // Clean previous children if any exist
            int childCount = rootGO.transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyImmediate(rootGO.transform.GetChild(i).gameObject);
            }

            var coreUI = rootGO.GetComponent<CoreGameUI>() ?? rootGO.AddComponent<CoreGameUI>();
            var floatingFeedback = rootGO.GetComponent<UIFloatingFeedback>() ?? rootGO.AddComponent<UIFloatingFeedback>();

            var pixelFont = VisualTheme.GetPixelFont();
            var defaultFont = pixelFont;

            // ── Full Dark Backdrop ──
            var bgGO = CreateUIObject("BiomeBackground", rootGO.transform);
            SetAnchor(bgGO.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var bgImg = bgGO.AddComponent<Image>();
            bgImg.color = VisualTheme.ColorBackgroundDeep;

            // ══════════════════════════════════════════════════════════════════
            // 1. TOP 60%: THE DATE & CONVERSATION (Y: 0.40 to 1.0)
            // ══════════════════════════════════════════════════════════════════
            var topPanelGO = CreateUIObject("DatePanel", rootGO.transform);
            var topRect = topPanelGO.GetComponent<RectTransform>();
            SetAnchor(topRect, new Vector2(0f, 0.40f), new Vector2(1f, 1f), Vector2.zero, Vector2.zero);
            var topBg = topPanelGO.AddComponent<Image>();
            topBg.color = VisualTheme.ColorDateBackground;

            // ── Background Behind the Girl (Outdoor Cafe Patio) ──
            var bgContainerGO = CreateUIObject("DateBackgroundContainer", topPanelGO.transform);
            var bgContRect = bgContainerGO.GetComponent<RectTransform>();
            SetAnchor(bgContRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            bgContainerGO.AddComponent<RectMask2D>();

            var bgImageGO = CreateUIObject("DateBackgroundImage", bgContainerGO.transform);
            var bgImgRect = bgImageGO.GetComponent<RectTransform>();
            SetAnchor(bgImgRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var dateBgImg = bgImageGO.AddComponent<Image>();
            dateBgImg.sprite = LoadSpriteAsset("Assets/Animations/background.png");
            dateBgImg.color = new Color(0.92f, 0.90f, 0.88f, 1f);
            var bgFitter = bgImageGO.AddComponent<AspectRatioFitter>();
            bgFitter.aspectRatio = 2048f / 1529f; // Native 4:3 ratio
            bgFitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;

            // Header Row (Round counter, Master timer)
            var headerRowGO = CreateUIObject("HeaderRow", topPanelGO.transform);
            var headerRect = headerRowGO.GetComponent<RectTransform>();
            SetAnchor(headerRect, new Vector2(0.02f, 0.90f), new Vector2(0.98f, 0.985f), Vector2.zero, Vector2.zero);

            // Round counter badge
            var roundBadgeGO = CreateUIObject("RoundBadge", headerRowGO.transform);
            var rbRect = roundBadgeGO.GetComponent<RectTransform>();
            SetAnchor(rbRect, new Vector2(0f, 0.05f), new Vector2(0.20f, 0.95f), Vector2.zero, Vector2.zero);
            var rbImg = roundBadgeGO.AddComponent<Image>();
            rbImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_inset_brown");
            rbImg.type = Image.Type.Sliced;
            rbImg.color = Color.white;
            var roundText = CreateText(roundBadgeGO.transform, "RoundCounter", "ROUND 1 / 10", 18, FontStyle.Bold, VisualTheme.ColorGoldAccent, TextAnchor.MiddleCenter,
                Vector2.zero, Vector2.one, pixelFont);

            // Master Timer Container
            var timerContainerGO = CreateUIObject("TimerContainer", headerRowGO.transform);
            var tcRect = timerContainerGO.GetComponent<RectTransform>();
            SetAnchor(tcRect, new Vector2(0.74f, 0.05f), new Vector2(0.99f, 0.95f), Vector2.zero, Vector2.zero);
            var tcBg = timerContainerGO.AddComponent<Image>();
            tcBg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_bar_frame");
            tcBg.type = Image.Type.Sliced;
            tcBg.color = Color.white;

            var timerMaskGO = CreateUIObject("TimerMask", timerContainerGO.transform);
            var tmRect = timerMaskGO.GetComponent<RectTransform>();
            SetAnchor(tmRect, Vector2.zero, Vector2.one, new Vector2(4, 3), new Vector2(-4, -3));
            timerMaskGO.AddComponent<RectMask2D>();

            var timerFillGO = CreateUIObject("TimerFill", timerMaskGO.transform);
            var tfRect = timerFillGO.GetComponent<RectTransform>();
            SetAnchor(tfRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var timerFillImg = timerFillGO.AddComponent<Image>();
            timerFillImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_bar_fill_green");
            timerFillImg.type = Image.Type.Filled;
            timerFillImg.fillMethod = Image.FillMethod.Horizontal;
            timerFillImg.color = Color.white;

            var timerSecText = CreateText(timerContainerGO.transform, "TimerSeconds", "20.0s", 18, FontStyle.Bold, VisualTheme.ColorParchmentText, TextAnchor.MiddleCenter,
                Vector2.zero, Vector2.one, pixelFont);

            // ── Centered Date Character (Bust-Up Seated Directly Above Dialogue Box in Upper 60%) ──
            var portraitFrameGO = CreateUIObject("DatePortraitFrame", topPanelGO.transform);
            var pfRect = portraitFrameGO.GetComponent<RectTransform>();
            SetAnchor(pfRect, new Vector2(0.24f, 0.20f), new Vector2(0.76f, 0.90f), Vector2.zero, Vector2.zero);
            portraitFrameGO.AddComponent<RectMask2D>();

            var portraitAvatarGO = CreateUIObject("Avatar", portraitFrameGO.transform);
            var paRect = portraitAvatarGO.GetComponent<RectTransform>();
            paRect.pivot = new Vector2(0.5f, 1.0f); // Top center
            paRect.anchorMin = new Vector2(0.5f, 1.0f);
            paRect.anchorMax = new Vector2(0.5f, 1.0f);
            paRect.anchoredPosition = new Vector2(0f, 0f);
            // Height in 60% top panel displays upper 50% bust-up crisply
            paRect.sizeDelta = new Vector2(760f, 760f);
            var paImg = portraitAvatarGO.AddComponent<Image>();
            paImg.sprite = UIProceduralTextureGenerator.GetSprite("icon_girl_avatar");
            paImg.color = Color.white;
            paImg.preserveAspect = true;

            var animator = portraitAvatarGO.AddComponent<Animator>();
#if UNITY_EDITOR
            var ctrl = UnityEditor.AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Animations/DateCharacter/DateCharacterAnimator.controller");
            if (ctrl != null) animator.runtimeAnimatorController = ctrl;
#endif

            var dateVisuals = portraitAvatarGO.AddComponent<DateCharacterVisuals>();

            // ── Cafe Table (Just Over Her Lower Body, Only Top Rim / Surface Visible) ──
            var tableContainerGO = CreateUIObject("DateTableContainer", topPanelGO.transform);
            var tcTableRect = tableContainerGO.GetComponent<RectTransform>();
            SetAnchor(tcTableRect, new Vector2(0.12f, 0.170f), new Vector2(0.88f, 0.235f), Vector2.zero, Vector2.zero);
            tableContainerGO.AddComponent<RectMask2D>();

            var tableGO = CreateUIObject("DateTableImage", tableContainerGO.transform);
            var tableRect = tableGO.GetComponent<RectTransform>();
            tableRect.anchorMin = new Vector2(0.5f, 1.0f);
            tableRect.anchorMax = new Vector2(0.5f, 1.0f);
            tableRect.pivot = new Vector2(0.5f, 1.0f); // Top rim of table
            tableRect.sizeDelta = new Vector2(680f, 624f);
            tableRect.anchoredPosition = new Vector2(0f, 0f);
            var tableImg = tableGO.AddComponent<Image>();
            tableImg.sprite = LoadSpriteAsset("Assets/Animations/table.png");
            tableImg.color = Color.white;
            tableImg.preserveAspect = true;
            tableImg.raycastTarget = false;

            // ── Unified Horizontal Dialogue Box (RPG Brown Wood Frame + Light Cream Parchment, Compact Sleek Height) ──
            var dialogBoxGO = CreateUIObject("UnifiedDialogueBox", topPanelGO.transform);
            var dRect = dialogBoxGO.GetComponent<RectTransform>();
            SetAnchor(dRect, new Vector2(0.02f, 0.012f), new Vector2(0.98f, 0.175f), Vector2.zero, Vector2.zero);
            var dImg = dialogBoxGO.AddComponent<Image>();
            dImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_brown");
            dImg.type = Image.Type.Sliced;
            dImg.color = Color.white;

            // Inset Light Parchment Paper for Visual Novel Romance Aesthetic
            var dialogParchmentGO = CreateUIObject("ParchmentInset", dialogBoxGO.transform);
            var dpRect = dialogParchmentGO.GetComponent<RectTransform>();
            SetAnchor(dpRect, new Vector2(0.008f, 0.05f), new Vector2(0.992f, 0.95f), Vector2.zero, Vector2.zero);
            var dpImg = dialogParchmentGO.AddComponent<Image>();
            dpImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_inset_light");
            dpImg.type = Image.Type.Sliced;
            dpImg.color = Color.white;

            // Speaker Badge (Top-Left pinned)
            var speakerBadgeGO = CreateUIObject("SpeakerBadge", dialogBoxGO.transform);
            var spkBadgeRect = speakerBadgeGO.GetComponent<RectTransform>();
            SetAnchor(spkBadgeRect, new Vector2(0.015f, 0.60f), new Vector2(0.205f, 0.98f), Vector2.zero, Vector2.zero);
            var spkBadgeImg = speakerBadgeGO.AddComponent<Image>();
            spkBadgeImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_long_brown");
            spkBadgeImg.type = Image.Type.Sliced;
            spkBadgeImg.color = Color.white;
            var speakerText = CreateText(speakerBadgeGO.transform, "SpeakerText", "DATE", 16, FontStyle.Bold, new Color(0.95f, 0.45f, 0.65f), TextAnchor.MiddleCenter,
                Vector2.zero, Vector2.one, pixelFont);

            // Spoken Dialogue Text (Dark Walnut Ink on Light Parchment)
            var dialogueText = CreateText(dialogBoxGO.transform, "DialogueText", "\"Loading scenario...\"", 20, FontStyle.Normal, VisualTheme.ColorDateText, TextAnchor.MiddleLeft,
                new Vector2(0.030f, 0.05f), new Vector2(0.955f, 0.64f), pixelFont);
            dialogueText.lineSpacing = 1.25f;

            // Romantic Dialogue Continue Indicator
            var contGO = CreateUIObject("ContinueIndicator", dialogParchmentGO.transform);
            var cRect = contGO.GetComponent<RectTransform>();
            SetAnchor(cRect, new Vector2(0.965f, 0.08f), new Vector2(0.990f, 0.38f), Vector2.zero, Vector2.zero);
            var cImg = contGO.AddComponent<Image>();
            cImg.sprite = UIProceduralTextureGenerator.GetSprite("icon_heart");
            cImg.color = new Color(0.85f, 0.35f, 0.50f, 0.85f);
            cImg.preserveAspect = true;

            // Meta Subtext (Top-Right: Connection Delta or Tone Tag)
            var metaSubtext = CreateText(dialogBoxGO.transform, "MetaSubtext", "", 16, FontStyle.Bold, new Color(0.15f, 0.55f, 0.25f), TextAnchor.MiddleRight,
                new Vector2(0.55f, 0.60f), new Vector2(0.97f, 0.96f), pixelFont);

            // ══════════════════════════════════════════════════════════════════
            // 2. MIDDLE DIVIDER LINE (60-40 HORIZONTAL SPLIT)
            // ══════════════════════════════════════════════════════════════════
            var divGO = CreateUIObject("DividerGlow", rootGO.transform);
            var divRect = divGO.GetComponent<RectTransform>();
            SetAnchor(divRect, new Vector2(0f, 0.396f), new Vector2(1f, 0.404f), Vector2.zero, Vector2.zero);
            var divImg = divGO.AddComponent<Image>();
            divImg.color = VisualTheme.ColorDivider;

            // ══════════════════════════════════════════════════════════════════
            // 3. BOTTOM 40%: THE INTERNAL WORLD (Y: 0.0 to 0.396)
            // ══════════════════════════════════════════════════════════════════
            var bottomAreaGO = CreateUIObject("InternalRtsArea", rootGO.transform);
            var bottomRect = bottomAreaGO.GetComponent<RectTransform>();
            SetAnchor(bottomRect, Vector2.zero, new Vector2(1f, 0.396f), Vector2.zero, Vector2.zero);
            var bottomBg = bottomAreaGO.AddComponent<Image>();
            bottomBg.color = VisualTheme.ColorInternalBackground;

            // ── Left Panel: 4 Affect Units (Calm, Anxiety, Confidence, Attraction) ──
            var leftPanelGO = CreateUIObject("EmotionsPanel", bottomAreaGO.transform);
            var lpRect = leftPanelGO.GetComponent<RectTransform>();
            SetAnchor(lpRect, new Vector2(0.02f, 0.13f), new Vector2(0.19f, 0.97f), Vector2.zero, Vector2.zero);
            var lpImg = leftPanelGO.AddComponent<Image>();
            lpImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_brown");
            lpImg.type = Image.Type.Sliced;
            lpImg.color = Color.white;

            var lpTitle = CreateText(leftPanelGO.transform, "Title", "AFFECT UNITS", 16, FontStyle.Bold, VisualTheme.ColorGoldAccent, TextAnchor.MiddleCenter,
                new Vector2(0f, 0.91f), new Vector2(1f, 1f), pixelFont);

            var vlgGO = CreateUIObject("CardsLayout", leftPanelGO.transform);
            var vlgRect = vlgGO.GetComponent<RectTransform>();
            SetAnchor(vlgRect, new Vector2(0.05f, 0.03f), new Vector2(0.95f, 0.90f), Vector2.zero, Vector2.zero);
            var vlg = vlgGO.AddComponent<VerticalLayoutGroup>();
            vlg.spacing = 6;
            vlg.childControlWidth = true;
            vlg.childControlHeight = true;

            var emotionDefinitions = new (CoreEmotion emotion, string role)[]
            {
                (CoreEmotion.Calm, "Stabilize & Heal"),
                (CoreEmotion.Anxiety, "Emergency Intercept"),
                (CoreEmotion.Confidence, "Assert & Steady"),
                (CoreEmotion.Attraction, "Amplify Warmth")
            };

            foreach (var (emotion, role) in emotionDefinitions)
            {
                var cardGO = CreateUIObject($"Card_{emotion}", vlgGO.transform);
                var cardBg = cardGO.AddComponent<Image>();
                cardBg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_long_brown");
                cardBg.type = Image.Type.Sliced;
                cardBg.color = Color.white;

                var dragger = cardGO.AddComponent<UIEmotionDraggable>();

                // Icon (Left)
                var iconGO = CreateUIObject("Icon", cardGO.transform);
                var iRect = iconGO.GetComponent<RectTransform>();
                SetAnchor(iRect, new Vector2(0.06f, 0.18f), new Vector2(0.28f, 0.82f), Vector2.zero, Vector2.zero);
                var iconImg = iconGO.AddComponent<Image>();
                iconImg.raycastTarget = false;

                // Emotion Name (Center Top)
                var label = CreateText(cardGO.transform, "Label", emotion.ToString().ToUpper(), 14, FontStyle.Bold, Color.white, TextAnchor.MiddleLeft,
                    new Vector2(0.32f, 0.52f), new Vector2(0.96f, 0.92f), pixelFont);
                label.horizontalOverflow = HorizontalWrapMode.Overflow;
                label.verticalOverflow = VerticalWrapMode.Overflow;

                // Role Subtitle (Center Bottom)
                var roleText = CreateText(cardGO.transform, "Role", role, 12, FontStyle.Bold, new Color(0.95f, 0.95f, 0.95f, 1f), TextAnchor.MiddleLeft,
                    new Vector2(0.32f, 0.10f), new Vector2(0.98f, 0.50f), pixelFont);
                roleText.horizontalOverflow = HorizontalWrapMode.Overflow;
                roleText.verticalOverflow = VerticalWrapMode.Overflow;

                // Selection glow ring
                var glowGO = CreateUIObject("SelectionGlow", cardGO.transform);
                var glowRect = glowGO.GetComponent<RectTransform>();
                SetAnchor(glowRect, Vector2.zero, Vector2.one, new Vector2(-4, -4), new Vector2(4, 4));
                var glowImg = glowGO.AddComponent<Image>();
                glowImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_long_beige");
                glowImg.type = Image.Type.Sliced;
                glowImg.color = VisualTheme.GetEmotionColor(emotion);
                glowImg.raycastTarget = false;
                glowGO.SetActive(false);

                dragger.AssignComponents(cardBg, iconImg, label, null, glowImg);
                dragger.Initialize(emotion);
                coreUI.RegisterEmotionDraggable(emotion, dragger);
            }

            // ── Center Biome: Event Alert Banner + 5-Node Neural Map ──
            var centerAreaGO = CreateUIObject("CenterNetworkArea", bottomAreaGO.transform);
            var centerRect = centerAreaGO.GetComponent<RectTransform>();
            SetAnchor(centerRect, new Vector2(0.20f, 0.12f), new Vector2(0.80f, 0.99f), Vector2.zero, Vector2.zero);

            // Active Event Ticker / Curated Heraldic Threat Alert Banner
            var bannerGO = CreateUIObject("ActiveEventBanner", centerAreaGO.transform);
            var bannerRect = bannerGO.GetComponent<RectTransform>();
            SetAnchor(bannerRect, new Vector2(0.04f, 0.83f), new Vector2(0.96f, 0.99f), Vector2.zero, Vector2.zero);
            var bannerBg = bannerGO.AddComponent<Image>();
            bannerBg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_brown");
            bannerBg.type = Image.Type.Sliced;
            bannerBg.color = Color.white;

            // Inner Parchment Scroll for Alert Readability
            var bannerScrollGO = CreateUIObject("ParchmentScroll", bannerGO.transform);
            var bsRect = bannerScrollGO.GetComponent<RectTransform>();
            SetAnchor(bsRect, new Vector2(0.012f, 0.08f), new Vector2(0.988f, 0.92f), Vector2.zero, Vector2.zero);
            var bsImg = bannerScrollGO.AddComponent<Image>();
            bsImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_inset_light");
            bsImg.type = Image.Type.Sliced;
            bsImg.color = Color.white;

            // Heraldic Alert Icon on Left
            var alertIconGO = CreateUIObject("AlertIcon", bannerScrollGO.transform);
            var aiRect = alertIconGO.GetComponent<RectTransform>();
            SetAnchor(aiRect, new Vector2(0.02f, 0.15f), new Vector2(0.07f, 0.85f), Vector2.zero, Vector2.zero);
            var aiImg = alertIconGO.AddComponent<Image>();
            aiImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_sword_gold");
            aiImg.preserveAspect = true;

            var eventHeadline = CreateText(bannerScrollGO.transform, "Headline", "⚠ SOCIAL THREAT DETECTED", 15, FontStyle.Bold, new Color(0.82f, 0.15f, 0.10f), TextAnchor.MiddleLeft,
                new Vector2(0.08f, 0.50f), new Vector2(0.98f, 0.96f), pixelFont);
            eventHeadline.horizontalOverflow = HorizontalWrapMode.Overflow;
            eventHeadline.verticalOverflow = VerticalWrapMode.Overflow;

            var eventDetails = CreateText(bannerScrollGO.transform, "Details", "Primary Answer Node: Brain | Impacting Composure and Organ Harmony", 13, FontStyle.Bold, new Color(0.12f, 0.08f, 0.06f, 1f), TextAnchor.MiddleLeft,
                new Vector2(0.08f, 0.04f), new Vector2(0.98f, 0.50f), pixelFont);
            eventDetails.horizontalOverflow = HorizontalWrapMode.Overflow;
            eventDetails.verticalOverflow = VerticalWrapMode.Overflow;

            coreUI.AssignEventBanner(bannerGO, eventHeadline, eventDetails, null);

            // Neural Pathways Component
            var pathwaysGO = CreateUIObject("NeuralPathways", centerAreaGO.transform);
            var pathwaysRect = pathwaysGO.GetComponent<RectTransform>();
            SetAnchor(pathwaysRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var neuralPathways = pathwaysGO.AddComponent<UINeuralPathways>();

            // 5 Organ Nodes in User's Custom Layout:
            //        VOICE (top-left)       HEART (top-right)
            //                 \           /
            //                 BRAIN (center)
            //                 /           \
            //         BODY (bottom-left)    LUNGS (bottom-right)
            var nodeNormalizedPositions = new Dictionary<InternalNodeType, Vector2>
            {
                { InternalNodeType.Voice, new Vector2(0.220f, 0.604f) },
                { InternalNodeType.Heart, new Vector2(0.780f, 0.604f) },
                { InternalNodeType.Brain, new Vector2(0.500f, 0.408f) },
                { InternalNodeType.Body,  new Vector2(0.250f, 0.205f) },
                { InternalNodeType.Lungs, new Vector2(0.750f, 0.205f) }
            };

            var nodePixelAnchors = new Dictionary<InternalNodeType, Vector2>();

            foreach (var kvp in nodeNormalizedPositions)
            {
                var nodeGO = CreateUIObject($"Node_{kvp.Key}", centerAreaGO.transform);
                var nRect = nodeGO.GetComponent<RectTransform>();
                nRect.anchorMin = kvp.Value;
                nRect.anchorMax = kvp.Value;
                nRect.sizeDelta = new Vector2(104, 104);
                nRect.anchoredPosition = Vector2.zero;

                float centerWidth = 1440f * (0.80f - 0.20f); // 864f
                float centerHeight = 1080f * 0.396f * (0.99f - 0.12f); // ~372f
                nodePixelAnchors[kvp.Key] = nRect.anchoredPosition + new Vector2(
                    (kvp.Value.x - 0.5f) * centerWidth,
                    (kvp.Value.y - 0.5f) * centerHeight
                );

                var rootHitbox = nodeGO.AddComponent<Image>();
                rootHitbox.color = Color.clear;
                rootHitbox.raycastTarget = true;

                var nodeView = nodeGO.AddComponent<UINodeView>();

                // Target Crown Glow (Kenney RPG Ornate Beige Beveled Box Frame)
                var crownGO = CreateUIObject("TargetCrown", nodeGO.transform);
                var crRect = crownGO.GetComponent<RectTransform>();
                SetAnchor(crRect, new Vector2(0.04f, 0.20f), new Vector2(0.96f, 1.02f), Vector2.zero, Vector2.zero);
                var crownImg = crownGO.AddComponent<Image>();
                crownImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_square_beige");
                crownImg.type = Image.Type.Sliced;
                crownImg.color = new Color(1.0f, 0.85f, 0.2f, 0.95f);
                crownImg.raycastTarget = false;
                crownGO.SetActive(false);

                // Primary Target Organ Floating Ribbon Badge
                var targetBadgeGO = CreateUIObject("TargetBadge", nodeGO.transform);
                var tbRect = targetBadgeGO.GetComponent<RectTransform>();
                SetAnchor(tbRect, new Vector2(0.05f, 1.01f), new Vector2(0.95f, 1.20f), Vector2.zero, Vector2.zero);
                var tbImg = targetBadgeGO.AddComponent<Image>();
                tbImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_long_beige");
                tbImg.type = Image.Type.Sliced;
                tbImg.color = Color.white;
                var tbText = CreateText(targetBadgeGO.transform, "Text", "* TARGET *", 13, FontStyle.Bold, VisualTheme.ColorDateText, TextAnchor.MiddleCenter,
                    Vector2.zero, Vector2.one, pixelFont);
                targetBadgeGO.SetActive(false);

                // Outer RPG Box Frame (Kenney buttonSquare_brown with 9-slice borders)
                var ringGO = CreateUIObject("OuterBoxFrame", nodeGO.transform);
                var ringRect = ringGO.GetComponent<RectTransform>();
                SetAnchor(ringRect, new Vector2(0.07f, 0.24f), new Vector2(0.93f, 0.98f), Vector2.zero, Vector2.zero);
                var ringImg = ringGO.AddComponent<Image>();
                ringImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_square_brown");
                ringImg.type = Image.Type.Sliced;
                ringImg.color = Color.white;
                ringImg.raycastTarget = true;

                // Inner Inset Slot (Kenney panelInset_brown recessed depth)
                var discGO = CreateUIObject("InnerSlot", ringGO.transform);
                var discRect = discGO.GetComponent<RectTransform>();
                SetAnchor(discRect, new Vector2(0.12f, 0.12f), new Vector2(0.88f, 0.88f), Vector2.zero, Vector2.zero);
                var discImg = discGO.AddComponent<Image>();
                discImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_inset_brown");
                discImg.type = Image.Type.Sliced;
                discImg.color = Color.white;
                discImg.raycastTarget = true;

                // Organ Icon
                var iconGO = CreateUIObject("OrganIcon", discGO.transform);
                var iRect = iconGO.GetComponent<RectTransform>();
                SetAnchor(iRect, new Vector2(0.18f, 0.28f), new Vector2(0.82f, 0.88f), Vector2.zero, Vector2.zero);
                var iconImg = iconGO.AddComponent<Image>();
                iconImg.raycastTarget = false;

                // Node Name Text (e.g. BRAIN)
                var nameText = CreateText(discGO.transform, "NodeName", kvp.Key.ToString().ToUpper(), 15, FontStyle.Bold, Color.white, TextAnchor.MiddleCenter,
                    new Vector2(0f, 0.02f), new Vector2(1f, 0.26f), pixelFont);
                nameText.horizontalOverflow = HorizontalWrapMode.Overflow;
                nameText.verticalOverflow = VerticalWrapMode.Overflow;

                // Health Bar (RPG Bar Frame below square box)
                var hpBarGO = CreateUIObject("HealthBar", nodeGO.transform);
                var hpRect = hpBarGO.GetComponent<RectTransform>();
                SetAnchor(hpRect, new Vector2(0.08f, 0.12f), new Vector2(0.92f, 0.20f), Vector2.zero, Vector2.zero);
                var hpBg = hpBarGO.AddComponent<Image>();
                hpBg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_bar_frame");
                hpBg.type = Image.Type.Sliced;
                hpBg.color = Color.white;

                var hpMaskGO = CreateUIObject("Mask", hpBarGO.transform);
                var hpmRect = hpMaskGO.GetComponent<RectTransform>();
                SetAnchor(hpmRect, Vector2.zero, Vector2.one, new Vector2(3, 2), new Vector2(-3, -2));
                hpMaskGO.AddComponent<RectMask2D>();

                var hpFillGO = CreateUIObject("Fill", hpMaskGO.transform);
                var hpfRect = hpFillGO.GetComponent<RectTransform>();
                SetAnchor(hpfRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
                var hpFill = hpFillGO.AddComponent<Image>();
                hpFill.sprite = UIProceduralTextureGenerator.GetSprite("rpg_bar_fill_green");
                hpFill.type = Image.Type.Filled;
                hpFill.fillMethod = Image.FillMethod.Horizontal;
                hpFill.fillAmount = 1f;
                hpFill.color = Color.white;

                // Single Status Badge Underneath (Wider ribbon so full text like Awkward Silence fits cleanly)
                var statusBadgeGO = CreateUIObject("StatusBadge", nodeGO.transform);
                var sbRect = statusBadgeGO.GetComponent<RectTransform>();
                SetAnchor(sbRect, new Vector2(-0.16f, -0.06f), new Vector2(1.16f, 0.11f), Vector2.zero, Vector2.zero);
                var sbImg = statusBadgeGO.AddComponent<Image>();
                sbImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_inset_brown");
                sbImg.type = Image.Type.Sliced;
                sbImg.color = Color.white;

                var sbText = CreateText(statusBadgeGO.transform, "StatusText", "Stable", 14, FontStyle.Bold, new Color(0.40f, 0.95f, 0.60f, 1f), TextAnchor.MiddleCenter,
                    Vector2.zero, Vector2.one, pixelFont);
                sbText.horizontalOverflow = HorizontalWrapMode.Overflow;
                sbText.verticalOverflow = VerticalWrapMode.Overflow;

                nodeView.AssignComponents(ringImg, discImg, iconImg, nameText, hpFill, sbImg, sbText, crownImg, targetBadgeGO, tbText);
                nodeView.Initialize(kvp.Key);
                coreUI.RegisterNodeView(kvp.Key, nodeView);
            }

            neuralPathways.Initialize(nodePixelAnchors);

            // ── Right Panel: "INTERNAL RESOURCES" (Oxygen, Composure, Connection) ──
            var rightPanelGO = CreateUIObject("ResourcesPanel", bottomAreaGO.transform);
            var rpRect = rightPanelGO.GetComponent<RectTransform>();
            SetAnchor(rpRect, new Vector2(0.81f, 0.13f), new Vector2(0.98f, 0.97f), Vector2.zero, Vector2.zero);
            var rpImg = rightPanelGO.AddComponent<Image>();
            rpImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_brown");
            rpImg.type = Image.Type.Sliced;
            rpImg.color = Color.white;

            var rpTitle = CreateText(rightPanelGO.transform, "Title", "INTERNAL RESOURCES", 16, FontStyle.Bold, VisualTheme.ColorGoldAccent, TextAnchor.MiddleCenter,
                new Vector2(0f, 0.91f), new Vector2(1f, 1f), pixelFont);

            var resLayoutGO = CreateUIObject("ResLayout", rightPanelGO.transform);
            var resLRect = resLayoutGO.GetComponent<RectTransform>();
            SetAnchor(resLRect, new Vector2(0.06f, 0.06f), new Vector2(0.94f, 0.88f), Vector2.zero, Vector2.zero);
            var rvlg = resLayoutGO.AddComponent<VerticalLayoutGroup>();
            rvlg.spacing = 10;
            rvlg.childControlWidth = true;
            rvlg.childControlHeight = true;

            var resourceBars = rightPanelGO.AddComponent<UIResourceBars>();

            // Oxygen Row
            var (o2Icon, o2Fill, o2Text) = CreateResourceRow(resLayoutGO.transform, "Oxygen", VisualTheme.ColorOxygen, pixelFont);
            // Composure Row
            var (cIcon, cFill, cText) = CreateResourceRow(resLayoutGO.transform, "Composure", VisualTheme.ColorComposure, pixelFont);
            // Connection Row
            var (connIcon, connFill, connText) = CreateResourceRow(resLayoutGO.transform, "Connection", VisualTheme.ColorConnection, pixelFont);

            resourceBars.AssignReferences(o2Icon, o2Fill, o2Text, cIcon, cFill, cText, connIcon, connFill, connText);

            // ── Bottom Bar: Composure Heart Rate Monitor (ECG & BPM) ──
            var bottomBarGO = CreateUIObject("HeartRateBottomBar", bottomAreaGO.transform);
            var bbRect = bottomBarGO.GetComponent<RectTransform>();
            SetAnchor(bbRect, new Vector2(0.02f, 0.015f), new Vector2(0.98f, 0.11f), Vector2.zero, Vector2.zero);
            var bbImg = bottomBarGO.AddComponent<Image>();
            bbImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_brown");
            bbImg.type = Image.Type.Sliced;
            bbImg.color = Color.white;

            var hrTitle = CreateText(bottomBarGO.transform, "Title", "HEART RATE (Reflecting Composure)", 16, FontStyle.Bold, VisualTheme.ColorGoldAccent, TextAnchor.MiddleLeft,
                new Vector2(0.02f, 0f), new Vector2(0.32f, 1f), pixelFont);

            var ecgFrameGO = CreateUIObject("EcgFrame", bottomBarGO.transform);
            var efRect = ecgFrameGO.GetComponent<RectTransform>();
            SetAnchor(efRect, new Vector2(0.32f, 0.10f), new Vector2(0.82f, 0.90f), Vector2.zero, Vector2.zero);
            var efImg = ecgFrameGO.AddComponent<Image>();
            efImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_inset_brown");
            efImg.type = Image.Type.Sliced;
            efImg.color = Color.white;

            var rawEcgGO = CreateUIObject("EcgWaveImage", ecgFrameGO.transform);
            var ecgRect = rawEcgGO.GetComponent<RectTransform>();
            SetAnchor(ecgRect, new Vector2(0.02f, 0.05f), new Vector2(0.98f, 0.95f), Vector2.zero, Vector2.zero);
            var ecgRaw = rawEcgGO.AddComponent<RawImage>();

            var heartIconGO = CreateUIObject("HeartIcon", bottomBarGO.transform);
            var hiRect = heartIconGO.GetComponent<RectTransform>();
            SetAnchor(hiRect, new Vector2(0.84f, 0.15f), new Vector2(0.88f, 0.85f), Vector2.zero, Vector2.zero);
            var hiImg = heartIconGO.AddComponent<Image>();
            hiImg.sprite = UIProceduralTextureGenerator.GetSprite("icon_heart");
            hiImg.color = VisualTheme.ColorComposure;

            var bpmText = CreateText(bottomBarGO.transform, "BpmText", "72 bpm", 20, FontStyle.Bold, VisualTheme.ColorParchmentText, TextAnchor.MiddleLeft,
                new Vector2(0.89f, 0f), new Vector2(0.98f, 1f), pixelFont);

            var heartRateMonitor = bottomBarGO.AddComponent<UIHeartRateMonitor>();
            heartRateMonitor.Initialize(ecgRaw, bpmText, hiImg);

            // Wire all root UI references (phase badge, tier badge, and eq gauge removed)
            coreUI.AssignTopPanel(
                roundText, null, null, null,
                null, null, null, null,
                null, null, null,
                timerFillImg, timerSecText, dateVisuals);

            coreUI.AssignUnifiedDialogue(dialogBoxGO, speakerText, dialogueText, metaSubtext, null, null, null);

            coreUI.AssignBottomSystems(resourceBars, heartRateMonitor, neuralPathways, floatingFeedback);

            // ══════════════════════════════════════════════════════════════════
            // 3. IN-GAME PAUSE BUTTON & PAUSE MODAL OVERLAY
            // ══════════════════════════════════════════════════════════════════
            var pauseBtnGO = CreateUIObject("InGamePauseButton", rootGO.transform);
            var pbRect = pauseBtnGO.GetComponent<RectTransform>();
            SetAnchor(pbRect, new Vector2(0.952f, 0.940f), new Vector2(0.990f, 0.985f), Vector2.zero, Vector2.zero);
            var pbImg = pauseBtnGO.AddComponent<Image>();
            pbImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_square_brown");
            pbImg.type = Image.Type.Sliced;
            var pauseBtn = pauseBtnGO.AddComponent<Button>();

            var pauseIconGO = CreateUIObject("PauseIcon", pauseBtnGO.transform);
            var piRect = pauseIconGO.GetComponent<RectTransform>();
            SetAnchor(piRect, new Vector2(0.20f, 0.20f), new Vector2(0.80f, 0.80f), Vector2.zero, Vector2.zero);
            var piImg = pauseIconGO.AddComponent<Image>();
            piImg.sprite = UIProceduralTextureGenerator.GetSprite("icon_pause");
            piImg.color = VisualTheme.ColorParchmentText;
            piImg.raycastTarget = false;

            // Pause Modal Canvas Overlay
            var pauseOverlayGO = CreateUIObject("PauseOverlay", rootGO.transform);
            var poRect = pauseOverlayGO.GetComponent<RectTransform>();
            SetAnchor(poRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var poBg = pauseOverlayGO.AddComponent<Image>();
            poBg.color = new Color(0.04f, 0.03f, 0.05f, 0.85f);
            poBg.raycastTarget = true;

            var pauseModalGO = CreateUIObject("PauseModal", pauseOverlayGO.transform);
            var pmRect = pauseModalGO.GetComponent<RectTransform>();
            pmRect.anchorMin = new Vector2(0.5f, 0.5f);
            pmRect.anchorMax = new Vector2(0.5f, 0.5f);
            pmRect.sizeDelta = new Vector2(500, 340);
            pmRect.anchoredPosition = Vector2.zero;
            var pmImg = pauseModalGO.AddComponent<Image>();
            pmImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_brown");
            pmImg.type = Image.Type.Sliced;

            var pmHeaderGO = CreateUIObject("Header", pauseModalGO.transform);
            var pmhRect = pmHeaderGO.GetComponent<RectTransform>();
            SetAnchor(pmhRect, new Vector2(0.06f, 0.76f), new Vector2(0.94f, 0.94f), Vector2.zero, Vector2.zero);
            var pmhImg = pmHeaderGO.AddComponent<Image>();
            pmhImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_inset_brown");
            pmhImg.type = Image.Type.Sliced;
            CreateText(pmHeaderGO.transform, "Title", "GAME PAUSED", 26, FontStyle.Bold, VisualTheme.ColorGoldAccent, TextAnchor.MiddleCenter,
                Vector2.zero, Vector2.one, pixelFont);

            CreateText(pauseModalGO.transform, "Subtitle", "Maya is waiting patiently...", 18, FontStyle.Normal, new Color(1.0f, 0.98f, 0.92f, 1f), TextAnchor.MiddleCenter,
                new Vector2(0.08f, 0.58f), new Vector2(0.92f, 0.72f), pixelFont);

            var resumeBtnGO = CreateUIObject("ResumeButton", pauseModalGO.transform);
            var resRect = resumeBtnGO.GetComponent<RectTransform>();
            resRect.anchorMin = new Vector2(0.5f, 0.5f);
            resRect.anchorMax = new Vector2(0.5f, 0.5f);
            resRect.sizeDelta = new Vector2(280, 50);
            resRect.anchoredPosition = new Vector2(0, -10);
            var resImg = resumeBtnGO.AddComponent<Image>();
            resImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_long_brown");
            resImg.type = Image.Type.Sliced;
            var resumeBtn = resumeBtnGO.AddComponent<Button>();
            CreateText(resumeBtnGO.transform, "Label", "RESUME", 20, FontStyle.Bold, Color.white, TextAnchor.MiddleCenter,
                Vector2.zero, Vector2.one, pixelFont);

            var pauseMenuBtnGO = CreateUIObject("MainMenuButton", pauseModalGO.transform);
            var pmbRect = pauseMenuBtnGO.GetComponent<RectTransform>();
            pmbRect.anchorMin = new Vector2(0.5f, 0.5f);
            pmbRect.anchorMax = new Vector2(0.5f, 0.5f);
            pmbRect.sizeDelta = new Vector2(280, 50);
            pmbRect.anchoredPosition = new Vector2(0, -70);
            var pmbImg = pauseMenuBtnGO.AddComponent<Image>();
            pmbImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_long_brown");
            pmbImg.type = Image.Type.Sliced;
            var pauseMenuBtn = pauseMenuBtnGO.AddComponent<Button>();
            CreateText(pauseMenuBtnGO.transform, "Label", "MAIN MENU", 20, FontStyle.Bold, new Color(0.95f, 0.90f, 0.82f, 1f), TextAnchor.MiddleCenter,
                Vector2.zero, Vector2.one, pixelFont);

            pauseOverlayGO.SetActive(false);

            // ══════════════════════════════════════════════════════════════════
            // 4. END GAME RESULTS OVERLAY (FULL SCREEN DIMMER & VERDICT)
            // ══════════════════════════════════════════════════════════════════
            var resultsOverlayGO = CreateUIObject("ResultsOverlay", rootGO.transform);
            var roRect = resultsOverlayGO.GetComponent<RectTransform>();
            SetAnchor(roRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var roBg = resultsOverlayGO.AddComponent<Image>();
            roBg.color = new Color(0.04f, 0.03f, 0.05f, 0.90f); // Dark rich dimmer over entire game
            roBg.raycastTarget = true;

            var resultsModalGO = CreateUIObject("ResultsModal", resultsOverlayGO.transform);
            var rmRect = resultsModalGO.GetComponent<RectTransform>();
            rmRect.anchorMin = new Vector2(0.5f, 0.5f);
            rmRect.anchorMax = new Vector2(0.5f, 0.5f);
            rmRect.sizeDelta = new Vector2(880, 680);
            rmRect.anchoredPosition = Vector2.zero;
            var rmImg = resultsModalGO.AddComponent<Image>();
            rmImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_brown");
            rmImg.type = Image.Type.Sliced;

            // Verdict Header Banner
            var rmHeaderGO = CreateUIObject("VerdictHeader", resultsModalGO.transform);
            var rmhRect = rmHeaderGO.GetComponent<RectTransform>();
            SetAnchor(rmhRect, new Vector2(0.04f, 0.88f), new Vector2(0.96f, 0.97f), Vector2.zero, Vector2.zero);
            var rmhImg = rmHeaderGO.AddComponent<Image>();
            rmhImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_inset_brown");
            rmhImg.type = Image.Type.Sliced;
            var verdictTitleTxt = CreateText(rmHeaderGO.transform, "Title", "SECOND DATE SECURED!", 28, FontStyle.Bold, VisualTheme.ColorGoldAccent, TextAnchor.MiddleCenter,
                Vector2.zero, Vector2.one, pixelFont);
            AddShadow(verdictTitleTxt.gameObject);

            // Outcome Narrative Box
            var narrativeBoxGO = CreateUIObject("NarrativeBox", resultsModalGO.transform);
            var nbRect = narrativeBoxGO.GetComponent<RectTransform>();
            SetAnchor(nbRect, new Vector2(0.04f, 0.57f), new Vector2(0.96f, 0.86f), Vector2.zero, Vector2.zero);
            var nbImg = narrativeBoxGO.AddComponent<Image>();
            nbImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_inset_brown");
            nbImg.type = Image.Type.Sliced;
            nbImg.color = new Color(0.14f, 0.09f, 0.07f, 0.96f); // Deep dark mahogany background for exceptional text contrast
            var verdictBodyTxt = CreateText(narrativeBoxGO.transform, "Body", "Maya smiled warmly...", 19, FontStyle.Normal, new Color(1.0f, 0.98f, 0.92f, 1f), TextAnchor.UpperLeft,
                new Vector2(0.035f, 0.06f), new Vector2(0.965f, 0.94f), pixelFont);
            verdictBodyTxt.lineSpacing = 1.25f;

            // Maya's Final Quote Box
            var quoteBoxGO = CreateUIObject("QuoteBox", resultsModalGO.transform);
            var qbRect = quoteBoxGO.GetComponent<RectTransform>();
            SetAnchor(qbRect, new Vector2(0.04f, 0.44f), new Vector2(0.96f, 0.55f), Vector2.zero, Vector2.zero);
            var qbImg = quoteBoxGO.AddComponent<Image>();
            qbImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_inset_brown");
            qbImg.type = Image.Type.Sliced;
            qbImg.color = new Color(0.18f, 0.12f, 0.09f, 0.96f);

            var quoteHeartGO = CreateUIObject("QuoteHeart", quoteBoxGO.transform);
            var qhRect = quoteHeartGO.GetComponent<RectTransform>();
            SetAnchor(qhRect, new Vector2(0.02f, 0.15f), new Vector2(0.06f, 0.85f), Vector2.zero, Vector2.zero);
            var qhImg = quoteHeartGO.AddComponent<Image>();
            qhImg.sprite = UIProceduralTextureGenerator.GetSprite("icon_heart");
            qhImg.color = new Color(0.95f, 0.45f, 0.65f, 1f);
            qhImg.preserveAspect = true;

            var quoteTxt = CreateText(quoteBoxGO.transform, "Quote", "Maya: \"I had such a wonderful time today!\"", 15, FontStyle.Normal, new Color(1.0f, 0.88f, 0.50f, 1f), TextAnchor.MiddleLeft,
                new Vector2(0.07f, 0f), new Vector2(0.98f, 1f), pixelFont);

            // Stats Section (Inside Inset Box)
            var statsBoxGO = CreateUIObject("StatsBox", resultsModalGO.transform);
            var rsbRect = statsBoxGO.GetComponent<RectTransform>();
            SetAnchor(rsbRect, new Vector2(0.04f, 0.16f), new Vector2(0.96f, 0.42f), Vector2.zero, Vector2.zero);
            var rsbImg = statsBoxGO.AddComponent<Image>();
            rsbImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_inset_brown");
            rsbImg.type = Image.Type.Sliced;
            rsbImg.color = new Color(0.14f, 0.09f, 0.07f, 0.96f);

            var tierBadgeTxt = CreateText(statsBoxGO.transform, "TierBadge", "OUTCOME: TIER 4 - PASSIONATE CHEMISTRY", 20, FontStyle.Bold, new Color(0.35f, 0.95f, 0.70f, 1f), TextAnchor.MiddleCenter,
                new Vector2(0.04f, 0.72f), new Vector2(0.96f, 0.96f), pixelFont);
            AddShadow(tierBadgeTxt.gameObject);

            // Connection Progress Row
            var connScoreTxt = CreateText(statsBoxGO.transform, "ConnScore", "CONNECTION: 85 / 100", 17, FontStyle.Bold, VisualTheme.ColorConnection, TextAnchor.MiddleLeft,
                new Vector2(0.04f, 0.38f), new Vector2(0.36f, 0.68f), pixelFont);
            AddShadow(connScoreTxt.gameObject);

            var connBarFrameGO = CreateUIObject("ConnBarFrame", statsBoxGO.transform);
            var cbfRect = connBarFrameGO.GetComponent<RectTransform>();
            SetAnchor(cbfRect, new Vector2(0.38f, 0.40f), new Vector2(0.96f, 0.66f), Vector2.zero, Vector2.zero);
            var cbfImg = connBarFrameGO.AddComponent<Image>();
            cbfImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_bar_frame");
            cbfImg.type = Image.Type.Sliced;
            cbfImg.color = Color.white;

            var connBarMaskGO = CreateUIObject("ConnBarMask", connBarFrameGO.transform);
            var cbmRect = connBarMaskGO.GetComponent<RectTransform>();
            SetAnchor(cbmRect, Vector2.zero, Vector2.one, new Vector2(4, 3), new Vector2(-4, -3));
            connBarMaskGO.AddComponent<RectMask2D>();

            var connFillGO = CreateUIObject("ConnFill", connBarMaskGO.transform);
            var cfRect = connFillGO.GetComponent<RectTransform>();
            SetAnchor(cfRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var connBarFill = connFillGO.AddComponent<Image>();
            connBarFill.sprite = UIProceduralTextureGenerator.GetSprite("rpg_bar_fill_yellow");
            connBarFill.type = Image.Type.Filled;
            connBarFill.fillMethod = Image.FillMethod.Horizontal;
            connBarFill.fillAmount = 0.85f;
            connBarFill.color = Color.white;

            // Composure Progress Row
            var compScoreTxt = CreateText(statsBoxGO.transform, "CompScore", "COMPOSURE: 70 / 100", 17, FontStyle.Bold, VisualTheme.ColorComposure, TextAnchor.MiddleLeft,
                new Vector2(0.04f, 0.08f), new Vector2(0.36f, 0.38f), pixelFont);
            AddShadow(compScoreTxt.gameObject);

            var compBarFrameGO = CreateUIObject("CompBarFrame", statsBoxGO.transform);
            var compbfRect = compBarFrameGO.GetComponent<RectTransform>();
            SetAnchor(compbfRect, new Vector2(0.38f, 0.10f), new Vector2(0.96f, 0.36f), Vector2.zero, Vector2.zero);
            var compbfImg = compBarFrameGO.AddComponent<Image>();
            compbfImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_bar_frame");
            compbfImg.type = Image.Type.Sliced;
            compbfImg.color = Color.white;

            var compBarMaskGO = CreateUIObject("CompBarMask", compBarFrameGO.transform);
            var compbmRect = compBarMaskGO.GetComponent<RectTransform>();
            SetAnchor(compbmRect, Vector2.zero, Vector2.one, new Vector2(4, 3), new Vector2(-4, -3));
            compBarMaskGO.AddComponent<RectMask2D>();

            var compFillGO = CreateUIObject("CompFill", compBarMaskGO.transform);
            var compfRect = compFillGO.GetComponent<RectTransform>();
            SetAnchor(compfRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var compBarFill = compFillGO.AddComponent<Image>();
            compBarFill.sprite = UIProceduralTextureGenerator.GetSprite("rpg_bar_fill_red");
            compBarFill.type = Image.Type.Filled;
            compBarFill.fillMethod = Image.FillMethod.Horizontal;
            compBarFill.fillAmount = 0.70f;
            compBarFill.color = Color.white;

            // Buttons: Main Menu & Play Again
            var resultsMenuBtnGO = CreateUIObject("MainMenuButton", resultsModalGO.transform);
            var rmbRect = resultsMenuBtnGO.GetComponent<RectTransform>();
            rmbRect.anchorMin = new Vector2(0.5f, 0.5f);
            rmbRect.anchorMax = new Vector2(0.5f, 0.5f);
            rmbRect.sizeDelta = new Vector2(250, 52);
            rmbRect.anchoredPosition = new Vector2(-140, -285);
            var rmbImg = resultsMenuBtnGO.AddComponent<Image>();
            rmbImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_long_brown");
            rmbImg.type = Image.Type.Sliced;
            var resultsMenuBtn = resultsMenuBtnGO.AddComponent<Button>();
            var rmbTxt = CreateText(resultsMenuBtnGO.transform, "Label", "MAIN MENU", 22, FontStyle.Bold, Color.white, TextAnchor.MiddleCenter,
                Vector2.zero, Vector2.one, pixelFont);
            AddShadow(rmbTxt.gameObject);

            var resultsRetryBtnGO = CreateUIObject("PlayAgainButton", resultsModalGO.transform);
            var rrbRect = resultsRetryBtnGO.GetComponent<RectTransform>();
            rrbRect.anchorMin = new Vector2(0.5f, 0.5f);
            rrbRect.anchorMax = new Vector2(0.5f, 0.5f);
            rrbRect.sizeDelta = new Vector2(250, 52);
            rrbRect.anchoredPosition = new Vector2(140, -285);
            var rrbImg = resultsRetryBtnGO.AddComponent<Image>();
            rrbImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_long_brown");
            rrbImg.type = Image.Type.Sliced;
            var resultsRetryBtn = resultsRetryBtnGO.AddComponent<Button>();
            var rrbTxt = CreateText(resultsRetryBtnGO.transform, "Label", "PLAY AGAIN", 22, FontStyle.Bold, VisualTheme.ColorGoldAccent, TextAnchor.MiddleCenter,
                Vector2.zero, Vector2.one, pixelFont);
            AddShadow(rrbTxt.gameObject);

            resultsOverlayGO.SetActive(false);

            // Wire into coreUI
            coreUI.WireOverlays(
                pauseBtn,
                pauseOverlayGO,
                resumeBtn,
                pauseMenuBtn,
                resultsOverlayGO,
                verdictTitleTxt,
                verdictBodyTxt,
                quoteTxt,
                connScoreTxt,
                connBarFill,
                compScoreTxt,
                compBarFill,
                tierBadgeTxt,
                resultsMenuBtn,
                resultsRetryBtn);

            return coreUI;
        }

        private static (Image icon, Image fill, Text text) CreateResourceRow(Transform parent, string title, Color themeColor, Font font)
        {
            var rowGO = CreateUIObject($"Row_{title}", parent);
            var rowBg = rowGO.AddComponent<Image>();
            rowBg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_inset_brown");
            rowBg.type = Image.Type.Sliced;
            rowBg.color = Color.white;

            // Icon
            var iconGO = CreateUIObject("Icon", rowGO.transform);
            var iRect = iconGO.GetComponent<RectTransform>();
            SetAnchor(iRect, new Vector2(0.05f, 0.2f), new Vector2(0.24f, 0.8f), Vector2.zero, Vector2.zero);
            var iconImg = iconGO.AddComponent<Image>();

            // Title Label
            var label = CreateText(rowGO.transform, "Title", title, 15, FontStyle.Bold, VisualTheme.ColorGoldAccent, TextAnchor.MiddleLeft,
                new Vector2(0.28f, 0.52f), new Vector2(0.68f, 0.92f), font);

            // Numeric Readout (e.g. 80 / 100)
            var valText = CreateText(rowGO.transform, "Value", "100 / 100", 14, FontStyle.Bold, VisualTheme.ColorParchmentText, TextAnchor.MiddleRight,
                new Vector2(0.68f, 0.52f), new Vector2(0.95f, 0.92f), font);

            // Sliced Bar Slot Background with stitched 9-sliced frame
            var barBgGO = CreateUIObject("BarBG", rowGO.transform);
            var bRect = barBgGO.GetComponent<RectTransform>();
            SetAnchor(bRect, new Vector2(0.28f, 0.16f), new Vector2(0.95f, 0.44f), Vector2.zero, Vector2.zero);
            var bBg = barBgGO.AddComponent<Image>();
            bBg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_bar_frame");
            bBg.type = Image.Type.Sliced;
            bBg.color = Color.white;

            var barMaskGO = CreateUIObject("BarMask", barBgGO.transform);
            var bmRect = barMaskGO.GetComponent<RectTransform>();
            SetAnchor(bmRect, Vector2.zero, Vector2.one, new Vector2(4, 3), new Vector2(-4, -3));
            barMaskGO.AddComponent<RectMask2D>();

            string fillKey = title switch
            {
                "Oxygen" => "rpg_bar_fill_green",
                "Composure" => "rpg_bar_fill_red",
                "Connection" => "rpg_bar_fill_yellow",
                _ => "rpg_bar_fill_yellow"
            };

            var fillGO = CreateUIObject("Fill", barMaskGO.transform);
            var fRect = fillGO.GetComponent<RectTransform>();
            SetAnchor(fRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var fillImg = fillGO.AddComponent<Image>();
            fillImg.sprite = UIProceduralTextureGenerator.GetSprite(fillKey);
            fillImg.type = Image.Type.Filled;
            fillImg.fillMethod = Image.FillMethod.Horizontal;
            fillImg.fillAmount = 1f;
            fillImg.color = Color.white;

            return (iconImg, fillImg, valText);
        }

        private static GameObject CreateUIObject(string name, Transform parent)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.AddComponent<RectTransform>();
            return go;
        }

        private static Text CreateText(Transform parent, string name, string content, int fontSize, FontStyle style, Color color, TextAnchor align,
            Vector2 minAnchor, Vector2 maxAnchor, Font font)
        {
            var go = CreateUIObject(name, parent);
            var rt = go.GetComponent<RectTransform>();
            SetAnchor(rt, minAnchor, maxAnchor, Vector2.zero, Vector2.zero);

            var txt = go.AddComponent<Text>();
            txt.text = content;
            txt.fontSize = fontSize;
            txt.fontStyle = style;
            txt.color = color;
            txt.alignment = align;
            txt.font = font;
            txt.raycastTarget = false;
            return txt;
        }

        private static void AddShadow(GameObject go, Color? color = null, Vector2? dist = null)
        {
            var s = go.AddComponent<Shadow>();
            s.effectColor = color ?? new Color(0f, 0f, 0f, 0.85f);
            s.effectDistance = dist ?? new Vector2(1.5f, -1.5f);
        }

        private static void SetAnchor(RectTransform rect, Vector2 min, Vector2 max, Vector2 offsetMin, Vector2 offsetMax)
        {
            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;
        }

        private static Sprite LoadSpriteAsset(string path)
        {
#if UNITY_EDITOR
            var assets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path);
            foreach (var a in assets)
            {
                if (a is Sprite s) return s;
            }
#endif
            return null;
        }
    }
}
