using System.Collections.Generic;
using UnityEngine;
using CtrlHeart.Core.Data;
using CtrlHeart.Core.Data.ScriptableObjects;
using CtrlHeart.Core.Interfaces;

namespace CtrlHeart.Core.Systems
{
    /// <summary>
    /// Handles selection of Questions, resolution into Answers, and post-answer Reactions.
    /// Implements:
    /// - Question selected by slot index + ConnectionTier (Master Design Bible Part 5 §5.3)
    /// - Resolution on timer expiry via Primary Target Node dominant emotion (Part 5 §5.2)
    /// - Body Override (critical Body health forces Frozen/Blank answer)
    /// - Reaction selection by question + post-answer ConnectionTier (Part 5 §5.5)
    /// </summary>
    public class DialogueManager : MonoBehaviour, IDialogueManager
    {
        [Header("Authored Content Registry")]
        [SerializeField] private List<QuestionData> registeredQuestions = new List<QuestionData>();
        [SerializeField] private List<AnswerData> registeredAnswers = new List<AnswerData>();
        [SerializeField] private List<ReactionData> registeredReactions = new List<ReactionData>();

        public void RegisterContent(IEnumerable<QuestionData> questions, IEnumerable<AnswerData> answers, IEnumerable<ReactionData> reactions)
        {
            registeredQuestions.AddRange(questions);
            registeredAnswers.AddRange(answers);
            registeredReactions.AddRange(reactions);
        }

        public QuestionData SelectQuestion(int slotIndex, ConnectionTier tier)
        {
            if (tier == ConnectionTier.Tier0_Collapse)
            {
                return null; // Date ends immediately
            }

            foreach (var q in registeredQuestions)
            {
                if (q != null && q.slotIndex == slotIndex && q.connectionTier == tier)
                {
                    return q;
                }
            }

            // Fallback to any question for this slot if exact tier variant not yet authored
            foreach (var q in registeredQuestions)
            {
                if (q != null && q.slotIndex == slotIndex) return q;
            }

            return null;
        }

        public AnswerData ResolveAnswer(QuestionData question, EmotionState dominantState, bool isCriticalBody)
        {
            if (question == null) return null;

            // 1. Body Override: critically low Body health forces Frozen/Blank
            if (isCriticalBody)
            {
                dominantState = EmotionState.FrozenBlank;
            }

            // Look up exact match
            foreach (var a in registeredAnswers)
            {
                if (a != null && a.parentQuestion == question && a.emotionState == dominantState)
                {
                    return a;
                }
            }

            // Fallback to Frozen/Blank if state answer missing
            foreach (var a in registeredAnswers)
            {
                if (a != null && a.parentQuestion == question && a.emotionState == EmotionState.FrozenBlank)
                {
                    return a;
                }
            }

            // Fallback to any answer for this question
            foreach (var a in registeredAnswers)
            {
                if (a != null && a.parentQuestion == question)
                {
                    return a;
                }
            }

            return null;
        }

        public ReactionData SelectReaction(QuestionData question, EmotionState state)
        {
            if (question == null) return null;

            // 1:1 lookup by question and player emotion state
            foreach (var r in registeredReactions)
            {
                if (r != null && r.parentQuestion == question && r.emotionState == state)
                {
                    return r;
                }
            }

            // Fallback: match by question and Frozen/Blank
            foreach (var r in registeredReactions)
            {
                if (r != null && r.parentQuestion == question && r.emotionState == EmotionState.FrozenBlank)
                {
                    return r;
                }
            }

            // Fallback to any reaction for this question
            foreach (var r in registeredReactions)
            {
                if (r != null && r.parentQuestion == question) return r;
            }

            return null;
        }

        public ReactionData SelectReaction(QuestionData question, ConnectionTier postAnswerTier)
        {
            if (question == null) return null;

            foreach (var r in registeredReactions)
            {
                if (r != null && r.parentQuestion == question && r.resultingTier == postAnswerTier)
                {
                    return r;
                }
            }

            foreach (var r in registeredReactions)
            {
                if (r != null && r.parentQuestion == question) return r;
            }

            return null;
        }

        private bool IsMixtureState(EmotionState state)
        {
            return state >= EmotionState.CalmAnxiety && state <= EmotionState.ConfidenceAttraction;
        }

        private EmotionState FallbackToCoreEmotion(EmotionState mixture)
        {
            // Maps each mixture back to its lead core emotion
            return mixture switch
            {
                EmotionState.CalmAnxiety => EmotionState.Calm,              // Calm + Anxiety -> Calm
                EmotionState.CalmConfidence => EmotionState.Confidence,    // Calm + Confidence -> Confidence
                EmotionState.CalmAttraction => EmotionState.Attraction,    // Calm + Attraction -> Attraction
                EmotionState.AnxietyConfidence => EmotionState.Anxiety,    // Anxiety + Confidence -> Anxiety
                EmotionState.AnxietyAttraction => EmotionState.Attraction, // Anxiety + Attraction -> Attraction
                EmotionState.ConfidenceAttraction => EmotionState.Confidence, // Confidence + Attraction -> Confidence
                _ => EmotionState.Calm
            };
        }
    }
}
