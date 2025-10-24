using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject cubPrefab;
    public float spawnHeight = 10f;
    public Vector2 spawnRangeX = new Vector2(-5f, 5f);
    public Vector2 spawnRangeZ = new Vector2(-5f, 5f);

    public float spawnInterval = 2f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnCub();
            timer = 0f;
        }
    }

    void SpawnCub()
    {
        float offsetX = Random.Range(spawnRangeX.x, spawnRangeX.y);
        float offsetZ = Random.Range(spawnRangeZ.x, spawnRangeZ.y);

        Vector3 basePosition = transform.position;

        Vector3 spawnPosition = new Vector3(
            basePosition.x + offsetX,
            basePosition.y + spawnHeight,
            basePosition.z + offsetZ
        );

        GameObject cub = Instantiate(cubPrefab, spawnPosition, Quaternion.identity);

        Rigidbody rb = cub.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = cub.AddComponent<Rigidbody>();
        }

        rb.useGravity = true;
    }
}
