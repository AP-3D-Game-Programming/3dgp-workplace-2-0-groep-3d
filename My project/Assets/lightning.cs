using UnityEngine;

public class RandomPrefabSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnInterval = 2f;
    public Vector2 xRange = new Vector2(-50f, 50f);
    public Vector2 yRange = new Vector2(5f, 20f);
    public float zPosition = 0f;

    private void Start()
    {
        Invoke(nameof(SpawnPrefab), spawnInterval);
    }

    private void SpawnPrefab()
    {
        float x = Random.Range(xRange.x, xRange.y);
        float y = Random.Range(yRange.x, yRange.y);
        Vector3 spawnPos = new Vector3(x, y, zPosition);

        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        Destroy(this);
    }
}
