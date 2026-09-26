using System.Collections.Generic;
using UnityEngine;
using CtrlHeart.Core.Data;
using CtrlHeart.Core.Data.ScriptableObjects;

namespace CtrlHeart.Core.Testing
{
    /// <summary>
    /// Master content repository for all 10 slots of the date.
    /// Completes Day 4 content scale-up across:
    /// - Slot 1: Arrival & First Impressions
    /// - Slot 2: Ordering & Drinks
    /// - Slot 3: Work & Routine
    /// - Slot 4: Spilled Water Incident (Physical Event)
    /// - Slot 5: Travel & Story Beat
    /// - Slot 6: Humor & Music (Awkwardness / Comfort)
    /// - Slot 7: Insecurities & Vulnerability (Deep Personal Conversation)
    /// - Slot 8: Direct Compliment / Chemistry Check (Emotional Climax)
    /// - Slot 9: Awkward Bill Payment / Final Stumble
    /// - Slot 10: "Would you want to do this again?" (The Final Question)
    /// </summary>
    public static class FullDateContent
    {
        public static (List<QuestionData> questions, List<AnswerData> answers, List<ReactionData> reactions) CreateFull10SlotsContent()
        {
            var questions = new List<QuestionData>();
            var answers = new List<AnswerData>();
            var reactions = new List<ReactionData>();

            // Include Slots 1-3
            var (s13Q, s13A, s13R) = Day3Content.CreateSlots1To3Content();
            questions.AddRange(s13Q);
            answers.AddRange(s13A);
            reactions.AddRange(s13R);

            // ════════════════════════════════════════════════════════
            // SLOT 4: SPILLED WATER INCIDENT (Physical Social Event)
            // Primary Target: Body & Voice | SweatSurge + Anxiety
            // ════════════════════════════════════════════════════════
            var q4 = CreateQuestion(4, ConnectionTier.Tier2_Maybe, 
                "*Gasp* Oh no! The waiter bumped our table—did any water splash onto your sleeve?!", 5.5f,
                new List<InternalNodeType> { InternalNodeType.Body, InternalNodeType.Voice },
                SocialEffectType.StressEvent, StressEventSubtype.SweatSurge, 26f);
            questions.Add(q4);

            answers.Add(CreateAnswer(q4, EmotionState.Calm, "Totally fine! Just a couple drops on the napkin, not a scratch on me.", +6f));
            answers.Add(CreateAnswer(q4, EmotionState.Anxiety, "Ah! Nope, I'm good! Well, my shoe might be damp, but who needs dry socks!", -2f));
            answers.Add(CreateAnswer(q4, EmotionState.Confidence, "Not at all, lightning reflexes. Here, take these extra napkins for your side.", +7f));
            answers.Add(CreateAnswer(q4, EmotionState.Attraction, "I barely noticed—I was too distracted listening to you.", +8f));
            answers.Add(CreateAnswer(q4, EmotionState.Mixture_1, "All good! Honestly, it wouldn't be a memorable date without a little chaos.", +5f));
            answers.Add(CreateAnswer(q4, EmotionState.Mixture_2, "Nothing a paper towel can't fix. Let's get the table wiped down.", +6f));
            answers.Add(CreateAnswer(q4, EmotionState.Mixture_3, "Haha, don't worry about me. Are you dry?", +8f));
            answers.Add(CreateAnswer(q4, EmotionState.Mixture_4, "Crisis averted! I almost knocked over the salt shaker trying to dodge it!", +3f));
            answers.Add(CreateAnswer(q4, EmotionState.Mixture_5, "I jumped a foot in the air, didn't I? Classic me!", +5f));
            answers.Add(CreateAnswer(q4, EmotionState.Mixture_6, "Handled smoothly. We survived our first minor disaster together.", +8f));
            answers.Add(CreateAnswer(q4, EmotionState.FrozenBlank, "Water... cold. Napkin wet.", -10f));

            reactions.Add(CreateReaction(q4, ConnectionTier.Tier1_Awkward, "Oh gosh, sorry! Let me flag down the waiter...", "worried_expression"));
            reactions.Add(CreateReaction(q4, ConnectionTier.Tier2_Maybe, "Phew! Glad neither of us got soaked. Good catch with the napkins!", "polite_laugh"));
            reactions.Add(CreateReaction(q4, ConnectionTier.Tier3_Strong, "Haha, you handled that with absolute grace! Thanks for helping out.", "warm_smile"));
            reactions.Add(CreateReaction(q4, ConnectionTier.Tier4_SecondDate, "Haha! Look at us surviving café hazards. You're wonderful.", "affectionate_laugh"));

            // ════════════════════════════════════════════════════════
            // SLOT 5: TRAVEL & ADVENTURE (Story Beat)
            // Primary Target: Lungs & Brain | Direct Emotion Push: Confidence
            // ════════════════════════════════════════════════════════
            var q5 = CreateQuestion(5, ConnectionTier.Tier2_Maybe,
                "If you could pack a bag right now and board a flight anywhere, where would we be landing?", 6f,
                new List<InternalNodeType> { InternalNodeType.Lungs, InternalNodeType.Brain },
                SocialEffectType.DirectEmotionPush, StressEventSubtype.None, 24f, CoreEmotion.Confidence);
            questions.Add(q5);

            answers.Add(CreateAnswer(q5, EmotionState.Calm, "A quiet coastal cabin in Norway or Scotland. Mist, hot tea, and quiet cliffs.", +5f));
            answers.Add(CreateAnswer(q5, EmotionState.Anxiety, "Anywhere that doesn't have turbulence! Maybe just a very fast train instead?", +1f));
            answers.Add(CreateAnswer(q5, EmotionState.Confidence, "Tokyo or Kyoto. Incredible street food, neon nights, and non-stop exploring.", +7f));
            answers.Add(CreateAnswer(q5, EmotionState.Attraction, "Anywhere with warm sunsets and great food, especially with company like you.", +9f));
            answers.Add(CreateAnswer(q5, EmotionState.FrozenBlank, "Airport... security... lost passport.", -9f));

            reactions.Add(CreateReaction(q5, ConnectionTier.Tier1_Awkward, "Haha, well lost luggage is definitely a mood killer!", "nervous_smile"));
            reactions.Add(CreateReaction(q5, ConnectionTier.Tier2_Maybe, "Ooh, that sounds like a fantastic getaway trip.", "daydreaming_look"));
            reactions.Add(CreateReaction(q5, ConnectionTier.Tier3_Strong, "No way, that is literally top of my bucket list! We have to compare notes.", "delighted_laugh"));
            reactions.Add(CreateReaction(q5, ConnectionTier.Tier4_SecondDate, "Careful, keep talking like that and I might hold you to booking tickets!", "playful_wink"));

            // ════════════════════════════════════════════════════════
            // SLOT 6: HUMOR & SILLINESS (Comfort Level Check)
            // Primary Target: Voice & Heart | Stress Event: AwkwardSilence
            // ════════════════════════════════════════════════════════
            var q6 = CreateQuestion(6, ConnectionTier.Tier2_Maybe,
                "Be totally honest—what's an embarrassing guilty pleasure song you blast in the car when nobody is looking?", 6f,
                new List<InternalNodeType> { InternalNodeType.Voice, InternalNodeType.Heart },
                SocialEffectType.StressEvent, StressEventSubtype.AwkwardSilence, 20f);
            questions.Add(q6);

            answers.Add(CreateAnswer(q6, EmotionState.Calm, "Classic 80s synth-pop. Wham! and Tears for Fears. Zero shame.", +6f));
            answers.Add(CreateAnswer(q6, EmotionState.Anxiety, "Promise you won't judge me?! It's 2000s boybands and I know every rap breakdown!", +4f));
            answers.Add(CreateAnswer(q6, EmotionState.Confidence, "Queen's Bohemian Rhapsody, singing all four vocal parts at max volume.", +7f));
            answers.Add(CreateAnswer(q6, EmotionState.Attraction, "Cheesy romantic acoustic covers. I'm a hopeless sap when good music plays.", +9f));
            answers.Add(CreateAnswer(q6, EmotionState.FrozenBlank, "Sounds... frequencies... radio waves.", -10f));

            reactions.Add(CreateReaction(q6, ConnectionTier.Tier1_Awkward, "Haha, you really clammed up there! I won't interrogate you!", "gentle_laugh"));
            reactions.Add(CreateReaction(q6, ConnectionTier.Tier2_Maybe, "Haha, that's hilarious! Those songs are absolute bangers though.", "broad_smile"));
            reactions.Add(CreateReaction(q6, ConnectionTier.Tier3_Strong, "YES! Oh that is so good. You have earned major cool points in my book.", "clapping_laugh"));
            reactions.Add(CreateReaction(q6, ConnectionTier.Tier4_SecondDate, "Now I need you to sing that on our next car ride together, no excuses!", "teasing_smile"));

            // ════════════════════════════════════════════════════════
            // SLOT 7: VULNERABILITY (Personal Conversation)
            // Primary Target: Brain & Lungs | Stress Event: Overthinking
            // ════════════════════════════════════════════════════════
            var q7 = CreateQuestion(7, ConnectionTier.Tier2_Maybe,
                "What's something you're trying to get better at recently? Like a personal goal or mindset shift?", 6.5f,
                new List<InternalNodeType> { InternalNodeType.Brain, InternalNodeType.Lungs },
                SocialEffectType.StressEvent, StressEventSubtype.Overthinking, 28f);
            questions.Add(q7);

            answers.Add(CreateAnswer(q7, EmotionState.Calm, "Being present in the moment instead of constantly rehearsing the next week.", +7f));
            answers.Add(CreateAnswer(q7, EmotionState.Anxiety, "Not overthinking every tiny social interaction... though that's an ongoing battle!", +3f));
            answers.Add(CreateAnswer(q7, EmotionState.Confidence, "Taking bigger creative risks and backing myself when things get difficult.", +8f));
            answers.Add(CreateAnswer(q7, EmotionState.Attraction, "Opening up and letting people see the authentic version of me, flaws and all.", +10f));
            answers.Add(CreateAnswer(q7, EmotionState.FrozenBlank, "Improvement... brain update... error 404.", -12f));

            reactions.Add(CreateReaction(q7, ConnectionTier.Tier1_Awkward, "Haha, hey, self-improvement is hard! Don't stress too much.", "soft_smile"));
            reactions.Add(CreateReaction(q7, ConnectionTier.Tier2_Maybe, "I relate to that so much. It's really refreshing hearing someone say that.", "genuine_nod"));
            reactions.Add(CreateReaction(q7, ConnectionTier.Tier3_Strong, "That's honestly profound. I really appreciate how genuine you are.", "tender_look"));
            reactions.Add(CreateReaction(q7, ConnectionTier.Tier4_SecondDate, "You're doing an amazing job. Honestly, it's so rare to connect like this.", "deep_smile"));

            // ════════════════════════════════════════════════════════
            // SLOT 8: THE CHEMISTRY CHECK (Emotional Climax)
            // Primary Target: Heart & Brain | Direct Emotion Push: Attraction + HeartFlutter
            // ════════════════════════════════════════════════════════
            var q8 = CreateQuestion(8, ConnectionTier.Tier3_Strong,
                "I have to say... I was pretty nervous before arriving today, but sitting here with you has felt surprisingly natural.", 6f,
                new List<InternalNodeType> { InternalNodeType.Heart, InternalNodeType.Brain },
                SocialEffectType.DirectEmotionPush, StressEventSubtype.None, 30f, CoreEmotion.Attraction);
            questions.Add(q8);

            answers.Add(CreateAnswer(q8, EmotionState.Calm, "Me too. You have this calming energy that just makes conversation easy.", +8f));
            answers.Add(CreateAnswer(q8, EmotionState.Anxiety, "Oh thank goodness! Inside my head it felt like a sitcom disaster, but I'm so glad!", +4f));
            answers.Add(CreateAnswer(q8, EmotionState.Confidence, "I had a feeling we'd hit it off. I'm really glad we made this happen.", +9f));
            answers.Add(CreateAnswer(q8, EmotionState.Attraction, "Honestly, my heart was racing when I saw you walk in. You look stunning.", +12f));
            answers.Add(CreateAnswer(q8, EmotionState.FrozenBlank, "You... natural. Me... meat robot.", -14f));

            reactions.Add(CreateReaction(q8, ConnectionTier.Tier1_Awkward, "Haha! Meat robot?! Okay, you definitely have a weird sense of humor!", "startled_chuckle"));
            reactions.Add(CreateReaction(q8, ConnectionTier.Tier2_Maybe, "Aww, thank you! It really has been a lovely afternoon.", "warm_blush"));
            reactions.Add(CreateReaction(q8, ConnectionTier.Tier3_Strong, "Aww, stop, that is so sweet! You just made my whole week.", "happy_blush"));
            reactions.Add(CreateReaction(q8, ConnectionTier.Tier4_SecondDate, "My heart was racing too... I think this is definitely something special.", "intense_loving_smile"));

            // ════════════════════════════════════════════════════════
            // SLOT 9: THE CHECK ARRIVES (Awkward Real-World Friction)
            // Primary Target: Voice & Body | Stress Event: SweatSurge
            // ════════════════════════════════════════════════════════
            var q9 = CreateQuestion(9, ConnectionTier.Tier2_Maybe,
                "Looks like the waiter just left the bill on our table. How do you usually like to handle this?", 5.5f,
                new List<InternalNodeType> { InternalNodeType.Voice, InternalNodeType.Body },
                SocialEffectType.StressEvent, StressEventSubtype.SweatSurge, 22f);
            questions.Add(q9);

            answers.Add(CreateAnswer(q9, EmotionState.Calm, "I've got this one covered. You can get the drinks next time.", +7f));
            answers.Add(CreateAnswer(q9, EmotionState.Anxiety, "Card fight! I mean—let's split! Or I pay! Whatever is polite!", +2f));
            answers.Add(CreateAnswer(q9, EmotionState.Confidence, "It's already taken care of. My treat, completely.", +8f));
            answers.Add(CreateAnswer(q9, EmotionState.Attraction, "I'd love to treat you. It was worth every penny just to share this time.", +10f));
            answers.Add(CreateAnswer(q9, EmotionState.FrozenBlank, "Money... math... paper rectangle.", -10f));

            reactions.Add(CreateReaction(q9, ConnectionTier.Tier1_Awkward, "Haha, well let's just go half-and-half then!", "polite_nod"));
            reactions.Add(CreateReaction(q9, ConnectionTier.Tier2_Maybe, "That's so generous of you, thank you! Next round is definitely on me.", "grateful_smile"));
            reactions.Add(CreateReaction(q9, ConnectionTier.Tier3_Strong, "You're too kind! Thank you so much for being such a sweet host.", "sweet_smile"));
            reactions.Add(CreateReaction(q9, ConnectionTier.Tier4_SecondDate, "Ooh, 'next time'? I'm definitely holding you to that second date!", "glowing_grin"));

            // ════════════════════════════════════════════════════════
            // SLOT 10: THE FINAL QUESTION
            // Primary Target: Brain, Heart & Voice | Panic + HeartFlutter
            // ════════════════════════════════════════════════════════
            var q10 = CreateQuestion(10, ConnectionTier.Tier2_Maybe,
                "I really had a wonderful time with you today... would you want to do this again sometime soon?", 6.5f,
                new List<InternalNodeType> { InternalNodeType.Heart, InternalNodeType.Voice, InternalNodeType.Brain },
                SocialEffectType.StressEvent, StressEventSubtype.HeartFlutter, 32f);
            questions.Add(q10);

            answers.Add(CreateAnswer(q10, EmotionState.Calm, "Absolutely. I'd love to see you again. Let's pick a day later this week.", +10f));
            answers.Add(CreateAnswer(q10, EmotionState.Anxiety, "YES! I mean—yes! Definitely. If you're not sick of my nervous chatter yet!", +5f));
            answers.Add(CreateAnswer(q10, EmotionState.Confidence, "Without a doubt. I already have a great dinner spot in mind for us.", +12f));
            answers.Add(CreateAnswer(q10, EmotionState.Attraction, "There is nothing I'd like more. Today was magical, and I want more of it.", +15f));
            answers.Add(CreateAnswer(q10, EmotionState.FrozenBlank, "Again... time is a flat circle... goodbye.", -15f));

            reactions.Add(CreateReaction(q10, ConnectionTier.Tier1_Awkward, "Haha... well, take care! Text me later maybe.", "polite_wave"));
            reactions.Add(CreateReaction(q10, ConnectionTier.Tier2_Maybe, "Yay! Let's text tonight and find a time!", "happy_wave"));
            reactions.Add(CreateReaction(q10, ConnectionTier.Tier3_Strong, "I can't wait! Today was genuinely wonderful.", "big_smile"));
            reactions.Add(CreateReaction(q10, ConnectionTier.Tier4_SecondDate, "YES! Best date I've been on in forever. Text me as soon as you get home!", "beaming_hug"));

            return (questions, answers, reactions);
        }

        private static QuestionData CreateQuestion(int slot, ConnectionTier tier, string text, float timeWindow,
            List<InternalNodeType> targets, SocialEffectType effect, StressEventSubtype sub, float mag, CoreEmotion pushEmotion = CoreEmotion.Calm)
        {
            var q = ScriptableObject.CreateInstance<QuestionData>();
            q.slotIndex = slot;
            q.connectionTier = tier;
            q.questionText = text;
            q.responseTimeWindow = timeWindow;
            q.socialEventProfile = new SocialEventProfile
            {
                targetNodes = targets,
                effectType = effect,
                stressEventSubtype = sub,
                pushTargetEmotion = pushEmotion,
                magnitude = mag
            };
            return q;
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
