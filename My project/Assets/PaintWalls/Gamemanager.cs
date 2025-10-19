using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("UI References")]
    public GameObject gameOverScreen; // optional

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
        Time.timeScale = 0f;
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
