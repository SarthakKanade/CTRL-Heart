using System;
using System.Collections.Generic;
using UnityEngine;
using CtrlHeart.Core.Data;

namespace CtrlHeart.Core.Data
{
    /// <summary>
    /// Authored text, subtitles, and tone definitions for all 5 endings.
    /// Master Design Bible Part 3 §3.4 & Part 5 §5.8.
    /// </summary>
    public static class EndingsData
    {
        public struct EndingContent
        {
            public EndingType endingType;
            public string title;
            public string summary;
            public string dateFinalQuote;
            public Color bannerColor;
        }

        public static EndingContent GetEnding(EndingType type)
        {
            return type switch
            {
                EndingType.SecondDate_Tier3_4 => new EndingContent
                {
                    endingType = EndingType.SecondDate_Tier3_4,
                    title = "SECOND DATE CONFIRMED",
                    summary = "Your internal control room held it together. The conversation flowed naturally, emotional moments resonated, and a genuine spark was formed.",
                    dateFinalQuote = "\"I honestly had the best time today. Let's do this again soon—I'll text you tonight!\"",
                    bannerColor = new Color(0.2f, 0.85f, 0.4f, 1f)
                },
                EndingType.Maybe_Tier2 => new EndingContent
                {
                    endingType = EndingType.Maybe_Tier2,
                    title = "THE 'MAYBE' ZONE",
                    summary = "Not a disaster, not quite a fairy tale. You survived the nervous jitters, but kept your cards close to your chest.",
                    dateFinalQuote = "\"Thanks for coffee! It was really nice getting to know you. Take care!\"",
                    bannerColor = new Color(0.95f, 0.75f, 0.2f, 1f)
                },
                EndingType.AwkwardEnding_Tier1 => new EndingContent
                {
                    endingType = EndingType.AwkwardEnding_Tier1,
                    title = "AWKWARD ENDING",
                    summary = "Silence hung heavy, stumbles were frequent, and your control center struggled to keep thoughts connected to words.",
                    dateFinalQuote = "\"Well... I should probably get going before traffic starts. Safe travels home...\"",
                    bannerColor = new Color(0.9f, 0.45f, 0.2f, 1f)
                },
                EndingType.DateCollapse_Tier0 => new EndingContent
                {
                    endingType = EndingType.DateCollapse_Tier0,
                    title = "DATE COLLAPSED",
                    summary = "Connection dropped to absolute zero. The date detached early due to unbearable friction or detachment.",
                    dateFinalQuote = "\"You know what... I just remembered I have an emergency appointment. I have to leave now.\"",
                    bannerColor = new Color(0.85f, 0.2f, 0.2f, 1f)
                },
                EndingType.Meltdown_ComposureZero => new EndingContent
                {
                    endingType = EndingType.Meltdown_ComposureZero,
                    title = "INTERNAL MELTDOWN",
                    summary = "Composure reached 0. Heart rate spiked out of control, hands were shaking, and your nervous system pulled the emergency brake.",
                    dateFinalQuote = "\"Are you okay?! You look like you're about to pass out! Do you need an ambulance?!\"",
                    bannerColor = new Color(0.9f, 0.1f, 0.1f, 1f)
                },
                _ => new EndingContent()
            };
        }
    }
}
