using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Powerups")]
    [SerializeField] private GameObject doubleJumpIcon; // Drag the Image here in the Inspector

    private GameObject pausePanel;
    private GameObject pauseButton;
    private bool isPaused;
    private bool isGameOver;

    private void Awake()
    {
        BuildPauseUI();
    }

    private void Start()
    {
        // A level should always begin at normal speed, including when opened directly in the Editor.
        Time.timeScale = 1f;
        AudioListener.pause = false;

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

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (LevelGoal.isLevelComplete)
        {
            if (pauseButton != null)
            {
                pauseButton.SetActive(false);
            }

            return;
        }

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
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
        if (isPaused || isGameOver || LevelGoal.isLevelComplete || pausePanel == null)
        {
            return;
        }

        isPaused = true;
        pausePanel.transform.SetAsLastSibling();
        pausePanel.SetActive(true);

        if (pauseButton != null)
        {
            pauseButton.SetActive(false);
        }

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

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (pauseButton != null && !isGameOver && !LevelGoal.isLevelComplete)
        {
            pauseButton.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        RestoreTimeScale();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        RestoreTimeScale();
        SceneManager.LoadScene("MainMenu"); 
    }

    public void EnableDoubleJumpIcon()
    {
        if (doubleJumpIcon != null)
        {
            doubleJumpIcon.SetActive(true);
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            PauseGame();
        }
    }

    private void OnDestroy()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }
    }

    private void RestoreTimeScale()
    {
        isPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    private void BuildPauseUI()
    {
        if (gameOverPanel == null)
        {
            Debug.LogError("UIManager cannot create the pause menu because Game Over Panel is not assigned.", this);
            return;
        }

        pausePanel = Instantiate(gameOverPanel, gameOverPanel.transform.parent);
        pausePanel.name = "PausePanel";
        pausePanel.SetActive(false);

        TextMeshProUGUI title = pausePanel.transform.Find("Text (TMP)")?.GetComponent<TextMeshProUGUI>();
        Button resumeButton = pausePanel.transform.Find("RetryButton")?.GetComponent<Button>();
        Button menuButton = pausePanel.transform.Find("MenuButton")?.GetComponent<Button>();

        if (title == null || resumeButton == null || menuButton == null)
        {
            Debug.LogError("UIManager could not find the expected Game Over Panel controls.", this);
            Destroy(pausePanel);
            pausePanel = null;
            return;
        }

        title.text = "PAUSED";
        resumeButton.gameObject.name = "ResumeButton";
        SetButtonLabel(resumeButton, "RESUME");
        SetButtonAction(resumeButton, ResumeGame);
        SetButtonAction(menuButton, LoadMainMenu);

        pauseButton = Instantiate(resumeButton.gameObject, transform);
        pauseButton.name = "PauseButton";
        SetButtonLabel(pauseButton.GetComponent<Button>(), "PAUSE");
        SetButtonAction(pauseButton.GetComponent<Button>(), TogglePause);

        RectTransform pauseButtonRect = pauseButton.GetComponent<RectTransform>();
        pauseButtonRect.anchorMin = Vector2.one;
        pauseButtonRect.anchorMax = Vector2.one;
        pauseButtonRect.pivot = Vector2.one;
        pauseButtonRect.anchoredPosition = new Vector2(-40f, -40f);
        pauseButtonRect.sizeDelta = new Vector2(180f, 70f);
        pauseButton.SetActive(true);
    }

    private static void SetButtonAction(Button button, UnityEngine.Events.UnityAction action)
    {
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(action);
    }

    private static void SetButtonLabel(Button button, string label)
    {
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>(true);
        if (buttonText != null)
        {
            buttonText.text = label;
        }
    }
}
