using System;
using UnityEngine;
using CtrlHeart.Core.Data;
using CtrlHeart.Core.Interfaces;

namespace CtrlHeart.Core.Systems
{
    /// <summary>
    /// Manages continuous resources: Oxygen, Focus, Composure, Connection.
    /// Implements passive regen curves driven by Lungs and Brain health.
    /// Master Design Bible Part 2 §2.5, §2.6 & Dev Plan Day 2 Track A.
    /// </summary>
    public class ResourceManager : MonoBehaviour, IResourceManager
    {
        [Header("State")]
        [SerializeField] private ResourceState state = new ResourceState();

        [Header("Tuning")]
        [SerializeField] private float focusLowThreshold = 20f;

        public ResourceState CurrentState => state;

        public event Action<ResourceState> OnResourceChanged;

        public void TickPassiveRegen(float deltaTime, float lungsHealth, float brainHealth)
        {
            float oxygenGain = SimulationMath.CalculateOxygenRegen(lungsHealth, state.oxygenBaseRegenRate) * deltaTime;
            float focusGain = SimulationMath.CalculateFocusRegen(brainHealth, state.focusBaseRegenRate) * deltaTime;

            state.oxygen = Mathf.Clamp(state.oxygen + oxygenGain, ResourceState.MIN_VALUE, ResourceState.MAX_VALUE);
            state.focus = Mathf.Clamp(state.focus + focusGain, ResourceState.MIN_VALUE, ResourceState.MAX_VALUE);

            OnResourceChanged?.Invoke(state);
        }

        public void ModifyOxygen(float delta)
        {
            state.oxygen = Mathf.Clamp(state.oxygen + delta, ResourceState.MIN_VALUE, ResourceState.MAX_VALUE);
            OnResourceChanged?.Invoke(state);
        }

        public void ModifyFocus(float delta)
        {
            state.focus = Mathf.Clamp(state.focus + delta, ResourceState.MIN_VALUE, ResourceState.MAX_VALUE);
            OnResourceChanged?.Invoke(state);
        }

        public void ModifyComposure(float delta)
        {
            state.composure = Mathf.Clamp(state.composure + delta, ResourceState.MIN_VALUE, ResourceState.MAX_VALUE);
            OnResourceChanged?.Invoke(state);
        }

        public void ModifyConnection(float delta)
        {
            state.connection = Mathf.Clamp(state.connection + delta, ResourceState.MIN_VALUE, ResourceState.MAX_VALUE);
            OnResourceChanged?.Invoke(state);
        }

        public bool IsFocusLow() => state.focus <= focusLowThreshold;
        public bool IsMeltdown() => state.IsMeltdown();
        public bool IsDateCollapsed() => state.IsDateCollapsed();
    }
}
