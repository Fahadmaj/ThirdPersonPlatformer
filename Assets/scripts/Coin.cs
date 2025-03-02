using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f; // Adjustable in Inspector

    void Update()
    {
        // Rotate the coin smoothly around the Y-axis
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
    }
}
