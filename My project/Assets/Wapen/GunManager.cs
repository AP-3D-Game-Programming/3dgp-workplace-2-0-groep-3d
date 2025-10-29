using UnityEngine;

public class GunManager : MonoBehaviour
{
    public static GunManager Instance;

    public MonoBehaviour[] gunObjects;
    private IGun[] guns;

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
            gunObjects[i].gameObject.SetActive(i == 0);

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

    private void ActivateGun(int index)
    {
        if (index < 0 || index >= guns.Length) return;

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
            weaponUIs[i].SetSelected(i == currentGunIndex);
        }
    }

    public void AddGun(GameObject gunPrefab)
    {
        foreach (var obj in gunObjects)
            if (obj.gameObject.name == gunPrefab.name)
                return;

        MonoBehaviour[] newGunObjects = new MonoBehaviour[gunObjects.Length + 1];
        IGun[] newGuns = new IGun[guns.Length + 1];

        for (int i = 0; i < gunObjects.Length; i++)
        {
            newGunObjects[i] = gunObjects[i];
            newGuns[i] = guns[i];
        }

        GameObject newGunObj = Instantiate(gunPrefab, transform);
        newGunObj.SetActive(false);

        MonoBehaviour mono = newGunObj.GetComponent<MonoBehaviour>();
        if (mono == null)
        {
            Debug.LogError("prefab");
            Destroy(newGunObj);
            return;
        }

        newGunObjects[newGunObjects.Length - 1] = mono;
        newGuns[newGuns.Length - 1] = mono as IGun;

        gunObjects = newGunObjects;
        guns = newGuns;

        if (weaponUIs != null && weaponUIs.Length >= newGunObjects.Length)
        {
            weaponUIs[newGunObjects.Length - 1].SetGun(mono as IGun);
        }
    }
}
