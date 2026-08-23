using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // This allows any other script to easily find the GameManager instance
    public static GameManager Instance { get; private set; }

    [Header("Score Tracking")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Dependencies")]
    [SerializeField] private UIManager uiManager; // Link this in the Inspector

    private void Awake()
    {
        // Simple Singleton pattern: Ensure there is only ever one GameManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        RunState.BeginLevel();
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        RunState.AddScore(amount);
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "POINTS: " + RunState.Score;
        }
    }

    // New Game Over Logic
    public void TriggerGameOver()
    {
        Debug.Log("Game Over Triggered!");
        
        // Freeze the game physics
        Time.timeScale = 0f; 

        // Tell the UI layer to show the screen
        if (uiManager != null)
        {
            uiManager.ShowGameOverScreen();
        }
    }

    // New pass-through method for the UI
    public void EnableDoubleJumpUI()
    {
        if (uiManager != null)
        {
            uiManager.EnableDoubleJumpIcon();
        }
    }

    public void EnableFireButtonUI()
    {
        if (uiManager != null)
        {
            uiManager.EnableFireButton();
        }
    }
    
    public void UpdateHealthUI(int currentHealth)
    {
        if (uiManager != null)
        {
            uiManager.UpdateHealthUI(currentHealth);
        }
    }
}