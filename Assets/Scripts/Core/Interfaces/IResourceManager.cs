using System;
using CtrlHeart.Core.Data;

namespace CtrlHeart.Core.Interfaces
{
    /// <summary>
    /// Manages the 4 continuous meters: Oxygen, Focus, Composure, Connection.
    /// Master Design Bible Part 2 §2.5 & §2.6.
    /// </summary>
    public interface IResourceManager
    {
        ResourceState CurrentState { get; }
        void TickPassiveRegen(float deltaTime, float lungsHealth, float brainHealth);
        void ModifyOxygen(float delta);
        void ModifyFocus(float delta);
        void ModifyComposure(float delta);
        void ModifyConnection(float delta);
        bool IsFocusLow();
        bool IsMeltdown();
        bool IsDateCollapsed();
    }
}
