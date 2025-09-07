using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public int score = 0; // current score
    public TextMeshProUGUI scoreText; // drag your PointsText UI here

    public UnityEvent onScoreIncrease;

    void Start()
    {
        onScoreIncrease.Invoke();
        UpdatePointsUI();
    }

    // Call this method to add points
    public void AddPoints(int amount)
    {
        score += amount;
        UpdatePointsUI();
        onScoreIncrease.Invoke();
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

    public int getScore()
    {
        return score;
    }

    public void IncrementScore()
    {
        score++;
        UpdatePointsUI();
    }
}
