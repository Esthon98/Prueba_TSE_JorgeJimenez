using UnityEngine;
using TMPro;

public class CubeSpawner : MonoBehaviour
{
    [Tooltip("Cube prefab to spawn")]
    public GameObject cubePrefab;
    [Tooltip("Range for spawning cubes")]
    public float spawnRange = 5f;
    [Tooltip("Number of cubes to spawn per button press")]
    public int cubeCount = 1;
    [Tooltip("Random materials to assign to each cube instance")]
    public Material[] randomMaterials;

    [Header("UI Counter")]
    [Tooltip("UI text element to display the number of spawned cubes")]
    public TextMeshProUGUI counterText;

    // Container to group spawned cubes and optimize hierarchy.
    private Transform cubeContainer;
    // Total number of cubes spawned.
    public int totalCubesSpawned = 0;

    private void Awake() => CheckContainer();

    void CheckContainer()
    {
        // Create or find a container named "CubeContainer" in the scene.
        GameObject container = GameObject.Find("CubeContainer");
        if (container == null) container = new GameObject("CubeContainer");
        cubeContainer = container.transform;
    }

    // Spawns cubes at random positions and assigns random materials if available.
    public void SpawnCube()
    {
        if (cubeContainer == null) CheckContainer();
        if (cubePrefab == null)
        {
            Debug.LogError("Cube prefab is not assigned.");
            return;
        }

        for (int i = 0; i < cubeCount; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(-spawnRange, spawnRange),
                Random.Range(-spawnRange, spawnRange),
                Random.Range(-spawnRange, spawnRange)
            );

            // Use Random.rotation for a completely random rotation.
            GameObject newCube = Instantiate(cubePrefab, randomPos, Random.rotation, cubeContainer);

            // Assign a random material if available.
            if (randomMaterials != null && randomMaterials.Length > 0)
            {
                Renderer renderer = newCube.GetComponent<Renderer>();
                if (renderer != null) renderer.material = randomMaterials[Random.Range(0, randomMaterials.Length)];
            }
            totalCubesSpawned++;
        }
        Debug.Log($"Spawned {cubeCount} cube(s). Total cubes: {totalCubesSpawned}");
        UpdateCounterUI();
    }

    // Clears all spawned cubes from the container.
    public void ClearCubes()
    {
        if (cubeContainer == null)
        {
            Debug.LogWarning("Cube container not found.");
            return;
        }

        int count = cubeContainer.childCount;
        // Remove all child cubes.
        for (int i = cubeContainer.childCount - 1; i >= 0; i--) DestroyImmediate(cubeContainer.GetChild(i).gameObject);
        totalCubesSpawned = 0;
        Debug.Log($"Cleared {count} cube(s).");
        UpdateCounterUI();
    }

    // Updates the UI text with the current number of spawned cubes.
    private void UpdateCounterUI() { if (counterText != null) counterText.text = "Cubes: " + totalCubesSpawned; }
}