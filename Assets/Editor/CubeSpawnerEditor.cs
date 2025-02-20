using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CubeSpawner))]
public class CubeSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector UI.
        DrawDefaultInspector();

        CubeSpawner spawner = (CubeSpawner)target;

        EditorGUILayout.Space();
        // Slider to set the number of cubes to spawn (e.g., from 1 to 20).
        spawner.cubeCount = EditorGUILayout.IntSlider("Number of Cubes", spawner.cubeCount, 1, 20);

        EditorGUILayout.Space();
        // Button to spawn cubes.
        if (GUILayout.Button("Spawn Cube(s)")) spawner.SpawnCube();
        EditorGUILayout.Space();

        // Button to clear all spawned cubes.
        if (GUILayout.Button("Clear All Cubes")) spawner.ClearCubes();
    }
}