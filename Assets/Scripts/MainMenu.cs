using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Make sure your first level's exact name is typed here
    [SerializeField] private string firstLevelName = "Level_01"; 

    private void Start()
    {
        // Forces the application to run at 60 FPS on mobile devices
        Application.targetFrameRate = 60;
    }
    public void StartGame()
    {
        RunState.StartNewRun();
        // Reset time scale just in case coming from a paused state
        Time.timeScale = 1f; 
        SceneManager.LoadScene(firstLevelName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game Initiated!");
        
        // This will only close the game when playing a built application (.exe / .apk)
        Application.Quit();
    }
}