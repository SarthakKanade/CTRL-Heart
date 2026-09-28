using System;
using UnityEngine;

namespace CtrlHeart.Core.Data.ScriptableObjects
{
    /// <summary>
    /// Represents an authored answer line matching an emotion state for a specific question.
    /// Master Design Bible Part 5 §5.4 & §5.6.
    /// </summary>
    [CreateAssetMenu(fileName = "Answer_Q_State", menuName = "CTRL-Heart/Answer Data")]
    public class AnswerData : ScriptableObject
    {
        [Tooltip("Reference to the parent Question")]
        public QuestionData parentQuestion;

        [Tooltip("The EmotionState (1 of 10, or FrozenBlank) that selects this answer")]
        public EmotionState emotionState;

        [TextArea(2, 4)]
        public string spokenText;

        [Tooltip("The pre-authored Connection delta applied immediately upon selection")]
        public float connectionDelta = 0f;

        [Tooltip("The authored grade band (HighFit, Reasonable, Poor, ActivelyWrong, FrozenBlank)")]
        public string band = "Reasonable";

        [Tooltip("True if answerText contains only stage direction and no spoken dialogue")]
        public bool isWordless = false;
    }
}
