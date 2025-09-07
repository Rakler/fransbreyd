using UnityEngine;
using TMPro;

public class HighScoreManager : MonoBehaviour
{
    public int highScore = 0; // current high score

    private int newScore;

    public ScoreManager scoreManager;

    public TextMeshProUGUI scoreText; // drag your PointsText UI here

    void Start()
    {
        checkHighScore();
        UpdateHighScoreUI();
    }

    public void checkHighScore()
    {
        newScore = scoreManager.getScore();
        if (newScore > highScore)
        {
            highScore = newScore;
        }
    }

    void UpdateHighScoreUI()
    {
        scoreText.text = "High score: " + highScore;
    }
    
}
