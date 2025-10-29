using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5;
    public bool canRun = true;
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    [Header("Map")]
    public Camera topDownCamera;
    public GameObject mapUI;
    public GameObject[] otherUI;
    public KeyCode toggleMapKey = KeyCode.M;

    private bool mapActive = false;
    private Rigidbody rb;
    public bool IsRunning { get; private set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (topDownCamera != null)
            topDownCamera.gameObject.SetActive(false);

        if (mapUI != null)
            mapUI.SetActive(false);
    }

    void Update()
    {
        ToggleMap();
    }

    void FixedUpdate()
    {
        if (!mapActive)
            HandleMovement();
    }

    void HandleMovement()
    {
        IsRunning = canRun && Input.GetKey(runningKey);
        float targetSpeed = IsRunning ? runSpeed : speed;

        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        move *= targetSpeed;

        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    }

    void ToggleMap()
    {
        if (Input.GetKeyDown(toggleMapKey))
        {
            mapActive = !mapActive;

            if (topDownCamera != null)
                topDownCamera.gameObject.SetActive(mapActive);

            if (mapUI != null)
                mapUI.SetActive(mapActive);

            foreach (var ui in otherUI)
            {
                if (ui != null)
                    ui.SetActive(!mapActive);
            }
        }
    }
}
