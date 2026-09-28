using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace CtrlHeart.Editor
{
    public static class DateAnimationSetup
    {
        private const string SourceFolder = "Assets/Animations/Female Character";
        private const string OutFolder = "Assets/Animations/DateCharacter";

        private static readonly Dictionary<string, (string clipName, bool isLoop, bool isPingPong)> AnimationConfigs = new()
        {
            // IDLE (Seamless breathing loops)
            { "Eaya-idle_", ("Date_Idle", true, true) },
            { "Maya-idle_maya_slow", ("Date_Idle_Slow", true, true) },

            // TALKING
            { "Saya-talk_casual_neutral", ("Date_Talk_Casual", true, false) },
            { "Faya-talk_happy_warm", ("Date_Talk_Happy_Warm", true, false) },
            { "Faya-talk_teasing_smug2", ("Date_Talk_Teasing_Smug", true, false) },
            { "Daya-talk_serious_vulnerable", ("Date_Talk_Serious_Vulnerable", true, false) },
            { "Daya-talk_hesitant_uncertain", ("Date_Talk_Hesitant", true, false) },
            { "Saya-talk_sharp_confrontational", ("Date_Talk_Sharp_Confrontational", true, false) },
            { "Vaya-talk_gentle_sad", ("Date_Talk_Gentle_Sad", true, false) },

            // SITUATIONAL & SCENARIO ACTIONS
            { "Awkward Silence-awkward_silence", ("Date_Awkward_Silence", false, false) },
            { "Awkward Silence-sympathetic_concern", ("Date_Sympathetic_Concern", false, false) },
            { "Gaya-2nd_date", ("Date_Second_Date_Warmth", false, false) },
            { "Gaya-ending__awkward", ("Date_Ending_Awkward", false, false) },
            { "Haya-expecting", ("Date_Expecting", false, false) },
            { "Jaya-uncomfortable", ("Date_Uncomfortable", false, false) },
            { "Kaya-hurt___stung", ("Date_Hurt_Stung", false, false) },
            { "Kaya-nervous_laugh", ("Date_Nervous_Laugh", false, false) },
            { "Kaya-pull_off", ("Date_Pull_Off", false, false) },
            { "Kaya-warm_interest", ("Date_Warm_Interest", false, false) },
            { "Maya-suprised", ("Date_Surprised", false, false) },
            { "Naya-confused", ("Date_Confused", false, false) },
            { "Naya-shut-down", ("Date_Shut_Down", false, false) },
            { "Test-discomfort", ("Date_Discomfort", false, false) },
            { "Test-neutral_acknowledge", ("Date_Neutral_Acknowledge", false, false) },
            { "Aaya-checking_time_restless", ("Date_Checking_Time_Restless", false, false) },
            { "Aaya-phone_buzz_facedown", ("Date_Phone_Buzz_Facedown", false, false) },
            { "Glass-swirling_drink", ("Date_Swirling_Drink", false, false) },
            { "Iaya-glass_spill_catch", ("Date_Glass_Spill_Catch", false, false) },
            { "Oaya-glass_spill_catch_—_p", ("Date_Player_Catches_Glass", false, false) },
            { "aya-eating_sipping", ("Date_Eating_Sipping", false, false) },
            { "hj-leaning_in_table", ("Date_Leaning_In_Table", false, false) },
            { "Caya-yawn_stifle_embarrassed", ("Date_Yawn_Stifle_Embarrassed", false, false) },

            // REACTIONS
            { "Eaya-react_sad_disappointment", ("Date_React_Sad_Disappointment", false, false) },
            { "Raya-react_relieved_sigh", ("Date_React_Relieved_Sigh", false, false) },
            { "Raya-react_skeptical_brow_raise", ("Date_React_Skeptical_Brow_Raise", false, false) },
            { "TATA-react_boredom_eyeroll", ("Date_React_Boredom_Eyeroll", false, false) },
            { "TATA-react_thinking_chintap", ("Date_React_Thinking_Chintap", false, false) },
            { "YAYA-react_deep_blush", ("Date_React_Deep_Blush", false, false) },
            { "YAYA-react_flirty_wink", ("Date_React_Flirty_Wink", false, false) },
            { "Vaya-hair_tuck_shy", ("Date_Hair_Tuck_Shy", false, false) },
            { "Caya-laugh_genuine_hearty", ("Date_Laugh_Hearty", false, false) }
        };

        [MenuItem("CTRL+HEART/Regenerate Date Animations")]
        public static void GenerateAnimations()
        {
            AssetDatabase.Refresh();

            if (!AssetDatabase.IsValidFolder("Assets/Animations"))
            {
                AssetDatabase.CreateFolder("Assets", "Animations");
            }

            // Clean out old animation folder and recreate fresh
            if (AssetDatabase.IsValidFolder(OutFolder))
            {
                AssetDatabase.DeleteAsset(OutFolder);
            }
            AssetDatabase.CreateFolder("Assets/Animations", "DateCharacter");

            // Locate all PNGs in the Female Character source directory
            var pngFiles = Directory.GetFiles(SourceFolder, "*.png", SearchOption.AllDirectories);
            Debug.Log($"[DateAnimationSetup] Found {pngFiles.Length} PNG files in {SourceFolder}");

            var clips = new Dictionary<string, AnimationClip>();
            var aliasToClip = new Dictionary<string, string>();

            foreach (var file in pngFiles)
            {
                string rawFileName = Path.GetFileNameWithoutExtension(file);

                // Exclude Napkin Catch as explicitly requested
                if (rawFileName.Contains("napkin"))
                {
                    Debug.Log($"[DateAnimationSetup] Skipping excluded asset: {rawFileName}");
                    continue;
                }

                if (!AnimationConfigs.TryGetValue(rawFileName, out var config))
                {
                    Debug.LogWarning($"[DateAnimationSetup] No config found for: {rawFileName}. Generating with default settings.");
                    config = (NormalizeClipName(rawFileName), false, false);
                }

                // Uniform 5x5 grid slicing (256x256 per frame, 0 jitter)
                SliceSpriteSheetUniform(file);

                var assets = AssetDatabase.LoadAllAssetsAtPath(file);
                var sprites = assets.OfType<Sprite>()
                    .OrderBy(s =>
                    {
                        string name = s.name;
                        int lastUnderscore = name.LastIndexOf('_');
                        if (lastUnderscore >= 0 && int.TryParse(name.Substring(lastUnderscore + 1), out int idx))
                            return idx;
                        return 0;
                    })
                    .ToArray();

                if (sprites.Length == 0)
                {
                    Debug.LogError($"[DateAnimationSetup] Failed to load sprites from {file}!");
                    continue;
                }

                float frameRate = 12f;
                var clip = new AnimationClip
                {
                    frameRate = frameRate
                };

                var binding = new EditorCurveBinding
                {
                    type = typeof(Image),
                    path = "",
                    propertyName = "m_Sprite"
                };

                // Prepare sprite frame sequence
                List<Sprite> frameSequence = new();
                if (config.isPingPong)
                {
                    // Forward 0..24, then Backward 23..1 for seamless non-snapping breathing loop
                    for (int i = 0; i < sprites.Length; i++) frameSequence.Add(sprites[i]);
                    for (int i = sprites.Length - 2; i >= 1; i--) frameSequence.Add(sprites[i]);
                }
                else
                {
                    frameSequence.AddRange(sprites);
                }

                var keyframes = new ObjectReferenceKeyframe[frameSequence.Count];
                for (int i = 0; i < frameSequence.Count; i++)
                {
                    keyframes[i] = new ObjectReferenceKeyframe
                    {
                        time = i / frameRate,
                        value = frameSequence[i]
                    };
                }

                var settings = AnimationUtility.GetAnimationClipSettings(clip);
                settings.loopTime = config.isLoop;
                AnimationUtility.SetAnimationClipSettings(clip, settings);
                AnimationUtility.SetObjectReferenceCurve(clip, binding, keyframes);

                string clipPath = $"{OutFolder}/{config.clipName}.anim";
                AssetDatabase.CreateAsset(clip, clipPath);
                clips[config.clipName] = clip;

                // Map raw name and normalized name to this clean clip
                aliasToClip[rawFileName] = config.clipName;
                aliasToClip[NormalizeClipName(rawFileName)] = config.clipName;

                Debug.Log($"[DateAnimationSetup] Created clip '{config.clipName}' ({frameSequence.Count} frames, Loop: {config.isLoop}) from {rawFileName}");
            }

            // Create Animator Controller
            string controllerPath = $"{OutFolder}/DateCharacterAnimator.controller";
            var controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
            var rootStateMachine = controller.layers[0].stateMachine;

            // Designate Idle state
            AnimatorState idleState = null;
            if (clips.TryGetValue("Date_Idle", out var idleClip))
            {
                idleState = rootStateMachine.AddState("Idle");
                idleState.motion = idleClip;
                rootStateMachine.defaultState = idleState;
            }
            else
            {
                Debug.LogError("[DateAnimationSetup] Date_Idle clip not found!");
            }

            // Setup states and transitions
            foreach (var kvp in clips)
            {
                if (kvp.Key == "Date_Idle") continue;

                var state = rootStateMachine.AddState(kvp.Key);
                state.motion = kvp.Value;

                // Add trigger for clean clip name
                controller.AddParameter(kvp.Key, AnimatorControllerParameterType.Trigger);

                // AnyState -> Action State
                var anyTransition = rootStateMachine.AddAnyStateTransition(state);
                anyTransition.AddCondition(AnimatorConditionMode.If, 0, kvp.Key);
                anyTransition.duration = 0.15f;
                anyTransition.canTransitionToSelf = false;

                bool isTalkingState = kvp.Key.StartsWith("Date_Talk_");

                if (!isTalkingState && idleState != null)
                {
                    // Non-looping action/reaction state -> Idle when finished
                    var exitTransition = state.AddTransition(idleState);
                    exitTransition.hasExitTime = true;
                    exitTransition.exitTime = 1.0f;
                    exitTransition.duration = 0.25f;
                    exitTransition.hasFixedDuration = true;
                }
            }

            // Global trigger to return to Idle at any time (e.g. when talking finishes)
            controller.AddParameter("PlayIdle", AnimatorControllerParameterType.Trigger);
            if (idleState != null)
            {
                var anyToIdle = rootStateMachine.AddAnyStateTransition(idleState);
                anyToIdle.AddCondition(AnimatorConditionMode.If, 0, "PlayIdle");
                anyToIdle.duration = 0.15f;
            }

            // Optional trigger to return to Slow Idle (for Slot 6 T2 recovery beat)
            controller.AddParameter("PlayIdleSlow", AnimatorControllerParameterType.Trigger);
            var slowState = rootStateMachine.states.FirstOrDefault(s => s.state.name == "Date_Idle_Slow").state;
            if (slowState != null)
            {
                var anyToSlow = rootStateMachine.AddAnyStateTransition(slowState);
                anyToSlow.AddCondition(AnimatorConditionMode.If, 0, "PlayIdleSlow");
                anyToSlow.duration = 0.15f;
            }

            // Add backward-compatible alias triggers so legacy tags continue to trigger animations seamlessly
            foreach (var alias in aliasToClip)
            {
                if (alias.Key != alias.Value)
                {
                    bool exists = controller.parameters.Any(p => p.name == alias.Key);
                    if (!exists)
                    {
                        controller.AddParameter(alias.Key, AnimatorControllerParameterType.Trigger);
                        var targetState = rootStateMachine.states.FirstOrDefault(s => s.state.name == alias.Value).state;
                        if (targetState != null)
                        {
                            var aliasTransition = rootStateMachine.AddAnyStateTransition(targetState);
                            aliasTransition.AddCondition(AnimatorConditionMode.If, 0, alias.Key);
                            aliasTransition.duration = 0.15f;
                            aliasTransition.canTransitionToSelf = false;
                        }
                    }
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"<color=green>[DateAnimationSetup] Successfully regenerated {clips.Count} animation clips and {controllerPath}</color>");
        }

        public static void SliceSpriteSheetUniform(string assetPath)
        {
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null) return;

            importer.isReadable = true;
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.spritePixelsPerUnit = 100f;
            importer.filterMode = FilterMode.Bilinear;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.maxTextureSize = 2048;

            string baseName = Path.GetFileNameWithoutExtension(assetPath);
            var metas = new List<SpriteMetaData>();

            int cellSize = 256;
            int texWidth = 1280;
            int texHeight = 1280;
            if (assetPath.Contains("idle_maya_slow"))
            {
                texWidth = 1024;
                texHeight = 1024;
            }
            int cols = texWidth / cellSize;
            int rows = texHeight / cellSize;

            int index = 0;
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    int x = col * cellSize;
                    int y = texHeight - (row + 1) * cellSize;

                    var meta = new SpriteMetaData
                    {
                        name = $"{baseName}_{index}",
                        rect = new Rect(x, y, cellSize, cellSize),
                        alignment = (int)SpriteAlignment.Center,
                        pivot = new Vector2(0.5f, 0.5f)
                    };
                    metas.Add(meta);
                    index++;
                }
            }

            importer.spritesheet = metas.ToArray();
            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();
        }

        public static string NormalizeClipName(string rawName)
        {
            return rawName
                .Replace(" ", "_")
                .Replace("-", "_")
                .Replace("___", "_")
                .Replace("__", "_")
                .Trim();
        }
    }
}

