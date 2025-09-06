using UnityEngine;
using UnityEngine.SceneManagement; // needed for restarting scenes

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverUI; // Assign your GameOver Canvas here

    private void Start()
    {
        // Make sure Game Over menu is hidden at start
        gameOverUI.SetActive(true);
    }

    // Call this when the game ends (for example from your CountdownTimer)
    public void ShowGameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f; // freeze the game
    }

    public void Restart()
    {
        Time.timeScale = 1f; // unfreeze
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reload current scene
    }

    public void QuitGame(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
