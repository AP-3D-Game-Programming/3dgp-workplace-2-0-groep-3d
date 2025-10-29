using UnityEngine;

public class BreakBuilding : MonoBehaviour
{
    [Header("Fling Settings")]
    public float flingForce = 800f;
    public float upwardForce = 300f;
    public bool allowFling = true; // toggle for static scenery

    private Rigidbody rb;
    private bool hasFlung = false;
    private MeshCollider meshCollider;

    void Start()
    {
        meshCollider = GetComponent<MeshCollider>();

        if (allowFling)
        {

            if (meshCollider != null && !meshCollider.convex)
            {
                Debug.LogWarning(name + ": MeshCollider is not convex. Making it convex for physics.");
                meshCollider.convex = true;
            }

            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }
            rb.isKinematic = true;
        }
        else
        {
            if (rb != null)
                Destroy(rb);
        }
    }

    public void Fling(Vector3 source)
    {
        if (!allowFling || hasFlung) return;

        hasFlung = true;
        rb.isKinematic = false;

        Vector3 direction = (transform.position - source).normalized + Vector3.up;
        rb.AddForce(direction * flingForce + Vector3.up * upwardForce);
        rb.AddTorque(Random.insideUnitSphere * 500f);

        // Optional: remove object after 10 seconds
        Destroy(gameObject, 10f);
    }
}
