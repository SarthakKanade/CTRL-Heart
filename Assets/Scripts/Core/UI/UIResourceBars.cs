using UnityEngine;
using UnityEngine.UI;
using CtrlHeart.Core.Data;
using CtrlHeart.Core.Visuals;

namespace CtrlHeart.Core.UI
{
    /// <summary>
    /// Displays the 4 continuous meters with distinct colors and tempos.
    /// Oxygen, Focus, Composure, Connection.
    /// Master Design Bible Part 2 §2.5, §2.6 & Dev Plan Day 1 Track B.
    /// </summary>
    public class UIResourceBars : MonoBehaviour
    {
        [Header("Oxygen")]
        [SerializeField] private Image oxygenFill;
        [SerializeField] private Text oxygenLabel;

        [Header("Focus")]
        [SerializeField] private Image focusFill;
        [SerializeField] private Text focusLabel;

        [Header("Composure")]
        [SerializeField] private Image composureFill;
        [SerializeField] private Text composureLabel;

        [Header("Connection")]
        [SerializeField] private Image connectionFill;
        [SerializeField] private Text connectionLabel;

        public void Initialize()
        {
            if (oxygenFill != null) oxygenFill.color = VisualTheme.ColorOxygen;
            if (focusFill != null) focusFill.color = VisualTheme.ColorFocus;
            if (composureFill != null) composureFill.color = VisualTheme.ColorComposure;
            if (connectionFill != null) connectionFill.color = VisualTheme.ColorConnection;
        }

        public void UpdateBars(ResourceState state)
        {
            if (state == null) return;

            if (oxygenFill != null) oxygenFill.fillAmount = Mathf.Clamp01(state.oxygen / 100f);
            if (focusFill != null) focusFill.fillAmount = Mathf.Clamp01(state.focus / 100f);
            if (composureFill != null) composureFill.fillAmount = Mathf.Clamp01(state.composure / 100f);
            if (connectionFill != null) connectionFill.fillAmount = Mathf.Clamp01(state.connection / 100f);

            if (oxygenLabel != null) oxygenLabel.text = $"O2: {Mathf.RoundToInt(state.oxygen)}%";
            if (focusLabel != null) focusLabel.text = $"Focus: {Mathf.RoundToInt(state.focus)}%";
            if (composureLabel != null) composureLabel.text = $"Composure: {Mathf.RoundToInt(state.composure)}%";
            if (connectionLabel != null) connectionLabel.text = $"Connection: {Mathf.RoundToInt(state.connection)}%";
        }
    }
}
