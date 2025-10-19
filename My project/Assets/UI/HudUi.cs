using UnityEngine;
using TMPro;

public class hudui : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text moneyText;
    public TMP_Text timerText;

    [Header("Timer Settings")]
    public bool countdown = true;

    private void Start()
    {
        // Ensure GameManager exists
        if (GameManager.Instance == null)
        {
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;

        // Update money
        moneyText.text = $"{GameManager.Instance.playerMoney}";

        // Update timer
        int timeLeft = Mathf.Max(GameManager.Instance.time, 0);
        int minutes = timeLeft / 60;
        int seconds = timeLeft % 60;
        timerText.text = $"{minutes:00}:{seconds:00}";

        // Optional countdown
        if (countdown && GameManager.Instance.time > 0)
        {
            GameManager.Instance.time -= Mathf.RoundToInt(Time.deltaTime);
        }
    }
}
