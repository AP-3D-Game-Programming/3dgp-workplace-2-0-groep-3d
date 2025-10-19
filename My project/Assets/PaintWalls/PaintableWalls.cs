using UnityEngine;

public class PaintableArea : MonoBehaviour
{
    public int rewardPerHit = 10; // Geld dat je krijgt als je correct raakt
    public int penaltyPerHit = 5; // Geld dat je verliest als buiten lijnen
    public Collider paintBounds;   // Collider die de 'binnen de lijnen'-zone definieert

    public void PaintHit(Vector3 hitPoint, Color paintColor)
    {
        if (paintBounds.bounds.Contains(hitPoint))
        {
            GameManager.Instance.AddMoney(rewardPerHit);
            Debug.Log("Correct hit! +" + rewardPerHit);
        }
        else
        {
            GameManager.Instance.AddMoney(-penaltyPerHit);
            Debug.Log("Outside lines! -" + penaltyPerHit);
        }
    }
    void Awake()
    {
        if (paintBounds == null)
        {
            paintBounds = GetComponent<Collider>();
            if (paintBounds == null)
            {
                Debug.LogError("No collider found for PaintableArea on " + gameObject.name);
            }
        }
    }

}
