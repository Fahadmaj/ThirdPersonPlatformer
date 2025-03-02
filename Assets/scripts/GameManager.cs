using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0; // Use int since scores are whole numbers

    private void Start()
    {
        UpdateScoreText(); // Initialize score display
    }

    public void IncrementScore()
    {
        score++;
        UpdateScoreText();
        Debug.Log($"Score updated: {score}");
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    public void ResetScore() // Optional: Call this to reset score if needed
    {
        score = 0;
        UpdateScoreText();
    }
}
