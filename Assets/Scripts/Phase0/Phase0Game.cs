using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Phase 0 Vertical Slice — Main Game Controller.
///
/// Orchestrates the entire date: intro → 4 questions → result.
/// Manages Focus resource, stress event timing, dialogue outcomes,
/// and scoring. This is throwaway Phase 0 code — intentionally
/// hardcoded per the Development Plan.
/// </summary>
public class Phase0Game : MonoBehaviour
{
    // ════════════════════════════════════════════════════════
    // SINGLETON
    // ════════════════════════════════════════════════════════

    public static Phase0Game Instance { get; private set; }

    // ════════════════════════════════════════════════════════
    // GAME STATE
    // ════════════════════════════════════════════════════════

    public enum GameState { Intro, QuestionActive, ShowingResponse, BetweenQuestions, DateOver }
    public GameState State { get; private set; } = GameState.Intro;

    // ════════════════════════════════════════════════════════
    // REFERENCES
    // ════════════════════════════════════════════════════════

    public BrainNode Brain { get; set; }
    public Phase0UI UI { get; private set; }

    // ════════════════════════════════════════════════════════
    // FOCUS RESOURCE
    // ════════════════════════════════════════════════════════

    public float Focus { get; private set; } = 70f;
    public const float MAX_FOCUS = 100f;

    // ════════════════════════════════════════════════════════
    // RESPONSE TIMER
    // ════════════════════════════════════════════════════════

    public float ResponseTimer { get; private set; }
    public float ResponseTimerMax { get; private set; }

    // ════════════════════════════════════════════════════════
    // STRESS EVENT
    // ════════════════════════════════════════════════════════

    public bool StressActive { get; private set; }
    private float stressStartDelay;
    private float stressTimer;
    private const float STRESS_DURATION = 4f;

    // ════════════════════════════════════════════════════════
    // EMOTION SELECTION (click-to-target)
    // ════════════════════════════════════════════════════════

    public EmotionType SelectedEmotion { get; private set; } = EmotionType.None;

    // ════════════════════════════════════════════════════════
    // COOLDOWN
    // ════════════════════════════════════════════════════════

    public float EmotionCooldown { get; private set; } = 0f;
    public const float COOLDOWN_DURATION = 2.5f;

    // ════════════════════════════════════════════════════════
    // SCORING
    // ════════════════════════════════════════════════════════

    private int[] responseTiers;
    private int currentQuestion = -1;

    // ════════════════════════════════════════════════════════
    // DIALOGUE DATA (hardcoded for Phase 0)
    // ════════════════════════════════════════════════════════

    public struct QuestionData
    {
        public string question;
        public float timerDuration;
        public float stressDelay;   // < 0 means no stress event
        public string[] answers;    // [tier0, tier1, tier2, tier3]
        public string[] reactions;  // [good, bad]
    }

    public QuestionData[] Questions { get; private set; }

    // ════════════════════════════════════════════════════════
    // LIFECYCLE
    // ════════════════════════════════════════════════════════

    void Awake()
    {
        Instance = this;
        InitDialogueData();
        responseTiers = new int[4];
    }

    void Start()
    {
        UI = GetComponent<Phase0UI>();
        if (UI == null) UI = gameObject.AddComponent<Phase0UI>();
        UI.BuildUI();
        StartCoroutine(GameLoop());
    }

    void Update()
    {
        if (State == GameState.QuestionActive)
        {
            UpdateFocus();

            if (EmotionCooldown > 0f)
                EmotionCooldown = Mathf.Max(EmotionCooldown - Time.deltaTime, 0f);
        }

        // Let the UI refresh every frame regardless of state
        if (UI != null)
            UI.UpdateVisuals();
    }

    // ════════════════════════════════════════════════════════
    // GAME LOOP (coroutine)
    // ════════════════════════════════════════════════════════

    IEnumerator GameLoop()
    {
        // ── Intro ──────────────────────────────────────
        State = GameState.Intro;
        UI.ShowIntro();
        yield return new WaitForSeconds(4f);

        // ── Question Loop ──────────────────────────────
        for (int q = 0; q < Questions.Length; q++)
        {
            currentQuestion = q;
            State = GameState.QuestionActive;
            Focus = Mathf.Max(Focus, 35f);   // Floor: don't start a question with zero Focus

            // Set up timer & stress
            ResponseTimerMax = Questions[q].timerDuration;
            ResponseTimer = ResponseTimerMax;
            StressActive = false;
            stressTimer = 0f;
            stressStartDelay = Questions[q].stressDelay;

            UI.ShowQuestion(Questions[q].question, q + 1);

            // ── Run the question in real-time ──────────
            float elapsed = 0f;
            while (ResponseTimer > 0f)
            {
                float dt = Time.deltaTime;
                elapsed += dt;
                ResponseTimer -= dt;

                // Trigger stress event at the right moment
                if (stressStartDelay >= 0 && elapsed >= stressStartDelay && !StressActive)
                {
                    StressActive = true;
                    stressTimer = STRESS_DURATION;
                    Brain.SetStress(true);
                    UI.ShowStressAlert("\u26a0 PANIC \u2014 Brain under pressure!");
                }

                // Count down stress
                if (StressActive)
                {
                    stressTimer -= dt;
                    if (stressTimer <= 0f)
                    {
                        StressActive = false;
                        Brain.SetStress(false);
                        UI.HideStressAlert();
                    }
                }

                yield return null;
            }

            // ── Timer expired — determine outcome ──────
            StressActive = false;
            Brain.SetStress(false);
            UI.HideStressAlert();

            int tier = DetermineResponseTier();
            responseTiers[q] = tier;

            State = GameState.ShowingResponse;
            UI.ShowResponse(Questions[q].answers[tier], tier);
            yield return new WaitForSeconds(2.5f);

            string reaction = (tier <= 1) ? Questions[q].reactions[0] : Questions[q].reactions[1];
            UI.ShowReaction(reaction);
            yield return new WaitForSeconds(3f);

            // ── Between questions ──────────────────────
            if (q < Questions.Length - 1)
            {
                State = GameState.BetweenQuestions;
                Brain.PartialReset();
                UI.ShowTransition(q + 2);
                yield return new WaitForSeconds(2f);
            }
        }

        // ── Date Over ──────────────────────────────────
        State = GameState.DateOver;
        CalculateAndShowResult();
    }

    // ════════════════════════════════════════════════════════
    // RESPONSE TIER DETERMINATION
    // ════════════════════════════════════════════════════════

    int DetermineResponseTier()
    {
        float h = Brain.health;
        float f = Focus;
        EmotionType dom = Brain.DominantEmotion;

        // Tier 0 — Best: healthy Brain, good Focus, Calm dominant
        if (h > 60f && f > 50f && dom == EmotionType.Calm) return 0;

        // Tier 1 — Good: decent Brain & Focus, any emotion
        if (h > 40f && f > 30f) return 1;

        // Tier 2 — Bad: struggling but still functional
        if (h > 20f && f > 10f) return 2;

        // Tier 3 — Failure: everything has collapsed
        return 3;
    }

    // ════════════════════════════════════════════════════════
    // FOCUS MANAGEMENT
    // ════════════════════════════════════════════════════════

    void UpdateFocus()
    {
        float dt = Time.deltaTime;
        float change = -2f * dt;                       // Base drain

        if (Brain.health < 50f)
            change -= 3f * dt;                         // Extra drain when Brain is damaged

        if (Brain.health > 70f && Brain.DominantEmotion == EmotionType.Calm)
            change += 5f * dt;                         // Recovery when Brain is healthy + Calm

        Focus = Mathf.Clamp(Focus + change, 0f, MAX_FOCUS);
    }

    // ════════════════════════════════════════════════════════
    // EMOTION INPUT
    // ════════════════════════════════════════════════════════

    /// <summary>Click-to-target: select an emotion (first click).</summary>
    public void SelectEmotion(EmotionType type)
    {
        if (type != EmotionType.None && EmotionCooldown > 0f) return;
        if (type != EmotionType.None && State != GameState.QuestionActive) return;
        SelectedEmotion = type;
        UI.HighlightSelectedEmotion(type);
    }

    /// <summary>Click-to-target: assign the selected emotion (second click on Brain).</summary>
    public void TryAssignSelectedEmotion()
    {
        if (SelectedEmotion != EmotionType.None)
        {
            AssignEmotionToBrain(SelectedEmotion);
        }
    }

    /// <summary>Core assignment method — called by both input modes.</summary>
    public void AssignEmotionToBrain(EmotionType type)
    {
        if (EmotionCooldown > 0f) return;
        if (State != GameState.QuestionActive) return;
        if (type == EmotionType.None) return;

        if (type == EmotionType.Calm)
        {
            Brain.ApplyCalm();
        }
        else if (type == EmotionType.Anxiety)
        {
            Brain.ApplyAnxiety();
            Focus = Mathf.Min(Focus + 15f, MAX_FOCUS);  // Instant Focus boost
        }

        EmotionCooldown = COOLDOWN_DURATION;
        SelectedEmotion = EmotionType.None;
        UI.ClearEmotionSelection();
        UI.ShowEmotionAssigned(type);
    }

    // ════════════════════════════════════════════════════════
    // SCORING & RESULT
    // ════════════════════════════════════════════════════════

    void CalculateAndShowResult()
    {
        int totalScore = 0;
        for (int i = 0; i < responseTiers.Length; i++)
            totalScore += (3 - responseTiers[i]);   // 3 for best, 0 for worst
        // Max = 12, Min = 0

        string result, detail;

        if (totalScore >= 10)
        {
            result = "\u2728 Second Date Secured! \u2728";
            detail = "Alex texts you that evening:\n\"I had a really great time. Same place next week?\"";
        }
        else if (totalScore >= 6)
        {
            result = "\U0001f914 Maybe...";
            detail = "Alex says goodbye with a warm smile.\nYou're not sure if they'll text\u2026 but maybe?";
        }
        else if (totalScore >= 3)
        {
            result = "\U0001f62c Awkward Ending";
            detail = "Alex politely says goodnight.\nYou both know this isn't going anywhere.";
        }
        else
        {
            result = "\U0001f480 Total Disaster";
            detail = "Alex excuses themselves to the bathroom\nand never comes back.";
        }

        UI.ShowDateResult(result, detail, responseTiers);
    }

    // ════════════════════════════════════════════════════════
    // RESTART
    // ════════════════════════════════════════════════════════

    public void RestartGame()
    {
        StopAllCoroutines();

        // Reset state
        Focus = 70f;
        EmotionCooldown = 0f;
        SelectedEmotion = EmotionType.None;
        StressActive = false;
        currentQuestion = -1;
        responseTiers = new int[4];

        // Reset Brain
        Brain.health = 80f;
        Brain.calmWeight = 0f;
        Brain.anxietyWeight = 0f;
        Brain.isReceivingCalm = false;
        Brain.isReceivingAnxiety = false;
        Brain.isUnderStress = false;
        Brain.effectTimer = 0f;

        UI.ResetUI();
        StartCoroutine(GameLoop());
    }

    // ════════════════════════════════════════════════════════
    // HARDCODED DIALOGUE DATA
    // ════════════════════════════════════════════════════════

    void InitDialogueData()
    {
        Questions = new QuestionData[4];

        // ── Question 1: Easy warm-up, no stress ───────
        Questions[0] = new QuestionData
        {
            question = "\"So, what do you do for a living?\"",
            timerDuration = 10f,
            stressDelay = -1f,    // No stress event
            answers = new[]
            {
                "\"I'm a developer, actually. There's something satisfying about solving puzzles all day. What about you?\"",
                "\"I work in tech. It keeps me busy, but I enjoy it.\"",
                "\"I, uh... work? At a computer? Sorry, I'm a little nervous.\"",
                "\"I have no job. Wait\u2014I do\u2014I meant I have a job, I just can't remember what it is right now.\""
            },
            reactions = new[]
            {
                "*Alex smiles warmly*\n\"That's cool! I love hearing people talk about what they do.\"",
                "*Alex tilts their head*\n\"You okay? You seem a little distracted.\""
            }
        };

        // ── Question 2: Stress at 3 seconds ───────────
        Questions[1] = new QuestionData
        {
            question = "\"What are you passionate about?\"",
            timerDuration = 8f,
            stressDelay = 3f,
            answers = new[]
            {
                "\"Music, honestly. I could talk about it for hours. There's this feeling when a song perfectly captures something you didn't know how to say...\"",
                "\"I really like music. And cooking, sometimes.\"",
                "\"Passionate? Um... I like... things? I promise I'm more interesting than this.\"",
                "\"I have no passions. Wait, that sounds terrible\u2014I meant\u2014\""
            },
            reactions = new[]
            {
                "*Alex leans in*\n\"I can tell! Your face lights up when you talk about it.\"",
                "*Alex laughs gently*\n\"Hey, no pressure. We all go blank sometimes.\""
            }
        };

        // ── Question 3: Stress at 2 seconds ───────────
        Questions[2] = new QuestionData
        {
            question = "\"Have you been on many dates recently?\"",
            timerDuration = 8f,
            stressDelay = 2f,
            answers = new[]
            {
                "\"A few, honestly. But this one's different \u2014 I'm actually having a good time.\"",
                "\"Not too many. I've been focused on other things. This is nice though.\"",
                "\"Dates? Ha. Ha ha. Can you tell?\"",
                "\"I've been on... zero? A hundred? I genuinely cannot access that information right now.\""
            },
            reactions = new[]
            {
                "*Alex blushes slightly*\n\"Same here, honestly. This is really nice.\"",
                "*Alex smiles awkwardly*\n\"That's... refreshingly honest?\""
            }
        };

        // ── Question 4: Hardest — stress at 1 second ──
        Questions[3] = new QuestionData
        {
            question = "\"Would you want to do this again?\"",
            timerDuration = 7f,
            stressDelay = 1f,
            answers = new[]
            {
                "\"Yeah, I'd really like that. How about next weekend?\"",
                "\"I'd like that, yeah. Let's figure something out.\"",
                "\"Yes\u2014I think\u2014probably? Sorry, is it hot in here?\"",
                "\"Absolutely\u2014I mean, no\u2014I mean YES. Please forget I said no.\""
            },
            reactions = new[]
            {
                "*Alex beams*\n\"I'd love that. It's a date!\"",
                "*Alex laughs*\n\"I'll take that as a yes?\""
            }
        };
    }
}
