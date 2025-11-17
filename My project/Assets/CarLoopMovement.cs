using UnityEngine;

public class CarLoopMovement : MonoBehaviour
{
    [Header("Beweging instellingen")]
    [SerializeField] private float speed = 5f;                  // Hoe snel de auto rijdt
    [SerializeField] private Vector3 moveDirection = Vector3.forward; // Richting waarin hij beweegt

    [Header("Reset instellingen")]
    private float travelDistance = 35f;        // Hoe ver de auto mag rijden voor hij reset
    private Vector3 startPosition;                              // Beginpunt
    private float distanceTraveled = 0f;                        // Afstand sinds laatste start
    private bool isDead = false;
    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Bereken verplaatsing deze frame
        Vector3 movement = moveDirection.normalized * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        // Tel de afgelegde afstand op
        distanceTraveled += movement.magnitude;

        // Controleer of afstand overschreden is
        if (distanceTraveled >= travelDistance)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        transform.position = startPosition;
        distanceTraveled = 0f;
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
