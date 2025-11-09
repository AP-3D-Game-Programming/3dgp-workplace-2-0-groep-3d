using UnityEngine;

public class Paintball : MonoBehaviour
{
    public GameObject paintSplatPrefab;
    public float splatSize = 0.3f;
    public float destroyDelay = 2f;
    public int damage = 10;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            return;
        if (collision.gameObject.CompareTag("PlayerGun"))
            return;
        if (collision.gameObject.CompareTag("PaintBall"))
            return;
        if (collision.gameObject.CompareTag("Wall"))
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


            PaintableArea area = collision.gameObject.GetComponent<PaintableArea>();
            if (area != null)
            {
                area.PaintHit(hitPoint, GetComponent<Renderer>().material.color);
            }

            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Police"))
        {
            PoliceAI police = collision.gameObject.GetComponent<PoliceAI>();
            if (police != null)
            {
                police.Stun(2f);
            }

            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Bear"))
        {
            Bear bear = collision.gameObject.GetComponent<Bear>();
            if (bear != null)
                bear.TakeDamage(damage);

            Destroy(gameObject);
            return;
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            Boss boss = collision.gameObject.GetComponent<Boss>();
            if (boss != null)
                boss.TakeDamage(damage);

            Destroy(gameObject);
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
