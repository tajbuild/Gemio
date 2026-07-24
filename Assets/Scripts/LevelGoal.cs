using System.Collections; // Required for Coroutines
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour
{
    [Header("Level Progression")]
    [SerializeField] private string nextSceneName = "Level_02";
    [SerializeField] private float transitionDelay = 1.5f; // Adjust this to match your animation length

    // This static bool can be read by any other script instantly
    public static bool isLevelComplete = false;

    private Animator anim;
    private bool isTriggered = false; // Prevents triggering multiple times

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true; // Lock the trigger so it only fires once
            
            // Activate the global win state!
            isLevelComplete = true;
            
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