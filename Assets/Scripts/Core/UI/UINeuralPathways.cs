using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CtrlHeart.Core.Data;
using CtrlHeart.Core.Visuals;

namespace CtrlHeart.Core.UI
{
    /// <summary>
    /// Renders the glowing neural pipelines connecting the 5 internal organ nodes
    /// and animates cute traveling impulse cells that flow across the biological network.
    /// Supports launching targeted energetic impulses when emotions are deployed.
    /// </summary>
    public class UINeuralPathways : MonoBehaviour
    {
        [System.Serializable]
        public struct PathwayConnection
        {
            public InternalNodeType from;
            public InternalNodeType to;
            public Vector2 curveOffset;
        }

        private readonly List<PathwayConnection> pathways = new List<PathwayConnection>
        {
            new PathwayConnection { from = InternalNodeType.Brain, to = InternalNodeType.Voice, curveOffset = new Vector2(-40, 20) },
            new PathwayConnection { from = InternalNodeType.Brain, to = InternalNodeType.Heart, curveOffset = new Vector2(40, 20) },
            new PathwayConnection { from = InternalNodeType.Voice, to = InternalNodeType.Body, curveOffset = new Vector2(-35, -20) },
            new PathwayConnection { from = InternalNodeType.Heart, to = InternalNodeType.Lungs, curveOffset = new Vector2(35, -20) },
            new PathwayConnection { from = InternalNodeType.Body, to = InternalNodeType.Lungs, curveOffset = new Vector2(0, -35) },
            new PathwayConnection { from = InternalNodeType.Voice, to = InternalNodeType.Heart, curveOffset = new Vector2(0, 30) },
            new PathwayConnection { from = InternalNodeType.Brain, to = InternalNodeType.Body, curveOffset = new Vector2(-75, -20) },
            new PathwayConnection { from = InternalNodeType.Brain, to = InternalNodeType.Lungs, curveOffset = new Vector2(75, -20) },
        };

        private readonly Dictionary<InternalNodeType, Vector2> nodeAnchors = new Dictionary<InternalNodeType, Vector2>();
        private readonly List<ImpulseRunner> activeRunners = new List<ImpulseRunner>();

        private Transform impulseContainer;
        private UIPipelineRenderer pipelineRenderer;
        private Sprite cellSprite;

        private class ImpulseRunner
        {
            public RectTransform rect;
            public Image image;
            public Vector2 start;
            public Vector2 control;
            public Vector2 end;
            public float progress;
            public float speed;
        }

        private void Start()
        {
            if (nodeAnchors.Count < 5)
            {
                AutoFindAndInitialize();
            }
        }

        public void AutoFindAndInitialize()
        {
            var dict = new Dictionary<InternalNodeType, Vector2>();
            if (transform.parent != null)
            {
                var views = transform.parent.GetComponentsInChildren<UINodeView>(true);
                foreach (var v in views)
                {
                    dict[v.nodeType] = (Vector2)v.RectTransform.localPosition;
                }
            }

            if (dict.Count >= 5)
            {
                Initialize(dict);
            }
        }

        public void Initialize(Dictionary<InternalNodeType, Vector2> positions)
        {
            nodeAnchors.Clear();
            foreach (var kvp in positions)
            {
                nodeAnchors[kvp.Key] = kvp.Value;
            }

            // Create or configure pipeline renderer
            if (pipelineRenderer == null)
            {
                var pipeGO = new GameObject("PipelineMesh");
                pipeGO.transform.SetParent(transform, false);
                var pRect = pipeGO.AddComponent<RectTransform>();
                pRect.anchorMin = Vector2.zero;
                pRect.anchorMax = Vector2.one;
                pRect.offsetMin = Vector2.zero;
                pRect.offsetMax = Vector2.zero;
                pipelineRenderer = pipeGO.AddComponent<UIPipelineRenderer>();
            }
            pipelineRenderer.SetNodePositions(positions);

            cellSprite = UIProceduralTextureGenerator.GetSprite("cell_impulse");

            if (impulseContainer == null)
            {
                var runnerParentGO = new GameObject("ImpulseContainer");
                runnerParentGO.transform.SetParent(transform, false);
                var rt = runnerParentGO.AddComponent<RectTransform>();
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                impulseContainer = runnerParentGO.transform;
            }

            // Spawn ambient travelling impulse cells
            SpawnAmbientImpulses();
        }

        private void SpawnAmbientImpulses()
        {
            Color[] cellColors = new Color[]
            {
                VisualTheme.ColorCalm,
                VisualTheme.ColorAnxiety,
                VisualTheme.ColorConfidence,
                VisualTheme.ColorAttraction,
                VisualTheme.ColorNodeBody
            };

            for (int i = 0; i < pathways.Count; i++)
            {
                var path = pathways[i];
                if (!nodeAnchors.ContainsKey(path.from) || !nodeAnchors.ContainsKey(path.to)) continue;

                var go = new GameObject($"Impulse_{i}");
                go.transform.SetParent(impulseContainer, false);

                var rt = go.AddComponent<RectTransform>();
                rt.sizeDelta = new Vector2(28, 28);

                var img = go.AddComponent<Image>();
                img.sprite = cellSprite;
                img.color = cellColors[i % cellColors.Length];
                img.raycastTarget = false;

                Vector2 p0 = nodeAnchors[path.from];
                Vector2 p2 = nodeAnchors[path.to];
                Vector2 p1 = (p0 + p2) * 0.5f + path.curveOffset;

                activeRunners.Add(new ImpulseRunner
                {
                    rect = rt,
                    image = img,
                    start = p0,
                    control = p1,
                    end = p2,
                    progress = (i * 0.23f) % 1.0f,
                    speed = 0.18f + (i % 3) * 0.04f
                });
            }
        }

        private void Update()
        {
            foreach (var runner in activeRunners)
            {
                runner.progress += Time.deltaTime * runner.speed;
                if (runner.progress > 1f)
                {
                    runner.progress -= 1f;
                }

                // Quadratic Bezier interpolation
                float t = runner.progress;
                float u = 1f - t;
                Vector2 pos = u * u * runner.start + 2f * u * t * runner.control + t * t * runner.end;

                runner.rect.anchoredPosition = pos;

                // Subtle organic pulse scale
                float scale = 0.85f + Mathf.Sin(Time.time * 6f + runner.progress * 10f) * 0.2f;
                runner.rect.localScale = Vector3.one * scale;
            }
        }

        public void LaunchTargetedImpulse(InternalNodeType targetNode, CoreEmotion emotion)
        {
            if (!nodeAnchors.ContainsKey(targetNode)) return;
            StartCoroutine(AnimateTargetedImpulse(targetNode, emotion));
        }

        private IEnumerator AnimateTargetedImpulse(InternalNodeType targetNode, CoreEmotion emotion)
        {
            var go = new GameObject("BurstImpulse");
            go.transform.SetParent(impulseContainer != null ? impulseContainer : transform, false);

            var rt = go.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(40, 40);

            var img = go.AddComponent<Image>();
            img.sprite = cellSprite;
            img.color = VisualTheme.GetEmotionColor(emotion);
            img.raycastTarget = false;

            Vector2 startPos = new Vector2(0f, -280f);
            Vector2 targetPos = nodeAnchors[targetNode];
            Vector2 midControl = (startPos + targetPos) * 0.5f + new Vector2(Random.Range(-50f, 50f), Random.Range(-20f, 40f));

            float duration = 0.65f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float ease = Mathf.SmoothStep(0f, 1f, t);

                float u = 1f - ease;
                rt.anchoredPosition = u * u * startPos + 2f * u * ease * midControl + ease * ease * targetPos;

                float scale = 1.0f + Mathf.Sin(t * Mathf.PI) * 0.6f;
                rt.localScale = Vector3.one * scale;

                yield return null;
            }

            Destroy(go);
        }
    }
}
