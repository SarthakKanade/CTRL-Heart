using System;
using System.Collections.Generic;
using UnityEngine;
using CtrlHeart.Core.Data;
using CtrlHeart.Core.Interfaces;

namespace CtrlHeart.Core.Systems
{
    /// <summary>
    /// Manages the 5 internal nodes, tracking health and emotional influence.
    /// Evaluates dominant emotion state using locked mixture rules.
    /// Master Design Bible Part 2 §2.3, §2.4 & Dev Plan Day 2 Track A.
    /// </summary>
    public class MindMapManager : MonoBehaviour, IMindMapManager
    {
        [Header("Node Settings")]
        [SerializeField] private float bodyCriticalThreshold = 25f;

        private readonly Dictionary<InternalNodeType, NodeState> nodes = new Dictionary<InternalNodeType, NodeState>();

        public event Action<InternalNodeType, NodeState> OnNodeChanged;

        private void Awake()
        {
            InitializeNodes();
        }

        public void InitializeNodes()
        {
            nodes.Clear();
            foreach (InternalNodeType type in Enum.GetValues(typeof(InternalNodeType)))
            {
                nodes[type] = new NodeState(type);
            }
        }

        public NodeState GetNode(InternalNodeType type)
        {
            return nodes.TryGetValue(type, out var state) ? state : null;
        }

        public EmotionState GetDominantState(InternalNodeType type)
        {
            var node = GetNode(type);
            return SimulationMath.EvaluateDominantState(node);
        }

        public void ApplyInfluence(InternalNodeType targetNode, CoreEmotion emotion, float deltaPercent)
        {
            var node = GetNode(targetNode);
            if (node == null) return;

            float current = node.GetInfluence(emotion);
            node.SetInfluence(emotion, current + deltaPercent);

            // Active player injection displaces competing emotions
            if (deltaPercent >= 10f)
            {
                foreach (CoreEmotion other in Enum.GetValues(typeof(CoreEmotion)))
                {
                    if (other != emotion)
                    {
                        float otherVal = node.GetInfluence(other);
                        node.SetInfluence(other, otherVal * 0.65f);
                    }
                }
            }

            NormalizeInfluences(node);
            OnNodeChanged?.Invoke(targetNode, node);
        }

        public void ApplyContinuousDrift(InternalNodeType targetNode, CoreEmotion emotion, float rate)
        {
            var node = GetNode(targetNode);
            if (node == null) return;

            float current = node.GetInfluence(emotion);
            node.SetInfluence(emotion, current + rate);
            NormalizeInfluences(node);
            OnNodeChanged?.Invoke(targetNode, node);
        }

        public void ApplyDamage(InternalNodeType targetNode, float damage)
        {
            var node = GetNode(targetNode);
            if (node == null) return;

            node.currentHealth = Mathf.Clamp(node.currentHealth - damage, 0f, NodeState.MAX_HEALTH);
            OnNodeChanged?.Invoke(targetNode, node);
        }

        public void HealNode(InternalNodeType targetNode, float amount)
        {
            var node = GetNode(targetNode);
            if (node == null) return;

            node.currentHealth = Mathf.Clamp(node.currentHealth + amount, 0f, NodeState.MAX_HEALTH);
            OnNodeChanged?.Invoke(targetNode, node);
        }

        public bool IsBodyCriticallyLow()
        {
            var body = GetNode(InternalNodeType.Body);
            return body != null && body.currentHealth <= bodyCriticalThreshold;
        }

        public void DecayInfluences(float amount)
        {
            foreach (var kvp in nodes)
            {
                kvp.Value.DecayInfluences(amount);
                OnNodeChanged?.Invoke(kvp.Key, kvp.Value);
            }
        }

        public void SettleInfluencesBetweenSlots(float factor = 0.50f)
        {
            foreach (var kvp in nodes)
            {
                var n = kvp.Value;
                n.calmInfluence *= factor;
                n.anxietyInfluence *= factor;
                n.confidenceInfluence *= factor;
                n.attractionInfluence *= factor;
                NormalizeInfluences(n);
                OnNodeChanged?.Invoke(kvp.Key, n);
            }
        }

        private void NormalizeInfluences(NodeState node)
        {
            float total = node.calmInfluence + node.anxietyInfluence + node.confidenceInfluence + node.attractionInfluence;
            if (total <= 0.01f)
            {
                node.calmInfluence = 0f;
                node.anxietyInfluence = 0f;
                node.confidenceInfluence = 0f;
                node.attractionInfluence = 0f;
                return;
            }

            // Keep normalized to 100% total
            node.calmInfluence = (node.calmInfluence / total) * 100f;
            node.anxietyInfluence = (node.anxietyInfluence / total) * 100f;
            node.confidenceInfluence = (node.confidenceInfluence / total) * 100f;
            node.attractionInfluence = (node.attractionInfluence / total) * 100f;
        }

        public float CalculateEquilibriumScore()
        {
            if (nodes.Count == 0) return 50f;

            float avgHealth = 0f;
            float calmTotal = 0f;
            float anxietyTotal = 0f;

            foreach (var kvp in nodes)
            {
                avgHealth += kvp.Value.currentHealth;
                calmTotal += kvp.Value.calmInfluence;
                anxietyTotal += kvp.Value.anxietyInfluence;
            }

            avgHealth /= nodes.Count;

            // Health variance across nodes (imbalance penalty)
            float variance = 0f;
            foreach (var kvp in nodes)
            {
                float diff = kvp.Value.currentHealth - avgHealth;
                variance += diff * diff;
            }
            variance /= nodes.Count;
            float stdDev = Mathf.Sqrt(variance);

            float healthScore = Mathf.Clamp01((avgHealth - stdDev * 0.75f) / 100f);
            float calmRatio = (calmTotal + 15f) / (calmTotal + anxietyTotal + 30f);

            float equilibrium = (healthScore * 0.65f + calmRatio * 0.35f) * 100f;
            return Mathf.Clamp(equilibrium, 0f, 100f);
        }
    }
}
