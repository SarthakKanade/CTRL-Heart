using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CtrlHeart.Core.UI
{
    /// <summary>
    /// Renders the bottom Heart Rate bar with a real-time animated ECG heartbeat wave
    /// and dynamic BPM readout tied directly to the player's Composure level.
    /// Matches bottom bar in Master Design Bible Part 2 §2.5 & UI Reference.
    /// </summary>
    public class UIHeartRateMonitor : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Text titleText;
        [SerializeField] private Text bpmText;
        [SerializeField] private Image heartIcon;
        [SerializeField] private RawImage ecgWaveImage;

        [Header("Waveform Settings")]
        [SerializeField] private int textureWidth = 512;
        [SerializeField] private int textureHeight = 64;
        [SerializeField] private Color waveColor = new Color(1.0f, 0.35f, 0.55f, 1f);
        [SerializeField] private Color waveTailColor = new Color(0.3f, 0.7f, 1.0f, 0.4f);

        private Texture2D ecgTexture;
        private float[] waveBuffer;
        private float currentComposure = 80f;
        private float pulsePhase = 0f;
        private float currentBPM = 68f;

        private void Awake()
        {
            if (ecgTexture == null) InitializeTexture();
        }

        public void Initialize(RawImage ecgRaw, Text bpm, Image heart)
        {
            ecgWaveImage = ecgRaw;
            bpmText = bpm;
            heartIcon = heart;
            if (ecgTexture == null) InitializeTexture();
            else if (ecgWaveImage != null) ecgWaveImage.texture = ecgTexture;
        }

        private void InitializeTexture()
        {
            ecgTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
            ecgTexture.filterMode = FilterMode.Bilinear;
            ecgTexture.wrapMode = TextureWrapMode.Clamp;

            waveBuffer = new float[textureWidth];
            for (int i = 0; i < textureWidth; i++)
            {
                waveBuffer[i] = 0.5f;
            }

            if (ecgWaveImage != null)
            {
                ecgWaveImage.texture = ecgTexture;
            }
        }

        public void SetComposure(float composure)
        {
            currentComposure = Mathf.Clamp(composure, 0f, 100f);
            // Lower composure -> higher heart rate
            // 100 Composure -> ~60 BPM (Calm)
            // 0 Composure -> ~140 BPM (Severe panic)
            currentBPM = Mathf.Lerp(140f, 60f, currentComposure / 100f);

            if (bpmText != null)
            {
                bpmText.text = $"{Mathf.RoundToInt(currentBPM)} bpm";
                bpmText.color = currentComposure < 35f ? new Color(1f, 0.3f, 0.3f, 1f) : Color.white;
            }
        }

        private void Update()
        {
            if (ecgTexture == null) return;

            // Heart icon subtle scale pulse on beats
            float beatFrequency = currentBPM / 60f;
            pulsePhase += Time.deltaTime * beatFrequency * Mathf.PI * 2f;

            if (heartIcon != null)
            {
                float beatScale = 1.0f + Mathf.Max(0f, Mathf.Sin(pulsePhase)) * (currentComposure < 40f ? 0.35f : 0.18f);
                heartIcon.transform.localScale = Vector3.one * beatScale;
                heartIcon.color = Color.Lerp(new Color(1f, 0.2f, 0.4f), new Color(1f, 0.5f, 0.6f), (Mathf.Sin(pulsePhase) + 1f) * 0.5f);
            }

            // Advance ECG waveform
            float advanceSpeed = Mathf.Lerp(180f, 320f, (100f - currentComposure) / 100f) * Time.deltaTime;
            int shiftPixels = Mathf.Max(1, Mathf.RoundToInt(advanceSpeed));

            for (int x = 0; x < textureWidth - shiftPixels; x++)
            {
                waveBuffer[x] = waveBuffer[x + shiftPixels];
            }

            // Generate new wave data at right edge
            for (int i = 0; i < shiftPixels; i++)
            {
                int idx = textureWidth - shiftPixels + i;
                float samplePhase = pulsePhase + (i / (float)textureWidth) * 4f;
                waveBuffer[idx] = GenerateEcgSample(samplePhase);
            }

            RedrawWaveform();
        }

        private float GenerateEcgSample(float phase)
        {
            float mod = Mathf.Repeat(phase, Mathf.PI * 2f);
            float baseLine = 0.5f;

            // P-wave
            if (mod > 0.5f && mod < 0.9f)
            {
                return baseLine + Mathf.Sin((mod - 0.5f) / 0.4f * Mathf.PI) * 0.12f;
            }
            // Q-dip
            if (mod >= 1.0f && mod < 1.15f)
            {
                return baseLine - 0.1f;
            }
            // R-peak (large spike)
            if (mod >= 1.15f && mod < 1.35f)
            {
                float t = (mod - 1.15f) / 0.2f;
                return baseLine + Mathf.Sin(t * Mathf.PI) * 0.44f;
            }
            // S-dip
            if (mod >= 1.35f && mod < 1.5f)
            {
                return baseLine - 0.14f;
            }
            // T-wave
            if (mod >= 1.8f && mod < 2.4f)
            {
                return baseLine + Mathf.Sin((mod - 1.8f) / 0.6f * Mathf.PI) * 0.16f;
            }

            return baseLine;
        }

        private void RedrawWaveform()
        {
            Color clearCol = new Color(0f, 0f, 0f, 0f);
            var pixels = new Color[textureWidth * textureHeight];

            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = clearCol;
            }

            for (int x = 0; x < textureWidth; x++)
            {
                float val = waveBuffer[x];
                int centerY = Mathf.Clamp(Mathf.RoundToInt(val * (textureHeight - 1)), 0, textureHeight - 1);

                // Draw line with thickness and anti-aliased gradient
                float xFactor = (float)x / textureWidth;
                Color currentLineColor = Color.Lerp(waveTailColor, waveColor, xFactor);

                for (int dy = -2; dy <= 2; dy++)
                {
                    int py = centerY + dy;
                    if (py >= 0 && py < textureHeight)
                    {
                        float alpha = Mathf.Clamp01(1.0f - Mathf.Abs(dy) * 0.4f);
                        pixels[py * textureWidth + x] = new Color(currentLineColor.r, currentLineColor.g, currentLineColor.b, currentLineColor.a * alpha);
                    }
                }
            }

            ecgTexture.SetPixels(pixels);
            ecgTexture.Apply();
        }
    }
}
