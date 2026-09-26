using System;
using UnityEngine;

namespace CtrlHeart.Core.Data.ScriptableObjects
{
    /// <summary>
    /// Configuration data for one of the 5 internal nodes.
    /// </summary>
    [CreateAssetMenu(fileName = "NodeData_", menuName = "CTRL-Heart/Node Data")]
    public class NodeData : ScriptableObject
    {
        public InternalNodeType nodeType;
        public string displayName;
        [TextArea(2, 4)] public string description;
        public Sprite nodeIcon;
        public Color nodeColor = Color.white;

        [Header("Baseline Stats")]
        public float maxHealth = 100f;
        public float naturalDecayRate = 0f;
    }
}
