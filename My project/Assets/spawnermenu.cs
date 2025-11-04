using UnityEngine;

public class Spawnermenu : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject cubPrefab;
    public float spawnHeight = 10f;
    public Vector2 spawnRangeX = new Vector2(-5f, 5f);
    public Vector2 spawnRangeZ = new Vector2(-5f, 5f);
    public float spawnInterval = 60f;

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
        Vector3 basePosition = transform.position;
        float offsetX = Random.Range(spawnRangeX.x, spawnRangeX.y);
        float offsetZ = Random.Range(spawnRangeZ.x, spawnRangeZ.y);

        Vector3 spawnPosition = new Vector3(
            basePosition.x + offsetX,
            basePosition.y + spawnHeight,
            basePosition.z + offsetZ
        );

        GameObject cub = Instantiate(cubPrefab, spawnPosition, Quaternion.identity);
        Rigidbody rb = cub.GetComponent<Rigidbody>() ?? cub.AddComponent<Rigidbody>();
        rb.useGravity = true;
    }
}
