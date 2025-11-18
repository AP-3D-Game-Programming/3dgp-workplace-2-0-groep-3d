using UnityEngine;

public class GunManager : MonoBehaviour
{
    public static GunManager Instance;

    public MonoBehaviour[] gunObjects;
    public IGun[] guns;

    public GunSelectionUI gunSelectionUI;
    public WeaponUI[] weaponUIs;
    public GunUI gunUI;


    private int currentGunIndex = 0;

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        guns = new IGun[gunObjects.Length];

        for (int i = 0; i < gunObjects.Length; i++)
        {
            guns[i] = gunObjects[i] as IGun;

            // Activate only the first gun by default
            gunObjects[i].gameObject.SetActive(i == 0);

            // Link weapon UI
            if (weaponUIs != null && i < weaponUIs.Length)
            {
                weaponUIs[i].SetGun(guns[i]);
            }
        }

        currentGunIndex = 0;
        gunUI.UpdateUI(guns[currentGunIndex]);
        gunSelectionUI.Highlight(currentGunIndex);
        UpdateWeaponUISelection();
    }



    private void Update()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                IGun currentGun = guns[currentGunIndex];
                if ((currentGun is PaintGun pg && pg.IsReloading) ||
                    (currentGun is PaintMinigun pmg && pmg.IsReloading))
                    return;

                ActivateGun(i);
                break;
            }
        }

        gunUI.UpdateUI(guns[currentGunIndex]);
    }
    public void ActivateGun(int index)
    {
        if (index < 0 || index >= gunObjects.Length || gunObjects[index] == null)
            return; // cannot activate unowned gun

        for (int i = 0; i < gunObjects.Length; i++)
            gunObjects[i].gameObject.SetActive(i == index);

        currentGunIndex = index;
        gunUI.UpdateUI(guns[currentGunIndex]);
        gunSelectionUI.Highlight(index);
        UpdateWeaponUISelection();
    }




    private void UpdateWeaponUISelection()
    {
        for (int i = 0; i < weaponUIs.Length; i++)
        {
            // Show UI only if we have a gun at this index
            bool hasGun = i < gunObjects.Length && gunObjects[i] != null;
            weaponUIs[i].gameObject.SetActive(hasGun);

            if (hasGun)
                weaponUIs[i].SetSelected(i == currentGunIndex);
        }
    }


    public void AddGun(GameObject gunPrefab)
    {
        // Check if player already has this gun
        foreach (var obj in gunObjects)
            if (obj.gameObject.name == gunPrefab.name)
                return;

        // Find the gun in children of GunManager
        MonoBehaviour gunMono = null;
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == gunPrefab.name)
            {
                gunMono = child.GetComponent<MonoBehaviour>();
                break;
            }
        }

        if (gunMono == null)
        {
            Debug.LogError("Gun not found in player hierarchy: " + gunPrefab.name);
            return;
        }

        // Add it to the arrays
        MonoBehaviour[] newGunObjects = new MonoBehaviour[gunObjects.Length + 1];
        IGun[] newGuns = new IGun[guns.Length + 1];

        for (int i = 0; i < gunObjects.Length; i++)
        {
            newGunObjects[i] = gunObjects[i];
            newGuns[i] = guns[i];
        }

        newGunObjects[newGunObjects.Length - 1] = gunMono;
        newGuns[newGuns.Length - 1] = gunMono as IGun;

        gunObjects = newGunObjects;
        guns = newGuns;

        gunMono.gameObject.SetActive(false);

        // Optional: link weapon UI if needed
        if (weaponUIs != null && weaponUIs.Length >= newGunObjects.Length)
            weaponUIs[newGunObjects.Length - 1].SetGun(gunMono as IGun);
    }

}
