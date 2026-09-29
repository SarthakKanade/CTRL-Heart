using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CtrlHeart.Core.Data;

namespace CtrlHeart.Core.Audio
{
    /// <summary>
    /// Master Audio & Music Manager for CTRL+HEART.
    /// Manages:
    /// 1. Background Music (BGM) Playlist: Starts with Blithe Part A & B, then cycles through cozy cafe tracks.
    /// 2. Ending Music Resolution: Triggers "Till Death Do Us Part" if date ends in second date agreement!
    /// 3. Procedural Heartbeats tied to Composure (Bible Part 2 §2.5).
    /// 4. UI / Emotion SFX feedback.
    /// </summary>
    public class AudioFeedbackManager : MonoBehaviour
    {
        public static AudioFeedbackManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource heartbeatSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("BGM Settings")]
        [Range(0f, 1f)]
        [SerializeField] private float bgmVolume = 0.38f;
        [SerializeField] private bool autoPlayOnStart = true;
        [SerializeField] private float crossfadeDuration = 1.5f;

        [Header("BGM Playlist (Order: Blithe A, Blithe B, then Cafe Ambience)")]
        [SerializeField] private List<AudioClip> bgmPlaylist = new List<AudioClip>();

        [Header("Special Climax Tracks")]
        [SerializeField] private AudioClip secondDateVictoryTrack; // "Till Death Do Us Part"

        [Header("Heartbeat Pitch Bands")]
        [SerializeField] private float stableBPM = 60f;     // Composure 75-100
        [SerializeField] private float nervousBPM = 85f;    // Composure 50-74
        [SerializeField] private float unstableBPM = 115f;  // Composure 25-49
        [SerializeField] private float franticBPM = 145f;   // Composure 1-24

        private int currentTrackIndex = 0;
        private bool isPlayingPlaylist = true;
        private Coroutine activeCrossfadeCoroutine;
        private float heartbeatTimer = 0f;
        private float currentBPM = 60f;

        public AudioSource BgmSource => bgmSource;
        public float BgmVolume => bgmVolume;
        public bool IsPlayingBGM => bgmSource != null && bgmSource.isPlaying;
        public string CurrentBgmTrackName => bgmSource != null && bgmSource.clip != null ? bgmSource.clip.name : "None";

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            EnsureAudioSources();
        }

        private void Start()
        {
            if (autoPlayOnStart && (bgmSource == null || !bgmSource.isPlaying))
            {
                StartBGM();
            }
        }

        public void EnsureAudioSources()
        {
            if (bgmSource == null)
            {
                bgmSource = gameObject.AddComponent<AudioSource>();
                bgmSource.playOnAwake = false;
                bgmSource.loop = false;
                bgmSource.spatialBlend = 0f; // 2D Stereo
                bgmSource.volume = bgmVolume;
            }

            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
                sfxSource.playOnAwake = false;
                sfxSource.spatialBlend = 0f;
                sfxSource.volume = 0.8f;
            }

            if (heartbeatSource == null)
            {
                heartbeatSource = gameObject.AddComponent<AudioSource>();
                heartbeatSource.playOnAwake = false;
                heartbeatSource.spatialBlend = 0f;
                heartbeatSource.volume = 0.4f;
            }
        }

        // ==========================================
        // BACKGROUND MUSIC (BGM) PLAYLIST SYSTEM
        // ==========================================

        /// <summary>
        /// Starts the background music playlist from Track 0 (Blithe part A).
        /// </summary>
        public void StartBGM()
        {
            EnsureAudioSources();

            if (bgmPlaylist == null || bgmPlaylist.Count == 0)
            {
                Debug.LogWarning("[AudioFeedbackManager] BGM Playlist is empty! Please assign clips or call AutoPopulateMusicPack.");
                return;
            }

            isPlayingPlaylist = true;
            currentTrackIndex = 0;
            PlayTrackImmediate(bgmPlaylist[currentTrackIndex], loop: false);
            Debug.Log($"<color=#99FF99>[AudioFeedbackManager] Started BGM: {bgmPlaylist[currentTrackIndex].name}</color>");
        }

        private void Update()
        {
            UpdateBGMPlaylist();
            UpdateHeartbeatTick();
        }

        private void UpdateBGMPlaylist()
        {
            if (!isPlayingPlaylist || bgmSource == null || bgmPlaylist == null || bgmPlaylist.Count == 0)
                return;

            // When the current track naturally reaches the end, advance to the next track
            if (!bgmSource.isPlaying && bgmSource.clip != null && bgmSource.time >= (bgmSource.clip.length - 0.2f))
            {
                AdvanceToNextTrack();
            }
        }

        private void AdvanceToNextTrack()
        {
            if (bgmPlaylist.Count == 0) return;

            currentTrackIndex = (currentTrackIndex + 1) % bgmPlaylist.Count;
            var nextClip = bgmPlaylist[currentTrackIndex];
            Debug.Log($"<color=#99FF99>[AudioFeedbackManager] Advancing to next track [{currentTrackIndex + 1}/{bgmPlaylist.Count}]: {nextClip.name}</color>");
            CrossfadeTo(nextClip, crossfadeDuration, loop: false);
        }

        /// <summary>
        /// Smoothly crossfades to a specific audio clip over the given duration.
        /// </summary>
        public void CrossfadeTo(AudioClip newClip, float duration = 1.5f, bool loop = false)
        {
            EnsureAudioSources();

            if (newClip == null) return;

            if (activeCrossfadeCoroutine != null)
            {
                StopCoroutine(activeCrossfadeCoroutine);
            }

            activeCrossfadeCoroutine = StartCoroutine(CrossfadeRoutine(newClip, duration, loop));
        }

        private IEnumerator CrossfadeRoutine(AudioClip newClip, float duration, bool loop)
        {
            float halfDuration = Mathf.Max(0.1f, duration * 0.5f);
            float startVol = bgmSource.isPlaying ? bgmSource.volume : 0f;

            // Fade out current track
            if (bgmSource.isPlaying && startVol > 0.01f)
            {
                float t = 0f;
                while (t < halfDuration)
                {
                    t += Time.unscaledDeltaTime;
                    bgmSource.volume = Mathf.Lerp(startVol, 0f, t / halfDuration);
                    yield return null;
                }
            }

            // Swap clip & start playing
            bgmSource.clip = newClip;
            bgmSource.loop = loop;
            bgmSource.time = 0f;
            bgmSource.Play();

            // Fade in new track
            float tIn = 0f;
            while (tIn < halfDuration)
            {
                tIn += Time.unscaledDeltaTime;
                bgmSource.volume = Mathf.Lerp(0f, bgmVolume, tIn / halfDuration);
                yield return null;
            }

            bgmSource.volume = bgmVolume;
            activeCrossfadeCoroutine = null;
        }

        private void PlayTrackImmediate(AudioClip clip, bool loop)
        {
            if (clip == null) return;
            bgmSource.clip = clip;
            bgmSource.loop = loop;
            bgmSource.volume = bgmVolume;
            bgmSource.time = 0f;
            bgmSource.Play();
        }

        /// <summary>
        /// Handles ending audio resolution:
        /// If date agreed for second date (Tier 3 or 4) -> Plays 'Till Death Do Us Part'!
        /// Otherwise fades out gracefully.
        /// </summary>
        public void OnDateEnding(EndingType ending)
        {
            isPlayingPlaylist = false;

            if (ending == EndingType.SecondDate_Tier3_4)
            {
                if (secondDateVictoryTrack != null)
                {
                    Debug.Log($"<color=#FF69B4>[AudioFeedbackManager] SUCCESS! Maya agreed to Second Date! Playing victory track: '{secondDateVictoryTrack.name}'</color>");
                    CrossfadeTo(secondDateVictoryTrack, 1.2f, loop: true);
                }
                else
                {
                    Debug.LogWarning("[AudioFeedbackManager] SecondDate victory track is not assigned!");
                }
            }
            else
            {
                // Awkward ending or meltdown: gracefully fade out to let the heavy silence sink in
                Debug.Log($"<color=yellow>[AudioFeedbackManager] Date concluded with {ending}. Fading out BGM.</color>");
                FadeOutBGM(2.0f);
            }
        }

        public void FadeOutBGM(float duration = 2.0f)
        {
            if (activeCrossfadeCoroutine != null) StopCoroutine(activeCrossfadeCoroutine);
            activeCrossfadeCoroutine = StartCoroutine(FadeOutRoutine(duration));
        }

        private IEnumerator FadeOutRoutine(float duration)
        {
            if (bgmSource != null && bgmSource.isPlaying)
            {
                float startVol = bgmSource.volume;
                float t = 0f;
                while (t < duration)
                {
                    t += Time.unscaledDeltaTime;
                    bgmSource.volume = Mathf.Lerp(startVol, 0f, t / duration);
                    yield return null;
                }
                bgmSource.Stop();
                bgmSource.volume = bgmVolume;
            }
            activeCrossfadeCoroutine = null;
        }

        // ==========================================
        // PROCEDURAL HEARTBEATS & SFX
        // ==========================================

        public void UpdateComposureHeartbeat(float composure)
        {
            if (composure >= 75f) currentBPM = stableBPM;
            else if (composure >= 50f) currentBPM = nervousBPM;
            else if (composure >= 25f) currentBPM = unstableBPM;
            else currentBPM = franticBPM;
        }

        private void UpdateHeartbeatTick()
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
            if (sfxSource != null && sfxSource.enabled)
            {
                sfxSource.pitch = Mathf.Lerp(0.8f, 1.3f, currentBPM / 150f);
            }
        }

        public void PlayReactionStinger(string animationClipTag)
        {
            Debug.Log($"[Audio] Reaction stinger triggered for tag: {animationClipTag}");
        }

        public void PlayEmotionDropSFX(CoreEmotion emotion)
        {
            Debug.Log($"[Audio] Emotion applied SFX: {emotion}");
        }

#if UNITY_EDITOR
        [ContextMenu("Auto Populate Music Pack")]
        public void AutoPopulateMusicPack()
        {
            bgmPlaylist.Clear();

            // Desired playlist order: Blithe A, Blithe B, then Autumn Leaves, Good Old Days, Closed Bakery, My Only Love
            string[] trackOrder = new string[]
            {
                "Assets/City Date-a-bgm-asset-pack/Blithe part A.ogg",
                "Assets/City Date-a-bgm-asset-pack/Blithe part B.ogg",
                "Assets/City Date-a-bgm-asset-pack/Autumn Leaves part A.ogg",
                "Assets/City Date-a-bgm-asset-pack/Autumn Leaves part B.ogg",
                "Assets/City Date-a-bgm-asset-pack/Good Old Days part A.ogg",
                "Assets/City Date-a-bgm-asset-pack/Good Old Days part B.ogg",
                "Assets/City Date-a-bgm-asset-pack/Closed Bakery part A.ogg",
                "Assets/City Date-a-bgm-asset-pack/Closed Bakery part B.ogg",
                "Assets/City Date-a-bgm-asset-pack/My Only Love.ogg"
            };

            foreach (var path in trackOrder)
            {
                var clip = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(path);
                if (clip != null)
                {
                    bgmPlaylist.Add(clip);
                }
            }

            secondDateVictoryTrack = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/City Date-a-bgm-asset-pack/Till Death Do Us Part.ogg");

            UnityEditor.EditorUtility.SetDirty(this);
            Debug.Log($"[AudioFeedbackManager] Successfully populated {bgmPlaylist.Count} playlist tracks and victory track: {secondDateVictoryTrack?.name}!");
        }
#endif
    }
}
