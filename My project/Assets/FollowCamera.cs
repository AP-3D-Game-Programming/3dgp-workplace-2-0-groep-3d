using UnityEngine;

public class GunAim : MonoBehaviour
{
    public Transform target; // assign GunTarget here

    void LateUpdate()
    {
        if (!target) return;

        // Rotate the gun to point at the target
        transform.LookAt(target);
    }
}
