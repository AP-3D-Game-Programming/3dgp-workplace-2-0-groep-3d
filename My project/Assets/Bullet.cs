using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject[] bearPrefabs;
    private bool hasLanded = false;

    void OnCollisionEnter(Collision collision)
    {
        if (hasLanded) return; // prevent multiple spawns
        hasLanded = true;
        if (collision.gameObject.CompareTag("Boss")) return;
        if (collision.gameObject.CompareTag("Bear")) return;

        SpawnBear();

        Destroy(gameObject);
    }

    void SpawnBear()
    {

        if (bearPrefabs.Length == 0) return;

        GameObject prefab = bearPrefabs[Random.Range(0, bearPrefabs.Length)];
        Vector3 spawnPos = new Vector3(transform.position.x, 0, transform.position.z); // ground level
        GameObject bear = Instantiate(prefab, spawnPos, Quaternion.identity);
        bear.tag = "Bear";

        // Optional: random rotation for variety
        bear.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
    }
}
