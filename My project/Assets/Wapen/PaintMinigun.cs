using UnityEngine;

public class PaintMinigun : MonoBehaviour, IGun
{
    public Transform firePoint;
    public GameObject paintballPrefab;
    public float shootForce = 700f;
    public Color[] paintColors;

    public Color CurrentPaintColor
    {
        get
        {
            if (paintColors.Length == 0) return Color.white;
            Color c = paintColors[colorIndex];
            c.a = 1f; // force fully opaque
            return c;
        }
    }
    public int colorIndex = 0;

    public float fireRate = 0.2f;
    private float Cooldown = 0f;

    public int maxAmmo = 30;
    public int currentAmmo;
    public float reloadTime = 1.5f;
    private bool isReloading = false;

    public float spreadAngle = 5f;
    // IGun implementation
    int IGun.currentAmmo => currentAmmo;
    int IGun.maxAmmo => maxAmmo;
    Color IGun.CurrentPaintColor => paintColors.Length > 0 ? paintColors[colorIndex] : Color.white;
    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading)
            return;

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= Cooldown)
        {
            Cooldown = Time.time + fireRate;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.E))
            CycleColor();
    }
    System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    public void Shoot()
    {
        if (currentAmmo <= 0)
            return;

        currentAmmo--;

        // Generate random spread
        float angle = Random.Range(0f, spreadAngle);
        Vector3 axis = Random.onUnitSphere;
        Vector3 direction = Quaternion.AngleAxis(angle, axis) * firePoint.forward;

        // Instantiate bullet
        GameObject ball = Instantiate(paintballPrefab, firePoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.linearVelocity = direction * 10f; // bullet speed

        Color c = paintColors[colorIndex];
        c.a = 1f;
        ball.GetComponent<Renderer>().material.color = c;

        Debug.Log("Ammo: " + currentAmmo + "/" + maxAmmo);
    }




    void CycleColor()
    {
        colorIndex = (colorIndex + 1) % paintColors.Length;
        Debug.Log("Paint color: " + paintColors[colorIndex]);
    }
}
