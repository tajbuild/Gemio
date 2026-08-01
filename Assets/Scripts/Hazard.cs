using UnityEngine;
using UnityEngine.SceneManagement;

public class Hazard : MonoBehaviour
{
    [SerializeField] private AudioClip deathSound; // Optional: Drag a sound here
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object entering the trigger is the player
        if (collision.CompareTag("Player"))
        {
            // Optional: Play a death sound right before the game freezes
            if (deathSound != null)
            {
                // Stop the Background Music cleanly
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.StopMusic();
                }
                
                AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, 1f);
            }
            
            // Tell the GameManager to handle the death state
            GameManager.Instance.TriggerGameOver();
            /*
            // If the level is already beaten, ignore the hazard completely!
            if (LevelGoal.isLevelComplete) return;
            // Reload the currently active scene to restart the level
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            */
        }
    }
}