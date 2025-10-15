using UnityEngine;

public class PaintGun : MonoBehaviour, IGun
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

        if (Input.GetButtonDown("Fire1") && Time.time >= Cooldown)
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

        GameObject ball = Instantiate(paintballPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * shootForce);

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
