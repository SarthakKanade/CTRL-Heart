using System.Collections.Generic;
using UnityEngine;
using CtrlHeart.Core.Data;
using CtrlHeart.Core.Data.ScriptableObjects;

namespace CtrlHeart.Core.Testing
{
    /// <summary>
    /// Generates Slot 1 data content and acts as a debug test harness for Day 2 verification.
    /// Master Design Bible Part 5 & Dev Plan Day 2 Checkpoint.
    /// </summary>
    public class Day2TestHarness : MonoBehaviour
    {
        public static (List<QuestionData> questions, List<AnswerData> answers, List<ReactionData> reactions) CreateSlot1Content()
        {
            var questions = new List<QuestionData>();
            var answers = new List<AnswerData>();
            var reactions = new List<ReactionData>();

            // ── Slot 1: Tier 2 (Maybe / Neutral Baseline) Question ──
            var qTier2 = ScriptableObject.CreateInstance<QuestionData>();
            qTier2.slotIndex = 1;
            qTier2.connectionTier = ConnectionTier.Tier2_Maybe;
            qTier2.questionText = "Hey! Thanks for meeting me here. Did you find the place okay?";
            qTier2.responseTimeWindow = 6f;
            qTier2.socialEventProfile = new SocialEventProfile
            {
                targetNodes = new List<InternalNodeType> { InternalNodeType.Brain, InternalNodeType.Voice },
                effectType = SocialEffectType.StressEvent,
                stressEventSubtype = StressEventSubtype.Panic,
                magnitude = 15f
            };
            questions.Add(qTier2);

            // ── Answers for qTier2 (Core + Mixtures + FrozenBlank) ──
            answers.Add(CreateAnswer(qTier2, EmotionState.Calm, "Yeah, no trouble at all. It's a really cozy spot.", +5f));
            answers.Add(CreateAnswer(qTier2, EmotionState.Anxiety, "I—yeah! Well, I took three wrong turns, but I made it!", -2f));
            answers.Add(CreateAnswer(qTier2, EmotionState.Confidence, "Found it instantly. I actually know the barista here.", +4f));
            answers.Add(CreateAnswer(qTier2, EmotionState.Attraction, "Totally fine—honestly, seeing you here made the rush worth it.", +8f));
            answers.Add(CreateAnswer(qTier2, EmotionState.Mixture_1, "I was a bit rushed, but walking in completely settled me.", +4f)); // Calm + Anxiety
            answers.Add(CreateAnswer(qTier2, EmotionState.Mixture_2, "Super easy to find. I was actually hoping to get a seat by the window.", +5f)); // Calm + Confidence
            answers.Add(CreateAnswer(qTier2, EmotionState.Mixture_3, "Yeah, smooth walk. You picked a really great table.", +7f)); // Calm + Attraction
            answers.Add(CreateAnswer(qTier2, EmotionState.Mixture_4, "I almost walked past it twice, but I played it off like I was exploring!", +2f)); // Anxiety + Confidence
            answers.Add(CreateAnswer(qTier2, EmotionState.Mixture_5, "I was nervous I'd be late, so I practically sprinted the last block!", +5f)); // Anxiety + Attraction
            answers.Add(CreateAnswer(qTier2, EmotionState.Mixture_6, "Oh, absolutely. I made sure to arrive early so you wouldn't wait.", +6f)); // Confidence + Attraction
            answers.Add(CreateAnswer(qTier2, EmotionState.FrozenBlank, "Uh... yeah. Doors. I walked through them.", -10f)); // Frozen / Blank

            // ── Reactions for qTier2 ──
            reactions.Add(CreateReaction(qTier2, ConnectionTier.Tier1_Awkward, "Oh... haha, okay! Well, take a breather, you look a bit on edge.", "concerned_look"));
            reactions.Add(CreateReaction(qTier2, ConnectionTier.Tier2_Maybe, "Great! I was worried GPS might send you down the alley behind here.", "polite_smile"));
            reactions.Add(CreateReaction(qTier2, ConnectionTier.Tier3_Strong, "Haha, awesome. I'm glad you like the vibe here too!", "warm_laugh"));
            reactions.Add(CreateReaction(qTier2, ConnectionTier.Tier4_SecondDate, "Aww, that's sweet of you to say! I'm really glad we're doing this.", "blushing_smile"));

            // ── Slot 1: Tier 1 (Awkward Start) Question ──
            var qTier1 = ScriptableObject.CreateInstance<QuestionData>();
            qTier1.slotIndex = 1;
            qTier1.connectionTier = ConnectionTier.Tier1_Awkward;
            qTier1.questionText = "Oh, hi! You look a little tense... is everything alright?";
            qTier1.responseTimeWindow = 5f;
            qTier1.socialEventProfile = new SocialEventProfile
            {
                targetNodes = new List<InternalNodeType> { InternalNodeType.Heart, InternalNodeType.Body },
                effectType = SocialEffectType.StressEvent,
                stressEventSubtype = StressEventSubtype.HeartFlutter,
                magnitude = 25f
            };
            questions.Add(qTier1);

            answers.Add(CreateAnswer(qTier1, EmotionState.Calm, "Just catching my breath from the walk, all good now.", +8f));
            answers.Add(CreateAnswer(qTier1, EmotionState.Anxiety, "Is it obvious? Ah, first date jitters, standard procedure!", +3f));
            answers.Add(CreateAnswer(qTier1, EmotionState.Confidence, "Never better. Just focused on having a good conversation.", +5f));
            answers.Add(CreateAnswer(qTier1, EmotionState.Attraction, "Honestly? Just a bit stunned by how great you look.", +10f));
            answers.Add(CreateAnswer(qTier1, EmotionState.FrozenBlank, "Everything is... vibrating.", -12f));

            reactions.Add(CreateReaction(qTier1, ConnectionTier.Tier1_Awkward, "Right... well, don't worry, I won't bite!", "nervous_chuckle"));
            reactions.Add(CreateReaction(qTier1, ConnectionTier.Tier2_Maybe, "Haha, totally fair. Let's just relax and get some coffee.", "reassuring_nod"));
            reactions.Add(CreateReaction(qTier1, ConnectionTier.Tier3_Strong, "Haha, well that's adorable. You can breathe now!", "warm_smile"));
            reactions.Add(CreateReaction(qTier1, ConnectionTier.Tier4_SecondDate, "Oh stop, you're making me blush already!", "laugh_and_blush"));

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
