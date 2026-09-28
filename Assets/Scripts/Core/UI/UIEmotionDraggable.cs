using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CtrlHeart.Core.Data;
using CtrlHeart.Core.Visuals;

namespace CtrlHeart.Core.UI
{
    /// <summary>
    /// Emotion unit card in the left panel matching the reference UI:
    /// - Calm (water drop, cyan)
    /// - Anxiety (lightning bolt, pink/coral)
    /// - Confidence (star, golden yellow)
    /// - Attraction (heart, purple)
    /// Supports both drag-and-drop onto organ nodes AND click-to-select interaction.
    /// </summary>
    public class UIEmotionDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public CoreEmotion emotionType;

        [SerializeField] private Image cardBackgroundImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private Text labelText;
        [SerializeField] private Image influenceBarFill;
        [SerializeField] private Image selectionGlowRing;

        private RectTransform rectTransform;
        private Canvas rootCanvas;
        private CanvasGroup canvasGroup;
        private Vector2 originalAnchoredPosition;
        private Transform originalParent;
        private bool isDragging = false;

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
            Color themeColor = VisualTheme.GetEmotionColor(emotion);

            if (labelText != null)
            {
                labelText.text = emotion.ToString();
                labelText.color = Color.white;
            }

            if (iconImage != null)
            {
                string iconKey = emotion switch
                {
                    CoreEmotion.Calm => "icon_calm",
                    CoreEmotion.Anxiety => "icon_anxiety",
                    CoreEmotion.Confidence => "icon_confidence",
                    CoreEmotion.Attraction => "icon_attraction",
                    _ => "circle_glow"
                };
                iconImage.sprite = UIProceduralTextureGenerator.GetSprite(iconKey);
                iconImage.color = themeColor;
            }

            if (influenceBarFill != null)
            {
                influenceBarFill.sprite = UIProceduralTextureGenerator.GetSprite("bar_pill");
                influenceBarFill.color = themeColor;
                influenceBarFill.fillAmount = 0.45f;
            }

            if (cardBackgroundImage != null)
            {
                var s = UIProceduralTextureGenerator.GetSprite("rpg_button_long_brown");
                cardBackgroundImage.sprite = s != null ? s : UIProceduralTextureGenerator.GetSprite("panel_glass");
                cardBackgroundImage.type = Image.Type.Sliced;
                cardBackgroundImage.color = Color.white;
            }

            SetSelected(false);
        }

        public void AssignComponents(Image bg, Image icon, Text label, Image barFill, Image glow)
        {
            cardBackgroundImage = bg;
            iconImage = icon;
            labelText = label;
            influenceBarFill = barFill;
            selectionGlowRing = glow;
        }

        public void SetSelected(bool selected)
        {
            if (selectionGlowRing != null)
            {
                selectionGlowRing.gameObject.SetActive(selected);
                if (selected)
                {
                    selectionGlowRing.color = VisualTheme.GetEmotionColor(emotionType);
                }
            }
        }

        public void UpdateInfluenceLevel(float fillPercent)
        {
            if (influenceBarFill != null)
            {
                influenceBarFill.fillAmount = Mathf.Clamp01(fillPercent);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isDragging) return;
            CoreGameUI.SelectEmotion(emotionType);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
            originalAnchoredPosition = rectTransform.anchoredPosition;
            originalParent = transform.parent;
            canvasGroup.blocksRaycasts = false;
            CoreGameUI.SelectEmotion(emotionType);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (rootCanvas == null) return;
            rectTransform.anchoredPosition += eventData.delta / rootCanvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
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

            // Snap back to tray
            rectTransform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalAnchoredPosition;
        }

        private void OnDroppedOnNode(InternalNodeType targetNode)
        {
            Debug.Log($"[DragDrop] Injected {emotionType} into {targetNode}");
            GameManager.Instance?.HandleEmotionDrop(targetNode, emotionType);
        }
    }
}
