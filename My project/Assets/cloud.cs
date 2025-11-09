using UnityEngine;
using System.Collections.Generic;

public class CloudSpawnerWithXRange : MonoBehaviour
{
    [Header("Cloud Prefabs")]
    public GameObject[] cloudPrefabs; // Array of cloud prefabs

    [Header("Spawn Volume (Y & Z)")]
    public Transform spawnVolumeCube; // Cube defining Y and Z bounds
    public Vector2 spawnXRange = new Vector2(-50f, 50f); // Custom X spawn range

    [Header("Spawn Settings")]
    public int maxClouds = 20;        // Maximum clouds
    public float spawnInterval = 2f;  // Time between spawns
    public Vector2 scaleRange = new Vector2(5f, 15f);

    [Header("Movement")]
    public Vector3 moveDirection = Vector3.left;
    public Vector2 speedRange = new Vector2(2f, 5f);

    [Header("Lifetime")]
    public Vector2 cloudLifetimeRange = new Vector2(20f, 40f); // 👈 Random lifetime range

    private float spawnTimer = 0f;
    private List<GameObject> activeClouds = new List<GameObject>();

    void Update()
    {
        // 🧹 Clean up destroyed clouds
        for (int i = activeClouds.Count - 1; i >= 0; i--)
        {
            if (activeClouds[i] == null)
                activeClouds.RemoveAt(i);
        }

        // ⏱ Spawn new clouds
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval && activeClouds.Count < maxClouds)
        {
            SpawnCloud();
            spawnTimer = 0f;
        }

        // ☁️ Move clouds
        foreach (var cloud in activeClouds)
        {
            if (cloud != null)
                cloud.transform.position += moveDirection.normalized *
                    cloud.GetComponent<CloudData>().speed * Time.deltaTime;
        }
    }

    private void SpawnCloud()
    {
        if (cloudPrefabs.Length == 0 || spawnVolumeCube == null)
            return;

        // Pick a random prefab
        GameObject prefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Length)];
        GameObject cloud = Instantiate(prefab, transform);

        // Random X position
        float x = Random.Range(spawnXRange.x, spawnXRange.y);

        // Random Y & Z from cube
        Vector3 cubeCenter = spawnVolumeCube.position;
        Vector3 cubeSize = spawnVolumeCube.localScale;
        float y = Random.Range(-cubeSize.y / 2f, cubeSize.y / 2f) + cubeCenter.y;
        float z = Random.Range(-cubeSize.z / 2f, cubeSize.z / 2f) + cubeCenter.z;

        cloud.transform.position = new Vector3(x, y, z);

        // Random scale
        float scale = Random.Range(scaleRange.x, scaleRange.y);
        cloud.transform.localScale = Vector3.one * scale;

        // Speed data
        CloudData data = cloud.AddComponent<CloudData>();
        data.speed = Random.Range(speedRange.x, speedRange.y);

        // 👇 Random lifetime between min and max
        float randomLifetime = Random.Range(cloudLifetimeRange.x, cloudLifetimeRange.y);
        Destroy(cloud, randomLifetime);

        // Track it
        activeClouds.Add(cloud);
    }
}

public class CloudData : MonoBehaviour
{
    public float speed = 3f;
}
