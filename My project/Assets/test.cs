using UnityEngine;

public class SpawnerActivator : MonoBehaviour
{
    public Spawner spawner;
    public GameObject promptUI;
    public KeyCode activationKey = KeyCode.E;

    private bool playerInRange = false;
    private bool hasActivated = false;

    void Update()
    {
        if (playerInRange && !hasActivated && Input.GetKeyDown(activationKey))
        {
            spawner.ActivateSpawner();
            hasActivated = true;
            if (promptUI) promptUI.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasActivated)
        {
            playerInRange = true;
            if (promptUI) promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (promptUI) promptUI.SetActive(false);
        }
    }
}
