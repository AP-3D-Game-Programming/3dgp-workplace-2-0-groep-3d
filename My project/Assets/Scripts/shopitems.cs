using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public string itemName;                // Display name
    public int price;                      // Cost in player money
    public GameObject gunObjectInPlayer;   // Assign the gun object in scene for weapons
    public bool isAmmo;                    // True if this item refills ammo
    public int ammoAmount;                 // Ammo added if isAmmo = true
    public float ammoMultiplier = 1f;
}
