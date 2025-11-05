using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public GameObject promptUI;
    public string sceneToLoad;
    public GameObject lockedUI; // Optional: shows “Paint more walls first!”

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryAdvanceLevel();
        }
    }

    void TryAdvanceLevel()
    {
        if (GameManager.Instance.HasMetWallGoal())
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            if (lockedUI != null)
            {
                lockedUI.SetActive(true);
                CancelInvoke(nameof(HideLockedUI));
                Invoke(nameof(HideLockedUI), 2f); // hide after 2 seconds
            }
            Debug.Log("You must paint more walls before leaving!");
        }
    }

    void HideLockedUI()
    {
        if (lockedUI != null)
            lockedUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            promptUI.SetActive(false);
        }
    }
}
