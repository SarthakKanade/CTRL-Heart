using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Handles emotion button input — both click-to-target and drag-and-drop.
/// Attach this to each emotion button (Calm / Anxiety).
///
/// Click-to-target flow:
///   1. Click this button → emotion is "selected" (highlighted)
///   2. Click the Brain node → selected emotion is assigned
///
/// Drag-and-drop flow:
///   1. Press and drag this button → a ghost follows the cursor
///   2. Release over Brain → emotion is assigned
///   3. Release elsewhere → nothing happens
/// </summary>
public class EmotionDragger : MonoBehaviour, IPointerClickHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Config")]
    public EmotionType emotionType;

    /// <summary>True while a valid drag is in progress.</summary>
    public bool IsDragValid { get; private set; }

    // ── Private state ──────────────────────────────────────
    private GameObject dragGhost;
    private Canvas rootCanvas;

    // ── Unity Lifecycle ────────────────────────────────────

    void Start()
    {
        rootCanvas = GetComponentInParent<Canvas>();
    }

    // ── Click-to-Target ────────────────────────────────────

    public void OnPointerClick(PointerEventData eventData)
    {
        var game = Phase0Game.Instance;
        if (game == null) return;
        if (game.EmotionCooldown > 0f) return;
        if (game.State != Phase0Game.GameState.QuestionActive) return;

        game.SelectEmotion(emotionType);
    }

    // ── Drag-and-Drop ──────────────────────────────────────

    public void OnBeginDrag(PointerEventData eventData)
    {
        var game = Phase0Game.Instance;
        IsDragValid = false;

        if (game == null) return;
        if (game.EmotionCooldown > 0f) return;
        if (game.State != Phase0Game.GameState.QuestionActive) return;

        IsDragValid = true;

        // Clear any click-selection
        game.SelectEmotion(EmotionType.None);

        // Create semi-transparent ghost that follows the cursor
        dragGhost = new GameObject("EmotionDragGhost");
        dragGhost.transform.SetParent(rootCanvas.transform, false);

        var img = dragGhost.AddComponent<Image>();
        img.sprite = GetComponent<Image>().sprite;
        Color srcColor = GetComponent<Image>().color;
        img.color = new Color(srcColor.r, srcColor.g, srcColor.b, 0.65f);
        img.raycastTarget = false;

        var rt = dragGhost.GetComponent<RectTransform>();
        rt.sizeDelta = GetComponent<RectTransform>().sizeDelta * 0.7f;

        // CanvasGroup prevents the ghost from blocking raycasts
        // so OnDrop fires on the target underneath
        var cg = dragGhost.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false;
        cg.interactable = false;

        MoveGhostToPointer(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsDragValid || dragGhost == null) return;
        MoveGhostToPointer(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragGhost != null)
        {
            Destroy(dragGhost);
            dragGhost = null;
        }
        IsDragValid = false;
    }

    // ── Helpers ────────────────────────────────────────────

    private void MoveGhostToPointer(PointerEventData eventData)
    {
        if (dragGhost == null) return;

        if (rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            dragGhost.transform.position = eventData.position;
        }
        else
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rootCanvas.GetComponent<RectTransform>(),
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint);
            dragGhost.GetComponent<RectTransform>().anchoredPosition = localPoint;
        }
    }
}
