using UnityEngine;
using UnityEngine.SceneManagement;

public class Hazard : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object entering the trigger is the player
        if (collision.CompareTag("Player"))
        {
            // Reload the currently active scene to restart the level
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}