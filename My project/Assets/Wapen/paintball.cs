using UnityEngine;

public class Paintball : MonoBehaviour
{
    public GameObject paintSplatPrefab;
    public float splatSize = 0.3f;
    public float destroyDelay = 2f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerGun"))
            return;
        if (collision.gameObject.CompareTag("PaintBall"))
            return;
        if (collision.gameObject.CompareTag("PaintableWall"))
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 hitPoint = contact.point + contact.normal * 0.001f;

            Quaternion rotation = Quaternion.LookRotation(contact.normal);
            rotation *= Quaternion.Euler(0f, 0f, Random.Range(0, 360f));

            GameObject splat = Instantiate(paintSplatPrefab, hitPoint, rotation);

            splat.transform.localScale = Vector3.one * splatSize;

            splat.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
            splat.transform.SetParent(collision.transform);

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
