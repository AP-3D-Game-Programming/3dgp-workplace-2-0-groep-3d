using UnityEngine;

public class GunManager : MonoBehaviour
{
    public static GunManager Instance;
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
                // Prevent switching if the current gun is reloading
                IGun currentGun = guns[currentGunIndex];
                if (currentGun is PaintGun pg && pg.IsReloading)
                    return;
                if (currentGun is PaintMinigun pmg && pmg.IsReloading)
                    return;

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
    public void AddGun(GameObject gunPrefab)
    {
        // Check if gun already exists
        foreach (var obj in gunObjects)
            if (obj.gameObject.name == gunPrefab.name)
                return;

        // Create a new array bigger by one
        MonoBehaviour[] newGunObjects = new MonoBehaviour[gunObjects.Length + 1];
        IGun[] newGuns = new IGun[guns.Length + 1];

        // Copy existing guns
        for (int i = 0; i < gunObjects.Length; i++)
        {
            newGunObjects[i] = gunObjects[i];
            newGuns[i] = guns[i];
        }

        // Instantiate the new gun as a child
        GameObject newGunObj = Instantiate(gunPrefab, transform);
        newGunObj.SetActive(false);

        MonoBehaviour mono = newGunObj.GetComponent<MonoBehaviour>();
        if (mono == null)
        {
            Debug.LogError("Gun prefab must have a MonoBehaviour implementing IGun");
            Destroy(newGunObj);
            return;
        }

        newGunObjects[newGunObjects.Length - 1] = mono;
        newGuns[newGuns.Length - 1] = mono as IGun;

        gunObjects = newGunObjects;
        guns = newGuns;
    }

}
