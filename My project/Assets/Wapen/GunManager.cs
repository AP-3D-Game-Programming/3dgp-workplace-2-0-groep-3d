using UnityEngine;

public class GunManager : MonoBehaviour
{
    public MonoBehaviour[] gunObjects;
    private IGun[] guns;

    public GunUI gunUI;       // Reference to the UI
    private int currentGunIndex = 0;

    private void Start()
    {
        guns = new IGun[gunObjects.Length];
        for (int i = 0; i < gunObjects.Length; i++)
        {
            guns[i] = gunObjects[i] as IGun;
            gunObjects[i].gameObject.SetActive(i == 0); // Activate only the first gun
        }

        currentGunIndex = 0;
        gunUI.UpdateUI(guns[currentGunIndex]);
    }


    private void Update()
    {
        // Switch guns with number keys 1,2,3...
        for (int i = 0; i < guns.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                ActivateGun(i);
                break;
            }
        }

        // Update UI
        gunUI.UpdateUI(guns[currentGunIndex]);
    }

    private void ActivateGun(int index)
    {
        if (index < 0 || index >= guns.Length) return;

        for (int i = 0; i < gunObjects.Length; i++)
            gunObjects[i].gameObject.SetActive(i == index);

        currentGunIndex = index;

        gunUI.UpdateUI(guns[currentGunIndex]);
    }

}
