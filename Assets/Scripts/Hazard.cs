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
            
            // If the level is already beaten, ignore the hazard completely!
            if (LevelGoal.isLevelComplete) return;

            // Optional: Play a death sound right before the game freezes
            // Stop the Background Music cleanly
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.StopMusic();
            }
            
            if (deathSound != null)
            {
                AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, 1f);
            }
            
            // Tell the GameManager to handle the death state
            GameManager.Instance.TriggerGameOver();

        }
    }
}