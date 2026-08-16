using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI levelText;

    // Pause button/panel implementation:
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseButton;
    private bool isPaused;
    private bool isGameOver;

    [Header("Powerups")]
    [SerializeField] private GameObject doubleJumpIcon; // Drag the Image here in the Inspector

    [Header("Health")]
    [SerializeField] private GameObject[] healthIcons;



    private void Awake()
    {
        if (gameOverPanel == null)
        {
            Debug.LogError("UIManager: Game Over Panel is not assigned.", this);
        }

        if (pausePanel == null)
        {
            Debug.LogError("UIManager: Pause Panel is not assigned.", this);
        }

        if (pauseButton == null)
        {
            Debug.LogError("UIManager: Pause Button is not assigned.", this);
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

        pausePanel.SetActive(false);
        pauseButton.SetActive(true);

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

    private void Update()
    {
        if (LevelGoal.isLevelComplete)
        {
            pauseButton.SetActive(false);
            return;
        }
    }

    public void ShowGameOverScreen()
    {
        isGameOver = true;
        isPaused = false;
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (pauseButton != null)
        {
            pauseButton.SetActive(false);
        }

        if (gameOverPanel != null)         
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        // Reset time to normal just in case we paused the game upon death
        RestoreGameTime();
        RunState.RestoreLevelStartScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        // Reset time and load the menu (assumes your menu scene is named "MainMenu")
        RestoreGameTime();
        SceneManager.LoadScene("MainMenu"); 
    }

    public void EnableDoubleJumpIcon()
    {
        if (doubleJumpIcon != null)
        {
            doubleJumpIcon.SetActive(true);
        }
    }

    public void TogglePause()
    {
        if (isGameOver || LevelGoal.isLevelComplete)
        {
            return;
        }

        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (isPaused || isGameOver || LevelGoal.isLevelComplete)
        {
            return;
        }

        isPaused = true;
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);

        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public void ResumeGame()
    {
        if (!isPaused)
        {
            return;
        }

        isPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;

        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
    }

    private void RestoreGameTime()
    {
        isPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void UpdateHealthUI(int currentHealth)
    {
        if (healthIcons == null)
        {
            return;
        }

        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (healthIcons[i] != null)
            {
                healthIcons[i].SetActive(i < currentHealth);
            }
        }
    }
}