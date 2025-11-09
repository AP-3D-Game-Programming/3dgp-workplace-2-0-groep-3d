using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    public Slider healthSlider;
    public GameObject healthBarRoot;

    private static BossHealthUI instance;

    void Awake()
    {
        instance = this;
        healthBarRoot.SetActive(false);
    }

    public static void Show(Boss boss)
    {
        if (instance == null) return;
        instance.healthBarRoot.SetActive(true);
        instance.healthSlider.maxValue = boss.MaxHealth;
        instance.healthSlider.value = boss.CurrentHealth;
    }

    public static void UpdateHealth(int currentHealth)
    {
        if (instance == null) return;
        instance.healthSlider.value = currentHealth;
    }

    public static void Hide()
    {
        if (instance == null) return;
        instance.healthBarRoot.SetActive(false);
    }
}
