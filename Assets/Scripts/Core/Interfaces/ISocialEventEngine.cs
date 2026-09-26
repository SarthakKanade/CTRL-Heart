using System;
using CtrlHeart.Core.Data;

namespace CtrlHeart.Core.Interfaces
{
    /// <summary>
    /// Executes SocialEventProfile payloads against internal nodes and resources.
    /// Master Design Bible Part 4 §4.3 & Dev Plan Day 2 Track A.
    /// </summary>
    public interface ISocialEventEngine
    {
        void TriggerSocialEvent(SocialEventProfile profile);
        event Action<SocialEventProfile> OnSocialEventTriggered;
    }
}
