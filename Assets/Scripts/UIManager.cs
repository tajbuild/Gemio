using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject gameOverPanel;

    private void Start()
    {
        // Ensure the panel is off when the level loads
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void ShowGameOverScreen()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        // Reset time to normal just in case we paused the game upon death
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        // Reset time and load the menu (assumes your menu scene is named "MainMenu")
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); 
    }
}