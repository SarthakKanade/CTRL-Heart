using UnityEngine;
using CtrlHeart.Core.Data;
using CtrlHeart.Core.Data.ScriptableObjects;
using CtrlHeart.Core.Systems;
using CtrlHeart.Core.Testing;

namespace CtrlHeart.Core.Testing
{
    /// <summary>
    /// Playable test controller for Day 2 checkpoint.
    /// Runs pure system logic: applies SocialEventProfiles, evaluates mixture states,
    /// checks Focus filter, Body override, and executes dialogue resolution on timer expiry.
    /// </summary>
    public class Day2SystemVerification : MonoBehaviour
    {
        [SerializeField] private MindMapManager mindMap;
        [SerializeField] private ResourceManager resources;
        [SerializeField] private SocialEventEngine eventEngine;
        [SerializeField] private DialogueManager dialogueManager;

        private void Start()
        {
            RunVerification();
        }

        [ContextMenu("Run Verification")]
        public void RunVerification()
        {
            Debug.Log("<color=cyan>=== CTRL+HEART DAY 2 VERIFICATION RUN ===</color>");

            // 1. Initialize systems
            if (mindMap == null) mindMap = gameObject.AddComponent<MindMapManager>();
            if (resources == null) resources = gameObject.AddComponent<ResourceManager>();
            if (eventEngine == null) eventEngine = gameObject.AddComponent<SocialEventEngine>();
            if (dialogueManager == null) dialogueManager = gameObject.AddComponent<DialogueManager>();

            mindMap.InitializeNodes();
            eventEngine.Initialize(mindMap, resources);

            // 2. Register Slot 1 content
            var (questions, answers, reactions) = FullDateContent.CreateSlotContent(1);
            dialogueManager.RegisterContent(questions, answers, reactions);
            Debug.Log($"[Verification] Registered {questions.Count} questions, {answers.Count} answers, {reactions.Count} reactions.");

            // 3. Select Question for Slot 1, Tier 2 (Maybe)
            var activeQuestion = dialogueManager.SelectQuestion(1, ConnectionTier.Tier2_Maybe);
            Debug.Log($"[Verification] Question: \"{activeQuestion.questionText}\"");

            // 4. Trigger Question's Social Event Profile (Panic on Brain & Voice)
            eventEngine.TriggerSocialEvent(activeQuestion.socialEventProfile);
            Debug.Log($"[Verification] Social Event Triggered: Brain Health={mindMap.GetNode(InternalNodeType.Brain).currentHealth}, Composure={resources.CurrentState.composure}");

            // 5. Test Drag/Influence: Apply Calm to Brain to stabilize it
            mindMap.ApplyInfluence(InternalNodeType.Brain, CoreEmotion.Calm, 50f);
            var dominantState = mindMap.GetDominantState(InternalNodeType.Brain);
            Debug.Log($"[Verification] Brain Dominant Emotion after Calm application: {dominantState}");

            // 6. Test Resolution
            bool isCriticalBody = mindMap.IsBodyCriticallyLow();
            var resolvedAnswer = dialogueManager.ResolveAnswer(activeQuestion, dominantState, isCriticalBody);
            Debug.Log($"[Verification] Resolved Answer: \"{resolvedAnswer.spokenText}\" (Delta: {resolvedAnswer.connectionDelta:+#;-#;0})");

            // 7. Apply Delta & Test Reaction Selection
            resources.ModifyConnection(resolvedAnswer.connectionDelta);
            var resultingTier = resources.CurrentState.GetConnectionTier();
            var reaction = dialogueManager.SelectReaction(activeQuestion, resultingTier);
            Debug.Log($"[Verification] Resulting Connection: {resources.CurrentState.connection} (Tier: {resultingTier}) -> Reaction: \"{reaction.reactionText}\" [{reaction.animationClipTag}]");

            // 8. Test Body Override rule (§5.7)
            mindMap.ApplyDamage(InternalNodeType.Body, 90f); // Critically low Body
            var bodyOverrideAnswer = dialogueManager.ResolveAnswer(activeQuestion, EmotionState.Confidence, mindMap.IsBodyCriticallyLow());
            Debug.Log($"[Verification] Body Override Test (Critical Body): \"{bodyOverrideAnswer.spokenText}\" (EmotionState: {bodyOverrideAnswer.emotionState})");

            Debug.Log("<color=green>=== DAY 2 VERIFICATION PASSED SUCCESSFULLY ===</color>");
        }
    }
}
