using UnityEngine;
using Unity.Cinemachine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private CinemachineCamera freeLookCamera; // Correct camera reference
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;

    private Rigidbody rb;
    private bool doubleJumpUsed;
    private LayerMask groundLayer = 1 << 3;

    private void Start()
    {
        // Ensure InputManager is assigned
        if (inputManager == null)
        {
            inputManager = FindObjectOfType<InputManager>();
            if (inputManager == null)
            {
                Debug.LogError("InputManager not found! Assign it in the Inspector.");
                return;
            }
        }

        // Ensure Cinemachine Camera is assigned
        if (freeLookCamera == null)
        {
            freeLookCamera = FindObjectOfType<CinemachineCamera>(); // Auto-find camera
        }

        if (freeLookCamera == null)
        {
            Debug.LogError("CinemachineCamera not found! Assign it in the Inspector.");
            return;
        }

        // Subscribe to movement and jumping events
        inputManager.OnMove.AddListener(MovePlayer);
        inputManager.OnJumpPressed.AddListener(Jump);

        rb = GetComponent<Rigidbody>();
    }

    private void MovePlayer(Vector2 direction)
    {
        if (freeLookCamera == null) return; // Ensure the camera is assigned

        // Set player forward direction to match the camera’s forward direction
        transform.forward = freeLookCamera.transform.forward;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); // Keep rotation level

        // Get movement direction based on camera forward
        Vector3 moveDirection = transform.forward * direction.y + freeLookCamera.transform.right * direction.x;
        moveDirection.y = 0; // Prevent vertical movement

        // Apply movement force
        rb.AddForce(speed * moveDirection.normalized, ForceMode.Force);
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Reset Y velocity for a clean jump
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
