using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Start()
    {
        // Grabs the exact name of your scene file (e.g., "Level_01")
        string sceneName = SceneManager.GetActiveScene().name;
        
        // Optional: Replace the underscore with a space so it reads "Level 01"
        if (levelText != null)
        {
            levelText.text = sceneName.Replace("_", " ");
        }
        
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