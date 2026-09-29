using System;
using System.Collections.Generic;
using UnityEngine;

namespace CtrlHeart.Core.UI
{
    /// <summary>
    /// Generates high-definition, anti-aliased procedural sprites for all organs,
    /// emotions, resource meters, glowing frames, and impulse cells.
    /// Uses 2D Signed Distance Fields (SDF) to guarantee razor-sharp, smooth rendering
    /// matching the Bioluminescent Control Room aesthetic without external asset dependencies.
    /// </summary>
    public static class UIProceduralTextureGenerator
    {
        private static readonly Dictionary<string, Sprite> cachedSprites = new Dictionary<string, Sprite>();

        public static Sprite GetSprite(string key)
        {
            if (cachedSprites.TryGetValue(key, out var sprite) && sprite != null)
                return sprite;

#if UNITY_EDITOR
            string rpgPath = key switch
            {
                "rpg_panel_brown" => "Assets/kenney_ui-pack-rpg-expansion/PNG/panel_brown.png",
                "rpg_panel_beige" => "Assets/kenney_ui-pack-rpg-expansion/PNG/panel_beige.png",
                "rpg_panel_beige_light" => "Assets/kenney_ui-pack-rpg-expansion/PNG/panel_beigeLight.png",
                "rpg_panel_inset_brown" => "Assets/kenney_ui-pack-rpg-expansion/PNG/panelInset_brown.png",
                "rpg_panel_inset_beige" => "Assets/kenney_ui-pack-rpg-expansion/PNG/panelInset_beige.png",
                "rpg_panel_inset_light" => "Assets/kenney_ui-pack-rpg-expansion/PNG/panelInset_beigeLight.png",
                "rpg_button_long_brown" => "Assets/kenney_ui-pack-rpg-expansion/PNG/buttonLong_brown.png",
                "rpg_button_long_beige" => "Assets/kenney_ui-pack-rpg-expansion/PNG/buttonLong_beige.png",
                "rpg_button_square_brown" => "Assets/kenney_ui-pack-rpg-expansion/PNG/buttonSquare_brown.png",
                "rpg_button_square_beige" => "Assets/kenney_ui-pack-rpg-expansion/PNG/buttonSquare_beige.png",
                "rpg_button_round_brown" => "Assets/kenney_ui-pack-rpg-expansion/PNG/buttonRound_brown.png",
                "rpg_button_round_beige" => "Assets/kenney_ui-pack-rpg-expansion/PNG/buttonRound_beige.png",
                "rpg_bar_back" => "Assets/kenney_ui-pack-rpg-expansion/PNG/barBack_horizontalMid.png",
                "rpg_bar_green" => "Assets/kenney_ui-pack-rpg-expansion/PNG/barGreen_horizontalMid.png",
                "rpg_bar_red" => "Assets/kenney_ui-pack-rpg-expansion/PNG/barRed_horizontalMid.png",
                "rpg_bar_yellow" => "Assets/kenney_ui-pack-rpg-expansion/PNG/barYellow_horizontalMid.png",
                "rpg_bar_blue" => "Assets/kenney_ui-pack-rpg-expansion/PNG/barBlue_horizontalBlue.png",
                "rpg_sword_gold" => "Assets/kenney_ui-pack-rpg-expansion/PNG/cursorSword_gold.png",
                "rpg_icon_cross_brown" => "Assets/kenney_ui-pack-rpg-expansion/PNG/iconCross_brown.png",
                "rpg_icon_circle_brown" => "Assets/kenney_ui-pack-rpg-expansion/PNG/iconCircle_brown.png",
                "rpg_icon_circle_beige" => "Assets/kenney_ui-pack-rpg-expansion/PNG/iconCircle_beige.png",
                "rpg_icon_cross_beige" => "Assets/kenney_ui-pack-rpg-expansion/PNG/iconCross_beige.png",
                "rpg_icon_check_bronze" => "Assets/kenney_ui-pack-rpg-expansion/PNG/iconCheck_bronze.png",
                "rpg_button_square_grey" => "Assets/kenney_ui-pack-rpg-expansion/PNG/buttonSquare_grey.png",
                "rpg_sprout_dialog" => "Assets/Sprout Lands - UI Pack - Basic pack/Sprite sheets/Dialouge UI/Premade dialog box  big.png",
                _ => null
            };

            if (key == "sprout_play_button")
            {
                var subAssets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath("Assets/Sprout Lands - UI Pack - Basic pack/Sprite sheets/UI Big Play Button.png");
                foreach (var a in subAssets)
                {
                    if (a is Sprite s && s.name == "UI Big Play Button_0")
                    {
                        cachedSprites[key] = s;
                        return s;
                    }
                }
            }

            if (!string.IsNullOrEmpty(rpgPath))
            {
                var loaded = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(rpgPath);
                if (loaded != null)
                {
                    cachedSprites[key] = loaded;
                    return loaded;
                }
            }
#endif

            sprite = GenerateSprite(key);
            if (sprite != null) cachedSprites[key] = sprite;
            return sprite;
        }

        private static Sprite GenerateSprite(string key)
        {
            return key switch
            {
                "icon_brain" => CreateBrainSprite(128),
                "icon_voice" => CreateVoiceSprite(128),
                "icon_heart" => CreateHeartSprite(128),
                "icon_body" => CreateBodySprite(128),
                "icon_lungs" => CreateLungsSprite(128),
                "icon_calm" => CreateDropletSprite(128),
                "icon_anxiety" => CreateLightningSprite(128),
                "icon_confidence" => CreateStarSprite(128),
                "icon_attraction" => CreateHeartSprite(128),
                "icon_ecg" => CreatePulseIconSprite(128),
                "icon_warning" => CreateWarningSprite(128),
                "icon_book" => CreateBookSprite(128),
                "icon_pause" => CreatePauseSprite(128),
                "cell_impulse" => CreateImpulseCellSprite(64),
                "ring_node" => CreateNodeRingSprite(256),
                "panel_glass" => CreateGlassPanelSprite(128),
                "bar_pill" => CreatePillBarSprite(128, 32),
                "circle_glow" => CreateGlowCircleSprite(128),
                "icon_girl_avatar" => CreateGirlAvatarSprite(128),
                _ => CreateCircleSprite(64)
            };
        }

        public static Sprite CreateCircleSprite(int size = 128)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            Vector2 c = new Vector2(size / 2f, size / 2f);
            float r = size * 0.46f;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float d = Vector2.Distance(new Vector2(x, y), c) - r;
                    float alpha = Mathf.Clamp01(0.5f - d);
                    tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
                }
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        public static Sprite CreateGlowCircleSprite(int size = 128)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            Vector2 c = new Vector2(size / 2f, size / 2f);
            float maxR = size * 0.48f;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dist = Vector2.Distance(new Vector2(x, y), c) / maxR;
                    float alpha = Mathf.Clamp01(1f - dist);
                    alpha = Mathf.Pow(alpha, 1.8f);
                    tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
                }
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        public static Sprite CreateNodeRingSprite(int size = 256)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            Vector2 c = new Vector2(size / 2f, size / 2f);
            float outerR = size * 0.46f;
            float innerR = size * 0.38f;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dist = Vector2.Distance(new Vector2(x, y), c);
                    float alpha = 0f;

                    if (dist <= outerR + 1f)
                    {
                        if (dist >= innerR - 1f)
                        {
                            // Outer glowing ring
                            float edgeOuter = Mathf.Clamp01(outerR + 1f - dist);
                            float edgeInner = Mathf.Clamp01(dist - (innerR - 1f));
                            alpha = Mathf.Min(edgeOuter, edgeInner) * 0.95f;
                        }
                        else
                        {
                            // Translucent dark center disc
                            alpha = 0.55f;
                        }
                    }
                    tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
                }
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        public static Sprite CreateBrainSprite(int size = 128)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            Vector2 c = new Vector2(size / 2f, size / 2f);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float nx = (x - c.x) / (size * 0.45f);
                    float ny = (y - c.y) / (size * 0.45f);

                    // Symmetrical two-lobed brain shape with cerebellum
                    float dLeft = Vector2.Distance(new Vector2(nx, ny), new Vector2(-0.35f, 0.1f)) - 0.45f;
                    float dRight = Vector2.Distance(new Vector2(nx, ny), new Vector2(0.35f, 0.1f)) - 0.45f;
                    float dTop = Vector2.Distance(new Vector2(nx, ny), new Vector2(0f, 0.25f)) - 0.52f;
                    float dBotLeft = Vector2.Distance(new Vector2(nx, ny), new Vector2(-0.25f, -0.35f)) - 0.35f;
                    float dBotRight = Vector2.Distance(new Vector2(nx, ny), new Vector2(0.25f, -0.35f)) - 0.35f;

                    float shapeDist = Mathf.Min(Mathf.Min(dLeft, dRight), Mathf.Min(dTop, Mathf.Min(dBotLeft, dBotRight)));

                    // Brain fold grooves (subtle lines)
                    float groove = Mathf.Sin(nx * 8f) * Mathf.Cos(ny * 7f + nx * 3f);
                    float grooveCenter = Mathf.Abs(nx) < 0.05f && ny > -0.2f ? 0.3f : 0f;

                    float alpha = Mathf.Clamp01(0.5f - shapeDist * (size * 0.45f));
                    if (groove > 0.65f || grooveCenter > 0f) alpha *= 0.6f;

                    tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
                }
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        public static Sprite CreateVoiceSprite(int size = 128)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            Vector2 c = new Vector2(size / 2f, size / 2f);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float nx = (x - c.x) / (size * 0.45f);
                    float ny = (y - c.y) / (size * 0.45f);

                    // Stylized smiling lips
                    float upperLip = ny - (0.35f * (1f - nx * nx) + 0.15f * Mathf.Sin(Mathf.Abs(nx) * Mathf.PI));
                    float lowerLip = ny - (-0.45f * (1f - nx * nx));
                    float mouthSpread = Mathf.Abs(nx) - 0.85f;

                    float inLip = Mathf.Max(mouthSpread, Mathf.Max(-upperLip, lowerLip));
                    float innerOpening = Mathf.Max(Mathf.Abs(nx) - 0.65f, Mathf.Max(ny - 0.05f * (1f - nx * nx), -ny - 0.1f * (1f - nx * nx)));

                    float alpha = Mathf.Clamp01(0.5f - inLip * (size * 0.45f));
                    if (innerOpening < 0f) alpha *= 0.25f;

                    tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
                }
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        public static Sprite CreateHeartSprite(int size = 128)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            Vector2 c = new Vector2(size / 2f, size / 2.15f);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float nx = (x - c.x) / (size * 0.42f);
                    float ny = (y - c.y) / (size * 0.42f);

                    // 2D Heart implicit curve: (x^2 + y^2 - 1)^3 - x^2 * y^3 <= 0
                    float a = nx * nx + ny * ny - 0.75f;
                    float heartVal = a * a * a - nx * nx * ny * ny * ny;

                    float alpha = Mathf.Clamp01(-heartVal * 2.5f);
                    tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
                }
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        public static Sprite CreateBodySprite(int size = 128)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            Vector2 c = new Vector2(size / 2f, size / 2f);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float nx = (x - c.x) / (size * 0.45f);
                    float ny = (y - c.y) / (size * 0.45f);

                    // Head circle
                    float dHead = Vector2.Distance(new Vector2(nx, ny), new Vector2(0f, 0.52f)) - 0.26f;

                    // Torso & shoulders
                    float dTorso = Vector2.Distance(new Vector2(nx, ny), new Vector2(0f, -0.05f)) - 0.38f;
                    // Lower body
                    float dLegs = Vector2.Distance(new Vector2(nx, ny), new Vector2(0f, -0.45f)) - 0.3f;

                    float dBody = Mathf.Min(dHead, Mathf.Min(dTorso, dLegs));
                    float alpha = Mathf.Clamp01(0.5f - dBody * (size * 0.45f));

                    tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
                }
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        public static Sprite CreateLungsSprite(int size = 128)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            Vector2 c = new Vector2(size / 2f, size / 2f);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float nx = (x - c.x) / (size * 0.45f);
                    float ny = (y - c.y) / (size * 0.45f);

                    // Trachea central stem
                    float dTrachea = Mathf.Max(Mathf.Abs(nx) - 0.08f, Mathf.Max(-ny - 0.1f, ny - 0.65f));

                    // Left & Right lung lobes
                    float dLeftLung = Vector2.Distance(new Vector2(nx, ny), new Vector2(-0.35f, -0.05f)) - 0.38f;
                    float dRightLung = Vector2.Distance(new Vector2(nx, ny), new Vector2(0.35f, -0.05f)) - 0.38f;

                    // Notch inner bottom
                    float dNotch = -Vector2.Distance(new Vector2(nx, ny), new Vector2(0f, -0.4f)) + 0.28f;

                    float lungShapes = Mathf.Min(dLeftLung, dRightLung);
                    lungShapes = Mathf.Max(lungShapes, dNotch);
                    float combined = Mathf.Min(dTrachea, lungShapes);

                    float alpha = Mathf.Clamp01(0.5f - combined * (size * 0.45f));
                    tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
                }
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        public static Sprite CreateDropletSprite(int size = 128)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            Vector2 c = new Vector2(size / 2f, size / 2.3f);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float nx = (x - c.x) / (size * 0.42f);
                    float ny = (y - c.y) / (size * 0.42f);

                    float r = 0.4f;
                    float dCircle = Vector2.Distance(new Vector2(nx, ny), Vector2.zero) - r;
                    float dCone = Mathf.Max(Mathf.Abs(nx) * 1.5f + (ny - 0.85f), -ny);

                    float dist = ny < 0 ? dCircle : Mathf.Min(dCircle, dCone);
                    float alpha = Mathf.Clamp01(0.5f - dist * (size * 0.42f));

                    tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
                }
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        public static Sprite CreateLightningSprite(int size = 128)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            Vector2 c = new Vector2(size / 2f, size / 2f);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float nx = (x - c.x) / (size * 0.45f);
                    float ny = (y - c.y) / (size * 0.45f);

                    // Dynamic bolt shape
                    bool inUpper = ny >= 0f && nx >= (ny * 0.5f - 0.4f) && nx <= (ny * 0.5f + 0.25f);
                    bool inLower = ny <= 0.1f && nx >= (ny * 0.5f - 0.25f) && nx <= (ny * 0.5f + 0.4f);
                    bool inCross = Mathf.Abs(ny) < 0.12f && nx >= -0.55f && nx <= 0.55f;

                    float alpha = (inUpper || inLower || inCross) ? 1f : 0f;
                    tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
                }
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        public static Sprite CreateStarSprite(int size = 128)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            Vector2 c = new Vector2(size / 2f, size / 2f);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float nx = (x - c.x) / (size * 0.45f);
                    float ny = (y - c.y) / (size * 0.45f);

                    // 4-point glowing star with soft diagonal curve
                    float d = Mathf.Pow(Mathf.Abs(nx), 0.5f) + Mathf.Pow(Mathf.Abs(ny), 0.5f) - 0.95f;
                    float alpha = Mathf.Clamp01(-d * 4f);

                    tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
                }
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        public static Sprite CreatePulseIconSprite(int size = 128)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            Vector2 c = new Vector2(size / 2f, size / 2f);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float nx = (x - c.x) / (size * 0.45f);
                    float ny = (y - c.y) / (size * 0.45f);

                    // ECG line with QRS spike
                    float lineY = 0f;
                    if (nx > -0.4f && nx <= -0.2f) lineY = (nx + 0.3f) * -1.5f;
                    else if (nx > -0.2f && nx <= 0.05f) lineY = (nx + 0.075f) * 6.5f;
                    else if (nx > 0.05f && nx <= 0.25f) lineY = (nx - 0.15f) * -4.5f;

                    float dist = Mathf.Abs(ny - lineY) - 0.08f;
                    float alpha = Mathf.Clamp01(0.5f - dist * (size * 0.45f));
                    if (Mathf.Abs(nx) > 0.85f) alpha = 0f;

                    tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
                }
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        public static Sprite CreateWarningSprite(int size = 128)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            Vector2 c = new Vector2(size / 2f, size / 2.3f);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float nx = (x - c.x) / (size * 0.45f);
                    float ny = (y - c.y) / (size * 0.45f);

                    // Equilateral triangle
                    float dTri = Mathf.Max(Mathf.Abs(nx) * 1.732f + ny - 0.7f, -ny - 0.45f);
                    float alpha = Mathf.Clamp01(-dTri * 5f);

                    // Exclamation cut
                    if (Mathf.Abs(nx) < 0.08f && ny >= -0.1f && ny <= 0.35f) alpha *= 0.2f;
                    if (Mathf.Abs(nx) < 0.08f && ny >= -0.32f && ny <= -0.2f) alpha *= 0.2f;

                    tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
                }
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        public static Sprite CreateImpulseCellSprite(int size = 64)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            Vector2 c = new Vector2(size / 2f, size / 2f);
            float r = size * 0.42f;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dist = Vector2.Distance(new Vector2(x, y), c);
                    float alpha = Mathf.Clamp01(1f - (dist / r));
                    alpha = Mathf.Pow(alpha, 1.5f);

                    // Cute tiny eyes
                    float nx = (x - c.x) / r;
                    float ny = (y - c.y) / r;
                    if (ny > 0.05f && ny < 0.3f && (Mathf.Abs(nx - 0.28f) < 0.1f || Mathf.Abs(nx + 0.28f) < 0.1f))
                    {
                        alpha *= 0.15f;
                    }

                    tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
                }
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        public static Sprite CreateGlassPanelSprite(int size = 128)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            float r = size * 0.15f;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float qx = Mathf.Abs(x - size / 2f) - (size / 2f - r);
                    float qy = Mathf.Abs(y - size / 2f) - (size / 2f - r);
                    float dist = Vector2.Distance(new Vector2(Mathf.Max(qx, 0f), Mathf.Max(qy, 0f)), Vector2.zero) + Mathf.Min(Mathf.Max(qx, qy), 0f) - r;

                    float alpha = 0f;
                    if (dist <= 0f)
                    {
                        // Inner glass fill + glowing edge
                        float edgeDist = -dist;
                        if (edgeDist < 2.5f) alpha = 0.9f;
                        else alpha = 0.65f;
                    }
                    tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
                }
            }
            tex.Apply();
            // 9-sliceable sprite
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, new Vector4(24, 24, 24, 24));
        }

        public static Sprite CreatePillBarSprite(int width = 128, int height = 32)
        {
            var tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            float r = height / 2f - 1f;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float qx = Mathf.Abs(x - width / 2f) - (width / 2f - r);
                    float qy = Mathf.Abs(y - height / 2f) - (height / 2f - r);
                    float dist = Vector2.Distance(new Vector2(Mathf.Max(qx, 0f), Mathf.Max(qy, 0f)), Vector2.zero) + Mathf.Min(Mathf.Max(qx, qy), 0f) - r;

                    float alpha = Mathf.Clamp01(0.5f - dist);
                    tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
                }
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, new Vector4(16, 8, 16, 8));
        }

        public static Sprite CreateGirlAvatarSprite(int size = 128)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            Vector2 c = new Vector2(size / 2f, size / 2.2f);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float nx = (x - c.x) / (size * 0.45f);
                    float ny = (y - c.y) / (size * 0.45f);

                    // Face oval
                    float dFace = Vector2.Distance(new Vector2(nx * 1.1f, ny), new Vector2(0f, 0.05f)) - 0.42f;

                    // Cute hair bangs & bob silhouette
                    float dHairTop = Vector2.Distance(new Vector2(nx * 0.95f, ny), new Vector2(0f, 0.28f)) - 0.48f;
                    float dHairSideL = Vector2.Distance(new Vector2(nx, ny), new Vector2(-0.45f, -0.05f)) - 0.28f;
                    float dHairSideR = Vector2.Distance(new Vector2(nx, ny), new Vector2(0.45f, -0.05f)) - 0.28f;
                    float dHair = Mathf.Min(dHairTop, Mathf.Min(dHairSideL, dHairSideR));

                    // Neck / collar
                    float dNeck = Mathf.Max(Mathf.Abs(nx) - 0.16f, Mathf.Max(-ny - 0.45f, ny + 0.2f));

                    float fullShape = Mathf.Min(dFace, Mathf.Min(dHair, dNeck));
                    float alpha = Mathf.Clamp01(0.5f - fullShape * (size * 0.45f));

                    Color col = new Color(0.96f, 0.82f, 0.78f, 1f); // Warm peach skin
                    if (dHair < 0.05f && (ny > 0.12f || Mathf.Abs(nx) > 0.32f))
                    {
                        col = new Color(0.35f, 0.22f, 0.32f, 1f); // Stylish soft chestnut/violet hair
                    }

                    // Eyes
                    if (ny > 0.02f && ny < 0.12f && (Mathf.Abs(nx - 0.18f) < 0.05f || Mathf.Abs(nx + 0.18f) < 0.05f))
                    {
                        col = new Color(0.2f, 0.15f, 0.25f, 1f);
                    }
                    // Gentle smile
                    if (ny > -0.16f && ny < -0.10f && Mathf.Abs(nx) < 0.10f)
                    {
                        col = new Color(0.85f, 0.4f, 0.5f, 1f);
                    }

                    tex.SetPixel(x, y, new Color(col.r, col.g, col.b, col.a * alpha));
                }
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        public static Sprite CreateBookSprite(int size = 128)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float nx = (x / (float)size) * 2f - 1f; // -1 to 1
                    float ny = (y / (float)size) * 2f - 1f; // -1 to 1

                    // Book dimensions
                    if (Mathf.Abs(nx) > 0.85f || Mathf.Abs(ny) > 0.72f)
                    {
                        tex.SetPixel(x, y, Color.clear);
                        continue;
                    }

                    // Leather cover outline / spine
                    float coverDist = Mathf.Max(Mathf.Abs(nx) - 0.82f, Mathf.Abs(ny) - 0.70f);
                    if (coverDist > 0f)
                    {
                        tex.SetPixel(x, y, Color.clear);
                        continue;
                    }

                    // Spine in the middle
                    bool isSpine = Mathf.Abs(nx) < 0.07f;

                    // Left and right pages
                    float pageDist = Mathf.Max(Mathf.Abs(nx) - 0.75f, Mathf.Abs(ny) - 0.63f);
                    bool isPage = pageDist <= 0f && !isSpine;

                    // Bookmark ribbon hanging down
                    bool isRibbon = (nx > 0.02f && nx < 0.12f && ny > -0.80f && ny < 0.55f);

                    Color col;
                    if (isRibbon)
                    {
                        col = new Color(0.85f, 0.20f, 0.22f, 1f); // Crimson ribbon
                        if (ny < -0.68f) col = new Color(0.72f, 0.15f, 0.18f, 1f); // darker ribbon tip
                    }
                    else if (isSpine)
                    {
                        col = new Color(0.35f, 0.18f, 0.08f, 1f); // Dark leather spine
                    }
                    else if (isPage)
                    {
                        // Slight page curvature shading
                        float curve = Mathf.Sin((Mathf.Abs(nx) - 0.07f) / 0.68f * Mathf.PI);
                        float shade = 0.92f + 0.08f * curve;
                        col = new Color(0.98f * shade, 0.95f * shade, 0.88f * shade, 1f); // Parchment

                        // Page margin crease
                        if (Mathf.Abs(nx) < 0.14f)
                        {
                            col = new Color(0.82f, 0.78f, 0.70f, 1f);
                        }

                        // Decorative text lines
                        float lineY = (ny + 0.45f) * 8f;
                        int lineIndex = Mathf.FloorToInt(lineY);
                        float fract = lineY - lineIndex;
                        if (lineIndex >= 1 && lineIndex <= 6 && fract > 0.65f && Mathf.Abs(nx) > 0.20f && Mathf.Abs(nx) < 0.68f)
                        {
                            col = new Color(0.65f, 0.58f, 0.50f, 0.9f); // Soft ink lines
                        }
                    }
                    else
                    {
                        // Leather cover trim
                        col = new Color(0.48f, 0.26f, 0.12f, 1f); // Warm leather brown
                    }

                    tex.SetPixel(x, y, col);
                }
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        public static Sprite CreatePauseSprite(int size = 128)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float nx = (x / (float)size) * 2f - 1f; // -1 to 1
                    float ny = (y / (float)size) * 2f - 1f; // -1 to 1

                    // Two vertical bars: bar 1 at nx in [-0.55, -0.15], bar 2 at nx in [0.15, 0.55], ny in [-0.60, 0.60]
                    float bar1 = Mathf.Max(Mathf.Abs(nx + 0.35f) - 0.18f, Mathf.Abs(ny) - 0.55f);
                    float bar2 = Mathf.Max(Mathf.Abs(nx - 0.35f) - 0.18f, Mathf.Abs(ny) - 0.55f);
                    float d = Mathf.Min(bar1, bar2);

                    float alpha = Mathf.Clamp01(0.5f - d * (size * 0.5f));
                    tex.SetPixel(x, y, new Color(1f, 0.97f, 0.90f, alpha));
                }
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        }
    }
}

