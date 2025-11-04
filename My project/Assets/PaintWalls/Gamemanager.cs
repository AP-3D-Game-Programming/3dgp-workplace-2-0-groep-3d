using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour, IHud
{
    public static GameManager Instance;

    [Header("Player Stats")]
    public int playerMoney = 100;

    [Header("Timer Settings")]
    public int startTime = 180;   // Start time in seconds
    [HideInInspector] public int time; // Current time left
    private float timer;          // Internal floating timer
    public bool timerRunning = true;
    public Button restartButton;
    public Button quitButton;

    [Header("UI References")]
    public GameObject gameOverScreen; // optional
    public TMP_Text totalMoneyText;   // For TextMeshPro

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        time = startTime;
        timer = startTime;

    }

    private void Update()
    {
        if (!timerRunning) return;

        if (timer > 0)
        {
            timer -= Time.deltaTime; // countdown smoothly
            time = Mathf.CeilToInt(timer); // convert to integer for display
        }
        else
        {
            timer = 0;
            time = 0;
            timerRunning = false;
            OnTimerEnd();
        }
    }

    public int CurrentPlayerMoney => playerMoney;
    public int Timer => time;

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        playerMoney = Mathf.Max(playerMoney, 0);
        Debug.Log($"Money: {playerMoney}");
    }

    private void OnTimerEnd()
    {
        EndGame();
    }

    public void EndGame()
    {
        gameOverScreen.SetActive(true);

        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartGame);
        }

        if (quitButton != null)
        {
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(QuitGame);
        }

        // Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (totalMoneyText != null)
        {
            totalMoneyText.text = $"Je hebt: {playerMoney}€ Verdient";
        }
    }






    // Restart current level
    public void RestartGame()
    {
        Debug.Log("test");
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Go back to Main Menu
    public void QuitGame()
    {
        Debug.Log("test");
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }
}
