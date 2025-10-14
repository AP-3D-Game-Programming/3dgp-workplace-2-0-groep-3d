using UnityEngine;

public class PaintGun : MonoBehaviour
{
    public Transform firePoint;
    public GameObject paintballPrefab;
    public float shootForce = 700f;
    public Color[] paintColors;
    public float fireRate = 0.2f;
    private float Cooldown = 0f;
    private int colorIndex = 0;

    public int maxAmmo = 30;
    public int currentAmmo;
    public float reloadTime = 1.5f;
    private bool isReloading = false;
    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= Cooldown)
        {
            Cooldown = Time.time + fireRate;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
            CycleColor();
    }
    System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    void Shoot()
    {
        if (currentAmmo <= 0)
            return;

        currentAmmo--;

        GameObject ball = Instantiate(paintballPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * shootForce);

        Color c = paintColors[colorIndex];
        ball.GetComponent<Renderer>().material.color = c;

        Debug.Log("Ammo: " + currentAmmo + "/" + maxAmmo);
    }


    void CycleColor()
    {
        colorIndex = (colorIndex + 1) % paintColors.Length;
        Debug.Log("Paint color: " + paintColors[colorIndex]);
    }
}
