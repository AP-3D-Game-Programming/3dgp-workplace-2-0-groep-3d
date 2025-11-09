using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float stopDistance = 3f;

    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Reward Settings")]
    public int moneyPerHit = 10;
    public int moneyOnDeath = 100;

    private bool isDead = false;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    private Animator animator;
    private void Start()
    {
        currentHealth = maxHealth;
        BossHealthUI.Show(this);
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        GameManager.Instance.AddMoney(moneyPerHit);

        BossHealthUI.UpdateHealth(currentHealth);

        Debug.Log($"Boss hit Health: {currentHealth}, Player money: {GameManager.Instance.playerMoney}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        GameManager.Instance.AddMoney(moneyOnDeath);
        BossHealthUI.Hide();
        Debug.Log("Boss ded");

        if (animator != null)
        {
            animator.SetTrigger("Death");
            Destroy(gameObject, 8f);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        BreakBuilding fling = collision.collider.GetComponentInParent<BreakBuilding>();
        if (fling != null)
        {
            fling.Fling(transform.position);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.EndGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("Player"))
        {
            GameManager.Instance.EndGame();
        }
    }
}
