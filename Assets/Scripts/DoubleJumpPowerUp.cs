using UnityEngine;

public class DoubleJumpPowerUp : MonoBehaviour
{
    
    [SerializeField] private AudioClip pickupSound; // Drag your sound here in the Inspector

    private void Start()
    {
        // If this upgrade was already collected before a retry or scene reload,
        // remove the duplicate power-up.
        if (RunState.HasDoubleJumpUnlocked)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            
            if (player != null)
            {
                // Save the upgrade for the remainder of the run.
                RunState.UnlockDoubleJump();
                // Unlock it immediately on the current Player.
                player.hasDoubleJumpUnlocked = true;
                
                // Play Pickup Sound 
                if (pickupSound != null)
                {
                    AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position, 1f);            
                }

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.EnableDoubleJumpUI();    
                }
                
                // Destroy the power-up object
                Destroy(gameObject);
            }
        }
    }
}