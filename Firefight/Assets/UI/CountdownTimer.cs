using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float timeRemaining = 300f; // 5 minutes
    public bool timerIsRunning = false;
    public TextMeshProUGUI timerText; 
    public GameObject gameOverScreen; // assign in Inspector
    SceneLoader sceneLoader;

    private void Start()
    {
        timerIsRunning = true;
        sceneLoader = new SceneLoader();
        // gameOverScreen.SetActive(false); // make sure it starts hidden
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                DisplayTime(timeRemaining);
                TriggerGameOver();
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
            timeToDisplay = 0;

        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TriggerGameOver()
    {
        Debug.Log("⏰ Game Over!");
        sceneLoader.LoadScene("GameOver");
        // gameOverScreen.SetActive(true); // show game over UI
        // You can also pause the game here if needed:
        // Time.timeScale = 0f;
    }
}
