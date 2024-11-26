using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
    [SerializeField] GameObject fuelOrbPrefab;
    [SerializeField] GameObject coldOrbPrefab;
    [SerializeField] GameObject oxygenOrbPrefab;

    [SerializeField] int initialFuelOrbs = 5;
    [SerializeField] int additionalFuelOrbs = 5;

    private int fuelOrbsCollected = 0;

    private Vector2 spawnAreaMin;
    private Vector2 spawnAreaMax;

    void Start()
    {
        // Calculate spawn area based on camera view
        CalculateSpawnArea();

        SpawnFuelOrbs(initialFuelOrbs);
    }

    // Calculates the spawn area based on the camera's viewport
    void CalculateSpawnArea()
    {
        Camera mainCamera = Camera.main;

        // Get the world position of the bottom-left and top-right corners of the viewport
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        // Adjust by 1 unit to stay within the screen boundaries minus 1
        spawnAreaMin = new Vector2(bottomLeft.x + 1f, bottomLeft.y + 1f);
        spawnAreaMax = new Vector2(topRight.x - 1f, topRight.y - 1f);
    }

    // Call this method when a fuel orb is collected
    public void FuelOrbCollected()
    {
        fuelOrbsCollected++;
        if (fuelOrbsCollected >= initialFuelOrbs)
        {
            fuelOrbsCollected = 0;
            SpawnFuelOrbs(additionalFuelOrbs);
        }
    }

    // Spawns fuel orbs and possibly other orbs
    void SpawnFuelOrbs(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Spawn Fuel Orb
            Vector2 spawnPosition = GetRandomPosition();
            Instantiate(fuelOrbPrefab, spawnPosition, Quaternion.identity);

            // 50% chance to spawn a cold or oxygen orb
            if (Random.value < 0.5f)
            {
                GameObject orbPrefab = Random.value < 0.5f ? coldOrbPrefab : oxygenOrbPrefab;
                Vector2 orbSpawnPosition = GetRandomPosition();
                Instantiate(orbPrefab, orbSpawnPosition, Quaternion.identity);
            }
        }
    }

    // Generates a random position within the spawn area
    Vector2 GetRandomPosition()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        return new Vector2(x, y);
    }
}
