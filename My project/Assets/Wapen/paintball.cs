using UnityEngine;

public class Paintball : MonoBehaviour
{
    public GameObject paintSplatPrefab;
    public float splatSize = 0.3f;
    public float destroyDelay = 2f;

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 hitPoint = contact.point + contact.normal * 0.001f;
        Quaternion rotation = Quaternion.LookRotation(contact.normal);

        // Spawn paint splat
        GameObject splat = Instantiate(paintSplatPrefab, hitPoint, rotation);
        splat.transform.Rotate(Vector3.forward, Random.Range(0, 360f));
        splat.transform.localScale = Vector3.one * splatSize;
        splat.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
        splat.transform.SetParent(collision.transform);

        Destroy(gameObject); // remove paintball
    }
}
