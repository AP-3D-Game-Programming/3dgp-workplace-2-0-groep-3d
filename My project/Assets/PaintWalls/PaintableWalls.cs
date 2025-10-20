using UnityEngine;

public class PaintableArea : MonoBehaviour
{
    public int rewardPerHit = 10; // Geld dat je krijgt als je correct raakt
    public int penaltyPerHit = 5; // Geld dat je verliest als buiten lijnen
    public Collider paintBounds;   // Collider die de 'binnen de lijnen'-zone definieert

    private int hitCount = 0;      // Houdt bij hoeveel keer dit object geraakt is
    public int maxHits = 10;       // Aantal hits voordat het object verdwijnt

    public void PaintHit(Vector3 hitPoint, Color paintColor)
    {
        // Als object al vernietigd is, doe niks
        if (hitCount >= maxHits) return;

        hitCount++; // Tel de hit

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
        CheckDestroy();

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PaintProjectile"))
        {
            GameManager.Instance.AddMoney(rewardPerHit);
            hitCount++;
            CheckDestroy();
        }
    }
    void Awake()
    {
        if (paintBounds == null)
        {
            paintBounds = GetComponentInChildren<Collider>();
            if (paintBounds == null)
            {
                Debug.LogError("No collider found for PaintableArea on " + gameObject.name);
            }
        }
    }
    void CheckDestroy()
    {
        if (hitCount >= maxHits)
            Destroy(gameObject);
    }
}
