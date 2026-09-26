using System.Collections.Generic;
using UnityEngine;
using CtrlHeart.Core.Data;
using CtrlHeart.Core.Data.ScriptableObjects;

namespace CtrlHeart.Core.Testing
{
    /// <summary>
    /// Content repository for Slots 1, 2, and 3 authored per Master Design Bible Part 5.
    /// Meets Day 3 Checkpoint: Slots 1-3 fully playable with continuous state carry-forward.
    /// </summary>
    public static class Day3Content
    {
        public static (List<QuestionData> questions, List<AnswerData> answers, List<ReactionData> reactions) CreateSlots1To3Content()
        {
            var questions = new List<QuestionData>();
            var answers = new List<AnswerData>();
            var reactions = new List<ReactionData>();

            // ════════════════════════════════════════════════════════
            // SLOT 1: ARRIVAL / FIRST IMPRESSIONS
            // ════════════════════════════════════════════════════════
            var (s1Q, s1A, s1R) = Day2TestHarness.CreateSlot1Content();
            questions.AddRange(s1Q);
            answers.AddRange(s1A);
            reactions.AddRange(s1R);

            // ════════════════════════════════════════════════════════
            // SLOT 2: ORDERING & DRINKS (Small Talk)
            // Primary Target: Voice & Lungs | Stress Event: AwkwardSilence
            // ════════════════════════════════════════════════════════
            var q2Tier2 = ScriptableObject.CreateInstance<QuestionData>();
            q2Tier2.slotIndex = 2;
            q2Tier2.connectionTier = ConnectionTier.Tier2_Maybe;
            q2Tier2.questionText = "What kind of coffee or tea are you in the mood for today?";
            q2Tier2.responseTimeWindow = 6f;
            q2Tier2.socialEventProfile = new SocialEventProfile
            {
                targetNodes = new List<InternalNodeType> { InternalNodeType.Voice, InternalNodeType.Lungs },
                effectType = SocialEffectType.StressEvent,
                stressEventSubtype = StressEventSubtype.AwkwardSilence,
                magnitude = 18f
            };
            questions.Add(q2Tier2);

            answers.Add(CreateAnswer(q2Tier2, EmotionState.Calm, "Probably just an oat flat white. Keep it simple and smooth.", +4f));
            answers.Add(CreateAnswer(q2Tier2, EmotionState.Anxiety, "Oh gosh, menus with too many syrups stress me out! Whatever you're having?", -1f));
            answers.Add(CreateAnswer(q2Tier2, EmotionState.Confidence, "Double espresso, straight up. I like bold flavors.", +5f));
            answers.Add(CreateAnswer(q2Tier2, EmotionState.Attraction, "Something sweet. Maybe caramel? You have that vibe of knowing great drinks.", +7f));
            answers.Add(CreateAnswer(q2Tier2, EmotionState.Mixture_1, "I usually get black coffee, but feeling like trying something gentler today.", +3f));
            answers.Add(CreateAnswer(q2Tier2, EmotionState.Mixture_2, "A pour-over Ethiopian roast if they have it. It has nice floral notes.", +6f));
            answers.Add(CreateAnswer(q2Tier2, EmotionState.Mixture_3, "A spiced chai latte sounds incredible right now, especially sharing a table.", +8f));
            answers.Add(CreateAnswer(q2Tier2, EmotionState.Mixture_4, "I panicked last week and ordered decaf iced water, so anything normal works!", +4f));
            answers.Add(CreateAnswer(q2Tier2, EmotionState.Mixture_5, "I want whatever looks the prettiest on the counter, honestly.", +6f));
            answers.Add(CreateAnswer(q2Tier2, EmotionState.Mixture_6, "Let me get both of us their specialty brew. On me.", +8f));
            answers.Add(CreateAnswer(q2Tier2, EmotionState.FrozenBlank, "Liquid. Hot liquid... in a cup.", -10f));

            reactions.Add(CreateReaction(q2Tier2, ConnectionTier.Tier1_Awkward, "Haha... well, liquid in a cup is usually what they serve!", "awkward_grin"));
            reactions.Add(CreateReaction(q2Tier2, ConnectionTier.Tier2_Maybe, "Ooh, solid choice. You can never go wrong with that here.", "nodding_smile"));
            reactions.Add(CreateReaction(q2Tier2, ConnectionTier.Tier3_Strong, "Yes! That flat white here is unmatched. Excellent taste.", "enthusiastic_nod"));
            reactions.Add(CreateReaction(q2Tier2, ConnectionTier.Tier4_SecondDate, "Ooh, you're treating me? Now you're just showing off, haha!", "playful_laugh"));

            // Slot 2: Tier 3 (High Connection variant)
            var q2Tier3 = ScriptableObject.CreateInstance<QuestionData>();
            q2Tier3.slotIndex = 2;
            q2Tier3.connectionTier = ConnectionTier.Tier3_Strong;
            q2Tier3.questionText = "You seem like you've got great taste. What's your go-to comfort drink here?";
            q2Tier3.responseTimeWindow = 6f;
            q2Tier3.socialEventProfile = new SocialEventProfile
            {
                targetNodes = new List<InternalNodeType> { InternalNodeType.Heart },
                effectType = SocialEffectType.DirectEmotionPush,
                pushTargetEmotion = CoreEmotion.Attraction,
                magnitude = 20f
            };
            questions.Add(q2Tier3);

            answers.Add(CreateAnswer(q2Tier3, EmotionState.Calm, "A warm vanilla matcha latte. Calming, not too sweet.", +6f));
            answers.Add(CreateAnswer(q2Tier3, EmotionState.Anxiety, "I—I always second guess it! But usually hot chocolate with extra foam!", +4f));
            answers.Add(CreateAnswer(q2Tier3, EmotionState.Confidence, "Their cortado is unbeatable. I judge every café by it.", +7f));
            answers.Add(CreateAnswer(q2Tier3, EmotionState.Attraction, "Whatever you recommend. I trust your taste completely.", +10f));
            answers.Add(CreateAnswer(q2Tier3, EmotionState.FrozenBlank, "Comfort... drink? Water?", -8f));

            reactions.Add(CreateReaction(q2Tier3, ConnectionTier.Tier1_Awkward, "Haha, well staying hydrated is very important!", "polite_smile"));
            reactions.Add(CreateReaction(q2Tier3, ConnectionTier.Tier2_Maybe, "Oh nice! That sounds super comforting.", "warm_nod"));
            reactions.Add(CreateReaction(q2Tier3, ConnectionTier.Tier3_Strong, "Ah, that's such a vibe! We're definitely on the same wavelength.", "laughing_smile"));
            reactions.Add(CreateReaction(q2Tier3, ConnectionTier.Tier4_SecondDate, "Oh wow, you really know what you're doing. I love that.", "sparkling_eyes"));

            // ════════════════════════════════════════════════════════
            // SLOT 3: WORK & LIFE (Comfort / Vulnerability)
            // Primary Target: Brain & Heart | Direct Emotion Push vs Stress
            // ════════════════════════════════════════════════════════
            var q3Tier2 = ScriptableObject.CreateInstance<QuestionData>();
            q3Tier2.slotIndex = 3;
            q3Tier2.connectionTier = ConnectionTier.Tier2_Maybe;
            q3Tier2.questionText = "So, when you're not out on dates, what usually keeps you busy during the week?";
            q3Tier2.responseTimeWindow = 6.5f;
            q3Tier2.socialEventProfile = new SocialEventProfile
            {
                targetNodes = new List<InternalNodeType> { InternalNodeType.Brain, InternalNodeType.Heart },
                effectType = SocialEffectType.StressEvent,
                stressEventSubtype = StressEventSubtype.Overthinking,
                magnitude = 22f
            };
            questions.Add(q3Tier2);

            answers.Add(CreateAnswer(q3Tier2, EmotionState.Calm, "Working in tech, but I make sure to balance it with evening walks and cooking.", +5f));
            answers.Add(CreateAnswer(q3Tier2, EmotionState.Anxiety, "Mostly stressing over deadlines and wondering if I replied to emails properly!", -2f));
            answers.Add(CreateAnswer(q3Tier2, EmotionState.Confidence, "Building creative projects. I like being deeply invested in what I build.", +6f));
            answers.Add(CreateAnswer(q3Tier2, EmotionState.Attraction, "Work keeps me occupied, but getting to know fascinating people is the real highlight.", +9f));
            answers.Add(CreateAnswer(q3Tier2, EmotionState.Mixture_1, "A pretty grounded routine—work, gym, and unwinding with good music.", +5f));
            answers.Add(CreateAnswer(q3Tier2, EmotionState.Mixture_2, "Leading a small team on design projects. Fast-paced, but I love the challenge.", +7f));
            answers.Add(CreateAnswer(q3Tier2, EmotionState.Mixture_3, "Making time for passion projects and hopefully finding someone to share them with.", +8f));
            answers.Add(CreateAnswer(q3Tier2, EmotionState.Mixture_4, "Lots of chaotic creative energy, balancing five things at once!", +4f));
            answers.Add(CreateAnswer(q3Tier2, EmotionState.Mixture_5, "I dive into hobbies so hard I forget to eat, especially when excited!", +5f));
            answers.Add(CreateAnswer(q3Tier2, EmotionState.Mixture_6, "Pursuing ambitious goals, but always making time for meaningful moments.", +8f));
            answers.Add(CreateAnswer(q3Tier2, EmotionState.FrozenBlank, "I do... tasks. At a desk. Existing.", -12f));

            reactions.Add(CreateReaction(q3Tier2, ConnectionTier.Tier1_Awkward, "Haha, 'existing at a desk'—I mean, aren't we all sometimes!", "sympathetic_grin"));
            reactions.Add(CreateReaction(q3Tier2, ConnectionTier.Tier2_Maybe, "That sounds really balanced. It's so hard to find that balance these days.", "attentive_nod"));
            reactions.Add(CreateReaction(q3Tier2, ConnectionTier.Tier3_Strong, "That's awesome. I can really tell you're passionate when you talk about it!", "engaged_smile"));
            reactions.Add(CreateReaction(q3Tier2, ConnectionTier.Tier4_SecondDate, "I love that mindset. Honestly, it's so refreshing to meet someone so driven yet sweet.", "flirty_smile"));

            // Slot 3: Tier 1 (Awkward recovery variant)
            var q3Tier1 = ScriptableObject.CreateInstance<QuestionData>();
            q3Tier1.slotIndex = 3;
            q3Tier1.connectionTier = ConnectionTier.Tier1_Awkward;
            q3Tier1.questionText = "Are you usually pretty introverted? You seem thoughtful, like you're deep in your head.";
            q3Tier1.responseTimeWindow = 6f;
            q3Tier1.socialEventProfile = new SocialEventProfile
            {
                targetNodes = new List<InternalNodeType> { InternalNodeType.Brain },
                effectType = SocialEffectType.StressEvent,
                stressEventSubtype = StressEventSubtype.Overthinking,
                magnitude = 20f
            };
            questions.Add(q3Tier1);

            answers.Add(CreateAnswer(q3Tier1, EmotionState.Calm, "A bit of both. I take things in first, then open up once I'm comfortable.", +7f));
            answers.Add(CreateAnswer(q3Tier1, EmotionState.Anxiety, "Is my internal monologue showing on my face? Oh boy!", +3f));
            answers.Add(CreateAnswer(q3Tier1, EmotionState.Confidence, "Just paying close attention to you and what you're saying.", +8f));
            answers.Add(CreateAnswer(q3Tier1, EmotionState.Attraction, "Honestly, you just have a very captivating presence. It made me pause.", +10f));
            answers.Add(CreateAnswer(q3Tier1, EmotionState.FrozenBlank, "Head empty. No thoughts. Just staring.", -10f));

            reactions.Add(CreateReaction(q3Tier1, ConnectionTier.Tier1_Awkward, "Haha, oh no! Well, no pressure, take all the time you need.", "soft_reassuring_smile"));
            reactions.Add(CreateReaction(q3Tier1, ConnectionTier.Tier2_Maybe, "I get that completely. It takes a second to settle in on first dates!", "warm_laugh"));
            reactions.Add(CreateReaction(q3Tier1, ConnectionTier.Tier3_Strong, "Haha, that's really charming actually. You don't have to overthink around me.", "radiant_smile"));
            reactions.Add(CreateReaction(q3Tier1, ConnectionTier.Tier4_SecondDate, "Well now you're making my heart skip a beat too!", "giggle_and_blush"));

            return (questions, answers, reactions);
        }

        private static AnswerData CreateAnswer(QuestionData parent, EmotionState state, string text, float delta)
        {
            var a = ScriptableObject.CreateInstance<AnswerData>();
            a.parentQuestion = parent;
            a.emotionState = state;
            a.spokenText = text;
            a.connectionDelta = delta;
            return a;
        }

        private static ReactionData CreateReaction(QuestionData parent, ConnectionTier resultingTier, string text, string clipTag)
        {
            var r = ScriptableObject.CreateInstance<ReactionData>();
            r.parentQuestion = parent;
            r.resultingTier = resultingTier;
            r.reactionText = text;
            r.animationClipTag = clipTag;
            return r;
        }
    }
}
