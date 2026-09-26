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

        private void NormalizeInfluences(NodeState node)
        {
            float total = node.calmInfluence + node.anxietyInfluence + node.confidenceInfluence + node.attractionInfluence;
            if (total <= 0.01f)
            {
                node.calmInfluence = 25f;
                node.anxietyInfluence = 25f;
                node.confidenceInfluence = 25f;
                node.attractionInfluence = 25f;
                return;
            }

            // Keep normalized to 100% total
            node.calmInfluence = (node.calmInfluence / total) * 100f;
            node.anxietyInfluence = (node.anxietyInfluence / total) * 100f;
            node.confidenceInfluence = (node.confidenceInfluence / total) * 100f;
            node.attractionInfluence = (node.attractionInfluence / total) * 100f;
        }
    }
}
