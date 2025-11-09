using UnityEngine;
using System.Collections;

public class PaintGun : MonoBehaviour, IGun
{
    public Transform firePoint;
    public GameObject paintballPrefab;
    public float shootForce = 700f;
    public Color[] paintColors;

    public int colorIndex = 0;
    public float fireRate = 0.2f;
    private float Cooldown = 0f;

    public int maxAmmo = 30;
    public int currentAmmo;
    public int totalAmmo = 90;
    int IGun.totalAmmo => totalAmmo;
    public float reloadTime = 1.5f;
    private bool isReloading = false;
    public Camera playerCamera;

    // IGun implementation
    int IGun.currentAmmo => currentAmmo;
    int IGun.maxAmmo => maxAmmo;
    Color IGun.CurrentPaintColor => paintColors.Length > 0 ? paintColors[colorIndex] : Color.white;
    public bool IsReloading => isReloading;
    public float recoilDistance = 0.2f;
    public float recoilSpeed = 10f;
    private Vector3 initialLocalPosition;
    private Coroutine recoilCoroutine; //fix voor recoil bug
    public int damage = 15;
    public Color CurrentPaintColor
    {
        get
        {
            if (paintColors.Length == 0) return Color.white;
            Color c = paintColors[colorIndex];
            c.a = 1f;
            return c;
        }
    }

    void Start()
    {
        currentAmmo = maxAmmo;
        initialLocalPosition = transform.localPosition;
    }

    void Update()
    {
        if (playerCamera != null)
        {
            Vector3 lookDirection = playerCamera.transform.forward;
            firePoint.rotation = Quaternion.LookRotation(lookDirection);
        }

        if (isReloading) return;

        // Reload input
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && totalAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // Shoot input
        if (Input.GetButtonDown("Fire1") && Time.time >= Cooldown)
        {
            Cooldown = Time.time + fireRate;
            StartCoroutine(Recoil());
            Shoot();

        }

        // Change paint color
        if (Input.GetKeyDown(KeyCode.E))
            CycleColor();
    }

    IEnumerator Reload()
    {
        if (totalAmmo <= 0 || currentAmmo == maxAmmo)
            yield break;

        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);

        int bulletsNeeded = maxAmmo - currentAmmo;
        int bulletsToReload = Mathf.Min(bulletsNeeded, totalAmmo);

        currentAmmo += bulletsToReload;
        totalAmmo -= bulletsToReload;

        isReloading = false;
        Debug.Log("Reloaded. Ammo: " + currentAmmo + "/" + maxAmmo + " | Total Ammo: " + totalAmmo);
    }
    IEnumerator Recoil()
    {
        Vector3 startPos = transform.localPosition;
        Vector3 targetPos = initialLocalPosition + transform.forward * recoilDistance;

        float t = 0f;
        while (t < 1f)
        {
            t += recoilSpeed * Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // Move forward to original position
        t = 0f;
        startPos = transform.localPosition;
        while (t < 1f)
        {
            t += recoilSpeed * Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startPos, initialLocalPosition, t);
            yield return null;
        }

        transform.localPosition = initialLocalPosition;
        recoilCoroutine = null; // Clear reference
    }


    public void Shoot()
    {

        if (currentAmmo <= 0)
        {
            Debug.Log("No ammo in magazine!");
            return;
        }

        currentAmmo--;

        GameObject ball = Instantiate(paintballPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * shootForce);
        Paintball pb = ball.GetComponent<Paintball>();
        if (pb != null)
            pb.damage = damage;
        Color c = paintColors[colorIndex];
        c.a = 1f;
        ball.GetComponent<Renderer>().material.color = c;

        AlertNearbyCops();

        Debug.Log("Ammo: " + currentAmmo + "/" + maxAmmo + " | Total Ammo: " + totalAmmo);
        if (recoilCoroutine != null)
            StopCoroutine(recoilCoroutine);

        recoilCoroutine = StartCoroutine(Recoil());
    }

    void AlertNearbyCops()
    {
        PoliceAI[] cops = FindObjectsOfType<PoliceAI>();
        foreach (PoliceAI cop in cops)
            cop.OnPlayerShot(firePoint.position);
    }

    void CycleColor()
    {
        colorIndex = (colorIndex + 1) % paintColors.Length;
        Debug.Log("Paint color: " + paintColors[colorIndex]);
    }
}
