using System;
using System.Collections.Generic;
using UnityEngine;
using CtrlHeart.Core.Data;
using CtrlHeart.Core.Interfaces;

namespace CtrlHeart.Core.Systems
{
    /// <summary>
    /// Executes SocialEventProfile payloads across target nodes and resources.
    /// Handles Stress Event threats and Direct Emotion Push opportunities.
    /// Master Design Bible Part 4 §4.3 & Dev Plan Day 2 Track A.
    /// </summary>
    public class SocialEventEngine : MonoBehaviour, ISocialEventEngine
    {
        [Header("Dependencies")]
        [SerializeField] private MindMapManager mindMap;
        [SerializeField] private ResourceManager resources;

        public event Action<SocialEventProfile> OnSocialEventTriggered;

        public void Initialize(MindMapManager mindMapManager, ResourceManager resourceManager)
        {
            mindMap = mindMapManager;
            resources = resourceManager;
        }

        public void TriggerSocialEvent(SocialEventProfile profile)
        {
            if (profile == null) return;

            OnSocialEventTriggered?.Invoke(profile);

            if (profile.targetNodes == null || profile.targetNodes.Count == 0)
            {
                return;
            }

            foreach (var targetNode in profile.targetNodes)
            {
                ApplyEffectToNode(targetNode, profile);
            }
        }

        private void ApplyEffectToNode(InternalNodeType targetNode, SocialEventProfile profile)
        {
            if (profile.effectType == SocialEffectType.StressEvent)
            {
                ApplyStressSubtype(targetNode, profile.stressEventSubtype, profile.magnitude);
            }
            else if (profile.effectType == SocialEffectType.DirectEmotionPush)
            {
                ApplyDirectEmotionPush(targetNode, profile.pushTargetEmotion, profile.magnitude);
            }
        }

        private void ApplyStressSubtype(InternalNodeType node, StressEventSubtype subtype, float magnitude)
        {
            if (mindMap != null)
            {
                mindMap.ApplyDamage(node, magnitude);
            }

            switch (subtype)
            {
                case StressEventSubtype.Panic:
                    // Brain: ↓Focus, ↑Anxiety
                    if (resources != null) resources.ModifyFocus(-magnitude * 0.75f);
                    if (mindMap != null) mindMap.ApplyInfluence(node, CoreEmotion.Anxiety, magnitude);
                    break;

                case StressEventSubtype.HeartFlutter:
                    // Heart: ↑Composure pressure
                    if (resources != null) resources.ModifyComposure(-magnitude * 0.8f);
                    if (mindMap != null) mindMap.ApplyInfluence(node, CoreEmotion.Anxiety, magnitude * 0.5f);
                    break;

                case StressEventSubtype.AwkwardSilence:
                    // Voice: ↓Voice health, ↓Focus slightly
                    if (resources != null) resources.ModifyFocus(-magnitude * 0.4f);
                    break;

                case StressEventSubtype.Overthinking:
                    // Brain: Focus interference, destabilizes Composure
                    if (resources != null)
                    {
                        resources.ModifyFocus(-magnitude);
                        resources.ModifyComposure(-magnitude * 0.3f);
                    }
                    break;

                case StressEventSubtype.SweatSurge:
                    // Body: visible physical embarrassment, damages Body
                    if (mindMap != null) mindMap.ApplyDamage(InternalNodeType.Body, magnitude * 1.2f);
                    if (resources != null) resources.ModifyComposure(-magnitude * 0.5f);
                    break;
            }
        }

        private void ApplyDirectEmotionPush(InternalNodeType node, CoreEmotion emotion, float magnitude)
        {
            if (mindMap != null)
            {
                mindMap.ApplyInfluence(node, emotion, magnitude);

                // Positive emotions restore slight health to the node
                if (emotion == CoreEmotion.Calm || emotion == CoreEmotion.Attraction)
                {
                    mindMap.HealNode(node, magnitude * 0.5f);
                }
            }

            if (resources != null)
            {
                if (emotion == CoreEmotion.Calm)
                {
                    resources.ModifyComposure(magnitude * 0.5f);
                }
                else if (emotion == CoreEmotion.Confidence)
                {
                    resources.ModifyFocus(magnitude * 0.4f);
                }
            }
        }
    }
}
