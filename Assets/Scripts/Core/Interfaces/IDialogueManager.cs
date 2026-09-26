using System;
using CtrlHeart.Core.Data;
using CtrlHeart.Core.Data.ScriptableObjects;

namespace CtrlHeart.Core.Interfaces
{
    /// <summary>
    /// Handles question selection, resolution to answer, and reaction selection.
    /// Master Design Bible Part 5 §5.2.
    /// </summary>
    public interface IDialogueManager
    {
        QuestionData SelectQuestion(int slotIndex, ConnectionTier tier);
        AnswerData ResolveAnswer(QuestionData question, EmotionState dominantState, bool isLowFocus, bool isCriticalBody);
        ReactionData SelectReaction(QuestionData question, ConnectionTier postAnswerTier);
    }
}
