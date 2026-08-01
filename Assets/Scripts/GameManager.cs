using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // This allows any other script to easily find the GameManager instance
    public static GameManager Instance { get; private set; }

    [Header("Score Tracking")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;

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
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "POINTS: " + score.ToString();
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
}