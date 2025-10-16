using UnityEngine;

public class GameManager : MonoBehaviour, IHud
{
    public static GameManager Instance;

    public int playerMoney = 100;
    public int time = 180;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public int CurrentPlayerMoney => playerMoney;
    public int Timer => time;

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        playerMoney = Mathf.Max(playerMoney, 0);
        Debug.Log("Money: " + playerMoney);
    }
}
