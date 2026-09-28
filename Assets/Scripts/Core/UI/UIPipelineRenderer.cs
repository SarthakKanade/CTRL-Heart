using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CtrlHeart.Core.Data;
using CtrlHeart.Core.Visuals;

namespace CtrlHeart.Core.UI
{
    /// <summary>
    /// Custom UI Graphic that draws glowing curved neural pipelines between all organ nodes.
    /// Creates the living bioluminescent circulatory network matching the reference UI.
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class UIPipelineRenderer : MaskableGraphic
    {
        [System.Serializable]
        public struct PipelineEdge
        {
            public InternalNodeType from;
            public InternalNodeType to;
            public Vector2 curveOffset;
        }

        private readonly List<PipelineEdge> edges = new List<PipelineEdge>
        {
            new PipelineEdge { from = InternalNodeType.Brain, to = InternalNodeType.Voice, curveOffset = new Vector2(-40, 20) },
            new PipelineEdge { from = InternalNodeType.Brain, to = InternalNodeType.Heart, curveOffset = new Vector2(40, 20) },
            new PipelineEdge { from = InternalNodeType.Voice, to = InternalNodeType.Body, curveOffset = new Vector2(-35, -20) },
            new PipelineEdge { from = InternalNodeType.Heart, to = InternalNodeType.Lungs, curveOffset = new Vector2(35, -20) },
            new PipelineEdge { from = InternalNodeType.Body, to = InternalNodeType.Lungs, curveOffset = new Vector2(0, -35) },
            new PipelineEdge { from = InternalNodeType.Voice, to = InternalNodeType.Heart, curveOffset = new Vector2(0, 30) },
            new PipelineEdge { from = InternalNodeType.Brain, to = InternalNodeType.Body, curveOffset = new Vector2(-75, -20) },
            new PipelineEdge { from = InternalNodeType.Brain, to = InternalNodeType.Lungs, curveOffset = new Vector2(75, -20) },
        };

        private readonly Dictionary<InternalNodeType, Vector2> nodeAnchors = new Dictionary<InternalNodeType, Vector2>();

        [SerializeField] private float pipeOuterWidth = 14f;
        [SerializeField] private float pipeInnerWidth = 5f;
        [SerializeField] private Color pipeOuterGlow = new Color(0.85f, 0.65f, 0.28f, 0.45f); // Warm Amber/Gold glow
        [SerializeField] private Color pipeInnerCore = new Color(1.0f, 0.88f, 0.50f, 0.85f);  // Luminous Antique Gold thread

        public void SetNodePositions(Dictionary<InternalNodeType, Vector2> positions)
        {
            nodeAnchors.Clear();
            foreach (var kvp in positions)
            {
                nodeAnchors[kvp.Key] = kvp.Value;
            }
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (nodeAnchors.Count < 5) return;

            foreach (var edge in edges)
            {
                if (!nodeAnchors.ContainsKey(edge.from) || !nodeAnchors.ContainsKey(edge.to)) continue;

                Vector2 p0 = nodeAnchors[edge.from];
                Vector2 p2 = nodeAnchors[edge.to];
                Vector2 p1 = (p0 + p2) * 0.5f + edge.curveOffset;

                // 1. Draw outer glowing translucent sheath
                DrawBezierRibbon(vh, p0, p1, p2, pipeOuterWidth, pipeOuterGlow);
                // 2. Draw inner bright luminescent core
                DrawBezierRibbon(vh, p0, p1, p2, pipeInnerWidth, pipeInnerCore);
            }
        }

        private void DrawBezierRibbon(VertexHelper vh, Vector2 p0, Vector2 p1, Vector2 p2, float width, Color col)
        {
            int segments = 16;
            float halfW = width * 0.5f;

            int startVertexIndex = vh.currentVertCount;

            for (int i = 0; i <= segments; i++)
            {
                float t = i / (float)segments;
                float u = 1f - t;

                // Quadratic Bezier point
                Vector2 pos = u * u * p0 + 2f * u * t * p1 + t * t * p2;

                // Tangent vector
                Vector2 tangent = 2f * u * (p1 - p0) + 2f * t * (p2 - p1);
                if (tangent.sqrMagnitude < 0.0001f) tangent = (p2 - p0);
                tangent.Normalize();

                // Normal vector perpendicular to tangent
                Vector2 normal = new Vector2(-tangent.y, tangent.x);

                Vector2 vLeft = pos + normal * halfW;
                Vector2 vRight = pos - normal * halfW;

                vh.AddVert(vLeft, col, new Vector2(0, t));
                vh.AddVert(vRight, col, new Vector2(1, t));

                if (i > 0)
                {
                    int currentLeft = startVertexIndex + i * 2;
                    int currentRight = currentLeft + 1;
                    int prevLeft = currentLeft - 2;
                    int prevRight = currentLeft - 1;

                    vh.AddTriangle(prevLeft, prevRight, currentRight);
                    vh.AddTriangle(prevLeft, currentRight, currentLeft);
                }
            }
        }
    }
}
