using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BearFling : MonoBehaviour
{
    [Header("Fling Settings")]
    public float flingDistanceMin = 3f;
    public float flingDistanceMax = 7f;

    public float flingHeightMin = 1f;
    public float flingHeightMax = 3f;

    public float spinStrengthMin = 180f;
    public float spinStrengthMax = 720f;

    public float flingDurationMin = 0.4f;
    public float flingDurationMax = 0.8f;

    public float flingRadius = 2f;
    public LayerMask buildingLayer;

    private HashSet<Transform> flungBuildings = new HashSet<Transform>();

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, flingRadius, buildingLayer);

        foreach (Collider hit in hits)
        {
            if (!flungBuildings.Contains(hit.transform))
            {
                Vector3 dirToBuilding = (hit.transform.position - transform.position).normalized;
                if (Vector3.Dot(transform.forward, dirToBuilding) > 0.3f) // only in front
                {
                    flungBuildings.Add(hit.transform);
                    StartCoroutine(FlingBuilding(hit.transform));
                }
            }
        }
    }

    private IEnumerator FlingBuilding(Transform building)
    {
        Vector3 startPos = building.position;
        Vector3 direction = (building.position - transform.position).normalized;

        float flingDistance = Random.Range(flingDistanceMin, flingDistanceMax);
        float flingHeight = Random.Range(flingHeightMin, flingHeightMax);
        float spinStrength = Random.Range(spinStrengthMin, spinStrengthMax);
        float flingDuration = Random.Range(flingDurationMin, flingDurationMax);

        float elapsed = 0f;

        while (elapsed < flingDuration)
        {
            float t = elapsed / flingDuration;

            float heightOffset = Mathf.Sin(t * Mathf.PI) * flingHeight;
            building.position = startPos + direction * flingDistance * t + Vector3.up * heightOffset;

            building.Rotate(Vector3.up, spinStrength * Time.deltaTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        building.position = startPos + direction * flingDistance;
    }
}
