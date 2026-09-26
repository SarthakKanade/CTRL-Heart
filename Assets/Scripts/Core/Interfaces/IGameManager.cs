using System;
using CtrlHeart.Core.Data;

namespace CtrlHeart.Core.Interfaces
{
    /// <summary>
    /// Coordinates between Date systems and Internal Mind systems.
    /// Master Design Bible Part 1 §1.3.
    /// </summary>
    public interface IGameManager
    {
        bool IsGameActive { get; }
        int CurrentSlotIndex { get; }
        void StartDate();
        void OnSlotTimerExpired();
        void TriggerEnding(EndingType ending);
    }
}
