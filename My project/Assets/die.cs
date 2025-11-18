using UnityEngine;

public class die : MonoBehaviour

{
    private bool isDead = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
