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
        private readonly List<TargetedEffect> activeEffects = new List<TargetedEffect>();
        private readonly Dictionary<InternalNodeType, float> suppressionTimers = new Dictionary<InternalNodeType, float>();

        public event Action<SocialEventProfile> OnSocialEventTriggered;

        public void Initialize(MindMapManager mindMapManager, ResourceManager resourceManager)
        {
            mindMap = mindMapManager;
            resources = resourceManager;
        }

        public void TriggerSocialEvent(SocialEventProfile profile)
        {
            activeEffects.Clear();
            suppressionTimers.Clear();

            if (profile == null) return;

            OnSocialEventTriggered?.Invoke(profile);

            if (profile.effects == null || profile.effects.Count == 0)
            {
                // Zero-disruption recovery beat (e.g., Slot 6 T2)
                return;
            }

            activeEffects.AddRange(profile.effects);

            foreach (var effect in profile.effects)
            {
                if (effect == null) continue;
                ApplyInitialEffectToNode(effect);
            }
        }

        public void TickActiveSocialEvent(float deltaTime)
        {
            if (activeEffects.Count == 0) return;

            // Update suppression countdowns
            if (suppressionTimers.Count > 0)
            {
                var keys = new List<InternalNodeType>(suppressionTimers.Keys);
                foreach (var k in keys)
                {
                    suppressionTimers[k] -= deltaTime;
                    if (suppressionTimers[k] <= 0f)
                    {
                        suppressionTimers.Remove(k);
                    }
                }
            }

            foreach (var effect in activeEffects)
            {
                if (effect == null) continue;
                if (suppressionTimers.ContainsKey(effect.targetNode)) continue;

                if (effect.effectType == SocialEffectType.StressEvent)
                {
                    // Continuous damage to node
                    if (mindMap != null)
                    {
                        mindMap.ApplyDamage(effect.targetNode, effect.magnitude * 0.08f * deltaTime);
                        mindMap.ApplyContinuousDrift(effect.targetNode, CoreEmotion.Anxiety, effect.magnitude * 0.04f * deltaTime);
                    }

                    // Continuous resource pressure (Balanced tension: ~1.5 to 2.5/sec instead of instant exhaustion)
                    if (resources != null)
                    {
                        switch (effect.stressEventSubtype)
                        {
                            case StressEventSubtype.Panic:
                                resources.ModifyComposure(-effect.magnitude * 0.07f * deltaTime);
                                break;
                            case StressEventSubtype.HeartFlutter:
                                resources.ModifyComposure(-effect.magnitude * 0.08f * deltaTime);
                                break;
                            case StressEventSubtype.SweatSurge:
                                mindMap?.ApplyDamage(InternalNodeType.Body, effect.magnitude * 0.15f * deltaTime);
                                resources.ModifyComposure(-effect.magnitude * 0.05f * deltaTime);
                                break;
                            case StressEventSubtype.Overthinking:
                                resources.ModifyComposure(-effect.magnitude * 0.06f * deltaTime);
                                break;
                            case StressEventSubtype.AwkwardSilence:
                                mindMap?.ApplyDamage(InternalNodeType.Voice, effect.magnitude * 0.15f * deltaTime);
                                resources.ModifyComposure(-effect.magnitude * 0.05f * deltaTime);
                                break;
                        }
                    }
                }
            }
        }

        public void OnPlayerIntervene(InternalNodeType node, CoreEmotion emotion)
        {
            // Player unit deployment suppresses active threat on this node for 10 seconds
            suppressionTimers[node] = 10f;

            if (emotion == CoreEmotion.Calm)
            {
                mindMap?.HealNode(node, 20f);
                mindMap?.ApplyInfluence(node, CoreEmotion.Calm, 45f);
                resources?.ModifyComposure(+8f);
            }
            else if (emotion == CoreEmotion.Anxiety)
            {
                mindMap?.ApplyInfluence(node, CoreEmotion.Anxiety, 45f);
                resources?.ModifyComposure(-3f); // raises nervous tension
            }
            else if (emotion == CoreEmotion.Confidence)
            {
                mindMap?.ApplyInfluence(node, CoreEmotion.Confidence, 45f);
                resources?.ModifyComposure(+8f); // Restores resolve & steady composure
                if (node == InternalNodeType.Voice || node == InternalNodeType.Brain)
                {
                    mindMap?.HealNode(node, 12f);
                }
            }
            else if (emotion == CoreEmotion.Attraction)
            {
                mindMap?.ApplyInfluence(node, CoreEmotion.Attraction, 45f);
                resources?.ModifyComposure(+6f); // Romantic warmth
                if (node == InternalNodeType.Heart || node == InternalNodeType.Body)
                {
                    mindMap?.HealNode(node, 12f);
                }
            }
        }

        public bool IsNodeSuppressed(InternalNodeType node) => suppressionTimers.ContainsKey(node);
        public float GetSuppressionRemaining(InternalNodeType node) => suppressionTimers.TryGetValue(node, out float t) ? t : 0f;

        public void StopSocialEvent()
        {
            activeEffects.Clear();
            suppressionTimers.Clear();
        }

        private void ApplyInitialEffectToNode(TargetedEffect effect)
        {
            if (effect.effectType == SocialEffectType.StressEvent)
            {
                ApplyStressSubtype(effect.targetNode, effect.stressEventSubtype, effect.magnitude);
            }
            else if (effect.effectType == SocialEffectType.DirectEmotionPush)
            {
                ApplyDirectEmotionPush(effect.targetNode, effect.pushTargetEmotion, effect.magnitude);
            }
        }

        private void ApplyStressSubtype(InternalNodeType node, StressEventSubtype subtype, float magnitude)
        {
            float damage = magnitude * 0.5f;
            if (mindMap != null)
            {
                mindMap.ApplyDamage(node, damage);
            }

            float initialInfluence = Mathf.Max(35f, magnitude * 2.5f);

            switch (subtype)
            {
                case StressEventSubtype.Panic:
                    // Brain: ↑Anxiety, ↓Composure flutter
                    if (resources != null) resources.ModifyComposure(-magnitude * 0.12f);
                    if (mindMap != null) mindMap.ApplyInfluence(node, CoreEmotion.Anxiety, initialInfluence);
                    break;

                case StressEventSubtype.HeartFlutter:
                    // Heart: Heart skip
                    if (resources != null) resources.ModifyComposure(-magnitude * 0.16f);
                    if (mindMap != null) mindMap.ApplyInfluence(node, CoreEmotion.Anxiety, initialInfluence);
                    break;

                case StressEventSubtype.AwkwardSilence:
                    // Voice: Voice catch
                    if (resources != null) resources.ModifyComposure(-magnitude * 0.10f);
                    if (mindMap != null) mindMap.ApplyInfluence(node, CoreEmotion.Anxiety, initialInfluence);
                    break;

                case StressEventSubtype.Overthinking:
                    // Brain: Racing thoughts
                    if (resources != null) resources.ModifyComposure(-magnitude * 0.12f);
                    if (mindMap != null) mindMap.ApplyInfluence(node, CoreEmotion.Anxiety, initialInfluence);
                    break;

                case StressEventSubtype.SweatSurge:
                    // Body: physical tension
                    if (mindMap != null)
                    {
                        mindMap.ApplyDamage(InternalNodeType.Body, magnitude * 0.5f);
                        mindMap.ApplyInfluence(node, CoreEmotion.Anxiety, initialInfluence);
                    }
                    if (resources != null) resources.ModifyComposure(-magnitude * 0.10f);
                    break;
            }
        }

        private void ApplyDirectEmotionPush(InternalNodeType node, CoreEmotion emotion, float magnitude)
        {
            float initialInfluence = Mathf.Max(35f, magnitude * 2.5f);
            if (mindMap != null)
            {
                mindMap.ApplyInfluence(node, emotion, initialInfluence);

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
                    resources.ModifyComposure(magnitude * 0.35f);
                }
            }
        }
    }
}
