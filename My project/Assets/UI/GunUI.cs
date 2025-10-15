using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunUI : MonoBehaviour
{
    public TMP_Text ammoText;
    public Image colorIndicator;

    public void UpdateUI(IGun gun)
    {
        if (gun == null) return;

        ammoText.text = $"{gun.currentAmmo}/{gun.maxAmmo}";
        Color c = gun.CurrentPaintColor;
        c.a = 1f;
        colorIndicator.color = c;
    }
}
