using UnityEngine;
using Unity.Cinemachine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private CinemachineCamera freeLookCamera;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float dashSpeed = 20f; // Dash speed
    [SerializeField] private float dashDuration = 0.2f; // Dash time
    [SerializeField] private float dashCooldown = 1f; // Time before dashing again

    private Rigidbody rb;
    private bool doubleJumpUsed;
    private bool isDashing = false;
    private float lastDashTime = -100f; // Ensures we can dash immediately
    private LayerMask groundLayer = 1 << 3;

    private void Start()
    {
        if (inputManager == null)
        {
            inputManager = FindObjectOfType<InputManager>();
            if (inputManager == null)
            {
                Debug.LogError("InputManager not found! Assign it in the Inspector.");
                return;
            }
        }

        if (freeLookCamera == null)
        {
            freeLookCamera = FindObjectOfType<CinemachineCamera>();
        }

        if (freeLookCamera == null)
        {
            Debug.LogError("CinemachineCamera not found! Assign it in the Inspector.");
            return;
        }

        inputManager.OnMove.AddListener(MovePlayer);
        inputManager.OnJumpPressed.AddListener(Jump);
        inputManager.OnDashPressed.AddListener(Dash);


        rb = GetComponent<Rigidbody>();
    }

    private void MovePlayer(Vector2 direction)
    {
        if (freeLookCamera == null || isDashing) return;

        transform.forward = freeLookCamera.transform.forward;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        Vector3 moveDirection = transform.forward * direction.y + freeLookCamera.transform.right * direction.x;
        moveDirection.y = 0;

        rb.AddForce(speed * moveDirection.normalized, ForceMode.Force);
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            doubleJumpUsed = false;
        }
        else if (!doubleJumpUsed)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            doubleJumpUsed = true;
        }
    }

    private void Dash()
    {
        if (Time.time < lastDashTime + dashCooldown || isDashing) return; // Dash cooldown check

        lastDashTime = Time.time;
        isDashing = true;

        Vector3 dashDirection = transform.forward; // Dash in the player's forward direction
        rb.linearVelocity = dashDirection * dashSpeed; // Set dash velocity

        Invoke(nameof(StopDash), dashDuration); // Stop dash after duration
    }

    private void StopDash()
    {
        isDashing = false;
        rb.linearVelocity = Vector3.zero; // Stop movement after dashing
    }

    private bool IsGrounded()
    {
        bool grounded = Physics.Raycast(transform.position, Vector3.down, 1.2f, groundLayer);

        Debug.DrawRay(transform.position, Vector3.down * 1.2f, Color.red);
        Debug.Log("Grounded: " + grounded);

        if (grounded)
        {
            doubleJumpUsed = false;
        }

        return grounded;
    }
}
