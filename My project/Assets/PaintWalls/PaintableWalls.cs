using UnityEngine;

public class PaintableArea : MonoBehaviour
{
    public int rewardPerHit = 10; // Money gained for correct hit
    public int penaltyPerHit = 5; // Money lost for wrong hit
    public Collider paintBounds;   // Collider defining the "inside lines" zone
    public Color requiredColor = Color.black; // The color the player must hit with

    private int hitCount = 0;
    public int maxHits = 10;

    public void PaintHit(Vector3 hitPoint, Color paintColor)
    {
        if (hitCount >= maxHits) return;

        hitCount++;

        // Check if hit is inside bounds
        bool insideBounds = paintBounds.bounds.Contains(hitPoint);

        // Check if the color matches
        if (paintColor == requiredColor)
        {
            if (insideBounds)
            {
                GameManager.Instance.AddMoney(rewardPerHit);
                Debug.Log("Correct hit with correct color! +" + rewardPerHit);
            }
            else
            {
                GameManager.Instance.AddMoney(-penaltyPerHit);
                Debug.Log("Outside lines! -" + penaltyPerHit);
            }
        }
        else
        {
            GameManager.Instance.AddMoney(-penaltyPerHit);
            Debug.Log("Wrong color! -" + penaltyPerHit);
        }

        CheckDestroy();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PaintProjectile"))
        {
            Color hitColor = other.GetComponent<Renderer>().material.color;
            PaintHit(other.transform.position, hitColor);
        }
    }

    void Awake()
    {
        if (paintBounds == null)
        {
            paintBounds = GetComponentInChildren<Collider>();
            if (paintBounds == null)
                Debug.LogError("No collider found for PaintableArea on " + gameObject.name);
        }
    }

    void CheckDestroy()
    {
        if (hitCount >= maxHits)
            Destroy(gameObject);
    }
}
