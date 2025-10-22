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

        int total = 0;
        if (gun is PaintGun pg) total = pg.totalAmmo;
        else if (gun is PaintMinigun pm) total = pm.totalAmmo;

        ammoText.text = $"{gun.currentAmmo}/{total}";
        Color c = gun.CurrentPaintColor;
        c.a = 1f;
        colorIndicator.color = c;
    }

}
