using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CtrlHeart.Core.UI
{
    /// <summary>
    /// Spawns animated floating status texts and feedback numbers across nodes and resource bars.
    /// Provides immediate visual clarity when threats strike, resources drain/regen, or emotions are applied.
    /// </summary>
    public class UIFloatingFeedback : MonoBehaviour
    {
        public static UIFloatingFeedback Instance { get; private set; }

        private Font uiFont;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            uiFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf") ?? Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        public void SpawnText(Vector3 screenOrWorldPos, string message, Color color, float duration = 1.6f, float floatDistance = 45f)
        {
            StartCoroutine(AnimateFloatingText(screenOrWorldPos, message, color, duration, floatDistance));
        }

        private IEnumerator AnimateFloatingText(Vector3 startPos, string message, Color color, float duration, float floatDistance)
        {
            var go = new GameObject("FloatingText");
            go.transform.SetParent(transform, false);

            var rt = go.AddComponent<RectTransform>();
            rt.position = startPos;
            rt.sizeDelta = new Vector2(260, 44);

            var bgImg = go.AddComponent<Image>();
            bgImg.sprite = UIProceduralTextureGenerator.GetSprite("rpg_button_long_beige");
            bgImg.type = Image.Type.Sliced;
            bgImg.color = Color.white;
            bgImg.raycastTarget = false;

            var textGO = new GameObject("Label");
            textGO.transform.SetParent(go.transform, false);
            var trt = textGO.AddComponent<RectTransform>();
            trt.anchorMin = Vector2.zero;
            trt.anchorMax = Vector2.one;
            trt.offsetMin = Vector2.zero;
            trt.offsetMax = Vector2.zero;

            var text = textGO.AddComponent<Text>();
            text.font = uiFont;
            text.fontSize = 15;
            text.fontStyle = FontStyle.Bold;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = message;
            text.color = color;
            text.raycastTarget = false;

            float elapsed = 0f;
            Vector3 initialPos = rt.localPosition;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                // Smooth upward drift with gentle ease-out
                float yOffset = Mathf.Sin(t * Mathf.PI * 0.5f) * floatDistance;
                rt.localPosition = initialPos + new Vector3(0f, yOffset, 0f);

                // Scale pop at start
                float scale = t < 0.2f ? Mathf.Lerp(0.6f, 1.15f, t / 0.2f) : Mathf.Lerp(1.15f, 1.0f, (t - 0.2f) / 0.8f);
                rt.localScale = Vector3.one * scale;

                // Alpha fade out near end
                float alpha = t > 0.6f ? Mathf.Lerp(1f, 0f, (t - 0.6f) / 0.4f) : 1f;
                text.color = new Color(color.r, color.g, color.b, alpha);

                yield return null;
            }

            Destroy(go);
        }
    }
}
