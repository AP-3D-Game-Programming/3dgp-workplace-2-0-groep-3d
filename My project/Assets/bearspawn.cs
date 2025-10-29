using UnityEngine;

public class BearSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject bearPrefab;      // The bear prefab to spawn
    public float spawnInterval = 5f;   // Seconds between spawns
    public Transform spawnPoint;       // Optional: where to spawn the bear

    private float timer = 0f;

    void Update()
    {
        if (bearPrefab == null) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnBear();
            timer = 0f;
        }
    }

    private void SpawnBear()
    {
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;
        GameObject bear = Instantiate(bearPrefab, spawnPos, Quaternion.identity);
        bear.tag = "Bear";
    }
}
