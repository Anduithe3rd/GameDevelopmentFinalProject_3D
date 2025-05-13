using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 7f;
    public float lookSensitivity = 2f;

    [Header("Dash Settings")]
    public float omniDashForce = 15f;
    public float groundDashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public LayerMask groundLayer;
    public string breakableTag = "BreakableWall";

    private Rigidbody rb;
    private Camera cam;

    private bool isGrounded;
    private bool isDashing;
    private float dashTimer;
    private float lastDashTime;

    private Vector3 dashDirection;

    private float verticalLookRotation;

    [Header("Dash Visual Effects")]
    public GameObject groundDashEffect;
    public GameObject omniDashEffect;

    private bool hasUsedOmniDash;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;

        omniDashEffect.SetActive(false);
        groundDashEffect.SetActive(false);
    }

    void Update()
    {
        LookAround();
        CheckGrounded();

        if (!isDashing)
        {
            Move();
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                Jump();

            if (Input.GetKeyDown(KeyCode.E) && Time.time > lastDashTime + dashCooldown)
                StartOmniDash();

            if (Input.GetKeyDown(KeyCode.Q) && isGrounded && Time.time > lastDashTime + dashCooldown)
                StartGroundDash();
        }

        if (isDashing)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= dashDuration)
                EndDash();
        }
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        // Rotate player
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        cam.transform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
    }

    void CheckGrounded()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    
        // Reset omnidash when we land
        if (!wasGrounded && isGrounded)
            hasUsedOmniDash = false;
    }


    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        Vector3 velocity = move.normalized * currentSpeed;
        Vector3 yVelocity = new Vector3(0, rb.linearVelocity.y, 0);

        rb.linearVelocity = velocity + yVelocity;
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // reset Y
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void StartOmniDash()
    {
        if (hasUsedOmniDash) return; // prevent second use until grounded

        dashDirection = cam.transform.forward;
        dashTimer = 0f;
        isDashing = true;
        lastDashTime = Time.time;
        hasUsedOmniDash = true; 

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(dashDirection.normalized * omniDashForce, ForceMode.VelocityChange);

        if (omniDashEffect != null)
            omniDashEffect.SetActive(true);
    }


    void StartGroundDash()
    {
        dashDirection = transform.forward;
        dashTimer = 0f;
        isDashing = true;
        lastDashTime = Time.time;

        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0); // clear XZ but keep Y vel
        rb.AddForce(dashDirection.normalized * groundDashForce, ForceMode.VelocityChange);

        if (groundDashEffect != null)
            groundDashEffect.SetActive(true);

    }

    void EndDash()
    {
        isDashing = false;

        
        if (omniDashEffect != null)
            omniDashEffect.SetActive(false);

        if (groundDashEffect != null)
            groundDashEffect.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDashing && isGrounded && collision.gameObject.CompareTag(breakableTag))
        {
            Destroy(collision.gameObject);
        }
    }
}

