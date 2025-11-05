using UnityEngine;

public class PaintableArea : MonoBehaviour
{
    public int rewardPerHit = 10;
    public int penaltyPerHit = 5;
    public Collider paintBounds;
    public Color requiredColor = Color.black;

    public int maxHits = 10;
    private int hitCount = 0;
    public GameObject uiElement;

    void Awake()
    {
        if (paintBounds == null)
        {
            paintBounds = GetComponentInChildren<Collider>();
            if (paintBounds == null)
                Debug.LogError("No collider found for PaintableArea on " + gameObject.name);
        }
    }

    public void PaintHit(Vector3 hitPoint, Color paintColor)
    {
        if (hitCount >= maxHits) return;

        hitCount++;

        bool insideBounds = paintBounds.bounds.Contains(hitPoint);

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

    void CheckDestroy()
    {
        if (hitCount >= maxHits)
        {
            // Notify GameManager
            GameManager.Instance.WallCompleted(this);

            // Destroy any attached UI
            if (uiElement != null)
                Destroy(uiElement);

            Destroy(gameObject);
        }
    }
}
