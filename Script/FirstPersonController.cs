using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float maxSpeed = 5f;
    public float stopForce = 10f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public Transform playerCamera;

    [Header("Camera Settings")]
    public Vector3 firstPersonOffset = new Vector3(0f, 1.6f, 0.6f);
    public Vector3 thirdPersonOffset = new Vector3(0f, 2f, -4f);
    public float cameraSwitchSpeed = 5f;

    private Rigidbody rb;
    private Animator animator;
    private float verticalRotation = 0f;
    private Vector3 movement;
    private bool isThirdPerson = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        playerCamera.localPosition = firstPersonOffset;
    }

    void Update()
    {
        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // Get movement input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward * vertical;
        Vector3 right = transform.right * horizontal;
        movement = (forward + right).normalized;

        // FIXED: Don't freeze animator, just set speed parameter
        if (animator != null)
        {
            // Try to set Speed parameter (for transitions between idle/run)
            try 
            {
                animator.SetFloat("Speed", movement.magnitude);
            }
            catch 
            {
                // If Speed parameter doesn't exist, just leave animator running normally
                // The animator will use its default state
            }
        }

        // Camera toggle with F5
        if (Input.GetKeyDown(KeyCode.F5))
        {
            isThirdPerson = !isThirdPerson;
        }

        // Smooth camera transition
        Vector3 targetOffset = isThirdPerson ? thirdPersonOffset : firstPersonOffset;
        playerCamera.localPosition = Vector3.Lerp(
            playerCamera.localPosition,
            targetOffset,
            Time.deltaTime * cameraSwitchSpeed
        );

        // ESC to unlock cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void FixedUpdate()
    {
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (movement.magnitude > 0.1f)
        {
            // Calculate target velocity
            Vector3 targetVelocity = movement * maxSpeed;
            
            // Calculate velocity change needed
            Vector3 velocityChange = targetVelocity - horizontalVelocity;
            
            // Apply force more gently (back to Force mode but with better calculation)
            rb.AddForce(velocityChange * moveSpeed, ForceMode.Force);

            // Hard cap at max speed
            if (horizontalVelocity.magnitude > maxSpeed)
            {
                horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
                rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
            }
        }
        else
        {
            // Apply stopping force
            Vector3 stopForceVector = -horizontalVelocity * stopForce;
            rb.AddForce(stopForceVector, ForceMode.Force);

            // Additional damping for quick stop
            rb.velocity = new Vector3(
                rb.velocity.x * 0.8f,
                rb.velocity.y,
                rb.velocity.z * 0.8f
            );

            if (horizontalVelocity.magnitude < 0.1f)
            {
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }
        }
    }
}