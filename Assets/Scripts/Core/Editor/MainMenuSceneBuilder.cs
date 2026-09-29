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
            CreateText(titleBoxGO.transform, "TaglineText", "Balance your internal organs. Steady your pulse. Win Maya's heart.", 18, FontStyle.Normal, new Color(0.98f, 0.94f, 0.85f, 0.95f), TextAnchor.MiddleCenter,
                new Vector2(0f, 0f), new Vector2(1f, 0.20f), pixelFont);

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
            CreateText(bookBtnGO.transform, "BookLabel", "HOW TO PLAY", 14, FontStyle.Bold, new Color(1f, 0.92f, 0.70f, 1f), TextAnchor.UpperCenter,
                new Vector2(-0.5f, -0.42f), new Vector2(1.5f, 0f), pixelFont);

            // ── "How to Play" Overlay Canvas / Div ──
            var howToPlayOverlayGO = CreateUIObject("HowToPlayOverlay", canvasGO.transform);
            var htpRect = howToPlayOverlayGO.GetComponent<RectTransform>();
            SetAnchor(htpRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var htpoBg = howToPlayOverlayGO.AddComponent<Image>();
            htpoBg.color = new Color(0.04f, 0.04f, 0.06f, 0.88f);
            htpoBg.raycastTarget = true;

            var htpModalGO = CreateUIObject("ModalPanel", howToPlayOverlayGO.transform);
            var htpmRect = htpModalGO.GetComponent<RectTransform>();
            htpmRect.anchorMin = new Vector2(0.5f, 0.5f);
            htpmRect.anchorMax = new Vector2(0.5f, 0.5f);
            htpmRect.sizeDelta = new Vector2(920, 720);
            htpmRect.anchoredPosition = Vector2.zero;
            var htpmImg = htpModalGO.AddComponent<Image>();
            htpmImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_brown");
            htpmImg.type = Image.Type.Sliced;

            // Modal Header Box
            var htpHeaderGO = CreateUIObject("HeaderBox", htpModalGO.transform);
            var htphRect = htpHeaderGO.GetComponent<RectTransform>();
            SetAnchor(htphRect, new Vector2(0.03f, 0.895f), new Vector2(0.97f, 0.975f), Vector2.zero, Vector2.zero);
            var htphImg = htpHeaderGO.AddComponent<Image>();
            htphImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_inset_brown");
            htphImg.type = Image.Type.Sliced;
            htphImg.color = new Color(0.16f, 0.10f, 0.08f, 0.95f);

            CreateText(htpHeaderGO.transform, "Title", "GUIDE: HOW TO PLAY CTRL+HEART", 24, FontStyle.Bold, VisualTheme.ColorGoldAccent, TextAnchor.MiddleCenter,
                Vector2.zero, Vector2.one, pixelFont);

            // Close Button (X in corner)
            var closeBtnGO = CreateUIObject("CloseButton", htpModalGO.transform);
            var cbRect = closeBtnGO.GetComponent<RectTransform>();
            cbRect.anchorMin = new Vector2(0.93f, 0.905f);
            cbRect.anchorMax = new Vector2(0.975f, 0.965f);
            cbRect.offsetMin = Vector2.zero;
            cbRect.offsetMax = Vector2.zero;
            var cbImg = closeBtnGO.AddComponent<Image>();
            cbImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_square_brown");
            cbImg.type = Image.Type.Sliced;
            var closeBtn = closeBtnGO.AddComponent<Button>();

            CreateText(closeBtnGO.transform, "XText", "X", 22, FontStyle.Bold, new Color(1f, 0.45f, 0.45f, 1f), TextAnchor.MiddleCenter,
                Vector2.zero, Vector2.one, pixelFont);

            // Content Panel (Cards Container)
            var contentBoxGO = CreateUIObject("ContentBox", htpModalGO.transform);
            var cbContRect = contentBoxGO.GetComponent<RectTransform>();
            SetAnchor(cbContRect, new Vector2(0.03f, 0.12f), new Vector2(0.97f, 0.88f), Vector2.zero, Vector2.zero);

            var vlg = contentBoxGO.AddComponent<VerticalLayoutGroup>();
            vlg.spacing = 10;
            vlg.padding = new RectOffset(0, 0, 0, 0);
            vlg.childControlWidth = true;
            vlg.childControlHeight = true;
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight = false;

            // Card 1: The Date
            CreateInstructionCard(contentBoxGO.transform, "Card1_Date", 
                "1. THE DATE (10 CONVERSATION TURNS)",
                new Color(0.98f, 0.75f, 0.20f, 1f),
                "You are sitting across from Maya at an outdoor cafe patio. Each conversational turn presents a social moment where Maya tests your personality, reacts to your autonomic state, or shares intimate thoughts.",
                pixelFont, 82f);

            // Card 2: RTS Intervention
            CreateInstructionCard(contentBoxGO.transform, "Card2_Rts", 
                "2. RTS INTERVENTION PHASE (20 SECONDS)",
                new Color(0.22f, 0.74f, 0.97f, 1f),
                "Deploy 4 core emotional affects onto your 5 internal organs to direct your responses:\n" +
                "  - Calm (Blue): Stabilizes heart rate, protects composure, and grounds speech.\n" +
                "  - Anxiety (Purple): Emergency alertness; rapid reflexes, but taxes composure.\n" +
                "  - Confidence (Gold): Playful banter & bold charm; counters Maya's teasing.\n" +
                "  - Attraction (Pink): Romantic warmth, emotional intimacy & sparks.\n" +
                "Organs: Brain (Logic), Voice (Speech), Heart (Passion), Body (Presence), Lungs (Oxygen).\n" +
                "Cost: Deploying an affect costs 15 Oxygen (supplied by Lungs).",
                pixelFont, 192f);

            // Card 3: Spoken Words
            CreateInstructionCard(contentBoxGO.transform, "Card3_Words", 
                "3. YOUR SPOKEN WORDS (5 SECONDS)",
                new Color(0.29f, 0.87f, 0.50f, 1f),
                "Your autonomic organ balance dictates what words and tone you actually speak aloud! Steady composure unlocks confident, charming lines.",
                pixelFont, 80f);

            // Card 4: Maya's Reaction & Verdict
            CreateInstructionCard(contentBoxGO.transform, "Card4_Reaction", 
                "4. MAYA'S REACTION & THE VERDICT (5 SECONDS)",
                new Color(0.96f, 0.45f, 0.71f, 1f),
                "Watch Maya's facial micro-expressions. If Composure hits 0, you suffer an autonomic Meltdown!\n" +
                "Reach Connection Tier 3 or 4 by Turn 10 to secure a second date - or even be invited over!",
                pixelFont, 85f);

            // Bottom Confirm Button: "UNDERSTOOD"
            var bottomBtnGO = CreateUIObject("UnderstoodButton", htpModalGO.transform);
            var btbRect = bottomBtnGO.GetComponent<RectTransform>();
            btbRect.anchorMin = new Vector2(0.5f, 0.5f);
            btbRect.anchorMax = new Vector2(0.5f, 0.5f);
            btbRect.sizeDelta = new Vector2(320, 52);
            btbRect.anchoredPosition = new Vector2(0, -320);

            var btbImg = bottomBtnGO.AddComponent<Image>();
            btbImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_long_brown");
            btbImg.type = Image.Type.Sliced;
            var bottomBtn = bottomBtnGO.AddComponent<Button>();
            bottomBtn.onClick.AddListener(menuController.OnCloseHowToPlay);

            CreateText(bottomBtnGO.transform, "Label", "GOT IT - START DATE", 22, FontStyle.Bold, new Color(1f, 0.95f, 0.65f, 1f), TextAnchor.MiddleCenter,
                Vector2.zero, Vector2.one, pixelFont);

            howToPlayOverlayGO.SetActive(false);

            // Wire MainMenuController serialized fields
            var htpField = typeof(MainMenuController).GetField("howToPlayPanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var playBtnField = typeof(MainMenuController).GetField("playButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var howToPlayBtnField = typeof(MainMenuController).GetField("howToPlayButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var closeHtpField = typeof(MainMenuController).GetField("closeHowToPlayButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            htpField?.SetValue(menuController, howToPlayOverlayGO);
            playBtnField?.SetValue(menuController, playBtn);
            howToPlayBtnField?.SetValue(menuController, bookBtn);
            closeHtpField?.SetValue(menuController, closeBtn);

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

        private static void CreateInstructionCard(Transform parent, string name, string headerText, Color headerColor, string bodyText, Font font, float preferredHeight)
        {
            var cardGO = CreateUIObject(name, parent);
            var cardImg = cardGO.AddComponent<Image>();
            cardImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_panel_inset_brown");
            cardImg.type = Image.Type.Sliced;
            cardImg.color = new Color(0.16f, 0.10f, 0.08f, 0.95f);

            var le = cardGO.AddComponent<LayoutElement>();
            le.preferredHeight = preferredHeight;
            le.flexibleWidth = 1f;

            var vlg = cardGO.AddComponent<VerticalLayoutGroup>();
            vlg.padding = new RectOffset(16, 16, 8, 8);
            vlg.spacing = 4;
            vlg.childControlWidth = true;
            vlg.childControlHeight = true;
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight = false;

            // Header Text
            var headGO = CreateUIObject("Header", cardGO.transform);
            var hTxt = headGO.AddComponent<Text>();
            hTxt.text = headerText;
            hTxt.fontSize = 18;
            hTxt.fontStyle = FontStyle.Bold;
            hTxt.color = headerColor;
            hTxt.font = font;
            hTxt.alignment = TextAnchor.MiddleLeft;
            hTxt.raycastTarget = false;
            var hLe = headGO.AddComponent<LayoutElement>();
            hLe.preferredHeight = 26;

            // Body Text
            var bodyGO = CreateUIObject("Body", cardGO.transform);
            var bTxt = bodyGO.AddComponent<Text>();
            bTxt.text = bodyText;
            bTxt.fontSize = 15;
            bTxt.fontStyle = FontStyle.Normal;
            bTxt.lineSpacing = 1.25f;
            bTxt.color = new Color(0.98f, 0.95f, 0.88f, 1f); // Warm crisp cream
            bTxt.font = font;
            bTxt.alignment = TextAnchor.UpperLeft;
            bTxt.raycastTarget = false;
        }
    }
}
