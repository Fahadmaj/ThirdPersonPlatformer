using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f; // Adjustable jump force

    private Rigidbody rb;
    private bool doubleJumpUsed; // Tracks if double jump has been used
    private LayerMask groundLayer = 1 << 3; // Layer 3 for Ground

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

        // Subscribe to movement and jumping events
        inputManager.OnMove.AddListener(MovePlayer);
        inputManager.OnJumpPressed.AddListener(Jump);

        rb = GetComponent<Rigidbody>();
    }

    private void MovePlayer(Vector2 direction)
    {
        Vector3 moveDirection = new Vector3(direction.x, 0f, direction.y);
        rb.AddForce(speed * moveDirection, ForceMode.Force);
    }

    private void Jump()
    {
        if (IsGrounded()) // First Jump (resets double jump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            doubleJumpUsed = false; // Reset double jump when touching the ground
        }
        else if (!doubleJumpUsed) // Allow one extra jump mid-air
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            doubleJumpUsed = true; // Mark that double jump is used
        }
    }

    private bool IsGrounded()
    {
        // Raycast to check if the player is touching the ground
        bool grounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);

        // Debugging log to check if the raycast is detecting the ground
        Debug.DrawRay(transform.position, Vector3.down * 1.1f, Color.red);
        Debug.Log("Grounded: " + grounded);

        if (grounded)
        {
            doubleJumpUsed = false; // Reset double jump when grounded
        }

        return grounded;
    }
}
