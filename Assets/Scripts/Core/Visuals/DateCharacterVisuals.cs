using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CtrlHeart.Core.Data;

namespace CtrlHeart.Core.Visuals
{
    /// <summary>
    /// Archetype categories for the shared reaction animations.
    /// Master Design Bible Part 5 §5.5 & Dev Plan Day 5.
    /// </summary>
    public enum DateExpressionArchetype
    {
        Neutral,
        Smiling,
        Laughing,
        Confused,
        Surprised,
        Nervous,
        Concerned,
        Awkward,
        Flirty,
        Hurt,
        Uncomfortable,
        ShutDown,
        Talking,
        Relieved,
        Bored,
        Thinking,
        Blushing,
        Sad
    }

    /// <summary>
    /// Controls the Date character visual representation and sprite animations on the top panel.
    /// Manages state transitions between Idle (RTS phase & player replies) and
    /// expressive animations (scenario prompts & reactions) with smooth returns.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class DateCharacterVisuals : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Image portraitImage;
        [SerializeField] private Image expressionOverlay;
        [SerializeField] private Text expressionBadgeText;
        [SerializeField] private Animator characterAnimator;

        [Header("Procedural Breathing (Optional if sprite idle is active)")]
        [SerializeField] private bool enableProceduralBreathing = false;
        [SerializeField] private float breathSpeed = 1.8f;
        [SerializeField] private float breathScaleMagnitude = 0.015f;

        [Header("Clip Transition Crossfade")]
        /// <remarks>
        /// fadeOut starts near the END of the current beat — gives a natural animation-ending feel.
        /// fadeIn happens at the START of the next beat — the new clip emerges smoothly.
        /// holdDuration = brief pause at minAlpha between clips (the actual clip-swap moment).
        /// </remarks>
        [SerializeField] private float crossfadeFadeOutDuration = 0.28f;
        [SerializeField] private float crossfadeFadeInDuration  = 0.35f;
        [SerializeField] [Range(0f,1f)] private float crossfadeMinAlpha = 0.0f;
        [SerializeField] private float crossfadeHoldDuration = 0.06f;

        /// <summary>
        /// Per-clip scale multipliers for sprite sheets where the artist drew the character
        /// larger or smaller than the baseline 1.0x Idle sheet (Eaya-idle_).
        /// </summary>
        private static readonly Dictionary<string, float> ClipScaleTable = new Dictionary<string, float>
        {
            // Small clips (drawn smaller by artist) -> scale UP to match Idle
            { "Date_Swirling_Drink",          1.065f },
            { "Date_Eating_Sipping",          1.065f },

            // Big clips (drawn larger / higher by artist) -> scale DOWN to match Idle
            { "Date_Talk_Teasing_Smug",       0.927f },
            { "Date_Talk_Serious_Vulnerable", 0.935f },
            { "Date_Warm_Interest",           0.954f },
            { "Date_Hair_Tuck_Shy",           0.958f },
            { "Date_Sympathetic_Concern",     0.958f },
            { "Date_Talk_Happy_Warm",         0.962f },
            { "Date_Pull_Off",                0.962f },
            { "Date_React_Flirty_Wink",       0.962f },
        };

        /// <summary>
        /// Per-clip position offsets (in canvas units) to keep the character's base grounded
        /// at the table and horizontally centered across all clip transitions.
        /// </summary>
        private static readonly Dictionary<string, Vector2> ClipOffsetTable = new Dictionary<string, Vector2>
        {
            // Small clips: upward offset cancels downward shift caused by top-pivot (0.5, 1.0) scaling
            { "Date_Swirling_Drink",          new Vector2( 6.9f,  40.6f) },
            { "Date_Eating_Sipping",          new Vector2( 3.8f,  40.8f) },

            // Big clips: downward offset keeps base seated firmly at table level
            { "Date_Talk_Teasing_Smug",       new Vector2(-4.2f, -44.8f) },
            { "Date_Talk_Serious_Vulnerable", new Vector2(-1.7f, -41.8f) },
            { "Date_Warm_Interest",           new Vector2(-3.4f, -27.6f) },
            { "Date_Hair_Tuck_Shy",           new Vector2(-2.1f, -24.8f) },
            { "Date_Sympathetic_Concern",     new Vector2(-6.4f, -24.8f) },
            { "Date_Talk_Happy_Warm",         new Vector2( 2.0f, -24.7f) },
            { "Date_Pull_Off",                new Vector2( 2.0f, -24.6f) },
            { "Date_React_Flirty_Wink",       new Vector2( 0.7f, -21.7f) },

            // Leaning in: smooth horizontal compensation so she leans naturally without snapping
            { "Date_Leaning_In_Table",        new Vector2(15.0f,  -6.2f) },
        };

        private RectTransform portraitRect;
        private Vector3 initialScale = Vector3.one;
        private Vector2 initialAnchoredPos = Vector2.zero;
        private float currentClipScale = 1f;  // set by ApplyClipScale; breathing multiplies on top
        private DateExpressionArchetype currentArchetype = DateExpressionArchetype.Neutral;
        private Coroutine activeReturnToIdleCoroutine;
        private Coroutine activeCrossfadeCoroutine;

        // Current active state name
        private string currentPlayingClip = "Idle";

        private void Awake()
        {
            if (portraitImage == null)
                portraitImage = GetComponent<Image>();

            if (portraitImage != null)
            {
                portraitImage.preserveAspect = true;
                portraitRect = portraitImage.GetComponent<RectTransform>();
                initialScale = portraitRect != null ? portraitRect.localScale : Vector3.one;
                initialAnchoredPos = portraitRect != null ? portraitRect.anchoredPosition : Vector2.zero;
            }

            if (characterAnimator == null)
                characterAnimator = GetComponent<Animator>();

            if (expressionBadgeText == null && transform.parent != null)
            {
                var badge = transform.parent.Find("ExpressionBadge");
                if (badge != null) expressionBadgeText = badge.GetComponent<Text>();
            }
        }

        private void Start()
        {
            PlayIdle();
        }

        private void Update()
        {
            if (enableProceduralBreathing && portraitRect != null)
            {
                float breath = Mathf.Sin(Time.time * breathSpeed) * breathScaleMagnitude;
                float s = currentClipScale;
                portraitRect.localScale = new Vector3(
                    initialScale.x * s + breath,
                    initialScale.y * s + breath,
                    initialScale.z
                );
            }
        }

        // ═══════════════════════════════════════════════════════════════════
        // CORE ANIMATION CONTROL API
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Plays the default resting idle animation (Date_Idle / Eaya-idle_).
        /// </summary>
        public void PlayIdle()
        {
            currentPlayingClip = "Date_Idle";
            currentArchetype = DateExpressionArchetype.Neutral;
            CrossfadeTrigger("PlayIdle");
            UpdateBadgeDisplay("Resting / Listening", DateExpressionArchetype.Neutral);
        }

        /// <summary>
        /// Plays a sequence of animation beats (clamping non-verbal actions with talking animations).
        /// </summary>
        public void PlaySequence(List<(string clip, float duration)> sequence, Action onComplete = null)
        {
            if (activeReturnToIdleCoroutine != null)
            {
                StopCoroutine(activeReturnToIdleCoroutine);
                activeReturnToIdleCoroutine = null;
            }

            activeReturnToIdleCoroutine = StartCoroutine(SequenceRoutine(sequence, onComplete));
        }

        private IEnumerator SequenceRoutine(List<(string clip, float duration)> sequence, Action onComplete)
        {
            // Ensure we start at full opacity
            SetAlpha(1f);

            for (int i = 0; i < sequence.Count; i++)
            {
                var beat = sequence[i];
                currentPlayingClip = beat.clip;
                currentArchetype = MapClipTagToArchetype(beat.clip);

                // Apply per-clip scale correction (e.g. drinking sprites are 6.2% smaller)
                ApplyClipScale(beat.clip);

                // Fire the animator trigger (we are already at minAlpha from previous fade-out,
                // or at 1.0 on the very first beat)
                FireAnimTrigger(beat.clip);

                UpdateBadgeDisplay(FormatClipDisplayName(beat.clip), currentArchetype);
                Debug.Log($"<color=#FF99BB>[DateVisuals] Beat [{i + 1}/{sequence.Count}]: {beat.clip} ({beat.duration:F1}s)</color>");

                // --- Fade IN slowly at the start of this beat ---
                yield return FadeToAlpha(1f, crossfadeFadeInDuration);

                // --- Hold at full opacity for the main body of the beat ---
                float holdTime = beat.duration - crossfadeFadeInDuration - crossfadeFadeOutDuration - crossfadeHoldDuration;
                if (holdTime > 0f)
                    yield return new WaitForSeconds(holdTime);

                // --- Fade OUT near the end of the beat ---
                yield return FadeToAlpha(crossfadeMinAlpha, crossfadeFadeOutDuration);

                // --- Brief hold at minAlpha: the clip-swap moment ---
                if (crossfadeHoldDuration > 0f)
                    yield return new WaitForSeconds(crossfadeHoldDuration);

                // Loop continues: next iteration fires the new trigger while still at minAlpha
            }

            // Restore scale to normal before returning to idle
            ApplyClipScale("Date_Idle");

            activeReturnToIdleCoroutine = null;
            if (onComplete != null)
            {
                onComplete();
            }
            else
            {
                // Fire idle trigger while still at minAlpha, then fade in
                currentPlayingClip = "Date_Idle";
                currentArchetype = DateExpressionArchetype.Neutral;
                FireAnimTrigger("PlayIdle");
                UpdateBadgeDisplay("Resting / Listening", DateExpressionArchetype.Neutral);
                yield return FadeToAlpha(1f, crossfadeFadeInDuration);
            }
        }

        // ═══════════════════════════════════════════════════════════════════
        // CROSSFADE HELPERS
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Used by PlayIdle() and PlayTalking() (outside of sequences).
        /// Dips to minAlpha, swaps clip, then fades back in.
        /// </summary>
        private void CrossfadeTrigger(string trigger)
        {
            if (activeCrossfadeCoroutine != null)
                StopCoroutine(activeCrossfadeCoroutine);
            activeCrossfadeCoroutine = StartCoroutine(CrossfadeSwitchRoutine(trigger));
        }

        private IEnumerator CrossfadeSwitchRoutine(string trigger)
        {
            yield return FadeToAlpha(crossfadeMinAlpha, crossfadeFadeOutDuration);
            ApplyClipScale(trigger);
            FireAnimTrigger(trigger);
            if (crossfadeHoldDuration > 0f)
                yield return new WaitForSeconds(crossfadeHoldDuration);
            yield return FadeToAlpha(1f, crossfadeFadeInDuration);
            activeCrossfadeCoroutine = null;
        }

        /// <summary>Lerps portraitImage alpha from current to target over duration seconds.</summary>
        private IEnumerator FadeToAlpha(float target, float duration)
        {
            if (portraitImage == null || duration <= 0f)
            {
                if (portraitImage != null) SetAlpha(target);
                yield break;
            }
            float elapsed = 0f;
            Color c = portraitImage.color;
            float start = c.a;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                c.a = Mathf.Lerp(start, target, elapsed / duration);
                portraitImage.color = c;
                yield return null;
            }
            c.a = target;
            portraitImage.color = c;
        }

        private void SetAlpha(float a)
        {
            if (portraitImage == null) return;
            Color c = portraitImage.color;
            c.a = a;
            portraitImage.color = c;
        }

        /// <summary>
        /// Applies localScale and anchoredPosition adjustments to portraitRect for clips where
        /// the character was drawn at a different scale or position than the baseline Idle sheet.
        /// Resets to initialScale and initialAnchoredPos for baseline clips.
        /// </summary>
        private void ApplyClipScale(string clipName)
        {
            if (portraitRect == null) return;
            if (clipName == "PlayIdle") clipName = "Date_Idle";

            float s = ClipScaleTable.TryGetValue(clipName, out float v) ? v : 1f;
            Vector2 offset = ClipOffsetTable.TryGetValue(clipName, out Vector2 off) ? off : Vector2.zero;

            currentClipScale = s;
            portraitRect.localScale = new Vector3(
                initialScale.x * s,
                initialScale.y * s,
                initialScale.z
            );
            portraitRect.anchoredPosition = initialAnchoredPos + offset;
        }

        /// <summary>Fires an Animator trigger safely, falling back to PlayIdle on failure.</summary>
        private void FireAnimTrigger(string trigger)
        {
            if (characterAnimator == null || characterAnimator.runtimeAnimatorController == null) return;
            try
            {
                characterAnimator.ResetTrigger(trigger);
                characterAnimator.SetTrigger(trigger);
            }
            catch
            {
                characterAnimator.SetTrigger("PlayIdle");
            }
        }

        /// <summary>
        /// Plays talking animation while dialogue is active.
        /// </summary>
        public void PlayTalking(string talkType = "casual")
        {
            string trigger = talkType.ToLowerInvariant() switch
            {
                "happy" or "warm" => "Date_Talk_Happy_Warm",
                "teasing" or "smug" => "Date_Talk_Teasing_Smug",
                "serious" or "vulnerable" => "Date_Talk_Serious_Vulnerable",
                "hesitant" or "uncertain" => "Date_Talk_Hesitant",
                "sharp" or "confrontational" => "Date_Talk_Sharp_Confrontational",
                _ => "Date_Talk_Casual"
            };

            currentPlayingClip = trigger;
            currentArchetype = DateExpressionArchetype.Talking;
            CrossfadeTrigger(trigger);
            UpdateBadgeDisplay(FormatClipDisplayName(trigger), DateExpressionArchetype.Talking);
        }

        /// <summary>
        /// Plays the scenario starting animation sequence when a new slot begins.
        /// Implements the exact 40 scenario beats and clamped sequences from user spec.
        /// </summary>
        public void PlayScenarioIntro(int slotIndex, ConnectionTier tier, string promptText)
        {
            var sequence = MapScenarioToAnimationSequence(slotIndex, tier);
            PlaySequence(sequence, null);
        }

        /// <summary>
        /// Plays reaction in response to player reply.
        /// Implements Group A-R classification based on delta and text, with talking clamping if spoken.
        /// </summary>
        public void PlayReaction(string animationClipTag, float delta = 0f, string reactionText = "")
        {
            var (reactionClip, talkingClip) = ResolveReactionGroup(animationClipTag, delta, reactionText);
            var sequence = new List<(string clip, float duration)>();

            bool hasSpokenDialogue = !string.IsNullOrEmpty(reactionText) && (reactionText.Contains("“") || reactionText.Contains("\""));

            if (hasSpokenDialogue && !string.IsNullOrEmpty(talkingClip))
            {
                // Clamping rule: Brief non-verbal reaction (1.0s) -> Talking clip (2.0s) -> Idle
                sequence.Add((reactionClip, 1.0f));
                sequence.Add((talkingClip, 2.0f));
            }
            else
            {
                // Wordless reaction beat (2.5s) -> Idle
                sequence.Add((reactionClip, 2.5f));
            }

            PlaySequence(sequence);
        }

        public void PlayReaction(string animationClipTag)
        {
            PlayReaction(animationClipTag, 0f, "");
        }

        public void SetExpressionFromTag(string animationClipTag)
        {
            PlayReaction(animationClipTag, 0f, "");
        }

        private void UpdateBadgeDisplay(string displayName, DateExpressionArchetype archetype)
        {
            if (expressionBadgeText != null)
            {
                expressionBadgeText.text = $"[Date: {displayName}]";
                expressionBadgeText.color = GetArchetypeColor(archetype);
            }
        }

        // ═══════════════════════════════════════════════════════════════════
        // 40 SCENARIO PROMPT ASSIGNMENTS (USER SPEC §1)
        // ═══════════════════════════════════════════════════════════════════

        public static List<(string clip, float duration)> MapScenarioToAnimationSequence(int slotIndex, ConnectionTier tier)
        {
            var seq = new List<(string clip, float duration)>();

            switch (slotIndex)
            {
                // Slot 1: Arrival / First Exchange
                case 1:
                    switch (tier)
                    {
                        case ConnectionTier.Tier4_SecondDate: // She compliments him -> shy hair tuck clamped into warm talk
                            seq.Add(("Date_Hair_Tuck_Shy", 1.2f));
                            seq.Add(("Date_Talk_Happy_Warm", 2.2f));
                            break;
                        case ConnectionTier.Tier3_Strong: // She asks about his day
                            seq.Add(("Date_Talk_Casual", 2.5f));
                            break;
                        case ConnectionTier.Tier2_Maybe: // Comfortable pause (wordless)
                            seq.Add(("Date_Swirling_Drink", 2.5f));
                            break;
                        default: // She notices he is tense -> clamped concern to hesitant question
                            seq.Add(("Date_Sympathetic_Concern", 1.2f));
                            seq.Add(("Date_Talk_Hesitant", 2.5f));
                            break;
                    }
                    break;

                // Slot 2: Small Talk Deepens
                case 2:
                    switch (tier)
                    {
                        case ConnectionTier.Tier4_SecondDate: // She teases about useless talent
                            seq.Add(("Date_Talk_Teasing_Smug", 2.5f));
                            break;
                        case ConnectionTier.Tier3_Strong: // Small physical mishap -> glass spill catch clamped to nervous laugh
                            seq.Add(("Date_Glass_Spill_Catch", 1.4f));
                            seq.Add(("Date_Nervous_Laugh", 2.0f));
                            break;
                        case ConnectionTier.Tier2_Maybe: // She asks what he does
                            seq.Add(("Date_Talk_Casual", 2.5f));
                            break;
                        default: // Avoids eye contact for a beat (wordless)
                            seq.Add(("Date_Awkward_Silence", 2.5f));
                            break;
                    }
                    break;

                // Slot 3: Real Curiosity vs Distance
                case 3:
                    switch (tier)
                    {
                        case ConnectionTier.Tier4_SecondDate: // She admits he is easy to talk to
                            seq.Add(("Date_Talk_Serious_Vulnerable", 2.5f));
                            break;
                        case ConnectionTier.Tier3_Strong: // Turns glass, smiling to herself (wordless)
                            seq.Add(("Date_Swirling_Drink", 2.5f));
                            break;
                        case ConnectionTier.Tier2_Maybe: // Stifles a small yawn, catches herself (wordless)
                            seq.Add(("Date_Yawn_Stifle_Embarrassed", 2.5f));
                            break;
                        default: // Calls out self-checking -> skeptical brow raise clamped to hesitant question
                            seq.Add(("Date_React_Skeptical_Brow_Raise", 1.2f));
                            seq.Add(("Date_Talk_Hesitant", 2.5f));
                            break;
                    }
                    break;

                // Slot 4: Environment Intrudes
                case 4:
                    switch (tier)
                    {
                        case ConnectionTier.Tier4_SecondDate: // Nearly knocks glass -> glass spill catch clamped to genuine hearty laugh
                            seq.Add(("Date_Glass_Spill_Catch", 1.2f));
                            seq.Add(("Date_Laugh_Hearty", 2.2f));
                            break;
                        case ConnectionTier.Tier3_Strong: // Looks at him warmly (wordless)
                            seq.Add(("Date_Warm_Interest", 2.5f));
                            break;
                        case ConnectionTier.Tier2_Maybe: // Phone buzzes; puts it face-down (wordless)
                            seq.Add(("Date_Phone_Buzz_Facedown", 2.5f));
                            break;
                        default: // Checks time, restless (wordless)
                            seq.Add(("Date_Checking_Time_Restless", 2.5f));
                            break;
                    }
                    break;

                // Slot 5: Midpoint - What Are You Looking For
                case 5:
                    switch (tier)
                    {
                        case ConnectionTier.Tier4_SecondDate: // Asks directly, earnestly
                            seq.Add(("Date_Talk_Serious_Vulnerable", 2.5f));
                            break;
                        case ConnectionTier.Tier3_Strong: // Asks, hedging
                            seq.Add(("Date_Talk_Hesitant", 2.5f));
                            break;
                        case ConnectionTier.Tier2_Maybe: // Hesitates before asking own question
                            seq.Add(("Date_Talk_Hesitant", 2.5f));
                            break;
                        default: // Asks if he'd rather be somewhere else
                            seq.Add(("Date_Talk_Sharp_Confrontational", 2.5f));
                            break;
                    }
                    break;

                // Slot 6: Vulnerability or Distance
                case 6:
                    switch (tier)
                    {
                        case ConnectionTier.Tier4_SecondDate: // Admits she nearly cancelled (gentle vulnerable talk)
                            seq.Add(("Date_Talk_Gentle_Sad", 2.5f));
                            break;
                        case ConnectionTier.Tier3_Strong: // Shared physical mishap -> player catches glass clamped to relieved sigh
                            seq.Add(("Date_Player_Catches_Glass", 1.4f));
                            seq.Add(("Date_React_Relieved_Sigh", 2.0f));
                            break;
                        case ConnectionTier.Tier2_Maybe: // Comfortable and quiet (deliberate zero-event breather)
                            seq.Add(("Date_Eating_Sipping", 2.5f));
                            break;
                        default: // Asks if this is going okay
                            seq.Add(("Date_Talk_Sharp_Confrontational", 2.5f));
                            break;
                    }
                    break;

                // Slot 7: Playfulness or Withdrawal
                case 7:
                    switch (tier)
                    {
                        case ConnectionTier.Tier4_SecondDate: // Leans in across table clamped to teasing smug talk
                            seq.Add(("Date_Leaning_In_Table", 1.4f));
                            seq.Add(("Date_Talk_Teasing_Smug", 2.5f));
                            break;
                        case ConnectionTier.Tier3_Strong: // Playful challenge
                            seq.Add(("Date_Talk_Teasing_Smug", 2.5f));
                            break;
                        case ConnectionTier.Tier2_Maybe: // Attention drifts briefly (wordless)
                            seq.Add(("Date_React_Boredom_Eyeroll", 2.5f));
                            break;
                        default: // Checks time again, more obviously (wordless)
                            seq.Add(("Date_Checking_Time_Restless", 2.5f));
                            break;
                    }
                    break;

                // Slot 8: "You Seem Nervous"
                case 8:
                    switch (tier)
                    {
                        case ConnectionTier.Tier4_SecondDate: // Calls it adorable -> blush clamped to shy hair tuck
                            seq.Add(("Date_React_Deep_Blush", 1.2f));
                            seq.Add(("Date_Hair_Tuck_Shy", 1.8f));
                            seq.Add(("Date_Talk_Happy_Warm", 2.0f));
                            break;
                        case ConnectionTier.Tier3_Strong: // Frames it as good nervous
                            seq.Add(("Date_Talk_Happy_Warm", 2.5f));
                            break;
                        case ConnectionTier.Tier2_Maybe: // Wonders whether it's her
                            seq.Add(("Date_Talk_Hesitant", 2.5f));
                            break;
                        default: // Questions whether it's landing
                            seq.Add(("Date_Talk_Sharp_Confrontational", 2.5f));
                            break;
                    }
                    break;

                // Slot 9: Insecurity / Stakes
                case 9:
                    switch (tier)
                    {
                        case ConnectionTier.Tier4_SecondDate: // Asks what he's insecure about -> gentle sad/vulnerable talk
                            seq.Add(("Date_Talk_Gentle_Sad", 2.5f));
                            break;
                        case ConnectionTier.Tier3_Strong: // Asks, knowing it may be too much
                            seq.Add(("Date_Talk_Hesitant", 2.5f));
                            break;
                        case ConnectionTier.Tier2_Maybe: // Starts, then stops herself (half-verbal -> thinking chintap)
                            seq.Add(("Date_Talk_Hesitant", 1.0f));
                            seq.Add(("Date_React_Thinking_Chintap", 2.0f));
                            break;
                        default: // Says she's asking questions into a wall
                            seq.Add(("Date_Talk_Sharp_Confrontational", 2.5f));
                            break;
                    }
                    break;

                // Slot 10: Final Convergence
                case 10:
                    switch (tier)
                    {
                        case ConnectionTier.Tier4_SecondDate: // Asks plainly
                            seq.Add(("Date_Talk_Casual", 2.5f));
                            break;
                        case ConnectionTier.Tier3_Strong: // Gives him an out
                            seq.Add(("Date_Talk_Hesitant", 2.5f));
                            break;
                        case ConnectionTier.Tier2_Maybe: // Unsure how he feels
                            seq.Add(("Date_Talk_Hesitant", 2.5f));
                            break;
                        default: // Braces for answer -> expecting clamped to sharp confrontation
                            seq.Add(("Date_Expecting", 1.2f));
                            seq.Add(("Date_Talk_Sharp_Confrontational", 2.5f));
                            break;
                    }
                    break;

                default:
                    seq.Add(("Date_Expecting", 2.5f));
                    break;
            }

            return seq;
        }

        // ═══════════════════════════════════════════════════════════════════
        // 400 REACTION ASSIGNMENTS (USER SPEC §2: GROUPS A TO R)
        // ═══════════════════════════════════════════════════════════════════

        public static (string reactionClip, string talkingClip) ResolveReactionGroup(
            string tag, float delta, string reactionText)
        {
            string t = (reactionText ?? "").ToLowerInvariant();
            string tagLower = (tag ?? "").ToLowerInvariant();

            // Group R: Slot 10 Bracing specifically (Slot 10 T1 failure)
            if (t.Contains("bracing") || t.Contains("ending") || tagLower.Contains("ending_awkward"))
            {
                return ("Date_Ending_Awkward", "Date_Talk_Sharp_Confrontational");
            }

            // Group O: Startled / Caught off guard / mishap-adjacent
            if (t.Contains("startled") || t.Contains("surprised") || t.Contains("wobble") || t.Contains("catches it") || tagLower.Contains("surprised"))
            {
                return ("Date_Surprised", "Date_Talk_Hesitant");
            }

            // Shy hair tuck
            if (t.Contains("tuck") || t.Contains("behind her ear") || tagLower.Contains("hair_tuck"))
            {
                return ("Date_Hair_Tuck_Shy", "Date_Talk_Happy_Warm");
            }

            // Stifled yawn
            if (t.Contains("yawn") || tagLower.Contains("yawn"))
            {
                return ("Date_Yawn_Stifle_Embarrassed", "Date_Talk_Hesitant");
            }

            // Hearty genuine laughter
            if (t.Contains("hearty") || t.Contains("laugh delightedly") || (delta >= 14f && t.Contains("laugh")))
            {
                return ("Date_Laugh_Hearty", "Date_Talk_Happy_Warm");
            }

            // Group B variant: Physical blushing mentioned in line
            if (t.Contains("blush") || t.Contains("flushed") || t.Contains("red in the cheeks") || tagLower.Contains("blush"))
            {
                return ("Date_React_Deep_Blush", "Date_Talk_Happy_Warm");
            }

            // Group C: Playful positive / Confidence+Attraction teasing / flirty wink
            if (t.Contains("wink") || t.Contains("flirt") || t.Contains("smirk") || t.Contains("teas") || tagLower.Contains("flirty") || tagLower.Contains("wink"))
            {
                return ("Date_React_Flirty_Wink", "Date_Talk_Teasing_Smug");
            }

            // Group D: Mild positive / relief / tension easing
            if (t.Contains("sigh") || t.Contains("relie") || t.Contains("tension ease") || t.Contains("relax") || tagLower.Contains("relief") || tagLower.Contains("sigh"))
            {
                return ("Date_React_Relieved_Sigh", "Date_Talk_Casual");
            }

            // Group I: Skeptical / doubtful / questioning what he said
            if (t.Contains("eyebrow") || t.Contains("brow") || t.Contains("skeptic") || t.Contains("doubt") || tagLower.Contains("skeptical"))
            {
                return ("Date_React_Skeptical_Brow_Raise", "Date_Talk_Hesitant");
            }

            // Group J: Boredom / checked out / over-talking
            if (t.Contains("eyeroll") || t.Contains("roll her eyes") || t.Contains("bored") || t.Contains("ramble") || tagLower.Contains("boredom"))
            {
                return ("Date_React_Boredom_Eyeroll", "Date_Talk_Sharp_Confrontational");
            }

            // Group K: Genuine hurt / stung by dismissive remark
            if (t.Contains("hurt") || t.Contains("stung") || t.Contains("wince") || t.Contains("flinch") || tagLower.Contains("hurt") || tagLower.Contains("stung"))
            {
                return ("Date_Hurt_Stung", "Date_Talk_Sharp_Confrontational");
            }

            // Group L: Disappointment / gentle sad lines
            if (t.Contains("sad") || t.Contains("disappoint") || t.Contains("drops a notch") || tagLower.Contains("sad") || tagLower.Contains("disappointment") || t.Contains("gentle"))
            {
                return ("Date_React_Sad_Disappointment", "Date_Talk_Gentle_Sad");
            }

            // Group M: Pulling away / emotional retreat
            if (t.Contains("pull back") || t.Contains("pulls away") || t.Contains("barrier") || t.Contains("distance") || tagLower.Contains("pull_off"))
            {
                return ("Date_Pull_Off", "Date_Talk_Sharp_Confrontational");
            }

            // Group P: Confused / doesn't understand answer
            if (t.Contains("confused") || t.Contains("doesn’t understand") || t.Contains("puzzled") || tagLower.Contains("confused"))
            {
                return ("Date_Confused", "Date_Talk_Hesitant");
            }

            // Group Q: Nervous laugh (his) mirrored / charmed despite awkwardness
            if (t.Contains("nervous laugh") || t.Contains("chuckle") || t.Contains("laughs lightly") || t.Contains("awkward grin") || tagLower.Contains("laugh") || tagLower.Contains("awkward_grin"))
            {
                return ("Date_Nervous_Laugh", "Date_Talk_Happy_Warm");
            }

            // Group F: Considering / unsure / thoughtful chin tap
            if (t.Contains("think") || t.Contains("ponder") || t.Contains("chin") || t.Contains("weigh") || tagLower.Contains("thinking") || tagLower.Contains("chintap"))
            {
                return ("Date_React_Thinking_Chintap", "Date_Talk_Hesitant");
            }

            // Group N: Full shutdown (worst end lines, typically delta <= -15)
            if (delta <= -15f || tagLower.Contains("shut_down") || t.Contains("shutdown") || t.Contains("cold"))
            {
                return ("Date_Shut_Down", "Date_Talk_Sharp_Confrontational");
            }

            // Group H: Visible awkwardness / silence
            if (delta <= -10f || tagLower.Contains("awkward_silence") || tagLower.Contains("uncomfortable") || t.Contains("awkward") || t.Contains("silence"))
            {
                bool isWordless = !t.Contains("“") && !t.Contains("\"");
                string clip = isWordless ? "Date_Awkward_Silence" : "Date_Uncomfortable";
                return (clip, "Date_Talk_Hesitant");
            }

            // Group G: Mild discomfort / guarded
            if (delta < 0f || tagLower.Contains("discomfort") || t.Contains("guard") || t.Contains("hesitat"))
            {
                return ("Date_Discomfort", "Date_Talk_Hesitant");
            }

            // Group A: Best-possible (+15 to +20, usually Calm+Confidence)
            if (delta >= 15f || tagLower.Contains("sparkling_eyes") || tagLower.Contains("2nd_date"))
            {
                return ("Date_Second_Date_Warmth", "Date_Talk_Happy_Warm");
            }

            // Group B: Strong positive (+10 to +14, often Calm+Attraction)
            if (delta >= 10f || tagLower.Contains("warm_interest") || tagLower.Contains("warm_smile") || tagLower.Contains("engaged_smile"))
            {
                return ("Date_Warm_Interest", "Date_Talk_Happy_Warm");
            }

            // Group E: Flat / neutral (Reasonable band low end, non-committal)
            return ("Date_Neutral_Acknowledge", "Date_Talk_Casual");
        }

        public static string MapScenarioToAnimationTrigger(int slotIndex, ConnectionTier tier, string promptText)
        {
            var seq = MapScenarioToAnimationSequence(slotIndex, tier);
            return seq.Count > 0 ? seq[0].clip : "Date_Idle";
        }

        public static string MapReactionTagToAnimationTrigger(string tag)
        {
            var (reactionClip, _) = ResolveReactionGroup(tag, 0f, "");
            return reactionClip;
        }

        public static DateExpressionArchetype MapClipTagToArchetype(string tag)
        {
            if (string.IsNullOrEmpty(tag)) return DateExpressionArchetype.Neutral;
            string t = tag.ToLowerInvariant();

            if (t.Contains("wink") || t.Contains("flirt") || t.Contains("second_date") || t.Contains("2nd_date")) return DateExpressionArchetype.Flirty;
            if (t.Contains("blush") || t.Contains("tuck") || t.Contains("shy")) return DateExpressionArchetype.Blushing;
            if (t.Contains("hearty") || t.Contains("laugh") || t.Contains("chuckle")) return DateExpressionArchetype.Laughing;
            if (t.Contains("warm") || t.Contains("smile") || t.Contains("engaged")) return DateExpressionArchetype.Smiling;
            if (t.Contains("surprised") || t.Contains("startled")) return DateExpressionArchetype.Surprised;
            if (t.Contains("confused")) return DateExpressionArchetype.Confused;
            if (t.Contains("nervous")) return DateExpressionArchetype.Nervous;
            if (t.Contains("sympathetic") || t.Contains("concerned") || t.Contains("worried")) return DateExpressionArchetype.Concerned;
            if (t.Contains("hurt") || t.Contains("stung")) return DateExpressionArchetype.Hurt;
            if (t.Contains("uncomfortable") || t.Contains("discomfort")) return DateExpressionArchetype.Uncomfortable;
            if (t.Contains("shut_down") || t.Contains("ending_awkward")) return DateExpressionArchetype.ShutDown;
            if (t.Contains("awkward")) return DateExpressionArchetype.Awkward;
            if (t.Contains("sigh") || t.Contains("relieved")) return DateExpressionArchetype.Relieved;
            if (t.Contains("boredom") || t.Contains("eyeroll") || t.Contains("yawn")) return DateExpressionArchetype.Bored;
            if (t.Contains("thinking") || t.Contains("chintap")) return DateExpressionArchetype.Thinking;
            if (t.Contains("gentle_sad") || t.Contains("sad") || t.Contains("disappoint")) return DateExpressionArchetype.Sad;
            if (t.Contains("talk")) return DateExpressionArchetype.Talking;

            return DateExpressionArchetype.Neutral;
        }

        private static string FormatClipDisplayName(string triggerName)
        {
            return triggerName
                .Replace("Date_Talk_", "TALKING: ")
                .Replace("Date_React_", "REACTION: ")
                .Replace("Date_", "")
                .Replace("_", " ")
                .ToUpperInvariant();
        }

        private static Color GetArchetypeColor(DateExpressionArchetype archetype)
        {
            return archetype switch
            {
                DateExpressionArchetype.Smiling => new Color(0.35f, 0.85f, 0.45f, 1f),
                DateExpressionArchetype.Laughing => new Color(0.95f, 0.8f, 0.25f, 1f),
                DateExpressionArchetype.Flirty => new Color(1.0f, 0.4f, 0.7f, 1f),
                DateExpressionArchetype.Blushing => new Color(1.0f, 0.5f, 0.6f, 1f),
                DateExpressionArchetype.Nervous => new Color(1f, 0.65f, 0.25f, 1f),
                DateExpressionArchetype.Concerned => new Color(0.4f, 0.75f, 1f, 1f),
                DateExpressionArchetype.Awkward => new Color(0.9f, 0.5f, 0.35f, 1f),
                DateExpressionArchetype.Surprised => new Color(0.95f, 0.9f, 0.3f, 1f),
                DateExpressionArchetype.Confused => new Color(0.75f, 0.55f, 0.9f, 1f),
                DateExpressionArchetype.Hurt => new Color(0.95f, 0.35f, 0.35f, 1f),
                DateExpressionArchetype.Uncomfortable => new Color(0.85f, 0.45f, 0.45f, 1f),
                DateExpressionArchetype.ShutDown => new Color(0.6f, 0.6f, 0.65f, 1f),
                DateExpressionArchetype.Relieved => new Color(0.45f, 0.85f, 0.75f, 1f),
                DateExpressionArchetype.Bored => new Color(0.7f, 0.65f, 0.6f, 1f),
                DateExpressionArchetype.Thinking => new Color(0.85f, 0.75f, 0.4f, 1f),
                DateExpressionArchetype.Sad => new Color(0.6f, 0.7f, 0.85f, 1f),
                DateExpressionArchetype.Talking => new Color(0.95f, 0.85f, 0.45f, 1f),
                _ => new Color(0.75f, 0.8f, 0.85f, 1f)
            };
        }
    }
}
