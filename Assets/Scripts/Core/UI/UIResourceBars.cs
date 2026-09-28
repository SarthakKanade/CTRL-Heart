using UnityEngine;
using UnityEngine.UI;
using CtrlHeart.Core.Data;
using CtrlHeart.Core.Visuals;

namespace CtrlHeart.Core.UI
{
    /// <summary>
    /// Displays the 3 continuous meters in the "Internal Resources & States" right panel.
    /// Oxygen, Composure, Connection.
    /// Matches the UI Reference: icons, rounded glowing pill bars, and "XX / 100" readouts.
    /// </summary>
    public class UIResourceBars : MonoBehaviour
    {
        [Header("Oxygen (Teal)")]
        [SerializeField] private Image oxygenIcon;
        [SerializeField] private Image oxygenFill;
        [SerializeField] private Text oxygenValueText;

        [Header("Composure (Coral)")]
        [SerializeField] private Image composureIcon;
        [SerializeField] private Image composureFill;
        [SerializeField] private Text composureValueText;

        [Header("Connection (Pink)")]
        [SerializeField] private Image connectionIcon;
        [SerializeField] private Image connectionFill;
        [SerializeField] private Text connectionValueText;

        private float targetOxygen = 80f;
        private float targetComposure = 75f;
        private float targetConnection = 50f;

        private float displayedOxygen = 80f;
        private float displayedComposure = 75f;
        private float displayedConnection = 50f;

        public void Initialize()
        {
            if (oxygenIcon != null) oxygenIcon.sprite = UIProceduralTextureGenerator.GetSprite("icon_lungs");
            if (composureIcon != null) composureIcon.sprite = UIProceduralTextureGenerator.GetSprite("icon_ecg");
            if (connectionIcon != null) connectionIcon.sprite = UIProceduralTextureGenerator.GetSprite("icon_heart");

            if (oxygenFill != null)
            {
                var s = UIProceduralTextureGenerator.GetSprite("rpg_bar_green");
                oxygenFill.sprite = s != null ? s : UIProceduralTextureGenerator.GetSprite("bar_pill");
                oxygenFill.color = s != null ? Color.white : VisualTheme.ColorOxygen;
            }
            if (composureFill != null)
            {
                var s = UIProceduralTextureGenerator.GetSprite("rpg_bar_red");
                composureFill.sprite = s != null ? s : UIProceduralTextureGenerator.GetSprite("bar_pill");
                composureFill.color = s != null ? Color.white : VisualTheme.ColorComposure;
            }
            if (connectionFill != null)
            {
                var s = UIProceduralTextureGenerator.GetSprite("rpg_bar_yellow");
                connectionFill.sprite = s != null ? s : UIProceduralTextureGenerator.GetSprite("bar_pill");
                connectionFill.color = s != null ? Color.white : VisualTheme.ColorConnection;
            }
        }

        public void AssignReferences(
            Image o2Icon, Image o2Fill, Text o2Text,
            Image cIcon, Image cFill, Text cText,
            Image connIcon, Image connFill, Text connText)
        {
            oxygenIcon = o2Icon;
            oxygenFill = o2Fill;
            oxygenValueText = o2Text;

            composureIcon = cIcon;
            composureFill = cFill;
            composureValueText = cText;

            connectionIcon = connIcon;
            connectionFill = connFill;
            connectionValueText = connText;

            Initialize();
        }

        public void UpdateBars(ResourceState state)
        {
            if (state == null) return;

            targetOxygen = state.oxygen;
            targetComposure = state.composure;
            targetConnection = state.connection;
        }

        private void Update()
        {
            // Smooth numeric and fill interpolation
            float lerpSpeed = 12f * Time.deltaTime;

            displayedOxygen = Mathf.Lerp(displayedOxygen, targetOxygen, lerpSpeed);
            displayedComposure = Mathf.Lerp(displayedComposure, targetComposure, lerpSpeed);
            displayedConnection = Mathf.Lerp(displayedConnection, targetConnection, lerpSpeed);

            if (oxygenFill != null) oxygenFill.fillAmount = Mathf.Clamp01(displayedOxygen / 100f);
            if (composureFill != null) composureFill.fillAmount = Mathf.Clamp01(displayedComposure / 100f);
            if (connectionFill != null) connectionFill.fillAmount = Mathf.Clamp01(displayedConnection / 100f);

            if (oxygenValueText != null) oxygenValueText.text = $"{Mathf.RoundToInt(displayedOxygen)} / 100";
            if (composureValueText != null) composureValueText.text = $"{Mathf.RoundToInt(displayedComposure)} / 100";
            if (connectionValueText != null) connectionValueText.text = $"{Mathf.RoundToInt(displayedConnection)} / 100";
        }
    }
}
