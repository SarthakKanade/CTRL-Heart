using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

/// <summary>
/// Creates the entire Phase 0 UI programmatically and keeps it updated.
///
/// Layout (persistent split-screen per the Game Bible §4):
///   Top ~48%  = The Date   (warm café aesthetic)
///   Bottom ~50% = Internal World (dark control-room aesthetic)
///   Thin divider between them
///
/// Plus full-screen overlays for Intro / Transition / Result.
/// </summary>
public class Phase0UI : MonoBehaviour
{
    // ════════════════════════════════════════════════════════
    // UI ELEMENT REFERENCES
    // ════════════════════════════════════════════════════════

    // ── Canvas ──
    private Canvas rootCanvas;

    // ── Date Panel ──
    private Text questionNumberText;
    private Text questionText;
    private Text responseText;
    private Image responseBG;
    private Text reactionText;
    private Image timerFill;

    // ── Internal Panel ──
    private Image brainCircleImage;
    private Image brainHealthFill;
    private Text brainDominantText;
    private GameObject stressAlertGO;
    private Text stressAlertText;
    private Image calmButtonImage;
    private Image anxietyButtonImage;
    private Image calmCooldownOverlay;
    private Image anxietyCooldownOverlay;
    private Image focusFill;
    private Text focusValueText;
    private Text statusText;
    private Image calmSelectionRing;
    private Image anxietySelectionRing;

    // ── Overlays ──
    private GameObject introOverlay;
    private Text introTitleText;
    private Text introSubText;
    private GameObject transitionOverlay;
    private Text transitionText;
    private GameObject resultOverlay;
    private Text resultTitleText;
    private Text resultDetailText;
    private Text resultScoreText;

    // ── Cached ──
    private Sprite circleSprite;
    private Font uiFont;

    // ════════════════════════════════════════════════════════
    // COLORS
    // ════════════════════════════════════════════════════════

    static readonly Color COL_DATE_BG       = new Color(1.00f, 0.95f, 0.88f);       // Warm cream
    static readonly Color COL_INTERNAL_BG   = new Color(0.04f, 0.09f, 0.15f);       // Dark navy
    static readonly Color COL_DIVIDER       = new Color(0.39f, 1.00f, 0.85f, 0.5f); // Teal glow
    static readonly Color COL_CALM          = new Color(0.26f, 0.65f, 0.96f);       // Blue
    static readonly Color COL_CALM_DARK     = new Color(0.08f, 0.40f, 0.75f);
    static readonly Color COL_ANXIETY       = new Color(1.00f, 0.65f, 0.15f);       // Orange
    static readonly Color COL_ANXIETY_DARK  = new Color(0.90f, 0.32f, 0.00f);
    static readonly Color COL_BRAIN         = new Color(0.15f, 0.78f, 0.71f);       // Teal
    static readonly Color COL_FOCUS         = new Color(0.67f, 0.28f, 0.74f);       // Purple
    static readonly Color COL_STRESS        = new Color(1.00f, 0.10f, 0.27f);       // Red
    static readonly Color COL_HEALTH_GOOD   = new Color(0.30f, 0.87f, 0.47f);       // Green
    static readonly Color COL_HEALTH_BAD    = new Color(1.00f, 0.25f, 0.25f);       // Red
    static readonly Color COL_TEXT_DARK     = new Color(0.24f, 0.15f, 0.14f);       // Dark brown
    static readonly Color COL_TEXT_LIGHT    = new Color(0.88f, 0.88f, 0.88f);       // Light gray
    static readonly Color COL_TEXT_DIM      = new Color(0.69f, 0.75f, 0.77f);       // Blue-gray
    static readonly Color COL_TIER0         = new Color(0.18f, 0.69f, 0.29f);       // Green
    static readonly Color COL_TIER1         = new Color(0.56f, 0.72f, 0.22f);       // Yellow-green
    static readonly Color COL_TIER2         = new Color(0.90f, 0.60f, 0.10f);       // Orange
    static readonly Color COL_TIER3         = new Color(0.90f, 0.22f, 0.21f);       // Red

    // ════════════════════════════════════════════════════════
    // BUILD UI
    // ════════════════════════════════════════════════════════

    public void BuildUI()
    {
        circleSprite = CreateCircleSprite(128);
        uiFont = GetDefaultFont();

        CreateEventSystem();
        CreateCanvas();
        CreateDatePanel();
        CreateDivider();
        CreateInternalPanel();
        CreateOverlays();
    }

    // ── Canvas ─────────────────────────────────────────────

    void CreateEventSystem()
    {
        var es = FindAnyObjectByType<EventSystem>();
        if (es == null)
        {
            var go = new GameObject("EventSystem");
            es = go.AddComponent<EventSystem>();
        }

        // Clean up legacy StandaloneInputModule if present to avoid Input System exceptions
        var legacyModule = es.GetComponent<StandaloneInputModule>();
        if (legacyModule != null)
        {
            DestroyImmediate(legacyModule);
        }

        // Ensure InputSystemUIInputModule is present
        var inputSystemModule = es.GetComponent<InputSystemUIInputModule>();
        if (inputSystemModule == null)
        {
            es.gameObject.AddComponent<InputSystemUIInputModule>();
        }
    }

    void CreateCanvas()
    {
        var canvasGO = new GameObject("Phase0Canvas");
        rootCanvas = canvasGO.AddComponent<Canvas>();
        rootCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        rootCanvas.sortingOrder = 0;

        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        canvasGO.AddComponent<GraphicRaycaster>();
    }

    // ── Date Panel (top half) ──────────────────────────────

    void CreateDatePanel()
    {
        var panel = CreatePanel(rootCanvas.transform, "DatePanel", COL_DATE_BG,
            new Vector2(0, 0.50f), Vector2.one,
            new Vector2(0, 0), new Vector2(0, 0)).transform;

        // Question number label
        questionNumberText = CreateText(panel, "QuestionNumber", "",
            22, COL_TEXT_DIM, TextAnchor.UpperLeft,
            new Vector2(0.04f, 0.85f), new Vector2(0.35f, 0.96f));

        // Date character placeholder (simple colored circle with initial)
        var avatarBG = CreateCircle(panel, "DateAvatar", new Color(0.85f, 0.55f, 0.45f),
            new Vector2(0.12f, 0.62f), 70f);
        CreateTextOnRect(avatarBG.transform, "DateInitial", "A",
            32, Color.white, TextAnchor.MiddleCenter);

        var avatarLabel = CreateText(panel, "DateName", "Alex",
            18, COL_TEXT_DARK, TextAnchor.MiddleCenter,
            new Vector2(0.06f, 0.48f), new Vector2(0.18f, 0.56f));

        // Question text
        questionText = CreateText(panel, "QuestionText", "",
            30, COL_TEXT_DARK, TextAnchor.MiddleCenter,
            new Vector2(0.22f, 0.45f), new Vector2(0.95f, 0.85f));

        // Response background + text
        var respBG = CreatePanel(panel, "ResponseBG", new Color(0, 0, 0, 0),
            new Vector2(0.22f, 0.18f), new Vector2(0.95f, 0.44f));
        responseBG = respBG.GetComponent<Image>();

        responseText = CreateText(respBG.transform, "ResponseText", "",
            22, COL_TIER0, TextAnchor.MiddleCenter,
            Vector2.zero, Vector2.one,
            new Vector2(12, 6), new Vector2(-12, -6));
        responseText.gameObject.SetActive(false);

        // Reaction text
        reactionText = CreateText(panel, "ReactionText", "",
            20, new Color(0.4f, 0.3f, 0.3f), TextAnchor.MiddleCenter,
            new Vector2(0.22f, 0.03f), new Vector2(0.95f, 0.20f));
        reactionText.fontStyle = FontStyle.Italic;
        reactionText.gameObject.SetActive(false);

        // Timer bar
        var timerBG = CreatePanel(panel, "TimerBarBG", new Color(0, 0, 0, 0.15f),
            new Vector2(0.04f, 0.01f), new Vector2(0.96f, 0.04f)).transform;

        var timerFillGO = new GameObject("TimerFill");
        SetRect(timerFillGO, timerBG.transform,
            Vector2.zero, new Vector2(1, 1),
            Vector2.zero, Vector2.zero);
        timerFill = timerFillGO.AddComponent<Image>();
        timerFill.color = Color.white;
        timerFill.type = Image.Type.Filled;
        timerFill.fillMethod = Image.FillMethod.Horizontal;
        timerFill.fillOrigin = 0;
        timerFill.fillAmount = 1f;
    }

    // ── Divider ────────────────────────────────────────────

    void CreateDivider()
    {
        CreatePanel(rootCanvas.transform, "Divider", COL_DIVIDER,
            new Vector2(0.02f, 0.49f), new Vector2(0.98f, 0.505f));
    }

    // ── Internal Panel (bottom half) ───────────────────────

    void CreateInternalPanel()
    {
        var panel = CreatePanel(rootCanvas.transform, "InternalPanel", COL_INTERNAL_BG,
            Vector2.zero, new Vector2(1, 0.485f),
            Vector2.zero, Vector2.zero).transform;

        // Title
        CreateText(panel, "InternalTitle", "\u2699 INTERNAL CONTROL",
            24, COL_TEXT_DIM, TextAnchor.MiddleCenter,
            new Vector2(0.25f, 0.90f), new Vector2(0.75f, 0.98f));

        // ── Brain Node ────────────────────────────────

        var brainCircleGO = CreateCircle(panel, "BrainCircle", COL_BRAIN,
            new Vector2(0.5f, 0.58f), 80f);

        // Attach BrainNode script
        var brainNode = brainCircleGO.gameObject.AddComponent<BrainNode>();
        Phase0Game.Instance.Brain = brainNode;
        brainCircleImage = brainCircleGO.gameObject.GetComponent<Image>();

        // "BRAIN" label above
        CreateText(panel, "BrainLabel", "BRAIN",
            18, COL_TEXT_LIGHT, TextAnchor.MiddleCenter,
            new Vector2(0.42f, 0.80f), new Vector2(0.58f, 0.88f));

        // Health bar background
        var healthBG = CreatePanel(panel, "HealthBarBG", new Color(0, 0, 0, 0.4f),
            new Vector2(0.40f, 0.42f), new Vector2(0.60f, 0.46f)).transform;

        // Health bar fill
        var healthFillGO = new GameObject("HealthFill");
        SetRect(healthFillGO, healthBG.transform,
            Vector2.zero, Vector2.one,
            new Vector2(2, 2), new Vector2(-2, -2));
        brainHealthFill = healthFillGO.AddComponent<Image>();
        brainHealthFill.color = COL_HEALTH_GOOD;
        brainHealthFill.type = Image.Type.Filled;
        brainHealthFill.fillMethod = Image.FillMethod.Horizontal;
        brainHealthFill.fillAmount = 0.8f;

        // Dominant emotion label
        brainDominantText = CreateText(panel, "DominantText", "Neutral",
            16, COL_TEXT_DIM, TextAnchor.MiddleCenter,
            new Vector2(0.35f, 0.36f), new Vector2(0.65f, 0.42f));

        // Stress alert (hidden by default)
        stressAlertGO = new GameObject("StressAlert");
        SetRect(stressAlertGO, panel.gameObject.transform,
            new Vector2(0.25f, 0.82f), new Vector2(0.75f, 0.92f));
        var alertBG = stressAlertGO.AddComponent<Image>();
        alertBG.color = new Color(COL_STRESS.r, COL_STRESS.g, COL_STRESS.b, 0.2f);
        stressAlertText = CreateText(stressAlertGO.transform, "StressAlertText", "",
            20, COL_STRESS, TextAnchor.MiddleCenter,
            Vector2.zero, Vector2.one);
        stressAlertText.fontStyle = FontStyle.Bold;
        stressAlertGO.SetActive(false);

        // ── Emotion Buttons ───────────────────────────

        // Calm button
        var calmBtnGO = CreateCircle(panel, "CalmButton", COL_CALM,
            new Vector2(0.30f, 0.18f), 55f);
        calmButtonImage = calmBtnGO.gameObject.GetComponent<Image>();
        var calmDragger = calmBtnGO.gameObject.AddComponent<EmotionDragger>();
        calmDragger.emotionType = EmotionType.Calm;

        // Selection ring for Calm
        var calmRingGO = CreateCircle(calmBtnGO, "CalmRing", Color.white,
            new Vector2(0.5f, 0.5f), 65f);
        calmSelectionRing = calmRingGO.gameObject.GetComponent<Image>();
        calmSelectionRing.color = new Color(1, 1, 1, 0);
        calmRingGO.gameObject.GetComponent<Image>().raycastTarget = false;

        CreateText(panel, "CalmLabel", "CALM\n(Heal + Stabilise)",
            14, COL_CALM, TextAnchor.MiddleCenter,
            new Vector2(0.20f, 0.02f), new Vector2(0.40f, 0.12f));

        // Calm cooldown overlay
        var calmCdGO = new GameObject("CalmCooldown");
        SetRect(calmCdGO, calmBtnGO,
            Vector2.zero, Vector2.one,
            Vector2.zero, Vector2.zero);
        calmCooldownOverlay = calmCdGO.AddComponent<Image>();
        calmCooldownOverlay.sprite = circleSprite;
        calmCooldownOverlay.color = new Color(0, 0, 0, 0.5f);
        calmCooldownOverlay.type = Image.Type.Filled;
        calmCooldownOverlay.fillMethod = Image.FillMethod.Radial360;
        calmCooldownOverlay.fillClockwise = false;
        calmCooldownOverlay.fillAmount = 0f;
        calmCooldownOverlay.raycastTarget = false;

        // Anxiety button
        var anxBtnGO = CreateCircle(panel, "AnxietyButton", COL_ANXIETY,
            new Vector2(0.70f, 0.18f), 55f);
        anxietyButtonImage = anxBtnGO.gameObject.GetComponent<Image>();
        var anxDragger = anxBtnGO.gameObject.AddComponent<EmotionDragger>();
        anxDragger.emotionType = EmotionType.Anxiety;

        // Selection ring for Anxiety
        var anxRingGO = CreateCircle(anxBtnGO, "AnxietyRing", Color.white,
            new Vector2(0.5f, 0.5f), 65f);
        anxietySelectionRing = anxRingGO.gameObject.GetComponent<Image>();
        anxietySelectionRing.color = new Color(1, 1, 1, 0);
        anxRingGO.gameObject.GetComponent<Image>().raycastTarget = false;

        CreateText(panel, "AnxietyLabel", "ANXIETY\n(+15 Focus, hurts Brain)",
            14, COL_ANXIETY, TextAnchor.MiddleCenter,
            new Vector2(0.58f, 0.02f), new Vector2(0.82f, 0.12f));

        // Anxiety cooldown overlay
        var anxCdGO = new GameObject("AnxietyCooldown");
        SetRect(anxCdGO, anxBtnGO,
            Vector2.zero, Vector2.one,
            Vector2.zero, Vector2.zero);
        anxietyCooldownOverlay = anxCdGO.AddComponent<Image>();
        anxietyCooldownOverlay.sprite = circleSprite;
        anxietyCooldownOverlay.color = new Color(0, 0, 0, 0.5f);
        anxietyCooldownOverlay.type = Image.Type.Filled;
        anxietyCooldownOverlay.fillMethod = Image.FillMethod.Radial360;
        anxietyCooldownOverlay.fillClockwise = false;
        anxietyCooldownOverlay.fillAmount = 0f;
        anxietyCooldownOverlay.raycastTarget = false;

        // ── Focus Meter ───────────────────────────────

        CreateText(panel, "FocusLabel", "FOCUS",
            16, COL_FOCUS, TextAnchor.MiddleCenter,
            new Vector2(0.88f, 0.82f), new Vector2(0.97f, 0.90f));

        var focusBG = CreatePanel(panel, "FocusBarBG", new Color(0, 0, 0, 0.4f),
            new Vector2(0.905f, 0.20f), new Vector2(0.945f, 0.80f)).transform;

        var focusFillGO = new GameObject("FocusFill");
        SetRect(focusFillGO, focusBG.transform,
            Vector2.zero, Vector2.one,
            new Vector2(2, 2), new Vector2(-2, -2));
        focusFill = focusFillGO.AddComponent<Image>();
        focusFill.color = COL_FOCUS;
        focusFill.type = Image.Type.Filled;
        focusFill.fillMethod = Image.FillMethod.Vertical;
        focusFill.fillOrigin = 0;
        focusFill.fillAmount = 0.7f;

        focusValueText = CreateText(panel, "FocusValue", "70",
            14, COL_TEXT_LIGHT, TextAnchor.MiddleCenter,
            new Vector2(0.88f, 0.13f), new Vector2(0.97f, 0.20f));

        // ── Status Text ───────────────────────────────

        statusText = CreateText(panel, "StatusText", "Click an emotion, then click Brain  \u2014  or drag it!",
            15, COL_TEXT_DIM, TextAnchor.MiddleCenter,
            new Vector2(0.15f, 0.00f), new Vector2(0.85f, 0.06f));
    }

    // ── Overlays ───────────────────────────────────────────

    void CreateOverlays()
    {
        // ── Intro Overlay ─────────────────────────────
        introOverlay = CreateFullOverlay("IntroOverlay", new Color(0.02f, 0.05f, 0.10f, 0.95f));

        introTitleText = CreateText(introOverlay.transform, "IntroTitle", "CTRL+HEART",
            60, COL_BRAIN, TextAnchor.MiddleCenter,
            new Vector2(0.1f, 0.50f), new Vector2(0.9f, 0.70f));
        introTitleText.fontStyle = FontStyle.Bold;

        introSubText = CreateText(introOverlay.transform, "IntroSub",
            "First date. Caf\u00e9 downtown.\n\nYou're not the person on the date.\nYou're the panic room inside their head.\n\n<i>Manage their emotions. Keep them together.</i>",
            24, COL_TEXT_LIGHT, TextAnchor.MiddleCenter,
            new Vector2(0.15f, 0.20f), new Vector2(0.85f, 0.50f));
        introSubText.supportRichText = true;

        introOverlay.SetActive(false);

        // ── Transition Overlay ────────────────────────
        transitionOverlay = CreateFullOverlay("TransitionOverlay", new Color(0.02f, 0.05f, 0.10f, 0.85f));

        transitionText = CreateText(transitionOverlay.transform, "TransitionText", "",
            36, COL_TEXT_LIGHT, TextAnchor.MiddleCenter,
            new Vector2(0.1f, 0.35f), new Vector2(0.9f, 0.65f));

        transitionOverlay.SetActive(false);

        // ── Result Overlay ────────────────────────────
        resultOverlay = CreateFullOverlay("ResultOverlay", new Color(0.02f, 0.05f, 0.10f, 0.92f));

        resultTitleText = CreateText(resultOverlay.transform, "ResultTitle", "",
            48, COL_BRAIN, TextAnchor.MiddleCenter,
            new Vector2(0.1f, 0.60f), new Vector2(0.9f, 0.80f));
        resultTitleText.fontStyle = FontStyle.Bold;

        resultDetailText = CreateText(resultOverlay.transform, "ResultDetail", "",
            24, COL_TEXT_LIGHT, TextAnchor.MiddleCenter,
            new Vector2(0.1f, 0.40f), new Vector2(0.9f, 0.60f));

        resultScoreText = CreateText(resultOverlay.transform, "ResultScore", "",
            18, COL_TEXT_DIM, TextAnchor.MiddleCenter,
            new Vector2(0.1f, 0.22f), new Vector2(0.9f, 0.40f));

        // Restart button
        var btnGO = new GameObject("RestartButton");
        SetRect(btnGO, resultOverlay.transform,
            new Vector2(0.35f, 0.08f), new Vector2(0.65f, 0.18f));
        var btnImg = btnGO.AddComponent<Image>();
        btnImg.color = COL_BRAIN;
        var btn = btnGO.AddComponent<Button>();
        btn.targetGraphic = btnImg;

        // Button color block
        var colors = btn.colors;
        colors.normalColor = COL_BRAIN;
        colors.highlightedColor = new Color(COL_BRAIN.r + 0.1f, COL_BRAIN.g + 0.1f, COL_BRAIN.b + 0.1f);
        colors.pressedColor = new Color(COL_BRAIN.r - 0.1f, COL_BRAIN.g - 0.1f, COL_BRAIN.b - 0.1f);
        btn.colors = colors;

        btn.onClick.AddListener(() => Phase0Game.Instance.RestartGame());

        CreateText(btnGO.transform, "BtnLabel", "PLAY AGAIN",
            22, Color.white, TextAnchor.MiddleCenter,
            Vector2.zero, Vector2.one);

        resultOverlay.SetActive(false);
    }

    // ════════════════════════════════════════════════════════
    // UPDATE VISUALS (called every frame by Phase0Game)
    // ════════════════════════════════════════════════════════

    public void UpdateVisuals()
    {
        var game = Phase0Game.Instance;
        if (game == null || game.Brain == null) return;
        var brain = game.Brain;

        // ── Timer bar ─────────────────────────────────
        if (game.State == Phase0Game.GameState.QuestionActive && game.ResponseTimerMax > 0)
        {
            float pct = game.ResponseTimer / game.ResponseTimerMax;
            timerFill.fillAmount = pct;

            // Color: white → yellow → red
            if (pct > 0.5f)
                timerFill.color = Color.Lerp(new Color(1f, 0.9f, 0.3f), Color.white, (pct - 0.5f) * 2f);
            else
                timerFill.color = Color.Lerp(COL_STRESS, new Color(1f, 0.9f, 0.3f), pct * 2f);
        }

        // ── Brain circle color ────────────────────────
        Color targetBrainColor = COL_BRAIN;
        if (brain.isUnderStress)
        {
            float pulse = Mathf.PingPong(Time.time * 3f, 1f);
            targetBrainColor = Color.Lerp(COL_BRAIN, COL_STRESS, pulse * 0.7f);
        }
        else if (brain.isReceivingCalm)
        {
            targetBrainColor = Color.Lerp(COL_BRAIN, COL_CALM, 0.4f);
        }
        else if (brain.isReceivingAnxiety)
        {
            targetBrainColor = Color.Lerp(COL_BRAIN, COL_ANXIETY, 0.4f);
        }
        brainCircleImage.color = Color.Lerp(brainCircleImage.color, targetBrainColor, Time.deltaTime * 5f);

        // ── Brain health bar ──────────────────────────
        float healthPct = brain.HealthPercent;
        brainHealthFill.fillAmount = Mathf.Lerp(brainHealthFill.fillAmount, healthPct, Time.deltaTime * 8f);
        brainHealthFill.color = Color.Lerp(COL_HEALTH_BAD, COL_HEALTH_GOOD, healthPct);

        // ── Dominant emotion ──────────────────────────
        switch (brain.DominantEmotion)
        {
            case EmotionType.Calm:
                brainDominantText.text = "Calm dominant";
                brainDominantText.color = COL_CALM;
                break;
            case EmotionType.Anxiety:
                brainDominantText.text = "Anxiety dominant";
                brainDominantText.color = COL_ANXIETY;
                break;
            default:
                brainDominantText.text = "Neutral";
                brainDominantText.color = COL_TEXT_DIM;
                break;
        }

        // ── Focus bar ─────────────────────────────────
        float focusPct = game.Focus / Phase0Game.MAX_FOCUS;
        focusFill.fillAmount = Mathf.Lerp(focusFill.fillAmount, focusPct, Time.deltaTime * 8f);
        focusValueText.text = Mathf.RoundToInt(game.Focus).ToString();

        // Focus bar color dims when low
        focusFill.color = Color.Lerp(
            new Color(COL_FOCUS.r * 0.4f, COL_FOCUS.g * 0.4f, COL_FOCUS.b * 0.4f),
            COL_FOCUS,
            focusPct);

        // ── Emotion cooldown overlays ─────────────────
        float cdPct = game.EmotionCooldown / Phase0Game.COOLDOWN_DURATION;
        calmCooldownOverlay.fillAmount = cdPct;
        anxietyCooldownOverlay.fillAmount = cdPct;

        // ── Stress alert flash ────────────────────────
        if (stressAlertGO.activeSelf)
        {
            float flash = Mathf.PingPong(Time.time * 4f, 1f);
            stressAlertText.color = new Color(COL_STRESS.r, COL_STRESS.g, COL_STRESS.b,
                0.6f + flash * 0.4f);
        }

        // ── Selection ring pulse ──────────────────────
        float ringPulse = 0.5f + Mathf.PingPong(Time.time * 2f, 0.5f);
        if (game.SelectedEmotion == EmotionType.Calm)
        {
            calmSelectionRing.color = new Color(1, 1, 1, ringPulse);
            anxietySelectionRing.color = new Color(1, 1, 1, 0);
        }
        else if (game.SelectedEmotion == EmotionType.Anxiety)
        {
            anxietySelectionRing.color = new Color(1, 1, 1, ringPulse);
            calmSelectionRing.color = new Color(1, 1, 1, 0);
        }
    }

    // ════════════════════════════════════════════════════════
    // SHOW / HIDE METHODS (called by Phase0Game)
    // ════════════════════════════════════════════════════════

    public void ShowIntro()
    {
        introOverlay.SetActive(true);
        transitionOverlay.SetActive(false);
        resultOverlay.SetActive(false);
        questionText.text = "";
        questionNumberText.text = "";
        responseText.gameObject.SetActive(false);
        reactionText.gameObject.SetActive(false);
        timerFill.fillAmount = 0f;
    }

    public void ShowQuestion(string question, int num)
    {
        introOverlay.SetActive(false);
        transitionOverlay.SetActive(false);

        questionNumberText.text = $"Question {num} of 4";
        questionText.text = question;
        responseText.gameObject.SetActive(false);
        reactionText.gameObject.SetActive(false);
        timerFill.fillAmount = 1f;

        statusText.text = "Click an emotion, then click Brain  \u2014  or drag it!";
    }

    public void ShowResponse(string response, int tier)
    {
        responseText.gameObject.SetActive(true);
        responseText.text = response;

        Color textColor;
        Color bgColor;
        switch (tier)
        {
            case 0:  textColor = COL_TIER0; bgColor = new Color(0.1f, 0.5f, 0.2f, 0.12f); break;
            case 1:  textColor = COL_TIER1; bgColor = new Color(0.4f, 0.5f, 0.1f, 0.12f); break;
            case 2:  textColor = COL_TIER2; bgColor = new Color(0.5f, 0.3f, 0.05f, 0.12f); break;
            default: textColor = COL_TIER3; bgColor = new Color(0.5f, 0.1f, 0.1f, 0.12f); break;
        }
        responseText.color = textColor;
        responseBG.color = bgColor;

        string[] tierLabels = { "\u2b50 Great response!", "\u2705 Decent.", "\u26a0 Shaky...", "\u274c Disaster!" };
        statusText.text = tierLabels[Mathf.Clamp(tier, 0, 3)];
        statusText.color = textColor;
    }

    public void ShowReaction(string reaction)
    {
        reactionText.gameObject.SetActive(true);
        reactionText.text = reaction;
    }

    public void ShowTransition(int nextQuestionNum)
    {
        transitionOverlay.SetActive(true);
        transitionText.text = $"Question {nextQuestionNum} of 4\n\n<size=20>Take a breath...</size>";
        transitionText.supportRichText = true;

        statusText.text = "";
    }

    public void ShowStressAlert(string msg)
    {
        stressAlertGO.SetActive(true);
        stressAlertText.text = msg;
        statusText.text = "STRESS! Assign Calm to mitigate!";
        statusText.color = COL_STRESS;
    }

    public void HideStressAlert()
    {
        stressAlertGO.SetActive(false);
    }

    public void HighlightSelectedEmotion(EmotionType type)
    {
        // Rings are updated in UpdateVisuals via pulse
    }

    public void ClearEmotionSelection()
    {
        calmSelectionRing.color = new Color(1, 1, 1, 0);
        anxietySelectionRing.color = new Color(1, 1, 1, 0);
    }

    public void ShowEmotionAssigned(EmotionType type)
    {
        if (type == EmotionType.Calm)
            statusText.text = "\u2744 Calm applied \u2014 Brain stabilising...";
        else
            statusText.text = "\u26a1 Anxiety surge \u2014 +15 Focus!";
        statusText.color = (type == EmotionType.Calm) ? COL_CALM : COL_ANXIETY;
    }

    public void ShowDateResult(string title, string detail, int[] tiers)
    {
        resultOverlay.SetActive(true);
        introOverlay.SetActive(false);
        transitionOverlay.SetActive(false);

        resultTitleText.text = title;
        resultDetailText.text = detail;

        string[] tierSymbols = { "\u2b50", "\u2705", "\u26a0", "\u274c" };
        string scoreLines = "Your responses:\n";
        string[] qShort = { "What do you do?", "Passions?", "Many dates?", "Again?" };
        for (int i = 0; i < tiers.Length; i++)
        {
            scoreLines += $"{tierSymbols[Mathf.Clamp(tiers[i], 0, 3)]}  {qShort[i]}\n";
        }
        resultScoreText.text = scoreLines;
    }

    public void ResetUI()
    {
        resultOverlay.SetActive(false);
        introOverlay.SetActive(false);
        transitionOverlay.SetActive(false);
        questionText.text = "";
        questionNumberText.text = "";
        responseText.gameObject.SetActive(false);
        reactionText.gameObject.SetActive(false);
        timerFill.fillAmount = 0f;
        stressAlertGO.SetActive(false);
        ClearEmotionSelection();
        calmCooldownOverlay.fillAmount = 0f;
        anxietyCooldownOverlay.fillAmount = 0f;
        statusText.text = "";
    }

    // ════════════════════════════════════════════════════════
    // HELPER METHODS — UI CREATION
    // ════════════════════════════════════════════════════════

    /// <summary>Creates a coloured panel (Image on a RectTransform).</summary>
    GameObject CreatePanel(Transform parent, string name, Color color,
        Vector2 anchorMin, Vector2 anchorMax,
        Vector2 offsetMin = default, Vector2 offsetMax = default)
    {
        var go = new GameObject(name);
        SetRect(go, parent, anchorMin, anchorMax, offsetMin, offsetMax);
        var img = go.AddComponent<Image>();
        img.color = color;
        img.raycastTarget = false;
        return go;
    }

    /// <summary>Creates a circle at a specific anchor point with a given radius. Returns the Transform.</summary>
    Transform CreateCircle(Transform parent, string name, Color color,
        Vector2 anchorPos, float radius)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);

        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = anchorPos;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(radius * 2f, radius * 2f);

        var img = go.AddComponent<Image>();
        img.sprite = circleSprite;
        img.color = color;
        img.raycastTarget = true;

        return go.transform;
    }

    /// <summary>Creates a Text element inside a parent.</summary>
    Text CreateText(Transform parent, string name, string content,
        int fontSize, Color color, TextAnchor alignment,
        Vector2 anchorMin, Vector2 anchorMax,
        Vector2 offsetMin = default, Vector2 offsetMax = default)
    {
        var go = new GameObject(name);
        SetRect(go, parent, anchorMin, anchorMax, offsetMin, offsetMax);
        var txt = go.AddComponent<Text>();
        txt.text = content;
        txt.fontSize = fontSize;
        txt.color = color;
        txt.alignment = alignment;
        txt.font = uiFont;
        txt.horizontalOverflow = HorizontalWrapMode.Wrap;
        txt.verticalOverflow = VerticalWrapMode.Overflow;
        txt.raycastTarget = false;
        return txt;
    }

    /// <summary>Creates a Text element that fills its parent RectTransform.</summary>
    Text CreateTextOnRect(Transform parent, string name, string content,
        int fontSize, Color color, TextAnchor alignment)
    {
        return CreateText(parent, name, content, fontSize, color, alignment,
            Vector2.zero, Vector2.one);
    }

    // CreateCircle is defined near CreatePanel above

    /// <summary>Creates a full-screen overlay panel.</summary>
    GameObject CreateFullOverlay(string name, Color bgColor)
    {
        var go = new GameObject(name);
        SetRect(go, rootCanvas.transform, Vector2.zero, Vector2.one);
        var img = go.AddComponent<Image>();
        img.color = bgColor;
        img.raycastTarget = true;  // Block clicks on elements behind
        return go;
    }

    /// <summary>Sets RectTransform anchors and offsets in one call.</summary>
    void SetRect(GameObject go, Transform parent,
        Vector2 anchorMin, Vector2 anchorMax,
        Vector2 offsetMin = default, Vector2 offsetMax = default)
    {
        go.transform.SetParent(parent, false);
        var rt = go.GetComponent<RectTransform>();
        if (rt == null) rt = go.AddComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.offsetMin = offsetMin;
        rt.offsetMax = offsetMax;
    }

    /// <summary>Overload: SetRect with a Transform parent from a circle/panel result.</summary>
    void SetRect(GameObject go, Component parent,
        Vector2 anchorMin, Vector2 anchorMax,
        Vector2 offsetMin = default, Vector2 offsetMax = default)
    {
        SetRect(go, parent.transform, anchorMin, anchorMax, offsetMin, offsetMax);
    }

    // ════════════════════════════════════════════════════════
    // HELPER METHODS — ASSETS
    // ════════════════════════════════════════════════════════

    /// <summary>Generates a circle sprite procedurally (no external assets needed).</summary>
    Sprite CreateCircleSprite(int size = 128)
    {
        var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Bilinear;

        float center = size / 2f;
        float radius = size / 2f - 2f;
        Color clear = Color.clear;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                if (dist <= radius - 1f)
                    texture.SetPixel(x, y, Color.white);
                else if (dist <= radius + 1f)
                    texture.SetPixel(x, y, new Color(1, 1, 1, Mathf.Clamp01(radius + 1f - dist) * 0.5f + 0.5f * Mathf.Clamp01(radius - dist + 1f)));
                else
                    texture.SetPixel(x, y, clear);
            }
        }

        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
    }

    /// <summary>Returns a usable font — tries built-in resources, falls back to OS.</summary>
    Font GetDefaultFont()
    {
        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (font == null) font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        if (font == null) font = Font.CreateDynamicFontFromOSFont("Arial", 14);
        if (font == null) font = Font.CreateDynamicFontFromOSFont("Helvetica", 14);
        return font;
    }
}
