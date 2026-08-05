using System.Collections; // Required for Coroutines
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour
{
    [Header("Level Progression")]

    [SerializeField] private float transitionDelay = 4f; // Adjust this to match your animation length

    [SerializeField] private AudioClip winSound; // Drag your sound here in the Inspector


    // This static bool can be read by any other script instantly
    public static bool isLevelComplete = false;

    private Animator anim;
    private bool isTriggered = false; // Prevents triggering multiple times

    void Awake()
    {
        anim = GetComponent<Animator>();

        // Reset the win state every time a level starts
        isLevelComplete = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true; // Lock the trigger so it only fires once
             // Activate the global win state!
            isLevelComplete = true;

            // 1. Stop the Background Music
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.StopMusic();
            }           
            
            // Play Win Sound 
            if (winSound != null)
            {
                AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position, 0.7f);            
            }

            // Fire the animation
            if (anim != null)
            {
                anim.SetTrigger("Activate");
            }

            // Freeze the player's movement (optional, but highly recommended)
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
                playerRb.simulated = false; // Completely stops physics on the player
            }

            // Start the delay timer before loading the scene
            StartCoroutine(LoadNextLevelDelayed());
        }
    }

    // The Coroutine that waits, then loads the scene
    private IEnumerator LoadNextLevelDelayed()
    {
        // Wait for the specified number of seconds
        yield return new WaitForSeconds(transitionDelay);
        
        // Calculate what the next scene index is based on the Build Settings
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if a next level actually exists in the Build Settings
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // If there are no more levels, send the player back to the Main Menu
            SceneManager.LoadScene("MainMenu");
        }
    }
}