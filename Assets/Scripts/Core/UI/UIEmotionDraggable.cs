using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CtrlHeart.Core.Data;
using CtrlHeart.Core.Visuals;

namespace CtrlHeart.Core.UI
{
    /// <summary>
    /// Draggable emotion unit supporting drag-and-drop onto mind map nodes.
    /// Master Design Bible Part 2 §2.2, Part 5 §5.10 & Dev Plan Day 2 Track A.
    /// </summary>
    public class UIEmotionDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public CoreEmotion emotionType;

        [SerializeField] private Image iconImage;
        [SerializeField] private Text labelText;

        private RectTransform rectTransform;
        private Canvas rootCanvas;
        private CanvasGroup canvasGroup;
        private Vector2 originalLocalPosition;
        private Transform originalParent;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
            rootCanvas = GetComponentInParent<Canvas>();
        }

        public void Initialize(CoreEmotion emotion)
        {
            emotionType = emotion;
            if (labelText != null) labelText.text = emotion.ToString();
            if (iconImage != null) iconImage.color = VisualTheme.GetEmotionColor(emotion);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            originalLocalPosition = rectTransform.localPosition;
            originalParent = transform.parent;
            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (rootCanvas == null) return;
            rectTransform.anchoredPosition += eventData.delta / rootCanvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;

            // Check if dropped onto a UINodeView
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                var nodeView = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<UINodeView>();
                if (nodeView != null)
                {
                    OnDroppedOnNode(nodeView.nodeType);
                }
            }

            // Return to tray
            rectTransform.SetParent(originalParent);
            rectTransform.localPosition = originalLocalPosition;
        }

        private void OnDroppedOnNode(InternalNodeType targetNode)
        {
            Debug.Log($"[DragDrop] Dropped {emotionType} onto {targetNode}");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.HandleEmotionDrop(targetNode, emotionType);
            }
        }
    }
}
