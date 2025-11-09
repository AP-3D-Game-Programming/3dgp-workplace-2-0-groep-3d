using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour, IHud
{
    public static GameManager Instance;

    [Header("Player Stats")]
    public int playerMoney = 100;

    [Header("Timer Settings")]
    public int startTime = 180;
    [HideInInspector] public int time;
    private float timer;
    public bool timerRunning = true;

    [Header("UI References")]
    public GameObject gameOverScreen;
    public Button restartButton;
    public Button quitButton;
    public TMP_Text totalMoneyText;

    [Header("Wall Progress UI")]
    public TMP_Text wallsProgressText; // Drag a TextMeshPro UI element here
    public int requiredWallsToExit = 5; // How many walls must be painted before leaving

    private List<PaintableArea> allWalls = new List<PaintableArea>();
    private int completedWalls = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            playerMoney = PlayerPrefs.GetInt("PlayerMoney", 100);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        time = startTime;
        timer = startTime;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        RegisterAllWalls();
        UpdateWallsUI();
    }

    private void Update()
    {
        if (!timerRunning) return;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            time = Mathf.CeilToInt(timer);
        }
        else
        {
            timer = 0;
            time = 0;
            timerRunning = false;
            OnTimerEnd();
        }
    }


    public void RegisterAllWalls()
    {
        allWalls.Clear();
        allWalls.AddRange(FindObjectsOfType<PaintableArea>());
    }

    public void WallCompleted(PaintableArea wall)
    {
        completedWalls++;
        UpdateWallsUI();
    }

    public void UpdateWallsUI()
    {
        if (wallsProgressText == null) return;

        // Clamp so it never exceeds requiredWallsToExit
        int shownCompleted = Mathf.Min(completedWalls, requiredWallsToExit);
        wallsProgressText.text = $"Walls Painted: {shownCompleted}/{requiredWallsToExit}";

        // Change color when requirement met
        if (HasMetWallGoal())
            wallsProgressText.color = Color.green;
        else
            wallsProgressText.color = Color.white;
    }

    public bool HasMetWallGoal()
    {
        return completedWalls >= requiredWallsToExit;
    }


    public int CurrentPlayerMoney => playerMoney;
    public int Timer => time;

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        playerMoney = Mathf.Max(playerMoney, 0);
        Debug.Log($"Money: {playerMoney}");
        PlayerPrefs.SetInt("PlayerMoney", playerMoney);
    }

    private void OnTimerEnd()
    {
        EndGame();
    }

    public void EndGame()
    {
        if (gameOverScreen != null)
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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (totalMoneyText != null)
        {
            int lostMoney = Mathf.FloorToInt(playerMoney * 0.5f);
            totalMoneyText.text = $"Je hebt: {playerMoney}€ Verdient\nVerloren: {lostMoney}€";
        }

    }
    public void RestartGame()
    {
        Debug.Log("Restarting game...");

        playerMoney = Mathf.FloorToInt(playerMoney * 0.5f);
        PlayerPrefs.SetInt("PlayerMoney", playerMoney);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Returning to main menu...");
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("PlayerMoney", playerMoney);
        PlayerPrefs.Save();
    }
}
