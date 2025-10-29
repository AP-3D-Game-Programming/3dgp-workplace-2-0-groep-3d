using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Bear : MonoBehaviour
{
    public Transform player;
    public float stopDistance = 1.5f;

    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null) return;

        ChasePlayer();
    }

    private void ChasePlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
        }

        Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookPos - transform.position), Time.deltaTime * 5f);
    }
}
