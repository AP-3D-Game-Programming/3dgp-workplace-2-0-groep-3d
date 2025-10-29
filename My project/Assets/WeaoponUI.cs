using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponUI : MonoBehaviour
{
    public Image background;
    public Image icon;
    public Image glowImage;
    public TMP_Text label;

    public Color normalColor = new Color(0.3f, 0.3f, 0.3f, 1f);
    public float highlightScale = 1.15f;
    public float animSpeed = 6f;
    public float glowAlpha = 0.5f;

    private bool isSelected = false;
    private Vector3 targetScale;
    private IGun currentGun;

    void Awake()
    {
        targetScale = Vector3.one;
        if (background != null)
            background.color = normalColor;

        if (glowImage != null)
        {
            Color c = glowImage.color;
            c.a = 0f;
            glowImage.color = c;
        }
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animSpeed);

        if (isSelected && currentGun != null && glowImage != null)
        {
            Color c = currentGun.CurrentPaintColor;
            c.a = glowAlpha;
            glowImage.color = Color.Lerp(glowImage.color, c, Time.deltaTime * 8f); // smooth fade
        }
        else if (glowImage != null)
        {
            Color c = glowImage.color;
            c.a = Mathf.Lerp(c.a, 0f, Time.deltaTime * 8f);
            glowImage.color = c;
        }
    }

    public void SetGun(IGun gun)
    {
        currentGun = gun;
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        targetScale = selected ? Vector3.one * highlightScale : Vector3.one;
    }
}
