using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class SceneManagerWindow : EditorWindow
{
    private Vector2 scrollPosition;

    // Add menu item.
    [MenuItem("Tools/Scene Manager")]
    public static void ShowWindow()
    {
        GetWindow<SceneManagerWindow>("Scene Manager");
    }

    private void OnEnable() => Repaint();

    // Draw the custom Editor window GUI.
    private void OnGUI()
    {
        GUILayout.Label("Scenes in Build Settings", EditorStyles.boldLabel);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));
        List<EditorBuildSettingsScene> scenes = EditorBuildSettings.scenes.ToList();

        for (int i = 0; i < scenes.Count; i++)
        {
            string scenePath = scenes[i].path;
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            GUILayout.BeginHorizontal();
            GUILayout.Label(sceneName, GUILayout.Width(200));
            if (GUILayout.Button("Load", GUILayout.Width(80))) LoadScene(scenePath);
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        GUILayout.Space(10);
        if (GUILayout.Button("Reload Current Scene", GUILayout.Height(30))) ReloadCurrentScene();
        GUILayout.Space(10);
        GUILayout.Label("Drag a scene here to add it to Build Settings", EditorStyles.boldLabel);

        // Drop area for dragging scene assets.
        Rect dropArea = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drag scene here", EditorStyles.helpBox);
        HandleDragAndDrop(dropArea);
    }

    // Load scene.
    private void LoadScene(string scenePath)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene(scenePath);
    }

    // Reload currently scene.
    private void ReloadCurrentScene()
    {
        string currentScenePath = SceneManager.GetActiveScene().path;
        if (!string.IsNullOrEmpty(currentScenePath)) LoadScene(currentScenePath);
    }

    // Handle drag and drop events to add scenes to Build Settings.
    private void HandleDragAndDrop(Rect dropArea)
    {
        Event evt = Event.current;
        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(evt.mousePosition)) return;
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    Object[] draggedObjects = DragAndDrop.objectReferences;
                    foreach (Object draggedObject in draggedObjects)
                    {
                        if (draggedObject is SceneAsset)
                        {
                            string scenePath = AssetDatabase.GetAssetPath(draggedObject);
                            AddSceneToBuildSettings(scenePath);
                        }
                    }
                }
                Event.current.Use();
                break;
        }
    }

    // Add a scene to Build Settings if it is not already included.
    private void AddSceneToBuildSettings(string scenePath)
    {
        List<EditorBuildSettingsScene> scenes = EditorBuildSettings.scenes.ToList();

        if (!scenes.Any(s => s.path == scenePath))
        {
            scenes.Add(new EditorBuildSettingsScene(scenePath, true));
            EditorBuildSettings.scenes = scenes.ToArray();
            Debug.Log($"Scene added to Build Settings: {scenePath}");
        }
        else Debug.Log($"Scene already exists in Build Settings: {scenePath}");
    }
}