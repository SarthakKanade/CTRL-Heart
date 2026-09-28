using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using CtrlHeart.Core.UI;

namespace CtrlHeart.Core.Editor
{
    public static class CoreSceneSetup
    {
        [MenuItem("CTRL+HEART/Rebuild Bioluminescent UI")]
        public static void RebuildUI()
        {
            var canvas = Object.FindFirstObjectByType<Canvas>();
            if (canvas == null)
            {
                var canvasGO = new GameObject("Canvas");
                canvas = canvasGO.AddComponent<Canvas>();
            }

            var coreUI = UIHierarchyBuilder.BuildUIHierarchy(canvas.gameObject);

            // Wire into GameManager if present in scene
            var gm = Object.FindFirstObjectByType<GameManager>();
            if (gm != null)
            {
                var fi = typeof(GameManager).GetField("ui", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                fi?.SetValue(gm, coreUI);
                EditorUtility.SetDirty(gm);
            }

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

            Debug.Log("<color=green>[CoreSceneSetup] Successfully rebuilt Bioluminescent RTS UI Hierarchy!</color>");
        }
    }
}
