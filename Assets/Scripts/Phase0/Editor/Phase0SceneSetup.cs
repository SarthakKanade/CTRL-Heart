using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

/// <summary>
/// Automatically sets up the Phase 0 scene with the necessary GameObject
/// when the scene is opened in the Unity Editor.
/// </summary>
[InitializeOnLoad]
public class Phase0SceneSetup
{
    static Phase0SceneSetup()
    {
        EditorSceneManager.sceneOpened += OnSceneOpened;
    }

    private static void OnSceneOpened(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
    {
        // Only auto-setup for Phase0_VerticalSlice scene
        if (scene.name != "Phase0_VerticalSlice") return;

        // Check if Phase0Game already exists in the scene
        Phase0Game existing = Object.FindFirstObjectByType<Phase0Game>();
        if (existing != null)
        {
            Debug.Log("[Phase0 Setup] GameManager already exists in scene. Skipping setup.");
            return;
        }

        // Create the GameManager GameObject
        GameObject gameManager = new GameObject("GameManager");
        gameManager.AddComponent<Phase0Game>();

        // Mark scene as dirty so changes are saved
        EditorSceneManager.MarkSceneDirty(scene);

        Debug.Log("[Phase0 Setup] ✓ Created GameManager with Phase0Game component. Scene is ready to play!");
        Debug.Log("[Phase0 Setup] → Press PLAY to start the vertical slice");
        Debug.Log("[Phase0 Setup] → Play 10 times and log your findings per the Development Plan");
    }

    [MenuItem("CTRL+HEART/Setup Phase 0 Scene")]
    private static void SetupPhase0SceneManual()
    {
        var activeScene = EditorSceneManager.GetActiveScene();

        if (activeScene.name != "Phase0_VerticalSlice")
        {
            Debug.LogWarning("[Phase0 Setup] Please open the Phase0_VerticalSlice scene first.");
            return;
        }

        Phase0Game existing = Object.FindFirstObjectByType<Phase0Game>();
        if (existing != null)
        {
            Debug.LogWarning("[Phase0 Setup] GameManager already exists. Delete it first if you want to recreate.");
            Selection.activeGameObject = existing.gameObject;
            return;
        }

        GameObject gameManager = new GameObject("GameManager");
        gameManager.AddComponent<Phase0Game>();

        EditorSceneManager.MarkSceneDirty(activeScene);
        Selection.activeGameObject = gameManager;

        Debug.Log("[Phase0 Setup] ✓ GameManager created! Scene ready to play.");
    }
}
