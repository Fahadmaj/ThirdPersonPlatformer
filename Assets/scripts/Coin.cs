using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;

    void Update()
    {
        // Rotate the coin smoothly
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player touches the coin
        {
            FindObjectOfType<GameManager>().IncrementScore(); // Increase score
            Destroy(gameObject); // Remove the coin
        }
    }
}
