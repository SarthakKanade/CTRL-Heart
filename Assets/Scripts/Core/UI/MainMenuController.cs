using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using CtrlHeart.Core.Audio;

namespace CtrlHeart.Core.UI
{
    /// <summary>
    /// Controller for the Main Menu Scene in CTRL+HEART.
    /// Manages:
    /// - Center Start/Play button -> launches MainDateScene
    /// - Side Book button -> opens How To Play modal overlay
    /// - How to Play overlay dismissal
    /// - Main menu BGM playback
    /// </summary>
    public class MainMenuController : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject howToPlayPanel;
        [SerializeField] private GameObject futurePlansPanel;

        [Header("Buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button howToPlayButton;
        [SerializeField] private Button closeHowToPlayButton;
        [SerializeField] private Button futurePlansButton;
        [SerializeField] private Button closeFuturePlansButton;
        [SerializeField] private Button understoodButton;
        [SerializeField] private Button hypedButton;

        private void Awake()
        {
            // Always ensure time scale is normal when entering main menu
            Time.timeScale = 1f;

            if (playButton != null)
                playButton.onClick.AddListener(OnPlayClicked);

            if (howToPlayButton != null)
                howToPlayButton.onClick.AddListener(OnOpenHowToPlay);

            if (closeHowToPlayButton != null)
                closeHowToPlayButton.onClick.AddListener(OnCloseHowToPlay);

            if (understoodButton != null)
                understoodButton.onClick.AddListener(OnCloseHowToPlay);

            if (futurePlansButton != null)
                futurePlansButton.onClick.AddListener(OnOpenFuturePlans);

            if (closeFuturePlansButton != null)
                closeFuturePlansButton.onClick.AddListener(OnCloseFuturePlans);

            if (hypedButton != null)
                hypedButton.onClick.AddListener(OnCloseFuturePlans);
        }

        private void Start()
        {
            if (howToPlayPanel != null)
                howToPlayPanel.SetActive(false);

            if (futurePlansPanel != null)
                futurePlansPanel.SetActive(false);

            // Ensure BGM plays smoothly
            var audioMgr = AudioFeedbackManager.Instance ?? FindFirstObjectByType<AudioFeedbackManager>();
            if (audioMgr != null && !audioMgr.IsPlayingBGM)
            {
                audioMgr.StartBGM();
            }
        }

        private void Update()
        {
            // Escape key closes modal if open
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (howToPlayPanel != null && howToPlayPanel.activeSelf)
                {
                    OnCloseHowToPlay();
                }
                if (futurePlansPanel != null && futurePlansPanel.activeSelf)
                {
                    OnCloseFuturePlans();
                }
            }
        }

        public void OnPlayClicked()
        {
            Time.timeScale = 1f;
            Debug.Log("<color=green>[MainMenuController] Starting Date! Loading MainDateScene...</color>");
            SceneManager.LoadScene("MainDateScene");
        }

        public void OnOpenHowToPlay()
        {
            if (howToPlayPanel != null)
            {
                howToPlayPanel.SetActive(true);
            }
        }

        public void OnCloseHowToPlay()
        {
            if (howToPlayPanel != null)
            {
                howToPlayPanel.SetActive(false);
            }
        }

        public void OnOpenFuturePlans()
        {
            if (futurePlansPanel != null)
            {
                futurePlansPanel.SetActive(true);
            }
        }

        public void OnCloseFuturePlans()
        {
            if (futurePlansPanel != null)
            {
                futurePlansPanel.SetActive(false);
            }
        }
    }
}
