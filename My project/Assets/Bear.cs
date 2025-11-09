using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Bear : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Movement Settings")]
    public float stopDistance = 1.5f;
    public float moveSpeed = 4f;

    [Header("Health Settings")]
    public int maxHealth = 50;
    private int currentHealth;

    [Header("Reward Settings")]
    public int moneyPerHit = 5;
    public int moneyOnDeath = 50;

    private bool isDead = false;
    private Animator animator;
    private NavMeshAgent agent;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookPos - transform.position), Time.deltaTime * 5f);
        }
        else
        {
            agent.isStopped = true;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        GameManager.Instance.AddMoney(moneyPerHit);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        GameManager.Instance.AddMoney(moneyOnDeath);

        if (animator != null)
        {
            animator.SetTrigger("Death");
            agent.isStopped = true;
            Destroy(gameObject, 5f); // waits for animation to finish
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
            GameManager.Instance.EndGame();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("Player"))
            GameManager.Instance.EndGame();
    }
}
