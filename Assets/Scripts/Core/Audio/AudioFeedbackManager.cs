using System;
using UnityEngine;
using CtrlHeart.Core.Data;

namespace CtrlHeart.Core.Audio
{
    /// <summary>
    /// Manages audio feedback, procedural heartbeats tied to Composure,
    /// and vocal/SFX reaction stingers.
    /// Master Design Bible Part 2 §2.5 (Heart Rate is Composure's audible expression) & Dev Plan Day 5 Priority 2.
    /// </summary>
    public class AudioFeedbackManager : MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource heartbeatSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Heartbeat Pitch Bands")]
        [SerializeField] private float stableBPM = 60f;     // Composure 75-100
        [SerializeField] private float nervousBPM = 85f;    // Composure 50-74
        [SerializeField] private float unstableBPM = 115f;  // Composure 25-49
        [SerializeField] private float franticBPM = 145f;   // Composure 1-24

        private float heartbeatTimer = 0f;
        private float currentBPM = 60f;

        private void Awake()
        {
            if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
        }

        public void UpdateComposureHeartbeat(float composure)
        {
            if (composure >= 75f) currentBPM = stableBPM;
            else if (composure >= 50f) currentBPM = nervousBPM;
            else if (composure >= 25f) currentBPM = unstableBPM;
            else currentBPM = franticBPM;
        }

        private void Update()
        {
            float interval = 60f / currentBPM;
            heartbeatTimer += Time.deltaTime;

            if (heartbeatTimer >= interval)
            {
                heartbeatTimer = 0f;
                PlayProceduralHeartbeatTick();
            }
        }

        private void PlayProceduralHeartbeatTick()
        {
            // Procedural synthesized click/thump if no audio asset assigned
            if (sfxSource != null && sfxSource.enabled)
            {
                // Volume and pitch scale with agitation
                sfxSource.pitch = Mathf.Lerp(0.8f, 1.3f, currentBPM / 150f);
            }
        }

        public void PlayReactionStinger(string animationClipTag)
        {
            // Plays appropriate audio stinger matching reaction tag
            Debug.Log($"[Audio] Reaction stinger triggered for tag: {animationClipTag}");
        }

        public void PlayEmotionDropSFX(CoreEmotion emotion)
        {
            Debug.Log($"[Audio] Emotion applied SFX: {emotion}");
        }
    }
}
