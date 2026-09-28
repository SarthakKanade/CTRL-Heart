using System;
using CtrlHeart.Core.Data;

namespace CtrlHeart.Core.Interfaces
{
    /// <summary>
    /// Manages the 5-node internal graph and evaluates emotional dominance.
    /// Master Design Bible Part 2 §2.3 & §2.4.
    /// </summary>
    public interface IMindMapManager
    {
        NodeState GetNode(InternalNodeType type);
        EmotionState GetDominantState(InternalNodeType type);
        void ApplyInfluence(InternalNodeType targetNode, CoreEmotion emotion, float deltaPercent);
        void ApplyDamage(InternalNodeType targetNode, float damage);
        void HealNode(InternalNodeType targetNode, float amount);
        bool IsBodyCriticallyLow();
        float CalculateEquilibriumScore();
    }
}
