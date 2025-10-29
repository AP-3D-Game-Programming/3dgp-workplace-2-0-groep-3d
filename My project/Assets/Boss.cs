using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5f;
    public float stopDistance = 3f;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {

            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        BreakBuilding fling = collision.collider.GetComponentInParent<BreakBuilding>();
        if (fling != null)
        {
            fling.Fling(transform.position);
        }
    }

}
