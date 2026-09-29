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

        [Header("Buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button howToPlayButton;
        [SerializeField] private Button closeHowToPlayButton;

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
        }

        private void Start()
        {
            if (howToPlayPanel != null)
                howToPlayPanel.SetActive(false);

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
    }
}
