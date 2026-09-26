using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// The four emotion types the player can command.
/// Phase 0 only uses Calm and Anxiety.
/// </summary>
public enum EmotionType { None, Calm, Anxiety }

/// <summary>
/// The Brain node — the single internal node in the Phase 0 vertical slice.
/// Tracks health, emotional influence weights, and stress state.
/// Also serves as the UI drop/click target for emotion assignment.
/// </summary>
public class BrainNode : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    // ── Health ──────────────────────────────────────────────
    [Header("Health")]
    public float health = 80f;
    public float maxHealth = 100f;

    // ── Emotion Weights (0–1 each) ─────────────────────────
    [Header("Emotion Influence")]
    public float calmWeight = 0f;
    public float anxietyWeight = 0f;

    // ── Active Emotion Effect ──────────────────────────────
    [Header("Active Effect")]
    public bool isReceivingCalm = false;
    public bool isReceivingAnxiety = false;
    public float effectTimer = 0f;

    // ── Stress ─────────────────────────────────────────────
    [Header("Stress")]
    public bool isUnderStress = false;

    // ── Properties ─────────────────────────────────────────

    public float HealthPercent => health / maxHealth;

    public EmotionType DominantEmotion
    {
        get
        {
            if (calmWeight > 0.15f && calmWeight > anxietyWeight) return EmotionType.Calm;
            if (anxietyWeight > 0.15f && anxietyWeight > calmWeight) return EmotionType.Anxiety;
            return EmotionType.None;
        }
    }

    public bool IsReceivingAnyEmotion => isReceivingCalm || isReceivingAnxiety;

    // ── Public Methods ─────────────────────────────────────

    /// <summary>
    /// Apply Calm to the Brain. Slow, healing, stabilising.
    /// Calm influence lasts 2.5 seconds.
    /// </summary>
    public void ApplyCalm()
    {
        isReceivingCalm = true;
        isReceivingAnxiety = false;
        effectTimer = 2.5f;
    }

    /// <summary>
    /// Apply Anxiety to the Brain. Fast, boosts Focus instantly, but damages Brain.
    /// Anxiety influence lasts 1.5 seconds.
    /// </summary>
    public void ApplyAnxiety()
    {
        isReceivingAnxiety = true;
        isReceivingCalm = false;
        effectTimer = 1.5f;
    }

    /// <summary>Toggle stress state (called by Phase0Game when Panic events fire).</summary>
    public void SetStress(bool active)
    {
        isUnderStress = active;
    }

    /// <summary>
    /// Partially recover between questions so the player isn't doomed
    /// by one bad round.
    /// </summary>
    public void PartialReset()
    {
        health = Mathf.Min(health + 25f, maxHealth);
        calmWeight *= 0.3f;
        anxietyWeight *= 0.3f;
        isReceivingCalm = false;
        isReceivingAnxiety = false;
        isUnderStress = false;
        effectTimer = 0f;
    }

    // ── Tick ────────────────────────────────────────────────

    void Update()
    {
        float dt = Time.deltaTime;

        // ── Calm effect ───────────────────────────────
        if (isReceivingCalm && effectTimer > 0f)
        {
            calmWeight = Mathf.Min(calmWeight + 0.4f * dt, 1f);
            anxietyWeight = Mathf.Max(anxietyWeight - 0.25f * dt, 0f);
            health = Mathf.Min(health + 4f * dt, maxHealth);   // Heal
            effectTimer -= dt;
            if (effectTimer <= 0f) isReceivingCalm = false;
        }

        // ── Anxiety effect ────────────────────────────
        if (isReceivingAnxiety && effectTimer > 0f)
        {
            anxietyWeight = Mathf.Min(anxietyWeight + 0.8f * dt, 1f);
            calmWeight = Mathf.Max(calmWeight - 0.3f * dt, 0f);
            health = Mathf.Max(health - 2f * dt, 0f);          // Damage
            effectTimer -= dt;
            if (effectTimer <= 0f) isReceivingAnxiety = false;
        }

        // ── Stress damage ─────────────────────────────
        if (isUnderStress)
        {
            float damage = 6f;                      // Base panic damage / sec
            if (isReceivingCalm)    damage = 2f;    // Calm mitigates
            if (isReceivingAnxiety) damage = 9f;    // Anxiety amplifies
            health = Mathf.Max(health - damage * dt, 0f);
        }

        // ── Natural weight decay ──────────────────────
        if (!isReceivingCalm)
            calmWeight = Mathf.Max(calmWeight - 0.08f * dt, 0f);
        if (!isReceivingAnxiety)
            anxietyWeight = Mathf.Max(anxietyWeight - 0.12f * dt, 0f);
    }

    // ── UI Event Handlers ──────────────────────────────────

    /// <summary>Receives a dragged emotion dropped onto the Brain circle.</summary>
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        var dragger = eventData.pointerDrag.GetComponent<EmotionDragger>();
        if (dragger != null && dragger.IsDragValid)
        {
            Phase0Game.Instance.AssignEmotionToBrain(dragger.emotionType);
        }
    }

    /// <summary>Click-to-target: assigns the currently selected emotion.</summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        Phase0Game.Instance.TryAssignSelectedEmotion();
    }
}
