using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using CtrlHeart.Core.Audio;
using CtrlHeart.Core.UI;
using CtrlHeart.Core.Visuals;

namespace CtrlHeart.Core.Editor
{
    public static class MainMenuSceneBuilder
    {
        private const string ScenePath = "Assets/Scenes/MainMenuScene.unity";

        [MenuItem("CTRL+HEART/Build Main Menu Scene")]
        public static void BuildMainMenuScene()
        {
            // Create a new empty scene
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // ── Main Camera ──
            var camGO = new GameObject("Main Camera");
            camGO.tag = "MainCamera";
            var cam = camGO.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.10f, 0.08f, 0.12f, 1f);
            cam.orthographic = false;
            camGO.AddComponent<AudioListener>();

            // ── EventSystem ──
            var esGO = new GameObject("EventSystem");
            esGO.AddComponent<UnityEngine.EventSystems.EventSystem>();
            esGO.AddComponent<InputSystemUIInputModule>();

            // ── Audio Manager ──
            var audioGO = new GameObject("AudioFeedbackManager");
            var audioMgr = audioGO.AddComponent<AudioFeedbackManager>();
            audioMgr.EnsureAudioSources();

            // Populate Audio Clips from project
            var playlistField = typeof(AudioFeedbackManager).GetField("bgmPlaylist", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var victoryField = typeof(AudioFeedbackManager).GetField("secondDateVictoryTrack", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var playlist = new System.Collections.Generic.List<AudioClip>();
            string[] trackNames = new string[]
            {
                "Blithe part A", "Blithe part B", "Autumn Leaves part A", "Autumn Leaves part B",
                "Good Old Days part A", "Good Old Days part B", "Closed Bakery part A", "Closed Bakery part B",
                "My Only Love"
            };

            foreach (var tName in trackNames)
            {
                var guids = AssetDatabase.FindAssets($"{tName} t:AudioClip");
                if (guids.Length > 0)
                {
                    var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(guids[0]));
                    if (clip != null) playlist.Add(clip);
                }
            }
            playlistField?.SetValue(audioMgr, playlist);

            var victoryGuids = AssetDatabase.FindAssets("Till Death Do Us Part t:AudioClip");
            if (victoryGuids.Length > 0)
            {
                var vClip = AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(victoryGuids[0]));
                victoryField?.SetValue(audioMgr, vClip);
            }

            // ── Canvas Setup ──
            var canvasGO = new GameObject("Canvas");
            var canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = cam;
            canvas.planeDistance = 10f;

            var scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1440, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            canvasGO.AddComponent<GraphicRaycaster>();

            var menuController = canvasGO.AddComponent<MainMenuController>();

            var defaultFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf") ?? Resources.GetBuiltinResource<Font>("Arial.ttf");
            var pixelFont = AssetDatabase.LoadAssetAtPath<Font>("Assets/Sprout Lands - UI Pack - Basic pack/fonts/pixelFont-7-8x14-sproutLands.ttf") ?? defaultFont;

            // ── Background (Outdoor Cafe Patio) ──
            var bgContainerGO = CreateUIObject("BackgroundContainer", canvasGO.transform);
            var bgContRect = bgContainerGO.GetComponent<RectTransform>();
            SetAnchor(bgContRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            bgContainerGO.AddComponent<RectMask2D>();

            var bgImageGO = CreateUIObject("BackgroundImage", bgContainerGO.transform);
            var bgImgRect = bgImageGO.GetComponent<RectTransform>();
            SetAnchor(bgImgRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var bgImg = bgImageGO.AddComponent<Image>();
            bgImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Animations/background.png");
            bgImg.color = new Color(0.88f, 0.85f, 0.82f, 1f);
            var bgFitter = bgImageGO.AddComponent<AspectRatioFitter>();
            bgFitter.aspectRatio = 2048f / 1529f;
            bgFitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;

            // Subtle dark vignette/overlay
            var vignetteGO = CreateUIObject("VignetteOverlay", canvasGO.transform);
            var vigRect = vignetteGO.GetComponent<RectTransform>();
            SetAnchor(vigRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var vigImg = vignetteGO.AddComponent<Image>();
            vigImg.color = new Color(0.08f, 0.06f, 0.12f, 0.35f);
            vigImg.raycastTarget = false;

            // ── Title Header ──
            var titleBoxGO = CreateUIObject("TitleBox", canvasGO.transform);
            var tbRect = titleBoxGO.GetComponent<RectTransform>();
            tbRect.anchorMin = new Vector2(0.5f, 0.5f);
            tbRect.anchorMax = new Vector2(0.5f, 0.5f);
            tbRect.sizeDelta = new Vector2(800, 220);
            tbRect.anchoredPosition = new Vector2(0, 200);

            // Title Shadow
            CreateText(titleBoxGO.transform, "TitleShadow", "CTRL + HEART", 68, FontStyle.Bold, new Color(0.12f, 0.07f, 0.05f, 0.85f), TextAnchor.MiddleCenter,
                new Vector2(0f, 0.38f), new Vector2(1f, 1.0f), pixelFont, new Vector2(3, -3));

            // Main Title
            CreateText(titleBoxGO.transform, "TitleText", "CTRL + HEART", 68, FontStyle.Bold, new Color(1f, 0.94f, 0.82f, 1f), TextAnchor.MiddleCenter,
                new Vector2(0f, 0.38f), new Vector2(1f, 1.0f), pixelFont);

            // Subtitle
            CreateText(titleBoxGO.transform, "SubtitleText", "An Internal Autonomic Dating Sim", 22, FontStyle.Bold, new Color(0.95f, 0.80f, 0.45f, 1f), TextAnchor.MiddleCenter,
                new Vector2(0f, 0.18f), new Vector2(1f, 0.40f), pixelFont);

            // Flavour Tagline
            CreateText(titleBoxGO.transform, "TaglineText", "Balance your internal organs. Steady your pulse. Win Maya's heart.", 16, FontStyle.Italic, new Color(0.90f, 0.86f, 0.80f, 0.9f), TextAnchor.MiddleCenter,
                new Vector2(0f, 0f), new Vector2(1f, 0.20f), defaultFont);

            // ── Center START / PLAY Button ──
            var playBtnGO = CreateUIObject("StartPlayButton", canvasGO.transform);
            var pbRect = playBtnGO.GetComponent<RectTransform>();
            pbRect.anchorMin = new Vector2(0.5f, 0.5f);
            pbRect.anchorMax = new Vector2(0.5f, 0.5f);
            pbRect.sizeDelta = new Vector2(280, 80);
            pbRect.anchoredPosition = new Vector2(0, -30);

            var pbImg = playBtnGO.AddComponent<Image>();
            pbImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_long_brown");
            pbImg.type = Image.Type.Sliced;
            var playBtn = playBtnGO.AddComponent<Button>();

            // Glowing golden play label
            CreateText(playBtnGO.transform, "PlayLabel", "START DATE", 28, FontStyle.Bold, new Color(1f, 0.98f, 0.90f, 1f), TextAnchor.MiddleCenter,
                Vector2.zero, Vector2.one, pixelFont);

            // ── Side Book Button (How To Play Guidebook) ──
            var bookBtnGO = CreateUIObject("HowToPlayBookButton", canvasGO.transform);
            var bbRect = bookBtnGO.GetComponent<RectTransform>();
            bbRect.anchorMin = new Vector2(0.5f, 0.5f);
            bbRect.anchorMax = new Vector2(0.5f, 0.5f);
            bbRect.sizeDelta = new Vector2(80, 80);
            bbRect.anchoredPosition = new Vector2(200, -30); // Placed right beside the play button

            var bbImg = bookBtnGO.AddComponent<Image>();
            bbImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_square_brown");
            bbImg.type = Image.Type.Sliced;
            var bookBtn = bookBtnGO.AddComponent<Button>();

            var bookIconGO = CreateUIObject("BookIcon", bookBtnGO.transform);
            var biRect = bookIconGO.GetComponent<RectTransform>();
            SetAnchor(biRect, new Vector2(0.12f, 0.12f), new Vector2(0.88f, 0.88f), Vector2.zero, Vector2.zero);
            var biImg = bookIconGO.AddComponent<Image>();
            biImg.sprite = UIProceduralTextureGenerator.GetSprite("icon_book");
            biImg.raycastTarget = false;

            // Small label beneath book button
            CreateText(bookBtnGO.transform, "BookLabel", "HOW TO PLAY", 11, FontStyle.Bold, new Color(1f, 0.92f, 0.70f, 1f), TextAnchor.UpperCenter,
                new Vector2(-0.5f, -0.40f), new Vector2(1.5f, 0f), pixelFont);

            // ── Side Star Button (Future Plans Roadmap) ──
            var futurePlansBtnGO = CreateUIObject("FuturePlansButton", canvasGO.transform);
            var fpbRect = futurePlansBtnGO.GetComponent<RectTransform>();
            fpbRect.anchorMin = new Vector2(0.5f, 0.5f);
            fpbRect.anchorMax = new Vector2(0.5f, 0.5f);
            fpbRect.sizeDelta = new Vector2(80, 80);
            fpbRect.anchoredPosition = new Vector2(-200, -30); // Symmetrical left side of play button

            var fpbImg = futurePlansBtnGO.AddComponent<Image>();
            fpbImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_square_brown");
            fpbImg.type = Image.Type.Sliced;
            var futurePlansBtn = futurePlansBtnGO.AddComponent<Button>();

            var fpIconGO = CreateUIObject("StarIcon", futurePlansBtnGO.transform);
            var fpiRect = fpIconGO.GetComponent<RectTransform>();
            SetAnchor(fpiRect, new Vector2(0.12f, 0.12f), new Vector2(0.88f, 0.88f), Vector2.zero, Vector2.zero);
            var fpiImg = fpIconGO.AddComponent<Image>();
            fpiImg.sprite = UIProceduralTextureGenerator.GetSprite("icon_confidence");
            fpiImg.raycastTarget = false;

            // Small label beneath future plans button
            CreateText(futurePlansBtnGO.transform, "PlansLabel", "FUTURE PLANS", 11, FontStyle.Bold, new Color(1f, 0.92f, 0.70f, 1f), TextAnchor.UpperCenter,
                new Vector2(-0.6f, -0.40f), new Vector2(1.6f, 0f), pixelFont);

            // ── "How to Play" Overlay Canvas / Div ──
            var howToPlayOverlayGO = CreateUIObject("HowToPlayOverlay", canvasGO.transform);
            var htpRect = howToPlayOverlayGO.GetComponent<RectTransform>();
            SetAnchor(htpRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var htpoBg = howToPlayOverlayGO.AddComponent<Image>();
            htpoBg.color = new Color(0.04f, 0.04f, 0.06f, 0.82f);
            htpoBg.raycastTarget = true;

            var htpModalGO = CreateUIObject("ModalPanel", howToPlayOverlayGO.transform);
            var htpmRect = htpModalGO.GetComponent<RectTransform>();
            htpmRect.anchorMin = new Vector2(0.5f, 0.5f);
            htpmRect.anchorMax = new Vector2(0.5f, 0.5f);
            htpmRect.sizeDelta = new Vector2(860, 680);
            htpmRect.anchoredPosition = Vector2.zero;
            var htpmImg = htpModalGO.AddComponent<Image>();
            htpmImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_brown");
            htpmImg.type = Image.Type.Sliced;

            // Modal Header Box
            var htpHeaderGO = CreateUIObject("HeaderBox", htpModalGO.transform);
            var htphRect = htpHeaderGO.GetComponent<RectTransform>();
            SetAnchor(htphRect, new Vector2(0.04f, 0.88f), new Vector2(0.96f, 0.97f), Vector2.zero, Vector2.zero);
            var htphImg = htpHeaderGO.AddComponent<Image>();
            htphImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_inset_brown");
            htphImg.type = Image.Type.Sliced;

            CreateText(htpHeaderGO.transform, "Title", "GUIDE: HOW TO PLAY CTRL+HEART", 22, FontStyle.Bold, VisualTheme.ColorGoldAccent, TextAnchor.MiddleCenter,
                Vector2.zero, Vector2.one, pixelFont);

            // Close Button (X in corner)
            var closeBtnGO = CreateUIObject("CloseButton", htpModalGO.transform);
            var cbRect = closeBtnGO.GetComponent<RectTransform>();
            cbRect.anchorMin = new Vector2(0.93f, 0.89f);
            cbRect.anchorMax = new Vector2(0.98f, 0.96f);
            cbRect.offsetMin = Vector2.zero;
            cbRect.offsetMax = Vector2.zero;
            var cbImg = closeBtnGO.AddComponent<Image>();
            cbImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_square_brown");
            cbImg.type = Image.Type.Sliced;
            var closeBtn = closeBtnGO.AddComponent<Button>();

            CreateText(closeBtnGO.transform, "XText", "X", 18, FontStyle.Bold, new Color(0.95f, 0.4f, 0.4f, 1f), TextAnchor.MiddleCenter,
                Vector2.zero, Vector2.one, pixelFont);

            // Content Panel (Inset Beige/Brown)
            var contentBoxGO = CreateUIObject("ContentBox", htpModalGO.transform);
            var cbContRect = contentBoxGO.GetComponent<RectTransform>();
            SetAnchor(cbContRect, new Vector2(0.04f, 0.14f), new Vector2(0.96f, 0.86f), Vector2.zero, Vector2.zero);
            var cbContImg = contentBoxGO.AddComponent<Image>();
            cbContImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_inset_brown");
            cbContImg.type = Image.Type.Sliced;

            string guideText =
                "<b><size=17><color=#FBBF24>1. THE DATE (10 SLOTS)</color></size></b>\n" +
                "You are sitting across from Maya at an outdoor café. Each slot presents a conversational moment.\n\n" +

                "<b><size=17><color=#38BDF8>2. PHASE 1: RTS INTERVENTION (20 SECONDS)</color></size></b>\n" +
                "Drag 4 core emotional affects onto your 5 internal organs to shape your thoughts:\n" +
                " • <color=#60A5FA><b>Calm</b></color>: Stabilizes heart rate, protects composure, grounds speech.\n" +
                " • <color=#C084FC><b>Anxiety</b></color>: Heightens alertness & sharp responses, but stresses composure.\n" +
                " • <color=#FDE047><b>Confidence</b></color>: Drives playful banter and boldness; shines when Maya teases.\n" +
                " • <color=#F87171><b>Attraction</b></color>: Deepens intimacy and vulnerability; creates romantic sparks.\n" +
                "<b>Organs:</b> Brain (Logic), Voice (Speech), Heart (Passion), Body (Presence), Lungs (Generates Oxygen).\n" +
                "<i>*Note: Deploying affects costs 15 Oxygen (supplied by Lungs).</i>\n\n" +

                "<b><size=17><color=#4ADE80>3. PHASE 2: YOUR SPOKEN WORDS (5 SECONDS)</color></size></b>\n" +
                "Your autonomic organ balance dictates what words and tone you actually speak aloud!\n\n" +

                "<b><size=17><color=#F472B6>4. PHASE 3: MAYA'S REACTION & THE VERDICT (5 SECONDS)</color></size></b>\n" +
                "Watch Maya's facial micro-expressions. If Composure hits 0, you suffer an autonomic Meltdown!\n" +
                "Reach <b>Connection Tier 3 or 4</b> by Slot 10 to secure a second date—or even be invited over!";

            CreateText(contentBoxGO.transform, "GuideBody", guideText, 14, FontStyle.Normal, new Color(0.96f, 0.94f, 0.90f, 1f), TextAnchor.UpperLeft,
                new Vector2(0.04f, 0.03f), new Vector2(0.96f, 0.97f), defaultFont);

            // Bottom Confirm Button: "UNDERSTOOD"
            var bottomBtnGO = CreateUIObject("UnderstoodButton", htpModalGO.transform);
            var btbRect = bottomBtnGO.GetComponent<RectTransform>();
            btbRect.anchorMin = new Vector2(0.5f, 0.5f);
            btbRect.anchorMax = new Vector2(0.5f, 0.5f);
            btbRect.sizeDelta = new Vector2(260, 48);
            btbRect.anchoredPosition = new Vector2(0, -295);

            var btbImg = bottomBtnGO.AddComponent<Image>();
            btbImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_long_brown");
            btbImg.type = Image.Type.Sliced;
            var bottomBtn = bottomBtnGO.AddComponent<Button>();
            bottomBtn.onClick.AddListener(menuController.OnCloseHowToPlay);

            CreateText(bottomBtnGO.transform, "Label", "UNDERSTOOD", 18, FontStyle.Bold, VisualTheme.ColorGoldAccent, TextAnchor.MiddleCenter,
                Vector2.zero, Vector2.one, pixelFont);

            howToPlayOverlayGO.SetActive(false);

            // ── "Future Plans" Overlay Modal ──
            var futurePlansOverlayGO = CreateUIObject("FuturePlansOverlay", canvasGO.transform);
            var fpOverlayRect = futurePlansOverlayGO.GetComponent<RectTransform>();
            SetAnchor(fpOverlayRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var fpoBg = futurePlansOverlayGO.AddComponent<Image>();
            fpoBg.color = new Color(0.04f, 0.04f, 0.06f, 0.82f);
            fpoBg.raycastTarget = true;

            var fpModalGO = CreateUIObject("ModalPanel", futurePlansOverlayGO.transform);
            var fpmRect = fpModalGO.GetComponent<RectTransform>();
            fpmRect.anchorMin = new Vector2(0.5f, 0.5f);
            fpmRect.anchorMax = new Vector2(0.5f, 0.5f);
            fpmRect.sizeDelta = new Vector2(880, 680);
            fpmRect.anchoredPosition = Vector2.zero;
            var fpmImg = fpModalGO.AddComponent<Image>();
            fpmImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_brown");
            fpmImg.type = Image.Type.Sliced;

            // Modal Header Box
            var fpHeaderGO = CreateUIObject("HeaderBox", fpModalGO.transform);
            var fphRect = fpHeaderGO.GetComponent<RectTransform>();
            SetAnchor(fphRect, new Vector2(0.04f, 0.88f), new Vector2(0.96f, 0.97f), Vector2.zero, Vector2.zero);
            var fphImg = fpHeaderGO.AddComponent<Image>();
            fphImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_inset_brown");
            fphImg.type = Image.Type.Sliced;

            CreateText(fpHeaderGO.transform, "Title", "FUTURE PLANS & ROADMAP", 22, FontStyle.Bold, VisualTheme.ColorGoldAccent, TextAnchor.MiddleCenter,
                Vector2.zero, Vector2.one, pixelFont);

            // Close Button (X in corner)
            var closeFpBtnGO = CreateUIObject("CloseButton", fpModalGO.transform);
            var cfpRect = closeFpBtnGO.GetComponent<RectTransform>();
            cfpRect.anchorMin = new Vector2(0.93f, 0.89f);
            cfpRect.anchorMax = new Vector2(0.98f, 0.96f);
            cfpRect.offsetMin = Vector2.zero;
            cfpRect.offsetMax = Vector2.zero;
            var cfpImg = closeFpBtnGO.AddComponent<Image>();
            cfpImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_square_brown");
            cfpImg.type = Image.Type.Sliced;
            var closeFpBtn = closeFpBtnGO.AddComponent<Button>();

            CreateText(closeFpBtnGO.transform, "XText", "X", 18, FontStyle.Bold, new Color(0.95f, 0.4f, 0.4f, 1f), TextAnchor.MiddleCenter,
                Vector2.zero, Vector2.one, pixelFont);

            // Content Panel (Inset Beige/Brown)
            var fpContentBoxGO = CreateUIObject("ContentBox", fpModalGO.transform);
            var fpcRect = fpContentBoxGO.GetComponent<RectTransform>();
            SetAnchor(fpcRect, new Vector2(0.04f, 0.14f), new Vector2(0.96f, 0.86f), Vector2.zero, Vector2.zero);
            var fpcImg = fpContentBoxGO.AddComponent<Image>();
            fpcImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_inset_brown");
            fpcImg.type = Image.Type.Sliced;

            string roadmapText =
                "<b><size=17><color=#FBBF24>1. EXPANDED ROSTER & NEW CHARACTERS</color></size></b>\n" +
                "• Adding fully written <b>Male and Female</b> dating routes, each with unique dialogue, neuroses, and emotional quirks.\n" +
                "• <i>(Community Demands Acknowledged):</i> Yes, by overwhelming request... <b>Goth Girls, Dominant Mommys, & Soft Femboys</b> are currently undergoing biological stress simulations. Our lab is on it. Stay tuned!\n\n" +

                "<b><size=17><color=#38BDF8>2. FULL VOICE-OVER (VOICE ACTING)</color></size></b>\n" +
                "• Expressive voice acting for both your date partner and your panicked internal monologue.\n" +
                "• Dynamic audio cues that react to your heart rate—nervous stutters, bold quips, and breathless pauses!\n\n" +

                "<b><size=17><color=#4ADE80>3. DATE LOCATION SELECTION</color></size></b>\n" +
                "• Choose your battleground: <b>Candlelight Bistro, Retro Arcade, Rainy Park Bench, or Bustling Night Market</b>.\n" +
                "• Each venue brings unique environmental stressors (e.g. Arcades drain Focus; Fancy Dining drains Composure)!\n\n" +

                "<b><size=17><color=#F472B6>4. MORE ORGANS, EMOTIONS & MINI-GAMES</color></size></b>\n" +
                "• Introducing <b>The Stomach</b> (Butterflies vs Acid Reflux) and <b>Sweaty Palms</b> as active organ nodes!\n" +
                "• Secret mixture emotions, unlockable outfits, and 10+ divergent branch endings.";

            CreateText(fpContentBoxGO.transform, "RoadmapBody", roadmapText, 14, FontStyle.Normal, new Color(0.96f, 0.94f, 0.90f, 1f), TextAnchor.UpperLeft,
                new Vector2(0.04f, 0.03f), new Vector2(0.96f, 0.97f), defaultFont);

            // Bottom Confirm Button: "HYPED!"
            var fpBottomBtnGO = CreateUIObject("HypedButton", fpModalGO.transform);
            var fpbbtRect = fpBottomBtnGO.GetComponent<RectTransform>();
            fpbbtRect.anchorMin = new Vector2(0.5f, 0.5f);
            fpbbtRect.anchorMax = new Vector2(0.5f, 0.5f);
            fpbbtRect.sizeDelta = new Vector2(260, 48);
            fpbbtRect.anchoredPosition = new Vector2(0, -295);

            var fpbbtImg = fpBottomBtnGO.AddComponent<Image>();
            fpbbtImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_long_brown");
            fpbbtImg.type = Image.Type.Sliced;
            var fpBottomBtn = fpBottomBtnGO.AddComponent<Button>();
            fpBottomBtn.onClick.AddListener(menuController.OnCloseFuturePlans);

            CreateText(fpBottomBtnGO.transform, "Label", "HYPED!", 18, FontStyle.Bold, VisualTheme.ColorGoldAccent, TextAnchor.MiddleCenter,
                Vector2.zero, Vector2.one, pixelFont);

            futurePlansOverlayGO.SetActive(false);

            // Wire MainMenuController serialized fields
            var htpField = typeof(MainMenuController).GetField("howToPlayPanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var playBtnField = typeof(MainMenuController).GetField("playButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var howToPlayBtnField = typeof(MainMenuController).GetField("howToPlayButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var closeHtpField = typeof(MainMenuController).GetField("closeHowToPlayButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var fpPanelField = typeof(MainMenuController).GetField("futurePlansPanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var fpBtnField = typeof(MainMenuController).GetField("futurePlansButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var closeFpBtnField = typeof(MainMenuController).GetField("closeFuturePlansButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var understoodBtnField = typeof(MainMenuController).GetField("understoodButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var hypedBtnField = typeof(MainMenuController).GetField("hypedButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            htpField?.SetValue(menuController, howToPlayOverlayGO);
            playBtnField?.SetValue(menuController, playBtn);
            howToPlayBtnField?.SetValue(menuController, bookBtn);
            closeHtpField?.SetValue(menuController, closeBtn);
            understoodBtnField?.SetValue(menuController, bottomBtn);

            fpPanelField?.SetValue(menuController, futurePlansOverlayGO);
            fpBtnField?.SetValue(menuController, futurePlansBtn);
            closeFpBtnField?.SetValue(menuController, closeFpBtn);
            hypedBtnField?.SetValue(menuController, fpBottomBtn);

            // Save Scene
            EditorSceneManager.SaveScene(scene, ScenePath);
            Debug.Log($"<color=green>[MainMenuSceneBuilder] Successfully created and saved '{ScenePath}'!</color>");
        }

        private static GameObject CreateUIObject(string name, Transform parent)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.AddComponent<RectTransform>();
            return go;
        }

        private static Text CreateText(Transform parent, string name, string content, int fontSize, FontStyle style, Color color, TextAnchor align,
            Vector2 minAnchor, Vector2 maxAnchor, Font font, Vector2? shadowOffset = null)
        {
            var go = CreateUIObject(name, parent);
            var rt = go.GetComponent<RectTransform>();
            SetAnchor(rt, minAnchor, maxAnchor, shadowOffset ?? Vector2.zero, shadowOffset ?? Vector2.zero);

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

        private static void SetAnchor(RectTransform rect, Vector2 min, Vector2 max, Vector2 offsetMin, Vector2 offsetMax)
        {
            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;
        }
    }
}
