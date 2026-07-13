using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour
{
    [Header("Level Progression")]
    [SerializeField] private string nextSceneName = "Level_02";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Level Complete! Loading: " + nextSceneName);
            
            // Load the next level scene by its name string
            SceneManager.LoadScene(nextSceneName);
        }
    }
}