using System.Collections; // Required for Coroutines
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour
{
    [Header("Level Progression")]
    [SerializeField] private string nextSceneName = "Level_02";
    [SerializeField] private float transitionDelay = 4f; // Adjust this to match your animation length

    [SerializeField] private AudioClip pickupSound; // Drag your sound here in the Inspector


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
            
            // 1. Stop the Background Music
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.StopMusic();
            }
            
            // Activate the global win state!
            isLevelComplete = true;
            
            // Play Pickup Sound 
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position, 0.7f);            
            }

            // Fire the animation
            if (anim != null)
            {
                anim.SetTrigger("Activate");
            }

            Debug.Log("Level Complete! Delaying load for animation...");
            
            // Start the delay timer before loading the scene
            StartCoroutine(LoadNextLevelDelayed());
        }
    }

    // The Coroutine that waits, then loads the scene
    private IEnumerator LoadNextLevelDelayed()
    {
        // Wait for the specified number of seconds
        yield return new WaitForSeconds(transitionDelay);
        
        // Load the next level
        SceneManager.LoadScene(nextSceneName);
    }
}