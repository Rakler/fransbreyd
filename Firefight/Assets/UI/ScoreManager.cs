using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score = 0; // current points
    public TextMeshProUGUI scoreText; // drag your PointsText UI here

    void Start()
    {
        UpdatePointsUI();
    }

    // Call this method to add points
    public void AddPoints(int amount)
    {
        score += amount;
        UpdatePointsUI();
    }

    // Call this method to subtract points if needed
    public void SubtractPoints(int amount)
    {
        score -= amount;
        if (score < 0) score = 0;
        UpdatePointsUI();
    }

    void UpdatePointsUI()
    {
        scoreText.text = "Score: " + score;
    }
}
