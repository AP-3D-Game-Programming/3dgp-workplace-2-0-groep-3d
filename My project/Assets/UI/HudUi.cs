using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudUi : MonoBehaviour
{
    public TMP_Text moneyText;
    public TMP_Text timeText;

    public TMP_Text ammoText;
    public Image colorIndicator;

    public IGun currentGun; // assign from GunManager

    void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (GameManager.Instance != null)
        {
            moneyText.text = $"Money: {GameManager.Instance.CurrentPlayerMoney}";
            timeText.text = $"Time: {GameManager.Instance.Timer}";
        }

        if (currentGun != null)
        {
            ammoText.text = $"{currentGun.currentAmmo}/{currentGun.maxAmmo}";

            Color c = currentGun.CurrentPaintColor;
            c.a = 1f;
            colorIndicator.color = c;
        }
    }
}
