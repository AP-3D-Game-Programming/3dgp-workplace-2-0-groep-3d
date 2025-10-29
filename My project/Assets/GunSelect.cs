using UnityEngine;

public class GunSelectionUI : MonoBehaviour
{
    public WeaponUI[] slots;

    private int currentIndex = -1;

    public void Highlight(int index)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSelected(i == index);
        }

        currentIndex = index;
    }
}
