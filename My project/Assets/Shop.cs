using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject shopUI;
    public ShopItem[] shopItems;
    public Button[] itemButtons; // Assign buttons in Inspector

    private void Start()
    {
        // Setup buttons dynamically
        for (int i = 0; i < shopItems.Length && i < itemButtons.Length; i++)
        {
            ShopItem item = shopItems[i];
            Button btn = itemButtons[i];
            TMP_Text txt = btn.GetComponentInChildren<TMP_Text>();
            if (txt != null)
            {
                txt.text = item.isAmmo ? $"{item.itemName} - ${item.price} (+{item.ammoAmount} ammo)"
                                       : $"{item.itemName} - ${item.price}";
            }

            int index = i; // Capture for closure
            btn.onClick.AddListener(() => BuyItem(index));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shopUI.SetActive(true);
            UnlockCursor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shopUI.SetActive(false);
            LockCursor();
        }
    }

    public void BuyItem(int index)
    {
        if (index < 0 || index >= shopItems.Length) return;

        ShopItem item = shopItems[index];

        if (GameManager.Instance.CurrentPlayerMoney < item.price)
        {
            Debug.Log("Not enough money!");
            return;
        }

        GameManager.Instance.AddMoney(-item.price);

        if (item.isAmmo)
        {
            // Reference the item directly from the array
            ShopItem shopItem = shopItems[index];

            // Refill ammo for all guns
            foreach (var gunObj in GunManager.Instance.gunObjects)
            {
                IGun gun = gunObj as IGun;
                if (gun is PaintGun pg)
                    pg.totalAmmo += shopItem.ammoAmount;
                else if (gun is PaintMinigun pm)
                    pm.totalAmmo += shopItem.ammoAmount;
            }

            Debug.Log($"Bought ammo refill: +{shopItem.ammoAmount} ammo");

            // Increase price by 10% for next purchase
            shopItem.price = Mathf.CeilToInt(shopItem.price * 1.1f);

            // Save back to array so it persists
            shopItems[index] = shopItem;

            // Update button text
            TMP_Text txt = itemButtons[index].GetComponentInChildren<TMP_Text>();
            if (txt != null)
            {
                txt.text = $"{shopItem.itemName} - ${shopItem.price} (+{shopItem.ammoAmount} ammo)";
            }
        }


        else if (item.gunObjectInPlayer != null)
        {
            // Check if player already has the gun
            foreach (var obj in GunManager.Instance.gunObjects)
            {
                if (obj.gameObject == item.gunObjectInPlayer)
                {
                    Debug.Log("You already own this gun!");
                    return;
                }
            }

            // Add existing gun to GunManager arrays
            int newLength = GunManager.Instance.gunObjects.Length + 1;
            MonoBehaviour[] newGunObjects = new MonoBehaviour[newLength];
            IGun[] newGuns = new IGun[newLength];

            for (int i = 0; i < GunManager.Instance.gunObjects.Length; i++)
            {
                newGunObjects[i] = GunManager.Instance.gunObjects[i];
                newGuns[i] = GunManager.Instance.guns[i];
            }

            MonoBehaviour mono = item.gunObjectInPlayer.GetComponent<MonoBehaviour>();
            newGunObjects[newLength - 1] = mono;
            newGuns[newLength - 1] = mono as IGun;

            GunManager.Instance.gunObjects = newGunObjects;
            GunManager.Instance.guns = newGuns;

            item.gunObjectInPlayer.SetActive(false); // Hide until equipped

            // Auto-equip
            int newGunIndex = GunManager.Instance.gunObjects.Length - 1;
            GunManager.Instance.ActivateGun(newGunIndex);

            // Disable button
            if (index >= 0 && index < itemButtons.Length)
                itemButtons[index].interactable = false;

            Debug.Log($"Bought new gun: {item.itemName}");
        }
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
